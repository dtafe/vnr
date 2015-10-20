using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Business.Finance.Domain;
using HRM.Business.Finance.Models;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class FIN_ApproverECLAIMController : ApiController
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
        public FIN_ApproverECLAIMModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new FIN_ApproverECLAIMModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<FIN_ApproverECLAIMEntity>(id, ConstantSql.hrm_eva_sp_get_ApproverECLAIMById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<FIN_ApproverECLAIMModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FIN_ApproverECLAIMModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<FIN_ApproverECLAIMEntity, FIN_ApproverECLAIMModel>(id);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/FIN_ApproverECLAIM")]
        public FIN_ApproverECLAIMModel Post([Bind]FIN_ApproverECLAIMModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<FIN_ApproverECLAIMModel>(model, "FIN_ApproverECLAIM", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion


            FIN_ApproverECLAIMService approverECLAIMService = new FIN_ApproverECLAIMService();
            var approverECLAIMEntity = model.CopyData<FIN_ApproverECLAIMEntity>();
            approverECLAIMService.AddApprovers(approverECLAIMEntity);
            return model;
        }
    }
}
