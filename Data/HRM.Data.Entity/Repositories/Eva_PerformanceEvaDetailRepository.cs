using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceEvaDetailRepository : CustomBaseRepository<Eva_PerformanceEvaDetail>
    {
        public Eva_PerformanceEvaDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
