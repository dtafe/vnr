using System.Web.Mvc;
using HRM.Presentation.UI.Controls.Menu;
using HRM.Presentation.Attendance.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_ProfileAttendanceController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Att_Service;

        //
        // GET: /Att_ComputeAttendance/
        public ActionResult Hre_ProfileAttendanceDetail()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_ProfileAttendance);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        public ActionResult ComputeAttendance(Att_AttendanceTableModel model)
        {
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Att_ComputeAttendance/", model);
            return View(result);
        }
	}
}