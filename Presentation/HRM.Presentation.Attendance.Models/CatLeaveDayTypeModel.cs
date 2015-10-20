using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Attendance.Models
{
    public class CatLeaveDayTypeModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_LeaveDayTypeName)]
        [StringLength(50, ErrorMessage = Constant.MsgStringLengthCatLeaveDayTypeName)]
        public string LeaveDayTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        [StringLength(50, ErrorMessage = Constant.MsgStringLengthCatLeaveDayTypeCode)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_PaidRate)]
        public float PaidRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_SocialRate)]
        public float SocialRate { get; set; }
    }
}
