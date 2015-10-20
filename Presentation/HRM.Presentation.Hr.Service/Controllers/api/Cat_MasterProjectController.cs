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
    public class Cat_MasterProjectController : ApiController
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
        public Cat_MasterProjectModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_MasterProjectModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_MasterProjectEntity>(id, ConstantSql.hrm_cat_sp_get_MasterProjectByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_MasterProjectModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Cat_MasterProjectServices();
            var result = service.Remove<Cat_MasterProjectEntity>(ID);
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_MasterProject")]
        public Cat_MasterProjectModel Post([Bind]Cat_MasterProjectModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_MasterProjectModel>(model, "Cat_MasterProject", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_MasterProjectEntity, Cat_MasterProjectModel>(model);
        }
    }
}