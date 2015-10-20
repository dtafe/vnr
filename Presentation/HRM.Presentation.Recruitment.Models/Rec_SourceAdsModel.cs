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
    public class Rec_SourceAdsModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Recruitment_SourceAdsName)]
        public string SourceAdsName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_Notes)]
        public string Notes { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SourceAdsName = "SourceAdsName";
            public const string Notes = "Notes";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }
    public class Rec_SourceAdsSearchModel
    {
        public string SourceAdsName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Rec_SourceAdsMultiModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_SourceAdsName)]
        public string SourceAdsName { get; set; }
    }



}
