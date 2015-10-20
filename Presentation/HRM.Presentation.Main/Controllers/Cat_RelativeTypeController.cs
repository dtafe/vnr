using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;

//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_RelativeTypeController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatRelativeType/
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult CatRelativeTypeInfo(string id)
        {
        

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatRelativeTypeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatRelativeType/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatRelativeType
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatRelativeType([DataSourceRequest] DataSourceRequest request)
        
        {
            var service = new RestServiceClient<IEnumerable<CatRelativeTypeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatRelativeType/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CatRelativeType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatRelativeTypeModel model)
        {
   
            var service = new RestServiceClient<CatRelativeTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatRelativeType/", model);
            return Json(result);
        }

        /// <summary>
        /// Form tạo mới
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult CreateCatRelativeType(CatRelativeTypeModel CatRelativeType)
        {

            CatRelativeTypeModel catRelativeTypemodel = null;
            if (ModelState.IsValid)
            {
                var client = CreateWebClient();
                string url = _hrm_Hr_Service + "CatRelativeTypeDataSource/AddCatRelativeType";
                string method = "POST";
                var json = new JavaScriptSerializer().Serialize(CatRelativeType);
                client.Headers.Add("Content-Type", "application/json");
                var response = client.UploadString(url, method, json);
                catRelativeTypemodel = JsonConvert.DeserializeObject<CatRelativeTypeModel>(response);
                //ModelState.AddModelError("Error", "Success");
                ViewBag.MsgInsert = "Insert success";
            }

            return View();
        }

        private WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            const string creds = "jbob" + ":" + "jbob12345";
            var bcreds = Encoding.ASCII.GetBytes(creds);
            var base64Creds = Convert.ToBase64String(bcreds);
            webClient.Headers.Add("Authorization", "Basic " + base64Creds);
            return webClient;
        }

        [HttpPost]
        public ActionResult Update([Bind] CatRelativeTypeModel catRelativeType)
        {

            var service = new RestServiceClient<CatRelativeTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatRelativeType/", catRelativeType);
            return Json(result);
        }

        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<CatRelativeTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatRelativeType/", id);
            return Json(result);
        }
        public ActionResult Edit(Guid id)
        {

            CatRelativeTypeModel catRelativeTypemodel = new CatRelativeTypeModel();
            catRelativeTypemodel.ID = id;
            var client = CreateWebClient();
            string url = _hrm_Hr_Service + "CatRelativeTypeDataSource/GetCatRelativeTypeById";
            string method = "POST";
            var json = new JavaScriptSerializer().Serialize(catRelativeTypemodel);
            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(url, method, json);
            CatRelativeTypeModel catRelativeType = new CatRelativeTypeModel();
            catRelativeType = JsonConvert.DeserializeObject<CatRelativeTypeModel>(response);

            return View(catRelativeType);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CatRelativeTypeModel CatRelativeType)
        {

            CatRelativeTypeModel catRelativeTypemodel = null;
            if (ModelState.IsValid)
            {
                var client = CreateWebClient();
                string url = _hrm_Hr_Service + "CatRelativeTypeDataSource/UpdateCatRelativeType";
                string method = "POST";
                var json = new JavaScriptSerializer().Serialize(CatRelativeType);
                client.Headers.Add("Content-Type", "application/json");
                var response = client.UploadString(url, method, json);
                catRelativeTypemodel = JsonConvert.DeserializeObject<CatRelativeTypeModel>(response);
            }
            return View();
        }
        public ActionResult CreateCatRelativeType()
        {
  
            return View();
        }

        #region MyRegion
        //public ActionResult RemoveSelected(string selectedIds)
        //{
        //    var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Cat_RelativeType);
        //    if (!isAccess)
        //    {
        //        return PartialView("AccessDenied");
        //    }
        //    var workDay = new List<CatRelativeTypeModel>(UserLogin);
        //    if (selectedIds != null)
        //    {
        //        var ids = selectedIds
        //            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => Convert.ToInt32(x))
        //            .ToArray();
        //        var service = new RestServiceClient<CatRelativeTypeModel>(UserLogin);
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        foreach (var id in ids)
        //        {
        //            service.Delete(_hrm_Hr_Service, "api/CatRelativeType/", id);
        //        }
        //    }
        //    return Json("");
        //} 
        #endregion
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatRelativeTypeModel>(_hrm_Hr_Service, "api/CatRelativeType/", selectedIds,
                ConstantPermission.Cat_RelativeType, DeleteType.Remove.ToString());
        }
    }

}