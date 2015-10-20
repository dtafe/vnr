using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatDistrictController : ApiController
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
        public CatDistrictModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatDistrictModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_DistrictEntity>(id,ConstantSql.hrm_cat_sp_get_DistrictById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatDistrictModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Category) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatDistrictModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_DistrictEntity, CatDistrictModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Cat_Category)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatDistrict")]
        public CatDistrictModel Post([Bind]CatDistrictModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatDistrictModel>(model, "Cat_District", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_DistrictEntity, CatDistrictModel>(model);
        }
    }
}