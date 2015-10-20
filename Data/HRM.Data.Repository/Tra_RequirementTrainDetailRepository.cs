using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{

    public class Tra_RequirementTrainDetailRepository : CustomBaseRepository<Tra_RequirementTrainDetail>
    {
        public Tra_RequirementTrainDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
