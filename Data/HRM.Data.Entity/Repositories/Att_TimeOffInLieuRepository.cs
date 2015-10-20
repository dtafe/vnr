using System.Linq;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Att_TimeOffInLieuRepository : CustomBaseRepository<Att_TimeOffInLieu>
    {
        public Att_TimeOffInLieuRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
