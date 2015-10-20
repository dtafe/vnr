using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Laundry.Models
{
    public class Lau_LineModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public string LineLMSName { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_Code)]
        public string Code { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_Notes)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerID)]
        public Guid? MarkerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_MarkerID)]
        public string MarkerName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
        public Guid? LockerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
        public string LockerLMSName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MachineCode)]
        public string MachineCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_Amount)]
        public double? Amount { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
          [DisplayName(ConstantDisplay.HRM_Laundry_Line_DateEffect)]
        public DateTime? DateEffect { get; set; }
      

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LineLMSName = "LineLMSName";
            public const string Note = "Note";
            public const string MarkerID = "MarkerID";
            public const string LockerID = "LockerID";
            public const string MachineCode = "MachineCode";
            public const string LockerLMSName = "LockerLMSName";
            public const string MarkerName = "MarkerName";
            public const string Amount = "Amount";
            public const string DateEffect = "DateEffect";
        }
    }

    public class Lau_LineSearchModel
    {
        public string Code { get; set; }
        public string LineLMSName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Lau_LineMultiModel
    {
        public Guid ID { get; set; }
        public string LineLMSName { get; set; }

    }
}
