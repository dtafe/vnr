using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>DS Chi Tiết Đã Đánh Giá 360</summary>
    public class Eva_PerformanceEvaDetail360Controller : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {          
            return View();
        }
        public ActionResult PerformanceEvaDetail360(string id, Guid? EvaluatorID, string IsActive)
        {
            ViewBag.IsActive = "1";           
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", id);
                if (result.EvaluatorID == EvaluatorID)
                    result.IsEvaluator = true;
                return View(result);
            }
            else
            {

                return View();
            }
        }
      
	}
}