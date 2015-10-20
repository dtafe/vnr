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
    public class Cat_OvertimeTypeController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatOvertimeType/
        public ActionResult Index()
        {
         
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatCountry
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatOvertimeType([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatOvertimeType/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// 
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatOvertimeType/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatCountry
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, [Bind] CatOvertimeTypeModel model)
        {
       
            var serviceGet = new RestServiceClient<IEnumerable<CatOvertimeTypeModel>>(UserLogin);
            var list = serviceGet.Get(_hrm_Hr_Service, "api/CatOvertimeType/");
            var l = serviceGet.Get(_hrm_Hr_Service, "api/CatOvertimeType/", model.ID);
            foreach (var item in list)
            {
                if (model.Code == item.Code && model.TypeTemp == item.TypeTemp)
                {
                    ModelState.AddModelError("PaymentType", "Đã tồn tại phương thức chi trả với mã này!");
                }

            }
            var result = new CatOvertimeTypeModel();
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatOvertimeTypeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                result = service.Put(_hrm_Hr_Service, "api/CatOvertimeType/", model);
                return Json(result);
            }
            return Json(new[] { result }.ToDataSourceResult(request, ModelState));

        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
 
            var service = new RestServiceClient<CatOvertimeTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatOvertimeType/", id);
            return Json(result);
        }

        public ActionResult CatOvertimeTypeInfo(string id)
        {
    
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatOvertimeTypeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var model = service.Get(_hrm_Hr_Service, "api/CatOvertimeType/", id1);
                if (model.IsHoliday == true && model.IsNightShift == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_HOLIDAY_NIGHTSHIFT.ToString();
                }
                else if (model.IsWeeked == true && model.IsNightShift == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_WEEKEND_NIGHTSHIFT.ToString();
                }
                else if (model.IsWorkDay == true && model.IsNightShift == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_WORKDAY_NIGHTSHIFT.ToString();
                }
                else if (model.IsHoliday == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_HOLIDAY.ToString();
                }
                else if (model.IsWeeked == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_WEEKEND.ToString();
                }
                else if (model.IsWorkDay == true)
                {
                    model.TypeTemp = EnumDropDown.OverTimeType.E_WORKDAY.ToString();
                }
                return View(model);
            }
            else
            {
                CatOvertimeTypeModel model = new CatOvertimeTypeModel();
                return View(model);
            }
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<CatCountryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatOvertimeType/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatOvertimeTypeModel>(_hrm_Hr_Service, "api/CatOvertimeType/", string.Join(",", selectedIds), ConstantPermission.Cat_OvertimeType, DeleteType.Remove.ToString());

        }
    }
}