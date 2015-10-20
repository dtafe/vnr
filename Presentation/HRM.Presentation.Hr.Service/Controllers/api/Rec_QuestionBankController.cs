using HRM.Business.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Recruitment.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Rec_QuestionBankController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        public Rec_QuestionBankModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_QuestionBankModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_QuestionBankEntity>(id, ConstantSql.hrm_rec_sp_get_QuestionBankById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_QuestionBankModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_QuestionBankModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_QuestionBankEntity, Rec_QuestionBankModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_QuestionBank")]
        public Rec_QuestionBankModel Post([Bind]Rec_QuestionBankModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_QuestionBankModel>(model, "Rec_QuestionBank", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            model.ActionStatus = "Success";
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_QuestionBankEntity, Rec_QuestionBankModel>(model);
        }
	}
}