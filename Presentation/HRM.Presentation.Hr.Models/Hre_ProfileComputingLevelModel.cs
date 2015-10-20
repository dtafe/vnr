using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfileComputingLevelModel : BaseViewModel 
    {


        [DisplayName(ConstantDisplay.HRM_HR_Computing_SpecialLevelID)]
        public Guid SpecialLevelID { get; set; }

      
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Computing_SpecialTypeID)]
        public Guid SpecialTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_SpecialLevelID)]
        public string ComputingLevel { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_SpecialLevelID)]
        public string ComputingType { get; set; }


        private Guid _id = Guid.Empty;
        public Guid Computing_ID
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
           
            public const string ComputingLevel = "ComputingLevel";
            public const string ComputingType = "ComputingType";
            public const string ProfileID = "ProfileID";
            public const string SpecialLevelID = "SpecialLevelID";
            public const string SpecialTypeID = "SpecialTypeID";
            public const string ProfileName = "ProfileName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string Computing_ID = "Computing_ID";
           
         
        }
    }
   
}
