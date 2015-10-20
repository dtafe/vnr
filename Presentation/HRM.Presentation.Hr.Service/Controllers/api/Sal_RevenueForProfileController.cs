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
    public class Sal_RevenueForProfileController : ApiController
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
        public Sal_RevenueForProfileModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_RevenueForProfileModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_RevenueForProfileEntity>(id, ConstantSql.hrm_sal_sp_get_RevenueForProfileById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_RevenueForProfileModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Sal_RevenueForProfileModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_RevenueForProfileEntity, Sal_RevenueForProfileModel>(id);
            return result;
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_RevenueForProfile")]
        public Sal_RevenueForProfileModel Post([Bind]Sal_RevenueForProfileModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_RevenueForProfileModel>(model, "Sal_RevenueForProfile", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_RevenueForProfileEntity, Sal_RevenueForProfileModel>(model);
        }
    }
}
