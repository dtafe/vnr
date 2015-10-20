using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class Hre_ProfilePartyUnionRepository : CustomBaseRepository<Hre_ProfilePartyUnion>
    {
        public Hre_ProfilePartyUnionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
