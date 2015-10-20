using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_TamScanLogModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Code)]
        public string TamScanLogCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_TimeLog)]
        public DateTime? TimeLog { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Type)]
        public string Type { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_SrcType)]
        public string SrcType { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Status)]
        public string Status { get; set; }

        [MaxLength(500)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Comment)]
        public string Comment { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_ManchineCode)]
        public string TamScanLogMachineCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public List<int?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_ProfileID)]
        public List<Guid> ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_TimeLog)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_DateTo)]
        public DateTime? DateTo { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_TAM_Status)]
        [MaxLength(150)]
        public string TAMStatus { get; set; }
        public int UserID { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TamScanLogCode = "TamScanLogCode";
            public const string TimeLog = "TimeLog";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Status = "Status";
            public const string Comment = "Comment";
            public const string TamScanLogMachineCode = "TamScanLogMachineCode";
            public const string OrgStructureID = "OrgStructureID";
            public const string ProfileIDs = "ProfileIDs";
            public const string ProfileName = "ProfileName";
            public const string IsExport = "IsExport";
            public const string ValueFields = "ValueFields";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string TAMStatus = "TAMStatus";
        }
    }
}
