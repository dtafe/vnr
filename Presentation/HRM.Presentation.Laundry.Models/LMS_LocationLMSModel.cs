using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Laundry.Models
{
    public class LMS_LocationLMSModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Location_Code)]
        public string Code { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Location_LocationLMSName)]
        public string LocationLMSName { get; set; }
        [MaxLength(2000)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Location_Note)]
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LocationLMSName = "LocationLMSName";
            public const string Note = "Note";
        }
    }
    public class LMS_LocationLMSSearchModel
    {
        public string Code { get; set; }
        public string LocationLMSName { get; set; }
    }
    public class LMS_LocationLMSMultiModel
    {
        public Guid ID { get; set; }
        public string LocationLMSName { get; set; }
    }
}
