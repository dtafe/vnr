using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_BonusSalaryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_Search_Month)]
        public DateTime? Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_BonusSalary_Bonus)]
        public double? Bonus { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public Guid? SalesTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_BonusSalary_Quarter)]
        public string Quarter { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public DateTime? DateMonth { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string Type = "Type";
            public const string OrgStructureName = "OrgStructureName";
            public const string Month = "Month";
            public const string Quarter = "Quarter";
            public const string Bonus = "Bonus";
            public const string SalesTypeName = "SalesTypeName";
            public const string CodeEmp = "CodeEmp";
            public const string SalesTypeID = "SalesTypeID";
            public const string OrgStructureID = "OrgStructureID";
            public const string JobTitleName = "JobTitleName";

        }
    }

    public class Eva_BonusSalarySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Common_Search_Month)]
        public DateTime? Month { get; set; }
        public string Type { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
