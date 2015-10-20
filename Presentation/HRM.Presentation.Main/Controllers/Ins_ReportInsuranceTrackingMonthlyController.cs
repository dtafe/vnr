using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>
    /// Báo cáo theo dõi mức tiền đóng bảo hiểm hàng tháng xem theo khoảng thời gian
    /// </summary>
    public class Ins_ReportInsuranceTrackingMonthlyController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
	}
}