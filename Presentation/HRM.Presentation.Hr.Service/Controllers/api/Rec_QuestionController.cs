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
    public class Rec_QuestionController : ApiController
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
        public Rec_QuestionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_QuestionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_QuestionEntity>(id, ConstantSql.hrm_rec_sp_get_QuestionById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_QuestionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_QuestionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_QuestionEntity, Rec_QuestionModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_Question")]
        public Rec_QuestionModel Post([Bind]Rec_QuestionModel model)
        {
            #region Validate
            string message = string.Empty;
            model.Position = 0;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_QuestionModel>(model, "Rec_Question", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_QuestionEntity, Rec_QuestionModel>(model);
        }
	}
}