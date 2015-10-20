using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using HRM.Infrastructure.Utilities;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using HRM.Presentation.UI.Controls.Kendo;

namespace HRM.Presentation.UI.Controls
{
    public class VnrSelectProfileOrOrgStructure
    {
        public VnrSelectProfileOrOrgStructure()
        {
            ProfileMultiSelect = new MultiSelectBuilderInfo();
            OrgTreeView = new TreeViewDropdDownBuilderInfo();
            BindingProfileForField = string.Empty;
            BindingExclusionProfileForField = string.Empty;
            BindingOrgStructureForField = string.Empty;
        }
        public string Name { get; set; }
        /// <summary>
        /// Tự binding dữ liệu khi select tới field chỉ định
        /// </summary>
        public string BindingProfileForField { get; set; }
        /// <summary>
        /// Tự binding dữ liệu khi select tới field chỉ định
        /// </summary>
        public string BindingExclusionProfileForField { get; set; }
        /// <summary>
        /// Tự binding dữ liệu khi select tới field chỉ định
        /// </summary>
        public string BindingOrgStructureForField { get; set; }
        public MultiSelectBuilderInfo ProfileMultiSelect { get; set; }        
        public TreeViewDropdDownBuilderInfo OrgTreeView { get; set; }

    }
}
