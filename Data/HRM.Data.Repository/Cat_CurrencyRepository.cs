using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using System.Linq;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_CurrencyRepository : CustomBaseRepository<Cat_Currency>
    {
        public Cat_CurrencyRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
