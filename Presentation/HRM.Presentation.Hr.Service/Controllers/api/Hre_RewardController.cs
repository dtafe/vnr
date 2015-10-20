using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_RewardController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Reward(Hre_Reward) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_RewardModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_RewardModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_RewardEntity>(id, ConstantSql.hrm_hr_sp_get_RewardById, ref status);
            var entity = service.GetData<Hre_RewardEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_RewardById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_RewardModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Reward(Hre_Reward) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_RewardModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_RewardEntity, Hre_RewardModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Reward(Hre_Reward)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Reward")]
        public Hre_RewardModel Post([Bind]Hre_RewardModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_RewardModel>(model, "Hre_Reward", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_RewardEntity, Hre_RewardModel>(model);
        }
    }
}
