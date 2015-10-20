using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_UnAllowCfgAmountRepository : CustomBaseRepository<Cat_UnAllowCfgAmount>
    {
        public Cat_UnAllowCfgAmountRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
    }
}
