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
    public class Cat_EmployeeTypeRepository : CustomBaseRepository<Cat_EmployeeType>
    {
        public Cat_EmployeeTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        
    }
}
