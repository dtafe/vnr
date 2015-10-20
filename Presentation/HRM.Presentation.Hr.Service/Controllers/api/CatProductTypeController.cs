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
    public class CatProductTypeController : ApiController
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
        /// [Chuc.Nguyen] - Lấy dữ liệu Ngân Hàng(Cat_Bank) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatProductTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatProductTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ProductTypeEntity>(id, ConstantSql.hrm_cat_sp_get_ProductTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<CatProductTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Cat_Bank) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatProductTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ProductTypeEntity, CatProductTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Cat_Bank)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatProductType")]
        public CatProductTypeModel Post([Bind]CatProductTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatProductTypeModel>(model, "Cat_ProductType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ProductTypeEntity, CatProductTypeModel>(model);
        }
    }
}
