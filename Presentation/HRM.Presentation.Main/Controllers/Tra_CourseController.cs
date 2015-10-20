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
    public class Tra_CourseController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TraCourseInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_CourseModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_Course/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Tra_ClassModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Tra_CourseModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Tra_Course/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Att_Grade_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Tra_CourseModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_Course/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Tra_ClassModel Sal_Grade)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Tra_CourseModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Tra_Course/", Sal_Grade);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Att_Grade_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_CourseModel>(_hrm_Hr_Service, "api/Tra_Course/", selectedIds, ConstantPermission.Tra_Course, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedTopic(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_CourseTopicModel>(_hrm_Hr_Service, "api/Tra_CourseTopic/", selectedIds, ConstantPermission.Tra_PlanDetail, DeleteType.Remove.ToString());
        }
    }
}