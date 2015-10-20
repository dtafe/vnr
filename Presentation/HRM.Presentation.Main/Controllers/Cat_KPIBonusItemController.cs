using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_KPIBonusItemController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Cat_KPIBonusItem/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CatKPIBonusItemInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table Cat_KPIBonus
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_KPIBonusItemModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonusItem/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Cat_KPIBonus
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Cat_KPIBonusItemModel model)
        {
            
            var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            
            var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", id);
            return Json(result);
        }
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            
            var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusItemModel user)
        {
            

            var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
      
            var user = new List<Cat_KPIBonusItemModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Cat_KPIBonusItemModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Hr_Service, "api/Cat_KPIBonusItem/", id);
                }
            }
            return Json("");
        }
	}
}