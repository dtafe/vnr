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
    public class Sys_TraConfigController : ApiController
    {

        public Sys_TraConfigModel GetById(Guid id)
        {
            var model = new Sys_TraConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetTraConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_TraConfigModel>();
            }
            return model;
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_TraConfig")]
        public String Post([Bind]Sys_TraConfigModel model)
        {
            string message = string.Empty;
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_TraConfigEntity>();
            service.SaveTraConfig(entity);
            return message;
        }
    }
}