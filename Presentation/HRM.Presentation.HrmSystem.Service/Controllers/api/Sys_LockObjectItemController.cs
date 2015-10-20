using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_LockObjectItemController : ApiController
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
        //
        // GET: /Sys_LockObjectItem/
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public IEnumerable<Sys_LockObjectItemModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_LockObjectItemServices();
            var list = service.GetAllUseEntity<Sys_LockObjectItemEntity>(ref status);

            return list.Translate<Sys_LockObjectItemModel>();

        }

        // GET api/<controller>/5
        public Sys_LockObjectItemModel Get(Guid id)
        {
            string status = string.Empty;
            var model = new Sys_LockObjectItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_LockObjectItemEntity>(id, ConstantSql.hrm_sys_sp_get_LockObjectItemByID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Sys_LockObjectItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Sys_LockObjectItemModel Put(Sys_LockObjectItemModel model)
        {
            var Sys_LockObjectItem = model.CopyData<Sys_LockObjectItemEntity>();
            
            var service = new Sys_LockObjectItemServices();
            if (model.ID != Guid.Empty)
            {
                Sys_LockObjectItem.ID = model.ID;
                service.Edit<Sys_LockObjectItemEntity>(Sys_LockObjectItem);
            }
            else
            {
                service.Add<Sys_LockObjectItemEntity>(Sys_LockObjectItem);
            }
            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_LockObjectItem")]
        public Sys_LockObjectItemModel Post(Sys_LockObjectItemModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var lockItemServices = new Sys_LockObjectItemServices();
            string message = string.Empty;
            if(model.ObjectName.IndexOf(',') > 1)
            {
               string[] objName = model.ObjectName.Split(',');
               if (objName.Length > 0)
               {
                   foreach (var item in objName)
                   {
                       Sys_LockObjectItemEntity entity = new Sys_LockObjectItemEntity();
                       entity = model.CopyData<Sys_LockObjectItemEntity>();
                       entity.ObjectName = item;
                      // model = service.UpdateOrCreate<Sys_LockObjectItemEntity, Sys_LockObjectItemModel>(model);
                      model.ActionStatus =  lockItemServices.Add(entity);
                   }
               
               }
              
               return model;
            }
            return service.UpdateOrCreate<Sys_LockObjectItemEntity, Sys_LockObjectItemModel>(model);
        }

        public Sys_LockObjectItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sys_LockObjectItemEntity, Sys_LockObjectItemModel>(id);
            return result;
        }
	}
}