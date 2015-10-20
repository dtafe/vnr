using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_UsualAllowanceLevelController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Mức Độ Trợ Cấp( Cat_UsualAllowanceLevel) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UsualAllowanceLevelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_UsualAllowanceLevelModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_UsualAllowanceLevelEntity>(id, ConstantSql.hrm_cat_sp_get_UsualAllowanceLevelById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_UsualAllowanceLevelModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Mức Độ Trợ Cấp( Cat_UsualAllowanceLevel) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UsualAllowanceLevelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_UsualAllowanceLevelEntity, Cat_UsualAllowanceLevelModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một Mức Độ Trợ Cấp( Cat_UsualAllowanceLevel)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_UsualAllowanceLevel")]
        public Cat_UsualAllowanceLevelModel Post([Bind]Cat_UsualAllowanceLevelModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UsualAllowanceLevelModel>(model, "Cat_UsualAllowanceLevel", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UsualAllowanceLevelEntity, Cat_UsualAllowanceLevelModel>(model);
        }
    }
}
