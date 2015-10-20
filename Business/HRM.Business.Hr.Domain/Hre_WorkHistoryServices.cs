using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;
using HRM.Data.Entity.Repositories;
using System.Collections;
using VnResource.Helper.Linq;
using VnResource.Helper.Data;

namespace HRM.Business.Hr.Domain
{
    public class Hre_WorkHistoryServices : BaseService
    {
        public string ActionApproved(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                //var repo = new Hre_WorkHistoryRepository(unitOfWork);
                var repoWorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstWorkHistorys = repoWorkHistory.FindBy(m => lstIds.Contains(m.ID)).ToList();
                foreach (var WorkHistory in lstWorkHistorys)
                {
                    WorkHistory.Status = WorkHistoryStatus.E_APPROVED.ToString();

                    if (WorkHistory.DateEffective != null && WorkHistory.DateEffective <= DateTime.Now.Date)
                    {
                        string status = string.Empty;
                        Hre_Profile profile = repoProfile.FindBy(m => m.IsDelete != true && m.ID == WorkHistory.ProfileID).FirstOrDefault();
                        if (profile != null)
                        {
                            profile.OrgStructureID = WorkHistory.OrganizationStructureID;
                            profile.JobTitleID = WorkHistory.JobTitleID;
                            profile.PositionID = WorkHistory.PositionID;
                            profile.DateOfEffect = WorkHistory.DateEffective;
                            profile.LaborType = WorkHistory.LaborType;
                            profile.CostCentreID = WorkHistory.CostCentreID;
                            profile.FormType = WorkHistory.FormType;
                            profile.EmpTypeID = WorkHistory.EmployeeTypeID;
                            profile.WorkingPlace = WorkHistory.WorkLocation;
                            //profileServices.Edit(profile);
                        }
                    }
                }
                unitOfWork.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string ActionCancel(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_WorkHistoryRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstWorkHistorys = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var WorkHistory in lstWorkHistorys)
                {
                    WorkHistory.Status = WorkHistoryStatus.E_REJECT.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string ActionWaitApprove(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_WorkHistoryRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstWorkHistorys = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var WorkHistory in lstWorkHistorys)
                {
                    WorkHistory.Status = WorkHistoryStatus.E_WAITAPPROVE.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
