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
    public class Rec_RecruitmentCampaignItemController : ApiController
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
        public Rec_RecruitmentCampaignItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_RecruitmentCampaignItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_RecruitmentCampaignItemEntity>(id, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignItemById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_RecruitmentCampaignItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_RecruitmentCampaignItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_RecruitmentCampaignItemEntity, Rec_RecruitmentCampaignItemModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_RecruitmentCampaignItem")]
        public Rec_RecruitmentCampaignItemModel Post([Bind]Rec_RecruitmentCampaignItemModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_RecruitmentCampaignItemModel>(model, "Rec_RecruitmentCampaignItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_RecruitmentCampaignItemEntity, Rec_RecruitmentCampaignItemModel>(model);
        }
	}
}