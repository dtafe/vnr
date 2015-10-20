using HRM.Business.Canteen.Domain;
using HRM.Business.Canteen.Models;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_MealRecordMissingController : ApiController
    {
        string status = string.Empty;
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
        public IEnumerable<Cat_TAMScanReasonMissMuitlModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
         
            var listEntity = service.GetData<Cat_TAMScanReasonMissMultiEntity>(string.Empty, ConstantSql.hrm_cat_sp_get_TamScanReasonMiss_multi,UserLogin, ref status);

            if (listEntity!=null)
            {
                var listModel = listEntity.Translate<Cat_TAMScanReasonMissMuitlModel>();
                return listModel;
            }
            
            return new List<Cat_TAMScanReasonMissMuitlModel>();
        }

        // GET api/<controller>/5
        public Can_MealRecordMissingModel Get(Guid id)
        {
            var service = new Can_MealRecordMissingServices();
            var result = service.GetData<Can_MealRecordMissingEntity>(id, ConstantSql.hrm_can_sp_get_MealRecordMissingId,UserLogin, ref status).FirstOrDefault();
            var model = new Can_MealRecordMissingModel
            {
                ID = result.ID,
                ProfileID = result.ProfileID,
                TamScanReasonMissID = result.TamScanReasonMissID,
                WorkDate = result.WorkDate,
                Amount = result.Amount,
                Status = result.Status,
                MealAllowanceTypeSettingID = result.MealAllowanceTypeSettingID,
                IsFullPay = result.IsFullPay,
                Type = result.Type,
                EmpCode = result.EmpCode,
                OrgStructureID = result.OrgStructureID,
                OrgStructureName = result.OrgStructureName,
                TamScanReasonMissName = result.TamScanReasonMissName,
                MealAllowanceTypeSettingName = result.MealAllowanceTypeSettingName
            };

            model.ActionStatus = status;
            return model;
        }

        
        public Can_MealRecordMissingModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_MealRecordMissingEntity, Can_MealRecordMissingModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
         [System.Web.Mvc.RouteAttribute("api/Can_MealRecordMissing")]
        public Can_MealRecordMissingModel Post([Bind]Can_MealRecordMissingModel model)
         {

            var mealRecordMissingModel = new Can_MealRecordMissingModel
            {
                ID = model.ID,
                ProfileID = model.ProfileID,
                TamScanReasonMissID = model.TamScanReasonMissID,
                WorkDate = model.WorkDate,
                Amount = model.Amount,
                Status = model.Status,
                MealAllowanceTypeSettingID = model.MealAllowanceTypeSettingID,
                IsFullPay = model.IsFullPay,
                Type= model.Type,
                ActionStatus = model.ActionStatus
            };
            ActionService service = new ActionService(UserLogin);
            if (mealRecordMissingModel.MealAllowanceTypeSettingID != null && mealRecordMissingModel.MealAllowanceTypeSettingID != Guid.Empty)
            {
                string status = string.Empty;
                var mealAllowanceTypeSetting = service.GetByIdUseStore<Can_MealAllowanceTypeSettingEntity>(mealRecordMissingModel.MealAllowanceTypeSettingID.Value,ConstantSql.hrm_can_sp_get_mealallowtypebyid, ref status);
                if (mealAllowanceTypeSetting.Amount!=null)
                    mealRecordMissingModel.Amount = (decimal)mealAllowanceTypeSetting.Amount;
            }
            else if (mealRecordMissingModel.TamScanReasonMissID != null && mealRecordMissingModel.TamScanReasonMissID != Guid.Empty)
            {
                string status = string.Empty;
                var tamScanReasonMiss = service.GetById<Cat_TAMScanReasonMissEntity>(mealRecordMissingModel.TamScanReasonMissID.Value, ref status);
                if (tamScanReasonMiss != null && tamScanReasonMiss.MealAllowanceTypeSettingID != null)
                {
                    var mealAllowanceTypeSetting = service.GetById<Can_MealAllowanceTypeSettingEntity>(tamScanReasonMiss.MealAllowanceTypeSettingID.Value, ref status);
                    mealRecordMissingModel.MealAllowanceTypeSettingID = tamScanReasonMiss.MealAllowanceTypeSettingID;
                    mealRecordMissingModel.Amount =(decimal) mealAllowanceTypeSetting.Amount;
                }
            }
            var data = service.UpdateOrCreate<Can_MealRecordMissingEntity, Can_MealRecordMissingModel>(mealRecordMissingModel);
           
            if (data!=null)
            {

                var dataModel = new Can_MealRecordMissingModel
                {
                    ID = data.ID,
                    ProfileID = data.ProfileID,
                    TamScanReasonMissID = model.TamScanReasonMissID,
                    WorkDate = data.WorkDate,
                    Amount = data.Amount,
                    Status = data.Status,
                    MealAllowanceTypeSettingID = data.MealAllowanceTypeSettingID,
                    IsFullPay = data.IsFullPay,
                    Type = data.Type,
                    EmpCode = data.EmpCode,
                    OrgStructureID = data.OrgStructureID,
                    OrgStructureName = data.OrgStructureName,
                    TamScanReasonMissName = data.TamScanReasonMissName,
                    MealAllowanceTypeSettingName = data.MealAllowanceTypeSettingName,
                    ActionStatus = data.ActionStatus
                };
                return dataModel;
            }
            return null;
        }
    }
}
