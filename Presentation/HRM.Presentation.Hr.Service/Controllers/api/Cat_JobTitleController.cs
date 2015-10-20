using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_JobTitleController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu JobTitle(Cat_JobTitle) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_JobTitleModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_JobTitleModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_JobTitleEntity>(id,ConstantSql.hrm_cat_sp_get_JobTitleById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_JobTitleModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của JobTitle(Cat_JobTitle) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_JobTitleModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_JobTitleEntity, Cat_JobTitleModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một JobTitle(Cat_JobTitle)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_JobTitle")]
        public Cat_JobTitleModel Post([Bind]Cat_JobTitleModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_JobTitleModel>(model, "Cat_JobTitle", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_JobTitleEntity, Cat_JobTitleModel>(model);
        }
       
    }
}
