using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Business.Main.Domain;
using HRM.Business.Attendance.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_LeavedayCustomController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Leaveday")]
        public Att_LeaveDayModel Post([Bind]Att_LeaveDayModel model)
        {
            Att_LeavedayServices services = new Att_LeavedayServices();
            model.ActionStatus = services.UpdateTotalDuration(model.lstLeaveIDs);
            return model;
        }
        
      
        
    }
}