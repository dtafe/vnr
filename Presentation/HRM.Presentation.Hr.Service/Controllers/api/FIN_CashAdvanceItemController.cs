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
    public class FIN_CashAdvanceItemController : ApiController
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
        public FIN_CashAdvanceItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_CashAdvanceItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Fin_CashAdvanceItemEntity>(id, ConstantSql.hrm_hr_sp_get_CashAdvanceItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_CashAdvanceItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FIN_CashAdvanceItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Fin_CashAdvanceItemEntity, FIN_CashAdvanceItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Fin_Claim")]
        public FIN_CashAdvanceItemModel Post([Bind]FIN_CashAdvanceItemModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_CashAdvanceItemModel>(model, "FIN_CashAdvanceItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);

            return service.UpdateOrCreate<Fin_CashAdvanceItemEntity, FIN_CashAdvanceItemModel>(model);
        }
    }
}
