using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using HRM.Business.Category.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ModelController : ApiController
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
        public Cat_ModelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ModelModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ModelEntity>(id, ConstantSql.hrm_Cat_SP_GET_ModelByModelID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_ModelModel>();
                model.Model_ID = model.ID;
            }
            return model;
        }
        /// <summary>
        /// Xoa n nhiều dòng.
        /// </summary>
        /// <param name="id">ID hoặc chuổi ID</param>
        /// <returns></returns>
        public Cat_ModelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ModelEntity, Cat_ModelModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_Model")]
        public Cat_ModelModel Post([Bind]Cat_ModelModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ModelModel>(model, "Cat_Model", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ModelEntity, Cat_ModelModel>(model);
        }
        
	}
}