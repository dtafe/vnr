using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Recruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Rec_QuestionBankController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_QuestionBank/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult QuestionBankInfo(string id)
        {
          
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_QuestionBankModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_QuestionBank/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_QuestionBankModel>(_Hrm_Hre_Service, "api/Rec_QuestionBank/", selectedIds, ConstantPermission.Rec_QuestionBank, DeleteType.Remove.ToString());
        }
	}
}