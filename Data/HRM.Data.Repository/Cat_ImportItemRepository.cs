using System.Data.SqlClient;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ImportItemRepository : CustomBaseRepository<Cat_ImportItem>
    {
        public Cat_ImportItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
