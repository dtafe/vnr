using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatRegionController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu Vùng Miền(Cat_Region) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatRegionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatRegionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_RegionEntity>(id,ConstantSql.hrm_cat_sp_get_RegionById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatRegionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Vùng Miền(Cat_Region) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatRegionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_RegionEntity, CatRegionModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Vùng Miền(Cat_Region)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatRegion")]
        public CatRegionModel Post([Bind]CatRegionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatRegionModel>(model, "Cat_Region", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_RegionEntity, CatRegionModel>(model);
        }

        
    }
}