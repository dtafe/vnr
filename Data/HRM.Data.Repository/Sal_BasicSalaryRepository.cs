using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Sal_BasicSalaryRepository : CustomBaseRepository<Sal_BasicSalary>
    {
        public Sal_BasicSalaryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
