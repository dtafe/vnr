using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_LockerModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]       
        public string LockerLMSName { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_Code)]
        public string Code { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_Notes)]       
        public string Note { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string LockerLMSName = "LockerLMSName";
            public const string Note = "Note";
        }
    }

    public class Lau_LockerSearchModel
    {
        public string Code { get; set; }
        public string LockerLMSName { get; set; }
    }
    public class Lau_LockerMultiModel
    {
        public Guid ID { get; set; }
        public string LockerLMSName { get; set; }
      
    }


}
