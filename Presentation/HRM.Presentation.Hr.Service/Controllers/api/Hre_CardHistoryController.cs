using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_CardHistoryController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Lịch Sử Thẻ(Hre_CardHistory) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_CardHistoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_CardHistoryModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_CardHistoryEntity>(id, ConstantSql.hrm_hr_sp_get_CardHistoryById, ref status);
            var entity = service.GetData<Hre_CardHistoryEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_CardHistoryById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_CardHistoryModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Lịch Sử Thẻ(Hre_CardHistory) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_CardHistoryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_CardHistoryEntity, Hre_CardHistoryModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Lịch Sử Thẻ(Hre_CardHistory)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_CardHistory")]
        public Hre_CardHistoryModel Post([Bind]Hre_CardHistoryModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_CardHistoryEntity, Hre_CardHistoryModel>(model);
        }
    }
}
