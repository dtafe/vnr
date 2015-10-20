using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_MealRecordModel : Service.BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_WorkDate)]
        public DateTime? WorkDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_TimeLog)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? TimeLog { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_TimeLog_Hours)]
        public DateTime? Hour { get { return TimeLog; } }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MachineCode)]
        public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public Guid? CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public Guid? CanteenID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MealAllowanceType)]
        public Guid? MealAllowanceTypeID { get; set; }
        public string MealAllowanceTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_Amount)]
        public decimal? Amount { get; set; }
        public bool? NoWorkDay { get; set; }
        public string LineName { get; set; }
        public string CateringName { get; set; }
        public string CanteenName { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string CodeAttendance { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileID  = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string CardCode = "CardCode";
            public const string CodeAttendance = "CodeAttendance";
            public const string WorkDay = "WorkDay";
            public const string TimeLog = "TimeLog";
            public const string Hour = "Hour";
            public const string MachineCode = "MachineCode";
            public const string LineID  = "LineID";
            public const string CateringID  = "CateringID";
            public const string CanteenID  = "CanteenID";
            public const string MealAllowanceType = "MealAllowanceType";
            public const string MealAllowanceTypeName = "MealAllowanceTypeName";
            public const string Amount = "Amount";
            public const string OrgStructureName = "OrgStructureName";

            public const string ProfileName = "ProfileName";
            public const string LineName = "LineName";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
        }
    }

    public class Can_MealRecordSearchModel
    {
        // Bỏ ProfileIDs vì liên uqna tới store get Mealrecord: nếu có IDs thì param truyền vào sẻ là 5, trong khi store chỉ cần 4
        public Guid? ProfileIDs { get; set; }
      //  public string CardCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MachineCode)]
        //public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public Guid? CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public Guid? CanteenID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MealAllowanceType)]
        public Guid? MealAllowanceType { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Can_MealRecordSummaryModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public string CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public string CanteenID { get; set; }

        public string selectedIDs { get; set; }
        public string ValueFields { get; set; }
        public bool IsExport { get; set; }
    }
}
