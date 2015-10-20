using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ComputeWorkDayModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_ProfileID)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_DateFrom)]
        public DateTime DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_DateTo)]
        public DateTime DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_ComputeContinue)]
        public bool ComputeContinue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_ComputeRepeat)]
        public bool ComputeRepeat { get; set; }

        public bool ComputeType { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public string selectedIds { get; set; }
        public Guid UserCreateID { get; set; }
        public Guid AsynTaskID { get; set; }

        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Reason = "Reason";
            public const string ComputeContinue = "ComputeContinue";
            public const string ComputeRepeat = "ComputeRepeat";
            public const string ComputeType = "ComputeType";
        }
    }
}
