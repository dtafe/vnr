using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class Hre_HDTJobModel : BaseModelPortal
    {
        #region [Quoc.Do]
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_HDTType)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_HDTJob_DateTo)]
        public DateTime? DateTo { get; set; }
        #endregion
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string HDTJobTypeName = "HDTJobTypeName";
           
        }
        
    }
}