using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatDayOffModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_LoopType)]
        public string LoopType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_DaysOFWeeks)]
        public string DaysOFWeeks { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_LoopNo)]
        public int? LoopNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_DateOff)]

        public DateTime DateOff { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        [MaxLength(500)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Comments)]
        [MaxLength(500)]
        public string Comments { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DayOff_IsLeaveDay)]
        public bool? IsLeaveDay { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_DayOff_UserUpdate)]
       public string UserUpdate { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_DayOff_DateUpdate)]
       public DateTime? DateUpdate { get; set; }

       [DisplayName(ConstantDisplay.HRM_Category_DayOff_Overwite)]
       public bool Overwite { get; set; }

        

       private DateTime? _dateEnd;
       public DateTime? Cat_DateEnd
       {
           get
           {
               _dateEnd = DateEnd;
               return _dateEnd;
           }
           set
           {
               _dateEnd = value;
               DateEnd = value;
           }
       }
       private DateTime? _dateStart;
       public DateTime? Cat_DateStart
       {
           get
           {
               _dateStart = DateStart;
               return _dateStart;
           }
           set
           {
               _dateStart = value;
               DateStart = value;
           }
       }


       private DateTime _dateOff;

       public DateTime Cat_DateOff {
           get
           {
               _dateOff = DateOff;
               return _dateOff;
           }
           set
           {
               _dateOff = value;
               DateOff = value;
           }
       }

        public partial class FieldNames
        {
            public const string DateOff = "DateOff";
            public const string Type = "Type";
            public const string Status = "Status";
            public const string Comments = "Comments";
            public const string OrgStructureID = "OrgStructureID";
            public const string IsLeaveDay = "IsLeaveDay";
            public const string OrgStructureName = "OrgStructureName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class CatDayOffSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
