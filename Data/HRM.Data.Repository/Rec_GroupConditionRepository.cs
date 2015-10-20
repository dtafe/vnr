using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_GroupConditionRepository : CustomBaseRepository<Rec_GroupCondition>
    {
        public Rec_GroupConditionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
