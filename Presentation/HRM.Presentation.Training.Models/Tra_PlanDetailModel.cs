using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_PlanDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public string PlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public Nullable<System.Guid> PlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_PlanDetail_OrgStructureName)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_PlanDetail_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_PlanDetail_Quantity)]
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> SumQuantity { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_PlanDetail_CourseName)]
        public string CourseName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string PlanName = "PlanName";
            public const string Quantity = "Quantity";
            public const string SumQuantity = "SumQuantity";
            public const string OrgStructureName = "OrgStructureName";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string CourseName = "CourseName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Tra_PlanDetailSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public Guid? PlanID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
  

}
