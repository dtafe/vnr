using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceEvaRepository : CustomBaseRepository<Eva_PerformanceEva>
    {
        public Eva_PerformanceEvaRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
