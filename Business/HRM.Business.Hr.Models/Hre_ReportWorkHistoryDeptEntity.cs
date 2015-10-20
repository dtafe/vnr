using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportWorkHistoryDeptEntity : HRMBaseModel
    {
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string FirstName { get; set; }
        public string NameFamily { get; set; }
        public Guid? OrgStructureID { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string OrgStructureName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string WorkPlace { get; set; }
        public string WorkPlaceOld { get; set; }
        public string PositionOld { get; set; }
        public string OrgOld { get; set; }
        public string JobTitleOld { get; set; }
        public string WorkLocationOld { get; set; }
        public DateTime? DateEffective { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Notes { get; set; }
        //
        public string UnitName { get; set; }
        public string DivisionName { get; set; }

        public string DepartmentName { get; set; }
        public string SectionName { get; set; }

        public string UnitNameOld { get; set; }
        public string DivisionNameOld { get; set; }

        public string DepartmentNameOld { get; set; }
        public string SectionNameOld { get; set; }
        public string Description { get; set; }
        public string SalaryClassName { get; set; }

        public string SalaryClassNameOld { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string WorkPlaceName { get; set; }


     
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string FirstName = "FirstName";
            public const string NameFamily = "NameFamily";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string WorkPlace = "WorkPlace";
            public const string WorkPlaceOld = "WorkPlaceOld";
            public const string PositionOld = "PositionOld";
            public const string OrgOld = "OrgOld";
            public const string JobTitleOld = "JobTitleOld";
            public const string WorkLocationOld = "WorkLocationOld";
            public const string DateEffective = "DateEffective";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string WorkPlaceName = "WorkPlaceName";


        }
    }
}
