using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileNotContractModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateHireFrom { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateHireTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        public string CodeAttendance { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
         public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email)]
        public string Email { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateUpdate)]
        public DateTime? DateUpdate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string CellPhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        public string EmployeeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        public Guid? EmpTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        public DateTime? DateEndProbation { get; set; }
        public Guid ExportID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string DateHire = "DateHire";
            public const string CodeAttendance = "CodeAttendance";
            public const string Email = "Email";
            public const string CodeEmp = "CodeEmp";
            public const string Gender = "Gender";
            public const string DateUpdate = "DateUpdate";
            public const string CellPhone = "CellPhone";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string DateEndProbation = "DateEndProbation";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
