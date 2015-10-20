using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Insurance.Models
{
    /// <summary>Danh Mục Trạng Thái D02</summary>
    public class Ins_TypeD02Model : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_TypeCode)]
        public string TypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_TypeName)]
        public string TypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_StatusCode)]
        public string StatusCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_StatusName)]
        public string StatusName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_TypeD02_Comment)]
        public string CommentCode { get; set; }

        public partial class FieldNames
        {
            public const string TypeCode = "TypeCode";
            public const string TypeName = "TypeName";
            public const string StatusCode = "StatusCode";
            public const string StatusName = "StatusName";
            public const string Comment = "Comment";
            public const string Note = "Note";
        }
    }
    public class Ins_TypeD02ModelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string TypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string TypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Ins_TypeD02MultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TotalRow { get; set; }
        public string StatusSyn { get; set; }
    }

}
