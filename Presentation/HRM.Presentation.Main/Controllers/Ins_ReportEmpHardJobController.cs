using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class Ins_ReportEmpHardJobController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
	}
}