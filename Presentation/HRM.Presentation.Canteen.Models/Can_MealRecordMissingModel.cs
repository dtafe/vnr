using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
//using HRM.Presentation.Category.Models;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_MealRecordMissingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_ProfileId)]
        public List<Guid> ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_ProfileName)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_EmpCode)]
        public string EmpCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_WorkDate)]
        public DateTime? WorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_Type)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_TamScanReasonMissName)]
        [UIHint("ComboBox")]
        public string TamScanReasonMissName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_TamScanReasonMissName)]
        public Nullable<Guid> TamScanReasonMissID { get; set; }

        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public Guid? MealAllowanceTypeSettingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public string MealAllowanceTypeSettingName { get; set; }

          [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_Amount)]
          public decimal? Amount { get; set; }

          [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_IsFullPay)]
          public bool? IsFullPay { get; set; }
          public string selectedIDs { get; set; }
          public string ValueFields { get; set; }
          public bool IsExport { get; set; }

          public string TAMScanReasonMissingCode { get; set; }
          public partial class FieldNames
          {
              public const string ID = "ID";
              public const string ProfileIDs = "ProfileIDs";
              public const string ProfileID = "ProfileID";
              public const string ProfileName = "ProfileName";
              public const string EmpCode = "EmpCode";
              public const string OrgStructureName = "OrgStructureName";
              public const string OrgStructureID = "OrgStructureID";
              public const string WorkDate = "WorkDate";
              public const string Type = "Type";
              public const string Status = "Status";
              public const string TamScanReasonMissName = "TamScanReasonMissName";
              public const string TamScanReasonMissID = "TamScanReasonMissID";
              public const string Cat_TAMScanReasonMissModel = "Cat_TAMScanReasonMissModel";
              public const string Note = "Note";
              public const string MealAllowanceTypeSettingID = "MealAllowanceTypeSettingID";
              public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
              public const string Amount = "Amount";
              public const string IsFullPay = "IsFullPay";
          }
    }
    public class Can_MealRecordMissingSearchModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ProfileID)]
        public string ProfileIDSearch { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_MissInOutReason)]
        public List<Guid> TamScanReasonMissID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public List<Guid?> MealAllowanceTypeSettingID { get; set; }
        public string selectedIDs { get; set; }
        public string ValueFields { get; set; }
        public bool IsExport { get; set; }
        public bool ComputeOrAddEdit { get; set; }
        public partial class FieldNames
        {
            public const string OrgStructureID = "OrgStructureID";
            public const string ProfileIDSearch = "ProfileIDSearch";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string TamScanReasonMissID = "TamScanReasonMissID";
            public const string Status = "Status";
            public const string MealAllowanceTypeSettingID = "MealAllowanceTypeSettingID";
        }
    }

    public class Can_RecordMissingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ProfileID)]
        public Guid? ProfileIDSearch { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_MissInOutReason)]
        public Guid? TamScanReasonMissID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_Status)]
        public string Status { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string ProfileID = "ProfileID";
            public const string OrgStructureID = "OrgStructureID";
            public const string TamScanReasonMissID = "TamScanReasonMissID";
            public const string Status = "Status";
        }
    }
}
