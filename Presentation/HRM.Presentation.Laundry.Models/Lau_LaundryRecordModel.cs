using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_LaundryRecordModel : BaseViewModel
    {
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public Guid? ProfileID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
         [MaxLength(50)]
         public string CodeEmp { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
         public string ProfileName { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
         public string CodeAttendance { get; set; }

         public Guid? TamScanLogID { get; set; }
         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MachineCode)]
         public string MachineCode { get; set; }

         public Guid? ManchineOfLineID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LineID)]
         public Guid? LineID { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
         public string LineLMSName { get; set; }

         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerID)]
         public Guid? MarkerID { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerName)]
         public string MarkerName { get; set; }

         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerID)]
         public Guid? LockerID { get; set; }
         public Guid? OrgStructureID { get; set; }
         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
         public string LockerLMSName { get; set; }

         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_Amount)]
         public double? Amount { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_Status)]
         public string Status { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_Type)]
         public string Type { get; set; }

         [MaxLength(50)]
         [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
         public string OrgStructureName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_TimeLog)]
         public DateTime? TimeLog { get; set; }

         [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_TimeLog_Hours)]
         public DateTime? Hour { get { return TimeLog; } }

         public DateTime? DateFrom { get; set; }
         public DateTime DateTo { get; set; }

         [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Hours)]
         public string Hours { get; set; }
         [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Count)]
         public DateTime? Count { get; set; }
         //public bool IsExport { get; set; }
         //public string ValueFields { get; set; }
         public partial class FieldNames
         {
             public const string ID = "ID";
             public const string ProfileID = "ProfileID";
             public const string ProfileName = "ProfileName";
             public const string CodeEmp = "CodeEmp";
             public const string CodeAttendance = "CodeAttendance";
             public const string MachineCode = "MachineCode";
             public const string LineID = "LineID";
             public const string LineLMSName = "LineLMSName";
             public const string MarkerID = "MarkerID";
             public const string MarkerName = "MarkerName";
             public const string LockerID = "LockerID";
             public const string LockerLMSName = "LockerLMSName";
             public const string Amount = "Amount";
             public const string Status = "Status";
             public const string Type = "Type";
             public const string Total = "Total";
             public const string TimeLog = "TimeLog";
             public const string Hour = "Hour";
             public const string Hours = "Hours";
             public const string Count = "Count";
             public const string OrgStructureName = "OrgStructureName";
             public const string UserCreate = "UserCreate";
             public const string DateCreate = "DateCreate";
         }
    }
    public class Lau_LaundryRecordSearchNewModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MachineCode)]
        public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerName)]
        public Guid? MarkerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker)]
        public Guid? LockerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Lau_LaundryRecordSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileIDs { get; set; }
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MachineCode)]
        public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public string LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerName)]
        public string MarkerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
        public string LockerID { get; set; }

        public string selectedIDs { get; set; }
        public string ValueFields { get; set; }
        public bool IsExport { get; set; }
    }


    public class Lau_ReportSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileIDs { get; set; }
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public string LineID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_MarkerName)]
        public string MarkerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]
        public string LockerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ShiftId)]
        public string ShiftID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_isViewOverProfile)]
        public bool isViewOverProfile { get; set; }

        public string selectedIDs { get; set; }
        public bool IsExport { get; set; }
        public Guid ExportID { get; set; }
    }

    public class Lau_ReportSummaryLaundryRecordModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public string LineLMSName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_MarkerName)]
        public string MarkerName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]
        public string LockerLMSName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_Price)]
        public int? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_Volume)]
        public int? Volume { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_TotalAmount)]
        public int? TotalAmount { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LineID = "LineID";
            public const string LineLMSName = "LineLMSName";
            public const string MarkerID = "MarkerID";
            public const string MarkerName = "MarkerName";
            public const string LockerID = "LockerID";
            public const string LockerLMSName = "LockerLMSName";
            public const string Price = "Price";
            public const string Volume = "Volume";
            public const string TotalAmount = "TotalAmount";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
        }

    }
}
