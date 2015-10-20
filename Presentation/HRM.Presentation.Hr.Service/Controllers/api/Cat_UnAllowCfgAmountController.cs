using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_UnAllowCfgAmountController : ApiController
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UnAllowCfgAmountModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_UnAllowCfgAmountModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_UnAllowCfgAmountEntity>(id, ConstantSql.hrm_cat_sp_get_UnAllowCfgAmountByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_UnAllowCfgAmountModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        //// DELETE api/<controller>
        //public void Delete(Guid ID)
        //{
        //    var service = new Cat_UnAllowCfgAmountServices();
        //    var result = service.Remove<Cat_UnAllowCfgAmountEntity>(ID);
        //}
        public Cat_UnAllowCfgAmountModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_UnAllowCfgAmountEntity, Cat_UnAllowCfgAmountModel>(id);
            return result;
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_UnAllowCfgAmount")]
        public Cat_UnAllowCfgAmountModel Post([Bind]Cat_UnAllowCfgAmountModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UnAllowCfgAmountModel>(model, "Cat_UnAllowCfgAmount", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UnAllowCfgAmountEntity, Cat_UnAllowCfgAmountModel>(model);
        }
    }
}