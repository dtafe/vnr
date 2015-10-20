using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ReqDocumentModel : BaseViewModel
    {
        public string TypeOfDocument { get; set; }
        public Nullable<System.Guid> ResignReasonID { get; set; }
        public string ResignReasonName { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> OrgID { get; set; }
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ReqDocument_ReqDocumentName)]
        public string ReqDocumentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ReqDocument_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ReqDocument_Code)]
        public string Code { get; set; } 
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ReqDocumentName = "ReqDocumentName";
            public const string Note = "Note";
            public const string Code = "Code";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
            public const string TypeOfDocument = "TypeOfDocument";
            public const string ResignReasonName = "ResignReasonName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string OrgName = "OrgName";
        }
    }

    public class Cat_ReqDocumentSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ReqDocument_ReqDocumentName)]
        public string ReqDocumentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ReqDocument_Code)]
        public string Code { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_ReqDocumentMuliModel
    {
        public Guid ID { get; set; }
        public string ReqDocumentName { get; set; }
    }
}
