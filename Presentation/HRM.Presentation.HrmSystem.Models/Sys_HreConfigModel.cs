using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_HreConfigModel : BaseViewModel
    {
        #region HRM_Hr_Hre (tab Nhân Sự)

        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsGroupByOrgProfileQuit)]
        public bool? IsGroupByOrgProfileQuit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateProfile)]
        public bool? IsCheckDuplicateProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateCodeEmp)]
        public bool? IsCheckDuplicateCodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateCodeAtt)]
        public bool? IsCheckDuplicateCodeAtt { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateRelatives)]
        public bool? IsCheckDuplicateRelatives { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateProfileAndBirthday)]
        public bool? IsCheckDuplicateProfileAndBirthday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateIDNo)]
        public bool? IsCheckDuplicateIDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_ProfileQuitBackGroundColor)]
        public string ProfileQuitBackGroundColor { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_ProfileQuitColor)]
        public string ProfileQuitColor { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_GenerateCodeContractFormular)]
        public string GenerateCodeContractFormular { get; set; }//Công Thức sinh mã hợp đồng
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_DefautCurrency)]
        public string DefautCurrency { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsShowExConByConResult)]
        public bool? IsShowExConByConResult { get; set; }
        public int? DaySuspenseExpiry { get; set; }
        public int? DayStopWorkingExpiry { get; set; }
        public int? DayComeBackExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsInputGeneralCandidateManual)]
        public bool? IsInputGeneralCandidateManual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsUseCodeEmpOfCustomer)]
        public bool? IsUseCodeEmpOfCustomer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsAlertIfNumberOfEmpExceedPlan)]
        public bool? IsAlertIfNumberOfEmpExceedPlan { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_Hre_FieldValidateHireProfile)]
        public string FieldValidateHireProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsNotUseExpiryContractLoop)]
        public bool? IsNotUseExpiryContractLoop { get; set; }
        #endregion


    }

}
