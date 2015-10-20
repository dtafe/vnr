using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Hr.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_RegisterComeBackController : MainBaseController
    {
        private readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            var userId = Session["UserId"];
           
            return View();
        }

        public ActionResult RegisterComeBackInfo(string id)
        {
            var userId = Session["UserId"];
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var modelResult = service.Get(_hrm_Hre_Service, "api/Hre_StopWorking/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult WorkingPositionInfo(string id)
        {
            var userId = Session["UserId"];
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var modelResult = service.Get(_hrm_Hre_Service, "api/Hre_WorkHistory/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult ResAppendixContractInfo(string id)
        {
            var userId = Session["UserId"];
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_AppendixContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var modelResult = service.Get(_hrm_Hre_Service, "api/Hre_AppendixContractCustorm/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(List<Guid> selectedIds) 
        {
            return RemoveOrDeleteAndReturn<Hre_StopWorkingModel>(_hrm_Hre_Service, "api/Hre_StopWorking/", string.Join(",", selectedIds), ConstantPermission.Hre_StopWorking, DeleteType.Remove.ToString());
        }
    }
}