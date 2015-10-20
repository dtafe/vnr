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
    /// <summary> Duyệt Thanh Toán Chi Phí</summary>
    public class FIN_ClaimCostPaymentApproveController : ApiController
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
        public FIN_ClaimCostPaymentApproveModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_ClaimCostPaymentApproveModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_ClaimCostPaymentApproveEntity>(id, ConstantSql.hrm_hr_sp_get_ClaimById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_ClaimCostPaymentApproveModel>();
                if (!string.IsNullOrEmpty(model.Other))
                {
                    model.IsOther = true;
                }
                else {
                    model.IsPaymentType = true;
                }

                if(model.IsCashAdvance != null && model.IsCashAdvance == false)
                {
                    model.IsNoneCashAdvance = true;
                }
                if (model.IsCashAdvance != null  && model.IsCashAdvance == true)
                {
                    model.IsNoneCashAdvance = false;
                }
                
                
                
            }
            model.ActionStatus = status;
            return model;
        }

      
        public FIN_ClaimCostPaymentApproveModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result1 = service.DeleteOrRemove<FIN_ClaimEntity, FIN_ClaimModel>(id);
            var result = result1.CopyData<FIN_ClaimCostPaymentApproveModel>();
            return result;
        }

       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/FIN_ClaimCostPaymentApprove")]
        public FIN_ClaimCostPaymentApproveModel Post([Bind]FIN_ClaimCostPaymentApproveModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_ClaimCostPaymentApproveModel>(model, "FIN_ClaimCostPaymentApprove", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            if(!string.IsNullOrEmpty(model.Other))
            {
                model.IsCashAdvance = null;
                model.CashAdvanceID = null;
                model.TravelRequestID = null;
            }
            if (model.IsCashAdvance != null && model.IsCashAdvance == false)
            {
                model.CashAdvanceID = null;
            }
            return service.UpdateOrCreate<FIN_ClaimCostPaymentApproveEntity, FIN_ClaimCostPaymentApproveModel>(model);
        }
    }
}
