using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_PayrollGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_PayrollGroupName)]
        public string PayrollGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_SalaryDateStart)]
        public int? SalaryDateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_SalaryMonthStart)]
        public int? SalaryMonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_ReportMappingID)]
        public Guid? ReportMappingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_ReportMappingID1)]
        public Guid? ReportMappingID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_OrderNumber)]
        public int? OrderNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_ReportMappingName)]
        public string ReportMappingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_ReportMappingName1)]
        public string ReportMappingName1 { get; set; }

        public partial class FieldNames
        {
            public const string PayrollGroupName = "PayrollGroupName";
            public const string Code = "Code";
            public const string SalaryDateStart = "SalaryDateStart";
            public const string SalaryMonthStart = "SalaryMonthStart";
            public const string Description = "Description";
            public const string ReportMappingID = "ReportMappingID";
            public const string ReportMappingName = "ReportMappingName";
            public const string ReportMappingID1 = "ReportMappingID1";
            public const string ReportMappingName1 = "ReportMappingName1";
            public const string OrderNumber = "OrderNumber";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Cat_PayrollGroupSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_PayrollGroupName)]
        public string PayrollGroupName { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_PayrollGroupMultiModel 
    {
        public Guid ID { get; set; }
        public string PayrollGroupName { get; set; }
        public int? OrderNumber { get; set; }
    }

}
