using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_GradeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_ProfileID)]
        public string ProfileIDs { get; set; }
        public string ProfileIDsExclude { get; set; }
        public System.Guid ProfileID { get; set; }
        public System.Guid ProfileIDExclude { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_GradePayrollID)]
        public Guid? GradePayrollID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_GradeCfgName)]
        public string GradeCfgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_MonthStart)]
        public DateTime? MonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_MonthEnd)]
        public DateTime? MonthEnd { get; set; }
        public string CodeEmps { get; set; }
        public bool IsProfileNotGrade { get; set; }
        public bool IsCreateByProfile { get; set; }
        public Nullable<int> SalaryDayClose { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string GradeCfgName = "GradeCfgName";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public const string GroupMonthEndCost = "MonthEnd";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string GradeID = "GradeID";
            public const string OrgStructureName = "OrgStructureName";
        }
    }
 

    public class Sal_GradeSearchModel
    {
        public string ProfileName { get; set; }
        public Guid? ProfileID { get; set; }
        public Guid? GradeID { get; set; }
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
