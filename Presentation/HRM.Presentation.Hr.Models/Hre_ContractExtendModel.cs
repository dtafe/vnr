using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ContractExtendModel : BaseViewModel
    {
        public Nullable<System.Guid> ContractID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_ContractExtend_AnnexCode)]
        public string AnnexCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_ContractExtend_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_ContractExtend_DateEnd)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        public string ContractNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStartContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEndContract { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string AnnexCode = "AnnexCode";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string ContractNo = "ContractNo";
            public const string DateStartContract = "DateStartContract";
            public const string DateEndContract = "DateEndContract";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }

    public class Hre_ContractExtendSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Accident_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
