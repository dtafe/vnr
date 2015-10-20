using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.ComponentModel;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ChildSickModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ChildSick)]
        public string ChildSickName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Relative)]
        public Nullable<System.Guid> RelativeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Relative)]
        public string RelativeName { get; set; }
        public string ProfileIDs { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string ChildSickName = "ChildSickName";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
        }
    }
    public class InsChildSickMultiModel
    {
        public Guid ID { get; set; }
        public string ChildSickName { get; set; }
    }
}
