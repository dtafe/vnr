using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Main.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_SaleEvaluationController : ApiController
    {
        public const string Eva_SaleEvaluation = "Eva_SaleEvaluation";
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
        //public IEnumerable<Eva_SalesTypeMultiModel> Get()
        //{
        //    BaseService service = new BaseService();
        //    string status = string.Empty;
        //    var listEntity = service.GetData<Eva_SalesTypeMultiModel>(string.Empty, ConstantSql.hrm_eva_sp_get_SalesType_multi, ref status);

        //    if (listEntity != null)
        //    {
        //        var listModel = listEntity.Translate<Eva_SalesTypeMultiModel>();
        //        return listModel;
        //    }
        //    return new List<Eva_SalesTypeMultiModel>();
        //}

        public Eva_SaleEvaluationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_SaleEvaluationModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_SaleEvaluationEntity>(id, ConstantSql.hrm_eva_sp_get_SaleEvaluationByID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_SaleEvaluationModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_SaleEvaluationModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_SaleEvaluationEntity, Eva_SaleEvaluationModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_SaleEvaluation")]
        public Eva_SaleEvaluationModel Post([Bind]Eva_SaleEvaluationModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_SaleEvaluationModel>(model, "Eva_SaleEvaluation", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion

            #region Tính tỉ lệ = thực đạt / chỉ tiêu

            if (model != null)
            {
                model.ResultNumber = model.ResultNumber.HasValue ? model.ResultNumber.Value : 0;
                model.TagetNumber = model.TagetNumber.HasValue ? model.TagetNumber.Value : 0;
                if (model.TagetNumber > 0)
                {
                    model.ResultPercent = (model.ResultNumber / model.TagetNumber)*100;
                }
            }
            #endregion


            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_SaleEvaluationEntity, Eva_SaleEvaluationModel>(model);
        }
	}
}