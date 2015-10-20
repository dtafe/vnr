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
    public class Cat_ValueEntityController : ApiController
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
        public Cat_ValueEntityModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ValueEntityModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ValueEntityEntity>(id, ConstantSql.hrm_cat_sp_get_ValueEntityById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_ValueEntityModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        
        public Cat_ValueEntityModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ValueEntityEntity, Cat_ValueEntityModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ValueEntity")]
        public Cat_ValueEntityModel Post([Bind]Cat_ValueEntityModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ValueEntityModel>(model, "Cat_ValueEntity", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ValueEntityEntity, Cat_ValueEntityModel>(model);
        }
    }
}
