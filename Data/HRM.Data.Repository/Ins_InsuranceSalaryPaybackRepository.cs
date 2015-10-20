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

    public class Ins_InsuranceSalaryPaybackRepository : GenericRepository<Ins_InsuranceSalaryPayback>
    {
        public Ins_InsuranceSalaryPaybackRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

     
    }
}
