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
    public class FIN_TravelRequestController : ApiController
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
        public FIN_TravelRequestModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_TravelRequestModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_TravelRequestEntity>(id, ConstantSql.hrm_hr_sp_get_TravelRequestById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_TravelRequestModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FIN_TravelRequestModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<FIN_TravelRequestEntity, FIN_TravelRequestModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Fin_TravelRequest")]
        public FIN_TravelRequestModel Post([Bind]FIN_TravelRequestModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_TravelRequestModel>(model, "FIN_TravelRequest", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<FIN_TravelRequestEntity, FIN_TravelRequestModel>(model);
        }
    }
}
