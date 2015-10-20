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
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Category.Web.Controllers
{
    public class Cat_GradePayrollController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Cat_GradePayroll/
        public ActionResult Index()
        {
           
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Cat_GradePayroll
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_GradePayrollModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_GradePayroll/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Cat_GradePayroll
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Cat_GradePayrollModel model)
        {
          
            var service = new RestServiceClient<Cat_GradePayrollModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_GradePayroll/", model);
            return Json(result);
        }  
       
        public ActionResult Remove(string id)
        {
          
            var service = new RestServiceClient<Cat_GradePayrollModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_GradePayroll/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
           
            
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Cat_GradePayrollModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_GradePayrollModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_GradePayroll/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_GradeAttendance_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(string id)
        {
         
            var service = new RestServiceClient<Cat_GradePayrollModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_GradePayroll/", id);            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_GradePayrollModel Cat_GradePayroll)
        {
          
            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_GradePayrollModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_GradePayroll/", Cat_GradePayroll);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_GradeAttendance_UpdateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_GradePayrollModel>(_hrm_Hr_Service, "api/Cat_GradePayroll/", selectedIds, ConstantPermission.Cat_GradePayroll,DeleteType.Remove.ToString());


        }

        [HttpPost]
        public ActionResult CreateList(List<CatElementModel> TotalCat_ElementModel, [Bind(Prefix = "models")] List<CatElementModel> updatedCat_ElementModel)
        {
            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            if (updatedCat_ElementModel != null)
            {
                for (int i = 0; i < updatedCat_ElementModel.Count; i++)
                {
                    //updatedCat_ElementModel[i].MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    service.Post(_hrm_Hr_Service, "api/CatElement/", updatedCat_ElementModel[i]);
                }
               
                
            }
            return Json(updatedCat_ElementModel);
        }

        [HttpPost]
        public ActionResult CreateAdvancePaymentList(List<CatElementModel> TotalCat_ElementModel, [Bind(Prefix = "models")] List<CatElementModel> updatedCat_ElementModel)
        {
            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            if (updatedCat_ElementModel != null)
            {
                
                for (int i = 0; i < updatedCat_ElementModel.Count; i++)
                {
                    updatedCat_ElementModel[i].MethodPayroll = MethodPayroll.E_ADNVANCE_PAYMENT.ToString();
                    service.Post(_hrm_Hr_Service, "api/CatElement/", updatedCat_ElementModel[i]);
                }


            }
            return Json(updatedCat_ElementModel);
        }

        [HttpPost]
        public ActionResult CreateInsuranceList(List<CatElementModel> TotalCat_ElementModel, [Bind(Prefix = "models")] List<CatElementModel> updatedCat_ElementModel)
        {
            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            if (updatedCat_ElementModel != null)
            {
                for (int i = 0; i < updatedCat_ElementModel.Count; i++)
                {
                    updatedCat_ElementModel[i].MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                   // updatedCat_ElementModel[i].TabType = CatElementType.Insurance.ToString();
                    service.Post(_hrm_Hr_Service, "api/CatElement/", updatedCat_ElementModel[i]);
                }


            }
            return Json(updatedCat_ElementModel);
        }

        public ActionResult CatElementFormInfo(string id)
        {
       
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatElementModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatElement/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
      
    }
    
}