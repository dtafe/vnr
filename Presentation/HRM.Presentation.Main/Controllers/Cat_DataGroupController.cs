using System.Web.Mvc;
using HRM.Presentation.Category.Models;
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
    public class Cat_DataGroupController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Cat_DataGroup/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_DataGroupModel>(_hrm_Hr_Service, "api/Cat_DataGroup/", selectedIds, ConstantPermission.Cat_DataGroup, DeleteType.Remove.ToString());
        }

        public ActionResult Create(Cat_DataGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_DataGroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_DataGroup/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Overtime_CreateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Cat_DataGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_DataGroup/", id);
            return View(result);
        }
    }
}