using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Data;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_GroupPermissionController : MainBaseController
    {
        //
        // GET: /Sys_GroupPermission/
        public ActionResult Index(string id)
        {
            
            Guid id1 = Guid.Empty;
            Guid.TryParse(id, out id1);
            var model = new PermissionMappingModel();
            List<RolesNameModel> listRolesName = new List<RolesNameModel>
            {
                new RolesNameModel() {Id = 1, Name = ConstantDisplay.HRM_System_Resource_Sys_View.TranslateString()},
                new RolesNameModel() {Id = 2, Name =  ConstantDisplay.HRM_System_Resource_Sys_Create.TranslateString()},
                new RolesNameModel() {Id = 3,Name =  ConstantDisplay.HRM_System_Resource_Sys_Edit.TranslateString()},
                new RolesNameModel() {Id = 4,Name =  ConstantDisplay.HRM_System_Resource_Sys_Delete.TranslateString()},
                new RolesNameModel() {Id = 5,Name =  ConstantDisplay.HRM_System_Resource_Sys_Export.TranslateString()},
                new RolesNameModel() {Id = 6, Name =  ConstantDisplay.HRM_System_Resource_Sys_Import.TranslateString()}
            };
            model.AvailableRolesName.AddRange(listRolesName);

            var service = new RestServiceClient<IEnumerable<Sys_GroupPermissionModel>>(UserLogin);
            service.SetCookies(Request.Cookies, hrm_Sys_Service);
            var listAvailablePermissions = service.Get(hrm_Sys_Service, "api/Sys_GroupPermission/", id1);

            model.AvailablePermissions.AddRange(listAvailablePermissions);

            var roles = new Dictionary<string, IDictionary<int, bool>>();
            foreach (var pr in listAvailablePermissions)
            {
                var role = new Dictionary<int, bool>
                {
                    {1, PrivilegeType.View.CheckPrivilege(pr.PrivilegeNumber.GetInteger())},
                    {2, PrivilegeType.Create.CheckPrivilege(pr.PrivilegeNumber.GetInteger())},
                    {3, PrivilegeType.Modify.CheckPrivilege(pr.PrivilegeNumber.GetInteger())},
                    {4, PrivilegeType.Delete.CheckPrivilege(pr.PrivilegeNumber.GetInteger())},
                    {5, PrivilegeType.Export.CheckPrivilege(pr.PrivilegeNumber.GetInteger())},
                    {6, PrivilegeType.Import.CheckPrivilege(pr.PrivilegeNumber.GetInteger())}
                };
                if (!string.IsNullOrEmpty(pr.ResourceName) && !roles.ContainsKey(pr.ResourceName))
                {
                    roles.Add(pr.ResourceName, role);
                }
                //   pr.ResourceNameTranslate = pr.ResourceName;
                pr.ResourceNameTranslate = ("HRM_System_Resource_" + pr.ResourceName).TranslateString();

                if (!string.IsNullOrEmpty(pr.ResourceType))
                {
                    pr.ResourceTypeTranslate = ("HRM_System_Resource_" + pr.ResourceType).TranslateString(); 
                }
                // pr.ResourceNameTranslate = (pr.ResourceName).TranslateString();
            }
            model.Allowed = roles;

            #region Dictionary resource
            //Lay danh sach resource để load lên list (bao gồm cả module cha) đã group theo module
            var dictionary = new Dictionary<string, List<Sys_GroupPermissionModel>>();
            string[] listModuleName = typeof(HRM.Infrastructure.Utilities.ModuleName).GetEnumNames();
            if (listModuleName != null && listModuleName.Any())
            {
                foreach (var moduleName in listModuleName)
                {
                    var moduleResource = listAvailablePermissions.Where(p => p.Category == moduleName).OrderBy(p => p.ResourceType).ToList();
                    #region bản 7 xử lý category salary là Payroll => nên sẽ add thêm những resource (category = "Payroll") vào category là Salary (mà không cần sửa lại bản 7)
                    if (moduleName == HRM.Infrastructure.Utilities.ModuleName.Salary.ToString())
                    {
                        var modulePayrollResource = listAvailablePermissions.Where(p => p.Category == "Payroll" && (p.ModuleName != null || p.ResourceType != null)).OrderBy(p => p.ResourceType).ToList();
                        moduleResource.AddRange(modulePayrollResource);
                    } 
                    #endregion

                    var firstModule = listAvailablePermissions.Where(p => p.ResourceName == moduleName).FirstOrDefault();
                    moduleResource.Insert(0, firstModule);
                    if (!string.IsNullOrEmpty(moduleName) && !dictionary.ContainsKey(moduleName))
                    {
                        dictionary.Add(moduleName, moduleResource);
                    }
                }
            }
            ViewBag.listResource = dictionary;
            var groupId = Guid.Empty;
            if (listAvailablePermissions.Any())
            {
                groupId = listAvailablePermissions.FirstOrDefault().GroupID;

                if (groupId != Guid.Empty)
                {
                    var groupService = new RestServiceClient<Sys_GroupModel>(UserLogin);
                    groupService.SetCookies(Request.Cookies, hrm_Sys_Service);
                    var modelGroup = groupService.Get(hrm_Sys_Service, "api/Sys_Group/", groupId);
                    if (modelGroup != null)
                    {
                        model.Code = modelGroup.Code;
                        model.Notes = modelGroup.Notes;
                        model.GroupName = modelGroup.GroupName;
                        model.IsActivate = modelGroup.IsActivate;
                    }
                }

            }
            ViewBag.groupId = groupId;
            #endregion

            return View(model);
        }

        private readonly string hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public ActionResult Sys_GroupPermissionInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_GroupPermissionModel>(UserLogin);
                service.SetCookies(Request.Cookies, hrm_Sys_Service);
                var modelResult = service.Get(hrm_Sys_Service, "api/Sys_GroupPermission/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tạo mời một Setting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Sys_GroupPermissionModel> model)
        {

            var service = new RestServiceClient<Sys_GroupPermissionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, hrm_Sys_Service);
            var sysGroupPermissionModels = model as Sys_GroupPermissionModel[] ?? model.ToArray();
            if (model != null)
            {
                foreach (var item in sysGroupPermissionModels)
                {
                    service.Post(hrm_Sys_Service, "api/Sys_GroupPermission/", item);
                }
            }

            return Json(sysGroupPermissionModels.ToDataSourceResult(request));
        }
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<Sys_GroupPermissionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, hrm_Sys_Service);
            var result = service.Remove(hrm_Sys_Service, "api/Sys_GroupPermission/", id);
            return Json(result);
        }
    }
}