using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class EmployeeController : ApiController
    {
        string status = string.Empty;

        // GET api/<controller>
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmployeeModel> Get()
        {
            var service = new EmployeeServices();
            var list = service.GetAllUseEntity<Sys_UserInfoEntity>(ref status);

            //Service sử dụng Sys_UserInfoEntity, phải chuyển về dạng model tương ứng khi trả về Web
            return list.Select(item => new EmployeeModel
            {
                ID = item.ID,
                Code = item.Code,
                LoginName = item.UserLogin,
                LDAPDatasource = item.LDAPDatasource,
                IsActivate = item.IsActivate
            });
        }

        // GET api/<controller>/5
        /// <summary>
        /// Lấy dữ liệu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeModel Get(Guid id)
        {
            var service = new EmployeeServices();
            var emp = service.GetById<Sys_UserInfoEntity>(id, ref status);

            //Service sử dụng Sys_UserInfoEntity, phải chuyển về dạng model tương ứng khi trả về Web
            var emp1 = new EmployeeModel()
            {
                ID = emp.ID,
                Code = emp.Code,
                LoginName = emp.UserLogin,
                //LDAPDatasource = emp.LDAPDatasource
            };
            return emp1;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        /// <summary>
        /// Lưu dữ liệu xuống database, với tham số đầu vào là Model tương ứng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EmployeeModel Put(EmployeeModel model)
        {
            //Chuyển model thành Model tương dứng dưới database
            var modelEmp = new Sys_UserInfoEntity()
            {
                UserLogin = model.LoginName,
                Code = model.Code,
                IsCheckLDAP = true,
                LDAPDatasource="1"
            };
            var service = new EmployeeServices();
            if (model.ID!= Guid.Empty)
            {
                modelEmp.ID = model.ID;
                service.EditEmployees(modelEmp);
            }
            else
            {
               service.Add<Sys_UserInfoEntity>(modelEmp);
            }
            return model;
        }

        // DELETE api/<controller>/5
        /// <summary>
        /// Delete record theo id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            var service = new EmployeeServices();
            var r = service.Remove<Sys_UserInfoEntity>(id);
        }
    }
}
