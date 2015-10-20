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
using System;
using HRM.Business.Finance.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class FIN_ApproveCashAdvanceController : ApiController
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
        public FIN_ApproveCashAdvanceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_ApproveCashAdvanceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Fin_ApproveCashAdvanceEntity>(id, ConstantSql.hrm_hr_sp_get_CashAdvanceById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_ApproveCashAdvanceModel>();
                if(model.TravelRequestID != null)
                {
                    model.IsTravelRequest = true;
                }
                if(!string.IsNullOrEmpty(model.Other))
                {
                    model.IsOther = true;
                }
            }
            model.ActionStatus = status;
            return model;
        }

      
        public FIN_ApproveCashAdvanceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Fin_ApproveCashAdvanceEntity, FIN_ApproveCashAdvanceModel>(id);
            return result;
        }

       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/FIN_ApproveCashAdvance")]
        public FIN_ApproveCashAdvanceModel Post([Bind]FIN_ApproveCashAdvanceModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_ApproveCashAdvanceModel>(model, "FIN_ApproveCashAdvance", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            if(!string.IsNullOrEmpty(model.Other))
            {
                model.IsEntertaiment = null;
                model.TravelRequestID = null;
            }
            if(model.IsEntertaiment == true)
            {
                model.TravelRequestID = null;
            }

            return service.UpdateOrCreate<Fin_ApproveCashAdvanceEntity, FIN_ApproveCashAdvanceModel>(model);
        }
    }
}
