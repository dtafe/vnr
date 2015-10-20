using HRM.Business.HrmSystem.Domain;
using HRM.Business.Main.Domain;
//using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Data;
using System;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    public class SysDataPermissionController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    //var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    //userLogin = headerValues.FirstOrDefault();
                    userLogin = "son.vo";
                }
                return userLogin;
            }
        }
        #endregion
        string status = string.Empty;
        // GET api/<controller>
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sys_DataPermissionModel> Get()
        {
            var service = new Sys_DataPermissionServices();
            var list = service.GetDataNotParam<Sys_DataPermissionEntity>(ConstantSql.hrm_system_sp_get_DataPermission, UserLogin, ref status);
                
            return list.Select(item => new Sys_DataPermissionModel
            {
                ID = item.ID,
                Code = item.Code,
                UserName = item.UserName,
                GroupName = item.GroupName,
                UserID = item.UserID,
                GroupID = item.GroupID,
                //Branches = item.Branches,
                DataGroups = item.DataGroups
            });

        }

        // GET api/<controller>/5
        public Sys_DataPermissionModel Get(Guid id)
        {
            var service = new Sys_DataPermissionServices();
            var result = service.GetById<Sys_DataPermissionEntity>(id, ref status);
            var DataPermission = new Sys_DataPermissionModel
            {
                ID = result.ID,
                Code = result.Code,
                UserID = result.UserID,
                GroupID = result.GroupID,
                //Branches = result.Branches,
                DataGroups = result.DataGroups,
                WorkPlace = result.WorkPlace
            };
            return DataPermission;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/SysDataPermission")]
        public Sys_DataPermissionModel Post([FromBody]Sys_DataPermissionModel model)
        {
            var service = new Sys_DataPermissionServices();

            var dataPermissionExist = service.CheckDuplicateDataPermission(model.UserID, model.GroupID);
            if (dataPermissionExist.ID != Guid.Empty)
            {
                model.ID = dataPermissionExist.ID;
            }

            byte[] branches = null;
            if (model.Branches != null)
            {
                branches = model.Branches.ToBinary();
            }
                
            var entity = new Sys_DataPermissionEntity
            {
                ID = model.ID,
                Code = model.Code,
                Branches = branches,
                GroupID = model.GroupID,
                UserID = model.UserID,
                DataGroup = model.DataGroup,
                DataGroups = model.DataGroups,
                OrgStructure = model.OrgStructure,
                WorkPlace =model.WorkPlace
            };

            if (model.ID != Guid.Empty)
            {
                model.ID = model.ID;
                service.Edit<Sys_DataPermissionEntity>(entity);
            }
            else
            {
                service.Add<Sys_DataPermissionEntity>(entity);
                model.ID = entity.ID;
            }

            return model;
        }
        // DELETE api/<controller>/5
        public void Delete(Guid id)
        {
            var service = new Sys_DataPermissionServices();
            var result = service.Delete<Sys_DataPermissionEntity>(id);
        }
    }
}