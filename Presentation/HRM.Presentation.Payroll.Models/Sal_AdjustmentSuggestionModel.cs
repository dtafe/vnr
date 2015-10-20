using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_AdjustmentSuggestionModel : BaseViewModel
    {
      
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryJobGroupName)]
        public string SalaryJobGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }

        //public String UserUpdate { get; set; }
        //public DateTime DateUpdate { get; set; }
        public String ContractNo { get; set; }

  
        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryJobGroupName)]
        public Guid? SalaryJobGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryRankName)]
        public string SalaryRankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryRankName)]
        public Guid? SalaryRankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryClassName)]
        public string SalaryClassName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryClassName)]
        public Guid? SalaryClassID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_DateOfEffect)]
         [DataType(DataType.Date)]
         public DateTime DateOfEffect { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_GrossAmount)]
        public string GrossAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_GrossAmount)]
        public double GrossAmountMoney { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }

        #region [Hien.Nguyen] - 20141029 - Thêm field trong module Porttal
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_InsuranceAmount)]
        public double InsuranceAmount { get; set; }
        public System.Guid CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_CurrencyName)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_DateOfRequest)]
        [DataType(DataType.Date)]
        public DateTime? DateOfRequest { get; set; }

        [DataType(DataType.Date)]
        public DateTime MonthYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_JobLevel)]
        public int? JobLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_DecisionNo)]
        public string DecisionNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryClassID)]
        public Guid? ClassRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalaryRankID)]
        public Guid? RankRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_PersonalRate)]
        public double? PersonalRate { get; set; }
       
        public Guid? CurrencyID1 { get; set; }
        public string CurrencyName1 { get; set; }
        public Guid? CurrencyID2 { get; set; }
        public string CurrencyName2 { get; set; }
        public Guid? CurrencyID3 { get; set; }
        public string CurrencyName3 { get; set; }
        public Guid? CurrencyID4 { get; set; }
        public string CurrencyName4 { get; set; }
        public Guid? CurrencyID5 { get; set; }
        public string CurrencyName5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount1)]
        public double? AllowanceAmount1 { get; set; }
        public string AllowanceAmountName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount2)]
        public double? AllowanceAmount2 { get; set; }
        public string AllowanceAmountName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount3)]
        public double? AllowanceAmount3 { get; set; }
        public string AllowanceAmountName3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount4)]
        public double? AllowanceAmount4 { get; set; }
        public string AllowanceAmountName4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount5)]
        public double? AllowanceAmount5 { get; set; }
        public string AllowanceAmountName5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AllowanceAmount6)]
        public double? AllowanceAmount6 { get; set; }
        public string AllowanceAmountName6 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_SalAdjustmentCampaignName)]
        public Guid? SalAdjustmentCampaignID { get; set; }
        public string SalAdjustmentCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_AmountTotal)]
        public double? AmountTotal { get; set; }

        //public Nullable<int> BasicSalaryFrom { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_From)]

        //public Nullable<int> BasicSalaryTo { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Payroll_BasicSalary_To)]
        
        private Guid _id = Guid.Empty;
        public Guid BasicSalary_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        #endregion

        public string Status { get; set; }
        public string StatusView { get; set; }
        public string IDs { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

        

        public partial class FieldNames
        {
            public const string Status = "Status";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";
            public const string StatusView = "StatusView";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";       
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string DepartmentName = "DepartmentName";
            public const string SectionName = "SectionName";
            public const string DateOfBirth = "DateOfBirth";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";
            public const string Note = "Note";
            public const string InsuranceAmount = "InsuranceAmount";

            public const string AllowanceAmount1 = "AllowanceAmount1";
            public const string AllowanceAmount2 = "AllowanceAmount2";
            public const string AllowanceAmount3 = "AllowanceAmount3";
            public const string AmountTotal = "AmountTotal";
            public const string ContractNo = "ContractNo";

 

            public const string SalaryJobGroupName = "SalaryJobGroupName";
            public const string SalaryRankName = "SalaryRankName";
            public const string SalaryClassName = "SalaryClassName";
            public const string GrossAmount = "GrossAmount";
            public const string DateOfRequest = "DateOfRequest";
           
            public const string CurrencyID = "CurrencyID";          

            public const string DateOfEffect = "DateOfEffect";
            public const string CurrencyName = "CurrencyName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            
        }
    }

    public class Sal_AdjustmentSuggestionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        public string OrderNumber { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string SalaryJobGroupName = "SalaryJobGroupName";
        }
    }
}
