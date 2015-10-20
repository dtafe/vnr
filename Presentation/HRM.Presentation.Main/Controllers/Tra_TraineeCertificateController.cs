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
    public class Tra_TraineeCertificateController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_TraineeModel>(_hrm_Hr_Service, "api/Tra_TraineeCertificate/", selectedIds, ConstantPermission.Tra_Trainee, DeleteType.Remove.ToString());
        }

    }
}