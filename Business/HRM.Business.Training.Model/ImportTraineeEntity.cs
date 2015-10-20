using System;

namespace HRM.Business.Training.Models
{
    public class ImportTraineeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }

        public string Column5 { get; set; }

        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }

        public string ClassCode { get; set; }
        public string Status { get; set; }
        public string RequirementCode { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ClassCode = "ClassCode";
            public const string Status = "Status";
            public const string ProfileName = "ProfileName";
            public const string RequirementCode = "RequirementCode";

            
        }
    }
}

