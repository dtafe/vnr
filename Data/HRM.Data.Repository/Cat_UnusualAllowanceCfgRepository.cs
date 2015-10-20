using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_UnusualAllowanceCfgRepository : CustomBaseRepository<Cat_UnusualAllowanceCfg>
    {
        public Cat_UnusualAllowanceCfgRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
    }
}
