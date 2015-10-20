using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Infrastructure.Utilities;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;

namespace HRM.Business.Attendance.Domain
{
    public class Att_CutOffDurationServices : BaseService
    {
        public Guid GetCutOffDurationDefault()
        {
            var guid = Guid.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_CutOffDurationRepository(unitOfWork);
                guid = repo.FindBy(m => (!m.IsDelete.HasValue || m.IsDelete != true) && m.MonthYear.Month == DateTime.Now.Month && m.MonthYear.Year == DateTime.Now.Year).Select(d=>d.ID).FirstOrDefault();
            }

            return guid;
        }
    }
}
