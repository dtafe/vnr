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
    public class Rec_CandidateBusinessModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateID)]
         public System.Guid CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateID)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateBusiness_BusinessType)]
        public string BusinessType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_CandidateBusiness_BusinessArea)]
        public string BusinessArea { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Reward_Description)]
        public string Description { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string BusinessType = "BusinessType";
            public const string BusinessArea = "BusinessArea";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Rec_CandidateBusinessByCandidateSearchModel
    {
        public Guid CandidateID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
