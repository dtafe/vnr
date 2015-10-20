
using System;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Web.Http;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_GeneralConfigController : ApiController
    {

        /// <summary>
        /// Lấy dữ liệu cấu hình chung
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_GeneralConfigModel GetById(Guid id)
        {
            var model = new Sys_GeneralConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetGeneralConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_GeneralConfigModel>();
            }
            return model;
        }

        /// <summary>
        ///  Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_GeneralConfig")]
        public String Post([Bind]Sys_GeneralConfigModel model)
        {
            string message = string.Empty;
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_GeneralConfigEntity>();
            service.SaveGeneralConfig(entity);
            return message;
        }

    }
}