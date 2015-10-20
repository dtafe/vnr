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
    public class Pur_MCAMController : ApiController
    {
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
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Pur_MCAM")]
        public Pur_MCAMModel Post([Bind]Pur_MCAMModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Pur_MCAMModel>(model, "Pur_MCAM_CreateOrUpdate", "Pur_MCAM", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Pur_MCAMEntity, Pur_MCAMModel>(model);
        }
        public Pur_MCAMModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Pur_MCAMEntity, Pur_MCAMModel>(id);
            return result;
        }
        public Pur_MCAMModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Pur_MCAMModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Pur_MCAMEntity>(id, ConstantSql.Hrm_CAT_SP_GET_PURMCAMBYID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Pur_MCAMModel>();
            }
            model.ActionStatus = status;
            return model;
        }
	}
}