using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportRelativesModel : BaseViewModel
    {
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_Report_E_UNIT)]
        public string E_UNIT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_Report_E_DIVISION)]
        public string E_DIVISION { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_Report_E_DEPARTMENT)]
        public string E_DEPARTMENT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_Report_E_SECTION)]
        public string E_SECTION { get; set; }
         [DisplayName(ConstantDisplay.HRM_Hre_Report_E_TEAM)]
        public string E_TEAM { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName)]
        public string RelativeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_RelativeName)]
        public string RelativeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOfBirth)]
        public string YearOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Relatives_Address)]
        public string Address { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PositionID)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName)]
        public Guid? RelativeTypeID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportID { get; set; }




        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOfBirth)]
        public DateTime? RelativesOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassName)]
        public string SalaryClassName { get; set; }
        public string WorkPlaceName { get; set; }
        public partial class FieldNames

        {
            public const string E_TEAM = "E_TEAM";
            public const string Address = "Address";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string ID = "ID";
            public const string RelativesCode = "RelativesCode";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DependantID = "DependantID";
            public const string DependantName = "DependantName";
            public const string RelativeName = "RelativeName";
            public const string YearOfBirth = "YearOfBirth";
            public const string YearOfBirths = "YearOfBirths";
            public const string Gender = "Gender";
            public const string CountryID = "CountryID";
            public const string CountryName = "CountryName";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
            public const string Career = "Career";
            public const string Notes = "Notes";
            public const string RelativeTypeName = "RelativeTypeName";
            public const string RelativeTypeID = "RelativeTypeID";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Relative_ID = "Relative_ID";


            public const string RankID = "RankID";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string RelativesOfBirth = "RelativesOfBirth";
            public const string SalaryClassName = "SalaryClassName";
            public const string WorkPlaceName = "WorkPlaceName";
        }
    }

}
