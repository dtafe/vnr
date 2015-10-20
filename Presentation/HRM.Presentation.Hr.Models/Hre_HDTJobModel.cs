using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_HDTJobModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusData)]
        public string StatusDataOutView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusData)]
        public string StatusDataOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusData)]
        public string StatusData { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusData)]
        public string StatusDataView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusOut)]
        public string StatusOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_StatusOut)]
        public string StatusOutView { get; set; } 
        public string selectedIds { get; set; }
        public string ProfileIDs { get; set; }
        public DateTime? DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string HDTJobGroupCode { get; set; }
        public string StandardElement { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public Guid? HDTJobTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public string HDTJobTypeName { get; set; }



        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_Type)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_Note)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }

        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string StatusView { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public Nullable<System.Guid> HDTJobGroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        public string UserLoginID { get; set; }


        private Guid _id = Guid.Empty;
        public Guid HDTJob_ID
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
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Unit)]
        public string Unit { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Section)]
        public string Part { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Line)]
        public string Line { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Session)]
        public string Session { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string HDTJobTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeNameHVN)]
        public string HDTJobTypeNameHVN { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_EncryptJob)]
        public string EncryptJob { get; set; }
        public string GroupType { get; set; }
        // Lấy thông tin mã cv,tên cv, mã vị trí cũ
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobTypeCodeOld)]
        public string HDTJobTypeCodeOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobTypeNameOld)]
        public string HDTJobTypeNameOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobGroupNameOld)]
        public string HDTJobGroupNameOld { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTJobGroupCodeOld)]
        public string HDTJobGroupCodeOld { get; set; }
        public double? PriceOld { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PriceOld = "PriceOld";
            public const string HDTJobTypeCodeOld = "HDTJobTypeCodeOld";
            public const string HDTJobGroupNameOld = "HDTJobGroupNameOld";
            public const string HDTJobGroupCodeOld = "HDTJobGroupCodeOld";
            public const string GroupType = "GroupType";
            public const string EncryptJob = "EncryptJob";
            public const string HDTJobTypeNameHVN = "HDTJobTypeNameHVN";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Type = "Type";
            public const string Description = "Description";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string Dept = "Dept";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
            public const string Unit = "Unit";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string Price = "Price";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string StatusOut = "StatusOut";
            public const string HDTJobTypeNameOld = "HDTJobTypeNameOld";
            public const string StatusOutView = "StatusOutView";
            public const string StatusData = "StatusData";
            public const string StatusDataView = "StatusDataView";
            public const string StatusDataOut = "StatusDataOut";
            public const string StatusDataOutView = "StatusDataOutView";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
        }
    }

    public class Hre_HDTJobSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public Guid? HDTJobTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeID = "HDTJobTypeID";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }

    public class Hre_HDTJobWaitingSearchModel
    {

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public Guid? HDTJobTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }

        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeID = "HDTJobTypeID";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }

    }

    public class Hre_HDTJobOutSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public Guid? HDTJobTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string Status { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeID = "HDTJobTypeID";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }

    }

    public class Hre_ReportHDTJobOutSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDT_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Guid? WorkPlaceID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ReportHDTJobInSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDT_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Guid? WorkPlaceID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Hre_ReportHDTJobDecisionAssignWorkSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTos)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
