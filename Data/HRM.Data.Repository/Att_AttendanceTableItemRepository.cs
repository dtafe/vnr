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
using HRM.Data.Entity;


namespace HRM.Data.Attendance.Data.Sql.Repositories
{
    public class Att_AttendanceTableItemRepository : CustomBaseRepository<Att_AttendanceTableItem>
    {
        public Att_AttendanceTableItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        
    }
}
