using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Reflection;
using System.Collections;
using HRM.Business.Main.Domain;
using HRM.Presentation.Service;
using System.Web.Caching;
using System.Web;
using HRM.Presentation.Payroll.Models;
using HRM.Business.Payroll.Models;
using System.Data.SqlTypes;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Presentation.Hr.Models;
using HRM.Business.Evaluation.Models;
using System.Web.Script.Serialization;
using VnResource.Helper.Utility;
using System.Xml;
using System.IO;
using HRM.Business.Payroll.Domain;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.Finance.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Recruitment.Models;
using HRM.Presentation.Recruitment.Models;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Cat_GetDataController : BaseController
    {
        BaseService baseService = new BaseService();
        string _status = string.Empty;
        public JsonResult JoinTimeInDate(DateTime date, string time)
        {
            if (time == null)
                return Json(date, JsonRequestBehavior.AllowGet);

            var _arr = time.Split(':');
            if (_arr[0].ToString().Equals("__"))
                _arr[0] = "00";
            if (_arr[1].ToString().Equals("__"))
                _arr[1] = "00";
            if (_arr[2].ToString().Equals("__"))
                _arr[2] = "00";
            TimeSpan _time = new TimeSpan(int.Parse(_arr[0].ToString()), int.Parse(_arr[1].ToString()), int.Parse(_arr[2].ToString()));
            double _hours = _time.TotalHours;

            return Json(date.AddHours(_hours), JsonRequestBehavior.AllowGet);
        }

        #region Cat_SourceAds
        [HttpPost]
        public JsonResult GetMultiSourceAds(string text)
        {
            return GetDataForControl<Cat_SourceAdsMultiModel, Cat_SourceAdsMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SourceAds_Multi);
        }

        public ActionResult GetSourceAdsList([DataSourceRequest] DataSourceRequest request, Cat_SourceAdsSearchModel model)
        {
            return GetListDataAndReturn<Cat_SourceAdsModel, Cat_SourceAdsEntity, Cat_SourceAdsSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SourceAds);
        }
        public ActionResult ExportSourceAdsSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SourceAdsEntity, Cat_SourceAdsModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SourceAdsByIds);
        }
        public ActionResult ExportAllSourceAdsList([DataSourceRequest] DataSourceRequest request, Cat_SourceAdsSearchModel model)
        {
            return ExportAllAndReturn<Cat_SourceAdsEntity, Cat_SourceAdsModel, Cat_SourceAdsSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SourceAds);
        }
        #endregion
        #region Cat_ExchangeRate
        public ActionResult GetExchangeRateList([DataSourceRequest] DataSourceRequest request, Cat_ExchangeRateSearchModel model)
        {
            return GetListDataAndReturn<Cat_ExchangeRateModel, Cat_ExchangeRateEntity, Cat_ExchangeRateSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ExchangeRate);
        }
        public ActionResult ExportExchangeRateSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ExchangeRateEntity, Cat_ExchangeRateModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ExchangeRateByIds);
        }
        public ActionResult ExportAllExchangeRateList([DataSourceRequest] DataSourceRequest request, Cat_ExchangeRateSearchModel model)
        {
            return ExportAllAndReturn<Cat_ExchangeRateEntity, Cat_ExchangeRateModel, Cat_ExchangeRateSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ExchangeRate);
        }
        #endregion
        #region Cat_Village
        public ActionResult GetVillageList([DataSourceRequest] DataSourceRequest request, Cat_VillageSearchModel model)
        {
            return GetListDataAndReturn<Cat_VillageModel, Cat_VillageEntity, Cat_VillageSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Village);
        }
        public ActionResult ExportVillageSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_VillageEntity, Cat_VillageModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_VillageByIds);
        }
        public ActionResult ExportAllVillageList([DataSourceRequest] DataSourceRequest request, Cat_VillageSearchModel model)
        {
            return ExportAllAndReturn<Cat_VillageEntity, Cat_VillageModel, Cat_VillageSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Village);
        }
        #endregion
        #region Cat_ReceivePlace
        public ActionResult GetListReceivePlace([DataSourceRequest] DataSourceRequest request, Cat_ReceivePlaceSearchModel model)
        {
            return GetListDataAndReturn<Cat_ReceivePlaceModel, Cat_ReceivePlaceEntity, Cat_ReceivePlaceSearchModel>(request, model, ConstantSql.hrm_Cat_SP_GET_RECEIVEPLACE);
        }
        #endregion
        #region Cat_model
        public ActionResult Cat_GetModelModel([DataSourceRequest] DataSourceRequest request, Cat_ModelSearchModel model)
        {
            return GetListDataAndReturn<Cat_ModelModel, Cat_ModelEntity, Cat_ModelSearchModel>(request, model, ConstantSql.hrm_Cat_sp_get_CatModel);
        }

        public ActionResult CatGetColorByModelID([DataSourceRequest] DataSourceRequest request, Guid ModelID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ModelID);
            var result = baseService.GetData<PUR_ColorModelModel>(objs, ConstantSql.hrm_Cat_SP_GET_ColorByModelID, UserLogin, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }
        #endregion

        #region Cat_UnAllowCfgAmount
        public ActionResult GetUnAllowCfgAmountList([DataSourceRequest] DataSourceRequest request, Cat_UnAllowCfgAmountSearchModel model)
        {
            return GetListDataAndReturn<Cat_UnAllowCfgAmountModel, Cat_UnAllowCfgAmountEntity, Cat_UnAllowCfgAmountSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Cat_UnAllowCfgAmount);
        }
        public ActionResult ExportUnAllowCfgAmountSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_UnAllowCfgAmountEntity, Cat_UnAllowCfgAmountModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_UnAllowCfgAmountByIds);
        }
        public ActionResult ExportAllUnAllowCfgAmountList([DataSourceRequest] DataSourceRequest request, Cat_UnAllowCfgAmountSearchModel model)
        {
            return ExportAllAndReturn<Cat_UnAllowCfgAmountEntity, Cat_UnAllowCfgAmountModel, Cat_UnAllowCfgAmountSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Cat_UnAllowCfgAmount);
        }
        #endregion


        #region Cat_DataGroup
        public ActionResult GetDataGroupList([DataSourceRequest] DataSourceRequest request, Cat_DataGroupSearchModel model)
        {
            return GetListDataAndReturn<Cat_DataGroupModel, Cat_DataGroupEntity, Cat_DataGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DataGroup);
        }
        public ActionResult ExportDataGroupSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_DataGroupEntity, Cat_DataGroupModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_DataGroupByIds);
        }
        public ActionResult ExportAllDataGroupList([DataSourceRequest] DataSourceRequest request, Cat_DataGroupSearchModel model)
        {
            return ExportAllAndReturn<Cat_DataGroupEntity, Cat_DataGroupModel, Cat_DataGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DataGroup);
        }
        #endregion
        #region Cat_DataGroupDetail

        public ActionResult ExportDataGroupDetailSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_DataGroupDetailEntity, Cat_DataGroupDetailModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_DataGroupByIds);
        }
        public ActionResult GetDataGroupDetailByDataGroupID([DataSourceRequest] DataSourceRequest request, Guid dataGroupID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(dataGroupID);
            var result = baseService.GetData<Cat_DataGroupDetailModel>(objs, ConstantSql.hrm_cat_sp_get_DataGroupDetailByDTGroupID, UserLogin, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }

        public JsonResult GetMultiDataGroup(string text)
        {
            return GetDataForControl<Cat_DataGroupMultiModel, Cat_DataGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_DataGroup_multi);
        }

        public JsonResult GetMultiMasterDataGroup(string text)
        {
            return GetDataForControl<Cat_MasterDataGroupMultiModel, Cat_MasterDataGroupMultiModel>(text, ConstantSql.hrm_cat_sp_get_MasterDataGroup_multi);
        }
        #endregion

        #region Cat_TrainLevel
        public ActionResult GetTrainLevelList([DataSourceRequest] DataSourceRequest request, Cat_TrainLevelSearchModel model)
        {
            return GetListDataAndReturn<Cat_TrainLevelModel, Cat_TrainLevelEntity, Cat_TrainLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TrainLevel);
        }
        public ActionResult ExportTrainLevelSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_TrainLevelEntity, Cat_TrainLevelModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_TrainLevelByIds);
        }
        public ActionResult ExportAllTrainLevelList([DataSourceRequest] DataSourceRequest request, Cat_TrainLevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_TrainLevelEntity, Cat_TrainLevelModel, Cat_TrainLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TrainLevel);
        }

        public JsonResult GetMultiTrainLevel(string text)
        {
            return GetDataForControl<Cat_TrainLevelMultiModel, Cat_TrainLevelMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TrainLevel_Multi);
        }
        #endregion

        #region Cat_TrainCategory
        public ActionResult GetTrainCategoryList([DataSourceRequest] DataSourceRequest request, Cat_TrainCategorySearchModel model)
        {
            return GetListDataAndReturn<Cat_TrainCategoryModel, Cat_TrainCategoryEntity, Cat_TrainCategorySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TrainCategory);
        }
        public ActionResult ExportTrainCategorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_TrainCategoryEntity, Cat_TrainCategoryModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_TrainCategoryByIds);
        }
        public ActionResult ExportAllTrainCategoryList([DataSourceRequest] DataSourceRequest request, Cat_TrainCategorySearchModel model)
        {
            return ExportAllAndReturn<Cat_TrainCategoryEntity, Cat_TrainCategoryModel, Cat_TrainCategorySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TrainCategory);
        }

        public JsonResult GetMultiTrainCategory(string text)
        {
            return GetDataForControl<Cat_TrainCategoryMultiModel, Cat_TrainCategoryMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TrainCategory_Multi);
        }
        #endregion

        #region Cat_Owner
        public ActionResult GetOwnerList([DataSourceRequest] DataSourceRequest request, Cat_OwnerSearchModel model)
        {
            return GetListDataAndReturn<Cat_OwnerModel, Cat_OwnerEntity, Cat_OwnerSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Owner);
        }
        public ActionResult ExportOwnerSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_OwnerEntity, Cat_OwnerModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_OwnerByIds);
        }
        public ActionResult ExportAllOwnerList([DataSourceRequest] DataSourceRequest request, Cat_OwnerSearchModel model)
        {
            return ExportAllAndReturn<Cat_OwnerEntity, Cat_OwnerModel, Cat_OwnerSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Owner);
        }
        public JsonResult GetMultiOwner(string text)
        {
            return GetDataForControl<Cat_OwnerMultiModel, Cat_OwnerMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Owner_Multi);
        }

        public JsonResult GetMultiFunction(string text)
        {
            return GetDataForControl<Cat_OwnerMultiModel, Cat_OwnerMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Function_Multi);
        }


        public JsonResult GetBudgetOwnerCascading(Guid functionID, string provinceFilter)
        {
            var result = new List<Fin_PurchaseRequestModel>();
            string status = string.Empty;
            if (functionID != Guid.Empty)
            {
                var service = new Cat_OwnerServices();
                result = service.GetData<Fin_PurchaseRequestModel>(functionID, ConstantSql.hrm_cat_sp_get_BudgetByFunctionId, UserLogin, ref status);
                //if (!string.IsNullOrEmpty(provinceFilter))
                //{
                //    var rs = result.Where(s => s.ProvinceName != null && s.ProvinceName.ToLower().Contains(provinceFilter.ToLower())).ToList();

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetChannelCascading(Guid budgetOwnerID, string provinceFilter)
        {
            var result = new List<Fin_PurchaseRequestModel>();
            string status = string.Empty;
            if (budgetOwnerID != Guid.Empty)
            {
                var service = new Cat_OwnerServices();
                result = service.GetData<Fin_PurchaseRequestModel>(budgetOwnerID, ConstantSql.hrm_cat_sp_get_ChannelByBudgetOwnerId, UserLogin, ref status);
                //if (!string.IsNullOrEmpty(provinceFilter))
                //{
                //    var rs = result.Where(s => s.ProvinceName != null && s.ProvinceName.ToLower().Contains(provinceFilter.ToLower())).ToList();

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostCentreCascading(Guid budgetOwnerID, string provinceFilter)
        {
            var result = new List<Fin_PurchaseRequestModel>();
            string status = string.Empty;
            if (budgetOwnerID != Guid.Empty)
            {
                var serivces = new ActionService(UserLogin);
                var entity = serivces.GetByIdUseStore<Fin_PurchaseRequestModel>(budgetOwnerID, ConstantSql.hrm_cat_sp_get_OwnerByIds, ref status);
                var cateService = new Cat_CateCodeServices();
                var lstObj = new List<object>();
                lstObj.Add(null);
                lstObj.Add(null);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue - 1);
                var lstCate = cateService.GetData<Cat_CateCodeModel>(lstObj, ConstantSql.hrm_cat_sp_get_CateCode, UserLogin, ref status);
                if (entity != null)
                {
                    if (entity.BudgetOwnerName == "EUCERIN")
                    {
                        var lstCateCode = lstCate.Where(s => s.CateCodeType == entity.BudgetOwnerName);
                        return Json(lstCateCode, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var lstCateCodes = lstCate.Where(s => s.CateCodeType != "EUCERIN");
                        return Json(lstCateCodes, JsonRequestBehavior.AllowGet);
                    }

                }
                //if (!string.IsNullOrEmpty(provinceFilter))
                //{
                //    var rs = result.Where(s => s.ProvinceName != null && s.ProvinceName.ToLower().Contains(provinceFilter.ToLower())).ToList();

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemCascading(Guid budgetOwnerID, string provinceFilter)
        {
            var result = new List<Fin_PurchaseRequestModel>();
            string status = string.Empty;
            if (budgetOwnerID != Guid.Empty)
            {
                var itemServices = new Cat_PurchaseItemsServices();
                var lstObj = new List<object>();
                lstObj.Add(null);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue - 1);
                var lstPurchaseItem = itemServices.GetData<Cat_PurchaseItemsModel>(lstObj, ConstantSql.hrm_cat_sp_get_PurchaseItems, UserLogin, ref status);
                lstPurchaseItem.Where(s => s.OwnerID == budgetOwnerID);
                return Json(lstPurchaseItem, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectCascading(Guid budgetOwnerID, string provinceFilter)
        {
            var result = new List<Cat_MasterProjectModel>();
            string status = string.Empty;
            if (budgetOwnerID != Guid.Empty)
            {
                var serivces = new ActionService(UserLogin);
                var entity = serivces.GetByIdUseStore<Fin_PurchaseRequestModel>(budgetOwnerID, ConstantSql.hrm_cat_sp_get_OwnerByIds, ref status);

                var ProjectServices = new Cat_MasterProjectServices();
                var lstObj = new List<object>();
                lstObj.Add(null);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue - 1);
                var lstMasterProject = ProjectServices.GetData<Cat_MasterProjectModel>(lstObj, ConstantSql.hrm_cat_sp_get_MasterProject, UserLogin, ref status);
                if (entity != null)
                {
                    if (entity.BudgetOwnerName != "EUCERIN")
                    {
                        var lstMasterProjects = lstMasterProject.Where(s => s.Type != "EUCERIN");
                        return Json(lstMasterProjects, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #region Cat_CateCode

        public ActionResult GetCateCodeList([DataSourceRequest] DataSourceRequest request, Cat_CateCodeSearchModel model)
        {
            return GetListDataAndReturn<Cat_CateCodeModel, Cat_CateCodeEntity, Cat_CateCodeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CateCode);
        }
        public ActionResult ExportCateCodeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CateCodeEntity, Cat_CateCodeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_CateCodeByIds);
        }
        public ActionResult ExportAllCateCodeList([DataSourceRequest] DataSourceRequest request, Cat_CateCodeSearchModel model)
        {
            return ExportAllAndReturn<Cat_CateCodeEntity, Cat_CateCodeModel, Cat_CateCodeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CateCode);
        }



        #endregion
        #region Cat_MasterProject
        public ActionResult GetMasterProjectList([DataSourceRequest] DataSourceRequest request, Cat_MasterProjectSearchModel model)
        {
            return GetListDataAndReturn<Cat_MasterProjectModel, Cat_MasterProjectEntity, Cat_MasterProjectSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_MasterProject);
        }
        public ActionResult ExportMasterProjectSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_MasterProjectEntity, Cat_MasterProjectModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_MasterProjectByIds);
        }
        public ActionResult ExportAllMasterProjectList([DataSourceRequest] DataSourceRequest request, Cat_MasterProjectSearchModel model)
        {
            return ExportAllAndReturn<Cat_MasterProjectEntity, Cat_MasterProjectModel, Cat_MasterProjectSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_MasterProject);
        }
        #endregion

        #region Cat_RelativeType
        public JsonResult GetMultiRelativeType(string text)
        {
            return GetDataForControl<CatRelativeTypeModel, Cat_RelativeTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_RelativeType_multi);
        }
        #endregion

        #region Cat_DisciplinedTypes
        public JsonResult GetMultiDisciplinedTypes(string text)
        {
            return GetDataForControl<Cat_DisciplinedTypesMultiModel, Cat_DisciplinedTypesMultiEntity>(text, ConstantSql.hrm_cat_sp_get_DisciplinedTypes_multi);
        }
        public ActionResult GetDisciplinedTypesList([DataSourceRequest] DataSourceRequest request, Cat_DisciplinedTypesSearchModel model)
        {
            return GetListDataAndReturn<Cat_DisciplinedTypesModel, Cat_DisciplinedTypesEntity, Cat_DisciplinedTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DisciplinedTypes);
        }

        public ActionResult ExportAllDisciplinedTypesList([DataSourceRequest] DataSourceRequest request, Cat_DisciplinedTypesSearchModel model)
        {
            return ExportAllAndReturn<Cat_DisciplinedTypesEntity, Cat_DisciplinedTypesModel, Cat_DisciplinedTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DisciplinedTypes);
        }

        #endregion

        #region Cat_Shift
        public JsonResult GetMultiShift(string text)
        {

            return GetDataForControl<CatShiftMultiModel, Cat_ShiftMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Shift_multi);
        }

        public ActionResult GetShiftItemByShiftIDList([DataSourceRequest] DataSourceRequest request, Guid ShiftID)
        {
            List<Cat_ShiftItemEntity> lstShitItemEntity = new List<Cat_ShiftItemEntity>();
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ShiftID);
            var result = baseService.GetData<Cat_ShiftItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ShiftItemByShiftID, UserLogin, ref status);
            if (result != null)
            {

                foreach (var item in result)
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Intime.Value.Hour, item.Intime.Value.Minute, item.Intime.Value.Second);
                    item.From = temp.AddHours(item.CoFrom);
                    //  item.To = DateTime.Now;
                    item.To = temp.AddHours(item.CoTo);
                    lstShitItemEntity.Add(item);
                }
            }
            return Json(lstShitItemEntity.ToDataSourceResult(request));
        }

        public ActionResult GetShiftOvertimeItemByShiftIDList([DataSourceRequest] DataSourceRequest request, Guid ShiftID)
        {
            List<Cat_ShiftItemEntity> lstShitItemEntity = new List<Cat_ShiftItemEntity>();
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ShiftID);
            var result = baseService.GetData<Cat_ShiftItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ShiftItemOvertimeByShiftID, UserLogin, ref status);
            if (result != null)
            {
                foreach (var item in result)
                {
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Intime.Value.Hour, item.Intime.Value.Minute, item.Intime.Value.Second);
                    item.From = temp.AddHours(item.CoFrom);
                    //  item.To = DateTime.Now;
                    item.To = temp.AddHours(item.CoTo);
                    lstShitItemEntity.Add(item);
                }
            }
            return Json(lstShitItemEntity.ToDataSourceResult(request));
        }
        #endregion

        #region ElementType
        public JsonResult GetElementType(string text)
        {

            return GetDataForControl<Sal_CostCentreSalElementTypeMultiModel, Cat_ElementMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Element_Multi);

        }
        public JsonResult GetElementTypePaidLeave(string text)
        {
            if (text == null || text == string.Empty)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                //text = "Paid";
                var listEntity = baseService.GetData<Cat_ElementMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Element_Multi, UserLogin, ref status).ToList();

                if (listEntity != null)
                {
                    listEntity = listEntity.Where(s => s.ElementCode == "PaidLeaveAndGoodHealth").ToList();
                    List<Sal_CostCentreSalElementTypeMultiModel> listModel = listEntity.Translate<Sal_CostCentreSalElementTypeMultiModel>();
                    return Json(listModel, JsonRequestBehavior.AllowGet);
                }
            }
            return GetDataForControl<Sal_CostCentreSalElementTypeMultiModel, Cat_ElementMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Element_Multi);
        }
        public JsonResult GetElementTypeBonusEvaluation(string text)
        {
            if (text == null || text == string.Empty)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                text = "Bonus";
                var listEntity = baseService.GetData<Cat_ElementMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Element_Multi, UserLogin, ref status).ToList();

                if (listEntity != null)
                {
                    listEntity = listEntity.Where(s => s.ElementCode == "BonusEvaluation").ToList();
                    List<Sal_CostCentreSalElementTypeMultiModel> listModel = listEntity.Translate<Sal_CostCentreSalElementTypeMultiModel>();
                    return Json(listModel, JsonRequestBehavior.AllowGet);
                }
            }
            return GetDataForControl<Sal_CostCentreSalElementTypeMultiModel, Cat_ElementMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Element_Multi);
        }
        #endregion



        #region cat_Orgstructure
        public JsonResult GetRegionByOrgStructureID(Guid ID, Guid? profileID, string Check)
        {
            if (Check == null)
            {
                ActionService Services = new ActionService(UserLogin);
                string status = string.Empty;
                var DataOrg = Services.GetByIdUseStore<Cat_OrgStructureEntity>(ID, ConstantSql.hrm_cat_sp_get_OrgStructureById, ref status);
                Hre_ProfileEntity DataProfile;
                Cat_RegionMultiEntity DataRegion;
                if (DataOrg.RegionID != null)
                {
                    if (profileID != null)
                    {
                        status = string.Empty;
                        DataProfile = Services.GetByIdUseStore<Hre_ProfileEntity>(profileID.Value, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                        if (DataProfile.RegionID == Guid.Empty)
                        {
                            DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataOrg.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                            return Json(DataRegion, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataProfile.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                            return Json(DataRegion, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataOrg.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                        return Json(DataRegion, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(GetDataForControl<CatRegionMultiModel, Cat_RegionMultiEntity>("", ConstantSql.hrm_cat_sp_get_Region_multi), JsonRequestBehavior.AllowGet);

            }
            return Json(null);
        }

        public JsonResult GetRegionByWorkPlaceID(Guid ID, Guid? profileID, string Check)
        {
            if (Check == null)
            {
                ActionService Services = new ActionService(UserLogin);
                string status = string.Empty;
                var DataWpl = Services.GetByIdUseStore<Cat_WorkPlaceEntity>(ID, ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status);
                Hre_ProfileEntity DataProfile;
                Cat_RegionMultiEntity DataRegion;
                if (DataWpl.RegionID != null)
                {
                    if (profileID != null)
                    {
                        DataProfile = Services.GetByIdUseStore<Hre_ProfileEntity>(profileID.Value, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                        if (DataProfile.RegionID == Guid.Empty)
                        {
                            DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataWpl.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                            return Json(DataRegion, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataProfile.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                            return Json(DataRegion, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        DataRegion = Services.GetByIdUseStore<Cat_RegionMultiEntity>(DataWpl.RegionID.Value, ConstantSql.hrm_cat_sp_get_RegionById, ref status);
                        return Json(DataRegion, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(GetDataForControl<CatRegionMultiModel, Cat_RegionMultiEntity>("", ConstantSql.hrm_cat_sp_get_Region_multi), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }
        #endregion


        #region Cat_OrgStructureType


        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng OrgStructureType (Cat_OrgStructureType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrgStructureTypeList([DataSourceRequest] DataSourceRequest request, CatOrgStructureTypeSearchModel model)
        {
            return GetListDataAndReturn<CatOrgStructureTypeModel, Cat_OrgStructureTypeEntity, CatOrgStructureTypeSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_OrgStructureType);
        }

        public JsonResult GetMultiOrgStructureType(string text)
        {
            return GetDataForControl<CatOrgStructureTypeMultiModel, Cat_OrgStructureTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_OrgStructureType_multi);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho OrgStructureType(OrgStructureType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllOrgStructureTypeList([DataSourceRequest] DataSourceRequest request, CatOrgStructureTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_OrgStructureTypeEntity, CatOrgStructureTypeModel, CatOrgStructureTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_OrgStructureType);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn OrgStructureType (OrgStructureType) theo điều kiện tìm kiếm
        public ActionResult ExportOrgStructureTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_OrgStructureTypeEntity, CatOrgStructureTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }

        #endregion

        #region Cat_OrgStructure
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng OrgStructure (Cat_OrgStructure)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetOrgStructureList([DataSourceRequest] DataSourceRequest request, CatOrgStructureSearchModel model)
        {
            return GetListDataAndReturn<CatOrgStructureModel, Cat_OrgStructureEntity, CatOrgStructureSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_AllOrgStructure);
        }


        /// [Tho.Bui] - Xuất danh sách dữ liệu cho OrgStructure(OrgStructure) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllOrgStructureList([DataSourceRequest] DataSourceRequest request, CatOrgStructureSearchModel model)
        {
            return ExportAllAndReturn<Cat_OrgStructureEntity, CatOrgStructureModel, CatOrgStructureSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AllOrgStructure);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn OrgStructure (OrgStructure) theo điều kiện tìm kiếm
        public ActionResult ExportOrgStructureSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_OrgStructureEntity, CatOrgStructureModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }

        public JsonResult GetMultiOrgStructure(string text)
        {
            return GetDataForControl<CatOrgStructureMultiModel, Cat_OrgStructureMultiEntity>(text, ConstantSql.hrm_cat_sp_get_OrgStructure_multi);

        }

        public JsonResult GetMultiOrgStructure_Cascading(string text)
        {
            //return GetDataForControl<CatOrgStructureMultiModel, Cat_OrgStructureMultiEntity>(text, ConstantSql.hrm_cat_sp_get_OrgStructure_multi);
            var arrText = text.Split('|').ToList();

            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[3]);
            lstParam[0] = (arrText[0] != null && arrText[0] != string.Empty) ? arrText[0] : null;
            lstParam[1] = (arrText[1] != null && arrText[1] != string.Empty) ? arrText[1] : null;
            lstParam[2] = (arrText[2] != null && arrText[2] != string.Empty) ? arrText[2] : null;


            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_OrgStructureMultiEntity>(lstParam, ConstantSql.hrm_cat_sp_get_OrgStructure_Cascading, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatOrgStructureMultiModel> listModel = listEntity.Translate<CatOrgStructureMultiModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        #region Cat_ShopGroup
        [HttpPost]
        public ActionResult GetShopGroupList([DataSourceRequest] DataSourceRequest request, Cat_ShopGroupSearchModel model)
        {
            return GetListDataAndReturn<Cat_ShopGroupModel, Cat_ShopGroupEntity, Cat_ShopGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ShopGroup);
        }
        public JsonResult GetMultiShopGroup(string text)
        {
            return GetDataForControl<Cat_ShopGroupMultiModel, Cat_ShopGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ShopGroup_multi);
        }

        public ActionResult ExportAllCat_ShopGroupList([DataSourceRequest] DataSourceRequest request, Cat_ShopGroupSearchModel model)
        {
            return ExportAllAndReturn<Cat_ShopGroupEntity, Cat_ShopGroupModel, Cat_ShopGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ShopGroup);
        }
        public ActionResult ExportCat_ShopGroupSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ShopGroupEntity, Cat_ShopGroupModel>(selectedIds, valueFields, ConstantSql.hrm_Cat_sp_get_ShopGroupIds);
        }
        #endregion


        public ActionResult CheckExistProfileInOrgStructure(List<Guid> selectedIds)
        {
            string status = string.Empty;
            string message = string.Empty;
            var service = new Cat_OrgStructureServices();
            var listModel = new List<CatOrgStructureModel>();
            var listEntity = service.GetDataNotParam<Cat_OrgStructureTreeViewEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure_Data_SumProfile, UserLogin, ref status);
            listModel = listEntity.Translate<CatOrgStructureModel>();
            for (int i = 0; i < selectedIds.Count; i++)
            {
                if (GetCountProfile(listModel, selectedIds[i], new int[2])[0] <= 0)
                {
                    //delete
                    message += service.Remove<Cat_OrgStructureEntity>(selectedIds[i]) + ",";

                }
                else
                {
                    message += listModel.Single(m => m.ID == selectedIds[i]).OrgStructureName + ",";

                }
            }
            message = message.Substring(0, message.LastIndexOf(','));

            return Json(message, JsonRequestBehavior.AllowGet);
            //var hrService = new Hre_ProfileServices();
            //var lstObj = new List<object>();
            //lstObj.Add(selectedIds);
            //lstObj.Add(null);
            //lstObj.Add(null);
            //var lstProfile = hrService.GetData<Hre_ProfileModel>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            //if(lstProfile.Count == 0 || lstProfile == null){
            //    rs = true;
            //}





        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách phòng ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        //[OutputCache(NoStore = true, Duration = 0)]
        public JsonResult GetOrgStructureTree(Guid? id, string UserName)
        {
            var service = new Cat_OrgStructureServices();
            string status = string.Empty;
            var listModel = new List<CatOrgStructureModel>();
            if (HttpContext.Cache[UserName] == null)
            {
                List<Object> listObject = new List<object>();
                listObject.Add(UserName != null ? UserName : "");
                var listEntity = service.GetData<Cat_OrgStructureTreeViewEntity>(listObject, ConstantSql.hrm_cat_sp_get_OrgStructure_Data, UserLogin, ref status);

                #region Xử lý phân quyền cho cây phòng ban
                if (UserName == string.Empty || UserName == Common.UserNameSystem)
                {
                    listEntity.ForEach(m => m.IsShow = true);
                }
                #endregion

                if (listEntity != null)
                {
                    listModel = listEntity.Translate<CatOrgStructureModel>();
                    //HttpContext.Cache["List_OrgStructureTreeView"] = listModel;
                    //HttpContext.Cache.Add(UserName, listModel, null, DateTime.Now.AddDays(30), TimeSpan.Zero, CacheItemPriority.Default, null);
                    HttpContext.Cache[UserName] = listModel;
                }
            }
            else
            {
                listModel = HttpContext.Cache[UserName] as List<CatOrgStructureModel>;
            }

            //lấy quyền phòng ban theo user


            var orgStructure = from e in listModel
                               where (id.HasValue ? e.ParentID == id : e.ParentID == null)
                               select new
                               {
                                   id = e.ID,
                                   Name = e.OrgStructureName,
                                   hasChildren = listModel.Any(ch => ch.ParentID == e.ID),
                                   IconPath = ConstantPathWeb.HrWebUrl + ConstantPath.IconPath + (e.Icon ?? "icon1.png"),
                                   OrderNumber = e.OrderNumber,
                                   Code = e.Code,
                                   IsShow = e.IsShow
                               };
            return Json(orgStructure.OrderBy(m => m.Code), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }


            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }

            //foreach (System.Collections.DictionaryEntry entry in HttpContext.Cache){
            //    HttpContext.Cache.Remove(entry.Key.ToString());
            //}

            return Json("");
        }

        /// <summary>
        /// Action clear cache treeview orgstructure
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult ClearCacheOrgStructure()
        {
            HttpContext.Cache.Remove("List_OrgStructureTreeView");
            HttpContext.Cache.Remove("List_OrgStructureTreeViewSumProfile");
            return Json("");
        }

        /// <summary>
        /// [Hien.Nguyen] Lấy danh sách phòng ban tổng hợp nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        //[OutputCache(NoStore = true, Duration = 0)]
        public JsonResult GetOrgStructureTreeSummaryProfile(Guid? id)
        {
            var service = new Cat_OrgStructureServices();
            string status = string.Empty;
            //var listEntity = service.GetData<Cat_OrgStructureTreeViewEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure_Data, ref status);
            //var listModel = new List<CatOrgStructureModel>();
            //if (listEntity != null)
            //{
            //    listModel = listEntity.Translate<CatOrgStructureModel>();
            //}
            //var modelChild = OrgStructureParentAndChild(id);
            //var org=listModel.Where(m=>id.HasValue?m.ParentID==id:m.ParentID==null).Select(s=>new{id=s.ID
            var listModel = new List<CatOrgStructureModel>();
            if (HttpContext.Cache["List_OrgStructureTreeViewSumProfile"] == null)
            {
                var listEntity = service.GetDataNotParam<Cat_OrgStructureTreeViewEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure_Data_SumProfile, UserLogin, ref status);
                //listModel = listEntity.Translate<CatOrgStructureModel>();
                if (listEntity != null)
                {
                    listModel = listEntity.Translate<CatOrgStructureModel>();
                    // HttpContext.Cache["List_OrgStructureTreeViewSumProfile"] = listModel;
                    HttpContext.Cache.Add("List_OrgStructureTreeViewSumProfile", listModel, null, DateTime.Now.AddDays(30), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
            }
            else
            {
                listModel = HttpContext.Cache["List_OrgStructureTreeViewSumProfile"] as List<CatOrgStructureModel>;
            }

            var orgStructure = from e in listModel.OrderBy(m => m.OrderNumber)
                               where (id.HasValue ? e.ParentID == id : e.ParentID == null)
                               select new
                               {
                                   id = e.ID,
                                   Name = e.OrgStructureName + " (" + e.ProfileIsWorking + "/" + (GetCountProfile(listModel, e.ID, new int[2])[0] + e.ProfileIsWorking).ToString() + ")",
                                   hasChildren = listModel.Any(ch => ch.ParentID == e.ID),
                                   IconPath = ConstantPathWeb.HrWebUrl + ConstantPath.IconPath + (e.Icon ?? "icon1.png"),
                                   OrderNumber = e.OrderNumber,
                                   Code = e.Code,
                                   ProfileCount = e.ProfileCount,
                                   ProfileIsWorking = e.ProfileIsWorking,
                               };

            return Json(orgStructure, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm xử lý đếm tổng số nhân viên và nhân viên đang làm việc trong phòng ban
        /// </summary>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int[] GetCountProfile(List<CatOrgStructureModel> source, Guid id, int[] value)
        {
            //Get các phòng ban con
            var listChild = source.Where(m => m.ParentID == id).ToList();

            //Điều kiện dừng
            if (listChild.Count <= 0)
            {
                return new int[2];
            }
            //Duyệt qua các con và chạy đệ quy tìm child của các phòng ban con
            foreach (var i in listChild)
            {
                value[0] += i.ProfileIsWorking;
                value[1] += i.ProfileCount;
                GetCountProfile(source, i.ID, value);
            }
            return value;
        }




        #region OrgStructureParentAndChild
        //public List<OrgStructureParentAndChild> OrgStructureParentAndChild(int? id)
        //{
        //    var service = new Cat_OrgStructureServices();
        //    string status = string.Empty;
        //    var listObj = new List<object>();
        //    //add para giả không có giá trị
        //    for (int i = 0; i < 5; i++)
        //    {
        //        listObj.Add(null);
        //    }
        //    var model = new OrgStructureParentAndChild();
        //    var listmodel = new List<OrgStructureParentAndChild>();
        //    var listIdTemp = new List<int>();
        //    var listIdTemp1 = new List<int>();
        //    var tempDic = new Dictionary<int, List<int>>();
        //    var listEntity = service.GetData<Cat_OrgStructureEntity>(listObj,ConstantSql.hrm_cat_sp_get_OrgStructure, ref status);
        //    if (listEntity != null)
        //    {
        //        foreach (var item in listEntity)
        //        {
        //            listIdTemp = GetChild(item.Id, listEntity);
        //            tempDic.Add(item.Id,listIdTemp);
        //        }

        //        foreach (var item in tempDic.Where(s=>s.Value.Count >0))
        //        {
        //            GetChildsTemp(item.Key, tempDic);
        //            while (GetChildsTemp(item.Key, tempDic).Count > 0)
        //            {
        //                GetChildsTemp(item.Key, tempDic);
        //            }
        //        } 

        //        //var listChild = GetChilds(listEntity);

        //        //foreach (var item in listChild.Keys)
        //        //{
        //        //    listIdTemp.AddRange(listId);
        //        //    while (listId.Count>0)
        //        //    {

        //        //    }

        //        //    listIdTemp.AddRange(listId);

        //        //    for(int i = 0; i<listId.Count; i++)
        //        //    {
        //        //        while (listId.Count > 0)
        //        //        {
        //        //            listId = GetChild(listId[i], listEntity);

        //        //        }
        //        //        model.Childs.AddRange(listChildNode);
        //        //        model.CountChild += listChildNode.Count;
        //        //    }
        //        //    if (model.Childs.Count > 0)
        //        //        listmodel.Add(model);
        //        //}
        //    }
        //    return listmodel;
        //}

        //public List<int> GetChilds(int id, Dictionary<int, List<int>> dictionary)
        //{
        //    return dictionary.Where(w => w.Key == id).Select(s => s.Value).FirstOrDefault();
        //}

        //public List<int> GetChildsTemp(int id, Dictionary<int, List<int>> dictionary)
        //{
        //    var count = dictionary.Count();
        //    var listId = new List<int>();
        //    while (count > 0)
        //    {
        //        var dataIds = GetChilds(id, dictionary);
        //        if (dataIds != null)
        //        {
        //            listId.AddRange(dataIds);
        //            foreach (var item in dataIds)
        //            {
        //                dataIds = GetChilds(item, dictionary);
        //                listId.AddRange(dataIds);
        //            }
        //        }
        //    }



        //    return dictionary.Where(w => w.Key == id).Select(s => s.Value).FirstOrDefault();
        //}


        //public List<int> GetChild(int parent, List<Cat_OrgStructureEntity> data)
        //{
        //    var listId = new List<int>();
        //    foreach(var item in data)
        //    {
        //        if (item.ParentID ==parent)
        //        listId.Add(item.Id);
        //    }
        //    return listId;
        //} 
        #endregion


        #endregion

        #region Cat_JobTitle
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng JobTitle (Cat_JobTitle)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobTitleList([DataSourceRequest] DataSourceRequest request, Cat_JobTitleSearchModel model)
        {
            return GetListDataAndReturn<Cat_JobTitleModel, Cat_JobTitleEntity, Cat_JobTitleSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_JobTitle);
        }
        public JsonResult GetJobTitleOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_JobTitleMultiEntity>(text, ConstantSql.hrm_cat_sp_get_JobTitle_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<Cat_JobTitleMultiModel> listModel = listEntity.Translate<Cat_JobTitleMultiModel>();
                listModel = listModel.OrderBy(s => s.JobTitleName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiJobTitle(string text)
        {
            return GetDataForControl<Cat_JobTitleMultiModel, Cat_JobTitleMultiEntity>(text, ConstantSql.hrm_cat_sp_get_JobTitle_Multi);
        }

        public JsonResult GetJobTitle()
        {
            //var service = new Cat_JobTitleServices();
            var result = baseService.GetAllUseEntity<Cat_JobTitleEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_JobTitle(Cat_JobTitle) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllJobTitleList([DataSourceRequest] DataSourceRequest request, Cat_JobTitleSearchModel model)
        {
            return ExportAllAndReturn<Cat_JobTitleEntity, Cat_JobTitleModel, Cat_JobTitleSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_JobTitle);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_JobTitle (Cat_JobTitle) theo điều kiện tìm kiếm
        public ActionResult ExportJobTitleSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_JobTitleEntity, Cat_JobTitleModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        #endregion

        #region Cat_Position
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng Position (Cat_Position)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPositionList([DataSourceRequest] DataSourceRequest request, CatPositionSearchModel model)
        {
            return GetListDataAndReturn<CatPositionModel, Cat_PositionEntity, CatPositionSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_Position);
        }

        public JsonResult GetPositionOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_PositionMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Position_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatPositionMultiModel> listModel = listEntity.Translate<CatPositionMultiModel>();
                listModel = listModel.OrderBy(s => s.PositionName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiPosition(string text)
        {
            return GetDataForControl<CatPositionMultiModel, Cat_PositionMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Position_Multi);
        }

        public JsonResult GetPosition()
        {
            var result = baseService.GetAllUseEntity<Cat_PositionEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_Position(Cat_Position) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllPositionList([DataSourceRequest] DataSourceRequest request, CatPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_PositionEntity, CatPositionModel, CatPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Position);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_Position (Cat_Position) theo điều kiện tìm kiếm
        public ActionResult ExportPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_PositionEntity, CatPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }

        #endregion

        #region Cat_ContractType
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng ContractType (Cat_ContractType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContractTypeList([DataSourceRequest] DataSourceRequest request, CatContractTypeSearchModel model)
        {
            return GetListDataAndReturn<CatContractTypeModel, Cat_ContractTypeEntity, CatContractTypeSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_ContractType);
        }
        public JsonResult GetMultiModelCode(string text)
        {
            return GetDataForControl<Cat_ModelModel, Cat_ModelEntity>(text, ConstantSql.hrm_Cat_SP_GET_ModeCode_Multi);
        }
        public JsonResult GetMultiColorCode(string text)
        {
            return GetDataForControl<PUR_ColorModelModel, PUR_ColorModelEntity>(text, ConstantSql.hrm_Cat_SP_GET_ColorCode_Multi);
        }
        public JsonResult GetMultiPaymentMethod(string text)
        {
            return GetDataForControl<Cat_PaymentMethodModel, Cat_PaymentMethodEntity>(text, ConstantSql.hrm_Cat_SP_GET_PaymentMethod_Multi);
        }
        public JsonResult GetMultiReceivePlace(string text)
        {
            return GetDataForControl<Cat_ReceivePlaceModel, Cat_ReceivePlaceEntity>(text, ConstantSql.hrm_Cat_SP_GET_ReceivePlace_Multi);
        }
        public JsonResult GetMultiContractType(string text, string UserID)
        {
            return GetDataForControl<CatContractTypeMultiModel, Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ContractType_multi);
        }

        public JsonResult GetMultiAppendixContractType(string text)
        {
            return GetDataForControl<CatAppendixContractTypeMultiModel, Cat_AppendixContractTypeEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_ContractType(Cat_ContractType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllContractTypeList([DataSourceRequest] DataSourceRequest request, CatContractTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_ContractTypeEntity, CatContractTypeModel, CatContractTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ContractType);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_ContractType (Cat_ContractType) theo điều kiện tìm kiếm
        public ActionResult ExportContractTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ContractTypeEntity, CatContractTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        [HttpPost]
        public ActionResult GetMonthOfDateEndProbation(Guid ContractTypeID, DateTime? DateHire)
        {
            ActionService service = new ActionService(UserLogin);
            DateTime date = DateTime.Now;
            if (DateHire.HasValue)
            {
                date = DateHire.Value;
            }
            string status = "";
            var rs = service.GetByIdUseStore<Cat_ContractTypeEntity>(ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
            if (rs.ValueTime.HasValue)
            {
                if (rs.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                {
                    date = date.AddMonths(int.Parse(rs.ValueTime.Value.ToString()));
                }
                else if (rs.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                {
                    date = date.AddYears(int.Parse(rs.ValueTime.Value.ToString()));
                }

            }
            return Json(date.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cat_EmployeeType
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng EmployeeType (Cat_EmployeeType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEmployeeTypeList([DataSourceRequest] DataSourceRequest request, CatEmployeeTypeSearchModel model)
        {
            return GetListDataAndReturn<CatEmployeeTypeModel, Cat_EmployeeTypeEntity, CatEmployeeTypeSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_EmployeeType);
        }

        public JsonResult GetEmployeeTypeOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_EmployeeTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EmployeeType_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatEmployeeTypeMultiModel> listModel = listEntity.Translate<CatEmployeeTypeMultiModel>();
                listModel = listModel.OrderBy(s => s.EmployeeTypeName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiEmployeeType(string text)
        {
            return GetDataForControl<CatEmployeeTypeMultiModel, Cat_EmployeeTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EmployeeType_Multi);
        }
        /// <summary>
        /// [Tho.Bui]:Multi theo AccidentType
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonResult GetMultiAccidentType(string text)
        {
            return GetDataForControl<Cat_AccidentTypeMutiModel, Cat_AccidentTypeEntity>(text, ConstantSql.hrm_cat_sp_get_AccidentType_MultiNew);
        }
        public JsonResult GetEmpType()
        {
            var result = baseService.GetAllUseEntity<Cat_EmployeeTypeEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_EmployeeType(Cat_EmployeeType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllEmployeeTypeList([DataSourceRequest] DataSourceRequest request, CatEmployeeTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_EmployeeTypeEntity, CatEmployeeTypeModel, CatEmployeeTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_EmployeeType);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_EmployeeType (Cat_EmployeeType) theo điều kiện tìm kiếm
        public ActionResult ExportEmployeeTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_EmployeeTypeEntity, CatEmployeeTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }

        #endregion

        #region Cat_NameEntity

        public JsonResult GetMultiRank(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Rank_Multi);
        }

        [HttpPost]
        public ActionResult GetTypeOfTransferList([DataSourceRequest] DataSourceRequest request, Cat_TypeOfTransferSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_TypeOfTransferSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TypeOfTransfer);
        }

        public JsonResult GetMultiTypeOfTransfer(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TypeOfTransfer_Multi);
        }

        public ActionResult ExportAllTypeOfTransferList([DataSourceRequest] DataSourceRequest request, Cat_TypeOfTransferSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_TypeOfTransferSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TypeOfTransfer);
        }

        public ActionResult ExportTypeOfTransferSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_TypeOfTransferByIds);
        }

        public JsonResult GetEducationLevelOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EducationLevel_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatNameEntityMultiModel> listModel = listEntity.Translate<CatNameEntityMultiModel>();
                listModel = listModel.OrderBy(s => s.NameEntityName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiEducationLevel(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EducationLevel_Multi);
        }

        public JsonResult GetMultiDiseList(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Dise_Multi);
        }

        public JsonResult GetMultiCategoryKPI(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_CategoryKPI_Multi);
        }

        #endregion

        #region Cat_ResignReason
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu bảng TAMScanReasonMiss (Cat_TAMScanReasonMiss)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTAMScanReasonMissList([DataSourceRequest] DataSourceRequest request, Cat_TAMScanReasonMissSearchModel model)
        {
            return GetListDataAndReturn<Cat_TAMScanReasonMissModel, Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss);
        }
        public ActionResult ExportAllTAMScanReasonMisslList([DataSourceRequest] DataSourceRequest request, Cat_TAMScanReasonMissSearchModel model)
        {
            return ExportAllAndReturn<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel, Cat_TAMScanReasonMissSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss);
        }

        public ActionResult ExportTAMScanReasonMissSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_TAMScanReasonMissByIds);
        }



        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_ResignReasonMiss(Cat_ResignReason) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllResignReasonMissList([DataSourceRequest] DataSourceRequest request, Cat_TAMScanReasonMissSearchModel model)
        {
            return ExportAllAndReturn<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel, Cat_TAMScanReasonMissSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_ResignReasonMiss (Cat_ResignReason) theo điều kiện tìm kiếm
        public ActionResult ExportResignReasonMissSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }

        #endregion

        #region Cat_OvertimeReason

        #endregion
        public ActionResult GetOvertimeReasonList([DataSourceRequest] DataSourceRequest request, Cat_OvertimeResonSearchModel model)
        {
            return GetListDataAndReturn<Cat_OvertimeResonModel, Cat_OvertimeResonEntity, Cat_OvertimeResonSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_OvertimeReason);
        }
        public ActionResult ExportAllOvertimeReasonlList([DataSourceRequest] DataSourceRequest request, Cat_OvertimeResonSearchModel model)
        {
            return ExportAllAndReturn<Cat_OvertimeResonEntity, Cat_OvertimeResonModel, Cat_OvertimeResonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_OvertimeReason);
        }

        public ActionResult ExportOvertimeReasonSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_OvertimeResonEntity, Cat_OvertimeResonModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_OvertimeReasonByIds);
        }
        #region
        /// <summary>
        /// [Quan.nguyen] - Lấy danh sách dữ liệu bảng ResignReason (Cat_ResignReason)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetResignReasonList([DataSourceRequest] DataSourceRequest request, CatResignReasonSearchModel model)
        {
            return GetListDataAndReturn<CatResignReasonModel, Cat_ResignReasonEntity, CatResignReasonSearchModel>
                (request, model, ConstantSql.hrm_cat_sp_get_ResignReason);
        }
        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_ResignReason(Cat_ResignReason) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllResignReasonList([DataSourceRequest] DataSourceRequest request, CatResignReasonSearchModel model)
        {
            return ExportAllAndReturn<Cat_ResignReasonEntity, CatResignReasonModel, CatResignReasonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ResignReason);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_ResignReason (Cat_ResignReason) theo điều kiện tìm kiếm
        public ActionResult ExportResignReasonSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ResignReasonEntity, CatResignReasonModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        #endregion

        #region Cat_Region
        public JsonResult GetMultiRegion(string text)
        {
            return GetDataForControl<CatRegionMultiModel, Cat_RegionMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Region_multi);
        }
        #endregion

        #region Cat_UsualAllowance
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Trợ Cấp (Cat_UsualAllowance)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUsualAllowanceList([DataSourceRequest] DataSourceRequest request, Cat_UsualAllowanceSearchModel model)
        {
            return GetListDataAndReturn<Cat_UsualAllowanceModel, Cat_UsualAllowanceEntity, Cat_UsualAllowanceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UsualAllowance);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu choTrợ Cấp (Cat_UsualAllowance) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllUsualAllowanceList([DataSourceRequest] DataSourceRequest request, Cat_UsualAllowanceSearchModel model)
        {
            return ExportAllAndReturn<Cat_UsualAllowanceEntity, Cat_UsualAllowanceModel, Cat_UsualAllowanceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UsualAllowance);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của  Trợ Cấp (Cat_UsualAllowance) theo điều kiện tìm kiếm
        public ActionResult ExportUsualAllowanceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_UsualAllowanceEntity, Cat_UsualAllowanceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_UsualAllowanceByIds);
        }

        public JsonResult GetMultiUsualAllowance(string text)
        {
            return GetDataForControl<Cat_UsualAllowanceMultiModel, Cat_UsualAllowanceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UsualAllowance_Multi);
        }
        #endregion

        #region Cat_LineItem
        [HttpPost]
        public ActionResult GetLineItemList([DataSourceRequest] DataSourceRequest request, Cat_LineItemSearchModel model)
        {
            return GetListDataAndReturn<Cat_LineItemModel, Cat_LineItemEntity, Cat_LineItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LineItem);
        }
        public JsonResult GetMultiLineItem(string text)
        {
            return GetDataForControl<Cat_LineItemMultiModel, Cat_LineItemMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LineItem_Multi);
        }
        public ActionResult ExportAllLineItemlList([DataSourceRequest] DataSourceRequest request, Cat_LineItemSearchModel model)
        {
            return ExportAllAndReturn<Cat_LineItemEntity, Cat_LineItemModel, Cat_LineItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LineItem);
        }

        public ActionResult ExportLineItemSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_LineItemEntity, Cat_LineItemModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LineItemByIds);
        }

        #endregion

        #region Cat_Item
        public ActionResult GetItemList([DataSourceRequest] DataSourceRequest request, Cat_ItemSearchModel model)
        {
            return GetListDataAndReturn<Cat_ItemEntity, Cat_ItemModel, Cat_ItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Item);
        }
        public JsonResult GetMultiItem(string text)
        {
            return GetDataForControl<Cat_ItemModel, Cat_ItemEntity>(text, ConstantSql.hrm_cat_sp_get_Item_Multi);
        }

        public ActionResult ExportAllItemlList([DataSourceRequest] DataSourceRequest request, Cat_ItemSearchModel model)
        {
            return ExportAllAndReturn<Cat_ItemEntity, Cat_ItemModel, Cat_ItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Item);
        }

        public ActionResult ExportItemSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ItemEntity, Cat_ItemModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ItemByIds);
        }

        #endregion

        #region Cat_Brand
        public JsonResult GetMultiBrand(string text)
        {
            return GetDataForControl<Cat_BrandMultiModel, Cat_BrandMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Brand_Multi);
        }
        public ActionResult ExportAllBrandList([DataSourceRequest] DataSourceRequest request, Cat_BrandSearchModel model)
        {
            return ExportAllAndReturn<Cat_BrandEntity, Cat_BrandModel, Cat_BrandSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Brand);
        }


        public ActionResult ExportBrandSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_BrandEntity, Cat_BrandModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_BrandByIds);
        }

        public ActionResult GetBrandList([DataSourceRequest] DataSourceRequest request, Cat_BrandSearchModel model)
        {
            return GetListDataAndReturn<Cat_BrandModel, Cat_BrandEntity, Cat_BrandSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Brand);
        }
        #endregion

        #region Cat_Unit
        public JsonResult GetMultiUnit(string text)
        {
            return GetDataForControl<Cat_UnitMultiModel, Cat_UnitMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Unit_Multi);
        }
        public ActionResult GetUnitList([DataSourceRequest] DataSourceRequest request, Cat_UnitSearchModel model)
        {
            return GetListDataAndReturn<Cat_UnitModel, Cat_UnitEntity, Cat_UnitSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Unit);
        }

        public ActionResult ExportAllUnitlList([DataSourceRequest] DataSourceRequest request, Cat_UnitSearchModel model)
        {
            return ExportAllAndReturn<Cat_UnitEntity, Cat_UnitModel, Cat_UnitSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Unit);
        }

        public ActionResult ExportUnitSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_UnitEntity, Cat_UnitModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_UnitByIds);
        }


        #endregion

        #region Cat_KBIBonus
        public JsonResult GetMultiKPIBonus(string text)
        {
            return GetDataForControl<Cat_KPIBonusMultiModel, Cat_KPIBonusMultiEntity>(text, ConstantSql.hrm_cat_sp_get_KPIBonus_Multi);
        }
        public ActionResult ExportAllKPIList([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusSearchMoel model)
        {
            return ExportAllAndReturn<Cat_KPIBonusEntity, Cat_KPIBonusModel, Cat_KPIBonusSearchMoel>(request, model, ConstantSql.hrm_cat_sp_get_KPIBonus);
        }

        public ActionResult ExportAllKPIItemList([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusItemSearchMoel model)
        {
            return ExportAllAndReturn<Cat_KPIBonusItemEntity, Cat_KPIBonusItemModel, Cat_KPIBonusItemSearchMoel>(request, model, ConstantSql.hrm_cat_sp_get_KPIBonusItem);
        }


        public ActionResult ExportKPIBonusSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_KPIBonusEntity, Cat_KPIBonusModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_KPIBonusIds);
        }
        public ActionResult ExportKPIBonusItemSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_KPIBonusItemEntity, Cat_KPIBonusItemModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_KPIBonusItemByIds);
        }


        public ActionResult GetKPIBonusList([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusSearchMoel model)
        {
            return GetListDataAndReturn<Cat_KPIBonusModel, Cat_KPIBonusEntity, Cat_KPIBonusSearchMoel>(request, model, ConstantSql.hrm_cat_sp_get_KPIBonus);
        }
        public ActionResult GetKPIBonusItemList([DataSourceRequest] DataSourceRequest request, Cat_KPIBonusItemSearchMoel model)
        {
            return GetListDataAndReturn<Cat_KPIBonusItemModel, Cat_KPIBonusItemEntity, Cat_KPIBonusItemSearchMoel>(request, model, ConstantSql.hrm_cat_sp_get_KPIBonusItem);
        }
        #endregion

        #region Cat_UsualAllowanceLevel
        public ActionResult GetUsualAllowanceLevelByAllowanceID([DataSourceRequest] DataSourceRequest request, Guid AllowanceID)
        {
            try
            {
                string status = string.Empty;
                var baseService = new BaseService();
                var objs = new List<object>();
                objs.Add(AllowanceID);
                var result = baseService.GetData<Cat_UsualAllowanceLevelEntity>(objs, ConstantSql.hrm_cat_sp_get_UsualAllowanceLevelByAllowanceID, UserLogin, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            catch
            {

            }
            return Json(null);
        }
        #endregion

        #region Cat_SalaryClass
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng mã lương (Cat_SalaryClass)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSalaryClassList([DataSourceRequest] DataSourceRequest request, Cat_SalaryClassSearchModel model)
        {
            return GetListDataAndReturn<Cat_SalaryClassModel, Cat_SalaryClassEntity, Cat_SalaryClassSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryClass);
        }


        [HttpPost]
        public ActionResult GetDataRankDetailByOrderNumber(string OrderNumber, string rankCode)
        {

            string status = string.Empty;

            var rankDetailServices = new Cat_SalaryClassServices();
            var objRankDetail = new List<object>();
            objRankDetail.AddRange(new object[4]);
            objRankDetail[2] = 1;
            objRankDetail[3] = int.MaxValue - 1;
            var lstRankDetail = rankDetailServices.GetData<Cat_SalaryRankEntity>(objRankDetail, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).ToList();

            if (lstRankDetail.Count > 0)
            {
                var entity = lstRankDetail.Where(s => s.Code == rankCode && s.OrderNumber == int.Parse(OrderNumber + 1)).FirstOrDefault();
                return Json(entity, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        public ActionResult GetDataRankByOrderNumber(string OrderNumber)
        {

            string status = string.Empty;

            var rankServices = new Cat_SalaryClassServices();
            var objRank = new List<object>();
            objRank.Add(null);
            objRank.Add(1);
            objRank.Add(int.MaxValue - 1);
            var lstRank = rankServices.GetData<Cat_SalaryClassEntity>(objRank, ConstantSql.hrm_cat_sp_get_SalaryClass, UserLogin, ref status).ToList();

            var rankDetailServices = new Cat_SalaryClassServices();
            var objRankDetail = new List<object>();
            objRankDetail.AddRange(new object[4]);
            objRankDetail[2] = 1;
            objRankDetail[3] = int.MaxValue - 1;
            var lstRankDetail = rankDetailServices.GetData<Cat_SalaryRankEntity>(objRankDetail, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).ToList();

            if (lstRank.Count > 0)
            {
                var entity = lstRank.Where(s => s.OrderNumber == int.Parse(OrderNumber + 1)).FirstOrDefault();
                if (entity != null)
                {
                    var lstRankDetailByRankID = lstRankDetail.Where(s => s.SalaryClassID == entity.ID).ToList();
                    int total = lstRankDetailByRankID.Count;
                    int count = lstRankDetailByRankID.Count - 1;
                    var orderNumber = total - count;
                    var rankDetailEntiy = lstRankDetail.Where(s => s.OrderNumber == orderNumber && entity.ID == s.SalaryClassID).FirstOrDefault();
                    return Json(rankDetailEntiy, JsonRequestBehavior.AllowGet);
                }

            }

            return null;
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho mã lương (Cat_SalaryClass) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSalaryClassList([DataSourceRequest] DataSourceRequest request, Cat_SalaryClassSearchModel model)
        {
            return ExportAllAndReturn<Cat_SalaryClassEntity, Cat_SalaryClassModel, Cat_SalaryClassSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryClass);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của mã lương (Cat_SalaryClass) theo điều kiện tìm kiếm
        public ActionResult ExportSalaryClassSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SalaryClassEntity, Cat_SalaryClassModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        [HttpPost]
        public JsonResult GetMultiSalaryClass(string text)
        {
            return GetDataForControl<Cat_SalaryClassMultiModel, Cat_SalaryClassMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryClass_multi);
        }

        public JsonResult GetMultiSalaryJobGroup(string text)
        {
            return GetDataForControl<Cat_SalaryJobGroupMultiModel, Cat_SalaryJobGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryJobGroup_multi);
        }

        public JsonResult GetMultiSalaryAdjCampaign(string text)
        {
            return GetDataForControl<Cat_SalAdjustmentCampaignMultiModel, Cat_SalAdjustmentCampaignMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryAdjCampaign_multi);
        }

        #endregion

        #region Cat_GradePayroll
        public ActionResult GetGradePayrollList([DataSourceRequest] DataSourceRequest request, Cat_GradePayrollSearchlModel model)
        {
            return GetListDataAndReturn<Cat_GradePayrollModel, Cat_GradePayrollEntity, Cat_GradePayrollSearchlModel>
                (request, model, ConstantSql.hrm_cat_sp_get_GradePayroll);
        }

        [HttpPost]
        public ActionResult GetElementByGradePayrollID([DataSourceRequest] DataSourceRequest request, Guid payrollID)
        {

            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(payrollID);
            var result = baseService.GetData<Cat_ElementEntity>(objs, ConstantSql.hrm_cat_sp_get_ElementByPayrollID, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult GetAdvancePaymentByGradePayrollID([DataSourceRequest] DataSourceRequest request, Guid payrollID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(payrollID);
            var result = baseService.GetData<Cat_ElementEntity>(objs, ConstantSql.hrm_cat_sp_get_AdvancePaymentByPayrollID, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Cat_PerformanceType
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Loại Đánh Giá (Cat_PerformanceType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPerformanceTypeList([DataSourceRequest] DataSourceRequest request, Cat_PerformanceTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_PerformanceTypeModel, Cat_PerformanceTypeEntity, Cat_PerformanceTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PerformanceType);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho mã lương (Cat_SalaryClass) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllPerformanceTypeList([DataSourceRequest] DataSourceRequest request, Cat_PerformanceTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_PerformanceTypeEntity, Cat_PerformanceTypeModel, Cat_PerformanceTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PerformanceType);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của mã lương (Cat_SalaryClass) theo điều kiện tìm kiếm
        public ActionResult ExportPerformanceTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_PerformanceTypeEntity, Cat_PerformanceTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_PerformanceTypeByIds);
        }

        public JsonResult GetMultiPerformanceType(string text)
        {
            return GetDataForControl<Cat_PerformanceTypeModel, Cat_PerformanceTypeEntity>(text, ConstantSql.hrm_cat_sp_get_PerformanceType_multi);
        }
        #endregion

        #region Cat_ProductItem

        public ActionResult GetProductItemList([DataSourceRequest] DataSourceRequest request, Cat_ProductItemSearchModel model)
        {
            return GetListDataAndReturn<Cat_ProductItemModel, Cat_ProductItemEntity, Cat_ProductItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ProductItem_All);
        }
        public ActionResult ExportAllProductItemList([DataSourceRequest]DataSourceRequest request, Cat_ProductItemSearchModel model)
        {
            return ExportAllAndReturn<Cat_ProductItemEntity, Cat_ProductItemModel, Cat_ProductItemSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ProductItem_All);
        }

        public ActionResult ExportProductItemSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ProductItemEntity, Cat_ProductItemModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ProductItemByIds);
        }
        #endregion

        #region Cat_Product
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Đơn Giá SP (Cat_Product)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductList([DataSourceRequest] DataSourceRequest request, CatProductSearchModel model)
        {
            return GetListDataAndReturn<CatProductModel, Cat_ProductEntity, CatProductSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Product);
        }

        public JsonResult GetMultiProduct(string text)
        {
            return GetDataForControl<Cat_ProductMultiModel, Cat_ProductMultiEntity>(text, ConstantSql.hrm_Cat_sp_get_Product_multi);
        }

        public JsonResult GetProductMulti()
        {
            //GetDataForControl<CatProductModel,Cat_ProductEntity>( ConstantSql.hrm_cat_sp_get_Product)
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            return GetData<Cat_ProductMultiModel, Cat_ProductMultiEntity>(listModel, ConstantSql.hrm_cat_sp_get_Product);
        }



        public JsonResult GetProductITemMulti(Guid? ID)
        {
            if (ID != null)
            {
                List<object> listModel = new List<object>();
                listModel.AddRange(new object[1]);
                listModel[0] = ID;
                return GetData<Cat_ProductItemModel, Cat_ProductItemEntity>(listModel, ConstantSql.hrm_cat_sp_get_ProductItem);
            }
            return Json(null);
        }

        public JsonResult GetFieldterProductITemMulti(string text)
        {
            //List<object> listModel = new List<object>();
            //listModel.AddRange(new object[2]);
            //listModel[1] = text;
            //return GetData<Cat_ProductItemModel, Cat_ProductItemEntity>(listModel, ConstantSql.hrm_cat_sp_get_ProductItem_Multi);
            return GetDataForControl<Cat_ProductItemMultiModel, Cat_ProductItemMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ProductItem_Multi);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Đơn Giá SP (Cat_Prodcut) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllProductList([DataSourceRequest] DataSourceRequest request, CatProductSearchModel model)
        {
            return ExportAllAndReturn<Cat_ProductEntity, CatProductModel, CatProductSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Product);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của Đơn Giá SP (Cat_Prodcut) theo điều kiện tìm kiếm
        public ActionResult ExportSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ProductEntity, CatProductModel>(string.Join(",", selectedIds), valueFields, ConstantSql.hrm_cat_sp_get_ProductByIds);
        }
        #endregion

        #region Cat_Bank
        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu bảng Ngân Hàng (Cat_Bank)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBankList([DataSourceRequest] DataSourceRequest request, CatBankSearchModel model)
        {
            return GetListDataAndReturn<CatBankModel, Cat_BankEntity, CatBankSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Bank);
        }
        public JsonResult GetMultiBank(string text)
        {
            return GetDataForControl<CatBankMultiModel, Cat_BankMultiEntity>(text, ConstantSql.hrm_Cat_sp_get_Bank_multi);
        }

        public ActionResult ExportAllBankList([DataSourceRequest] DataSourceRequest request, CatBankSearchModel model)
        {
            return ExportAllAndReturn<Cat_BankEntity, CatBankModel, CatBankSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Bank);
        }
        #endregion

        #region Cat_ProductType
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng ProductType
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetProductTypeList([DataSourceRequest] DataSourceRequest request, CatProductTypeSearchModel model)
        {
            return GetListDataAndReturn<CatProductTypeModel, Cat_ProductTypeEntity, CatProductTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ProductType);
        }
        public ActionResult ExportAllProductTypeList([DataSourceRequest] DataSourceRequest request, CatProductTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_ProductTypeEntity, CatProductTypeModel, CatProductTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ProductType);
        }

        public ActionResult ExportProductTypeSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ProductTypeEntity, CatProductTypeModel>(string.Join(",", selectedIds), valueFields, ConstantSql.hrm_cat_sp_get_ProductTypeByIds);
        }

        public JsonResult GetMultiProductType(string text)
        {
            return GetDataForControl<CatProductTypeMultiModel, Cat_ProductTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ProductType_Multi);
        }
        #endregion

        #region Cat_Category
        /// <summary>
        /// [Tin.Nguyen] - Lấy danh sách dữ liệu bảng Loại Thiết Bị (Cat_Category)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCategoryList([DataSourceRequest] DataSourceRequest request, CatCategorySearchModel model)
        {
            return GetListDataAndReturn<CatCategoryModel, Cat_CategoryEntity, CatCategorySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Category);
        }

        public ActionResult ExportCatCategorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CategoryEntity, CatCategoryModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_CatagoryByIds);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllCategoryList([DataSourceRequest] DataSourceRequest request, CatCategorySearchModel model)
        {
            return ExportAllAndReturn<Cat_CategoryEntity, CatCategoryModel, CatCategorySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Category);
        }
        #endregion

        #region Cat_Currency
        [HttpPost]
        public ActionResult GetCurrencyList([DataSourceRequest] DataSourceRequest request, CatCurrencySearchModel model)
        {
            return GetListDataAndReturn<CatCurrencyModel, Cat_CurrencyEntity, CatCurrencySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Currency);
        }

        public JsonResult GetMultiCurrency(string text)
        {

            return GetDataForControl<CatCurrencyMultiModel, Cat_CurrencyMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Currency_Multi);
        }
        public JsonResult GetMultiCurrencyVND(string text)
        {
            if (text == string.Empty || text == null)
            {
                text = "VND";
            }
            return GetDataForControl<CatCurrencyMultiModel, Cat_CurrencyMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Currency_Multi);
        }
        public JsonResult GetCurrency()
        {
            var result = baseService.GetAllUseEntity<Cat_CurrencyMultiEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCatCurrencySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CurrencyEntity, CatCurrencyModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_CurrencyByIds);
        }

        public ActionResult ExportAllCurrencyList([DataSourceRequest] DataSourceRequest request, CatCurrencySearchModel model)
        {
            return ExportAllAndReturn<Cat_CurrencyEntity, CatCurrencyModel, CatCurrencySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Currency);
        }

        #endregion

        #region Cat_District
        /// <summary>
        /// [Tin.Nguyen] - Lấy danh sách dữ liệu bảng Quận/Huyện (Cat_District)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDistrictList([DataSourceRequest] DataSourceRequest request, CatDistrictSearchModel model)
        {
            return GetListDataAndReturn<CatDistrictModel, Cat_DistrictEntity, CatDistrictSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_District);
        }

        public JsonResult GetDistrict()
        {
            var result = baseService.GetAllUseEntity<Cat_DistrictEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCatDistrictSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_DistrictEntity, CatDistrictModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_DistrictByIds);
        }

        public ActionResult ExportAllDistrictList([DataSourceRequest] DataSourceRequest request, CatDistrictSearchModel model)
        {
            return ExportAllAndReturn<Cat_DistrictEntity, CatDistrictModel, CatDistrictSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_District);
        }


        public JsonResult GetDistrictCascading(Guid province, string districtFilter)
        {
            var result = new List<CatDistrictModel>();
            string status = string.Empty;
            if (province != Guid.Empty)
            {
                var service = new Cat_DistrictServices();
                result = service.GetData<CatDistrictModel>(province, ConstantSql.hrm_cat_sp_get_DisctrictByProvinceId, UserLogin, ref status);
                result = result.OrderBy(s => s.DistrictName).ToList();
                if (!string.IsNullOrEmpty(districtFilter))
                {
                    var rs = result.Where(s => s.DistrictName != null && s.DistrictName.ToLower().Contains(districtFilter.ToLower())).OrderBy(s => s.DistrictName).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cat_Province

        /// <summary>
        /// [Tin.Nguyen] - Lấy danh sách dữ liệu bảng Tỉnh/Thành Phố (Cat_Province)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProvinceList([DataSourceRequest] DataSourceRequest request, CatProvinceSearchModel model)
        {
            return GetListDataAndReturn<CatProvinceModel, Cat_ProvinceEntity, CatProvinceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Province);
        }

        public JsonResult GetMultiProvince(string text)
        {
            return GetDataForControl<CatProvinceMultiModel, Cat_ProvinceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Province_multi);
        }
        public JsonResult GetMultiResignReason(string text)
        {
            return GetDataForControl<CatResignReasonModel, Cat_ResignReasonMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ResignReason_multi);
        }

        public ActionResult ExportCatProvinceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ProvinceEntity, CatProvinceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ProvinceByIds);
        }

        public ActionResult ExportAllProvinceList([DataSourceRequest] DataSourceRequest request, CatProvinceSearchModel model)
        {
            return ExportAllAndReturn<Cat_ProvinceEntity, CatProvinceModel, CatProvinceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Province);
        }

        public JsonResult GetProvince()
        {
            var result = baseService.GetAllUseEntity<Cat_DistrictEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetProvinceCascading(Guid country, string provinceFilter)
        {
            var result = new List<CatProvinceModel>();
            string status = string.Empty;
            if (country != Guid.Empty)
            {
                var service = new Cat_ProvinceServices();
                result = service.GetData<CatProvinceModel>(country, ConstantSql.hrm_cat_sp_get_ProvinceByCountryId, UserLogin, ref status);
                result = result.OrderBy(s => s.ProvinceName).ToList();
                if (!string.IsNullOrEmpty(provinceFilter))
                {
                    var rs = result.Where(s => s.ProvinceName != null && s.ProvinceName.ToLower().Contains(provinceFilter.ToLower())).ToList();
                    rs = rs.OrderBy(s => s.ProvinceName).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }


            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVillageCascading(Guid districtid, string villageFilter)
        {
            var result = new List<Cat_VillageModel>();
            string status = string.Empty;
            if (districtid != Guid.Empty)
            {
                var service = new Cat_ProvinceServices();
                result = service.GetData<Cat_VillageModel>(districtid, ConstantSql.hrm_cat_sp_get_VillageByDistrictId, UserLogin, ref status);
                result = result.OrderBy(s => s.VillageName).ToList();
                if (!string.IsNullOrEmpty(villageFilter))
                {
                    var rs = result.Where(s => s.VillageName != null && s.VillageName.ToLower().Contains(villageFilter.ToLower())).OrderBy(s => s.VillageName).ToList();

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobTypeCascading(Guid RoleID, string jobtitleFilter)
        {
            var result = new List<Cat_JobTypeModel>();
            string status = string.Empty;
            if (RoleID != Guid.Empty)
            {
                var service = new Cat_ProvinceServices();
                result = service.GetData<Cat_JobTypeModel>(RoleID, ConstantSql.hrm_cat_sp_get_JobTypeByRoleId, UserLogin, ref status);
                if (!string.IsNullOrEmpty(jobtitleFilter))
                {
                    var rs = result.Where(s => s.JobTypeName != null && s.JobTypeName.ToLower().Contains(jobtitleFilter.ToLower())).ToList();

                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cat_Country
        /// <summary>
        /// [Tin.Nguyen] - Lấy danh sách dữ liệu bảng Quốc Gia (Cat_Country)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCountryList([DataSourceRequest] DataSourceRequest request, CatCountrySearchModel model)
        {
            return GetListDataAndReturn<CatCountryModel, Cat_CountryEntity, CatCountrySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Country);
        }

        public JsonResult GetMultiCountry(string text)
        {
            return GetDataForControl<CatCountryMultiModel, Cat_CountryMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Country_multi);
        }
        public JsonResult GetCountryCascading(string text)
        {
            //return GetDataForControl<CatCountryMultiModel, Cat_CountryMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Country_multi);
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_CountryMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Country_multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatCountryMultiModel> listModel = listEntity.Translate<CatCountryMultiModel>();
                listModel = listModel.OrderBy(s => s.CountryName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetMultiDistrict(string text)
        {
            return GetDataForControl<CatDistrictMultiModel, Cat_DistrictMultiEntity>(text, ConstantSql.hrm_cat_sp_get_District_multi);
        }
        public JsonResult GetCountry()
        {
            var result = baseService.GetAllUseEntity<Cat_CountryEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExportCatCountrySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CountryEntity, CatCountryModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_CountryByIds);
        }

        public ActionResult ExportAllCountryList([DataSourceRequest] DataSourceRequest request, CatCountrySearchModel model)
        {
            return ExportAllAndReturn<Cat_CountryEntity, CatCountryModel, CatCountrySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Country);
        }

        #endregion

        #region Cat_CostCentre
        [HttpPost]
        public ActionResult GetCatCostCentreList([DataSourceRequest] DataSourceRequest request, CatCostCentreSearchModel model)
        {
            return GetListDataAndReturn<CatCostCentreModel, Cat_CostCentreEntity, CatCostCentreSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CostCentre);
        }
        /// [Phuoc.Le] - Xuất danh sách dữ liệu cho Mã chi phí (Cat_CostCentre) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllCostCentreList([DataSourceRequest] DataSourceRequest request, CatCostCentreSearchModel model)
        {
            return ExportAllAndReturn<Cat_CostCentreEntity, CatCostCentreModel, CatCostCentreSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CostCentre);
        }

        /// [Phuoc.Le] - Xuất các dòng dữ liệu được chọn của  Mã chi phí (Cat_CostCentre) theo điều kiện tìm kiếm

        public ActionResult ExportCostCentreSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CostCentreEntity, CatCostCentreModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_CostCentreByIds);
        }


        public JsonResult GetMultiCostCentre(string text)
        {
            return GetDataForControl<CatCostCentreMultiModel, Cat_CostCentreMultiEntity>(text, ConstantSql.hrm_cat_sp_get_CostCentre_Multi);
        }

        public JsonResult GetCostCentre()
        {
            var result = baseService.GetAllUseEntity<Cat_CostCentreEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Cat_LeaveDayType
        [HttpPost]
        public ActionResult GetLeaveDayTypeList([DataSourceRequest] DataSourceRequest request, CatLeaveDayTypeSearchModel model)
        {
            return GetListDataAndReturn<CatLeaveDayTypeModel, Cat_LeaveDayTypeEntity, CatLeaveDayTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LeaveDayType);
        }

        public ActionResult ExportAllLeaveDayTypelList([DataSourceRequest] DataSourceRequest request, CatLeaveDayTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_LeaveDayTypeEntity, CatLeaveDayTypeModel, CatLeaveDayTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LeaveDayType);
        }

        public ActionResult ExportLeaveDayTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_LeaveDayTypeEntity, CatLeaveDayTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LeaveDayTypeByIds);
        }

        public JsonResult GetMultiLeaveDayType(string text)
        {
            //return GetDataForControl<CatLeaveDayTypeMultiModel, Cat_LeaveDayTypeMultiEntity>(string.Empty, ConstantSql.hrm_cat_sp_get_LeaveDayType_multi);

            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_LeaveDayTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LeaveDayType_multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatLeaveDayTypeMultiModel> listModel = listEntity.Translate<CatLeaveDayTypeMultiModel>();
                listModel = listModel.OrderBy(s => s.LeaveDayTypeName).ToList();

                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cat_DayOff
        [HttpPost]
        public ActionResult GetDayOffList([DataSourceRequest] DataSourceRequest request, CatDayOffSearchModel model)
        {
            return GetListDataAndReturn<CatDayOffModel, Cat_DayOffEntity, CatDayOffSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DayOff);
        }

        public ActionResult ExportAllDayOfflList([DataSourceRequest] DataSourceRequest request, CatDayOffSearchModel model)
        {
            return ExportAllAndReturn<Cat_DayOffEntity, CatDayOffModel, CatDayOffSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DayOff);
        }

        //public ActionResult ExportDayOffSelected(string selectedIds, string valueFields)
        //{
        //    return ExportSelectedAndReturn<Cat_DayOffEntity, CatDayOffModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_DayOffByIds);
        //}

        public ActionResult ExportDayoffSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_DayOffEntity, CatDayOffModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_DayOffByIds);
        }


        #endregion

        #region Cat_LateEarlyRule
        [HttpPost]
        public ActionResult GetCatLateEarlyRuleList([DataSourceRequest] DataSourceRequest request, CatLateEarlyRuleSearchModel model)
        {
            return GetListDataAndReturn<CatLateEarlyRuleModel, Cat_LateEarlyRuleEntity, CatLateEarlyRuleSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LateEarlyRule);
        }
        /// <summary>
        /// [Hien.Nguyen] 22/10/2014 - Lấy dữ liệu bảng  CatLateEarlyRule by cfg id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCatLateEarlyRuleByCfgIDList([DataSourceRequest] DataSourceRequest request, CatLateEarlyRuleByCfgIDModelSearch model)
        {
            return GetListDataAndReturn<CatLateEarlyRuleModel, Cat_LateEarlyRuleEntity, CatLateEarlyRuleByCfgIDModelSearch>(request, model, ConstantSql.hrm_cat_sp_get_LateEarlyRuleByAttGradeId);
        }


        public ActionResult DeleteInCellLateEarly([Bind(Prefix = "models")] CatLateEarlyRuleModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_LateEarlyRuleEntity, CatLateEarlyRuleModel>(model.ID.ToString());
            return Json("");
        }

        public ActionResult CreateInCellLateEarly([Bind(Prefix = "models")] List<CatLateEarlyRuleModel> model, Guid id)
        {
            var service = new BaseService();
            string status = string.Empty;
            //var t = service.GetAllUserEntity<Cat_GradeMultiEntity>(ref status);
            var GradeCfg = GetData<CatLateEarlyRuleMultiModel, Cat_GradeMultiEntity>("", ConstantSql.hrm_cat_sp_get_Grade_multi);
            //var service = new RestServiceClient<CatLateEarlyRuleModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Cat_Service);

            if (model != null)
            {
                if (GradeCfg != null && GradeCfg.Count > 0 && id != Guid.Empty)
                {
                    foreach (var i in model)
                    {
                        i.GradeCfgID = GradeCfg.FirstOrDefault().ID;
                        i.GradeAttID = id;
                        //item = service.Add<Cat_LateEarlyRuleEntity, CatLateEarlyRuleModel>(i);
                        service.Add<Cat_LateEarlyRuleEntity>(i.CopyData<Cat_LateEarlyRuleEntity>());
                    }
                }

            }
            return Json("");

        }

        #endregion

        #region Cat_Grade
        [HttpPost]
        public ActionResult GetGradeList([DataSourceRequest] DataSourceRequest request, Sal_GradeSearchModel model)
        {
            return GetListDataAndReturn<Sal_GradeModel, Sal_GradeEntity, Sal_GradeSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_Grade);
        }

        public JsonResult GetMultiCatGrade(string text)
        {
            return GetDataForControl<CatGradeMultiModel, Cat_GradeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Grade_multi);
        }

        #endregion

        #region Cat_Supplier
        [HttpPost]
        public ActionResult GetSupplierList([DataSourceRequest] DataSourceRequest request, Cat_SupplierSearchModel model)
        {
            return GetListDataAndReturn<Cat_SupplierModel, Cat_SupplierEntity, Cat_SupplierSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Supplier);
        }

        public ActionResult ExportAllSupplierlList([DataSourceRequest] DataSourceRequest request, Cat_SupplierSearchModel model)
        {
            return ExportAllAndReturn<Cat_SupplierEntity, Cat_SupplierModel, Cat_SupplierSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Supplier);
        }

        public ActionResult ExportSupplierSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SupplierEntity, Cat_SupplierModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SupplierByIds);
        }

        public JsonResult GetMultiSupplier(string text)
        {
            return GetDataForControl<Cat_SupplierMultiModel, Cat_SupplierMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Supplier_Multi);
        }
        #endregion

        #region Cat_PurchaseItems
        public ActionResult GetPurchaseItemsList([DataSourceRequest] DataSourceRequest request, Cat_PurchaseItemsSearchModel model)
        {
            return GetListDataAndReturn<Cat_PurchaseItemsModel, Cat_PurchaseItemsEntity, Cat_PurchaseItemsSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PurchaseItems);
        }

        public ActionResult ExportAllPurchaseItems([DataSourceRequest] DataSourceRequest request, Cat_PurchaseItemsSearchModel model)
        {
            return ExportAllAndReturn<Cat_PurchaseItemsEntity, Cat_PurchaseItemsModel, Cat_PurchaseItemsSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PurchaseItems);
        }

        public ActionResult ExportPurchaseItemsSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_PurchaseItemsEntity, Cat_PurchaseItemsModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_PurchaseItemsByIds);
        }

        public JsonResult GetMultiPurchaseItems(string text)
        {
            return GetDataForControl<Cat_PurchaseItemsMultiModel, Cat_PurchaseItemsMultiEntity>(text, ConstantSql.hrm_cat_sp_get_PruchaseItems_Multi);
        }
        #endregion

        #region Cat_Overtime
        [HttpPost]
        public ActionResult GetOvertimeTypeList([DataSourceRequest] DataSourceRequest request, CatOvertimeTypeSearchModel model)
        {
            return GetListDataAndReturn<CatOvertimeTypeModel, Cat_OvertimeTypeEntity, CatOvertimeTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_OvertimeType);
        }


        public JsonResult GetMultiOvertimeType(string text)
        {
            return GetDataForControl<CatOvertimeTypeMultiModel, Cat_OvertimeTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_OvertimeType_multi);
        }

        public ActionResult ExportAllOvertimeTypelList([DataSourceRequest] DataSourceRequest request, CatOvertimeTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_OvertimeTypeEntity, CatOvertimeTypeModel, CatOvertimeTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_OvertimeType);
        }

        public ActionResult ExportOvertimeTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_OvertimeTypeEntity, CatOvertimeTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_OvertimeTypeByIds);
        }




        //public JsonResult GetMultiOvertimeType(string text)
        //{
        //    var service = new Cat_OvertimeTypeServices();
        //    var get = service.GetMulti(text);
        //    var result = get.Select(item => new CatOvertimeTypeMultiModel()
        //    {
        //        Id = item.Id,
        //        OvertimeTypeName = item.OvertimeTypeName,
        //    });
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //   public ActionResult GetOvertimeTypeList([DataSourceRequest] DataSourceRequest request, CatOvertimeTypeSearchModel ctModel)  
        //   {
        //    var service = new Cat_OvertimeTypeServices();
        //    ctModel.Rate = ctModel.Rate == 0.0 ? null : ctModel.Rate;
        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(ctModel)
        //    };
        //    var result = service.GetCat_OvertimeType(lstModel).ToList().Translate<CatOvertimeTypeModel>();
        //    return Json(result.ToDataSourceResult(request));
        //}
        #endregion

        #region Cat_HDTJobGroup
        public JsonResult GetMultiHDTJobGroup(string text)
        {
            return GetDataForControl<Cat_HDTJobGroupMultiModel, Cat_HDTJobGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_HDTJobGroup_multi);
        }

        public JsonResult GetMultiHDTJobGroupCode(string text)
        {
            return GetDataForControl<Cat_HDTJobGroupCodeMultiModel, Cat_HDTJobGroupCodeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_HDTJobGroupCode_multi);
        }
        public ActionResult ExportAllHDTJobGroupList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobGroupSearchModel model)
        {
            return ExportAllAndReturn<Cat_HDTJobGroupEntity, Cat_HDTJobGroupModel, Cat_HDTJobGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobGroup);
        }


        public ActionResult ExportHDTJobGroupSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_HDTJobGroupEntity, Cat_HDTJobGroupModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_HDTJobGroupByIds);
        }

        public ActionResult GetHDTJobGroupList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobGroupSearchModel model)
        {
            return GetListDataAndReturn<Cat_HDTJobGroupModel, Cat_HDTJobGroupEntity, Cat_HDTJobGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobGroup);
        }

        public ActionResult ChangeStatusListHDTJobGroup(string selectedIds, string Status)
        {
            Cat_HDTJobGroupServices Services = new Cat_HDTJobGroupServices();
            ResultsObject result = Services.UpdateStatus(selectedIds, Status);
            return Json(result);
        }
        #endregion

        #region Cat_Element
        [HttpPost]
        public ActionResult GetElementList([DataSourceRequest] DataSourceRequest request, CatElementCommissionSearchModel model)
        {
            return GetListDataAndReturn<CatElementModel, Cat_ElementEntity, CatElementCommissionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ElementByMethod);
        }
        [HttpPost]
        public ActionResult GetElementListbyMethodPayroll([DataSourceRequest] DataSourceRequest request, Guid GradeID, string MethodPayroll)
        {
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            lstModel.AddRange(new object[7]);
            lstModel[2] = null;
            lstModel[3] = Common.DotNetToOracle(GradeID.ToString());
            lstModel[4] = null;
            lstModel[5] = 1;
            lstModel[6] = Int32.MaxValue - 1;
            var listEntity = baseService.GetData<CatElementModel>(lstModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.MethodPayroll == MethodPayroll).ToList();
            return Json(listEntity.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetFormulaTemplateList([DataSourceRequest] DataSourceRequest request, Cat_FormulaTemplateSearchModel model)
        {
            return GetListDataAndReturn<Cat_FormulaTemplateModel, Cat_FormulaTemplateEntity, Cat_FormulaTemplateSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_FormulaTemplate);
        }

        public ActionResult ExportSElementSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ElementEntity, CatElementModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ElementByIds);
        }
        public ActionResult ExportAllElementList([DataSourceRequest] DataSourceRequest request, CatElementSearchModel model)
        {
            return ExportAllAndReturn<Cat_ElementEntity, CatElementModel, CatElementSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Element);
        }

        public ActionResult GetElementListForCreate([DataSourceRequest] DataSourceRequest request, string type, Guid? GradePayroll, string TabType, string Method)
        {
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            if (Method == null || Method == string.Empty)
            {
                Method = MethodPayroll.E_NORMAL.ToString();
            }

            #region Load dữ liệu của phần phụ cấp
            if (type == CatElementType.Allowances.ToString())
            {
                lstModel = new List<object>();
                lstModel.AddRange(new object[4]);
                lstModel[2] = 1;
                lstModel[3] = Int32.MaxValue - 1;
                var listUsualAllowance = baseService.GetData<Cat_UsualAllowanceModel>(lstModel, ConstantSql.hrm_cat_sp_get_UsualAllowance, UserLogin, ref status);

                List<CatElementModel> listResult = new List<CatElementModel>();
                if (listUsualAllowance != null)
                {
                    foreach (var i in listUsualAllowance)
                    {
                        listResult.Add(new CatElementModel() { ElementName = i.UsualAllowanceName, ElementCode = i.Code, Formula = i.Formula });

                    }
                }
                return Json(listResult.ToDataSourceResult(request));
            }
            #endregion

            #region Load Dữ Liệu Phụ cấp bất thường
            if (type == CatElementType.AllowancesOut.ToString())
            {
                List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg = new List<Cat_UnusualAllowanceCfgEntity>();
                lstModel = new List<object>();
                lstModel.AddRange(new object[6]);
                lstModel[4] = 1;
                lstModel[5] = Int32.MaxValue - 1;
                listUnusualAllowanceCfg = baseService.GetData<Cat_UnusualAllowanceCfgEntity>(lstModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).ToList();

                List<CatElementModel> listResult = new List<CatElementModel>();
                if (listUnusualAllowanceCfg != null)
                {
                    foreach (var i in listUnusualAllowanceCfg)
                    {
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code, Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-1", ElementCode = i.Code + "_N_1", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-2", ElementCode = i.Code + "_N_2", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-3", ElementCode = i.Code + "_N_3", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-4", ElementCode = i.Code + "_N_4", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-5", ElementCode = i.Code + "_N_5", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng N-6", ElementCode = i.Code + "_N_6", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = "Số tiền bù " + i.UnusualAllowanceCfgName + " Tháng N-1", ElementCode = i.Code + "_AMOUNTOFOFFSET_N_1", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Tháng 3", ElementCode = i.Code + "_T3", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Theo Timeline", ElementCode = i.Code + "_TIMELINE", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Theo Timeline Tháng N-1", ElementCode = i.Code + "_TIMELINE_N_1", Formula = i.Code });

                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Theo Kỳ Chốt Lương", ElementCode = i.Code + "_DAYCLOSE", Formula = i.Code });
                        listResult.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName + " Theo Kỳ Chốt Lương", ElementCode = i.Code + "_DAYCLOSE_N_1", Formula = i.Code });

                    }
                }
                return Json(listResult.ToDataSourceResult(request));
            }
            #endregion

            #region Ứng lương
            if (type == CatElementType.AdvancePayment.ToString())
            {
                List<CatElementModel> listAdvancePayment = new List<CatElementModel>();
                return Json(listAdvancePayment.ToDataSourceResult(request));
            }
            #endregion

            #region Công
            if (type == CatElementType.Attendance.ToString())
            {
                lstModel = new List<object>();
                lstModel.AddRange(new object[7]);
                lstModel[2] = type;
                lstModel[5] = 1;
                lstModel[6] = Int32.MaxValue - 1;
                var listAtt = baseService.GetData<CatElementModel>(lstModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.IsApplyGradeAll != true);
                return Json(listAtt.ToDataSourceResult(request));
            }
            #endregion

            #region Chuyến bay
            if (type == CatElementType.FLIGHT.ToString())
            {
                lstModel = new List<object>();
                lstModel.AddRange(new object[7]);
                lstModel[2] = type;
                lstModel[5] = 1;
                lstModel[6] = Int32.MaxValue - 1;
                var listAtt = baseService.GetData<CatElementModel>(lstModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.IsApplyGradeAll != true);
                return Json(listAtt.ToDataSourceResult(request));
            }
            #endregion

            #region Đánh giá
            if (type == CatElementType.Evaluation.ToString())
            {
                lstModel = new List<object>();
                lstModel.AddRange(new object[7]);
                lstModel[2] = type;
                lstModel[5] = 1;
                lstModel[6] = Int32.MaxValue - 1;
                var listAtt = baseService.GetData<CatElementModel>(lstModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.IsApplyGradeAll != true);
                return Json(listAtt.ToDataSourceResult(request));
            }
            #endregion

            lstModel = new List<object>();
            lstModel.AddRange(new object[7]);
            lstModel[2] = type;
            lstModel[3] = GradePayroll;
            lstModel[4] = TabType;
            lstModel[5] = 1;
            lstModel[6] = Int32.MaxValue - 1;
            var listEntity = baseService.GetData<CatElementModel>(lstModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.MethodPayroll == Method).ToList();
            return Json(listEntity.ToDataSourceResult(request));
        }

        public ActionResult GetFormulaTemplate([DataSourceRequest] DataSourceRequest request, string Type, Guid GradeID)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Cat_FormulaTemplateEntity> listCat_FormulaTemplate = baseService.GetData<Cat_FormulaTemplateEntity>(listModel, ConstantSql.hrm_cat_sp_get_FormulaTemplate, UserLogin, ref status).Where(m => m.Type == Type && m.GradeID == GradeID).ToList();

            string RootPath = Common.GetPath("FormulaTemplate\\TemplateFormula.xml");
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            FileStream fs = new FileStream(RootPath, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("FormulaTemplateNode");

            foreach (XmlNode i in xmlnode)
            {
                Cat_FormulaTemplateEntity item = new Cat_FormulaTemplateEntity();
                item.ElementCode = i.Attributes["ElementName"].Value;
                item.ElementName = i.Attributes["ElementCode"].Value;
                item.Invisible = false;
                item.IsBold = false;
                item.Type = i.Attributes["Type"].Value;
                item.Formula = i.Attributes["Formula"].Value;

                if (!listCat_FormulaTemplate.Any(m => m.ElementCode == item.ElementCode))
                {
                    listCat_FormulaTemplate.Add(item);
                }
            }
            return Json(listCat_FormulaTemplate.Where(m => m.Type == Type).ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy dữ liệu Enum cho lưới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetElementInEnum([DataSourceRequest] DataSourceRequest request)
        {
            IList<SelectListItem>
             valuesAsList = Enum.GetValues(typeof(PayrollElement))
             .Cast<PayrollElement>
                 ()
                 .Select(x => new SelectListItem { Value = x.ToString(), Text = EnumDropDown.GetEnumDescription(x) })
                 .ToList();
            List<CatElementModel> listModel = new List<CatElementModel>();
            foreach (var i in valuesAsList)
            {
                listModel.Add(new CatElementModel() { ElementCode = i.Value.ToString(), ElementName = i.Text.ToString(), Formula = "[" + i.Value.ToString() + "]" });
            }
            return Json(listModel.ToDataSourceResult(request));
        }

        public ActionResult GetInsuranceElementInEnum([DataSourceRequest] DataSourceRequest request)
        {
            IList<SelectListItem>
             valuesAsList = Enum.GetValues(typeof(InsuranceElement))
             .Cast<InsuranceElement>
                 ()
                 .Select(x => new SelectListItem { Value = x.ToString(), Text = EnumDropDown.GetEnumDescription(x) })
                 .ToList();
            List<CatElementModel> listModel = new List<CatElementModel>();
            foreach (var i in valuesAsList)
            {
                listModel.Add(new CatElementModel() { ElementCode = i.Value.ToString(), ElementName = i.Text.ToString(), Formula = "[" + i.Value.ToString() + "]" });
            }
            return Json(listModel.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Hien.Nguyen] Tạo ra các phần tử tính lương theo mã loại ngày nghỉ và tăng ca
        /// Mỗi loại tăng ca và mỗi lại ngày nghỉ đều lưu lại là phần tử
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateFirstElement()
        {
            #region GetData
            ActionService action = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            var listLeaveday = baseService.GetData<Cat_LeaveDayTypeEntity>(listobject, ConstantSql.hrm_cat_sp_get_LeaveDayType, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[5]);
            listobject[3] = 1;
            listobject[4] = Int32.MaxValue - 1;
            var listOvertimeType = baseService.GetData<Cat_OvertimeTypeEntity>(listobject, ConstantSql.hrm_cat_sp_get_OvertimeType, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[7]);
            listobject[5] = 1;
            listobject[6] = Int32.MaxValue - 1;
            var listElement = baseService.GetData<Cat_ElementEntity>(listobject, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            var listGrade = baseService.GetData<Cat_GradePayrollMultiEntity>(listobject, ConstantSql.hrm_cat_sp_get_GradePayroll, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            var listKPIBonus = baseService.GetData<Cat_KPIBonusEntity>(listobject, ConstantSql.hrm_cat_sp_get_KPIBonus, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[3]);
            listobject[1] = 1;
            listobject[2] = Int32.MaxValue - 1;
            var listSalesType = baseService.GetData<Eva_SalesTypeEntity>(listobject, ConstantSql.hrm_eva_sp_get_SalesType, UserLogin, ref status);

            //listobject = new List<object>();
            //listobject.AddRange(new object[5]);
            //listobject[3] = 1;
            //listobject[4] = Int32.MaxValue - 1;
            //var listTitle = baseService.GetData<Cat_JobTitleEntity>(listobject, ConstantSql.hrm_cat_sp_get_JobTitle, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            var listPosition = baseService.GetData<Cat_PositionEntity>(listobject, ConstantSql.hrm_cat_sp_get_Position, UserLogin, ref status);

            List<Cat_RoleEntity> listCat_Role = new List<Cat_RoleEntity>();
            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            listCat_Role = baseService.GetData<Cat_RoleEntity>(listobject, ConstantSql.hrm_cat_sp_get_Role, UserLogin, ref status).ToList();

            List<Cat_JobTypeEntity> listCat_JobType = new List<Cat_JobTypeEntity>();
            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            listCat_JobType = baseService.GetData<Cat_JobTypeEntity>(listobject, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status).ToList();

            List<Cat_ShiftEntity> listCat_Shift = new List<Cat_ShiftEntity>();
            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            listCat_Shift = baseService.GetData<Cat_ShiftEntity>(listobject, ConstantSql.hrm_cat_sp_get_Shift, UserLogin, ref status).ToList();

            listobject = new List<object>();
            listobject.AddRange(new object[4]);
            listobject[2] = 1;
            listobject[3] = Int32.MaxValue - 1;
            var listCurrency = baseService.GetData<Cat_CurrencyEntity>(listobject, ConstantSql.hrm_cat_sp_get_Currency, UserLogin, ref status);

            listobject = new List<object>();
            listobject.AddRange(new object[3]);
            listobject[1] = 1;
            listobject[2] = Int32.MaxValue - 1;
            var listPerformanceType = baseService.GetData<Cat_PerformanceTypeEntity>(listobject, ConstantSql.hrm_cat_sp_get_PerformanceType, UserLogin, ref status);
            #endregion

            List<Cat_ElementEntity> listModel = new List<Cat_ElementEntity>();

            #region Xóa hết các phần tử
            List<Cat_ElementEntity> _temp = listElement.Where(m => m.ElementType == CatElementType.Comission.ToString() || m.ElementType == CatElementType.Evaluation.ToString() || m.ElementType == CatElementType.Attendance.ToString() || m.ElementType == CatElementType.FLIGHT.ToString()).ToList();
            if (_temp != null && _temp.Count > 0)
            {
                foreach (var i in _temp)
                {
                    baseService.Delete<Cat_ElementEntity>(i.ID);
                    //listElement.Remove(i);
                }
            }
            #endregion

            #region Add các phần tử ngày nghỉ
            if (listLeaveday != null && listLeaveday.Count > 0)
            {
                foreach (var i in listLeaveday)
                {
                    if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_HOURS"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_LEAVE_" + i.Code + "_HOURS";
                        item.ElementName = "Tổng giờ nghỉ loại " + i.LeaveDayTypeName + " trong tháng";
                        item.Formula = "[ATT_LEAVE_" + i.Code + "_HOURS]";
                        item.Description = i.Notes;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_HOURS" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_LEAVE_" + i.Code + "_HOURS";
                            item.ElementName = "Tổng giờ nghỉ loại " + i.LeaveDayTypeName + " trong tháng";
                            item.Formula = "[ATT_LEAVE_" + i.Code + "_HOURS]";
                            item.Description = i.Notes;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                    }

                    //Số ngày nghỉ tháng N-1
                    if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_DAY_PREV"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_LEAVE_" + i.Code + "_DAY_PREV";
                        item.ElementName = "Tổng ngày nghỉ loại " + i.LeaveDayTypeName + " trong tháng N-1";
                        item.Formula = "[ATT_LEAVE_" + i.Code + "_DAY_PREV]";
                        item.Description = i.Notes;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_DAY_PREV" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_LEAVE_" + i.Code + "_DAY_PREV";
                            item.ElementName = "Tổng ngày nghỉ loại " + i.LeaveDayTypeName + " trong tháng N-1";
                            item.Formula = "[ATT_LEAVE_" + i.Code + "_DAY_PREV]";
                            item.Description = i.Notes;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                    }

                    //tổng số ngày nghỉ của từng loại theo năm
                    if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_DAY_INYEAR"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_LEAVE_" + i.Code + "_DAY_INYEAR";
                        item.ElementName = "Tổng ngày nghỉ loại " + i.LeaveDayTypeName + " trong năm";
                        item.Formula = "[ATT_LEAVE_" + i.Code + "_DAY_INYEAR]";
                        item.Description = i.Notes;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_LEAVE_" + i.Code + "_DAY_INYEAR" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_LEAVE_" + i.Code + "_DAY_INYEAR";
                            item.ElementName = "Tổng ngày nghỉ loại " + i.LeaveDayTypeName + " trong năm";
                            item.Formula = "[ATT_LEAVE_" + i.Code + "_DAY_INYEAR]";
                            item.Description = i.Notes;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                    }
                }
            }
            #endregion

            #region Add các phần tử tiền tệ

            if (listCurrency != null)
            {
                foreach (var i in listCurrency)
                {
                    List<Cat_CurrencyEntity> listCurrencyRemoveCurrent = new List<Cat_CurrencyEntity>(listCurrency);
                    listCurrencyRemoveCurrent.Remove(i);
                    foreach (var j in listCurrencyRemoveCurrent)
                    {
                        //Giá mua
                        if (!listElement.Any(m => m.ElementName == i.Code + "_" + j.Code + "_BUYING"))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = i.Code + "_" + j.Code + "_BUYING";
                            item.ElementName = "Giá mua " + i.CurrencyName + "_" + j.CurrencyName;
                            item.Formula = "[" + i.Code + "_" + j.Code + "_BUYING]";
                            item.Description = i.Description;
                            item.ElementType = CatElementType.ExChangeRate.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                        else
                        {
                            if (!listElement.Any(m => m.ElementName == i.Code + "_" + j.Code + "_BUYING" && m.GradePayrollID == null))
                            {
                                Cat_ElementEntity item = new Cat_ElementEntity();
                                item.ElementCode = i.Code + "_" + j.Code + "_BUYING";
                                item.ElementName = "Giá mua " + i.CurrencyName + "_" + j.CurrencyName;
                                item.Formula = "[" + i.Code + "_" + j.Code + "_BUYING]";
                                item.Description = i.Description;
                                item.ElementType = CatElementType.ExChangeRate.ToString();
                                item.GradePayrollID = null;
                                item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                                listModel.Add(item);
                            }
                        }

                        //Giá bán
                        if (!listElement.Any(m => m.ElementName == i.Code + "_" + j.Code + "_SELLING"))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = i.Code + "_" + j.Code + "_SELLING";
                            item.ElementName = "Giá bán " + i.CurrencyName + "_" + j.CurrencyName;
                            item.Formula = "[" + i.Code + "_" + j.Code + "_SELLING]";
                            item.Description = i.Description;
                            item.ElementType = CatElementType.ExChangeRate.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                        else
                        {
                            if (!listElement.Any(m => m.ElementName == i.Code + "_" + j.Code + "_SELLING" && m.GradePayrollID == null))
                            {
                                Cat_ElementEntity item = new Cat_ElementEntity();
                                item.ElementCode = i.Code + "_" + j.Code + "_SELLING";
                                item.ElementName = "Giá bán " + i.CurrencyName + "_" + j.CurrencyName;
                                item.Formula = "[" + i.Code + "_" + j.Code + "_SELLING]";
                                item.Description = i.Description;
                                item.ElementType = CatElementType.ExChangeRate.ToString();
                                item.GradePayrollID = null;
                                item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                                listModel.Add(item);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Add các phần tử là tăng ca
            if (listOvertimeType != null && listOvertimeType.Count > 0)
            {
                foreach (var i in listOvertimeType)
                {
                    #region Tăng ca tháng N
                    if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS";
                        item.ElementName = i.OvertimeTypeName;
                        item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS]";
                        item.Description = i.Comment;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS";
                            item.ElementName = i.OvertimeTypeName;
                            item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS]";
                            item.Description = i.Comment;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);

                        }
                    }
                    #endregion

                    #region Tăng ca tháng N-1
                    if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS_PREV"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS_PREV";
                        item.ElementName = i.OvertimeTypeName + "Tháng N-1";
                        item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS_PREV]";
                        item.Description = i.Comment;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS_PREV" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS_PREV";
                            item.ElementName = i.OvertimeTypeName + "Tháng N-1";
                            item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS_PREV]";
                            item.Description = i.Comment;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                    }
                    #endregion

                    #region Tăng ca tháng N khi có thay đổi lương
                    if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS_AFTER"))
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS_AFTER";
                        item.ElementName = i.OvertimeTypeName + " sau khi thay đổi lương";
                        item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS_AFTER]";
                        item.Description = i.Comment;
                        item.ElementType = CatElementType.Attendance.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                    else
                    {
                        if (!listElement.Any(m => m.ElementName == "ATT_OVERTIME_" + i.Code + "_HOURS_AFTER" && m.GradePayrollID == null))
                        {
                            Cat_ElementEntity item = new Cat_ElementEntity();
                            item.ElementCode = "ATT_OVERTIME_" + i.Code + "_HOURS_AFTER";
                            item.ElementName = i.OvertimeTypeName + " sau khi thay đổi lương";
                            item.Formula = "[ATT_OVERTIME_" + i.Code + "_HOURS_AFTER]";
                            item.Description = i.Comment;
                            item.ElementType = CatElementType.Attendance.ToString();
                            item.GradePayrollID = null;
                            item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                            listModel.Add(item);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Add các phần tử là hoa hồng
            if (listKPIBonus != null && listKPIBonus.Count > 0)
            {
                foreach (var i in listKPIBonus)
                {
                    Cat_ElementEntity item = new Cat_ElementEntity();
                    item.ElementCode = CatElementType.Comission.ToString().ToUpper() + "_" + i.Code;
                    item.ElementName = i.KPIBonusName;
                    item.Formula = "[" + CatElementType.Comission.ToString().ToUpper() + "_" + i.Code + "]";
                    item.Description = i.Note;
                    item.ElementType = CatElementType.Comission.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);
                }
            }

            //các phần tử lương hoa hồng trong bảng cat_element
            List<Cat_ElementEntity> listElementByCommission = listElement.Where(m => m.MethodPayroll == MethodPayroll.E_COMMISSION_PAYMENT.ToString()).ToList();
            if (listElementByCommission.Count > 0)
            {
                foreach (var element in listElementByCommission)
                {
                    Cat_ElementEntity item = new Cat_ElementEntity();
                    item.ElementCode = "ELEMENT" + CatElementType.Comission.ToString().ToUpper() + "_" + element.ElementCode;
                    item.ElementName = element.ElementName;
                    item.Formula = "[ELEMENT" + CatElementType.Comission.ToString().ToUpper() + "_" + element.ElementCode + "]";
                    item.Description = element.Description;
                    item.ElementType = CatElementType.Comission.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);
                }
            }
            #endregion

            #region Add Các Phần Tử là số lượng chức vụ (position)
            if (listPosition != null && listPosition.Count > 0)
            {
                foreach (var i in listPosition)
                {
                    Cat_ElementEntity item = new Cat_ElementEntity();
                    item.ElementCode = CatElementType.Comission.ToString().ToUpper() + "_COUNTPOSITION_" + i.Code;
                    item.ElementName = i.PositionName;
                    item.Formula = "[" + CatElementType.Comission.ToString().ToUpper() + "_COUNTPOSITION_" + i.Code + "]";
                    item.Description = i.Description;
                    item.ElementType = CatElementType.Comission.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);
                }
            }
            #endregion

            #region Add các phần tử đánh giá
            if (listSalesType != null && listSalesType.Count > 0)
            {
                IList<string> List_EvaBonusType = Enum.GetValues(typeof(EvaBonusType))
                .Cast<EvaBonusType>()
                .Select(x => x.ToString())
                .ToList();
                foreach (var i in listSalesType)
                {
                    foreach (var j in List_EvaBonusType)
                    {
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = CatElementType.Evaluation.ToString().ToUpper() + "_" + i.Code + "_" + j;
                        item.ElementName = i.SalesTypeName;
                        item.Formula = "[" + CatElementType.Evaluation.ToString().ToUpper() + "_" + i.Code + "_" + j + "]";
                        item.Description = i.Note;
                        item.ElementType = CatElementType.Evaluation.ToString();
                        item.GradePayrollID = null;
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                }
            }
            if (listPerformanceType != null && listPerformanceType.Count > 0)
            {
                foreach (var i in listPerformanceType)
                {
                    Cat_ElementEntity item = new Cat_ElementEntity();
                    item.ElementCode = CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_" + i.Code;
                    item.ElementName = i.PerformanceTypeName;
                    item.Formula = "[" + CatElementType.Evaluation.ToString().ToUpper() + "_PERFORMANCETYPE_" + i.Code + "]";
                    item.Description = i.Description;
                    item.ElementType = CatElementType.Evaluation.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);
                }
            }
            #endregion

            #region Vietject
            #region Các phần tử là Đơn Giá Công Việc

            if (listCat_Role != null && listCat_Role.Count > 0 && listCat_JobType != null && listCat_JobType.Count > 0)
            {
                for (int i = 0; i < listCat_Role.Count; i++)
                {
                    for (int j = 0; j < listCat_JobType.Count; j++)
                    {
                        //Số giờ bay
                        Cat_ElementEntity item = new Cat_ElementEntity();
                        item.ElementCode = CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_HOURS";
                        item.ElementName = "Số giờ bay " + listCat_Role[i].RoleName + " " + listCat_JobType[j].JobTypeName;
                        item.Formula = "[" + CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_HOURS]";
                        item.ElementType = CatElementType.FLIGHT.ToString();
                        item.GradePayrollID = null;
                        item.Description = "";
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);

                        //Số chặn bay
                        item = new Cat_ElementEntity();
                        item.ElementCode = CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_ROUTES";
                        item.ElementName = "Số chặn bay " + listCat_Role[i].RoleName + " " + listCat_JobType[j].JobTypeName;
                        item.Formula = "[" + CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_ROUTES]";
                        item.ElementType = CatElementType.FLIGHT.ToString();
                        item.GradePayrollID = null;
                        item.Description = "";
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);

                        //Số tiền
                        item = new Cat_ElementEntity();
                        item.ElementCode = CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_AMOUNT";
                        item.ElementName = "Tiền " + listCat_Role[i].RoleName + " " + listCat_JobType[j].JobTypeName;
                        item.Formula = "[" + CatElementType.FLIGHT.ToString() + "_" + listCat_Role[i].Code.ReplaceSpace() + "_" + listCat_JobType[j].Code.ReplaceSpace() + "_AMOUNT]";
                        item.ElementType = CatElementType.FLIGHT.ToString();
                        item.GradePayrollID = null;
                        item.Description = "";
                        item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        listModel.Add(item);
                    }
                }
            }
            #endregion

            #endregion

            #region Honda - tổng số ngày làm việc theo từng ca của nhân viên trong tháng

            if (listCat_Shift != null && listCat_Shift.Count > 0)
            {
                foreach (var i in listCat_Shift)
                {
                    Cat_ElementEntity item = new Cat_ElementEntity();
                    item.ElementCode = "ATT_SHIFT" + "_" + i.Code + "_" + "DAY";
                    item.ElementName = "Tổng số ngày làm việc của ca" + i.ShiftName;
                    item.Formula = "[" + "ATT_SHIFT" + "_" + i.Code + "_" + "DAY" + "]";
                    item.Description = string.Empty;
                    item.ElementType = CatElementType.Attendance.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);

                    item = new Cat_ElementEntity();
                    item.ElementCode = "ATT_SHIFT" + "_" + i.Code + "_" + "DAY_PREV";
                    item.ElementName = "Tổng số ngày làm việc của ca" + i.ShiftName + " Tháng N-1";
                    item.Formula = "[" + "ATT_SHIFT" + "_" + i.Code + "_" + "DAY_PREV" + "]";
                    item.Description = string.Empty;
                    item.ElementType = CatElementType.Attendance.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);

                    item = new Cat_ElementEntity();
                    item.ElementCode = "ATT_SHIFT" + "_" + i.Code + "_" + "HOURS";
                    item.ElementName = "Tổng số giờ làm việc của ca" + i.ShiftName + " Tháng N-1";
                    item.Formula = "[" + "ATT_SHIFT" + "_" + i.Code + "_" + "HOURS" + "]";
                    item.Description = string.Empty;
                    item.ElementType = CatElementType.Attendance.ToString();
                    item.GradePayrollID = null;
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    listModel.Add(item);
                }
            }

            #endregion

        #endregion

            baseService.Add<Cat_ElementEntity>(listModel);

            return Json(true);
        }



        #region Cat_AdvancePayment
        public ActionResult GetAdvancePaymentList([DataSourceRequest] DataSourceRequest request, CatElementSearchModel model)
        {
            return GetListDataAndReturn<CatElementModel, Cat_ElementEntity, CatElementSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AdvancePayment);
        }
        #endregion

        #region Cat_EthnicGroup

        [HttpPost]
        public ActionResult GetEthnicGroupList([DataSourceRequest] DataSourceRequest request, CatEthnicGroupSearchModel model)
        {
            return GetListDataAndReturn<CatEthnicGroupModel, Cat_EthnicGroupEntity, CatEthnicGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_EthnicGroup);
        }

        public ActionResult ExportEthnicGroupSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_EthnicGroupEntity, CatEthnicGroupModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_EthnicGroupsByIds);
        }

        public ActionResult ExportAllEthnicGroup([DataSourceRequest] DataSourceRequest request, CatEthnicGroupSearchModel model)
        {
            return ExportAllAndReturn<Cat_EthnicGroupEntity, CatEthnicGroupModel, CatEthnicGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_EthnicGroup);
        }

        /// <summary>
        /// Lấy danh sách chức vụ
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEthnicGroup()
        {
            //var service = new Cat_EthnicGroupServices();
            var result = baseService.GetAllUseEntity<Cat_EthnicGroupEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEthnicGroupOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_EthnicGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EthnicGroup_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<Cat_EthnicGroupMultiModel> listModel = listEntity.Translate<Cat_EthnicGroupMultiModel>();
                listModel = listModel.OrderBy(s => s.EthnicGroupName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiEthnicGroup(string text)
        {
            return GetDataForControl<Cat_EthnicGroupMultiModel, Cat_EthnicGroupMultiModel>(text, ConstantSql.hrm_cat_sp_get_EthnicGroup_Multi);
        }
        #endregion



        #region Cat_ReqDocument

        [HttpPost]
        public ActionResult GetReqDocumentList([DataSourceRequest] DataSourceRequest request, Cat_ReqDocumentSearchModel model)
        {
            return GetListDataAndReturn<Cat_ReqDocumentModel, Cat_ReqDocumentEntity, Cat_ReqDocumentSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ReqDocument);
        }

        public ActionResult ExportCatReqDocumentSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ReqDocumentEntity, Cat_ReqDocumentModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ReqDocumentByIds);
        }

        public ActionResult ExportAllReqDocumentList([DataSourceRequest] DataSourceRequest request, Cat_ReqDocumentSearchModel model)
        {
            return ExportAllAndReturn<Cat_ReqDocumentEntity, Cat_ReqDocumentModel, Cat_ReqDocumentSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ReqDocument);

        }

        public JsonResult GetMultiReqDocument(string text)
        {
            return GetDataForControl<Cat_ReqDocumentModel, Cat_ReqDocumentEntity>(text, ConstantSql.hrm_hr_sp_get_ReqDocument_multi);
        }

        //public JsonResult GetReqDocumentCodeps(string codeemps)
        //{
        //    string status = string.Empty;
        //    var service = new Cat_ProfileServices();
        //    var result = service.GetData<Hre_ProfileMultiEntity>(codeemps, ConstantSql.hrm_hr_sp_get_ProfileByCodeEmps, ref status);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Cat_ExportItem

        //[HttpPost]
        public ActionResult GetExportItemList([DataSourceRequest] DataSourceRequest request, CatExportItemModel model)
        {
            return GetListDataAndReturn<CatExportItemModel, Cat_ExportItemEntity, CatExportItemModel>(request, model, ConstantSql.hrm_cat_sp_get_ExportItem);
        }



        //[HttpPost]
        public ActionResult GetExportItemByExportIDList([DataSourceRequest] DataSourceRequest request, Guid ExportID)
        {

            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ExportID);
            var result = baseService.GetData<Cat_ExportItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ExportItemByExportID, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));


            //string status = string.Empty;
            //var baseService = new BaseService();
            //var objs = new List<object>();
            //objs.Add(ExportID);

            //var service = new Cat_ExportItemServices();
            //var result = baseService.GetData<Cat_ExportItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ExportItemByExportID, ref status);
            //return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetPivotItemByPivotIDList([DataSourceRequest] DataSourceRequest request, Guid PivotID)
        {

            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(PivotID);
            var result = baseService.GetData<Cat_PivotItemEntity>(objs, ConstantSql.hrm_cat_sp_get_PivotItemByPivotID, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));


            //string status = string.Empty;
            //var baseService = new BaseService();
            //var objs = new List<object>();
            //objs.Add(ExportID);

            //var service = new Cat_ExportItemServices();
            //var result = baseService.GetData<Cat_ExportItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ExportItemByExportID, ref status);
            //return Json(result.ToDataSourceResult(request));
        }


        #endregion

        #region Cat_Export

        [HttpPost]
        public ActionResult GetExportList([DataSourceRequest] DataSourceRequest request, CatExportSearchModel model)
        {
            return GetListDataAndReturn<CatExportModel, Cat_ExportEntity, CatExportSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Export);

        }
        [HttpPost]
        public ActionResult GetPivotList([DataSourceRequest] DataSourceRequest request, Cat_PivotSearchModel model)
        {
            return GetListDataAndReturn<Cat_PivotModel, Cat_PivotEntity, Cat_PivotSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Pivot);
        }

        public JsonResult GetMultiExport(string text)
        {
            return GetDataForControl<CatExportMultiModel, Cat_ExportMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Export_multi);
        }

        public JsonResult GetMultiExportWord(string text)
        {
            return GetDataForControl<CatExportMultiModel, Cat_ExportMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ExportWord_multi);
        }

        public JsonResult GetMultiReportMapping(string text)
        {
            return GetDataForControl<CatExportMultiModel, Cat_ExportMultiEntity>(text, ConstantSql.hrm_cat_sp_get_reportMapping_multi);
        }

        [HttpPost]
        public ActionResult GetExportWordList([DataSourceRequest] DataSourceRequest request, CatExportSearchModel model)
        {
            return GetListDataAndReturn<CatExportModel, Cat_ExportEntity, CatExportSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ExportWord);
        }
        #endregion

        #region Cat_GradeAttendance

        [HttpPost]
        public ActionResult GetGradeAttendanceList([DataSourceRequest] DataSourceRequest request, Cat_GradeAttendanceSearchModel model)
        {
            return GetListDataAndReturn<Cat_GradeAttendanceModel, Cat_GradeAttendanceEntity, Cat_GradeAttendanceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendance);
        }

        public JsonResult GetMultiGradeAttendance(string text)
        {
            return GetDataForControl<Cat_GradeAttendanceMultiModel, Cat_GradeAttendanceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_GradeAttendance_Multi);
        }


        public ActionResult ExportAllGradeAttendancelList([DataSourceRequest] DataSourceRequest request, Cat_GradeAttendanceSearchModel model)
        {
            return ExportAllAndReturn<Cat_GradeAttendanceEntity, Cat_GradeAttendanceModel, Cat_GradeAttendanceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendance);
        }

        public ActionResult ExportGradeAttendanceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_GradeAttendanceEntity, Cat_GradeAttendanceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendanceByIds);
        }

        #endregion

        #region Cat_ImportItem
        public ActionResult GetAllCatImportItem([DataSourceRequest] DataSourceRequest request, CatImportItemModel model)
        {
            return GetListDataAndReturn<CatImportItemModel, Cat_ImportItemEntity, CatImportItemModel>(request, model, ConstantSql.hrm_cat_sp_get_ImportItem);
        }
        public JsonResult GetMultiChildField(string text, string objectNameRoot, string objectName)
        {
            //if (!string.IsNullOrEmpty(objectName) && objectName.LastIndexOf("2") == objectName.Length - 1)
            //{
            //   // objectName = objectName.Substring(0, objectName.Length - 1);
            //}

            string status = string.Empty;
            var modules = typeof(HRM.Infrastructure.Utilities.ModuleKey).GetEnumNames();
            if (objectName.Length > 3 && modules.Contains(objectName.Substring(0, 3)))
            {
                var service = new Cat_ImportItemServices();

                #region Code cũ (sử dụng store)
                //List<object> objs = new List<object>();
                //objs.Add(text ?? string.Empty);
                //objs.Add(objectName);
                //objs.Add(1);
                //objs.Add(100);
                //    var get1 = service.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status);
                //var get = service.GetChildFiledMulti(text, objectName); 
                #endregion

                var get = service.GetTableNames(objectNameRoot, objectName).Select(p => new
                {
                    ID = p,
                    Name = p
                }).ToList();

                var result = get.Select(item => new CatChildFieldsMultiModel()
                {
                    ID = item.ID,
                    Name = item.Name
                });
                result = result.OrderBy(p => p.Name);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        #endregion



        #region Cat_AppendixContractType

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng loại phụ lục hợp đồng (Cat_AppendixContractType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAppendixContractTypeList([DataSourceRequest] DataSourceRequest request, Cat_AppendixContractTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_AppendixContractTypeModel, Cat_AppendixContractTypeEntity, Cat_AppendixContractTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AppendixContractType);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho loại phụ lục hợp đồng (Cat_AppendixContractType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllAppendixContractTypeList([DataSourceRequest] DataSourceRequest request, Cat_AppendixContractTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_AppendixContractTypeEntity, Cat_AppendixContractTypeModel, Cat_AppendixContractTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AppendixContractType);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của loại phụ lục hợp đồng (Cat_AppendixContractType) theo điều kiện tìm kiếm
        public ActionResult ExportAppendixContractTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_AppendixContractTypeEntity, Cat_AppendixContractTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_AppendixContractTypeByIds);
        }

        #endregion

        #region Cat_RewardedType

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng loại Khen Thưởng (Cat_RewardedType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardedTypeList([DataSourceRequest] DataSourceRequest request, Cat_RewardedTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_RewardedTypeModel, Cat_RewardedTypeEntity, Cat_RewardedTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_RewardedType);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho loại Khen Thưởng (Cat_RewardedType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllRewardedTypeList([DataSourceRequest] DataSourceRequest request, Cat_RewardedTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_RewardedTypeEntity, Cat_RewardedTypeModel, Cat_RewardedTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_RewardedType);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn củaloại Khen Thưởng (Cat_RewardedType) theo điều kiện tìm kiếm
        public ActionResult ExportRewardedTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RewardedTypeEntity, Cat_RewardedTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_RewardedTypeByIds);
        }

        public JsonResult GetMultiRewardedType(string text)
        {
            return GetDataForControl<Cat_RewardedTypeMultiModel, Cat_RewardedTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_RewardType_multi);
        }

        #endregion

        #region Cat_SalaryClassType
        public JsonResult GetMultiSalaryClassType(string text)
        {
            return GetDataForControl<Cat_SalaryClassTypeMultiModel, Cat_SalaryClassTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryClassType_multi);
        }
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng loại mã lương (Cat_SalaryClassType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSalaryClassTypeList([DataSourceRequest] DataSourceRequest request, Cat_SalaryClassTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_SalaryClassTypeModel, Cat_SalaryClassTypeEntity, Cat_SalaryClassTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryClassType);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho loại mã lương (Cat_SalaryClassType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSalaryClassTypeList([DataSourceRequest] DataSourceRequest request, Cat_SalaryClassTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_SalaryClassTypeEntity, Cat_SalaryClassTypeModel, Cat_SalaryClassTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryClassType);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của loại mã lương (Cat_SalaryClassType) theo điều kiện tìm kiếm
        public ActionResult ExportSalaryClassTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SalaryClassTypeEntity, Cat_SalaryClassTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassTypeByIds);
        }

        #endregion

        #region Cat_AccidentType
        //public JsonResult GetMultiAccidentType(string text)
        //{
        //    return GetDataForControl<Cat_AccidentTypeMultiModel, Cat_AccidentTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AccidentType_Multi);
        //}
        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng loại tai nạn lao động (Cat_AccidentType)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAccidentTypeList([DataSourceRequest] DataSourceRequest request, Cat_AccidentTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_AccidentTypeModel, Cat_AccidentTypeEntity, Cat_AccidentTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AccidentType);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho loại tai nạn lao động (Cat_AccidentType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllAccidentTypeList([DataSourceRequest] DataSourceRequest request, Cat_AccidentTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_AccidentTypeEntity, Cat_AccidentTypeModel, Cat_AccidentTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AccidentType);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của loại tai nạn lao động (Cat_AccidentType) theo điều kiện tìm kiếm
        public ActionResult ExportAccidentTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_AccidentTypeEntity, Cat_AccidentTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_AccidentTypeByIds);
        }

        #endregion

        #region Cat_Qualification

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Trình Độ Chuyên Môn (Cat_Qualification)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQualificationList([DataSourceRequest] DataSourceRequest request, Cat_QualificationSearchModel model)
        {
            return GetListDataAndReturn<CatQualificationModel, Cat_QualificationEntity, Cat_QualificationSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Qualification);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Trình Độ Chuyên Môn (Cat_Qualification) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllQualificationList([DataSourceRequest] DataSourceRequest request, Cat_QualificationSearchModel model)
        {
            return ExportAllAndReturn<Cat_QualificationEntity, CatQualificationModel, Cat_QualificationSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Qualification);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của Trình Độ Chuyên Môn (Cat_Qualification) theo điều kiện tìm kiếm
        public ActionResult ExportQualificationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_QualificationEntity, CatQualificationModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_QualificationByIds);
        }
        #endregion

        #region Cat_EducationLevel
        public JsonResult GetMultiCatNameEntityByType(string Type)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(Type, ConstantSql.hrm_cat_sp_get_NameEntityByType_multi);
        }

        public JsonResult GetMultiCutOffDurationType(string Type)
        {
            return GetDataForControl<Cat_CutOffDurationTypeMultiModel, Cat_CutOffDurationTypeMultiEntity>(Type, ConstantSql.hrm_cat_sp_get_CutOffDurationType_Multi);
        }

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Trình Độ Học Vấn (Cat_NameEntity)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEducationLevelList([DataSourceRequest] DataSourceRequest request, Cat_EducationLevelSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_EducationLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_EducationLevel);
        }

        [HttpPost]
        public ActionResult GetDisciplineReasonList([DataSourceRequest] DataSourceRequest request, Cat_DisciplineReasonSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_DisciplineReasonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DisciplineReason);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllDisciplineReasonList([DataSourceRequest] DataSourceRequest request, Cat_DisciplineReasonSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_DisciplineReasonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_DisciplineReason);
        }


        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Trình Độ Học Vấn (Cat_NameEntity) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllEducationLevelList([DataSourceRequest] DataSourceRequest request, Cat_EducationLevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_EducationLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_EducationLevel);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của Trình Độ Học Vấn (Cat_NameEntity) theo điều kiện tìm kiếm
        public ActionResult ExportEducationLevelSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_EducationLevelByIds);
        }

        public ActionResult GetMutilEducationLevel(string selectedIds)
        {
            var service = new BaseService();
            string status = "";
            var result = service.GetData<Cat_NameEntityEntity>(selectedIds, ConstantSql.hrm_cat_sp_get_EducationLevelByIds, UserLogin, ref status).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiDisciplineReason(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Discipline_Multi);
        }

        #endregion


        #region Cat_GraduatedLevel

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Trình Độ văn hóa (Cat_NameEntity)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGraduatedLevelList([DataSourceRequest] DataSourceRequest request, Cat_GraduatedLevelSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_GraduatedLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_GraduatedLevel);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Trình Độ Học Vấn (Cat_NameEntity) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllGraduatedLevelList([DataSourceRequest] DataSourceRequest request, Cat_GraduatedLevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_GraduatedLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_GraduatedLevel);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của Trình Độ Học Vấn (Cat_NameEntity) theo điều kiện tìm kiếm
        public ActionResult ExportGraduatedLevelSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_GraduatedLevelByIds);
        }

        public JsonResult GetMultiGraduatedLevel(string text)
        {
            return GetDataForControl<CatGraduatedLevelMultiModel, Cat_GraduatedLevelMultiEntity>(text, ConstantSql.hrm_cat_sp_get_GraduatedLevel_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContrExportAllCatShiftlListactTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }

        public JsonResult GetMultiLockObject(string text)
        {
            return GetDataForControl<CatLockObjectMultiModel, Cat_LockObjectMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LockObject_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContrExportAllCatShiftlListactTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }

        #endregion

        #region Cat_ReasonDeny
        public ActionResult GetReasonDenyList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ResonDeny);
        }
        public ActionResult ExportReasonDenySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ResonDenyByIds);
        }
        public ActionResult ExportAllReasonDenylList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ResonDeny);
        }
        public JsonResult GetMultiReasonDeny(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ReasonDeny_Multi);
        }
        #endregion

        #region Cat_LevelGeneral

        /// <summary>
        /// [Quoc.Do] - Lấy danh sách dữ liệu bảng Trình Độ theo type (Cat_NameEntity)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLevelGeneralList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }

        [HttpPost]
        public ActionResult GetLockObjectGeneralList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LockObject);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Trình Độ theo type (Cat_NameEntity) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllLevelGeneralList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của Trình Độ theo type (Cat_NameEntity) theo điều kiện tìm kiếm
        public ActionResult ExportLevelGeneralSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }

        #endregion

        #region Cat_BlackListReson
        public ActionResult GetBlackListResonList([DataSourceRequest] DataSourceRequest request, Cat_BlackListResonSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_BlackListResonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllBlackListResonList([DataSourceRequest] DataSourceRequest request, Cat_BlackListResonSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_BlackListResonSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportBlackListResonSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }

        public ActionResult GetMultiResonBlackList(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_BlackListReason_Multi);
        }
        #endregion
        #region Cat_TimeEvalutionData
        public ActionResult GetTimeEvalutionDataList([DataSourceRequest]DataSourceRequest request, Cat_TimeEvalutionDataSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_TimeEvalutionDataSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportAllTimeEvalutionDataList([DataSourceRequest]DataSourceRequest request, Cat_TimeEvalutionDataSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_TimeEvalutionDataSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportTimeEvalutionDataSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }
        public ActionResult GetMultiTimeEvalutionData(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TimeEvalutionData_Multi);
        }
        #endregion

        #region Cat_AreaPostJob - Vùng đăng tuyển

        public ActionResult GetAreaPostJobList([DataSourceRequest] DataSourceRequest request, Cat_AreaPostJobSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_AreaPostJobSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        // [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllAreaPostJobList([DataSourceRequest] DataSourceRequest request, Cat_AreaPostJobSearchModel model)
        {
            return ExportAllAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_AreaPostJobSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportAreaPostJobListSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }

        public ActionResult GetMultiAreaPostJob(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AreaPostJob_Multi);
        }

        #endregion
        #region Cat_TypeOfStop
        public ActionResult GetTypeOfStopList([DataSourceRequest]DataSourceRequest request, Cat_TypeOfStopSearchModel model)
        {
            return GetListDataAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_TypeOfStopSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportAllTypeOfStopList([DataSourceRequest]DataSourceRequest request, Cat_TypeOfStopSearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_TypeOfStopSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LevelGeneral);
        }
        public ActionResult ExportTypeOfStopSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }
        public ActionResult GetMultiTypeOfStop(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityEntity>(text, ConstantSql.hrm_cat_sp_get_TypeOfStop_Multi);
        }
        #endregion


        #region Cat_CutOffDurationType
        [HttpPost]
        public ActionResult GetCutOffDurationTypeList([DataSourceRequest] DataSourceRequest request, Cat_CutOffDurationTypeSearchModel model)
        {
            return GetListDataAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_CutOffDurationTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CutOffDurationType);
        }
        #endregion

        #region Cat_ConditionalColor
        [HttpPost]
        public ActionResult GetCatConditionalColor([DataSourceRequest] DataSourceRequest request, CatConditionalColorSearchModel model)
        {
            return GetListDataAndReturn<Cat_ConditionalColorModel, Cat_ConditionalColorEntity, CatConditionalColorSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_conditioncolor);
        }

        public ActionResult ExportCatConditionalColorSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ConditionalColorEntity, Cat_ConditionalColorModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_conditioncolorByIds);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllCatConditionalColor([DataSourceRequest] DataSourceRequest request, CatConditionalColorSearchModel model)
        {
            return ExportAllAndReturn<Cat_ConditionalColorEntity, Cat_ConditionalColorModel, CatConditionalColorSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_conditioncolor);
        }
        #endregion

        #region Cat_Pivot
        public ActionResult GetMultiPivot(string text)
        {
            return GetDataForControl<Cat_PivotMultiModel, Cat_PivotMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Pivot_Multi);
        }
        #endregion
        #region Cat_Import
        [HttpPost]
        public ActionResult GetCatImport([DataSourceRequest] DataSourceRequest request, Cat_ImportSearchModel model)
        {
            return GetListDataAndReturn<Cat_ImportEntity, Cat_ImportEntity, Cat_ImportSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Import);
        }

        public JsonResult GetMultiImport(string text)
        {
            string keyword = string.Empty;
            string obj = string.Empty;
            var arrText = text.Split('|');
            keyword = arrText[0];
            obj = arrText[1];

            if (arrText.Count() == 2 && obj != string.Empty)
            {
                if (keyword == null || keyword == " ")
                    keyword = string.Empty;
                var service = new BaseService();
                string status = string.Empty;
                var listEntity = service.GetData<Cat_ImportMultiEntity>(keyword, ConstantSql.hrm_cat_sp_get_Import_multi, UserLogin, ref status);
                if (listEntity != null)
                {
                    listEntity = listEntity.Where(s => s.ObjectName == obj).ToList();
                    List<CatImportMultiModel> listModel = listEntity.Translate<CatImportMultiModel>();
                    return Json(listModel, JsonRequestBehavior.AllowGet);
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }

            return GetDataForControl<CatImportMultiModel, Cat_ImportMultiEntity>(keyword, ConstantSql.hrm_cat_sp_get_Import_multi);
        }

        public JsonResult GetMultiObjectName(string text)
        {
            var listObj = new List<object> { text, 1, 500 };
            return GetData<CatSysTablesMultiModel, Cat_SysTablesMultiEntity>(listObj, ConstantSql.hrm_cat_sp_get_tables);
        }
        public JsonResult GetMultiFieldNameByTableName(string text)
        {
            var listObj = new List<object> { text, 1, 500 };
            return GetData<CatSysTablesMultiModel, Cat_SysTablesMultiEntity>(listObj, ConstantSql.hrm_cat_sp_get_FieldNameByTableName);
        }

        public JsonResult GetMultiObjectNameByObjName(string text)
        {
            var listObj = new List<object> { text, 1, 500 };
            return GetData<CatSysTablesMultiModel, Cat_SysTablesMultiEntity>(listObj, ConstantSql.hrm_cat_sp_get_tablesByTableName);
        }
        /// <summary>
        /// Dữ liệu đúng dùng để lưu
        /// </summary>
        private static Dictionary<Guid, IList> ListImportDataTemp { get; set; }

        /// <summary>
        /// Dữ liệu sai dùng để export ra lại excel.
        /// </summary>
        private static Dictionary<Guid, IList> ListInvalidDataTemp { get; set; }

        /// <summary>
        /// Dữ liệu sai dùng để export ra lại excel.
        /// </summary>
        private static Dictionary<Guid, ProgressEventArgs> ListComputePercent { get; set; }

        public ActionResult ImportValidate(CatImportModel Model)
        {
            if (ListComputePercent != null && ListComputePercent.ContainsKey(Model.UserID))
            {
                ListComputePercent.Remove(Model.UserID);
            }

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatImportModel>(Model, "Cat_Import_List", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            return Json(message);
        }
        public ActionResult ImportResultInterviewValidate(CatImportModel Model)
        {
            //if (ListComputePercent != null && ListComputePercent.ContainsKey(Model.UserID))
            //{
            //    ListComputePercent.Remove(Model.UserID);
            //}

            //#region Validate
            string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatImportModel>(Model, "Cat_Import_List", ref message);
            //if (!checkValidate)
            //{
            //    var ls = new object[] { "error", message };
            //    return Json(ls);
            //}
            //#endregion

            return Json(message);
        }


        public ContentResult ProgessChanged([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            if (ListComputePercent != null && ListComputePercent.ContainsKey(model.UserID))
            {
                var arg = ListComputePercent[model.UserID];
                model.Percent = arg.Percent.ToString();
                model.ProcessName = arg.Name;
                model.ProcessNameView = arg.Value;
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue - 1;
            var result = new ContentResult();
            result.Content = serializer.Serialize(model);
            result.ContentType = "text/json";
            return result;
        }

        [HttpPost]
        public ContentResult Import([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");

            ImportService importService = new ImportService
            {
                FileName = _fileName,
                UserID = model.UserID,
                ImportTemplateID = model.ID,
                DateTimeFormat = model.FormatDate,
                ImportMode = model.ProcessDuplicateData == HRM.Infrastructure.Utilities.EnumDropDown.DuplicateData.E_INSERT.ToString() ?
                ImportDataMode.Insert : model.ProcessDuplicateData == HRM.Infrastructure.Utilities.EnumDropDown.DuplicateData.E_UPDATE.ToString() ?
                ImportDataMode.Update : ImportDataMode.Skip
            };

            importService.ProgressChanged += importService_ProgressChanged;
            DataTable dataTableInvalid = new DataTable("InvalidData");
            DataTable dataTable = new DataTable("ImportData");

            string[] lstFieldInvalid = new string[] 
            { 
                "DataField", 
                "InvalidValue",
                "ExcelField", 
                "ExcelValue", 
                "ValueType", 
                "Desciption" 
            };

            try
            {
                importService.Import();
                dataTable = importService.ListImportData.Translate(importService.ListValueField.ToArray());
                dataTableInvalid = importService.ListInvalidData.Translate(lstFieldInvalid);

                if (model.UserID != Guid.Empty)
                {
                    if (ListImportDataTemp == null)
                    {
                        ListImportDataTemp = new Dictionary<Guid, IList>();
                    }

                    if (ListImportDataTemp.ContainsKey(model.UserID))
                    {
                        ListImportDataTemp[model.UserID] = importService.ListImportData;
                    }
                    else
                    {
                        ListImportDataTemp.Add(model.UserID, importService.ListImportData);
                    }

                    if (ListInvalidDataTemp == null)
                    {
                        ListInvalidDataTemp = new Dictionary<Guid, IList>();
                    }

                    if (ListInvalidDataTemp.ContainsKey(model.UserID))
                    {
                        ListInvalidDataTemp[model.UserID] = importService.ListInvalidData.Select(d => d.ImportData).Distinct().ToList();
                    }
                    else
                    {
                        ListInvalidDataTemp.Add(model.UserID, importService.ListInvalidData.Select(d => d.ImportData).Distinct().ToList());
                    }
                }
                else
                {
                    model.Description = "Người dùng ảo";
                }

                if (model.IsImportAndSave)
                {
                    if (importService.Save(model.UserID, importService.ListImportData, UserLogin))
                    {
                        model.Description = NotificationType.Success.ToString();
                    }
                    else
                    {
                        model.Description = NotificationType.Error.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }

            model.ListImportData = dataTable.ConfigTable().ToDataSourceResult(request);
            model.ListInvalidData = dataTableInvalid.ConfigTable().ToDataSourceResult(request);
            model.UrlInvalidFileName = ExportInvalidData(model.UserID, model.ID, importService);

            model.ListValueField = importService.ListValueField;
            model.ListDisplayField = lstFieldInvalid.ToList();

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue - 1;
            var result = new ContentResult();
            result.Content = serializer.Serialize(model);
            result.ContentType = "text/json";
            return result;
        }

        public ActionResult SaveImport([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            try
            {
                if (model.UserID != Guid.Empty && ListImportDataTemp != null
                    && ListImportDataTemp.ContainsKey(model.UserID))
                {
                    ImportService importService = new ImportService
                    {
                        UserID = model.UserID,
                        ImportTemplateID = model.ID,
                        DateTimeFormat = model.FormatDate,
                        ImportMode = model.ProcessDuplicateData == HRM.Infrastructure.Utilities.EnumDropDown.DuplicateData.E_INSERT.ToString() ?
                        ImportDataMode.Insert : model.ProcessDuplicateData == HRM.Infrastructure.Utilities.EnumDropDown.DuplicateData.E_UPDATE.ToString() ?
                        ImportDataMode.Update : ImportDataMode.Skip
                    };

                    importService.ProgressChanged += importService_ProgressChanged;

                    if (importService.Save(model.UserID, ListImportDataTemp[model.UserID], UserLogin))
                    {
                        ListImportDataTemp[model.UserID] = null;
                        ListImportDataTemp.Remove(model.UserID);
                        return Json(NotificationType.Success.ToString());
                    }
                    else
                    {
                        return Json(NotificationType.Error.ToString());
                    }
                }
                else
                {
                    return Json(NotificationType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                return Json(NotificationType.Error + "," + ex.GetExceptionMessage());
            }
        }

        public string ExportInvalidData(Guid userID, Guid templateID, ImportService importService)
        {
            var result = string.Empty;

            try
            {
                if (userID != Guid.Empty && ListInvalidDataTemp != null && ListInvalidDataTemp.ContainsKey(userID))
                {
                    result = importService.Export(templateID, ListInvalidDataTemp[userID], Common.GetPath(Common.DownloadURL));
                }
            }
            catch (Exception)
            {

            }

            return result;
        }


        void importService_ProgressChanged(ProgressEventArgs e)
        {
            if (e != null && !string.IsNullOrWhiteSpace(e.Value))
            {
                if (ListComputePercent == null)
                {
                    ListComputePercent = new Dictionary<Guid, ProgressEventArgs>();
                }

                if (ListComputePercent.ContainsKey(e.ID))
                {
                    ListComputePercent[e.ID] = e;
                }
                else
                {
                    ListComputePercent.Add(e.ID, e);
                }
            }
        }

        [HttpPost]
        public ActionResult ConvertFromPivot([DataSourceRequest] DataSourceRequest request, Cat_PivotModel model)
        {
            var _fileName = Common.GetPath(Common.TemplateURL) + model.PivotFileName;
            _fileName = _fileName.Replace("/", "\\");

            UnpivotService service = new UnpivotService
            {
                UserID = model.UserID,
                PivotTemplateID = model.ID,
                FileName = _fileName,
            };

            string[] outputFiles = service.Unpivot();
            if (!string.IsNullOrEmpty(outputFiles[0]))
            {
                outputFiles[0] = NotificationType.Success.ToString() + "," + outputFiles[0];
                return Json(outputFiles[0], JsonRequestBehavior.AllowGet);
            }
            return Json("-1", JsonRequestBehavior.AllowGet); ;
        }

        #endregion

        #region Cat_WorkPlace
        public ActionResult GetWorkPlaceList([DataSourceRequest] DataSourceRequest request, CatWorkPlaceSearchModel model)
        {
            return GetListDataAndReturn<CatWorkPlaceModel, Cat_WorkPlaceEntity, CatWorkPlaceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_WorkPlace);
        }

        public JsonResult GetWorkPlaceOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_WorkPlaceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_WorkPlace_Multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<CatWorkPlaceMultihModel> listModel = listEntity.Translate<CatWorkPlaceMultihModel>();
                listModel = listModel.OrderBy(s => s.WorkPlaceName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiWorkPlace(string text)
        {
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            var result = service.GetData<CatWorkPlaceMultihModel>(text, ConstantSql.hrm_cat_sp_get_WorkPlace_Multi, ref status).ToList();
            result = result.OrderBy(m => m.WorkPlaceName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_WorkPlace(Cat_WorkPlace) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllWorkPlaceList([DataSourceRequest] DataSourceRequest request, CatWorkPlaceSearchModel model)
        {
            return ExportAllAndReturn<Cat_WorkPlaceEntity, CatWorkPlaceModel, CatWorkPlaceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_WorkPlace);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_WorkPlace (Cat_WorkPlace) theo điều kiện tìm kiếm
        public ActionResult ExportWorkPlaceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_WorkPlaceEntity, CatWorkPlaceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        #endregion

        #region Cat_HDTJob

        [HttpPost]
        public ActionResult ApprovedHDTJobType(string selectedIds)
        {
            var service = new Cat_HDTJobTypeServices();
            var message = service.ActionApproved(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult GetHDTJobList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_HDTJobTypeModel, Cat_HDTJobTypeEntity, Cat_HDTJobTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobType);
        }

        [HttpPost]
        public ActionResult ApprovedHDTJobAll([DataSourceRequest] DataSourceRequest request, Cat_HDTJobTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_HDTJobTypeModel, Cat_HDTJobTypeEntity, Cat_HDTJobTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_set_ApprovedAllHDTJobType);
        }

        public JsonResult GetMultiHDTJob(string text)
        {
            return GetDataForControl<Cat_HDTJobTypeMultihModel, Cat_HDTJobTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_HDTJobType_Multi);
        }
        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_HDTJob(Cat_HDTJob) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllHDTJobTypeList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_HDTJobTypeEntity, Cat_HDTJobTypeModel, Cat_HDTJobTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobType);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_HDTJob (Cat_HDTJob) theo điều kiện tìm kiếm
        public ActionResult ExportHDTJobTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_HDTJobTypeEntity, Cat_HDTJobTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        #endregion

        #region Cat_Qualification
        public JsonResult GetMultiQualification(string text)
        {
            return GetDataForControl<CatQualificationMultiModel, Cat_QualificationMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Qualification_Multi);
        }

        public JsonResult GetMultiQualificationLevel(string text)
        {
            return GetDataForControl<CatQualificationLevelMultiModel, Cat_QualificationMultiLevelEntity>(text, ConstantSql.hrm_cat_sp_get_QualificationLevel_Multi);
        }
        #endregion
        public JsonResult GetScreenName(string screenName)
        {
            var service = new Cat_ExportServices();
            string status = string.Empty;
            var result = service.GetData<CatExportModel>(screenName, ConstantSql.hrm_cat_sp_get_Export_multi, UserLogin, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckByCar(string Ids)
        {
            Pur_MCAMService serv = new Pur_MCAMService();
            serv.CheckBuyCar(Ids, UserLogin);
            return null;
        }
        public JsonResult GetScreenNameWord(string screenName)
        {
            var service = new Cat_ExportServices();
            string status = string.Empty;
            var result = service.GetData<CatExportModel>(screenName, ConstantSql.hrm_cat_sp_get_ExportWord_multi, UserLogin, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// [Thong.Huynh] - 2014/05/28
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        #region GetRelativeTypeList
        //public JsonResult GetRelativeTypeList([DataSourceRequest] DataSourceRequest request, CatRelativeTypeSearchModel searchModel)
        //{
        //    var service = new Cat_RelativeTypeServices();
        //    ListQueryModel model = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(searchModel)
        //    };
        //    var result = service.GetRelativeType(model).ToList().Translate<CatRelativeTypeModel>();
        //    if (searchModel.IsExport)
        //    {
        //        var fullPath = ExportService.Export(result, searchModel.ValueFields.Split(','));
        //        return Json(fullPath);
        //    }
        //    request.Page = 1;
        //    var dataSourceResult = result.ToDataSourceResult(request);
        //    dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
        //    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        //} 
        #endregion
        #region Cat_RelativeType
        public ActionResult GetRelativeTypeList([DataSourceRequest] DataSourceRequest request, CatRelativeTypeSearchModel searchModel)
        {
            return GetListDataAndReturn<CatRelativeTypeModel, Cat_RelativeTypeEntity, CatRelativeTypeSearchModel>(request, searchModel, ConstantSql.hrm_cat_sp_get_RelativesType);
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Cat_RelativeType(Cat_RelativeType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllRelativeTypeList([DataSourceRequest] DataSourceRequest request, CatRelativeTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_RelativeTypeEntity, CatRelativeTypeModel, CatRelativeTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_RelativesType);
        }

        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn Cat_RelativeType (Cat_RelativeType) theo điều kiện tìm kiếm
        public ActionResult ExportRelativeTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RelativeTypeEntity, CatRelativeTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryClassByIds);
        }
        #endregion


        #region ExportRelativesTypeSelected old
        //public ActionResult ExportRelativesTypeSelected(string selectedIds, string valueFields)
        //{
        //    var service = new Cat_RelativeTypeServices();
        //    var result = service.GetRelativeTypeByIds(selectedIds).Translate<CatRelativeTypeModel>();
        //    var fullPath = ExportService.Export(result, valueFields.Split(','));
        //    return Json(fullPath);
        //} 
        #endregion
        public ActionResult ExportRelativesTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RelativeTypeEntity, CatRelativeTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_RelativesTypeIds);
        }
        /// <summary>
        /// [Thong.Huynh] - 2014/05/28
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        #region GetCatShiftList old
        //public JsonResult GetCatShiftList([DataSourceRequest] DataSourceRequest request, CatShiftSearchModel model)
        //{
        //    var service = new Cat_ShiftServices();
        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(model)
        //    };
        //    var result = service.GetCat_Shift(lstModel).ToList().Translate<CatShiftModel>();
        //    return Json(result.ToDataSourceResult(request));
        //} 
        #endregion
        public ActionResult GetCatShiftList([DataSourceRequest] DataSourceRequest request, CatShiftSearchModel searchModel)
        {

            return GetListDataAndReturn<CatShiftModel, Cat_ShiftEntity, CatShiftSearchModel>(request, searchModel, ConstantSql.hrm_cat_sp_get_Shift);
        }
        public ActionResult ExportAllCatShiftlList([DataSourceRequest] DataSourceRequest request, CatShiftSearchModel model)
        {
            return ExportAllAndReturn<Cat_ShiftEntity, CatShiftModel, CatShiftSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Shift);
        }
        public ActionResult ExportCatShiftSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ShiftEntity, CatShiftModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ShiftByIds);
        }




        #region Cat_Religion
        /// <summary>
        /// [Hieu.Van] - 2014/05/27
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReligionList([DataSourceRequest] DataSourceRequest request, CatReligionSearchModel model)
        {
            return GetListDataAndReturn<CatReligionModel, Cat_ReligionEntity, CatReligionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Religion);
        }

        public ActionResult ExportCatReligionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ReligionEntity, CatReligionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ReligionByIds);
        }

        public ActionResult ExportAllReligionList([DataSourceRequest] DataSourceRequest request, CatReligionSearchModel model)
        {
            return ExportAllAndReturn<Cat_ReligionEntity, CatReligionModel, CatReligionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Religion);
        }


        /// <summary>
        /// Lay61 danh sách tôn giáo
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReligion()
        {
            //var result = service.GetCatReligion().ToList().Translate<CatReligionModel>();
            var result = baseService.GetAllUseEntity<Cat_ReligionEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMultiReligion(string text)
        {
            return GetDataForControl<Cat_ReligionMultiModel, Cat_ReligionMultiModel>(text, ConstantSql.hrm_cat_sp_get_religion_Multi);
        }
        #endregion

        /// <summary>
        /// [Thong.Huynh] - 2014/05/28
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>


        /// <summary>
        /// [Tam.Le] - 7.8.2014 - Lấy dữ liệu "Cat_TAMScanReasonMiss","Can_MealAllowanceTypeSetting" theo Id cua Cat_TAMScanReasonMiss
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GetTAMScanReasonMiss_ById(Guid Id)
        {

            string status = string.Empty;
            var model = new Cat_TAMScanReasonMissModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_TAMScanReasonMissEntity>(Id, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss_ById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_TAMScanReasonMissModel>();
            }
            return Json(model);
            //var result = new List<Cat_TAMScanReasonMissModel>();
            //string status = string.Empty;
            //if (Id > 0)
            //{
            //    var service = new Cat_TAMScanReasonMissServices();
            //    result = service.GetData<Cat_TAMScanReasonMissModel>(Id, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss_ById, ref status);

            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Chuc.Nguyen] - 2014/05/30
        /// Lấy danh sách loại nhân viên dùng cho các control có datasource trừ grid
        /// </summary>
        /// <returns></returns>
        //public JsonResult GetCostCentre()
        //{
        //    var service = new Cat_CostCentreServices();
        //    var result = service.Get().ToList().Translate<CatCostCentreModel>();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetShift()
        {
            var service = new Cat_ShiftServices();
            //var data = service.GetCatShift();
            //var data = baseService.GetAllUseEntity<Cat_ShiftEntity>(ref _status);
            var data = baseService.GetDataNotParam<Cat_ShiftMultiEntity>(ConstantSql.hrm_cat_sp_get_Shift_multi, UserLogin, ref _status);
            var result = data.Select(item => new Cat_ShiftMultiEntity()
            {
                ID = item.ID,
                ShiftName = item.ShiftName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hieu.Van] - 2014/05/27
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        #region GetRegionList
        //[HttpPost]
        //public ActionResult GetRegionList([DataSourceRequest] DataSourceRequest request, CatRegionSearchModel _model)
        //{
        //    var service = new Cat_RegionServices();

        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(_model)
        //    };
        //    var result = service.GetCatRegion(_model.RegionName, _model.Code).ToList().Translate<CatRegionModel>();

        //    return Json(result.ToDataSourceResult(request));
        //} 
        #endregion
        [HttpPost]
        public ActionResult GetRegionList([DataSourceRequest] DataSourceRequest request, CatRegionSearchModel model)
        {
            return GetListDataAndReturn<CatRegionModel, Cat_RegionEntity, CatRegionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Region);
        }

        public ActionResult ExportCatRegionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RegionEntity, CatRegionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_RegionByIds);
        }

        public ActionResult ExportAllRegionList([DataSourceRequest] DataSourceRequest request, CatRegionSearchModel model)
        {
            return ExportAllAndReturn<Cat_RegionEntity, CatRegionModel, CatRegionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Region);
        }

        /// <summary>
        /// [Chuc.Nguyen] - 2014/05/27
        /// Lấy dữ liệu load lên lưới bằng cùng điều kiện tìm kiếm cho catbank
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AGetBankList([DataSourceRequest] DataSourceRequest request, CatBankSearchModel model)
        {

            return null;
        }

        [HttpPost]
        public ActionResult GetImportItemByImportIDList([DataSourceRequest] DataSourceRequest request, Guid? ImportID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ImportID);
            var result = baseService.GetData<Cat_ImportItemEntity>(objs, ConstantSql.hrm_cat_sp_get_ImportItemByImportID, UserLogin, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request));
            }

            return null;
        }

        [HttpPost]
        public ActionResult GetSyncItemBySyncIDList([DataSourceRequest] DataSourceRequest request, Guid? SyncID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(SyncID);
            var result = baseService.GetData<Cat_SyncItemEntity>(objs, ConstantSql.hrm_cat_sp_get_SyncItemBySyncID, UserLogin, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request));
            }

            return null;
        }



        public JsonResult GetMultiTamScanReasonMiss(string text)
        {
            return GetDataForControl<Cat_TAMScanReasonMissMuitlModel, Cat_TAMScanReasonMissMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TamScanReasonMiss_multi);
        }



        #region Check Trùng dữ liệu
        #region TAMScanMissReason
        [HttpPost]
        public ActionResult CheckDuplicateMissReason(string code, int id)
        {
            var isDuplicate = false;
            var service = new Cat_TAMScanReasonMissServices();
            var isDuplicateData = service.IsDuplication(code, id);
            if (isDuplicateData)
            {
                isDuplicate = true;
            }
            return Json(isDuplicate);
        }
        #endregion
        #endregion

        #region General
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
        #endregion

        #region PayrollGroup

        [HttpPost]
        public ActionResult GetPayrollGroupList([DataSourceRequest] DataSourceRequest request, Cat_PayrollGroupSearchModel model)
        {
            return GetListDataAndReturn<Cat_PayrollGroupModel, Cat_PayrollGroupEntity, Cat_PayrollGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_payrollGroup);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllPayrollGroupList([DataSourceRequest] DataSourceRequest request, Cat_PayrollGroupSearchModel model)
        {
            return ExportAllAndReturn<Cat_PayrollGroupEntity, Cat_PayrollGroupModel, Cat_PayrollGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_payrollGroup);
        }



        public ActionResult ExportPayrollGroupSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_PayrollGroupEntity, Cat_PayrollGroupModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_PayrollGroupByIds);
        }
        public JsonResult GetPayrollGroupOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_PayrollGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_PayrollGroup_multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<Cat_PayrollGroupMultiModel> listModel = listEntity.Translate<Cat_PayrollGroupMultiModel>();
                listModel = listModel.OrderBy(s => s.PayrollGroupName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiPayrollGroup(string text)
        {
            return GetDataForControl<Cat_PayrollGroupMultiModel, Cat_PayrollGroupMultiEntity>(text, ConstantSql.hrm_cat_sp_get_PayrollGroup_multi);
        }

        #endregion
        #region AccountType
        [HttpPost]
        public ActionResult GetAccountTypeList([DataSourceRequest] DataSourceRequest request, Cat_AccountTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_AccountTypeModel, Cat_AccountTypeEntity, Cat_AccountTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AccountType);
        }
        /// [Phuoc.Le] - Xuất danh sách dữ liệu choTrợ Cấp (Cat_AccountType) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllAccountTypeList([DataSourceRequest] DataSourceRequest request, Cat_AccountTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_AccountTypeEntity, Cat_AccountTypeModel, Cat_AccountTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AccountType);
        }

        /// [Phuoc.Le] - Xuất các dòng dữ liệu được chọn của  Trợ Cấp (Cat_AccountType) theo điều kiện tìm kiếm

        public ActionResult ExportAccountTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_AccountTypeEntity, Cat_AccountTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_AccountTypeByIds);
        }

        public JsonResult GetMultiAccountType(string text)
        {
            return GetDataForControl<Cat_AccountTypeMultiModel, Cat_AccountTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AccountType_Multi);
        }



        #endregion
        #region Cat_UnusualAllowanceCfg
        [HttpPost]
        public ActionResult GetUnusualAllowanceCfgList([DataSourceRequest] DataSourceRequest request, CatUnusualAllowanceCfgSearchModel model)
        {
            return GetListDataAndReturn<Cat_UnusualAllowanceCfgModel, Cat_UnusualAllowanceCfgEntity, CatUnusualAllowanceCfgSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg);
        }

        public JsonResult UnusualAllowanceCfg_multi(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_UnusualAllowanceCfgMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg_multi, UserLogin, ref status);
            if (listEntity != null)
            {
                List<Cat_UnusualAllowanceCfgMuitlModel> listModel = listEntity.Translate<Cat_UnusualAllowanceCfgMuitlModel>();
                listModel = listModel.OrderBy(s => s.UnusualAllowanceCfgName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CatUnusualAllowanceCfg_multi(string text)
        {
            return GetDataForControl<Cat_UnusualAllowanceCfgMuitlModel, Cat_UnusualAllowanceCfgMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg_multi);
            //if (text == null || text == " ")
            //    text = string.Empty;
            //var service = new BaseService();
            //string status = string.Empty;
            //var listEntity = service.GetData<Cat_UnusualAllowanceCfgMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg_multi, ref status);
            //if (listEntity != null)
            //{
            //    List<Cat_UnusualAllowanceCfgMuitlModel> listModel = listEntity.Translate<Cat_UnusualAllowanceCfgMuitlModel>();
            //    listModel = listModel.OrderBy(s => s.UnusualAllowanceCfgName).ToList();
            //    return Json(listModel, JsonRequestBehavior.AllowGet);
            //}
            //return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportAllUnusualAllowanceCfgList([DataSourceRequest] DataSourceRequest request, CatUnusualAllowanceCfgSearchModel model)
        {
            request.PageSize = int.MaxValue - 1;
            return ExportAllAndReturn<Cat_UnusualAllowanceCfgEntity, Cat_UnusualAllowanceCfgModel, CatUnusualAllowanceCfgSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg);
        }

        public ActionResult ExportUnusualAllowanceCfgSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_UnusualAllowanceCfgEntity, Cat_UnusualAllowanceCfgModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfgIds);
        }


        public JsonResult GetMultiUnusualAllowanceCfg(string text, string type)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(text);
            objs.Add(type != string.Empty ? type : null);
            objs.Add(null);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Chỉ lấy loại PC là bất thường (E_UNUSUALALLOWANCE)
        public JsonResult GetMultiUnusualAllowanceEDCfg(string text, string type)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(text);
            objs.Add(null);
            objs.Add(null);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //loc theo khoang nhan hay khoang tru
        public JsonResult GetMultiUnusualAllowanceEDCfgByEDType(string text, string type)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(text);
            objs.Add(type);
            objs.Add(null);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Chỉ lấy loại PC là thường xuyên (E_ALLOWANCE)
        public JsonResult GetMultiUnusualAllowanceUnCfg(string text, string type)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(text);
            objs.Add(type);
            objs.Add(HRM.Infrastructure.Utilities.EnumDropDown.UnusualEDType.E_ALLOWANCE.ToString());
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceEDCfg, UserLogin, ref status);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiUnusualAllowanceCfgPaidLeave(string text, string type)
        {
            //if (text == string.Empty || text == null)
            //{
            //    text = "Paid";
            //}
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(null);
            objs.Add(null);
            objs.Add(type);
            objs.Add(null);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).Where(m => m.Code == text).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUnusualAllowanceCfgBonusEvaluation(string text, string type)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(null);
            objs.Add(null);
            objs.Add(type);
            objs.Add(null);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_UnusualAllowanceCfgMuitlModel>(objs, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).Where(m => m.Code == text).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiUnusualCfgChildCare(string text)
        {
            return GetDataForControl<Cat_UnusualAllowanceCfgMuitlModel, Cat_UnusualAllowanceCfgMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UnuCfgbyChild);
        }
        #endregion

        #region Cat_Budget
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_Budget
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBudgetList([DataSourceRequest] DataSourceRequest request, Cat_BudgetSearchModel model)
        {
            return GetListDataAndReturn<Cat_BudgetModel, Cat_BudgetEntity, Cat_BudgetSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Budget);
        }

        /// [Phuoc.Le] - Xuất danh sách dữ liệu choTrợ Cấp (Cat_Budget) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllCatBudgetList([DataSourceRequest] DataSourceRequest request, Cat_BudgetSearchModel model)
        {
            return ExportAllAndReturn<Cat_BudgetEntity, Cat_BudgetModel, Cat_BudgetSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Budget);
        }

        /// [Phuoc.Le] - Xuất các dòng dữ liệu được chọn của  Trợ Cấp (Cat_Budget) theo điều kiện tìm kiếm

        public ActionResult ExportCatBudgetSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_BudgetEntity, Cat_BudgetModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_BudgetByIds);
        }

        public JsonResult GetMultiBudget(string text)
        {
            return GetDataForControl<Cat_BudgetMultiModel, Cat_BudgetMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Budget_Multi);
        }



        #endregion

        #region Cat_YouthUnionPosition
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_YouthUnionPosition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetYouthUnionPositionList([DataSourceRequest] DataSourceRequest request, Cat_YouthUnionPositionSearchModel model)
        {
            return GetListDataAndReturn<Cat_YouthUnionPositionModel, Cat_YouthUnionPositionEntity, Cat_YouthUnionPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_YouthUnionPosition);
        }

        public JsonResult GetMultiYouthUnionPosition(string text)
        {
            return GetDataForControl<Cat_YouthUnionPositionModel, Cat_YouthUnionPositionEntity>(text, ConstantSql.hrm_cat_sp_get_YouthUnionPosition_Multi);
        }

        public ActionResult ExportYouthUnionPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_YouthUnionPositionEntity, Cat_YouthUnionPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_YouthUnionPositionByIds);
        }

        // [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllYouthUnionPositionList([DataSourceRequest] DataSourceRequest request, Cat_YouthUnionPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_YouthUnionPositionEntity, Cat_YouthUnionPositionModel, Cat_YouthUnionPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_YouthUnionPosition);
        }
        #endregion

        #region Cat_CommunistPartyPosition
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_CommunistPartyPosition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCommunistPartyPositionList([DataSourceRequest] DataSourceRequest request, Cat_CommunistPartyPositionSearchModel model)
        {
            return GetListDataAndReturn<Cat_CommunistPartyPositionModel, Cat_CommunistPartyPositionEntity, Cat_CommunistPartyPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CommunistPartyPosition);
        }

        public ActionResult ExportCatCommunistPartyPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_CommunistPartyPositionEntity, Cat_CommunistPartyPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_CommunistPartyPositionByIds);
        }

        public ActionResult ExportAllCommunistPartyPositionList([DataSourceRequest] DataSourceRequest request, Cat_CommunistPartyPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_CommunistPartyPositionEntity, Cat_CommunistPartyPositionModel, Cat_CommunistPartyPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_CommunistPartyPosition);
        }
        //public JsonResult GetMultiCommunistPartyPosition(string text)
        //{
        //    return GetDataForControl<Cat_CommunistPartyPositionModel, Cat_CommunistPartyPositionEntity>(text, ConstantSql.hrm_cat_sp_get_CommunistPartyPosition_Multi);
        //}
        /// <summary>
        /// [Tho.Bui]: Get mutiselect CommunistPartyPosition
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonResult GetMultiCommunistPartyPosition(string text)
        {
            return GetDataForControl<Cat_CommunistPartyPositionModel, Cat_CommunistPartyPositionEntity>(text, ConstantSql.hrm_cat_sp_get_CommunistPartyPosition_multi);
        }
        #endregion

        #region Cat_WoundedSoldierTypes
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_WoundedSoldierTypes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWoundedSoldierTypesList([DataSourceRequest] DataSourceRequest request, Cat_WoundedSoldierTypesSearchModel model)
        {
            return GetListDataAndReturn<Cat_WoundedSoldierTypesModel, Cat_WoundedSoldierTypesEntity, Cat_WoundedSoldierTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_WoundedSoldierTypes);
        }

        public JsonResult GetMultiWoundedSoldierTypes(string text)
        {
            return GetDataForControl<Cat_WoundedSoldierTypesModel, Cat_WoundedSoldierTypesEntity>(text, ConstantSql.hrm_cat_sp_get_WoundedSoldierTypes_Multi);
        }

        public ActionResult ExportCatWoundedSoldierTypesSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_WoundedSoldierTypesEntity, Cat_WoundedSoldierTypesModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_WoundedSoldierTypesByIds);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllWoundedSoldierTypesList([DataSourceRequest] DataSourceRequest request, Cat_WoundedSoldierTypesSearchModel model)
        {
            return ExportAllAndReturn<Cat_WoundedSoldierTypesEntity, Cat_WoundedSoldierTypesModel, Cat_WoundedSoldierTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_WoundedSoldierTypes);
        }
        #endregion

        #region Cat_PoliticalLevel
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_PoliticalLevel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPoliticalLevelList([DataSourceRequest] DataSourceRequest request, Cat_PoliticalLevelSearchModel model)
        {
            return GetListDataAndReturn<Cat_PoliticalLevelModel, Cat_PoliticalLevelEntity, Cat_PoliticalLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PoliticalLevel);
        }

        public JsonResult GetMultiPoliticalLevel(string text)
        {
            return GetDataForControl<Cat_PoliticalLevelModel, Cat_PoliticalLevelEntity>(text, ConstantSql.hrm_cat_sp_get_PoliticalLevel_Multi);
        }

        public ActionResult ExportCatPoliticalLevelSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_PoliticalLevelEntity, Cat_PoliticalLevelModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_PoliticalLevelByIds);
        }

        public ActionResult ExportAllPoliticalLevelList([DataSourceRequest] DataSourceRequest request, Cat_PoliticalLevelSearchModel model)
        {
            return ExportAllAndReturn<Cat_PoliticalLevelEntity, Cat_PoliticalLevelModel, Cat_PoliticalLevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_PoliticalLevel);
        }

        #endregion
        #region Cat_ArmedForceTypes
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_ArmedForceTypes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetArmedForceTypesList([DataSourceRequest] DataSourceRequest request, Cat_ArmedForceTypesSearchModel model)
        {
            return GetListDataAndReturn<Cat_ArmedForceTypesModel, Cat_ArmedForceTypesEntity, Cat_ArmedForceTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ArmedForceTypes);
        }

        public JsonResult GetMultiArmedForceTypes(string text)
        {
            return GetDataForControl<Cat_ArmedForceTypesModel, Cat_ArmedForceTypesEntity>(text, ConstantSql.hrm_cat_sp_get_ArmedForceTypes_Multi);
        }

        public ActionResult ExportCatArmedForceTypesSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ArmedForceTypesEntity, Cat_ArmedForceTypesModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ArmedForceTypesByIds);
        }

        public ActionResult ExportAllArmedForceTypesList([DataSourceRequest] DataSourceRequest request, Cat_ArmedForceTypesSearchModel model)
        {
            return ExportAllAndReturn<Cat_ArmedForceTypesEntity, Cat_ArmedForceTypesModel, Cat_ArmedForceTypesSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ArmedForceTypes);
        }
        #endregion
        #region Cat_TradeUnionistPosition
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_TradeUnionistPosition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTradeUnionistPositionList([DataSourceRequest] DataSourceRequest request, Cat_TradeUnionistPositionSearchModel model)
        {
            return GetListDataAndReturn<Cat_TradeUnionistPositionModel, Cat_TradeUnionistPositionEntity, Cat_TradeUnionistPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TradeUnionistPosition);
        }

        public JsonResult GetMultiTradeUnionistPosition(string text)
        {
            return GetDataForControl<Cat_TradeUnionistPositionModel, Cat_TradeUnionistPositionEntity>(text, ConstantSql.hrm_cat_sp_get_TradeUnionistPosition_Multi);
        }

        public ActionResult ExportCatTradeUnionistPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_TradeUnionistPositionEntity, Cat_TradeUnionistPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_TradeUnionistPositionByIds);
        }

        public ActionResult ExportAllTradeUnionistPositionList([DataSourceRequest] DataSourceRequest request, Cat_TradeUnionistPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_TradeUnionistPositionEntity, Cat_TradeUnionistPositionModel, Cat_TradeUnionistPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_TradeUnionistPosition);
        }

        #endregion
        #region Cat_SelfDefenceMilitiaPosition
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_SelfDefenceMilitiaPosition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSelfDefenceMilitiaPositionList([DataSourceRequest] DataSourceRequest request, Cat_SelfDefenceMilitiaPositionSearchModel model)
        {
            return GetListDataAndReturn<Cat_SelfDefenceMilitiaPositionModel, Cat_SelfDefenceMilitiaPositionEntity, Cat_SelfDefenceMilitiaPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SelfDefenceMilitiaPosition);
        }

        public JsonResult GetMultiSelfDefenceMilitiaPosition(string text)
        {
            return GetDataForControl<Cat_SelfDefenceMilitiaPositionModel, Cat_SelfDefenceMilitiaPositionEntity>(text, ConstantSql.hrm_cat_sp_get_SelfDefenceMilitiaPosition_Multi);
        }

        public ActionResult ExportCatSelfDefenceMilitiaPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SelfDefenceMilitiaPositionEntity, Cat_SelfDefenceMilitiaPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SelfDefenceMilitiaPositionByIds);
        }

        public ActionResult ExportAllSelfDefenceMilitiaPositionList([DataSourceRequest] DataSourceRequest request, Cat_SelfDefenceMilitiaPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_SelfDefenceMilitiaPositionEntity, Cat_SelfDefenceMilitiaPositionModel, Cat_SelfDefenceMilitiaPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SelfDefenceMilitiaPosition);
        }

        #endregion
        #region Cat_VeteranUnionPosition
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_VeteranUnionPosition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVeteranUnionPositionList([DataSourceRequest] DataSourceRequest request, Cat_VeteranUnionPositionSearchModel model)
        {
            return GetListDataAndReturn<Cat_VeteranUnionPositionModel, Cat_VeteranUnionPositionEntity, Cat_VeteranUnionPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_VeteranUnionPosition);
        }

        public JsonResult GetMultiVeteranUnionPosition(string text)
        {
            return GetDataForControl<Cat_VeteranUnionPositionModel, Cat_VeteranUnionPositionEntity>(text, ConstantSql.hrm_cat_sp_get_VeteranUnionPosition_Multi);
        }

        public ActionResult ExportCatVeteranUnionPositionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_VeteranUnionPositionEntity, Cat_VeteranUnionPositionModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_VeteranUnionPositionByIds);
        }

        public ActionResult ExportAllVeteranUnionPositionList([DataSourceRequest] DataSourceRequest request, Cat_VeteranUnionPositionSearchModel model)
        {
            return ExportAllAndReturn<Cat_VeteranUnionPositionEntity, Cat_VeteranUnionPositionModel, Cat_VeteranUnionPositionSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_VeteranUnionPosition);
        }

        #endregion
        #region [Tho.Bui]:Cat_SalaryRank
        /// <summary>
        /// [Tho.Bui] - Lấy danh sách dữ liệu bảng Cat_SalaryRank (Cat_SalaryRank)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSalaryRankList([DataSourceRequest] DataSourceRequest request, Cat_SalaryRankSearchModel model)
        {
            return GetListDataAndReturn<Cat_SalaryRankModel, Cat_SalaryRankEntity, Cat_SalaryRankSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryRank);
        }
        /// [Tho.Bui] - Xuất các dòng dữ liệu được chọn của mã lương (Cat_SalaryClass) theo điều kiện tìm kiếm
        public ActionResult ExportSalaryRankSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SalaryRankEntity, Cat_SalaryRankModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SalaryRankByIds);
        }
        /// [Tho.Bui] - Xuất danh sách dữ liệu cho mã lương (Cat_SalaryRank) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAlSalaryRankList([DataSourceRequest] DataSourceRequest request, Cat_SalaryRankSearchModel model)
        {
            return ExportAllAndReturn<Cat_SalaryRankEntity, Cat_SalaryRankModel, Cat_SalaryRankSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_SalaryRank);
        }

        public JsonResult GetMultiSalaryRank(string text, string SalaryClassID)
        {
            string status = string.Empty;
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(Common.DotNetToOracle(SalaryClassID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_SalaryRankMultiEntity>(objs, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).OrderBy(s => s.SalaryRankName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiSalaryRankAndRankDetail(string text)
        {
            return GetDataForControl<Cat_SalaryRankMultiEntity, Cat_SalaryRankMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryRank_Multi);
        }
        public JsonResult GetMultiSalaryRankClassList(string text)
        {
            return GetDataForControl<Cat_SalaryClassMultiEntity, Cat_SalaryClassMultiEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryRankClass_Multi);
        }

        [HttpPost]
        public ActionResult GetDataOfSalaryRank(Guid ID)
        {
            ActionService service = new ActionService(UserLogin);
            string status = "";
            var entity = service.GetByIdUseStore<Cat_SalaryRankEntity>(ID, ConstantSql.hrm_cat_sp_get_SalaryRankById, ref status);
            var rs = new { SalaryClassName = entity.SalaryClassCode, BasicSalary = entity.SalaryStandard, AbilityTitle = entity.AbilityTitleVNI, SalaryClassID = entity.SalaryClassID };
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSalaryRankBySalaryClassID(Guid SalaryClassID)
        {
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            var result = service.GetData<Cat_SalaryRankEntity>(Common.DotNetToOracle(SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_SalaryRankBySalaryClassId, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Cat_Budget
        /// <summary>
        /// [Kiet.Chung] - Lấy danh sách dữ liệu bảng Cat_NameEntity
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNameEntityList([DataSourceRequest] DataSourceRequest request, Cat_NameEntitySearchModel model)
        {
            return GetListDataAndReturn<Cat_NameEntityModel, Cat_NameEntityEntity, Cat_NameEntitySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_NameEntity);
        }
        [HttpPost]
        public ActionResult GetNameEntityByKPI([DataSourceRequest] DataSourceRequest request, Cat_NameEntityByKPISearchModel model)
        {
            return GetListDataAndReturn<Cat_NameEntityModel, Cat_NameEntityEntity, Cat_NameEntityByKPISearchModel>(request, model, ConstantSql.hrm_cat_sp_get_NameEntityByKPI);
        }
        public ActionResult ExportAllEntityByKPI([DataSourceRequest] DataSourceRequest request, Cat_NameEntityByKPISearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, Cat_NameEntityModel, Cat_NameEntityByKPISearchModel>(request, model, ConstantSql.hrm_cat_sp_get_NameEntityByKPI);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllNameEntityList([DataSourceRequest] DataSourceRequest request, Cat_NameEntitySearchModel model)
        {
            return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_NameEntitySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_NameEntity);
        }

        public ActionResult ExportNameEntitySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LevelGeneralByIds);
        }

        #endregion

        #region Cat_Element

        //public JsonResult GetMultiGradePayroll()
        //{
        //    return GetDataForControl<Cat_GradePayrollMultiModel, Cat_GradePayrollMultiEntity>("", ConstantSql.hrm_cat_sp_get_GradePayroll_multi);
        //}

        public JsonResult GetMultiGradePayroll(string text)
        {
            string status = string.Empty;
            BaseService baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(text);
            objs.Add(text);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Cat_GradePayrollEntity>(objs, ConstantSql.hrm_cat_sp_get_GradePayroll, UserLogin, ref status).OrderBy(s => s.GradeCfgName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiCatElement(string text)
        {
            return GetDataForControl<CatElementModel, Cat_ElementEntity>(text, ConstantSql.hrm_cat_sp_get_CatElement_multi);
        }

        /// <summary>
        /// [Hien.Nguyen] Kiểm tra xem công thức có hợp lệ hay không
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckFormula(string elementCode, string values)
        {
            if (values.Replace("\n", "").Replace(" ", "") == string.Empty)
            {
                return Json(new { success = false, data = 0 });
            }
            values = values.Replace("\n", "");
            string status = string.Empty;
            var baseService = new BaseService();
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<CatElementModel> listCat_Element = baseService.GetData<CatElementModel>(listModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status);

            #region Kiểm tra xem công thức có giống tên phần tử hay không
            Sal_ComputePayrollServices PayrollServices = new Sal_ComputePayrollServices();
            //Các phần tử tính lương tách ra từ 1 chuỗi công thức
            List<string> ListFormula = PayrollServices.ParseFormulaToList(values).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();
            if (ListFormula.Any(m => m.Replace("[", "").Replace("]", "") == elementCode))
            {
                return Json(new { success = false, mess = "Công Thức Sai ! Công thức không được chứa phần tử [" + elementCode + "]" });
            }

            #endregion

            #region Add thêm các phần tử Enum để kiểm tra
            var valuesAsList = Enum.GetValues(typeof(PayrollElement)).Cast<PayrollElement>().ToList();
            foreach (var i in valuesAsList)
            {
                listCat_Element.Add(new CatElementModel() { ElementCode = i.ToString(), Formula = "[" + i.ToString() + "]" });
            }
            #endregion

            #region Add thêm các phần tử là phụ cấp
            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            var listUsualAllowance = baseService.GetData<Cat_UsualAllowanceModel>(listModel, ConstantSql.hrm_cat_sp_get_UsualAllowance, UserLogin, ref status);

            if (listUsualAllowance != null)
            {
                foreach (var i in listUsualAllowance)
                {
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UsualAllowanceName, ElementCode = i.Code, Formula = i.Formula });
                }
            }
            #endregion

            #region Add thêm các phần tử là phụ cấp bất thường
            List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg = new List<Cat_UnusualAllowanceCfgEntity>();
            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            listUnusualAllowanceCfg = baseService.GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).ToList();
            if (listUnusualAllowanceCfg != null)
            {
                foreach (var i in listUnusualAllowanceCfg)
                {
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code, Formula = i.Formula });

                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_6", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_5", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_4", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_3", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_2", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_N_1", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_AMOUNTOFOFFSET_N_1", Formula = i.Formula });

                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_T3", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_TIMELINE", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_TIMELINE_N_1", Formula = i.Formula });

                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_DAYCLOSE_N_1", Formula = i.Formula });
                    listCat_Element.Add(new CatElementModel() { ElementName = i.UnusualAllowanceCfgName, ElementCode = i.Code + "_DAYCLOSE", Formula = i.Formula });
                }
            }
            #endregion

            #region Kiểm tra phần tử (kiểu mới)
            string formula = values.Replace("/t", "").Replace("/n", "").Trim();
            foreach (var i in listCat_Element)
            {
                string _tmp = "[" + i.ElementCode + "]";
                if (formula.IndexOf(_tmp) != -1)
                {
                    formula = formula.Replace(_tmp, " 1 ");
                }
            }

            //kiểm tra trường hợp có sử dụng phần tử động
            if (formula.IndexOf(PayrollElement.DYN_COUNTDAYOVERTIMEBYTYPE_.ToString()) != -1)
            {
                return Json(new { success = true, mess = "Công Thức Đúng !" });
            }

            try
            {
                HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel value = FormulaHelper.ParseFormula(formula, new List<ElementFormula>());
                if (value.ErrorMessage == string.Empty)
                {
                    return Json(new { success = true, mess = "Công Thức Đúng !" });
                }
                else
                {
                    return Json(new { success = false, mess = "Công Thức Sai !" });
                }

            }
            catch
            {
                return Json(new { success = false, mess = "Công Thức Sai !" });
            }
            #endregion
        }
        /// <summary>
        ///  [Hien.Nguyen] Kiểm tra xem công thức có hợp lệ hay không
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckFormulaUsualAllowance(string values)
        {
            if (values.Replace("\n", "").Replace(" ", "") == string.Empty)
            {
                return Json(new { success = false, data = 0 });
            }
            values = values.Replace("\n", "");
            string status = string.Empty;
            List<CatElementModel> listCat_Element = new List<CatElementModel>();

            #region Add thêm các phần tử Enum để kiểm tra
            var valuesAsList = Enum.GetValues(typeof(PayrollElement)).Cast<PayrollElement>().ToList();
            foreach (var i in valuesAsList)
            {
                listCat_Element.Add(new CatElementModel() { ElementCode = i.ToString(), Formula = "[" + i.ToString() + "]" });
            }
            #endregion

            #region Kiểm tra phần tử và các phép toán
            string formula = values.Replace(" ", "").Replace("+", " ").Replace("-", " ").Replace("*", " ").Replace("/", " ");
            if (formula.Contains("  "))//trường hợp 2 phương thức đứng chung (++,--)
            {
                string errorValue = values[formula.IndexOf("  ")].ToString();
                return Json(new { success = false, data = errorValue });
            }
            List<string> listFormula = formula.Split(' ').ToList();

            #region Kiểm tra dấu đóng mở phải là từng cặp
            int Open = values.Count(m => m == '(');
            int Close = values.Count(m => m == ')');
            if (Open != Close)
            {
                return Json(new { success = false, data = "(" });
            }
            #endregion
            for (int i = 0; i < listFormula.Count; i++)
            {
                if (listFormula[i].Contains("("))
                {
                    if (!listFormula[i].StartsWith("("))
                    {
                        return Json(new { success = false, data = listFormula[i] });
                    }
                }
                if (listFormula[i].Contains(")"))
                {
                    if (!listFormula[i].EndsWith(")"))
                    {
                        return Json(new { success = false, data = listFormula[i] });
                    }
                }

                listFormula[i] = listFormula[i].Replace("(", "").Replace(")", "");

                if (listFormula[i] == "")//Loại bỏ trường hợp phần tử rỗng
                {
                    return Json(new { success = false, data = i });
                }
                if (listFormula[i].StartsWith("[") && listFormula[i].EndsWith("]"))//Là phần tử tính lương
                {
                    if (!listCat_Element.Any(m => m.ElementCode == listFormula[i].Replace("[", "").Replace("]", "")))
                    {
                        return Json(new { success = false, data = listFormula[i] });
                    }
                }
                else//Là số
                {
                    if (!Common.IsNumber(listFormula[i]))
                    {
                        return Json(new { success = false, data = listFormula[i] });
                    }
                }
            }
            #endregion
            return Json(new { success = true });
        }

        #endregion

        #region Cat_RateInsurance

        [HttpPost]
        public ActionResult GetRateInsuranceList([DataSourceRequest] DataSourceRequest request, Cat_RateInsuranceSearchModel model)
        {
            return GetListDataAndReturn<Cat_RateInsuranceModel, Cat_RateInsuranceEntity, Cat_RateInsuranceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_RateInsurance);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Tỷ Lệ Bảo Hiểm (Cat_RateInsurance) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllRateInsuranceList([DataSourceRequest] DataSourceRequest request, Cat_RateInsuranceSearchModel model)
        {
            return ExportAllAndReturn<Cat_RateInsuranceEntity, Cat_RateInsuranceModel, Cat_RateInsuranceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_RateInsurance);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn củaTỷ Lệ Bảo Hiểm (Cat_RateInsurance) theo điều kiện tìm kiếm
        public ActionResult ExportRateInsuranceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RateInsuranceEntity, Cat_RateInsuranceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_RateInsuranceByIds);
        }
        #endregion

        #region Cat_ValueEntity

        [HttpPost]
        public ActionResult GetValueEntityList([DataSourceRequest] DataSourceRequest request, Cat_ValueEntitySearchModel model)
        {
            return GetListDataAndReturn<Cat_ValueEntityModel, Cat_ValueEntityEntity, Cat_ValueEntitySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ValueEntity);
        }

        /// [Quoc.Do] - Xuất danh sách dữ liệu cho Tỷ Lệ Bảo Hiểm (Cat_RateInsurance) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllValueEntityList([DataSourceRequest] DataSourceRequest request, Cat_ValueEntitySearchModel model)
        {
            return ExportAllAndReturn<Cat_ValueEntityEntity, Cat_ValueEntityModel, Cat_ValueEntitySearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ValueEntity);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn củaTỷ Lệ Bảo Hiểm (Cat_RateInsurance) theo điều kiện tìm kiếm
        public ActionResult ExportValueEntitySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ValueEntityEntity, Cat_ValueEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_ValueEntityByIds);
        }
        #endregion

        #region Cat_Shop
        [HttpPost]
        public ActionResult GetShopList([DataSourceRequest] DataSourceRequest request, Cat_ShopSearchModel model)
        {
            return GetListDataAndReturn<Cat_ShopModel, Cat_ShopEntity, Cat_ShopSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Shop);
        }
        [HttpPost]
        public JsonResult GetMultiShop(string text)
        {
            return GetDataForControl<Cat_ShopMultiModel, Cat_ShopMultiEntity>(text, ConstantSql.hrm_Cat_sp_get_Shop_multi);
        }

        [HttpPost]
        public JsonResult GetTreeViewShop(Guid? ID)
        {
            List<object> listModel = new List<object>();
            var service = new BaseService();
            string status = string.Empty;

            if (ID == null || ID == Guid.Empty)//Load Group Shop
            {
                List<Cat_ShopGroupEntity> listGroup = new List<Cat_ShopGroupEntity>();
                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                listGroup = service.GetData<Cat_ShopGroupEntity>(listModel, ConstantSql.hrm_cat_sp_get_ShopGroup, UserLogin, ref status).ToList();
                var result = from e in listGroup
                             select new
                             {
                                 id = e.ID,
                                 Name = e.ShopGroupName,
                                 hasChildren = e.CountShop > 0 ? true : false,
                                 Group = 0,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Cat_ShopEntity> listShop = new List<Cat_ShopEntity>();
                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[0] = ID;
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                listShop = service.GetData<Cat_ShopEntity>(listModel, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status).ToList();
                var result = from e in listShop
                             select new
                             {
                                 id = e.ID,
                                 Name = e.ShopName,
                                 hasChildren = false,
                                 Group = 1,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportAllCat_ShopList([DataSourceRequest] DataSourceRequest request, Cat_ShopSearchModel model)
        {
            return ExportAllAndReturn<Cat_ShopModel, Cat_ShopEntity, Cat_ShopSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Shop);
        }
        public ActionResult ExportCat_ShopSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_ShopEntity, Cat_ShopModel>(selectedIds, valueFields, ConstantSql.hrm_Cat_sp_get_ShopIds);
        }

        public JsonResult GetShopByOrgID(Guid orgID, string ShopFilter)
        {
            var result = new List<Cat_ShopModel>();
            string status = string.Empty;
            if (orgID != Guid.Empty)
            {
                var service = new Cat_ShopServices();
                result = service.GetData<Cat_ShopModel>(orgID, ConstantSql.hrm_Cat_sp_get_ShopbyOrgID, UserLogin, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHDTJobTypeByHDTJobGroupID(Guid HDTJobGroup, string HDTJobTypeFilter)
        {
            var result = new List<Cat_HDTJobTypeModel>();
            string status = string.Empty;
            if (HDTJobGroup != Guid.Empty)
            {
                var service = new Cat_HDTJobTypeServices();
                result = service.GetData<Cat_HDTJobTypeModel>(HDTJobGroup, ConstantSql.hrm_Cat_sp_get_HDTJobTypeByGroupID, UserLogin, ref status);
                if (!string.IsNullOrEmpty(HDTJobTypeFilter))
                {
                    var rs = result.Where(s => s.HDTJobTypeName != null && s.HDTJobTypeName.ToLower().Contains(HDTJobTypeFilter.ToLower())).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHDTJobTypeCodeByHDTJobGroupID(Guid HDTJobGroupID, string filterHDTJob)
        {
            var result = new List<Cat_HDTJobTypeCodeMultiModel>();
            string status = string.Empty;
            List<object> obj = new List<object>();
            obj.Add(HDTJobGroupID);
            obj.Add(filterHDTJob);
            //if (HDTJobGroupID != Guid.Empty)
            //{
            var service = new Cat_HDTJobTypeServices();
            result = service.GetData<Cat_HDTJobTypeCodeMultiModel>(obj, ConstantSql.hrm_Cat_sp_get_HDTJobTypeCodeByGroupID, UserLogin, ref status);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetTypeHDTJobGroupID(Guid ID)
        {
            string status = string.Empty;
            if (ID != Guid.Empty)
            {
                var service = new Cat_HDTJobGroupServices();
                var data = service.GetData<Cat_HDTJobGroupModel>(Common.DotNetToOracle(ID.ToString()), ConstantSql.hrm_cat_sp_get_HDTJobGroupById, UserLogin, ref status).ToList();
                if (data == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                var result = from e in data
                             select new
                             {
                                 Text = e.Type.TranslateString(),
                                 Value = e.Type,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypeHDTJobTypeID(Guid ID)
        {
            string status = string.Empty;
            if (ID != Guid.Empty)
            {
                var service = new Cat_HDTJobTypeServices();
                var data = service.GetData<Cat_HDTJobTypeModel>(Common.DotNetToOracle(ID.ToString()), ConstantSql.hrm_cat_sp_get_HDTJobTypeById, UserLogin, ref status).ToList();
                if (data == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                var result = from e in data
                             select new
                             {
                                 GroupName = e.HDTJobGroupName,
                                 GroupID = e.HDTJobGroupID,
                                 Text = e.Type.TranslateString(),
                                 Value = e.Type,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ApproveHDTJobTypePrice(List<Guid> selectedIds)
        {
            var service = new Cat_HDTJobTypePriceServices();
            var message = service.ActionApproved(selectedIds);
            return Json(message);
        }


        #endregion

        #region Cat_Role
        [HttpPost]
        public ActionResult GetRoleList([DataSourceRequest] DataSourceRequest request, Cat_RoleSearchModel model)
        {
            return GetListDataAndReturn<Cat_RoleModel, Cat_RoleEntity, Cat_RoleSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Role);
        }



        public ActionResult ExportRoleSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_RoleEntity, Cat_RoleModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_RoleByIds);
        }

        public JsonResult GetMultiRole(string text)
        {
            return GetDataForControl<Cat_RoleMultiModel, Cat_RoleMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Role_multi);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllRoleList([DataSourceRequest] DataSourceRequest request, Cat_RoleSearchModel model)
        {
            return ExportAllAndReturn<Cat_RoleEntity, Cat_RoleModel, Cat_RoleSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Role);
        }
        #endregion

        #region Cat_JobType
        [HttpPost]
        public ActionResult GetJobTypeList([DataSourceRequest] DataSourceRequest request, Cat_JobTypeSearchModel model)
        {
            return GetListDataAndReturn<Cat_JobTypeModel, Cat_JobTypeEntity, Cat_JobTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_JobType);
        }
        //public ActionResult ExportAllReasonDenylList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        //{
        //    return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ResonDeny);
        //}

        public ActionResult ExportJobTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_JobTypeEntity, Cat_JobTypeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_JobTypeByIds);
        }

        public JsonResult GetMultiJobType(string text)
        {
            return GetDataForControl<Cat_JobTypeMultiModel, Cat_JobTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_JobType_multi);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllJobTypeList([DataSourceRequest] DataSourceRequest request, Cat_JobTypeSearchModel model)
        {
            return ExportAllAndReturn<Cat_JobTypeEntity, Cat_JobTypeModel, Cat_JobTypeSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_JobType);
        }
        #endregion

        #region Cat_UnitPrice
        [HttpPost]
        public ActionResult GetUnitPriceList([DataSourceRequest] DataSourceRequest request, Cat_UnitPriceSearchModel model)
        {
            return GetListDataAndReturn<Cat_UnitPriceModel, Cat_UnitPriceEntity, Cat_UnitPriceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UnitPrice);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllUnitPriceList([DataSourceRequest] DataSourceRequest request, Cat_UnitPriceSearchModel model)
        {
            return ExportAllAndReturn<Cat_UnitPriceEntity, Cat_UnitPriceEntity, Cat_UnitPriceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_UnitPrice);
        }

        //public ActionResult ExportAllReasonDenylList([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        //{
        //    return ExportAllAndReturn<Cat_NameEntityEntity, CatNameEntityModel, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_ResonDeny);
        //}

        public ActionResult ExportUnitPriceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_UnitPriceEntity, Cat_UnitPriceEntity>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_UnitPriceByIds);
        }

        public JsonResult GetJobTypeNameByRoleID(Guid roleid)
        {
            var result = new List<Cat_JobTypeMultiModel>();
            string status = string.Empty;
            if (roleid != Guid.Empty)
            {
                var service = new Cat_UnitPriceServices();
                result = service.GetData<Cat_JobTypeMultiModel>(roleid, ConstantSql.hrm_cat_sp_get_JobTypeByRoleId, UserLogin, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cat_HDTJobTypePrice

        [HttpPost]
        public ActionResult GetHDTJobTypePriceList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobTypePriceSearchModel model)
        {
            return GetListDataAndReturn<Cat_HDTJobTypePriceModel, Cat_HDTJobTypePriceEntity, Cat_HDTJobTypePriceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobTypePrice);
        }

        public ActionResult ExportAllHDTJobTypePriceList([DataSourceRequest] DataSourceRequest request, Cat_HDTJobTypePriceSearchModel model)
        {
            return ExportAllAndReturn<Cat_HDTJobTypePriceEntity, Cat_HDTJobTypePriceModel, Cat_HDTJobTypePriceSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_HDTJobTypePrice);
        }

        public ActionResult ExportHDTJobTypePriceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_HDTJobTypePriceEntity, Cat_HDTJobTypePriceModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_HDTJobTypePriceByIds);
        }
        #endregion

        #region Load chế độ công và chế độ lượng theo Rank( code của Cat_SalaryClass)
        [HttpPost]
        public ActionResult GetGradePayrollAndGradeAttendanceByRank(string Rank)
        {
            var serviceGradePayroll = new Cat_GradePayrollServices();
            var serviceGradeAttendance = new Cat_GradeAttendanceServices();
            Guid GradePayrollID = serviceGradePayroll.GetGradePayrollByRank(Rank);
            Guid GradeAttendanceID = serviceGradeAttendance.GetGradeAttendanceByRank(Rank);
            var data = new { GradePayrollID = GradePayrollID, GradeAttendanceID = GradeAttendanceID };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Cat_Topic
        [HttpPost]
        public ActionResult GetTopicList([DataSourceRequest] DataSourceRequest request, Cat_TopicSearchModel model)
        {
            return GetListDataAndReturn<Cat_TopicModel, Cat_TopicEntity, Cat_TopicSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Topic);

        }

        [HttpPost]
        public ActionResult ExportTopicList([DataSourceRequest] DataSourceRequest request, Cat_TopicSearchModel model)
        {
            return ExportAllAndReturn<Cat_TopicEntity, Cat_TopicModel, Cat_TopicSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Topic);
        }

        public JsonResult GetMultiTopic(string text)
        {
            return GetDataForControl<Cat_TopicMultiModel, Cat_TopicMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Topic_Multi);
        }
        #endregion



        public ActionResult ExportBankSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_BankEntity, CatBankModel>(selectedIds, valueFields, ConstantSql.hrm_Cat_sp_get_BankByIds);
        }

        [HttpPost]
        public ActionResult GetDateComeBackSearch()
        {
            string status = string.Empty;
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
            var valueDayComeBackExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYCOMEBACKEXPIRY);
            if (valueDayComeBackExpiry >= 0)
            {
                DateFrom = DateTo.AddDays(-valueDayComeBackExpiry);
            }
            return Json(DateFrom, JsonRequestBehavior.AllowGet);
        }

        #region cat_MasterDataGroup
        public ActionResult GetCat_MasterDataGroupList([DataSourceRequest] DataSourceRequest request, Cat_MasterDataGroupSearchModel model)
        {
            return GetListDataAndReturn<Cat_MasterDataGroupModel, Cat_MasterDataGroupEntity, Cat_MasterDataGroupSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_MasterDataGroup);
        }

        [HttpPost]
        public ActionResult GetMasterGroupItemByMasterGroupIDList([DataSourceRequest] DataSourceRequest request, Guid? masterDataGroupID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(masterDataGroupID);
            var result = baseService.GetData<Cat_MasterDataGroupItemEntity>(objs, ConstantSql.hrm_cat_sp_get_MasterDataGroupItemByMasterDataGroupID, UserLogin, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request));
            }

            return null;
        }
        #region Pur_MCAM
        //public JsonResult GetListPurMCAM(Pur_MCAMModel model)
        //{
        //    var service = new BaseService();
        //    string status = string.Empty;
        //    List<object> lstobj = new List<object>();
        //    lstobj.AddRange(new object[8]);
        //    lstobj[0]=model.OrgStructureID;
        //    lstobj[1]=model.WorkPlaceID;
        //    lstobj[2]=model.CodeEmp;
        //    lstobj[3]=model.ProfileName;
        //    lstobj[4]=model.ModelType;
        //    lstobj[5]=model.ModelName;
        //    lstobj[6]=1;
        //    lstobj[7]=Int32.MaxValue - 1;
        //    var result = service.GetData<Pur_MCAMModel>(lstobj, ConstantSql.Hrm_CAT_SP_GET_PURMCAM, ref status).ToList();
        //    return Json(result.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetListPurMCAM([DataSourceRequest] DataSourceRequest request, Pur_MCAMSearchModel model)
        {
            return GetListDataAndReturn<Pur_MCAMModel, Pur_MCAMEntity, Pur_MCAMSearchModel>(request, model, ConstantSql.Hrm_CAT_SP_GET_PURMCAM);
        }

        #endregion
        #region Cat_MasterDataGroup
        #region Cat_SysLockObject
        public ActionResult ExportSysLockObjectAll([DataSourceRequest] DataSourceRequest request, Cat_LevelSearchModel model)
        {
            return ExportAllAndReturn<CatNameEntityModel, Cat_NameEntityEntity, Cat_LevelSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_LockObject);
        }
        public ActionResult ExportSysLockObjectbyIDs(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_NameEntityEntity, CatNameEntityModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_LockObjectByIDs);
        }
        #endregion
        public JsonResult GetMultiCatTable(string text)
        {
            var service = new BaseService();
            string status = "";
            List<object> tableParam = new List<object>();
            tableParam.AddRange(new object[3]);
            tableParam[0] = text;
            tableParam[1] = 1;
            tableParam[2] = Int32.MaxValue - 1;
            var result = service.GetData<Cat_SysTablesMultiEntity>(tableParam, ConstantSql.hrm_cat_sp_get_tables, UserLogin, ref status).ToList();
            result = result.Where(p => p.Name.Substring(0, 3) == HRM.Infrastructure.Utilities.ModuleKey.Cat.ToString()).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiCatTableForDashBoard(Guid userID)
        {
            var service = new Sys_UserServices();
            var lstMasterDataGroupMulti = service.GetMultiCatTableForDashBoard(userID);
            return Json(lstMasterDataGroupMulti, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChildObjectName(string objectName)
        {
            var services = new Cat_MasterDataGroupServices();
            var result = services.GetChildItems(objectName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region Cat_Skill
        public ActionResult GetSkillList([DataSourceRequest] DataSourceRequest request, Cat_SkillSearchModel model)
        {
            return GetListDataAndReturn<Cat_SkillModel, Cat_SkillEntity, Cat_SkillSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Skill);
        }
        public ActionResult ExportSkillSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Cat_SkillEntity, Cat_SkillModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_SkillByIds);
        }
        public ActionResult ExportAllSkillList([DataSourceRequest] DataSourceRequest request, Cat_SkillSearchModel model)
        {
            return ExportAllAndReturn<Cat_SkillEntity, Cat_SkillModel, Cat_SkillSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Skill);
        }
        public ActionResult GetSkillTopicBySkillID([DataSourceRequest] DataSourceRequest request, Guid skillID)
        {
            string status = string.Empty;

            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(skillID);
            objs.Add(request.Page);
            objs.Add(request.PageSize);
            var result = baseService.GetData<Cat_SkillTopicModel>(objs, ConstantSql.hrm_cat_sp_get_SkillTopicBySkillId, UserLogin, ref status);
            if (result != null)

                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }

        #endregion

        public ActionResult CreateAndUpdateOrgMoreInfo(Guid? orgID, string servicesType, DateTime? contractFrom, DateTime? contractTo, string billingCompanyName, string billingAddress, string taxCode, string des, string durationPay, string recipientInvoice, string tetePhone, string cellPhone, string email)
        {
            var status = string.Empty;
            var message = string.Empty;
            var baseService = new BaseService();
            var orgInfoServices = new Cat_OrgMoreInforServices();
            var orgModel = new Cat_OrgMoreInforModel();

            if (orgID != null)
            {
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(orgID.Value.ToString()));
                var orgInfoEntity = baseService.GetData<Cat_OrgMoreInforEntity>(objs, ConstantSql.hrm_hr_sp_get_OrgMoreInfoByOrgID, UserLogin, ref status).FirstOrDefault();
                if (orgInfoEntity != null)
                {
                    orgInfoEntity.ServicesType = servicesType;
                    orgInfoEntity.ContractFrom = contractFrom;
                    orgInfoEntity.ContractTo = contractTo;
                    orgInfoEntity.BillingCompanyName = billingCompanyName;
                    orgInfoEntity.BillingAddress = billingAddress;
                    orgInfoEntity.TaxCode = taxCode;
                    orgInfoEntity.Description = des;
                    orgInfoEntity.DurationPay = durationPay;
                    orgInfoEntity.RecipientInvoice = recipientInvoice;
                    orgInfoEntity.TelePhone = tetePhone;
                    orgInfoEntity.CellPhone = cellPhone;
                    orgInfoEntity.Email = email;
                    orgInfoEntity.OrgStructureID = orgID;
                    message = orgInfoServices.Edit(orgInfoEntity);
                    orgModel = orgInfoEntity.CopyData<Cat_OrgMoreInforModel>();
                    orgModel.ActionStatus = message;

                }
                else
                {
                    var orgInfoAddEntity = new Cat_OrgMoreInforEntity();
                    orgInfoAddEntity.ServicesType = servicesType;
                    orgInfoAddEntity.ContractFrom = contractFrom;
                    orgInfoAddEntity.ContractTo = contractTo;
                    orgInfoAddEntity.BillingCompanyName = billingCompanyName;
                    orgInfoAddEntity.BillingAddress = billingAddress;
                    orgInfoAddEntity.TaxCode = taxCode;
                    orgInfoAddEntity.Description = des;
                    orgInfoAddEntity.DurationPay = durationPay;
                    orgInfoAddEntity.RecipientInvoice = recipientInvoice;
                    orgInfoAddEntity.TelePhone = tetePhone;
                    orgInfoAddEntity.CellPhone = cellPhone;
                    orgInfoAddEntity.Email = email;
                    orgInfoAddEntity.OrgStructureID = orgID;
                    message = orgInfoServices.Add(orgInfoAddEntity);
                    orgModel = orgInfoAddEntity.CopyData<Cat_OrgMoreInforModel>();
                    orgModel.ActionStatus = message;

                }
            }

            return Json(orgModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovedTravelRequest(Guid travelRequestId, Guid userId, string type)
        {
            var status = string.Empty;
            var message = string.Empty;

            var services = new FIN_ClaimService();
            message = services.ApprovedTravelRequest(travelRequestId, userId, type);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovedCashAdvance(Guid cashAdvanceId, Guid userId, string type)
        {
            var status = string.Empty;
            var message = string.Empty;

            var services = new FIN_CashAdvanceService();
            message = services.ApprovedCashAdvance(cashAdvanceId, userId, type);

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovedClaim(Guid claimId, Guid userId, string type)
        {
            var status = string.Empty;
            var message = string.Empty;

            var services = new FIN_ClaimService();
            message = services.ApprovedClaim(claimId, userId, type);

            return Json(message, JsonRequestBehavior.AllowGet);
        }


        // lấy ds nguồn chi phí
        public ActionResult GetMultiCostSource(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_CostSource_Multi);
        }

        // lấy ds phương tiện đi làm
        public ActionResult GetMultiVehicle(string text)
        {
            return GetDataForControl<CatNameEntityModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Vehicle_Multi);
        }


        public ActionResult ExportWordInterviewCampaignDetailByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            string status = string.Empty;
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
            var lstInterviewCampaignDetail = baseService.GetData<Rec_InterviewCampaignDetailEntity>(objs, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetailByIds, UserLogin, ref status);
            if (lstInterviewCampaignDetail == null)
                return null;
            int i = 0;
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportRec_InterviewCampaignDetail" + suffix;
            if (lstInterviewCampaignDetail.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            int count = 0;
            foreach (var InterviewCampaignDetail in lstInterviewCampaignDetail)
            {
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;

                if (!string.IsNullOrEmpty(valueFields))
                {
                    List<object> lstObjExport = new List<object>();
                    var exportID = Guid.Parse(valueFields);
                    lstObjExport.Add(exportID);

                    template = exportService.GetData<Cat_ExportEntity>(exportID, ConstantSql.hrm_cat_sp_get_ExportById, UserLogin, ref status).FirstOrDefault();
                }
                if (template == null)
                {
                    continue;
                }
                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(InterviewCampaignDetail.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(InterviewCampaignDetail.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                //var ilJobVacancy = new List<Rec_InterviewCampaignDetailEntity>();
                //ilJobVacancy.Add(InterviewCampaignDetail);
                //ExportService.ExportWord(outputPath, templatepath, ilJobVacancy);
                string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
                //var serviceRec = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                //var result = serviceRec.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", InterviewCampaignDetail.CandidateID.Value.ToString());
                var result = service.GetData<Rec_CandidateModel>(Common.DotNetToOracle(InterviewCampaignDetail.CandidateID.ToString()), ConstantSql.hrm_rec_sp_get_CandidateBusinessById, ref status).FirstOrDefault();

                
                List<Rec_CandidateModel> lstRec = new List<Rec_CandidateModel>();
                lstRec.Add(result);
                DataTable tblCandidate = new DataTable();
                tblCandidate.TableName = "tblCandidate";
                tblCandidate = lstRec.Translate();
                var lstCandidateHistory = service.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(InterviewCampaignDetail.CandidateID.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryByCandidateId, ref status);
                DataTable tblCandidateHistory = new DataTable();
                tblCandidateHistory.TableName = "tblCandidateHistory";
                tblCandidateHistory = lstCandidateHistory.Translate();
                DataSet dsData = new DataSet();
                dsData.Tables.Add(tblCandidate);
                dsData.Tables.Add(tblCandidateHistory);
                ExportService.ExportWithRegions(outputPath, templatepath, dsData);

            }
            if (lstInterviewCampaignDetail.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        #region Cat_AbilitiTitle
        [HttpPost]
        public ActionResult GetAbilityTileList([DataSourceRequest] DataSourceRequest request, Cat_AbilityTileSearchModel model)
        {
            return GetListDataAndReturn<Cat_AbilityTileModel, Cat_AbilityTileEntity, Cat_AbilityTileSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AbilityTile);
        }

        public ActionResult ExportAllAbilityTileList([DataSourceRequest] DataSourceRequest request, Cat_AbilityTileSearchModel model)
        {
            return ExportAllAndReturn<Cat_AbilityTileEntity, Cat_AbilityTileModel, Cat_AbilityTileSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_AbilityTile);
        }

        public ActionResult GetMultiAbilityTile(string text)
        {
            return GetDataForControl<Cat_AbilityTileModel, Cat_AbilityTileEntity>(text, ConstantSql.hrm_cat_sp_get_AbilityTile_Multi);
        }

        #endregion

        #region Hien.Nguyen TimeAnalyze - HoldSalary

        public ActionResult GetTimeAnalyze_CatNameEntity(string text)
        {
            return GetDataForControl<CatNameEntityMultiModel, Cat_NameEntityMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TimeAnalyze);
        }

        #endregion

        public JsonResult GetReqDocumentByResignReasonID(Guid ID)
        {
            if (ID != Guid.Empty)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                List<object> listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = int.MaxValue - 1;
                var listReqDocument = baseService.GetData<Cat_ReqDocumentEntity>(listModel, ConstantSql.hrm_cat_sp_get_ReqDocument, UserLogin, ref status).ToList();
                if (listReqDocument.Count == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    listReqDocument = listReqDocument.Where(s => s.ResignReasonID == ID).ToList();
                    var result = from e in listReqDocument
                                 select new
                                 {
                                     Text = e.ReqDocumentName.TranslateString(),
                                     Value = e.ID,
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReqDocumentCascading(Guid ReqDocumentID, string ReqDocumentFilter)
        {
            var result = new List<Cat_ReqDocumentModel>();
            if (ReqDocumentID != Guid.Empty)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                var service = new Cat_ProvinceServices();
                List<object> listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = int.MaxValue - 1;
                var listReqDocument = baseService.GetData<Cat_ReqDocumentModel>(listModel, ConstantSql.hrm_cat_sp_get_ReqDocument, UserLogin, ref status).ToList();
                result = listReqDocument.Where(s => s.ResignReasonID == ReqDocumentID).ToList();
                if (!string.IsNullOrEmpty(ReqDocumentFilter))
                {
                    var rs = result.Where(s => s.ReqDocumentName != null && s.ReqDocumentName.ToLower().Contains(ReqDocumentFilter.ToLower())).ToList();
                    rs = rs.OrderBy(s => s.ReqDocumentName).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Cat_Sync
        [HttpPost]
        public ActionResult GetCatSync([DataSourceRequest] DataSourceRequest request, Cat_SyncSearchModel model)
        {
            return GetListDataAndReturn<Cat_SyncEntity, Cat_SyncEntity, Cat_SyncSearchModel>(request, model, ConstantSql.hrm_cat_sp_get_Sync);
        }
        #endregion

    }
}