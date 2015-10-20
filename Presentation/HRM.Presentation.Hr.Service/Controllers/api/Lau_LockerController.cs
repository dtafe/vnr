
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Laundry.Models;
using HRM.Presentation.Service;
using HRM.Business.Laundry.Models;
using System;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Lau_LockerController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Tủ Đồ(Lau_Locker) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LockerModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Lau_LockerModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<LMS_LockerLMSEntity>(id, ConstantSql.hrm_lau_sp_get_LockerById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Lau_LockerModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tủ Đồ(Lau_Locker) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LockerModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<LMS_LockerLMSEntity, Lau_LockerModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Tủ Đồ(Lau_Locker)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Locker")]
        public Lau_LockerModel Post([Bind]Lau_LockerModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<LMS_LockerLMSEntity, Lau_LockerModel>(model);
        }
    }
}