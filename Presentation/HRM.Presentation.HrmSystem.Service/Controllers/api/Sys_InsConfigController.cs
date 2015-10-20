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
    public class Sys_InsConfigController : ApiController
    {

        public Sys_InsConfigModel GetById(Guid id)
        {
            var model = new Sys_InsConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetInsConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_InsConfigModel>();
            }
            return model;
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_InsConfig")]
        public String Post([Bind]Sys_InsConfigModel model)
        {
            string message = string.Empty;
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_InsConfigEntity>();
            service.SaveInsConfig(entity);
            return message;
        }
    }
}