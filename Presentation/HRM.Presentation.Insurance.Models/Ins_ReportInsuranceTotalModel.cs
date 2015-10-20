using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportInsuranceTotalModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_Stt)]
        public int? Stt { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_Name)]
        public string Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_TotalEmp)]
        public int TotalEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_BHXH)]
        public double? BHXH { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_BHYT)]
        public double? BHYT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_BHTN)]
        public double? BHTN { get; set; }
          [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_TotalAll)]
        public double? TotalAll { get; set; }
                
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string Name = "Name";
            public const string TotalEmp = "TotalEmp";
            public const string BHXH = "BHXH";
            public const string BHYT = "BHYT ";
            public const string BHTN = "BHTN";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string TotalAll = "TotalAll";
        }
    }
    public class Ins_ReportInsuranceTotalSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_INS_ReportInsuranceTotal_MonthYear)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public Guid? SocialInsPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public List<Guid> WorkPlaceIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public List<Guid> SocialInsPlaceIDs { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
