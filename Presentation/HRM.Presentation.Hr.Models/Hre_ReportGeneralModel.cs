using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportGeneralSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)] 
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AllowLimitOvertime_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_TypeOfStop)]
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public Guid? ResignReasonID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassID)]
        public Nullable<System.Guid> SalaryClassID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }        
    }

    public class Hre_ReportContractExpiredSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateHireFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateHireTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateSignedStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateSignedEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> SalaryClassID { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }
}
