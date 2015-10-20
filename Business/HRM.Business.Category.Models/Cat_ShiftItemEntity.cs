using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ShiftItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public int TotalRow { get; set; }
        public Guid ShiftID { get; set; }
        public string ShiftItemName { get; set; }
        public double CoFrom { get; set; }
        public double CoTo { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? Intime { get; set; }
        public bool? IsNightShift { get; set; }
        public bool? IsOfficial { get; set; }
        public Guid? OvertimeTypeID { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string ShiftItemType { get; set; }
        public string DurationType { get; set; }
        public string OrgStructureName { get; set; }
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

    }
}
