using System;
using HRM.Business.HrmSystem.Domain;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    public class SysResourceController : ApiController
    {
        string status = string.Empty;

        // GET api/<controller>
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sys_ResourceModel> Get()
        {
            var service = new Sys_ResourceServices();
            var list = service.GetAllUseEntity<Sys_ResourceEntity>(ref status);

            return list.Select(item => new Sys_ResourceModel
            {
                ID = item.ID,
                Code = item.Code,
                //ResourceType = item.ResourceType,
                ResourceName = item.ResourceName,
                ModuleName = item.ModuleName,
                //Description = item.Description
            });
        }

        // GET api/<controller>/5
        public Sys_ResourceModel Get(Guid id)
        {
            var service = new Sys_ResourceServices();
            var result = service.GetById<Sys_ResourceEntity>(id, ref status);
            var AttWorkDay = new Sys_ResourceModel
            {
                ID = result.ID,
                Code = result.Code,
                //ResourceType = result.ResourceType,
                ResourceName = result.ResourceName,
                ModuleName = result.ModuleName,
                //Description = result.Description
            };
            return AttWorkDay;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        public Sys_ResourceModel Put(Sys_ResourceModel model)
        {
            var Resource = new Sys_ResourceEntity
            {
                ID = model.ID,
                Code = model.Code,
                //ResourceType = model.ResourceType,
                ResourceName = model.ResourceName,
                ModuleName = model.ModuleName,
                //Description = model.Description
               
            };
            var service = new Sys_ResourceServices();
            if (model.ID != Guid.Empty)
            {
                Resource.ID = model.ID;
                service.Edit<Sys_ResourceEntity>(Resource);
            }
            else
            {
                service.Add<Sys_ResourceEntity>(Resource);
            }

            return model;
        }
        // DELETE api/<controller>/5
        public void Delete(Guid id)
        {
            var service = new Sys_ResourceServices();
            var result = service.Remove<Sys_ResourceEntity>(id);
        }
    }
}