using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Hre_ProfileDataSourceController : Controller
    { 
        // GET: Hre_ProfileDataSource
        public ActionResult Index()
        {
            return View();
        }

        private List<SortAttribute> ExtractSortAttributes(DataSourceRequest request)
        {
            List<SortAttribute> list = new List<SortAttribute>();
            foreach (var sort in request.Sorts)
            {
                SortAttribute attribute = new SortAttribute()
                {
                    Member = sort.Member,
                    SortDirection = sort.SortDirection
                };
                list.Add(attribute);
            }
            return list;
        }

        private List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractFilterAttributes(DataSourceRequest request)
        {
            List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
            foreach (Kendo.Mvc.FilterDescriptor filter in request.Filters)
            {
                HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                {
                    Member = filter.Member,
                    MemberType = filter.MemberType,
                };
                switch (filter.Operator)
                {
                    case Kendo.Mvc.FilterOperator.Contains:
                        attribute.Operator = FILTEROPERATOR.Contains;
                        break;
                    case Kendo.Mvc.FilterOperator.StartsWith:
                        attribute.Operator = FILTEROPERATOR.StartWith;
                        break;
                    case Kendo.Mvc.FilterOperator.EndsWith:
                        attribute.Operator = FILTEROPERATOR.EndWith;
                        break;
                }
                list.Add(attribute);
            }
            return list;
        }
    }
}