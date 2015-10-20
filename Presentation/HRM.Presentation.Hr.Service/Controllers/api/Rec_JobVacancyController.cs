using HRM.Business.Recruitment.Domain;
using HRM.Business.Recruitment.Models;
using HRM.Infrastructure.Utilities;
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
    public class Rec_JobVacancyController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        #region MyRegion
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
        public Rec_JobVacancyModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_JobVacancyModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_JobVacancyEntity>(id, ConstantSql.hrm_rec_sp_get_JobVacancyById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_JobVacancyModel>();
                model.JobVacancyID = model.ID;
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_JobVacancyModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_JobVacancyEntity, Rec_JobVacancyModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_JobVacancy")]
        public Rec_JobVacancyModel Post([Bind]Rec_JobVacancyModel model)
        {
           
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_JobVacancyModel>(model, "Rec_JobVacancy", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if(model != null && model.DateEnd != null && model.DateStart != null)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_JobVacancyModel>(model, "Rec_JobVacancyCheckDate", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            #endregion
            if (model.ID != Guid.Empty)
            {
                var service1=new Rec_JobVacancyServices();
                model.JobConditionIDs = service1.GetJobConditionIDs(model.ID);
                
            }
            ActionService service = new ActionService(UserLogin);
            var rs= service.UpdateOrCreate<Rec_JobVacancyEntity, Rec_JobVacancyModel>(model);
            if (rs.ID != Guid.Empty)
                rs.JobVacancyID = rs.ID;
            return rs;
        }
	}
}