using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ExchangeRateController : ApiController
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
        #region HRM8.20/7/2014
        
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_ExchangeRateModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ExchangeRateModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ExchangeRateEntity>(id, ConstantSql.hrm_cat_sp_get_ExchangeRateByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_ExchangeRateModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public Cat_ExchangeRateModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ExchangeRateEntity, Cat_ExchangeRateModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ExchangeRate")]
        public Cat_ExchangeRateModel Post([Bind]Cat_ExchangeRateModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ExchangeRateModel>(model, "Cat_ExchangeRate", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ExchangeRateEntity, Cat_ExchangeRateModel>(model);
        }
    }
}