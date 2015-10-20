using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Main.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_SalesTypeController : ApiController
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

        public const string Eva_SalesType = "Eva_SalesType";

        public IEnumerable<Eva_SalesTypeMultiModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetDataNotParam<Eva_SalesTypeMultiModel>(ConstantSql.hrm_eva_sp_get_SalesType_multi,UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<Eva_SalesTypeMultiModel>();
                return listModel;
            }
            return new List<Eva_SalesTypeMultiModel>();
        }

        public Eva_SalesTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_SalesTypeModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_SalesTypeEntity>(id, ConstantSql.hrm_eva_sp_get_SaleTypebyID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_SalesTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_SalesTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_SalesTypeEntity, Eva_SalesTypeModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_SalesType")]
        public Eva_SalesTypeModel Post([Bind]Eva_SalesTypeModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_SalesTypeModel>(model, Eva_SalesType, ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_SalesTypeEntity, Eva_SalesTypeModel>(model);
        }
	}
}