using System;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Insurance.Models;
using HRM.Presentation.Service;
using HRM.Business.Insurance.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Ins_ChildSickController : ApiController
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
        public Ins_ChildSickModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Ins_ChildSickModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Ins_ChildSickEntity>(id, ConstantSql.hrm_ins_sp_get_ChildSickById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Ins_ChildSickModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Ins_ChildSickModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Ins_ChildSickEntity, Ins_ChildSickModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Ins_ChildSick")]
        public Ins_ChildSickModel Post([Bind]Ins_ChildSickModel model)
        {
            
            string status = string.Empty;
            if (model.ProfileID == Guid.Empty)
                model.ProfileID = Common.ConvertToGuid(model.ProfileIDs);
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Ins_ChildSickModel>(model, "Ins_ChildSick", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            
                ActionService service = new ActionService(UserLogin);
                return service.UpdateOrCreate<Ins_ChildSickEntity, Ins_ChildSickModel>(model);

        }
      
    }
}
