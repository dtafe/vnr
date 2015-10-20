using System;
using System.Collections.Generic;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Http;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    public class Sys_GroupController : ApiController
    {
        // GET api/<controller>
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IQueryable<Sys_GroupModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_GroupServices();
            var list = service.GetAllUseEntity<Sys_GroupEntity>(ref status).AsQueryable<Sys_GroupEntity>();

            return list.Select(item => new Sys_GroupModel
            {
                ID = item.ID,
                GroupName = item.GroupName,
                Code = item.Code,
                IsActivate = item.IsActivate,
                Notes = item.Description
            });
        }

        // GET api/<controller>/5
        public Sys_GroupModel Getbyid(Guid id)
        {
            var service = new Sys_GroupServices();
            var result = service.GetSysGroupById(id);
            var sysGroupModel = new Sys_GroupModel
            {
                ID = result.ID,
                Code = result.Code,
                GroupName = result.GroupName,
                IsActivate = result.IsActivate,
                Notes = result.Notes
            };
            return sysGroupModel;

        }

        public Sys_GroupModel Put(Sys_GroupModel model)
        {
            var group = new Sys_GroupEntity
            {
                ID = model.ID,
                Code = model.Code,
                GroupName = model.GroupName,
                IsActivate = model.IsActivate,

            };
            var service = new Sys_GroupServices();
            if (model.ID != Guid.Empty)
            {
                group.ID = model.ID;
                service.Edit<Sys_GroupEntity>(group);
            }
            else
            {
                service.Add<Sys_GroupEntity>(group);
            }

            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_GroupEntity")]
        public string Post([Bind]Sys_GroupModel group)
        {
            var success = string.Empty;
            var model = new Sys_GroupEntity
            {
                ID = group.ID,
                Code = group.Code,
                GroupName = group.GroupName,
                IsActivate = group.IsActivate
            };
            var service = new Sys_GroupServices();
            if (group.ID != Guid.Empty)
            {
                group.ID = group.ID;
                success = service.Edit<Sys_GroupEntity>(model) + ",1";
            }
            else
            {
                success = service.Add<Sys_GroupEntity>(model) + ",0";
                group.ID = model.ID;
            }

            return success;
        }

        // DELETE api/<controller>/5
        public void Delete(Guid id)
        {
            var service = new Sys_GroupServices();
            var result = service.Delete<Sys_GroupEntity>(id);//delete nhóm (sys_Group)
            
            if (id != Guid.Empty)
            {
                //delete ds nhóm quyền (sys_GroupPermission2)
                string status = string.Empty;
                BaseService baseService = new BaseService();
                List<object> lstObj = new List<object>();
                lstObj.Add(id);
                service.UpdateData<Sys_GroupPermission2Entity>(lstObj, ConstantSql.hrm_sys_sp_delete_GroupPermission2, ref status);
            }
        }
    }

}