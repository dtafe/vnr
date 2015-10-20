using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_StopWorkingController : ApiController
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
        public Hre_StopWorkingModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_StopWorkingModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_StopWorkingEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_StopWorkingById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_StopWorkingModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Hre_StopWorkingModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_StopWorkingEntity, Hre_StopWorkingModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_StopWorking")]
        public Hre_StopWorkingModel Post([Bind]Hre_StopWorkingModel model)
        {
            ActionService service = new ActionService(UserLogin);
            #region Validate
            var ValiteConfig = "Hre_StopWorking";
            if (model.RequestDateComeBack != null)
            {
                ValiteConfig = "Hre_StopWorking_RegisterComeBack";
            }
            else if (model.DateStop != null && model.RequestDate != null && model.TypeSuspense != null)
            {
                ValiteConfig = "Hre_StopWorking_RegisterSuspense";
            }
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_StopWorkingModel>(model, ValiteConfig, ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            string status = "";
            var result = service.GetData<Hre_StopWorkingEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_StopWorkingByProId, ref status);
            if (result.Count > 0 && result.FirstOrDefault().ID != model.ID)
            {
                model.ActionStatus = ConstantMessages.RegisterDuplicate.TranslateString();
                return model;
            }
            if (model.Status == null)
            {
                model.Status = EnumDropDown.StopWorkStatus.E_WAITAPPROVE.ToString();
            }

            #endregion
           
            
           return service.UpdateOrCreate<Hre_StopWorkingEntity, Hre_StopWorkingModel>(model, model.UserID);

        }
    }
}
