using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Lau_ReportExceptClothingController : MainBaseController
    {
        private readonly string _hrm_Lau_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Provider/
        public ActionResult Index()
        {
            return View();
        }
    }
}
