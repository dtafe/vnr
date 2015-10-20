using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_UnusualAllowanceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Status { get; set; }
        public string StatusTranslate { get; set; }
        public System.Guid ProfileID { get; set; }//
        public string CodeEmp { get; set; }
        public string EDType { get; set; }
        public string EDTypeView { get; set; }
        public string OrgStructureID { get; set; }
        public string WorkingPlaceID { get; set; }
        public string EDTypeCfg { get; set; }
        public string ProfileName { get; set; }
        public Guid? Element { get; set; }
        public Nullable<System.Guid> UnusualEDTypeID { get; set; }
        public string UnusualEDTypeCode { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
        public System.Guid? BudgetId { get; set; }//
        public string BudgetName { get; set; }
        public string Type { get; set; }//
        public Nullable<double> Amount { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        //public System.DateTime? MonthStart { get; set; }//
        //public System.DateTime? MonthEnd { get; set; }//

        public Nullable<System.DateTime> MonthStart { get; set; }
        public Nullable<System.DateTime> MonthEnd { get; set; }

        public bool? AddToHourlyRate { get; set; }//
        public bool? ChargePIT { get; set; }//
        public bool? IsFollowDay { get; set; }
        public bool? IsAddToHourlyRate { get; set; }
        public string Notes { get; set; }//
        //public string UserUpdate { get; set; }
        //public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.DateTime> DateSubmitDoc { get; set; }//
        public Nullable<System.DateTime> DateOccur { get; set; }//
        public string TypeUnusual { get; set; }//
        public Nullable<int> NoCompensation { get; set; }//
        public Nullable<System.Guid> RelativeID { get; set; }//
        public string RelativeName { get; set; }
        public Nullable<System.Guid> RelativeTypeID { get; set; }
        public string RelativeTypeName { get; set; }
        public string YearOfBirth { get; set; }
        public string YearOld { get; set; }
        public string MonthOld { get; set; }
        public DateTime? DateHire { get; set; }
        //public double? AmountofOffset { get; set; }
        public Guid? CurrencyID2 { get; set; }
        public string OrgStructureName { get; set; }
        public string WorkPlaceName { get; set; }
        public string UnusualAllowanceCfgTypeName { get; set; }
        public Nullable<double> AmountOfOffSet { get; set; }
        public bool IsExcludePayslip { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        public string OrgStructureIDs { get; set; }
        public string ProfileIDs { get; set; }
        public string ExProfileIDs { get; set; }
        public partial class FieldNames
        {

            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateHire = "DateHire";
            public const string YearOfBirth = "YearOfBirth";
            public const string MonthStart = "MonthStart";
            public const string RelativeNumber = "RelativeNumber";
            public const string Amount = "Amount";
            public const string AmountOfOffSet = "AmountOfOffSet";
            public const string Notes = "Notes";
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";
            public const string UnusualAllowanceCfgCode = "UnusualAllowanceCfgCode";
            public const string NumberUnusualAllowancePlus = "NumberUnusualAllowancePlus";
            public const string AmountUnusualAllowancePlus = "AmountUnusualAllowancePlus";
            public const string NumberUnusualAllowanceMinus = "NumberUnusualAllowanceMinus";
            public const string AmountUnusualAllowanceMinus = "AmountUnusualAllowanceMinus";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string EDTypeView = "EDTypeView";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Sal_UnusualAllowanceMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
    }
}
