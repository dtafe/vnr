using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Finance.Models;
using HRM.Business.Finance.Domain;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class FIN_ApproveTravelCostController : ApiController
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
        public FIN_ApproveTravelCostModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_ApproveTravelCostModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_ApproveTravelCostEntity>(id, ConstantSql.hrm_hr_sp_get_TravelRequestById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_ApproveTravelCostModel>();
            }
            model.ActionStatus = status;
            return model;
        }
              
        public FIN_ApproveTravelCostModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result1 = service.DeleteOrRemove<FIN_TravelRequestEntity, FIN_TravelRequestModel>(id);
            var result=  result1.CopyData<FIN_ApproveTravelCostModel>();
            return result;
        }

       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/FIN_ApproveTravelRequest")]
        public FIN_ApproveTravelCostModel Post([Bind]FIN_ApproveTravelCostModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_ApproveTravelCostModel>(model, "FIN_ApproveTravelRequest", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<FIN_ApproveTravelCostEntity, FIN_ApproveTravelCostModel>(model);
        }
    }
}
