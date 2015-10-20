using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_JobTitleModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleName_StringLength)]
        [Required(ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleName_Required)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleCode)]
        public string JobTitleCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleCode)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleNameEn)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleNameEn_StringLength)]
        public string JobTitleNameEn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public List<Guid?> listOrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobDescription)]
        public string JobDescription { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryMax)]
        public double? SalaryMax { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryMin)]
        public double? SalaryMin { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_EmployeeTypeID)]
        public Guid? EmployeeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        public string EmployeeTypeName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string JobTitleName = "JobTitleName";
            public const string Code = "Code";
            public const string JobTitleCode = "JobTitleCode";
            public const string JobTitleNameEn = "JobTitleNameEn";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string CostCentreName = "CostCentreName";
            public const string CostCentreID = "CostCentreID";
            public const string JobDescription = "JobDescription";
            public const string Notes = "Notes";
            public const string SalaryMax = "SalaryMax";
            public const string SalaryMin = "SalaryMin";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string EmployeeTypeID = "EmployeeTypeID";
        }
    }
    public class Cat_JobTitleSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleCode)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Cat_JobTitleMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string JobTitleName { get; set; }
    }
}
