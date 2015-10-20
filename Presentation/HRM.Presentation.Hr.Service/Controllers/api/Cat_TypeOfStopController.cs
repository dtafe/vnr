using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Presentation.Evaluation.Models;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_TypeOfStopController : ApiController
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
        public CatNameEntityModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatNameEntityModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_NameEntityEntity>(id, ConstantSql.hrm_cat_sp_get_NameEntityById, ref status);
            if(entity!= null)
            {
                model = entity.CopyData<CatNameEntityModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public CatNameEntityModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_NameEntityEntity, CatNameEntityModel>(id);
            return result;
        }
        /// <summary>
        /// xoa hay cap nhat mot doi tuong
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CatNameEntityModel Post([Bind]CatNameEntityModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatNameEntityModel>(model, "Cat_TypeOfStop", ref message);
            if(!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_NameEntityEntity, CatNameEntityModel>(model);
        }
        
    }
}
