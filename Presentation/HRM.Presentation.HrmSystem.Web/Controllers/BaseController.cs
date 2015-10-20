using System;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Properties

        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public string UserName
        {
            get
            {
                var userName = Session["UserName"].ToString();
                if (!string.IsNullOrEmpty(userName))
                {
                    return userName;
                }
                return string.Empty;
            }
        }

        public Guid UserId
        {
            get
            {
                if (Session["UserId"] !=null)
                {
                    Guid userId = (Guid)Session["UserId"];

                    if (userId != Guid.Empty)
                    {
                        return userId;
                    }
                }

                return Guid.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary> Kiểm tra quyền cho user </summary>
        /// <param name="userId"></param>
        /// <param name="privilegeType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool CheckPermission(Guid userId, PrivilegeType privilegeType, string permission)
        {
           return true;
            var permissionModel = new PermissionModel
            {
                UserID = userId,
                PrivilegeType = privilegeType,
                Permission = permission
            };
            var service = new RestServiceClient<PermissionModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            PermissionModel result = service.Post(_hrm_Sys_Service, "api/SysPermission/", permissionModel);
            return result.IsAllowAccess;
        }

        #endregion

        /// <summary>
        /// [Chuc.Nguyen]- Xóa hoặc chuyển đổi trạng thái IsDelete = true của một đối tượng bất kỳ
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlService"></param>
        /// <param name="api"></param>
        /// <param name="selectedIds"></param>
        /// <param name="constantPermission"></param>
        /// <param name="deleteType"></param>
        /// <returns></returns>
        public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, List<Guid> selectedIds, string constantPermission, string deleteType) where TModel : class
        {

            if (selectedIds.Count >= 0)
            {
                var service = new RestServiceClient<TModel>();
                service.SetCookies(this.Request.Cookies, urlService);
                for (int i = 0; i < selectedIds.Count - 1; i++)
                {
                    service.Delete(urlService, api, deleteType + "," + selectedIds[i]);
                }
                var dataReturn = service.Delete(urlService, api, deleteType + "," + selectedIds[selectedIds.Count - 1]);
                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, string selectedIds, string constantPermission, string deleteType) where TModel : class
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, constantPermission);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = deleteType + "," + selectedIds;
                var service = new RestServiceClient<TModel>();
                service.SetCookies(this.Request.Cookies, urlService);
                var dataReturn = service.Delete(urlService, api, selectedIds);

                //var service = new RestServiceClient<TModel>();
                //service.SetCookies(this.Request.Cookies, urlService);
                //for (int i = 0; i < selectedIds.Count - 1; i++)
                //{
                //    service.Delete(urlService, api, selectedIds[i]);
                //}
                //var dataReturn = service.Delete(urlService, api, selectedIds[selectedIds.Count - 1]);
                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        
	}
}