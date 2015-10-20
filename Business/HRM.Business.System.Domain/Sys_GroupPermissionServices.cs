using System;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Data;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_GroupPermissionServices : BaseService
    {
        public void CreateOrUpdateGroupPermission(Guid groupId, List<Sys_GroupPermission2Entity> lstAdding, List<Sys_GroupPermission2Entity> lstEdit, List<Guid> resetPermissionResourceIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork) (new UnitOfWork(context));
                var repoGroupPermission2 = new Sys_GroupPermission2Repository(unitOfWork);
                var sysGroupPermission2 = new Sys_GroupPermission2();
                var hasChangeData = false;
                var edit = lstEdit.Translate<Sys_GroupPermission2>();
                var add = lstAdding.Translate<Sys_GroupPermission2>();
                if (add.Any())
                {
                    #region Tạm thời khoá đoạn này vì trên controller api (api/Sys_GroupPermissionController) đã xử lý rồi
                    //Trước khi thêm mới => lay những resouces không nằm trong db
                    //var groupPermisstion2s = GetGroupPermissionByGroup(groupId);
                    //var resourceIds = groupPermisstion2s.Select(p=>p.ResourceID).ToList();
                    //add = add.Where(p => !resourceIds.Contains(p.ResourceID)).ToList(); 
                    #endregion

                    //thêm mới
                    repoGroupPermission2.Add(add);
                    hasChangeData = true;
                }
                if (edit.Any())
                {
                    repoGroupPermission2.Edit(edit);
                    hasChangeData = true;
                }

                if (hasChangeData)
                {
                    repoGroupPermission2.SaveChanges();
                }



                #region reset quyền là 0 neu tat ca groupPermissionByGroupId khi chưa gán quyền (view,insert,modify....)

                if (groupId != Guid.Empty)
                {
                    var listResourceId = new List<Guid>();
                    listResourceId = lstAdding.Select(p => p.ResourceID).ToList();
                    if (lstEdit.Any())
                    {
                        var listResourceIdEdit = lstEdit.Select(p => p.ResourceID).ToList();
                        listResourceId.AddRange(listResourceIdEdit);
                    }
                    listResourceId = listResourceId.Distinct().ToList();

                    var serviceGroupPermission = new Sys_GroupPermissionServices();
                    var groupPermissions = serviceGroupPermission.GetAllGroupPermissionByGroupId(groupId).Translate<Sys_GroupPermission2>();
                    groupPermissions = groupPermissions.Where(p => !resetPermissionResourceIds.Contains(p.ResourceID)).ToList();
                    if (groupPermissions.Any())
                    {
                        foreach (var sysGroupPermissionEntity in groupPermissions)
                        {
                            sysGroupPermissionEntity.PrivilegeNumber = 0;
                            repoGroupPermission2.Edit(sysGroupPermissionEntity);
                        }
                        repoGroupPermission2.SaveChanges();
                    }

                }
                #endregion


            }
        }


        public List<Sys_GroupPermission2Entity> GetAllGroupPermissionByGroupId(Guid groupId)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_GroupPermissionRepository repo = new Sys_GroupPermissionRepository(unitOfWork);
                var groupPermissions = repo.GetAllGroupPermissionByGroupId(groupId);

                return groupPermissions.Translate<Sys_GroupPermission2Entity>();
            }
        }

        public List<Sys_GroupPermission2Entity> GetGroupPermissionByGroup(Guid groupId)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_GroupPermissionRepository repo = new Sys_GroupPermissionRepository(unitOfWork);
                var model = repo.GetGroupPermissionByGroup(groupId).Translate<Sys_GroupPermission2Entity>();
                return model;
                //return null;
            }
        }

        public Sys_GroupPermission2Entity GetGroupPermissionByGroupAndResoure(Guid groupId, Guid resourceId)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_GroupPermissionRepository repo = new Sys_GroupPermissionRepository(unitOfWork);
                var model = repo.GetGroupPermissionByGroupAndResoure(groupId, resourceId).FirstOrDefault();
                return model.CopyData<Sys_GroupPermission2Entity>();
            }
        }
        
    }
}
