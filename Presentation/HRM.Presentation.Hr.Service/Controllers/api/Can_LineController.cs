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
    public class Can_LineController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu Line theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_LineModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_LineModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Can_LineEntity>(id, ConstantSql.hrm_can_sp_get_LineById, ref status).FirstOrDefault(); 
            if (entity != null)
            {
                model = entity.CopyData<Can_LineModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Line sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_LineModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_LineEntity, Can_LineModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Line
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Can_LineModel Post([Bind]Can_LineModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Can_LineEntity, Can_LineModel>(model);
        }
        
    }
}
