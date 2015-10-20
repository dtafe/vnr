using HRM.Business.Attendance.Domain;
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
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_RosterController : ApiController
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
        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu Lịch Sử Thẻ(Att_Roster) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_RosterModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_RosterModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_RosterEntity>(id, ConstantSql.hrm_att_sp_get_RosterById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_RosterModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Lịch Sử Thẻ(Att_Roster) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_RosterModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_RosterEntity, Att_RosterModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Lịch Sử Thẻ(Att_Roster)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Roster")]
        public Att_RosterModel Post([Bind]Att_RosterModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var baseService = new BaseService();
            string status = string.Empty;
            var hrService = new Hre_ProfileServices();
            string strMessages = string.Empty;
            var rosterService = new Att_RosterServices();
            var sysAllSetttingService = new Sys_AllSettingServices();
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_RosterModel>(model, "Att_Roster", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_RosterModel>(model, "Att_ProfileNotRoster", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (string.IsNullOrEmpty(model.OrgStructureIDs) && string.IsNullOrEmpty(model.ProfileIds) && model.ProfileID == Guid.Empty)
            {
                message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("ProfileID").TranslateString());
                model.ActionStatus = message;
                return model;
            }
            #endregion

            List<Guid> lstProfileIDs = new List<Guid>();

            if(!string.IsNullOrEmpty(model.OrgStructureIDs))
            {
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureIDs);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstProfile = hrService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure,UserLogin, ref status).Select(s => s.ID).ToList();
               lstProfileIDs.AddRange(lstProfile);
            }
            else if (model.ProfileIds != null && model.ProfileIds!="")
            {
               var listGuid = model.ProfileIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
               lstProfileIDs.AddRange(listGuid);
            }
            var lstRosterEntity_Validate = new List<Att_RosterEntity>();
            foreach (var item in lstProfileIDs)
            {
                Att_RosterEntity rosterEntity = model.CopyData<Att_RosterEntity>();
                rosterEntity.ProfileID = item;
                if (rosterEntity.ID != null && rosterEntity.ID!= Guid.Empty)
                {
                }
                else
                {
                    rosterEntity.ID = Guid.NewGuid();
                }
                lstRosterEntity_Validate.Add(rosterEntity);
            }
            var key = AppConfig.HRM_ATT_ALLOWSAVEDUPLICATE.ToString();
            var lstSysAllSetting = sysAllSetttingService.GetData<Sys_AllSettingEntity>(key,ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status).FirstOrDefault();


            var ValidateLess12Hour = AppConfig.HRM_ATT_VALIDATE_ROSTER_NON_CONTINUE_12HOUR.ToString();
            var lstSysSetting_ValidateLess12Hour = sysAllSetttingService.GetData<Sys_AllSettingEntity>(ValidateLess12Hour, ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status).FirstOrDefault();
            if (lstSysSetting_ValidateLess12Hour != null && lstSysSetting_ValidateLess12Hour.Value1==bool.TrueString)
            {
                string Err = string.Empty;
                Err = rosterService.ValidateShiftHourContinue(lstRosterEntity_Validate);
                if (Err != string.Empty)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + Err);
                    return model;
                }
            }

            if (!string.IsNullOrEmpty(model.OrgStructureIDs))
            {
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureIDs);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstProfile = hrService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure,UserLogin, ref status).Select(s => s.ID).ToList();
                if(lstProfile.Count==0 || lstProfile == null)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Info + "," + strMessages + ConstantMessages.FieldDuplicate);
                    return model;
                }
                if(lstProfile != null && model.ProfileIds != null)
                {
                        Guid[] listGuid = null;
                        listGuid = model.ProfileIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
                        lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
                }
                List<Att_RosterEntity> lstRosterEntity = new List<Att_RosterEntity>();
                List<object> paraRoster = new List<object>();
                paraRoster.AddRange(new object[4]);
                paraRoster[0]=(object)model.OrgStructureIDs;
                paraRoster[1]=null;
                paraRoster[2]=null;
                paraRoster[3]=null;
                var listRosterData = rosterService.GetData<Att_RosterEntity>(paraRoster, ConstantSql.hrm_att_getdata_Roster,UserLogin, ref status);
                foreach (var item in lstProfile)
                {
                    Att_RosterModel modelSave = model.CopyData<Att_RosterModel>();
                    var listRoster = listRosterData.Where(s => s.ProfileID == item);
                    if (modelSave.Type != RosterType.E_CHANGE_SHIFT.ToString()
                        && modelSave.Type != RosterType.E_TIME_OFF.ToString() 
                        && listRoster != null 
                        && listRoster.Any(d => d.DateStart <= model.DateEnd && d.DateEnd >= model.DateStart))
                    {
                        strMessages += listRoster.FirstOrDefault().ProfileName + ", ";
                        continue;
                    }
                    modelSave.ProfileID = item;
                    Att_RosterEntity rosterEntity = modelSave.CopyData<Att_RosterEntity>();
                    lstRosterEntity.Add(rosterEntity);
                }
                if (strMessages == "" && lstRosterEntity.Count > 0)
                {
                    model.ActionStatus = rosterService.Add(lstRosterEntity);
                }
                else
                {
                    if (lstSysAllSetting != null && lstSysAllSetting.Value1 == bool.TrueString && lstRosterEntity.Count > 0)
                    {
                        model.ActionStatus = rosterService.Add(lstRosterEntity);
                    }
                    else if (lstSysAllSetting.Value1 == bool.FalseString)
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                        return model;
                    }
                    else if(lstRosterEntity.Count == 0)
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Info + "," + strMessages + ConstantMessages.FieldDuplicate);
                        return model;
                    }
                }
                return model;
            }
            //Xử lý khi chọn nhiều nhân viên
            if (model.ProfileIds != null && model.ProfileIds.IndexOf(',') > 0 && model.OrgStructureIDs == null)
            {
                Guid[] listGuid = null;
                listGuid = model.ProfileIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
                List<Att_RosterEntity> lstRosterEntity = new List<Att_RosterEntity>();
                foreach (var item in listGuid)
                {
                    Att_RosterModel modelSave = model.CopyData<Att_RosterModel>();
                    var listRoster = service.GetData<Att_RosterEntity>(item, ConstantSql.hrm_att_sp_get_RosterByProfileId, ref status);
                    if (modelSave.Type != RosterType.E_CHANGE_SHIFT.ToString()
                        && modelSave.Type != RosterType.E_TIME_OFF.ToString()
                        && listRoster != null
                        && listRoster.Any(d => d.DateStart <= model.DateEnd && d.DateEnd >= model.DateStart))
                    {
                        strMessages += listRoster.FirstOrDefault().ProfileName + ", ";
                        continue;
                    }
                    modelSave.ProfileID = item;
                    Att_RosterEntity rosterEntity = modelSave.CopyData<Att_RosterEntity>();
                    lstRosterEntity.Add(rosterEntity);
                }
                if(strMessages == "")
                {
                    model.ActionStatus = rosterService.Add(lstRosterEntity);
                }
                else
                {
                    if (lstSysAllSetting != null && lstSysAllSetting.Value1 == bool.TrueString)
                    {
                        model.ActionStatus = rosterService.Add(lstRosterEntity);
                    }
                    else
                    {
                        model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                        return model;
                    }
                }
                return model;
            }
            //Xử lý khi chỉ chọn 1 nhân viên
            if(model.ID == Guid.Empty){
                var lstRoster = service.GetData<Att_RosterEntity>(model.ProfileID, ConstantSql.hrm_att_sp_get_RosterByProfileId, ref status);
                if (model.Type != RosterType.E_CHANGE_SHIFT.ToString()
                    && model.Type != RosterType.E_TIME_OFF.ToString()
                    && lstRoster != null 
                    && lstRoster.Any(d => d.DateStart <= model.DateEnd && d.DateEnd >= model.DateStart))
                {
                    strMessages = lstRoster.FirstOrDefault().ProfileName + ",";
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                    return model;
                }
                return service.UpdateOrCreate<Att_RosterEntity, Att_RosterModel>(model);
            }
            return service.UpdateOrCreate<Att_RosterEntity, Att_RosterModel>(model);
        }

        /// <summary>
        /// [Hieu.Van] Update status 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Roster")]
        public Att_RosterModel Put([Bind]Att_RosterModel model)
        {
            model.ProfileIds = Common.DotNetToOracle(model.ProfileIds);
            BaseService service = new BaseService();
            string status = string.Empty;
            var lstCheck = service.GetData<Att_RosterModel>(model.ProfileIds, ConstantSql.hrm_att_sp_get_RosterByIds,UserLogin, ref status);
            foreach (var item in lstCheck)
            {
                if (item.UserApproveID != model.UserApproveID && item.UserApproveID2 != model.UserApproveID2)
                {
                    model.ActionStatus = "NoPermission";
                    return model;
                }
            }

            List<object> lstObj = new List<object>();
            lstObj.Add(model.ProfileIds);
            lstObj.Add(model.Status);
            var rs = service.UpdateData<Att_RosterModel>(lstObj, ConstantSql.hrm_att_sp_Set_Roster_Status, ref status);
            if (rs != null)
            {
                return model;
            }
            return null;
        }

    }
}