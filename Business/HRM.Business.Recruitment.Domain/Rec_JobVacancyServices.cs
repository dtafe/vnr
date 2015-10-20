using System;
using System.Linq.Dynamic;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Data.Entity.Repositories;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_JobVacancyServices : BaseService
    {
        public string AddConditionIntoVacancy(Guid JobVacancyID, string ConditionIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_JobVacancyRepository(unitOfWork);
                Rec_JobVacancy JobVacancy = repo.GetById(JobVacancyID);
                if (JobVacancy == null)
                    return null;
                List<Guid> lstCondition = new List<Guid>();
                List<Guid> lstAddCondition = new List<Guid>();
                lstAddCondition = ConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                if (!string.IsNullOrEmpty(JobVacancy.JobConditionIDs))
                {
                    lstCondition=JobVacancy.JobConditionIDs.Split(',').Select(x=>Guid.Parse(x)).ToList();
                }
                lstCondition.AddRange(lstAddCondition);
                lstCondition = lstCondition.Distinct().ToList();
                JobVacancy.JobConditionIDs = string.Join(",", lstCondition);
                repo.SaveChanges();
                return JobVacancy.JobConditionIDs;
            }
            
        }
        public string GetJobConditionIDs(Guid JobVacancyID)
        {
            string Rs = null;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_JobVacancyRepository(unitOfWork);
                Rec_JobVacancy JobVacancy = repo.GetById(JobVacancyID);
                if (JobVacancy == null)
                    return null;
                Rs = JobVacancy.JobConditionIDs;
            }
            return Rs;
 
        }
        public string AddJobCavancy(Guid JobVacancyID, Guid ConditionID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_JobVacancyRepository(unitOfWork);
                Rec_JobVacancy JobVacancy = repo.GetById(JobVacancyID);
                if (JobVacancy == null)
                    return null;
                List<Guid> lstCondition = new List<Guid>();

                if (!string.IsNullOrEmpty(JobVacancy.JobConditionIDs))
                {
                    var arr = JobVacancy.JobConditionIDs.Split(',');

                    for (int i = 0; i < arr.Length; i++)
                    {
                        try
                        {
                            lstCondition.Add(Guid.Parse(arr[i].ToString()));
                        }
                        catch
                        {
                        }

                    }
                }
                if (lstCondition.Contains(ConditionID))
                    return null;
                else
                {
                    lstCondition.Add(ConditionID);
                    JobVacancy.JobConditionIDs = string.Join(",", lstCondition);
                   // JobVacancy.JobConditionIDs.Substring(0, JobVacancy.JobConditionIDs.LastIndexOf(','));
                    repo.SaveChanges();
                    return JobVacancy.JobConditionIDs;
 
                }




            }
            return null;
        }
        public string DeleteJobCavancy(Guid JobVacancyID, string ConditionIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_JobVacancyRepository(unitOfWork);
                var ilConditionIDs = ConditionIDs.Split(',');
                List<Guid> lsConditionIDs = new List<Guid>();
                lsConditionIDs = ConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                
                Rec_JobVacancy JobVacancy = repo.GetById(JobVacancyID);
                if (JobVacancy == null)
                    return null;
                List<Guid> lstCondition = new List<Guid>();
                lstCondition = JobVacancy.JobConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                if (lstCondition != null && lstCondition.Count != 0)
                {
                    lstCondition = lstCondition.Where(x => !lsConditionIDs.Contains(x)).ToList();
                    JobVacancy.JobConditionIDs = string.Join(",", lstCondition);
                    repo.SaveChanges();
                    return JobVacancy.JobConditionIDs;
                }
               

            }
            return null;
        }
    }
}
