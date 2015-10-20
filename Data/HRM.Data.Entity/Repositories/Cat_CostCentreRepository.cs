using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_CostCentreRepository : CustomBaseRepository<Cat_CostCentre>
    {
        public Cat_CostCentreRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
