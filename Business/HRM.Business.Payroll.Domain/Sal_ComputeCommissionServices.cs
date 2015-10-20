using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Threading;
using SystemThread = System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Insurance.Models;
using HRM.Business.Evaluation.Models;
using VnResource.Helper;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Domain;
using System.Reflection;
using VnResource.Helper.Setting;
using VnResource.Helper.Linq;
using System.Threading.Tasks;
using VnResource.Helper.Ado;
using VnResource.AdoHelper;
using HRM.Business.Canteen.Models;


namespace HRM.Business.Payroll.Domain
{
    public class Sal_ComputeCommissionServices : BaseService
    {
        Sal_ComputePayrollServices PayrollServices = new Sal_ComputePayrollServices();
        private bool DeletePayrollTable(List<Guid> listProfileID, Guid cutOffDurationID)
        {
            #region Delete PayrollTable

            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = new UnitOfWork(context);
                    int pageSize = 2000;//tối đa là 2100 parameter
                    int result = 0;

                    foreach (var listProfileIDBySize in listProfileID.Chunk(pageSize))
                    {
                        result += unitOfWork.SetIsDelete(context.Sal_PayCommission.Where(m => m.CutoffDurationID != null && m.ProfileID != null && !m.IsDelete.HasValue && (Guid)m.CutoffDurationID == cutOffDurationID && listProfileIDBySize.Contains((Guid)m.ProfileID)));
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            #endregion
        }
        public Guid ComputeCommission(List<Hre_ProfileEntity> listProfile, Att_CutOffDurationEntity CutOffDuration, string HeaderTemplateLog, string methodPayroll, Guid CutOffDuration2ID, string UserLogin)
        {
            Sys_AsynTask asynTask = new Sys_AsynTask()
            {
                ID = Guid.NewGuid(),
                Summary = "Tính Hoa Hồng Cho Tháng " + CutOffDuration.CutOffDurationName,
                Type = AsynTask.Payroll_Computing.ToString(),
                Status = AsynTaskStatus.Doing.ToString(),
                TimeStart = DateTime.Now,
                PercentComplete = 0.01,
            };

            Task.Run(() =>
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);

                    if (listProfile.Count == 0)
                    {
                        asynTask.TimeEnd = DateTime.Now;
                        asynTask.Description = "Không tồn tại nhân viên nào !";
                        asynTask.PercentComplete = 1D;
                    }

                    repoSys_AsynTask.Add(asynTask);
                    unitOfWork.SaveChanges();
                }

                if (listProfile.Count > 0)
                {
                    ComputeCommission(listProfile, CutOffDuration,
                        asynTask.ID, HeaderTemplateLog, methodPayroll, CutOffDuration2ID, UserLogin);
                }
            });

            return asynTask.ID;
        }

        public void ComputeCommission(List<Hre_ProfileEntity> listProfile, Att_CutOffDurationEntity CutOffDuration, Guid asynTaskID, string HeaderTemplateLog, string methodPayroll, Guid CutOffDuration2ID, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoPayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var Sys_Model = repoSys_AsynTask.FindBy(m => m.ID == asynTaskID).FirstOrDefault();

                if (!DeletePayrollTable(listProfile.Select(d => d.ID).ToList(), CutOffDuration.ID))
                {
                    Sys_Model.Status = AsynTaskStatus.Error.ToString();
                    Sys_Model.Description = "Không thể xóa bảng lương cũ";
                    Sys_Model.PercentComplete = 1;
                    unitOfWork.SaveChanges();
                }
                else
                {
                    #region Get All Data

                    DateTime DatetimeStart = DateTime.Now;
                    ComputePayrollDataModel TotalData = PayrollServices.GetDataForComputeSalary(CutOffDuration,UserLogin);
                    //FileLog.WriteLog("", "Get Data", Common.ComputeTime(DatetimeStart, DateTime.Now));

                    #endregion

                    //Kiểm tra xem có lỗi store hay không
                    if (TotalData.Status != null && TotalData.Status != string.Empty)
                    {
                        Sys_Model.Status = AsynTaskStatus.Error.ToString();
                        Sys_Model.Description = TotalData.Status;
                        Sys_Model.PercentComplete = 1;
                        unitOfWork.SaveChanges();
                    }
                    else
                    {
                        #region Ghi Log
                        //Lấy đường dẫn thư mục ghi log
                        //set lại thư mục ghi log
                        //WriteLog.SettingPath = Common.GetPath("Log");
                        //WriteLog.DynamicDirectory = "ComputePayroll" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                        //WriteLog.FileName = "LogFile";
                        //WriteLog.WriteLog(HeaderTemplateLog, "", "");
                        #endregion

                        int pageSize = 100;//mỗi lần tính cho 100 nhân viên
                        int totalProfile = listProfile.Count;//Tổng số nhâ viên

                        foreach (var listProfileBySize in listProfile.Chunk(pageSize))
                        {
                            ComputeCommission_Progress(TotalData, listProfileBySize.ToList(), CutOffDuration, Sys_Model.ID, totalProfile, methodPayroll, CutOffDuration2ID);
                        }
                    }
                }
            }
        }

        public void ComputeCommission_Progress(ComputePayrollDataModel TotalData, List<Hre_ProfileEntity> ProfileID, Att_CutOffDurationEntity CutOffDuration, Guid Sys_AsynTaskID, int totalProfile, string methodPayroll, Guid CutOffDuration2ID)
        {
            using (var context = new VnrHrmDataContext())
            {
                TraceLogManager FileLog = new TraceLogManager();
                var unitOfWork = new UnitOfWork(context);

                #region Init Repo
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoHre_profile = new CustomBaseRepository<Hre_ProfileEntity>(unitOfWork);
                var repoCat_Element = new CustomBaseRepository<Cat_ElementEntity>(unitOfWork);
                var repoSal_BasicSalary = new CustomBaseRepository<Sal_BasicSalaryEntity>(unitOfWork);
                //var repoPayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                //var repoPayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);

                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeTypeEntity>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayTypeEntity>(unitOfWork);
                var repoCat_UsualAllowanceEntity = new CustomBaseRepository<Cat_UsualAllowanceEntity>(unitOfWork);
                var repoCat_GradePayroll = new CustomBaseRepository<Cat_GradePayrollEntity>(unitOfWork);
                var repoCat_UnusualAllowanceCfg = new CustomBaseRepository<Cat_UnusualAllowanceCfgEntity>(unitOfWork);
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                #endregion

                //Các biến xử dụng chung
                Sys_AsynTask asynTask = new Sys_AsynTask();

                //[SCV] list lưu tiền khấu nhân viên chưa đủ thâm niên của shop
                Dictionary<Guid, ValueCount> listTmpDeduction = new Dictionary<Guid, ValueCount>();
                List<Sal_PayCommissionItem> listPayrollTableItem = new List<Sal_PayCommissionItem>();
                List<Sal_PayCommission> listPayrollTable = new List<Sal_PayCommission>();

                //Get asynTask
                asynTask = repoSys_AsynTask.FindBy(m => m.ID == Sys_AsynTaskID).FirstOrDefault();

                //Order By theo ngày vào làm để tính trường hợp nhân viên không đủ thâm niên của dự án SCV
                ProfileID = ProfileID.OrderBy(m => m.DateHire).ToList();
                ParallelOptions parallelOptions = new ParallelOptions();

                #region Duyệt Profile

                Parallel.For(0, ProfileID.Count, parallelOptions, d =>
                {
                    var profileItem = ProfileID[d];
                    bool isCancled = false;

                    //Biến tổng lưu tất cả các value của Formula
                    List<ElementFormula> listElementFormula = new List<ElementFormula>();

                    //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                    Sal_GradeEntity Grade = PayrollServices.FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
      
                    //lấy phần tử tính lương theo grade
                    List<Cat_ElementEntity> listElement = TotalData.listElement_All.Where(m => m.GradePayrollID != null && (Guid)m.GradePayrollID == Grade.GradePayrollID && m.MethodPayroll != null && m.MethodPayroll == methodPayroll).ToList();
                   
                    if (!isCancled)
                    {
                        #region tạo mới PayrollTable
                        Sal_PayCommission PayrollTable_Model = new Sal_PayCommission()
                        {
                            ID = Guid.NewGuid(),
                            ProfileID = profileItem.ID,
                            CutoffDurationID = CutOffDuration.ID,
                            MonthYear = CutOffDuration.MonthYear,
                            CutoffDuration2ID = CutOffDuration2ID,
                            //OrgStructureID = profileItem.OrgStructureID,
                            //PositionID = profileItem.PositionID,
                            //JobTitleID = profileItem.JobTitleID,
                            //EmployeeTypeID = profileItem.EmpTypeID,
                            //PayrollGroupID = profileItem.PayrollGroupID,
                            //CostCentreID = profileItem.CostCentreID,
                            //IncomeBeforeTax = 0,
                            //DependantCount = 0,
                            //IncomeTaxable = 0,
                            //AmountPaidPITCom = 0,
                            //AmountPaidPITEmp = 0,
                            //IncomeNET = 0,
                            //BankID = profileItem.BankID,
                            //AccountNo = profileItem.AccountNo,
                        };

                        lock (listPayrollTable)
                        {
                            listPayrollTable.Add(PayrollTable_Model);
                        }

                        #endregion

                        //bắt lỗi tính công thức có giá trị nào null hay không
                        try
                        {
                            listElementFormula = PayrollServices.ParseElementFormula(listElementFormula, listElement, TotalData, profileItem, CutOffDuration, listTmpDeduction,false, FileLog);

                            listElement = listElement.OrderBy(m => m.OrderNumber).ToList();//sắp xếp lại

                            //Duyệt qua các phần tử tính lương 
                            foreach (var elementItem in listElement)
                            {
                                Sal_PayCommissionItem tableItem = new Sal_PayCommissionItem();
                                tableItem.ID = Guid.NewGuid();
                                tableItem.PayCommissionID = PayrollTable_Model.ID;
                                tableItem.Name = elementItem.ElementName;
                                tableItem.Code = elementItem.ElementCode;
                                tableItem.OrderNo = elementItem.OrderNumber != null ? (int)elementItem.OrderNumber : 0;
                                tableItem.ElementType = elementItem.TabType;
                                tableItem.ValueType = elementItem.Type;
                                tableItem.IsShow = elementItem.Invisible != null ? !elementItem.Invisible : true;
                                if (elementItem.IsBold == true)
                                {
                                    tableItem.Description += "E_BOLD,";
                                }
                                tableItem.Description += elementItem.ElementLevel + "," + elementItem.Type;
                                var ElementResult = listElementFormula.Where(m => m.VariableName.Trim() == elementItem.ElementCode.Trim()).FirstOrDefault();
                                if (ElementResult != null)
                                {
                                    tableItem.Value = ElementResult.Value.ToString();
                                    tableItem.Description = ElementResult.ErrorMessage;
                                }
                                else
                                {
                                    tableItem.Value = "0";
                                    tableItem.Description = "Không Tìm Thấy Phần Tử !";
                                }

                                lock (listPayrollTableItem)
                                {
                                    listPayrollTableItem.Add(tableItem);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Sal_PayCommissionItem tableItem = new Sal_PayCommissionItem();
                            tableItem.ID = Guid.NewGuid();
                            tableItem.PayCommissionID = PayrollTable_Model.ID;
                            tableItem.Name = "Lỗi, không thể tính được phần tử " + ex.Message;
                            tableItem.Code = "Error";
                            tableItem.OrderNo = 0;
                            tableItem.ElementType = "Payroll";
                            tableItem.ValueType = "Double";
                            tableItem.Value = "0";
                            tableItem.Description = ex.Message;

                            lock (listPayrollTableItem)
                            {
                                listPayrollTableItem.Add(tableItem);
                            }
                        }
                    }
                });

                #endregion

                asynTask = repoSys_AsynTask.FindBy(m => m.ID == Sys_AsynTaskID).FirstOrDefault();
                asynTask.PercentComplete += ((double)ProfileID.Count / (double)totalProfile);
                asynTask.Description = FileLog.GetFullPath(FileLog.FileName);
                asynTask.TimeEnd = DateTime.Now;

                if (asynTask.PercentComplete >= 1)
                {
                    asynTask.Status = AsynTaskStatus.Done.ToString();
                    asynTask.PercentComplete = 1D;
                }

                var connection = context.Database.Connection.GetAdoConnection();

                using (DbCommander commander = new DbCommander(connection))
                {
                    if (connection.IsSqlConnection())
                    {
                        var bulkCopyHelper = new SqlBulkCopyHelper(connection.ConnectionString);
                        var dtPayrollTable = commander.GetSchema("Columns", new string[] { null, null, typeof(Sal_PayCommission).Name });
                        var dtPayrollTableItem = commander.GetSchema("Columns", new string[] { null, null, typeof(Sal_PayCommissionItem).Name });
                        var payrollTableFields = dtPayrollTable.Rows.OfType<System.Data.DataRow>().Select(d => d["Column_Name"].GetString()).ToArray();
                        var payrollTableItemFields = dtPayrollTableItem.Rows.OfType<System.Data.DataRow>().Select(d => d["Column_Name"].GetString()).ToArray();
                        bulkCopyHelper.WriteToServer(listPayrollTable, typeof(Sal_PayCommission).Name, payrollTableFields);

                        foreach (var listPayrollTableItemBySize in listPayrollTableItem.Chunk(2000))
                        {
                            bulkCopyHelper.WriteToServer(listPayrollTableItemBySize.ToList(),
                                typeof(Sal_PayCommissionItem).Name, payrollTableItemFields);
                        }
                    }
                    else
                    {
                        var dtPayrollTable = commander.GetSchema("Columns", new string[] { null, typeof(Sal_PayCommission).Name });
                        var dtPayrollTableItem = commander.GetSchema("Columns", new string[] { null, typeof(Sal_PayCommissionItem).Name });
                        var payrollTableFields = dtPayrollTable.Rows.OfType<System.Data.DataRow>().Select(d => d["Name"].GetString()).ToArray();
                        var payrollTableItemFields = dtPayrollTableItem.Rows.OfType<System.Data.DataRow>().Select(d => d["Name"].GetString()).ToArray();

                        commander.InsertList(typeof(Sal_PayCommission).Name, listPayrollTable, payrollTableFields);

                        foreach (var listPayrollTableItemBySize in listPayrollTableItem.Chunk(2000))
                        {
                            commander.InsertList(typeof(Sal_PayCommissionItem).Name, listPayrollTableItemBySize.ToList(), payrollTableItemFields);
                        }
                    }
                }

                //repoPayrollTableItem.Add(listPayrollTableItem);
                //repoPayrollTable.Add(listPayrollTable);
                unitOfWork.SaveChanges();
            }
        }
    }
}
