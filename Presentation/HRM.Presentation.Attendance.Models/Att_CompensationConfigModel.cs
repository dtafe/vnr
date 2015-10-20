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
    public class Att_CompensationConfigModel : BaseViewModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_Year)]
        public Nullable<int> Year { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_MonthBeginInYear)]
        public Nullable<int> MonthBeginInYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_InitAvailable)]
        public Nullable<double> InitAvailable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_MonthResetInitAvailable)]
        public Nullable<int> MonthResetInitAvailable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_MonthStartProfile)]
        public Nullable<int> MonthStartProfile { get; set; }
        public string CompBankCode { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Year = "Year";
            public const string MonthBeginInYear = "MonthBeginInYear";
            public const string InitAvailable = "InitAvailable";
            public const string MonthResetInitAvailable = "MonthResetInitAvailable";
            public const string MonthStartProfile = "MonthStartProfile";
            public const string CompBankCode = "CompBankCode";
        }
    }
    public class Att_CompensationConfigSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CompensationConfig_Year)]
        public Nullable<int> Year { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
