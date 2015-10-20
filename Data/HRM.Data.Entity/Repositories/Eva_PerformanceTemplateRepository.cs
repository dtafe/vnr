using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_PerformanceTemplateRepository : CustomBaseRepository<Eva_PerformanceTemplate>
    {
        public Eva_PerformanceTemplateRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
