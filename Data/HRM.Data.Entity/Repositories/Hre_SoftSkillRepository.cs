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
    public class Hre_SoftSkillRepository : CustomBaseRepository<Hre_SoftSkill>
    {
        public Hre_SoftSkillRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
