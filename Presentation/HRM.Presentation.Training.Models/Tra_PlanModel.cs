using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_PlanModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public string PlanName { get; set; }
        public Nullable<System.Guid> PlanTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Audience { get; set; }
        public string AchieveTarget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Cost)]
        public Nullable<double> Cost { get; set; }
        public Nullable<bool> IsApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Code)]
        public string Code { get; set; }
        public Nullable<int> Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Outside)]
        public Nullable<bool> Outside { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Inside)]
        public Nullable<bool> Inside { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_SumQuantity)]
        public int? SumQuantity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_TotalClass)]
        public Nullable<int> TotalClass { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_CourseName)]
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_CourseName)]
        public string CourseName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Plan_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string PlanName = "PlanName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Cost = "Cost";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string Description = "Description";
            public const string CourseName = "CourseName";
        }
    }
    public class Tra_PlanSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public string PlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_PlanName)]
        public string PlanCode { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Tra_PlanMultiModel
    {
        public Guid ID { get; set; }
        public string PlanName { get; set; }
        public int TotalRow { get; set; }
    }

}
