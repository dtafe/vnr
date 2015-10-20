using System;
using System.Collections.Generic;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using System.Linq;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_ResourceServices : BaseService
    {

        public List<Sys_ResourceEntity> GetAllResources()
        {
            string status = string.Empty;
            var resources = GetAllUseEntity<Sys_Resource>(ref status).ToList();
            var resourceEntities = new List<Sys_ResourceEntity>();
            if (resources != null || resources.Any())
            {
                resourceEntities = resources.Translate<Sys_ResourceEntity>();
            }
            return resourceEntities;
        }

        public bool UpdateResource(List<Sys_ResourceEntity> lstResource)
        {
            using (var context = new VnrHrmDataContext())
            {
                var isExistedUser = false;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_ResourceRepository(unitOfWork);
                string status = string.Empty;
                var resources = GetAllUseEntity<Sys_Resource>(ref status);
                var resourceAdding = new List<Sys_Resource>();
                foreach (var sysResource in lstResource)
                {
                    var resourceExist = resources.Where(p => p.ResourceName == sysResource.ResourceName).FirstOrDefault();
                    if (resourceExist == null)
                    {
                        if (sysResource != null && sysResource.ResourceType == HRM.Infrastructure.Utilities.ResourceType.Other.ToString() && !string.IsNullOrEmpty(sysResource.ResourceName))
                        {
                            sysResource.Category = sysResource.ModuleName;
                        }

                        var resourceAddNew = sysResource.CopyData<Sys_Resource>();
                        resourceAddNew.ID = Guid.NewGuid();
                        var resourceExisted = resourceAdding.Where(p => sysResource != null && p.ResourceName == sysResource.ResourceName).FirstOrDefault();
                        if (resourceExisted == null)
                        {
                            resourceAdding.Add(resourceAddNew);
                        }
                    }
                }
                repo.Add(resourceAdding);
                repo.SaveChanges();
            }
            return false;
        }

        public List<string> GetAllResourceName()
        {
            using (var context = new VnrHrmDataContext())
            {
                var isExistedUser = false;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_ResourceRepository(unitOfWork);
                var existedUser = repo.FindBy(p => p.IsDelete == null).Select(p => p.ResourceName).ToList();
                return existedUser;
            }
        }
    }
}
