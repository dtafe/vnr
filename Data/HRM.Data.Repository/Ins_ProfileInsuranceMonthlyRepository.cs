using System.Linq;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace HRM.Data.Entity.Repositories
{

    public class Ins_ProfileInsuranceMonthlyRepository : GenericRepository<Ins_ProfileInsuranceMonthly>
    {
        public Ins_ProfileInsuranceMonthlyRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

     
    }
}
