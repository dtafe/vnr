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
    public class Rec_QuestionTypeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_QuestionTypeRec/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rec_QuestionTypeInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_QuestionTypeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_QuestionType/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_QuestionTypeModel>(_Hrm_Hre_Service, "api/Rec_QuestionType/", String.Join(",", selectedIds), ConstantPermission.Rec_QuestionType, DeleteType.Remove.ToString());
        }
	}
}