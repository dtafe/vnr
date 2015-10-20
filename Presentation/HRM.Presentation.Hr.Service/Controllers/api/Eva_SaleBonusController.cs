using System;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    /// <summary> Thưởng Doanh Số </summary>
    public class Eva_SaleBonusController : ApiController
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

        public Eva_SaleBonusModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_SaleBonusModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_SaleBonusEntity>(id, ConstantSql.hrm_eva_sp_get_SaleBonusById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_SaleBonusModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_SaleBonusModel DeleteOrRemove(string id)
        {
            var service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_SaleBonusEntity, Eva_SaleBonusModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_SaleBonus")]
        public Eva_SaleBonusModel Post([Bind]Eva_SaleBonusModel model)
        {
            #region Validate

            string message = string.Empty;
            string status=string.Empty;
            var baseService = new ActionService(UserLogin);
            var bService = new BaseService();
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_SaleBonusModel>(model, "Eva_SaleBonus", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            #region kt trùng dữ liệu với Jobttile,KPI,Type theo mức phần %
            List<object> lst=new List<object>();
            var lstSaleBonus = bService.GetDataNotParam<Eva_SaleBonusEntity>(ConstantSql.hrm_eva_getdata_SaleBonus,UserLogin, ref status).ToList();
            if (lstSaleBonus != null && lstSaleBonus.Count != 0)
            {
                if (model.ID == Guid.Empty)
                {
                    if (lstSaleBonus.Where(x => x.JobTittleID == model.JobTittleID && x.SalesTypeID == model.SalesTypeID && x.Type == model.Type && x.FromPercent <= model.ToPercent && x.ToPercent >= model.FromPercent).Count() > 0)
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                        return model;
                    }
                }
                else
                {
                    lstSaleBonus = lstSaleBonus.Where(x => x.ID != model.ID).ToList();
                    if (lstSaleBonus.Where(x => x.JobTittleID == model.JobTittleID && x.SalesTypeID == model.SalesTypeID && x.Type == model.Type && x.FromPercent <= model.ToPercent && x.ToPercent >= model.FromPercent).Count() > 0)
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                        return model;
                    }
                }
 
            }

            #endregion
            model.ActionStatus = "Success";
            var service = new ActionService(UserLogin);

            return service.UpdateOrCreate<Eva_SaleBonusEntity, Eva_SaleBonusModel>(model);
        }
        
	}
}