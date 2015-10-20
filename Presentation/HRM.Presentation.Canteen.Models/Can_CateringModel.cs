using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using System;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_CateringModel :BaseViewModel
    {
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringCode)]
        public string CateringCode { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)] 
        public string CateringName { get; set; }
        [MaxLength(2000)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_Notes)] 
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CateringCode = "CateringCode";
            public const string CateringName = "CateringName";
            public const string Note = "Note";            
        }
    }
    public class Can_CateringSearchModel 
    {     
        public string CateringCode { get; set; }
        public string CateringName { get; set; }
    }
    public class Can_CateringMultiModel
    {
        public Guid ID { get; set; }
        public string CateringName { get; set; }
    }
}
