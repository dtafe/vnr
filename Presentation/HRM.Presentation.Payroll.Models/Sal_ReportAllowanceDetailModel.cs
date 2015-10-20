using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ReportAllowanceDetailSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSyn { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecord_MealAllowanceType)]
        public string UnusualEDTypeID { get; set; }
        public Guid UserID { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }
    public class Sal_ReportAllowanceDetailModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string WorkPlace { get; set; }
        public DateTime? HireDate { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string OrgStructureName { get; set; }
        public Double Amount_USD { get; set; }
        public Double Amount_VND { get; set; }
        public Double Amount_VND_ROUND { get; set; }
        public Double Total_Insurance { get; set; }
        public Double PIT { get; set; }
        public string Notes { get; set; }
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]
        public string PayrollGroup { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }

        public int TotalRow { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string HireDate = "HireDate";
            public const string OrgStructureName = "OrgStructureName";
            public const string BankName = "BankName";
            public const string BankAccountNo = "BankAccountNo";
            public const string BankCode = "BankCode";
            public const string Amount_USD = "Amount_USD";
            public const string Amount_VND = "Amount_VND";
            public const string Amount_VND_ROUND = "Amount_VND_ROUND";
            public const string Total_Insurance = "Total_Insurance";
            public const string PIT = "PIT";
            public const string Notes = "Notes";
            public const string MonthYear = "MonthYear";
            public const string PayrollGroup = "PayrollGroup";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
        }
    }

    

}
