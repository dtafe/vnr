using HRM.Business.Hr.Models;
using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Models;
using System.Data;
using System.Collections;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;

namespace HRM.Business.Hr.Domain
{
    public class Hre_StopWorkingServices : BaseService
    {
        public string ActionApproved(string selectedIds, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_StopWorkingRepository(unitOfWork);
                var profileServices = new Hre_ProfileServices();
                string status = string.Empty;
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstStopWorkings = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var StopWorking in lstStopWorkings)
                {
                    if (StopWorking.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_STOP.ToString())
                    {
                        var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(StopWorking.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                        StopWorking.LastStatusSyn = profile.StatusSyn;
                        profile.StatusSyn = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_STOP.ToString();
                        StopWorking.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                        profile.TypeSuspense = StopWorking.TypeSuspense;
                        profile.RequestDate = StopWorking.RequestDate;
                        profile.StopWorkType = StopWorking.StopWorkType;
                        profile.DateQuit = StopWorking.DateStop;
                        profile.ResReasonID = StopWorking.ResignReasonID;
                        profile.IsHoldSal = StopWorking.IsHoldSal;
                        profile.TypeOfStop = StopWorking.TypeOfStop;
                        profile.ResignNo = StopWorking.DecisionNo;
                        profileServices.Edit(profile);
                    }
                    else if (StopWorking.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString())
                    {
                        var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(StopWorking.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                        profile.StatusSyn = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString();
                        profile.TypeOfStop = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString();
                        StopWorking.LastStatusSyn = profile.StatusSyn;
                        StopWorking.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                        profile.TypeSuspense = StopWorking.TypeSuspense;
                        profile.RequestDate = StopWorking.RequestDate;
                        profile.StopWorkType = StopWorking.StopWorkType;
                        profile.DateQuit = StopWorking.DateStop;
                        profile.ResReasonID = StopWorking.ResignReasonID;
                        profile.ResignNo = StopWorking.DecisionNo;
                        profile.IsHoldSal = null;
                        profileServices.Edit(profile);
                    }
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string ActionCancel(string selectedIds, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_StopWorkingRepository(unitOfWork);
                var profileServices = new Hre_ProfileServices();
                string status = string.Empty;
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstStopWorkings = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var StopWorking in lstStopWorkings)
                {
                    var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(StopWorking.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                    profile.StatusSyn = StopWorking.LastStatusSyn;
                    StopWorking.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_CANCEL.ToString();
                    profile.StopWorkType = null;
                    profile.TypeSuspense = null;
                    profile.RequestDate = null;
                    profile.DateQuit = null;
                    profile.ResReasonID = null;
                    profile.IsHoldSal = false;
                    profile.ResignNo = null;
                    profile.TypeOfStop = null;
                    profileServices.Edit(profile);
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string ActionApprovedSuspense(string selectedIds, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_StopWorkingRepository(unitOfWork);
                var profileServices = new Hre_ProfileServices();
                string status = string.Empty;
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstSuspenses = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var Suspense in lstSuspenses)
                {

                    var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Suspense.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                    Suspense.LastStatusSyn = profile.StatusSyn;
                    profile.StatusSyn = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString();
                    profile.TypeOfStop = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString();
                    Suspense.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    profile.RequestDate = Suspense.RequestDate;
                    profile.DateQuit = Suspense.DateStop;
                    profile.IsHoldSal = Suspense.IsHoldSal;
                    profile.TypeSuspense = Suspense.TypeSuspense;
                    profile.StopWorkType = Suspense.StopWorkType;
                    profile.ResReasonID = Suspense.ResignReasonID;
                    profile.ResignNo = Suspense.DecisionNo;
                    profileServices.Edit(profile);

                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string ActionApprovedComeBack(string selectedIds, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_StopWorkingRepository(unitOfWork);
                var profileServices = new Hre_ProfileServices();
                var contractSevices = new Hre_ContractServices();
                var ContractExtendServices = new Hre_ContractExtendServices();
                var contractTypeSevices = new Cat_ContractTypeServices();

                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                //   var lstSuspenses = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                var services = new BaseService();
                var lstObj = new List<object>();
                lstObj.AddRange(new object[14]);
                lstObj[12] = 1;
                lstObj[13] = int.MaxValue - 1;
                var lstSuspenses = services.GetData<Hre_StopWorkingEntity>(lstObj, ConstantSql.hrm_hr_sp_get_RegisterComback, userLogin, ref status).ToList().Translate<Hre_StopWorking>();
                lstSuspenses = lstSuspenses.Where(s => lstIds.Contains(s.ID)).ToList();
                var lstSuspense = new List<Hre_StopWorkingEntity>();
                foreach (var Suspense in lstSuspenses)
                {
                    var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Suspense.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                    Suspense.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    Suspense.StatusComeBack = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    profile.StatusSyn = Suspense.LastStatusSyn;
                    profile.RequestDate = null;
                    profile.DateQuit = null;
                    profile.IsHoldSal = false;
                    profile.TypeSuspense = null;
                    profile.StopWorkType = null;
                    profile.ResReasonID = null;
                    profile.TypeOfStop = null;
                    profile.ResignNo = null;
                    profileServices.Edit(profile);
                    var contract = contractSevices.GetData<Hre_ContractEntity>(Common.DotNetToOracle(Suspense.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status).OrderByDescending(s => s.DateStart).FirstOrDefault();

                    if (contract != null)
                    {
                        var contractType = contractSevices.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(contract.ContractTypeID.ToString()), ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status).FirstOrDefault();
                        if (Suspense.TypeSuspense == HRM.Infrastructure.Utilities.EnumDropDown.TypeSuspense.E_MILITARY.ToString())
                        {
                            int month = 0;
                            if (contractType == null)
                            {
                                continue;
                            }

                            if (contractType.ValueTime != null)
                            {
                                month = (int)contractType.ValueTime.Value;
                                if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                                {
                                    month = month * 12;
                                }
                                contract.DayContract = month;
                                if (contract.DateEnd != null)
                                {
                                    if (Suspense.DateComeBack <= contract.DateEnd.Value)
                                    {
                                        if (Suspense.DateStop != null)
                                        {
                                            double daySus = ((Suspense.DateComeBack.Value.Subtract(Suspense.DateStop.Value)).TotalDays / 30);
                                            contract.DayExtend = (int)Math.Floor(daySus);
                                            if (contract.DateExtend == null)
                                            {
                                                contract.DateExtend = contract.DateEnd.Value.AddMonths(contract.DayExtend.Value);
                                            }
                                            else
                                            {
                                                contract.DateExtend = contract.DateExtend.Value.AddMonths(contract.DayExtend.Value);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Suspense.DateStop != null)
                                        {
                                            double daySus = (contract.DateEnd.Value.Subtract(Suspense.DateStop.Value)).TotalDays / 30;
                                            contract.DayExtend = (int)Math.Floor(daySus);
                                            if (contract.DateExtend == null)
                                            {
                                                contract.DateExtend = contract.DateEnd.Value.AddMonths(contract.DayExtend.Value);
                                            }
                                            else
                                            {
                                                contract.DateExtend = contract.DateExtend.Value.AddMonths(contract.DayExtend.Value);
                                            }
                                            Suspense.DateComeBack = Suspense.DateComeBack.Value.AddMonths(contract.DayExtend.Value);
                                        }
                                    }
                                }
                            }
                            contract.StatusEvaluation = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                            Hre_ContractExtendEntity contractExtend = new Hre_ContractExtendEntity();
                            contractExtend.ContractID = contract.ID;
                            contractExtend.DateStart = Suspense.DateComeBack;
                            contractExtend.DateEnd = contract.DateExtend;
                            contractSevices.Edit(contract);
                            ContractExtendServices.Add(contractExtend);
                        }
                    }
                    repo.Edit(Suspense);
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string UpdateWorkingPosition(string selectedIds, string userLogin)
        {
            string message = string.Empty;
            string status = string.Empty;
            var profileServices = new Hre_ProfileServices();
            var workhistoryServices = new Hre_WorkHistoryServices();
            List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            //   var lstSuspenses = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
            var services = new BaseService();
            var lstObj = new List<object>();
            lstObj.AddRange(new object[14]);
            lstObj[12] = 1;
            lstObj[13] = int.MaxValue - 1;
            var lstComback = services.GetData<Hre_StopWorkingEntity>(lstObj, ConstantSql.hrm_hr_sp_get_RegisterComback, userLogin, ref status).ToList().Translate<Hre_StopWorking>();
            lstComback = lstComback.Where(s => lstIds.Contains(s.ID)).ToList();
            Hre_WorkHistoryEntity workHistoryEntity;
            foreach (var item in lstComback)
            {
                var profile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(item.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                profile.DateOfEffect = item.DateComeBack;
                string supervisor = null;
                if (profile.SupervisorID != null)
                {
                    var supervisorbyProfile = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(profile.SupervisorID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                    if (supervisorbyProfile != null)
                    {
                        supervisor = supervisorbyProfile.ProfileName;
                    }
                }
                workHistoryEntity = new Hre_WorkHistoryEntity();
                workHistoryEntity.ProfileID = item.ProfileID.Value;
                workHistoryEntity.DateEffective = item.DateComeBack != null ? item.DateComeBack.Value : DateTime.Now;
                workHistoryEntity.OrganizationStructureID = profile.OrgStructureID;
                workHistoryEntity.JobTitleID = profile.JobTitleID;
                workHistoryEntity.PositionID = profile.PositionID;
                workHistoryEntity.Supervisor = supervisor;
                profileServices.Edit(profile);
                workhistoryServices.Add(workHistoryEntity);
            }
            message = NotificationType.Success.ToString();
            return message;
        }

        public string ActionCancelComback(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_StopWorkingRepository(unitOfWork);
                var profileServices = new Hre_ProfileServices();
                string status = string.Empty;
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstSuspenses = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var Suspense in lstSuspenses)
                {
                    Suspense.DateComeBack = null;
                    Suspense.RequestDateComeBack = null;
                    Suspense.Status = HRM.Infrastructure.Utilities.EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
