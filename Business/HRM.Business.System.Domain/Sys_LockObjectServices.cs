using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_LockObjectServices : BaseService
    {

        public void Approved(List<Guid> selectedIds, string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMessage = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLockObject = new Sys_LockObjectRepository(unitOfWork);
                var lstLockObject = repoLockObject.FindBy(s => selectedIds.Contains(s.ID)).ToList();

                foreach (var lockObject in lstLockObject)
                {
                    lockObject.Status = status;
                }
                repoLockObject.SaveChanges();

            }
        }
    }
}
