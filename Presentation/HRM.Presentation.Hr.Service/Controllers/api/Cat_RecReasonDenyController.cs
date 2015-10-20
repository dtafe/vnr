using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Presentation.Evaluation.Models;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_RecReasonDenyController : ApiController
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
        public Cat_NameEntityModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_NameEntityModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_NameEntityEntity>(id, ConstantSql.hrm_cat_sp_get_NameEntityById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_NameEntityModel>();
            }
            model.ActionStatus = status;
            return model;
        }
        public Cat_NameEntityModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_NameEntityEntity, Cat_NameEntityModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ResonDeny")]
        public Cat_NameEntityModel Post([Bind]Cat_NameEntityModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_NameEntityModel>(model, "Cat_NameEntity", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
        }
	}
}