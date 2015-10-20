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
    public class Cat_AccidentTypeController : ApiController
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
        // GET: /Cat_AccidentType/

        public Cat_AccidentTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_AccidentTypeModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_AccidentTypeEntity>(id, ConstantSql.hrm_cat_sp_get_AccidentTypeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_AccidentTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>/5
      
        public Cat_AccidentTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_AccidentTypeEntity, Cat_AccidentTypeModel>(id);
            return result;
        }


        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa loại tai nạn (Cat_AccidentType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_AccidentType")]
        public Cat_AccidentTypeModel Post([Bind]Cat_AccidentTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_AccidentTypeModel>(model, "Cat_AccidentType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_AccidentTypeEntity, Cat_AccidentTypeModel>(model);
        }
	}
}