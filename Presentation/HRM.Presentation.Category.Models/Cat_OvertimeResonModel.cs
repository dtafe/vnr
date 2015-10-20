using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_OvertimeResonModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_OvertimeName)]
        public string OvertimeResonName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_OvertimeNameEN)]
        public string OvertimeResonNameEng { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_UserUpdate)]
        public string UserUpdate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeReason_DateUpdate)]
        public DateTime? DateUpdate { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {

            public const string OvertimeResonName = "OvertimeResonName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string OvertimeResonNameEng = "OvertimeResonNameEng";
           
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
          
        }
    }
    public class Cat_OvertimeResonSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public string OvertimeResonName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
