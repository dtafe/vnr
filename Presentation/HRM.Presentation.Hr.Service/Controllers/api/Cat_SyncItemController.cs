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
    public class Cat_SyncItemController : ApiController
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
        /// 
        /// </summary>
        /// <param name="Leaveday"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_SyncItem")]
        public Cat_SyncItemModel Post([Bind]Cat_SyncItemModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_SyncItemEntity, Cat_SyncItemModel>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SyncItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_SyncItemEntity, Cat_SyncItemModel>(id);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SyncItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_SyncItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_SyncItemEntity>(id, ConstantSql.hrm_cat_sp_get_ImportItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_SyncItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }
    }
}