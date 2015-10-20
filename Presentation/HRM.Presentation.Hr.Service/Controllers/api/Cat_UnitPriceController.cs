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
    public class Cat_UnitPriceController : ApiController
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
        public Cat_UnitPriceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_UnitPriceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_UnitPriceEntity>(id, ConstantSql.hrm_cat_sp_get_UnitPriceById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_UnitPriceModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Cat_UnitPriceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_UnitPriceEntity, Cat_UnitPriceModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_UnitPrice")]
        public Cat_UnitPriceModel Post([Bind]Cat_UnitPriceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UnitPriceModel>(model, "Cat_UnitPrice", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UnitPriceEntity, Cat_UnitPriceModel>(model);
        }
    }
}
