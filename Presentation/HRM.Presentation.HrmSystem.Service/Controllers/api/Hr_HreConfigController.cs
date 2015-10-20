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
    public class Hr_HreConfigController : ApiController
    {
        /// <summary>
        /// [Kiet.Chung] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_HreConfigModel GetById(Guid id)
        {
            var model = new Sys_HreConfigModel();
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = service.GetHrConfig();
            if (entity != null)
            {
                model = entity.CopyData<Sys_HreConfigModel>();
            }
            return model;
        }
       
        /// <summary>
        /// [Kiet.Chung] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hr_HreConfig")]
        public String Post([Bind]Sys_HreConfigModel model)
        {
            string message = string.Empty;
            var service = new Sys_AttOvertimePermitConfigServices();
            var entity = model.CopyData<Sys_HreConfigEntity>();
            service.SaveHrConfig(entity);
            return message;
        }
    }
}