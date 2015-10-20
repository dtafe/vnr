using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Category.Models
{
    public class Cat_ReceivePlaceModel :BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ReceivePlace_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ReceivePlace)]
        public string ReceivePlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ReceivePlace_Note)]
        public string Note { get; set; }
        public partial class FieldNames
        {
            public const string Code ="Code";
           
            public const string ReceivePlace = "ReceivePlace";
          
            public const string Note = "Note";
        }
    }
    public class Cat_ReceivePlaceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ReceivePlace)]
        public string ReceivePlace { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
