using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Recruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.Payroll.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Kai_RankMarkController : MainBaseController
    {
        readonly string _Hrm_Kai_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Kai_RankMark/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kai_RankMarkInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Kai_RankMarkModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Kai_Service);
                var modelResult = service.Get(_Hrm_Kai_Service, "api/Kai_RankMark/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Kai_RankMarkModel>(_Hrm_Kai_Service, "api/Kai_RankMark/", selectedIds, ConstantPermission.Kai_RankMark, DeleteType.Remove.ToString());
        }
	}
}