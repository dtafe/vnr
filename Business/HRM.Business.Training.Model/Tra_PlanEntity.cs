using System;


namespace HRM.Business.Training.Models
{
    public class Tra_PlanEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string PlanName { get; set; }
        public Nullable<System.Guid> PlanTypeID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Audience { get; set; }
        public string AchieveTarget { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<bool> IsApprove { get; set; }
        public string Code { get; set; }
        public Nullable<int> Type { get; set; }
   
        public Nullable<bool> Outside { get; set; }
        public int? SumQuantity { get; set; }
        public string Status { get; set; }
        public Nullable<int> TotalClass { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public string CourseName { get; set; }
    }

    public class Tra_PlanMultiEntity 
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string PlanName { get; set; }
    }
}

