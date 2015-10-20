
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_SalConfigEntity 
    {
        #region tab 1

        //Cấu hình phần tử
        public string ConfigElement  { get; set; }
        public string Value8 { get; set; }
        public string Value52 { get; set; }
        public string Value53 { get; set; }
        public string Value54 { get; set; }
        public string Value55 { get; set; }
        public string Value56 { get; set; }
        public string Value57 { get; set; }
        public string Value58 { get; set; }
        public string Value59 { get; set; }
        //Phần tử dùng để tính hold lương
        public string HRM_SAL_HOLDSALARY_ELEMENT { get; set; }
        public string HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX { get; set; }
        public string HRM_SAL_ELEMENT_REALWAGES { get; set; }
        public DateTime? HRM_SAL_DATECLOSE_ALLOWANCE { get; set; }
        public double? HRM_SAL_NUMBER_WARRING_HOMEPAGE { get; set; }
        public double? HRM_SAL_DATECLOSE_SALARY { get; set; }
        public bool? HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER { get; set; }
        #endregion
    }
}
