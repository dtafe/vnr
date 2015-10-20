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
    public class Hre_DependantController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Dependant(Hre_Dependant) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_DependantModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_DependantModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_DependantEntity>(id, ConstantSql.hrm_hr_sp_get_DependantById, ref status);
            var entity = service.GetData<Hre_DependantEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_DependantById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_DependantModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Dependant(Hre_Dependant) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_DependantModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_DependantEntity, Hre_DependantModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Dependant(Hre_Dependant)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Dependant")]
        public Hre_DependantModel Post([Bind]Hre_DependantModel model)
        {
            #region Validate
            string status = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_DependantModel>(model, "Hre_Dependant", ref status);
            if (!checkValidate)
            {
                model.ActionStatus = status;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_DependantEntity, Hre_DependantModel>(model);
            //var entity = service.GetData<Hre_RelativesEntity>(model.IDNo, ConstantSql.hrm_hr_sp_get_DependantByIdNo, ref status);
            //if (entity != null && entity.Count > 0)
            //{
            //    model.ActionStatus += ",Số CMND đã tồn tại trong hệ thống";
            //}
            //return model;
        }

    }
}
