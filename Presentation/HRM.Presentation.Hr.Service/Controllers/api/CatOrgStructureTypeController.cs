using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using System;
using VnResource.Helper.Data;
using System.Linq;
using HRM.Business.Category.Models;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatOrgStructureTypeController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu OrgStructureType  (Cat_OrgStructureType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOrgStructureTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatOrgStructureTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_OrgStructureTypeEntity>(id,ConstantSql.hrm_cat_sp_get_OrgStructureTypeById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatOrgStructureTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của OrgStructureType(Cat_OrgStructureType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatOrgStructureTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_OrgStructureTypeEntity, CatOrgStructureTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một OrgStructureType(Cat_OrgStructureType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatOrgStructureType")]
        public CatOrgStructureTypeModel Post([Bind]CatOrgStructureTypeModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatOrgStructureTypeModel>(model, "Cat_OrgStructureType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_OrgStructureTypeEntity, CatOrgStructureTypeModel>(model);
        }
    }
}