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
    public class Cat_ReligionController : MainBaseController
    {

        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatReligion/
        public ActionResult Index()
        {
         
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatReligion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatReligion([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatReligionModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatReligion/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatReligionModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatReligion/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatReligion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatReligionModel model)
        {
     
            var service = new RestServiceClient<CatReligionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatReligion/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
          
            var service = new RestServiceClient<CatReligionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatReligion/", id);
            return Json(result);
        }

        public ActionResult CreateOrUpdate(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatReligionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatReligion/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        #region RemoveSelected old 
        //public ActionResult RemoveSelected(string selectedIds)
        //{
        //    var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Cat_Religion);
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    var religion = new List<CatReligionModel>(UserLogin);
        //    if (selectedIds != null)
        //    {
        //        var ids = selectedIds
        //            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => Convert.ToInt32(x))
        //            .ToArray();
        //        var service = new RestServiceClient<CatReligionModel>(UserLogin);
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        foreach (var id in ids)
        //        {
        //            service.Delete(_hrm_Hr_Service, "api/CatReligion/", id);
        //        }
        //    }
        //    return Json("");
        //} 
        #endregion
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatReligionModel>(_hrm_Hr_Service, "api/CatReligion/", selectedIds,
                ConstantPermission.Cat_RelativeType, DeleteType.Remove.ToString());
        }
    }
}