using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Hre_CandidateHistoryRepository : CustomBaseRepository<Hre_CandidateHistory>
    {
        public Hre_CandidateHistoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
