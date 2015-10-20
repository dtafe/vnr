using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_SalaryDepartmentItemController : ApiController
    {
        #region UserLogin
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
        public Sal_SalaryDepartmentItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_SalaryDepartmentItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_SalaryDepartmentItemEntity>(id, ConstantSql.hrm_sal_sp_get_SalDepartmentItemById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_SalaryDepartmentItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Sal_SalaryDepartmentItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_SalaryDepartmentItemEntity, Sal_SalaryDepartmentItemModel>(id);
            return result;
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_SalaryDepartmentItem")]
        public Sal_SalaryDepartmentItemModel Post([Bind]Sal_SalaryDepartmentItemModel model)
        {
            //#region Validate
            //string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_SalaryDepartmentItemModel>(model, "Sal_SalaryDepartmentItem", ref message);
            //if (!checkValidate)
            //{
            //    model.ActionStatus = message;
            //    return model;
            //}
            //#endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_SalaryDepartmentItemEntity, Sal_SalaryDepartmentItemModel>(model);
        }
    }
}
