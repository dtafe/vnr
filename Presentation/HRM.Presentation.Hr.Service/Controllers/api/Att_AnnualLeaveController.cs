using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_AnnualLeaveController : ApiController
    {
        #region MyRegion
            //// GET api/<controller>
            ///// <summary>
            ///// Lấy tất cả dữ liệu
            ///// </summary>
            ///// <returns></returns>
            //public IEnumerable<Att_AnnualLeaveModel> Get()
            //{
            //    var service = new Att_AnnualLeaveServices();
            //    var list = service.Get();

            //    return list.Select(item => 
            //        new Att_AnnualLeaveModel
            //    {
            //        Id = item.Id,
            //        ProfileID = item.ProfileID,
            //        //ProfileName = item.ProfileName,
            //        Year = item.Year,
            //        MonthStart = item.MonthStart,
            //        InitAnlValue = item.InitAnlValue,
            //        InitSickValue = item.InitSickValue,
            //        InitSaveSickValue = item.InitSaveSickValue,
            //        AnlValueLastYear = item.AnlValueLastYear,
            //        ExpireAnlValueLastYear = item.ExpireAnlValueLastYear,
            //        AnlMonthReset = item.AnlMonthReset,
            //        MonthResetAnlOfBeforeYear = item.MonthResetAnlOfBeforeYear
            //    });
            //}

            //// GET api/<controller>/5
            //public Att_AnnualLeaveModel Get(int id)
            //{
            //    var service = new Att_AnnualLeaveServices();
            //    var result = service.Get(id);
            //    var AttAnnualLeave = new Att_AnnualLeaveModel
            //    {
            //        Id = result.Id,
            //        ProfileID = result.ProfileID,
            //        ProfileName = result.ProfileName,
            //        Year = result.Year,
            //        MonthStart = result.MonthStart,
            //        InitAnlValue = result.InitAnlValue,
            //        InitSickValue = result.InitSickValue,
            //        InitSaveSickValue = result.InitSaveSickValue,
            //        AnlValueLastYear = result.AnlValueLastYear,
            //        ExpireAnlValueLastYear = result.ExpireAnlValueLastYear,
            //        AnlMonthReset = result.AnlMonthReset,
            //        MonthResetAnlOfBeforeYear = result.MonthResetAnlOfBeforeYear
            //    };
            //    return AttAnnualLeave;
            //}

            //public Att_AnnualLeaveModel Put(Att_AnnualLeaveModel model)
            //{
            //    var AttAnnualLeave = new Att_AnnualLeave
            //    {
            //        Id = model.Id,
            //        ProfileID = model.ProfileID,
            //        Year = model.Year,
            //        MonthStart = model.MonthStart,
            //        InitAnlValue = model.InitAnlValue,
            //        InitSickValue = model.InitSickValue,
            //        InitSaveSickValue = model.InitSaveSickValue,
            //        AnlValueLastYear = model.AnlValueLastYear,
            //        ExpireAnlValueLastYear = model.ExpireAnlValueLastYear,
            //        AnlMonthReset = model.AnlMonthReset,
            //        MonthResetAnlOfBeforeYear = model.MonthResetAnlOfBeforeYear
            //    };
            //    var service = new Att_AnnualLeaveServices();
            //    if (model.Id != 0)
            //    {
            //        AttAnnualLeave.Id = model.Id;
            //        service.Edit(AttAnnualLeave);
            //    }
            //    else
            //    {
            //        service.Add(AttAnnualLeave);
            //    }

            //    return model;
            //}

            //[System.Web.Mvc.HttpPost]
            //[System.Web.Mvc.RouteAttribute("api/Att_AnnualLeave")]
            //public Att_AnnualLeaveModel Post([FromBody]Att_AnnualLeaveModel annualLeave)
            //{
            //    var model = new Att_AnnualLeave
            //    {
            //        Id = annualLeave.Id,
            //        ProfileID = annualLeave.ProfileID,
            //        Year = annualLeave.Year,
            //        MonthStart = annualLeave.MonthStart,
            //        InitAnlValue = annualLeave.InitAnlValue,
            //        InitSickValue = annualLeave.InitSickValue,
            //        InitSaveSickValue = annualLeave.InitSaveSickValue,
            //        AnlValueLastYear = annualLeave.AnlValueLastYear,
            //        ExpireAnlValueLastYear = annualLeave.ExpireAnlValueLastYear,
            //        AnlMonthReset = annualLeave.AnlMonthReset,
            //        MonthResetAnlOfBeforeYear = annualLeave.MonthResetAnlOfBeforeYear
            //    };
            //    var service = new Att_AnnualLeaveServices();
            //    if (annualLeave.Id != 0)
            //    {
            //        model.Id = annualLeave.Id;
            //        service.Edit(model);
            //    }
            //    else
            //    {
            //        service.Add(model);
            //        annualLeave.Id = model.Id;
            //    }

            //    return annualLeave;
            //}

            //// DELETE api/<controller>/5
            //public Att_AnnualLeaveModel Delete(int id)
            //{
            //    var service = new Att_AnnualLeaveServices();
            //    service.Delete(id);
            //    var result = new Att_AnnualLeaveModel();
            //    return result;
            //}
        #endregion
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
        /// <summary>
        /// [Hieu.Van] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_AnnualLeaveModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_AnnualLeaveModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_AnnualLeaveEntity>(id, ConstantSql.hrm_att_sp_get_AnnualLeaveById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_AnnualLeaveModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_AnnualLeaveModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(id);
            return result;
            //Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            //var result = service.Remove<Att_AnnualLeaveEntity>(id);
            //return new Att_AnnualLeaveModel();
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_AnnualLeave")]
        public Att_AnnualLeaveModel Post([Bind]Att_AnnualLeaveModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_AnnualLeaveModel>(model, "Att_AnnualLeave", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.ID == null || model.ID == Guid.Empty)
            {
                if (string.IsNullOrEmpty(model.OrgStructureID) && string.IsNullOrEmpty(model.ProfileIDs))
                {
                    message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("ProfileID").TranslateString());
                    model.ActionStatus = message;
                    return model;
                }
            }

            #endregion

            ActionService service = new ActionService(UserLogin);
            var baseService = new BaseService();

            if (model.ID != null && model.ID != Guid.Empty)
            {
                return service.UpdateOrCreate<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(model);
            }
           
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> lstMulti = new List<Guid>();
            List<Guid> lstOgrg = new List<Guid>();

            if (!string.IsNullOrEmpty(model.ProfileIDs))
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstMulti.Add(_Id);
                }
            }
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                List<Hre_ProfileEntity> lstOrg = baseService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                lstProfileIDs = lstOrg.Select(d => d.ID).ToList();
                //_profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                //lstProfileIDs.AddRange(_profileIDs);
            }
            lstProfileIDs.AddRange(lstMulti);
            lstProfileIDs = lstProfileIDs.Distinct().ToList();
            #endregion
            if (lstProfileIDs.Count > 0)
            {
                foreach (Guid item in lstProfileIDs)
                {
                    Att_AnnualLeaveModel modelSave = model.CopyData<Att_AnnualLeaveModel>();
                    modelSave.ID = Guid.Empty;
                    modelSave.ProfileID = item;
                    modelSave = service.UpdateOrCreate<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(modelSave);
                    if (modelSave.ActionStatus != NotificationType.Success.ToString())
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error.ToString());
                        return modelSave;
                    }
                    model.ID = modelSave.ID;
                    model.ProfileID = modelSave.ProfileID;
                    model.ActionStatus = modelSave.ActionStatus;
                }
                //model.SetPropertyValue(Constant.ActionStatus, NotificationType.Success.ToString());
                return model;
            }
            else
                return service.UpdateOrCreate<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(model);

            //if (!string.IsNullOrEmpty(model.ProfileIDs))
            //{
            //    string[] arrProfileIds = model.ProfileIDs.Split(',');
            //    foreach (string ProfileId in arrProfileIds)
            //    {
            //        var guiId = Guid.Parse(ProfileId);
            //        Att_AnnualLeaveModel modelSave = model.CopyData<Att_AnnualLeaveModel>();
            //        modelSave.ProfileID = guiId;
            //        modelSave = service.UpdateOrCreate<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(modelSave);
            //        if (modelSave.ActionStatus != NotificationType.Success.ToString())
            //            return modelSave;
            //    }
            //    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Success.ToString());
            //    return model;
            //}
            //else
            //    return service.UpdateOrCreate<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(model);
        }
    }
}