using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_OvertimeResonController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu ResignReason(Cat_ResignReason) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_OvertimeResonModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_OvertimeResonModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_OvertimeResonEntity>(id, ConstantSql.hrm_cat_sp_get_OvertimeReasonById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_OvertimeResonModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của ResignReason(Cat_ResignReason) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_OvertimeResonModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_OvertimeResonEntity, Cat_OvertimeResonModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một ResignReason(Cat_ResignReason)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatResignReason")]
        public Cat_OvertimeResonModel Post([Bind]Cat_OvertimeResonModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_OvertimeResonModel>(model, "Cat_OvertimeReson", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_OvertimeResonEntity, Cat_OvertimeResonModel>(model);
        }

    }


}