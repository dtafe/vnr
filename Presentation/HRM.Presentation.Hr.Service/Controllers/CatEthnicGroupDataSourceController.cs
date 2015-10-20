using HRM.Data.Category.Model;
using Kendo.Mvc.UI;
using System;
//using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category;
using System.Net.Http;
using System.Net;

namespace HRM.Presentation.Category.Service.Controllers
{
    public class CatEthnicGroupDataSourceController : Controller, IDisposable
    {
        //
        // GET: /CatEthnicGroupDataSource/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCatEthnicGroup([DataSourceRequest] DataSourceRequest request)
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var list = service.GetCatEthnicGroup();
            return Json(list.ToDataSourceResult(request));
        }

        public JsonResult GetCatEthnicGroupById([DataSourceRequest] DataSourceRequest request, Cat_EthnicGroup catEthnicGroup)
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var CatEthnicGroup = service.GetByIdCatEthnicGroup(catEthnicGroup.Id);
            return Json(CatEthnicGroup);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCatEthnicGroupTest()
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var list = service.GetCatEthnicGroup();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCatEthnicGroup(HttpRequestMessage request, Cat_EthnicGroup EthnicGroup)
        {
            var model = new Cat_EthnicGroup();
            model.Id = EthnicGroup.Id;
            model.EthnicGroupName = EthnicGroup.EthnicGroupName;
            model.EthnicGroupCode = EthnicGroup.EthnicGroupCode;
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var result = service.AddCatEthnicGroup(model);
            if (result == false) model = null;
            return Json(model);
        }
        public JsonResult UpdateCatEthnicGroup(HttpRequestMessage request, Cat_EthnicGroup EthnicGroup)
        {

            var model = new Cat_EthnicGroup();
            model.Id = EthnicGroup.Id;
            model.EthnicGroupName = EthnicGroup.EthnicGroupName;
            model.EthnicGroupCode = EthnicGroup.EthnicGroupCode;
            model.UserCreate = EthnicGroup.UserCreate;
            model.UserLockID = EthnicGroup.UserLockID;
            model.UserUpdate = EthnicGroup.UserUpdate;
            model.DateCreate = EthnicGroup.DateCreate;
            model.DateLock = EthnicGroup.DateLock;
            model.DateUpdate = EthnicGroup.DateUpdate;
            model.IPCreate = EthnicGroup.IPCreate;
            model.IPUpdate = EthnicGroup.IPUpdate;
            model.IsDelete = EthnicGroup.IsDelete;
            model.ServerCreate = EthnicGroup.ServerCreate;
            model.ServerUpdate = EthnicGroup.ServerUpdate;

            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var result = service.UpdateCatEthnicGroup(model);
            if (result == false) model = null;
            return Json(model);
        }
        public JsonResult DeleteCatEthnicGroup(HttpRequestMessage request, Cat_EthnicGroup EthnicGroup)          
        {
            Cat_EthnicGroup catEthnicGroup = new Cat_EthnicGroup();
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var result = service.DeleteCatEthnicGroup(EthnicGroup.Id);
            if (result == false) catEthnicGroup = null;
            return Json(catEthnicGroup);
        }
    }
}