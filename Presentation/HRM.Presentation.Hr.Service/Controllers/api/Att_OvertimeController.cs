using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
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
using HRM.Presentation.HrmSystem.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Domain;
using System.Reflection;
using System.Collections;
using System.Data.SqlTypes;
using HRM.Business.Attendance.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
//
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.HrmSystem.Domain;
using System.Threading.Tasks;
using HRM.Presentation.Category.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_OvertimeController : ApiController
    {

        #region MyRegion
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Att_OvertimeModel> Get()
        //{
        //    var service = new Att_OvertimeServices();
        //    ListQueryModel model = new ListQueryModel();
        //    var list = service.Gets(model);            
        //    return list.Select(item => new Att_OvertimeModel
        //    {
        //        Id = item.Id,               
        //        ProfileID = item.ProfileID,               
        //        Status = item.Status,
        //        WorkDate = item.WorkDate,              
        //        RegisterHours = item.RegisterHours,
        //        MethodPayment = item.MethodPayment,
        //        ReasonOT = item.ReasonOT,            
        //        ShiftID = item.ShiftID,               
        //        OvertimeTypeID = item.OvertimeTypeID,
        //        DateUpdate = item.DateUpdate,
        //        UserApproveID1 = item.UserApproveID1,
        //        UserApproveID2 = item.UserApproveID2              
        //    });
        //}

        //// GET api/<controller>/5
        //public Att_OvertimeModel Get(int id)
        //{
        //    var service = new Att_OvertimeServices();
        //    var result = service.GetOvertimeById(id);
        //    var AttOvertime = new Att_OvertimeModel
        //    {
        //        Id = result.Id,
        //        ProfileID = result.ProfileID,            
        //        Status = result.Status,
        //        WorkDate = result.WorkDate,              
        //        RegisterHours = result.RegisterHours,
        //        MethodPayment = result.MethodPayment,
        //        ReasonOT = result.ReasonOT,              
        //        ShiftID = result.ShiftID,             
        //        OvertimeTypeID = result.OvertimeTypeID,
        //        DateUpdate = result.DateUpdate,
        //        UserApproveID1 = result.UserApproveID1,
        //        UserApproveID2 = result.UserApproveID2                
        //    };
        //    return AttOvertime;
        //}
        //// Analysis
        //public Att_OvertimeModel Put(Att_OvertimeModel model)
        //{
        //    var AttOvertime = new Att_Overtime
        //    {
        //        Id = model.Id,
        //        ProfileID = model.ProfileID,
        //        Status = model.Status,
        //        WorkDate = model.WorkDate,
        //        RegisterHours = model.RegisterHours,
        //        MethodPayment = model.MethodPayment,
        //        ReasonOT = model.ReasonOT,
        //        ShiftID = model.ShiftID,
        //        OvertimeTypeID = model.OvertimeTypeID,
        //        DateUpdate = model.DateUpdate,
        //        UserApproveID1 = model.UserApproveID1,
        //        UserApproveID2 = model.UserApproveID2
        //    };
        //    var service = new Att_OvertimeServices();
        //    if (model.Id != 0)
        //    {
        //        AttOvertime.Id = model.Id;
        //        service.Edit(AttOvertime);
        //    }
        //    else
        //    {
        //        service.Add(AttOvertime);
        //    }
        //    return model;
        //}

        //// DELETE api/<controller>/5
        //public Att_OvertimeModel Delete(int id)
        //{
        //    var service = new Att_OvertimeServices();
        //    service.Remove(id);
        //    var result = new Att_OvertimeModel();
        //    return result;
        //}
        #endregion       

        #region UserLogin
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
        /// [Hieu.Van] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_OvertimeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_OvertimeModel();
            ActionService service = new ActionService(userLogin);
            var entity = service.GetByIdUseStore<Att_OvertimeEntity>(id, ConstantSql.hrm_att_sp_get_OvertimeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_OvertimeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_OvertimeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_OvertimeEntity, Att_OvertimeModel>(id);
            return result;
            //Att_OvertimeServices service = new Att_OvertimeServices();
            //var result = service.Remove<Att_OvertimeEntity>(id);
            //return new Att_OvertimeModel();
			
        }


        /// <summary>
        /// [Hieu.Van] Update status 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Att_Overtime")]
        public Att_OvertimeModel Put([Bind]Att_OvertimeModel model)
        {
            model.ProfileIds = Common.DotNetToOracle(model.ProfileIds);
            BaseService service = new BaseService();
            string status = string.Empty;
           
            List<object> lstObj = new List<object>();
            if (string.IsNullOrEmpty(model.Status) && string.IsNullOrEmpty(model.MethodPayment))
            {
                lstObj.AddRange(new object[2]);
                lstObj[0] = model.ProfileIds;
                lstObj[1] = model.IsNonOvertime;
                var rs = new List<Att_OvertimeModel>();
                rs = service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.Att_sp_Set_Overtime_AllowOvertime, ref status);
                model.ActionStatus = status;
                return model;
            }
            else
            {
                lstObj.AddRange(new object[3]);
                lstObj[0] = model.ProfileIds;
                lstObj[1] = model.Status;
                lstObj[2] = model.MethodPayment;
                var rs = new List<Att_OvertimeModel>();
                if (model.MethodPayment == "E_CASHOUT" || model.MethodPayment == "E_TIMEOFF")
                {
                    #region Change MethodPayment
                    rs = service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Payment, ref status);
                    model.ActionStatus = status;
                    #endregion
                }
                else
                {
                    #region  không cho phép duyệt ot cho bản thân 
                    var checkConfigApp = service.GetData<Sys_AllSettingModel>(AppConfig.HRM_ATT_OT_DONOTAPPROVEOTMYSELF.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status);
                    var checkUserLG = service.GetData<Sys_UserModel>(model.UserApproveID, ConstantSql.hrm_sys_sp_get_UserbyId,UserLogin, ref status);
                    bool APPROVEOTMYSELF = false;

                    Guid? ProIDofUserLG = checkUserLG.Select(s => s.ProfileID).FirstOrDefault();
                    if (ProIDofUserLG != null && ProIDofUserLG != Guid.Empty)
                    {
                        if (checkConfigApp != null && checkConfigApp.Select(s => s.Value1).FirstOrDefault() == bool.TrueString)
                        {
                            APPROVEOTMYSELF = true;
                        }
                    }
                    #endregion

                    #region Change Status
                    var lstCheck = service.GetData<Att_RosterModel>(model.ProfileIds, ConstantSql.hrm_att_sp_get_OvertimeByIds,UserLogin, ref status);
                    if (status == NotificationType.Success.ToString())
                        status = "";
                    else
                    {
                        model.ActionStatus = status;
                        return model;
                    }
                    string lstFirstApprove = "";
                    string lstNormal = "";
                    foreach (var item in lstCheck)
                    {
                        if (APPROVEOTMYSELF == true && ProIDofUserLG == item.ProfileID)
                        {
                            model.ActionStatus = "NoApproveOTMySelf";
                            return model;
                        }
                        if (item.UserApproveID != model.UserApproveID && item.UserApproveID2 != model.UserApproveID2)
                        {
                            model.ActionStatus = "NoPermission";
                            return model;
                        }
                        if (model.Status == AttendanceDataStatus.E_APPROVED.ToString())
                        {
                            if (item.UserApproveID == model.UserApproveID)
                                lstFirstApprove += Common.DotNetToOracle(item.ID.ToString()) + ",";
                            if (item.UserApproveID2 == model.UserApproveID2 && (item.Status == AttendanceDataStatus.E_FIRST_APPROVED.ToString() || item.UserApproveID == model.UserApproveID))
                                lstNormal += Common.DotNetToOracle(item.ID.ToString()) + ",";
                        }
                        else
                            if (model.Status == AttendanceDataStatus.E_WAIT_APPROVED.ToString())
                            {
                                if (item.UserApproveID == model.UserApproveID)
                                    lstNormal += Common.DotNetToOracle(item.ID.ToString()) + ",";
                            }
                            else
                                lstNormal += Common.DotNetToOracle(item.ID.ToString()) + ",";

                        //if (item.UserApproveID2 == model.UserApproveID2 && (item.Status == AttendanceDataStatus.E_FIRST_APPROVED.ToString() || item.UserApproveID == model.UserApproveID))
                        //{
                        //    lstFirstApprove += Common.DotNetToOracle(item.ID.ToString()) + ",";
                        //}
                        //else
                        //{
                        //    if (item.UserApproveID == model.UserApproveID)
                        //        lstNormal += Common.DotNetToOracle(item.ID.ToString()) + ",";
                        //}
                    }
                    if (lstFirstApprove != "")
                    {
                        lstFirstApprove = lstFirstApprove.Substring(0, lstFirstApprove.Length - 1);
                        lstObj[0] = lstFirstApprove;
                        lstObj[1] = AttendanceDataStatus.E_FIRST_APPROVED.ToString();
                        service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Status, ref status);
                    }
                    if (lstNormal != "")
                    {
                        lstNormal = lstNormal.Substring(0, lstNormal.Length - 1);
                        lstObj[0] = lstNormal;
                        lstObj[1] = model.Status;
                        service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Status, ref status);
                    }
                    //rs = service.UpdateData<Att_OvertimeModel>(lstObj, ConstantSql.hrm_att_sp_Set_Overtime_Status, ref status);
                    if (status == "")
                        status = "NoPermission";
                    model.ActionStatus = status;
                    #endregion
                }
            }
            return model;
        }
        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Overtime")]
        public Att_OvertimeModel Post([Bind]List<Att_OvertimeModel> lstModel)
        {
            ActionService service = new ActionService(UserLogin);
            Att_ProcessApprovedServices smService = new Att_ProcessApprovedServices();
            List<Att_OvertimeEntity> lstSM = new List<Att_OvertimeEntity>();
            Att_OvertimeModel result = new Att_OvertimeModel();

            foreach (var model in lstModel)
            {
                if (model.WorkDateTime != null && model.WorkDateTime.Length < 8)
                {
                    model.WorkDateTime = model.WorkDateTime + ":00";
                }

                //[Hien.Nguyen] reset hours datetime
                //model.WorkDate = new DateTime(model.WorkDate.Year, model.WorkDate.Month, model.WorkDate.Day, 12, 0, 0);
                //model.WorkDate = Common.JoinTimeInDate(model.WorkDate, model.WorkDateTime);

                model.WorkDateRoot = model.WorkDate;
                if (model.Status == EnumDropDown.OverTimeStatus.E_APPROVED.ToString()
                && model.RegisterHours != model.ApproveHours)
                {
                    model.ApproveHours = model.RegisterHours;
                }
                result = service.UpdateOrCreate<Att_OvertimeEntity, Att_OvertimeModel>(model);
                if (result.ActionStatus == "Success")
                {
                    lstSM.Add(model.Copy<Att_OvertimeEntity>());
                }
            }
            smService.GetEmailToSend_Overtime(lstSM,UserLogin);
            return result;
        }
    }
}