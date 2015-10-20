using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
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

    public class Att_AttendanceTableItemItemController : ApiController
    {
        #region MyRegion
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Att_AttendanceTableItemItemModel> Get()
        //{
        //    var service = new Att_AttendanceTableItemItemServices();
        //    var list = service.GetAttendanceTableItemItem();

        //    return list.Select(item => new Att_AttendanceTableItemItemModel
        //    {
        //        Id = item.Id,
        //        AttendanceTableItemID = item.AttendanceTableItemID,
        //        WorkDate = item.WorkDate,
        //        AvailableHours = item.AvailableHours,
        //        ShiftID = item.ShiftID,
        //        ShiftName = item.ShiftName,
        //        WorkHours = item.WorkHours
        //    });
        //}

        //// GET api/<controller>/5
        //public Att_AttendanceTableItemItemModel Get(int id)
        //{
        //    var service = new Att_AttendanceTableItemItemServices();
        //    var result = service.Get(id);
        //    var AttWorkDay = new Att_AttendanceTableItemItemModel
        //    {
        //        Id = result.Id,
        //        AttendanceTableItemID = result.AttendanceTableItemID,
        //        WorkDate = result.WorkDate,
        //        AvailableHours = result.AvailableHours,
        //        ShiftID = result.ShiftID,
        //        WorkHours = result.WorkHours
        //    };
        //    return AttWorkDay;
        //}

        ////public IEnumerable<Att_AttendanceTableItemItemModel> GetByAttendanceTableItemId(int id)
        ////{
        ////    var service = new Att_AttendanceTableItemItemServices();
        ////    var result = service.GetByAttendanceTableItemId(id);
        ////    var listModel = result.Translate<Att_AttendanceTableItemItemModel>();
        ////    return listModel;
        ////}

        //// POST api/<controller>
        //public IEnumerable<Att_AttendanceTableItemItemModel> Post([FromBody] int id)
        //{
        //    var service = new Att_AttendanceTableItemItemServices();
        //    var result = service.GetByAttendanceTableItemId(id);
        //    var listModel = result.Translate<Att_AttendanceTableItemItemModel>();
        //    return listModel;
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{

        //}


        //public Att_AttendanceTableItemItemModel Put(Att_AttendanceTableItemItemModel model)
        //{
        //    var AttWorkDay = new Att_AttendanceTableItemItem
        //    {
        //        Id = model.Id,
        //        AttendanceTableItemID = model.AttendanceTableItemID,
        //        WorkDate = model.WorkDate,
        //        AvailableHours = model.AvailableHours,
        //        ShiftID = model.ShiftID,
        //        WorkHours = model.WorkHours
        //    };
        //    var service = new Att_AttendanceTableItemItemServices();
        //    if (model.Id != 0)
        //    {
        //        AttWorkDay.Id = model.Id;
        //        service.Edit(AttWorkDay);
        //    }
        //    else
        //    {
        //        service.Add(AttWorkDay);
        //    }

        //    return model;
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //    var service = new Att_AttendanceTableItemItemServices();
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
        public Att_AttendanceTableItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_AttendanceTableItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_AttendanceTableItemEntity>(id, ConstantSql.hrm_att_sp_get_AttendanceTableItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_AttendanceTableItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_AttendanceTableItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_AttendanceTableItemEntity, Att_AttendanceTableItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_AttendanceTableItem")]
        public Att_AttendanceTableItemModel Post([Bind]Att_AttendanceTableItemModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_AttendanceTableItemEntity, Att_AttendanceTableItemModel>(model);
        }

    }
}