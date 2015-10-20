using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Setting;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_ItemTrackingController : Controller
    {
        //
        // GET: /Sys_ItemTracking/
        public ActionResult Index()
        {
            return View();
        }

        public void SaveItemTracking(ItemTrackingModel model)
        {
            ItemTrackingInfo itemTrackingInfo = model.Copy<ItemTrackingInfo>();
            var itemTracking = new ItemTrackingManager();
            itemTracking.SettingPath = Common.GetPath(Constant.Settings);
            itemTracking.WriteFile(itemTrackingInfo);
        }
	}
}