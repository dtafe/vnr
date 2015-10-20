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
    public class Cat_HDTJobTypePriceController : ApiController
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
        /// Lấy dữ liệu Mức Bồi Dưỡng Công Việc Nặng Nhọc theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_HDTJobTypePriceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_HDTJobTypePriceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_HDTJobTypePriceEntity>(id, ConstantSql.hrm_cat_sp_get_HDTJobTypePriceById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_HDTJobTypePriceModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// Xóa hoặc chuyển đổi trạng thái của Mức Bồi Dưỡng Công Việc Nặng Nhọc sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_HDTJobTypePriceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_HDTJobTypePriceEntity, Cat_HDTJobTypePriceModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa một Mức Bồi Dưỡng Công Việc Nặng Nhọc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_HDTJobTypePrice")]
        public Cat_HDTJobTypePriceModel Post([Bind]Cat_HDTJobTypePriceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_HDTJobTypePriceModel>(model, "Cat_HDTJobTypePrice", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_HDTJobTypePriceEntity, Cat_HDTJobTypePriceModel>(model);
        }
    }
}
