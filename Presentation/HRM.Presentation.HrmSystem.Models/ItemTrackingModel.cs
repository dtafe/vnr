using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class ItemTrackingSearchModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class ItemTrackingModel
    {
        public Guid ID { get; set; }
        public string Alias { get; set; }
        public string CodeField { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }
        public string ExcludedFields { get; set; }
        public string InfoField { get; set; }
        public string Name { get; set; }
        public string NameField { get; set; }
        public bool Update { get; set; }

        public class FieldNames
        {
            public const string ID = "ID";
            public const string Alias = "Alias";
            public const string CodeField = "CodeField";
            public const string Create = "Create";
            public const string Delete = "Delete";
            public const string ExcludedFields = "ExcludedFields";
            public const string InfoField = "InfoField";
            public const string Name = "Name";
            public const string NameField = "NameField";
            public const string Update = "Update";
        }
    }

    public class TrackingInfoModel
    {
        public Guid ID { get; set; }
        public string CurrentValue { get; set; }
        public DateTime? DateUpdate { get; set; }
        public DateTime? DateUpdateTo { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string MoreInfo { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
        public string OriginalValue { get; set; }
        public string Reference { get; set; }
        public string State { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        public string UserUpdate { get; set; }

        public class FieldNames
        {
            public const string ID = "ID";
            public const string CurrentValue = "CurrentValue";
            public const string DateUpdate = "DateUpdate";
            public const string DateUpdateTo = "DateUpdateTo";
            public const string FieldName = "FieldName";
            public const string FieldType = "FieldType";
            public const string MoreInfo = "MoreInfo";
            public const string ObjectCode = "ObjectCode";
            public const string ObjectID = "ObjectID";
            public const string ObjectName = "ObjectName";
            public const string OriginalValue = "OriginalValue";
            public const string Reference = "Reference";
            public const string State = "State";
            public const string TableName = "TableName";
            public const string UserUpdate = "UserUpdate";
        }
    }
}
