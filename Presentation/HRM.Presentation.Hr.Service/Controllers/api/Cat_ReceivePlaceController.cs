using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ReceivePlaceController : ApiController
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
        public Cat_ReceivePlaceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ReceivePlaceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ReceivePlaceEntity>(id, ConstantSql.hrm_Cat_SP_GET_RECEIVEPLACEBYID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_ReceivePlaceModel>();

            }
            return model;
        }

        public Cat_ReceivePlaceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ReceivePlaceEntity, Cat_ReceivePlaceModel>(id);
            return result;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ReceivePlace")]
        public Cat_ReceivePlaceModel Post([Bind]Cat_ReceivePlaceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ReceivePlaceModel>(model, "Cat_ReceivePlace", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ReceivePlaceEntity, Cat_ReceivePlaceModel>(model);
        }
	}
}