using System.Linq;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using VnResource.Helper.Data;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_GroupPermission2Repository : CustomBaseRepository<Sys_GroupPermission2>
    {
        public Sys_GroupPermission2Repository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
