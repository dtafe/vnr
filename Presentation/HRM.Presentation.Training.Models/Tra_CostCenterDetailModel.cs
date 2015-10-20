using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_CostCenterDetailModel : BaseViewModel
    {
  
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_STT)]
        public string STT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_CostCenterDetail_CostCenterName)]

        public Guid? CostCenterID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_CostCenterDetail_CostCenterName)]

        public string CostCenterName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_CostCenterDetail_Amount)]

        public double? Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_CostCenterDetail_Description)]

        public string Description { get; set; }

        public Guid? TraineeID { get; set; }
        public partial class FieldNames
        {
            public const string STT = "STT";
            public const string CostCenterName = "CostCenterName";
            public const string Amount = "Amount";
            public const string Description = "Description";
           
        }
    
    
       
    }
    public class Tra_CostCenterMultiModel
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
}
