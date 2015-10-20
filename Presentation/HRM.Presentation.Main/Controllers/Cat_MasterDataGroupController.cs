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


namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_MasterDataGroupController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Cat_MasterDataGroup/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CatMasterDataGroupInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_MasterDataGroupModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult CatMasterDataGroupForDashBoardInfo()
        {
            return View();
        }

        public ActionResult CatMasterDataGroupItemInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_MasterDataGroupItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_MasterDataGroupModel>(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", selectedIds, ConstantPermission.Cat_MasterDataGroup, DeleteType.Remove.ToString());
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Cat_MasterDataGroupModel model)
        {
            var service = new RestServiceClient<Cat_MasterDataGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {

            var service = new RestServiceClient<Cat_MasterDataGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", Common.ConvertToGuid(id));
            return Json(result);
        }

        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Cat_MasterDataGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_MasterDataGroup/", id);
            return View(result);
        }


    }
}