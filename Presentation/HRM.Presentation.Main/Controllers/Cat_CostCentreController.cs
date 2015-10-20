using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;

//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_CostCentreController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatCostCentre/
        public ActionResult Index()
        {
            
            return View();
        }
        /// <summary>
        /// Màn hình cho edit và create
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CatCostCentreInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatCostCentreModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var modelResult = service.Get(_hrm_Hre_Service, "api/CatCostCentre/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Remove(Guid id)
        {
            
            var service = new RestServiceClient<CatCostCentreModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hre_Service);
            var result = service.Delete(_hrm_Hre_Service, "api/CatCostCentre/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatCostCentreModel>(_hrm_Hre_Service, "api/CatCostCentre/", selectedIds, ConstantPermission.Cat_CostCentre, DeleteType.Remove.ToString());
      
        }
    } 
}