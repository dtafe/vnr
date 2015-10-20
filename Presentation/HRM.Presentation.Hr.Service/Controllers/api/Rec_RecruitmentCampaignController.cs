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
    public class Rec_RecruitmentCampaignController : ApiController
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
        public Rec_RecruitmentCampaignModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_RecruitmentCampaignModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_RecruitmentCampaignEntity>(id, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_RecruitmentCampaignModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_RecruitmentCampaignModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_RecruitmentCampaign")]
        public Rec_RecruitmentCampaignModel Post([Bind]Rec_RecruitmentCampaignModel model)
        {
            if (model.ID == Guid.Empty)
            {
                model.IsActivate = false;
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_RecruitmentCampaignModel>(model, "Rec_RecruitmentCampaign", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignModel>(model);
        }
	}
}