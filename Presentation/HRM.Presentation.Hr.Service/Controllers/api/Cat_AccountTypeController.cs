using HRM.Business.Category.Domain;
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
    public class Cat_AccountTypeController : ApiController
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
        //
        // GET: /Cat_AccountType/

        public Cat_AccountTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_AccountTypeModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_AccountTypeEntity>(id, ConstantSql.hrm_cat_sp_get_AccountTypeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_AccountTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>/5
        //public void Delete(Guid ID)
        //{
        //    var service = new Cat_AccountTypeServices();
        //    var result = service.Remove<Cat_AccountTypeEntity>(ID);
        //}
        
        public Cat_AccountTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_AccountTypeEntity, Cat_AccountTypeModel>(id);
            return result;
        }


        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa loại tài khoản(Cat_AccountType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_AccountType")]
        public Cat_AccountTypeModel Post([Bind]Cat_AccountTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_AccountTypeModel>(model, "Cat_AccountType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_AccountTypeEntity, Cat_AccountTypeModel>(model);
        }
	}
}