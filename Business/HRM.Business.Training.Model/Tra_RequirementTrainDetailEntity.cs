using System;


namespace HRM.Business.Training.Models
{
    public class Tra_RequirementTrainDetailEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string PositionName { get; set; }
        public string CodeEmp { get; set; }
        public string JobTitleName { get; set; }
        public string IDNo { get; set; }
        public string OrgStructureName { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> ClassID { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public string CourseName { get; set; }
        public Nullable<System.Guid> TopicID { get; set; }
        public string TopicName { get; set; }
        public string TopicXML { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }

        public double? Seniority { get; set; }

        public DateTime? YearAnalyze { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
    }
}

