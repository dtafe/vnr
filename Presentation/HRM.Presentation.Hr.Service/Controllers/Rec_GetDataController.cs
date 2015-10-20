using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Reflection;
using System.Collections;
using HRM.Business.Main.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using System.Data.SqlTypes;
using HRM.Presentation.Recruitment.Models;
using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using System.IO;
using HRM.Business.Hr.Models;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Category.Models;
using System.Web.Script.Serialization;
 

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Rec_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        #region Rec_QuestionType

        [HttpPost]
        public ActionResult GetRec_QuestionTypeList([DataSourceRequest] DataSourceRequest request, Rec_QuestionTypeSearchModel model)
        {
            return GetListDataAndReturn<Rec_QuestionTypeModel, Rec_QuestionTypeEntity, Rec_QuestionTypeSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_QuestionType);

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllRec_QuestionTypeList([DataSourceRequest] DataSourceRequest request, Rec_QuestionTypeSearchModel model)
        {
            return ExportAllAndReturn<Rec_QuestionTypeModel, Rec_QuestionTypeEntity, Rec_QuestionTypeSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_QuestionType);
        }

        public JsonResult GetMultiRec_QuestionType(string text)
        {
            return GetDataForControl<Rec_QuestionTypeModel, Rec_QuestionTypeEntity>(text, ConstantSql.hrm_rec_sp_get_QuestionType_multi);
        }

        #endregion

        #region Rec_RecruitmentCampaignItem

        [HttpPost]
        public ActionResult GetRecruitmentCampaignItemList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentCampaignItemSearchModel model)
        {
            return GetListDataAndReturn<Rec_RecruitmentCampaignItemModel, Rec_RecruitmentCampaignItemEntity, Rec_RecruitmentCampaignItemSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignItem);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportRecruitmentCampaignItemList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentCampaignItemSearchModel model)
        {
            return ExportAllAndReturn<Rec_RecruitmentCampaignItemEntity, Rec_RecruitmentCampaignItemModel, Rec_RecruitmentCampaignItemSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignItem);
        }


        [HttpPost]
        public ActionResult ExportRecruitmentCampaignItemSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_RecruitmentCampaignItemEntity, Rec_RecruitmentCampaignItemModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignItemByIds);
        }

        #endregion

        #region Rec_RecruitmentCampaign
        [HttpPost]
        public JsonResult GetMultiRecruitmentCampaign(string text)
        {
            return GetDataForControl<Rec_RecruitmentCampaignMultiModel, Rec_RecruitmentCampaignMultiEntity>(text, ConstantSql.hrm_rec_sp_get_RecruitmentCampaign_multi);
        }
        [HttpPost]
        public ActionResult GetRecruitmentCampaignList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentCampaignSearchModel model)
        {
            //return GetListDataAndReturn<Rec_RecruitmentCampaignModel, Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentCampaign);
            string status = string.Empty;
            var service = new BaseService();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<Rec_RecruitmentCampaignEntity>(lstModel, ConstantSql.hrm_rec_sp_get_RecruitmentCampaign, UserLogin, ref status);
            List<object> lstObjCosDetail = new List<object>();
            lstObjCosDetail.Add(1);
            lstObjCosDetail.Add(int.MaxValue - 1);
            var listRecCosDetail = service.GetData<Rec_RecCostDetailEntity>(lstObjCosDetail, ConstantSql.hrm_rec_sp_get_RecCosDetail, UserLogin,ref status);
            var listModel = new List<Rec_RecruitmentCampaignModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    var lstCosdetailByRecruimentcampain = listRecCosDetail.Where(s => s.RecCampaignID == item.ID).Sum(s => s.Amount);
                    if (lstCosdetailByRecruimentcampain != null && lstCosdetailByRecruimentcampain != 0)
                    {
                        item.Budget = lstCosdetailByRecruimentcampain;
                    }
                    var newModle = (Rec_RecruitmentCampaignModel)typeof(Rec_RecruitmentCampaignModel).CreateInstance();
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
            var listModelNull = new List<Rec_RecruitmentCampaignModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }
        public ActionResult ExportRecruitmentCampaignListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentCampaignSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_RecruitmentCampaignModel();
            var services = new BaseService();
            var result = GetListData<Rec_RecruitmentCampaignModel, Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentCampaign, ref status);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_RecruitmentCampaignModel",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportRecruitmentCampaignList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentCampaignSearchModel model)
        {
            return ExportAllAndReturn<Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignModel, Rec_RecruitmentCampaignSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentCampaign);
        }


        [HttpPost]
        public ActionResult ExportRecruitmentCampaignSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_RecruitmentCampaignEntity, Rec_RecruitmentCampaignModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_RecruitmentCampaignByIds);
        }
        [HttpPost]
        public ActionResult UpdateRecruitmentCampaignStatus(string selectedIds, string Status)
        {
            var service = new Rec_RecruitmentCampaignServices();
            return Json(service.UpdateRecruitmentCampaignStatus(selectedIds, Status), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateRecruitmentCampaignActive(string selectedIds, bool Value)
        {
            var service = new Rec_RecruitmentCampaignServices();
            return Json(service.UpdateRecruitmentCampaignActive(selectedIds, Value), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Rec_Tag
        public JsonResult GetMultiTag(string text)
        {
            return GetDataForControl<Rec_TagMultiModel, Rec_TagMultiEntity>(text, ConstantSql.hrm_rec_sp_get_Tag_Multi);
        }
        public ActionResult ExportAllTagList([DataSourceRequest] DataSourceRequest request, Rec_TagSearchModel model)
        {
            return ExportAllAndReturn<Rec_TagEntity, Rec_TagModel, Rec_TagSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Tag);
        }


        //public ActionResult ExportTagSelected(string selectedIds, string valueFields)
        //{
        //    return ExportSelectedAndReturn<Rec_TagEntity, Rec_TagModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_TagById);
        //}

        public ActionResult GetRecList([DataSourceRequest] DataSourceRequest request, Rec_TagSearchModel model)
        {
            return GetListDataAndReturn<Rec_TagModel, Rec_TagEntity, Rec_TagSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Tag);
        }
        #endregion

        #region Rec_QuestionBank

        [HttpPost]
        public ActionResult GetQuestionBankList([DataSourceRequest] DataSourceRequest request, Rec_QuestionBankSearchModel model)
        {
            return GetListDataAndReturn<Rec_QuestionBankModel, Rec_QuestionBankEntity, Rec_QuestionBankSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_QuestionBank);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportQuestionBankList([DataSourceRequest] DataSourceRequest request, Rec_QuestionBankSearchModel model)
        {
            return ExportAllAndReturn<Rec_QuestionBankEntity, Rec_QuestionBankModel, Rec_QuestionBankSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_QuestionBank);
        }


        [HttpPost]
        public ActionResult ExportQuestionBankSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_QuestionBankEntity, Rec_QuestionBankModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_QuestionBankByIds);
        }
        [HttpPost]
        public JsonResult GetMultiQuestionBank(string text)
        {
            return GetDataForControl<Rec_QuestionBankMultiModel, Rec_QuestionBankMultiEntity>(text, ConstantSql.hrm_rec_sp_get_QuestionBank_multi);
        }
        #endregion

        #region Rec_Questionaire
        public JsonResult GetMultiQuestionaire(string text)
        {
            return GetDataForControl<Rec_QuestionaireModel, Rec_QuestionaireEntity>(text, ConstantSql.hrm_rec_sp_get_Questionaire_Multi);
        }

        [HttpPost]
        public ActionResult GetQuestionaireList([DataSourceRequest] DataSourceRequest request, Rec_QuestionaireSearchModel model)
        {
            return GetListDataAndReturn<Rec_QuestionaireModel, Rec_QuestionaireEntity, Rec_QuestionaireSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Questionaire);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportQuestionaireList([DataSourceRequest] DataSourceRequest request, Rec_QuestionaireSearchModel model)
        {
            return ExportAllAndReturn<Rec_QuestionaireEntity, Rec_QuestionaireModel, Rec_QuestionaireSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Questionaire);
        }


        [HttpPost]
        public ActionResult ExportQuestionaireSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_QuestionaireEntity, Rec_QuestionaireModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_QuestionaireByIds);
        }

        #endregion

        #region Rec_Questionaire
        [HttpPost]
        public ActionResult GetQuestionByQuestionaireIdList([DataSourceRequest] DataSourceRequest request, Guid? QuestionaireID)
        {
            Rec_QuestionSearchModel model = new Rec_QuestionSearchModel();
            model.QuestionaireID = QuestionaireID;
            return GetListDataAndReturn<Rec_QuestionModel, Rec_QuestionEntity, Rec_QuestionSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_QuestionByQuestionaireId);

        }
        #endregion

        #region Rec_SourceAds
        [HttpPost]
        public JsonResult GetMultiSourceAds(string text)
        {
            return GetDataForControl<Rec_SourceAdsMultiModel, Rec_SourceAdsMultiEntity>(text, ConstantSql.hrm_rec_sp_get_SourceAds_Multi);
        }
        public ActionResult GetSourceAdsList([DataSourceRequest] DataSourceRequest request, Rec_SourceAdsSearchModel model)
        {
            return GetListDataAndReturn<Rec_SourceAdsModel, Rec_SourceAdsEntity, Rec_SourceAdsSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_SourceAds);
        }
        public ActionResult ExportSourceAdsSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_SourceAdsEntity, Rec_SourceAdsModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_SourceAdsByIds);
        }
        public ActionResult ExportAllSourceAdsList([DataSourceRequest] DataSourceRequest request, Rec_SourceAdsSearchModel model)
        {
            return ExportAllAndReturn<Rec_SourceAdsEntity, Rec_SourceAdsModel, Rec_SourceAdsSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_SourceAds);
        }
        #endregion
        #region Rec_RecruitmentHistory(New)

        public ActionResult GetRecruitmentHistoryListNew([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentHistorySearchModelNew model)
        {
            return GetListDataAndReturn<Rec_RecruitmentHistoryModel, Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistorySearchModelNew>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentHistory);
        }
        public ActionResult ExportAllRecruitmentHistoryListNew([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentHistorySearchModelNew model)
        {
            return ExportAllAndReturn<Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistoryModel, Rec_RecruitmentHistorySearchModelNew>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentHistory);
        }
        public ActionResult ExportRecruitmentHistoryListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentHistorySearchModelNew model)
        {
            request.PageSize = 10000000;
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_RecruitmentHistoryModel();
            var result = GetListData<Rec_RecruitmentHistoryModel, Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistorySearchModelNew>(request, model, ConstantSql.hrm_rec_sp_get_RecruitmentHistory, ref status);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_RecruitmentHistoryModel",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
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

        #region Rec_Candidate

        public ActionResult ExportCandidateByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Rec_CandidateModel();
            var baseService = new BaseService();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
            {
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            }
            var lstCandidate = baseService.GetData<Rec_CandidateEntity>(strIDs, ConstantSql.hrm_rec_sp_get_CandidateByIds, UserLogin, ref status);
            if (lstCandidate == null)
                return null;
            int i = 0;
            var lstCandidateID = lstCandidate.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstCandidateID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var candidate in lstCandidate)
            {
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
                //template = exportService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();

                if (!string.IsNullOrEmpty(valueFields))
                {
                    template = exportService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, UserLogin,ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(candidate.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(candidate.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lstcontract = new List<Rec_CandidateEntity>();
                lstcontract.Add(candidate);
                ExportService.ExportWord(outputPath, templatepath, lstcontract);
            }
            if (lstCandidateID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }




        public JsonResult GetMultiCandidate(string text)
        {
            return GetDataForControl<Rec_CandidateMultiModel, Rec_CandidateMultiEntity>(text, ConstantSql.hrm_rec_sp_get_Candidate_Multi);
        }

        public ActionResult ExportCandidateSelected(string selectedIds, string valueFields)
        {
            //return ExportSelectedAndReturn<Rec_CandidateEntity, Rec_CandidateModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_CandidateByIds);
            string status = string.Empty;
            var baseService = new BaseService();
            var lstCandidates = baseService.GetData<Rec_CandidateModel>(Common.DotNetToOracle(selectedIds), ConstantSql.hrm_rec_sp_get_CandidateByIds, UserLogin, ref status).ToList();
            foreach (var item in lstCandidates)
            {
                if (item.ReasonFailFilter != null)
                {
                    List<string> lsititem = item.ReasonFailFilter.Split(',').ToList();
                    string Stringresult = string.Empty;
                    foreach (var itemstring in lsititem)
                    {
                        Stringresult += itemstring.TranslateString() + ',';
                    }
                    Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                    item.ReasonFailFilter = Stringresult;
                }
            }
            status = ExportService.Export(lstCandidates, valueFields.TryGetValue<string>().Split(','));
            return Json(status);
        }

        public ActionResult GetCandidateList([DataSourceRequest] DataSourceRequest request, Rec_CandidateSearchModel model)
        {
            if (model.Evaluated == HRM.Infrastructure.Utilities.EnumDropDown.StatusHealth.E_ACCEPT.ToString())
            {
                model.IsEvaluated = true;
            }
            else if (model.Evaluated == HRM.Infrastructure.Utilities.EnumDropDown.StatusHealth.E_NOTACCEPT.ToString())
            {
                model.IsEvaluated = false;
            }

            string status = string.Empty;
            var service = new BaseService();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<Rec_CandidateEntity>(lstModel, ConstantSql.hrm_rec_sp_get_Candidate, UserLogin, ref status);

            var listModel = new List<Rec_CandidateModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    if (item.ReasonFailFilter != null)
                    {
                        List<string> lsititem = item.ReasonFailFilter.Split(',').ToList();
                        string Stringresult = string.Empty;
                        foreach (var itemstring in lsititem)
                        {
                            Stringresult += itemstring.TranslateString() + ',';
                        }
                        Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                        item.ReasonFailFilter = Stringresult;
                    }
                    var newModle = (Rec_CandidateModel)typeof(Rec_CandidateModel).CreateInstance();
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
            var listModelNull = new List<Rec_CandidateModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }


        public ActionResult ExportAllCandidateList([DataSourceRequest] DataSourceRequest request, Rec_CandidateSearchModel model)
        {
            string status = string.Empty;
            var service = new BaseService();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = int.MaxValue - 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<Rec_CandidateEntity>(lstModel, ConstantSql.hrm_rec_sp_get_Candidate, UserLogin, ref status);
            if (model.Evaluated == HRM.Infrastructure.Utilities.EnumDropDown.StatusHealth.E_ACCEPT.ToString())
            {
                listEntity = listEntity.Where(s => s.PassFilterResume == true).ToList();
            }
            if (model.Evaluated == HRM.Infrastructure.Utilities.EnumDropDown.StatusHealth.E_NOTACCEPT.ToString())
            {
                listEntity = listEntity.Where(s => s.PassFilterResume == false).ToList();
            }
            var listModel = new List<Rec_CandidateModel>();
            if (listEntity != null)
            {
                foreach (var item in listEntity)
                {
                    if (item.ReasonFailFilter != null)
                    {
                        List<string> lsititem = item.ReasonFailFilter.Split(',').ToList();
                        string Stringresult = string.Empty;
                        foreach (var itemstring in lsititem)
                        {
                            Stringresult += itemstring.TranslateString() + ',';
                        }
                        Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                        item.ReasonFailFilter = Stringresult;
                    }
                }
            }
            status = ExportService.Export(listEntity, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));

            return Json(status);
        }


        public ActionResult GetCandidateInBlackList([DataSourceRequest] DataSourceRequest request, Rec_CandidateInBlackSearchModel model)
        {
            string status = string.Empty;
            var services = new BaseService();
            var result = GetListData<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateInBlackSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateBlack, ref status);

            foreach (var item in result)
            {
                if (item.ReasonFailFilter != null)
                {
                    List<string> lsititem = item.ReasonFailFilter.Split(',').ToList();
                    string Stringresult = string.Empty;
                    foreach (var itemstring in lsititem)
                    {
                        Stringresult += itemstring.TranslateString() + ',';
                    }
                    Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                    item.ReasonFailFilter = Stringresult;
                }

            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult ExportCandidateListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_CandidateSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_CandidateModel();
            var baseService = new BaseService();
            request.PageSize = int.MaxValue - 1;
            var result = GetListData<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Candidate, ref status);
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateExamFrom", Value = model.DateExamFrom == null ? DateTime.Now : model.DateExamFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateExamTo", Value = model.DateExamTo == null ? DateTime.Now : model.DateExamTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_CandidateModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var objInterview = new List<object>();
            objInterview.AddRange(new object[2]);
            objInterview[0] = 1;
            objInterview[1] = int.MaxValue - 1;
            var lstInterView = baseService.GetData<Rec_InterviewModel>(objInterview, ConstantSql.hrm_rec_sp_get_InterviewDataReport,UserLogin, ref status).ToList();

            foreach (var item in result)
            {
                if (lstInterView.Count > 0)
                {
                    var dataLevel1 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 1).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel2 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 2).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel3 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 3).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel4 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 4).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel5 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 5).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (dataLevel1 != null)
                    {
                        item.Score1_1 = dataLevel1.Score1;
                        item.Score1_2 = dataLevel1.Score2;
                        item.Score1_3 = dataLevel1.Score3;
                        item.KQ1 = dataLevel1.ResultInterviewView;
                        item.LanguageCode1 = dataLevel1.LanguageCode;
                        item.DateInterview1 = dataLevel1.DateInterview;
                    }
                    if (dataLevel2 != null)
                    {
                        item.Score2_1 = dataLevel2.Score1;
                        item.Score2_2 = dataLevel2.Score2;
                        item.Score2_3 = dataLevel2.Score3;
                        item.KQ2 = dataLevel2.ResultInterviewView;
                        item.LanguageCode2 = dataLevel2.LanguageCode;
                        item.DateInterview2 = dataLevel2.DateInterview;
                    }
                    if (dataLevel3 != null)
                    {
                        item.Score3_1 = dataLevel3.Score1;
                        item.Score3_2 = dataLevel3.Score2;
                        item.Score3_3 = dataLevel3.Score3;
                        item.KQ3 = dataLevel3.ResultInterviewView;
                        item.LanguageCode3 = dataLevel3.LanguageCode;
                        item.DateInterview3 = dataLevel3.DateInterview;
                    }
                    if (dataLevel4 != null)
                    {
                        item.Score4_1 = dataLevel4.Score1;
                        item.Score4_2 = dataLevel4.Score2;
                        item.Score4_3 = dataLevel4.Score3;
                        item.KQ4 = dataLevel4.ResultInterviewView;
                        item.LanguageCode4 = dataLevel4.LanguageCode;
                        item.DateInterview4 = dataLevel4.DateInterview;
                    }
                    if (dataLevel5 != null)
                    {
                        item.Score5_1 = dataLevel5.Score1;
                        item.Score5_2 = dataLevel5.Score2;
                        item.Score5_3 = dataLevel5.Score3;
                        item.KQ5 = dataLevel5.ResultInterviewView;
                        item.LanguageCode5 = dataLevel5.LanguageCode;
                        item.DateInterview5 = dataLevel5.DateInterview;
                    }
                }
            }

            if (model.ExportId != Guid.Empty)
            {

                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetListCandidate([DataSourceRequest] DataSourceRequest request, Rec_CandidateGeneralMultiSearchModel candidateModel)
        {
            #region [Hien.Nguyen]
            if (candidateModel.CandidateID != null && candidateModel.CandidateID != Guid.Empty)
                candidateModel.Keyword = null;
            #endregion
            return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateGeneralMultiSearchModel>(request, candidateModel, ConstantSql.hrm_hr_sp_get_Candidate_GeneralGrid);
        }
        [HttpPost]
        public ActionResult UpdateStatusHireCandidate(string selectedIds, string userID)
        {
            var service = new Rec_CandidateServices();
            service.UpdateStatusHireCandidate(selectedIds, userID, UserLogin);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFilterCandidateListValidate(Rec_FilterCandidateModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_FilterCandidateModel>(Model, "FilterCandidateInfo", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        public ActionResult GetFilterCandidateList([DataSourceRequest] DataSourceRequest request, Rec_FilterCandidateModel model)
        {
            var services = new Rec_CandidateServices();
            var dateFrom = DateTime.Now.Date;
            var dateTo = DateTime.Now.Date;
            var lstModel = new List<Rec_CandidateModel>();

            if (model.DateFrom != null)
            {
                dateFrom = model.DateFrom.Value;
            }

            if (model.DateTo != null)
            {
                dateTo = model.DateTo.Value;
            }
            var result = services.FilterCandidate(dateFrom, dateTo, model.JobVacancyIDs, UserLogin, model.GetListFail, model.IsIncludeEvaCandidate).ToList();
            if (result.Count > 0)
            {
                lstModel = result.Translate<Rec_CandidateModel>();
            }
            return Json(lstModel.ToDataSourceResult(request));
            //return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateGeneralMultiSearchModel>(request, candidateModel, ConstantSql.hrm_hr_sp_get_Candidate_GeneralGrid);
        }

        // Click nút gọi điện ds ứng viên trúng tuyển
        public ActionResult SubmitCall(string selectedIds)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => Guid.Parse(x))
                   .ToList();
            }
            var lstMealRecordMissing = new List<Rec_CandidateEntity>();
            var services = new Rec_CandidateServices();
            services.SubmitCall(ids);
            return Json("");
        }

        #endregion

        #region Rec_WaitingInterviewList
        //[Tho.Bui] - Load theo lich su ung vien
        public ActionResult GetWaitingInterList([DataSourceRequest] DataSourceRequest request, Rec_WaitingInterviewSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<Rec_RecruitmentHistoryModel> resultRecruitmentHistory = new List<Rec_RecruitmentHistoryModel>();
            Rec_RecruitmentHistoryModel ObjRecruitmentHistory = null;
            var InterviewServices = new Rec_InterviewServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var lstWaitingInterview = baseService.GetData<Rec_RecruitmentHistoryModel>(lstModel, ConstantSql.hrm_rec_sp_get_WaitingInterList, UserLogin, ref status);

            if (lstWaitingInterview == null || lstWaitingInterview.Count == 0)
            {
                return null;
            }

          

            lstWaitingInterview = lstWaitingInterview.Where(s => s.JobVacancyID != null).ToList();

            var lstrechisids = lstWaitingInterview.Select(s => s.ID).Distinct().ToList();

            var objCampaignDetail = new List<object>();
            objCampaignDetail.AddRange(new object[16]);
            objCampaignDetail[14] = 1;
            objCampaignDetail[15] = int.MaxValue - 1;
            var resultCampaignDetail = baseService.GetData<Rec_InterviewCampaignDetailModel>(objCampaignDetail, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail, UserLogin, ref status);

            resultCampaignDetail = resultCampaignDetail.Where(s => s.RecruitmentHistoryID != null && lstrechisids.Contains(s.RecruitmentHistoryID.Value)).ToList();

            var lstjobvacancyids = lstWaitingInterview.Where(s => s.JobVacancyID != null).Select(s => s.JobVacancyID).Distinct().ToList();
            string strJobVacancyIds = string.Empty;
            foreach (Guid item in lstjobvacancyids)
            {
                strJobVacancyIds += item;
                strJobVacancyIds += ",";
            }
            if (strJobVacancyIds.Length > 0)
            {
                strJobVacancyIds = strJobVacancyIds.Substring(0, strJobVacancyIds.Length - 1);
            }
            var lstJobVacancy = baseService.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(strJobVacancyIds), ConstantSql.hrm_rec_sp_get_JobVacancyByIds, UserLogin, ref status);
            #region Kết quả là rớt hoặc vỏng phỏng vấn = cuối thì KHÔNG load vào danh sách chờ phỏng vấn
            foreach (var his in lstWaitingInterview)
            {
                ObjRecruitmentHistory = new Rec_RecruitmentHistoryModel();
                var lstresultCampaignDetail = resultCampaignDetail.Where(s => s.RecruitmentHistoryID == his.ID && s.LevelInterview == null).ToList();
                if (his.JobVacancyID.HasValue)
                {
                    var entityJobVacancy = lstJobVacancy.Where(s => s.ID == his.JobVacancyID).FirstOrDefault();
                    if (entityJobVacancy != null && his.LevelInterview != entityJobVacancy.NoLevelInterview && lstresultCampaignDetail.Count == 0 && his.Status != HRM.Infrastructure.Utilities.Interview.E_PASS.ToString())
                    {
                        ObjRecruitmentHistory = his;
                        resultRecruitmentHistory.Add(ObjRecruitmentHistory);
                    }
                }
            }
            #endregion
            return Json(resultRecruitmentHistory.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWaitingInterviewList([DataSourceRequest] DataSourceRequest request, Rec_WaitingInterviewSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<Rec_CandidateModel> resultCandidate = new List<Rec_CandidateModel>();
            Rec_CandidateModel ObjCandidate = null;
            var InterviewServices = new Rec_InterviewServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = baseService.GetData<Rec_CandidateModel>(lstModel, ConstantSql.hrm_rec_sp_get_WaitingInterviewList, UserLogin, ref status);
            #region Kết quả là rớt hoặc vỏng phỏng vấn = cuối thì KHÔNG load vào danh sách chờ phỏng vấn

            result = result.Where(s => s.Status != HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString()).ToList();
            foreach (var item in result)
            {
                ObjCandidate = new Rec_CandidateModel();
                objs = new List<object>();
                objs.Add(item.ID);
                objs.Add(1);
                objs.Add(10000);
                //var resultInterview = baseService.GetData<Rec_InterviewModel>(objs, ConstantSql.hrm_rec_sp_get_InterviewByCandidateID, ref status);
                //Danh sach ke hoach cua ung vien
                var resultCampaignDetail = baseService.GetData<Rec_InterviewCampaignDetailModel>(objs, ConstantSql.hrm_rec_sp_get_InCamDetailByCddId, UserLogin, ref status).ToList();
                if (item.JobVacancyID.HasValue)
                {
                    var entityJobVacancy = service.GetByIdUseStore<Rec_JobVacancyEntity>(item.JobVacancyID.Value, ConstantSql.hrm_rec_sp_get_JobVacancyId, ref status);
                    if (item.LevelInterview == null)
                        item.LevelInterview = 0;
                    if (item.LevelInterview != entityJobVacancy.NoLevelInterview && item.LevelInterview == resultCampaignDetail.Count)
                    {
                        ObjCandidate = item;
                        resultCandidate.Add(ObjCandidate);
                    }
                }
            }
            #endregion
            return Json(resultCandidate.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //var result = GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_WaitingInterviewList);
        //return result;
        //}

        public ActionResult ExportWatingInterviewByTemplate([DataSourceRequest] DataSourceRequest request, Rec_WaitingInterviewSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<Rec_RecruitmentHistoryModel> resultRecruitmentHistory = new List<Rec_RecruitmentHistoryModel>();
            Rec_RecruitmentHistoryModel ObjRecruitmentHistory = null;
            var InterviewServices = new Rec_InterviewServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var lstWaitingInterview = baseService.GetData<Rec_RecruitmentHistoryModel>(lstModel, ConstantSql.hrm_rec_sp_get_WaitingInterList, UserLogin, ref status);

            if (lstWaitingInterview == null || lstWaitingInterview.Count == 0)
            {
                return null;
            }
            lstWaitingInterview = lstWaitingInterview.Where(s => s.JobVacancyID != null).ToList();


            var lstrechisids = lstWaitingInterview.Select(s => s.ID).Distinct().ToList();

            var objCampaignDetail = new List<object>();
            objCampaignDetail.AddRange(new object[16]);
            objCampaignDetail[14] = 1;
            objCampaignDetail[15] = int.MaxValue - 1;
            var resultCampaignDetail = baseService.GetData<Rec_InterviewCampaignDetailModel>(objCampaignDetail, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail, UserLogin, ref status);

            resultCampaignDetail = resultCampaignDetail.Where(s => s.RecruitmentHistoryID != null && lstrechisids.Contains(s.RecruitmentHistoryID.Value)).ToList();

            var lstjobvacancyids = lstWaitingInterview.Where(s => s.JobVacancyID != null).Select(s => s.JobVacancyID).Distinct().ToList();
            string strJobVacancyIds = string.Empty;
            foreach (Guid item in lstjobvacancyids)
            {
                strJobVacancyIds += item;
                strJobVacancyIds += ",";
            }
            if (strJobVacancyIds.Length > 0)
            {
                strJobVacancyIds = strJobVacancyIds.Substring(0, strJobVacancyIds.Length - 1);
            }
            var lstJobVacancy = baseService.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(strJobVacancyIds), ConstantSql.hrm_rec_sp_get_JobVacancyByIds, UserLogin, ref status);
            #region Kết quả là rớt hoặc vỏng phỏng vấn = cuối thì KHÔNG load vào danh sách chờ phỏng vấn
            foreach (var his in lstWaitingInterview)
            {
                ObjRecruitmentHistory = new Rec_RecruitmentHistoryModel();
                var lstresultCampaignDetail = resultCampaignDetail.Where(s => s.RecruitmentHistoryID == his.ID && s.LevelInterview == null).ToList();
                if (his.JobVacancyID.HasValue)
                {
                    var entityJobVacancy = lstJobVacancy.Where(s => s.ID == his.JobVacancyID).FirstOrDefault();
                    if (entityJobVacancy != null && his.LevelInterview != entityJobVacancy.NoLevelInterview && lstresultCampaignDetail.Count == 0 && his.Status != HRM.Infrastructure.Utilities.Interview.E_PASS.ToString())
                    {
                        ObjRecruitmentHistory = his;
                        resultRecruitmentHistory.Add(ObjRecruitmentHistory);
                    }
                }
            }
            #endregion

            var isDataTable = false;
            object obj = new Rec_RecruitmentHistoryModel();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = resultRecruitmentHistory;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_RecruitmentHistoryModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var objInterview = new List<object>();
            objInterview.AddRange(new object[2]);
            objInterview[0] = 1;
            objInterview[1] = int.MaxValue - 1;
            var lstInterView = baseService.GetData<Rec_InterviewModel>(objInterview, ConstantSql.hrm_rec_sp_get_InterviewDataReport, UserLogin, ref status).ToList();

            foreach (var item in resultRecruitmentHistory)
            {
                if (lstInterView.Count > 0)
                {
                    var dataLevel1 = lstInterView.Where(s => s.CandidateID == item.CandidateID && s.LevelInterview == 1).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel2 = lstInterView.Where(s => s.CandidateID == item.CandidateID && s.LevelInterview == 2).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel3 = lstInterView.Where(s => s.CandidateID == item.CandidateID && s.LevelInterview == 3).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel4 = lstInterView.Where(s => s.CandidateID == item.CandidateID && s.LevelInterview == 4).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel5 = lstInterView.Where(s => s.CandidateID == item.CandidateID && s.LevelInterview == 5).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if(dataLevel1 != null)
                    {
                        item.Score1_1 = dataLevel1.Score1;
                        item.Score1_2 = dataLevel1.Score2;
                        item.Score1_3 = dataLevel1.Score3;
                        item.KQ1 = dataLevel1.ResultInterviewView;
                        item.LanguageCode1 = dataLevel1.LanguageCode;
                        item.DateInterview1 = dataLevel1.DateInterview;
                    }
                    if (dataLevel2 != null)
                    {
                        item.Score2_1 = dataLevel2.Score1;
                        item.Score2_2 = dataLevel2.Score2;
                        item.Score2_3 = dataLevel2.Score3;
                        item.KQ2 = dataLevel2.ResultInterviewView;
                        item.LanguageCode2 = dataLevel2.LanguageCode;
                        item.DateInterview2 = dataLevel2.DateInterview;
                    }
                    if (dataLevel3 != null)
                    {
                        item.Score3_1 = dataLevel3.Score1;
                        item.Score3_2 = dataLevel3.Score2;
                        item.Score3_3 = dataLevel3.Score3;
                        item.KQ3 = dataLevel3.ResultInterviewView;
                        item.LanguageCode3 = dataLevel3.LanguageCode;
                        item.DateInterview3 = dataLevel3.DateInterview;
                    }
                    if (dataLevel4 != null)
                    {
                        item.Score4_1 = dataLevel4.Score1;
                        item.Score4_2 = dataLevel4.Score2;
                        item.Score4_3 = dataLevel4.Score3;
                        item.KQ4 = dataLevel4.ResultInterviewView;
                        item.LanguageCode4 = dataLevel4.LanguageCode;
                        item.DateInterview4 = dataLevel4.DateInterview;
                    }
                    if (dataLevel5 != null)
                    {
                        item.Score5_1 = dataLevel5.Score1;
                        item.Score5_2 = dataLevel5.Score2;
                        item.Score5_3 = dataLevel5.Score3;
                        item.KQ5 = dataLevel5.ResultInterviewView;
                        item.LanguageCode5 = dataLevel5.LanguageCode;
                        item.DateInterview5 = dataLevel5.DateInterview;
                    }
                }
            }

            if (model.ExportId != Guid.Empty)
            {

                var fullPath = ExportService.Export(model.ExportId, resultRecruitmentHistory, null, model.ExportType);
                return Json(fullPath);
            }

            return Json(resultRecruitmentHistory.ToDataSourceResult(request));
        }
        public ActionResult ExportIntervieCampaignDetaiByTemplate([DataSourceRequest] DataSourceRequest request, Rec_InterviewCampaignDetailSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<object> objshis = new List<object>();
            List<Rec_InterviewCampaignDetailModel> resultInterviewCampaignDetail = new List<Rec_InterviewCampaignDetailModel>();
            var InterviewServices = new Rec_InterviewCampaignDetailServices();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = baseService.GetData<Rec_InterviewCampaignDetailModel>(lstModel, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail, UserLogin, ref status);
            #region Load Chi tiet ke hoach chua phong van
            if (result != null)
            {
                var InTerCamDetaiServices = new Rec_InterviewCampaignDetailServices();
                var HistoryServices = new Rec_RecruitmentHistoryServices();
                var lstCandidateIDS = result.Select(s => s.CandidateID).Distinct().ToList();
                string temp = string.Empty;
                foreach (Guid item in lstCandidateIDS)
                {
                    temp += item;
                    temp += ",";
                }
                if (temp.Length > 0)
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
                var lstCandidateHistory = HistoryServices.GetData<Rec_RecruitmentHistoryEntity>(Common.DotNetToOracle(temp), ConstantSql.hrm_rec_sp_get_RecHisByListCandidateID, UserLogin, ref status).ToList();
                foreach (var item in result)
                {
                    //var IlInterviewcampaugnDetail = result.Where(s => s.CandidateID == item.CandidateID).ToList();
                    var ObjHisCandidate = lstCandidateHistory.Where(s => s.ID == item.RecruitmentHistoryID).FirstOrDefault();
                    if (ObjHisCandidate == null)
                    {
                        continue;
                    }
                    Rec_JobVacancyEntity entityJobVacancy = null;
                    if (ObjHisCandidate != null && ObjHisCandidate.JobVacancyID != Guid.Empty && ObjHisCandidate.JobVacancyID != null)
                    {
                        entityJobVacancy = service.GetByIdUseStore<Rec_JobVacancyEntity>(ObjHisCandidate.JobVacancyID.Value, ConstantSql.hrm_rec_sp_get_JobVacancyId, ref status);
                    }
                    if (entityJobVacancy != null)
                    {
                        if (item.LevelInterview != null && item.LevelInterview == entityJobVacancy.NoLevelInterview)
                        {
                            continue;
                        }
                    }
                    if (item.LevelInterview == null && (ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_PASS.ToString()
                     && ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString()
                        ))
                    {
                        if (ObjHisCandidate != null)
                        {
                            if (ObjHisCandidate.LevelInterview == null)
                            {
                                item.LevelInterview = 1;
                                resultInterviewCampaignDetail.Add(item);
                            }
                            else
                            {
                                item.LevelInterview = ObjHisCandidate.LevelInterview + 1;
                                resultInterviewCampaignDetail.Add(item);
                            }
                        }
                    }
                }
            }
            resultInterviewCampaignDetail = resultInterviewCampaignDetail.Distinct().ToList();
            #endregion

            var isDataTable = false;
            object obj = new Rec_InterviewCampaignDetailModel();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = resultInterviewCampaignDetail;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_InterviewCampaignDetailModel",
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

                var fullPath = ExportService.Export(model.ExportId, resultInterviewCampaignDetail, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            return Json(resultInterviewCampaignDetail.ToDataSourceResult(request));
        }

        public ActionResult ExportAllWaitingInterviewList([DataSourceRequest] DataSourceRequest request, Rec_WaitingInterviewSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<Rec_RecruitmentHistoryModel> resultRecruitmentHistory = new List<Rec_RecruitmentHistoryModel>();
            Rec_RecruitmentHistoryModel ObjRecruitmentHistory = null;
            var InterviewServices = new Rec_InterviewServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var lstWaitingInterview = baseService.GetData<Rec_RecruitmentHistoryModel>(lstModel, ConstantSql.hrm_rec_sp_get_WaitingInterList, UserLogin, ref status);

            if (lstWaitingInterview == null || lstWaitingInterview.Count == 0)
            {
                return null;
            }
            lstWaitingInterview = lstWaitingInterview.Where(s => s.JobVacancyID != null).ToList();
            var lstrechisids = lstWaitingInterview.Select(s => s.ID).ToList();
            var resultCampaignDetail = new List<Rec_InterviewCampaignDetailModel>();
            int _total = lstrechisids.Count;
            int _totalPage = _total / 100 + 1;
            int _pageSize = 100;
            for (int _page = 1; _page <= _totalPage; _page++)
            {
                int _skip = _pageSize * (_page - 1);
                var _listCurrenPage = lstrechisids.Skip(_skip).Take(_pageSize).ToList();
                string _strselectedIDs = Common.DotNetToOracle(string.Join(",", _listCurrenPage));

                var lstCampaignDetail = baseService.GetData<Rec_InterviewCampaignDetailModel>(_strselectedIDs, ConstantSql.hrm_rec_sp_get_InCamDetailByLstRechisIDs, UserLogin, ref status).ToList();
                if (lstCampaignDetail != null && lstCampaignDetail.Count > 0)
                {
                    resultCampaignDetail.AddRange(lstCampaignDetail);
                }
            }

            var lstjobvacancyids = lstWaitingInterview.Where(s => s.JobVacancyID != null).Select(s => s.JobVacancyID).Distinct().ToList();
            string strJobVacancyIds = string.Empty;
            foreach (Guid item in lstjobvacancyids)
            {
                strJobVacancyIds += item;
                strJobVacancyIds += ",";
            }
            if (strJobVacancyIds.Length > 0)
            {
                strJobVacancyIds = strJobVacancyIds.Substring(0, strJobVacancyIds.Length - 1);
            }
            var lstJobVacancy = baseService.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(strJobVacancyIds), ConstantSql.hrm_rec_sp_get_JobVacancyByIds, UserLogin, ref status);
            foreach (var his in lstWaitingInterview)
            {
                ObjRecruitmentHistory = new Rec_RecruitmentHistoryModel();
                var lstresultCampaignDetail = resultCampaignDetail.Where(s => s.RecruitmentHistoryID == his.ID && s.LevelInterview == null).ToList();
                if (his.JobVacancyID.HasValue)
                {
                    var entityJobVacancy = lstJobVacancy.Where(s => s.ID == his.JobVacancyID).FirstOrDefault();
                    if (his.LevelInterview == null)
                        his.LevelInterview = 0;
                    if (entityJobVacancy != null && his.LevelInterview != entityJobVacancy.NoLevelInterview && lstresultCampaignDetail.Count == 0 && his.Status != HRM.Infrastructure.Utilities.Interview.E_PASS.ToString())
                    {
                        ObjRecruitmentHistory = his;
                        resultRecruitmentHistory.Add(ObjRecruitmentHistory);
                    }
                }
            }

            status = ExportService.Export(resultRecruitmentHistory, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));

            return Json(status);
        }
        #endregion


        #region Rec_CandidatesFailToGetTheJob
        public ActionResult GetCandidatesFailToGetTheJob([DataSourceRequest] DataSourceRequest request, Rec_CandidateFailToGetTheJobSearchModel model)
        {
            return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateFailToGetTheJobSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidatesFailToGetTheJob);
        }
        public ActionResult ExportCandidatesFailToGetTheJobListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_CandidateFailToGetTheJobSearchModel model)
        {
            //return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidatesFailToGetTheJob);

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_CandidateModel();
            var services = new BaseService();
            var result = GetListData<Rec_CandidateModel, Rec_CandidateEntity, Rec_CandidateFailToGetTheJobSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Relatives, ref status);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_CandidateModel",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllFailJobList([DataSourceRequest] DataSourceRequest request, Rec_EnrolledCandidateSearchModel model)
        {
            return ExportAllAndReturn<Rec_CandidateEntity, Rec_CandidateModel, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidatesFailToGetTheJob);
        }
        public ActionResult ExportFailJobSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_CandidateEntity, Rec_CandidateModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_CanFailJobIds);
        }
        #endregion
        #region Rec_JobVacancy
        [HttpPost]
        public JsonResult GetMultiJobVacancy(string text)
        {
            return GetDataForControl<Rec_JobVacancyMultiModel, Rec_JobVacancyMultiEntity>(text, ConstantSql.hrm_rec_sp_get_JobVacancy_multi);
        }
        [HttpPost]
        public ActionResult GetJobVacancyList([DataSourceRequest] DataSourceRequest request, Rec_JobVacancySearchModel model)
        {
            return GetListDataAndReturn<Rec_JobVacancyModel, Rec_JobVacancyEntity, Rec_JobVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobVacancy);

        }
        public ActionResult ExportJobVacancyListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_JobVacancySearchModel model)
        {
            //return GetListDataAndReturn<Rec_JobVacancyModel, Rec_JobVacancyEntity, Rec_JobVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobVacancy);

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_JobVacancyModel();
            var services = new BaseService();
            var result = GetListData<Rec_JobVacancyModel, Rec_JobVacancyEntity, Rec_JobVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobVacancy, ref status);
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStartFrom", Value = model.DateStartFrom == null ? DateTime.Now : model.DateStartFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateStartTo", Value = model.DateStartTo == null ? DateTime.Now : model.DateStartTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_JobVacancyModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportJobVacancyList([DataSourceRequest] DataSourceRequest request, Rec_JobVacancySearchModel model)
        {
            return ExportAllAndReturn<Rec_JobVacancyEntity, Rec_JobVacancyModel, Rec_JobVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobVacancy);
        }


        [HttpPost]
        public ActionResult ExportJobVacancySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_JobVacancyEntity, Rec_JobVacancyModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_JobVacancyByIds);
        }

        #endregion
        #region Rec_JobCondition
        [HttpPost]
        public ActionResult GetJobConditionByVacancyId([DataSourceRequest] DataSourceRequest request, Rec_JobConditionByVacancySearchModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.JobConditionIDs))
            {
                var ids = model.JobConditionIDs
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Common.DotNetToOracle(x))
                        .ToList();
                model.JobConditionIDs = string.Join(",", ids);

            }

            return GetListDataAndReturn<Rec_JobConditionModel, Rec_JobConditionEntity, Rec_JobConditionByVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobConditionByVacancyId);

        }
        public ActionResult ExportJobConditionByVacancyList([DataSourceRequest] DataSourceRequest request, Rec_JobConditionByVacancySearchModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.JobConditionIDs))
            {
                var ids = model.JobConditionIDs
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Common.DotNetToOracle(x))
                        .ToList();
                model.JobConditionIDs = string.Join(",", ids);

            }
            return ExportAllAndReturn<Rec_JobConditionEntity, Rec_JobConditionModel, Rec_JobConditionByVacancySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobConditionByVacancyId);
        }
        [HttpPost]
        public ActionResult GetJobConditionList([DataSourceRequest] DataSourceRequest request, Rec_JobConditionSearchModel model)
        {
            return GetListDataAndReturn<Rec_JobConditionModel, Rec_JobConditionEntity, Rec_JobConditionSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobCondition);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportJobConditionList([DataSourceRequest] DataSourceRequest request, Rec_JobConditionSearchModel model)
        {
            return ExportAllAndReturn<Rec_JobConditionEntity, Rec_JobConditionModel, Rec_JobConditionSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_JobCondition);
        }


        [HttpPost]
        public ActionResult ExportJobConditionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_JobConditionEntity, Rec_JobConditionModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_JobConditionByIds);
        }
        [HttpPost]
        public ActionResult AddJobCavancy(Guid JobVacancyID, Guid ConditionID)
        {
            var service = new Rec_JobVacancyServices();
            string ConditionIDs = service.AddJobCavancy(JobVacancyID, ConditionID);
            return Json(ConditionIDs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddConditionIntoVacancy(Guid JobVacancyID, string ConditionIDs)
        {
            var service = new Rec_JobVacancyServices();
            string str = service.AddConditionIntoVacancy(JobVacancyID, ConditionIDs);
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteJobCavancy(Guid JobVacancyID, string ConditionIDs)
        {
            var service = new Rec_JobVacancyServices();
            ConditionIDs = service.DeleteJobCavancy(JobVacancyID, ConditionIDs);
            return Json(ConditionIDs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteConditionInGroupCondition(Guid GroupConditionID, string ConditionIDs)
        {
            var service = new Rec_GroupConditionServices();
            ConditionIDs = service.DeleteConditionInGroupCondition(GroupConditionID, ConditionIDs);
            return Json(ConditionIDs, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public ActionResult GetJobConditionIDs(Guid JobVacancyID)
        {
            var service = new Rec_JobVacancyServices();
            string ConditionIDs = ConditionIDs = service.GetJobConditionIDs(JobVacancyID);
            // kiểm tra nếu json thì rỗng thì trả về chữ "null" để nhận dạng trong ajax để tránh ko vào success
            if (ConditionIDs == null)
                ConditionIDs = "null";
            return Json(ConditionIDs, JsonRequestBehavior.AllowGet);
        }

        #region [Tho.Bui]: Rec_Interview

        [HttpPost]
        public ActionResult ComputeResultInterview(string selectedIds)
        {
            var service = new Rec_InterviewServices();
            var message = service.ComputeResultInterview(selectedIds, UserLogin);
            return Json(message);
        }

        [HttpPost]
        public ActionResult GetInterviewList([DataSourceRequest] DataSourceRequest request, Rec_InterviewSearchModel model)
        {
            return GetListDataAndReturn<Rec_InterviewModel, Rec_InterviewEntity, Rec_InterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Interview);

        }
        public ActionResult ExportInterviewListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_InterviewSearchModel model)
        {
            //return GetListDataAndReturn<Rec_InterviewModel, Rec_InterviewEntity, Rec_InterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Interview);

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_InterviewModel();
            var services = new BaseService();
            var result = GetListData<Rec_InterviewModel, Rec_InterviewEntity, Rec_InterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Interview, ref status);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_InterviewModel",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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
        /// <summary>
        /// get levelinterview and jobvancyID from Rec_interview by CandidateID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult GetCandidateID(Guid ID)
        {
            var result = new List<Rec_InterviewModel>();
            string status = string.Empty;
            if (ID != Guid.Empty)
            {
                var service = new Cat_ProvinceServices();
                result = service.GetData<Rec_InterviewModel>(ID, ConstantSql.hrm_rec_sp_get_CandidateId, UserLogin, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// get list interviewlist from Rec_Interview by candidateID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetInterviewListByCandidateID([DataSourceRequest] DataSourceRequest request, Guid ID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(ID);
            var result = baseService.GetData<Rec_InterviewModel>(objs, ConstantSql.hrm_rec_sp_get_InterviewByCandidateID, UserLogin, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportInterviewList([DataSourceRequest] DataSourceRequest request, Rec_InterviewSearchModel model)
        {
            return ExportAllAndReturn<Rec_InterviewEntity, Rec_InterviewModel, Rec_InterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_Interview);
        }


        [HttpPost]
        public ActionResult ExportInterviewSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_InterviewEntity, Rec_InterviewModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_InterviewByIds);
        }

        [HttpPost]
        //Get Level Inter view
        public JsonResult getLevelInterview(Guid? ID)
        {
            int? level = 0;
            if (ID == null)
            {
                return Json(level);
            }
            string status = string.Empty;
            var baseService = new BaseService();
            var levelByCandidate = baseService.GetData<Rec_RecruitmentHistoryEntity>(Common.DotNetToOracle(ID.ToString()), ConstantSql.hrm_rec_sp_get_RecHistoryByCandidateID, UserLogin, ref status).FirstOrDefault();
            if (levelByCandidate == null)
            {
                level = 1;
            }
            else
            {
                if (levelByCandidate.LevelInterview == null)
                {
                    level = 1;

                }
                else
                {
                    level = levelByCandidate.LevelInterview + 1;        
                }
            }
            return Json(level);
        }
        //Load ten yeu cau tuyen 
        [HttpPost]
        public JsonResult getJobInterview(Guid ID)
        {
            string JobVacancyName = string.Empty;
            string status = string.Empty;
            try
            {
                var baseService = new BaseService();
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(ID.ToString()));
                Guid? JobVacancyID = baseService.GetData<Rec_CandidateEntity>(objs, ConstantSql.hrm_rec_sp_get_CandidateById1, UserLogin, ref status).Select(s => s.JobVacancyID).FirstOrDefault();
                if (JobVacancyID != Guid.Empty && JobVacancyID != null)
                {
                    JobVacancyName = baseService.GetData<Rec_JobVacancyEntity>(JobVacancyID, ConstantSql.hrm_rec_sp_get_JobVacancyId1, UserLogin, ref status).Select(s => s.JobVacancyName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                string a = NotificationType.Error + "," + ex.Message;
            }
            return Json(JobVacancyName);
        }
        //lay candidateId
        [HttpPost]
        public JsonResult getCandidateId(Guid ID)
        {
            Guid? CandidateId = null;
            string status = string.Empty;
            try
            {
                var baseService = new BaseService();
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(ID.ToString()));
                CandidateId = baseService.GetData<Rec_InterviewCampaignDetailEntity>(objs, ConstantSql.hrm_rec_sp_get_CandidateByCdCpDetailId, UserLogin, ref status).Select(s => s.CandidateID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string a = NotificationType.Error + "," + ex.Message;
            }
            return Json(CandidateId);
        }
        //Get NameCandidate
        [HttpPost]
        public JsonResult getNameCandidate(Guid ID)
        {
            string CandidateName = null;
            string status = string.Empty;
            try
            {
                var baseService = new BaseService();
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(ID.ToString()));
                CandidateName = baseService.GetData<Rec_CandidateEntity>(objs, ConstantSql.hrm_rec_sp_get_CandidateById1, UserLogin, ref status).Select(s => s.CandidateName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string a = NotificationType.Error + "," + ex.Message;
            }
            return Json(CandidateName);
        }
        //Load ID yeu cau tuyen 
        [HttpPost]
        public JsonResult getJobIDInterview(Guid ID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(ID.ToString()));
            Guid? JobVacancyID = baseService.GetData<Rec_CandidateEntity>(objs, ConstantSql.hrm_rec_sp_get_CandidateById1, UserLogin, ref status).Select(s => s.JobVacancyID).FirstOrDefault();
            return Json(JobVacancyID);
        }
        //Load đối tượng Nhóm điều kiện
        [HttpPost]
        public JsonResult getGroupCondition(Guid ID)
        {
            IList<Rec_JobConditionEntity> ilistJobCondition = new List<Rec_JobConditionEntity>();
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(ID.ToString()));
            var GroupCondition = baseService.GetData<Rec_CandidateEntity>(objs, ConstantSql.hrm_rec_sp_get_GroupConditionByID, UserLogin, ref status).FirstOrDefault();
            List<Guid> ilist = null;
            if (GroupCondition != null && GroupCondition.JobConditionIDs != null)
            {
                ilist = GroupCondition.JobConditionIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            if (ilist != null)
            {
                foreach (Guid item in ilist)
                {
                    objs = new List<object>();
                    Rec_JobConditionEntity ObjJobCondition = new Rec_JobConditionEntity();
                    objs.Add(Common.DotNetToOracle(item.ToString()));
                    ObjJobCondition = baseService.GetData<Rec_JobConditionEntity>(objs, ConstantSql.hrm_rec_sp_get_JobConditionForInterView, UserLogin, ref status).FirstOrDefault();
                    if (ObjJobCondition != null)
                    {
                        ObjJobCondition.EnumTranslate = ObjJobCondition.ConditionName.TranslateString();
                        ilistJobCondition.Add(ObjJobCondition);
                    }
                }
            }
            return Json(ilistJobCondition);
        }
        #endregion

        #region Rec_RecruitmentHistory
        //Load RecruitmentHistoryID
        [HttpPost]
        public JsonResult getRecruitmentHistoryId(Guid ID)
        {
            string status = string.Empty;
            var RecruitmentHisService = new Rec_RecruitmentHistoryServices();
            Guid? RecruitmentHistoryId = RecruitmentHisService.GetData<Rec_RecruitmentHistoryEntity>(ID, ConstantSql.hrm_rec_sp_get_RecruitmentHistoryIdByCandidateId, UserLogin, ref status).OrderByDescending(s => s.DateApply).Select(s => s.ID).FirstOrDefault();
            return Json(RecruitmentHistoryId);
        }

        //load result by condition
        [HttpPost]
        public string getResultInterview(Guid ID, string str, double? score1, double? score2, double? score3, string candidateID)
        {
            string status = string.Empty;
            string result = HRM.Infrastructure.Utilities.Interview.E_PASS.ToString();
            string[] strCondition = str.Split('|').ToArray();
            if (str != "" && str != null)
            {
                strCondition = str.Substring(0, str.Length - 1).Split('|').ToArray();
            }

            //string status = HRM.Infrastructure.Utilities.Interview.E_PASS.ToString();
            var RecruitmentHisService = new Rec_RecruitmentHistoryServices();
            var RecJobconditionservice = new Rec_JobConditionServices();
            IList<Rec_JobConditionEntity> ilistJobCondition = new List<Rec_JobConditionEntity>();

            var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(ID.ToString()));
            var GroupCondition = baseService.GetData<Rec_CandidateEntity>(objs, ConstantSql.hrm_rec_sp_get_GroupConditionByID, UserLogin, ref status).FirstOrDefault();
            if (GroupCondition.JobConditionIDs == null)
            {
                return null;
            }
            List<Guid> ilist = GroupCondition.JobConditionIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
            string jobConditionIds = string.Empty;
            foreach (Guid item in ilist)
            {
                jobConditionIds += item;
                jobConditionIds += ",";
            }
            if (jobConditionIds.Length > 0)
            {
                jobConditionIds = jobConditionIds.Substring(0, jobConditionIds.Length - 1);
            }
            var lstJobConditions = baseService.GetData<Rec_JobConditionEntity>(Common.DotNetToOracle(jobConditionIds), ConstantSql.hrm_rec_sp_get_JobConditionByIds, UserLogin, ref status).ToList();
            var candidate = baseService.GetData<Rec_CandidateEntity>(Common.DotNetToOracle(candidateID), ConstantSql.hrm_rec_sp_get_CandidateByIds, UserLogin, ref status).FirstOrDefault();
            List<object> lstparadiseelist = new List<object>();
            lstparadiseelist.Add(null);
            lstparadiseelist.Add(EnumDropDown.EntityType.E_SICK_REC.ToString());
            lstparadiseelist.Add(1);
            lstparadiseelist.Add(int.MaxValue - 1);
            var lstsick = baseService.GetData<Cat_NameEntityEntity>(lstparadiseelist, ConstantSql.hrm_cat_sp_get_LevelGeneral, UserLogin, ref status).ToList();

            foreach (Guid item in ilist)
            {

                var ObjJobCondition = lstJobConditions.Where(s => s.ID == item).FirstOrDefault();
                if (ObjJobCondition != null)
                {
                    string strConditionName = ObjJobCondition.Value1.Trim();
                    foreach (char c in strConditionName)
                    {
                        if (!char.IsDigit(c))
                            continue;
                    }
                    int value1 = 0;
                    if (ObjJobCondition.ConditionName != "E_DISEASEIDS" && ObjJobCondition.ConditionName != null)
                    {
                        value1 = int.Parse(ObjJobCondition.Value1.Trim());
                    }

                    if (!string.IsNullOrEmpty(strCondition[0].ToString()))
                    {
                        foreach (var temp in strCondition)
                        {
                            string[] strtemp = temp.Split(',').ToArray();
                            if (strtemp != null && strtemp[1] != null)
                            {
                                int value2 = int.Parse(strtemp[1].ToString().Trim());
                                if (ObjJobCondition.ConditionName.Contains(strtemp[0].ToString()) && strtemp[0].ToString() != "E_SCORE1" && strtemp[0].ToString() != "E_SCORE2"
                                    && strtemp[0].ToString() != "E_SCORE3" && strtemp[0].ToString() != "E_DISEASEIDS")
                                {
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != value2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < value2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > value2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                }
                                if (ObjJobCondition.ConditionName.Contains(strtemp[0].ToString()) && strtemp[0].ToString() == "E_SCORE1")
                                {
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score1)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score1)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score1)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                }

                                if (ObjJobCondition.ConditionName.Contains(strtemp[0].ToString()) && strtemp[0].ToString() == "E_SCORE2")
                                {
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score2)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                }
                                if (ObjJobCondition.ConditionName.Contains(strtemp[0].ToString()) && strtemp[0].ToString() == "E_SCORE3")
                                {
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score3)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score3)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                    if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score3)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                        break;
                                    }
                                }
                                if (ObjJobCondition.ConditionName.Contains(strtemp[0].ToString()) && strtemp[0].ToString() == "E_DISEASEIDS")
                                {
                                    var lstDiseaseCondition = ObjJobCondition.Value1.Split(',').Select(x => x).ToList();
                                    var lstsickbycondition = lstsick.Where(s => lstDiseaseCondition.Contains(Common.DotNetToOracle(s.ID.ToString()))).ToList();
                                    var lstDiseseCandidate = candidate.DiseaseListIDs.Split(',').ToList();
                                    if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() == 0)
                                    {
                                        result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                                    }
                                }
                            }
                            if (strtemp != null && strtemp[1] == null && (score1 != null && score2 != null && score3 != null))
                            {

                            }
                        }
                    }
                    if (ObjJobCondition.ConditionName == ConditionName.E_SCORE1.ToString())
                    {
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score1)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score1)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score1)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                    }
                    if (ObjJobCondition.ConditionName == ConditionName.E_SCORE2.ToString())
                    {
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score2)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score2)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score2)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                    }
                    if (ObjJobCondition.ConditionName == ConditionName.E_SCORE3.ToString())
                    {
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && value1 != score3)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && value1 < score3)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                        if (ObjJobCondition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && value1 > score3)
                        {
                            result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            break;
                        }
                    }
                    if (ObjJobCondition.ConditionName == ConditionName.E_DISEASEIDS.ToString())
                    {
                        var lstDiseaseCondition = ObjJobCondition.Value1.Split(',').Select(x => x).ToList();
                        var lstsickbycondition = lstsick.Where(s => lstDiseaseCondition.Contains(Common.DotNetToOracle(s.ID.ToString()))).ToList();
                        if (candidate.DiseaseListIDs != null)
                        {
                            var lstDiseseCandidate = candidate.DiseaseListIDs.Split(',').ToList();
                            if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() >= 1)
                            {
                                result = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                            }
                        }

                    }
                }
                if (result == HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString())
                    break;
            }
            return result;
        }

        #endregion

        #region Rec_CandidateHistory
        /// <summary>
        /// get list candidatehistory by candidatehId from Rec_CandiateHistory
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// 
        //public ActionResult GetCandidateHistoryByCandidateId([DataSourceRequest] DataSourceRequest request, Guid ID)
        //{
        //    string status = string.Empty;
        //    var baseService = new BaseService();
        //    var objs = new List<object>();
        //    var result = baseService.GetData<Rec_CandidateHistoryModel>(objs, ConstantSql.hrm_rec_sp_get_CandidateHistoryByCandidateID, ref status);
        //    if (result != null)
        //        return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    return null;
        //}


        public ActionResult GetCandidateHistoryByCandidateID([DataSourceRequest] DataSourceRequest request, Guid? candidateID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var objs = new List<object>();
            var result = baseService.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(candidateID.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryByCandidateId, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Med_Disease
        [HttpPost]
        public JsonResult GetMultiDisease(string text)
        {
            return GetDataForControl<Med_DiseaseMultiModel, Med_DiseaseMultiEntity>(text, ConstantSql.hrm_rec_sp_get_JobVacancy_multi);
        }
        #endregion

        #region Rec_RecruitmentHistory
        public ActionResult GetRecruitmentHistoryList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentHistorySearchModel model)
        {
            return GetListDataAndReturn<Rec_RecruitmentHistoryModel, Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistorySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruimentHistoryByCandidateID);
        }
        public ActionResult ExportAllRecruitmentHistoryList([DataSourceRequest] DataSourceRequest request, Rec_RecruitmentHistorySearchModel model)
        {
            return ExportAllAndReturn<Rec_RecruitmentHistoryEntity, Rec_RecruitmentHistoryModel, Rec_RecruitmentHistorySearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RecruimentHistoryByCandidateID);
        }
        //[HttpPost]
        //public ActionResult GetJobConditionIDs(Guid JobVacancyID)
        //{
        //    var service = new Rec_JobVacancyServices();
        //    string ConditionIDs = service.GetJobConditionIDs(JobVacancyID);
        //    return Json(ConditionIDs, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult GetHistoryApprove(string HistoryApproves)
        {
            var service = new Rec_CandidateServices();
            //var rs = service.GetHistoryApprove(HistoryApprove);
            string strResult = HistoryApproves ?? string.Empty;
            List<HistoryApprove> listHistoryApprove = new List<HistoryApprove>();
            HistoryApprove historyApp = null;
            string[] arrHistory = strResult.Split(new char[] { ';' });
            foreach (string history in arrHistory)
            {
                int index__ = history.IndexOf("#");
                if (index__ > 0)
                {
                    historyApp = new HistoryApprove();
                    string[] arr = history.Split(new char[] { '#' });
                    if (arr.Length < 5)
                        continue;
                    historyApp.DateApprove = arr[0];
                    historyApp.UserApprove = arr[1];
                    historyApp.Status = EnumDropDown.GetEnumDescription(arr[2]);
                    //Dich Change
                    string[] _change = arr[3].Split(new char[] { '$' });
                    if (_change.Length == 2)
                        historyApp.Changes = EnumDropDown.GetEnumDescription(_change[0]) + " ==> " + EnumDropDown.GetEnumDescription(_change[1]);
                    historyApp.WaitUserApprove = arr[4];
                    listHistoryApprove.Add(historyApp);
                }
            }
            return Json(listHistoryApprove, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Rec_CandidateQualification
        public ActionResult GetCandidateQualificationByCandidateID([DataSourceRequest] DataSourceRequest request, Rec_CandidateQualificationSeachModel model)
        {
            return GetListDataAndReturn<Rec_CandidateQualificationModel, Rec_CandidateQualificationEntity, Rec_CandidateQualificationSeachModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateQualificationByCandidateID);
        }
        #endregion

        #region Rec_CandidateComputingLevel
        public ActionResult GetCandidateComputingLevelByCandidateID([DataSourceRequest] DataSourceRequest request, Rec_CandidateQualificationSeachModel model)
        {
            return GetListDataAndReturn<Rec_CandidateComputingLevelModel, Rec_CandidateComputingLevelEntity, Rec_CandidateQualificationSeachModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateComputingLevelByCandidateID);
        }
        #endregion

        #region Rec_CandidateLanguageLevel
        public ActionResult GetCandidateLanguageLevelByCandidateID([DataSourceRequest] DataSourceRequest request, Rec_CandidateQualificationSeachModel model)
        {
            return GetListDataAndReturn<Rec_CandidateLanguageLevelModel, Rec_CandidateLanguageLevelEntity, Rec_CandidateQualificationSeachModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateLanguageLevelByCandidateID);
        }
        #endregion

        #region Rec_CandidateBusiness

        [HttpPost]
        public ActionResult GetCandidateBusinessByCandidateList([DataSourceRequest] DataSourceRequest request, Rec_CandidateBusinessByCandidateSearchModel model)
        {
            return GetListDataAndReturn<Rec_CandidateBusinessModel, Rec_CandidateBusinessEntity, Rec_CandidateBusinessByCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateBusinessByCandidateId);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportCandidateBusinessByCandidateList([DataSourceRequest] DataSourceRequest request, Rec_CandidateBusinessByCandidateSearchModel model)
        {
            return ExportAllAndReturn<Rec_CandidateBusinessEntity, Rec_CandidateBusinessModel, Rec_CandidateBusinessByCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_CandidateBusinessByCandidateId);
        }


        [HttpPost]
        public ActionResult ExportCandidateBusinessSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_CandidateBusinessEntity, Rec_CandidateBusinessModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_CandidateBusinessByIds);
        }

        #endregion

        #region Rec_Relative

        [HttpPost]
        public ActionResult GetRelativeByCandidateList([DataSourceRequest] DataSourceRequest request, Rec_RelativeByCandidateSearchModel model)
        {
            return GetListDataAndReturn<Rec_RelativeModel, Rec_RelativeEntity, Rec_RelativeByCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RelativeByCandidateId);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportRelativeByCandidateList([DataSourceRequest] DataSourceRequest request, Rec_RelativeByCandidateSearchModel model)
        {
            return ExportAllAndReturn<Rec_RelativeEntity, Rec_RelativeModel, Rec_RelativeByCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_RelativeByCandidateId);
        }


        [HttpPost]
        public ActionResult ExportRelativeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_RelativeEntity, Rec_RelativeModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_RelativeByIds);
        }

        #endregion

        #region Rec_InterviewCampaign

        public ActionResult ExportAllInterviewCampaignList([DataSourceRequest] DataSourceRequest request, Rec_InterviewCampaignSearchModel model)
        {
            return ExportAllAndReturn<Rec_InterviewCampaignEntity, Rec_InterviewCampaignModel, Rec_InterviewCampaignSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_InterviewCampaign);
        }

        public ActionResult ExportInterviewCampaignSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_InterviewCampaignEntity, Rec_InterviewCampaignModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_InterviewCampaignByIds);
        }

        public ActionResult GetInterviewCampaignList([DataSourceRequest] DataSourceRequest request, Rec_InterviewCampaignSearchModel model)
        {
            return GetListDataAndReturn<Rec_InterviewCampaignModel, Rec_InterviewCampaignEntity, Rec_InterviewCampaignSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_InterviewCampaign);
        }

        public JsonResult GetMultiRec_InterviewCampaign(string text)
        {
            return GetDataForControl<Rec_InterviewCampaignModel, Rec_InterviewCampaignEntity>(text, ConstantSql.hrm_rec_sp_get_InterviewCampaign_multi);
        }

        #endregion

        #region Rec_EnrolledCandidates
        public ActionResult GetEnrolledCandidates([DataSourceRequest] DataSourceRequest request, Rec_EnrolledCandidateSearchModel model)
        {
            // return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_EnrolledCandidates);

            string status = string.Empty;
            var service = new BaseService();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<Rec_CandidateEntity>(lstModel, ConstantSql.hrm_rec_sp_get_EnrolledCandidates, UserLogin, ref status);
            var listModel = new List<Rec_CandidateModel>();
            if (listEntity != null)
            {
                var ObjResultInterview = new List<object>();
                ObjResultInterview.AddRange(new object[24]);
                ObjResultInterview[22] = 1;
                ObjResultInterview[23] = int.MaxValue - 1;
                var lstResultInterview = service.GetData<Rec_InterviewEntity>(ObjResultInterview, ConstantSql.hrm_rec_sp_get_Interview, UserLogin, ref status).ToList();
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    var resultInterviewByCandidate = lstResultInterview.Where(s => s.CandidateID == item.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (resultInterviewByCandidate != null)
                    {
                        item.Score1 = resultInterviewByCandidate.Score1;
                        item.Score2 = resultInterviewByCandidate.Score2;
                        item.Score3 = resultInterviewByCandidate.Score3;
                        item.ResultInterview = resultInterviewByCandidate.ResultInterviewView;
                    }

                    var newModle = (Rec_CandidateModel)typeof(Rec_CandidateModel).CreateInstance();
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
            var listModelNull = new List<Rec_CandidateModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public ActionResult ExportEnrolledCandidateListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_EnrolledCandidateSearchModel model)
        {
            var actionservices = new ActionService(UserLogin);
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_CandidateModel();
            var result = GetListData<Rec_CandidateModel, Rec_CandidateEntity, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_EnrolledCandidates, ref status);
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_CandidateModel",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };

                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var objInterview = new List<object>();
            objInterview.AddRange(new object[2]);
            objInterview[0] = 1;
            objInterview[1] = int.MaxValue - 1;
            var lstInterView = actionservices.GetData<Rec_InterviewModel>(objInterview, ConstantSql.hrm_rec_sp_get_InterviewDataReport, ref status).ToList();

            if (model.ExportId != Guid.Empty)
            {
                foreach (var item in result)
                {
                    var dataLevel1 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 1).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel2 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 2).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel3 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 3).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel4 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 4).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel5 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 5).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (dataLevel1 != null)
                    {
                        item.Score1_1 = dataLevel1.Score1;
                        item.Score1_2 = dataLevel1.Score2;
                        item.Score1_3 = dataLevel1.Score3;
                        item.KQ1 = dataLevel1.ResultInterviewView;
                        item.LanguageCode1 = dataLevel1.LanguageCode;
                        item.DateInterview1 = dataLevel1.DateInterview;
                    }
                    if (dataLevel2 != null)
                    {
                        item.Score2_1 = dataLevel2.Score1;
                        item.Score2_2 = dataLevel2.Score2;
                        item.Score2_3 = dataLevel2.Score3;
                        item.KQ2 = dataLevel2.ResultInterviewView;
                        item.LanguageCode2 = dataLevel2.LanguageCode;
                        item.DateInterview2 = dataLevel2.DateInterview;
                    }
                    if (dataLevel3 != null)
                    {
                        item.Score3_1 = dataLevel3.Score1;
                        item.Score3_2 = dataLevel3.Score2;
                        item.Score3_3 = dataLevel3.Score3;
                        item.KQ3 = dataLevel3.ResultInterviewView;
                        item.LanguageCode3 = dataLevel3.LanguageCode;
                        item.DateInterview3 = dataLevel3.DateInterview;
                    }
                    if (dataLevel4 != null)
                    {
                        item.Score4_1 = dataLevel4.Score1;
                        item.Score4_2 = dataLevel4.Score2;
                        item.Score4_3 = dataLevel4.Score3;
                        item.KQ4 = dataLevel4.ResultInterviewView;
                        item.LanguageCode4 = dataLevel4.LanguageCode;
                        item.DateInterview4 = dataLevel4.DateInterview;
                    }
                    if (dataLevel5 != null)
                    {
                        item.Score5_1 = dataLevel5.Score1;
                        item.Score5_2 = dataLevel5.Score2;
                        item.Score5_3 = dataLevel5.Score3;
                        item.KQ5 = dataLevel5.ResultInterviewView;
                        item.LanguageCode5 = dataLevel5.LanguageCode;
                        item.DateInterview5 = dataLevel5.DateInterview;
                    }
                    var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                    return Json(fullPath);
                }
            }

            return Json(result.ToDataSourceResult(request));
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllEnrolledCanList([DataSourceRequest] DataSourceRequest request, Rec_EnrolledCandidateSearchModel model)
        {
            //return ExportAllAndReturn<Rec_CandidateEntity, Rec_CandidateModel, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_EnrolledCandidates);
            model.IsExport = true;
            model.ValueFields = model.ValueFields.Replace("Gender", "udGender");
            model.ValueFields = model.ValueFields.Replace("Status", "udStatus");
            string fullPath = string.Empty, status = string.Empty;

            var listModel = GetListData<Rec_CandidateEntity, Rec_CandidateModel, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_EnrolledCandidates, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
        }
        public ActionResult ExportEnrolledCanSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_CandidateEntity, Rec_CandidateModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_EnrolledCanIds);
        }

        public ActionResult ExportWordEnrolledCandidateByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Rec_CandidateModel();
            var baseService = new BaseService();

            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
            {
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            }
            var lstCandidate = baseService.GetData<Rec_CandidateEntity>(strIDs, ConstantSql.hrm_rec_sp_get_EnrolledCanIds, UserLogin, ref status);

            if (lstCandidate == null)
                return null;
            int i = 0;
            var lstCandidateID = lstCandidate.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstCandidateID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var candidate in lstCandidate)
            {
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
                //template = exportService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();

                if (!string.IsNullOrEmpty(valueFields))
                {
                    template = exportService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, UserLogin, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(candidate.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(candidate.CandidateName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var profile = service.GetByIdUseStore<Hre_ProfileEntity>(candidate.ID, ConstantSql.hrm_hr_sp_get_ProfileByCandidateID, ref status);
                if (profile != null)
                {
                    var liscontract = baseService.GetData<Hre_ContractEntity>(profile.ID, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, UserLogin, ref status);
                    if (liscontract.Count > 0)
                    {
                        liscontract = liscontract.OrderBy(m => m.DateSigned).ToList();
                        var contract = liscontract[0];
                        candidate.Salary = contract.Salary;
                        candidate.ContractNo = contract.ContractNo;
                        candidate.DateApplyContract = contract.DateStart;
                        candidate.DateSignedConstract = contract.DateSigned;
                        candidate.Currency = contract.CurenncyInsName;
                    }
                }
                var lstcontract = new List<Rec_CandidateEntity>();
                lstcontract.Add(candidate);
                ExportService.ExportWord(outputPath, templatepath, lstcontract);
            }
            if (lstCandidateID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }




        /// Cập nhật Status và Reasondeny khi chọn nhiều ứng viên trên lưới
        [HttpPost]
        public ActionResult UpdateReasonDenny([Bind]Rec_CandidateModel model)
        {
            IList<Rec_CandidateEntity> list = new List<Rec_CandidateEntity>();
            if (!string.IsNullOrEmpty(model.listId))
            {
                List<Guid> lisIDs = model.listId.Split(',').Select(x => Guid.Parse(x)).ToList();
                Rec_CandidateEntity ObjProfile = null;
                Rec_CandidateServices CandidateService = new Rec_CandidateServices();
                foreach (Guid item in lisIDs)
                {
                    string status = string.Empty;
                    ObjProfile = new Rec_CandidateEntity();
                    var ResultProfile = CandidateService.GetData<Rec_CandidateEntity>(item, ConstantSql.hrm_rec_sp_get_CandidateById, UserLogin, ref status).FirstOrDefault();
                    ObjProfile = ResultProfile;
                    ObjProfile.StatusHire = HRM.Infrastructure.Utilities.ProfileStatusSyn.E_UNHIRE.ToString();
                    ObjProfile.ReasonDeny = model.ReasonDeny;
                    //ObjProfile.StatusSyn = HRM.Infrastructure.Utilities.ProfileStatusSyn.E_UNHIRE.ToString();
                    CandidateService.Edit(ObjProfile);
                    list.Add(ObjProfile);
                }
            }
            return Json(list);
        }
        #endregion

        #region [Tho.Bui] Rec_InterviewCampaignDetail

        public ActionResult GetIntervieCampaignDetailValidate(Rec_InterviewCampaignDetailSearchModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_InterviewCampaignDetailSearchModel>(Model, "Rec_InterviewCampaignDetail", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        [HttpPost]
        public ActionResult GetIntervieCampaignDetailList([DataSourceRequest] DataSourceRequest request, Rec_InterviewCampaignDetailSearchModel model)
        {
            List<Rec_InterviewCampaignDetailModel> resultInterviewCampaignDetail = new List<Rec_InterviewCampaignDetailModel>();

            string status = string.Empty;
            var baseService = new BaseService();
            var actionService = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<object> objshis = new List<object>();

            var InterviewServices = new Rec_InterviewCampaignDetailServices();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            int? level = (model != null && model.LevelInterview != 0) ? model.LevelInterview : null;
            List<object> lstModels = new List<object>();
            lstModels.AddRange(new object[16]);
            lstModels[0] = model.CandidateName;
            lstModels[1] = model.DateFrom;
            lstModels[2] = model.DateTo;
            lstModels[3] = model.CodeCandidate;
            lstModels[4] = model.RankID;
            lstModels[5] = model.OrgStructureID;
            lstModels[6] = model.JobVacancyID;
            lstModels[7] = model.PositionID;
            lstModels[8] = level;
            lstModels[13] = model.WorkPlaceID;
            lstModels[14] = 1;
            lstModels[15] = Int32.MaxValue - 1;

            var result = actionService.GetData<Rec_InterviewCampaignDetailModel>(lstModels, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail, ref status);
            #region Load Chi tiet ke hoach chua phong van
            if (result != null)
            {
                var InTerCamDetaiServices = new Rec_InterviewCampaignDetailServices();
                var HistoryServices = new Rec_RecruitmentHistoryServices();
                var lstCandidateIDS = result.Select(s => s.CandidateID).Distinct().ToList();

                var lstCandidateHistory = new List<Rec_RecruitmentHistoryEntity>();
                int _total = lstCandidateIDS.Count;
                int _totalPage = _total / 100 + 1;
                int _pageSize = 100;
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = lstCandidateIDS.Skip(_skip).Take(_pageSize).ToList();
                    string _strselectedIDs = Common.DotNetToOracle(string.Join(",", _listCurrenPage));

                    var lstRecruitmentHistory = baseService.GetData<Rec_RecruitmentHistoryEntity>(_strselectedIDs, ConstantSql.hrm_rec_sp_get_RecHisByListCandidateID, UserLogin, ref status).ToList();
                    if (lstRecruitmentHistory != null && lstRecruitmentHistory.Count > 0)
                    {
                        lstCandidateHistory.AddRange(lstRecruitmentHistory);
                    }
                }

                foreach (var item in result)
                {
                    bool IsAdd = false;
                    //var IlInterviewcampaugnDetail = result.Where(s => s.CandidateID == item.CandidateID).ToList();
                    var ObjHisCandidate = lstCandidateHistory.Where(s => s.ID == item.RecruitmentHistoryID).FirstOrDefault();
                    if (ObjHisCandidate == null)
                    {
                        continue;
                    }
                    if (item.LevelInterview == null)
                    {
                        IsAdd = true;
                    }

                    Rec_JobVacancyEntity entityJobVacancy = null;
                    if (ObjHisCandidate != null && ObjHisCandidate.JobVacancyID != Guid.Empty && ObjHisCandidate.JobVacancyID != null)
                    {
                        entityJobVacancy = actionService.GetByIdUseStore<Rec_JobVacancyEntity>(ObjHisCandidate.JobVacancyID.Value, ConstantSql.hrm_rec_sp_get_JobVacancyId, ref status);
                    }
                    if (entityJobVacancy != null)
                    {
                        if (item.LevelInterview != null && item.LevelInterview == entityJobVacancy.NoLevelInterview)
                        {
                            continue;
                        }
                    }
                    if (item.LevelInterview == null && (ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_PASS.ToString()
                     && ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString()
                        ))
                    {
                        if (ObjHisCandidate != null)
                        {
                            if (ObjHisCandidate.LevelInterview == null)
                            {
                                item.LevelInterview = 1;
                            }
                            else
                            {
                                item.LevelInterview = ObjHisCandidate.LevelInterview + 1;
                            }

                            if (IsAdd == true)
                            {
                                resultInterviewCampaignDetail.Add(item);
                            }
                        }
                    }
                }
            }
            resultInterviewCampaignDetail = resultInterviewCampaignDetail.Distinct().ToList();
            #endregion

            var isDataTable = false;
            object obj = new Rec_InterviewCampaignDetailModel();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = resultInterviewCampaignDetail;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_InterviewCampaignDetailModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var objInterview = new List<object>();
            objInterview.AddRange(new object[2]);
            objInterview[0] = 1;
            objInterview[1] = int.MaxValue - 1;
            var lstInterView = baseService.GetData<Rec_InterviewModel>(objInterview, ConstantSql.hrm_rec_sp_get_InterviewDataReport, UserLogin, ref status).ToList();


            if (model.ExportId != Guid.Empty)
            {
                foreach (var item in resultInterviewCampaignDetail)
                {
                    var dataLevel1 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 1).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel2 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 2).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel3 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 3).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel4 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 4).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel5 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 5).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (dataLevel1 != null)
                    {
                        item.Score1_1 = dataLevel1.Score1;
                        item.Score1_2 = dataLevel1.Score2;
                        item.Score1_3 = dataLevel1.Score3;
                        item.KQ1 = dataLevel1.ResultInterviewView;
                        item.LanguageCode1 = dataLevel1.LanguageCode;
                        item.DateInterview1 = dataLevel1.DateInterview;
                    }
                    if (dataLevel2 != null)
                    {
                        item.Score2_1 = dataLevel2.Score1;
                        item.Score2_2 = dataLevel2.Score2;
                        item.Score2_3 = dataLevel2.Score3;
                        item.KQ2 = dataLevel2.ResultInterviewView;
                        item.LanguageCode2 = dataLevel2.LanguageCode;
                        item.DateInterview2 = dataLevel2.DateInterview;
                    }
                    if (dataLevel3 != null)
                    {
                        item.Score3_1 = dataLevel3.Score1;
                        item.Score3_2 = dataLevel3.Score2;
                        item.Score3_3 = dataLevel3.Score3;
                        item.KQ3 = dataLevel3.ResultInterviewView;
                        item.LanguageCode3 = dataLevel3.LanguageCode;
                        item.DateInterview3 = dataLevel3.DateInterview;
                    }
                    if (dataLevel4 != null)
                    {
                        item.Score4_1 = dataLevel4.Score1;
                        item.Score4_2 = dataLevel4.Score2;
                        item.Score4_3 = dataLevel4.Score3;
                        item.KQ4 = dataLevel4.ResultInterviewView;
                        item.LanguageCode4 = dataLevel4.LanguageCode;
                        item.DateInterview4 = dataLevel4.DateInterview;
                    }
                    if (dataLevel5 != null)
                    {
                        item.Score5_1 = dataLevel5.Score1;
                        item.Score5_2 = dataLevel5.Score2;
                        item.Score5_3 = dataLevel5.Score3;
                        item.KQ5 = dataLevel5.ResultInterviewView;
                        item.LanguageCode5 = dataLevel5.LanguageCode;
                        item.DateInterview5 = dataLevel5.DateInterview;
                    }
                }
                var fullPath = ExportService.Export(model.ExportId, resultInterviewCampaignDetail, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            // return Json(resultInterviewCampaignDetail.ToDataSourceResult(request));
            return Json(resultInterviewCampaignDetail.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //var result = GetListDataAndReturn<Rec_InterviewCampaignDetailModel, Rec_InterviewCampaignDetailEntity, Rec_InterviewCampaignDetailSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail);
            //return result;
        }

        public ActionResult ExportAllInterviewDetailListNew([DataSourceRequest] DataSourceRequest request, Rec_InterviewCampaignDetailSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            ActionService service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            List<object> objshis = new List<object>();
            List<Rec_InterviewCampaignDetailModel> resultInterviewCampaignDetail = new List<Rec_InterviewCampaignDetailModel>();
            var InterviewServices = new Rec_InterviewCampaignDetailServices();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = baseService.GetData<Rec_InterviewCampaignDetailModel>(lstModel, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetail, UserLogin, ref status);
            if (result != null)
            {
                var InTerCamDetaiServices = new Rec_InterviewCampaignDetailServices();
                var HistoryServices = new Rec_RecruitmentHistoryServices();
                var lstCandidateIDS = result.Select(s => s.CandidateID).Distinct().ToList();
                //string temp = string.Empty;
                //foreach (Guid item in lstCandidateIDS)
                //{
                //    temp += item;
                //    temp += ",";
                //}
                //if (temp.Length > 0)
                //{
                //    temp = temp.Substring(0, temp.Length - 1);
                //}

               // var lstCandidateHistory = HistoryServices.GetData<Rec_RecruitmentHistoryEntity>(Common.DotNetToOracle(temp), ConstantSql.hrm_rec_sp_get_RecHisByListCandidateID, UserLogin, ref status).ToList();
                var lstCandidateHistory = new List<Rec_RecruitmentHistoryEntity>();
                int _total = lstCandidateIDS.Count;
                int _totalPage = _total / 100 + 1;
                int _pageSize = 100;
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = lstCandidateIDS.Skip(_skip).Take(_pageSize).ToList();
                    string _strselectedIDs = Common.DotNetToOracle(string.Join(",", _listCurrenPage));

                    var lstRecruitmentHistory = baseService.GetData<Rec_RecruitmentHistoryEntity>(_strselectedIDs, ConstantSql.hrm_rec_sp_get_RecHisByListCandidateID, UserLogin, ref status).ToList();
                    if (lstRecruitmentHistory != null && lstRecruitmentHistory.Count > 0)
                    {
                        lstCandidateHistory.AddRange(lstRecruitmentHistory);
                    }
                }
                foreach (var item in result)
                {
                    //var IlInterviewcampaugnDetail = result.Where(s => s.CandidateID == item.CandidateID).ToList();
                    var ObjHisCandidate = lstCandidateHistory.Where(s => s.ID == item.RecruitmentHistoryID).FirstOrDefault();
                    if (ObjHisCandidate == null)
                    {
                        continue;
                    }
                    Rec_JobVacancyEntity entityJobVacancy = null;
                    if (ObjHisCandidate != null && ObjHisCandidate.JobVacancyID != Guid.Empty && ObjHisCandidate.JobVacancyID != null)
                    {
                        entityJobVacancy = service.GetByIdUseStore<Rec_JobVacancyEntity>(ObjHisCandidate.JobVacancyID.Value, ConstantSql.hrm_rec_sp_get_JobVacancyId, ref status);
                    }
                    if (entityJobVacancy != null)
                    {
                        if (item.LevelInterview != null && item.LevelInterview == entityJobVacancy.NoLevelInterview)
                        {
                            continue;
                        }
                    }
                    if (item.LevelInterview == null && (ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_PASS.ToString()
                     && ObjHisCandidate.Status != HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString()
                        ))
                    {
                        if (ObjHisCandidate != null)
                        {
                            if (ObjHisCandidate.LevelInterview == null)
                            {
                                item.LevelInterview = 1;
                                resultInterviewCampaignDetail.Add(item);
                            }
                            else
                            {
                                item.LevelInterview = ObjHisCandidate.LevelInterview + 1;
                                resultInterviewCampaignDetail.Add(item);
                            }
                        }
                    }
                }
            }
            resultInterviewCampaignDetail = resultInterviewCampaignDetail.Distinct().ToList();
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(resultInterviewCampaignDetail, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status);

        }
        public ActionResult ExportAllInterviewDetailList([DataSourceRequest] DataSourceRequest request, Rec_WaitingInterviewSearchModel model)
        {
            return ExportAllAndReturn<Rec_CandidateEntity, Rec_CandidateModel, Rec_WaitingInterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_WaitingInterviewList);
        }
        public ActionResult GetInCamDetailByCandidateId([DataSourceRequest] DataSourceRequest request, string selectedIds)
        {
            string status = string.Empty;
            var baseService = new BaseService();

            var objs1 = new List<object>();
            objs1.Add(Common.DotNetToOracle(selectedIds));
            objs1.Add(1);
            objs1.Add(10000);
            List<Guid> CandidateIds = baseService.GetData<Rec_RecruitmentHistoryModel>(objs1, ConstantSql.hrm_hr_sp_get_RecHistoryByMoreIds, UserLogin, ref status).Select(s => (s.CandidateID)).ToList();
            string temp = string.Empty;
            foreach (Guid item in CandidateIds)
            {
                temp += item;
                temp += ",";
            }
            if (temp.Length > 0)
            {
                temp = temp.Substring(0, temp.Length - 1);
            }
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(temp));
            objs.Add(1);
            objs.Add(10000);
            var result = baseService.GetData<Rec_InterviewCampaignDetailModel>(objs, ConstantSql.hrm_rec_sp_get_InCamDetailByCddId, UserLogin, ref status);
            // result = result.Where(s => s.LevelInterView == null).ToList();
            if (result.Count == 0)
            {
                var resultcandidate = baseService.GetData<Rec_CandidateModel>(objs, ConstantSql.hrm_hr_sp_get_CandidateByMoreIds, UserLogin, ref status);
                if (resultcandidate != null)
                {
                    return Json(resultcandidate.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        #endregion

        #region
        public ActionResult GetFailCandidates([DataSourceRequest] DataSourceRequest request, Rec_FailCandidateSearchModel model)
        {
            return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_FailCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_FailCandidates);

        }
        public ActionResult ExportFailCandidatesListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_FailCandidateSearchModel model)
        {
            //return GetListDataAndReturn<Rec_CandidateModel, Rec_CandidateEntity, Rec_EnrolledCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_FailCandidates);

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_CandidateModel();
            var services = new BaseService();
            var result = GetListData<Rec_CandidateModel, Rec_CandidateEntity, Rec_FailCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_FailCandidates, ref status);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_CandidateModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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
        public ActionResult ExportAllFailCandidateslList([DataSourceRequest] DataSourceRequest request, Rec_FailCandidateSearchModel model)
        {
            string status = string.Empty;
            var listModel = GetListData<Rec_CandidateEntity, Rec_CandidateModel, Rec_FailCandidateSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_FailCandidates, ref status);
            status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            return Json(status);
        }

        #endregion

        #region Rec_GroupCondition
        [HttpPost]
        public ActionResult GetJobConditionByGroupConditionId([DataSourceRequest] DataSourceRequest request, Rec_JobConditionByGroupConditionSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var result = baseService.GetData<Rec_JobConditionModel>(Common.DotNetToOracle(model.JobConditionIDs), ConstantSql.hrm_rec_sp_get_JobConditionByIds, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetGroupConditionList([DataSourceRequest] DataSourceRequest request, Rec_GroupConditionSearchModel model)
        {
            return GetListDataAndReturn<Rec_GroupConditionModel, Rec_GroupConditionEntity, Rec_GroupConditionSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_GroupCondition);

        }

        [HttpPost]
        public ActionResult AddConditionIntoGroupCondition(Guid GroupConditionID, string ConditionIDs)
        {
            var service = new Rec_GroupConditionServices();
            string str = service.AddConditionIntoGroupCondition(GroupConditionID, ConditionIDs);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMultiGroupCondition(string text)
        {
            return GetDataForControl<Rec_GroupConditionMultiModel, Rec_GroupConditionMultiEntity>(text, ConstantSql.hrm_cat_sp_get_GroupCondition_Multi);
        }

        public ActionResult ExportGroupConditionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Rec_GroupConditionEntity, Rec_GroupConditionModel>(selectedIds, valueFields, ConstantSql.hrm_rec_sp_get_GroupConditionByIds);
        }

        public ActionResult ExportAllGroupConditionsList([DataSourceRequest] DataSourceRequest request, Rec_GroupConditionSearchModel model)
        {
            return ExportAllAndReturn<Rec_GroupConditionModel, Rec_GroupConditionEntity, Rec_GroupConditionSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_GroupCondition);
        }
        #endregion

        #region Rec_Report

        /// [Tho.Bui] 
        [HttpPost]
        public ActionResult GetReportJobVacancy([DataSourceRequest] DataSourceRequest request, Rec_ReportJobVacancySearchModel model)
        {
            #region Validate
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Rec_ReportJobVacancySearchModel>(model, "Rec_ReportJobVacancySearch", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion

            var JobVacancyServices = new Rec_JobVacancyServices();
            List<object> listObj = new List<object>();

            listObj.Add(model.OrgStructureID);
            listObj.Add(model.DateStart);
            listObj.Add(model.DateEnd);
            listObj.Add(model.RankID);
            listObj.Add(model.Type);
            string status = string.Empty;
            var result = JobVacancyServices.GetData<Rec_JobVacancyEntity>(listObj, ConstantSql.hrm_rec_sp_get_ReportJobVacancy, UserLogin, ref status).ToList().Translate<Rec_JobVacancyModel>();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Rec_JobVacancyModel(),
                    FileName = "Rec_ReportJobVacancy",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult ApprovedCandidate(string selectedIds)
        {
            var service = new Rec_CandidateServices();
            var message = service.ActionApprovedCandidate(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult CancelCandidate(string selectedIds)
        {
            var service = new Rec_CandidateServices();
            var message = service.ActionCancelCandidate(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult OutOfBlackList(string selectedIds)
        {
            var service = new Rec_CandidateServices();
            var message = service.OutOfBlackList(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult BackToInterview(string selectedIds)
        {
            var service = new Rec_CandidateServices();
            var message = service.BackToInterview(selectedIds);
            return Json(message);
        }

        // Quay lại DS hồ sơ trúng tuyển
        [HttpPost]
        public ActionResult ComeBackToCandidate(string selectedIds)
        {
            var service = new Rec_CandidateServices();
            var message = service.CombackToCandidate(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult AddToBlackListCandidate([Bind]Rec_CandidateModel model)
        {
            var service = new Rec_CandidateServices();
            var message = service.ActionBlackListCandidate(model.lstprofileid, model.ReasonBlackListID);
            return Json(message);
        }
        #endregion

        #region Rec_ReportInterview
        public ActionResult GetReportInterviewList([DataSourceRequest] DataSourceRequest request, Rec_ReportInterviewSearchModel model)
        {
            return GetListDataAndReturn<Rec_ReportInterviewModel, Rec_ReportInterviewEntity, Rec_ReportInterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_ReportInterview);
        }

        public ActionResult GetExportReportInterviewListByTemplate([DataSourceRequest] DataSourceRequest request, Rec_ReportInterviewSearchModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateExamFrom", Value = model.DateExamFrom == null ? DateTime.Now : model.DateExamFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateExamTo", Value = model.DateExamTo == null ? DateTime.Now : model.DateExamTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Rec_ReportInterviewModel();
            var services = new BaseService();
            var result = GetListData<Rec_ReportInterviewModel, Rec_ReportInterviewEntity, Rec_ReportInterviewSearchModel>(request, model, ConstantSql.hrm_rec_sp_get_ReportInterview, ref status);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rec_ReportInterviewModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
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


        #region Rec_ReportListCandidate
        [HttpPost]
        public ActionResult GetReportListCandidate([DataSourceRequest] DataSourceRequest request, Rec_ReportListCandidateModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Rec_ReportListCandidateModel>(model, "Rec_ReportListCandidateModel", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            var baseService = new BaseService();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateExamFrom", Value = model.DateExamFrom == null ? DateTime.Now : model.DateExamFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateExamTo", Value = model.DateExamTo == null ? DateTime.Now : model.DateExamTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            var JobVacancyServices = new Rec_CandidateServices();
            List<object> listObj = new List<object>();
            listObj.Add(model.CodeCandidate);
            listObj.Add(model.CandidateName);
            listObj.Add(model.Gender);
            listObj.Add(model.PositionID);
            listObj.Add(model.RankID);
            listObj.Add(model.SourceAdsID);
            listObj.Add(model.Type);
            listObj.Add(model.DateExamFrom);
            listObj.Add(model.DateExamTo);
            listObj.Add(1);
            listObj.Add(int.MaxValue - 1);
            string status = string.Empty;
            var result = JobVacancyServices.GetData<Rec_ReportListCandidateEntity>(listObj, ConstantSql.hrm_rec_sp_get_ReportListCandidate, UserLogin, ref status).ToList().Translate<Rec_ReportListCandidateModel>();

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Rec_ReportListCandidateModel(),
                    FileName = "Rec_ReportListCandidate",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var objInterview = new List<object>();
            objInterview.AddRange(new object[2]);
            objInterview[0] = 1;
            objInterview[1] = int.MaxValue - 1;
            var lstInterView = baseService.GetData<Rec_InterviewModel>(objInterview, ConstantSql.hrm_rec_sp_get_InterviewDataReport, UserLogin, ref status).ToList();

            foreach (var item in result)
            {
                if (lstInterView.Count > 0)
                {
                    var dataLevel1 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 1).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel2 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 2).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel3 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 3).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel4 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 4).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var dataLevel5 = lstInterView.Where(s => s.CandidateID == item.ID && s.LevelInterview == 5).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (dataLevel1 != null)
                    {
                        item.Score1_1 = dataLevel1.Score1;
                        item.Score1_2 = dataLevel1.Score2;
                        item.Score1_3 = dataLevel1.Score3;
                        item.KQ1 = dataLevel1.ResultInterviewView;
                        item.LanguageCode1 = dataLevel1.LanguageCode;
                        item.DateInterview1 = dataLevel1.DateInterview;
                    }
                    if (dataLevel2 != null)
                    {
                        item.Score2_1 = dataLevel2.Score1;
                        item.Score2_2 = dataLevel2.Score2;
                        item.Score2_3 = dataLevel2.Score3;
                        item.KQ2 = dataLevel2.ResultInterviewView;
                        item.LanguageCode2 = dataLevel2.LanguageCode;
                        item.DateInterview2 = dataLevel2.DateInterview;
                    }
                    if (dataLevel3 != null)
                    {
                        item.Score3_1 = dataLevel3.Score1;
                        item.Score3_2 = dataLevel3.Score2;
                        item.Score3_3 = dataLevel3.Score3;
                        item.KQ3 = dataLevel3.ResultInterviewView;
                        item.LanguageCode3 = dataLevel3.LanguageCode;
                        item.DateInterview3 = dataLevel3.DateInterview;
                    }
                    if (dataLevel4 != null)
                    {
                        item.Score4_1 = dataLevel4.Score1;
                        item.Score4_2 = dataLevel4.Score2;
                        item.Score4_3 = dataLevel4.Score3;
                        item.KQ4 = dataLevel4.ResultInterviewView;
                        item.LanguageCode4 = dataLevel4.LanguageCode;
                        item.DateInterview4 = dataLevel4.DateInterview;
                    }
                    if (dataLevel5 != null)
                    {
                        item.Score5_1 = dataLevel5.Score1;
                        item.Score5_2 = dataLevel5.Score2;
                        item.Score5_3 = dataLevel5.Score3;
                        item.KQ5 = dataLevel5.ResultInterviewView;
                        item.LanguageCode5 = dataLevel5.LanguageCode;
                        item.DateInterview5 = dataLevel5.DateInterview;
                    }
                }
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ExportJobVacancyByTemplate(List<Guid> selectedIds, string valueFields)
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
            var lstJobVacancy = baseService.GetData<Rec_JobVacancyEntity>(strIDs, ConstantSql.hrm_rec_sp_get_JobVacancyByIds, UserLogin, ref status);
            if (lstJobVacancy == null)
                return null;
            int i = 0;
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportRec_JobVacancy" + suffix;
            if (lstJobVacancy.Count > 1)
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
            foreach (var JobVacancy in lstJobVacancy)
            {
                if (JobVacancy.DateProposal.HasValue)
                {
                    JobVacancy.DateProposalFormat = JobVacancy.DateProposal.Value.ToString("dd/MM/yyyy");
                }
                if (JobVacancy.DateStart.HasValue)
                {
                    JobVacancy.DateStartFormat = JobVacancy.DateStart.Value.ToString("dd/MM/yyyy");
                }
                if (JobVacancy.DateEnd.HasValue)
                {
                    JobVacancy.DateEndFormat = JobVacancy.DateEnd.Value.ToString("dd/MM/yyyy");
                }
                if (JobVacancy.DateComplete.HasValue)
                {
                    JobVacancy.DateCompleteFormat = JobVacancy.DateComplete.Value.ToString("dd/MM/yyyy");
                }
                if (JobVacancy.DateEndReceiveCV.HasValue)
                {
                    JobVacancy.DateEndReceiveCVFormat = JobVacancy.DateEndReceiveCV.Value.ToString("dd/MM/yyyy");
                }

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

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(JobVacancy.Code) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(JobVacancy.Code) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilJobVacancy = new List<Rec_JobVacancyEntity>();
                ilJobVacancy.Add(JobVacancy);
                ExportService.ExportWord(outputPath, templatepath, ilJobVacancy);
            }
            if (lstJobVacancy.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }
        #endregion


        public ActionResult GetRecCostDetailByRecruitmentCampaign([DataSourceRequest] DataSourceRequest request, Guid? RecruitmentCampaignID)
        {
            if (RecruitmentCampaignID != null)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                var result = baseService.GetData<Rec_RecCostDetailEntity>(Common.DotNetToOracle(RecruitmentCampaignID.ToString()), ConstantSql.hrm_rec_sp_get_CostDetailByRecruitmentCampaignId, UserLogin, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetJobVacancyByID(string JobVacancyID)
        {
            string status = string.Empty;
            var profileID = Guid.Empty;
            if (JobVacancyID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(JobVacancyID))
            {
                profileID = Common.ConvertToGuid(JobVacancyID);
            }

            var profileServices = new Rec_JobVacancyServices();
            ActionService service = new ActionService(UserLogin);
            var jobVacancy = profileServices.GetData<Rec_JobVacancyEntity>(profileID, ConstantSql.hrm_rec_sp_get_JobVacancyId, UserLogin, ref status).FirstOrDefault();

            if (jobVacancy != null)
            {
                return Json(jobVacancy, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GetMPByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var result = baseService.GetData<Hre_MPModel>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_MPByProfileId, UserLogin, ref status);
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ContentResult ImportResultInterview([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");

            Rec_ImportInterviewResultService importService = new Rec_ImportInterviewResultService
            {
                FileName = _fileName,
                UserID = model.UserID,

            };

            DataTable dataTableInvalid = new DataTable("InvalidData");
            DataTable dataTable = new DataTable("ImportData");

            try
            {
                importService.ImportInterviewResult();
                dataTable = importService.GetImportObject();
                dataTableInvalid = importService.GetInvalidObject();
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }

            var importConfigs = new Dictionary<string, Dictionary<string, object>>();

            foreach (DataColumn item in dataTable.Columns)
            {
                var displayField = importService.FieldMappings.Where(d => d.Value == item.ColumnName).Select(d => d.Key).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(displayField) || importService.ListInvisibleField.Contains(item.ColumnName))
                {
                    var config = new Dictionary<string, object>();
                    config.Add("hidden", true);
                    importConfigs.Add(item.ColumnName, config);
                }
                else
                {
                    var config = new Dictionary<string, object>();
                    config.Add("title", displayField);
                    importConfigs.Add(item.ColumnName, config);
                }
            }

            model.ListImportData = dataTable.ConfigTable(importConfigs).ToDataSourceResult(request);
            model.ListInvalidData = dataTableInvalid.ConfigTable().ToDataSourceResult(request);

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue - 1;
            var result = new ContentResult();
            result.Content = serializer.Serialize(model);
            result.ContentType = "text/json";
            return result;
        }

        public ActionResult SaveResultInterviewImport([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            try
            {
                Rec_ImportInterviewResultService importService = new Rec_ImportInterviewResultService
                {
                    UserID = model.UserID,
                };

                if (importService.SaveInterviewResult())
                {
                    return Json(NotificationType.Success.ToString());
                }
                else
                {
                    return Json(NotificationType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                return Json(NotificationType.Error + "," + ex.GetBaseException());
            }
        }

        #region coomencode
        public ActionResult ExportWorkHistoryByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string[] valueFieldsExportID = valueFields.Split(',');
            var actionService = new ActionService(UserLogin);
            valueFields = valueFieldsExportID[0];
            //string _exportID = valueFieldsExportID[1];
            Guid exportID;
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            string status = string.Empty;
            DateTime DateStart = DateTime.Now;
            Cat_ExportEntity template = null;
            var exportService = new Cat_ExportServices();
            if (!string.IsNullOrEmpty(valueFields))
            {
                template = exportService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, UserLogin, ref status).FirstOrDefault();
            }
            if (template == null)
            {
                messages = "Error";
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

          
            var baseService = new BaseService();
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1); 
            var lstWorkHistory = baseService.GetData<Hre_WorkHistoryEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ExportWorkHistoryByIds,UserLogin, ref status);

            if (lstWorkHistory == null)
                return null;
            #region Lấy Dữ liệu hợp đồng
            Guid[] lstProfileByProfileID = lstWorkHistory.Select(s => (Guid)s.ProfileID).ToList().Distinct().ToArray();
            string profileIDs = string.Empty;

            foreach (var item in lstProfileByProfileID)
            {
                profileIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (profileIDs.IndexOf(",") > 0)
            {
                profileIDs = profileIDs.Substring(0, profileIDs.Length - 1);
            }
            
            var lstContract = actionService.GetData<Hre_ContractEntity>(profileIDs, ConstantSql.hrm_hr_sp_get_ContractBYProIDs, ref status);
            #endregion

            #region Lấy danh sách phụ lục hợp hợp đồng
            List<object> _lstobj = new List<object>();
            _lstobj.AddRange(new object[7]);
            _lstobj[5] = 1;
            _lstobj[6] = int.MaxValue - 1;
            var lstAppdixContract = actionService.GetData<Hre_AppendixContractEntity>(_lstobj, ConstantSql.hrm_hr_sp_get_AppendixContract, ref status);
            #endregion

            int i = 0;
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_ProfileProbation" + suffix;
            if (lstWorkHistory != null && lstWorkHistory.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var workHistory in lstWorkHistory)
            {                
                string outputPath = string.Empty;
                List<object> lstObjExport = new List<object>();
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(1);
                lstObjExport.Add(10000000);
                workHistory.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                workHistory.DateNow_Day = DateTime.Now.Day.ToString();
                workHistory.DateNow_Month = DateTime.Now.Month.ToString();
                workHistory.DateNow_Year = DateTime.Now.Year.ToString();                
                workHistory.DateEffectiveFormat = workHistory.DateEffective.ToString("dd/MM/yyyy");
                
                if (workHistory.DateNotice.HasValue)
                {
                    workHistory.DateNoticeFormat = workHistory.DateNotice.Value.ToString("dd/MM/yyyy");
                }

                if (workHistory.DateComeBack.HasValue)
                {
                    workHistory.DateComeBackFormat = workHistory.DateComeBack.Value.ToString("dd/MM/yyyy");
                }

                if (workHistory.Gender == "E_FEMALE")
                {
                    workHistory.GraveName = "Ms." + workHistory.ProfileName.Substring(workHistory.ProfileName.LastIndexOf(' '));
                }
                else
                {
                    workHistory.GraveName = "Mr." + workHistory.ProfileName.Substring(workHistory.ProfileName.LastIndexOf(' '));
                }
                if (workHistory.Gender == "E_FEMALE")
                {
                    workHistory.GenderView = "Chị";
                }
                if (workHistory.Gender == "E_MALE")
                {
                    workHistory.GenderView = "Anh";
                }

                #region lấy thông tin hợp đồng
                if (lstContract != null && lstContract.Count > 0)
                {
                    var _ContractByProid = lstContract.Where(s => s.ProfileID == workHistory.ProfileID).OrderByDescending(s => s.DateEnd).FirstOrDefault();
                    if (_ContractByProid != null )
                    {
                        //lấy mức lương điều chỉnh theo phuk lục hợp đông mới nhất.
                        #region lấy mức lương điều chỉnh theo phuk lục hợp đông mới nhất.
                        var _AppendixContract = lstAppdixContract.Where(s => s.ContractID == _ContractByProid.ID).OrderByDescending(s => s.DateofEffect).ToList();
                        if (_AppendixContract != null && _AppendixContract.Count > 0)
                        {
                            #region Salary
                            int stt = 0;
                            do
                            {
                                if (_AppendixContract[stt].Salary != null)
                                {
                                    _ContractByProid.Salary = _AppendixContract[stt].Salary;
                                    break;
                                }
                                stt++;
                            } while (_AppendixContract[stt].Salary == null || stt < _AppendixContract.Count);
                            #endregion
                            #region Allowance1
                            stt = 0;
                            do
                            {
                                if (_AppendixContract[stt].Allowance1 != null)
                                {
                                    _ContractByProid.Allowance1 = _AppendixContract[stt].Allowance1;
                                    break;
                                }
                                stt++;
                            } while (_AppendixContract[stt].Allowance1 == null || stt < _AppendixContract.Count);
                            #endregion
                            #region Allowance2
                            stt = 0;
                            do
                            {
                                if (_AppendixContract[stt].Allowance2 != null)
                                {
                                    _ContractByProid.Allowance2 = _AppendixContract[stt].Allowance2;
                                    break;
                                }
                                stt++;
                            } while (_AppendixContract[stt].Allowance2 == null || stt < _AppendixContract.Count);
                            #endregion
                            #region Allowance3
                            stt = 0;
                            do
                            {
                                if (_AppendixContract[stt].Allowance3 != null)
                                {
                                    _ContractByProid.Allowance3 = _AppendixContract[stt].Allowance3;
                                    break;
                                }
                                stt++;
                            } while (_AppendixContract[stt].Allowance3 == null || stt < _AppendixContract.Count);
                            #endregion
                            #region Allowance4
                            stt = 0;
                            do
                            {
                                if (_AppendixContract[stt].Allowance4 != null)
                                {
                                    _ContractByProid.Allowance4 = _AppendixContract[stt].Allowance4;
                                    break;
                                }
                                stt++;
                            } while (_AppendixContract[stt].Allowance4 == null || stt < _AppendixContract.Count);
                            #endregion
                           
                        }
                        workHistory.Salary = _ContractByProid.Salary;
                        workHistory.Allowance1 = _ContractByProid.Allowance1;
                        workHistory.Allowance2 = _ContractByProid.Allowance2;
                        workHistory.Allowance3 = _ContractByProid.Allowance3;
                        workHistory.Allowance4 = _ContractByProid.Allowance4;
                        #endregion
                    }
                }
                #endregion

                


               
                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(workHistory.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(workHistory.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilContract = new List<Hre_WorkHistoryEntity>();
                ilContract.Add(workHistory);
                ExportService.ExportWord(outputPath, templatepath, ilContract);
            }
            if (lstWorkHistory != null && lstWorkHistory.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }
        #endregion
    }
    
}