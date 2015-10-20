using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_CodeObjectController : ApiController
    {
        #region MyRegion
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
        public Sys_CodeObjectModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_CodeObjectModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_CodeObjectEntity>(id, ConstantSql.hrm_sys_sp_get_CodeObjectById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sys_CodeObjectModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        public Sys_CodeObjectModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_CodeObjectEntity, Sys_CodeObjectModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        public Sys_CodeObjectModel Post([Bind]Sys_CodeObjectModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_CodeObjectModel>(model, "Sys_CodeObject", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_CodeObjectEntity, Sys_CodeObjectModel>(model);
        }
    }
}
