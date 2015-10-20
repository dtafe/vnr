using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_MPModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Hre_MP_ProfileID)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_FromDate)]
        public Nullable<System.DateTime> FromDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_ToDate)]
        public Nullable<System.DateTime> ToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_MPRank)]
        public string MPRank { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_MPRank)]
        public string MPRankView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_MP_Notes)]
        public string Notes { get; set; }
        private Guid _id = Guid.Empty;
        public Guid MP_ID
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
            public const string ProfileName = "ProfileName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string MPRank = "MPRank";
            public const string Notes = "Notes";
            public const string MPRankView = "MPRankView";
        }
    }
}
