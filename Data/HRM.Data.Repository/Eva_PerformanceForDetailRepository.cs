using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceForDetailRepository : CustomBaseRepository<Eva_PerformanceForDetail>
    {
        public Eva_PerformanceForDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
