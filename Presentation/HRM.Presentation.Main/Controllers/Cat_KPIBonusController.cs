using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_KPIBonusController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Cat_KPIBonus/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult CatKPIBonusInfo(string id)
        {
          
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonus/", id1);
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
            var service = new RestServiceClient<IEnumerable<Cat_KPIBonusModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonus/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Cat_KPIBonus
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Cat_KPIBonusModel model)
        {
        
            var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_KPIBonus/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_KPIBonus/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_KPIBonus/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusModel user)
        {
            
            var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_KPIBonus/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
           
            var user = new List<Cat_KPIBonusModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Cat_KPIBonusModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Hr_Service, "api/Cat_KPIBonus/", id);
                }
            }
            return Json("");
        }
    }
}