using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Kai_KaizenDataModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_KaizenName)]
        public string KaizenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public Nullable<System.Guid> CategoryID { get; set; }
        public string CategoryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_MarkIdea)]
        public Nullable<double> MarkIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_AmountIdea)]
        public Nullable<double> AmountIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_MarkPerform)]
        public Nullable<double> MarkPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_AmountPerform)]
        public Nullable<double> AmountPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_SumMark)]
        public Nullable<double> SumMark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_SumAmount)]
        public Nullable<double> SumAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_Accumulate)]
        public Nullable<double> Accumulate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public string CategoryList { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public string CategoryCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_AmountTransfered)]
        public Nullable<double> AmountTransfered { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_DateTransferPayment)]
        public Nullable<System.DateTime> DateTransferPayment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_Status)]
        public string Status { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string KaiOrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.Kai_Kaizen_KaizenFata_Paymentout)]
        public Nullable<bool> IsPaymentOut { get; set; }
         [DisplayName(ConstantDisplay.HRM_Kai_Category_AccumulateRevice)]
        public double? AccumulateRevice { get; set; }
         public bool? IsNotRankMask { get; set; }

        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string CategoryList = "CategoryList";
            public const string CategoryCode = "CategoryCode";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string KaiOrgStructureName = "KaiOrgStructureName";
            public const string KaizenName = "KaizenName";
            public const string Month = "Month";
            public const string DateTransferPayment = "DateTransferPayment";
            public const string CategoryID = "CategoryID";
            public const string CategoryName = "CategoryName";
            public const string MarkIdea = "MarkIdea";
            public const string AmountIdea = "AmountIdea";
            public const string MarkPerform = "MarkPerform";
            public const string AmountPerform = "AmountPerform";
            public const string SumMark = "SumMark";
            public const string SumAmount = "SumAmount";
            public const string Accumulate = "Accumulate";
            public const string ProfileID = "ProfileID";
            public const string AmountTransfered = "AmountTransfered";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string IsPaymentOut = "IsPaymentOut";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Kai_KaizenDataSearchModel
    {
      
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_KaizenName)]
        public string KaizenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_Category_CategoryName)]
        public Guid? CategoryID { get; set; }
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthTo)]
        public DateTime? DateTo { get; set; }
      
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Kai_KaizenDataMultiModel
    {
        public Guid ID { get; set; }
        public string ProfileID { get; set; }
        public string KaizenName { get; set; }
        public DateTime Month { get; set; }
        public Nullable<double> MarkIdea { get; set; }
        public Nullable<double> AmountIdea { get; set; }
        public Nullable<double> MarkPerform { get; set; }
        public Nullable<double> AmountPerform { get; set; }
        public Nullable<double> SumMark { get; set; }
        public Nullable<double> SumAmount { get; set; }
        public Nullable<double> Accumulate { get; set; }
        public int TotalRow { get; set; }
    }
}
