using System;


namespace HRM.Business.Training.Models
{
    public class Tra_TrainerMailReminderEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string UserInfoIDs { get; set; }
    }
}

