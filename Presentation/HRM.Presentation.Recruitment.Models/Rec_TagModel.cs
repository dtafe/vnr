using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_TagModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Recruitment_Tag_TagName)]
        public string TagName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_Tag_EntityType)]
        public string EntityType { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TagName = "TagName";
            public const string EntityType = "EntityType";
            public const string Note = "Note";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }
    public class Rec_TagSearchModel
    {
        public string TagName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_TagMultiModel
    {
        public Guid ID { get; set; }
        public string TagName { get; set; }
        public int TotalRow { get; set; }
    }
}
