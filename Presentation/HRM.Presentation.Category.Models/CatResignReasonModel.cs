using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatResignReasonModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public string ResignReasonName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonNameEN)]
        public string ResignReasonNameEn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_NotesEN)]
        public string NotesEn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_UserUpdate)]
        public string UserUpdate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_DateUpdate)]
        public DateTime? DateUpdate { get; set; }

        public partial class FieldNames
        {
           
            public const string ResignReasonName = "ResignReasonName";
            public const string Code = "Code";
            public const string Notes = "Notes";
            public const string ResignReasonNameEn = "ResignReasonNameEn";
            public const string NotesEn = "NotesEn";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
          
        }
    }
    public class CatResignReasonSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public string ResignReasonName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
