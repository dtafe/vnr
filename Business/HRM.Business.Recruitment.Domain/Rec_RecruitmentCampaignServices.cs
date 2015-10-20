using System;
using System.Linq.Dynamic;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;


namespace HRM.Business.Recruitment.Domain
{
    public class Rec_RecruitmentCampaignServices : BaseService
    {
        public bool UpdateRecruitmentCampaignStatus(string selectedIds, string Status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_RecruitmentCampaignRepository(unitOfWork);
                var lstIds = selectedIds.Split(',');
                List<Guid> listID = new List<Guid>();
                for (int i = 0; i < lstIds.Length; i++)
                {
                    try
                    {
                        listID.Add(Guid.Parse(lstIds[i].ToString()));
                    }
                    catch 
                    { }
                }
                var listRecruitmentCampaign = repo.FindBy(x => listID.Contains(x.ID)).ToList();
                foreach (var item in listRecruitmentCampaign)
                {
                    item.Status = Status;
                }
                repo.SaveChanges();

            }
            return true;
        }
        public bool UpdateRecruitmentCampaignActive(string selectedIds, bool Value)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_RecruitmentCampaignRepository(unitOfWork);
                var lstIds = selectedIds.Split(',');
                List<Guid> listID = new List<Guid>();
                for (int i = 0; i < lstIds.Length; i++)
                {
                    try
                    {
                        listID.Add(Guid.Parse(lstIds[i].ToString()));
                    }
                    catch
                    { }
                }
                var listRecruitmentCampaign = repo.FindBy(x => listID.Contains(x.ID)).ToList();
                foreach (var item in listRecruitmentCampaign)
                {
                    item.IsActivate = Value;
                }
                repo.SaveChanges();

            }
            return true;
        }
    }
}
