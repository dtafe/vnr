using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Training.Models;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Tra_TrainerMailReminderController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TraTrainerMailReminderInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_TrainerMailReminderModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_TrainerMailReminder/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
     
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_TrainerMailReminderModel>(_hrm_Hr_Service, "api/Tra_TrainerMailReminder/", selectedIds, ConstantPermission.Tra_TrainerMailReminder, DeleteType.Remove.ToString());
        }
    }
}