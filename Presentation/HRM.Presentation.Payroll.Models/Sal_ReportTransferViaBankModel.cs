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
    public class Sal_ReportTransferViaBankSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Element_ElementName)]
        public string ElementType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]
        public Guid? PayrollGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string StatusEmployees { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankName)]
        public Nullable<System.Guid> BankID { get; set; }
                [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_GroupBank)]
        public string GroupBank { get; set; }


        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }
    public class Sal_ReportTransferViaBankModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string NameEnglish { get; set; }
        public DateTime? HireDate { get; set; }
        public string IDNo { get; set; }
        public string OrgStructureName { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankCode { get; set; }
        public int Amount_USD { get; set; }
        public int Amount_VND_INT { get; set; }
        public Double Amount_VND { get; set; }
        public int Amount_VND_ROUND { get; set; }
        public Double Total_Insurance { get; set; }
        public Double PIT { get; set; }
        public string Notes { get; set; }
        public DateTime? MonthYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_PayrollGroupID)]
        public string PayrollGroup { get; set; }

        public int TotalRow { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string EnglishName = "EnglishName";
            public const string HireDate = "HireDate";
            public const string IDNo = "IDNo";
            public const string OrgStructureName = "OrgStructureName";
            public const string BankName = "BankName";
            public const string BankAccountNo = "BankAccountNo";
            public const string BankCode = "BankCode";
            public const string Amount_USD = "Amount_USD";
            public const string Amount_VND_INT = "Amount_VND_INT";
            public const string Amount_VND = "Amount_VND";
            public const string Amount_VND_ROUND = "Amount_VND_ROUND";
            public const string Total_Insurance = "Total_Insurance";
            public const string PIT = "PIT";
            public const string Notes = "Notes";
            public const string MonthYear = "MonthYear";
            public const string PayrollGroup = "PayrollGroup";
        }
    }

    

}
