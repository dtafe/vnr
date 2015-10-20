using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileHDTInMonthModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_Month)]
        public DateTime? Month { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Unit)]
        public string Unit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Part)]
        public string Part { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Line)]
        public string Line { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Session)]
        public string Session { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeName)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeNameHVN)]
        public string HDTJobTypeNameHVN { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_StandardElement)]
        public string StandardElement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_EncryptJob)]
        public string EncryptJob { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        public string CodeShift1 { get; set; }
        public string CodeShift2 { get; set; }
        public string CodeShift3 { get; set; }
        public string CodeShift4 { get; set; }
        public string CodeShift5 { get; set; }
        public string CodeShift6 { get; set; }
        public string CodeShift7 { get; set; }
        public string CodeShift8 { get; set; }
        public string CodeShift9 { get; set; }
        public string CodeShift10 { get; set; }
        public string CodeShift11 { get; set; }
        public string CodeShift12 { get; set; }
        public string CodeShift13 { get; set; }
        public string CodeShift14 { get; set; }
        public string CodeShift15 { get; set; }
        public string CodeShift16 { get; set; }
        public string CodeShift17 { get; set; }
        public string CodeShift18 { get; set; }
        public string CodeShift19 { get; set; }
        public string CodeShift20 { get; set; }
        public string CodeShift21 { get; set; }
        public string CodeShift22 { get; set; }
        public string CodeShift23 { get; set; }
        public string CodeShift24 { get; set; }
        public string CodeShift25 { get; set; }
        public string CodeShift26 { get; set; }
        public string CodeShift27 { get; set; }
        public string CodeShift28 { get; set; }
        public string CodeShift29 { get; set; }
        public string CodeShift30 { get; set; }
        public string CodeShift31 { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Month = "Month";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Unit = "Unit";
            public const string Dept = "Dept";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string HDTJobTypeNameHVN = "HDTJobTypeNameHVN";
            public const string StandardElement = "StandardElement";
            public const string EncryptJob = "EncryptJob";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string CodeShift1 = "CodeShift1 ";
            public const string CodeShift2 = "CodeShift2 ";
            public const string CodeShift3 = "CodeShift3 ";
            public const string CodeShift4 = "CodeShift4 ";
            public const string CodeShift5 = "CodeShift5 ";
            public const string CodeShift6 = "CodeShift6 ";
            public const string CodeShift7 = "CodeShift7 ";
            public const string CodeShift8 = "CodeShift8 ";
            public const string CodeShift9 = "CodeShift9 ";
            public const string CodeShift10 = "CodeShift10";
            public const string CodeShift11 = "CodeShift11";
            public const string CodeShift12 = "CodeShift12";
            public const string CodeShift13 = "CodeShift13";
            public const string CodeShift14 = "CodeShift14";
            public const string CodeShift15 = "CodeShift15";
            public const string CodeShift16 = "CodeShift16";
            public const string CodeShift17 = "CodeShift17";
            public const string CodeShift18 = "CodeShift18";
            public const string CodeShift19 = "CodeShift19";
            public const string CodeShift20 = "CodeShift20";
            public const string CodeShift21 = "CodeShift21";
            public const string CodeShift22 = "CodeShift22";
            public const string CodeShift23 = "CodeShift23";
            public const string CodeShift24 = "CodeShift24";
            public const string CodeShift25 = "CodeShift25";
            public const string CodeShift26 = "CodeShift26";
            public const string CodeShift27 = "CodeShift27";
            public const string CodeShift28 = "CodeShift28";
            public const string CodeShift29 = "CodeShift29";
            public const string CodeShift30 = "CodeShift30";
            public const string CodeShift31 = "CodeShift31";
        }
      
    }
}
