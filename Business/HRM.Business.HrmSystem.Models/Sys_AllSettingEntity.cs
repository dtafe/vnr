
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_ForeignKeyMultiEntity
    {
        public string CONSTRAINT_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string R_CONSTRAINT_NAME { get; set; }
    }

    public class DuplicateEntity
    {
        public Guid ID { get; set; }
    }
    public class Sys_AllSettingEntity 
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string ModuleName { get; set; }
        public Guid UserID { get; set; }

        public string MaxHoursOneShift { get; set; }
        public string MaxHoursNextInOut { get; set; }
        public string MinMinutesSameAtt { get; set; }
        public string TypeLoadData { get; set; }
        public string SymbolIn { get; set; }
        public string SymbolOut { get; set; }
        public string DetectShift { get; set; }
        public Nullable<bool> IsDelete { get; set; }

        public int? TotalRow { get; set; }
    }
}
