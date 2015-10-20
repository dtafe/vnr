using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
//using HRM.Data.Hr.Data;
//using HRM.Data.Hr.Model;
//using HRM.Data.Category.Model;
//using HRM.Data.Category.Data;
//using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_JobTitleRepository : CustomBaseRepository<Cat_JobTitle>
    {
        public Cat_JobTitleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
