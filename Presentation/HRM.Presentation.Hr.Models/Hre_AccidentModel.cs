using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_AccidentModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Accident_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_ProfileID)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_Date)]
        public DateTime Date { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_Place)]
        [MaxLength(1000)]
        public string Place { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_AccidentTypeID)]
        public Guid? AccidentTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Accident_AccidentTypeName)]
        public string AccidentTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_FirstMoney)]
        public double? FirstMoney { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_RealMoney)]
        public double? RealMoney { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_CompanyPay)]
        public double? CompanyPay { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_Description)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Accident_Note)]
        [MaxLength(1000)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        //[DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        //public List<Guid?> OrgStructureID { get; set; }
        public string Notes { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Accident_ID
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
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string Date = "Date";
            public const string Place = "Place";
            public const string AccidentTypeID = "AccidentTypeID";
            public const string AccidentTypeName = "AccidentTypeName";
            public const string FirstMoney = "FirstMoney";
            public const string RealMoney = "RealMoney";
            public const string CompanyPay = "CompanyPay";
            public const string Description = "Description";
            public const string Note = "Note";
            public const string CodeEmp = "CodeEmp";
            public const string PositionName = "PositionName";
            public const string PositionID = "PositionID";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string Accident_ID = "Accident_ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Hre_AccidentSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Accident_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }
    public class Hre_ReportAccidentSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Acciden_DateFrom)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Accident_AccidentTypeID)]
        public Guid? AccidentTypeID { get; set; }
        
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string OrgStructureID = "OrgStructureID";
            public const string AccidentTypeID = "AccidentTypeID";
        }
        
    }
}
