using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ExceptionOvertimeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;      

        public ActionResult Index()
        {
            return View();
        }      

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            var ot = new Att_OvertimeModel();
            if (selectedIds != null || selectedIds.Count>0)
            {
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Att_Overtime/", id);
                }
            }
            return Json("");
        }
        /// <summary>
        /// Xử lí thay doi trang thai cua overtime
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(string selectedIds, string status)
        {
            var ot = new Att_OvertimeModel();
            if (selectedIds != null)
            {
                var lstSelectedIds = selectedIds.Split(',');
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in lstSelectedIds)
                {
                    ot.ID = Guid.Parse(id);
                    ot.Status = status;
                    service.Put(_Hrm_Hre_Service, "api/Att_OvertimeCustom/", ot);
                }
            }
            return Json("");
        } 
    }
}