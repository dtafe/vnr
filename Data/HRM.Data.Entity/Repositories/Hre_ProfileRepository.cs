using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Hre_ProfileRepository : CustomBaseRepository<Hre_Profile>
    {
        public Hre_ProfileRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
