using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_InterviewCampaignDetailRepository : CustomBaseRepository<Rec_InterviewCampaignDetail>
    {
        public Rec_InterviewCampaignDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
