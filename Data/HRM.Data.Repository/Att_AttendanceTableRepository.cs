using System;
using System.Collections.Generic;
using System.Linq;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
//using HRM.Data.Attendance.Model;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Attendance.Data.Sql.Repositories
{
    public class Att_AttendanceTableRepository : CustomBaseRepository<Att_AttendanceTable>
    {
        public Att_AttendanceTableRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
