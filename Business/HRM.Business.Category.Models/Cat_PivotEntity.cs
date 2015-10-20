using System;

namespace HRM.Business.Category.Models
{
    public class Cat_PivotMultiEntity
    {
        public Guid ID { get; set; }
        public string PivotName { get; set; }
    }
    public class Cat_PivotEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string PivotName { get; set; }
        //public string ScreenName { get; set; }
        //public string ObjectName { get; set; }

        //public long? SheetIndex { get; set; }

        //public long? StartRowIndex { get; set; }

        //public long? StartColumnIndex { get; set; }
        //public string TemplateFile { get; set; }

        public bool? IsGroup { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? IsProtected { get; set; }
        public bool? IsColumnDynamic { get; set; }
        public bool? IsSmartMarkers { get; set; }
    }
}
