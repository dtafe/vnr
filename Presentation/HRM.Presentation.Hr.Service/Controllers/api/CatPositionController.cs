using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

using System;
using HRM.Business.Category.Models;
using System.Linq;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatPositionController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Position(Cat_Position) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatPositionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatPositionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_PositionEntity>(id, ConstantSql.hrm_cat_sp_get_PositionById,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatPositionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Position(Cat_Position) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatPositionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_PositionEntity, CatPositionModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Position(Cat_Position)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatPosition")]
        public CatPositionModel Post([Bind]CatPositionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatPositionModel>(model, "Cat_Position", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_PositionEntity, CatPositionModel>(model);
        }
    }
}
