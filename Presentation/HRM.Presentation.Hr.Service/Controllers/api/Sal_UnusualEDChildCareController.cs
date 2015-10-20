using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using System.Data;
using HRM.Business.Hr.Domain;
using System.Collections.Generic;
using HRM.Business.Hr.Models;
using System.Collections;
using System.Linq;
namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_UnusualEDChildCareController : ApiController
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
        // GET: /Sal_UnusualEDChildCare/
        public Sal_UnusualAllowanceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_UnusualAllowanceModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_UnusualAllowanceEntity>(id, ConstantSql.hrm_sal_sp_get_UnusualEDChildCareById, ref status);
            if (entity != null)
            {
                model = entity.Copy<Sal_UnusualAllowanceModel>();
            }
            model.ActionStatus = status;
            return model;
        }
        public Sal_UnusualAllowanceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(id);
            return result;
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
            DateTime now = model.DateSubmitDoc.Value;
            int age = now.Year - model.MonthStart.Value.Year;
            if (now < model.MonthStart.Value.AddYears(age)) age--;
            if (age > 18 || model.DateSubmitDoc.Value > model.MonthEnd)
            {
                model.ActionStatus = ConstantMessages.WarningRelativeGreaterThan18.TranslateString();
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
                    if (model.DateSubmitDoc != null)
                    {
                        var checkDob = (dateNow.Year - model.DateSubmitDoc.Value.Year) * 12 + dateNow.Month - model.DateSubmitDoc.Value.Month;
                        if (checkDob >= 216)
                        {
                            model.ActionStatus = ConstantMessages.WarningRelativeGreaterThan18.ToString().TranslateString();
                            return model;
                        }
                    }

                    //if(!string.IsNullOrEmpty(entity.YearOfBirth))
                    //{

                    //    var strDob = entity.YearOfBirth.Split('/');
                    ////    var dob = DateTime.Parse(entity.YearOfBirth);
                    //    var checkDob = (dateNow.Year - int.Parse(strDob[2])) * 12 + dateNow.Month - int.Parse(strDob[1]);
                    //    if (checkDob >= 216)
                    //    {
                    //        model.ActionStatus = ConstantMessages.WarningRelativeGreaterThan18.ToString().TranslateString();
                    //        return model;
                    //    }
                    //}   
                }
            }

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(model);
        }
    }
}