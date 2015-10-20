using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_JobConditionRepository : CustomBaseRepository<Rec_JobCondition>
    {
        public Rec_JobConditionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
