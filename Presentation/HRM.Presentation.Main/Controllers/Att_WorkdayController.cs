using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_WorkdayController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttWorkDay
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_WorkdayModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_WorkDay/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttWorkDay
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_WorkdayModel model)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_WorkDay/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_WorkDay/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
     
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_WorkdayModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_WorkDay/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_WorkDay_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_WorkDay/",id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_WorkdayModel AttWorkDay)
        {           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_WorkDay/", AttWorkDay);                
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_WorkDay_UpdateSuccess.TranslateString();
            }
            return View();
        }

        /// <summary>
        /// Tạo mời chỉnh sửa Nghỉ Phép Và Trễ Sớm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateInline([Bind(Prefix="models")] List<Att_WorkdayModel> model)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            Att_WorkdayModel result = new Att_WorkdayModel();
            for (int i = 0; i < model.Count; i++)
            {
                #region [Hien.Nguyen] xử lý gộp time vào datetime
                //Nếu date == null
                if (model[i].FirstInTime == null)
                {
                    model[i].FirstInTime = model[i].WorkDate;//gán bằng với workdate
                    if (model[i].TempTimeIn != null)//Nếu giờ khác null thì gán giờ vào
                    {
                        model[i].FirstInTime = new DateTime(model[i].FirstInTime.Value.Year, model[i].FirstInTime.Value.Month,
                            model[i].FirstInTime.Value.Day, int.Parse(model[i].TempTimeIn.Split(':').ToList()[0]),
                           int.Parse(model[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(model[i].TempTimeIn.Split(':').ToList()[2]));
                    }
                }
                else//Nếu date != null 
                {
                    if (model[i].TempTimeIn != null)//Nếu time khác null thì gán time vào cho date
                    {
                        model[i].FirstInTime = new DateTime(model[i].FirstInTime.Value.Year, model[i].FirstInTime.Value.Month,
                           model[i].FirstInTime.Value.Day, int.Parse(model[i].TempTimeIn.Split(':').ToList()[0]),
                           int.Parse(model[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(model[i].TempTimeIn.Split(':').ToList()[2]));
                    }
                }
                if (model[i].LastOutTime == null)
                {
                    model[i].LastOutTime = model[i].WorkDate;//gán bằng với workdate
                    if (model[i].TempTimeOut != null)//Nếu giờ khác null thì gán giờ vào
                    {
                        model[i].LastOutTime = new DateTime(model[i].LastOutTime.Value.Year, model[i].LastOutTime.Value.Month,
                            model[i].LastOutTime.Value.Day, int.Parse(model[i].TempTimeOut.Split(':').ToList()[0]),
                            int.Parse(model[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(model[i].TempTimeOut.Split(':').ToList()[2]));
                    }
                }
                else
                {
                    if (model[i].TempTimeOut != null)//Nếu time khác null thì gán time vào cho date
                    {
                        model[i].LastOutTime = new DateTime(model[i].LastOutTime.Value.Year, model[i].LastOutTime.Value.Month,
                           model[i].LastOutTime.Value.Day, int.Parse(model[i].TempTimeOut.Split(':').ToList()[0]),
                           int.Parse(model[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(model[i].TempTimeOut.Split(':').ToList()[2]));
                    }
                }
                #endregion
                result = service.Post(_Hrm_Hre_Service, "api/Att_WorkDay/", model[i]);
            }
            return Json(result);
        }

        /// <summary>
        /// Tạo mời chỉnh sửa Nghỉ Phép Và Trễ Sớm page ComputeWorkDayAdjust
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateInlineAdjust([Bind(Prefix = "models")] List<Att_WorkdayModel> model)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            Att_WorkdayModel result = new Att_WorkdayModel();
            for (int i = 0; i < model.Count; i++)
            {

                #region [Hien.Nguyen] xử lý gộp time vào datetime
                //Nếu date == null
                if (model[i].InTime1 == null)
                {
                    model[i].InTime1 = model[i].WorkDate;//gán bằng với workdate
                    if (model[i].TempTimeIn != null)//Nếu giờ khác null thì gán giờ vào
                    {
                        model[i].InTime1 = new DateTime(model[i].InTime1.Value.Year, model[i].InTime1.Value.Month,
                            model[i].InTime1.Value.Day, int.Parse(model[i].TempTimeIn.Split(':').ToList()[0]),
                            int.Parse(model[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(model[i].TempTimeIn.Split(':').ToList()[2]));
                    }
                }
                else//Nếu date != null 
                {
                    if (model[i].TempTimeIn != null)//Nếu time khác null thì gán time vào cho date
                    {
                        model[i].InTime1 = new DateTime(model[i].InTime1.Value.Year, model[i].InTime1.Value.Month,
                           model[i].InTime1.Value.Day, int.Parse(model[i].TempTimeIn.Split(':').ToList()[0]),
                           int.Parse(model[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(model[i].TempTimeIn.Split(':').ToList()[2]));
                    }
                }
                if (model[i].OutTime1 == null)
                {
                    model[i].OutTime1 = model[i].WorkDate;//gán bằng với workdate
                    if (model[i].TempTimeOut != null)//Nếu giờ khác null thì gán giờ vào
                    {
                        model[i].OutTime1 = new DateTime(model[i].OutTime1.Value.Year, model[i].OutTime1.Value.Month,
                            model[i].OutTime1.Value.Day, int.Parse(model[i].TempTimeOut.Split(':').ToList()[0]),
                             int.Parse(model[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(model[i].TempTimeOut.Split(':').ToList()[2]));
                    }
                }
                else
                {
                    if (model[i].TempTimeOut != null)//Nếu time khác null thì gán time vào cho date
                    {
                        model[i].OutTime1 = new DateTime(model[i].OutTime1.Value.Year, model[i].OutTime1.Value.Month,
                           model[i].OutTime1.Value.Day, int.Parse(model[i].TempTimeOut.Split(':').ToList()[0]),
                            int.Parse(model[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(model[i].TempTimeOut.Split(':').ToList()[2]));
                    }
                }
                #endregion
                result = service.Post(_Hrm_Hre_Service, "api/Att_WorkDay/", model[i]);
            }
            return Json(result);
        }
        public ActionResult Submit(Guid id)
        {
            var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_WorkDay/", id);
            return View(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_WorkdayModel>(_Hrm_Hre_Service, "api/Att_WorkDay/", selectedIds, ConstantPermission.Att_Workday, DeleteType.Remove.ToString());
        }

        /// <summary>
        /// Xử lí thay doi trang thai cua ngày công
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status)
        {
            var model = new Att_WorkdayModel();
            if (selectedIds != null)
            {
                var selectedIdsOrcl = String.Join(",", selectedIds);
                var service = new RestServiceClient<Att_WorkdayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                model.ProfileIds = selectedIdsOrcl;
                model.Status = status;
                model = service.Put(_Hrm_Hre_Service, "api/Att_WorkDay/", model);
            }
            return Json(model);
        } 

	}
}