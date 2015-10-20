using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class CatPositionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_Position_PositionName_StringLength)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_Description)]        
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_Code)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_Position_Code_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_OrgStructure)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_TargetAmount)]
        public double? TargetAmount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionEngName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_Position_PositionEngName_StringLength)]
        public string PositionEngName { get; set; }
    }
}
