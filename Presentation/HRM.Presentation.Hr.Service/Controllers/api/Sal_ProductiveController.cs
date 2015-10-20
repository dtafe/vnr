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

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Sal_ProductiveController : ApiController
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
        public Sal_ProductiveModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_ProductiveModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_ProductiveEntity>(id, ConstantSql.hrm_sal_sp_get_Sal_ProductiveByID, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_ProductiveModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        public Sal_ProductiveModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_ProductiveEntity, Sal_ProductiveModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_Productive")]
        public Sal_ProductiveModel Post([Bind]Sal_ProductiveModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ProductiveModel>(model, "Sal_Productive", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_ProductiveEntity, Sal_ProductiveModel>(model);
        }

	}
}