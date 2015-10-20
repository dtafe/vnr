using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_EvaluatorRepository : CustomBaseRepository<Eva_Evaluator>
    {
        public Eva_EvaluatorRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
