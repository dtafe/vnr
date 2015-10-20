using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_GradePayrollRepository : CustomBaseRepository<Cat_GradePayroll>
    {
        public Cat_GradePayrollRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
