using System;
using HRM.Business.HrmSystem.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_AllSettingController : ApiController
    {

        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public IEnumerable<Sys_AllSettingModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_AllSettingServices();
            var list = service.GetAllUseEntity<Sys_AllSettingEntity>(ref status);

            return list.Select(item => new Sys_AllSettingModel
            {
                ID = item.ID,
                Name = item.Name,
                Value1 = item.Value1,
                Value2 = item.Value2,
                ModuleName = item.ModuleName
            });
            
        }

        // GET api/<controller>/5
        public Sys_AllSettingModel Get(Guid id)
        {
            string status = string.Empty;
            var service = new Sys_AllSettingServices();
            var result = service.GetById<Sys_AllSettingEntity>(id, ref status);
            var Sys_AllSetting = new Sys_AllSettingModel
            {
                ID = result.ID,
                Name = result.Name,
                Value1 = result.Value1,
                Value2 = result.Value2,
                ModuleName = result.ModuleName
            };
            return Sys_AllSetting;
        }

        public Sys_AllSettingModel Put(Sys_AllSettingModel model)
        {
            var sysAllSetting = new Sys_AllSettingEntity
            {
                ID = model.ID,
                Name = model.Name,
                Value1 = model.Value1,
                Value2 = model.Value2,
                ModuleName = model.ModuleName,
                UserID = model.UserID
            };
            var service = new Sys_AllSettingServices();
            if (model.ID != Guid.Empty)
            {
                sysAllSetting.ID = model.ID;
                service.Edit<Sys_AllSettingEntity>(sysAllSetting);
            }
            else
            {
                service.Add<Sys_AllSettingEntity>(sysAllSetting);
            }

            return model;
        }

        public void Post(List<Dictionary<string,string>> lstModel)
        {
            Sys_AllSettingServices service = new Sys_AllSettingServices();
            service.SaveConfig(lstModel);
        }

        // DELETE api/<controller>/5
        public void Delete(Guid id)
        {
            var service = new Sys_AllSettingServices();
            var result = service.Remove<Sys_AllSettingEntity>(id);
        }

    }
}