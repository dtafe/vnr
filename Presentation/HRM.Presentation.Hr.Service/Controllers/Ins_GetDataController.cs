using System;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Insurance.Domain;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using HRM.Business.Insurance.Models;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Presentation.Insurance.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using System.IO;
using HRM.Business.Category.Domain;
using System.Data;
using System.Reflection;
using HRM.Presentation.Hr.Models;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class
        Ins_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        BaseService baseService = new BaseService();
        string _status = string.Empty;

        #region Get List

        public ActionResult GetChildSickList([DataSourceRequest] DataSourceRequest request, Ins_ChildSickModel model)
        {
            return GetListDataAndReturn<Ins_ChildSickModel, Ins_ChildSickEntity, Ins_ChildSickModel>(request, model, ConstantSql.hrm_ins_sp_get_ChildSick);
        }
        public JsonResult GetMultiChildSick(string text, Guid profileId)
        {
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(profileId);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Ins_ChildSickEntity>(lstParam, ConstantSql.hrm_ins_sp_get_ChildSick_Multi, ref status);
            var result = get.Select(item => new Ins_ChildSickModel()
            {
                ID = item.ID,
                ChildSickName = item.ChildSickName
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInsuranceRecordList([DataSourceRequest] DataSourceRequest request, Ins_InsuranceRecordSearchModel model)
        {
            return GetListDataAndReturn<Ins_InsuranceRecordModel, Ins_InsuranceRecordEntity, Ins_InsuranceRecordSearchModel>(request, model, ConstantSql.hrm_ins_sp_get_InsuranceRecord);
        }
        [HttpPost]
        public ActionResult RealDayCount([DataSourceRequest] DataSourceRequest request, string ProfileIDs, string insurancetype, DateTime? DateStart, DateTime? DateEnd)
        {
            List<Guid> listProfileID = new List<Guid>();
            var result = 0;
            var strprofileid = ProfileIDs.Split(',');
            string status = string.Empty;
            foreach (var x in strprofileid)
            {
                try
                {
                    listProfileID.Add(Common.ConvertToGuid(x));

                }
                catch
                {
                }
            }
            var baseservice = new ActionService(UserLogin);  //new BaseService();
            var listPara_InOut = new List<object>();
            listPara_InOut.Add(null);
            listPara_InOut.Add(DateStart);
            listPara_InOut.Add(DateEnd);
            //Lay danh sach InOut
            List<Att_WorkdayEntity> lstIO = new List<Att_WorkdayEntity>();
            //DS InOut
            lstIO = baseservice.GetData<Att_WorkdayEntity>(listPara_InOut, ConstantSql.hrm_att_getdata_Workday, ref status);
            if (lstIO != null)
            {
                lstIO = lstIO.Where(x => listProfileID.Contains(x.ProfileID)).ToList();
                if (lstIO == null || lstIO.Count == 0)
                {
                    result = 0;
                    //return Json(result);
                }
            }
            DateTime DateEnd2 = DateEnd.Value.AddDays(1).AddMinutes(-1);
            var listPara_DayOff = new List<object>();
            listPara_DayOff.Add(DateStart);
            listPara_DayOff.Add(DateEnd2);

            List<DateTime> lstDayOff = baseservice.GetData<Cat_DayOffEntity>(listPara_DayOff, ConstantSql.hrm_cat_getdata_DayOff, ref status).Select(x => x.DateOff).ToList<DateTime>();
            bool isWorkDayByInOut = false; //Có inout là có đi làm
            bool isWorkDayByInOutMoreThan2Hour = false; // có in out nhung phải trên 2 tiếng thì mới có đi làm
            //string typeCfg = AppConfig.E_CONFIG_INSRECORD.ToString();
            var objWorkDayByInOut = baseservice.GetData<Sys_AllSettingEntity>(AppConfig.E_CONFIG_INSRECORD_WORKDAYBYINOUT.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            if (objWorkDayByInOut.FirstOrDefault() != null)
                bool.TryParse(objWorkDayByInOut.FirstOrDefault().Value1, out isWorkDayByInOut);
            var objWorkDayByInOutMoreThan2Hour = baseservice.GetData<Sys_AllSettingEntity>(AppConfig.E_CONFIG_INSRECORD_WORKDAYBYINOUTMORETHAN2HOUR.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            if (objWorkDayByInOut.FirstOrDefault() != null)
                bool.TryParse(objWorkDayByInOutMoreThan2Hour.FirstOrDefault().Value1, out isWorkDayByInOutMoreThan2Hour);
            foreach (Guid profileID in listProfileID)
            {

                DateTime startIns = DateStart.Value;
                DateTime endIns = DateEnd.Value;

                int Numday = InsuranceServices.CalNumOfDayLeave(profileID, startIns, endIns, insurancetype, lstDayOff, lstIO, isWorkDayByInOut, isWorkDayByInOutMoreThan2Hour);
                result = Numday;
                break;
            }

            return Json(result);

        }

        #region InsuranceSettlement - Quyết Toán Bảo Hiểm

        public ActionResult ExportInsuranceSettlementSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Ins_InsuranceSettlementEntity, Ins_InsuranceSettlementModel>(selectedIds, valueFields, ConstantSql.hrm_ins_sp_get_InsuranceSettlementByIds);
        }

        [HttpPost]
        public ActionResult ExportInsuranceSettlementList([DataSourceRequest] DataSourceRequest request, Ins_InsuranceSettlementSearchModel model)
        {
            return ExportAllAndReturn<Ins_InsuranceSettlementModel, Ins_InsuranceSettlementEntity, Ins_InsuranceSettlementSearchModel>(request, model, ConstantSql.hrm_ins_sp_get_InsuranceSettlement);
        }

        public ActionResult GetInsuranceSettlementList([DataSourceRequest] DataSourceRequest request, Ins_InsuranceSettlementSearchModel model)
        {
            return GetListDataAndReturn<Ins_InsuranceSettlementModel, Ins_InsuranceSettlementEntity, Ins_InsuranceSettlementSearchModel>(request, model, ConstantSql.hrm_ins_sp_get_InsuranceSettlement);
        }

        #region Chức Năng Quyết Toán
        /// <summary>xác nhận “Đã Quyết toán”</summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsuranceSettlementSelected(string selectedIds)
        {
            var profileIds = selectedIds.Split(',')
                .Where(g => { Guid temp; return Guid.TryParse(g, out temp); })
                .Select(s => Guid.Parse(s)).ToList();

            var service = new InsuranceServices();
            var result = service.InsuranceSettlementSelected(profileIds);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>“Trả sổ bảo hiểm”</summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsuranceReceiveSocialInsSelected(string selectedIds)
        {
            var profileIds = selectedIds.Split(',')
              .Where(g => { Guid temp; return Guid.TryParse(g, out temp); })
              .Select(s => Guid.Parse(s)).ToList();

            var service = new InsuranceServices();
            var result = service.InsuranceReceiveSocialInsSelected(profileIds);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #endregion

        #region Report

        public ActionResult ExportInsuranceRecordSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Ins_InsuranceRecordEntity, Ins_InsuranceRecordModel>(selectedIds, valueFields, ConstantSql.hrm_ins_sp_get_InsuranceRecordByIds);
        }

        [HttpPost]
        public ActionResult ExportInsuranceRecordList([DataSourceRequest] DataSourceRequest request, Ins_InsuranceRecordSearchModel model)
        {
            return ExportAllAndReturn<Ins_InsuranceRecordEntity, Ins_InsuranceRecordModel, Ins_InsuranceRecordSearchModel>(request, model, ConstantSql.hrm_ins_sp_get_InsuranceRecord);
        }
        #endregion

        #region Export

        public ActionResult ExportAnalyzeInsuranceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Ins_ProfileInsuranceMonthlyEntity, Ins_ProfileInsuranceMonthlyModel>(selectedIds, valueFields, ConstantSql.hrm_ins_sp_get_ProfInsMonthlyByIds);
        }

        [HttpPost]
        public ActionResult ExportAnalyzeInsuranceList([DataSourceRequest] DataSourceRequest request, Ins_ProfileInsuranceMonthlySearchModel model)
        {
            return ExportAllAndReturn<Ins_ProfileInsuranceMonthlyModel, Ins_ProfileInsuranceMonthlyEntity, Ins_ProfileInsuranceMonthlySearchModel>(request, model, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly);
        }

        #endregion

        #region Other

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
            var hrService = new ActionService(UserLogin); //Hre_ProfileServices();
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

        static bool _isProcessing = false;
        #region Phân Tích
        [HttpPost]
        public ActionResult AnalyzeInsurance([DataSourceRequest] DataSourceRequest request, Ins_ProfileInsuranceMonthlySearchModel model)
        {
            var messageReturn = "Success";

            var insService = new InsuranceServices();
            var dateCheck = DateTime.Now;
            string orgs = null;
            var periodInsurance = string.Empty;
            var codeEmp = string.Empty;
            if (model != null)
            {
                if (model.MonthYear.HasValue)
                {
                    dateCheck = model.MonthYear.Value;
               }
                orgs = model.OrgStructureID;
                periodInsurance = Request["PeriodInsurance"] != null ? Request["PeriodInsurance"].ToString() : string.Empty;
                codeEmp = Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp] != null ? Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp].ToString() : string.Empty;
            }
            try
            {
                if (_isProcessing == false)
                {
                    _isProcessing = true;
                    insService.AnalyzeAndSaveInsuranceByMonth(orgs, dateCheck, periodInsurance, codeEmp, model.SocialInsPlaceIDs,UserLogin);
                }
                else
                {
                    messageReturn = "IsProcessing";
                }
            }
            catch (Exception)
            {
                _isProcessing = false;
            }
            _isProcessing = false;

            return Json(messageReturn);
        }

        public ActionResult AnalyzeInsuranceList([DataSourceRequest] DataSourceRequest request, Ins_ProfileInsuranceMonthlySearchModel model)
        {
            string status = string.Empty;
            var socialInsPlaceIDs = model.SocialInsPlaceIDs;
            var hrService = new Hre_ProfileServices();
            var services = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.AddRange(new object[5]);
            listObj[0] = model.OrgStructureID;
            listObj[1] = model.MonthYear;
            listObj[3] = 1;
            listObj[4] = Int32.MaxValue - 1;
            var result = services.GetData<Ins_ProfileInsuranceMonthlyModel>(listObj, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, ref status);
            result = result.Where(m => (m.IsSocialInsurance.HasValue && m.IsSocialInsurance.Value)).ToList();

            socialInsPlaceIDs=socialInsPlaceIDs.Where(m=>m != Guid.Empty).ToList();
            if (socialInsPlaceIDs.Any())
            {
                result = result.Where(m => socialInsPlaceIDs.Contains(m.SocialInsPlaceID ?? Guid.Empty)).ToList();    
            }           
            
            foreach (var item in result)
            {
                if (item.IsSocialInsurance.HasValue && !item.IsSocialInsurance.Value)
                {
                    item.SalaryInsurance = null;
                    item.SalaryHealthInsurance = null;
                }
                if (item.IsUnEmpInsurance.HasValue && !item.IsUnEmpInsurance.Value)
                {
                    item.SalaryUnEmpInsurance = null;
                }
            }

            //GetListData<Ins_ProfileInsuranceMonthlyModel, Ins_ProfileInsuranceMonthlyEntity, Ins_ProfileInsuranceMonthlySearchModel>(model, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, ref status);
            if (result.Any())
            {
                var codeEmp = Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp] != null ? Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    result = result.Where(p => p.CodeEmp == codeEmp).ToList();
                }
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult AnalyzeInsuranceValidate([DataSourceRequest] DataSourceRequest request, Ins_ProfileInsuranceMonthlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var codeEmp = Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp] != null ? Request[Ins_ProfileInsuranceMonthlyModel.FieldNames.CodeEmp].ToString() : string.Empty;

            if (model.OrgStructureID == null && string.IsNullOrEmpty(codeEmp))
            {
                var ls = new object[] { "error", ConstantDisplay.OrgNEmpCodeNotAllowNull.TranslateString() };
                return Json(ls);
            }
            else
            {
                var checkValidate = ValidatorService.OnValidateData<Ins_ProfileInsuranceMonthlySearchModel>(model, "AnalyzeInsurance", ref message);
                if (!checkValidate)
                {
                    var ls = new object[] { "error", message };
                    return Json(ls);
                }
            }

            #endregion
            return Json(message);
        }

        #endregion

        #endregion

        #region Insurance Report

        #region BC D02

        /// <summary>Lấy ngày chốt BH (từ cấu hình )</summary>
        /// <returns></returns>
        public ActionResult GetPeriodInsuranceDay()
        {
            var service = new Ins_InsuranceReportServices();
            var periodInsDayCurrentMonth = service.PeriodInsuranceDayCurrentMonth;
            return Json(periodInsDayCurrentMonth, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportReportD02TSValidate([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD02SearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_InsuranceReportD02SearchModel>(model, "Ins_ReportD02TS", ref message);

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);

            }
            #endregion

            return Json(message);
        }

        /// <summary> BC D02 TS </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Ins_InsuranceRptD02TS([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD02Model model)
        {
            var services = new Ins_InsuranceReportServices();
            List<HeaderInfo> listHeaderInfo = null;
            var result = services.SearchD02TS(model.All, model.Increase, model.Descrease, model.MonthYear, model.OrgStructureID, model.SearchNoteType, model.SearchStatus, model.CodeEmp, model.SocialInsPlaceIDs,UserLogin);
            var lstModel = new List<Ins_InsuranceReportD02Model>();
            if (result != null)
            {
                //lstModel = result.Translate<Ins_InsuranceReportD02Model>();
                foreach (var item in result)
                {
                    var rptD02Model = new Ins_InsuranceReportD02Model();
                    rptD02Model = item.Copy<Ins_InsuranceReportD02Model>();
                    lstModel.Add(rptD02Model);
                }

                //  return Json(lstModel, JsonRequestBehavior.AllowGet);
            }
            var isDataTable = false;
            object obj = new Ins_InsuranceReportD02Model();

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }

            #region Header Info
            var codeEmp = string.Empty;
            if (model != null && model.UserID != Guid.Empty)
            {
                Hre_ProfileServices profileService = new Hre_ProfileServices();
                var profile = profileService.GetProfileInfo(model.UserID,UserLogin);
                if (profile != null && !string.IsNullOrEmpty(profile.CodeEmp))
                {
                    codeEmp = profile.CodeEmp;
                }
            }
            HeaderInfo headerInfoMonthYear = new HeaderInfo() { Name = "MonthYear", Value = model.MonthYear };
            HeaderInfo headerInfoCurrentDate = new HeaderInfo() { Name = "CurrentDate", Value = DateTime.Now };
            HeaderInfo headerInfoCodeEmp = new HeaderInfo() { Name = "CodeEmp", Value = codeEmp };
            listHeaderInfo = new List<HeaderInfo>() { headerInfoMonthYear, headerInfoCurrentDate, headerInfoCodeEmp };
            #endregion

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Ins_InsuranceReportD02SearchModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    HeaderInfo = listHeaderInfo,
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, listHeaderInfo, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        /// <summary>Phân Tích D02</summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputeReportReportD02TS([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD02Model model)
        {
            var services = new Ins_InsuranceReportServices();
            var result = services.LoadData(model.All, model.Increase, model.Descrease, model.MonthYear, model.OrgStructureID, model.SearchNoteType, model.SearchStatus, model.CodeEmp, model.SocialInsPlaceIDs,UserLogin);
            var lstModel = new List<Ins_InsuranceReportD02Model>();
            if (result != null)
            {
                //lstModel = result.Translate<Ins_InsuranceReportD02Model>();
                foreach (var item in result)
                {
                    var rptD02Model = new Ins_InsuranceReportD02Model();
                    rptD02Model = item.Copy<Ins_InsuranceReportD02Model>();
                    lstModel.Add(rptD02Model);
                }

                //  return Json(lstModel, JsonRequestBehavior.AllowGet);
            }
            var isDataTable = false;
            object obj = new Ins_InsuranceReportD02Model();

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
                    FileName = "Ins_InsuranceReportD02SearchModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }


        [HttpPost]
        public JsonResult GetDropdownlistTypeInsuranceD02TS(string typeId)
        {
            var a = Enum.GetValues(typeof(TypeInsuranceD02TS)).Cast<TypeInsuranceD02TS>().ToList();
            var result = new List<string>();
            if (typeId == TypeInsuranceD02TS.E_TANG.ToString())
            {
                result.Add("All");
                result.Add(TypeInsuranceD02TS.E_TANG_LD.ToString());
                result.Add(TypeInsuranceD02TS.E_TANG_TS.ToString());
                //result.Add(TypeInsuranceD02TS.E_TANG_BENH.ToString());
                //result.Add(TypeInsuranceD02TS.E_TANG_BHYT.ToString());
                result.Add(TypeInsuranceD02TS.E_TANG_BHTN.ToString());
                result.Add(TypeInsuranceD02TS.E_TANG_LEAVE_14WORKINGDAYS.ToString());

            }
            if (typeId == TypeInsuranceD02TS.E_GIAM.ToString())
            {
                result.Add("All");
                result.Add(TypeInsuranceD02TS.E_GIAM_LD_BHYT.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_QUIT_SUSPENSE.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_TS.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_LEAVE_14WORKINGDAYS.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_PREGNANT_14WORKINGDAYS.ToString());
                //result.Add(TypeInsuranceD02TS.E_GIAM_BENH.ToString());
            }
            if (typeId == TypeInsuranceD02TS.E_TANG_LUONG.ToString())
            {
                result.Add("All");
                result.Add(TypeInsuranceD02TS.E_TANG_LUONG.ToString());
                result.Add(TypeInsuranceD02TS.E_TANG_LUONG_CHANGEJOBNAME.ToString());
            }
            if (typeId == TypeInsuranceD02TS.E_GIAM_LUONG.ToString())
            {
                result.Add("All");
                result.Add(TypeInsuranceD02TS.E_GIAM_LUONG.ToString());
                result.Add(TypeInsuranceD02TS.E_GIAM_LUONG_CHANGEJOBNAME.ToString());
            }
            if (typeId == TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString())
            {
                result.Add(TypeInsuranceD02TS.E_CHANGEJOBNAME.ToString());
            }
            if (typeId == TypeInsuranceD02TS.E_Dieu_Chinh.ToString())
            {             
                result.Add("All");
                result.Add(TypeInsuranceD02TS.E_Dieu_Chinh.ToString());
            }

            IList<SelectListItem> listD02 = a.Where(p => result.Contains(p.ToString())).Select(x => new SelectListItem { Value = x.ToString(), Text = EnumDropDown.GetEnumDescription(x) }).ToList();
            if (!listD02.Any() || listD02.Count > 1)
            {
                listD02.Insert(0, new SelectListItem { Value = "All", Text = "Tất Cả" });
            }
            return Json(listD02, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region BC D03

        [HttpPost]
        public ActionResult ReportReportD03TSValidate([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD03SearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_InsuranceReportD03SearchModel>(model, "Ins_ReportD03TS", ref message);

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);

            }
            #endregion

            return Json(message);
        }

        /// <summary> BC D02 TS </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Ins_InsuranceRptD03TS([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD03Model model)
        {
            var services = new Ins_InsuranceReportServices();
            var result = services.LoadDataD03(model.All, model.Increase, model.Descrease, model.MonthYear, model.OrgStructureID, model.SearchNoteType, model.SearchStatus, model.CodeEmp, model.WorkPlaceIDs,UserLogin);
            var lstModel = new List<Ins_InsuranceReportD03Model>();
            if (result != null)
            {
                //lstModel = result.Translate<Ins_InsuranceReportD03Model>();
                foreach (var item in result)
                {
                    var rptD02Model = new Ins_InsuranceReportD03Model();
                    rptD02Model = item.Copy<Ins_InsuranceReportD03Model>();
                    lstModel.Add(rptD02Model);
                }

                //  return Json(lstModel, JsonRequestBehavior.AllowGet);
            }
            var isDataTable = false;
            object obj = new Ins_InsuranceReportD03Model();

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
                    FileName = "Ins_InsuranceReportD03SearchModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }


        #endregion

        #region BC Dữ Liệu HDTJob Lần 2
        
        [HttpPost]
        public ActionResult Ins_ReportEmpHardJob2ndValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportEmpHardJob2ndSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportEmpHardJob2ndSearchModel>(model, "Ins_ReportEmpHardJob2nd", ref message);

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        /// <summary> BC Dữ Liệu HDTJob Lần 2 </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Ins_ReportEmpHardJob2nd([DataSourceRequest] DataSourceRequest request, Ins_ReportEmpHardJob2ndModel model)
        {
            var services = new Ins_InsuranceReportServices();
          //  var result = services.LoadDataD03(model.All, model.Increase, model.Descrease, model.MonthYear, model.OrgStructureID, model.SearchNoteType, model.SearchStatus, model.CodeEmp, model.WorkPlaceIDs);
            var result = services.ReportEmpHardJob2nd(model.OrgStructureID, model.MonthYear.Value,UserLogin).Translate<Ins_ReportEmpHardJob2ndModel>();
            result = result.OrderBy(m => m.Stt).ToList();
            var lstModel = new List<Ins_ReportEmpHardJob2ndModel>();
            if (result != null)
            {
                foreach (var item in result)
                {
                    var rptD02Model = new Ins_ReportEmpHardJob2ndModel();
                    rptD02Model = item.Copy<Ins_ReportEmpHardJob2ndModel>();
                    lstModel.Add(rptD02Model);
                }
            }
            var isDataTable = false;
            object obj = new Ins_ReportEmpHardJob2ndModel();

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
                    FileName = "Ins_ReportEmpHardJob2ndSearchModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }


        #endregion

        public ActionResult GetReportNotHaveSocial([DataSourceRequest] DataSourceRequest request, Ins_ReportNotHaveSocialModel model)
        {
            var services = new Ins_InsuranceRecordServices();
            var result = services.GetReportNotHaveSocial(model.DateFrom, model.DateTo, model.OrgStructureID,UserLogin);
            var lstModel = new List<Ins_ReportNotHaveSocialModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportNotHaveSocialModel>();
                return Json(lstModel.ToDataSourceResult(request));
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        public ActionResult ExportNoHaveSocial([DataSourceRequest] DataSourceRequest request, Ins_ReportNotHaveSocialModel model)
        {
            var services = new Ins_InsuranceRecordServices();
            var result = services.GetReportNotHaveSocial(model.DateFrom, model.DateTo, model.OrgStructureID,UserLogin);
            var lstModel = new List<Ins_ReportNotHaveSocialModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportNotHaveSocialModel>();
                //  return Json(lstModel, JsonRequestBehavior.AllowGet);
            }
            var isDataTable = false;
            object obj = new Ins_ReportNotHaveSocialModel();

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
                    FileName = "Ins_ReportNotHaveSocialModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        public ActionResult ExportNotHaveSocialByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var profileServices = new Hre_ProfileServices();
            var baseService = new BaseService();
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);

            // var lstProfile = baseService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByListId, ref status);
            var strQuery = "exec hrm_hr_sp_get_ProfileById '" + strIDs + "'";
            var lstProfile = ExecuteQuery(strQuery);

            if (lstProfile == null)
                return null;
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            int i = 0;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfile.Rows.Count > 1 && lstProfile != null)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            DataTable tb = null;
            foreach (DataRow profile in lstProfile.Rows)
            {
                tb = lstProfile.Clone();
                tb.ImportRow(profile);

                //     profile.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                ActionService service = new ActionService(UserLogin);
                var exportService = new ActionService(UserLogin); //Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                List<object> lstObjExport = new List<object>();
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(1);
                lstObjExport.Add(10000000);

                template = exportService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
                if (template == null)
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                    //continue;
                }

                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau("Test") + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau("Test") + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilContract = new List<object>();
                ilContract.Add(tb);
                ExportService.ExportWord(outputPath, templatepath, tb);
            }
            if (lstProfile != null && lstProfile.Rows.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        #region Báo cáo theo dõi mức tiền đóng bảo hiểm hàng tháng xem theo khoảng thời gian
        [HttpPost]
        public JsonResult GetIns_ReportInsuranceTrackingMonthly([DataSourceRequest] DataSourceRequest request, Ins_ReportInsuranceTrackingMonthlySearchModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrSocialInsPlaceIDs))
                {
                    var socialInsPlaceIDs = model.StrSocialInsPlaceIDs.Split(',');
                    List<Guid> socailInsPlaceIDsTemp = new List<Guid>();
                    if (socialInsPlaceIDs.Length > 0)
                    {
                        foreach (var item in socialInsPlaceIDs)
                        {
                            Guid gTemp = new Guid();
                            gTemp = Guid.Empty;
                            Guid.TryParse(item, out gTemp);
                            if (gTemp != Guid.Empty)
                            {
                                socailInsPlaceIDsTemp.Add(gTemp);
                            }
                        }
                        if (socailInsPlaceIDsTemp.Any())
                        {
                            model.SocialInsPlaceIDs = socailInsPlaceIDsTemp;
                        }
                    }
                }


                var service = new Ins_InsuranceReportServices();
                var result = service.Ins_ReportInsuranceTrackingMonthlyLoadData(model.OrgStructureID, model.MonthYearFrom, model.MonthYearTo, model.IsProfileQuit, model.CodeEmp, model.SocialInsPlaceIDs,UserLogin);
                return Json(result.ToDataSourceResult(request));
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpPost]
        public ActionResult ExportIns_ReportInsuranceTrackingMonthly([DataSourceRequest] DataSourceRequest request, Ins_ReportInsuranceTrackingMonthlySearchModel model)
        {

            var services = new Ins_InsuranceReportServices();
            var result = services.Ins_ReportInsuranceTrackingMonthlyLoadData(model.OrgStructureID, model.MonthYearFrom, model.MonthYearTo, model.IsProfileQuit, model.CodeEmp, model.WorkPlaceIDs,UserLogin);
            var lstModel = new List<Ins_ReportInsuranceTrackingMonthlySearchModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportInsuranceTrackingMonthlySearchModel>();
                //  return Json(lstModel, JsonRequestBehavior.AllowGet);
            }
            var isDataTable = false;
            object obj = new Ins_ReportInsuranceTrackingMonthlySearchModel();

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
                    FileName = "Ins_ReportInsuranceTrackingMonthly",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId.HasValue && model.ExportId.Value != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId.Value, result, null, ExportFileType.Excel);
                return Json(fullPath);
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Ins_ReportInsuranceTrackingMonthlyValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportInsuranceTrackingMonthlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportInsuranceTrackingMonthlySearchModel>(model, "Ins_ReportInsuranceTrackingMonthly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }
        #endregion

        #region BC Đủ Điều Kiện Tham Gia Bảo Hiểm theo tháng

        [HttpPost]
        public ActionResult ReportJoinInsuranceMonthlyValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportJoinInsuranceMonthlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportJoinInsuranceMonthlySearchModel>(model, "Ins_ReportJoinInsuranceMonthly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }


        /// <summary> BC Đủ Điều Kiện Tham Gia Bảo Hiểm Theo Tháng </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ReportJoinInsuranceMonthly([DataSourceRequest] DataSourceRequest request, Ins_ReportJoinInsuranceMonthlyModel model)
        {
            var services = new Ins_InsuranceReportServices();
            List<HeaderInfo> listHeaderInfo = null;
            var result = services.ReportJoinInsuranceMonthly(model.OrgStructureID, model.MonthYear.Value, model.CodeEmp, model.SocialInsPlaceIDs,UserLogin);
            var lstModel = new List<Ins_ReportJoinInsuranceMonthlyModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportJoinInsuranceMonthlyModel>();
                lstModel = lstModel.OrderBy(p => p.STT).ToList();
            }
            var isDataTable = false;
            object obj = new Ins_ReportJoinInsuranceMonthlyModel();

            #region Header Info
            var codeEmp = string.Empty;
            if (model != null && model.UserID != Guid.Empty)
            {
                Hre_ProfileServices profileService = new Hre_ProfileServices();
                var profile = profileService.GetProfileInfo(model.UserID,UserLogin);
                if (profile != null && !string.IsNullOrEmpty(profile.CodeEmp))
                {
                    codeEmp = profile.CodeEmp;
                }
            }
            HeaderInfo headerInfoMonthYear = new HeaderInfo() { Name = "MonthYear", Value = model.MonthYear };
            HeaderInfo headerInfoCurrentDate = new HeaderInfo() { Name = "CurrentDate", Value = DateTime.Now };
            HeaderInfo headerInfoCodeEmp = new HeaderInfo() { Name = "CodeEmp", Value = codeEmp };
            listHeaderInfo = new List<HeaderInfo>() { headerInfoMonthYear, headerInfoCurrentDate, headerInfoCodeEmp };
            #endregion

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
                    FileName = "Ins_ReportJoinInsuranceMonthlySearchModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, listHeaderInfo, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        #region  BC Chức Danh Theo Tháng

        [HttpPost]
        public ActionResult ReportJobNameMonthlyValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportJobNameMonthlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportJobNameMonthlySearchModel>(model, "Ins_ReportJobnameMonthly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }


        /// <summary> BC Chức Danh Theo Tháng </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ReportJobNameMonthly([DataSourceRequest] DataSourceRequest request, Ins_ReportJobNameMonthlyModel model)
        {
            var services = new Ins_InsuranceReportServices();
            var result = services.ReportJobNameMonthly(model.OrgStructureID, model.MonthYear.Value, model.CodeEmp, model.SocialInsPlaceIDs,UserLogin);
            var lstModel = new List<Ins_ReportJobNameMonthlyModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportJobNameMonthlyModel>();
            }
            var isDataTable = false;
            object obj = new Ins_ReportJobNameMonthlyModel();

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
                    FileName = "Ins_ReportJobNameMonthlyModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        #endregion

        #region Đuôi D02

        [HttpPost]
        public ActionResult ReportD02TailValidate([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD02TailSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_InsuranceReportD02TailSearchModel>(model, "Ins_ReportD02TSTail", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        /// <summary> BC Đuôi D02 </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ReportD02Tail([DataSourceRequest] DataSourceRequest request, Ins_InsuranceReportD02TailModel model)
        {
            var services = new Ins_InsuranceReportServices();
            var result = services.ReportD02Tail(model.OrgStructureID, model.MonthYear.Value,UserLogin);
            var lstModel = new List<Ins_InsuranceReportD02TailModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_InsuranceReportD02TailModel>().OrderBy(p => p.STT).ToList();
            }
            var isDataTable = false;
            object obj = new Ins_InsuranceReportD02TailModel();

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
                    FileName = "Ins_InsuranceReportD02TailModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }


        #endregion

        #endregion

        #region BC Dữ Liệu BH NV Theo Tháng (Group Theo CostCenter) (form kế toán)

        [HttpPost]
        public ActionResult ReportInsCostCenterMonthlyValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportInsCostCenterMonthlySearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportInsCostCenterMonthlySearchModel>(model, "Ins_ReportInsCostCenterMonthly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        /// <summary>  BC Dữ Liệu BH NV Theo Tháng (Group Theo CostCenter) (form kế toán) </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ReportInsCostCenterMonthly([DataSourceRequest] DataSourceRequest request, Ins_ReportInsCostCenterMonthlyModel model)
        {
            var services = new Ins_InsuranceReportServices();
            var result = services.ReportInsCostCenterMonthly(model.OrgStructureID, model.MonthYear.Value, model.CostCentreName, model.SocialInsPlaceIDs,UserLogin);
            var lstModel = new List<Ins_ReportInsCostCenterMonthlyModel>();
            if (result != null)
            {
                lstModel = result.Translate<Ins_ReportInsCostCenterMonthlyModel>();
            }
            var isDataTable = false;
            object obj = new Ins_ReportInsCostCenterMonthlyModel();

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
                    FileName = "Ins_ReportInsCostCenterMonthlySearchModel",
                    OutPutPath = path,
                    DownloadPath = "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstModel, null, model.ExportType);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(lstModel.ToDataSourceResult(request));
        }

        #endregion

        #endregion

        #region C70AReport
        [HttpPost]
        public ActionResult GetReportC70AValidate([DataSourceRequest] DataSourceRequest request, Ins_SearchC70AReportModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Ins_SearchC70AReportModel>(model, "Ins_ReportC70A", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);

        }

        /// <summary>
        /// [Hieu.Van] 
        /// Báo Cáo C70A
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportC70A([DataSourceRequest] DataSourceRequest request, Ins_SearchC70AReportModel model)
        {
            var isDataTable = false;
            object obj = new Ins_C70aReportEntity();
            var service = new Ins_InsuranceReportServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };

            var result = service.GetReportC70A(model.DateMonth, model.DateStart, model.DateEnd, model.OrgStructureID, model.CutOffDurationName, model.IsCreateTemplate);
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
                    FileName = "Ins_C70aReportModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        if (Enum.IsDefined(typeof(InsuranceRecordType), row[Ins_C70aReportModel.FieldNames.Status].ToString()))
                        {
                            row[Ins_C70aReportModel.FieldNames.Status] = row[Ins_C70aReportModel.FieldNames.Status].ToString().TranslateString();
                        }
                        if (Enum.IsDefined(typeof(InsuranceRecordType), row[Ins_C70aReportModel.FieldNames.CodeEmp].ToString()))
                        {
                            row[Ins_C70aReportModel.FieldNames.CodeEmp] = row[Ins_C70aReportModel.FieldNames.CodeEmp].ToString().TranslateString();
                        }
                        if (Enum.IsDefined(typeof(InsuranceRecordType), row[Ins_C70aReportModel.FieldNames.GroupName].ToString()))
                        {
                            row[Ins_C70aReportModel.FieldNames.GroupName] = row[Ins_C70aReportModel.FieldNames.GroupName].ToString().TranslateString();
                        }
                    }
                }

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }

            #region mapping dataTable to dataList
            //List<Ins_C70aReportModel> dataList = new List<Ins_C70aReportModel>();
            //Ins_C70aReportModel aTSource = null;
            //if (result.Rows.Count > 0)
            //{
            //    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            //    var objFieldNames = (from PropertyInfo aProp in typeof(Ins_C70aReportModel).GetProperties(flags)
            //                         select new
            //                         {
            //                             Name = aProp.Name,
            //                             Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
            //                         }).ToList();
            //    var dataTblFieldNames = (from DataColumn aHeader in result.Columns
            //                             select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
            //    var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
            //    foreach (DataRow dataRow in result.AsEnumerable().ToList())
            //    {
            //        aTSource = new Ins_C70aReportModel();
            //        foreach (var aField in commonFields)
            //        {
            //            PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
            //            if (dataRow[aField.Name] == DBNull.Value)
            //                continue;
            //            propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
            //        }
            //        dataList.Add(aTSource);
            //    }
            //}
            #endregion

            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region BC Tong Hop Thanh Toan Bao Hiem
        public ActionResult GetReportInsuranceList([DataSourceRequest] DataSourceRequest request, Ins_ReportInsuranceTotalSearchModel model)
        {
            string status = string.Empty;
            //var isDataTable = false;
            var service = new BaseService();
            //object obj = new Hre_ProfileModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            //    List<Hre_ProfileEntity> listProfile = service.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_sal_sp_getdata_Profile, ref status);
            var ReportServices = new Ins_InsuranceReportServices();
            DateTime _DateFrom = DateTime.MinValue;
            DateTime _DateTo = DateTime.MaxValue;
            if (model != null && model.DateFrom != null)
            {
                _DateFrom = model.DateFrom.Value;
            }
            if (model != null && model.DateTo != null)
            {
                _DateTo = model.DateTo.Value;
            }
            var result = ReportServices.BC_INSURANCETOTAL(_DateFrom, _DateTo, model.OrgStructureIDs, model.SocialInsPlaceIDs, UserLogin);
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
                    FileName = "Ins_ReportInsuranceTotalEntity",
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
            //return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
            #region mapping dataTable to dataList
            List<Ins_ReportInsuranceTotalModel> dataList = new List<Ins_ReportInsuranceTotalModel>();
            Ins_ReportInsuranceTotalModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Ins_ReportInsuranceTotalModel).GetProperties(flags)
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
                    aTSource = new Ins_ReportInsuranceTotalModel();
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
        #endregion

        #region BC DS NV Huong tro cap nang nhoc, doc hai, nguy hiem
        public ActionResult ReportEmpHardJobListValidate([DataSourceRequest] DataSourceRequest request, Ins_ReportEmpHardJobSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_ReportEmpHardJobSearchModel>(model, "Ins_ReportEmpHardJob", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }


        public ActionResult GetReportEmpHardJobList([DataSourceRequest] DataSourceRequest request, Ins_ReportEmpHardJobSearchModel model)
        {
            string status = string.Empty;
            var ReportServices = new Ins_InsuranceReportServices();
            DateTime _DateFrom = DateTime.MinValue;
            DateTime _DateTo = DateTime.MaxValue;
            if (model != null && model.DateFrom != null)
            {
                _DateFrom = model.DateFrom.Value;
            }
            if (model != null && model.DateTo != null)
            {
                _DateTo = model.DateTo.Value;
            }
            var result = ReportServices.BC_EmpHardJob(_DateFrom, _DateTo, model.OrgStructureID);
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
                    FileName = "Ins_ReportEmpHardJoblEntity",
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
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
        }
        #endregion

        #region Insurance Salary PayBack - Truy Lĩnh Bảo Hiểm
        /// <summary>Ds Truy Lĩnh Bảo Hiểm</summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetInsuranceSalaryPaybackList([DataSourceRequest] DataSourceRequest request, Ins_InsuranceSalaryPaybackSearchModel model)
        {
            return GetListDataAndReturn<Ins_InsuranceSalaryPaybackModel, Ins_InsuranceSalaryPaybackEntity, Ins_InsuranceSalaryPaybackSearchModel>(request, model, ConstantSql.hrm_ins_sp_get_SalPayBack);
        }

        public JsonResult GetMultiTypeD02TypeCode(string text)
        {
            string status = string.Empty;
            var service = new InsuranceServices();
            var get = service.GetTypeD02TypeNameList(text);
            if (get != null)
            {
                var result = get.Select(item => new Ins_TypeD02MultiEntity()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }


        public JsonResult GetMultiTypeD02StatusCode(string text)
        {
            string status = string.Empty;
            var service = new InsuranceServices();
            var get = service.GetTypeD02StatusNameList(text);
            if (get != null)
            {
                var result = get.Select(item => new Ins_TypeD02MultiEntity()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public JsonResult GetMultiTypeD02CommentCode(string text)
        {
            string status = string.Empty;
            var service = new InsuranceServices();
            var get = service.GetTypeD02CommentList(text);
            if (get != null)
            {
                var result = get.Select(item => new Ins_TypeD02MultiEntity()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        /// <summary>validate Truy Lĩnh Bảo Hiểm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsuranceSalaryPaybackValidate([DataSourceRequest] DataSourceRequest request, Ins_InsuranceSalaryPaybackSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Ins_InsuranceSalaryPaybackSearchModel>(model, "Ins_InsuranceSalaryPayback", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }

            #endregion
            return Json(message);
        }
        #region MyRegion
        [HttpPost]
        public ActionResult CalculateInsurancePayBack(List<Guid> payBackIds,DateTime monthYear)
        {
            var messageReturn = "Success";
            var insService = new InsuranceServices();
            insService.CalculateInsurancePayBack(payBackIds, monthYear, UserLogin);
            return Json(messageReturn);
        }

        [HttpPost]
        public ActionResult GetProfileMonthly(Guid profileId, DateTime fromMonthEffect)
        {
            var insService = new InsuranceServices();
            var profileMonthly = insService.GetProfileMonthly(profileId, fromMonthEffect);
            var result = new { AmoutHDTIns = 0.0, InsSalary = 0.0, JobName = string.Empty, SocialInsPlaceID = Guid.Empty, IsSocialInsurance = false, IsHealthInsurance = false, IsUnEmpInsurance = false };
            if (profileMonthly != null)
            {
                result = new {
                    AmoutHDTIns = profileMonthly.AmountHDTIns ?? 0,
                    InsSalary = profileMonthly.SalaryInsurance ?? 0,
                    JobName=profileMonthly.JobName ,
                    SocialInsPlaceID = profileMonthly.SocialInsPlaceID??Guid.Empty,
                    IsSocialInsurance = profileMonthly.IsSocialInsurance??false,
                    IsHealthInsurance = profileMonthly.IsHealthInsurance??false,
                    IsUnEmpInsurance = profileMonthly.IsUnEmpInsurance??false                
                };
            }                
            return Json(result);
        }
        
        #endregion

        #endregion
    }

}