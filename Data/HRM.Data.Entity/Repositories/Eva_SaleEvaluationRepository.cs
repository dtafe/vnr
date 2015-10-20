using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_SaleEvaluationRepository : CustomBaseRepository<Eva_SaleEvaluation>
    {
        public Eva_SaleEvaluationRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
