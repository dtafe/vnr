using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Data;
using System.Web.Http;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_ConditionApprovedController : ApiController
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
         public Sys_ConditionApprovedModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_ConditionApprovedModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_ConditionApprovedEntity>(id, ConstantSql.hrm_sys_sp_get_ConditionApprovedById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Sys_ConditionApprovedModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        
        public Sys_ConditionApprovedModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_ConditionApprovedEntity, Sys_ConditionApprovedModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_ConditionApproved")]
        public Sys_ConditionApprovedModel Post([Bind]Sys_ConditionApprovedModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_ConditionApprovedModel>(model, "Sys_ConditionApproved", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_ConditionApprovedEntity, Sys_ConditionApprovedModel>(model);
        }
    
    }
}
