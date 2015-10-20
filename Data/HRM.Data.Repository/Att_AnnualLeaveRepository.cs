using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Business.Attendance.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Att_AnnualLeaveRepository : CustomBaseRepository<Att_AnnualLeave>
    {
        public Att_AnnualLeaveRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        


    }
}
