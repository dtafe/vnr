using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_RptExceptionDataModel : BaseViewModel
    {
        public Guid ID { get; set; }
        public Guid InOutID { get; set; }
        public Guid ShiftID { get; set; }
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public String CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public String ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Date)]
        public DateTime? Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }
        public string EntityType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentName1)]
        public string Department { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionName1)]
        public string Section { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DataType)]
        public string DataType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ExceptionOvertime_Title)]
        public string Exception { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
       
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string InOutID = "InOutID";
            public const string ShiftID = "ShiftID";
            public const string ProfileID = "ProfileID";
            public const string EntityType = "EntityType";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Position = "Position";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string Date = "Date";
            public const string DataType = "DataType";
            public const string Exception = "Exception";
            public const string Description = "Description";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }

    public class Att_RptExceptionDataSearchModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public String OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_NoScan)]
        public bool NoScan { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DifferenceMoreRoster)]
        public bool DifferenceMoreRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Difference)]
        public double Difference { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_LessHours)]
        public double LessHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_MissInOut)]
        public bool MissInOut { get; set; }
        
        public bool IsExport { get; set; }
        public String ValueFields { get; set; }
        public Guid ExportId { get; set; }
        public string UserExport { get; set; }

    }
}
