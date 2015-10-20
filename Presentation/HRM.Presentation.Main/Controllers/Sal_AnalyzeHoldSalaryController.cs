using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using System.Linq;
using HRM.Presentation.Payroll.Models;
namespace HRM.Presentation.Main.Controllers
{
    public class Sal_AnalyzeHoldSalaryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Sal_ReportSalaryTableMonth/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Prefix = "totalRecord")] List<Sal_HoldSalaryModel> TotalAtt_OvertimeModel, [Bind(Prefix = "models")] List<Sal_HoldSalaryModel> updatedAtt_OvertimeModel)
        {
            List<Sal_HoldSalaryModel> lstReturn = new List<Sal_HoldSalaryModel>();
            Sal_HoldSalaryModel _return = new Sal_HoldSalaryModel();

            var service = new RestServiceClient<Sal_HoldSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            if (updatedAtt_OvertimeModel != null)
            {

                for (int i = 0; i < updatedAtt_OvertimeModel.Count; i++)
                {
                  _return =  service.Post(_hrm_Hr_Service, "api/Sal_HoldSalary/", updatedAtt_OvertimeModel[i]);
                }
                lstReturn.Add(_return);
            }

            DataSourceRequest request = new DataSourceRequest()
            {
                Page = 1,
                PageSize = 50
            };
            return Json(lstReturn.ToDataSourceResult(request));
        }

      
	}
}