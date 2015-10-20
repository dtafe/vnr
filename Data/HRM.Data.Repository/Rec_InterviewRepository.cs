using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_InterviewRepository : CustomBaseRepository<Rec_Interview>
    {
        public Rec_InterviewRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
