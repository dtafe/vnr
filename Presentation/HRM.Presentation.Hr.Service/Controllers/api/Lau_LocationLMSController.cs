using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System.Collections.Generic;
using HRM.Presentation.Laundry.Models;
using HRM.Presentation.Service;
using HRM.Business.Laundry.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Lau_LocationLMSController : ApiController
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
        /// Lấy dữ liệu Location theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LMS_LocationLMSModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new LMS_LocationLMSModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<LMS_LocationLMSModel>(id, ConstantSql.hrm_lau_sp_get_locationbyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<LMS_LocationLMSModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// Xóa hoặc chuyển đổi trạng thái của Location sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LMS_LocationLMSModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<LMS_LocationLMSEntity, LMS_LocationLMSModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa một Location
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public LMS_LocationLMSModel Post([Bind]LMS_LocationLMSModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<LMS_LocationLMSEntity, LMS_LocationLMSModel>(model);
        }
     
    }
}
