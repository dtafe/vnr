using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_MarkerModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_MarkerName)]       
        public string MarkerName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_Code)]
        public string Code { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_Notes)]       
        public string Note { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string MarkerName = "MarkerName";
            public const string Note = "Note";
        }
    }

    public class Lau_MarkerSearchModel
    {
        public string Code { get; set; }
        public string MarkerName { get; set; }
    }
    public class Lau_MarkerMultiModel
    {
        public Guid ID { get; set; }
        public string MarkerName { get; set; }
      
    }


}
