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

    public class Cat_EthnicGroupController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatEthnicGroup/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult CatEthnicGroupInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatEthnicGroupModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatEthnicGroup/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatEthnicGroup
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatEthnicGroup([DataSourceRequest] DataSourceRequest request)
        
        {
            var service = new RestServiceClient<IEnumerable<CatEthnicGroupModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatEthnicGroup/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CatEthnicGroup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatEthnicGroupModel model)
        {
           
            var service = new RestServiceClient<CatEthnicGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatEthnicGroup/", model);
            return Json(result);
        }

        /// <summary>
        /// Form tạo mới
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult CreateCatEthnicGroup(CatEthnicGroupModel CatEthnicGroup)
        {
          
            CatEthnicGroupModel catEthnicGroupmodel = null;
            if (ModelState.IsValid)
            {
                var client = CreateWebClient();
                string url = _hrm_Hr_Service + "CatEthnicGroupDataSource/AddCatEthnicGroup";
                string method = "POST";
                var json = new JavaScriptSerializer().Serialize(CatEthnicGroup);
                client.Headers.Add("Content-Type", "application/json");
                var response = client.UploadString(url, method, json);
                catEthnicGroupmodel = JsonConvert.DeserializeObject<CatEthnicGroupModel>(response);
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
        public ActionResult Update([Bind] CatEthnicGroupModel catEthnicGroup)
        {
            
            var service = new RestServiceClient<CatEthnicGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatEthnicGroup/", catEthnicGroup);
            return Json(result);
        }

        public ActionResult Delete(Guid id)
        {
           
            var service = new RestServiceClient<CatEthnicGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatEthnicGroup/", id);
            return Json(result);
        }
        public ActionResult Edit(Guid id)
        {
            
            CatEthnicGroupModel catEthnicGroupmodel = new CatEthnicGroupModel();
            catEthnicGroupmodel.ID = id;
            var client = CreateWebClient();
            string url = _hrm_Hr_Service + "CatEthnicGroupDataSource/GetCatEthnicGroupById";
            string method = "POST";
            var json = new JavaScriptSerializer().Serialize(catEthnicGroupmodel);
            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(url, method, json);
            CatEthnicGroupModel catEthnicGroup = new CatEthnicGroupModel();
            catEthnicGroup = JsonConvert.DeserializeObject<CatEthnicGroupModel>(response);

            return View(catEthnicGroup);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CatEthnicGroupModel CatEthnicGroup)
        {
            
            CatEthnicGroupModel catEthnicGroupmodel = null;
            if (ModelState.IsValid)
            {
                var client = CreateWebClient();
                string url = _hrm_Hr_Service + "CatEthnicGroupDataSource/UpdateCatEthnicGroup";
                string method = "POST";
                var json = new JavaScriptSerializer().Serialize(CatEthnicGroup);
                client.Headers.Add("Content-Type", "application/json");
                var response = client.UploadString(url, method, json);
                catEthnicGroupmodel = JsonConvert.DeserializeObject<CatEthnicGroupModel>(response);
            }
            return View();
        }
        public ActionResult CreateCatEthnicGroup()
        {
           
            return View();
        }

     

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatEthnicGroupModel>(_hrm_Hr_Service, "api/CatEthnicGroup/", selectedIds, ConstantPermission.Cat_EthnicGroup, DeleteType.Remove.ToString());

        }

    }

}