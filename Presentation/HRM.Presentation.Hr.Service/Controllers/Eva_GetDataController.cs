using System;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Evaluation.Domain;
using HRM.Business.Hr.Domain;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Business.Evaluation.Models;
using System.Collections.Generic;
using HRM.Business.Category.Models;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using System.Data;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Hr.Models;
using System.IO;
using HRM.Business.Category.Domain;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Eva_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        string _status = string.Empty;

        #region Eva_PerformanceTemplate - Mau Bang Danh Gia

        /// <summary> Lấy ds bảng đánh giá </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportTemplateSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_PerformanceTemplateEntity, Eva_PerformanceTemplateModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_PerformanceTemplateByIds);
        }


        [HttpPost]
        public ActionResult ExportTemplateList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceTemplateSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceTemplateEntity, Eva_PerformanceTemplateModel, Eva_PerformanceTemplateSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_performanceTemplate);
        }
        [HttpPost]
        public ActionResult GetTemplateList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceTemplateSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceTemplateModel, Eva_PerformanceTemplateEntity, Eva_PerformanceTemplateSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_performanceTemplate);
        }

        [HttpPost]
        public ActionResult AddPerformanceDetail(string KPIIDs, Guid PerformanceTemplateID)
        {

            var service = new Eva_PerformanceDetailServices();

            // Thêm PerformanceDetail
            var isSuccess = service.AddPerformanceDetail(KPIIDs, PerformanceTemplateID);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetKPIByTemplateIDList([DataSourceRequest] DataSourceRequest request, Eva_KPISearchModel model)
        {
            if (model != null)
            {
                model.PerformanceTemplateID = Common.DotNetToOracle(model.PerformanceTemplateID);
            }
            return GetListDataAndReturn<Eva_KPIModel, Eva_KPIEntity, Eva_KPISearchModel>(request, model, ConstantSql.hrm_eva_sp_get_KpiByTemplateID);
        }

        [HttpPost]
        public ActionResult EditPerformanceDetail(Eva_KPIModel model)
        {
            Eva_PerformanceDetailServices service = new Eva_PerformanceDetailServices();
            var entity = service.EditPerformanceDetail(model.ID, model.Rate, model.OrderNumber);
            var result = entity.CopyData<Eva_KPIModel>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetKPIByPerformanceIDList([DataSourceRequest] DataSourceRequest request, Eva_KPIPerformanceIDSearchModel model)
        {
            if (model != null)
            {
                model.PerformanceTemplateID = Common.DotNetToOracle(model.PerformanceTemplateID);
                model.PerformanceID = Common.DotNetToOracle(model.PerformanceID);
            }

            Guid performanceTemplateId = Guid.Empty;
            Guid.TryParse(model.PerformanceTemplateID, out performanceTemplateId);

            var service = new ActionService(UserLogin);
            var performanceDetailService = new Eva_PerformanceDetailServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;
            var listEntity = service.GetData<Eva_KPIEntity>(lstModel, ConstantSql.hrm_eva_sp_get_KpiByPerformanceID, ref status);

            //neu eva_performanceForDetail co data => thi lay data cua eva_performanceForDetail
            if (listEntity.Where(m=>m.PerformanceForDetailID.HasValue && m.PerformanceForDetailID.Value != Guid.Empty).FirstOrDefault() != null)
            {
                listEntity = listEntity.Where(m => m.PerformanceForDetailID.HasValue && m.PerformanceForDetailID.Value != Guid.Empty).ToList();
            }

            //     var kpiOrderNumbers = performanceDetailService.GetPerformanceDetailsByTemplateID(performanceTemplateId);

            var nameEntityNames = listEntity.Select(m => m.NameEntityName).Distinct().ToList();
            foreach (var nameEntityName in nameEntityNames)
            {
                var kpiObjects = listEntity.Where(m => m.NameEntityName == nameEntityName).OrderBy(m => m.OrderNumber).ToList();
                int stt = 1;
                foreach (var item in kpiObjects)
                {
                    item.Stt = stt++;
                }
            }
            listEntity = listEntity.OrderBy(m => m.OrderNumber).ToList();

            return Json(listEntity.ToDataSourceResult(request, ModelState));
            //  return GetListDataAndReturn<Eva_KPIModel, Eva_KPIEntity, Eva_KPIPerformanceIDSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_KpiByPerformanceID);
        }

        public JsonResult GetMultiEvaTemplate(string text)
        {
            return GetDataForControl<Eva_PerformanceTemplateMultiModel, Eva_PerformanceTemplateMultiEntity>(text, ConstantSql.hrm_eva_sp_get_Template_Multi);
        }

        [HttpPost]
        public ActionResult GetPerformanceTemplateInfo([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_PerformanceTemplateEntity>(id, ConstantSql.hrm_cat_sp_get_TemplateById, ref status);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Eva_Level - Cấp Bậc

        /// <summary> [Tho.Bui]:Get danh sách bậc hệ sồ lương </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetEvaLEvelList([DataSourceRequest] DataSourceRequest request, Eva_LevelSearchModel model)
        {
            return GetListDataAndReturn<Eva_LevelModel, Eva_LevelEntity, Eva_LevelSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_EvaLevelList);
        }

        /// <summary> [Tho.Bui] - Xuất dữ liệu đã chọn Eva_Level </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportEvaLevelSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_LevelEntity, Eva_LevelModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_EvaLevelByIds);
        }

        /// <summary> [Tho.Bui] - Xuât danh sách Eva_Level </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportEvaLecvelList([DataSourceRequest] DataSourceRequest request, Eva_LevelSearchModel model)
        {
            return ExportAllAndReturn<Eva_LevelEntity, Eva_LevelModel, Eva_LevelSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_EvaLevelList);
        }

        #endregion

        #region Eva_Performance - Danh Gia NV



        /// <summary>
        /// [Quoc.Do]:Get danh sách Tự Đánh Giá (Eva_Performance)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetPerformanceList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceModel, Eva_PerformanceEntity, Eva_PerformanceSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Performance);
        }

        /// <summary>
        /// Duyệt performance
        /// </summary>
        /// <param name="performanceId"></param>
        /// <returns></returns>
        public ActionResult ApprovePerformance(Guid performanceId)
        {
            Eva_PerformanceServices evaService = new Eva_PerformanceServices();
            evaService.ApproveEvaPerformance(performanceId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // [Quoc.Do] - Xuất dữ liệu đã chọn Đánh Giá của 1 nhân viên (Eva_Performance)
        public ActionResult GetPerformanceByProfileList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceProfileSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceModel, Eva_PerformanceEntity, Eva_PerformanceProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceByProfile);
        }

        [HttpPost]
        public ActionResult ExportPerformanceProfileList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceProfileSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEntity, Eva_PerformanceModel, Eva_PerformanceProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceByProfile);
        }
        /// <summary>
        /// [Quoc.Do] - Xuất dữ liệu đã chọn Tự Đánh Giá (Eva_Performance)
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportPerformanceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_PerformanceEntity, Eva_PerformanceModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_PerformanceByIds);
        }

        /// <summary>
        /// [Quoc.Do] - Xuât danh sách Tự Đánh Giá (Eva_Performance)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportPerformanceList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEntity, Eva_PerformanceModel, Eva_PerformanceSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Performance);
        }

        [HttpPost]
        public ActionResult CheckPerformanceExist([DataSourceRequest] DataSourceRequest request, Eva_PerformanceModel model)
        {
            var performanceService = new Eva_PerformanceServices();
            var performanceEntity = model.CopyData<Eva_PerformanceEntity>();
            var isExisted = performanceService.CheckPerformanceExist(performanceEntity);
            return Json(isExisted, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPerformancePlanInfo([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_PerformancePlanEntity>(id, ConstantSql.hrm_eva_sp_get_PerformancePlanById, ref status);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Eva_PerformanceEva - Đánh Giá cho NV theo các cấp
        public ActionResult GetPerformanceEvaByProfileList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaProfileSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceEvaModel, Eva_PerformanceEvaEntity, Eva_PerformanceEvaProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaByProfile);
        }
        [HttpPost]
        public ActionResult ExportPerformanceEvaByProfileList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaProfileSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel, Eva_PerformanceEvaProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaByProfile);
        }


        public ActionResult GetPerformanceEvaWaitEvaList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaWaitEvaSearchModel model)
        {
            //chỉ load những dữ liệu theo profileId
            if (model != null)
            {
                model.EvaluatorID = Common.DotNetToOracle(model.EvaluatorID);
            }
            if (model != null && model.EvaluatorID == null)
            {
                model.EvaluatorID = Common.DotNetToOracle(Guid.Empty.ToString());
            }

            return GetListDataAndReturn<Eva_PerformanceEvaModel, Eva_PerformanceEvaEntity, Eva_PerformanceEvaWaitEvaSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaWaitingEva);
        }
        [HttpPost]
        public ActionResult CountWaitEva(Guid? EvaluatorID)
        {
            Eva_PerformanceEvaWaitEvaSearchModel model = new Eva_PerformanceEvaWaitEvaSearchModel();
            // model.EvaluatorID = Common.DotNetToOracle(EvaluatorID.ToString());
            // var rs = GetListDataAndReturn<Eva_PerformanceEvaModel, Eva_PerformanceEvaEntity, Eva_PerformanceEvaWaitEvaSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaWaitingEva).CopyData<Eva_PerformanceEvaModel>().TotalRow;
            var baseService = new ActionService(UserLogin);
            string status = "";
            var objs = new List<object>();
            objs.Add(EvaluatorID);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(null);
            objs.Add(1);
            objs.Add(10000);
            var count = baseService.GetData<Eva_PerformanceEvaEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaWaitingEva, ref status).Select(s => s.ID).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ExportPerformanceEvaWaitList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaWaitEvaSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel, Eva_PerformanceEvaWaitEvaSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaWaitingEva);
        }
        [HttpPost]
        public ActionResult ExportPerformanceEvaSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_PerformanceEvaByIds);
        }
        public ActionResult GetPerformanceEvaActiveList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaActiveSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceEvaModel, Eva_PerformanceEvaEntity, Eva_PerformanceEvaActiveSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaActive);
        }
        public ActionResult GetPerformanceEvaByPerformance([DataSourceRequest] DataSourceRequest request, Guid PerformanceID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.ConvertToGuid(PerformanceID.ToString()));
            var result = baseService.GetData<Eva_PerformanceEvaEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaByPerformance, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult GetPerformanceEvaDetailList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaActiveSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceEvaModel, Eva_PerformanceEvaEntity, Eva_PerformanceEvaActiveSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaActive);
        }
        [HttpPost]
        public ActionResult ExportPerformanceEvaActiveList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceEvaActiveSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel, Eva_PerformanceEvaActiveSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceEvaActive);
        }


        #endregion

        #region Eva_PerformancePlan - Kế Hoạch Đánh Giá
        /// <summary>
        /// [Quoc.Do]:Get danh sách Kế Hoạch Đánh Giá (Eva_PerformancePlan)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetPerformancePlanList([DataSourceRequest] DataSourceRequest request, Eva_PerformancePlanSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformancePlanModel, Eva_PerformancePlanEntity, Eva_PerformancePlanSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformancePlan);
        }
        /// <summary>
        /// [Quoc.Do] - Xuất dữ liệu đã chọn Kế Hoạch Đánh Giá (Eva_PerformancePlan)
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportPerformancePlanSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_PerformancePlanEntity, Eva_PerformancePlanModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_PerformancePlanByIds);
        }
        /// <summary>
        /// [Quoc.Do] - Xuât danh sách Kế Hoạch Đánh Giá (Eva_PerformancePlan)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportPerformancePlanList([DataSourceRequest] DataSourceRequest request, Eva_PerformancePlanSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformancePlanEntity, Eva_PerformancePlanModel, Eva_PerformancePlanSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformancePlan);
        }

        public JsonResult GetMultiPerformancePlan(string text)
        {
            return GetDataForControl<Eva_PerformancePlanModel, Eva_PerformancePlanEntity>(text, ConstantSql.hrm_eva_sp_get_PerformancePlan_Multi);
        }
        #endregion

        #region Eva_KPI - Tieu Chi Danh Gia

        /// <summary> [Quoc.Do]:Get danh sách tiêu chí đánh giá (Eva_KPI) </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetKPIList([DataSourceRequest] DataSourceRequest request, Eva_KPISearch1Model model)
        {
            return GetListDataAndReturn<Eva_KPIModel, Eva_KPIEntity, Eva_KPISearch1Model>(request, model, ConstantSql.hrm_eva_sp_get_KPI);
        }

        /// <summary> [Quoc.Do] - Xuất dữ liệu đã chọn Eva_KPI </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportKPISelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_KPIEntity, Eva_KPIModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_KPIByIds);
        }

        /// <summary> [Quoc.Do] - Xuât danh sách Eva_KPI </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportKPIList([DataSourceRequest] DataSourceRequest request, Eva_KPISearch1Model model)
        {
            return ExportAllAndReturn<Eva_KPIEntity, Eva_KPIModel, Eva_KPISearch1Model>(request, model, ConstantSql.hrm_eva_sp_get_KPI);
        }

        public JsonResult GetMultiKPI(string text)
        {
            return GetDataForControl<Eva_KPIMultiModel, Eva_KPIMultiEntity>(text, ConstantSql.hrm_eva_sp_get_KPI_Multi);
        }

        public JsonResult GetMultiObjectForKPI(string text)
        {
            var listObj = new List<object>();
            listObj.Add(text);
            return GetData<CatObjectKPIMultiModel, CatObjectKPIMultiEntity>(listObj, ConstantSql.hrm_cat_sp_get_objectKPI_Multi);
        }

        public ActionResult GetKPIListByNameEntityID([DataSourceRequest] DataSourceRequest request, Eva_KPIMultiModel model)
        {
            return GetListDataAndReturn<Eva_KPIModel, Eva_KPIEntity, Eva_KPIMultiModel>(request, model, ConstantSql.hrm_eva_sp_get_KPIListByNameEntityID);
        }
        public JsonResult GetMultiKPIListByNameEntityID(string text, string strNameEntityID)
        {
            if (text == ConstantDisplay.HRM_Evaluation_KPI_SelectObject.TranslateString())
                text = null;
            Guid? NameEntityID = null;
            if (!string.IsNullOrEmpty(strNameEntityID))
                NameEntityID = Guid.Parse(strNameEntityID);
            var services = new ActionService(UserLogin);
            var listObj = new List<object>();
            listObj.Add(text);
            listObj.Add(NameEntityID);
            string status = "";
            //string status = "";
            var lstHre_ProfileMultiModel = services.GetData<Eva_KPIModel>(listObj, ConstantSql.hrm_eva_sp_get_KPIMutilByNameEntityID, ref status);
            return Json(lstHre_ProfileMultiModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMultiCategoryKPI([DataSourceRequest] DataSourceRequest request, string Keyword)
        {
            // return GetDataForControl<Eva_KPIMultiModel, Eva_KPIMultiEntity>(Common.DotNetToOracle(Keyword), ConstantSql.hrm_cat_sp_get_objectKPI_Multi);


            var baseService = new ActionService(UserLogin);
            var listObj = new List<object>();
            listObj.Add(Keyword);
            var result = baseService.GetData<Eva_KPIMultiEntity>(Common.DotNetToOracle(Keyword), ConstantSql.hrm_eva_sp_get_KPIByEntityId, ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
            //  return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region SalesType - Loại Doanh Số

        public ActionResult GetSalesTypeList([DataSourceRequest] DataSourceRequest request, Eva_SalesTypeSearchModel model)
        {
            return GetListDataAndReturn<Eva_SalesTypeModel, Eva_SalesTypeEntity, Eva_SalesTypeSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SalesType);
        }

        [HttpPost]
        public ActionResult ExportSalesTypeList([DataSourceRequest] DataSourceRequest request, Eva_SalesTypeSearchModel model)
        {
            return ExportAllAndReturn<Eva_SalesTypeEntity, Eva_SalesTypeModel, Eva_SalesTypeSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SalesType);
        }

        public ActionResult ExportSalesTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_SalesTypeEntity, Eva_SalesTypeModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_SalesTypeByIds);
        }
        #endregion

        #region Eva_SaleEvaluation - Doanh Số Cá Nhân
        public ActionResult GetSaleEvaluationByProfileList([DataSourceRequest] DataSourceRequest request, Eva_SaleEvaluationByProfileSearchModel model)
        {
            if (model != null)
            {
                model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            }
            return GetListDataAndReturn<Eva_SaleEvaluationModel, Eva_SaleEvaluationEntity, Eva_SaleEvaluationByProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleEvaluationByProfile);
        }

        public ActionResult GetSaleEvaluationList([DataSourceRequest] DataSourceRequest request, Eva_SaleEvaluationSearchModel model)
        {
            return GetListDataAndReturn<Eva_SaleEvaluationModel, Eva_SaleEvaluationEntity, Eva_SaleEvaluationSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleEvaluation);
        }

        //public ActionResult GetSaleEvaluationListByProID([DataSourceRequest] DataSourceRequest request, Eva_SaleEvaluationPortalByProfileSearchModel model)
        //{

        //    return GetListDataAndReturn<Eva_SaleEvaluationModel, Eva_SaleEvaluationEntity, Eva_SaleEvaluationPortalByProfileSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleEvaluationByProId);
        //}
        public ActionResult GetSaleEvaluationListByProID([DataSourceRequest] DataSourceRequest request, Guid? profileID)
        {

            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            // objs.Add(1);
            //  objs.Add(10000000);
            List<Eva_SaleEvaluationEntity> lsiEva = new List<Eva_SaleEvaluationEntity>();
            var result = baseService.GetData<Eva_SaleEvaluationEntity>(objs, ConstantSql.hrm_eva_sp_get_SaleEvaluationByProId, ref status);
            if (result != null && result.Count > 0)
            {
                lsiEva = result;
            }
            return Json(lsiEva.ToDataSourceResult(request));
        }

        public JsonResult GetMultiSalesType(string text)
        {
            return GetDataForControl<Eva_SalesTypeMultiModel, Eva_SalesTypeMultiEntity>(text, ConstantSql.hrm_eva_sp_get_SalesType_multi);
        }
        public ActionResult ExportSaleEvaluationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_SaleEvaluationEntity, Eva_SaleEvaluationModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_SaleEvaluationByIds);
        }
        /// <summary>
        /// [Quoc.Do] - Xuât doanh số Eva_SaleEvaluation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSaleEvaluationList([DataSourceRequest] DataSourceRequest request, Eva_SaleEvaluationSearchModel model)
        {
            return ExportAllAndReturn<Eva_SaleEvaluationEntity, Eva_SaleEvaluationModel, Eva_SaleEvaluationSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleEvaluation);
        }
        #endregion

        #region Eva_Level - Cấp Bậc
        public JsonResult GetMultiLevel(string text)
        {
            return GetDataForControl<Eva_LevelMultiModel, Eva_LevelMultiEntity>(text, ConstantSql.hrm_eva_sp_get_Level_multi);
        }
        #endregion

        #region Eva_Evaluator - Nguoi Danh Gia

        [HttpPost]
        public ActionResult DeleteByProfile(string id)
        {
            var service1 = new Eva_EvaluatorServices();
            var result = service1.DeleteListEvaluator(id);
            var result1 = result.CopyData<Eva_EvaluatorModel>();
            return Json(result1);
        }

        [HttpPost]
        public ActionResult ExportEvaluatorSelected(string selectedIds, string valueFields)
        {
            // return ExportSelectedAndReturn<Eva_EvaluatorEntity, Eva_EvaluatorModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_EvaluatorByIds);
            var service = new ActionService(UserLogin);
            var status = string.Empty;
            List<object> lstObjExport = new List<object>();
            lstObjExport.Add(null);
            lstObjExport.Add(null);
            lstObjExport.Add(1);
            lstObjExport.Add(10000000);
            selectedIds = Common.DotNetToOracle(selectedIds);
            var listEntityAll = service.GetData<Eva_EvaluatorEntity>(lstObjExport, ConstantSql.hrm_eva_sp_get_Evaluator, ref status);
            var listEntity = service.GetData<Eva_EvaluatorEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_EvaluatorByIds, ref status);
            //var group = listEntity.Where(x=>listEntity)
            var listByID = listEntity.Select(x => x.ProfileID).ToList();
            var result = new List<Eva_EvaluatorModel>();
            foreach (var obj in listEntity)
            {
                Eva_EvaluatorModel objEvaluator = new Eva_EvaluatorModel();
                objEvaluator.ProfileName = obj.ProfileName;
                objEvaluator.PerformanceTypeName = obj.PerformanceTypeName;
                var list = listEntityAll.Where(x => x.PerformanceTypeID == obj.PerformanceTypeID && x.ProfileID == obj.ProfileID).ToList();
                if (list != null && list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        objEvaluator.EvaluatorName += item.EvaluatorName + ", ";

                    }
                    objEvaluator.EvaluatorName = objEvaluator.EvaluatorName.Substring(0, objEvaluator.EvaluatorName.LastIndexOf(','));
                }
                result.Add(objEvaluator);


            }
            status = ExportService.Export(Guid.Empty, result, valueFields.Split(','), null);
            return Json(status);


        }

        [HttpPost]
        public ActionResult ExportEvaluatorList([DataSourceRequest] DataSourceRequest request, Eva_EvaluatorSearchModel model)
        {
            // return ExportAllAndReturn<Eva_EvaluatorEntity, Eva_EvaluatorModel, Eva_EvaluatorSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Evaluator);
            if (model != null)
            {
                model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            }



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
            var listEntity = service.GetData<Eva_EvaluatorEntity>(lstModel, ConstantSql.hrm_eva_sp_get_Evaluator, ref status);
            if (listEntity != null)
            {
                request.Page = 1;
                var listModel = listEntity.Translate<Eva_EvaluatorModel>();
                var profileIds = listModel.GroupBy(p => new { p.ProfileID, p.PerformanceTypeID }).ToList();
                var evaluatorReturn = new List<Eva_EvaluatorModel>();
                var evaluatorNames = string.Empty;
                foreach (var profileId in profileIds)
                {
                    var evaluators = listModel.Where(p => p.ProfileID == profileId.Key.ProfileID && p.PerformanceTypeID == profileId.Key.PerformanceTypeID).OrderBy(p => p.OrderNo).ToList();
                    var evaluator = evaluators.FirstOrDefault();
                    evaluatorNames = string.Empty;
                    foreach (var evaEvaluatorModel in evaluators)
                    {
                        evaluatorNames += evaEvaluatorModel.OrderNo + " - " + evaEvaluatorModel.EvaluatorName + " , ";
                    }
                    var eva = new Eva_EvaluatorModel();
                    if (evaluator != null)
                    {
                        eva.ID = evaluator.ID;
                        eva.ProfileName = evaluator.ProfileName;
                        eva.PerformanceTypeID = evaluator.PerformanceTypeID;
                        eva.PerformanceTypeName = evaluator.PerformanceTypeName;
                    }
                    eva.EvaluatorName = evaluatorNames.Substring(0, evaluatorNames.Length - 2);
                    evaluatorReturn.Add(eva);
                }


                var dataSourceResult = evaluatorReturn.ToDataSourceResult(request);
                if (evaluatorReturn.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = evaluatorReturn.Count() <= 0 ? 0 : (int)evaluatorReturn.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                status = ExportService.Export(evaluatorReturn, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }
            return Json(null);

        }

        public ActionResult GetEvaluatorList([DataSourceRequest] DataSourceRequest request, Eva_EvaluatorSearchModel model)
        {
            // return GetListDataAndReturn<Eva_EvaluatorModel, Eva_EvaluatorEntity, Eva_EvaluatorSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Evaluator);
            if (model != null)
            {
                model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            }

            //#region lay ds evaluator hiển thị trên 1 dòng ứng với profile

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
            var listEntity = service.GetData<Eva_EvaluatorEntity>(lstModel, ConstantSql.hrm_eva_sp_get_Evaluator, ref status);
            if (listEntity != null)
            {
                request.Page = 1;
                var listModel = listEntity.Translate<Eva_EvaluatorModel>();
                var profileIds = listModel.GroupBy(p => new { p.ProfileID, p.PerformanceTypeID }).ToList();
                var evaluatorReturn = new List<Eva_EvaluatorModel>();
                var evaluatorNames = string.Empty;
                foreach (var profileId in profileIds)
                {
                    var evaluators = listModel.Where(p => p.ProfileID == profileId.Key.ProfileID && p.PerformanceTypeID == profileId.Key.PerformanceTypeID).OrderBy(p => p.OrderNo).ToList();
                    var evaluator = evaluators.FirstOrDefault();
                    evaluatorNames = string.Empty;
                    foreach (var evaEvaluatorModel in evaluators)
                    {
                        evaluatorNames += evaEvaluatorModel.OrderNo + " - " + evaEvaluatorModel.EvaluatorName + " , ";
                    }
                    var eva = new Eva_EvaluatorModel();
                    if (evaluator != null)
                    {
                        eva.ID = evaluator.ID;
                        eva.ProfileName = evaluator.ProfileName;
                        eva.PerformanceTypeID = evaluator.PerformanceTypeID;
                        eva.PerformanceTypeName = evaluator.PerformanceTypeName;
                        eva.ProfileID = profileId.Key.ProfileID;
                    }
                    eva.EvaluatorName = evaluatorNames.Substring(0, evaluatorNames.Length - 2);
                    evaluatorReturn.Add(eva);
                }


                var dataSourceResult = evaluatorReturn.ToDataSourceResult(request);
                //if (evaluatorReturn.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                //{
                //    dataSourceResult.Total = evaluatorReturn.Count() <= 0 ? 0 : (int)evaluatorReturn.FirstOrDefault().GetPropertyValue("TotalRow");
                //}
                dataSourceResult.Total = evaluatorReturn.Count();
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new List<Eva_EvaluatorModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }
        #endregion

        public ActionResult GetEvaluatorOfProfileList([DataSourceRequest] DataSourceRequest request, Eva_EvaluatorSearchModel model)
        {
            //chỉ load những dữ liệu theo profileId
            if (model != null)
            {
                model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            }
            if (model != null && model.ProfileID == null)
            {
                model.ProfileID = Common.DotNetToOracle(Guid.Empty.ToString());
            }
            return GetListDataAndReturn<Eva_EvaluatorModel, Eva_EvaluatorEntity, Eva_EvaluatorSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Evaluator);
        }

        [HttpPost]
        public JsonResult GetMultiEvaluator(string profileID, string performanceTypeID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            if (!string.IsNullOrEmpty(profileID))
            {
                profileID = Common.DotNetToOracle(profileID);
            }
            if (!string.IsNullOrEmpty(performanceTypeID))
            {
                performanceTypeID = Common.DotNetToOracle(performanceTypeID);
            }

            List<object> lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(profileID);
            lstObj.Add(performanceTypeID);
            lstObj.Add(null);
            lstObj.Add(1);
            lstObj.Add(int.MaxValue - 1);

            var listModel = service.GetData<Eva_EvaluatorMultiModel>(lstObj, ConstantSql.hrm_eva_sp_get_Evaluator_multi, ref status);
            if (listModel != null)
            {
                // List<Eva_EvaluatorMultiModel> listModel = listEntity.Translate<Eva_EvaluatorMultiModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMultiEvaluator360(string profileID, string performanceTypeID, string performanceTemplateID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            if (!string.IsNullOrEmpty(profileID))
            {
                profileID = Common.DotNetToOracle(profileID);
            }
            if (!string.IsNullOrEmpty(performanceTypeID))
            {
                performanceTypeID = Common.DotNetToOracle(performanceTypeID);
            }

            List<object> lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(profileID);
            lstObj.Add(performanceTypeID);
            lstObj.Add(performanceTemplateID);

            var listModel = service.GetData<Eva_EvaluatorMultiModel>(lstObj, ConstantSql.hrm_eva_sp_get_Evaluator_multi, ref status);
            if (listModel != null)
            {
                // List<Eva_EvaluatorMultiModel> listModel = listEntity.Translate<Eva_EvaluatorMultiModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiPerformanceTemplate(string performanceTypeID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            if (performanceTypeID != null)
            {
                performanceTypeID = Common.DotNetToOracle(performanceTypeID);
            }

            List<object> lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(performanceTypeID);

            var listEntity = service.GetData<Eva_PerformanceTemplateMultiModel>(lstObj, ConstantSql.hrm_eva_sp_get_PerformanceTemplate_multi, ref status);
            if (listEntity != null)
            {
                List<Eva_PerformanceTemplateMultiModel> listModel = listEntity.Translate<Eva_PerformanceTemplateMultiModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMultiPerformanceTemplateWithJobTitle(Guid performanceTypeID, Guid profileID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;            
            List<object> lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(performanceTypeID);

            //get jobtitle of profile
            var perService = new Eva_PerformanceTemplateServices();
            var jobTitleID = perService.GetJobTitleOfProfile(profileID);

            var listEntity = service.GetData<Eva_PerformanceTemplateMultiModel>(lstObj, ConstantSql.hrm_eva_sp_get_PerformanceTemplate_multi, ref status);
            if (jobTitleID != Guid.Empty)
            {
                listEntity = listEntity.Where(m => m.JobTitleID == null || (m.JobTitleID.Value == jobTitleID)).ToList();    
            }            

            if (listEntity != null)
            {
                List<Eva_PerformanceTemplateMultiModel> listModel = listEntity.Translate<Eva_PerformanceTemplateMultiModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #region Eva_TagEva - Loại Đánh Giá
        public JsonResult GetMultiTag(string text)
        {
            return GetDataForControl<Eva_TagEvaMultiModel, Eva_TagEvaMultiEntity>(text, ConstantSql.hrm_eva_sp_get_TagEva_multi);
        }
        #endregion

        #region Eva_PerformanceEvaDetail - Danh Gia Chi Tiet Theo cac Tieu Chi

        public JsonResult GetPerformanceEvaDetailByPerformanceEvaID([DataSourceRequest] DataSourceRequest request, Guid PerfomanceEvaID)
        {
            try
            {
                string status = string.Empty;
                var baseService = new ActionService(UserLogin);
                var objs = new List<object>();

                objs.Add(PerfomanceEvaID);
                var result = baseService.GetData<Eva_PerformanceEvaDetailEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailByPerformanceEvaID, ref status);
                result = result.OrderBy(m => m.OrderNumber).ToList();

                var nameEntityNames = result.Select(m => m.NameEntityName).Distinct().ToList();
                foreach (var nameEntityName in nameEntityNames)
                {
                    var perEvaDetailObjects = result.Where(m => m.NameEntityName == nameEntityName).OrderBy(m => m.OrderNumber).ToList();
                    int stt = 1;
                    foreach (var item in perEvaDetailObjects)
                    {
                        item.Stt = stt++;
                    }
                }
                return Json(result.ToDataSourceResult(request));
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpPost]
        public ActionResult UpdatePerformanceExtendIDOfPerformance([DataSourceRequest] DataSourceRequest request, Guid PerformanceID)
        {
            var performanceServices = new ActionService(UserLogin);
            string status = string.Empty;
            DataTable table = new DataTable();
            var performanceEvDetailEntity = performanceServices.GetData<Eva_PerformanceEvaDetailEntity>(PerformanceID, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailByPerformanceEvaID, ref status).ToList();
            var performanceEvaEntity = performanceServices.GetData<Eva_PerformanceEvaEntity>(PerformanceID, ConstantSql.hrm_eva_sp_get_PerformanceEvaById, ref status).FirstOrDefault();
            if (performanceEvaEntity != null)
            {

                DataRow dr = table.NewRow();
                table.Columns.Add();
            }
            return null;
        }

        public JsonResult GetPerformanceEvaDetailHistoryByPerformanceEvaID([DataSourceRequest] DataSourceRequest request, Guid PerfomanceEvaID, string OrderEva)
        {
            try
            {
                string status = string.Empty;
                var baseService = new ActionService(UserLogin);
                var objs = new List<object>();
                int Order = 0;
                if (OrderEva != null)
                {
                    Order = int.Parse(OrderEva);
                }

                objs.Add(Common.DotNetToOracle(PerfomanceEvaID.ToString()));
                objs.Add(Order);
                var result = baseService.GetData<Eva_PerformanceEvaDetailEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailAllByPerformanceEvaID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetPerformanceEvaDynamicByPerformanceEvaID([DataSourceRequest] DataSourceRequest request, Guid PerformanceID, Guid PerformanceEvaID, int OrderEva)
        {
            //var gPerfomanceID = Guid.Empty;
            //var gPerfomanceEvaID = Guid.Empty;
            //if (Request["PerformanceID"] != null)
            //{
            //    Guid.TryParse(Request["PerformanceID"].ToString(), out gPerfomanceID);
            //}
            //if (Request["PerfomanceEvaID"] != null)
            //{
            //    Guid.TryParse(Request["PerfomanceEvaID"].ToString(), out gPerfomanceEvaID);
            //}

            try
            {
                Eva_PerformanceEvaServices service = new Eva_PerformanceEvaServices();
                var result = service.GetPerformEvaDetail(PerformanceID, PerformanceEvaID, OrderEva,UserLogin);
                return Json(result.ToDataSourceResult(request));
            }
            catch
            {
                return Json(null);
            }
        }

        #endregion

        #region Tính Công thức tính của PerformanceEva
        [HttpPost]
        public JsonResult GetTotalMarkByFormula([DataSourceRequest] DataSourceRequest request, string Formula, float TOTALMARK, float TOTALRATE, int TOTALKPI)
        {
            try
            {
                List<ElementFormula> lstFormular = new List<ElementFormula>();
                lstFormular.Add(new ElementFormula("TOTALMARK", TOTALMARK, 0));
                lstFormular.Add(new ElementFormula("TOTALRATE", TOTALRATE, 0));
                lstFormular.Add(new ElementFormula("TOTALKPI", TOTALKPI, 0));

                var result = FormulaHelper.ParseFormula(Formula, lstFormular).Value;
                result = Math.Round(double.Parse(result.ToString()), 3);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTotalMarkByFormula360([Bind(Prefix = "models")] List<Eva_PerformanceEvaDetailModel> listModel, string Formula)
        {
            try
            {
                double TOTALMARK = 0;
                double TOTALRATE = 0;
                int TOTALKPI = listModel.Count();
                if (string.IsNullOrEmpty(Formula))
                {
                    double totalMarkValue = 0;
                    double totalRate = 0;
                    foreach (var item in listModel)
                    {
                        if (item.Mark.HasValue)
                        {
                            totalRate += item.Rate ?? 0;
                        }
                        totalMarkValue += (item.Mark ?? 0) * (item.Rate ?? 0);
                    }
                    var resultMark = totalMarkValue / totalRate;
                    resultMark = Math.Round(resultMark, 3);
                    return Json(resultMark, JsonRequestBehavior.AllowGet);
                }

                foreach (var item in listModel)
                {
                    TOTALMARK += item.Mark ?? 0;
                    TOTALRATE += item.Rate ?? 0;
                }
                List<ElementFormula> lstFormular = new List<ElementFormula>();
                lstFormular.Add(new ElementFormula("TOTALMARK", TOTALMARK, 0));
                lstFormular.Add(new ElementFormula("TOTALRATE", TOTALRATE, 0));
                lstFormular.Add(new ElementFormula("TOTALKPI", TOTALKPI, 0));
                var result = FormulaHelper.ParseFormula(Formula, lstFormular).Value;
                result = Math.Round(double.Parse(result.ToString()), 3);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Set level theo TotalMark của PerformanceEva
        [HttpPost]
        public JsonResult GetTotalMark([DataSourceRequest] DataSourceRequest request, float TotalMark)
        {

            var service1 = new Eva_LevelServices();
            var objLevel = service1.GetLevel(TotalMark);
            var result = new Eva_LevelMultiModel();
            if (objLevel != null)
                result = new Eva_LevelMultiModel { ID = objLevel.ID, LevelName = objLevel.LevelName };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Report

        [HttpPost]
        public ActionResult ReportReportEvaluationResultValidate([DataSourceRequest] DataSourceRequest request, Eva_ReportEvaluationResultSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Eva_ReportEvaluationResultSearchModel>(model, "Eva_ReportEvaluationResult", ref message);

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);

            }
            #endregion

            return Json(message);
        }


        [HttpPost]
        public ActionResult GetReportReportEvaluationResult([DataSourceRequest] DataSourceRequest request, Eva_ReportEvaluationResultSearchModel model)
        {
            DateTime periodFromDate = model.PeriodFromDate ?? DateTime.Now;
            DateTime periodToDate = model.PeriodToDate ?? DateTime.Now;
            var result = new List<Eva_ReportEvaluationResultModel>();
            var isDataTable = false;
            object obj = new Eva_ReportEvaluationResultModel();
            if (model != null)
            {
                var service = new Eva_ReportEvaluationResultServices();
                result = service.GetReportEvaluationResult(model.OrgStructureID, periodFromDate, periodToDate, model.PerformanceTemplateID,UserLogin)
                        .ToList()
                        .Translate<Eva_ReportEvaluationResultModel>();

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
                        FileName = "Eva_ReportEvaluationResultModel",
                        OutPutPath = path,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = isDataTable
                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }

                if (model.ExportID != Guid.Empty)
                {
                    var fullPath = ExportService.Export(model.ExportID, result, null, model.ExportType);
                    return Json(fullPath);
                }
            }
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region Eva_BonusSalary
        [HttpPost]
        public ActionResult GetBonusSalaryList([DataSourceRequest] DataSourceRequest request, Eva_BonusSalarySearchModel model)
        {
            return GetListDataAndReturn<Eva_BonusSalaryModel, Eva_BonusSalaryEntity, Eva_BonusSalarySearchModel>(request, model, ConstantSql.hrm_eva_sp_get_BonusSalary);
        }

        [HttpPost]
        public ActionResult ExportBonusSalaryList([DataSourceRequest] DataSourceRequest request, Eva_BonusSalarySearchModel model)
        {
            return ExportAllAndReturn<Eva_BonusSalaryEntity, Eva_BonusSalaryModel, Eva_BonusSalarySearchModel>(request, model, ConstantSql.hrm_eva_sp_get_BonusSalary);
        }

        public ActionResult ExportBonusSalarySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_BonusSalaryEntity, Eva_BonusSalaryModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_BonusSalaryByIds);
        }
        #endregion
        #region Eva_SaleBonus - Danh Muc Thuong

        /// <summary> [Quoc.Do]:Get danh sách Danh Mục Thưởng (Eva_SaleBonus) </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetSaleBonusList([DataSourceRequest] DataSourceRequest request, Eva_SaleBonusSearchModel model)
        {
            return GetListDataAndReturn<Eva_SaleBonusModel, Eva_SaleBonusEntity, Eva_SaleBonusSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleBonus);
        }

        /// <summary> [Quoc.Do] - Xuất dữ liệu đã chọn Danh Mục Thưởng (Eva_SaleBonus) </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportSaleBonusSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_SaleBonusEntity, Eva_SaleBonusModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_SaleBonusByIds);
        }

        /// <summary> [Quoc.Do] - Xuât danh sách Danh Mục Thưởng (Eva_SaleBonus) </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExporSaleBonusList([DataSourceRequest] DataSourceRequest request, Eva_SaleBonusSearchModel model)
        {
            return ExportAllAndReturn<Eva_SaleBonusEntity, Eva_SaleBonusModel, Eva_SaleBonusSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_SaleBonus);
        }
        #endregion

        #region KPIBuilding

        public ActionResult GetKPIBuildingList([DataSourceRequest] DataSourceRequest request, Eva_KPIBuildingSearchModel model)
        {
            var endApproved = PerformaceEvaStatus.E_APPROVE_END.ToString();
            var status = string.Empty;
            var service = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = service.GetData<Eva_KPIBuildingEntity>(lstModel, ConstantSql.hrm_eva_sp_get_Performance, ref status);
            result = result.Where(m => m.Status != endApproved).ToList();
            return Json(result.ToDataSourceResult(request));
            //return GetListDataAndReturn<Eva_KPIBuildingModel, Eva_KPIBuildingEntity, Eva_KPIBuildingSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Performance);
        }


        [HttpPost]
        public ActionResult ExportKPIBuildingList([DataSourceRequest] DataSourceRequest request, Eva_KPIBuildingSearchModel model)
        {
            return ExportAllAndReturn<Eva_KPIBuildingEntity, Eva_KPIBuildingModel, Eva_KPIBuildingSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_Performance);
        }


        public ActionResult ExportKPIBuildingSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Eva_KPIBuildingEntity, Eva_KPIBuildingModel>(selectedIds, valueFields, ConstantSql.hrm_eva_sp_get_PerformanceByIds);
        }

        #endregion
        [HttpPost]
        public ActionResult UpdatePerformanceExtendIDOfPerformanceV2(Guid PerformanceExtendID, Guid PerformanceID)
        {
            var service = new Eva_PerformanceServices();
            service.UpdatePerformanceExtendIDOfPerformance(PerformanceExtendID, PerformanceID);
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPerformanceForDetailByPerformanceID([DataSourceRequest] DataSourceRequest request, Eva_PerformanceForDetailSearchModel model)
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
            var status = string.Empty;
            var listEntity = service.GetData<Eva_PerformanceForDetailModel>(lstModel, ConstantSql.hrm_eva_sp_get_PerformanceForDetailByPerformanceId, ref status);
            listEntity = listEntity.OrderBy(d => d.NameEntityName).ToList();
            int stt = 1;
            for (int i = 0; i < listEntity.Count; i++)
            {
                listEntity[i].Stt = stt++;
                if (i != listEntity.Count - 1 && listEntity[i].NameEntityName != listEntity[i+1].NameEntityName)
                {
                    stt = 1;
                }
            }
            
            listEntity = listEntity.OrderBy(m => m.OrderNumber).ToList();

            return Json(listEntity.ToDataSourceResult(request, ModelState));

            //return GetListDataAndReturn<Eva_PerformanceForDetailModel, Eva_PerformanceForDetailEntity, Eva_PerformanceForDetailSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceForDetailByPerformanceId);

        }
        [HttpPost, ValidateInput(true)]
        public ActionResult SubmitKPIforPerformance([Bind(Prefix = "PerformanceID")]Guid PerformanceID, [Bind(Prefix = "models")] List<Eva_KPIModel> Eva_KPIModels)
        {
            var service = new Eva_PerformanceServices();
            string rs = null;
            List<Eva_KPIEntity> list = Eva_KPIModels.Translate<Eva_KPIEntity>();
            rs = service.SubmitKPIforPerformance(PerformanceID, list);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult ApproveKPIforPerformance([Bind(Prefix = "PerformanceID")]Guid PerformanceID, [Bind(Prefix = "models")] List<Eva_KPIModel> Eva_KPIModels)
        {
            var service = new Eva_PerformanceServices();
            string rs = null;
            List<Eva_KPIEntity> list = Eva_KPIModels.Translate<Eva_KPIEntity>();
            rs = service.ApproveKPIforPerformance(PerformanceID, list);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        #region Eva_EvalutionData
        public ActionResult SummaryEvalutionData([DataSourceRequest] DataSourceRequest request, Eva_EvalutionDataSearchModel model)
        {
            var evaServices = new Eva_ReportServices();
            var result = new List<Eva_EvalutionDataModel>();
            var listEntity = evaServices.SummaryEvalutionData(model.YearEvalution, model.TimesGetDataID, model.OrgStructureID, model.DateStart, model.DateEnd,UserLogin);
            if (listEntity != null)
            {
                result = listEntity.Translate<Eva_EvalutionDataModel>();
            }
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEvalutionDataValidation([DataSourceRequest] DataSourceRequest request, Eva_EvalutionDataSearchModel model)
        {
            var evaServices = new Eva_EvalutionDataServices();
            string status = string.Empty;
            #region xoa dl old
            //DateTime _year = new DateTime(model.YearEvalution, 01, 01);
            //List<object> obj = new List<object>();
            //obj.Add(model.OrgStructureID);
            //obj.Add(_year);
            //obj.Add(model.Time);
            //var lstEva_EvalutionData = evaServices.GetData<Eva_EvalutionDataEntity>(obj, ConstantSql.hrm_eva_getdata_EvalutionData, ref status).ToList();
            //if (lstEva_EvalutionData.Count > 0)
            //{

            //    if (lstEva_EvalutionData.Count > 0)
            //    {
            //        ActionService service = new ActionService(UserLogin);
            //        int _total = lstEva_EvalutionData.Count;
            //        int _totalPage = _total / 200 + 1;
            //        int _pageSize = 200;
            //        var dataReturn = new object();
            //        for (int _page = 1; _page <= _totalPage; _page++)
            //        {
            //            int _skip = _pageSize * (_page - 1);
            //            var _listCurrenPage = lstEva_EvalutionData.Skip(_skip).Take(_pageSize).ToList();
            //            List<Guid> lstEva_EvalutionDataID = _listCurrenPage.Select(s => s.ID).ToList();
            //            string evalutionDataID = DeleteType.Delete.ToString() + "," + string.Join(",", lstEva_EvalutionDataID);
            //            var result = service.DeleteOrRemove<Eva_EvalutionDataEntity, Eva_EvalutionDataModel>(evalutionDataID);
            //        }
            //    }
            //}
            List<object> objpara = new List<object>();
            objpara.AddRange(new object[2]);
            objpara[0] =Common.DotNetToOracle(model.TimesGetDataID.ToString());
            objpara[1] = model.YearEvalution;
            evaServices.UpdateData<Eva_EvalutionDataEntity>(objpara, ConstantSql.hrm_vnr_DeleteEvalutionData, ref status);
            #endregion
            status = evaServices.SaveEvalutionData(model.YearEvalution, model.TimesGetDataID, model.OrgStructureID, model.DateStart, model.DateEnd,UserLogin);
            return Json(status);
        }
        public ActionResult GetEvalutionDataList([DataSourceRequest] DataSourceRequest request, Eva_EvalutionDataSearchModel model)
        {
            return GetListDataAndReturn<Eva_EvalutionDataModel, Eva_EvalutionDataEntity, Eva_EvalutionDataSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_EvalutionData);
            //string status = string.Empty;
            //var service = new ActionService(UserLogin);
            //ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    PageSize = request.PageSize,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request),
            //    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            //};
            //var hreServiceProfile = new Hre_ProfileServices();
            //var lstProfile = hreServiceProfile.GetProfileNameAll();
            //var listEntity = service.GetData<Eva_EvalutionDataEntity>(lstModel, ConstantSql.hrm_eva_sp_get_EvalutionData, ref status);
            //var listModel = new List<Eva_EvalutionDataModel>();
            //if (listEntity != null)
            //{
            //    request.Page = 1;
            //    foreach (var item in listEntity)
            //    {
            //        int _tempC1 = 0;
            //        int _TempC2 = 0;
            //        int _tempC3 = 0;
            //        int _tempC4 = 0;
            //        int _tempC5 = 0;
            //        int _tempC6 = 0;
            //        int _tempC7 = 0;
            //        int _tempC8 = 0;
            //        int _tempC9 = 0;
            //        #region TotalC1C2
            //        if (item.C1 != null)
            //            _tempC1 = item.C1.Value;
            //        if (item.C2 != null)
            //            _TempC2 = item.C2.Value;
            //        if (item.C1 != null || item.C2 != null)
            //            item.TotalC1C2 = _tempC1 + _TempC2;
            //        #endregion
            //        #region TotalC3C4C5C6C7
            //        if (item.C3!=null)
            //            _tempC3=item.C3.Value;
            //        if(item.C4!=null)
            //            _tempC4=item.C4.Value;
            //        if (item.C5 != null)
            //            _tempC5 = item.C5.Value;
            //        if (item.C6 != null)
            //            _tempC6 = item.C6.Value;
            //        if (item.C7 != null)
            //            _tempC7 = item.C7.Value;
            //        if (item.C3 != null || item.C4 != null || item.C5 != null || item.C6 != null || item.C7 != null)
            //            item.TotalC3C4C5C6C7 = _tempC3 + _tempC4 + _tempC5 + _tempC6 + _tempC7;
            //        #endregion
            //        #region TotalC1C2C3C4C5C6C7
            //        if (item.C1!=null||item.C2!=null||item.C3 != null || item.C4 != null || item.C5 != null || item.C6 != null || item.C7 != null)
            //            item.TotalC1C2C3C4C5C6C7 =_tempC1 +_TempC2+ _tempC3 + _tempC4 + _tempC5 + _tempC6 + _tempC7;
            //        #endregion
            //        #region  TotalC8C9
            //        if (item.C8!=null)
            //            _tempC8=item.C8.Value;
            //        if(item.C9!=null)
            //            _tempC9=item.C9.Value;
            //        if (item.C8 != null || item.C9 != null)
            //            item.TotalC8C9 = _tempC8 + _tempC9;
            //        #endregion                    
            //        var newModle = (Eva_EvalutionDataModel)typeof(Eva_EvalutionDataModel).CreateInstance();
            //        foreach (var property in item.GetType().GetProperties())
            //        {
            //            newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
            //        }
            //        listModel.Add(newModle);
            //    }
            //    var dataSourceResult = listModel.ToDataSourceResult(request);
            //    if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
            //    {
            //        dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
            //    }
            //    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            //}
            //var listModelNull = new List<Eva_EvalutionDataModel>();
            //ModelState.AddModelError("Id", status);
            //return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetExportEvalutionDataByTemplate([DataSourceRequest] DataSourceRequest request, Eva_EvalutionDataSearchModel model)
        {
            var service = new Eva_ReportServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)

            };
            var status = string.Empty;
            //var result = service.GetEvalutionDataByTemplate(model.YearEvalution,model.Time,model.OrgStructureID);
            var result = service.GetReportEvalutionData(model.YearEvalution, model.TimesGetDataID, model.OrgStructureID, model.IsCreateTemplate, model.DateStart, model.DateEnd, null, null,null,null,null,null,UserLogin);
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
                    FileName = "Eva_ReportEvalutionDataEntity",
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

        #region BC tong hop dl danh gia
        public ActionResult GetReportEvalutionData([DataSourceRequest] DataSourceRequest request, Eva_ReportEvalutionDataModel model)
        {
            var service = new Eva_ReportServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)

            };
            model.DateFrom = new DateTime(model.HistoryYearEvalution, 1, 1);
            model.DateTo = new DateTime(model.YearEvalution, 1, 1);

            var result = service.GetReportEvalutionData(model.YearEvalution, model.TimesGetDataID, model.OrgStructureID, model.IsCreateTemplate, model.DateStart, model.DateEnd, model.DateFrom, model.DateTo,model.JobTitleID,model.PositionID,model.WorkPlaceID,model.RankID,UserLogin);

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
                    FileName = "Eva_ReportEvalutionDataEntity",
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

        [HttpPost]
        public ActionResult ExportKPIBuildingListPersonal([DataSourceRequest] DataSourceRequest request, Eva_PerformanceExportModel model)
        {
            string status = string.Empty;
            var service = new Eva_PerformanceExtendServices();
            var hrService = new Hre_ProfileServices();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Eva_PerformanceExtendEntity(),
                    FileName = "Eva_PerformanceExtendEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var ids = model.PerformanceID.Split(',').ToList();
            string perID = ids[0];
            //string perID = Common.DotNetToOracle(ids[0]);
            //var result = hrService.GetData<Hre_AccidentEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIsBackList, ref status).ToList().Translate<Hre_AccidentModel>();
            var lstResult = new List<Eva_PerformanceExtendEntity>();
            var result = service.GetPerformanceExtendByPerID(perID,UserLogin);
            lstResult.Add(result);

            if (model.ExportId != Guid.Empty)
            {
                if (lstResult.Count > 0)
                {
                    foreach (var item in lstResult)
                    {
                        item.V1 = (item.V1 != null && item.V1 != "") ? EnumDropDown.GetEnumDescription<Mobility>((Mobility)Enum.Parse(typeof(Mobility), item.V1, true)) : string.Empty;
                        item.V8 = (item.V8 != null && item.V8 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V8, true)) : string.Empty;
                        item.V9 = (item.V9 != null && item.V9 != "") ? EnumDropDown.GetEnumDescription<Mobility>((Mobility)Enum.Parse(typeof(Proficiency), item.V9, true)) : string.Empty;
                        item.V10 = (item.V10 != null && item.V10 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V10, true)) : string.Empty;
                        item.V11 = (item.V11 != null && item.V11 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V11, true)) : string.Empty;
                        item.V12 = (item.V12 != null && item.V12 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V12, true)) : string.Empty;
                        item.V15 = (item.V15 != null && item.V15 != "") ? EnumDropDown.GetEnumDescription<ThisYear>((ThisYear)Enum.Parse(typeof(ThisYear), item.V15, true)) : string.Empty;
                        item.V16 = (item.V16 != null && item.V16 != "") ? EnumDropDown.GetEnumDescription<ThisYear>((ThisYear)Enum.Parse(typeof(ThisYear), item.V16, true)) : string.Empty;
                        item.V22 = (item.V22 != null && item.V22 != "") ? EnumDropDown.GetEnumDescription<RiskToLose>((RiskToLose)Enum.Parse(typeof(RiskToLose), item.V22, true)) : string.Empty;
                        item.V23 = (item.V23 != null && item.V23 != "") ? EnumDropDown.GetEnumDescription<UrgencyToMove>((UrgencyToMove)Enum.Parse(typeof(UrgencyToMove), item.V23, true)) : string.Empty;
                        item.V25 = (item.V25 != null && item.V25 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V25, true)) : string.Empty;
                        item.V26 = (item.V26 != null && item.V26 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V26, true)) : string.Empty;
                        item.V27 = (item.V27 != null && item.V27 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V27, true)) : string.Empty;
                        item.V28 = (item.V28 != null && item.V28 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V28, true)) : string.Empty;
                        item.V29 = (item.V29 != null && item.V29 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V29, true)) : string.Empty;
                        item.V35 = (item.V35 != null && item.V35 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V35, true)) : string.Empty;
                        item.V36 = (item.V36 != null && item.V36 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V36, true)) : string.Empty;
                        item.V37 = (item.V37 != null && item.V37 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V37, true)) : string.Empty;
                        item.V38 = (item.V38 != null && item.V38 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V38, true)) : string.Empty;
                    }
                    //lstResult.ForEach(s => s.V1 = EnumDropDown.GetEnumDescription<Mobility>((Mobility) Enum.Parse(typeof(Mobility), s.V1,true)));

                    //Mobility 1
                    //Proficiency 8-9-10-11-12
                    //ThisYear 15-16
                    //Option 25-26-27-28-29
                    //MeasureType 35-36-37-38
                    //RiskToLose 22
                    //UrgencyToMove 23

                }
                var fullPath = ExportService.Export(model.ExportId, lstResult, model.ExportType);

                return Json(fullPath);
            }
            return Json(lstResult.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult ExportPerformanceEvaDetailListPersonal([DataSourceRequest] DataSourceRequest request, Eva_PerformanceExportModel model)
        {
            string status = string.Empty;
            var service = new Eva_PerformanceExtendServices();
            var hrService = new Hre_ProfileServices();
            var lstEva_PerformanceExtendTemplateEntity = new List<Eva_PerformanceExtendTemplateEntity>();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Eva_PerformanceExtendTemplateEntity(),
                    FileName = "Eva_PerformanceExtendTemplateEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var ids = model.PerformanceID.Split(',').ToList();
            string perID = ids[0];

            var actService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(perID));
            var result = actService.GetData<Eva_PerformanceEvaDetailEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailByPerformanceEvaID, ref status);

            var performanceServices = new Eva_PerformanceServices();
            var objPerformance = new List<object>();
            objPerformance.AddRange(new object[14]);
            objPerformance[12] = 1;
            objPerformance[13] = int.MaxValue - 1;
            var lstPerformance = actService.GetData<Eva_KPIBuildingModel>(objPerformance, ConstantSql.hrm_eva_sp_get_Performance, ref status);
            var performanceEvaEntity = actService.GetData<Eva_PerformanceEvaEntity>(Common.DotNetToOracle(perID), ConstantSql.hrm_eva_sp_get_PerformanceEvaById, ref status).FirstOrDefault();

            var lstResult = new List<Eva_PerformanceExtendEntity>();
            var result1 = service.GetPerformanceExtendByPerID(performanceEvaEntity.PerformanceID.ToString(),UserLogin);
            lstResult.Add(result1);

            if (lstResult.Count > 0)
            {
                foreach (var item in lstResult)
                {
                    item.V1 = (item.V1 != null && item.V1 != "") ? EnumDropDown.GetEnumDescription<Mobility>((Mobility)Enum.Parse(typeof(Mobility), item.V1, true)) : string.Empty;
                    item.V8 = (item.V8 != null && item.V8 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V8, true)) : string.Empty;
                    item.V9 = (item.V9 != null && item.V9 != "") ? EnumDropDown.GetEnumDescription<Mobility>((Mobility)Enum.Parse(typeof(Proficiency), item.V9, true)) : string.Empty;
                    item.V10 = (item.V10 != null && item.V10 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V10, true)) : string.Empty;
                    item.V11 = (item.V11 != null && item.V11 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V11, true)) : string.Empty;
                    item.V12 = (item.V12 != null && item.V12 != "") ? EnumDropDown.GetEnumDescription<Proficiency>((Proficiency)Enum.Parse(typeof(Proficiency), item.V12, true)) : string.Empty;
                    item.V15 = (item.V15 != null && item.V15 != "") ? EnumDropDown.GetEnumDescription<ThisYear>((ThisYear)Enum.Parse(typeof(ThisYear), item.V15, true)) : string.Empty;
                    item.V16 = (item.V16 != null && item.V16 != "") ? EnumDropDown.GetEnumDescription<ThisYear>((ThisYear)Enum.Parse(typeof(ThisYear), item.V16, true)) : string.Empty;
                    item.V22 = (item.V22 != null && item.V22 != "") ? EnumDropDown.GetEnumDescription<RiskToLose>((RiskToLose)Enum.Parse(typeof(RiskToLose), item.V22, true)) : string.Empty;
                    item.V23 = (item.V23 != null && item.V23 != "") ? EnumDropDown.GetEnumDescription<UrgencyToMove>((UrgencyToMove)Enum.Parse(typeof(UrgencyToMove), item.V23, true)) : string.Empty;
                    item.V25 = (item.V25 != null && item.V25 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V25, true)) : string.Empty;
                    item.V26 = (item.V26 != null && item.V26 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V26, true)) : string.Empty;
                    item.V27 = (item.V27 != null && item.V27 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V27, true)) : string.Empty;
                    item.V28 = (item.V28 != null && item.V28 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V28, true)) : string.Empty;
                    item.V29 = (item.V29 != null && item.V29 != "") ? EnumDropDown.GetEnumDescription<Option>((Option)Enum.Parse(typeof(Option), item.V29, true)) : string.Empty;
                    item.V35 = (item.V35 != null && item.V35 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V35, true)) : string.Empty;
                    item.V36 = (item.V36 != null && item.V36 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V36, true)) : string.Empty;
                    item.V37 = (item.V37 != null && item.V37 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V37, true)) : string.Empty;
                    item.V38 = (item.V38 != null && item.V38 != "") ? EnumDropDown.GetEnumDescription<MeasureType>((MeasureType)Enum.Parse(typeof(MeasureType), item.V38, true)) : string.Empty;
                }

            }

            foreach (var item in result)
            {
                var entity = lstResult.FirstOrDefault();
                var entityResult = new Eva_PerformanceExtendTemplateEntity();
                entityResult = item.CopyData<Eva_PerformanceExtendTemplateEntity>();
                entityResult.CodeEmp = entity.CodeEmp;
                entityResult.ProfileName = entity.ProfileName;
                entityResult.PositionName = entity.PositionName;
                entityResult.DateHire = entity.DateHire;
                entityResult.JobTitleName = entity.JobTitleName;
                entityResult.DateOfBirth = entity.DateOfBirth;
                entityResult.PayrollGroupName = entity.PayrollGroupName;
                entityResult.SupervisorName = entity.SupervisorName;
                entityResult.HighSupervisorName = entity.HighSupervisorName;
                entityResult.WorkPlaceName = entity.WorkPlaceName;
                entityResult.TCountryName = entity.TCountryName;
                entityResult.TProvinceName = entity.TProvinceName;
                entityResult.Channel = entity.Channel;
                entityResult.Region = entity.Region;
                entityResult.Area = entity.Area;
                entityResult.DateOfEffect = entity.DateOfEffect;

                entityResult.V1 = entity.V1;
                entityResult.V2 = entity.V2;
                entityResult.V3 = entity.V3;
                entityResult.V4 = entity.V4;
                entityResult.V5 = entity.V5;
                entityResult.V6 = entity.V6;
                entityResult.V7 = entity.V7;
                entityResult.V8 = entity.V8;
                entityResult.V9 = entity.V9;
                entityResult.V10 = entity.V10;
                entityResult.V11 = entity.V11;
                entityResult.V12 = entity.V12;
                entityResult.V13 = entity.V13;
                entityResult.V14 = entity.V14;
                entityResult.V15 = entity.V15;
                entityResult.V16 = entity.V16;
                entityResult.V17 = entity.V17;
                entityResult.V18 = entity.V18;
                entityResult.V19 = entity.V19;
                entityResult.V20 = entity.V20;
                entityResult.V21 = entity.V21;
                entityResult.V22 = entity.V22;
                entityResult.V23 = entity.V23;
                entityResult.V24 = entity.V24;
                entityResult.V25 = entity.V25;
                entityResult.V26 = entity.V26;
                entityResult.V27 = entity.V27;
                entityResult.V28 = entity.V28;
                entityResult.V29 = entity.V29;
                entityResult.V30 = entity.V30;
                entityResult.V31 = entity.V31;
                entityResult.V32 = entity.V32;
                entityResult.V33 = entity.V33;
                entityResult.V34 = entity.V34;
                entityResult.V35 = entity.V35;
                entityResult.V36 = entity.V36;
                entityResult.V37 = entity.V37;
                entityResult.V38 = entity.V38;
                entityResult.V39 = entity.V39;
                entityResult.V40 = entity.V40;
                entityResult.V41 = entity.V41;
                entityResult.V42 = entity.V42;
                entityResult.V43 = entity.V43;
                entityResult.V44 = entity.V44;
                entityResult.V45 = entity.V45;
                entityResult.V46 = entity.V46;
                entityResult.V47 = entity.V47;
                entityResult.V48 = entity.V48;
                entityResult.V49 = entity.V49;
                entityResult.V50 = entity.V50;
                entityResult.V51 = entity.V51;
                entityResult.V52 = entity.V52;
                entityResult.V53 = entity.V53;
                entityResult.V54 = entity.V54;
                entityResult.V55 = entity.V55;
                entityResult.V56 = entity.V56;
                entityResult.V57 = entity.V57;
                entityResult.V58 = entity.V58;
                entityResult.V59 = entity.V59;
                lstEva_PerformanceExtendTemplateEntity.Add(entityResult);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstEva_PerformanceExtendTemplateEntity, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        #region Kết quả đánh giá V2

        public ActionResult GetPerformanceGeneralList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceGeneralSearchModel model)
        {
            return GetListDataAndReturn<Eva_PerformanceModel, Eva_PerformanceEntity, Eva_PerformanceGeneralSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceGeneral);
        }

        public ActionResult ExportContractWaitingByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Eva_PerformanceEvaServices();
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }

            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstContract = baseService.GetData<Eva_PerformanceEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceGeneralListID, ref status);
            if (lstContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportEva_Performance" + suffix;
            if (lstContract.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var contract in lstContract)
            {
                contract.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                //contract.DateNow_Day = DateTime.Now.Day.ToString();
                //contract.DateNow_Month = DateTime.Now.Month.ToString();
                //contract.DateNow_Year = DateTime.Now.Year.ToString();
                if(contract.Birthday!=null)
                {
                    contract.Birthday = "Ngày" + " " + contract.DayOfBirth + " " + contract.MonthOfBirth + " " + contract.YearOfBirth;  //contract..HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                }

              
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                var actService = new ActionService(UserLogin);
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                List<object> lstObjExport = new List<object>();
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(1);
                lstObjExport.Add(10000000);
                if (!string.IsNullOrEmpty(valueFields))
                    template = actService.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                if (template == null)
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                //if (!System.IO.File.Exists(templatepath))
                //{
                //    messages = "NotTemplate";
                //    return Json(messages, JsonRequestBehavior.AllowGet);
                //}
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstcontract = new List<Eva_PerformanceEntity>();
                lstcontract.Add(contract);
                ExportService.ExportWord(outputPath, templatepath, lstcontract);
            }
            if (lstContract.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }
        public ActionResult ExportGetPerformanceGeneralByTemplate([DataSourceRequest] DataSourceRequest request, Eva_PerformanceGeneralSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Eva_PerformanceModel();
            //var result = GetListData<Eva_PerformanceModel, Hre_AccidentEntity, Eva_PerformanceGeneralSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceGeneral, ref status);
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Eva_PerformanceModel(),
                    FileName = "Eva_Performance",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var baseService = new ActionService(UserLogin);
            List<Eva_PerformanceEntity> lstPerformance = baseService.GetData<Eva_PerformanceEntity>(lstModel, ConstantSql.hrm_eva_sp_get_PerformanceGeneral, ref status);
            Eva_ReportServices evaService=new Eva_ReportServices();
            List<Eva_PerformanceEntity> lstPerformanceEntity = evaService.GetPerformanceGeneralList(lstPerformance, model.OrgStructureID,UserLogin);
            var result = new List<Eva_PerformanceModel>();
            if (lstPerformanceEntity!=null)
            {
                result = lstPerformanceEntity.Translate<Eva_PerformanceModel>();
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ExportPerformanceGeneralList([DataSourceRequest] DataSourceRequest request, Eva_PerformanceGeneralSearchModel model)
        {
            return ExportAllAndReturn<Eva_PerformanceEntity, Eva_PerformanceModel, Eva_PerformanceGeneralSearchModel>(request, model, ConstantSql.hrm_eva_sp_get_PerformanceGeneral);
        }

        public JsonResult ApplyPerformance(string selectedIds)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            string result = ConstantMessages.Succeed;
            var lstperformance = service.GetData<Eva_PerformanceEntity>(Common.DotNetToOracle(selectedIds), ConstantSql.hrm_eva_sp_get_PerformanceByIds, ref status).ToList();
            var performanceservices = new Eva_PerformanceServices();
            foreach (var performance in lstperformance)
            {
                if (performance.ProfileID != null && performance.RankID != null && performance.DateEffect != null)
                {
                    result = performanceservices.ApplyPerformance(performance.ProfileID, performance.RankDetailID, performance.DateEffect,UserLogin);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>xuat word theo template</summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportPerformanceGeneralByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
            {
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            }

            objs.Add(strIDs);
            var lstPerformance = baseService.GetData<Eva_PerformanceModel>(objs, ConstantSql.hrm_eva_sp_get_PerformanceGeneralListID, ref status);
            if (lstPerformance == null)
            {
                return null;
            }
            int i = 0;
            var lstPerformanceID = lstPerformance.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportPerformanceGeneral" + suffix;
            if (lstPerformanceID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            int fileOrderNumber = 0;
            List<Guid> lstProfileIDs=null;
            if(lstPerformance!=null)
                 lstProfileIDs=lstPerformance.Select(s=>s.ProfileID.Value).Distinct().ToList();
            var hreService = new Hre_ProfileServices();
            var lstContract = hreService.GetContractByProfileID(lstProfileIDs).ToList();
           
            foreach (var performance in lstPerformance)
            {
                performance.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (performance.Birthday != null)
                {
                    performance.Birthday = "Ngày" + " " + performance.DayOfBirth +" "+"Tháng"+ " " + performance.MonthOfBirth +" "+"Năm"+ " " + performance.YearOfBirth;  //contract..HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                }
                if(performance.WorkPermitInsDate!=null)
                {
                    performance.WorkPermitInsDateFormat = performance.WorkPermitInsDate.Value.ToString("dd/MM/yyyy");
                    performance.WorkPermitInsDateFormatEN = performance.WorkPermitInsDate.Value.ToString("dd/MMM/yyyy");
                }
                if(lstContract.Count>0)
                {
                    var objContractByProfile = lstContract.Where(s => s.ProfileID == performance.ProfileID).OrderByDescending(s => s.DateSigned).FirstOrDefault();
                    if(objContractByProfile!=null && objContractByProfile.DateSigned!=null)
                    {
                        performance.DateSignedFormat = objContractByProfile.DateSigned.Value.ToString("dd/MM/yyyy");
                        performance.DateSignedFormatEN = objContractByProfile.DateSigned.Value.ToString("dd/MMM/yyyy");
                    }
                }
                string DateNow_Day = DateTime.Now.Day.ToString();
                string DateNow_Month = DateTime.Now.Month.ToString();
               string DateNow_Year = DateTime.Now.Year.ToString();

               performance.DateNowvn = "Ngày" + " " + DateNow_Day + " " + "Tháng" + " " + DateNow_Month +" " + "Năm" + " " + DateNow_Year;  //contract..HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;

                fileOrderNumber++;
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                List<object> lstObjExport = new List<object>();
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(1);
                lstObjExport.Add(10000000);

                if (!string.IsNullOrEmpty(valueFields))
                {
                    template = service.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                }

                if (template == null)
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }

                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(performance.ProfileName) + fileOrderNumber + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(performance.ProfileName) + fileOrderNumber + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lstPers = new List<Eva_PerformanceModel>();
                lstPers.Add(performance);
                ExportService.ExportWord(outputPath, templatepath, lstPers);
            }
            if (lstPerformanceID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            } 
            return Json(fileDoc);
        }
        public void SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
            if (selectedIds.Count>0)
            {
                //string[] lstIds = selectedIds.Split(',').ToArray();
                string strIds="";
                foreach (var item in selectedIds)
                {
                    strIds += Common.DotNetToOracle(item.ToString()) + ",";
                }
                ActionService service = new ActionService(UserLogin);
                string statusMessages = string.Empty;
                List<object> lstObj = new List<object>();
                lstObj.Add(strIds);
                lstObj.Add(status);
                service.UpdateData<Eva_PerformanceEntity>(lstObj, ConstantSql.hrm_eva_sp_Set_Performance_Status, ref statusMessages);
            }
        }
        #endregion

        public ActionResult GetPerformanceResultByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(profileID.ToString()));
            var result = baseService.GetData<Eva_PerformanceEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceResultByProfileID, ref status);
            return Json(result.ToDataSourceResult(request));
        }
    }

}