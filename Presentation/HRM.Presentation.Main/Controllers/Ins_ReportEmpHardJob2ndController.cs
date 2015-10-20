using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>
    /// Báo cáo dữ liệu HDT lần 2
    /// </summary>
    public class Ins_ReportEmpHardJob2ndController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
	}
}