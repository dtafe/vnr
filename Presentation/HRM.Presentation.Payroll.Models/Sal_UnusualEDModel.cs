using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_UnusualAllowanceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileCode)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        //loai hieu hy
        //public System.Guid UnusualEDTypeID { get; set; }
        public Nullable<System.Guid> UnusualEDTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualAllowanceCfgName)]
        public string UnusualAllowanceCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_BudgetId)]
        public Guid? BudgetId { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_BudgetName)]
        public string BudgetName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        //public Guid CurrencyID { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public string CurrencyName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_UnusualED_DateEffect)]
        //public System.DateTime MonthStart { get; set; }

        public Nullable<System.DateTime> MonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_MonthEnd)]
        //public System.DateTime MonthEnd { get; set; }
        public Nullable<System.DateTime> MonthEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_AddToHourlyRate)]
        public bool? AddToHourlyRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ChargePIT)]
        public bool? ChargePIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_IsFollowDay)]
        public bool? IsFollowDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_Notes)]
        public string Notes { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UserUpdate)]
        //public string UserUpdate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_DateUpdate)]
        //public Nullable<System.DateTime> DateUpdate { get; set; }
        #region phu cap nuoi con nho
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        public string StatusTranslate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DayOfBirth)]
        public string YearOfBirth { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOld)]
        public string YearOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthOld)]
        public string MonthOld { get; set; }
 
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualEDChild_NoCompensation)]
        public int? NoCompensation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualEDChild_DateSubmitDoc)]
        public DateTime? DateSubmitDoc { get; set; }
        #endregion
        #region thanh toan phieu hy
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_TypeUnusual)]
        public string TypeUnusual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_RelativeName)]
        public Nullable<System.Guid> RelativeID { get; set; }//
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_RelativeName)]
        public string RelativeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_RelativeTypeName)]
        public Nullable<System.Guid> RelativeTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_RelativeTypeName)]
        public string RelativeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_DateOccur)]
        public Nullable<System.DateTime> DateOccur { get; set; }
        #endregion
        #region Phụ cấp đánh giá năm
           [DisplayName(ConstantDisplay.HRM_Category_Element_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualDetail_Year)]
        public DateTime Year { get; set; }
         [DisplayName(ConstantDisplay.HRM_Payroll_ComparePayroll_ElementIDs)]
        public Guid? Element { get; set; }
        #endregion
        public string ProfileIDs { get; set; }
        public string ProfileIDExclusion { get; set; }
        public string OrgStructureID { get; set; }
        public string OrgStructureIDs { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkingPlaceID { get; set; }
        public string OrgStructureName { get; set; }
        public string WorkPlaceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }
        private Guid _id = Guid.Empty;
        public bool? IsGeneralProfile { get; set; }
        public Guid UnusualED_ID
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
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_AmountofOffset)]
        public Nullable<double> AmountOfOffSet { get; set; }
        
         [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public Guid? CurrencyID2 { get; set; }
         public string UnusualAllowanceCfgTypeName { get; set; }
         public string UnusualEDTypeCode { get; set; }
         public string DepartmentName { get; set; }
         public Nullable<System.DateTime> DateOfBirth { get; set; }
         public string E_DEPARTMENT { get; set; }
         public string E_UNIT { get; set; }
         public string E_DIVISION { get; set; }
         public string E_SECTION { get; set; }
         public string E_TEAM { get; set; }
         public string ExProfileIDs { get; set; }

        public partial class FieldNames
        {
            public const string UnusualAllowanceCfgTypeName = "UnusualAllowanceCfgTypeName";
            public const string CodeEmp = "CodeEmp";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";
            public const string ProfileName = "ProfileName";
            public const string UnusualEDTypeID = "UnusualEDTypeID";
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";
            public const string BudgetId = "BudgetId";
            public const string BudgetName = "BudgetName";
            public const string Type = "Type";
            public const string Amount = "Amount";
            public const string AmountOfOffSet = "AmountOfOffSet";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string ChargePIT = "ChargePIT";
            public const string IsFollowDay = "IsFollowDay";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string Status = "Status";
            public const string NoCompensation = "NoCompensation";
            public const string YearOfBirth = "YearOfBirth";
            public const string DateHire = "DateHire";
            public const string StatusTranslate = "StatusTranslate";
            public const string DateCreate = "DateCreate";
            public const string OrgStructureName = "OrgStructureName";
            public const string SectionName = "SectionName";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string DepartmentName = "DepartmentName";
            public const string DateOfBirth = "DateOfBirth";

            public const string E_DEPARTMENT = "E_DEPARTMENT";

            #region thanh toan phieu hy
            public const string TypeUnusual = "TypeUnusual";
            public const string RelativeName = "RelativeName";
            public const string RelativeTypeName = "RelativeTypeName";
            public const string DateOccur = "DateOccur";

            #endregion

        }
    }
    public class Sal_UnusualEDSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileCode)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }
           
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        public System.Guid? UnusualEDTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_UnusualAllowanceCfg_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkPlaceID { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
                
    }
    public class Sal_UnusualEDMutiModel
    {
        public Guid ID { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
    }

    public class Sal_UnusualAllowanceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        public Nullable<System.Guid> UnusualEDTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_UnusualED_DateEffect)]
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }
    public class Sal_UnusualAllowanceRelativeMuti
    {
        public Guid ID { get; set; }
        public string RelativeName { get; set; }
        public DateTime? DateHire { get; set; }

    }
    public class Sal_UnusualAllowanceRelativeTypeMuti
    {
        public Guid ID { get; set; }
        public string RelativeTypeName { get; set; }

    }
    public class Sal_UnusualAllowanceCurrencyMuti
    {
        public Guid ID { get; set; }
        public string CurrencyName { get; set; }

    }
    public class Sal_UnusualAllowanceAmountMuti
    {
        public Guid ID { get; set; }
        public double? Amount { get; set; }

    }
    public class Sal_UnusualAllowanceYearOfBirth
    {
        public Guid ID { get; set; }
        public string StrYearOfBirth { get; set; }
        public DateTime? YearOfBirth { get; set; }
        public string DateEnd { get; set; }

    }
}
