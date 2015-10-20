
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_HreConfigEntity
    {
        #region Cấu hình Nhân Sự

        // Kiểm tra trùng tên nv theo BDF - kiểm tra trùng tên nv
        public bool? IsGroupByOrgProfileQuit { get; set; }
        public bool? IsCheckDuplicateProfile { get; set; }
        public bool? IsCheckDuplicateCodeEmp { get; set; }
        public bool? IsCheckDuplicateCodeAtt { get; set; }
        public bool? IsCheckDuplicateRelatives { get; set; }
        public bool? IsCheckDuplicateProfileAndBirthday { get; set; }
        public bool? IsCheckDuplicateIDNo { get; set; }
        public string ProfileQuitBackGroundColor { get; set; }
        public string ProfileQuitColor { get; set; }
        public string GenerateCodeContractFormular { get; set; }//Công Thức sinh mã hợp đồng
        public string DefautCurrency { get; set; }
        public bool? IsShowExConByConResult { get; set; }
        public int? DaySuspenseExpiry { get; set; }
        public int? DayStopWorkingExpiry { get; set; }
        public int? DayComeBackExpiry { get; set; }
        public bool? IsInputGeneralCandidateManual { get; set; }
        public bool? IsUseCodeEmpOfCustomer { get; set; }
        public bool? IsAlertIfNumberOfEmpExceedPlan { get; set; }
        public string FieldValidateHireProfile { get; set; } //cấu hình field bắt buộc khi click nút nhận việc.
        public bool? IsNotUseExpiryContractLoop { get; set; }

        #endregion 
    }
}
