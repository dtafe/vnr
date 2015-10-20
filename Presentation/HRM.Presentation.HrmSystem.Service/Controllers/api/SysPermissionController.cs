using HRM.Business.Main.Domain;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    /// <summary> Phân quyền </summary>
    public class SysPermissionController : ApiController
    {
        /// <summary> Check Permission  </summary>
        /// <param name="permissionModel"></param>
        /// <returns></returns>
        public PermissionModel Post(PermissionModel permissionModel)
        {
            var service = new SecurityService();
            bool result = false;
            if (permissionModel != null )
            {
                result = service.CheckPermission(permissionModel.UserID, permissionModel.PrivilegeType, permissionModel.Permission);    
            }
            
            return new PermissionModel() { IsAllowAccess = result };
        }

        /// <summary>
        /// [Hien.Nguyen]
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public TempPermissionModel Put(PermissionModel userID)
        {
            var service = new SecurityService();
            TempPermissionModel result = new TempPermissionModel();
            result.Data = service.GetAllPermission(userID.UserID);
            return result;
        }

        public TempPermissionModel Get(Guid id)
        {
            var service = new SecurityService();
            TempPermissionModel result = new TempPermissionModel();
            result.Data = service.GetAllPermission(id);
            return result;
        }
    }
}
