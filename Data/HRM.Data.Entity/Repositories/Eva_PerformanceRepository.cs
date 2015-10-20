using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceRepository : CustomBaseRepository< Eva_Performance>
    {
        public Eva_PerformanceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
