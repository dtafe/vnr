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

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobtitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionEngName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_Position_PositionEngName_StringLength)]
        public string PositionEngName { get; set; }

        public string JobTitleCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public string CostCentreName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PositionName = "PositionName";
            public const string Description = "Description";
            public const string Code = "Code";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";       
            public const string PositionEngName = "PositionEngName";
            public const string CostCentreName = "CostCentreName";
            public const string OrgStructureName = "OrgStructureName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate"; 
        }
    }
    public class CatPositionSearchModel
    {
        //public int Id { get; set; }
        public string PositionName { get; set; }
        public string Code { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        
    }
    public class CatPositionMultiModel
    {
        public Guid ID { get; set; }
        public string PositionName { get; set; }
    }
}
