using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Sal_InsuranceSalaryRepository : CustomBaseRepository<Sal_InsuranceSalary>
    {
        public Sal_InsuranceSalaryRepository(IUnitOfWork unitOfWork): base(unitOfWork){}

    }
}
