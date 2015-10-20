using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using HRM.Business.Main.Domain;
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
    public class Rec_RecruitmentHistoryController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Thông Tin Ứng Viên(Rec_RecruitmentHistory) theo Id
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
        public Rec_RecruitmentHistoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_RecruitmentHistoryModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_RecruitmentHistoryEntity>(id, ConstantSql.hrm_rec_sp_get_RecruitmentHistoryById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_RecruitmentHistoryModel>();
            }
            if (!string.IsNullOrEmpty(model.Status))
                model.Status = model.Status.TranslateString();
            model.ActionStatus = status;
            return model;
        }

        public Rec_RecruitmentHistoryModel DeleteOrRemove(string id)
        {

            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistoryModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa thông tin ứng viên (Rec_Candidate)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_RecruitmentHistory")]
        public Rec_RecruitmentHistoryModel Post([Bind]Rec_RecruitmentHistoryModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_RecruitmentHistoryModel>(model, "Rec_RecruitmentHistory", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            if (model.CandidateID != null)
            {
                string status = string.Empty;
                var candidate = service.GetData<Rec_CandidateEntity>(Common.DotNetToOracle(model.CandidateID.ToString()), ConstantSql.hrm_rec_sp_get_CandidateById, ref status).FirstOrDefault();
                model.CandidateName = candidate != null ? candidate.CandidateName : null;
            }

            return service.UpdateOrCreate<Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistoryModel>(model);
        }
    }
}