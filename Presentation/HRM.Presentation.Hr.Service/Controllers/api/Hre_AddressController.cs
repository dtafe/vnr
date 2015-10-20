using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Web.Mvc;
using HRM.Business.Hr.Models;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_AddressController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Địa Chỉ(Hre_Address) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AddressModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_AddressModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_AddressEntity>(id, ConstantSql.hrm_hr_sp_get_AddressById, ref status);
            var entity = service.GetData<Hre_AddressEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_AddressById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_AddressModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Địa Chỉ(Hre_Address) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AddressModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_AddressEntity, Hre_AddressModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Địa Chỉ(Hre_Address)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Address")]
        public Hre_AddressModel Post([Bind]Hre_AddressModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_AddressEntity, Hre_AddressModel>(model);
        }
    }
}
