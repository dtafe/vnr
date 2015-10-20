using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ContractTypeController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatContractType/
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatCountry
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatContractType([DataSourceRequest] DataSourceRequest request)
        {           
            var service = new RestServiceClient<IEnumerable<CatContractTypeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Hre_Service, "api/CatContractType/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatContractTypeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Hre_Service, "api/CatContractType/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatCountry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatContractTypeModel model)
        {
            
            //var service = new RestServiceClient<CatContractTypeModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            //var result = service.Put(_hrm_Hre_Service, "api/CatContractType/", model);
            //return Json(result);
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Overtime);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<CatContractTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Hre_Service, "api/CatContractType/", id);
            return View(result);
        }
        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
           
            var service = new RestServiceClient<CatContractTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Delete(_hrm_Hre_Service, "api/CatContractType/", id);
            return Json(result);
        }

        public ActionResult CatContractTypeInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatContractTypeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var result = service.Get(_hrm_Hre_Service, "api/CatContractType/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult InsuranceConfigInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_InsuranceConfigModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var result = service.Get(_hrm_Hre_Service, "api/Cat_InsuranceConfig/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            
            var service = new RestServiceClient<CatContractTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Remove(_hrm_Hre_Service, "api/CatContractType/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatContractTypeModel>(_hrm_Hre_Service, "api/CatContractType/", selectedIds, ConstantPermission.Cat_ContractType,DeleteType.Remove.ToString());
     
        }
	}
}