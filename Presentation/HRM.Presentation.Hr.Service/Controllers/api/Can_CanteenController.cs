using HRM.Business.Canteen.Domain;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_CanteenController : ApiController
    {
        #region MyRegion
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
        /// [Tam.Le] - Lấy dữ liệu Canteen theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_CanteenModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_CanteenModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_CanteenEntity>(id, ConstantSql.hrm_can_sp_get_canteenbyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_CanteenModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Canteen sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_CanteenModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_CanteenEntity, Can_CanteenModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Canteen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Can_CanteenModel Post([Bind]Can_CanteenModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Can_CanteenEntity, Can_CanteenModel>(model);
        }
        
    }
}
