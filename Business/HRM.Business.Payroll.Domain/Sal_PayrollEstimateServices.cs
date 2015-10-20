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
    public class Sal_PayrollEstimateServices : BaseService
    {
        public bool ComputePayrollEstimate(List<Sal_PayrollEstimateDetailEntity> listPayrollEstimateDetail, Sal_PayrollEstimateEntity model, string userLogin)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoCutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                    var repoPayrollEstimateDetail = new CustomBaseRepository<Sal_PayrollEstimateDetail>(unitOfWork);
                    var repoPayrollEstimate = new CustomBaseRepository<Sal_PayrollEstimate>(unitOfWork);

                    List<Att_CutOffDuration> listCutoffDuration = repoCutOffDuration.FindBy(m => m.IsDelete != true && m.MonthYear <= model.MonthEnd && m.MonthYear >= model.MonthStart).ToList();

                    #region Delete Sal_PayrollEstimate và Sal_PayrollEstimateDetail

                    List<Sal_PayrollEstimateDetail> ListPayrollEstimateDetailDelete = new List<Sal_PayrollEstimateDetail>();
                    List<Sal_PayrollEstimate> ListPayrollEstimateDelete = new List<Sal_PayrollEstimate>();

                    ListPayrollEstimateDelete = repoPayrollEstimate.FindBy(m => m.IsDelete != true && m.OrgStructureID == model.OrgStructureID).ToList();
                    ListPayrollEstimateDelete = ListPayrollEstimateDelete.Where(m => listCutoffDuration.Any(t => t.ID == m.CutOffDurationID)).ToList();

                    ListPayrollEstimateDetailDelete = repoPayrollEstimateDetail.FindBy(m => m.IsDelete != true).ToList();
                    ListPayrollEstimateDetailDelete = ListPayrollEstimateDetailDelete.Where(m => ListPayrollEstimateDelete.Any(t => t.ID == m.PayrollEstimateID)).ToList();

                    ListPayrollEstimateDelete.ForEach(m => m.IsDelete = true);
                    ListPayrollEstimateDetailDelete.ForEach(m => m.IsDelete = true);

                    unitOfWork.SaveChanges();
                    #endregion
                    
                    //Giờ công chuẩn lấy trong web config
                    double TotalAmount = 0;

                    //lay du lieu  cau hinh
                    string status = string.Empty;
                    Sys_AllSettingServices sysServices = new Sys_AllSettingServices();
                    var AllSetting = sysServices.GetData<Sys_AllSettingEntity>("HRM_SAL_PAYROLL_ESTIMATE_SALRYAVERAGE", ConstantSql.hrm_sys_sp_get_AllSettingByKey, userLogin, ref status).FirstOrDefault();
                    double StandardHour = 200;
                    Double.TryParse(AllSetting != null ? AllSetting.Value2 : "a", out StandardHour);

                    foreach (var CutOff in listCutoffDuration)
                    {
                        //reset tổng tiền master
                        TotalAmount = 0;
                        //tạo dữ liệu master
                        Sal_PayrollEstimate Master = new Sal_PayrollEstimate();
                        Master.ID = Guid.NewGuid();
                        Master.OrgStructureID = model.OrgStructureID;
                        Master.CutOffDurationID = CutOff.ID;
                        Master.PayrollGroupID = model.PayrollGroupID;
                        Master.RateAdjust = model.RateAdjust;
                        Master.OrgStructureType = model.OrgStructureType;
                        Master.BonusBudget = model.BonusBudget;
                        Master.StatusEmp = model.StatusEmp;

                        foreach (var i in listPayrollEstimateDetail)
                        {
                            #region Check null value
                            i.SalaryAverage = i.SalaryAverage != null ? (double)i.SalaryAverage : 0;
                            i.QuantityEmp = i.QuantityEmp != null ? (double)i.QuantityEmp : 0;
                            i.LeaveUnpaid = i.LeaveUnpaid != null ? (double)i.LeaveUnpaid : 0;

                            i.OvertimeNormal = i.OvertimeNormal != null ? (double)i.OvertimeNormal : 0;
                            i.OvertimeNightNormal = i.OvertimeNightNormal != null ? (double)i.OvertimeNightNormal : 0;
                            i.OvertimeWeekend = i.OvertimeWeekend != null ? (double)i.OvertimeWeekend : 0;
                            i.OvertimeNightWeekend = i.OvertimeNightWeekend != null ? (double)i.OvertimeNightWeekend : 0;
                            i.OvertimeHoliday = i.OvertimeHoliday != null ? (double)i.OvertimeHoliday : 0;
                            i.OvertimeNightHoliday = i.OvertimeNightHoliday != null ? (double)i.OvertimeNightHoliday : 0;

                            model.RateAdjust = model.RateAdjust != null ? (double)model.RateAdjust : 0;
                            model.BonusBudget = model.BonusBudget != null ? (double)model.BonusBudget : 0;
                            #endregion

                            #region Tính toán số liệu
                            double AmountHour = (double)i.SalaryAverage * (double)i.QuantityEmp;
                            double AmountLeaveDay = (double)i.SalaryAverage * (double)i.LeaveUnpaid != 0 ? ((double)i.LeaveUnpaid / StandardHour) : 1;
                            double AmountOvertime = 0;
                            AmountOvertime += (double)i.SalaryAverage * 1.5 * (double)i.OvertimeNormal;
                            AmountOvertime += (double)i.SalaryAverage * 1.95 * (double)i.OvertimeNightNormal;
                            AmountOvertime += (double)i.SalaryAverage * 2.0 * (double)i.OvertimeWeekend;
                            AmountOvertime += (double)i.SalaryAverage * 2.6 * (double)i.OvertimeNightWeekend;
                            AmountOvertime += (double)i.SalaryAverage * 3.0 * (double)i.OvertimeHoliday;
                            AmountOvertime += (double)i.SalaryAverage * 3.9 * (double)i.OvertimeNightHoliday;
                            double Percent = (double)model.RateAdjust != 0 ? (double)model.RateAdjust / 100 : 1;
                            double AmountTotal = ((AmountHour - AmountLeaveDay + AmountOvertime) * Percent) + (double)model.BonusBudget;
                            //cập nhật tổng tiền cho master
                            TotalAmount += AmountTotal;
                            #endregion

                            //tạo dữ liệu detail
                            Sal_PayrollEstimateDetail Detail = new Sal_PayrollEstimateDetail();
                            Detail.ID = Guid.NewGuid();
                            Detail.SalaryAverage = i.SalaryAverage;
                            Detail.PayrollEstimateID = Master.ID;
                            Detail.OrgStructureID = i.OrgStructureID;
                            Detail.LeaveUnpaid = i.LeaveUnpaid;
                            Detail.OvertimeHoliday = i.OvertimeHoliday;
                            Detail.OvertimeNightHoliday = i.OvertimeNightHoliday;
                            Detail.OvertimeNightNormal = i.OvertimeNightNormal;
                            Detail.OvertimeNightWeekend = i.OvertimeNightWeekend;
                            Detail.QuantityEmp = i.QuantityEmp;
                            Detail.OvertimeNormal = i.OvertimeNormal;
                            Detail.OvertimeWeekend = i.OvertimeWeekend;
                            Detail.AmountHour = AmountHour;
                            Detail.AmountLeaveDay = AmountLeaveDay;
                            Detail.AmountOvertime = AmountOvertime;
                            Detail.AmountTotal = AmountTotal;
                            repoPayrollEstimateDetail.Add(Detail);
                        }

                        //update tổng tiền
                        Master.AmountTotal = TotalAmount;
                        repoPayrollEstimate.Add(Master);
                    }
                    unitOfWork.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Sal_PayrollEstimateDetailEntity> GetTemplatePayrollEstimate(Sal_PayrollEstimateEntity model)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoPayrollEstimateDetail = new CustomBaseRepository<Sal_PayrollEstimateDetail>(unitOfWork);

                List<Cat_OrgStructure> listOrg = repoOrgStructure.FindBy(m => m.IsDelete != true).ToList();
                if (model.OrgStructureIDs != null)
                {
                    string[] listOrdernumber = model.OrgStructureIDs.Split(',');
                    listOrg = listOrg.Where(m => listOrdernumber.Contains(m.OrderNumber.ToString())).ToList();
                }

                List<Sal_PayrollEstimateDetail> ListPayrollEstimateDetail = new List<Sal_PayrollEstimateDetail>();
                ListPayrollEstimateDetail = repoPayrollEstimateDetail.FindBy(m => m.IsDelete != true && m.PayrollEstimateID == null).ToList();

                List<Sal_PayrollEstimateDetailEntity> Result = new List<Sal_PayrollEstimateDetailEntity>();
                foreach (var i in listOrg)
                {
                    Sal_PayrollEstimateDetailEntity item = new Sal_PayrollEstimateDetailEntity();
                    item.ID = Guid.NewGuid();
                    item.OrgStructureName = i.OrgStructureName;
                    item.OrgStructureCode = i.Code;
                    item.OrgStructureID = i.ID;
                    var _tmp = ListPayrollEstimateDetail.FirstOrDefault(m => m.OrgStructureID == i.ID);
                    if (_tmp != null)
                    {
                        item.LeaveUnpaid = _tmp.LeaveUnpaid != null ? _tmp.LeaveUnpaid : 0;
                        item.OvertimeHoliday = _tmp.OvertimeHoliday != null ? _tmp.OvertimeHoliday : 0;
                        item.OvertimeNightHoliday = _tmp.OvertimeNightHoliday != null ? _tmp.OvertimeNightHoliday : 0;
                        item.OvertimeNightNormal = _tmp.OvertimeNightNormal != null ? _tmp.OvertimeNightNormal : 0;
                        item.OvertimeNightWeekend = _tmp.OvertimeNightWeekend != null ? _tmp.OvertimeNightWeekend : 0;
                        item.OvertimeNormal = _tmp.OvertimeNormal != null ? _tmp.OvertimeNormal : 0;
                        item.OvertimeWeekend = _tmp.OvertimeWeekend != null ? _tmp.OvertimeWeekend : 0; 
                    }
                    else
                    {
                        item.LeaveUnpaid = 0;
                        item.OvertimeHoliday = 0;
                        item.OvertimeNightHoliday = 0;
                        item.OvertimeNightNormal = 0;
                        item.OvertimeNightWeekend = 0;
                        item.OvertimeNormal = 0;
                        item.OvertimeWeekend = 0;
                    }
                    Result.Add(item);
                }
                return Result;
            }
        }

        public bool SaveTemplatePayrollEstimate(List<Sal_PayrollEstimateDetailEntity> listEntity)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoPayrollEstimateDetail = new CustomBaseRepository<Sal_PayrollEstimateDetail>(unitOfWork);

                List<Sal_PayrollEstimateDetail> ListPayrollEstimateDetail = new List<Sal_PayrollEstimateDetail>();
                ListPayrollEstimateDetail = repoPayrollEstimateDetail.FindBy(m => m.IsDelete != true && m.PayrollEstimateID == null).ToList();

                foreach (var i in listEntity)
                {
                    var _tmp = ListPayrollEstimateDetail.FirstOrDefault(m => m.OrgStructureID == i.OrgStructureID);
                    if (_tmp != null)
                    {
                        _tmp.LeaveUnpaid = i.LeaveUnpaid;
                        _tmp.OvertimeHoliday = i.OvertimeHoliday;
                        _tmp.OvertimeNightHoliday = i.OvertimeNightHoliday;
                        _tmp.OvertimeNightNormal = i.OvertimeNightNormal;
                        _tmp.OvertimeNightWeekend = i.OvertimeNightWeekend;
                        _tmp.OvertimeNormal = i.OvertimeNormal;
                        _tmp.OvertimeWeekend = i.OvertimeWeekend;
                    }
                    else
                    {
                        Sal_PayrollEstimateDetail item = new Sal_PayrollEstimateDetail();
                        item.ID = Guid.NewGuid();
                        item.OrgStructureID = i.OrgStructureID;
                        item.LeaveUnpaid = i.LeaveUnpaid;
                        item.OvertimeHoliday = i.OvertimeHoliday;
                        item.OvertimeNightHoliday = i.OvertimeNightHoliday;
                        item.OvertimeNightNormal = i.OvertimeNightNormal;
                        item.OvertimeNightWeekend = i.OvertimeNightWeekend;
                        item.OvertimeNormal = i.OvertimeNormal;
                        item.OvertimeWeekend = i.OvertimeWeekend;
                        repoPayrollEstimateDetail.Add(item);
                    }
                }
                unitOfWork.SaveChanges();
                return true;
            }
        }
    }
}
