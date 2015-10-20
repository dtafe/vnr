using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using System;
using HRM.Presentation.Payroll.Models;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using HRM.Business.Payroll.Models;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_RelativesCustomController : ApiController
    {
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
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_UnusualEDChildCare")]
        public Sal_UnusualAllowanceModel Post([Bind]Sal_UnusualAllowanceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualEDChildCareInfo", "Sal_UnusualAllowance", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.RelativeID != null)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualEDChildCareDuplicate", "Sal_UnusualAllowance", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceModel>(model, "Sal_UnusualEDChildCareDuplicate1", "Sal_UnusualAllowance", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            DateTime now = model.MonthEnd.Value;
            int age = now.Year - model.MonthStart.Value.Year;
            if (now < model.MonthStart.Value.AddYears(age)) age--;
            if (age > 18)
            {
                model.ActionStatus = "Con nhân viên không được vượt quá 18 tuổi";
                return model;
            }

            #endregion

            string status = string.Empty;
            var relativeServices = new Hre_RelativesServices();
            var objRelatives = new List<object>();
            objRelatives.AddRange(new object[11]);
            objRelatives[9] = 1;
            objRelatives[10] = int.MaxValue - 1;
            var lstRelatives = relativeServices.GetData<Hre_RelativesEntity>(objRelatives, ConstantSql.hrm_hr_sp_get_Relatives, UserLogin, ref status).ToList();
            if (model.RelativeID != null)
            {
                var dateNow = DateTime.Now;
                var entity = lstRelatives.Where(s => s.ID == model.RelativeID).FirstOrDefault();
                if (entity != null)
                {
                    if (!string.IsNullOrEmpty(entity.YearOfBirth))
                    {
                        var strDob = entity.YearOfBirth.Split('/');
                        //    var dob = DateTime.Parse(entity.YearOfBirth);
                        var checkDob = (dateNow.Year - int.Parse(strDob[2])) * 12 + dateNow.Month - int.Parse(strDob[1]);
                        if (checkDob >= 216)
                        {
                            model.ActionStatus = ConstantMessages.WarningRelativeGreaterThan18.ToString().TranslateString();
                            return model;
                        }
                    }
                }
            }

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(model);
        }
    }
}
