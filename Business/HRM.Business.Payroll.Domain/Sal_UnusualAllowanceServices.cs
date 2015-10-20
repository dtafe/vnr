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

namespace HRM.Business.Payroll.Domain
{
    public class Sal_UnusualAllowanceServices : BaseService
    {
        public void SaveUnusualAllowance(List<Sal_UnusualAllowanceEntity> model)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoUnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);

                repoUnusualAllowance.Add(model.Translate<Sal_UnusualAllowance>());
                unitOfWork.SaveChanges();
            }
        }

        public Guid ComputeBonusUnusualAllowance(Sal_UnusualAllowanceEntity model, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoCat_Element = new CustomBaseRepository<Cat_Element>(unitOfWork);


                //tạo asynTask
                Sys_AsynTask asynTask = new Sys_AsynTask()
                {
                    ID = Guid.NewGuid(),
                    Summary = "Tính thưởng : " + model.MonthStart.Value.ToString("dd/MM/yyyy"),
                    Type = AsynTask.Payroll_Compute_BonusUnusualAllowance.ToString(),
                    Status = AsynTaskStatus.Doing.ToString(),
                    TimeStart = DateTime.Now,
                    PercentComplete = 0.01,
                };
                repoSys_AsynTask.Add(asynTask);
                unitOfWork.SaveChanges();

                Thread threadCompute = new Thread(() => ComputeBonusUnusualAllowance_Progress(asynTask.ID, model, UserLogin));
                threadCompute.Start();

                return asynTask.ID;
            }

        }


        public Guid ComputeAnnualLeaveAllowance(Sal_UnusualAllowanceEntity model, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoCat_Element = new CustomBaseRepository<Cat_Element>(unitOfWork);


                //tạo asynTask
                Sys_AsynTask asynTask = new Sys_AsynTask()
                {
                    ID = Guid.NewGuid(),
                    Summary = "Tính phép năm : " + model.MonthStart.Value.ToString("dd/MM/yyyy"),
                    Type = AsynTask.Payroll_Compute_AnnualLeaveAllowance.ToString(),
                    Status = AsynTaskStatus.Doing.ToString(),
                    TimeStart = DateTime.Now,
                    PercentComplete = 0.01,
                };
                repoSys_AsynTask.Add(asynTask);
                unitOfWork.SaveChanges();

                Thread threadCompute = new Thread(() => ComputeBonusUnusualAllowance_Progress(asynTask.ID, model, UserLogin, true));
                threadCompute.Start();

                return asynTask.ID;
            }
        }

        public void ComputeBonusUnusualAllowance_Progress(Guid AsynTaskID, Sal_UnusualAllowanceEntity model, string UserLogin, bool AllowanceEvaluationYear = false)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSys_AsynTask = new CustomBaseRepository<Sys_AsynTask>(unitOfWork);
                var repoCat_Element = new CustomBaseRepository<Cat_Element>(unitOfWork);
                var repoCat_UnusualAllowanceCfg = new CustomBaseRepository<Cat_UnusualAllowanceCfg>(unitOfWork);
                var repoUnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);

                Sys_AsynTask asynTask = repoSys_AsynTask.GetById(AsynTaskID);

                Sal_ComputePayrollServices CumputePayrollServices = new Sal_ComputePayrollServices();
                List<object> listModel = new List<object>();
                string status = string.Empty;
                Sal_ComputePayrollServices Services = new Sal_ComputePayrollServices();
                List<Sal_UnusualAllowanceEntity> ListResult = new List<Sal_UnusualAllowanceEntity>();
                List<ElementFormula> listElementFormula = new List<ElementFormula>();
                Sal_UnusualAllowanceServices UnusualAllowanceServices = new Sal_UnusualAllowanceServices();

                Att_CutOffDurationEntity CutOffDuration = new Att_CutOffDurationEntity();
                if (model.MonthEnd != null)
                {
                    CutOffDuration.MonthYear = model.MonthStart.Value;
                    CutOffDuration.DateStart = model.MonthStart.Value;
                    CutOffDuration.DateEnd = model.MonthEnd.Value;
                }
                else
                {
                    CutOffDuration.MonthYear = new DateTime(model.MonthStart.Value.Year, model.MonthStart.Value.Month, 1);
                    CutOffDuration.DateStart = new DateTime(model.MonthStart.Value.Year, model.MonthStart.Value.Month, 1);
                    CutOffDuration.DateEnd = new DateTime(model.MonthStart.Value.Year, model.MonthStart.Value.Month, 1).AddMonths(1).AddDays(-1);
                }

                List<Hre_ProfileEntity> ListProfile = new List<Hre_ProfileEntity>();

                if (model.OrgStructureIDs != null && model.OrgStructureIDs != string.Empty)
                {
                    listModel = new List<object>();
                    listModel.AddRange(new object[18]);
                    listModel[2] = model.OrgStructureIDs;
                    listModel[16] = 1;
                    listModel[17] = Int32.MaxValue - 1;
                    ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);
                    if (model.ExProfileIDs != null)
                    {
                        string[] ExProfile = model.ExProfileIDs.Split(',');
                        ListProfile = ListProfile.Where(m => ExProfile.Contains(m.ID.ToString())).ToList();
                    }
                }
                else if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    ListProfile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status);
                }


                //lọc profile theo workplace
                if (model.WorkingPlaceID != null)
                {
                    string[] listWorkPlare = model.WorkingPlaceID.Split(',');
                    ListProfile = ListProfile.Where(m => listWorkPlare.Contains(m.WorkPlaceID.ToString())).ToList();
                }

                if (ListProfile == null || ListProfile.Count <= 0)
                {
                    asynTask.PercentComplete = 1;
                    unitOfWork.SaveChanges();
                    return;
                }

                List<Cat_UnusualAllowanceCfg> listUnusualAllowanceCfg = new List<Cat_UnusualAllowanceCfg>();
                listUnusualAllowanceCfg = repoCat_UnusualAllowanceCfg.FindBy(m => m.IsDelete != true).ToList();

                #region Delete các phụ cấp trong năm đã có
                List<Sal_UnusualAllowance> listUnusualAllowance = repoUnusualAllowance.FindBy(m => m.IsDelete != true && m.MonthStart != null && model.MonthStart != null && m.MonthStart.Value.Year == model.MonthStart.Value.Year && m.Status != WorkHistoryStatus.E_APPROVED.ToString()).ToList();

                //nếu là tính phép năm sức khỏe tốt thì delete thêm loại GoodHealth
                if (AllowanceEvaluationYear)
                {
                    var _tmp = listUnusualAllowanceCfg.FirstOrDefault(m => m.Code == "GoodHealth");
                    listUnusualAllowance = listUnusualAllowance.Where(m => (m.UnusualEDTypeID == model.UnusualEDTypeID || m.UnusualEDTypeID == _tmp.ID) && ListProfile.Any(t => t.ID == m.ProfileID)).ToList();
                }
                else
                {
                    listUnusualAllowance = listUnusualAllowance.Where(m => m.UnusualEDTypeID == model.UnusualEDTypeID && ListProfile.Any(t => t.ID == m.ProfileID)).ToList();
                }
                //update isdelete is true
                listUnusualAllowance.ForEach(m => m.IsDelete = true);
                #endregion

                //Cat_ElementEntity ElementItem = repoCat_Element.GetById((Guid)model.Element).Copy<Cat_ElementEntity>();
                Cat_ElementEntity ElementItem = repoCat_Element.FindBy(m => m.ID == (Guid)model.Element).FirstOrDefault().Copy<Cat_ElementEntity>();


                ComputePayrollDataModel TotalData = Services.GetDataForComputeSalary(CutOffDuration, UserLogin);

                List<Cat_ElementEntity> listElementAll = new List<Cat_ElementEntity>();
                listElementAll = TotalData.listElement_All;

                #region Xử lý cho tính phép năm, sức khỏe tốt
                List<Cat_UnusualAllowanceCfg> ListUnusualAllowanceCfg = new List<Cat_UnusualAllowanceCfg>();
                Cat_UnusualAllowanceCfg PaidLeave = null;
                Cat_UnusualAllowanceCfg GoodHealth = null;
                ElementFormula FormulaValue = new ElementFormula();
                if (AllowanceEvaluationYear)
                {
                    ListUnusualAllowanceCfg = repoCat_UnusualAllowanceCfg.FindBy(m => m.IsDelete != true).ToList();
                    PaidLeave = ListUnusualAllowanceCfg.FirstOrDefault(m => m.Code == "PaidLeave");
                    GoodHealth = ListUnusualAllowanceCfg.FirstOrDefault(m => m.Code == "GoodHealth");
                }
                #endregion

                foreach (var profile in ListProfile)
                {
                    if (AllowanceEvaluationYear)
                    {
                        #region Tính Công Thức
                        try
                        {
                            listElementFormula = new List<ElementFormula>();
                            TotalData.listElement_All = repoCat_Element.FindBy(m => m.IsDelete != true).ToList().Translate<Cat_ElementEntity>();
                            listElementFormula = CumputePayrollServices.ParseFormula(ElementItem, listElementFormula, TotalData, profile, CutOffDuration, new Dictionary<Guid, ValueCount>());
                        }
                        catch
                        {
                            listElementFormula = null;
                            continue;
                        }
                        #endregion

                        if (PaidLeave != null)
                        {
                            //lưu phần tử TienBuPhepNam
                            Sal_UnusualAllowance UnusualAllowance = new Sal_UnusualAllowance();
                            UnusualAllowance.ID = Guid.NewGuid();
                            UnusualAllowance.ProfileID = profile.ID;
                            UnusualAllowance.UnusualEDTypeID = PaidLeave.ID;

                            UnusualAllowance.MonthStart = CutOffDuration.DateStart;
                            UnusualAllowance.MonthEnd = CutOffDuration.DateEnd;
                            UnusualAllowance.Type = EnumDropDown.EDType.E_EARNING.ToString();
                            UnusualAllowance.CurrencyID = model.CurrencyID;
                            FormulaValue = listElementFormula.Where(m => m.VariableName.ReplaceSpace() == "TienPhepNam").FirstOrDefault();
                            UnusualAllowance.Amount = double.Parse(FormulaValue != null ? FormulaValue.Value.ToString() : "0");
                            repoUnusualAllowance.Add(UnusualAllowance);
                        }

                        if (GoodHealth != null)
                        {
                            //lưu phần tử TienSucKhoe
                            Sal_UnusualAllowance UnusualAllowance = new Sal_UnusualAllowance();
                            UnusualAllowance = new Sal_UnusualAllowance();
                            UnusualAllowance.ID = Guid.NewGuid();
                            UnusualAllowance.ProfileID = profile.ID;
                            UnusualAllowance.UnusualEDTypeID = GoodHealth.ID;

                            UnusualAllowance.MonthStart = CutOffDuration.DateStart;
                            UnusualAllowance.MonthEnd = CutOffDuration.DateEnd;
                            UnusualAllowance.Type = EnumDropDown.EDType.E_EARNING.ToString();
                            UnusualAllowance.CurrencyID = model.CurrencyID;
                            FormulaValue = listElementFormula.Where(m => m.VariableName.ReplaceSpace() == "TienSucKhoe").FirstOrDefault();
                            UnusualAllowance.Amount = double.Parse(FormulaValue != null ? FormulaValue.Value.ToString() : "0");
                            repoUnusualAllowance.Add(UnusualAllowance);
                        }
                    }
                    else
                    {
                        Sal_UnusualAllowance UnusualAllowance = new Sal_UnusualAllowance();
                        UnusualAllowance.ID = Guid.NewGuid();
                        UnusualAllowance.ProfileID = profile.ID;
                        UnusualAllowance.UnusualEDTypeID = model.UnusualEDTypeID;

                        UnusualAllowance.MonthStart = CutOffDuration.DateStart;
                        UnusualAllowance.MonthEnd = CutOffDuration.DateEnd;
                        UnusualAllowance.Type = EnumDropDown.EDType.E_EARNING.ToString();
                        UnusualAllowance.CurrencyID = model.CurrencyID;
                        try
                        {
                            listElementFormula = new List<ElementFormula>();
                            TotalData.listElement_All = repoCat_Element.FindBy(m => m.IsDelete != true).ToList().Translate<Cat_ElementEntity>();
                            listElementFormula = CumputePayrollServices.ParseFormula(ElementItem, listElementFormula, TotalData, profile, CutOffDuration, new Dictionary<Guid, ValueCount>());
                            FormulaValue = listElementFormula.Where(m => m.VariableName.ReplaceSpace() == ElementItem.ElementCode.ReplaceSpace()).FirstOrDefault();
                            UnusualAllowance.Amount = double.Parse(FormulaValue != null ? FormulaValue.Value.ToString() : "0");
                        }
                        catch
                        {
                            UnusualAllowance.Amount = 0;
                            UnusualAllowance.Notes = "Không tính được công thức - Lỗi: " + TotalData.Status;
                        }
                        repoUnusualAllowance.Add(UnusualAllowance);
                    }

                    if (ListProfile.IndexOf(profile) % 10 == 0)
                    {
                        asynTask.PercentComplete = (double)(ListProfile.IndexOf(profile) + 1) / (double)ListProfile.Count;
                        unitOfWork.SaveChanges();
                    }
                }
                asynTask.PercentComplete = 1;
                unitOfWork.SaveChanges();
            }
        }

        public List<Sal_UnusualAllowanceEntity> GetUnusualAllowanceByUnusualEDTypeIDAndDateOccur(Guid? ProfileID, Guid? UnusualEDTypeID, DateTime? _datestart, DateTime? _dateend)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var lstUnusualAllowance = unitOfWork.CreateQueryable<Sal_UnusualAllowance>(Guid.Empty, s => s.ProfileID == ProfileID && s.UnusualEDTypeID == UnusualEDTypeID && s.DateOccur != null && s.DateOccur >= _datestart && s.DateOccur <= _dateend).Select(s => new { s.ProfileID, s.DateOccur }).ToList();
                return lstUnusualAllowance.Translate<Sal_UnusualAllowanceEntity>();
            }

        }

        public ResultsObject UpdateSatus(List<Guid> listIds, string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoUnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);

                List<Sal_UnusualAllowance> listResult = repoUnusualAllowance.FindBy(m => m.IsDelete != true).ToList();
                listResult = listResult.Where(m => listIds.Contains(m.ID)).ToList();
                listResult.ForEach(m => m.Status = status);
                unitOfWork.SaveChanges();
                return new ResultsObject();
            }
        }
    }
}
