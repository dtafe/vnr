﻿using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using HRM.Business.Main.Domain;
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
    public class Rec_CandidateLanguageLevelController : ApiController
    {

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
        public Rec_CandidateLanguageLevelModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_CandidateLanguageLevelModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_CandidateLanguageLevelEntity>(id, ConstantSql.hrm_rec_sp_get_Rec_CandidateLanguageLevelById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_CandidateLanguageLevelModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_CandidateLanguageLevelModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_CandidateLanguageLevelEntity, Rec_CandidateLanguageLevelModel>(id);
            return result;
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_CandidateLanguageLevel")]
        public Rec_CandidateLanguageLevelModel Post([Bind]Rec_CandidateLanguageLevelModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_CandidateLanguageLevelModel>(model, "Rec_CandidateLanguageLevel", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_CandidateLanguageLevelEntity, Rec_CandidateLanguageLevelModel>(model);
        }
    }
}