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
    public class Cat_TrainLevelController : ApiController
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
        public Cat_TrainLevelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_TrainLevelModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_TrainLevelEntity>(id, ConstantSql.hrm_cat_sp_get_TrainLevelByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_TrainLevelModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        public Cat_TrainLevelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_TrainLevelEntity, Cat_TrainLevelModel>(id);
            return result;
        }

  
        /// <summary>
        /// Xử lý thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_TrainLevel")]
        public Cat_TrainLevelModel Post([Bind]Cat_TrainLevelModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_TrainLevelModel>(model, "Cat_TrainLevel", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_TrainLevelEntity, Cat_TrainLevelModel>(model);
        }
    }
}