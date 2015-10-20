using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Presentation.Service;
using HRM.Presentation.Attendance.Models;
using VnResource.Helper.Data;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Att_ActionDataController : HrmMvcController
    {
        //SonVo - 20140617 - chuyển thành trạng thái Sumit 
        public ActionResult SubmitWorkDay(string selectedIds)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => new Guid(x))
                   .ToList();
            }
            Att_WorkDayServices service = new Att_WorkDayServices();
            service.SubmitWorkDay(ids);

            return Json("");
        }

        //SonVo - 20140617 - chuyển thành trạng thái Sumit
        public ActionResult SubmitOvertime(string selectedIds)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => new Guid(x))
                   .ToList();
            }
            Att_OvertimeServices service = new Att_OvertimeServices();
            service.SubmitOvertime(ids);
            return Json("");
        }



        //SonVo - 20140617 - chuyển thành trạng thái Sumit
        public ActionResult SubmitRoster(string selectedIds)
        {
            List<Guid> ids = new List<Guid>();
            if (selectedIds != null)
            {
                ids = selectedIds
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => new Guid(x))
                   .ToList();
            }
            Att_RosterServices service = new Att_RosterServices();
            service.SubmitRoster(ids);
            return Json("");
        }

        


       
	}
}