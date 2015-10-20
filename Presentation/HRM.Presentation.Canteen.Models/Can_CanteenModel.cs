using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_CanteenModel :BaseViewModel
    {
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenCode)]
        public string CanteenCode { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]    
        public string CanteenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_Location)]    
        public Guid? LocationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_LocationName)]    
        public string LocationName { get; set; }
        [MaxLength(2000)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_Notes)]    
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CanteenCode = "CanteenCode";
            public const string CanteenName = "CanteenName";
            public const string LocationID = "LocationID";
            public const string LocationName = "LocationName";
            public const string Note = "Note";           
        }
    }
    public class Can_CanteenSearchModel 
    {      
        public string CanteenCode { get; set; }
        public string CanteenName { get; set; }
        public Guid? LocationID { get; set; } 
    }
    public class Can_CanteenMultiModel
    {
        public Guid ID { get; set; }
        public string CanteenName { get; set; }
    }
}
