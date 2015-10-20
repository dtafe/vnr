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
    public class Att_TAMModel : BaseViewModel
    {
        public Guid LoginUserID { get; set; }
        public Guid AsynTaskID { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_ProfileID)]
        public string ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_ProfileID)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_DateFrom)]
        public DateTime DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_DateTo)]
        public DateTime DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_CodeAttendance)]
        [MaxLength(50)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_TimeLog)]
        public DateTime? TimeLog { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_TAM_Status)]
        [MaxLength(150)]
        public string TAMStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        public string StatusTranslate
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Status))
                    return this.Status.TranslateString();
                return string.Empty;
            }
        }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_MachineNo)]
        [MaxLength(50)]
        public string MachineNo { get; set; }
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CardCode = "CardCode";
            public const string TimeLog = "TimeLog";
            public const string Status = "Status";
            public const string StatusTranslate = "StatusTranslate";
            public const string TAMStatus = "TAMStatus";
            public const string MachineNo = "MachineNo";
        }
    }
}
