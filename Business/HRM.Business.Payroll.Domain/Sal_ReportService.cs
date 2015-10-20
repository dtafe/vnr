using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HRM.Data.Entity.Repositories;
using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using VnResource.Helper.Data;
using HRM.Business.Attendance.Domain;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Insurance.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.Evaluation.Models;


namespace HRM.Business.Payroll.Domain
{
    public class Sal_ReportService : BaseService
    {
        #region BC bảng lương tháng của toàn công ty
        /// <summary>
        /// Khỏi tạo các cột dữ liệu cho BC
        /// </sumary>    
        public DataTable GetSchemaOverallPayroll()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tblData = new DataTable("Sal_ReportBasicSalaryMonthlyModel");
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ID);
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, typeof(string));
                tblData.Columns.Add("CodeAttendance", typeof(string));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, typeof(string));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IsPaid, typeof(bool));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName, typeof(string));
                tblData.Columns.Add("LaborType", typeof(string));
                tblData.Columns.Add("CostCenterCodePayrollTable", typeof(string));
                tblData.Columns.Add("OrgStructureType", typeof(string));
                tblData.Columns.Add("EmployeeType", typeof(string));
                tblData.Columns.Add("ShopGroupName", typeof(string));
                tblData.Columns.Add("Supervisor", typeof(string));
                tblData.Columns.Add("HighSupervisor", typeof(string));
                tblData.Columns.Add("WorkPlace", typeof(string));
                tblData.Columns.Add("IDNo", typeof(string));
                tblData.Columns.Add("MonthYear", typeof(DateTime));
                tblData.Columns.Add("DateHire", typeof(DateTime));
                tblData.Columns.Add("DateQuit", typeof(DateTime));
                tblData.Columns.Add("DateEndProbation", typeof(DateTime));
                tblData.Columns.Add("Email", typeof(string));
                tblData.Columns.Add("MonthStart", typeof(DateTime));
                tblData.Columns.Add("MonthEnd", typeof(DateTime));
                tblData.Columns.Add("PositionName", typeof(string));
                tblData.Columns.Add("PositionCode", typeof(string));
                tblData.Columns.Add("JobtitleName", typeof(string));
                tblData.Columns.Add("JobtitleCode", typeof(string));
                tblData.Columns.Add("ShiftLeaderName", typeof(String));
                tblData.Columns.Add("Rank", typeof(String));
                tblData.Columns.Add("Target", typeof(double));
                tblData.Columns.Add("Actual", typeof(double));
                tblData.Columns.Add("BankAccountNo", typeof(string));
                tblData.Columns.Add("AccountNo", typeof(string));
                tblData.Columns.Add("AccountNo2", typeof(string));
                tblData.Columns.Add("bankCode", typeof(string));
                tblData.Columns.Add("BankName", typeof(string));
                tblData.Columns.Add("SalaryClassName", typeof(string));
                tblData.Columns.Add("CostSource", typeof(string));
                tblData.Columns.Add("TaxCode", typeof(string));
                tblData.Columns.Add("CostCenter", typeof(string));
                tblData.Columns.Add("CostCenterCode", typeof(string));
                tblData.Columns.Add("PayrollGroup", typeof(string));
                tblData.Columns.Add("SocialInsNo", typeof(string));
                tblData.Columns.Add("Công Ty", typeof(String));
                tblData.Columns.Add("Chi Nhánh", typeof(String));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Channel, typeof(String));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Region, typeof(String));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Area, typeof(String));
                tblData.Columns.Add("Mã Công Ty", typeof(String));
                tblData.Columns.Add("Mã Chi Nhánh", typeof(String));
                tblData.Columns.Add("Mã Channel", typeof(String));
                tblData.Columns.Add("Mã Region", typeof(String));
                tblData.Columns.Add("Mã Area", typeof(String));
                return tblData;
            }

        }

        /// <summary>
        /// BC bảng lương
        /// <param name="dateStartCutOffDuration">Ngày bắt đầu của kỳ lương</param>
        /// <param name="monthYear">Tháng</param>
        /// <param name="listOrgIDs">Ds phòng ban</param>
        /// <param name="listPrGroupIDs">Ds nhóm Lương</param>
        /// <param name="isIncludeQuitEmp">Bao gồm NV nghỉ việc</param>
        /// </sumary>  
        public DataTable RefreshData(DateTime dateStartCutOffDuration, DateTime dateEndCutOffDuration, DateTime monthYear, List<Hre_ProfileEntity> listProfile, Guid? gradePayrollID, Boolean isIncludeQuitEmp, string codeEmp, string orderNumber, string Transfer, Guid[] workingPlaceID, Guid[] costcenterIds, string UserLogin)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {


                    #region " Load dữ liệu"
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                    //kỳ lương (hiện tại mặc định đầu tháng -> cuối tháng)
                    DateTime from = new DateTime(monthYear.Year, monthYear.Month, 1);
                    DateTime to = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));

                    //Ds tất cả phòng ban
                    var orgServices = new Cat_OrgStructureServices();
                    var lstObjOrg = new List<object>();
                    //  var reposOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                    var listOrgAll = orgServices.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, UserLogin, ref status).ToList();

                    var lstObjOrgNumber = new List<object>();
                    lstObjOrgNumber.Add(orderNumber);
                    var lstOrgID = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrgNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => s.ID).ToList();


                    //Ds cửa Hàng
                    var shopServices = new Cat_ShopServices();
                    var lstObjShop = new List<object>();
                    lstObjShop.Add(null);
                    lstObjShop.Add(null);
                    lstObjShop.Add(null);
                    lstObjShop.Add(1);
                    lstObjShop.Add(int.MaxValue - 1);
                    var lstShop = shopServices.GetData<Cat_ShopEntity>(lstObjShop, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status).ToList();

                    //Ds tất cả loại PB
                    var orgTypeServices = new Cat_OrgStructureTypeServices();
                    var lstObjOrgType = new List<object>();
                    lstObjOrgType.Add(string.Empty);
                    lstObjOrgType.Add(string.Empty);
                    lstObjOrgType.Add(1);
                    lstObjOrgType.Add(int.MaxValue - 1);
                    var listOrgType = orgTypeServices.GetData<Cat_OrgStructureType>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status).ToList();

                    //Ds nhân viên
                    var reposProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    var profileServices = new Hre_ProfileServices();
                    var lstObjProfile = new List<object>();
                    lstObjProfile.AddRange(new object[17]);
                    lstObjProfile[15] = 1;
                    lstObjProfile[16] = int.MaxValue - 1;

                    var lstModel = new List<object>();
                    lstModel.AddRange(new object[10]);
                    //lstModel[4] = CutOffDuration.DateStart;
                    //lstModel[5] = CutOffDuration.DateEnd;
                    lstModel[8] = 1;
                    lstModel[9] = Int32.MaxValue - 1;
                    List<Sal_HoldSalaryEntity> listHoldSalary = orgTypeServices.GetData<Sal_HoldSalaryEntity>(lstModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status).Where(m => m.MonthSalary <= dateEndCutOffDuration && (m.MonthEndSalary == null || m.MonthEndSalary >= dateStartCutOffDuration)).ToList();
                    //bỏ những nhân viên đang bị hold lương
                    listProfile = listProfile.Where(m => !listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();

                    bool isGroup = profileServices.IsGroupByOrgProfileQuit();

                    //lọc theo nhóm lương
                    List<Guid> listProfileID = listProfile.Select(s => s.ID).Distinct().ToList();

                    //lọc nhân viên nghỉ việc
                    if (!isIncludeQuitEmp)
                    {
                        listProfile = listProfile.Where(pro => pro.DateQuit == null || pro.DateQuit > from).ToList();
                    }

                    if (workingPlaceID != null)
                    {
                        listProfile = listProfile.Where(pro => pro.WorkPlaceID != null && workingPlaceID.Contains(pro.WorkPlaceID.Value)).ToList();
                    }

                    if (costcenterIds != null)
                    {
                        listProfile = listProfile.Where(pro => pro.CostCentreID != null && costcenterIds.Contains(pro.CostCentreID.Value)).ToList();

                    }

                    //lọc theo tên nhân viên
                    if (!string.IsNullOrEmpty(codeEmp))
                    {
                        listProfile = listProfile.Where(s => s.CodeEmp != null && s.CodeEmp.Contains(codeEmp)).ToList();
                    }

                    //ds chế độ lương
                    var saleGradeServices = new Sal_GradeServices();
                    var lstObjSalGrade = new List<object>();
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(1);
                    lstObjSalGrade.Add(int.MaxValue - 1);

                    var reposSalGrade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                    var lstGradeAll = reposSalGrade.FindBy(s => s.IsDelete != true).ToList();
                    //var lstGradeAll = GetData<Sal_GradeEntity>(lstObjSalGrade, ConstantSql.hrm_sal_sp_get_Sal_Grade, UserLogin,ref status);
                    //ds thông tin lương

                    var salInfoServices = new Sal_SalaryInformationServices();
                    var lstObjSalInfo = new List<object>();
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(null);
                    lstObjSalInfo.Add(1);
                    lstObjSalInfo.Add(int.MaxValue - 1);

                    var salInfoServices1 = new Sal_SalaryInformationServices();
                    var lstProfileIDs = new List<Guid>();
                    var lstSalaryInformation = salInfoServices.GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref status).ToList();
                    if (Transfer == EnumDropDown.Transfer.E_TRANSFER.ToString())
                    {
                        lstSalaryInformation = lstSalaryInformation.Where(s => s.IsCash == true).ToList();
                        lstProfileIDs = lstSalaryInformation.Select(s => s.ProfileID).ToList();
                    }
                    if (Transfer == EnumDropDown.Transfer.E_CASH.ToString())
                    {
                        lstSalaryInformation = lstSalaryInformation.Where(s => s.IsCash == false).ToList();
                        lstProfileIDs = lstSalaryInformation.Select(s => s.ProfileID).ToList();
                    }
                    if (Transfer == EnumDropDown.Transfer.E_OTHER.ToString())
                    {
                        lstProfileIDs = lstSalaryInformation.Select(s => s.ProfileID).ToList();
                    }

                    //ds loại mã lương 
                    var salaryClassServices = new Cat_SalaryClassServices();
                    var lstObjSalaryClass = new List<object>();
                    lstObjSalaryClass.Add(null);
                    lstObjSalaryClass.Add(1);
                    lstObjSalaryClass.Add(int.MaxValue - 1);
                    var lstSalaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjSalaryClass, ConstantSql.hrm_cat_sp_get_SalaryClass, UserLogin, ref status).ToList().Translate<Cat_SalaryClass>();

                    var revenueProfileServices = new Sal_RevenueForProfileServices();
                    var lstObjRevenueForProfile = new List<object>();
                    lstObjRevenueForProfile.Add(null);
                    lstObjRevenueForProfile.Add(1);
                    lstObjRevenueForProfile.Add(int.MaxValue - 1);
                    var lstRevenueForProfile = revenueProfileServices.GetData<Sal_RevenueForProfileEntity>(lstObjRevenueForProfile, ConstantSql.hrm_sal_sp_get_RevenueForProfile, UserLogin, ref status).ToList();

                    //Ds phần tử lương
                    var elementServices = new Cat_ElementServices();
                    var methodPayroll = MethodPayroll.E_NORMAL.ToString();
                    var lstObjElement = new List<object>();
                    lstObjElement.AddRange(new object[8]);
                    lstObjElement[6] = 1;
                    lstObjElement[7] = int.MaxValue - 1;
                    var reposPayrollElement = new CustomBaseRepository<Cat_Element>(unitOfWork);
                    var listPayrollElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_ElementByMethod, UserLogin, ref status).Where(s => s.MethodPayroll == methodPayroll).ToList().Translate<Cat_Element>();

                    #region Chọn lấy  back up bảng lương - Tung.Ly 20150513
                    var salComputePayrollService = new Sal_ComputePayrollServices();
                    var lstSalPayrollTb = new List<Sal_PayrollTableEntity>();
                    var lstSalPayrollTbItem = new List<Sal_PayrollTableItemEntity>();

                    #region param store bang luong
                    var payrollTableServices = new Sal_PayrollTableServices();
                    var lstObjPayrollTable = new List<object>();
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(from);
                    lstObjPayrollTable.Add(to);
                    lstObjPayrollTable.Add(1);
                    lstObjPayrollTable.Add(int.MaxValue - 1);
                    #endregion

                    #region param store bảng lương chi tiết
                    var payrollTableItemServices = new Sal_PayrollTableItemServices();
                    var lstObjPayrollTableItem = new List<object>();
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(from);
                    lstObjPayrollTableItem.Add(to);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(1);
                    lstObjPayrollTableItem.Add(int.MaxValue - 1);
                    #endregion

                    //kiem ra co du lieu backup không theo kỳ lương
                    if (salComputePayrollService.CheckDataIsBackUp(TypeDataBKInScheduleTask.E_PAYROLL_BK.ToString(), dateStartCutOffDuration))
                    {
                        //Du liệu backup
                        #region ds bảng lương(backcup)
                        lstSalPayrollTb = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTableBK, UserLogin, ref status)
                            .Where(pr => pr.IsDelete == null && pr.MonthYear == monthYear && listProfileID.Contains(pr.ProfileID)).ToList();
                        List<Guid> listSalPayrollIDs = lstSalPayrollTb.Select(p => p.ID).ToList();
                        #endregion

                        #region   ds bảng lương chi tiết(backup)
                        lstSalPayrollTbItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItemBK, UserLogin, ref status)
                            .Where(it => it.IsDelete == null && listSalPayrollIDs.Contains(it.PayrollTableID)).ToList();
                        #endregion
                    }
                    else
                    {
                        #region ds bảng lương
                        //  var reposSalPayrollTb = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                        lstSalPayrollTb = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status)
                            .Where(pr => pr.IsDelete == null && pr.MonthYear == monthYear && listProfileID.Contains(pr.ProfileID)).ToList();
                        List<Guid> listSalPayrollIDs = lstSalPayrollTb.Select(p => p.ID).ToList();
                        #endregion

                        #region   ds bảng lương chi tiết
                        //  var reposSalPayrollTbItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                        lstSalPayrollTbItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status)
                            .Where(it => it.IsDelete == null && listSalPayrollIDs.Contains(it.PayrollTableID)).ToList();
                        #endregion
                    }
                    #endregion

                    //  var lstSalPayrollTbItem = reposSalPayrollTbItem.GetAll().Where(it => it.IsDelete == null && listSalPayrollIDs.Contains(it.PayrollTableID)).ToList();
                    //Tạo cột dữ liệu cho table
                    DataTable tblData = GetSchemaOverallPayroll();
                    #endregion
                    #region " Không có dữ liệu Sal_PayrollTable"
                    if (lstSalPayrollTb == null || lstSalPayrollTb.Count <= 0)
                    {
                        foreach (Hre_ProfileEntity profile in listProfile)
                        {
                            var lstRankByProfileId = lstShop.Where(s => s.ID == profile.ShopID).FirstOrDefault();
                            DataRow dr = tblData.NewRow();
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthYear] = monthYear;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeAttendance] = profile.CodeAttendance;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.LaborType] = profile.LaborType;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = profile.OrgStructureName;

                            dr["ShopGroupName"] = lstRankByProfileId != null ? lstRankByProfileId.ShopGroupName : string.Empty;
                            dr["OrgStructureType"] = profile.E_COMPANY_CODE + "->" + profile.E_BRANCH_CODE + "->" + profile.E_UNIT_CODE + "->" + profile.E_DIVISION_CODE + "->" + profile.E_DEPARTMENT_CODE + "->" + profile.E_TEAM_CODE + "->" + profile.E_SECTION_CODE;

                            //Sal_BasicSalaryEntity BasicSalaryByProfile = listBasicSalary.SingleOrDefault(m => m.ProfileID == profile.ID);
                            //dr["SalaryRankName"] = BasicSalaryByProfile != null ? BasicSalaryByProfile.SalaryRankName : "";

                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.Cat_EmployeeType != null ? profile.Cat_EmployeeType.EmployeeTypeName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.EmployeeTypeName;
                            //dr["CostCenterCodePayrollTable"] = profile.CostCentreNamePayrollTable;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor != null ? profile.Supervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor != null ? profile.HighSupervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace != null ? profile.Cat_WorkPlace.WorkPlaceName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.WorkPlaceName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax != null ? profile.CodeTax : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo != null ? profile.IDNo : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.CostCentreName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.CostCentreName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.Code : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.CostCentreCode;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.Cat_Position != null ? profile.Cat_Position.PositionName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.PositionName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.JobTitleName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.JobTitleName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Email] = profile.Email != null ? profile.Email : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Email] = profile.Email;
                            if (profile.DateHire != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                            if (profile.DateQuit != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                            if (profile.DateEndProbation != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateEndProbation] = profile.DateEndProbation.Value;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo != null ? profile.SocialInsNo : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.Cat_PayrollGroup != null ? profile.Cat_PayrollGroup.PayrollGroupName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.PayrollGroupName;
                            dr["CostSource"] = profile.CostSourceName;
                            if (profile.SalaryClassID != null)
                            {
                                Cat_SalaryClass salClass = lstSalaryClass.Where(s => s.ID == profile.SalaryClassID.Value && s.IsDelete == null).FirstOrDefault();
                                if (salClass != null)
                                {
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryClassName] = salClass.SalaryClassName;
                                }
                            }
                            if (profile.OrgStructureID != null)
                            {
                                var orgName = GetParentOrg(listOrgAll, listOrgType, profile.OrgStructureID);
                                if (orgName.Count < 3)
                                {
                                    orgName.Insert(0, string.Empty);
                                    if (orgName.Count < 3)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }
                                }
                                dr[Hre_ReportHCSalesEntity.FieldNames.Channel] = orgName[2];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Region] = orgName[1];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Area] = orgName[0];



                            }

                            //   Hre_Contract hrcontract = lstHreContractAll.Where(hr => hr.ProfileID == profile.ID && hr.IsDelete == null).FirstOrDefault();
                            //if (hrcontract != null && hrcontract.RankRateID != null)
                            //{
                            //    Cat_SalaryRank salrank = lstSalaryRankAll.Where(rk => rk.ID == hrcontract.RankRateID.Value && rk.IsDelete == null).FirstOrDefault();
                            //    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryRankName] = salrank != null ? salrank.SalaryRankName : string.Empty;
                            //}

                            #region Insurance, Bank
                            var salaryInformationList = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();
                            if (salaryInformationList.Count > 0)
                            {
                                Sal_SalaryInformationEntity bankSalary = salaryInformationList[0];
                                string accountTemp = string.Empty;
                                string accountNo = string.Empty;
                                string accountNo2 = string.Empty;
                                string bankNameTemp = string.Empty;
                                string bankCode = string.Empty;

                                accountTemp = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo2 = bankSalary.AccountNo2 == null ? string.Empty : bankSalary.AccountNo2;
                                bankNameTemp = bankSalary.BankName == null ? string.Empty : bankSalary.BankName;
                                bankCode = bankSalary.BankCode1 == null ? string.Empty : bankSalary.BankCode1;

                                if (!string.IsNullOrEmpty(accountTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = accountTemp.Trim();
                                if (!string.IsNullOrEmpty(accountNo))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo] = accountNo.Trim();
                                if (!string.IsNullOrEmpty(accountNo2))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo2] = accountNo2.Trim();
                                if (!string.IsNullOrEmpty(bankCode))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode] = bankCode.Trim();
                                if (!string.IsNullOrEmpty(bankNameTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = bankNameTemp.Trim();
                            }
                            #endregion

                            tblData.Rows.Add(dr);
                        }
                    }
                    #endregion
                    #region " Có dữ liệu tính lương Sal_PayrollTable"
                    else
                    {
                        foreach (Sal_PayrollTableEntity payroll in lstSalPayrollTb)
                        {
                            if (Transfer != null)
                            {
                                listProfile = listProfile.Where(s => lstProfileIDs.Contains(s.ID)).ToList();
                            }
                            Hre_ProfileEntity profile = listProfile.Where(s => s.ID == payroll.ProfileID).FirstOrDefault();

                            if (profile == null)
                                continue;

                            var lstRevenueByProfileId = lstRevenueForProfile.Where(s => s.ProfileID == payroll.ProfileID).FirstOrDefault();
                            var lstRankByProfileId = lstShop.Where(s => s.ID == profile.ShopID).FirstOrDefault();
                            //var unusualByProfileId = lstUnusual.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                            List<Sal_PayrollTableItemEntity> listItem = lstSalPayrollTbItem.Where(pi => pi.PayrollTableID == payroll.ID).ToList();

                            DataRow dr = tblData.NewRow();
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ID] = payroll.ID;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthYear] = monthYear;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeAttendance] = profile.CodeAttendance;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = profile.OrgStructureName;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IsPaid] = payroll.IsPaid == null ? false : payroll.IsPaid.Value;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.LaborType] = profile.LaborType;
                            dr["OrgStructureType"] = profile.E_COMPANY_CODE + "->" + profile.E_BRANCH_CODE + "->" + profile.E_UNIT_CODE + "->" + profile.E_DIVISION_CODE + "->" + profile.E_DEPARTMENT_CODE + "->" + profile.E_TEAM_CODE + "->" + profile.E_SECTION_CODE;
                            dr["ShopGroupName"] = lstRankByProfileId != null ? lstRankByProfileId.ShopGroupName : string.Empty;

                            //Sal_BasicSalaryEntity BasicSalaryByProfile = listBasicSalary.SingleOrDefault(m => m.ProfileID == profile.ID);
                            //dr["SalaryRankName"] = BasicSalaryByProfile != null ? BasicSalaryByProfile.SalaryRankName : "";

                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.Cat_EmployeeType != null ? profile.Cat_EmployeeType.EmployeeTypeName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.EmployeeTypeName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor != null ? profile.Supervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor != null ? profile.HighSupervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace != null ? profile.Cat_WorkPlace.WorkPlaceName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.WorkPlaceName;
                            //  dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkingPlace != null ? profile.WorkingPlace : string.Empty;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax != null ? profile.CodeTax : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo != null ? profile.IDNo : string.Empty;
                            dr["CostCenterCodePayrollTable"] = payroll.CostCentreNamePayrollTable;

                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.CostCentreName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.CostCentreName;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.Code : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.CostCentreCode;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.Cat_Position != null ? profile.Cat_Position.PositionName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.PositionName;
                            //dr["PositionCode"] = profile.Cat_Position != null ? profile.Cat_Position.Code : string.Empty;
                            dr["PositionCode"] = profile.PositionCode;

                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.JobTitleName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.JobTitleName;
                            //dr["JobtitleCode"] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.Code : string.Empty;
                            dr["JobtitleCode"] = profile.JobTitleCode;
                            dr["CostSource"] = profile.CostSourceName;
                            //dr["Rank"] = lstRankByProfileId != null ? lstRankByProfileId.Rank : string.Empty;
                            //dr["Target"] = lstRevenueByProfileId != null ? lstRevenueByProfileId.Target : 0;
                            //dr["Actual"] = lstRevenueByProfileId != null ? lstRevenueByProfileId.Actual : 0;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Email] = profile.Email != null ? profile.Email : string.Empty;
                            if (profile.DateHire != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                            if (profile.DateQuit != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                            if (profile.DateEndProbation != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateEndProbation] = profile.DateEndProbation.Value;

                            //if (unusualByProfileId != null)
                            //{
                            //    if (unusualByProfileId.MonthStart != null)
                            //    {
                            //        dr["MonthStart"] = unusualByProfileId.MonthStart;
                            //    }
                            //}
                            //if (unusualByProfileId != null)
                            //{
                            //    if (unusualByProfileId.MonthEnd != null)
                            //    {
                            //        dr["MonthEnd"] = unusualByProfileId.MonthEnd;
                            //    }
                            //}
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo != null ? profile.SocialInsNo : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo;
                            //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.Cat_PayrollGroup != null ? profile.Cat_PayrollGroup.PayrollGroupName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.PayrollGroupName;
                            if (profile.OrgStructureID != null)
                            {
                                var orgName = new List<string>();
                                var orgCode = new List<string>();
                                if (isGroup)
                                {

                                    var orgEntity = listOrgAll.Where(s => s.ID == profile.OrgStructureID).FirstOrDefault();
                                    if (orgEntity != null)
                                    {
                                        orgName = GetParentOrgNameForShiseido(listOrgAll, listOrgType, profile.OrgStructureID);
                                        if (orgName.Count < 5)
                                        {
                                            orgName.Insert(0, string.Empty);
                                            if (orgName.Count < 5)
                                            {
                                                orgName.Insert(0, string.Empty);
                                            }
                                            if (orgName.Count < 5)
                                            {
                                                orgName.Insert(0, string.Empty);
                                            }
                                            if (orgName.Count < 5)
                                            {
                                                orgName.Insert(0, string.Empty);
                                            }
                                            if (orgName.Count < 5)
                                            {
                                                orgName.Insert(0, string.Empty);
                                            }

                                        }

                                        orgCode = GetParentOrgCodeForShiseido(listOrgAll, listOrgType, profile.OrgStructureID);
                                        if (orgCode.Count < 5)
                                        {
                                            orgCode.Insert(0, string.Empty);
                                            if (orgCode.Count < 5)
                                            {
                                                orgCode.Insert(0, string.Empty);
                                            }
                                            if (orgCode.Count < 5)
                                            {
                                                orgCode.Insert(0, string.Empty);
                                            }
                                            if (orgCode.Count < 5)
                                            {
                                                orgCode.Insert(0, string.Empty);
                                            }
                                            if (orgCode.Count < 5)
                                            {
                                                orgCode.Insert(0, string.Empty);
                                            }
                                        }

                                    }
                                    dr["Công Ty"] = orgName[4];
                                    dr["Chi Nhánh"] = orgName[3];
                                    dr[Hre_ReportHCSalesEntity.FieldNames.Channel] = orgName[2];
                                    dr[Hre_ReportHCSalesEntity.FieldNames.Region] = orgName[1];
                                    dr[Hre_ReportHCSalesEntity.FieldNames.Area] = orgName[0];


                                    dr["Mã Công Ty"] = orgCode[4];
                                    dr["Mã Chi Nhánh"] = orgCode[3];
                                    dr["Mã Channel"] = orgCode[2];
                                    dr["Mã Region"] = orgCode[1];
                                    dr["Mã Area"] = orgCode[0];
                                }





                                //var orgBranch = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_BRANCH, listOrgAll, listOrgType);
                                //var orgOrg = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_DEPARTMENT, listOrgAll, listOrgType);
                                //var orgTeam = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_TEAM, listOrgAll, listOrgType);
                                //var orgSection = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_SECTION, listOrgAll, listOrgType);
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            }



                            //     Hre_Contract hrcontract = lstHreContractAll.Where(hr => hr.ProfileID == profile.ID && hr.IsDelete == null).FirstOrDefault();

                            if (profile.SalaryClassID != null)
                            {
                                Cat_SalaryClass salClass = lstSalaryClass.Where(s => s.ID == profile.SalaryClassID.Value && s.IsDelete == null).FirstOrDefault();
                                if (salClass != null)
                                {
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryClassName] = salClass.SalaryClassName;
                                }
                            }

                            //if (hrcontract != null && hrcontract.RankRateID != null)
                            //{
                            //    if (profile.SalaryClassID != null)
                            //    {
                            //        Cat_SalaryRank salrank = lstSalaryRankAll.Where(rk => rk.SalaryClassID == profile.SalaryClassID.Value && hrcontract.RankRateID == rk.ID && rk.IsDelete == null).FirstOrDefault();
                            //        if (salrank != null)
                            //            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryRankName] = salrank.SalaryRankName;
                            //    }

                            //}

                            #region Insurance, Bank
                            var salaryInformationList = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();


                            if (salaryInformationList.Count > 0)
                            {
                                Sal_SalaryInformationEntity bankSalary = salaryInformationList[0];
                                string accountTemp = string.Empty;
                                string accountNo = string.Empty;
                                string accountNo2 = string.Empty;
                                string bankNameTemp = string.Empty;
                                string bankCode = string.Empty;

                                accountTemp = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo2 = bankSalary.AccountNo2 == null ? string.Empty : bankSalary.AccountNo2;
                                bankNameTemp = bankSalary.BankName == null ? string.Empty : bankSalary.BankName;
                                bankCode = bankSalary.BankCode1 == null ? string.Empty : bankSalary.BankCode1;

                                if (!string.IsNullOrEmpty(accountTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = accountTemp.Trim();
                                if (!string.IsNullOrEmpty(accountNo))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo] = accountNo.Trim();
                                if (!string.IsNullOrEmpty(accountNo2))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo2] = accountNo2.Trim();
                                if (!string.IsNullOrEmpty(bankCode))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode] = bankCode.Trim();
                                if (!string.IsNullOrEmpty(bankNameTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = bankNameTemp.Trim();
                            }
                            #endregion

                            #region sal grade
                            Sal_Grade grade = new Sal_Grade();
                            if (gradePayrollID != null)
                            {
                                grade = lstGradeAll.Where(gr => gr.IsDelete == null && gr.ProfileID == profile.ID && gradePayrollID.Value == gr.GradePayrollID
                                                                    && gr.MonthStart < to).OrderByDescending(gr => gr.MonthStart).FirstOrDefault();
                            }
                            else
                            {
                                grade = lstGradeAll.Where(gr => gr.IsDelete == null && gr.ProfileID == profile.ID
                                                                       && gr.MonthStart < to).OrderByDescending(gr => gr.MonthStart).FirstOrDefault();
                            }


                            if (grade != null)
                            {
                                Cat_GradePayroll gradepayroll = grade.Cat_GradePayroll;
                                if (gradepayroll != null && gradepayroll.IsFormulaSalary == true)
                                {
                                    try
                                    {
                                        var lstSalPrItem = lstSalPayrollTbItem.Where(sal => sal.PayrollTableID == payroll.ID).ToList();
                                        var lstelement = listPayrollElement.Where(pr => pr.GradePayrollID == gradepayroll.ID).ToList();
                                        if (lstelement != null && lstelement.Count > 0)
                                        {
                                            foreach (Cat_Element payrollElement in lstelement)
                                            {
                                                if (payrollElement != null && !String.IsNullOrEmpty(payrollElement.ElementCode))
                                                {
                                                    if (payrollElement.Type == "Nvarchar")
                                                    {
                                                        //Add phần tử vào cột dữ liệu nếu chưa có
                                                        if (!tblData.Columns.Contains(payrollElement.ElementCode))
                                                        {
                                                            tblData.Columns.Add(payrollElement.ElementCode);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Add phần tử vào cột dữ liệu nếu chưa có
                                                        if (!tblData.Columns.Contains(payrollElement.ElementCode))
                                                        {
                                                            tblData.Columns.Add(payrollElement.ElementCode, typeof(Double));
                                                        }
                                                    }



                                                    //Lấy value của phần tử 
                                                    if (tblData.Columns.Contains(payrollElement.ElementCode))
                                                    {
                                                        Sal_PayrollTableItemEntity item = lstSalPrItem.Where(it => it.Code == payrollElement.ElementCode).FirstOrDefault();
                                                        Double value = 0;
                                                        if (item != null)
                                                        {
                                                            if (item.ValueType == typeof(Double).Name)
                                                            {
                                                                Double.TryParse(item.Value, out value);
                                                                dr[payrollElement.ElementCode] = value;
                                                            }
                                                            if (item.ValueType == EnumDropDown.DataType.Nvarchar.ToString())
                                                            {
                                                                dr[payrollElement.ElementCode] = item.Value;
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                }
                                tblData.Rows.Add(dr);
                            }
                            #endregion


                        }
                    }
                    #endregion
                    var configs = new Dictionary<string, Dictionary<string, object>>();
                    var config = new Dictionary<string, object>();
                    config.Add("hidden", true);
                    configs.Add("ID", config);
                    return tblData.ConfigTable(configs, true);
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        #endregion



        #region BC Ứng Lương Tháng
        public DataTable GetSchemaReportUnusualPay()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tblData = new DataTable("Sal_ReportSalaryMonthlyModel");

                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, typeof(string));
                tblData.Columns.Add("CodeAttendance", typeof(string));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, typeof(string));
                tblData.Columns.Add("LaborType", typeof(string));
                tblData.Columns.Add("EmployeeType", typeof(string));

                tblData.Columns.Add("Supervisor", typeof(string));
                tblData.Columns.Add("HighSupervisor", typeof(string));
                tblData.Columns.Add("WorkPlace", typeof(string));
                // tblData.Columns.Add("WorkingPlace", typeof(string));
                tblData.Columns.Add("IDNo", typeof(string));

                tblData.Columns.Add("MonthYear", typeof(DateTime));
                tblData.Columns.Add("DateHire", typeof(DateTime));
                tblData.Columns.Add("DateQuit", typeof(DateTime));
                tblData.Columns.Add("DateEndProbation", typeof(DateTime));
                tblData.Columns.Add("Email", typeof(string));

                tblData.Columns.Add("PositionName", typeof(string));
                tblData.Columns.Add("PositionCode", typeof(string));
                tblData.Columns.Add("JobtitleName", typeof(string));
                tblData.Columns.Add("JobtitleCode", typeof(string));
                // tblData.Columns.Add("ShiftLeaderName", typeof(String));
                //tblData.Columns.Add("Rank", typeof(String));
                //tblData.Columns.Add("Target", typeof(double));
                //tblData.Columns.Add("Actual", typeof(double));

                tblData.Columns.Add("BankAccountNo", typeof(string));
                tblData.Columns.Add("AccountNo", typeof(string));
                tblData.Columns.Add("AccountNo2", typeof(string));
                tblData.Columns.Add("bankCode", typeof(string));
                tblData.Columns.Add("BankName", typeof(string));
                tblData.Columns.Add("SalaryRankName", typeof(string));

                tblData.Columns.Add("TaxCode", typeof(string));
                tblData.Columns.Add("CostCenter", typeof(string));
                tblData.Columns.Add("CostCenterCode", typeof(string));
                tblData.Columns.Add("PayrollGroup", typeof(string));
                tblData.Columns.Add("SocialInsNo", typeof(string));
                tblData.Columns.Add("Công Ty", typeof(String));
                tblData.Columns.Add("Chi Nhánh", typeof(String));


                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Channel, typeof(String));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Region, typeof(String));
                tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Area, typeof(String));
                tblData.Columns.Add("Mã Công Ty", typeof(String));
                tblData.Columns.Add("Mã Chi Nhánh", typeof(String));
                tblData.Columns.Add("Mã Channel", typeof(String));
                tblData.Columns.Add("Mã Region", typeof(String));
                tblData.Columns.Add("Mã Area", typeof(String));


                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchCode, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentCode, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamCode, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionCode, typeof(String));

                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchName, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentName, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamName, typeof(String));
                //tblData.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionName, typeof(String));

                //Ds phần tử lương
                var elementServices = new Cat_ElementServices();
                var lstObjElement = new List<object>();
                lstObjElement.Add(string.Empty);
                lstObjElement.Add(string.Empty);
                lstObjElement.Add(string.Empty);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue);
                //  var reposPayrollElement = new CustomBaseRepository<Cat_ElementMultiEntity>(unitOfWork);
                //   var listPayrollElement = elementServices.GetData<Cat_ElementMultiEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin,ref status).ToList();
                //foreach (var item in listPayrollElement)
                //{
                //    tblData.Columns.Add(item.ElementCode);
                //}
                return tblData;
            }

        }

        public DataTable ReportUnusualPay(DateTime monthYear, List<Guid> listProfileIDs, Guid? payrollGroupID, Boolean isIncludeQuitEmp, string codeEmp, string orderNumber, string UserLogin)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {


                    #region " Load dữ liệu"
                    string status = string.Empty;
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                    //kỳ lương (hiện tại mặc định đầu tháng -> cuối tháng)
                    DateTime from = new DateTime(monthYear.Year, monthYear.Month, 1);
                    DateTime to = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));

                    //Ds tất cả phòng ban
                    var orgServices = new Cat_OrgStructureServices();
                    var lstObjOrg = new List<object>();
                    //  var reposOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                    var listOrgAll = orgServices.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, UserLogin, ref status).ToList();

                    var lstObjOrgNumber = new List<object>();
                    lstObjOrgNumber.Add(orderNumber);
                    var lstOrgID = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrgNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => s.ID).ToList();


                    //Ds cửa Hàng
                    var shopServices = new Cat_ShopServices();
                    var lstObjShop = new List<object>();
                    lstObjShop.Add(null);
                    lstObjShop.Add(null);
                    lstObjShop.Add(null);
                    lstObjShop.Add(1);
                    lstObjShop.Add(int.MaxValue - 1);
                    var lstShop = shopServices.GetData<Cat_ShopEntity>(lstObjShop, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status).ToList();

                    //Ds tất cả loại PB
                    var orgTypeServices = new Cat_OrgStructureTypeServices();
                    var lstObjOrgType = new List<object>();
                    lstObjOrgType.Add(string.Empty);
                    lstObjOrgType.Add(string.Empty);
                    lstObjOrgType.Add(1);
                    lstObjOrgType.Add(int.MaxValue - 1);
                    //    var reposOrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                    var listOrgType = orgTypeServices.GetData<Cat_OrgStructureType>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status).ToList();

                    //Ds nhân viên
                    var reposProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                    var profileServices = new Hre_ProfileServices();
                    var lstObjProfile = new List<object>();
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(string.Empty);
                    lstObjProfile.Add(1);
                    lstObjProfile.Add(int.MaxValue - 1);

                    //lọc theo phòng ban
                    //var listProfile = reposProfile.GetAll().Where(hr => hr.IsDelete == null
                    // && hr.OrgStructureID.HasValue
                    // && listOrgIDs.Contains(hr.OrgStructureID.Value)).ToList();
                    var listProfile = reposProfile.GetAll().Where(s => s.IsDelete != true).ToList();
                    if (listProfileIDs != null)
                    {
                        listProfile = listProfile.Where(s => listProfileIDs.Contains(s.ID)).ToList();
                    }


                    //lọc theo nhóm lương
                    // listProfile = listProfile.Where(hr => hr.PayrollGroupID.HasValue && listPrGroupIDs.Contains(hr.PayrollGroupID.Value)).ToList();
                    List<Guid> listProfileID = listProfile.Select(s => s.ID).Distinct().ToList();

                    //lọc nhân viên nghỉ việc
                    if (!isIncludeQuitEmp)
                    {
                        listProfile = listProfile.Where(pro => pro.DateHire <= to && (pro.DateQuit == null || pro.DateQuit.Value > from)).ToList();
                    }
                    if (payrollGroupID != null && payrollGroupID != Guid.Empty)
                    {
                        listProfile = listProfile.Where(s => s.PayrollGroupID == payrollGroupID).ToList();
                    }

                    //lọc theo tên nhân viên
                    if (!string.IsNullOrEmpty(codeEmp))
                    {
                        listProfile = listProfile.Where(s => s.CodeEmp == codeEmp).ToList();
                    }

                    //ds chế độ lương
                    var saleGradeServices = new Sal_GradeServices();
                    var lstObjSalGrade = new List<object>();
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(string.Empty);
                    lstObjSalGrade.Add(1);
                    lstObjSalGrade.Add(int.MaxValue - 1);

                    var reposSalGrade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                    var lstGradeAll = reposSalGrade.GetAll().Where(s => s.IsDelete != true).ToList();
                    // var lstGradeAll = saleGradeServices.GetData<Sal_Grade>(lstObjSalGrade, ConstantSql.hrm_sal_sp_get_Sal_Grade, UserLogin,ref status).ToList();
                    //ds thông tin lương

                    var salInfoServices = new Sal_SalaryInformationServices();
                    var lstObjSalInfo = new List<object>();
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(string.Empty);
                    lstObjSalInfo.Add(1);
                    lstObjSalInfo.Add(int.MaxValue - 1);

                    var salInfoServices1 = new Sal_SalaryInformationServices();
                    var lstSalaryInformation = salInfoServices.GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref status).OrderByDescending(sal => sal.DateUpdate).ToList().Translate<Sal_SalaryInformation>();

                    //var reposSalaryInformation = new CustomBaseRepository<Sal_SalaryInformation>(unitOfWork);
                    //var lstSalaryInformation = salInfoServices.GetData<Sal_SalaryInformation>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation,UserLogin,ref status).OrderByDescending(sal => sal.DateUpdate).ToList();
                    //       var lstSalaryInformation = new Sal_SalaryInformation();

                    //ds hợp đồng
                    var contractServices = new Hre_ContractServices();
                    var lstObjContract = new List<object>();
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(string.Empty);
                    lstObjContract.Add(1);
                    lstObjContract.Add(int.MaxValue - 1);
                    var reposHreContract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                    //  var lstHreContractAll = contractServices.GetData<Hre_ContractEntity>(lstObjContract, ConstantSql.hrm_hr_sp_get_Contract, UserLogin,ref status).ToList().Translate<Hre_Contract>();
                    var lstHreContractAll = reposHreContract.GetAll().Where(s => s.IsDelete != true).ToList();

                    //ds mức lươn64864866498*-*/-*/7896543564555555978g
                    var salaryRankServices = new Cat_SalaryRankServices();
                    var lstObjSalaryRank = new List<object>();
                    lstObjSalaryRank.Add(string.Empty);
                    lstObjSalaryRank.Add(null);
                    lstObjSalaryRank.Add(1);
                    lstObjSalaryRank.Add(int.MaxValue - 1);
                    var reposSalaryRank = new CustomBaseRepository<Cat_SalaryRank>(unitOfWork);
                    var lstSalaryRankAll = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).ToList().Translate<Cat_SalaryRank>();

                    var revenueProfileServices = new Sal_RevenueForProfileServices();
                    var lstObjRevenueForProfile = new List<object>();
                    lstObjRevenueForProfile.Add(null);
                    lstObjRevenueForProfile.Add(1);
                    lstObjRevenueForProfile.Add(int.MaxValue - 1);
                    var lstRevenueForProfile = revenueProfileServices.GetData<Sal_RevenueForProfileEntity>(lstObjRevenueForProfile, ConstantSql.hrm_sal_sp_get_RevenueForProfile, UserLogin, ref status).ToList();

                    //Ds phần tử lương
                    var elementServices = new Cat_ElementServices();
                    var methodPayroll = MethodPayroll.E_ADNVANCE_PAYMENT.ToString();
                    var lstObjElement = new List<object>();
                    lstObjElement.Add("");
                    lstObjElement.Add("");
                    lstObjElement.Add("");
                    lstObjElement.Add(1);
                    lstObjElement.Add(int.MaxValue - 1);
                    var reposPayrollElement = new CustomBaseRepository<Cat_Element>(unitOfWork);
                    //var reposPayrollElement = new CustomBaseRepository<Cat_ElementMultiEntity>(unitOfWork);
                    // var listPayrollElement = elementServices.GetData<Cat_ElementMultiEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element,UserLogin,ref status).ToList();
                    var listPayrollElement = reposPayrollElement.GetAll().Where(s => s.MethodPayroll == methodPayroll && s.IsDelete != true).ToList();


                    // ds Bảng Ứng Lương
                    var repoUnusualPay = new CustomBaseRepository<Sal_UnusualPay>(unitOfWork);
                    var lstUnusualPay = repoUnusualPay.GetAll().Where(s => s.IsDelete != true && s.MonthYear == monthYear && listProfileID.Contains(s.ProfileID)).ToList();
                    List<Guid> lstUnusualPayIDs = lstUnusualPay.Select(s => s.ID).ToList();

                    //Ds Bảng Ứng Lương Chi Tiết
                    var repoUnusualPayItem = new CustomBaseRepository<Sal_UnusualPayItem>(unitOfWork);
                    var lstUnusualPayItem = repoUnusualPayItem.GetAll().Where(s => s.IsDelete != true && lstUnusualPayIDs.Contains(s.UnusualPayID)).ToList();

                    //      //ds bảng lương
                    //      var payrollTableServices = new Sal_PayrollTableServices();
                    //      var lstObjPayrollTable = new List<object>();
                    //      lstObjPayrollTable.Add(string.Empty);
                    //      lstObjPayrollTable.Add(string.Empty);
                    //      lstObjPayrollTable.Add(1);
                    //      lstObjPayrollTable.Add(int.MaxValue -1);
                    //      var reposSalPayrollTb = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                    ////      var lstSalPayrollTb = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin,ref status).Where(pr => pr.IsDelete == null && pr.MonthYear == monthYear && listProfileID.Contains(pr.ProfileID)).ToList();
                    //      var lstSalPayrollTb = reposSalPayrollTb.GetAll().Where(pr => pr.IsDelete == null && pr.MonthYear == monthYear && listProfileID.Contains(pr.ProfileID)).ToList();
                    //      List<Guid> listSalPayrollIDs = lstSalPayrollTb.Select(p => p.ID).ToList();
                    //      //order theo pb
                    //      lstSalPayrollTb = lstSalPayrollTb.OrderBy(m => m.Hre_Profile.Cat_OrgStructure.OrderOrg != null ? m.Hre_Profile.Cat_OrgStructure.OrderOrg.Value : 0).ToList();

                    // //ds bảng lương chi tiết
                    // var payrollTableItemServices = new Sal_PayrollTableItemServices();
                    // var lstObjPayrollTableItem = new List<object>();
                    // lstObjPayrollTableItem.Add(string.Empty);
                    // lstObjPayrollTableItem.Add(string.Empty);
                    // lstObjPayrollTableItem.Add(1);
                    // lstObjPayrollTableItem.Add(int.MaxValue -1);
                    // var reposSalPayrollTbItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                    //// var lstSalPayrollTbItem = payrollTableItemServices.GetData<Sal_PayrollTableItem>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin,ref status).Where(it => it.IsDelete == null && listSalPayrollIDs.Contains(it.PayrollTableID)).ToList();
                    // var lstSalPayrollTbItem = reposSalPayrollTbItem.GetAll().Where(it => it.IsDelete == null && listSalPayrollIDs.Contains(it.PayrollTableID)).ToList();

                    //Tạo cột dữ liệu cho table
                    DataTable tblData = GetSchemaReportUnusualPay();
                    #endregion
                    #region " Không có dữ liệu Sal_PayrollTable"
                    if (lstUnusualPay == null || lstUnusualPay.Count <= 0)
                    {
                        foreach (Hre_Profile profile in listProfile)
                        {
                            DataRow dr = tblData.NewRow();
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthYear] = monthYear;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeAttendance] = profile.CodeAttendance;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.LaborType] = profile.LaborType;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.Cat_EmployeeType != null ? profile.Cat_EmployeeType.EmployeeTypeName : string.Empty;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor != null ? profile.Supervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor != null ? profile.HighSupervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace != null ? profile.Cat_WorkPlace.WorkPlaceName : string.Empty;
                            //   dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkingPlace != null ? profile.WorkingPlace : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax != null ? profile.CodeTax : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo != null ? profile.IDNo : string.Empty;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.CostCentreName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.Code : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.Cat_Position != null ? profile.Cat_Position.PositionName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.JobTitleName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Email] = profile.Email != null ? profile.Email : string.Empty;
                            if (profile.DateHire != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                            if (profile.DateQuit != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                            if (profile.DateEndProbation != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateEndProbation] = profile.DateEndProbation.Value;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo != null ? profile.SocialInsNo : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.Cat_PayrollGroup != null ? profile.Cat_PayrollGroup.PayrollGroupName : string.Empty;

                            if (profile.OrgStructureID != null)
                            {
                                var orgName = GetParentOrg(listOrgAll, listOrgType, profile.OrgStructureID);
                                if (orgName.Count < 3)
                                {
                                    orgName.Insert(0, string.Empty);
                                    if (orgName.Count < 3)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }
                                }
                                dr[Hre_ReportHCSalesEntity.FieldNames.Channel] = orgName[2];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Region] = orgName[1];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Area] = orgName[0];

                                //var orgBranch = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_BRANCH, listOrgAll, listOrgType);
                                //var orgOrg = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_DEPARTMENT, listOrgAll, listOrgType);
                                //var orgTeam = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_TEAM, listOrgAll, listOrgType);
                                //var orgSection = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_SECTION, listOrgAll, listOrgType);
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            }

                            Hre_Contract hrcontract = lstHreContractAll.Where(hr => hr.ProfileID == profile.ID && hr.IsDelete == null).FirstOrDefault();
                            if (hrcontract != null && hrcontract.RankRateID != null)
                            {
                                Cat_SalaryRank salrank = lstSalaryRankAll.Where(rk => rk.ID == hrcontract.RankRateID.Value && rk.IsDelete == null).FirstOrDefault();
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryRankName] = salrank != null ? salrank.SalaryRankName : string.Empty;
                            }

                            #region Insurance, Bank
                            var salaryInformationList = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();
                            if (salaryInformationList.Count > 0)
                            {
                                Sal_SalaryInformation bankSalary = salaryInformationList[0];
                                string accountTemp = string.Empty;
                                string accountNo = string.Empty;
                                string accountNo2 = string.Empty;
                                string bankNameTemp = string.Empty;
                                string bankCode = string.Empty;

                                accountTemp = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo2 = bankSalary.AccountNo2 == null ? string.Empty : bankSalary.AccountNo2;
                                bankNameTemp = bankSalary.Cat_Bank == null ? string.Empty : bankSalary.Cat_Bank.BankName;
                                bankCode = bankSalary.Cat_Bank == null ? string.Empty : bankSalary.Cat_Bank.BankCode;

                                if (!string.IsNullOrEmpty(accountTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = accountTemp.Trim();
                                if (!string.IsNullOrEmpty(accountNo))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo] = accountNo.Trim();
                                if (!string.IsNullOrEmpty(accountNo2))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo2] = accountNo2.Trim();
                                if (!string.IsNullOrEmpty(bankCode))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode] = bankCode.Trim();
                                if (!string.IsNullOrEmpty(bankNameTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = bankNameTemp.Trim();
                            }
                            #endregion

                            tblData.Rows.Add(dr);
                        }
                    }
                    #endregion
                    #region " Có dữ liệu tính lương Sal_PayrollTable"
                    else
                    {
                        foreach (Sal_UnusualPay payroll in lstUnusualPay)
                        {
                            Hre_Profile profile = listProfile.Where(s => s.ID == payroll.ProfileID).FirstOrDefault();
                            // var lstRevenueByProfileId = lstRevenueForProfile.Where(s => s.ProfileID == payroll.ProfileID).FirstOrDefault();
                            //  var lstRankByProfileId = lstShop.Where(s => s.ID == profile.ShopID).FirstOrDefault();
                            List<Sal_UnusualPayItem> listItem = lstUnusualPayItem.Where(pi => pi.UnusualPayID == payroll.ID).ToList();
                            if (profile == null)
                                continue;

                            DataRow dr = tblData.NewRow();
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthYear] = monthYear;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeAttendance] = profile.CodeAttendance;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.LaborType] = profile.LaborType;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EmployeeType] = profile.Cat_EmployeeType != null ? profile.Cat_EmployeeType.EmployeeTypeName : string.Empty;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Supervisor] = profile.Supervisor != null ? profile.Supervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.HighSupervisor] = profile.HighSupervisor != null ? profile.HighSupervisor : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace != null ? profile.Cat_WorkPlace.WorkPlaceName : string.Empty;
                            //  dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkingPlace != null ? profile.WorkingPlace : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax != null ? profile.CodeTax : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.IDNo] = profile.IDNo != null ? profile.IDNo : string.Empty;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.CostCentreName : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.Cat_CostCentre != null ? profile.Cat_CostCentre.Code : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName] = profile.Cat_Position != null ? profile.Cat_Position.PositionName : string.Empty;
                            dr["PositionCode"] = profile.Cat_Position != null ? profile.Cat_Position.Code : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.JobTitleName : string.Empty;
                            dr["JobtitleCode"] = profile.Cat_JobTitle != null ? profile.Cat_JobTitle.Code : string.Empty;
                            //      dr["Rank"] = lstRankByProfileId != null ? lstRankByProfileId.Rank : string.Empty;
                            //   dr["Target"] = lstRevenueByProfileId != null ? lstRevenueByProfileId.Target : 0;
                            //     dr["Actual"] = lstRevenueByProfileId != null ? lstRevenueByProfileId.Actual : 0;

                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Email] = profile.Email != null ? profile.Email : string.Empty;
                            if (profile.DateHire != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                            if (profile.DateQuit != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                            if (profile.DateEndProbation != null)
                                dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateEndProbation] = profile.DateEndProbation.Value;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SocialInsNo] = profile.SocialInsNo != null ? profile.SocialInsNo : string.Empty;
                            dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PayrollGroup] = profile.Cat_PayrollGroup != null ? profile.Cat_PayrollGroup.PayrollGroupName : string.Empty;

                            if (profile.OrgStructureID != null)
                            {

                                var orgName = GetParentOrgNameForShiseido(listOrgAll, listOrgType, profile.OrgStructureID);
                                if (orgName.Count < 5)
                                {
                                    orgName.Insert(0, string.Empty);
                                    if (orgName.Count < 5)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }
                                    if (orgName.Count < 5)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }
                                    if (orgName.Count < 5)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }
                                    if (orgName.Count < 5)
                                    {
                                        orgName.Insert(0, string.Empty);
                                    }

                                }

                                dr["Công Ty"] = orgName[4];
                                dr["Chi Nhánh"] = orgName[3];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Channel] = orgName[2];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Region] = orgName[1];
                                dr[Hre_ReportHCSalesEntity.FieldNames.Area] = orgName[0];

                                var orgCode = GetParentOrgCodeForShiseido(listOrgAll, listOrgType, profile.OrgStructureID);
                                if (orgCode.Count < 5)
                                {
                                    orgCode.Insert(0, string.Empty);
                                    if (orgCode.Count < 5)
                                    {
                                        orgCode.Insert(0, string.Empty);
                                    }
                                    if (orgCode.Count < 5)
                                    {
                                        orgCode.Insert(0, string.Empty);
                                    }
                                    if (orgCode.Count < 5)
                                    {
                                        orgCode.Insert(0, string.Empty);
                                    }
                                    if (orgCode.Count < 5)
                                    {
                                        orgCode.Insert(0, string.Empty);
                                    }
                                }
                                dr["Mã Công Ty"] = orgCode[4];
                                dr["Mã Chi Nhánh"] = orgCode[3];
                                dr["Mã Channel"] = orgCode[2];
                                dr["Mã Region"] = orgCode[1];
                                dr["Mã Area"] = orgCode[0];

                                //var orgBranch = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_BRANCH, listOrgAll, listOrgType);
                                //var orgOrg = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_DEPARTMENT, listOrgAll, listOrgType);
                                //var orgTeam = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_TEAM, listOrgAll, listOrgType);
                                //var orgSection = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_SECTION, listOrgAll, listOrgType);
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            }

                            Hre_Contract hrcontract = lstHreContractAll.Where(hr => hr.ProfileID == profile.ID && hr.IsDelete == null).FirstOrDefault();
                            if (hrcontract != null && hrcontract.RankRateID != null)
                            {
                                Cat_SalaryRank salrank = lstSalaryRankAll.Where(rk => rk.ID == hrcontract.RankRateID.Value && rk.IsDelete == null).FirstOrDefault();
                                if (salrank != null)
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryRankName] = salrank.SalaryRankName;
                            }

                            #region Insurance, Bank
                            List<Sal_SalaryInformation> salaryInformationList = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();
                            if (salaryInformationList.Count > 0)
                            {
                                Sal_SalaryInformation bankSalary = salaryInformationList[0];
                                string accountTemp = string.Empty;
                                string accountNo = string.Empty;
                                string accountNo2 = string.Empty;
                                string bankNameTemp = string.Empty;
                                string bankCode = string.Empty;

                                accountTemp = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo = bankSalary.AccountNo == null ? string.Empty : bankSalary.AccountNo;
                                accountNo2 = bankSalary.AccountNo2 == null ? string.Empty : bankSalary.AccountNo2;
                                bankNameTemp = bankSalary.Cat_Bank == null ? string.Empty : bankSalary.Cat_Bank.BankName;
                                bankCode = bankSalary.Cat_Bank == null ? string.Empty : bankSalary.Cat_Bank.BankCode;

                                if (!string.IsNullOrEmpty(accountTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = accountTemp.Trim();
                                if (!string.IsNullOrEmpty(accountNo))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo] = accountNo.Trim();
                                if (!string.IsNullOrEmpty(accountNo2))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.AccountNo2] = accountNo2.Trim();
                                if (!string.IsNullOrEmpty(bankCode))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode] = bankCode.Trim();
                                if (!string.IsNullOrEmpty(bankNameTemp))
                                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = bankNameTemp.Trim();
                            }
                            #endregion

                            #region sal grade
                            //   Sal_Grade grade = lstGradeAll.Where(gr => gr.IsDelete == null && gr.ProfileID == profile.ID
                            //                                           && gr.MonthStart < to).OrderByDescending(gr => gr.MonthStart).FirstOrDefault();
                            // if (grade != null)
                            //  {
                            // Cat_GradePayroll gradepayroll = grade.Cat_GradePayroll1;
                            // if (gradepayroll != null && gradepayroll.IsFormulaSalary == true)
                            //   {
                            try
                            {
                                var lstSalPrItem = lstUnusualPayItem.Where(sal => sal.UnusualPayID == payroll.ID).ToList();
                                //  var lstelement = listPayrollElement.Where(pr => pr.GradePayrollID == gradepayroll.ID).ToList();
                                if (listPayrollElement != null && listPayrollElement.Count > 0)
                                {
                                    foreach (Cat_Element payrollElement in listPayrollElement)
                                    {
                                        if (payrollElement != null && !String.IsNullOrEmpty(payrollElement.ElementCode))
                                        {
                                            //Add phần tử vào cột dữ liệu nếu chưa có
                                            if (!tblData.Columns.Contains(payrollElement.ElementCode))
                                            {
                                                tblData.Columns.Add(payrollElement.ElementCode, typeof(Double));
                                            }
                                            //Lấy value của phần tử 
                                            if (tblData.Columns.Contains(payrollElement.ElementCode))
                                            {
                                                Sal_UnusualPayItem item = lstSalPrItem.Where(it => it.Element == payrollElement.ElementCode).FirstOrDefault();
                                                Double value = 0;
                                                if (item != null)
                                                {
                                                    //if (item.ValueType == typeof(Double).Name)
                                                    //  {
                                                    Double.TryParse(item.Amount.ToString(), out value);
                                                    // }
                                                }
                                                dr[payrollElement.ElementCode] = value;
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            // }
                            // }
                            #endregion

                            tblData.Rows.Add(dr);
                        }
                    }
                    #endregion
                    return tblData.ConfigTable(true);
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
        #endregion

        #region So Sánh Lương

        public DataTable GetSchemaComparePayroll(Guid[] _cutOffDurationIDs, Guid[] _elementIDs, string CompareType, string orderNumber, Guid? OrgTypeID, string ShowDataType, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Sal_ComparePayrollModel");
                var E_ORGSTRUCTURE = EnumDropDown.CompareType.E_ORGSTRUCTURE.ToString();
                var E_PROFILE = EnumDropDown.CompareType.E_PROFILE.ToString();
                var column = EnumDropDown.ShowDataType.E_COLUMN.ToString();
                var row = EnumDropDown.ShowDataType.E_ROW.ToString();

                #region Group Theo Nhân Viên
                if (CompareType == E_PROFILE || string.IsNullOrEmpty(CompareType))
                {
                    tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.CodeEmp, typeof(string));
                    tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.ProfileName, typeof(string));
                    tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.OrgStructureName, typeof(string));
                    var attCutOffDurationServices = new Att_CutOffDurationServices();
                    var lstObjCutOff = new List<object>();
                    lstObjCutOff.Add(null);
                    lstObjCutOff.Add(1);
                    lstObjCutOff.Add(int.MaxValue - 1);
                    var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                    lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();

                    var elementServices = new Cat_ElementServices();
                    var lstObjElement = new List<object>();
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(1);
                    lstObjElement.Add(int.MaxValue - 1);
                    var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                    lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();
                    foreach (var item in lstElement)
                    {
                        foreach (var cutOff in lstCutOffDuration)
                        {
                            var elementName = item.ElementName + "_" + (lstCutOffDuration.IndexOf(cutOff) + 1).ToString();
                            if (!tb.Columns.Contains(elementName))
                            {
                                tb.Columns.Add(elementName, typeof(double));
                            }
                        }
                    }
                }

                #endregion

                #region Group Theo Phòng Ban
                if (CompareType == E_ORGSTRUCTURE)
                {
                    if (ShowDataType == column)
                    {
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.OrgStructureCode, typeof(string));
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.OrgStructureName, typeof(string));
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.GroupNumberProfile, typeof(int));
                        var attCutOffDurationServices = new Att_CutOffDurationServices();
                        var lstObjCutOff = new List<object>();
                        lstObjCutOff.Add(null);
                        lstObjCutOff.Add(1);
                        lstObjCutOff.Add(int.MaxValue - 1);
                        var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                        lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();

                        var elementServices = new Cat_ElementServices();
                        var lstObjElement = new List<object>();
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(1);
                        lstObjElement.Add(int.MaxValue - 1);
                        var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                        lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();
                        foreach (var item in lstElement)
                        {
                            foreach (var cutOff in lstCutOffDuration)
                            {
                                var elementName = item.ElementName + "_" + (lstCutOffDuration.IndexOf(cutOff) + 1).ToString();
                                if (!tb.Columns.Contains(elementName))
                                {
                                    tb.Columns.Add(elementName, typeof(double));
                                }
                            }
                        }
                    }
                    if (ShowDataType == row)
                    {
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.OrgStructureCode, typeof(string));
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.OrgStructureName, typeof(string));
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.ElementName, typeof(string));
                        tb.Columns.Add(Sal_ComparePayrollEntity.FieldNames.GroupNumberProfile, typeof(int));
                        var attCutOffDurationServices = new Att_CutOffDurationServices();
                        var lstObjCutOff = new List<object>();
                        lstObjCutOff.Add(null);
                        lstObjCutOff.Add(1);
                        lstObjCutOff.Add(int.MaxValue - 1);
                        var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                        lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();
                        foreach (var item in lstCutOffDuration)
                        {
                            if (!tb.Columns.Contains(item.CutOffDurationName))
                            {
                                tb.Columns.Add(item.CutOffDurationName, typeof(double));
                            }
                        }
                    }

                }
                #endregion

                return tb;
            }
        }

        public DataTable ComparePayroll(Guid[] _cutOffDurationIDs, Guid[] _elementIDs, string OrgOrderNumber, Guid? OrgTypeID, bool isCreateTemplate, string CompareType, string ShowDataType, Guid[] WorkingPlace, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable table = GetSchemaComparePayroll(_cutOffDurationIDs, _elementIDs, CompareType, OrgOrderNumber, OrgTypeID, ShowDataType, UserLogin);
                var E_ORGSTRUCTURE = EnumDropDown.CompareType.E_ORGSTRUCTURE.ToString();
                var E_PROFILE = EnumDropDown.CompareType.E_PROFILE.ToString();
                var column = EnumDropDown.ShowDataType.E_COLUMN.ToString();
                var row = EnumDropDown.ShowDataType.E_ROW.ToString();

                if (isCreateTemplate)
                {
                    return table.ConfigTable();
                }
                #region Group Theo Nhân Viên
                if (CompareType == E_PROFILE || string.IsNullOrEmpty(CompareType))
                {
                    // Xử Lý Group Phòng Ban
                    var orgsService = new Cat_OrgStructureServices();
                    var lstObjOrgByOrderNumber = new List<object>();
                    lstObjOrgByOrderNumber.Add(OrgOrderNumber);
                    var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => new { s.ID, s.OrgStructureTypeID, s.OrderNumber }).ToList();
                    if (OrgTypeID != null)
                    {
                        lstOrgByOrderNumber = lstOrgByOrderNumber.Where(s => s.OrgStructureTypeID.Value == OrgTypeID.Value).ToList();
                    }
                    var strOrderNumer = string.Empty;
                    var OrderNumber = string.Empty;
                    OrderNumber = null;
                    foreach (var OrgOrderNumer in lstOrgByOrderNumber)
                    {
                        strOrderNumer += OrgOrderNumer.OrderNumber + ",";
                    }
                    if (!string.IsNullOrEmpty(strOrderNumer))
                    {
                        OrderNumber = strOrderNumer.Substring(0, strOrderNumer.Length - 1);
                    }


                    var profileServices = new Hre_ProfileServices();
                    var lstObjProfile = new List<object>();
                    lstObjProfile.Add(OrderNumber);
                    lstObjProfile.Add(null);
                    lstObjProfile.Add(null);
                    var listProfile = profileServices.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                    if (WorkingPlace != null)
                    {
                        listProfile = listProfile.Where(m => m.WorkPlaceID != null && WorkingPlace.Contains((Guid)m.WorkPlaceID)).ToList();
                    }

                    var attCutOffDurationServices = new Att_CutOffDurationServices();
                    var lstObjCutOff = new List<object>();
                    lstObjCutOff.Add(null);
                    lstObjCutOff.Add(1);
                    lstObjCutOff.Add(int.MaxValue - 1);
                    var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                    lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();
                    var lstCutOffDurationID = lstCutOffDuration.Select(s => s.ID).ToList();

                    var elementServices = new Cat_ElementServices();
                    var lstObjElement = new List<object>();
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(null);
                    lstObjElement.Add(1);
                    lstObjElement.Add(int.MaxValue - 1);
                    var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                    lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();

                    var payrollTableServices = new Sal_PayrollTableServices();
                    var lstObjPayrollTable = new List<object>();
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(null);
                    lstObjPayrollTable.Add(1);
                    lstObjPayrollTable.Add(int.MaxValue - 1);
                    var lstPayrollTable = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status).ToList();

                    var payrollTableItemServices = new Sal_PayrollTableItemServices();
                    var lstObjPayrollTableItem = new List<object>();
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(null);
                    lstObjPayrollTableItem.Add(1);
                    lstObjPayrollTableItem.Add(int.MaxValue - 1);
                    var lstPayrollTableItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).ToList();

                    foreach (var profile in listProfile)
                    {
                        DataRow dr = table.NewRow();
                        dr[Sal_ComparePayrollEntity.FieldNames.CodeEmp] = profile != null ? profile.CodeEmp : string.Empty;
                        dr[Sal_ComparePayrollEntity.FieldNames.ProfileName] = profile != null ? profile.ProfileName : string.Empty;
                        dr[Sal_ComparePayrollEntity.FieldNames.OrgStructureName] = profile != null ? profile.OrgStructureName : string.Empty;
                        foreach (var element in lstElement)
                        {
                            foreach (var cutOff in lstCutOffDuration)
                            {
                                var lstPayrollTableByProfileID = lstPayrollTable.Where(s => s.ProfileID == profile.ID && cutOff.ID == s.CutOffDurationID.Value).ToList();
                                foreach (var payrollTable in lstPayrollTableByProfileID)
                                {
                                    var payrollTableItemData = lstPayrollTableItem.Where(s => s.PayrollTableID == payrollTable.ID).ToList();
                                    foreach (var payrollTableItem in payrollTableItemData)
                                    {
                                        var elementData = payrollTableItemData.Where(s => element.ElementCode == s.Code).FirstOrDefault();
                                        double value = 0;
                                        var elementName = element.ElementName + "_" + (lstCutOffDuration.IndexOf(cutOff) + 1).ToString();
                                        if (table.Columns.Contains(elementName))
                                        {
                                            if (elementData != null)
                                            {
                                                if (elementData.ValueType == typeof(Double).Name)
                                                {
                                                    Double.TryParse(elementData.Value, out value);
                                                }
                                            }
                                            dr[elementName] = value;
                                        }
                                    }
                                }
                            }
                        }
                        table.Rows.Add(dr);
                    }
                }
                #endregion


                #region Group Theo Phòng Ban
                if (CompareType == E_ORGSTRUCTURE)
                {
                    #region Show dữ liệu theo dạng cột
                    if (ShowDataType == column)
                    {
                        // Xử Lý Group Phòng Ban
                        var orgsService = new Cat_OrgStructureServices();
                        var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, UserLogin, ref status).ToList();
                        var lstObjOrgByOrderNumber = new List<object>();
                        lstObjOrgByOrderNumber.Add(OrgOrderNumber);
                        var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => new { s.ID, s.OrgStructureTypeID, s.OrderNumber, s.OrgStructureName, s.Code }).ToList();
                        if (OrgTypeID != null)
                        {
                            lstOrgByOrderNumber = lstOrgByOrderNumber.Where(s => s.OrgStructureTypeID.Value == OrgTypeID.Value).ToList();
                        }

                        var profileServices = new Hre_ProfileServices();
                        var lstObjProfile = new List<object>();
                        lstObjProfile.AddRange(new object[18]);
                        lstObjProfile[16] = 1;
                        lstObjProfile[17] = int.MaxValue - 1;
                        var listProfile = profileServices.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

                        if (WorkingPlace != null)
                        {
                            listProfile = listProfile.Where(m => m.WorkPlaceID != null && WorkingPlace.Contains((Guid)m.WorkPlaceID)).ToList();
                        }

                        var attCutOffDurationServices = new Att_CutOffDurationServices();
                        var lstObjCutOff = new List<object>();
                        lstObjCutOff.Add(null);
                        lstObjCutOff.Add(1);
                        lstObjCutOff.Add(int.MaxValue - 1);
                        var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                        lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();
                        var lstCutOffDurationID = lstCutOffDuration.Select(s => s.ID).ToList();

                        var elementServices = new Cat_ElementServices();
                        var lstObjElement = new List<object>();
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(1);
                        lstObjElement.Add(int.MaxValue - 1);
                        var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                        lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();

                        var payrollTableServices = new Sal_PayrollTableServices();
                        var lstObjPayrollTable = new List<object>();
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(1);
                        lstObjPayrollTable.Add(int.MaxValue - 1);
                        var lstPayrollTable = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status).ToList();

                        var payrollTableItemServices = new Sal_PayrollTableItemServices();
                        var lstObjPayrollTableItem = new List<object>();
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(1);
                        lstObjPayrollTableItem.Add(int.MaxValue - 1);
                        var lstPayrollTableItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).ToList();

                        foreach (var org in lstOrgByOrderNumber)
                        {
                            DataRow dr = table.NewRow();
                            double total = 0;
                            //xử lý đếm nhân viên của phòng ban con
                            orderNumber = string.Empty;
                            orderNumber += org.OrderNumber.ToString() + ",";
                            getChildOrgStructure(lstallorgs, org.ID);
                            // orderNumber += "," + org.OrderNumber.ToString();
                            if (orderNumber.IndexOf(',') > 0)
                                orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);



                            var lstObjProfileByOrgOrderNumer = new List<object>();
                            lstObjProfileByOrgOrderNumer.Add(orderNumber);
                            lstObjProfileByOrgOrderNumer.Add(null);
                            lstObjProfileByOrgOrderNumer.Add(null);
                            var listProfileByOrderNumber = profileServices.GetData<Hre_ProfileEntity>(lstObjProfileByOrgOrderNumer, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).Select(s => s.ID).ToList();

                            dr[Sal_ComparePayrollEntity.FieldNames.OrgStructureCode] = org != null ? org.Code : string.Empty;
                            dr[Sal_ComparePayrollEntity.FieldNames.OrgStructureName] = org != null ? org.OrgStructureName : string.Empty;
                            dr[Sal_ComparePayrollEntity.FieldNames.GroupNumberProfile] = listProfile.Where(m => m.OrgStructureID == org.ID).Count();
                            var lstProfileByOrgID = listProfile.Where(s => listProfileByOrderNumber.Contains(s.ID)).ToList();

                            foreach (var profile in lstProfileByOrgID)
                            {

                                foreach (var element in lstElement)
                                {
                                    foreach (var cutOff in lstCutOffDuration)
                                    {
                                        var lstPayrollTableByProfileID = lstPayrollTable.Where(s => s.ProfileID == profile.ID && cutOff.ID == s.CutOffDurationID.Value).ToList();
                                        foreach (var payrollTable in lstPayrollTableByProfileID)
                                        {
                                            var payrollTableItemData = lstPayrollTableItem.Where(s => s.PayrollTableID == payrollTable.ID).ToList();
                                            foreach (var payrollTableItem in payrollTableItemData)
                                            {
                                                var elementData = payrollTableItemData.Where(s => element.ElementCode == s.Code).FirstOrDefault();
                                                double value = 0;
                                                var elementName = element.ElementName + "_" + (lstCutOffDuration.IndexOf(cutOff) + 1).ToString();
                                                if (table.Columns.Contains(elementName))
                                                {
                                                    if (elementData != null)
                                                    {
                                                        if (elementData.ValueType == typeof(Double).Name)
                                                        {
                                                            Double.TryParse(elementData.Value, out value);
                                                        }
                                                    }
                                                    total += value;
                                                    dr[elementName] = total;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            table.Rows.Add(dr);
                        }
                    }
                    #endregion

                    #region Show dữ liệu theo dạng Dòng
                    if (ShowDataType == row)
                    {
                        // Xử Lý Group Phòng Ban
                        var orgsService = new Cat_OrgStructureServices();
                        var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, UserLogin, ref status).ToList();
                        var lstObjOrgByOrderNumber = new List<object>();
                        lstObjOrgByOrderNumber.Add(OrgOrderNumber);
                        var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => new { s.ID, s.OrgStructureTypeID, s.OrderNumber, s.OrgStructureName, s.Code }).ToList();
                        if (OrgTypeID != null)
                        {
                            lstOrgByOrderNumber = lstOrgByOrderNumber.Where(s => s.OrgStructureTypeID.Value == OrgTypeID.Value).ToList();
                        }

                        var profileServices = new Hre_ProfileServices();
                        var lstObjProfile = new List<object>();
                        lstObjProfile.AddRange(new object[18]);
                        lstObjProfile[16] = 1;
                        lstObjProfile[17] = int.MaxValue - 1;
                        var listProfile = profileServices.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

                        var attCutOffDurationServices = new Att_CutOffDurationServices();
                        var lstObjCutOff = new List<object>();
                        lstObjCutOff.Add(null);
                        lstObjCutOff.Add(1);
                        lstObjCutOff.Add(int.MaxValue - 1);
                        var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                        lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();
                        var lstCutOffDurationID = lstCutOffDuration.Select(s => s.ID).ToList();

                        var elementServices = new Cat_ElementServices();
                        var lstObjElement = new List<object>();
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(null);
                        lstObjElement.Add(1);
                        lstObjElement.Add(int.MaxValue - 1);
                        var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                        lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();

                        var payrollTableServices = new Sal_PayrollTableServices();
                        var lstObjPayrollTable = new List<object>();
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(null);
                        lstObjPayrollTable.Add(1);
                        lstObjPayrollTable.Add(int.MaxValue - 1);
                        var lstPayrollTable = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status).ToList();

                        var payrollTableItemServices = new Sal_PayrollTableItemServices();
                        var lstObjPayrollTableItem = new List<object>();
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(null);
                        lstObjPayrollTableItem.Add(1);
                        lstObjPayrollTableItem.Add(int.MaxValue - 1);
                        var lstPayrollTableItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).ToList();

                        foreach (var org in lstOrgByOrderNumber)
                        {
                            DataRow dr = table.NewRow();

                            //xử lý đếm nhân viên của phòng ban con
                            orderNumber = string.Empty;
                            orderNumber += org.OrderNumber.ToString() + ",";
                            getChildOrgStructure(lstallorgs, org.ID);
                            // orderNumber += "," + org.OrderNumber.ToString();
                            if (orderNumber.IndexOf(',') > 0)
                                orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);



                            var lstObjProfileByOrgOrderNumer = new List<object>();
                            lstObjProfileByOrgOrderNumer.Add(orderNumber);
                            lstObjProfileByOrgOrderNumer.Add(null);
                            lstObjProfileByOrgOrderNumer.Add(null);
                            var listProfileByOrderNumber = profileServices.GetData<Hre_ProfileEntity>(lstObjProfileByOrgOrderNumer, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).Select(s => s.ID).ToList();

                            dr[Sal_ComparePayrollEntity.FieldNames.OrgStructureCode] = org != null ? org.Code : string.Empty;
                            dr[Sal_ComparePayrollEntity.FieldNames.OrgStructureName] = org != null ? org.OrgStructureName : string.Empty;
                            dr[Sal_ComparePayrollEntity.FieldNames.GroupNumberProfile] = listProfile.Where(m => m.OrgStructureID == org.ID).Count();
                            table.Rows.Add(dr);
                            var lstProfileByOrgID = listProfile.Where(s => listProfileByOrderNumber.Contains(s.ID)).ToList();
                            var lstProfileID = lstProfileByOrgID.Select(s => s.ID).ToList();

                            foreach (var element in lstElement)
                            {
                                DataRow dr1 = table.NewRow();
                                double total = 0;
                                dr1[Sal_ComparePayrollEntity.FieldNames.ElementName] = element.ElementCode;
                                foreach (var cutOff in lstCutOffDuration)
                                {
                                    var lstPayrollTableByProfileID = lstPayrollTable.Where(s => lstProfileID.Contains(s.ProfileID) && cutOff.ID == s.CutOffDurationID.Value).ToList();
                                    foreach (var payrollTable in lstPayrollTableByProfileID)
                                    {
                                        var payrollTableItemData = lstPayrollTableItem.Where(s => s.PayrollTableID == payrollTable.ID && element.ElementCode == s.Code).ToList();
                                        foreach (var payrollTableItem in payrollTableItemData)
                                        {

                                            double value = 0;
                                            var elementName = element.ElementName + "_" + (lstCutOffDuration.IndexOf(cutOff) + 1).ToString();
                                            if (table.Columns.Contains(cutOff.CutOffDurationName))
                                            {

                                                if (payrollTableItem.ValueType == typeof(Double).Name)
                                                {
                                                    Double.TryParse(payrollTableItem.Value, out value);
                                                }

                                                total += value;
                                                dr1[cutOff.CutOffDurationName] = total;
                                            }

                                        }
                                    }
                                }
                                if (total > 0)
                                {
                                    table.Rows.Add(dr1);
                                }

                            }
                        }
                    }
                    #endregion

                }
                #endregion

                //Dictionary<string, Dictionary<string, object>> configs = new Dictionary<string, Dictionary<string, object>>();
                //Dictionary<string, object> config = new Dictionary<string, object>();
                //config.Add("locked", true);
                //configs.Add("CodeEmp", config);
                //configs.Add("ProfileName", config);
                //configs.Add("OrgStructureCode", config);
                //configs.Add("OrgStructureName", config);
                return table.ConfigTable(true);
            }
        }

        #endregion

        #region RptCostCentreByOrg
        public DataTable GetSchemaRptCostCentreByOrg(Guid[] _elementIDs, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Sal_ReportCostCentreByOrgEntity");
                tb.Columns.Add(Sal_ReportCostCentreByOrgEntity.FieldNames.CostCentreCode);
                tb.Columns.Add(Sal_ReportCostCentreByOrgEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Sal_ReportCostCentreByOrgEntity.FieldNames.OrgStructureName);

                var elementServices = new Cat_ElementServices();
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();
                foreach (var item in lstElement)
                {
                    if (!tb.Columns.Contains(item.ElementName))
                    {
                        tb.Columns.Add(item.ElementName, typeof(double));
                    }
                }
                return tb;
            }
        }

        public DataTable GetReportCostCentreByOrg(Guid[] _cutOffDurationIDs, Guid[] _elementIDs, string OrgOrderNumber, string UserLogin, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var baservice = new BaseService();
                DataTable table = GetSchemaRptCostCentreByOrg(_elementIDs, UserLogin);

                if (isCreateTemplate)
                {
                    return table.ConfigTable();
                }

                // Xử Lý Group Phòng Ban
                var orgsService = new Cat_OrgStructureServices();
                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(OrgOrderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructureEntity>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => new { s.ID, s.OrgStructureTypeID, s.OrderNumber, s.OrgStructureName, s.GroupCostCentreID, s.Code }).ToList();

                var strOrderNumer = string.Empty;
                var OrderNumber = string.Empty;
                OrderNumber = null;
                foreach (var OrgOrderNumer in lstOrgByOrderNumber)
                {
                    strOrderNumer += OrgOrderNumer.OrderNumber + ",";
                }
                if (!string.IsNullOrEmpty(strOrderNumer))
                {
                    OrderNumber = strOrderNumer.Substring(0, strOrderNumer.Length - 1);
                }

                var profileServices = new Hre_ProfileServices();
                var lstObjProfile = new List<object>();
                lstObjProfile.Add(OrderNumber);
                lstObjProfile.Add(null);
                lstObjProfile.Add(null);
                var listProfile = profileServices.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                var lstProfileIDs = listProfile.Select(s => s.ID).ToList();


                List<object> lstObjCostCentreSal = new List<object>();
                lstObjCostCentreSal.AddRange(new object[4]);
                lstObjCostCentreSal[0] = null;
                lstObjCostCentreSal[1] = null;
                lstObjCostCentreSal[2] = 1;
                lstObjCostCentreSal[3] = int.MaxValue - 1;
                var lstSal_CostCentreSal = baservice.GetData<Sal_CostCentreSalEntity>(lstObjCostCentreSal, ConstantSql.hrm_sal_sp_getdata_CostCentreSal, UserLogin, ref status).ToList();

                var lstObjCostCentre = new List<object>();
                lstObjCostCentre.AddRange(new object[4]);
                lstObjCostCentre[0] = null;
                lstObjCostCentre[1] = null;
                lstObjCostCentre[2] = 1;
                lstObjCostCentre[3] = int.MaxValue - 1;
                var lstCostCentre = baservice.GetData<Cat_CostCentreEntity>(lstObjCostCentreSal, ConstantSql.hrm_cat_sp_get_CostCentre, UserLogin, ref status).ToList();
                var lstCostCentreIDs = lstCostCentre.Select(s => s.ID).ToList();

                var payrollTableServices = new Sal_PayrollTableServices();
                var lstObjPayrollTable = new List<object>();
                lstObjPayrollTable.Add(null);
                lstObjPayrollTable.Add(null);
                lstObjPayrollTable.Add(null);
                lstObjPayrollTable.Add(null);
                lstObjPayrollTable.Add(1);
                lstObjPayrollTable.Add(int.MaxValue - 1);
                var lstPayrollTable = payrollTableServices.GetData<Sal_PayrollTableEntity>(lstObjPayrollTable, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status).ToList();

                var payrollTableItemServices = new Sal_PayrollTableItemServices();
                var lstObjPayrollTableItem = new List<object>();
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(null);
                lstObjPayrollTableItem.Add(1);
                lstObjPayrollTableItem.Add(int.MaxValue - 1);
                var lstPayrollTableItem = payrollTableItemServices.GetData<Sal_PayrollTableItemEntity>(lstObjPayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).ToList();

                var elementServices = new Cat_ElementServices();
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                var lstElement = elementServices.GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).ToList();
                lstElement = lstElement.Where(s => _elementIDs.Contains(s.ID)).ToList();
                foreach (var cost in lstCostCentre)
                {
                    //      var lstOrgByCostcentre = lstOrgByOrderNumber.Where(s => s.GroupCostCentreID.Value == cost.ID).ToList();
                    foreach (var org in lstOrgByOrderNumber)
                    {
                        DataRow dr = table.NewRow();
                        double? sumProfile = 0;
                        var lstProfileByOrg = listProfile.Where(s => s.OrgStructureID.Value == org.ID).ToList();
                        dr[Sal_ReportCostCentreByOrgEntity.FieldNames.CostCentreCode] = cost == null ? string.Empty : cost.Code;
                        dr[Sal_ReportCostCentreByOrgEntity.FieldNames.OrgStructureCode] = org == null ? string.Empty : org.Code;
                        dr[Sal_ReportCostCentreByOrgEntity.FieldNames.OrgStructureName] = org == null ? string.Empty : org.OrgStructureName;

                        foreach (var profile in lstProfileByOrg)
                        {
                            var lstPayrollTableByProfileID = lstPayrollTable.Where(s => profile.ID == s.ProfileID && s.CutOffDurationID.Value == _cutOffDurationIDs[0]).ToList();
                            foreach (var payrollTable in lstPayrollTableByProfileID)
                            {
                                var lstPayrollTableItemByTableID = lstPayrollTableItem.Where(s => s.PayrollTableID == payrollTable.ID).ToList();
                                foreach (var item in lstPayrollTableItemByTableID)
                                {
                                    var costcentreByProfile = lstSal_CostCentreSal.Where(s => s.CostCentreID.Value == cost.ID && payrollTable.ProfileID == s.ProfileID.Value && item.Code == s.ElementType).ToList();

                                    foreach (var costcentre in costcentreByProfile)
                                    {
                                        foreach (var element in lstElement)
                                        {
                                            Double value = 0;
                                            if (costcentre.ElementType == element.ElementCode)
                                            {
                                                if (table.Columns.Contains(element.ElementName))
                                                {
                                                    Double.TryParse(item.Value, out value);
                                                    sumProfile += value * costcentre.Rate;
                                                    dr[element.ElementName] = sumProfile;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        table.Rows.Add(dr);
                    }
                }

                return table.ConfigTable(true);
            }
        }

        #endregion

        #region BC Hoa Hồng Cho Cửa Hàng

        public DataTable GetSchemaReportVenueForShop(string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Sal_ReportRevenueForShopsModel");
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.ShopCode, typeof(string));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.ShopName, typeof(string));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Rank, typeof(string));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Target, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Actual, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.CompletionRate, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Total, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Shiftleader, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Commission, typeof(double));

                var kpiBonusService = new Cat_KPIBonusServices();
                var lstObj = new List<object>();
                lstObj.Add(null);
                lstObj.Add(null);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue - 1);
                var lstKPIBonus = kpiBonusService.GetData<Cat_KPIBonusEntity>(lstObj, ConstantSql.hrm_cat_sp_get_KPIBonus, UserLogin, ref status).ToList();
                if (lstKPIBonus != null)
                {
                    foreach (var item in lstKPIBonus)
                    {
                        var CommissionName = Sal_ReportRevenueForShopsEntity.FieldNames.Commission + item.KPIBonusName;
                        if (!tb.Columns.Contains(item.KPIBonusName) || !tb.Columns.Contains(CommissionName))
                        {
                            tb.Columns.Add(item.KPIBonusName, typeof(double));
                            tb.Columns.Add(CommissionName, typeof(double));
                        }
                    }
                }
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.TopLine5Forcus, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.CommissionTopLine5Forcus, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.ProductForcus, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.CommissionProductForcus, typeof(double));
                tb.Columns.Add(Sal_ReportRevenueForShopsEntity.FieldNames.Incentive, typeof(double));

                return tb;
            }
        }
        public DataTable GetReportRevenueForShop(DateTime dateFrom, DateTime dateTo, string orderNumber, string UserLogin, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable table = GetSchemaReportVenueForShop(UserLogin);
                if (isCreateTemplate)
                {
                    return table.ConfigTable();
                }

                var orgServices = new Cat_OrgStructureServices();
                var lstObjOrg = new List<object>();
                lstObjOrg.Add(orderNumber);
                var lstOrgID = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).Select(s => s.ID).ToList();



                var shopServices = new Cat_ShopServices();
                var lstObjShop = new List<object>();
                lstObjShop.Add(null);
                lstObjShop.Add(null);
                lstObjShop.Add(null);
                lstObjShop.Add(1);
                lstObjShop.Add(int.MaxValue - 1);
                var lstShop = shopServices.GetData<Cat_ShopEntity>(lstObjShop, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status).ToList();
                var lstShopID = new List<Guid>();
                if (lstOrgID != null)
                {
                    lstShop = lstShop.Where(s => lstOrgID.Contains(s.OrgStructureID.Value)).ToList();
                }
                var kpiBonusService = new Cat_KPIBonusServices();
                var lstObjKPIBonus = new List<object>();
                lstObjKPIBonus.Add(null);
                lstObjKPIBonus.Add(null);
                lstObjKPIBonus.Add(1);
                lstObjKPIBonus.Add(int.MaxValue - 1);
                var lstKPIBonus = kpiBonusService.GetData<Cat_KPIBonusEntity>(lstObjKPIBonus, ConstantSql.hrm_cat_sp_get_KPIBonus, UserLogin, ref status).ToList();

                var kpiBonusItemServices = new Cat_KPIBonusItemServices();
                var lstObjKPIBonusItem = new List<object>();
                lstObjKPIBonusItem.Add(null);
                lstObjKPIBonusItem.Add(null);
                lstObjKPIBonusItem.Add(1);
                lstObjKPIBonusItem.Add(int.MaxValue - 1);
                var lstKPIBonusItem = kpiBonusItemServices.GetData<Cat_KPIBonusItemEntity>(lstObjKPIBonusItem, ConstantSql.hrm_cat_sp_get_KPIBonusItem, UserLogin, ref status).ToList();

                var revenueForRecordServices = new Sal_RevenueRecordService();
                var lstObjRevenueRecord = new List<object>();
                lstObjRevenueRecord.Add(null);
                lstObjRevenueRecord.Add(null);
                lstObjRevenueRecord.Add(1);
                lstObjRevenueRecord.Add(int.MaxValue - 1);
                var lstRevenueRecord = revenueForRecordServices.GetData<Sal_RevenueRecordEntity>(lstObjRevenueRecord, ConstantSql.hrm_sal_sp_get_RevenueRecord, UserLogin, ref status).Where(s => s.Month.Value.Month == dateFrom.Month && s.Month.Value.Year == dateFrom.Year && s.Month.Value.Month == dateFrom.Month && s.Month.Value.Year == dateFrom.Year).ToList();

                var revenueForShopServices = new Sal_RevenueForShopServices();
                var lstObjRevenueForShop = new List<object>();
                lstObjRevenueForShop.Add(null);
                lstObjRevenueForShop.Add(null);
                lstObjRevenueForShop.Add(null);
                lstObjRevenueForShop.Add(null);
                lstObjRevenueForShop.Add(1);
                lstObjRevenueForShop.Add(int.MaxValue - 1);
                var lstRevenueForShop = revenueForRecordServices.GetData<Sal_RevenueForShopEntity>(lstObjRevenueForShop, ConstantSql.hrm_sal_sp_get_RevenueForShop, UserLogin, ref status).ToList();

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[18]);
                listModel[16] = 1;
                listModel[17] = int.MaxValue - 1;
                List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();


                List<ElementFormula> listElementFormula = new List<ElementFormula>();

                foreach (var item in lstShop)
                {
                    #region Lấy các phần tử là Enum
                    Sal_RevenueForShopEntity RevenueForShopItem = new Sal_RevenueForShopEntity();
                    RevenueForShopItem = lstRevenueForShop.Where(m => m.ShopID == item.ID && m.KPIBonusID == lstKPIBonus.Where(t => t.IsTotalRevenue == true).FirstOrDefault().ID && m.DateFrom <= dateTo && m.DateTo >= dateFrom).FirstOrDefault();

                    if (RevenueForShopItem != null)
                    {
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_TAGET_SHOP.ToString(), RevenueForShopItem.Target, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), RevenueForShopItem.Actual, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PRECENT_REVENUE.ToString(), RevenueForShopItem.Actual / RevenueForShopItem.Target, 0));
                    }
                    else
                    {
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_TAGET_SHOP.ToString(), 0, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), 0, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PRECENT_REVENUE.ToString(), 0, 0));
                    }

                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_COUNT_SHOPMEMBER.ToString(), listProfile.Where(m => m.ShopID == item.ID).Count(), 0));


                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_COUNT_SL.ToString(), lstShop.Where(m => m.ID == item.ID).FirstOrDefault().NoShiftLeader, 0));


                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_RANK.ToString(), lstShop.Where(m => m.ID == item.ID).FirstOrDefault().Rank, 0));

                    #endregion




                    DataRow dr = table.NewRow();
                    var lstRevenueForShopByID = lstRevenueForShop.Where(s => s.ShopID == item.ID && lstKPIBonus.Where(m => m.IsTotalRevenue == true).FirstOrDefault().ID == s.KPIBonusID).FirstOrDefault();

                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.ShopCode] = item != null ? item.Code : string.Empty;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.ShopName] = item != null ? item.ShopName : string.Empty;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Rank] = item != null ? item.Rank : string.Empty;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Target] = lstRevenueForShopByID != null ? lstRevenueForShopByID.Target : 0;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Actual] = lstRevenueForShopByID != null ? lstRevenueForShopByID.Actual : 0;

                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.CompletionRate] = lstRevenueForShopByID != null ? lstRevenueForShopByID.Actual.Value / lstRevenueForShopByID.Target.Value : 0;

                    double totalPrice = 0;
                    foreach (var kpi in lstKPIBonus)
                    {
                        var CommissionName = Sal_ReportRevenueForShopsEntity.FieldNames.Commission + kpi.KPIBonusName;
                        var lstKpiBonusByID = lstKPIBonusItem.Where(s => s.ShopID == item.ID && kpi.ID == s.KPIBonusID).FirstOrDefault();
                        var lstRevenueRecordByID = lstRevenueRecord.Where(s => s.ShopID.Value == item.ID && s.KPIBonusID == kpi.ID).FirstOrDefault();
                        if (table.Columns.Contains(kpi.KPIBonusName))
                        {
                            dr[kpi.KPIBonusName] = lstKpiBonusByID != null ? lstKpiBonusByID.Value : 0;
                        }
                        if (table.Columns.Contains(CommissionName))
                        {
                            dr[CommissionName] = lstRevenueRecordByID != null ? lstRevenueRecordByID.Amount * 1 : 0;
                            totalPrice += lstRevenueRecordByID != null ? (double)lstRevenueRecordByID.Amount : 0;
                        }
                    }
                    var lstRevenueRecordByItem = lstRevenueRecord.Where(s => s.KPIBonusID == null && s.Type == EnumDropDown.SalesType.E_ITEM_MAJOR.ToString() && s.ShopID == item.ID).FirstOrDefault();
                    var lstRevenueRecordByLineItem = lstRevenueRecord.Where(s => s.KPIBonusID == null && s.Type == EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString() && s.ShopID == item.ID).FirstOrDefault();
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.TopLine5Forcus] = item != null ? item.MainLineProduct : 0;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.CommissionTopLine5Forcus] = lstRevenueRecordByItem != null ?
                        lstRevenueRecordByItem.Amount : 0;
                    totalPrice += lstRevenueRecordByItem != null ? (double)lstRevenueRecordByItem.Amount : 0;

                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.ProductForcus] = item != null ? item.PromoteProduct : 0;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.CommissionProductForcus] = lstRevenueRecordByLineItem != null ? lstRevenueRecordByLineItem.Amount : 0;
                    totalPrice += lstRevenueRecordByLineItem != null ? (double)lstRevenueRecordByLineItem.Amount : 0;

                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Commission] = totalPrice;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Incentive] = totalPrice;
                    double FriceSL = (double)GetObjectValue(new List<Cat_ElementEntity>(), listElementFormula, item.Formular1).Value;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Shiftleader] = FriceSL;
                    dr[Sal_ReportRevenueForShopsEntity.FieldNames.Total] = totalPrice + FriceSL;

                    table.Rows.Add(dr);

                }
                return table.ConfigTable(true);
            }
        }


        /// <summary>
        /// [Hien.Nguyen] Hàm tính value cho công tức
        /// </summary>
        /// <param name="ListElement">List các Element đã tính được trước đó</param>
        /// <param name="formula">Công thức</param>
        /// <returns>Giá trị tính được từ công thức</returns>
        public HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel GetObjectValue(List<Cat_ElementEntity> listElement, List<ElementFormula> ListElementFormula, string formula)
        {
            try
            {
                formula = formula.Replace("\n", "").Replace("\t", "").Trim();
                //Các phần tử tính lương tách ra từ 1 chuỗi công thức
                var ListFormula = ParseFormulaToList(formula).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();

                //Các phần tử tính lương chưa có kết quả
                var ListFormulaNotValue = ListFormula.Where(m => !ListElementFormula.Any(t => t.VariableName == m.Replace("[", "").Replace("]", ""))).ToList();

                //Nếu có tồn tại phần tử chưa có Value, thì đi tính value cho phần tử đó
                if (ListFormulaNotValue != null && ListFormulaNotValue.Count > 0)
                {
                    foreach (var i in ListFormulaNotValue)
                    {
                        if (!ListElementFormula.Any(m => m.VariableName == i.Replace("[", "").Replace("]", "")))
                        {
                            //string ttt = "";
                            //foreach (var j in listElement)
                            //{
                            //    if (j.ElementCode == i.Replace("[", "").Replace("]", ""))
                            //        ttt = j.Formula;
                            //}
                            ElementFormula item = new ElementFormula(i.Replace("[", "").Replace("]", ""), GetObjectValue(listElement, ListElementFormula, listElement.Where(m => m.ElementCode == i.Replace("[", "").Replace("]", "")).FirstOrDefault().Formula), 0);
                            ListElementFormula.Add(item);
                        }
                    }
                }
                //Do mệnh đề if luôn trả về false nên xử lý riêng cho mệnh đề if ở đây
                //nguyên ngân là do dll CalcEngine, nhưng chưa tìm ra cách giải quyết
                if (formula.ToUpper().Contains("IF("))
                {
                    foreach (var i in ListElementFormula.Distinct().ToList())
                    {
                        if (formula.Contains("[" + i.VariableName + "]"))
                        {
                            formula = formula.Replace("[" + i.VariableName + "]", i.Value.ToString());
                        }
                    }
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }
                else
                {
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Tách công thức thành List các phần tử để kiểm tra
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public List<string> ParseFormulaToList(string formula)
        {
            formula = formula.Replace("[", " [").Replace("]", "] ");
            return formula.Split(' ').Where(m => m.StartsWith("[") && m.EndsWith("]")).Distinct().ToList();


            //formula = formula.Replace(" ", "").Replace("+", " ").Replace("-", " ").Replace("*", " ").Replace("/", " ").Replace("(", " ").Replace(")", "");
            //return formula.Split(' ').ToList();
        }
        #endregion

        #region BC Lương Tổng Hợp
        public DataTable CreateReportSalaryMonthlySchema()
        {
            DataTable tb = new DataTable("Sal_ReportSalaryMonthlyModel");
            tb.Columns.Add(Sal_ReportSalaryMonthlyEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Sal_ReportSalaryMonthlyEntity.FieldNames.CostCenterCode);
            tb.Columns.Add(Sal_ReportSalaryMonthlyEntity.FieldNames.HeadCount);
            return tb;
        }


        #endregion

        public DataTable CreateReportGroupPayrollCostCentreSchema()
        {
            DataTable tb = new DataTable("Sal_ReportGroupPayrollCostCentreEntity");
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileID, typeof(Guid));
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CostCentreName);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.JobtitleName);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ElementName);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate_Money);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Amount);
            tb.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DateStart, typeof(DateTime));
            return tb;
        }
        public DataTable GetReportGroupPayrollCostCentre(DateTime datestart, DateTime dateend, List<Hre_ProfileEntity> profiles, string elementType, string UserLogin, bool isCreateTemplate)
        {
            DataTable table = CreateReportGroupPayrollCostCentreSchema();
            if (isCreateTemplate)
            {
                return table;
            }
            if (dateend != null)
                dateend = dateend.AddDays(1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var baservice = new BaseService();
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                List<object> parajobtitle = new List<object>();
                parajobtitle.AddRange(new object[5]);
                parajobtitle[0] = null;
                parajobtitle[1] = null;
                parajobtitle[2] = null;
                parajobtitle[3] = 1;
                parajobtitle[4] = 10000000;
                string status = string.Empty;
                //list Jobtitle
                var lstJotitle = baservice.GetData<Cat_JobTitleEntity>(parajobtitle, ConstantSql.hrm_cat_sp_get_JobTitle, UserLogin, ref status).ToList();
                if (lstJotitle == null)
                    return table;
                List<object> paraSal_CostCentreSal = new List<object>();
                paraSal_CostCentreSal.AddRange(new object[4]);
                paraSal_CostCentreSal[0] = datestart;
                paraSal_CostCentreSal[1] = dateend;
                paraSal_CostCentreSal[2] = 1;
                paraSal_CostCentreSal[3] = 10000000;
                //list Sal_CostCentreSal theo ngay va elementtype
                var lstSal_CostCentreSal = baservice.GetData<Sal_CostCentreSalEntity>(paraSal_CostCentreSal, ConstantSql.hrm_sal_sp_getdata_CostCentreSal, UserLogin, ref status).ToList();
                if (profiles.Count > 0)
                {
                    Guid[] orgprofileids = profiles.Select(s => s.ID).ToArray();
                    lstSal_CostCentreSal = lstSal_CostCentreSal.Where(s => orgprofileids.Contains(s.ProfileID.Value)).ToList();

                }
                Guid[] profileidSal_CostCentreSal = lstSal_CostCentreSal.Select(s => s.ProfileID.Value).Distinct().ToArray();
                profiles = profiles.Where(s => profileidSal_CostCentreSal.Contains(s.ID)).ToList();
                // chi lay nhung nv nao muon xu ly
                if (lstSal_CostCentreSal.Count == 0)
                    return table;
                //cutoff hien tai
                Guid[] cutOffDurationID = repoAtt_CutOffDuration.FindBy(s => s.DateStart <= DateTime.Today && s.DateEnd >= DateTime.Today).Select(s => s.ID).ToArray();
                //list id nv 
                Guid[] lstprofileIDs = lstSal_CostCentreSal.Select(s => s.ProfileID.Value).Distinct().ToArray();
                string profileIDs = string.Join(",", lstprofileIDs);
                string cutOffDurationIDs = string.Join(",", cutOffDurationID);
                List<object> paraSal_PayrollTableItem = new List<object>();
                paraSal_PayrollTableItem.AddRange(new object[2]);
                paraSal_PayrollTableItem[0] = profileIDs;
                paraSal_PayrollTableItem[1] = cutOffDurationIDs;
                //luong  nv
                var lstPayroll = baservice.GetData<Sal_PayrollTableEntity>(paraSal_PayrollTableItem, ConstantSql.hrm_sal_sp_get_PayrollTableItemByProfileId, UserLogin, ref status).ToList();
                //duyet tat ca chuc danh
                foreach (var profile in profiles)
                {

                    //Lay ds luong cua tung nv
                    var Payrollprofiles = lstPayroll.Where(s => s.ProfileID == profile.ID).ToList();
                    //xu ly luong va he so cho tung nhan vien
                    var lstSal_CostCentreSalprofile = lstSal_CostCentreSal.Where(s => s.ProfileID == profile.ID).ToList();

                    foreach (var Sal_CostCentreSalprofile in lstSal_CostCentreSalprofile)
                    {
                        double? tongtien = 0;
                        DataRow row = table.NewRow();
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileID] = profile.ID;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CostCentreName] = Sal_CostCentreSalprofile.CostCentreName;
                        //chuc danh theo costcenter
                        var jobtile = lstJotitle.Where(s => s.CostCentreID == Sal_CostCentreSalprofile.CostCentreID).FirstOrDefault();
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.JobtitleName] = jobtile.JobTitleName;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ElementName] = Sal_CostCentreSalprofile.ElementName;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate] = Sal_CostCentreSalprofile.Rate;
                        //lay luong theo elementtype
                        var payroll = Payrollprofiles.Where(s => s.ElementType == Sal_CostCentreSalprofile.ElementType).FirstOrDefault();
                        //tien theo element type
                        if (payroll != null)
                            row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate_Money] = payroll.Value != null ? double.Parse(payroll.Value) : 0;
                        //tong tien cua 1 chuc danh (hay costcentre)
                        double payrollvalue = 0;
                        if (payroll != null)
                            payrollvalue = payroll.Value != null ? double.Parse(payroll.Value) : 0;
                        tongtien = payrollvalue * Sal_CostCentreSalprofile.Rate;
                        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Amount] = tongtien;
                        table.Rows.Add(row);
                    }
                }
                #region jobtile
                //foreach (var jobtile in lstJotitle)
                //{
                //    double? tongtien = 0;
                //    DataRow row = table.NewRow();
                //    row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.JobtitleName] = jobtile.JobTitleName;
                //    //lay costcentre tuong ung
                //    lstSal_CostCentreSal = lstSal_CostCentreSal.Where(s => s.CostCentreID == jobtile.CostCentreID).ToList();
                //    if (lstSal_CostCentreSal.Count>0)
                //    {
                //        //xu ly tung nhan vien trong salcost_centresal
                //        var profileIDSal_CostCentreSal = lstSal_CostCentreSal.FirstOrDefault();
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CostCentreName] = lstSal_CostCentreSal.FirstOrDefault().CostCentreName;
                //        //Lay ds luong cua tung nv
                //        var Payrollprofiles = lstPayroll.Where(s => s.ProfileID == profileIDSal_CostCentreSal.ProfileID).ToList();
                //        //xu ly luong va he so cho tung nhan vien
                //        var lstSal_CostCentreSalprofile = lstSal_CostCentreSal.Where(s => s.ProfileID == profileIDSal_CostCentreSal.ProfileID).ToList();
                //        var profile = profiles.Where(s => s.ID == profileIDSal_CostCentreSal.ProfileID).FirstOrDefault();
                //        Guid? orgId = profile.OrgStructureID;
                //        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileName] = profile.ProfileName;
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;

                //        //tinh luong theo he so cua nhan vien do
                //        foreach (var Sal_CostCentreSalprofile in lstSal_CostCentreSalprofile)
                //        {

                //            row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ElementType] = Sal_CostCentreSalprofile.ElementType;
                //            row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate] = Sal_CostCentreSalprofile.Rate;
                //            //lay luong theo elementtype
                //           var payroll = Payrollprofiles.Where(s => s.ElementType == Sal_CostCentreSalprofile.ElementType).FirstOrDefault();
                //            //tien theo element type
                //            row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Rate_Money] =payroll!=null? double.Parse(payroll.Value): 0;
                //            //tong tien cua 1 chuc danh (hay costcentre)
                //            double payrollvalue = 0;
                //            if (payroll != null)
                //                payrollvalue = double.Parse(payroll.Value);
                //            tongtien += tongtien + (payrollvalue * Sal_CostCentreSalprofile.Rate);
                //        }
                //        row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Amount] = tongtien;
                //        //remove nv da xet ra khoi list
                //        lstSal_CostCentreSal = lstSal_CostCentreSal.Where(s => s.ProfileID != profileIDSal_CostCentreSal.ProfileID).ToList();
                //    }
                //    table.Rows.Add(row);
                //}
                #endregion

            }
            return table;
        }

        #region Hàm Lấy Phòng Ban theo đệ quy
        //Biến để get orderNumber của phòng ban
        string orderNumber = string.Empty;
        //Hàm đệ quy để lấy phòng ban
        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent, Guid? orgTypeID)
        {
            var child = source.Where(m => m.ParentID == idParent && m.OrgStructureTypeID == orgTypeID).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID, orgTypeID);
                }
            }
        }

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

        #region Lấy tên Phòng Ban của Nhân Viên
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOrg"></param>
        /// <param name="listOrgType"></param>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public List<string> GetParentOrg(List<Cat_OrgStructure> listOrg, List<Cat_OrgStructureType> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();
            while (true)
            {
                var item = listOrg.Single(m => m.ID == CurrentID);
                if (item != null)
                {
                    result.Add(listOrg.Single(m => m.ID == CurrentID).OrgStructureName);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "DEPARTMENT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }


            }
        }
        public List<string> GetParentOrgNameForShiseido(List<Cat_OrgStructure> listOrg, List<Cat_OrgStructureType> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();
            while (true)
            {
                var item = listOrg.Single(m => m.ID == CurrentID);
                if (item != null)
                {
                    result.Add(listOrg.Single(m => m.ID == CurrentID).OrgStructureName);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "CT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }


            }
        }

        public List<string> GetParentOrgCodeForShiseido(List<Cat_OrgStructure> listOrg, List<Cat_OrgStructureType> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();
            while (true)
            {
                var item = listOrg.Single(m => m.ID == CurrentID);
                if (item != null)
                {
                    result.Add(listOrg.Single(m => m.ID == CurrentID).Code);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "CT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }


            }
        }
        #endregion

        #region Hieu.van - BL Chuyển khoản (compare Ver.7) - TransferViaBank
        public DataTable getSchema_TransferViaBank(string nameReport,string[] listElement)
        {

            DataTable tblData = new DataTable(nameReport);
            tblData.Columns.Add("CodeEmp", typeof(string));
            tblData.Columns.Add("ProfileName", typeof(string));
            tblData.Columns.Add("CostCenter", typeof(string));
            tblData.Columns.Add("EnglishName", typeof(string));
            tblData.Columns.Add("HireDate", typeof(DateTime));
            tblData.Columns.Add("IDNo", typeof(string));
            tblData.Columns.Add("OrgStructureName", typeof(string));
            tblData.Columns.Add("BankGroup", typeof(string));
            tblData.Columns.Add("BankName", typeof(string));
            tblData.Columns.Add("AccountName", typeof(string));
            tblData.Columns.Add("BankAccountNo", typeof(string));
            tblData.Columns.Add("BankCode", typeof(string));
            //tblData.Columns.Add("Amount_USD", typeof(int));
            //tblData.Columns.Add("Amount_VND_INT", typeof(int));
            //tblData.Columns.Add("Amount_VND", typeof(Double));
            //tblData.Columns.Add("Amount_VND_ROUND", typeof(int));
            tblData.Columns.Add("MonthYear", typeof(DateTime));
            tblData.Columns.Add("PayrollGroup", typeof(string));
            foreach (var i in listElement)
            {
                if (!tblData.Columns.Contains(i))
                {
                    tblData.Columns.Add(i, typeof(Double));
                }
            }
            tblData.Columns.Add("Total_Insurance", typeof(Double));
            tblData.Columns.Add("PIT", typeof(Double));
            tblData.Columns.Add("Notes", typeof(string));
            return tblData;
        }

        public DataTable GetReportTransferViaBank(List<Hre_ProfileEntity> lstProfileFull, List<Guid> lstPgID, string GroupBank, List<Guid> listBankID, Guid durationID, bool ExcludeZero, string status, string arrayElement, string nameReport,string UserLogin, bool isCreateTemplate)
        {
            string[] ListElement = arrayElement.Split(',');
            DataTable tblData = getSchema_TransferViaBank(nameReport, ListElement);
            if (isCreateTemplate)
            {
                return tblData;
            }

            DateTime From = DateTime.MinValue;
            DateTime To = DateTime.MinValue;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var repoSal_SalaryInformation = new CustomBaseRepository<Sal_SalaryInformation>(unitOfWork);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                var repoCat_CostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);

                var cutoff = repoAtt_CutOffDuration.FindBy(s => s.ID == durationID).FirstOrDefault();
                var monthYear = cutoff.MonthYear;
                From = cutoff.DateStart;
                To = cutoff.DateEnd;

                if (durationID == Guid.Empty)
                {
                    //string typeConfig = AppConfig.E_RANGE_SALARY_MONTH.ToString();
                    //Sys_AppConfig sys_AppConfig = EntityService.GetEntity<Sys_AppConfig>(GuidContext
                    //        , LoginUserID, sy => sy.Info == typeConfig, true);
                    //Att_AttendanceServices.GetSalaryDateRange(null, sys_AppConfig, null, monthYear, out From, out To);

                    List<Cat_GradeAttendance> lstGradeAttendance = repoCat_GradeAttendance.GetAll().ToList();
                    Att_AttendanceServices.GetRangeMaxMinGrade(lstGradeAttendance, monthYear, out From, out To);
                }
                List<Hre_Profile> lstProfileALL = new Hre_ProfileServices().GetWorkingProfile(lstProfileFull, From, To);
                List<Hre_Profile> lstProfile = LoadProfileStatus(status, lstProfileALL, From, To);

                Cat_PayrollGroup prGroup = null;

                if (lstPgID.Count > 0)
                {
                    lstProfile = lstProfile.Where(p => p.PayrollGroupID.HasValue && lstPgID.Contains(p.PayrollGroupID.Value)).ToList();
                    //.OrderBy(p => p.udLinkDepartmentCode).ToList();
                }

                List<Sal_SalaryInformation> lstSalaryInformation = repoSal_SalaryInformation.FindBy(m => m.IsDelete != true).OrderByDescending(sal => sal.DateUpdate).ToList();
                List<Cat_CostCentre> listCostCentre = repoCat_CostCentre.FindBy(m => m.IsDelete != true).ToList();
                if (listBankID.Count > 0)
                {
                    lstSalaryInformation = lstSalaryInformation.Where(p => p.BankID.HasValue && listBankID.Contains(p.BankID.Value)).ToList();
                }

                if (!String.IsNullOrEmpty(GroupBank))
                {
                    lstSalaryInformation = lstSalaryInformation.Where(m => m.GroupBank.Contains(GroupBank)).ToList();
                }

                List<Guid> profileIDs = lstProfile.Select(s => s.ID).ToList();
                //List<Sal_PayrollTableItem> payrollTableItems = repoSal_PayrollTableItem.FindBy(m => m.IsDelete != true).ToList().Where(s => parollIDs.Contains(s.PayrollTableID)).ToList();

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[9]);
                listModel[1] = Common.DotNetToOracle(durationID.ToString());
                listModel[7] = 1;
                listModel[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> payrollTableItems = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

                foreach (Hre_Profile profile in lstProfile)
                {
                    List<Sal_SalaryInformation> lstSalaryInforProfile = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();
                    if (lstSalaryInforProfile == null
                        || lstSalaryInforProfile.Count <= 0
                        || lstSalaryInforProfile[0].AccountNo == null
                        || lstSalaryInforProfile[0].AccountNo == String.Empty)
                        continue;

                    var proEntity = lstProfileFull.Where(s => s.ID == profile.ID).FirstOrDefault();

                    List<Sal_PayrollTableItemEntity> lstProfilePayrollItem = payrollTableItems.Where(p => p.ProfileID == profile.ID).ToList();
                    List<Sal_PayrollTableItem> lstProfilePayrollItemNotEntity = lstProfilePayrollItem.Translate<Sal_PayrollTableItem>();

                    if (lstProfilePayrollItem == null || lstProfilePayrollItem.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        DataRow dr = tblData.NewRow();
                        dr["CodeEmp"] = profile.CodeEmp;
                        dr["ProfileName"] = profile.ProfileName;
                        var CostCentreByProfile = listCostCentre.FirstOrDefault(m => m.ID == profile.CostCentreID);
                        if (CostCentreByProfile != null)
                            dr["CostCenter"] = CostCentreByProfile.CostCentreName;
                        dr["EnglishName"] = profile.NameEnglish;
                        dr["HireDate"] = profile.DateHire;
                        dr["IDNo"] = profile.IDNo;
                        dr["OrgStructureName"] = proEntity.OrgStructureName;
                        dr["MonthYear"] = monthYear;
                        dr["AccountName"] = lstSalaryInforProfile[0].AccountName != null ? lstSalaryInforProfile[0].AccountName : string.Empty;
                        dr["BankAccountNo"] = lstSalaryInforProfile[0].AccountNo != null ? lstSalaryInforProfile[0].AccountNo : string.Empty;
                        dr["BankGroup"] = lstSalaryInforProfile[0].GroupBank != null ? lstSalaryInforProfile[0].GroupBank : string.Empty;
                        dr["BankName"] = lstSalaryInforProfile[0].Cat_Bank == null ? string.Empty : lstSalaryInforProfile[0].Cat_Bank.BankName;
                        dr["BankCode"] = lstSalaryInforProfile[0].Cat_Bank == null ? string.Empty : lstSalaryInforProfile[0].Cat_Bank.BankCode;
                        if (prGroup != null)
                            dr["PayrollGroup"] = prGroup.PayrollGroupName;
                        String currency = string.Empty;

                        object currencyObj = Sal_PayrollLib.GetItemValue(lstProfilePayrollItemNotEntity, PayrollElemType.CURRENCY.ToString());
                        if (currencyObj == null || String.IsNullOrEmpty(currencyObj.ToString()))
                            currency = CurrencyCode.VND.ToString();
                        else
                            currency = currencyObj.ToString();
                        //dr["Amount_VND"] = 0;
                        //dr["Amount_VND_ROUND"] = 0;
                        //dr["Amount_USD"] = 0;
                        //object _amount = Sal_PayrollLib.GetItemValue(listItem, elementCode);

                        object _amount_Total_InsuranceEmp = Sal_PayrollLib.GetItemValue(lstProfilePayrollItemNotEntity, PayrollElemType.TOTAL_INSURANCE_EMP.ToString());
                        object _amount_Total_InsuranceComp = Sal_PayrollLib.GetItemValue(lstProfilePayrollItemNotEntity, PayrollElemType.TOTAL_INSURANCE_COM.ToString());

                        double _Total_InsuranceEmp = 0;
                        double _Total_InsuranceComp = 0;
                        double _Total_Insurance = 0;

                        // insurance SonNgo
                        double.TryParse(_amount_Total_InsuranceEmp == null ? "0" : _amount_Total_InsuranceEmp.ToString(), out _Total_InsuranceEmp);
                        double.TryParse(_amount_Total_InsuranceComp == null ? "0" : _amount_Total_InsuranceComp.ToString(), out _Total_InsuranceComp);

                        _Total_Insurance = _Total_InsuranceEmp + _Total_InsuranceComp;

                        // PIT - SonNgo
                        object _amount_PIT = Sal_PayrollLib.GetItemValue(lstProfilePayrollItemNotEntity, PayrollElemType.PIT_EMP.ToString());
                        double _PIT = 0;
                        double.TryParse(_amount_PIT == null ? "0" : _amount_PIT.ToString(), out _PIT);

                        //Amount
                        //double totalAmount = 0;
                        //double.TryParse(_amount == null ? "0" : _amount.ToString(), out totalAmount);

                        if (currency == CurrencyCode.VND.ToString())
                        {
                            // Sonngo - 20120703 - Không được làm tròn về kiểu Int, vì chênh lệch với bảng lương toàn công ty
                            // Nếu cần làm tròn thì trên excel
                            dr["Total_Insurance"] = _Total_Insurance;
                            dr["PIT"] = _PIT;
                            //}
                        }

                        if (ExcludeZero)
                        {
                            String temp = Sal_PayrollLib.GetItemValue(lstProfilePayrollItemNotEntity
                                , PayrollElemType.NET_TAKE_HOME.ToString()).ToString();
                            if (String.IsNullOrEmpty(temp) == true || temp == "0")
                                continue;
                        }


                        foreach (var elementItem in ListElement)
                        {
                            Sal_PayrollTableItemEntity value = lstProfilePayrollItem.FirstOrDefault(m => m.Code == elementItem);
                            if (tblData.Columns.Contains(elementItem))
                            {
                                tblData.Columns.Remove(elementItem);
                            }
                            if (value != null)
                            {
                                if (value.ValueType == EnumDropDown.ElementDataType.Double.ToString())
                                {
                                    tblData.Columns.Add(elementItem, typeof(double));
                                    double _tmp = 0;
                                    double.TryParse(value.Value, out _tmp);
                                    dr[elementItem] = _tmp;
                                }
                                else if (value.ValueType == EnumDropDown.ElementDataType.Datetime.ToString())
                                {
                                    tblData.Columns.Add(elementItem, typeof(DateTime));
                                    DateTime _tmp = DateTime.MinValue;
                                    DateTime.TryParse(value.Value, out _tmp);
                                    dr[elementItem] = _tmp;
                                }
                                else
                                {
                                    tblData.Columns.Add(elementItem, typeof(string));
                                    dr[elementItem] = value.Value;
                                }
                            }
                        }

                        tblData.Rows.Add(dr);
                    }
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                config.Add("hidden", true);
                configs.Add("ID", config);
                return tblData.ConfigTable(configs, true);
            }
        }

        public List<Hre_Profile> LoadProfileStatus(String status, List<Hre_Profile> lstProfile, DateTime From, DateTime To)
        {
            if (status == StatusEmployee.E_WORKING.ToString())
            {
                lstProfile = lstProfile.Where(pro => (pro.DateQuit == null || pro.DateQuit > To) && pro.DateHire < From).ToList();
            }
            else if (status == StatusEmployee.E_NEWEMPLOYEE.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateHire < To && pro.DateHire > From).ToList();
            }
            else if (status == StatusEmployee.E_STOPWORKING.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateQuit < To && pro.DateQuit > From).ToList();
            }
            else if (status == StatusEmployee.E_WORKINGANDNEW.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateQuit == null || pro.DateQuit > To).ToList();
            }
            else
            {
                return lstProfile;
            }
            return lstProfile;
        }

        #endregion

        #region Hieu.van - BL Tiền Mặt (compare Ver.7) - CashPayroll
        public DataTable getSchema_Cash()
        {

            DataTable tblData = new DataTable();
            tblData.Columns.Add("CodeEmp", typeof(string));
            tblData.Columns.Add("ProfileName", typeof(string));
            tblData.Columns.Add("HireDate", typeof(DateTime));
            tblData.Columns.Add("JobTitleName", typeof(string));
            tblData.Columns.Add("PositionName", typeof(string));
            tblData.Columns.Add("OrgStructureName", typeof(string));

            tblData.Columns.Add("Amount_USD", typeof(Double));
            tblData.Columns.Add("Amount_VND", typeof(Double));
            tblData.Columns.Add("Amount_VND_ROUND", typeof(Double));
            tblData.Columns.Add("Notes", typeof(string));
            tblData.Columns.Add("MonthYear", typeof(DateTime));
            tblData.Columns.Add("Total_Insurance", typeof(Double));
            tblData.Columns.Add("PIT", typeof(Double));
            tblData.Columns.Add("BankName", typeof(string));
            tblData.Columns.Add("BankAccountNo", typeof(string));
            tblData.Columns.Add("PayrollGroup", typeof(string));
            return tblData;
        }

        public DataTable GetReportCash(List<Hre_ProfileEntity> lstProfileFull, List<Guid> lstPgID, Guid durationID, bool ExcludeZero, string status, string elementCode, bool isCreateTemplate)
        {
            DataTable tblData = getSchema_Cash();
            if (isCreateTemplate)
            {
                return tblData;
            }

            DateTime From = DateTime.MinValue;
            DateTime To = DateTime.MinValue;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var repoSal_SalaryInformation = new CustomBaseRepository<Sal_SalaryInformation>(unitOfWork);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                var repoSal_BasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);

                var cutoff = repoAtt_CutOffDuration.FindBy(s => s.ID == durationID).FirstOrDefault();
                var monthYear = cutoff.MonthYear;
                From = cutoff.DateStart;
                To = cutoff.DateEnd;

                if (durationID == Guid.Empty)
                {
                    //string typeConfig = AppConfig.E_RANGE_SALARY_MONTH.ToString();
                    //Sys_AppConfig sys_AppConfig = EntityService.GetEntity<Sys_AppConfig>(GuidContext
                    //        , LoginUserID, sy => sy.Info == typeConfig, true);
                    //Att_AttendanceServices.GetSalaryDateRange(null, sys_AppConfig, null, monthYear, out From, out To);

                    List<Cat_GradeAttendance> lstGradeAttendance = repoCat_GradeAttendance.GetAll().ToList();
                    Att_AttendanceServices.GetRangeMaxMinGrade(lstGradeAttendance, monthYear, out From, out To);
                }

                List<Hre_Profile> lstProfileALL = new Hre_ProfileServices().GetWorkingProfile(lstProfileFull, From, To);
                List<Hre_Profile> lstProfile = LoadProfileStatus(status, lstProfileALL, From, To);

                if (lstPgID.Count > 0)
                {
                    lstProfile = lstProfile.Where(p => p.PayrollGroupID.HasValue && lstPgID.Contains(p.PayrollGroupID.Value)).ToList();
                    //.OrderBy(p => p.udLinkDepartmentCode).ToList();
                }

                List<Guid> lstProfileID = Hre_ProfileServices.GetProfileIdList(lstProfile);
                List<Sal_PayrollTable> lstPayroll = new List<Sal_PayrollTable>();
                if (durationID == Guid.Empty)
                    lstPayroll = repoSal_PayrollTable.GetAll().Where(proll => proll.MonthYear == monthYear
                                                        && lstProfileID.Contains(proll.ProfileID))
                                                        .OrderByDescending(pay => pay.DateUpdate).ToList();
                else
                    lstPayroll = repoSal_PayrollTable.GetAll().Where(proll => proll.CutOffDurationID == durationID
                                                        && lstProfileID.Contains(proll.ProfileID))
                                                        .OrderByDescending(pay => pay.DateUpdate).ToList();

                List<Sal_SalaryInformation> lstSalaryInformation = repoSal_SalaryInformation.GetAll().Where(sal => lstProfileID.Contains(sal.ProfileID))
                    .OrderByDescending(sal => sal.DateUpdate).ToList();

                List<Sal_BasicSalary> lstBasicSalary = repoSal_BasicSalary.GetAll().Where(sal => lstProfileID.Contains(sal.ProfileID))
                                                            .OrderBy(bs => bs.DateOfEffect).ToList();


                foreach (Hre_Profile profile in lstProfile)
                {
                    var proEntity = lstProfileFull.Where(s => s.ID == profile.ID).FirstOrDefault();

                    List<Sal_SalaryInformation> lstSalaryInforProfile = lstSalaryInformation.Where(sal => sal.ProfileID == profile.ID).ToList();
                    if (lstSalaryInforProfile != null && lstSalaryInforProfile.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(lstSalaryInforProfile[0].AccountNo))
                            continue;
                    }

                    List<Sal_PayrollTable> lstProfilePayroll = lstPayroll.Where(p => p.ProfileID == profile.ID)
                                                                         .ToList();
                    if (lstProfilePayroll.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        DataRow dr = tblData.NewRow();
                        dr["ProfileName"] = profile.ProfileName;
                        dr["CodeEmp"] = profile.CodeEmp;
                        dr["HireDate"] = profile.DateHire;
                        dr["OrgStructureName"] = profile.OrgStructureID == null ? string.Empty : proEntity.OrgStructureName;

                        dr["MonthYear"] = monthYear;

                        dr["JobTitleName"] = profile.JobTitleID == null ? string.Empty : proEntity.JobTitleName;
                        dr["PositionName"] = profile.PositionID == null ? string.Empty : proEntity.PositionName;
                        List<Sal_BasicSalary> basicSalaryList = lstBasicSalary.Where(sa => sa.ProfileID == profile.ID).ToList();
                        String currency = string.Empty;
                        if (basicSalaryList.Count > 0)
                        {
                            currency = basicSalaryList[0].Cat_Currency.Code;
                        }

                        List<Sal_PayrollTableItem> listItem = lstProfilePayroll[0].Sal_PayrollTableItem.ToList();
                        dr["Amount_VND"] = 0;
                        dr["Amount_VND_ROUND"] = 0;
                        dr["Amount_USD"] = 0;

                        object _amount = Sal_PayrollLib.GetItemValue(listItem, elementCode);

                        // SonNgo - 20120702
                        object _amount_Total_InsuranceEmp = Sal_PayrollLib.GetItemValue(listItem, PayrollElemType.TOTAL_INSURANCE_EMP.ToString());
                        object _amount_Total_InsuranceComp = Sal_PayrollLib.GetItemValue(listItem, PayrollElemType.TOTAL_INSURANCE_COM.ToString());

                        double _Total_InsuranceEmp = 0;
                        double _Total_InsuranceComp = 0;
                        double _Total_Insurance = 0;

                        // insurance SonNgo
                        double.TryParse(_amount_Total_InsuranceEmp == null ? "0" : _amount_Total_InsuranceEmp.ToString(), out _Total_InsuranceEmp);
                        double.TryParse(_amount_Total_InsuranceComp == null ? "0" : _amount_Total_InsuranceComp.ToString(), out _Total_InsuranceComp);

                        _Total_Insurance = _Total_InsuranceEmp + _Total_InsuranceComp;

                        // PIT - SonNgo
                        object _amount_PIT = Sal_PayrollLib.GetItemValue(listItem, PayrollElemType.PIT_EMP.ToString());
                        double _PIT = 0;
                        double.TryParse(_amount_PIT == null ? "0" : _amount_PIT.ToString(), out _PIT);

                        double totalAmount = 0;
                        double.TryParse(_amount == null ? "0" : _amount.ToString(), out totalAmount);

                        if (totalAmount == 0)
                        {
                            continue;
                        }

                        if (currency == CurrencyCode.VND.ToString())
                        {
                            dr["Amount_VND"] = totalAmount;// PayrollService.GetItemValueTypeofDouble(listItem, PayrollElemType.NET_TAKE_HOME.ToString());
                            dr["Amount_VND_ROUND"] = Common.GetRoundMoney(totalAmount, 3);

                            // SonNgo - 20120702
                            dr["Total_Insurance"] = _Total_Insurance;
                            dr["PIT"] = _PIT;
                        }
                        else if (currency == CurrencyCode.USD.ToString())
                        {
                            dr["Amount_USD"] = totalAmount;// PayrollService.GetItemValueTypeofDouble(listItem, PayrollElemType.NET_TAKE_HOME.ToString());
                        }

                        if (ExcludeZero)
                        {
                            String temp = Sal_PayrollLib.GetItemValue(listItem
                                , PayrollElemType.NET_TAKE_HOME.ToString()).ToString();

                            if (String.IsNullOrEmpty(temp) || temp == "0")
                                continue;
                        }
                        tblData.Rows.Add(dr);
                    }
                }
                return tblData;
            }
        }

        #endregion

        #region Hieu.van - BC Thuế Thu Nhập Cá Nhân

        public DataTable getSchema_PIT()
        {

            DataTable tblData = new DataTable();
            tblData.Columns.Add("CountPersonalVN", typeof(Double));
            tblData.Columns.Add("CountPersonalEN", typeof(Double));

            tblData.Columns.Add("BeforeTaxPersonalVN", typeof(Double));
            tblData.Columns.Add("BeforeTaxPersonalVN_NoContract", typeof(Double));
            tblData.Columns.Add("BeforeTaxPersonalEN", typeof(Double));

            tblData.Columns.Add("IncomeTaxablePersonalVN", typeof(Double));
            tblData.Columns.Add("IncomeTaxablePersonalVN_NoContract", typeof(Double));
            tblData.Columns.Add("IncomeTaxablePersonalEN", typeof(Double));

            tblData.Columns.Add("PitEmpPersonalVN", typeof(Double));
            tblData.Columns.Add("PitEmpPersonalVN_NoContract", typeof(Double));
            tblData.Columns.Add("PitEmpPersonalEN", typeof(Double));

            tblData.Columns.Add("Year", typeof(int));
            return tblData;
        }

        public DataTable GetReportPIT(List<Hre_ProfileEntity> profiles, int year, string[] employeeStatus, bool isCreateTemplate)
        {
            DataTable tblData = getSchema_PIT();
            if (isCreateTemplate)
            {
                return tblData;
            }
            DateTime dateFrom = new DateTime(year, 1, 1);
            DateTime dateTo = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);

                if (employeeStatus != null)
                {
                    if (employeeStatus.Contains(StatusEmployee.E_WORKING.ToString()))
                    {
                        profiles = profiles.Where(pro => (pro.DateQuit == null || pro.DateQuit >= dateTo) && pro.DateHire < dateFrom).ToList();
                    }
                    if (employeeStatus.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                    {
                        profiles = profiles.Where(pro => pro.DateHire <= dateTo && pro.DateHire >= dateFrom).ToList();
                    }
                    if (employeeStatus.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                    {
                        profiles = profiles.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= dateTo && pro.DateQuit.Value >= dateFrom).ToList();
                    }
                    if (employeeStatus.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                    {
                        profiles = profiles.Where(pro => pro.DateQuit == null || pro.DateQuit >= dateTo).ToList();
                    }
                }

                //List<Guid> orgIDs = listFilterInfo.GetFilterValue1<List<Guid>>(InstanceNames.OrgStructure);
                //DateTime date = listFilterInfo.GetFilterValue1<DateTime?>(InstanceNames.Year) ?? DateTime.Now;

                //List<Hre_Profile> profiles = EntityService.GetEntityList<Hre_Profile>(EntityService.GuidMainContext, LoginUserID, hre => (hre.DateQuit == null || hre.DateQuit.Value.Year < year));
                //if (orgIDs[0] != Guid.Empty)
                //{
                //    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                //}
                List<Guid> lstGuidProfile = profiles.Select(pro => pro.ID).ToList();
                //Lay bang luong toan cong ty trong nam           
                List<Sal_PayrollTable> list_PayrollTable = repoSal_PayrollTable.GetAll().Where(ir => ir.MonthYear.Year == year && lstGuidProfile.Contains(ir.ProfileID)).ToList();

                //Lay bang luong chi tiet     
                List<Guid> lstGuidPayTable = list_PayrollTable.Select(pay => pay.ID).ToList();
                List<Sal_PayrollTableItem> list_PayrollTableItem = repoSal_PayrollTableItem.GetAll().Where(ir => lstGuidPayTable.Contains(ir.PayrollTableID)).ToList();

                //Lay ds hop dong.
                List<Hre_Contract> lstHreContract = repoHre_Contract.GetAll().Where(ir => lstGuidProfile.Contains(ir.ProfileID)).ToList();
                List<Guid> lstGuidProContract = lstHreContract.Select(con => con.ProfileID).Distinct().ToList();
                List<Guid> lstGuidProNoContract = lstGuidProfile.Where(pro => !lstGuidProContract.Contains(pro)).ToList();

                DataRow row = tblData.NewRow();
                row["Year"] = year;
                //Dem Count so nhan vien
                row["CountPersonalVN"] = lstGuidProfile.Count();
                row["CountPersonalEN"] = 0;
                //Tinh thu nhap truoc thue
                String strBefTax = PayrollElemType.INCOME_BEFORE_TAX.ToString();
                List<Guid> lstGuidPayBefTax_Contract = list_PayrollTable.Where(pay => lstGuidProContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Guid> lstGuidPayBefTax_NoContract = list_PayrollTable.Where(pay => lstGuidProNoContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Sal_PayrollTableItem> listBefTaxPayItem_Contract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_Contract.Contains(item.PayrollTableID) && item.ElementType == strBefTax).ToList();
                List<Sal_PayrollTableItem> listBefTaxPayItem_NoContract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_NoContract.Contains(item.PayrollTableID) && item.ElementType == strBefTax).ToList();
                row["BeforeTaxPersonalVN"] = listBefTaxPayItem_Contract.Sum(item => Convert.ToDouble(item.Value));
                row["BeforeTaxPersonalVN_NoContract"] = listBefTaxPayItem_NoContract.Sum(item => Convert.ToDouble(item.Value));

                row["BeforeTaxPersonalEN"] = 0;

                //Tinh thu nhap chiu thue
                String strIncomeTaxable = PayrollElemType.INCOME_TAXABLE.ToString();
                List<Guid> lstGuidPayInTax_Contract = list_PayrollTable.Where(pay => lstGuidProContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Guid> lstGuidPayInTax_NoContract = list_PayrollTable.Where(pay => lstGuidProNoContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Sal_PayrollTableItem> listInTaxPayItem_Contract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_Contract.Contains(item.PayrollTableID) && item.ElementType == strIncomeTaxable).ToList();
                List<Sal_PayrollTableItem> listInTaxPayItem_NoContract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_NoContract.Contains(item.PayrollTableID) && item.ElementType == strIncomeTaxable).ToList();
                row["IncomeTaxablePersonalVN"] = listInTaxPayItem_Contract.Sum(item => Convert.ToDouble(item.Value));
                row["IncomeTaxablePersonalVN_NoContract"] = listInTaxPayItem_NoContract.Sum(item => Convert.ToDouble(item.Value));

                row["IncomeTaxablePersonalEN"] = 0;


                //Tinh pit
                String strPitEmp = PayrollElemType.PIT_EMP.ToString();
                List<Guid> lstGuidPayPitEmp_Contract = list_PayrollTable.Where(pay => lstGuidProContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Guid> lstGuidPayPitEmp_NoContract = list_PayrollTable.Where(pay => lstGuidProNoContract.Contains(pay.ProfileID)).Select(pay => pay.ID).ToList();
                List<Sal_PayrollTableItem> listPitEmpPayItem_Contract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_Contract.Contains(item.PayrollTableID) && item.ElementType == strPitEmp).ToList();
                List<Sal_PayrollTableItem> listPitEmpPayItem_NoContract = list_PayrollTableItem.Where(item => lstGuidPayBefTax_NoContract.Contains(item.PayrollTableID) && item.ElementType == strPitEmp).ToList();
                row["PitEmpPersonalVN"] = listPitEmpPayItem_Contract.Sum(item => Convert.ToDouble(item.Value));
                row["PitEmpPersonalVN_NoContract"] = listPitEmpPayItem_NoContract.Sum(item => Convert.ToDouble(item.Value));

                row["PitEmpPersonalEN"] = 0;
                tblData.Rows.Add(row);

                return tblData;
            }
        }

        #endregion

        #region Hien.Nguyen BC Biến Động Lương

        public DataTable ReportVariableSalary(List<Hre_ProfileEntity> listProfile, DateTime MonthYear, string[] ArrElement, string UserLogin, bool IsCreateTemplate = false)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);

                #region Create Schema
                DataTable Table = new DataTable("ReportVariableSalary");
                Table.Columns.Add("STT");
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CodeEmp);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileName);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Brand);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DepartmentName);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.WorkPlace);
                //Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Position);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.PositionName);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.PositionCode);
                Table.Columns.Add(Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DateHire);
                foreach (var i in ArrElement)
                {
                    Table.Columns.Add(i + "_1", typeof(double));
                    Table.Columns.Add(i + "_2", typeof(double));
                }

                if (IsCreateTemplate)
                {
                    return Table;
                }
                #endregion

                var baservice = new BaseService();
                string status = string.Empty;
                List<object> listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_ShopEntity> listShop = GetData<Cat_ShopEntity>(listModel, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[9]);
                listModel[2] = new DateTime(MonthYear.Year, MonthYear.Month, 1);
                listModel[3] = new DateTime(MonthYear.Year, MonthYear.Month, 1).AddMonths(1).AddDays(-1);
                listModel[7] = 1;
                listModel[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem_1 = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[9]);
                MonthYear = MonthYear.AddMonths(-1);
                listModel[2] = new DateTime(MonthYear.Year, MonthYear.Month, 1);
                listModel[3] = new DateTime(MonthYear.Year, MonthYear.Month, 1).AddMonths(1).AddDays(-1);
                listModel[7] = 1;
                listModel[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem_2 = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

                foreach (var profile in listProfile)
                {
                    DataRow Row = Table.NewRow();
                    Row["STT"] = listProfile.IndexOf(profile) + 1;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.ProfileName] = profile.ProfileName;
                    if (profile.ShopID != null)
                    {
                        var _tmpShop = listShop.Where(m => m.ID == profile.ShopID).FirstOrDefault();
                        if (_tmpShop != null)
                        {
                            Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Brand] = _tmpShop.ShopGroupName;
                        }
                    }
                    //Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DepartmentName] = profile.DepartmentNameOrg;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DepartmentName] = profile.E_DEPARTMENT;
                    //Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.WorkPlace] =profile.WorkingPlace;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.WorkPlace] = profile.WorkPlaceName;
                    //Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.Position] =profile.PositionName;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.PositionName] = profile.PositionName;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.PositionCode] = profile.PositionCode;
                    Row[Sal_ReportGroupPayrollCostCentreEntity.FieldNames.DateHire] = profile.DateHire;

                    #region Xuất các phần tử lương
                    double _tmp = 0;
                    foreach (var element in ArrElement)
                    {
                        var PayrollItem_1 = listPayrollTableItem_1.Where(m => m.ProfileID == profile.ID && m.Code == element).FirstOrDefault();
                        var PayrollItem_2 = listPayrollTableItem_2.Where(m => m.ProfileID == profile.ID && m.Code == element).FirstOrDefault();
                        if (PayrollItem_1 != null && double.TryParse(PayrollItem_1.Value, out _tmp) == true)
                        {
                            Row[element + "_1"] = double.Parse(PayrollItem_1.Value);
                        }
                        if (PayrollItem_2 != null && double.TryParse(PayrollItem_2.Value, out _tmp) == true)
                        {
                            Row[element + "_2"] = double.Parse(PayrollItem_2.Value);
                        }


                    }
                    Table.Rows.Add(Row);
                    #endregion
                }

                return Table;
            }
        }

        #endregion

        #region Hien.Nguyen Lịch Sử Các Phần Tử Lương

        public DataTable ReportHistoryPayrollElement(List<Hre_ProfileEntity> listProfile, DateTime MonthStart, DateTime MonthEnd, string[] ArrElement, string UserLogin, bool IsCteateTemplate = false)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                List<Sal_PayrollTableItemEntity> ListPayrollTableByElement = new List<Sal_PayrollTableItemEntity>();
                List<Sal_PayrollTableItemEntity> ListPayrollTableByMonth = new List<Sal_PayrollTableItemEntity>();

                DateTime _MonthStart = MonthStart;
                DateTime _MonthEnd = MonthEnd;

                DataTable Table = new DataTable("ReportHistoryPayrollElement");
                Table.Columns.Add("STT");
                Table.Columns.Add("Category");
                int index = 1;
                while (MonthStart <= MonthEnd)
                {
                    Table.Columns.Add("Month" + index++, typeof(double));
                    MonthStart = MonthStart.AddMonths(1);
                }

                MonthStart = _MonthStart;
                MonthEnd = _MonthEnd;

                if (IsCteateTemplate)
                {
                    return Table;
                }

                string status = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status);
                lstElement = lstElement.Where(m => ArrElement.Contains(m.ElementCode)).ToList();

                var ListObjectPayroll = new List<object>();
                ListObjectPayroll.AddRange(new object[9]);
                ListObjectPayroll[2] = MonthStart;
                ListObjectPayroll[3] = MonthEnd;
                ListObjectPayroll[7] = 1;
                ListObjectPayroll[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listTableItem = GetData<Sal_PayrollTableItemEntity>(ListObjectPayroll, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

                listTableItem = listTableItem.Where(m => m.ProfileID != null && listProfile.Any(t => t.ID == (Guid)m.ProfileID)).ToList();


                foreach (var element in lstElement)
                {
                    DataRow Row = Table.NewRow();
                    Row["STT"] = lstElement.IndexOf(element) + 1;
                    if (lstElement.IndexOf(element) == 0)
                    {
                        Row["Category"] = "Number of employees";
                        index = 1;
                        while (MonthStart <= MonthEnd)
                        {
                            Row["Month" + index] = listTableItem.Where(m => m.MonthYear.Value.Month == MonthStart.Month && m.MonthYear.Value.Year == MonthStart.Year).Count();
                            MonthStart = MonthStart.AddMonths(1);
                            index++;
                        }

                        MonthStart = _MonthStart;
                        MonthEnd = _MonthEnd;
                    }
                    else
                    {
                        Row["Category"] = element.ElementName;
                        //lọc tableitem theo element code đang duyệt
                        ListPayrollTableByElement = listTableItem.Where(m => m.Code == element.ElementCode).ToList();
                        index = 1;
                        while (MonthStart <= MonthEnd)
                        {
                            //lọc tableitem theo Month đang duyệt
                            ListPayrollTableByMonth = ListPayrollTableByElement.Where(m => m.MonthYear.Value.Month == MonthStart.Month && m.MonthYear.Value.Year == MonthStart.Year).ToList();

                            double _Value = 0;
                            foreach (var payrolltable in ListPayrollTableByMonth)
                            {
                                double _tmp = 0;
                                double.TryParse(payrolltable.Value, out _tmp);
                                _Value += _tmp;
                            }
                            Row["Month" + index] = _Value;

                            MonthStart = MonthStart.AddMonths(1);
                            index++;
                        }

                        MonthStart = _MonthStart;
                        MonthEnd = _MonthEnd;
                    }
                    Table.Rows.Add(Row);
                }

                return Table;
            }

        }

        #endregion

        #region Báo cáo HVN
        /// <summary>
        /// Báo cáo DeadCount số lượng nhân viên theo từng tháng
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="OrgTructureIds"></param>
        /// <returns></returns>
        public DataTable Sal_ReportHeadCount(DateTime dateStart, DateTime dateEnd, string OrgTructureIds, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region GetData
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructureEntity>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitleEntity>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistoryEntity>(unitOfWork);

                List<object> listModel = new List<object>();

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_OrgStructureEntity> listOrgTructure = GetData<Cat_OrgStructureEntity>(listModel, ConstantSql.hrm_cat_sp_get_AllOrgStructure, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_JobTitleEntity> listJobtitle = GetData<Cat_JobTitleEntity>(listModel, ConstantSql.hrm_cat_sp_get_JobTitle, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[1] = dateStart;
                listModel[2] = dateEnd;
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Hre_WorkHistoryEntity> listWorkHistory = GetData<Hre_WorkHistoryEntity>(listModel, ConstantSql.hrm_hr_sp_get_RptWorkHistoryDeptByids, UserLogin, ref status);
                #endregion

                #region Progress
                DataTable Table = new DataTable("Sal_ReportHeadCount");

                //Create column
                Table.Columns.Add("STT");
                Table.Columns.Add("E_UNIT");
                Table.Columns.Add("E_DIVISION");
                Table.Columns.Add("E_DEPARTMENT");
                Table.Columns.Add("E_TEAM");
                Table.Columns.Add("E_SECTION");
                //   Table.Columns.Add("Department");

                //Conver lại datetime cho dễ xử lý
                dateStart = new DateTime(dateStart.Year, dateStart.Month, 1);
                dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, 1);

                //Biến lưu lại tên column động theo thời gian
                List<DateTime> listColumnName = new List<DateTime>();

                while (dateStart <= dateEnd)
                {
                    //Lọc tên column
                    string sateString = dateStart.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

                    //Add vào list lưu column lại
                    listColumnName.Add(dateStart);

                    //Create column
                    Table.Columns.Add(sateString);

                    //Tăng datestart lên 1 tháng
                    dateStart = dateStart.AddMonths(1);
                }

                //Lấy ra các phòng ban được chọn
                if (OrgTructureIds != null && OrgTructureIds != string.Empty)
                {
                    string[] listOrg = OrgTructureIds.Split(',').ToArray();
                    listOrgTructure = listOrgTructure.Where(m => listOrg.Any(t => t == m.OrderNumber.ToString())).ToList();

                }

                List<Cat_JobTitleEntity> _tmpJobtitle = new List<Cat_JobTitleEntity>();
                List<Cat_OrgStructureEntity> _tmpOrgStructure = new List<Cat_OrgStructureEntity>();

                //Duyệt wa các phòng ban đó
                if (listOrgTructure.Count > 0)
                {
                    for (int i = 0; i < listOrgTructure.Count; i++)
                    {
                        //lọc ra các nhân viên thuộc phòng ban hiện tại
                        List<Hre_WorkHistoryEntity> WorkHistoryItem = listWorkHistory.Where(m => m.OrganizationStructureID == listOrgTructure[i].ID).ToList();

                        DataRow row = Table.NewRow();
                        row["STT"] = i + 1;
                        row["E_UNIT"] = listOrgTructure[i].E_UNIT;
                        row["E_DIVISION"] = listOrgTructure[i].E_DIVISION;
                        row["E_DEPARTMENT"] = listOrgTructure[i].E_DEPARTMENT;
                        row["E_TEAM"] = listOrgTructure[i].E_TEAM;
                        row["E_SECTION"] = listOrgTructure[i].E_SECTION;
                        // row["Department"] = listOrgTructure[i].OrgStructureName;


                        //dữ liệu của row là phòng ban
                        foreach (DateTime column in listColumnName)
                        {
                            DateTime from = new DateTime(column.Year, column.Month, 1);
                            DateTime to = column.AddMonths(1).AddDays(-1);
                            row[column.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")] = WorkHistoryItem.Where(m => from <= m.DateEffective && to >= m.DateEffective).Select(m => m.ProfileID).Distinct().Count();
                        }
                        Table.Rows.Add(row);

                        //lọc jobtitle
                        _tmpJobtitle = listJobtitle.Where(m => m.OrgStructureID == listOrgTructure[i].ID).ToList();
                        if (_tmpJobtitle.Count > 0)
                        {
                            foreach (var jobTitle in _tmpJobtitle)
                            {
                                row = Table.NewRow();
                                row["STT"] = "";
                                row["E_UNIT"] = jobTitle.E_UNIT;
                                row["E_DIVISION"] = jobTitle.E_DIVISION;
                                row["E_DEPARTMENT"] = jobTitle.E_DEPARTMENT;
                                row["E_TEAM"] = jobTitle.E_TEAM;
                                row["E_SECTION"] = jobTitle.E_SECTION;

                                //   row["Department"] = jobTitle.JobTitleName;
                                foreach (DateTime column in listColumnName)
                                {
                                    DateTime from = new DateTime(column.Year, column.Month, 1);
                                    DateTime to = column.AddMonths(1).AddDays(-1);
                                    row[column.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")] = WorkHistoryItem.Where(m => m.JobTitleID == jobTitle.ID && from <= m.DateEffective && to >= m.DateEffective).Select(m => m.ProfileID).Distinct().Count();
                                }
                                Table.Rows.Add(row);
                            }
                        }
                    }
                }
                #endregion

                return Table;
            }
        }
        /// <summary>
        /// Xuất báo cáo lương theo loại phần tử (ElementType)
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="OrgTructureIds"></param>
        /// <param name="ElementType"></param>
        /// <returns></returns>
        public DataTable Sal_ReportSalaryElement(DateTime dateStart, DateTime dateEnd, string OrgTructureIds, string[] ElementType, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region GetData
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSal_PayrollTableItem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);
                var repoSal_PayrollTable = new CustomBaseRepository<Sal_PayrollTable>(unitOfWork);

                List<object> listModel = new List<object>();

                List<Sal_PayrollTableEntity> listTable = new List<Sal_PayrollTableEntity>();
                List<Sal_PayrollTableItemEntity> listTableItem = new List<Sal_PayrollTableItemEntity>();

                //Lọc từ bảng lương theo tất cả các điều kiện

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[2] = dateStart;
                listModel[3] = dateEnd;
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                listTable = GetData<Sal_PayrollTableEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[9]);
                listModel[2] = dateStart;
                listModel[3] = dateEnd;
                listModel[6] = 1;
                listModel[7] = Int32.MaxValue - 1;
                listTableItem = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

                //Get Profile
                listModel = new List<object>();
                listModel.AddRange(new object[18]);
                listModel[16] = 1;
                listModel[17] = int.MaxValue - 1;
                List<Hre_ProfileEntity> listHre_profile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_OrgStructureEntity> listOrgTructure = GetData<Cat_OrgStructureEntity>(listModel, ConstantSql.hrm_cat_sp_get_AllOrgStructure, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_JobTitleEntity> listJobtitle = GetData<Cat_JobTitleEntity>(listModel, ConstantSql.hrm_cat_sp_get_JobTitle, UserLogin, ref status);

                //lọc lại payrollTableItem theo ElementType
                listTableItem = listTableItem.Where(m => ElementType.Any(t => t == m.ElementType)).ToList();

                #endregion

                #region Process

                #region Create Column
                //Init datatable
                DataTable Table = new DataTable("Sal_ReportSalaryElement");

                //Create column
                Table.Columns.Add("STT");
                Table.Columns.Add("E_UNIT");
                Table.Columns.Add("E_DIVISION");
                Table.Columns.Add("E_DEPARTMENT");
                Table.Columns.Add("E_TEAM");
                Table.Columns.Add("E_SECTION");
                // Table.Columns.Add("Department");

                //Conver lại datetime cho dễ xử lý
                dateStart = new DateTime(dateStart.Year, dateStart.Month, 1);
                dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, 1);

                //Biến lưu lại tên column động theo thời gian
                List<DateTime> listColumnName = new List<DateTime>();

                while (dateStart <= dateEnd)
                {
                    //Lọc tên column
                    string sateString = dateStart.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

                    //Add vào list lưu column lại
                    listColumnName.Add(dateStart);

                    //Create column
                    Table.Columns.Add(sateString);

                    //Tăng datestart lên 1 tháng
                    dateStart = dateStart.AddMonths(1);
                }
                #endregion

                //Lấy ra các phòng ban được chọn
                if (OrgTructureIds != null && OrgTructureIds != string.Empty)
                {
                    string[] listOrg = OrgTructureIds.Split(',').ToArray();
                    listOrgTructure = listOrgTructure.Where(m => listOrg.Any(t => t == m.OrderNumber.ToString())).ToList();

                }

                List<Cat_JobTitleEntity> _tmpJobtitle = new List<Cat_JobTitleEntity>();
                List<Cat_OrgStructureEntity> _tmpOrgStructure = new List<Cat_OrgStructureEntity>();

                //Duyệt wa các phòng ban đó
                if (listOrgTructure.Count > 0)
                {
                    for (int i = 0; i < listOrgTructure.Count; i++)
                    {
                        DataRow row = Table.NewRow();
                        row["STT"] = i + 1;
                        row["E_UNIT"] = listOrgTructure[i].E_UNIT;
                        row["E_DIVISION"] = listOrgTructure[i].E_DIVISION;
                        row["E_DEPARTMENT"] = listOrgTructure[i].E_DEPARTMENT;
                        row["E_TEAM"] = listOrgTructure[i].E_TEAM;
                        row["E_SECTION"] = listOrgTructure[i].E_SECTION;
                        //  row["Department"] = listOrgTructure[i].OrgStructureName;

                        //dữ liệu của row là phòng ban
                        foreach (DateTime column in listColumnName)
                        {
                            //Lọc tableitem theo tháng hiện tại
                            var _tmpTable = listTable.Where(m => m.MonthYear.Year == column.Year && m.MonthYear.Month == column.Month && m.OrgStructureID == listOrgTructure[i].ID).ToList();
                            var listTableItemByMonthYear = listTableItem.Where(m => _tmpTable.Any(t => t.ID == m.PayrollTableID)).ToList();

                            double ValueItem = 0;
                            double TotalValue = 0;
                            foreach (var TableItemByMonthYear in listTableItemByMonthYear)
                            {
                                ValueItem = 0;
                                Double.TryParse(TableItemByMonthYear.Value, out ValueItem);
                                TotalValue += ValueItem;
                            }
                            //Add to row
                            row[column.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")] = TotalValue;
                            //Reset total value
                            TotalValue = 0;
                            //Add to table

                        }
                        Table.Rows.Add(row);

                        //lọc jobtitle
                        _tmpJobtitle = listJobtitle.Where(m => m.OrgStructureID == listOrgTructure[i].ID).ToList();
                        //Add dữ liệu row của Jobtitle
                        if (_tmpJobtitle.Count > 0)
                        {
                            foreach (var jobTitle in _tmpJobtitle)
                            {
                                row = Table.NewRow();
                                row["STT"] = "";
                                row["E_UNIT"] = jobTitle.E_UNIT;
                                row["E_DIVISION"] = jobTitle.E_DIVISION;
                                row["E_DEPARTMENT"] = jobTitle.E_DEPARTMENT;
                                row["E_TEAM"] = jobTitle.E_TEAM;
                                row["E_SECTION"] = jobTitle.E_SECTION;
                                foreach (DateTime column in listColumnName)
                                {
                                    //Lọc tableitem theo tháng hiện tại
                                    var _tmpTable = listTable.Where(m => m.MonthYear.Year == column.Year && m.MonthYear.Month == column.Month && m.OrgStructureID == listOrgTructure[i].ID && m.JobTitleID == jobTitle.ID).ToList();
                                    var listTableItemByMonthYear = listTableItem.Where(m => _tmpTable.Any(t => t.ID == m.PayrollTableID)).ToList();

                                    double ValueItem = 0;
                                    double TotalValue = 0;
                                    foreach (var TableItemByMonthYear in listTableItemByMonthYear)
                                    {
                                        ValueItem = 0;
                                        Double.TryParse(TableItemByMonthYear.Value, out ValueItem);
                                        TotalValue += ValueItem;
                                    }
                                    //Add to row
                                    row[column.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")] = TotalValue;
                                    //Reset total value
                                    TotalValue = 0;
                                    //Add to table
                                }
                                Table.Rows.Add(row);
                            }
                        }
                    }
                }

                #endregion

                return Table;
            }
        }

        /// <summary>
        /// Báo cáo con nhỏ
        /// </summary>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        /// <param name="OrgTructureIds"></param>
        /// <returns></returns>
        public DataTable ReportChildCare(DateTime datestart, string OrgTructureIds, string UserLogin, bool IsCreateTemplate)
        {
            #region Create Column
            DataTable Table = new DataTable("ReportChildCare");
            Table.Columns.Add("STT");
            Table.Columns.Add(ConstantDisplay.HRM_Sal_HoldSalary_CodeEmp.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_HR_Relatives_ProfileName.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Hre_Report_E_UNIT.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Hre_Report_E_DIVISION.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Hre_Report_E_DEPARTMENT.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Hre_Report_E_TEAM.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Hre_Report_E_SECTION.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Payroll_UnusualEDChild_ChildCount.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Recruitment_UnusualAllowance_Amount.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Recruitment_UnusualAllowance_AmountofOffset.TranslateString());
            Table.Columns.Add(ConstantDisplay.HRM_Sal_InsuranceSalry_Note.TranslateString());

            if (IsCreateTemplate)
            {
                return Table;
            }
            #endregion

            #region Get Data
            string status = string.Empty;
            List<object> listModel = new List<object>();
            DateTime dateend = new DateTime(datestart.Year, datestart.Month, 1).AddMonths(1).AddDays(-1);

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgTructureIds;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            //listModel[4] = new DateTime(datestart.Year, datestart.Month, 1);
            //listModel[5] = new DateTime(datestart.Year, datestart.Month, 1).AddMonths(1).AddDays(-1);
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = datestart;
            listModel[4] = dateend;
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare, UserLogin, ref status).Where(m => ListProfile.Any(t => t.ID == m.ProfileID) && m.MonthStart != null && m.MonthStart.Value.Year == datestart.Year && m.MonthStart.Value.Month == datestart.Month).ToList();

            //loc nhan vien Hold Luong
            listHoldSalary = listHoldSalary.Where(m => (m.MonthSalary <= dateend && m.MonthEndSalary == null) || (m.MonthSalary <= dateend && m.MonthEndSalary != null && m.MonthEndSalary >= datestart)).ToList();


            #endregion

            #region Progess
            Guid[] ListProfileID = ListUnusualAllowance.Select(m => m.ProfileID).Distinct().ToArray();
            ListProfileID = ListProfileID.Where(m => !listHoldSalary.Any(t => t.ProfileID == m)).ToArray();
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowanceByProfile = new List<Sal_UnusualAllowanceEntity>();

            for (int i = 0; i < ListProfileID.Count(); i++)
            {
                ListUnusualAllowanceByProfile = ListUnusualAllowance.Where(m => m.ProfileID == ListProfileID[i]).ToList();
                DataRow row = Table.NewRow();
                row["STT"] = i + 1;
                row[ConstantDisplay.HRM_Sal_HoldSalary_CodeEmp.TranslateString()] = ListUnusualAllowanceByProfile[0].CodeEmp;
                row[ConstantDisplay.HRM_HR_Relatives_ProfileName.TranslateString()] = ListUnusualAllowanceByProfile[0].ProfileName;
                row[ConstantDisplay.HRM_Hre_Report_E_UNIT.TranslateString()] = ListUnusualAllowanceByProfile[0].E_UNIT;
                row[ConstantDisplay.HRM_Hre_Report_E_DIVISION.TranslateString()] = ListUnusualAllowanceByProfile[0].E_DIVISION;
                row[ConstantDisplay.HRM_Hre_Report_E_DEPARTMENT.TranslateString()] = ListUnusualAllowanceByProfile[0].E_DEPARTMENT;
                row[ConstantDisplay.HRM_Hre_Report_E_TEAM.TranslateString()] = ListUnusualAllowanceByProfile[0].E_TEAM;
                row[ConstantDisplay.HRM_Hre_Report_E_SECTION.TranslateString()] = ListUnusualAllowanceByProfile[0].E_SECTION;
                row[ConstantDisplay.HRM_Payroll_UnusualEDChild_ChildCount.TranslateString()] = ListUnusualAllowanceByProfile.Count();
                row[ConstantDisplay.HRM_Recruitment_UnusualAllowance_Amount.TranslateString()] = ListUnusualAllowanceByProfile.Count() * 110000;
                //Hiện tại task ghi ko rõ ràng nên làm theo ý là sum lại [Hien.Nguyen] 
                row[ConstantDisplay.HRM_Recruitment_UnusualAllowance_AmountofOffset.TranslateString()] = ListUnusualAllowanceByProfile.Sum(m => m.AmountOfOffSet);
                string Note = string.Empty;
                ListUnusualAllowanceByProfile.ForEach(m => Note = m.Notes + ",");
                row[ConstantDisplay.HRM_Sal_InsuranceSalry_Note.TranslateString()] = Note;
                Table.Rows.Add(row);
            }
            #endregion
            return Table;
        }

        /// <summary>
        /// Báo cáo tổng danh sách hưởng phụ cấp.
        /// </summary>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        /// <param name="OrgTructureIds"></param>
        /// <param name="IsCreateTemplate"></param>
        /// <returns></returns>
        public DataTable ReportTotalUnusualAllowance(DateTime datestart, DateTime dateend, string OrgTructureIds, string UserLogin, bool IsCreateTemplate)
        {
            #region Get Data
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowanceGroupRelative = new List<Sal_UnusualAllowanceEntity>();

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgTructureIds;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[2] = "E_APPROVED";
            listModel[3] = datestart;
            listModel[4] = dateend;
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare, UserLogin, ref status).OrderBy(m => m.ProfileID).Where(m => ListProfile.Any(t => t.ID == m.ProfileID)).ToList();

            #endregion

            DataTable Table = new DataTable("ReportTotalUnusualAllowance");
            Table.Columns.Add("STT");
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.CodeEmp);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.ProfileName);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_UNIT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_TEAM);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_SECTION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.DateHire, typeof(DateTime));
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.MonthStart, typeof(DateTime));
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.YearOfBirth, typeof(string));
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.RelativeNumber);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.Amount, typeof(double));
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.AmountOfOffSet, typeof(double));
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.Notes);

            if (IsCreateTemplate)
            {
                return Table;
            }

            Guid[] listProfilieIDs = ListUnusualAllowance.Select(m => m.ProfileID).Distinct().ToArray();
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowanceByProfile = new List<Sal_UnusualAllowanceEntity>();
            List<Sal_UnusualAllowanceEntity> CountChild = new List<Sal_UnusualAllowanceEntity>();
            int index = 1;
            for (int i = 0; i < listProfilieIDs.Count(); i++)
            {
                ListUnusualAllowanceByProfile = ListUnusualAllowance.Where(m => m.ProfileID == listProfilieIDs[i]).OrderBy(m => m.MonthStart).ToList();
                foreach (var item in ListUnusualAllowanceByProfile)
                {
                    DataRow Row = Table.NewRow();
                    Row["STT"] = index++;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.ProfileName] = item.ProfileName;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.YearOfBirth] = item.YearOfBirth;
                    if (ListUnusualAllowance[i].DateHire != null)
                    {
                        Row[Sal_UnusualAllowanceEntity.FieldNames.DateHire] = item.DateHire;
                    }
                    if (ListUnusualAllowance[i].MonthStart != null)
                    {
                        Row[Sal_UnusualAllowanceEntity.FieldNames.MonthStart] = item.MonthStart;
                    }
                    //CountChild = ListUnusualAllowanceByProfile.Where(m => m.MonthStart <= item.MonthStart).ToList();
                    CountChild = ListUnusualAllowanceByProfile.GetRange(0, ListUnusualAllowanceByProfile.IndexOf(item) + 1);
                    Row[Sal_UnusualAllowanceEntity.FieldNames.RelativeNumber] = CountChild.Count();
                    Row[Sal_UnusualAllowanceEntity.FieldNames.Amount] = CountChild.Sum(m => m.Amount != null ? m.Amount : 0);
                    Row[Sal_UnusualAllowanceEntity.FieldNames.AmountOfOffSet] = CountChild.Sum(m => m.AmountOfOffSet != null ? m.AmountOfOffSet : 0);
                    Row[Sal_UnusualAllowanceEntity.FieldNames.Notes] = item.Notes;
                    Table.Rows.Add(Row);
                }
            }

            return Table;
        }

        /// <summary>
        /// Danh Sách Từng Loại Hiếu Hỉ
        /// </summary>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        /// <param name="OrgTructureIds"></param>
        /// <returns></returns>
        public DataTable ReportFilialWedding(DateTime datestart, DateTime dateend, string OrgTructureIds, Guid? _UnusualEDTypeID, bool isIncludeQuitEmp, string UserLogin, bool IsCreateTemplate)
        {
            #region Getdata
            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgTructureIds;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding, UserLogin, ref status);
            ListUnusualAllowance = ListUnusualAllowance.Where(m => m.MonthStart >= datestart && m.MonthStart <= dateend && m.UnusualEDTypeID == _UnusualEDTypeID).ToList();

            //loc nv nghi viec
            if (isIncludeQuitEmp == null || isIncludeQuitEmp == false)
            {
                ListProfile = ListProfile.Where(m => m.DateQuit == null || m.DateQuit > dateend).ToList();
            }

            #endregion

            #region Progess
            //Create column
            DataTable Table = new DataTable("ReportFilialWedding");
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("E_UNIT");
            Table.Columns.Add("E_DIVISION");
            Table.Columns.Add("E_DEPARTMENT");
            Table.Columns.Add("E_TEAM");
            Table.Columns.Add("E_SECTION");
            Table.Columns.Add("PaymentDate", typeof(DateTime));
            Table.Columns.Add("UnusualEDTypeID");
            Table.Columns.Add("RelativeName");
            Table.Columns.Add("DateOccur", typeof(DateTime));
            Table.Columns.Add("RelativeTypeName");
            Table.Columns.Add("Amount", typeof(double));
            Table.Columns.Add("Notes");

            if (IsCreateTemplate)
            {
                return Table;
            }

            for (int i = 0; i < ListUnusualAllowance.Count; i++)
            {
                DataRow Row = Table.NewRow();
                Row["STT"] = i + 1;
                Row["CodeEmp"] = ListUnusualAllowance[i].CodeEmp;
                Row["ProfileName"] = ListUnusualAllowance[i].ProfileName;
                Row["E_UNIT"] = ListUnusualAllowance[i].E_UNIT;
                Row["E_DIVISION"] = ListUnusualAllowance[i].E_DIVISION;
                Row["E_DEPARTMENT"] = ListUnusualAllowance[i].E_DEPARTMENT;
                Row["E_TEAM"] = ListUnusualAllowance[i].E_TEAM;
                Row["E_SECTION"] = ListUnusualAllowance[i].E_SECTION;
                if (ListUnusualAllowance[i].MonthStart != null)
                {
                    Row["PaymentDate"] = ListUnusualAllowance[i].MonthStart;
                }

                Row["UnusualEDTypeID"] = ListUnusualAllowance[i].UnusualAllowanceCfgName;
                Row["RelativeName"] = ListUnusualAllowance[i].RelativeName;
                if (ListUnusualAllowance[i].DateOccur != null)
                {
                    Row["DateOccur"] = ListUnusualAllowance[i].DateOccur;
                }
                Row["RelativeTypeName"] = ListUnusualAllowance[i].RelativeTypeName;
                if (ListUnusualAllowance[i].Amount != null)
                {
                    Row["Amount"] = ListUnusualAllowance[i].Amount;
                }
                Row["Notes"] = ListUnusualAllowance[i].Notes;
                Table.Rows.Add(Row);
            }
            #endregion

            return Table;
        }

        /// <summary>
        /// Danh Sách Tổng Hợp Nhân Viên Hưởng Chế Độ Hiếu Hỉ
        /// </summary>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        /// <returns></returns>
        public DataTable ReportTotalProfileUnusualAllowance(DateTime MonthStart, DateTime MonthEnd, string OrgStructure, bool isIncludeQuitEmp, string UserLogin, bool IsCreateTemplate)
        {
            #region Create column
            DataTable Table = new DataTable("ReportTotalProfileUnusualAllowance");
            Table.Columns.Add("STT");
            Table.Columns.Add(ConstantDisplay.HRM_Training_Category.TranslateString());

            //Biến lưu lại tên column động theo thời gian
            List<string> listColumnName = new List<string>();
            List<DateTime> listDate = new List<DateTime>();

            DateTime datestart = new DateTime(MonthStart.Year, MonthStart.Month, MonthStart.Day);
            DateTime dateend = new DateTime(MonthEnd.Year, MonthEnd.Month, MonthEnd.Day);

            while (datestart <= dateend)
            {
                //Lưu biến vào date
                listDate.Add(datestart);

                //Lọc tên column
                string Column1 = "Month" + (listDate.IndexOf(datestart) + 1) + "_SL";
                string Column2 = "Month" + (listDate.IndexOf(datestart) + 1) + "_Amount";

                //Add vào list lưu column lại
                listColumnName.Add(Column1);
                listColumnName.Add(Column2);

                //Create column
                Table.Columns.Add(Column1);
                Table.Columns.Add(Column2);

                //Tăng datestart lên 1 tháng
                datestart = datestart.AddMonths(1);
            }

            if (IsCreateTemplate)
            {
                return Table;
            }
            #endregion


            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure;
            listModel[16] = 1;
            listModel[17] = int.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = MonthStart;
            listModel[4] = MonthEnd;
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding, UserLogin, ref status);
            ListUnusualAllowance = ListUnusualAllowance.Where(m => m.TypeUnusual == "E_ExcludePayslip" && m.IsExcludePayslip == true).ToList();

            //List<string> listUnusualAllowanceCfgCode = new List<string>();
            //listUnusualAllowanceCfgCode.Add("BIRTH OF A CHILD");
            //listUnusualAllowanceCfgCode.Add("DEATH OF EMPLOYEE CHILD");
            //listUnusualAllowanceCfgCode.Add("DEATH OF EMPLOYEE SPOUSE");
            //listUnusualAllowanceCfgCode.Add("FUNERAL OF PARENT");
            //listUnusualAllowanceCfgCode.Add("MARRIAGE OF EMPLOYEE");
            //listUnusualAllowanceCfgCode.Add("DEATH OF EMPLOYEE");

            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            List<Cat_UnusualAllowanceCfgEntity> ListUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).Where(m => m.IsExcludePayslip == true).ToList();

            //lọc các phụ cấp thuộc loại cần lấy
            ListUnusualAllowance = ListUnusualAllowance.Where(m => ListUnusualAllowanceCfg.Any(t => t.ID == m.UnusualEDTypeID)).ToList();

            //loại trự nv nghỉ việc
            if (!isIncludeQuitEmp)
            {
                ListProfile = ListProfile.Where(m => m.DateQuit == null || m.DateQuit >= dateend).ToList();
                ListUnusualAllowance = ListUnusualAllowance.Where(m => ListProfile.Any(t => t.ID == m.ProfileID)).ToList();
            }

            //Conver lại datetime cho dễ xử lý
            MonthStart = new DateTime(MonthStart.Year, MonthStart.Month, 1);
            MonthEnd = new DateTime(MonthEnd.Year, MonthEnd.Month, 1);

            //Duyệt qua các loại hiếu hỷ
            for (int i = 0; i < ListUnusualAllowanceCfg.Count; i++)
            {
                DataRow row = Table.NewRow();
                row["STT"] = i + 1;
                row[ConstantDisplay.HRM_Training_Category.TranslateString()] = ListUnusualAllowanceCfg[i].UnusualAllowanceCfgName;
                foreach (var j in listDate)
                {
                    var ListUnusualAllowanceForDate = ListUnusualAllowance.Where(m => m.UnusualEDTypeID == ListUnusualAllowanceCfg[i].ID && m.MonthStart != null && new DateTime(m.MonthStart.Value.Year, m.MonthStart.Value.Month, 1) == j).ToList();
                    row["Month" + (listDate.IndexOf(j) + 1) + "_SL"] = ListUnusualAllowanceForDate.Count();
                    row["Month" + (listDate.IndexOf(j) + 1) + "_Amount"] = ListUnusualAllowanceForDate.Sum(m => m.Amount);
                }
                Table.Rows.Add(row);
            }
            return Table;
        }

        /// <summary>
        /// Danh Sách Chi Tiết Nhân Viên Hưởng Chế Độ Hiếu Hỉ
        /// </summary>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        /// <returns></returns>
        public DataTable ReportProfileEntitledAllowance(DateTime datestart, DateTime dateend, List<Guid> ListProfileIds, string OrgStructure, string UserLogin, bool isIncludeQuitEmp)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = datestart;
            listModel[4] = dateend;
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> ListUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualAllowanceFilialWedding, UserLogin, ref status).Where(m => m.IsExcludePayslip == true).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[17]);
            listModel[2] = OrgStructure;
            listModel[15] = 1;
            listModel[16] = int.MaxValue - 1;
            List<Hre_ProfileEntity> ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            List<Cat_UnusualAllowanceCfgEntity> ListUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status);

            //loại trự nv nghỉ việc
            if (!isIncludeQuitEmp)
            {
                ListProfile = ListProfile.Where(m => m.DateQuit == null || m.DateQuit >= dateend).ToList();
                ListUnusualAllowance = ListUnusualAllowance.Where(m => ListProfile.Any(t => t.ID == m.ProfileID)).ToList();
            }

            //lọc theo nhân viên
            if (ListProfileIds != null && ListProfileIds.Count > 0)
            {
                ListUnusualAllowance = ListUnusualAllowance.Where(m => ListProfileIds.Any(t => t == m.ProfileID)).ToList();
            }

            //Create column
            DataTable Table = new DataTable("ReportProfileEntitledAllowance");
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("Department");
            Table.Columns.Add("PaymentDate", typeof(DateTime));
            Table.Columns.Add("TypeMode");
            Table.Columns.Add("WhoIncurredRegime");
            Table.Columns.Add("Relative");
            Table.Columns.Add("DateBorn", typeof(DateTime));
            Table.Columns.Add("Amount", typeof(double));
            Table.Columns.Add("Noted");

            Hre_ProfileEntity ProfileItem = new Hre_ProfileEntity();
            Cat_UnusualAllowanceCfgEntity ListUnusualAllowanceCfgItem = new Cat_UnusualAllowanceCfgEntity();
            for (int i = 0; i < ListUnusualAllowance.Count; i++)
            {
                ProfileItem = ListProfile.Where(m => m.ID == ListUnusualAllowance[i].ProfileID).FirstOrDefault();
                if (ProfileItem == null)
                {
                    continue;
                }
                DataRow row = Table.NewRow();
                row["STT"] = i + 1;

                if (ProfileItem != null)
                {
                    row["CodeEmp"] = ProfileItem.CodeEmp;
                    row["ProfileName"] = ProfileItem.ProfileName;
                    row["Department"] = ProfileItem.OrgStructureName;
                }
                else
                {
                    row["CodeEmp"] = "";
                    row["ProfileName"] = "";
                    row["Department"] = "";
                }
                ListUnusualAllowanceCfgItem = ListUnusualAllowanceCfg.Where(m => m.ID == ListUnusualAllowance[i].UnusualEDTypeID).FirstOrDefault();
                if (ListUnusualAllowanceCfgItem != null)
                {
                    row["TypeMode"] = ListUnusualAllowanceCfgItem.Code;
                }
                if (ListUnusualAllowance[i].MonthStart != null)
                {
                    row["PaymentDate"] = ListUnusualAllowance[i].MonthStart;
                }

                row["WhoIncurredRegime"] = ListUnusualAllowance[i].RelativeName;
                row["Relative"] = ListUnusualAllowance[i].RelativeTypeName;
                if (ListUnusualAllowance[i].DateOccur != null)
                {
                    row["DateBorn"] = ListUnusualAllowance[i].DateOccur;
                }
                if (ListUnusualAllowance[i].Amount != null)
                {
                    row["Amount"] = ListUnusualAllowance[i].Amount;
                }

                row["Noted"] = ListUnusualAllowance[i].Notes;
                Table.Rows.Add(row);
            }
            return Table;
        }

        /// <summary>
        /// Bổ sung Báo cáo chi tiết tổng thu nhập lương của 1 người/ năm
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="YearStart"></param>
        /// <param name="YearEnd"></param>
        /// <returns></returns>
        public DataTable ReportTotalIncomeByProfile(Guid ProfileID, DateTime YearStart, DateTime YearEnd, Guid GradeID, string UserLogin)
        {

            #region Get Data

            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[0] = Common.DotNetToOracle(ProfileID.ToString());
            listModel[4] = YearStart;
            listModel[5] = YearEnd;
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_PayrollTableItemEntity> ListPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[3] = Common.DotNetToOracle(GradeID.ToString());
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            List<Cat_ElementEntity> ListElement = GetData<Cat_ElementEntity>(listModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status);

            Hre_ProfileEntity Profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();

            #endregion

            #region Progress

            //create columns
            DataTable Table = new DataTable("ReportTotalIncomeByProfile");
            Table.Columns.Add("Year");
            Table.Columns.Add("Month");
            Table.Columns.Add("Code");
            if (ListElement.Count > 0 && ListElement != null)
            {
                foreach (var elementItem in ListElement)
                {
                    Table.Columns.Add(elementItem.ElementCode);
                }

                //update lại year để tiện so sánh
                YearStart = new DateTime(YearStart.Year, 1, 1);
                YearEnd = new DateTime(YearEnd.Year, 12, 1);

                while (YearStart <= YearEnd)
                {
                    DataRow Row = Table.NewRow();
                    Row["Year"] = YearStart.Year;
                    Row["Month"] = YearStart.Month;
                    Row["Code"] = Profile.CodeEmp;

                    //lọc ra dữ liệu trong tháng và remove đi trong list tổng để tối ưu tốc độ
                    List<Sal_PayrollTableItemEntity> listPayrollTableItemByMonth = ListPayrollTableItem.Where(m => m.MonthYear.Value.Year == YearStart.Year && m.MonthYear.Value.Month == YearStart.Month).ToList();
                    ListPayrollTableItem.RemoveRange(listPayrollTableItemByMonth);

                    foreach (var elementItem in ListElement)
                    {
                        var ValueItem = listPayrollTableItemByMonth.Where(m => m.Code == elementItem.ElementCode).FirstOrDefault();
                        if (ValueItem != null)
                        {
                            Row[elementItem.ElementCode] = ValueItem.Value;
                        }
                        else
                        {
                            Row[elementItem.ElementCode] = "";
                        }
                    }
                    Table.Rows.Add(Row);

                    //tăng year start lên
                    YearStart = YearStart.AddMonths(1);
                }
            }
            #endregion
            return Table;
        }

        public DataTable CreateColumn_ReportAllowanceStopWorking(DateTime datetime)
        {
            //Biến lưu cột tháng
            List<DateTime> listColumns = new List<DateTime>();
            DataTable Table = new DataTable("ReportAllowanceStopWorking");
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("Name");
            Table.Columns.Add("Dept");
            Table.Columns.Add("From");
            Table.Columns.Add("To");
            Table.Columns.Add("ResignationDate");
            for (int i = 1; i <= 6; i++)
            {
                listColumns.Add(datetime.AddMonths(i * (-1)));
                //Table.Columns.Add(datetime.AddMonths(i * (-1)).ToString("dd/MM/yyyy"));
                Table.Columns.Add("Month" + (listColumns.IndexOf(datetime.AddMonths(i * (-1))) + 1).ToString());
            }
            return Table;
        }

        /// <summary>
        /// Bổ sung báo cáo Danh sách trợ cấp thôi việc
        /// </summary>
        /// <param name="ListProfile"></param>
        /// <returns></returns>
        public DataTable ReportAllowanceStopWorking(List<Hre_ProfileEntity> ListProfile, DateTime MonthStopWorking, string ElementCode, bool IsCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var RepoPayrollTableIem = new CustomBaseRepository<Sal_PayrollTableItem>(unitOfWork);

                string status = string.Empty;
                List<object> listModel = new List<object>();

                DataTable Table = new DataTable();
                if (IsCreateTemplate)
                {
                    Table = CreateColumn_ReportAllowanceStopWorking(MonthStopWorking);
                    return Table;
                }

                //lọc ra các nhân viên đủ điều kiện
                ListProfile = ListProfile.Where(m => m.DateHire <= new DateTime(2008, 12, 31) && m.DateQuit != null && (m.DateQuit.Value.Month == MonthStopWorking.Month && m.DateQuit.Value.Year == MonthStopWorking.Year)).ToList();

                //Biến lưu cột tháng
                List<DateTime> listColumns = new List<DateTime>();

                Table = new DataTable("ReportAllowanceStopWorking");
                Table.Columns.Add("STT");
                Table.Columns.Add("CodeEmp");
                Table.Columns.Add("Name");
                Table.Columns.Add("E_UNIT");
                Table.Columns.Add("E_DIVISION");
                Table.Columns.Add("E_DEPARTMENT");
                Table.Columns.Add("E_TEAM");
                Table.Columns.Add("E_SECTION");
                // Table.Columns.Add("Dept");
                Table.Columns.Add("From");
                Table.Columns.Add("To");
                Table.Columns.Add("ResignationDate");
                for (int i = 1; i <= 6; i++)
                {
                    listColumns.Add(MonthStopWorking.AddMonths(i * (-1)));

                    Table.Columns.Add("Month" + (listColumns.IndexOf(MonthStopWorking.AddMonths(i * (-1))) + 1).ToString());
                }

                for (int i = 0; i < ListProfile.Count; i++)
                {
                    DataRow Row = Table.NewRow();
                    Row["STT"] = i + 1;
                    Row["CodeEmp"] = ListProfile[i].CodeEmp;
                    Row["Name"] = ListProfile[i].ProfileName;
                    Row["E_UNIT"] = ListProfile[i].E_UNIT;
                    Row["E_DIVISION"] = ListProfile[i].E_DIVISION;
                    Row["E_DEPARTMENT"] = ListProfile[i].E_DEPARTMENT;
                    Row["E_TEAM"] = ListProfile[i].E_TEAM;
                    Row["E_SECTION"] = ListProfile[i].E_SECTION;
                    Row["From"] = ListProfile[i].DateHire;
                    Row["To"] = new DateTime(2008, 12, 31);
                    Row["ResignationDate"] = MonthStopWorking;

                    double Value = 0;
                    foreach (var Month in listColumns)
                    {
                        Value = 0;
                        Sal_PayrollTableItem PayrollTableByMonth = RepoPayrollTableIem.FindBy(m => m.IsDelete != true && m.MonthYear != null && m.MonthYear == Month && m.Code.ReplaceSpace() == ElementCode.ReplaceSpace()).FirstOrDefault();
                        if (PayrollTableByMonth != null)
                        {
                            double.TryParse(PayrollTableByMonth.Value, out Value);
                        }
                        Row["Month" + (listColumns.IndexOf(Month) + 1).ToString()] = Value;
                    }
                    Table.Rows.Add(Row);
                }

                return Table;

            }
        }

        /// <summary>
        /// Báo cáo tổng thu nhập chịu thuế năm
        /// </summary>
        /// <param name="ProfileIds"></param>
        /// <param name="CutOffduration"></param>
        /// <returns></returns>
        public DataTable ReportTotalAnnualTaxableIncome(DateTime Year, List<string> WorkingPlace, List<string> ProfileIDs, string OrgStructure, List<string> UnusualAllowanceCfg, string UserLogin, bool IsCreateTemplate)
        {
            Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
            DateTime DateClose = Sys_Services.GetConfigValue<DateTime>(AppConfig.HRM_SAL_DATECLOSE_ALLOWANCE);
            DateClose = new DateTime(Year.Year, DateTime.Now.Month, DateClose.Day);

            DataTable Table = new DataTable("ReportTotalAnnualTaxableIncome");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("CostCentre");
            foreach (var i in UnusualAllowanceCfg)
            {
                if (!Table.Columns.Contains(i))
                {
                    Table.Columns.Add(i);
                }
            }
            Table.Columns.Add("Remark");

            string status = string.Empty;
            List<object> listModel = new List<object>();
            DateTime YearStart = new DateTime(Year.Year, 1, 1);
            DateTime YearEnd = new DateTime(Year.Year, 12, 31);

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure != string.Empty ? OrgStructure : null;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).Where(m => m.DateQuit == null || m.DateQuit >= DateClose).ToList();

            if (ProfileIDs.Count > 0)
            {
                //lọc theo workingPlace
                listProfile = listProfile.Where(m => ProfileIDs.Contains(m.ID.ToString())).ToList();
            }

            if (WorkingPlace.Count > 0)
            {
                //lọc theo workingPlace
                listProfile = listProfile.Where(m => m.WorkPlaceID != null && WorkingPlace.Contains(m.WorkPlaceID.ToString())).ToList();
            }

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[3] = YearStart;
            listModel[4] = YearEnd;
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> listUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status).Where(m => UnusualAllowanceCfg.Contains(m.UnusualEDTypeCode)).ToList();

            foreach (var profile in listProfile)
            {
                DataRow Row = Table.NewRow();
                Row["CodeEmp"] = profile.CodeEmp;
                Row["ProfileName"] = profile.ProfileName;
                Row["CostCentre"] = profile.CostCentreCode;
                string Remark = string.Empty;
                foreach (var i in UnusualAllowanceCfg)
                {
                    var listUnusualAllowanceByProfile = listUnusualAllowance.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeCode == i).ToList();
                    Row[i] = listUnusualAllowanceByProfile.Sum(m => m.Amount);
                    listUnusualAllowanceByProfile.ForEach(m => Remark += m.Notes);
                }
                Row["Remark"] = Remark;
                Table.Rows.Add(Row);
            }
            return Table;
        }

        public DataTable Kai_ReportPaymentoutList(List<Kai_ReportPaymentoutEntity> ListProfile, DateTime MonthStart, DateTime MonthEnd, bool IsCreateTemplate)
        {



            DataTable Table = new DataTable("Kai_ReportPaymentoutSearchModel");
            #region Create Columns
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("E_UNIT");
            Table.Columns.Add("E_DIVISION");
            Table.Columns.Add("E_DEPARTMENT");
            Table.Columns.Add("E_TEAM");
            Table.Columns.Add("E_SECTION");
            Table.Columns.Add("AMOUNT");
            Table.Columns.Add("Note");

            if (IsCreateTemplate)
            {
                return Table;
            }

            #endregion


            for (int i = 0; i < ListProfile.Count; i++)
            {
                DataRow Row = Table.NewRow();
                Row["STT"] = i + 1;
                Row["CodeEmp"] = ListProfile[i].CodeEmp;
                Row["ProfileName"] = ListProfile[i].ProfileName;
                Row["E_UNIT"] = ListProfile[i].E_UNIT;
                Row["E_DIVISION"] = ListProfile[i].E_DIVISION;
                Row["E_DEPARTMENT"] = ListProfile[i].E_DEPARTMENT;
                Row["E_TEAM"] = ListProfile[i].E_TEAM;
                Row["E_SECTION"] = ListProfile[i].E_SECTION;
                Row["AMOUNT"] = ListProfile[i].AMOUNT;
                Row["Note"] = ListProfile[i].Note;

                Table.Rows.Add(Row);
            }

            return Table;
        }

        /// <summary>
        /// Báo cáo dữ liệu hoạch toán lương
        /// </summary>
        /// <param name="CutOffDuration"></param>
        /// <param name="GradePayroll"></param>
        /// <param name="CosCentre"></param>
        /// <returns></returns>
        public DataTable ReportPlanningPayroll(Guid CutOffDurationID, string CostCentre, string ElementCode, string OrgStructure, List<String> WorkingPlace, List<String> SalaryClassName, string StatusSyn, bool isIncludeQuitEmp, string UserLogin, bool IsCreateTemplate)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();


            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            List<Cat_CostCentreEntity> listCostCentre = GetData<Cat_CostCentreEntity>(listModel, ConstantSql.hrm_cat_sp_get_CostCentre, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[3]);
            listModel[1] = 1;
            listModel[2] = Int32.MaxValue - 1;
            List<Cat_SalaryClassEntity> listSalaryClass = GetData<Cat_SalaryClassEntity>(listModel, ConstantSql.hrm_cat_sp_get_SalaryClass, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[1] = Common.DotNetToOracle(CutOffDurationID.ToString());
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status);

            Att_CutOffDurationEntity CutoffDuration = (Att_CutOffDurationEntity)GetFirstData<Att_CutOffDurationEntity>(Common.DotNetToOracle(CutOffDurationID.ToString()), ConstantSql.hrm_att_sp_get_CutOffDurationById, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure != string.Empty ? OrgStructure : null;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            //listModel = new List<object>();
            //listModel.AddRange(new object[10]);
            //listModel[5] = CutoffDuration.DateStart;
            //listModel[6] = CutoffDuration.DateEnd;
            //listModel[8] = 1;
            //listModel[9] = Int32.MaxValue - 1;
            //List<Sal_BasicSalaryEntity> listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin,ref status);

            //loc nhan vien nghi viec
            if (!isIncludeQuitEmp)
            {
                if (CutoffDuration != null)
                {
                    Att_CutOffDurationEntity _tmpModel = (Att_CutOffDurationEntity)CutoffDuration;
                    listProfile = listProfile.Where(m => m.DateQuit == null || m.DateQuit > _tmpModel.MonthYear).ToList();
                }
            }

            //loc theo noi lam viec
            if (WorkingPlace != null && WorkingPlace.Count > 0)
            {
                listProfile = listProfile.Where(m => WorkingPlace.Any(t => t == m.WorkPlaceID.ToString())).ToList();
            }

            //lọc theo salary class
            if (SalaryClassName != null && SalaryClassName.Count > 0)
            {
                listProfile = listProfile.Where(m => SalaryClassName.Contains(m.SalaryClassID.ToString())).ToList();
            }

            if (StatusSyn != null && StatusSyn != string.Empty)
            {
                listProfile = listProfile.Where(m => m.StatusSyn == StatusSyn).ToList();
            }

            //lấy costcenter được chọn
            if (CostCentre != null && CostCentre != string.Empty)
            {
                string[] ArrCostCentre = CostCentre.Split(',').ToArray();
                listCostCentre = listCostCentre.Where(m => ArrCostCentre.Any(t => t == m.ID.ToString())).ToList();
            }
            //lấy phần tử
            List<string> ListElementCode = new List<string>();
            if (ElementCode != null && ElementCode != string.Empty)
            {
                ListElementCode = ElementCode.Split(',').ToList();
            }

            DataTable Table = new DataTable("ReportPlanningPayroll");
            Table.Columns.Add("CostCentre");
            Table.Columns.Add("SalaryClass");
            //Table.Columns.Add("ProfileName");
            Table.Columns.Add("CodeEmp");
            foreach (var elementItem in ListElementCode)
            {
                if (elementItem != "PCANTRUA")
                {
                    Table.Columns.Add(elementItem, typeof(double));
                }
                else
                {
                    //Xử lý riêng cho HVN , nếu là phần tử PCANTRUA thì tách ra 1 cột là tiền dương và 1 cột là tiền âm
                    Table.Columns.Add(elementItem, typeof(double));
                    Table.Columns.Add("Minus_" + elementItem, typeof(double));
                }

            }


            if (IsCreateTemplate)
            {
                return Table;
            }

            List<Hre_ProfileEntity> ListProfileByCostCentre = new List<Hre_ProfileEntity>();
            List<Sal_PayrollTableItemEntity> listPayrollTableItemByProfile = new List<Sal_PayrollTableItemEntity>();
            double Value = 0;
            double _tmp = 0;
            foreach (var item in listCostCentre)
            {
                foreach (var salaryClassItem in listSalaryClass)
                {
                    //lấy nhân viên theo costcentre
                    ListProfileByCostCentre = listProfile.Where(m => m.CostCentreID == item.ID && m.SalaryClassID == salaryClassItem.ID).ToList();
                    if (ListProfileByCostCentre.Count > 0)
                    {
                        DataRow Row = Table.NewRow();
                        Row["CostCentre"] = item.CostCentreName;
                        Row["SalaryClass"] = salaryClassItem.SalaryClassName;
                        Row["CodeEmp"] = ListProfileByCostCentre.Count;
                        foreach (var elementItem in ListElementCode)
                        {
                            if (elementItem != "PCANTRUA")
                            {
                                //reset value
                                Value = 0;
                                //lọc ra bảng lương theo mã phần tử và theo nhân viên
                                listPayrollTableItemByProfile = listPayrollTableItem.Where(m => m.Code == elementItem && ListProfileByCostCentre.Any(t => t.ID == m.ProfileID)).ToList();

                                if (listPayrollTableItemByProfile.Count > 0)
                                {
                                    foreach (var i in listPayrollTableItemByProfile)
                                    {
                                        _tmp = 0;
                                        double.TryParse(i.Value, out _tmp);
                                        Value += _tmp;
                                    }
                                }
                                Row[elementItem] = Value;
                            }
                            else//xử lý riêng cho HVN
                            {
                                //reset value
                                Value = 0;
                                double Value_Minus = 0;
                                //lọc ra bảng lương theo mã phần tử và theo nhân viên
                                listPayrollTableItemByProfile = listPayrollTableItem.Where(m => m.Code == elementItem && ListProfileByCostCentre.Any(t => t.ID == m.ProfileID)).ToList();

                                if (listPayrollTableItemByProfile.Count > 0)
                                {
                                    foreach (var i in listPayrollTableItemByProfile)
                                    {
                                        _tmp = 0;
                                        double.TryParse(i.Value, out _tmp);
                                        if (_tmp >= 0)
                                        {
                                            Value += _tmp;
                                        }
                                        else
                                        {
                                            Value_Minus += (_tmp * -1);
                                        }
                                    }
                                }
                                Row[elementItem] = Value;
                                Row["Minus_" + elementItem] = Value_Minus;
                            }
                        }
                        Table.Rows.Add(Row);
                    }
                }
            }
            return Table;
        }

        /// <summary>
        /// Báo Cáo danh sách Không Trả Phụ Cấp Do Hold Lương
        /// </summary>
        /// <param name="Month"></param>
        /// <param name="Orgstructure"></param>
        /// <returns></returns>
        public DataTable ReportNotPayAllowancesByHoldSalary(DateTime MonthStart, DateTime MonthEnd, string Orgstructure, string UserLogin, bool IsCreateTemplate)
        {
            #region Create Column

            DataTable Table = new DataTable("ReportNotPayAllowancesByHoldSalary");
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("E_UNIT");
            Table.Columns.Add("E_DIVISION");
            Table.Columns.Add("E_DEPARTMENT");
            Table.Columns.Add("E_TEAM");
            Table.Columns.Add("E_SECTION");
            Table.Columns.Add("CurrenDate", typeof(DateTime));
            Table.Columns.Add("DateStartHoldSalary", typeof(DateTime));
            Table.Columns.Add("DateEndHoldSalary", typeof(DateTime));
            Table.Columns.Add("ChildNumber");
            Table.Columns.Add("AmountOfOffset");
            Table.Columns.Add("Note");

            if (IsCreateTemplate)
            {
                return Table;
            }

            #endregion

            string status = string.Empty;
            List<object> listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[17]);
            listModel[2] = Orgstructure;
            listModel[15] = 1;
            listModel[16] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[3] = MonthStart;
            listModel[4] = MonthEnd;
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> listUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            //listModel[4] = MonthStart;
            //listModel[5] = MonthEnd;
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status).Where(m => (m.MonthSalary <= MonthEnd && m.MonthEndSalary == null) || (m.MonthSalary <= MonthEnd && m.MonthEndSalary != null && m.MonthEndSalary >= MonthStart) && m.Status == EnumDropDown.HoldSalaryStatus.E_APPROVED.ToString()).ToList();


            string Notes = string.Empty;
            listProfile = listProfile.Where(m => listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();
            listProfile = listProfile.Where(m => listUnusualAllowance.Any(t => t.ProfileID == m.ID)).ToList();

            MonthStart = new DateTime(MonthStart.Year, MonthStart.Month, 1);
            MonthEnd = new DateTime(MonthEnd.Year, MonthEnd.Month, 1);
            while (MonthStart <= MonthEnd)
            {
                for (int i = 0; i < listProfile.Count; i++)
                {
                    Sal_HoldSalaryEntity listHoldSalaryByProfile = listHoldSalary.Where(m => m.ProfileID == listProfile[i].ID && ((m.MonthSalary <= MonthStart && m.MonthEndSalary == null) || (m.MonthSalary <= MonthStart && m.MonthEndSalary != null && m.MonthEndSalary >= MonthStart))).FirstOrDefault();
                    if (listHoldSalaryByProfile != null)
                    {
                        DataRow Row = Table.NewRow();
                        Notes = string.Empty;
                        Row["STT"] = i + 1;
                        Row["CodeEmp"] = listProfile[i].CodeEmp;
                        Row["ProfileName"] = listProfile[i].ProfileName;
                        Row["CurrenDate"] = MonthStart;
                        if (listHoldSalaryByProfile.MonthSalary != null)
                            Row["DateStartHoldSalary"] = listHoldSalaryByProfile.MonthSalary;
                        if (listHoldSalaryByProfile.MonthEndSalary != null)
                            Row["DateEndHoldSalary"] = listHoldSalaryByProfile.MonthEndSalary;
                        Row["E_UNIT"] = listProfile[i].E_UNIT;
                        Row["E_DIVISION"] = listProfile[i].E_DIVISION;
                        Row["E_DEPARTMENT"] = listProfile[i].E_DEPARTMENT;
                        Row["E_TEAM"] = listProfile[i].E_TEAM;
                        Row["E_SECTION"] = listProfile[i].E_SECTION;
                        Row["ChildNumber"] = listUnusualAllowance.Where(m => m.ProfileID == listProfile[i].ID && m.MonthStart <= MonthStart && m.MonthEnd >= MonthStart).Count();
                        Row["AmountOfOffset"] = listUnusualAllowance.Where(m => m.ProfileID == listProfile[i].ID && m.MonthStart <= MonthStart && m.MonthEnd >= MonthStart).Sum(m => m.AmountOfOffSet);
                        listUnusualAllowance.ForEach(m => Notes += m.Notes != null ? m.Notes : "");
                        Row["Note"] = Notes;
                        Table.Rows.Add(Row);
                    }

                }

                MonthStart = MonthStart.AddMonths(1);
            }
            return Table;
        }

        /// <summary>
        /// Bổ sung báo cáo Danh sách nhân viên không trừ được tiền
        /// </summary>
        /// <param name="ListProfile"></param>
        /// <param name="MonthYear"></param>
        /// <param name="IsCreateTemplate"></param>
        /// <returns></returns>
        public DataTable ReportProfileNotDeductMoney(List<Hre_ProfileEntity> listProfile, Att_CutOffDurationEntity CutOffDuration, string UserLogin, bool IsCreateTemplate)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Sal_UnusualAllowanceEntity> listUnusualAllowanceByCfg = new List<Sal_UnusualAllowanceEntity>();

            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[3] = CutOffDuration.DateStart;
            listModel[4] = CutOffDuration.DateEnd;
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> listUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status).Where(m => m.Type == HRM.Infrastructure.Utilities.EnumDropDown.UnusualEDType.E_UNUSUALALLOWANCE.ToString()).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[4] = CutOffDuration.DateStart;
            listModel[5] = CutOffDuration.DateEnd;
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status);

            DataTable Table = new DataTable("ReportAccountingOfClearing");
            Table.Columns.Add("STT");
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.CodeEmp);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgCode);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.Amount);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.EDTypeView);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_UNIT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_TEAM);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_SECTION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.WorkPlaceName);

            if (IsCreateTemplate)
            {
                return Table;
            }

            //lọc nhân viên có phụ cấp trong tháng
            //listProfile = listProfile.Where(m => listUnusualAllowance.Any(t => t.ProfileID == m.ID)).ToList();
            listProfile = listProfile.Where(m => listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();

            if (listProfile.Count > 0)
            {
                string WorkingPlaceName = listProfile[0].WorkingPlace;
                int i = 1;
                foreach (var UnusualCfg in listUnusualAllowanceCfg)
                {
                    if (UnusualCfg == null)
                        continue;
                    foreach (var profile in listProfile)
                    {
                        double amountUnusualAllow = 0;
                        DataRow Row = Table.NewRow();
                        Row["STT"] = i++;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgCode] = UnusualCfg.Code;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.EDTypeView] = UnusualCfg.EDTypeView;
                        var lstnusualAllowanceByProfile = listUnusualAllowance.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == UnusualCfg.ID && m.Amount > 0).ToList();
                        if (lstnusualAllowanceByProfile != null && lstnusualAllowanceByProfile.Count > 0)
                        {
                            amountUnusualAllow = lstnusualAllowanceByProfile.Sum(m => m.Amount).Value;
                            Row[Sal_UnusualAllowanceEntity.FieldNames.Amount] = amountUnusualAllow;
                        }

                        Row[Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName] = UnusualCfg.UnusualAllowanceCfgName;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                        Row[Sal_UnusualAllowanceEntity.FieldNames.WorkPlaceName] = profile.WorkingPlace;

                        if (amountUnusualAllow > 0)
                        {
                            Table.Rows.Add(Row);
                        }
                    }
                }
            }

            return Table;
        }
        /// <summary>
        /// BC hạch toán các khoản bù trừ
        /// </summary>
        /// <param name="MonthYear"></param>
        /// <returns></returns>
        public DataTable ReportAccountingOfClearing(Att_CutOffDurationEntity Cutoffduration, string OrgStructure, Guid? WorkPlace, string UserLogin, bool IsCreateTemplate)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Sal_UnusualAllowanceEntity> listUnusualAllowanceByCfg = new List<Sal_UnusualAllowanceEntity>();

            listModel = new List<object>();
            listModel.AddRange(new object[6]);
            listModel[3] = "E_UNUSUALALLOWANCE";
            listModel[4] = 1;
            listModel[5] = Int32.MaxValue - 1;
            List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(listModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[3] = Cutoffduration.DateStart;
            listModel[4] = Cutoffduration.DateEnd;
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_UnusualAllowanceEntity> listUnusualAllowance = GetData<Sal_UnusualAllowanceEntity>(listModel, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status);
            if (listUnusualAllowance != null)
                listUnusualAllowance = listUnusualAllowance.Where(s => s.Amount > 0).ToList();
            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            //lọc nhân viên theo nơi làm việc
            if (WorkPlace != null)
            {
                listProfile = listProfile.Where(m => m.WorkPlaceID == WorkPlace).ToList();
            }

            //lọc các loại phụ cấp theo nhân viên
            //listUnusualAllowance = listUnusualAllowance.Where(m => listProfile.Any(t => t.ID == m.ProfileID)).ToList();

            //lọc nhân viên có phụ cấp trong tháng
            listProfile = listProfile.Where(m => listUnusualAllowance.Any(t => t.ProfileID == m.ID)).ToList();

            DataTable Table = new DataTable("ReportAccountingOfClearing");
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.CodeEmp);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgCode);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.Amount);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.EDTypeView);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_UNIT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_TEAM);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.E_SECTION);
            Table.Columns.Add(Sal_UnusualAllowanceEntity.FieldNames.WorkPlaceName);

            if (IsCreateTemplate)
            {
                return Table;
            }
            string WorkingPlaceName = "";
            if (listProfile.Count > 0)
            {
                WorkingPlaceName = listProfile[0].WorkingPlace;
            }
            foreach (var UnusualCfg in listUnusualAllowanceCfg)
            {
                foreach (var profile in listProfile)
                {
                    DataRow Row = Table.NewRow();
                    Row[Sal_UnusualAllowanceEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgCode] = UnusualCfg.Code;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                    var UnusualAllowanceByProfile = listUnusualAllowance.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == UnusualCfg.ID).ToList();
                    if (UnusualAllowanceByProfile.Count > 0)
                    {
                        Row[Sal_UnusualAllowanceEntity.FieldNames.Amount] = UnusualAllowanceByProfile.Sum(m => m.Amount);
                    }
                    Row[Sal_UnusualAllowanceEntity.FieldNames.EDTypeView] = UnusualCfg.EDTypeView;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName] = UnusualCfg.UnusualAllowanceCfgName;
                    //Row[Sal_UnusualAllowanceEntity.FieldNames.WorkPlaceName] = profile.WorkingPlace;
                    Row[Sal_UnusualAllowanceEntity.FieldNames.WorkPlaceName] = profile.WorkPlaceName;
                    Table.Rows.Add(Row);
                }
            }
            return Table;
        }

        /// <summary>
        /// Báo cáo hạch toán tiền phép ốm
        /// </summary>
        /// <param name="listCosCentre"></param>
        /// <param name="ArrayElementCode"></param>
        /// <param name="IsCreateTemplate"></param>
        /// <returns></returns>
        public DataTable ReportAllowsAccountingOfSick(DateTime YearStart, string OrgStructure, List<string> ArrayElementCode, string StatusSync, List<Guid> listClassSalaryIDs, string WorkingPlaceIDs, string UserLogin, bool IsCreateTemplate)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByCostCentre = new List<Hre_ProfileEntity>();

            //listModel = new List<object>(); 
            //listModel.AddRange(new object[4]);
            //listModel[2] = 1;
            //listModel[3] = Int32.MaxValue - 1;
            //List<Cat_CostCentreEntity> listCostCentre = GetData<Cat_CostCentreEntity>(listModel, ConstantSql.hrm_cat_sp_get_CostCentre, UserLogin,ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[9]);
            listModel[2] = new DateTime(YearStart.Year, 1, 1);
            listModel[3] = new DateTime(YearStart.Year, 12, 31);
            listModel[7] = 1;
            listModel[8] = Int32.MaxValue - 1;
            List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).Where(m => ArrayElementCode.Any(t => t.ReplaceSpace() == m.Code.ReplaceSpace())).ToList();

            //Att_CutOffDurationEntity CutoffDuration = (Att_CutOffDurationEntity)GetFirstData<Att_CutOffDurationEntity>(Common.DotNetToOracle(CutOffDurationID.ToString()), ConstantSql.hrm_att_sp_get_CutOffDurationById, UserLogin,ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure;
            listModel[16] = 1;
            listModel[17] = Int32.MaxValue - 1;
            List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

            //lọc nhân viên theo trạng thái
            if (StatusSync != null && StatusSync != string.Empty)
            {
                listProfile = listProfile.Where(m => m.StatusSyn == StatusSync).ToList();
            }

            //lọc theo bật lương
            if (listClassSalaryIDs != null && listClassSalaryIDs.Count() > 0)
            {
                listProfile = listProfile.Where(m => listClassSalaryIDs.Any(t => t == m.SalaryClassID)).ToList();
            }

            //lọc theo nơi làm việc
            if (WorkingPlaceIDs != null)
            {
                string[] listWorking = WorkingPlaceIDs.Split(',');
                listProfile = listProfile.Where(m => m.WorkPlaceID != null && listWorking.Contains(m.WorkPlaceID.ToString())).ToList();
            }

            Guid[] listCostCentre = listProfile.Select(m => m.SalaryClassID != null ? (Guid)m.SalaryClassID : Guid.Empty).Where(m => m != Guid.Empty).ToArray();

            DataTable Table = new DataTable("ReportAllowsAccountingOfSick");
            Table.Columns.Add("SalaryClassName");
            Table.Columns.Add("CountOfEmpCode");
            foreach (var i in ArrayElementCode)
            {
                Table.Columns.Add(i, typeof(double));
            }

            if (IsCreateTemplate)
            {
                return Table;
            }

            foreach (var item in listCostCentre)
            {
                DataRow Row = Table.NewRow();
                Row["SalaryClassName"] = listProfile.Where(m => m.SalaryClassID == item).FirstOrDefault().SalaryClassName;
                listProfileByCostCentre = listProfile.Where(m => m.CostCentreID == item).ToList();
                Row["CountOfEmpCode"] = listProfileByCostCentre.Count();
                foreach (var CostItem in ArrayElementCode)
                {
                    double Amount = 0;
                    var ListValue = listPayrollTableItem.Where(m => m.Code == CostItem && listProfileByCostCentre.Any(t => t.ID == m.ProfileID)).ToList();
                    foreach (var payrollItem in ListValue)
                    {
                        double.TryParse(payrollItem.Value, out Amount);
                    }
                    Row[CostItem] = Amount;
                }
                Table.Rows.Add(Row);
            }
            return Table;
        }

        /// <summary>
        /// Báo Cáo Tổng Hợp Chuyển Tiền Kaizen Hàng Tháng
        /// </summary>
        /// <param name="MonthStart"></param>
        /// <param name="MonthEnd"></param>
        /// <param name="OrgStructure"></param>
        /// <returns></returns>
        public DataTable ReportKaizenMonthlyCash(List<Hre_ProfileEntity> listProfile, DateTime MonthStart, DateTime MonthEnd, string OrgStructure, string UserLogin, bool IsCreateTemplate, string ReportName)
        {
            MonthStart = new DateTime(MonthStart.Year, MonthStart.Month, 1);
            MonthEnd = new DateTime(MonthEnd.Year, MonthEnd.Month, 1);

            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Hre_ProfileEntity> listProfileByCostCentre = new List<Hre_ProfileEntity>();

            //listModel = new List<object>();
            //listModel.AddRange(new object[16]);
            //listModel[2] = OrgStructure;
            //listModel[14] = 1;
            //listModel[15] = Int32.MaxValue - 1;
            //List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin,ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[5]);
            listModel[3] = 1;
            listModel[4] = Int32.MaxValue - 1;
            List<Cat_OrgStructureEntity> listOrgStructure = GetData<Cat_OrgStructureEntity>(listModel, ConstantSql.hrm_cat_sp_get_OrgStructure, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[4]);
            listModel[2] = 1;
            listModel[3] = Int32.MaxValue - 1;
            List<Cat_OrgStructureTypeEntity> listOrgStructureType = GetData<Cat_OrgStructureTypeEntity>(listModel, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status);

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[2] = OrgStructure;
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            List<Kai_KaizenDataEntity> listKaiZen = GetData<Kai_KaizenDataEntity>(listModel, ConstantSql.hrm_Kai_sp_get_KaiZenData, UserLogin, ref status);

            DataTable Table = new DataTable(ReportName);
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("E_UNIT");
            Table.Columns.Add("E_DIVISION");
            Table.Columns.Add("E_DEPARTMENT");
            Table.Columns.Add("E_TEAM");
            Table.Columns.Add("E_SECTION");
            Table.Columns.Add("OrgStructureTypeName");
            Table.Columns.Add("Amount");
            Table.Columns.Add("Notes");

            if (IsCreateTemplate)
            {
                return Table;
            }

            for (int i = 0; i < listProfile.Count; i++)
            {
                DataRow Row = Table.NewRow();
                Row["STT"] = i + 1;
                Row["CodeEmp"] = listProfile[i].CodeEmp;
                Row["ProfileName"] = listProfile[i].ProfileName;
                Row["E_UNIT"] = listProfile[i].E_UNIT;
                Row["E_DIVISION"] = listProfile[i].E_DIVISION;
                Row["E_DEPARTMENT"] = listProfile[i].E_DEPARTMENT;
                Row["E_TEAM"] = listProfile[i].E_TEAM;
                Row["E_SECTION"] = listProfile[i].E_SECTION;
                var OrgByProfile = listOrgStructure.Where(m => m.ID == listProfile[i].OrgStructureID).FirstOrDefault();
                if (OrgByProfile != null)
                {
                    Row["OrgStructureTypeName"] = OrgByProfile.OrgStructureTypeName;
                }
                var KaiZenByProfile = listKaiZen.Where(m => m.ProfileID == listProfile[i].ID && m.Month != null && m.Month >= MonthStart && m.Month <= MonthEnd).ToList();
                Row["Amount"] = KaiZenByProfile.Sum(m => m.SumAmount != null ? (double)m.SumAmount : 0);

                Table.Rows.Add(Row);
            }
            return Table;
        }


        /// <summary>
        /// Bổ dung báo cáo tổng hợp lương theo năm
        /// </summary>
        /// <param name="YearStart"></param>
        /// <param name="YearEnd"></param>
        /// <returns></returns>
        public DataTable ReportGeneralAnnualWage(int YearStart, int YearEnd, List<string> ArrayElementCode, string UserLogin, bool IsCreateTemplate, string ReportName)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();

            DataTable Table = new DataTable(ReportName);
            Table.Columns.Add("STT");
            Table.Columns.Add("Year");
            foreach (var i in ArrayElementCode)
            {
                Table.Columns.Add(i);
            }

            if (IsCreateTemplate)
            {
                return Table;
            }

            int index = 1;
            while (YearStart <= YearEnd)
            {
                DataRow Row = Table.NewRow();
                Row["STT"] = index++;
                Row["Year"] = YearStart;

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[0] = YearStart;
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                List<Cat_ElementEntity> listPayrollTableItem = GetData<Cat_ElementEntity>(listModel, ConstantSql.hrm_Sal_sp_get_SumAmountPayrollTableItem, UserLogin, ref status);

                foreach (var i in ArrayElementCode)
                {
                    var Amount = listPayrollTableItem.Where(m => m.ElementCode.ReplaceSpace() == i.ReplaceSpace()).ToList();
                    Row[i] = Amount.Sum(m => m.Amount != null ? m.Amount : 0);
                }
                Table.Rows.Add(Row);
                YearStart++;
            }
            return Table;
        }

        public DataTable GetSchemaReportChangingTheBasicSalary(string TableName)
        {
            DataTable Table = new DataTable(TableName);
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("E_UNIT");
            Table.Columns.Add("E_DIVISION");
            Table.Columns.Add("E_DEPARTMENT");
            Table.Columns.Add("E_TEAM");
            Table.Columns.Add("E_SECTION");
            Table.Columns.Add("LastSalary", typeof(double));
            Table.Columns.Add("EffectiveDateLast", typeof(DateTime));
            Table.Columns.Add("NewSalary", typeof(double));
            Table.Columns.Add("EffectiveDateNew", typeof(DateTime));
            return Table;
        }

        /// <summary>
        /// báo cáo quá trình lương căn bản
        /// </summary>
        /// <param name="OrgStructure"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public DataTable ReportChangingTheBasicSalary(string OrgStructure, DateTime DateFrom, DateTime DateTo, string UserLogin)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            Hre_ProfileServices Services = new Hre_ProfileServices();
            List<Sal_BasicSalaryEntity> ListBasicSalaryByProfile = new List<Sal_BasicSalaryEntity>();
            double GrossAmount = 0;
            int count = 0;

            listModel = new List<object>();
            listModel.AddRange(new object[18]);
            listModel[2] = OrgStructure;
            listModel[16] = 1;
            listModel[17] = int.MaxValue - 1;
            var listProfile = Services.GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[5] = DateFrom;
            listModel[6] = DateTo;
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            var listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);

            DataTable Table = GetSchemaReportChangingTheBasicSalary("ReportChangingTheBasicSalary");

            foreach (var profile in listProfile)
            {
                GrossAmount = 0;
                DataRow Row = Table.NewRow();
                Row["CodeEmp"] = profile.CodeEmp;
                Row["ProfileName"] = profile.ProfileName;
                Row["E_UNIT"] = profile.E_UNIT;
                Row["E_DIVISION"] = profile.E_DIVISION;
                Row["E_DEPARTMENT"] = profile.E_DEPARTMENT;
                Row["E_TEAM"] = profile.E_TEAM;
                Row["E_SECTION"] = profile.E_SECTION;
                ListBasicSalaryByProfile = listBasicSalary.Where(m => m.ProfileID == profile.ID).ToList();
                if (ListBasicSalaryByProfile.Count > 0)
                {
                    if (ListBasicSalaryByProfile.Count <= 1)
                    {
                        double.TryParse(ListBasicSalaryByProfile.FirstOrDefault().GrossAmount, out GrossAmount);
                        Row["NewSalary"] = GrossAmount;
                        Row["EffectiveDateNew"] = ListBasicSalaryByProfile.FirstOrDefault().DateOfEffect;
                    }
                    else
                    {
                        count = ListBasicSalaryByProfile.Count;
                        double.TryParse(ListBasicSalaryByProfile[count - 1].GrossAmount, out GrossAmount);
                        Row["NewSalary"] = GrossAmount;
                        Row["EffectiveDateNew"] = ListBasicSalaryByProfile[count - 1].DateOfEffect;

                        double.TryParse(ListBasicSalaryByProfile[count - 2].GrossAmount, out GrossAmount);
                        Row["LastSalary"] = GrossAmount;
                        Row["EffectiveDateLast"] = ListBasicSalaryByProfile[count - 2].DateOfEffect;
                    }
                }
                Table.Rows.Add(Row);
            }

            return Table;
        }

        public DataTable ReportTotalSumaryKaiZenSchema()
        {
            DataTable Table = new DataTable("ReportTotalSumaryKaiZen");
            Table.Columns.Add("STT");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("BranchName");
            Table.Columns.Add("DepartmentName");
            Table.Columns.Add("TeamName");
            Table.Columns.Add("SectionName");
            Table.Columns.Add("StatusSyn");
            Table.Columns.Add("Trạng thái đề xuất");
            Table.Columns.Add("Month");
            Table.Columns.Add("Year");
            Table.Columns.Add("Accumulate");
            Table.Columns.Add("MarkPerform");
            return Table;
        }
        public DataTable ReportTotalSumaryKaiZen(DateTime DateFrom, DateTime DateTo, string OrgStructure, string UserLogin, bool IsCreateTemplate)
        {
            string status = string.Empty;
            List<object> listModel = new List<object>();
            List<Kai_KaizenDataEntity> listKaizenDataByProfile = new List<Kai_KaizenDataEntity>();

            listModel = new List<object>();
            listModel.AddRange(new object[10]);
            listModel[8] = 1;
            listModel[9] = Int32.MaxValue - 1;
            List<Kai_KaizenDataEntity> listKaizenData = GetData<Kai_KaizenDataEntity>(listModel, ConstantSql.hrm_Kai_sp_get_KaiZenData, UserLogin, ref status);

            DataTable Table = ReportTotalSumaryKaiZenSchema();
            if (IsCreateTemplate)
            {
                return Table;
            }

            List<Guid?> listProfileIDs = listKaizenData.Select(m => m.ProfileID).Distinct().ToList();
            foreach (var profile in listProfileIDs)
            {
                listKaizenDataByProfile = listKaizenData.Where(m => m.ProfileID == profile).ToList();
                if (listKaizenDataByProfile.Count <= 0)
                {
                    continue;
                }
                DataRow Row = Table.NewRow();

                Row["STT"] = listProfileIDs.IndexOf(profile) + 1;
                Row["CodeEmp"] = listKaizenDataByProfile[0].CodeEmp;
                Row["ProfileName"] = listKaizenDataByProfile[0].ProfileName;

                Row["BranchName"] = listKaizenDataByProfile[0].BranchName;
                Row["DepartmentName"] = listKaizenDataByProfile[0].DepartmentName;
                Row["TeamName"] = listKaizenDataByProfile[0].TeamName;
                Row["SectionName"] = listKaizenDataByProfile[0].SectionName;
                Row["DepartmentName"] = listKaizenDataByProfile[0].E_TEAM;


                Row["BranchName"] = listKaizenDataByProfile[0].BranchName;
                Row["StatusSyn"] = listKaizenDataByProfile[0].StatusSyn;

            }





            return new DataTable();
        }
        #endregion

        #region BC NV Có Con Hưởng PC
        public DataTable CreateReportHaveChildUsunualSchema()
        {
            DataTable tb = new DataTable("Sal_ReportHaveChildUsunualEntity");
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.E_SECTION);
            for (int i = 1; i < 11; i++)
            {
                tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.RelativeName + i);
                tb.Columns.Add(Sal_ReportHaveChildUsunualEntity.FieldNames.DateBorn + i, typeof(DateTime));

            }

            return tb;
        }

        public DataTable GetReportHaveChildUsunual(string orderNumberOrg, DateTime? dateFrom, DateTime? dateTo, string UserLogin, bool IsCreateTemplate)
        {

            DataTable table = CreateReportHaveChildUsunualSchema();
            if (IsCreateTemplate)
            {
                return table;
            }
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var orgTypeServices = new Cat_OrgStructureTypeServices();
                var objOrgType = new List<object>();
                objOrgType.Add(null);
                objOrgType.Add(null);
                objOrgType.Add(1);
                objOrgType.Add(int.MaxValue - 1);
                var lstOrgType = orgTypeServices.GetData<Cat_OrgStructureTypeEntity>(objOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status).ToList();

                var orgServices = new Cat_OrgStructureServices();
                var objOrg = new List<object>();
                objOrg.Add(null);
                objOrg.Add(null);
                objOrg.Add(null);
                objOrg.Add(1);
                objOrg.Add(int.MaxValue - 1);
                var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, UserLogin, ref status).ToList();

                //if (lstOrgType != null)
                //{
                //    lstOrg = lstOrg.Where(s => s.OrgStructureTypeID != null && s.OrgStructureTypeID == lstOrgType.ID).ToList();
                //}
                var lstOrgNumber = lstOrg.Select(s => s.OrderNumber).ToList();
                var strOrderNumber = string.Join(",", lstOrgNumber);

                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.Add(strOrderNumber);
                objProfile.Add(null);
                objProfile.Add(null);
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                var relativeServices = new Hre_RelativesServices();
                var objRelative = new List<object>();
                objRelative.AddRange(new object[11]);
                objRelative[9] = 1;
                objRelative[10] = int.MaxValue - 1;
                var lstRelatives = relativeServices.GetData<Hre_RelativesEntity>(objRelative, ConstantSql.hrm_hr_sp_get_Relatives, UserLogin, ref status).ToList();
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var UnusualAllowance = new Sal_UnusualAllowanceServices();
                var objUnusualAllowance = new List<object>();
                objUnusualAllowance.AddRange(new object[7]);
                objUnusualAllowance[3] = dateFrom;
                objUnusualAllowance[4] = dateTo;
                objUnusualAllowance[5] = 1;
                objUnusualAllowance[6] = int.MaxValue - 1;
                List<Sal_UnusualAllowanceEntity> listUnusualAllowance = UnusualAllowance.GetData<Sal_UnusualAllowanceEntity>(objUnusualAllowance, ConstantSql.hrm_sal_sp_get_UnusualEDChildCare, UserLogin, ref status);

                //lọc nhân viên có hưởng phụ cấp con nhỏ
                lstProfile = lstProfile.Where(m => listUnusualAllowance.Any(t => t.ProfileID == m.ID)).ToList();

                foreach (var item in lstProfile)
                {
                    DataRow dr = table.NewRow();

                    var lstRelativeByProfileID = lstRelatives.Where(s => s.ProfileID == item.ID).ToList();
                    if (lstRelativeByProfileID.Count == 0)
                    {
                        continue;
                    }
                    Guid? orgId = item.OrgStructureID;
                    var unit = LibraryService.GetNearestParent(orgId, OrgUnit.E_UNIT, orgs, orgTypes);
                    var division = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var department = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var section = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.ProfileName] = item.ProfileName;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    dr[Sal_ReportHaveChildUsunualEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    if (lstRelativeByProfileID.Count > 0)
                    {
                        var count = 0;
                        foreach (var relative in lstRelativeByProfileID)
                        {
                            var _tmp = new DateTime();
                            count++;
                            if (count == 11)
                            {
                                break;
                            }
                            if (relative.YearOfBirth != null)
                            {
                                var strYearOfBirth = relative.YearOfBirth.Split('/');
                                var newYOB = new DateTime(int.Parse(strYearOfBirth[2]), int.Parse(strYearOfBirth[1]), int.Parse(strYearOfBirth[0]));
                                var dateCheck = DateTime.Now;

                                var age = (dateCheck.Subtract(_tmp).TotalDays / 30) / 12;
                                dr[Sal_ReportHaveChildUsunualEntity.FieldNames.RelativeName + count] = relative.RelativeName;
                                dr[Sal_ReportHaveChildUsunualEntity.FieldNames.DateBorn + count] = newYOB;

                            }

                        }
                    }
                    table.Rows.Add(dr);
                }

                return table.ConfigTable(true);
            }


        }
        #endregion

        #region BC Danh Sách Tiền OT VÀ Nghỉ Không Lương
        public DataTable getSchemaTableReportAmountOTandUPLeave(List<Cat_ElementEntity> lstElementcode)
        {
            DataTable tb = new DataTable("ReportAmountOTandUnpadLeave");

            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.StatusSyn);

            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateStop, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);

            foreach (var item in lstElementcode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }
            return tb;
        }
        public DataTable ReportAmountOTandUnpadLeave(string lstOrgstructureID, string profileIds, Guid cutOffDurationID, string ElementCode, Guid? workingPlaceID, string statusSyn, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                DateTime monthyear = new DateTime();
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var reposOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var reposAttCutoffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);

                List<string> lstElementCode = new List<string>();
                if (ElementCode != null && ElementCode != string.Empty)
                {
                    lstElementCode = ElementCode.Split(',').ToList();
                }

                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);
                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);
                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList();

                DataTable tb = getSchemaTableReportAmountOTandUPLeave(lstElement);

                //Ds nhân viên
                #region MyRegion
                string status = string.Empty;
                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                List<Hre_ProfileEntity> listProfileByIds = new List<Hre_ProfileEntity>();

                //Lọc theo phòng ban
                if ((lstOrgstructureID != null && lstOrgstructureID != string.Empty)
                    || (lstOrgstructureID == null && profileIds == null))
                {
                    List<object> listObj = new List<object>();
                    listObj.Add(lstOrgstructureID);
                    listObj.Add(string.Empty);
                    listObj.Add(string.Empty);
                    lstProfile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);
                }

                //lọc theo nhân viên
                List<object> lstModel = new List<object>();
                if (profileIds != null && profileIds != string.Empty)
                {
                    lstModel.AddRange(new object[16]);
                    lstModel[14] = 1;
                    lstModel[15] = Int32.MaxValue - 1;
                    listProfileByIds = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(profileIds), ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status).ToList();
                }

                //kết 2 list nhân viên lại
                if (listProfileByIds != null && listProfileByIds.Count > 0)
                {
                    foreach (var profile in listProfileByIds)
                    {
                        if (!lstProfile.Any(m => m.ID == profile.ID))
                        {
                            lstProfile.Add(profile);
                        }
                    }
                }


                #endregion
                //string status = string.Empty;
                //List<object> listModel = new List<object>();

                //listModel = new List<object>();
                //listModel.AddRange(new object[16]);
                //listModel[2] = lstOrgstructureID;
                //listModel[14] = 1;
                //listModel[15] = Int32.MaxValue - 1;
                //List<Hre_ProfileEntity> lstProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin,ref status);


                if (workingPlaceID != null)
                {
                    lstProfile = lstProfile.Where(s => s.WorkPlaceID != null && workingPlaceID.Value == s.WorkPlaceID).ToList();
                }
                if (!string.IsNullOrEmpty(statusSyn))
                {
                    lstProfile = lstProfile.Where(s => s.StatusSyn != null && s.StatusSyn == statusSyn).ToList();
                }
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //kỳ lương
                if (cutOffDurationID != Guid.Empty)
                {
                    var cutoff = reposAttCutoffDuration.GetById(cutOffDurationID);
                    if (cutoff != null)
                    {
                        monthyear = cutoff.MonthYear;
                        from = cutoff.DateStart;
                        to = cutoff.DateEnd;
                    }
                }

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = from;
                listModelprtb[3] = to;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> lstPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = from;
                listModelprtbit[3] = to;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> lstPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                lstPayrollTableItem = lstPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                //Ds phòng ban
                var lstOrgStructure = reposOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }
                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.StatusSyn] = profile.StatusSyn;

                    if (profile.DateQuit != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                    }
                    if (profile.StatusSyn != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.StatusSyn] = profile.StatusSyn;
                    }
                    if (profile.DateStop != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateStop] = profile.DateStop;
                    }
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = profile.CostCentreName;

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION] = profile.E_SECTION;


                    //Bảng lương của profile
                    var payrollTbID_Profile = lstPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var lstpayrollTbItem_Profile = lstPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (lstpayrollTbItem_Profile != null && lstpayrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = lstpayrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                        tb.Rows.Add(dr);
                    }

                }
                #endregion
                return tb;
            }
        }
        #endregion

        #region Hieu.van - Báo cáo tổng hợp tính tiền phép ốm

        public DataTable GetSchemaTotalAnnualSick(List<string> _elementIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("ReportTotalAnnualSick");

                foreach (var item in _elementIDs)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item, typeof(double));
                    }
                }
                return tb;
            }
        }
        /// <summary>
        /// Báo cáo tổng hợp tính tiền phép ốm
        /// </summary>
        /// <param name="ProfileIds"></param>
        /// <param name="CutOffduration"></param>
        /// <returns></returns>
        public DataTable ReportTotalAnnualSick(List<Hre_ProfileEntity> ListProfile, List<string> ElementCode, DateTime MonthStart, DateTime MonthEnd, bool IsCreateTemplate)
        {
            DataTable table = GetSchemaTotalAnnualSick(ElementCode);
            if (IsCreateTemplate)
            {
                return table;
            }

            return table;
        }

        #endregion

        #region Báo cáo tổng hợp thuế PIT hàng tháng
        public DataTable GetSchemaTotalPITMonthly(List<Cat_ElementEntity> lstElementCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("ReportTotalPITMonthly");
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode);

                foreach (var item in lstElementCode)
                {
                    if (item == null)
                        continue;
                    if (!tb.Columns.Contains(item.ElementCode))
                    {
                        if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(Double));
                        }
                        else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(DateTime));
                        }
                        else
                        {
                            tb.Columns.Add(item.ElementCode);
                        }
                    }
                }
                return tb;
            }
        }

        public DataTable ReportTotalPITMonthly(List<Hre_ProfileEntity> lstProfile, List<string> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);
                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList();

                DataTable table = GetSchemaTotalPITMonthly(lstElement);
                if (isCreateTemplate)
                {
                    return table;
                }

                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = table.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                        table.Rows.Add(dr);
                    }

                }
                #endregion

                return table;
            }
        }

        #endregion

        #region Báo cáo Chuyển Khoản _ Lấy theo phần tử động version 2
        public DataTable GetSchemaTransferViaBank_ED(List<Cat_ElementEntity> lstElementCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("ReportTransferViaBank_ED");
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);

                foreach (var item in lstElementCode)
                {
                    if (item == null)
                        continue;
                    if (!tb.Columns.Contains(item.ElementCode))
                    {
                        if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(Double));
                        }
                        else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(DateTime));
                        }
                        else
                        {
                            tb.Columns.Add(item.ElementCode);
                        }
                    }
                }
                return tb;
            }
        }

        public DataTable ReportTransferViaBank_ED(List<Hre_ProfileEntity> lstProfile, List<string> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool IsCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);
                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList();

                DataTable table = GetSchemaTransferViaBank_ED(lstElement);
                if (IsCreateTemplate)
                {
                    return table;
                }

                var repoSalaryInfomation = new CustomBaseRepository<Sal_SalaryInformation>(unitOfWork);
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                //thông tin lương
                var lstsalaryinfomation = repoSalaryInfomation.GetAll().Where(sal => lstProfileID.Contains(sal.ProfileID))
                                          .Select(sal => new { sal.ProfileID, sal.IsCash, sal.AccountNo, sal.Cat_Bank.BankName }).ToList();
                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.GetAll().Where(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }
                    var salaryInfoPro = lstsalaryinfomation.Where(sal => sal.ProfileID == profile.ID).FirstOrDefault();
                    if (salaryInfoPro != null && salaryInfoPro.IsCash == false)
                    {
                        DataRow dr = table.NewRow();
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;

                        var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = salaryInfoPro.AccountNo;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = salaryInfoPro.BankName;

                        var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                        //Bảng lương mỗi profile
                        var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                        var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                        if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                        {
                            foreach (var element in lstElementCode)
                            {
                                var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                                if (prItem != null)
                                {
                                    if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                    {
                                        dr[element] = Convert.ToDouble(prItem.Value);
                                    }
                                    else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                    {
                                        dr[element] = Convert.ToDateTime(prItem.Value);
                                    }
                                    else
                                        dr[element] = prItem.Value;
                                }
                            }
                            table.Rows.Add(dr);
                        }
                    }
                }
                #endregion

                return table;
            }
        }

        #endregion

        #region Báo cáo Tiền Mặt _ Lấy theo phần tử động version 2
        public DataTable GetSchemaCash_ED(List<Cat_ElementEntity> lstElementCode, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo);

            foreach (var item in lstElementCode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }
            return tb;
        }

        public DataTable ReportCash_ED(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, String groupBank, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.AddRange(new object[7]);
                lstObjElement[5] = 1;
                lstObjElement[6] = Int32.MaxValue - 1;
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);

                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList(); ;
                DataTable tb = GetSchemaCash_ED(lstElement, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }


                //ds thông tin lương
                string statusSI = string.Empty;
                var lstObjSalInfo = new List<object>();
                lstObjSalInfo.AddRange(new object[8]);
                lstObjSalInfo[6] = 1;
                lstObjSalInfo[7] = Int32.MaxValue - 1;
                List<Sal_SalaryInformationEntity> lstSalaryInformation = GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref statusSI);
                if (lstSalaryInformation != null && groupBank != null)
                {
                    //lstSalaryInformation = lstSalaryInformation.Where(m => lstBankIDs.Any(t => t == m.BankID)).ToList();
                    lstSalaryInformation = lstSalaryInformation.Where(m => m.GroupBank == groupBank).ToList();

                    //lọc nv theo thông tin lương
                    lstProfile = lstProfile.Where(m => lstSalaryInformation.Any(t => t.ProfileID == m.ID)).ToList();
                }

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.GetAll().Where(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;
                    var salaryInfo = lstSalaryInformation.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                    if (salaryInfo != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = salaryInfo.BankName;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankCode] = salaryInfo.BankCode1;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = salaryInfo.AccountNo;
                    }

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                        tb.Rows.Add(dr);
                    }
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var config130 = new Dictionary<string, object>();
                var config100 = new Dictionary<string, object>();
                config90.Add("width", 90);
                config100.Add("width", 100);
                config130.Add("width", 130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, config90);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, config130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode, config100);

                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region BC So Sánh Các Khoản Bù Trừ
        public DataTable CreateCompareUnusualAllowanceSchema()
        {
            DataTable tb = new DataTable("Sal_CompareUnusualAllowanceEntity");
            tb.Columns.Add(Sal_CompareUnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName);
            tb.Columns.Add(Sal_CompareUnusualAllowanceEntity.FieldNames.EEARING1);
            tb.Columns.Add(Sal_CompareUnusualAllowanceEntity.FieldNames.EEARING2);
            tb.Columns.Add(Sal_CompareUnusualAllowanceEntity.FieldNames.EDEDUCTION1);
            tb.Columns.Add(Sal_CompareUnusualAllowanceEntity.FieldNames.EDEDUCTION2);

            return tb;
        }

        public DataTable GetCompareUnusualAllowance(DateTime dateFrom, DateTime dateTo, string orderNmber, Guid? workplaceID, string statusProfile, string UserLogin, bool IsCreateTeamplate)
        {
            DataTable tb = CreateCompareUnusualAllowanceSchema();
            using (var context = new VnrHrmDataContext())
            {

                var monthSub = dateFrom.AddMonths(-1);
                string status = string.Empty;
                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.Add(orderNmber);
                objProfile.Add(null);
                objProfile.Add(null);
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);
                if (workplaceID != null)
                {
                    lstProfile = lstProfile.Where(s => s.WorkPlaceID != null && s.WorkPlaceID.Value == workplaceID.Value).ToList();
                }
                string _statusAll = EnumDropDown.StatusEmployee.E_ALL.ToString();
                if (!string.IsNullOrEmpty(statusProfile) && statusProfile != _statusAll)
                {
                    lstProfile = lstProfile.Where(s => s.Status == statusProfile).ToList();
                }
                var lstProfileID = lstProfile.Select(s => s.ID).ToList();

                var UnusCfgServices = new Cat_UnusualAllowanceCfgServices();
                var objUnuCfg = new List<object>();
                objUnuCfg.Add(null);
                objUnuCfg.Add(null);
                objUnuCfg.Add(null);
                objUnuCfg.Add(null);
                objUnuCfg.Add(1);
                objUnuCfg.Add(int.MaxValue - 1);
                var lstUnuCfg = UnusCfgServices.GetData<Cat_UnusualAllowanceCfgEntity>(objUnuCfg, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref status).Where(m => m.Type == EnumDropDown.UnusualEDType.E_UNUSUALALLOWANCE.ToString()).ToList();

                var unuServices = new Sal_UnusualAllowanceServices();
                var objUnu = new List<object>();
                objUnu.AddRange(new object[9]);
                objUnu[7] = 1;
                objUnu[8] = int.MaxValue - 1;
                var lstUnu = unuServices.GetData<Sal_UnusualAllowanceEntity>(objUnu, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status);

                var pastDateFrom = dateFrom.AddMonths(-1);
                var pastDateTo = dateTo.AddMonths(-1);

                foreach (var item in lstUnuCfg)
                {
                    DataRow dr = tb.NewRow();
                    dr[Sal_CompareUnusualAllowanceEntity.FieldNames.UnusualAllowanceCfgName] = item.UnusualAllowanceCfgName;
                    var earing = EnumDropDown.EDType.E_EARNING.ToString();
                    var deduction = EnumDropDown.EDType.E_DEDUCTION.ToString();

                    var lstUnuByTypeID = lstUnu.Where(s => s.UnusualEDTypeID == item.ID && lstProfileID.Contains(s.ProfileID)).ToList();

                    var lstPastEaring = lstUnuByTypeID.Where(s => s.EDTypeCfg == earing && s.MonthStart >= pastDateFrom && s.MonthEnd <= pastDateTo).ToList();
                    var lstLastEaring = lstUnuByTypeID.Where(s => s.EDTypeCfg == earing && s.MonthStart >= dateFrom && s.MonthEnd <= dateTo).ToList();

                    var lstPastDeduction = lstUnuByTypeID.Where(s => s.EDTypeCfg == deduction && s.MonthStart >= pastDateFrom && s.MonthEnd <= pastDateTo).ToList();
                    var lstLastDeduction = lstUnuByTypeID.Where(s => s.EDTypeCfg == deduction && s.MonthStart >= dateFrom && s.MonthEnd <= dateTo).ToList();
                    if (tb.Columns.Contains("EEARING1"))
                    {
                        if (lstPastEaring.Count > 0)
                            dr[Sal_CompareUnusualAllowanceEntity.FieldNames.EEARING1] = lstPastEaring.Sum(s => s.Amount);
                    }
                    if (tb.Columns.Contains("EEARING2"))
                    {
                        if (lstLastEaring.Count > 0)
                            dr[Sal_CompareUnusualAllowanceEntity.FieldNames.EEARING2] = lstLastEaring.Sum(s => s.Amount);
                    }
                    if (tb.Columns.Contains("EDEDUCTION1"))
                    {
                        if (lstPastDeduction.Count > 0)
                            dr[Sal_CompareUnusualAllowanceEntity.FieldNames.EDEDUCTION1] = lstPastDeduction.Sum(s => s.Amount);
                    }
                    if (tb.Columns.Contains("EDEDUCTION1"))
                    {
                        if (lstLastDeduction.Count > 0)
                            dr[Sal_CompareUnusualAllowanceEntity.FieldNames.EDEDUCTION2] = lstLastDeduction.Sum(s => s.Amount);
                    }
                    tb.Rows.Add(dr);
                }
                return tb.ConfigTable(true);
            }
        }

        #endregion

        #region Bc thưởng nghỉ lễ
        public DataTable GetSchemaBonusHoliday(List<Cat_ElementEntity> lstElementCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("ReportBonusHoliday");
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);
                tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);

                foreach (var item in lstElementCode)
                {
                    if (item == null)
                        continue;
                    if (!tb.Columns.Contains(item.ElementCode))
                    {
                        if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(Double));
                        }
                        else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode, typeof(DateTime));
                        }
                        else
                        {
                            tb.Columns.Add(item.ElementCode);
                        }
                    }
                }
                return tb;
            }
        }
        public DataTable ReportBonusHoliday(List<Hre_ProfileEntity> lstProfile, List<string> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);
                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList();

                DataTable table = GetSchemaBonusHoliday(lstElement);
                if (isCreateTemplate)
                {
                    return table;
                }

                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[9]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[6] = 1;
                listModelprtb[7] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[6] = 1;
                listModelprtbit[7] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.GetAll().Where(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = table.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                    }

                    table.Rows.Add(dr);
                }
                #endregion

                return table;
            }
        }
        #endregion

        #region Hieu.van - Báo cáo chi tiết phụ cấp

        public DataTable CreateSchema_AllowanceDetail(DateTime dateStart, DateTime dateEnd)
        {

            DataTable tblData = new DataTable("Sal_ReportAllowanceDetailModel");
            tblData.Columns.Add("ProfileName", typeof(String));
            tblData.Columns.Add("CodeEmp", typeof(String));
            tblData.Columns.Add("WorkPlace", typeof(String));
            tblData.Columns.Add("E_UNIT", typeof(String));
            tblData.Columns.Add("E_DIVISION", typeof(String));
            tblData.Columns.Add("E_DEPARTMENT", typeof(String));
            tblData.Columns.Add("E_TEAM", typeof(String));
            tblData.Columns.Add("E_SECTION", typeof(String));

            tblData.Columns.Add("Amount", typeof(Double));
            tblData.Columns.Add("UnusualAllowanceCfgName", typeof(String));
            tblData.Columns.Add("UnusualAllowanceCfgCode", typeof(String));
            //tblData.Columns.Add("EDTypeView", typeof(String));
            tblData.Columns.Add("AmountCfg", typeof(double));
            tblData.Columns.Add("IsChargePIT", typeof(bool));


            tblData.Columns.Add("EDType", typeof(String));

            var df = new DataColumn("DateFrom", typeof(DateTime));
            var de = new DataColumn("DateTo", typeof(DateTime));
            tblData.Columns.Add("Comment", typeof(String));
            df.DefaultValue = dateStart;
            de.DefaultValue = dateEnd;
            tblData.Columns.Add(df);
            tblData.Columns.Add(de);

            return tblData;
        }

        public DataTable GetReportAllowanceDetail(string OrgStructureID, string UnusualEDTypeID, DateTime dateStart, DateTime dateEnd, string StatusSyn, string WorkPlace, string UserLogin, bool isCreateTemplate)
        {
            DataTable tblData = CreateSchema_AllowanceDetail(dateStart, dateEnd);
            if (isCreateTemplate)
            {
                return tblData;
            }
            string status = string.Empty;
            //List<object> strIDs = new List<object>();
            //strIDs.AddRange(new object[3]);
            //strIDs[0] = (object)OrgStructureID;
            //List<Hre_ProfileEntity> lstProfileFull = new List<Hre_ProfileEntity>();
            //List<Hre_ProfileEntity> listProfileByOrg = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin,ref status).ToList();

            List<object> lstObjProfile = new List<object>();
            lstObjProfile.AddRange(new object[18]);
            lstObjProfile[2] = OrgStructureID;
            lstObjProfile[16] = 1;
            lstObjProfile[17] = int.MaxValue - 1;
            List<Hre_ProfileEntity> lstProfileFull = new List<Hre_ProfileEntity>();
            List<Hre_ProfileEntity> listProfileByOrg = GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

            //Lọc theo trạng thái nhân viên và nơi làm việc
            if (listProfileByOrg != null && listProfileByOrg.Count > 0)
            {
                if (StatusSyn != null)
                {
                    List<string> lstSTT = StatusSyn.Split(',').ToList();
                    if (lstSTT.Contains(StatusEmployee.E_WORKING.ToString()))
                    {
                        lstProfileFull.AddRange(listProfileByOrg.Where(pro => (pro.DateQuit == null || pro.DateQuit >= dateEnd) && pro.DateHire < dateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_NEWEMPLOYEE.ToString()))
                    {
                        lstProfileFull.AddRange(listProfileByOrg.Where(pro => pro.DateHire <= dateEnd && pro.DateHire >= dateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_STOPWORKING.ToString()))
                    {
                        lstProfileFull.AddRange(listProfileByOrg.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= dateEnd && pro.DateQuit.Value >= dateStart).ToList());
                    }
                    if (lstSTT.Contains(StatusEmployee.E_WORKINGANDNEW.ToString()))
                    {
                        lstProfileFull.AddRange(listProfileByOrg.Where(pro => pro.DateQuit == null || pro.DateQuit >= dateEnd).ToList());
                    }
                }
                if (WorkPlace != null)
                {
                    List<Guid> lstWP = WorkPlace.Split(',').Select(s => Guid.Parse(s)).ToList();
                    //listProfileByOrg = listProfileByOrg.Where(s => s.WorkPlaceID != null && lstWP.Contains(s.WorkPlaceID.Value)).ToList();
                    lstProfileFull.AddRange(listProfileByOrg.Where(s => s.WorkPlaceID != null && lstWP.Contains(s.WorkPlaceID.Value)).ToList());
                }

                if (StatusSyn == null && WorkPlace == null)
                {
                    lstProfileFull = new List<Hre_ProfileEntity>(listProfileByOrg);
                }
            }



            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSal_UnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);
                var repoCat_UnusualAllowanceCfg = new CustomBaseRepository<Cat_UnusualAllowanceCfg>(unitOfWork);
                var repoCat_WorkPlace = new CustomBaseRepository<Cat_WorkPlace>(unitOfWork);

                var lstType = repoCat_UnusualAllowanceCfg.FindBy(m => m.IsDelete != true && m.Type == HRM.Infrastructure.Utilities.EnumDropDown.UnusualEDType.E_UNUSUALALLOWANCE.ToString()).ToList();
                var lstWorkPlace = repoCat_WorkPlace.FindBy(m => m.IsDelete != true).ToList();

                List<object> lstObj = new List<object>();
                lstObj.AddRange(new object[9]);
                lstObj[2] = UnusualEDTypeID;
                lstObj[3] = dateStart;
                lstObj[4] = dateEnd;
                lstObj[7] = 1;
                lstObj[8] = Int32.MaxValue - 1;
                var lstUA = GetData<Sal_UnusualAllowanceEntity>(lstObj, ConstantSql.hrm_sal_sp_get_UnusualED, UserLogin, ref status);

                foreach (var pro in lstProfileFull)
                {
                    var lstUAbyProfile = lstUA.Where(s => s.ProfileID == pro.ID).ToList();
                    if (lstUAbyProfile == null || lstUAbyProfile.Count == 0)
                        continue;
                    foreach (var cfg in lstType)
                    {
                        var lstUAbyType = lstUAbyProfile.Where(s => s.IsDelete == null && s.UnusualEDTypeID == cfg.ID).ToList();
                        if (lstUAbyType == null || lstUAbyType.Count == 0 || lstUAbyType.Sum(s => s.Amount.GetDouble()) <= 0)
                            continue;

                        var row = tblData.NewRow();
                        var wpTemp = lstWorkPlace.Where(s => s.ID == pro.WorkPlaceID).FirstOrDefault();
                        row["ProfileName"] = pro.ProfileName;
                        row["CodeEmp"] = pro.CodeEmp;
                        row["WorkPlace"] = wpTemp != null ? wpTemp.WorkPlaceName : string.Empty;
                        row["E_UNIT"] = pro.E_UNIT;
                        row["E_DIVISION"] = pro.E_DIVISION;
                        row["E_DEPARTMENT"] = pro.E_DEPARTMENT;
                        row["E_TEAM"] = pro.E_TEAM;
                        row["E_SECTION"] = pro.E_SECTION;
                        row["UnusualAllowanceCfgName"] = cfg.UnusualAllowanceCfgName;
                        row["UnusualAllowanceCfgCode"] = cfg.Code;
                        //row["EDTypeView"] = (cfg.EDType != null && cfg.EDType != "") ? EnumDropDown.GetEnumDescription<EDType>((EDType)Enum.Parse(typeof(EDType), cfg.EDType, true)) : null;
                        if (cfg.Amount > 0)
                            row["AmountCfg"] = cfg.Amount;
                        row["IsChargePIT"] = cfg.IsChargePIT;
                        row["Comment"] = cfg.Comment;

                        row["EDType"] = (cfg.EDType != null && cfg.EDType != "") ? EnumDropDown.GetEnumDescription<EDType>((EDType)Enum.Parse(typeof(EDType), cfg.EDType, true)) : null;
                        row["Amount"] = lstUAbyType.Sum(s => s.Amount.GetDouble());
                        tblData.Rows.Add(row);
                    }
                }
                return tblData.ConfigTable(true);
            }
        }

        #endregion

        #region Hien.NGuyen BC Lương Và Phụ Cấp Thôi Việc

        public DataTable GetSchemaSalaryAllowanceQuit(List<Cat_ElementEntity> lstElementCode, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Settlement, typeof(int));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.StatusSettlement, typeof(bool));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description);

            foreach (var item in lstElementCode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }
            return tb;
        }
        public DataTable ReportSalaryAllowanceQuit(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport, bool IsHoldSalary = false)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);

                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList(); ;
                DataTable tb = GetSchemaSalaryAllowanceQuit(lstElement, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                //ds thông tin lương
                string statusSI = string.Empty;
                var lstObjSalInfo = new List<object>();
                lstObjSalInfo.AddRange(new object[8]);
                lstObjSalInfo[6] = 1;
                lstObjSalInfo[7] = Int32.MaxValue - 1;
                List<Sal_SalaryInformationEntity> lstSalaryInformation = GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref statusSI);

                ////Bảng lương

                //List<object> listModelprtb = new List<object>();
                //listModelprtb = new List<object>();
                //listModelprtb.AddRange(new object[6]);
                //listModelprtb[2] = monthStart;
                //listModelprtb[3] = monthEnd;
                //listModelprtb[4] = 1;
                //listModelprtb[5] = Int32.MaxValue - 1;
                //List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTb = string.Empty;
                List<object> listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[4] = monthStart;
                listModel[5] = monthEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();
                //List<Guid> listPayrollTable = listPayrollTableItem.Select(m => m.PayrollTableID).Distinct().ToList();


                var lstOrgStructure = repoOrgStructure.FindBy(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.FindBy(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process

                foreach (var profile in lstProfile)
                {
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.ProfileID == profile.ID).ToList();
                    if (payrollTbItem_Profile == null || payrollTbItem_Profile.Count <= 0)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax] = profile.CodeTax;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.StatusSettlement] = profile.IsSettlement != null ? profile.IsSettlement : false;

                    if (profile.DateQuit != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                    }
                    if (profile.Settlement != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Settlement] = profile.Settlement;

                    }

                    #region Add lý do bị giữ lương, ngày nghỉ việc và ngày bắt đầu giữ lương.
                    string remark = string.Empty;
                    string dayleave = string.Empty;
                    string monthsalary = string.Empty;
                    if (IsHoldSalary)
                    {
                        Sal_HoldSalaryEntity HoldSalaryByID = listHoldSalary.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                        remark = HoldSalaryByID.DayLeave != null ? "Nghỉ " + HoldSalaryByID.DayLeave.ToString() + " ngày\n" : string.Empty;
                        remark += HoldSalaryByID.IsLeaveContinuous == true ? "Nghỉ liên tục 3 ngày\n" : string.Empty;
                        remark += HoldSalaryByID.Terminate == true ? "Có thông tin nghỉ việc" : string.Empty;
                        dayleave = HoldSalaryByID.DayLeave != null ? HoldSalaryByID.DayLeave.ToString() : string.Empty;
                        monthsalary = HoldSalaryByID.MonthSalary != null ? HoldSalaryByID.MonthSalary.ToString() : string.Empty;
                    }
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description] = remark;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave] = dayleave;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary] = monthsalary;
                    #endregion


                    var salaryInfo = lstSalaryInformation.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                    if (salaryInfo != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = salaryInfo.BankName;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = salaryInfo.AccountNo;
                    }

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                    //Bảng lương mỗi profile
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var config130 = new Dictionary<string, object>();
                var config100 = new Dictionary<string, object>();
                config90.Add("width", 90);
                config100.Add("width", 100);
                config130.Add("width", 130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, config90);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, config130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode, config100);

                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region BC dùng chung hàm
        public DataTable GetSchemaGeneral(List<Cat_ElementEntity> lstElementCode, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description);

            foreach (var item in lstElementCode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }
            return tb;
        }
        public DataTable ReportElementDynamicGeneral(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport, bool IsHoldSalary = false)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);

                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList(); ;
                DataTable tb = GetSchemaGeneral(lstElement, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //ds thông tin lương
                string statusSI = string.Empty;
                var lstObjSalInfo = new List<object>();
                lstObjSalInfo.AddRange(new object[8]);
                lstObjSalInfo[6] = 1;
                lstObjSalInfo[7] = Int32.MaxValue - 1;
                List<Sal_SalaryInformationEntity> lstSalaryInformation = GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref statusSI);

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[4] = monthStart;
                listModel[5] = monthEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.GetAll().Where(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process
                //lọc profile theo nhân viên bị giữ lương
                if (IsHoldSalary)
                {
                    lstProfile = lstProfile.Where(m => listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();
                }


                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax] = profile.CodeTax;

                    #region Add lý do bị giữ lương, ngày nghỉ việc và ngày bắt đầu giữ lương.
                    string remark = string.Empty;
                    string dayleave = string.Empty;
                    string monthsalary = string.Empty;
                    if (IsHoldSalary)
                    {
                        Sal_HoldSalaryEntity HoldSalaryByID = listHoldSalary.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                        remark = HoldSalaryByID.DayLeave != null ? "Nghỉ " + HoldSalaryByID.DayLeave.ToString() + " ngày\n" : string.Empty;
                        remark += HoldSalaryByID.IsLeaveContinuous == true ? "Nghỉ liên tục 3 ngày\n" : string.Empty;
                        remark += HoldSalaryByID.Terminate == true ? "Có thông tin nghỉ việc" : string.Empty;
                        dayleave = HoldSalaryByID.DayLeave != null ? HoldSalaryByID.DayLeave.ToString() : string.Empty;
                        monthsalary = HoldSalaryByID.MonthSalary != null ? HoldSalaryByID.MonthSalary.ToString() : string.Empty;
                    }
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description] = remark;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave] = dayleave;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary] = monthsalary;
                    #endregion


                    var salaryInfo = lstSalaryInformation.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                    if (salaryInfo != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = salaryInfo.BankName;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = salaryInfo.AccountNo;
                    }

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var config130 = new Dictionary<string, object>();
                var config100 = new Dictionary<string, object>();
                config90.Add("width", 90);
                config100.Add("width", 100);
                config130.Add("width", 130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, config90);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, config130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode, config100);

                return tb.ConfigTable(configs);
            }
        }
        #endregion

        #region Hien.Nguyen BC Tổng Hợp Tính Tiền Phép Năm, Sức Khỏe Tốt
        public DataTable GetSchemaRemittanceAllowSick(List<Cat_ElementEntity> lstElementCode, List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.SalaryRankName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BasicSalary, typeof(double));

            foreach (var item in lstElementCode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }

            foreach (var item in listUnusualAllowanceCfg)
            {
                if (!tb.Columns.Contains(item.Code))
                {
                    tb.Columns.Add(item.Code, typeof(Double));
                }
            }

            return tb;
        }

        public DataTable ReportRemittanceAllowSick(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, List<string> UnusualAllowanceCfgIds, DateTime Year, string UserLogin, bool isCreateTemplate, String nameReport, bool IsHoldSalary = false)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                DateTime monthStart = new DateTime(Year.Year, 3, 1);
                DateTime monthEnd = new DateTime(Year.Year, 3, 31);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);
                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList();

                List<object> lstModel = new List<object>();
                lstModel.AddRange(new object[6]);
                lstModel[4] = 1;
                lstModel[5] = Int32.MaxValue - 1;
                List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg = GetData<Cat_UnusualAllowanceCfgEntity>(lstModel, ConstantSql.hrm_cat_sp_get_UnusualAllowanceCfg, UserLogin, ref statusEl).Where(m => UnusualAllowanceCfgIds.Contains(m.ID.ToString())).ToList();

                DataTable tb = GetSchemaRemittanceAllowSick(lstElement, listUnusualAllowanceCfg, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[5] = monthStart;
                listModel[6] = monthEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_BasicSalaryEntity> listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref statusEl);

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                #endregion

                #region Process

                foreach (var profile in lstProfile)
                {
                    DataRow dr = tb.NewRow();

                    tb.Rows.Add(dr);
                }
                #endregion

                return tb.ConfigTable();
            }
        }

        #endregion

        #region BC Chi Tiết Thưởng Đánh Giá

        public DataTable GetSchemaBonusEvaDetail(string[] UnusualAllowanceCfg, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);

            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DisplayNameCostCentreName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BonusEvaluation);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BasicSalary, typeof(double));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EvaLevel);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));


            foreach (var i in UnusualAllowanceCfg)
            {
                if (!tb.Columns.Contains(i))
                {
                    tb.Columns.Add(i, typeof(double));
                }
            }
            return tb;
        }

        public DataTable ReportBonusEvaDetail(List<Hre_ProfileEntity> lstProfile, string[] lstUnusualAllowanceCode, DateTime YearStart, string UserLogin, bool isCreateTemplate, String nameReport, bool IsHoldSalary = false)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoUnusualAllowancecfg = new CustomBaseRepository<Cat_UnusualAllowanceCfg>(unitOfWork);
                var repoUnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);
                var repoBasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);
                var repoEva_Performance = new CustomBaseRepository<Eva_Performance>(unitOfWork);
                string status = string.Empty;

                DateTime monthStart = new DateTime(YearStart.Year, 3, 1);
                DateTime monthEnd = new DateTime(YearStart.Year, 3, 31);
                DateTime start = new DateTime(YearStart.Year, 1, 1);
                DateTime end = new DateTime(YearStart.Year, 12, 31);

                DataTable tb = GetSchemaBonusEvaDetail(lstUnusualAllowanceCode, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }
                var lstUnusualAllowanceCfg = repoUnusualAllowancecfg.FindBy(m => m.IsDelete != true).Select(m => new { m.ID, m.Code, m.UnusualAllowanceCfgName }).ToList();
                var BonusEvaluation = lstUnusualAllowanceCfg.FirstOrDefault(m => m.Code == "BonusEvaluation");
                lstUnusualAllowanceCfg = lstUnusualAllowanceCfg.Where(m => lstUnusualAllowanceCode.Contains(m.Code)).ToList();


                var lstUnusualAllowanceAll = repoUnusualAllowance.FindBy(m => m.IsDelete != true && m.MonthStart <= end && m.MonthEnd >= start).Select(m => new { m.ID, m.ProfileID, m.Amount, m.UnusualEDTypeID, m.MonthStart, m.MonthEnd }).ToList();
                var lstUnusualAllowance = lstUnusualAllowanceAll.Where(m => m.MonthStart <= monthEnd && m.MonthEnd >= monthStart).ToList();


                List<object> listModel = new List<object>();
                listModel.AddRange(new object[14]);
                listModel[12] = 1;
                listModel[13] = Int32.MaxValue - 1;
                List<Eva_PerformanceEntity> listEvaPerformance = GetData<Eva_PerformanceEntity>(listModel, ConstantSql.hrm_eva_sp_get_Performance, UserLogin, ref status).Where(m => m.PeriodFromDate <= new DateTime(YearStart.Year, 12, 1) && m.PeriodToDate >= new DateTime(YearStart.Year, 1, 1)).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[6] = monthEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_BasicSalaryEntity> listBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);

                //listModel = new List<object>();
                //listModel.AddRange(new object[3]);
                //listModel[1] = 1;
                //listModel[2] = Int32.MaxValue - 1;
                //List<Sal_CostCentreSalEntity> listCostCentre = GetData<Sal_CostCentreSalEntity>(listModel, ConstantSql.hrm_sal_sp_get_CostCentre, UserLogin,ref status); 

                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire != null ? profile.DateHire : DateTime.MinValue;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = profile.CostCentreName;
                    if (BonusEvaluation != null)
                    {
                        var BonusEvaluationByProfile = lstUnusualAllowanceAll.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == BonusEvaluation.ID).ToList();
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BonusEvaluation] = BonusEvaluationByProfile.Sum(m => m.Amount);
                    }

                    var BasicSalaryByProfile = listBasicSalary.FirstOrDefault(m => m.ProfileID == profile.ID);
                    if (BasicSalaryByProfile != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BasicSalary] = BasicSalaryByProfile.GrossAmount;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DisplayNameCostCentreName] = BasicSalaryByProfile.SalaryRankName;
                    }

                    Eva_PerformanceEntity PerformanceByProfile = listEvaPerformance.FirstOrDefault(m => m.ProfileID == profile.ID);
                    if (PerformanceByProfile != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.EvaLevel] = PerformanceByProfile.Level1Name;
                    }

                    foreach (var i in lstUnusualAllowanceCfg)
                    {
                        if (i.Code == "BonusEvaluation")
                        {
                            var lstUnusualAllowancebyProfile = lstUnusualAllowanceAll.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == i.ID).ToList();
                            dr[i.Code] = lstUnusualAllowancebyProfile.Sum(m => m.Amount);
                        }
                        else
                        {
                            var lstUnusualAllowancebyProfile = lstUnusualAllowance.Where(m => m.ProfileID == profile.ID && m.UnusualEDTypeID == i.ID).ToList();
                            dr[i.Code] = lstUnusualAllowancebyProfile.Sum(m => m.Amount);
                        }
                    }

                    tb.Rows.Add(dr);
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var config130 = new Dictionary<string, object>();
                var config100 = new Dictionary<string, object>();
                config90.Add("width", 90);
                config100.Add("width", 100);
                config130.Add("width", 130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, config90);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, config130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter, config100);

                return tb.ConfigTable(configs);
            }
        }
        #endregion

        #region Hien.Nguyen BC Chuyển Khoản Thưởng NV Bị Giữ Lương
        public DataTable GetSchemaTransferBonusHold(List<Cat_ElementEntity> lstElementCode, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthHoldSalary, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description);

            foreach (var item in lstElementCode)
            {
                if (item == null)
                    continue;
                if (!tb.Columns.Contains(item.ElementCode))
                {
                    if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(Double));
                    }
                    else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                    {
                        tb.Columns.Add(item.ElementCode, typeof(DateTime));
                    }
                    else
                    {
                        tb.Columns.Add(item.ElementCode);
                    }
                }
            }
            return tb;
        }
        public DataTable ReportTransferBonusHold(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {

                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCostCentre = new CustomBaseRepository<Cat_CostCentre>(unitOfWork);
                var repoElement = new CustomBaseRepository<Cat_Element>(unitOfWork);

                //ds phần tử
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(null);
                lstObjElement.Add(1);
                lstObjElement.Add(int.MaxValue - 1);
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);

                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList(); ;
                DataTable tb = GetSchemaTransferBonusHold(lstElement, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                //ds thông tin lương
                string statusSI = string.Empty;
                var lstObjSalInfo = new List<object>();
                lstObjSalInfo.AddRange(new object[8]);
                lstObjSalInfo[6] = 1;
                lstObjSalInfo[7] = Int32.MaxValue - 1;
                List<Sal_SalaryInformationEntity> lstSalaryInformation = GetData<Sal_SalaryInformationEntity>(lstObjSalInfo, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformation, UserLogin, ref statusSI);

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[4] = monthStart;
                listModel[5] = monthEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                var lstCostcentre = repoCostCentre.GetAll().Where(cost => cost.IsDelete == null).Select(cost => new { cost.ID, cost.Code, cost.CostCentreName }).ToList();
                #endregion

                #region Process

                lstProfile = lstProfile.Where(m => listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeTax] = profile.CodeTax;

                    #region Add lý do bị giữ lương, ngày nghỉ việc và ngày bắt đầu giữ lương.
                    string remark = string.Empty;
                    string dayleave = string.Empty;
                    string monthsalary = string.Empty;
                    Sal_HoldSalaryEntity HoldSalaryByID = listHoldSalary.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                    remark = HoldSalaryByID.DayLeave != null ? "Nghỉ " + HoldSalaryByID.DayLeave.ToString() + " ngày\n" : string.Empty;
                    remark += HoldSalaryByID.IsLeaveContinuous == true ? "Nghỉ liên tục 3 ngày\n" : string.Empty;
                    remark += HoldSalaryByID.Terminate == true ? "Có thông tin nghỉ việc" : string.Empty;
                    dayleave = HoldSalaryByID.DayLeave != null ? HoldSalaryByID.DayLeave.ToString() : string.Empty;
                    monthsalary = HoldSalaryByID.MonthSalary != null ? HoldSalaryByID.MonthSalary.ToString() : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthHoldSalary] = HoldSalaryByID.MonthSalary;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.Description] = remark;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DayLeave] = dayleave;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.MonthSalary] = monthsalary;
                    if (profile.DateQuit != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                    }


                    #endregion


                    var salaryInfo = lstSalaryInformation.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                    if (salaryInfo != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankName] = salaryInfo.BankName;
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.BankAccountNo] = salaryInfo.AccountNo;
                    }

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    var costcentre = lstCostcentre.Where(cost => cost.ID == profile.CostCentreID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode] = costcentre != null ? costcentre.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter] = costcentre != null ? costcentre.CostCentreName : string.Empty;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                    {
                        foreach (var element in lstElementCode)
                        {
                            var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                            if (prItem != null)
                            {
                                if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDouble(prItem.Value);
                                }
                                else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                {
                                    dr[element] = Convert.ToDateTime(prItem.Value);
                                }
                                else
                                    dr[element] = prItem.Value;
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var config130 = new Dictionary<string, object>();
                var config100 = new Dictionary<string, object>();
                config90.Add("width", 90);
                config100.Add("width", 100);
                config130.Add("width", 130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp, config90);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName, config130);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenter, config100);
                configs.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CostCenterCode, config100);

                return tb.ConfigTable(configs);
            }
        }
        #endregion

        #region Sal_ReportProfile
        public DataTable CreateReportProfileScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Sal_ReportProfileEntity");
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.CodeEmp, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.ProfileName, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.UnitNameOrg, typeof(string));
                //tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.UnitCode, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DepartmentNameOrg, typeof(string));
                //tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DepartmentCode, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.SectionNameOrg, typeof(string));
                //tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.SectionCode, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.TeamNameOrg, typeof(string));
                //tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.TeamCode, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.WorkPlaceName, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.StatusSyn, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.SalaryClassName, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.SalaryRankName, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.BasicSalary, typeof(double));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.CostCentreCode, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.IDNo, typeof(string));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateOfBirth, typeof(DateTime));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateHire, typeof(DateTime));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateStart, typeof(DateTime));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateEnd, typeof(DateTime));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.IsUnEmpInsurance, typeof(bool));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.IsHealthInsurance, typeof(bool));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.IsSocialInsurance, typeof(bool));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.IsTradeUnionist, typeof(bool));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateStop, typeof(DateTime));
                tb.Columns.Add(Sal_ReportProfileEntity.FieldNames.DateQuit, typeof(DateTime));
                return tb;
            }
        }
        public DataTable GetReportProfile(List<Hre_ProfileEntity> lstprofile, Guid _CutOffDurationID, string UserLogin)
        {


            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportProfileScheme();
                if (lstprofile == null)
                    return table;
                //var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                //var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                //var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                //var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                //hop dong
                string status = string.Empty;
                var hreServiceContract = new Hre_ContractServices();
                List<object> paraContract = new List<object>();
                paraContract.AddRange(new object[21]);
                paraContract[19] = 1;
                paraContract[20] = int.MaxValue;
                var lstContract = hreServiceContract.GetData<Hre_ContractEntity>(paraContract, ConstantSql.hrm_hr_sp_get_Contract, UserLogin, ref status);
                //ky cong
                string cutoff = Common.DotNetToOracle(_CutOffDurationID.ToString());
                var baseService = new BaseService();
                var objAtt_CutOffDurationEntity = baseService
                    .GetData<Att_CutOffDurationEntity>(cutoff, ConstantSql.hrm_att_sp_get_CutOffDurationById, UserLogin, ref status)
                    .FirstOrDefault();
                //
                DateTime _dateFrom = DateTime.MinValue;
                DateTime _dateTo = DateTime.MaxValue;
                if (objAtt_CutOffDurationEntity != null)
                {
                    if (objAtt_CutOffDurationEntity.DateStart != null)
                        _dateFrom = objAtt_CutOffDurationEntity.DateStart;
                    if (objAtt_CutOffDurationEntity.DateEnd != null)
                        _dateTo = objAtt_CutOffDurationEntity.DateEnd;
                }
                var salServiceBasicSalary = new Sal_BasicSalaryServices();
                List<object> paraBasicSalary = new List<object>();
                paraBasicSalary.AddRange(new object[4]);
                paraBasicSalary[0] = _dateFrom;
                paraBasicSalary[1] = _dateTo;
                paraBasicSalary[2] = 1;
                paraBasicSalary[3] = int.MaxValue - 1;
                var lstBasicSalary = salServiceBasicSalary.GetData<Sal_BasicSalaryEntity>(paraBasicSalary, ConstantSql.hrm_sal_sp_getdata_BasicSalaryByCutOff, UserLogin, ref status);
                //Hre_ProfilePartyUnion
                var repoHre_ProfilePartyUnion = new Hre_ProfilePartyUnionRepository(unitOfWork);
                var lstProfilePartyUnion = repoHre_ProfilePartyUnion.FindBy(s => s.IsDelete == null && s.IsTradeUnionist == true && s.YouthUnionEnrolledDate <= _dateTo).ToList();
                //Hre_StopWorking
                List<object> paraStopWorking = new List<object>();
                paraStopWorking.AddRange(new object[17]);
                paraStopWorking[6] = _dateFrom;
                paraStopWorking[7] = _dateTo;
                paraStopWorking[15] = 1;
                paraStopWorking[16] = int.MaxValue - 1;
                var hreServiceStopWorking = new Hre_StopWorkingServices();
                var lstStopWorking = hreServiceStopWorking.GetData<Hre_StopWorkingEntity>(paraStopWorking, ConstantSql.hrm_hr_sp_get_StopWorking, UserLogin, ref status);
                //Ins_ProfileInsuranceMonthly
                var repoProfileInsuranceMonthly = new Ins_ProfileInsuranceMonthlyRepository(unitOfWork);
                var lstProfileInsuranceMonthly = repoProfileInsuranceMonthly.FindBy(s => s.IsDelete == null && s.MonthYear >= _dateFrom && s.MonthYear <= _dateTo).ToList();

                foreach (var profile in lstprofile)
                {
                    if (profile != null)
                    {
                        var objContract = lstContract.Where(s => s.ProfileID == profile.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                        var objBasicSalary = lstBasicSalary.Where(s => s.ProfileID == profile.ID).OrderByDescending(s => s.DateOfEffect).FirstOrDefault();
                        var objProfilePartyUnion = lstProfilePartyUnion.Where(s => s.ProfileID == profile.ID).OrderByDescending(s => s.YouthUnionEnrolledDate).FirstOrDefault();
                        var objStopWorking = lstStopWorking.Where(s => s.ProfileID == profile.ID).OrderByDescending(s => s.DateStop).FirstOrDefault();
                        var objProfileInsuranceMonthly = lstProfileInsuranceMonthly.Where(s => s.ProfileID == profile.ID).OrderByDescending(s => s.MonthYear).FirstOrDefault();
                        DataRow row = table.NewRow();
                        if (profile.CodeEmp != null)
                            row[Sal_ReportProfileEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        if (profile.ProfileName != null)
                            row[Sal_ReportProfileEntity.FieldNames.ProfileName] = profile.ProfileName;
                        if (profile.UnitNameOrg != null)
                            row[Sal_ReportProfileEntity.FieldNames.UnitNameOrg] = profile.UnitNameOrg;
                        if (profile.DepartmentNameOrg != null)
                            row[Sal_ReportProfileEntity.FieldNames.DepartmentNameOrg] = profile.DepartmentNameOrg;
                        if (profile.SectionNameOrg != null)
                            row[Sal_ReportProfileEntity.FieldNames.SectionNameOrg] = profile.SectionNameOrg;
                        if (profile.TeamNameOrg != null)
                            row[Sal_ReportProfileEntity.FieldNames.TeamNameOrg] = profile.TeamNameOrg;
                        if (profile.WorkPlaceName != null)
                            row[Sal_ReportProfileEntity.FieldNames.WorkPlaceName] = profile.WorkPlaceName;
                        if (profile.StatusSyn != null)
                            row[Sal_ReportProfileEntity.FieldNames.StatusSyn] = profile.StatusSyn;
                        if (objBasicSalary != null && objBasicSalary.SalaryClassName != null)
                            row[Sal_ReportProfileEntity.FieldNames.SalaryClassName] = objBasicSalary.SalaryClassName;
                        if (objBasicSalary != null && objBasicSalary.GrossAmount != null)
                            row[Sal_ReportProfileEntity.FieldNames.BasicSalary] = objBasicSalary.GrossAmount;
                        if (objBasicSalary != null && objBasicSalary.SalaryRankName != null)
                            row[Sal_ReportProfileEntity.FieldNames.SalaryRankName] = objBasicSalary.SalaryRankName;
                        if (profile.CostCentreCode != null)
                            row[Sal_ReportProfileEntity.FieldNames.CostCentreCode] = profile.CostCentreCode;
                        if (profile.IDNo != null)
                            row[Sal_ReportProfileEntity.FieldNames.IDNo] = profile.IDNo;
                        if (profile.DateOfBirth != null)
                            row[Sal_ReportProfileEntity.FieldNames.DateOfBirth] = profile.DateOfBirth;
                        if (profile.DateHire != null)
                            row[Sal_ReportProfileEntity.FieldNames.DateHire] = profile.DateHire;
                        if (objContract != null)
                        {
                            if (objContract.DateStart != null)
                                row[Sal_ReportProfileEntity.FieldNames.DateStart] = objContract.DateStart;
                            if (objContract.DateEnd != null)
                                row[Sal_ReportProfileEntity.FieldNames.DateEnd] = objContract.DateEnd;
                        }
                        //if (profile.WorkingPlace != null)
                        //    row[Sal_ReportProfileEntity.FieldNames.WorkingPlace] = profile.WorkingPlace;
                        if (objProfileInsuranceMonthly != null && objProfileInsuranceMonthly.IsUnEmpInsurance != null)
                            row[Sal_ReportProfileEntity.FieldNames.IsUnEmpInsurance] = objProfileInsuranceMonthly.IsUnEmpInsurance;
                        if (objProfileInsuranceMonthly != null && objProfileInsuranceMonthly.IsSocialInsurance != null)
                            row[Sal_ReportProfileEntity.FieldNames.IsSocialInsurance] = objProfileInsuranceMonthly.IsSocialInsurance;
                        if (objProfileInsuranceMonthly != null && objProfileInsuranceMonthly.IsHealthInsurance != null)
                            row[Sal_ReportProfileEntity.FieldNames.IsHealthInsurance] = objProfileInsuranceMonthly.IsHealthInsurance;
                        if (objProfilePartyUnion != null && objProfilePartyUnion.IsTradeUnionist != null)
                            row[Sal_ReportProfileEntity.FieldNames.IsTradeUnionist] = objProfilePartyUnion.IsTradeUnionist;
                        if (objStopWorking != null && objStopWorking.DateStop != null)
                            row[Sal_ReportProfileEntity.FieldNames.DateStop] = objStopWorking.DateStop;
                        if (profile.DateHire != null)
                            row[Sal_ReportProfileEntity.FieldNames.DateHire] = profile.DateHire;
                        table.Rows.Add(row);
                    }
                }
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region DANH SÁCH TỔNG HỢP BHXH, BHYT, BHTN, TRUY THU BH (XH,YT,TN) Theo Năm
        public DataTable GetSchema_ReportGeneralInsuranceInYear(List<Cat_ElementEntity> lstElementCode, DateTime monthStart, DateTime monthEnd, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION);
            //tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            //tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);

            for (DateTime month = monthStart; month <= monthEnd; month = month.AddMonths(1))
            {
                foreach (var item in lstElementCode)
                {
                    if (item == null)
                        continue;
                    if (!tb.Columns.Contains(item.ElementCode + month.Month.ToString()))
                    {
                        if (item.Type != null && item.Type.ToUpper() == typeof(Double).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode + month.Month.ToString(), typeof(Double));
                        }
                        else if (item.Type != null && item.Type.ToUpper() == typeof(DateTime).Name.ToUpper())
                        {
                            tb.Columns.Add(item.ElementCode + month.Month.ToString(), typeof(DateTime));
                        }
                        else
                        {
                            tb.Columns.Add(item.ElementCode + month.Month.ToString());
                        }
                    }
                }
            }
            return tb;
        }
        public DataTable ReportGeneralInsuranceInYear(List<Hre_ProfileEntity> lstProfile, List<String> lstElementCode, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                //ds phần tử 
                string statusEl = string.Empty;
                var lstObjElement = new List<object>();
                lstObjElement.AddRange(new object[7]);
                lstObjElement[5] = 1;
                lstObjElement[6] = Int32.MaxValue - 1;
                List<Cat_ElementEntity> lstElement = GetData<Cat_ElementEntity>(lstObjElement, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref statusEl);

                lstElement = lstElement.Where(m => lstElementCode.Contains(m.ElementCode)).ToList(); ;
                DataTable tb = GetSchema_ReportGeneralInsuranceInYear(lstElement, monthStart, monthEnd, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                #endregion

                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.E_SECTION] = profile.E_SECTION;


                    //var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    //dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    //Bảng lương mỗi profile theo từng tháng
                    for (DateTime month = monthStart; month <= monthEnd; month = month.AddMonths(1))
                    {
                        var payrollTbID_Profile = listPayrollTable.Where(sal => sal.MonthYear == month && sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                        var payrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                        if (payrollTbItem_Profile != null && payrollTbItem_Profile.Count > 0)
                        {
                            foreach (var element in lstElementCode)
                            {
                                var prItem = payrollTbItem_Profile.Where(salIt => salIt.Code == element).FirstOrDefault();
                                if (prItem != null)
                                {
                                    if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                    {
                                        dr[element + month.Month.ToString()] = Convert.ToDouble(prItem.Value);
                                    }
                                    else if (prItem.ValueType != null && prItem.ValueType.ToUpper() == typeof(DateTime).Name.ToUpper())
                                    {
                                        dr[element + month.Month.ToString()] = Convert.ToDateTime(prItem.Value);
                                    }
                                    else
                                        dr[element + month.Month.ToString()] = prItem.Value;
                                }
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion

                return tb.ConfigTable();
            }
        }
        #endregion

        #region Xuất phiếu lương toàn công ty
        public DataTable GetSchemaExportPayroll()
        {
            DataTable tb = new DataTable("Sal_ReportBasicSalaryMonthlyModel");
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.JobtitleName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.PositionName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode);
            tb.Columns.Add(Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace);
            return tb;
        }
        public string ExportPayroll(Guid templateID, List<Hre_ProfileEntity> lstProfile, DateTime monthStart, DateTime monthEnd, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                #endregion

                //Nếu chỉ có 1 nhân viên thì nén trong vòng lặp, nếu ko thì xong mới nén tất cả
                Boolean isZipfile = false;
                if (lstProfile != null && lstProfile.Count > 1)
                {
                    isZipfile = true;
                }

                //Đường dẫn file export
                string outPath = string.Empty;

                //Thư mục nén và đường dẫn
                string folderSave = DateTime.Now.ToString("_ddMMyyyyHHmmss");
                string dirpath = Common.GetPath(Common.DownloadURL + folderSave);


                #region Process
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataTable tb = GetSchemaExportPayroll();
                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkingPlace;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var lstpayrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (lstpayrollTbItem_Profile != null && lstpayrollTbItem_Profile.Count > 0)
                    {
                        foreach (var item in lstpayrollTbItem_Profile)
                        {
                            Double value = 0;
                            if (!tb.Columns.Contains(item.Code))
                            {
                                tb.Columns.Add(item.Code, typeof(Double));
                            }
                            if (tb.Columns.Contains(item.Code))
                            {
                                if (item.ValueType != null && item.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    Double.TryParse(item.Value, out value);
                                }
                                dr[item.Code] = value;
                            }
                        }
                        tb.Rows.Add(dr);
                    }
                    if (tb.Rows.Count > 0)
                    {
                        outPath = ExportService.ExportPlaysip(templateID, profile.ProfileName, folderSave, tb, ExportFileType.Excel);
                    }

                    //Nếu chỉ có 1 người thì nén file luôn
                    if (!isZipfile)
                    {
                        string fileName = Common.GetFileName(outPath);
                        return fileName;
                    }
                }
                //Nén tất cả nhiều profile
                if (isZipfile)
                {
                    var outpath = Common.MultiExport("", isZipfile, folderSave);
                    return outpath;
                }
                #endregion
                return null;
            }
        }
        #endregion

        #region Gửi mail payslip
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateID">Template xuất payslip</param>
        /// <param name="lstProfile"></param>
        /// <param name="monthStart"></param>
        /// <param name="monthEnd"></param>
        /// <returns></returns>

        public Boolean CheckSendMail(String emailTo, String outPath, DateTime dateStart, DateTime dateEnd, Hre_ProfileEntity profile)
        {
            using (var context = new VnrHrmDataContext())
            {
                Boolean iSuccess = false;
                var service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSysTemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string title = dateEnd.ToString("MM-yyyy") + "-" + profile.ProfileName;
                string titleMail = "PaySlip Of " + title;
                string body = string.Empty;

                string typeTemplate = EnumDropDown.EmailType.E_PAYSLIP.ToString();
                var template = repoSysTemplateSendMail.FindBy(s => s.Type == typeTemplate).FirstOrDefault();
                if (template != null)
                {
                    if (!template.Subject.IsNullOrEmpty())
                    {
                        titleMail = template.Subject + title;
                    }
                    string[] strParaFields = new string[] 
                    {
                        Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName,
                        Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateFrom,
                        Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateTo
                    };
                    string[] strParaValues = new string[]
                    {
                        profile.ProfileName,
                        dateStart.ToShortDateString(),
                        dateEnd.ToShortDateString()
                    };
                    body = LibraryService.ReplaceContentFile(template.Content, strParaFields, strParaValues);
                }
                iSuccess = service.SendMail(titleMail, emailTo, body, outPath);
                return iSuccess;
            }
        }

        public DataErrorCode SendMailPayslip(Guid templateID, List<Hre_ProfileEntity> lstProfile, DateTime monthStart, DateTime monthEnd, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region Get Data
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoOrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                //ds nhân viên
                var lstProfileID = lstProfile.Select(hr => hr.ID).ToList();

                //Bảng lương
                string statusTb = string.Empty;
                List<object> listModelprtb = new List<object>();
                listModelprtb = new List<object>();
                listModelprtb.AddRange(new object[6]);
                listModelprtb[2] = monthStart;
                listModelprtb[3] = monthEnd;
                listModelprtb[4] = 1;
                listModelprtb[5] = Int32.MaxValue - 1;
                List<Sal_PayrollTableEntity> listPayrollTable = GetData<Sal_PayrollTableEntity>(listModelprtb, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref statusTb);

                string statusTbit = string.Empty;
                List<object> listModelprtbit = new List<object>();
                listModelprtbit = new List<object>();
                listModelprtbit.AddRange(new object[9]);
                listModelprtbit[2] = monthStart;
                listModelprtbit[3] = monthEnd;
                listModelprtbit[7] = 1;
                listModelprtbit[8] = Int32.MaxValue - 1;
                List<Sal_PayrollTableItemEntity> listPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModelprtbit, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref statusTbit);
                listPayrollTableItem = listPayrollTableItem.Where(it => it.Value != null && it.Value != string.Empty).ToList();

                var lstOrgStructure = repoOrgStructure.GetAll().Where(org => org.IsDelete == null).Select(org => new { org.ID, org.Code, org.OrgStructureName }).ToList();
                #endregion

                //Đường dẫn file export
                string outPath = string.Empty;

                //Thư mục nén và đường dẫn
                string folderSave = DateTime.Now.ToString("_ddMMyyyyHHmmss");
                string dirpath = Common.GetPath(Common.DownloadURL + folderSave);

                #region Process
                bool isSuccess = false;
                foreach (var profile in lstProfile)
                {
                    if (profile == null)
                    {
                        continue;
                    }

                    DataTable tb = GetSchemaExportPayroll();
                    DataRow dr = tb.NewRow();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkPlaceName;

                    if (profile.DateHire != null)
                    {
                        dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    var orgStructure = lstOrgStructure.Where(org => org.ID == profile.OrgStructureID).FirstOrDefault();
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureCode] = orgStructure != null ? orgStructure.Code : string.Empty;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.OrgStructureName] = orgStructure != null ? orgStructure.OrgStructureName : string.Empty;

                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.TaxCode] = profile.CodeTax;
                    dr[Sal_ReportBasicSalaryMonthlyEntity.FieldNames.WorkingPlace] = profile.WorkingPlace;

                    //Bảng lương mỗi profile
                    var payrollTbID_Profile = listPayrollTable.Where(sal => sal.ProfileID == profile.ID).Select(sal => sal.ID).FirstOrDefault();
                    var lstpayrollTbItem_Profile = listPayrollTableItem.Where(salit => salit.PayrollTableID == payrollTbID_Profile).ToList();
                    if (lstpayrollTbItem_Profile != null && lstpayrollTbItem_Profile.Count > 0)
                    {
                        foreach (var item in lstpayrollTbItem_Profile)
                        {
                            Double value = 0;
                            if (!tb.Columns.Contains(item.Code))
                            {
                                tb.Columns.Add(item.Code, typeof(Double));
                            }
                            if (tb.Columns.Contains(item.Code))
                            {
                                if (item.ValueType != null && item.ValueType.ToUpper() == typeof(Double).Name.ToUpper())
                                {
                                    Double.TryParse(item.Value, out value);
                                }
                                dr[item.Code] = value;
                            }
                        }
                        tb.Rows.Add(dr);
                    }
                    if (tb.Rows.Count > 0)
                    {
                        outPath = ExportService.Export(templateID, tb, ExportFileType.Excel);
                        string emailTo = profile.Email;
                        if (emailTo != null && emailTo != string.Empty)
                        {
                            isSuccess = CheckSendMail(emailTo, outPath, monthStart, monthEnd, profile);
                        }
                    }
                }
                #endregion

                if (isSuccess)
                    return DataErrorCode.Success;
                else
                    return DataErrorCode.Error;
            }
        }
        #endregion

        #region Hieu.Van - BC tỷ lệ tham dự và tỷ lệ áp dụng - KAIZEN

        public DataTable GetSchema_kaizen(List<DateTime> lstMonth, String nameReport)
        {
            DataTable tb = new DataTable(nameReport);

            tb.Columns.Add("Menu");
            int _temp = 0;
            foreach (DateTime date in lstMonth)
            {
                //tb.Columns.Add(date.ToString("MMM-yyyy"), typeof(double));
                _temp += 1;
                if (!tb.Columns.Contains("Month" + _temp))
                {
                    tb.Columns.Add("Month" + _temp, typeof(double));
                }
            }
            return tb;
        }
        public DataTable ReportKaizenGeneral(string strOrgStructure, DateTime monthStart, DateTime monthEnd, string UserLogin, bool isCreateTemplate, String nameReport)
        {
            Guid OrgID = Guid.Empty;
            Guid.TryParse(strOrgStructure, out OrgID);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<DateTime> lstMonth = new List<DateTime>();

                for (DateTime datecheck = monthStart; datecheck <= monthEnd; datecheck = datecheck.AddMonths(1))
                {
                    lstMonth.Add(datecheck);
                }
                DataTable tb = GetSchema_kaizen(lstMonth, nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                #region getdata

                string status = string.Empty;
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                //var orgParent = LibraryService.GetNearestParent(OrgID, (OrgUnit)Enum.Parse(typeof(OrgUnit), TypeOrg), orgs, orgTypes);
                getChildOrgStructure(orgs, OrgID);

                List<int> orderNum = orderNumber.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => int.Parse(m)).ToList();
                List<Guid> lstOrgID = orgs.Where(m => orderNum.Contains(m.OrderNumber)).Select(m => m.ID).ToList();
                //Get Profile
                List<object> lstWH = new List<object>();
                lstWH.Add(null);
                lstWH.Add(monthEnd);
                var HistoryOrg = GetData<Hre_WorkHistory>(lstWH, ConstantSql.hrm_hre_getdata_WorkHistory, UserLogin, ref status)
                    .Where(m => m.OrganizationStructureID != null)
                    .Select(m => new { m.ID, m.ProfileID, m.OrganizationStructureID, m.DateEffective }).OrderByDescending(m => m.DateEffective).ToList();
                //Todo: Get Schemma 
                //DataRow dr = tb.NewRow();
                //for (DateTime dateCheck = monthStart; dateCheck <= monthEnd; dateCheck = dateCheck.AddMonths(1))
                //{
                //    DateTime BeginMonth = new DateTime(dateCheck.Year, dateCheck.Month, 1);
                //    var GroupHistoryByTime = HistoryOrg.Where(m => m.DateEffective <= BeginMonth).GroupBy(m => m.ProfileID).Select(m => m.FirstOrDefault()).ToList();
                //    dr[dateCheck.ToString("yyyyMMdd")] = GroupHistoryByTime.Count(m => lstOrgID.Contains(m.OrganizationStructureID.Value));
                //}




                //List<object> listObj = new List<object>();
                //listObj.AddRange(new object[3]);
                //listObj[0] = orderNumber;
                //var listProfileByOrg = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin,ref status);
                //var lstProfileID = listProfileByOrg.Select(s => s.ID).ToList();

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Kai_KaizenDataEntity> lstKaiZen = GetData<Kai_KaizenDataEntity>(listModel, ConstantSql.hrm_Kai_sp_get_KaiZenData, UserLogin, ref status);


                #endregion

                #region listRow

                List<string> listRow = new List<string>();

                listRow.Add("EmpTotal");
                listRow.Add("EmpJoin");
                listRow.Add("Perform");
                listRow.Add("Idea");
                listRow.Add("Fail");
                listRow.Add("Detection");
                #endregion
                #region NV dang lam viec
                string strE_HIRE = ProfileStatusSyn.E_HIRE.ToString();
                List<Guid> lstProfileIDs = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, s => s.StatusSyn == strE_HIRE).Select(s => s.ID).ToList();
                #endregion
                #region lich su nhan vien dua vao Profileids
                var lstWorkHistory = new List<Hre_WorkHistory>().Select(s => new
                {
                    s.ProfileID,
                    s.SalaryClassID,
                }).ToList();

                #endregion

                #region Process

                //var lstKaizenByProfile = lstKaiZen.Where(s => s.Month != null && lstProfileID.Contains(s.ProfileID.Value)).ToList();

                Dictionary<DateTime, List<Guid>> dicProfileByMonth = new Dictionary<DateTime, List<Guid>>();
                foreach (var item in lstMonth)
                {
                    DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);

                    var GroupHistoryByTime = HistoryOrg.Where(m => m.DateEffective <= BeginMonth).GroupBy(m => m.ProfileID).Select(m => m.FirstOrDefault()).ToList();
                    dicProfileByMonth.Add(BeginMonth, GroupHistoryByTime.Where(m => lstOrgID.Contains(m.OrganizationStructureID.Value)).Select(m => m.ProfileID).ToList());
                }

                foreach (var row in listRow)
                {
                    DataRow dro = tb.NewRow();
                    dro["Menu"] = row;

                    switch (row)
                    {
                        #region EmpTotal

                        case "EmpTotal":
                            int _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];
                                //dro[item.ToString("MMM-yyyy")] = lstProfileID.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstProfileID.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        #region EmpJoin

                        case "EmpJoin":
                            _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                DateTime EndMonth = BeginMonth.AddMonths(1).AddMilliseconds(-1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];

                                var lstKaizenInMonth = lstKaiZen.Where(s => s.Month != null
                                    && (BeginMonth <= s.Month.Value && s.Month.Value <= BeginMonth.AddMonths(1).AddMinutes(-1))
                                    && lstProfileID.Contains(s.ProfileID.Value)).Select(m => m.ProfileID).Distinct().ToList();

                                //dro[item.ToString("MMM-yyyy")] = lstKaizenInMonth.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstKaizenInMonth.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        #region Perform

                        case "Perform":
                            _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                DateTime EndMonth = BeginMonth.AddMonths(1).AddMilliseconds(-1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];

                                var lstKaizenInMonth = lstKaiZen.Where(s => s.Month != null
                                    && (BeginMonth <= s.Month.Value && s.Month.Value <= BeginMonth.AddMonths(1).AddMinutes(-1))
                                    && s.MarkPerform != null && s.MarkPerform != 0
                                    && lstProfileID.Contains(s.ProfileID.Value)).ToList();

                                //dro[item.ToString("MMM-yyyy")] = lstKaizenInMonth.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstKaizenInMonth.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        #region Idea

                        case "Idea":
                            _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                DateTime EndMonth = BeginMonth.AddMonths(1).AddMilliseconds(-1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];

                                var lstKaizenInMonth = lstKaiZen.Where(s => s.Month != null
                                    && (BeginMonth <= s.Month.Value && s.Month.Value <= BeginMonth.AddMonths(1).AddMinutes(-1))
                                    && s.MarkIdea > 0
                                    && (s.MarkPerform == null || s.MarkPerform == 0)
                                    && lstProfileID.Contains(s.ProfileID.Value)).ToList();

                                //dro[item.ToString("MMM-yyyy")] = lstKaizenInMonth.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstKaizenInMonth.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        #region Fail

                        case "Fail":
                            _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                DateTime EndMonth = BeginMonth.AddMonths(1).AddMilliseconds(-1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];

                                var lstKaizenInMonth = lstKaiZen.Where(s => s.Month != null
                                    && (BeginMonth <= s.Month.Value && s.Month.Value <= BeginMonth.AddMonths(1).AddMinutes(-1))
                                    && (s.MarkPerform != null && s.MarkPerform == 0)
                                    && (s.MarkIdea != null && s.MarkIdea == 0)
                                    && lstProfileID.Contains(s.ProfileID.Value)).ToList();

                                //dro[item.ToString("MMM-yyyy")] = lstKaizenInMonth.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstKaizenInMonth.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        #region Detection

                        case "Detection":
                            _temp = 0;
                            foreach (var item in lstMonth)
                            {
                                DateTime BeginMonth = new DateTime(item.Year, item.Month, 1);
                                DateTime EndMonth = BeginMonth.AddMonths(1).AddMilliseconds(-1);
                                List<Guid> lstProfileID = dicProfileByMonth[BeginMonth];

                                var lstKaizenInMonth = lstKaiZen.Where(s => s.Month != null
                                    && (BeginMonth <= s.Month.Value && s.Month.Value <= BeginMonth.AddMonths(1).AddMinutes(-1))
                                    && s.CategoryName == "Phát Hiện"
                                    && lstProfileID.Contains(s.ProfileID.Value)).ToList();

                                //dro[item.ToString("MMM-yyyy")] = lstKaizenInMonth.Count;
                                _temp += 1;
                                dro["Month" + _temp] = lstKaizenInMonth.Count;
                            }
                            tb.Rows.Add(dro);
                            continue;

                        #endregion

                        default:
                            continue;
                    }
                }


                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }


        #endregion

        #region Hien.Nguyen - Báo cáo lương hưu

        public DataTable ReportPensions(List<Hre_ProfileEntity> ListProfile, DateTime Month, string UserLogin, bool IsCreateTemplate = false)
        {
            DataTable Table = new DataTable("ReportPensions");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("IsMale", typeof(bool));
            Table.Columns.Add("DateHire", typeof(DateTime));
            Table.Columns.Add("YearNumberBHXH", typeof(double));
            Table.Columns.Add("SALARYMEDIUMBHXH", typeof(double));
            //Table.Columns.Add("PensionRate");
            //Table.Columns.Add("Pension");
            //Table.Columns.Add("Total");
            //Table.Columns.Add("TotalNotCondition");

            if (IsCreateTemplate)
            {
                return Table;
            }

            #region Getdata
            var baservice = new BaseService();
            string status = string.Empty;
            List<object> listModel = new List<object>();
            listModel.AddRange(new object[5]);
            listModel[2] = Month;
            listModel[3] = 1;
            listModel[4] = Int32.MaxValue - 1;
            List<Ins_ProfileInsuranceMonthlyEntity> listProfileInsuranceMonthly = GetData<Ins_ProfileInsuranceMonthlyEntity>(listModel, ConstantSql.hrm_ins_sp_get_ProfileInsMonthly, UserLogin, ref status);
            //listProfileInsuranceMonthly = listProfileInsuranceMonthly.Where(m => m.ProfileID != null && ListProfile.Any(t => t.ID == m.ProfileID)).ToList();
            #endregion

            foreach (var i in ListProfile)
            {
                DataRow Row = Table.NewRow();
                Row["ProfileName"] = i.ProfileName;
                Row["DateHire"] = i.DateHire != null ? i.DateHire : DateTime.MinValue;
                Row["IsMale"] = i.Gender == HRM.Infrastructure.Utilities.EnumDropDown.Sexual.E_MALE.ToString() || i.Gender == "Nam" ? true : false;

                //lọc các dự liệu theo nhân viên đang duyệt
                List<Ins_ProfileInsuranceMonthlyEntity> listProfileInsuranceMonthlyByProfile = listProfileInsuranceMonthly.Where(m => m.ProfileID != null && m.ProfileID == i.ID && m.MonthYear != null && m.MonthYear <= DateTime.Now && m.MonthYear >= i.DateHire).ToList();

                //tổng số tháng có đóng bảo hiểm
                int MonthNumber = listProfileInsuranceMonthlyByProfile.Sum(m => m.IsSocialInsurance == null || m.IsSocialInsurance == false ? 0 : 1);
                double YearNumber = MonthNumber / 12;
                if (MonthNumber - YearNumber * 12 >= 3 && MonthNumber - YearNumber * 12 < 7)
                {
                    YearNumber += 0.5;
                }
                else if (MonthNumber - YearNumber * 12 >= 7)
                {
                    YearNumber++;
                }
                Row["YearNumberBHXH"] = YearNumber;

                //tính lương bình quân
                double TotalMoney = listProfileInsuranceMonthlyByProfile.Sum(m => m.MoneySocialInsurance != null ? (double)m.MoneySocialInsurance : 0);
                if (TotalMoney != 0 && MonthNumber != 0)
                {
                    Row["SALARYMEDIUMBHXH"] = TotalMoney / MonthNumber;
                }
                else
                {
                    Row["SALARYMEDIUMBHXH"] = 0;
                }


                Table.Rows.Add(Row);
            }

            return Table;
        }

        #endregion

        #region Hien.Nguyen - Báo cáo nhân viên bị giữ lương

        public DataTable ReportHoldSalary(Att_CutOffDurationEntity CutoffDuration, string OrgStructureIDs, string WorkPlace, string UserLogin)
        {
            #region Create Table
            DataTable Table = new DataTable("ReportHoldSalary");
            Table.Columns.Add("STT", typeof(int));
            Table.Columns.Add("CostcenterCode");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("Dept");
            Table.Columns.Add("DateHoldSalary", typeof(DateTime));
            Table.Columns.Add("Remark");
            #endregion

            using (var context = new VnrHrmDataContext())
            {

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var reposAttCutoffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                string status = string.Empty;
                #region GetData

                List<object> listModel = new List<object>();
                listModel.AddRange(new object[18]);
                listModel[2] = OrgStructureIDs;
                listModel[16] = 1;
                listModel[17] = Int32.MaxValue - 1;
                List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[4] = CutoffDuration.DateStart;
                listModel[5] = CutoffDuration.DateEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_HoldSalaryEntity> listHoldSalary = GetData<Sal_HoldSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_HoldSalary, UserLogin, ref status);

                #endregion

                #region Progress

                //lọc nv bị giữ lương
                listProfile = listProfile.Where(m => listHoldSalary.Any(t => t.ProfileID == m.ID)).ToList();

                for (int i = 0; i < listProfile.Count; i++)
                {
                    DataRow Row = Table.NewRow();
                    Row["STT"] = i + 1;
                    Row["CostcenterCode"] = listProfile[i].CostCentreCode;
                    Row["CodeEmp"] = listProfile[i].CodeEmp;
                    Row["ProfileName"] = listProfile[i].ProfileName;
                    Row["Dept"] = listProfile[i].OrgStructureName;
                    var HoldSalaryByProfile = listHoldSalary.FirstOrDefault(m => m.ProfileID == listProfile[i].ID);
                    if (HoldSalaryByProfile != null)
                    {
                        Row["DateHoldSalary"] = HoldSalaryByProfile != null && HoldSalaryByProfile.MonthSalary != null ? HoldSalaryByProfile.MonthSalary : null;

                        string remark = string.Empty;
                        remark = HoldSalaryByProfile.DayLeave != null ? "Nghỉ " + HoldSalaryByProfile.DayLeave.ToString() + " ngày\n" : string.Empty;
                        remark += HoldSalaryByProfile.IsLeaveContinuous == true ? "Nghỉ liên tục 3 ngày\n" : string.Empty;
                        remark += HoldSalaryByProfile.Terminate == true ? "Có thông tin nghỉ việc" : string.Empty;
                        Row["Remark"] = remark;
                    }
                    Table.Rows.Add(Row);
                }

                #endregion

                return Table;
            }
        }
        #endregion

        #region Hien.Nguyen BC Lương NV Nghỉ Việc

        public DataTable ReportSalaryProfileQuit(List<Hre_ProfileEntity> ListProfile, Att_CutOffDurationEntity CutoffDuration)
        {
            DataTable Table = new DataTable("ReportSalaryProfileQuit");



            return new DataTable();
        }

        #endregion

        #region Hien.Nguyen BC Tổng Hợp Tính Tiền Phép Năm, Sức Khỏe Tốt

        public DataTable ReportTotalAnnualSick(DateTime Year, string OrgStructure, string listProfileids, string WorkingPlaces)
        {
            DataTable Table = new DataTable();
            return new DataTable();
        }

        #endregion

        #region Hien.Nguyen  Bổ sung báo cáo danh sách nhân viên âm lương

        public DataTable ReportProfileSalaryNegative(string OrgStructure, List<Guid> ProfileIds, Att_CutOffDurationEntity CutoffDuration, List<Guid> SalaryRank, string UserLogin, bool IsCreateTemplate = false)
        {
            DataTable Table = new DataTable("ReportProfileSalaryNegative");
            Table.Columns.Add("CodeEmp");
            Table.Columns.Add("ProfileName");
            Table.Columns.Add("OrgStructure");

            #region GetData
            string status = string.Empty;
            var profileServices = new Hre_ProfileServices();
            var listModel = new List<object>();

            listModel = new List<object>();
            listModel.AddRange(new object[7]);
            listModel[5] = 1;
            listModel[6] = Int32.MaxValue - 1;
            var listElement = profileServices.GetData<Cat_ElementEntity>(listModel, ConstantSql.hrm_cat_sp_get_Element, UserLogin, ref status).Where(m => m.IsApplyGradeAll == true || m.GradePayrollID != null).ToList();

            if (IsCreateTemplate)
            {
                foreach (var i in listElement)
                {
                    if (!Table.Columns.Contains(i.ElementCode))
                    {
                        Table.Columns.Add(i.ElementCode);
                    }
                }
                return Table;
            }

            listModel = new List<object>();
            listModel.AddRange(new object[17]);
            listModel[2] = OrgStructure;
            listModel[15] = 1;
            listModel[16] = int.MaxValue - 1;
            var listProfile = profileServices.GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status);

            if (ProfileIds.Count > 0)
            {
                listProfile = listProfile.Where(m => ProfileIds.Contains(m.ID)).ToList();
            }

            if (SalaryRank.Count > 0)
            {
                listModel = new List<object>();
                listModel.AddRange(new object[10]);
                listModel[6] = CutoffDuration.DateEnd;
                listModel[8] = 1;
                listModel[9] = Int32.MaxValue - 1;
                List<Sal_BasicSalaryEntity> ListBasicSalary = GetData<Sal_BasicSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_BasicPayroll, UserLogin, ref status);
                var ListProfileByRank = ListBasicSalary.Where(m => m.RankRateID != null && SalaryRank.Contains((Guid)m.RankRateID)).Select(m => m.ProfileID).ToList();
                listProfile = listProfile.Where(m => ListProfileByRank.Contains(m.ID)).ToList();
            }

            listModel = new List<object>();
            listModel.Add(null);
            listModel.Add(CutoffDuration.ID);
            listModel.Add(null);
            listModel.Add(null);
            listModel.Add(null);
            listModel.Add(null);
            listModel.Add(null);
            listModel.Add(1);
            listModel.Add(int.MaxValue - 1);
            var lstPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(listModel, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).ToList();
            Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
            string REALWAGES = Sys_Services.GetConfigValue<string>(AppConfig.HRM_SAL_ELEMENT_REALWAGES);

            List<Sal_PayrollTableItemEntity> ListPayrollTableItemResult = new List<Sal_PayrollTableItemEntity>();

            //lọc nhân viên có phần tử lớn hơn 0
            if (!string.IsNullOrEmpty(REALWAGES))
            {
                string[] listPayrollTable = lstPayrollTableItem.Select(m => m.PayrollTableID.ToString()).ToArray();
                foreach (var i in listPayrollTable)
                {
                    //lấy các item thuộc PayrollTable tương ứng
                    var lstPayrollTableItemByTable = lstPayrollTableItem.Where(m => m.PayrollTableID.ToString() == i).ToList();
                    var Item = lstPayrollTableItemByTable.FirstOrDefault(m => m.Code == REALWAGES);
                    if (Item != null)
                    {
                        double _tmp = int.MaxValue;
                        double.TryParse(Item.Value, out _tmp);
                        if (_tmp != int.MaxValue && _tmp < 0)
                        {
                            ListPayrollTableItemResult.AddRange(lstPayrollTableItemByTable);
                        }
                    }
                }
            }

            //lọc nhân viên nghỉ việc
            listProfile = listProfile.Where(m => m.DateQuit != null && m.DateQuit <= CutoffDuration.DateEnd && ListPayrollTableItemResult.Any(t => t.ProfileID == m.ID)).ToList();

            #endregion

            foreach (var profile in listProfile)
            {
                DataRow Row = Table.NewRow();
                Row["CodeEmp"] = profile.CodeEmp;
                Row["ProfileName"] = profile.ProfileName;
                Row["OrgStructure"] = profile.OrgStructureName;

                var lstPayrollTableItemByProfile = ListPayrollTableItemResult.Where(m => m.ProfileID == profile.ID).ToList();
                foreach (var tableItem in lstPayrollTableItemByProfile)
                {
                    if (Table.Columns.Contains(tableItem.Code))//đã tồn tại cột
                    {
                        if (tableItem.ValueType == EnumDropDown.ElementDataType.Double.ToString())
                        {
                            double _tmp = 0;
                            Double.TryParse(tableItem.Value, out _tmp);
                            Row[tableItem.Code] = _tmp;
                        }
                        else if (tableItem.ValueType == EnumDropDown.ElementDataType.Datetime.ToString())
                        {
                            DateTime _tmp = DateTime.MinValue;
                            DateTime.TryParse(tableItem.Value, out _tmp);
                            Row[tableItem.Code] = _tmp;
                        }
                        else
                        {
                            Row[tableItem.Code] = tableItem.Value;
                        }
                    }
                    else//chưa tồn tại
                    {
                        if (tableItem.ValueType == EnumDropDown.ElementDataType.Double.ToString())
                        {
                            double _tmp = 0;
                            Table.Columns.Add(tableItem.Code, typeof(double));
                            Double.TryParse(tableItem.Value, out _tmp);
                            Row[tableItem.Code] = _tmp;
                        }
                        else if (tableItem.ValueType == EnumDropDown.ElementDataType.Datetime.ToString())
                        {
                            DateTime _tmp = DateTime.MinValue;
                            Table.Columns.Add(tableItem.Code, typeof(DateTime));
                            DateTime.TryParse(tableItem.Value, out _tmp);
                            Row[tableItem.Code] = _tmp;
                        }
                        else
                        {
                            Table.Columns.Add(tableItem.Code, typeof(string));
                            Row[tableItem.Code] = tableItem.Value;
                        }
                    }
                }
                Table.Rows.Add(Row);
            }


            return Table;
        }

        #endregion

        #region Tin.Nguyen - Quỹ Lương

        public DataTable GetPayrollEstimateSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Sal_PayrollEstimateDetailEntity");
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.QuantityEmp);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.SalaryAverage);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.LeaveUnpaid);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNormal);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeHoliday);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeWeekend);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightNormal);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightHoliday);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightWeekend);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.AmountHour);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.AmountLeaveDay);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.AmountOvertime);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.AmountTotal);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.RateAdjust);
                tb.Columns.Add(Sal_PayrollEstimateDetailEntity.FieldNames.BonusBudget);



                return tb;
            }
        }

        public DataTable GetPayrollEstimate(Guid? cutOffID, Guid? orgID, Guid? payrollID, string orgType, string statusEmp, double? rateAdjust, double? bounusBudget, string UserLogin, bool IsCreateTenplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = GetPayrollEstimateSchema();
                var status = string.Empty;
                if (IsCreateTenplate)
                {
                    return table;
                }
                var objEstimate = new List<object>();
                objEstimate.AddRange(new object[9]);
                objEstimate[0] = cutOffID;
                objEstimate[1] = orgID;
                objEstimate[2] = payrollID;
                objEstimate[3] = orgType;
                objEstimate[4] = statusEmp;
                objEstimate[5] = rateAdjust;
                objEstimate[6] = bounusBudget;
                objEstimate[7] = 1;
                objEstimate[8] = int.MaxValue - 1;
                var lstEstimate = GetData<Sal_PayrollEstimateEntity>(objEstimate, ConstantSql.hrm_sal_sp_get_Estimate, UserLogin, ref status).ToList();

                var objEstimateDetail = new List<object>();
                objEstimateDetail.Add(1);
                objEstimateDetail.Add(int.MaxValue - 1);
                var lstEstimateDetail = GetData<Sal_PayrollEstimateDetailEntity>(objEstimateDetail, ConstantSql.hrm_sal_sp_get_EstimateDetail, UserLogin, ref status).ToList();

                foreach (var item in lstEstimate)
                {
                    var lstEstimateDetailByEsId = lstEstimateDetail.Where(s => s.PayrollEstimateID != null && s.PayrollEstimateID.Value == item.ID).ToList();
                    foreach (var detailEntity in lstEstimateDetailByEsId)
                    {
                        DataRow dr = table.NewRow();
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OrgStructureName] = detailEntity.OrgStructureName;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OrgStructureCode] = detailEntity.OrgStructureCode;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.QuantityEmp] = detailEntity.QuantityEmp;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.SalaryAverage] = detailEntity.SalaryAverage;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.LeaveUnpaid] = detailEntity.LeaveUnpaid;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNormal] = detailEntity.OvertimeNormal;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeHoliday] = detailEntity.OvertimeHoliday;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeWeekend] = detailEntity.OvertimeWeekend;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightNormal] = detailEntity.OvertimeNightNormal;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightHoliday] = detailEntity.OvertimeNightHoliday;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.OvertimeNightWeekend] = detailEntity.OvertimeNightWeekend;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.AmountHour] = detailEntity.AmountHour;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.AmountLeaveDay] = detailEntity.AmountLeaveDay;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.AmountOvertime] = detailEntity.AmountOvertime;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.AmountTotal] = detailEntity.AmountTotal;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.RateAdjust] = detailEntity.RateAdjust;
                        dr[Sal_PayrollEstimateDetailEntity.FieldNames.BonusBudget] = detailEntity.BonusBudget;


                        table.Rows.Add(dr);

                    }
                }

                return table;

            }
        }
        #endregion

        #region Tin.Nguyen - BC So Sánh Lương

        public DataTable GetReportComparePayrollSchema(Guid[] _cutOffDurationIDs, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Sal_ReportComparePayrollEntity");
                tb.Columns.Add("ElementName");
                var attCutOffDurationServices = new Att_CutOffDurationServices();
                var lstObjCutOff = new List<object>();
                lstObjCutOff.Add(null);
                lstObjCutOff.Add(1);
                lstObjCutOff.Add(int.MaxValue - 1);
                var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                lstCutOffDuration = lstCutOffDuration.Where(s => _cutOffDurationIDs.Contains(s.ID)).ToList();
                foreach (var item in lstCutOffDuration)
                {
                    tb.Columns.Add(item.CutOffDurationName);
                }
                return tb;
            }
        }


        public DataTable GetReportComparePayroll(Guid[] cutofDurationIds, string UserLogin, string orderNumber, Guid? workPlaceID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                DataTable tb = GetReportComparePayrollSchema(cutofDurationIds, UserLogin);

                var objProfile = new List<object>();
                objProfile.AddRange(new object[17]);
                objProfile[2] = orderNumber;
                objProfile[15] = 1;
                objProfile[16] = int.MaxValue - 1;
                var lstProfile = GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status).ToList();


                var attCutOffDurationServices = new Att_CutOffDurationServices();
                var lstObjCutOff = new List<object>();
                lstObjCutOff.Add(null);
                lstObjCutOff.Add(1);
                lstObjCutOff.Add(int.MaxValue - 1);
                var lstCutOffDuration = attCutOffDurationServices.GetData<Att_CutOffDurationEntity>(lstObjCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, UserLogin, ref status).ToList();
                if (cutofDurationIds != null)
                {
                    lstCutOffDuration = lstCutOffDuration.Where(s => cutofDurationIds.Contains(s.ID)).ToList();
                }

                if (workPlaceID != null)
                {
                    lstProfile = lstProfile.Where(s => s.WorkPlaceID != null && workPlaceID.Value == s.WorkPlaceID.Value).ToList();
                }
                List<Guid> lstProfileIds = new List<Guid>();
                if (lstProfile.Count > 0)
                {
                    lstProfileIds = lstProfile.Select(s => s.ID).ToList();
                }

                List<Guid> lstAttTableIds = new List<Guid>();
                var obAttTable = new List<object>();
                obAttTable.AddRange(new object[7]);
                obAttTable[5] = 1;
                obAttTable[6] = int.MaxValue - 1;
                var lstAttTable = GetData<Att_AttendanceTableEntity>(obAttTable, ConstantSql.hrm_att_sp_get_attdancetable, UserLogin, ref status).ToList();

                var objPayroll = new List<object>();
                objPayroll.AddRange(new object[6]);
                objPayroll[4] = 1;
                objPayroll[5] = int.MaxValue - 1;
                var lstPayroll = GetData<Sal_PayrollTableEntity>(objPayroll, ConstantSql.hrm_sal_sp_get_PayrollTable, UserLogin, ref status).ToList();

                List<Guid> lstPayrollIds = new List<Guid>();
                if (cutofDurationIds != null)
                {
                    lstAttTable = lstAttTable.Where(s => s.CutOffDurationID != null && cutofDurationIds.Contains(s.CutOffDurationID.Value) && !string.IsNullOrEmpty(s.CutOffDurationName) && lstProfileIds.Contains(s.ProfileID)).OrderByDescending(s => s.DateUpdate).ToList();
                    lstAttTableIds = lstAttTable.Select(s => s.ID).ToList();

                    lstPayroll = lstPayroll.Where(s => s.CutOffDurationID != null && cutofDurationIds.Contains(s.CutOffDurationID.Value) && !string.IsNullOrEmpty(s.CutOffDurationName) && lstProfileIds.Contains(s.ProfileID)).OrderByDescending(s => s.DateUpdate).Distinct().ToList();
                    lstPayrollIds = lstPayroll.Select(s => s.ID).ToList();
                }

                var obiPayrollItem = new List<object>();
                obiPayrollItem.AddRange(new object[9]);
                obiPayrollItem[7] = 1;
                obiPayrollItem[8] = int.MaxValue - 1;
                var lstPayrollTableItem = GetData<Sal_PayrollTableItemEntity>(obiPayrollItem, ConstantSql.hrm_sal_sp_get_PayrollTableItem, UserLogin, ref status).Where(s => s.ValueType == EnumDropDown.ElementDataType.Double.ToString()).ToList();

                var objAttTableItem = new List<object>();
                objAttTableItem.AddRange(new object[5]);
                objAttTableItem[3] = 1;
                objAttTableItem[4] = int.MaxValue - 1;
                var lstAttTableItem = GetData<Att_AttendanceTableItemEntity>(objAttTableItem, ConstantSql.hrm_att_sp_get_AttendanceTableItem, UserLogin, ref status);

                if (lstAttTableIds.Count > 0)
                {
                    lstAttTableItem = lstAttTableItem.Where(s => lstAttTableIds.Contains(s.AttendanceTableID)).ToList();
                }

                List<string> lstElementCode = new List<string>();
                if (lstPayrollIds.Count > 0)
                {
                    lstPayrollTableItem = lstPayrollTableItem.Where(s => lstPayrollIds.Contains(s.PayrollTableID)).ToList();
                    lstElementCode = lstPayrollTableItem.Select(s => s.Code).Distinct().ToList();
                }

                //lấy dữ liệu bên công
                List<string> lstAttElement = new List<string>();
                lstAttElement.Add("No. of empl");
                lstAttElement.Add("No. of working days_office");
                lstAttElement.Add("Overtime (hours)");
                lstAttElement.Add("Overtime (convert to 100%)");

                //foreach (var item in lstAttElement)
                //{
                //    DataRow dr = tb.NewRow();
                //    dr["ElementName"] = item;
                //    foreach (var cutOf in lstCutOffDuration)
                //    {
                //        var lstAttIdsByCutOf = lstAttTable.Where(s => s.CutOffDurationID != null && s.CutOffDurationID.Value == cutOf.ID && lstProfileIds.Contains(s.ProfileID)).Select(s => s.ID).ToList();
                //        var lstItemByAttId = lstAttTableItem.Where(s => lstAttIdsByCutOf.Contains(s.AttendanceTableID)).ToList();
                //        if(tb.Columns.Contains(cutOf.CutOffDurationName))
                //        {
                //            dr[cutOf.CutOffDurationName] = lstItemByAttId.Count();
                //        }
                //    }
                //}


                foreach (var item in lstElementCode)
                {
                    DataRow dr = tb.NewRow();
                    dr["ElementName"] = item;
                    foreach (var cutOf in lstCutOffDuration)
                    {
                        var lstPayrollIdsByCutOf = lstPayroll.Where(s => s.CutOffDurationID != null && s.CutOffDurationID.Value == cutOf.ID && lstProfileIds.Contains(s.ProfileID)).Select(s => s.ID).ToList();
                        var lstItemByPayrollId = lstPayrollTableItem.Where(s => lstPayrollIdsByCutOf.Contains(s.PayrollTableID) && item == s.Code).ToList();
                        if (tb.Columns.Contains(cutOf.CutOffDurationName))
                        {
                            dr[cutOf.CutOffDurationName] = lstItemByPayrollId.Sum(s => double.Parse(s.Value));
                        }
                    }
                    tb.Rows.Add(dr);
                }

                return tb;
            }
        }


        #endregion


    }
}
