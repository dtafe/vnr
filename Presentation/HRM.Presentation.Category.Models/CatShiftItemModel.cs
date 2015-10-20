using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatShiftItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_ShiftID)]
        public Guid ShiftID { get; set; }

        public Guid? ShiftOvertime_ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakOut)]
        public string ShiftItemName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_CoFrom)]
        public double CoFrom { get; set; }
        public DateTime? Intime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_CoTo)]
        public double CoTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_CoFrom)]
        public DateTime? From { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_CoTo)]
        public DateTime? To { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakOut)]
        public bool? IsNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakOut)]
        public bool? IsOfficial { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_OvertimeTypeID)]
        public Guid? OvertimeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_TimeCoBreakOut)]
        public string ShiftItemType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_DurationType)]
        public string DurationType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShiftItem_OvertimeTypeID)]
        public string OvertimeTypeName { get; set; }


        private Guid _id = Guid.Empty;
        public Guid ShiftItem_ID
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
            public const string ShiftID = "ShiftID";
            public const string ShiftItemName = "ShiftItemName";
            public const string CoFrom = "CoFrom";
            public const string CoTo = "CoTo";
            public const string IsNightShift = "IsNightShift";
            public const string IsOfficial = "IsOfficial";
            public const string OvertimeTypeID = "OvertimeTypeID";
            public const string OrgStructureID = "OrgStructureID";
            public const string DurationType = "DurationType";
            public const string ShiftItemType = "ShiftItemType";
            public const string OrgStructureName = "OrgStructureName";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string From = "From";
            public const string To = "To";
        
        }
    }
}
