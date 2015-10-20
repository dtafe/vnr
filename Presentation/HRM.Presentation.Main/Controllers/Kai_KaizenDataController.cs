using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Kai_KaizenDataController : MainBaseController
    {
        readonly string _Hrm_Kai_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_Question/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kai_KaizenDataInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Kai_KaizenDataModel>(UserLogin); 
                service.SetCookies(Request.Cookies, _Hrm_Kai_Service);
                var modelResult = service.Get(_Hrm_Kai_Service, "api/Kai_KaizenData/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Kai_KaizenDataModel>(_Hrm_Kai_Service, "api/Kai_KaizenData/", selectedIds, ConstantPermission.Kai_KaizenData, DeleteType.Remove.ToString());
        }

        public ActionResult ApprevedKaiZenDataInfo()
        {
            return View();
        }

        public ActionResult CustomAccumulateInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Kai_KaizenDataModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Kai_Service);
                var modelResult = service.Get(_Hrm_Kai_Service, "api/Kai_KaizenData/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult PaymenOutInfo()
        {
            return View();
        }

	}
}