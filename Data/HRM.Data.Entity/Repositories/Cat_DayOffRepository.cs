using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_DayOffRepository : CustomBaseRepository<Cat_DayOff>
    {
        public Cat_DayOffRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM8.20/7/2017
        
        public List<Cat_DayOff> Get()
        {
            var result = this.DbSet.ToList();
            return result;
        }
        #endregion
    }
}
