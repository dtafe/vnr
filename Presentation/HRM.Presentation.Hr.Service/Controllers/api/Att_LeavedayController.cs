using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
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
using HRM.Business.Main.Domain;
using HRM.Business.Attendance.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Models;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_LeavedayController : ApiController
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

        public Att_LeaveDayModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_LeaveDayModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_LeaveDayEntity>(id, ConstantSql.hrm_att_sp_get_LeaveDayById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_LeaveDayModel>();
                if (model.LeaveHours != null && model.LeaveHours > 0)
                {
                    if (model.DurationType == LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString() || model.DurationType == LeaveDayDurationType.E_LASTHALFSHIFT.ToString())
                    {
                        var guiId = model.ProfileID;
                        var start = model.DateStart;
                        var end = model.DateEnd;
                        if (guiId != null && guiId != Guid.Empty && start != null && end != null)
                        {
                            var listRoster = service.GetData<Att_RosterEntity>(guiId, ConstantSql.hrm_att_sp_get_RosterByProfileId, ref status);
                            if (listRoster != null)
                            {
                                for (DateTime i = start; i <= end; i = i.AddDays(1))
                                {
                                    var roster = listRoster.Where(d => d.DateStart <= i && d.DateEnd >= i).FirstOrDefault();
                                    if (roster != null)
                                    {
                                        var shift = SearchShift(roster, i);
                                        if (shift != null && shift != Guid.Empty)
                                        {
                                            var catShift = service.GetByIdUseStore<Cat_ShiftEntity>((Guid)shift, ConstantSql.hrm_cat_sp_get_ShiftById, ref status);
                                            if (catShift != null)
                                            {
                                                if (model.DurationType == LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString())
                                                {
                                                    model.HoursFrom = catShift.InTime;
                                                    model.HoursTo = catShift.InTime.AddHours(catShift.CoBreakOut);
                                                    break;
                                                }
                                                else
                                                {
                                                    model.HoursFrom = catShift.InTime.AddHours(catShift.CoBreakIn);
                                                    model.HoursTo = catShift.InTime.AddHours(catShift.CoOut);
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            model.ActionStatus = status;
            return model;

        }
        /// <summary>
        /// Tìm ca làm việc trong Roster
        /// </summary>
        /// <param name="roster"></param>
        /// <param name="workday"></param>
        /// <returns></returns>
        private Guid? SearchShift(Att_RosterEntity roster, DateTime workday)
        {
            if (roster != null)
            {
                //Lấy ra thứ của WorkDate và tìm ca của thứ tương ứng
                switch (workday.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return roster.MonShiftID;
                    case DayOfWeek.Tuesday:
                        return roster.TueShiftID;
                    case DayOfWeek.Wednesday:
                        return roster.WedShiftID;
                    case DayOfWeek.Thursday:
                        return roster.ThuShiftID;
                    case DayOfWeek.Friday:
                        return roster.FriShiftID;
                    case DayOfWeek.Saturday:
                        return roster.SatShiftID;
                    case DayOfWeek.Sunday:
                        return roster.SunShiftID;
                }
            }

            return null;
        }
        public Att_LeaveDayModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_LeaveDayEntity, Att_LeaveDayModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Leaveday")]
        public Att_LeaveDayModel Post([Bind]Att_LeaveDayModel model)
        {
            string status = "";
            string message = string.Empty;
            //truong hop edit 1 nhan vien
            if (model.ProfileID != Guid.Empty)
            {
                model.ProfileIds = model.ProfileID.ToString();
            }
            BaseService baseService = new BaseService();
            List<Guid> lstProfileIDs = new List<Guid>();
            #region Xu ly lay lst nhan vien cuoi cung
            //xu ly chon nhan vien hay chon phong ban
            if (!string.IsNullOrEmpty(model.ProfileIds))
            {
                List<Guid> lstMulti = new List<Guid>();
                var lst = model.ProfileIds.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstMulti.Add(_Id);
                }
                lstProfileIDs = lstMulti;
            }
            else
            {
                if (model.strOrgStructureIDs == "")
                {
                    model.strOrgStructureIDs = null;
                }
                List<object> lstObj = new List<object>();
                lstObj.Add(model.strOrgStructureIDs);
                lstObj.Add(null);
                lstObj.Add(null);
                List<Hre_ProfileEntity> lstOrg = baseService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                lstProfileIDs = lstOrg.Select(d => d.ID).ToList();
                //neu co loai tru
                if(!string.IsNullOrEmpty(model.ProfileIDsExclude) && lstProfileIDs.Count>0)
                {
                    List<Guid> lstProfileIDsExclude = new List<Guid>();
                    var lst = model.ProfileIDsExclude.Split(',');
                    foreach (var item in lst)
                    {
                        Guid _Id = new Guid(item);
                        lstProfileIDsExclude.Add(_Id);
                    }
                    lstProfileIDs = lstProfileIDs.Where(d => !lstProfileIDsExclude.Contains(d)).ToList();
                }
            }
            if (lstProfileIDs.Count()==0)
            {
                message = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("ProfileID").TranslateString());
                model.ActionStatus = message;
                return model;
            }
            #endregion
            if (model.HoursFrom != null)
            {
                model.DateStart = new DateTime(model.DateStart.Year, model.DateStart.Month, model.DateStart.Day, model.HoursFrom.Value.Hour, model.HoursFrom.Value.Minute, 0);

            }
            if (model.HoursTo != null)
            {
                model.DateEnd = new DateTime(model.DateStart.Year, model.DateStart.Month, model.DateStart.Day, model.HoursTo.Value.Hour, model.HoursTo.Value.Minute, 0);
            }
            #region Validate

         
            var checkValidate = true;
            if(model.IsPortal ==true)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_LeaveDayModel>(model, "Att_LeaveDayPortal", ref message);
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_LeaveDayModel>(model, "Att_LeaveDay", ref message);
            }
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.LeaveDays == 0 && model.LeaveHours == 0)
            {
                model.ActionStatus = ConstantMessages.AccessDenied.TranslateString();
                return model;
            }
            #endregion



            //TimeSpan ts = new TimeSpan(model.HoursFrom.Value.Hour, model.HoursFrom.Value.Minute, 0);
            //model.DateStart = model.DateStart + ts;


            if (model.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString())
            {
                if (model.HoursFrom.HasValue)
                    model.DateStart = new DateTime(model.DateStart.Year, model.DateStart.Month, model.DateStart.Day, model.HoursFrom.Value.Hour, model.HoursFrom.Value.Minute, model.HoursFrom.Value.Second);
                if (model.HoursTo.HasValue)
                    model.DateEnd = new DateTime(model.DateEnd.Year, model.DateEnd.Month, model.DateEnd.Day, model.HoursTo.Value.Hour, model.HoursTo.Value.Minute, model.HoursTo.Value.Second);

                if (model.DurationType == LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString() || model.DurationType == LeaveDayDurationType.E_LASTHALFSHIFT.ToString())
                {
                    model.LeaveDays = 0.5;
                }
            }

            ActionService service = new ActionService(UserLogin);
            List<object> lstobject = new List<object>();
            lstobject.AddRange(new object[11]);
            //lstobject[4] = DateTime.MinValue; //DateFrom
            //lstobject[5] = DateTime.MaxValue; //DateTo
            lstobject[9] = 1;
            //lstobject[10] = int.MaxValue;
            lstobject[10] = Int32.MaxValue - 1;
      
            string strMessages = "";
            var _catLeaveDay = new Att_LeavedayServices();
            List<Att_LeaveDayEntity> lstLeaveDayInDbUpdate = _catLeaveDay.GetData<Att_LeaveDayEntity>(lstobject, ConstantSql.hrm_att_sp_get_Leaveday, UserLogin, ref status);

            #region [Hien.Nguyen] - Kiểm tra trùng khi đăng kí cùng một ngày và cùng loại ngày nghỉ

            //if (lstLeaveDayInDbUpdate != null)
            //{
            //    if (model.ID == Guid.Empty)
            //    {
            //        if (lstLeaveDayInDbUpdate.Any(m => m.ProfileID == model.ProfileID && m.DateStart <= model.DateEnd && m.DateEnd >= model.DateStart && m.LeaveDayTypeID == model.LeaveDayTypeID))
            //        {
            //            model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
            //            return model;
            //        }
            //    }
            //    else
            //    {
            //        var leaveDay = _catLeaveDay.GetData<Att_LeaveDayEntity>(model.ID, ConstantSql.hrm_att_sp_get_LeaveDayById, ref status).FirstOrDefault();
            //        if (leaveDay != null)
            //        {
            //            if (leaveDay.LeaveDayTypeID != model.LeaveDayTypeID || leaveDay.DateStart != model.DateStart || leaveDay.DateEnd != model.DateEnd || leaveDay.ProfileID != model.ProfileID)
            //            {
            //                if (lstLeaveDayInDbUpdate.Any(m => m.ProfileID == model.ProfileID && m.DateStart <= model.DateEnd && m.DateEnd >= model.DateStart && m.LeaveDayTypeID == model.LeaveDayTypeID))
            //                {
            //                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
            //                    return model;
            //                }
            //            }
            //        }
            //    }
            //}

            #endregion

            #region Kiểm tra đăng ký trùng 1 ngày chỉ 1 loại ngày nghỉ nếu là Loại Toàn Ca, Còn lại thì 2 loại ngày nghỉ
            if (model.ID != Guid.Empty)//nếu là chỉnh sửa thì loại bỏ dòng đó ra khỏi list tổng
            {
                lstLeaveDayInDbUpdate = lstLeaveDayInDbUpdate.Where(m => m.ID != model.ID).ToList();
            }
            if (model.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())//Toàn ca chỉ được dk 1 dòng
            {
                if (lstLeaveDayInDbUpdate.Any(m => lstProfileIDs.Contains(m.ProfileID) && m.DateStart <= model.DateEnd && m.DateEnd >= model.DateStart))
                {
                    //model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                    return model;
                }
            }
            else//Ngược lại thì đk 2 dòng
            {
                if (lstLeaveDayInDbUpdate.Any(m => lstProfileIDs.Contains(m.ProfileID) && m.DateStart <= model.DateEnd && m.DateEnd >= model.DateStart && m.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString()))
                {
                    //  model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                    return model;
                }
                else if (lstLeaveDayInDbUpdate.Where(m => lstProfileIDs.Contains(m.ProfileID) && m.DateStart <= model.DateEnd && m.DateEnd >= model.DateStart && m.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString()).Count() > 1)
                {
                    // model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                    return model;
                }
            }


            #endregion

            #region Triet.Mai Kiểm tra vượt giới hạn cho 1 tháng
            //string[] ProfileArr = model.ProfileIds.Split(',');
            //List<Guid> lstProfileID = new List<Guid>();
            //foreach (var item in lstProfileIDs)
            //{
            //    Guid ProfileID = item;
            //    //Guid.TryParse(item, out ProfileID);
            //    if (ProfileID != Guid.Empty)
            //        lstProfileID.Add(ProfileID);
            //}
            var LeaveDayEntity = model.CopyData<Att_LeaveDayEntity>();
            string Error = (new Att_LeavedayServices()).ValidateOverDayInMonth(LeaveDayEntity, lstProfileIDs);
            if (Error != string.Empty)
            {
                model.SetPropertyValue(Constant.ActionStatus, Error);
                return model;
            }
            #endregion

            List<Att_LeaveDayEntity> lstSM = new List<Att_LeaveDayEntity>();
            Att_ProcessApprovedServices smService = new Att_ProcessApprovedServices();

            if (!string.IsNullOrEmpty(model.ProfileIds))
            {
            
                //string[] arrProfileIds = model.ProfileIds.Split(',');
                strMessages = "";
                if (strMessages != "")
                {
                    // model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + string.Format(ConstantMessages.FieldDuplicate.TranslateString(), ConstantDisplay.HRM_Evaluation_Information.TranslateString()));
                    return model;
                }
                else
                {
                    var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                    var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(model.LeaveDayTypeID, ConstantSql.hrm_cat_sp_get_LeaveDayTypeById, UserLogin, ref status).FirstOrDefault();


                    if (lstProfileIDs.Count()==1)
                    {

                        var guiId = lstProfileIDs[0];
                        Att_LeaveDayModel modelSave = model.CopyData<Att_LeaveDayModel>();
                        modelSave.ProfileID = guiId;
                        #region Kiểm tra Tổng Số Ngày Nghỉ
                        if (lstLeavedayType != null)
                        {
                            if (model.LeaveDays > lstLeavedayType.MaxPerTimes)
                            {
                                modelSave.ActionStatus = "ErrorTotalLeave";
                                return modelSave;
                            }
                            if (model.LeaveDays > lstLeavedayType.MaxPerYear)
                            {
                                modelSave.ActionStatus = "ErrorTotalLeave";
                                return modelSave;
                            }
                        }
                        #endregion

                        modelSave = service.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(modelSave);
                        if (modelSave.ActionStatus == NotificationType.Success.ToString())
                        {
                            lstSM.Add(modelSave.Copy<Att_LeaveDayEntity>());
                        }
                        smService.GetEmailToSend_LeaveDay(lstSM, UserLogin);
                        return modelSave;
                    }
                  
                    foreach (var ProfileId in lstProfileIDs)
                    {
                        var guiId = ProfileId;
                        Att_LeaveDayModel modelSave = model.CopyData<Att_LeaveDayModel>();
                        modelSave.ProfileID = guiId;

                        #region Kiểm tra Tổng Số Ngày Nghỉ
                        if (lstLeavedayType != null)
                        {
                            if (model.LeaveDays > lstLeavedayType.MaxPerTimes)
                            {
                                modelSave.ActionStatus = "ErrorTotalLeave";
                                return modelSave;
                            }
                            if (model.LeaveDays > lstLeavedayType.MaxPerYear)
                            {
                                modelSave.ActionStatus = "ErrorTotalLeave";
                                return modelSave;
                            }
                        }
                        #endregion

                        modelSave = service.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(modelSave);
                        if (modelSave.ActionStatus == NotificationType.Success.ToString())
                        {
                            lstSM.Add(modelSave.Copy<Att_LeaveDayEntity>());
                        }
                        else
                        {
                            smService.GetEmailToSend_LeaveDay(lstSM, UserLogin);
                            return modelSave;
                        }
                    }
                }
                smService.GetEmailToSend_LeaveDay(lstSM, UserLogin);

                model.SetPropertyValue(Constant.ActionStatus, NotificationType.Success.ToString());
                return model;
            }
            else
            {
                //Đã kiểm tra ở trên , nên không cần kt dưới này - [hien.nguyen]
                //if (lstLeaveDayInDbUpdate != null && lstLeaveDayInDbUpdate.Any(d => d.ID != model.ID && d.ProfileID == model.ProfileID && d.DateStart <= model.DateEnd && d.DateEnd >= model.DateStart))
                //{
                //    strMessages = lstLeaveDayInDbUpdate.Where(d => d.ProfileID == model.ProfileID).FirstOrDefault().ProfileName;
                //    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + strMessages + ConstantMessages.FieldDuplicate);
                //    return model;
                //}
                //else
                model = service.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(model);
                if (model.ActionStatus == NotificationType.Success.ToString())
                {
                    lstSM.Add(model.Copy<Att_LeaveDayEntity>());
                    smService.GetEmailToSend_LeaveDay(lstSM, UserLogin);
                }
                return model;
            }
        }

        /// <summary>
        /// [Hieu.Van] Update status 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Leaveday")]
        public Att_LeaveDayModel Put([Bind]Att_LeaveDayModel model)
        {
            model.ProfileIds = Common.DotNetToOracle(model.ProfileIds);

            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.AddRange(new object[2]);
            var lstCheck = service.GetData<Att_LeaveDayModel>(model.ProfileIds, ConstantSql.hrm_att_sp_get_LeavedayByIds, UserLogin, ref status);
            string lstFirstApprove = "";
            string lstNormal = "";
            foreach (var item in lstCheck)
            {
                if (item.UserApproveID != model.UserApproveID && item.UserApproveID2 != model.UserApproveID2)
                {
                    model.ActionStatus = "NoPermission";
                    return model;
                }
                if (item.UserApproveID2 == model.UserApproveID2 && (item.Status == AttendanceDataStatus.E_FIRST_APPROVED.ToString() || item.UserApproveID == model.UserApproveID))
                {
                    lstFirstApprove += Common.DotNetToOracle(item.ID.ToString()) + ",";
                }
                else
                {
                    lstNormal += Common.DotNetToOracle(item.ID.ToString()) + ",";
                }
            }
            if (lstFirstApprove != "")
            {
                lstFirstApprove = lstFirstApprove.Substring(0, lstFirstApprove.Length - 1);
                lstObj[0] = lstFirstApprove;
                lstObj[1] = model.Status;
                service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, ref status);
            }
            if (lstNormal != "")
            {
                lstNormal = lstNormal.Substring(0, lstNormal.Length - 1);
                lstObj[0] = lstNormal;
                lstObj[1] = model.Status;
                if (model.Status == AttendanceDataStatus.E_APPROVED.ToString())
                    lstObj[1] = AttendanceDataStatus.E_FIRST_APPROVED.ToString();
                service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, ref status);
            }
            if (status == "")
                status = "NoPermission";
            //foreach (var item in lstCheck)
            //{
            //    if (item.UserApproveID != model.UserApproveID && item.UserApproveID2 != model.UserApproveID2)
            //    {
            //        model.ActionStatus = "NoPermission";
            //        return model;
            //    }
            //}
            //lstObj.Add(model.ProfileIds);
            //lstObj.Add(model.Status);
            //var rs = service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, ref status);
            model.ActionStatus = status;
            return model;
        }

    }
}