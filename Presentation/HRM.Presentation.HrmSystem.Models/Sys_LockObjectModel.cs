using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_LockObjectModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public System.DateTime DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public System.DateTime DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserApprove1ID)]
        public Nullable<System.Guid> UserApprove1ID { get; set; }
        public string UserApprove1Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserApprove2ID)]
        public Nullable<System.Guid> UserApprove2ID { get; set; }
        public string UserApprove2Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructure)]
        public string OrgStructureID { get; set; }
        public List<int> lstOrgStructureID { get; set; }
        public byte[] OrgStructures { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollGroup)]
        public byte[] PayrollGroups { get; set; }
        public string PayrollGroupID { get; set; }
        public List<int> lstPayrollGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Comment)]
        public string Commend { get; set; }

        private Guid _id = Guid.Empty;
        public Guid? CutOffDurationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]

        public string WorkPlaceName { get; set; }
        public string OrgStructureName { get; set; }
        public string CutOffDurationName { get; set; }
        public string StatusView { get; set; }

        public List<Guid> selecteIds { get; set; }

        public Guid LockObject_ID
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

        public Guid? LockObjectTypeID { get; set; }
        public string NameEntityName { get; set; }
        public bool IsCreateTemplate { get; set; }
        public ExportFileType ExportType { get; set; }
        public Guid ExportId { get; set; }
        public partial class FieldNames
        {
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Type = "Type";
            public const string UserApprove1ID = "UserApprove1ID";
            public const string UserApprove2ID = "UserApprove2ID";
            public const string Status = "Status";
            public const string OrgStructures = "OrgStructures";
            public const string PayrollGroups = "PayrollGroups";
            public const string DateUpdate = "DateUpdate";
            public const string UserCreate = "UserCreate";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CutOffDurationName = "CutOffDurationName";
            public const string StatusView = "StatusView";

        }
    }
    public class Sys_LockObjectSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
    }
}
