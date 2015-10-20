using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_RateInsuranceController : ApiController
    {
        #region UserLogin
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
        public Cat_RateInsuranceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_RateInsuranceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_RateInsuranceEntity>(id, ConstantSql.hrm_cat_sp_get_RateInsuranceById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_RateInsuranceModel>();

                #region nhân 100 cho các tỉ lệ (để hiển thị %)
                if (model != null)
                {
                    model.SocialInsCompRate = model.SocialInsCompRate*100 ;
                    model.SocialInsEmpRate = model.SocialInsEmpRate * 100;
                    model.HealthInsCompRate = model.HealthInsCompRate * 100;
                    model.HealthInsEmpRate = model.HealthInsEmpRate * 100;
                    model.UnemployInsCompRate = model.UnemployInsCompRate * 100;
                    model.UnemployInsEmpRate = model.UnemployInsEmpRate * 100;
                }
                #endregion

            }
            model.ActionStatus = status;
            return model;
        }

        
        public Cat_RateInsuranceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_RateInsuranceEntity, Cat_RateInsuranceModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_RateInsurance")]
        public Cat_RateInsuranceModel Post([Bind]Cat_RateInsuranceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_RateInsuranceModel>(model, "Cat_RateInsurance", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);

            #region chia 100 cho các tỉ lệ (để hiển thị %)
            if (model != null)
            {
                model.SocialInsCompRate = model.SocialInsCompRate / 100;
                model.SocialInsEmpRate = model.SocialInsEmpRate / 100;
                model.HealthInsCompRate = model.HealthInsCompRate / 100;
                model.HealthInsEmpRate = model.HealthInsEmpRate / 100;
                model.UnemployInsCompRate = model.UnemployInsCompRate / 100;
                model.UnemployInsEmpRate = model.UnemployInsEmpRate / 100;
            }
            #endregion

            return service.UpdateOrCreate<Cat_RateInsuranceEntity, Cat_RateInsuranceModel>(model);
        }
    }
}
