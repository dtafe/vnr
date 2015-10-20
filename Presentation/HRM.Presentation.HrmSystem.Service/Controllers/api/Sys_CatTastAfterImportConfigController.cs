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
    public class Sys_CatTastAfterImportConfigController : ApiController
    {

        public Sys_CatTastAfterImportConfigModel GetById(Guid id)
        {
            var model = new Sys_CatTastAfterImportConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetCatAfterConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_CatTastAfterImportConfigModel>();
            }
            return model;
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_TaskAfterImportConfig")]
        public String Post([Bind]Sys_CatTastAfterImportConfigModel model)
        {
            string message = string.Empty;
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_CatTaskAfterImportConfigEntity>();
            service.SaveCatAfterConfig(entity);
            return message;
        }
	}
}