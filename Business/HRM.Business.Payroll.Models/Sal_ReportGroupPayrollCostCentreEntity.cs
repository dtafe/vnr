using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_ReportGroupPayrollCostCentreEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string CodeEmp { get; set; }

        public string ProfileName { get; set; }

        public string GroupCode { get; set; }

        public string SectionCode { get; set; }

        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }
        public string ElementType { get; set; }
        public string ElementName { get; set; }
        public string CostCentreName { get; set; }
        public Nullable<double> Rate { get; set; }

        public double Rate_Money { get; set; }

        public Nullable<System.DateTime> DateStart { get; set; }
        public string JobtitleName { get; set; }
        public double Amount { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }




        public Guid ProfileID { get; set; }
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DepartmentCode = "DepartmentCode";
            public const string ElementType = "ElementType";
            public const string ElementName = "ElementName";
            public const string CostCentreName = "CostCentreName";
            public const string Rate = "Rate";
            public const string Rate_Money = "Rate_Money";
            public const string DateStart = "DateStart";
            public const string JobtitleName = "JobtitleName";
            public const string DepartmentName = "DepartmentName";
            public const string Amount = "Amount";
            public const string ProfileID = "ProfileID";
            public const string Brand = "Brand";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string WorkPlace = "WorkPlace";
            public const string Position = "Position";
            public const string PositionName = "PositionName";
            public const string PositionCode = "PositionCode";
            public const string DateHire = "DateHire";
        }
    }
}
