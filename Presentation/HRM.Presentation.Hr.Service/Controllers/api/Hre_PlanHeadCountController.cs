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

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_PlanHeadCountController : ApiController
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
        public Hre_PlanHeadCountModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_PlanHeadCountModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Hre_PlanHeadCountEntity>(id, ConstantSql.hrm_hr_sp_get_PlanHeadCountById, ref status);
            //var entity = service.GetData<Hre_PlanHeadCountEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_AccidentById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_PlanHeadCountModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_PlanHeadCountModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_PlanHeadCountEntity, Hre_PlanHeadCountModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Accident")]
        public Hre_PlanHeadCountModel Post([Bind]Hre_PlanHeadCountModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_PlanHeadCountModel>(model, "Hre_PlanHeadCount", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_PlanHeadCountEntity, Hre_PlanHeadCountModel>(model);
        }
    }
}
