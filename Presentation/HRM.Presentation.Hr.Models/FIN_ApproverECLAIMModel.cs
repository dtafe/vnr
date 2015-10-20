using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_ApproverECLAIMModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_FIN_Claim_ProfileID)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_ApprovedType)]
        public string ApprovedType { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_ApprovedID)]
        public Guid? ApprovedID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_OrderNo)]
        public int? OrderNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_Note)]
        public string Note { get; set; }
        public string ProfileName { get; set; }
        public string ApprovedName { get; set; }
        public List<Guid> ApprovedIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_ApprovedType)]
        public string ApprovedTypeView { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string ApprovedType = "ApprovedType";
            public const string ApprovedTypeView = "ApprovedTypeView";
            public const string ApprovedID = "ApprovedID";
            public const string OrderNo = "OrderNo";
            public const string ApprovedName = "ApprovedName";
            public const string Note = "Note";
        }
    }

    public class FIN_ApproverECLAIMSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public string ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_ApproverECLAIM_ApprovedType)]
        public string ApprovedType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ApprovedName)]
        public string ApprovedID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_OrderNo)]
        public int? OrderNo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
