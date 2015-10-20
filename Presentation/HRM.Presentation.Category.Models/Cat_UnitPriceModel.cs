using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UnitPriceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_RoleID)]
        public Nullable<System.Guid> RoleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_RoleID)]
        public string RoleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public Nullable<System.Guid> JobTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public string JobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string RoleID = "RoleID";
            public const string RoleName = "RoleName";
            public const string JobTypeID = "JobTypeID";
            public const string JobTypeName = "JobTypeName";
            public const string Amount = "Amount";
            public const string Date = "Date";
            public const string Note = "Note";
        }
    }

    public class Cat_UnitPriceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_RoleID)]
        public Nullable<System.Guid> RoleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public Nullable<System.Guid> JobTypeID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
