using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_LineModel :BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)] 
        public Guid? CanteenID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)] 
        public string CanteenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public Guid? CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)] 
        public string CateringName { get; set; }      
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineCode)]
        public string LineCode { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)] 
        public string LineName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_MachineCode)]
        public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_Amount)] 
        public double? Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineHDTJOB)]
        public string HDTJ { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_Standard)] 
        public bool? Standard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_IsHDTJOB)]
        public bool? IsHDTJOB { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_DateEffect)]
        public DateTime? DateEffect { get; set; }

        [MaxLength(2000)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_Notes)] 
        public string Note { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string MachineCode = "MachineCode";
            public const string CanteenName = "CanteenName";
            public const string CateringName = "CateringName";
            public const string LineCode = "LineCode";
            public const string LineName = "LineName";
            public const string Amount = "Amount";
            public const string DateEffect = "DateEffect";
            public const string IsHDTJOB = "IsHDTJOB";
            public const string HDTJ = "HDTJ";
            public const string Standard = "Standard";
            public const string Note = "Note";     
        }
    }
    public class Can_LineSearchModel 
    {
        public Guid? CanteenID { get; set; }
        public Guid? CateringID { get; set; }       
        public string LineCode { get; set; }       
        public string LineName { get; set; }
    }
    public class Can_LineMultiModel
    {
        public Guid ID { get; set; }
        public string LineName { get; set; }
    }
}
