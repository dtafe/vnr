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
    public class Sal_ReportGroupPayrollCostCentreModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]

        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_ProfileID)]
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
 
        public string GroupCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
   
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]

        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DeptPath)]
        public string DepartmentName { get; set; }

        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ElementType)]
        public string ElementType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CodeAlocal_ElementType)]
        public string ElementName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_CostCentreName)]
        public string CostCentreName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_Rate)]
        public Nullable<double> Rate { get; set; }
         [DisplayName(ConstantDisplay.HRM_Canteen_ReportCardNotStand_SumAmount)]
        public double Rate_Money { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_Amount)]
         public double Amount { get; set; }
        

        [DisplayName(ConstantDisplay.HRM_Payroll_Costcentre_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobtitleName { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }


        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }



        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

            public const string DepartmentCode = "DepartmentCode";
            public const string ElementType = "ElementType";
            public const string ElementName = "ElementName";
            public const string CostCentreName = "CostCentreName";
            public const string Rate = "Rate";
            public const string Rate_Money = "Rate_Money";
            public const string DateStart = "DateStart";
            public const string JobtitleName = "JobtitleName";
            public const string Amount = "Amount";
            public const string ProfileID = "ProfileID";
        }
    }
}
