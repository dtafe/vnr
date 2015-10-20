using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ExportRepository : CustomBaseRepository<Cat_Export>
    {
        public Cat_ExportRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
