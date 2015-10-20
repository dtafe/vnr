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
    public class Sal_ComputePayrollServices : BaseService
    {
        private bool DeletePayrollTable(List<Guid> listProfileID, Guid cutOffDurationID, bool Settlement, TraceLogManager FileLog)
        {
            #region Delete PayrollTable

            try
            {
                if (!Settlement)
                {
                    using (var context = new VnrHrmDataContext())
                    {
                        var unitOfWork = new UnitOfWork(context);
                        int pageSize = 2000;//tối đa là 2100 parameter
                        int result = 0;
                        DateTime DatetimeStart = DateTime.Now;

                        foreach (var listProfileIDBySize in listProfileID.Chunk(pageSize))
                        {
                            result += unitOfWork.SetIsDelete(context.Sal_PayrollTable.Where(d => !d.IsDelete.HasValue
                                && d.CutOffDurationID == cutOffDurationID && listProfileIDBySize.Contains(d.ProfileID)));
                        }
                        FileLog.WriteLog("", "Delete Old Data Payroll", Common.ComputeTime(DatetimeStart, DateTime.Now));
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            #endregion
        }

        public Guid ComputePayroll(List<Hre_ProfileEntity> listProfile, Att_CutOffDurationEntity CutOffDuration, string HeaderTemplateLog, string UserLogin, bool Settlement = false)
        {
            Sys_AsynTask asynTask = new Sys_AsynTask()
            {
                ID = Guid.NewGuid(),
                Summary = "Tính Lương Cho Tháng " + CutOffDuration.CutOffDurationName,
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
                    ComputePayroll(listProfile, CutOffDuration,
                        asynTask.ID, HeaderTemplateLog, UserLogin, Settlement);
                }
            });

            return asynTask.ID;
        }

        public void ComputePayroll(List<Hre_ProfileEntity> listProfile, Att_CutOffDurationEntity CutOffDuration, Guid asynTaskID, string HeaderTemplateLog, string UserLogin, bool Settlement = false)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoPayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var Sys_Model = repoSys_AsynTask.FindBy(m => m.ID == asynTaskID).FirstOrDefault();

                #region Ghi Log
                TraceLogManager FileLog = new TraceLogManager();
                //Lấy đường dẫn thư mục ghi log
                //set lại thư mục ghi log
                FileLog.SettingPath = Common.GetPath("Log");
                FileLog.DynamicDirectory = "ComputePayroll" + DateTime.Now.ToString("ddMMyyyy-HHmmss");
                FileLog.FileName = "LogFile";
                FileLog.WriteLog(HeaderTemplateLog, "", "");
                #endregion

                if (!DeletePayrollTable(listProfile.Select(d => d.ID).ToList(), CutOffDuration.ID, Settlement, FileLog))
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
                    ComputePayrollDataModel TotalData = GetDataForComputeSalary(CutOffDuration, UserLogin);
                    FileLog.WriteLog("", "Get All Data", Common.ComputeTime(DatetimeStart, DateTime.Now));

                    #endregion

                    //Kiểm tra xem có lỗi store hay không
                    if (TotalData.Status != null && TotalData.Status != string.Empty)
                    {
                        Sys_Model.Status = AsynTaskStatus.Error.ToString();
                        Sys_Model.Description = TotalData.Status;
                        Sys_Model.PercentComplete = 1;
                        FileLog.WriteLog("", "Store Error " + TotalData.Status);
                        unitOfWork.SaveChanges();
                    }
                    else
                    {
                        int pageSize = 100;//mỗi lần tính cho 100 nhân viên
                        int totalProfile = listProfile.Count;//Tổng số nhâ viên

                        Sys_AttOvertimePermitConfigServices ConfigServices = new Sys_AttOvertimePermitConfigServices();
                        bool ComputeOrderNumber = ConfigServices.GetConfigValue<bool>(AppConfig.HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER);

                        foreach (var listProfileBySize in listProfile.Chunk(pageSize))
                        {
                            ComputePayroll_Progress(TotalData, listProfileBySize.ToList(), CutOffDuration, Sys_Model.ID, totalProfile, FileLog, ComputeOrderNumber, Settlement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compute Payroll New
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="CutOffDuration"></param>
        /// <param name="Sys_AsynTaskID"></param>
        public void ComputePayroll_Progress(ComputePayrollDataModel TotalData, List<Hre_ProfileEntity> ProfileID, Att_CutOffDurationEntity CutOffDuration, Guid Sys_AsynTaskID, int totalProfile, TraceLogManager FileLog, bool ComputeOrderNumber, bool Settlement = false)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = new UnitOfWork(context);
                DateTime DatetimeStart = new DateTime();

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
                List<Sal_PayrollTableItem> listPayrollTableItem = new List<Sal_PayrollTableItem>();
                List<Sal_PayrollTable> listPayrollTable = new List<Sal_PayrollTable>();

                //Get asynTask
                asynTask = repoSys_AsynTask.FindBy(m => m.ID == Sys_AsynTaskID).FirstOrDefault();

                //Order By theo ngày vào làm để tính trường hợp nhân viên không đủ thâm niên của dự án SCV
                ProfileID = ProfileID.OrderBy(m => m.DateHire).ToList();
                ParallelOptions parallelOptions = new ParallelOptions();

                #region Duyệt Profile

                //Parallel.For(0, ProfileID.Count, parallelOptions, d =>
                //{

                //});

                for (int i = 0; i < ProfileID.Count; i++)
                {
                    TraceLogItemInfo logItem = new TraceLogItemInfo();
                    DatetimeStart = DateTime.Now;
                    var profileItem = ProfileID[i];
                    bool isCancled = false;

                    //Biến tổng lưu tất cả các value của Formula
                    List<ElementFormula> listElementFormula = new List<ElementFormula>();

                    //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                    Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                    List<Cat_ElementEntity> listElement = new List<Cat_ElementEntity>();
                    if (Grade.GradePayrollID != null && Grade.GradePayrollID != Guid.Empty)
                    {
                        listElement = new List<Cat_ElementEntity>(TotalData.listElement_All.Where(m => (m.GradePayrollID != null && (Guid)m.GradePayrollID == Grade.GradePayrollID) || m.IsApplyGradeAll == true));
                    }

                    //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                    Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();
                    if (CatGrade != null && CatGrade.SalaryTimeTypeClose != null)
                    {
                        DateTime DateClose = new DateTime();
                        if (CatGrade.SalaryTimeTypeClose == SalaryTimeTypeClose.E_CURRENTMONTH.ToString())
                        {
                            DateClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose > 30 && CutOffDuration.MonthYear.Month % 2 == 0 ? 30 : (int)CatGrade.SalaryDayClose : 1);
                        }
                        else if (CatGrade.SalaryTimeTypeClose == SalaryTimeTypeClose.E_LASTMONTH.ToString())
                        {
                            DateClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose > 30 && CutOffDuration.MonthYear.Month % 2 == 0 ? 30 : (int)CatGrade.SalaryDayClose : 1).AddMonths(-1);
                        }

                        if (profileItem.DateHire != null && (DateTime)profileItem.DateHire > DateClose)
                        {
                            isCancled = true;
                            FileLog.WriteLog("NV có ngày vào làm sau ngày chốt lương của tháng", "", "");
                        }
                    }

                    if (!isCancled)
                    {
                        #region tạo mới PayrollTable
                        Sal_PayrollTable PayrollTable_Model = new Sal_PayrollTable();
                        if (Settlement == true)
                        {
                            if (repoSal_PayrollTable.FindBy(m => m.CutOffDurationID == CutOffDuration.ID && m.ProfileID == profileItem.ID).FirstOrDefault() != null)
                            {
                                Att_CutOffDurationEntity CutOff = TotalData.listCutOffDuration.Where(m => m.MonthYear >= CutOffDuration.MonthYear).OrderBy(m => m.MonthYear).FirstOrDefault();
                                PayrollTable_Model = new Sal_PayrollTable()
                                {
                                    ID = Guid.NewGuid(),
                                    ProfileID = profileItem.ID,
                                    CutOffDurationID = CutOff.ID,
                                    MonthYear = CutOff.MonthYear,
                                    OrgStructureID = profileItem.OrgStructureID,
                                    PositionID = profileItem.PositionID,
                                    JobTitleID = profileItem.JobTitleID,
                                    EmployeeTypeID = profileItem.EmpTypeID,
                                    PayrollGroupID = profileItem.PayrollGroupID,
                                    CostCentreID = profileItem.CostCentreID,
                                    IncomeBeforeTax = 0,
                                    IsCash = profileItem.IsCash,
                                    DependantCount = 0,
                                    IncomeTaxable = 0,
                                    AmountPaidPITCom = 0,
                                    AmountPaidPITEmp = 0,
                                    IncomeNET = 0,
                                    BankID = profileItem.BankID,
                                    AccountNo = profileItem.AccountNo,
                                    Status = FileLog.GetFullPath(FileLog.FileName).Replace(".XML", "_" + DateTime.Now.ToString("yyyyMMdd") + ".XML"),
                                };
                            }
                            else
                            {
                                PayrollTable_Model = new Sal_PayrollTable()
                                {
                                    ID = Guid.NewGuid(),
                                    ProfileID = profileItem.ID,
                                    CutOffDurationID = CutOffDuration.ID,
                                    MonthYear = CutOffDuration.MonthYear,
                                    OrgStructureID = profileItem.OrgStructureID,
                                    PositionID = profileItem.PositionID,
                                    JobTitleID = profileItem.JobTitleID,
                                    EmployeeTypeID = profileItem.EmpTypeID,
                                    PayrollGroupID = profileItem.PayrollGroupID,
                                    CostCentreID = profileItem.CostCentreID,
                                    IncomeBeforeTax = 0,
                                    DependantCount = 0,
                                    IncomeTaxable = 0,
                                    AmountPaidPITCom = 0,
                                    AmountPaidPITEmp = 0,
                                    IncomeNET = 0,
                                    BankID = profileItem.BankID,
                                    AccountNo = profileItem.AccountNo,
                                    Status = FileLog.GetFullPath(FileLog.FileName).Replace(".XML", "_" + DateTime.Now.ToString("yyyyMMdd") + ".XML"),
                                };
                            }
                        }
                        else
                        {
                            PayrollTable_Model = new Sal_PayrollTable()
                            {
                                ID = Guid.NewGuid(),
                                ProfileID = profileItem.ID,
                                CutOffDurationID = CutOffDuration.ID,
                                MonthYear = CutOffDuration.MonthYear,
                                OrgStructureID = profileItem.OrgStructureID,
                                PositionID = profileItem.PositionID,
                                JobTitleID = profileItem.JobTitleID,
                                EmployeeTypeID = profileItem.EmpTypeID,
                                PayrollGroupID = profileItem.PayrollGroupID,
                                CostCentreID = profileItem.CostCentreID,
                                IncomeBeforeTax = 0,
                                DependantCount = 0,
                                IncomeTaxable = 0,
                                AmountPaidPITCom = 0,
                                AmountPaidPITEmp = 0,
                                IncomeNET = 0,
                                BankID = profileItem.BankID,
                                AccountNo = profileItem.AccountNo,
                                //Status = FileLog.GetFullPath(FileLog.FileName).Replace(".XML", "_" + DateTime.Now.ToString("yyyyMMdd") + ".XML"),
                            };
                        }

                        lock (listPayrollTable)
                        {
                            listPayrollTable.Add(PayrollTable_Model);
                        }

                        #endregion

                        //bắt lỗi tính công thức có giá trị nào null hay không
                        try
                        {
                            //DatetimeStart = DateTime.Now;
                            listElementFormula = ParseElementFormula(listElementFormula, listElement, TotalData, profileItem, CutOffDuration, listTmpDeduction, ComputeOrderNumber, FileLog);
                            //FileLog.WriteLog("", "Compute Formula", Common.ComputeTime(DatetimeStart, DateTime.Now));

                            listElement = listElement.OrderBy(m => m.OrderNumber).ToList();//sắp xếp lại

                            //Duyệt qua các phần tử tính lương 
                            foreach (var elementItem in listElement)
                            {
                                Sal_PayrollTableItem tableItem = new Sal_PayrollTableItem();
                                tableItem.ID = Guid.NewGuid();
                                tableItem.PayrollTableID = PayrollTable_Model.ID;
                                tableItem.Name = elementItem.ElementName;
                                tableItem.Code = elementItem.ElementCode;
                                tableItem.MonthYear = CutOffDuration.MonthYear;
                                tableItem.IsDecrypt = false;
                                tableItem.IsAddToHourlyRate = false;
                                tableItem.IsChargePIT = false;
                                tableItem.OrderNo = elementItem.OrderNumber != null ? (int)elementItem.OrderNumber : 0;
                                tableItem.ElementType = elementItem.TabType;
                                tableItem.ValueType = elementItem.Type;
                                tableItem.IsShow = elementItem.Invisible != null ? !elementItem.Invisible : true;
                                if (elementItem.IsBold == true)
                                {
                                    tableItem.Description4 += "E_BOLD,";
                                }
                                tableItem.Description4 += elementItem.ElementLevel + "," + elementItem.Type;
                                var ElementResult = listElementFormula.Where(m => m.VariableName.Trim() == elementItem.ElementCode.Trim());
                                if (ElementResult != null)
                                {
                                    tableItem.Value = ElementResult.LastOrDefault().Value.ToString();
                                    tableItem.Description1 = ElementResult.LastOrDefault().ErrorMessage;
                                }
                                else
                                {
                                    tableItem.Value = "0";
                                    tableItem.Description1 = "Không Tìm Thấy Phần Tử !";
                                }

                                lock (listPayrollTableItem)
                                {
                                    listPayrollTableItem.Add(tableItem);
                                }
                            }
                            logItem.Summary += profileItem.CodeEmp + "-" + profileItem.ProfileName + "-" + Common.ComputeTime(DatetimeStart, DateTime.Now) + "-" + listElement.Count.ToString() + "Element Formula";
                            logItem.Source = "Sucess";
                            FileLog.WriteLog(logItem);
                        }
                        catch (Exception ex)
                        {
                            logItem.Summary += profileItem.CodeEmp + "-" + profileItem.ProfileName + "-" + Common.ComputeTime(DatetimeStart, DateTime.Now) + "-" + listElement.Count.ToString() + " Element Formula";
                            logItem.Description = ex.Message;
                            logItem.Source = "Error";
                            FileLog.WriteLog(logItem);
                            //FileLog.WriteLog("Error", "Error Compute Profle " + ProfileID[i].CodeEmp + "-" + ProfileID[i].ProfileName, Common.ComputeTime(DatetimeStart, DateTime.Now));

                            Sal_PayrollTableItem tableItem = new Sal_PayrollTableItem();
                            tableItem.ID = Guid.NewGuid();
                            tableItem.PayrollTableID = PayrollTable_Model.ID;
                            tableItem.Name = "Lỗi, không thể tính được phần tử " + ex.Message;
                            tableItem.Code = "Error";
                            tableItem.MonthYear = CutOffDuration.MonthYear;
                            tableItem.IsDecrypt = false;
                            tableItem.IsAddToHourlyRate = false;
                            tableItem.IsChargePIT = false;
                            tableItem.OrderNo = 0;
                            tableItem.ElementType = "Payroll";
                            tableItem.ValueType = "Double";
                            tableItem.Description4 = "Double";
                            tableItem.Value = "0";
                            tableItem.Description1 = ex.Message;

                            lock (listPayrollTableItem)
                            {
                                listPayrollTableItem.Add(tableItem);
                            }
                        }
                    }
                }

                #endregion

                asynTask = repoSys_AsynTask.FindBy(m => m.ID == Sys_AsynTaskID).FirstOrDefault();
                asynTask.PercentComplete += ((double)ProfileID.Count / (double)totalProfile);
                asynTask.Description = FileLog.GetFullPath(FileLog.FileName).Replace(".XML", "_" + DateTime.Now.ToString("yyyyMMdd") + ".XML");
                asynTask.TimeEnd = DateTime.Now;

                if (asynTask.PercentComplete >= 1)
                {
                    asynTask.Status = AsynTaskStatus.Done.ToString();
                    asynTask.PercentComplete = 1D;
                }

                //if (dataErrorCode == DataErrorCode.Locked)
                //{
                //    asynTask.PercentComplete = 1D;
                //    asynTask.Description = "Dữ Liệu Tính Lương Đã Bị Khóa";
                //    FileLog.WriteLog("Dữ liệu tính lương bị khóa", "--------------------", "--------------------");
                //}

                DatetimeStart = DateTime.Now;

                var connection = context.Database.Connection.GetAdoConnection();

                using (DbCommander commander = new DbCommander(connection))
                {
                    if (connection.IsSqlConnection())
                    {
                        var bulkCopyHelper = new SqlBulkCopyHelper(connection.ConnectionString);
                        var dtPayrollTable = commander.GetSchema("Columns", new string[] { null, null, typeof(Sal_PayrollTable).Name });
                        var dtPayrollTableItem = commander.GetSchema("Columns", new string[] { null, null, typeof(Sal_PayrollTableItem).Name });
                        var payrollTableFields = dtPayrollTable.Rows.OfType<System.Data.DataRow>().Select(d => d["Column_Name"].GetString()).ToArray();
                        var payrollTableItemFields = dtPayrollTableItem.Rows.OfType<System.Data.DataRow>().Select(d => d["Column_Name"].GetString()).ToArray();
                        bulkCopyHelper.WriteToServer(listPayrollTable, typeof(Sal_PayrollTable).Name, payrollTableFields);

                        foreach (var listPayrollTableItemBySize in listPayrollTableItem.Chunk(2000))
                        {
                            try
                            {
                                bulkCopyHelper.WriteToServer(listPayrollTableItemBySize.ToList(),
                                       typeof(Sal_PayrollTableItem).Name, payrollTableItemFields);
                            }
                            catch (Exception ex)
                            {
                                FileLog.WriteLog(ex.Message, "", "");
                            }
                        }
                    }
                    else
                    {
                        var dtPayrollTable = commander.GetSchema("Columns", new string[] { null, typeof(Sal_PayrollTable).Name });
                        var dtPayrollTableItem = commander.GetSchema("Columns", new string[] { null, typeof(Sal_PayrollTableItem).Name });
                        var payrollTableFields = dtPayrollTable.Rows.OfType<System.Data.DataRow>().Select(d => d["Name"].GetString()).ToArray();
                        var payrollTableItemFields = dtPayrollTableItem.Rows.OfType<System.Data.DataRow>().Select(d => d["Name"].GetString()).ToArray();

                        commander.InsertList(typeof(Sal_PayrollTable).Name, listPayrollTable, payrollTableFields);

                        foreach (var listPayrollTableItemBySize in listPayrollTableItem.Chunk(2000))
                        {
                            commander.InsertList(typeof(Sal_PayrollTableItem).Name, listPayrollTableItemBySize.ToList(), payrollTableItemFields);
                        }
                    }
                }
                FileLog.WriteLog("", "", "Save Change 100 Profile-" + Common.ComputeTime(DatetimeStart, DateTime.Now));

                //repoPayrollTableItem.Add(listPayrollTableItem);
                //repoPayrollTable.Add(listPayrollTable);
                unitOfWork.SaveChanges();
            }
        }

        /// Hàm lấy các dữ liệu cần thiết để tính công thức
        /// </summary>
        /// <param name="CutOffDuration"></param>
        /// <returns></returns>
        public ComputePayrollDataModel GetDataForComputeSalary(Att_CutOffDurationEntity CutOffDuration, string UserLogin)
        {
            //biến lưu tất cả các dữ liệu lấy lên
            ComputePayrollDataModel TotalData = new ComputePayrollDataModel();
            try
            {
                #region GetData

                string status = string.Empty;
                List<object> listModel = new List<object>();


                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                //listModel[0] = Common.DotNetToOracle(CutOffDuration.ID.ToString());
                listModel[1] = CutOffDuration.MonthYear;
                listModel[2] = CutOffDuration.MonthYear;
                listModel[3] = 1;
                listModel[4] = int.MaxValue - 1;
                TotalData.listAttendanceTableItem = GetData<Att_AttendanceTableItemEntity>(listModel, ConstantSql.hrm_att_sp_get_AttendanceTableItem, UserLogin, ref status);
                TotalData.Status += TotalData.listAttendanceTableItem == null ? ConstantSql.hrm_att_sp_get_AttendanceTableItem + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[3] = CutOffDuration.DateStart;
                listModel[4] = CutOffDuration.DateEnd;//sửa lại store lấy lên tất cả các grade mà ko có ngày kết thúc
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                TotalData.listGrade = GetData<Sal_GradeEntity>(listModel, ConstantSql.hrm_sal_sp_get_Sal_Grade, UserLogin, ref status);
                TotalData.Status += TotalData.listGrade == null ? ConstantSql.hrm_sal_sp_get_Sal_Grade + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                TotalData.listElement_All = GetData<Cat_ElementEntity>(listModel, ConstantSql.hrm_cat_sp_get_Element_All, UserLogin, ref status);
                TotalData.Status += TotalData.listElement_All == null ? ConstantSql.hrm_cat_sp_get_Element_All + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listUsualAllowance = GetData<Cat_UsualAllowanceEntity>(listModel, ConstantSql.hrm_cat_sp_get_UsualAllowance, UserLogin, ref status);
                TotalData.Status += TotalData.listUsualAllowance == null ? ConstantSql.hrm_cat_sp_get_UsualAllowance + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                TotalData.listUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status);
                TotalData.Status += TotalData.listUnusualAllowanceCfg == null ? ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                //ảnh hưởng tới dữ liệu phụ cấp tháng 3
                listModel = new List<object>();
                listModel.AddRange(new object[9]);
                //listModel[3] = CutOffDuration.DateStart;
                //listModel[4] = CutOffDuration.DateEnd;
                listModel[7] = 1;
                listModel[8] = Int32.MaxValue - 1;
                TotalData.listSalUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status);
                TotalData.Status += TotalData.listSalUnusualAllowance == null ? ConstantSql.hrm_sal_sp_get_UnusualED + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                TotalData.listUnusualAllowanceT3 = TotalData.listSalUnusualAllowance.Where(m => m.MonthStart <= new DateTime(CutOffDuration.MonthYear.Year, 3, 31) && m.MonthEnd >= new DateTime(CutOffDuration.MonthYear.Year, 3, 1)).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[3] = new DateTime(CutOffDuration.MonthYear.Year - 1, 1, 1);
                listModel[4] = new DateTime(CutOffDuration.MonthYear.Year, 12, 31);
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                TotalData.listAttendanceTable = GetData<Att_AttendanceTableEntity>(listModel, ConstantSql.hrm_att_sp_get_attdancetable, UserLogin, ref status);
                TotalData.Status += TotalData.listAttendanceTable == null ? ConstantSql.hrm_att_sp_get_attdancetable + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                TotalData.listCutOffDuration = GetData<Att_CutOffDurationEntity>(listModel, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status);
                TotalData.Status += TotalData.listCutOffDuration == null ? ConstantSql.hrm_att_sp_get_CutOffDurations + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                //Lấy dữ liệu bảng công tháng n-1
                var CutOffDuration_Prev = TotalData.listCutOffDuration.Where(m => m.MonthYear < CutOffDuration.MonthYear).OrderByDescending(m => m.MonthYear).FirstOrDefault();
                if (CutOffDuration_Prev != null)
                {
                    TotalData.Att_AttendanceTable_Prev = TotalData.listAttendanceTable.Where(m => m.CutOffDurationID == CutOffDuration_Prev.ID).ToList();
                }
                else
                {
                    TotalData.Att_AttendanceTable_Prev = new List<Att_AttendanceTableEntity>();
                }

                listModel = new List<object>();
                listModel.AddRange(new object[14]);
                listModel[12] = 1;
                listModel[13] = Int32.MaxValue - 1;
                TotalData.listDiscipline = GetData<Hre_DisciplineEntity>(listModel, ConstantSql.hrm_hr_sp_get_Discipline, UserLogin, ref status);
                TotalData.Status += TotalData.listDiscipline == null ? ConstantSql.hrm_hr_sp_get_Discipline + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[17]);
                listModel[15] = 1;
                listModel[16] = Int32.MaxValue - 1;
                TotalData.listHre_StopWorking = GetData<Hre_StopWorkingEntity>(listModel, ConstantSql.hrm_hr_sp_get_StopWorking, UserLogin, ref status);
                TotalData.Status += TotalData.listHre_StopWorking == null ? ConstantSql.hrm_hr_sp_get_StopWorking + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();//
                listModel.AddRange(new object[14]);
                listModel[6] = CutOffDuration.MonthYear.AddMonths(-1);
                listModel[7] = CutOffDuration.MonthYear.AddMonths(1).AddDays(-1);
                listModel[12] = 1;
                listModel[13] = Int32.MaxValue - 1;
                TotalData.listHre_HDTJob_All = GetData<Hre_HDTJobEntity>(listModel, ConstantSql.hrm_hr_sp_get_HDTJob, UserLogin, ref status);
                TotalData.Status += TotalData.listHre_HDTJob_All == null ? ConstantSql.hrm_hr_sp_get_HDTJob + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[11]);
                listModel[9] = 1;
                listModel[10] = Int32.MaxValue - 1;
                TotalData.listDependant = GetData<Hre_DependantEntity>(listModel, ConstantSql.hrm_hr_sp_get_Dependant, UserLogin, ref status);
                TotalData.Status += TotalData.listDependant == null ? ConstantSql.hrm_hr_sp_get_Dependant + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[6] = CutOffDuration.DateEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                TotalData.listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);
                TotalData.Status += TotalData.listBasicSalary == null ? ConstantSql.hrm_sal_sp_get_BasicPayroll + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[6] = new DateTime(CutOffDuration.MonthYear.Year, 3, 31);
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                if (CutOffDuration.MonthYear.Month < 3)
                {
                    TotalData.listBasicSalaryT3 = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);
                    TotalData.Status += TotalData.listBasicSalary == null ? ConstantSql.hrm_sal_sp_get_BasicPayroll + "," : "";
                    if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;
                }
                else
                {
                    TotalData.listBasicSalaryT3 = TotalData.listBasicSalary.Where(m => m.DateOfEffect <= new DateTime(CutOffDuration.MonthYear.Year, 3, 31)).ToList();
                }

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[0] = CutOffDuration.DateStart;
                listModel[1] = CutOffDuration.DateEnd;
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listSal_SalaryDepartmentItem = GetData<Sal_SalaryDepartmentItemEntity>(listModel, ConstantSql.hrm_Sal_sp_get_SalaryDepartmentItem, UserLogin, ref status);
                TotalData.Status += TotalData.listSal_SalaryDepartmentItem == null ? ConstantSql.hrm_Sal_sp_get_SalaryDepartmentItem + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCat_GradePayroll = GetData<Cat_GradePayrollEntity>(listModel, ConstantSql.hrm_cat_sp_get_GradePayroll, UserLogin, ref status);
                TotalData.Status += TotalData.listCat_GradePayroll == null ? ConstantSql.hrm_cat_sp_get_GradePayroll + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);//viết lại store where theo cutoff
                //listModel[1] = CutOffDuration.MonthYear;
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listInsurance = GetData<Ins_ProfileInsuranceMonthlyEntity>(listModel, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, UserLogin, ref status);
                TotalData.Status += TotalData.listInsurance == null ? ConstantSql.hrm_ins_sp_get_ProfileInsMonthly + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[14]);
                listModel[10] = new DateTime(CutOffDuration.MonthYear.Year, 1, 1);
                listModel[11] = new DateTime(CutOffDuration.MonthYear.Year, 12, 31);
                listModel[12] = 1;
                listModel[13] = Int32.MaxValue - 1;
                TotalData.listEva_Performance = GetData<Eva_PerformanceEntity>(listModel, ConstantSql.hrm_eva_sp_get_Performance, UserLogin, ref status);
                TotalData.Status += TotalData.listEva_Performance == null ? ConstantSql.hrm_eva_sp_get_Performance + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listOvertimeType = GetData<Cat_OvertimeTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_OvertimeType, UserLogin, ref status);
                TotalData.Status += TotalData.listOvertimeType == null ? ConstantSql.hrm_cat_sp_get_OvertimeType + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listLeavedayType = GetData<Cat_LeaveDayTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_LeaveDayType, UserLogin, ref status);
                TotalData.Status += TotalData.listLeavedayType == null ? ConstantSql.hrm_cat_sp_get_LeaveDayType + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCat_Shift = GetData<Cat_ShiftEntity>(listModel, ConstantSql.hrm_cat_sp_get_Shift, UserLogin, ref status);
                TotalData.Status += TotalData.listCat_Shift == null ? ConstantSql.hrm_cat_sp_get_Shift + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listPosition = GetData<Cat_PositionEntity>(listModel, ConstantSql.hrm_cat_sp_get_Position, UserLogin, ref status);
                TotalData.Status += TotalData.listPosition == null ? ConstantSql.hrm_cat_sp_get_Position + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                TotalData.listSalesType = GetData<Eva_SalesTypeEntity>(listModel, ConstantSql.hrm_eva_sp_get_SalesType, UserLogin, ref status);
                TotalData.Status += TotalData.listSalesType == null ? ConstantSql.hrm_eva_sp_get_SalesType + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[0] = CutOffDuration.MonthYear;
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listEva_BonusSalary = GetData<Eva_BonusSalaryEntity>(listModel, ConstantSql.hrm_eva_sp_get_BonusSalary, UserLogin, ref status);
                TotalData.Status += TotalData.listEva_BonusSalary == null ? ConstantSql.hrm_eva_sp_get_BonusSalary + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[10]);//Viết lại store where theo tháng áp dụng
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                TotalData.listSal_HoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status);
                TotalData.Status += TotalData.listSal_HoldSalary == null ? ConstantSql.hrm_sal_sp_get_HoldSalary + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[8]);//Viết lại store where theo tháng áp dụng
                listModel[6] = 1;
                listModel[7] = Int32.MaxValue - 1;
                TotalData.listProfilePartyUnion = GetData<Hre_ProfilePartyUnionEntity>(listModel, ConstantSql.hrm_hr_sp_get_PartyUnionList, UserLogin, ref status);
                TotalData.Status += TotalData.listProfilePartyUnion == null ? ConstantSql.hrm_hr_sp_get_PartyUnionList + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);//Viết lại store where theo tháng áp dụng
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listDayOff = GetData<Cat_DayOffEntity>(listModel, ConstantSql.hrm_cat_sp_get_DayOff, UserLogin, ref status);
                TotalData.Status += TotalData.listDayOff == null ? ConstantSql.hrm_cat_sp_get_DayOff + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);//Viết lại store where theo tháng áp dụng
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listUnAllowCfgAmount = GetData<Cat_UnAllowCfgAmountEntity>(listModel, ConstantSql.hrm_cat_sp_get_Cat_UnAllowCfgAmount, UserLogin, ref status);
                TotalData.Status += TotalData.listUnAllowCfgAmount == null ? ConstantSql.hrm_cat_sp_get_Cat_UnAllowCfgAmount + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                #region Lương Hoa Hồng
                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[0] = CutOffDuration.MonthYear;
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listRevenueRecord = GetData<Sal_RevenueRecordEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueRecord, UserLogin, ref status);
                TotalData.Status += TotalData.listRevenueRecord == null ? ConstantSql.hrm_sal_sp_get_RevenueRecord + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[1] = CutOffDuration.DateStart;
                listModel[2] = CutOffDuration.DateEnd;
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                TotalData.listRevenueForShop = GetData<Sal_RevenueForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueForShop, UserLogin, ref status);
                TotalData.Status += TotalData.listRevenueForShop == null ? ConstantSql.hrm_sal_sp_get_RevenueForShop + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                TotalData.listRevenueForProfile = GetData<Sal_RevenueForProfileEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueForProfile, UserLogin, ref status);
                TotalData.Status += TotalData.listRevenueForProfile == null ? ConstantSql.hrm_sal_sp_get_RevenueForProfile + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listKPIBonus = GetData<Cat_KPIBonusEntity>(listModel, ConstantSql.hrm_cat_sp_get_KPIBonus, UserLogin, ref status);
                TotalData.Status += TotalData.listKPIBonus == null ? ConstantSql.hrm_cat_sp_get_KPIBonus + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listShop = GetData<Cat_ShopEntity>(listModel, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status);
                TotalData.Status += TotalData.listShop == null ? ConstantSql.hrm_cat_sp_get_Shop + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listItemForShop = GetData<Sal_ItemForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_ItemForShop, UserLogin, ref status);
                TotalData.Status += TotalData.listItemForShop == null ? ConstantSql.hrm_sal_sp_get_ItemForShop + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                TotalData.listLineItemForShop = GetData<Sal_LineItemForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_LineItemForShop, UserLogin, ref status);
                TotalData.Status += TotalData.listLineItemForShop == null ? ConstantSql.hrm_sal_sp_get_LineItemForShop + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[2] = CutOffDuration.ID;
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                TotalData.listPayCommissionItem = GetData<Sal_PayCommissionItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayCommissionItem, UserLogin, ref status);
                TotalData.Status += TotalData.listPayCommissionItem == null ? ConstantSql.hrm_sal_sp_get_PayCommissionItem + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;
                #endregion

                #region vietject
                listModel = new List<object>();
                listModel.AddRange(new object[4]);//viết lại store where với ngày tháng < cutof.dateend
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCat_UnitPrice = GetData<Cat_UnitPriceEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnitPrice, UserLogin, ref status);
                TotalData.Status += TotalData.listCat_UnitPrice == null ? ConstantSql.hrm_cat_sp_get_UnitPrice + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[8]);//viết lại store where với ngày tháng
                listModel[6] = 1;
                listModel[7] = Int32.MaxValue - 1;
                TotalData.listAtt_TimeSheet = GetData<Att_TimeSheetEntity>(listModel, ConstantSql.hrm_att_sp_get_TimeSheet, UserLogin, ref status);
                TotalData.Status += TotalData.listAtt_TimeSheet == null ? ConstantSql.hrm_att_sp_get_TimeSheet + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCat_Role = GetData<Cat_RoleEntity>(listModel, ConstantSql.hrm_cat_sp_get_Role, UserLogin, ref status);
                TotalData.Status += TotalData.listCat_Role == null ? ConstantSql.hrm_cat_sp_get_Role + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCat_JobType = GetData<Cat_JobTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status);
                TotalData.Status += TotalData.listCat_JobType == null ? ConstantSql.hrm_cat_sp_get_JobType + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                #endregion

                #region Tổng hợp dữ liệu canteen
                listModel = new List<object>();
                listModel.AddRange(new object[6]);//Viết lại store where theo tháng áp dụng
                //listModel[0] = CutOffDuration.DateStart;
                //listModel[1] = CutOffDuration.DateEnd;
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                TotalData.listSumryMealRecord = GetData<Can_SumryMealRecordEntity>(listModel, ConstantSql.hrm_can_sp_get_SumMealRecord, UserLogin, ref status);
                TotalData.Status += TotalData.listSumryMealRecord == null ? ConstantSql.hrm_can_sp_get_SumMealRecord + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;
                #endregion

                #region Tiền tệ
                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                TotalData.listCurrency = GetData<Cat_CurrencyEntity>(listModel, ConstantSql.hrm_cat_sp_get_Currency, UserLogin, ref status);
                TotalData.Status += TotalData.listCurrency == null ? ConstantSql.hrm_cat_sp_get_Currency + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                TotalData.listExchangeRate = GetData<Cat_ExchangeRateEntity>(listModel, ConstantSql.hrm_cat_sp_get_ExchangeRate, UserLogin, ref status);
                TotalData.Status += TotalData.listExchangeRate == null ? ConstantSql.hrm_cat_sp_get_ExchangeRate + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;
                #endregion

                #region Đánh Giá

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                TotalData.listPerformanceType = GetData<Cat_PerformanceTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_PerformanceType, UserLogin, ref status);
                TotalData.Status += TotalData.listPerformanceType == null ? ConstantSql.hrm_cat_sp_get_PerformanceType + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                #endregion

                listModel = new List<object>();
                listModel.AddRange(new object[17]);
                listModel[11] = CutOffDuration.DateStart;
                listModel[12] = CutOffDuration.DateEnd;
                listModel[15] = 1;
                listModel[16] = int.MaxValue - 1;
                TotalData.listWorkHistory = GetData<Hre_WorkHistoryEntity>(listModel, ConstantSql.hrm_hr_sp_get_WorkHistory, UserLogin, ref status);
                TotalData.Status += TotalData.listWorkHistory == null ? ConstantSql.hrm_hr_sp_get_WorkHistory + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[8]);
                listModel[1] = CutOffDuration.DateStart;
                listModel[2] = CutOffDuration.DateEnd;
                listModel[6] = 1;
                listModel[7] = int.MaxValue - 1;
                TotalData.listAnnualDetail = GetData<Att_AnnualDetailEntity>(listModel, ConstantSql.hrm_att_sp_get_AnnualDetail, UserLogin, ref status);
                TotalData.Status += TotalData.listAnnualDetail == null ? ConstantSql.hrm_att_sp_get_AnnualDetail + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[8]);
                listModel[6] = 1;
                listModel[7] = int.MaxValue - 1;
                TotalData.listSalaryInformation = GetData<Sal_SalaryInformationEntity>(listModel, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref status);
                TotalData.Status += TotalData.listSalaryInformation == null ? ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[3] = CutOffDuration.DateStart;
                listModel[4] = CutOffDuration.DateEnd;
                listModel[8] = 1;
                listModel[9] = int.MaxValue - 1;
                TotalData.listRoster = GetData<Att_RosterEntity>(listModel, ConstantSql.hrm_att_sp_get_Roster, UserLogin, ref status);
                TotalData.Status += TotalData.listRoster == null ? ConstantSql.hrm_att_sp_get_Roster + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                listModel = new List<object>();
                listModel.AddRange(new object[13]);
                listModel[2] = CutOffDuration.DateStart;
                listModel[3] = CutOffDuration.DateEnd;
                listModel[11] = 1;
                listModel[12] = int.MaxValue - 1;
                TotalData.listOverTime = GetData<Att_OvertimeEntity>(listModel, ConstantSql.hrm_att_sp_get_Overtime, UserLogin, ref status);
                TotalData.Status += TotalData.listOverTime == null ? ConstantSql.hrm_att_sp_get_Overtime + "," : "";
                if (!string.IsNullOrEmpty(TotalData.Status)) return TotalData;

                #endregion

                return TotalData;
            }
            catch (Exception ex)
            {
                TotalData.Status = ex.Message;
                return TotalData;
            }

        }

        /// <summary>
        /// Lấy dữ liệu có liên quan tới Cutoffduration
        /// </summary>
        /// <param name="TotalData"></param>
        /// <param name="CutOffDuration"></param>
        /// <returns></returns>
        public ComputePayrollDataModel GetDataForComputeSalary_ForCutOff(ComputePayrollDataModel TotalData, Att_CutOffDurationEntity CutOffDuration, string UserLogin)
        {
            #region GetData
            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = CutOffDuration.DateStart;
            listModel[4] = CutOffDuration.DateEnd;//sửa lại store lấy lên tất cả các grade mà ko có ngày kết thúc
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            TotalData.listGrade = GetData<Sal_GradeEntity>(listModel, ConstantSql.hrm_sal_sp_get_Sal_Grade, UserLogin, ref status);
            TotalData.Status += TotalData.listGrade == null ? ConstantSql.hrm_sal_sp_get_Sal_Grade + "," : "";

            TotalData.listUnusualAllowanceT3 = TotalData.listSalUnusualAllowance.Where(m => m.MonthStart <= new DateTime(CutOffDuration.MonthYear.Year, 3, 31) && m.MonthEnd >= new DateTime(CutOffDuration.MonthYear.Year, 3, 1)).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = new DateTime(CutOffDuration.MonthYear.Year - 1, 1, 1);
            listModel[4] = new DateTime(CutOffDuration.MonthYear.Year, 12, 31);
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            TotalData.listAttendanceTable = GetData<Att_AttendanceTableEntity>(listModel, ConstantSql.hrm_att_sp_get_attdancetable, UserLogin, ref status);
            TotalData.Status += TotalData.listAttendanceTable == null ? ConstantSql.hrm_att_sp_get_attdancetable + "," : "";


            //Lấy dữ liệu bảng công tháng n-1
            var CutOffDuration_Prev = TotalData.listCutOffDuration.Where(m => m.MonthYear < CutOffDuration.MonthYear).OrderByDescending(m => m.MonthYear).FirstOrDefault();
            if (CutOffDuration_Prev != null)
            {
                TotalData.Att_AttendanceTable_Prev = TotalData.listAttendanceTable.Where(m => m.CutOffDurationID == CutOffDuration_Prev.ID).ToList();
            }
            else
            {
                TotalData.Att_AttendanceTable_Prev = new List<Att_AttendanceTableEntity>();
            }

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[6] = CutOffDuration.DateEnd;
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            TotalData.listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);
            TotalData.Status += TotalData.listBasicSalary == null ? ConstantSql.hrm_sal_sp_get_BasicPayroll + "," : "";

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[6] = new DateTime(CutOffDuration.MonthYear.Year, 3, 31);
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            if (CutOffDuration.MonthYear.Month < 3)
            {
                TotalData.listBasicSalaryT3 = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);
                TotalData.Status += TotalData.listBasicSalary == null ? ConstantSql.hrm_sal_sp_get_BasicPayroll + "," : "";
            }
            else
            {
                TotalData.listBasicSalaryT3 = TotalData.listBasicSalary.Where(m => m.DateOfEffect <= new DateTime(CutOffDuration.MonthYear.Year, 3, 31)).ToList();
            }

            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[0] = CutOffDuration.DateStart;
            listModel[1] = CutOffDuration.DateEnd;
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            TotalData.listSal_SalaryDepartmentItem = GetData<Sal_SalaryDepartmentItemEntity>(listModel, ConstantSql.hrm_Sal_sp_get_SalaryDepartmentItem, UserLogin, ref status);
            TotalData.Status += TotalData.listSal_SalaryDepartmentItem == null ? ConstantSql.hrm_Sal_sp_get_SalaryDepartmentItem + "," : "";

            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[0] = CutOffDuration.MonthYear;
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            TotalData.listEva_BonusSalary = GetData<Eva_BonusSalaryEntity>(listModel, ConstantSql.hrm_eva_sp_get_BonusSalary, UserLogin, ref status);
            TotalData.Status += TotalData.listEva_BonusSalary == null ? ConstantSql.hrm_eva_sp_get_BonusSalary + "," : "";

            #region Lương Hoa Hồng
            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[0] = CutOffDuration.MonthYear;
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            TotalData.listRevenueRecord = GetData<Sal_RevenueRecordEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueRecord, UserLogin, ref status);
            TotalData.Status += TotalData.listRevenueRecord == null ? ConstantSql.hrm_sal_sp_get_RevenueRecord + "," : "";

            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[1] = CutOffDuration.DateStart;
            listModel[2] = CutOffDuration.DateEnd;
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            TotalData.listRevenueForShop = GetData<Sal_RevenueForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueForShop, UserLogin, ref status);
            TotalData.Status += TotalData.listRevenueForShop == null ? ConstantSql.hrm_sal_sp_get_RevenueForShop + "," : "";

            #endregion

            listModel = new List<object>();
            listModel.AddRange(new object[17]);
            listModel[11] = CutOffDuration.DateStart;
            listModel[12] = CutOffDuration.DateEnd;
            listModel[15] = 1;
            listModel[16] = int.MaxValue - 1;
            TotalData.listWorkHistory = GetData<Hre_WorkHistoryEntity>(listModel, ConstantSql.hrm_hr_sp_get_WorkHistory, UserLogin, ref status);
            TotalData.Status += TotalData.listWorkHistory == null ? ConstantSql.hrm_hr_sp_get_WorkHistory + "," : "";

            listModel = new List<object>();
            listModel.AddRange(new object[8]);
            listModel[1] = CutOffDuration.DateStart;
            listModel[2] = CutOffDuration.DateEnd;
            listModel[6] = 1;
            listModel[7] = int.MaxValue - 1;
            TotalData.listAnnualDetail = GetData<Att_AnnualDetailEntity>(listModel, ConstantSql.hrm_att_sp_get_AnnualDetail, UserLogin, ref status);
            TotalData.Status += TotalData.listAnnualDetail == null ? ConstantSql.hrm_att_sp_get_AnnualDetail + "," : "";
            #endregion
            return TotalData;
        }

        /// <summary>
        /// Hàm chạy tính hết tất cả các formula có trong grade
        /// </summary>
        /// <param name="listElementFormula"></param>
        /// <param name="listElement"></param>
        /// <param name="TotalData"></param>
        /// <param name="profileItem"></param>
        /// <param name="CutOffDuration"></param>
        /// <param name="listTmpDeduction"></param>
        /// <returns></returns>
        public List<ElementFormula> ParseElementFormula(List<ElementFormula> listElementFormula, List<Cat_ElementEntity> listGradeElement, ComputePayrollDataModel TotalData, Hre_ProfileEntity profileItem, Att_CutOffDurationEntity CutOffDuration, Dictionary<Guid, ValueCount> listTmpDeduction, bool ComputeOrderNumber, TraceLogManager FileLog)
        {
            if (ComputeOrderNumber)
            {
                listGradeElement = listGradeElement.OrderBy(m => m.OrderNumber).ToList();
                foreach (var elementItem in listGradeElement)
                {
                    try
                    {
                        listElementFormula = GetStaticValues(TotalData, listElementFormula, profileItem, CutOffDuration, elementItem.ElementCode, listTmpDeduction);
                        var result = FormulaHelper.ParseFormula(elementItem.Formula.Replace("[", "").Replace("]", ""), listElementFormula.Distinct().ToList());
                        listElementFormula.Add(new ElementFormula(elementItem.ElementCode, result.Value, 0, result.ErrorMessage));
                        //listElementFormula = ParseFormula(elementItem, listElementFormula, TotalData, profileItem, CutOffDuration, listTmpDeduction);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(elementItem.ElementCode);
                    }
                }
            }
            else
            {
                foreach (var elementItem in listGradeElement)
                {

                    try
                    {
                        listElementFormula = ParseFormula(elementItem, listElementFormula, TotalData, profileItem, CutOffDuration, listTmpDeduction);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(elementItem.ElementCode);
                    }

                }
            }
            return listElementFormula;
        }

        /// <summary>
        /// Hàm Parse công thức ra phần tử và lưu vào list tổng 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="listElementFormula"></param>
        /// <param name="TotalData"></param>
        /// <param name="profileItem"></param>
        /// <param name="CutOffDuration"></param>
        /// <param name="listTmpDeduction"></param>
        /// <returns></returns>
        public List<ElementFormula> ParseFormula(Cat_ElementEntity formula, List<ElementFormula> listElementFormula, ComputePayrollDataModel TotalData, Hre_ProfileEntity profileItem, Att_CutOffDurationEntity CutOffDuration, Dictionary<Guid, ValueCount> listTmpDeduction)
        {
            string strFormula = formula.Formula.Replace("\n", "").Replace("\t", "").Trim();

            //Các phần tử tính lương tách ra từ 1 chuỗi công thức
            List<string> ListFormula = ParseFormulaToList(strFormula).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();

            //Các phần tử tính lương chưa có kết quả
            List<string> ListFormulaNotValue = ListFormula.Where(m => !listElementFormula.Any(t => t.VariableName == m.Replace("[", "").Replace("]", ""))).ToList();

            //có phần tử chưa được tính trước đó
            if (ListFormulaNotValue != null && ListFormulaNotValue.Count > 0)
            {
                foreach (string elementNotValue in ListFormulaNotValue)
                {

                    //kiểm tra phần tử đó là phần tử Enum hay là phần tử công thức
                    if (TotalData.listElement_All.Any(m => m.GradePayrollID != null && m.ElementCode == elementNotValue.Replace("[", "").Replace("]", "")))//là phần tử công thức
                    {
                        //var tt = TotalData.listElement_All.Where(m => m.ElementCode == elementNotValue.Replace("[", "").Replace("]", "")).FirstOrDefault();
                        listElementFormula = ParseFormula(TotalData.listElement_All.Where(m => m.ElementCode == elementNotValue.Replace("[", "").Replace("]", "")).FirstOrDefault(), listElementFormula, TotalData, profileItem, CutOffDuration, listTmpDeduction);
                    }
                    else//là phần tử enum
                    {
                        listElementFormula = GetStaticValues(TotalData, listElementFormula, profileItem, CutOffDuration, formula.ElementCode, listTmpDeduction);
                    }
                }
            }

            //Do mệnh đề if luôn trả về false nên xử lý riêng cho mệnh đề if ở đây
            //nguyên ngân là do dll CalcEngine, nhưng chưa tìm ra cách giải quyết
            if (formula.Formula.ToUpper().Contains("IF("))
            {
                string _formula = formula.Formula;
                foreach (var i in listElementFormula.Distinct().ToList())
                {
                    if (formula.Formula.Contains("[" + i.VariableName + "]"))
                    {
                        if (i.Value != null)
                        {
                            _formula = _formula.Replace("[" + i.VariableName + "]", i.Value.ToString());
                        }
                        else
                        {
                            i.Value = 0;
                            i.ErrorMessage = "Null";
                            _formula = _formula.Replace("[" + i.VariableName + "]", i.Value.ToString());
                        }
                    }
                }
                var result = FormulaHelper.ParseFormula(_formula.Replace("[", "").Replace("]", ""), listElementFormula.Distinct().ToList());
                listElementFormula.Add(new ElementFormula(formula.ElementCode, result.Value, 0, result.ErrorMessage));
            }
            else
            {
                var result = FormulaHelper.ParseFormula(formula.Formula.Replace("[", "").Replace("]", ""), listElementFormula.Distinct().ToList());
                listElementFormula.Add(new ElementFormula(formula.ElementCode, result.Value, 0, result.ErrorMessage));
            }
            return listElementFormula.Distinct().ToList();
        }

        /// <summary>
        /// Hàm lấy các phần tử là Enum
        /// </summary>
        /// <param name="TotalData">Class chứa tất cả các dữ liệu lấy lên để xử lý</param>
        /// <param name="listElementFormula">Lưu giá trị các thông thức đã tính rồi</param>
        /// <param name="profileItem">Nhân viên hiện tại được tính</param>
        /// <param name="CutOffDuration">Kỳ tính lương</param>
        /// <param name="formula">Công thức tính</param>
        /// <param name="listTmpDeduction">Biến tạm phục vụ cho tiền khấu trừ thâm niên của dự án SCV</param>
        /// <param name="DateClose">Ngày chốt lương</param>
        /// <returns></returns>
        public List<ElementFormula> GetStaticValues(ComputePayrollDataModel TotalData, List<ElementFormula> listElementFormula, Hre_ProfileEntity profileItem, Att_CutOffDurationEntity CutOffDuration, string ElementCode, Dictionary<Guid, ValueCount> listTmpDeduction)
        {
            Cat_ElementEntity formula = TotalData.listElement_All.Where(m => m.ElementCode == ElementCode).FirstOrDefault();
            ElementFormula item = new ElementFormula();

            #region Thêm các phần tử là loại phụ cấp
            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUsualAllowance.Select(m => m.Code).ToArray()))
            {
                foreach (var t in TotalData.listUsualAllowance)
                {
                    listElementFormula.Add(new ElementFormula(t.Code, t.Formula, 0));
                }
            }
            #endregion

            #region Quy đổi tiền tệ
            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listCurrency.Select(m => m.Code + "_BUYING").ToArray()))
            {
                //list lưu các element 
                var ListExChangeRateElement = TotalData.listElement_All.Where(m => m.ElementCode.EndsWith("_BUYING") && m.GradePayrollID == null).ToList();
                //list lưu giá trị tiền tệ
                var ListExChangeRateByGrade = TotalData.listExchangeRate.Where(m => m.MonthOfEffect >= CutOffDuration.DateStart && m.MonthOfEffect <= CutOffDuration.DateEnd).ToList();
                foreach (var i in ListExChangeRateElement)
                {
                    string[] arrCurrencyCode = i.ElementCode.Split('_').ToArray();
                    if (arrCurrencyCode.Count() != 3)
                    {
                        var ExChangeRateItem = ListExChangeRateByGrade.Where(m => m.CurrencyBaseCode == arrCurrencyCode[0] && m.CurrencyDestCode == arrCurrencyCode[1]).OrderByDescending(m => m.MonthOfEffect).FirstOrDefault();
                        if (ExChangeRateItem != null)
                        {
                            item = new ElementFormula(i.ElementCode, ExChangeRateItem.BuyingRate != null ? ExChangeRateItem.BuyingRate : 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listCurrency.Select(m => m.Code + "_SELLING").ToArray()))
            {
                //list lưu các element 
                var ListExChangeRateElement = TotalData.listElement_All.Where(m => m.ElementCode.EndsWith("_SELLING") && m.GradePayrollID == null).ToList();
                //list lưu giá trị tiền tệ
                var ListExChangeRateByGrade = TotalData.listExchangeRate.Where(m => m.MonthOfEffect >= CutOffDuration.DateStart && m.MonthOfEffect <= CutOffDuration.DateEnd).ToList();
                foreach (var i in ListExChangeRateElement)
                {
                    string[] arrCurrencyCode = i.ElementCode.Split('_').ToArray();
                    if (arrCurrencyCode.Count() != 3)
                    {
                        var ExChangeRateItem = ListExChangeRateByGrade.Where(m => m.CurrencyBaseCode == arrCurrencyCode[0] && m.CurrencyDestCode == arrCurrencyCode[1]).OrderByDescending(m => m.MonthOfEffect).FirstOrDefault();
                        if (ExChangeRateItem != null)
                        {
                            item = new ElementFormula(i.ElementCode, ExChangeRateItem.SellingRate != null ? ExChangeRateItem.SellingRate : 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }
            }
            #endregion

            #region Kiểm tra xem nhân viên này có phụ cấp phát sinh trong tháng đang tính lương hay không ?
            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_T3").ToArray()))//đã lấy lên chưa ?
            {
                List<Sal_UnusualAllowanceEntity> listSal_UnusualT3 = new List<Sal_UnusualAllowanceEntity>();
                listSal_UnusualT3 = TotalData.listUnusualAllowanceT3.Where(m => m.ProfileID == profileItem.ID).ToList();
                ElementFormula _tmpitem = new ElementFormula();
                for (int j = 0; j < TotalData.listUnusualAllowanceCfg.Count; j++)
                {
                    //lay phu cap thang 3
                    _tmpitem = new ElementFormula();
                    _tmpitem.VariableName = TotalData.listUnusualAllowanceCfg[j].Code + "_T3";
                    var Sal_UnusualItem = listSal_UnusualT3.Where(m => m.UnusualEDTypeID == TotalData.listUnusualAllowanceCfg[j].ID && m.MonthStart <= CutOffDuration.DateEnd && (m.MonthEnd == null || m.MonthEnd >= CutOffDuration.DateStart)).ToList();
                    if (Sal_UnusualItem != null)
                    {
                        _tmpitem.Value = Sal_UnusualItem.Sum(m => m.Amount != null ? m.Amount : 0);
                        listElementFormula.Add(_tmpitem);
                    }
                    else
                    {
                        _tmpitem.Value = "0";
                        _tmpitem.ErrorMessage = "Không Có Phụ Cấp Trong Tháng 3";
                        listElementFormula.Add(_tmpitem);
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_DAYCLOSE_N_1").ToArray()))
            {
                DateTime Dateform = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateTo = CutOffDuration.DateEnd.AddMonths(-1);

                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, Dateform, DateTo);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                //ngày bắt đầu chốt lương
                DateTime DateClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                //ngày kết thúc chốt lương
                DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                List<Sal_UnusualAllowanceEntity> ListUnusualAllowanceByProfile = TotalData.listSalUnusualAllowance.Where(m => m.ProfileID == profileItem.ID && m.MonthStart != null && m.MonthStart <= DateEndClose && (m.MonthEnd == null || m.MonthEnd >= DateClose)).ToList();

                ElementFormula _item = new ElementFormula();
                foreach (var i in TotalData.listUnusualAllowanceCfg)
                {
                    var listValue = ListUnusualAllowanceByProfile.Where(m => m.UnusualEDTypeID == i.ID).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                    if (listValue != null)
                    {
                        _item = new ElementFormula(i.Code + "_DAYCLOSE_N_1", listValue.Amount != null ? listValue.Amount : 0, 0);
                        listElementFormula.Add(_item);
                    }
                    else
                    {
                        _item = new ElementFormula(i.Code + "_DAYCLOSE_N_1", 0, 0);
                        listElementFormula.Add(_item);
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_DAYCLOSE").ToArray()))
            {
                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                //ngày bắt đầu chốt lương
                DateTime DateClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                //ngày kết thúc chốt lương
                DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                List<Sal_UnusualAllowanceEntity> ListUnusualAllowanceByProfile = TotalData.listSalUnusualAllowance.Where(m => m.ProfileID == profileItem.ID && m.MonthStart != null && m.MonthStart <= DateEndClose && (m.MonthEnd == null || m.MonthEnd >= DateClose)).ToList();

                ElementFormula _item = new ElementFormula();
                foreach (var i in TotalData.listUnusualAllowanceCfg)
                {
                    var listValue = ListUnusualAllowanceByProfile.Where(m => m.UnusualEDTypeID == i.ID).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                    if (listValue != null)
                    {
                        _item = new ElementFormula(i.Code + "_DAYCLOSE", listValue.Amount != null ? listValue.Amount : 0, 0);
                        listElementFormula.Add(_item);
                    }
                    else
                    {
                        _item = new ElementFormula(i.Code + "_DAYCLOSE", 0, 0);
                        listElementFormula.Add(_item);
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_AMOUNTOFOFFSET_N_1").ToArray()))
            {
                DateTime Dateform = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateTo = CutOffDuration.DateEnd.AddMonths(-1);

                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, Dateform, DateTo);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                //ngày bắt đầu chốt lương
                DateTime DateClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                //ngày kết thúc chốt lương
                DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                List<Sal_UnusualAllowanceEntity> listUnusualAllowanceByDateClose = TotalData.listSalUnusualAllowance.Where(m => m.MonthStart <= DateEndClose && (m.MonthEnd >= DateClose || m.MonthEnd == null) && m.ProfileID == profileItem.ID).ToList();

                ElementFormula _item = new ElementFormula();
                foreach (var i in TotalData.listUnusualAllowanceCfg)
                {
                    var listValue = listUnusualAllowanceByDateClose.Where(m => m.UnusualEDTypeID == i.ID).ToList();
                    if (listValue != null)
                    {
                        _item = new ElementFormula(i.Code + "_AMOUNTOFOFFSET_N_1", listValue.Sum(m => m.AmountOfOffSet != null ? m.AmountOfOffSet : 0), 0);
                        listElementFormula.Add(_item);
                    }
                    else
                    {
                        _item = new ElementFormula(i.Code + "_AMOUNTOFOFFSET_N_1", 0, 0);
                        listElementFormula.Add(_item);
                    }
                }

            }

            //lấy mức phụ cấp theo timeline tháng n-1
            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_TIMELINE_N_1").ToArray()))
            {
                DateTime Dateform = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateTo = CutOffDuration.DateEnd.AddMonths(-1);

                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, Dateform, DateTo);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                //ngày bắt đầu chốt lương
                DateTime DateClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                //ngày kết thúc chốt lương
                DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.AddMonths(-1).Year, CutOffDuration.MonthYear.AddMonths(-1).Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                List<Cat_UnAllowCfgAmountEntity> listUnAllowCfgAmount = TotalData.listUnAllowCfgAmount.Where(m => m.FromDate <= DateEndClose && m.ToDate >= DateClose).ToList();

                ElementFormula _item = new ElementFormula();
                foreach (var i in TotalData.listUnusualAllowanceCfg)
                {
                    var listValue = listUnAllowCfgAmount.Where(m => m.UnusualAllowanceID != null && (Guid)m.UnusualAllowanceID == i.ID).ToList();
                    if (listValue != null)
                    {
                        _item = new ElementFormula(i.Code + "_TIMELINE_N_1", listValue.Sum(m => m.Amount != null ? m.Amount : 0), 0);
                        listElementFormula.Add(_item);
                    }
                    else
                    {
                        _item = new ElementFormula(i.Code + "_TIMELINE_N_1", 0, 0);
                        listElementFormula.Add(_item);
                    }
                }
            }

            //lấy mức phụ cấp theo timeline
            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_TIMELINE").ToArray()))
            {
                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                //ngày bắt đầu chốt lương
                DateTime DateClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                //ngày kết thúc chốt lương
                DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                List<Cat_UnAllowCfgAmountEntity> listUnAllowCfgAmount = TotalData.listUnAllowCfgAmount.Where(m => m.FromDate <= DateEndClose && m.ToDate >= DateClose).ToList();

                ElementFormula _item = new ElementFormula();
                foreach (var i in TotalData.listUnusualAllowanceCfg)
                {
                    var listValue = listUnAllowCfgAmount.Where(m => m.UnusualAllowanceID != null && (Guid)m.UnusualAllowanceID == i.ID).ToList();
                    if (listValue != null)
                    {
                        _item = new ElementFormula(i.Code + "_TIMELINE", listValue.Sum(m => m.Amount != null ? m.Amount : 0), 0);
                        listElementFormula.Add(_item);
                    }
                    else
                    {
                        _item = new ElementFormula(i.Code + "_TIMELINE", 0, 0);
                        listElementFormula.Add(_item);
                    }
                }
            }


            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code + "_N_1").ToArray()))//đã lấy lên chưa ?
            {
                List<Sal_UnusualAllowanceEntity> listSal_Unusual = new List<Sal_UnusualAllowanceEntity>();
                ElementFormula _tmpitem = new ElementFormula();
                listSal_Unusual = TotalData.listSalUnusualAllowance.Where(m => m.ProfileID == profileItem.ID).ToList();
                //lấy các loại phụ cấp của 6 tháng trước tháng tính lương (Honda)
                for (int j = 0; j < TotalData.listUnusualAllowanceCfg.Count; j++)
                {
                    for (int t = 1; t <= 6; t++)
                    {
                        _tmpitem = new ElementFormula();
                        _tmpitem.VariableName = TotalData.listUnusualAllowanceCfg[j].Code + "_N_" + t.ToString();
                        var Sal_UnusualItem = listSal_Unusual.Where(m => m.UnusualEDTypeID == TotalData.listUnusualAllowanceCfg[j].ID && m.MonthStart <= CutOffDuration.DateEnd.AddMonths(t * -1) && (m.MonthEnd == null || m.MonthEnd >= CutOffDuration.DateStart.AddMonths(t * -1))).ToList();
                        if (Sal_UnusualItem != null)
                        {
                            _tmpitem.Value = Sal_UnusualItem.Sum(m => m.Amount != null ? m.Amount : 0);
                            listElementFormula.Add(_tmpitem);
                        }
                        else
                        {
                            _tmpitem.Value = "0";
                            _tmpitem.ErrorMessage = "Không Có Phụ Cấp Trong Tháng";
                            listElementFormula.Add(_tmpitem);
                        }
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, TotalData.listUnusualAllowanceCfg.Select(m => m.Code).ToArray()))//đã lấy lên chưa ?
            {
                List<Sal_UnusualAllowanceEntity> listSal_Unusual = new List<Sal_UnusualAllowanceEntity>();
                // List<Sal_UnusualAllowanceEntity> listSal_UnusualT3 = new List<Sal_UnusualAllowanceEntity>();
                listSal_Unusual = TotalData.listSalUnusualAllowance.Where(m => m.ProfileID == profileItem.ID).ToList();
                // listSal_UnusualT3 = TotalData.listUnusualAllowanceT3.Where(m => m.ProfileID == profileItem.ID).ToList();
                //add loại phụ cấp bất thường vào list phần tử(listElement), các loại phụ cấp nào không có thì cho formula = 0
                for (int j = 0; j < TotalData.listUnusualAllowanceCfg.Count; j++)
                {
                    ElementFormula _tmpitem = new ElementFormula();
                    _tmpitem.VariableName = TotalData.listUnusualAllowanceCfg[j].Code;
                    var Sal_UnusualItem = listSal_Unusual.Where(m => m.UnusualEDTypeID == TotalData.listUnusualAllowanceCfg[j].ID && m.MonthStart <= CutOffDuration.DateEnd && (m.MonthEnd == null || m.MonthEnd >= CutOffDuration.DateStart)).ToList();
                    if (Sal_UnusualItem != null)
                    {
                        _tmpitem.Value = Sal_UnusualItem.Sum(m => m.Amount != null ? m.Amount : 0);
                        listElementFormula.Add(_tmpitem);
                    }
                    else
                    {
                        _tmpitem.Value = "0";
                        _tmpitem.ErrorMessage = "Không Có Phụ Cấp Trong Tháng";
                        listElementFormula.Add(_tmpitem);
                    }
                }
            }
            #endregion

            #region Lấy giá trị các phần tử là Enum

            var listAttendanceTableProCut = TotalData.listAttendanceTable.Where(m => m.ProfileID == profileItem.ID && m.DateStart <= CutOffDuration.DateEnd && m.DateEnd >= CutOffDuration.DateStart).FirstOrDefault();

            #region Enum HRE
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.HR_WORKINGDAY.ToString(), PayrollElement.HR_LEAVEDAY.ToString(), PayrollElement.HR_IS_LEAVEDAY.ToString(), PayrollElement.HR_IS_WORKINGDAY.ToString() }))
            {
                //Ngày vào làm
                item = new ElementFormula(PayrollElement.HR_WORKINGDAY.ToString(), profileItem.DateHire, 0);
                listElementFormula.Add(item);

                //Ngày nghỉ việc
                item = new ElementFormula(PayrollElement.HR_LEAVEDAY.ToString(), profileItem.DateQuit != null ? profileItem.DateQuit : DateTime.MinValue, 0, profileItem.DateQuit != null ? "" : "Null");
                listElementFormula.Add(item);

                //NV có ngày nghỉ việc trong tháng
                item = new ElementFormula(PayrollElement.HR_IS_LEAVEDAY.ToString(), (profileItem.DateQuit <= CutOffDuration.DateEnd && profileItem.DateQuit >= CutOffDuration.DateStart) == true ? 0 : 1, 0);
                listElementFormula.Add(item);

                //NV có ngày vào làm trong tháng
                item = new ElementFormula(PayrollElement.HR_IS_WORKINGDAY.ToString(), (profileItem.DateHire <= CutOffDuration.DateEnd && profileItem.DateHire >= CutOffDuration.DateStart) == true ? 1 : 0, 0);
                listElementFormula.Add(item);

            }

            ////Bậc / Hệ số lương (Code)
            //item = new ElementFormula(PayrollElement.HR_SALARYCLASSNAME.ToString(), profileItem.SalaryClassID != null ? profileItem.SalaryClassName : "", 0);
            //listElementFormula.Add(item);


            //Số ngày từ ngày vào đến cuối tháng, không tính những ngày dayoff từ ngày vào đến cuối tháng (VD: vào ngày 05/01, số ngày dayoff từ mùng 5 đến 31 = 6 => trả về: 31 (số ngày trong tháng) - 5 (ngày vào) + 1 (từ 5 đến 31 là 27 ngày nên phải + thêm 1) - 6 (số ngày dayoff từ ngày 05/01 đến 31/01) = 21)
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_COUNT_DAY_TO_DATEEND_CUTOFF.ToString()))
            {
                if (profileItem.DateHire != null && profileItem.DateHire >= new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, 1))
                {
                    DateTime DateStart = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, 1);
                    DateTime DateEnd = DateStart.AddMonths(1).AddDays(-1);
                    //số ngày trong tháng
                    double dayInCutoff = DateEnd.Subtract(profileItem.DateHire.Value).TotalDays + 1;
                    double dayOff = TotalData.listDayOff.Where(m => m.DateOff <= DateEnd && m.DateOff >= profileItem.DateHire.Value).Count();

                    item = new ElementFormula(PayrollElement.HR_COUNT_DAY_TO_DATEEND_CUTOFF.ToString(), dayInCutoff - dayOff, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.HR_COUNT_DAY_TO_DATEEND_CUTOFF.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }
            }

            //Số ngày trong tháng tính lương - tổng số ngày dayoff trong tháng tính lương (VD: tháng 1 có 31 ngày - 8 ngày dayoff = 23)
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_COUNT_DAY_IN_CUTOFF.ToString()))
            {
                DateTime DateStart = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, 1);
                DateTime DateEnd = DateStart.AddMonths(1).AddDays(-1);

                double dayInCutoff = DateEnd.Subtract(DateStart).TotalDays + 1;
                double dayOff = TotalData.listDayOff.Where(m => m.DateOff <= DateEnd && m.DateOff >= DateStart).Count();

                item = new ElementFormula(PayrollElement.HR_COUNT_DAY_IN_CUTOFF.ToString(), dayInCutoff - dayOff, 0);
                listElementFormula.Add(item);
            }

            //Số ngày dayoff từ ngày 1 tháng tính lương đến ngày vào làm (VD: vào làm ngày 05/01, 01/01, 02/01 là ngày dayoff => trả về 2)
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_COUNT_DAYOFF_TO_DATEHIRE.ToString()))
            {
                //ngày đầu tháng tính lương
                DateTime DateStart = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, 1);
                //nếu ngày làm việc lớn hơn ngày đầu tháng
                if (profileItem.DateHire != null && profileItem.DateHire >= DateStart)
                {
                    int DayNumber = TotalData.listDayOff.Where(m => m.DateOff <= profileItem.DateHire.Value && m.DateOff >= DateStart).Count();
                    item = new ElementFormula(PayrollElement.HR_COUNT_DAYOFF_TO_DATEHIRE.ToString(), DayNumber, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.HR_COUNT_DAYOFF_TO_DATEHIRE.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }
            }

            //Nhân viên có ngày vào làm hoặc ngày đi làm lại trong khoảng từ ngày 1 đến ngày chốt lương trong tháng tính lương thì trả về 1, nếu không trả về 0
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_IS_BACK_TO_WORK.ToString()))
            {
                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();
                //kiểm tra ngày đi làm
                if (profileItem.DateHire != null && profileItem.DateHire.Value.Month == CutOffDuration.MonthYear.Month && profileItem.DateHire.Value.Year == CutOffDuration.MonthYear.Year)
                {
                    if (CatGrade.SalaryDayClose != null && profileItem.DateHire.Value.Day <= CatGrade.SalaryDayClose)
                    {
                        item = new ElementFormula(PayrollElement.HR_IS_BACK_TO_WORK.ToString(), 1, 0);
                        listElementFormula.Add(item);
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.HR_IS_BACK_TO_WORK.ToString(), 0, 0);
                        listElementFormula.Add(item);
                    }
                }
                else//kiểm tra có ngày vào làm lại hay không
                {
                    if (CatGrade.SalaryDayClose != null)
                    {
                        List<Hre_WorkHistoryEntity> listWorkHistoryByProfile = TotalData.listWorkHistory.Where(m => m.ProfileID == profileItem.ID && m.DateComeBack != null && m.DateComeBack <= new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, (int)CatGrade.SalaryDayClose)).ToList();
                        if (listWorkHistoryByProfile.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.HR_IS_BACK_TO_WORK.ToString(), 1, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.HR_IS_BACK_TO_WORK.ToString(), 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.HR_IS_BACK_TO_WORK.ToString(), 0, 0);
                        listElementFormula.Add(item);
                    }
                }
            }

            //Nhân viên có trong  doanh sách kỷ luật trong tháng tính lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_NUMBER_DAY_BEFORE_WORK.ToString()))
            {
                double CountDay = 0;
                //nếu ngày vào làm lớn hơn
                if (profileItem.DateHire != null && profileItem.DateHire > CutOffDuration.DateStart && profileItem.DateHire <= CutOffDuration.DateEnd)
                {
                    DateTime dateStart = new DateTime(profileItem.DateHire.Value.Year, profileItem.DateHire.Value.Month, 1);
                    CountDay = profileItem.DateHire.Value.Subtract(dateStart).TotalDays;

                    int DayOff = TotalData.listDayOff.Where(m => ((m.OrgStructureID == null || m.OrgStructureID == profileItem.OrgStructureID) || m.OrgStructureID == profileItem.OrgStructureID) && m.DateOff >= dateStart && m.DateOff < profileItem.DateHire).Count();
                    CountDay -= DayOff;
                }

                //kiểm tra xem có vào làm lại hay không
                Hre_StopWorkingEntity StopWorkingByProfile = TotalData.listHre_StopWorking.Where(m => m.ProfileID != null && m.DateComeBack != null && m.ProfileID == profileItem.ID && m.DateComeBack >= CutOffDuration.DateStart && m.DateComeBack <= CutOffDuration.DateEnd).OrderByDescending(m => m.DateComeBack).FirstOrDefault();
                if (StopWorkingByProfile != null && StopWorkingByProfile.DateComeBack != null)
                {
                    DateTime dateStart = new DateTime(StopWorkingByProfile.DateComeBack.Value.Year, StopWorkingByProfile.DateComeBack.Value.Month, 1);
                    CountDay = StopWorkingByProfile.DateComeBack.Value.Subtract(dateStart).TotalDays;

                    int DayOff = TotalData.listDayOff.Where(m => (m.OrgStructureID == null || m.OrgStructureID == profileItem.OrgStructureID) && m.DateOff >= dateStart && m.DateOff < profileItem.DateHire).Count();
                    CountDay -= DayOff;
                }

                item = new ElementFormula(PayrollElement.HR_NUMBER_DAY_BEFORE_WORK.ToString(), CountDay, 0);
                listElementFormula.Add(item);
            }

            //Nhân viên có trong  doanh sách kỷ luật trong tháng tính lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_NUMBER_DAY_BEFORE_WORK_PREV.ToString()))
            {
                DateTime DateStartPrev = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndPrev = CutOffDuration.DateEnd.AddMonths(-1);
                double CountDay = 0;
                int DayClose = 1;

                //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();
                if (CatGrade != null && CatGrade.SalaryDayClose != null)
                {
                    DayClose = (int)CatGrade.SalaryDayClose;
                }

                //nếu ngày vào làm lớn hơn
                if (profileItem.DateHire != null && profileItem.DateHire > DateStartPrev && profileItem.DateHire <= DateEndPrev && profileItem.DateHire.Value.Day > DayClose)
                {
                    CountDay = new DateTime(profileItem.DateHire.Value.Year, profileItem.DateHire.Value.Month, 1).AddMonths(1).AddDays(-1).Subtract(profileItem.DateHire.Value).TotalDays + 1;
                    DateStartPrev = new DateTime(profileItem.DateHire.Value.Year, profileItem.DateHire.Value.Month, 1).AddMonths(1).AddDays(-1);
                    int DayOff = TotalData.listDayOff.Where(m => (m.OrgStructureID == null || m.OrgStructureID == profileItem.OrgStructureID) && m.DateOff <= DateStartPrev && m.DateOff >= profileItem.DateHire).Count();
                    CountDay -= DayOff;
                }

                //kiểm tra xem có vào làm lại hay không
                Hre_StopWorkingEntity StopWorkingByProfile = TotalData.listHre_StopWorking.Where(m => m.ProfileID != null && m.DateComeBack != null && m.ProfileID == profileItem.ID && m.DateComeBack >= DateStartPrev && m.DateComeBack <= DateEndPrev).OrderByDescending(m => m.DateComeBack).FirstOrDefault();
                if (StopWorkingByProfile != null && StopWorkingByProfile.DateComeBack != null)
                {
                    CountDay = new DateTime(StopWorkingByProfile.DateComeBack.Value.Year, StopWorkingByProfile.DateComeBack.Value.Month, 1).AddMonths(1).AddDays(-1).Subtract(StopWorkingByProfile.DateComeBack.Value).TotalDays + 1;
                    DateStartPrev = new DateTime(profileItem.DateHire.Value.Year, profileItem.DateHire.Value.Month, 1).AddMonths(1).AddDays(-1);
                    int DayOff = TotalData.listDayOff.Where(m => (m.OrgStructureID == null || m.OrgStructureID == profileItem.OrgStructureID) && m.DateOff <= DateStartPrev && m.DateOff >= profileItem.DateHire).Count();
                    CountDay -= DayOff;
                }

                item = new ElementFormula(PayrollElement.HR_NUMBER_DAY_BEFORE_WORK_PREV.ToString(), CountDay, 0);
                listElementFormula.Add(item);
            }


            //Nhân viên có trong  doanh sách kỷ luật trong tháng tính lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_IS_DISCIPLINE.ToString()))//đã lấy lên chưa ?
            {
                DateTime datefrom = new DateTime(CutOffDuration.MonthYear.Year - 1, 4, 1);
                DateTime dateto = new DateTime(CutOffDuration.MonthYear.Year, 3, 31);
                var listDisciplineProfile = TotalData.listDiscipline.Where(m => m.ProfileID == profileItem.ID && m.DateOfEffective >= datefrom && m.DateOfEffective <= dateto).FirstOrDefault();

                item = new ElementFormula(PayrollElement.HR_IS_DISCIPLINE.ToString(), listDisciplineProfile != null ? 1 : 0, 0);
                listElementFormula.Add(item);
            }

            //Phần tử tổng thời gian tạm hoãn công việc tính tới cuối kỳ lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_TOTAL_DAY_STOP_WORKING.ToString()))//đã lấy lên chưa ?
            {
                item = new ElementFormula(PayrollElement.HR_TOTAL_DAY_STOP_WORKING.ToString(), SumStopWorkingDay(TotalData.listHre_StopWorking.Where(m => m.ProfileID == profileItem.ID).ToList(), CutOffDuration.DateEnd), 0);
                listElementFormula.Add(item);
            }

            //Nhân viên có được tính trợ cấp hay không (Có ngày vào làm từ 1996<=N<=31/12/2008)
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_IS_COMPUTE_SUBSIDIZE.ToString()))
            {
                DateTime from = new DateTime(1996, 1, 1);
                DateTime to = new DateTime(2008, 12, 31);
                item = new ElementFormula(PayrollElement.HR_IS_COMPUTE_SUBSIDIZE.ToString(), (profileItem.DateHire <= to && profileItem.DateHire >= from) == true ? 1 : 0, 0);
                listElementFormula.Add(item);
            }

            //Lấy thông tin hợp đồng
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_CONSTRACT_TYPE.ToString()))
            {
                //TotalData.
            }

            #region HDT JOB

            #region Ngày vào làm và ngày kết thúc HDT JOB tháng N
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_START_DATE_HDTJOB.ToString()) || CheckIsExistFormula(listElementFormula, formula, PayrollElement.HR_END_DATE_HDTJOB.ToString()))//đã lấy lên chưa ?
            {
                Hre_HDTJobEntity HDTJOB_DateFrom = new Hre_HDTJobEntity();
                //Ngày vào làm HDT JOB
                HDTJOB_DateFrom = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.DateFrom <= CutOffDuration.DateEnd && (m.DateTo >= CutOffDuration.DateStart || m.DateTo == null) && m.Status == HDTJobStatus.E_APPROVE.ToString()).OrderBy(m => m.DateFrom).FirstOrDefault();
                if (HDTJOB_DateFrom != null)
                {
                    DateTime form = HDTJOB_DateFrom.DateFrom != null ? (DateTime)HDTJOB_DateFrom.DateFrom : DateTime.MinValue;
                    DateTime to = HDTJOB_DateFrom.DateTo != null ? (DateTime)HDTJOB_DateFrom.DateTo : CutOffDuration.DateEnd;

                    item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB.ToString(), form, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB.ToString(), to, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB.ToString(), DateTime.MinValue, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB.ToString(), DateTime.MinValue, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            #endregion

            #region Ngày vào làm và ngày kết thúc HDT JOB tháng N-1
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.HR_END_DATE_HDTJOB_PREV.ToString(), PayrollElement.HR_START_DATE_HDTJOB_PREV.ToString() }))
            {
                var CutOffDuration_Prev = TotalData.listCutOffDuration.Where(m => m.MonthYear == CutOffDuration.MonthYear.AddMonths(-1)).OrderByDescending(m => m.MonthYear).FirstOrDefault();

                if (CutOffDuration_Prev != null)
                {
                    List<Hre_HDTJobEntity> listHre_HDTJob_Prev = new List<Hre_HDTJobEntity>();
                    Hre_HDTJobEntity HDTJOB_DateFrom = new Hre_HDTJobEntity();
                    //Ngày vào làm HDT JOB
                    HDTJOB_DateFrom = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.DateFrom <= CutOffDuration_Prev.DateEnd && (m.DateTo >= CutOffDuration_Prev.DateStart || m.DateTo == null) && m.Status == HDTJobStatus.E_APPROVE.ToString()).OrderBy(m => m.DateFrom).FirstOrDefault();
                    if (HDTJOB_DateFrom != null)
                    {
                        DateTime form = HDTJOB_DateFrom.DateFrom != null ? (DateTime)HDTJOB_DateFrom.DateFrom : DateTime.MinValue;
                        DateTime to = HDTJOB_DateFrom.DateTo != null ? (DateTime)HDTJOB_DateFrom.DateTo : CutOffDuration.DateEnd;

                        item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB_PREV.ToString(), form, 0);
                        listElementFormula.Add(item);

                        item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB_PREV.ToString(), to, 0);
                        listElementFormula.Add(item);
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB_PREV.ToString(), DateTime.MinValue, 0, "Null");
                        listElementFormula.Add(item);

                        item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB_PREV.ToString(), DateTime.MinValue, 0, "Null");
                        listElementFormula.Add(item);
                    }
                }
                else
                {
                    item = new ElementFormula(PayrollElement.HR_START_DATE_HDTJOB_PREV.ToString(), DateTime.MinValue, 0, "Không tồn tại kỳ N-1");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.HR_END_DATE_HDTJOB_PREV.ToString(), DateTime.MinValue, 0, "Không tồn tại kỳ N-1");
                    listElementFormula.Add(item);
                }

            }


            #endregion

            #region Tính số ngày công đi làm HDT JOB Tháng N

            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.ATT_WORKDAY_HDTJOB_4.ToString()) || CheckIsExistFormula(listElementFormula, formula, PayrollElement.ATT_WORKDAY_HDTJOB_5.ToString()))//đã lấy lên chưa ?
            {
                double HDTJobDay = 0;
                if (listAttendanceTableProCut != null)
                {
                    if (listAttendanceTableProCut.HDTJobType1 != null && listAttendanceTableProCut.HDTJobType1 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount1 != null ? (int)listAttendanceTableProCut.HDTJobDayCount1 : 0;
                    }
                    if (listAttendanceTableProCut.HDTJobType2 != null && listAttendanceTableProCut.HDTJobType2 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount2 != null ? (int)listAttendanceTableProCut.HDTJobDayCount2 : 0;
                    }
                    if (listAttendanceTableProCut.HDTJobType3 != null && listAttendanceTableProCut.HDTJobType3 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount3 != null ? (int)listAttendanceTableProCut.HDTJobDayCount3 : 0;
                    }
                }

                //Số ngày công làm HDT Job Loại 4 (tháng N)
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_4.ToString(), HDTJobDay, 0);
                listElementFormula.Add(item);

                HDTJobDay = 0;
                if (listAttendanceTableProCut != null)
                {
                    if (listAttendanceTableProCut.HDTJobType1 != null && listAttendanceTableProCut.HDTJobType1 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount1 != null ? (int)listAttendanceTableProCut.HDTJobDayCount1 : 0;
                    }
                    if (listAttendanceTableProCut.HDTJobType2 != null && listAttendanceTableProCut.HDTJobType2 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount2 != null ? (int)listAttendanceTableProCut.HDTJobDayCount2 : 0;
                    }
                    if (listAttendanceTableProCut.HDTJobType3 != null && listAttendanceTableProCut.HDTJobType3 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                    {
                        HDTJobDay += listAttendanceTableProCut.HDTJobDayCount3 != null ? (int)listAttendanceTableProCut.HDTJobDayCount3 : 0;
                    }
                }

                //Số ngày công làm HDT Job Loại 5 (tháng N)
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_5.ToString(), HDTJobDay, 0);
                listElementFormula.Add(item);
            }

            #endregion

            #region Tính số ngày công đi làm HDT JOB Tháng N-1

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKDAY_HDTJOB_PREV_4.ToString(), PayrollElement.ATT_WORKDAY_HDTJOB_PREV_5.ToString() }))
            {
                //lấy dữ liệu công tháng N-1
                var listAttendanceTableProCut_Prev = TotalData.Att_AttendanceTable_Prev.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault();

                double HDTJobDay = 0;
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType1 != null && listAttendanceTableProCut_Prev.HDTJobType1 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount1 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount1 : 0;
                }
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType2 != null && listAttendanceTableProCut_Prev.HDTJobType2 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount2 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount2 : 0;
                }
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType3 != null && listAttendanceTableProCut_Prev.HDTJobType3 == EnumDropDown.HDTJobType.E_TYPE4.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount3 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount3 : 0;
                }

                //Số ngày công làm HDT Job Loại 4 (tháng N)
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_PREV_4.ToString(), HDTJobDay, 0);
                listElementFormula.Add(item);

                HDTJobDay = 0;
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType1 != null && listAttendanceTableProCut_Prev.HDTJobType1 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount1 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount1 : 0;
                }
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType2 != null && listAttendanceTableProCut_Prev.HDTJobType2 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount2 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount2 : 0;
                }
                if (listAttendanceTableProCut_Prev != null && listAttendanceTableProCut_Prev.HDTJobType3 != null && listAttendanceTableProCut_Prev.HDTJobType3 == EnumDropDown.HDTJobType.E_TYPE5.ToString())
                {
                    HDTJobDay += listAttendanceTableProCut_Prev.HDTJobDayCount3 != null ? (int)listAttendanceTableProCut_Prev.HDTJobDayCount3 : 0;
                }

                //Số ngày công làm HDT Job Loại 5 (tháng N)
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDTJOB_PREV_5.ToString(), HDTJobDay, 0);
                listElementFormula.Add(item);
            }

            #endregion

            #region Phần tử kiểm tra có ngày ra HDT hay không tháng N và tháng N-1

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_HDT_IS_DATE_END.ToString(), PayrollElement.ATT_HDT_IS_DATE_END_N_1.ToString() }))
            {


                #region Tháng N
                List<Hre_HDTJobEntity> HDTJOBByProfile = new List<Hre_HDTJobEntity>();
                HDTJOBByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.DateFrom <= CutOffDuration.DateEnd && (m.DateTo >= CutOffDuration.DateStart || m.DateTo == null) && m.Status == HDTJobStatus.E_APPROVE.ToString()).ToList();
                if (HDTJOBByProfile.Any(m => m.DateTo == null))
                {
                    item = new ElementFormula(PayrollElement.ATT_HDT_IS_DATE_END.ToString(), 1, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_HDT_IS_DATE_END.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }
                #endregion

                #region Tháng N-1

                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);

                HDTJOBByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.DateFrom <= DateEndN1 && (m.DateTo >= DateStartN1 || m.DateTo == null) && m.Status == HDTJobStatus.E_APPROVE.ToString()).ToList();
                if (HDTJOBByProfile.Any(m => m.DateTo == null))
                {
                    item = new ElementFormula(PayrollElement.ATT_HDT_IS_DATE_END.ToString(), 1, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_HDT_IS_DATE_END.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }
                #endregion


            }

            #endregion

            #region Số ngày làm HDT loại 4 và 5 trừ ngày DayOff (tháng N và tháng N-1)
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_4.ToString(), PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_5.ToString(), PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_4_N_1.ToString(), PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_5_N_1.ToString() }))
            {
                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);

                List<Hre_HDTJobEntity> ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString()).ToList();
                List<Hre_HDTJobEntity> ListHDTByProfileN = ListHDTByProfile.Where(m => m.DateFrom <= CutOffDuration.DateEnd && (m.DateTo >= CutOffDuration.DateStart || m.DateTo == null)).ToList();
                List<Hre_HDTJobEntity> ListHDTByProfileN1 = ListHDTByProfile.Where(m => m.DateFrom <= DateEndN1 && (m.DateTo >= DateStartN1 || m.DateTo == null)).ToList();

                double ListDayOff = TotalData.listDayOff.Count(m => m.DateOff <= CutOffDuration.DateEnd && m.DateOff >= CutOffDuration.DateStart);
                double ListDayOffN1 = TotalData.listDayOff.Count(m => m.DateOff <= DateEndN1 && m.DateOff >= DateStartN1);

                #region tính số ngày làm HDT loại 4
                List<Hre_HDTJobEntity> ListHDTByProfileN_Type4 = ListHDTByProfileN.Where(m => m.Type == EnumDropDown.HDTJobType.E_TYPE4.ToString()).ToList();
                double Day_Type4 = 0;
                DateTime _tmp = new DateTime();
                foreach (var i in ListHDTByProfileN_Type4)
                {
                    _tmp = i.DateTo != null ? i.DateTo.Value : CutOffDuration.DateEnd;
                    if (i.DateFrom != null)
                    {
                        Day_Type4 += _tmp.Subtract(i.DateFrom.Value).TotalDays + 1;
                    }
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_4.ToString(), Day_Type4, 0);
                listElementFormula.Add(item);
                #endregion

                #region tính số ngày làm HDT loại 5
                List<Hre_HDTJobEntity> ListHDTByProfileN_Type5 = ListHDTByProfileN.Where(m => m.Type == EnumDropDown.HDTJobType.E_TYPE5.ToString()).ToList();
                double Day_Type5 = 0;
                foreach (var i in ListHDTByProfileN_Type5)
                {
                    _tmp = i.DateTo != null ? i.DateTo.Value : CutOffDuration.DateEnd;
                    if (i.DateFrom != null)
                    {
                        Day_Type5 += _tmp.Subtract(i.DateFrom.Value).TotalDays + 1;
                    }
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_5.ToString(), Day_Type5, 0);
                listElementFormula.Add(item);
                #endregion

                #region tính số ngày làm HDT loại 4 tháng N-1
                ListHDTByProfileN_Type4 = ListHDTByProfileN1.Where(m => m.Type == EnumDropDown.HDTJobType.E_TYPE4.ToString()).ToList();
                Day_Type4 = 0;
                _tmp = new DateTime();
                foreach (var i in ListHDTByProfileN_Type4)
                {
                    _tmp = i.DateTo != null ? i.DateTo.Value : CutOffDuration.DateEnd;
                    if (i.DateFrom != null)
                    {
                        Day_Type4 += _tmp.Subtract(i.DateFrom.Value).TotalDays + 1;
                    }
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_4_N_1.ToString(), Day_Type4, 0);
                listElementFormula.Add(item);
                #endregion

                #region tính số ngày làm HDT loại 5 tháng N-1
                ListHDTByProfileN_Type5 = ListHDTByProfileN1.Where(m => m.Type == EnumDropDown.HDTJobType.E_TYPE5.ToString()).ToList();
                Day_Type5 = 0;
                foreach (var i in ListHDTByProfileN_Type5)
                {
                    _tmp = i.DateTo != null ? i.DateTo.Value : CutOffDuration.DateEnd;
                    if (i.DateFrom != null)
                    {
                        Day_Type5 += _tmp.Subtract(i.DateFrom.Value).TotalDays + 1;
                    }
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_HDT_NOT_DAYOFF_5_N_1.ToString(), Day_Type5, 0);
                listElementFormula.Add(item);
                #endregion

            }
            #endregion

            #region Số ngày Day Off từ đầu tháng đến ngày vào HDT

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT.ToString(), PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT_N_1.ToString() }))
            {
                #region Tháng N
                Hre_HDTJobEntity ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).OrderBy(m => m.DateFrom).FirstOrDefault();

                if (ListHDTByProfile != null)
                {
                    double DayOff = TotalData.listDayOff.Count(m => m.DateOff <= ListHDTByProfile.DateFrom && m.DateOff >= CutOffDuration.DateStart);
                    item = new ElementFormula(PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT.ToString(), DayOff, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                }
                #endregion

                #region Tháng N-1
                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);
                ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN1 && m.DateTo >= DateStartN1).OrderBy(m => m.DateFrom).FirstOrDefault();

                if (ListHDTByProfile != null)
                {
                    double DayOff = TotalData.listDayOff.Count(m => m.DateOff <= ListHDTByProfile.DateFrom && m.DateOff >= DateStartN1);
                    item = new ElementFormula(PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT_N_1.ToString(), DayOff, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_DAYOFF_STARTMONTH_STARTHDT_N_1.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                }
                #endregion
            }

            #endregion

            #region số ngày từ ngày vào hdt đến cuối tháng trừ số ngày dayoff từ ngày vào đến cuối tháng(tháng N và tháng N-1)

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND.ToString(), PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND_N_1.ToString() }))
            {
                double WorkDay = 0;
                double DayOff = 0;
                #region Tháng N
                List<Hre_HDTJobEntity> ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).OrderBy(m => m.DateFrom).ToList();

                if (ListHDTByProfile.Count > 0)
                {
                    foreach (var i in ListHDTByProfile)
                    {
                        if (i.DateFrom != null && i.DateTo != null)
                        {
                            if (i.DateTo < CutOffDuration.DateEnd)
                            {
                                WorkDay += i.DateTo.Value.Subtract(i.DateFrom.Value).TotalDays + 1;
                            }
                            else
                            {
                                WorkDay += CutOffDuration.DateEnd.Subtract(i.DateFrom.Value).TotalDays + 1;
                            }
                        }
                    }
                    DayOff = TotalData.listDayOff.Count(m => m.DateOff >= ListHDTByProfile.FirstOrDefault().DateFrom && m.DateOff <= CutOffDuration.DateEnd);
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND.ToString(), WorkDay - DayOff, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                }
                #endregion

                #region Tháng N-1
                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);
                WorkDay = 0;
                DayOff = 0;
                ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN1 && m.DateTo >= DateStartN1).OrderBy(m => m.DateFrom).ToList();

                if (ListHDTByProfile.Count > 0)
                {
                    foreach (var i in ListHDTByProfile)
                    {
                        if (i.DateFrom != null && i.DateTo != null)
                        {
                            if (i.DateTo < CutOffDuration.DateEnd)
                            {
                                WorkDay += i.DateTo.Value.Subtract(i.DateFrom.Value).TotalDays + 1;
                            }
                            else
                            {
                                WorkDay += CutOffDuration.DateEnd.Subtract(i.DateFrom.Value).TotalDays + 1;
                            }
                        }
                    }
                    DayOff = TotalData.listDayOff.Count(m => m.DateOff >= ListHDTByProfile.FirstOrDefault().DateFrom && m.DateOff <= CutOffDuration.DateEnd);
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND_N_1.ToString(), WorkDay - DayOff, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_MONTHEND_N_1.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                }

                #endregion
            }

            #endregion

            #region số ngày từ ngày vào hdt tháng N-1 đến ngày ra hdt tháng N-1 trừ ngày dayoff tháng N-1 và N-2
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_1.ToString(), PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_2.ToString() }))
            {
                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);
                DateTime DateStartN2 = CutOffDuration.DateStart.AddMonths(-2);
                DateTime DateEndN2 = CutOffDuration.DateEnd.AddMonths(-2);
                double workDayHDT = 0;
                double DayOff = TotalData.listDayOff.Count(m => m.DateOff <= DateEndN1 && m.DateOff >= DateStartN1);

                #region Tháng N-1
                List<Hre_HDTJobEntity> ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN1 && m.DateTo >= DateStartN1).OrderBy(m => m.DateFrom).ToList();
                foreach (var i in ListHDTByProfile)
                {
                    workDayHDT += i.DateTo.Value.Subtract(i.DateFrom.Value).TotalDays + 1;
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_1.ToString(), workDayHDT - DayOff, 0);
                listElementFormula.Add(item);

                #endregion

                #region Tháng N-2
                workDayHDT = 0;
                DayOff = TotalData.listDayOff.Count(m => m.DateOff <= DateEndN2 && m.DateOff >= DateStartN2);
                ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN2 && m.DateTo >= DateStartN2).OrderBy(m => m.DateFrom).ToList();
                foreach (var i in ListHDTByProfile)
                {
                    workDayHDT += i.DateTo.Value.Subtract(i.DateFrom.Value).TotalDays + 1;
                }
                item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_2.ToString(), workDayHDT - DayOff, 0);
                listElementFormula.Add(item);
                #endregion

            }
            #endregion

            #region Ngày vào HDT tháng N-1 và N-2

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_STARTDATE_HDT_N_1.ToString(), PayrollElement.ATT_STARTDATE_HDT_N_2.ToString() }))
            {
                DateTime DateStartN1 = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEndN1 = CutOffDuration.DateEnd.AddMonths(-1);
                DateTime DateStartN2 = CutOffDuration.DateStart.AddMonths(-2);
                DateTime DateEndN2 = CutOffDuration.DateEnd.AddMonths(-2);

                Hre_HDTJobEntity ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN1 && m.DateTo >= DateStartN1).OrderBy(m => m.DateFrom).FirstOrDefault();
                if (ListHDTByProfile != null)
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_1.ToString(), ListHDTByProfile.DateFrom, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_1.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }

                ListHDTByProfile = TotalData.listHre_HDTJob_All.Where(m => m.ProfileID == profileItem.ID && m.Status == HDTJobStatus.E_APPROVE.ToString() && m.DateTo != null && m.DateFrom != null && m.DateFrom <= DateEndN2 && m.DateTo >= DateStartN2).OrderBy(m => m.DateFrom).FirstOrDefault();
                if (ListHDTByProfile != null)
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_2.ToString(), ListHDTByProfile.DateFrom, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKDAY_STARTHDT_ENDHDT_N_2.ToString(), 0, 0);
                    listElementFormula.Add(item);
                }


            }

            #endregion

            #endregion

            #endregion

            #region Enum phần tử lương
            //Có tham gia công đoàn
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.IS_HRE_TRADEUNION.ToString()))
            {
                var ProfilePartyUnion = TotalData.listProfilePartyUnion.Where(m => m.ProfileID == profileItem.ID && m.IsTradeUnionist == true && m.TradeUnionistEnrolledDate != null && m.TradeUnionistEnrolledDate.Value <= CutOffDuration.DateEnd).FirstOrDefault();

                item = new ElementFormula(PayrollElement.IS_HRE_TRADEUNION.ToString(), ProfilePartyUnion != null ? 1 : 0, 0);
                listElementFormula.Add(item);
            }

            //Người phụ thuộc
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_DEPENDENT.ToString()))
            {
                item = new ElementFormula(PayrollElement.SAL_DEPENDENT.ToString(), GetDependantNumber(TotalData.listDependant, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd), 0);
                listElementFormula.Add(item);
            }

            //Mức lương HDT
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_SALARY_HDT.ToString(), PayrollElement.SAL_SALARY_HDT_N_1.ToString() }))
            {
                var Insurence = TotalData.listInsurance.Where(m => m.ProfileID == profileItem.ID && m.MonthYear != null && m.MonthYear.Value.Year == CutOffDuration.MonthYear.Year && m.MonthYear.Value.Month == CutOffDuration.MonthYear.Month).FirstOrDefault();
                item = new ElementFormula(PayrollElement.SAL_SALARY_HDT.ToString(), Insurence != null && Insurence.AmountChargeIns != null ? Insurence.AmountChargeIns : 0, 0);
                listElementFormula.Add(item);

                DateTime MonthYearPrev = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CutOffDuration.MonthYear.Day).AddMonths(-1);
                Insurence = TotalData.listInsurance.Where(m => m.ProfileID == profileItem.ID && m.MonthYear != null && m.MonthYear.Value.Year == MonthYearPrev.Year && m.MonthYear.Value.Month == MonthYearPrev.Month).FirstOrDefault();
                item = new ElementFormula(PayrollElement.SAL_SALARY_HDT_N_1.ToString(), Insurence != null && Insurence.AmountChargeIns != null ? Insurence.AmountChargeIns : 0, 0);
                listElementFormula.Add(item);
            }

            //Luong co ban thang 3
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_BASIC_SALARY_T3.ToString()))
            {
                item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_T3.ToString(), TotalData.listBasicSalaryT3.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).FirstOrDefault().GrossAmount, 0);
                listElementFormula.Add(item);
            }

            //Bậc lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_SALARY_RANK_NAME.ToString()))
            {
                Sal_BasicSalaryEntity BasicSalarybyProfile = new Sal_BasicSalaryEntity();
                BasicSalarybyProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).FirstOrDefault();
                item = new ElementFormula(PayrollElement.SAL_SALARY_RANK_NAME.ToString(), BasicSalarybyProfile != null ? BasicSalarybyProfile.SalaryRankName : "", 0);
                listElementFormula.Add(item);
            }

            //Bậc lương (class)
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_SALARY_CLASS_NAME.ToString()))
            {
                Sal_BasicSalaryEntity BasicSalarybyProfile = new Sal_BasicSalaryEntity();
                BasicSalarybyProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).FirstOrDefault();
                item = new ElementFormula(PayrollElement.SAL_SALARY_CLASS_NAME.ToString(), BasicSalarybyProfile != null ? BasicSalarybyProfile.SalaryClassName : "", 0);
                listElementFormula.Add(item);
            }

            //Hệ số lương nhân viên
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_BASIC_PERSONALRATE.ToString()))
            {
                List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();
                item = new ElementFormula(PayrollElement.SAL_BASIC_PERSONALRATE.ToString(), SalaryProfile.FirstOrDefault().PersonalRate != null ? SalaryProfile.FirstOrDefault().PersonalRate : 0, 0, "Null");
                listElementFormula.Add(item);
            }

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_BASIC_SALARY_DATECLOSE.ToString(), PayrollElement.SAL_BASIC_SALARY_DATECLOSE_N_1.ToString() }))
            {
                List<Sal_BasicSalaryEntity> ListSalaryProfile = new List<Sal_BasicSalaryEntity>();
                ListSalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();
                if (ListSalaryProfile.Count > 0)
                {
                    //Lấy các phần tử tính lương nằm trong Grade của nhân viên
                    Sal_GradeEntity Grade = FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd);
                    //loại bỏ nhân viên có ngày vào làm sau ngày chốt lương
                    Cat_GradePayrollEntity CatGrade = TotalData.listCat_GradePayroll.Where(m => m.ID == Grade.GradePayrollID).FirstOrDefault();

                    //ngày bắt đầu chốt lương
                    DateTime DateClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1).AddDays(1).AddMonths(-1);
                    //ngày kết thúc chốt lương
                    DateTime DateEndClose = new DateTime(CutOffDuration.MonthYear.Year, CutOffDuration.MonthYear.Month, CatGrade.SalaryDayClose != null ? (int)CatGrade.SalaryDayClose : 1);

                    //lọc lại lương cơ bản theo kỳ chốt
                    ListSalaryProfile = ListSalaryProfile.Where(m => m.DateOfEffect <= DateEndClose).ToList();

                    //lương cơ bản gần nhất
                    Sal_BasicSalaryEntity SalaryProfile = ListSalaryProfile.FirstOrDefault();

                    //nếu ngày thay đổi lương nằm trong kỳ chốt lương thì lấy 2 mức
                    if (SalaryProfile.DateOfEffect >= DateClose && SalaryProfile.DateOfEffect <= DateEndClose)
                    {
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE.ToString(), SalaryProfile.GrossAmount, 0);
                        listElementFormula.Add(item);

                        Sal_BasicSalaryEntity SalaryProfile_Prev = ListSalaryProfile.Where(m => m.DateOfEffect < DateClose).OrderByDescending(m => m.DateOfEffect).FirstOrDefault();
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE_N_1.ToString(), SalaryProfile_Prev != null ? SalaryProfile_Prev.GrossAmount : "0", 0);
                        listElementFormula.Add(item);
                    }
                    else//chỉ áp dụng 1 mức lương
                    {
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE.ToString(), SalaryProfile.GrossAmount, 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE_N_1.ToString(), SalaryProfile.GrossAmount, 0);
                        listElementFormula.Add(item);
                    }
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE.ToString(), 0, 0, "Không có lương cơ bản !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_DATECLOSE_N_1.ToString(), 0, 0, "Không có thay đổi lương trong tháng !");
                    listElementFormula.Add(item);
                }
            }

            //Lương cơ bản
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_BASIC_SALARY.ToString(), PayrollElement.HR_SALARYCLASSNAME.ToString(), PayrollElement.SAL_BASIC_SALARY2.ToString(), PayrollElement.SAL_BASIC_SALARY1.ToString(), PayrollElement.SAL_BASIC_SALARY_N_1.ToString(), PayrollElement.SAL_BASIC_SALARY_N_2.ToString(), PayrollElement.SAL_BASIC_SALARY_N_3.ToString(), PayrollElement.SAL_BASIC_SALARY_N_4.ToString(), PayrollElement.SAL_BASIC_SALARY_N_5.ToString(), PayrollElement.SAL_BASIC_SALARY_N_6.ToString(), PayrollElement.SAL_INCENTIVE.ToString() }))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                    SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();
                    if (SalaryProfile != null && SalaryProfile.Count > 0)//có lương cơ bản
                    {
                        //bật lương
                        item = new ElementFormula(PayrollElement.HR_SALARYCLASSNAME.ToString(), SalaryProfile.FirstOrDefault().SalaryClassCode, 0, "Null");
                        listElementFormula.Add(item);

                        //lương cơ bản tháng hiện tại
                        if (SalaryProfile.FirstOrDefault().DateOfEffect <= CutOffDuration.DateStart)//chỉ có 1 mức lương trong tháng
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY.ToString(), SalaryProfile.OrderByDescending(m => m.DateOfEffect).FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                            item = new ElementFormula(PayrollElement.SAL_INCENTIVE.ToString(), 0, 0);
                            listElementFormula.Add(item);
                        }
                        else//2 mức lương trong tháng
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);

                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY2.ToString(), SalaryProfile.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                            if (SalaryProfile.Count > 1)
                            {
                                item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY1.ToString(), SalaryProfile[1].GrossAmount, 0);
                            }
                            else
                            {
                                item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY1.ToString(), 0, 0, "Null");
                            }

                            listElementFormula.Add(item);

                            #region Tính số ngày thay đổi lương
                            List<Att_RosterEntity> listRosterProfile = new List<Att_RosterEntity>();
                            //Lọc ra các roster thuộc nhân viên và nằm trong tháng tính lương
                            listRosterProfile = TotalData.listRoster.Where(m => m.ProfileID == profileItem.ID).ToList();

                            int totalLeave = 0;
                            foreach (var j in listRosterProfile)
                            {
                                DateTime _tmp = j.DateStart;
                                while (true)
                                {
                                    if (_tmp > CutOffDuration.DateEnd)
                                    {
                                        break;
                                    }
                                    if (_tmp >= SalaryProfile.FirstOrDefault().DateOfEffect)
                                    {
                                        int day = (int)_tmp.DayOfWeek;
                                        switch (day)
                                        {
                                            case 0://CN
                                                if (j.SunShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 1://T2
                                                if (j.MonShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 2:
                                                if (j.TueShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 3:
                                                if (j.WedShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 4:
                                                if (j.ThuShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 5:
                                                if (j.FriShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            case 6:
                                                if (j.SatShiftID != null)
                                                {
                                                    totalLeave++;
                                                }
                                                break;
                                            default:

                                                break;
                                        }
                                    }
                                    _tmp = _tmp.AddDays(1);
                                }
                            }
                            //cập nhật lại giá trị cho enum số ngày thay đổi lương
                            int days = (CutOffDuration.DateEnd - SalaryProfile.FirstOrDefault().DateOfEffect).Days;
                            days = days - totalLeave;
                            item = new ElementFormula(PayrollElement.SAL_INCENTIVE.ToString(), days, 0);
                            listElementFormula.Add(item);
                            //item = listElementFormula.Where(m => m.VariableName == PayrollElement.SAL_INCENTIVE.ToString()).FirstOrDefault();
                            //item.Value = days;
                            #endregion
                        }

                        #region lương cơ bản 6 tháng trước đó
                        var _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-1)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_1.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_1.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-2)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_2.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_2.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-3)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_3.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_3.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-4)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_4.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_4.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-5)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_5.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_5.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        _basicsalaryPrevCurrentMonth = SalaryProfile.Where(m => m.DateOfEffect <= CutOffDuration.DateEnd.AddMonths(-6)).OrderByDescending(m => m.DateOfEffect).ToList();
                        if (_basicsalaryPrevCurrentMonth.Count > 0)
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_6.ToString(), _basicsalaryPrevCurrentMonth.FirstOrDefault().GrossAmount, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_6.ToString(), 0, 0, "Null");
                            listElementFormula.Add(item);
                        }
                        #endregion
                    }
                    else//không có lương cơ bản
                    {
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY.ToString(), 0, 0, "Không có lương cơ bản tháng N");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_1.ToString(), 0, 0, "Không có lương cơ bản tháng N-1");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_2.ToString(), 0, 0, "Không có lương cơ bản tháng N-2");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_3.ToString(), 0, 0, "Không có lương cơ bản tháng N-3");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_4.ToString(), 0, 0, "Không có lương cơ bản tháng N-4");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_5.ToString(), 0, 0, "Không có lương cơ bản tháng N-5");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY_N_6.ToString(), 0, 0, "Không có lương cơ bản tháng N-6");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY1.ToString(), 0, 0, "Không có lương cơ bản tháng N");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_BASIC_SALARY2.ToString(), 0, 0, "Không có lương cơ bản tháng N");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.SAL_INCENTIVE.ToString(), 0, 0, "Không có thay đổi lương tháng N");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.HR_SALARYCLASSNAME.ToString(), 0, 0, "Không có Bậc / Hệ số lương tháng N");
                        listElementFormula.Add(item);
                    }
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_UNUSUALALLOWANCE_MONTHSTART.ToString(), PayrollElement.SAL_UNUSUALALLOWANCE_MONTHEND.ToString(), PayrollElement.SAL_UNUSUALALLOWANCE_YEARSTART.ToString(), PayrollElement.SAL_UNUSUALALLOWANCE_YEAREND.ToString(), PayrollElement.SAL_UNUSUALALLOWANCE_NOCOMPENSATION.ToString() }))
            {
                Sal_UnusualAllowanceEntity SalUnusualAllowanceProfile = TotalData.listSalUnusualAllowance.Where(m => m.ProfileID == profileItem.ID && m.MonthStart <= CutOffDuration.DateEnd && (m.MonthEnd == null || m.MonthEnd >= CutOffDuration.DateStart)).FirstOrDefault();

                //Tháng bắt đầu hưởng PC
                item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_MONTHSTART.ToString(), SalUnusualAllowanceProfile != null ? SalUnusualAllowanceProfile.MonthStart != null ? SalUnusualAllowanceProfile.MonthStart : DateTime.MinValue : DateTime.MinValue, 0);
                listElementFormula.Add(item);

                //Tháng kết thúc hưởng PC
                item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_MONTHEND.ToString(), SalUnusualAllowanceProfile != null ? SalUnusualAllowanceProfile.MonthEnd != null ? SalUnusualAllowanceProfile.MonthEnd : DateTime.MinValue : DateTime.MinValue, 0);
                listElementFormula.Add(item);

                //Năm bắt đầu hưởng PC
                item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_YEARSTART.ToString(), SalUnusualAllowanceProfile != null ? SalUnusualAllowanceProfile.MonthStart != null ? SalUnusualAllowanceProfile.MonthStart.Value.Year : 0 : 0, 0);
                listElementFormula.Add(item);

                //Năm kết thúc hưởng PC
                item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_YEAREND.ToString(), SalUnusualAllowanceProfile != null ? SalUnusualAllowanceProfile.MonthEnd != null ? SalUnusualAllowanceProfile.MonthEnd.Value.Year : 0 : 0, 0);
                listElementFormula.Add(item);

                //Số tháng bù
                item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_NOCOMPENSATION.ToString(), SalUnusualAllowanceProfile != null ? SalUnusualAllowanceProfile.NoCompensation != null ? SalUnusualAllowanceProfile.NoCompensation : 0 : 0, 0);
                listElementFormula.Add(item);
            }

            //// Mức phụ cấp con nhỏ mỗi tháng
            //item = new ElementFormula(PayrollElement.SAL_UNUSUALALLOWANCE_NOCOMPENSATION.ToString(), 0, 0);
            //listElementFormula.Add(item);

            //Thánh tính lương
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.ATT_CUTOFFDURATION_MONTH.ToString()))
            {
                item = new ElementFormula(PayrollElement.ATT_CUTOFFDURATION_MONTH.ToString(), CutOffDuration.DateStart, 0);
                listElementFormula.Add(item);
            }

            //Tổng lương bộ phận của nhân viên
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_SALARY_DEPARTMENT.ToString()))
            {
                if (TotalData.listSal_SalaryDepartmentItem.Any(m => m.ProfileID == profileItem.ID))
                {
                    var AmountSalary = TotalData.listSal_SalaryDepartmentItem.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault().AmoutSalary;
                    item = new ElementFormula(PayrollElement.SAL_SALARY_DEPARTMENT.ToString(), AmountSalary != null ? AmountSalary : 0, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_SALARY_DEPARTMENT.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_SALARY_DATE_CLOSE.ToString()))
            {
                Cat_GradePayrollEntity CatGradePayrollItem = TotalData.listCat_GradePayroll.Where(m => m.ID == FindGradePayrollByProfileAndMonthYear(TotalData.listGrade, profileItem.ID, CutOffDuration.DateStart, CutOffDuration.DateEnd).ID).FirstOrDefault();
                if (CatGradePayrollItem != null && CatGradePayrollItem.SalaryDayClose != null)
                {
                    item = new ElementFormula(PayrollElement.SAL_SALARY_DATE_CLOSE.ToString(), CatGradePayrollItem.SalaryDayClose, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_SALARY_DATE_CLOSE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            //Giữ lương
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_HOLD_SALARY.ToString(), PayrollElement.SAL_HOLD_SALARY_AFTERTAX.ToString() }))
            {
                DateTime _tmpCutoffDuration = CutOffDuration.MonthYear.AddMonths(-1);
                Sal_HoldSalaryEntity holdSalaryItem = TotalData.listSal_HoldSalary.Where(m => m.ProfileID == profileItem.ID
                    && m.MonthEndSalary != null
                    && m.Status == EnumDropDown.WorkdayStatus.E_APPROVED.ToString()
                    && m.MonthEndSalary.Value.Month == _tmpCutoffDuration.Month
                    && m.MonthEndSalary.Value.Year == _tmpCutoffDuration.Year).FirstOrDefault();

                if (holdSalaryItem != null)
                {
                    item = new ElementFormula(PayrollElement.SAL_HOLD_SALARY.ToString(), holdSalaryItem.AmountSalary, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_HOLD_SALARY_AFTERTAX.ToString(), holdSalaryItem.AmountSalaryAfterTax, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_HOLD_SALARY.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_HOLD_SALARY_AFTERTAX.ToString(), 0, 0, "null");
                    listElementFormula.Add(item);
                }
            }

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_SALARY_ACCOUNT_NO.ToString(), PayrollElement.SAL_SALARY_GROUP_BANK.ToString(), PayrollElement.SAL_SALARY_BANK_NAME.ToString() }))
            {
                Sal_SalaryInformationEntity SalaryInfomationByProfile = TotalData.listSalaryInformation.FirstOrDefault(m => m.ProfileID == profileItem.ID);
                if (SalaryInfomationByProfile != null)
                {
                    item = new ElementFormula(PayrollElement.SAL_SALARY_ACCOUNT_NO.ToString(), SalaryInfomationByProfile.AccountNo, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_SALARY_GROUP_BANK.ToString(), SalaryInfomationByProfile.GroupBank, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_SALARY_BANK_NAME.ToString(), SalaryInfomationByProfile.BankName, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_SALARY_ACCOUNT_NO.ToString(), "", 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_SALARY_GROUP_BANK.ToString(), "", 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_SALARY_BANK_NAME.ToString(), "", 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            #endregion

            #region Enum phần tử công

            #region Phần tử công tháng trước (N-1)
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKING_DAY_PREV.ToString(), PayrollElement.ATT_STD_DAY_PREV.ToString() }))
            {
                if (TotalData.Att_AttendanceTable_Prev != null)
                {
                    Att_AttendanceTableEntity _tmp = TotalData.Att_AttendanceTable_Prev.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault();
                    if (_tmp != null)
                    {
                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY_PREV.ToString(), _tmp.RealWorkDayCount, 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_STD_DAY_PREV.ToString(), _tmp.StdWorkDayCount, 0);
                        listElementFormula.Add(item);
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY_PREV.ToString(), 0, 0, "Null");
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_STD_DAY_PREV.ToString(), 0, 0, "Null");
                        listElementFormula.Add(item);
                    }
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_WORKING_DAY_PREV.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.ATT_STD_DAY_PREV.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }
            #endregion

            #region Ngày công đi làm thực tế
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKING_DAY.ToString(), PayrollElement.ATT_WORKING_DAY_AFTER.ToString() }))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);

                    //lấy lương cơ bản của nhân viên
                    List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                    SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();

                    if (SalaryProfile.Count > 0 && SalaryProfile.FirstOrDefault().DateOfEffect > CutOffDuration.DateStart)//có thay đổi lương trong tháng
                    {
                        //ngày bắt đầu mức lương 1 và ngày bắt đầu mức lương 2
                        DateTime dateStart1 = CutOffDuration.DateStart;
                        DateTime dateStart2 = SalaryProfile.FirstOrDefault().DateOfEffect;

                        //lấy dữ liệu công theo cutoff
                        List<Att_AttendanceTableItemEntity> listAttTableItem = TotalData.listAttendanceTableItem.Where(m => m.ProfileID == profileItem.ID).ToList();

                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY.ToString(), listAttTableItem.Where(m => m.WorkDate < dateStart2).Count(), 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY_AFTER.ToString(), listAttTableItem.Where(m => m.WorkDate >= dateStart2).Count(), 0);
                        listElementFormula.Add(item);
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY.ToString(), listAttendanceTableProCut.RealWorkDayCount, 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_WORKING_DAY_AFTER.ToString(), 0, 0);
                        listElementFormula.Add(item);
                    }
                }
            }
            #endregion

            #region Ngày công đi làm tính lương

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_WORKING_PAIDLEAVE_DAY.ToString(), PayrollElement.ATT_WORKING_PAIDLEAVE_DAY_AFTER.ToString() }))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);


                    //lấy lương cơ bản của nhân viên
                    List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                    SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();

                    if (SalaryProfile.Count > 0 && SalaryProfile.FirstOrDefault().DateOfEffect > CutOffDuration.DateStart)//có thay đổi lương trong tháng
                    {
                        //ngày bắt đầu mức lương 1 và ngày bắt đầu mức lương 2
                        DateTime dateStart1 = CutOffDuration.DateStart;
                        DateTime dateStart2 = SalaryProfile.FirstOrDefault().DateOfEffect;

                        //lưu số ngày công tính lương trước và sau khi thay đổi lương
                        double workpaid = 0;
                        double workpaid_after = 0;

                        //lấy dữ liệu công theo cutoff
                        List<Att_AttendanceTableItemEntity> listAttTableItem = TotalData.listAttendanceTableItem.Where(m => m.ProfileID == profileItem.ID).ToList();

                        //duyệt wa tất cả các dòng
                        foreach (var tableItem in listAttTableItem)
                        {
                            if (tableItem.WorkDate < dateStart2)//trước khi điều chỉnh
                            {
                                workpaid += tableItem.WorkPaidHours / tableItem.AvailableHours;
                            }
                            if (tableItem.WorkDate >= dateStart2)//sau khi điều chỉnh
                            {
                                workpaid_after += tableItem.WorkPaidHours / tableItem.AvailableHours;
                            }
                        }

                        item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY.ToString(), listAttTableItem.Where(m => m.WorkDate < dateStart2).Count(), 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY_AFTER.ToString(), listAttTableItem.Where(m => m.WorkDate >= dateStart2).Count(), 0);
                        listElementFormula.Add(item);
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY.ToString(), listAttendanceTableProCut.RealWorkDayCount, 0);
                        listElementFormula.Add(item);
                        item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY_AFTER.ToString(), 0, 0);
                        listElementFormula.Add(item);
                    }
                }
            }

            #endregion

            #region Số ngày phép năm cộng dồn - Số ngày phép ốm cộng dồn
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_ANNUAL_INCREMENTAL.ToString(), PayrollElement.ATT_SICK_INCREMENTAL.ToString() }))
            {
                List<Att_AnnualDetailEntity> AnnualDetailByProfile = TotalData.listAnnualDetail.Where(m => m.ProfileID == profileItem.ID).ToList();

                if (AnnualDetailByProfile != null && AnnualDetailByProfile.Count <= 0)
                {
                    var ANNUAL = AnnualDetailByProfile.FirstOrDefault(m => m.Type == AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString());
                    var SICK = AnnualDetailByProfile.FirstOrDefault(m => m.Type == AnnualLeaveDetailType.E_SICK_LEAVE.ToString());
                    item = new ElementFormula(PayrollElement.ATT_ANNUAL_INCREMENTAL.ToString(), ANNUAL != null ? ANNUAL.InitAvailable != null ? ANNUAL.InitAvailable : 0 : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.ATT_SICK_INCREMENTAL.ToString(), SICK != null ? SICK.InitAvailable != null ? SICK.InitAvailable : 0 : 0, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.ATT_ANNUAL_INCREMENTAL.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.ATT_SICK_INCREMENTAL.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            #endregion

            //Phần tử công tháng hiện tại
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_STD_DAY.ToString(), PayrollElement.ATT_HOURS_PER_DAY.ToString(), PayrollElement.ATT_OVERTIME_PIT_HOURS.ToString(), PayrollElement.ATT_TOTAL_ANNUALLEAVE_AVAILABLE.ToString(), PayrollElement.ATT_ANNUALLEAVE_ADJACENT.ToString(), PayrollElement.ATT_TOTAL_SICK_AVAILABLE.ToString(), PayrollElement.ATT_SICK_ADJACENT.ToString(), PayrollElement.ATT_ANNUALLEAVE.ToString(), PayrollElement.ATT_SICKLEAVE.ToString(), PayrollElement.ATT_WORKING_NIGHTSHIFT.ToString() }))
            {
                if (listAttendanceTableProCut != null)
                {
                    //item = new ElementFormula(PayrollElement.ATT_WORKING_DAY.ToString(), listAttendanceTableProCut.RealWorkDayCount, 0);
                    //listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_STD_DAY.ToString(), listAttendanceTableProCut.StdWorkDayCount, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_HOURS_PER_DAY.ToString(), listAttendanceTableProCut.HourPerDay, 0);
                    listElementFormula.Add(item);

                    //item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY.ToString(), listAttendanceTableProCut.TotalPaidWorkDayCount, 0);
                    //listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_WORKING_NIGHTSHIFT.ToString(), listAttendanceTableProCut.NightShiftHours, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_TOTAL_ANNUALLEAVE_AVAILABLE.ToString(), listAttendanceTableProCut.TotalAnlDayAvailable != null ? listAttendanceTableProCut.TotalAnlDayAvailable : 0, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_ANNUALLEAVE_ADJACENT.ToString(), listAttendanceTableProCut.AnlDayAdjacent, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_ANNUALLEAVE.ToString(), listAttendanceTableProCut.AnlDayTaken, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_SICKLEAVE.ToString(), listAttendanceTableProCut.SickDayTaken, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_TOTAL_SICK_AVAILABLE.ToString(), listAttendanceTableProCut.TotalSickDayAvailable, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_SICK_ADJACENT.ToString(), listAttendanceTableProCut.SickDayAdjacent, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    //item = new ElementFormula(PayrollElement.ATT_WORKING_DAY.ToString(), 0, 0, "Null");
                    //listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_STD_DAY.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_HOURS_PER_DAY.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    //item = new ElementFormula(PayrollElement.ATT_WORKING_PAIDLEAVE_DAY.ToString(), 0, 0, "Null");
                    //listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_WORKING_NIGHTSHIFT.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_TOTAL_ANNUALLEAVE_AVAILABLE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_ANNUALLEAVE_ADJACENT.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_TOTAL_SICK_AVAILABLE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_SICK_ADJACENT.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_SICKLEAVE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.ATT_ANNUALLEAVE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }


            //Số ngày nghỉ có trả lương
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_DAY.ToString(), PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_DAY_NOT_PAY.ToString() }))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //Guid AttendanceTableItemByAttID = Guid.Empty;
                    double Total_LeaveDay = 0;
                    double Total_LeaveDay_NotPay = 0;
                    if (listAttendanceTableProCut != null)
                    {
                        // AttendanceTableItemByAtt = repoAttendanceTable.FindBy(m => m.IsDelete != true && m.ID == listAttendanceTableProCut.ID).FirstOrDefault();
                        //AttendanceTableItemByAttID = unitOfWork.CreateQueryable<Att_AttendanceTable>(m => m.ID == listAttendanceTableProCut.ID).Select(m => m.ID).FirstOrDefault();
                        //  var AttendanceTableItem = repoAttendanceTableItem.FindBy(m => m.IsDelete != true && m.AttendanceTableID == AttendanceTableItemByAttID).ToList();
                        var AttendanceTableItem = TotalData.listAttendanceTableItem.Where(m => m.AttendanceTableID == listAttendanceTableProCut.ID).ToList();

                        for (int j = 0; j < AttendanceTableItem.Count; j++)
                        {
                            if (AttendanceTableItem[j].LeaveTypeID != null)
                            {
                                var LeaveDay = TotalData.listLeavedayType.Where(m => m.ID == AttendanceTableItem[j].LeaveTypeID).FirstOrDefault();
                                if (LeaveDay != null)
                                {
                                    //code của là so sánh với IsWorkDay
                                    if (LeaveDay.IsAnnualLeave || LeaveDay.PaidRate >= 1)
                                    {
                                        Total_LeaveDay += AttendanceTableItem[j].PaidLeaveHours / AttendanceTableItem[j].AvailableHours;
                                    }
                                    else if (!LeaveDay.IsAnnualLeave && LeaveDay.PaidRate <= 0)
                                    {
                                        //Total_LeaveDay_NotPay += AttendanceTableItem[j].PaidLeaveHours / AttendanceTableItem[j].AvailableHours; 
                                        Total_LeaveDay_NotPay++;
                                    }
                                }
                            }
                        }
                    }
                    item = new ElementFormula(PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_DAY.ToString(), Total_LeaveDay, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_DAY_NOT_PAY.ToString(), Total_LeaveDay_NotPay, 0);
                    listElementFormula.Add(item);
                }
            }

            //Tổng số ngày công trong năm
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_TOTAL_WORKDAY_IN_YEAR.ToString() }))
            {
                var Attantendence = TotalData.listAttendanceTable.Where(m => m.ProfileID == profileItem.ID).ToList();
                item = new ElementFormula(PayrollElement.ATT_TOTAL_WORKDAY_IN_YEAR.ToString(), Attantendence.Sum(m => m.StdWorkDayCount), 0);
                listElementFormula.Add(item);
            }

            //Tổng số ngày công thực tế trong năm
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_TOTAL_REALITYWORKDAY_IN_YEAR.ToString() }))
            {
                var Attantendence = TotalData.listAttendanceTable.Where(m => m.ProfileID == profileItem.ID).ToList();
                item = new ElementFormula(PayrollElement.ATT_TOTAL_REALITYWORKDAY_IN_YEAR.ToString(), Attantendence.Sum(m => m.RealWorkDayCount), 0);
                listElementFormula.Add(item);
            }

            //Tổng số ngày làm việc trong năm (365-dayoff)
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_TOTAL_DAY_NOT_DAYOFF_IN_YEAR.ToString() }))
            {
                DateTime form = new DateTime(CutOffDuration.MonthYear.Year - 1, 4, 1);
                DateTime to = new DateTime(CutOffDuration.MonthYear.Year, 3, 31);
                int days = new DateTime(CutOffDuration.MonthYear.Year, 12, 31).DayOfYear;
                int dayOff = TotalData.listDayOff.Where(m => m.DateOff >= form && m.DateOff <= to && m.OrgStructureID == null).Count();
                item = new ElementFormula(PayrollElement.ATT_TOTAL_DAY_NOT_DAYOFF_IN_YEAR.ToString(), days - dayOff, 0);
                listElementFormula.Add(item);
            }


            #endregion

            #region Enum phần tử bảo hiểm
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.INS_HEALTH_INSURANCE.ToString(), PayrollElement.INS_SALARY_INSURANCE.ToString(), PayrollElement.INS_SOCIAL_INSURANCE.ToString(), PayrollElement.INS_UNEMP_INSURANCE.ToString(), PayrollElement.INS_SOCIAL_INSURANCE_PROFILE.ToString(), PayrollElement.INS_SOCIAL_INSURANCE_COMPANY.ToString(), PayrollElement.INS_UNEMP_INSURANCE_PROFILE.ToString(), PayrollElement.INS_UNEMP_INSURANCE_COMPANY.ToString(), PayrollElement.INS_HEALTH_INSURANCE_PROFILE.ToString(), PayrollElement.INS_HEALTH_INSURANCE_COMPANY.ToString() }))
            {
                Ins_ProfileInsuranceMonthlyEntity InsItem = TotalData.listInsurance.Where(m => m.ProfileID == profileItem.ID && m.MonthYear != null && m.MonthYear.Value.Year == CutOffDuration.MonthYear.Year && m.MonthYear.Value.Month == CutOffDuration.MonthYear.Month).FirstOrDefault();
                if (InsItem != null)
                {
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE.ToString(), InsItem.MoneyHealthInsurance == null ? null : InsItem.MoneyHealthInsurance, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SALARY_INSURANCE.ToString(), InsItem.SalaryInsurance == null ? null : InsItem.SalaryInsurance, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE.ToString(), InsItem.MoneySocialInsurance == null ? null : InsItem.MoneySocialInsurance, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE.ToString(), InsItem.MoneyUnEmpInsurance == null ? null : InsItem.MoneyUnEmpInsurance, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE_PROFILE.ToString(), InsItem.SocialInsEmpAmount == null ? null : InsItem.SocialInsEmpAmount, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE_COMPANY.ToString(), InsItem.SocialInsComAmount == null ? null : InsItem.SocialInsComAmount, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE_PROFILE.ToString(), InsItem.UnemployEmpAmount == null ? null : InsItem.UnemployEmpAmount, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE_COMPANY.ToString(), InsItem.UnemployComAmount == null ? null : InsItem.UnemployComAmount, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE_PROFILE.ToString(), InsItem.HealthInsEmpAmount == null ? null : InsItem.HealthInsEmpAmount, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE_COMPANY.ToString(), InsItem.HealthInsComAmount == null ? null : InsItem.HealthInsComAmount, 0);
                    listElementFormula.Add(item);

                }
                else//nếu không có bảo hiểm thì cập nhất value = 0
                {
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SALARY_INSURANCE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE_PROFILE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_SOCIAL_INSURANCE_COMPANY.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE_PROFILE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_UNEMP_INSURANCE_COMPANY.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE_PROFILE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.INS_HEALTH_INSURANCE_COMPANY.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            #endregion

            #region Enum phần tử hoa hồng

            //Lấy taget & actual của shop
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_COM_TAGET_SHOP.ToString(), PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), PayrollElement.SAL_COM_PRECENT_REVENUE.ToString() }))
            {
                Sal_RevenueForShopEntity RevenueForShopItem = TotalData.listRevenueForShop.Where(m => m.ShopID == profileItem.ShopID && m.KPIBonusID == TotalData.listKPIBonus.Where(t => t.IsTotalRevenue == true).FirstOrDefault().ID && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).FirstOrDefault();
                if (RevenueForShopItem != null)
                {
                    item = new ElementFormula(PayrollElement.SAL_COM_TAGET_SHOP.ToString(), RevenueForShopItem.Target, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), RevenueForShopItem.Actual, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_PRECENT_REVENUE.ToString(), RevenueForShopItem.Actual / RevenueForShopItem.Target, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_COM_TAGET_SHOP.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_PRECENT_REVENUE.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }


            //Lấy tên cửa hàng
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_COM_SHOPNAME.ToString()))
            {
                item = new ElementFormula(PayrollElement.SAL_COM_SHOPNAME.ToString(), profileItem.ShopName == null ? "" : profileItem.ShopName, 0);
                listElementFormula.Add(item);
            }


            //Lấy taget & actual của nhân viên
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_COM_TAGET_CUSTOMER.ToString(), PayrollElement.SAL_COM_ACTUAL_CUSTOMER.ToString() }))
            {
                Sal_RevenueForProfileEntity RevenueForProfileItem = new Sal_RevenueForProfileEntity();
                RevenueForProfileItem = TotalData.listRevenueForProfile.Where(m => m.ProfileID == profileItem.ID && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).FirstOrDefault();
                if (RevenueForProfileItem != null)
                {
                    item = new ElementFormula(PayrollElement.SAL_COM_TAGET_CUSTOMER.ToString(), RevenueForProfileItem.Target, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_ACTUAL_CUSTOMER.ToString(), RevenueForProfileItem.Actual, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_COM_TAGET_CUSTOMER.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.SAL_COM_ACTUAL_CUSTOMER.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }


            //Số tháng làm việc
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_COM_WORKINGMONTH.ToString()))
            {
                double CountMonth = ((CutOffDuration.MonthYear.Year - profileItem.DateHire.Value.Year) * 12) + CutOffDuration.MonthYear.Month - profileItem.DateHire.Value.Month + (profileItem.DateHire.Value.Day < CutOffDuration.DateEnd.Day ? 1 : 0);
                item = new ElementFormula(PayrollElement.SAL_COM_WORKINGMONTH.ToString(), CountMonth, 0);
                listElementFormula.Add(item);
            }

            //tổng số nhân viên của cửa hàng
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_COM_COUNT_SHOPMEMBER.ToString()))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    int TotalProfileForShop = repoProfile.FindBy(m => m.ShopID == profileItem.ShopID).Count();
                    item = new ElementFormula(PayrollElement.SAL_COM_COUNT_SHOPMEMBER.ToString(), TotalProfileForShop, 0);
                    listElementFormula.Add(item);
                }
            }

            //Chức danh
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_COM_JOBTITLE.ToString()))
            {
                item = new ElementFormula(PayrollElement.SAL_COM_JOBTITLE.ToString(), profileItem.PositionCode == null ? "" : profileItem.PositionCode, 0);
                listElementFormula.Add(item);
            }

            //Số lượng ca trưởng trong cửa hàng
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_COM_COUNT_SL.ToString(), PayrollElement.SAL_COM_RANK.ToString() }))
            {
                if (profileItem.ShopID != null)
                {
                    int Count_SL = (int)TotalData.listShop.Single(m => m.ID == profileItem.ShopID).NoShiftLeader;
                    item = new ElementFormula(PayrollElement.SAL_COM_COUNT_SL.ToString(), Count_SL, 0);
                    listElementFormula.Add(item);

                    //cấp bậc của cửa hàng
                    item = new ElementFormula(PayrollElement.SAL_COM_RANK.ToString(), TotalData.listShop.Single(m => m.ID == profileItem.ShopID).Rank, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.SAL_COM_COUNT_SL.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.SAL_COM_RANK.ToString(), 0, 0, "Null");
                    listElementFormula.Add(item);
                }
            }

            //Tổng doanh thu của tất cả nhân viên trong shop
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_TOTAL_ACTUAL_PROFILE_SHOP.ToString()))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    var TotalPRofileInShop = repoProfile.FindBy(m => m.ShopID == profileItem.ShopID).ToList();
                    var RevenueForProfileInShop = TotalData.listRevenueForProfile.Where(m => TotalPRofileInShop.Any(t => t.ID == m.ProfileID) && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).ToList();
                    item = new ElementFormula(PayrollElement.SAL_TOTAL_ACTUAL_PROFILE_SHOP.ToString(), RevenueForProfileInShop.Sum(m => m.Actual), 0);
                    listElementFormula.Add(item);
                }

            }
            #endregion

            #region Enum phần tử đánh giá

            //Loại đánh giá
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.EVA_PERFORMANCE_TYPE_CODE.ToString(), PayrollElement.EVA_PERFORMANCE_LEVEL_NAME.ToString() }))
            {
                var PerformentceProfile = TotalData.listEva_Performance.Where(m => m.ProfileID == profileItem.ID && m.PeriodFromDate <= CutOffDuration.DateEnd && m.PeriodToDate >= CutOffDuration.DateStart).FirstOrDefault();

                item = new ElementFormula(PayrollElement.EVA_PERFORMANCE_TYPE_CODE.ToString(), PerformentceProfile != null ? PerformentceProfile.PerformanceTypeCode : "", 0);
                listElementFormula.Add(item);

                //Cấp độ đánh giá
                item = new ElementFormula(PayrollElement.EVA_PERFORMANCE_LEVEL_NAME.ToString(), PerformentceProfile != null ? PerformentceProfile.Level1Name : "", 0);
                listElementFormula.Add(item);
            }

            if (CheckIsExistFormula(listElementFormula, formula, CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_", TotalData.listPerformanceType.Select(m => m.Code).ToArray()))
            {
                DateTime YearStart = new DateTime(CutOffDuration.MonthYear.Year, 1, 1);
                DateTime YearEnd = new DateTime(CutOffDuration.MonthYear.Year, 12, 31);
                var PerformentceByProfile = TotalData.listEva_Performance.Where(m => m.ProfileID == profileItem.ID && m.DateEffect <= YearEnd && m.DateEffect >= YearStart).OrderByDescending(m => m.DateEffect).ToList();
                if (PerformentceByProfile != null)
                {
                    foreach (var i in TotalData.listPerformanceType)
                    {
                        var PerformentceByProfileAndType = PerformentceByProfile.FirstOrDefault(m => m.PerformanceTypeID == i.ID);
                        if (PerformentceByProfileAndType != null)
                        {
                            item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_" + i.Code, PerformentceByProfileAndType.Level1Name != null ? PerformentceByProfileAndType.Level1Name : "", 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_" + i.Code, "", 0, "null");
                            listElementFormula.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var i in TotalData.listPerformanceType)
                    {
                        item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_" + i.Code, "", 0, "null");
                        listElementFormula.Add(item);
                    }
                }
            }

            #endregion

            #region Enum phần tử CanTeen

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.CAN_SUMAMOUNT_N_1.ToString(), PayrollElement.CAN_AMOUNTEATNOTSTANDAR_N_1.ToString(), PayrollElement.CAN_AMOUNTCARDMORE_N_1.ToString(), PayrollElement.CAN_AMOUNTNOTWORKHASEAT_N_1.ToString(), PayrollElement.CAN_AMOUNTHDTJOB_N_1.ToString(), PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT_N_1.ToString(), PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT_N_1.ToString() }))
            {
                DateTime DateStart = CutOffDuration.DateStart.AddMonths(-1);
                DateTime DateEnd = CutOffDuration.DateEnd.AddMonths(-1);

                var SumryMealRecordByRrofile = TotalData.listSumryMealRecord.Where(m => m.ProfileID == profileItem.ID && (m.DateFrom != null && m.DateTo != null) && m.DateFrom <= DateEnd && m.DateTo >= DateStart).OrderByDescending(m => m.DateFrom).FirstOrDefault();
                if (SumryMealRecordByRrofile != null)
                {
                    item = new ElementFormula(PayrollElement.CAN_SUMAMOUNT_N_1.ToString(), SumryMealRecordByRrofile.SumAmount != null ? SumryMealRecordByRrofile.SumAmount : 0, 0);
                    listElementFormula.Add(item);

                    item = new ElementFormula(PayrollElement.CAN_AMOUNTEATNOTSTANDAR_N_1.ToString(), SumryMealRecordByRrofile.AmountEatNotStandar != null ? SumryMealRecordByRrofile.AmountEatNotStandar : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTCARDMORE_N_1.ToString(), SumryMealRecordByRrofile.SumAmountCardMore != null ? SumryMealRecordByRrofile.SumAmountCardMore : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKHASEAT_N_1.ToString(), SumryMealRecordByRrofile.AmountNotWorkHasEat != null ? SumryMealRecordByRrofile.AmountNotWorkHasEat : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTHDTJOB_N_1.ToString(), SumryMealRecordByRrofile.AmountHDTJob != null ? SumryMealRecordByRrofile.AmountHDTJob : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT_N_1.ToString(), SumryMealRecordByRrofile.AmountNotWorkButHasHDT != null ? SumryMealRecordByRrofile.AmountNotWorkButHasHDT : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT_N_1.ToString(), SumryMealRecordByRrofile.AmountSubtractWorngStandarHDT != null ? SumryMealRecordByRrofile.AmountSubtractWorngStandarHDT : 0, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.CAN_SUMAMOUNT_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTEATNOTSTANDAR_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTCARDMORE_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKHASEAT_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTHDTJOB_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT_N_1.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                }

            }



            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.CAN_SUMAMOUNT.ToString(), PayrollElement.CAN_AMOUNTEATNOTSTANDAR.ToString(), PayrollElement.CAN_AMOUNTCARDMORE.ToString(), PayrollElement.CAN_AMOUNTNOTWORKHASEAT.ToString(), PayrollElement.CAN_AMOUNTHDTJOB.ToString(), PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT.ToString(), PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT.ToString() }))
            {
                var SumryMealRecordByRrofile = TotalData.listSumryMealRecord.Where(m => m.ProfileID == profileItem.ID && (m.DateFrom != null && m.DateTo != null) && m.DateFrom <= CutOffDuration.DateEnd && m.DateTo >= CutOffDuration.DateStart).OrderByDescending(m => m.DateFrom).FirstOrDefault();
                if (SumryMealRecordByRrofile != null)
                {
                    item = new ElementFormula(PayrollElement.CAN_SUMAMOUNT.ToString(), SumryMealRecordByRrofile.SumAmount != null ? SumryMealRecordByRrofile.SumAmount : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTEATNOTSTANDAR.ToString(), SumryMealRecordByRrofile.AmountEatNotStandar != null ? SumryMealRecordByRrofile.AmountEatNotStandar : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTCARDMORE.ToString(), SumryMealRecordByRrofile.SumAmountCardMore != null ? SumryMealRecordByRrofile.SumAmountCardMore : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKHASEAT.ToString(), SumryMealRecordByRrofile.AmountNotWorkHasEat != null ? SumryMealRecordByRrofile.AmountNotWorkHasEat : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTHDTJOB.ToString(), SumryMealRecordByRrofile.AmountHDTJob != null ? SumryMealRecordByRrofile.AmountHDTJob : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT.ToString(), SumryMealRecordByRrofile.AmountNotWorkButHasHDT != null ? SumryMealRecordByRrofile.AmountNotWorkButHasHDT : 0, 0);
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT.ToString(), SumryMealRecordByRrofile.AmountSubtractWorngStandarHDT != null ? SumryMealRecordByRrofile.AmountSubtractWorngStandarHDT : 0, 0);
                    listElementFormula.Add(item);
                }
                else
                {
                    item = new ElementFormula(PayrollElement.CAN_SUMAMOUNT.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTEATNOTSTANDAR.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTCARDMORE.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKHASEAT.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTHDTJOB.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTNOTWORKBUTHASHDT.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                    item = new ElementFormula(PayrollElement.CAN_AMOUNTSUBTRACTWRONGSTANDARHDT.ToString(), 0, 0, "Không có dữ liệu trong kỳ tính lương !");
                    listElementFormula.Add(item);
                }

            }

            #endregion

            #endregion

            #region Các phần tử động
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.DYN_COUNTDAYOVERTIMEBYTYPE_.ToString(), new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //var repoAttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                    //var repoAttOvertime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                    //tách phần tử ra để lấy tham số
                    string Parameter = string.Empty;
                    var ttt = ParseFormulaToList(formula.Formula);
                    List<string> ListFormula = ParseFormulaToList(formula.Formula).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1 && m.StartsWith("[" + PayrollElement.DYN_COUNTDAYOVERTIMEBYTYPE_.ToString())).ToList();

                    //Att_AttendanceTable listAttTable = TotalData.listAttendanceTable.Where(m => m.CutOffDurationID == CutOffDuration.ID && m.ProfileID == profileItem.ID && m.IsDelete != true).FirstOrDefault();
                    List<Att_AttendanceTableItemEntity> listAttTableItem = TotalData.listAttendanceTableItem.Where(m => m.AttendanceTableID == listAttendanceTableProCut.ID).ToList();

                    //lấy danh sách đăng ký tăng ca
                    List<Att_OvertimeEntity> listOverTime = TotalData.listOverTime.Where(m => m.ProfileID == profileItem.ID).ToList();

                    //duyệt qua các phần tử động để lấy tham số
                    foreach (var i in ListFormula)
                    {
                        string[] listParam = i.Split('_');
                        if (listParam.Count() >= 3)//nếu là 3 phần tử là đúng công thức, ngược lại là sai
                        {
                            string OtTypeCode = i.Replace(PayrollElement.DYN_COUNTDAYOVERTIMEBYTYPE_.ToString(), "").Replace("_" + listParam.LastOrDefault(), "").Replace("]", "").Replace("[", "");
                            double number = 0;
                            double CountOtDay = 0;
                            Cat_OvertimeTypeEntity OTType = TotalData.listOvertimeType.Where(m => m.Code == OtTypeCode).FirstOrDefault();

                            List<Att_OvertimeEntity> listOverTimeByTableItem = new List<Att_OvertimeEntity>();
                            Att_OvertimeEntity OverTimeItem = new Att_OvertimeEntity();
                            Cat_Shift ShiftItem = new Cat_Shift();

                            if (double.TryParse(listParam.LastOrDefault().Replace("]", "").Replace("[", ""), out number) && OTType != null)
                            {
                                number = number / 10;
                                CountOtDay = 0;
                                //lọc ra các loại OT theo loại
                                foreach (var tableItem in listAttTableItem)
                                {
                                    listOverTimeByTableItem = listOverTime.Where(m => m.WorkDate.Date == tableItem.WorkDate.Date).ToList();
                                    if (tableItem.OvertimeTypeID != null && tableItem.OvertimeTypeID == OTType.ID)
                                    {
                                        OverTimeItem = listOverTimeByTableItem.Where(m => m.OvertimeTypeID == (Guid)tableItem.OvertimeTypeID).FirstOrDefault();
                                        if (OverTimeItem != null && OverTimeItem.ShiftID != null)
                                        {
                                            ShiftItem = TotalData.listCat_Shift.Where(m => m.ID == (Guid)OverTimeItem.ShiftID).FirstOrDefault().Copy<Cat_Shift>();
                                            CountOtDay += tableItem.OvertimeHours / ShiftItem.udAvailableHours >= number ? 1 : 0;
                                        }
                                    }
                                    else if (tableItem.ExtraOvertimeTypeID != null && tableItem.ExtraOvertimeTypeID == OTType.ID)
                                    {
                                        OverTimeItem = listOverTimeByTableItem.Where(m => m.OvertimeTypeID == (Guid)tableItem.ExtraOvertimeTypeID).FirstOrDefault();
                                        if (OverTimeItem != null && OverTimeItem.ShiftID != null)
                                        {
                                            ShiftItem = TotalData.listCat_Shift.Where(m => m.ID == (Guid)OverTimeItem.ShiftID).FirstOrDefault().Copy<Cat_Shift>();
                                            CountOtDay += tableItem.ExtraOvertimeHours / ShiftItem.udAvailableHours >= number ? 1 : 0;
                                        }
                                    }
                                    else if (tableItem.ExtraOvertimeType2ID != null && tableItem.ExtraOvertimeType2ID == OTType.ID)
                                    {
                                        OverTimeItem = listOverTimeByTableItem.Where(m => m.OvertimeTypeID == (Guid)tableItem.ExtraOvertimeType2ID).FirstOrDefault();
                                        if (OverTimeItem != null && OverTimeItem.ShiftID != null)
                                        {
                                            ShiftItem = TotalData.listCat_Shift.Where(m => m.ID == (Guid)OverTimeItem.ShiftID).FirstOrDefault().Copy<Cat_Shift>();
                                            CountOtDay += tableItem.ExtraOvertimeHours2 / ShiftItem.udAvailableHours >= number ? 1 : 0;
                                        }
                                    }
                                    else if (tableItem.ExtraOvertimeType3ID != null && tableItem.ExtraOvertimeType3ID == OTType.ID)
                                    {
                                        OverTimeItem = listOverTimeByTableItem.Where(m => m.OvertimeTypeID == (Guid)tableItem.ExtraOvertimeType3ID).FirstOrDefault();
                                        if (OverTimeItem != null && OverTimeItem.ShiftID != null)
                                        {
                                            ShiftItem = TotalData.listCat_Shift.Where(m => m.ID == (Guid)OverTimeItem.ShiftID).FirstOrDefault().Copy<Cat_Shift>();
                                            CountOtDay += tableItem.ExtraOvertimeHours3 / ShiftItem.udAvailableHours >= number ? 1 : 0;
                                        }
                                    }
                                }
                                item = new ElementFormula(i.Replace("]", "").Replace("[", ""), CountOtDay, 0);
                                listElementFormula.Add(item);
                            }
                            else//sai công thức , không convert đc số giờ
                            {
                                item = new ElementFormula(i.Replace("]", "").Replace("[", ""), 0, 0, "Công thức động sai !");
                                listElementFormula.Add(item);
                            }
                        }
                        else
                        {
                            item = new ElementFormula(i.Replace("]", "").Replace("[", ""), 0, 0, "Công thức động sai !");
                            listElementFormula.Add(item);
                        }
                    }
                }
            }
            #endregion

            #region Lấy giá trị cho các phần tử là các loại OT và LeaveDay

            #region Lấy OT theo từng loại và lấy tổng số giờ tăng ca đã quy đổi ra hệ số 1

            #region OT tháng N


            List<Cat_ElementEntity> listElement_OT = new List<Cat_ElementEntity>();
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_OVERTIME_", "_HOURS"))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //var repoAttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);

                    //double SumOvertime = 0;
                    //double SumOvertimeInsurance = 0;

                    //lấy lương cơ bản của nhân viên
                    List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                    SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();

                    //có thay đổi lương trong tháng
                    if (SalaryProfile.Count > 0 && SalaryProfile.FirstOrDefault().DateOfEffect > CutOffDuration.DateStart)//có thay đổi lương trong tháng
                    {
                        double OtHour = 0;
                        //ngày bắt đầu mức lương 1 và ngày bắt đầu mức lương 2
                        DateTime dateStart1 = CutOffDuration.DateStart;
                        DateTime dateStart2 = SalaryProfile.FirstOrDefault().DateOfEffect;

                        //lấy dữ liệu công theo cutoff
                        List<Att_AttendanceTableItemEntity> listAttTableItem = TotalData.listAttendanceTableItem.Where(m => m.ProfileID == profileItem.ID).ToList();

                        if (listAttTableItem != null && listAttTableItem.Count > 0)
                        {
                            listAttTableItem = listAttTableItem.Where(m => m.WorkDate < dateStart2).ToList();
                            //duyệt wa các loại ot
                            foreach (var OTType in TotalData.listOvertimeType)
                            {
                                OtHour = 0;
                                //tính số giờ OT của từng loại
                                foreach (var tableItem in listAttTableItem)
                                {
                                    if (tableItem.OvertimeTypeID != null && tableItem.OvertimeTypeID == OTType.ID)
                                    {
                                        OtHour += tableItem.OvertimeHours;
                                    }
                                    else if (tableItem.ExtraOvertimeTypeID != null && tableItem.ExtraOvertimeTypeID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours;
                                    }
                                    else if (tableItem.ExtraOvertimeType2ID != null && tableItem.ExtraOvertimeType2ID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours2;
                                    }
                                    else if (tableItem.ExtraOvertimeType3ID != null && tableItem.ExtraOvertimeType3ID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours3;
                                    }
                                }
                                item = new ElementFormula("ATT_OVERTIME_" + OTType.Code + "_HOURS", OtHour, 0);
                                listElementFormula.Add(item);
                            }
                        }
                        else
                        {
                            foreach (var OTType in TotalData.listOvertimeType)
                            {
                                item = new ElementFormula("ATT_OVERTIME_" + OTType.Code + "_HOURS", 0, 0);
                                listElementFormula.Add(item);
                            }
                        }
                    }
                    else//không thay đổi lương trong tháng
                    {
                        listElement_OT = TotalData.listElement_All.Where(m => m.ElementCode.StartsWith("ATT_OVERTIME_") && m.ElementCode.EndsWith("_HOURS")).ToList();
                        foreach (var OT in listElement_OT)
                        {
                            var itemOverTime = TotalData.listOvertimeType.Where(m => m.Code == OT.ElementCode.Replace("ATT_OVERTIME_", "").Replace("_HOURS", "")).FirstOrDefault();

                            double value = 0;
                            if (itemOverTime != null && listAttendanceTableProCut != null)
                            {
                                if (listAttendanceTableProCut.Overtime1Type != null && listAttendanceTableProCut.Overtime1Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime1Hours;
                                }
                                if (listAttendanceTableProCut.Overtime2Type != null && listAttendanceTableProCut.Overtime2Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime2Hours;
                                }
                                if (listAttendanceTableProCut.Overtime3Type != null && listAttendanceTableProCut.Overtime3Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime3Hours;
                                }
                                if (listAttendanceTableProCut.Overtime4Type != null && listAttendanceTableProCut.Overtime4Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime4Hours;
                                }
                                if (listAttendanceTableProCut.Overtime5Type != null && listAttendanceTableProCut.Overtime5Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime5Hours;
                                }
                                if (listAttendanceTableProCut.Overtime6Type != null && listAttendanceTableProCut.Overtime6Type == itemOverTime.ID)
                                {
                                    value += listAttendanceTableProCut.Overtime6Hours;
                                }
                            }
                            item = new ElementFormula(OT.ElementCode, value, 0);
                            listElementFormula.Add(item);

                            //if (itemOverTime != null)
                            //{
                            //    SumOvertimeInsurance += value * itemOverTime.TaxRate;//Tính số giờ tăng ca có chịu thuế
                            //    SumOvertime += value * itemOverTime.Rate;//tính hệ số và lưu vào biến tổng số giờ tăng ca
                            //}
                        }
                        ////Lưu giá trị cho Enum tổng số giớ tăng ca trong tháng
                        //item = new ElementFormula(PayrollElement.ATT_OVERTIME_HOURS.ToString(), SumOvertime, 0);
                        //listElementFormula.Add(item);

                        ////Lưu giá trị cho Enum tổng số giớ tăng ca trong tháng có tính thuế
                        //item = new ElementFormula(PayrollElement.ATT_OVERTIME_PIT_HOURS.ToString(), SumOvertimeInsurance, 0);
                        //listElementFormula.Add(item);
                    }
                }
            }
            #endregion

            #region Tổng giờ tăng ca trong tháng và tổng giờ tăng ca trong tháng có tính thuế

            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_OVERTIME_PIT_HOURS.ToString(), PayrollElement.ATT_OVERTIME_HOURS.ToString() }))
            {
                double SumOvertime = 0;
                double SumOvertimeInsurance = 0;

                foreach (var itemOverTime in TotalData.listOvertimeType)
                {
                    double value = 0;
                    if (itemOverTime != null && listAttendanceTableProCut != null)
                    {
                        if (listAttendanceTableProCut.Overtime1Type != null && listAttendanceTableProCut.Overtime1Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime1Hours;
                        }
                        if (listAttendanceTableProCut.Overtime2Type != null && listAttendanceTableProCut.Overtime2Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime2Hours;
                        }
                        if (listAttendanceTableProCut.Overtime3Type != null && listAttendanceTableProCut.Overtime3Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime3Hours;
                        }
                        if (listAttendanceTableProCut.Overtime4Type != null && listAttendanceTableProCut.Overtime4Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime4Hours;
                        }
                        if (listAttendanceTableProCut.Overtime5Type != null && listAttendanceTableProCut.Overtime5Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime5Hours;
                        }
                        if (listAttendanceTableProCut.Overtime6Type != null && listAttendanceTableProCut.Overtime6Type == itemOverTime.ID)
                        {
                            value += listAttendanceTableProCut.Overtime6Hours;
                        }
                    }

                    if (itemOverTime != null)
                    {
                        SumOvertimeInsurance += value * itemOverTime.TaxRate;//Tính số giờ tăng ca có chịu thuế
                        SumOvertime += value * itemOverTime.Rate;//tính hệ số và lưu vào biến tổng số giờ tăng ca
                    }
                }
                //Lưu giá trị cho Enum tổng số giớ tăng ca trong tháng
                item = new ElementFormula(PayrollElement.ATT_OVERTIME_HOURS.ToString(), SumOvertime, 0);
                listElementFormula.Add(item);

                //Lưu giá trị cho Enum tổng số giớ tăng ca trong tháng có tính thuế
                item = new ElementFormula(PayrollElement.ATT_OVERTIME_PIT_HOURS.ToString(), SumOvertimeInsurance, 0);
                listElementFormula.Add(item);
            }

            #endregion

            #region Các loại OT nếu có thay đổi lương trong tháng

            if (CheckIsExistFormula(listElementFormula, formula, "ATT_OVERTIME_", "_HOURS_AFTER"))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //var repoAttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                    //lấy lương cơ bản của nhân viên
                    List<Sal_BasicSalaryEntity> SalaryProfile = new List<Sal_BasicSalaryEntity>();
                    SalaryProfile = TotalData.listBasicSalary.Where(m => m.ProfileID == profileItem.ID).OrderByDescending(m => m.DateOfEffect).ToList();

                    if (SalaryProfile.Count > 0 && SalaryProfile.FirstOrDefault().DateOfEffect > CutOffDuration.DateStart)//có thay đổi lương trong tháng
                    {
                        double OtHour = 0;
                        //ngày bắt đầu mức lương 1 và ngày bắt đầu mức lương 2
                        DateTime dateStart1 = CutOffDuration.DateStart;
                        DateTime dateStart2 = SalaryProfile.FirstOrDefault().DateOfEffect;

                        //lấy dữ liệu công theo cutoff
                        List<Att_AttendanceTableItemEntity> listAttTableItem = TotalData.listAttendanceTableItem.Where(m => m.ProfileID == profileItem.ID).ToList();

                        if (listAttTableItem != null && listAttTableItem.Count > 0)
                        {
                            listAttTableItem = listAttTableItem.Where(m => m.WorkDate >= dateStart2).ToList();
                            //duyệt wa các loại ot
                            foreach (var OTType in TotalData.listOvertimeType)
                            {
                                OtHour = 0;
                                //tính số giờ OT của từng loại
                                foreach (var tableItem in listAttTableItem)
                                {
                                    if (tableItem.OvertimeTypeID != null && tableItem.OvertimeTypeID == OTType.ID)
                                    {
                                        OtHour += tableItem.OvertimeHours;
                                    }
                                    else if (tableItem.ExtraOvertimeTypeID != null && tableItem.ExtraOvertimeTypeID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours;
                                    }
                                    else if (tableItem.ExtraOvertimeType2ID != null && tableItem.ExtraOvertimeType2ID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours2;
                                    }
                                    else if (tableItem.ExtraOvertimeType3ID != null && tableItem.ExtraOvertimeType3ID == OTType.ID)
                                    {
                                        OtHour += tableItem.ExtraOvertimeHours3;
                                    }
                                }
                                item = new ElementFormula("ATT_OVERTIME_" + OTType.Code + "_HOURS_AFTER", OtHour, 0);
                                listElementFormula.Add(item);
                            }
                        }
                        else
                        {
                            foreach (var OTType in TotalData.listOvertimeType)
                            {
                                item = new ElementFormula("ATT_OVERTIME_" + OTType.Code + "_HOURS_AFTER", 0, 0);
                                listElementFormula.Add(item);
                            }
                        }
                    }
                    else//không có lương cơ bản hoặc không có thay đổi lương trong tháng
                    {
                        foreach (var OTType in TotalData.listOvertimeType)
                        {
                            item = new ElementFormula("ATT_OVERTIME_" + OTType.Code + "_HOURS_AFTER", 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }
            }

            #endregion

            #region OT tháng N-1
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_OVERTIME_", "_HOURS_PREV"))
            {
                listElement_OT = TotalData.listElement_All.Where(m => m.ElementCode.StartsWith("ATT_OVERTIME_") && m.ElementCode.EndsWith("_HOURS_PREV")).ToList();
                if (listElement_OT != null && listElement_OT.Count > 0)
                {
                    foreach (var OT in listElement_OT)
                    {
                        var itemOverTime = TotalData.listOvertimeType.Where(m => m.Code == OT.ElementCode.Replace("ATT_OVERTIME_", "").Replace("_HOURS_PREV", "")).FirstOrDefault();

                        double value = 0;
                        var _tmpAttendanceTable = TotalData.Att_AttendanceTable_Prev.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault();
                        if (itemOverTime != null && _tmpAttendanceTable != null)
                        {
                            if (_tmpAttendanceTable.Overtime1Type != null && _tmpAttendanceTable.Overtime1Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime1Hours;
                            }
                            if (_tmpAttendanceTable.Overtime2Type != null && _tmpAttendanceTable.Overtime2Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime2Hours;
                            }
                            if (_tmpAttendanceTable.Overtime3Type != null && _tmpAttendanceTable.Overtime3Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime3Hours;
                            }
                            if (_tmpAttendanceTable.Overtime4Type != null && _tmpAttendanceTable.Overtime4Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime4Hours;
                            }
                            if (_tmpAttendanceTable.Overtime5Type != null && _tmpAttendanceTable.Overtime5Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime5Hours;
                            }
                            if (_tmpAttendanceTable.Overtime6Type != null && _tmpAttendanceTable.Overtime6Type == itemOverTime.ID)
                            {
                                value += _tmpAttendanceTable.Overtime6Hours;
                            }
                        }
                        item = new ElementFormula(OT.ElementCode, value, 0);
                        listElementFormula.Add(item);
                    }
                }
            }

            #endregion

            #endregion

            #region Lấy LeaveDay theo từng loại và lấy tổng nghỉ
            double SumLeaveday = 0;
            double SumLeavedayIsSalary = 0;

            //N-1
            double SumLeaveday_Prev = 0;
            double SumLeavedayIsSalary_Prev = 0;

            List<Cat_ElementEntity> listElement_Leave = new List<Cat_ElementEntity>();
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_LEAVE_", "_HOURS") || CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_LEAVE_HOURS.ToString(), PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_HOURS.ToString() }))
            {
                listElement_Leave = TotalData.listElement_All.Where(m => m.ElementCode.StartsWith("ATT_LEAVE_") && m.ElementCode.EndsWith("_HOURS")).ToList();

                foreach (var LD in listElement_Leave)
                {
                    var itemLeaveday = TotalData.listLeavedayType.Where(m => m.Code == LD.ElementCode.Replace("ATT_LEAVE_", "").Replace("_HOURS", "")).FirstOrDefault();

                    double value = 0;
                    if (itemLeaveday != null && listAttendanceTableProCut != null)
                    {
                        if (listAttendanceTableProCut.LeaveDay1Type != null && listAttendanceTableProCut.LeaveDay1Type == itemLeaveday.ID)
                        {
                            value += listAttendanceTableProCut.LeaveDay1Hours;
                        }
                        if (listAttendanceTableProCut.LeaveDay2Type != null && listAttendanceTableProCut.LeaveDay2Type == itemLeaveday.ID)
                        {
                            value += listAttendanceTableProCut.LeaveDay2Hours;
                        }
                        if (listAttendanceTableProCut.LeaveDay3Type != null && listAttendanceTableProCut.LeaveDay3Type == itemLeaveday.ID)
                        {
                            value += listAttendanceTableProCut.LeaveDay3Hours;
                        }
                        if (listAttendanceTableProCut.LeaveDay4Type != null && listAttendanceTableProCut.LeaveDay4Type == itemLeaveday.ID)
                        {
                            value += listAttendanceTableProCut.LeaveDay4Hours;
                        }
                    }
                    item = new ElementFormula(LD.ElementCode, value, 0);
                    listElementFormula.Add(item);


                    SumLeaveday += value;//Tổng giờ nghỉ trong tháng
                    if (itemLeaveday != null)
                    {
                        SumLeavedayIsSalary += value * itemLeaveday.PaidRate;//tổng giờ nghỉ có trả lương
                    }

                    #region Lấy LeaveDay theo từng lại và lấy tổng ngày nghỉ tháng N - 1

                    value = 0;
                    var _tmpAttendanceTable = TotalData.Att_AttendanceTable_Prev.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault();
                    if (itemLeaveday != null && _tmpAttendanceTable != null)
                    {
                        if (_tmpAttendanceTable.LeaveDay1Type != null && _tmpAttendanceTable.LeaveDay1Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay1Hours;
                        }
                        if (_tmpAttendanceTable.LeaveDay2Type != null && _tmpAttendanceTable.LeaveDay2Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay2Hours;
                        }
                        if (_tmpAttendanceTable.LeaveDay3Type != null && _tmpAttendanceTable.LeaveDay3Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay3Hours;
                        }
                        if (_tmpAttendanceTable.LeaveDay4Type != null && _tmpAttendanceTable.LeaveDay4Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay4Hours;
                        }
                    }
                    SumLeaveday_Prev += value;//Tổng giờ nghỉ trong tháng
                    if (itemLeaveday != null)
                    {
                        SumLeavedayIsSalary_Prev += value * itemLeaveday.PaidRate;//tổng giờ nghỉ có trả lương
                    }
                    #endregion
                }

                //tạo phần tử Enum tổng số giờ nghỉ trong tháng
                item = new ElementFormula(PayrollElement.ATT_LEAVE_HOURS.ToString(), SumLeaveday, 0);
                listElementFormula.Add(item);

                //tạo phần tử Enum tổng số giờ nghỉ trong tháng có tính lương
                item = new ElementFormula(PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_HOURS.ToString(), SumLeavedayIsSalary, 0);
                listElementFormula.Add(item);
            }

            //Số ngày nghỉ tháng N-1
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_LEAVE_", "_DAY_PREV"))
            {
                listElement_Leave = TotalData.listElement_All.Where(m => m.ElementCode.StartsWith("ATT_LEAVE_") && m.ElementCode.EndsWith("_DAY_PREV")).ToList();
                var _tmpAttendanceTable = TotalData.Att_AttendanceTable_Prev.Where(m => m.ProfileID == profileItem.ID).FirstOrDefault();
                foreach (var LD in listElement_Leave)
                {
                    var itemLeaveday = TotalData.listLeavedayType.Where(m => m.Code == LD.ElementCode.Replace("ATT_LEAVE_", "").Replace("_DAY_PREV", "")).FirstOrDefault();

                    double value = 0;
                    if (itemLeaveday != null && _tmpAttendanceTable != null)
                    {
                        if (_tmpAttendanceTable.LeaveDay1Type != null && _tmpAttendanceTable.LeaveDay1Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay1Days != null ? (double)_tmpAttendanceTable.LeaveDay1Days : 0;
                        }
                        if (_tmpAttendanceTable.LeaveDay2Type != null && _tmpAttendanceTable.LeaveDay2Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay2Days != null ? (double)_tmpAttendanceTable.LeaveDay2Days : 0;
                        }
                        if (_tmpAttendanceTable.LeaveDay3Type != null && _tmpAttendanceTable.LeaveDay3Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay3Days != null ? (double)_tmpAttendanceTable.LeaveDay3Days : 0;
                        }
                        if (_tmpAttendanceTable.LeaveDay4Type != null && _tmpAttendanceTable.LeaveDay4Type == itemLeaveday.ID)
                        {
                            value += _tmpAttendanceTable.LeaveDay4Days != null ? (double)_tmpAttendanceTable.LeaveDay4Days : 0;
                        }
                    }
                    item = new ElementFormula(LD.ElementCode, value, 0);
                    listElementFormula.Add(item);
                }
            }

            //Tổng số Ngày Nghỉ từng loại trong năm
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_LEAVE_", "_DAY_INYEAR"))
            {
                using (var context = new VnrHrmDataContext())
                {
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoSys_AttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                    var repoSys_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);

                    DateTime from = new DateTime(CutOffDuration.MonthYear.Year - 1, 4, 1);
                    DateTime to = new DateTime(CutOffDuration.MonthYear.Year, 3, 31);

                    List<Att_AttendanceTable> listAttendanceTableByProfile = repoSys_AttendanceTable.FindBy(m => m.IsDelete != true && m.ProfileID == profileItem.ID && m.MonthYear != null && m.MonthYear.Value >= from && m.MonthYear.Value <= to).ToList();

                    listElement_Leave = TotalData.listElement_All.Where(m => m.ElementCode.StartsWith("ATT_LEAVE_") && m.ElementCode.EndsWith("_DAY_INYEAR")).ToList();

                    foreach (var LD in listElement_Leave)
                    {
                        var itemLeaveday = TotalData.listLeavedayType.Where(m => m.Code == LD.ElementCode.Replace("ATT_LEAVE_", "").Replace("_DAY_INYEAR", "")).FirstOrDefault();

                        double value = 0;
                        foreach (var _tmpAttendanceTable in listAttendanceTableByProfile)
                        {
                            if (itemLeaveday != null && _tmpAttendanceTable != null)
                            {
                                if (_tmpAttendanceTable.LeaveDay1Type != null && _tmpAttendanceTable.LeaveDay1Type == itemLeaveday.ID)
                                {
                                    value += _tmpAttendanceTable.LeaveDay1Days != null ? (double)_tmpAttendanceTable.LeaveDay1Days : 0;
                                }
                                if (_tmpAttendanceTable.LeaveDay2Type != null && _tmpAttendanceTable.LeaveDay2Type == itemLeaveday.ID)
                                {
                                    value += _tmpAttendanceTable.LeaveDay2Days != null ? (double)_tmpAttendanceTable.LeaveDay2Days : 0;
                                }
                                if (_tmpAttendanceTable.LeaveDay3Type != null && _tmpAttendanceTable.LeaveDay3Type == itemLeaveday.ID)
                                {
                                    value += _tmpAttendanceTable.LeaveDay3Days != null ? (double)_tmpAttendanceTable.LeaveDay3Days : 0;
                                }
                                if (_tmpAttendanceTable.LeaveDay4Type != null && _tmpAttendanceTable.LeaveDay4Type == itemLeaveday.ID)
                                {
                                    value += _tmpAttendanceTable.LeaveDay4Days != null ? (double)_tmpAttendanceTable.LeaveDay4Days : 0;
                                }
                            }
                        }
                        item = new ElementFormula(LD.ElementCode, value, 0);
                        listElementFormula.Add(item);
                    }
                }
            }


            #region N-1
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.ATT_LEAVE_HOURS_PREV.ToString(), PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_HOURS_PREV.ToString() }))
            {
                //Tổng số giờ nghỉ trong tháng N-1
                item = new ElementFormula(PayrollElement.ATT_LEAVE_HOURS_PREV.ToString(), SumLeaveday_Prev, 0);
                listElementFormula.Add(item);

                //Tổng số giờ nghỉ trong tháng có tính lương N-1
                item = new ElementFormula(PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_HOURS_PREV.ToString(), SumLeavedayIsSalary_Prev, 0);
                listElementFormula.Add(item);
            }
            #endregion
            #endregion

            #endregion

            #region  Honda - tổng số ngày làm việc theo từng ca của nhân viên trong tháng
            if (CheckIsExistFormula(listElementFormula, formula, "ATT_SHIFT_", "_HOURS"))
            {
                using (var context = new VnrHrmDataContext())
                {
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //var repoAtt_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);

                    List<Att_AttendanceTableItemEntity> listAttTableItemByShift = new List<Att_AttendanceTableItemEntity>();
                    List<Att_AttendanceTableItemEntity> listAttendanceTableItemByAtt = TotalData.listAttendanceTableItem.Where(m => m.AttendanceTableID == listAttendanceTableProCut.ID).ToList();

                    for (int j = 0; j < TotalData.listCat_Shift.Count; j++)
                    {
                        listAttTableItemByShift = listAttendanceTableItemByAtt.Where(m => m.ShiftID != null && m.ShiftID == TotalData.listCat_Shift[j].ID).ToList();
                        item = new ElementFormula("ATT_SHIFT_" + TotalData.listCat_Shift[j].Code + "_HOURS", listAttTableItemByShift.Sum(m => m.AvailableHours), 0);
                        listElementFormula.Add(item);
                    }
                }
            }


            if (CheckIsExistFormula(listElementFormula, formula, "ATT_SHIFT_", "_DAY"))
            {
                using (var context = new VnrHrmDataContext())
                {
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    //var repoAtt_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);

                    List<Att_AttendanceTableItemEntity> listAttTableItemByShift = new List<Att_AttendanceTableItemEntity>();
                    List<Att_AttendanceTableItemEntity> listAttendanceTableItemByAtt = TotalData.listAttendanceTableItem.Where(m => m.AttendanceTableID == listAttendanceTableProCut.ID).ToList();
                    List<Att_AttendanceTableItemEntity> listAttendanceTableItemByAtt_Prev = new List<Att_AttendanceTableItemEntity>();
                    if (TotalData.Att_AttendanceTable_Prev.Count() >= 0)
                    {

                        Att_AttendanceTableEntity _tmp = TotalData.Att_AttendanceTable_Prev.Where(t => t.ProfileID == profileItem.ID).FirstOrDefault();
                        Guid _tmpID = Guid.Empty;
                        if (_tmp != null)
                        {
                            _tmpID = _tmp.ID;
                        }
                        listAttendanceTableItemByAtt_Prev = TotalData.listAttendanceTableItem.Where(m => m.AttendanceTableID == _tmpID).ToList();
                    }

                    for (int j = 0; j < TotalData.listCat_Shift.Count; j++)
                    {
                        listAttTableItemByShift = listAttendanceTableItemByAtt.Where(m => m.ShiftID != null && m.ShiftID == TotalData.listCat_Shift[j].ID).ToList();
                        if (listAttTableItemByShift != null && listAttTableItemByShift.Count > 0)
                        {
                            item = new ElementFormula("ATT_SHIFT" + "_" + TotalData.listCat_Shift[j].Code + "_" + "DAY", listAttTableItemByShift.Where(d => d.AvailableHours > 0).Sum(d => (d.WorkPaidHours + d.LateEarlyMinutes / 60.0) / d.AvailableHours), 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula("ATT_SHIFT" + "_" + TotalData.listCat_Shift[j].Code + "_" + "DAY", 0, 0);
                            listElementFormula.Add(item);
                        }

                        //tháng N-1
                        listAttTableItemByShift = listAttendanceTableItemByAtt_Prev.Where(m => m.ShiftID != null && m.ShiftID == TotalData.listCat_Shift[j].ID).ToList();
                        if (listAttTableItemByShift != null && listAttTableItemByShift.Count > 0)
                        {
                            item = new ElementFormula("ATT_SHIFT" + "_" + TotalData.listCat_Shift[j].Code + "_" + "DAY_PREV", listAttTableItemByShift.Where(d => d.AvailableHours > 0).Sum(d => (d.WorkPaidHours + d.LateEarlyMinutes / 60.0) / d.AvailableHours), 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula("ATT_SHIFT" + "_" + TotalData.listCat_Shift[j].Code + "_" + "DAY_PREV", 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }

            }

            #endregion

            #region Lấy giá trị cho các loại phần tử là Hoa Hồng

            if (CheckIsExistFormula(listElementFormula, formula, CatElementType.Comission.ToString().ToUpper(), ""))
            {
                //lấy doanh thu của shop trong tháng
                List<Sal_RevenueRecordEntity> revenueShopInMonth = new List<Sal_RevenueRecordEntity>();
                if (profileItem.ShopID != null)
                {
                    revenueShopInMonth = TotalData.listRevenueRecord.Where(m => m.ShopID == profileItem.ShopID).ToList();
                }

                if (TotalData.listKPIBonus != null && TotalData.listKPIBonus.Count > 0)
                {
                    foreach (var j in TotalData.listKPIBonus)
                    {
                        if (revenueShopInMonth.Any(m => m.KPIBonusID == j.ID))
                        {
                            listElementFormula.Add(new ElementFormula(CatElementType.Comission.ToString().ToUpper() + "_" + j.Code, revenueShopInMonth.Where(m => m.KPIBonusID == j.ID).FirstOrDefault().Amount, 0));
                        }
                        else
                        {
                            listElementFormula.Add(new ElementFormula(CatElementType.Comission.ToString().ToUpper() + "_" + j.Code, 0, 0));
                        }
                    }
                }
            }

            #region Phần tử lương hoa hồng đã tính được, trong bảng Sal_PaycCommission
            if (CheckIsExistFormula(listElementFormula, formula, "ELEMENT" + CatElementType.Comission.ToString().ToUpper() + "_", ""))
            {
                List<Cat_ElementEntity> listElementByCommission = TotalData.listElement_All.Where(m => m.GradePayrollID == null && m.MethodPayroll == MethodPayroll.E_COMMISSION_PAYMENT.ToString()).ToList();
                if (TotalData.listPayCommissionItem != null)
                {
                    //duyệt wa tất cả các phần tử
                    foreach (var element in listElementByCommission)
                    {
                        string elementCode = element.ElementCode.Replace("ELEMENT" + CatElementType.Comission.ToString().ToUpper() + "_", "");
                        Sal_PayCommissionItemEntity PayCommissionItem = TotalData.listPayCommissionItem.Where(m => m.ProfileID != null && m.Code.ReplaceSpace() == elementCode.ReplaceSpace()).FirstOrDefault();
                        if (PayCommissionItem != null)
                        {
                            item = new ElementFormula(element.ElementCode, PayCommissionItem.Value, 0);
                            listElementFormula.Add(item);
                        }
                        else
                        {
                            item = new ElementFormula(element.ElementCode, 0, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var element in listElementByCommission)
                    {
                        item = new ElementFormula(element.ElementCode, 0, 0);
                        listElementFormula.Add(item);
                    }
                }
            }
            #endregion

            #region Lấy dữ liệu các phần tử là tổng số lượng chức vụ (Position) trong shop
            if (CheckIsExistFormula(listElementFormula, formula, CatElementType.Comission.ToString().ToUpper() + "_COUNTPOSITION_", ""))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    if (TotalData.listPosition != null && TotalData.listPosition.Count > 0)
                    {
                        var lstProfile = repoProfile.FindBy(m => m.ShopID == profileItem.ShopID).ToList();
                        foreach (var j in TotalData.listPosition)
                        {
                            listElementFormula.Add(new ElementFormula(CatElementType.Comission.ToString().ToUpper() + "_COUNTPOSITION_" + j.Code, lstProfile.Where(m => m.PositionID == j.ID).Count(), 0));
                        }
                    }
                }
            }
            #endregion

            #region Lấy giá trị cho 2 enum là dòng sản phẩm và sản phẩm
            if (CheckIsExistFormula(listElementFormula, formula, new string[] { PayrollElement.SAL_COM_PERCENT_SHOP_LINEITEM.ToString(), PayrollElement.SAL_COM_PERCENT_SHOP_ITEM.ToString() }))
            {
                //lấy doanh thu của shop trong tháng
                List<Sal_RevenueRecordEntity> revenueShopInMonth = new List<Sal_RevenueRecordEntity>();
                if (profileItem.ShopID != null)
                {
                    revenueShopInMonth = TotalData.listRevenueRecord.Where(m => m.ShopID == profileItem.ShopID).ToList();
                }

                //SAL_COM_PERCENT_SHOP_5
                if (revenueShopInMonth.Any(m => m.Type == EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString()))
                {
                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PERCENT_SHOP_LINEITEM.ToString(), revenueShopInMonth.Where(m => m.Type == EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString()).FirstOrDefault().Amount, 0));
                }
                else
                {
                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PERCENT_SHOP_LINEITEM.ToString(), 0, 0));
                }

                //SAL_COM_PERCENT_SHOP_6
                if (revenueShopInMonth.Any(m => m.Type == EnumDropDown.SalesType.E_ITEM_MAJOR.ToString()))
                {
                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PERCENT_SHOP_ITEM.ToString(), revenueShopInMonth.Where(m => m.Type == EnumDropDown.SalesType.E_ITEM_MAJOR.ToString()).FirstOrDefault().Amount, 0));
                }
                else
                {
                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PERCENT_SHOP_ITEM.ToString(), 0, 0));
                }
            }


            #endregion

            #region Tính giá trị cho phần tử khấu trừ khi có nhân viên ko đủ thâm niên trong shop
            if (CheckIsExistFormula(listElementFormula, formula, PayrollElement.SAL_COM_BONUS_SCV.ToString()))
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    if (profileItem.ShopID != null)
                    {
                        //tính tiền khấu trừ khi khong đủ thâm niên
                        double Money_Deduction = 0;
                        try
                        {
                            //Hiện tại đang lấy trong webconfig, sau này sẻ lấy trong bảng setting
                            Money_Deduction = (double)GetObjectValue(TotalData.listElement_All, listElementFormula, TotalData.listElement_All.Where(m => m.ElementCode == ConstantPathWeb.Hrm_Sal_ElementName_Comission).FirstOrDefault().Formula);
                        }
                        catch { }

                        if (Money_Deduction != 0)//nhân viên này không đủ thâm niên
                        {
                            if (listTmpDeduction.Any(m => m.Key == profileItem.ShopID))//đã có nhân viên ko đủ thâm niên trong shop trước đó
                            {
                                double _tmp = listTmpDeduction.Single(m => m.Key == profileItem.ShopID).Value.Value;
                                int countValue = listTmpDeduction.Single(m => m.Key == profileItem.ShopID).Value.Count;
                                listTmpDeduction.Remove((Guid)profileItem.ShopID);//xóa phần tử trước đó
                                listTmpDeduction.Add((Guid)profileItem.ShopID, new ValueCount(Money_Deduction + _tmp, countValue++));//thêm lại phần tử đó và cập nhật lại value
                            }
                            else//là nhân viên ko đủ thâm niên đầu tiên trong shop
                            {
                                listTmpDeduction.Add((Guid)profileItem.ShopID, new ValueCount(Money_Deduction, 1));//thêm lại phần tử đó và cập nhật lại value
                            }
                            item = new ElementFormula(PayrollElement.SAL_COM_BONUS_SCV.ToString(), 0, 0);
                            listElementFormula.Add(item);
                        }
                        else//nhân viên này đủ thâm niên, kiểm tra xem shop của nhân viên này có nhân viên nào ko đủ thâm niên hay không
                        {
                            if (listTmpDeduction.Any(m => m.Key == profileItem.ShopID))//đã có nhân viên ko đủ thâm niên trong shop trước đó
                            {
                                int CountProfile = repoProfile.FindBy(m => m.ShopID == profileItem.ShopID).Count() - listTmpDeduction.Single(m => m.Key == profileItem.ShopID).Value.Count;
                                item = new ElementFormula(PayrollElement.SAL_COM_BONUS_SCV.ToString(), listTmpDeduction.Single(m => m.Key == profileItem.ShopID).Value.Value / CountProfile, 0);
                                listElementFormula.Add(item);
                            }
                            else
                            {
                                item = new ElementFormula(PayrollElement.SAL_COM_BONUS_SCV.ToString(), 0, 0);
                                listElementFormula.Add(item);
                            }
                        }
                    }
                    else
                    {
                        item = new ElementFormula(PayrollElement.SAL_COM_BONUS_SCV.ToString(), 0, 0);
                        listElementFormula.Add(item);
                    }
                }
            }

            #endregion

            #endregion

            #region lấy các phần tử Đánh Giá

            if (CheckIsExistFormula(listElementFormula, formula, CatElementType.Evaluation.ToString().ToUpper(), ""))
            {
                IList<string> List_EvaBonusType = Enum.GetValues(typeof(EvaBonusType))
                                          .Cast<EvaBonusType>()
                                          .Select(x => x.ToString())
                                          .ToList();

                if (TotalData.listSalesType != null && TotalData.listSalesType.Count > 0)
                {
                    foreach (var j in TotalData.listSalesType)
                    {
                        foreach (var t in List_EvaBonusType)
                        {
                            try
                            {
                                Eva_BonusSalaryEntity BonusSalaryITem = TotalData.listEva_BonusSalary.Where(m => m.ProfileID == profileItem.ID
                                    && m.SalesTypeID == j.ID
                                    && m.Type == t).FirstOrDefault();
                                if (BonusSalaryITem != null && BonusSalaryITem.Bonus != null)
                                {
                                    item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_" + j.Code + "_" + t, BonusSalaryITem.Bonus, 0);
                                    listElementFormula.Add(item);
                                }
                                else
                                {
                                    item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_" + j.Code + "_" + t, 0, 0);
                                    listElementFormula.Add(item);
                                }
                            }
                            catch
                            {
                                item = new ElementFormula(CatElementType.Evaluation.ToString().ToUpper() + "_" + j.Code + "_" + t, 0, 0);
                                listElementFormula.Add(item);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Vietject

            #region Lấy các phần tử đơn giá theo vai trò (Vietject)

            if (CheckIsExistFormula(listElementFormula, formula, CatElementType.FLIGHT.ToString() + "_", "_HOURS") ||
                CheckIsExistFormula(listElementFormula, formula, CatElementType.FLIGHT.ToString() + "_", "_ROUTES") ||
                CheckIsExistFormula(listElementFormula, formula, CatElementType.FLIGHT.ToString() + "_", "_AMOUNT"))
            {
                List<Att_TimeSheetEntity> Att_TimeSheetItem = TotalData.listAtt_TimeSheet.Where(m => m.ProfileID == profileItem.ID && m.Date <= CutOffDuration.DateEnd && m.Date >= CutOffDuration.DateStart).OrderByDescending(m => m.Date).ToList();
                List<Cat_UnitPriceEntity> Cat_UnitPrice = TotalData.listCat_UnitPrice.Where(m => m.Date <= CutOffDuration.DateEnd).OrderByDescending(m => m.Date).ToList();
                if (TotalData.listCat_Role != null && TotalData.listCat_Role.Count > 0 && TotalData.listCat_JobType != null && TotalData.listCat_JobType.Count > 0)
                {
                    foreach (var Role in TotalData.listCat_Role)
                    {
                        foreach (var JobType in TotalData.listCat_JobType)
                        {
                            var Att_TimeSheetItemByItem = Att_TimeSheetItem.Where(m => m.RoleID == Role.ID && m.JobTypeID == JobType.ID).OrderByDescending(m => m.Date).ToList();
                            var Cat_UnitPriceByItem = Cat_UnitPrice.Where(m => m.RoleID == Role.ID && m.JobTypeID == JobType.ID).OrderByDescending(m => m.Date).FirstOrDefault();
                            //số giờ bay
                            item = new ElementFormula(CatElementType.FLIGHT.ToString() + "_" + Role.Code.ReplaceSpace() + "_" + JobType.Code.ReplaceSpace() + "_HOURS", Att_TimeSheetItemByItem.Sum(m => m.NoHour), 0);
                            listElementFormula.Add(item);
                            //số chặn bay
                            item = new ElementFormula(CatElementType.FLIGHT.ToString() + "_" + Role.Code.ReplaceSpace() + "_" + JobType.Code.ReplaceSpace() + "_ROUTES", Att_TimeSheetItemByItem.Sum(m => m.Sector), 0);
                            listElementFormula.Add(item);

                            //số tiền
                            double Amount = Cat_UnitPriceByItem != null && Cat_UnitPriceByItem.Amount != null ? (double)Cat_UnitPriceByItem.Amount : 0;
                            item = new ElementFormula(CatElementType.FLIGHT.ToString() + "_" + Role.Code.ReplaceSpace() + "_" + JobType.Code.ReplaceSpace() + "_AMOUNT", Amount, 0);
                            listElementFormula.Add(item);
                        }
                    }
                }
            }
            #endregion

            #endregion

            return listElementFormula.Distinct().ToList();
        }

        /// <summary>
        /// Kiểm tra xem phần tử có nằm trong công thức hay không
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckIsExistFormula(List<ElementFormula> listElementFormula, Cat_ElementEntity formula, string value)
        {
            //loại bỏ các khoản trắng và xuống dòng
            formula.Formula = formula.Formula.Replace("\n", "").Replace("\t", "").Trim();

            List<string> listFormulaItem = ParseFormulaToList(formula.Formula);

            if (listFormulaItem.Any(m => m.Replace("[", "").Replace("]", "") == value.ReplaceSpace()))
            {
                if (!listElementFormula.Any(m => m.VariableName.ReplaceSpace() == value.ReplaceSpace()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra xem phần tử có nằm trong công thức hay không
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckIsExistFormula(List<ElementFormula> listElementFormula, Cat_ElementEntity formula, string[] value)
        {
            //loại bỏ các khoản trắng và xuống dòng
            formula.Formula = formula.Formula.Replace("\n", "").Replace("\t", "").Trim();

            //Tách các phần tử từ công thức ra một array
            List<string> listFormulaItem = ParseFormulaToList(formula.Formula);

            if (listFormulaItem.Any(m => value.Any(t => t == m.Replace("[", "").Replace("]", ""))))
            {
                if (value.Any(m => !listElementFormula.Any(t => t.VariableName == m.Replace("[", "").Replace("]", ""))))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  Kiểm tra xem phần tử có nằm trong công thức hay không
        /// </summary>
        /// <param name="listElementFormula"></param>
        /// <param name="formula"></param>
        /// <param name="StartsWith"></param>
        /// <param name="EndsWith"></param>
        /// <returns></returns>
        public bool CheckIsExistFormula(List<ElementFormula> listElementFormula, Cat_ElementEntity formula, string StartsWith, string EndsWith)
        {
            //loại bỏ các khoản trắng và xuống dòng
            formula.Formula = formula.Formula.Replace("\n", "").Replace("\t", "").Trim();

            //Tách các phần tử từ công thức ra một array
            List<string> listFormulaItem = ParseFormulaToList(formula.Formula);

            if (listFormulaItem.Any(m => m.Replace("[", "").Replace("]", "").StartsWith(StartsWith) && m.Replace("[", "").Replace("]", "").EndsWith(EndsWith)))
            {
                if (!listElementFormula.Any(m => m.VariableName.StartsWith(StartsWith) && m.VariableName.EndsWith(EndsWith)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIsExistFormula(List<ElementFormula> listElementFormula, Cat_ElementEntity formula, string StartsWith, string[] ArrayEndsWith)
        {
            //loại bỏ các khoản trắng và xuống dòng
            formula.Formula = formula.Formula.Replace("\n", "").Replace("\t", "").Trim();

            //Tách các phần tử từ công thức ra một array
            List<string> listFormulaItem = ParseFormulaToList(formula.Formula);
            foreach (var EndsWith in ArrayEndsWith)
            {
                if (listFormulaItem.Any(m => m.Replace("[", "").Replace("]", "").StartsWith(StartsWith) && m.Replace("[", "").Replace("]", "").EndsWith(EndsWith)))
                {
                    foreach (var i in listFormulaItem)
                    {
                        if (!listElementFormula.Any(m => m.VariableName == i.Replace("[", "").Replace("]", "")))
                        {
                            return true;
                        }
                    }
                    //if (!listElementFormula.Any(m => m.VariableName.StartsWith(StartsWith) && m.VariableName.EndsWith(EndsWith)))
                    //{
                    //    return true;
                    //}
                }
            }
            return false;
        }

        /// <summary>
        /// Hàm tìm kiếm chế độ lương
        /// </summary>
        /// <param name="listGrade">List Chế độ lương</param>
        /// <param name="ProfileID">ID Profile</param>
        /// <param name="GradePayrollID">GradePayrollID</param>
        /// <param name="MondthYear">Ngày trong kỳ lương</param>
        /// <returns></returns>
        public Sal_GradeEntity FindGradePayrollByProfileAndMonthYear(List<Sal_GradeEntity> listGrade, Guid ProfileID, DateTime DateStart, DateTime DateEnd)
        {
            List<Sal_GradeEntity> listModel = listGrade.Where(m =>
                m.ProfileID == ProfileID
                ).OrderByDescending(m => m.MonthStart).ToList();
            if (listModel.Count > 0)
            {
                return listModel.FirstOrDefault();
            }
            else
            {
                return new Sal_GradeEntity();
            }
        }

        /// <summary>
        /// Hàm tính tổng số ngày tạm hoãn công việc tính từ cuối kỳ lương trở về trước
        /// </summary>
        /// <param name="listModel"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public double SumStopWorkingDay(List<Hre_StopWorkingEntity> listModel, DateTime dateTo)
        {
            List<Hre_StopWorkingEntity> Result = new List<Hre_StopWorkingEntity>();
            double TotalDay = 0;
            //lọc ra các record đã được duyệt và ngày bắt đầu dừng công việc nằm trước ngày cuối cùng của kỳ lương
            Result = listModel.Where(m => m.Status == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString() && m.DateStop != null && m.DateStop <= dateTo).ToList();

            if (Result != null && Result.Count > 0)
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    //nếu ngày quay lại làm được duyệt
                    if (Result[i].StatusComeBack == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString())
                    {
                        TimeSpan span = (DateTime)Result[i].DateComeBack - (DateTime)Result[i].DateStop;
                        TotalDay += span.TotalDays;
                    }
                    else
                    {
                        TimeSpan span = dateTo - (DateTime)Result[i].DateStop;
                        TotalDay += span.TotalDays;
                        break;
                    }
                }
                return TotalDay;
            }
            else
            {
                return TotalDay;
            }
        }

        /// <summary>
        /// Get số người phụ thuộc trong kỳ tính lương của nhân viên
        /// </summary>
        /// <param name="listDependant"></param>
        /// <param name="ProfileID"></param>
        /// <param name="CutOffduration_Datefrom"></param>
        /// <param name="CutOffduration_DateTo"></param>
        /// <returns></returns>
        public int GetDependantNumber(List<Hre_DependantEntity> listDependant, Guid ProfileID, DateTime CutOffduration_Datefrom, DateTime CutOffduration_DateTo)
        {
            int result = 0;
            DateTime from = new DateTime(CutOffduration_Datefrom.Year, CutOffduration_Datefrom.Month, 1);
            DateTime to = new DateTime(CutOffduration_DateTo.Year, CutOffduration_DateTo.Month, 1);
            listDependant = listDependant.Where(m => m.ProfileID == ProfileID).ToList();

            for (int i = 0; i < listDependant.Count; i++)
            {
                listDependant[i].MonthOfEffect = new DateTime(listDependant[i].MonthOfEffect.Value.Year, listDependant[i].MonthOfEffect.Value.Month, 1);
                if (listDependant[i].MonthOfExpiry != null)
                {
                    listDependant[i].MonthOfExpiry = new DateTime(listDependant[i].MonthOfExpiry.Value.Year, listDependant[i].MonthOfExpiry.Value.Month, 1);
                }
            }
            result = listDependant.Where(m => m.MonthOfEffect <= CutOffduration_DateTo && (m.MonthOfExpiry == null || m.MonthOfExpiry >= CutOffduration_Datefrom)).Count();

            return result;
        }
        //private static Thread threadCompute;

        /// <summary>
        /// Tính giữ lương
        /// </summary>
        /// <param name="listProfileIds">Id bảng HoldSalary</param>
        /// <param name="CutoffdurationID">Kỳ tính lương</param>
        /// <returns></returns>
        public ResultsObject ComputeHoldSalary(List<Guid> listHoldSalaryIDs, Guid CutoffdurationID, string UserLogin)
        {
            try
            {
                Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
                string elementBeforeTax = Sys_Services.GetConfigValue<string>(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT);
                string elementAfterTax = Sys_Services.GetConfigValue<string>(AppConfig.HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX);
                //Nếu chưa có config thì out
                if (string.IsNullOrEmpty(elementBeforeTax) || string.IsNullOrEmpty(elementAfterTax))
                {
                    return new ResultsObject() { Success = false, Messenger = ConstantDisplay.HRM_Sal_HoldSalary_ConfigElementHoldSalary.TranslateString() };
                }

                using (var context = new VnrHrmDataContext())
                {
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                    var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                    var repoSal_HoldSalary = new CustomBaseRepository<Sal_HoldSalary>(unitOfWork);
                    var repoHre_Profile = new CustomBaseRepository<Hre_ProfileEntity>(unitOfWork);
                    var repoPayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                    var payrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                    #region Get data
                    List<object> listModel = new List<object>();
                    listModel.AddRange(new object[3]);
                    listModel[1] = 1;
                    listModel[2] = Int32.MaxValue - 1;
                    List<Att_CutOffDurationEntity> listCutoffduration_All = GetData<Att_CutOffDurationEntity>(listModel, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();

                    listModel = new List<object>();
                    listModel.AddRange(new object[5]);
                    listModel[3] = 1;
                    listModel[4] = Int32.MaxValue - 1;
                    List<Sal_HoldSalary> listHoldSalary = repoSal_HoldSalary.FindBy(m => m.IsDelete != true && m.Terminate != true).ToList();
                    listHoldSalary = listHoldSalary.Where(m => listHoldSalaryIDs.Any(t => t == m.ID)).ToList();

                    listModel = new List<object>();
                    listModel.AddRange(new object[7]);
                    listModel[5] = 1;
                    listModel[6] = Int32.MaxValue - 1;
                    List<Cat_ElementEntity> listElement = GetData<Cat_ElementEntity>(listModel, ConstantSql.hrm_cat_sp_get_Element_All, UserLogin, ref status);

                    listModel = new List<object>();
                    listModel.AddRange(new object[18]);
                    listModel[16] = 1;
                    listModel[17] = Int32.MaxValue - 1;
                    List<Hre_ProfileEntity> listHre_profile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();
                    #endregion

                    //Get byid cutoff
                    Att_CutOffDurationEntity CutoffdurationItem = listCutoffduration_All.Single(m => m.ID == CutoffdurationID);

                    //Lọc cutoff, chỉ lấy cutoff có tháng/năm lớn hơn tháng/năm của nhân viên bị giữ lương có ngày bắt đầu giữ lương nhỏ nhất
                    DateTime dateCutoff = new DateTime(CutoffdurationItem.MonthYear.Year, CutoffdurationItem.MonthYear.Month, 1);
                    var _tmp = listHoldSalary.FirstOrDefault().MonthSalary.Value;
                    DateTime dateFirstHold = new DateTime(_tmp.Year, _tmp.Month, 1);
                    List<Att_CutOffDurationEntity> listCutoffduration = listCutoffduration_All.Where(m => new DateTime(m.MonthYear.Year, m.MonthYear.Month, 1) >= dateFirstHold && new DateTime(m.MonthYear.Year, m.MonthYear.Month, 1) < dateCutoff).ToList();

                    //Get dữ liệu lập công thức
                    ComputePayrollDataModel TotalData = GetDataForComputeSalary(CutoffdurationItem, UserLogin);

                    //Biến lưu giá trị tính được qua từng tháng theo từng nhân viên
                    Dictionary<Guid, double[]> listAmountHoldSalary = new Dictionary<Guid, double[]>();

                    for (int i = 0; i < listCutoffduration.Count; i++)
                    {
                        //lấy lại các dữ liệu liên quan tới cutoff
                        TotalData = GetDataForComputeSalary_ForCutOff(TotalData, listCutoffduration[i], UserLogin);

                        //biến tạm lưu các nhân viên được tính kỳ đang duyệt qua
                        List<Sal_HoldSalary> listProfile_Any_Cutoff = new List<Sal_HoldSalary>();
                        listProfile_Any_Cutoff = listHoldSalary.Where(m => new DateTime(m.MonthSalary.Value.Year, m.MonthSalary.Value.Month, 1) <= new DateTime(listCutoffduration[i].MonthYear.Year, listCutoffduration[i].MonthYear.Month, 1)).ToList();

                        //bắt đầu tính qua từng nhân viên bị giữ lương trong tháng đang duyệt qua
                        for (int j = 0; j < listProfile_Any_Cutoff.Count; j++)
                        {
                            #region Tính công thức
                            //tính công thức
                            var listElementByConfigBeforeTax = listElement.FirstOrDefault(m => m.ElementCode == elementBeforeTax);
                            var listElementByConfigAfterTax = listElement.FirstOrDefault(m => m.ElementCode == elementAfterTax);
                            List<Cat_ElementEntity> ListElement = new List<Cat_ElementEntity>();
                            ListElement.Add(listElementByConfigBeforeTax);
                            ListElement.Add(listElementByConfigAfterTax);
                            List<ElementFormula> listElementFormula = new List<ElementFormula>();
                            listElementFormula = ParseElementFormula(listElementFormula, ListElement, TotalData, listHre_profile.Single(m => m.ID == listProfile_Any_Cutoff[j].ProfileID), listCutoffduration[i], new Dictionary<Guid, ValueCount>(), false, new TraceLogManager());

                            //lưu vài biến tổng
                            if (listElementFormula != null)
                            {
                                if (listAmountHoldSalary.Any(m => m.Key == listProfile_Any_Cutoff[j].ID))
                                {
                                    listAmountHoldSalary[listProfile_Any_Cutoff[j].ID][0] += double.Parse(listElementFormula.FirstOrDefault(m => m.VariableName == listElementByConfigBeforeTax.ElementCode).Value.ToString());
                                    listAmountHoldSalary[listProfile_Any_Cutoff[j].ID][1] += double.Parse(listElementFormula.FirstOrDefault(m => m.VariableName == listElementByConfigAfterTax.ElementCode).Value.ToString());
                                }
                                else
                                {
                                    var ElementByConfigBeforeTax = listElementFormula.FirstOrDefault(m => m.VariableName == listElementByConfigBeforeTax.ElementCode);
                                    var ElementByConfigAfterTax = listElementFormula.FirstOrDefault(m => m.VariableName == listElementByConfigAfterTax.ElementCode);

                                    listAmountHoldSalary.Add(listProfile_Any_Cutoff[j].ID, new double[] { double.Parse(ElementByConfigBeforeTax.Value.ToString()), 0 });

                                    listAmountHoldSalary[listProfile_Any_Cutoff[j].ID][1] = double.Parse(ElementByConfigAfterTax.Value.ToString());
                                }
                            }
                            #endregion
                        }
                    }

                    for (int i = 0; i < listHoldSalary.Count; i++)
                    {
                        listHoldSalary[i].MonthEndSalary = CutoffdurationItem.MonthYear;
                        if (listAmountHoldSalary.Any(m => m.Key == listHoldSalary[i].ID))
                        {
                            listHoldSalary[i].AmountSalary = listAmountHoldSalary[listHoldSalary[i].ID][0];
                            listHoldSalary[i].AmountSalaryAfterTax = listAmountHoldSalary[listHoldSalary[i].ID][1];
                        }
                        else
                        {
                            listHoldSalary[i].AmountSalary = null;
                            listHoldSalary[i].AmountSalaryAfterTax = null;
                        }
                    }
                    unitOfWork.SaveChanges();
                    return new ResultsObject();
                }
            }
            catch (Exception ex)
            {
                return new ResultsObject() { Success = false, Messenger = ex.Message };
            }
        }


        /// <summary>
        /// Kiểm tra lấy dữ liệu Backup hay không?
        /// </summary>
        /// <param name="typeEnumBk">Mã loại dữ liệu đc backup (lấy trong "TypeDataBKInScheduleTask")</param>
        /// <param name="dateViewReport">Thời gian xem báo cáo (lấy ngày DateStart của kỳ lương hoặc kỳ công) </param>
        /// <returns>True/False</returns>
        public Boolean CheckDataIsBackUp(String typeEnumBk, DateTime dateViewReport)
        {
            using (var context = new VnrHrmDataContext())
            {
                Boolean res = false;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var resSysAutoBk = new CustomBaseRepository<Sys_AutoBackupEntity>(unitOfWork);

                //Lấy dòng dữ liệu trong ScheduleTask theo đũng mã dữ liệu đc backup (Dữ liệu Lương hoặc Công. vd: E_PAYROLL_BK)
                var sysAutoBk = unitOfWork.CreateQueryable<Sys_AutoBackup>(Guid.Empty, m => m.Code == typeEnumBk).FirstOrDefault();
                if (sysAutoBk != null)
                {
                    //Thời gian bắt đầu Backup 
                    DateTime dateStartBk = new DateTime();
                    if (sysAutoBk.DateStart != null)
                    {
                        dateStartBk = sysAutoBk.DateStart.Value;
                    }

                    //Thời gian chờ 
                    int timeWait = 0;
                    if (sysAutoBk.TimeWaiting != null)
                    {
                        timeWait = sysAutoBk.TimeWaiting.Value;
                    }

                    //Loại thời gian chờ (loại: phút, giờ, ngày, tháng)
                    var typeWait = sysAutoBk.Type;

                    //Thời gian chênh lệch khi kiểm tra
                    DateTime datecheck = new DateTime();

                    //Kiểm tra ngày xem báo cáo và ngày bắt đầu backup theo loại thời gian chờ 
                    //=> add thời gian chờ vào thời gian bắt đầu backup
                    if (typeWait == ScheduleTaskType.ByMinutes.ToString())
                    {
                        datecheck = dateStartBk.AddMinutes(timeWait);
                    }
                    else if (typeWait == ScheduleTaskType.ByHours.ToString())
                    {
                        datecheck = dateStartBk.AddHours(timeWait);
                    }
                    else if (typeWait == ScheduleTaskType.ByDays.ToString())
                    {
                        datecheck = dateStartBk.AddDays(timeWait);
                    }
                    else if (typeWait == ScheduleTaskType.ByMonths.ToString())
                    {
                        datecheck = dateStartBk.AddMonths(timeWait);
                    }

                    //Nếu thời gian xem báo cáo > thời gian datecheck => lấy dữ liệu backup
                    if (dateViewReport <= datecheck)
                    {
                        res = true;
                    }
                }
                return res;
            }
        }


        public void Approved(Guid selectedIds, string status, Guid? workPlaceID, string OrderNumber, string userLoginName)
        {

            using (var context = new VnrHrmDataContext())
            {
                string statusMessage = string.Empty;

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                int[] orderNumber = null;
                if (!string.IsNullOrEmpty(OrderNumber))
                {
                    orderNumber = OrderNumber.Split(',').Select(s => int.Parse(s)).ToArray();
                }
                string strOrg = string.Empty;
                if (orderNumber != null)
                {
                    strOrg = string.Join(",", orderNumber.OrderBy(s => s).ToList());
                }

                var lockObjectServices = new Sys_LockObjectServices();
                var objLockObj = new List<object>();
                objLockObj.AddRange(new object[5]);

                objLockObj[3] = 1;
                objLockObj[4] = int.MaxValue - 1;
                var lstLockObject = lockObjectServices.GetData<Sys_LockObjectEntity>(objLockObj, ConstantSql.hrm_sys_sp_get_LockObject, userLoginName, ref statusMessage).Where(s => s.Type == "E_APPROVED_PAYROLL").ToList();

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                List<Att_CutOffDurationEntity> listCutoffduration_All = GetData<Att_CutOffDurationEntity>(listModel, ConstantSql.hrm_att_sp_get_CutOffDurations, userLoginName, ref status).Where(s => s.ID == selectedIds).ToList();

                var cutOffEntity = new Att_CutOffDurationEntity();
                if (selectedIds != Guid.Empty)
                {
                    cutOffEntity = listCutoffduration_All.Where(s => selectedIds == s.ID).FirstOrDefault();
                }
                var lockObjectEntity = new Sys_LockObjectEntity();
                if (cutOffEntity != null)
                {
                    lockObjectEntity = lstLockObject.Where(s => s.DateStart == cutOffEntity.DateStart && s.DateEnd == cutOffEntity.DateEnd).FirstOrDefault();
                }

                if (lockObjectEntity != null)
                {
                    if (orderNumber != null)
                    {
                        lockObjectEntity.OrgStructures = Common.ListNumbersToBinary(orderNumber.ToList());
                        lockObjectEntity.WorkPlaceID = workPlaceID;
                        lockObjectServices.Edit(lockObjectEntity);
                    }
                }
                else
                {
                    if (orderNumber != null)
                    {
                        var lockObjectNewEntity = new Sys_LockObjectEntity();
                        lockObjectNewEntity.DateStart = cutOffEntity.DateStart;
                        lockObjectNewEntity.DateEnd = cutOffEntity.DateEnd;
                        lockObjectNewEntity.OrgStructures = Common.ListNumbersToBinary(orderNumber.ToList());
                        lockObjectNewEntity.Status = EnumDropDown.LockObjectStatus.E_WAITING_APPROVED.ToString();
                        lockObjectNewEntity.WorkPlaceID = workPlaceID;
                        lockObjectNewEntity.UserCreate = userLoginName;
                        lockObjectNewEntity.Type = "E_APPROVED_PAYROLL";
                        lockObjectServices.Add(lockObjectNewEntity);
                    }
                }

            }
        }


    }



}
