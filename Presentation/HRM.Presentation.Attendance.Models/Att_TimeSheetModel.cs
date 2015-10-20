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
    public class Att_TimeSheetModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_ProfileID)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_RoleID)]
        public Nullable<System.Guid> RoleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_RoleID)]
        public string RoleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_JobTypeID)]
        public Nullable<System.Guid> JobTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_JobTypeID)]
        public string JobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_NoHour)]
        public Nullable<double> NoHour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_Sector)]
        public Nullable<double> Sector { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public string CodeEmp { get; set; }
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string RoleID = "RoleID";
            public const string RoleName = "RoleName";
            public const string JobTypeID = "JobTypeID";
            public const string JobTypeName = "JobTypeName";
            public const string NoHour = "NoHour";
            public const string Date = "Date";
            public const string Sector = "Sector";
            public const string Note = "Note";
            public const string CodeEmp = "CodeEmp";
        }
    }

    public class Att_TimeSheetSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_RoleID)]
        public Nullable<System.Guid> RoleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_JobTypeID)]
        public Nullable<System.Guid> JobTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TimeSheet_Date)]
        public Nullable<System.DateTime> DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        public string CodeEmp { get; set; }
    }
    
}
