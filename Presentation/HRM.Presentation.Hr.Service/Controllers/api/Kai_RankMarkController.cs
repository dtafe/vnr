using HRM.Business.Payroll.Models;
using HRM.Business.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.Recruitment.Models;
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
    public class Kai_RankMarkController : ApiController
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
        public Kai_RankMarkModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Kai_RankMarkModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Kai_RankMarkEntity>(id, ConstantSql.hrm_kai_sp_get_RankMarkById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Kai_RankMarkModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Kai_RankMarkModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Kai_RankMarkEntity, Kai_RankMarkModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Kai_RankMark")]
        public Kai_RankMarkModel Post([Bind]Kai_RankMarkModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Kai_RankMarkModel>(model, "Kai_RankMark", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Kai_RankMarkEntity, Kai_RankMarkModel>(model);
        }
	}
}