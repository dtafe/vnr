using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Attendance.Models;
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_GradeAttendanceController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Cat_GradeAttendance/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult GradeAttendanceInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_TAMScanLogModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_GradeAttendance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Cat_GradeAttendance
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_GradeAttendanceModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_GradeAttendance/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Cat_GradeAttendance
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Cat_GradeAttendanceModel model)
        {
            
            var service = new RestServiceClient<Cat_GradeAttendanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_GradeAttendance/", model);
            return Json(result);
        }  
       
        public ActionResult Remove(string id)
        {
           
            var service = new RestServiceClient<Cat_GradeAttendanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_GradeAttendance/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Cat_GradeAttendanceModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_GradeAttendanceModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_GradeAttendance/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_GradeAttendance_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(string id)
        {
          
            var service = new RestServiceClient<Cat_GradeAttendanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_GradeAttendance/", id);            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_GradeAttendanceModel Cat_GradeAttendance)
        {
           
            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_GradeAttendanceModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_GradeAttendance/", Cat_GradeAttendance);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_GradeAttendance_UpdateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_GradeAttendanceModel>(_hrm_Hr_Service, "api/Cat_GradeAttendance/", string.Join(",",selectedIds), ConstantPermission.Cat_GradeAttendance,DeleteType.Remove.ToString());


        }
      
    }
    
}