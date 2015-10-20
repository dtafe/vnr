using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;
using System.Collections.Generic;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_HDTJobGroupController : ApiController
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
        public Cat_HDTJobGroupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_HDTJobGroupModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_HDTJobGroupEntity>(id, ConstantSql.hrm_cat_sp_get_HDTJobGroupById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_HDTJobGroupModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Cat_HDTJobGroupModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_HDTJobGroupEntity, Cat_HDTJobGroupModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_HDTJobGroup")]
        public Cat_HDTJobGroupModel Post([Bind]Cat_HDTJobGroupModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_HDTJobGroupModel>(model, "Cat_HDTJobGroup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            ActionService action = new ActionService(UserLogin);
            string status = string.Empty;
            if(model.ID!= null)
            {
                var resut = action.GetByIdUseStore<Cat_HDTJobGroupEntity>(model.ID, ConstantSql.hrm_cat_sp_get_HDTJobGroupById, ref status);
                if (resut != null && resut.Status == EnumDropDown.Status.E_APPROVED.ToString())
                {
                    model.ActionStatus = ConstantDisplay.StatusApproveCannotEdit.TranslateString();
                    return model;
                }
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_HDTJobGroupEntity, Cat_HDTJobGroupModel>(model);
        }
    }
}
