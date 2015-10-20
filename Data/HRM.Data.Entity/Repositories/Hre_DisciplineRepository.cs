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
    public class Hre_DisciplineRepository : CustomBaseRepository<Hre_Discipline>
    {
        public Hre_DisciplineRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
