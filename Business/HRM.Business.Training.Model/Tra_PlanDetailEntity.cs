using System;


namespace HRM.Business.Training.Models
{
    public class Tra_PlanDetailEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string PlanName { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        
        public string OrgStructureName { get; set; }
        public Nullable<int> Quantity { get; set; }

        public Nullable<int> SumQuantity { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public string CourseName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}

