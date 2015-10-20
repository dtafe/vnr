using System.Data.SqlClient;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ImportRepository : CustomBaseRepository<Cat_Import>
    {
        public Cat_ImportRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
