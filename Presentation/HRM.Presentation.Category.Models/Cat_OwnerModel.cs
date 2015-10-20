using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_OwnerModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OwnerName)]
        public string OwnerName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OwnerType)]
        public string OwnerType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OwnerParentID)]
        public Nullable<System.Guid> OwnerParentID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OrderNumber)]
        public Nullable<int> OrderNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OwnerParentID)]
        public string OwnerParentName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string OwnerName = "OwnerName";
            public const string OwnerType = "OwnerType";
            public const string OwnerParentID = "OwnerParentID";
            public const string OrderNumber = "OrderNumber";
            public const string Code = "Code";
            public const string Note = "Note";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
            public const string OwnerParentName = "OwnerParentName";
        }
    }

    public class Cat_OwnerSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Owner_OwnerName)]
        public string OwnerName { get; set; }
        //public string OwnerType { get; set; }
        //public Nullable<System.Guid> OwnerParentID { get; set; }
        //public string OwnerType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_OwnerMultiModel
    {
        public Guid ID { get; set; }
        public string OwnerName { get; set; }
    }



}
