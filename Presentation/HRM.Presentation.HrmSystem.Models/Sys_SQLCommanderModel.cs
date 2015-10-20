using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_SQLCommanderModel : BaseViewModel
    {
        public string ColumnName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_SQL_CommanText)]
        public string Commander { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public partial class FieldNames
        {
            public const string Commander = "Commander";
            public const string ColumnName = "ColumnName";
        }
    }
}
