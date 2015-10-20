using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_TrainerMailReminderModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Tra_TrainerMailReminder_OrgStructureID)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TrainerMailReminder_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TrainerMailReminder_UserInfoIDs)]
        public string UserInfoIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Tra_TrainerMailReminderSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_TrainerMailReminder_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
