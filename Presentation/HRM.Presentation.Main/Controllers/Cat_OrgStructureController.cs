using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_OrgStructureController : MainBaseController
    {

        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Cat_OrgStructure/
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult Create()
        {

            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(CatOrgStructureModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatOrgStructureModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/CatOrgStructure/", model);

                ViewBag.MsgInsert = "Insert success";
            }
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
            var service = new RestServiceClient<CatOrgStructureModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/CatOrgStructure/", id);

            ViewBag.MsgInsert = "Insert success";
            //}
            return View(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatOrgStructureModel>(_Hrm_Hre_Service, "api/CatOrgStructure/", selectedIds, ConstantPermission.Cat_OrgStructure, DeleteType.Remove.ToString());
        }
        public ActionResult Cat_OrgStructureInformation(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatOrgStructureModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/CatOrgStructure/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult OrgMoreInforContractInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_OrgMoreInforContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Cat_OrgMoreInforContract/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
}