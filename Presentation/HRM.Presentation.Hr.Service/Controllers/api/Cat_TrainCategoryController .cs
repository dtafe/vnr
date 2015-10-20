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
    public class Cat_TrainCategoryController : ApiController
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
        public Cat_TrainCategoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_TrainCategoryModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_TrainCategoryEntity>(id, ConstantSql.hrm_cat_sp_get_TrainCategoryByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_TrainCategoryModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public Cat_TrainCategoryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_TrainCategoryEntity, Cat_TrainCategoryModel>(id);
            return result;
        }
  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_TrainCategory")]
        public Cat_TrainCategoryModel Post([Bind]Cat_TrainCategoryModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_TrainCategoryModel>(model, "Cat_TrainCategory", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_TrainCategoryEntity, Cat_TrainCategoryModel>(model);
        }
    }
}