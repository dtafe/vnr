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
    public class Sys_DelegateApproveController : ApiController
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
        public Sys_DelegateApproveModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_DelegateApproveModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_DelegateApproveEntity>(id, ConstantSql.hrm_sys_sp_get_DelegateApproveById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sys_DelegateApproveModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        public Sys_DelegateApproveModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_DelegateApproveEntity, Sys_DelegateApproveModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        public Sys_DelegateApproveModel Post([Bind]Sys_DelegateApproveModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_DelegateApproveModel>(model, "Sys_DelegateApprove", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_DelegateApproveEntity, Sys_DelegateApproveModel>(model);
        }
    }
}
