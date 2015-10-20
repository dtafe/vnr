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
    public class Att_GradeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileID)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_GradeAttendanceName)]
        [MaxLength(150)]
        public string GradeAttendanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Grade_GradeAttendanceID)]
        public Guid? GradeAttendanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Grade_MonthStart)]
        public DateTime? MonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Grade_MonthEnd)]
        public DateTime? MonthEnd { get; set; }
        public bool IsProfileNotGrade { get; set; }
        public string ProfileIDsExclude { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string ProfileName = "ProfileName";          
            public const string GradeAttendanceName = "GradeAttendanceName";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Att_GradeSearchModel
    {      
        public string ProfileName { get; set; }
        public Guid? GradeAttendanceID { get; set; }
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
