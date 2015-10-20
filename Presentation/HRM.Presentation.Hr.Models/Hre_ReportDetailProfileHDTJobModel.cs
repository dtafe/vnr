using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportDetailProfileHDTJobModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_DateApply)]
        public DateTime? DateSearch { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string HDTJobTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public string HDTJobTypeName { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobTypeCodeOld)]
        public string HDTJobTypeCodeOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobGroupNameOld)]
        public string HDTJobTypeNameOld { get; set; }
        public double? PriceOld { get; set; }

        public partial class FieldNames
        {
            public const string HDTJobTypeCodeOld = "HDTJobTypeCodeOld";
            public const string HDTJobTypeNameOld = "HDTJobTypeNameOld";
            public const string PriceOld = "PriceOld";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Price = "Price";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
      
    }
}
