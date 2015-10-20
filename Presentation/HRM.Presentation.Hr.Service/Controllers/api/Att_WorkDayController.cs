using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Main.Domain;
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

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_WorkDayController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu Lịch Sử Thẻ(Att_WorkDay) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_WorkdayModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_WorkdayModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_WorkdayEntity>(id, ConstantSql.hrm_att_sp_get_WorkDayById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_WorkdayModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Lịch Sử Thẻ(Att_WorkDay) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_WorkdayModel DeleteOrRemove(Guid id)
        {
            //ActionService service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Att_WorkdayEntity, Att_WorkdayModel>(id);
            //return result;
            Att_WorkDayServices service = new Att_WorkDayServices();
            var result = service.Remove<Att_WorkdayEntity>(id);
            //return result;
            return new Att_WorkdayModel();
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Lịch Sử Thẻ(Att_WorkDay)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_WorkDay")]
        public Att_WorkdayModel Post([Bind]Att_WorkdayModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_WorkdayEntity, Att_WorkdayModel>(model);
        }

        /// <summary>
        /// [Hieu.Van] Update status 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Att_WorkdayModel Put(Att_WorkdayModel model)
        {
            model.ProfileIds = Common.DotNetToOracle(model.ProfileIds);
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.ProfileIds);
            List<Att_WorkdayModel> rs = null;
            if (model.Status == "UpdateInOut")
            {
                rs = service.UpdateData<Att_WorkdayModel>(lstObj, ConstantSql.hrm_att_sp_set_WorkDay_addInOut, ref status);
            }
            if (model.Status == "ChangeInOut")
            {
                rs = service.UpdateData<Att_WorkdayModel>(lstObj, ConstantSql.hrm_att_sp_set_WorkDay_ChangeInOut, ref status);
            }
            if (model.Status == "CancelLateEarly")
            {
                lstObj.Add(0);
                rs = service.UpdateData<Att_WorkdayModel>(lstObj, ConstantSql.hrm_att_sp_set_WorkDay_LateEarlyDuration, ref status);
            }
            if (model.Status == AttendanceDataStatus.E_WAIT_APPROVED.ToString() || model.Status == AttendanceDataStatus.E_APPROVED.ToString())
            {
                lstObj.Add(model.Status);
                rs = service.UpdateData<Att_WorkdayModel>(lstObj, ConstantSql.hrm_att_sp_get_WorkDay_UpdateStatus, ref status);
            }
            model.ActionStatus = status;
            return model;
            //if (rs != null)
            //{
            //    return model;
            //}
            //return null;
        }
    }
}