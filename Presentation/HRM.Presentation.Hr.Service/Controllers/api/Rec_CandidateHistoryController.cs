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
    public class Rec_CandidateHistoryController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Lịch Sử Ứng Tuyển(Rec_CandidateHistory) theo Id
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
        public Rec_CandidateHistoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_CandidateHistoryModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_CandidateHistoryEntity>(id, ConstantSql.hrm_rec_sp_get_CandidateHistoryById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_CandidateHistoryModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        //public void Delete(Guid ID)
        //{
        //    var service = new Rec_CandidateHistoryService(); 
        //    var result = service.Remove<Rec_CandidateHistoryEntity>(ID);
        //}

        public Rec_CandidateHistoryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_CandidateHistoryEntity, Rec_CandidateHistoryModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa Lịch sử ứng tuyển (Rec_CandidateHistory)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_CandidateHistory")]
        public Rec_CandidateHistoryModel Post([Bind]Rec_CandidateHistoryModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_CandidateHistoryModel>(model, "Rec_CandidateHistory", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_CandidateHistoryEntity, Rec_CandidateHistoryModel>(model);
        }
    }
}