using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_LocationModel :BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Location_LocationCode)]
        public string LocationCode { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Location_LocationName)] 
        public string LocationName { get; set; }
        [MaxLength(2000)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_Notes)] 
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LocationCode = "LocationCode";
            public const string LocationName = "LocationName";
            public const string Note = "Note";           
        }
    }
    public class Can_LocationSearchModel 
    {    
        public string LocationCode { get; set; }   
        public string LocationName { get; set; }        
    }
    public class Can_LocationMultiModel
    {
        public Guid ID { get; set; }
        public string LocationName { get; set; }
    }
}
