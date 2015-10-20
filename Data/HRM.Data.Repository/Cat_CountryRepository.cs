using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_CountryRepository : CustomBaseRepository<Cat_Country>
    {
        public Cat_CountryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
