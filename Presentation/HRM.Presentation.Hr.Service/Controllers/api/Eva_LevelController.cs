using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_LevelController : ApiController
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
        public Eva_LevelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_LevelModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_LevelEntity>(id, ConstantSql.hrm_eva_sp_get_EvaLevelById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_LevelModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_LevelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_LevelEntity, Eva_LevelModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_Level")]
        public Eva_LevelModel Post([Bind]Eva_LevelModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_LevelModel>(model, "Eva_Level", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_LevelEntity, Eva_LevelModel>(model);
        }
	}
}