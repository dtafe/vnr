using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_GradeAttendanceRepository : CustomBaseRepository<Cat_GradeAttendance>
    {
        public Cat_GradeAttendanceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
