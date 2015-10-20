using System;
using System.Linq;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using System.Web.Http;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Data;
using System.Web.Mvc;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_EvaConfigController : ApiController
    {
        public Sys_EvaConfigModel GetById(Guid id)
        {
            var model = new Sys_EvaConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetEvaConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_EvaConfigModel>();
            }
            return model;
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_EvaConfig")]
        public String Post([Bind]Sys_EvaConfigModel model)
        {
            string message = string.Empty;
            #region Validate
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_EvaConfigModel>(model, "Sys_EvaConfig", ref message);
            if (!checkValidate)
            {
               // model.ActionStatus = message;
                return message;
            }
            #endregion

            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_EvaConfigEntity>();
            service.SaveEvaConfig(entity);
            return message;
        }
    }
}