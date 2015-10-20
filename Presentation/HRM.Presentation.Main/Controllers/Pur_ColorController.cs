using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Pur_ColorController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Pur_Color/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Pur_ColorInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.DotNetToOracle(id);
                var service = new RestServiceClient<PUR_ColorModelModel>(UserLogin);

                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Pur_Color/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<PUR_ColorModelModel>(_hrm_Hr_Service, "api/Pur_Color/", selectedIds, ConstantPermission.Pur_Color_Delete, DeleteType.Remove.ToString());
        }
    }
}