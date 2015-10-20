using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Web.Mvc;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_UniformController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Uniform(Hre_Uniform) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_UniformModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_UniformModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_UniformEntity>(id, ConstantSql.hrm_hr_sp_get_UniformById, ref status);
            var entity = service.GetData<Hre_UniformEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_UniformById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_UniformModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Uniform(Hre_Uniform) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_UniformModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_UniformEntity, Hre_UniformModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Uniform(Hre_Uniform)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Uniform")]
        public Hre_UniformModel Post([Bind]Hre_UniformModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_UniformModel>(model, "Hre_Uniform", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_UniformEntity, Hre_UniformModel>(model);
        }
    }
}
