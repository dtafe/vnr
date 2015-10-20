using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Service;
using HRM.Business.Main.Domain;
namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_CostCentreSalController : ApiController
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
        public Sal_CostCentreSalModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_CostCentreSalModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_CostCentreSalEntity>(id, ConstantSql.hrm_hr_sp_get_Sal_CostCentreSalById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_CostCentreSalModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        public Sal_CostCentreSalModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_CostCentreSalEntity, Sal_CostCentreSalModel>(id);
            return result;
        }

  
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_CostCentreSal")]
        public Sal_CostCentreSalModel Post([Bind]Sal_CostCentreSalModel model)
        {
            ActionService service = new ActionService(UserLogin);
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_CostCentreSalModel>(model, "Sal_CostCentreSal", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;

            }
            #endregion
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID.ToString()));
            objs.Add(1);
            objs.Add(10000);
            var result = baseService.GetData<Sal_CostCentreSalEntity>(objs, ConstantSql.hrm_hr_sp_get_Sal_CostCentreSalByProfileId, UserLogin, ref status);
            //kiem tra he so cua 1 nv
            if(model.ID == Guid.Empty)
            {
                double? rate = result.Sum(s => s.Rate);
                double? ratenew = rate + model.Rate;
                if (ratenew > 1)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + ",");
                    return model;
                }
            }
            else
            {
                result = result.Where(s => s.ID != model.ID).ToList();
                double? rate = result.Sum(s => s.Rate);
                double? ratenew = rate + model.Rate;
                if (ratenew > 1)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + ",");
                    return model;
                }
            }
            //kiem tra trung
            var resultdulicate = result.Where(s => s.CostCentreID == model.CostCentreID && s.ElementType == model.ElementType).ToList();
            if (resultdulicate.Count > 0)
            {
                model.SetPropertyValue(Constant.ActionStatus, NotificationType.Info + "," + ConstantMessages.FieldDuplicate);
                return model;
            }
            return service.UpdateOrCreate<Sal_CostCentreSalEntity, Sal_CostCentreSalModel>(model);
        }
    }
}
