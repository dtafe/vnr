using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_MachineOfLineModel : BaseViewModel
    {


        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_LineID)]       
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_DateFrom)]   
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_DateTo)]   
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_LineName)]   
        public string LineName { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_Laundry_MachineOfLine_Note)]       
        public string Notes { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Notes = "Notes";
        }
    }

    public class Lau_MachineOfLineSearchModel
    {
        public string Code { get; set; }
        public Guid? LineID { get; set; }
    }

    public class Lau_MachineOfLineMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }

    }

}
