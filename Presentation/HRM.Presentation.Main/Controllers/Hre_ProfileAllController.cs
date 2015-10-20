using System.IO;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Attendance.Models;
using System;
using System.Linq;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.Xml.Linq;
using System.Xml;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_ProfileAllController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Main_Web = ConstantPathWeb.Hrm_Main_Web;

        // GET: /Hre_Profile/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_ProfileAll);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        public ActionResult Hre_ProfileBasicView(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileBasic(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfilePersonal(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfilePersonalView(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileContact(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileContactView(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileAll(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Hre_ProfileAllView(Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

       
        public Hre_ProfileModel GetById(Guid? id)
        {            
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", (Guid)id);
            return result;
        }

        public Att_AttendanceTableModel GetById(Guid id, Guid cutoffId)
        {
            var model = new ProfileIdAndCutOffIdModel()
            {
                ProfileId = id,
                CutOffId = cutoffId
            };
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Att_AttendanceTable/", model);
            return result;
        }

        

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_ProfileModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/");
            
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <returns></returns>
         
        [HttpGet]
        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_ProfileAll);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_ProfileAll);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return View(result);
        }
      
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_ProfileAll);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_ProfileModel>(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());

            
        }

        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> files)
        {           
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var physicalPath = Path.Combine(Server.MapPath("~/Content/images/profiles/"), fileName);
                        file.SaveAs(physicalPath);
                    }
                    return Json(fileName);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        /// Tab Khen Thưởng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_RewardDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Reward);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Reward);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_RewardModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_RewardCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tab Kỹ Luật
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_DisciplineDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Discipline);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Discipline);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_DisciplineModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_DisciplineCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tab Người Thân
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Hre_RelativesDetail([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Relatives);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Relatives);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<IEnumerable<Hre_RelativesModel>>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_RelativesCustom/", id);
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return View();
            }
        }
        public ActionResult Hre_ProfileInsurance(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Profile);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Profile);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

    }
    
}