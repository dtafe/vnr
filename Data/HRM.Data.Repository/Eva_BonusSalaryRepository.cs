using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_BonusSalaryRepository : CustomBaseRepository<Eva_BonusSalary>
    {
        public Eva_BonusSalaryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
