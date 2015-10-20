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
    public class Rec_CandidateComputingLevelModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Computing_SpecialTypeID)]
        public System.Guid SpecialTypeID { get; set; }
        public System.Guid CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_SpecialLevelID)]
        public string ComputingType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_SpecialLevelID)]
        public string ComputingLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Computing_SpecialLevelID)]
        public System.Guid SpecialLevelID { get; set; }
        public partial class FieldNames
        {
            public const string ComputingType = "ComputingType";
            public const string ComputingLevel = "ComputingLevel";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
 
}
