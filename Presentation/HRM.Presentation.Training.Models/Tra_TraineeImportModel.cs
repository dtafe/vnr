using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_TraineeImportModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string ClassCode { get; set; }
        public string RequirementCode { get; set; }

        public string Status { get; set; }
        public partial class FieldNames
        {
            public const string ClassCode = "ClassCode";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Status = "Status";
            public const string RequirementCode = "RequirementCode";

        }

    }

    public class Tra_TraineeChangeClassImportModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ClassOldCode { get; set; }
        public string ClassNewCode { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ClassOldCode = "ClassOldCode";
            public const string ClassNewCode = "ClassNewCode";

        }
    }


}
