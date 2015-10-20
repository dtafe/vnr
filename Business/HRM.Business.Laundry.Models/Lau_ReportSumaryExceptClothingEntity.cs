using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Laundry.Models
{
    public class Lau_ReportSumaryExceptClothingEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }

        public string CodeEmp { get; set; }

        public string ProfileName { get; set; }

        public string LockerName { get; set; }

        public string LineName { get; set; }

        /// <summary> Số Bộ Giặt </summary>
        public double Amount { get; set; }

        /// <summary> Số Bộ Giặt Trong Tháng </summary>
        public double SumMonthAmount { get; set; }

        /// <summary> Số bộ vượt tiêu chuẩn </summary>
        public double ExceedingStandards { get; set; }

        /// <summary> Tiêu Chuẩn Trong Tháng </summary>
        public double MonthStandards { get; set; }

        /// <summary>Tiền Trừ Vào Lương </summary>
        public double SubtractSalary { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public string CodeBranch { get; set; }

        public string CodeOrg { get; set; }

        public string CodeTeam { get; set; }

        public string CodeSection { get; set; }

        public string BranchName { get; set; }

        public string OrgName { get; set; }

        public string TeamName { get; set; }

        public string SectionName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string LockerName = "LockerName";
            public const string LineName = "LineName";
            public const string Amount = "Amount";
            public const string SumMonthAmount = "SumMonthAmount";
            public const string ExceedingStandards = "ExceedingStandards";
            public const string SubtractSalary = "SubtractSalary";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeBranch = "CodeBranch";
            public const string CodeOrg = "CodeOrg";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
        }
    }
}
