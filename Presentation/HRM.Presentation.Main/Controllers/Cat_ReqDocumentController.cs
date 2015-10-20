using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ReqDocumentController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        private readonly string _Hrm_Eva_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
          
            return View();
        }

        public ActionResult Cat_ReqDocumentInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_ReqDocumentModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_ReqDocument/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_ReqDocumentModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_ReqDocument/", id1);
                if (modelResult != null)
                    modelResult.ID = modelResult.ID;
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

        /// <summary>
        /// [Chuc.Nguyen] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_ReqDocumentModel>(_hrm_Hr_Service, "api/Cat_ReqDocument/", selectedIds, ConstantPermission.Cat_ReqDocument, DeleteType.Remove.ToString());
        }
    }
}