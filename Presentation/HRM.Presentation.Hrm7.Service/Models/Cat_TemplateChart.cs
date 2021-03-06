//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Presentation.Hrm7.Service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cat_TemplateChart
    {
        public Cat_TemplateChart()
        {
            this.Cat_TemplateChartColumn = new HashSet<Cat_TemplateChartColumn>();
            this.Cat_TemplateChartItem = new HashSet<Cat_TemplateChartItem>();
        }
    
        public System.Guid ID { get; set; }
        public string TemplateChartName { get; set; }
        public Nullable<bool> Is3D { get; set; }
        public string GroupBy { get; set; }
        public Nullable<bool> IsSwap { get; set; }
        public string Color { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string PageType { get; set; }
        public string Page { get; set; }
    
        public virtual ICollection<Cat_TemplateChartColumn> Cat_TemplateChartColumn { get; set; }
        public virtual ICollection<Cat_TemplateChartItem> Cat_TemplateChartItem { get; set; }
    }
}
