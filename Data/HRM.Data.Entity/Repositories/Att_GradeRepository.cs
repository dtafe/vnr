using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Att_GradeRepository : CustomBaseRepository<Att_Grade>
    {
        public Att_GradeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        
        
    }
}
