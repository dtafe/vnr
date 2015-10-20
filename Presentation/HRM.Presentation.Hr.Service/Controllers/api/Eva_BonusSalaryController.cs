using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Business.Evaluation.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities.Helper;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    /// <summary> Thưởng Lương </summary>
    public class Eva_BonusSalaryController : ApiController
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
        public Eva_BonusSalaryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_BonusSalaryModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_BonusSalaryEntity>(id, ConstantSql.hrm_eva_sp_get_BonusSalary, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_BonusSalaryModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_BonusSalaryModel DeleteOrRemove(string id)
        {
            var service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_BonusSalaryEntity, Eva_BonusSalaryModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_BonusSalary")]
        public Eva_BonusSalaryModel Post([Bind]Eva_BonusSalaryModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_BonusSalaryModel>(model, "Eva_BonusSalary", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            #region kt dữ liệu khi tính phòng ban, tháng hoặc quý
            if (string.IsNullOrEmpty(model.Quarter) && string.IsNullOrEmpty(model.Month.ToString()))
            {
                message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("Quarter").TranslateString()) + " Or " + string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("Month").TranslateString());
                model.ActionStatus = message;
                return model;
 
            }
            #endregion
            var service = new Eva_BonusSalaryServices();
            var bService = new BaseService();
            var bonusSalaryEntity = model.CopyData<Eva_BonusSalaryEntity>();
            var m = service.CalculateBonusSalary(bonusSalaryEntity,UserLogin);
            return m.CopyData<Eva_BonusSalaryModel>();
        }
        
	}
}