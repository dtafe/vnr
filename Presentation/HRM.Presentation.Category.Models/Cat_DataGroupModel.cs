using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_DataGroupMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string DataGroupName { get; set; }
        public int OrderNumber { get; set; }
    }
    public class Cat_DataGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DataGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DataGroup_DataGroupName)]
        public string DataGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DataGroup_OrderNumber)]
        public int OrderNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DataGroup_Notes)]
        public string Notes { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        Guid _id = Guid.Empty;
        public Guid DataGroup_ID
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
            public const string DataGroupName = "DataGroupName";
            public const string OrderNumber = "OrderNumber";
            public const string Notes = "Notes";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_DataGroupSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DataGroup_DataGroupName)]
        public string DataGroupName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
