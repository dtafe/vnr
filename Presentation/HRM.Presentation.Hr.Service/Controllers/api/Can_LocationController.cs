using HRM.Business.Canteen.Domain;
using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_LocationController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu Location theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_LocationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_LocationModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_LocationEntity>(id, ConstantSql.hrm_can_sp_get_locationbyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_LocationModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Location sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_LocationModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_LocationEntity, Can_LocationModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Location
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Can_LocationModel Post([Bind]Can_LocationModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Can_LocationEntity, Can_LocationModel>(model);
        }
     
    }
}
