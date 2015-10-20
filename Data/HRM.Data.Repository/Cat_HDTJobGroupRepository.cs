using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_HDTJobGroupRepository : CustomBaseRepository<Cat_HDTJobGroup>
    {
        public Cat_HDTJobGroupRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
    }
}
