using System;
using System.Collections.Generic;

namespace HRM.Infrastructure.Utilities
{
    public class GridBuilderModel
    {
        public Guid? ID { get; set; }
        public Guid UserID { get; set; }
        public string GridControlName { get; set; }
        public bool IsGet { get; set; }
        public string StringXml { get; set; }
        public int PageSize { get; set; }
        public string ActionStatus { get; set; }
        public string ObjectName { get; set; }
        public string[] ValueFields { get; set; }
        public Dictionary<string, int> SizeFields { get; set; }
        public string[] GroupFields { get; set; }
        public string[] LockedFields { get; set; }
        public Dictionary<string, bool> FilterFields { get; set; }
        public Dictionary<string, string> DisplayFields { get; set; }
        public Dictionary<string, string> Columns { get; set; }
    }

    public class ResultsObject
    {
        public ResultsObject()
        {
            Success = true;
            Messenger = ConstantDisplay.Hrm_Succeed.TranslateString();
        }
        public bool Success { get; set; }
        public string Messenger { get; set; }
    }
}
