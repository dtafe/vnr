using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Insurance.Models
{
    /// <summary>Đuôi D02</summary>
    public class Ins_InsuranceReportD02TailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_INS_D02Tail_PhatSinh)]
        public string Name { get; set; }
         [DisplayName(ConstantDisplay.HRM_INS_D02Tail_PrePeriodInsSocialAmount)]
        public double PrePeriodInsSocialAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_D02Tail_PrePeriodInsHealthAmount)]
        public double PrePeriodInsHealthAmount { get; set; }
          [DisplayName(ConstantDisplay.HRM_INS_D02Tail_PrePeriodInsUnEmpAmount)]
        public double PrePeriodInsUnEmpAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsSocialIncreaseAmount)]
        public double InsSocialIncreaseAmount { get; set; }
          [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsSocialDecreaseAmount)]
        public double InsSocialDecreaseAmount { get; set; }
         [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsHealthIncreaseAmount)]
        public double InsHealthIncreaseAmount { get; set; }
          [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsHealthDecreaseAmount)]
        public double InsHealthDecreaseAmount { get; set; }
         [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsUnEmpIncreaseAmount)]
        public double InsUnEmpIncreaseAmount { get; set; }
          [DisplayName(ConstantDisplay.HRM_INS_D02Tail_InsUnEmpDecreaseAmount)]
        public double InsUnEmpDecreaseAmount { get; set; }

          public int STT{ get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public bool? IsExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_MonthYear)]
        public DateTime? MonthYear { get; set; }       
        public Guid ExportId { get; set; }

        public partial class FieldNames
        {
            public const string Name = "Name";
            public const string PrePeriodInsSocialAmount = "PrePeriodInsSocialAmount";
            public const string PrePeriodInsHealthAmount = "PrePeriodInsHealthAmount";
            public const string PrePeriodInsUnEmpAmount = "PrePeriodInsUnEmpAmount";
            public const string InsSocialIncreaseAmount = "InsSocialIncreaseAmount";
            public const string InsSocialDecreaseAmount = "InsSocialDecreaseAmount";
            public const string InsHealthIncreaseAmount = "InsHealthIncreaseAmount";
            public const string InsHealthDecreaseAmount = "InsHealthDecreaseAmount";
            public const string InsUnEmpIncreaseAmount = "InsUnEmpIncreaseAmount";
            public const string InsUnEmpDecreaseAmount = "InsUnEmpDecreaseAmount";
        }
    }
    public class Ins_InsuranceReportD02TailSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_MonthYear)]       
        public DateTime? MonthYear { get; set; }       
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }

}
