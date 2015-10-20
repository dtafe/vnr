using HRM.Business.Category.Domain;

using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatEthnicGroupController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Dân Tộc(Cat_EthnicGroup) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatEthnicGroupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatEthnicGroupModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_EthnicGroupEntity>(id,ConstantSql.hrm_cat_sp_get_EthnicGroupById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatEthnicGroupModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Dân Tộc(Cat_EthnicGroup) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatEthnicGroupModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_EthnicGroupEntity, CatEthnicGroupModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Dân Tộc(Cat_EthnicGroup)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatEthnicGroup")]
        public CatEthnicGroupModel Post([Bind]CatEthnicGroupModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatEthnicGroupModel>(model, "Cat_EthnicGroup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_EthnicGroupEntity, CatEthnicGroupModel>(model);
        }
    }
}
