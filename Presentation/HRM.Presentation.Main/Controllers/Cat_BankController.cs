using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Hr.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Cat_BankController : MainBaseController
    {
        private readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            var userId = Session["UserId"];
           
            return View();
        }
        
        public ActionResult CatBankInfo(string id)
        {
            var userId = Session["UserId"];
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatBankModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hre_Service);
                var modelResult = service.Get(_hrm_Hre_Service, "api/CatBank/", id1);
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
            return RemoveOrDeleteAndReturn<CatBankModel>(_hrm_Hre_Service, "api/CatBank/", selectedIds, ConstantPermission.Cat_Bank, DeleteType.Remove.ToString());
        }
    }
}