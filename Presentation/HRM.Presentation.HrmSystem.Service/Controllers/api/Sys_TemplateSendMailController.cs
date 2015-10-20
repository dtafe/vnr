using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_TemplateSendMailController : ApiController
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
        public Sys_TemplateSendMailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_TemplateSendMailModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_TemplateSendMailEntity>(id, ConstantSql.hrm_sys_sp_get_TemplateSendMailById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sys_TemplateSendMailModel>();
            }

            //model.Content= HttpUtility.HtmlEncode(model.Content);
            model.ActionStatus = status;
            return model;
        }


        public Sys_TemplateSendMailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_TemplateSendMailEntity, Sys_TemplateSendMailModel>(id);
            return result;
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_TemplateSendMail")]
        [ValidateInput(false)]
        public Sys_TemplateSendMailModel Post([Bind]Sys_TemplateSendMailModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_TemplateSendMailModel>(model, "Sys_TemplateSendMail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            model.Content = "<html><head><title></title></head><body class='scayt-enabled'>" + HttpUtility.HtmlDecode(model.Content) + "</body></html>";
            model.Content = HttpUtility.HtmlEncode(model.Content);
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_TemplateSendMailEntity, Sys_TemplateSendMailModel>(model);
        }
    
    }
}