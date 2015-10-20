using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

using System;
using System.Linq;
using System.Xml;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_CustomReportController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_CustomReport/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Rep_MasterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Rep_Master/", id);
            return View(result);
        }

        public ActionResult ConditionInfo()
        {
            return View();
        }

        public ActionResult ConditionItemInfo(string id, Guid? MasterID)
        {
            ViewBag.MasterID = MasterID;
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rep_ConditionItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Rep_ConditionItem/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rep_ConditionItemModel>(_hrm_Sys_Service, "api/Rep_ConditionItem/", selectedIds, ConstantPermission.Att_CutOffDuration, DeleteType.Remove.ToString());
        }

        public ActionResult GeneralCustomReport(Guid id)
        {
            var service = new RestServiceClient<Rep_MasterModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var modelResult = service.Get(_hrm_Sys_Service, "api/Rep_Master/", id);

            if (modelResult.ScriptParams != null && modelResult.ScriptParams != string.Empty)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(modelResult.ScriptParams);
                modelResult.NodeList = xml.DocumentElement.SelectNodes("/Root/ControlModel");
            }
           
           

            return View(modelResult);
        }

        public ActionResult RenderControlIndex()
        {
            return View();
        }

    }
}