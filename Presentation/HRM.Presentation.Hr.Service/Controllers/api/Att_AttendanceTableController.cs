using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Att_AttendanceTableController : ApiController
    {
        #region MyRegion
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Att_AttendanceTableModel> Get()
        //{
        //    ListQueryModel model = new ListQueryModel();

        //    var service = new Att_AttendanceTableServices();
        //    var list = service.GetAttendanceTable(model);

        //    return list.Select(item => new Att_AttendanceTableModel
        //    {
        //        Id = item.Id,
        //        ProfileID = item.ProfileID,
        //        ProfileName = item.ProfileName,
        //        CutOffDurationID = item.CutOffDurationID,
        //        CutOffDurationName = item.CutOffDurationName,
        //        Status = item.Status,
        //        StdWorkDayCount = item.StdWorkDayCount,
        //        RealWorkDayCount = item.RealWorkDayCount,
        //        PaidWorkDayCount = item.PaidWorkDayCount,
        //        HourPerDay = item.HourPerDay,
        //        NightShiftHours = item.NightShiftHours,
        //        OTNightShiftHours = item.OTNightShiftHours,
        //        AnlDayTaken = item.AnlDayTaken,
        //        AnlDayAvailable = item.AnlDayAvailable,
        //        DateStart = item.DateStart,
        //        DateEnd = item.DateEnd,
        //        LateEarlyDeductionHours = item.LateEarlyDeductionHours,
        //        UserRegister = item.UserRegister,
        //        DateRegister = item.DateRegister,
        //        UserApprove = item.UserApprove,
        //        DateApprove = item.DateApprove,
        //        Note = item.Note
        //    });
        //}

        //// GET api/<controller>/5
        //public Att_AttendanceTableModel Get(int id)
        //{
        //    var service = new Att_AttendanceTableServices();
        //    var repo = service.GetByProfileId(id);
        //    var result = new Att_AttendanceTableModel
        //    {
        //        Id = repo.Id,
        //        ProfileID = repo.ProfileID,
        //        CutOffDurationID = repo.CutOffDurationID,
        //        Status = repo.Status,
        //        StdWorkDayCount = repo.StdWorkDayCount,
        //        RealWorkDayCount = repo.RealWorkDayCount,
        //        PaidWorkDayCount = repo.PaidWorkDayCount,
        //        HourPerDay = repo.HourPerDay,
        //        NightShiftHours = repo.NightShiftHours,
        //        OTNightShiftHours = repo.OTNightShiftHours,
        //        AnlDayTaken = repo.AnlDayTaken,
        //        AnlDayAvailable = repo.AnlDayAvailable,
        //        DateStart = repo.DateStart,
        //        DateEnd = repo.DateEnd,
        //        LateEarlyDeductionHours = repo.LateEarlyDeductionHours,
        //        UserRegister = repo.UserRegister,
        //        DateRegister = repo.DateRegister,
        //        UserApprove = repo.UserApprove,
        //        DateApprove = repo.DateApprove,
        //        Note = repo.Note
        //    };
        //    return result;

        //}

        ///// <summary>
        ///// Lấy Tất cả dữ liệu theo ProfileId (do chưa xây dựng được hàm riêng)
        ///// </summary>
        ///// <param name="profileid"></param>
        ///// <returns></returns>
        //public Att_AttendanceTableModel Post(int id)
        //{
        //    var service = new Att_AttendanceTableServices();
        //    var repo = service.GetByProfileId(id);
        //    var result = new Att_AttendanceTableModel
        //    {
        //        Id = repo.Id,
        //        ProfileID = repo.ProfileID,
        //        CutOffDurationID = repo.CutOffDurationID,
        //        Status = repo.Status,
        //        StdWorkDayCount = repo.StdWorkDayCount,
        //        RealWorkDayCount = repo.RealWorkDayCount,
        //        PaidWorkDayCount = repo.PaidWorkDayCount,
        //        HourPerDay = repo.HourPerDay,
        //        NightShiftHours = repo.NightShiftHours,
        //        OTNightShiftHours = repo.OTNightShiftHours,
        //        AnlDayTaken = repo.AnlDayTaken,
        //        AnlDayAvailable = repo.AnlDayAvailable,
        //        DateStart = repo.DateStart,
        //        DateEnd = repo.DateEnd,
        //        LateEarlyDeductionHours = repo.LateEarlyDeductionHours,
        //        UserRegister = repo.UserRegister,
        //        DateRegister = repo.DateRegister,
        //        UserApprove = repo.UserApprove,
        //        DateApprove = repo.DateApprove,
        //        Note = repo.Note
        //    };
        //    return result;
        //}

        //public Att_AttendanceTableModel Put(ProfileIdAndCutOffIdModel model)
        //{
        //    var service = new Att_AttendanceTableServices();
        //    var listEntity = new List<Att_AttendanceTable>();
        //    var entity = service.GetAttTableByCutOffAndProfileId(model.ProfileId, model.CutOffId);
        //    listEntity.Add(entity);

        //    var resultModel = listEntity.Translate<Att_AttendanceTableModel>();
        //    return resultModel[0];
        //}

        ////public Att_AttendanceTableModel Put(Att_AttendanceTableModel model)
        ////{
        ////    var attWorkDay = new Att_AttendanceTable
        ////    {
        ////        Id = model.Id,
        ////        ProfileID = model.ProfileID,
        ////        CutOffDurationID = model.CutOffDurationID,
        ////        Status = model.Status,
        ////        StdWorkDayCount = model.StdWorkDayCount,
        ////        RealWorkDayCount = model.RealWorkDayCount,
        ////        PaidWorkDayCount = model.PaidWorkDayCount,
        ////        HourPerDay = model.HourPerDay,
        ////        NightShiftHours = model.NightShiftHours,
        ////        OTNightShiftHours = model.OTNightShiftHours,
        ////        AnlDayTaken = model.AnlDayTaken,
        ////        AnlDayAvailable = model.AnlDayAvailable,
        ////        DateStart = model.DateStart,
        ////        DateEnd = model.DateEnd,
        ////        LateEarlyDeductionHours = model.LateEarlyDeductionHours,
        ////        UserRegister = model.UserRegister,
        ////        DateRegister = model.DateRegister,
        ////        UserApprove = model.UserApprove,
        ////        DateApprove = model.DateApprove,
        ////        Note = model.Note
        ////    };
        ////    var service = new Att_AttendanceTableServices();
        ////    if (model.Id != 0)
        ////    {
        ////        attWorkDay.Id = model.Id;
        ////        service.Edit(attWorkDay);
        ////    }
        ////    else
        ////    {
        ////        service.Add(attWorkDay);
        ////    }

        ////    return model;
        ////}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //    var service = new Att_AttendanceTableServices();
        //    var result = service.Delete(id);
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
        public Att_AttendanceTableModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_AttendanceTableModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_AttendanceTableEntity>(id, ConstantSql.hrm_att_sp_get_AttendanceTableById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_AttendanceTableModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_AttendanceTableModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_AttendanceTableEntity, Att_AttendanceTableModel>(id);
            return result;
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_AttendanceTable")]
        public Att_AttendanceTableModel Post([Bind]Att_AttendanceTableModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_AttendanceTableEntity, Att_AttendanceTableModel>(model);
        }
    }
}