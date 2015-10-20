using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_KPIRepository : CustomBaseRepository<Eva_KPI>
    {
        public Eva_KPIRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
