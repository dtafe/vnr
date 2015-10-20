using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_RecruitmentCampaignRepository : CustomBaseRepository<Rec_RecruitmentCampaign>
    {
        public Rec_RecruitmentCampaignRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
