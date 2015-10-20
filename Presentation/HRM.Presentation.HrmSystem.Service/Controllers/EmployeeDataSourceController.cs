//using HRM.Data.HrmSystem.Model;

using HRM.Business.HrmSystem.Models;
using Kendo.Mvc.UI;
using System;
using Kendo.Mvc.Extensions;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    public class EmployeeDataSourceController : Controller, IDisposable
    {
        string status = string.Empty;
        //
        // GET: /EmployeeDataSource/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetEmployees([DataSourceRequest] DataSourceRequest request)
        {
            EmployeeServices service = new EmployeeServices();
            var list = service.GetAllUseEntity<Sys_UserInfoEntity>(ref status);
            return Json(list.ToDataSourceResult(request)); 
            
            #region code cũ
            //EmployeeServices service = new EmployeeServices();
            //var list = service.GetEmployees();
            //return Json(list.ToDataSourceResult(request)); 
            #endregion
        }

        public JsonResult GetEmployeesId(Guid id)
        {
            EmployeeServices service = new EmployeeServices();
            var emp = service.GetById<Sys_UserInfoEntity>(id, ref status);
            var emp1 = new Sys_UserInfoEntity()
            {
                ID = emp.ID,
                UserLogin = emp.UserLogin,
                UserInfoName = emp.UserInfoName
            };
            return Json(emp1);

            #region code cũ
            //EmployeeServices service = new EmployeeServices();
            //var emp = service.GetEmployeesById(id);
            //var emp1 = new Data.HrmSystem.Model.Sys_User()
            //{
            //    Id = emp.Id,
            //    UserLogin = emp.UserLogin,
            //    UserName = emp.UserName
            //};
            //return Json(emp1); 
            #endregion
        }

































        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateEmp([DataSourceRequest] DataSourceRequest request,Sys_User user)
        //{
        //    EmployeeServices service = new EmployeeServices();
        //    var list = service.UpdateEmployees(user);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        //public Sys_User Put(long id, Sys_User User)
        //{
        //    var modelCateogry = _categoryFetcher.GetCategory(id);

        //    modelCateogry.Name = category.Name;
        //    modelCateogry.Description = category.Description;

        //    _session.SaveOrUpdate(modelCateogry);

        //    return _categoryMapper.CreateCategory(modelCateogry);
        //}

        //public HttpResponseMessage AddEmployees(HttpRequestMessage request, Sys_User user)
        //{
        //    var modelCategory = new Data.HrmSystem.Model.Sys_User
        //    {
        //        UserLogin = "dddd",
        //        UserName = "dkaf"
        //    };
        //    var response = request.CreateResponse(HttpStatusCode.Created, modelCategory);
        //    return response;
        //}
	}
}