using System;
using System.Linq;
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

    public class Cat_ShiftController : MainBaseController
    {
        //readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatShift/
        public ActionResult Index()
        {
       
            return View();
        }


        public ActionResult CatShiftInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatShiftModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatShift/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatShift
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatShiftModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatShift/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] CatShiftModel model)
        {
         
            var service = new RestServiceClient<CatShiftModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatShift/", model);
            return Json(result);
        }
      
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind] CatShiftModel model)
        {

            var service = new RestServiceClient<CatShiftModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatShift/", model);
            return Json(result);
        }
      
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
    
            bool delete = false;
            var service = new RestServiceClient<CatShiftModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatShift/", id);
            return Json(result);
        }

        public ActionResult Create([Bind] CatShiftModel model)
        {

            var service = new RestServiceClient<CatShiftModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatShift/", model);
            return View();
        }

        public ActionResult Edit(string id)
        {

            var id1 = Common.ConvertToGuid(id);
            var service = new RestServiceClient<CatShiftModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatShift/", id1);
            return View(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatShiftModel>(_hrm_Hr_Service, "api/CatShift/", string.Join(",",selectedIds),
                ConstantPermission.Cat_Shift, DeleteType.Remove.ToString());
        }

        public ActionResult ShiftItemInfo(string id, string ShiftName)
        {
 
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatShiftItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/CatShiftItem/", id1);
                if (modelResult.ShiftID == Guid.Empty)
                    modelResult.ShiftID = id1;
                return View(modelResult);
            }
            else
            {
                if (Request["ShiftID"] != null)
                {
                    string aaa = Request["ShiftID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.ShiftID = aaa;
                    }
                }
                return View();
            }
        }

        public ActionResult RemoveSelectedShiftItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatShiftItemModel>(_hrm_Hr_Service, "api/CatShiftItem/", selectedIds, ConstantPermission.Cat_Shift, DeleteType.Remove.ToString());
        }

        public ActionResult ShiftovertimeInfo(string id, string ShiftName)
        {
     
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatShiftItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/CatShiftItem/", id1);
                if (modelResult.ShiftID == Guid.Empty)
                    modelResult.ShiftID = id1;
                return View(modelResult);
            }
            else
            {
                if (Request["ShiftID"] != null)
                {
                    string aaa = Request["ShiftID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.ShiftID = aaa;
                    }
                }
                return View();
            }
        }

        public ActionResult RemoveSelectedShiftovertime(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatShiftItemModel>(_hrm_Hr_Service, "api/CatShiftItem/", selectedIds, ConstantPermission.Cat_Shift, DeleteType.Remove.ToString());
        }
    }
    
}