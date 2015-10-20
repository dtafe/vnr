using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_CandidateHistoryRepository : CustomBaseRepository<Rec_CandidateHistory>
    {
        public Rec_CandidateHistoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
