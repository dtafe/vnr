using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.Canteen.Domain;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Business.Canteen.Models;
using ListQueryModel = HRM.Infrastructure.Utilities.ListQueryModel;
using System;
using HRM.Business.Hr.Domain;
using System.Data.SqlTypes;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
 

namespace HRM.Presentation.Canteen.Service.Controllers
{
    public class Canteen_ActionDataController : HrmMvcController
    {
        //SonVo - 20140803 - chuyển trạng thái submit màn hình Dữ liệu quên quẹt thẻ
        public ActionResult SubmitMealRecordMissing(string selectedIds, string status)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => Guid.Parse(x))
                   .ToList();
            }
            var lstMealRecordMissing = new List<Can_MealRecordMissingEntity>();
            var services = new Can_MealRecordMissingServices();
            services.SubmitStatus(ids, status);
            return Json("");
        }

        //SonVo - 20140803 - chuyển trạng thái submit màn hình phụ cấp ăn
        public ActionResult SubmitMealAllowance(string selectedIds, string status)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => Guid.Parse(x))
                   .ToList();
            }
            var lstMealRecordMissing = new List<Can_MealAllowanceToMoneyEntity>();
            var services = new Can_MealAllowanceToMoneyServices();
            services.SubmitStatus(ids, status);
            return Json("");
        }
    }
}