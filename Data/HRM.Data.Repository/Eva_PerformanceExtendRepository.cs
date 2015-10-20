using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceExtendRepository : CustomBaseRepository<Eva_PerformanceExtend>
    {
        public Eva_PerformanceExtendRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
