using System.Web.Mvc;
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
using HRM.Presentation.Service;
using HRM.Presentation.Main.Controllers;
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_MachineOfLineController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Can_MachineOfLine/
        public ActionResult Index()
        {
            return View();
        }
 
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MachineOfLineModel>(_hrm_Can_Service, "api/Can_MachineOfLine/", selectedIds,
               ConstantPermission.Can_Canteen, DeleteType.Remove.ToString());
            //var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.DeleteSelected(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds);

        }
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_MachineOfLineModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Delete(_hrm_Can_Service, "api/Can_MachineOfLine/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_MachineOfLineModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_MachineOfLineModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_MachineOfLine/", model);
                //ViewBag.MsgInsert = ConstantDisplay.HRM_Canteen_Location_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_MachineOfLineModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_MachineOfLine/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_MachineOfLineModel Can_MachineOfLine)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_MachineOfLineModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_MachineOfLine/", Can_MachineOfLine);
                //return Json(result);
                //ViewBag.MsgUpdate = ConstantDisplay.HRM_Canteen_Location_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult CreateOrUpdate(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
              //  var id1 = int.Parse(id);
                var service = new RestServiceClient<Can_MachineOfLineModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MachineOfLine/", id);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
    
}