using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ConditionalColorController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu màu (Cat_ConditionalColor) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_ConditionalColorModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ConditionalColorModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ConditionalColorEntity>(id, ConstantSql.hrm_cat_sp_get_Cat_ConditionalColorById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_ConditionalColorModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của màu(Cat_ConditionalColor) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_ConditionalColorModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ConditionalColorEntity, Cat_ConditionalColorModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một màu(Cat_ConditionalColor)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ConditionalColor")]
        public Cat_ConditionalColorModel Post([Bind]Cat_ConditionalColorModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ConditionalColorModel>(model, "Cat_ConditionalColor", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ConditionalColorEntity, Cat_ConditionalColorModel>(model);
        }
    }
}
