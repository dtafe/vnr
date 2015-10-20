using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Recruitment.Models;
using HRM.Presentation.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Rec_JobConditionController : ApiController
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
        public Rec_JobConditionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_JobConditionModel();

            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_JobConditionEntity>(id, ConstantSql.hrm_rec_sp_get_JobConditionById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_JobConditionModel>();
                if (model.ConditionName == ConditionName.E_DISEASEIDS.ToString() && !string.IsNullOrEmpty(model.Value1))
                {
                    var service1 = new BaseService();
                    var lst = service1.GetData<Cat_ComputingLevelMultiEntity>(model.Value1, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds, UserLogin ,ref status).ToList();
                    var ids = model.Value1
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => Common.OracleToDotNet(x.ToString()))
                            .ToList();

                    model.Value1 = string.Join(",", ids);
                    model.DiseaseIDs = string.Join(",", ids);
                    //   model.ArrDisease = lst;

                }

            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rec_JobConditionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_JobConditionEntity, Rec_JobConditionModel>(id);
            return result;
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_JobCondition")]
        public Rec_JobConditionModel Post([Bind]Rec_JobConditionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_JobConditionModel>(model, "Rec_JobCondition", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            if (model.ConditionName == ConditionName.E_DISEASEIDS.ToString() && !string.IsNullOrEmpty(model.Value1))
            {
                var ids = model.Value1
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Common.DotNetToOracle(x))
                        .ToList();
                model.Value1 = string.Join(",", ids);

            }
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_JobConditionEntity, Rec_JobConditionModel>(model);
        }
    }
}