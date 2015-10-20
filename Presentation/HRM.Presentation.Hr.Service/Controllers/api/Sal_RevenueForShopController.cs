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
    public class Sal_RevenueForShopController : ApiController
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
        public Sal_RevenueForShopModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_RevenueForShopModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_RevenueForShopEntity>(id, ConstantSql.hrm_sal_sp_get_RevenueForShopById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_RevenueForShopModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Sal_RevenueForShopModel DeleteOrRemove(string id)
        {
            //ActionService service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Sal_RevenueForShopEntity, Sal_RevenueForShopModel>(id);
            //return result;

            if (id != string.Empty)
            {
                var listID = id.Split(',').ToList();
                foreach (var i in listID)
                {
                    Sal_RevenueForShopServices service = new Sal_RevenueForShopServices();
                    var result = service.Remove<Sal_RevenueForShopEntity>(Common.ConvertToGuid(i));
                }
            }
            return new Sal_RevenueForShopModel();
        }

       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_RevenueForShop")]
        public Sal_RevenueForShopModel Post([Bind]Sal_RevenueForShopModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_RevenueForShopModel>(model, "Sal_RevenueForShop", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_RevenueForShopEntity, Sal_RevenueForShopModel>(model);
        }
    }
}
