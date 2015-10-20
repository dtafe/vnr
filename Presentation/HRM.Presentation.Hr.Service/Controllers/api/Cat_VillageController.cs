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
    public class Cat_VillageController : ApiController
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
        public Cat_VillageModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_VillageModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_VillageEntity>(id, ConstantSql.hrm_cat_sp_get_VillageByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_VillageModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Cat_VillageModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_VillageEntity, Cat_VillageModel>(id);
            return result;
        }
  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_Village")]
        public Cat_VillageModel Post([Bind]Cat_VillageModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_VillageModel>(model, "Cat_Village", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_VillageEntity, Cat_VillageModel>(model);
        }
    }
}