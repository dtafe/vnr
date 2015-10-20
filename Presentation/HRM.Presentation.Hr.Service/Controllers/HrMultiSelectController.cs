using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class HrMultiSelectController : HrmMvcController
    {
        private string userLogin = string.Empty;
        public string UserLogin 
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        //
        // GET: /HrMultiSelect/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMultiProfile(string text)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, UserLogin, ref status);
            if (get != null)
            {
                var result = get.Select(item => new Hre_ProfileMultiModel()
                {
                    ID = item.ID,
                    ProfileName = item.ProfileName,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetMultiProfileOrQuit(string text,bool isProfileQuit)
        {
            //hrm_hr_sp_get_ProfileQuit_multi
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, UserLogin, ref status);
            if (isProfileQuit)
            {
                get = service.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_ProfileQuit_multi, UserLogin, ref status);
            }
            if (get != null)
            {
                var result = get.Select(item => new Hre_ProfileMultiModel()
                {
                    ID = item.ID,
                    ProfileName = item.ProfileName,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null);            
        }

        /// <summary> Load Profile la nguoi phê duyệt </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonResult GetMultiProfileApprove(string text)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_ApproveProfile_multi, UserLogin, ref status);
            var result = get.Select(item => new Hre_ProfileMultiModel()
            {
                ID = item.ID,
                ProfileName = item.ProfileName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Load Profile la cấp trên đánh giá </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonResult GetMultiHighProfile(string text)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_HighProfile_multi, UserLogin, ref status);
            var result = get.Select(item => new Hre_ProfileMultiModel()
            {
                ID = item.ID,
                ProfileName = item.ProfileName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiProfileByCodeEmp(string text)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileCodeEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, UserLogin, ref status);
            return Json(get, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hieu.Van] 09/07/2014 - Danh sách nhân viên Nữ
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonResult GetMultiProfileFemale(string text)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var get = service.GetData<Hre_ProfileMultiEntity>(text.Trim(), ConstantSql.hrm_hr_sp_get_ProfileFemale_Multi, UserLogin, ref status);
            var result = get.Select(item => new Hre_ProfileMultiModel()
            {
                ID = item.ID,
                ProfileName = item.ProfileName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiRelation()
        {
            string status = string.Empty;
            var service = new Hre_RelativesServices();
            var get = service.GetAllUseEntity<Hre_RelativesEntity>(ref status);
            var result = get.Select(item => new Hre_RelativesModel()
            {
                ID = item.ID,
                RelativeName = item.RelativeName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiDependant(string text)
        {
            string status = string.Empty;
            var service = new Hre_DependantServices();
            var get = service.GetData<Hre_DependantEntity>(text, ConstantSql.hrm_hre_sp_get_Dependant_multi, UserLogin, ref status);
            var result = get.Select(item => new Hre_DependantMultiModel()
            {
                ID = item.ID,
                DependantName = item.DependantName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}