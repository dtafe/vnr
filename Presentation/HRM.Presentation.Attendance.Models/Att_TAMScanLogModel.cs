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
    public class Att_ProIDAndCutIDModel
    {
        public Guid LoginUserID { get; set; }
        public string ProfileID { get; set; }
        public string CutOffDurationID { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Att_TAMScanLogSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_CardCode)]
        [MaxLength(50)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_EmpCode)]
        public string CodeEmp { get; set; }
      

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_SrcType)]
        //[MaxLength(50)]
        //public string SrcType { get; set; }
        public string Status { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_Status)]
        //[MaxLength(50)]
        //public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_DateTo)]
        public DateTime? DateTo { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_MachineNo)]
        //[MaxLength(50)]
     //   public string MachineNo { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Att_TAMScanLogModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_CardCode)]
        [MaxLength(50)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        [MaxLength(50)]
        public string CodeAttendance { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_DateFrom)]
        public DateTime? TimeLog { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_SrcType)]
        [MaxLength(50)]
        public string SrcType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_Comment)]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_MachineNo)]
        [MaxLength(50)]
        public string MachineNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public Guid CutOffID { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_UserUpdate)]
        //[MaxLength(50)]
        //public string UserUpdate { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_TAMScanLog_DateUpdate)]
        //public DateTime? DateUpdate { get; set; }

        private Guid _id = Guid.Empty;
        public Guid TAMScanLog_ID
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

        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CardCode = "CardCode";
            public const string TimeLog = "TimeLog";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Status = "Status";
            public const string Comment = "Comment";
            public const string MachineNo = "MachineNo";
            public const string CodeEmp = "CodeEmp";
            public const string CodeAttendance = "CodeAttendance";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
}
