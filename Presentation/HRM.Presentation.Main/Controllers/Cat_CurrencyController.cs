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
    public class Cat_CurrencyController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatCurrency/
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatCurrency
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatCurrency([DataSourceRequest] DataSourceRequest request)
        {            
            var service = new RestServiceClient<IEnumerable<CatCurrencyModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Hre_Service, "api/CatCurrency/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CatCurrency
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatCurrencyModel model)
        {
            
            var service = new RestServiceClient<CatCurrencyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Put(_hrm_Hre_Service, "api/CatCurrency/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(string id)
        {
            
            var service = new RestServiceClient<CatCurrencyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Delete(_hrm_Hre_Service, "api/CatCurrency/", id);
            return Json(result);
        }

        public ActionResult CreateOrUpdate(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatCurrencyModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var result = service.Get(_hrm_Hre_Service, "api/CatCurrency/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatCurrencyModel>(_hrm_Hre_Service, "api/CatCurrency/", selectedIds, ConstantPermission.Cat_Currency, DeleteType.Remove.ToString());
          
        }

    }
}