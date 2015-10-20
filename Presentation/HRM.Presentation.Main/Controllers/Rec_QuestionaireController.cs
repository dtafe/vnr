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
    public class Rec_QuestionaireController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_Questionaire/
        public ActionResult Index()
        {
          
            return View();
        }

        public ActionResult QuestionaireInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_QuestionaireModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Questionaire/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_QuestionaireModel>(_Hrm_Hre_Service, "api/Rec_Questionaire/", selectedIds, ConstantPermission.Rec_Questionaire, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveQuestionSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_QuestionModel>(_Hrm_Hre_Service, "api/Rec_Question/", selectedIds, ConstantPermission.Rec_Question, DeleteType.Remove.ToString());
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult CreateQuestion(Guid? QuestionaireID, Guid? QuestionBankID, double Rate)

        {
            Rec_QuestionModel model = new Rec_QuestionModel();
            model.QuestionaireID = QuestionaireID;
            model.QuestionBankID = QuestionBankID;
            model.Rate = Rate;
            model.Position = 0;
            var service = new RestServiceClient<Rec_QuestionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Rec_Question/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}