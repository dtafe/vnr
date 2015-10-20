using System.Linq;
using System.Web.Mvc;
using HRM.Business.Hr.Domain;
using HRM.Business.Main.Domain;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Business.Hr.Models;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using Microsoft.Ajax.Utilities;
using VnResource.Helper.Data;
using System;
using System.Reflection;
using System.Collections;
using System.Data.SqlTypes;
using HRM.Presentation.Service;
using HRM.Business.Category.Models;
using HRM.Presentation.Category.Models;
using System.Data;
using HRM.Business.Category.Domain;
using System.IO;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Payroll.Models;
using HRM.Presentation.Payroll.Models;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Attendance.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Finance.Models;
using HRM.Business.Finance.Domain;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.Evaluation.Domain;
using HRM.Business.Recruitment.Domain;
using HRM.Business.HrmSystem.Domain;
using HRM.Presentation.Recruitment.Models;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Hre_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        #region Hre_Profile


        [HttpPost]
        public ActionResult SetBlackListProfile(string selectedIds)
        {
            var service = new Hre_ProfileServices();
            var message = service.SetBlackListProfile(selectedIds);
            return Json(message);
        }
        //Check nv da nghi viec
        public JsonResult CheckProfileStoWorked(Guid profileid)
        {

            ActionService service = new ActionService(UserLogin);
            if (profileid != Guid.Empty)
            {
                string status = string.Empty;
                var entity = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(profileid.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
                if (entity != null && entity.DateQuit != null && entity.DateQuit < DateTime.Now)
                {
                    return Json("error");
                }
            }
            return Json("success");
        }
        //Muti nhân viên đang làm việc
        public JsonResult GetMultiProfileWorking(string text)
        {
            return GetDataForControl<Hre_ProfileModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_rec_sp_get_ProfileWorking_Multi);
        }

        string strOrderNumber = string.Empty;

        ///// <summary>
        ///// Hieu.Van
        ///// Hàm tự động xử lý send Mail
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult SendMailContract()
        //{
        //    LibraryService service = new LibraryService();
        //    LibraryService.SendMailContract();
        //    return Json(null);
        //}

        [HttpPost]
        public ActionResult GetOrgChartPortal()
        {
            var status = string.Empty;
            var listEntity = GetListData<Hre_OrgChartEntity>(ConstantSql.hrm_hr_sp_get_OrgChart);
            if (listEntity != null)
            {
                //var root = listEntity.Select(d => new Hre_OrgChartPortal()
                //{
                //    id = d.ID,
                //    parentId = d.ParentID,
                //    name = d.ProfileName,
                //    title = d.JobTitleName,
                //    image = "/Content/images/org/" + d.ImagePath

                //}).Where(d => d.parentId == null).FirstOrDefault();

                //var listModel = new List<Hre_OrgChartPortal>();
                //listModel.Add(root);

                var listModel = listEntity.OrderBy(d => d.Code).Select(d => new Hre_OrgChartPortal()
                {
                    id = d.ID,
                    parentId = d.ParentID,
                    name = d.ProfileName,
                    title = d.JobTitleName,
                    image = Hrm_Main_Web + "/Images/" + d.ImagePath

                }).ToList();
                //listModel.AddRange(listModel1);
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }

            return Json(null);
        }
        /// <summary>
        /// [Hien.Nguyen] Hàm lấy các phòng ban con
        /// </summary>
        /// <param name="source"></param>
        /// <param name="Ids"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private void GetChildOrgStructureID(List<Cat_OrgStructureTreeViewEntity> source, Guid Ids)
        {
            var listChild = source.Where(m => m.ParentID == Ids).ToList();
            if (listChild.Count <= 0)
            {
                return;
            }
            foreach (var i in listChild)
            {
                strOrderNumber += i.OrderNumber.ToString() + ",";
                GetChildOrgStructureID(source, i.ID);
            }
        }

        /// <summary>
        /// [Hien.Nguyen] DropdownNoCheckbox
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileListForDropdownNoCheckbox([DataSourceRequest] DataSourceRequest request, Hre_OrgStructureDetailsSearchModel model)
        {
            var actionService = new ActionService(UserLogin);
            string status = "";
            if (model.OrgStructureID != null)
            {
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(null);
                var listEntity = actionService.GetData<Cat_OrgStructureTreeViewEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure_Data, ref status);
                var orgId = listEntity.Where(m => m.OrderNumber.ToString() == model.OrgStructureID).FirstOrDefault();
                if (orgId.ID != null)
                {
                    strOrderNumber = string.Empty;
                    GetChildOrgStructureID(listEntity, orgId.ID);
                    model.OrgStructureID = strOrderNumber + model.OrgStructureID;
                }
            }

            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_OrgStructureDetailsSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileOrgStructureDetail);
        }

        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu cho Nhân Viên (Hre_Profile) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult GetProfileList([DataSourceRequest] DataSourceRequest request, Hre_ProfileActiveSearchModel model)
        {

            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileActiveSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileActive);
        }

        #region MyRegion
        //public ActionResult GetProfileChangeList([DataSourceRequest] DataSourceRequest request, Hre_ProfileChangeSearchModel model)
        //{

        //    //return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Profile);

        //    var service = new BaseService();
        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        PageIndex = request.Page,
        //        PageSize = request.PageSize,
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request),
        //        AdvanceFilters = ExtractAdvanceFilterAttributes(model)
        //    };
        //    List<object> pataProfile = new List<object>();
        //    pataProfile.AddRange(new object[16]);
        //    pataProfile[0] = model.ProfileName;
        //    pataProfile[1] = model.CodeEmp;
        //    pataProfile[2] = model.OrgStructureID;
        //    pataProfile[3] = model.PositionID;
        //    pataProfile[4] = model.Gender;
        //    pataProfile[5] = model.IDNo;
        //    pataProfile[6] = model.JobTitleID;
        //    pataProfile[7] = model.EmpTypeID;
        //    pataProfile[8] = model.DateFrom;
        //    pataProfile[9] = model.DateTo;
        //    pataProfile[14] = 1;
        //    pataProfile[15] = int.MaxValue - 1;


        //    var status = string.Empty;
        //    var result = service.GetData<Hre_ProfileEntity>(pataProfile, ConstantSql.hrm_hr_sp_get_Profile, ref status);
        //    var listModel = new List<Hre_ProfileModel>();
        //    if (result != null)
        //    {
        //        request.Page = 1;
        //        foreach (var item in result)
        //        {
        //            var newModle = (Hre_ProfileModel)typeof(Hre_ProfileModel).CreateInstance();
        //            foreach (var property in item.GetType().GetProperties())
        //            {
        //                newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
        //            }
        //            listModel.Add(newModle);
        //        }
        //        var dataSourceResult = listModel.ToDataSourceResult(request);
        //        if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
        //        {
        //            dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
        //        }
        //        return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        //    }
        //    var listModelNull = new List<Hre_ProfileModel>();
        //    ModelState.AddModelError("Id", status);
        //    return Json(listModelNull.ToDataSourceResult(request, ModelState));
        //}
        #endregion





        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu cho Nhân Viên (Hre_Profile) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult ExportProfileListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileActiveSearchModel model)
        {
            //if(model.ExportId == Guid.Empty)
            //{
            //    return null;
            //}
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;
            var profileServices = new Hre_ProfileServices();

            bool isGroup = profileServices.IsGroupByOrgProfileQuit();
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileActiveSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileActive, ref status);

            if (isGroup == true)
            {
                var rptServices = new Hre_ReportServices();
                var orgServices = new Cat_OrgStructureServices();
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(1);
                lstObjOrg.Add(int.MaxValue - 1);
                var lstOrg = actionService.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = actionService.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();

                result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileActiveSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileActive, ref status);
                DataTable table = new DataTable("Hre_ProfileModel");
                table.Columns.Add("CodeEmp");
                table.Columns.Add("ProfileName");
                table.Columns.Add("FirstName");
                table.Columns.Add("NameFamily");
                table.Columns.Add("Channel");
                table.Columns.Add("Region");
                table.Columns.Add("Area");
                table.Columns.Add("IDNo");
                table.Columns.Add("IDDateOfIssue", typeof(DateTime));
                table.Columns.Add("IDPlaceOfIssue");
                table.Columns.Add("DateOfBirth", typeof(DateTime));
                table.Columns.Add("PlaceOfBirth");
                table.Columns.Add("Gender");
                table.Columns.Add("MarriageStatus");
                table.Columns.Add("NationalityName");
                table.Columns.Add("ReligionName");
                table.Columns.Add("EthnicGroupName");
                table.Columns.Add("Email");
                table.Columns.Add("CellPhone");
                table.Columns.Add("CodeTax");
                table.Columns.Add("TDistrictName");
                table.Columns.Add("PDistrictName");
                table.Columns.Add("JobTitleName");
                table.Columns.Add("DateHire", typeof(DateTime));
                table.Columns.Add("ContractNo");
                table.Columns.Add("DateStartContract");
                table.Columns.Add("DateEndContract");
                table.Columns.Add("ContractTypeName");
                table.Columns.Add("TimesOfContract");
                table.Columns.Add("Notes");
                table.Columns.Add("TAddress");
                table.Columns.Add("PAddress");
                table.Columns.Add("BasicSalary", typeof(double));

                var salaryServices = new Sal_BasicSalaryServices();
                var objSalary = new List<object>();
                objSalary.AddRange(new object[10]);
                objSalary[8] = 1;
                objSalary[9] = int.MaxValue - 1;
                var lstBasicSalary = actionService.GetData<Sal_BasicSalaryEntity>(objSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref status).ToList();

                var unuServices = new Sal_UnusualAllowanceServices();
                var objUnu = new List<object>();
                objUnu.AddRange(new object[9]);
                objUnu[7] = 1;
                objUnu[8] = int.MaxValue - 1;
                var lstUnu = actionService.GetData<Sal_UnusualAllowanceEntity>(objUnu, ConstantSql.hrm_sal_sp_get_UnusualED, ref status).ToList();

                foreach (var item in result)
                {
                    var basicSalaryEntity = lstBasicSalary.Where(s => s.ProfileID == item.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var lstUnuByProfileID = lstUnu.Where(s => s.ProfileID == item.ID).ToList();
                    var orgName = new List<string>();
                    if (item.OrgStructureID != null)
                    {
                        orgName = rptServices.GetParentOrgName(lstOrg, lstOrgType, item.OrgStructureID);
                        if (orgName.Count < 3)
                        {
                            orgName.Insert(0, string.Empty);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                            }
                        }
                    }

                    var lstObjPayroll = new List<object>();
                    lstObjPayroll.Add(item.ID);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(1);
                    lstObjPayroll.Add(int.MaxValue - 1);
                    var payrollTableByProfileID = actionService.GetData<Sal_PayrollTableItemEntity>(lstObjPayroll, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile, ref status).OrderByDescending(s => s.DateCreate);

                    var lstObjContract = new List<object>();
                    lstObjContract.Add(item.ID);
                    var lstContract = actionService.GetData<Hre_ContractEntity>(lstObjContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status).OrderBy(s => s.DateCreate).ToList();

                    var lstObjRelative = new List<object>();
                    lstObjRelative.Add(item.ID);
                    lstObjRelative.Add(1);
                    lstObjRelative.Add(Int32.MaxValue - 1);
                    var lstDependant = actionService.GetData<Hre_DependantEntity>(lstObjRelative, ConstantSql.hrm_hr_sp_get_DependantByProfileId, ref status).ToList();

                    DataRow dr = table.NewRow();


                    dr["CodeEmp"] = item.CodeEmp;
                    dr["ProfileName"] = item.ProfileName;
                    dr["FirstName"] = item.FirstName;
                    dr["NameFamily"] = item.NameFamily;
                    if (orgName.Count > 0)
                    {
                        dr["Channel"] = orgName[2];
                        dr["Region"] = orgName[1];
                        dr["Area"] = orgName[0];

                    }

                    dr["IDNo"] = item.IDNo;
                    if (item.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = item.IDDateOfIssue.Value;
                    }

                    dr["IDPlaceOfIssue"] = item.IDPlaceOfIssue;
                    if (item.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = item.DateOfBirth;
                    }
                    dr["Gender"] = item.Gender;
                    if (item.Gender == EnumDropDown.Gender.E_FEMALE.ToString())
                    {
                        dr["Gender"] = "Nữ";
                    }
                    if (item.Gender == EnumDropDown.Gender.E_MALE.ToString())
                    {
                        dr["Gender"] = "Nam";
                    }
                    dr["MarriageStatus"] = item.MarriageStatus;
                    dr["NationalityName"] = item.NationalityName;
                    dr["ReligionName"] = item.ReligionName;
                    dr["EthnicGroupName"] = item.EthnicGroupName;
                    dr["Email"] = item.Email;
                    dr["CellPhone"] = item.Cellphone;
                    dr["CodeTax"] = item.CodeTax;
                    dr["TDistrictName"] = item.TDistrictName;
                    dr["PDistrictName"] = item.PDistrictName;
                    dr["JobTitleName"] = item.JobTitleName;
                    dr["Notes"] = item.Notes;
                    dr["TAddress"] = item.TAddress;
                    dr["PAddress"] = item.PAddress;
                    if (basicSalaryEntity != null)
                    {
                        dr["BasicSalary"] = double.Parse(basicSalaryEntity.GrossAmount);
                    }
                    if (item.DateHire != null)
                    {
                        dr["DateHire"] = item.DateHire.Value;
                    }

                    //if (payrollTableByProfileID != null)
                    //{
                    //    foreach (var payroll in payrollTableByProfileID)
                    //    {
                    //        Double value = 0;
                    //        if (!table.Columns.Contains(payroll.Code))
                    //        {
                    //            table.Columns.Add(payroll.Code, typeof(Double));
                    //        }
                    //        if (table.Columns.Contains(payroll.Code))
                    //        {
                    //            if (payroll.ValueType == typeof(Double).Name)
                    //            {
                    //                Double.TryParse(payroll.Value, out value);
                    //            }
                    //            dr[payroll.Code] = value;
                    //        }
                    //    }
                    //}

                    if (lstContract != null && lstContract.Count > 0)
                    {
                        var contractEntity = lstContract.FirstOrDefault();
                        dr["TimesOfContract"] = lstContract.Count;
                        dr["ContractNo"] = contractEntity.ContractNo;
                        dr["ContractTypeName"] = contractEntity.ContractTypeName;
                        dr["DateStartContract"] = contractEntity.DateStart;
                        if (contractEntity.DateEnd != null)
                        {
                            dr["DateStartContract"] = contractEntity.DateEnd.Value;
                        }

                    }
                    if (lstUnuByProfileID.Count > 0 && lstUnuByProfileID != null)
                    {
                        foreach (var unu in lstUnuByProfileID)
                        {
                            var titleName = unu.UnusualEDTypeCode + "|" + unu.UnusualAllowanceCfgName;
                            if (!table.Columns.Contains(titleName))
                            {
                                table.Columns.Add(titleName);
                            }
                            if (table.Columns.Contains(titleName))
                            {
                                dr[titleName] = unu.Amount;
                            }
                        }
                    }

                    if (lstDependant.Count > 0 && lstDependant != null)
                    {
                        foreach (var dependant in lstDependant)
                        {
                            var titleName = dependant.DependantName + "|" + dependant.RelativeTypeName;
                            if (!table.Columns.Contains(titleName))
                            {
                                table.Columns.Add(titleName);
                            }
                            if (table.Columns.Contains(titleName))
                            {
                                dr[titleName] = dependant.DependantName;
                            }
                        }
                    }
                    table.Rows.Add(dr);
                }

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
                        Object = table,
                        FileName = "Hre_ProfileModel",
                        OutPutPath = path,
                        // HeaderInfo = listHeaderInfo,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = true
                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }


                if (model.ExportId != Guid.Empty)
                {

                    var fullPath = ExportService.Export(model.ExportId, table, null, model.ExportType);
                    return Json(fullPath);
                }

                return Json(result.ToDataSourceResult(request));
            }

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
                    FileName = "Hre_ProfileModel",
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

        [HttpPost]
        public ActionResult ExportProfileAllListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileAllSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            var service = new BaseService();
            object obj = new Hre_ProfileModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileAllSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);


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
                    FileName = "Hre_ProfileModel",
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

        /// [Tho.Bui] 
        [HttpPost]
        public ActionResult GetProfileIsBackList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchIsBackListModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchIsBackListModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileIsBackList);
        }
        /// [Tho.Bui]
        [HttpPost]
        public ActionResult GetDateEndInfoList([DataSourceRequest] DataSourceRequest request, Hre_VisaInfoSearchModel model)
        {
            return GetListDataAndReturn<Hre_VisaInfoModel, Hre_VisaInfoEntity, Hre_VisaInfoSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_VisaInfoDateEndList);
        }
        [HttpPost]
        public ActionResult GetDateEndInfoListss([DataSourceRequest] DataSourceRequest request, Hre_VisaInfoSearchModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateEnd ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo ?? DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_VisaInfoModel(),
                    FileName = "Hre_VisaInfoModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var service = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.Add(Model.DateEnd);
            listObj.Add(Model.DateTo);
            listObj.Add(null);
            listObj.Add(null);
            string status = string.Empty;
            var result = service.GetData<Hre_VisaInfoEntity>(listObj, ConstantSql.hrm_hr_sp_get_VisaInfoDateEndList, ref status).ToList().Translate<Hre_VisaInfoModel>();
            var lstprofileids = result.Select(s => s.Visa_ID).ToList();

            foreach (var item in result)
            {
                Guid profileID = item.Visa_ID;
                // item.Visa_ID. = result.Count(s => s.Visa_ID == profileID);
            }


            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        /// [Tho.Bui] 
        [HttpPost]
        public ActionResult GetDateEndAccidentTypeList([DataSourceRequest] DataSourceRequest request, Hre_AccidentModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_AccidentModel>(model, "Hre_ReportAccident", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            //DateTime From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //DateTime To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            #endregion
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom != null ? model.DateFrom.Value : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo != null ? model.DateTo.Value : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            var service = new ActionService(UserLogin);

            List<object> listObj = new List<object>();
            DateTime? From = SqlDateTime.MinValue.Value;
            DateTime? To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            listObj.Add(From);
            listObj.Add(To);
            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                strOrgIDs = model.OrgStructureID;
            }
            listObj.Add(strOrgIDs);
            Guid? Acctype = Guid.Empty;
            if (model.AccidentTypeID != Guid.Empty)
            {
                Acctype = model.AccidentTypeID;
            }
            listObj.Add(Acctype);
            string status = string.Empty;
            var result = service.GetData<Hre_AccidentEntity>(listObj, ConstantSql.hrm_hr_sp_get_DateEndAccidentTypeList, ref status).ToList().Translate<Hre_AccidentModel>();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_AccidentModel(),
                    FileName = "Hre_ReportAccident",
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
            //return GetListDataAndReturn<Hre_AccidentModel, Hre_AccidentEntity, Hre_ReportAccidentSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_DateEndAccidentTypeList);
        }

        public ActionResult Translate(string field)
        {
            var strField = field.Replace("\n", "");
            var listField = strField.Split(',');
            Dictionary<string, string> listTranslate = new Dictionary<string, string>();
            for (int i = 0; i < listField.Count() - 1; i++)
            {
                listTranslate.Add(listField[i], listField[i].TranslateString());
            }

            return Json(listTranslate);
        }
        /// [Chuc.Nguyen] - Xuất danh sách dữ liệu cho Nhân Viên (Hre_Profile) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult ExportProfileList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Profile);
        }

        /// [Tho.Bui] - Export Profilebacklist
        [HttpPost]
        public ActionResult ExportProfileIsBackList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchIsBackListModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ProfileSearchIsBackListModel>(model, "Hre_ReportProfileIsBackList", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            string status = string.Empty;

            object obj = new Hre_ProfileSearchIsBackListModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = GetListData<Hre_ReportProfileIsBackListModel, Hre_ReportProfileIsBackListEntity, Hre_ProfileSearchIsBackListModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileIsBackList, ref status);

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportProfileIsBackListModel(),
                    FileName = "Hre_ReportProfileIsBackList",
                    OutPutPath = path,
                    //HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Nhân Viên (Hre_Accident) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult ExportDateEndAccidentList([DataSourceRequest] DataSourceRequest request, Hre_AccidentModel model)
        {
            var ActionService = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            DateTime? From = SqlDateTime.MinValue.Value;
            DateTime? To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            listObj.Add(From);
            listObj.Add(To);
            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                strOrgIDs = model.OrgStructureID;
            }
            listObj.Add(strOrgIDs);
            Guid? Acctype = Guid.Empty;
            if (model.AccidentTypeID != Guid.Empty)
            {
                Acctype = model.AccidentTypeID;
            }
            listObj.Add(Acctype);
            string status = string.Empty;
            var result = ActionService.GetData<Hre_AccidentEntity>(listObj, ConstantSql.hrm_hr_sp_get_DateEndAccidentTypeList, ref status).ToList().Translate<Hre_AccidentModel>();
            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        /// [Tho.Bui]
        [HttpPost]
        public ActionResult ExportDateEndVisaInfoList([DataSourceRequest] DataSourceRequest request, Hre_VisaInfoSearchModel model)
        {
            return ExportAllAndReturn<Hre_VisaInfoEntity, Hre_VisaInfoModel, Hre_VisaInfoSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_VisaInfoDateEndList);
        }

        [HttpPost]
        /// [Chuc.Nguyen] - Xuất các dòng dữ liệu được chọn của Nhân Viên (Hre_Profile) ra file Excel
        public ActionResult ExportSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_ProfileEntity, Hre_ProfileModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_ProfileByIds);
        }

        public ActionResult ExportPersionalInformation([DataSourceRequest] DataSourceRequest request, Hre_PersionalInfoSearchModel model)
        {
            var service = new Hre_ProfileServices();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            var isDataTable = false;
            object obj = new DataTable();

            var result = service.GetPersionalInfo(model.id, UserLogin, model.IsCreateTemplate);

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
                    FileName = "Hre_PersionalInformationModel",
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

        /// [Tho.Bui] - Export seclect Hre_profile Not full data
        public ActionResult ExportProfileNptFullDataSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_ProfileEntity, Hre_ProfileModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_ProfileNotFullDataByIds);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetListProfiles([DataSourceRequest] DataSourceRequest request, Hre_ProfileGeneralMultiSearchModel profileModel)
        {
            #region [Hien.Nguyen]
            if (profileModel.ProfileID != null && profileModel.ProfileID != Guid.Empty)
                profileModel.Keyword = null;
            #endregion
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileGeneralMultiSearchModel>(request, profileModel, ConstantSql.hrm_hr_sp_get_Profile_GeneralGrid);
        }

        public ActionResult ExportListProfile([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Profile_GeneralGrid);
        }


        /// [Hieu.Van] - 2014/07/02 Lấy Mã Thẻ(CardCode) theo mã Nhân Viên(Hre_Profile)
        public JsonResult GetCardCodeByProfileId(Guid id)
        {
            var service = new ActionService(UserLogin);
            var status = string.Empty;
            var fieldData = service.GetFirstData<Hre_ProfileCodeEntity>(id, ConstantSql.hrm_hre_sp_get_Profile_CardCodeByProfileID, ref status);
            List<object> listObj = new List<object>();
            listObj.Add(status);
            listObj.Add(fieldData);
            return Json(listObj, JsonRequestBehavior.AllowGet);
        }

        /// [Quoc.Do] - 2014/09/19
        /// Lấy dữ liệu tất cả ds NV  load lên lưới bằng store
        [HttpPost]
        public ActionResult GetProfileAllList([DataSourceRequest] DataSourceRequest request, Hre_ProfileAllSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileAllSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileAll);
        }
        /// [Son.Vo] - 2014/07/10
        /// Lấy dữ liệu NV nghỉ việc load lên lưới bằng store
        [HttpPost]
        public ActionResult GetProfileQuitList([DataSourceRequest] DataSourceRequest request, Hre_ProfileQuitSearchModel model)
        {
            var profileServices = new Hre_ProfileServices();
            var service = new ActionService(UserLogin);
            bool isGroup = profileServices.IsGroupByOrgProfileQuit();

            if (isGroup == true)
            {
                #region Group theo DS phòng ban - DDF
                var rptServices = new Hre_ReportServices();
                string status = string.Empty;

                var orgServices = new Cat_OrgStructureServices();
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(1);
                lstObjOrg.Add(int.MaxValue - 1);
                var lstOrg = service.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = service.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();

                ListQueryModel lstModel = new ListQueryModel
                {
                    PageIndex = request.Page,
                    PageSize = request.PageSize,
                    Filters = ExtractFilterAttributes(request),
                    Sorts = ExtractSortAttributes(request),
                    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
                };
                var lstEntity = service.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_ProfileQuit, ref status);
                var lstProfileEntity = new List<Hre_ProfileEntity>();
                var lstProfileModel = new List<Hre_ProfileModel>();

                if (lstEntity != null)
                {
                    request.Page = 1;
                    foreach (var item in lstEntity)
                    {
                        var profileEntity = new Hre_ProfileEntity();
                        var orgName = new List<string>();
                        if (item.OrgStructureID != null)
                        {
                            orgName = rptServices.GetParentOrgName(lstOrg, lstOrgType, item.OrgStructureID);
                        }
                        // orgName = rptServices.GetParentOrgName(lstOrg, lstOrgType, item.OrgStructureID);
                        if (orgName.Count == 0)
                            continue;
                        if (orgName.Count < 3)
                        {
                            orgName.Insert(0, string.Empty);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                            }
                        }
                        profileEntity = item.CopyData<Hre_ProfileEntity>();
                        profileEntity.Channel = orgName[2];
                        profileEntity.Region = orgName[1];
                        profileEntity.Area = orgName[0];
                        lstProfileEntity.Add(profileEntity);
                    }
                    lstProfileModel = lstProfileEntity.Translate<Hre_ProfileModel>();
                    var dataSourceResult = lstProfileModel.ToDataSourceResult(request);
                    if (lstProfileModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                    {
                        dataSourceResult.Total = lstProfileModel.Count() <= 0 ? 0 : (int)lstProfileModel.FirstOrDefault().GetPropertyValue("TotalRow");
                    }

                    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileQuitSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQuit);

        }

        public ActionResult ExportProfileQuitList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQuit);
        }

        public ActionResult ExportProfileQuitListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileQuitSearchModel model)
        {
            var hreservices = new Hre_ProfileServices();
            var service = new ActionService(UserLogin);
            bool isGroup = hreservices.IsGroupByOrgProfileQuit();
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var services = new BaseService();
            var rptServices = new Hre_ReportServices();
            var orgServices = new Cat_OrgStructureServices();
            #region Group theo phòng ban - BDF
            if (isGroup == true)
            {
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(1);
                lstObjOrg.Add(int.MaxValue - 1);
                var lstOrg = service.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = service.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();

                var result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileQuitSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQuit, ref status);

                DataTable table = new DataTable("Hre_ProfileQuitModel");
                table.Columns.Add("CodeEmp");
                table.Columns.Add("ProfileName");
                table.Columns.Add("FirstName");
                table.Columns.Add("NameFamily");
                table.Columns.Add("Channel");
                table.Columns.Add("Region");
                table.Columns.Add("Area");
                table.Columns.Add("IDNo");
                table.Columns.Add("IDDateOfIssue", typeof(DateTime));
                table.Columns.Add("IDPlaceOfIssue");
                table.Columns.Add("DateOfBirth", typeof(DateTime));
                table.Columns.Add("DateQuit", typeof(DateTime));
                table.Columns.Add("PlaceOfBirth");
                table.Columns.Add("Gender");
                table.Columns.Add("MarriageStatus");
                table.Columns.Add("NationalityName");
                table.Columns.Add("ReligionName");
                table.Columns.Add("EthnicGroupName");
                table.Columns.Add("Email");
                table.Columns.Add("CellPhone");
                table.Columns.Add("CodeTax");
                table.Columns.Add("TDistrictName");
                table.Columns.Add("PDistrictName");
                table.Columns.Add("JobTitleName");
                table.Columns.Add("DateHire", typeof(DateTime));
                table.Columns.Add("ContractNo");
                table.Columns.Add("DateStartContract");
                table.Columns.Add("DateEndContract");
                table.Columns.Add("ContractTypeName");
                table.Columns.Add("TimesOfContract");
                table.Columns.Add("Notes");

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
                        Object = table,
                        FileName = "Hre_ProfileQuitModel",
                        OutPutPath = path,
                        // HeaderInfo = listHeaderInfo,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = true
                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }
                foreach (var item in result)
                {
                    var orgName = new List<string>();
                    if (item.OrgStructureID != null)
                    {
                        orgName = rptServices.GetParentOrgName(lstOrg, lstOrgType, item.OrgStructureID);
                        if (orgName.Count < 3)
                        {
                            orgName.Insert(0, string.Empty);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                            }
                        }
                    }
                    var lstObjPayroll = new List<object>();
                    lstObjPayroll.Add(item.ID);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(null);
                    lstObjPayroll.Add(1);
                    lstObjPayroll.Add(int.MaxValue - 1);
                    var payrollTableByProfileID = service.GetData<Sal_PayrollTableItemEntity>(lstObjPayroll, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile, ref status).OrderByDescending(s => s.DateCreate);
                    var lstObjContract = new List<object>();
                    lstObjContract.Add(item.ID);
                    var lstContract = service.GetData<Hre_ContractEntity>(lstObjContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status).OrderBy(s => s.DateCreate).ToList();
                    var lstObjRelative = new List<object>();
                    lstObjRelative.Add(item.ID);
                    lstObjRelative.Add(1);
                    lstObjRelative.Add(Int32.MaxValue - 1);
                    var lstDependant = service.GetData<Hre_DependantEntity>(lstObjRelative, ConstantSql.hrm_hr_sp_get_DependantByProfileId, ref status).ToList();
                    DataRow dr = table.NewRow();
                    dr["CodeEmp"] = item.CodeEmp;
                    dr["ProfileName"] = item.ProfileName;
                    dr["FirstName"] = item.FirstName;
                    dr["NameFamily"] = item.NameFamily;
                    if (orgName.Count > 0)
                    {
                        dr["Channel"] = orgName[2];
                        dr["Region"] = orgName[1];
                        dr["Area"] = orgName[0];
                    }
                    dr["IDNo"] = item.IDNo;
                    if (item.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = item.IDDateOfIssue.Value;
                    }

                    dr["IDPlaceOfIssue"] = item.IDPlaceOfIssue;
                    if (item.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = item.DateOfBirth;
                    }
                    dr["Gender"] = item.Gender;
                    if (item.Gender == EnumDropDown.Gender.E_FEMALE.ToString())
                    {
                        dr["Gender"] = "Nữ";
                    }
                    if (item.Gender == EnumDropDown.Gender.E_MALE.ToString())
                    {
                        dr["Gender"] = "Nam";
                    }
                    dr["MarriageStatus"] = item.MarriageStatus;
                    dr["NationalityName"] = item.NationalityName;
                    dr["ReligionName"] = item.ReligionName;
                    dr["EthnicGroupName"] = item.EthnicGroupName;
                    dr["Email"] = item.Email;
                    dr["CellPhone"] = item.Cellphone;
                    dr["CodeTax"] = item.CodeTax;
                    dr["TDistrictName"] = item.TDistrictName;
                    dr["PDistrictName"] = item.PDistrictName;
                    dr["JobTitleName"] = item.JobTitleName;
                    dr["Notes"] = item.Notes;
                    if (item.DateHire != null)
                    {
                        dr["DateHire"] = item.DateHire.Value;
                    }
                    if (item.DateQuit != null)
                    {
                        dr["DateQuit"] = item.DateQuit.Value;

                    }
                    if (payrollTableByProfileID != null)
                    {
                        foreach (var payroll in payrollTableByProfileID)
                        {
                            Double value = 0;
                            if (!table.Columns.Contains(payroll.Code))
                            {
                                table.Columns.Add(payroll.Code, typeof(Double));
                            }
                            if (table.Columns.Contains(payroll.Code))
                            {
                                if (payroll.ValueType == typeof(Double).Name)
                                {
                                    Double.TryParse(payroll.Value, out value);
                                }
                                dr[payroll.Code] = value;
                            }
                        }
                    }
                    if (lstContract != null && lstContract.Count > 0)
                    {
                        var contractEntity = lstContract.FirstOrDefault();
                        dr["TimesOfContract"] = lstContract.Count;
                        dr["ContractNo"] = contractEntity.ContractNo;
                        dr["ContractTypeName"] = contractEntity.ContractTypeName;
                        dr["DateStartContract"] = contractEntity.DateStart;
                        if (contractEntity.DateEnd != null)
                        {
                            dr["DateStartContract"] = contractEntity.DateEnd.Value;
                        }
                    }
                    if (lstDependant.Count > 0 && lstDependant != null)
                    {
                        foreach (var dependant in lstDependant)
                        {
                            var titleName = dependant.DependantName + "|" + dependant.RelativeTypeName;
                            if (!table.Columns.Contains(titleName))
                            {
                                table.Columns.Add(titleName);
                            }
                            if (table.Columns.Contains(titleName))
                            {
                                dr[titleName] = dependant.DependantName;
                            }
                        }
                    }
                    table.Rows.Add(dr);
                }
                if (model.ExportId != Guid.Empty)
                {
                    var fullPath = ExportService.Export(model.ExportId, table, null, model.ExportType);
                    return Json(fullPath);
                }

                return Json(result.ToDataSourceResult(request));
            }
            #endregion
            else
            {
                var result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileQuitSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQuit, ref status);

                if (model != null && model.IsCreateTemplate)
                {
                    var path = Common.GetPath("Templates");
                    ExportService exportService = new ExportService();
                    ConfigExport cfgExport = new ConfigExport()
                    {
                        Object = new Hre_ProfileModel(),
                        FileName = "Hre_Profile",
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
        }

        /// [Tho.Bui] - 2014/09/19
        /// Lấy dữ liệu NV nghỉ hưu load lên lưới bằng store
        [HttpPost]
        public ActionResult GetProfileRetirementList([DataSourceRequest] DataSourceRequest request, Hre_ProfileRetirementSearchModel model)
        {
            var status = string.Empty;
            var listEntity = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileRetirementSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileRetirement, ref status);

            if (model.ExportId != Guid.Empty)
            {

                var fullPath = ExportService.Export(model.ExportId, listEntity, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(listEntity.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Son.Vo] - 2014/07/10
        /// Lấy dữ liệu NV nghỉ việc load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileWaitingHireList([DataSourceRequest] DataSourceRequest request, Hre_ProfileWaitingHireSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileWaitingHireSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileWaitingHire);
        }
        /// <summary>
        /// [Tho.Bui] - 2014/07/10
        /// Lấy dữ liệu NV nhân viên bị thiếu các trường
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileNotFullData([DataSourceRequest] DataSourceRequest request, Hre_ReportNotFullDataSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ReportNotFullDataSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileNotFullData);
        }
        /// <summary>
        /// [Tho.Bui] - 2014/07/10
        /// Lấy dữ liệu NV không nhận việc bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileUnHire([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileUnHire);
        }

        [HttpPost]
        public ActionResult ExportProfileUnHireList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileUnHire);
        }
        /// <summary>
        /// [Tam.Le] - 2014/07/10
        /// Lấy dữ liệu NV thử việc load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileProbationList([DataSourceRequest] DataSourceRequest request, Hre_ProfileProbationSearchModel model)
        {
            if (model.IsExCludeQuitEmp == true)
            {
                model.Status = "E_WAITING_APPROVE,";
            }
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileProbationSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProbationProfile);

        }

        public ActionResult ExportProfileProbationListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileProbationSearchModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom != null ? model.DateFrom : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo != null ? model.DateTo : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsExCludeQuitEmp == true)
            {
                model.Status = "E_WAITING_APPROVE,";
            }
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = actionService.GetData<Hre_ProfileModel>(lstModel, ConstantSql.hrm_hr_sp_get_ProbationProfile, ref status);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ProfileModel(),
                    FileName = "Hre_Profile",
                    HeaderInfo = listHeaderInfo,
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

        public ActionResult GetPlanHeadCountList([DataSourceRequest] DataSourceRequest request, Hre_PlanHeadCountSearchModel model)
        {
            return GetListDataAndReturn<Hre_PlanHeadCountModel, Hre_PlanHeadCountEntity, Hre_PlanHeadCountSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PlanHeadCount);

        }
        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Hre_Profile (Hre_Profile)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileProbationList([DataSourceRequest] DataSourceRequest request, Hre_ProfileProbationSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileProbationSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProbationProfile);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Hre_Profile (Hre_Profile)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileWaitingHireList([DataSourceRequest] DataSourceRequest request, Hre_ProfileWaitingHireSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileWaitingHireSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileWaitingHire);
        }
        /// <summary>
        /// [Tho.Bui] - Xuất danh sách dữ liệu cho Hre_Profile not full data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileNotFullDataList([DataSourceRequest] DataSourceRequest request, Hre_ReportNotFullDataSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ReportNotFullDataSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileNotFullData);
        }
        /// <summary>
        /// [Quoc.Do] - Xuất tất cả danh sách dữ liệu cho Hre_Profile (Hre_Profile)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileAllList([DataSourceRequest] DataSourceRequest request, Hre_ProfileAllSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileAllSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileAll);
        }
        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Hre_Profile (Hre_Profile)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>


        /// <summary>
        /// [Tho.Bui] - Xuất danh sách dữ liệu nhân viên nghĩ hưu cho Hre_Profile (Hre_Profile)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfileRetirementList([DataSourceRequest] DataSourceRequest request, Hre_ProfileRetirementSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfileRetirementSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileRetirement);
        }

        /// <summary>
        /// Lấy danh sách hồ sơ nhân viên
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProfile()
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var hre = service.GetAllUseEntity<Hre_ProfileEntity>(ref status);
            var result = hre.Select(item => new Hre_ProfileSelectModel()
            {
                ID = item.ID,
                ProfileName = item.ProfileName
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileByIdAnCutOffId(int id, int cutOffId)
        {
            string status = string.Empty;
            var service = new Hre_ProfileServices();
            var hre = service.GetAllUseEntity<Hre_ProfileEntity>(ref status);
            var result = hre.Select(item => new Hre_ProfileSelectModel()
            {
                ID = item.ID,
                ProfileName = item.ProfileName
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //SonVo - 20140904 - chuyển trạng thái NV sang NV đang làm việc
        public ActionResult ActionHire(string selectedIds, string status, string userID)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => Guid.Parse(x))
                   .ToList();
            }
            var lstMealRecordMissing = new List<Hre_ProfileEntity>();
            var services = new Hre_ProfileServices();
            services.SubmitStatus(ids, status, userID, UserLogin);
            return Json("");
        }
        public ActionResult SetStatusHire(string selectedIds, string statusPofile, string statusBasicSalary, string statusWorkHistory, string statusContract)
        {
            string status = string.Empty;
            BaseService service = new BaseService();
            ActionService ActionService = new ActionService(UserLogin);
            List<object> lstObj = new List<object>();
            lstObj.Add(Common.DotNetToOracle(selectedIds));
            lstObj.Add(statusPofile);
            lstObj.Add(statusBasicSalary);
            lstObj.Add(statusWorkHistory);
            lstObj.Add(statusContract);
            var rs = service.UpdateData<Hre_ProfileModel>(lstObj, ConstantSql.hrm_hre_sp_Set_ApproveProfile_Status, ref status);

            var contractServices = new Hre_ContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContract = ActionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();

            var lstProfileIDs = selectedIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
            if (lstProfileIDs != null)
            {
                foreach (var item in lstProfileIDs)
                {
                    var lstContractByProfileID = lstContract.Where(s => s.Status == EnumDropDown.Status.E_APPROVED.ToString() && item == s.ProfileID).ToList();

                    var contractEntity = lstContract.Where(s => s.ProfileID == item).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    var listIdContract = string.Empty;
                    if (lstContractByProfileID != null)
                    {
                        listIdContract = string.Join(",", lstContractByProfileID.Select(d => d.ContractTypeID));
                        if (contractEntity != null)
                        {
                            contractEntity = SetNewCodeContract(contractEntity, listIdContract);
                            contractEntity.Status = EnumDropDown.Status.E_APPROVED.ToString();
                            status = contractServices.Edit(contractEntity);
                        }
                    }

                }
            }

            if (status != "")
            {
                return Json(status);
            }
            return Json("");
        }

        public ActionResult ExportProfileQuitByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;
            ActionService ActionService = new ActionService(UserLogin);

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
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
            var lstProfile = ActionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileQuitByListId, ref status);
            if (lstProfile == null || lstProfile.Count == 0)
                return null;

            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfile.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var profile in lstProfile)
            {

                profile.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                profile.DateNow_Day = DateTime.Now.Day.ToString();
                profile.DateNow_Month = DateTime.Now.Month.ToString();
                profile.DateNow_Year = DateTime.Now.Year.ToString();
                if (profile.DateStart.HasValue)
                    profile.DateStartFormat = profile.DateStart.Value.ToString("dd/MM/yyyy");
                if (profile.RequestDate.HasValue)
                    profile.RequestDateFormat = profile.RequestDate.Value.ToString("dd/MM/yyyy");
                if (profile.DateQuit.HasValue)
                    profile.DateQuitFormat = profile.DateQuit.Value.ToString("dd/MM/yyyy");
                if (profile.DateHire.HasValue)
                    profile.DateHireFormat = profile.DateHire.Value.ToString("dd/MM/yyyy");

                if (profile.DateOfBirth.HasValue)
                    profile.DateOfBirthFormat = profile.DateOfBirth.Value.ToString("dd/MM/yyyy");
                if (profile.IDDateOfIssue.HasValue)
                    profile.IDDateOfIssueFormat = profile.IDDateOfIssue.Value.ToString("dd/MM/yyyy");
                if (profile.DateSigned.HasValue)
                    profile.DateSignedFormat = profile.DateSigned.Value.ToString("dd/MM/yyyy");
                if (profile.DateQuitSign.HasValue)
                {
                    profile.DateQuitSignFormat = profile.DateQuitSign.Value.ToString("dd/MM/yyyy");
                }

                if (profile.Gender == "E_FEMALE")
                {
                    profile.GraveName = "Ms." + profile.ProfileName.Substring(profile.ProfileName.LastIndexOf(' '));
                }
                else
                {
                    profile.GraveName = "Mr." + profile.ProfileName.Substring(profile.ProfileName.LastIndexOf(' '));
                }
                if (profile.Gender == "E_FEMALE")
                {
                    profile.GenderView = "Chị";
                }
                if (profile.Gender == "E_MALE")
                {
                    profile.GenderView = "Anh";
                }



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

                template = ActionService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstProfile1 = new List<Hre_ProfileEntity>();
                lstProfile1.Add(profile);
                ExportService.ExportWord(outputPath, templatepath, lstProfile1);
            }
            if (lstProfile.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportWordProfileByTemplate(Guid selectedIds, string valueFields)
        {
            var actionService = new ActionService(UserLogin);
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var baseService = new BaseService();
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(selectedIds.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            if (lstProfile == null || lstProfile.Count == 0)
                return null;

            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfile.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var profile in lstProfile)
            {

                profile.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (profile.DateStart.HasValue)
                    profile.DateStartFormat = profile.DateStart.Value.ToString("dd/MM/yyyy");
                if (profile.RequestDate.HasValue)
                    profile.RequestDateFormat = profile.RequestDate.Value.ToString("dd/MM/yyyy");
                if (profile.DateQuit.HasValue)
                    profile.DateQuitFormat = profile.DateQuit.Value.ToString("dd/MM/yyyy");

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

                template = actionService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstProfile1 = new List<Hre_ProfileEntity>();
                lstProfile1.Add(profile);
                ExportService.ExportWord(outputPath, templatepath, lstProfile1);
            }
            if (lstProfile.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportProfileWorkingByTemplate(List<Guid> selectedIds, string valueFields)
        {
            var actionService = new ActionService(UserLogin);
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var ActionService = new ActionService(UserLogin);
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
            var lstProfile = baseService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileWorkingByListId, UserLogin, ref status);
            if (lstProfile == null)
                return null;
            int i = 0;
            var lstProfileID = lstProfile.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfileID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            #region ds nguoi than cua nhan vien duoc chon
            string strProfileIDs = "";
            if (lstProfileID != null)
            {
                foreach (var ProfileID in lstProfileID)
                {
                    string _strProfileID = Common.DotNetToOracle(ProfileID.ToString());
                    strProfileIDs += _strProfileID + ",";
                }
            }
            var lstRelativeByProfileIDs = ActionService.GetData<Hre_RelativesEntity>(strProfileIDs, ConstantSql.hrm_hr_sp_get_RelativeByProfileIds, ref status);
            #endregion

            foreach (var profile in lstProfile)
            {
                if (profile.IDDateOfIssue.HasValue)
                    profile.IDDateOfIssueFormat = profile.IDDateOfIssue.Value.ToString("dd/MM/yyyy");
                if (profile.DateOfBirth.HasValue)
                    profile.DateOfBirthFormat = profile.DateOfBirth.Value.ToString("dd/MM/yyyy");
                if (profile.Salary != null)
                    profile.SalaryFormat = String.Format("{0:0,0}", profile.Salary);
                if (profile.Allowance1 != null)
                    profile.Allowance1Format = String.Format("{0:0,0}", profile.Allowance1);

                if (profile.DayOfBirth > 0 && profile.MonthOfBirth > 0 && profile.YearOfBirth > 0)
                {
                    profile.Birthday = profile.DayOfBirth + "/" + profile.MonthOfBirth + "/" + profile.YearOfBirth;
                }
                if (profile.DateHire.HasValue)
                {
                    profile.DateHireFormat = profile.DateHire.Value.ToString("dd/MM/yyyy");
                }
                profile.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (profile.DateStart.HasValue)
                {
                    profile.DateStartString = "Ngày " + profile.DateStart.Value.Day + " Tháng " + profile.DateStart.Value.Month + " Năm " + profile.DateStart.Value.Year + " ";
                    profile.DateStartFormat = profile.DateStart.Value.ToString("dd/MM/yyyy");
                }
                if (profile.DateEnd.HasValue)
                {
                    profile.DateEndString = "Ngày " + profile.DateEnd.Value.Day + " Tháng " + profile.DateEnd.Value.Month + " Năm " + profile.DateEnd.Value.Year + " ";
                    profile.DateEndFormat = profile.DateEnd.Value.ToString("dd/MM/yyyy");
                }
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
                    template = actionService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                #region lay nguoi than cua tung nhan vien
                List<Hre_RelativesEntity> lstRelativeByProfileID = new List<Hre_RelativesEntity>();
                if (lstRelativeByProfileIDs != null)
                {
                    lstRelativeByProfileID = lstRelativeByProfileIDs.Where(s => s.ProfileID == profile.ID).ToList();
                }

                #endregion

                var lstcontract = new List<Hre_ProfileEntity>();
                lstcontract.Add(profile);
                Hre_ProfileEntity objProfile = new Hre_ProfileEntity();
                //string[] FieldtblProfiles = new string[] { "ProfileName","ID"};
                //DataTable tblProfiles = Common.ConvertIListToDataTable(lstcontract, FieldtblProfiles);
                DataTable tblProfiles = new DataTable();
                tblProfiles = lstcontract.Translate();
                DataTable tblRelatives = new DataTable();
                tblRelatives = lstRelativeByProfileID.Translate();
                //Common.ConvertIListToDataTable(lstRelativeByProfileID, FieldtblRelatives);
                DataSet dsData = new DataSet();
                dsData.Tables.Add(tblProfiles);
                dsData.Tables.Add(tblRelatives);
                dsData.Tables[0].TableName = "tblProfiles";
                dsData.Tables[1].TableName = "tblRelatives";

                //ExportService.ExportWord(outputPath, templatepath, dsData);
                ExportService.ExportWithRegions(outputPath, templatepath, dsData);
            }
            if (lstProfileID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportWorkHistoryByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string[] valueFieldsExportID = valueFields.Split(',');
            valueFields = valueFieldsExportID[0];
            string _exportID = valueFieldsExportID[1];
            Guid exportID;

            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var actionService = new ActionService(UserLogin);
            var workhistoryService = new Hre_WorkHistoryServices();
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstProfile = actionService.GetData<Hre_WorkHistoryEntity>(objs, ConstantSql.hrm_hr_sp_get_ExportWorkHistoryByIds, ref status);
            if (lstProfile == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfile.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var profile in lstProfile)
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

                if (_exportID != "")
                {
                    exportID = Guid.Parse(_exportID);
                    template = actionService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields && s.ID == exportID).FirstOrDefault();
                }
                else
                {
                    template = actionService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
                }
                //template = exportService.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstWorkHistory = new List<Hre_WorkHistoryEntity>();
                lstWorkHistory.Add(profile);
                ExportService.ExportWord(outputPath, templatepath, lstWorkHistory);
            }
            if (lstProfile.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportWorkHistoryByListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_WorkHistorySearchModel model)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Hre_ProfileModel();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = actionService.GetData<Hre_WorkHistoryModel>(lstModel, ConstantSql.hrm_hr_sp_get_WorkHistory, ref status);
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom != null ? model.DateFrom : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo != null ? model.DateTo : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_WorkHistoryModel(),
                    FileName = "Hre_WorkHistory",
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

        public ActionResult ExportProfileWaitingHireByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileWaitingHireSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileWaitingHireSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileWaitingHire, ref status);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ProfileModel(),
                    FileName = "Hre_Profile",
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

        public ActionResult ExportProfileDisciplineListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_DisciplineSearchModel model)
        {
            var ActionService = new ActionService(UserLogin);
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_DisciplineModel, Hre_DisciplineEntity, Hre_DisciplineSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Discipline, ref status);
            if (result != null && result.Count > 0)
            {
                #region lấy Org và OrgType

                var orgServices = new Cat_OrgStructureServices();
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(null);
                lstObjOrg.Add(1);
                lstObjOrg.Add(int.MaxValue - 1);
                var lstOrg = ActionService.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = ActionService.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();
                #endregion

                foreach (var item in result)
                {
                    Guid? orgId = item.OrgStructureID1;
                    var org = lstOrg.FirstOrDefault(s => s.ID == item.OrgStructureID1);
                    var orgBranch = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_BRANCH, lstOrg, lstOrgType);
                    var orgGroup = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_GROUP, lstOrg, lstOrgType);
                    var orgOrg = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
                    var orgTeam = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_TEAM, lstOrg, lstOrgType);
                    var orgSection = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_SECTION, lstOrg, lstOrgType);
                    var orgDivision = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DIVISION, lstOrg, lstOrgType);

                    item.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    item.GroupName = orgGroup != null ? orgGroup.OrgStructureName : string.Empty;
                    item.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    item.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    item.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    item.DivisionName = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;

                    item.DisciplineCount = result.Where(s => s.ProfileID == item.ProfileID).Count();
                }

            }
            //if (model.IsCreateTemplateForDynamicGrid)
            //{
            //    obj = result;
            //    isDataTable = false;
            //}
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_DisciplineModel(),
                    FileName = "Hre_Discipline",
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
        public ActionResult ExportProfileRewardListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_RewardSearchModel model)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Hre_RewardSearchModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = actionService.GetData<Hre_RewardModel>(lstModel, ConstantSql.hrm_hr_sp_get_Reward, ref status);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_RewardModel(),
                    FileName = "Hre_Reward",
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

        public ActionResult ExportCandidateGeneralListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_CandidateGeneralSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_CandidateGeneralModel, Hre_CandidateGeneralEntity, Hre_CandidateGeneralSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CandidateGeneral, ref status);
            //if (model.IsCreateTemplateForDynamicGrid)
            //{
            //    obj = result;
            //    isDataTable = false;
            //}
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_CandidateGeneralModel(),
                    FileName = "Hre_CandidateGeneral",
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


        //ToLE
        public ActionResult ExportRecCandidateHistoryListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ProfileCandidateHistorySearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_CandidateHistoryModel();
            var result = GetListData<Hre_CandidateHistoryModel, Hre_CandidateHistoryEntity, Hre_ProfileCandidateHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_RecCandidateHistory, ref status);
            if (model.IsCreateTemplateForDynamicGrid)
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
                    FileName = "Hre_CandidateHistoryModel",
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

        #region Gởi Mail cho ưng viên không đạt
        [HttpPost]
        public ActionResult SendMailCandidateFail([DataSourceRequest] DataSourceRequest request, List<Guid> candidateIds)
        {
            var service = new Rec_CandidateServices();
            var result = false;
            if (candidateIds != null && candidateIds.Any())
            {
                result = service.SendMailCandidateFail(candidateIds, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gởi Mail cho ưng viên Tuyển
        [HttpPost]
        public ActionResult SendMailCandidatePass([DataSourceRequest] DataSourceRequest request, List<Guid> candidateIds)
        {
            var service = new Rec_CandidateServices();
            bool result = false;
            if (candidateIds != null && candidateIds.Any())
            {
                result = service.SendMailCandidateFail(candidateIds, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult ExportContractByCombobox(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var ActionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstContract = ActionService.GetData<Hre_ContractEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status);
            if (lstContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
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

                contract.IDDateOfIssueFormat = contract.IDDateOfIssue.HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                if (contract.DateStart != null)
                    contract.DateStartFormat = contract.DateStart.Day.ToString() + "/" + contract.DateStart.Month.ToString() + "/" + contract.DateStart.Year.ToString();
                if (contract.DateEnd.HasValue)
                    contract.DateEndFormat = contract.DateEnd.Value.Day.ToString() + "/" + contract.DateEnd.Value.Month.ToString() + "/" + contract.DateEnd.Value.Year.ToString();
                if (contract.DateHire.HasValue)
                    contract.DateHireFormat = contract.DateHire.Value.Day.ToString() + "/" + contract.DateHire.Value.Month.ToString() + "/" + contract.DateHire.Value.Year.ToString();
                if (contract.DateEndProbation.HasValue)
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.Day.ToString() + "/" + contract.DateEndProbation.Value.Month.ToString() + "/" + contract.DateEndProbation.Value.Year.ToString();
                contract.SalaryFormat = contract.Salary.HasValue ? contract.Salary.Value.ToString("N") : "0";
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
                template = ActionService.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                if (template == null)
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstcontract = new List<Hre_ContractEntity>();
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

        public ActionResult ExportAppendixContractByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var ActionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstAppendixContract = ActionService.GetData<Hre_AppendixContractEntity>(objs, ConstantSql.hrm_hr_sp_get_AppendixContractByListId, ref status);
            if (lstAppendixContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_AppendixContract" + suffix;
            if (lstAppendixContract.Count > 1)
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

            List<object> lstObjAppendixContractType = new List<object>();
            lstObjAppendixContractType.Add(null);
            lstObjAppendixContractType.Add(1);
            lstObjAppendixContractType.Add(int.MaxValue - 1);
            var lstAppendixContractType = ActionService.GetData<Cat_AppendixContractTypeEntity>(lstObjAppendixContractType, ConstantSql.hrm_cat_sp_get_AppendixContractType, ref status);
            foreach (var objAppendixContract in lstAppendixContract)
            {
                var AppendixContractType = lstAppendixContractType.Where(s => s.ID == objAppendixContract.AppendixContractTypeID).FirstOrDefault();
                count++;
                ActionService services = new ActionService(UserLogin);
                List<object> listObj = new List<object>();
                listObj.Add(objAppendixContract.ContractID);
                var ResultEntity = services.GetData<Hre_AppendixContractEntity>(listObj, ConstantSql.hrm_hr_sp_get_AppendixContractByIdContractID, ref status);
                if (ResultEntity != null && ResultEntity.Count > 0)
                {
                    if (ResultEntity.Count > 2)
                    {
                        ResultEntity = ResultEntity.Where(s => s.DateSignedAppendixContract < objAppendixContract.DateSignedAppendixContract).ToList();
                        ResultEntity = ResultEntity.OrderByDescending(s => s.DateSignedAppendixContract).ToList();
                        objAppendixContract.CurrentSalary = ResultEntity[0].Salary;
                        objAppendixContract.CurrentAllowance1 = ResultEntity[0].Allowance1;
                        objAppendixContract.CurrentAllowance2 = ResultEntity[0].Allowance2;
                        objAppendixContract.CurrentAllowance3 = ResultEntity[0].Allowance3;
                        objAppendixContract.CurrentAllowance4 = ResultEntity[0].Allowance4;

                    }
                    else
                    {
                        var ResultProfile = services.GetByIdUseStore<Hre_ContractEntity>(objAppendixContract.ContractID, ConstantSql.hrm_hr_sp_get_ContractById, ref status);
                        if (ResultProfile != null)
                        {
                            objAppendixContract.CurrentSalary = ResultProfile.Salary;
                            objAppendixContract.CurrentAllowance1 = ResultProfile.Allowance1;
                            objAppendixContract.CurrentAllowance2 = ResultProfile.Allowance2;
                            objAppendixContract.CurrentAllowance3 = ResultProfile.Allowance3;
                            objAppendixContract.CurrentAllowance4 = ResultProfile.Allowance4;
                        }
                    }
                }
                if (objAppendixContract.DateSignedAppendixContract.HasValue)
                {
                    objAppendixContract.DateSignedAppendixContractFormat = objAppendixContract.DateSignedAppendixContract.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.DateSignedAppendixContract_Day = objAppendixContract.DateSignedAppendixContract.Value.Day.ToString();
                    objAppendixContract.DateSignedAppendixContract_Month = objAppendixContract.DateSignedAppendixContract.Value.Month.ToString();
                    objAppendixContract.DateSignedAppendixContract_Year = objAppendixContract.DateSignedAppendixContract.Value.Year.ToString();
                    objAppendixContract.PLDateSign = objAppendixContract.DateSignedAppendixContract.Value.ToString("dd-MMM-yyyy");
                }

                if (objAppendixContract.DateofEffect.HasValue)
                {
                    objAppendixContract.DateofEffectFormat = objAppendixContract.DateofEffect.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.DateofEffect_Day = objAppendixContract.DateofEffect.Value.Day.ToString();
                    objAppendixContract.DateofEffect_Month = objAppendixContract.DateofEffect.Value.Month.ToString();
                    objAppendixContract.DateofEffect_Year = objAppendixContract.DateofEffect.Value.Year.ToString();
                    objAppendixContract.PLDateStart = objAppendixContract.DateofEffect.Value.ToString("dd-MMM-yyyy");
                }

                if (objAppendixContract.DateEndAppendixContract.HasValue)
                {
                    objAppendixContract.DateEndAppendixContractFormat = objAppendixContract.DateEndAppendixContract.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.DateEndAppendixContract_Day = objAppendixContract.DateEndAppendixContract.Value.Day.ToString();
                    objAppendixContract.DateEndAppendixContract_Month = objAppendixContract.DateEndAppendixContract.Value.Month.ToString();
                    objAppendixContract.DateEndAppendixContract_Year = objAppendixContract.DateEndAppendixContract.Value.Year.ToString();
                    objAppendixContract.PLDateEnd = objAppendixContract.DateEndAppendixContract.Value.ToString("dd-MMM-yyyy");
                }

                if (objAppendixContract.DateOfBirth.HasValue)
                {
                    objAppendixContract.DayOfBirthFormat = objAppendixContract.DateOfBirth.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.DayOfBirth_Day = objAppendixContract.DateOfBirth.Value.Day.ToString();
                    objAppendixContract.DayOfBirth_Month = objAppendixContract.DateOfBirth.Value.Month.ToString();
                    objAppendixContract.DayOfBirth_Year = objAppendixContract.DateOfBirth.Value.Year.ToString();
                }
                if (objAppendixContract.IDDateOfIssue.HasValue)
                {
                    objAppendixContract.IDDateOfIssueFormat = objAppendixContract.IDDateOfIssue.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.IDDateOfIssue_Day = objAppendixContract.IDDateOfIssue.Value.Day.ToString();
                    objAppendixContract.IDDateOfIssue_Month = objAppendixContract.IDDateOfIssue.Value.Month.ToString();
                    objAppendixContract.IDDateOfIssue_Year = objAppendixContract.IDDateOfIssue.Value.Year.ToString();
                }

                if (objAppendixContract.DateSign.HasValue)
                {
                    objAppendixContract.HDDateSign = objAppendixContract.DateSign.Value.ToString("dd-MMM-yyyy");
                }

                if (objAppendixContract.DateStart.HasValue)
                {
                    objAppendixContract.HDStarDate = objAppendixContract.DateStart.Value.ToString("dd-MMM-yyyy");
                }
                if (objAppendixContract.DateSigned.HasValue)
                {
                    objAppendixContract.DateSignedFormat = objAppendixContract.DateSigned.Value.ToString("dd/MM/yyyy");
                    objAppendixContract.DateSigned_Day = objAppendixContract.DateSigned.Value.Day.ToString();
                    objAppendixContract.DateSigned_Month = objAppendixContract.DateSigned.Value.Month.ToString();
                    objAppendixContract.DateSigned_Year = objAppendixContract.DateSigned.Value.Year.ToString();
                }
                objAppendixContract.ContractSalaryFormat = String.Format("{0:0,0}", objAppendixContract.ContractSalary);
                objAppendixContract.Allowance1Format = String.Format("{0:0,0}", objAppendixContract.Allowance1);
                if (objAppendixContract.Gender == "E_FEMALE")
                {
                    objAppendixContract.GenderView = "Nữ";
                }
                if (objAppendixContract.Gender == "E_MALE")
                {
                    objAppendixContract.GenderView = "Name";
                }

                objAppendixContract.PLSalary = String.Format("{0:0,0}", objAppendixContract.Salary);
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                if (AppendixContractType != null)
                {
                    template = ActionService.GetData<Cat_ExportEntity>(Common.DotNetToOracle(AppendixContractType.ExportID.ToString()), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(objAppendixContract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(objAppendixContract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (count == lstAppendixContract.Count)
                {
                    ExportService.ExportWord(outputPath, templatepath, lstAppendixContract);
                }
            }
            if (lstAppendixContract.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportProfileProbationTemplate(List<Guid> selectedIds, string valueFields)
        {
            ActionService service = new ActionService(UserLogin);
            string[] valueFieldsExportID = valueFields.Split(',');
            valueFields = valueFieldsExportID[0];
            //string _exportID = valueFieldsExportID[1];
            Guid exportID;
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
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
            var lstProfileProbation = service.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileWaitingHireByListId, ref status);

            if (lstProfileProbation == null)
                return null;
            int i = 0;
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_ProfileProbation" + suffix;
            if (lstProfileProbation != null && lstProfileProbation.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var objProfileProbation in lstProfileProbation)
            {
                if (objProfileProbation.DateHire.HasValue)
                {
                    objProfileProbation.DayOfDateHire = objProfileProbation.DateHire.Value.Day;
                    objProfileProbation.MonthOfDateHire = objProfileProbation.DateHire.Value.Month;
                    objProfileProbation.YearOfDateHire = objProfileProbation.DateHire.Value.Year;
                    objProfileProbation.DateHireFormat = objProfileProbation.DateHire.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileProbation.DateEndProbation.HasValue)
                {
                    objProfileProbation.DayOfDateProbation = objProfileProbation.DateEndProbation.Value.Day;
                    objProfileProbation.MonthOfDateProbation = objProfileProbation.DateEndProbation.Value.Month;
                    objProfileProbation.YearOfDateProbation = objProfileProbation.DateEndProbation.Value.Year;
                    objProfileProbation.DateEndProbationFormat = objProfileProbation.DateEndProbation.Value.ToString("dd/MM/yyyy");

                }
                objProfileProbation.Salary = objProfileProbation.Salary.HasValue ? objProfileProbation.Salary.Value : 0;

                objProfileProbation.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                objProfileProbation.DateNow_Day = DateTime.Now.Day.ToString();
                objProfileProbation.DateNow_Month = DateTime.Now.Month.ToString();
                objProfileProbation.DateNow_Year = DateTime.Now.Year.ToString();
                objProfileProbation.DateNow_Hour = DateTime.Now.ToString("HH:mm:ss");
                if (objProfileProbation.DateOfBirth != null)
                    objProfileProbation.DateOfBirthFormat = objProfileProbation.DateOfBirth.Value.ToString("dd/MM/yyyy");
                if (objProfileProbation.IDDateOfIssue != null)
                    objProfileProbation.IDDateOfIssueFormat = objProfileProbation.IDDateOfIssue.Value.ToString("dd/MM/yyyy");

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

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(objProfileProbation.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(objProfileProbation.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilContract = new List<Hre_ProfileEntity>();
                ilContract.Add(objProfileProbation);
                ExportService.ExportWord(outputPath, templatepath, ilContract);
            }
            if (lstProfileProbation != null && lstProfileProbation.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }
        /// <summary>
        /// [Tho.Bui]:Export Reward word templete
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        /// 
        public ActionResult ExporProfileRewardTemplate(List<Guid> selectedIds, string valueFields)
        {
            ActionService service = new ActionService(UserLogin);
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
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
            var lstProfileReward = service.GetData<Hre_RewardEntity>(objs, ConstantSql.hrm_hr_sp_get_TempleteRewardByIds, ref status);
            if (lstProfileReward == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "Reward" + suffix;
            if (lstProfileReward.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var objProfileReward in lstProfileReward)
            {
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

                template = service.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(objProfileReward.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(objProfileReward.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilReward = new List<Hre_RewardEntity>();
                ilReward.Add(objProfileReward);
                ExportService.ExportWord(outputPath, templatepath, ilReward);
            }
            if (lstProfileReward.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }
        /// <summary>
        /// [Tho.Bui]:Export Discipline templete
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        /// 
        public ActionResult ExporProfileDisciplineTemplate(List<Guid> selectedIds, string valueFields)
        {
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;
            ActionService service = new ActionService(UserLogin);

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
            var lstProfileDiscipline = service.GetData<Hre_DisciplineEntity>(objs, ConstantSql.hrm_hr_sp_get_TempleteDisciplineByIds, ref status);
            if (lstProfileDiscipline == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "Discipline" + suffix;
            if (lstProfileDiscipline.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var objProfileDiscipline in lstProfileDiscipline)
            {
                objProfileDiscipline.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                objProfileDiscipline.DateNow_Day = DateTime.Now.Day.ToString();
                objProfileDiscipline.DateNow_Month = DateTime.Now.Month.ToString();
                objProfileDiscipline.DateNow_Year = DateTime.Now.Year.ToString();
                if (objProfileDiscipline.DateOfIssuance.HasValue)
                {
                    objProfileDiscipline.DateOfIssuanceFormat = objProfileDiscipline.DateOfIssuance.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileDiscipline.DateOfViolation.HasValue)
                {
                    objProfileDiscipline.DateOfViolationFormat = objProfileDiscipline.DateOfViolation.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileDiscipline.DateOfEffective.HasValue)
                {
                    objProfileDiscipline.DateOfEffectiveFormat = objProfileDiscipline.DateOfEffective.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileDiscipline.DateHire.HasValue)
                {
                    objProfileDiscipline.DateHireFormat = objProfileDiscipline.DateHire.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileDiscipline.DateQuit.HasValue)
                {
                    if (objProfileDiscipline.DateHire.HasValue)
                    {
                        objProfileDiscipline.MonthWorking = Math.Floor(objProfileDiscipline.DateQuit.Value.Subtract(objProfileDiscipline.DateHire.Value).TotalDays / 30);
                        if (objProfileDiscipline.MonthWorking.HasValue)
                            objProfileDiscipline.YearWorking = Math.Floor(objProfileDiscipline.MonthWorking.Value / 12);
                        if (objProfileDiscipline.YearWorking > 0)
                        {
                            objProfileDiscipline.MonthWorking = objProfileDiscipline.MonthWorking - (objProfileDiscipline.YearWorking * 12);
                        }
                    }
                }
                else
                {
                    if (objProfileDiscipline.DateHire.HasValue)
                        objProfileDiscipline.MonthWorking = Math.Floor(DateTime.Now.Subtract(objProfileDiscipline.DateHire.Value).TotalDays / 30);
                    if (objProfileDiscipline.MonthWorking.HasValue)
                        objProfileDiscipline.YearWorking = Math.Floor(objProfileDiscipline.MonthWorking.Value / 12);
                    if (objProfileDiscipline.YearWorking > 0)
                    {
                        objProfileDiscipline.MonthWorking = objProfileDiscipline.MonthWorking - (objProfileDiscipline.YearWorking * 12);
                    }
                }
                //if (objProfileReward.DateHire.HasValue)
                //{
                //    objProfileReward.DayOfDateHire = objProfileReward.DateHire.Value.Day;
                //    objProfileReward.MonthOfDateHire = objProfileReward.DateHire.Value.Month;
                //    objProfileReward.YearOfDateHire = objProfileReward.DateHire.Value.Year;
                //}
                //if (objProfileReward.DateEndProbation.HasValue)
                //{
                //    objProfileReward.DayOfDateProbation = objProfileReward.DateEndProbation.Value.Day;
                //    objProfileReward.MonthOfDateProbation = objProfileReward.DateEndProbation.Value.Month;
                //    objProfileReward.YearOfDateProbation = objProfileReward.DateEndProbation.Value.Year;
                //}
                //objProfileReward.Salary = objProfileReward.Salary.HasValue ? objProfileReward.Salary.Value : 0;
                //objProfileReward.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
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
                    template = service.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                }

                if (template == null)
                {
                    continue;
                }

                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(objProfileDiscipline.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(objProfileDiscipline.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilDiscipline = new List<Hre_DisciplineEntity>();
                ilDiscipline.Add(objProfileDiscipline);
                ExportService.ExportWord(outputPath, templatepath, ilDiscipline);
            }
            if (lstProfileDiscipline.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult ExportWordProfileWaitingHireByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            DateTime DateStart = DateTime.Now;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }

            ActionService baseService = new ActionService(UserLogin);
            string status = string.Empty;
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstProfileWaitingHire = baseService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileWaitingHireByListId, ref status);
            if (lstProfileWaitingHire == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_ProfileWaitingHire" + suffix;
            if (lstProfileWaitingHire.Count > 1)
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

            var lstcandidateGeneralByProfileIDs = baseService.GetData<Hre_CandidateGeneralEntity>(strIDs, ConstantSql.hrm_hr_sp_get_CandidateGeneralByProfileIDs, ref status);
            #region ds nguoi than cua nhan vien duoc chon
            string strProfileIDs = "";
            if (selectedIds != null)
            {
                foreach (var ProfileID in selectedIds)
                {
                    string _strProfileID = Common.DotNetToOracle(ProfileID.ToString());
                    strProfileIDs += _strProfileID + ",";
                }
            }
            var lstRelativeByProfileIDs = baseService.GetData<Hre_RelativesEntity>(strProfileIDs, ConstantSql.hrm_hr_sp_get_RelativeByProfileIds, ref status);
            #endregion

            foreach (var objProfileWaitingHire in lstProfileWaitingHire)
            {
                var candidateGeneralByProfile = lstcandidateGeneralByProfileIDs.Where(s => s.ProfileID == objProfileWaitingHire.ID).FirstOrDefault();
                if (objProfileWaitingHire.DateHire.HasValue)
                {
                    objProfileWaitingHire.DateHireFormat = objProfileWaitingHire.DateHire.Value.ToString("dd/MM/yyyy");
                    objProfileWaitingHire.DayOfDateHire = objProfileWaitingHire.DateHire.Value.Day;
                    objProfileWaitingHire.MonthOfDateHire = objProfileWaitingHire.DateHire.Value.Month;
                    objProfileWaitingHire.YearOfDateHire = objProfileWaitingHire.DateHire.Value.Year;
                    objProfileWaitingHire.DateHireFormatEN = objProfileWaitingHire.DateHire.Value.ToString("dd-MMM-yyyy");
                }
                if (objProfileWaitingHire.DateEndProbation.HasValue)
                {
                    objProfileWaitingHire.DateEndProbationFormat = objProfileWaitingHire.DateEndProbation.Value.ToString("dd/MM/yyyy");
                    objProfileWaitingHire.DayOfDateProbation = objProfileWaitingHire.DateEndProbation.Value.Day;
                    objProfileWaitingHire.MonthOfDateProbation = objProfileWaitingHire.DateEndProbation.Value.Month;
                    objProfileWaitingHire.YearOfDateProbation = objProfileWaitingHire.DateEndProbation.Value.Year;
                    objProfileWaitingHire.DateEndProbationFormatEN = objProfileWaitingHire.DateEndProbation.Value.ToString("dd-MM-yyyy");
                }

                objProfileWaitingHire.DateNow = DateTime.Now.ToString("dd/MM/yyyy");

                if (objProfileWaitingHire.Gender != null)
                {
                    if (objProfileWaitingHire.Gender == "E_MALE")
                    {
                        objProfileWaitingHire.NameByGerder = "Anh";
                    }
                    else if (objProfileWaitingHire.Gender == "E_FEMALE")
                    {
                        objProfileWaitingHire.NameByGerder = "Chị";
                    }
                }
                if (objProfileWaitingHire.Gender == "E_FEMALE")
                {
                    //objProfileWaitingHire.GenderView = "Ms." + objProfileWaitingHire.ProfileName.Substring(objProfileWaitingHire.ProfileName.LastIndexOf(' '));
                    objProfileWaitingHire.GenderView = "Ms." + objProfileWaitingHire.ProfileName != null ? objProfileWaitingHire.ProfileName.Trim() : string.Empty;
                }
                else if (objProfileWaitingHire.Gender == "E_FEMALE")
                {
                    //objProfileWaitingHire.GenderView = "Mr." + objProfileWaitingHire.ProfileName.Substring(objProfileWaitingHire.ProfileName.LastIndexOf(' '));
                    objProfileWaitingHire.GenderView = "Mr." + objProfileWaitingHire.ProfileName != null ? objProfileWaitingHire.ProfileName.Trim() : string.Empty;
                }
                objProfileWaitingHire.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                objProfileWaitingHire.DateNow_Day = DateTime.Now.Day.ToString();
                objProfileWaitingHire.DateNow_Month = DateTime.Now.Month.ToString();
                objProfileWaitingHire.DateNow_Year = DateTime.Now.Year.ToString();
                objProfileWaitingHire.SalaryFormat = String.Format("{0:0,0}", objProfileWaitingHire.Salary);
                // Lấy thông tin HĐ
                if (objProfileWaitingHire.Allowance1 != null)
                    objProfileWaitingHire.Allowance1Format = String.Format("{0:0,0}", objProfileWaitingHire.Allowance1);
                if (objProfileWaitingHire.Allowance2 != null)
                    objProfileWaitingHire.Allowance2Format = String.Format("{0:0,0}", objProfileWaitingHire.Allowance2);
                if (objProfileWaitingHire.Allowance3 != null)
                    objProfileWaitingHire.Allowance3Format = String.Format("{0:0,0}", objProfileWaitingHire.Allowance3);
                if (objProfileWaitingHire.Allowance4 != null)
                    objProfileWaitingHire.Allowance4Format = String.Format("{0:0,0}", objProfileWaitingHire.Allowance4);
                objProfileWaitingHire.AllowanceID1Name = candidateGeneralByProfile != null ? candidateGeneralByProfile.AllowanceID1Name : null;
                objProfileWaitingHire.AllowanceID2Name = candidateGeneralByProfile != null ? candidateGeneralByProfile.AllowanceID2Name : null;
                objProfileWaitingHire.AllowanceID3Name = candidateGeneralByProfile != null ? candidateGeneralByProfile.AllowanceID3Name : null;
                objProfileWaitingHire.AllowanceID4Name = candidateGeneralByProfile != null ? candidateGeneralByProfile.AllowanceID5Name : null;
                if (objProfileWaitingHire.DateOfBirth != null)
                {
                    objProfileWaitingHire.DateOfBirthFormat = objProfileWaitingHire.DateOfBirth.Value.ToString("dd/MM/yyyy");
                }
                if (objProfileWaitingHire.SalaryClassID != null)
                {
                    var rankdetailbyProfile = baseService.GetData<Cat_SalaryRankEntity>(Common.DotNetToOracle(objProfileWaitingHire.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_SalaryRankBySalaryClassId, ref status).FirstOrDefault();
                    if (rankdetailbyProfile != null)
                    {
                        objProfileWaitingHire.Salary = rankdetailbyProfile.SalaryStandard;
                    }
                }
                var lstunusualallowancebyprofile = baseService.GetData<Sal_UnusualAllowanceEntity>(objProfileWaitingHire.ID, ConstantSql.hrm_sal_sp_get_UnusualAllowanceByProfileid, ref status);
                if (lstunusualallowancebyprofile != null)
                {
                    var PhuCapvung = lstunusualallowancebyprofile.Where(s => s.UnusualEDTypeCode == "PhuCapVung").ToList().FirstOrDefault();
                    var PhucapConNho = lstunusualallowancebyprofile.Where(s => s.UnusualEDTypeCode == "ChildCare").ToList().FirstOrDefault();
                    objProfileWaitingHire.PCArea = PhuCapvung.Amount;
                    objProfileWaitingHire.PCConNho = PhucapConNho.Amount;
                }
                //var contractbyprofile = baseService.GetData<Hre_ContractEntity>(Common.DotNetToOracle(objProfileWaitingHire.ID.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
                //if(contractbyprofile != null)
                //{
                //    var contractEntity = contractbyprofile.OrderByDescending(s => s.DateCreate).Select(s => new {s.AllowanceID1, s.AllowanceID2, s.AllowanceID3, s.AllowanceID4}).ToList();
                //    if(contractEntity != null)
                //    {

                //        var allowance1 = listUsualAllowance.Where(s => contractEntity.Contains(s.ID) && s.Code == "").FirstOrDefault();
                //    }
                //}
                ActionService service = new ActionService(UserLogin);
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(objProfileWaitingHire.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(objProfileWaitingHire.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilContract = new List<Hre_ProfileEntity>();
                ilContract.Add(objProfileWaitingHire);

                #region lay nguoi than cua tung nhan vien
                List<Hre_RelativesEntity> lstRelativeByProfileID = new List<Hre_RelativesEntity>();
                if (lstRelativeByProfileIDs != null)
                {
                    lstRelativeByProfileID = lstRelativeByProfileIDs.Where(s => s.ProfileID == objProfileWaitingHire.ID).ToList();
                }

                #endregion

                var lstcontract = new List<Hre_ProfileEntity>();
                lstcontract.Add(objProfileWaitingHire);
                Hre_ProfileEntity objProfile = new Hre_ProfileEntity();
                DataTable tblProfiles = new DataTable();
                tblProfiles = lstcontract.Translate();
                DataTable tblRelatives = new DataTable();
                tblRelatives = lstRelativeByProfileID.Translate();
                DataSet dsData = new DataSet();
                dsData.Tables.Add(tblProfiles);
                dsData.Tables.Add(tblRelatives);
                dsData.Tables[0].TableName = "tblProfiles";
                dsData.Tables[1].TableName = "tblRelatives";

                //ExportService.ExportWord(outputPath, templatepath, dsData);
                ExportService.ExportWithRegions(outputPath, templatepath, dsData);
            }

            if (lstProfileWaitingHire.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        #endregion

        #region Hre_Accident

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu cho bảng Tai Nạn (Hre_Accident) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetAccidentList([DataSourceRequest] DataSourceRequest request, Hre_AccidentSearchModel model)
        {
            return GetListDataAndReturn<Hre_AccidentModel, Hre_AccidentEntity, Hre_AccidentSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Accident);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất các dòng dữ liệu được chọn của bảng Tai Nạn (Hre_Accident) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportAccidentSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_AccidentEntity, Hre_AccidentModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_AccidentByIds);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất danh sách dữ liệu cho Nhân Viên (Hre_Profile) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAccidentList([DataSourceRequest] DataSourceRequest request, Hre_AccidentSearchModel model)
        {
            return ExportAllAndReturn<Hre_AccidentEntity, Hre_AccidentModel, Hre_AccidentSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Accident);
        }

        /// <summary>
        ///[Tho.Bui] Get Discipline theo profileid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="profileID"></param>
        /// <returns></returns>
        public ActionResult GetAccidentProID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var ActionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = ActionService.GetData<Hre_AccidentEntity>(objs, ConstantSql.hrm_hr_sp_get_AccidentprofileId, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }

        public ActionResult ExportGetAccidentListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_AccidentSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_AccidentModel, Hre_AccidentEntity, Hre_AccidentSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Accident, ref status);
            //if (model.IsCreateTemplateForDynamicGrid)
            //{
            //    obj = result;
            //    isDataTable = false;
            //}
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_AccidentModel(),

                    FileName = "Hre_Accident",
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

        #region Language
        public ActionResult GetLanguageProID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var ActionService = new ActionService(UserLogin);
            var result = ActionService.GetData<Hre_ProfileLanguageLevelModel>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_LanguageprofileId, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult GetMultiLanguageType(string text)
        {
            return GetDataForControl<CatLanguageTypeMultiModel, Cat_LanguageTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LanguageType_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        public JsonResult GetMultiLanguageLevel(string text)
        {
            return GetDataForControl<CatLanguageLevelMultiModel, Cat_LanguageLevelMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LanguageLevel_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        public JsonResult GetMultiLanguageSkill(string text)
        {
            return GetDataForControl<CatLanguageSkillMultiModel, Cat_LanguageSkillMultiEntity>(text, ConstantSql.hrm_cat_sp_get_LanguageSkill_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        #endregion

        #region Hre_Qualification
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho trình độ chuyên môn (Hre_Qualification) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQualificationList([DataSourceRequest] DataSourceRequest request, Hre_ProfileQualificationModelSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileQualificationModel, Hre_ProfileQualificationEntity, Hre_ProfileQualificationModelSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQualification);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho trình độ chuyên môn (Hre_Qualification) theo điều kiện tìm kiếm
        /// </summary>   
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportQualificationList([DataSourceRequest] DataSourceRequest request, Hre_ProfileQualificationModelSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileQualificationEntity, Hre_ProfileQualificationModel, Hre_ProfileQualificationModelSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileQualification);
        }

        /// <summary> 
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của trình độ chuyên môn (Hre_Qualification) ra file Excel 
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportQualificationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_QualificationEntity, Hre_QualificationModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_QualificationByIds);
        }

        public ActionResult ExportProfileQualificationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_ProfileQualificationEntity, Hre_ProfileQualificationModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_ProfileQualificationByIds);
        }


        public ActionResult GetQualificationID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var ActionService = new ActionService(UserLogin);
            var result = ActionService.GetData<Hre_ProfileQualificationModel>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_QualificationByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Hre_Address
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Adress (Hre_Address) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAddressList([DataSourceRequest] DataSourceRequest request, Hre_AddressSearchModel model)
        {
            return GetListDataAndReturn<Hre_AddressModel, Hre_AddressEntity, Hre_AddressSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Address);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Adress(Hre_Address) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAddressList([DataSourceRequest] DataSourceRequest request, Hre_AddressSearchModel model)
        {
            return ExportAllAndReturn<Hre_AddressEntity, Hre_AddressModel, Hre_AddressSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Address);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Adress (Hre_Address) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportAddressSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_AddressEntity, Hre_AddressModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_AddressByIds);
        }

        #endregion

        #region Hre_CardHistory
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho CardHistory (Hre_CardHistory) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCardHistoryList([DataSourceRequest] DataSourceRequest request, Hre_CardHistorySearchModel model)
        {
            return GetListDataAndReturn<Hre_CardHistoryModel, Hre_CardHistoryEntity, Hre_CardHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CardHistory);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho CardHistory(Hre_CardHistory) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportCardHistoryList([DataSourceRequest] DataSourceRequest request, Hre_CardHistorySearchModel model)
        {
            return ExportAllAndReturn<Hre_CardHistoryEntity, Hre_CardHistoryModel, Hre_CardHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CardHistory);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của CardHistory (Hre_CardHistory) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportCardHistorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_CardHistoryEntity, Hre_CardHistoryModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_CardHistoryByIds);
        }

        public ActionResult GetCardHistoryByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var ActionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = ActionService.GetData<Hre_CardHistoryEntity>(objs, ConstantSql.hrm_hr_sp_get_CardHistorysByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }


        #endregion

        #region Hre_ContractWaiting
        public ActionResult GetContractWaitingList([DataSourceRequest] DataSourceRequest request, Hre_ContractWaitingSearchModel model)
        {
            return GetListDataAndReturn<Hre_ContractModel, Hre_ContractEntity, Hre_ContractWaitingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractWaiting);
        }
        public ActionResult ExportAllContractWaitingList([DataSourceRequest] DataSourceRequest request, Hre_ContractWaitingSearchModel model)
        {
            var contractServices = new Hre_ContractServices();
            string status = string.Empty;
            var result = GetListData<Hre_ContractEntity, Hre_ContractModel, Hre_ContractWaitingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractWaiting, ref status);
            DataTable tableData = contractServices.GetDataContract(result, UserLogin);
            var isDataTable = false;

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tableData,
                    FileName = "Hre_ContractEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, tableData, null, model.ExportType);

                return Json(fullPath);
            }
            var listModel = new List<Hre_ContractModel>();
            if (result != null)
            {
                request.Page = 1;
                foreach (var item in result)
                {
                    var newModle = (Hre_ContractModel)typeof(Hre_ContractModel).CreateInstance();
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
            return Json(result.ToDataSourceResult(request));

        }
        #endregion

        #region Hre_EvaluationContractWaitingApproved
        [HttpPost]
        public ActionResult GetEvaluationContractWaitingList([DataSourceRequest] DataSourceRequest request, Hre_EvaluationContractWaitingApprovedSearchModel model)
        {
            int _page = request.Page;
            var service = new Hre_ReportServices();
            var ActionService = new ActionService(UserLogin);
            var contractServices = new Hre_ContractServices();
            string status = string.Empty;
            request.Page = 1;
            request.PageSize = int.MaxValue - 1;
            var result = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_EvaluationContractWaitingApprovedSearchModel>(request, model,
                ConstantSql.hrm_hr_sp_get_EvaluationContractWaitingApprove, ref status);
            if (result != null)
            {
                result = result.Where(s => s.StatusEvaluation != "E_APPROVED" && s.ContractResult != null).ToList();
            }
            var lstObjContractType = new List<object>();
            lstObjContractType.AddRange(new object[6]);
            lstObjContractType[4] = 1;
            lstObjContractType[5] = int.MaxValue - 1;
            var lstContractType = ActionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status);

            if (model.IsMissingInformation == true)
            {
                result = result.Where(s => s.DateEndNextContract == null).ToList();
            }

            var lstContractEntity = result.Translate<Hre_ContractEntity>();
            DataTable tableData = contractServices.GetDataContract(lstContractEntity, UserLogin);
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateStart == null ? DateTime.Now : model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateEnd == null ? DateTime.Now : model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = tableData;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ContractEntity",
                    OutPutPath = path,
                    HeaderInfo=listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, tableData, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            var listModel = new List<Hre_ContractModel>();
            if (result != null)
            {
                request.Page = _page;
                foreach (var item in result)
                {
                    var newModle = (Hre_ContractModel)typeof(Hre_ContractModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = result.Count;
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            return Json(result.ToDataSourceResult(request));

        }
        #endregion

        #region DS hđ kết thúc
        public ActionResult GetContractEndList([DataSourceRequest] DataSourceRequest request, Hre_ContractEndSearchModel model)
        {
            return GetListDataAndReturn<Hre_ContractModel, Hre_ContractEntity, Hre_ContractEndSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractEnd);
        }
        public ActionResult ExportAllContractEndList([DataSourceRequest] DataSourceRequest request, Hre_ContractEndSearchModel model)
        {
            Hre_ContractServices ActionServices = new Hre_ContractServices();
            string status = string.Empty;
            var result = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_ContractEndSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractEnd, ref status);
            var lstEntity = result.Translate<Hre_ContractEntity>();
            DataTable tb = ActionServices.GetDataContract(lstEntity, UserLogin);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tb,
                    FileName = "Hre_ContractEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, tb, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Hre_Contract
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Contract (Hre_Contract) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContractList([DataSourceRequest] DataSourceRequest request, Hre_ContractSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;

            var result = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_ContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractList, ref status);
            if (model.IsLastestContract == true && result != null)
            {
                var lstProfileID = result.Select(s => s.ProfileID).Distinct().ToList();
                if (lstProfileID.Count > 0)
                {
                    result = result.Where(s => lstProfileID.Contains(s.ProfileID)).OrderByDescending(s => s.DateStart).DistinctBy(s => s.ProfileID).ToList();
                }
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ContractModel(),
                    FileName = "Hre_Contract",
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

            var listModel = new List<Hre_ContractModel>();
            if (result != null)
            {
                request.Page = 1;
                foreach (var item in result)
                {
                    var newModle = (Hre_ContractModel)typeof(Hre_ContractModel).CreateInstance();
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
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult ExportContractListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ContractSearchModel model)
        {
            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var result = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_ContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractList, ref status);
            DataTable tb = new DataTable();
            var actionServices = new ActionService(UserLogin);
            var _ConstractSearchModel = new Hre_ContractSearchModel();
            var _lsContractAll = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_ContractSearchModel>(request, _ConstractSearchModel, ConstantSql.hrm_hr_sp_get_Contract, ref status);

            var Lstobj = new List<object>();
            Lstobj.AddRange(new object[18]);
            Lstobj[16] = 1;
            Lstobj[17] = (int.MaxValue - 1);
            var _profileAll = actionServices.GetData<Hre_ProfileEntity>(Lstobj, ConstantSql.hrm_hr_sp_get_Profile, ref status);

            #region lay thong tin hop dong
            for (int i = 0; i < result.Count; i++)
            {
                #region Lay thong tin Profile
                var _profile = _profileAll.Where(s => s.ID == result[i].ProfileID && result[i].ProfileID != null).FirstOrDefault();
                if (_profile != null)
                {
                    if (_profile.PAddress != null)
                        result[i].PAddress = _profile.PAddress;

                    if (_profile.PDistrictName != null)
                        result[i].PDistrictName = _profile.PDistrictName;

                    if (_profile.PCountryName != null)
                        result[i].PCountryName = _profile.PCountryName;

                    if (_profile.PProvinceName != null)
                        result[i].PProvinceName = _profile.PProvinceName;

                    if (result[i].ProfileName != null)
                    {
                        var _profilename = result[i].ProfileName.Split(' ');
                        if (_profilename.Count() == 1)
                        {
                            result[i].FristName = result[i].ProfileName;
                        }
                        else
                        {
                            if (_profilename.Count() == 2)
                            {
                                result[i].FristName = _profilename[0];
                                result[i].LastName = _profilename[1];
                            }
                            else
                            {
                                result[i].FristName = _profilename[0];
                                result[i].LastName = _profilename[_profilename.Count() - 1];
                                for (int j = 1; j < _profilename.Count() - 1; j++)
                                {
                                    result[i].TenDem = result[i].TenDem + _profilename[j];
                                }
                            }

                        }
                    }
                }
                #endregion
                #region Lay thong tin hopn dong truoc do
                var _lsContract = _lsContractAll.Where(s => s.ID == result[i].ID && result[i].ID != null).ToList();
                if (_lsContract != null && _lsContract.Count >= 2)
                {
                    _lsContract = _lsContract.Where(m => m.DateSigned <= result[i].DateSigned).ToList();
                    if (_lsContract.Count >= 2)
                    {
                        Hre_ContractModel _contract = new Hre_ContractModel();
                        _lsContract = _lsContract.OrderByDescending(m => m.DateSigned).ToList();
                        _contract = _lsContract[0];
                        if (_contract.DateStart != null)
                        {
                            result[i].DateStartOld = _contract.DateStart;
                            result[i].DateStartOld_Day = _contract.DateStart.Day.ToString();
                            result[i].DateStartOld_Month = _contract.DateStart.Month.ToString();
                            result[i].DateStartOld_Year = _contract.DateStart_Year.ToString();
                        }
                        if (_contract.DateEnd != null)
                        {
                            result[i].DateEndOld = _contract.DateEnd;
                            result[i].DateEndOld_Day = _contract.DateEnd.Value.Day.ToString();
                            result[i].DateEndOld_Month = _contract.DateEnd.Value.Month.ToString();
                            result[i].DateEndOld_Year = _contract.DateEnd.Value.Year.ToString();
                        }
                        if (_contract.DateSigned != null)
                        {
                            result[i].DateSignedOld = _contract.DateSigned;
                            result[i].DateSignedOld_Day = _contract.DateSigned.Value.Day.ToString();
                            result[i].DateSignedOld_Month = _contract.DateSigned.Value.Month.ToString();
                            result[i].DateSignedOld_Year = _contract.DateSigned.Value.Year.ToString();
                        }
                    }

                }
                #endregion
            }

            #endregion


            var lstEntity = result.Translate<Hre_ContractEntity>();
            tb = contractServices.GetDataContract(lstEntity, UserLogin);

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tb,
                    FileName = "Hre_Contract",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstEntity, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Contract (Hre_Contract) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportContractList([DataSourceRequest] DataSourceRequest request, Hre_ContractSearchModel model)
        {
            return ExportAllAndReturn<Hre_ContractEntity, Hre_ContractModel, Hre_ContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractList);

        }


        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Contract (Hre_Contract) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportContractSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_ContractEntity, Hre_ContractModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_ContractByIds);
        }

        public ActionResult GetContractByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var ActionService = new ActionService(UserLogin);
            var result = ActionService.GetData<Hre_ContractEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ExportContractByTemplate(List<Guid> selectedIds, string valueFields)
        {
            var _actionService = new ActionService(UserLogin);
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstContract = _actionService.GetData<Hre_ContractEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status);
            if (lstContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
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
                if (contract.NoPrint == null)
                {
                    contract.NoPrint = 1;
                }
                else
                {
                    contract.NoPrint++;
                }

                if (contract.PassportDateOfIssue.HasValue)
                {
                    contract.PassportDateOfIssueFormatEN = contract.PassportDateOfIssue.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.PassportDateOfExpiry.HasValue)
                {
                    contract.PassportDateOfExpiryFormatEN = contract.PassportDateOfExpiry.Value.ToString("dd-MMM-yyyy");
                }

                if (contract.TerminateDate.HasValue)
                {
                    contract.TerminateDateFormatEN = contract.TerminateDate.Value.ToString("dd-MMM-yyyy");
                }

                contract.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                contract.DateNow_Day = DateTime.Now.Day.ToString();
                contract.DateNow_Month = DateTime.Now.Month.ToString();
                contract.DateNow_Year = DateTime.Now.Year.ToString();
                contract.IDDateOfIssueFormat = contract.IDDateOfIssue.HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                if (contract.DateStart != null)
                {
                    contract.DateStartFormat = contract.DateStart.ToString("dd/MM/yyyy");
                    contract.DateStart_Day = contract.DateStart.Day.ToString();
                    contract.DateStart_Month = contract.DateStart.Month.ToString();
                    contract.DateStart_Year = contract.DateStart.Year.ToString();
                    contract.DateStartFormatEN = contract.DateStart.ToString("dd-MMM-yyyy");
                }
                if (contract.DateEnd.HasValue)
                {
                    contract.DateEndFormat = contract.DateEnd.Value.ToString("dd/MM/yyyy");
                    contract.DateEnd_Day = contract.DateEnd.Value.Day.ToString();
                    contract.DateEnd_Month = contract.DateEnd.Value.Month.ToString();
                    contract.DateEnd_Year = contract.DateEnd.Value.Year.ToString();
                    contract.DateEndFormatEN = contract.DateEnd.Value.ToString("dd-MMM-yyyy");
                }

                if (contract.DateSigned.HasValue)
                {
                    contract.DateSignedFormat = contract.DateSigned.Value.ToString("dd/MM/yyyy");
                    contract.DateSigned_Day = contract.DateSigned.Value.Day.ToString();
                    contract.DateSigned_Month = contract.DateSigned.Value.Month.ToString();
                    contract.DateSigned_Year = contract.DateSigned.Value.Year.ToString();
                    contract.DateSignedFormatEN = contract.DateSigned.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.DateEndProbation.HasValue)
                {
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.ToString("dd/MM/yyyy");
                    contract.DateEndProbation_Day = contract.DateEndProbation.Value.Day.ToString();
                    contract.DateEndProbation_Month = contract.DateEndProbation.Value.Month.ToString();
                    contract.DateEndProbation_Year = contract.DateEndProbation.Value.Year.ToString();
                    contract.DateEndProbationFormatEN = contract.DateEndProbation.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.DateOfBirth.HasValue)
                {
                    contract.DateSignedFormat = contract.DateOfBirth.Value.ToString("dd/MM/yyyy");
                    contract.DateOfBirth_Day = contract.DateOfBirth.Value.Day.ToString();
                    contract.DateOfBirth_Month = contract.DateOfBirth.Value.Month.ToString();
                    contract.DateOfBirth_Year = contract.DateOfBirth.Value.Year.ToString();
                    contract.DateOfBirthFormatEN = contract.DateOfBirth.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.TerminateDate.HasValue)
                {
                    contract.TerminateDateFormat = contract.TerminateDate.Value.ToString("dd/MM/yyyy");
                }
                if (contract.DateHire.HasValue)
                {
                    contract.DateHireFormat = contract.DateHire.Value.ToString("dd/MM/yyyy");
                    contract.DateHire_Day = contract.DateHire.Value.Day.ToString();
                    contract.DateHire_Month = contract.DateHire.Value.Month.ToString();
                    contract.DateHire_Year = contract.DateHire.Value.Year.ToString();
                    contract.DateOfBirthFormatEN = contract.DateHire.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.DateEndProbation.HasValue)
                {
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.ToString("dd/MM/yyyy");
                }
                contract.SalaryFormat = String.Format("{0:0,0}", contract.Salary);
                if (contract.DateOfEffect.HasValue)
                {
                    contract.DateOfEffectFormat = contract.DateOfEffect.Value.ToString("dd/MM/yyyy");
                    contract.DateOfEffectFormatEng = contract.DateOfEffect.Value.ToString("MMM dd,yyyy");

                    contract.DateOfEffectMoreTwoMonthFormat = contract.DateOfEffect.Value.AddMonths(+2).ToString("dd MMM yyyy");
                }
                if (contract.Gender == "E_FEMALE")
                {
                    contract.GraveName = "Ms." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                else
                {
                    contract.GraveName = "Mr." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                if (contract.Gender == "E_FEMALE")
                {
                    contract.GenderView = "Chị";
                }
                if (contract.Gender == "E_MALE")
                {
                    contract.GenderView = "Anh";
                }
                if (contract.DateQuit.HasValue)
                {
                    if (contract.DateHire.HasValue)
                    {
                        contract.MonthWorking = Math.Floor(contract.DateQuit.Value.Subtract(contract.DateHire.Value).TotalDays / 30);
                        contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                        if (contract.YearWorking > 0)
                        {
                            contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                        }
                    }
                }
                else
                {
                    contract.MonthWorking = Math.Floor(DateTime.Now.Subtract(contract.DateHire.Value).TotalDays / 30);
                    contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                    if (contract.YearWorking > 0)
                    {
                        contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                    }
                }
                contractServices.Edit(contract);
                ActionService service = new ActionService(UserLogin);
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                if (contract.ExportID.HasValue)
                    template = _actionService.GetData<Cat_ExportEntity>(Common.DotNetToOracle(contract.ExportID.Value.ToString()), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstcontract = new List<Hre_ContractEntity>();
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

        public ActionResult ExportContractAllByTemplate([DataSourceRequest] DataSourceRequest request, Hre_ContractSearchModel model)
        {
            var contractServices = new Hre_ContractServices();
            var actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            var lstContractAll = GetListData<Hre_ContractModel, Hre_ContractEntity, Hre_ContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractList, ref status);


            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);



            var objs = new List<object>();
            string strIDs = string.Empty;
            //foreach (var item in selectedIds)
            //{
            //    strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            //}
            //if (strIDs.IndexOf(",") > 0)
            //    strIDs = strIDs.Substring(0, strIDs.Length - 1);
            //objs.Add(strIDs);
            //var lstContract = baseService.GetData<Hre_ContractEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status);
            if (lstContractAll == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstContractAll.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var contract in lstContractAll)
            {
                if (contract.NoPrint == null)
                {
                    contract.NoPrint = 1;
                }
                else
                {
                    contract.NoPrint++;
                }
                contract.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                contract.DateNow_Day = DateTime.Now.Day.ToString();
                contract.DateNow_Month = DateTime.Now.Month.ToString();
                contract.DateNow_Year = DateTime.Now.Year.ToString();
                contract.IDDateOfIssueFormat = contract.IDDateOfIssue.HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                if (contract.DateStart != null)
                {
                    contract.DateStartFormat = contract.DateStart.ToString("dd/MM/yyyy");
                    contract.DateStart_Day = contract.DateStart.Day.ToString();
                    contract.DateStart_Month = contract.DateStart.Month.ToString();
                    contract.DateStart_Year = contract.DateStart.Year.ToString();
                }
                if (contract.DateEnd.HasValue)
                {
                    contract.DateEndFormat = contract.DateEnd.Value.ToString("dd/MM/yyyy");
                    contract.DateEnd_Day = contract.DateStart.Day.ToString();
                    contract.DateEnd_Month = contract.DateStart.Month.ToString();
                    contract.DateEnd_Year = contract.DateStart.Year.ToString();
                }

                if (contract.DateSigned.HasValue)
                {
                    contract.DateSignedFormat = contract.DateSigned.Value.ToString("dd/MM/yyyy");
                    contract.DateSigned_Day = contract.DateSigned.Value.Day.ToString("dd/MM/yyyy");
                    contract.DateSigned_Month = contract.DateSigned.Value.Month.ToString("dd/MM/yyyy");
                    contract.DateSigned_Year = contract.DateSigned.Value.Year.ToString("dd/MM/yyyy");
                }

                if (contract.DateHire.HasValue)
                {
                    contract.DateHireFormat = contract.DateHire.Value.ToString("dd/MM/yyyy");
                }
                if (contract.DateEndProbation.HasValue)
                {
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.ToString("dd/MM/yyyy");
                }
                contract.SalaryFormat = contract.Salary.HasValue ? contract.Salary.Value.ToString("N") : "0";
                if (contract.DateOfEffect.HasValue)
                {
                    contract.DateOfEffectFormat = contract.DateOfEffect.Value.ToString("dd MMM yyyy");
                    contract.DateOfEffectMoreTwoMonthFormat = contract.DateOfEffect.Value.AddMonths(+2).ToString("dd MMM yyyy");
                }
                if (contract.Gender == "E_FEMALE")
                {
                    contract.GraveName = "Ms." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                else
                {
                    contract.GraveName = "Mr." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                if (contract.DateQuit.HasValue)
                {
                    if (contract.DateHire.HasValue)
                    {
                        contract.MonthWorking = Math.Floor(contract.DateQuit.Value.Subtract(contract.DateHire.Value).TotalDays / 30);
                        contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                        if (contract.YearWorking > 0)
                        {
                            contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                        }
                    }
                }
                else
                {
                    if (contract.DateHire.HasValue)
                    {
                        contract.MonthWorking = Math.Floor(DateTime.Now.Subtract(contract.DateHire.Value).TotalDays / 30);
                        contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                        if (contract.YearWorking > 0)
                        {
                            contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                        }
                    }

                }
                contractServices.Edit(contract);

                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                //List<object> lstObjExport = new List<object>();
                //lstObjExport.Add(null);
                //lstObjExport.Add(null);
                //lstObjExport.Add(null);
                //lstObjExport.Add(null);
                //lstObjExport.Add(1);
                //lstObjExport.Add(10000000);
                if (contract.ExportID.HasValue)
                    template = actionServices.GetData<Cat_ExportEntity>(Common.DotNetToOracle(contract.ExportID.Value.ToString()), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstcontract = new List<Hre_ContractModel>();
                lstcontract.Add(contract);
                ExportService.ExportWord(outputPath, templatepath, lstcontract);
            }
            if (lstContractAll.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
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
            var contractServices = new Hre_ContractServices();
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstContract = actionServices.GetData<Hre_ContractEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status);
            if (lstContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
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
                contract.DateNow_Day = DateTime.Now.Day.ToString();
                contract.DateNow_Month = DateTime.Now.Month.ToString();
                contract.DateNow_Year = DateTime.Now.Year.ToString();

                contract.IDDateOfIssueFormat = contract.IDDateOfIssue.HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                if (contract.DateStart != null)
                    contract.DateStartFormat = contract.DateStart.ToString("dd/MM/yyyy");
                if (contract.DateEnd.HasValue)
                    contract.DateEndFormat = contract.DateEnd.Value.ToString("dd/MM/yyyy");
                if (contract.DateHire.HasValue)
                    contract.DateHireFormat = contract.DateHire.Value.ToString("dd/MM/yyyy");
                if (contract.DateEndProbation.HasValue)
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.ToString("dd/MM/yyyy");
                contract.SalaryFormat = contract.Salary.HasValue ? contract.Salary.Value.ToString("N") : "0";
                if (contract.DateOfEffect.HasValue)
                {
                    contract.DateOfEffectFormat = contract.DateOfEffect.Value.ToString("dd MMM yyyy");
                    contract.DateOfEffectMoreTwoMonthFormat = contract.DateOfEffect.Value.AddMonths(+2).ToString("dd MMM yyyy");

                }
                if (contract.Gender == "E_FEMALE")
                {
                    contract.GraveName = "Ms." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                else
                {
                    contract.GraveName = "Mr." + contract.ProfileName.Substring(contract.ProfileName.LastIndexOf(' '));
                }
                if (contract.DateQuit.HasValue)
                {
                    if (contract.DateHire.HasValue)
                    {
                        contract.MonthWorking = Math.Floor(contract.DateQuit.Value.Subtract(contract.DateHire.Value).TotalDays / 30);
                        contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                        if (contract.YearWorking > 0)
                        {
                            contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                        }
                    }
                }
                else
                {
                    contract.MonthWorking = Math.Floor(DateTime.Now.Subtract(contract.DateHire.Value).TotalDays / 30);
                    contract.YearWorking = Math.Floor(contract.MonthWorking.Value / 12);
                    if (contract.YearWorking > 0)
                    {
                        contract.MonthWorking = contract.MonthWorking - (contract.YearWorking * 12);
                    }
                }
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                if (!string.IsNullOrEmpty(valueFields))
                    template = actionServices.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                var lstcontract = new List<Hre_ContractEntity>();
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

        #endregion

        #region Fin_PurchaseRequest
        [HttpPost]
        public ActionResult GetPurchaseRequestList([DataSourceRequest] DataSourceRequest request, Fin_PurchaseRequestSearchModel model)
        {
            return GetListDataAndReturn<Fin_PurchaseRequestModel, FIN_PurchaseRequestEntity, Fin_PurchaseRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PurchaseRequest);
        }

        public ActionResult ExportAllPurchaseRequestList([DataSourceRequest] DataSourceRequest request, Fin_PurchaseRequestSearchModel model)
        {
            return ExportAllAndReturn<FIN_PurchaseRequestEntity, Fin_PurchaseRequestModel, Fin_PurchaseRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PurchaseRequest);
        }

        public ActionResult ExportPurchaseRequestelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<FIN_PurchaseRequestEntity, Fin_PurchaseRequestModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_PurchaseRequestByIds);
        }


        public ActionResult ExportPurchaseRequest(string purchaseID, Guid? exportID, bool isCreateTemplate, bool isCreateTemplateForDynamicGrid)
        {

            string status = string.Empty;
            var baseService = new BaseService();
            ExportFileType exportFileType = ExportFileType.Excel;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.ConvertToGuid(purchaseID));
            var entityPR = actionServices.GetByIdUseStore<Fin_PurchaseRequestModel>(Common.ConvertToGuid(purchaseID), ConstantSql.hrm_hr_sp_get_PurchaseRequestById, ref status);
            var lstPRItem = actionServices.GetData<Fin_PurchaseRequestItemModel>(objs, ConstantSql.hrm_cat_sp_get_PRItemByPRID, ref status);
            DataTable table = new DataTable("Fin_PurchaseRequestModel");
            if (entityPR != null && lstPRItem != null)
            {

                table.Columns.Add("FunctionName");
                table.Columns.Add("BudgetOwnerName");
                table.Columns.Add("ChannelName");
                table.Columns.Add("BudgetChargedIn");
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("SupplierName");
                table.Columns.Add("Description");
                table.Columns.Add("Code");
                table.Columns.Add("CateCodeType");
                table.Columns.Add("Name");
                table.Columns.Add("ProjectName");
                table.Columns.Add("PurchaseItemName");
                table.Columns.Add("PurchaseItemCost");
                table.Columns.Add("Quantity", typeof(double));
                table.Columns.Add("UnitPrice", typeof(double));
                table.Columns.Add("Amount", typeof(double));

                foreach (var item in lstPRItem)
                {
                    DataRow dr = table.NewRow();

                    dr["FunctionName"] = entityPR.FunctionName;
                    dr["BudgetOwnerName"] = entityPR.BudgetOwnerName;
                    dr["ChannelName"] = entityPR.ChannelName;
                    dr["BudgetChargedIn"] = entityPR.BudgetChargedIn == null ? string.Empty : entityPR.BudgetChargedIn.Value.ToShortDateString();
                    dr["From"] = entityPR.From == null ? string.Empty : entityPR.From.Value.ToShortDateString(); ;
                    dr["To"] = entityPR.To == null ? string.Empty : entityPR.To.Value.ToShortDateString();
                    dr["SupplierName"] = entityPR.SupplierName;
                    dr["Description"] = entityPR.Description;

                    dr["Code"] = item.Code == null ? string.Empty : item.Code;
                    dr["CateCodeType"] = item.CateCodeType == null ? string.Empty : item.CateCodeType;
                    dr["Name"] = item.Name == null ? string.Empty : item.Name;
                    dr["ProjectName"] = item.ProjectName == null ? string.Empty : item.ProjectName;
                    dr["PurchaseItemName"] = item.PurchaseItemName == null ? string.Empty : item.PurchaseItemName;
                    dr["PurchaseItemCost"] = item.PurchaseItemCost == null ? string.Empty : item.PurchaseItemCost;
                    dr["Quantity"] = item.Quantity == null ? 0 : item.Quantity.Value;
                    dr["UnitPrice"] = item.UnitPrice == null ? 0 : item.UnitPrice.Value;
                    dr["Amount"] = item.Amount == null ? 0 : item.Amount.Value;
                    table.Rows.Add(dr);
                }
                var result = table;

                object obj = new Fin_PurchaseRequestModel();
                var isDataTable = false;

                if (isCreateTemplateForDynamicGrid)
                {
                    obj = result;
                    isDataTable = true;
                }
                if (isCreateTemplate)
                {
                    var path = Common.GetPath("Templates");
                    ExportService exportService = new ExportService();
                    ConfigExport cfgExport = new ConfigExport()
                    {
                        Object = obj,
                        FileName = "Fin_PurchaseRequestModel",
                        OutPutPath = path,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = isDataTable
                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }

            }
            if (exportID != null)
            {
                var fullPath = ExportService.Export((Guid)exportID, table, exportFileType);
                return Json(fullPath);
            }

            return null;
        }
        #endregion

        #region ExportWord DS HD đến hạn
        public ActionResult ExportExpiryContractByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string[] valueFieldsExportID = valueFields.Split(',');
            valueFields = valueFieldsExportID[0];
            string _exportID = valueFieldsExportID[1];
            Guid exportID;

            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            var lstContract = actionServices.GetData<Hre_ContractEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status);
            if (lstContract == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
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
                contract.IDDateOfIssueFormat = contract.IDDateOfIssue.HasValue ? contract.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                if (contract.DateStart != null)
                {
                    contract.DateStartFormat = contract.DateStart.ToString("dd/MM/yyyy");
                }

                if (contract.DateEnd.HasValue)
                {
                    contract.DateEndFormat = contract.DateEnd.Value.ToString("dd/MM/yyyy");
                    contract.DateEndMonth = contract.DateEnd.Value.Month;
                    contract.DateEndYear = contract.DateEnd.Value.Year;
                    contract.DateEndFormatEN = contract.DateEnd.Value.ToString("dd-MMM-yyyy");
                }
                if (contract.DateHire.HasValue)
                {
                    contract.DateHireFormat = contract.DateHire.Value.ToString("dd/MM/yyyy");
                    contract.DateEndMonth = contract.DateHire.Value.Month;
                    contract.DateEndYear = contract.DateHire.Value.Year;
                    contract.DateHireFormatEN = contract.DateHire.Value.ToString("dd-MMM-yyyy");
                }

                if (contract.DateEndProbation.HasValue)
                    contract.DateEndProbationFormat = contract.DateEndProbation.Value.ToString("dd/MM/yyyy");
                contract.SalaryFormat = contract.Salary.HasValue ? contract.Salary.Value.ToString("N") : "0";
                ActionService service = new ActionService(UserLogin);
                var exportService = new Cat_ExportServices();
                Cat_ExportEntity template = null;
                string outputPath = string.Empty;
                //List<object> lstObjExport = new List<object>();
                //lstObjExport.Add(Common.DotNetToOracle(valueFields));

                //template = service.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                List<object> lstObjExport = new List<object>();
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(null);
                lstObjExport.Add(1);
                lstObjExport.Add(10000000);
                if (_exportID != "")
                {
                    exportID = Guid.Parse(_exportID);
                    template = actionServices.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields && s.ID == exportID).FirstOrDefault();
                }
                else
                {
                    template = actionServices.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(contract.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var lstcontract = new List<Hre_ContractEntity>();
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
        #endregion

        #region Hre_Dependant
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Dependant (Hre_Dependant) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDependantList([DataSourceRequest] DataSourceRequest request, Hre_DependantSearchModel model)
        {
            return GetListDataAndReturn<Hre_DependantModel, Hre_DependantEntity, Hre_DependantSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Dependant);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Dependant (Hre_Dependant) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportDependantList([DataSourceRequest] DataSourceRequest request, Hre_DependantSearchModel model)
        {
            return ExportAllAndReturn<Hre_DependantEntity, Hre_DependantModel, Hre_DependantSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Dependant);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Dependant (Hre_Dependant) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportDependantSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_DependantEntity, Hre_DependantModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_DependantByIds);
        }
        #endregion

        #region Hre_Discipline
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Discipline (Hre_Discipline) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDisciplineList([DataSourceRequest] DataSourceRequest request, Hre_DisciplineSearchModel model)
        {
            return GetListDataAndReturn<Hre_DisciplineModel, Hre_DisciplineEntity, Hre_DisciplineSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Discipline);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Discipline (Hre_Discipline) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportDisciplineList([DataSourceRequest] DataSourceRequest request, Hre_DisciplineSearchModel model)
        {
            return ExportAllAndReturn<Hre_DisciplineEntity, Hre_DisciplineModel, Hre_DisciplineSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Discipline);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Discipline (Hre_Discipline) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportDisciplineSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_DisciplineEntity, Hre_DisciplineModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_DisciplineByIds);
        }

        /// <summary>
        ///[Tho.Bui] Get Discipline theo profileid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="profileID"></param>
        /// <returns></returns>
        public ActionResult GetDisciplineProID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = actionServices.GetData<Hre_DisciplineEntity>(objs, ConstantSql.hrm_hr_sp_get_DisciplineprofileId, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }


        public int CountTimeDisByPro(Guid ProfileID)
        {
            int count = 0;
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_DisciplineEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_DisciplineprofileId, ref status).ToList();
            if (result != null)
            {
                count = result.Count();
            }
            return count;
        }

        #endregion

        #region Hre_ProfilePartyUnion
        /// <summary>
        /// [Tho.Bui]: Loat ProfilePartyAndUnion by ProfileID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="profileID"></param>
        /// <returns></returns>
        public Hre_ProfilePartyUnionModel ProfilePartyUnionProfileID(Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = actionServices.GetData<Hre_ProfilePartyUnionEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfilePartyUnionprofileId, ref status);
            var data = result.CopyData<Hre_ProfilePartyUnionModel>();
            if (result != null)
                return data;
            return null;
        }
        /// <summary>
        /// [Tho.Bui] - Lấy danh sách dữ liệu cho ProfilePartyUnion theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfilePartyUnionList([DataSourceRequest] DataSourceRequest request, Hre_ProfilePartyUnionSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfilePartyUnionModel, Hre_ProfilePartyUnionEntity, Hre_ProfilePartyUnionSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PartyUnionList);
        }

        /// <summary>
        /// [Tho.Bui] - Xuất dữ liệu đã chọn Hre_ProfilePartyUnion
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportPartyUnionSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_ProfilePartyUnionEntity, Hre_ProfilePartyUnionModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_ProfilePartyUnionByIds);
        }
        /// <summary>
        /// [Tho.Bui] - Xuât danh sách đảng đoàn
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportProfilePartyUnionList([DataSourceRequest] DataSourceRequest request, Hre_ProfilePartyUnionSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfilePartyUnionEntity, Hre_ProfilePartyUnionModel, Hre_ProfilePartyUnionSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PartyUnionList);
        }
        #endregion

        #region Computing
        public ActionResult GetComputingProID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_ProfileComputingLevelModel>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_ComputingprofileId, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetMultiComputingLevel(string text)
        {
            return GetDataForControl<CatComputingLevelMultiModel, Cat_ComputingLevelMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ComputingLevel_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        public JsonResult GetMultiComputingType(string text)
        {
            return GetDataForControl<CatComputingTypeMultiModel, Cat_ComputingTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_ComputingType_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }

        #endregion

        #region Education
        public JsonResult GetMultiEducationLevel(string text)
        {
            return GetDataForControl<CatEducationLevelMultiModel, Cat_EducationLevelMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EducationLevel_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        #endregion

        #region JobVancancyReson
        public JsonResult GetMultiJobVancancyResonLevel(string text)
        {
            return GetDataForControl<CatJobVancancyResonMultiModel, Cat_JobVancancyResonMultiEntity>(text, ConstantSql.hrm_cat_sp_get_JobVancancyReson_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        #endregion

        #region Hre_HDTJob

        [HttpPost]
        public ActionResult ApproveHDTJobOut(string selectedIds)
        {
            var service = new Hre_HDTJobServices();
            var message = service.ActionApprovedHDTJobOut(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult GetHDTJobOutList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobOutSearchModel model)
        {
            return GetListDataAndReturn<Hre_HDTJobModel, Hre_HDTJobEntity, Hre_HDTJobOutSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HDTJobOut);
        }

        [HttpPost]
        public ActionResult ExportHDTJobOutList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobOutSearchModel model)
        {
            return ExportAllAndReturn<Hre_HDTJobEntity, Hre_HDTJobModel, Hre_HDTJobOutSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HDTJobOut);
        }

        [HttpPost]
        public ActionResult GetHDTJobWaitingList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobWaitingSearchModel model)
        {
            return GetListDataAndReturn<Hre_HDTJobModel, Hre_HDTJobEntity, Hre_HDTJobWaitingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HDTJobWaiting);
        }

        [HttpPost]
        public ActionResult ApprovedAllHDTJobWaiting([DataSourceRequest] DataSourceRequest request, Hre_HDTJobSearchModel model)
        {
            return GetListDataAndReturn<Cat_HDTJobTypeModel, Cat_HDTJobTypeEntity, Hre_HDTJobSearchModel>(request, model, ConstantSql.hrm_hr_sp_set_ApprovedAllHDTJob);
        }

        [HttpPost]
        public ActionResult ExportHDTJobWaitingList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobWaitingSearchModel model)
        {
            return ExportAllAndReturn<Hre_HDTJobEntity, Hre_HDTJobModel, Hre_HDTJobWaitingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HDTJobWaiting);
        }
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho HDTJob (Hre_HDTJob) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHDTJobList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobSearchModel model)
        {
            var actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstObjSearch = new List<object>();
            lstObjSearch.Add(model.ProfileName);
            lstObjSearch.Add(model.CodeEmp);
            lstObjSearch.Add(model.HDTJobTypeID);
            lstObjSearch.Add(model.JobTitleID);
            lstObjSearch.Add(model.PositionID);
            lstObjSearch.Add(model.OrgStructureID);
            lstObjSearch.Add(model.DateFrom);
            lstObjSearch.Add(model.DateTo);
            lstObjSearch.Add(model.Price);
            lstObjSearch.Add(model.IsCreateTemplate);
            lstObjSearch.Add(model.ExportId);
            lstObjSearch.Add(model.ExportType);
            lstObjSearch.Add(1);
            lstObjSearch.Add(int.MaxValue - 1);
            var result = actionServices.GetData<Hre_HDTJobEntity>(lstObjSearch, ConstantSql.hrm_hr_sp_get_HDTJob, ref status);
            if (result.Count > 0)
            {
                var profileServices = new Hre_ProfileServices();
                var listResult = profileServices.getHDTJobByPrice(result, model.DateFrom, model.DateTo);
                return Json(listResult.ToDataSourceResult(request));
            }
            else
            {
                return null;
            }

        }

        public ActionResult ExportHDTJobListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_HDTJobSearchModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom == null ? DateTime.Now : model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo == null ? DateTime.Now : model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var actionServices = new ActionService(UserLogin);
            List<object> lstObjSearch = new List<object>();
            lstObjSearch.Add(model.ProfileName);
            lstObjSearch.Add(model.CodeEmp);
            lstObjSearch.Add(model.HDTJobTypeID);
            lstObjSearch.Add(model.JobTitleID);
            lstObjSearch.Add(model.PositionID);
            lstObjSearch.Add(model.OrgStructureID);
            lstObjSearch.Add(model.DateFrom);
            lstObjSearch.Add(model.DateTo);
            lstObjSearch.Add(model.Price);
            lstObjSearch.Add(model.IsCreateTemplate);
            lstObjSearch.Add(model.ExportId);
            lstObjSearch.Add(model.ExportType);
            lstObjSearch.Add(1);
            lstObjSearch.Add(int.MaxValue - 1);
            var result = actionServices.GetData<Hre_HDTJobEntity>(lstObjSearch, ConstantSql.hrm_hr_sp_get_HDTJob, ref status);
            var profileServices = new Hre_ProfileServices();
            var listResult = profileServices.getHDTJobByPrice(result, model.DateFrom, model.DateTo).Translate<Hre_HDTJobModel>();
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_HDTJobModel(),
                    FileName = "Hre_HDTJob",
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
                if (model.DateFrom != null && model.DateTo != null)
                {
                    var fullPath = ExportService.Export(model.ExportId, listResult, listHeaderInfo, model.ExportType);
                    return Json(fullPath);
                }
                else
                {
                    var fullPath = ExportService.Export(model.ExportId, listResult, null, model.ExportType);
                    return Json(fullPath);
                }

            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho HDTJob (Hre_HDTJob) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportHDTJobList([DataSourceRequest] DataSourceRequest request, Hre_HDTJobSearchModel model)
        {
            return ExportAllAndReturn<Hre_HDTJobEntity, Hre_HDTJobModel, Hre_HDTJobSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HDTJob);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của HDTJob (Hre_HDTJob) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportHDTJobSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_HDTJobEntity, Hre_HDTJobModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_HDTJobByIds);
        }

        [HttpPost]
        public ActionResult GetHDTJobByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {

            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = actionServices.GetData<Hre_HDTJobEntity>(objs, ConstantSql.hrm_hr_sp_get_HDTJobsByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetHDTJobDataByProfileID(Guid ProfileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_HDTJobEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_HDTJobsByProfileId, ref status).OrderByDescending(s => s.DateFrom).FirstOrDefault();
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult ApprovedHDTJob(string selectedIds)
        {
            var service = new Hre_HDTJobServices();
            var message = service.ActionApproved(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult CheckDataHDTJobIn(string selectedIds)
        {
            var service = new Hre_HDTJobServices();
            int countInvalid = service.CheckDataHDTJobIn(selectedIds);
            return Json(countInvalid);
        }

        [HttpPost]
        public ActionResult CheckDataHDTJobOut(string selectedIds)
        {
            var service = new Hre_HDTJobServices();
            var message = service.CheckDataHDTJobOut(selectedIds);
            return Json(message);
        }



        [HttpPost]
        public ActionResult ApprovedHDTJobWaiting(string selectedIds)
        {
            var arrParam = selectedIds.Split('|').ToList();
            var userLG = arrParam[0] ?? string.Empty;
            var Ids = arrParam[1];
            var service = new Hre_HDTJobServices();
            var message = service.ActionApprovedForHDTJobWaiting(Ids, userLG);
            return Json(message);
        }

        [HttpPost]
        public ActionResult ApprovedHDTJobOut(string selectedIds)
        {
            var arrParam = selectedIds.Split('|').ToList();
            var userLG = arrParam[0] ?? string.Empty;
            var Ids = arrParam[1];
            var service = new Hre_HDTJobServices();
            var message = service.ActionApprovedForHDTJobWaiting(Ids, userLG);
            return Json(message);
        }


        // Hủy cv nặng nhọc
        [HttpPost]
        public ActionResult ActionCancelHDT(string selectedIds)
        {
            var service = new Hre_HDTJobServices();
            var message = service.ActionCancel(selectedIds);
            return Json(message);
        }

        #endregion

        #region Hre_Relatives
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Relatives (Hre_Relatives) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRelativesList([DataSourceRequest] DataSourceRequest request, Hre_RelativesSearchModel model)
        {
            return GetListDataAndReturn<Hre_RelativesModel, Hre_RelativesEntity, Hre_RelativesSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Relatives);
        }

        [HttpPost]
        public ActionResult ExportRelativesListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_RelativesSearchModel model)
        {

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Hre_RelativesModel();
            var actionServices = new ActionService(UserLogin);
            var result = GetListData<Hre_RelativesModel, Hre_RelativesEntity, Hre_RelativesSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Relatives, ref status);

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
                    FileName = "Hre_RelativesModel",
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


            //return GetListDataAndReturn<Hre_RelativesModel, Hre_RelativesEntity, Hre_RelativesSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Relatives);
        }


        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Relatives (Hre_Relatives) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportRelativesList([DataSourceRequest] DataSourceRequest request, Hre_RelativesSearchModel model)
        {
            return ExportAllAndReturn<Hre_RelativesEntity, Hre_RelativesModel, Hre_RelativesSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Relatives);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Relatives (Hre_Relatives) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportRelativesSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_RelativesEntity, Hre_RelativesModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_RelativesByIds);
        }
        #endregion

        #region Hre_Reward
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Reward (Hre_Reward) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRewardList([DataSourceRequest] DataSourceRequest request, Hre_RewardSearchModel model)
        {
            return GetListDataAndReturn<Hre_RewardModel, Hre_RewardEntity, Hre_RewardSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Reward);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Reward (Hre_Reward) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportRewardList([DataSourceRequest] DataSourceRequest request, Hre_RewardSearchModel model)
        {
            return ExportAllAndReturn<Hre_RewardEntity, Hre_RewardModel, Hre_RewardSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Reward);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Reward (Hre_Reward) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportRewardSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_RewardEntity, Hre_RewardModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_RewardByIds);
        }

        /// <summary>
        /// [tho.Bui] lấy danh sách ReWard theo ProfileID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="profileID"></param>
        /// <returns></returns>
        public ActionResult GetRewardID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = actionServices.GetData<Hre_RewardEntity>(objs, ConstantSql.hrm_hr_sp_get_ReWardByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Hre_SoftSkill
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho SoftSkill (Hre_SoftSkill) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSoftSkillList([DataSourceRequest] DataSourceRequest request, Hre_SoftSkillSearchModel model)
        {
            return GetListDataAndReturn<Hre_SoftSkillModel, Hre_SoftSkillEntity, Hre_SoftSkillSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_SoftSkill);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho SoftSkill (Hre_SoftSkill) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportSoftSkillList([DataSourceRequest] DataSourceRequest request, Hre_SoftSkillSearchModel model)
        {
            return ExportAllAndReturn<Hre_SoftSkillEntity, Hre_SoftSkillModel, Hre_SoftSkillSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_SoftSkill);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của SoftSkill (Hre_SoftSkill) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportSoftSkillSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_SoftSkillEntity, Hre_SoftSkillModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_SoftSkillByIds);
        }
        #endregion

        #region Hre_Uniform
        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho Uniform (Hre_Uniform) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUniformList([DataSourceRequest] DataSourceRequest request, Hre_UniformSearchModel model)
        {
            return GetListDataAndReturn<Hre_UniformModel, Hre_UniformEntity, Hre_UniformSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Uniform);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho Uniform (Hre_Uniform) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportUniformList([DataSourceRequest] DataSourceRequest request, Hre_UniformSearchModel model)
        {
            return ExportAllAndReturn<Hre_UniformEntity, Hre_UniformModel, Hre_UniformSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Uniform);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của Uniform (Hre_Uniform) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportUniformSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_UniformEntity, Hre_UniformModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_UniformByIds);
        }
        #endregion

        #region Hre_WorkHistory

        [HttpPost]
        public ActionResult ApproveWorkHistory(string selectedIds)
        {
            var service = new Hre_WorkHistoryServices();
            var message = service.ActionApproved(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult CancelWorkHistory(string selectedIds)
        {
            var service = new Hre_WorkHistoryServices();
            var message = service.ActionCancel(selectedIds);
            return Json(message);
        }

        [HttpPost]
        public ActionResult WaitApproveWorkHistory(string selectedIds)
        {
            var service = new Hre_WorkHistoryServices();
            var message = service.ActionWaitApprove(selectedIds);
            return Json(message);
        }

        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho WorkHistory (Hre_WorkHistory) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult GetWorkHistoryList([DataSourceRequest] DataSourceRequest request, Hre_WorkHistorySearchModel model)
        {
            return GetListDataAndReturn<Hre_WorkHistoryEntity, Hre_WorkHistoryModel, Hre_WorkHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_WorkHistory);
        }

        [HttpPost]
        public ActionResult GetWorkHistoryWaitingApproveList([DataSourceRequest] DataSourceRequest request, Hre_WorkHistoryWaitingSearchModel model)
        {
            return GetListDataAndReturn<Hre_WorkHistoryEntity, Hre_WorkHistoryModel, Hre_WorkHistoryWaitingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_WorkHistoryWaitingApprove);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho WorkHistory (Hre_WorkHistory) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportWorkHistoryList([DataSourceRequest] DataSourceRequest request, Hre_WorkHistorySearchModel model)
        {
            return ExportAllAndReturn<Hre_WorkHistoryEntity, Hre_WorkHistoryModel, Hre_WorkHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_WorkHistory);
        }
        public ActionResult GetRelativeByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_RelativesEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_RelativeByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetDependantByProfileID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            // Son.vo - 20141220 - lấy người thân không cần truyền vào kỳ công làm gì.
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = actionServices.GetData<Hre_DependantEntity>(objs, ConstantSql.hrm_hr_sp_get_DependantByProfileId, ref status);
            if (result == null)
            {
                return null;
            }

            if (model.IsExport)
            {
                status = ExportService.Export(result, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của WorkHistory (Hre_WorkHistory) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportWorkHistorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_WorkHistoryEntity, Hre_WorkHistoryModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_WorkHistoryByIds);
        }

        public ActionResult GetWorkHistoryByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            List<Hre_WorkHistoryEntity> result = new List<Hre_WorkHistoryEntity>();
            var data = actionServices.GetData<Hre_WorkHistoryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, ref status);
            if (data != null && data.Count > 0)
            {
                result = data;
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetTopWorkHistoryByProfileID(Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_WorkHistoryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, ref status).ToList();
            Hre_WorkHistoryEntity objWorkHistory = new Hre_WorkHistoryEntity();
            if (result.Count > 0)
            {
                objWorkHistory = result.OrderByDescending(s => s.DateEffective).FirstOrDefault();
                return Json(objWorkHistory);
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetCandidateHistoryByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_CandidateHistoryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_CandidateHistoryByProfileId, ref status);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Quoc.Do] - Xuất các dòng dữ liệu được theo ProfileID của VisaInfo (Hre_VisaInfo) 
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult GetVisaInfoByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var result = actionServices.GetData<Hre_VisaInfoEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_VisaInfoByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region Report HR

        /// <summary>
        /// [SonVo] - 2014/06/05
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportCodeNotInSystem([DataSourceRequest] DataSourceRequest request, Hre_ReportCodeNotInSystemModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom != null ? Model.DateFrom : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo != null ? Model.DateTo : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportCodeNotInSystemModel(),
                    FileName = " Hre_ReportCodeNotInSystem",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            var actionServices = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();
            //ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request)
            //};
            List<object> listobj = new List<object>();
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
            listobj.Add(From);
            listobj.Add(To);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportCodeNotInSystemEntity>(listobj, ConstantSql.hrm_hr_sp_get_RptCodeNotInSystem, ref status).ToList().Translate<Hre_ReportCodeNotInSystemModel>();
            //var result = service.GetReportCodeNotInSystem(From, To).ToList().Translate<Hre_ReportCodeNotInSystemModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);

                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// [SonVo] - 2014/06/05
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReportHDTJob([DataSourceRequest] DataSourceRequest request, Hre_ReportHDTJobModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportHDTJobModel(),
                    FileName = "Hre_ReportHDTJob",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHDTJobModel>(model, "Hre_ReportHDTJob", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var actionServices = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();

            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            if (model.DateFrom != null)
            {
                From = model.DateFrom.Value;
            }
            if (model.DateTo != null)
            {
                To = model.DateTo.Value;
            }
            string status = string.Empty;
            var hdtJobServices = new Hre_HDTJobServices();
            List<object> listObjHDTJob = new List<object>();
            listObjHDTJob.Add(model.OrgStructureID);
            listObjHDTJob.Add(model.DateFrom);
            listObjHDTJob.Add(model.DateTo);
            var result = actionServices.GetData<Hre_ReportHDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_RptHDTJob, ref status).ToList().Translate<Hre_ReportHDTJobModel>();

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, null, model.ExportType);

                return Json(fullPath);
            }


            return Json(result.ToDataSourceResult(request));
        }

        #region BC NV đang làm việc

        public ActionResult GetReportProfileWorkingValidate(Hre_ReportProfileWorkingModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportProfileWorkingModel>(model, "Hre_ReportProfileWorking", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }


        [HttpPost]
        public ActionResult GetReportProfileWorking([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileWorkingModel Model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileWorkingModel>(Model, "Hre_ReportProfileWorking", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);

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
            var isDataTable = false;
            object obj = new Hre_ReportProfileWorkingEntity();
            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.DateFrom);
            listObj.Add(Model.DateTo);
            listObj.Add(Model.CodeEmp);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportProfileWorkingEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptWorkingProfile, ref status);

            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportProfileWorkingEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {

                string[] valueField = null;
                if (Model.ValueFields != null)
                {
                    valueField = Model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(Model.ExportID, result, null, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetReportProfileProbation([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileProbationModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateStart != null ? Model.DateStart : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateEnd != null ? Model.DateEnd : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportProfileProbationModel(),
                    FileName = "Hre_ReportProfileProbation",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileProbationModel>(Model, "Hre_ReportProfileProbation", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);

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

            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(From);
            listObj.Add(To);

            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportProfileProbationEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptProbationProfile, ref status).ToList().Translate<Hre_ReportProfileProbationModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetReportPrenancy([DataSourceRequest] DataSourceRequest request, Hre_ReportPregnancyModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportPregnancyModel>(Model, "Hre_ReportPregnancy", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            string status = string.Empty;

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
            var isDataTable = false;
            object obj = new Hre_ReportPregnancyModel();

            List<object> listObj = new List<object>();
            listObj.Add(Model.DateStart);
            listObj.Add(Model.DateEnd);
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.CodeEmp);
            var result = actionServices.GetData<Hre_ReportPregnancyEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptPrenancy, ref status).ToList();

            var orgServices = new Cat_OrgStructureServices();
            var lstObjOrg = new List<object>();
            lstObjOrg.Add(null);
            lstObjOrg.Add(null);
            lstObjOrg.Add(null);
            lstObjOrg.Add(1);
            lstObjOrg.Add(int.MaxValue - 1);
            var lstOrg = actionServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            var orgTypeService = new Cat_OrgStructureTypeServices();
            var lstObjOrgType = new List<object>();
            lstObjOrgType.Add(null);
            lstObjOrgType.Add(null);
            lstObjOrgType.Add(1);
            lstObjOrgType.Add(int.MaxValue - 1);
            var lstOrgType = actionServices.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();

            var lstRptPrenancyEntity = new List<Hre_ReportPregnancyEntity>();

            foreach (var item in result)
            {
                var pregnancyEntity = new Hre_ReportPregnancyEntity();
                var orgName = service.GetParentOrgName(lstOrg, lstOrgType, item.OrgID);
                if (orgName.Count < 3)
                {
                    orgName.Insert(0, string.Empty);
                    if (orgName.Count < 3)
                    {
                        orgName.Insert(0, string.Empty);
                    }
                }
                pregnancyEntity = item.CopyData<Hre_ReportPregnancyEntity>();
                pregnancyEntity.Channel = orgName[2];
                pregnancyEntity.Region = orgName[1];
                pregnancyEntity.Area = orgName[0];
                lstRptPrenancyEntity.Add(pregnancyEntity);
            }

            var lstRptPrenancyModel = lstRptPrenancyEntity.Translate<Hre_ReportPregnancyModel>();


            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstRptPrenancyModel;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportPregnancyModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {

                string[] valueField = null;
                if (Model.ValueFields != null)
                {
                    valueField = Model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(Model.ExportID, lstRptPrenancyModel, null, Model.ExportType);

                return Json(fullPath);
            }
            return Json(lstRptPrenancyModel.ToDataSourceResult(request));
        }

        public ActionResult GetReportProfileQuit([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileQuitModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateStart != null ? Model.DateStart : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateEnd != null ? Model.DateEnd : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportProfileQuitModel(),
                    FileName = "Hre_ReportProfileQuit",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileQuitModel>(Model, "Hre_ReportProfileQuit", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            //DateTime From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //DateTime To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
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

            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(From);
            listObj.Add(To);

            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportProfileQuitEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptQuitProfile, ref status).ToList().Translate<Hre_ReportProfileQuitModel>();
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetReportWorkHistoryDept([DataSourceRequest] DataSourceRequest request, Hre_ReportWorkHistoryDeptModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportWorkHistoryDeptModel>(Model, "Hre_ReportWorkHistoryDept", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var actionServices = new ActionService(UserLogin);
            var profileServices = new Hre_ProfileServices();
            var rptServices = new Hre_ReportServices();
            List<object> listObj = new List<object>();
            listObj.Add(Model.DateFrom);
            listObj.Add(Model.DateTo);
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.CodeEmp);
            listObj.Add(Model.JobTitleID);
            listObj.Add(Model.PositionID);
            listObj.Add(Model.OrgStructureIDs);
            listObj.Add(Model.TypeOfTransferID);
            listObj.Add(Model.SalaryClassID);
            listObj.Add(Model.WorkPlaceID);
            listObj.Add(Model.Status);
            listObj.Add(1);
            listObj.Add(int.MaxValue - 1);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportWorkHistoryDeptEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptWorkHistoryDept, ref status).ToList().Translate<Hre_ReportWorkHistoryDeptModel>();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom != null ? Model.DateFrom : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo != null ? Model.DateTo : DateTime.Now };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlaceName", Value = ((result != null && result.FirstOrDefault() != null) && result.FirstOrDefault().WorkPlaceName != null) ? result.FirstOrDefault().WorkPlaceName : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportWorkHistoryDeptModel(),
                    FileName = "Hre_ReportWorkHistoryDept",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            bool isgroup = profileServices.IsGroupByOrgProfileQuit();
            if (isgroup == true)
            {
                List<Hre_ReportWorkHistoryDeptModel> resultNew = new List<Hre_ReportWorkHistoryDeptModel>();
                if (result.Count > 0)
                {
                    var orgServices = new Cat_OrgStructureServices();
                    var lstObjOrg = new List<object>();
                    lstObjOrg.Add(null);
                    lstObjOrg.Add(null);
                    lstObjOrg.Add(null);
                    lstObjOrg.Add(1);
                    lstObjOrg.Add(int.MaxValue - 1);
                    var lstOrg = actionServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                    var orgTypeService = new Cat_OrgStructureTypeServices();
                    var lstObjOrgType = new List<object>();
                    lstObjOrgType.Add(null);
                    lstObjOrgType.Add(null);
                    lstObjOrgType.Add(1);
                    lstObjOrgType.Add(int.MaxValue - 1);
                    var lstOrgType = actionServices.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();

                    foreach (var item in result)
                    {
                        var orgName = new List<string>();
                        if (item.OrgStructureID != null)
                        {
                            orgName = rptServices.GetParentOrgName(lstOrg, lstOrgType, item.OrgStructureID);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                                if (orgName.Count < 3)
                                {
                                    orgName.Insert(0, string.Empty);
                                }
                            }
                        }
                        if (orgName.Count > 0)
                        {
                            item.Channel = orgName[2];
                            item.Region = orgName[1];
                            item.Area = orgName[0];
                        }
                        resultNew.Add(item);
                    }
                }
                if (Model.ExportID != Guid.Empty)
                {
                    var fullPath = ExportService.Export(Model.ExportID, resultNew, listHeaderInfo, Model.ExportType);

                    return Json(fullPath);
                }

                return Json(resultNew.ToDataSourceResult(request));
            }

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ReportWorkHistoryDeptValidate(Hre_ReportWorkHistoryDeptModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportWorkHistoryDeptModel>(model, "Hre_ReportWorkHistoryDeptValidate", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            return Json(message);
        }
        public ActionResult ExportWordReportWorkHistoryDeptByTemplate(List<Guid> selectedIds, string valueFields, Hre_ReportWorkHistoryDeptModel model)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            var contractServices = new Hre_ReportWorkHistoryDeptModel();
            var baseService = new BaseService();
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);

            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.Add(strIDs);
            listObj.Add(model.DateFrom);
            listObj.Add(model.DateTo);
            string status = string.Empty;
            var lstReportWorkHistory = actionServices.GetData<Hre_ReportWorkHistoryDeptEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptWorkHistoryDeptByids, ref status).ToList();
            if (lstReportWorkHistory == null)
                return null;
            int i = 0;
            var lstReportWorkHistoryID = lstReportWorkHistory.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstReportWorkHistory.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var ReportWorkHistory in lstReportWorkHistory)
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
                template = actionServices.GetData<Cat_ExportEntity>(lstObjExport, ConstantSql.hrm_cat_sp_get_ExportWord, ref status).Where(s => s.ScreenName == valueFields).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(ReportWorkHistory.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(ReportWorkHistory.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lst = new List<Hre_ReportWorkHistoryDeptEntity>();
                lst.Add(ReportWorkHistory);
                ExportService.ExportWord(outputPath, templatepath, lst);
            }
            if (lstReportWorkHistory.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public ActionResult GetReportProfileNew([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileNewModel Model)
        {
            string status = string.Empty;
            var services = new Hre_ReportServices();
            var isDataTable = false;
            object obj = new Hre_ReportProfileNewModel();

            #region Validate
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileNewModel>(Model, "Hre_ReportProfileNew", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            DateTime From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            #endregion
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }

            var result = services.GetReportProfileNew(
                From,
                To, 
                Model.OrgStructureID,
                Model.IsCreateTemplate,
                Model.CodeEmp,
                Model.ProfileName,
                Model.SalaryClassID,
                Model.CodeCandidate,
                Model.WorkPlaceID,
                Model.EmpTypeID,
                UserLogin
                );

            if (Model != null && Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportProfileNewModel(),
                    FileName = "Hre_ReportProfileNew",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        #region Báo cáo Sinh nhật
        [HttpPost]
        public ActionResult GetReportBirthday([DataSourceRequest] DataSourceRequest request, Hre_ReportBirthdayModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom != null ? Model.DateFrom.Value : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo != null ? Model.DateTo.Value : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            #region Tạo  Template
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportBirthdayModel(),
                    FileName = "Hre_ReportBirthday",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion  
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportBirthdayModel>(Model, "Hre_ReportBirthday", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            List<Guid?> OrgIds = new List<Guid?>();
            listObj.Add(Model.DateFrom);
            listObj.Add(Model.DateTo);
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.DateQuitFrom);
            listObj.Add(Model.DateQuitTo);
            listObj.Add(Model.WorkPlaceID);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportBirthdayEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptBirthday, ref status).ToList().Translate<Hre_ReportBirthdayModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }


            return Json(result.ToDataSourceResult(request));

        }
        #endregion

        #region Báo cáo Kỷ Luật
        [HttpPost]
        public ActionResult GetReportProfileDiscipline([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileDisciplineModel Model)
        {
            #region Code cũ
            //var service = new Hre_ReportServices();
            //var hrService = new Hre_ProfileServices();
            //var isDataTable = false;
            //object obj = new Hre_ReportProfileDisciplineModel();
            //List<object> listObj = new List<object>();
            //DateTime From = SqlDateTime.MinValue.Value;
            //DateTime To = SqlDateTime.MaxValue.Value;
            //List<Guid?> OrgIds = new List<Guid?>();
            //if (Model.DateFrom != null)
            //{
            //    From = Model.DateFrom.Value;
            //    listObj.Add(From);
            //}
            //else
            //{
            //    listObj.Add(null);
            //}
            //if (Model.DateTo != null)
            //{
            //    To = Model.DateTo.Value;
            //    listObj.Add(To);
            //}
            //else
            //{
            //    listObj.Add(null);
            //}

            //string strOrgIDs = null;
            //if (!string.IsNullOrEmpty(Model.OrgStructureID))
            //{
            //    strOrgIDs = Model.OrgStructureID;
            //}

            //listObj.Add(strOrgIDs);


            //string status = string.Empty;
            //var result = hrService.GetData<Hre_ReportProfileDisciplineEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptDiscripline, ref status).ToList().Translate<Hre_ReportProfileDisciplineModel>();

            //if (Model.IsCreateTemplateForDynamicGrid)
            //{
            //    obj = result;
            //    isDataTable = true;
            //}
            //if (Model != null && Model.IsCreateTemplate)
            //{
            //    var path = Common.GetPath("Templates");
            //    ExportService exportService = new ExportService();
            //    ConfigExport cfgExport = new ConfigExport()
            //    {
            //        Object = obj,
            //        FileName = "Hre_ReportProfileDisciplineModel",
            //        OutPutPath = path,
            //        DownloadPath = Hrm_Main_Web + "Templates",
            //        IsDataTable = isDataTable
            //    };
            //    var str = exportService.CreateTemplate(cfgExport);
            //    return Json(str);
            //}

            //if (Model.ExportID != Guid.Empty)
            //{
            //    if (result != null && result.Count > 0)
            //    {
            //        #region lấy Org và OrgType

            //        var orgServices = new Cat_OrgStructureServices();
            //        var lstObjOrg = new List<object>();
            //        lstObjOrg.Add(null);
            //        lstObjOrg.Add(null);
            //        lstObjOrg.Add(null);
            //        lstObjOrg.Add(1);
            //        lstObjOrg.Add(int.MaxValue - 1);
            //        var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            //        var orgTypeService = new Cat_OrgStructureTypeServices();
            //        var lstObjOrgType = new List<object>();
            //        lstObjOrgType.Add(null);
            //        lstObjOrgType.Add(null);
            //        lstObjOrgType.Add(1);
            //        lstObjOrgType.Add(int.MaxValue - 1);
            //        var lstOrgType = orgTypeService.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();
            //        #endregion

            //        foreach (var item in result)
            //        {
            //            Guid? orgId = item.OrgStructureID1;
            //            var org = lstOrg.FirstOrDefault(s => s.ID == item.OrgStructureID1);
            //            var orgBranch = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_BRANCH, lstOrg, lstOrgType);
            //            var orgGroup = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_GROUP, lstOrg, lstOrgType);
            //            var orgOrg = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
            //            var orgTeam = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_TEAM, lstOrg, lstOrgType);
            //            var orgSection = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_SECTION, lstOrg, lstOrgType);
            //            var orgDivision = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DIVISION, lstOrg, lstOrgType);

            //            item.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
            //            item.GroupName = orgGroup != null ? orgGroup.OrgStructureName : string.Empty;
            //            item.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
            //            item.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
            //            item.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
            //            item.DivisionName = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;

            //            item.DisciplineCount = result.Where(s => s.ProfileID == item.ProfileID).Count();
            //        }

            //    }
            //    string[] valueField = null;
            //    if (Model.ValueFields != null)
            //    {
            //        valueField = Model.ValueFields.Split(',');
            //    }
            //    var fullPath = ExportService.Export(Model.ExportID, result, null, Model.ExportType);

            //    return Json(fullPath);
            //}
            //return Json(result.ToDataSourceResult(request)); 
            #endregion

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo ?? DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportProfileDisciplineModel(),
                    FileName = "Hre_ReportProfileDiscipline",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileDisciplineModel>(Model, "Hre_ReportProfileDiscipline", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion

            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.Add(Model.DateFrom);
            listObj.Add(Model.DateTo);
            listObj.Add(Model.OrgStructureID);

            string status = string.Empty;

            var result = actionServices.GetData<Hre_ReportProfileDisciplineEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptDiscripline, ref status).ToList().Translate<Hre_ReportProfileDisciplineModel>();
            var lstprofileids = result.Select(s => s.ProfileID).ToList();

            foreach (var item in result)
            {
                Guid profileID = item.ProfileID;
                item.count = result.Count(s => s.ProfileID == profileID);
            }


            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Báo cáo Khen Thưởng
        [HttpPost]
        public ActionResult GetReportReward([DataSourceRequest] DataSourceRequest request, Hre_ReportRewardModel Model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportRewardModel>(Model, "Hre_ReportReward", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Hre_ReportRewardModel();
            List<object> listObj = new List<object>();
            DateTime From = SqlDateTime.MinValue.Value;
            DateTime To = SqlDateTime.MaxValue.Value;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
                listObj.Add(From);
            }
            else
            {
                listObj.Add(null);
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
                listObj.Add(To);
            }
            else
            {
                listObj.Add(null);
            }

            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                strOrgIDs = Model.OrgStructureID;
            }

            listObj.Add(strOrgIDs);


            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = From };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = To };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportRewardEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptReward, ref status).ToList().Translate<Hre_ReportRewardModel>();

            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportReward",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion


        #region Báo cáo lịch sử làm việc nhân viên
        [HttpPost]
        public ActionResult GetReportHistoryProfile([DataSourceRequest] DataSourceRequest request, Hre_ReportHistoryProfileModel Model)
        {
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            List<Guid?> OrgIds = new List<Guid?>();
            listObj.Add(Model.DateHireFrom);
            listObj.Add(Model.DateHireTo);
            listObj.Add(Model.DateQuitFrom);
            listObj.Add(Model.DateQuitTo);
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.CodeEmp);
            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                strOrgIDs = Model.OrgStructureID;
            }

            listObj.Add(strOrgIDs);

            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportHistoryProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptHistoryProfile, ref status).ToList().Translate<Hre_ReportHistoryProfileModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);

                return Json(fullPath);
            }


            return Json(result.ToDataSourceResult(request));

        }
        #endregion

        #region Báo cáo Nhân Viên Chưa Có Hợp Đồng
        [HttpPost]
        public ActionResult GetReportProfileNotContract([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileNotContractModel Model)
        {
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.DateHireFrom);
            listObj.Add(Model.DateHireTo);
            listObj.Add(Model.PositionID);
            listObj.Add(Model.JobTitleID);
            listObj.Add(Model.Gender);
            listObj.Add(Model.EmpTypeID);
            listObj.Add(1);
            listObj.Add(int.MaxValue - 1);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportProfileNotContractEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptNotContract, ref status).ToList().Translate<Hre_ReportProfileNotContractModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);

                return Json(fullPath);
            }


            return Json(result.ToDataSourceResult(request));

        }

        #endregion

        #region Báo cáo số lượng nhân viên theo phòng ban
        public ActionResult GetReportOrgProfle([DataSourceRequest] DataSourceRequest request, CatOrgStructureModel Model)
        {
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> listObj = new List<object>();
            List<Guid?> OrgIds = new List<Guid?>();
            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                strOrgIDs = Model.OrgStructureID;
            }
            listObj.Add(strOrgIDs);
            string status = string.Empty;
            var result = actionServices.GetData<Cat_OrgStructureEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptOrgProfile, ref status).ToList().Translate<CatOrgStructureModel>();

            object obj = new CatOrgStructureModel();
            var isDataTable = false;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "CatOrgStructureModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Báo cáo Thâm niên
        [HttpPost]
        public ActionResult GetReportSeniority([DataSourceRequest] DataSourceRequest request, Hre_ReportSeniorityModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateSeniority != null ? Model.DateSeniority.Value : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportSeniorityModel(),
                    FileName = "Hre_ReportSeniority",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportSeniorityModel>(Model, "Hre_ReportSeniority", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);

            DateTime DateSeniority = DateTime.Now;
            List<Guid?> OrgIds = new List<Guid?>();
            if (Model.DateSeniority != null)
            {
                DateSeniority = Model.DateSeniority.Value;
            }

            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                strOrgIDs = Model.OrgStructureID;
            }
            List<object> listObj = new List<object>();
            listObj.Add(strOrgIDs);
            listObj.Add(string.Empty);
            listObj.Add(string.Empty);

            string status = string.Empty;
            var listEntity = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, ref status).ToList();
            if (listEntity.Count == 0)
            {
                return Json(null);
            }
            var result = service.GetReportSeniority(DateSeniority, listEntity).ToList().Translate<Hre_ReportSeniorityModel>();

            if (Model.ExportID != Guid.Empty)
            {
                string[] valueField = null;
                if (Model.ValueFields != null)
                {
                    valueField = Model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }


            return Json(result.ToDataSourceResult(request));

        }

        #endregion

        #region BC Headcount thâm niên
        public ActionResult GetReportHCSeniorityValidate(Hre_ReportHCSeniorityModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportHCSeniorityModel>(model, "Hre_ReportHCSeniority", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }


        public ActionResult GetReportHCSeniority([DataSourceRequest] DataSourceRequest request, Hre_ReportHCSeniorityModel model)
        {
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();

            object obj = new Hre_ReportHCSeniorityModel();
            var isDataTable = false;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            var result = service.GetReportHCSeniority(model.MonthSearch, model.JobtitleID, model.OrgStructureID, model.OrgStructureTypeID, model.isIncludeQuitEmp, model.IsCreateTemplate, UserLogin);

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
                    FileName = "Hre_ReportHCSeniorityModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }


            if (model.ExportID != Guid.Empty)
            {
                result.Rows[0].Delete();
                //var row = result.Rows.Count;
                //result.Rows[row - 1].Delete();

                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    var colName = model.ValueFields;
                    valueField = colName.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            //  string dataReturn = result.ConvertDataTabletoString();
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region BC HeadCount Giới Tính

        public ActionResult GetReportHCGenderValidate(Hre_ReportHCGenderModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportHCGenderModel>(model, "Hre_ReportHCGender", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        public ActionResult CreateTemplate(string t1, bool t2, string t3, string t4, bool t5, bool t6)
        {
            var service = new BaseService();
            Cat_ExportEntity exportEntity = new Cat_ExportEntity()
            {
                ExportName = t1,
                IsColumnDynamic = t2,
                ScreenName = t4,
                TemplateFile = t3,
                IsReadOnly = t5,
                IsProtected = t6,
                IsSmartMarkers = true
            };

            service.Add(exportEntity);
            return Json("");
        }
        //public ActionResult CreateTemplate(CreateTemplateModel model)
        //{
        //    if (model != null)
        //    {
        //        var service = new BaseService();
        //        Cat_ExportEntity exportEntity = new Cat_ExportEntity()
        //        {
        //            ExportName = model.TemplateName,
        //            IsColumnDynamic = model.IsDynamic,
        //            ScreenName = model.ScreenName,
        //            TemplateFile = model.TemplateFile
        //        };

        //        service.Add(exportEntity);
        //    }
        //    return Json("");
        //}
        public ActionResult GetReportHCGender([DataSourceRequest] DataSourceRequest request, Hre_ReportHCGenderModel model)
        {
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            object obj = new Hre_ReportHCGenderModel();
            var isDataTable = false;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            var result = service.GetReportHCGender(model.MonthSearch, model.JobtitleID, model.OrgStructureID, model.OrgStructureTypeID, model.Gender, model.isIncludeQuitEmp, model.IsCreateTemplate, UserLogin);

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
                    FileName = "Hre_ReportHCGenderModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                result.Rows[0].Delete();
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                //  var row = result.Rows.Count;
                //    result.Rows[row - 1].Delete();

                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region BC HeadCount Hàng Tháng

        public ActionResult GetReportMonthlyHCValidate(Hre_ReportMonthlyHCModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportMonthlyHCModel>(model, "Hre_ReportMonthlyHC", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportMonthlyHC([DataSourceRequest] DataSourceRequest request, Hre_ReportMonthlyHCModel model)
        {
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            object obj = new Hre_ReportMonthlyHCModel();
            var isDataTable = false;
            //List<object> listObj = new List<object>();
            //listObj.Add(model.OrgStructureID);
            //listObj.Add(string.Empty);
            //listObj.Add(string.Empty);

            //string status = string.Empty;

            //var listEntity = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, ref status).Select(s => s.ID).ToList();

            var result = service.GetReportMonthlyHC(model.dateSearch, model.JobtitleID, model.OrgStructureID, model.OrgStructureTypeID, model.IsCreateTemplate, UserLogin);
            var rs = result.Translate<Hre_ReportMonthlyHCModel>();

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
                    FileName = "Hre_ReportMonthlyHCModel",
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
            //0string dataReturn = result.ConvertDataTabletoString();
            return Json(rs.ToDataSourceResult(request));
        }
        #endregion

        #region BC NV làm lại
        [HttpPost]
        public ActionResult GetReportProfileComeBack([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileComeBackModel Model)
        {
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
            var contractServices = new Hre_ContractServices();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileComeBackModel>(Model, "Hre_ReportProfileComeBack", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            List<object> lstpara = new List<object>();
            lstpara.Add(Model.DateFrom);
            lstpara.Add(Model.DateTo);
            lstpara.Add(Model.OrgStructureIDs);
            lstpara.Add(Model.ProfileName);
            lstpara.Add(Model.CodeEmp);
            lstpara.Add(Model.RankID);
            lstpara.Add(Model.WorkPlaceID);
            var result = actionServices.GetData<Hre_ReportProfileComeBackModel>(lstpara, ConstantSql.hrm_hr_sp_get_RptProfileComBack, ref status);

            var lstResultEntity = result.Translate<Hre_ReportProfileComeBackEntity>();

            var dataResult = contractServices.GetDataContractByProfileID(lstResultEntity, UserLogin);

            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = dataResult;
                isDataTable = true;
            }


            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportProfileComeBackEntity",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, dataResult, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult ExportProfileComeBackByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            string status = string.Empty;
            var actionServices = new ActionService(UserLogin);
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
            var lstPerformance = actionServices.GetData<Hre_ReportProfileComeBackModel>(strIDs, ConstantSql.hrm_hr_sp_get_RptProfileComBackByID, ref status);
            if (lstPerformance == null)
            {
                return null;
            }
            int i = 0;
            var lstPerformanceID = lstPerformance.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "Hre_ReportProfileComeBackModel" + suffix;
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
            foreach (var performance in lstPerformance)
            {

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
                    template = actionServices.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                var lstPers = new List<Hre_ReportProfileComeBackModel>();
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

        #endregion
        #endregion

        #region BC HeadCount Doanh Số
        public ActionResult GetReportHCSalesValidate(Hre_ReportHCSalesModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportHCSalesModel>(model, "Hre_ReportHCSales", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportHCSales([DataSourceRequest] DataSourceRequest request, Hre_ReportHCSalesModel model)
        {
            var service = new Hre_ReportServices();
            var eva_Service = new Eva_ReportServices();
            var hrService = new Hre_ProfileServices();

            //List<object> listObj = new List<object>();
            //listObj.Add(model.OrgStructureID);
            //listObj.Add(string.Empty);
            //listObj.Add(string.Empty);

            //string status = string.Empty;

            //var listEntity = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, ref status).Select(s => s.ID).ToList();

            var result = eva_Service.GetReportHCSales(model.dateSearch, model.OrgStructureID, model.IsCreateTemplate, UserLogin);
            //var rs = result.Translate<Hre_ReportMonthlyHCModel>();

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            object obj = new Hre_ReportHCSalesModel();
            var isDataTable = false;
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
                    FileName = "Hre_ReportHCSalesModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                //var row = result.Rows.Count;
                // result.Rows[row - 1].Delete();
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            //0string dataReturn = result.ConvertDataTabletoString();
            return Json(result.ToDataSourceResult(request));
        }

        #endregion


        #region Trình Độ Học Vấn
        public ActionResult GetReportEducationCharList([DataSourceRequest] DataSourceRequest request, Hre_ReportEducationChartListModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom != null ? Model.DateFrom : DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo != null ? Model.DateTo : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportEducationChartListModel(),
                    FileName = "Hre_ReportEducationChartList",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportEducationChartListModel>(Model, "Hre_ReportProfileNew", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            DateTime From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            #endregion

            if (Model.DateFrom != null)
            {
                From = Model.DateFrom.Value;
            }
            if (Model.DateTo != null)
            {
                To = Model.DateTo.Value;
            }
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)Model.OrgStructureID;

            string status = string.Empty;
            List<Guid> lstProfileIDs = actionServices.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            var result = service.GetReportEducationCharList(From, To, lstProfileIDs, Model.AppliedForThisPeriod).ToList().Translate<Hre_ReportEducationChartListModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region DS HD đến hạn
        [HttpPost]
        public ActionResult GetReportExpiryContract([DataSourceRequest] DataSourceRequest request, Hre_ReportExpiryContractModel Model)
        {
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);

            DateTime From = DateTime.Now.AddMonths(-1);
            DateTime To = DateTime.Now.AddMonths(1);
            if (Model.DateStart != null)
            {
                From = Model.DateStart.Value;
            }
            if (Model.DateEnd != null)
            {
                To = Model.DateEnd.Value;
            }

            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.Status);
            listObj.Add(From);
            listObj.Add(To);
            listObj.Add(Model.CodeEmp);
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.IDNo);
            listObj.Add(Model.WorkPlaceID);
            listObj.Add(Model.DateSignedFrom);
            listObj.Add(Model.DateSignedTo);
            listObj.Add(Model.ContractNo);
            listObj.Add(1);
            listObj.Add(int.MaxValue - 1);
            string status = string.Empty;
            var result = actionServices.GetData<Hre_ReportExpiryContractEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptExpireContract, ref status).ToList().Translate<Hre_ReportExpiryContractModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);
                return Json(fullPath);
                //ExportService.ExportWord(@"D:\app\TestHD.doc", @"D:\app\BD_HDLDNV.doc", result);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetExpiryContract([DataSourceRequest] DataSourceRequest request, Hre_ReportExpiryContractModel Model)
        {
            string status = string.Empty;
            var service = new Hre_ReportServices();
            var actionServices = new ActionService(UserLogin);
            var profileServices = new Hre_ProfileServices();
            var contractServices = new Hre_ContractServices();
            BaseService baseServices = new BaseService();
            bool isshowloopcontract = profileServices.IsNotUseExpiryContractLoop();

            var ShowAfterDate1 = actionServices.GetData<Sys_AllSettingEntity>("HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEAFTE", ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).FirstOrDefault();
            var ShowBeforDate1 = actionServices.GetData<Sys_AllSettingEntity>("HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEBEFOR", ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).FirstOrDefault();
            DateTime? dateTo = null;
            DateTime? dateFrom = null;
            if (isshowloopcontract == false)
            {
                dateTo = DateTime.Now.AddDays(Convert.ToDouble(ShowAfterDate1.Value1));
                dateFrom = DateTime.Now.AddDays(-Convert.ToDouble(ShowBeforDate1.Value1));
            }
            var isDataTable = false;
            object obj = new Hre_ReportExpiryContractModel();
         
            //var lstProfile = new List<Hre_ProfileEntity>();
            var objProfile = new List<object>();
            objProfile.AddRange(new object[2]);
            objProfile[0] = 1;
            objProfile[1] = int.MaxValue - 1;
            var lstProfile = actionServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileDataAll, ref status).ToList();

            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);

            var lstContractType = actionServices.GetData<CatContractTypeModel>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.Status);
            listObj.Add(dateFrom);
            listObj.Add(dateTo);
            listObj.Add(Model.CodeEmp);
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.IDNo);
            listObj.Add(Model.WorkPlaceID);
            listObj.Add(Model.DateSignedFrom);
            listObj.Add(Model.DateSignedTo);
            listObj.Add(Model.ContractNo);
            listObj.Add(1);
            listObj.Add(int.MaxValue - 1);

            var result = actionServices.GetData<Hre_ReportExpiryContractEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptExpireContract, ref status).Where(s => s.StatusEvaluation != WorkdayStatus.E_APPROVED.ToString()).ToList().Translate<Hre_ReportExpiryContractModel>();

           var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContracts = actionServices.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();
           
            Guid[] _RankDetailForNextContract = null;
            if (!string.IsNullOrEmpty(Model.RankDetailForNextContractIds))
            {
                _RankDetailForNextContract = Model.RankDetailForNextContractIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            if (Model.ContractTypeID != null)
            {
                result = result.Where(s => s.ContractTypeID == Model.ContractTypeID).ToList();
            }
            if (!string.IsNullOrEmpty(Model.Status))
            {
                result = result.Where(s => s.Status == Model.Status).ToList();
            }

            var lstModel = new List<Hre_ReportExpiryContractModel>();


            if (_RankDetailForNextContract != null)
            {
                result = result.Where(s => _RankDetailForNextContract.Contains(s.RankDetailForNextContract != null ? s.RankDetailForNextContract.Value : Guid.Empty)).ToList();
            }

            if (Model.EvaType == EnumDropDown.EvaExpiryContract.E_EVA_CONTRACT.ToString())
            {
                result = result.Where(s => s.StatusEvaluation != WorkdayStatus.E_APPROVED.ToString() && s.ContractResult != null).ToList();
                if (isshowloopcontract == false)
                {
                    var model = new Hre_ReportExpiryContractModel();
                    foreach (var item in result)
                    {
                        var ContractByProfileID = lstContracts.Where(s => s.ProfileID == item.ProfileID).OrderByDescending(s => s.DateCreate).FirstOrDefault();
                        if (ContractByProfileID != null)
                        {
                            if (ContractByProfileID.DateCreate != null && ContractByProfileID.DateCreate.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                if (item.DateExtend != null && item.DateExtend >= dateFrom && item.DateExtend <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                                if (item.DateExtend == null && item.DateEnd != null && item.DateEnd >= dateFrom && item.DateEnd <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in result)
                    {
                        var dateSenior = new TimeSpan();
                        double monthSenior = 0;
                        var profileEntity = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        if (profileEntity != null && profileEntity.DateHire != null && Model.DateEnd != null)
                        {
                            dateSenior = Model.DateEnd.Value.Subtract(profileEntity.DateHire.Value);
                            monthSenior = Math.Floor(dateSenior.TotalDays / 30);
                        }

                        item.MonthSenior = (double?)monthSenior;
                        var dateCheck = DateTime.Now;
                        var model = new Hre_ReportExpiryContractModel();
                        var contractTypeEntity = lstContractType.Where(s => item.ContractTypeID == s.ID).FirstOrDefault();
                            if (item.ContractResult == null)
                            {
                                if (contractTypeEntity != null && contractTypeEntity.ExpiryContractLoop != null)
                                {
                                    var dateExpiry = dateCheck.AddDays(contractTypeEntity.ExpiryContractLoop.Value);

                                    if (item.DateExtend != null && item.DateExtend <= dateExpiry)
                                    {
                                        model = item;
                                        lstModel.Add(model);
                                    }
                                    if (item.DateExtend == null && item.DateEnd != null && item.DateEnd.Value <= dateExpiry)
                                    {
                                        model = item;
                                        lstModel.Add(model);
                                    }
                                }
                            }
                        
                    }
                }

            }
            else if (Model.EvaType == EnumDropDown.EvaExpiryContract.E_NONEEVA_CONTRACT.ToString()) 
            {
                result = result.Where(s =>  s.ContractResult == null).ToList();
                if (isshowloopcontract == false)
                {
                    var model = new Hre_ReportExpiryContractModel();
                    foreach (var item in result)
                    {
                        var ContractByProfileID = lstContracts.Where(s => s.ProfileID == item.ProfileID).OrderByDescending(s => s.DateCreate).FirstOrDefault();
                        if (ContractByProfileID != null)
                        {
                            if (ContractByProfileID.DateCreate != null && ContractByProfileID.DateCreate.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                if (item.DateExtend != null && item.DateExtend >= dateFrom && item.DateExtend <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                                if (item.DateExtend == null && item.DateEnd != null && item.DateEnd >= dateFrom && item.DateEnd <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in result)
                    {
                        var dateSenior = new TimeSpan();
                        double monthSenior = 0;
                        var profileEntity = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        if (profileEntity != null && profileEntity.DateHire != null && Model.DateEnd != null)
                        {
                            dateSenior = Model.DateEnd.Value.Subtract(profileEntity.DateHire.Value);
                            monthSenior = Math.Floor(dateSenior.TotalDays / 30);
                        }

                        item.MonthSenior = (double?)monthSenior;
                        var dateCheck = DateTime.Now;
                        var model = new Hre_ReportExpiryContractModel();
                        var contractTypeEntity = lstContractType.Where(s => item.ContractTypeID == s.ID).FirstOrDefault();
                        if (item.ContractResult == null)
                        {
                            if (contractTypeEntity != null && contractTypeEntity.ExpiryContractLoop != null)
                            {
                                var dateExpiry = dateCheck.AddDays(contractTypeEntity.ExpiryContractLoop.Value);

                                if (item.DateExtend != null && item.DateExtend <= dateExpiry)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                                if (item.DateExtend == null && item.DateEnd != null && item.DateEnd.Value <= dateExpiry)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                if (isshowloopcontract == false)
                {
                    var model = new Hre_ReportExpiryContractModel();
                    foreach (var item in result)
                    {
                        var ContractByProfileID = lstContracts.Where(s => s.ProfileID == item.ProfileID).OrderByDescending(s => s.DateCreate).FirstOrDefault();
                        if (ContractByProfileID != null)
                        {
                            if (ContractByProfileID.DateCreate != null && ContractByProfileID.DateCreate.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                            {
                                if (item.DateExtend != null && item.DateExtend >= dateFrom && item.DateExtend <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                                if (item.DateExtend == null && item.DateEnd != null && item.DateEnd >= dateFrom && item.DateEnd <= dateTo)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in result)
                    {
                        var dateSenior = new TimeSpan();
                        double monthSenior = 0;
                        var profileEntity = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        if (profileEntity != null && profileEntity.DateHire != null && Model.DateEnd != null)
                        {
                            dateSenior = Model.DateEnd.Value.Subtract(profileEntity.DateHire.Value);
                            monthSenior = Math.Floor(dateSenior.TotalDays / 30);
                        }

                        item.MonthSenior = (double?)monthSenior;
                        var dateCheck = DateTime.Now;
                        var model = new Hre_ReportExpiryContractModel();
                        var contractTypeEntity = lstContractType.Where(s => item.ContractTypeID == s.ID).FirstOrDefault();
                      
                            if (contractTypeEntity != null && contractTypeEntity.ExpiryContractLoop != null)
                            {
                                var dateExpiry = dateCheck.AddDays(contractTypeEntity.ExpiryContractLoop.Value);

                                if (item.DateExtend != null && item.DateExtend <= dateExpiry)
                                {
                                    model = item;
                                    lstModel.Add(model);
                                }
                                if (item.DateExtend == null && item.DateEnd != null && item.DateEnd.Value <= dateExpiry)
                                {
                                    model = item;
                                    lstModel.Add(model); 
                                }
                            }
                    }
                }
            }
            
            #region Lấy phụ lục hợp đông
            var _ReportService = new Hre_ContractServices();
            var lisEntity = result.Translate<Hre_ContractEntity>();
            DataTable tb = _ReportService.GetDataContract(lisEntity, UserLogin);
            #endregion
            #region Xuất template

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tb,
                    FileName = "Hre_ContractEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, tb, Model.ExportType);
                return Json(fullPath);
            }
            return Json(lstModel.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult CheckAddNewContract(string ContractTypeID, string ContractID)
        {
            string status = string.Empty;
            var contractTypeID = Guid.Empty;
            var message = string.Empty;
            if (!string.IsNullOrEmpty(ContractTypeID))
            {
                contractTypeID = Common.ConvertToGuid(ContractTypeID);
            }
            var contractID = Common.ConvertToGuid(ContractID);

            ActionService service = new ActionService(UserLogin);
            var lstContract = service.GetData<Hre_ContractEntity>(contractID, ConstantSql.hrm_hr_sp_get_ContractById, ref status).FirstOrDefault();

            List<object> listObj = new List<object>();

            listObj.Add(1);
            listObj.Add(1000);
            var actionServices = new ActionService(UserLogin);
            var lstBasicSalary = actionServices.GetData<Sal_BasicSalaryEntity>(listObj, ConstantSql.hrm_sal_sp_get_BasicPayrollGetAll, ref status);
            Sal_BasicSalaryEntity basicSalary = new Sal_BasicSalaryEntity();
            if (lstBasicSalary != null && lstBasicSalary.Count != 0)
            {
                basicSalary = lstBasicSalary.Where(s => s.ProfileID == lstContract.ProfileID && s.DateOfEffect <= lstContract.DateEnd && lstContract.ProfileID == s.ProfileID).OrderByDescending(s => s.DateOfEffect).FirstOrDefault();
            }
            if (basicSalary == null || lstBasicSalary.Count <= 0)
            {
                message = "Error";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            var result = basicSalary.CopyData<Hre_ReportExpiryContractModel>();
            result.ContractTypeID = contractTypeID;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckAddNextContract(string ContractTypeID, string ContractID)
        {
            string status = string.Empty;
            var contractTypeID = Guid.Empty;
            Guid convertContractID = Guid.Empty;
            string AddMessage = string.Empty;
            List<Guid> lstContractID = new List<Guid>();
            if (!string.IsNullOrEmpty(ContractTypeID))
            {
                contractTypeID = Guid.Parse(ContractTypeID);
            }
            if (ContractID.IndexOf(",") > 1)
            {
                var lstID = ContractID.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertContractID = Common.ConvertToGuid(lstID[i]);
                    lstContractID.Add(convertContractID);
                }
            }
            else
            {
                convertContractID = Common.ConvertToGuid(ContractID);
                lstContractID.Add(convertContractID);
            }

            ActionService service = new ActionService(UserLogin);
            var actionServices = new ActionService(UserLogin);
            var contractServices = new Hre_ContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContracts = actionServices.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).Where(s => lstContractID.Contains(s.ID)).ToList();
            var lstContract = service.GetData<Hre_ContractEntity>(convertContractID, ConstantSql.hrm_hr_sp_get_ContractById, ref status).FirstOrDefault();

            var contractTypeServices = new Cat_ContractTypeServices();
            List<object> lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(10000000);
            var lstContractType = actionServices.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status);

            List<object> listObj = new List<object>();

            listObj.Add(1);
            listObj.Add(1000);
            var BasicSalaryServices = new Sal_BasicSalaryServices();
            var lstBasicSalary = actionServices.GetData<Sal_BasicSalaryEntity>(listObj, ConstantSql.hrm_sal_sp_get_BasicPayrollGetAll, ref status).Where(s => s.ProfileID == lstContract.ProfileID && s.DateOfEffect <= lstContract.DateEnd).OrderByDescending(s => s.DateOfEffect).FirstOrDefault();

            foreach (var hreContract in lstContracts)
            {
                //   lstContractType = lstContractType.Where( s=> s.ID == hreContract.ContractTypeID).ToList();
                string contractnextID = lstContractType.Where(s => s.ID == hreContract.ContractTypeID).Select(s => s.ContractNextID).FirstOrDefault();
                if (contractnextID == null)
                {
                    AddMessage = "Error";
                    return Json(AddMessage);
                }
                else
                {
                    Guid contractTypeIDbyContractnext = Common.OracleToDotNet(contractnextID);
                    Hre_ContractEntity contract = new Hre_ContractEntity();
                    hreContract.CopyData(contract);

                    contract.ContractTypeID = contractTypeIDbyContractnext;
                    // contract.ContractNo = getContractNo(contract, contract.DateSigned ?? DateTime.Now);
                    if (contract.DateEnd != null)
                    {
                        contract.DateSigned = contract.DateStart = contract.DateEnd.Value.AddDays(1);
                    }
                    Cat_ContractTypeEntity catContractType = new Cat_ContractTypeEntity();

                    catContractType = lstContractType.Where(s => s.ID == contractTypeIDbyContractnext).FirstOrDefault();

                    if (catContractType != null)
                    {
                        int month = 0;
                        if (catContractType.ValueTime != null)
                        {
                            month = (int)catContractType.ValueTime.Value;
                        }
                        if (catContractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                        {
                            month = month * 12;
                        }

                        if (hreContract.DateEnd != null)
                        {
                            contract.DateStart = hreContract.DateEnd.Value.AddDays(1);

                            contract.DateEnd = contract.DateStart.AddMonths(month);
                        }
                    }
                    else
                    {
                        contract.DateEnd = null;
                    }
                    if (lstBasicSalary != null)
                    {
                        contract.InsuranceAmount = lstBasicSalary.InsuranceAmount;
                        contract.CurenncyID1 = lstBasicSalary.CurrencyID1;
                        contract.ClassRateID = lstBasicSalary.ClassRateID;
                        contract.RankRateID = lstBasicSalary.RankRateID;
                    }
                    //Add new
                    AddMessage = contractServices.Add(contract);
                    //AddMessage = "Success";
                }

            }
            return Json(AddMessage);

        }



        public string getContractNo(Hre_ContractEntity contract, DateTime? DateSigned)
        {
            var profileService = new Hre_ProfileServices();
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstObjProfile = new List<object>();
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(1);
            lstObjProfile.Add(100000000);
            var profile = actionService.GetData<Hre_ProfileEntity>(contract.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            if (profile == null)
                return "";

            //string contractNo = GetNewCode(contract, Hre_Contract.FieldNames.ContractNo);
            //if (contractNo.IsNotNullOrEmpty())
            //{
            //    return contractNo;
            //}
            var ContractServices = new Hre_ContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContracts = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status);
            string contractNo = "HD/" + profile.CodeEmp + "/" + (DateSigned == null ? string.Empty : DateSigned.Value.Year.ToString());
            int contractNumber = lstContracts.Count(s => s.IsDelete == null);

            if (contractNumber > 1)
            {
                contractNo += "-" + contractNumber;
            }

            return contractNo;
        }

        [HttpPost]
        public ActionResult GetContractTypeByID(string ContractTypeID, string ProfileID)
        {
            string status = string.Empty;
            var contractTypeID = Guid.Empty;
            var profileID = Guid.Empty;

            if (!string.IsNullOrEmpty(ContractTypeID))
            {
                contractTypeID = Common.ConvertToGuid(ContractTypeID);
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                if (ProfileID.IndexOf(',') > 1)
                {
                    var profileIDs = ProfileID.Split(',');
                    profileID = Common.ConvertToGuid(profileIDs[0]);
                }
                else
                {
                    profileID = Common.ConvertToGuid(ProfileID);
                }
            }

            var actionService = new ActionService(UserLogin);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(contractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status).FirstOrDefault();

            var profileServices = new Hre_ProfileServices();
            var profile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            var contractServices = new Hre_ContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContracts = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status);

            var contractByprofile = lstContracts.Where(s => s.ProfileID == profileID).OrderByDescending(s => s.DateEnd).FirstOrDefault();

            if (lstContractType != null)
            {
                int month = 0;
                if (lstContractType.Type == EnumDropDown.TypeContract.E_PROBATION.ToString() && profile != null)
                {
                    if (profile.DateEndProbation != null)
                    {
                        lstContractType.DateStart = profile.DateHire;
                        lstContractType.DateSigned = profile.DateHire;
                        lstContractType.DateEnd = profile.DateEndProbation;
                    }
                    else
                    {
                        lstContractType.DateStart = DateTime.Now;
                    }
                }

                else
                {
                    if (contractByprofile != null && contractByprofile.DateEnd != null)
                    {
                        lstContractType.DateStart = contractByprofile.DateEnd.Value.Date.AddDays(1);
                        lstContractType.DateSigned = lstContractType.DateStart;
                    }
                    else
                    {
                        lstContractType.DateStart = profile.DateHire;
                        lstContractType.DateSigned = profile.DateHire;

                    }
                }

                if (lstContractType.ValueTime != null)
                {
                    month = (int)lstContractType.ValueTime.Value;
                    if (lstContractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                    {
                        month = month * 12;
                    }
                    lstContractType.DateEnd = lstContractType.DateStart.Value.AddMonths(month);

                }
                if (!string.IsNullOrEmpty(lstContractType.Formula))
                {
                    if (contractByprofile != null)
                    {
                        contractByprofile = SetNewDateEndContract(contractByprofile);
                        lstContractType.DateEnd = contractByprofile.DateEnd;
                    }
                    else
                    {
                        Hre_ContractEntity contractEntity = new Hre_ContractEntity();
                        contractEntity.DateStart = profile.DateHire.Value;
                        contractEntity.ContractTypeID = lstContractType.ID;
                        contractEntity = SetNewDateEndContract(contractEntity);
                        lstContractType.DateEnd = contractEntity.DateEnd;
                    }
                }

            }

            return Json(lstContractType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDateByContractTypeID(string ProfileID, string ContractTypeID, DateTime dateStart)
        {

            string status = string.Empty;
            var contractTypeID = Guid.Empty;
            var profileID = Guid.Empty;

            if (!string.IsNullOrEmpty(ContractTypeID))
            {
                contractTypeID = Common.ConvertToGuid(ContractTypeID);
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                if (ProfileID.IndexOf(',') > 1)
                {
                    var profileIDs = ProfileID.Split(',');
                    profileID = Common.ConvertToGuid(profileIDs[0]);
                }
                else
                {
                    profileID = Common.ConvertToGuid(ProfileID);
                }
            }

            var actionService = new ActionService(UserLogin);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(contractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status).FirstOrDefault();

            var profile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContracts = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status);

            var contractByprofile = lstContracts.Where(s => s.ProfileID == profileID).OrderByDescending(s => s.DateEnd).FirstOrDefault();

            if (lstContractType != null)
            {
                int countDate = 0;
                int month = 0;
                if (lstContractType.Type == EnumDropDown.TypeContract.E_PROBATION.ToString() && profile != null)
                {
                    if (profile.DateEndProbation != null)
                    {
                        lstContractType.DateStart = profile.DateHire;
                        lstContractType.DateSigned = profile.DateHire;
                        lstContractType.DateEnd = profile.DateEndProbation;
                    }
                    else
                    {
                        lstContractType.DateStart = DateTime.Now;
                    }
                }

                else
                {
                    if (contractByprofile != null && contractByprofile.DateEnd != null)
                    {
                        lstContractType.DateStart = contractByprofile.DateEnd.Value.Date.AddDays(1);
                        lstContractType.DateSigned = lstContractType.DateStart;
                    }
                    else
                    {
                        lstContractType.DateStart = profile.DateHire;
                        lstContractType.DateSigned = profile.DateHire;

                    }
                }

                if (lstContractType.ValueTime != null)
                {
                    countDate = (int)dateStart.Subtract(lstContractType.DateStart.Value).TotalDays;
                    month = (int)lstContractType.ValueTime.Value;
                    if (lstContractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                    {
                        month = month * 12;
                    }
                    lstContractType.DateEnd = lstContractType.DateStart.Value.AddMonths(month);
                    lstContractType.DateEnd = lstContractType.DateEnd.Value.AddDays(countDate);
                    lstContractType.DateStart = dateStart;
                    lstContractType.DateSigned = dateStart;
                }
            }

            return Json(lstContractType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDataByProfileID(string ProfileID)
        {
            string status = string.Empty;
            var profileID = Guid.Empty;
            if (ProfileID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            if (lstProfile != null)
            {
                return Json(lstProfile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public ActionResult GetDataByProfileIDv2(string ProfileID)
        {
            string status = string.Empty;
            var profileID = Guid.Empty;
            if (ProfileID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileByIdv2, ref status).FirstOrDefault();
            var salary = actionService.GetData<Sal_BasicSalaryEntity>(profileID, ConstantSql.hrm_sal_sp_get_BasicSalaryByProfileIds, ref status)
                .OrderByDescending(s => s.DateOfEffect).FirstOrDefault();
            if (salary != null)
            {
                lstProfile.BasicSalary = salary.GrossAmount!=null? Convert.ToDouble(salary.GrossAmount):0;
            }
            if (lstProfile != null)
            {
                return Json(lstProfile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetDataCashByCashID(string ProfileID)
        {
            string status = string.Empty;
            var profileID = Guid.Empty;
            if (ProfileID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            if (lstProfile != null)
            {
                return Json(lstProfile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetDataByHDTJobTypeID(string HDTJobTypeID)
        {

            string status = string.Empty;
            var _HDTJobTypeID = Guid.Empty;
            if (HDTJobTypeID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(HDTJobTypeID))
            {
                _HDTJobTypeID = Common.ConvertToGuid(HDTJobTypeID);
            }

            var services = new Cat_HDTJobTypeServices();
            ActionService actionService = new ActionService(UserLogin);
            var HDTJobTypeEntity = actionService.GetData<Cat_HDTJobTypeEntity>(_HDTJobTypeID, ConstantSql.hrm_cat_sp_get_HDTJobTypeById, ref status).FirstOrDefault();

            if (HDTJobTypeEntity != null)
            {
                return Json(HDTJobTypeEntity, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        [HttpPost]
        public ActionResult GetDataByUserLoginID(string ProfileID)
        {

            string status = string.Empty;
            var profileID = Guid.Empty;
            if (ProfileID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            var profileServices = new Hre_ProfileServices();
            ActionService service = new ActionService(UserLogin);
            var userEntity = service.GetByIdUseStore<Sys_UserInfoEntity>(profileID, ConstantSql.hrm_sys_sp_get_UserbyId, ref status);
            var lstProfile = service.GetByIdUseStore<Hre_ProfileEntity>(userEntity.ProfileID.Value, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);

            if (lstProfile != null)
            {
                return Json(lstProfile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

     
        [HttpPost]
        public ActionResult GetBasicSalaryByProfileID(string ProfileID)
        {
            string status = string.Empty;
            Guid profileID = Guid.Empty;
            string message = string.Empty;
            if (string.IsNullOrEmpty(ProfileID))
            {
                message = "Error";
                return Json(message);
            }
            profileID = Common.ConvertToGuid(ProfileID);

            List<object> listObj = new List<object>();

            listObj.Add(1);
            listObj.Add(1000);
            var actionService = new ActionService(UserLogin);
            var lstBasicSalary = actionService.GetData<Sal_BasicSalaryEntity>(listObj, ConstantSql.hrm_sal_sp_get_BasicPayrollGetAll, ref status).Where(s => s.ProfileID == profileID && s.DateOfEffect != null).FirstOrDefault();
            if (lstBasicSalary == null)
            {
                message = "NoBasicSalary";
                return Json(message);
            }
            return Json(lstBasicSalary, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProfileIDs([Bind]Hre_ContractModel model)
        {
            #region Validate

            string message_Error = string.Empty;

            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ContractModel>(model, "Hre_Contract", ref message_Error);
            if (!checkValidate)
            {
                model.ActionStatus = message_Error;
                return Json(message_Error, JsonRequestBehavior.AllowGet);
            }
            if (model.ContractEvaType == "E_ANNUAL_EVALUATION")
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ContractModel>(model, "EvaContractinfo", "Hre_Contract", ref message_Error);
                if (!checkValidate)
                {
                    model.ActionStatus = message_Error;
                    return Json(message_Error, JsonRequestBehavior.AllowGet);
                }
            }

            #endregion
            string status = string.Empty;
            Guid convertProfileID = Guid.Empty;
            string message = string.Empty;
            List<Guid> lstProfileID = new List<Guid>();
            var insuranceServices = new Sal_InsuranceSalaryServices();
            var ContractServices = new Hre_ContractServices();
            var hrService = new Hre_ProfileServices();
            var actionService = new ActionService(UserLogin);
            if (model.ID == Guid.Empty)
            {
                model.DateExtend = model.DateEnd;
            }

            if (model.ProfileIDs != null && model.ProfileIDs.IndexOf(',') > 1)
            {
                var lstID = model.ProfileIDs.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertProfileID = Common.ConvertToGuid(lstID[i]);
                    lstProfileID.Add(convertProfileID);
                }
            }

            else
            {
                convertProfileID = Common.ConvertToGuid(model.ProfileIDs);
                lstProfileID.Add(convertProfileID);
            }

            var lstProfile = new List<Hre_ProfileEntity>();
            if (model.ProfileIDs != null)
            {
                lstProfile = actionService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
            }
            if (!string.IsNullOrEmpty(model.OrgStructureIDs))
            {
                List<Guid> listGuid = new List<Guid>();
                if (model.ProfileIDs != null)
                {
                    var listStr = model.ProfileIDs.Split(',');

                    if (listStr[0] != "")
                    {

                        foreach (var item in listStr)
                        {
                            listGuid.Add(Guid.Parse(item));
                        }
                    }
                }
                string strIDs = string.Empty;
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureIDs);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstProfileids = actionService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, ref status).Select(s => s.ID).ToList();
                if (listGuid != null)
                {
                    lstProfileids = lstProfileids.Where(s => !listGuid.Contains(s)).ToList();

                    foreach (var item in lstProfileids)
                    {
                        strIDs += Common.DotNetToOracle(item.ToString()) + ",";
                    }
                    if (strIDs.IndexOf(",") > 0)
                        strIDs = strIDs.Substring(0, strIDs.Length - 1);
                    var lstProfileadd = actionService.GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);
                    lstProfile.AddRange(lstProfileadd);
                }
                if (lstProfileids.Count == 0 && model.ProfileID == Guid.Empty)
                {
                    model.ActionStatus = ConstantDisplay.HRM_Common_NotEmployee.TranslateString();
                    return Json(model.ActionStatus, JsonRequestBehavior.AllowGet);
                    lstProfile = lstProfile.Where(s => lstProfileids.Contains(s.ID)).ToList();
                }
            }

            var contractTypeService = new Cat_ContractTypeServices();
            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            var insuranceConfigServices = new Cat_InsuranceConfigServices();
            var objInsuranceConfig = new List<object>();
            objInsuranceConfig.Add(1);
            objInsuranceConfig.Add(int.MaxValue - 1);
            var lstInsuranceConfig = actionService.GetData<Cat_InsuranceConfigEntity>(objInsuranceConfig, ConstantSql.hrm_cat_sp_get_InsuranceConfig, ref status).ToList();

            foreach (var profile in lstProfile)
            {
                model.ProfileID = profile.ID;
                var objContract = new List<object>();
                objContract.Add(model.ProfileID);
                var lstContractByProfileID = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
                var contractTypeEntity = lstContractType.Where(s => s.ID == model.ContractTypeID).FirstOrDefault();
                #region Xử Lý Lương BHXH

                if (contractTypeEntity != null && contractTypeEntity.NoneTypeInsuarance == true)
                {
                    var insuranceEntity = new Sal_InsuranceSalaryEntity
                    {
                        ProfileID = model.ProfileID,
                        InsuranceAmount = model.InsuranceAmount,
                        DateEffect = model.DateStart,
                        IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                        IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                        IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                        CurrencyID = model.CurenncyID1
                    };
                    message = insuranceServices.Add(insuranceEntity);
                }
                if (contractTypeEntity != null && contractTypeEntity.NoneTypeInsuarance == false)
                {
                    var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID == model.ContractTypeID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    if (insuranceConfigEntity != null)
                    {
                        var insuranceEntity = new Sal_InsuranceSalaryEntity
                        {
                            ProfileID = model.ProfileID,
                            InsuranceAmount = model.InsuranceAmount,
                            DateEffect = model.DateStart,
                            IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                            IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                            IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                            CurrencyID = model.CurenncyID1
                        };
                        message = insuranceServices.Add(insuranceEntity);
                    }
                }
                #endregion
                if (model.CreateBasicSalary == true)
                {
                    Sal_BasicSalaryServices salaryservices = new Sal_BasicSalaryServices();
                    Sal_BasicSalaryEntity basicSalaryBycontract = new Sal_BasicSalaryEntity();

                    basicSalaryBycontract.ProfileID = profile.ID;
                    basicSalaryBycontract.GrossAmount = model.Salary != null ? model.Salary.ToString() : "0";
                    basicSalaryBycontract.CurrencyID = model.CurenncyID != null ? model.CurenncyID.Value : Guid.Empty;
                    basicSalaryBycontract.PersonalRate = model.PersonalRate;
                    basicSalaryBycontract.DateOfEffect = model.DateStart;

                    basicSalaryBycontract.InsuranceAmount = model.InsuranceAmount != null ? model.InsuranceAmount.Value : 0;
                    basicSalaryBycontract.CurrencyID1 = model.CurenncyID1;

                    basicSalaryBycontract.ClassRateID = model.ClassRateID;
                    basicSalaryBycontract.RankRateID = model.RankRateID;

                    basicSalaryBycontract.AllowanceType1ID = model.AllowanceID1;
                    basicSalaryBycontract.AllowanceAmount1 = model.Allowance1;
                    basicSalaryBycontract.CurrencyID2 = model.CurenncyID2;

                    basicSalaryBycontract.AllowanceType2ID = model.AllowanceID2;
                    basicSalaryBycontract.AllowanceAmount2 = model.Allowance2;
                    basicSalaryBycontract.CurrencyID3 = model.CurenncyID3;

                    basicSalaryBycontract.AllowanceType3ID = model.AllowanceID3;
                    basicSalaryBycontract.AllowanceAmount3 = model.Allowance3;
                    basicSalaryBycontract.CurrencyID4 = model.CurenncyIDSalary;

                    salaryservices.Add(basicSalaryBycontract);
                }

                if (model.BasicSalaryForPerson == true)
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(1);
                    listObj.Add(1000);
                    var BasicSalaryServices = new Sal_BasicSalaryServices();
                    var basicSalaryByProfile = actionService.GetData<Sal_BasicSalaryEntity>(Common.DotNetToOracle(profile.ID.ToString()), ConstantSql.hrm_sal_sp_get_BasicSalaryByProfileIds, ref status).FirstOrDefault();
                    Hre_ContractEntity contractgetNo = new Hre_ContractEntity();
                    model.CopyData(contractgetNo);
                    Hre_ContractEntity contract = new Hre_ContractEntity();

                    contract.Allowance = model.Allowance;
                    contract.Allowance1 = model.Allowance1;
                    contract.Allowance2 = model.Allowance2;
                    contract.Allowance3 = model.Allowance3;
                    contract.Allowance4 = model.Allowance4;
                    if (basicSalaryByProfile != null)
                    {
                        contract.AllowanceID1 = basicSalaryByProfile.AllowanceType1ID != null ? basicSalaryByProfile.AllowanceType1ID : null;
                        contract.AllowanceID2 = basicSalaryByProfile.AllowanceType2ID != null ? basicSalaryByProfile.AllowanceType2ID : null;
                        contract.AllowanceID3 = basicSalaryByProfile.AllowanceType3ID != null ? basicSalaryByProfile.AllowanceType3ID : null;
                        contract.AllowanceID4 = basicSalaryByProfile.AllowanceType4ID != null ? basicSalaryByProfile.AllowanceType4ID : null;
                        contract.ClassRateID = basicSalaryByProfile.ClassRateID != null ? basicSalaryByProfile.ClassRateID : null;
                        contract.CurenncyID = basicSalaryByProfile.CurrencyID;
                        contract.CurenncyID1 = basicSalaryByProfile.CurrencyID1 != null ? basicSalaryByProfile.CurrencyID1 : null;
                        contract.CurenncyID2 = basicSalaryByProfile.CurrencyID2 != null ? basicSalaryByProfile.CurrencyID2 : null;
                        contract.CurenncyID3 = basicSalaryByProfile.CurrencyID3 != null ? basicSalaryByProfile.CurrencyID3 : null;
                        contract.CurenncyID4 = basicSalaryByProfile.CurrencyID4 != null ? basicSalaryByProfile.CurrencyID4 : null;
                        contract.CurenncyID5 = basicSalaryByProfile.CurrencyID5 != null ? basicSalaryByProfile.CurrencyID5 : null;
                    }
                    contract.Code = model.Code;
                    contract.CodeEmp = model.CodeEmp;
                    contract.ContractTypeID = model.ContractTypeID;
                    contract.CurenncyIDSalary = model.CurenncyIDSalary;
                    contract.CurenncyInsName = model.CurenncyInsName;
                    contract.CurenncyOAllowanceName = model.CurenncyOAllowanceName;
                    contract.CurrencySalName = model.CurrencySalName;
                    contract.DateAuthorize = model.DateAuthorize;
                    contract.DateCreate = model.DateCreate;
                    contract.DateUpdate = DateTime.Now;
                    contract.DateEnd = model.DateEnd;
                    contract.DateSigned = model.DateSigned;
                    contract.DateStart = model.DateStart;
                    contract.FollowNo = model.FollowNo;
                    contract.FormPaySalary = model.FormPaySalary;
                    contract.HourWorkInMonth = model.HourWorkInMonth;
                    contract.InsuranceAmount = model.InsuranceAmount;
                    contract.IPCreate = model.IPCreate;
                    contract.IPUpdate = model.IPUpdate;
                    contract.JobTitleID = model.JobTitleID;
                    contract.PersonalRate = model.PersonalRate;
                    contract.PositionID = model.PositionID;
                    contract.ProfileID = model.ProfileID;
                    contract.ProfileSingID = model.ProfileSingID;
                    contract.ProfileName = model.ProfileName;
                    contract.ProfileSingName = model.ProfileSingName;
                    contract.QualificationID = model.QualificationID;
                    contract.RankRateID = basicSalaryByProfile.RankRateID;
                    contract.Salary = basicSalaryByProfile == null ? (double?)null : Convert.ToDouble(basicSalaryByProfile.GrossAmount);
                    contract.ClassRateID = basicSalaryByProfile.ClassRateID;
                    contract.SalaryClassTypeID = model.SalaryClassTypeID;
                    contract.ServerCreate = model.ServerCreate;
                    contract.ServerUpdate = model.ServerUpdate;
                    contract.WorkPlaceID = model.WorkPlaceID;
                    contract.NextContractTypeID = model.NextContractTypeID;
                    contract.Remark = model.Remark;
                    contract.RankDetailForNextContract = model.RankDetailForNextContract;
                    contract.ContractEvaType = model.ContractEvaType;
                    contract.DateOfContractEva = model.DateOfContractEva;
                    contract.EvaluationResult = model.EvaluationResult;
                    contract.ContractResult = model.ContractResult;
                    contract.TypeOfPass = model.TypeOfPass;
                    contract.DateEndNextContract = model.DateEndNextContract;
                    contract.DateExtend = model.DateEnd;
                    contract.Status = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_WAITING.ToString();

                    if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                    {
                        contract = SetNewDateEndContract(contract);
                    }
                    //Tạo mã cho hợp đồng
                    //   contract = SetNewCodeContract(contract, listIdContract);
                    if (contract.ID == Guid.Empty)
                    {
                        contract.ActionStatus = ContractServices.Add(contract);
                    }
                    else
                    {
                        contract.ActionStatus = ContractServices.Edit(contract);
                    }

                    return Json(contract, JsonRequestBehavior.AllowGet);
                }

                Hre_ContractEntity contractgetNoGetBasicSalary = new Hre_ContractEntity();
                model.CopyData(contractgetNoGetBasicSalary);
                Hre_ContractEntity contractNoGetBasicSalary = new Hre_ContractEntity();
                contractNoGetBasicSalary.Allowance = model.Allowance;
                contractNoGetBasicSalary.Allowance1 = model.Allowance1;
                contractNoGetBasicSalary.Allowance3 = model.Allowance3;
                contractNoGetBasicSalary.Allowance2 = model.Allowance2;
                contractNoGetBasicSalary.Allowance4 = model.Allowance4;
                contractNoGetBasicSalary.AllowanceID1 = model.AllowanceID1;
                contractNoGetBasicSalary.AllowanceID2 = model.AllowanceID2;
                contractNoGetBasicSalary.AllowanceID3 = model.AllowanceID3;
                contractNoGetBasicSalary.AllowanceID4 = model.AllowanceID4;
                contractNoGetBasicSalary.ClassRateID = model.ClassRateID;
                contractNoGetBasicSalary.ClassRateName = model.ClassRateName;
                contractNoGetBasicSalary.Code = model.Code;
                contractNoGetBasicSalary.CodeEmp = model.CodeEmp;
                contractNoGetBasicSalary.ContractTypeID = model.ContractTypeID;
                contractNoGetBasicSalary.CurenncyID = model.CurenncyID;
                contractNoGetBasicSalary.CurenncyID1 = model.CurenncyID1;
                contractNoGetBasicSalary.CurenncyID2 = model.CurenncyID2;
                contractNoGetBasicSalary.CurenncyID3 = model.CurenncyID3;
                contractNoGetBasicSalary.CurenncyID4 = model.CurenncyID4;
                contractNoGetBasicSalary.CurenncyID5 = model.CurenncyID5;
                contractNoGetBasicSalary.CurenncyIDSalary = model.CurenncyIDSalary;
                contractNoGetBasicSalary.CurenncyInsName = model.CurenncyInsName;
                contractNoGetBasicSalary.CurenncyOAllowanceName = model.CurenncyOAllowanceName;
                contractNoGetBasicSalary.CurrencySalName = model.CurrencySalName;
                contractNoGetBasicSalary.DateAuthorize = model.DateAuthorize;
                contractNoGetBasicSalary.DateCreate = model.DateCreate;
                contractNoGetBasicSalary.DateEnd = model.DateEnd;
                contractNoGetBasicSalary.DateSigned = model.DateSigned;
                contractNoGetBasicSalary.DateStart = model.DateStart;
                contractNoGetBasicSalary.DateUpdate = DateTime.Now;
                contractNoGetBasicSalary.FollowNo = model.FollowNo;
                contractNoGetBasicSalary.FormPaySalary = model.FormPaySalary;
                contractNoGetBasicSalary.HourWorkInMonth = model.HourWorkInMonth;
                contractNoGetBasicSalary.InsuranceAmount = model.InsuranceAmount;
                contractNoGetBasicSalary.IPCreate = model.IPCreate;
                contractNoGetBasicSalary.IPUpdate = model.IPUpdate;
                contractNoGetBasicSalary.JobTitleID = model.JobTitleID;
                contractNoGetBasicSalary.PersonalRate = model.PersonalRate;
                contractNoGetBasicSalary.PositionID = model.PositionID;
                contractNoGetBasicSalary.ProfileID = model.ProfileID;
                contractNoGetBasicSalary.ProfileSingID = model.ProfileSingID;
                contractNoGetBasicSalary.ProfileName = model.ProfileName;
                contractNoGetBasicSalary.ProfileSingName = model.ProfileSingName;
                contractNoGetBasicSalary.QualificationID = model.QualificationID;
                contractNoGetBasicSalary.RankRateID = model.RankRateID;
                contractNoGetBasicSalary.Salary = model.Salary;
                contractNoGetBasicSalary.SalaryClassTypeID = model.SalaryClassTypeID;
                contractNoGetBasicSalary.ServerCreate = model.ServerCreate;
                contractNoGetBasicSalary.ServerUpdate = model.ServerUpdate;
                contractNoGetBasicSalary.WorkPlaceID = model.WorkPlaceID;
                contractNoGetBasicSalary.NextContractTypeID = model.NextContractTypeID;
                contractNoGetBasicSalary.Remark = model.Remark;
                contractNoGetBasicSalary.RankDetailForNextContract = model.RankDetailForNextContract;
                contractNoGetBasicSalary.ContractEvaType = model.ContractEvaType;
                contractNoGetBasicSalary.DateOfContractEva = model.DateOfContractEva;
                contractNoGetBasicSalary.EvaluationResult = model.EvaluationResult;
                contractNoGetBasicSalary.ContractResult = model.ContractResult;
                contractNoGetBasicSalary.TypeOfPass = model.TypeOfPass;
                contractNoGetBasicSalary.DateEndNextContract = model.DateEndNextContract;
                contractNoGetBasicSalary.DateExtend = model.DateEnd;
                contractNoGetBasicSalary.Status = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_WAITING.ToString();


                if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                {
                    contractNoGetBasicSalary = SetNewDateEndContract(contractNoGetBasicSalary);
                }
                //Tạo mã cho hợp đồng
                // contractNoGetBasicSalary = SetNewCodeContract(contractNoGetBasicSalary, listIdContract);
                if (contractNoGetBasicSalary.ID == Guid.Empty)
                {
                    contractNoGetBasicSalary.ActionStatus = ContractServices.Add(contractNoGetBasicSalary);
                }
                else
                {
                    contractNoGetBasicSalary.ActionStatus = ContractServices.Edit(contractNoGetBasicSalary);
                }

                message = contractNoGetBasicSalary.ActionStatus;
                return Json(contractNoGetBasicSalary, JsonRequestBehavior.AllowGet);

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditContractByEvaContract([Bind]Hre_ContractModel model)
        {
            string status = string.Empty;
            Guid convertProfileID = Guid.Empty;
            string message = string.Empty;
            var lstContractEidt = new List<Hre_ContractEntity>();
            List<Guid> lstIDs = new List<Guid>();
            var insuranceServices = new Sal_InsuranceSalaryServices();
            var contractServices = new Hre_ContractServices();

            if (model.selectedIds != null && model.selectedIds.IndexOf(',') > 1)
            {
                var lstID = model.selectedIds.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertProfileID = Common.ConvertToGuid(lstID[i]);
                    lstIDs.Add(convertProfileID);
                }
            }
            else
            {
                convertProfileID = Common.ConvertToGuid(model.selectedIds);
                lstIDs.Add(convertProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var appendixContractServices = new Hre_AppendixContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContract = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();
            if (lstIDs != null)
            {
                lstContract = lstContract.Where(s => lstIDs.Contains(s.ID)).ToList();
            }

            var objAppendixContarct = new List<object>();
            objAppendixContarct.AddRange(new object[7]);
            objAppendixContarct[5] = 1;
            objAppendixContarct[6] = int.MaxValue - 1;
            var lstAppendixContract = actionService.GetData<Hre_AppendixContractEntity>(objAppendixContarct, ConstantSql.hrm_hr_sp_get_AppendixContract, ref status).ToList();

            var contractTypeService = new Cat_ContractTypeServices();
            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();


            foreach (var item in lstContract)
            {
                if (item.DateExtend == null)
                {
                    item.DateExtend = item.DateEnd;
                }

                    item.ContractResult = model.ContractResult;
                    item.ContractEvaType = model.ContractEvaType;
                    item.ContractEvaType = model.ContractEvaType;
                    item.DateOfContractEva = model.DateOfContractEva;
                    item.EvaluationResult = model.EvaluationResult;
                    item.TypeOfPass = model.TypeOfPass;
                    item.RankDetailForNextContract = model.RankDetailForNextContract;
                    item.NextContractTypeID = model.NextContractTypeID;
                    item.StatusEvaluation = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_WAITING.ToString();
                    var nextContracttype = lstContractType.Where(s => s.ID == model.NextContractTypeID).FirstOrDefault();
                    var contractEntity = new Hre_ContractEntity();
                    contractEntity = item.CopyData<Hre_ContractEntity>();
                    if (item.DateEnd != null)
                    {
                        contractEntity.DateStart = item.DateEnd.Value.AddDays(1);
                    }
                    SetNewDateEndNextContractByContractAndNextContractID(contractEntity, model.NextContractTypeID.Value);
                    item.DateEndNextContract = contractEntity.DateEndNextContract;
                    message = contractServices.Edit(item);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public Hre_ContractEntity SetNewCodeContract(Hre_ContractEntity contractEntity, string strIdConstract)
        {
            if (contractEntity != null)
            {
                var actionService = new ActionService(UserLogin);
                var status = string.Empty;
                var objContractType1 = actionService.GetFirstData<Cat_ContractTypeEntity>(Common.DotNetToOracle(contractEntity.ContractTypeID.ToString()), ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
                var objProfile1 = actionService.GetFirstData<Hre_ProfileEntity>(Common.DotNetToOracle(contractEntity.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                if (objContractType1 != null)
                {
                    var contractType = (Cat_ContractTypeEntity)objContractType1;
                    var profileEntity = (Hre_ProfileEntity)objProfile1;
                    if (!string.IsNullOrEmpty(contractType.Type))
                    {
                        var objSysConfig = actionService.GetFirstData<Sys_AllSettingEntity>(AppConfig.HRM_HRE_GENERATE_CODE_CONTRACT.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
                        if (objSysConfig != null)
                        {
                            var sysConfig = (Sys_AllSettingEntity)objSysConfig;
                            if (sysConfig != null && !string.IsNullOrEmpty(sysConfig.Value1))
                            {
                                ElementFormula elementContactType = new ElementFormula("ContactType", contractType.Type, 0);
                                ElementFormula elementCodeEmp = new ElementFormula("CodeEmp", profileEntity.CodeEmp, 0);
                                ElementFormula elementOrdinal = new ElementFormula("Ordinal", 1, 0);

                                var result = FormulaHelper.ParseFormula(sysConfig.Value1, new List<ElementFormula>() { elementContactType, elementCodeEmp });
                                if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                                {
                                    var value = result.Value;
                                    if (value != null)
                                    {
                                        string newCode = value.ToString();
                                        if (newCode.EndsWith("Ordinal"))
                                        {
                                            var strNewCode = newCode.Substring(0, newCode.Length - 7);
                                            if (!string.IsNullOrEmpty(strIdConstract))
                                            {
                                                var listContractType = actionService.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(strIdConstract), ConstantSql.hrm_cat_sp_get_ContractTypeByIds, ref status);
                                                if (listContractType != null)
                                                {
                                                    var listContractTypeById = listContractType.Where(d => d.Type == contractType.Type).FirstOrDefault();
                                                    if (listContractTypeById != null)
                                                    {
                                                        var listId = strIdConstract.Split(',').ToList();
                                                        var count = listId.Where(s => listContractTypeById.ID.ToString() == s).Select(d => d == listContractTypeById.ToString()).Count();
                                                        //var count = listContractTypeById.Count();
                                                        newCode = strNewCode + "-" + (count + 1);
                                                    }
                                                    else
                                                    {
                                                        //var listId = strIdConstract.Split(',').ToList();
                                                        //var count = listId.Select(d => d == listContractTypeById.ID.ToString()).Count();
                                                        newCode = strNewCode + "-" + "1";
                                                    }
                                                }
                                                else
                                                {
                                                    newCode = strNewCode + "-" + 1;
                                                }
                                            }
                                            else
                                            {
                                                newCode = strNewCode + "-" + 1;
                                            }

                                        }
                                        contractEntity.ContractNo = newCode;
                                    }
                                }
                            }
                        }
                    }
                }
                contractEntity.ActionStatus = status;
            }
            return contractEntity;
        }

        public Hre_ContractEntity SetNewDateEndContract(Hre_ContractEntity contractEntity)
        {
            if (contractEntity != null)
            {
                var actionService = new ActionService(UserLogin);
                var status = string.Empty;
                var objContractType = actionService.GetFirstData<Cat_ContractTypeEntity>(contractEntity.ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
                if (objContractType != null)
                {
                    var contractTypeEntity = (Cat_ContractTypeEntity)objContractType;
                    if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                    {
                        var formula = contractTypeEntity.Formula.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        ElementFormula elementContactType = new ElementFormula("ContractType", contractTypeEntity.Type, 0);
                        ElementFormula elementUnitType = new ElementFormula("UnitType", contractTypeEntity.UnitTime, 0);
                        ElementFormula elementValueTime = new ElementFormula("ValueTime", contractTypeEntity.ValueTime, 0);
                        ElementFormula elementDateStart = new ElementFormula("DateStart", contractEntity.DateStart, 0);

                        var result = FormulaHelper.ParseFormula(formula, new List<ElementFormula>() { elementContactType, elementUnitType, elementDateStart, elementValueTime });

                        if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            if (result.Value.GetType().Name == "DateTime")
                            {
                                contractEntity.DateEnd = (DateTime)result.Value;
                            }
                            else
                            {
                                contractEntity.ErrorMessage = contractEntity.ProfileName;

                            }

                        }
                    }
                }

            }
            return contractEntity;
        }

        public Hre_ContractEntity SetNewDateEndNextContractByContractAndNextContractID(Hre_ContractEntity contractEntity, Guid nextContractID)
        {
            if (contractEntity != null)
            {
                var actionService = new ActionService(UserLogin);
                var status = string.Empty;
                var objContractType = actionService.GetFirstData<Cat_ContractTypeEntity>(Common.DotNetToOracle(nextContractID.ToString()), ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
                if (objContractType != null)
                {
                    var contractTypeEntity = (Cat_ContractTypeEntity)objContractType;
                    if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                    {
                        var formula = contractTypeEntity.Formula.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        ElementFormula elementContactType = new ElementFormula("ContractType", contractTypeEntity.Type, 0);
                        ElementFormula elementUnitType = new ElementFormula("UnitType", contractTypeEntity.UnitTime, 0);
                        ElementFormula elementValueTime = new ElementFormula("ValueTime", contractTypeEntity.ValueTime, 0);
                        ElementFormula elementDateStart = new ElementFormula("DateStart", contractEntity.DateStart, 0);

                        var result = FormulaHelper.ParseFormula(formula, new List<ElementFormula>() { elementContactType, elementUnitType, elementDateStart, elementValueTime });

                        if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            if (result.Value.GetType().Name == "DateTime")
                            {
                                contractEntity.DateEndNextContract = (DateTime)result.Value;
                            }
                            else
                            {
                                contractEntity.ErrorMessage = contractEntity.ProfileName;

                            }

                        }
                    }
                }

            }
            return contractEntity;
        }

        #region Tạo Mới HĐ Và Lương Kế Tiếp cho Honda

        [HttpPost]
        public ActionResult GetDataByContractID(string contractID, string contractTypeID)
        {

            string status = string.Empty;

            if (contractID == null)
            {
                return null;
            }
            var actionService = new ActionService(UserLogin);

            var salaryRankServices = new Cat_SalaryRankServices();
            var lstObjSalaryRank = new List<object>();
            lstObjSalaryRank.Add(null);
            lstObjSalaryRank.Add(1);
            lstObjSalaryRank.Add(int.MaxValue - 1);

            var contractTypeServices = new Cat_ContractTypeServices();
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(Common.ConvertToGuid(contractTypeID), ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status).FirstOrDefault();

            var lstSalaryRank = actionService.GetData<Cat_SalaryRankEntity>(lstObjSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();

            var result = actionService.GetData<Hre_ContractEntity>(Common.ConvertToGuid(contractID), ConstantSql.hrm_hr_sp_get_ContractById, ref status).FirstOrDefault();
            if (result != null)
            {
                if (result.ContractEvaType == EnumDropDown.ContractEvaType.E_EXPIRED_APPRENTICE.ToString())
                {
                    int month = 0;
                    if (lstContractType.ValueTime != null)
                    {
                        month = (int)lstContractType.ValueTime.Value;
                        if (lstContractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                        {
                            month = month * 12;
                        }
                        lstContractType.DateStart = result.DateEnd.Value.AddDays(1);
                    }
                    //chưa tìm dc cách xử lý nên hard code 
                    var lstSalaryRankNew = lstSalaryRank.Where(s => s.SalaryRankName == "R0V08").FirstOrDefault();
                    var contractModel = new Hre_ContractModel
                    {
                        //   ContractNo = getContractNo(result, result.DateSigned),
                        ProfileID = result.ProfileID,
                        DateStart = result.DateEnd.Value.AddDays(1),
                        DateSigned = result.DateEnd.Value.AddDays(1),
                        DateEnd = lstContractType.DateStart.Value.AddMonths(month),
                        Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                        RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                        ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                        ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                        SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName

                    };
                    return Json(contractModel, JsonRequestBehavior.AllowGet);
                }

                if (result.ContractEvaType == EnumDropDown.ContractEvaType.E_ANNUAL_EVALUATION.ToString())
                {
                    var contractModel = new Hre_ContractModel
                    {
                        //  ContractNo = getContractNo(result, result.DateSigned),
                        ProfileID = result.ProfileID,
                        DateStart = new DateTime(DateTime.Now.Year, 06, 01),
                    };
                    return Json(contractModel, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult GetDataBySalaryRankID(string rankID)
        {

            string status = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var salaryRankService = new Cat_SalaryRankServices();
            var result = service.GetByIdUseStore<Cat_SalaryRankEntity>(Common.ConvertToGuid(rankID), ConstantSql.hrm_cat_sp_get_SalaryRankById, ref status);
            var model = result.CopyData<Cat_SalaryRankModel>();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLastContractByProfileID(string profileID)
        {
            string status = string.Empty;
            string message = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var actionService = new ActionService(UserLogin);
            var objContract = new List<object>();
            objContract.Add(Common.DotNetToOracle(profileID));
            // Theo task 0049902 dựa vào ngày bắt đầu để lấy hd mới nhất
            var contractEntity = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status).OrderByDescending(s => s.DateStart).FirstOrDefault();
            if (contractEntity != null)
            {
                var model = contractEntity.CopyData<Hre_ContractModel>();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = ConstantMessages.WarningProfileHaveNotContract.ToString().TranslateString();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return null;

        }

        public ActionResult SaveContractAndBasicSalary([Bind]Hre_ContractModel model)
        {
            #region Validate

            string message_Error = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ContractModel>(model, "Hre_Contract", ref message_Error);
            if (!checkValidate)
            {
                model.ActionStatus = message_Error;
                return Json(message_Error, JsonRequestBehavior.AllowGet);
            }

            #endregion
            string status = string.Empty;
            Guid convertProfileID = Guid.Empty;
            var insuranceServices = new Sal_InsuranceSalaryServices();
            string message = string.Empty;
            List<Guid> lstProfileID = new List<Guid>();
            var ContractServices = new Hre_ContractServices();

            if (model.ProfileIDs != null && model.ProfileIDs.IndexOf(',') > 1)
            {
                var lstID = model.ProfileIDs.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertProfileID = Common.ConvertToGuid(lstID[i]);
                    lstProfileID.Add(convertProfileID);
                }
            }
            else
            {
                convertProfileID = Common.ConvertToGuid(model.ProfileIDs);
                lstProfileID.Add(convertProfileID);
            }
            List<object> lstObjProfile = new List<object>();
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(null);
            lstObjProfile.Add(1);
            lstObjProfile.Add(int.MaxValue - 1);
            var actionService = new ActionService(UserLogin);
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status).ToList();
            lstProfile = lstProfile.Where(s => lstProfileID.Contains(s.ID) && s.IsDelete == null).ToList();

            var contractTypeService = new Cat_ContractTypeServices();
            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            foreach (var profile in lstProfile)
            {
                model.ProfileID = profile.ID;
                var contractTypeEntity = lstContractType.Where(s => s.ID == model.ContractTypeID).FirstOrDefault();
                #region Xử Lý Lương BHXH

                if (contractTypeEntity != null && contractTypeEntity.NoneTypeInsuarance == true)
                {
                    var insuranceEntity = new Sal_InsuranceSalaryEntity
                    {
                        ProfileID = model.ProfileID,
                        InsuranceAmount = model.InsuranceAmount,
                        DateEffect = model.DateStart,
                        IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                        IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                        IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                        CurrencyID = model.CurenncyID1
                    };
                    message = insuranceServices.Add(insuranceEntity);
                }
                #endregion
                if (model.CreateBasicSalary == true)
                {
                    Sal_BasicSalaryServices salaryservices = new Sal_BasicSalaryServices();
                    Sal_BasicSalaryEntity basicSalaryBycontract = new Sal_BasicSalaryEntity();

                    basicSalaryBycontract.ProfileID = profile.ID;
                    basicSalaryBycontract.GrossAmount = model.Salary != null ? model.Salary.ToString() : "0";
                    basicSalaryBycontract.CurrencyID = model.CurenncyID != null ? model.CurenncyID.Value : Guid.Empty;
                    basicSalaryBycontract.PersonalRate = model.PersonalRate;
                    basicSalaryBycontract.DateOfEffect = model.DateStart;

                    basicSalaryBycontract.InsuranceAmount = model.InsuranceAmount != null ? model.InsuranceAmount.Value : 0;
                    basicSalaryBycontract.CurrencyID1 = model.CurenncyID1;

                    basicSalaryBycontract.ClassRateID = model.ClassRateID;
                    basicSalaryBycontract.RankRateID = model.RankRateID;

                    basicSalaryBycontract.AllowanceType1ID = model.AllowanceID1;
                    basicSalaryBycontract.AllowanceAmount1 = model.Allowance1;
                    basicSalaryBycontract.CurrencyID2 = model.CurenncyID2;

                    basicSalaryBycontract.AllowanceType2ID = model.AllowanceID2;
                    basicSalaryBycontract.AllowanceAmount2 = model.Allowance2;
                    basicSalaryBycontract.CurrencyID3 = model.CurenncyID3;

                    basicSalaryBycontract.AllowanceType3ID = model.AllowanceID3;
                    basicSalaryBycontract.AllowanceAmount3 = model.Allowance3;
                    basicSalaryBycontract.CurrencyID4 = model.CurenncyIDSalary;

                    salaryservices.Add(basicSalaryBycontract);

                }

                if (model.BasicSalaryForPerson == true)
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(1);
                    listObj.Add(1000);
                    var BasicSalaryServices = new Sal_BasicSalaryServices();
                    var lstBasicSalary = actionService.GetData<Sal_BasicSalaryEntity>(listObj, ConstantSql.hrm_sal_sp_get_BasicPayrollGetAll, ref status).Where(s => s.ProfileID == profile.ID && s.DateOfEffect != null).FirstOrDefault();
                    Hre_ContractEntity contractgetNo = new Hre_ContractEntity();
                    model.CopyData(contractgetNo);
                    Hre_ContractEntity contract = new Hre_ContractEntity
                    {
                        ID = model.ID,
                        Allowance = model.Allowance,
                        Allowance1 = model.Allowance1,
                        Allowance3 = model.Allowance3,
                        Allowance2 = model.Allowance2,
                        Allowance4 = model.Allowance4,
                        AllowanceID1 = lstBasicSalary.AllowanceType1ID,
                        AllowanceID2 = lstBasicSalary.AllowanceType2ID,
                        AllowanceID3 = lstBasicSalary.AllowanceType3ID,
                        AllowanceID4 = lstBasicSalary.AllowanceType4ID,
                        ClassRateID = lstBasicSalary.ClassRateID,
                        Code = model.Code,
                        CodeEmp = model.CodeEmp,
                        //ContractNo = getContractNo(contractgetNo, contractgetNo.DateSigned),
                        ContractTypeID = model.ContractTypeID,
                        CurenncyID = lstBasicSalary.CurrencyID,
                        CurenncyID1 = lstBasicSalary.CurrencyID1,
                        CurenncyID2 = lstBasicSalary.CurrencyID2,
                        CurenncyID3 = lstBasicSalary.CurrencyID3,
                        CurenncyID4 = lstBasicSalary.CurrencyID4,
                        CurenncyID5 = lstBasicSalary.CurrencyID5,
                        CurenncyIDSalary = model.CurenncyIDSalary,
                        CurenncyInsName = model.CurenncyInsName,
                        CurenncyOAllowanceName = model.CurenncyOAllowanceName,
                        CurrencySalName = model.CurrencySalName,
                        DateAuthorize = model.DateAuthorize,
                        DateCreate = model.DateCreate,
                        DateUpdate = DateTime.Now,
                        DateEnd = model.DateEnd,
                        DateSigned = model.DateSigned,
                        DateStart = model.DateStart,
                        FollowNo = model.FollowNo,
                        FormPaySalary = model.FormPaySalary,
                        HourWorkInMonth = model.HourWorkInMonth,
                        InsuranceAmount = model.InsuranceAmount,
                        IPCreate = model.IPCreate,
                        IPUpdate = model.IPUpdate,
                        JobTitleID = model.JobTitleID,
                        PersonalRate = model.PersonalRate,
                        PositionID = model.PositionID,
                        ProfileID = model.ProfileID,
                        ProfileSingID = model.ProfileSingID,
                        ProfileName = model.ProfileName,
                        ProfileSingName = model.ProfileSingName,
                        QualificationID = model.QualificationID,
                        RankRateID = lstBasicSalary.RankRateID,
                        Salary = lstBasicSalary == null ? (double?)null : Convert.ToDouble(lstBasicSalary.GrossAmount),
                        SalaryClassTypeID = lstBasicSalary.ClassRateID,
                        ServerCreate = model.ServerCreate,
                        ServerUpdate = model.ServerUpdate,
                        WorkPlaceID = model.WorkPlaceID,
                    };
                    if (contract.ID == Guid.Empty)
                    {
                        contract.ActionStatus = ContractServices.Add(contract);
                    }
                    else
                    {
                        contract.ActionStatus = ContractServices.Edit(contract);
                    }

                    return Json(contract, JsonRequestBehavior.AllowGet);
                }

                Hre_ContractEntity contractgetNoGetBasicSalary = new Hre_ContractEntity();
                model.CopyData(contractgetNoGetBasicSalary);
                Hre_ContractEntity contractNoGetBasicSalary = new Hre_ContractEntity
                {
                    ID = model.ID,
                    Allowance = model.Allowance,
                    Allowance1 = model.Allowance1,
                    Allowance3 = model.Allowance3,
                    Allowance2 = model.Allowance2,
                    Allowance4 = model.Allowance4,
                    AllowanceID1 = model.AllowanceID1,
                    AllowanceID2 = model.AllowanceID2,
                    AllowanceID3 = model.AllowanceID3,
                    AllowanceID4 = model.AllowanceID4,
                    ClassRateID = model.ClassRateID,
                    ClassRateName = model.ClassRateName,
                    Code = model.Code,
                    CodeEmp = model.CodeEmp,
                    // ContractNo = getContractNo(contractgetNoGetBasicSalary, contractgetNoGetBasicSalary.DateSigned),
                    ContractTypeID = model.ContractTypeID,
                    CurenncyID = model.CurenncyID,
                    CurenncyID1 = model.CurenncyID1,
                    CurenncyID2 = model.CurenncyID2,
                    CurenncyID3 = model.CurenncyID3,
                    CurenncyID4 = model.CurenncyID4,
                    CurenncyID5 = model.CurenncyID5,
                    CurenncyIDSalary = model.CurenncyIDSalary,
                    CurenncyInsName = model.CurenncyInsName,
                    CurenncyOAllowanceName = model.CurenncyOAllowanceName,
                    CurrencySalName = model.CurrencySalName,
                    DateAuthorize = model.DateAuthorize,
                    DateCreate = model.DateCreate,
                    DateEnd = model.DateEnd,
                    DateSigned = model.DateSigned,
                    DateStart = model.DateStart,
                    DateUpdate = DateTime.Now,
                    FollowNo = model.FollowNo,
                    FormPaySalary = model.FormPaySalary,
                    HourWorkInMonth = model.HourWorkInMonth,
                    InsuranceAmount = model.InsuranceAmount,
                    IPCreate = model.IPCreate,
                    IPUpdate = model.IPUpdate,
                    JobTitleID = model.JobTitleID,
                    PersonalRate = model.PersonalRate,
                    PositionID = model.PositionID,
                    ProfileID = model.ProfileID,
                    ProfileSingID = model.ProfileSingID,
                    ProfileName = model.ProfileName,
                    ProfileSingName = model.ProfileSingName,
                    QualificationID = model.QualificationID,
                    RankRateID = model.RankRateID,
                    Salary = model.Salary,
                    SalaryClassTypeID = model.SalaryClassTypeID,
                    ServerCreate = model.ServerCreate,
                    ServerUpdate = model.ServerUpdate,
                    WorkPlaceID = model.WorkPlaceID,
                };

                if (contractNoGetBasicSalary.ID == Guid.Empty)
                {
                    contractNoGetBasicSalary.ActionStatus = ContractServices.Add(contractNoGetBasicSalary);
                }
                else
                {
                    contractNoGetBasicSalary.ActionStatus = ContractServices.Edit(contractNoGetBasicSalary);
                }

                return Json(contractNoGetBasicSalary, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveContractAndSaslary(DateTime? dateStart, DateTime? dateEnd, string orgStructureID, string contractTypeID, string RankDetailForNextContract, List<Guid> selectedIds, string statusContract, bool IsEvaluation)
        {
            var hrService = new Hre_ProfileServices();
            string message = string.Empty;
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;
            var lstObjProfile = new List<object>();
            lstObjProfile.AddRange(new object[16]);
            lstObjProfile[14] = 1;
            lstObjProfile[15] = int.MaxValue - 1;
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();

            var salaryRankServices = new Cat_SalaryRankServices();
            var lstObjSalaryRank = new List<object>();
            lstObjSalaryRank.Add(null);
            lstObjSalaryRank.Add(null);
            lstObjSalaryRank.Add(1);
            lstObjSalaryRank.Add(int.MaxValue - 1);
            var lstSalaryRank = actionService.GetData<Cat_SalaryRankEntity>(lstObjSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();
            var contractServices = new Hre_ContractServices();
            var lisobjContract = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            lisobjContract.Add(strIDs);
            var lstContractByProfileID = actionService.GetData<Hre_ContractEntity>(lisobjContract, ConstantSql.hrm_hr_sp_get_ContractsByListId, ref status).ToList();
            var workhistoryService = new Hre_WorkHistoryServices();
            var lstObjWorkhistory = new List<object>();
            lstObjWorkhistory.AddRange(new object[17]);
            lstObjWorkhistory[15] = 1;
            lstObjWorkhistory[16] = int.MaxValue - 1;
            var lstWorkhistory = actionService.GetData<Hre_WorkHistoryEntity>(lstObjWorkhistory, ConstantSql.hrm_hr_sp_get_WorkHistory, ref status).ToList();

            var basicSalaryService = new Sal_BasicSalaryServices();

            var attGradeService = new Att_GradeServices();
            var lstObjAttGrade = new List<object>();
            lstObjAttGrade.AddRange(new object[6]);
            lstObjAttGrade[4] = 1;
            lstObjAttGrade[5] = int.MaxValue - 1;
            var lstAttGrade = actionService.GetData<Att_GradeEntity>(lstObjAttGrade, ConstantSql.hrm_att_sp_get_Att_Grade, ref status).ToList();

            var gradeService = new Sal_GradeServices();
            var lstObjSalGrade = new List<object>();
            lstObjSalGrade.AddRange(new object[7]);
            lstObjSalGrade[5] = 1;
            lstObjSalGrade[6] = int.MaxValue - 1;
            var lstSalGrade = actionService.GetData<Sal_GradeEntity>(lstObjSalGrade, ConstantSql.hrm_sal_sp_get_Sal_Grade, ref status).ToList();

            var gradePayrollService = new Cat_GradePayrollServices();
            var lstObjGradePayroll = new List<object>();
            lstObjGradePayroll.Add(null);
            lstObjGradePayroll.Add(null);
            lstObjGradePayroll.Add(1);
            lstObjGradePayroll.Add(int.MaxValue - 1);
            var lstGradePayroll = actionService.GetData<Cat_GradePayrollEntity>(lstObjGradePayroll, ConstantSql.hrm_cat_sp_get_GradePayroll, ref status).ToList();

            var gradeAttService = new Cat_GradeAttendanceServices();
            var lstObjGradeAtt = new List<object>();
            lstObjGradeAtt.AddRange(new object[10]);
            lstObjGradeAtt[8] = 1;
            lstObjGradeAtt[9] = int.MaxValue - 1;
            var lstGradeAtt = actionService.GetData<Cat_GradeAttendanceEntity>(lstObjGradeAtt, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendance, ref status).ToList();

            var currencyServices = new Cat_CurrencyServices();
            var lstObjCurrency = new List<object>();
            lstObjCurrency.Add(null);
            lstObjCurrency.Add(null);
            lstObjCurrency.Add(1);
            lstObjCurrency.Add(int.MaxValue - 1);
            var lstCurrency = actionService.GetData<Cat_CurrencyEntity>(lstObjCurrency, ConstantSql.hrm_cat_sp_get_Currency, ref status).ToList();
            var lstCurrencyNew = lstCurrency.Where(s => s.CurrencyName == "VND").FirstOrDefault();

            var contractTypeService = new Cat_ContractTypeServices();
            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            var insuranceConfigServices = new Cat_InsuranceConfigServices();
            var objInsuranceConfig = new List<object>();
            objInsuranceConfig.Add(1);
            objInsuranceConfig.Add(int.MaxValue - 1);
            var lstInsuranceConfig = actionService.GetData<Cat_InsuranceConfigEntity>(objInsuranceConfig, ConstantSql.hrm_cat_sp_get_InsuranceConfig, ref status).ToList();

            var insuranceServices = new Sal_InsuranceSalaryServices();
            var objInsurance = new List<object>();
            objInsurance.AddRange(new object[9]);
            objInsurance[7] = 1;
            objInsurance[8] = int.MaxValue - 1;
            var lstInsurance = actionService.GetData<Sal_InsuranceSalaryEntity>(objInsurance, ConstantSql.hrm_sal_sp_get_InsuranceSalary, ref status).ToList();

            foreach (var item in lstContractByProfileID)
            {
                var contractTypeEntity = new Cat_ContractTypeEntity();
                var objContract = new List<object>();
                objContract.Add(item.ProfileID);
                var lstContractIdByProfileID = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
                var listIdContract = string.Empty;
                if (lstContractIdByProfileID != null)
                {
                    listIdContract = string.Join(",", lstContractIdByProfileID.Select(d => d.ContractTypeID));
                }

                var profile = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();

                if (item.NextContractTypeID != null)
                {
                    contractTypeEntity = lstContractType.Where(s => item.NextContractTypeID.Value == s.ID).FirstOrDefault();
                }
                else
                {
                    message = ConstantMessages.WarningContractHaveNotNextContract.ToString().TranslateString();
                    return Json(message, JsonRequestBehavior.AllowGet);
                    // contractTypeEntity = lstContractType.Where(s => Guid.Parse(Common.DotNetToOracle(item.ContractNextID.ToString())) == s.ID).FirstOrDefault();
                }


                var workingHistoryEntity = lstWorkhistory.Where(s => s.ProfileID == item.ProfileID).FirstOrDefault();
                var objSalGrade = new List<object>();
                objSalGrade.Add(item.ProfileID);
                objSalGrade.Add(null);
                objSalGrade.Add(1);
                objSalGrade.Add(int.MaxValue - 1);
                var salGradeByProfileIDEntity = actionService.GetData<Sal_GradeEntity>(objSalGrade, ConstantSql.hrm_sal_sp_get_GradeAndAllownaceByProId, ref status).FirstOrDefault();
                var objAttGrade = new List<object>();
                objAttGrade.Add(item.ProfileID);
                objAttGrade.Add(null);
                objAttGrade.Add(1);
                objAttGrade.Add(int.MaxValue - 1);
                var attGradeByProfileIDEntity = actionService.GetData<Att_GradeEntity>(objAttGrade, ConstantSql.hrm_att_sp_get_GradeAttendanceByProIdCutID, ref status).FirstOrDefault();

                if (contractTypeEntity == null)
                {
                    continue;
                }

                if (item.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
                {
                    if (item.ContractEvaType == EnumDropDown.ContractEvaType.E_EXPIRED_APPRENTICE.ToString())
                    {

                        //chưa tìm dc cách xử lý nên hard code 
                        var lstSalaryRankNew = new Cat_SalaryRankEntity();
                        // var lstSalaryRankNew = lstSalaryRank.Where(s => item.RankDetailForNextContract != null && s.ID == item.RankDetailForNextContract.Value).FirstOrDefault();
                        if (item.RankDetailForNextContract != null)
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => item.RankDetailForNextContract != null && s.ID == item.RankDetailForNextContract.Value).FirstOrDefault();
                        }
                        else
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => item.RankRateID != null && s.ID == item.RankRateID.Value).FirstOrDefault();
                        }

                        #region Xử lý Hre_Contract
                        if (item.TypeOfPass == EnumDropDown.TypeOfPass.E_SIGNED_NEXTCONTRACT.ToString())
                        {
                            int month = 0;
                            if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                            {
                                month = (int)contractTypeEntity.ValueTime.Value;
                                if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                                {
                                    month = month * 12;
                                }
                                contractTypeEntity.DateStart = item.DateEnd.Value.AddDays(1);

                                //chưa tìm dc cách xử lý nên hard code 
                                //  var nextContractTypeID = Common.ConvertToGuid(contractTypeEntity.ContractNextID).ToString();
                                var contractEntity = new Hre_ContractEntity
                                {
                                    //   ContractNo = getContractNo(item, item.DateSigned),
                                    ProfileID = item.ProfileID,
                                    ProfileName = item.ProfileName,
                                    DateStart = item.DateEnd.Value.AddDays(1),
                                    DateSigned = item.DateEnd.Value.AddDays(1),
                                    JobTitleID = item.JobTitleID,
                                    PositionID = item.PositionID,
                                    DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                    Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                    RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                    ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                    SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                    ContractTypeID = contractTypeEntity.ID
                                };

                                if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                                {
                                    contractEntity = SetNewDateEndContract(contractEntity);
                                }

                                if (item.DateEndNextContract != null)
                                {
                                    contractEntity.DateEnd = item.DateEndNextContract.Value;
                                }
                                if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                                {
                                    return Json(contractEntity, JsonRequestBehavior.AllowGet);
                                }
                                message = contractServices.Add(contractEntity);
                            }
                            else
                            {
                                contractTypeEntity.DateStart = item.DateEnd.Value.AddDays(1);

                                //chưa tìm dc cách xử lý nên hard code 
                                //  var nextContractTypeID = Common.ConvertToGuid(contractTypeEntity.ContractNextID).ToString();
                                var contractEntity = new Hre_ContractEntity
                                {
                                    //  ContractNo = getContractNo(item, item.DateSigned),
                                    ProfileID = item.ProfileID,
                                    ProfileName = item.ProfileName,
                                    DateStart = item.DateEnd.Value.AddDays(1),
                                    DateSigned = item.DateEnd.Value.AddDays(1),
                                    JobTitleID = item.JobTitleID,
                                    PositionID = item.PositionID,
                                    //   DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                    Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                    RankRateID = lstSalaryRankNew == null ? item.RankRateID : lstSalaryRankNew.ID,
                                    ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                    SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                    ContractTypeID = contractTypeEntity.ID
                                };

                                if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                                {
                                    contractEntity = SetNewDateEndContract(contractEntity);
                                }
                                if (item.DateEndNextContract != null)
                                {
                                    contractEntity.DateEnd = item.DateEndNextContract.Value;
                                }
                                if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                                {
                                    return Json(contractEntity, JsonRequestBehavior.AllowGet);
                                }
                                message = contractServices.Add(contractEntity);
                            }
                        }

                        #endregion

                        #region Xử Lý Sal_BasicSalary
                        var salaryEntity = new Sal_BasicSalaryEntity
                        {
                            ProfileID = item.ProfileID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                            DateOfEffect = item.DateEnd.Value.AddDays(1),
                            CurrencyID = lstCurrencyNew.ID,
                            Note = item.Remark


                        };
                        message = basicSalaryService.Add(salaryEntity);

                        #endregion

                        #region Xử Lý Hre_Profile
                        var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                        profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;

                        hrService.Edit(profileEntity);

                        if (workingHistoryEntity != null)
                        {
                            if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                            {
                                var workhistoryEntity = new Hre_WorkHistoryEntity
                                {
                                    ProfileID = item.ProfileID,
                                    SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    DateEffective = item.DateEnd.Value.AddDays(1)
                                };
                                message = workhistoryService.Add(workhistoryEntity);
                            }

                        }
                        else
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = item.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = item.DateEnd.Value.AddDays(1)
                            };
                            message = workhistoryService.Add(workhistoryEntity);
                        }

                        #endregion

                        #region Sal_Grade
                        var lstGradeByProfileID = lstSalGrade.Where(s => item.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (salGradeByProfileIDEntity != null)
                        {
                            if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                            {
                                var gradeEntity = new Sal_GradeEntity
                                {
                                    //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                    ProfileID = item.ProfileID,
                                    GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                    MonthStart = item.DateEnd.Value.AddDays(1)
                                };
                                message = gradeService.Add(gradeEntity);
                            }

                        }
                        else
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = item.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = item.DateEnd.Value.AddDays(1)
                            };
                            message = gradeService.Add(gradeEntity);
                        }

                        #endregion

                        #region Att_Grade
                        var lstAttGradeByProfileID = lstAttGrade.Where(s => item.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (attGradeByProfileIDEntity != null)
                        {
                            if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                            {
                                var gradeAttEntity = new Att_GradeEntity
                                {
                                    // ID = lstGradeAttByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                    ProfileID = item.ProfileID,
                                    GradeAttendanceID = lstAttGradeByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                    MonthStart = item.DateEnd.Value.AddDays(1)
                                };
                                message = attGradeService.Add(gradeAttEntity);
                            }
                        }
                        else
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                // ID = lstGradeAttByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = item.ProfileID,
                                GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = item.DateEnd.Value.AddDays(1)
                            };
                            message = attGradeService.Add(gradeAttEntity);
                        }


                        #endregion

                        #region Xử Lý Lương BHXH
                        if (contractTypeEntity.NoneTypeInsuarance == true)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == item.ProfileID && s.DateEffect == item.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = item.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = item.DateEnd.Value.AddDays(1),
                                IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                                IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                                IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                                CurrencyID = lstCurrencyNew.ID
                            };
                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                                insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                                insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                                message = insuranceServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = insuranceServices.Add(insuranceEntity);
                            }


                        }

                        if (contractTypeEntity.NoneTypeInsuarance == false)
                        {

                            var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            if (insuranceConfigEntity != null)
                            {
                                var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == item.ProfileID && s.DateEffect == item.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                var insuranceEntity = new Sal_InsuranceSalaryEntity
                                {
                                    ProfileID = item.ProfileID,
                                    InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                    DateEffect = item.DateEnd.Value.AddDays(1),
                                    IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                    IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                    IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                    CurrencyID = lstCurrencyNew.ID
                                };

                                if (insuranceEntityByProfileID != null)
                                {
                                    insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                    insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                    insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                    insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                    message = insuranceServices.Edit(insuranceEntityByProfileID);
                                }
                                else
                                {
                                    message = insuranceServices.Add(insuranceEntity);
                                }

                            }
                        }
                        #endregion

                        //  return Json(message, JsonRequestBehavior.AllowGet);
                    }

                    if (item.ContractEvaType == EnumDropDown.ContractEvaType.E_ANNUAL_EVALUATION.ToString() && item.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
                    {

                        var lstSalaryRankNew = new Cat_SalaryRankEntity();
                        // var lstSalaryRankNew = lstSalaryRank.Where(s => item.RankDetailForNextContract != null && s.ID == item.RankDetailForNextContract.Value).FirstOrDefault();
                        if (item.RankDetailForNextContract != null)
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => item.RankDetailForNextContract != null && s.ID == item.RankDetailForNextContract.Value).FirstOrDefault();
                        }
                        else
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => item.RankRateID != null && s.ID == item.RankRateID.Value).FirstOrDefault();
                        }

                        #region Xử lý Hre_Contract
                        int month = 0;
                        if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                        {
                            month = (int)contractTypeEntity.ValueTime.Value;
                            if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                            {
                                month = month * 12;
                            }
                            contractTypeEntity.DateStart = item.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 

                            var contractEntity = new Hre_ContractEntity
                            {
                                // ContractNo = getContractNo(item, item.DateSigned),
                                ProfileID = item.ProfileID,
                                ProfileName = item.ProfileName,
                                DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                                DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                                JobTitleID = item.JobTitleID,
                                PositionID = item.PositionID,
                                DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID
                            };

                            if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                            {
                                contractEntity = SetNewDateEndContract(contractEntity);
                            }
                            if (item.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = item.DateEndNextContract.Value;
                            }
                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {
                                return Json(contractEntity, JsonRequestBehavior.AllowGet);
                            }
                            message = contractServices.Add(contractEntity);
                        }
                        else
                        {
                            contractTypeEntity.DateStart = item.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 

                            var contractEntity = new Hre_ContractEntity
                            {
                                // ContractNo = getContractNo(item, item.DateSigned),
                                ProfileID = item.ProfileID,
                                ProfileName = item.ProfileName,
                                DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                                DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                                JobTitleID = item.JobTitleID,
                                PositionID = item.PositionID,
                                // DateEnd = null,
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID
                            };

                            if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                            {
                                contractEntity = SetNewDateEndContract(contractEntity);
                            }

                            if (item.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = item.DateEndNextContract.Value;
                            }
                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {

                                return Json(contractEntity, JsonRequestBehavior.AllowGet);
                            }
                            message = contractServices.Add(contractEntity);
                        }
                        #endregion

                        #region Xử Lý Sal_BasicSalary
                        var salaryEntity = new Sal_BasicSalaryEntity
                        {
                            ProfileID = item.ProfileID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                            DateOfEffect = new DateTime(DateTime.Now.Year, 6, 1),
                            CurrencyID = lstCurrencyNew.ID,
                            Note = item.Remark

                        };
                        message = basicSalaryService.Add(salaryEntity);

                        #endregion

                        #region Xử Lý Hre_Profile
                        var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                        profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;
                        message = hrService.Edit(profileEntity);

                        if (workingHistoryEntity != null)
                        {
                            if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                            {
                                var workhistoryEntity = new Hre_WorkHistoryEntity
                                {
                                    ProfileID = item.ProfileID,
                                    SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    DateEffective = new DateTime(DateTime.Now.Year, 6, 1)
                                };
                                message = workhistoryService.Add(workhistoryEntity);
                            }

                        }
                        else
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = item.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = workhistoryService.Add(workhistoryEntity);
                        }
                        #endregion

                        #region Sal_Grade
                        var lstGradeByProfileID = lstSalGrade.Where(s => item.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (salGradeByProfileIDEntity != null)
                        {
                            if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                            {
                                var gradeEntity = new Sal_GradeEntity
                                {
                                    //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                    ProfileID = item.ProfileID,
                                    GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                    MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                                };
                                message = gradeService.Add(gradeEntity);
                            }
                        }
                        else
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //  ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = item.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = gradeService.Add(gradeEntity);
                        }


                        #endregion

                        #region Att_Grade
                        var lstAttGradeByProfileID = lstAttGrade.Where(s => item.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (attGradeByProfileIDEntity != null)
                        {
                            if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                            {
                                var gradeAttEntity = new Att_GradeEntity
                                {
                                    //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                    ProfileID = item.ProfileID,
                                    GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                    MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                                };
                                message = attGradeService.Add(gradeAttEntity);
                            }
                        }
                        else
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = item.ProfileID,
                                GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = attGradeService.Add(gradeAttEntity);
                        }


                        #endregion

                        #region Xử Lý Lương BHXH
                        if (contractTypeEntity.NoneTypeInsuarance == true)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == item.ProfileID && s.DateEffect == item.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = item.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = item.DateEnd.Value.AddDays(1),
                                IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                                IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                                IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                                CurrencyID = lstCurrencyNew.ID
                            };

                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                                insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                                insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                                message = insuranceServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = insuranceServices.Add(insuranceEntity);
                            }
                        }
                        if (contractTypeEntity.NoneTypeInsuarance == false)
                        {
                            var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            if (insuranceConfigEntity != null)
                            {
                                var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == item.ProfileID && s.DateEffect == item.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                var insuranceEntity = new Sal_InsuranceSalaryEntity
                                {
                                    ProfileID = item.ProfileID,
                                    InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                    DateEffect = item.DateEnd.Value.AddDays(1),
                                    IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                    IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                    IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                    CurrencyID = lstCurrencyNew.ID
                                };
                                if (insuranceEntityByProfileID != null)
                                {
                                    insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                    insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                    insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                    insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                    message = insuranceServices.Edit(insuranceEntityByProfileID);
                                }
                                else
                                {
                                    message = insuranceServices.Add(insuranceEntity);
                                }
                            }
                        }
                        #endregion

                    }
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
            //   return null;
        }

        #endregion

        #endregion

        #region PurchaseRequest
        public ActionResult GetDataByCostCentreID([DataSourceRequest] DataSourceRequest request, Guid costcentreID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            var entity = service.GetByIdUseStore<Cat_CateCodeModel>(costcentreID, ConstantSql.hrm_cat_sp_get_CateCodeByIds, ref status);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataByContractTypeID([DataSourceRequest] DataSourceRequest request, Guid contractTypeID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            var entity = service.GetByIdUseStore<CatContractTypeModel>(contractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDataByItemID([DataSourceRequest] DataSourceRequest request, Guid itemID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            var entity = service.GetByIdUseStore<Cat_PurchaseItemsModel>(itemID, ConstantSql.hrm_cat_sp_get_PurchaseItemsById, ref status);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPurchaseRequestItemByPurchaseRequestID([DataSourceRequest] DataSourceRequest request, Guid PurchaseRequestID)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(PurchaseRequestID);
            var result = actionService.GetData<Fin_PurchaseRequestItemModel>(objs, ConstantSql.hrm_cat_sp_get_PRItemByPRID, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region Hre_AppendixContract
        public ActionResult GetAppendixContractByContractList([DataSourceRequest] DataSourceRequest request, Guid? ContractID)
        {
            if (ContractID != null)
            {
                string status = string.Empty;
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(ContractID);
                var result = actionService.GetData<Hre_AppendixContractEntity>(objs, ConstantSql.hrm_cat_sp_get_AppendixContractByContractID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }

        public JsonResult GetMultiAppendixContractType(string text)
        {
            return GetDataForControl<HreAppendixContractTypeMultiModel, Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_EmployeeType_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }
        public JsonResult GetMultiRelatives(string text)
        {
            return GetDataForControl<Hre_RelativesMultiModel, Hre_RelativesMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Relatives_Multi);

            //return GetDataForControl<HreAppendixContractTypeMultiModel, hre Cat_ContractTypeMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AppendixContractType_multi);
        }


        /// <summary>
        /// [Son.Vo] - Lấy danh sách dữ liệu cho AppendixContract (Hre_AppendixContract) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAppendixContractList([DataSourceRequest] DataSourceRequest request, Hre_AppendixContractSearchModel model)
        {
            return GetListDataAndReturn<Hre_AppendixContractModel, Hre_AppendixContractEntity, Hre_AppendixContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_AppendixContract);
        }

        [HttpPost]
        public ActionResult GetAppendixExpiredContractList([DataSourceRequest] DataSourceRequest request, Hre_AppendixContractSearchModel model)
        {
            return GetListDataAndReturn<Hre_AppendixContractModel, Hre_AppendixContractEntity, Hre_AppendixContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_AppendixExpiredContract);
        }

        /// <summary>
        /// [Son.Vo] - Xuất danh sách dữ liệu cho AppendixContract (Hre_AppendixContract) theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAppendixContractList([DataSourceRequest] DataSourceRequest request, Hre_AppendixContractSearchModel model)
        {
            return ExportAllAndReturn<Hre_AppendixContractEntity, Hre_AppendixContractModel, Hre_AppendixContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_AppendixContract);
        }

        /// <summary>
        /// [Son.Vo] - Xuất các dòng dữ liệu được chọn của AppendixContract (Hre_AppendixContract) ra file Excel
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <returns></returns>
        public ActionResult ExportAppendixContractSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_AppendixContractEntity, Hre_AppendixContractModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_AppendixContractByIds);
        }


        #endregion

        #region General

        /// <summary>
        /// [Hien.Pham] - Hàm mở rộng chuyển các tham số điều kiện tìm kiếm thành đối tượng param
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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


        public ActionResult GetHighSupervisor(string ProfileID)
        {
            if (ProfileID == null || ProfileID == string.Empty)
            {
                return Json("");
            }
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            List<Hre_SupervisorEntity> ListSupervisor = new List<Hre_SupervisorEntity>();
            List<object> lstModel = new List<object>();
            lstModel.Add(Common.DotNetToOracle(ProfileID));
            ListSupervisor = actionService.GetData<Hre_SupervisorEntity>(lstModel, ConstantSql.hrm_hre_getdata_Supervisor, ref status).ToList();
            Hre_SupervisorEntity result = ListSupervisor.FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm lấy mã chuỗi phòng ban cha
        /// </summary>
        /// <param name="ListOrg">List Phòng Ban</param>
        /// <param name="CurrentID">ID Phòng ban hiện tại</param>
        /// <returns></returns>
        public string GetCodeOrgStructure(List<Cat_OrgStructureEntity> ListOrg, Guid CurrentID)
        {

            string result = string.Empty;
            while (true)
            {
                var model = ListOrg.Single(m => m.ID == CurrentID);
                result = result.Insert(0, model.Code + "/");
                CurrentID = (Guid)model.ParentID;

                if (model.ParentID == null)
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// [Hien.Nguyen] Hàm lấy mã chuỗi phòng ban cha
        /// </summary>
        /// <param name="CurrentID">ID Phòng ban hiện tại</param>
        /// <returns></returns>
        public string GetCodeOrgStructure(Guid CurrentID)
        {
            if (CurrentID == Guid.Empty)
            {
                return "";
            }
            string result = string.Empty;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            List<Cat_OrgStructureEntity> ListOrg = new List<Cat_OrgStructureEntity>();
            List<object> lstModel = new List<object>();
            lstModel.AddRange(new object[5]);
            lstModel[3] = 1;
            lstModel[4] = Int32.MaxValue - 1;
            ListOrg = actionService.GetData<Cat_OrgStructureEntity>(lstModel, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            while (true)
            {
                var model = ListOrg.Where(m => m.ID == CurrentID).FirstOrDefault();
                if (model == null)
                {
                    return "";
                }
                result = result.Insert(0, model.Code + "/");
                if (model.ParentID == null)
                {
                    return result.Substring(0, result.Length - 1);
                }
                else
                {
                    CurrentID = (Guid)model.ParentID;
                }
            }
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm lấy mã chuỗi phòng ban cha
        /// </summary>
        /// <param name="CurrentID">ID Phòng ban hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCodeOrgStructureForID(Guid id)
        {
            return Json(GetCodeOrgStructure(id));
        }

        public JsonResult GetProfileOrd(string text)
        {
            if (text == null || text == " ")
                text = string.Empty;
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;
            var listEntity = actionService.GetData<Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, ref status);
            if (listEntity != null)
            {
                List<Hre_ProfileMultiModel> listModel = listEntity.Translate<Hre_ProfileMultiModel>();
                listModel = listModel.OrderBy(s => s.ProfileName).ToList();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiProfile(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi);
        }

        public JsonResult GetMultiProfileQuit(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_ProfileQuit_multi);
        }

        [HttpPost]
        public JsonResult GetMultiProfileOrQuit()
        {
            string isProfileQuit = "false";
            if (string.IsNullOrEmpty(isProfileQuit) || isProfileQuit == "false")
            {
                return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(null, ConstantSql.hrm_hr_sp_get_Profile_multi);    
            }
            else
            {
                return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(null, ConstantSql.hrm_hr_sp_get_ProfileQuit_multi);
            }            
        }


        public JsonResult GetProfileByCodeEmps(string codeemps)
        {
            if (codeemps != "")
                codeemps = codeemps.Replace(" ", "");
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var result = actionService.GetData<Hre_ProfileMultiEntity>(codeemps, ConstantSql.hrm_hr_sp_get_ProfileByCodeEmps, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreDocumentByCode(string codeDocument)
        {
            if (codeDocument != "")
            {
                codeDocument = codeDocument.Replace(" ", "");
            }
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var result = actionService.GetData<Cat_ReqDocumentEntity>(codeDocument, ConstantSql.hrm_cat_sp_get_StoreDocumentByCode, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiProfileSuspense(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_ProfileSuspense_multi);
        }

        public ActionResult GetProfileNameByProfileID(List<Guid> profileIds)
        {
            string status = string.Empty;
            List<Hre_ProfileModel> result = new List<Hre_ProfileModel>();
            var actionService = new ActionService(UserLogin);
            if (profileIds != null)
            {
                #region MyRegion
                //if (selectedIds.Count >= 0)
                //{
                //    int _total = selectedIds.Count;
                //    int _totalPage = _total / 5 + 1;
                //    int _pageSize = 5;
                //    var dataReturn = new object();
                //    for (int _page = 1; _page <= _totalPage; _page++)
                //    {
                //        int _skip = _pageSize * (_page - 1);
                //        var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                //        var _strDelIDs = string.Join(",", _listCurrenPage);
                //        var service = new RestServiceClient<TModel>();
                //        service.SetCookies(this.Request.Cookies, urlService);
                //        dataReturn = service.Delete(urlService, api, deleteType + "," + _strDelIDs);
                //    }
                //    return Json(dataReturn, JsonRequestBehavior.AllowGet);

                //}
                //return Json(null); 
                #endregion

                int _total = profileIds.Count;
                int _totalPage = _total / 5 + 1;
                int _pageSize = 5;
                var dataReturn = new object();
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = profileIds.Skip(_skip).Take(_pageSize).ToList();
                    var _strDelIDs = string.Join(",", _listCurrenPage);
                    result.AddRange(actionService.GetData<Hre_ProfileModel>(Common.DotNetToOracle(_strDelIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status));
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllMultiProfile(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_AllProfile_multi);
        }

        public JsonResult GetProfileNotGradeMulti(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_ProfileNotGrade_multi);
        }

        #region att_profilenotgrade
        public JsonResult GetProfileNotGradeMultids(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(Common.DotNetToOracle(text), ConstantSql.hrm_hr_sp_get_ProfileNotGrade_multids);

        }
        public ActionResult ExportAllProfileNotGrade([DataSourceRequest] DataSourceRequest request, Hre_ProfilenotGradeSearchModel model)
        {
            model.CodeEmp = "";
            model.ProfileName = "";
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfilenotGradeSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileNotGrade);
        }
        public ActionResult Get_Att_ProfileNotGrade([DataSourceRequest] DataSourceRequest request, Hre_ProfileModel model)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            objs.AddRange(new object[4]);
            objs[0] = model.ProfileName;
            objs[1] = model.CodeEmp;
            objs[2] = 1;
            objs[3] = Int32.MaxValue - 1;
            var result = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileNotGrade, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        #endregion att_profilenotgrade

        #region sal_profilenotgrade

        public JsonResult Get_SAL_ProfileNotGradeMultids(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(Common.DotNetToOracle(text), ConstantSql.hrm_hr_sp_get_PFNGMUlTIds);
        }
        public ActionResult Get_Sal_ProfileNotGrade([DataSourceRequest] DataSourceRequest request, Hre_ProfileModel model)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            objs.AddRange(new object[4]);
            objs[0] = model.ProfileName;
            objs[1] = model.CodeEmp;
            objs[2] = 1;
            objs[3] = Int32.MaxValue - 1;
            var result = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_PFNG_sal, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ExportAllSalProfileNotGrade([DataSourceRequest] DataSourceRequest request, Hre_ProfilenotGradeSearchModel model)
        {
            model.CodeEmp = "";
            model.ProfileName = "";
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfilenotGradeSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_PFNG_sal);
        }

        #endregion

        public ActionResult GetCodeAlocalByProfileID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            objs.Add(1);
            objs.Add(10000);

            var result = actionService.GetData<Sal_CodeAlocalEntity>(objs, ConstantSql.hrm_hr_sp_get_CodeAlocalByProfileId, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult GetSalCostCentreSalByProfileID([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = actionService.GetData<Sal_CostCentreSalEntity>(objs, ConstantSql.hrm_hr_sp_get_Sal_CostCentreSalByProfileId, ref status);
            if (result != null)
            {
                return Json(result.ToDataSourceResult(request));
            }
            else
            {
                return Json(null);
            }


        }
        //kiem tra he so 1nv khong lon hon 1
        public ActionResult CheckRateSalCostCentreSalByProfileID(Att_ProIDAndCutIDModel model)
        {

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(1);
            objs.Add(10000);
            var result = actionService.GetData<Sal_CostCentreSalEntity>(objs, ConstantSql.hrm_hr_sp_get_Sal_CostCentreSalByProfileId, ref status);
            double? rate = result.Sum(s => s.Rate);
            var ls = new object[] { rate };
            return Json(ls);
            //  return Json(result.ToDataSourceResult(request));
        }


        public string CheckDuplicateCodeEmp(Guid? ID, string codeEmp)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateCodeEmp();

            if (ischeck == false)
            {
                return null;
            }

            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string profile = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(ID);
            objs.Add(codeEmp);
            objs.Add(1);
            objs.Add(10000);

            var profilename = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByCodeEmp, ref status);
            if (profilename != null)
            {
                var profilebyid = profilename.FirstOrDefault();
                if (profilebyid != null)
                {
                    profile = profilebyid.ProfileName;
                }
            }

            return profile;
        }

        public string CheckDuplicateCodeAtt(Guid? ID, string codeAtt)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateCodeAtt();

            if (ischeck == false)
            {
                return null;
            }
            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string profile = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(ID);
            objs.Add(codeAtt);
            var profilename = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByCodeAttID, ref status).FirstOrDefault();
            if (profilename != null)
            {
                profile = profilename.ProfileName;
            }

            return profile;
        }

        public string CheckDuplicateRelatives(Guid? ID, string name)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateRelatives();

            if (ischeck == false)
            {
                return null;
            }

            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string result = null;
            bool isduplicate = false;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objsRelative = new List<object>();
            objsRelative.Add(ID);
            objsRelative.Add(name);
            var lstRelativeName = actionService.GetData<Hre_RelativesEntity>(objsRelative, ConstantSql.hrm_hr_sp_get_Relativebyname, ref status).Select(s => new { s.RelativeName }).ToList();
            if (lstRelativeName != null && lstRelativeName.Count > 0)
            {
                foreach (var item in lstRelativeName)
                {
                    result += item.RelativeName + ", ";
                }
                result = result.Substring(0, result.Length - 2);
            }
            var objsProfile = new List<object>();
            objsProfile.Add(name);
            var lstProfileName = actionService.GetData<Hre_ProfileEntity>(objsProfile, ConstantSql.hrm_hr_sp_get_profilebyname, ref status).Select(s => new { s.CodeEmp, s.ProfileName }).ToList();

            if (lstProfileName != null && lstProfileName.Count > 0)
            {
                foreach (var item in lstProfileName)
                {
                    if (item.CodeEmp == null)
                    {
                        result += item.ProfileName + ", ";
                    }
                    result += item.ProfileName + " - " + item.CodeEmp + ", ";
                }
                result = result.Substring(0, result.Length - 2);
            }

            return result;
        }

        public string CheckDuplicateProfileName(Guid? ID, string profileName)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateProfileName(UserLogin);

            if (ischeck == false)
            {
                return null;
            }

            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string profile = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(ID);
            objs.Add(profileName);
            objs.Add(1);
            objs.Add(10000000);
            var profilename = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByProfileName, ref status).Select(s => new { s.ProfileName, s.CodeEmp }).ToList();
            var relativeNameByprofilename = actionService.GetData<Hre_RelativesEntity>(objs, ConstantSql.hrm_hr_sp_get_RelativeByProfileName, ref status).Select(s => new { s.CodeEmp, s.RelativeName }).ToList();
            if (profilename != null && profilename.Count >= 1)
            {
                foreach (var item in profilename)
                {
                    //if (item.CodeEmp == null)
                    //{
                    //    profile += item.ProfileName + ", ";
                    //}
                    profile += item.ProfileName + " - " + item.CodeEmp + ", ";
                }


            }

            if (relativeNameByprofilename != null && relativeNameByprofilename.Count >= 1)
            {

                string codeemp = "";
                foreach (var item in relativeNameByprofilename)
                {
                    //if (item.CodeEmp == null )
                    //{
                    //    profile += item.RelativeName + ", ";
                    //}
                    if (item.CodeEmp != codeemp)
                        profile += item.RelativeName + "(" + item.CodeEmp + "), ";
                    if (item.CodeEmp != null)
                        codeemp = item.CodeEmp.ToString();
                }

            }
            if (profile != null)
                profile = profile.Substring(0, profile.Length - 2);
            return profile;
        }

        public string CheckDuplicateProfileNameAndBirthDay(Guid? ID, string profileName, string day, string month, string year)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateProfileNameAndBirthDay();
            if (ischeck == false)
            {
                return null;
            }

            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string profile = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(ID);
            objs.Add(profileName);
            objs.Add(day);
            objs.Add(month);
            objs.Add(year);
            objs.Add(1);
            objs.Add(10000);
            var profilename = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByProfileNameAndBirthDay, ref status);

            if (profilename != null && profilename.Count > 0)
            {
                foreach (var item in profilename)
                {
                    profile = item.ProfileName + " - " + item.CodeEmp + ", ";
                }

                profile = profile.Substring(0, profile.Length - 2);
            }
            return profile;
        }

        public string CheckDuplicateIDNo(Guid? ID, string idNo)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            var hre_profileservices = new Hre_ProfileServices();
            Boolean ischeck = hre_profileservices.IsCheckDuplidateIDNo();
            if (ischeck == false)
            {
                return null;
            }

            if (ID == null)
            {
                ID = Guid.Empty;
            }
            string profile = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(ID);
            objs.Add(idNo);
            objs.Add(1);
            objs.Add(10000);
            var profileByIdNo = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileByIDNo, ref status).FirstOrDefault();
            if (profileByIdNo != null && profileByIdNo.IsBlackList == true)
            {
                profile = "Black";
            }
            else if (profileByIdNo != null && (profileByIdNo.IsBlackList == null || profileByIdNo.IsBlackList == false))
            {
                profile = profileByIdNo.ProfileName;
            }

            return profile;
        }

        //[HttpPost]
        /// <summary>
        /// Cập nhật Status và Reasondeny khi chọn nhiều nhân viên trên lưới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateReasonDenny([Bind]Hre_ProfileModel model)
        {
            IList<Hre_ProfileEntity> list = new List<Hre_ProfileEntity>();
            if (!string.IsNullOrEmpty(model.listId))
            {
                List<Guid> lisIDs = model.listId.Split(',').Select(x => Guid.Parse(x)).ToList();
                Hre_ProfileEntity ObjProfile = null;
                var ProfileService = new Hre_ProfileServices();
                var actionService = new ActionService(UserLogin);
                foreach (Guid item in lisIDs)
                {
                    string status = string.Empty;
                    ObjProfile = new Hre_ProfileEntity();
                    var ResultProfile = actionService.GetData<Hre_ProfileEntity>(item, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
                    ObjProfile = ResultProfile;
                    ObjProfile.StatusHire = HRM.Infrastructure.Utilities.ProfileStatusSyn.E_UNHIRE.ToString();
                    ObjProfile.ReasonDeny = model.ReasonDeny;
                    ObjProfile.StatusSyn = HRM.Infrastructure.Utilities.ProfileStatusSyn.E_UNHIRE.ToString();
                    ProfileService.Edit(ObjProfile);
                    list.Add(ObjProfile);
                }
            }
            return Json(list);
        }
        /// <summary>
        /// Cập nhật phòng ban cho nhân viên
        /// </summary>
        /// <param name="SalaryClassName"></param>
        /// <param name="ProfileIDs"></param>
        /// <param name="DateEndProbation"></param>
        /// <param name="DateHire"></param>
        /// <returns></returns>
        public ActionResult UpdateOrgProfile([Bind]Hre_ProfileModel model)
        {
            IList<Hre_ProfileEntity> list = new List<Hre_ProfileEntity>();
            if (!string.IsNullOrEmpty(model.listId))
            {
                List<Guid> lisIDs = model.listId.Split(',').Select(x => Guid.Parse(x)).ToList();
                Hre_ProfileEntity ObjProfile = null;
                var actionService = new ActionService(UserLogin);
                var ProfileService = new Hre_ProfileServices();
                foreach (Guid item in lisIDs)
                {
                    string status = string.Empty;
                    ObjProfile = new Hre_ProfileEntity();
                    var ResultProfile = actionService.GetData<Hre_ProfileEntity>(item, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
                    ObjProfile = ResultProfile;
                    ObjProfile.OrgStructureID = model.OrgStructureID;
                    ProfileService.Edit(ObjProfile);
                    list.Add(ObjProfile);
                }
            }
            return Json(list);
        }

        [HttpPost]
        public ActionResult ValidateUpdateFileCandidate(Hre_ProfileTemp model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfileTemp>(model, "Hre_ProfileTemp", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
            }
            else
            {
                model.ActionStatus = "Success";

            }
            return Json(model, JsonRequestBehavior.AllowGet);
            #endregion
        }
        [HttpPost]
        public ActionResult ValidateUpdateBasicSalary(Hre_ProfileTemp model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfileTemp>(model, "Hre_ProfileTempBasicSalary", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
            }
            else
            {
                model.ActionStatus = "Success";

            }
            return Json(model, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpPost]
        public ActionResult ActionApplyCandidateGeneral(string selectedIds)
        {
            string message = NotificationType.Success.ToString();
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;
            var lstCandidateGeneral = actionService.GetData<Hre_CandidateGeneralEntity>(Common.DotNetToOracle(selectedIds), ConstantSql.hrm_hr_sp_get_GeneralCandidateByIds, ref status);
            var salaryRankServices = new Cat_SalaryRankServices();
            var salaryClassServices = new Cat_SalaryClassServices();
            var profileServices = new Hre_ProfileServices();
            var contracttypeServices = new Cat_ContractTypeServices();
            var workHistoryServices = new Hre_WorkHistoryServices();
            var rankEntity = new Cat_SalaryRankEntity();
            var salaryClass = new Cat_SalaryClassEntity();
            var contractypeentity = new Cat_ContractTypeEntity();
            ActionService service = new ActionService(UserLogin);
            var salgradeServices = new Sal_GradeServices();
            var attgradeservive = new Att_GradeServices();

            foreach (var CandidateGeneral in lstCandidateGeneral)
            {
                #region Code Cũ
                //var datecontract = DateTime.Now;
                //if (CandidateGeneral.EnteringDate != null)
                //{
                //    datecontract = CandidateGeneral.EnteringDate.Value;
                //}
                //if (CandidateGeneral.RankRateID != Guid.Empty)
                //{
                //    rankEntity = service.GetByIdUseStore<Cat_SalaryRankEntity>(CandidateGeneral.RankRateID.Value,
                //                        ConstantSql.hrm_cat_sp_get_SalaryRankById, ref status);
                //}
                //if(CandidateGeneral.ContractTypeID != Guid.Empty)
                //{
                //    contractypeentity = service.GetByIdUseStore<Cat_ContractTypeEntity>(CandidateGeneral.ContractTypeID.Value, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
                //}

                //var profileEntity = profileServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(CandidateGeneral.ProfileID.ToString()),
                //        ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

                //if (contractypeentity != null && contractypeentity.ValueTime != null)
                //{
                //    if (contractypeentity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                //    {
                //        datecontract = datecontract.AddMonths(int.Parse(contractypeentity.ValueTime.Value.ToString()));
                //    }
                //    else if (contractypeentity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                //    {
                //        datecontract = datecontract.AddYears(int.Parse(contractypeentity.ValueTime.Value.ToString()));
                //    }
                //}

                //if(profileEntity != null)
                //{
                //    profileEntity.OrgStructureID = CandidateGeneral.OrgStructureID;
                //    profileEntity.SalaryClassID = CandidateGeneral.SalaryClassID;
                //    profileEntity.DateHire = CandidateGeneral.EnteringDate;
                //    profileEntity.DateEndProbation = datecontract;
                //    profileEntity.WorkPlaceID = CandidateGeneral.WorkPlaceID;
                //}

                // var workplaceServices = new Cat_WorkPlaceServices();
                // Cat_WorkPlaceEntity workplace = null;
                //if(CandidateGeneral.WorkPlaceID != Guid.Empty)
                //{
                //workplace = workplaceServices.GetData<Cat_WorkPlaceEntity>(Common.DotNetToOracle(CandidateGeneral.WorkPlaceID.ToString()), ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status).FirstOrDefault();
                //}

                //var WorkHistoryEntity = new Hre_WorkHistoryEntity();
                //    WorkHistoryEntity.ProfileID = CandidateGeneral.ProfileID.Value;
                //    WorkHistoryEntity.DateEffective = CandidateGeneral.EnteringDate != null ? CandidateGeneral.EnteringDate.Value ? DateTime.Now;
                //    WorkHistoryEntity.SalaryClassID = CandidateGeneral.SalaryClassID;
                //    WorkHistoryEntity.OrganizationStructureID = CandidateGeneral.OrgStructureID;
                //    WorkHistoryEntity.WorkLocation = workplace != null ? workplace.WorkPlaceName : null;
                //    workHistoryServices.Add(WorkHistoryEntity);
                //} 
                #endregion

                var datecontract = DateTime.Now;
                if (CandidateGeneral.ContractTypeID != Guid.Empty)
                {
                    contractypeentity = actionService.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(CandidateGeneral.ContractTypeID.ToString()),
                        ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status).FirstOrDefault();
                }

                if (contractypeentity != null && contractypeentity.ValueTime != null)
                {
                    if (contractypeentity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                    {
                        datecontract = datecontract.AddMonths(int.Parse(contractypeentity.ValueTime.Value.ToString()));
                    }
                    else if (contractypeentity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                    {
                        datecontract = datecontract.AddYears(int.Parse(contractypeentity.ValueTime.Value.ToString()));
                    }
                }
                profileServices.UpdateSalaryClassNameForProfile(null, CandidateGeneral.ProfileID.ToString(), datecontract, CandidateGeneral.EnteringDate.Value,
                    CandidateGeneral.OrgStructureID.Value, CandidateGeneral.RankRateID.Value, CandidateGeneral.WorkPlaceID.Value, CandidateGeneral.ContractTypeID.Value,
                    CandidateGeneral.BasicSalary.ToString(), UserLogin);
                //  profileServices.AddDataForContract(CandidateGeneral.BasicSalary.ToString(), CandidateGeneral.ProfileID.ToString(), CandidateGeneral.ContractTypeID.Value, CandidateGeneral.EnteringDate.Value, CandidateGeneral.RankRateID.Value);
                salgradeServices.AddDataForGrade(CandidateGeneral.ProfileID.ToString(), CandidateGeneral.GradePayrollID.Value, CandidateGeneral.EnteringDate.Value);
                attgradeservive.AddDataForGrade(CandidateGeneral.ProfileID.ToString(), CandidateGeneral.GradeAttendanceID.Value, CandidateGeneral.EnteringDate.Value);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UpdateSalaryClassNameForProfile(string SalaryClassName, string ProfileIDs, DateTime DateEndProbation, DateTime DateHire, Guid OrgStructureID, Guid SalaryRankID, Guid WorkPlaceID, Guid ContractTypeID, string BasicSalary)
        {
            var service = new Hre_ProfileServices();
            service.UpdateSalaryClassNameForProfile(SalaryClassName, ProfileIDs, DateEndProbation, DateHire, OrgStructureID, SalaryRankID, WorkPlaceID, ContractTypeID, BasicSalary, UserLogin);
            return Json(null);
        }

        [HttpPost]
        public ActionResult UpdateDataForProfileWaiting(string SalaryClassName, string ProfileIDs, DateTime? DateEndProbation,
            DateTime DateHire, Guid OrgStructureID, Guid? SalaryRankID, Guid? WorkPlaceID, Guid ContractTypeID, string BasicSalary,
            Guid? GradePayrollID, Guid? GradeAttendanceID,

            Guid? jobTitleID, Guid? positionID, Guid? allowanceID1, Guid? allowanceID2, Guid? allowanceID3, Guid? allowanceID4, Guid? allowanceID5,
            double? allowance1, double? allowance2, double? allowance3, double? allowance4, double? allowance5, Guid? currencyID, double? insuranceSalary, string codeEmp)
        {
            var profileSevices = new Hre_ProfileServices();
            string message = string.Empty;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var workingHistoryServices = new Hre_WorkHistoryServices();
            var basicSalaryServices = new Sal_BasicSalaryServices();
            var insServices = new Sal_InsuranceSalaryServices();
            var attGradeServices = new Att_GradeServices();
            var salGradeServices = new Sal_GradeServices();
            var contractServices = new Hre_ContractServices();
            var contractTypeServices = new Cat_ContractTypeServices();
            var currencyServices = new Cat_CurrencyServices();
            var settingServices = new Sys_AllSettingServices();

            var lstProfiles = actionService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);

            var candidateServices = new Hre_CandidateGeneralServices();
            var lstCandidateGeneral = actionService.GetData<Hre_CandidateGeneralEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_hr_sp_get_CandidateGeneralByProfileIDs, ref status).ToList();

            var salaryRankServices = new Cat_SalaryRankServices();
            var lstObjRank = new List<object>();
            lstObjRank.Add(null);
            lstObjRank.Add(null);
            lstObjRank.Add(1);
            lstObjRank.Add(int.MaxValue - 1);
            var lstRank = actionService.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();
            var rankEntity = lstRank.Where(s => s.ID == SalaryRankID).FirstOrDefault();

            Cat_SalaryClassEntity salaryClassEntity = null;
            if (rankEntity != null)
            {
                var salaryClassServices = new Cat_SalaryClassServices();
                var lstObjClass = new List<object>();
                lstObjClass.Add(null);
                lstObjClass.Add(1);
                lstObjClass.Add(int.MaxValue - 1);
                var salaryClass = actionService.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList();
                salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();
            }

            var lstObjContractType = new List<object>();
            lstObjContractType.AddRange(new object[6]);
            lstObjContractType[4] = 1;
            lstObjContractType[5] = int.MaxValue - 1;
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            var lstProfile = new List<Hre_ProfileEntity>();
            var workplaceServices = new Cat_WorkPlaceServices();
            var workplace = actionService.GetData<Cat_WorkPlaceEntity>(Common.DotNetToOracle(WorkPlaceID.ToString()), ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status).FirstOrDefault();

            var lstAttGrade = actionService.GetData<Att_GradeEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_sal_sp_get_Att_GradeByProfileIds, ref status).ToList();

            var lstSalGrade = actionService.GetData<Sal_GradeEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_sal_sp_get_Sal_GradeByProfileIds, ref status).ToList();

            var lstBasicSalary = actionService.GetData<Sal_BasicSalaryEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_sal_sp_get_BasicSalaryByProfileIds, ref status).ToList();

            var lstInsuranceSalary = actionService.GetData<Sal_InsuranceSalaryEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_sal_sp_get_InsuranceSalaryByProfileIds, ref status).ToList();

            var lstWorkingHistory = actionService.GetData<Hre_WorkHistoryEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileIds, ref status).ToList();

            foreach (var item in lstProfiles)
            {
                var candidateGeneralByProfile = lstCandidateGeneral.Where(s => s.ProfileID.Value == item.ID).FirstOrDefault();

                var objContract = new List<object>();
                var lstContractByProfileID = actionService.GetData<Hre_ContractEntity>(Common.DotNetToOracle(item.ID.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
                var listIdContract = string.Empty;
                if (lstContractByProfileID != null)
                {
                    listIdContract = string.Join(",", lstContractByProfileID.Select(d => d.ContractTypeID));
                }
                var contractType = lstContractType.Where(s => s.ID == ContractTypeID).FirstOrDefault();
                DateTime dateEnd = DateHire;
                if (contractType != null)
                {
                    if (contractType.ValueTime != null)
                    {
                        if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                        {
                            dateEnd = DateHire.AddMonths(int.Parse(contractType.ValueTime.Value.ToString()));
                        }
                        else if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                        {
                            dateEnd = DateHire.AddYears(int.Parse(contractType.ValueTime.Value.ToString()));
                        }
                    }
                }
                double Salary = 0;
                if (!string.IsNullOrEmpty(BasicSalary))
                    Salary = double.Parse(BasicSalary);
                if (candidateGeneralByProfile == null)
                {
                    #region Add CandidateGeneral
                    Hre_CandidateGeneralEntity candidateGeneral = new Hre_CandidateGeneralEntity();
                    candidateGeneral.ProfileID = item.ID;
                    candidateGeneral.BasicSalary = Salary;
                    candidateGeneral.RankRateID = SalaryRankID;
                    if (salaryClassEntity != null)
                    {
                        candidateGeneral.SalaryClassID = salaryClassEntity.ID;
                    }
                    candidateGeneral.ContractTypeID = ContractTypeID;
                    candidateGeneral.EnteringDate = DateHire;
                    candidateGeneral.OrgStructureID = OrgStructureID;
                    candidateGeneral.GradeAttendanceID = GradeAttendanceID;
                    candidateGeneral.GradePayrollID = GradePayrollID;
                    candidateGeneral.WorkPlaceID = WorkPlaceID;
                    candidateGeneral.JobTitleID = jobTitleID;
                    candidateGeneral.PositionID = positionID;
                    candidateGeneral.AllowanceID1 = allowanceID1;
                    candidateGeneral.AllowanceID2 = allowanceID2;
                    candidateGeneral.AllowanceID3 = allowanceID3;
                    candidateGeneral.AllowanceID4 = allowanceID4;
                    candidateGeneral.AllowanceID5 = allowanceID5;
                    candidateGeneral.Allowance1 = allowance1;
                    candidateGeneral.Allowance2 = allowance2;
                    candidateGeneral.Allowance3 = allowance3;
                    candidateGeneral.Allowance4 = allowance4;
                    candidateGeneral.Allowance5 = allowance5;
                    candidateGeneral.CurrencyID = currencyID;
                    candidateGeneral.CodeEmp = codeEmp;
                    message = candidateServices.Add(candidateGeneral);

                    #endregion

                    #region Add Contract
                    //Add new contract

                    Hre_ContractEntity Contract = new Hre_ContractEntity();
                    Contract.ProfileID = item.ID;
                    Contract.Salary = Salary;
                    Contract.ContractTypeID = ContractTypeID;
                    Contract.DateStart = DateHire;
                    Contract.DateSigned = DateHire;
                    Contract.RankRateID = SalaryRankID;
                    if (salaryClassEntity != null)
                    {
                        Contract.ClassRateID = salaryClassEntity.ID;
                    }

                    Contract.InsuranceAmount = insuranceSalary;
                    Contract.AllowanceID1 = allowanceID1;
                    Contract.AllowanceID2 = allowanceID2;
                    Contract.AllowanceID3 = allowanceID3;
                    Contract.AllowanceID4 = allowanceID4;
                    Contract.Allowance1 = allowance1;
                    Contract.Allowance2 = allowance2;
                    Contract.Allowance3 = allowance3;
                    Contract.Allowance4 = allowance4;
                    Contract.Allowance = allowance5;

                    Contract.CurenncyID = currencyID;
                    Contract.CurenncyID1 = currencyID;
                    Contract.CurenncyID2 = currencyID;
                    Contract.CurenncyID3 = currencyID;
                    Contract.CurenncyIDSalary = currencyID;
                    Contract.CurenncyID4 = currencyID;
                    Contract.CurenncyID5 = currencyID;
                    Contract.Status = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_WAITING.ToString();
                    Contract.JobTitleID = jobTitleID;
                    Contract.PositionID = positionID;

                    if (!string.IsNullOrEmpty(contractType.Formula))
                    {
                        Contract = SetNewDateEndContract(Contract);
                    }
                    Contract.DateExtend = Contract.DateEnd;
                    //   Contract = SetNewCodeContract(Contract, listIdContract);
                    message = contractServices.Add(Contract);
                    #endregion

                    #region Edit Profile
                    //Edit Profile
                    item.OrgStructureID = OrgStructureID;
                    if (salaryClassEntity != null)
                    {
                        item.SalaryClassID = salaryClassEntity.ID;
                    }
                    item.DateOfEffect = DateHire;
                    item.DateHire = DateHire;
                    item.DateEndProbation = Contract.DateEnd.Value;
                    item.WorkPlaceID = WorkPlaceID;
                    item.ContractTypeID = ContractTypeID;
                    item.CodeEmp = codeEmp;
                    item.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();

                    message = profileSevices.Edit(item);
                    #endregion

                    #region Add Sal_Insurance
                    // Add Insurance
                    if (contractType != null && contractType.NoneTypeInsuarance == true)
                    {
                        var insuranceEntity = new Sal_InsuranceSalaryEntity
                        {
                            ProfileID = item.ID,
                            InsuranceAmount = insuranceSalary,
                            DateEffect = DateHire,
                            IsSocialIns = contractType.IsSocialInsurance == null ? null : contractType.IsSocialInsurance,
                            IsUnimploymentIns = contractType.IsUnEmployInsurance == null ? null : contractType.IsUnEmployInsurance,
                            IsMedicalIns = contractType.IsHealthInsurance == null ? null : contractType.IsHealthInsurance,
                            CurrencyID = currencyID
                        };
                        message = insServices.Add(insuranceEntity);
                    }
                    #endregion

                    #region Add Sal_BasicSalary
                    Sal_BasicSalaryEntity basicSalaryEntity = new Sal_BasicSalaryEntity();
                    basicSalaryEntity.ProfileID = item.ID;
                    basicSalaryEntity.GrossAmount = BasicSalary;
                    basicSalaryEntity.Amount = BasicSalary.Encrypt();
                    basicSalaryEntity.DateOfEffect = DateHire;
                    basicSalaryEntity.RankRateID = SalaryRankID;
                    basicSalaryEntity.CurrencyID = currencyID.Value;
                    if (salaryClassEntity != null)
                    {
                        basicSalaryEntity.ClassRateID = salaryClassEntity.ID;
                    }

                    basicSalaryEntity.AllowanceType1ID = allowanceID1;
                    basicSalaryEntity.AllowanceType2ID = allowanceID2;
                    basicSalaryEntity.AllowanceType3ID = allowanceID3;
                    basicSalaryEntity.AllowanceType4ID = allowanceID4;
                    basicSalaryEntity.AllowanceTypeID5 = allowanceID5;

                    basicSalaryEntity.AllowanceAmount1 = allowance1;
                    basicSalaryEntity.AllowanceAmount2 = allowance2;
                    basicSalaryEntity.AllowanceAmount3 = allowance3;
                    basicSalaryEntity.AllowanceAmount4 = allowance4;
                    basicSalaryEntity.AllowanceAmount5 = allowance5;

                    basicSalaryEntity.CurrencyID5 = currencyID;
                    basicSalaryEntity.CurrencyID2 = currencyID;
                    basicSalaryEntity.CurrencyID3 = currencyID;
                    basicSalaryEntity.CurrencyID4 = currencyID;
                    if (insuranceSalary != null)
                    {
                        basicSalaryEntity.InsuranceAmount = insuranceSalary.Value;
                    }

                    message = basicSalaryServices.Add(basicSalaryEntity);
                    #endregion

                    #region Add WorkHistory
                    Hre_WorkHistoryEntity workHistory = new Hre_WorkHistoryEntity();
                    workHistory.ProfileID = item.ID;
                    workHistory.DateEffective = DateHire;
                    if (salaryClassEntity != null)
                    {
                        workHistory.SalaryClassID = salaryClassEntity.ID;
                    }
                    workHistory.OrganizationStructureID = OrgStructureID;
                    workHistory.WorkLocation = workplace != null ? workplace.WorkPlaceName : null;
                    workHistory.JobTitleID = jobTitleID;
                    workHistory.PositionID = positionID;

                    message = workingHistoryServices.Add(workHistory);
                    #endregion

                    #region Add Att_Grade
                    Att_GradeEntity attGradeEntity = new Att_GradeEntity();
                    attGradeEntity.ProfileID = item.ID;
                    attGradeEntity.GradeAttendanceID = GradeAttendanceID;
                    attGradeEntity.MonthStart = DateHire;
                    message = attGradeServices.Add(attGradeEntity);
                    #endregion

                    #region Add Sal_Grade
                    Sal_GradeEntity salGradeEntity = new Sal_GradeEntity();
                    salGradeEntity.ProfileID = item.ID;
                    salGradeEntity.GradePayrollID = GradePayrollID;
                    salGradeEntity.MonthStart = DateHire;
                    message = salGradeServices.Add(salGradeEntity);
                    #endregion

                }
                else
                {
                    #region Edit CandidateGeneral

                    candidateGeneralByProfile.ProfileID = item.ID;
                    candidateGeneralByProfile.BasicSalary = Salary;
                    candidateGeneralByProfile.RankRateID = SalaryRankID;
                    if (salaryClassEntity != null)
                    {
                        candidateGeneralByProfile.SalaryClassID = salaryClassEntity.ID;
                    }
                    candidateGeneralByProfile.ContractTypeID = ContractTypeID;
                    candidateGeneralByProfile.EnteringDate = DateHire;
                    candidateGeneralByProfile.OrgStructureID = OrgStructureID;
                    candidateGeneralByProfile.GradeAttendanceID = GradeAttendanceID;
                    candidateGeneralByProfile.GradePayrollID = GradePayrollID;
                    candidateGeneralByProfile.WorkPlaceID = WorkPlaceID;
                    candidateGeneralByProfile.JobTitleID = jobTitleID;
                    candidateGeneralByProfile.PositionID = positionID;
                    candidateGeneralByProfile.AllowanceID1 = allowanceID1;
                    candidateGeneralByProfile.AllowanceID2 = allowanceID2;
                    candidateGeneralByProfile.AllowanceID3 = allowanceID3;
                    candidateGeneralByProfile.AllowanceID4 = allowanceID4;
                    candidateGeneralByProfile.AllowanceID5 = allowanceID5;
                    candidateGeneralByProfile.Allowance1 = allowance1;
                    candidateGeneralByProfile.Allowance2 = allowance2;
                    candidateGeneralByProfile.Allowance3 = allowance3;
                    candidateGeneralByProfile.Allowance4 = allowance4;
                    candidateGeneralByProfile.Allowance5 = allowance5;
                    candidateGeneralByProfile.CurrencyID = currencyID;
                    candidateGeneralByProfile.CodeEmp = codeEmp;
                    message = candidateServices.Edit(candidateGeneralByProfile);

                    #endregion

                    #region Edit Contract
                    if (lstContractByProfileID != null)
                    {
                        var contractEntityByProfileID = lstContractByProfileID.FirstOrDefault();
                        if (contractEntityByProfileID != null)
                        {
                            contractEntityByProfileID.Salary = Salary;
                            contractEntityByProfileID.ContractTypeID = ContractTypeID;
                            contractEntityByProfileID.DateStart = DateHire;
                            contractEntityByProfileID.DateSigned = DateHire;
                            contractEntityByProfileID.DateEnd = dateEnd;
                            contractEntityByProfileID.RankRateID = SalaryRankID;
                            if (salaryClassEntity != null)
                            {
                                contractEntityByProfileID.ClassRateID = salaryClassEntity.ID;
                            }

                            contractEntityByProfileID.InsuranceAmount = insuranceSalary;
                            contractEntityByProfileID.AllowanceID1 = allowanceID1;
                            contractEntityByProfileID.AllowanceID2 = allowanceID2;
                            contractEntityByProfileID.AllowanceID3 = allowanceID3;
                            contractEntityByProfileID.AllowanceID4 = allowanceID4;
                            contractEntityByProfileID.Allowance1 = allowance1;
                            contractEntityByProfileID.Allowance2 = allowance2;
                            contractEntityByProfileID.Allowance3 = allowance3;
                            contractEntityByProfileID.Allowance4 = allowance4;
                            contractEntityByProfileID.Allowance = allowance5;
                            contractEntityByProfileID.CurenncyID = currencyID;
                            contractEntityByProfileID.CurenncyID1 = currencyID;
                            contractEntityByProfileID.CurenncyID2 = currencyID;
                            contractEntityByProfileID.CurenncyID3 = currencyID;
                            contractEntityByProfileID.CurenncyIDSalary = currencyID;
                            contractEntityByProfileID.CurenncyID4 = currencyID;
                            contractEntityByProfileID.CurenncyID5 = currencyID;
                            contractEntityByProfileID.Status = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_WAITING.ToString();
                            contractEntityByProfileID.JobTitleID = jobTitleID;
                            contractEntityByProfileID.PositionID = positionID;
                            if (!string.IsNullOrEmpty(contractType.Formula))
                            {
                                contractEntityByProfileID = SetNewDateEndContract(contractEntityByProfileID);
                            }
                            if (contractEntityByProfileID.DateExtend == null)
                            {
                                contractEntityByProfileID.DateExtend = dateEnd;
                            }
                            message = contractServices.Edit(contractEntityByProfileID);
                            if (contractEntityByProfileID.DateEnd != null)
                            {
                                item.DateEndProbation = contractEntityByProfileID.DateEnd.Value;
                            }
                        }
                    }

                    #endregion

                    #region Edit Profile
                    //Edit Profile
                    item.OrgStructureID = OrgStructureID;
                    if (salaryClassEntity != null)
                    {
                        item.SalaryClassID = salaryClassEntity.ID;
                    }
                    item.DateOfEffect = DateHire;
                    item.DateHire = DateHire;
                    item.WorkPlaceID = WorkPlaceID;
                    item.ContractTypeID = ContractTypeID;
                    item.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();
                    item.CodeEmp = codeEmp;
                    message = profileSevices.Edit(item);
                    #endregion

                    #region Edit Sal_Insurance
                    var insSalaryEntityByProfileID = lstInsuranceSalary.Where(s => s.ProfileID == item.ID).OrderBy(s => s.DateUpdate).FirstOrDefault();
                    if (insSalaryEntityByProfileID != null)
                    {
                        if (contractType != null && contractType.NoneTypeInsuarance == true)
                        {
                            insSalaryEntityByProfileID.InsuranceAmount = insuranceSalary;
                            insSalaryEntityByProfileID.DateEffect = DateHire;
                            insSalaryEntityByProfileID.IsSocialIns = contractType.IsSocialInsurance == null ? null : contractType.IsSocialInsurance;
                            insSalaryEntityByProfileID.IsUnimploymentIns = contractType.IsUnEmployInsurance == null ? null : contractType.IsUnEmployInsurance;
                            insSalaryEntityByProfileID.IsMedicalIns = contractType.IsHealthInsurance == null ? null : contractType.IsHealthInsurance;
                            insSalaryEntityByProfileID.CurrencyID = currencyID;

                            message = insServices.Edit(insSalaryEntityByProfileID);
                        }
                    }
                    #endregion

                    #region Edit Sal_BasicSalary
                    var basicSalaryEntityByProfileID = lstBasicSalary.Where(s => s.ProfileID == item.ID).FirstOrDefault();
                    if (basicSalaryEntityByProfileID != null)
                    {
                        basicSalaryEntityByProfileID.GrossAmount = BasicSalary;
                        basicSalaryEntityByProfileID.Amount = BasicSalary.Encrypt();
                        basicSalaryEntityByProfileID.DateOfEffect = DateHire;
                        basicSalaryEntityByProfileID.RankRateID = SalaryRankID;
                        if (salaryClassEntity != null)
                        {
                            basicSalaryEntityByProfileID.ClassRateID = salaryClassEntity.ID;
                        }
                        basicSalaryEntityByProfileID.CurrencyID = currencyID.Value;
                        basicSalaryEntityByProfileID.AllowanceType1ID = allowanceID1;
                        basicSalaryEntityByProfileID.AllowanceType2ID = allowanceID2;
                        basicSalaryEntityByProfileID.AllowanceType3ID = allowanceID3;
                        basicSalaryEntityByProfileID.AllowanceType4ID = allowanceID4;
                        basicSalaryEntityByProfileID.AllowanceTypeID5 = allowanceID5;
                        basicSalaryEntityByProfileID.AllowanceAmount1 = allowance1;
                        basicSalaryEntityByProfileID.AllowanceAmount2 = allowance2;
                        basicSalaryEntityByProfileID.AllowanceAmount3 = allowance3;
                        basicSalaryEntityByProfileID.AllowanceAmount4 = allowance4;
                        basicSalaryEntityByProfileID.AllowanceAmount5 = allowance5;
                        basicSalaryEntityByProfileID.CurrencyID5 = currencyID;
                        basicSalaryEntityByProfileID.CurrencyID2 = currencyID;
                        basicSalaryEntityByProfileID.CurrencyID3 = currencyID;
                        basicSalaryEntityByProfileID.CurrencyID4 = currencyID;
                        if (insuranceSalary != null)
                        {
                            basicSalaryEntityByProfileID.InsuranceAmount = insuranceSalary.Value;
                        }
                        message = basicSalaryServices.Edit(basicSalaryEntityByProfileID);
                    }
                    #endregion

                    #region Edit WorkingHistory
                    var workingByProfileID = lstWorkingHistory.Where(s => s.ProfileID == item.ID).FirstOrDefault();
                    if (workingByProfileID != null)
                    {
                        workingByProfileID.DateEffective = DateHire;
                        if (salaryClassEntity != null)
                        {
                            workingByProfileID.SalaryClassID = salaryClassEntity.ID;
                        }
                        workingByProfileID.OrganizationStructureID = OrgStructureID;
                        workingByProfileID.WorkLocation = workplace != null ? workplace.WorkPlaceName : null;
                        workingByProfileID.JobTitleID = jobTitleID;
                        workingByProfileID.PositionID = positionID;
                        message = workingHistoryServices.Edit(workingByProfileID);
                    }
                    #endregion

                    #region Edit Att_Grade
                    var attGradeEntityByProfileID = lstAttGrade.Where(s => s.ProfileID == item.ID).FirstOrDefault();
                    if (attGradeEntityByProfileID != null)
                    {
                        attGradeEntityByProfileID.GradeAttendanceID = GradeAttendanceID;
                        attGradeEntityByProfileID.MonthStart = DateHire;
                        message = attGradeServices.Edit(attGradeEntityByProfileID);
                    }
                    #endregion

                    #region Edit Sal_Grade
                    var salGradeEntityByProfileID = lstSalGrade.Where(s => s.ProfileID == item.ID).FirstOrDefault();
                    if (salGradeEntityByProfileID != null)
                    {
                        salGradeEntityByProfileID.GradePayrollID = GradePayrollID;
                        salGradeEntityByProfileID.MonthStart = DateHire;
                        message = salGradeServices.Edit(salGradeEntityByProfileID);
                    }
                    #endregion
                }
            }


            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddDataForContract(string BasicSalary, string ProfileIDs, Guid ContractTypeID, DateTime DateHire, Guid SalaryRankID)
        {
            var service = new Hre_ProfileServices();
            service.AddDataForContract(BasicSalary, ProfileIDs, ContractTypeID, DateHire, SalaryRankID, UserLogin);
            return Json(null);
        }
        [HttpPost]
        public ActionResult UpdateStatusApprovedProfile(string selectedIds)
        {
            var service = new Hre_ProfileServices();
            service.UpdateStatusApprovedProfile(selectedIds);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateStatusApprovedContract(string selectedIds, string Status)
        {
            var service = new Hre_ContractServices();
            service.SubmitStatus(selectedIds, Status);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProfileKeepingSalary([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return GetListDataAndReturn<Hre_ProfileModel, Hre_ProfileEntity, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_KeepingSalary);
        }

        [HttpPost]
        public ActionResult ExportProfileKeepingSalaryList([DataSourceRequest] DataSourceRequest request, Hre_ProfileSearchModel model)
        {
            return ExportAllAndReturn<Hre_ProfileEntity, Hre_ProfileModel, Hre_ProfileSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_KeepingSalary);
        }

        [HttpPost]
        public ActionResult GetTravelRequestList([DataSourceRequest] DataSourceRequest request, FIN_TravelRequestSearchModel model)
        {
            return GetListDataAndReturn<FIN_TravelRequestModel, FIN_TravelRequestEntity, FIN_TravelRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_TravelRequest);
        }


        [HttpPost]
        public ActionResult CheckAllowRemoveFinTravelRequest(string id)
        {
            FIN_ApproverECLAIMService service = new FIN_ApproverECLAIMService();
            var isAllowRemoved = service.CheckAllowRemoveFinTravelRequest(id);
            return Json(isAllowRemoved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckAllowRemoveCashAdvance(string id)
        {
            FIN_ApproverECLAIMService service = new FIN_ApproverECLAIMService();
            var isAllowRemoved = service.CheckAllowRemoveCashAdvance(id);
            return Json(isAllowRemoved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckAllowRemoveClaim(string id)
        {
            FIN_ApproverECLAIMService service = new FIN_ApproverECLAIMService();
            var isAllowRemoved = service.CheckAllowRemoveClaim(id);
            return Json(isAllowRemoved, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportTravelRequestByTemplate([DataSourceRequest] DataSourceRequest request, FIN_TravelRequestSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new FIN_TravelRequestItemModel();
            var lstModel = new List<FIN_TravelRequestItemModel>();
            request.Page = 1;
            request.PageSize = int.MaxValue - 1;
            var result = GetListData<FIN_TravelRequestModel, FIN_TravelRequestEntity, FIN_TravelRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_TravelRequest, ref status);
            //string _ProfileName = "";
            //string _PositionName = "";
            if (result != null)
            {
                //_ProfileName = result[0].ProfileName;
                //_PositionName = result[0].PositionName;
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(result[0].ID);
                lstModel = actionService.GetData<FIN_TravelRequestItemModel>(objs, ConstantSql.hrm_hr_sp_get_TravelRequestItemByTravelRequestID, ref status);
            }
            //HeaderInfo headerInfo1 = new HeaderInfo() { Name = "ProfileName", Value = _ProfileName };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "PositionName", Value = _PositionName };
            //List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstModel;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "FIN_TravelRequestItemModel",
                    OutPutPath = path,
                    //HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
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
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetTravelRequestItemByTravelRequestID([DataSourceRequest] DataSourceRequest request, Guid? TravelRequestID)
        {
            if (TravelRequestID != null)
            {
                string status = string.Empty;
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(TravelRequestID.ToString()));
                var result = actionService.GetData<FIN_TravelRequestItemModel>(objs, ConstantSql.hrm_hr_sp_get_TravelRequestItemByTravelRequestID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }

        public ActionResult GetClaimItemByClaimID([DataSourceRequest] DataSourceRequest request, Guid? ClaimID)
        {
            if (ClaimID != null)
            {
                string status = string.Empty;
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(ClaimID.ToString()));
                var result = actionService.GetData<FIN_ClaimItemModel>(objs, ConstantSql.hrm_hr_sp_get_ClaimItemByClaimID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }

        public ActionResult GetCashAdvanceItemByCashAdvanceID([DataSourceRequest] DataSourceRequest request, Guid? CashAdvanceID)
        {
            if (CashAdvanceID != null)
            {
                string status = string.Empty;
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(Common.DotNetToOracle(CashAdvanceID.ToString()));
                var result = actionService.GetData<FIN_CashAdvanceItemModel>(objs, ConstantSql.hrm_hr_sp_get_CashAdvanceItemByCashAdvanceID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }


        #region Hre_StopWorking

        [HttpPost]
        public ActionResult GetStopWorkingList([DataSourceRequest] DataSourceRequest request, Hre_StopWorkingSearchModel model)
        {
            return GetListDataAndReturn<Hre_StopWorkingModel, Hre_StopWorkingEntity, Hre_StopWorkingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_StopWorking);
        }

        public ActionResult ExportStopWorkingList([DataSourceRequest] DataSourceRequest request, Hre_StopWorkingSearchModel model)
        {
            return ExportAllAndReturn<Hre_StopWorkingEntity, Hre_StopWorkingModel, Hre_StopWorkingSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_StopWorking);
        }

        [HttpPost]
        public ActionResult ApproveStopWorking(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.ActionApproved(selectedIds, UserLogin);
            return Json(message);
        }

        [HttpPost]
        public ActionResult ActionCancelComback(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.ActionCancelComback(selectedIds);
            return Json(message);
        }
        [HttpPost]
        public ActionResult CancelStopWorking(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.ActionCancel(selectedIds, UserLogin);
            return Json(message);
        }

        [HttpPost]
        public ActionResult ApproveSuspense(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.ActionApprovedSuspense(selectedIds, UserLogin);
            return Json(message);
        }

        [HttpPost]
        public ActionResult ApproveComBack(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.ActionApprovedComeBack(selectedIds, UserLogin);
            return Json(message);
        }

        [HttpPost]
        public ActionResult UpdateWorkingPosition(string selectedIds)
        {
            var service = new Hre_StopWorkingServices();
            var message = service.UpdateWorkingPosition(selectedIds, UserLogin);
            return Json(message);
        }

        public ActionResult ExportStopWorkingByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;

            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_WorkHistoryServices();
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstStopWorking = actionService.GetData<Hre_StopWorkingEntity>(objs, ConstantSql.hrm_hr_sp_get_StopWorkingByListId, ref status);
            if (lstStopWorking == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_StopWorking" + suffix;
            if (lstStopWorking.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var stopWorking in lstStopWorking)
            {
                if (stopWorking.DateStop.HasValue)
                {
                    stopWorking.DateStopFormat = stopWorking.DateStop.Value.ToString("dd/MM/yyyy");
                }
                if (stopWorking.DateQuit.HasValue)
                    stopWorking.DateQuitFormat = stopWorking.DateQuit.Value.ToString("dd/MM/yyyy");
                if (stopWorking.RequestDate.HasValue)
                    stopWorking.RequestDateFormat = stopWorking.RequestDate.Value.ToString("dd/MM/yyyy");
                stopWorking.RequestDate_Day = stopWorking.RequestDate.Value.Day.ToString();
                stopWorking.RequestDate_Month = stopWorking.RequestDate.Value.Month.ToString();
                stopWorking.RequestDate_Year = stopWorking.RequestDate.Value.Year.ToString();


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
                    template = actionService.GetData<Cat_ExportEntity>(Common.DotNetToOracle(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();

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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(stopWorking.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(stopWorking.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lststopworking = new List<Hre_StopWorkingEntity>();
                lststopworking.Add(stopWorking);
                ExportService.ExportWord(outputPath, templatepath, lststopworking);
            }
            List<object> lstNo = new List<object>();
            lstNo.Add(strIDs);
            if (lstStopWorking.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);

                var updateNoPrint1 = actionService.UpdateData<Hre_StopWorkingEntity>(lstNo, ConstantSql.hrm_hr_sp_set_UpdateNoPrint, ref status);
                return Json(fileZip);
            }
            var updateNoPrint2 = actionService.UpdateData<Hre_StopWorkingEntity>(lstNo, ConstantSql.hrm_hr_sp_set_UpdateNoPrint, ref status);
            return Json(fileDoc);
        }


        public string ValidateDateComback(Guid ProfileID, DateTime DateStop)
        {
            string status = string.Empty;
            string messageValidate = string.Empty;
            var actionService = new ActionService(UserLogin);
            var result = actionService.GetData<Hre_StopWorkingEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_StopWorkingByProId, ref status)
            .Where(s => s.StopWorkType == "E_SUSPENSE").OrderByDescending(s => s.DateStop).FirstOrDefault();
            if (result != null)
            {
                if (result.DateStop != null && result.DateComeBack == null)
                {
                    messageValidate = ConstantDisplay.HRM_HR_ProfileInSuspenseTime.TranslateString();
                }
                else if (result.DateStop != null && result.DateComeBack != null && result.DateStop <= DateStop && DateStop <= result.DateComeBack)
                {
                    messageValidate = ConstantDisplay.HRM_HR_ProfileHaveSuspenseInThisTime.TranslateString();

                }
            }
            return messageValidate;
        }

        public string ValidateRegisterComback(Guid ProfileID, DateTime DateStop, DateTime DateComback)
        {
            string status = string.Empty;
            string messageValidate = string.Empty;
            var actionService = new ActionService(UserLogin);
            var result = actionService.GetData<Hre_StopWorkingEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_StopWorkingByProId, ref status).Where(s => s.DateComeBack != null)
            .OrderByDescending(s => s.DateStop).FirstOrDefault();
            if (result != null)
            {

                if (result.DateStop != null && result.DateComeBack != null
                    && DateStop == result.DateStop && DateComback != result.DateComeBack)
                {
                    messageValidate = ConstantDisplay.HRM_HRM_EmpHaveRegisterComback.TranslateString();
                }
            }
            return messageValidate;
        }

        [HttpPost]
        public ActionResult GetDataViewStop(string ProfileID)
        {
            string status = string.Empty;
            var profileID = Guid.Empty;
            if (ProfileID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            var profileServices = new Hre_ProfileServices();
            var actionService = new ActionService(UserLogin);
            var profile = actionService.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            string deptPath = null;

            if (profile != null)
            {
                if (profile.OrgStructureID != null)
                {
                    deptPath = GetCodeOrgStructure(profile.OrgStructureID.Value);
                }
                profile.OrgStructureName = deptPath;
                return Json(profile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetDataByRegistSuspenseID(string SuspenseID)
        {
            string status = string.Empty;
            var suspenseID = Guid.Empty;
            if (SuspenseID.IndexOf(',') > 0)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(SuspenseID))
            {
                suspenseID = Common.ConvertToGuid(SuspenseID);
            }

            var actionService = new ActionService(UserLogin);
            var lstProfile = actionService.GetData<Hre_StopWorkingEntity>(suspenseID, ConstantSql.hrm_hr_sp_get_StopWorkingById, ref status).FirstOrDefault();

            if (lstProfile != null)
            {
                var model = lstProfile.CopyData<Hre_StopWorkingModel>();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        #endregion

        #region Hre_RegisterSuspense

        [HttpPost]
        public ActionResult GetSuspenseList([DataSourceRequest] DataSourceRequest request, Hre_RegisterSuspenseSearchModel model)
        {
            return GetListDataAndReturn<Hre_StopWorkingModel, Hre_StopWorkingEntity, Hre_RegisterSuspenseSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Suspense);
        }

        [HttpPost]
        public ActionResult GetProfileSuspenseList([DataSourceRequest] DataSourceRequest request, Hre_SuspenseSearchModel model)
        {
            return GetListDataAndReturn<Hre_StopWorkingModel, Hre_StopWorkingEntity, Hre_SuspenseSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileSuspense);
        }

        public ActionResult ExportProfileSuspenseListByTemplate([DataSourceRequest] DataSourceRequest request, Hre_SuspenseSearchModel model)
        {
            #region Tao template
            
            
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom == null ? DateTime.Now : model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo == null ? DateTime.Now : model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_StopWorkingModel(),
                    FileName = "Hre_StopWorking",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion
            string status = string.Empty;
           // var isDataTable = false;
            object obj = new Hre_ProfileModel();
            var actionService = new ActionService(UserLogin);

            var lstObj = new List<object>();
            lstObj.Add(model.ProfileName);
            lstObj.Add(model.CodeEmp);
            lstObj.Add(model.JobTitleID);
            lstObj.Add(model.PositionID);
            lstObj.Add(model.OrgStructureID);
            lstObj.Add(model.DateFrom);
            lstObj.Add(model.DateTo);
            lstObj.Add(model.TypeSuspense);
            lstObj.Add(model.Status);
            lstObj.Add(model.StatusComeBack);
            lstObj.Add(model.RequestDateComebackFrom);
            lstObj.Add(model.RequestDateComebackTo);
            lstObj.Add(model.DateComebackFrom);
            lstObj.Add(model.DateComebackTo);
            lstObj.Add(model.RankID);
            lstObj.Add(model.IsCreateTemplate);
            lstObj.Add(model.ExportId);
            lstObj.Add(model.ExportType);
            lstObj.Add(1);
            lstObj.Add(int.MaxValue - 1);

            var lisObjClas = new List<object>();
            lisObjClas.AddRange(new object[3]);
            lisObjClas[1] = 1;
            lisObjClas[2] = int.MaxValue - 1;
            var lisObjRank = new List<object>();
            lisObjRank.AddRange(new object[4]);
            lisObjRank[2] = 1;
            lisObjRank[3] = int.MaxValue - 1;
            var listResult = actionService.GetData<Hre_StopWorkingEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileSuspense, ref status).Translate<Hre_StopWorkingModel>();
            var lisClass = actionService.GetData<Cat_SalaryClassEntity>(lisObjClas,ConstantSql.hrm_cat_sp_get_SalaryClass, ref status);
            var lisRank = actionService.GetData<Cat_SalaryRankEntity>(lisObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status);
            foreach (var item in listResult)
            {
               
                var lisClassbyproid = lisClass.Where(s => s.ID == item.SalaryClassID).FirstOrDefault();
                var lisRankbyclassid = lisRank.Where(s => lisClassbyproid !=null && s.SalaryClassID == lisClassbyproid.ID).FirstOrDefault();
                item.SalaryRankName = lisRankbyclassid != null ? lisRankbyclassid.SalaryRankName : string.Empty;
            }
            var profileServices = new Hre_ProfileServices();
           

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, listResult, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(listResult.ToDataSourceResult(request));
        }


        public ActionResult ExportSuspenseList([DataSourceRequest] DataSourceRequest request, Hre_RegisterSuspenseSearchModel model)
        {
            return ExportAllAndReturn<Hre_StopWorkingEntity, Hre_StopWorkingModel, Hre_RegisterSuspenseSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Suspense);
        }

        // Xuất template đăng ký tạm hoãn theo mẫu word
        public ActionResult ExportListSuspenseByTemplate(List<Guid> selectedIds, string valueFields)
        {
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_StopWorkingServices();
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstStopWorking = actionService.GetData<Hre_StopWorkingEntity>(objs, ConstantSql.hrm_hr_sp_get_StopWorkingByListId, ref status);
            if (lstStopWorking == null)
                return null;
            int i = 0;
            var lstStopWorkingID = lstStopWorking.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContract = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();

            var objAppendixContarct = new List<object>();
            objAppendixContarct.AddRange(new object[7]);
            objAppendixContarct[5] = 1;
            objAppendixContarct[6] = int.MaxValue - 1;
            var lstAppendixContract = actionService.GetData<Hre_AppendixContractEntity>(objAppendixContarct, ConstantSql.hrm_hr_sp_get_AppendixContract, ref status).ToList();
            if (lstStopWorkingID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var stopWorking in lstStopWorking)
            {
                var contractEntity = lstContract.Where(s => s.ProfileID == stopWorking.ProfileID.Value).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

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

                template = actionService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                if (contractEntity != null)
                {
                    var appendixContract = lstAppendixContract.Where(s => s.ContractID == contractEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    stopWorking.ContractTypeName = contractEntity.ContractTypeName;
                    stopWorking.DateStartFormat = contractEntity.DateStart.ToString("dd/MM/yyyy");
                    stopWorking.Salary = contractEntity.Salary != null ? contractEntity.Salary : 0;
                    stopWorking.ContractNo = contractEntity.ContractNo != null ? contractEntity.ContractNo : string.Empty;
                    stopWorking.YearOfBirth = contractEntity.YearOfBirth;
                    stopWorking.MonthOfBirth = contractEntity.MonthOfBirth;
                    stopWorking.DayOfBirth = contractEntity.DayOfBirth;

                    if (contractEntity.DateSigned.HasValue)
                    {
                        stopWorking.DateSignedFormat = contractEntity.DateSigned.Value.ToString("dd/MM/yyyy");
                    }

                    if (contractEntity.DateEnd.HasValue)
                    {
                        stopWorking.DateEndFormat = contractEntity.DateEnd.Value.ToString("dd/MM/yyyy");
                    }


                    if (appendixContract != null)
                    {
                        if (appendixContract.DateEndAppendixContract.HasValue)
                        {
                            stopWorking.DateEndAppendixContractFormat = appendixContract.DateEndAppendixContract.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (stopWorking.RequestDate.HasValue)
                {
                    stopWorking.RequestDateString = stopWorking.RequestDate.Value.ToString("dd/MM/yyyy");
                }

                if (stopWorking.DateStop.HasValue)
                {
                    stopWorking.DateStopString = stopWorking.DateStop.Value.ToString("dd/MM/yyyy");
                }

                if (stopWorking.DateComeBack.HasValue)
                {
                    stopWorking.DateComeBackString = stopWorking.DateComeBack.Value.ToString("dd/MM/yyyy");
                }

                if (stopWorking.DateExpired.HasValue)
                {
                    stopWorking.DateExpiredString = stopWorking.DateExpired.Value.ToString("dd/MM/yyyy");
                }

                if (stopWorking.DateHire.HasValue)
                {
                    stopWorking.DateHireString = stopWorking.DateHire.Value.ToString("dd/MM/yyyy");
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(stopWorking.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(stopWorking.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lststopWorking = new List<Hre_StopWorkingEntity>();
                lststopWorking.Add(stopWorking);
                ExportService.ExportWord(outputPath, templatepath, lststopWorking);
            }
            if (lstStopWorkingID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }



        #endregion

        #region Hre_RegisterComback

        [HttpPost]
        public ActionResult GetRegisterCombackList([DataSourceRequest] DataSourceRequest request, Hre_RegisterComeBackSearchModel model)
        {
            return GetListDataAndReturn<Hre_StopWorkingModel, Hre_StopWorkingEntity, Hre_RegisterComeBackSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_RegisterComback);
        }

        public ActionResult ExportRegisterCombackList([DataSourceRequest] DataSourceRequest request, Hre_RegisterComeBackSearchModel model)
        {
            return ExportAllAndReturn<Hre_StopWorkingEntity, Hre_StopWorkingModel, Hre_RegisterComeBackSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_RegisterComback);
        }


        public JsonResult GetResonRegisterByProfileID(Guid profileid)
        {
            var result = new Hre_StopWorkingModel();
            string status = string.Empty;
            if (profileid != Guid.Empty)
            {
                var actionService = new ActionService(UserLogin);
                result = actionService.GetData<Hre_StopWorkingModel>(Common.DotNetToOracle(profileid.ToString()), ConstantSql.hrm_hr_sp_get_StopWorkingByProId, ref status).OrderByDescending(s => s.DateStop).FirstOrDefault();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHDTJobGroupByID(Guid HDTJobGroupID)
        {
            var result = new List<Cat_HDTJobGroupModel>();
            string status = string.Empty;
            if (HDTJobGroupID != Guid.Empty)
            {
                var actionService = new ActionService(UserLogin);
                result = actionService.GetData<Cat_HDTJobGroupModel>(HDTJobGroupID, ConstantSql.hrm_cat_sp_get_HDTJobGroupById, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ApprovedPurchaseRequest
        [HttpPost]
        public ActionResult GetApprovedPurchaseRequestList([DataSourceRequest] DataSourceRequest request, Fin_ApprovedPurchaseRequestSearchModel model)
        {
            return GetListDataAndReturn<Fin_PurchaseRequestModel, FIN_PurchaseRequestEntity, Fin_ApprovedPurchaseRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ApprovedPurchaseRequest);
        }
        #endregion
        [HttpPost]
        public ActionResult GetClaimByTravelRequestID([DataSourceRequest] DataSourceRequest request, Guid TravelRequestID)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(TravelRequestID.ToString()));
            var result = actionService.GetData<FIN_ClaimModel>(objs, ConstantSql.hrm_hr_sp_get_ClaimByTravelRequestID, ref status);
            return Json(result.ToDataSourceResult(request));
        }
        //[HttpPost]
        //public ActionResult GetClaimItemByClaimID([DataSourceRequest] DataSourceRequest request, FIN_ClaimItemSearchByClaimIDModel model)
        //{
        //    return GetListDataAndReturn<FIN_ClaimItemModel, FIN_ClaimItemEntity, FIN_ClaimItemSearchByClaimIDModel>(request, model, ConstantSql.hrm_hr_sp_get_ClaimItemByClaimID);
        //}
        [HttpPost]
        public JsonResult GetMultiTravelRequest(string text)
        {
            return GetDataForControl<FIN_TravelRequestMultiModel, FIN_TravelRequestMultiEntity>(text, ConstantSql.hrm_hr_sp_get_TravelRequest_Multi);
        }

        [HttpPost]
        public JsonResult GetMultiCashAdvance(string text)
        {
            return GetDataForControl<FIN_CashAdvanceMultiModel, FIN_CashAdvanceMultiEntity>(text, ConstantSql.hrm_hr_sp_get_CashAdvance_Multi);
        }

        #region ApprovedFIN_Claim
        [HttpPost]
        public ActionResult GetApprovedFIN_ClaimList([DataSourceRequest] DataSourceRequest request, FIN_ApprovedClaimSearchModel model)
        {
            return GetListDataAndReturn<FIN_ClaimModel, FIN_ClaimEntity, FIN_ApprovedClaimSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ApprovedClaim);
        }
        #endregion

        #region ApprovedFIN_TravelRequest
        [HttpPost]
        public ActionResult GetApprovedFIN_TravelRequestList([DataSourceRequest] DataSourceRequest request, FIN_ApprovedTravelRequestSearchModel model)
        {
            return GetListDataAndReturn<FIN_TravelRequestModel, FIN_TravelRequestEntity, FIN_ApprovedTravelRequestSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ApprovedFIN_TravelRequest);
        }
        #endregion
        [HttpPost]
        public ActionResult GetProfileIDByTravelRequests(Guid TravelRequestID)
        {
            var service = new FIN_TravelRequestService();
            return Json(service.GetProfileIDByTravelRequests(TravelRequestID), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetClaimList([DataSourceRequest] DataSourceRequest request, FIN_ClaimSearchModel model)
        {
            return GetListDataAndReturn<FIN_ClaimModel, FIN_ClaimEntity, FIN_ClaimSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Claim);
        }

        public ActionResult ExportClaimByTemplate([DataSourceRequest] DataSourceRequest request, FIN_ClaimSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new FIN_ClaimItemModel();
            var lstModel = new List<FIN_ClaimItemModel>();
            request.Page = 1;
            request.PageSize = int.MaxValue - 1;
            var result = GetListData<FIN_ClaimModel, FIN_ClaimEntity, FIN_ClaimSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Claim, ref status);
            Guid[] Ids = null;

            if (!string.IsNullOrEmpty(model.ValueFields))
            {
                Ids = model.ValueFields.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            //string _ProfileName = "";
            //string _PositionName = "";
            if (result != null)
            {
                //_ProfileName = result[0].ProfileName;
                //_PositionName = result[0].PositionName;
                var claimEntity = new FIN_ClaimModel();
                if (Ids != null)
                {
                    claimEntity = result.Where(s => Ids.Contains(s.ID)).FirstOrDefault();
                }

                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();
                if (claimEntity != null)
                {
                    objs.Add(claimEntity.ID);
                    lstModel = actionService.GetData<FIN_ClaimItemModel>(objs, ConstantSql.hrm_hr_sp_get_ClaimItemByClaimID, ref status);
                }

            }
            //HeaderInfo headerInfo1 = new HeaderInfo() { Name = "ProfileName", Value = _ProfileName };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "PositionName", Value = _PositionName };
            //List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstModel;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "FIN_ClaimItemModel",
                    OutPutPath = path,
                    //HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
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
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult GetClaimCostPaymentApproveList([DataSourceRequest] DataSourceRequest request, FIN_ClaimCostPaymentApproveSearchModel model)
        {
            return GetListDataAndReturn<FIN_ClaimCostPaymentApproveModel, FIN_ClaimCostPaymentApproveEntity, FIN_ClaimCostPaymentApproveSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Claim);
        }


        [HttpPost]
        public ActionResult GetCashAdvanceList([DataSourceRequest] DataSourceRequest request, FIN_CashAdvanceSearchModel model)
        {
            return GetListDataAndReturn<FIN_CashAdvanceModel, Fin_CashAdvanceEntity, FIN_CashAdvanceSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CashAdvance);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportClaimList([DataSourceRequest] DataSourceRequest request, FIN_ClaimSearchModel model)
        {
            return ExportAllAndReturn<FIN_ClaimEntity, FIN_ClaimModel, FIN_ClaimSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_Claim);
        }
        [HttpPost]
        public ActionResult ExportCashAdvanceByTemplate([DataSourceRequest] DataSourceRequest request, FIN_CashAdvanceSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new FIN_CashAdvanceItemModel();
            var lstModel = new List<FIN_CashAdvanceItemModel>();
            request.Page = 1;
            request.PageSize = int.MaxValue - 1;
            Guid[] Ids = null;
            if (!string.IsNullOrEmpty(model.ValueFields))
            {
                Ids = model.ValueFields.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            var result = GetListData<FIN_CashAdvanceModel, Fin_CashAdvanceEntity, FIN_CashAdvanceSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CashAdvance, ref status);
            //string _ProfileName="";
            //string _PositionName="";
            if (result != null)
            {
                //_ProfileName = result[0].ProfileName;
                //_PositionName = result[0].PositionName;
                var cashEntity = new FIN_CashAdvanceModel();
                if (Ids != null)
                {
                    cashEntity = result.Where(s => Ids.Contains(s.ID)).FirstOrDefault();
                }
                var actionService = new ActionService(UserLogin);
                var objs = new List<object>();

                if (cashEntity != null)
                {
                    objs.Add(cashEntity.ID);

                    lstModel = actionService.GetData<FIN_CashAdvanceItemModel>(objs, ConstantSql.hrm_hr_sp_get_CashAdvanceItemByCashAdvanceID, ref status);
                }
                //objs.Add(Common.DotNetToOracle(CashAdvanceID.ToString()));

            }
            //HeaderInfo headerInfo1 = new HeaderInfo() { Name = "ProfileName", Value = _ProfileName };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "PositionName", Value = _PositionName };
            //List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = lstModel;
                isDataTable = false;
            }
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "FIN_CashAdvanceItemModel",
                    OutPutPath = path,
                    //HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
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
            return Json(result.ToDataSourceResult(request));
        }

        #region Get Profile Info by Id

        [HttpPost]
        public JsonResult GetProfileInfo(Guid profileId)
        {
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var profileInfo = service.GetProfileInfo(profileId, UserLogin);
            return Json(profileInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //kiem tra la leader hay ko

        [System.Web.Mvc.HttpPost]
        public ActionResult CheckLeader(Guid LeaderID, Guid ProfileID)
        {
            var service = new Hre_ProfileServices();
            bool rs = false;
            rs = service.CheckLeader(LeaderID, ProfileID);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMultiProfileByParameter(string ProfileIDs)
        {
            string Status = "";
            var actionService = new ActionService(UserLogin);
            var para = new List<object>();
            ProfileIDs = string.Join(",", ProfileIDs.Split(',').Select(x => Common.DotNetToOracle(x.ToString())).ToList());
            para.Add(ProfileIDs);
            var rs = actionService.GetData<Hre_ProfileIdEntity>(para, ConstantSql.hrm_hr_sp_Profile_MultiByPara, ref Status).Select(x => new { x.ID, x.ProfileName }).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult SaveHreCandidateGeneral(Hre_ProfileTemp model)
        //{
        //    if (model != null && model.TempProfileIDs != null)
        //    {
        //        string status = string.Empty;
        //        var salaryRankServices = new Cat_SalaryRankServices();
        //        var lstObjRank = new List<object>();
        //        lstObjRank.Add(null);
        //        lstObjRank.Add(null);
        //        lstObjRank.Add(1);
        //        lstObjRank.Add(int.MaxValue - 1);
        //        var lstRank = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();
        //        var rankEntity = lstRank.Where(s => s.ID == model.SalaryRankID).FirstOrDefault();

        //        var salaryClassServices = new Cat_SalaryClassServices();
        //        var lstObjClass = new List<object>();
        //        lstObjClass.Add(null);
        //        lstObjClass.Add(1);
        //        lstObjClass.Add(int.MaxValue - 1);
        //        var salaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList();
        //        var salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();
        //        Hre_CandidateGeneralServices services = new Hre_CandidateGeneralServices();
        //        List<Hre_CandidateGeneralEntity> lstCandidateGeneral = new List<Hre_CandidateGeneralEntity>();

        //        var service = new BaseService();
        //        var lstprofile = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.TempProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();

        //        foreach (var profile in lstprofile)
        //        {
        //            Hre_CandidateGeneralEntity candidateGeneral = new Hre_CandidateGeneralEntity();
        //            candidateGeneral.ProfileID = profile.ID;
        //            if (model.BasicSalary != null)
        //            {
        //                candidateGeneral.BasicSalary = double.Parse(model.BasicSalary);
        //            }
        //            candidateGeneral.RankRateID = model.SalaryRankID;
        //            if (salaryClassEntity != null)
        //            {
        //                candidateGeneral.SalaryClassID = salaryClassEntity.ID;
        //            }
        //            candidateGeneral.ContractTypeID = model.ContractTypeID;
        //            candidateGeneral.EnteringDate = model.DateHire;
        //            candidateGeneral.OrgStructureID = model.OrgStructureID;
        //            candidateGeneral.GradeAttendanceID = model.GradeAttendanceID;
        //            candidateGeneral.GradePayrollID = model.GradePayrollID;
        //            candidateGeneral.WorkPlaceID = model.WorkPlaceID;
        //            lstCandidateGeneral.Add(candidateGeneral);
        //        }

        //        model.ActionStatus = services.Add(lstCandidateGeneral);
        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult GetCandidateGeneralList([DataSourceRequest] DataSourceRequest request, Hre_CandidateGeneralSearchModel model)
        {
            return GetListDataAndReturn<Hre_CandidateGeneralModel, Hre_CandidateGeneralEntity, Hre_CandidateGeneralSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_CandidateGeneral);
        }



        public string CheckDuplicateHDTJob(Guid ProfileID, DateTime DateFrom)
        {
            // Kiểm tra có check trùng dữ liệu hay không
            string result = null;
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var hdtJobByProfile = actionService.GetData<Hre_HDTJobEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_HDTJobsByProfileId, ref status).OrderByDescending(s => s.DateTo).FirstOrDefault();
            if (hdtJobByProfile != null && hdtJobByProfile.DateTo > DateFrom)
            {
                result = "Nhân Viên : (" + hdtJobByProfile.ProfileName + ") Đã Đăng Ký Công Việc Nặng Nhọc Từ Ngày: "
                    + (hdtJobByProfile.DateFrom != null ? hdtJobByProfile.DateFrom.Value.ToString("dd/MM/yyyy") : string.Empty)
                    + " Đến Ngày: " + (hdtJobByProfile.DateTo != null ? hdtJobByProfile.DateTo.Value.ToString("dd/MM/yyyy") : string.Empty);
            }
            return result;
        }

        [HttpPost]
        public ActionResult PassingRecDetail(string selectedIds)
        {
            var service = new Rec_InterviewCampaignDetailServices();
            var message = service.ActionPassing(selectedIds, UserLogin);
            return Json(message);
        }

        #region Báo cáo Kỷ Luật

        public ActionResult GetReportSummaryDisciplineValidate(Hre_ReportSummaryDisciplineModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportSummaryDisciplineModel>(model, "Hre_ReportSummaryDiscipline", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);

        }
        [HttpPost]
        public ActionResult GetReportSummaryDiscipline([DataSourceRequest] DataSourceRequest request, Hre_ReportSummaryDisciplineModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model.DateFrom == null)
            {
                model.DateFrom = DateTime.Now.Date;
            }
            if (model.DateTo == null)
            {
                model.DateTo = DateTime.Now.AddDays(1).AddMilliseconds(-1);
            }

            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            var isDataTable = false;
            object obj = new Hre_ReportSummaryDisciplineModel();


            string status = string.Empty;
            var result = service.GetReportSummaryDiscipline(model.DateFrom.Value, model.DateTo.Value, model.OrgStructureID, model.IsCreateTemplate, UserLogin);

            if (model.IsCreateTemplateForDynamicGrid)
            {

                result.Columns.RemoveAt(2);
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
                    FileName = "Hre_ReportSummaryDisciplineModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult GetReportRelatives([DataSourceRequest] DataSourceRequest request, Hre_ReportRelativesModel Model)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportRelativesModel(),
                    FileName = "Hre_ReportRelatives",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportRelativesModel>(Model, "Hre_ReportRelatives", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            List<object> listObj = new List<object>();
            listObj.Add(Model.ProfileName);
            listObj.Add(Model.CodeEmp);
            listObj.Add(Model.PositionID);
            listObj.Add(Model.JobTitleID);
            listObj.Add(Model.OrgStructureIDs);
            listObj.Add(Model.RelativeName);
            listObj.Add(Model.RelativeTypeID);
            listObj.Add(Model.RankID);
            listObj.Add(Model.WorkPlaceID);
            listObj.Add(Model.RelativesOfBirth);
            var result = actionService.GetData<Hre_ReportRelativesEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptRelatives, ref status).Translate<Hre_ReportRelativesModel>();

            if (Model.ExportID != Guid.Empty)
            {
                //var fullPath = ExportService.Export(Model.ExportID, result, Model.ExportType);
                var fullPath = ExportService.Export(Model.ExportID, result, null, Model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetReportKaizenDetail([DataSourceRequest] DataSourceRequest request, Kai_ReportKaizenDetailModel Model)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Kai_ReportKaizenDetailModel();

            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Kai_ReportKaizenDetailModel>(Model, "Kai_ReportKaizenDetail", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = new DateTime(Model.DateFrom.Value.Year, Model.DateFrom.Value.Month, 1).AddMonths(1).AddDays(-1) };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(Model.DateFrom);
            listObj.Add(new DateTime(Model.DateFrom.Value.Year, Model.DateFrom.Value.Month, 1).AddMonths(1).AddDays(-1));
            listObj.Add(1);
            listObj.Add(Int32.MaxValue - 1);
            var result = actionService.GetData<Kai_ReportKaizenDetailEntity>(listObj, ConstantSql.hrm_sal_sp_get_RptKaizenDataDetail, ref status).Translate<Kai_ReportKaizenDetailModel>();
            if (result != null)
                result = result.Where(s => (s.IsPaymentOut == false || s.IsPaymentOut == null)).ToList();
            if (Model != null && Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Kai_ReportKaizenDetailModel(),
                    FileName = "Kai_ReportKaizenDetail",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetReportKaizenMonthly([DataSourceRequest] DataSourceRequest request, Kai_ReportKaizenDetailModel Model)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var isDataTable = false;
            object obj = new Kai_ReportKaizenDetailModel();

            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Kai_ReportKaizenDetailModel>(Model, "Kai_ReportCTKaizenDataMonthly", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            #endregion
            List<object> listObj = new List<object>();
            listObj.Add(Model.OrgStructureID);
            listObj.Add(null);
            listObj.Add(null);
            listObj.Add(null);
            listObj.Add(null);
            var result = actionService.GetData<Kai_ReportKaizenDetailEntity>(listObj, ConstantSql.hrm_sal_sp_get_RptKaizenDataDetail, ref status).Where(s => s.MonthReport != null && s.YearReport != null && s.MonthReport == Model.DateFrom.Value.Month && s.YearReport == Model.DateFrom.Value.Year && (s.IsPaymentOut == false || s.IsPaymentOut == null)).ToList().Translate<Kai_ReportKaizenDetailModel>();

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (Model != null && Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Kai_ReportKaizenDetailModel(),
                    FileName = "Kai_ReportKaizenDetail",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetReportKaizenAccumulate([DataSourceRequest] DataSourceRequest request, Kai_ReportKaizenDetailModel model)
        {
            string status = string.Empty;
            var service = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);
            #region Validate
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Kai_ReportKaizenDetailModel>(model, "Kai_ReportKaizenDetail", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }

            #endregion
            List<object> listObj = new List<object>();
            listObj.Add(model.OrgStructureID);
            listObj.Add(model.DateFrom);
            listObj.Add(model.DateTo);
            List<Kai_ReportKaizenDetailEntity> lstKaizenDetailEntity = actionService.GetData<Kai_ReportKaizenDetailEntity>(listObj, ConstantSql.hrm_sal_sp_get_RptKaizenDataAccumulate, ref status);
            var result = service.GetReportKaizenAccumulate(lstKaizenDetailEntity, model.DateFrom, model.DateTo, model.RateDetail, model.IsCreateTemplate);
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
                    FileName = "Kai_ReportKaizenDetailEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable,
                    HeaderInfo = listHeaderInfo,
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult GetReportPayHDTJob([DataSourceRequest] DataSourceRequest request, Hre_ReportPayHDTJobModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportPayHDTJobModel(),
                    FileName = "Hre_ReportPayHDTJob",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportPayHDTJobModel>(Model, "Hre_ReportPayHDTJob", ref message);
            if (!checkValidate)
            {
                // var ls = new object[] { "error", message };
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetPayHDTJob(Model.DateFrom, Model.DateTo, Model.OrgStructureIDs, UserLogin).Translate<Hre_ReportPayHDTJobModel>();

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetReportProfileHDTInMonth([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileHDTInMonthModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileHDTInMonthModel>(Model, "Hre_ReportProfileHDTInMonth", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }

            if (Model.Month == null)
            {
                Model.Month = DateTime.Now;
            }
            #endregion

            var ReportServices = new Hre_ReportServices();
            List<string> lstUnit = new List<string>();
            List<string> lstDept = new List<string>();
            List<string> lstPart = new List<string>();
            if (Model.Part != null)
            {
                lstPart = Model.Part.Split(',').ToList();
            }
            if (Model.Unit != null)
            {
                lstUnit = Model.Unit.Split(',').ToList();
            }
            if (Model.Dept != null)
            {
                lstDept = Model.Dept.Split(',').ToList();
            }

            var result = ReportServices.GetReportProfileHDTInMonth(Model.Month.Value, Model.OrgStructureIDs, lstUnit, lstDept, lstPart, Model.IsCreateTemplate, UserLogin);
            object obj = new DataTable();
            bool isDataTable = false;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            HeaderInfo headerInfo = new HeaderInfo() { Name = "Month", Value = Model.Month };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportProfileHDTInMonth",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }



            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetReportProfileHDTNotWork([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileHDTNotWorkModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportProfileHDTNotWorkModel>(model, "Hre_ReportProfileHDTNotWork", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var ReportServices = new Hre_ReportServices();
            string ReportName = "Hre_ReportProfileHDTNotWork";
            var Table = ReportServices.GetReportProfileHDTNotWork(model.DateFrom, model.DateTo, model.OrgStructureIDs, ReportName, model.IsCreateTemplate, UserLogin);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = ReportName,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion


        // Son.Vo - kiểm tra thông tin nv khi click nút nhận việc
        [HttpPost]
        public string CheckDataActionHire(string lstProfileID)
        {
            string returnValue = null;
            string status = string.Empty;
            var profileID = Guid.Empty;
            var actionService = new ActionService(UserLogin);
            var profileServices = new Hre_ProfileServices();
            var lstprofile = actionService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(lstProfileID), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
            var strvalidate = profileServices.StrValidateActionHire();

            if (!string.IsNullOrEmpty(strvalidate))
            {
                string[] valueField = strvalidate.Split(',');
                foreach (var profile in lstprofile)
                {
                    foreach (var value in valueField)
                    {
                        if (profile.GetPropertyValue(value) == null)
                        {
                            returnValue += profile.CodeCandidate + ",";
                        }
                    }
                }
            }
            else
            {
                foreach (var profile in lstprofile)
                {
                    if (profile.ProfileName == null || profile.DateHire == null || profile.IDNo == null || profile.Gender == null || profile.YearOfBirth == null
                        || profile.PAddress == null || profile.WorkPlaceID == null || profile.Cellphone == null || profile.OrgStructureID == null
                        || profile.ContractTypeID == null || profile.SalaryClassID == null)
                    {
                        returnValue += profile.CodeCandidate + ",";
                    }
                }
            }
            if (returnValue != null)
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 1);
            }
            return returnValue;
        }

        #region BC NV Cho Nghi Viec
        public ActionResult GetReportProfileIsWaitingStopWorkingList([DataSourceRequest] DataSourceRequest request, Hre_ReportProfileWaitingStopWorkingSearchModel model)
        {
            string status = string.Empty;
            //var isDataTable = false;
            var actionService = new ActionService(UserLogin);
            //object obj = new Hre_ProfileModel();

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            if ((model.IsCreateTemplate == true && model.IsCreateTemplateForDynamicGrid == true) || model.ExportId != Guid.Empty)
            {
                lstModel = new ListQueryModel
                {
                    PageSize = int.MaxValue - 1,
                    PageIndex = 1,
                    Filters = ExtractFilterAttributes(request),
                    Sorts = ExtractSortAttributes(request),
                    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
                };
            }

            var listProfile = actionService.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_ProfileWaitingStopWoking, ref status);
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportProfileWaitingStopWorking(listProfile);
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
                    FileName = "Hre_ReportProfileWaitingStopWorkingEntity",
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
                var fullPath = string.Empty;
                if (headerInfo1.Value == null || headerInfo2.Value == null)
                {
                    fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                }
                else
                {
                    fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                }
                return Json(fullPath);
            }
            //return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
            #region mapping dataTable to dataList
            List<Hre_ReportProfileWaitingStopWorkingModel> dataList = new List<Hre_ReportProfileWaitingStopWorkingModel>();
            Hre_ReportProfileWaitingStopWorkingModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Hre_ReportProfileWaitingStopWorkingModel).GetProperties(flags)
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
                    aTSource = new Hre_ReportProfileWaitingStopWorkingModel();
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

        #region HieuVan - Báo cáo nhân viên nghỉ việc
        public ActionResult GetReportProfileQuitV2([DataSourceRequest] DataSourceRequest request, Hre_ReportGeneralSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportGeneralSearchModel>(model, "Hre_ReportProfileQuitV2", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Hre_ReportServices Services = new Hre_ReportServices();
            string NameTable = "ReportProfileQuitV2";
            List<Guid> listOrgID = new List<Guid>();

            if (model.OrgStructureID != null)
            {
                listOrgID = model.OrgStructureID.StringToList();
            }

            DataTable Table = Services.GetReportProfileQuitV2(listOrgID, (DateTime)model.DateStart,
                (DateTime)model.DateEnd, model.CodeEmp, model.ProfileName, model.ResignReasonID, model.TypeOfStopID, model.JobTitleID, model.PositionID, model.WorkPlaceID, model.IsCreateTemplate, NameTable, UserLogin);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region HieuVan - Báo cáo thông tin nhân viên
        public ActionResult GetReportProfileInformation([DataSourceRequest] DataSourceRequest request, Hre_ReportGeneralSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportGeneralSearchModel>(model, "Hre_ReportGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Hre_ReportServices Services = new Hre_ReportServices();
            string NameTable = "ReportProfileInformation";
            List<Guid> listOrgID = new List<Guid>();

            if (model.OrgStructureID != null)
            {
                listOrgID = model.OrgStructureID.StringToList();
            }

            DataTable Table = Services.GetReportProfileInformation(listOrgID, (DateTime)model.DateStart,
                (DateTime)model.DateEnd, model.CodeEmp, model.ProfileName, model.IsCreateTemplate, NameTable, UserLogin);



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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region HieuVan - Báo cáo chi tiết hợp đồng lao động
        public ActionResult GetReportContractDetail([DataSourceRequest] DataSourceRequest request, Hre_ReportContractExpiredSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportContractExpiredSearchModel>(model, "Hre_ReportGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var Services = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);
            List<Guid> listOrgID = new List<Guid>();
            string NameTable = "ReportContractDetail";

            if (model.OrgStructureID != null)
            {
                listOrgID = model.OrgStructureID.StringToList();
            }

            //DataTable Table = Services.GetReportContractDetail(listOrgID, (DateTime)model.DateStart,
            //    (DateTime)model.DateEnd, model.CodeEmp, model.ProfileName, model.IsCreateTemplate, NameTable);
            string status = string.Empty;
            List<Hre_ProfileEntity> lstProfileForRptContractEXP = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                List<object> listObj = new List<object>();
                listObj.AddRange(new object[14]);
                listObj[0] = model.ProfileName;
                listObj[1] = model.CodeEmp;
                listObj[2] = model.OrgStructureID;
                listObj[3] = model.JobTitleID;
                listObj[4] = model.PositionID;
                listObj[5] = model.DateHireFrom;
                listObj[6] = model.DateHireTo;
                listObj[7] = model.EmpTypeID;
                listObj[8] = model.Gender;
                listObj[9] = model.WorkPlaceID;
                listObj[10] = model.SalaryClassID;
                listObj[11] = model.IDNo;
                listObj[12] = 1;
                listObj[13] = int.MaxValue - 1;
                lstProfileForRptContractEXP = actionService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileForRptContractExpired, ref status);
            }
            DataTable Table = Services.GetReportContractDetail(lstProfileForRptContractEXP, model.DateSignedStart, model.DateSignedEnd, model.ContractNo, model.ContractTypeID, model.IsCreateTemplate, NameTable);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateHireFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateHireTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
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
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region HieuVan - Báo cáo hợp đồng hết hạn
        public ActionResult GetReportContractExpired([DataSourceRequest] DataSourceRequest request, Hre_ReportContractExpiredSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportContractExpiredSearchModel>(model, "Hre_ReportGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var Services = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);

            #region Getdata

            //List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            //if (!model.IsCreateTemplate)
            //{
            //    //Lọc theo phòng ban
            //    List<object> listObj = new List<object>();
            //    listObj.Add(model.OrgStructureID);
            //    listObj.Add(model.ProfileName);
            //    listObj.Add(model.CodeEmp);
            //    listProfileByOrg = Services.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            //}
            #endregion
            #region lay nhan vien theo dieu kien loc
            string status = string.Empty;
            List<Hre_ProfileEntity> lstProfileForRptContractEXP = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                List<object> listObj = new List<object>();
                listObj.AddRange(new object[14]);
                listObj[0] = model.ProfileName;
                listObj[1] = model.CodeEmp;
                listObj[2] = model.OrgStructureID;
                listObj[3] = model.JobTitleID;
                listObj[4] = model.PositionID;
                listObj[5] = model.DateHireFrom;
                listObj[6] = model.DateHireTo;
                listObj[7] = model.EmpTypeID;
                listObj[8] = model.Gender;
                listObj[9] = model.WorkPlaceID;
                listObj[10] = model.SalaryClassID;
                listObj[11] = model.IDNo;
                listObj[12] = 1;
                listObj[13] = int.MaxValue - 1;
                lstProfileForRptContractEXP = actionService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileForRptContractExpired, ref status);
            }
            #endregion



            string NameTable = "ReportContractExpired";
            DataTable Table = Services.GetReportContractExpired(lstProfileForRptContractEXP, model.DateSignedStart, model.DateSignedEnd, model.ContractNo, model.ContractTypeID, model.IsCreateTemplate, NameTable);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateHireFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateHireTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
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
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region HieuVan - Báo cáo hợp đồng hiện tại
        public ActionResult GetReportContractCurrent([DataSourceRequest] DataSourceRequest request, Hre_ReportGeneralSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportGeneralSearchModel>(model, "Hre_ReportGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var Services = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);

            #region Getdata
            string status = string.Empty;
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureID);
                listObj.Add(model.ProfileName);
                listObj.Add(model.CodeEmp);
                listProfileByOrg = actionService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }
            #endregion

            string NameTable = "ReportContractCurrent";
            DataTable Table = Services.GetReportContractCurrent(listProfileByOrg, (DateTime)model.DateStart, (DateTime)model.DateEnd, model.IsCreateTemplate, NameTable);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region HieuVan - Hỗ trợ báo cáo thông tin nhân viên tại 1 thời điểm
        public ActionResult GetReportProfileInformationMoment([DataSourceRequest] DataSourceRequest request, Hre_ReportGeneralSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            //Do BC chỉ sd tìm kiếm 1 ngày
            model.DateEnd = DateTime.Now;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportGeneralSearchModel>(model, "Hre_ReportGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var Services = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);

            #region Getdata
            string status = string.Empty;
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureID);
                listObj.Add(model.ProfileName);
                listObj.Add(model.CodeEmp);
                listProfileByOrg = actionService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }
            #endregion

            string NameTable = "ReportProfileInformationMoment";
            DataTable Table = Services.GetReportProfileInformationMoment(listProfileByOrg, (DateTime)model.DateStart, model.IsCreateTemplate, NameTable, model.WorkPlaceID, model.SalaryClassID);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = model.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = NameTable,
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, Table, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            #endregion

            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        [HttpPost]
        public ActionResult GetReportRecieveObjectByTime([DataSourceRequest] DataSourceRequest request, Hre_ReportRecieveObjectByTimeModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportRecieveObjectByTimeModel>(Model, "Hre_ReportRecieveObjectByTime", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom == null ? DateTime.Now : Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo == null ? DateTime.Now : Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model.DateFrom == DateTime.MinValue)
            {
                Model.DateFrom = DateTime.Now;
            }
            if (Model.DateTo == DateTime.MaxValue)
            {
                Model.DateTo = DateTime.Now;
            }
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportRecieveObjectByTime(Model.OrgStructureID, Model.DateFrom, Model.DateTo, Model.IsCreateTemplate, UserLogin);
            object obj = new DataTable();
            bool isDataTable = false;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Hre_ReportRecieveObjectByTime",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        public string ValidateHDTJob(Guid? ProfileID, DateTime? DateFrom, DateTime? DateTo)
        {
            string messageValidate = string.Empty;
            var baseService = new BaseService();
            string status = string.Empty;
            var lstHDTjobs = baseService.GetData<Hre_HDTJobEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_HDTJobsByProfileId, UserLogin, ref status).Where(s => s.DateFrom != DateFrom).ToList();
            foreach (var item in lstHDTjobs)
            {
                if (DateFrom <= item.DateFrom && item.DateFrom <= DateTo && DateFrom != item.DateFrom && item.DateTo != DateTo)
                {
                    messageValidate = ConstantDisplay.HRM_EmpRegisteredOnThisTime.TranslateString();
                }
            }
            return messageValidate;
        }

        [HttpPost]
        public ActionResult GetReportSumaryHDTProfile([DataSourceRequest] DataSourceRequest request, Hre_ReportSumaryHDTProfileModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = Model.DateFrom ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = Model.DateTo ?? DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportSumaryHDTProfileModel(),
                    FileName = "Hre_ReportSumaryHDTProfile",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportSumaryHDTProfileModel>(Model, "Hre_ReportSumaryHDTProfile", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportSumaryHDTProfile(
                Model.DateFrom,
                Model.DateTo,
                Model.HDTJobGroupID,
                Model.CodeEmp, UserLogin).Translate<Hre_ReportSumaryHDTProfileModel>();

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        #region Hre_ReportSummaryDependant
        public ActionResult GetReportSummaryDependantList([DataSourceRequest] DataSourceRequest request, Hre_ReportSummaryDependantModel model)
        {
            string status = string.Empty;
            var service = new BaseService();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "MonthOfExpiry", Value = model.MonthOfExpiry };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1 };
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };

            var ReportServices = new Hre_ReportServices();
            var actionService = new ActionService(UserLogin);
            List<object> paraDependant = new List<object>();
            paraDependant.AddRange(new object[11]);
            paraDependant[0] = model.ProfileName;
            paraDependant[1] = model.CodeEmp;
            paraDependant[2] = model.OrgStructureID;
            paraDependant[3] = model.JobTitleID;
            paraDependant[4] = model.PositionID;
            paraDependant[5] = model.DependantName;
            paraDependant[6] = model.RelationID;
            paraDependant[7] = null;
            paraDependant[8] = null;
            paraDependant[9] = 1;
            paraDependant[10] = int.MaxValue - 1;
            var lstDependant = actionService.GetData<Hre_DependantEntity>(paraDependant, ConstantSql.hrm_hr_sp_get_Dependant, ref status);

            if (model.MonthOfExpiry != null && lstDependant != null)
            {
                lstDependant = lstDependant.Where(s => (s.MonthOfEffect <= model.MonthOfExpiry && model.MonthOfExpiry <= s.MonthOfExpiry)
                    || (s.MonthOfEffect <= model.MonthOfExpiry && s.MonthOfExpiry == null)).ToList();
            }

            var result = ReportServices.GetReportSummaryDependant(lstDependant, model.IsCreateTemplate);

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
                    FileName = "Hre_ReportSummaryDependantEntity",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportSummaryDependantModel>(model, "Hre_ReportSummaryDependant", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion

            if (model.ExportId != Guid.Empty)
            {
                string fullPath = "";
                if (model.MonthOfExpiry != null)
                {
                    fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                }
                else
                {
                    fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                }

                return Json(fullPath);
            }
            #region mapping dataTable to dataList
            List<Hre_ReportSummaryDependantModel> dataList = new List<Hre_ReportSummaryDependantModel>();
            Hre_ReportSummaryDependantModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Hre_ReportSummaryDependantModel).GetProperties(flags)
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
                    aTSource = new Hre_ReportSummaryDependantModel();
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


        [HttpPost]
        public ActionResult GetReportHDTJobNotDateEnd([DataSourceRequest] DataSourceRequest request, Hre_ReportHDTJobNotDateEndModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom == null ? DateTime.Now : Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo == null ? DateTime.Now : Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportHDTJobNotDateEndModel(),
                    FileName = "Hre_ReportHDTJobNotDateEnd",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHDTJobNotDateEndModel>(Model, "Hre_ReportHDTJobNotDateEnd", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportHDTJobNotDateEnd(
                Model.DateFrom,
                Model.DateTo,
                Model.OrgStructureIDs, UserLogin).Translate<Hre_ReportHDTJobNotDateEndModel>();

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetDelegateApproveList([DataSourceRequest] DataSourceRequest request, Sys_DelegateApproveSearchModel Sys_GroupSearchModel)
        {
            return GetListDataAndReturn<Sys_DelegateApproveModel, Sys_DelegateApproveEntity, Sys_DelegateApproveSearchModel>(request, Sys_GroupSearchModel, ConstantSql.hrm_sys_sp_get_DelegateApprove);
        }

        [HttpPost]
        public ActionResult GetReportSumarySeniorHDTProfile([DataSourceRequest] DataSourceRequest request, Hre_ReportSumarySeniorHDTProfileModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportSumarySeniorHDTProfileModel>(Model, "Hre_ReportSumaryHDTProfile", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportSumarySeniorHDTProfile(Model.DateFrom, Model.DateTo, Model.ProfileName, Model.CodeEmp, Model.OrgStructureID, UserLogin).Translate<Hre_ReportSumarySeniorHDTProfileModel>();
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = Model.DateFrom ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = Model.DateTo ?? DateTime.Now };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "E_DEPARTMENT", Value = (Model.OrgStructureID != null && result != null) ? result.FirstOrDefault().E_DEPARTMENT : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportSumarySeniorHDTProfileModel(),
                    FileName = "Hre_ReportSumarySeniorHDTProfile",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetLstJobTitleByOrgStructureID(Guid orgID, string ShopFilter)
        {
            string status = string.Empty;


            var actionService = new ActionService(UserLogin);
            var lstObjJobTitle = new List<object>();
            lstObjJobTitle.Add(null);
            lstObjJobTitle.Add(null);
            lstObjJobTitle.Add(null);
            lstObjJobTitle.Add(1);
            lstObjJobTitle.Add(int.MaxValue - 1);
            var lstAlljobtitle = actionService.GetData<Cat_JobTitleEntity>(lstObjJobTitle, ConstantSql.hrm_cat_sp_get_JobTitle, ref status).ToList();
            if (orgID != Guid.Empty)
            {
                if (lstAlljobtitle != null)
                {
                    var lstAlljobtitleByOrg = lstAlljobtitle.Where(s => s.OrgStructureID == orgID).ToList();

                    return Json(lstAlljobtitleByOrg, JsonRequestBehavior.AllowGet);

                    //  return Json(lstAlljobtitle,JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }


        [HttpPost]
        public ActionResult GetLstCostCentreByOrgStructureID(Guid orgID, string CostCentreFilter)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var lstObjCostCentre = new List<object>();
            lstObjCostCentre.Add(null);
            lstObjCostCentre.Add(null);
            lstObjCostCentre.Add(1);
            lstObjCostCentre.Add(int.MaxValue - 1);
            var lstAllCostCentre = actionService.GetData<Cat_CostCentreEntity>(lstObjCostCentre, ConstantSql.hrm_cat_sp_get_CostCentre, ref status).ToList();
            if (orgID != Guid.Empty)
            {
                if (lstAllCostCentre != null)
                {
                    var lstAllCostCentreByOrg = lstAllCostCentre.Where(s => s.OrgStructureID == orgID).ToList();

                    return Json(lstAllCostCentreByOrg, JsonRequestBehavior.AllowGet);

                }
            }

            return null;
        }

        public ActionResult GetAppendixContractByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(profileID.ToString()));
            var result = actionService.GetData<Hre_AppendixContractEntity>(objs, ConstantSql.hrm_hr_sp_get_AppendixContractByProfileID, ref status);
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetDateSuspenseSearch()
        {
            string status = string.Empty;
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
            var valueDaySuspenseExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY);
            if (valueDaySuspenseExpiry >= 0)
            {
                DateFrom = DateTo.AddDays(-valueDaySuspenseExpiry);
            }
            return Json(DateFrom, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDateStopWorkingSearch()
        {
            string status = string.Empty;
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
            var valueDaySuspenseExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYSTOPWORKINGEXPIRY);
            if (valueDaySuspenseExpiry >= 0)
            {
                DateFrom = DateTo.AddDays(-valueDaySuspenseExpiry);
            }
            return Json(DateFrom, JsonRequestBehavior.AllowGet);
        }

        #region Báo cáo trường hợp bất thường HDT Job

        public ActionResult GetReportUnusualHDTValidate(Hre_ReportUnusualHDTModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportUnusualHDTModel>(model, "Hre_ReportUnusualHDT", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        public ActionResult GetReportUnusualHDT([DataSourceRequest] DataSourceRequest request, Hre_ReportUnusualHDTModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportUnusualHDTModel>(model, "Hre_ReportUnusualHDT", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            object obj = new Hre_ReportUnusualHDTModel();
            var isDataTable = false;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };



            var result = service.GetReportUnusualHDT(
                model.DateFrom,
                model.DateTo,
                model.OrgStructureIDs,
                model.IsCreateTemplate, UserLogin);

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
                    FileName = "Hre_ReportUnusualHDTModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                result.Rows[0].Delete();
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        [HttpPost]
        public ActionResult UpdateSettlementProfiles(List<Guid> ProfileIds, int Settlement, DateTime? DateSettlement)
        {
            Hre_ProfileServices Services = new Hre_ProfileServices();
            bool result = Services.UpdateSettlementProfiles(ProfileIds, Settlement, DateSettlement);
            if (result)
            {
                return Json(new { success = true, mess = ConstantDisplay.Hrm_Succeed.TranslateString() });
            }
            else
            {
                return Json(new { success = false, mess = ConstantDisplay.Hrm_Error.TranslateString() });
            }

        }

        #region Hre_ReportSummaryDependantDeduction
        public ActionResult GetReportSummaryDependantDeductionValidate(Hre_ReportSummaryDependantDeductionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportSummaryDependantDeductionModel>(model, "Hre_ReportSummaryDependantDeduction", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        [HttpPost]
        public ActionResult GetReportSummaryDependantDeduction([DataSourceRequest] DataSourceRequest request, Hre_ReportSummaryDependantDeductionModel model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "MonthFrom", Value = model.MonthFrom ?? DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "MonthTo", Value = model.MonthTo ?? DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportSummaryDependantDeductionModel(),
                    FileName = "Hre_ReportSummaryDependantDeductionModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportSummaryDependantDeductionModel>(model, "Hre_ReportSummaryDependantDeduction", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            List<object> paraDependant = new List<object>();
            paraDependant.AddRange(new object[11]);
            paraDependant[9] = 1;
            paraDependant[10] = int.MaxValue;
            var lstDependant = actionService.GetData<Hre_DependantEntity>(paraDependant, ConstantSql.hrm_hr_sp_get_Dependant, ref status);
            var ReportServices = new Hre_ReportServices();

            if (model.MonthFrom != null && model.MonthTo != null && lstDependant != null)
            {
                lstDependant = lstDependant.Where(s =>
                    ((model.MonthFrom <= s.MonthOfExpiry && s.MonthOfExpiry <= model.MonthTo) || (s.MonthOfExpiry == null))
                    && (s.MonthOfEffect <= model.MonthTo)).ToList();
            }
            var result = ReportServices.GetReportSummaryDependantDeduction(lstDependant, model.IsCreateTemplate);

            if (model.ExportId != Guid.Empty)
            {
                if (model.MonthFrom != null && model.MonthTo != null)
                {
                    var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                    return Json(fullPath);
                }
                else
                {
                    var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                    return Json(fullPath);
                }
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        [HttpPost]
        public ActionResult BackToWaiting(string selectedIds)
        {
            var service = new Hre_ProfileServices();
            var message = service.BackToWaiting(selectedIds);
            return Json(message);
        }

        [HttpPost]
        // Son.Vo - Màn hình cập nhật hồ sơ ứng viên có nhập theo Rank detail hay nhập bằng tay.
        public ActionResult IsInputGeneralCandidateManual()
        {
            var profileServices = new Hre_ProfileServices();
            Boolean ischeck = profileServices.IsInputGeneralCandidateManual();
            if (ischeck == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // Son.Vo - Màn hình tạo mới ứng viên có hiển thị field mã NV KH hay không.
        public ActionResult IsUseCodeEmpOfCustomer()
        {
            var profileServices = new Hre_ProfileServices();
            Boolean ischeck = profileServices.IsUseCodeEmpOfCustomer();
            if (ischeck == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        // Son.Vo - kiểm tra có thông báo khi nhập nv vượt quá số lượng của phòng ban hay không
        [HttpPost]
        public bool IsAlertIfNumberOfEmpExceedPlan(Guid? OrgStructureID)
        {
            var profileServices = new Hre_ProfileServices();
            Boolean ischeck = profileServices.IsAlertIfNumberOfEmpExceedPlan();
            bool result = false;
            if (ischeck == false || OrgStructureID == null)
            {
                return false;
            }
            string status = string.Empty;
            var baseService = new BaseService();
            var CountProfileByOrgStructureID = baseService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(OrgStructureID.ToString()), ConstantSql.hrm_hr_sp_get_ProfilebyOrgStructureID, UserLogin, ref status).Count;
            var CountProfileByPlanHeadCount = baseService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(OrgStructureID.ToString()), ConstantSql.hrm_hr_sp_get_PlanHeadCountbyOrgStructureID, UserLogin, ref status).Count;

            if (CountProfileByOrgStructureID >= CountProfileByPlanHeadCount && CountProfileByPlanHeadCount > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public ActionResult GetOrgMoreInforContractByOrgID([DataSourceRequest] DataSourceRequest request, Guid? OrgStructureID)
        {
            if (OrgStructureID != null && OrgStructureID != Guid.Empty)
            {
                string status = string.Empty;
                var baseService = new BaseService();
                var result = baseService.GetData<Cat_OrgMoreInforContractEntity>(Common.DotNetToOracle(OrgStructureID.ToString()), ConstantSql.hrm_cat_sp_get_OrgMoreInforContractByOrgID, UserLogin, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return null;
        }
        #region Người Duyệt (ECLAIM)

        public ActionResult GetApproverOfProfileList([DataSourceRequest] DataSourceRequest request, FIN_ApproverECLAIMSearchModel model)
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
            return GetListDataAndReturn<FIN_ApproverECLAIMModel, FIN_ApproverECLAIMEntity, FIN_ApproverECLAIMSearchModel>(request, model, ConstantSql.hrm_fin_sp_get_ApproverECLAIM);
        }

        public ActionResult GetApproverList([DataSourceRequest] DataSourceRequest request, FIN_ApproverECLAIMSearchModel model)
        {
            if (model != null)
            {
                model.ProfileID = Common.DotNetToOracle(model.ProfileID);
            }

            //#region lay ds approver hiển thị trên 1 dòng ứng với profile

            var actionService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = 1,
                PageSize = int.MaxValue - 1,//request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;
            var listEntity = actionService.GetData<FIN_ApproverECLAIMEntity>(lstModel, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, ref status).ToList();
            if (model.OrderNo.HasValue)
            {
                listEntity = listEntity.Where(s => s.OrderNo != null && s.OrderNo.Value == model.OrderNo.Value).ToList();
            }
            if (listEntity != null)
            {
                // request.Page = 1;
                //var listModel = listEntity.Translate<FIN_ApproverECLAIMModel>();
                var listModel = listEntity;
                var profileIds = listModel.GroupBy(p => new { p.ProfileID, p.ApprovedType }).ToList();
                var approverReturn = new List<FIN_ApproverECLAIMModel>();
                var approverNames = string.Empty;
                foreach (var profileId in profileIds)
                {
                    var approvers = listModel.Where(p => p.ProfileID == profileId.Key.ProfileID && p.ApprovedType == profileId.Key.ApprovedType).OrderBy(p => p.OrderNo).ToList();
                    var approver = approvers.FirstOrDefault();
                    approverNames = string.Empty;
                    foreach (var finApproverModel in approvers)
                    {
                        approverNames += finApproverModel.OrderNo + " - " + finApproverModel.ApprovedName + " , ";
                    }
                    var fin = new FIN_ApproverECLAIMModel();
                    if (approver != null)
                    {
                        fin.ID = approver.ID;
                        fin.ProfileName = approver.ProfileName;
                        fin.ApprovedType = approver.ApprovedType;
                        fin.ApprovedTypeView = approver.ApprovedTypeView;
                        fin.ProfileID = profileId.Key.ProfileID;
                    }
                    fin.ApprovedName = approverNames.Substring(0, approverNames.Length - 2);
                    approverReturn.Add(fin);
                }


                var dataSourceResult = approverReturn.ToDataSourceResult(request);
                dataSourceResult.Total = approverReturn.Count();
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new List<FIN_ApproverECLAIMModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteApproverByProfile(string id)
        {
            var service1 = new FIN_ApproverECLAIMService();
            var result = service1.DeleteListApprover(id);
            var result1 = result.CopyData<FIN_ApproverECLAIMModel>();
            return Json(result1);
        }
        #endregion

        [HttpPost]
        public ActionResult ApprovedContractAll([DataSourceRequest] DataSourceRequest request, Hre_ContractSearchModel model)
        {
            return GetListDataAndReturn<Hre_ContractModel, Hre_ContractEntity, Hre_ContractSearchModel>(request, model, ConstantSql.hrm_hr_sp_set_ApprovedAllContract);
        }


        public ActionResult ActionSubmitTravelRequestItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_TravelRequestService();
            services.ActionSubmit(selectedIds, status, userId);
            return Json("");
        }



        public ActionResult ActionApprovedTravelRequestItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_TravelRequestService();
            services.ActionApproved(selectedIds, status, userId);
            return Json("");

        }


        public ActionResult ActionSendMailTravelRequest(List<Guid> selectedIds, Guid userId, string host)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_TravelRequestService();
            var status = services.ProcessSendMailForFirstApprove(host, userId, selectedIds[0]);
            return Json("");
        }

        public ActionResult ActionSendMailClaim(List<Guid> selectedIds, Guid userId, string host)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_ClaimService();
            var status = services.ProcessSendMailForFirstApprove(host, userId, selectedIds[0]);
            return Json("");
        }


        public ActionResult ActionSendMailCashAdvance(List<Guid> selectedIds, Guid userId, string host)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_CashAdvanceService();
            var status = services.ProcessSendMailForFirstApprove(host, userId, selectedIds[0]);
            return Json("");
        }


        public ActionResult ActionApprovedTravelRequestItemAll(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_TravelRequestService();
            services.ActionApprovedAll(selectedIds, status, userId);
            return Json("");
        }
        public ActionResult ActionApprovedClaimItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_ClaimItemService();
            services.ActionApproved(selectedIds, status, userId);
            return Json("");
        }

        public ActionResult ActionApprovedClaimItemAll(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_ClaimItemService();
            services.ActionApprovedAll(selectedIds, status, userId);
            return Json("");
        }

        public ActionResult ActionSubmitClaimItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_ClaimItemService();
            services.ActionSubmit(selectedIds, status, userId);
            return Json("");
        }

        public ActionResult ActionApprovedCashAdvanceItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_CashAdvanceService();
            services.ActionApproved(selectedIds, status, userId);
            return Json("");
        }
        public ActionResult ActionApprovedCashAdvanceItemAll(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_CashAdvanceService();
            services.ActionApprovedAll(selectedIds, status, userId);
            return Json("");
        }

        public ActionResult ActionSubmitCashAdvanceItem(List<Guid> selectedIds, string status, Guid userId)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new FIN_CashAdvanceService();
            services.ActionSubmit(selectedIds, status, userId);
            return Json("");
        }

        [HttpPost]
        public ActionResult GetLstRankByRankDetail(Guid? RankDetailID)
        {
            if (RankDetailID != Guid.Empty && RankDetailID != null)
            {
                string status = string.Empty;
                var actionService = new ActionService(UserLogin);
                var rankdetail = actionService.GetData<Cat_SalaryRankEntity>(Common.DotNetToOracle(RankDetailID.ToString()), ConstantSql.hrm_cat_sp_get_SalaryRankById, ref status).Select(s => s.SalaryClassID).ToList();
                if (rankdetail != null)
                {
                    var lstObjRank = new List<object>();
                    lstObjRank.Add(null);
                    lstObjRank.Add(1);
                    lstObjRank.Add(int.MaxValue - 1);
                    var lstSalaryClass = actionService.GetData<Cat_SalaryClassEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList();

                    var lstRankByRankDetail = lstSalaryClass.Where(s => rankdetail.Contains(s.ID)).ToList();
                    return Json(lstRankByRankDetail, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }

            }

            return null;
        }

        #region BC HeadCount theo tháng

        public ActionResult GetReportHCByMonthValidate(Hre_ReportHCByMonthModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ReportHCByMonthModel>(model, "Hre_ReportHCByMonth", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        public ActionResult GetReportHCByMonth([DataSourceRequest] DataSourceRequest request, Hre_ReportHCByMonthModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHCByMonthModel>(model, "Hre_ReportHCByMonth", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            var service = new Hre_ReportServices();
            var hrService = new Hre_ProfileServices();
            object obj = new Hre_ReportHCByMonthModel();
            var isDataTable = false;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            var result = service.GetReportHCByMonth(model.dateSearch,
                model.OrgStructureID,
                model.IsCreateTemplate, UserLogin);

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
                    FileName = "Hre_ReportHCByMonthModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportID != Guid.Empty)
            {
                result.Rows[0].Delete();
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                string[] valueField = null;
                if (model.ValueFields != null)
                {
                    valueField = model.ValueFields.Split(',');
                }
                var fullPath = ExportService.Export(model.ExportID, result, listHeaderInfo, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        [HttpPost]
        public ActionResult GetReportDetailProfileHDTJob([DataSourceRequest] DataSourceRequest request, Hre_ReportDetailProfileHDTJobModel model)
        {
            #region Validate
            string status = string.Empty;
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportDetailProfileHDTJobModel>(model, "Hre_ReportDetailProfileHDTJob", ref message);
            if (!checkValidate)
            {
                return Json(message);
            }
            #endregion
            var actionService = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();
            object obj = new Hre_ReportDetailProfileHDTJobModel();
            var isDataTable = false;
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateSearch", Value = model.DateSearch != null ? model.DateSearch : DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1 };
            List<object> lstObjSearch = new List<object>();
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(model.OrgStructureID);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(null);
            lstObjSearch.Add(1);
            lstObjSearch.Add(int.MaxValue - 1);
            var result = actionService.GetData<Hre_HDTJobEntity>(lstObjSearch, ConstantSql.hrm_hr_sp_get_HDTJob, ref status).Where(s =>
                (s.DateFrom <= model.DateSearch && s.DateTo >= model.DateSearch) || (s.DateFrom <= model.DateSearch && s.DateTo == null)).ToList();

            if (result.Count > 0)
            {
                var profileServices = new Hre_ProfileServices();
                var listResult = profileServices.getHDTJobByPrice(result, model.DateFrom, model.DateTo).Translate<Hre_ReportDetailProfileHDTJobModel>();
                return Json(listResult.ToDataSourceResult(request));
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportDetailProfileHDTJobModel(),
                    FileName = "Hre_ReportDetailProfileHDTJob",
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
        public ActionResult GetReportDependantProfileQuit([DataSourceRequest] DataSourceRequest request, Hre_ReportDependantProfileQuitModel Model)
        {
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_ReportDependantProfileQuitModel(),
                    FileName = "Hre_ReportDependantProfileQuit",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportDependantProfileQuitModel>(Model, "Hre_ReportPayHDTJob", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportDependantProfileQuit(Model.DateQuitFrom, Model.DateQuitTo, Model.WorkPlaceID, Model.OrgStructureID, UserLogin).Translate<Hre_ReportDependantProfileQuitModel>();
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, result, null, Model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetCandidateGeneralByProfileId(string ProfileIDs)
        {
            var service = new Hre_ProfileServices();
            var status = string.Empty;
            if (!string.IsNullOrEmpty(ProfileIDs))
            {
                var lstCandidateGeneral = service.GetData<Hre_CandidateGeneralEntity>(Common.DotNetToOracle(ProfileIDs), ConstantSql.hrm_hr_sp_get_CandidateGeneralByProfileID, UserLogin, ref status).ToList();
                if (lstCandidateGeneral != null && lstCandidateGeneral.Count >= 1)
                {
                    var candidateByProfile = lstCandidateGeneral.FirstOrDefault();
                    return Json(candidateByProfile, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(string.Empty);
        }


        // Son.Vo - 20150529 - Gia hạn HĐ
        [HttpPost]
        public ActionResult ExtendContract([Bind]Hre_ContractModel model)
        {
            string status = string.Empty;
            Guid convertProfileID = Guid.Empty;
            string message = string.Empty;
            var lstContractEidt = new List<Hre_ContractEntity>();
            List<Guid> lstIDs = new List<Guid>();
            var insuranceServices = new Sal_InsuranceSalaryServices();
            var ContractServices = new Hre_ContractServices();
            var ExtendContractServices = new Hre_ContractExtendServices();
            if (model.selectedIds != null && model.selectedIds.IndexOf(',') > 1)
            {
                var lstID = model.selectedIds.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertProfileID = Common.ConvertToGuid(lstID[i]);
                    lstIDs.Add(convertProfileID);
                }
            }
            else
            {
                convertProfileID = Common.ConvertToGuid(model.selectedIds);
                lstIDs.Add(convertProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var appendixContractServices = new Hre_AppendixContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContract = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();
            if (lstIDs != null)
            {
                lstContract = lstContract.Where(s => lstIDs.Contains(s.ID)).ToList();
            }

            foreach (var item in lstContract)
            {
                item.DateExtend = model.DateExtendTo;
                ContractServices.Edit(item);
                Hre_ContractExtendEntity Entity = new Hre_ContractExtendEntity();
                Entity.ContractID = item.ID;
                Entity.DateStart = model.DateExtendFrom;
                Entity.DateEnd = model.DateExtendTo;
                message = ExtendContractServices.Add(Entity);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        // Son.Vo - 20150529 - cập nhật kết quả đánh giá HĐ
        [HttpPost]
        public ActionResult UpdateEvaContract([Bind]Hre_ContractModel model)
        {
            string status = string.Empty;
            Guid convertProfileID = Guid.Empty;
            string message = string.Empty;
            var lstContractEidt = new List<Hre_ContractEntity>();
            List<Guid> lstIDs = new List<Guid>();
            var insuranceServices = new Sal_InsuranceSalaryServices();
            var ContractServices = new Hre_ContractServices();
            if (model.selectedIds != null && model.selectedIds.IndexOf(',') > 1)
            {
                var lstID = model.selectedIds.Split(',');
                for (int i = 0; i < lstID.Length; i++)
                {
                    convertProfileID = Common.ConvertToGuid(lstID[i]);
                    lstIDs.Add(convertProfileID);
                }
            }
            else
            {
                convertProfileID = Common.ConvertToGuid(model.selectedIds);
                lstIDs.Add(convertProfileID);
            }

            var actionService = new ActionService(UserLogin);
            var appendixContractServices = new Hre_AppendixContractServices();
            var objContract = new List<object>();
            objContract.AddRange(new object[21]);
            objContract[19] = 1;
            objContract[20] = int.MaxValue - 1;
            var lstContract = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, ref status).ToList();
            if (lstIDs != null)
            {
                lstContract = lstContract.Where(s => lstIDs.Contains(s.ID)).ToList();
            }

            foreach (var item in lstContract)
            {
                if (model.RankDetailForNextContract != null)
                {
                    item.RankDetailForNextContract = model.RankDetailForNextContract;
                }

                if (model.DateEndNextContract != null)
                {
                    item.DateEndNextContract = model.DateEndNextContract;
                }

                // Son.Vo - 20150613 - theo task 0049393
                if (lstContract.Count == 1 && model.RankDetailForNextContract == null)
                {
                    item.RankDetailForNextContract = item.RankRateID;
                }

                message = ContractServices.Edit(item);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public string SaveContractAndNextSalaryApprovedEvaluation(Hre_ContractEntity contract)
        {

            if (contract.DateEndNextContract == null)
            {
                return string.Empty;
            }

            // Lấy biến Dateend này gắn cho quá trình công tác khi cập nhật quá trinh công tác ở dưới
            DateTime? dateEnd = contract.DateEnd;

            string message = string.Empty;
            var actionService = new ActionService(UserLogin);
            string status = string.Empty;

            var profile = actionService.GetData<Hre_ProfileEntity>(contract.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
            var hrService = new Hre_ProfileServices();
            var salaryRankServices = new Cat_SalaryRankServices();
            var lstObjSalaryRank = new List<object>();
            lstObjSalaryRank.Add(null);
            lstObjSalaryRank.Add(null);
            lstObjSalaryRank.Add(1);
            lstObjSalaryRank.Add(int.MaxValue - 1);
            var lstSalaryRank = actionService.GetData<Cat_SalaryRankEntity>(lstObjSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();

            var contractServices = new Hre_ContractServices();

            var workhistoryService = new Hre_WorkHistoryServices();
            var lstObjWorkhistory = new List<object>();
            lstObjWorkhistory.AddRange(new object[17]);
            lstObjWorkhistory[15] = 1;
            lstObjWorkhistory[16] = int.MaxValue - 1;
            var lstWorkhistory = actionService.GetData<Hre_WorkHistoryEntity>(lstObjWorkhistory, ConstantSql.hrm_hr_sp_get_WorkHistory, ref status).ToList();

            var basicSalaryService = new Sal_BasicSalaryServices();

            var attGradeService = new Att_GradeServices();
            var lstObjAttGrade = new List<object>();
            lstObjAttGrade.AddRange(new object[6]);
            lstObjAttGrade[4] = 1;
            lstObjAttGrade[5] = int.MaxValue - 1;
            var lstAttGrade = actionService.GetData<Att_GradeEntity>(lstObjAttGrade, ConstantSql.hrm_att_sp_get_Att_Grade, ref status).ToList();

            var gradeService = new Sal_GradeServices();
            var lstObjSalGrade = new List<object>();
            lstObjSalGrade.AddRange(new object[7]);
            lstObjSalGrade[5] = 1;
            lstObjSalGrade[6] = int.MaxValue - 1;
            var lstSalGrade = actionService.GetData<Sal_GradeEntity>(lstObjSalGrade, ConstantSql.hrm_sal_sp_get_Sal_Grade, ref status).ToList();

            var gradePayrollService = new Cat_GradePayrollServices();
            var lstObjGradePayroll = new List<object>();
            lstObjGradePayroll.Add(null);
            lstObjGradePayroll.Add(null);
            lstObjGradePayroll.Add(1);
            lstObjGradePayroll.Add(int.MaxValue - 1);
            var lstGradePayroll = actionService.GetData<Cat_GradePayrollEntity>(lstObjGradePayroll, ConstantSql.hrm_cat_sp_get_GradePayroll, ref status).ToList();

            var gradeAttService = new Cat_GradeAttendanceServices();
            var lstObjGradeAtt = new List<object>();
            lstObjGradeAtt.AddRange(new object[10]);
            lstObjGradeAtt[8] = 1;
            lstObjGradeAtt[9] = int.MaxValue - 1;
            var lstGradeAtt = actionService.GetData<Cat_GradeAttendanceEntity>(lstObjGradeAtt, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendance, ref status).ToList();

            var currencyServices = new Cat_CurrencyServices();
            var lstObjCurrency = new List<object>();
            lstObjCurrency.Add(null);
            lstObjCurrency.Add(null);
            lstObjCurrency.Add(1);
            lstObjCurrency.Add(int.MaxValue - 1);
            var lstCurrency = actionService.GetData<Cat_CurrencyEntity>(lstObjCurrency, ConstantSql.hrm_cat_sp_get_Currency, ref status).ToList();
            var lstCurrencyNew = lstCurrency.Where(s => s.CurrencyName == "VND").FirstOrDefault();

            var contractTypeService = new Cat_ContractTypeServices();
            var lstObjContractType = new List<object>();
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(null);
            lstObjContractType.Add(1);
            lstObjContractType.Add(int.MaxValue - 1);
            var lstContractType = actionService.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, ref status).ToList();

            var insuranceConfigServices = new Cat_InsuranceConfigServices();
            var objInsuranceConfig = new List<object>();
            objInsuranceConfig.Add(1);
            objInsuranceConfig.Add(int.MaxValue - 1);
            var lstInsuranceConfig = actionService.GetData<Cat_InsuranceConfigEntity>(objInsuranceConfig, ConstantSql.hrm_cat_sp_get_InsuranceConfig, ref status).ToList();

            var insuranceServices = new Sal_InsuranceSalaryServices();
            var objInsurance = new List<object>();
            objInsurance.AddRange(new object[9]);
            objInsurance[7] = 1;
            objInsurance[8] = int.MaxValue - 1;
            var lstInsurance = actionService.GetData<Sal_InsuranceSalaryEntity>(objInsurance, ConstantSql.hrm_sal_sp_get_InsuranceSalary, ref status).ToList();


            var contractTypeEntity = new Cat_ContractTypeEntity();
            var objContract = new List<object>();
            objContract.Add(contract.ProfileID);
            var lstContractIdByProfileID = actionService.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, ref status);
            var listIdContract = string.Empty;
            if (lstContractIdByProfileID != null)
            {
                listIdContract = string.Join(",", lstContractIdByProfileID.Select(d => d.ContractTypeID));
            }

            if (contract.NextContractTypeID != null)
            {
                contractTypeEntity = lstContractType.Where(s => contract.NextContractTypeID.Value == s.ID).FirstOrDefault();
            }
            else
            {
                message = ConstantMessages.WarningContractHaveNotNextContract.ToString().TranslateString();
                return message;
            }


            if (contractTypeEntity != null)
            {
                if (contractTypeEntity.Type == EnumDropDown.TypeContract.E_NODURATION.ToString())
                {
                    return string.Empty;
                }
            }

            var workingHistoryEntity = lstWorkhistory.Where(s => s.ProfileID == contract.ProfileID).FirstOrDefault();
            var objSalGrade = new List<object>();
            objSalGrade.Add(contract.ProfileID);
            objSalGrade.Add(null);
            objSalGrade.Add(1);
            objSalGrade.Add(int.MaxValue - 1);
            var salGradeByProfileIDEntity = actionService.GetData<Sal_GradeEntity>(objSalGrade, ConstantSql.hrm_sal_sp_get_GradeAndAllownaceByProId, ref status).FirstOrDefault();
            var objAttGrade = new List<object>();
            objAttGrade.Add(contract.ProfileID);
            objAttGrade.Add(null);
            objAttGrade.Add(1);
            objAttGrade.Add(int.MaxValue - 1);
            var attGradeByProfileIDEntity = actionService.GetData<Att_GradeEntity>(objAttGrade, ConstantSql.hrm_att_sp_get_GradeAttendanceByProIdCutID, ref status).FirstOrDefault();

            if (contractTypeEntity == null)
            {
                return string.Empty;
            }

            var contracttypeByContract = lstContractType.Where(s => s.ID == contract.ContractTypeID).FirstOrDefault();

            if (contract.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
            {
                if (contract.ContractEvaType == EnumDropDown.ContractEvaType.E_EXPIRED_APPRENTICE.ToString())
                {

                    //chưa tìm dc cách xử lý nên hard code 
                    var lstSalaryRankNew = new Cat_SalaryRankEntity();
                    if (contract.RankDetailForNextContract != null)
                    {
                        lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankDetailForNextContract != null && s.ID == contract.RankDetailForNextContract.Value).FirstOrDefault();
                    }
                    else
                    {
                        lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankRateID != null && s.ID == contract.RankRateID.Value).FirstOrDefault();
                    }

                    #region Xử lý Hre_Contract
                    if (contract.TypeOfPass == EnumDropDown.TypeOfPass.E_SIGNED_NEXTCONTRACT.ToString())
                    {
                        int month = 0;
                        if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                        {
                            month = (int)contractTypeEntity.ValueTime.Value;
                            if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                            {
                                month = month * 12;
                            }
                            contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 
                            //  var nextContractTypeID = Common.ConvertToGuid(contractTypeEntity.ContractNextID).ToString();
                            var contractEntity = new Hre_ContractEntity
                            {
                                //   ContractNo = getContractNo(item, item.DateSigned),
                                ProfileID = contract.ProfileID,
                                ProfileName = contract.ProfileName,
                                DateStart = contract.DateEnd.Value.AddDays(1),
                                DateSigned = contract.DateEnd.Value.AddDays(1),
                                JobTitleID = contract.JobTitleID,
                                PositionID = contract.PositionID,
                                DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID,
                                DateEndNextContract = contract.DateEndNextContract,
                            };
                            if (contract.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = contract.DateEndNextContract.Value;
                            }
                            contractEntity = SetNewCodeContract(contractEntity, listIdContract);

                            #region Nếu là loại hđ xác định thời hạn thì update lại cột TimesContract theo task 0049731
                            if (contracttypeByContract != null && contracttypeByContract.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_DURATION.ToString())
                            {
                                try
                                {
                                    string times = contractEntity.ContractNo.Substring(contractEntity.ContractNo.Length - 1, 1);
                                    int Year = int.Parse(times);
                                    contractEntity.TimesContract = Year;
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                            contractEntity.Status = "E_APPROVED";
                            contractEntity.StatusEvaluation = "E_APPROVED";
                            contractEntity.DateExtend = contract.DateEnd;
                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {
                                return string.Empty;
                            }
                            message = contractServices.Add(contractEntity);
                        }
                        else
                        {
                            contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 
                            //  var nextContractTypeID = Common.ConvertToGuid(contractTypeEntity.ContractNextID).ToString();
                            var contractEntity = new Hre_ContractEntity
                            {
                                //  ContractNo = getContractNo(item, item.DateSigned),
                                ProfileID = contract.ProfileID,
                                ProfileName = contract.ProfileName,
                                DateStart = contract.DateEnd.Value.AddDays(1),
                                DateSigned = contract.DateEnd.Value.AddDays(1),
                                JobTitleID = contract.JobTitleID,
                                PositionID = contract.PositionID,
                                //   DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? contract.RankRateID : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID
                            };

                            //if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                            //{
                            //    contractEntity = SetNewDateEndContract(contractEntity);
                            //}
                            if (contract.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = contract.DateEndNextContract.Value;
                            }
                            contractEntity.DateExtend = contract.DateEnd;
                            contractEntity.StatusEvaluation = "E_APPROVED";

                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {
                                return string.Empty;
                            }

                            message = contractServices.Add(contractEntity);
                        }
                    }

                    //Edit lai StatusEvaluation 
                    contract.StatusEvaluation = "E_APPROVED";
                    message = contractServices.Edit(contract);

                    #endregion

                    #region Xử Lý Sal_BasicSalary
                    var salaryEntity = new Sal_BasicSalaryEntity
                    {
                        ProfileID = contract.ProfileID,
                        ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                        RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                        GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                        DateOfEffect = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                        CurrencyID = lstCurrencyNew.ID,
                        Note = contract.Remark,
                        Status = "E_APPROVED"
                    };
                    message = basicSalaryService.Add(salaryEntity);

                    #endregion

                    #region Xử Lý Hre_Profile
                    var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                    Guid? _AbilityTileID = null;
                    profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;
                    if (profileEntity.SalaryClassID != null)
                    {
                        var abilityTitleBySalaryClass = hrService.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(profileEntity.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, UserLogin, ref status).FirstOrDefault();
                        if (abilityTitleBySalaryClass != null)
                        {
                            _AbilityTileID = abilityTitleBySalaryClass.ID;
                            profileEntity.AbilityTileID = _AbilityTileID;
                        }
                    }

                    hrService.Edit(profileEntity);

                    if (workingHistoryEntity != null)
                    {
                        if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = contract.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                AbilityTileID = _AbilityTileID,
                                Status = "E_APPROVED"

                            };
                            message = workhistoryService.Add(workhistoryEntity);
                        }


                    }
                    else
                    {
                        var workhistoryEntity = new Hre_WorkHistoryEntity
                        {
                            ProfileID = contract.ProfileID,
                            SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                            Status = "E_APPROVED",

                        };
                        message = workhistoryService.Add(workhistoryEntity);
                    }

                    #endregion

                    #region Sal_Grade
                    var lstGradeByProfileID = lstSalGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                    var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                    if (salGradeByProfileIDEntity != null)
                    {
                        if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = contract.DateSigned.Value,
                            };
                            message = gradeService.Add(gradeEntity);
                        }

                    }
                    else
                    {
                        var gradeEntity = new Sal_GradeEntity
                        {
                            //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                            ProfileID = contract.ProfileID,
                            GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                            MonthStart = contract.DateSigned.Value,
                        };
                        message = gradeService.Add(gradeEntity);
                    }

                    #endregion

                    #region Att_Grade
                    var lstAttGradeByProfileID = lstAttGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                    var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                    if (attGradeByProfileIDEntity != null)
                    {
                        if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                // ID = lstGradeAttByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradeAttendanceID = lstAttGradeByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = contract.DateSigned.Value,
                            };
                            message = attGradeService.Add(gradeAttEntity);
                        }
                    }
                    else
                    {
                        var gradeAttEntity = new Att_GradeEntity
                        {
                            // ID = lstGradeAttByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                            ProfileID = contract.ProfileID,
                            GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                            MonthStart = contract.DateSigned.Value,
                        };
                        message = attGradeService.Add(gradeAttEntity);
                    }


                    #endregion

                    #region Xử Lý Lương BHXH
                    if (contractTypeEntity.NoneTypeInsuarance == true)
                    {
                        var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                        var insuranceEntity = new Sal_InsuranceSalaryEntity
                        {
                            ProfileID = contract.ProfileID,
                            InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                            DateEffect = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                            IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                            IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                            IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                            CurrencyID = lstCurrencyNew.ID
                        };
                        if (insuranceEntityByProfileID != null)
                        {
                            insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                            insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                            insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                            insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                            message = insuranceServices.Edit(insuranceEntityByProfileID);
                        }
                        else
                        {
                            message = insuranceServices.Add(insuranceEntity);
                        }


                    }

                    if (contractTypeEntity.NoneTypeInsuarance == false)
                    {

                        var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                        if (insuranceConfigEntity != null)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = contract.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                                IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                CurrencyID = lstCurrencyNew.ID
                            };

                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                message = insuranceServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = insuranceServices.Add(insuranceEntity);
                            }

                        }
                    }
                    #endregion
                }

                if (contract.ContractEvaType == EnumDropDown.ContractEvaType.E_ANNUAL_EVALUATION.ToString() && contract.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
                {
                    var lstSalaryRankNew = new Cat_SalaryRankEntity();
                    if (contract.RankDetailForNextContract != null)
                    {
                        lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankDetailForNextContract != null && s.ID == contract.RankDetailForNextContract.Value).FirstOrDefault();
                    }
                    else
                    {
                        lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankRateID != null && s.ID == contract.RankRateID.Value).FirstOrDefault();
                    }

                    #region Xử lý Hre_Contract
                    int month = 0;
                    if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                    {
                        month = (int)contractTypeEntity.ValueTime.Value;
                        if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                        {
                            month = month * 12;
                        }
                        contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                        //chưa tìm dc cách xử lý nên hard code 

                        var contractEntity = new Hre_ContractEntity
                        {
                            // ContractNo = getContractNo(item, item.DateSigned),
                            ProfileID = contract.ProfileID,
                            ProfileName = contract.ProfileName,
                            DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                            DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                            JobTitleID = contract.JobTitleID,
                            PositionID = contract.PositionID,
                            DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                            Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                            SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                            ContractTypeID = contractTypeEntity.ID
                        };

                        //if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                        //{
                        //    contractEntity = SetNewDateEndContract(contractEntity);
                        //}
                        if (contract.DateEndNextContract != null)
                        {
                            contractEntity.DateEnd = contract.DateEndNextContract.Value;
                        }
                        contractEntity = SetNewCodeContract(contractEntity, listIdContract);

                        #region Nếu là loại hđ xác định thời hạn thì update lại cột TimesContract theo task 0049731
                        if (contracttypeByContract != null && contracttypeByContract.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_DURATION.ToString())
                        {
                            try
                            {
                                string times = contractEntity.ContractNo.Substring(contractEntity.ContractNo.Length - 1, 1);
                                int Year = int.Parse(times);
                                contractEntity.TimesContract = Year;
                            }
                            catch
                            {
                            }
                        }
                        #endregion

                        contractEntity.Status = "E_APPROVED";
                        contractEntity.DateExtend = contract.DateEnd;
                        contractEntity.StatusEvaluation = "E_APPROVED";

                        if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                        {
                            return string.Empty;
                        }
                        message = contractServices.Add(contractEntity);
                    }
                    else
                    {
                        contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                        //chưa tìm dc cách xử lý nên hard code 

                        var contractEntity = new Hre_ContractEntity
                        {
                            // ContractNo = getContractNo(item, item.DateSigned),
                            ProfileID = contract.ProfileID,
                            ProfileName = contract.ProfileName,
                            DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                            DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                            JobTitleID = contract.JobTitleID,
                            PositionID = contract.PositionID,
                            // DateEnd = null,
                            Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                            SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                            ContractTypeID = contractTypeEntity.ID
                        };

                        //if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                        //{
                        //    contractEntity = SetNewDateEndContract(contractEntity);
                        //}

                        if (contract.DateEndNextContract != null)
                        {
                            contractEntity.DateEnd = contract.DateEndNextContract.Value;
                        }
                        contractEntity.DateExtend = contract.DateEnd;
                        contractEntity.StatusEvaluation = "E_APPROVED";
                        if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                        {

                            return string.Empty;
                        }
                        message = contractServices.Add(contractEntity);
                    }
                    //Edit lai StatusEvaluation 
                    contract.StatusEvaluation = "E_APPROVED";
                    message = contractServices.Edit(contract);

                    #endregion

                    #region Xử Lý Sal_BasicSalary
                    var salaryEntity = new Sal_BasicSalaryEntity
                    {
                        ProfileID = contract.ProfileID,
                        ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                        RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                        GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                        DateOfEffect = new DateTime(DateTime.Now.Year, 6, 1),
                        CurrencyID = lstCurrencyNew.ID,
                        Note = contract.Remark,
                        Status = "E_APPROVED"

                    };
                    message = basicSalaryService.Add(salaryEntity);

                    #endregion

                    #region Xử Lý Hre_Profile
                    var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                    profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;
                    Guid? _AbilityTileID = null;
                    if (profileEntity.SalaryClassID != null)
                    {
                        var abilityTitleBySalaryClass = actionService.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(profileEntity.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, ref status).FirstOrDefault();
                        if (abilityTitleBySalaryClass != null)
                        {
                            _AbilityTileID = abilityTitleBySalaryClass.ID;
                            profileEntity.AbilityTileID = _AbilityTileID;
                        }
                    }
                    message = hrService.Edit(profileEntity);
                    if (workingHistoryEntity != null)
                    {
                        if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = contract.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                AbilityTileID = _AbilityTileID,
                                Status = "E_APPROVED"
                            };
                            message = workhistoryService.Add(workhistoryEntity);
                        }
                    }
                    else
                    {
                        var workhistoryEntity = new Hre_WorkHistoryEntity
                        {
                            ProfileID = contract.ProfileID,
                            SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                            Status = "E_APPROVED"

                        };
                        message = workhistoryService.Add(workhistoryEntity);
                    }
                    #endregion

                    #region Sal_Grade
                    var lstGradeByProfileID = lstSalGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                    var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                    if (salGradeByProfileIDEntity != null)
                    {
                        if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1),

                            };
                            message = gradeService.Add(gradeEntity);
                        }
                    }
                    else
                    {
                        var gradeEntity = new Sal_GradeEntity
                        {
                            //  ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                            ProfileID = contract.ProfileID,
                            GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                            MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                        };
                        message = gradeService.Add(gradeEntity);
                    }


                    #endregion

                    #region Att_Grade
                    var lstAttGradeByProfileID = lstAttGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                    var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                    if (attGradeByProfileIDEntity != null)
                    {
                        if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = attGradeService.Add(gradeAttEntity);
                        }
                    }
                    else
                    {
                        var gradeAttEntity = new Att_GradeEntity
                        {
                            //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                            ProfileID = contract.ProfileID,
                            GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                            MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                        };
                        message = attGradeService.Add(gradeAttEntity);
                    }


                    #endregion

                    #region Xử Lý Lương BHXH
                    if (contractTypeEntity.NoneTypeInsuarance == true)
                    {
                        var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                        var insuranceEntity = new Sal_InsuranceSalaryEntity
                        {
                            ProfileID = contract.ProfileID,
                            InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                            DateEffect = contract.DateEnd.Value.AddDays(1),
                            IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                            IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                            IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                            CurrencyID = lstCurrencyNew.ID,

                        };

                        if (insuranceEntityByProfileID != null)
                        {
                            insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                            insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                            insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                            insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                            message = insuranceServices.Edit(insuranceEntityByProfileID);
                        }
                        else
                        {
                            message = insuranceServices.Add(insuranceEntity);
                        }
                    }
                    if (contractTypeEntity.NoneTypeInsuarance == false)
                    {
                        var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                        if (insuranceConfigEntity != null)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = contract.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = contract.DateEnd.Value.AddDays(1),
                                IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                CurrencyID = lstCurrencyNew.ID
                            };
                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                message = insuranceServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = insuranceServices.Add(insuranceEntity);
                            }
                        }
                    }
                    #endregion

                }
            }

            return message;
            //   return null;
        }

        [HttpPost]
        public ActionResult ApprovedEvaContract(string selectedIds)
        {
            var ContractServices = new Hre_ContractServices();
            var profileServices = new Hre_ProfileServices();
            var ContractExtendServices = new Hre_ContractExtendServices();
            string message = string.Empty;
            string status = string.Empty;
            var lstContract = ContractServices.GetData<Hre_ContractEntity>(Common.DotNetToOracle(selectedIds), ConstantSql.hrm_hr_sp_get_ContractByIds, UserLogin, ref status).ToList();
            foreach (var contract in lstContract)
            {
                if (contract.ContractResult == HRM.Infrastructure.Utilities.EnumDropDown.ResultContract.PASS.ToString())
                {
                    if (contract.TypeOfPass == HRM.Infrastructure.Utilities.EnumDropDown.TypeOfPass.E_SIGNED_NEXTCONTRACT.ToString())
                    {
                        if (contract.RankDetailForNextContract == null || contract.DateEndNextContract == null)
                        {
                            message = ConstantDisplay.HRM_HR_Profile_LackOfRequiredInformation.ToString();
                            return Json(message);
                        }
                        contract.StatusEvaluation = "E_APPROVED";
                        message = profileServices.SaveContractAndNextSalaryApprovedEvaluation(contract,UserLogin);

                    }
                    else if (contract.TypeOfPass == HRM.Infrastructure.Utilities.EnumDropDown.TypeOfPass.E_SIGNED_APPENDIXCONTRACT.ToString())
                    {
                        Hre_ContractExtendEntity entity = new Hre_ContractExtendEntity();
                        entity.ContractID = contract.ID;
                        if (contract.DateExtend != null)
                        {
                            entity.DateStart = contract.DateExtend;
                        }
                        else
                        {
                            entity.DateStart = contract.DateEnd;
                        }
                        entity.DateEnd = contract.DateEndNextContract;
                        message = ContractExtendServices.Add(entity);
                        contract.StatusEvaluation = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                        contract.DateExtend = contract.DateEndNextContract;
                        contract.DateStart = contract.DateExtend != null ? contract.DateExtend.Value.AddDays(1) : contract.DateEnd.Value;
                        contract.DateExtend = contract.DateEndNextContract;
                        message = ContractServices.Edit(contract);
                    }
                }
                else
                {
                    contract.StatusEvaluation = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                    message = ContractServices.Edit(contract);
                }

            }
            return Json(message);
        }


        #region Hre_ExtendContract
        [HttpPost]
        public ActionResult GetContractExtendList([DataSourceRequest] DataSourceRequest request, Hre_ContractExtendSearchModel model)
        {
            return GetListDataAndReturn<Hre_ContractExtendModel, Hre_ContractExtendEntity, Hre_ContractExtendSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractExtendList);
        }
        public ActionResult ExportAllContractExtendList([DataSourceRequest] DataSourceRequest request, Hre_ContractExtendSearchModel model)
        {
            string status = string.Empty;
            Hre_ContractServices ActionServices = new Hre_ContractServices();
            var result = GetListData<Hre_ContractExtendModel, Hre_ContractExtendEntity, Hre_ContractExtendSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ContractExtendList, ref status);
            var lstEntity = result.Translate<Hre_ContractEntity>();
            DataTable tb = ActionServices.GetDataContract(lstEntity, UserLogin);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tb,
                    FileName = "Hre_ContractEntity",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, tb, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }
        public ActionResult ExportWordContractExtendByTemplate(List<Guid> selectedIds, string valueFields)
        {
            string messages = string.Empty;
            DateTime DateStart = DateTime.Now;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }

            string status = string.Empty;
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstContractExtend = actionService.GetData<Hre_ContractExtendEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractExtendByListId, ref status);
            if (lstContractExtend == null)
                return null;
            int i = 0;

            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_ProfileWaitingHire" + suffix;
            if (lstContractExtend.Count > 1)
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


            foreach (var ContractExtend in lstContractExtend)
            {
                ContractExtend.DateSign = DateTime.Now.Day;
                ContractExtend.MonthSign = DateTime.Now.Month;
                ContractExtend.YearSign = DateTime.Now.Year;
                if (ContractExtend.IDDateOfIssue != null)
                {
                    ContractExtend.IDDateOfIssueFormat = ContractExtend.IDDateOfIssue.Value.ToString("dd/MM/yyyy");
                }

                if (ContractExtend.DateStartContract != null)
                {
                    ContractExtend.DateStartContractFormat = ContractExtend.DateStartContract.Value.ToString("dd/MM/yyyy");
                }

                if (ContractExtend.DateEndContract != null)
                {
                    ContractExtend.DateEndContractFormat = ContractExtend.DateEndContract.Value.ToString("dd/MM/yyyy");
                }

                if (ContractExtend.DateStart != null)
                {
                    ContractExtend.DateStartFormat = ContractExtend.DateStart.Value.ToString("dd/MM/yyyy");
                }

                if (ContractExtend.DateEnd != null)
                {
                    ContractExtend.DateEndFormat = ContractExtend.DateEnd.Value.ToString("dd/MM/yyyy");
                }

                if (ContractExtend.DateSigned != null)
                {
                    ContractExtend.DateSignedContractFormat = ContractExtend.DateSigned.Value.ToString("dd/MM/yyyy");
                }


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
                    template = actionService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
                }

                if (template == null)
                {
                    messages = "Error";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }

                string templatepath = Common.GetPath(Common.TemplateURL + template.TemplateFile);

                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(ContractExtend.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(ContractExtend.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                var ilContract = new List<Hre_ContractExtendEntity>();
                ilContract.Add(ContractExtend);
                ExportService.ExportWord(outputPath, templatepath, ilContract);
            }

            if (lstContractExtend.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        #endregion

        #region Hre_ProfileCandidateHistory

        [HttpPost]
        public ActionResult GetProfileCandidateHistory([DataSourceRequest] DataSourceRequest request, Hre_ProfileCandidateHistorySearchModel model)
        {
            return GetListDataAndReturn<Hre_CandidateHistoryModel, Hre_CandidateHistoryEntity, Hre_ProfileCandidateHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileCandidateHistory);
        }

        public ActionResult ExportListProfileCandidateHistory([DataSourceRequest] DataSourceRequest request, Hre_ProfileCandidateHistorySearchModel model)
        {
            return ExportAllAndReturn<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel, Hre_ProfileCandidateHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_ProfileCandidateHistory);
        }

        public ActionResult ExportCandidateHistorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel>(selectedIds, valueFields, ConstantSql.hrm_hr_sp_get_CandidateHistoryByIds);
        }


        [HttpPost]
        public ActionResult GetRecCandidateHistory([DataSourceRequest] DataSourceRequest request, Hre_ProfileCandidateHistorySearchModel model)
        {
            return GetListDataAndReturn<Hre_CandidateHistoryModel, Hre_CandidateHistoryEntity, Hre_ProfileCandidateHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_RecCandidateHistory);
        }

        public ActionResult ExportListRecCandidateHistory([DataSourceRequest] DataSourceRequest request, Hre_ProfileCandidateHistorySearchModel model)
        {
            return ExportAllAndReturn<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel, Hre_ProfileCandidateHistorySearchModel>(request, model, ConstantSql.hrm_hr_sp_get_RecCandidateHistory);
        }

        #endregion


        [HttpPost]
        public ActionResult GetReportHDTJobOut([DataSourceRequest] DataSourceRequest request, Hre_ReportHDTJobOutSearchModel Model)
        {
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_HDTJobModel(),
                    FileName = "Hre_HDTJob",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHDTJobOutSearchModel>(Model, "Hre_ReportHDTJobOut", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportHDTJobOut(Model.DateTo, Model.OrgStructureID, Model.WorkPlaceID, Model.ProfileName, Model.CodeEmp, UserLogin).Translate<Hre_HDTJobModel>();

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult GetReportHDTJobIn([DataSourceRequest] DataSourceRequest request, Hre_ReportHDTJobInSearchModel Model)
        {
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_HDTJobModel(),
                    FileName = "Hre_HDTJob",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHDTJobInSearchModel>(Model, "Hre_ReportHDTJobIn", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportHDTJobIn(Model.DateFrom, Model.OrgStructureID, Model.WorkPlaceID, Model.ProfileName, Model.CodeEmp, UserLogin).Translate<Hre_HDTJobModel>();

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }



        [HttpPost]
        public ActionResult GetReportHDTJobDecisionAssignWork([DataSourceRequest] DataSourceRequest request, Hre_ReportHDTJobDecisionAssignWorkSearchModel Model)
        {
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Hre_HDTJobModel(),
                    FileName = "Hre_HDTJob",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #region Validate
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ReportHDTJobDecisionAssignWorkSearchModel>(Model, "Hre_ReportHDTJobDecisionAssignWork", ref message);
            if (!checkValidate)
            {
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            #endregion
            var ReportServices = new Hre_ReportServices();
            var result = ReportServices.GetReportHDTJobDecisionAssignWork(Model.DateFrom, Model.OrgStructureID, Model.PositionID, Model.JobTitleID, Model.ProfileName, Model.CodeEmp, UserLogin).Translate<Hre_HDTJobModel>();

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, result, listHeaderInfo, Model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        public ActionResult ExportProfileRetirementByTemplate(List<Guid> selectedIds, string valueFields)
        {
            //string folderStore = DateTime.Now.ToString("ddMMyyyyHHmmss");
            DateTime DateStart = DateTime.Now;
            string messages = string.Empty;
            string dirpath = Common.GetPath(Common.DownloadURL); ;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            string status = string.Empty;
            var contractServices = new Hre_ContractServices();
            var actionService = new ActionService(UserLogin);
            var objs = new List<object>();
            string strIDs = string.Empty;
            foreach (var item in selectedIds)
            {
                strIDs += Common.DotNetToOracle(item.ToString()) + ",";
            }
            if (strIDs.IndexOf(",") > 0)
                strIDs = strIDs.Substring(0, strIDs.Length - 1);
            objs.Add(strIDs);
            var lstProfile = actionService.GetData<Hre_ProfileEntity>(objs, ConstantSql.hrm_hr_sp_get_ProfileRetirementByListId, ref status);
            if (lstProfile == null)
                return null;
            int i = 0;
            var lstProfileID = lstProfile.Select(s => s.ID).Distinct().ToList();
            String suffix = DateStart.ToString("_ddMMyyyyHHmmss");
            string folferPath = string.Empty;
            string folderName = "ExportHre_Contract" + suffix;
            if (lstProfileID.Count > 1)
            {
                folferPath = dirpath + "/" + folderName;
                Directory.CreateDirectory(folferPath);
            }
            else
            {
                folferPath = dirpath;
            }
            var fileDoc = string.Empty;
            foreach (var profile in lstProfile)
            {
                if (profile.IDDateOfIssue.HasValue)
                    profile.IDDateOfIssueFormat = profile.IDDateOfIssue.Value.ToString("dd/MM/yyyy");
                if (profile.DateOfBirth.HasValue)
                    profile.DateOfBirthFormat = profile.DateOfBirth.Value.ToString("dd/MM/yyyy");

                if (profile.SocialInsIssueDate.HasValue)
                    profile.SocialInsIssueDateFormat = profile.SocialInsIssueDate.Value.ToString("dd/MM/yyyy");

                if (profile.Salary != null)
                    profile.SalaryFormat = String.Format("{0:0,0}", profile.Salary);
                if (profile.Allowance1 != null)
                    profile.Allowance1Format = String.Format("{0:0,0}", profile.Allowance1);

                if (profile.DayOfBirth > 0 && profile.MonthOfBirth > 0 && profile.YearOfBirth > 0)
                {
                    profile.Birthday = profile.DayOfBirth + "/" + profile.MonthOfBirth + "/" + profile.YearOfBirth;
                }
                if (profile.DateHire.HasValue)
                {
                    profile.DateHireFormat = profile.DateHire.Value.ToString("dd/MM/yyyy");
                }
                profile.DateNow = DateTime.Now.ToString("dd/MM/yyyy");
                if (profile.DateStart.HasValue)
                {
                    profile.DateStartString = "Ngày " + profile.DateStart.Value.Day + " Tháng " + profile.DateStart.Value.Month + " Năm " + profile.DateStart.Value.Year + " ";
                    profile.DateStartFormat = profile.DateStart.Value.ToString("dd/MM/yyyy");
                }

                if (profile.DateEnd.HasValue)
                {
                    profile.DateEndString = "Ngày " + profile.DateEnd.Value.Day + " Tháng " + profile.DateEnd.Value.Month + " Năm " + profile.DateEnd.Value.Year + " ";
                    profile.DateEndFormat = profile.DateEnd.Value.ToString("dd/MM/yyyy");
                }
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
                    template = actionService.GetData<Cat_ExportEntity>(Guid.Parse(valueFields), ConstantSql.hrm_cat_sp_get_ExportById, ref status).FirstOrDefault();
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
                outputPath = folferPath + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                fileDoc = NotificationType.Success.ToString() + "," + Common.DownloadURL + "/" + Common.ChuyenTVKhongDau(profile.ProfileName) + suffix + i.ToString() + "_" + template.TemplateFile;
                if (!System.IO.File.Exists(templatepath))
                {
                    messages = "NotTemplate";
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                var lstcontract = new List<Hre_ProfileEntity>();
                lstcontract.Add(profile);
                ExportService.ExportWord(outputPath, templatepath, lstcontract);
            }
            if (lstProfileID.Count > 1)
            {
                var fileZip = Common.MultiExport("", true, folderName);
                return Json(fileZip);
            }
            return Json(fileDoc);
        }

        public string ValidateProfile([DataSourceRequest] DataSourceRequest request, Hre_ProfileModel model)
        {
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_ProfileModel>(model, "Hre_Profile", ref message);
            if (!checkValidate)
            {
                return message;
            }
            else
            {
                return null;
            }
        }

        public string ValidateRegisterComeBack([DataSourceRequest] DataSourceRequest request, Hre_StopWorkingModel model)
        {
            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Hre_StopWorkingModel>(model, "Hre_StopWorking_SusRegisterComeBack", ref message);
            if (!checkValidate)
            {
                return message;
            }
            else
            {
                return null;
            }
        }
    }
}
