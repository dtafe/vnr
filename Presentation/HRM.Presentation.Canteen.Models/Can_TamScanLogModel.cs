using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_TamScanLogModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_CardCode)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_TimeLog)]
        public DateTime? TimeLog { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_Type)]
        public string Type { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_SrcType)]
        public string SrcType { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_Status)]
        public string Status { get; set; }

        [MaxLength(500)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_Comment)]
        public string Comment { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_MachineCode)]
        public string MachineCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Reward_ProfileName)]
        public List<Guid> ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TAM_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_TimeLog)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_TamScanLog_DateTo)]
        public DateTime? DateTo { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_TAM_Status)]
        [MaxLength(150)]
        public string TAMStatus { get; set; }
        public Guid UserID { get; set; }
        public Guid AsynTaskID { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CardCode = "CardCode";
            public const string TimeLog = "TimeLog";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Status = "Status";
            public const string Comment = "Comment";
            public const string MachineCode = "MachineCode";
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
