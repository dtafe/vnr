using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using System.Linq;
using System;

namespace HRM.Business.Evaluation.Domain
{
    public class Eva_PerformanceTemplateServices : BaseService
    {
        public Guid GetJobTitleOfProfile(Guid profileId)
        {
            using (var context = new VnrHrmDataContext())
            {              
                var status = string.Empty;
                var jobTitleID = Guid.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                jobTitleID = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => m.ID == profileId && m.JobTitleID != null).Select(m => m.JobTitleID.Value).FirstOrDefault();
                return jobTitleID;
            }
        }

    }
}
