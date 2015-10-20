using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Pur_ColorController : ApiController
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
        public PUR_ColorModelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new PUR_ColorModelModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<PUR_ColorModelEntity>(id, ConstantSql.hrm_Cat_SP_GET_PurColorByID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<PUR_ColorModelModel>();
                
            }
            return model;
        }

        public PUR_ColorModelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<PUR_ColorModelEntity, PUR_ColorModelModel>(id);
            return result;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Pur_Color")]

       public PUR_ColorModelModel Post([Bind]PUR_ColorModelModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<PUR_ColorModelModel>(model, "Pur_ColorModel", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<PUR_ColorModelEntity, PUR_ColorModelModel>(model);
        }
	}
}