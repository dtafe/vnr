using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_CardHistoryModel : BaseViewModel
    {
      
        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_ProfileID)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_CardCode)]
        [MaxLength(50)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_DateEffect)]
        public DateTime? DateEffect { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public Guid OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }


        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CardCode = "CardCode";
            public const string DateEffect = "DateEffect";
            public const string UserCreate = "UserCreate";
            public const string UserUpdate = "UserUpdate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";          
            public const string PositionName = "PositionName";        
            public const string JobTitleName = "JobTitleName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }      
    }
    public class Hre_CardHistorySearchModel 
    {  
        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_ProfileID)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_PositionID)]
        public Guid? PositionID { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_CardCode)]
        public string CardCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {           
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureID = "OrgStructureID";
            public const string JobTitleID = "JobTitleID";
            public const string PositionID = "PositionID";
            public const string CardCode = "CardCode";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
}
