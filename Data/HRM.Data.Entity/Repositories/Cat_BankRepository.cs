using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_BankRepository : CustomBaseRepository<Cat_Bank>
    {
        public Cat_BankRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
