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
    public class Cat_RewardedTypeController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Loại Khen Thưởng (Cat_RewardedType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_RewardedTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_RewardedTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_RewardedTypeEntity>(id, ConstantSql.hrm_cat_sp_get_RewardedTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_RewardedTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Loại Khen Thưởng (Cat_RewardedType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_RewardedTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_RewardedTypeEntity, Cat_RewardedTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một  Loại Khen Thưởng (Cat_RewardedType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_RewardedType")]
        public Cat_RewardedTypeModel Post([Bind]Cat_RewardedTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_RewardedTypeModel>(model, "Cat_RewardedType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_RewardedTypeEntity, Cat_RewardedTypeModel>(model);
        }
    }
}
