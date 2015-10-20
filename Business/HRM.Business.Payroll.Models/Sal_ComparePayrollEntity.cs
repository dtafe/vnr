using HRM.Business.Attendance.Models;
using HRM.Business.BaseModel;
using HRM.Business.Category.Models;
using HRM.Business.Canteen.Models;
using HRM.Business.Evaluation.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Insurance.Models;
using System;
using System.Collections.Generic;

namespace HRM.Business.Payroll.Models
{
    public class Sal_ComparePayrollEntity : HRMBaseModel
    {

        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string ElementName { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }


        public partial class FieldNames
        {

            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string ElementName = "ElementName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string GroupNumberProfile = "GroupNumberProfile";
        }
    }

    /// <summary>
    /// Lớp lưu số tiền và số lượng nhân viên của số tiền đó.
    /// sử dụng cho tính lương hoa hồng
    /// </summary>

    public class ValueCount
    {
        public ValueCount(double value, int count)
        {
            this.Value = value;
            this.Count = count;
        }
        public double Value { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Class dùng để lưu trữ dữ liệu cho hàm tính lương
    /// </summary>
    public class ComputePayrollDataModel
    {
        public List<Sal_GradeEntity> listGrade { get; set; }
        public List<Cat_ElementEntity> listElement_All { get; set; }
        public List<Cat_UsualAllowanceEntity> listUsualAllowance { get; set; }
        public List<Cat_UnusualAllowanceCfgEntity> listUnusualAllowanceCfg { get; set; }
        public List<Sal_UnusualAllowanceEntity> listSalUnusualAllowance { get; set; }
        public List<Att_AttendanceTableEntity> listAttendanceTable { get; set; }
        public List<Att_CutOffDurationEntity> listCutOffDuration { get; set; }
        public List<Hre_DisciplineEntity> listDiscipline { get; set; }
        public List<Hre_StopWorkingEntity> listHre_StopWorking { get; set; }
        public List<Hre_DependantEntity> listDependant { get; set; }
        public List<Sal_BasicSalaryEntity> listBasicSalary { get; set; }
        public List<Sal_SalaryDepartmentItemEntity> listSal_SalaryDepartmentItem { get; set; }
        public List<Cat_GradePayrollEntity> listCat_GradePayroll { get; set; }
        public List<Ins_ProfileInsuranceMonthlyEntity> listInsurance { get; set; }
        public List<Eva_PerformanceEntity> listEva_Performance { get; set; }
        public List<Cat_OvertimeTypeEntity> listOvertimeType { get; set; }
        public List<Cat_LeaveDayTypeEntity> listLeavedayType { get; set; }
        public List<Cat_ShiftEntity> listCat_Shift { get; set; }
        public List<Cat_PositionEntity> listPosition { get; set; }
        public List<Eva_SalesTypeEntity> listSalesType { get; set; }
        public List<Eva_BonusSalaryEntity> listEva_BonusSalary { get; set; }
        public List<Sal_RevenueRecordEntity> listRevenueRecord { get; set; }
        public List<Sal_RevenueForShopEntity> listRevenueForShop { get; set; }
        public List<Sal_RevenueForProfileEntity> listRevenueForProfile { get; set; }
        public List<Cat_KPIBonusEntity> listKPIBonus { get; set; }
        public List<Cat_ShopEntity> listShop { get; set; }
        public List<Sal_ItemForShopEntity> listItemForShop { get; set; }
        public List<Sal_LineItemForShopEntity> listLineItemForShop { get; set; }
        public List<Cat_UnitPriceEntity> listCat_UnitPrice { get; set; }
        public List<Att_TimeSheetEntity> listAtt_TimeSheet { get; set; }
        public List<Cat_RoleEntity> listCat_Role { get; set; }
        public List<Cat_JobTypeEntity> listCat_JobType { get; set; }
        public List<Sal_HoldSalaryEntity> listSal_HoldSalary { get; set; }
        public List<Hre_HDTJobEntity> listHre_HDTJob_All { get; set; }
        public List<Att_AttendanceTableEntity> Att_AttendanceTable_Prev { get; set; }
        public List<Sal_BasicSalaryEntity> listBasicSalaryT3 { get; set; }
        public List<Sal_UnusualAllowanceEntity> listUnusualAllowanceT3 { get; set; }
        public List<Hre_ProfilePartyUnionEntity> listProfilePartyUnion { get; set; }
        public List<Cat_DayOffEntity> listDayOff { get; set; }
        public List<Cat_UnAllowCfgAmountEntity> listUnAllowCfgAmount { get; set; }
        public List<Can_SumryMealRecordEntity> listSumryMealRecord { get; set; }
        public List<Cat_CurrencyEntity> listCurrency { get; set; }
        public List<Cat_ExchangeRateEntity> listExchangeRate { get; set; }
        public List<Hre_WorkHistoryEntity> listWorkHistory { get; set; }
        public List<Sal_PayCommissionItemEntity> listPayCommissionItem { get; set; }
        public List<Att_AnnualDetailEntity> listAnnualDetail { get; set; }
        public List<Sal_SalaryInformationEntity> listSalaryInformation { get; set; }
        public List<Att_RosterEntity> listRoster { get; set; }
        public List<Att_AttendanceTableItemEntity> listAttendanceTableItem { get; set; }
        public List<Att_OvertimeEntity> listOverTime { get; set; }
        public List<Cat_PerformanceTypeEntity> listPerformanceType { get; set; }
        public string Status { get; set; }
    }


}
