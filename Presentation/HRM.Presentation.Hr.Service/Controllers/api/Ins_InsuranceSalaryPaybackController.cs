using System;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Insurance.Domain;
using HRM.Presentation.Insurance.Models;
using HRM.Presentation.Service;
using HRM.Business.Insurance.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Collections.Generic;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Ins_InsuranceSalaryPaybackController : ApiController
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
        public Ins_InsuranceSalaryPaybackModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Ins_InsuranceSalaryPaybackModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Ins_InsuranceSalaryPaybackEntity>(id, ConstantSql.hrm_ins_sp_get_SalPayBackByID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Ins_InsuranceSalaryPaybackModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Ins_InsuranceSalaryPaybackModel DeleteOrRemove(string id)
        {
            //ActionService service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Ins_InsuranceSalaryPaybackEntity, Ins_InsuranceSalaryPaybackModel>(id);
            var model = new Ins_InsuranceSalaryPaybackModel {
                ActionStatus = "Success"
            };
            InsuranceServices service = new InsuranceServices();
            service.DeleteInsurancePayBack(id);
            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Ins_InsuranceSalaryPayback")]
        public Ins_InsuranceSalaryPaybackModel Post([Bind]Ins_InsuranceSalaryPaybackModel model)
        {
            var hrService = new Hre_ProfileServices();
            var baseService = new BaseService();
            string status = string.Empty;
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Ins_InsuranceSalaryPaybackModel>(model, "Ins_InsuranceSalaryPayback", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Ins_InsuranceSalaryPaybackEntity, Ins_InsuranceSalaryPaybackModel>(model);
        }

    }
}
