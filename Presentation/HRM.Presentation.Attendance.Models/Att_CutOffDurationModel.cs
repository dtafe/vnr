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
    public class Att_CutOffDurationMultiModel 
    {
        public Guid ID { get; set; }
        public string CutOffDurationName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int TotalRow { get; set; }
    }
    public class Att_CutOffDurationModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_OverTimeDuration)]
        public Nullable<System.DateTime> OvertimeStart { get; set; }
        public Nullable<System.DateTime> OvertimeEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_LeaveDayDuration)]
        public Nullable<System.DateTime> LeavedayStart { get; set; }
        public Nullable<System.DateTime> LeavedayEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_Duration)]
        [MaxLength(150)]
        public string CutOffDurationName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_MonthYear)]
        public DateTime MonthYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_DateStart)]
        public DateTime DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_DateEnd)]
        public DateTime DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_DurationType)]
        [MaxLength(50)]
        public string DurationType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_IsInsuranceSocial)]
        public Nullable<bool> IsInsuranceSocial { get; set; }
       

        public partial class FieldNames
        {
            public const string CutOffDurationName = "CutOffDurationName";
            public const string MonthYear = "MonthYear";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string DurationType = "DurationType";
            public const string IsInsuranceSocial = "IsInsuranceSocial";
        }
    }

    public class Att_CutOffDurationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_CutOffDurationName)]
        [MaxLength(50)]
        public string CutOffDurationName { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string CutOffDurationName = "CutOffDurationName";
        }
    }

}
