using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Canteen.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Business.Canteen.Models;
using ListQueryModel = HRM.Infrastructure.Utilities.ListQueryModel;
using System;
using HRM.Business.Hr.Domain;
using System.Data.SqlTypes;
//using HRM.Business.Hr.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Presentation.Category.Models;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities.Helpers;
using HRM.Business.Attendance.Models;
using System.Threading.Tasks;


namespace HRM.Presentation.Canteen.Service.Controllers
{
    /// <summary>
    /// Hỗ trợ các hàm lấy dữ liệu cho các control như: dropdownlist, combobox, mutiselect....
    /// </summary>
    public class Canteen_GetDataController : BaseController
    {
        string status = string.Empty;
        #region Multi

        public JsonResult GetMultiLocation(string text)
        {
            return GetDataForControl<Can_LocationMultiModel, Can_LocationMultiEntity>(text, ConstantSql.hrm_can_sp_get_Location_multi);
        }
        public JsonResult GetMultiCatering(string text)
        {
            return GetDataForControl<Can_CateringMultiModel, Can_CateringMultiEntity>(text, ConstantSql.hrm_can_sp_get_Catering_multi);
        }
        public JsonResult GetMultiCanteen(string text)
        {
            return GetDataForControl<Can_CanteenMultiModel, Can_CanteenMultiEntity>(text, ConstantSql.hrm_can_sp_get_Canteen_multi);
        }
        public JsonResult GetMultiLine(string text)
        {
            return GetDataForControl<Can_LineMultiModel, Can_LineMultiEntity>(text, ConstantSql.hrm_can_sp_get_Line_multi);
        }

        public JsonResult GetMultiMachineOfLine(string text)
        {
            return GetDataForControl<Can_MachineOfLineMultiModel, Can_MachineOfLineMultiEntity>(text, ConstantSql.hrm_can_sp_get_MachineOfLine_multi);
        }

        public JsonResult GetMultiMealAllowanceTypeSetting(string text)
        {

            return GetDataForControl<Can_MealAllowanceTypeSettingMultiModel, Can_MealAllowanceTypeSettingMultiEntity>(text, ConstantSql.hrm_can_sp_get_MealAllowanceTypeSetting_Multi);
        }

        public ActionResult GetCan_SumryMealRecord([DataSourceRequest] DataSourceRequest request, Can_SumryMealRecordSearchModel model)
        {
            ActionService action = new ActionService(UserLogin);
            string status = string.Empty;
            var Cutoffduration = action.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutoffdurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            model.DateFrom = Cutoffduration.DateStart;
            model.DateTo = Cutoffduration.DateEnd;

            return GetListDataAndReturn<Can_SumryMealRecordModel, Can_SumryMealRecordEntity, Can_SumryMealRecordSearchModel>(request, model, ConstantSql.hrm_can_sp_get_SumMealRecord);
        }
        public ActionResult ComputeSumryMealRecord(Guid CutoffdurationID, string OrgStructureID)
        {
            BaseService baseservices = new BaseService();
            var Actionservices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructureID;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = Actionservices.GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, ref status);

            Can_ReportServices CanteenServices = new Can_ReportServices();
            bool result = CanteenServices.SaveSumryMealRecord(CutoffdurationID, ListProfile.Select(m => m.ID).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// Lấy tất cả danh sách máy quẹt thẻ bởi store [hrm_can_get_MachineOfLine]
        /// </summary>
        /// <returns></returns>     
        public ActionResult GetMachineOfLineList([DataSourceRequest] DataSourceRequest request, Can_MachineOfLineSearchModel machineOfLineModel)
        {
            var modelSearch = new Can_MachineOfLineSearchModel();
            modelSearch.MachineOfLineMachineCode = machineOfLineModel.MachineOfLineMachineCode;
            modelSearch.LineID = machineOfLineModel.LineID;
            modelSearch.LineID = modelSearch.LineID == null ? null : modelSearch.LineID;
            modelSearch.DateFrom = machineOfLineModel.DateFrom;
            modelSearch.DateTo = machineOfLineModel.DateTo;
            return GetListDataAndReturn<Can_MachineOfLineModel, Can_MachineOfLineEntity, Can_MachineOfLineSearchModel>(request, modelSearch, ConstantSql.hrm_can_get_MachineOfLine);
        }
        /// <summary>
        /// Lấy tất cả danh sách căn tin bởi store [hrm_cat_sp_get_Canteen]
        /// </summary>
        /// <returns></returns>  
        public ActionResult GetCanteenList([DataSourceRequest] DataSourceRequest request, Can_CanteenSearchModel CanteenModel)
        {
            var modelSearch = new Can_CanteenSearchModel();
            modelSearch.CanteenName = CanteenModel.CanteenName;
            modelSearch.CanteenCode = CanteenModel.CanteenCode;
            modelSearch.LocationID = CanteenModel.LocationID;
            modelSearch.LocationID = modelSearch.LocationID == null ? null : modelSearch.LocationID;
            return GetListDataAndReturn<Can_CanteenModel, Can_CanteenEntity, Can_CanteenSearchModel>(request, modelSearch, ConstantSql.hrm_can_sp_get_Canteen);
        }
        public ActionResult GetCateringList([DataSourceRequest] DataSourceRequest request, Can_CateringSearchModel CateringModel)
        {
            var modelSearch = new Can_CateringSearchModel();
            modelSearch.CateringName = CateringModel.CateringName;
            modelSearch.CateringCode = CateringModel.CateringCode;
            return GetListDataAndReturn<Can_CateringModel, Can_CateringEntity, Can_CateringSearchModel>(request, modelSearch, ConstantSql.hrm_can_sp_get_Catering);
        }
        public ActionResult GetLineList([DataSourceRequest] DataSourceRequest request, Can_LineSearchModel LineModel)
        {
            var modelSearch = new Can_LineSearchModel();
            modelSearch.CanteenID = LineModel.CanteenID;
            modelSearch.CateringID = LineModel.CateringID;
            modelSearch.LineCode = LineModel.LineCode;
            modelSearch.LineName = LineModel.LineName;
            return GetListDataAndReturn<Can_LineModel, Can_LineEntity, Can_LineSearchModel>(request, modelSearch, ConstantSql.hrm_can_sp_get_Line);
        }
        public ActionResult GetLocationList([DataSourceRequest] DataSourceRequest request, Can_LocationSearchModel LocationModel)
        {
            var modelSearch = new Can_LocationSearchModel();
            modelSearch.LocationName = LocationModel.LocationName;
            modelSearch.LocationCode = LocationModel.LocationCode;
            return GetListDataAndReturn<Can_LocationModel, Can_LocationEntity, Can_LocationSearchModel>(request, modelSearch, ConstantSql.hrm_can_sp_get_Location);
        }
        public ActionResult GetMealAllowanceTypeSettingList([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceTypeSettingSearchModel modelSearch)
        {
            return GetListDataAndReturn<Can_MealAllowanceTypeSettingModel, Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingSearchModel>(request, modelSearch, ConstantSql.hrm_can_get_MealAllowanceType);
        }

        //[Quan.Nguyen] Lay Du Lieu Theo Store Man Hinh DS Đơn Giá Chuẩn [Can_MealPriceTypeSetting]
        public ActionResult GetMealPriceTypeSettingList([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceTypeSettingSearchModel modelSearch)
        {
            return GetListDataAndReturn<Can_MealAllowanceTypeSettingModel, Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingSearchModel>(request, modelSearch, ConstantSql.hrm_can_get_MealPriceType);
        }



        /// <summary>
        /// [Hieu.Van] Lấy toàn bộ dữ liệu theop store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mealModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMealRecordList([DataSourceRequest] DataSourceRequest request, Can_MealRecordSearchModel model)
        {
            BaseService baseService = new BaseService();
            string status = string.Empty;

            #region xử lý tìm kiếm từ 6h30
            DateTime dateConfig = DateTime.MinValue;
            var rsTimeConfig = baseService.GetData<Sys_AllSettingEntity>(AppConfig.HRM_CAN_MEALRECORD_EATEND_CONFIG.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status).FirstOrDefault();
            if (rsTimeConfig != null && rsTimeConfig.Value1 != null)
            {
                dateConfig = DateTimeHelper.ConvertStringToDateTime(rsTimeConfig.Value1, ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond_ffffff.ToString());
            }
            double hourConfig = dateConfig.Hour + (((double)dateConfig.Minute) / 60);

            if (model.DateFrom != null)
            {
                model.DateFrom = model.DateFrom.Value.Date.AddHours(hourConfig);
            }
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddHours(hourConfig).AddMilliseconds(-1);
            }
            #endregion

            var result = GetListData<Can_MealRecordModel, Can_MealRecordEntity, Can_MealRecordSearchModel>(request, model, ConstantSql.hrm_can_get_MealRecord, ref status);
            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(dataSourceResult);
        }

        [HttpPost]
        public ActionResult GetTAMData([DataSourceRequest] DataSourceRequest request, Can_TamScanLogModel model)
        {
            var service = new Can_TamServices();
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

        #region can_BackPay
        public ActionResult GetBackPayList([DataSourceRequest] DataSourceRequest request, Can_BackPaySearchModel backpaySearchModel)
        {
            string status = string.Empty;
            var result = GetListData<Can_BackPayModel, Can_BackPayEntity, Can_BackPaySearchModel>(request, backpaySearchModel, ConstantSql.hrm_can_sp_get_BackPay, ref status);
            if (backpaySearchModel.IsExport)
            {
                var fullPath = ExportService.Export(result, backpaySearchModel.ValueFields.Split(','));
                return Json(fullPath);
            }

            #region Phân Trang

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(dataSourceResult);

            #endregion


            //return Json(result.ToDataSourceResult(request));
        }
        #endregion


        public JsonResult GetLineByID(Guid id)
        {
            string status = string.Empty;
            var model = new Can_LineModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Can_LineEntity>(id, ConstantSql.hrm_can_sp_get_LineById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Can_LineModel>();
            }
            return Json(model);
            //var result = GetDataForControl<Can_LineModel, Can_LineEntity>(id, ConstantSql.hrm_can_sp_get_LineById);
            //return Json(result);
        }
        /// <summary>
        /// [Hieu.Van] Tồng hợp dữ liệu ăn
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult GetMealRecordSummary([DataSourceRequest] DataSourceRequest request, Can_MealRecordSummaryModel model)
        {

            Can_MealRecordServices CanService = new Can_MealRecordServices();
            var baseService = new BaseService();
            var Actionservices = new ActionService(UserLogin);
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            List<int?> OrgIds = new List<int?>();
            if (model.DateFrom != null)
            {
                dateStart = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                dateEnd = model.DateTo.Value;
            }

            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.OrgStructureIDs);
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

                if (model.OrgStructureIDs != null)
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

            var result = CanService.GetMealRecordSummary(model.LineID, model.CateringID, model.CanteenID, dateStart, dateEnd, lstProfileIDs,UserLogin).ToList().Translate<Can_MealRecordModel>();

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
                    lstSelect.Add(Guid.Parse(item));
                }
                var resultSelect = result.Where(m => lstSelect.Contains(m.ID)).ToList();
                var fullPath = ExportService.Export(resultSelect, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            //request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetMealAllowanceToMoneyList([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceToMoneySearchModel mealModel)
        {
            var result = GetListData<Can_MealAllowanceToMoneyModel, Can_MealAllowanceToMoneyEntity, Can_MealAllowanceToMoneySearchModel>
                (request, mealModel, ConstantSql.hrm_can_get_MealAllowanceToMoney, ref status).Translate<Can_MealAllowanceToMoneyModel>();
            if (mealModel.IsExport)
            {
                var fullPath = ExportService.Export(result, mealModel.ValueFields.Split(','));
                return Json(fullPath);
            }
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(dataSourceResult);
        }

        [HttpPost]
        public ActionResult GetMealRecordMissingList([DataSourceRequest] DataSourceRequest request, Can_MealRecordMissingSearchModel model)
        {
            var service = new Can_MealRecordMissingServices();
            var hrService = new Hre_ProfileServices();
            DateTime DateFrom = SqlDateTime.MinValue.Value;
            DateTime DateTo = SqlDateTime.MaxValue.Value;
            if (model.DateFrom != null)
            {
                DateFrom = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                DateTo = model.DateTo.Value;
            }
            //string strOrgIDs = Common.ListToString(model.OrgStructureID);
            #region xử lý lấy lstProfileIds theo OrgStructureID
            List<Guid> lstProfileIDs = new List<Guid>();
            string status = string.Empty;
            List<object> lstObj = new List<object>();

            //lstObj.Add(strOrgIDs); 
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Guid> _temp = new List<Guid>();
            List<Guid> _profileIDs = new List<Guid>();
            var baseService = new ActionService(UserLogin);
            if (model.ProfileIDSearch != null)
            {
                var lst = model.ProfileIDSearch.Split(',');
                _temp = lst.Select(Guid.Parse).ToList();

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
            // var result = service.GetAllUseEntity<Can_MealRecordMissingEntity>(ref status);

            service.ComputeMealRecordMissing(lstProfileIDs,
                DateFrom,
                DateTo,
                model.TamScanReasonMissID,
                model.Status,
                model.MealAllowanceTypeSettingID);
            return Json(true);
        }

        /// <summary>
        /// Tính số lần theo công thức (Dùng computeBackPay)
        /// </summary>
        /// <param name="amount_missing"></param>
        /// <returns></returns>
        private double ComputeAmountBackPay(double amount_missing)
        {
            return ((amount_missing / 3) * 2 + amount_missing - (amount_missing / 3) * 3);
        }

        /// <summary>
        /// [Tung.Ly 20140730 Modify]
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComputeBackPayList([DataSourceRequest] DataSourceRequest request, Can_ComputeBackPaySearchModel model)
        {
            string status = string.Empty;
            string note = "Tổng Hợp Trả Lại Tiền Ăn ";

            var baseService = new BaseService();
            var dataComputes = GetListData<Can_ComputeBackPayModel, Can_ComputeBackPayModel, Can_ComputeBackPaySearchModel>(request, model, ConstantSql.hrm_can_sp_get_ComputeBackPay, ref status);
            var computeBackPayModels = new List<Can_ComputeBackPayModel>();
            if (dataComputes.Any() && model != null && model.MonthYear.HasValue)
            {
                var month = model.MonthYear.Value.Month;
                var year = model.MonthYear.Value.Year;

                var backPays = baseService.GetAllUseEntity<Can_BackPayEntity>(ref status).Where(p => p.MonthYear.HasValue && p.MonthYear.Value.Year == year && p.MonthYear.Value.Month == month).ToList();

                foreach (var canComputeBackPayModel in dataComputes)
                {
                    var backPay = backPays.Where(p => p.ProfileID == canComputeBackPayModel.ProfileID && p.MealAllowanceTypeSettingID == canComputeBackPayModel.MealAllowanceTypeSettingID).FirstOrDefault();

                    #region Thêm mới vào Can_BackPay
                    if (backPay == null)
                    {
                        var backPayEntity = new Can_BackPayEntity
                        {
                            ProfileID = canComputeBackPayModel.ProfileID,
                            MonthYear = model.MonthYear,
                            MealAllowanceTypeSettingID = canComputeBackPayModel.MealAllowanceTypeSettingID
                        };

                        if (canComputeBackPayModel.IsFullPay.HasValue && canComputeBackPayModel.IsFullPay.Value)
                        {
                            //neu isFullPay => cap nhat count , amount
                            backPayEntity.Count = (int?)canComputeBackPayModel.Total;
                            backPayEntity.Amount = (canComputeBackPayModel.Amount * canComputeBackPayModel.Total);
                        }
                        else
                        {
                            //neu ko la isFullPay => cap nhat CountByFomular,AmountByFomular
                            backPayEntity.CountByFomular = (int?)ComputeAmountBackPay(canComputeBackPayModel.Total);
                            backPayEntity.AmountByFomular = canComputeBackPayModel.Amount * ComputeAmountBackPay(canComputeBackPayModel.Total);
                        }
                        //add
                        backPayEntity.Note = note + canComputeBackPayModel.MonthYear;
                        baseService.Add<Can_BackPayEntity>(backPayEntity);
                        backPays.Add(backPayEntity);
                    }
                    #endregion

                    #region Chỉnh sửa Can_BackPay
                    else
                    {
                        //edit
                        //neu isFullPay => cap nhat count , amount
                        //neu ko la isFullPay => cap nhat CountByFomular,AmountByFomular
                        if (canComputeBackPayModel.IsFullPay.HasValue && canComputeBackPayModel.IsFullPay.Value)
                        {
                            //neu isFullPay => cap nhat count , amount
                            backPay.Count = (int?)canComputeBackPayModel.Total;
                            backPay.Amount = canComputeBackPayModel.Amount * canComputeBackPayModel.Total;
                        }
                        else
                        {
                            //neu ko la isFullPay => cap nhat CountByFomular,AmountByFomular
                            backPay.CountByFomular = (int?)ComputeAmountBackPay(canComputeBackPayModel.Total);
                            backPay.AmountByFomular = canComputeBackPayModel.Amount * ComputeAmountBackPay(canComputeBackPayModel.Total);
                        }

                        if (!string.IsNullOrEmpty(backPay.Note))
                        {
                            canComputeBackPayModel.Note = backPay.Note;
                        }
                        backPay.Note = note + canComputeBackPayModel.MonthYear;
                        baseService.Edit<Can_BackPayEntity>(backPay);
                    }
                    #endregion
                }

                #region List hiển thị
                var profileIds = dataComputes.Select(p => p.ProfileID).ToList();

                var modelSearch = new Can_BackPaySearchModel();
                modelSearch.ProfileName = null;
                modelSearch.DateFrom = null;
                modelSearch.DateTo = null;

                var datas = GetListData<Can_BackPayModel, Can_BackPayEntity, Can_BackPaySearchModel>(request, modelSearch, ConstantSql.hrm_can_sp_get_BackPay, ref status);
                computeBackPayModels = datas.Where(p => p.MonthYear.HasValue && p.MonthYear.Value.Year == year && p.MonthYear.Value.Month == month)
                     .Select(p => new Can_ComputeBackPayModel
                     {
                         ProfileName = p.ProfileName,
                         MonthYear = p.MonthYear.HasValue ? p.MonthYear.Value.ToString("MM-yyyy") : string.Empty,
                         Note = p.Note,
                         Amount = p.Amount ?? 0,
                         MealAllowanceTypeSettingID = p.MealAllowanceTypeSettingID,
                         Type = p.MealAllowanceTypeName,
                         CountByFomular = ComputeAmountBackPay(p.CountByFomular ?? 0),
                         AmountByFomular = p.AmountByFomular,
                         Summary = ComputeAmountBackPay(p.CountByFomular ?? 0) + (p.Count ?? 0),
                         Total = p.Count ?? 0
                     })
                     .ToList();

                request.Page = 1;
                var dataSourceResult = computeBackPayModels.ToDataSourceResult(request);
                dataSourceResult.Total = computeBackPayModels.Count() <= 0 ? 0 : datas.FirstOrDefault().TotalRow;
                return Json(dataSourceResult);
                #endregion
            }

            return Json(null);
            #region Phân Trang


            #endregion


            // return Json(computeBackPayModels.ToDataSourceResult(request));
        }

        public ActionResult GetRecordMissingList([DataSourceRequest] DataSourceRequest request, Can_RecordMissingSearchModel model)
        {
            if (model.DateFrom != null)
            {
                model.DateFrom = model.DateFrom.Value.Date;
            }
            if (model.DateTo != null)
            {
                model.DateTo = model.DateTo.Value.Date.AddDays(1).AddMilliseconds(-1);
            }

            string status = string.Empty;
            var result = GetListData<Can_MealRecordMissingModel, Can_MealRecordMissingEntity, Can_RecordMissingSearchModel>(request, model, ConstantSql.hrm_can_sp_get_RecordMissing, ref status);
            // Session["TamScanReasonMiss"] = GetData<Cat_TAMScanReasonMissMuitlModel, Cat_TAMScanReasonMissMultiEntity>(string.Empty, ConstantSql.hrm_cat_sp_get_TamScanReasonMiss_multi); ;
            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return Json(dataSourceResult);
        }

        /// <summary>
        /// [Hieu.Van] - Xuất các dòng dữ liệu được chọn của LaudryRecord ra file Excel
        /// Dùng Để Format Field Trước Khi Xuất Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportMealRecordSelected(string selectedIds, string valueFields)
        {
            selectedIds = Common.DotNetToOracle(selectedIds);
            //return ExportSelectedAndReturn<Can_MealRecordEntity, Can_MealRecordModel>(selectedIds, valueFields, ConstantSql.hrm_can_sp_get_MealRecord_ByIds);
            var listModel = GetData<Can_MealRecordModel, Can_MealRecordEntity>(selectedIds, ConstantSql.hrm_can_sp_get_MealRecord_ByIds);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Can_MealRecordModel.FieldNames.WorkDay, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Can_MealRecordModel.FieldNames.TimeLog, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Can_MealRecordModel.FieldNames.Hour, ConstantFormat.HRM_Format_Grid_ShortTime);
            formatFields.Add(Can_MealRecordModel.FieldNames.Amount, ConstantFormat.HRM_Format_Money);
            status = ExportService.Export(listModel, valueFields.Split(','), formatFields);
            return Json(status);
        }

        public ActionResult ExportMealAllowanceToMoneyList([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceToMoneySearchModel model)
        {
            return ExportAllAndReturn<Can_MealAllowanceToMoneyModel, Can_MealAllowanceToMoneyEntity, Can_MealAllowanceToMoneySearchModel>(request, model, ConstantSql.hrm_can_get_MealAllowanceToMoney);
        }

        public ActionResult ExportMealAllowanceTypeSetting([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceTypeSettingSearchModel model)
        {
            return ExportAllAndReturn<Can_MealAllowanceTypeSettingModel, Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingSearchModel>(request, model, ConstantSql.hrm_can_get_MealAllowanceType);
        }


        public ActionResult ExportMealPriceTypeSetting([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceTypeSettingSearchModel model)
        {
            return ExportAllAndReturn<Can_MealAllowanceTypeSettingModel, Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingSearchModel>(request, model, ConstantSql.hrm_can_get_MealPriceType);
        }
        public ActionResult ExportMealRecord([DataSourceRequest] DataSourceRequest request, Can_MealRecordSearchModel model)
        {
            var listModel = GetListData<Can_MealRecordModel, Can_MealRecordEntity, Can_MealRecordSearchModel>(request, model, ConstantSql.hrm_can_get_MealRecord, ref status);
            Dictionary<string, string> formatFields = new Dictionary<string, string>();
            formatFields.Add(Can_MealRecordModel.FieldNames.WorkDay, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Can_MealRecordModel.FieldNames.TimeLog, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Can_MealRecordModel.FieldNames.DateCreate, ConstantFormat.HRM_Format_DayMonthYear);
            formatFields.Add(Can_MealRecordModel.FieldNames.Hour, ConstantFormat.HRM_Format_Grid_ShortTime);
            formatFields.Add(Can_MealRecordModel.FieldNames.Amount, ConstantFormat.HRM_Format_Money);
            status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','), formatFields);
            return Json(status);
        }
        #region Can_Report

        #region Can_GetReportMealTimeSummary
        public ActionResult GetReportMealTimeSummary([DataSourceRequest] DataSourceRequest request, Can_ReportMealTimeSummaryModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = DateTime.Now.AddMonths(-1);
            DateTime To = DateTime.Now;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            string status = string.Empty;
            var result = service.ReportMealTimeSummary(Model.CateringIDs,
                Model.CanteenIDs,
                Model.LineIDs,
                From,
                To,
                Model.OrgIDs);
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportAdjustmentMealAllowancePayment
        public ActionResult GetReportAdjustmentMealAllowancePayment([DataSourceRequest] DataSourceRequest request, Can_ReportAdjustmentMealAllowancePaymentModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            var Actionservices = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            string strOrgIDs = Common.ListToString(OrgIds);
            string profileName = string.Empty;
            string codeEmp = string.Empty;
            if (Model.ProfileName != string.Empty)
            {
                profileName = Model.ProfileName;
            }

            if (Model.CodeEmp != string.Empty)
            {
                codeEmp = Model.CodeEmp;
            }
            List<Guid> lstProfileIDs = new List<Guid>();
            List<Object> Lstsearch = new List<object>();
            Lstsearch.Add(strOrgIDs);
            Lstsearch.Add(profileName);
            Lstsearch.Add(codeEmp);
            if (Model.OrgStructureID != null)
            {
                string status = string.Empty;
                lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(Lstsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            }
            var result = service.ReportAdjustmentMealAllowancePayment(From, To, lstProfileIDs);
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportMealTimeDetailNoEat
        public ActionResult GetReportMealTimeDetailNoEat([DataSourceRequest] DataSourceRequest request, Can_ReportMealTimeDetailNoEatSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            var result = service.ReportMealTimeDetailNoEat(Model.WorkPlaceID, Model.Status, Model.CodeEmp, Model.IsIncludeQuitEmp, From, To, Model.OrgStructureID);
            var rs = result.Translate<Can_ReportMealTimeDetailNoEatEntity>();
            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportMealTimeDetail
        public ActionResult GetReportMealTimeDetail([DataSourceRequest] DataSourceRequest request, Can_ReportMealTimeDetailSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            var result = service.ReportMealTimeDetail(Model.CateringIDs, Model.CanteenIDs, Model.LineIDs, From, To, Model.OrgStructureID);
            var rs = result.Translate<Can_ReportMealTimeDetailModel>();
            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportMultiSlideCard
        public ActionResult GetReportMultiSlideCard([DataSourceRequest] DataSourceRequest request, Can_ReportMultiSlideCardSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            var Actionservices = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            string profilename = null;
            string codeemp = null;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(strOrgIDs);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            List<Guid> lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.ReportMultiSlideCard(Model.CateringID, Model.CanteenID, Model.LineID, Model.WorkPlaceID, From, To, lstProfileIDs, Model.IsIncludeQuitEmp);
            var rs = result.Translate<Can_ReportMultiSlideCardModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }

        #endregion

        #region Can_ReportCardNotStand
        public ActionResult GetReportCardNotStand([DataSourceRequest] DataSourceRequest request, Can_ReportCardNotStandSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            if (Model.CanteenID != null) Model.CanteenIDs.Add(Model.CanteenID);
            if (Model.CateringID != null) Model.CateringIDs.Add(Model.CateringID);
            if (Model.LineID != null) Model.LineIDs.Add(Model.LineID);
            var result = service.ReportCardNotStand(Model.CateringIDs, Model.CanteenIDs, Model.LineIDs, From, To, Model.OrgIDs);
            var rs = result.Translate<Can_ReportCardNotStandModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportDetailCard
        public ActionResult GetReportDetailCard([DataSourceRequest] DataSourceRequest request, Can_ReportDetailCardSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            if (Model.CanteenID != null) Model.CanteenIDs.Add(Model.CanteenID);
            if (Model.CateringID != null) Model.CateringIDs.Add(Model.CateringID);
            if (Model.LineID != null) Model.LineIDs.Add(Model.LineID);
            var result = service.ReportDetailCard(Model.CateringIDs, Model.CanteenIDs, Model.LineIDs, From, To, Model.OrgStructureID);
            var rs = result.Translate<Can_ReportDetailCardModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportSumaryReturnMoneyEat
        public ActionResult GetReportSumaryReturnMoneyEat([DataSourceRequest] DataSourceRequest request, Can_ReportSumaryReturnMoneyEatSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            var result = service.GetReportSumaryReturnMoneyEat(Model.CateringIDs, Model.CanteenIDs, Model.LineIDs, Model.WorkPlaceID, From, To, Model.OrgStructureID, Model.CodeEmp, Model.IsIncludeQuitEmp);
            #region xử lý xuất báo cáo
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportCardByLocation
        public ActionResult GetReportCardByLocation([DataSourceRequest] DataSourceRequest request, Can_ReportCardByLocationSearchModel Model)
        {
            var baseService = new ActionService(UserLogin);
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            string profilename = null;
            string codeemp = Model.CodeEmp;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(strOrgIDs);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            List<Guid> lstProfileIDs = baseService.GetData<Hre_ProfileIdEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.ReportCardByLocation(Model.CateringID, Model.CanteenID, Model.LineID, From, To, lstProfileIDs, Model.workPlaceID, Model.eatPlaceID, Model.IsIncludeQuitEmp);
            var rs = result.Translate<Can_ReportCardByLocationModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }

        #endregion

        #region Can_ReportPayMoneyEat
        public ActionResult GetReportPayMoneyEat([DataSourceRequest] DataSourceRequest request, Can_ReportPayMoneyEatSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            var result = service.GetReportPayMoneyEat(Model.CateringID, Model.CanteenID, Model.LineID, From, To, Model.OrgStructureID, Model.CountryID, Model.CodeEmp, Model.IsIncludeQuitEmp);
            //var rs = result.Translate<Can_ReportPayMoneyEatModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportMealAllowanceOfEmployee
        public ActionResult GetReportMealAllowanceOfEmployee([DataSourceRequest] DataSourceRequest request, Can_ReportMealAllowanceOfEmployeeSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            var Actionservices = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            string profilename = null;
            string codeemp = Model.CodeEmp;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(strOrgIDs);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            List<Guid> lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.ReportMealAllowanceOfEmployee(Model.MealAllowanceTypeSettingID, From, To, lstProfileIDs, Model.WorkPlaceID, Model.IsIncludeQuitEmp);
            var rs = result.Translate<Can_ReportCardByLocationModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region Can_ReportHDTJobCardMore
        public ActionResult GetReportHDTJobCardMore([DataSourceRequest] DataSourceRequest request, Can_ReportHDTJobCardMoreSearchModel Model)
        {
            var service = new Can_ReportServices();
            var hrService = new Hre_ProfileServices();
            var Actionservices = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.OrgStructureID != null && Model.OrgStructureID.Count() > 0)
            {
                OrgIds = Model.OrgStructureID;
            }
            string strOrgIDs = Common.ListToString(OrgIds);
            string status = string.Empty;
            string profilename = null;
            string codeemp = Model.CodeEmp;
            List<object> objectsearch = new List<object>();
            objectsearch.Add(strOrgIDs);
            objectsearch.Add(profilename);
            objectsearch.Add(codeemp);
            List<Guid> lstProfileIDs = Actionservices.GetData<Hre_ProfileIdEntity>(objectsearch, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.ReportHDTJobCardMore(Model.CateringID, Model.CanteenID, Model.LineID, From, To, lstProfileIDs, Model.IsIncludeQuitEmp);
            var rs = result.Translate<Can_ReportCardByLocationModel>();

            #region xử lý xuất báo cáo

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, rs);
                return Json(fullPath);
            }
            #endregion
            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #endregion

        public string CheckConnectTAM()
        {
            var service = new Can_TamServices();
            string message = "";
            bool status = false;

            bool isconnect = service.IsConnected(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_, ref message);

            if (isconnect == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            var sys_AllSetting = new Sys_AllSettingEntity();
            sys_AllSetting = LibraryService.GetSys_AllSettingByKey(Constant.HRM_SYS_COMPUTE_TAMLOG_CAN);
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

        public JsonResult GetCanteenCascading(string Catering)
        {
            if (Catering != string.Empty)
            {
                var result = GetDataForControl<Can_CanteenMultiModel, Can_CanteenMultiEntity>(Common.DotNetToOracle(Catering), ConstantSql.hrm_can_sp_get_Canteen_ByCateringId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetMealAllowanceType(string MealAllowanceTypeSettingID)
        {
            if (MealAllowanceTypeSettingID != string.Empty)
            {
                try
                {
                    if (MealAllowanceTypeSettingID == Guid.Empty.ToString())
                    {
                        var result = GetDataForControl<Cat_TAMScanReasonMissModel, Cat_TAMScanReasonMissEntity>("", ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss);
                        return Json(result.Data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = GetDataForControl<Cat_TAMScanReasonMissModel, Cat_TAMScanReasonMissEntity>(Common.DotNetToOracle(MealAllowanceTypeSettingID), ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss_ById);
                        return Json(result.Data, JsonRequestBehavior.AllowGet);
                    }

                }
                catch
                {
                    return Json(new { });
                }

            }
            return Json(new { });
        }
    }
}