using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Payroll.Models;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_InsuranceConfigController : ApiController
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
        public Cat_InsuranceConfigModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_InsuranceConfigModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_InsuranceConfigEntity>(id, ConstantSql.hrm_cat_sp_get_InsuranceConfigById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_InsuranceConfigModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Cat_Bank) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_InsuranceConfigModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_InsuranceConfigEntity, Cat_InsuranceConfigModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Cat_Bank)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_InsuranceConfig")]
        public Cat_InsuranceConfigModel Post([Bind]Cat_InsuranceConfigModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_InsuranceConfigModel>(model, "Cat_InsuranceConfig", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_InsuranceConfigEntity, Cat_InsuranceConfigModel>(model);
        }
    }
}
