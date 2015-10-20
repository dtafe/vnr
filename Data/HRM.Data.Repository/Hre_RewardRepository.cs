using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Hre_RewardRepository : CustomBaseRepository<Hre_Reward>
    {
        public Hre_RewardRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
