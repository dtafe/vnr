using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

using HRM.Infrastructure.Utilities;
using System.ComponentModel;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSumaryShiftMonthEntity : HRMBaseModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }//
        public string ProfileName { get; set; }//
        public string BranchCode { get; set; }//
        public string OrgStructureCode { get; set; }//
        
      
        public string TeamCode { get; set; }//
        public string SectionCode { get; set; }//
        public string BranchName { get; set; }//
        public string OrgStructureName { get; set; }//
        
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }//
        public string SectionName { get; set; }//
   
        public DateTime? DateExport { get; set; }//
        public DateTime? DateFrom { get; set; }//
        public DateTime? DateTo { get; set; }//
        public string JobtitleCode { get; set; }//
        public string PositionCode { get; set; }//

        public partial class FieldNames
        {

            public const string OrgStructureCode = "OrgStructureCode";//
            public const string CodeEmp = "CodeEmp";//
            public const string ProfileName = "ProfileName";//
            public const string BranchCode = "BranchCode";//
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";//
            public const string SectionCode = "SectionCode";//
            public const string BranchName = "BranchName";//
            public const string OrgStructureName = "OrgStructureName";//
            
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";//
            public const string SectionName = "SectionName";//
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";//
            public const string DateFrom = "DateFrom";//
            public const string DateTo = "DateTo";//
            public const string JobtitleCode = "JobtitleCode";//
            public const string PositionCode = "PositionCode";//


        }                         
    }

}
