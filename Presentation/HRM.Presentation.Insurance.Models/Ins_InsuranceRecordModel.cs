using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_InsuranceRecordModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_InsuranceType)]
        public string InsuranceType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_InsuranceType)]
        public string InsuranceTypeTrans { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_RecordDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime RecordDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DateSuckle)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateSuckle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_TypeSuckle)]

        public string TypeSuckle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_TypeSick)]

        public string TypeSick { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DateStartWorking)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateStartWorking { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DayCount)]

        public double DayCount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_DayCountOld)]
        public Nullable<double> DayCountOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_LeaveInYear)]
        public Nullable<double> LeaveInYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_Comment)]
        public string Comment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_TypeData)]
        public string TypeData { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relative)]
        public Nullable<System.Guid> RelativesID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Relative)]
        public string RelativesName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_ChildSick)]
        public Nullable<System.Guid> ChildSickID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Insurance_ChildSick)]
        public string ChildSickName { get; set; }
        public int TotalRow { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string InsuranceType = "InsuranceType";
            public const string InsuranceTypeTrans = "InsuranceTypeTrans";
            public const string RecordDate = "RecordDate";
            public const string DayCount = "DayCount";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
    public class Ins_InsuranceRecordSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_InsuranceType)]
        public string InsuranceType { get; set; }
        public DateTime? RecordDateFrom { get; set; }
        public DateTime? RecordDateTo { get; set; }
        public DateTime? DateStartFrom { get; set; }
        public DateTime? DateStartTo { get; set; }
        public DateTime? DateEndFrom { get; set; }
        public DateTime? DateEndTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
   
}
