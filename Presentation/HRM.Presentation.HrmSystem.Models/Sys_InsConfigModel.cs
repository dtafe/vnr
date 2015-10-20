using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_InsConfigModel : BaseViewModel
    {
        #region HRM_Sys_Ins (tab Bảo Hiểm)

        /// <summary> Tổng hợp dữ liệu BH tạm đến ngày [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_SumDataTempt)]
        public int? SumDataTempt{ get; set; }

        /// <summary> Tổng hợp dữ liệu BH chính thức đến ngày [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_SumDataMain)]
        public int? SumDataMain{ get; set; }

        /// <summary> Ngày Chốt BH Báo Tăng LĐ (ngày 18 đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDay)]
        public int? PeriodInsuranceDay { get; set; }

        /// <summary> Ngày Bắt Đầu Chu Kỳ Báo Giảm LĐ (ngày 15 tháng N-1 đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayDecreaseFrom)]
        public int? PeriodInsuranceDayDecreaseFrom { get; set; }
        /// <summary> Ngày Chốt Chu Kỳ Báo Giảm LĐ (ngày 14 tháng N đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayDecreaseFrom)]
        public int? PeriodInsuranceDayDecreaseTo { get; set; }


        /// <summary> Ngày Bắt Đầu Chu Kỳ BH  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayDefaultFrom)]
        public int? PeriodInsuranceDayDefaultFrom { get; set; }
        /// <summary> Ngày Chốt Chu Kỳ BH (ngày 14 tháng N đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayDefaultFrom)]
        public int? PeriodInsuranceDayDefaultTo { get; set; }

        /// <summary> Ngày bắt đầu số ngày nghỉ >=14 ngày  (ngày 1 tháng N đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsurance14DayFrom)]
        public int? PeriodInsurance14DayFrom { get; set; }
        /// <summary> Ngày kết thúc số ngày nghỉ >=14 ngày  (ngày 31 tháng N đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsurance14DayFrom)]
        public int? PeriodInsurance14DayTo { get; set; }

        /// <summary> Ngày bắt đầu Thay đổi lương  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayChangeSalFrom)]
        public int? PeriodInsuranceDayChangeSalFrom { get; set; }

        /// <summary> Ngày kết thúc Thay đổi lương  (ngày 14 tháng [N] đối với honda) [int] </summary>
        [DisplayName(ConstantDisplay.HRM_Hr_INS_PeriodInsuranceDayChangeSalFrom)]
        public int? PeriodInsuranceDayChangeSalTo { get; set; }


        #endregion


    }

}
