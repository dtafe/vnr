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
    public class Cat_UsualAllowanceController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Trợ Cấp( Cat_UsualAllowance) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UsualAllowanceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_UsualAllowanceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_UsualAllowanceEntity>(id, ConstantSql.hrm_cat_sp_get_UsualAllowanceById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_UsualAllowanceModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Trợ Cấp( Cat_UsualAllowance) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UsualAllowanceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_UsualAllowanceEntity, Cat_UsualAllowanceModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một Trợ Cấp( Cat_UsualAllowance)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_UsualAllowance")]
        public Cat_UsualAllowanceModel Post([Bind]Cat_UsualAllowanceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UsualAllowanceModel>(model, "Cat_UsualAllowance", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UsualAllowanceEntity, Cat_UsualAllowanceModel>(model);
        }
    }
}
