using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Insurance.Models;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>Truy Lĩnh Bảo Hiểm</summary>
    public class Ins_InsuranceSalaryPaybackController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
              
        public ActionResult InsurancePayBackInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Ins_InsuranceSalaryPaybackModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Ins_InsuranceSalaryPayback/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Ins_InsuranceSalaryPaybackModel>(_Hrm_Hr_Service, "api/Ins_InsuranceSalaryPayback/", selectedIds, ConstantPermission.Ins_InsuranceSalaryPayback, DeleteType.Remove.ToString());
        }
	}
}