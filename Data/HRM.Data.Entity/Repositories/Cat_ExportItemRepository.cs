using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ExportItemRepository : CustomBaseRepository<Cat_ExportItem>
    {
        public Cat_ExportItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
