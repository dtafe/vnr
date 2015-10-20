using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportWorkHistoryDeptModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        public string StatusView { get; set; }
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_JRType)]
        public Guid? TypeOfTransferID { get; set; }
        public Guid? OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_FirstName)]
        public string FirstName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameFamily)]
        public string NameFamily { get; set; }

        public string UnitName { get; set; }
        public string DivisionName { get; set; }

        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkPlace)]
        public string WorkPlace { get; set; }

        public string SalaryClassName { get; set; }

        public string UnitNameOld { get; set; }
        public string DivisionNameOld { get; set; }

        public string DepartmentNameOld { get; set; }
        public string SectionNameOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleOld)]
        public string JobTitleOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionOld)]
        public string PositionOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPlaceOld)]
        public string WorkPlaceOld { get; set; }

        public string SalaryClassNameOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_DateEffective)]
        public DateTime? DateEffective { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureName { get; set; }
   

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgOld)]
        public string OrgOld { get; set; }
 
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkLocationOld)]
        public string WorkLocationOld { get; set; }
   
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }

        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Notes { get; set; }
    
   
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string WorkPlaceName { get; set; }

        public partial class FieldNames
        {
            public const string StatusView = "StatusView";
            public const string Status = "Status";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string FirstName = "FirstName";
            public const string NameFamily = "NameFamily";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string WorkPlace = "WorkPlace";
            public const string WorkPlaceOld = "WorkPlaceOld";
            public const string PositionOld = "PositionOld";
            public const string OrgOld = "OrgOld";
            public const string JobTitleOld = "JobTitleOld";
            public const string WorkLocationOld = "WorkLocationOld";
            public const string DateEffective = "DateEffective";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string UnitName = "UnitName";
            public const string DivisionName = "DivisionName";
            public const string DepartmentName = "DepartmentName";
            public const string SectionName = "SectionName";
            public const string Description = "Description";

            public const string UnitNameOld = "UnitNameOld";
            public const string DivisionNameOld = "DivisionNameOld";
            public const string DepartmentNameOld = "DepartmentNameOld";
            public const string SectionNameOld = "SectionNameOld";

            public const string SalaryClassName = "SalaryClassName";
            public const string SalaryClassNameOld = "SalaryClassNameOld";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
