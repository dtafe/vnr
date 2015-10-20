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
using HRM.Presentation.Payroll.Models;
using HRM.Business.Payroll.Models;
using HRM.Presentation.Service;
using HRM.Presentation.Hr.Models;
using HRM.Business.Hr.Models;
using HRM.Presentation.Category.Models;
using HRM.Business.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Attendance.Models;
using HRM.Business.Payroll.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Attendance.Domain;
using HRM.Business.Category.Domain;
using System.Data.SqlTypes;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities.Helper;
using System.IO;
using VnResource.Exporter;
using VnResource.Helper.Utility;
using Kendo.Mvc;
using System.ComponentModel;


namespace HRM.Presentation.Payroll.Service.Controllers
{
    public class Sal_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        string Hrm_Hr_Services = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];
        string _status = string.Empty;
        //
        // GET: /Sal_GetData/
        public ActionResult Index()
        {
            return View();
        }

        /// [Quoc.Do] - Lấy danh sách dữ liệu cho thông tin tài khoản (Sal_SalaryInformation) theo điều kiện tìm kiếm
        [HttpPost]
        public ActionResult GetSal_SalaryInformationList([DataSourceRequest] DataSourceRequest request, Sal_SalaryInformationSearchModel model)
        {
            return GetListDataAndReturn<Sal_SalaryInformationModel, Sal_SalaryInformationEntity, Sal_SalaryInformationSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation);

        }
        /// [Quoc.Do] - Xuất danh sách dữ liệu cho thông tin tài khoản (Sal_SalaryInformation) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSal_SalaryInformationList([DataSourceRequest] DataSourceRequest request, Sal_SalaryInformationSearchModel model)
        {
            return ExportAllAndReturn<Sal_SalaryInformationModel, Sal_SalaryInformationEntity, Sal_SalaryInformationSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation);
        }

        /// [Quoc.Do] - Xuất các dòng dữ liệu được chọn của thông tin tài khoản (Sal_SalaryInformation) theo điều kiện tìm kiếm
        public ActionResult ExportSal_SalaryInformationSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_SalaryInformationEntity, Sal_SalaryInformationModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_SalaryInformationIds);
        }

        //public JsonResult GetMultiSalaryInformation(string text)
        //{
        //    return GetDataForControl<HRM.Presentation.Payroll.Models.Sal_SalaryInformationSearchModel.Sal_SalaryInformationMutiModel, Sal_SalaryInformationMultiEntity>(text, ConstantSql.hrm_sal_sp_get_SalaryInformationIds);

        //}

        public JsonResult GetMultiProfile(string text)
        {
            return GetDataForControl<Hre_ProfileMultiModel, Hre_ProfileMultiEntity>(text, ConstantSql.hrm_hr_sp_get_Profile_multi);
        }
        public JsonResult GetMultiBank(string text)
        {
            return GetDataForControl<CatBankMultiModel, Cat_BankMultiEntity>(text, ConstantSql.hrm_Cat_sp_get_Bank_multi);
        }
        public JsonResult GetMultiCurrency(string text)
        {
            return GetDataForControl<CatCurrencyMultiModel, Cat_CurrencyMultiEntity>(text, ConstantSql.hrm_cat_sp_get_Currency_Multi);
        }

        public JsonResult GetMultiDepartment(string text)
        {
            return GetDataForControl<Sal_SalaryDepartmentMultiModel, Sal_SalaryDepartmentMultiEntity>(text, ConstantSql.hrm_sal_sp_get_SalDepartment_Multi);
        }

        public ActionResult Get_SalaryDeparment([DataSourceRequest] DataSourceRequest request, Sal_SalaryDepartmentSearchModel model)
        {
            return GetListDataAndReturn<Sal_SalaryDepartmentModel, Sal_SalaryDepartmentEntity, Sal_SalaryDepartmentSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_SalaryDeparment);
        }
        public ActionResult ExportAllSalaryDeparmentList([DataSourceRequest] DataSourceRequest request, Sal_SalaryDepartmentSearchModel model)
        {
            return ExportAllAndReturn<Sal_SalaryDepartmentEntity, Sal_SalaryDepartmentModel, Sal_SalaryDepartmentSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_SalaryDeparment);
        }

        public ActionResult ExportAllSalaryDeparmentDetailList([DataSourceRequest] DataSourceRequest request, Sal_SalaryDepartmentModel model)
        {
            string fullPath = string.Empty, status = string.Empty;

            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(model.PurchaseRequestID);
            var listModel = baseService.GetData<Sal_SalaryDepartmentItemModel>(objs, ConstantSql.hrm_cat_sp_get_DepartmentIteByDepartmentID, ref status);

            //      var listModel = GetListData<TModel, TEntity, TModelSearch>(request, model, storeName, ref status);

            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportSalaryDeparmentSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_SalaryDepartmentEntity, Sal_SalaryDepartmentModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_SalDepartmentByIds);
        }

        public ActionResult GetDepartmentItemByDepartmentID([DataSourceRequest] DataSourceRequest request, Guid? PurchaseRequestID)
        {

            if (PurchaseRequestID != null)
            {
                string status = string.Empty;
                var baseService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(PurchaseRequestID);
                var result = baseService.GetData<Sal_SalaryDepartmentItemModel>(objs, ConstantSql.hrm_cat_sp_get_DepartmentIteByDepartmentID, ref status);
                return Json(result.ToDataSourceResult(request));
            }
            return Json("");
        }

        #region Sal_ProductCapacity
        public ActionResult GetProductCapacityList([DataSourceRequest] DataSourceRequest request, Sal_ProductCapacitySearchModel model)
        {
            return GetListDataAndReturn<Sal_ProductCapacityModel, Sal_ProductCapacityEntity, Sal_ProductCapacitySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_ProductCapacity);
        }
        public ActionResult ExportProductCapacitySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_ProductCapacityEntity, Sal_ProductCapacityModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_ProductCapacityByIds);
        }
        public ActionResult ExportAllProductCapacityList([DataSourceRequest] DataSourceRequest request, Sal_ProductCapacitySearchModel model)
        {
            return ExportAllAndReturn<Sal_ProductCapacityEntity, Sal_ProductCapacityModel, Sal_ProductCapacitySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_ProductCapacity);
        }
        #endregion

        #region BasicSalary
        public ActionResult GetBasicSalary([DataSourceRequest] DataSourceRequest request, Sal_BasicSalarySearchModel payrollModel)
        {
            return GetListDataAndReturn<Sal_BasicSalaryModel, Sal_BasicSalaryEntity, Sal_BasicSalarySearchModel>(request, payrollModel, ConstantSql.hrm_sal_sp_get_BasicPayroll);
        }

        public ActionResult ExportAllBasicSalaryList([DataSourceRequest] DataSourceRequest request, Sal_BasicSalarySearchModel model)
        {
            return ExportAllAndReturn<Sal_BasicSalaryModel, Sal_BasicSalaryEntity, Sal_BasicSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_BasicPayroll);
        }

        public ActionResult ExportBasicSalarySelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_BasicSalaryEntity, Sal_BasicSalaryModel>(string.Join(",", selectedIds), valueFields, ConstantSql.hrm_sal_sp_get_BasicSalaryIds);
        }

        public ActionResult GetBasicSalaryByIdProfile([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            objs.Add(1);
            objs.Add(10000);
            var _tmp = baseService.GetData<Sal_BasicSalaryEntity>(objs, ConstantSql.hrm_sal_sp_get_BasicPayrollById, ref status);

            if (model.IsExport)
            {
                status = ExportService.Export(_tmp, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }
            return Json(_tmp.ToDataSourceResult(request));
        }

        // Son.Vo - 20150317 - load phụ cấp với loại pc là thường xuyên - có loại pc UnusualAllowanceCfg = E_UNUSUALALLOWANCE 
        public ActionResult GetUnusualEDByIdProfile([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Sal_UnusualAllowanceEntity>(objs, ConstantSql.hrm_sal_sp_get_UnusualEDByProId, ref status);
            if (model.IsExport)
            {
                status = ExportService.Export(result, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }
            return Json(result.ToDataSourceResult(request));
        }

        // Son.Vo - 20150317 - load phụ cấp với loại pc là bất thường - có loại pc UnusualAllowanceCfg = E_ALLOWANCE 
        public ActionResult GetUnEDByIdProfile([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var result = baseService.GetData<Sal_UnusualAllowanceEntity>(objs, ConstantSql.hrm_sal_sp_get_UnuEDByProId, ref status);
            if (model.IsExport)
            {
                status = ExportService.Export(result, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult GetGradeAndAllowanceByIdProfile([DataSourceRequest] DataSourceRequest request, Att_ProIDAndCutIDModel model)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(model.ProfileID));
            objs.Add(Common.DotNetToOracle(model.CutOffDurationID));
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);

            var result = baseService.GetData<Sal_GradeEntity>(objs, ConstantSql.hrm_sal_sp_get_GradeAndAllownaceByProId, ref status);
            if (model.IsExport)
            {
                status = ExportService.Export(result, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
                return Json(status);
            }

            if (result == null)
            {
                return null;
            }
            return Json(result.ToDataSourceResult(request));

        }
        /// <summary>
        /// [Hien.Nguyen] lấy dữ liệu bảng PayrollTableItem by Id Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetPayrollTableItemByIdProfile([DataSourceRequest] DataSourceRequest request, Sal_PayrollTableItemModelExportPDF model)
        {
            if (model.ExportId != Guid.Empty)
            {
                ActionService actServices = new ActionService(UserLogin);
                ListQueryModel lstModel = new ListQueryModel
                {
                    PageSize = int.MaxValue - 1,
                    PageIndex = 1,
                    Filters = ExtractFilterAttributes(request),
                    Sorts = ExtractSortAttributes(request),
                    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
                };
                string status = string.Empty;
                var service = new ActionService(UserLogin);

                var insuranceService = new ActionService(UserLogin);

                //var objSalary = new List<object>();
                //objSalary.AddRange(new object[10]);
                //objSalary[8] = 1;
                //objSalary[9] = int.MaxValue - 1;
                //var basicSalaryEnity = insuranceService.GetData<Sal_InsuranceSalaryEntity>(objSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref status).ToList();


                var objInsurance = new List<object>();
                objInsurance.AddRange(new object[9]);
                objInsurance[7] = 1;
                objInsurance[8] = int.MaxValue - 1;
                var insuranceEntity = insuranceService.GetData<Sal_InsuranceSalaryEntity>(objInsurance, ConstantSql.hrm_sal_sp_get_InsuranceSalary, ref status).Where(s => s.ProfileID == model.ProfileID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                var listEntity = service.GetData<Sal_PayrollTableItemEntity>(lstModel, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile, ref status).Translate<Sal_PayrollTableItemModel>();

                var profileEntity = actServices.GetByIdUseStore<Hre_ProfileEntity>(model.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);

                var CutoffDurationByID = actServices.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

                var objSalInformation = new List<object>();
                objSalInformation.AddRange(new object[8]);
                objSalInformation[6] = 1;
                objSalInformation[7] = int.MaxValue - 1;
                var Sal_InfomationByProfile = insuranceService.GetData<Sal_SalaryInformationEntity>(objSalInformation, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, ref status).Where(m => m.ProfileID == profileEntity.ID).FirstOrDefault();


                var objCostcenterCode = new List<object>();
                objCostcenterCode.Add(profileEntity.ProfileName);
                objCostcenterCode.Add(1);
                objCostcenterCode.Add(int.MaxValue - 1);
                var lstCostCenterByProfile = insuranceService.GetData<Sal_CostCentreSalEntity>(objCostcenterCode, ConstantSql.hrm_sal_sp_get_CostCentre, ref status).ToList();


                var costcenterEntity = new Sal_CostCentreSalEntity();

                if (CutoffDurationByID != null)
                {
                    costcenterEntity = lstCostCenterByProfile.Where(s => s.DateStart >= CutoffDurationByID.DateStart && s.DateStart <= CutoffDurationByID.DateEnd).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                }


                DataTable table = new DataTable();
                if (listEntity != null && profileEntity != null)
                {

                    DataRow dr = table.NewRow();
                    table.Columns.Add("CodeEmp");
                    table.Columns.Add("ProfileName");
                    table.Columns.Add("JobTitleName");
                    table.Columns.Add("PositionName");
                    table.Columns.Add("OrgStructureName");
                    table.Columns.Add("SalaryClassName");
                    table.Columns.Add("WorkPlaceName");
                    table.Columns.Add("CostCenterCode");
                    table.Columns.Add("DateExport", typeof(DateTime));



                    table.Columns.Add("DateHire", typeof(DateTime));
                    table.Columns.Add("CodeTax");
                    table.Columns.Add("Email");
                    table.Columns.Add("GrossAmount", typeof(double));
                    table.Columns.Add("InsuranceAmount", typeof(double));

                    table.Columns.Add("DateEnd", typeof(DateTime));
                    table.Columns.Add("BankName", typeof(string));
                    table.Columns.Add("AccountNo", typeof(string));

                    dr["CodeEmp"] = profileEntity.CodeEmp;
                    dr["ProfileName"] = profileEntity.ProfileName;
                    dr["JobTitleName"] = profileEntity.JobTitleName;
                    dr["PositionName"] = profileEntity.PositionName;
                    dr["OrgStructureName"] = profileEntity.OrgStructureName;
                    dr["SalaryClassName"] = profileEntity.SalaryClassName;
                    dr["CostCenterCode"] = costcenterEntity != null ? costcenterEntity.CostCenterCode : string.Empty;
                    if (CutoffDurationByID != null)
                    {
                        dr["DateExport"] = CutoffDurationByID.MonthYear;
                        dr["DateEnd"] = CutoffDurationByID.DateEnd;
                    }



                    dr["WorkPlaceName"] = profileEntity.WorkPlaceName;
                    dr["DateHire"] = profileEntity.DateHire != null ? profileEntity.DateHire.Value.ToShortDateString() : string.Empty;
                    dr["CodeTax"] = profileEntity.CodeTax;
                    dr["Email"] = profileEntity.Email;
                    //dr["GrossAmount"] = basicSalaryEntity != null ? double.Parse(basicSalaryEntity.GrossAmount): 0;
                    dr["InsuranceAmount"] = insuranceEntity != null ? insuranceEntity.InsuranceAmount : 0;

                    dr["AccountNo"] = Sal_InfomationByProfile != null && Sal_InfomationByProfile.AccountNo != null ? Sal_InfomationByProfile.AccountNo : "";
                    dr["BankName"] = Sal_InfomationByProfile != null && Sal_InfomationByProfile.BankName != null ? Sal_InfomationByProfile.BankName : "";

                    //Sort theo tiền tệ
                    foreach (var i in listEntity)
                    {
                        Double value = 0;
                        if (Double.TryParse(i.Value, out value))
                        {
                            i.Amount = value;
                        }
                        else
                        {
                            i.Amount = double.MaxValue;
                        }
                    }
                    listEntity = listEntity.OrderByDescending(m => m.Amount).ToList();

                    //Lọc các cột phụ cấp cộng ra
                    List<Sal_PayrollTableItemModel> listUnusual = new List<Sal_PayrollTableItemModel>();
                    listUnusual = listEntity.Where(m => m.ElementType == CatElementType.AllowancesOut.ToString()).ToList();
                    if (listUnusual != null && listUnusual.Count > 0)
                    {
                        listUnusual = listUnusual.Where(m => m.Value != "0" && m.Value != "0.0" && m.Value != string.Empty).ToList();
                        for (int i = 0; i < listUnusual.Count; i++)
                        {
                            //tạo các phụ cấp
                            table.Columns.Add("PC_Label" + (i + 1).ToString());
                            dr["PC_Label" + (i + 1).ToString()] = listUnusual[i].Name;

                            //tạo các phụ cấp
                            table.Columns.Add("PC_Value" + (i + 1).ToString());
                            dr["PC_Value" + (i + 1).ToString()] = listUnusual[i].Value;

                            ////Loại bỏ các phần tử là phụ cấp khỏi list tổng
                            //listEntity.Remove(listUnusual[i]);
                        }
                    }

                    //Lọc các cột phụ cấp trừ ra
                    listUnusual = new List<Sal_PayrollTableItemModel>();
                    listUnusual = listEntity.Where(m => m.ElementType == CatElementType.AllowancesOut_Minus.ToString()).ToList();
                    if (listUnusual != null && listUnusual.Count > 0)
                    {
                        listUnusual = listUnusual.Where(m => m.Value != "0" && m.Value != "0.0" && m.Value != string.Empty).ToList();
                        for (int i = 0; i < listUnusual.Count; i++)
                        {
                            //tạo các phụ cấp
                            table.Columns.Add("PC_Minus_Label" + (i + 1).ToString());
                            dr["PC_Minus_Label" + (i + 1).ToString()] = listUnusual[i].Name;

                            //tạo các phụ cấp
                            table.Columns.Add("PC_Minus_Value" + (i + 1).ToString());
                            dr["PC_Minus_Value" + (i + 1).ToString()] = listUnusual[i].Value;

                            ////Loại bỏ các phần tử là phụ cấp khỏi list tổng
                            //listEntity.Remove(listUnusual[i]);
                        }
                    }

                    foreach (var item in listEntity)
                    {
                        Double value = 0;
                        if (item.ValueType == "Nvarchar")
                        {
                            if (!table.Columns.Contains(item.Code))
                            {
                                table.Columns.Add(item.Code, typeof(string));
                            }
                            if (table.Columns.Contains(item.Code))
                            {
                                dr[item.Code] = item.Value;
                            }
                        }
                        if (item.ValueType == "Double")
                        {
                            if (!table.Columns.Contains(item.Code))
                            {
                                table.Columns.Add(item.Code, typeof(Double));
                            }
                            if (table.Columns.Contains(item.Code))
                            {
                                if (item.ValueType == typeof(Double).Name)
                                {
                                    Double.TryParse(item.Value, out value);
                                }
                                dr[item.Code] = value;
                            }
                        }

                    }
                    table.Rows.Add(dr);
                }
                if (model.ExportPDF != null && model.ExportPDF == true)
                {
                    var fullPath = ExportService.Export(model.ExportId, table, ExportFileType.PDF);
                    return Json(fullPath);
                }
                else
                {
                    var fullPath = ExportService.Export(model.ExportId, table, model.ExportType);
                    return Json(fullPath);
                }

            }
            var baseService = new ActionService(UserLogin);

            string str = string.Empty;
            List<object> ListModel = new List<object>();
            ListModel.AddRange(new object[7]);
            ListModel[0] = Common.DotNetToOracle(model.ProfileID.ToString());
            ListModel[1] = model.CutOffDurationID != null ? Common.DotNetToOracle(model.CutOffDurationID.ToString()) : null;
            ListModel[5] = 1;
            ListModel[6] = Int32.MaxValue - 1;
            var listResult = baseService.GetData<Sal_PayrollTableItemEntity>(ListModel, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile, ref str);
            if (listResult != null && listResult.Count > 0)
            {
                listResult = listResult.Where(m => (m.Value.ReplaceSpace() != "0" || m.Description1 != string.Empty) && (m.IsShow != false)).ToList();

                listResult = listResult.Where(m => m.ElementType != CatElementType.Comission.ToString()).ToList();

                listResult = listResult.OrderBy(s => s.OrderNo).ToList();
            }
            return Json(listResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return GetListDataAndReturn<Sal_PayrollTableItemModel, Sal_PayrollTableItemEntity, Sal_PayrollTableItemModelSearch>(request, model, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile);
        }

        public ActionResult GetPayrollCommissionByProfileID([DataSourceRequest] DataSourceRequest request, Sal_PayrollTableItemModelSearch model)
        {
            var baseService = new ActionService(UserLogin);
            string str = string.Empty;
            List<object> ListModel = new List<object>();
            ListModel.AddRange(new object[7]);
            ListModel[0] = Common.DotNetToOracle(model.ProfileID.ToString());
            ListModel[1] = model.CutOffDurationID != null ? Common.DotNetToOracle(model.CutOffDurationID.ToString()) : null;
            ListModel[5] = 1;
            ListModel[6] = Int32.MaxValue - 1;
            var listResult = baseService.GetData<Sal_PayrollTableItemEntity>(ListModel, ConstantSql.hrm_sal_sp_get_PayCommissionItem, ref str);
            if (listResult != null && listResult.Count > 0)
            {
                listResult = listResult.Where(m => m.Value.ReplaceSpace() != "0" || m.Description1 != string.Empty && (m.IsShow != false)).ToList();
            }
            return Json(listResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMultiHoldSalary()
        {
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[10]);
            return GetData<Sal_HoldSalaryModel, Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary);
        }

        public ActionResult GetUnusualPayItemByIdProfile([DataSourceRequest] DataSourceRequest request, Sal_UnusualPayItemSearchModel model)
        {
            if (model.ExportId != Guid.Empty)
            {
                ActionService actServices = new ActionService(UserLogin);
                ListQueryModel lstModel = new ListQueryModel
                {
                    PageSize = int.MaxValue - 1,
                    PageIndex = 1,
                    Filters = ExtractFilterAttributes(request),
                    Sorts = ExtractSortAttributes(request),
                    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
                };
                string status = string.Empty;
                var service = new BaseService();
                var listEntity = service.GetData<Sal_UnusualPayItemEntity>(lstModel, ConstantSql.hrm_sal_sp_get_UnusualPayItemByProfile, UserLogin, ref status).Translate<Sal_UnusualPayItemModel>();
                var profileEntity = actServices.GetByIdUseStore<Hre_ProfileEntity>(model.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                if (listEntity != null && profileEntity != null)
                {
                    DataTable table = new DataTable();
                    DataRow dr = table.NewRow();
                    table.Columns.Add("CodeEmp");
                    table.Columns.Add("ProfileName");
                    table.Columns.Add("JobTitleName");
                    table.Columns.Add("DateHire", typeof(DateTime));
                    table.Columns.Add("CodeTax");

                    dr["CodeEmp"] = profileEntity.CodeEmp;
                    dr["ProfileName"] = profileEntity.ProfileName;
                    dr["JobTitleName"] = profileEntity.JobTitleName;
                    dr["DateHire"] = profileEntity.DateHire != null ? profileEntity.DateHire.Value.ToShortDateString() : string.Empty;
                    dr["CodeTax"] = profileEntity.CodeTax;
                    foreach (var item in listEntity)
                    {
                        Double value = 0;
                        if (!table.Columns.Contains(item.Code))
                        {
                            table.Columns.Add(item.Code, typeof(Double));
                        }
                        if (table.Columns.Contains(item.Code))
                        {
                            //if (item.Amount == typeof(Double).Name)
                            //{
                            //    Double.TryParse(item.Value, out value);
                            //}
                            dr[item.Code] = item.Amount;
                        }
                    }
                    table.Rows.Add(dr);
                    var fullPath = ExportService.Export(model.ExportId, table, model.ExportType);
                    return Json(fullPath);
                }
            }
            return GetListDataAndReturn<Sal_UnusualPayItemModel, Sal_UnusualPayItemEntity, Sal_UnusualPayItemSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_UnusualPayItemByProfile);
        }

        public JsonResult GetMultiSalaryJobGroup(string text)
        {
            //return GetDataForControl<Cat_TAMScanReasonMissMuitlModel, Cat_TAMScanReasonMissMultiEntity>(text, ConstantSql.hrm_cat_sp_get_TamScanReasonMiss_multi);
            return GetDataForControl<CatSalaryJobGroupMultiModel, Cat_SalaryJobGroupEntity>(text, ConstantSql.hrm_cat_sp_get_SalaryJobGroup_Multi);
        }



        #endregion

        #region Sal_AdjustmentSuggestion
        public ActionResult GetAdjustmentSuggestion([DataSourceRequest] DataSourceRequest request, Sal_AdjustmentSuggestionSearchModel model)
        {

            var hreServices = new ActionService(UserLogin);
            string status = string.Empty;

            var objProfile = new List<object>();
            objProfile.AddRange(new object[17]);
            objProfile[0] = model.ProfileName;
            objProfile[1] = model.CodeEmp;
            objProfile[2] = model.OrgStructureIDs;
            objProfile[15] = 1;
            objProfile[16] = int.MaxValue - 1;
            var lstProfileID = hreServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status).Select(s => s.ID).ToList();

            var salaryServices = new ActionService(UserLogin);
            var objSalary = new List<object>();
            objSalary.AddRange(new object[10]);
            objSalary[8] = 1;
            objSalary[9] = int.MaxValue - 1;
            var lstBasicSalary = salaryServices.GetData<Sal_BasicSalaryEntity>(objSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref status).ToList();

            List<Sal_AdjustmentSuggestionEntity> lstAdjustmentSuggestionEntity = new List<Sal_AdjustmentSuggestionEntity>();
            List<Sal_AdjustmentSuggestionModel> lstAdjustmentSuggestion = new List<Sal_AdjustmentSuggestionModel>();

            foreach (var item in lstProfileID)
            {
                Sal_AdjustmentSuggestionEntity adjustmentSuggestionModel = new Sal_AdjustmentSuggestionEntity();
                var basicSalaryEntityByProfileID = lstBasicSalary.Where(s => item == s.ProfileID).FirstOrDefault();

                if (basicSalaryEntityByProfileID != null)
                {


                    adjustmentSuggestionModel = basicSalaryEntityByProfileID.CopyData<Sal_AdjustmentSuggestionEntity>();

                    lstAdjustmentSuggestionEntity.Add(adjustmentSuggestionModel);
                }
            }
            if (lstAdjustmentSuggestionEntity.Count > 0)
            {
                lstAdjustmentSuggestion = lstAdjustmentSuggestionEntity.Translate<Sal_AdjustmentSuggestionModel>();
                return Json(lstAdjustmentSuggestion.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        public ActionResult GetAdjustmentSuggestionRankDetail([DataSourceRequest] DataSourceRequest request, Sal_AdjustmentSuggestionSearchModel model)
        {

            var hreServices = new ActionService(UserLogin);
            string status = string.Empty;
            var rankServices = new ActionService(UserLogin);
            var objRank = new List<object>();
            objRank.Add(null);
            objRank.Add(1);
            objRank.Add(int.MaxValue - 1);
            var lstRank = rankServices.GetData<Cat_SalaryClassEntity>(objRank, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList();

            var rankDetailServices = new ActionService(UserLogin);
            var objRankDetail = new List<object>();
            objRankDetail.AddRange(new object[4]);
            objRankDetail[2] = 1;
            objRankDetail[3] = int.MaxValue - 1;
            var lstRankDetail = rankDetailServices.GetData<Cat_SalaryRankEntity>(objRankDetail, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();

            Guid[] profileID = null;
            if (!string.IsNullOrEmpty(model.ValueFields))
            {
                if (model.ValueFields.IndexOf('-') > 1)
                {
                    profileID = model.ValueFields.Split(',').Select(s => Guid.Parse(s)).ToArray();
                }
            }
            var objProfile = new List<object>();
            objProfile.AddRange(new object[17]);
            objProfile[0] = model.ProfileName;
            objProfile[1] = model.CodeEmp;
            objProfile[2] = model.OrgStructureIDs;
            objProfile[15] = 1;
            objProfile[16] = int.MaxValue - 1;
            var lstProfileID = hreServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status).Select(s => s.ID).ToList();

            if (profileID != null)
            {
                lstProfileID = lstProfileID.Where(s => profileID.Contains(s)).ToList();
            }

            var salaryServices = new ActionService(UserLogin);
            var objSalary = new List<object>();
            objSalary.AddRange(new object[10]);
            objSalary[8] = 1;
            objSalary[9] = int.MaxValue - 1;
            var lstBasicSalary = salaryServices.GetData<Sal_BasicSalaryEntity>(objSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref status).ToList();

            List<Sal_AdjustmentSuggestionEntity> lstAdjustmentSuggestionEntity = new List<Sal_AdjustmentSuggestionEntity>();
            List<Sal_AdjustmentSuggestionModel> lstAdjustmentSuggestion = new List<Sal_AdjustmentSuggestionModel>();
            foreach (var item in lstProfileID)
            {
                Sal_AdjustmentSuggestionEntity adjustmentSuggestionModel = new Sal_AdjustmentSuggestionEntity();
                var basicSalaryEntityByProfileID = lstBasicSalary.Where(s => item == s.ProfileID).FirstOrDefault();
                if (basicSalaryEntityByProfileID != null)
                {
                    var lstRankDetailByRankID = lstRankDetail.Where(s => s.SalaryClassID == basicSalaryEntityByProfileID.ClassRateID).ToList();
                    adjustmentSuggestionModel = basicSalaryEntityByProfileID.CopyData<Sal_AdjustmentSuggestionEntity>();

                    double orderNumberParse = 0;
                    if (model.OrderNumber != null)
                    {
                        double.TryParse(model.OrderNumber, out orderNumberParse);
                    }
                    double orderNumberIncrease = (double)basicSalaryEntityByProfileID.OrderNumber.Value + int.Parse(model.OrderNumber);
                    if (orderNumberIncrease >= lstRankDetailByRankID.Count)
                    {
                        var orderNumberTimes = Math.Floor(orderNumberIncrease / double.Parse(lstRankDetailByRankID.Count.ToString()));
                        var orderNumberSub = orderNumberIncrease - (double.Parse(lstRankDetailByRankID.Count.ToString()) * orderNumberTimes);
                        var rankNew = lstRank.Where(s => (basicSalaryEntityByProfileID.OrderNumberRank + orderNumberTimes) == s.OrderNumber).FirstOrDefault();
                        if (rankNew != null)
                        {
                            if (orderNumberSub == 0)
                            {
                                var rankDetailSpecial = lstRankDetail.Where(s => rankNew.ID == s.SalaryClassID).OrderBy(s => s.OrderNumber).FirstOrDefault();
                                if (rankDetailSpecial != null)
                                {
                                    adjustmentSuggestionModel.ClassRateID = rankDetailSpecial.SalaryClassID;
                                    adjustmentSuggestionModel.SalaryClassName = rankDetailSpecial.SalaryClassName;
                                    adjustmentSuggestionModel.RankRateID = rankDetailSpecial.ID;
                                    adjustmentSuggestionModel.SalaryRankName = rankDetailSpecial.SalaryRankName;
                                    adjustmentSuggestionModel.GrossAmount = rankDetailSpecial.SalaryStandard.ToString();
                                    lstAdjustmentSuggestionEntity.Add(adjustmentSuggestionModel);
                                }
                            }
                            var rankDetailNew = lstRankDetail.Where(s => rankNew.ID == s.SalaryClassID && s.OrderNumber == orderNumberSub).FirstOrDefault();
                            if (rankDetailNew != null)
                            {
                                adjustmentSuggestionModel.ClassRateID = rankDetailNew.SalaryClassID;
                                adjustmentSuggestionModel.SalaryClassName = rankDetailNew.SalaryClassName;
                                adjustmentSuggestionModel.RankRateID = rankDetailNew.ID;
                                adjustmentSuggestionModel.SalaryRankName = rankDetailNew.SalaryRankName;
                                adjustmentSuggestionModel.GrossAmount = rankDetailNew.SalaryStandard.ToString();
                                lstAdjustmentSuggestionEntity.Add(adjustmentSuggestionModel);
                            }
                        }

                    }
                    else
                    {

                        var rankDetailEntity = lstRankDetail.Where(s => s.SalaryClassID == basicSalaryEntityByProfileID.ClassRateID && s.OrderNumber == (basicSalaryEntityByProfileID.OrderNumber + int.Parse(model.OrderNumber))).FirstOrDefault();
                        if (rankDetailEntity != null)
                        {
                            adjustmentSuggestionModel.ClassRateID = rankDetailEntity.SalaryClassID;
                            adjustmentSuggestionModel.SalaryClassName = rankDetailEntity.SalaryClassName;
                            adjustmentSuggestionModel.RankRateID = rankDetailEntity.ID;
                            adjustmentSuggestionModel.SalaryRankName = rankDetailEntity.SalaryRankName;
                            adjustmentSuggestionModel.GrossAmount = rankDetailEntity.SalaryStandard.ToString();
                            lstAdjustmentSuggestionEntity.Add(adjustmentSuggestionModel);
                        }

                    }

                }

            }
            if (lstAdjustmentSuggestionEntity.Count > 0)
            {
                lstAdjustmentSuggestion = lstAdjustmentSuggestionEntity.Translate<Sal_AdjustmentSuggestionModel>();
                return Json(lstAdjustmentSuggestion.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult GetAdjustmentSuggestionRank([DataSourceRequest] DataSourceRequest request, Sal_AdjustmentSuggestionSearchModel model)
        {

            var hreServices = new ActionService(UserLogin);
            string status = string.Empty;

            var rankServices = new ActionService(UserLogin);
            var objRank = new List<object>();
            objRank.Add(null);
            objRank.Add(1);
            objRank.Add(int.MaxValue - 1);
            var lstRank = rankServices.GetData<Cat_SalaryClassEntity>(objRank, ConstantSql.hrm_cat_sp_get_SalaryClass, ref status).ToList();

            var rankDetailServices = new ActionService(UserLogin);
            var objRankDetail = new List<object>();
            objRankDetail.AddRange(new object[4]);
            objRankDetail[2] = 1;
            objRankDetail[3] = int.MaxValue - 1;
            var lstRankDetail = rankDetailServices.GetData<Cat_SalaryRankEntity>(objRankDetail, ConstantSql.hrm_cat_sp_get_SalaryRank, ref status).ToList();

            Guid[] profileID = null;
            if (!string.IsNullOrEmpty(model.ValueFields))
            {
                if (model.ValueFields.IndexOf('-') > 1)
                {
                    profileID = model.ValueFields.Split(',').Select(s => Guid.Parse(s)).ToArray();
                }

            }
            var objProfile = new List<object>();
            objProfile.AddRange(new object[17]);
            objProfile[0] = model.ProfileName;
            objProfile[1] = model.CodeEmp;
            objProfile[2] = model.OrgStructureIDs;
            objProfile[15] = 1;
            objProfile[16] = int.MaxValue - 1;
            var lstProfileID = hreServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status).Select(s => s.ID).ToList();

            if (profileID != null)
            {
                lstProfileID = lstProfileID.Where(s => profileID.Contains(s)).ToList();
            }

            var salaryServices = new ActionService(UserLogin);
            var objSalary = new List<object>();
            objSalary.AddRange(new object[10]);
            objSalary[8] = 1;
            objSalary[9] = int.MaxValue - 1;
            var lstBasicSalary = salaryServices.GetData<Sal_BasicSalaryEntity>(objSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref status).ToList();

            List<Sal_AdjustmentSuggestionEntity> lstAdjustmentSuggestionEntity = new List<Sal_AdjustmentSuggestionEntity>();
            List<Sal_AdjustmentSuggestionModel> lstAdjustmentSuggestion = new List<Sal_AdjustmentSuggestionModel>();
            foreach (var item in lstProfileID)
            {
                Sal_AdjustmentSuggestionEntity adjustmentSuggestionModel = new Sal_AdjustmentSuggestionEntity();
                var basicSalaryEntityByProfileID = lstBasicSalary.Where(s => item == s.ProfileID).FirstOrDefault();

                if (basicSalaryEntityByProfileID != null)
                {
                    var rankEntity = lstRank.Where(s => (basicSalaryEntityByProfileID.OrderNumberRank + int.Parse(model.OrderNumber)) == s.OrderNumber).FirstOrDefault();
                    if (rankEntity != null)
                    {
                        var lstRankDetailByRankID = lstRankDetail.Where(s => s.SalaryClassID == rankEntity.ID).ToList();
                        int total = lstRankDetailByRankID.Count;
                        int count = lstRankDetailByRankID.Count - 1;
                        var orderNumberIndex = total - count;
                        var rankDetailEntity = lstRankDetail.Where(s => s.OrderNumber == orderNumberIndex && rankEntity.ID == s.SalaryClassID).FirstOrDefault();
                        adjustmentSuggestionModel = basicSalaryEntityByProfileID.CopyData<Sal_AdjustmentSuggestionEntity>();
                        if (rankDetailEntity != null)
                        {
                            adjustmentSuggestionModel.ClassRateID = rankDetailEntity.SalaryClassID;
                            adjustmentSuggestionModel.SalaryClassName = rankDetailEntity.SalaryClassName;
                            adjustmentSuggestionModel.RankRateID = rankDetailEntity.ID;
                            adjustmentSuggestionModel.SalaryRankName = rankDetailEntity.SalaryRankName;
                            adjustmentSuggestionModel.GrossAmount = rankDetailEntity.SalaryStandard.ToString();
                            lstAdjustmentSuggestionEntity.Add(adjustmentSuggestionModel);
                        }
                    }

                    //var rankDetailEntity = lstRankDetail.Where(s => s.SalaryClassName == basicSalaryEntityByProfileID.SalaryClassCode && s.OrderNumber == (basicSalaryEntityByProfileID.OrderNumber + int.Parse(model.OrderNumber))).FirstOrDefault();
                    //adjustmentSuggestionModel = basicSalaryEntityByProfileID.CopyData<Sal_AdjustmentSuggestionEntity>();
                    //if (rankDetailEntity != null)
                    //{
                    //    adjustmentSuggestionModel.ClassRateID = rankDetailEntity.SalaryClassID;
                    //    adjustmentSuggestionModel.SalaryClassName = rankDetailEntity.SalaryClassName;
                    //    adjustmentSuggestionModel.RankRateID = rankDetailEntity.ID;
                    //    adjustmentSuggestionModel.SalaryRankName = rankDetailEntity.SalaryRankName;
                    //    adjustmentSuggestionModel.GrossAmount = rankDetailEntity.SalaryStandard.ToString();
                    //}

                }

            }
            if (lstAdjustmentSuggestionEntity.Count > 0)
            {
                lstAdjustmentSuggestion = lstAdjustmentSuggestionEntity.Translate<Sal_AdjustmentSuggestionModel>();
                return Json(lstAdjustmentSuggestion.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        #endregion

        #region Sal_InsuranceSalary
        public ActionResult GetInsuranceSalaryList([DataSourceRequest] DataSourceRequest request, Sal_InsuranceSalarySearchModel model)
        {
            return GetListDataAndReturn<Sal_InsuranceSalaryModel, Sal_InsuranceSalaryEntity, Sal_InsuranceSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_InsuranceSalary);
            //var result = GetListData<Sal_BasicSalaryModel, Sal_BasicSalaryEntity, Sal_BasicSalarySearchModel>(request, payrollModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, ref _status).Translate<Sal_BasicSalaryModel>();
            //var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            //return Json(dataSourceResult);
        }

        public ActionResult ExportAllInsuranceSalaryList([DataSourceRequest] DataSourceRequest request, Sal_InsuranceSalarySearchModel model)
        {
            return ExportAllAndReturn<Sal_InsuranceSalaryModel, Sal_InsuranceSalaryEntity, Sal_InsuranceSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_InsuranceSalary);
        }

        public ActionResult ExportInsuranceSalarySelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_InsuranceSalaryEntity, Sal_InsuranceSalaryModel>(string.Join(",", selectedIds), valueFields, ConstantSql.hrm_sal_sp_get_InsuranceSalaryIds);
        }

        #endregion

        #region Sal_Producttive
        [HttpPost]
        public ActionResult GetSal_Producttive([DataSourceRequest] DataSourceRequest request, Sal_ProductiveSearchModel model)
        {
            return GetListDataAndReturn<Sal_ProductiveModel, Sal_ProductiveEntity, Sal_ProductiveSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_Producttive);
        }
        public ActionResult ExportAllSalProductiveList([DataSourceRequest] DataSourceRequest request, Sal_ProductiveSearchModel model)
        {
            return ExportAllAndReturn<Sal_ProductiveModel, Sal_ProductiveEntity, Sal_ProductiveSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_Producttive);
        }
        public ActionResult ExportSalProductiveList(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_ProductiveEntity, Sal_ProductiveModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_Sal_ProducttiveByIds);
        }
        #endregion

        #region Sal_ProductSalary
        [HttpPost]
        public ActionResult GetSal_ProductSalary([DataSourceRequest] DataSourceRequest request, Sal_ProductSalarySearchModel model)
        {
            return GetListDataAndReturn<Sal_ProductSalaryModel, Sal_ProductSalaryEntity, Sal_ProductSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_ProductSalary);
        }

        [HttpPost]
        public ActionResult ComputeProductSalary(Sal_ProductSalarySearchModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ProductSalarySearchModel>(model, "Sal_ProductSalary", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            Sal_ProductSalaryServices Services = new Sal_ProductSalaryServices();
            try
            {
                DateTime DateStart = new DateTime(model.MonthStart.Value.Year, model.MonthStart.Value.Month, 1);
                DateTime DateEnd = new DateTime(model.MonthStart.Value.Year, model.MonthStart.Value.Month, 1).AddMonths(1).AddDays(-1);

                Services.ComputeProductSalary(model.OrgStructure, model.ProductID, model.ProductItemID, DateStart, DateEnd, UserLogin);
                var result = new object[] { "success", ConstantDisplay.HRM_Sal_ComputeProductSalary_Success.TranslateString() };
                return Json(result);
            }
            catch
            {
                var result = new object[] { "error", ConstantDisplay.HRM_Sal_ComputeProductSalary_Error.TranslateString() };
                return Json(result);
            }
        }
        public ActionResult ExportProductSalarySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_ProductSalaryEntity, Sal_ProductSalaryModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_ProductSalaryByIds);
        }
        #endregion

        #region Sal_Grade
        [HttpPost]
        public ActionResult GetGradeList([DataSourceRequest] DataSourceRequest request, Sal_GradeSearchModel model)
        {
            return GetListDataAndReturn<Sal_GradeModel, Sal_GradeEntity, Sal_GradeSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_Grade);
        }




        public ActionResult ExportAllSalGradeList([DataSourceRequest] DataSourceRequest request, Sal_GradeSearchModel model)
        {
            return ExportAllAndReturn<Sal_GradeModel, Sal_GradeEntity, Sal_GradeSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_Sal_Grade);
        }



        public ActionResult ExportSalGradeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_GradeEntity, Sal_GradeModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_getSal_GradeByIds);
        }
        #endregion

        #region Sal_HoldSalary
        [HttpPost]
        public ActionResult GetHoldSalaryList([DataSourceRequest] DataSourceRequest request, Sal_HoldSalarySearchModel model)
        {
            if (model.IsExcludeQuitEmp == true)
            {
                model.DateQuit = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(model.WorkingPlace))
            {
                model.WorkingPlace = Common.DotNetToOracle(model.WorkingPlace);
            }
            return GetListDataAndReturn<Sal_HoldSalaryModel, Sal_HoldSalaryEntity, Sal_HoldSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_HoldSalary);
        }
        [HttpPost]
        public ActionResult GetComputeHoldSalary([DataSourceRequest] DataSourceRequest request, Sal_HoldSalarySearchModel model)
        {
            if (model.IsExcludeQuitEmp == true)
            {
                model.DateQuit = DateTime.Now;
            }
            var baseService = new ActionService(UserLogin);
            string[] listProfileids = model.ProfileName.Split(',');
            string status = string.Empty;
            ActionService services = new ActionService(UserLogin);
            List<object> ListModel = new List<object>();
            ListModel.AddRange(new object[10]);
            ListModel[8] = 1;
            ListModel[9] = Int32.MaxValue - 1;
            List<Sal_HoldSalaryEntity> listResult = baseService.GetData<Sal_HoldSalaryEntity>(ListModel, ConstantSql.hrm_sal_sp_get_HoldSalary, ref status).Where(m => listProfileids.Contains(m.ID.ToString())).ToList();
            return Json(listResult.ToDataSourceResult(request));

            //return GetListDataAndReturn<Sal_HoldSalaryModel, Sal_HoldSalaryEntity, Sal_HoldSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_HoldSalary);
        }
        public ActionResult ExportAllHoldSalaryList([DataSourceRequest] DataSourceRequest request, Sal_HoldSalarySearchModel model)
        {
            model.ValueFields = model.ValueFields.Replace(",Status", ",StatusTranslate");
            return ExportAllAndReturn<Sal_HoldSalaryEntity, Sal_HoldSalaryModel, Sal_HoldSalarySearchModel>(request, model, ConstantSql.hrm_sal_sp_get_HoldSalary);
        }
        public ActionResult ExportHoldSalarySelected(string selectedIds, string valueFields)
        {
            valueFields = valueFields.Replace(",Status", ",StatusTranslate");
            return ExportSelectedAndReturn<Sal_HoldSalaryEntity, Sal_HoldSalaryModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_HoldSalaryByIds);
        }


        public ActionResult ActionApprovedHoldSalary(List<Guid> selectedIds, string status)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new Sal_HoldSalaryServices();
            services.Approved(selectedIds, status, UserLogin);
            return Json("");
        }

        public ActionResult ActionApprovedLockObject(Guid selectedIds, string status, Guid? workPlaceID, string orderNumber, string userLoginName)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new Sal_ComputePayrollServices();
            services.Approved(selectedIds, status, workPlaceID, orderNumber, userLoginName);
            return Json("");
        }

        /// <summary>
        /// Validate cho màn hình "Phân Tích Nhân Viên Bị Giữ Lương"
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidationComputeHoldSalry(Sal_HoldSalaryModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_HoldSalaryModel>(model, "Sal_AnalyzeHoldSalary", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            else
            {
                var ls = new object[] { "success", message };
                return Json(ls);
            }
            #endregion

        }

        public ActionResult ComputeHoldSalry([DataSourceRequest] DataSourceRequest request, Sal_HoldSalaryModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_HoldSalaryModel>(model, "Sal_AnalyzeHoldSalary", ref message);
            if (!checkValidate)
            {
                return Json(new List<Sal_HoldSalaryModel>());
            }

            var services = new Sal_HoldSalaryServices();
            var result = new List<Sal_HoldSalaryModel>();
            Guid[] _ProfileIDs = null;
            if (model.ProfileIDs != null)
                _ProfileIDs = model.ProfileIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            var listEntity = services.ComputeHoldSalary(model.CutOffDurationID.Value, _ProfileIDs, model.OrgStructureID, (Guid)model.TimeAnalyzeID, UserLogin);
            if (listEntity != null)
            {
                result = listEntity.Translate<Sal_HoldSalaryModel>();
            }
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveHoldSalary(List<Sal_HoldSalaryModel> model)
        {

            return Json("");
        }

        #endregion

        #region Sal_CostCentre
        /// <summary>
        /// Tho.Bui
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSalCostCentreList([DataSourceRequest] DataSourceRequest request, SalCostCentreSearchModel model)
        {
            return GetListDataAndReturn<SalCostCentreModel, Sal_CostCentreSalEntity, SalCostCentreSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_CostCentre);
        }

        /// [Phuoc.Le] - Xuất danh sách dữ liệu cho Mã chi phí (Sal_CostCentre) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSalCostCentreList([DataSourceRequest] DataSourceRequest request, SalCostCentreSearchModel model)
        {
            return ExportAllAndReturn<SalCostCentreModel, Sal_CostCentreSalEntity, SalCostCentreSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_CostCentre);
        }

        /// [Phuoc.Le] - Xuất các dòng dữ liệu được chọn của  Mã chi phí (Sal_CostCentre) theo điều kiện tìm kiếm

        public ActionResult ExportSalCostCentreSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_CostCentreSalEntity, SalCostCentreModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_CostCentreByIds);
        }

        public JsonResult GetMultiCostCentre(string text)
        {
            return GetDataForControl<SalCostCentreMultiModel, Sal_CostCentreSalEntity>(text, ConstantSql.hrm_sal_sp_get_CostCentre_Multi);
        }

        public JsonResult GetCostCentre()
        {
            BaseService baseser = new BaseService();
            var result = baseser.GetAllUseEntity<Sal_CostCentreSalEntity>(ref _status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion Sal_UnusualED

        #region Sal_UnusualED
        [HttpPost]
        public ActionResult GetUnusualEDList([DataSourceRequest] DataSourceRequest request, Sal_UnusualEDSearchModel model)
        {
            return GetListDataAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, Sal_UnusualEDSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_UnusualED);
        }


        /// [Phuoc.Le] - Xuất danh sách dữ liệu choTrợ Cấp (Sal_UnusualED) theo điều kiện tìm kiếm
        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllUnusualEDList([DataSourceRequest] DataSourceRequest request, Sal_UnusualEDSearchModel model)
        {
            return ExportAllAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, Sal_UnusualEDSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_UnusualED);
        }

        /// [Phuoc.Le] - Xuất các dòng dữ liệu được chọn của  Trợ Cấp (Sal_UnusualED) theo điều kiện tìm kiếm

        public ActionResult ExportUnusualEDSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_UnusualEDByIds);
        }



        public ActionResult GetAllowanceEvaluationList([DataSourceRequest] DataSourceRequest request, AllowanceEvaluationYearSearchModel model)
        {
            return GetListDataAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, AllowanceEvaluationYearSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_AllowanceEvaluationYear);
        }

        public ActionResult ExportAllAllowanceEvaluationList([DataSourceRequest] DataSourceRequest request, AllowanceEvaluationYearSearchModel model)
        {
            return ExportAllAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, AllowanceEvaluationYearSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_AllowanceEvaluationYear);
        }

        public ActionResult GetUnusualEDChildCareList([DataSourceRequest] DataSourceRequest request, UnusualEDSearchModel model)
        {
            /*
             * Dù UnusualEDSearchModelTam đễ thay thế UnusualEDSearchModel để không phải sửa nhiều PROCEDURE liên quan.
             */
            #region
            var modeltam = new UnusualEDSearchModelTam();
            if (model != null)
            {
                modeltam.CodeEmp = model.CodeEmp;
                modeltam.ProfileName = model.ProfileName;
                modeltam.Status = model.Status;
                modeltam.DateStart = model.DateStart;
                modeltam.DateEnd = model.DateEnd;
                modeltam.IsExport = model.IsExport;
                modeltam.ValueFields = model.ValueFields;
            }
            #endregion
            return GetListDataAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, UnusualEDSearchModelTam>(request, modeltam, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare);

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllUnusualEDChildCareList([DataSourceRequest] DataSourceRequest request, UnusualEDSearchModel model)
        {
            #region
            var modeltam = new UnusualEDSearchModelTam();
            if (model != null)
            {
                modeltam.CodeEmp = model.CodeEmp;
                modeltam.ProfileName = model.ProfileName;
                modeltam.Status = model.Status;
                modeltam.DateStart = model.DateStart;
                modeltam.DateEnd = model.DateEnd;
                modeltam.IsExport = model.IsExport;
                modeltam.ValueFields = model.ValueFields;
            }
            #endregion
            return ExportAllAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, UnusualEDSearchModelTam>(request, modeltam, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare);
        }

        public ActionResult ExportAllUnusualEDChildCareListBytemplate([DataSourceRequest] DataSourceRequest request, UnusualEDSearchModel model)
        {
            #region
            var modeltam = new UnusualEDSearchModelTam();
            string status = string.Empty;
            if (model != null)
            {
                modeltam.CodeEmp = model.CodeEmp;
                modeltam.ProfileName = model.ProfileName;
                modeltam.Status = model.Status;
                modeltam.DateStart = model.DateStart;
                modeltam.DateEnd = model.DateEnd;
                modeltam.IsExport = model.IsExport;
                modeltam.ValueFields = model.ValueFields;
            }
            #endregion
            #region lstheaderinfo

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "UserCreateName", Value = UserLogin };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DatePrint", Value = DateTime.Now.ToString("dd/MM/yyyy") };
            List<HeaderInfo> lstheaderinfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            #endregion
            if (model != null && model.IsCreateTemplate)
            {

                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Sal_UnusualAllowanceModel(),
                    FileName = "Sal_UnusualAllowance",
                    OutPutPath = path,
                    HeaderInfo = lstheaderinfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = false
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            var ActionServices = new ActionService(UserLogin);
            var result = GetListData<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, UnusualEDSearchModelTam>(request, modeltam, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare, ref status);
            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.YearOfBirth))
                {
                    try
                    {
                        var dt = item.YearOfBirth.Split('/').ToList().Select(s => int.Parse(s)).ToArray();
                        item.YearOld = (((DateTime.Now.Year - dt[2]) * 12 + (DateTime.Now.Month - dt[1])) / 12).ToString();
                        item.MonthOld = (((DateTime.Now.Year - dt[2]) * 12 + (DateTime.Now.Month - dt[1])) % 12).ToString();
                    }
                    catch
                    {
                        item.YearOld = "Error";
                        item.MonthOld = "Error";
                    }
                }
                else
                {
                    item.YearOld = "Error";
                    item.MonthOld = "Error";
                }
            }
            if (model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportID, result, lstheaderinfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }


        // Loi- chua viet sp Multi
        public JsonResult GetMultiUnusual(string text)
        {
            return GetDataForControl<Sal_UnusualEDMutiModel, Sal_UnusualAllowanceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_AccountType_Multi);

        }
        public ActionResult SetStatusApproved(List<Guid> selectedIds, string statusApproved)
        {

            Sal_UnusualAllowanceServices UnusualAllowanceService = new Sal_UnusualAllowanceServices();
            ResultsObject Result = UnusualAllowanceService.UpdateSatus(selectedIds, statusApproved);
            if (!Result.Success)
            {
                return Json(ConstantDisplay.Hrm_Error.Translate());
            }
            return Json("");
        }
        public ActionResult SetIsPaid(List<Guid> selectedIds)
        {
            string status = string.Empty;
            BaseService service = new BaseService();
            List<object> lstObj = new List<object>();
            string _selectedids = string.Join(",", selectedIds);
            lstObj.Add(Common.DotNetToOracle(_selectedids));

            var rs = service.UpdateData<Sal_PayrollTableModel>(lstObj, ConstantSql.hrm_sal_sp_set_PayrollTable_IsPaid, ref status);
            if (status != "")
            {
                return Json(status);
            }
            return Json("");
        }

        #endregion

        #region Compute Payroll

        /// <summary>
        /// [Hien.Nguyen]
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputePayroll([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Hre_ProfileEntity> listProfileAll = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();

            lstModel.AddRange(new object[17]);
            //lstModel[2] = Model.OrgStructureID;
            lstModel[15] = 1;
            lstModel[16] = Int32.MaxValue - 1;
            listProfileAll = Hre_Services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);

            //Lọc theo phòng ban
            if (Model.OrgStructureID != null && Model.OrgStructureID != string.Empty)
            {
                string[] listOrderNumber = Model.OrgStructureID.Split(',');
                listProfileByIds = listProfileAll.Where(m => m.OrderNumber != null && listOrderNumber.Contains(m.OrderNumber.ToString())).ToList();
            }

            if (Model.ListProfileIDs != null && Model.ListProfileIDs.Count > 0)
            {
                listProfileByIds = listProfileByIds.Concat(listProfileAll.Where(m => Model.ListProfileIDs.Contains(m.ID))).ToList();
            }

            //Chỉ lấy nhân viên đang làm việc
            if (Model.isIncludeWorkingEmp)
            {
                listProfileByIds = listProfileByIds.Where(s => s.DateQuit == null).ToList();
            }

            //lọc theo gradepayroll
            if (Model.GradePayrollID != null && Model.GradePayrollID != string.Empty)
            {
                string[] listGrade = Model.GradePayrollID.Split(',');
                lstModel = new List<object>();
                lstModel.AddRange(new object[7]);
                lstModel[5] = 1;
                lstModel[6] = Int32.MaxValue - 1;
                var listSalGrade = Hre_Services.GetData<Sal_GradeEntity>(lstModel, ConstantSql.hrm_sal_sp_get_Sal_Grade, ref status).Where(m => m.GradePayrollID != null && listGrade.Contains(m.GradePayrollID.ToString()));
                listProfileByIds = listProfileByIds.Where(m => listSalGrade.Any(t => t.ProfileID == m.ID)).ToList();
            }

            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            lstModel = new List<object>();
            lstModel.AddRange(new object[10]);
            //lstModel[4] = CutOffDuration.DateStart;
            //lstModel[5] = CutOffDuration.DateEnd;
            lstModel[8] = 1;
            lstModel[9] = Int32.MaxValue - 1;
            List<Sal_HoldSalaryEntity> listHoldSalary = Hre_Services.GetData<Sal_HoldSalaryEntity>(lstModel, ConstantSql.hrm_sal_sp_get_HoldSalary, ref status).Where(m => m.MonthSalary <= CutOffDuration.DateEnd && (m.MonthEndSalary == null || m.MonthEndSalary >= CutOffDuration.DateStart)).ToList();
            listProfileByIds = listProfileByIds.Where(m => !listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();

            //Lọc theo nơi làm việc
            if (!string.IsNullOrEmpty(Model.WorkPlace))
            {
                string[] listWorkPlaceID = Model.WorkPlace.Split(',');
                listProfileByIds = listProfileAll.Where(m => m.WorkPlaceID != null && listWorkPlaceID.Contains(m.WorkPlaceID.ToString())).ToList();
            }

            //Tạo template ghi log
            string Str = string.Format("{0} / {1} / {2} Tính Nghỉ Việc / {3} ", UserLogin, CutOffDuration.CutOffDurationName, Model.isIncludeWorkingEmp == true ? "Có" : "Không", Model.WorkPlaceName);

            #endregion
            var SalService = new Sal_ComputePayrollServices();

            #region Kiểm tra xem bảng lương đã được duyệt hay chưa
            var objLockObject = new List<object>();
            objLockObject.Add(CutOffDuration.DateStart);
            objLockObject.Add(CutOffDuration.DateEnd);
            objLockObject.Add(SysLockObjectType.E_APPROVED_PAYROLL.ToString());
            objLockObject.Add(1);
            objLockObject.Add(int.MaxValue - 1);
            var lstLockObject = actionservice.GetData<Sys_LockObjectEntity>(objLockObject, ConstantSql.hrm_sys_sp_get_LockObject, ref status).Where(m => m.Status == LockObjectStatus.E_APPROVED.ToString());

            int[] ListOrderNumberByProfile = listProfileByIds.Where(m => m.OrderNumber != null).Select(m => (int)m.OrderNumber).Distinct().ToArray();

            foreach (var i in lstLockObject)
            {
                List<int> listOrderNumber = Common.GetListNumbersFromBinary(i.OrgStructures);
                if (listOrderNumber.Any(m => ListOrderNumberByProfile.Contains(m)))
                {
                    return Json(new ResultsObject() { Success = false, Messenger = ConstantDisplay.HRM_Sal_LockObjectByCutOffDurationOrOrgstructure.TranslateString() });
                }
            }
            //nếu chưa duyệt thì lưu thêm 1 dòng
            var services = new Sal_ComputePayrollServices();
            services.Approved(CutOffDuration.ID, LockObjectStatus.E_WAIT_APPROVED.ToString(), null, string.Join(",", ListOrderNumberByProfile), Model.UserLogin);
            #endregion

            Guid result = SalService.ComputePayroll(listProfileByIds, CutOffDuration, Str, UserLogin);

            return Json(result);

        }

        [HttpPost]
        public ActionResult ComputeCommission([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            //Lọc theo phòng ban
            if (Model.OrgStructureID != null && Model.OrgStructureID != string.Empty)
            {
                lstModel.AddRange(new object[18]);
                lstModel[2] = Model.OrgStructureID;
                lstModel[16] = 1;
                lstModel[17] = Int32.MaxValue - 1;
                listProfileByOrg = actionservice.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();
            }

            //lọc theo nhân viên
            if (Model.ProfileIDs != null && Model.ProfileIDs != string.Empty)
            {
                lstModel.AddRange(new object[16]);
                lstModel[14] = 1;
                lstModel[15] = Int32.MaxValue - 1;
                listProfileByIds = Hre_Services.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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

            //Chỉ lấy nhân viên đang làm việc
            if (Model.isIncludeWorkingEmp)
            {
                listProfileByOrg = listProfileByOrg.Where(s => s.DateQuit == null).ToList();
            }

            //có quyết toán nghỉ việc hay ko
            if (Model.PaymentQuit)
            {
                listProfileByOrg = listProfileByOrg.Where(s => s.DateQuit != null && s.StatusSyn == ProfileStatusSyn.E_PAYMENT_QUIT.ToString()).ToList();
            }


            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            //Lọc theo nơi làm việc
            Cat_WorkPlaceEntity WorkPlaceItem = new Cat_WorkPlaceEntity();
            if (Model.WorkPlaceID != null)
            {
                WorkPlaceItem = actionservice.GetByIdUseStore<Cat_WorkPlaceEntity>((Guid)Model.WorkPlaceID, ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status);
                listProfileByOrg = listProfileByOrg.Where(m => m.WorkPlaceID == WorkPlaceItem.ID).ToList();
            }

            //Tạo template ghi log
            string Str = string.Format("{0} / {1} / {2} Tính Nghỉ Việc / {3} ", Common.UserNameSystem, CutOffDuration.CutOffDurationName, Model.isIncludeWorkingEmp == true ? "Có" : "Không", WorkPlaceItem.WorkPlaceName);

            #endregion
            var SalService = new Sal_ComputeCommissionServices();

            Guid result = SalService.ComputeCommission(listProfileByOrg, CutOffDuration, Str, Model.MethodPayroll, Model.CutOffDuration2ID, UserLogin);

            return Json(result);

        }

        //Tính quyết toán nghỉ việc
        public ActionResult ComputePayrollSettlement([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            //Lọc theo phòng ban
            if (Model.OrgStructureID != null && Model.OrgStructureID != string.Empty)
            {
                lstModel.AddRange(new object[17]);
                lstModel[2] = Model.OrgStructureID;
                lstModel[15] = 1;
                lstModel[16] = Int32.MaxValue - 1;
                listProfileByOrg = actionservice.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status).ToList();
            }

            //lọc theo nhân viên
            if (Model.ProfileIDs != null && Model.ProfileIDs != string.Empty)
            {
                lstModel.AddRange(new object[16]);
                lstModel[14] = 1;
                lstModel[15] = Int32.MaxValue - 1;
                listProfileByIds = Hre_Services.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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

            //Lọc ra nhân viên đã đăng ký nghỉ việc
            listProfileByOrg = listProfileByOrg.Where(m => m.DateHire != null && m.IsSettlement != true && m.Settlement == Model.Settlement).ToList();

            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            //lọc nhân viên có thời gian quyết toán trong kỳ
            listProfileByOrg = listProfileByOrg.Where(m => m.MonnthSettlement != null && m.MonnthSettlement.Value.Month == CutOffDuration.MonthYear.Month && m.MonnthSettlement.Value.Year == CutOffDuration.MonthYear.Year).ToList();

            //Lọc theo nơi làm việc
            Cat_WorkPlaceEntity WorkPlaceItem = new Cat_WorkPlaceEntity();
            if (Model.WorkPlaceID != null)
            {
                WorkPlaceItem = actionservice.GetByIdUseStore<Cat_WorkPlaceEntity>((Guid)Model.WorkPlaceID, ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status);
                listProfileByOrg = listProfileByOrg.Where(m => m.WorkPlaceID == WorkPlaceItem.ID).ToList();
            }

            //Tạo template ghi log
            string Str = string.Format("{0} / {1} / {2} Tính Nghỉ Việc / {3} ", UserLogin, CutOffDuration.CutOffDurationName, Model.isIncludeWorkingEmp == true ? "Có" : "Không", WorkPlaceItem.WorkPlaceName);

            #endregion
            var SalService = new Sal_ComputePayrollServices();

            Guid result = SalService.ComputePayroll(listProfileByOrg, CutOffDuration, Str, UserLogin, true);

            return Json(result);
        }

        /// <summary>
        /// Tính giữ lương
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult ComputeHoldSalary([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskModel Model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_AsynTaskModel>(Model, "Sal_HoldSalary", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            else
            {
                var salService = new Sal_ComputePayrollServices();
                ResultsObject result = salService.ComputeHoldSalary(Model.ListIDs, (Guid)Model.CutOffDurationID, UserLogin);
                //var ls = new object[] { "success", "Success".TranslateString() };
                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult ComputeAdvancePayment([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            lstModel.AddRange(new object[18]);
            lstModel[2] = Model.OrgStructureID;
            lstModel[16] = 1;
            lstModel[17] = Int32.MaxValue - 1;
            var profileID = new Guid();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();
            //Xử lý khi chọn nhân viên
            if (Model.ProfileIDs != null)
            {
                if (Model.ProfileIDs.IndexOf(',') > 0)
                {
                    listProfile = Hre_Services.GetData<Hre_ProfileEntity>(Model.ProfileIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
                }
                else
                {
                    profileID = new Guid(Model.ProfileIDs);
                    listProfile = actionservice.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).ToList();
                }

            }
            else
            {
                listProfile = actionservice.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();
            }

            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            #endregion
            var SalService = new Sal_ComputePayrollServices();
            // Guid result = SalService.ComputeAdvancePayment(listProfile, CutOffDuration);

            return Json(Guid.Empty);
        }


        [HttpPost]
        public ActionResult ComputePayrollByProfileID([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            var profileID = new Guid();
            //List<object> lstModel = new List<object>();
            //lstModel.AddRange(new object[12]);
            //lstModel[2] = Model.OrgStructureID;
            //lstModel[10] = 1;
            //lstModel[11] = Int32.MaxValue;
            //var listProfile = Hre_Services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();
            // mượn field Model.OrgStructureID
            if (Model.ProfileID != null)
            {
                profileID = (Guid)Model.ProfileID;
            }
            var profileById = actionservice.GetByIdUseStore<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            listProfile.Add(profileById);
            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            #endregion
            //Tạo template ghi log
            string Str = string.Format("{0} / {1} / {2} Tính Nghỉ Việc / {3} ", UserLogin, CutOffDuration.CutOffDurationName, Model.isIncludeWorkingEmp == true ? "Có" : "Không", "");

            var SalService = new Sal_ComputePayrollServices();
            Guid result = SalService.ComputePayroll(listProfile, CutOffDuration, Str, UserLogin);

            return Json(result);
        }
        [HttpPost]
        public ActionResult ComputeCommissionByProfileID([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            var profileID = new Guid();
            //List<object> lstModel = new List<object>();
            //lstModel.AddRange(new object[12]);
            //lstModel[2] = Model.OrgStructureID;
            //lstModel[10] = 1;
            //lstModel[11] = Int32.MaxValue;
            //var listProfile = Hre_Services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();
            // mượn field Model.OrgStructureID
            if (Model.ProfileID != null)
            {
                profileID = (Guid)Model.ProfileID;
            }
            var profileById = actionservice.GetByIdUseStore<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            listProfile.Add(profileById);
            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            #endregion
            //Tạo template ghi log
            string Str = string.Format("{0} / {1} / {2} Tính Nghỉ Việc / {3} ", Common.UserNameSystem, CutOffDuration.CutOffDurationName, Model.isIncludeWorkingEmp == true ? "Có" : "Không", "");

            //var SalService = new Sal_ComputePayrollServices();
            //Guid result = SalService.ComputePayroll(listProfile, CutOffDuration, Str);
            var SalService = new Sal_ComputeCommissionServices();

            Guid result = SalService.ComputeCommission(listProfile, CutOffDuration, Str, Model.MethodPayroll, Model.CutOffDuration2ID, UserLogin);


            return Json(result);
        }

        [HttpPost]
        public ActionResult ComputeAdvancePaymentByProfileID([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskComputeModel Model)
        {
            #region Get Data
            var Hre_Services = new ActionService(UserLogin);
            ActionService actionservice = new ActionService(UserLogin);
            string status = string.Empty;
            var profileID = new Guid();
            //List<object> lstModel = new List<object>();
            //lstModel.AddRange(new object[12]);
            //lstModel[2] = Model.OrgStructureID;
            //lstModel[10] = 1;
            //lstModel[11] = Int32.MaxValue;
            //var listProfile = Hre_Services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();
            // mượn field Model.OrgStructureID
            if (!string.IsNullOrEmpty(Model.OrgStructureID))
            {
                profileID = new Guid(Model.OrgStructureID);
            }
            var profileById = actionservice.GetByIdUseStore<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            listProfile.Add(profileById);
            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            #endregion
            var SalService = new Sal_ComputePayrollServices();
            //Guid result = SalService.ComputeAdvancePayment(listProfile, CutOffDuration);

            return Json(Guid.Empty);
        }
        #endregion

        #region Config Element

        [HttpPost]
        public ActionResult GetConfigElement(string key)
        {
            string status = string.Empty;
            Sys_AllSettingServices sysServices = new Sys_AllSettingServices();

            Sys_AllSettingModel result = new Sys_AllSettingModel();
            result.Name = key;
            var rs = sysServices.GetData<Sys_AllSettingModel>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).FirstOrDefault();
            if (rs != null)
                return Json(rs);
            return Json(result);
        }

        public ActionResult SaveConfigElement(string ID, string Key, string Value)
        {
            Guid configID = Guid.Empty;
            Guid.TryParse(ID, out configID);

            string status = string.Empty;
            Sys_AllSettingServices sysServices = new Sys_AllSettingServices();
            Sys_AllSettingEntity model = new Sys_AllSettingEntity();
            if (configID == Guid.Empty)
            {
                model.Name = Key;
                model.Value1 = Value;
                status = sysServices.Add(model);
            }
            else
            {
                model.ID = configID;
                model.Name = Key;
                model.Value1 = Value;
                status = sysServices.Edit(model);
            }

            if (status == "Success")
            {
                status += "/" + Value;
            }
            return Json(status);

        }

        public ActionResult SaveConfigEstimate(string ID, string Key, string Value1, string Value2)
        {
            Guid configID = Guid.Empty;
            Guid.TryParse(ID, out configID);

            string status = string.Empty;
            Sys_AllSettingServices sysServices = new Sys_AllSettingServices();
            Sys_AllSettingEntity model = new Sys_AllSettingEntity();
            if (configID == Guid.Empty)
            {
                model.Name = Key;
                model.Value1 = Value1;
                model.Value2 = Value2;
                status = sysServices.Add(model);
            }
            else
            {
                model.ID = configID;
                model.Name = Key;
                model.Value1 = Value1;
                model.Value2 = Value2;
                status = sysServices.Edit(model);
            }

            if (status == "Success")
            {
                status += "/" + Value1 + "," + Value2;
            }
            return Json(status);
        }

        #endregion

        #region Report
        public ActionResult ReportSalaryTable([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {

            string status = string.Empty;

            Sal_ReportService service = new Sal_ReportService();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            List<object> lstModel = new List<object>();
            lstModel.AddRange(new object[4]);
            lstModel[2] = 1;
            lstModel[3] = Int32.MaxValue - 1;
            var listPayrollGroupID = service1.GetData<Cat_PayrollGroupEntity>(lstModel, ConstantSql.hrm_cat_sp_get_payrollGroup, ref status).Select(m => m.ID).ToList();

            var attCutOffDurationServices = new Att_CutOffDurationServices();
            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            DateTime _dateStart = DateTime.Now;
            DateTime _dateEnd = DateTime.Now;
            if (cutOffDurationEntity != null)
            {
                _dateStart = cutOffDurationEntity.DateStart;
                _dateEnd = cutOffDurationEntity.DateEnd;
            }
            Guid[] rankID = null;
            if (!string.IsNullOrEmpty(model.RankID))
            {
                rankID = model.RankID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            Guid[] workPlaceID = null;
            if (!string.IsNullOrEmpty(model.WorkingPlaceIDs))
            {
                workPlaceID = model.WorkingPlaceIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            Guid[] costcenterIds = null;
            if (!string.IsNullOrEmpty(model.CostCenterIds))
            {
                costcenterIds = model.CostCenterIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            var lstProfiles = service1.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);

            //lấy user login
            var UserExport = lstProfiles.FirstOrDefault(m => m.ID == model.UserID);

            List<Guid> lstProfileIDs = null;
            //List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();
            if (rankID != null)
            {
                lstProfiles = lstProfiles.Where(s => s.SalaryClassID != null && rankID.Contains(s.SalaryClassID.Value)).ToList();
            }
            if (model.StatusSyn == StatusEmployee.E_ALL.ToString() || model.StatusSyn == null)
            {
                lstProfileIDs = lstProfiles.Select(s => s.ID).ToList();
            }
            else
            {
                if (model.StatusSyn == StatusEmployee.E_WORKING.ToString())
                {
                    lstProfiles = lstProfiles.Where(pro => (pro.DateQuit == null || pro.DateQuit >= _dateEnd) && pro.DateHire < _dateStart).ToList();
                }
                else if (model.StatusSyn == StatusEmployee.E_NEWEMPLOYEE.ToString())
                {
                    lstProfiles = lstProfiles.Where(pro => pro.DateHire <= _dateEnd && pro.DateHire >= _dateStart).ToList();
                }
                else if (model.StatusSyn == StatusEmployee.E_STOPWORKING.ToString())
                {
                    lstProfiles = lstProfiles.Where(pro => pro.DateQuit != null && pro.DateQuit.Value.Date <= _dateEnd.Date && pro.DateQuit.Value.Date >= _dateStart.Date).ToList();
                }
                else if (model.StatusSyn == StatusEmployee.E_WORKINGANDNEW.ToString())
                {
                    lstProfiles = lstProfiles.Where(pro => pro.DateQuit == null || pro.DateQuit >= _dateEnd).ToList();
                }
                lstProfileIDs = lstProfiles.Select(s => s.ID).ToList();
            }
            if (lstProfileIDs != null)
                lstProfiles = lstProfiles.Where(s => lstProfileIDs.Contains(s.ID)).ToList();

            DataTable result = service.RefreshData(cutOffDurationEntity.DateStart, cutOffDurationEntity.DateEnd, cutOffDurationEntity.MonthYear, lstProfiles, model.GradePayrollID, model.isIncludeQuitEmp, model.CodeEmp, model.OrgStructureID, model.isIncludeTransfer, workPlaceID, costcenterIds, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutOffDurationEntity.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutOffDurationEntity.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkingPlaceID", Value = model.WorkingPlace != null ? model.WorkingPlace : "" };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "UserExport", Value = UserExport != null ? UserExport.CodeEmp : "" };

            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportBasicSalaryMonthlyModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                if (model.isExportGroup)
                {
                    ExportService exportService = new ExportService();
                    exportService.BeforeExport += OnBeforeExportSalary;
                    string fileName = exportService.ExportByTemplate(string.Empty, model.ExportId, result);
                    fileName = Common.MultiExport(ConstantPathWeb.Hrm_Hre_Service + Common.DownloadURL, false, fileName);
                    return Json(fileName);
                }

                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            //return Json(result.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private void OnBeforeExportSalary(BeforeExportEventArgs e)
        {
            if (e.ExportTemplates != null)
            {
                var firstTemplate = e.ExportTemplates.FirstOrDefault();
                var dataSource = e.ExportTemplates.Select(d => d.DataSource).FirstOrDefault();
                var dtSalaryInfo = dataSource.IsTypeOf(typeof(DataTable)) ? (DataTable)dataSource : null;
                List<ExportTemplate> listExportTemplate = new List<ExportTemplate>();

                if (dtSalaryInfo != null && firstTemplate != null)
                {
                    var listCostRow = dtSalaryInfo.Rows.OfType<DataRow>().GroupBy(d => d["CostCenterCode"]).ToList();

                    if (string.IsNullOrWhiteSpace(firstTemplate.FileName))
                    {
                        if (!string.IsNullOrWhiteSpace(firstTemplate.TemplateFile))
                        {
                            string fileName = Utilities.GetFileNameWithoutExtension(firstTemplate.TemplateFile);
                            string fileExtension = Utilities.GetExtension(firstTemplate.TemplateFile);
                            string downloadPath = Common.GetPath(Common.DownloadURL);
                            string fileSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                            downloadPath += "\\" + fileName + "_" + fileSuffix + fileExtension;
                            downloadPath = downloadPath.Replace("/", "\\");
                            firstTemplate.FileName = downloadPath;
                        }
                    }

                    foreach (var costRow in listCostRow)
                    {
                        var listProfileRow = costRow.GroupBy(d => d["CodeEmp"]).ToList();
                        int sheetIndex = 0;

                        foreach (var profileRow in listProfileRow)
                        {
                            ExportTemplate tempalte = new ExportTemplate();
                            firstTemplate.CopyData(tempalte, "DataSource");
                            listExportTemplate.Add(tempalte);

                            tempalte.TargetSheetIndex = sheetIndex++;
                            tempalte.TargetSheetName = profileRow.Key.GetString();

                            string filePath = Utilities.GetDirectoryPath(tempalte.FileName);
                            string fileExtension = Utilities.GetExtension(tempalte.FileName);
                            string fileName = Utilities.GetFileNameWithoutExtension(tempalte.FileName);
                            fileName = fileName + "_" + costRow.Key.GetString() + fileExtension;
                            tempalte.FileName = filePath + "\\" + fileName;

                            DataTable dt = profileRow.ToList().Translate();
                            tempalte.DataSource = dt;
                        }
                    }
                }

                e.ExportTemplates = listExportTemplate.ToArray();
            }
        }

        public ActionResult ReportSalaryStaffNumber([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.Sal_ReportHeadCount((DateTime)model.DateFrom, (DateTime)model.DateTo, model.OrgStructureID, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };

            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportHeadCount",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportComparePayroll([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            Guid[] _cutOffIds = null;
            if (!string.IsNullOrEmpty(model.CutOffDurationIds))
            {
                _cutOffIds = model.CutOffDurationIds.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }

            DataTable Table = Services.GetReportComparePayroll(_cutOffIds, UserLogin, model.OrgStructureID, model.WorkingPlaceID);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            //Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            //if (model.UserID != null)
            //{
            //    profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            //}
            //HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            //HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            //HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };

            // List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportComparePayrollEntity",
                    OutPutPath = path,
                    //   HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, null, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportCompareUnusualAllowance([DataSourceRequest] DataSourceRequest request, Sal_CompareUnusualAllowanceModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            var baseService = new BaseService();
            string cutoff = Common.DotNetToOracle(model.CutOffDurationID.ToString());
            var objAtt_CutOffDurationEntity = baseService
           .GetData<Att_CutOffDurationEntity>(cutoff, ConstantSql.hrm_att_sp_get_CutOffDurationById, UserLogin, ref status)
           .FirstOrDefault();
            DateTime _DateFrom = DateTime.MinValue;
            DateTime _DateTo = DateTime.MaxValue;
            if (objAtt_CutOffDurationEntity != null)
            {
                //DateTime _currenDateFrom=objAtt_CutOffDurationEntity.DateStart;
                //_DateFrom = _currenDateFrom.AddMonths(-1);
                _DateFrom = objAtt_CutOffDurationEntity.DateStart;
                _DateTo = objAtt_CutOffDurationEntity.DateEnd;
            }


            DataTable result = Services.GetCompareUnusualAllowance(_DateFrom, _DateTo, model.OrgStructureID, model.WorkingPlaceID, model.StatusSyn, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = _DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = _DateFrom.AddMonths(-1) };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkingPlaceID", Value = model.WorkingPlace != null ? model.WorkingPlace : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_CompareUnusualAllowanceEntity",
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
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            #endregion

            //var isDataTable = false;
            //object obj = new DataTable();
            //if (model.IsCreateTemplateForDynamicGrid)
            //{
            //    var col = result.Columns.Count;
            //    result.Columns.RemoveAt(col - 1);
            //    obj = result;
            //    isDataTable = true;
            //}
            //if (model != null && model.IsCreateTemplate)
            //{
            //    var path = Common.GetPath("Templates");
            //    ExportService exportService = new ExportService();
            //    ConfigExport cfgExport = new ConfigExport()
            //    {
            //        Object = obj,
            //        FileName = "Sal_CompareUnusualAllowanceEntity",
            //        OutPutPath = path,
            //        DownloadPath = Hrm_Main_Web + "Templates",
            //        IsDataTable = isDataTable

            //    };
            //    var str = exportService.CreateTemplate(cfgExport);
            //    return Json(str);
            //}
            //if (model.ExportId != Guid.Empty)
            //{
            //    var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
            //    return Json(fullPath);
            //}
            //return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
        }

        public ActionResult ReportProfileNotDeductMoney([DataSourceRequest] DataSourceRequest request, Sal_CompareUnusualAllowanceModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_CompareUnusualAllowanceModel>(model, "Sal_ReportProfileNotDeductMoney", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }

            Sal_ReportService Services = new Sal_ReportService();
            List<Hre_ProfileEntity> ListProfile = new List<Hre_ProfileEntity>();
            ActionService service = new ActionService(UserLogin);
            ActionService ActionServices = new ActionService(UserLogin);

            Att_CutOffDurationEntity CutOffDuration = new Att_CutOffDurationEntity();
            if (model.CutOffDurationID != null)
            {
                CutOffDuration = ActionServices.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref message);
            }

            List<object> lstModel = new List<object>();
            lstModel.AddRange(new object[18]);
            lstModel[2] = model.OrgStructureID;
            lstModel[16] = 1;
            lstModel[17] = Int32.MaxValue - 1;
            ListProfile = ActionServices.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref message);

            //lọc theo khu vực
            if (model.WorkingPlaceID != null)
            {
                ListProfile = ListProfile.Where(m => m.WorkPlaceID == model.WorkingPlaceID).ToList();
            }

            //lọc theo trạng thái nhân viên
            if (model.StatusSyn != null && model.StatusSyn != string.Empty)
            {
                ListProfile = ListProfile.Where(m => m.StatusSyn == model.StatusSyn).ToList();
            }

            DataTable Table = Services.ReportProfileNotDeductMoney(ListProfile, CutOffDuration, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkingPlace", Value = model.WorkingPlace };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportProfileNotDeductMoney",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportFilialWedding([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "Sal_ReportFilialWedding", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportFilialWedding((DateTime)model.DateFrom, (DateTime)model.DateTo, model.OrgStructureID, model.UnusualEDTypeID, model.isIncludeQuitEmp, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo4, headerInfo3 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportFilialWedding",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportChildCare([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            DataTable Table = Services.ReportChildCare((DateTime)model.DateFrom, model.OrgStructureID, UserLogin, model.IsCreateTemplate);

            //lấy user đang đăng nhập
            string CodeEmp = string.Empty;
            string status = string.Empty;
            Hre_ProfileEntity Profile = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                Profile = service1.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "ProfileName", Value = Profile != null && Profile.CodeEmp != null ? Profile.CodeEmp : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportChildCare",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ReportTotalUnusualAllowance([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            DataTable Table = Services.ReportTotalUnusualAllowance((DateTime)model.DateFrom, (DateTime)model.DateTo, model.OrgStructureID, UserLogin, model.IsCreateTemplate);

            //lấy user đang đăng nhập
            string CodeEmp = string.Empty;
            string status = string.Empty;
            Hre_ProfileEntity Profile = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                Profile = service1.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "CodeEmp", Value = Profile != null && Profile.CodeEmp != null ? Profile.CodeEmp : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalUnusualAllowance",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ReportTotalProfileUnusualAllowance([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "ReportTotalProfileUnusualAllowance", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion


            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportTotalProfileUnusualAllowance((DateTime)model.DateFrom, (DateTime)model.DateTo, model.OrgStructureID, model.isIncludeQuitEmp, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalProfileUnusualAllowance",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult ReportProfileEntitledAllowance([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "ReportProfileEntitledAllowance", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportProfileEntitledAllowance((DateTime)model.DateFrom, (DateTime)model.DateTo, model.ListProfileIDs, model.OrgStructureID, UserLogin, model.isIncludeQuitEmp);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportProfileEntitledAllowance",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportTotalAnnualTaxableIncome([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(Model, "Sal_RemittanceAllowSick", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            DataTable Table = Services.ReportTotalAnnualTaxableIncome((DateTime)Model.MonthYear, Model.WorkPlace.Split(',').ToList(), Model.ProfileIDs.Split(',').ToList(), Model.OrgStructureID, Model.ElementType.Split(',').ToList(), UserLogin, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (Model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)Model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "YearExport", Value = Model.MonthYear };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkingPlace", Value = Model.WorkingPlace != null ? Model.WorkingPlace : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalAnnualTaxableIncome",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Kai_ReportPaymentoutList([DataSourceRequest] DataSourceRequest request, Kai_ReportPaymentoutSearchModel Model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Kai_ReportPaymentoutEntity> listProfileByOrg = new List<Kai_ReportPaymentoutEntity>();
            List<Kai_ReportPaymentoutEntity> listProfileByIds = new List<Kai_ReportPaymentoutEntity>();


            lstModel.AddRange(new object[7]);
            lstModel[0] = Model.OrgStructureID;
            lstModel[1] = Model.CodeEmp;
            lstModel[2] = Model.ProfileName;
            lstModel[3] = Model.DateFrom;
            lstModel[4] = Model.DateTo;
            lstModel[5] = 1;
            lstModel[6] = Int32.MaxValue - 1;
            listProfileByOrg = services.GetData<Kai_ReportPaymentoutEntity>(lstModel, ConstantSql.hrm_sal_sp_get_KaiReportPaymentout, ref status);

            DataTable Table = Services.Kai_ReportPaymentoutList(listProfileByOrg, (DateTime)Model.DateFrom, (DateTime)Model.DateTo, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            //ActionService ActionServices = new ActionService(UserLogin);

            //Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            //if (Model.UserID != null)
            //{
            //    profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)Model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            //}
            //HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = Model.DateTo };
            //  HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkingPlace", Value = Model.WorkingPlace != null ? Model.WorkingPlace : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo4 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Kai_ReportPaymentoutSearchModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (Model.ExportID != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportID, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportPensions([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel Model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(Model, "Sal_ReportPensions", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();

            lstModel.AddRange(new object[18]);
            lstModel[2] = Model.OrgStructureID;
            lstModel[16] = 1;
            lstModel[17] = Int32.MaxValue - 1;
            listProfileByOrg = services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status);

            //lọc theo nhân viên
            if (Model.ProfileIDs != null && Model.ProfileIDs != string.Empty)
            {
                Guid[] ListProfileID = Model.ProfileIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
                listProfileByOrg = listProfileByOrg.Where(m => ListProfileID.Any(t => t == m.ID)).ToList();
            }

            DataTable Table = Services.ReportPensions(listProfileByOrg, (DateTime)Model.DateFrom, UserLogin, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (Model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)Model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportPensions",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }

            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportAllowanceStopWorking([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel Model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            Guid[] lstProfileIDs = null;
            if (!string.IsNullOrEmpty(Model.ProfileIDs))
            {
                lstProfileIDs = Model.ProfileIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }


            lstModel.AddRange(new object[18]);
            lstModel[2] = Model.OrgStructureID;
            lstModel[16] = 1;
            lstModel[17] = Int32.MaxValue - 1;
            listProfileByOrg = services.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_hr_sp_get_Profile, ref status);

            if (lstProfileIDs != null)
            {
                listProfileByOrg = listProfileByOrg.Where(s => lstProfileIDs.Contains(s.ID)).ToList();
            }

            if (Model.WorkingPlaceID != null)
            {
                listProfileByOrg = listProfileByOrg.Where(s => s.WorkPlaceID != null && s.WorkPlaceID == Model.WorkingPlaceID.Value).ToList();
            }

            DataTable Table = Services.ReportAllowanceStopWorking(listProfileByOrg, (DateTime)Model.DateFrom, Model.ElementType.Replace(",", ""), Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (Model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)Model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkingPlace", Value = Model.WorkingPlace };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalAnnualTaxableIncome",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportNotPayAllowancesByHoldSalary([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel Model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            DataTable Table = Services.ReportNotPayAllowancesByHoldSalary((DateTime)Model.DateFrom, (DateTime)Model.DateTo, Model.OrgStructureID, UserLogin, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }

            //lấy user đang đăng nhập
            string CodeEmp = string.Empty;
            string status = string.Empty;
            Hre_ProfileEntity Profile = new Hre_ProfileEntity();
            if (Model.UserID != null)
            {
                Profile = service1.GetByIdUseStore<Hre_ProfileEntity>((Guid)Model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = Model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = Profile != null && Profile.CodeEmp != null ? Profile.CodeEmp : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportNotPayAllowancesByHoldSalary",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }
            SortDescriptor Sort1=new SortDescriptor("CodeEmp",ListSortDirection.Ascending);
            request.Sorts = new List<SortDescriptor>() { Sort1 };
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportSalaryElement([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            string[] ArrayElement = model.ElementType.Split(',');
            DataTable Table = Services.Sal_ReportSalaryElement((DateTime)model.DateFrom, (DateTime)model.DateTo, model.OrgStructureID, ArrayElement, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportSalaryElement",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable,

                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Sal_ReportPlanningPayroll([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            List<Cat_WorkPlaceEntity> WorkPlace = new List<Cat_WorkPlaceEntity>();
            string status = string.Empty;
            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            List<string> WorkingPlace = new List<string>();
            if (model.WorkingPlaceIDs != null)
            {
                WorkingPlace = model.WorkingPlaceIDs.Split(',').ToList();
            }

            List<string> SalaryClassName = new List<string>();
            if (model.SalaryClassIDs != null)
            {
                SalaryClassName = model.SalaryClassIDs.Split(',').ToList();
            }

            //DataTable Table = Services.ReportPlanningPayroll((Guid)model.CutOffDurationID, model.CostCenter, model.ElementType);
            DataTable Table = Services.ReportPlanningPayroll((Guid)model.CutOffDurationID, model.CostCenter, model.ElementType, model.OrgStructureID, WorkingPlace, SalaryClassName, model.StatusSyn, model.isIncludeQuitEmp, UserLogin, model.IsCreateTemplate);

            if (model.IsCreateTemplate || model.ExportId != Guid.Empty)
            {
                List<object> lstModel = new List<object>();
                lstModel.AddRange(new object[3]);
                lstModel[1] = 1;
                lstModel[2] = Int32.MaxValue - 1;
                WorkPlace = service1.GetData<Cat_WorkPlaceEntity>(lstModel, ConstantSql.hrm_cat_sp_get_WorkPlace, ref status);
                WorkPlace = WorkPlace.Where(m => WorkingPlace.Any(t => t == m.ID.ToString())).ToList();
            }

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutOffDurationEntity.MonthYear };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkPlace", Value = string.Join(",", WorkPlace.Select(m => m.WorkPlaceName).ToArray()) };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportPlanningPayroll",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Sal_ReportAmountOTandUnpadLeave([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            string status = string.Empty;
            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);


            DataTable Table = Services.ReportAmountOTandUnpadLeave(model.OrgStructureID, model.ProfileIDs, (Guid)model.CutOffDurationID, model.ElementType, model.WorkingPlaceID, model.StatusSyn, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutOffDurationEntity.MonthYear };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportAmountOTandUnpadLeave",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ReportTotalIncomeByProfile([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportTotalIncomeByProfile(Guid.Parse(model.ProfileIDs), (DateTime)model.DateFrom, (DateTime)model.DateTo, (Guid)model.GradeID, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalIncomeByProfile",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportAccountingOfClearing([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "Sal_ReportAccountingOfClearing", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            string status = string.Empty;
            Sal_ReportService Services = new Sal_ReportService();
            ActionService action = new ActionService(UserLogin);

            var cutOffDurationEntity = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            if (cutOffDurationEntity == null)
            {
                var ls = new object[] { "error", "Không có dữ liệu kỳ lương !" };
                return Json(ls);
            }

            DataTable Table = Services.ReportAccountingOfClearing(cutOffDurationEntity, model.OrgStructureID, model.WorkingPlaceID, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutOffDurationEntity.DateStart };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "DateTo", Value = cutOffDurationEntity.DateEnd };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlace != null ? model.WorkingPlace : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportAccountingOfClearing",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ReportHaveChildUnusual([DataSourceRequest] DataSourceRequest request, Sal_ReportHaveChildUnusualModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();

            DataTable Table = Services.GetReportHaveChildUsunual(model.OrgStructureID, model.DateFrom, model.DateTo, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }

            Hre_ProfileEntity Profile = new Hre_ProfileEntity();
            ActionService service1 = new ActionService(UserLogin);
            string status = string.Empty;
            if (model.UserID != null)
            {
                Profile = service1.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "CodeEmp", Value = Profile.CodeEmp };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportHaveChildUsunualEntity",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }





        public ActionResult ComparePayrollValiedate(Sal_ComparePayrollModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ComparePayrollModel>(model, "Sal_ComparePayroll", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        public ActionResult ComparePayroll([DataSourceRequest] DataSourceRequest request, Sal_ComparePayrollModel model)
        {

            string status = string.Empty;

            Sal_ReportService service = new Sal_ReportService();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            Guid[] _CutOffDurationIDs = null;
            if (model.CutOffDurationIDs != null)
                _CutOffDurationIDs = model.CutOffDurationIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _ElementIDs = null;
            if (model.ElementIDs != null)
                _ElementIDs = model.ElementIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();

            Guid[] _WorkingPlace = null;
            if (model.WorkingPlaceID != null)
                _WorkingPlace = model.WorkingPlaceID.Split(',').Select(s => Guid.Parse(s)).ToArray();

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Guid> lstProfileIDs = service1.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            DataTable result = service.ComparePayroll(_CutOffDurationIDs, _ElementIDs, model.OrgStructureID, model.OrgStructureTypeID, model.IsCreateTemplate, model.CompareType, model.ShowDataType, _WorkingPlace, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
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
                    FileName = "Sal_ComparePayrollModel",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {


                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }

        #region RptCostCentreByOrg
        public ActionResult ReportCostcentreByOrgValiedate(Sal_ComparePayrollModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ComparePayrollModel>(model, "Sal_ReportCostCentreByOrg", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportCostCentreByOrg([DataSourceRequest] DataSourceRequest request, Sal_ReportCostCentreByOrgModel model)
        {

            string status = string.Empty;
            ActionService ActionServices = new ActionService(UserLogin);
            Sal_ReportService service = new Sal_ReportService();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            Guid[] _CutOffDurationIDs = null;
            if (model.CutOffDurationIDs != null)
                _CutOffDurationIDs = model.CutOffDurationIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _ElementIDs = null;
            if (model.ElementIDs != null)
                _ElementIDs = model.ElementIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();


            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Guid> lstProfileIDs = ActionServices.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            DataTable result = service.GetReportCostCentreByOrg(_CutOffDurationIDs, _ElementIDs, model.OrgStructureID, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;

            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>(model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }



            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportCostCentreByOrgEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable,
                    HeaderInfo = listHeaderInfo
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }

        #endregion

        public ActionResult ReportSalaryMonthly([DataSourceRequest] DataSourceRequest request, Sal_ReportSalaryMonthlyModel model)
        {
            string status = string.Empty;
            Sal_ReportService service = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);
            var attCutOffDurationServices = new Att_CutOffDurationServices();

            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

            List<Guid> lstProfileIDs = service1.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            DataTable result = service.ReportUnusualPay(cutOffDurationEntity.MonthYear, lstProfileIDs, model.PayrollGroupID, model.isIncludeQuitEmp, model.CodeEmp, model.OrgStructureID, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportSalaryMonthlyModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult ReportRevenueForShop([DataSourceRequest] DataSourceRequest request, Sal_ReportRevenueForShopsModel model)
        {
            string status = string.Empty;
            Sal_ReportService service = new Sal_ReportService();
            ActionService service1 = new ActionService(UserLogin);

            DataTable result = service.GetReportRevenueForShop(model.DateFrom, model.DateTo, model.OrgStructureID, UserLogin, model.IsCreateTemplate);


            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateTime.Now };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportRevenueForShopsModel",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }

            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetReportGroupPayrollCostCentreValidate(Sal_ReportGroupPayrollCostCentreModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportGroupPayrollCostCentreModel>(model, "Sal_ReportGroupPayrollCostCentre", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
                //return Json(message);
            }
            #endregion
            return Json(message);

        }
        public ActionResult GetReportGroupPayrollCostCentre([DataSourceRequest] DataSourceRequest request, Sal_ReportGroupPayrollCostCentreModel model)
        {
            var service = new Sal_ReportService();
            ActionService ActionServices = new ActionService(UserLogin);
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
            List<Hre_ProfileEntity> lstProfileIDs = ActionServices.GetData<Hre_ProfileEntity>(strorgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
            var result = service.GetReportGroupPayrollCostCentre(From, To, lstProfileIDs, model.ElementType, UserLogin, model.IsCreateTemplate);
            if (model.IsCreateTemplateForDynamicGrid)
            {

                result.Columns.RemoveAt(2);
                obj = result;
                isDataTable = true;
            }
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>(model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }



            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportGroupPayrollCostCentreModel",
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
                var fullPath = ExportService.Export(model.ExportId, result);
                return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(result.ToDataSourceResult(request));
        }


        #endregion

        [HttpPost]
        public ActionResult GetSal_ItemForShopList([DataSourceRequest] DataSourceRequest request, Sal_ItemForShopSearchModel model)
        {
            return GetListDataAndReturn<Sal_ItemForShopModel, Sal_ItemForShopEntity, Sal_ItemForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_ItemForShop);
        }

        public ActionResult GetSal_Sal_RevenueForProfileList([DataSourceRequest] DataSourceRequest request, Sal_RevenueForProfileSearchModel model)
        {
            return GetListDataAndReturn<Sal_RevenueForProfileModel, Sal_RevenueForProfileEntity, Sal_RevenueForProfileSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_RevenueForProfile);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSal_ItemForShopList([DataSourceRequest] DataSourceRequest request, Sal_ItemForShopSearchModel model)
        {
            return ExportAllAndReturn<Sal_ItemForShopModel, Sal_ItemForShopEntity, Sal_ItemForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_ItemForShop);
        }

        [HttpPost]
        public ActionResult GetSal_LineItemForShopList([DataSourceRequest] DataSourceRequest request, Sal_LineItemForShopSearchModel model)
        {
            return GetListDataAndReturn<Sal_LineItemForShopModel, Sal_LineItemForShopEntity, Sal_LineItemForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_LineItemForShop);
        }

        [HttpPost]
        public ActionResult GetSal_RevenueRecordList([DataSourceRequest] DataSourceRequest request, Sal_RevenueRecordSearchModel model)
        {
            var revenueRecordServices = new Sal_RevenueRecordService();
            var actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.AddRange(new object[4]);
            lstObj[0] = model.Month;
            lstObj[1] = model.ShopID;
            lstObj[2] = 1;
            lstObj[3] = int.MaxValue - 1;
            var lstRevenueRecord = actionServices.GetData<Sal_RevenueRecordEntity>(lstObj, ConstantSql.hrm_sal_sp_get_RevenueRecord, ref status).ToList();
            foreach (var i in lstRevenueRecord.Where(m => m.KPIBonusID == null))
            {
                if (i.Type == EnumDropDown.SalesType.E_ITEM_MAJOR.ToString())
                {
                    i.KPIBonusName = "Sản Phẩm Đẩy Mạnh";
                }
                if (i.Type == EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString())
                {
                    i.KPIBonusName = "Dòng Sản Phẩm Chủ Đạo";
                }
            }

            return Json(lstRevenueRecord.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataByShopID(string ShopID, DateTime? Month)
        {


            string status = string.Empty;
            var shopID = Guid.Empty;
            if (string.IsNullOrEmpty(ShopID))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(ShopID))
            {
                shopID = Common.ConvertToGuid(ShopID);
            }

            var lstObj = new List<object>();
            //lstObj.Add(null);
            //lstObj.Add(null);
            //lstObj.Add(null);
            //lstObj.Add(1);
            //lstObj.Add(int.MaxValue - 1);
            //var OrgServices = new Cat_OrgStructureServices();
            //var listOrg = OrgServices.GetData<Cat_OrgStructureEntity>(lstObj, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(null);
            lstObj.Add(null);
            lstObj.Add(1);
            lstObj.Add(int.MaxValue - 1);
            var ShopServices = new Cat_ShopServices();
            var services = new ActionService(UserLogin);
            var listShop = services.GetData<Cat_ShopEntity>(lstObj, ConstantSql.hrm_cat_sp_get_Shop, ref status).ToList();



            lstObj = new List<object>();
            lstObj.Add(Month);
            lstObj.Add(shopID);
            lstObj.Add(1);
            lstObj.Add(int.MaxValue - 1);
            var revenueRecordServices = new Sal_RevenueRecordService();

            var lstRevenueRecord = services.GetData<Sal_RevenueForShopModel>(lstObj, ConstantSql.hrm_sal_sp_get_RevenueRecordByShopID, ref status).FirstOrDefault();

            string strOrg = string.Empty;
            ActionService action = new ActionService(UserLogin);
            Cat_ShopEntity ShopItem = action.GetByIdUseStore<Cat_ShopEntity>(shopID, ConstantSql.hrm_cat_sp_get_ShopById, ref status);

            if (lstRevenueRecord != null)
            {
                lstRevenueRecord.StrOrg = ShopItem.ShopGroupName + @"\" + ShopItem.ShopName;
                return Json(lstRevenueRecord, JsonRequestBehavior.AllowGet);
            }
            else
            {

                Sal_RevenueForShopModel result = new Sal_RevenueForShopModel()
                {
                    Target = 0,
                    Actual = 0,
                    TotalProfile = ShopItem.TotalProfile,
                    NoShiftLeader = (int)ShopItem.NoShiftLeader,
                    Note = "0",
                    TotalPrince = 0,
                    StrOrg = ShopItem.ShopGroupName + @"\" + ShopItem.ShopName,
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDataForChildGrid(string ShopID, DateTime? Month, string KPIBonusID, string type)
        {
            string status = string.Empty;
            var shopID = Guid.Empty;
            var kpiBonus = Guid.Empty;
            DataSourceRequest request = new DataSourceRequest();
            var baseServices = new ActionService(UserLogin);
            request.PageSize = 20;
            request.Page = 1;
            var itemForShop = EnumDropDown.SalesType.E_ITEM_MAJOR.ToString();
            var lineItemForShop = EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString();
            var sale = EnumDropDown.SalesType.E_SALE.ToString();
            if (!string.IsNullOrEmpty(ShopID))
            {
                shopID = Common.ConvertToGuid(ShopID);
            }
            if (KPIBonusID == "null")
            {
                KPIBonusID = null;
            }
            if (!string.IsNullOrEmpty(KPIBonusID))
            {
                kpiBonus = Common.ConvertToGuid(KPIBonusID);
            }

            if (!string.IsNullOrEmpty(type) && type == itemForShop)
            {
                var lstObjItem = new List<object>();
                lstObjItem.Add(Month);
                lstObjItem.Add(shopID);
                //lstObjItem.Add(kpiBonus);
                lstObjItem.Add(1);
                lstObjItem.Add(int.MaxValue - 1);
                var itemServices = new Sal_ItemForShopServices();
                var lstItem = baseServices.GetData<Sal_RevenueForShopModel>(lstObjItem, ConstantSql.hrm_sal_sp_get_ItemForChildGrid, ref status);
                return Json(lstItem.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(type) && type == lineItemForShop)
            {
                var lstObjLineItem = new List<object>();
                lstObjLineItem.Add(Month);
                lstObjLineItem.Add(shopID);
                lstObjLineItem.Add(1);
                lstObjLineItem.Add(int.MaxValue - 1);
                var lineItemServices = new Sal_LineItemForShopServices();
                var lstLineItem = baseServices.GetData<Sal_RevenueForShopModel>(lstObjLineItem, ConstantSql.hrm_sal_sp_get_LineItemForChildGrid, ref status);
                return Json(lstLineItem.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            if (!string.IsNullOrEmpty(type) && type == sale)
            {
                var lstObj = new List<object>();
                lstObj.Add(Month);
                lstObj.Add(shopID);
                lstObj.Add(kpiBonus);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue - 1);
                var revenueRecordServices = new Sal_RevenueRecordService();
                var lstRevenueRecord = baseServices.GetData<Sal_RevenueForShopModel>(lstObj, ConstantSql.hrm_sal_sp_get_RevenueRecordForChildGrid, ref status);

                foreach (var i in lstRevenueRecord)
                {
                    double actual = i.Actual != null ? (double)i.Actual : 0;
                    double target = i.Target != null ? (double)i.Target : 0;
                    i.Percent = actual / target;
                }
                //lstRevenueRecord.ForEach(m => m.Percent = m.Actual != null ? (double)m.Actual : 0 / m.Target != null ? (double)m.Target : 0);

                return Json(lstRevenueRecord.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSal_LineItemForShopList([DataSourceRequest] DataSourceRequest request, Sal_LineItemForShopSearchModel model)
        {
            return ExportAllAndReturn<Sal_LineItemForShopModel, Sal_LineItemForShopEntity, Sal_LineItemForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_LineItemForShop);
        }

        /// <summary>
        ///  Hàm tím hoa hồng cho cửa hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComputeComission(Guid? ShopID, DateTime Month)
        {
            Sal_RevenueRecordService serivce = new Sal_RevenueRecordService();
            serivce.ComputeComissionService(ShopID, Month, UserLogin);
            return Json(string.Empty);
        }

        [HttpPost]
        public ActionResult GetSal_RevenueForShopList([DataSourceRequest] DataSourceRequest request, Sal_RevenueForShopSearchModel model)
        {
            return GetListDataAndReturn<Sal_RevenueForShopModel, Sal_RevenueForShopEntity, Sal_RevenueForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_RevenueForShop);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllSal_RevenueForShopList([DataSourceRequest] DataSourceRequest request, Sal_RevenueForShopSearchModel model)
        {
            return ExportAllAndReturn<Sal_RevenueForShopModel, Sal_RevenueForShopEntity, Sal_RevenueForShopSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_RevenueForShop);
        }

        public ActionResult ExportAllSal_RevenueForProfileList([DataSourceRequest] DataSourceRequest request, Sal_RevenueForProfileSearchModel model)
        {
            return ExportAllAndReturn<Sal_RevenueForProfileModel, Sal_RevenueForProfileEntity, Sal_RevenueForProfileSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_RevenueForProfile);
        }

        public ActionResult ExportAllSal_RevenueRecordList([DataSourceRequest] DataSourceRequest request, Sal_RevenueRecordSearchModel model)
        {
            return ExportAllAndReturn<Sal_RevenueRecordModel, Sal_RevenueRecordEntity, Sal_RevenueRecordSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_RevenueRecord);
        }

        public ActionResult ExportSal_RevenueRecordSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_RevenueRecordEntity, Sal_RevenueRecordModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_RevenueRecordIds);
        }

        public ActionResult ExportSal_RevenueForShopSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_RevenueForShopEntity, Sal_RevenueForShopModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_RevenueForShopIds);
        }

        public ActionResult ExportSal_ItemForShopSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_ItemForShopEntity, Sal_ItemForShopModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_ItemForShopIds);
        }

        public ActionResult ExportSal_LineItemForShopSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_LineItemForShopEntity, Sal_LineItemForShopModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_LineItemForShopIds);
        }

        public ActionResult ExportSal_RevenueForProfileSelected(List<Guid> selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_RevenueForProfileEntity, Sal_RevenueForProfileModel>(string.Join(",", selectedIds), valueFields, ConstantSql.hrm_sal_sp_get_RevenueForProfileIds);
        }
        [HttpPost]
        public ActionResult AddDataForGrade(string ProfileIDs, Guid GradePayrollID, DateTime DateHire)
        {
            var servive = new Sal_GradeServices();
            servive.AddDataForGrade(ProfileIDs, GradePayrollID, DateHire);
            return Json(null);
        }
        [HttpPost]
        public ActionResult AddDataForBasicSalary(string ProfileIDs, string BasicSalary, DateTime DateHire, Guid SalaryRankID)
        {
            var servive = new Sal_BasicSalaryServices();
            servive.AddDataForBasicSalary(ProfileIDs, BasicSalary, DateHire, SalaryRankID, UserLogin);
            return Json(null);
        }

        public ActionResult CreateTemplateFormula([Bind(Prefix = "models")]List<Cat_FormulaTemplateEntity> listFormulaTemplate, [Bind(Prefix = "GradeID")] Guid GradeID)
        {
            Cat_FormulaTemplateServices FormulaServices = new Cat_FormulaTemplateServices();
            FormulaServices.CreateFormulaTemplate(listFormulaTemplate, GradeID);
            return Json(true);
        }

        [HttpPost]
        public ActionResult AddAdjustmentSuggestionForBasicSalary([Bind(Prefix = "models")]List<Sal_AdjustmentSuggestionModel> lstAdjust, [Bind(Prefix = "datetimes")] DateTime dateEffect)
        {
            if (lstAdjust.Count > 0)
            {
                var servive = new Sal_BasicSalaryServices();
                var lstAdjustEntity = lstAdjust.Translate<Sal_AdjustmentSuggestionEntity>();
                string message = string.Empty;
                servive.AddAdjustmentSuggestionForBasicSalary(lstAdjustEntity, dateEffect, UserLogin);
                message = NotificationType.Success.ToString();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #region Sal_HealthInsuranceAndOrther
        public ActionResult GetHealthInsuranceAndOrtherList([DataSourceRequest] DataSourceRequest request, Sal_UnusualAllowanceSearchModel model)
        {
            return GetListDataAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_HealthInsuranceAndOrther);
        }
        public ActionResult ExportAllHealthInsuranceAndOrtherList([DataSourceRequest] DataSourceRequest request, Sal_UnusualAllowanceSearchModel model)
        {
            return ExportAllAndReturn<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel, Sal_UnusualAllowanceSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_HealthInsuranceAndOrther);
        }
        #endregion
        #region Sal_UnusualAllowance
        public ActionResult GetUnusualAllowanceList([DataSourceRequest] DataSourceRequest request, Sal_UnusualAllowanceSearchModel model)
        {
            return GetListDataAndReturn<Sal_UnusualAllowanceModel, Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding);
        }
        public ActionResult ExportSUnusualAllowanceSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel>(selectedIds, valueFields, ConstantSql.hrm_sal_sp_get_UnusualAllowanceByIds);
        }
        public ActionResult ExportAllUnusualAllowanceList([DataSourceRequest] DataSourceRequest request, Sal_UnusualAllowanceSearchModel model)
        {
            return ExportAllAndReturn<Sal_UnusualAllowanceEntity, Sal_UnusualAllowanceModel, Sal_UnusualAllowanceSearchModel>(request, model, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding);
        }

        public JsonResult GetRalativeByProfileId(Guid profileid)
        {
            var result = new List<Sal_UnusualAllowanceRelativeMuti>();
            var baseServices = new ActionService(UserLogin);
            string status = string.Empty;
            if (profileid != Guid.Empty)
            {
                var service = new Hre_RelativesServices();
                result = baseServices.GetData<Sal_UnusualAllowanceRelativeMuti>(profileid, ConstantSql.hrm_cat_sp_get_RelativeByProfileId, ref status);
                //if (!string.IsNullOrEmpty(relativeFilter))
                //{
                //    var rs = result.Where(s => s.RelativeName != null && s.RelativeName.ToLower().Contains(relativeFilter.ToLower())).ToList();

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDateHireForProfile(Guid ProfileID)
        {
            DateTime result = new DateTime();
            string status = string.Empty;
            if (ProfileID != Guid.Empty)
            {
                var service = new ActionService(UserLogin);
                Hre_ProfileEntity profile = service.GetByIdUseStore<Hre_ProfileEntity>(ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                if (profile != null)
                {
                    result = (DateTime)profile.DateHire;
                }
            }
            return Json(result.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRalativeByProfileIdForChidlCare(Guid profileid)
        {
            var result = new List<Sal_UnusualAllowanceRelativeMuti>();
            string status = string.Empty;
            if (profileid != Guid.Empty)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Hre_RelativesServices();
                result = baseService.GetData<Sal_UnusualAllowanceRelativeMuti>(profileid, ConstantSql.hrm_cat_sp_get_RelativeByProfileIfForChildcare, ref status);
                //if (!string.IsNullOrEmpty(relativeFilter))
                //{
                //    var rs = result.Where(s => s.RelativeName != null && s.RelativeName.ToLower().Contains(relativeFilter.ToLower())).ToList();

                //    return Json(rs, JsonRequestBehavior.AllowGet);
                //}
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYearOfBirthByRalativeId(Guid relativeid)
        {
            var result = new List<Sal_UnusualAllowanceYearOfBirth>();
            string status = string.Empty;
            if (relativeid != Guid.Empty)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Hre_RelativesServices();
                result = baseService.GetData<Sal_UnusualAllowanceYearOfBirth>(relativeid, ConstantSql.hrm_cat_sp_get_YearOfBirthByRelativeId, ref status);
            }
            if (result != null && result.Count > 0)
            {
                if (result[0].StrYearOfBirth != null)
                {
                    var strDob = Common.SplitStringDatetime(result[0].StrYearOfBirth, '/');

                    //  result[0].StrYearOfBirth = result[0].YearOfBirth.Value.ToShortDateString();
                    result[0].DateEnd = new DateTime(int.Parse(strDob[2]), int.Parse(strDob[1]), 1).AddYears(18).AddDays(-1).ToString("dd/MM/yyyy");
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFormulaByUnusualEDType(Guid? ID)
        {
            if (ID != null)
            {
                string status = string.Empty;
                var service = new ActionService(UserLogin);
                var result = service.GetByIdUseStore<Cat_UnusualAllowanceCfgEntity>((Guid)ID, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfgId, ref status);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new Cat_UnusualAllowanceCfgEntity());
            }

        }

        public JsonResult GetRalativeTypeByRelativeId(Guid relativeid, string relativetypeFilter)
        {
            var result = new List<Sal_UnusualAllowanceRelativeTypeMuti>();
            string status = string.Empty;
            if (relativeid != Guid.Empty)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Cat_RelativeTypeServices();
                result = baseService.GetData<Sal_UnusualAllowanceRelativeTypeMuti>(relativeid, ConstantSql.hrm_cat_sp_get_RelativeTypeByRelativeId, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRelativeTypeByProfileId(Guid profileid, string relativetypeFilter)
        {
            var result = new List<Sal_UnusualAllowanceRelativeMuti>();
            string status = string.Empty;
            if (profileid != Guid.Empty)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Cat_RelativeTypeServices();
                result = baseService.GetData<Sal_UnusualAllowanceRelativeMuti>(profileid, ConstantSql.hrm_cat_sp_get_RelativeTypeByProfileId, ref status);
                if (!string.IsNullOrEmpty(relativetypeFilter))
                {
                    var rs = result.Where(s => s.RelativeName != null && s.RelativeName.ToLower().Contains(relativetypeFilter.ToLower())).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUnusualEDType(string text)
        {
            return GetDataForControl<Sal_UnusualEDMutiModel, Sal_UnusualAllowanceMultiEntity>(text, ConstantSql.hrm_cat_sp_get_UnusualEDType_multi);
        }

        public JsonResult GetMultiHealthInsuranceAndOrther(string text)
        {
            return GetDataForControl<Sal_UnusualEDMutiModel, Sal_UnusualAllowanceMultiEntity>("HealthInsuranceAndOrther", ConstantSql.hrm_cat_sp_get_UnusualEDTypeByCode);
        }
        public JsonResult GetMultiAmountByUnusualAllowanceCfgid(Guid unusualedtypeid, string amountFilter)
        {

            var result = new List<Sal_UnusualAllowanceAmountMuti>();
            string status = string.Empty;
            if (unusualedtypeid != Guid.Empty)
            {
                var baseService = new ActionService(UserLogin);
                var service = new Cat_RelativeTypeServices();
                result = baseService.GetData<Sal_UnusualAllowanceAmountMuti>(unusualedtypeid, ConstantSql.hrm_cat_sp_get_AmountByUSCfg_multi, ref status);
                if (!string.IsNullOrEmpty(amountFilter))
                {
                    var rs = result.Where(s => s.Amount != null).ToList();
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAmountByChildCare(string id)
        {
            var result = new List<Cat_UnusualAllowanceCfgAmout>();
            var model = new Sal_UnusualAllowanceModel();
            string status = string.Empty;
            var service = new Sal_UnusualAllowanceServices();
            result = service.GetData<Cat_UnusualAllowanceCfgAmout>(id, ConstantSql.hrm_cat_sp_get_GetAmountByChildCare, UserLogin, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHre_UnusualAllowanceList([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(profileID);
            var result = baseService.GetData<Sal_UnusualAllowanceModel>(objs, ConstantSql.hrm_sal_sp_get_UnusualAllowanceByProfileid, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return Json(null);
        }
        public ActionResult GetHre_UnusualEDChildCareList([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            var baseService = new ActionService(UserLogin);
            string status = string.Empty;
            // var baseService = new BaseService();
            var objs = new List<object>();
            objs.Add(profileID);
            var result = baseService.GetData<Sal_UnusualAllowanceModel>(objs, ConstantSql.hrm_sal_sp_get_UnusualEDChildCareByProfileid, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }
        public JsonResult GetProfileNameByProfileId(Guid profileid)
        {
            var result = new Hre_ProfileEntity();
            string status = string.Empty;
            if (profileid != Guid.Empty)
            {
                var service = new ActionService(UserLogin);
                result = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(profileid.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lương phòng ban ComputeSalaryDeparment

        public JsonResult ComputeSalaryDeparment(Guid? ID)
        {
            if (ID != null && ID != Guid.Empty)
            {
                Sal_SalaryDepartmentServices services = new Sal_SalaryDepartmentServices();
                services.ComputeDepartment((Guid)ID);
                return Json(true);
            }
            else
            {
                return Json(false);
            }


        }

        public JsonResult ComputeSumProduct(Guid Product, int Count)
        {
            ActionService services = new ActionService(UserLogin);

            Cat_ProductItemEntity ProductItem = new Cat_ProductItemEntity();
            string status = string.Empty;
            ProductItem = services.GetByIdUseStore<Cat_ProductItemEntity>(Product, ConstantSql.hrm_cat_sp_get_ProductItemById, ref status);
            if (ProductItem != null)
            {
                if (ProductItem.UnitPrice != null)
                {
                    return Json(ProductItem.UnitPrice * Count);
                }
                else
                {
                    return Json(0);
                }
            }
            return Json(0);
        }
        #endregion


        #region Kai_RankMark
        public ActionResult GetKai_RankMarkList([DataSourceRequest]DataSourceRequest request, Kai_RankMarkSearchModel model)
        {
            return GetListDataAndReturn<Kai_RankMarkModel, Kai_RankMarkEntity, Kai_RankMarkSearchModel>(request, model, ConstantSql.hrm_kai_sp_get_RankMark);
        }
        public ActionResult ExportAllKai_RankMarkList([DataSourceRequest]DataSourceRequest request, Kai_RankMarkSearchModel model)
        {
            return ExportAllAndReturn<Kai_RankMarkEntity, Kai_RankMarkModel, Kai_RankMarkSearchModel>(request, model, ConstantSql.hrm_kai_sp_get_RankMark);
        }
        public ActionResult ExportKai_RankMarkSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Kai_RankMarkEntity, Kai_RankMarkModel>(selectedIds, valueFields, ConstantSql.hrm_kai_sp_get_RankMarkByIds);
        }
        #endregion

        [HttpPost]
        public ActionResult GetKai_CategoryList([DataSourceRequest] DataSourceRequest request, Kai_CategorySearchModel model)
        {
            return GetListDataAndReturn<Kai_CategoryEntity, Kai_CategoryModel, Kai_CategorySearchModel>(request, model, ConstantSql.hrm_Kai_sp_get_Category);
        }

        public ActionResult ExportAllKaiCategoryList([DataSourceRequest] DataSourceRequest request, Kai_CategorySearchModel model)
        {
            return ExportAllAndReturn<Kai_CategoryModel, Kai_CategoryEntity, Kai_CategorySearchModel>(request, model, ConstantSql.hrm_Kai_sp_get_Category);
        }
        public ActionResult ExportKaiCategorySelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Kai_CategoryEntity, Kai_CategoryModel>(selectedIds, valueFields, ConstantSql.hrm_Kai_sp_get_CategoryByIds);
        }

        public JsonResult GetMultiCategory(string text)
        {
            return GetDataForControl<Kai_CategoryModel, Kai_CategoryEntity>(text, ConstantSql.hrm_cat_sp_get_Category_multi);
        }

        public JsonResult GetMultiCategoryType(string text)
        {
            return GetDataForControl<Kai_CategoryModel, Kai_CategoryEntity>(text, ConstantSql.hrm_cat_sp_get_GetMultiCategoryType_munti);
        }

        [HttpPost]
        public ActionResult GetkaiZenDataList([DataSourceRequest] DataSourceRequest request, Kai_KaizenDataSearchModel model, bool? IsHolSalary, bool? IsInvalidData)
        {
            ActionService Services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listObject = new List<object>();
            listObject.AddRange(new object[10]);
            listObject[0] = model.ProfileName;
            listObject[1] = model.CodeEmp;
            listObject[2] = model.KaizenName;
            listObject[3] = model.OrgStructureID;
            listObject[4] = model.CategoryID;
            listObject[5] = model.Status;
            listObject[6] = model.DateFrom;
            listObject[7] = model.DateTo;
            listObject[8] = 1;
            listObject[9] = Int32.MaxValue - 1;
            List<Kai_KaizenDataEntity> listResult = Services.GetData<Kai_KaizenDataEntity>(listObject, ConstantSql.hrm_Kai_sp_get_KaiZenData, ref status);

            if (IsHolSalary != null && IsHolSalary == true)
            {
                listObject = new List<object>();
                listObject.AddRange(new object[10]);
                listObject[4] = model.DateFrom;
                listObject[5] = model.DateTo;
                listObject[8] = 1;
                listObject[9] = Int32.MaxValue - 1;
                List<Sal_HoldSalaryEntity> listHolSalary = Services.GetData<Sal_HoldSalaryEntity>(listObject, ConstantSql.hrm_sal_sp_get_HoldSalary, ref status);

                listResult = listResult.Where(m => listHolSalary.Any(t => t.ProfileID == m.ProfileID)).ToList();
            }

            if (IsInvalidData != null && IsInvalidData == true)
            {
                listObject = new List<object>();
                listObject.AddRange(new object[3]);
                listObject[1] = 1;
                listObject[2] = Int32.MaxValue - 1;
                var result = Services.GetData<Kai_RankMarkEntity>(listObject, ConstantSql.hrm_kai_sp_get_RankMark, ref status);
                List<Kai_KaizenDataEntity> listKaizenData = new List<Kai_KaizenDataEntity>();

                foreach (var i in listResult)
                {
                    if (i.MarkIdea != null && i.MarkPerform != null)
                    {
                        result = result.Where(m => m.MarkIdea != null && m.MarkIdea == i.MarkIdea).ToList();
                        if (!result.Any(m => m.MarkPerform == i.MarkPerform))
                        {
                            listKaizenData.Add(i);
                        }
                    }
                }
                return Json(listKaizenData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return Json(listResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetMarkAmountKaizen(double? MarkIdea, double? MarkPerform)
        {
            ActionService Services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listObject = new List<object>();
            listObject.AddRange(new object[3]);
            listObject[1] = 1;
            listObject[2] = Int32.MaxValue - 1;
            List<Kai_RankMarkEntity> listRankMark = Services.GetData<Kai_RankMarkEntity>(listObject, ConstantSql.hrm_kai_sp_get_RankMark, ref status);

            if (listRankMark != null)
            {
                MarkIdea = MarkIdea != null ? MarkIdea : 0;
                MarkPerform = MarkPerform != null ? MarkPerform : 0;

                Kai_RankMarkEntity rankMarkItem = listRankMark.FirstOrDefault(m => m.MarkIdea != null && m.MarkIdea != null && m.MarkIdea == MarkIdea && m.MarkPerform == MarkPerform);
                if (rankMarkItem != null)
                {
                    double AmountIdea = rankMarkItem.AmountIdea != null ? (double)rankMarkItem.AmountIdea : 0;
                    double AmountPerform = rankMarkItem.AmountPerform != null ? (double)rankMarkItem.AmountPerform : 0;
                    return Json(new { TotalAmount = AmountIdea + AmountPerform }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { TotalAmount = -1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { TotalAmount = -1 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetkaiZenDataListPaymentOut([DataSourceRequest] DataSourceRequest request, Kai_KaizenDataSearchModel model, bool? IsHolSalary)
        {
            ActionService Services = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listObject = new List<object>();
            listObject.AddRange(new object[10]);
            listObject[0] = model.ProfileName;
            listObject[1] = model.CodeEmp;
            listObject[3] = model.OrgStructureID;
            listObject[4] = model.CategoryID;
            listObject[6] = model.DateFrom;
            listObject[7] = model.DateTo;
            listObject[8] = 1;
            listObject[9] = Int32.MaxValue - 1;
            List<Kai_KaizenDataEntity> listResult = Services.GetData<Kai_KaizenDataEntity>(listObject, ConstantSql.hrm_Kai_sp_get_KaiZenData, ref status);
            return Json(listResult.Where(m => m.IsPaymentOut == true).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }



        public ActionResult ExportAllKaiZenDataList([DataSourceRequest] DataSourceRequest request, Kai_KaizenDataSearchModel model)
        {
            return ExportAllAndReturn<Kai_KaizenDataModel, Kai_KaizenDataEntity, Kai_KaizenDataSearchModel>(request, model, ConstantSql.hrm_Kai_sp_get_KaiZenData);
        }

        [HttpPost]
        public ActionResult SetPaymentOut(List<Guid> selectIds, DateTime MonthYear)
        {
            Kai_KaiZenDataServices ServiesKaiZen = new Kai_KaiZenDataServices();
            if (MonthYear == null)
            {
                MonthYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            int[] Result = ServiesKaiZen.SetPayrmentOut(selectIds, MonthYear);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult SaveCustomAccumulate(Kai_KaizenDataModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Kai_KaizenDataModel>(model, "CustomAccumulateInfo", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            Kai_KaiZenDataServices ServiesKaiZen = new Kai_KaiZenDataServices();
            bool Result = ServiesKaiZen.SaveCustomAccumulate(model.ID, (double)model.AccumulateRevice);
            if (Result)
            {
                return Json(new { success = NotificationType.Success.ToString(), mess = ConstantDisplay.Hrm_Succeed.TranslateString() });
            }
            else
            {
                return Json(new { success = NotificationType.Error.ToString(), mess = ConstantDisplay.Hrm_Error.TranslateString() });
            }
        }

        [HttpPost]
        public ActionResult SetApproveKaizenData(List<Guid> selectIds)
        {
            Kai_KaiZenDataServices ServiesKaiZen = new Kai_KaiZenDataServices();
            bool Result = ServiesKaiZen.SetApproveKaiZenData(selectIds);
            if (Result)
            {
                return Json(new { success = NotificationType.Success.ToString(), mess = ConstantDisplay.Hrm_Succeed.TranslateString() });
            }
            else
            {
                return Json(new { success = NotificationType.Error.ToString(), mess = ConstantDisplay.Hrm_Error.TranslateString() });
            }
        }

        [HttpPost]
        public ActionResult SetWaitApproveKaizenData(List<Guid> selectIds)
        {
            Kai_KaiZenDataServices ServiesKaiZen = new Kai_KaiZenDataServices();
            string[] Result = ServiesKaiZen.SetWaitApproveKaiZenData(selectIds);
            return Json(Result);
            //if (Result)
            //{
            //    return Json(new { success = NotificationType.Success.ToString(), mess = ConstantDisplay.Hrm_Succeed.TranslateString() });
            //}
            //else
            //{
            //    return Json(new { success = NotificationType.Error.ToString(), mess = ConstantDisplay.Hrm_Error.TranslateString() });
            //}
        }

        public ActionResult ExportKaiZenDataSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Kai_KaizenDataEntity, Kai_KaizenDataModel>(selectedIds, valueFields, ConstantSql.hrm_kai_sp_get_KaiZenDataByIds);
        }

        [HttpPost]
        public ActionResult MoveKaiZenOut(List<Guid> SelectIds)
        {
            string status = string.Empty;
            Kai_KaiZenDataServices service = new Kai_KaiZenDataServices();
            bool Result = service.SaveAmountKaiZen(SelectIds);
            if (Result)
            {
                return Json(new { success = NotificationType.Success.ToString(), mess = ConstantDisplay.Hrm_Succeed.TranslateString() });
            }
            else
            {
                return Json(new { success = NotificationType.Error.ToString(), mess = ConstantDisplay.Hrm_Error.TranslateString() });
            }
        }

        public ActionResult ComputeBonus(Sal_UnusualAllowanceEntity model)
        {
            #region Validate
            string message = string.Empty;
            bool checkValidate = false;
            if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceEntity>(model, "Sal_BonusEvalutionYearInfoProfile", ref message);
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceEntity>(model, "Sal_BonusEvalutionYearInfoOrgStructure", ref message);
            }

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_UnusualAllowanceServices services = new Sal_UnusualAllowanceServices();
            Guid Result = services.ComputeBonusUnusualAllowance(model, UserLogin);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComputeAnnualLeaveAllowanceInfo(Sal_UnusualAllowanceEntity model)
        {
            #region Validate
            string message = string.Empty;
            bool checkValidate = false;
            if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceEntity>(model, "Sal_BonusEvalutionYearInfoProfile", ref message);
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_UnusualAllowanceEntity>(model, "Sal_BonusEvalutionYearInfoOrgStructure", ref message);
            }

            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion


            Sal_UnusualAllowanceServices services = new Sal_UnusualAllowanceServices();
            Guid Result = services.ComputeAnnualLeaveAllowance(model, UserLogin);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #region Hieu.Van - Báo cáo chuyển khoản (compare Ver.7)

        public ActionResult GetReportTransferViaBank([DataSourceRequest] DataSourceRequest request, Sal_ReportTransferViaBankSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportTransferViaBankSearchModel>(model, "Sal_ReportTransferViaBank", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var service = new Sal_ReportService();
            var hrService = new ActionService(UserLogin);

            List<Guid> lstPgID = new List<Guid>();
            if (model.PayrollGroupID != null)
            {
                lstPgID.Add(model.PayrollGroupID.Value);
            }
            List<Guid> listBankID = new List<Guid>();
            if (model.BankID != null)
            {
                lstPgID.Add(model.BankID.Value);
            }

            List<object> strIDs = new List<object>();
            strIDs.AddRange(new object[3]);
            strIDs[0] = (object)model.OrgStructureID;
            string status = string.Empty;

            var isDataTable = false;
            object obj = new DataTable();
            List<Hre_ProfileEntity> lstProfileFull = hrService.GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            string nameReport = "Sal_ReportTransferViaBankModel";
            var result = service.GetReportTransferViaBank(lstProfileFull, lstPgID, model.GroupBank, listBankID, model.CutOffDurationID.Value,
                                        model.NoDisplay0Data, model.StatusEmployees, model.ElementType, nameReport, UserLogin, model.IsCreateTemplate);

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
                    FileName = nameReport,
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>();

                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);

                //var fullPath = ExportService.Export(model.ExportId, result);
                //return Json(fullPath.ToString().Replace("Success,", "").ToString());
            }
            return Json(result.ToDataSourceResult(request));
        }

        #endregion

        #region Hieu.van - Báo Cáo Tiền Mặt (compare Ver.7)

        public ActionResult GetReportCash([DataSourceRequest] DataSourceRequest request, Sal_ReportCashSearchModel model)
        {
            var service = new Sal_ReportService();
            var hrService = new ActionService(UserLogin);

            List<Guid> lstPgID = new List<Guid>();
            if (model.PayrollGroupID != null)
            {
                lstPgID.Add(model.PayrollGroupID.Value);
            }

            List<object> strIDs = new List<object>();
            strIDs.AddRange(new object[3]);
            strIDs[0] = (object)model.OrgStructureID;
            string status = string.Empty;

            var isDataTable = false;
            object obj = new DataTable();
            List<Hre_ProfileEntity> lstProfileFull = hrService.GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            var result = service.GetReportCash(lstProfileFull, lstPgID, model.CutOffDurationID.Value,
                                        model.NoDisplay0Data, model.StatusEmployees, model.ElementType, model.IsCreateTemplate);

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
                    FileName = "Sal_ReportCashModel",
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

        #endregion

        #region Hieu.Van - Báo Cáo Thuế Thu Nhập Cá Nhân
        public ActionResult GetReportPITValidate(Sal_ReportPITSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportPITSearchModel>(model, "Sal_ReportPIT", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);

        }

        public ActionResult GetReportPIT([DataSourceRequest] DataSourceRequest request, Sal_ReportPITSearchModel model)
        {
            var service = new Sal_ReportService();
            var hrService = new ActionService(UserLogin);
            int year = model.Year == null ? DateTime.Now.Year : model.Year.Value;

            List<object> strIDs = new List<object>();
            strIDs.AddRange(new object[3]);
            strIDs[0] = (object)model.OrgStructureID;
            string status = string.Empty;
            string[] _StatusEmployees = null;
            if (model.StatusEmployees != null)
                _StatusEmployees = model.StatusEmployees.Split(',').Select(s => s).ToArray();

            var isDataTable = false;
            object obj = new DataTable();
            List<Hre_ProfileEntity> lstProfileFull = hrService.GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

            var result = service.GetReportPIT(lstProfileFull, year, _StatusEmployees, model.IsCreateTemplate);

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
                    FileName = "Sal_ReportPITModel",
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
        #endregion

        #region Hien.Nguyen BC Chuyển Khoản Thưởng NV Bị Giữ Lương

        public ActionResult ReportTransferBonusHold([DataSourceRequest] DataSourceRequest request, Sal_ReportElementDynamicGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportElementDynamicGeneralModel>(model, "Sal_ReportElementDynamicGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();

            if (!model.IsCreateTemplate)
            {
                //Lọc theo kỳ lương
                if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                {
                    ActionService action = new ActionService(UserLogin);
                    cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                }

                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = Services.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
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

            List<String> ListElement = model.ElementType.Split(',').ToList();

            #endregion

            DataTable Table = Services.ReportTransferBonusHold(listProfileByOrg, ListElement, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hien.Nguyen BC Lương Và Phụ Cấp Thôi Việc

        public ActionResult ReportSalaryAllowanceQuit([DataSourceRequest] DataSourceRequest request, Sal_ReportElementDynamicGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportElementDynamicGeneralModel>(model, "Sal_ReportSalaryAllowanceQuit", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();

            if (!model.IsCreateTemplate)
            {
                //Lọc theo kỳ lương
                if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                {
                    ActionService action = new ActionService(UserLogin);
                    cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                }

                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
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

            //chỉ lấy những nhân viên đã nghủ việc
            listProfileByOrg = listProfileByOrg.Where(m => m.DateQuit != null && m.DateQuit <= cutoff.DateEnd).ToList();

            //lọc theo số lấn quyết toán
            if (model.Settlement != null)
            {
                listProfileByOrg = listProfileByOrg.Where(m => m.Settlement == model.Settlement).ToList();
            }

            List<String> ListElement = model.ElementType.Split(',').ToList();

            #endregion

            DataTable Table = Services.ReportSalaryAllowanceQuit(listProfileByOrg, ListElement, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - Báo cáo phần tử động

        public ActionResult ReportElementDynamicGeneral([DataSourceRequest] DataSourceRequest request, Sal_ReportElementDynamicGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportElementDynamicGeneralModel>(model, "Sal_ReportElementDynamicGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();

            if (!model.IsCreateTemplate)
            {
                //Lọc theo kỳ lương
                if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                {
                    ActionService action = new ActionService(UserLogin);
                    cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                }

                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
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

            List<String> ListElement = model.ElementType.Split(',').ToList();

            #endregion

            DataTable Table = Services.ReportElementDynamicGeneral(listProfileByOrg, ListElement, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region BC Chi Tiết Thưởng Đánh Giá
        public ActionResult ReportBonusEvaDetail([DataSourceRequest] DataSourceRequest request, Sal_ReportElementDynamicGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportElementDynamicGeneralModel>(model, "Sal_ReportBonusEvaDetail", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();

            if (!model.IsCreateTemplate)
            {
                //Lọc theo kỳ lương
                if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                {
                    ActionService action = new ActionService(UserLogin);
                    cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                }

                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
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

            string[] ListElement = model.ElementType.Split(',');

            #endregion

            DataTable Table = Services.ReportBonusEvaDetail(listProfileByOrg, ListElement, (DateTime)model.Year, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "YearExport", Value = model.Year };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4, headerInfo5 };

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

        #region BC Tổng Hợp Tính Tiền Phép Năm, Sức Khỏe Tốt
        public ActionResult ReportRemittanceAllowSick([DataSourceRequest] DataSourceRequest request, Sal_ReportElementDynamicGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportElementDynamicGeneralModel>(model, "Sal_ReportElementDynamicGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
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
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                    if (model.WorkPlace != null)
                    {
                        List<Guid> lstWP = model.WorkPlace.Split(',').Select(s => Guid.Parse(s)).ToList();
                        listProfileByOrg = listProfileByOrg.Where(s => s.WorkPlaceID != null && lstWP.Contains(s.WorkPlaceID.Value)).ToList();
                    }
                }


                Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
                DateTime DateClose = Sys_Services.GetConfigValue<DateTime>(AppConfig.HRM_SAL_DATECLOSE_ALLOWANCE);
                //CHỈ XUẤT DỮ LIỆU NHÂN VIÊN CÓ NGÀY NGHỈ VIỆC (DATEQUIT) >= NGÀY CHÔT (TASK SỐ 46859)
                if (DateClose != null)
                {
                    DateClose = new DateTime(DateTime.Now.Year, DateClose.Month, DateClose.Day);
                    listProfileByOrg = listProfileByOrg.Where(m => m.DateQuit == null || m.DateQuit >= DateClose).ToList();
                }
            }

            List<string> ListElement = model.ElementType.Split(',').ToList();
            List<string> ListUnusualAllowanceCfg = new List<string>();
            if (!string.IsNullOrEmpty(model.UnsualAllowanceCfg))
            {
                ListUnusualAllowanceCfg = model.UnsualAllowanceCfg.Split(',').ToList();
            }
            #endregion

            DataTable Table = Services.ReportRemittanceAllowSick(listProfileByOrg, ListElement, ListUnusualAllowanceCfg, (DateTime)model.Year, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.Year };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - Báo cáo tổng hợp thuế PIT hàng tháng
        public ActionResult ReportTotalPITMonthly([DataSourceRequest] DataSourceRequest request, Sal_ReportTotalPITMonthlyModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportTotalPITMonthlyModel>(Model, "Sal_ReportTotalPITMonthly", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();
            ActionService actionServices = new ActionService(UserLogin);

            //Lọc theo phòng ban
            if (Model.CutOffDurationID != null && Model.CutOffDurationID != Guid.Empty)
            {
                ActionService action = new ActionService(UserLogin);
                cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            }

            //Lọc theo phòng ban
            if ((Model.OrgStructureID != null && Model.OrgStructureID != string.Empty)
                || (Model.OrgStructureID == null && Model.ProfileIDs == null))
            {
                List<object> listObj = new List<object>();
                listObj.Add(Model.OrgStructureID);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }

            //lọc theo nhân viên
            if (Model.ProfileIDs != null && Model.ProfileIDs != string.Empty)
            {
                lstModel.AddRange(new object[16]);
                lstModel[14] = 1;
                lstModel[15] = Int32.MaxValue - 1;
                listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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

            #endregion

            List<String> ListElement = Model.ElementType.Split(',').ToList();
            DataTable Table = Services.ReportTotalPITMonthly(listProfileByOrg, ListElement, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTotalPITMonthly",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }

            #endregion

            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        #region Hieu.Van - Báo cáo Chuyển Khoản _ Lấy theo phần tử động version 2
        public ActionResult ReportTransferViaBank_ED([DataSourceRequest] DataSourceRequest request, Sal_ReportTransferViaBank_EDSearchModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportTransferViaBank_EDSearchModel>(Model, "Sal_ReportTransferViaBank_ED", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();
            ActionService actionServices = new ActionService(UserLogin);

            //Lọc theo phòng ban
            if (Model.CutOffDurationID != null && Model.CutOffDurationID != Guid.Empty)
            {
                ActionService action = new ActionService(UserLogin);
                cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(Model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            }

            //Lọc theo phòng ban
            if ((Model.OrgStructureID != null && Model.OrgStructureID != string.Empty)
                || (Model.OrgStructureID == null && Model.ProfileIDs == null))
            {
                List<object> listObj = new List<object>();
                listObj.Add(Model.OrgStructureID);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            }

            //lọc theo nhân viên
            if (Model.ProfileIDs != null && Model.ProfileIDs != string.Empty)
            {
                lstModel.AddRange(new object[16]);
                lstModel[14] = 1;
                lstModel[15] = Int32.MaxValue - 1;
                listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(Model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                if (Model.StatusEmployees != null)
                {
                    List<Hre_ProfileEntity> listProfileByOrgTEMP = new List<Hre_ProfileEntity>();

                    List<string> lstSTT = Model.StatusEmployees.Split(',').ToList();
                    if (lstSTT.Contains(StatusEmployee.E_WORKING.ToString()))
                    {
                        listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                    {
                        listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                    {
                        listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                    {
                        listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
                    }
                    listProfileByOrg = listProfileByOrgTEMP;
                }
                if (Model.WorkPlace != null)
                {
                    List<Guid> lstWP = Model.WorkPlace.Split(',').Select(s => Guid.Parse(s)).ToList();
                    listProfileByOrg = listProfileByOrg.Where(s => s.WorkPlaceID != null && lstWP.Contains(s.WorkPlaceID.Value)).ToList();
                }
            }

            #endregion

            List<String> ListElement = Model.ElementType.Split(',').ToList();
            DataTable Table = Services.ReportTransferViaBank_ED(listProfileByOrg, ListElement, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, Model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (Model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };
            if (Model != null && Model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportTransferViaBank_ED",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (Model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(Model.ExportId, Table, listHeaderInfo, Model.ExportType);
                return Json(fullPath);
            }

            #endregion

            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        #region Hieu.Van - Báo cáo Tiền Mặt _ Lấy theo phần tử động version 2
        public ActionResult ReportCash_ED([DataSourceRequest] DataSourceRequest request, Sal_ReportCash_EDModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportCash_EDModel>(model, "Sal_ReportCash_ED", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();
            ActionService actionServices = new ActionService(UserLogin);

            if (!model.IsCreateTemplate)
            {
                //Lọc theo phòng ban
                if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                {
                    ActionService action = new ActionService(UserLogin);
                    cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                }

                //Lọc theo phòng ban
                if ((model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                    || (model.OrgStructureID == null && model.ProfileIDs == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(model.OrgStructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd) && pro.DateHire < cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= cutoff.DateEnd && pro.DateHire >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= cutoff.DateEnd && pro.DateQuit.Value >= cutoff.DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= cutoff.DateEnd).ToList());
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

            List<String> ListElement = model.ElementType.Split(',').ToList();

            #endregion

            DataTable Table = Services.ReportCash_ED(listProfileByOrg, ListElement, model.GroupBank, (DateTime)cutoff.DateStart, (DateTime)cutoff.DateEnd, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = cutoff.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = cutoff.DateEnd };
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

        #region Hieu.Van - Báo cáo chi tiết phụ cấp
        public ActionResult GetReportAllowanceDetail([DataSourceRequest] DataSourceRequest request, Sal_ReportAllowanceDetailSearchModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportAllowanceDetailSearchModel>(model, "Sal_ReportAllowanceDetail", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            var service = new Sal_ReportService();
            var actionservice = new ActionService(UserLogin);
            var hrService = new ActionService(UserLogin);


            var isDataTable = false;
            object obj = new DataTable();
            string status = string.Empty;
            var CutOffDuration = actionservice.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            var result = service.GetReportAllowanceDetail(model.OrgStructureID, model.UnusualEDTypeID, CutOffDuration.DateStart, CutOffDuration.DateEnd, model.StatusSyn, model.WorkPlace, UserLogin, model.IsCreateTemplate);

            if (model.IsCreateTemplateForDynamicGrid)
            {
                //result.Columns.RemoveAt(2);
                obj = result;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = CutOffDuration.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = CutOffDuration.DateEnd };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportAllowanceDetailModel",
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
                var fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, ExportFileType.Excel);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Hieu.Van - DANH SÁCH TỔNG HỢP BHXH, BHYT, BHTN, TRUY THU BH (XH,YT,TN) Theo Năm

        public ActionResult ReportGeneralInsuranceInYear([DataSourceRequest] DataSourceRequest request, Sal_ReportGeneralInsuranceInYearModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportGeneralInsuranceInYearModel>(model, "Sal_ReportGeneralInsuranceInYear", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            DateTime DateStart = model.MonthStart.Value;
            DateTime DateEnd = model.MonthStart.Value;
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
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= DateEnd) && pro.DateHire < DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= DateEnd && pro.DateHire >= DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= DateEnd && pro.DateQuit.Value >= DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= DateEnd).ToList());
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

            List<String> ListElement = model.ElementType.Split(',').ToList();

            #endregion

            DataTable Table = Services.ReportGeneralInsuranceInYear(listProfileByOrg, ListElement, DateStart, DateEnd, UserLogin, model.IsCreateTemplate, model.ReportName);

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
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };


            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - Báo cáo hoạch toán tiền phép ốm

        public ActionResult ReportAllowsAccountingOfSick([DataSourceRequest] DataSourceRequest request, Sal_ReportAllowsAccountingOfSickModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportAllowsAccountingOfSickModel>(model, "Sal_ReportAllowsAccountingOfSick", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            Att_CutOffDurationEntity cutoff = new Att_CutOffDurationEntity();

            //Lọc theo phòng ban
            if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
            {
                ActionService action = new ActionService(UserLogin);
                cutoff = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            }

            List<String> ListElement = model.ElementType.Split(',').ToList();
            List<Guid> lstSC = new List<Guid>();
            if (model.SalaryClassIDs != null)
                lstSC = model.SalaryClassIDs.Split(',').Select(s => Guid.Parse(s)).ToList();


            #endregion

            DataTable Table = Services.ReportAllowsAccountingOfSick((DateTime)model.Year, model.OrgStructureID, ListElement, model.StatusSyn, lstSC, model.WorkPlace, UserLogin, model.IsCreateTemplate);

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

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                ActionService ActionServices = new ActionService(UserLogin);
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "Year", Value = model.Year };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID != null && profileByID.CodeEmp != null ? profileByID.CodeEmp : "" };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - BC tổng hợp danh sách chuyển tiền kaizen hàng tháng

        public ActionResult ReportKaizenMonthlyCash([DataSourceRequest] DataSourceRequest request, Sal_ReportKaizenMonthlyCashModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportKaizenMonthlyCashModel>(model, "Sal_ReportKaizenMonthlyCash", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            #region Getdata
            List<Hre_ProfileEntity> listProfileByOrg = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();
            ActionService actionServices = new ActionService(UserLogin);
            DateTime DateStart = model.DateFrom.Value;
            DateTime DateEnd = model.DateTo.Value;
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
                    listProfileByOrg = actionServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                }

                //lọc theo nhân viên
                if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = actionServices.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileIDs), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status).ToList();
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
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= DateEnd) && pro.DateHire < DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= DateEnd && pro.DateHire >= DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= DateEnd && pro.DateQuit.Value >= DateStart).ToList());
                        }
                        if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                        {
                            listProfileByOrgTEMP.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= DateEnd).ToList());
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


            #endregion

            DataTable Table = Services.ReportKaizenMonthlyCash(listProfileByOrg, DateStart, DateEnd, model.OrgStructureID, UserLogin, model.IsCreateTemplate, model.ReportName);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                //var col = Table.Columns.Count;
                //Table.Columns.RemoveAt(col - 1);
                obj = Table;
                isDataTable = true;
            }
            var WP = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty;
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - Bổ dung báo cáo tổng hợp lương theo năm

        public ActionResult ReportGeneralAnnualWage([DataSourceRequest] DataSourceRequest request, Sal_ReportGeneralAnnualWageModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportGeneralAnnualWageModel>(model, "Sal_ReportGeneralAnnualWage", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion

            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();

            List<String> ListElement = model.ElementType.Split(',').ToList();

            DataTable Table = Services.ReportGeneralAnnualWage(model.YearStart.Value, model.YearEnd.Value, ListElement, UserLogin, model.IsCreateTemplate, model.ReportName);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                //var col = Table.Columns.Count;
                //Table.Columns.RemoveAt(col - 1);
                obj = Table;
                isDataTable = true;
            }
            var WP = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty;
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.YearStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.YearEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkPlace", Value = model.WorkingPlaceName != null ? model.WorkingPlaceName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5 };

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

        #region Hieu.Van - BC tỷ lệ tham dự và tỷ lệ áp dụng - KAIZEN

        public ActionResult ReportKaizenGeneral([DataSourceRequest] DataSourceRequest request, Sal_ReportKaizenGeneralModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportKaizenGeneralModel>(model, "Sal_ReportKaizenGeneral", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            if (model.DateFrom != null && model.DateTo != null)
            {
                DateTime _tempDateTo = model.DateFrom.Value.AddMonths(11);
                if (model.DateTo > _tempDateTo)
                {
                    message = ConstantDisplay.HRM_Sal_ReportKaizenGenera_Only12Month.TranslateString();
                    var ls = new object[] { "error", message };
                    return Json(ls);
                }
            }
            #endregion

            BaseService _base = new BaseService();
            Sal_ReportService Services = new Sal_ReportService();
            string status = string.Empty;
            List<object> lstModel = new List<object>();


            DataTable Table = Services.ReportKaizenGeneral(model.OrgStructureID, model.DateFrom.Value, model.DateTo.Value, UserLogin, model.IsCreateTemplate, model.ReportName);

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

            #region Lấy các cấp phòng ban cho header

            Guid orgId = Guid.Empty;
            Guid.TryParse(model.OrgStructureID, out orgId);
            var orgServices = new Cat_OrgStructureServices();
            ActionService actionServices = new ActionService(UserLogin);

            var lstOrgs = new List<object>();
            lstOrgs.Add(null);
            lstOrgs.Add(null);
            lstOrgs.Add(null);
            lstOrgs.Add(1);
            lstOrgs.Add(int.MaxValue - 1);

            var lstOrg = actionServices.GetData<Cat_OrgStructureEntity>(lstOrgs, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();
            //  var orgs = orgServices.GetData<Cat_OrgStructureEntity>(orgId, ConstantSql.hrm_cat_sp_get_OrgStructureById, ref status).FirstOrDefault();

            var lstOrgTypes = new List<object>();
            lstOrgTypes.Add(null);
            lstOrgTypes.Add(null);
            lstOrgTypes.Add(1);
            lstOrgTypes.Add(int.MaxValue - 1);
            var orgTypes = actionServices.GetData<Cat_OrgStructureTypeEntity>(lstOrgTypes, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status);

            var orgE_DEPARTMENT = LibraryService.GetNearestParentEntity((Guid?)orgId, OrgUnit.E_DEPARTMENT, lstOrg, orgTypes);
            var orgE_UNIT = LibraryService.GetNearestParentEntity((Guid?)orgId, OrgUnit.E_UNIT, lstOrg, orgTypes);
            var orgE_DIVISION = LibraryService.GetNearestParentEntity((Guid?)orgId, OrgUnit.E_DIVISION, lstOrg, orgTypes);
            var orgE_COMPANY = LibraryService.GetNearestParentEntity((Guid?)orgId, OrgUnit.E_COMPANY, lstOrg, orgTypes);






            //var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
            //var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

            //Guid? orgId = profile.OrgStructureID;

            //var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
            //var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();


            //var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
            //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
            //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
            //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
            //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);





            #endregion

            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "E_DEPARTMENT", Value = orgE_DEPARTMENT != null ? orgE_DEPARTMENT.OrgStructureName : string.Empty };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "E_UNIT", Value = orgE_UNIT != null ? orgE_UNIT.OrgStructureName : string.Empty };
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "E_DIVISION", Value = orgE_DIVISION != null ? orgE_DIVISION.OrgStructureName : string.Empty };
            HeaderInfo headerInfo6 = new HeaderInfo() { Name = "E_COMPANY", Value = orgE_COMPANY != null ? orgE_COMPANY.OrgStructureName : string.Empty };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5, headerInfo6 };

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

        #region Hien.Nguyen - BC quá trình lương căn bản
        public ActionResult ReportChangingTheBasicSalary([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportChangingTheBasicSalary(model.OrgStructureID, (DateTime)model.DateFrom, (DateTime)model.DateTo, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportChangingTheBasicSalary",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        #region Hien.Nguyen - BC Biến Động Lương

        [HttpPost]
        public ActionResult ReportVariableSalary([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService hrService = new ActionService(UserLogin);
            string status = string.Empty;
            string[] listElement = model.ElementType.Split(',');
            List<Hre_ProfileEntity> listProfileAll = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();

            var objProfile = new List<object>();
            objProfile.AddRange(new object[17]);
            //objProfile[2] = model.OrgStructureID;
            objProfile[15] = 1;
            objProfile[16] = int.MaxValue - 1;
            listProfileAll = hrService.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);

            //loc theo phòng ban
            if (model.OrgStructureID != null && model.OrgStructureID != string.Empty)
            {
                string[] listOrderNumber = model.OrgStructureID.Split(',').ToArray();
                listProfile = listProfileAll.Where(m => m.OrderNumber != null && listOrderNumber.Contains(m.OrderNumber.ToString())).ToList();
            }

            //loc theo phòng ban
            if (model.WorkingPlaceID != null)
            {
                //listProfile = listProfile.Concat(listProfileAll.Where(m => m.WorkPlaceID != null && (Guid)m.WorkPlaceID == (Guid)model.WorkingPlaceID)).ToList(); 
                listProfile = listProfile.Where(m => m.WorkPlaceID != null && m.WorkPlaceID == (Guid)model.WorkingPlaceID).ToList();
            }

            //lọc theo workplace
            //if (model.WorkingPlaceID != null)
            //{
            //    listProfile = listProfile.Concat(listProfileAll.Where(m => m.WorkPlaceID == model.WorkingPlaceID)).ToList();
            //}

            //lọc theo nhân viên
            if (model.ListProfileIDs != null && model.ListProfileIDs.Count > 0)
            {
                var listProfileByIds = listProfileAll.Where(m => model.ListProfileIDs.Contains(m.ID)).ToList();
                listProfile = listProfile.Concat(listProfileByIds).ToList();
            }

            DataTable Table = Services.ReportVariableSalary(listProfile, (DateTime)model.DateFrom, listElement, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkingPlace", Value = model.WorkingPlace };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportVariableSalary",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region Hien.Nguyen - BC Lịch Sử Các Phần Tử Lương

        public ActionResult ReportHistoryPayrollElement([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            ActionService hrService = new ActionService(UserLogin);
            string status = string.Empty;
            string[] listElement = model.ElementType.Split(',');
            List<Hre_ProfileEntity> listProfileAll = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfile = new List<Hre_ProfileEntity>();

            var objProfile = new List<object>();
            objProfile.AddRange(new object[17]);
            //objProfile[2] = model.OrgStructureID;
            objProfile[15] = 1;
            objProfile[16] = int.MaxValue - 1;
            listProfileAll = hrService.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);

            //loc theo phòng ban
            if (model.OrgStructureID != null && model.OrgStructureID != string.Empty)
            {
                string[] listOrderNumber = model.OrgStructureID.Split(',').ToArray();
                listProfile = listProfileAll.Where(m => m.OrderNumber != null && listOrderNumber.Contains(m.OrderNumber.ToString())).ToList();
            }

            //loc theo Shop
            if (model.ShopIDs != null && model.ShopIDs != string.Empty)
            {
                string[] listShop = model.ShopIDs.Split(',').ToArray();
                listProfile = listProfile.Concat(listProfileAll.Where(m => m.ShopID != null && listShop.Contains(m.ShopID.ToString()))).ToList();
            }

            //lọc theo workplace
            if (model.WorkingPlaceID != null)
            {
                listProfile = listProfile.Concat(listProfileAll.Where(m => m.WorkPlaceID == model.WorkingPlaceID)).ToList();
            }

            DataTable Table = Services.ReportHistoryPayrollElement(listProfile, (DateTime)model.DateFrom, (DateTime)model.DateTo, listElement, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo6 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "WorkingPlace", Value = model.WorkingPlace };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "ShopNames", Value = model.ShopNames };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4, headerInfo5, headerInfo6 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportReportHistoryPayrollElement",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region Hien.Nguyen Bổ sung báo cáo danh sách nhân viên âm lương
        public ActionResult ReportProfileSalaryNegative([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            string message = string.Empty;
            ActionService ActionServices = new ActionService(UserLogin);
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "ReportProfileSalaryNegative", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }

            Att_CutOffDurationEntity CutoffDuration = ActionServices.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref message);

            model.ProfileIDs = model.ProfileIDs != null ? model.ProfileIDs : "";
            model.RankID = model.RankID != null ? model.RankID : "";
            List<Guid> SalaryRankIDs = new List<Guid>();
            List<Guid> ProfileIDs = new List<Guid>();
            if (!string.IsNullOrEmpty(model.RankID))
            {
                SalaryRankIDs = model.RankID.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            if (!string.IsNullOrEmpty(model.ProfileIDs))
            {
                ProfileIDs = model.ProfileIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
            }
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.ReportProfileSalaryNegative(model.OrgStructureID, ProfileIDs, CutoffDuration, SalaryRankIDs, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();

            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref message);
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateStart", Value = CutoffDuration.DateStart };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateEnd", Value = CutoffDuration.DateEnd };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "SalaryRankName", Value = model.SalaryRankName };
            HeaderInfo headerInfo5 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo6 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "WorkingPlace", Value = model.WorkingPlace };

            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo5, headerInfo6, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportProfileSalaryNegative",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        [HttpPost]
        public ActionResult ApprevedKaiZenData(List<Guid> selectedIds, DateTime MonthYear, Guid? UnusualEDTypeID, Guid? Currency)
        {
            List<object> listModel = new List<object>();
            string status = string.Empty;
            ActionService action = new ActionService(UserLogin);
            ActionService Hr_services = new ActionService(UserLogin);

            #region Validation
            if (selectedIds == null || selectedIds.Count <= 0)
            {
                return Json(ConstantDisplay.HRM_Message_PleaseSelectData.TranslateString(), JsonRequestBehavior.AllowGet);
            }
            if (UnusualEDTypeID == null)
            {
                return Json("[Phụ Cấp] không thể bỏ trống", JsonRequestBehavior.AllowGet);
            }
            if (Currency == null)
            {
                return Json("[Tiền Tệ] không thể bỏ trống", JsonRequestBehavior.AllowGet);
            }
            #endregion


            Kai_KaiZenDataServices Services = new Kai_KaiZenDataServices();
            int[] Result = Services.ApprevedKaiZenData(selectedIds, MonthYear, (Guid)UnusualEDTypeID, (Guid)Currency);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #region Sal_ReportProfile
        public ActionResult GetReportProfileList([DataSourceRequest] DataSourceRequest request, Sal_ReportProfileSearchModel model)
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
            #region MyRegion
            //  ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    PageSize = request.PageSize,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request),
            //    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            //};
            //if((model.IsCreateTemplate==true && model.IsCreateTemplateForDynamicGrid==true) || model.ExportId != Guid.Empty)
            //{
            //    lstModel = new ListQueryModel
            //    {
            //        PageSize = int.MaxValue - 1,
            //        PageIndex = 1,
            //        Filters = ExtractFilterAttributes(request),
            //        Sorts = ExtractSortAttributes(request),
            //        AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            //    };
            //}
            #endregion


            var listProfile = service.GetData<Hre_ProfileEntity>(lstModel, ConstantSql.hrm_sal_sp_getdata_Profile, UserLogin, ref status);
            var ReportServices = new Sal_ReportService();
            var result = ReportServices.GetReportProfile(listProfile, model.CutOffDurationID, UserLogin);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DataExport", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_ReportProfileEntity",
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
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            //return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = int.MaxValue };
            #region mapping dataTable to dataList
            List<Sal_ReportProfileModel> dataList = new List<Sal_ReportProfileModel>();
            Sal_ReportProfileModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Sal_ReportProfileModel).GetProperties(flags)
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
                    aTSource = new Sal_ReportProfileModel();
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
        public ActionResult ExportPaysipForCompany([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            ActionService service1 = new ActionService(UserLogin);
            Sal_ReportService service = new Sal_ReportService();
            var hrService = new ActionService(UserLogin);
            string status = string.Empty;
            DateTime _dateStart = DateTime.Now;
            DateTime _dateEnd = DateTime.Now;
            var attCutOffDurationServices = new Att_CutOffDurationServices();
            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            if (cutOffDurationEntity != null)
            {
                _dateStart = cutOffDurationEntity.DateStart;
                _dateEnd = cutOffDurationEntity.DateEnd;
            }

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            var lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            var result = service.ExportPayroll(model.ExportId, lstProfiles, _dateStart, _dateEnd, UserLogin);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendMailPayslip([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            ActionService service1 = new ActionService(UserLogin);
            Sal_ReportService service = new Sal_ReportService();
            var hrService = new ActionService(UserLogin);
            string status = string.Empty;
            DateTime _dateStart = DateTime.Now;
            DateTime _dateEnd = DateTime.Now;
            var attCutOffDurationServices = new Att_CutOffDurationServices();
            var cutOffDurationEntity = service1.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            if (cutOffDurationEntity != null)
            {
                _dateStart = cutOffDurationEntity.DateStart;
                _dateEnd = cutOffDurationEntity.DateEnd;
            }
            List<Guid> lstProIDs = new List<Guid>();
            if (model.ProfileIDs != null && model.ProfileIDs != string.Empty)
            {
                lstProIDs = model.ProfileIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
            }

            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;
            var lstProfiles = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
            if (lstProIDs.Count > 0)
            {
                lstProfiles = lstProfiles.Where(hr => lstProIDs.Contains(hr.ID)).ToList();
            }

            var result = service.SendMailPayslip(model.ExportId, lstProfiles, _dateStart, _dateEnd, UserLogin);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Send mail payslip cá nhân
        public ActionResult SendMailPayslipOfProfile([DataSourceRequest] DataSourceRequest request, Sal_PayrollTableItemModelExportPDF model)
        {
            var isSuccess = false;
            if (model.ExportId != Guid.Empty)
            {
                ActionService actServices = new ActionService(UserLogin);
                ListQueryModel lstModel = new ListQueryModel
                {
                    PageSize = int.MaxValue - 1,
                    PageIndex = 1,
                    Filters = ExtractFilterAttributes(request),
                    Sorts = ExtractSortAttributes(request),
                    AdvanceFilters = ExtractAdvanceFilterAttributes(model)
                };
                string status = string.Empty;
                var service = new BaseService();
                var reportService = new Sal_ReportService();
                var listEntity = service.GetData<Sal_PayrollTableItemEntity>(lstModel, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfile, UserLogin, ref status).Translate<Sal_PayrollTableItemModel>();
                var profileEntity = actServices.GetByIdUseStore<Hre_ProfileEntity>(model.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                DataTable table = new DataTable();
                #region add row
                if (listEntity != null && profileEntity != null)
                {
                    DataRow dr = table.NewRow();
                    table.Columns.Add("CodeEmp");
                    table.Columns.Add("ProfileName");
                    table.Columns.Add("JobTitleName");
                    table.Columns.Add("PositionName");
                    table.Columns.Add("OrgStructureName");
                    table.Columns.Add("SalaryClassName");
                    table.Columns.Add("WorkPlaceName");
                    table.Columns.Add("CodeTax");

                    dr["CodeEmp"] = profileEntity.CodeEmp;
                    dr["ProfileName"] = profileEntity.ProfileName;
                    dr["JobTitleName"] = profileEntity.JobTitleName;
                    dr["PositionName"] = profileEntity.PositionName;
                    dr["OrgStructureName"] = profileEntity.OrgStructureName;
                    dr["SalaryClassName"] = profileEntity.SalaryClassName;
                    dr["WorkPlaceName"] = profileEntity.WorkPlaceName;
                    dr["CodeTax"] = profileEntity.CodeTax;

                    //Sort theo tiền tệ
                    foreach (var i in listEntity)
                    {
                        Double value = 0;
                        if (Double.TryParse(i.Value, out value))
                        {
                            i.Amount = value;
                        }
                        else
                        {
                            i.Amount = double.MaxValue;
                        }
                    }
                    listEntity = listEntity.OrderByDescending(m => m.Amount).ToList();

                    //Lọc các cột phụ cấp cộng ra
                    List<Sal_PayrollTableItemModel> listUnusual = new List<Sal_PayrollTableItemModel>();
                    listUnusual = listEntity.Where(m => m.ElementType == CatElementType.AllowancesOut.ToString()).ToList();
                    if (listUnusual != null && listUnusual.Count > 0)
                    {
                        for (int i = 0; i < listUnusual.Count; i++)
                        {
                            //tạo các phụ cấp
                            table.Columns.Add("PC_Label" + (i + 1).ToString());
                            dr["PC_Label" + (i + 1).ToString()] = listUnusual[i].Name;

                            //tạo các phụ cấp
                            table.Columns.Add("PC_Value" + (i + 1).ToString());
                            dr["PC_Value" + (i + 1).ToString()] = listUnusual[i].Value;

                            ////Loại bỏ các phần tử là phụ cấp khỏi list tổng
                            //listEntity.Remove(listUnusual[i]);
                        }
                    }

                    //Lọc các cột phụ cấp trừ ra
                    listUnusual = new List<Sal_PayrollTableItemModel>();
                    listUnusual = listEntity.Where(m => m.ElementType == CatElementType.AllowancesOut_Minus.ToString()).ToList();
                    if (listUnusual != null && listUnusual.Count > 0)
                    {
                        for (int i = 0; i < listUnusual.Count; i++)
                        {
                            //tạo các phụ cấp
                            table.Columns.Add("PC_Minus_Label" + (i + 1).ToString());
                            dr["PC_Minus_Label" + (i + 1).ToString()] = listUnusual[i].Name;

                            //tạo các phụ cấp
                            table.Columns.Add("PC_Minus_Value" + (i + 1).ToString());
                            dr["PC_Minus_Value" + (i + 1).ToString()] = listUnusual[i].Value;

                            ////Loại bỏ các phần tử là phụ cấp khỏi list tổng
                            //listEntity.Remove(listUnusual[i]);
                        }
                    }

                    foreach (var item in listEntity)
                    {
                        Double value = 0;
                        if (!table.Columns.Contains(item.Code))
                        {
                            table.Columns.Add(item.Code, typeof(Double));
                        }
                        if (table.Columns.Contains(item.Code))
                        {
                            if (item.ValueType == typeof(Double).Name)
                            {
                                Double.TryParse(item.Value, out value);
                            }
                            dr[item.Code] = value;
                        }
                    }
                    table.Rows.Add(dr);
                }
                #endregion
                if (table.Rows.Count > 0 && listEntity != null && listEntity.Count > 0)
                {
                    string fullPath = string.Empty;
                    if (model.ExportPDF != null && model.ExportPDF == true)
                    {
                        fullPath = ExportService.Export(model.ExportId, table, ExportFileType.PDF);
                    }
                    else
                    {
                        fullPath = ExportService.Export(model.ExportId, table, model.ExportType);
                    }


                    DateTime dateStart = DateTime.Now;
                    DateTime dateEnd = DateTime.Now;
                    Att_CutOffDurationEntity cutoffDuration = new Att_CutOffDurationEntity();
                    if (model.CutOffDurationID != null && model.CutOffDurationID != Guid.Empty)
                    {
                        ActionService action = new ActionService(UserLogin);
                        cutoffDuration = action.GetByIdUseStore<Att_CutOffDurationEntity>(model.CutOffDurationID.Value, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
                        if (cutoffDuration != null)
                        {
                            dateStart = cutoffDuration.DateStart;
                            dateEnd = cutoffDuration.DateEnd;
                        }
                    }
                    string emailTo = profileEntity.Email;
                    if (emailTo != null && emailTo != string.Empty)
                    {
                        isSuccess = reportService.CheckSendMail(emailTo, fullPath, dateStart, dateEnd, profileEntity);
                    }
                }
                else
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }



        #region [Hien.Nguyen] Phan Tich Quy Luong Du Kien
        private List<Cat_OrgStructureEntity> ListChildOrgStructure;
        private List<Cat_OrgStructureEntity> GetChildOrgStructure(List<Cat_OrgStructureEntity> listOrg, Cat_OrgStructureEntity currentOrg)
        {
            var listChildItem = listOrg.Where(m => m.ParentID != null && m.ParentID == currentOrg.ID).ToList();
            if (listChildItem.Count <= 0)
            {
                return ListChildOrgStructure;
            }
            foreach (var i in listChildItem)
            {
                ListChildOrgStructure.Add(i);
                GetChildOrgStructure(listOrg, i);
            }
            return ListChildOrgStructure;
        }
        public ActionResult ValidateAnalysisPayrollEstimat(Sal_PayrollEstimateModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_PayrollEstimateModel>(model, "Sal_PayrollEstimate", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            var result = new object[] { "success", message };
            return Json(result);
        }
        public ActionResult AnalysisPayrollEstimat([DataSourceRequest] DataSourceRequest request, Sal_PayrollEstimateModel model)
        {
            BaseService services = new BaseService();
            ActionService actionServices = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Sal_PayrollEstimateDetailModel> listResult = new List<Sal_PayrollEstimateDetailModel>();
            List<Sal_PayrollEstimateDetailModel> result = new List<Sal_PayrollEstimateDetailModel>();
            ListChildOrgStructure = new List<Cat_OrgStructureEntity>();


            #region Get Data
            listModel = new List<object>();
            listModel.AddRange(new object[5]);
            listModel[3] = 1;
            listModel[4] = Int32.MaxValue - 1;
            var listOrgStructure = actionServices.GetData<Cat_OrgStructureEntity>(listModel, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status);

            listModel = new List<object>();
            listModel.Add(null);
            listModel.Add(null);
            listModel.Add(1);
            listModel.Add(Int32.MaxValue - 1);
            var listOrgStructureType = actionServices.GetData<Cat_OrgStructureTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[17]);
            listModel[15] = 1;
            listModel[16] = int.MaxValue - 1;
            var listProfile = actionServices.GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            var listPayrollGroup = actionServices.GetData<Cat_PayrollGroupEntity>(listModel, ConstantSql.hrm_cat_sp_get_payrollGroup, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[2]);
            listModel[0] = 1;
            listModel[1] = Int32.MaxValue - 1;
            var listPayrollEstimateDetail = actionServices.GetData<Sal_PayrollEstimateDetailEntity>(listModel, ConstantSql.hrm_sal_sp_get_EstimateDetail, ref status);

            #endregion

            listOrgStructure = GetChildOrgStructure(listOrgStructure, listOrgStructure.FirstOrDefault(m => m.ID == (Guid)model.OrgStructureID));

            //loc nv
            if (model.StatusEmp != null && model.StatusEmp != string.Empty)
            {
                listProfile = listProfile.Where(m => m.StatusSyn == model.StatusEmp).ToList();
            }
            //loc theo loai phong ban
            if (model.OrgStructureType != null && model.OrgStructureType != string.Empty)
            {
                string[] arrayOrgstructureType = model.OrgStructureType.Split(',');
                listOrgStructure = listOrgStructure.Where(m => arrayOrgstructureType.Contains(m.OrgStructureTypeID.ToString())).ToList();
            }

            //lay du lieu  cau hinh
            Sys_AllSettingServices sysServices = new Sys_AllSettingServices();
            var AllSetting = sysServices.GetData<Sys_AllSettingModel>("HRM_SAL_PAYROLL_ESTIMATE_SALRYAVERAGE", ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).FirstOrDefault();

            foreach (var i in listOrgStructure)
            {
                Sal_PayrollEstimateDetailModel item = new Sal_PayrollEstimateDetailModel();
                //item.ID = Guid.NewGuid();
                item.OrgStructureCode = i.Code;
                item.OrgStructureName = i.OrgStructureName;
                item.OrgStructureID = (Guid?)i.ID;
                item.QuantityEmp = listProfile.Count(m => m.OrgStructureID == i.ID);
                //du lieu lay trong config
                if (item.QuantityEmp != 0)
                {
                    item.SalaryAverage = AllSetting != null ? double.Parse(AllSetting.Value1.ToString()) : 54000;
                }
                else
                {
                    item.SalaryAverage = 0;
                }

                var _tmp = listPayrollEstimateDetail.FirstOrDefault(m => m.PayrollEstimateID == null && m.OrgStructureID == i.ID);
                if (_tmp != null)
                {
                    item.LeaveUnpaid = _tmp.LeaveUnpaid;
                    item.LeaveUnpaidView = _tmp.LeaveUnpaidView;
                    item.OvertimeHoliday = _tmp.OvertimeHoliday;
                    item.OvertimeNightHoliday = _tmp.OvertimeNightHoliday;
                    item.OvertimeNightNormal = _tmp.OvertimeNightNormal;
                    item.OvertimeNightWeekend = _tmp.OvertimeNightWeekend;
                    item.OvertimeNormal = _tmp.OvertimeNormal;
                    item.OvertimeWeekend = _tmp.OvertimeWeekend;
                }
                else
                {
                    item.LeaveUnpaid = 0;
                    item.LeaveUnpaidView = 0;
                    item.OvertimeHoliday = 0;
                    item.OvertimeNightHoliday = 0;
                    item.OvertimeNightNormal = 0;
                    item.OvertimeNightWeekend = 0;
                    item.OvertimeNormal = 0;
                    item.OvertimeWeekend = 0;
                }
                listResult.Add(item);
            }

            var TotalResult = listResult.Translate<Sal_PayrollEstimateDetailModel>();
            return new JsonResult() { Data = TotalResult.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult ComputePayrollEstimate([Bind(Prefix = "models")]List<Sal_PayrollEstimateDetailModel> listModel, [Bind(Prefix = "params")]Sal_PayrollEstimateModel model)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_PayrollEstimateModel>(model, "Sal_PayrollEstimate", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            Sal_PayrollEstimateServices Services = new Sal_PayrollEstimateServices();
            listModel.ForEach(m => m.LeaveUnpaid = m.LeaveUnpaidView);
            bool result = Services.ComputePayrollEstimate(listModel.Translate<Sal_PayrollEstimateDetailEntity>(), model.Copy<Sal_PayrollEstimateEntity>(), UserLogin);
            return Json(result);
        }


        public ActionResult CreateTemplatePayrollEstimat([DataSourceRequest] DataSourceRequest request, Sal_PayrollEstimateModel model)
        {
            Sal_PayrollEstimateServices Services = new Sal_PayrollEstimateServices();
            var Result = Services.GetTemplatePayrollEstimate(model.Copy<Sal_PayrollEstimateEntity>());
            Result.ForEach(m => m.LeaveUnpaidView = (float)m.LeaveUnpaid);
            return Json(Result.ToDataSourceResult(request));
        }

        public ActionResult SaveTemplatePayrollEstimat([Bind(Prefix = "models")]List<Sal_PayrollEstimateDetailModel> listModel)
        {
            Sal_PayrollEstimateServices Services = new Sal_PayrollEstimateServices();
            listModel.ForEach(m => m.LeaveUnpaid = m.LeaveUnpaidView);
            bool result = Services.SaveTemplatePayrollEstimate(listModel.Translate<Sal_PayrollEstimateDetailEntity>());
            return Json(result);
        }

        #endregion

        #region Tin.Nguyen Report Quỹ Lương Dự Kiến
        public ActionResult ReportPayrollEstimate([DataSourceRequest] DataSourceRequest request, Sal_PayrollEstimateModel model)
        {
            Sal_ReportService Services = new Sal_ReportService();
            DataTable Table = Services.GetPayrollEstimate(model.CutOffDurationID, model.OrgStructureID, model.PayrollGroupID, model.OrgStructureType, model.StatusEmp, model.RateAdjust, model.BonusBudget, UserLogin, model.IsCreateTemplate);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }
            ActionService ActionServices = new ActionService(UserLogin);
            string status = string.Empty;


            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Sal_PayrollEstimateDetailEntity",
                    OutPutPath = path,
                    //  HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, null, model.ExportType);
                return Json(fullPath);
            }


            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region  Hold Lương

        #region Hien.Nguyen BC Nhân Viên Bị Giữ Lương

        public ActionResult ReportHoldSalary([DataSourceRequest] DataSourceRequest request, Sal_ReportBasicSalaryMonthlyModel model)
        {
            ActionService ActionServices = new ActionService(UserLogin);
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ReportBasicSalaryMonthlyModel>(model, "Sal_ReportHoldSalary", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }

            Sal_ReportService Services = new Sal_ReportService();
            ActionService hrService = new ActionService(UserLogin);
            string status = string.Empty;

            Att_CutOffDurationEntity CutOffDuration = ActionServices.GetByIdUseStore<Att_CutOffDurationEntity>((Guid)model.CutOffDurationID, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);

            DataTable Table = Services.ReportHoldSalary(CutOffDuration, model.OrgStructureID, model.WorkingPlaceIDs, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = Table;
                isDataTable = true;
            }

            Hre_ProfileEntity profileByID = new Hre_ProfileEntity();
            if (model.UserID != null)
            {
                profileByID = ActionServices.GetByIdUseStore<Hre_ProfileEntity>((Guid)model.UserID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
            }
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "MonthYear", Value = CutOffDuration.MonthYear };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "WorkingPlace", Value = model.WorkingPlace };
            HeaderInfo headerInfo3 = new HeaderInfo() { Name = "CodeEmp", Value = profileByID.CodeEmp };
            HeaderInfo headerInfo4 = new HeaderInfo() { Name = "DateExport", Value = DateTime.Now };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2, headerInfo3, headerInfo4 };
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "ReportHoldSalary",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, Table, listHeaderInfo, model.ExportType);
                return Json(fullPath);
            }
            return new JsonResult() { Data = Table.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region Lưu NV bị giữ lương
        public ActionResult SaveChangeHoldSalary([Bind(Prefix = "totalRecord")] List<Sal_HoldSalaryModel> TotalAtt_OvertimeModel)
        {
            List<Sal_HoldSalaryModel> lstReturn = new List<Sal_HoldSalaryModel>();
            Sal_HoldSalaryModel _return = new Sal_HoldSalaryModel();

            Sal_HoldSalaryServices HoldSalaryServices = new Sal_HoldSalaryServices();
            ResultsObject Result = HoldSalaryServices.SaveChangeHoldSalary(TotalAtt_OvertimeModel.Translate<Sal_HoldSalaryEntity>());

            //DataSourceRequest request = new DataSourceRequest()
            //{
            //    Page = 1,
            //    PageSize = 50
            //};
            return Json(Result);
        }
        #endregion

        #endregion

        public ActionResult GetOutputPath(string OutputPath)
        {
            //string status = string.Empty;
            CompressFile.CreateZipFile(OutputPath.Replace("xml", "zip"), OutputPath, new DirectoryInfo("Log/"));

            //status = Common.MultiExport("Log/", false, OutputPath);
            return Json(OutputPath);
        }
    }
}
