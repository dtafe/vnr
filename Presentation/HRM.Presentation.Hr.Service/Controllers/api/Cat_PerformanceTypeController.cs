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
    public class Cat_PerformanceTypeController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Loại Đánh Giá( Cat_PerformanceType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_PerformanceTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_PerformanceTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_PerformanceTypeEntity>(id, ConstantSql.hrm_cat_sp_get_PerformanceTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_PerformanceTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Loại Đánh Giá( Cat_PerformanceType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_PerformanceTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_PerformanceTypeEntity, Cat_PerformanceTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một Loại Đánh Giá( Cat_PerformanceType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_PerformanceType")]
        public Cat_PerformanceTypeModel Post([Bind]Cat_PerformanceTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_PerformanceTypeModel>(model, "Cat_PerformanceType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_PerformanceTypeEntity, Cat_PerformanceTypeModel>(model);
        }
    }
}
