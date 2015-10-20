using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;


namespace HRM.Presentation.Payroll.Models
{
    public class UnusualEDModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public System.Guid ProfileID { get; set; }
       [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }
       [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UnusualEDTypeID)]
        public System.Guid UnusualEDTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_Amount)]
        public double Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ChargePIT)]
        public bool ChargePIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_MonthStart)]
        public System.DateTime MonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_MonthEnd)]
        public System.DateTime MonthEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_UserUpdate)]
        public string UserUpdate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_DateUpdate)]
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string UnusualEDTypeID = "UnusualEDTypeID";
            public const string Amount = "Amount";
            public const string ChargePIT = "ChargePIT";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class UnusualEDSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }
    /// <summary>
    /// Model tạm của UnusualEDSearchModel
    /// </summary>
    public class UnusualEDSearchModelTam
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }
    public class AllowanceEvaluationYearSearchModel
    {
        public string TypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkPlaceID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }


    }
}
