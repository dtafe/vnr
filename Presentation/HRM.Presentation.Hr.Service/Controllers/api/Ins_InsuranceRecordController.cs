using System;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Insurance.Domain;
using HRM.Presentation.Insurance.Models;
using HRM.Presentation.Service;
using HRM.Business.Insurance.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Collections.Generic;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Ins_InsuranceRecordController : ApiController
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

        public Ins_InsuranceRecordModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Ins_InsuranceRecordModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Ins_InsuranceRecordEntity>(id, ConstantSql.hrm_ins_sp_get_InsuranceRecordById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Ins_InsuranceRecordModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Ins_InsuranceRecordModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Ins_InsuranceRecordEntity, Ins_InsuranceRecordModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Ins_InsuranceRecord")]
        public Ins_InsuranceRecordModel Post([Bind]Ins_InsuranceRecordModel model)
        {
            var hrService = new Hre_ProfileServices();
            var baseService = new BaseService();
            var InsuranceRecordService = new Ins_InsuranceRecordServices();
            List<Ins_InsuranceRecordEntity> ilInsuranceRecord = new List<Ins_InsuranceRecordEntity>();
            string status = string.Empty;
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Ins_InsuranceRecordModel>(model, "Ins_InsuranceRecord", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            if (model.ID == Guid.Empty)
            {

                if (string.IsNullOrEmpty(model.OrgStructureIDs) && string.IsNullOrEmpty(model.ProfileIds) && model.ProfileID == Guid.Empty)
                {
                    message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("ProfileID").TranslateString());
                    model.ActionStatus = message;
                    return model;
                }
                if (!string.IsNullOrEmpty(model.OrgStructureIDs))
                {
                    List<Guid> listGuid = new List<Guid>();
                    if (model.ProfileIds != null)
                    {
                        var listStr = model.ProfileIds.Split(',');

                        if (listStr[0] != "")
                        {

                            foreach (var item in listStr)
                            {
                                listGuid.Add(Guid.Parse(item));
                            }
                        }
                    }

                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureIDs);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    var lstProfile = baseService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure,UserLogin, ref status).Select(x => x.ID);
                    if (listGuid.Count > 0)
                    {
                        lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
                    }
                    if (lstProfile.Count() > 0)
                    {

                        foreach (var objProfile in lstProfile)
                        {
                            var modelSave = model.CopyData<Ins_InsuranceRecordModel>();
                            modelSave.ProfileID = objProfile;
                            ilInsuranceRecord.Add(modelSave.CopyData<Ins_InsuranceRecordEntity>());

                        }
                        model.ActionStatus =InsuranceRecordService.Add(ilInsuranceRecord);


                    }

                    return model;
                }
                if (!string.IsNullOrEmpty(model.ProfileIds))
                {
                    List<Guid> ilprofileID = new List<Guid>();
                    var listprofile = model.ProfileIds.Split(',');
                    foreach (var x in listprofile)
                    {
                        try
                        {
                            ilprofileID.Add(Guid.Parse(x));
                        }
                        catch
                        {
                        }
                    }
                    foreach (var item in ilprofileID)
                    {
                        var modelSave = model.CopyData<Ins_InsuranceRecordModel>();
                        modelSave.ProfileID = item;
                        ilInsuranceRecord.Add(modelSave.CopyData<Ins_InsuranceRecordEntity>());

                    }
                    model.ActionStatus=InsuranceRecordService.Add(ilInsuranceRecord);
                    

                }
            }
            else
            {
                ActionService service = new ActionService(UserLogin);
                return service.UpdateOrCreate<Ins_InsuranceRecordEntity, Ins_InsuranceRecordModel>(model);
            }
            return model;





        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Ins_InsuranceRecord")]
        public Ins_InsuranceRecordModel Put([Bind]Ins_InsuranceRecordModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Ins_InsuranceRecordModel>(model, "Ins_InsuranceRecord", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Ins_InsuranceRecordEntity, Ins_InsuranceRecordModel>(model);

        }
    }
}
