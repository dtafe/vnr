using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_VisaInfoSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_VisaInfo_DateEnd)]
        public DateTime? DateEnd { get; set; }
        public DateTime? DateTo { get; set; }
        
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }


        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        //public Nullable<DateTime> DateQuit { get; set; }

        //public Nullable<DateTime> DtToQuit { get; set; }

        public Guid ExportID { get; set; }
        public ExportFileType ExportType { get; set; }

    }

    public class Hre_VisaInfoModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_VisaInfo_Name)]
        public string VisaInfoName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Country)]
        public System.Guid? CountryID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Country)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_VisaInfo_DateStart)]
        public System.DateTime? DateStart { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_VisaInfo_DateEnd)]
        public System.DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_VisaInfo_Notes)]
         public string Notes { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Visa_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string VisaInfoName = "VisaInfoName";
            public const string CountryID = "CountryID";
            public const string CountryName = "CountryName";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Notes = "Notes";
            public const string Visa_ID = "Visa_ID";
            
        }
    }
}
