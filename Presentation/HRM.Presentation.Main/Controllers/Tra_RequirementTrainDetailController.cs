using System.Web.Mvc;
using HRM.Presentation.Training.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Tra_RequirementTrainDetailController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_RequirementTrainDetailModel>(_hrm_Hr_Service, "api/Tra_RequirementTrainDetail/", selectedIds, ConstantPermission.Tra_RequirementTrainDetail, DeleteType.Remove.ToString());
        }
        public ActionResult Tra_RequirementTrainDetailInfo(string id)
        {
       
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_RequirementTrainDetailModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_RequirementTrainDetail/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
       
    }
}