using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_MachineOfLineModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_MachineCode)]
        public string MachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_LineCode)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_LineName)]
        public string LineName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_DateTo)]
        public DateTime? DateTo { get; set; }
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_Notes)]    
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LineID = "LineID";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Note = "Note";           
        }
    }
    public class Can_MachineOfLineSearchModel 
    {

        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_MachineCode)]
        public string MachineOfLineMachineCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_LineCode)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MachineOfLine_DateTo)]
        public DateTime? DateTo { get; set; }
    }

    public class Can_MachineOfLineMultiModel
    {
        public Guid? ID { get; set; }
        public string MachineOfLineMachineCode { get; set; }
    }
   
}
