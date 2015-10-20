using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileDeptCountModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileId)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DateHire)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_PlaceOfBirth)]
        public string PlaceOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DayOfBirth)]
        public DateTime? DayOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public double LastMonthCost { get; set; }
        public double LastMonthCostCurrent { get; set; }
        public double LastMonthOTCost { get; set; }
        public double LastMonthOTCostCurrent { get; set; }
        public double HeadCountPlanOfficial { get; set; }
        public double HeadCountPlanSeasonal { get; set; }
        public double TotalSalaryBudget { get; set; }
        public double TotalOvertimeBudget { get; set; }
        public string EpmloyeeName { get; set; }
        public string ActualTraining { get; set; }
        public string Actualcasual { get; set; }

        public Guid ExportID { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string DayOfBirth = "DayOfBirth";
            public const string CodeEmp = "CodeEmp";
            public const string LastMonthCost = "LastMonthCost";
            public const string LastMonthCostCurrent = "LastMonthCostCurrent";
            public const string LastMonthOTCost = "LastMonthOTCost";
            public const string LastMonthOTCostCurrent = "LastMonthOTCostCurrent";
            public const string HeadCountPlanOfficial = "HeadCountPlanOfficial";
            public const string HeadCountPlanSeasonal = "HeadCountPlanSeasonal";
            public const string TotalSalaryBudget = "TotalSalaryBudget";
            public const string TotalOvertimeBudget = "TotalOvertimeBudget";
            public const string EpmloyeeName = "EpmloyeeName";
            public const string ActualTraining = "ActualTraining";
            public const string Actualcasual = "Actualcasual";
        }
    }
}
