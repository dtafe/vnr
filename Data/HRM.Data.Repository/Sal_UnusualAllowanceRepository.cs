using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Sal_UnusualAllowanceRepository : CustomBaseRepository<Sal_UnusualAllowance>
    {
        public Sal_UnusualAllowanceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
    }
}
