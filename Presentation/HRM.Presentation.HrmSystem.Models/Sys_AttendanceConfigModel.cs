using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_AttendanceConfigModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_Config)]
        public string AnnualConfig { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_Compute)]
        public string AnnualCompute { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? AnnualDateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string AnnualOrgStructure { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_AnnualFirstMonth)]
        public bool? AnnualFirstMonth { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_AnnualMonth)]
        public bool? AnnualMonth { get; set; }



        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_Config)]
        public string InsuranceConfig { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_AttendanceConfig_Compute)]
        public string InsuranceCompute { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? InsuranceDateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string InsuranceOrgStructure { get; set; }

        public partial class FieldNames
        {
            public const string AnnualConfig = "AnnualConfig";
            public const string AnnualCompute = "AnnualCompute";
            public const string AnnualDateFrom = "AnnualDateFrom";
            public const string AnnualOrgStructure = "AnnualOrgStructure";

            public const string InsuranceConfig = "InsuranceConfig";
            public const string InsuranceCompute = "InsuranceCompute";
            public const string InsuranceDateFrom = "InsuranceDateFrom";
            public const string InsuranceOrgStructure = "InsuranceOrgStructure";
        }

        
    }
}
