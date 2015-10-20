using System.Net.Http.Formatting;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VnResource.Helper.Data;
using VnResource.Helper.Security;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_GroupPermissionController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    //var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    //userLogin = headerValues.FirstOrDefault();
                    userLogin = "son.vo";
                }
                return userLogin;
            }
        }
        #endregion
        /// <summary> Lấy danh sách nhóm quyền theo GroupId </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Sys_GroupPermissionModel> Get(Guid id)
        {
            string statusGroupPermission = string.Empty;
            string status = string.Empty;
            var serviceGroupPermission = new Sys_GroupPermissionServices();
            var serviceResource = new Sys_ResourceServices();

            var listGroupPermission =
                serviceGroupPermission.GetData<Sys_GroupPermission2Entity>(id,
                    ConstantSql.hrm_sys_sp_get_GroupPermissionByGroupId, UserLogin, ref statusGroupPermission).Select(p => new Sys_GroupPermissionModel
            {
                ID = p.ID,
                GroupID = p.GroupID,
                ResourceID = p.ResourceID,
                GroupName = p.GroupName,
                ModuleName = p.ModuleName,
                ResourceName = p.ResourceName,
                ResourceType = p.ResourceType,
                Code = p.Code,
                Category = p.Category,
                PrivilegeNumber = Convert.ToInt32(p.PrivilegeNumber ?? 0)
            }).ToList();

            List<Guid> listResourceID = listGroupPermission.Select(d => d.ResourceID).Distinct().ToList();
            var resourceQueryable = serviceResource.GetAllResources();

            if (listResourceID.Count() >= 0 && resourceQueryable != null)
            {
                resourceQueryable = resourceQueryable.Where(d => !listResourceID.Contains(d.ID)).ToList();
            }

            if (listGroupPermission != null && resourceQueryable != null && resourceQueryable.Any())
            {
                listGroupPermission.AddRange(resourceQueryable.Select(d => new Sys_GroupPermissionModel
                {
                    GroupID = id,
                    ResourceID = d.ID,
                    ModuleName = d.ModuleName,
                    Category = d.Category,
                    ResourceType = d.ResourceType,
                    ResourceName = d.ResourceName
                }).ToList());
            }


            var lstGroupPermissionWithLMS = listGroupPermission.Where(p => p.Category == "LMS").ToList();
            var lstGroupPermissionWithoutLMS = listGroupPermission.Where(p => p.Category != "LMS").ToList();

            foreach (var sysResourceEntity in lstGroupPermissionWithLMS)
            {
                if (sysResourceEntity.Category == "LMS")
                {
                    sysResourceEntity.Category = HRM.Infrastructure.Utilities.ModuleName.Laundry.ToString();
                    sysResourceEntity.ModuleName = HRM.Infrastructure.Utilities.ModuleName.Laundry.ToString();
                }
            }

            lstGroupPermissionWithLMS.AddRange(lstGroupPermissionWithoutLMS);
            lstGroupPermissionWithLMS = lstGroupPermissionWithLMS.OrderBy(p => p.ResourceName).ToList();

            return lstGroupPermissionWithLMS ?? (lstGroupPermissionWithLMS = new List<Sys_GroupPermissionModel>());
            //   return listGroupPermission ?? (listGroupPermission = new List<Sys_GroupPermissionModel>());
        }

        /// <summary> Lưu nhóm quyền </summary>
        /// <param name="grouppermissionInfo"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_GroupEntityPermission2")]
        public Guid Post([FromBody]FormDataCollection grouppermissionInfo)
        {
            Guid groupId = Guid.Empty;
            if (grouppermissionInfo != null)
            {
                #region Lưu Sys_GroupEntity
                var groupInfos = grouppermissionInfo.Where(p => !p.Key.Contains("allow")).Distinct().ToList();
                Guid id = Guid.Empty;
                bool IsActivate = false;

                #region Get Group Info
                Guid.TryParse(groupInfos.Where(p => p.Key == "ID").Select(p => p.Value).FirstOrDefault(), out id);
                var code = groupInfos.Where(p => p.Key == Sys_GroupModel.FieldNames.Code).Select(p => p.Value).FirstOrDefault();
                var description = groupInfos.Where(p => p.Key == Sys_GroupModel.FieldNames.Notes).Select(p => p.Value).FirstOrDefault();
                var groupName = groupInfos.Where(p => p.Key == Sys_GroupModel.FieldNames.GroupName).Select(p => p.Value).FirstOrDefault();
                bool.TryParse(groupInfos.Where(p => p.Key == Sys_GroupModel.FieldNames.IsActivate).Select(p => p.Value).FirstOrDefault(), out IsActivate);
                #endregion

                var group = new Sys_GroupEntity()
                {
                    ID = id,
                    IsActivate = IsActivate,
                    Code = code,
                    Notes = description,
                    GroupName = groupName
                };
                var groupService = new Sys_GroupServices();
                if (group.ID == Guid.Empty)
                {
                    //add
                    var statusAdding = groupService.Add<Sys_GroupEntity>(group);
                    groupId = group.ID;
                }
                else
                {
                    //edit
                    string statusSysGroup = string.Empty;
                    groupId = id;
                    var statusEditing = groupService.GetSysGroupById(group.ID);
                    if (statusEditing != null)
                    {
                        statusEditing.IsActivate = group.IsActivate;
                        statusEditing.Code = group.Code;
                        statusEditing.Notes = group.Notes;
                        statusEditing.Description = group.Notes;
                        statusEditing.GroupName = group.GroupName;
                        groupService.Edit<Sys_GroupEntity>(statusEditing);
                    }
                }

                #endregion

                List<Guid> listResourceId = new List<Guid>();
                List<string> lstValue = grouppermissionInfo.Where(p => p.Key.Contains("allow") && p.Value != "0").Select(m => m.Value).Distinct().ToList();
                var lstAdding = new List<Sys_GroupPermission2Entity>();
                var lstEditing = new List<Sys_GroupPermission2Entity>();
                var service = new Sys_GroupPermissionServices();
                //[Tung.Ly 20150114] - Modify : lay danh sach Sys_GroupPermission2 bởi GroupID bằng entity thay vì dùng store 
                // Dùng store không lấy được dữ liệu => nên tạm thời dùng entity
                var groupPermisstion2s = service.GetAllGroupPermissionByGroupId(groupId);
                foreach (string itemValue in lstValue)
                {
                    //lay thong tin nhom quyen
                    var lstgrouppermissionInfo = grouppermissionInfo.Where(m => m.Key.Contains("allow") && m.Value == itemValue).ToList();
                    int privilegeNumber = 0;
                    Guid resourceId = Guid.Empty;
                    Guid.TryParse(itemValue, out resourceId);
                    if (resourceId != Guid.Empty && !listResourceId.Any(p=>p == resourceId))
                    {
                        listResourceId.Add(resourceId);
                    }

                    #region Get Privilege (View/Create/Modify/Delete/Export/Import)
                    foreach (var item in lstgrouppermissionInfo)
                    {
                        var lst = item.Key.Split(new char[] { '_' });
                        int number = int.Parse(lst[2]);
                        
                        switch (number)
                        {
                            case 1:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.View;
                                break;
                            case 2:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.Create;
                                break;
                            case 3:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.Modify;
                                break;
                            case 4:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.Delete;
                                break;
                            case 5:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.Export;
                                break;
                            case 6:
                                privilegeNumber = privilegeNumber + (int)PrivilegeType.Import;
                                break;
                        }
                    }
                    #endregion

                    #region Thêm/chỉnh sửa nhóm quyền

                    groupId = group.ID;
                    var groupPermisstion = groupPermisstion2s.Where(p => p.ResourceID == resourceId).FirstOrDefault();

                    if (groupPermisstion == null || groupPermisstion.ID == Guid.Empty)
                    {
                        groupPermisstion = new Sys_GroupPermission2Entity
                        {
                            GroupID = groupId,
                            ResourceID = resourceId,
                            PrivilegeNumber = privilegeNumber
                        };
                        if (lstAdding.FirstOrDefault(p=>p.ResourceID == resourceId && p.GroupID == groupId) == null)
                        {
                             lstAdding.Add(groupPermisstion);
                        }
                    }
                    else
                    {
                        if (groupPermisstion.PrivilegeNumber != privilegeNumber)
                        {
                            groupPermisstion.PrivilegeNumber = privilegeNumber;
                            //check groupPermission exist
                            if (lstEditing.FirstOrDefault(p => p.ResourceID == resourceId && p.GroupID == groupId) == null)
                            {
                                lstEditing.Add(groupPermisstion);
                            }
                        }
                    }
                    #endregion

                }//close foreach

                #region Thêm/Chỉnh sửa danh sách nhóm quyền (GroupPermission)

                var gService = new Sys_GroupPermissionServices();
                gService.CreateOrUpdateGroupPermission(groupId, lstAdding, lstEditing, listResourceId);

                #endregion
            }
            return groupId;
        }

        // DELETE api/<controller>/5
        public void Delete(Guid id)
        {
            var service = new Sys_GroupPermissionServices();
            var result = service.Remove<Sys_GroupPermission2Entity>(id);
        }

    }
}