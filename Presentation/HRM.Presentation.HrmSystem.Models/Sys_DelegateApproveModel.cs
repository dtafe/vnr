using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_DelegateApproveModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserID)]
        public Nullable<System.Guid> UserID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserID)]
        public string UserName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserDelegateID)]
        public Nullable<System.Guid> UserDelegateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserDelegateID)]
        public string UserDelegateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DataTypeDelegate)]
        public string DataTypeDelegate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DataTypeDelegate)]
        public string DataTypeDelegateView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_Status)]
        public string Status { get; set; }
        public partial class FieldNames
        {
            public const string UserName = "UserName";
            public const string UserDelegateName = "UserDelegateName";
            public const string DataTypeDelegateView = "DataTypeDelegateView";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DataTypeDelegate = "DataTypeDelegate";
            public const string Status = "Status";
        }
    }

    public class Sys_DelegateApproveSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserID)]
        public string UserName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_UserDelegateID)]
        public string UserDelegateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sys_DelegateApprove_DateTo)]
        public DateTime? DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}