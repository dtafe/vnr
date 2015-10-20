
using HRM.Data.BaseRepository;

using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_DistrictRepository : CustomBaseRepository<Cat_District>
    {
        public Cat_DistrictRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
        

    }
}
