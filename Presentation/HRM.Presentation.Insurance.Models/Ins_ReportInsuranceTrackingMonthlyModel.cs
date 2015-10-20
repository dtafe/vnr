using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportInsuranceTrackingMonthlyModel : BaseViewModel
    {
    }
    public class Ins_ReportInsuranceTrackingMonthlySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeaveDetail_MonthStart)]
        public DateTime? MonthYearFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public Guid? WorkPlaceID{ get; set; }

        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public List<Guid> WorkPlaceIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Ins_WorkingPlaceName)]
        public string StrWorkPlaceIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]        
        public Guid? SocialInsPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public List<Guid> SocialInsPlaceIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public string StrSocialInsPlaceIDs { get; set; }

        public DateTime? MonthYearTo { get; set; }
        /// <summary>Nhan Vien Co Nghi Viec khong</summary>
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_Terminate)]        
        public bool? IsProfileQuit { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid? ExportId { get; set; }
    }

}
