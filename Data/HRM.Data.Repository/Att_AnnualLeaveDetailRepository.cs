using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Att_AnnualLeaveDetailRepository : CustomBaseRepository<Att_AnnualLeaveDetail>
    {
        public Att_AnnualLeaveDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool CalculateAnnualLeaveDetails(int year)
        {
            return false;
        }


    }
}
