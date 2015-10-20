using System.Data.SqlClient;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Hre_HDTJobRepository : CustomBaseRepository<Hre_HDTJob>
    {
        public Hre_HDTJobRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
