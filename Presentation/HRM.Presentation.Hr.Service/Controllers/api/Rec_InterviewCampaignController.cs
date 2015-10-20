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
    public class Rec_InterviewCampaignController : ApiController
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
        public Rec_InterviewCampaignModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_InterviewCampaignModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_InterviewCampaignEntity>(id, ConstantSql.hrm_rec_sp_get_InterviewCampaignById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_InterviewCampaignModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public Rec_InterviewCampaignModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_InterviewCampaignEntity, Rec_InterviewCampaignModel>(id);
            return result;
        }

       
        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa Kế Hoạch Phỏng Vấn (Rec_InterviewCampaign)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_InterviewCampaign")]
        public Rec_InterviewCampaignModel Post([Bind]Rec_InterviewCampaignModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_InterviewCampaignModel>(model, "Rec_InterviewCampaign", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            var result = service.UpdateOrCreate<Rec_InterviewCampaignEntity, Rec_InterviewCampaignModel>(model);
            if (model.listCandidateGuidIds != null)
            {
                List<Guid> listId = model.listCandidateGuidIds;
                Rec_InterviewCampaignDetailEntity OjbInterviewDetail = null;
                Rec_InterviewCampaignDetailServices InterviewCampaignDetailService = new Rec_InterviewCampaignDetailServices();
                string status = string.Empty;
                foreach (Guid item in listId)
                {
                    var RecruitmentHisService = new Rec_RecruitmentHistoryServices(); 
                    var ilistRecruitmentHistory = RecruitmentHisService.GetData<Rec_RecruitmentHistoryEntity>(item, ConstantSql.hrm_rec_sp_get_RecruitmentHistoryIdByCandidateId, UserLogin,ref status).ToList();
                    Guid? RecruitmentHistoryId = ilistRecruitmentHistory.Where(s => s.Status != HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString() 
                                                                               && s.Status != HRM.Infrastructure.Utilities.Interview.E_PASS.ToString() && s.Status != null).Select(s => s.ID).FirstOrDefault();
                    OjbInterviewDetail = new Rec_InterviewCampaignDetailEntity();
                    OjbInterviewDetail.InterviewCampaignID = result.ID;
                    OjbInterviewDetail.CandidateID = item;
                    OjbInterviewDetail.DateInterview = model.DateInterviewFrom;
                    OjbInterviewDetail.RecruitmentHistoryID = RecruitmentHistoryId;
                    OjbInterviewDetail.LevelInterview = model.LevelInterview;
                    InterviewCampaignDetailService.Add(OjbInterviewDetail);
                }
            }
            return result;
        }
    }
}