using Devart.Data.Oracle;
//using HRM.Business.Category.Models;
using HRM.Data.BaseRepository;
//using HRM.Data.Category.Data;
//using HRM.Data.Category.Model;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_OrgStructureRepository : CustomBaseRepository<Cat_OrgStructure>
    {
        public Cat_OrgStructureRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

