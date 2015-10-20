using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;

using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_LateEarlyRuleRepository : CustomBaseRepository<Cat_LateEarlyRule>
    {
        public Cat_LateEarlyRuleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
