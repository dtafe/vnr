using System;
using System.Linq;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using System.Collections.Generic;
namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_GroupServices : BaseService
    {
        public List<Sys_GroupEntity> Get()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_GroupRepository(unitOfWork);
                var groups = repo.FindBy(i => i.IsDelete == null).ToList();
                List<Sys_GroupEntity> result = new List<Sys_GroupEntity>();
                if (groups != null && groups.Any())
                {
                    result = groups.Translate<Sys_GroupEntity>();
                }
                return result;
            }
        }

        public Sys_GroupEntity GetSysGroupById(Guid id)
        {
            string status = string.Empty;
            var result = GetById<Sys_Group>(id, ref status);
            Sys_GroupEntity sysGroupEntity = null;
            if (result != null)
            {
                sysGroupEntity = result.CopyData<Sys_GroupEntity>();
            }
            return sysGroupEntity;
        }

    }
}
