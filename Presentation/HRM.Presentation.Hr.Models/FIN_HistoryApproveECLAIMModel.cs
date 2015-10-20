using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_HistoryApproveECLAIMModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ProfileID)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ApprovedType)]
        public string ApprovedType { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_CashAdvanceID)]
        public Guid? CashAdvanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ClaimID)]
        public Guid? ClaimID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_TravelRequestID)]
        public Guid? TravelRequestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_TravelRequestID)]
        public string TravelRequestName { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_DateApproved)]
        public DateTime? DateApproved { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_UserApproveClaimID)]
        public Guid? UserApproveClaimID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_OrderNo)]
        public int? OrderNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ApprovedIDs)]
        public List<Guid> ApprovedIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ApprovedIDs)]
        public string ApprovedName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ApprovedType)]
        public string ApprovedTypeView { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_UserApproveClaimID)]
        public string UserApproveClaimName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ApprovedType = "ApprovedType";
            public const string DateApproved = "DateApproved";
            public const string OrderNo = "OrderNo";
            public const string Note = "Note";
            public const string ProfileName = "ProfileName";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string ApprovedName = "ApprovedName";
            public const string ApprovedTypeView = "ApprovedTypeView";
            public const string UserApproveClaimName = "UserApproveClaimName";
            public const string TravelRequestName = "TravelRequestName";
        }
    }

    public class FIN_HistoryApproveECLAIMSearchModel
    {
        public DateTime? DateApprovedFrom { get; set; }
        public DateTime? DateApprovedTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_ApprovedType)]
        public string ApprovedType { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_HistoryApproveECLAIM_Status)]
        public string Status { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
