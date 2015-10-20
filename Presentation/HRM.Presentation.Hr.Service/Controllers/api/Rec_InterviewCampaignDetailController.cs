using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
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
    public class Rec_InterviewCampaignDetailController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Kế Hoạch Phỏng Vấn (Rec_InterviewCampaign) theo Id
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
        public Rec_InterviewCampaignDetailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_InterviewCampaignDetailModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_InterviewCampaignDetailEntity>(id, ConstantSql.hrm_rec_sp_get_InterviewCampaignById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_InterviewCampaignDetailModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Rec_InterviewCampaignDetailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_InterviewCampaignDetailEntity, Rec_InterviewCampaignDetailModel>(id);
            return result;
        }

        public Rec_InterviewCampaignDetailModel Post([Bind]Rec_InterviewCampaignDetailModel model)
        {
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            if (model.InterviewCampaignID != null)
            {
                var interviewcampaign = service.GetData<Rec_InterviewCampaignEntity>(Common.DotNetToOracle(model.InterviewCampaignID.ToString()), ConstantSql.hrm_rec_sp_get_InterviewCampaignById, ref status).FirstOrDefault();
                if (interviewcampaign != null)
                {
                    model.DateFrom = interviewcampaign.DateInterviewFrom;
                    model.DateTo = interviewcampaign.DateInterviewTo;
                    model.LevelInterview = interviewcampaign.LevelInterview;
                }
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_InterviewCampaignDetailModel>(model, "Rec_InterviewCampaignDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
           
            return service.UpdateOrCreate<Rec_InterviewCampaignDetailEntity, Rec_InterviewCampaignDetailModel>(model);
        }
	}
}