using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.ComponentModel;
namespace HRM.Presentation.Category.Models
{
    public class Cat_AbilityTileModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_AbilityTile_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_AbilityTile_RankID)]
        public string RankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_AbilityTile_AbilityTitleVNI)]
        public string AbilityTitleVNI { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_AbilityTile_AbilityTitleEng)]
        public string AbilityTitleEng { get; set; }
        public string Code { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string RankName = "RankName";
            public const string AbilityTitleVNI = "AbilityTitleVNI";
            public const string AbilityTitleEng = "AbilityTitleEng";
            public const string Code = "Code";
        }
    }

    public class Cat_AbilityTileSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_AbilityTile_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }

    public class Cat_AbilityTileMultiModel
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string Code { get; set; }
    }
}
