using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using System.Web.Http;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System.Web.Mvc;
using System;
using System.Linq;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_MealRecordController : ApiController
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
        /// [Hieu.Van] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_MealRecordModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_MealRecordModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Can_MealRecordEntity>(id, ConstantSql.hrm_can_get_MealRecordById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<Can_MealRecordModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_MealRecordModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_MealRecordEntity, Can_MealRecordModel>(id);
            return result;
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Can_MealRecord")]
        public Can_MealRecordModel Post([Bind]Can_MealRecordModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.UpdateOrCreate<Can_MealRecordEntity, Can_MealRecordModel>(model);
            return result;
        }
    }
}
