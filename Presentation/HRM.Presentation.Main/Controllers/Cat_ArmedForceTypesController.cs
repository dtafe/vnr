﻿using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ArmedForceTypesController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Cat_ArmedForceTypesInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_ArmedForceTypesModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_ArmedForceTypes/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_ArmedForceTypesModel>(_hrm_Hr_Service, "api/Cat_ArmedForceTypes/", selectedIds, ConstantPermission.Cat_ArmedForceTypes, DeleteType.Remove.ToString());
        }
    }
}