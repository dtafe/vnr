using System.Web.Mvc;
using HRM.Presentation.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace HRM.Presentation.Main.Controllers
{
    public class GeneralCandidateController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Rec_Candidate/
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Tab_Rec(Guid id)
        {
           
            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            return View(result);
        }

        public ActionResult GeneralCandidateBasic(Guid id)
        {
            
            if (id != Guid.Empty) return View();
            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            if (result.ImagePath == null)
            {
                result.ImagePath = "no_avatar.jpg";
            }
            return View(result);
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Rec_Candidate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Rec_CandidateModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([Bind] Rec_CandidateModel model)
        {

            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_Candidate/", model);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GeneralCandidateEdit(Guid id)
        {

            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult GeneralCandidateEdit([Bind] Rec_CandidateModel model)
        {

            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_Candidate/", model);
            return View(result);
        }

        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            
            bool delete = false;
            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Remove(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            return Json(result);
        }

        public ActionResult GetPartialView(string partialName)
        {
            return PartialView(partialName);
        }

    }
}