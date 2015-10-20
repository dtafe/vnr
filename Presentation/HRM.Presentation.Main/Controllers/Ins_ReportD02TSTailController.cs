using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>
    /// Đuôi D02-TS
    /// </summary>
    public class Ins_ReportD02TSTailController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
	}
}