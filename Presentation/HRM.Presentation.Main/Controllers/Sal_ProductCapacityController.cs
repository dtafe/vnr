using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_ProductCapacityController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: Sal_ProductCapacity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sal_ProductCapacityInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_ProductCapacityModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_ProductCapacity/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_ProductCapacityModel>(_hrm_Hr_Service, "api/Sal_ProductCapacity/", selectedIds, ConstantPermission.Sal_ProductCapacity, DeleteType.Remove.ToString());
        }
    }
}