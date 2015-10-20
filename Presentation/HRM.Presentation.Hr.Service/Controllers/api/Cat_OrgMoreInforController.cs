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
    public class Cat_OrgMoreInforController : ApiController
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
        public Cat_OrgMoreInforModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_OrgMoreInforModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_OrgMoreInforEntity>(id, ConstantSql.hrm_cat_sp_get_OwnerByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_OrgMoreInforModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Cat_OwnerServices();
            var result = service.Remove<Cat_OrgMoreInforModel>(ID);
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_Owner")]
        public Cat_OrgMoreInforModel Post([Bind]Cat_OrgMoreInforModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_OrgMoreInforModel>(model, "Cat_OrgMoreInfor", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_OrgMoreInforEntity, Cat_OrgMoreInforModel>(model);
        }
    }
}