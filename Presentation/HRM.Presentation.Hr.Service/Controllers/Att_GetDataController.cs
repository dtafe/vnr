using HRM.Business.Attendance.Domain;
using HRM.Business.Main.Domain;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Domain;
using System.Reflection;
using System.Collections;
using System.Data.SqlTypes;
using HRM.Business.Attendance.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
//
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.HrmSystem.Domain;
using System.Threading.Tasks;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Utility;

using Microsoft.Ajax.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Att_GetDataController : BaseController
    {
        string hrm_Att_Service_Path_Report = ConstantPathWeb.Hrm_Att_Service_Path_Report;
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];

        #region Att_Pregnancy

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu bảng Thai Sản(Att_Pregnancy) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPregnancyList([DataSourceRequest] DataSourceRequest request, Att_PregnancySearchModel model)
        {
            return GetListDataAndReturn<Att_PregnancyModel, Att_PregnancyEntity, Att_PregnancySearchModel>(request,
                model, ConstantSql.hrm_att_sp_get_Pregnancy);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất danh sách dữ liệu cho bảng Thai Sản(Att_Pregnancy) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportPregnancyList([DataSourceRequest] DataSourceRequest request, Att_PregnancySearchModel model)
        {
            return ExportAllAndReturn<Att_PregnancyEntity, Att_PregnancyModel, Att_PregnancySearchModel>(request, model,
                ConstantSql.hrm_att_sp_get_Pregnancy);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất dữ liệu được chọn cho bảng Thai Sản(Att_Pregnancy)
        /// </summary>
        public ActionResult ExportPregnancySelected(List<Guid> selectedIds, string valueFields)
        {
            //selectedIds = Common.DotNetToOracle(selectedIds);
            return ExportSelectedAndReturn<Att_PregnancyEntity, Att_PregnancyModel>(String.Join(",", selectedIds), valueFields,
                ConstantSql.hrm_att_sp_get_PregnancyByIds);
        }



        #endregion

        #region Att_RosterGroup

        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng RosterGroup (Att_RosterGroup) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRosterGroupList([DataSourceRequest] DataSourceRequest request, Att_RosterGroupSearchModel model)
        {
            return GetListDataAndReturn<Att_RosterGroupModel, Att_RosterGroupEntity, Att_RosterGroupSearchModel>(request,
                model, ConstantSql.hrm_att_sp_get_RosterGroup);
        
        }

        [HttpPost]
        public ActionResult GetChangeRosterGroupList([DataSourceRequest] DataSourceRequest request, Att_RosterGroupSearchModel model)
        {

            string status = string.Empty;
            ActionService services = new ActionService(UserLogin);
            var objRosterGroup = new List<object>();
            objRosterGroup.AddRange(new object[6]);
            objRosterGroup[4] = 1;
            objRosterGroup[5] = int.MaxValue - 1;

            var lstRosterGroup = services.GetData<Att_RosterGroupModel>(objRosterGroup, ConstantSql.hrm_att_sp_get_RosterGroup,ref status).ToList();
            var lstResult = new List<Att_RosterGroupModel>();
            foreach (var item in lstRosterGroup)
            {
                if(item.DateStart != null)
                {
                    item.Month = new DateTime(item.DateStart.Value.Year, item.DateStart.Value.Month, 1);
                    lstResult.Add(item);
                }
             
            }

            if (lstResult.Count > 0)
            {
                lstResult = lstResult.DistinctBy(s => s.Month).ToList();
            }
            return Json(lstResult.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
            //return GetListDataAndReturn<Att_RosterGroupModel, Att_RosterGroupEntity, Att_RosterGroupSearchModel>(request,
            //    model, ConstantSql.hrm_att_sp_get_RosterGroup);
        }


        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho bảng RosterGroup (Att_RosterGroup) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportRosterGroupList([DataSourceRequest] DataSourceRequest request, Att_RosterGroupSearchModel model)
        {
            return ExportAllAndReturn<Att_RosterGroupEntity, Att_RosterGroupModel, Att_RosterGroupSearchModel>(request, model,
                ConstantSql.hrm_att_sp_get_RosterGroup);
        }

        /// <summary>
        /// [Son.Vo] - Xuất dữ liệu được chọn cho bảng RosterGroup (Att_RosterGroup)
        /// </summary>
        public ActionResult ExportRosterGroupSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_RosterGroupEntity, Att_RosterGroupModel>(String.Join(",", selectedIds), valueFields,
                ConstantSql.hrm_att_sp_get_RosterGroupByIds);
        }


        #endregion

        #region Att_Roster

        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng Roster (Att_Roster) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRosterList([DataSourceRequest] DataSourceRequest request, Att_RosterSearchModel model)
        {
            if (model.DateEnd != null)
                model.DateEnd = model.DateEnd.Value.Date.AddDays(1).AddMilliseconds(-1);

            return GetListDataAndReturn<Att_RosterModel, Att_RosterEntity, Att_RosterSearchModel>(request,
                model, ConstantSql.hrm_att_sp_get_Roster);
        }
        public ActionResult GetProfileByOrgId(string orgs, string parameter)
        {
            var listStr = parameter.Split(',');
            List<Guid> listGuid = new List<Guid>();
            if (listStr[0] != "")
            {

                foreach (var item in listStr)
                {
                    listGuid.Add(Guid.Parse(item));
                }
            }
            string status = string.Empty;
            var hrService = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.Add(orgs);
            listObj.Add(string.Empty);
            listObj.Add(string.Empty);

            //var lstprofile = hrService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            var data = hrService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            if (data != null)
            {
                var dataSelected = data.Select(d => new { d.ID, d.CodeEmp }).Where(d => !listGuid.Contains(d.ID)).ToList();
                var strReturn = string.Join(",", dataSelected.Select(d => d.CodeEmp));
                object[] re = new object[] { dataSelected.Count, strReturn };
                return Json(re);
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng Roster (Att_Roster) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileNotRosterList([DataSourceRequest] DataSourceRequest request, Att_SearchProfileNotRosterModel model)
        {

            Att_RosterServices repo = new Att_RosterServices();
            var hrService = new ActionService(UserLogin);
            DateTime? dateFrom = SqlDateTime.MinValue.Value;
            DateTime? dateTo = SqlDateTime.MinValue.Value;
            if (model.DateStart != null)
            {
                dateFrom = model.DateStart.Value.Date;
            }


            if (model.DateEnd != null)
            {
                dateTo = model.DateEnd.Value.Date;
            }
            string status = string.Empty;
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Hre_ProfileEntity> lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();



            var result = repo.CheckRoster(dateFrom, dateTo, lstProfileIDs, model.isNotRoster, model.isDuplicateRoster, model.isConstantRoster).ToList().Translate<Att_ProfileModel>();
            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.Count;
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);

            //return GetListDataAndReturn<Att_ProfileModel, Att_ProfileEntity, Att_SearchProfileNotRosterModel>(request,
            //    model, ConstantSql.hrm_att_sp_get_Roster_ProfileNotRoster);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho bảng Roster (Att_Roster) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportRosterList([DataSourceRequest] DataSourceRequest request, Att_RosterSearchModel model)
        {
            model.ValueFields = model.ValueFields.Replace("Status,", "StatusTranslate,");
            model.ValueFields = model.ValueFields.Replace("Type,", "TypeTranslate,");

            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var listModel = GetListData<Att_RosterModel, Att_RosterEntity, Att_RosterSearchModel>(request, model, ConstantSql.hrm_att_sp_get_Roster, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
            //return ExportAllAndReturn<Att_RosterEntity, Att_RosterModel, Att_RosterSearchModel>(request, model,
            //    ConstantSql.hrm_att_sp_get_Roster);
        }

        /// <summary>
        /// [Son.Vo] - Xuất dữ liệu được chọn cho bảng Roster (Att_Roster)
        /// </summary>
        public ActionResult ExportRosterSelected(List<Guid> selectedIds, string valueFields)
        {
            valueFields = valueFields.Replace("Status,", "StatusTranslate,");
            valueFields = valueFields.Replace("Type,", "TypeTranslate,");

            return ExportSelectedAndReturn<Att_RosterEntity, Att_RosterModel>(String.Join(",", selectedIds), valueFields,
                ConstantSql.hrm_att_sp_get_RosterByIds);
        }

        /// <summary>
        /// [Hieu.van] - Xuất danh sách dữ liệu cho Nhân Viên chưa có ca 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileNotRosterList([DataSourceRequest] DataSourceRequest request, Att_SearchProfileNotRosterModel model)
        {
            return ExportAllAndReturn<Att_ProfileEntity, Att_ProfileModel, Att_SearchProfileNotRosterModel>(request, model,
                ConstantSql.hrm_att_sp_get_Roster_ProfileNotRoster);
        }

        /// <summary>
        /// [Son.Vo] - Xuất dữ liệu được chọn cho bảng Roster (Att_Roster)
        /// </summary>
        public ActionResult ExportProfileNotRosterSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_ProfileEntity, Att_ProfileModel>(String.Join(",", selectedIds), valueFields,
                ConstantSql.hrm_att_sp_get_Roster_ProfileNotRosterByIds);
        }


        #endregion

        #region Att_WorkDay
        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng WorkDay (Att_WorkDay) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkDayList([DataSourceRequest] DataSourceRequest request, Att_WorkDaySearchModel model)
        {
            #region code cu get bang store

            string status = string.Empty;
            List<Att_WorkdayModel> lstWorkday = GetListData<Att_WorkdayModel, Att_WorkdayEntity, Att_WorkDaySearchModel>(request,
                model, ConstantSql.hrm_att_sp_get_WorkDay, ref status);

            List<Att_WorkdayModel> lstResult = new List<Att_WorkdayModel>();
            Att_WorkdayModel rs = null;
            foreach (var item in lstWorkday)
            {
                rs = new Att_WorkdayModel();

                rs.ProfileName = item.ProfileName;
                rs.WorkDate = item.WorkDate;
                rs.FirstInTime = item.FirstInTime;
                rs.LastOutTime = item.LastOutTime;
                rs.ShiftName = item.ShiftName;
                rs.WorkDuration = item.WorkDuration;
                rs.ID = item.ID;
                rs.ProfileID = item.ProfileID;
                rs.CodeEmp = item.CodeEmp;
                rs.CodeAttendance = item.CodeAttendance;
                rs.ShiftID = item.ShiftID;
                rs.udLeavedayCode1 = item.udLeavedayCode1;
                rs.InTime1 = item.InTime1;
                rs.OutTime1 = item.OutTime1;
                rs.Type = item.Type;
                rs.Status = item.Status;
                rs.ShiftActual = item.ShiftActual;
                rs.ShiftApprove = item.ShiftApprove;
                rs.ShiftActualName = item.ShiftActualName;
                rs.ShiftApproveName = item.ShiftApproveName;
                rs.LateDuration1 = item.LateDuration1;
                rs.EarlyDuration1 = item.EarlyDuration1;
                rs.LateEarlyDuration = item.LateEarlyDuration;
                rs.LateEarlyRoot = item.LateEarlyRoot;
                rs.LateEarlyReason = item.LateEarlyReason;
                rs.MissInOutReason = item.MissInOutReason;
                rs.TAMScanReasonMissName = item.TAMScanReasonMissName;
                rs.SrcType = item.SrcType;
                rs.LateEarlyError = item.LateEarlyError;
                if (item.ShiftID != null)
                {
                    rs.ShiftInTime = item.ShiftInTime;
                    rs.ShiftOutTime = item.ShiftInTime.Value.AddHours(item.CoOut.Value != null ? item.CoOut.Value : 0.0);
                }
                rs.TotalRow = item.TotalRow;
                lstResult.Add(rs);
            }

            request.Page = 1;
            var dataSourceResult = lstResult.ToDataSourceResult(request);
            dataSourceResult.Total = lstResult.Count() <= 0 ? 0 : lstResult.FirstOrDefault().TotalRow;
            return Json(dataSourceResult);
            #endregion
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho bảng WorkDay (Att_WorkDay) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportWorkDayList([DataSourceRequest] DataSourceRequest request, Att_WorkDaySearchModel model)
        {
            string status = string.Empty;

            var listModel = GetListData<Att_WorkdayModel, Att_WorkdayEntity, Att_WorkDaySearchModel>(request, model, ConstantSql.hrm_att_sp_get_WorkDay, ref status);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Att_WorkdayModel.FieldNames.FirstInTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_WorkdayModel.FieldNames.LastOutTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_WorkdayModel.FieldNames.WorkDuration, ConstantFormat.HRM_Format_Number_Double);

            status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','), formatFields);
            return Json(status);
            //return ExportAllAndReturn<Att_WorkdayEntity, Att_WorkdayModel, Att_WorkDaySearchModel>(request, model,
            //    ConstantSql.hrm_att_sp_get_WorkDay);
        }

        [HttpPost]
        public ActionResult ExportWorkDayTemplateList([DataSourceRequest] DataSourceRequest request, Att_WorkDayExportTemplateModel model)
        {
            string status = string.Empty;
            Att_WorkDaySearchModel search = new Att_WorkDaySearchModel();
            search.ProfileName = model.ProfileName;
            search.CodeEmp = model.CodeEmp;
            search.DateFrom = model.DateFrom;
            search.DateTo = model.DateTo;
            search.ShiftID = Common.DotNetToOracle(model.ShiftID);

            search.OrgStructureID = model.OrgStructureID;
            search.SrcType = model.SrcType;
            search.Type = model.Type;
            search.LateDuration = model.LateDuration;
            search.EarlyDuration = model.EarlyDuration;

            request.PageSize = 1000000000;

            var lstResult = GetListData<Att_WorkdayModel, Att_WorkdayEntity, Att_WorkDaySearchModel>(request, search, ConstantSql.hrm_att_sp_get_WorkDay, ref status);
            if (lstResult.Count > 0)
            {

                foreach (var item in lstResult)
                {
                    item.Type = item.Type != null ? item.Type.TranslateString() : null;
                    item.Status = item.Status != null ? item.Status.TranslateString() : null;
                    item.SrcType = item.SrcType != null ? item.SrcType.TranslateString() : null;
                    item.TempTimeIn = item.InTime1 == null ? "" : item.InTime1.Value.ToString(@"HH:mm:ss");
                    item.TempTimeOut = item.OutTime1 == null ? "" : item.OutTime1.Value.ToString(@"HH:mm:ss");
                    item.TempInTimeRoot = item.InTimeRoot == null ? "" : item.InTimeRoot.Value.ToString(@"HH:mm:ss");
                    item.TempOutTimeRoot = item.OutTimeRoot == null ? "" : item.OutTimeRoot.Value.ToString(@"HH:mm:ss");

                }
            }
            var fullPath = ExportService.Export(model.ExportId, lstResult);
            return Json(fullPath);
        }

        /// <summary>va
        /// [Son.Vo] - Xuất dữ liệu được chọn cho bảng WorkDay (Att_WorkDay)
        /// </summary>
        public ActionResult ExportWorkDaySelected(List<Guid> selectedIds, string valueFields)
        {
            string status = string.Empty;
            var selectidsOrcl = Common.DotNetToOracle(String.Join(",", selectedIds));
            var listModel = GetData<Att_WorkdayModel, Att_WorkdayEntity>(selectidsOrcl, ConstantSql.hrm_att_sp_get_WorkDayByIds);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Att_WorkdayModel.FieldNames.FirstInTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_WorkdayModel.FieldNames.LastOutTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_WorkdayModel.FieldNames.WorkDuration, ConstantFormat.HRM_Format_Number_Double);

            status = ExportService.Export(listModel, valueFields.Split(','), formatFields);
            return Json(status);
            //return ExportSelectedAndReturn<Att_WorkdayEntity, Att_WorkdayModel>(selectedIds, valueFields,
            //    ConstantSql.hrm_att_sp_get_WorkDayByIds);
        }
        public ActionResult ValidateOvertimeStatus(Att_OvertimeValidateModel model)
        {
            #region Validate
            string message = string.Empty;
            if (model.buttonId == "btnChangeManualLeave")
            {
                Guid guidID = Guid.Parse(model.selectedIds);
                ActionService actionservice = new ActionService(UserLogin);
                var objAtt_OT = actionservice.GetByIdUseStore<Att_OvertimeEntity>(guidID, ConstantSql.hrm_att_sp_get_OvertimeById, ref message);
                if (message == NotificationType.Success.ToString())
                {
                    message = "";
                    if (objAtt_OT.MethodPayment != EnumDropDown.MethodPayment.E_CASHOUT.ToString())
                        message = ConstantMessages.DoNotSelectPaymentMethodLeave.TranslateString();
                    if (objAtt_OT.Status != EnumDropDown.StatusOT.E_CONFIRM.ToString() && objAtt_OT.Status != EnumDropDown.StatusOT.E_APPROVED.ToString())
                    {
                        message = ConstantMessages.PlsSelectStatusApprove.TranslateString();
                    }
                }
            }
            if (model.buttonId == "btnApprove")
            {
                ActionService service = new ActionService(UserLogin);
                model.selectedIds = Common.DotNetToOracle(model.selectedIds);
                var lstCheck = service.GetData<Att_RosterModel>(model.selectedIds, ConstantSql.hrm_att_sp_get_OvertimeByIds, ref message);
                if (message == NotificationType.Success.ToString())
                {
                    message = "";
                    foreach (var item in lstCheck)
                    {
                        if (item.Status == EnumDropDown.OverTimeStatus.E_REJECTED.ToString())
                        {
                            message = ConstantMessages.StatusRejectcannotEdit.TranslateString();
                            break;
                        }
                    }
                }
            }
            if (model.buttonId == "btnPayment" || model.buttonId == "btnLeave")
            {
                ActionService service = new ActionService(UserLogin);
                model.selectedIds = Common.DotNetToOracle(model.selectedIds);
                var lstCheck = service.GetData<Att_RosterModel>(model.selectedIds, ConstantSql.hrm_att_sp_get_OvertimeByIds, ref message);
                if (message == NotificationType.Success.ToString())
                {
                    message = "";
                    foreach (var item in lstCheck)
                    {
                        if (item.Status == EnumDropDown.OverTimeStatus.E_APPROVED.ToString() || item.Status == EnumDropDown.OverTimeStatus.E_FIRST_APPROVED.ToString() || item.Status == EnumDropDown.OverTimeStatus.E_REJECTED.ToString())
                        {
                            message = ConstantMessages.StatusApproveCannotEdit.TranslateString();
                            break;
                        }
                    }
                }
            }

            if (message != "")
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            else
            {
                var ls = new object[] { ConstantMessages.Succeed.ToString(), message };
                return Json(ls);
            }
            #endregion
        }
        public ActionResult ValidateLeavedayStatus(Att_OvertimeValidateModel model)
        {
            string message = string.Empty;
            if (model.buttonId == "btnWaitApprove")
            {
                ActionService service = new ActionService(UserLogin);
                Guid guidID = Guid.Parse(model.selectedIds);
                model.selectedIds = Common.DotNetToOracle(model.selectedIds);
                var lstCheck = service.GetData<Att_LeaveDayModel>(model.selectedIds, ConstantSql.hrm_att_sp_get_LeavedayByIds, ref message);
                if (message == NotificationType.Success.ToString())
                {
                    message = "";
                    foreach (var item in lstCheck)
                    {
                        if (item.Status == AttendanceDataStatus.E_APPROVED.ToString() || item.Status == AttendanceDataStatus.E_FIRST_APPROVED.ToString())
                        {
                            message = ConstantMessages.StatusApproveCannotEdit.TranslateString();
                            break;
                        }
                        if (item.Status == AttendanceDataStatus.E_REJECTED.ToString())
                        {
                            message = ConstantMessages.StatusRejectcannotEdit.TranslateString();
                            break;
                        }
                    }
                }
            }
            if (message != "")
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            else
            {
                var ls = new object[] { ConstantMessages.Succeed.ToString(), message };
                return Json(ls);
            }
        }
        public ActionResult ValidateWorkdayStatus(Att_OvertimeValidateModel model)
        {
            string message = string.Empty;
            if (model.buttonId == "btnWaitting")
            {
                ActionService service = new ActionService(UserLogin);
                model.selectedIds = Common.DotNetToOracle(model.selectedIds);
                var lstCheck = service.GetData<Att_WorkdayEntity>(model.selectedIds, ConstantSql.hrm_att_sp_get_WorkDayByIds, ref message);
                if (message == NotificationType.Success.ToString())
                {
                    message = "";
                    foreach (var item in lstCheck)
                    {
                        if (item.Status == EnumDropDown.WorkdayStatus.E_APPROVED.ToString() || item.Status == EnumDropDown.WorkdayStatus.E_FIRST_APPROVED.ToString() || item.Status == EnumDropDown.WorkdayStatus.E_REJECTED.ToString())
                        {
                            message = ConstantMessages.StatusApproveCannotEdit.TranslateString();
                            break;
                        }
                    }
                }
            }

            if (message != "")
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            else
            {
                var ls = new object[] { ConstantMessages.Succeed.ToString(), message };
                return Json(ls);
            }
        }
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình phân tích nghỉ phép trễ sớm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetComputeLeaveLateEarlyValidate(Att_LateEarlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_LateEarlySearchModel>(model, "Att_ComputeLeaveLateEarly", ref message);
            if (!checkValidate)
            {
                //return Json(message);
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }
        /// <summary>
        /// [Hieu.Van] - 05/06/2014
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComputeLeaveLateEarly([DataSourceRequest] DataSourceRequest request, Att_LateEarlySearchModel model)
        {
             var ActionService = new ActionService(UserLogin);
            Att_LeavedayServices repo = new Att_LeavedayServices();

            DateTime? DateFrom = SqlDateTime.MinValue.Value;
            DateTime? DateTo = SqlDateTime.MaxValue.Value;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }
                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(model.ProfileIDs);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }
            #endregion

            var result = repo.ComputeLeaveLateEarly(DateFrom, DateTo, lstProfileIDs, model.Type).ToList().Translate<Att_WorkdayModel>();

            #region [Hien.Nguyen]
            foreach (var item in result)
            {
                item.TempTimeIn = item.InTime1 == null ? "" : item.InTime1.Value.ToString(@"HH:mm:ss");
                item.TempTimeOut = item.OutTime1 == null ? "" : item.OutTime1.Value.ToString(@"HH:mm:ss");
            }
            #endregion

            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            //return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue };
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình dữ liệu chấm công hàng ngày
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetComputeWorkdayAdjustValidate(Att_WorkDaySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_WorkDaySearchModel>(model, "Att_ComputeWorkdayAdjust", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Hieu.Van] - 05/06/2014
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComputeWorkdayAdjust([DataSourceRequest] DataSourceRequest request, Att_WorkDaySearchModel model)
        {
            model.ShiftID = Common.DotNetToOracle(model.ShiftID);
            Att_ComputeWorkdayAdjustServices service = new Att_ComputeWorkdayAdjustServices();
            if (model.DateFrom != null)
            {
                model.DateFrom = model.DateFrom.Value.Date;
            }
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddMilliseconds(-1);
            }
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };


            List<Att_WorkdayModel> lstWorkday = new List<Att_WorkdayModel>();

            var lstResult = service.ComputeWorkdayAdjust(lstModel,UserLogin);
            if (lstResult != null)
            {
                lstWorkday = lstResult.OrderBy(m => m.WorkDate).ToList().Translate<Att_WorkdayModel>();
                foreach (var item in lstWorkday)
                {
                    item.TempTimeIn = item.InTime1 == null ? "" : item.InTime1.Value.ToString(@"HH:mm:ss");
                    item.TempTimeOut = item.OutTime1 == null ? "" : item.OutTime1.Value.ToString(@"HH:mm:ss");
                    item.TempInTimeRoot = item.InTimeRoot == null ? "" : item.InTimeRoot.Value.ToString(@"HH:mm:ss");
                    item.TempOutTimeRoot = item.OutTimeRoot == null ? "" : item.OutTimeRoot.Value.ToString(@"HH:mm:ss");
                }
            }

            request.Page = 1;
            var dataSourceResult = lstWorkday.ToDataSourceResult(request);
            dataSourceResult.Total = lstWorkday.Count > 0 ? lstWorkday.FirstOrDefault().TotalRow : 0;
            return Json(dataSourceResult);
            //return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue };
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình tổng hợp ngày công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetComputeWorkdayValidate(Att_ComputeWorkDayModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ComputeWorkDayModel>(model, "Att_ComputeWorkDay", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        #region HRM8 - 20140721
        /// <summary>
        /// [Hieu.Van] - 05/06/2014
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetComputeWorkday([DataSourceRequest] DataSourceRequest request, Att_ComputeWorkDayModel model)
        {
            var service = new Att_WorkDayServices();
            DateTime dateFrom = model.DateFrom;
            DateTime dateTo = model.DateTo;

            if (model.ComputeContinue)
            {
                dateFrom = model.DateTo;
                dateTo = DateTime.Now;
            }

            model.ID = service.CreateComputingTask(model.UserCreateID, dateFrom, dateTo);

            if (!string.IsNullOrWhiteSpace(model.ProfileIDs))
            {
                var listProfileID = model.ProfileIDs.StringToList();

                Task task = Task.Run(() => service.ComputeWorkday(model.ID,
                    model.UserCreateID, dateFrom, dateTo, listProfileID));
            }
            else
            {
                var listOrgStructureID = model.OrgStructureID.StringToList();

                Task task = Task.Run(() => service.ComputeWorkday(model.ID,
                    model.UserCreateID, dateFrom, dateTo, listOrgStructureID, null));
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/07/11
        /// Update status workday Duyệt và Chờ Duyệt
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public bool ChangeStatusWorkday(Att_ChangeStatusWorkdayModel model)
        {
            return true;
            //return ExportAllAndReturn<Att_WorkdayEntity, Att_WorkdayModel, Att_ChangeStatusWorkdayModel>(request, model,
            //    ConstantSql.hrm_att_sp_get_WorkDay);
            ////hrm_att_sp_get_WorkDay_UpdateStatus
            //var service = new Att_WorkDayServices();
            //var result = service.UpdateStatusWorkday(model.SelectedIds, model.Type);
            //return result;
        }
        /// <summary>
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>       
        [HttpPost]
        public ActionResult GetWorkDayByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_WorkdayModel, Att_WorkdayEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_WorkDayByProIDandCutOID);
            }
            return GetListDataAndReturn<Att_WorkdayModel, Att_WorkdayEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_WorkDayByProIDandCutOID);
        }

        public ActionResult GetGradeAttendanceByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_GradeModel, Att_GradeEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_GradeAttendanceByProIdCutID);
            }
            return GetListDataAndReturn<Att_GradeModel, Att_GradeEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_GradeAttendanceByProIdCutID);

            //string status = string.Empty;
            // var ActionService = new ActionService(UserLogin);
            //var objs = new List<object>();
            //objs.Add(Common.DotNetToOracle(model.ProfileID));
            //var result = ActionService.GetData<Att_GradeModel>(objs, ConstantSql.hrm_hr_sp_get_GradeAttendanceByProfileId, ref status);
            //return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetRosterByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_RosterModel, Att_RosterEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_RosterByProIDandCutOID);
            }
            return GetListDataAndReturn<Att_RosterModel, Att_RosterEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_RosterByProIDandCutOID);
        }

        [HttpPost]
        public ActionResult GetAttendanceDetailByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);

            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_AttendanceTableItemModel, Att_AttendanceTableItemEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_AttendanceTableItemByProIDandCutOID);
            }

            return GetListDataAndReturn<Att_AttendanceTableItemModel, Att_AttendanceTableItemEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_AttendanceTableItemByProIDandCutOID);
        }

        [HttpPost]
        public ActionResult GetAttendanceTableByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);

            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            if (lstModel.PageSize == 0)
                lstModel.PageSize = 50;
            var status = string.Empty;
            var listEntity = service.GetData<Att_AttendanceTableEntity>(lstModel, ConstantSql.hrm_att_sp_get_AttendanceTableByProIDandCutOID, ref status);
            if (listEntity != null)
            {
                var listModel = listEntity.FirstOrDefault().CopyData<Att_AttendanceTableModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new Att_AttendanceTableModel();
            return Json(listModelNull, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Att_TamScanLog

        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng WorkDay (Att_WorkDay) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTAMScanLogByProfileID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_TAMScanLogModel, Att_TAMScanLogEntity, Att_ProIDAndCutIDModel>(request,
                    model, ConstantSql.hrm_att_sp_get_TAMScanLogByProfileId);
            }
            return GetListDataAndReturn<Att_TAMScanLogModel, Att_TAMScanLogEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_TAMScanLogByProfileId);

            // var ActionService = new ActionService(UserLogin);
            //string status = string.Empty;
            //var objs = new List<object>();
            //objs.Add(Common.DotNetToOracle(model.ProfileID));
            //objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            //objs.Add(request.Page);
            //objs.Add(request.PageSize);
            //var result = ActionService.GetData<Att_TAMScanLogEntity>(objs, ConstantSql.hrm_att_sp_get_TAMScanLogByProfileId, ref status);
            //return Json(result.ToDataSourceResult(request));
        }


        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLeavedayByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID);
            }
            return GetListDataAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID);
        }

        #region Hre_CompensationDetailView
        public ActionResult GetCompensationDetailByProfileId([DataSourceRequest]DataSourceRequest request, Att_CompensationDetailViewSeachModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            return GetListDataAndReturn<Att_CompensationDetailModel, Att_CompensationDetailEntity, Att_CompensationDetailViewSeachModel>(request, model, ConstantSql.hrm_att_sp_get_CompensationDetailByProfileId);
        }
        public ActionResult ExportCompensationDetailByProfileId([DataSourceRequest]DataSourceRequest request, Att_CompensationDetailViewSeachModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            return ExportAllAndReturn<Att_CompensationDetailEntity, Att_CompensationDetailModel, Att_CompensationDetailViewSeachModel>(request, model, ConstantSql.hrm_att_sp_get_CompensationDetailByProfileId);
        }
        #endregion


        #region Hre_AnnualDetailView
        public ActionResult GetAnnualDetailByProfileId([DataSourceRequest]DataSourceRequest request, Att_AnnualDetailViewSearchMode model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            //return GetListDataAndReturn<Att_AnnualDetailModel, Att_AnnualDetailEntity, Att_AnnualDetailViewSearchMode>(request, model, ConstantSql.hrm_att_sp_get_AnnualDetailByProfileId);
            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;
            var listEntity = service.GetData<Att_AnnualDetailEntity>(lstModel, ConstantSql.hrm_att_sp_get_AnnualDetailByProfileId, ref status);
            var listModel = new List<Att_AnnualDetailModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    item.Available = item.Available != null ? item.Available : 0;
                    item.InitAvailable = item.InitAvailable != null ? item.InitAvailable : 0;
                    item.AvailableTotal = item.Available + item.InitAvailable;

                    item.TotalLeaveBefFromInitAvailable = item.TotalLeaveBefFromInitAvailable != null ? item.TotalLeaveBefFromInitAvailable : 0;
                    item.TotalLeaveBef = item.TotalLeaveBef != null ? item.TotalLeaveBef : 0;
                    item.LeaveBefTotal = item.TotalLeaveBefFromInitAvailable + item.TotalLeaveBef;

                    item.LeaveInMonthFromInitAvailable = item.LeaveInMonthFromInitAvailable != null ? item.LeaveInMonthFromInitAvailable : 0;
                    item.LeaveInMonth = item.LeaveInMonth != null ? item.LeaveInMonth : 0;
                    item.LeaveInMonthTotal = item.LeaveInMonthFromInitAvailable + item.LeaveInMonth;

                    var newModle = (Att_AnnualDetailModel)typeof(Att_AnnualDetailModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new List<Att_AnnualDetailModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }
        public ActionResult ExportAnnualDetailByProfileId([DataSourceRequest]DataSourceRequest request, Att_AnnualDetailViewSearchMode model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            //return ExportAllAndReturn<Att_AnnualDetailEntity, Att_AnnualDetailModel, Att_AnnualDetailViewSearchMode>(request, model, ConstantSql.hrm_att_sp_get_AnnualDetailByProfileId);
            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<Att_AnnualDetailEntity>(lstModel, ConstantSql.hrm_att_sp_get_AnnualDetailByProfileId, ref status);
            var listModel = new List<Att_AnnualDetailModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    item.Available = item.Available != null ? item.Available : 0;
                    item.InitAvailable = item.InitAvailable != null ? item.InitAvailable : 0;
                    item.AvailableTotal = item.Available + item.InitAvailable;

                    item.TotalLeaveBefFromInitAvailable = item.TotalLeaveBefFromInitAvailable != null ? item.TotalLeaveBefFromInitAvailable : 0;
                    item.TotalLeaveBef = item.TotalLeaveBef != null ? item.TotalLeaveBef : 0;
                    item.LeaveBefTotal = item.TotalLeaveBefFromInitAvailable + item.TotalLeaveBef;

                    item.LeaveInMonthFromInitAvailable = item.LeaveInMonthFromInitAvailable != null ? item.LeaveInMonthFromInitAvailable : 0;
                    item.LeaveInMonth = item.LeaveInMonth != null ? item.LeaveInMonth : 0;
                    item.LeaveInMonthTotal = item.LeaveInMonthFromInitAvailable + item.LeaveInMonth;

                    var newModle = (Att_AnnualDetailModel)typeof(Att_AnnualDetailModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
            }
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status);
        }

        public ActionResult GetAnnualLeaveByProfileId([DataSourceRequest] DataSourceRequest request, Att_AnnualDetailViewSearchMode model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);

            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            if (lstModel.PageSize == 0)
                lstModel.PageSize = 50;
            var status = string.Empty;
            var listEntity = service.GetData<Att_AnnualLeaveEntity>(lstModel, ConstantSql.hrm_att_sp_get_AnnualLeaveByProfileId, ref status);
            if (listEntity != null)
            {
                var listModel = listEntity.FirstOrDefault().CopyData<Att_AnnualLeaveModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new Att_AnnualLeaveModel();
            return Json(listModelNull, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// Load data theo profileID va CutOffDurationID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOvertimeByProIDandCutOID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            model.CutOffDurationID = Common.DotNetToOracle(model.CutOffDurationID);
            if (model.IsExport)
            {
                return ExportAllAndReturn<Att_OvertimeModel, Att_OvertimeEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_OvertimeByProIDandCutOID);
            }
            return GetListDataAndReturn<Att_OvertimeModel, Att_OvertimeEntity, Att_ProIDAndCutIDModel>(request,
               model, ConstantSql.hrm_att_sp_get_OvertimeByProIDandCutOID);
        }

        public ActionResult GetTAMScanLogListValidate(Att_TAMScanLogSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_TAMScanLogSearchModel>(model, "Att_TAMScanLog/Index", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            return Json(message);
            #endregion
        }
        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu bảng WorkDay (Att_WorkDay) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTAMScanLogList([DataSourceRequest] DataSourceRequest request, Att_TAMScanLogSearchModel model)
        {
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddMilliseconds(-1);
            }
            return GetListDataAndReturn<Att_TAMScanLogModel, Att_TAMScanLogEntity, Att_TAMScanLogSearchModel>(request,
               model, ConstantSql.hrm_att_sp_get_TAMScanLog);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho bảng TAMScanLog (Att_TAMScanLog) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult ExportTAMScanLogList([DataSourceRequest] DataSourceRequest request, Att_TAMScanLogSearchModel model)
        {
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddMilliseconds(-1);
            }
            return ExportAllAndReturn<Att_TAMScanLogEntity, Att_TAMScanLogModel, Att_TAMScanLogSearchModel>(request, model,
                ConstantSql.hrm_att_sp_get_TAMScanLog);
        }

        /// <summary>
        /// [Son.Vo] - Xuất dữ liệu được chọn cho bảng TAMScanLog (Att_TAMScanLog)
        /// </summary>
        public ActionResult ExportTAMScanLogSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_TAMScanLogEntity, Att_TAMScanLogModel>(String.Join(",", selectedIds), valueFields,
                ConstantSql.hrm_att_sp_get_TAMScanLogByIds);
        }

        #region HRM8 - 20140721
        ///// <summary>
        ///// [Hieu.Van] - 2014/05/23
        ///// Lấy dữ liệu load lên lưới bằng store
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="otModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetTAMScanLogByProfileID([DataSourceRequest] DataSourceRequest request, Att_TAMScanLogModel model)
        //{
        //    ////hrm_att_sp_get_TAMScanLogByProfileId
        //    //int ProID = model.ProfileID;
        //    //int CutOffID = model.CutOffID;
        //    //var service = new Att_TAMScanLogServices();
        //    //var result = service.GetByProfileID(ProID, CutOffID);
        //    //return Json(result.ToDataSourceResult(request));

        //    return GetListDataAndReturn<Att_TAMScanLogModel, Att_WorkdayEntity, Att_TAMScanLogSearchModel>(request,
        //       model, ConstantSql.hrm_att_sp_get_TAMScanLogByProfileId);
        //} 

        ///// <summary>
        ///// [Hieu.Van] - 2014/05/23
        ///// Lấy dữ liệu load lên lưới bằng store
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="otModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetTAMScanLogList([DataSourceRequest] DataSourceRequest request, Att_TAMScanLogSearchModel Model)
        //{
        //    var service = new Att_TAMScanLogServices();

        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        PageSize = request.PageSize,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(Model)
        //    };
        //    var result = service.GetTAMScanLog(lstModel).ToList().Translate<Att_TAMScanLogModel>();
        //    if (Model.IsExport)
        //    {
        //        var fullPath = ExportService.Export(result, Model.ValueFields.Split(','));
        //        return Json(fullPath);
        //    }

        //    request.Page = 1;
        //    var dataSourceResult = result.ToDataSourceResult(request);
        //    dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
        //    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        //}


        ///// <summary>
        ///// [Hieu.Van] - 2014/07/03
        ///// Lấy dữ liệu load lên Xuất Excel TAMScanLog
        ///// </summary>
        //public ActionResult ExportTAMScanLogSelected(string selectedIds, string valueFields)
        //{
        //    var service = new Att_TAMScanLogServices();
        //    var result = service.GetTAMScanLogByIds(selectedIds).Translate<Att_TAMScanLogModel>();
        //    var fullPath = ExportService.Export(result, valueFields.Split(','));
        //    return Json(fullPath);
        //}
        #endregion
        #endregion

        #region att_Overtime
        public JsonResult GetPaymentMethodByProfileID(Guid ProfileID, DateTime WorkDate)
        {
            string result = string.Empty;
            Att_ReportServices _attReportService=new Att_ReportServices();
            Att_GradeEntity objGrade=_attReportService.GetGradeByProfileId(ProfileID,WorkDate);
            if(objGrade!=null)
            {
                string status = string.Empty;
                ActionService service = new ActionService(UserLogin);
                string id = Common.DotNetToOracle(objGrade.GradeAttendanceID.ToString());
                var objGradeAttendance = service.GetData<Cat_GradeAttendanceEntity>(id, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendanceByIds, ref status);
                if (objGradeAttendance != null)
                    result = objGradeAttendance.Select(s => s.MethodPayment).FirstOrDefault();
            }
            return Json(result);
        }

        public void SetStatusSelected(string selectedIds, string status, string userApproved)
        {
            if (!string.IsNullOrEmpty(selectedIds))
            {


                if (status == OverTimeStatus.E_APPROVED.ToString())
                {
                    //Att_OvertimeServices
                    List<Guid> lstOvertimeID = new List<Guid>();
                    var selectIDarr = selectedIds.Split(new char[','], StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var item in selectIDarr)
                    {
                        Guid overtimeID = Guid.Empty;
                        Guid.TryParse(item, out overtimeID);
                        if (overtimeID != Guid.Empty)
                        {
                            lstOvertimeID.Add(overtimeID);
                        }
                    }
                    Guid loginUserID = Guid.Empty;
                    Guid.TryParse(userApproved, out loginUserID);

                    Att_OvertimeServices otService = new Att_OvertimeServices();
                    otService.ApproveOvertime(lstOvertimeID, loginUserID);
                }
                else
                {
                    BaseService service = new BaseService();
                    string statusMessages = string.Empty;
                    List<object> lstObj = new List<object>();
                    lstObj.Add(selectedIds);
                    lstObj.Add(status);
                    lstObj.Add(null);
                    service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Status, ref statusMessages);
                }
            }
        }
        public JsonResult GetConfigAproveOverTime()
        {
            Sys_AttOvertimePermitConfigServices sysAttService = new Sys_AttOvertimePermitConfigServices();
            var result = sysAttService.GetConfigValue<int>(AppConfig.HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME);
            return Json(result);
        }

        #endregion

        // kiểm tra người đó là ng duyệt đầu hay ng duyệt cuối, nếu đầu status trả về là E_First_Approved
        [HttpPost]
        public JsonResult GetLevelApprovedOfUser(string TEntity, string recordID, string userID)
        {
            List<Guid> lstRCID = recordID.Split(',').Select(s => Guid.Parse(s)).ToList();
            BaseService _base = new BaseService();
            string status = string.Empty;
            string strID_Frist_Approved = string.Empty;
            string strID_Approved = string.Empty;
            Guid user = Guid.Empty;
            Guid.TryParse(userID, out user);

            foreach (Guid rcID in lstRCID)
            {
                var record = _base.GetById(rcID, TEntity);
                if (record != null)
                {
                    Guid app1 = Guid.Empty;
                    Guid app2 = Guid.Empty;
                    Guid.TryParse(record.GetPropertyValue("UserApproveID").ToString(), out app1);
                    Guid.TryParse(record.GetPropertyValue("UserApproveID2").ToString(), out app2);

                    if (user == app1 && user != app2)
                    {
                        strID_Frist_Approved += "," + rcID.ToString();
                    }
                    else if (user == app1 && user == app2)
                    {
                        strID_Approved += "," + rcID.ToString();
                    }
                    else if (user == app2)
                    {
                        strID_Approved += "," + rcID.ToString();
                    }
                }
            }
            return Json(new { lstFirstApproved = strID_Frist_Approved, lstApproved = strID_Approved });
        }

        // get cấu hình Áp dụng cắt theo giờ làm việc tối đa
        [HttpPost]
        public JsonResult GetConfig_AllowSplit()
        {
            #region Xử lý trường hợp Không Tách Tăng Ca

            string status = string.Empty;
             var ActionService = new ActionService(UserLogin);
            List<object> lstSys = new List<object>();
            lstSys.Add(AppConfig.HRM_ATT_OT_OTPERMIT_.ToString());
            lstSys.Add(null); lstSys.Add(null);
            var config = ActionService.GetData<Sys_AllSettingEntity>(lstSys, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            if (config.Count > 0)
            {
                double f = 0;
                var _configCFG = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString()).FirstOrDefault();
                var _configFML = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString()).FirstOrDefault();
                if ( _configCFG.Value1 != bool.TrueString)
                {
                    return Json(new { configMaxOT = false, formulaMaxOT = 0 });
                }
                else
                {
                    double.TryParse(_configFML.Value1, out f);
                    return Json(new { configMaxOT = true, formulaMaxOT = f });
                }
            }
            return Json(new { configMaxOT = false, formulaMaxOT = 0 });

            #endregion
        }

        // get cấu hình Cho Phép Tách Tăng Ca
        [HttpPost]
        public bool GetConfig_AllowCut()
        {
            #region Xử lý trường hợp Không Tách Tăng Ca

            string status = string.Empty;
             var ActionService = new ActionService(UserLogin);
            List<object> lstSys = new List<object>();
            lstSys.Add(AppConfig.HRM_ATT_OT_ISALLOWCUT.ToString());
            lstSys.Add(null); lstSys.Add(null);
            var config = ActionService.GetData<Sys_AllSettingEntity>(lstSys, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            if (config != null)
            {
                var _config = config.FirstOrDefault();
                if (_config.Value1 != bool.TrueString)
                {
                    return false;
                }
            }
            return true;

            #endregion
        }

        // Cho phép trùng OT nếu khác loại
        [HttpPost]
        public ActionResult GetConfig_Allow()
        {
            #region Cho phép trùng OT nếu khác loại

            string status = string.Empty;
             var ActionService = new ActionService(UserLogin);
            List<object> lstSys = new List<object>();
            lstSys.Add(AppConfig.HRM_ATT_ALLOWSAVEDUPLICATE.ToString());
            lstSys.Add(null); lstSys.Add(null);
            var config = ActionService.GetData<Sys_AllSettingEntity>(lstSys, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            if (config != null)
            {
                var _config = config.FirstOrDefault();
                if (config.Count != 0)
                {
                    if (_config.Value1 == bool.TrueString)
                    {
                        return Json(true);
                    }
                }
            }
            return Json(false);

            #endregion
        }

        public ActionResult GetCutOffDuration([DataSourceRequest] DataSourceRequest request, Att_CutOffDurationSearchModel otModel)
        {
            return GetListDataAndReturn<Att_CutOffDurationModel, Att_CutOffDurationEntity, Att_CutOffDurationSearchModel>(request, otModel, ConstantSql.hrm_att_sp_get_CutOffDurations);
        }

        public JsonResult GetMultiCutOffDuration(string text)
        {
            string status = string.Empty;
            ActionService ActionService = new ActionService(UserLogin);
            var objs = new List<object>(); 
            objs.Add(text);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = ActionService.GetData<Att_CutOffDurationEntity>(objs, ConstantSql.hrm_att_sp_get_CutOffDurations, ref status).OrderByDescending(s => s.DateStart).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public Guid GetCutOffDurationDefault()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            return service.GetCutOffDurationDefault(); ;
        }

        public JsonResult GetStartEndOfCutOffDuration(Guid cutoffID)
        {
            DateTime DS = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime DE = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1 > 12 ? 1 : DateTime.Now.Month + 1, 1).AddDays(-1);
            DateTime Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            string status = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var result = service.GetByIdUseStore<Att_CutOffDurationEntity>(cutoffID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            if (result != null)
            {
                if (result.DateStart != null)
                {
                    DS = result.DateStart;
                }
                if (result.DateEnd != null)
                {
                    DE = result.DateEnd;
                }
                if (result.MonthYear != null)
                {
                    Month = result.MonthYear;
                }
            }
            return Json(new { DateStart = DS, DateEnd = DE, Month = Month });
        }


        private void SetStatusOvertimeOnWorkday(List<Att_OvertimeEntity> lstOvertime)
        {
            if (lstOvertime == null || lstOvertime.Count == 0)
                return;

            foreach (var item in lstOvertime)
            {
                item.udAlreadyOvertimeID = item.ID;
                item.udOvertimeStatus = item.Status.TranslateString();
            }
            return;
        }
        /// <summary>
        /// [hien.pham] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetOvertimeList([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };

            string status = string.Empty;
            if (model.DateEnd != null)
                model.DateEnd = model.DateEnd.Value.Date.AddDays(1).AddMilliseconds(-1);
            var listOvertime = service.GetData<Att_OvertimeEntity>(lstModel, ConstantSql.hrm_att_sp_get_Overtime, ref status);

            var overtimeServices = new Att_OvertimeServices();
            List<Att_OvertimeEntity> lstOvertimeCheck = listOvertime as List<Att_OvertimeEntity>;
            List<Att_OvertimeEntity> lstOvertimeTmp = new List<Att_OvertimeEntity>();
            lstOvertimeTmp.AddRange(lstOvertimeCheck);
            SetStatusOvertimeOnWorkday(lstOvertimeTmp);
            overtimeServices.FillterAllowOvertime(lstOvertimeTmp);
            foreach (var item in lstOvertimeCheck)
            {
                Att_OvertimeEntity overtimeTmp = lstOvertimeTmp.Where(m => m.ID == item.ID).FirstOrDefault();
                item.udHourByMonth = overtimeTmp.udHourByMonth;
                item.udHourByYear = overtimeTmp.udHourByYear;
            }
            listOvertime = lstOvertimeCheck;
            var listModel = listOvertime.Translate<Att_OvertimeModel>();
            request.Page = 1;
            var dataSourceResult = listModel.ToDataSourceResult(request);
            if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
            {
                dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                //dataSourceResult.Total = listModel.FirstOrDefault().TotalRow;
            }
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [hien.pham] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetOvertimeApprovedList([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };

            string status = string.Empty;
            if (model.DateEnd != null)
                model.DateEnd = model.DateEnd.Value.Date.AddDays(1).AddMilliseconds(-1);
            if (!string.IsNullOrEmpty(model.Status))
                model.Status += ",";
            var listOvertime = service.GetData<Att_OvertimeEntity>(lstModel, ConstantSql.hrm_att_sp_get_Overtime, ref status);

            var overtimeServices = new Att_OvertimeServices();
            List<Att_OvertimeEntity> lstOvertimeCheck = listOvertime as List<Att_OvertimeEntity>;
            List<Att_OvertimeEntity> lstOvertimeTmp = new List<Att_OvertimeEntity>();
            lstOvertimeTmp.AddRange(lstOvertimeCheck);
            SetStatusOvertimeOnWorkday(lstOvertimeTmp);
            overtimeServices.FillterAllowOvertime(lstOvertimeTmp);
            foreach (var item in lstOvertimeCheck)
            {
                Att_OvertimeEntity overtimeTmp = lstOvertimeTmp.Where(m => m.ID == item.ID).FirstOrDefault();
                item.udHourByMonth = overtimeTmp.udHourByMonth;
                item.udHourByYear = overtimeTmp.udHourByYear;
            }
            listOvertime = lstOvertimeCheck;

            //if (listOvertime != null && listOvertime.Count > 0)
            //{
            //    listOvertime = listOvertime.Where(s => !(s.Status == OverTimeStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == model.SysUserID)).ToList();
            //}
            if (listOvertime != null && listOvertime.Count > 0 && !string.IsNullOrEmpty(model.Status) && model.Status != (LeaveDayStatus.E_FIRST_APPROVED.ToString() + ","))
            {
                listOvertime = listOvertime.Where(s => !(s.Status == OverTimeStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == model.SysUserID)).ToList();
            }
            var listModel = listOvertime.Translate<Att_OvertimeModel>();
            request.Page = 1;
            var dataSourceResult = listModel.ToDataSourceResult(request);
            if (listModel.Count > 0)
            {
                //dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                dataSourceResult.Total = listModel.FirstOrDefault().TotalRow;
            }
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOvertimeManualLeave([DataSourceRequest] DataSourceRequest request, Att_OvertimeAnalysisLeaveModel model)
        {
            string status = string.Empty;
            if (model.LeaveDay1 == null && model.LeaveDay2 == null)
            {
                var listModelNull = new List<Att_OvertimeModel>();
                status = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("LeaveDay").TranslateString());
                ModelState.AddModelError("Id", status);
                return Json(listModelNull.ToDataSourceResult(request, ModelState));
            }
            var Att_OTmodel = new Att_OvertimeModel();
            ActionService actionservice = new ActionService(UserLogin);
            var objAtt_OT = actionservice.GetByIdUseStore<Att_OvertimeEntity>(model.ID, ConstantSql.hrm_att_sp_get_OvertimeById, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = "";
                Att_OvertimeServices Att_OTServices = new Att_OvertimeServices();
                List<Att_OvertimeEntity> lstAtt_OvertimeEntity = Att_OTServices.AddTimeOffInLieu(objAtt_OT, model.LeaveDay1, model.LeaveDay2, out status);
                if (status == "")
                {
                    request.Page = 1;
                    var listModel = lstAtt_OvertimeEntity.Translate<Att_OvertimeModel>();
                    var dataSourceResult = listModel.ToDataSourceResult(request);
                    if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                    {
                        dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                    }
                    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var listModelNull = new List<Att_OvertimeModel>();
                    ModelState.AddModelError("Id", status);
                    return Json(listModelNull.ToDataSourceResult(request, ModelState));
                }
            }
            else
            {
                var listModelNull = new List<Att_OvertimeModel>();
                ModelState.AddModelError("Id", status);
                return Json(listModelNull.ToDataSourceResult(request, ModelState));
            }

        }

        public ActionResult ChangeMethodOverTime_Manual_Leave([DataSourceRequest] DataSourceRequest request, Att_OvertimeAnalysisLeaveModel model)
        {
            string status = string.Empty;
            if (model.LeaveDay1 == null && model.LeaveDay2 == null)
            {
                status = string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ("LeaveDay").TranslateString());
                var ls = new object[] { "error", status };
                return Json(ls);
            }
            var Att_OTmodel = new Att_OvertimeModel();
            ActionService actionservice = new ActionService(UserLogin);
            var objAtt_OT = actionservice.GetByIdUseStore<Att_OvertimeEntity>(model.ID, ConstantSql.hrm_att_sp_get_OvertimeById, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = "";
                Att_OvertimeServices Att_OTServices = new Att_OvertimeServices();
                string strStatusTemp = "";
                Cat_LeaveDayTypeServices Cat_LDTypeServices = new Cat_LeaveDayTypeServices();
                List<Cat_LeaveDayTypeEntity> lstLeaveDayType = Cat_LDTypeServices.GetAllUseEntity<Cat_LeaveDayTypeEntity>(ref strStatusTemp);
                List<Att_OvertimeEntity> lstAtt_OvertimeEntity = Att_OTServices.AddTimeOffInLieu(objAtt_OT, model.LeaveDay1, model.LeaveDay2, out status);
                if (status == "")
                {
                    //save
                    int check = 0;
                    var service = new ActionService(UserLogin);
                    foreach (Att_OvertimeEntity item in lstAtt_OvertimeEntity)
                    {
                        Att_OTmodel = item.CopyData<Att_OvertimeModel>();
                        Att_OTmodel = actionservice.UpdateOrCreate<Att_OvertimeEntity, Att_OvertimeModel>(Att_OTmodel);
                        if (Att_OTmodel.ActionStatus != NotificationType.Success.ToString())
                        {
                            var ls = new object[] { "error", status };
                            return Json(ls);
                            //var listModelNull = new List<Att_OvertimeModel>();
                            //ModelState.AddModelError("Id", status);
                            //return Json(listModelNull.ToDataSourceResult(request, ModelState));
                        }
                    }
                    //dang ky nghi bu
                    if (model.LeaveDay1.HasValue)
                    {
                        Att_LeaveDayModel leaveDay = new Att_LeaveDayModel();
                        leaveDay.ID = Guid.Empty;
                        leaveDay.ProfileID = objAtt_OT.ProfileID;
                        leaveDay.LeaveDayTypeID = Att_OTServices.loadLeavDayTypeID();
                        leaveDay.DateStart = model.LeaveDay1.Value;
                        leaveDay.DateEnd = model.LeaveDay1.Value;
                        leaveDay.TotalDuration = 1;
                        leaveDay.LeaveDays = 1;
                        leaveDay.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        leaveDay.DateOvertimeOff = objAtt_OT.WorkDateRoot ?? objAtt_OT.WorkDate.Date;
                        leaveDay.LeaveHours = Att_OTServices.HourInShiftByDate(objAtt_OT.ProfileID, model.LeaveDay1.Value);
                        leaveDay = actionservice.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(leaveDay);
                    }
                    if (model.LeaveDay2.HasValue)
                    {
                        Att_LeaveDayModel leaveDay = new Att_LeaveDayModel();
                        leaveDay.ID = Guid.Empty;
                        leaveDay.ProfileID = objAtt_OT.ProfileID;
                        leaveDay.LeaveDayTypeID = Att_OTServices.loadLeavDayTypeID();
                        leaveDay.DateStart = model.LeaveDay2.Value;
                        leaveDay.DateEnd = model.LeaveDay2.Value;
                        leaveDay.TotalDuration = 1;
                        leaveDay.LeaveDays = 1;
                        leaveDay.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        leaveDay.DateOvertimeOff = objAtt_OT.WorkDateRoot ?? objAtt_OT.WorkDate.Date;
                        leaveDay.LeaveHours = Att_OTServices.HourInShiftByDate(objAtt_OT.ProfileID, model.LeaveDay2.Value);
                        leaveDay = actionservice.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(leaveDay);
                    }
                    //ActionService service = new ActionService(UserLogin);
                    //return service.UpdateOrCreate<Cat_OvertimeTypeEntity, CatOvertimeTypeModel>(model);

                    request.Page = 1;
                    var listModel = lstAtt_OvertimeEntity.Translate<Att_OvertimeModel>();
                    var dataSourceResult = listModel.ToDataSourceResult(request);
                    if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                    {
                        dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                    }
                    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ls = new object[] { "error", status };
                    return Json(ls);
                    //var listModelNull = new List<Att_OvertimeModel>();
                    //ModelState.AddModelError("Id", status);
                    //return Json(listModelNull.ToDataSourceResult(request, ModelState));
                }
            }
            else
            {
                var ls = new object[] { "error", status };
                return Json(ls);
                //var listModelNull = new List<Att_OvertimeModel>();
                //ModelState.AddModelError("Id", status);
                //return Json(listModelNull.ToDataSourceResult(request, ModelState));
            }

        }

        public ActionResult GetComputeOvertimeListValidate(Att_ComputeOvertimeModel otModel)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ComputeOvertimeModel>(otModel, "Att_ComputeOvertime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }
        /// <summary>
        /// [hieu.Van]
        /// Lấy tất cà dữ liệu Phân Tích Tăng Ca 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComputeOvertimeList([DataSourceRequest] DataSourceRequest request, Att_ComputeOvertimeModel otModel)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ComputeOvertimeModel>(otModel, "Att_ComputeOvertime", ref message);
            if (!checkValidate)
            {
                //return Json(message);
            }
            #endregion
            var otService = new Att_OvertimeServices();
            var hrService = new ActionService(UserLogin);
             var ActionService = new ActionService(UserLogin);
            Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity = new Att_OvertimeInfoFillterAnalyze()
            {
                isAllowGetOTOutterShift = otModel.isAllowGetOTOutterShift,
                isAllowGetBeforeShift = otModel.isAllowGetBeforeShift,
                isAllowGetAfterShift = otModel.isAllowGetAfterShift,
                isAllowGetInShift = otModel.isAllowGetInShift,
                isAllowGetTypeBaseOnActualDate = otModel.isAllowGetTypeBaseOnActualDate,
                isAllowGetTypeBaseOnBeginShift = otModel.isAllowGetTypeBaseOnBeginShift,
                isAllowGetTypeBaseOnEndShift = otModel.isAllowGetTypeBaseOnBeginShift,
                isAllowGetNightShift = otModel.isAllowGetNightShift,
                MininumOvertimeHour = otModel.MininumOvertimeHour
            };

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(otModel.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (otModel.ProfileID != null)
            {
                var lst = otModel.ProfileID.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }

                if (otModel.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(otModel.ProfileID);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            #endregion

            #region OvertimeType Process
            List<string> lstTypeData = new List<string>();
            if (otModel.Type != null)
            {
                lstTypeData = otModel.Type.Split(',').ToList();
            }
            else
            {
                lstTypeData = new List<string>();
                lstTypeData.Add(EnumDropDown.ComputeOvertimeType.E_DATA_OT.ToString());
                lstTypeData.Add(EnumDropDown.ComputeOvertimeType.E_DATA_NON_OT.ToString());
                lstTypeData.Add(EnumDropDown.ComputeLeavedayType.E_DATA_LEAVE.ToString());
                lstTypeData.Add(EnumDropDown.ComputeLeavedayType.E_DATA_NON_LEAVE.ToString());
            }
            #endregion

            List<Att_OvertimeModel> result = new List<Att_OvertimeModel>();
            DataTable rsDataTable = new DataTable();
            if (otModel.FilterCompute == null || otModel.FilterCompute == "Filter_All")
            {
                rsDataTable = otService
                    .LoadDataAnalyzeOvertime(otModel.OrgStructureID, otModel.DateFrom, otModel.DateTo, lstProfileIDs, lstTypeData, _OvertimeInfoFillterAnalyzeEntity,UserLogin);

                //result = rsDataTable.Translate<Att_OvertimeModel>();
            }
            if (otModel.FilterCompute == "Filter_NotLimit")
            {
                rsDataTable = otService
                    .LoadDataAnalyzeOvertime_DataNotLimit(otModel.OrgStructureID, otModel.DateFrom, otModel.DateTo, lstProfileIDs, lstTypeData, _OvertimeInfoFillterAnalyzeEntity, UserLogin);
            }
            if (otModel.FilterCompute == "Filter_Limit")
            {
                rsDataTable = otService
                    .LoadDataAnalyzeOvertime_DataLimit(otModel.OrgStructureID, otModel.DateFrom, otModel.DateTo, lstProfileIDs, lstTypeData, _OvertimeInfoFillterAnalyzeEntity, UserLogin);
            }

            Att_OvertimeModel aTSource = null;
            if (rsDataTable.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Att_OvertimeModel).GetProperties(flags)
                                     select new
                                     {
                                         Name = aProp.Name,
                                         Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                     }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in rsDataTable.Columns
                                         select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
                foreach (DataRow dataRow in rsDataTable.AsEnumerable().ToList())
                {
                    aTSource = new Att_OvertimeModel();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        if (dataRow[aField.Name] == DBNull.Value)
                            continue;
                        propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                    }
                    result.Add(aTSource);
                }
            }

            #region [Hien.Nguyen] tách ngày và giờ ra
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.TempTimeIn = item.InTime == null ? "" : item.InTime.Value.ToString(@"HH:mm:ss");
                    item.TempTimeOut = item.OutTime == null ? "" : item.OutTime.Value.ToString(@"HH:mm:ss");
                }
            }
            #endregion

            #region Process Excel
            if (otModel.IsExport)
            {
                var fullPath = ExportService.Export(otModel.ExportID, rsDataTable);
                return Json(fullPath);
            }
            #endregion


            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            //return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            //if (dataSourceResult.Data.ToString().Length >= Int32.MaxValue)
            //{
            //    return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
            //else
            //{
            return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
            //Trả về dữ liệu lớn nên dùng kiểu return như thế này

        }

        #region Att_CompensationDetail
        public ActionResult GetCompensationDetailList([DataSourceRequest] DataSourceRequest request, Att_CompensationDetailSearchModel model)
        {

            object obj = new Att_CompensationDetailEntity();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_CompensationDetailEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            Att_CompensationDetailSearchV2Model mSearch = new Att_CompensationDetailSearchV2Model();
            mSearch.Year = model.Year;
            mSearch.OrgStructureID = model.OrgStructureID;
            mSearch.ProfileName = model.ProfileName;
            mSearch.CodeEmp = model.CodeEmp;

            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(mSearch)
            };
            var status = string.Empty;
            var listEntity = service.GetData<Att_AnnualDetailEntity>(lstModel, ConstantSql.hrm_att_sp_get_CompensationDetail, ref status);
            #region xử lý xuất báo cáo
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, listEntity);
                return Json(fullPath);
            }
            #endregion

            var listModel = new List<Att_CompensationDetailModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                listModel = listEntity.Translate<Att_CompensationDetailModel>();
                var dataSourceResult = listModel.ToDataSourceResult(request);
                dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().TotalRow;
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError("Id", status);
            return Json(listModel.ToDataSourceResult(request, ModelState));
        }

        public ActionResult ComputeCompensationDetailList([DataSourceRequest] DataSourceRequest request, Att_CompensationDetailSearchModel model)
        {

            var service = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(model.ProfileName);
            lstObj.Add(model.CodeEmp);
            var lstProfileidentity = service.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgN, ref status);
            if (lstProfileidentity != null)
            {
                var lstProfileIds = lstProfileidentity.Select(s => s.ID).ToList();
                Att_CompensationDetailServices attServiceCompensationDetail = new Att_CompensationDetailServices();
                attServiceCompensationDetail.AnalyzeCompensation(model.Year, lstProfileIds);
                return Json("Success");
            }
            return Json("Success");
        }
        #endregion


        /// <summary>
        /// [Thong.Huynh] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnnualLeaveList([DataSourceRequest] DataSourceRequest request, Att_AnnualLeaveSearchModel Model)
        {
            return GetListDataAndReturn<Att_AnnualLeaveModel, Att_AnnualLeaveEntity, Att_AnnualLeaveSearchModel>(request, Model, ConstantSql.hrm_att_sp_get_AnnualLeave);
        }


        [HttpPost]
        public ActionResult GetAnnualDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualDetailSearchModel model)
        {

            object obj = new Att_AnnualDetailEntity();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_AnnualDetailEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            Att_AnnualDetailSearchV2Model mSearch = new Att_AnnualDetailSearchV2Model();
            mSearch.Year = model.Year;
            mSearch.OrgStructureID = model.OrgStructureID;
            mSearch.ProfileName = model.ProfileName;
            mSearch.CodeEmp = model.CodeEmp;

            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(mSearch)
            };
            var status = string.Empty;
            var listEntity = service.GetData<Att_AnnualDetailEntity>(lstModel, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);

            #region xử lý xuất báo cáo
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, listEntity);
                return Json(fullPath);
            }
            #endregion

            var listModel = new List<Att_AnnualDetailModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                listModel = listEntity.Translate<Att_AnnualDetailModel>();
                var dataSourceResult = listModel.ToDataSourceResult(request);
                dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().TotalRow;
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError("Id", status);
            return Json(listModel.ToDataSourceResult(request, ModelState));

            //return GetListDataAndReturn<Att_AnnualDetailModel, Att_AnnualDetailEntity, Att_AnnualDetailSearchV2Model>(request, mSearch, ConstantSql.hrm_att_sp_get_AnnualDetail);
        }

        /// <summary>
        /// [Hieu.Van]
        /// Lấy dữ liệu phân tích Phép Năm - Hàm bản 7-ko dùng cho HonDa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputeAnnualDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualDetailSearchModel model)
        {
            string status = string.Empty;
            var service = new Att_AnnualDetailServices();
             var ActionService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            bool IsApplyNewStyleAnnualLeave = false;
            string ALLOWANALYZEANNUAL = AppConfig.HRM_ATT_ANNUALDETAIL_ALLOWANALYZEANNUAL.ToString();
            var Config = ActionService.GetData<Sys_AllSettingEntity>(ALLOWANALYZEANNUAL, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).FirstOrDefault();
            if (Config != null && Config.Value1 != null && Config.Value1 == bool.TrueString)
            {
                IsApplyNewStyleAnnualLeave = true;
            }
            if (IsApplyNewStyleAnnualLeave == false)
                return Json("");

            var result = service.AnalyzeAnnualDetail(model.Year, model.OrgStructureID, model.PayrollGroup, model.ProfileName, model.CodeEmp,UserLogin);

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() > 0 ? result.Count() : 0;
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Tung.Ly 20140709]
        /// Lấy dữ liệu phân tích ngày nghỉ chi tiết hàng năm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnnualLeaveDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualLeaveDetailSearchModel model)
        {
            var service = new Att_AnnualLeaveDetailServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            var result = service.SearchAnnualLeaveDetail(model.Year, model.OrgStructureID, model.Type, UserLogin);
            List<Att_AnnualLeaveDetailModel> lstResult = new List<Att_AnnualLeaveDetailModel>();
            if (result.Count > 0)
            {
                lstResult = result.Translate<Att_AnnualLeaveDetailModel>();
            }
            #region xử lý xuất báo cáo

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstResult);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = lstResult.ToDataSourceResult(request);
            dataSourceResult.Total = lstResult.Count() > 0 ? lstResult.Count() : 0;
            return Json(lstResult.ToDataSourceResult(request));
        }

        #region Phép Ốm
        /// <summary>
        /// [Tung.Ly 20140709]
        /// Lấy dữ liệu phân tích phép ốm hàng năm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnnualSickLeaveDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualSickLeaveDetailSearchModel model)
        {
            var service = new Att_AnnualSickLeaveDetailServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            var result = service.SearchAnnualSickLeaveDetail(model.Year, model.OrgStructureID, model.ProfileStatus,UserLogin);
            List<Att_AnnualLeaveDetailModel> lstResult = new List<Att_AnnualLeaveDetailModel>();
            if (result.Count > 0)
            {
                lstResult = result.Translate<Att_AnnualLeaveDetailModel>();
            }
            #region xử lý xuất báo cáo

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstResult);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = lstResult.ToDataSourceResult(request);
            dataSourceResult.Total = lstResult.Count() > 0 ? lstResult.Count() : 0;
            return Json(lstResult.ToDataSourceResult(request));

        }

        #endregion

        #region Ds BHXH

        /// <summary>
        /// Lấy dữ liệu phân tích BHXH
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnnualInsuranceLeaveDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualInsuranceLeaveDetailSearchModel model)
        {
            var service = new Att_AnnualInsuranceLeaveDetailServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            var result = service.SearchAnnualInsuranceLeaveDetail(model.Year, model.OrgStructureID, model.ProfileStatus,UserLogin);
            List<Att_AnnualLeaveDetailModel> lstResult = new List<Att_AnnualLeaveDetailModel>();
            if (result.Count > 0)
            {
                lstResult = result.Translate<Att_AnnualLeaveDetailModel>();
            }
            #region xử lý xuất báo cáo

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstResult);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = lstResult.ToDataSourceResult(request);
            dataSourceResult.Total = lstResult.Count() > 0 ? lstResult.Count() : 0;
            return Json(lstResult.ToDataSourceResult(request));
        }


        #endregion
        /// <summary>
        /// [Bang.nguyen] - 20/10/2014
        /// Validate màn hình danh sách xác nhận tăng ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetOvertimeConfirmValidate(Att_OvertimeSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeSearchModel>(model, "Att_OvertimeConfirm", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);
        }
        /// <summary>
        /// [Hieu.Van] - 05/06/2014
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOvertimeConfirm([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchModel model)
        {


             var ActionService = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            Att_OvertimeServices repo = new Att_OvertimeServices();

            DateTime? DateFrom = SqlDateTime.MinValue.Value;
            DateTime? DateTo = SqlDateTime.MaxValue.Value;
            List<Guid?> lstOTType = model.OTType;
            double? RegisterFrom = 0;
            double? RegisterTo = 0;
            double? ApproveFrom = 0;
            double? ApproveTo = 0;
            //List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateStart != null)
            {
                DateFrom = model.DateStart.Value;
            }
            if (model.DateEnd != null)
            {
                DateTo = model.DateEnd.Value;
            }

            if (model.OTRegisterFrom != null)
            {
                RegisterFrom = model.OTRegisterFrom.Value;
            }
            if (model.OTRegisterTo != null)
            {
                RegisterTo = model.OTRegisterTo.Value;
            }

            if (model.OTApproveFrom != null)
            {
                ApproveFrom = model.OTApproveFrom.Value;
            }
            if (model.OTApproveTo != null)
            {
                ApproveTo = model.OTApproveTo.Value;
            }

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(model.ProfileIDs);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }
            #endregion

            var result = repo.GetOvertimeConfirm(RegisterFrom, RegisterTo, ApproveFrom, ApproveTo, DateFrom, DateTo, lstProfileIDs, lstOTType, UserLogin).ToList().Translate<Att_OvertimeModel>();

            #region [Hien.Nguyen] Tách ngày và giờ ra riêng
            if (result != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].TempTimeIn = result[i].InTime == null ? "" : result[i].InTime.Value.ToString(@"HH:mm:ss");
                    result[i].TempTimeOut = result[i].OutTime == null ? "" : result[i].OutTime.Value.ToString(@"HH:mm:ss");
                }
            }
            #endregion

            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count > 0 ? result.Count : 0;
            //return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue };
        }
        /// <summary>
        /// [Tam.Le] - 2014/08/08
        /// Báo cáo nghỉ chờ duyệt
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        public ActionResult ExportLeavedayWaitApproveReport([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel Model)
        {
            var service = new Att_LeavedayServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateStart != null)
            {
                From = Model.DateStart.Value;
            }
            if (Model.DateEnd != null)
            {
                To = Model.DateEnd.Value;
            }
            string errorMess = string.Empty;
            List<Guid> lstOrgStructureID = null;
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                lstOrgStructureID = Model.OrgStructureID
                  .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => new Guid(x))
                  .ToList<Guid>();
            }

            var result = service.LoadData(From, To, lstOrgStructureID, Model.ProfileName, Model.CodeEmp, Model.LeaveDayTypeID.Value, out errorMess);
            var rs = result.Translate<Att_LeaveDayModel>();
            #region xử lý xuất báo cáo

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = rs.ToDataSourceResult(request);
            dataSourceResult.Total = rs.Count() <= 0 ? 0 : rs.Count();
            return Json(rs.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLeaveDayList([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel Model)
        {
            return GetListDataAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_LeavedaySearchModel>(request, Model, ConstantSql.hrm_att_sp_get_Leaveday);
        }


        /// <summary>
        /// [Hieu.Van] 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLeaveDayApprovedList([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            //return GetListDataAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_LeavedaySearchModel>(request, model, ConstantSql.hrm_att_sp_get_Leaveday);

            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;
            if (!string.IsNullOrEmpty(model.Status))
                model.Status += ",";
            var listEntity = service.GetData<Att_LeaveDayEntity>(lstModel, ConstantSql.hrm_att_sp_get_Leaveday, ref status);
            var listModel = new List<Att_LeaveDayModel>();
            if (listEntity != null && listEntity.Count > 0)
            {
                if (!string.IsNullOrEmpty(model.Status) && model.Status != (LeaveDayStatus.E_FIRST_APPROVED.ToString() + ","))
                    listEntity = listEntity.Where(s => !(s.Status == LeaveDayStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == model.SysUserID)).ToList();
                if (listEntity != null && listEntity.Count > 0)
                {
                    request.Page = 1;
                    listModel = listEntity.Translate<Att_LeaveDayModel>();
                    var dataSourceResult = listModel.ToDataSourceResult(request);
                    if (listModel.FirstOrDefault().TotalRow != null)
                    {
                        dataSourceResult.Total = listModel.Count() <= 0 ? 0 : listModel.FirstOrDefault().TotalRow;
                    }
                    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                }
            }

            var dataSource = listModel.ToDataSourceResult(request);
            return Json(dataSource, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLeaveDayListPortal([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel Model)
        {
            return GetListDataAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_LeavedaySearchModel>(request, Model, ConstantSql.hrm_att_sp_get_LeavedayPortal);
        }

        [HttpPost]
        public ActionResult GetLeaveDayListPersonal([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            var service = new ActionService(UserLogin);
            List<Att_LeaveDayModel> listModel = new List<Att_LeaveDayModel>();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            string status = string.Empty;
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = Common.DotNetToOracle(model.SysUserID.Value.ToString());
            objpara[1] = request.Page;
            objpara[2] = request.PageSize;
            var listLeaveday = service.GetData<Att_LeaveDayEntity>(objpara, ConstantSql.hrm_att_sp_get_LeaveDayByProfileIds, ref status);

            if (listLeaveday != null && listLeaveday.Count > 0)
                listModel = listLeaveday.Translate<Att_LeaveDayModel>();
            request.Page = 1;
            var dataSourceResult = listModel.ToDataSourceResult(request);
            if (listLeaveday != null && listLeaveday.Count > 0 && listModel.FirstOrDefault().TotalRow != null)
            {
                dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().TotalRow;
            }
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOvertimeListPersonal([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            var service = new ActionService(UserLogin);
            List<Att_OvertimeModel> listModel = new List<Att_OvertimeModel>();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            string status = string.Empty;
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = model.SysUserID;
            objpara[1] = request.Page;
            objpara[2] = request.PageSize;
            var listOvertime = service.GetData<Att_OvertimeEntity>(objpara, ConstantSql.hrm_att_sp_get_OvertimeByProfileId, ref status);

            if (listOvertime != null && listOvertime.Count > 0)
            {
                listModel = listOvertime.Translate<Att_OvertimeModel>();
                listModel = listModel.OrderByDescending(s => s.WorkDate).ToList();
            }
            request.Page = 1;
            var dataSourceResult = listModel.ToDataSourceResult(request);
            if (listOvertime != null && listOvertime.Count > 0 && listModel.FirstOrDefault().TotalRow != null)
            {
                dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().TotalRow;
            }
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        public Att_RosterEntity GetRosterByPriotiy(List<Att_RosterEntity> lstRoster)
        {
            string E_TIME_OFF = RosterType.E_TIME_OFF.ToString();
            Att_RosterEntity lstRosterTimeOff = lstRoster.Where(m => m.Type == E_TIME_OFF).OrderByDescending(m => m.DateCreate).FirstOrDefault();
            if (lstRosterTimeOff != null)
                return lstRosterTimeOff;
            string E_CHANGE_SHIFT = RosterType.E_CHANGE_SHIFT.ToString();
            Att_RosterEntity lstRosterChangeShift = lstRoster.Where(m => m.Type == E_CHANGE_SHIFT).OrderByDescending(m => m.DateCreate).FirstOrDefault();
            if (lstRosterChangeShift != null)
                return lstRosterChangeShift;
            string E_ABNORMAL = RosterType.E_ABNORMAL.ToString();
            Att_RosterEntity lstRosterAbnormal = lstRoster.Where(m => m.Type == E_ABNORMAL).OrderByDescending(m => m.DateCreate).FirstOrDefault();
            if (lstRosterAbnormal != null)
                return lstRosterAbnormal;
            Att_RosterEntity lstRosterNormal = lstRoster.OrderByDescending(m => m.DateCreate).FirstOrDefault();
            return lstRosterNormal;
        }



        public ActionResult GetRosterForCheckLeaveDay(string profileId, string dateStart, string dateEnd, string ShiftType)
        {
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            Att_ComputeLeaveDayModel listModel = new Att_ComputeLeaveDayModel();
            double leaveDays = 0;
            double leaveHours = 0;
            if (!string.IsNullOrEmpty(profileId) && !string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
            {
                var guiId = new Guid(profileId);
                var start = DateTime.Parse(dateStart);
                var end = DateTime.Parse(dateEnd);
                var listRoster = service.GetData<Att_RosterEntity>(guiId, ConstantSql.hrm_att_sp_get_RosterByProfileId, ref status);
                string rosterType = RosterType.E_TIME_OFF.ToString();
                string E_CHANGE_SHIFT = RosterType.E_CHANGE_SHIFT.ToString();
                if (listRoster != null)
                {
                    for (DateTime i = start; i <= end; i = i.AddDays(1))
                    {
                        var roster = GetRosterByPriotiy(listRoster.Where(d => d.DateStart <= i && d.DateEnd >= i && d.Status == RosterStatus.E_APPROVED.ToString()).ToList());
                        if (roster != null)
                        {
                            if (ShiftType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                            {
                                var shift = SearchShift(roster, i);
                                if (shift != null && shift != Guid.Empty)
                                {
                                    var entity = service.GetByIdUseStore<Cat_ShiftEntity>((Guid)shift, ConstantSql.hrm_cat_sp_get_ShiftById, ref status);
                                    if (entity != null && entity.StdWorkHours != null && entity.WorkHours != null && entity.StdWorkHours != entity.WorkHours)
                                    {
                                        leaveDays += (entity.WorkHours.Value / entity.StdWorkHours.Value);
                                    }
                                    else
                                    {
                                        leaveDays++;
                                    }
                                    if (entity.WorkHours != null)
                                        leaveHours += entity.WorkHours.Value;
                                }
                            }
                            else
                            {
                                var shift = SearchShift(roster, i);
                                if (shift != null && shift != Guid.Empty)
                                {
                                    var entity = service.GetByIdUseStore<Cat_ShiftEntity>((Guid)shift, ConstantSql.hrm_cat_sp_get_ShiftById, ref status);
                                    if (entity != null)
                                    {
                                        var totalHour = entity.CoOut - (entity.CoBreakOut - entity.CoBreakIn);

                                        if (ShiftType == LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString())
                                        {
                                            listModel.HoursFrom = entity.InTime;
                                            listModel.HoursTo = entity.InTime.AddHours(entity.CoBreakIn);

                                            leaveHours = entity.CoBreakIn;
                                        }
                                        else if (ShiftType == LeaveDayDurationType.E_LASTHALFSHIFT.ToString())
                                        {
                                            listModel.HoursFrom = entity.InTime.AddHours(entity.CoBreakOut);
                                            listModel.HoursTo = entity.InTime.AddHours(entity.CoOut);

                                            leaveHours = entity.CoOut - entity.CoBreakOut;
                                        }
                                        else if (ShiftType == LeaveDayDurationType.E_MIDDLEOFSHIFT.ToString())
                                        {
                                            listModel.HoursFrom = entity.InTime.AddHours(entity.CoBreakIn);
                                            listModel.HoursTo = entity.InTime.AddHours(entity.CoBreakOut);

                                            leaveHours = entity.CoBreakOut - entity.CoBreakIn;
                                        }
                                        else
                                        {
                                            leaveHours = totalHour / 2;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }

                    if (ShiftType == LeaveDayDurationType.E_FULLSHIFT.ToString() && leaveDays == 0)
                    {
                        listModel.Messages = "Không có lịch làm việc trong khoảng thời gian này";
                    }
                    if (!(ShiftType == LeaveDayDurationType.E_FULLSHIFT.ToString()) && leaveHours == 0)
                    {
                        listModel.Messages = "Không có ca làm việc phù hợp";
                    }
                }
            }
            listModel.LeaveDays = leaveDays;
            listModel.LeaveHours = leaveHours;
            return Json(listModel);
        }

        #region Tìm ca trong lịch làm việc

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

        #endregion


        /// <summary>
        /// [Tam.Le - 20/06/2014
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>      
        [HttpPost]
        public ActionResult GetGradeList([DataSourceRequest] DataSourceRequest request, Att_GradeSearchModel GradeModel)
        {
            return GetListDataAndReturn<Att_GradeModel, Att_GradeEntity, Att_GradeSearchModel>(request, GradeModel, ConstantSql.hrm_att_sp_get_Att_Grade);
        }



        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAttendanceTableList([DataSourceRequest] DataSourceRequest request, Att_AttendanceTableModel Model)
        {
            return GetListDataAndReturn<Att_AttendanceTableModel, Att_AttendanceTableEntity, Att_AttendanceTableModel>(request, Model, ConstantSql.hrm_att_sp_get_AttendanceTable);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCutOffDurationList([DataSourceRequest] DataSourceRequest request, Att_CutOffDurationSearchModel Model)
        {
            return GetListDataAndReturn<Att_CutOffDurationModel, Att_CutOffDurationEntity, Att_CutOffDurationSearchModel>(request, Model, ConstantSql.hrm_att_sp_get_CutOffDurations);
        }

        public ActionResult ExportAllCutOffDurationlList([DataSourceRequest] DataSourceRequest request, Att_CutOffDurationSearchModel model)
        {
            return ExportAllAndReturn<Att_CutOffDurationEntity, Att_CutOffDurationModel, Att_CutOffDurationSearchModel>(request, model, ConstantSql.hrm_att_sp_get_CutOffDurations);
        }


        /// <summary>
        /// Kiểm tra đăng ký tăng ca trùng khi tạo mới OT
        /// </summary>
        /// <param name="attOvertime"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AnalysisOvertime_Duplicate([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime, [Bind(Prefix = "params")]Att_OvertimeModel model)
        {
            if (attOvertime == null || attOvertime.Count() <= 0)
            {
                attOvertime = new List<Att_OvertimeModel>();
                if (model != null)
                    attOvertime.Add(model);
            }
            var lstDuplicate = new Att_OvertimeModel();
            List<Att_OvertimeEntity> lstOvertime = attOvertime.Translate<Att_OvertimeEntity>();
            Att_OvertimeServices service = new Att_OvertimeServices();
            var result = service.checkDuplidate_Overtime(lstOvertime, UserLogin);
            if (result)
            {
                lstDuplicate.ActionStatus = "Success";
            }
            else
            {
                lstDuplicate.ActionStatus = "Error";
            }
            return Json(lstDuplicate);
        }

        /// <summary>
        /// [Kiet.Chung] 17/12/2014
        /// Validate BC Dữ Liệu Không Hợp Lệ - DL Gốc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetRptExceptionDataValidate(Att_RptExceptionDataSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_RptExceptionDataSearchModel>(model, "Att_RptExceptionDataSearch", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            return Json(message);
            #endregion
        }
        public ActionResult GetRptExceptionData([DataSourceRequest] DataSourceRequest request, Att_RptExceptionDataSearchModel model)
        {
            string message = "";
            var service = new Att_RptExceptionDataServices();
            var Result = service.GetAtt_RptExceptionData(model.DateFrom, model.DateTo, model.OrgStructureIDs, model.NoScan, model.DifferenceMoreRoster, model.Difference, model.LessHours, model.MissInOut, out message, model.UserExport, UserLogin);
            var lstResult = new List<Att_RptExceptionDataModel>();
            if (Result != null)
                lstResult = Result.Translate<Att_RptExceptionDataModel>();
            var isDataTable = false;
            object obj = new Att_RptExceptionDataEntity();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstResult;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_RptExceptionDataSearchModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstResult, null, model.ExportType);
                // var fullPath = ExportService.Export(model.ExportId, lstResult);
                return Json(fullPath);
            }
            return Json(lstResult.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate Ds Tăng ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AnalysisOvertimeListValidate(Att_OvertimeAnalysisModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeAnalysisModel>(model, "Att_Overtime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);

            }

            string status = string.Empty;
            var ActionServices = new ActionService(UserLogin);

            #region check so h dang ky
            var key = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString();
            var lstSysAllSetting = ActionServices.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).Select(s => s.Value1).FirstOrDefault();
            if (lstSysAllSetting != null)
            {
                var registerConfig = Double.Parse(lstSysAllSetting);
                if (model.RegisterHours > registerConfig)
                {
                    var ls = new object[] { "errorRegisterHours", message };
                    return Json(ls);
                }
            }
            #endregion

            #region Check Thai Sản
            var lstID = model.ProfileID.Split(',');
            List<Guid> lstGuid = lstID.Select(s => Guid.Parse(s)).ToList();
            var keyPreg = AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME.ToString();
            var lstSysPreg = ActionServices.GetData<Sys_AllSettingEntity>(keyPreg, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).Select(s => s.Value1).FirstOrDefault();

            if (lstSysPreg != null && lstSysPreg == bool.FalseString)
            {
                List<object> lstPreg = new List<object>();
                lstPreg.AddRange(new object[8]);
                lstPreg[0] = model.ProfileName;
                lstPreg[6] = 1;
                lstPreg[7] = Int32.MaxValue - 1;
                var lstPregnancy = ActionServices.GetData<Att_PregnancyEntity>(lstPreg, ConstantSql.hrm_att_sp_get_Pregnancy, ref status);
                if (lstPregnancy.Count > 0)
                {
                    lstPregnancy = lstPregnancy.Where(s => lstGuid.Contains(s.ProfileID) && s.DateStart.Value <= model.WorkDate.Value && s.DateEnd.Value >= model.WorkDate.Value).ToList();
                    if (lstPregnancy.Count > 0)
                    {
                        var ls = new object[] { "errorPregnancy", message };
                        return Json(ls);
                    }
                }
            }

            #endregion

            return Json(message);
            #endregion
        }

        /// <summary>
        /// [Hien.Nguyen] 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// ([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime, [Bind(Prefix = "params")]Att_OvertimeModel model)
        public ActionResult AnalysisOvertimeListValidatePortal([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertime, [Bind(Prefix = "params")]Att_OvertimeAnalysisModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeAnalysisModel>(model, "Att_Overtime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);

            }

            string status = string.Empty;
            var ActionServices = new ActionService(UserLogin);

            #region check so h dang ky
            var key = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString();
            var lstSysAllSetting = ActionServices.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).Select(s => s.Value1).FirstOrDefault();
            if (lstSysAllSetting != null)
            {
                var registerConfig = Double.Parse(lstSysAllSetting);
                if (model.RegisterHours > registerConfig)
                {
                    var ls = new object[] { "errorRegisterHours", message };
                    return Json(ls);
                }
            }
            #endregion

            #region Check Thai Sản
            var lstID = model.ProfileID.Split(',');
            List<Guid> lstGuid = lstID.Select(s => Guid.Parse(s)).ToList();
            var keyPreg = AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME.ToString();
            var lstSysPreg = ActionServices.GetData<Sys_AllSettingEntity>(keyPreg, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).Select(s => s.Value1).FirstOrDefault();

            if (lstSysPreg != null && lstSysPreg == bool.FalseString)
            {
                List<object> lstPreg = new List<object>();
                lstPreg.AddRange(new object[8]);
                lstPreg[0] = model.ProfileName;
                lstPreg[6] = 1;
                lstPreg[7] = Int32.MaxValue - 1;
                var lstPregnancy = ActionServices.GetData<Att_PregnancyEntity>(lstPreg, ConstantSql.hrm_att_sp_get_Pregnancy, ref status);
                if (lstPregnancy.Count > 0)
                {
                    lstPregnancy = lstPregnancy.Where(s => lstGuid.Contains(s.ProfileID) && s.DateStart.Value <= model.WorkDate.Value && s.DateEnd.Value >= model.WorkDate.Value).ToList();
                    if (lstPregnancy.Count > 0)
                    {
                        var ls = new object[] { "errorPregnancy", message };
                        return Json(ls);
                    }
                }
            }

            #endregion

            #region Hien.Nguyen check trung ngay da dang ky
            if (attOvertime != null && attOvertime.Count > 0)
            {
                foreach (var i in attOvertime)
                {
                    DateTime Date1 = new DateTime(i.WorkDate.Year, i.WorkDate.Month, i.WorkDate.Day);
                    DateTime Date2 = new DateTime(model.WorkDate.Value.Year, model.WorkDate.Value.Month, model.WorkDate.Value.Day);
                    if (Date2 == Date1 && (i.WorkDate.Hour * 60 + i.WorkDate.Minute < model.WorkHour.Value.Hour * 60 + model.WorkHour.Value.Minute + model.RegisterHours.Value * 60 && i.WorkDate.Hour * 60 + i.WorkDate.Minute + i.RegisterHours * 60 > model.WorkHour.Value.Hour * 60 + model.WorkHour.Value.Minute))
                    {
                        var ls = new object[] { "error", "Ngày " + Date1.ToString("dd/MM/yyyy") + " đã được đăng ký" };
                        return Json(ls);
                    }
                }
            }
            #endregion

            #region Hieu.Van check trùng đã dk trong Database
            if (model != null)
            {
                List<Att_OvertimeEntity> lstOvertime = new List<Att_OvertimeEntity>();
                Att_OvertimeEntity entity = model.CopyData<Att_OvertimeEntity>();
                if (model.WorkHour != null)
                {
                    var hour = model.WorkHour.Value.Minute + (model.WorkHour.Value.Hour * 60);
                    entity.WorkDate = model.WorkDate.Value.AddMinutes(hour);
                }
                if (model.ProfileID != null)
                {
                    Guid id = Guid.Empty;
                    model.ProfileID = Common.DotNetToOracle(model.ProfileID);
                    Guid.TryParse(model.ProfileID, out id);
                    entity.ProfileID = id;
                }
                entity.ShiftID = model.ShiftID;
                entity.RegisterHours = model.RegisterHours.Value;
                entity.OvertimeTypeID = model.OvertimeTypeID.Value;
                entity.DurationType = model.DurationType;
                entity.MethodPayment = model.MethodPayment;
                entity.UserApproveID = model.UserApproveID.Value;
                entity.UserApproveID2 = model.UserApproveID2;
                entity.ReasonOT = model.ReasonOT;
                entity.Status = model.Status;
                lstOvertime.Add(entity);
                Att_OvertimeServices service = new Att_OvertimeServices();
                var result = service.checkDuplidate_Overtime(lstOvertime, UserLogin);

                if (!result)
                {
                    var ls = new object[] { "error", "Ngày " + model.WorkDate.Value.ToString("dd/MM/yyyy") + " đã được đăng ký" };
                    return Json(ls);
                }
            }
            #endregion


            return Json(message);
            #endregion
        }


        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Phân Tích Khi Tạo Mới Tăng Ca
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        public ActionResult AnalysisOvertimeList([DataSourceRequest] DataSourceRequest request, Att_OvertimeAnalysisModel model)
        {
            #region Validate

            //string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeAnalysisModel>(model, "Att_Overtime", ref message);
            //if (!checkValidate)
            //{
            //    var ls = new object[] { "error", message };
            //    return Json(ls);

            //}

            #endregion
            bool IsAllowSplitShift = true;

            Cat_ShiftServices serviceShift = new Cat_ShiftServices();
            Cat_DayOffServices servicesDayOff = new Cat_DayOffServices();
            Cat_OvertimeTypeServices servicesOvertimeType = new Cat_OvertimeTypeServices();

            List<Att_OvertimeEntity> listOvertime = new List<Att_OvertimeEntity>();
            List<Att_OvertimeModel> lstOT = new List<Att_OvertimeModel>();
            List<Att_OvertimeModel> result = new List<Att_OvertimeModel>();
            Att_OvertimeModel _ot1;
            Att_OvertimeModel _ot2;
            Att_OvertimeEntity _overtimeEntity = null;
            //Sys_UserInfoEntity user1 = new Sys_UserInfoEntity();
            //Sys_UserInfoEntity user2 = new Sys_UserInfoEntity();

            var lstProfileIds = model.ProfileID.Split(',');
            model.WorkDate = Common.JoinTimeInDate(model.WorkDate.Value, model.WorkDateTime);

            //TrƯờng Hợp SD Portal
            if (model.WorkHour != null)
            {
                var hour = (model.WorkHour.Value.Hour * 60) + model.WorkHour.Value.Minute;
                model.WorkDate = model.WorkDate.Value.Date.AddMinutes(hour);
            }

            #region Xử lý trường hợp Không Tách Tăng Ca

            string status = string.Empty;
             var ActionService = new ActionService(UserLogin);
            List<object> lstSys = new List<object>();
            lstSys.Add(AppConfig.HRM_ATT_OT_ISALLOWCUT.ToString());
            lstSys.Add(null);
            lstSys.Add(null);
            var config = ActionService.GetData<Sys_AllSettingEntity>(lstSys, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            if (config != null)
            {
                var _config = config.FirstOrDefault();
                if (_config != null && _config.Value1 != bool.TrueString)
                {
                    IsAllowSplitShift = false;
                }
            }

            #endregion

            #region new
            //checkbox Theo ca làm việc từng người : chưa có time làm
            //model.ByShiftProfile = false;

            //Att_AnalysisOvertimeServices service = new Att_AnalysisOvertimeServices();
            //Att_OvertimeEntity overtime = new Att_OvertimeEntity();
            //string status = OverTimeStatus.E_SUBMIT.ToString();
            //overtime.Status = status;
            //overtime.WorkDate = model.WorkDate.Value;
            //overtime.UserApproveID = model.UserApproveID;
            //overtime.UserApproveID2 = model.UserApproveID2;
            //overtime.RegisterHours = model.RegisterHours;
            //overtime.ShiftID = model.ShiftID;
            //overtime.MethodPayment = model.MethodPayment;
            //overtime.ReasonOT = model.ReasonOT;
            //overtime.DurationType = model.OvertimeDurationType;

            //var lstOT = service.LoadData(overtime, model.ProfileID, model.ByShiftProfile);
            //return new JsonResult() { Data=lstOT.ToDataSourceResult(request),MaxJsonLength=Int32.MaxValue}; 
            #endregion

            #region old
            #region listOvertimeInfoFillterAnalyze - Xử lý Trong Ca-Cuối Ca-Đầu Ca
            Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity = new Att_OvertimeInfoFillterAnalyze()
            {
                isAllowGetOTOutterShift = true,

                isAllowGetTypeBaseOnActualDate = true,
                isAllowGetTypeBaseOnBeginShift = true,
                isAllowGetTypeBaseOnEndShift = false,
                isAllowGetNightShift = true,
                MininumOvertimeHour = 0.5
            };
            if (model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_EARLY.ToString())
            {
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetBeforeShift = true;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetAfterShift = false;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetInShift = false;
            }
            if (model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_LATE.ToString())
            {
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetBeforeShift = false;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetAfterShift = true;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetInShift = false;
            }
            if (model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_UNLIMIT.ToString()
                || model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_SHIFT.ToString()
                || model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_BREAK.ToString())
            {
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetBeforeShift = false;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetAfterShift = false;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetInShift = true;
            }
            if (model.DurationType == EnumDropDown.OvertimeDurationType.E_OT_UNLIMIT.ToString())
            {
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetBeforeShift = true;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetAfterShift = true;
                _OvertimeInfoFillterAnalyzeEntity.isAllowGetInShift = true;
            }

            #endregion

            #region getdata
            var lstShiftItem = new List<Cat_ShiftItemEntity>();
            List<object> lst4Model = new List<object>() { null, null, 1, 50 };
            //for (int i = 0; i < 4; i++)
            //{
            //    lst4Model.Add(null);
            //}
            List<object> ls5Model = new List<object>() { null, null, null, 1, 50 };
            //for (int i = 0; i < 5; i++)
            //{
            //    ls5Model.Add(null);
            //}
            string proStr = Common.DotNetToOracle(model.ProfileID);
            string user = string.Empty;
            var lstShift = ActionService.GetData<Cat_ShiftEntity>(lst4Model, ConstantSql.hrm_cat_sp_get_Shift, ref status);
            var lstDayOff = ActionService.GetData<Cat_DayOffEntity>(ls5Model, ConstantSql.hrm_cat_sp_get_DayOff, ref status);
            var lstOvertimeType = ActionService.GetData<Cat_OvertimeTypeEntity>(ls5Model, ConstantSql.hrm_cat_sp_get_OvertimeType, ref status);
            var lstProfileDetails = ActionService.GetData<Hre_ProfileEntity>(proStr, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);
            //var lstUserInfoDetails = ActionService.GetData<Sys_UserInfoEntity>(user, ConstantSql.hrm_sys_sp_get_user_multi, ref status);
            #endregion
            //user1 = lstUserInfoDetails.Where(s => s.ID == model.UserApproveID).FirstOrDefault();
            //user2 = lstUserInfoDetails.Where(s => s.ID == model.UserApproveID2).FirstOrDefault();
            var shiftDetail = lstShift.Where(s => s.ID == model.ShiftID).FirstOrDefault();

            string statusOT = model.Status;

            foreach (var pro in lstProfileIds)
            {
                result = new List<Att_OvertimeModel>();

                var _profile = lstProfileDetails.Where(s => s.ID == Guid.Parse(pro)).FirstOrDefault();

                #region listOvertime
                _overtimeEntity = new Att_OvertimeEntity()
                {
                    ID = model.ID,
                    ProfileID = Guid.Parse(pro),
                    Status = statusOT,
                    WorkDate = model.WorkDate.Value,
                    RegisterHours = model.RegisterHours.Value,
                    MethodPayment = model.MethodPayment,
                    ReasonOT = model.ReasonOT,
                    ShiftID = model.ShiftID,
                    UserApproveID = model.UserApproveID.Value,
                    UserApproveID2 = model.UserApproveID2
                };
                listOvertime.Add(_overtimeEntity);
                #endregion

                var service = new Att_OvertimeServices();
                if (IsAllowSplitShift)
                {
                    result = service.StartAnalyzeOvertime(listOvertime,
                                                    lstShift,
                                                    lstShiftItem,
                                                    lstDayOff,
                                                    lstOvertimeType,
                                                    _OvertimeInfoFillterAnalyzeEntity, UserLogin).ToList().Translate<Att_OvertimeModel>();
                }
                else
                {
                    _ot1 = new Att_OvertimeModel();

                    _ot1.WorkDate = model.WorkDate.Value;
                    _ot1.RegisterHours = model.RegisterHours.Value;
                    _ot1.Status = model.Status;
                    _ot1.MethodPayment = model.MethodPayment;
                    _ot1.ProfileID = Guid.Parse(pro);
                    _ot1.ShiftID = model.ShiftID;
                    _ot1.UserApproveID = model.UserApproveID.Value;
                    _ot1.UserApproveID3 = model.UserApproveID3;
                    _ot1.UserApproveID2 = model.UserApproveID2;
                    _ot1.OvertimeTypeID = model.OvertimeTypeID.Value;
                    _ot1.UserApproveName2 = model.UserApproveName2;
                    _ot1.UserApproveName3 = model.UserApproveName3;
                    _ot1.UserApproveName1 = model.UserApproveName;
                    _ot1.ReasonOT = model.ReasonOT;
                    _ot1.Status = statusOT;

                    result.Add(_ot1);
                }


                foreach (var item in result)
                {
                    item.ID = Guid.Empty;
                    if (item.ProfileID != Guid.Empty)
                    {
                        item.ProfileName = _profile.ProfileName;
                        item.CodeEmp = _profile.CodeEmp;
                    }
                    if (item.ShiftID != Guid.Empty)
                    {
                        item.ShiftName = shiftDetail.ShiftName;
                    }
                    if (item.OvertimeTypeID != Guid.Empty)
                    {
                        item.OvertimeTypeName = lstOvertimeType.Where(s => s.ID == item.OvertimeTypeID).FirstOrDefault().OvertimeTypeName;
                    }
                    item.DurationType = model.DurationType;
                    item.ReasonOT = model.ReasonOT;
                    item.MethodPayment = model.MethodPayment;
                    item.UserApproveID = model.UserApproveID.Value;
                    item.UserApproveName1 = model.UserApproveName;
                    item.UserApproveID3 = model.UserApproveID3;
                    item.UserApproveName3 = model.UserApproveName3;
                    item.UserApproveID2 = model.UserApproveID2;
                    item.UserApproveName2 = model.UserApproveName2;
                    item.Status = statusOT;
                    item.WorkDateTime = item.WorkDate.ToString("HH:mm");
                    item.RegisterTimeOut = item.WorkDate.AddHours(item.RegisterHours).ToString("HH:mm");
                    lstOT.Add(item);
                }
            }
            request.Page = 1;
            var dataSourceResult = lstOT.ToDataSourceResult(request);
            if (lstOT.Count >= 0)
            {
                dataSourceResult.Total = lstOT.Count() <= 0 ? 0 : (int)lstOT.Count;
            }
            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            //return new JsonResult() { Data = lstOT.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue };
            //return Json(lstOT.ToDataSourceResult(request));
            #endregion
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo chi tiết quẹt thẻ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailSwingCardListValidate(Att_ReportDetailSwingCardModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailSwingCardModel>(model, "Att_ReportDetailSwingCard", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Kiet.Chung] 4/11/2014
        /// BC Số Lần Quên Quẹt Thẻ
        /// </summary>
        public ActionResult GetReportCountLateInOut([DataSourceRequest] DataSourceRequest request, Att_ReportCountLateInOutModel model)
        {
            var service = new Att_ReportServices();
             var ActionService = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            var AttWDService = new Att_WorkDayServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (model.ProfileID != null)
            {
                var lst = model.ProfileID.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(model.ProfileID);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            #endregion
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            List<Att_WorkdayEntity> lstAttWD = AttWDService.GetWorkDaysByDate(DateFrom, DateTo);
            List<Guid> lstProfileId = lstProfileIDs.Select(d => d.ID).ToList();
            lstAttWD = lstAttWD.Where(d => lstProfileId.Contains(d.ProfileID)).ToList();

            List<object> lstObjTANScanReason = new List<object>();
            lstObjTANScanReason.Add(null);
            lstObjTANScanReason.Add(null);
            lstObjTANScanReason.Add(null);
            var resons = ActionService.GetData<Cat_TAMScanReasonMissEntity>(lstObjTANScanReason, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss, ref status).ToList();
            var resonIDs = resons.Select(d => d.ID).ToList();
            List<Att_ReportCountLateInOutModel> lstResult = new List<Att_ReportCountLateInOutModel>();
            foreach (var item in lstProfileIDs)
            {
                Att_ReportCountLateInOutModel result = new Att_ReportCountLateInOutModel();
                var WorkdayProfile = lstAttWD.Where(d => d.ProfileID == item.ID).ToList();
                result.ProfileName = item.ProfileName;
                result.CodeEmp = item.CodeEmp;
                result.CodeOrg = item.OrgStructureName;
                if (model != null && model.IsCreateTemplate || model.ExportId != Guid.Empty)
                {
                    result.DateFrom = DateFrom;
                    result.DateTo = DateTo;
                    result.UserExport = model.UserExport;
                    result.DateExport = DateTime.Today;
                }

                result.EarlyDurationMore2 = WorkdayProfile.Count(s => s.LateEarlyDuration >= 120);
                result.EarlyDurationLess2 = WorkdayProfile.Count(s => s.LateEarlyDuration > 0 && s.LateEarlyDuration < 120);
                double countForget = WorkdayProfile.Count(s => s.InTimeRoot == null && s.MissInOutReason != null && resonIDs.Contains(s.MissInOutReason.Value));
                countForget += WorkdayProfile.Count(s => s.OutTimeRoot == null && s.MissInOutReason != null && resonIDs.Contains(s.MissInOutReason.Value));
                result.CountForget = countForget;
                if (!model.IsNoteAllowZero || result.EarlyDurationMore2 > 0 || result.EarlyDurationLess2 > 0 || result.CountForget > 0)
                    lstResult.Add(result);
            }

            var isDataTable = false;
            object obj = new Att_ReportCountLateInOutModelExport();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstResult;
                isDataTable = true;
            }
            ConfigExport cfgExport = new ConfigExport()
            {
                ExportId = model.ExportId,
                Object = obj,
                DataSource = lstResult,
                FileName = "Att_ReportCountLateInOutModel.xls",
                IsDataTable = isDataTable,
                VariableName = "Att_ReportCountLateInOutModelExport"
            };
            if (model != null && model.IsCreateTemplate)
            {
                cfgExport.IsExport = false;
                ExportService expService = new ExportService();
                var str = expService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                cfgExport.IsExport = true;
                var fullPath = ExportService.ExportByGenCode(cfgExport);
                return Json(fullPath);
            }
            return Json(lstResult.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDetailSwingCardList([DataSourceRequest] DataSourceRequest request, Att_ReportDetailSwingCardModel model)
        {
            var service = new Att_ReportServices();
             var ActionService = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (model.ProfileID != null)
            {
                var lst = model.ProfileID.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(model.ProfileID);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            #endregion
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            string[] _Status = null;
            if (model.Status != null)
                _Status = model.Status.Split(',').ToArray();
            string CardCode = model.CardCode;
            var isDataTable = false;
            object obj = new Att_ReportDetailSwingCardEntity();
            //var result = service.GetReportDetailSwingCard(DateFrom, DateTo, lstProfileIDs, _ShiftIDs, _Status, model.isIncludeQuitEmp, model.OrgStructureID).Distinct();
            var result = service.GetReportDetailSwingCard(DateFrom, DateTo, lstProfileIDs, CardCode, _Status, model.OrgStructureID, model.UserExport, UserLogin);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailSwingCardModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                // var fullPath = ExportService.Export(model.ExportId, result);
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo sai ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportWrongShiftListValidate([DataSourceRequest] DataSourceRequest request, Att_ReportWrongShiftModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportWrongShiftModel>(model, "Att_ReportWrongShift", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Hieu.Van] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportWrongShiftList([DataSourceRequest] DataSourceRequest request, Att_ReportWrongShiftModel model)
        {
             var ActionService = new ActionService(UserLogin);
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = SqlDateTime.MinValue.Value;
            DateTime DateTo = SqlDateTime.MaxValue.Value;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    t = new Hre_ProfileEntity();
                    Guid _Id = new Guid(item);
                    t.ID = _Id;
                    _temp.Add(t);
                }

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    string selectedIds = Common.DotNetToOracle(model.ProfileIDs);
                    lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }
            }
            else
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            #endregion

            //if (model.ShiftID != null)
            //{
            //    model.ShiftIDs.Add(model.ShiftID);
            //}
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var result = service.GetReportWrongShift(DateFrom, DateTo, lstProfileIDs, _ShiftIDs, model.isIncludeQuitEmp, model.UserExport);

            var isDataTable = false;
            object obj = new Att_ReportWrongShiftEntity();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportWrongShiftModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, result, model.ValueFields.Split(','));
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        /// <summary>
        /// [bang.nguyen] 18/11/2014
        /// validate man hinh BC chi tiet cong tung ngay
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDailyAttendanceListValidate([DataSourceRequest] DataSourceRequest request, Att_ReportDailyAttendanceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDailyAttendanceModel>(model, "Att_ReportDailyAttendance", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Hieu.Van] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDailyAttendanceList([DataSourceRequest] DataSourceRequest request, Att_ReportDailyAttendanceModel model)
        {
             var ActionService = new ActionService(UserLogin);
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = SqlDateTime.MinValue.Value;
            DateTime DateTo = SqlDateTime.MaxValue.Value;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Hre_ProfileEntity> _temp = new List<Hre_ProfileEntity>();
            Hre_ProfileEntity t = new Hre_ProfileEntity();
            List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();
            lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            #endregion
            Guid[] _PayrollIDs = null;
            if (model.PayrollIDs != null)
                _PayrollIDs = model.PayrollIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var result = service.GetReportDailyAttendance(DateFrom, DateTo, model.UserExport, lstProfileIDs, _PayrollIDs, model.excludeNotInOut, model.IsCreateTemplate, UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDailyAttendanceModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    valueField = model.ValueFields.Split(',');
                //}
                //var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                //return Json(fullPath);
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath);

            }
            return Json(result.ToDataSourceResult(request));
        }




        /// <summary>
        /// [Son.Vo] - 2014/05/27
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllowLimitOvertimeList([DataSourceRequest] DataSourceRequest request, Att_AllowLimitOvertimeSearchModel Model)
        {
            return GetListDataAndReturn<Att_AllowLimitOvertimeModel, Att_AllowLimitOvertimeEntity, Att_AllowLimitOvertimeSearchModel>(request, Model, ConstantSql.hrm_att_sp_get_AllowLimitOvertime);
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình BC công tổng hợp
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportMonthlyTimeSheetValidate(Att_ReportMonthlyTimeSheetModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportMonthlyTimeSheetModel>(Model, "Att_ReportMonthlyTimeSheet", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Son.Vo] - 2014/05/27
        /// Lấy dữ liệu load lên lưới bằng store - GetReportMonthlyTimeSheet
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportMonthlyTimeSheet([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyTimeSheetModel Model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            //if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureID;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            //List<Guid> lstProfileIDs = hrService.GetProfileIdsByOrg(strOrgIDs);
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)Model.OrgStructureID;
            #region MyRegion
            //if (Model.ShiftID != null)
            //{
            //    Model.ShiftIDs.Add(Model.ShiftID);
            //}
            //if (Model.PayrollID != null)
            //{
            //    Model.PayrollIDs.Add(Model.PayrollID);
            //}
            #endregion
            Guid[] _ShiftIDs = null;
            if (Model.ShiftIDs != null)
                _ShiftIDs = Model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _PayrollIDs = null;
            if (Model.PayrollIDs != null)
                _PayrollIDs = Model.PayrollIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();

            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportMonthlyTimeSheet(From, To, Model.OrgStructureID, lstProfile, _ShiftIDs, _PayrollIDs, Model.isIncludeQuitEmp, Model.NoDisplay0Data, Model.ExportId, UserLogin);
            // var result = new DataTable();
            // result = result.Translate<Att_ReportMonthlyTimeSheetModel>();


            if (Model.ExportId != Guid.Empty)
            {
                foreach (DataColumn column in result.Columns)
                {
                    Model.ValueFields = Model.ValueFields + "," + column.ColumnName;
                }
                var row = result.Rows.Count;
                result.Rows[row - 1].Delete();

                string[] valueField = null;
                if (Model.ValueFields != null)
                {
                    valueField = Model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(Model.ExportId, valueField, result, Model.ExportType);
                return Json(fullPath);
            }
            //return new JsonResult() { Data = result.ToDataSourceResult(request) , MaxJsonLength=int.MaxValue};

            //if (Model.ExportId != Guid.Empty)
            //{
            //    var fullPath = ExportService.Export(Model.ExportId, result);
            //    //[Hien.Nguyen] Do Path trả về lỗi nhưng chưa tìm đc nguyên nhân nên dùng cách replace path
            //    return Json(fullPath.ToString().Replace("Success,", "").ToString());
            //}

            return Json(result.ToDataSourceResult(request));
        }


        public ActionResult GetReportGeneralMonthlyAttendanceValidate(Att_ReportGeneralMonthlyAttendanceModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportGeneralMonthlyAttendanceModel>(Model, "Att_ReportGeneralMonthlyAttendance", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        public ActionResult GetReportGeneralMonthlyAttendance([DataSourceRequest] DataSourceRequest request, Att_ReportGeneralMonthlyAttendanceModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            //if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureID;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            //List<Guid> lstProfileIDs = hrService.GetProfileIdsByOrg(strOrgIDs);
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            #region MyRegion
            //if (Model.ShiftID != null)
            //{
            //    Model.ShiftIDs.Add(Model.ShiftID);
            //}
            //if (Model.PayrollID != null)
            //{
            //    Model.PayrollIDs.Add(Model.PayrollID);
            //}
            #endregion
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _PayrollIDs = null;
            if (model.PayrollIDs != null)
                _PayrollIDs = model.PayrollIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            string[] _StatusEmployees = null;
            if (model.StatusEmployees != null)
                _StatusEmployees = model.StatusEmployees.Split(',').Select(s => s).ToArray();
            string[] _HourMonthlyWorkings = null;
            if (model.HourMonthlyWorkings != null)
                _HourMonthlyWorkings = model.HourMonthlyWorkings.Split(',').Select(s => s).ToArray();

            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportGeneralMonthlyAttendance(From, To, model.OrgStructureID, lstProfile, _ShiftIDs, _PayrollIDs, model.StatusEmployees, model.GroupByTypes, model.ExcludeIfWorkingDayIsZero, model.IsShowOTHourRow, model.UserExport, UserLogin);
            // var result = new DataTable();
            //var rs = result.Translate<Att_ReportMonthlyTimeSheetModel>();

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportGeneralMonthlyAttendanceModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //}
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();

                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    valueField = model.ValueFields.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            else
            {
                result.Columns.Remove("DateFrom");
                result.Columns.Remove("DateTo");
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
        }

        public ActionResult GetReportMonthlyRosterListValidate(Att_ReportMonthlyRosterModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportMonthlyRosterModel>(model, "Att_ReportMonthlyRoster", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        public ActionResult GetReportMonthlyRosterList([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyRosterModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }
            string status = string.Empty;
            string profilename = model.ProfileName;
            string codeemp = model.CodeEmp;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(model.OrgStructureID);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            var lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            Guid[] _PayrollIDs = null;
            if (model.PayrollIDs != null)
                _PayrollIDs = model.PayrollIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var result = service.GetReportMonthlyRoster(DateFrom, DateTo, model.OrgStructureID, lstProfileIDs, model.OnlyHolydayNotHaveRoster, _PayrollIDs, model.UserExport, UserLogin);

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportMonthlyRosterModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                //var count = result.Rows.Count;
                //result.Rows[count - 1].Delete();
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                // var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            //return new JsonResult() { Data = result.ToDataSourceResult(request) ,MaxJsonLength=Int32.MaxValue};
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);


        }

        public ActionResult GetReportMonthlyTimeSheetV2Validate(Att_ReportMonthlyTimeSheetModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportMonthlyTimeSheetModel>(Model, "Att_ReportMonthlyTimeSheetV2", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }
        public ActionResult GetReportMonthlyTimeSheetV2([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyTimeSheetModel model)
        {
            var service = new Att_ReportServices();
             var ActionService = new ActionService(UserLogin);
            string status = string.Empty;

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            List<Guid?> OrgIds = new List<Guid?>();

            var result = service.GetReportMonthlyTimeSheetV2(model.WorkHourType, model.GradeAttendanceID, 
                model.CutOffDurationID.Value, model.ProfileID, model.OrgStructureID, model.CodeEmp, model.isIncludeQuitEmp,
                model.NoDisplay0Data, model.IsCreateTemplate, model.UserExport, UserLogin, model.JobTitleID, model.PositionID);

            string cutoff = Common.DotNetToOracle(model.CutOffDurationID.ToString());
            var CutOffDurationEntity = ActionService.GetData<Att_CutOffDurationEntity>(cutoff, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status).FirstOrDefault();

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = CutOffDurationEntity.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = CutOffDurationEntity.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CutOffDurationName", Value = CutOffDurationEntity.CutOffDurationName };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3 };
            object obj = new Att_ReportMonthlyTimeSheetModel();
            var isDataTable = false;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportMonthlyTimeSheetModelV2",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable,
                    HeaderInfo = listHeaderInfo
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                //   var row = result.Rows.Count;
                // result.Rows[row - 1].Delete();

                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            #region mapping dataTable to dataList
            List<Att_ReportMonthlyTimeSheetModel> dataList = new List<Att_ReportMonthlyTimeSheetModel>();
            Att_ReportMonthlyTimeSheetModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Att_ReportMonthlyTimeSheetModel).GetProperties(flags)
                                     select new
                                     {
                                         Name = aProp.Name,
                                         Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                     }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in result.Columns
                                         select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
                foreach (DataRow dataRow in result.AsEnumerable().ToList())
                {
                    aTSource = new Att_ReportMonthlyTimeSheetModel();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        if (dataRow[aField.Name] == DBNull.Value)
                            continue;
                        propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                    }
                    dataList.Add(aTSource);
                }
            }
            #endregion

            return Json(dataList.ToDataSourceResult(request));
        }


        public ActionResult GetReportMonthlyHourFlightLocal([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyTimeSheetModel model)
        {

            var service = new Att_ReportServices();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            List<Guid?> OrgIds = new List<Guid?>();

            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _PayrollIDs = null;
            if (model.PayrollIDs != null)
                _PayrollIDs = model.PayrollIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _workPlaceIDs = null;
            if (model.WorkPlaceIDs != null)
                _workPlaceIDs = model.WorkPlaceIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();


            var result = service.GetReportMonthlyHourFlightLocal(model.CutOffDurationID.Value, model.OrgStructureID, _ShiftIDs, _PayrollIDs, _workPlaceIDs, model.CodeEmp, model.isIncludeQuitEmp, model.NoDisplay0Data, model.IsCreateTemplate, model.UserExport, UserLogin);
            object obj = new DataTable();
            var isDataTable = false;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportMonthlyTimeSheetModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var row = result.Rows.Count;
                result.Rows[row - 1].Delete();

                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            List<Att_ReportMonthlyTimeSheetModel> dataList = new List<Att_ReportMonthlyTimeSheetModel>();
            Att_ReportMonthlyTimeSheetModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Att_ReportMonthlyTimeSheetModel).GetProperties(flags)
                                     select new
                                     {
                                         Name = aProp.Name,
                                         Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                     }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in result.Columns
                                         select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
                foreach (DataRow dataRow in result.AsEnumerable().ToList())
                {
                    aTSource = new Att_ReportMonthlyTimeSheetModel();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        if (dataRow[aField.Name] == DBNull.Value)
                            continue;
                        propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                    }
                    dataList.Add(aTSource);
                }
            }

            return Json(dataList.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo chi tiết quên quẹt thẻ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailForgetCardValidate(Att_ReportDetailForgetCardModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailForgetCardModel>(Model, "Att_ReportDetailForgetCard", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Son.Vo] - 2014/06/02
        /// Lấy dữ liệu load lên lưới bằng store - GetReportDetailShift
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDetailForgetCard([DataSourceRequest] DataSourceRequest request, Att_ReportDetailForgetCardModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }

            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            if (model.ShiftID != null)
            {
                model.ShiftIDs.Add(model.ShiftID);
            }
            var isDataTable = false;
            object obj = new DataTable();

            List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            var result = service.GetReportDetailForgetCard(From, To, lstProfileIDs, model.ShiftIDs, model.UserExport);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailForgetCardModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                // var fullPath = ExportService.Export(Model.ExportId, result);
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }

            //var rs = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            //var dataSourceResult = rs.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.Count();
            //return Json(dataSourceResult);
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo ca và lịch làm việc
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailShiftValidate(Att_ReportDetailShiftModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailShiftModel>(Model, "Att_ReportDetailShift", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        /// <summary>
        /// [Son.Vo] - 2014/06/02
        /// Lấy dữ liệu load lên lưới bằng store - GetReportDetailShift
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDetailShift([DataSourceRequest] DataSourceRequest request, Att_ReportDetailShiftModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            //List<Guid?> OrgIds = new List<Guid?>();
            string OrgIds = string.Empty;
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            if (model.OrgStructureID != null && model.OrgStructureID.Count() > 0)
            {
                OrgIds = model.OrgStructureID;
            }
            List<object> strorgIDs = new List<object>();
            strorgIDs.AddRange(new object[3]);
            strorgIDs[0] = (object)model.OrgStructureID;
            strorgIDs[1] = model.ProfileName;
            strorgIDs[2] = model.CodeEmp;
            string status = string.Empty;
            var isDataTable = false;
            object obj = new DataTable();
            List<Hre_ProfileEntity> lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strorgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportDetailShift(From, To, lstProfileIDs, model.UserExport, UserLogin);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom.Value };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo.Value };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailShiftModel",
                    HeaderInfo=listHeaderInfo,
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                //[Hien.Nguyen] Do Path trả về lỗi nhưng chưa tìm đc nguyên nhân nên dùng cách replace path
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }

            return Json(result.ToDataSourceResult(request));
        }


        public ActionResult GetReportProfileAllowLimitOvertimeValidate(Att_ReportProfileAllowLimitOvertimeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportProfileAllowLimitOvertimeModel>(model, "Att_ReportProfileAllowLimitOvertime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        [HttpPost]
        public ActionResult GetReportProfileAllowLimitOvertime([DataSourceRequest] DataSourceRequest request, Att_ReportProfileAllowLimitOvertimeModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            //List<Guid?> OrgIds = new List<Guid?>();
            string OrgIds = string.Empty;
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }
            if (model.OrgStructureID != null && model.OrgStructureID.Count() > 0)
            {
                OrgIds = model.OrgStructureID;
            }
            List<object> strorgIDs = new List<object>();
            strorgIDs.AddRange(new object[3]);
            strorgIDs[0] = (object)model.OrgStructureID;
            strorgIDs[1] = model.ProfileName;
            strorgIDs[2] = model.CodeEmp;
            string status = string.Empty;
            var isDataTable = false;
            object obj = new DataTable();
            List<Hre_ProfileEntity> lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strorgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportProfileAllowLimitOvertime(From, To, lstProfileIDs, model.OrgStructureID, model.UserExport, model.IsCreateTemplate, UserLogin);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportProfileAllowLimitOvertimeModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }

            return Json(result.ToDataSourceResult(request));
        }


        public ActionResult GetReportDiligenceYearValidate(Att_ReportDiligenceYearModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDiligenceYearModel>(model, "Att_ReportDiligenceYear", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportDiligenceYear([DataSourceRequest] DataSourceRequest request, Att_ReportDiligenceYearModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }

            string status = string.Empty;
            var code = model.CodeEmp;
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportDiligenceYear(From, To, model.OrgStructureID, lstProfile, model.UserExport);
            // var rs = result.Translate<Att_ReportDiligenceYearModel>();

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDiligenceYearModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validation màn hình báo cáo chi tiết giờ vào/ giờ ra
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportSumaryInOutValidate(Att_ReportSumaryInOutModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSumaryInOutModel>(Model, "Att_ReportSumaryInOut", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Son.Vo] - 2014/06/02
        /// Lấy dữ liệu load lên lưới bằng store - GetReportSumaryInOut
        /// </summary>
        [HttpPost]
        public ActionResult GetReportSumaryInOut([DataSourceRequest] DataSourceRequest request, Att_ReportSumaryInOutModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }

            string status = string.Empty;
            var code = model.CodeEmp;
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var isDataTable = false;
            object obj = new DataTable();
            var result = service.GetReportSumaryInOut(From, To, lstProfile, _ShiftIDs, code, model.isIncludeQuitEmp, model.UserExport, UserLogin);
            // var rs = result.Translate<Att_ReportSumaryInOutModel>();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSumaryInOutModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(Model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo thống kê đi muộn về sớm
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportSumaryLateInOutValidate(Att_ReportSumaryLateInOutModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSumaryLateInOutModel>(Model, "Att_ReportSumaryLateInOut", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Son.Vo] - 2014/06/02
        /// Lấy dữ liệu load lên lưới bằng store - GetReportSumaryLateInOut
        /// </summary>
        [HttpPost]
        public ActionResult GetReportSumaryLateInOut([DataSourceRequest] DataSourceRequest request, Att_ReportSumaryLateInOutModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }

            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var isDataTable = false;
            object obj = new Att_ReportSumaryLateInOutEntity();
            var result = service.GetReportSumaryLateInOut(From, To, lstProfile, _ShiftIDs, model.isIncludeQuitEmp, model.NoDisplay0Data, model.UserExport);
            //  var rs = result.Translate<Att_ReportSumaryLateInOutModel>();

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSumaryLateInOutModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo chi tiết nghỉ hàng ngày
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportLeavedayListValidate(Att_ReportLeavedayModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportLeavedayModel>(Model, "Att_ReportLeaveday", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);
        }
        /// <summary>
        /// [Kiet.Chung] - 2014/11/17
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        public ActionResult GetReportRptInOutAdjustmentValidate(Att_RptInOutAdjustmentSearchModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_RptInOutAdjustmentSearchModel>(Model, "Att_RptInOutAdjustment", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);
        }
        public ActionResult GetReportRptInOutAdjustment([DataSourceRequest] DataSourceRequest request, Att_RptInOutAdjustmentSearchModel model)
        {
            var AttReportService = new Att_ReportServices();
            List<Att_ReportInOutAdjustmentEntity> lstAtt_WorkdayEntity = AttReportService.GetReportInOutAdjustment(model.DateFrom, model.DateTo, model.TAMScanReasonMissIDs, model.OrgStructureIDs, model.CodeEmp, model.PayrollGroupIDs, model.UserExport);
            if (lstAtt_WorkdayEntity != null)
            {

                var isDataTable = false;
                object obj = new Att_ReportInOutAdjustmentEntity();
                if (model.IsCreateTemplateForDynamicGrid)
                {
                    obj = lstAtt_WorkdayEntity;
                    isDataTable = true;
                }
                if (model != null && model.IsCreateTemplate)
                {
                    var path = Common.GetPath("Templates");
                    ExportService exportService = new ExportService();
                    ConfigExport cfgExport = new ConfigExport()
                    {
                        Object = obj,
                        FileName = "Att_RptInOutAdjustmentSearchModel",
                        OutPutPath = path,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = isDataTable

                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }

                if (model.ExportId != Guid.Empty)
                {
                    var fullPath = ExportService.Export(model.ExportId, lstAtt_WorkdayEntity, null, model.ExportType);
                    //   var fullPath = ExportService.Export(model.ExportId, lstResult);
                    return Json(fullPath);
                }
                List<Att_ReportInOutAdjustmentModel> lstResult = lstAtt_WorkdayEntity.Translate<Att_ReportInOutAdjustmentModel>();
                return Json(lstResult.ToDataSourceResult(request));
            }
            return null;
        }

        /// <summary>
        /// [Tam.Le] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportLeavedayList([DataSourceRequest] DataSourceRequest request, Att_ReportLeavedayModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.FromDate != null)
            {
                From = model.FromDate.Value;
            }
            if (model.ToDate != null)
            {
                To = model.ToDate.Value;
            }
            //if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureID;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            //List<Guid> lstProfileIDs = hrService.GetProfileIdsByOrg(strOrgIDs);
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _LeaveDayTypeIDs = null;
            if (model.LeaveDayTypeIDs != null)
                _LeaveDayTypeIDs = model.LeaveDayTypeIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var isDataTable = false;
            object obj = new DataTable();

            List<Hre_ProfileEntity> lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            var result = service.GetReportLeaveday(From, To, model.OrgStructureID, lstProfileIDs, _ShiftIDs, _LeaveDayTypeIDs, model.isIncludeQuitEmp,
                    model.isIncludeReasonMissFromWorkday, model.UserExport, model.CodeEmp, UserLogin);
            //if (Model.ExportId != Guid.Empty)
            //{
            //    var fullPath = ExportService.Export(Model.ExportId, result);
            //    return Json(fullPath);
            //}
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportLeavedayModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                //if (result.Rows.Count > 0)
                //{
                //    var count = result.Rows.Count;
                //    result.Rows[count - 1].Delete();
                //}
                //string ValueFields = string.Empty;
                //if (result != null)
                //{
                //    foreach (DataColumn column in result.Columns)
                //    {
                //        ValueFields = ValueFields + "," + column.ColumnName.ToString();
                //    }
                //}

                //string[] valueField = null;
                //if (ValueFields != null)
                //{
                //    var colName = ValueFields;
                //    valueField = colName.Split(',');
                //}


                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Tam.Le] - 2014/07/14
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportShiftAdjustmentList([DataSourceRequest] DataSourceRequest request, Att_ReportShiftAdjustmentModel Model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.FromDate != null)
            {
                From = Model.FromDate.Value;
            }
            if (Model.ToDate != null)
            {
                To = Model.ToDate.Value;
            }
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            var result = service.GetReportShiftAdjustmentList(From, To, OrgIds).ToList().Translate<Att_ReportShiftAdjustmentModel>();
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate Màn hình danh sách đăng ký tăng ca trùng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetExceptionOvertimeListValidate(Att_OvertimeModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_OvertimeModel>(Model, "Att_ExceptionOvertime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// Lấy dữ liệu Danh Sách Đăng Ký Tăng Ca Trùng load lên lưới 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExceptionOvertimeList([DataSourceRequest] DataSourceRequest request, Att_OvertimeModel Model)
        {
             var ActionService = new ActionService(UserLogin);
            var service = new Att_AttendanceServices();
            var hrService = new ActionService(UserLogin);

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateStart != null)
            {

                From = Model.DateStart;
            }
            if (Model.DateEnd != null)
            {
                To = Model.DateEnd.Date.AddDays(1).AddMilliseconds(-1);
            }

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(Model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            #endregion

            var result = service.GetExceptionOvertimeList(From, To, lstProfileIDs,UserLogin).ToList().Translate<Att_OvertimeModel>();
            if (Model.IsExport == true)
            {
                var fullPath = ExportService.Export(result, Model.ValueFields.Split(','));
                return Json(fullPath);
            }
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                TotalCount = result.Count > 0 ? result.Count : 0
            };
            return Json(result.ToDataSourceResult(request));
            //request.Page = 1;
            //var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count > 0 ? result.Count : 0;
            //return Json(dataSourceResult);
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo tổng hợp ngày nghỉ hàng tháng
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportLeaveMonthListValidate(Att_ReportLeaveMonthModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportLeaveMonthModel>(Model, "Att_ReportLeaveMonth", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }


        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo tổng hợp ngày nghỉ hàng tháng
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailedMonthlyStayListValidate(Att_ReportDetailedMonthlyStayModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailedMonthlyStayModel>(Model, "Att_ReportDetailedMonthlyStay", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        /// <summary>
        /// [Tam.Le] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportLeaveMonthList([DataSourceRequest] DataSourceRequest request, Att_ReportLeaveMonthModel model)
        {
            var service = new Att_ReportServices();

            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.FromDate != null)
            {
                From = model.FromDate.Value;
            }
            if (model.ToDate != null)
            {
                To = model.ToDate.Value;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.FromDate.Value };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.ToDate.Value };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            Guid[] _LeaveDayTypeIDs = null;
            if (model.LeaveDayTypeIDs != null)
                _LeaveDayTypeIDs = model.LeaveDayTypeIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var isDataTable = false;
            object obj = new DataTable();
            //List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            List<Hre_ProfileEntity> lstProfiles = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            var result = service.GetReportLeaveMonth(From, To, lstProfiles, model.OrgStructureID, model.isIncludeQuitEmp, _LeaveDayTypeIDs, model.CodeEmp, model.NoDisplay0Data, model.UserExport, model.IsCreateTemplate, UserLogin);
            //         var rs = result.Translate<Att_ReportLeaveMonthModel>();

            //neu luoi dong
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportLeaveMonthModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }




            if (model.ExportId != Guid.Empty)
            {
                // result.Rows[0].Delete();
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();
                //model.ValueFields = string.Empty;
                //if (result != null)
                //{
                //    foreach (DataColumn column in result.Columns)
                //    {
                //        model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //    }
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{

                //    valueField = model.ValueFields.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue };

        }



        [HttpPost]
        public ActionResult GetReportReportDetailedMonthlyStayList([DataSourceRequest] DataSourceRequest request, Att_ReportDetailedMonthlyStayModel model)
        {
            var service = new Att_ReportServices();

            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.FromDate != null)
            {
                From = model.FromDate.Value;
            }
            if (model.ToDate != null)
            {
                To = model.ToDate.Value;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.FromDate.Value };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.ToDate.Value };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            Guid[] _LeaveDayTypeIDs = null;
            if (model.LeaveDayTypeIDs != null)
                _LeaveDayTypeIDs = model.LeaveDayTypeIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var isDataTable = false;
            object obj = new DataTable();
            //List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            List<Hre_ProfileEntity> lstProfiles = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            var result = service.GetReportDetailedMonthlyStay(From, To, lstProfiles, model.OrgStructureID, model.isIncludeQuitEmp, _LeaveDayTypeIDs, model.CodeEmp, model.NoDisplay0Data, model.UserExport, model.IsCreateTemplate, UserLogin);
            //         var rs = result.Translate<Att_ReportLeaveMonthModel>();

            //neu luoi dong
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailedMonthlyStayModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }




            if (model.ExportId != Guid.Empty)
            {
                // result.Rows[0].Delete();
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();
                //model.ValueFields = string.Empty;
                //if (result != null)
                //{
                //    foreach (DataColumn column in result.Columns)
                //    {
                //        model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //    }
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{

                //    valueField = model.ValueFields.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue };

        }
        
        
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo tổng hợp ngày nghỉ hàng năm
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportLeaveYearListValidation(Att_ReportLeaveYearModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportLeaveYearModel>(Model, "Att_ReportLeaveYear", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Tam.Le] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportLeaveYearList([DataSourceRequest] DataSourceRequest request, Att_ReportLeaveYearModel model)
        {
            var service = new Att_ReportServices();

            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            List<Guid?> OrgIds = new List<Guid?>();
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.GetReportLeaveYear(model.Year, model.LeaveDayTypeIDs, lstProfileIDs, model.UserExport, model.CodeEmp);

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportLeaveYearModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo tổng hợp nghỉ ốm phép
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportSickLeaveListValidate(Att_ReportSickLeaveModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSickLeaveModel>(Model, "Att_ReportSickLeave", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Tam.Le] - 2014/05/31
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportSickLeaveList([DataSourceRequest] DataSourceRequest request, Att_ReportSickLeaveModel Model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.FromDate != null)
            {
                From = Model.FromDate.Value;
            }
            if (Model.ToDate != null)
            {
                To = Model.ToDate.Value;
            }

            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)Model.OrgStructureID;
            List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.GetReportSickLeave(From, lstProfileIDs).ToList().Translate<Att_ReportSickLeaveModel>();
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo chi tiết ngày nghỉ ốm
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailLeaveSickListValidate(Att_ReportDetailLeaveSickModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailLeaveSickModel>(Model, "Att_ReportDetailLeaveSick", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        /// <summary>
        /// [Tam.Le] - 2014/05/06
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportDetailLeaveSickList([DataSourceRequest] DataSourceRequest request, Att_ReportDetailLeaveSickModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.FromDate != null)
            {
                From = model.FromDate.Value;
            }
            if (model.ToDate != null)
            {
                To = model.ToDate.Value.AddDays(1).AddMilliseconds(-1);
            }
            //if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureID;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;


            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            //if (Model.ShiftID != null)
            //{
            //    Model.ShiftIDs.Add(Model.ShiftID);
            //}
            //if (Model.LeaveDayTypeID != null)
            //{
            //    Model.LeaveDayTypeIDs.Add(Model.LeaveDayTypeID);
            //}
            //bool? _isIncludeQuitEmp = Model.isIncludeQuitEmp;

            var isDataTable = false;
            object obj = new Att_ReportDetailLeaveSickEntity();

            List<Hre_ProfileEntity> lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportDetailLeaveSick(From, To, model.OrgStructureID, lstProfileIDs, model.ShiftIDs, model.LeaveDayTypeIDs, model.isIncludeQuitEmp, model.UserExport);

            if (model.IsCreateTemplateForDynamicGrid)
            {


                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailLeaveSickModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };

                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                //var fullPath = ExportService.Export(model.ExportId, result, model.ExportType);
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van]
        /// Báo cáo cảnh báo tăng ca
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportWarringOvertimeListValidate([DataSourceRequest] DataSourceRequest request, Att_ReportWarringOvertimeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportWarringOvertimeModel>(model, "Att_ReportWarringOvertimeModel", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);
        }
        /// <summary>
        /// [Hieu.Van]
        /// Báo cáo cảnh báo tăng ca
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportWarringOvertimeList([DataSourceRequest] DataSourceRequest request, Att_ReportWarringOvertimeModel model)
        {
            string status = string.Empty;
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateStart != null)
            {
                From = model.DateStart.Value;
            }
            if (model.DateEnd != null)
            {
                To = model.DateEnd.Value.AddDays(1).AddMilliseconds(-1);
            }

            if (model.OnWeek)
            {
                model.Type = "OnWeek";
            }
            else if (model.OnMonth)
            {
                model.Type = "OnMonth";
            }
            else if (model.OnYear)
            {
                model.Type = "OnYear";
            }

            var result = service.GetReportWarringOvertimeList(From, To, model.OrgStructureIDs, model.Type, model.UserExport, UserLogin);

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportWarringOvertimeModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                //Xuat theo cot dong
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName;
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van]
        /// BC Dữ Liệu Không Hợp Lệ
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportExceptionDataAdvList([DataSourceRequest] DataSourceRequest request, Att_ReportExceptionDataAdvModel model)
        {
            Att_ReportExceptionDataAdv_ConditionEntity condition = new Att_ReportExceptionDataAdv_ConditionEntity();
            condition.DateStart = model.DateStart;
            condition.DateEnd = model.DateEnd;
            condition.PayrollGroup = model.PayrollGroup;
            condition.StatusEmployee = model.StatusEmployee;
            condition.OrgStructureID = model.OrgStructureID;
            condition.ExcludeManagerStaff = model.ExcludeManagerStaff;
            condition.ShowListLeaveDay = model.ShowListLeaveDay;
            condition.NoShift = model.NoShift;
            condition.LessHours = model.LessHours;
            condition.DifferenceMoreRoster = model.DifferenceMoreRoster;
            condition.OTDuplicate = model.OTDuplicate;
            condition.NoScan = model.NoScan;
            condition.MissInOut = model.MissInOut;
            condition.More = model.More;
            condition.Less = model.Less;


            string status = string.Empty;
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();


            var result = service.GetReportExceptionDataAdvList(condition, model.UserExport, UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportExceptionDataAdvModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                //Xuat theo cot dong
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName;
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                // var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputeAttendance([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Validate
            string message = string.Empty;
            if (Model.CutOffDurationID == null || Model.CutOffDurationID == Guid.Empty)
            {
                var ls = new object[] { "error", "[Kỳ Công] Không Được Để Trống!" };
                return Json(ls);
            }
            #endregion

            var service = new Att_AttendanceServices();
             var ActionService = new ActionService(UserLogin);
            var status = string.Empty;

            List<Guid> listOrgStructureID = new List<Guid>();
            List<Guid> listPayrollGroupID = new List<Guid>();
            List<Guid> listWorkplaceID = new List<Guid>();

            if (!string.IsNullOrWhiteSpace(Model.OrgStructureID))
            {
                listOrgStructureID = Model.OrgStructureID.StringToList();
            }
            if (!string.IsNullOrWhiteSpace(Model.WorkPlace))
            {
                listWorkplaceID = Model.WorkPlace.StringToList();
            }
            if (!string.IsNullOrWhiteSpace(Model.SalaryJobGroup))
            {
                listPayrollGroupID = Model.SalaryJobGroup.StringToList();
            }

            string cutoff = Common.DotNetToOracle(Model.CutOffDurationID.ToString());
            DateTime _MonthYear = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;

            var objAtt_CutOffDurationEntity = ActionService.GetData<Att_CutOffDurationEntity>(cutoff,
                ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status).FirstOrDefault();

            if (objAtt_CutOffDurationEntity != null)
            {
                _MonthYear = objAtt_CutOffDurationEntity.MonthYear;
            }

            Model.ID = service.CreateComputingTask(Model.UserCreateID, _MonthYear);

            Task task = Task.Run(() => service.ComputeAttendance(Model.ID, Model.UserCreateID, _MonthYear,
                Model.CutOffDurationID, listOrgStructureID, listPayrollGroupID, listWorkplaceID, Model.OnlyQuitEmp));

            return Json(Model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/05/23
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputeAttendanceForProfile([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel Model)
        {
            var service = new Att_AttendanceServices();
             var ActionService = new ActionService(UserLogin);
            var status = string.Empty;

            string cutoff = Common.DotNetToOracle(Model.CutOffDurationID.ToString());
            DateTime _MonthYear = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;

            var objAtt_CutOffDurationEntity = ActionService.GetData<Att_CutOffDurationEntity>(cutoff,
                ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status).FirstOrDefault();

            if (objAtt_CutOffDurationEntity != null)
            {
                _MonthYear = objAtt_CutOffDurationEntity.MonthYear;
            }

            var dataErrorCode = service.ComputeAttendance(Guid.Empty, Model.LoginUserID, _MonthYear,
                objAtt_CutOffDurationEntity.ID, Model.ProfileID.TryGetValue<Guid>());

            return Json(Model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// [Son.Vo] - 2014/06/05
        /// Lấy dữ liệu load lên lưới bằng store - GetReportProfileByShift
        /// </summary>
        [HttpPost]
        public ActionResult GetReportProfileByShift1([DataSourceRequest] DataSourceRequest request, DateTime dateFrom, DateTime dateTo, string orgIds)
        {
            Att_ReportProfileByShiftModel model = new Att_ReportProfileByShiftModel();
            model.DateFrom = dateFrom;
            model.DateTo = dateTo;
            model.OrgStructureID = orgIds;
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            //List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            string OrgIds = string.Empty;
            if (model.OrgStructureID != null && model.OrgStructureID.Count() > 0)
            {
                OrgIds = model.OrgStructureID;
            }
            //string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)OrgIds;
            var lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            List<Guid> lstProfileIDs = lstProfiles.Select(s => s.ID).ToList();
            if (!string.IsNullOrEmpty(model.PayrollGroupIDs))
            {
                var arrPayrollGroupIDs = Common.StringToList(model.PayrollGroupIDs);
                if (arrPayrollGroupIDs.Any())
                {
                    //lay danh sach profile theo các nhóm lương truyền vào
                    lstProfileIDs = lstProfiles.Where(p => arrPayrollGroupIDs.Contains(p.PayrollGroupID ?? Guid.Empty)).Select(p => p.ID).ToList();
                    //lstProfileIDs = loc lai theo nhom luong    
                }
            }

            List<object> payroll = new List<object>();
            payroll.Add(null);
            payroll.Add(null);
            payroll.Add(1);
            payroll.Add(500);
            //List<Guid> lstPayrollGroup = GetData<>
            var result = service.GetReportProfileByShift(From, To, lstProfileIDs, false, model.UserExport);
            //var rs = result.Translate<Att_ReportProfileByShiftModel>();
            if (model.ExportID != Guid.Empty)
            {


                var fullPath = ExportService.Export(model.ExportID, result);
                //[Hien.Nguyen] Do Path trả về lỗi nhưng chưa tìm đc nguyên nhân nên dùng cách replace path
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            string dataReturn = result.ConvertDataTabletoString();
            return Json(dataReturn.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate BC NV làm việc theo ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetReportProfileByShiftValidate(Att_ReportProfileByShiftModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportProfileByShiftModel>(model, "Att_ReportProfileByShift", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        public ActionResult GetReportProfileByShift([DataSourceRequest] DataSourceRequest request, Att_ReportProfileByShiftModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            //List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            #region Tạo template BC
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            DataTable dtResult = service.GetReportProfileByShift(From, To, null, true, model.UserExport);
            object obj = new DataTable();
            var isDataTable = false;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = dtResult.Columns.Count;
                dtResult.Columns.RemoveAt(col - 1);
                obj = dtResult;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportProfileByShiftModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion
            string OrgIds = string.Empty;
            if (model.OrgStructureID != null && model.OrgStructureID.Count() > 0)
            {
                OrgIds = model.OrgStructureID;
            }
            //string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)OrgIds;
            var lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            List<Guid> lstProfileIDs = lstProfiles.Select(s => s.ID).ToList();
            if (!string.IsNullOrEmpty(model.PayrollGroupIDs))
            {
                var arrPayrollGroupIDs = Common.StringToList(model.PayrollGroupIDs);
                if (arrPayrollGroupIDs.Any())
                {
                    //lay danh sach profile theo các nhóm lương truyền vào
                    lstProfileIDs = lstProfiles.Where(p => arrPayrollGroupIDs.Contains(p.PayrollGroupID ?? Guid.Empty)).Select(p => p.ID).ToList();
                    //lstProfileIDs = loc lai theo nhom luong    
                }
            }
            var result = service.GetReportProfileByShift(From, To, lstProfileIDs, false, model.UserExport);

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportID, valuefields, result, model.ExportType);
                return Json(fullPath);
            }
            //string dataReturn = result.ConvertDataTabletoString();
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình BC thống kê ngày nghỉ ốm hàng năm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetReportSummaryLeaveYearSickValidate(Att_ReportSummaryLeaveYearSickModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSummaryLeaveYearSickModel>(model, "Att_ReportSummaryLeaveYearSick", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Tam.Le] - 2014/06/05
        /// Lấy dữ liệu load lên lưới bằng store - GetReportSummaryLeaveYearSick
        /// </summary>
        [HttpPost]
        public ActionResult GetReportSummaryLeaveYearSick([DataSourceRequest] DataSourceRequest request, Att_ReportSummaryLeaveYearSickModel model)
        {
            string status = string.Empty;
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            Guid[] _LeaveDayTypeIDs = null;
            if (model.LeaveDayTypeIDs != null)
                _LeaveDayTypeIDs = model.LeaveDayTypeIDs.Split(',').Select(s => Guid.Parse(s)).ToArray(); ;


            var lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            var result = service
                .GetReportSummaryLeaveYearSick(model.Year, lstProfileIDs, _LeaveDayTypeIDs, model.NoDisplay0Data, model.UserExport);
            var rs = result;
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);
            rs.Columns.RemoveAt(3);

            var isDataTable = false;
            object obj = new DataTable();

            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSummaryLeaveYearSickModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();
                //model.ValueFields = string.Empty;
                //if (rs != null)
                //{
                //    foreach (DataColumn column in rs.Columns)
                //    {
                //        model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //    }
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = rs.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
        }
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo đi muộn về sớm
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportSummaryLateInOutValidate(Att_ReportSummaryLateInOutModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSummaryLateInOutModel>(Model, "Att_ReportSummaryLateInOut", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Tam.Le] - 2014/06/05
        /// Lấy dữ liệu load lên lưới bằng store - GetReportSummaryLateInOut
        /// </summary>
        [HttpPost]
        public ActionResult GetReportSummaryLateInOut([DataSourceRequest] DataSourceRequest request, Att_ReportSummaryLateInOutModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }
            //if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureID;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            Guid[] _ShiftIDs = null;
            if (model.ShiftIDs != null)
                _ShiftIDs = model.ShiftIDs.Split(',').Select(s => Guid.Parse(s)).ToArray(); ;
            var isDataTable = false;
            object obj = new Att_ReportSummaryLateInOutEntity();

            // List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            List<Hre_ProfileEntity> lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportSummaryLateInOut(From, To, lstProfile, _ShiftIDs, model.isIncludeQuitEmp, model.CodeEmp, model.UserExport);
            //   var rs = result.Translate<Att_ReportSummaryLateInOutModel>();

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSummaryLateInOutModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                // var fullPath = ExportService.Export(Model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình danh sách ngày nghỉ và trễ sớm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult AnalysysLeaveAndLateEarlyValidate([DataSourceRequest] DataSourceRequest request, Att_AnalysysLeaveAndLateEarlyModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_AnalysysLeaveAndLateEarlyModel>(Model, "Att_AnalysysLeaveAndLateEarly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        /// <summary>
        /// [Son.Vo] - 2014/07/07
        /// Lấy dữ liệu load lên lưới bằng store - Phân Tích Ngày Nghỉ
        /// </summary>
        [HttpPost]
        public ActionResult AnalysysLeaveAndLateEarly([DataSourceRequest] DataSourceRequest request, Att_AnalysysLeaveAndLateEarlyModel Model)
        {
            var service = new Att_LeavedayServices();
            var hrService = new ActionService(UserLogin);

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            //if (Model.OrgStructureIds != null && Model.OrgStructureIds.Count() > 0)
            //{
            //    OrgIds = Model.OrgStructureIds;
            //}
            //string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(Model.OrgStructureIds);
            lstObj.Add(null);
            lstObj.Add(null);

            var lstProfile = hrService.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            List<Guid> lstProfileIDs = new List<Guid>();
            if (lstProfile != null && lstProfile.Count > 0)
            {
                lstProfileIDs = lstProfile.Select(s => s.ID).ToList();
            }
            //List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            var result = service.AnalysisLeaveAndLate(From, To, lstProfileIDs).ToList().Translate<Att_AnalysysLeaveAndLateEarlyModel>();
            if (Model.IsExport == true)
            {
                var fullPath = ExportService.Export(result, Model.ValueFields.Split(','));
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình xử lý dữ liệu chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetTAMDataValidate(Att_TAMModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_TAMModel>(model, "Att_TAM", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        [HttpPost]
        public ActionResult GetTAMData([DataSourceRequest] DataSourceRequest request, Att_TAMModel model)
        {
            var service = new Att_TAMServices();
            List<Guid> listProfileID = new List<Guid>();
            List<Guid> listOrgStructureID = new List<Guid>();

            model.AsynTaskID = service.CreateComputingTask(model.LoginUserID,
                model.DateFrom, model.DateTo);

            if (!string.IsNullOrWhiteSpace(model.ProfileID))
            {
                listProfileID = model.ProfileID.StringToList();

                Task task = Task.Run(() => service.SyncTAMLog(model.LoginUserID,
                    model.AsynTaskID, false, model.DateFrom, model.DateTo, listProfileID));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.OrgStructureID))
                {
                    listOrgStructureID = model.OrgStructureID.StringToList();
                }

                Task task = Task.Run(() => service.SyncTAMLog(model.LoginUserID, model.AsynTaskID,
                    false, model.DateFrom, model.DateTo, listOrgStructureID, null, null));
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAllowLimitOvertimeList([DataSourceRequest] DataSourceRequest request, Att_AllowLimitOvertimeSearchModel model)
        {
            return ExportAllAndReturn<Att_AllowLimitOvertimeEntity, Att_AllowLimitOvertimeModel, Att_AllowLimitOvertimeSearchModel>(request, model, ConstantSql.hrm_att_sp_get_AllowLimitOvertime);
        }

        public ActionResult ExportAllowLimitOvertimeSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_AllowLimitOvertimeEntity, Att_AllowLimitOvertimeModel>(String.Join(",", selectedIds), valueFields, ConstantSql.hrm_att_sp_get_AllowLimitOvertimeByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAnnualLeaveList([DataSourceRequest] DataSourceRequest request, Att_AnnualLeaveSearchModel model)
        {
            return ExportAllAndReturn<Att_AnnualLeaveEntity, Att_AnnualLeaveModel, Att_AnnualLeaveSearchModel>(request, model, ConstantSql.hrm_att_sp_get_AnnualLeave);
        }

        public ActionResult ExportAnnualLeaveSelected(List<Guid> selectedIds, string valueFields)
        {
            string strSelectedIDS = String.Join(",", selectedIds);
            return ExportSelectedAndReturn<Att_AnnualLeaveEntity, Att_AnnualLeaveModel>(strSelectedIDS, valueFields, ConstantSql.hrm_att_sp_get_AnnualLeaveByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAttendanceTableList([DataSourceRequest] DataSourceRequest request, Att_AttendanceTableModel model)
        {
            return ExportAllAndReturn<Att_AttendanceTableEntity, Att_AttendanceTableModel, Att_AttendanceTableModel>(request, model, ConstantSql.hrm_att_sp_get_AttendanceTable);
        }

        public ActionResult ExportAttendanceTableSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_AttendanceTableEntity, Att_AttendanceTableModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_AttendanceTableByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportCutOffDurationList([DataSourceRequest] DataSourceRequest request, Att_CutOffDurationSearchModel model)
        {
            return ExportAllAndReturn<Att_CutOffDurationEntity, Att_CutOffDurationModel, Att_CutOffDurationSearchModel>(request, model, ConstantSql.hrm_att_sp_get_CutOffDurations);
        }

        public ActionResult ExportCutOffDurationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_CutOffDurationEntity, Att_CutOffDurationModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_CutOffDurationByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportGradeList([DataSourceRequest] DataSourceRequest request, Att_GradeSearchModel model)
        {
            return ExportAllAndReturn<Att_GradeEntity, Att_GradeModel, Att_GradeSearchModel>(request, model, ConstantSql.hrm_att_sp_get_Att_Grade);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/07/03
        /// Lấy dữ liệu load lên Xuất Excel Grade
        /// </summary>
        public ActionResult ExportGradeSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_GradeEntity, Att_GradeModel>(String.Join(",", selectedIds), valueFields, ConstantSql.hrm_att_sp_get_GradeByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportOvertimeList([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            //return ExportAllAndReturn<Att_OvertimeModel, Att_OvertimeEntity, Att_OvertimeSearchOTModel>(request, model, ConstantSql.hrm_att_sp_get_Overtime);

            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var listModel = GetListData<Att_OvertimeEntity, Att_OvertimeEntity, Att_OvertimeSearchOTModel>(request, model, ConstantSql.hrm_att_sp_get_Overtime, ref status);
            if (status == NotificationType.Success.ToString())
            {
                var overtimeServices = new Att_OvertimeServices();
                List<Att_OvertimeEntity> lstOvertimeTmp = new List<Att_OvertimeEntity>();
                lstOvertimeTmp.AddRange(listModel);
                SetStatusOvertimeOnWorkday(lstOvertimeTmp);
                overtimeServices.FillterAllowOvertime(lstOvertimeTmp);
                foreach (var item in listModel)
                {
                    Att_OvertimeEntity overtimeTmp = lstOvertimeTmp.Where(m => m.ID == item.ID).FirstOrDefault();
                    item.udHourByMonth = overtimeTmp.udHourByMonth;
                    item.udHourByYear = overtimeTmp.udHourByYear;
                }
                model.ValueFields = model.ValueFields.Replace("Status,", "StatusTranslate,");
                model.ValueFields = model.ValueFields.Replace("MethodPayment,", "MethodPaymentTranslate,");

                status = ExportService.Export(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
        }

        /// <summary>
        /// [Kiet.Chung] - ExportWaitingData
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportWaitingData([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            //return ExportAllAndReturn<Att_OvertimeModel, Att_OvertimeEntity, Att_OvertimeSearchOTModel>(request, model, ConstantSql.hrm_att_sp_get_Overtime);

            model.SetPropertyValue("IsExport", true);
            string status = string.Empty;
            var listModel = GetListData<Att_OvertimeModel, Att_OvertimeEntity, Att_OvertimeSearchOTModel>(request, model, ConstantSql.hrm_att_sp_get_Overtime, ref status);

            if (status == NotificationType.Success.ToString())
            {
                if (listModel != null)
                {
                    listModel = listModel.Where(s => s.Status == EnumDropDown.OverTimeStatus.E_WAIT_APPROVED.ToString()).ToList();

                    DataTable tb = new DataTable();
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.CodeEmp);
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.ProfileName);
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.PositionName);
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.WorkDate);
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.ShiftName);
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.InTime, typeof(DateTime));
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.OutTime, typeof(DateTime));
                    tb.Columns.Add(Att_OvertimeModel.FieldNames.OvertimeTypeName);
                    foreach (Att_OvertimeModel item in listModel)
                    {
                        DataRow row = tb.NewRow();
                        row[Att_OvertimeModel.FieldNames.CodeEmp] = item.CodeEmp;
                        row[Att_OvertimeModel.FieldNames.ProfileName] = item.ProfileName;
                        row[Att_OvertimeModel.FieldNames.PositionName] = item.PositionName;
                        row[Att_OvertimeModel.FieldNames.WorkDate] = item.WorkDate;
                        row[Att_OvertimeModel.FieldNames.ShiftName] = item.ShiftName;
                        if (item.InTime != null)
                            row[Att_OvertimeModel.FieldNames.InTime] = item.InTime;
                        if (item.OutTime != null)
                            row[Att_OvertimeModel.FieldNames.OutTime] = item.OutTime;
                        row[Att_OvertimeModel.FieldNames.OvertimeTypeName] = item.OvertimeTypeName;
                        tb.Rows.Add(row);
                    }

                    if (model.ExportId != Guid.Empty)
                    {
                        var fullPath = ExportService.Export(model.ExportId, tb);
                        return Json(fullPath);
                    }
                }

                //ExportService.ExportNormal();

                //status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").Validate<string>().Split(','));
            }

            return Json(status);

        }

        [HttpPost]
        public ActionResult ExportOvertimeListPortal([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            return ExportAllAndReturnPortal<Att_OvertimeModel, Att_OvertimeEntity, Att_OvertimeSearchOTModel>(request, model, ConstantSql.hrm_att_sp_get_Overtime);
        }

        [HttpPost]
        public ActionResult ExportOvertimeListPortalByProfile([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            ActionService service = new ActionService(UserLogin);
            model.IsExport = true;
            string fullPath = string.Empty, status = string.Empty;
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = model.SysUserID;
            objpara[1] = 1;
            objpara[2] = int.MaxValue - 1;
            var listModel = service.GetData<Att_OvertimeEntity>(objpara, ConstantSql.hrm_att_sp_get_OvertimeByProfileId, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.ExportPortal(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
        }
        [HttpPost]
        public ActionResult ExportLeavedayListPortalByProfile([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            ActionService service = new ActionService(UserLogin);
            model.IsExport = true;
            string fullPath = string.Empty, status = string.Empty;
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = Common.DotNetToOracle(model.SysUserID.Value.ToString());
            objpara[1] = 1;
            objpara[2] = int.MaxValue - 1;

            var listModel = service.GetData<Att_LeaveDayEntity>(objpara, ConstantSql.hrm_att_sp_get_LeaveDayByProfileIds, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.ExportPortal(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/07/03
        /// Lấy dữ liệu load lên Xuất Excel Overtime
        /// </summary>
        public ActionResult ExportOvertimeSelected(List<Guid> selectedIds, string valueFields)
        {
            string status = string.Empty;
            var selectedIdsOrcl = Common.DotNetToOracle(String.Join(",", selectedIds));
            var listModel = GetData<Att_OvertimeModel, Att_OvertimeEntity>(selectedIdsOrcl, ConstantSql.hrm_att_sp_get_OvertimeByIds);

            var overtimeServices = new Att_OvertimeServices();
            List<Att_OvertimeEntity> lstOvertimeTmp = new List<Att_OvertimeEntity>();
            lstOvertimeTmp = listModel.Translate<Att_OvertimeEntity>();
            //lstOvertimeTmp.AddRange(listModel);
            SetStatusOvertimeOnWorkday(lstOvertimeTmp);
            overtimeServices.FillterAllowOvertime(lstOvertimeTmp);

            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Att_OvertimeModel.FieldNames.WorkDate, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Att_OvertimeModel.FieldNames.InTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_OvertimeModel.FieldNames.OutTime, ConstantFormat.HRM_Format_HourMin_AM_PM);

            valueFields = valueFields.Replace("Status,", "StatusTranslate,");
            valueFields = valueFields.Replace("MethodPayment,", "MethodPaymentTranslate,");
            status = ExportService.Export(lstOvertimeTmp, valueFields.Split(','), formatFields);
            return Json(status);
            // return ExportSelectedAndReturn<Att_OvertimeEntity, Att_OvertimeModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_OvertimeByIds);
        }
        [HttpPost]

        public ActionResult Export_gridcurrent([Bind(Prefix = "models")]List<Att_OvertimeModel> lstmodel, [Bind(Prefix = "values")]string valueFields)
        {

            string status = string.Empty;
            status = ExportService.Export(lstmodel, valueFields.Split(','));
            return Json(status);

        }


        public ActionResult ExportOvertimeSelectedPortal(List<Guid> selectedIds, string valueFields)
        {
            string status = string.Empty;
            var selectedId = Common.DotNetToOracle(String.Join(",", selectedIds));
            var listModel = GetData<Att_OvertimeModel, Att_OvertimeEntity>(selectedId, ConstantSql.hrm_att_sp_get_OvertimeByIds);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Att_OvertimeModel.FieldNames.WorkDate, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Att_OvertimeModel.FieldNames.InTime, ConstantFormat.HRM_Format_HourMin_AM_PM);
            formatFields.Add(Att_OvertimeModel.FieldNames.OutTime, ConstantFormat.HRM_Format_HourMin_AM_PM);

            status = ExportService.ExportPortal(listModel, valueFields.Split(','), formatFields);
            return Json(status);
            // return ExportSelectedAndReturn<Att_OvertimeEntity, Att_OvertimeModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_OvertimeByIds);
        }

        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportLeavedayList([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            model.ValueFields = model.ValueFields.Replace("Status,", "StatusTranslate,");

            return ExportAllAndReturn<Att_LeaveDayEntity, Att_LeaveDayModel, Att_LeavedaySearchModel>(request, model, ConstantSql.hrm_att_sp_get_Leaveday);

        }

        [HttpPost]
        public ActionResult ExportLeavedayListPortal([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            return ExportAllAndReturnPortal<Att_LeaveDayEntity, Att_LeaveDayModel, Att_LeavedaySearchModel>(request, model, ConstantSql.hrm_att_sp_get_Leaveday);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/07/03
        /// Lấy dữ liệu load lên Xuất Excel Leaveday
        /// </summary>
        public ActionResult ExportLeavedaySelected(List<Guid> selectedIds, string valueFields)
        {
            valueFields = valueFields.Replace("Status,", "StatusTranslate,");

            return ExportSelectedAndReturn<Att_LeaveDayEntity, Att_LeaveDayModel>(String.Join(",", selectedIds), valueFields, ConstantSql.hrm_att_sp_get_LeavedayByIds);
        }
        public ActionResult ExportLeavedaySelectedPortal(string selectedIds, string valueFields)
        {
            valueFields = valueFields.Replace("Status,", "StatusTranslate,");

            return ExportSelectedAndReturnPortal<Att_LeaveDayEntity, Att_LeaveDayModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_LeavedayByIds);
        }


        /// <summary>
        /// [Hieu.Van] - Xuất danh sách dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAnnualLeaveDetailList([DataSourceRequest] DataSourceRequest request, Att_AnnualLeaveDetailSearchModel model)
        {
            return ExportAllAndReturn<Att_AnnualLeaveDetailEntity, Att_AnnualLeaveDetailModel, Att_AnnualLeaveDetailSearchModel>(request, model, ConstantSql.hrm_att_sp_get_AnnualLeaveDetail);
        }

        /// <summary>
        /// [Tung.Ly] - 2014/07/10
        /// Lay dữ liệu load lên xuất excel AnnualLeaveDetail
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportAnnualLeaveDetailSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_AnnualLeaveDetailEntity, Att_AnnualLeaveDetailModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_AnnualLeaveDetailByIds);
        }


        /// <summary>
        /// [Hieu.Van] - 2014/07/14
        /// Update status Leaveday Duyệt và Chờ Duyệt
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public bool ChangeStatusLeaveday(Att_ChangeStatusLeavedayModel model)
        {
            model.SelectedIds = Common.DotNetToOracle(model.SelectedIds);
            var service = new Att_LeavedayServices();
            var result = service.UpdateStatusLeaveday(model.SelectedIds, model.Type);
            return result;
        }

        /// <summary>
        /// [Hieu.Van] - 2014/07/11
        /// Button Copy sao Chép trên màn hình Att_LeaveDay
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        /// 
        public ActionResult CopyLeaveDay(List<Guid> selectedIds)
        {
            var selectedIdsOrcl = Common.DotNetToOracle(String.Join(",", selectedIds));
            var service = new ActionService(UserLogin);
            Att_LeaveDayEntity model;
            string result = null;
            bool rs = true;
            var status = string.Empty;

             var ActionService = new BaseService();
            var listEntity = service.GetData<Att_LeaveDayEntity>(selectedIdsOrcl, ConstantSql.hrm_att_sp_get_LeavedayByIds, ref status);
            foreach (var item in listEntity)
            {

                model = item.CopyData<Att_LeaveDayEntity>();

                result = ActionService.Add<Att_LeaveDayEntity>(model);
                if (result != "Success")
                {
                    rs = false;
                }
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #region Kiểm tra kết nổi màn hình cấu hình tải dữ liệu + màn hình tải dữ liệu chấm công
        public bool CheckConnectTAM()
        {
            var service = new Att_TAMServices();
            string message = "";
            bool status = false;

            bool isconnect = service.IsConnected(AppConfig.HRM_SYS_TAMSCANLOG_1_, ref message);

            if (isconnect == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            return status;
        }

        public bool CheckConnectTAMConfig(string serverName, string userId, string password, string dbName)
        {
            var service = new Att_TAMServices();
            string status = string.Empty;
            bool result = false;

            bool isconnect = service.IsConnected(serverName,
                userId, password, dbName, ref status);

            if (isconnect == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region BC Tăng Ca Hàng Tháng
        /// <summary>
        /// Validate màn hình BC Tăng Ca Hàng Tháng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetReportMonthlyOvertimeListValidate(Att_ReportMonthlyOvertimeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportMonthlyOvertimeModel>(model, "Att_ReportMonthlyOvertime", ref message);
            if (!checkValidate)
            {
                var rs = new object[] { "error", message };
                return Json(rs);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportMonthlyOvertimeList([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyOvertimeModel model)
        {
            Att_ReportMonthlyOvertimeConditionEntity condition = new Att_ReportMonthlyOvertimeConditionEntity();
            condition.OvertimeDetail = model.OvertimeDetail;
            condition.RemoveNotHasWorkday = model.RemoveNotHasWorkday;
            condition.IncludeAllEmployee = model.IncludeAllEmployee;
            condition.RemoveCompensation100 = model.RemoveCompensation100;
            condition.RecordIns = model.RecordIns;
            condition.Cumulative = model.Cumulative;

            condition.OrgStructureID = model.OrgStructureID;
            condition.StatusEmployee = model.StatusEmployee;
            condition.OvertimeStatus = model.OvertimeStatus;
            condition.TypeHour = model.TypeHour;

            var service = new Att_ReportServices();
             var ActionService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            if (model.PayrollGroup != null)
            {
                condition.PayrollGroup = model.PayrollGroup.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            if (model.CumulativeType != null)
            {
                condition.CumulativeType = model.CumulativeType.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now.AddDays(1).AddMilliseconds(-1);
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                condition.DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                condition.DateTo = model.DateTo.Value;
            }

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom.Value };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo.Value };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            string status = string.Empty;
            List<object> strOrgIDs = new List<object>();
            //strOrgIDs.AddRange(new object[3]);
            //strOrgIDs[0] = (object)model.OrgStructureID;
            strOrgIDs.Add(model.OrgStructureID);
            strOrgIDs.Add(null);
            strOrgIDs.Add(null);

            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }

            var result = service.GetReportMonthlyOvertime(lstProfileIDs, condition, model.UserExport, model.IsCreateTemplate,UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportMonthlyOvertimeModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region BC cong hang thang
          public ActionResult GetReportMonthly([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyModel model)
        {
            string status = string.Empty;
            var rptservice = new Att_ReportServices();
            var service = new ActionService(UserLogin);
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[3]);
            objpara[0] = (object)model.OrgStructureID;
            List<Hre_ProfileIdEntity> lstProfileIDs = service.GetData<Hre_ProfileIdEntity>(objpara, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgN, ref status).ToList();
            var lstProfileID = lstProfileIDs.Select(s => s.ID).ToList();
            var result = rptservice.GetReportAttendanceByMonthV2(lstProfileID, model.Month, model.IsCreateTemplate);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                //var col = result.Columns.Count;
                //result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportMonthlyEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Thống Kê Ca Làm Việc
        public ActionResult GetReportSumaryShiftMonthList([DataSourceRequest] DataSourceRequest request, Att_ReportSumaryShiftMonthModel model)
        {
            string status = string.Empty;
            var service = new Att_ReportServices();
             var ActionService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now.AddDays(1).AddMilliseconds(-1);
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value.AddDays(1).AddMilliseconds(-1);
            }

            List<Guid?> OrgIds = new List<Guid?>();
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Hre_ProfileEntity> lstProfileIDs = ActionService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            var result = service.GetReportSumaryShiftMonthList(DateFrom, DateTo, model.OrgStructureID, model.ProfileIDs, model.ShiftID, model.NoDisplay0Data, model.isIncludeQuitEmp, UserLogin);

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSumaryShiftMonthModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Báo Cáo Tăng Ca
        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình BC chi tiết ngày làm thêm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetReportDetailOvertimeListValidate(Att_ReportDetailOvertimeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportDetailOvertimeModel>(model, "Att_ReportDetailOvertime", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        #region BC Chi Tiết Ngày Làm Thêm
        public ActionResult GetReportDetailOvertimeList([DataSourceRequest] DataSourceRequest request, Att_ReportDetailOvertimeModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }
            string profileName = string.Empty;
            string codeEmp = string.Empty;
            if (model.ProfileName != string.Empty)
            {
                profileName = model.ProfileName;
            }
            if (model.CodeEmp != string.Empty)
            {
                codeEmp = model.CodeEmp;
            }
            var result = service.GetReportDetailOvertime(model.UserExport, DateFrom, DateTo, profileName, codeEmp, model.OrgStructureID, model.OverTimeTypeID, model.NoDisplay0Data, model.IsIncludeQuitEmp);

            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportDetailOvertimeModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportId != Guid.Empty)
            {
                //Xuat theo cot dong
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName;
                //}
                //var count = result.Rows.Count;
                //result.Rows[count - 1].Delete();
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //  var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);

                //var fullPath = ExportService.Export(model.ExportId, result);
                //return Json(fullPath);
            }
            //else
            //{
            //    result.Columns.Remove("DateFrom");
            //    result.Columns.Remove("DateTo");
            //    result.Columns.Remove("DateExport");
            //}
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        /// <summary>
        /// [Bang.nguyen] 20/10/2014
        /// Validate màn hình báo cáo thống kê tăng ca
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GetReportSummaryOvertimeMonthListValidate(Att_ReportSummaryOvertimeMonthModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportSummaryOvertimeMonthModel>(Model, "Att_ReportSummaryOvertimeMonth", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        #region BC Thống Kê Tăng Ca [Tho.Bui]: Edit 2014.11.03
        public ActionResult GetReportSummaryOvertimeMonthList([DataSourceRequest] DataSourceRequest request, Att_ReportSummaryOvertimeMonthModel model)
        {
            //[Tho.Bui] Cat chuoi gan vao list<Guid>

            //List<Guid?> listOvertimeTypeID = null;
            //if (Model.OvertimeTypeID != null)
            //{
            //    var listStr = Model.OvertimeTypeID.Split(',');
            //    if (listStr[0] != "")
            //    {
            //        foreach (var item in listStr)
            //        {
            //            listOvertimeTypeID.Add(Guid.Parse(item));
            //        }
            //    }
            //}
            //List<Guid?> listShiftID = null;
            //if (Model.ShiftID != null)
            //{
            //    var ListStrShiftID = Model.ShiftID.Split(',');
            //    if (ListStrShiftID[0] != "")
            //    {
            //        foreach (var item1 in ListStrShiftID)
            //        {
            //            listShiftID.Add(Guid.Parse(item1));
            //        }
            //    }
            //}

            Guid[] _listOvertimeTypeID = null;
            if (model.OvertimeTypeID != null)
                _listOvertimeTypeID = model.OvertimeTypeID.Split(',').Select(s => Guid.Parse(s)).ToArray();

            Guid[] _listShiftID = null;
            if (model.ShiftID != null)
                _listShiftID = model.ShiftID.Split(',').Select(s => Guid.Parse(s)).ToArray();

            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }
            string status = string.Empty;
            string profilename = model.ProfileName;
            string codeemp = model.CodeEmp;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(model.OrgStructureID);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            var lstProfileIDs = hrService.GetData<Hre_ProfileEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportSummaryOvertimeMonth(DateFrom, DateTo, model.OrgStructureID, lstProfileIDs, _listOvertimeTypeID, _listShiftID, model.NoDisplay0Data, model.IsIncludeQuitEmp, model.UserExport, UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportSummaryOvertimeMonthModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                //var count = result.Rows.Count;
                //result.Rows[count - 1].Delete();
                //model.ValueFields = string.Empty;
                //foreach (DataColumn column in result.Columns)
                //{
                //    model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{
                //    var colName = model.ValueFields;
                //    valueField = colName.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                // var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            //return new JsonResult() { Data = result.ToDataSourceResult(request) ,MaxJsonLength=Int32.MaxValue};
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion


        ///// <summary>
        ///// [Bang.nguyen] 20/10/2014
        ///// Validate BC thống kê làm thêm năm
        ///// </summary>
        ///// <param name="Model"></param>
        ///// <returns></returns>
        //public ActionResult GetReportStatisticsOvertimeByYearValidate(Att_ReportStatisticsOvertimeByYearModel Model)
        //{
        //    #region Validate
        //    string message = string.Empty;
        //    var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportStatisticsOvertimeByYearModel>(Model, "Att_ReportStatisticsOvertimeByYear", ref message);
        //    if (!checkValidate)
        //    {
        //        var ls = new object[] { "error", message };
        //        return Json(ls);
        //        //return Json(message);
        //    }
        //    #endregion
        //    return Json(message);

        //}
        #region BC Thống Kê Làm Thêm Năm
        public ActionResult GetReportStatisticsOvertimeByYear([DataSourceRequest] DataSourceRequest request, Att_ReportStatisticsOvertimeByYearModel model)
        {
            var service = new Att_ReportServices();
            var hrService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            // DateTime Year = DateTime.Now;
            int Year = 0;
            //List<Guid?> OrgIds = new List<Guid?>();
            if (model.Year != null)
            {
                Year = model.Year;
            }

            string profileName = string.Empty;
            string codeEmp = string.Empty;
            if (model.ProfileName != string.Empty)
            {
                profileName = model.ProfileName;
            }
            if (model.CodeEmp != string.Empty)
            {
                codeEmp = model.CodeEmp;
            }
            List<Guid> lstProfileIDs = new List<Guid>();
            List<Object> Lstsearch = new List<object>();
            Lstsearch.Add(model.OrgStructureID);
            Lstsearch.Add(profileName);
            Lstsearch.Add(codeEmp);
            string status = string.Empty;

            List<Hre_ProfileEntity> lstProfiles = hrService.GetData<Hre_ProfileEntity>(Lstsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportStatisticsOvertimeByYear(Year, lstProfiles, model.OvertimeTypeID, model.NoDisplay0Data, model.IsIncludeQuitEmp, model.UserExport, UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportStatisticsOvertimeByYearModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #endregion

        #region General
        private List<SortAttribute> ExtractSortAttributes(DataSourceRequest request)
        {
            List<SortAttribute> list = new List<SortAttribute>();
            if (request.Sorts == null)
                return list;
            foreach (var sort in request.Sorts)
            {
                SortAttribute attribute = new SortAttribute()
                {
                    Member = sort.Member,
                    SortDirection = sort.SortDirection
                };
                list.Add(attribute);
            }
            return list;
        }

        private List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractFilterAttributes(DataSourceRequest request)
        {
            List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
            if (request.Filters == null)
                return list;
            foreach (Kendo.Mvc.FilterDescriptor filter in request.Filters)
            {
                HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                {
                    Member = filter.Member,
                    MemberType = filter.MemberType,
                };
                switch (filter.Operator)
                {
                    case Kendo.Mvc.FilterOperator.IsEqualTo:
                        attribute.Operator = FILTEROPERATOR.Equals;
                        break;
                    case Kendo.Mvc.FilterOperator.Contains:
                        attribute.Operator = FILTEROPERATOR.Contains;
                        break;
                    case Kendo.Mvc.FilterOperator.StartsWith:
                        attribute.Operator = FILTEROPERATOR.StartWith;
                        break;
                    case Kendo.Mvc.FilterOperator.EndsWith:
                        attribute.Operator = FILTEROPERATOR.EndWith;
                        break;
                }
                list.Add(attribute);
            }
            return list;
        }

        private List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractAdvanceFilterAttributes(object model)
        {
            List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
            if (model == null)
                return list;

            PropertyInfo[] propertyInfos = model.GetType().GetProperties();
            List<PropertyInfo> lstPropertyInfo = propertyInfos.ToList();

            foreach (PropertyInfo _profertyInfo in lstPropertyInfo)
            {
                HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                {
                    Member = _profertyInfo.Name,
                    MemberType = _profertyInfo.PropertyType,
                    Value2 = model.GetPropertyValue(_profertyInfo.Name)

                };
                if (_profertyInfo.PropertyType.Name == "List`1")
                {
                    attribute.MemberType = typeof(object);
                    var lstObj = (model.GetPropertyValue(_profertyInfo.Name) as IList);
                    object result = null;
                    if (lstObj != null)
                        result = string.Join(",", lstObj.OfType<object>().Select(x => x.ToString()).ToArray());
                    attribute.Value2 = result;
                }
                else if (_profertyInfo.PropertyType == typeof(DateTime))
                {
                    attribute.MemberType = typeof(DateTime);
                    if (attribute.Value2 != null && attribute.Value2.ToString() == DateTime.MinValue.ToString())
                    {
                        attribute.Value2 = null;
                    }
                }

                list.Add(attribute);
            }
            return list;
        }

        #endregion

        /// <summary>
        /// [Hien.Nguyen]
        /// </summary>
        /// <param name="selectIds"></param>
        /// <param name="LeaveDayCode"></param>
        /// <param name="UserApproved"></param>
        /// <param name="Comment"></param>
        /// 
        [HttpPost]
        public ActionResult SaveLeaveData([Bind(Prefix = "models")]List<Att_WorkdayModel> model, [Bind(Prefix = "LeaveDay")]string LeaveDayID, [Bind(Prefix = "Comment")] string Comment)
        {
            String message = "";
            try
            {
                Guid leavedayid = Guid.Parse(LeaveDayID.Split(',').FirstOrDefault().ToString());
                Guid? UserApproved = null;
                if (LeaveDayID.Split(',').LastOrDefault() != "null")
                    UserApproved = Guid.Parse(LeaveDayID.Split(',').LastOrDefault().ToString());
                var actionservices = new ActionService(UserLogin);
                var _attLeaveDay = new Att_LeavedayServices();
                var _attWorkDay = new Att_WorkDayServices();
                string status = string.Empty;
                List<object> lstobj = new List<object>();
                lstobj.AddRange(new object[4]);
                lstobj[2] = 1;
                lstobj[3] = 1000;

                var listLeaveDaytype = actionservices.GetData<Cat_LeaveDayTypeEntity>(lstobj, ConstantSql.hrm_cat_sp_get_LeaveDayType, ref status);
                lstobj = new List<object>();
                lstobj.AddRange(new object[3]);
                var listLeaveDay = actionservices.GetData<Att_LeaveDayEntity>(lstobj, ConstantSql.hrm_att_getdata_LeaveDay, ref status);

                for (int i = 0; i < model.Count; i++)
                {
                    if (leavedayid != null)
                    {
                        if (listLeaveDaytype.Single(m => m.ID == leavedayid) != null)
                        {
                            var leaveDay = listLeaveDay.Where(m => m.ProfileID == model[i].ProfileID && m.DateStart == model[i].WorkDate && m.DateEnd == model[i].WorkDate).FirstOrDefault();//lấy dữ liệu leaveday xem nhân viên này đã dk nghỉ hay chưa
                            if (leaveDay != null) //nếu đã dk nghỉ ngày đó rồi thì kiểm tra loại ngày nghỉ
                            {
                                var modelLeavedaytypeID = leavedayid;
                                if (leaveDay.LeaveDayTypeID == modelLeavedaytypeID)
                                {
                                    continue;
                                }
                                else
                                {
                                    leaveDay.LeaveDayTypeID = modelLeavedaytypeID;
                                    _attLeaveDay.Edit<Att_LeaveDayEntity>(leaveDay);
                                }
                            }
                            else
                            {
                                message = _attLeaveDay.SaveLeaveDataItem(model[i].ID, leavedayid, UserApproved, Comment, false);
                            }
                        }
                    }
                    else
                    {
                        message = _attLeaveDay.SaveLeaveDataItem(model[i].ID, leavedayid, UserApproved, Comment, false);
                    }

                }
                if (message == "")
                    message = "success";
                var ls = new object[] { message };
                return Json(ls);
            }
            catch
            {
                var ls = new object[] { message };
                return Json(ls);
                //return Json(new { success = false });
            }
        }

        /// <summary>
        /// [Hien.Nguyen] 2014/10/26 Tạo mới chỉnh sửa Nghỉ Phép Và Trễ Sớm page ComputeWorkDayAdjust
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateInlineAdjust([Bind(Prefix = "models")] List<Att_WorkdayModel> model)
        {
            String message = "";
            var _attLeaveDay = new ActionService(UserLogin);
            var _attWorkDay = new Att_WorkDayServices();
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstobj = new List<object>();
            lstobj.AddRange(new object[4]);
            lstobj[2] = 1;
            lstobj[3] = 10000;

            var listLeaveDaytype = _attLeaveDay.GetData<Cat_LeaveDayTypeEntity>(lstobj, ConstantSql.hrm_cat_sp_get_LeaveDayType, ref status);
            DateTime dtMin = DateTime.MinValue;
            DateTime dtMax = DateTime.MaxValue;
            dtMin = model.OrderBy(d => d.WorkDate).FirstOrDefault().WorkDate;
            dtMax = model.OrderBy(d => d.WorkDate).LastOrDefault().WorkDate;
            lstobj = new List<object>();
            lstobj.AddRange(new object[3]);
            //var listLeaveDay = _attLeaveDay.GetData<Att_LeaveDayEntity>(lstobj, ConstantSql.hrm_att_getdata_LeaveDay, ref status);
            List<Guid> lstWorkdayID = model.Select(d => d.ID).ToList();
            string strSelectedIds = "";
            foreach (var item in lstWorkdayID)
            {
                strSelectedIds += item.ToString() + ",";
            }
            if (strSelectedIds != "")
                strSelectedIds = strSelectedIds.Substring(0, strSelectedIds.Length - 1);
            strSelectedIds = Common.DotNetToOracle(strSelectedIds);
            var lstWorkdayOld = service.GetData<Att_WorkdayEntity>(strSelectedIds, ConstantSql.hrm_att_sp_get_WorkDayByIds, ref message);
            foreach (var item in model)
            {
                Att_ComputeWorkdayAdjustServices ComputeWorkdayAdjustServices = new Att_ComputeWorkdayAdjustServices();
                message = ComputeWorkdayAdjustServices.ValidateSaveWorkday(item.CopyData<Att_WorkdayEntity>(), lstWorkdayOld.Where(d => d.ID == item.ID).FirstOrDefault(), UserLogin);
                if (message != "")
                    return (Json(message.TranslateString()));
            }
            //for (int i = 0; i < model.Count; i++)
            //{

            //    if (model[i].udLeavedayCode1 != null)
            //    {
            //        Cat_LeaveDayTypeEntity objCat_leaveDayType = listLeaveDaytype.FirstOrDefault(m => m.CodeStatistic == model[i].udLeavedayCode1);
            //        if (objCat_leaveDayType != null)
            //        {
            //            var leaveDay = listLeaveDay.Where(m => m.ProfileID == model[i].ProfileID && m.DateStart <= model[i].WorkDate && m.DateEnd >= model[i].WorkDate).FirstOrDefault();//lấy dữ liệu leaveday xem nhân viên này đã dk nghỉ hay chưa
            //            if (leaveDay != null) //nếu đã dk nghỉ ngày đó rồi thì kiểm tra loại ngày nghỉ
            //            {
            //                var modelLeavedaytypeID = listLeaveDaytype.Single(m => m.CodeStatistic == model[i].udLeavedayCode1).ID;
            //                if (leaveDay.LeaveDayTypeID == modelLeavedaytypeID)
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    leaveDay.LeaveDayTypeID = modelLeavedaytypeID;
            //                    _attLeaveDay.Edit<Att_LeaveDayEntity>(leaveDay);
            //                }
            //            }
            //            else
            //            {
            //                message = _attLeaveDay.SaveLeaveDataItem(model[i].ID, objCat_leaveDayType.ID, null, "", false);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        _attWorkDay.Edit<Att_WorkdayEntity>(model[i].CopyData<Att_WorkdayEntity>());
            //        //_attLeaveDay.SaveLeaveDataItem(model[i].ID, listLeaveDaytype.Where(m => m.Code == model[i].udLeavedayCode1).FirstOrDefault().ID, null, "", false);
            //    }

            //}
            // Chua Thong Bao dươc khi edit tren luoi
            message = ConstantMessages.Succeed.TranslateString();
            return Json(new { message });
        }

        /// <summary>
        /// [Kiet.Chung] - GetDashBoardLeavesSummary
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DashboardLeavesSummary([DataSourceRequest] DataSourceRequest request, DashboardLeavesSummarySearchModel model)
        {
            return GetListDataAndReturn<DashboardLeavesSummaryModel, DashboardLeavesSummaryEntity, DashboardLeavesSummarySearchModel>(request, model, ConstantSql.hrm_att_sp_get_DashboardLeaves);
        }

        [HttpPost]
        public ActionResult HasConfigAnlInsLeaveDetail()
        {
            var hasConfig = false;
            string status = string.Empty;
            var service = new Att_AnnualInsuranceLeaveDetailServices();
            hasConfig = service.HasConfigAnnualLeaveAtTotalConfig();
            return Json(hasConfig);
        }

        [HttpPost]
        public ActionResult ChangeMethodOverTime_Manual([Bind]Att_OvertimeModel model)
        {
            Att_OvertimeServices domain = new Att_OvertimeServices();
            string status = string.Empty;
            // var model = new Att_OvertimeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_OvertimeEntity>(model.ID, ConstantSql.hrm_att_sp_get_OvertimeById, ref status);
            //var result = model.CopyData<Att_OvertimeEntity>();
            entity.TimeRegister = model.TimeRegister;
            entity.HourToTimeOff = model.HourToTimeOff;
            entity.LeaveDay1 = model.LeaveDay1;
            entity.LeaveDay2 = model.LeaveDay2;
            entity.TimeOffReal = model.TimeOffReal;
            entity.IsNonOvertime = model.IsNonOvertime;
            domain.ChangeMethodOverTime_Manual(entity);
            return Json(entity);
        }
        /// [Tho.Bui] - Xuất danh sách dữ liệu (Pregnancy) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult ExportPregnancytList([DataSourceRequest] DataSourceRequest request, Att_ReportPregnancySearchModel model)
        {
            return ExportAllAndReturn<Att_PregnancyEntity, Att_PregnancyModel, Att_ReportPregnancySearchModel>(request, model, ConstantSql.hrm_att_sp_get_ReportPregnancy);
        }

        /// <summary>
        /// [Hien.Nguyen] - xử lý form lưu ComputeOvertime
        /// </summary>
        /// <param name="updatedAtt_OvertimeModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateComputeOvertime([Bind(Prefix = "models")] List<Att_OvertimeModel> updatedAtt_OvertimeModel, [Bind(Prefix = "options")]List<string> Options)
        {
            string message = string.Empty;
            string status = Options[0] != null ? Options[0] : "";
            Guid UserAppvoveID = Options[5] != null ? Guid.Parse(Options[5]) : Guid.Empty;
            string method = Options[1] != null ? Options[1] : "";
            string Reason = Options[4] != null ? Options[4] : "";
            double TimeRegister = Options[2] != null && Options[2] != "" ? double.Parse(Options[2]) : 0;
            double HourToTimeOff = Options[3] != null && Options[3] != "" ? double.Parse(Options[3]) : 0;
            Guid UserLoginID = Guid.Empty;
            Guid.TryParse(Options.LastOrDefault(), out UserLoginID);
            //Guid UserLogin = Options.LastOrDefault() != null ? Guid.Parse(Options.LastOrDefault()) : Guid.Empty;
            double ResgisterHour = TimeRegister + HourToTimeOff;
            #region check so h dang ky

            var SysServices = new ActionService(UserLogin);
            var key = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString();
            var lstSysAllSetting = SysServices.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref message).Select(s => s.Value1).FirstOrDefault();
            if (lstSysAllSetting != null)
            {
                var registerConfig = Double.Parse(lstSysAllSetting);
                if (TimeRegister > registerConfig)
                {
                    var ls = new object[] { "errorRegisterHours", message };
                    return Json(ls);
                }
                if (HourToTimeOff > registerConfig)
                {
                    var ls = new object[] { "errorRegisterHours", message };
                    return Json(ls);
                }
                if (ResgisterHour > registerConfig)
                {
                    var ls = new object[] { "errorRegisterHours", message };
                    return Json(ls);
                }
            }
            #endregion

            Att_OvertimeServices domain = new Att_OvertimeServices();
            bool result = domain.CreateComputeOvertime(updatedAtt_OvertimeModel.Translate<Att_OvertimeEntity>(), method, status, UserAppvoveID, HourToTimeOff, TimeRegister, Reason, UserLoginID, UserLogin);
            if (result)
            {
                return Json(true);
            }
            return Json(false);
        }

        /// <summary>
        /// [Hien.Nguyen] - Chức năng không tính tăng ca - màng hình Phân Tích Tăng Ca
        /// </summary>
        /// <param name="lstOvertimeEntity"></param>
        public ActionResult CalNonAllowOvertime([Bind(Prefix = "models")] List<Att_OvertimeModel> updatedAtt_OvertimeModel)
        {
            Att_OvertimeServices domain = new Att_OvertimeServices();
            domain.CalNonAllowOvertime(updatedAtt_OvertimeModel.Translate<Att_OvertimeEntity>(), UserLogin);
            return Json(new { });
        }
        /// <summary>
        /// [Hien.Nguyen] - Chức năng tính tăng ca - màng hình Phân Tích Tăng Ca
        /// </summary>
        /// <param name="lstOvertimeEntity"></param>
        public ActionResult CalAllowOvertime([Bind(Prefix = "models")] List<Att_OvertimeModel> updatedAtt_OvertimeModel)
        {
            Att_OvertimeServices domain = new Att_OvertimeServices();
            domain.CalAllowOvertime(updatedAtt_OvertimeModel.Translate<Att_OvertimeEntity>(), UserLogin);
            return Json(new { });
        }


        #region Att_TimeSheet
        [HttpPost]
        public ActionResult GetTimeSheetList([DataSourceRequest] DataSourceRequest request, Att_TimeSheetSearchModel model)
        {
            return GetListDataAndReturn<Att_TimeSheetModel, Att_TimeSheetEntity, Att_TimeSheetSearchModel>(request, model, ConstantSql.hrm_att_sp_get_TimeSheet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllTimeSheetList([DataSourceRequest] DataSourceRequest request, Att_TimeSheetSearchModel model)
        {
            return ExportAllAndReturn<Att_TimeSheetEntity, Att_TimeSheetModel, Att_TimeSheetSearchModel>(request, model, ConstantSql.hrm_att_sp_get_TimeSheet);
        }
        #endregion
        #region BC Trễ Sớm Theo Năm
        [HttpPost]
        public ActionResult GetReportLateEarlyTotal([DataSourceRequest] DataSourceRequest request, Att_ReportLateEarlyTotalModel model)
        {
            var service = new Att_ReportLateEarlyTotalServices();
            var hrService = new ActionService(UserLogin);
            var entity = model.CopyData<Att_ReportLateEarlyTotalEntity>();

            //DateTime DateFrom = DateTime.Now;
            //DateTime DateTo = DateTime.Now;
            //List<Guid?> OrgIds = new List<Guid?>();
            //if (model.SDateFrom != null)
            //{
            //    DateFrom = model.SDateFrom.Value;
            //}
            //if (model.SDateTo != null)
            //{
            //    DateTo = model.SDateTo.Value;
            //}

            //string strOrgIDs = null;
            //if (!string.IsNullOrEmpty(model.OrgStructureID))
            //{
            //    strOrgIDs = model.OrgStructureID;
            //}
            //List<object> listObj = new List<object>();
            //listObj.Add(strOrgIDs);
            //listObj.Add(string.Empty);
            //listObj.Add(string.Empty);


            //string status = string.Empty;
            var result = new DataTable();
            //var listEntity = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, ref status).Select(s => s.ID).ToList();
            //if (listEntity == null || listEntity.Count != 0)
            //{
            result = service.GetReportLateEarlyTotal(entity, UserLogin);
            // }
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Att_ReportLateEarlyTotalModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }




            if (model.ExportId != Guid.Empty)
            {
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();
                //model.ValueFields = string.Empty;
                //if (result != null)
                //{
                //    foreach (DataColumn column in result.Columns)
                //    {
                //        model.ValueFields = model.ValueFields + "," + column.ColumnName.ToString();
                //    }
                //}
                //string[] valueField = null;
                //if (model.ValueFields != null)
                //{

                //    valueField = model.ValueFields.Split(',');
                //}
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                //var fullPath = ExportService.Export(model.ExportId, valueField, result, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue };


        }
        #endregion
        [HttpPost]
        public ActionResult AddDataForGrade(string ProfileIDs, Guid GradeAttendanceID, DateTime DateHire)
        {
            var servive = new Att_GradeServices();
            servive.AddDataForGrade(ProfileIDs, GradeAttendanceID, DateHire);
            return Json(null);
        }

        public bool GetPermissionApproved(string userID, string permissionCheck)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(userID, out id);
            if (id == Guid.Empty)
            {
                return false;
            }
            userID = Common.DotNetToOracle(userID);
            string status = string.Empty;
             var ActionService = new ActionService(UserLogin);

            ////kiểm tra đối với những dự án không sử dụng phê duyệt thì mặc định status là approved
            //var key = AppConfig.HRM_CONFIG_DEFAULTAPPROVED.ToString();
            //var defaultapproved = ActionService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            //if (defaultapproved != null && defaultapproved.Count > 0)
            //{
            //    var _default = defaultapproved.FirstOrDefault().Value1;
            //    if (_default == Boolean.TrueString)
            //    {
            //        return true;
            //    }
            //}

            List<object> lstSys = new List<object>();
            lstSys.Add(userID);
            lstSys.Add(permissionCheck);
            lstSys.Add(1);
            lstSys.Add(1000000);
            var config = ActionService.GetData<Sys_UserApproveEntity>(lstSys, ConstantSql.hrm_sys_sp_get_UserApprove, ref status);
            if (config != null && config.Count > 0)
            {
                return true;
            }
            return false;
        }

        #region BC phép năm

        public ActionResult ReportAnnualDetail([DataSourceRequest] DataSourceRequest request, Att_ReportAnnualDetailModel model)
        {
            #region Validate
            string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_ReportAnnualDetailModel>(model, "Att_ReportAnnualDetail", ref message);
            //if (!checkValidate)
            //{
            //    var ls = new object[] { "error", message };
            //    return Json(ls);
            //}
            if (model.Year == null)
            {
                var ls = new object[] { "error", string.Format(ConstantMessages.FieldNotAllowNull.TranslateString(), ConstantDisplay.HRM_Attendance_AnnualLeave_Year.TranslateString()) };
                return Json(ls);
            }
            #endregion

            ActionService Services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();

            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = Services.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = Services.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }

                //kết 2 list nhân viên lại
                if (listProfileByIds != null && listProfileByIds.Count > 0)
                {
                    foreach (var profile in listProfileByIds)
                    {
                        if (!listProfileByOrg.Any(m => m.ID == profile.ID))
                        {
                            listProfileByOrg.Add(profile);
                        }
                    }
                }

                //Lọc theo trạng thái nhân viên và nơi làm việc
                if (listProfileByOrg != null && listProfileByOrg.Count > 0)
                {
                    if (model.StatusSyn != null)
                    {
                        List<Hre_ProfileEntity> listProfileByOrgTEMP = new List<Hre_ProfileEntity>();

                        List<string> lstSTT = model.StatusSyn.Split(',').ToList();
                        if (lstSTT.Contains(StatusEmployee.E_WORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= model.DateTo) && pro.DateHire < model.DateFrom).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= model.DateTo && pro.DateHire >= model.DateFrom).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= model.DateTo && pro.DateQuit.Value >= model.DateFrom).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= model.DateTo).ToList());
                        }
                        listProfileByOrg = listProfileByOrgTEMP;
                    }
                    if (model.WorkPlace != null)
                    {
                        List<Guid> lstWP = model.WorkPlace.Split(',').Select(s => Guid.Parse(s)).ToList();
                        listProfileByOrg = listProfileByOrg.Where(s => s.WorkPlaceID != null && lstWP.Contains(s.WorkPlaceID.Value)).ToList();
                    }
                }
            }

            //lọc theo nơi làm việc
            if (model.WorkingPlaceID != null)
            {
                listProfileByOrg = listProfileByOrg.Where(m => m.WorkPlaceID == model.WorkingPlaceID).ToList();
            }

            #endregion
            Att_ReportServices Services1 = new Att_ReportServices();
            DataTable Table = Services1.ReportAnnualDetail(model.OrgStructureID, listProfileByOrg, model.Year ?? DateTime.Today.Year, model.IsCreateTemplate, model.ReportName, UserLogin);
            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = Table.Columns.Count;
                Table.Columns.RemoveAt(col - 1);
                obj = Table;
                isDataTable = true;
            }
            var WP = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo ?? DateTime.Now };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = model.ReportName,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            #endregion
            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


        #region Danh Sách DL Duyệt Overtime Cho Người Dùng

        [HttpPost]
        public ActionResult Get_Overtime_WillBeApproveByUser([DataSourceRequest] DataSourceRequest request, Att_OvertimeSearchOTModel model)
        {
            var service = new Att_ProcessApprovedServices();
            ActionService actionServices = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;

            #region GetData

            List<object> lstParamOT = new List<object>();
            lstParamOT.Add(model.OrgStructureID);
            lstParamOT.Add(model.DateStart);
            lstParamOT.Add(model.DateEnd);
            var lstOvertime = actionServices.GetData<Att_OvertimeEntity>(lstParamOT, ConstantSql.hrm_att_getdata_Overtime, ref status);

            if (model.OrgStructureID != null || model.ProfileName != null || model.CodeEmp != null)
            {
                ActionService hreService = new ActionService(UserLogin);
                List<object> listObj = new List<object>();
                listObj[0] = model.OrgStructureID;
                listObj[1] = model.ProfileName;
                listObj[2] = model.CodeEmp;
                var lstprofile = hreService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                if (lstprofile.Count() > 0)
                {
                    var lstPID = lstprofile.Select(s => s.ID).ToList();
                    lstOvertime = lstOvertime.Where(s => lstPID.Contains(s.ProfileID)).ToList();
                }
            }

            #endregion

            var listEntity = service.Get_Overtime_WillBeApproveByUser(model.SysUserID.Value, lstOvertime, UserLogin);
            var listModel = new List<Att_OvertimeModel>();
            if (listEntity != null && listEntity.Count > 0)
            {
                listModel = listEntity.Translate<Att_OvertimeModel>();
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().TotalRow != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : listModel.FirstOrDefault().TotalRow;
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            return Json(listModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Set_ApproveOvertime_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_ApproveOvertime_ByModuleApprove(host, lstIDS, UserLoginID, UserLogin);
            return Json(data);
        }
        [HttpPost]
        public JsonResult Set_RejectOvertime_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_RejectOvertime_ByModuleApprove(host, lstIDS, UserLoginID, UserLogin);
            return Json(data);
        }

        [HttpPost]
        public JsonResult Set_ApproveLeaveDay_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_ApproveLeaveDay_ByModuleApprove(host, lstIDS, UserLoginID,UserLogin);
            return Json(data);
        }
        [HttpPost]
        public JsonResult Set_RejectLeaveDay_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_RejectLeaveDay_ByModuleApprove(host, lstIDS, UserLoginID, UserLogin);
            return Json(data);
        }

        [HttpPost]
        public JsonResult Set_ApproveRoster_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_ApproveRoster_ByModuleApprove(host, lstIDS, UserLoginID, UserLogin);
            return Json(data);
        }
        public JsonResult Set_RejectRoster_ByModuleApprove(string host, string selectedIds, Guid UserLoginID)
        {
            List<Guid> lstIDS = new List<Guid>();
            if (selectedIds != null)
            {
                lstIDS = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            var service = new Att_ProcessApprovedServices();
            var data = service.Set_RejectRoster_ByModuleApprove(host, lstIDS, UserLoginID, UserLogin);
            return Json(data);
        }

        #endregion

        #region Danh Sách DL Duyệt LeaveDay Cho Người Dùng

        [HttpPost]
        public ActionResult Get_LeaveDay_WillBeApproveByUser([DataSourceRequest] DataSourceRequest request, Att_LeavedaySearchModel model)
        {
            var service = new Att_ProcessApprovedServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;

            #region GetData

            Att_LeavedayServices ldService = new Att_LeavedayServices();
            ActionService actionServices = new ActionService(UserLogin);
            List<object> para = new List<object>();
            para.AddRange(new object[3]);
            para[0] = model.OrgStructureID;
            para[1] = model.DateStart;
            para[2] = model.DateEnd;
            var lstLeaveDay = actionServices.GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, ref status);

            if (model.OrgStructureID != null || model.ProfileName != null || model.CodeEmp != null)
            {
                ActionService hreService = new ActionService(UserLogin);
                List<object> listObj = new List<object>();
                listObj[0] = model.OrgStructureID;
                listObj[1] = model.ProfileName;
                listObj[2] = model.CodeEmp;
                var lstprofile = hreService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                if (lstprofile.Count() > 0)
                {
                    var lstPID = lstprofile.Select(s => s.ID).ToList();
                    lstLeaveDay = lstLeaveDay.Where(s => lstPID.Contains(s.ProfileID)).ToList();
                }
            }

            #endregion

            var listEntity = service.Get_LeaveDay_WillBeApproveByUser(model.SysUserID.Value, lstLeaveDay, UserLogin);
            var listModel = new List<Att_LeaveDayModel>();
            if (listEntity != null && listEntity.Count > 0)
            {
                listModel = listEntity.Translate<Att_LeaveDayModel>();
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().TotalRow != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : listModel.FirstOrDefault().TotalRow;
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            return Json(listModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Danh Sách DL Duyệt Roster Cho Người Dùng

        [HttpPost]
        public ActionResult Get_Roster_WillBeApproveByUser([DataSourceRequest] DataSourceRequest request, Att_RosterSearchV2Model model)
        {
            var service = new Att_ProcessApprovedServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;

            #region GetData

            Att_RosterServices rtService = new Att_RosterServices();
            ActionService actionServices = new ActionService(UserLogin);
            List<object> lstParamRT = new List<object>();
            lstParamRT.AddRange(new object[4]);
            lstParamRT[0] = model.OrgStructureID;
            lstParamRT[1] = model.DateStart;
            lstParamRT[2] = model.DateEnd;
            List<Att_RosterEntity> listRoster = actionServices.GetData<Att_RosterEntity>(lstParamRT, ConstantSql.hrm_att_getdata_Roster, ref status);

            if (model.OrgStructureID != null || model.ProfileName != null || model.CodeEmp != null)
            {
                ActionService hreService = new ActionService(UserLogin);
                List<object> listObj = new List<object>();
                listObj[0] = model.OrgStructureID;
                listObj[1] = model.ProfileName;
                listObj[2] = model.CodeEmp;
                var lstprofile = hreService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                if (lstprofile.Count() > 0)
                {
                    var lstPID = lstprofile.Select(s => s.ID).ToList();
                    listRoster = listRoster.Where(s => lstPID.Contains(s.ProfileID)).ToList();
                }
            }

            #endregion

            var listEntity = service.Get_Roster_WillBeApproveByUser(model.SysUserID.Value, listRoster, UserLogin);
            var listModel = new List<Att_RosterModel>();
            if (listEntity != null && listEntity.Count > 0)
            {
                listModel = listEntity.Translate<Att_RosterModel>();
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().TotalRow != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : listModel.FirstOrDefault().TotalRow;
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            return Json(listModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult GetPersonalSubmitOvertime([DataSourceRequest] DataSourceRequest request, Att_PersonalSubmitOvertimeSearchModel otModel)
        {
            return GetListDataAndReturn<Att_OvertimeModel, Att_OvertimeEntity, Att_PersonalSubmitOvertimeSearchModel>(request, otModel, ConstantSql.hrm_att_sp_get_PersonalSubmitOvertime);
        }

        public ActionResult GetPersonalSubmitLeaveDay([DataSourceRequest] DataSourceRequest request, Att_PersonalSubmitLeavedaySearchModel otModel)
        {
            return GetListDataAndReturn<Att_LeaveDayModel, Att_LeaveDayEntity, Att_PersonalSubmitLeavedaySearchModel>(request, otModel, ConstantSql.hrm_att_sp_get_PersonalSubmitLeaveDay);
        }


        public ActionResult GetProfileChangeList([DataSourceRequest] DataSourceRequest request, Att_ConfigFirstYearSearchModel model)
        {

            //return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Profile);

            var service = new ActionService(UserLogin);
            Att_ReportServices attService = new Att_ReportServices();
            List<object> pataAnnualDetail = new List<object>();
            pataAnnualDetail.AddRange(new object[8]);
            pataAnnualDetail[0] = model.Year;
            pataAnnualDetail[3] = model.OrgStructureID;
            pataAnnualDetail[4] = model.ProfileName;
            pataAnnualDetail[5] = model.CodeEmp;
            pataAnnualDetail[6] = request.Page;
            pataAnnualDetail[7] = request.PageSize * 12;
            var status = string.Empty;
            var lstAnnualDetail = service.GetData<Att_AnnualDetailEntity>(pataAnnualDetail, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);
            DateTime? datemax = lstAnnualDetail.OrderByDescending(m => m.MonthYear).Select(m => m.MonthYear).FirstOrDefault();
            var listModelNull = new List<Att_AnnualDetailModel>();
            if (datemax == null)
            {

                ModelState.AddModelError("Id", status);
                return Json(listModelNull.ToDataSourceResult(request, ModelState));
            }
            var result = lstAnnualDetail.Where(m => m.MonthYear == datemax).ToList();
            var listModel = new List<Att_AnnualDetailModel>();
            if (result != null)
            {
                request.Page = 1;
                foreach (var item in result)
                {
                    var newModle = (Att_AnnualDetailModel)typeof(Att_AnnualDetailModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                //return new JsonResult() { Data = dataSourceResult.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
            }
            //var listModelNull = new List<Att_AnnualDetailModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
            #region MyRegion
            //object obj = new Att_AnnualDetailEntity();
            //if (model != null && model.IsCreateTemplate)
            //{
            //    var path = Common.GetPath("Templates");
            //    ExportService exportService = new ExportService();
            //    ConfigExport cfgExport = new ConfigExport()
            //    {
            //        Object = obj,
            //        FileName = "Att_AnnualDetailEntity",
            //        OutPutPath = path,
            //        DownloadPath = Hrm_Main_Web + "Templates",
            //        IsDataTable = false
            //    };
            //    var str = exportService.CreateTemplate(cfgExport);
            //    return Json(str);
            //}

            //Att_AnnualDetailSearchV2Model mSearch = new Att_AnnualDetailSearchV2Model();
            //mSearch.Year = model.Year;
            //mSearch.OrgStructureID = model.OrgStructureID;
            //mSearch.ProfileName = model.ProfileName;
            //mSearch.CodeEmp = model.CodeEmp;

            //var service = new ActionService(UserLogin);
            //ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    PageSize = request.PageSize,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request),
            //    AdvanceFilters = ExtractAdvanceFilterAttributes(mSearch)
            //};
            //var status = string.Empty;
            //var listEntity = service.GetData<Att_AnnualDetailEntity>(lstModel, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);

            //#region xử lý xuất báo cáo
            //if (model.ExportId != Guid.Empty)
            //{
            //    var fullPath = ExportService.Export(model.ExportId, listEntity);
            //    return Json(fullPath);
            //}
            //#endregion

            //var listModel = new List<Att_AnnualDetailModel>();
            //if (listEntity != null)
            //{
            //    request.Page = 1;
            //    listModel = listEntity.Translate<Att_AnnualDetailModel>();
            //    var dataSourceResult = listModel.ToDataSourceResult(request);
            //    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().TotalRow;
            //    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            //}
            //ModelState.AddModelError("Id", status);
            //return Json(listModel.ToDataSourceResult(request, ModelState));

            #endregion

        }

        #region Att_ConfigFirstYearOff
        public ActionResult GetOffProfileChangeList([DataSourceRequest] DataSourceRequest request, Att_ConfigFirstYearOffSearchModel model)
        {
            var service = new ActionService(UserLogin);
            Att_ReportServices attService = new Att_ReportServices();
            List<object> pataAnnualDetail = new List<object>();
            pataAnnualDetail.AddRange(new object[8]);
            pataAnnualDetail[0] = model.Year;
            pataAnnualDetail[3] = model.OrgStructureID;
            pataAnnualDetail[4] = model.ProfileName;
            pataAnnualDetail[5] = model.CodeEmp;
            pataAnnualDetail[6] = request.Page;
            pataAnnualDetail[7] = request.PageSize * 12;
            var status = string.Empty;
            var lstAnnualDetail = service.GetData<Att_AnnualDetailEntity>(pataAnnualDetail, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);
            DateTime? datemax = lstAnnualDetail.OrderByDescending(m => m.MonthYear).Select(m => m.MonthYear).FirstOrDefault();
            var listModelNull = new List<Att_AnnualDetailModel>();
            if (datemax == null)
            {
                ModelState.AddModelError("Id", status);
                return Json(listModelNull.ToDataSourceResult(request, ModelState));
            }
            var result = lstAnnualDetail.Where(m => m.MonthYear == datemax).ToList();
            var listModel = new List<Att_AnnualDetailModel>();
            if (result != null)
            {
                request.Page = 1;
                foreach (var item in result)
                {
                    var newModle = (Att_AnnualDetailModel)typeof(Att_AnnualDetailModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }
        #endregion


        public ActionResult SetSickLeaveBeginYear([DataSourceRequest] DataSourceRequest request, Att_ConfigFirstYearSearchModel model)
        {
            var service = new ActionService(UserLogin);
            List<object> pataAnnualDetail = new List<object>();
            pataAnnualDetail.AddRange(new object[8]);
            pataAnnualDetail[0] = model.Year;
            pataAnnualDetail[3] = model.OrgStructureID;
            pataAnnualDetail[4] = model.ProfileName;
            pataAnnualDetail[5] = model.CodeEmp;
            pataAnnualDetail[6] = request.Page;
            pataAnnualDetail[7] = request.PageSize * 12;
            var status = string.Empty;
            var lstAnnualDetail = service.GetData<Att_AnnualDetailEntity>(pataAnnualDetail, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);



            Guid[] lstProfileIDs = null;
            Att_AnnualLeaveServices attServicesAnnualLeave = new Att_AnnualLeaveServices();
            //attServicesAnnualLeave.SetSickLeaveBeginYear(lstProfileIDs, model.NumerDayChange, model.YearChange);
            return Json("success");
        }
        public ActionResult SetAnnualLeaveBeginYear([DataSourceRequest] DataSourceRequest request, Att_ConfigFirstYearSearchModel model)
        {

            var service = new ActionService(UserLogin);
            List<object> pataAnnualDetail = new List<object>();
            pataAnnualDetail.AddRange(new object[8]);
            pataAnnualDetail[0] = model.Year;
            pataAnnualDetail[3] = model.OrgStructureID;
            pataAnnualDetail[4] = model.ProfileName;
            pataAnnualDetail[5] = model.CodeEmp;
            pataAnnualDetail[6] = 1;
            pataAnnualDetail[7] = int.MaxValue - 1;
            var status = string.Empty;
            var lstAnnualDetail = service.GetData<Att_AnnualDetailEntity>(pataAnnualDetail, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);
            var lstProfileID = lstAnnualDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).Distinct().ToList();
            Att_AnnualLeaveServices attServicesAnnualLeave = new Att_AnnualLeaveServices();
            if (model.TypeAnnual == TypeAnnual.E_SICK.ToString())
            {
                attServicesAnnualLeave.SetSickLeaveBeginYear(lstProfileID, model.NumerDayChange, model.YearChange);
            }
            else
            {
                attServicesAnnualLeave.SetAnnualLeaveBeginYear(lstProfileID, model.NumerDayChange, model.YearChange, null, null, null);
            }
            return Json("success");

        }
        #region Cau hinh phep bu dau nam
        public ActionResult SetCompensationLeaveBeginYear([DataSourceRequest] DataSourceRequest request, Att_ConfigFirstYearOffSearchModel model)
        {
            var service = new ActionService(UserLogin);
            List<object> pataAnnualDetail = new List<object>();
            pataAnnualDetail.AddRange(new object[8]);
            pataAnnualDetail[0] = model.Year;
            pataAnnualDetail[3] = model.OrgStructureID;
            pataAnnualDetail[4] = model.ProfileName;
            pataAnnualDetail[5] = model.CodeEmp;
            pataAnnualDetail[6] = 1;
            pataAnnualDetail[7] = int.MaxValue - 1;
            var status = string.Empty;
            var lstAnnualDetail = service.GetData<Att_AnnualDetailEntity>(pataAnnualDetail, ConstantSql.hrm_att_sp_get_AnnualDetail, ref status);
            var lstProfileID = lstAnnualDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).Distinct().ToList();
            Att_CompensationServices attServicesCompensation = new Att_CompensationServices();
            attServicesCompensation.SetCompensationLeaveBeginYear(lstProfileID, model.NumerDayChange, model.YearChange, null, null, null);
            return Json("success");
        }
        #endregion

        #region Att_CompensationConfig


        public ActionResult GetCompensationConfigList([DataSourceRequest] DataSourceRequest request, Att_CompensationConfigSearchModel model)
        {
            return GetListDataAndReturn<Att_CompensationConfigModel, Att_CompensationConfigEntity, Att_CompensationConfigSearchModel>(request, model, ConstantSql.hrm_att_sp_get_CompensationConfig);
        }
        public ActionResult ExportCompensationConfigSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Att_CompensationConfigEntity, Att_CompensationConfigModel>(selectedIds, valueFields, ConstantSql.hrm_att_sp_get_CompensationConfigByIds);
        }
        public ActionResult ExportAllCompensationConfigList([DataSourceRequest] DataSourceRequest request, Att_CompensationConfigSearchModel model)
        {
            return ExportAllAndReturn<Att_CompensationConfigEntity, Att_CompensationConfigModel, Att_CompensationConfigSearchModel>(request, model, ConstantSql.hrm_att_sp_get_CompensationConfig);
        }
        #endregion

        #region ImportRoster

        [HttpPost]
        public JsonResult ImportRosterShowList([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Att_RosterServices();
            List<ImportRosterModel> ListRosterImport = new List<ImportRosterModel>();
            List<ImportRosterModel> ListRosterCorrect = new List<ImportRosterModel>();
            List<ImportRosterModel> ListRosterError = new List<ImportRosterModel>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");

            var ImportService = new ImportRosterService
            {
                SkipRowNumbers = model.SkipRowNumbers,
                LoginUserID = model.UserID,
                FileName = _fileName
            };

            if (model.ImportRosterType == ImportRosterType.OverrideMonth.ToString())
            {
                ImportService.ImportType = ImportRosterType.OverrideMonth;
            }
            else
            {
                ImportService.ImportType = ImportRosterType.OverrideHasValue;
            }

            try
            {
                ImportService.ImportRoster();
                model.ListImportData = ImportService.GetImportObject();
                model.ListInvalidData = ImportService.GetInvalidObject();
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveImportRoster([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var ImportService = new ImportRosterService
            {
                LoginUserID = model.UserID,
            };

            if (model.ImportRosterType == ImportRosterType.OverrideHasValue.ToString())
            {
                ImportService.ImportType = ImportRosterType.OverrideHasValue;
            }
            else
            {
                ImportService.ImportType = ImportRosterType.OverrideMonth;
            }

            try
            {
                var dataErrorCode = ImportService.SaveRoster();
                string message = NotificationType.Success.ToString();

                if (dataErrorCode != DataErrorCode.Success)
                {
                    message = NotificationType.Error.ToString();
                }

                model.Description = message;
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public FilePathResult DownloadRosterTemplate([DataSourceRequest] DataSourceRequest request)
        {
            string fileName = ConstantPath.ImportRosterTemplate;
            string templatepath = Common.GetPath(Common.TemplateURL).Replace("/", "\\");
            return File(templatepath + "\\" + fileName, DefaultConstants.ExcelContentType, fileName);
        }

        #endregion

        #region ImportLeaveday
        public static IList ListInvalidDataTempLeave { get; set; }
        private static ImportService ImportServiceLeave { get; set; }

        [HttpPost]
        public JsonResult ImportLeavedayShowList([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            Session["LeavedayImport"] = null;
            var services = new Att_LeavedayServices();
            List<ImportLeavedayModel> ListLeavedayImport = new List<ImportLeavedayModel>();
            List<ImportLeavedayModel> ListLeavedayCorrect = new List<ImportLeavedayModel>();
            List<ImportLeavedayModel> ListLeavedayError = new List<ImportLeavedayModel>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");

            ImportServiceLeave = new ImportService
            {
                //FileName = Common.GetPath(model.TemplateFile),
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,

            };
            try
            {
                var table = ImportServiceLeave.ImportNew(_fileName, model.TemplateFile, model.ID);
                ListLeavedayImport = table.Translate<ImportLeavedayModel>();
                services.CheckData((Guid)Session[SessionObjects.UserId], ListLeavedayImport, out ListLeavedayCorrect, out ListLeavedayError);
                Session["LeavedayImport"] = ListLeavedayCorrect;
                if (ListLeavedayError.Count > 0)
                {
                    return Json(ListLeavedayError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(ListLeavedayError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveImportLeaveday([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Att_LeavedayServices();
            List<ImportLeavedayModel> ListLeavedayCorrect = (List<ImportLeavedayModel>)Session["LeavedayImport"];
            try
            {
                var message = services.SaveListLeave(ListLeavedayCorrect);
                if (message == NotificationType.Success.ToString())
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(NotificationType.Error.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult UpdateRegisterHours(string timeStart, string timeEnd)
        {
            double RegisterHours = 0;
            if (!string.IsNullOrEmpty(timeStart) && !string.IsNullOrEmpty(timeEnd))
            {
                string[] time = timeStart.Split(':');
                DateTime Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(time.FirstOrDefault()), int.Parse(time.LastOrDefault()), 0);
                time = timeEnd.Split(':');
                DateTime End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(time.FirstOrDefault()), int.Parse(time.LastOrDefault()), 0);

                if (End < Start)
                {
                    End = End.AddDays(1);
                }

                RegisterHours = End.Subtract(Start).TotalHours;
            }
            return Json(RegisterHours);
        }
    }
}