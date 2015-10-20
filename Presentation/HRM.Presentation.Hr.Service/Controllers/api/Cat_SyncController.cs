using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_SyncController : ApiController
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
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SyncModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_SyncModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_SyncEntity>(id, ConstantSql.hrm_cat_sp_get_SyncById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_SyncModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SyncModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_SyncEntity, Cat_SyncModel>(id);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_Sync")]
        public Cat_SyncModel Post([Bind]Cat_SyncModel model)
        {
            #region Validate
            string message = NotificationType.Success.ToString();
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_SyncModel>(model, "Cat_Sync", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_SyncEntity, Cat_SyncModel>(model);
        }
    }
}