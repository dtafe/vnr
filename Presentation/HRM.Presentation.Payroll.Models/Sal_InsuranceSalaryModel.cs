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
    public class Sal_InsuranceSalaryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_ProfileID)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_ProfileID)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_CurrencyID)]
        public Guid? CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_CurrencyID)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_DateEffect)]
        public DateTime? DateEffect { get; set; }
        public string CodeEmps { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_IsSocialIns)]
        public bool? IsSocialIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_IsMedicalIns)]
        public bool? IsMedicalIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_IsUnimploymentIns)]
        public bool? IsUnimploymentIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_InsuranceAmount)]
        public Nullable<double> InsuranceAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_InsuranceSalry_DecisionNo)]
        public string DecisionNo { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public string ProfileIDsExclude { get; set; }
        public bool IsCreateByProfile { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string DateEffect = "DateEffect";
            public const string IsSocialIns = "IsSocialIns";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string IsMedicalIns = "IsMedicalIns";
            public const string IsUnimploymentIns = "IsUnimploymentIns";
            public const string OrgStructureName = "OrgStructureName";
            public const string SectionName = "SectionName";
            public const string DepartmentName = "DepartmentName";
            public const string CodeEmp = "CodeEmp";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string Note = "Note";
            public const string DecisionNo = "DecisionNo";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }


    public class Sal_InsuranceSalarySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHireStart { get; set; }
        public DateTime? DateHireEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfEffect)]
        public DateTime? DateEffectStart { get; set; }
        public DateTime? DateEffectEnd { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
