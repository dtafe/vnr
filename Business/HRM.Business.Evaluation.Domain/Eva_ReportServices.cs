using System;
using System.Collections.Generic;
using System.Xml;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System.Linq;
using HRM.Presentation.Evaluation.Models;
using System.Data;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Attendance;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Payroll.Domain;
using HRM.Business.Payroll.Models;
using System.Timers;
using VnResource.Helper.Linq;
using HRM.Business.HrmSystem.Domain;
namespace HRM.Business.Evaluation.Domain
{
    public class Eva_ReportServices
    {

        //Biến để get orderNumber của phòng ban
        string orderNumber = string.Empty;

        #region BC HeadCount Doanh Số
        DataTable CreateReportHCSalesSchema(string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable datatable = new DataTable("Hre_ReportHCSalesEntity");
                string key1 = "SaleIn";
                string key2 = "SaleOut";
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.CodeEmp);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.ProfileName);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.PositionName);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.JobTitleName);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.ProfileSupervisorName);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.DateHire, typeof(DateTime));
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.Channel);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.Region);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.Area);
                datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.WorkingPlaceName);

                var salesTypeServices = new Eva_SalesTypeServices();
                var lstObjSalesType = new List<object>();
                lstObjSalesType.Add(null);
                lstObjSalesType.Add(1);
                lstObjSalesType.Add(int.MaxValue - 1);
                var lstSalesType = salesTypeServices.GetData<Eva_SalesTypeEntity>(lstObjSalesType, ConstantSql.hrm_eva_sp_get_SalesType, userLogin, ref status).Select(s => s.Code).ToList();
                if (lstSalesType != null && lstSalesType.Count > 0)
                {
                    lstSalesType = lstSalesType.Where(s => s != key1 && s != key2).ToList();
                }

                for (int i = 1; i <= 12; i++)
                {
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.TargetSalesIn + "_" + i, typeof(double));
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.ActSalesIn + "_" + i, typeof(double));
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.SalesIn + "_" + i, typeof(double));
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.TargetSalesOut + "_" + i, typeof(double));
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.ActSalesOut + "_" + i, typeof(double));
                    datatable.Columns.Add(Hre_ReportHCSalesEntity.FieldNames.SalesOut + "_" + i, typeof(double));
                    foreach (var item in lstSalesType)
                    {
                        datatable.Columns.Add(item + "_" + i, typeof(double));
                    }
                }



                return datatable;
            }
        }

        public DataTable GetReportHCSales(DateTime DateSearch, Guid orgID, bool isCreateTemplate,string userLogin)
        {

            Hre_ReportServices reportServices = new Hre_ReportServices();
            DataTable table = CreateReportHCSalesSchema(userLogin);
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                if (isCreateTemplate)
                {
                    return table.ConfigTable();
                }
                string key1 = "SaleIn";
                string key2 = "SaleOut";
                var count = 1;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var serviceProfile = new Hre_ProfileServices();
                var baseService = new BaseService();

                var orgsService = new Cat_OrgStructureServices();

                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = orgTypeService.GetData<Cat_OrgStructureType>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, userLogin, ref status);

                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();
                var lstOrgName = lstallorgs.Where(s => s.ID == orgID).FirstOrDefault();

                var listorgid = lstorgs.Select(s => new { s.ID, s.OrderNumber, s.Code, s.OrgStructureName }).ToList();

                //Xử Lý lấy tất cả nhân viên trong phòng ban đã chọn và group 1 cấp
                var orgIDs = string.Empty;
                orderNumber = string.Empty;

                foreach (var item in listorgid)
                {
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);
                }
                if (orderNumber.IndexOf(',') > 0)
                    orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(orderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin, ref status).Select(s => s.ID).ToList();

                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);

                var lstprofile = reportServices.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).ToList();

                var salesTypeServices = new Eva_SalesTypeServices();
                var lstObjSalesType = new List<object>();
                lstObjSalesType.Add(null);
                lstObjSalesType.Add(1);
                lstObjSalesType.Add(int.MaxValue);
                var lstSalesType = salesTypeServices.GetData<Eva_SalesTypeEntity>(lstObjSalesType, ConstantSql.hrm_eva_sp_get_SalesType, userLogin, ref status).Select(s => s.Code).ToList();

                var saleEvaluationServices = new Eva_SaleEvaluationServices();
                var lstObjSaleEvaluation = new List<object>();
                lstObjSaleEvaluation.Add(null);
                lstObjSaleEvaluation.Add(null);
                lstObjSaleEvaluation.Add(null);
                lstObjSaleEvaluation.Add(1);
                lstObjSaleEvaluation.Add(int.MaxValue);
                var lstSaleEvaluation = saleEvaluationServices.GetData<Eva_SaleEvaluationEntity>(lstObjSaleEvaluation, ConstantSql.hrm_eva_sp_get_SaleEvaluation, userLogin, ref status).ToList();

                foreach (var org in listorgid)
                {
                    DataRow row = table.NewRow();

                    row[Hre_ReportHCSalesEntity.FieldNames.CodeEmp] = org == null ? string.Empty : org.OrgStructureName;

                    //xử lý đếm nhân viên của phòng ban con
                    orderNumber = string.Empty;
                    orderNumber += org.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, org.ID);

                    if (orderNumber.IndexOf(',') > 0)
                        orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                    var lstObjOrgByOrderNumberCount = new List<object>();
                    lstObjOrgByOrderNumberCount.Add(orderNumber);
                    var lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin, ref status).ToList();
                    //if(count <= 0){
                    //    continue;
                    //}
                    bool addTitle = false;
                    foreach (var item in lstOrgByOrderNumberCount)
                    {
                        var lstprofilebyOrg = lstprofile.Where(s => s.OrgStructureID != null && item.ID == s.OrgStructureID.Value && s.DateHire != null && s.DateHire.Value.Year <= DateSearch.Year && s.DateQuit == null).Select(s => s.ID).ToList();
                        if (lstprofilebyOrg == null && lstprofilebyOrg.Count <= 0)
                        {
                            continue;
                        }
                        var lstSaleEvaluations = lstSaleEvaluation.Where(s => lstprofilebyOrg.Contains(s.ProfileID.Value) && s.Year != null && s.Year.Value.Year <= DateSearch.Year).ToList();
                        count = 0;
                        count = lstSaleEvaluation.Count;
                        foreach (var sale in lstSaleEvaluations)
                        {

                            var lstProfileResult = lstprofile.Where(s => s.ID == sale.ProfileID.Value).FirstOrDefault();

                            var orgName = reportServices.GetParentOrg(lstallorgs, lstOrgType, lstProfileResult.OrgStructureID);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                                if (orgName.Count < 3)
                                {
                                    orgName.Insert(0, string.Empty);
                                }
                            }

                            DataRow row1 = table.NewRow();

                            row1[Hre_ReportHCSalesEntity.FieldNames.CodeEmp] = lstProfileResult == null ? string.Empty : lstProfileResult.CodeEmp;
                            row1[Hre_ReportHCSalesEntity.FieldNames.ProfileName] = lstProfileResult == null ? string.Empty : lstProfileResult.ProfileName;
                            row1[Hre_ReportHCSalesEntity.FieldNames.PositionName] = lstProfileResult == null ? string.Empty : lstProfileResult.PositionName;
                            row1[Hre_ReportHCSalesEntity.FieldNames.JobTitleName] = lstProfileResult == null ? string.Empty : lstProfileResult.JobTitleName;
                            row1[Hre_ReportHCSalesEntity.FieldNames.ProfileSupervisorName] = lstProfileResult == null ? string.Empty : lstProfileResult.SupervisorName;
                            row1[Hre_ReportHCSalesEntity.FieldNames.DateHire] = lstProfileResult == null ? string.Empty : lstProfileResult.DateHire.Value.ToShortDateString();
                            row1[Hre_ReportHCSalesEntity.FieldNames.Channel] = orgName[2];
                            row1[Hre_ReportHCSalesEntity.FieldNames.Region] = orgName[1];
                            row1[Hre_ReportHCSalesEntity.FieldNames.Area] = orgName[0];
                            row1[Hre_ReportHCSalesEntity.FieldNames.WorkingPlaceName] = lstProfileResult == null ? string.Empty : lstProfileResult.WorkPlaceName;
                            for (int i = 1; i <= 12; i++)
                            {
                                var saleCode = sale.SalesTypeCode + "_" + i;
                                if (sale.SalesTypeCode == key1 && sale.Year != null && sale.Year.Value.Month == i)
                                {
                                    row1[Hre_ReportHCSalesEntity.FieldNames.TargetSalesIn + "_" + i] = sale.TagetNumber;
                                    row1[Hre_ReportHCSalesEntity.FieldNames.ActSalesIn + "_" + i] = sale.ResultNumber;
                                    row1[Hre_ReportHCSalesEntity.FieldNames.SalesIn + "_" + i] = sale.ResultPercent != null ? sale.ResultPercent.Value.ToString(ConstantFormat.HRM_Format_Number_Double2) : null;
                                }
                                if (sale.SalesTypeCode == key2 && sale.Year != null && sale.Year.Value.Month == i)
                                {
                                    row1[Hre_ReportHCSalesEntity.FieldNames.TargetSalesOut + "_" + i] = sale.TagetNumber;
                                    row1[Hre_ReportHCSalesEntity.FieldNames.ActSalesOut + "_" + i] = sale.ResultNumber;
                                    row1[Hre_ReportHCSalesEntity.FieldNames.SalesOut + "_" + i] = sale.ResultPercent != null ? sale.ResultPercent.Value.ToString(ConstantFormat.HRM_Format_Number_Double2) : null;
                                }

                                if (lstSaleEvaluation.Where(s => s.Year != null).Select(s => s.Year.Value.Month).ToList().Contains(i) && table.Columns.Contains(saleCode))
                                {
                                    row1[sale.SalesTypeCode + "_" + i] = sale.ResultPercent != null ? sale.ResultPercent.Value.ToString(ConstantFormat.HRM_Format_Number_Double2) : null;
                                }
                            }
                            if (!addTitle)
                            {
                                table.Rows.Add(row);
                                addTitle = true;
                            }
                            table.Rows.Add(row1);
                        }

                    }


                }



                return table.ConfigTable(true);
            }
        }

        #endregion
        #region Tong hop dl đánh giá
        public List<Eva_EvalutionDataEntity> SummaryEvalutionData(int year, Guid? _TimesGetDataID, string orgStructureID, DateTime? _daystart, DateTime? _dayend,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                //    #region Get Data
                string status = string.Empty;
                List<Eva_EvalutionDataEntity> lstEvalutionDataEntity = new List<Eva_EvalutionDataEntity>();

                //    //ds nv
                var hrService = new Hre_ProfileServices();
                var service = new BaseService();
                List<object> strOrgIDs = new List<object>();
                strOrgIDs.AddRange(new object[1]);
                strOrgIDs[0] = (object)orgStructureID;

                var lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_eva_sp_getdata_ProfileByOrg, userLogin, ref status).ToList();
                if (lstProfile == null || lstProfile.Count == 0)
                    return lstEvalutionDataEntity;
                List<Guid> lstProfileIDs = lstProfile.Select(s => s.ID).ToList();
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                var lstattattendancetable = new List<Att_AttendanceTable>().Select(d => new
                {
                    d.LateEarlyDeductionHours,
                    d.ProfileID
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lstattattendancetable.AddRange(unitOfWork.CreateQueryable<Att_AttendanceTable>(Guid.Empty,
                        d => lstProfileID.Contains(d.ProfileID) && d.MonthYear >= _daystart
                            && d.MonthYear <= _dayend).Select(d => new
                            {
                                d.LateEarlyDeductionHours,
                                d.ProfileID
                            }).ToList());
                }

                #region Lay cau hinh luu vao cot C1->C16
                Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
                string DataC1 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C1);
                string DataC2 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C2);
                string DataC3 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C3);
                string DataC4 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C4);
                string DataC5 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C5);
                string DataC6 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C6);
                string DataC7 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C7);
                string DataC8 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C8);
                string DataC9 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C9);
                string DataC10 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C10);
                string DataC11 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C11);
                string DataC12 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C12);
                string DataC13 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C13);
                string DataC14 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C14);
                string DataC15 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C15);
                string DataC16 = sysServices.GetConfigValue<string>(AppConfig.HRM_EVA_CONFIG_SAVE_C16);
                #endregion

                string[] lstDataC1 = null;
                string[] lstDataC2 = null;
                string[] lstDataC3 = null;
                string[] lstDataC4 = null;
                string[] lstDataC5 = null;
                string[] lstDataC6 = null;
                string[] lstDataC7 = null;
                string[] lstDataC8 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC1 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC2 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC3 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC4 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC5 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC6 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC7 = null;
                List<Cat_LeaveDayTypeEntity> templstleavedaytypeC8 = null;
                List<object> tempobjparam = new List<object>();
                tempobjparam.Add(orgStructureID);
                var tempcatService = new Cat_LeaveDayTypeServices();
                var templstcatLeaveDayType = tempcatService.GetData<Cat_LeaveDayTypeEntity>(tempobjparam, ConstantSql.hrm_cat_getdata_LeaveDayType, userLogin, ref status).ToList();
                if (templstcatLeaveDayType.Count > 0)
                {

                    #region Loai Nghi
                    #region C1
                    if (DataC1 != null)
                    {
                        DataC1 = DataC1.Replace(" ", "").Trim();
                        lstDataC1 = DataC1.Split(',').ToArray();
                        if (lstDataC1 != null && lstDataC1.Count() > 0)
                        {
                            templstleavedaytypeC1 = templstcatLeaveDayType.Where(s => lstDataC1.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C2
                    if (DataC2 != null)
                    {
                        DataC2 = DataC2.Replace(" ", "").Trim();
                        lstDataC2 = DataC2.Split(',').ToArray();
                        if (lstDataC2 != null && lstDataC2.Count() > 0)
                        {
                            templstleavedaytypeC2 = templstcatLeaveDayType.Where(s => lstDataC2.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C3
                    if (DataC3 != null)
                    {
                        DataC3 = DataC3.Replace(" ", "").Trim();
                        lstDataC3 = DataC3.Split(',').ToArray();
                        if (lstDataC3 != null && lstDataC3.Count() > 0)
                        {
                            templstleavedaytypeC3 = templstcatLeaveDayType.Where(s => lstDataC3.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C4
                    if (DataC4 != null)
                    {
                        DataC4 = DataC4.Replace(" ", "").Trim();
                        lstDataC4 = DataC4.Split(',').ToArray();
                        if (lstDataC4 != null && lstDataC4.Count() > 0)
                        {
                            templstleavedaytypeC4 = templstcatLeaveDayType.Where(s => lstDataC4.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C5
                    if (DataC5 != null)
                    {
                        DataC5 = DataC5.Replace(" ", "").Trim();
                        lstDataC5 = DataC5.Split(',').ToArray();
                        if (lstDataC5 != null && lstDataC5.Count() > 0)
                        {
                            templstleavedaytypeC5 = templstcatLeaveDayType.Where(s => lstDataC5.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C6
                    if (DataC6 != null)
                    {
                        DataC6 = DataC6.Replace(" ", "").Trim();
                        lstDataC6 = DataC6.Split(',').ToArray();
                        if (lstDataC6 != null && lstDataC6.Count() > 0)
                        {
                            templstleavedaytypeC6 = templstcatLeaveDayType.Where(s => lstDataC6.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C7
                    if (DataC7 != null)
                    {
                        DataC7 = DataC7.Replace(" ", "").Trim();
                        lstDataC7 = DataC7.Split(',').ToArray();
                        if (lstDataC7 != null && lstDataC7.Count() > 0)
                        {
                            templstleavedaytypeC7 = templstcatLeaveDayType.Where(s => lstDataC7.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #region C8
                    if (DataC8 != null)
                    {
                        DataC8 = DataC8.Replace(" ", "").Trim();
                        lstDataC8 = DataC8.Split(',').ToArray();
                        if (lstDataC8 != null && lstDataC8.Count() > 0)
                        {
                            templstleavedaytypeC8 = templstcatLeaveDayType.Where(s => lstDataC8.Contains(s.Code)).ToList();
                        }
                    }
                    #endregion
                    #endregion
                }
                #region doi tuong loc theo loai nghi
                //var catService = new Cat_LeaveDayTypeServices();
                //List<object> objparam = new List<object>();
                //objparam.Add(orgStructureID);
                //var lstcatLeaveDayType = catService.GetData<Cat_LeaveDayTypeEntity>(objparam, ConstantSql.hrm_cat_getdata_LeaveDayType, ref status).ToList();
                //var lstleavedaytypeC1 = lstcatLeaveDayType.Where(s => s.Code == "P").ToList();
                //var lstleavedaytypeC2 = lstcatLeaveDayType.Where(s => s.Code == "M" || s.Code == "SM" || s.Code == "SP" || s.Code == "DL" || s.Code == "DSP").ToList();
                //var lstleavedaytypeC3 = lstcatLeaveDayType.Where(s => s.Code == "SU" || s.Code == "SC").ToList();
                //var lstleavedaytypeC4 = lstcatLeaveDayType.Where(s => s.Code == "DP" || s.Code == "SD").ToList();
                //var lstleavedaytypeC5 = lstcatLeaveDayType.Where(s => s.Code == "AL").ToList();
                //var lstleavedaytypeC7 = lstcatLeaveDayType.Where(s => s.Code == "D").ToList();
                Dictionary<string, List<Cat_LeaveDayTypeEntity>> dicLeaveByType = new Dictionary<string, List<Cat_LeaveDayTypeEntity>>();
                //dicLeaveByType.Add("C1", lstleavedaytypeC1);
                //dicLeaveByType.Add("C2", lstleavedaytypeC2);
                //dicLeaveByType.Add("C3", lstleavedaytypeC3);
                //dicLeaveByType.Add("C4", lstleavedaytypeC4);
                //dicLeaveByType.Add("C5", lstleavedaytypeC5);
                //dicLeaveByType.Add("C7", lstleavedaytypeC7);
                if (templstleavedaytypeC1 != null && templstleavedaytypeC1.Count > 0)
                    dicLeaveByType.Add("C1", templstleavedaytypeC1);
                if (templstleavedaytypeC2 != null && templstleavedaytypeC2.Count > 0)
                    dicLeaveByType.Add("C2", templstleavedaytypeC2);
                if (templstleavedaytypeC3 != null && templstleavedaytypeC3.Count > 0)
                    dicLeaveByType.Add("C3", templstleavedaytypeC3);
                if (templstleavedaytypeC4 != null && templstleavedaytypeC4.Count > 0)
                    dicLeaveByType.Add("C4", templstleavedaytypeC4);
                if (templstleavedaytypeC5 != null && templstleavedaytypeC5.Count > 0)
                    dicLeaveByType.Add("C5", templstleavedaytypeC5);
                if (templstleavedaytypeC6 != null && templstleavedaytypeC6.Count > 0)
                    dicLeaveByType.Add("C6", templstleavedaytypeC6);
                if (templstleavedaytypeC7 != null && templstleavedaytypeC7.Count > 0)
                    dicLeaveByType.Add("C7", templstleavedaytypeC7);
                if (templstleavedaytypeC8 != null && templstleavedaytypeC8.Count > 0)
                    dicLeaveByType.Add("C8", templstleavedaytypeC8);
                #endregion
                #region ngay nghi
                string strE_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                var lstLeaveDay = new List<Att_LeaveDay>().Select(d => new
                {
                    d.LeaveDayTypeID,
                    d.ProfileID,
                    d.LeaveDays,
                    d.DateStart,
                    d.DateEnd
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lstLeaveDay.AddRange(unitOfWork.CreateQueryable<Att_LeaveDay>(Guid.Empty,
                        d => lstProfileID.Contains(d.ProfileID) && d.DateStart <= _dayend
                            && d.DateEnd >= _daystart && d.Status == strE_APPROVED).Select(d => new
                            {
                                d.LeaveDayTypeID,
                                d.ProfileID,
                                d.LeaveDays,
                                d.DateStart,
                                d.DateEnd
                            }).ToList());
                }
                #endregion
                #region ky luat
                var lsthreDiscipline = new List<Hre_Discipline>().Select(d => new
                {
                    d.DisciplinedTypesSuggestID,
                    d.ProfileID
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lsthreDiscipline.AddRange(unitOfWork.CreateQueryable<Hre_Discipline>(Guid.Empty,
                        d => lstProfileID.Contains(d.ProfileID) && d.DateOfEffective >= _daystart
                            && d.DateOfEffective <= _dayend).Select(d => new
                            {
                                d.DisciplinedTypesSuggestID,
                                d.ProfileID
                            }).ToList());
                }
                #endregion
                #region loai ky luat
                List<object> paraDisciplinedTypes = new List<object>();
                paraDisciplinedTypes.AddRange(new object[3]);
                paraDisciplinedTypes[1] = 1;
                paraDisciplinedTypes[2] = int.MaxValue;
                var catSerciceDisciplinedTypes = new Cat_DisciplinedTypesServices();
                var lstDisciplinedTypes = catSerciceDisciplinedTypes.GetData<Cat_DisciplinedTypesEntity>(paraDisciplinedTypes, ConstantSql.hrm_cat_sp_get_DisciplinedTypes, userLogin, ref status);
                //var objDisciplinedTypeVW = lstDisciplinedTypes.Where(s => s.Code == "VW").FirstOrDefault();
                //var objDisciplinedTypeWW = lstDisciplinedTypes.Where(s => s.Code == "WW").FirstOrDefault();
                //var objDisciplinedTypeDS = lstDisciplinedTypes.Where(s => s.Code == "DS").FirstOrDefault();

                Cat_DisciplinedTypesEntity tempobjDisciplinedType11 = null;
                Cat_DisciplinedTypesEntity tempobjDisciplinedType12 = null;
                Cat_DisciplinedTypesEntity tempobjDisciplinedType13 = null;
                #endregion
                #region Loai ky luat
                if (lstDisciplinedTypes != null)
                {
                    #region C11
                    if (DataC11 != null)
                    {
                        DataC11 = DataC11.Replace(" ", "").Trim();
                        tempobjDisciplinedType11 = lstDisciplinedTypes.Where(s => s.Code == DataC11).FirstOrDefault();
                    }
                    #endregion
                    #region C12
                    if (DataC12 != null)
                    {
                        DataC12 = DataC12.Replace(" ", "").Trim();
                        tempobjDisciplinedType12 = lstDisciplinedTypes.Where(s => s.Code == DataC12).FirstOrDefault();
                    }
                    #endregion
                    #region C13
                    if (DataC13 != null)
                    {
                        DataC13 = DataC13.Replace(" ", "").Trim();
                        tempobjDisciplinedType13 = lstDisciplinedTypes.Where(s => s.Code == DataC13).FirstOrDefault();
                    }
                    #endregion
                }

                #endregion
                #region Danh gia
                var kaiServiceKaizenData = new Kai_KaiZenDataServices();
                var listKaiKaizenData = new List<Kai_KaizenData>().Select(d => new
                {
                    d.Accumulate,
                    d.MarkPerform,
                    d.ProfileID
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    listKaiKaizenData.AddRange(unitOfWork.CreateQueryable<Kai_KaizenData>(Guid.Empty, d => d.ProfileID.HasValue
                        && lstProfileID.Contains(d.ProfileID.Value) && d.Month >= _daystart && d.Month <= _dayend).Select(d => new
                        {
                            d.Accumulate,
                            d.MarkPerform,
                            d.ProfileID
                        }).ToList());
                }
                #endregion
                #region Loai thieu quet the la quen quet the voi ma la FC
                var objTamScanResonMissID = unitOfWork.CreateQueryable<Cat_TAMScanReasonMiss>(Guid.Empty, s => s.Code.Trim() == "FC").Select(s => s.ID).FirstOrDefault();
                #endregion
                #region lay du lieu quen quet the
          
                var lstworkDayProfiles = new List<Att_Workday>().Select(d => new
                {
                    d.ProfileID,
                    d.WorkDate
                    
                }).ToList();
                if (objTamScanResonMissID != null)
                {
                    List<string> lstType = new List<string> 
                    { 
                        WorkdayType.E_MISS_IN.ToString(),
                        WorkdayType.E_MISS_IN_OUT.ToString(),
                        WorkdayType.E_MISS_OUT.ToString()
                    };
                    foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                    {
                        lstworkDayProfiles.AddRange(unitOfWork.CreateQueryable<Att_Workday>(Guid.Empty, d => lstProfileID.Contains(d.ProfileID) && _daystart <= d.WorkDate && d.WorkDate <= _dayend && lstType.Contains(d.Type) && d.MissInOutReason == objTamScanResonMissID).Select(d => new
                        {
                            d.ProfileID,
                            d.WorkDate
                        }).ToList());
                    }
                }
              
                #endregion
                bool? _tempC9LATEEARLYDEDUCTIONHOURS = false;
                bool? _tempC9FAILINGTORECORDCARD = false;
                bool? _tempC10LATEEARLYDEDUCTIONHOURS = false;
                bool? _tempC10FAILINGTORECORDCARD = false;

                #region xac dinh dlieu luu cot C9 va C10
                if (DataC9 != null)
                {
                    if (DataC9.Replace(" ", "").Trim() == "LATEEARLYDEDUCTIONHOURS")
                    {
                        _tempC9LATEEARLYDEDUCTIONHOURS = true;
                    }
                    else if (DataC9.Replace(" ", "").Trim() == "FAILINGTORECORDCARD")
                    {
                        _tempC9FAILINGTORECORDCARD = true;
                    }
                }
                if (DataC10 != null)
                {
                    if (DataC10.Replace(" ", "").Trim() == "LATEEARLYDEDUCTIONHOURS")
                    {
                        _tempC10LATEEARLYDEDUCTIONHOURS = true;
                    }
                    else if (DataC10.Replace(" ", "").Trim() == "FAILINGTORECORDCARD")
                    {
                        _tempC10FAILINGTORECORDCARD = true;
                    }
                }
                #endregion
                #region xac dinh dlieu luu cot C14->C16
                bool? _tempC14MARKPERFORM = false;
                bool? _tempC14ACCUMULATE = false;
                bool? _tempC15MARKPERFORM = false;
                bool? _tempC15ACCUMULATE = false;
                bool? _tempC16MARKPERFORM = false;
                bool? _tempC16ACCUMULATE = false;

                if (DataC14 != null)
                {
                    if (DataC14.Replace(" ", "").Trim() == "MARKPERFORM")
                    {
                        _tempC14MARKPERFORM = true;
                    }
                    else if (DataC14.Replace(" ", "").Trim() == "ACCUMULATE")
                    {
                        _tempC14ACCUMULATE = true;
                    }
                }
                if (DataC15 != null)
                {
                    if (DataC15.Replace(" ", "").Trim() == "MARKPERFORM")
                    {
                        _tempC15MARKPERFORM = true;
                    }
                    else if (DataC15.Replace(" ", "").Trim() == "ACCUMULATE")
                    {
                        _tempC15ACCUMULATE = true;
                    }
                }
                if (DataC16 != null)
                {
                    if (DataC16.Replace(" ", "").Trim() == "MARKPERFORM")
                    {
                        _tempC16MARKPERFORM = true;
                    }
                    else if (DataC16.Replace(" ", "").Trim() == "ACCUMULATE")
                    {
                        _tempC16ACCUMULATE = true;
                    }
                }

                #endregion

                foreach (var profile in lstProfile)
                {
                    Eva_EvalutionDataEntity entity = new Eva_EvalutionDataEntity();
                    entity.CodeEmp = profile.CodeEmp;
                    entity.ProfileName = profile.ProfileName;
                    entity.ProfileID = profile.ID;
                    entity.TimesGetDataID = _TimesGetDataID;
                    entity.Year = new DateTime(year, 01, 01);
                    int C1 = 0;
                    int C2 = 0;
                    int C3 = 0;
                    int C4 = 0;
                    int C5 = 0;
                    int C6 = 0;
                    int C7 = 0;
                    int C8 = 0;
                    int C9 = 0;
                    int C10 = 0;
                    var lstLeaveDayprofile = lstLeaveDay.Where(s => s.ProfileID == profile.ID).ToList();
                    if (lstLeaveDayprofile.Count > 0)
                    {
                        foreach (var attLeaveDayprofile in lstLeaveDayprofile)
                        {
                            #region C1->C8
                            foreach (var item in dicLeaveByType)
                            {
                                List<Cat_LeaveDayTypeEntity> lstLeavetype = (List<Cat_LeaveDayTypeEntity>)item.Value;
                                int temp = 0;
                                if (attLeaveDayprofile.LeaveDayTypeID != null && lstLeavetype.Any(m => m.ID == attLeaveDayprofile.LeaveDayTypeID) && attLeaveDayprofile.LeaveDays > 0)
                                {
                                    string _strLeaveDays = attLeaveDayprofile.LeaveDays.ToString();
                                    string[] _partLeaveDays = _strLeaveDays.Split('.');
                                    int _intLeaveDays = int.Parse(_partLeaveDays[0]);
                                    if (_partLeaveDays.Count() > 1)
                                    {
                                        int _modLeaveDays = int.Parse(_partLeaveDays[1]);
                                        if (_modLeaveDays > 0)
                                        {
                                            temp += _intLeaveDays + 1;
                                        }
                                    }
                                    else
                                    {
                                        temp += _intLeaveDays;
                                    }
                                }
                                if (item.Key == "C1")
                                {
                                    C1 += temp;
                                }
                                if (item.Key == "C2")
                                {
                                    C2 += temp;
                                }
                                if (item.Key == "C3")
                                {
                                    C3 += temp;
                                }
                                if (item.Key == "C4")
                                {
                                    C4 += temp;
                                }
                                if (item.Key == "C5")
                                {
                                    C5 += temp;
                                }
                                if (item.Key == "C6")
                                {
                                    C6 += temp;
                                }
                                if (item.Key == "C7")
                                {
                                    C7 += temp;
                                }
                                if (item.Key == "C8")
                                {
                                    C8 += temp;
                                }
                            }
                        }
                            #endregion
                    }

                    #region tong tre som va quen quet the C9->C10
                    #region C9
                    if (_tempC9LATEEARLYDEDUCTIONHOURS == true)
                    {
                        var lstattattendancetableprofile = lstattattendancetable.Where(s => s.ProfileID == profile.ID).ToList();
                        if (lstattattendancetableprofile.Count > 0)
                        {
                            C9 = lstattattendancetableprofile.Where(s => s.LateEarlyDeductionHours > 0).Count();
                        }
                    }
                    else if (_tempC9FAILINGTORECORDCARD == true)
                    {
                        var lstworkDayprofile = lstworkDayProfiles.Where(s => s.ProfileID == profile.ID).ToList();
                        if (lstworkDayprofile.Count > 0)
                        {
                            C9 = lstworkDayprofile.Count();
                        }
                        if (lstLeaveDayprofile.Count > 0)
                        {
                            foreach (var objWorkDay in lstworkDayprofile)
                            {
                                var objLeaveDayByWorkDay = lstLeaveDayprofile.Where(s => s.DateStart <= objWorkDay.WorkDate && s.DateEnd >= objWorkDay.WorkDate).ToList();
                                if (objLeaveDayByWorkDay.Count > 0)
                                {
                                    C9 -= 1;
                                }
                            }
                        }
                    }
                    #endregion
                    #region C10
                    if (_tempC10LATEEARLYDEDUCTIONHOURS == true)
                    {
                        var lstattattendancetableprofile = lstattattendancetable.Where(s => s.ProfileID == profile.ID).ToList();
                        if (lstattattendancetableprofile.Count > 0)
                        {
                            C10 = lstattattendancetableprofile.Where(s => s.LateEarlyDeductionHours > 0).Count();
                        }
                    }
                    else if (_tempC10FAILINGTORECORDCARD == true)
                    {
                        var lstworkDayprofile = lstworkDayProfiles.Where(s => s.ProfileID == profile.ID).ToList();
                        if (lstworkDayprofile.Count > 0)
                        {
                            C10 = lstworkDayprofile.Count();
                        }
                        if (lstLeaveDayprofile.Count>0)
                        {
                            foreach (var objWorkDay in lstworkDayprofile)
                            {
                                var objLeaveDayByWorkDay = lstLeaveDayprofile.Where(s => s.DateStart <= objWorkDay.WorkDate && s.DateEnd >= objWorkDay.WorkDate).ToList();
                                if (objLeaveDayByWorkDay.Count > 0)
                                {
                                    C10 -= 1;
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    if (C1 > 0)
                        entity.C1 = C1;
                    if (C2 > 0)
                        entity.C2 = C2;
                    if (C3 > 0)
                        entity.C3 = C3;
                    if (C4 > 0)
                        entity.C4 = C4;
                    if (C5 > 0)
                        entity.C5 = C5;
                    if (C6 > 0)
                        entity.C6 = C6;
                    if (C7 > 0)
                        entity.C7 = C7;
                    if (C8 > 0)
                        entity.C8 = C8;
                    if (C9 > 0)
                        entity.C9 = C9;
                    if (C10 > 0)
                        entity.C10 = C10;
                    #region C11->C13
                    if (lsthreDiscipline.Count > 0)
                    {
                        var lsthreDisciplineprofile = lsthreDiscipline.Where(s => s.ProfileID == profile.ID).ToList();
                        if (lsthreDisciplineprofile.Count > 0)
                        {
                            if (tempobjDisciplinedType11 != null)
                            {
                                var lsthreDiscipline11 = lsthreDisciplineprofile.Where(s => s.DisciplinedTypesSuggestID == tempobjDisciplinedType11.ID).ToList();
                                if (lsthreDiscipline11.Count > 0)
                                    entity.C11 = lsthreDiscipline11.Count;
                            }
                            if (tempobjDisciplinedType12 != null)
                            {
                                var lsthreDiscipline12 = lsthreDisciplineprofile.Where(s => s.DisciplinedTypesSuggestID == tempobjDisciplinedType12.ID).ToList();
                                if (lsthreDiscipline12.Count > 0)
                                    entity.C12 = lsthreDiscipline12.Count;
                            }
                            if (tempobjDisciplinedType13 != null)
                            {
                                var lsthreDiscipline13 = lsthreDisciplineprofile.Where(s => s.DisciplinedTypesSuggestID == tempobjDisciplinedType13.ID).ToList();
                                if (lsthreDiscipline13.Count > 0)
                                    entity.C13 = lsthreDiscipline13.Count;
                            }
                        }
                    }
                    #endregion
                    #region C14->C16
                    if (listKaiKaizenData.Count > 0)
                    {
                        var listKaiKaizenDataprofile = listKaiKaizenData.Where(s => s.ProfileID == profile.ID).ToList();
                        int _tempAccumulate = 0;
                        int _MarkPerform = 0;
                        if (listKaiKaizenDataprofile.Count > 0)
                        {
                            #region Danh Gia
                            _MarkPerform = listKaiKaizenDataprofile.Where(s => s.MarkPerform > 0).Count();
                            _tempAccumulate = int.Parse(listKaiKaizenDataprofile.Select(s => s.Accumulate).Sum().ToString());
                            #region C14
                            if (_tempC14MARKPERFORM == true && _MarkPerform > 0)
                            {
                                entity.C14 = _MarkPerform;
                            }
                            else if (_tempC14ACCUMULATE == true && _tempAccumulate > 0)
                            {
                                entity.C14 = _tempAccumulate;
                            }
                            #endregion
                            #region C15
                            if (_tempC15MARKPERFORM == true && _MarkPerform > 0)
                            {
                                entity.C15 = _MarkPerform;
                            }
                            else if (_tempC15ACCUMULATE == true && _tempAccumulate > 0)
                            {
                                entity.C15 = _tempAccumulate;
                            }
                            #endregion
                            #region C16
                            if (_tempC16MARKPERFORM == true && _MarkPerform > 0)
                            {
                                entity.C16 = _MarkPerform;
                            }
                            else if (_tempC16ACCUMULATE == true && _tempAccumulate > 0)
                            {
                                entity.C16 = _tempAccumulate;
                            }
                            #endregion
                            #endregion
                        }

                    }
                    #endregion
                    if (entity != null)
                    {
                        lstEvalutionDataEntity.Add(entity);
                    }
                }
                return lstEvalutionDataEntity;
                //    #endregion
            }
        }
        #endregion

        #region Hàm Đệ Quy Lấy Phòng Ban
        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent)
        {
            var child = source.Where(m => m.ParentID == idParent).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID);
                }
            }
        }
        #endregion

        DataTable CreateReportEvalutionSchema(DateTime? _dateFrom, DateTime? _dateTo,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Eva_ReportEvalutionDataEntity");
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ProfileName);

                if (_dateFrom != null && _dateTo != null)
                {
                    if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.HistoryDateFrom))
                    {
                        tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.HistoryDateFrom, typeof(DateTime));
                    }
                }

                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.Year, typeof(DateTime));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.Time, typeof(int));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.E_DIVISION);

                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.E_DEPARTMENT);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.UnitAbb);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.DivisionAbb);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.E_SECTION);

                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.DepartmentAbb);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.GroupCode);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.LocationByGroupCode);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ActualWokingPlace);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ReportEvalutionDataNode, typeof(DateTime));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.Age, typeof(int));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.Entering, typeof(DateTime));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ServiceYear, typeof(string));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ServiceYearKi, typeof(string));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.PayRankSalary);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.BasicSalary, typeof(double));
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.Rank);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.RankZone);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.JobTitle);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.QualificationAbilitytitle);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.BasicSalary1, typeof(double?));
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.AreaAllowance);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.SpecialAllowance);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.DrivingAllowance);
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.AdjustmentAllowanceForVP, typeof(double?));
                //tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ChildCareAllowance, typeof(double?));


                string status = string.Empty;
                List<object> paraUnusualAllowanceCfg = new List<object>();
                paraUnusualAllowanceCfg.AddRange(new object[6]);
                paraUnusualAllowanceCfg[4] = 1;
                paraUnusualAllowanceCfg[5] = int.MaxValue;
                var catServiceUnusualAllowanceCfg = new Cat_UnusualAllowanceCfgServices();
                var lstUnusualAllowanceCfg = catServiceUnusualAllowanceCfg.GetData<Cat_UnusualAllowanceCfgEntity>(paraUnusualAllowanceCfg, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, userLogin, ref status);
                if (lstUnusualAllowanceCfg != null)
                {
                    foreach (var item in lstUnusualAllowanceCfg)
                    {
                        if (!string.IsNullOrEmpty(item.Code) && !tb.Columns.Contains(item.Code))
                        {
                            tb.Columns.Add(item.Code, typeof(double));
                        }
                    }
                }
                if (_dateFrom != null && _dateTo != null)
                {
                    int _yearFrom = _dateFrom.Value.Year;
                    int _yearTo = _dateTo.Value.Year;
                    while (_yearFrom <= _yearTo)
                    {
                        string YearField = "Year" + _yearFrom;
                        if (!tb.Columns.Contains(YearField))
                            tb.Columns.Add(YearField);
                        _yearFrom += 1;
                    }
                    if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.HistoryDateFrom))
                    {
                        tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.HistoryDateFrom, typeof(DateTime));
                    }
                }
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TotalSalaryAllowance))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TotalSalaryAllowance, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C1))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C1, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C2))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C2, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C3))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C3, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C4))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C4, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C5))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C5, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C6))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C6, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C7))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C7, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C8))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C8, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C9))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C9, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C10))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C10, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C11))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C11, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C12))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C12, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C13))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C13, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C14))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C14, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C15))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C15, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C16))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C16, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C17))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C17);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C18))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C18);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C19))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C19, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C20))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C20, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C21))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C21, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C22))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C22);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C23))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C23, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C24))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C24, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C25))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C25, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C26))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C26, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C27))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C27, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C28))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C28, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C29))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C29, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C30))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C30, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C31))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C31);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C32))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C32);
                //if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2))
                //    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2, typeof(int));
                //if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TotalC3C4C5C6C7))
                //    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TotalC3C4C5C6C7, typeof(int));
                //if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2C3C4C5C6C7))
                //    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2C3C4C5C6C7, typeof(int));
                //if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TotalC8C9))
                //    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TotalC8C9, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionData))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionData);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.DateStart))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.DateStart, typeof(DateTime));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.DateEnd))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.DateEnd, typeof(DateTime));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionDataCode))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionDataCode);
                return tb;
            }
        }
        public DataTable GetReportEvalutionData(int year, Guid? _TimesGetDataID, string orgStructureID, bool _IsCreateTemplate, DateTime? _dateStart,
            DateTime? _dateEnd, DateTime? _dateFrom, DateTime? _dateTo, Guid? _JobTitleID, Guid? _PositionID, Guid? _WorkPlaceID, Guid? _RankID,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportEvalutionSchema(_dateFrom, _dateTo, userLogin);
                if (_IsCreateTemplate)
                    return table;
                string status = string.Empty;
                List<Eva_EvalutionDataEntity> lstEvalutionDataEntity = new List<Eva_EvalutionDataEntity>();
                DateTime _datetime = new DateTime(year, 01, 01);
                //Ngay goc
                DateTime datetimeNode = _dateEnd.Value;
                List<object> paraEvalutionData = new List<object>();
                paraEvalutionData.AddRange(new object[17]);
                paraEvalutionData[0] = orgStructureID;
                paraEvalutionData[1] = year;
                paraEvalutionData[2] = _datetime;
                //paraEvalutionData[3] = time;
                paraEvalutionData[10] = _TimesGetDataID;

                paraEvalutionData[11] = _JobTitleID;
                paraEvalutionData[12] = _PositionID;
                paraEvalutionData[13] = _WorkPlaceID;
                paraEvalutionData[14] = _RankID;

                paraEvalutionData[15] = 1;
                paraEvalutionData[16] = int.MaxValue - 1;
                var evaServiceEvalutionData = new Eva_EvalutionDataServices();
                //chi danh gia nhung nv ngay vao lam <= ngay goc
                var lstEvalutionData = evaServiceEvalutionData.GetData<Eva_EvalutionDataEntity>(paraEvalutionData, ConstantSql.hrm_eva_getdata_EvalutionData, userLogin, ref status);
                Guid[] lstSalaryClassIDs = null;
                Guid[] lstProfielIds = null;
                if (lstEvalutionData != null)
                {
                    lstSalaryClassIDs = lstEvalutionData.Where(s => s.SalaryClassID != null).Select(s => s.SalaryClassID.Value).Distinct().ToArray();
                    lstProfielIds = lstEvalutionData.Where(s => s.ProfileID != null).Select(s => s.ProfileID.Value).Distinct().ToArray();
                }
                #region luong co ban
                List<object> paraBasicSalary = new List<object>();
                paraBasicSalary.AddRange(new object[10]);
                DateTime _dateMin = DateTime.MinValue;

                paraBasicSalary[2] = orgStructureID;
                paraBasicSalary[3] = _dateMin;
                paraBasicSalary[4] = datetimeNode;
                paraBasicSalary[5] = _dateMin;
                paraBasicSalary[6] = datetimeNode;
                paraBasicSalary[8] = 1;
                paraBasicSalary[9] = int.MaxValue - 1;
                var salServiceBasicSalary = new Sal_BasicSalaryServices();
                var lstBasicSalary = salServiceBasicSalary.GetData<Sal_BasicSalaryEntity>(paraBasicSalary, ConstantSql.hrm_sal_sp_get_BasicPayroll, userLogin, ref status);
                if (lstBasicSalary != null && lstProfielIds != null)
                    lstBasicSalary = lstBasicSalary.Where(s => lstProfielIds.Contains(s.ProfileID)).ToList();
                List<object> paraSalaryRank = new List<object>();
                paraSalaryRank.AddRange(new object[4]);
                paraSalaryRank[2] = 1;
                paraSalaryRank[3] = int.MaxValue;
                var catServiceSalaryRank = new Cat_SalaryRankServices();
                var lstSalaryRank = catServiceSalaryRank.GetData<Cat_SalaryRankEntity>(paraSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, userLogin, ref status);
                if (lstSalaryRank != null && lstSalaryClassIDs != null)
                    lstSalaryRank = lstSalaryRank.Where(s => s.DateOfEffect <= datetimeNode && lstSalaryClassIDs.Contains(s.SalaryClassID.Value)).ToList();
                #endregion
                #region phu cap
                List<object> paraUnusualAllowance = new List<object>();
                paraUnusualAllowance.AddRange(new object[9]);

                paraBasicSalary[3] = _dateMin;
                paraBasicSalary[4] = datetimeNode;
                paraUnusualAllowance[7] = 1;
                paraUnusualAllowance[8] = int.MaxValue - 1;
                var salServiceUnusualAllowance = new Sal_UnusualAllowanceServices();
                var lstUnusualAllowance = salServiceUnusualAllowance.GetData<Sal_UnusualAllowanceEntity>(paraUnusualAllowance, ConstantSql.hrm_sal_sp_get_UnusualED, userLogin, ref status);
                List<Guid> lstUnusualAllowanceIDs = null;
                if (lstUnusualAllowance != null)
                {
                    //lstUnusualAllowance = lstUnusualAllowance.Where(s => s.MonthEnd <= datetimeNode || (s.MonthStart <= datetimeNode && s.MonthEnd == null)).ToList();
                    lstUnusualAllowanceIDs = lstUnusualAllowance.Where(s => s.UnusualEDTypeID != null).Select(s => s.UnusualEDTypeID.Value).Distinct().ToList();
                }

                #endregion
                #region loai phu cap
                List<object> paraUnusualAllowanceCfg = new List<object>();
                paraUnusualAllowanceCfg.AddRange(new object[6]);
                paraUnusualAllowanceCfg[4] = 1;
                paraUnusualAllowanceCfg[5] = int.MaxValue - 1;
                var catServiceUnusualAllowanceCfg = new Cat_UnusualAllowanceCfgServices();
                var lstUnusualAllowanceCfg = catServiceUnusualAllowanceCfg.GetData<Cat_UnusualAllowanceCfgEntity>(paraUnusualAllowanceCfg, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, userLogin, ref status);
                if (lstUnusualAllowanceCfg != null && lstUnusualAllowanceIDs != null)
                    lstUnusualAllowanceCfg = lstUnusualAllowanceCfg.Where(s => lstUnusualAllowanceIDs.Contains(s.ID)).ToList();
                #endregion
                #region lay lich su danh gia
                List<Eva_PerformanceEntity> lstPerformance = null;
                int _yearFrom = DateTime.MinValue.Year;
                int _yearTo = DateTime.MinValue.Year;
                if (_dateFrom != null && _dateTo != null)
                {
                    _yearFrom = _dateFrom.Value.Year;
                    _yearTo = _dateTo.Value.Year;
                    var evaServicePerformance = new Eva_PerformanceServices();
                    List<object> paraPerformance = new List<object>();
                    paraPerformance.AddRange(new object[20]);
                    paraPerformance[2] = orgStructureID;
                    paraPerformance[18] = 1;
                    paraPerformance[19] = int.MaxValue - 1;
                    lstPerformance = evaServicePerformance.GetData<Eva_PerformanceEntity>(paraPerformance, ConstantSql.hrm_eva_sp_get_PerformanceGeneral, userLogin, ref status);
                    if (lstPerformance != null)
                        lstPerformance = lstPerformance.Where(s => s.YearEvaluation != null).ToList();
                    if (lstPerformance.Count != 0)
                        lstPerformance = lstPerformance.Where(s => s.YearEvaluation.Value.Year >= _yearFrom && s.YearEvaluation.Value.Year <= _yearTo).ToList();
                }
                #endregion
                DateTime dateFirstKi = _dateStart.Value;
                foreach (var item in lstEvalutionData)
                {
                    DataRow row = table.NewRow();
                    #region thong tin nhan vien
                    row[Eva_ReportEvalutionDataEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.ProfileName] = item.ProfileName;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.GroupCode] = item.CostCentreCode;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.LocationByGroupCode] = item.WorkPlaceCode;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.ReportEvalutionDataNode] = datetimeNode;
                    #endregion
                    #region tham nien
                    if (item.DateOfBirth != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.Age] = DateTime.Today.Year - item.DateOfBirth.Value.Year;
                    if (item.DateHire != null)
                    {
                        row[Eva_ReportEvalutionDataEntity.FieldNames.Entering] = item.DateHire;
                        double dayServiceYear;
                        dayServiceYear = datetimeNode.Subtract(item.DateHire.Value).TotalDays;
                        double _TotalYear = dayServiceYear / 360;
                        double _tempYear = 0;
                        if (_TotalYear > 0)
                        {
                            string[] parts = _TotalYear.ToString().Split('.');
                            double _intMonth = double.Parse(parts[0]);
                            if (parts.Length > 1)
                            {
                                double _modMonth = double.Parse("0." + parts[1]) * 12;
                                if (_modMonth >= 10)
                                {
                                    _tempYear = _intMonth + (_modMonth / 100);
                                }
                                else
                                {
                                    _tempYear = _intMonth + (_modMonth / 10);
                                }
                            }
                            row[Eva_ReportEvalutionDataEntity.FieldNames.ServiceYear] = String.Format("{0:0.0}", _tempYear); ;
                        }
                        if (item.DateHire.Value > dateFirstKi)
                        {
                            if (_TotalYear > 0)
                                row[Eva_ReportEvalutionDataEntity.FieldNames.ServiceYearKi] = String.Format("{0:0.0}", _tempYear);
                        }
                        else
                        {
                            row[Eva_ReportEvalutionDataEntity.FieldNames.ServiceYearKi] = 1.0;
                        }
                    }
                    #endregion

                    var objBasicSalary = lstBasicSalary.Where(s => s.ProfileID == item.ProfileID).FirstOrDefault();
                    if (objBasicSalary != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.BasicSalary] = double.Parse(objBasicSalary.GrossAmount);
                    row[Eva_ReportEvalutionDataEntity.FieldNames.Rank] = item.SalaryClassName;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.QualificationAbilitytitle] = item.AbilityTitleVNI;
                    if (item.SalaryClassID != null)
                    {
                        var objSalaryRank = lstSalaryRank.Where(s => s.SalaryClassID == item.SalaryClassID).FirstOrDefault();
                        if (objSalaryRank != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.RankZone] = objSalaryRank.SalaryRankName;
                    }
                    row[Eva_ReportEvalutionDataEntity.FieldNames.JobTitle] = item.JobTitleName;
                    Guid[] lstUnusualAllowanceProfileCfgIds = null;
                    if (lstUnusualAllowance != null)
                    {
                        lstUnusualAllowance = lstUnusualAllowance.Where(s => s.ProfileID == item.ProfileID && s.UnusualEDTypeID != null).ToList();
                        lstUnusualAllowanceProfileCfgIds = lstUnusualAllowance.Select(s => s.UnusualEDTypeID.Value).ToArray();
                    }
                    if (lstUnusualAllowanceCfg != null && lstUnusualAllowanceProfileCfgIds != null)
                        lstUnusualAllowanceCfg = lstUnusualAllowanceCfg.Where(s => lstUnusualAllowanceProfileCfgIds.Contains(s.ID)).ToList();
                    double? totalUnusualAllowance = 0;
                    if (lstUnusualAllowanceCfg != null)
                    {
                        foreach (var itemUnusualAllowanceCfg in lstUnusualAllowanceCfg)
                        {
                            var objUnusualAllowance = lstUnusualAllowance.Where(s => s.UnusualEDTypeID == itemUnusualAllowanceCfg.ID).OrderByDescending(s => s.MonthEnd).FirstOrDefault();
                            if (objUnusualAllowance != null)
                            {
                                row[itemUnusualAllowanceCfg.Code] = objUnusualAllowance.Amount;
                                totalUnusualAllowance += objUnusualAllowance.Amount;
                            }
                        }
                    }
                    #region lich su danh gia
                    if (_dateFrom != null && _dateTo != null && lstPerformance != null)
                    {
                        _yearFrom = _dateFrom.Value.Year;
                        _yearTo = _dateTo.Value.Year;
                        var lstPerformanceProfile = lstPerformance.Where(s => s.ProfileID == item.ProfileID).OrderByDescending(s => s.YearEvaluation).ToList();
                        if (lstPerformanceProfile.Count != 0)
                        {
                            while (_yearTo >= _yearFrom)
                            {
                                var objPerformanceFirst = lstPerformanceProfile.Where(s => s.YearEvaluation.Value.Year == _yearTo).OrderByDescending(s => s.YearEvaluation).FirstOrDefault();
                                _yearTo -= 1;
                                var objPerformance = lstPerformanceProfile.Where(s => s.YearEvaluation.Value.Year == _yearTo).OrderByDescending(s => s.YearEvaluation).FirstOrDefault();
                                int _yeartemp = _yearTo + 1;
                                if (objPerformanceFirst != null && objPerformanceFirst.Level1Name != null)
                                {
                                    if (objPerformanceFirst.RankID != null && objPerformance != null && objPerformance.Level1Name != null && objPerformance.RankID != null)
                                    {
                                        if (objPerformanceFirst.RankID != objPerformance.RankID)
                                        {
                                            row["Year" + _yeartemp] = "(" + objPerformanceFirst.Level1Name + ")";
                                            row["Year" + _yearTo] = "(" + objPerformance.Level1Name + ")";
                                            _yearTo -= 1;
                                            while (_yearTo >= _yearFrom)
                                            {
                                                var objPerformanceYear = lstPerformanceProfile.Where(s => s.YearEvaluation.Value.Year == _yearTo).OrderByDescending(s => s.YearEvaluation).FirstOrDefault();
                                                if (objPerformanceYear != null && objPerformanceYear.Level1Name != null)
                                                    row["Year" + _yearTo] = "(" + objPerformanceYear.Level1Name + ")";
                                                _yearTo -= 1;
                                            }
                                        }
                                        else
                                        {
                                            row["Year" + _yeartemp] = objPerformanceFirst.Level1Name;
                                        }
                                    }
                                    else
                                    {
                                        row["Year" + _yeartemp] = objPerformanceFirst.Level1Name;
                                    }
                                }
                            }
                        }
                        row[Eva_ReportEvalutionDataEntity.FieldNames.HistoryDateFrom] = _dateFrom.Value.ToShortDateString();
                    }
                    #endregion
                    if (totalUnusualAllowance > 0)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.TotalSalaryAllowance] = totalUnusualAllowance;
                    if (item.Year != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.Year] = item.Year;
                    //if (item.Time !=null)
                    //    row[Eva_ReportEvalutionDataEntity.FieldNames.Time] = item.Time;
                    #region lan danh gia
                    if (item.TimeEvalutionData != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionData] = item.TimeEvalutionData;
                    if (item.TimeEvalutionDataCode != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.TimeEvalutionDataCode] = item.TimeEvalutionDataCode;

                    #endregion


                    #region C1->C32
                    if (item.C1 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C1] = item.C1;
                    if (item.C2 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C2] = item.C2;
                    if (item.C3 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C3] = item.C3;
                    if (item.C4 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C4] = item.C4;
                    if (item.C5 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C5] = item.C5;
                    if (item.C6 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C6] = item.C6;
                    if (item.C7 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C7] = item.C7;
                    if (item.C8 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C8] = item.C8;
                    if (item.C9 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C9] = item.C9;
                    if (item.C10 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C10] = item.C10;
                    if (item.C11 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C11] = item.C11;
                    if (item.C12 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C12] = item.C12;
                    if (item.C13 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C13] = item.C13;
                    if (item.C14 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C14] = item.C14;
                    if (item.C15 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C15] = item.C15;
                    if (item.C16 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C16] = item.C16;
                    if (item.C17 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C17] = item.C17;
                    if (item.C18 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C18] = item.C18;
                    if (item.C19 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C19] = item.C19;
                    if (item.C20 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C20] = item.C20;
                    if (item.C21 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C21] = item.C21;
                    if (item.C22 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C22] = item.C22;
                    if (item.C23 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C23] = item.C23;
                    if (item.C24 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C24] = item.C24;
                    if (item.C25 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C25] = item.C25;
                    if (item.C26 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C26] = item.C26;
                    if (item.C27 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C27] = item.C27;
                    if (item.C28 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C28] = item.C28;
                    if (item.C29 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C29] = item.C29;
                    if (item.C30 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C30] = item.C30;
                    if (item.C31 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C31] = item.C31;
                    if (item.C32 != null)
                        row[Eva_ReportEvalutionDataEntity.FieldNames.C32] = item.C32;
                    #endregion
                    #region Bo sung
                    //int _tempC1 = 0;
                    //int _TempC2 = 0;
                    //int _tempC3 = 0;
                    //int _tempC4 = 0;
                    //int _tempC5 = 0;
                    //int _tempC6 = 0;
                    //int _tempC7 = 0;
                    //int _tempC8 = 0;
                    //int _tempC9 = 0;
                    //#region TotalC1C2
                    //if (item.C1 != null)
                    //    _tempC1 = item.C1.Value;
                    //if (item.C2 != null)
                    //    _TempC2 = item.C2.Value;
                    //if (item.C1 != null || item.C2 != null)
                    //    row[Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2] = _tempC1 + _TempC2;
                    //#endregion
                    //#region TotalC3C4C5C6C7
                    //if (item.C3 != null)
                    //    _tempC3 = item.C3.Value;
                    //if (item.C4 != null)
                    //    _tempC4 = item.C4.Value;
                    //if (item.C5 != null)
                    //    _tempC5 = item.C5.Value;
                    //if (item.C6 != null)
                    //    _tempC6 = item.C6.Value;
                    //if (item.C7 != null)
                    //    _tempC7 = item.C7.Value;
                    //if (item.C3 != null || item.C4 != null || item.C5 != null || item.C6 != null || item.C7 != null)
                    //    row[Eva_ReportEvalutionDataEntity.FieldNames.TotalC3C4C5C6C7] = _tempC3 + _tempC4 + _tempC5 + _tempC6 + _tempC7;
                    //#endregion
                    //#region TotalC1C2C3C4C5C6C7
                    //if (item.C1 != null || item.C2 != null || item.C3 != null || item.C4 != null || item.C5 != null || item.C6 != null || item.C7 != null)
                    //    row[Eva_ReportEvalutionDataEntity.FieldNames.TotalC1C2C3C4C5C6C7] = _tempC1 + _TempC2 + _tempC3 + _tempC4 + _tempC5 + _tempC6 + _tempC7;
                    //#endregion
                    //#region  TotalC8C9
                    //if (item.C8 != null)
                    //    _tempC8 = item.C8.Value;
                    //if (item.C9 != null)
                    //    _tempC9 = item.C9.Value;
                    //if (item.C8 != null || item.C9 != null)
                    //    row[Eva_ReportEvalutionDataEntity.FieldNames.TotalC8C9] = _tempC8 + _tempC9;
                    //#endregion                    
                    #endregion
                    #region thoi gian danh gia
                    row[Eva_ReportEvalutionDataEntity.FieldNames.DateStart] = _dateStart;
                    row[Eva_ReportEvalutionDataEntity.FieldNames.DateEnd] = _dateEnd;
                    #endregion
                    table.Rows.Add(row);
                }
                return table.ConfigTable();
                //    #endregion
            }
        }

        public List<Eva_PerformanceEntity> GetPerformanceGeneralList(List<Eva_PerformanceEntity> lstPerformce, string orgStructureID,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                //DataTable table = CreateReportEvalutionSchema(_dateFrom, _dateTo);
                //if (_IsCreateTemplate)
                //    return table;
                string status = string.Empty;
                //DateTime _datetime = new DateTime(year, 01, 01);
                //Ngay goc
                //DateTime datetimeNode = _dateEnd.Value;
                List<object> paraEvalutionData = new List<object>();
                paraEvalutionData.AddRange(new object[17]);
                paraEvalutionData[0] = orgStructureID;
                //paraEvalutionData[1] = year;
                //paraEvalutionData[2] = _datetime;
                ////paraEvalutionData[3] = time;
                //paraEvalutionData[10] = _TimesGetDataID;

                //paraEvalutionData[11] = _JobTitleID;
                //paraEvalutionData[12] = _PositionID;
                //paraEvalutionData[13] = _WorkPlaceID;
                //paraEvalutionData[14] = _RankID;

                paraEvalutionData[15] = 1;
                paraEvalutionData[16] = int.MaxValue - 1;
                var evaServiceEvalutionData = new Eva_EvalutionDataServices();
                //chi danh gia nhung nv ngay vao lam <= ngay goc
                var lstEvalutionData = evaServiceEvalutionData.GetData<Eva_EvalutionDataEntity>(paraEvalutionData, ConstantSql.hrm_eva_getdata_EvalutionData, userLogin, ref status);
                Guid[] lstSalaryClassIDs = null;
                Guid[] lstProfielIds = null;
                if (lstEvalutionData != null)
                {
                    lstSalaryClassIDs = lstEvalutionData.Where(s => s.SalaryClassID != null).Select(s => s.SalaryClassID.Value).Distinct().ToArray();
                    lstEvalutionData = lstEvalutionData.Where(s => s.Year != null).ToList();
                }
                if(lstPerformce!=null)
                {
                    lstProfielIds = lstPerformce.Where(s => s.ProfileID != null).Select(s => s.ProfileID.Value).Distinct().ToArray();
                }
                #region luong co ban
                var lstBasicSalary = new List<Sal_BasicSalary>().Select(s => new { s.ProfileID, s.GrossAmount }).ToList();
                foreach (var lstProfile in lstProfielIds.Chunk(1000))
                {
                    lstBasicSalary.AddRange(unitOfWork.CreateQueryable<Sal_BasicSalary>(Guid.Empty, s => lstProfile.Contains(s.ProfileID)).Select(s => new { s.ProfileID, s.GrossAmount }).ToList());
                }
                List<object> paraSalaryRank = new List<object>();
                paraSalaryRank.AddRange(new object[4]);
                paraSalaryRank[2] = 1;
                paraSalaryRank[3] = int.MaxValue;
                var catServiceSalaryRank = new Cat_SalaryRankServices();
                var lstSalaryRank = catServiceSalaryRank.GetData<Cat_SalaryRankEntity>(paraSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, userLogin, ref status);
                if (lstSalaryRank != null && lstSalaryClassIDs != null)
                    lstSalaryRank = lstSalaryRank.Where(s => lstSalaryClassIDs.Contains(s.SalaryClassID.Value)).ToList();
                // s.DateOfEffect <= datetimeNode && 
                #endregion
             
                //DateTime dateFirstKi = _dateStart.Value;
                List<Eva_PerformanceEntity> lstPerformanceModel = new List<Eva_PerformanceEntity>();
                //lay all cau hinh 
                var lstAllSetting = unitOfWork.CreateQueryable<Sys_AllSetting>(Guid.Empty).Select(s => new { s.Name, s.Value1 }).ToList();
                foreach (var objPerformanceModel in lstPerformce)
                {
                    DateTime? _DateConfig=null;
                    DateTime? _dateFrom=null;
                    DateTime? _dateTo=null;
                    DateTime? datetimeNode=null;
                    DateTime? dateFirstKi=null;

                    //lay ket qua gan nhat danh gia cua nhan vien
                    if (objPerformanceModel.DateEffect != null)
                    {
                        if (lstEvalutionData != null)
                        {
                            var objEvalutionData = lstEvalutionData.Where(s => s.ProfileID == objPerformanceModel.ProfileID && objPerformanceModel.DateEffect.Value.Year == s.Year.Value.Year).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            if (objEvalutionData != null && objEvalutionData.Year != null)
                            {
                                int? year;
                                year = objEvalutionData.Year.Value.Year;
                                //Get cau hinh ket thuc nam tai chinh
                                if (lstAllSetting.Count > 0)
                                {
                                    var tempDate = lstAllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_DAYENDYEARFINANCE.ToString()).Select(s => s.Value1).FirstOrDefault();
                                    DateTime tempDateConfig = DateTime.Now;
                                    DateTime.TryParse(tempDate, out tempDateConfig);
                                    _DateConfig = tempDateConfig;
                                }
                                if (_DateConfig != null)
                                {
                                    DateTime? _tempDate = _DateConfig.Value.AddDays(1);
                                    _dateFrom = new DateTime(year.Value, _tempDate.Value.Month, _tempDate.Value.Day);
                                    if (_dateFrom != null)
                                        dateFirstKi = _dateFrom;
                                    _DateConfig = new DateTime(DateTime.Now.Year, _DateConfig.Value.Month, _DateConfig.Value.Day);
                                    if (DateTime.Now.Date > _DateConfig)
                                    {
                                        _dateTo = new DateTime(year.Value + 1, _DateConfig.Value.Month, _DateConfig.Value.Day);
                                    }
                                    else
                                    {
                                        _dateTo = DateTime.Now.Date;
                                    }
                                }
                                if (_dateTo != null)
                                {
                                    datetimeNode = _dateTo;
                                    objPerformanceModel.ReportEvalutionDataNode = datetimeNode;
                                }

                                if (objEvalutionData.CostCentreCode != null)
                                    objPerformanceModel.GroupCode = objEvalutionData.CostCentreCode;
                                if (objEvalutionData.WorkPlaceCode != null)
                                    objPerformanceModel.LocationByGroupCode = objEvalutionData.WorkPlaceCode;

                                #region tham nien
                                if (objEvalutionData.DateOfBirth != null)
                                    objPerformanceModel.Age = DateTime.Today.Year - objEvalutionData.DateOfBirth.Value.Year;
                                if (objEvalutionData.DateHire != null)
                                {
                                    objPerformanceModel.Entering = objEvalutionData.DateHire;
                                    double dayServiceYear;
                                    dayServiceYear = datetimeNode.Value.Subtract(objEvalutionData.DateHire.Value).TotalDays;
                                    double _TotalYear = dayServiceYear / 360;
                                    double _tempYear = 0;
                                    if (_TotalYear > 0)
                                    {
                                        string[] parts = _TotalYear.ToString().Split('.');
                                        double _intMonth = double.Parse(parts[0]);
                                        if (parts.Length > 1)
                                        {
                                            double _modMonth = double.Parse("0." + parts[1]) * 12;
                                            if (_modMonth >= 10)
                                            {
                                                _tempYear = _intMonth + (_modMonth / 100);
                                            }
                                            else
                                            {
                                                _tempYear = _intMonth + (_modMonth / 10);
                                            }
                                        }
                                        objPerformanceModel.ServiceYear = String.Format("{0:0.0}", _tempYear); ;
                                    }
                                    if (objEvalutionData.DateHire.Value > dateFirstKi)
                                    {
                                        if (_TotalYear > 0)
                                            objPerformanceModel.ServiceYearKi = String.Format("{0:0.0}", _tempYear);
                                    }
                                    else
                                    {
                                        objPerformanceModel.ServiceYearKi = String.Format("{0:0.0}", 1.0);
                                    }
                                }
                                if(lstBasicSalary.Count>0)
                                {
                                    var objBasicSalary = lstBasicSalary.Where(s => s.ProfileID == objEvalutionData.ProfileID).FirstOrDefault();
                                    if (objBasicSalary != null)
                                        objPerformanceModel.BasicSalary = double.Parse(objBasicSalary.GrossAmount);
                                }
                                if (objEvalutionData.SalaryClassName != null)
                                    objPerformanceModel.Rank = objEvalutionData.SalaryClassName;
                                if (objEvalutionData.AbilityTitleVNI != null)
                                    objPerformanceModel.QualificationAbilitytitle = objEvalutionData.AbilityTitleVNI;
                                if (objEvalutionData.SalaryClassID != null)
                                {
                                    var objSalaryRank = lstSalaryRank.Where(s => s.SalaryClassID == objEvalutionData.SalaryClassID).FirstOrDefault();
                                    if (objSalaryRank != null)
                                        objPerformanceModel.RankZone = objSalaryRank.SalaryRankName;
                                }
                                objPerformanceModel.JobTitleName = objEvalutionData.JobTitleName;
                                double? totalUnusualAllowance = 0;
                                if (totalUnusualAllowance > 0)
                                    objPerformanceModel.TotalSalaryAllowance = totalUnusualAllowance;
                                if (objEvalutionData.Year != null)
                                    objPerformanceModel.Year = objEvalutionData.Year;
                                #region lan danh gia
                                if (objEvalutionData.TimeEvalutionData != null)
                                    objPerformanceModel.TimeEvalutionData = objEvalutionData.TimeEvalutionData;
                                if (objEvalutionData.TimeEvalutionDataCode != null)
                                    objPerformanceModel.TimeEvalutionDataCode = objEvalutionData.TimeEvalutionDataCode;
                                #endregion
                                #region C1->C32
                                if (objEvalutionData.C1 != null)
                                    objPerformanceModel.C1 = objEvalutionData.C1;
                                if (objEvalutionData.C2 != null)
                                    objPerformanceModel.C2 = objEvalutionData.C2;
                                if (objEvalutionData.C3 != null)
                                    objPerformanceModel.C3 = objEvalutionData.C3;
                                if (objEvalutionData.C4 != null)
                                    objPerformanceModel.C4 = objEvalutionData.C4;
                                if (objEvalutionData.C5 != null)
                                    objPerformanceModel.C5 = objEvalutionData.C5;
                                if (objEvalutionData.C6 != null)
                                    objPerformanceModel.C6 = objEvalutionData.C6;
                                if (objEvalutionData.C7 != null)
                                    objPerformanceModel.C7 = objEvalutionData.C7;
                                if (objEvalutionData.C8 != null)
                                    objPerformanceModel.C8 = objEvalutionData.C8;
                                if (objEvalutionData.C9 != null)
                                    objPerformanceModel.C9 = objEvalutionData.C9;
                                if (objEvalutionData.C10 != null)
                                    objPerformanceModel.C10 = objEvalutionData.C10;
                                if (objEvalutionData.C11 != null)
                                    objPerformanceModel.C11 = objEvalutionData.C11;
                                if (objEvalutionData.C12 != null)
                                    objPerformanceModel.C12 = objEvalutionData.C12;
                                if (objEvalutionData.C13 != null)
                                    objPerformanceModel.C13 = objEvalutionData.C13;
                                if (objEvalutionData.C14 != null)
                                    objPerformanceModel.C14 = objEvalutionData.C14;
                                if (objEvalutionData.C15 != null)
                                    objPerformanceModel.C15 = objEvalutionData.C15;
                                if (objEvalutionData.C16 != null)
                                    objPerformanceModel.C16 = objEvalutionData.C16;
                                if (objEvalutionData.C17 != null)
                                    objPerformanceModel.C17 = objEvalutionData.C17;
                                if (objEvalutionData.C18 != null)
                                    objPerformanceModel.C18 = objEvalutionData.C18;
                                if (objEvalutionData.C19 != null)
                                    objPerformanceModel.C19 = objEvalutionData.C19;
                                if (objEvalutionData.C20 != null)
                                    objPerformanceModel.C20 = objEvalutionData.C20;
                                if (objEvalutionData.C21 != null)
                                    objPerformanceModel.C21 = objEvalutionData.C21;
                                if (objEvalutionData.C22 != null)
                                    objPerformanceModel.C22 = objEvalutionData.C22;
                                if (objEvalutionData.C23 != null)
                                    objPerformanceModel.C23 = objEvalutionData.C23;
                                if (objEvalutionData.C24 != null)
                                    objPerformanceModel.C24 = objEvalutionData.C24;
                                if (objEvalutionData.C25 != null)
                                    objPerformanceModel.C25 = objEvalutionData.C25;
                                if (objEvalutionData.C26 != null)
                                    objPerformanceModel.C26 = objEvalutionData.C26;
                                if (objEvalutionData.C27 != null)
                                    objPerformanceModel.C27 = objEvalutionData.C27;
                                if (objEvalutionData.C28 != null)
                                    objPerformanceModel.C28 = objEvalutionData.C28;
                                if (objEvalutionData.C29 != null)
                                    objPerformanceModel.C29 = objEvalutionData.C29;
                                if (objEvalutionData.C30 != null)
                                    objPerformanceModel.C30 = objEvalutionData.C30;
                                if (objEvalutionData.C31 != null)
                                    objPerformanceModel.C31 = objEvalutionData.C31;
                                if (objEvalutionData.C32 != null)
                                    objPerformanceModel.C32 = objEvalutionData.C32;
                                #endregion
                                #endregion
                                #region thoi gian danh gia
                                if (_dateFrom != null)
                                    objPerformanceModel.DateStart = _dateFrom;
                                if (_dateTo != null)
                                    objPerformanceModel.DateEnd = _dateTo;
                            }
                        }
                                #endregion
                    }
                    lstPerformanceModel.Add(objPerformanceModel);
                }
                return lstPerformanceModel;
            }
        }
        DataTable CreateEvalutionDataSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Eva_ReportEvalutionDataEntity");
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.ProfileName);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C1))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C1, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C2))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C2, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C3))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C3);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C4))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C4, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C5))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C5, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C6))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C6, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C7))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C7, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C8))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C8, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C9))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C9, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C10))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C10, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C11))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C11, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C12))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C12, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C13))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C13, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C14))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C14, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C15))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C15, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C16))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C16, typeof(int));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C17))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C17);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C18))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C18);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C19))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C19, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C20))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C20, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C21))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C21, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C22))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C22);
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C23))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C23, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C24))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C24, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C25))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C25, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C26))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C26, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C27))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C27, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C28))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C28, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C29))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C29, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C30))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C30, typeof(double));
                if (!tb.Columns.Contains(Eva_ReportEvalutionDataEntity.FieldNames.C31))
                    tb.Columns.Add(Eva_ReportEvalutionDataEntity.FieldNames.C31);
                return tb;
            }
        }
        public DataTable GetEvalutionDataByTemplate(int year, int? time, string orgStructureID,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                DataTable table = CreateEvalutionDataSchema();

                DateTime _daystart = new DateTime(year - 1, 04, 01);
                int daysInFeb = System.DateTime.DaysInMonth(year, 2);
                DateTime _dayend = new DateTime(year, 02, daysInFeb);
                if (time == 2)
                {
                    int daysInMar = System.DateTime.DaysInMonth(year, 3);
                    _dayend = new DateTime(year, 03, daysInMar);
                }
                string status = string.Empty;
                List<Eva_EvalutionDataEntity> lstEvalutionDataEntity = new List<Eva_EvalutionDataEntity>();
                //    //ds nv
                var hrService = new Hre_ProfileServices();
                var service = new BaseService();
                List<object> strOrgIDs = new List<object>();
                strOrgIDs.AddRange(new object[3]);
                strOrgIDs[0] = (object)orgStructureID;
                var lstProfile = hrService.GetData<Hre_ProfileEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).ToList();
                if (lstProfile == null)
                    return table;
                DateTime _datetime = new DateTime(year, 01, 01);
                List<object> paraEvalutionData = new List<object>();
                paraEvalutionData.AddRange(new object[13]);

                paraEvalutionData[0] = orgStructureID;
                paraEvalutionData[1] = year;
                paraEvalutionData[2] = _datetime;
                paraEvalutionData[3] = time;
                paraEvalutionData[11] = 1;
                paraEvalutionData[12] = int.MaxValue - 1;

                var evaServiceEvalutionData = new Eva_EvalutionDataServices();
                var lstEvalutionData = evaServiceEvalutionData.GetData<Eva_EvalutionDataEntity>(paraEvalutionData, ConstantSql.hrm_eva_sp_get_EvalutionData, userLogin, ref status);
                foreach (var item in lstEvalutionData)
                {
                    var profile = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                    if (profile != null)
                    {
                        DataRow row = table.NewRow();

                        row[Eva_ReportEvalutionDataEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        row[Eva_ReportEvalutionDataEntity.FieldNames.ProfileName] = profile.ProfileName;
                        if (item.C1 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C1] = item.C1;
                        if (item.C2 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C2] = item.C2;
                        if (item.C3 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C3] = item.C3;
                        if (item.C4 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C4] = item.C4;
                        if (item.C5 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C5] = item.C5;
                        if (item.C6 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C6] = item.C6;
                        if (item.C7 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C7] = item.C7;
                        if (item.C8 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C8] = item.C8;
                        if (item.C9 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C9] = item.C9;
                        if (item.C10 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C10] = item.C10;
                        if (item.C11 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C11] = item.C11;
                        if (item.C12 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C12] = item.C12;
                        if (item.C13 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C13] = item.C13;
                        if (item.C14 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C14] = item.C14;
                        if (item.C15 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C15] = item.C15;
                        if (item.C16 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C16] = item.C16;
                        if (item.C17 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C17] = item.C17;
                        if (item.C18 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C18] = item.C18;
                        if (item.C19 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C19] = item.C19;
                        if (item.C20 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C20] = item.C20;
                        if (item.C21 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C21] = item.C21;
                        if (item.C22 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C22] = item.C22;
                        if (item.C23 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C23] = item.C23;
                        if (item.C24 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C24] = item.C24;
                        if (item.C25 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C25] = item.C25;
                        if (item.C26 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C26] = item.C26;
                        if (item.C27 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C27] = item.C27;
                        if (item.C28 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C28] = item.C28;
                        if (item.C29 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C29] = item.C29;
                        if (item.C30 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C30] = item.C30;
                        if (item.C31 != null)
                            row[Eva_ReportEvalutionDataEntity.FieldNames.C31] = item.C31;
                        table.Rows.Add(row);
                    }
                }
                return table.ConfigTable(true);
                //    #endregion
            }
        }
    }
}
