using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_RecruitmentHistoryRepository : CustomBaseRepository<Rec_RecruitmentHistory>
    {
        public Rec_RecruitmentHistoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
