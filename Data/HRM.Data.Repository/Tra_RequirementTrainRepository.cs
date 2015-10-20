using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{

    public class Tra_RequirementTrainRepository : CustomBaseRepository<Tra_RequirementTrain>
    {
        public Tra_RequirementTrainRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
