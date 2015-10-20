using System;


namespace HRM.Business.Training.Models
{
    public class Tra_ReportTraineeJoinCourseEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string CourseName { get; set; }
        public string TopicName { get; set; }
        public string CodeEmp { get; set; }
        public string TraineeName { get; set; }
        public string IDNo { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string OrgStructureName { get; set; }
        public string ClassName { get; set; }
        public string Note { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public double? Seniority { get; set; }
        public Guid? RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string TraineeName = "TraineeName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CourseName = "CourseName";
            public const string IDNo = "IDNo";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string ClassName = "ClassName";
            public const string Note = "Note";
            public const string Seniority = "Seniority";


            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

            public const string RequirementTrainName = "RequirementTrainName";
            
   
        }
    }

   

}

