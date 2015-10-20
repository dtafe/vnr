using System;


namespace HRM.Business.Category.Models
{
    public class Cat_PayrollGroupEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string PayrollGroupName { get; set; }
        public string Code { get; set; }
        public int? SalaryDateStart { get; set; }
        public int? SalaryMonthStart { get; set; }
        public string Description { get; set; }
        public Guid? ReportMappingID { get; set; }
        public Guid? ReportMappingID1 { get; set; }
        public int? OrderNumber { get; set; }
        public string ReportMappingName { get; set; }
        public string ReportMappingName1 { get; set; }
    }

    public class Cat_PayrollGroupMultiEntity 
    {
        public Guid ID { get; set; }
        public string PayrollGroupName { get; set; }
        public int? OrderNumber { get; set; }
    }
   
}

