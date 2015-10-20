using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Infrastructure.Utilities.Helper;
using System.Threading;
using HRM.Business.Insurance.Models;


namespace HRM.Business.Payroll.Domain
{
    public class Sal_GradeServices : BaseService
    {
        public void AddDataForGrade(string ProfileIDs, Guid GradePayrollID, DateTime DateHire)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoGrade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                List<Guid> lstProfileIDs = ProfileIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                List<Sal_Grade> lstGrade = new List<Sal_Grade>();
                foreach (var item in lstProfileIDs)
                {
                    Sal_Grade Grade = new Sal_Grade();
                    Grade.ProfileID = item;
                    Grade.GradePayrollID = GradePayrollID;
                    Grade.MonthStart = DateHire;
                    lstGrade.Add(Grade);
                }
                repoGrade.Add(lstGrade);
                repoGrade.SaveChanges();
            }
        }
       
    }
}
