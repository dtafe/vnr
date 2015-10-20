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
using System;
using System.Linq;
using HRM.Presentation.Category.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ComputeWorkDayController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            #region [Hien.Nguyen] get data for selectbox edit inline on gird
            var service = new RestServiceClient<IEnumerable<CatShiftMultiModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var catShift = service.Get(_Hrm_Hre_Service, "api/CatShift/");
            ViewData["Cat_Shift"] = catShift; 
            #endregion

            return View();
        }

        public ActionResult Compute()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ComputeWorkDay([DataSourceRequest] DataSourceRequest request, Att_ComputeWorkDayModel model)
        {
            var service = new RestServiceClient<IEnumerable<Att_WorkdayModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Att_ComputeWorkDay/", model);

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Att_WorkdayModel> lstmodel)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            if (lstmodel != null)
            {
                foreach (var item in lstmodel)
                {
                    service.Put(_Hrm_Hre_Service, "api/Att_WorkDay/", item);
                }
            }

            return Json(lstmodel);
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            var computeWorkDay = new List<Att_ComputeWorkDayModel>();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Att_ComputeWorkDayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Att_ComputeWorkDay/", id);
                }
            }
            return Json("");
        }

	}
}