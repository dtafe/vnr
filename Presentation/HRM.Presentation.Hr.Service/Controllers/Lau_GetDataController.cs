using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Laundry.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Laundry.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using HRM.Business.Laundry.Models;
using System.Data.SqlTypes;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.HrmSystem.Models;
using System.Threading.Tasks;
namespace HRM.Presentation.Hr.Service.Controllers
{
    /// <summary>
    /// Hỗ trợ các hàm lấy dữ liệu cho các control như: dropdownlist, combobox, mutiselect....
    /// </summary>
    public class Lau_GetDataController : BaseController
    {
        string status = string.Empty;
        #region Get Multi



        public JsonResult GetMultiLocker(string text)
        {
            return GetDataForControl<Lau_LockerMultiModel, Lau_LockerMultiEntity>(text, ConstantSql.hrm_lau_sp_get_Locker_multi);
        }

        public JsonResult GetMultiLine(string text)
        {
            return GetDataForControl<Lau_LineMultiModel, Lau_LineMultiEntity>(text, ConstantSql.hrm_lau_sp_get_Line_multi);
        }

        public JsonResult GetMultiMarker(string text)
        {
            return GetDataForControl<Lau_MarkerMultiModel, Lau_MarkerMultiEntity>(text, ConstantSql.hrm_lau_sp_get_Marker_multi);
        }

        public JsonResult GetMultiMachineOfLine(string text)
        {
            return GetDataForControl<Lau_MachineOfLineMultiModel, Lau_MachineOfLineMultiEntity>(text, ConstantSql.hrm_lau_sp_get_MachineOfLine_multi);
        }

        public ActionResult GetLocationList([DataSourceRequest] DataSourceRequest request, LMS_LocationLMSSearchModel LocationModel)
        {
            var modelSearch = new LMS_LocationLMSSearchModel();
            modelSearch.Code = LocationModel.Code;
            modelSearch.LocationLMSName = LocationModel.LocationLMSName;
            return GetListDataAndReturn<LMS_LocationLMSModel, LMS_LocationLMSEntity, LMS_LocationLMSSearchModel>(request, modelSearch, ConstantSql.hrm_lau_sp_get_Location);
        }

        #endregion

        /// <summary>
        /// 
        /// Lấy dữ liệu Provider load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauProviderModel"></param>
        /// <returns></returns>

        /// <summary>
        /// 
        /// Lấy dữ liệu Reader load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauReaderModel"></param>
        /// <returns></returns>



        /// <summary>
        /// 
        /// Lấy dữ liệu Tủ đồ load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLockerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLockerList([DataSourceRequest] DataSourceRequest request, Lau_LockerSearchModel lauLockerModel)
        {
            return GetListDataAndReturn<Lau_LockerModel, LMS_LockerLMSEntity, Lau_LockerSearchModel>(request, lauLockerModel, ConstantSql.hrm_lau_sp_get_Locker);
        }




        /// <summary>
        /// 
        /// Lấy dữ liệu Tủ đồ load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLockerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMachineOfLineList([DataSourceRequest] DataSourceRequest request, Lau_MachineOfLineSearchModel model)
        {
            return GetListDataAndReturn<Lau_MachineOfLineModel, Lau_MachineOfLineEntity, Lau_MachineOfLineSearchModel>(request, model, ConstantSql.hrm_lau_sp_get_MachineOfLine);
        }


        /// <summary>
        /// 
        /// Lấy dữ liệu Tủ đồ load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLockerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLaundryRecordList([DataSourceRequest] DataSourceRequest request, Lau_LaundryRecordSearchNewModel model)
        {
            if (model.DateFrom != null)
            {
                model.DateFrom = model.DateFrom.Value.Date;
            }
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddMilliseconds(-1);
            }
            if (model.ProfileIDs != null)
            {
                model.ProfileIDs = Common.DotNetToOracle(model.ProfileIDs);
            }
            return GetListDataAndReturn<Lau_LaundryRecordModel, LMS_LaundryRecordEntity, Lau_LaundryRecordSearchNewModel>(request, model, ConstantSql.hrm_lau_sp_get_LaundryRecord);
        }
        /// <summary>
        /// [Tam.Le] - 8.4.2014 - Lấy dữ liệu Marker theo Line Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GetMarkerCascading(int lineId)
        {
            var baseServices = new ActionService(UserLogin);
            var result = new List<Lau_MarkerModel>();
            string status = string.Empty;
            if (lineId > 0)
            {
                var service = new Lau_MarkerServices();
                result = baseServices.GetData<Lau_MarkerModel>(lineId, ConstantSql.hrm_lau_sp_get_MarkerByLineId, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// [Tam.Le] - 8.4.2014 - Lấy dữ liệu Locker theo Line Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GetLockerCascading(int lineId)
        {
            var baseServices = new ActionService(UserLogin);
            var result = new List<Lau_LockerModel>();
            string status = string.Empty;
            if (lineId > 0)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Lau_LockerServices();
                result = baseServices.GetData<Lau_LockerModel>(lineId, ConstantSql.hrm_lau_sp_get_LockerByLineId, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// [Tam.Le] - Lấy dữ liệu Lau_Line theo Id trả về cho hàm ajax nhận model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetLineById(Guid id)
        {
            string status = string.Empty;
            var model = new Lau_LineModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<LMS_LineLMSEntity>(id, ConstantSql.hrm_lau_sp_get_LineById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Lau_LineModel>();
            }
            //model.ActionStatus = status;
            //return Json(model, JsonRequestBehavior.AllowGet);
            return Json(model);
        }
        /// <summary>
        /// [Tam.Le] - Xuất danh sách dữ liệu cho LaudryRecord theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportLaudryRecordList([DataSourceRequest] DataSourceRequest request, Lau_LaundryRecordSearchNewModel model)
        {
            var listModel = GetListData<Lau_LaundryRecordModel, LMS_LaundryRecordEntity, Lau_LaundryRecordSearchNewModel>(request, model, ConstantSql.hrm_lau_sp_get_LaundryRecord, ref status);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.TimeLog, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.Hour, ConstantFormat.HRM_Format_Grid_ShortTime);
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.Amount, ConstantFormat.HRM_Format_Money);
            status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','), formatFields);
            return Json(status);
            //return ExportAllAndReturn<Lau_LaundryRecordEntity, Lau_LaundryRecordModel, Lau_LaundryRecordSearchNewModel>(request, model, ConstantSql.hrm_lau_sp_get_LaundryRecord);
        }

        /// <summary>
        /// [Tam.Le] - Xuất các dòng dữ liệu được chọn của LaudryRecord ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportLaudryRecordSelected(string selectedIds, string valueFields)
        {
            selectedIds = Common.DotNetToOracle(selectedIds);
            var listModel = GetData<Lau_LaundryRecordModel, LMS_LaundryRecordEntity>(selectedIds, ConstantSql.hrm_lau_sp_get_LaundryRecord_ByIds);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.TimeLog, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.Hour, ConstantFormat.HRM_Format_Grid_ShortTime);
            formatFields.Add(Lau_LaundryRecordModel.FieldNames.Amount, ConstantFormat.HRM_Format_Money);
            status = ExportService.Export(listModel, valueFields.Split(','), formatFields);
            return Json(status);
            // return ExportSelectedAndReturn<Lau_LaundryRecordEntity, Lau_LaundryRecordModel>(selectedIds, valueFields, ConstantSql.hrm_lau_sp_get_LaundryRecord_ByIds);
        }
        /// <summary>
        /// 
        /// Lấy dữ liệu Tủ đồ load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLockerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMarkerList([DataSourceRequest] DataSourceRequest request, Lau_MarkerSearchModel model)
        {
            return GetListDataAndReturn<Lau_MarkerModel, LMS_MarkerEntity, Lau_MarkerSearchModel>(request, model, ConstantSql.hrm_lau_sp_get_Marker);
        }

        /// <summary>
        /// 
        /// Lấy dữ liệu Cửa nhận giặt load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLineList([DataSourceRequest] DataSourceRequest request, Lau_LineSearchModel model)
        {
            return GetListDataAndReturn<Lau_LineModel, LMS_LineLMSEntity, Lau_LineSearchModel>(request, model, ConstantSql.hrm_lau_sp_get_Line);
        }

        [HttpPost]
        public ActionResult GetTAMData([DataSourceRequest] DataSourceRequest request, Lau_TamScanLogModel model)
        {
            var service = new Lau_TamServices();
            List<Guid> listOrgStructureID = new List<Guid>();

            DateTime dateFrom = DateTime.Now.Date;
            DateTime dateTo = DateTime.Now.Date;

            if (model.DateFrom.HasValue)
            {
                dateFrom = model.DateFrom.Value;
            }

            if (model.DateTo.HasValue)
            {
                dateTo = model.DateTo.Value;
            }

            model.AsynTaskID = service.CreateComputingTask(model.UserID,
                dateFrom, dateTo);

            if (model.ProfileID != null && model.ProfileID.Count > 0)
            {
                Task task = Task.Run(() => service.SyncTAMLog(model.UserID,
                    model.AsynTaskID, false, dateFrom, dateTo, model.ProfileID));
            }
            else
            {
                if (model.OrgStructureID != null)
                {
                    listOrgStructureID = model.OrgStructureID.Where(d => d.HasValue).Select(d => d.Value).ToList();
                }

                Task task = Task.Run(() => service.SyncTAMLog(model.UserID, model.AsynTaskID,
                    false, dateFrom, dateTo, listOrgStructureID, null, null));
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportLau_TamScanLogList([DataSourceRequest] DataSourceRequest request, Lau_TamScanLogModel model)
        {
            string statusExport = string.Empty;
            return Json(statusExport);
        }
        /// <summary>
        /// [Hieu.Van] Tồng hợp dữ liệu ăn
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetLaundryRecordSummary([DataSourceRequest] DataSourceRequest request, Lau_LaundryRecordSearchModel model)
        {
            Lau_LaundryRecordServices LauService = new Lau_LaundryRecordServices();
            var baseService = new BaseService();
            var Actionservices = new ActionService(UserLogin);

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

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
            var baseServices = new ActionService(UserLogin);
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

                    lstProfileIDs = Actionservices.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = Actionservices.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            }
            #endregion

            // Hàm xử lý tổng hợp
            var result = LauService.GetLaundryRecordSummary(model.LineID, model.LockerID, model.MarkerID, dateStart, dateEnd, lstProfileIDs, UserLogin).ToList().Translate<Lau_LaundryRecordModel>();
            if (result != null)
            {
                var submit = ConstantDisplay.HRM_Enum_Submit.TranslateString();
                var auto = ConstantDisplay.HRM_Enum_Auto.TranslateString();
                result.Where(s => s.Status == LaundryRecordStatus.E_SUBMIT.ToString()).Select(s => s.Status = submit).ToList();
                result.Where(s => s.Type == LaundryRecordType.E_AUTO.ToString()).Select(s => s.Type = auto).ToList();
            }
            #region xử lý xuất áo cáo
            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            if (model.selectedIDs != null)
            {
                var strSelect = model.selectedIDs.Split(',');
                List<Guid> lstSelect = new List<Guid>();
                foreach (var item in strSelect)
                {
                    lstSelect.Add(new Guid(item));
                }
                var resultSelect = result.Where(m => lstSelect.Contains(m.ID)).ToList();
                var fullPath = ExportService.Export(resultSelect, model.ValueFields.Split(','));
                return Json(fullPath);
            }
            #endregion

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(result.ToDataSourceResult(request));
        }


        /// <summary>
        /// [Hieu.Van]
        /// 3.5.1	Báo cáo tổng hợp số lượng giặt là 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetReportSummaryLaundryRecord([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();

            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();
            var baseService = new ActionService(UserLogin);
            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                // _temp = lst.Select(new Guid()).ToList();
               
                if (model.OrgStructureID != null)
                {

                    lstProfileIDs = baseService.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = baseService.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion

            //Hàm xừ lý báo cáo
            var result = service.GetReportSummaryLaundryRecord(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, isIncludeQuitEmp, model.CodeEmp, UserLogin).Translate<Lau_ReportSummaryLaundryRecordModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result);
                return Json(fullPath);
            }

            #endregion

            //request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van]
        /// 3.5.3	Báo cáo Số lượng quần áo giặt 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetReportClothing([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            BaseService baseService = new BaseService();
            var Actionservices = new ActionService(UserLogin);
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;
            List<Guid> lstShift = new List<Guid>();
            if (model.ShiftID != null)
            {
                var lst = model.ShiftID.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstShift.Add(_Id);
                }

            }

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();

            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                //_temp = lst.Select(new Guid()).ToList();

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion

            //Hàm xừ lý báo cáo
            var result = service
                .GetReportClothing(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, lstShift, isIncludeQuitEmp, UserLogin)
                .Translate<Lau_ReportClothingModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result);
                return Json(fullPath);
            }

            #endregion

            //request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hieu.Van]
        /// 3.5.3	Báo cáo Số lượng quần áo giặt 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetReportSummaryClothing([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            BaseService baseService = new BaseService();
            var baseServices = new ActionService(UserLogin);
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;
            List<Guid> lstShift = new List<Guid>();
            if (model.ShiftID != null)
            {
                var lst = model.ShiftID.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstShift.Add(_Id);
                }

            }

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();

            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                //_temp = lst.Select(int.Parse).ToList();

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion

            //Hàm xừ lý báo cáo
            var result = service
                .GetReportSummaryClothing(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, lstShift, isIncludeQuitEmp, UserLogin);

            var rs = result.Translate<Lau_ReportSummaryClothingModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, rs);
                return Json(fullPath);
            }

            #endregion

            var dataSourceResult = rs.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }

        //[HttpPost]
        public ActionResult GetReportSummaryExceptClothing([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            BaseService baseService = new BaseService();
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;
            bool isViewOverProfile = model.isViewOverProfile ? true : false;
            List<Guid> lstShift = new List<Guid>();
            if (model.ShiftID != null)
            {
                var lst = model.ShiftID.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstShift.Add(_Id);
                }
            }

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();
            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion
            var baseServices = new ActionService(UserLogin);
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                //_temp = lst.Select(int.Parse).ToList();

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion


            var data = service.GetReportSummaryExceptClothing(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, lstShift, isIncludeQuitEmp, isViewOverProfile, UserLogin);
            var result = data.Translate<Lau_ReportSummaryExceptClothingModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result);
                return Json(fullPath);
            }

            #endregion

            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(result.ToDataSourceResult(request));
        }



        /// <summary>
        /// [Hieu.Van]
        /// 3.5.3	Báo cáo Số lượng quần áo giặt 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportEmpDetail([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            BaseService baseService = new BaseService();
            var Actionservices = new ActionService(UserLogin);
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();
            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                //_temp = lst.Select(int.Parse).ToList();

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion

            var result = service.GetReportEmpDetail(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, isIncludeQuitEmp, model.CodeEmp, UserLogin);
            var rs = result.Translate<Lau_ReportEmpDetailModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, rs);
                return Json(fullPath);
            }

            #endregion

            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }

        /// <summary>
        /// Báo cáo trừ tiền giặt
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lauLineModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetReportExceptClothing([DataSourceRequest] DataSourceRequest request, Lau_ReportSearchModel model)
        {
            Lau_ReportServices service = new Lau_ReportServices();
            BaseService baseService = new BaseService();
            bool isIncludeQuitEmp = model.isIncludeQuitEmp ? true : false;
            List<Guid> lstShift = new List<Guid>();
            if (model.ShiftID != null)
            {
                var lst = model.ShiftID.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    lstShift.Add(_Id);
                }

            }

            #region xử lý dateStart - dateEnd
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }
            #endregion

            #region xử lý line - locker - marker

            List<Guid?> lstLine = new List<Guid?>();
            List<Guid?> lstLocker = new List<Guid?>();
            List<Guid?> lstMarker = new List<Guid?>();
            if (model.LineID != null)
            {
                foreach (var line in model.LineID.Split(','))
                {
                    lstLine.Add(new Guid(line));
                }
            }
            if (model.LockerID != null)
            {
                foreach (var locker in model.LockerID.Split(','))
                {
                    lstLocker.Add(new Guid(locker));
                }
            }
            if (model.MarkerID != null)
            {
                foreach (var marker in model.MarkerID.Split(','))
                {
                    lstMarker.Add(new Guid(marker));
                }
            }

            #endregion
            var baseServices = new ActionService(UserLogin);
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();

            if (model.ProfileIDs != null)
            {
                var lst = model.ProfileIDs.Split(',');
                foreach (var item in lst)
                {
                    Guid _Id = new Guid(item);
                    _temp.Add(_Id);
                }

                //_temp = lst.Select(int.Parse).ToList();

                if (model.OrgStructureID != null)
                {
                    lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
                    _profileIDs = lstProfileIDs.Where(m => !_temp.Contains(m)).ToList();
                    lstProfileIDs.AddRange(_profileIDs);
                }
                else
                {
                    lstProfileIDs = _temp;
                }
            }
            else
            {
                lstProfileIDs = baseServices.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(m => m.ID).ToList();
            }
            #endregion

            var result = service.GetReportExceptClothing(lstLine, lstLocker, lstMarker, lstProfileIDs, dateStart, dateEnd, lstShift, isIncludeQuitEmp, UserLogin);
            var rs = result.Translate<Lau_ReportExceptClothingModel>();

            #region xử lý xuất báo cáo

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, rs);
                return Json(fullPath);
            }

            #endregion

            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Tam.Le 8.1.2014 Modify]
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CheckConnectTAM()
        {
            var service = new Lau_TamServices();
            string message = "";
            bool status = false;

            bool isconnect = service.IsConnected(AppConfig.HRM_SYS_LAU_TAMSCANLOG_1_, ref message);

            if (isconnect == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var sys_AllSetting = new Sys_AllSettingEntity();
            sys_AllSetting = LibraryService.GetSys_AllSettingByKey(Constant.HRM_SYS_COMPUTE_TAMLOG_LAU);
            string result = string.Empty;
            string DateFrom = string.Empty;
            string DateTo = string.Empty;

            if (sys_AllSetting != null)
            {
                DateFrom = sys_AllSetting.Value2 != null ? sys_AllSetting.Value2.ToString() : string.Empty;
                DateTo = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString().Split(' ')[1] + " " + DateTime.Now.ToString().Split(' ')[2];
            }

            result = status.ToString() + "|" + DateFrom + "|" + DateTo;
            return result;
        }

        public ActionResult ExportLauLine([DataSourceRequest] DataSourceRequest request, Lau_LineSearchModel model)
        {
            return ExportAllAndReturn<Lau_LineModel, LMS_LineLMSEntity, Lau_LineSearchModel>(request, model, ConstantSql.hrm_lau_sp_get_Line);
        }
    }
}