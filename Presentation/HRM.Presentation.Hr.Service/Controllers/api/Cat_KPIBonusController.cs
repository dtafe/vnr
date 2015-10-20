using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_KPIBonusController : ApiController
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
        /// [T Lấy dữ liệu Thương Hiệu(Cat_KPIBonus) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_KPIBonusModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_KPIBonusModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_KPIBonusEntity>(id, ConstantSql.hrm_cat_sp_get_KPIBonusIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_KPIBonusModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Cat_KPIBonusServices();
            var result = service.Remove<Cat_KPIBonusEntity>(ID);
        }

   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_KPIBonus")]
        public Cat_KPIBonusModel Post([Bind]Cat_KPIBonusModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_KPIBonusModel>(model, "Cat_KPIBonus", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_KPIBonusEntity, Cat_KPIBonusModel>(model);
        }
    }
}