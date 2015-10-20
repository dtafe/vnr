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
    public class Cat_AppendixContractTypeController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Loại phụ lục hợp đồng (Cat_AppendixContractType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_AppendixContractTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_AppendixContractTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_AppendixContractTypeEntity>(id, ConstantSql.hrm_cat_sp_get_AppendixContractTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_AppendixContractTypeModel>();
            }
            model.ActionStatus= status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Loại phụ lục hợp đồng (Cat_AppendixContractType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_AppendixContractTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_AppendixContractTypeEntity, Cat_AppendixContractTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một  Loại phụ lục hợp đồng (Cat_AppendixContractType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_AppendixContractType")]
        public Cat_AppendixContractTypeModel Post([Bind]Cat_AppendixContractTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_AppendixContractTypeModel>(model, "Cat_AppendixContractType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_AppendixContractTypeEntity, Cat_AppendixContractTypeModel>(model);
        }
    }
}
