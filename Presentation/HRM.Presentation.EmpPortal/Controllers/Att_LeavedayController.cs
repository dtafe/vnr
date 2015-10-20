using HRM.Business.Attendance.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.EmpPortal.Controllers
{
    public class Att_LeavedayController : BasePortalController
    {
        //
        // GET: /Att_Leaveday/
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return GetView();
        }
        public ActionResult LeavedayList()
        {
            return GetView();
        }

        public ActionResult Approve()
        {
            return GetView();
        }

        public ActionResult Submit()
        {
            return GetView();
        }

        public ActionResult Edit(Guid id)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
            string status = string.Empty;
            var model = new Att_LeaveDayModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_LeaveDayEntity>(id, ConstantSql.hrm_att_sp_get_LeaveDayById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_LeaveDayModel>();
            } 
            model.ActionStatus = status;
            return GetOnlyView(model);
        }
        private static Guid IdLeaveday;
        public string Save(Att_LeaveDayModel model)
        {
            if (IdLeaveday != Guid.Empty)
            {
                model.ID = IdLeaveday;
            }
            if (model.DurationType =="E_FULLSHIFT" )
            {
                model.LeaveHours = 8;
            }
            ActionService service = new ActionService(UserLogin);
            service.UpdateOrCreate<Att_LeaveDayEntity, Att_LeaveDayModel>(model);
            //IdLeaveday = model.ID;
            //if (model.ID != Guid.Empty)
            //{
            //    return model.ID.ToString();
            //}
            return model.ActionStatus;
        }


        public void SetStatusSelected(string selectedIds, string status)
        {
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = Common.DotNetToOracle(selectedIds);
                BaseService service = new BaseService();
                string statusMessages = string.Empty;
                List<object> lstObj = new List<object>();
                lstObj.Add(selectedIds);
                lstObj.Add(status);
                service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, ref statusMessages);
            }
        }

        public void SetStatusSelectedWithReason(string selectedIds, string status, string reason)
        {
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = Common.DotNetToOracle(selectedIds);
                BaseService service = new BaseService();
                string statusMessages = string.Empty;
                List<object> lstObj = new List<object>();
                lstObj.Add(selectedIds);
                lstObj.Add(status);
                lstObj.Add(reason);
                service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_LeavedayPortal_UpdateStatus, ref statusMessages);
            }
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds.Count >= 0)
            {
                BaseService service = new BaseService();
                var status = string.Empty;
                for (int i = 0; i < selectedIds.Count; i++)
                {
                    status = service.Remove<Att_LeaveDayEntity>(selectedIds[i]);
                    if (status.StartsWith("Error"))
                    {
                        break;
                    }
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        public ActionResult Create(Att_LeaveDayModel model)
        {
            var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Att_Leaveday/", model);
            return Json(result);
        }

	}
}