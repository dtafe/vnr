using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Training.Models;
using Kendo.Mvc.UI;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_SkillController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_SkillModel>(_hrm_Hr_Service, "api/Cat_Skill/", selectedIds, ConstantPermission.Cat_Skill, DeleteType.Remove.ToString());
        }
        public ActionResult Create(Cat_SkillModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_SkillModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_Skill/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Overtime_CreateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Cat_SkillModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_Skill/", id);
            return View(result);
        }
       
    }
}