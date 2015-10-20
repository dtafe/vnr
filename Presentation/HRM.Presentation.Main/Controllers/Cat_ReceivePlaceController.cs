using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Cat_ReceivePlaceController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cat_ReceivePlaceInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.DotNetToOracle(id);
                var service = new RestServiceClient<Cat_ReceivePlaceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_ReceivePlace/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_ReceivePlaceModel>(_hrm_Hr_Service, "api/Cat_ReceivePlace/", selectedIds, ConstantPermission.Cat_ReceivePlace_Delete, DeleteType.Remove.ToString());
        }
	}
}