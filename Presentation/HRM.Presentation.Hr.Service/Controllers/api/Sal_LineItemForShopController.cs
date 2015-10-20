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
    public class Sal_LineItemForShopController : ApiController
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
        public Sal_LineItemForShopModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_LineItemForShopModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_LineItemForShopEntity>(id, ConstantSql.hrm_sal_sp_get_LineItemForShopById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_LineItemForShopModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Sal_LineItemForShopModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_LineItemForShopEntity, Sal_LineItemForShopModel>(id);
            return result;
        }

       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_LineItemForShop")]
        public Sal_LineItemForShopModel Post([Bind]Sal_LineItemForShopModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_LineItemForShopModel>(model, "Sal_LineItemForShop", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_LineItemForShopEntity, Sal_LineItemForShopModel>(model);
        }
    }
}
