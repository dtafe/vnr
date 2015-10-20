using System.Collections.Generic;

namespace HRM.Infrastructure.Utilities
{
    public class ListQueryModel
    {
        public List<SortAttribute> Sorts { get; set; }
        public List<FilterAttribute> Filters { get; set; }
        public List<FilterAttribute> AdvanceFilters { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string UserLogin { get; set; }
    }

    public class ParamaterModle
    {
        public string SqlQuery { get; set; }
        public object[] Params { get; set; }
    }
}
