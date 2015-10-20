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
    public class Can_BackPayController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu BackPay theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_BackPayModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_BackPayModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_BackPayEntity>(id, ConstantSql.hrm_can_sp_get_backpaybyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_BackPayModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của BackPay sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_BackPayModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_BackPayEntity, Can_BackPayModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một BackPay
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Can_BackPayModel Post([Bind]Can_BackPayModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.UpdateOrCreate<Can_BackPayEntity, Can_BackPayModel>(model);
            return result;
        }
    }
}
