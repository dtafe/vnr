using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatCostCentreModel :BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_CatCostCentre_CostCentreName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatCostCentre_CostCentreName_StringLength)]      
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_CatCostCentre_Code)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatCostCentre_Code_StringLength)] 
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_GroupCost)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CostCentre_GroupCost_StringLength)]         
        public string GroupCost { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_Description)]
        [StringLength(500, ErrorMessage = ConstantDisplay.HRM_Category_CostCentre_Description_StringLength)]          
        public string Description { get; set; }

        
       
        //public virtual ICollection<Cat_JobTitle> Cat_JobTitles { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CostCentreName = "CostCentreName";
            public const string Code = "Code";
            public const string GroupCost = "GroupCost";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class CatCostCentreMultiModel
    {
        public Guid ID { get; set; }
        public string CostCentreName { get; set; }
        public string Code { get; set; }

    }

    public class CatCostCentreSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_CostCentreName)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_CostCentre_Code)]
        public string CostCentreCode { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
