using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_KPIBuildingEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PerformanceTemplateID { get; set; }
        public string PerformanceTemplateName { get; set; }
        public Nullable<System.Guid> PerformancePlanID { get; set; }
        public string PerformancePlanName { get; set; }
        public Nullable<System.DateTime> PeriodFromDate { get; set; }
        public Nullable<System.DateTime> PeriodToDate { get; set; }
        public Nullable<System.Guid> TagID { get; set; }
        public string TagName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<double> TotalMark { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string ResultNote { get; set; }
        public Nullable<System.Guid> LevelID { get; set; }
        public string LevelName { get; set; }
        public Nullable<double> Proportion { get; set; }
        public string AttachFile { get; set; }
        public string CurrentStatus { get; set; }
        public string Status { get; set; }
        public string Formula { get; set; }
        public string PerformanceTime { get; set; }
        public List<Guid> EvaluatorIDs { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public string EvaluatorIDList { get; set; }
        public List<Eva_KPIEntity> KPIs { get; set; }
        public string AttachFileLast { get; set; }
        public string PerformanceEvaStatus { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        
    }
}
