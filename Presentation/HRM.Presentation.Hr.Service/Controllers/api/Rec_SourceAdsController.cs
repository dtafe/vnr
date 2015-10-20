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
    public class Rec_SourceAdsController : ApiController
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
        public Rec_SourceAdsModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_SourceAdsModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_SourceAdsEntity>(id, ConstantSql.hrm_rec_sp_get_SourceAdsByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_SourceAdsModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Rec_SourceAdsServices();
            var result = service.Remove<Rec_SourceAdsEntity>(ID);
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_SourceAds")]
        public Rec_SourceAdsModel Post([Bind]Rec_SourceAdsModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_SourceAdsModel>(model, "Rec_SourceAds", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_SourceAdsEntity, Rec_SourceAdsModel>(model);
        }
    }
}