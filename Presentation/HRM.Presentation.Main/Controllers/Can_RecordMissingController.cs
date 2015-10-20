using System.Web.Mvc;
using HRM.Presentation.Canteen.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_RecordMissingController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Can_BackPay/
        public ActionResult Index()
        {


            #region [Hien.Nguyen] get data for selectbox edit inline on gird
            var service = new RestServiceClient<IEnumerable<Cat_TAMScanReasonMissMuitlModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Can_Service);
            var lstTamScanReasonMiss = service.Get(_hrm_Can_Service, "api/Can_MealRecordMissing/");
            ViewData["TamScanReasonMiss"] = lstTamScanReasonMiss;

            var serviceMeal = new RestServiceClient<IEnumerable<Can_MealAllowanceTypeSettingMultiModel>>(UserLogin);
            serviceMeal.SetCookies(Request.Cookies, _hrm_Can_Service);
            var lstMeal = serviceMeal.Get(_hrm_Can_Service, "api/Can_MealAllowanceTypeSetting/");
            List<Can_MealAllowanceTypeSettingMultiModel> lst;
            if (lstMeal == null)
                lst = new List<Can_MealAllowanceTypeSettingMultiModel>();
            else
                lst = lstMeal.ToList();
            lst.Insert(0, new Can_MealAllowanceTypeSettingMultiModel() { ID = Guid.Empty, MealAllowanceTypeSettingName = "- Tất Cả -" });
            ViewData["MealAllowanceTypeSetting"] = lst; 
            #endregion
            
            
            return View();
        }

        public ActionResult RecordMissingInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);

                var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealRecordMissing/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_MealRecordMissingModel model)
        {
          
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                if (model.MealAllowanceTypeSettingID == Guid.Empty)
                    model.MealAllowanceTypeSettingID = null;
                var result = service.Post(_hrm_Can_Service, "api/Can_MealRecordMissing/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_Position_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_MealRecordMissing/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_MealRecordMissingModel Can_RecordMissing)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_MealRecordMissing/", Can_RecordMissing);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Position_UpdateSuccess.TranslateString();
            }
            return View();
        }

        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Remove(_hrm_Can_Service, "api/Can_MealRecordMissing/", id);
            return Json(result);
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MealRecordMissingModel>(_hrm_Can_Service, "api/Can_MealRecordMissing/", selectedIds, ConstantPermission.Can_MealRecordMissing, DeleteType.Remove.ToString());
        }


    }

}