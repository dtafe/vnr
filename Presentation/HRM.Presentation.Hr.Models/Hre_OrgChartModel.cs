using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_OrgChartModel
    {
        public System.Guid ID { get; set; }
        public string OrgChartName { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsRoot { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string JobTitleName { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
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
    }

    public class Hre_OrgChartPortal
    {
        public Guid id { get; set; }
        public Guid? parentId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
}
