using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatLeaveDayTypeMultiModel 
    {
        public Guid ID { get; set; }
        public string LeaveDayTypeName { get; set; }
        public string Code { get; set; }
        public string CodeStatistic { get; set; }
    }

    public class CatLeaveDayTypeModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_LeaveDayTypeName)]
        public string LeaveDayTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_PaidRate)]
        public double PaidRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_SocialRate)]
        public double SocialRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Penalty)]
        public double Penalty { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_MaxPerTimes)]
        public int? MaxPerTimes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_MaxPerYear)]
        public int? MaxPerYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_MaxPerMonth)]
        public int? MaxPerMonth { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_CodeStatistic)]
        public string CodeStatistic { get; set; }

         [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_OvertimeID)]
        public Guid? OvertimeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_IsWorkDay)]
        public bool IsWorkDay { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_IsAnnualLeave)]
        public bool IsAnnualLeave { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_IsTimeOffInLieu)]
        public bool? IsTimeOffInLieu { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_IsTimeOffMakeUp)]
        public bool? IsTimeOffMakeUp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_IsLoadRelatives)]
        public bool? IsLoadRelatives { get; set; }

     
        public string OrgStructureName { get; set; }

       
        public string OvertimeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Formula)]
        public string Formula { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_InsuranceType)]
        public string InsuranceType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_SalaryType)]
        public string SalaryType { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_UserCreate)]
        //public string UserCreate { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_DateCreate)]
        //public DateTime DateCreate { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_UserUpdate)]
        //public string UserUpdate { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_DateUpdate)]
        //public DateTime DateUpdate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_ReasonName)]
        public Guid? MissInOutReasonID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_TAMScanReasonMiss_ReasonName)]
        public string TAMScanReasonMissName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Notes)]
        public string Notes { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LeaveDayTypeName = "LeaveDayTypeName";
            public const string Code = "Code";
            public const string PaidRate = "PaidRate";
            public const string SocialRate = "SocialRate";
            public const string TAMScanReasonMissName = "TAMScanReasonMissName";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string IsAnnualLeave = "IsAnnualLeave";
            public const string Notes = "Notes";
        }
    }
    public class CatLeaveDayTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        public string LeaveDayTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_LeaveDayTypeName)]
        public string LeaveDayTypeName { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
