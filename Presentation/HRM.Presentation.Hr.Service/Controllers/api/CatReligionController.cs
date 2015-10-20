using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatReligionController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu Religion theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatReligionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatReligionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ReligionEntity>(id,ConstantSql.hrm_cat_sp_get_ReligionById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatReligionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của Religion sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatReligionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ReligionEntity, CatReligionModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một Religion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatReligion")]
        public CatReligionModel Post([Bind]CatReligionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatReligionModel>(model, "Cat_Religion", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ReligionEntity, CatReligionModel>(model);
        }
        
    }
}