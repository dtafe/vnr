using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_UserSettingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_UserSetting_LanguageName)]
        [StringLength(200, ErrorMessage = ConstantDisplay.HRM_System_UserSetting_LanguageName_StringLength)]
        public string LanguageName { get; set; }

        public string LanguageValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_UserSetting_ThemeName)]
        [StringLength(200, ErrorMessage = ConstantDisplay.HRM_System_UserSetting_ThemeName_StringLength)]
        public string ThemeName { get; set; }
    }
}
