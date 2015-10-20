using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_OvertimeConfirmController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Prefix = "models")]List<Att_OvertimeModel> attOvertimes)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            if (attOvertimes != null)
            {
                for (int i = 0; i < attOvertimes.Count(); i++)
                {
                    #region [Hien.Nguyen] xử lý gộp time vào datetime
                    //Nếu date == null
                    if (attOvertimes[i].InTime == null)
                    {
                        attOvertimes[i].InTime = attOvertimes[i].WorkDate;//gán bằng với workdate
                        if (attOvertimes[i].TempTimeIn != null)//Nếu giờ khác null thì gán giờ vào
                        {
                            attOvertimes[i].InTime = new DateTime(attOvertimes[i].InTime.Value.Year, attOvertimes[i].InTime.Value.Month,
                                attOvertimes[i].InTime.Value.Day, int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[0]),
                                int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[2]));
                        }
                    }
                    else//Nếu date != null 
                    {
                        if (attOvertimes[i].TempTimeIn != null)//Nếu time khác null thì gán time vào cho date
                        {
                            attOvertimes[i].InTime = new DateTime(attOvertimes[i].InTime.Value.Year, attOvertimes[i].InTime.Value.Month,
                               attOvertimes[i].InTime.Value.Day, int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[0]),
                               int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[1]), int.Parse(attOvertimes[i].TempTimeIn.Split(':').ToList()[2]));
                        }
                    }
                    if (attOvertimes[i].OutTime == null)
                    {
                        attOvertimes[i].OutTime = attOvertimes[i].WorkDate;//gán bằng với workdate
                        if (attOvertimes[i].TempTimeOut != null)//Nếu giờ khác null thì gán giờ vào
                        {
                            attOvertimes[i].OutTime = new DateTime(attOvertimes[i].OutTime.Value.Year, attOvertimes[i].OutTime.Value.Month,
                                attOvertimes[i].OutTime.Value.Day, int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[0]),
                                 int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[2]));
                        }
                    }
                    else
                    {
                        if (attOvertimes[i].TempTimeOut != null)//Nếu time khác null thì gán time vào cho date
                        {
                            attOvertimes[i].OutTime = new DateTime(attOvertimes[i].OutTime.Value.Year, attOvertimes[i].OutTime.Value.Month,
                               attOvertimes[i].OutTime.Value.Day, int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[0]),
                               int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[1]), int.Parse(attOvertimes[i].TempTimeOut.Split(':').ToList()[2]));
                        }
                    }
                    #endregion
                }
                    service.Post(_Hrm_Hre_Service, "api/Att_OvertimeConfirm/", attOvertimes);                
            }              
            
            return Json(attOvertimes);
        }
	}
}