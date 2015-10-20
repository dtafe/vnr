using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_UnusualAllowanceCfgController : ApiController
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
        string status = string.Empty;
        
        // GET api/<controller>/5
        public Cat_UnusualAllowanceCfgModel Get(Guid id)
        {
            var profileName = string.Empty;
            var service = new Cat_UnusualAllowanceCfgServices();
          
            ActionService actionService = new ActionService(UserLogin);
            var result = actionService.GetByIdUseStore<Cat_UnusualAllowanceCfgEntity>(id, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfgId, ref status);
            
            if (result == null)
            {
                return new Cat_UnusualAllowanceCfgModel();
            }
            
            var cat_UnsualAllowanceCfgEntity = new Cat_UnusualAllowanceCfgModel
            {
                ID = result.ID,
                Code = result.Code,
                Comment = result.Comment,
                UnusualAllowanceCfgName = result.UnusualAllowanceCfgName,
                IsAddToHourlyRate = result.IsAddToHourlyRate,
                IsChargePIT = result.IsChargePIT,
                IsExcludePayslip = result.IsExcludePayslip,
                MethodCalculation = result.MethodCalculation,
                Formula = result.Formula,
                EDType = result.EDType,
                Amount = result.Amount,
                Type = result.Type
            };
            return cat_UnsualAllowanceCfgEntity;

        }
        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_UnusualAllowanceCfgEntity")]
        public Cat_UnusualAllowanceCfgModel Post([Bind]Cat_UnusualAllowanceCfgModel catAllowanceCfg)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UnusualAllowanceCfgModel>(catAllowanceCfg, "Cat_UnusualAllowanceCfg", ref message);
            if (!checkValidate)
            {
                catAllowanceCfg.ActionStatus = message;
                return catAllowanceCfg;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UnusualAllowanceCfgEntity, Cat_UnusualAllowanceCfgModel>(catAllowanceCfg);
            //var success = string.Empty;
         
            //var model = new Cat_UnusualAllowanceCfgEntity
            //{
            //    ID = catAllowanceCfg.ID,
            //    Code = catAllowanceCfg.Code,
            //    Comment = catAllowanceCfg.Comment,
            //    UnusualAllowanceCfgName = catAllowanceCfg.UnusualAllowanceCfgName,
            //    IsAddToHourlyRate = catAllowanceCfg.IsAddToHourlyRate,
            //    IsChargePIT = catAllowanceCfg.IsChargePIT,
            //    IsExcludePayslip = catAllowanceCfg.IsExcludePayslip,
            //    MethodCalculation = catAllowanceCfg.MethodCalculation,
            //    Formula = catAllowanceCfg.Formula,
            //    EDType = catAllowanceCfg.EDType,
            //    Amount = catAllowanceCfg.Amount
            //};

            //var service = new Cat_UnusualAllowanceCfgServices();

            //if (catAllowanceCfg.ID != Guid.Empty)
            //{
            //    model.ID = catAllowanceCfg.ID;
            //    success = service.Edit<Cat_UnusualAllowanceCfgEntity>(model) + ",0";
            //}
            //else
            //{
            //    success = service.Add<Cat_UnusualAllowanceCfgEntity>(model) + ",1";
            //}
            //return success;
        }

        // DELETE api/<controller>/5
        public void Delete(Guid ID)
        {
            var service = new Cat_UnusualAllowanceCfgServices();
            var result = service.Remove<Cat_UnusualAllowanceCfgEntity>(ID);
        }
    }
}
