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

    public class Lau_LineController : ApiController
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
        /// [Quan.Nguyen] - Lấy dữ liệu Cửa nhận giặt(Lau_Line) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LineModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Lau_LineModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Lau_LineModel>(id, ConstantSql.hrm_lau_sp_get_LineById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Lau_LineModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quan.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Cửa nhận giặt(Lau_Line) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LineModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<LMS_LineLMSEntity, Lau_LineModel>(id);
            return result;
        }

        /// <summary>
        /// [Quan.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Cửa nhận giặt(Lau_Line)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Line")]
        public Lau_LineModel Post([Bind]Lau_LineModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<LMS_LineLMSEntity, Lau_LineModel>(model);
        }
    }
}