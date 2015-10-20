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
    
    public class Rec_CandidateController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Thông Tin Ứng Viên(Rec_Candidate) theo Id
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

        public Rec_CandidateModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_CandidateModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_CandidateEntity>(id, ConstantSql.hrm_rec_sp_get_CandidateById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_CandidateModel>();
            }
            if (!string.IsNullOrEmpty(model.Status))
                model.Status = model.Status.TranslateString();
            model.ActionStatus = status;
            return model;
        }

        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Rec_CandidateServices();
            var result = service.Remove<Rec_CandidateEntity>(ID);
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa thông tin ứng viên (Rec_Candidate)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_Candidate")]
        public Rec_CandidateModel Post([Bind]Rec_CandidateModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_CandidateModel>(model, "Rec_Candidate", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
           
            if(model != null && model.DateExpiresPassport !=null && model.DateIssuePassport != null)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_CandidateModel>(model, "Rec_CandidateCheckDate", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            Rec_RecruitmentHistoryServices recruimentHistoryService = new Rec_RecruitmentHistoryServices();
            #region Check trùng và xử lý lưu or edit lại dòng cũ
            BaseService baseservice = new BaseService();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.ID);
            lstObj.Add(model.IdentifyNumber);
            lstObj.Add(model.CandidateName);
            lstObj.Add(model.DateOfBirth);
            var candidate = baseservice.GetData<Rec_CandidateModel>(lstObj, ConstantSql.hrm_rec_sp_checkduplidatecandidate, UserLogin,ref status).FirstOrDefault();
            if (candidate != null)
            {
                model.PassFilterResume = null;
                model.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_NEW.ToString();
                model.ID = candidate.ID;
            }

            // Làm theo task 0048666 - edit mà thay đổi ngày nộp thì update trạng thái = null - task 0049801 sửa lại thành E_NEW
            if (model.ID != Guid.Empty)
            {
                var candidateEntity = baseservice.GetData<Rec_CandidateEntity>(Common.DotNetToOracle(model.ID.ToString()), ConstantSql.hrm_rec_sp_get_CandidateById, UserLogin, ref status).FirstOrDefault();
                
                if (candidateEntity != null && candidateEntity.DateApply != model.DateApply && candidateEntity.Status != model.Status)
                {
                    model.Status = model.Status;
                }
                if (candidateEntity != null && candidateEntity.DateApply == model.DateApply && candidateEntity.Status != model.Status)
                {
                    model.Status = model.Status;
                }
                if (candidateEntity != null && candidateEntity.DateApply != model.DateApply && model.Status == candidateEntity.Status)
                {
                    model.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_NEW.ToString();
                }
            }

            // theo task 0049799 - tạo mới trạng thái e_new
            if(model.ID == Guid.Empty)
            {
                model.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_NEW.ToString();
            }

            var userCreateID = Guid.Empty;
            Guid.TryParse(model.UserCreateID, out userCreateID);

            var candidateModel = service.UpdateOrCreate<Rec_CandidateEntity, Rec_CandidateModel>(model, userCreateID);
            #region Xử lý lưu lịch sử ứng viên
            DoActionAfterImport doActionImport = new DoActionAfterImport();
            List<Guid> lstID = new List<Guid>();
            lstID.Add(model.ID);
            doActionImport.ImportRecruitmentHistory(lstID,UserLogin); 
            #endregion
            #endregion
            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_Candidate")]
        public Rec_CandidateModel Put([Bind]Rec_CandidateModel model)
        {
            model.ProfileIds = Common.DotNetToOracle(model.ProfileIds);
            BaseService service = new BaseService();
            string status = string.Empty;
            var lstCheck = service.GetData<Rec_CandidateModel>(model.ProfileIds, ConstantSql.hrm_rec_sp_get_CandidateByIds, UserLogin, ref status);
            List<object> lstObj = new List<object>();
            lstObj.Add(model.ProfileIds);
            lstObj.Add(model.Status);
            var rs = service.UpdateData<Rec_CandidateModel>(lstObj, ConstantSql.hrm_rec_sp_Set_Candidate_Status, ref status);
            if (rs != null)
            {
                return model;
            }
            return null;
        }
    }
}