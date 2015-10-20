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
    public class FIN_ClaimController : ApiController
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
        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu bảng Tai Nạn(Hre_Accident) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FIN_ClaimModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_ClaimModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_ClaimEntity>(id, ConstantSql.hrm_hr_sp_get_ClaimById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_ClaimModel>();
                if (!string.IsNullOrEmpty(model.Other))
                {
                    model.IsManufactureName = true;
                }
                else {
                    model.IsProfile = true;
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

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FIN_ClaimModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<FIN_ClaimEntity, FIN_ClaimModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Fin_Claim")]
        public FIN_ClaimModel Post([Bind]FIN_ClaimModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_ClaimModel>(model, "FIN_Claim", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            if(!string.IsNullOrEmpty(model.Other))
            {
                model.ExpensesType = null;
            }
            if (model.IsCashAdvance != null && model.IsCashAdvance == false)
            {
                model.CashAdvanceID = null;
            }
            return service.UpdateOrCreate<FIN_ClaimEntity, FIN_ClaimModel>(model);
        }
    }
}
