using System.Web.Http;
using HRM.Presentation.Category.Models;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatProvinceController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Thiết Bị(Cat_Category) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatProvinceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatProvinceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ProvinceEntity>(id,ConstantSql.hrm_cat_sp_get_ProvinceById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatProvinceModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Category) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatProvinceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ProvinceEntity, CatProvinceModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Cat_Category)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatProvince")]
        public CatProvinceModel Post([Bind]CatProvinceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatProvinceModel>(model, "Cat_Province", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ProvinceEntity, CatProvinceModel>(model);
        }
    }
}