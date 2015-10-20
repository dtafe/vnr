using System.Linq;
using HRM.Data.BaseRepository;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_EthnicGroupRepository : CustomBaseRepository<Cat_EthnicGroup>
    {
        public Cat_EthnicGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
