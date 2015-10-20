using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_ReportEvaluationResultEntity : HRMBaseModel
    {
        public string CostCentreName { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string Department { get; set; }
        public string OrgStructureName { get; set; }
        public string SupervisorID { get; set; }
        public string PositionName { get; set; }
        public string JobTitle { get; set; }
        public DateTime? DateHire { get; set; }
        public int YearOfService { get; set; }
        public double? MarkLevel1 { get; set; }
        public double? MarkLevel2 { get; set; }
        public string EvaluationLevel2 { get; set; }
        public string Level { get; set; }
        public double? MarkLevel3 { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CostCentreName = "CostCentreName";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Department = "Department";
            public const string OrgStructureName = "OrgStructureName";
            public const string SupervisorID = "SupervisorID";
            public const string JobTitle = "JobTitle";
            public const string DateHire = "DateHire";
            public const string YearOfService = "YearOfService";
            public const string MarkLevel1 = "MarkLevel1";
            public const string MarkLevel2 = "MarkLevel2";
            public const string EvaluationLevel2 = "EvaluationLevel2";
            public const string Level = "Level";
            public const string MarkLevel3 = "MarkLevel3";
            public const string PositionName = "PositionName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
        
    }
   
}
