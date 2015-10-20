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
    public class Fin_PurchaseRequestItemController : ApiController
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
        public Fin_PurchaseRequestItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Fin_PurchaseRequestItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_PurchaseRequestItemEntity>(id, ConstantSql.hrm_hr_sp_get_PurchaseRequestItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Fin_PurchaseRequestItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Fin_PurchaseRequestItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<FIN_PurchaseRequestItemEntity, Fin_PurchaseRequestItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/FIN_PurchaseRequestItem")]
        public Fin_PurchaseRequestItemModel Post([Bind]Fin_PurchaseRequestItemModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Fin_PurchaseRequestItemModel>(model, "FIN_PurchaseRequestItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            string status = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var purchaseEntity = service.GetByIdUseStore<FIN_PurchaseRequestEntity>(model.PurchaseRequestID.Value,ConstantSql.hrm_hr_sp_get_PurchaseRequestById,ref status);
            if (purchaseEntity.CurrencyRate1 != null && purchaseEntity.CurencryRate2 != null)
            {
                model.AmountConvert = model.Amount / purchaseEntity.CurrencyRate1;
            }

            return service.UpdateOrCreate<FIN_PurchaseRequestItemEntity, Fin_PurchaseRequestItemModel>(model);
        }
    }
}
