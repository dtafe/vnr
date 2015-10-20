using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_CandidateRepository : CustomBaseRepository<Rec_Candidate>
    {
        public Rec_CandidateRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
