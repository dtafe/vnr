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
    public class Cat_MasterDataGroupController : ApiController
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
        /// <summary>Lấy dữ liệuMasterDataGroup</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_MasterDataGroupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_MasterDataGroupModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_MasterDataGroupEntity>(id, ConstantSql.hrm_cat_sp_get_MasterDataGroupid, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_MasterDataGroupModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>Xóa hoặc chuyển đổi trạng thái (Cat_MasterDataGroup) sang IsDelete</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_MasterDataGroupModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_MasterDataGroupEntity, Cat_MasterDataGroupModel>(id);
            return result;
        }

        /// <summary>[Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Cat_Bank)</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_MasterDataGroup")]
        public Cat_MasterDataGroupModel Post([Bind]Cat_MasterDataGroupModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_MasterDataGroupModel>(model, "Cat_MasterDataGroup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_MasterDataGroupEntity, Cat_MasterDataGroupModel>(model);
        }
    }
}
