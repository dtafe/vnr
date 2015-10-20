using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Data.Main.Data.Sql;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;
namespace HRM.Business.Category.Domain
{
    public class Cat_GradeAttendanceServices: BaseService
    {
        public Guid GetGradeAttendanceByRank(string Rank)
        {
            Guid rs = Guid.Empty;
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_GradeAttendanceRepository(unitOfWork);
                var lstGradeAttendance = repo.FindBy(x => x.Code == Rank&&x.IsDelete==null).ToList();
                if (lstGradeAttendance != null && lstGradeAttendance.Count != 0)
                {
                    rs = lstGradeAttendance.FirstOrDefault().ID;
                }


            }
            return rs;
        }
    }
}
