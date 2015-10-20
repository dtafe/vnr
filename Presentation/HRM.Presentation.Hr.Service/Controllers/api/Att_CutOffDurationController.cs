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
    public class Att_CutOffDurationController : ApiController
    {
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
        public Att_CutOffDurationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_CutOffDurationModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetById<Att_CutOffDurationEntity>(id, ref status);
            var entity = service.GetByIdUseStore<Att_CutOffDurationEntity>(id, ConstantSql.hrm_att_sp_get_CutOffDurationById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_CutOffDurationModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_CutOffDurationModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_CutOffDurationEntity, Att_CutOffDurationModel>(id);
            return result;
            //Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            //var result = service.Remove<Att_CutOffDurationEntity>(id);
            //return new Att_CutOffDurationModel();
			
        }

        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_CutOffDuration")]
        public Att_CutOffDurationModel Post([Bind]Att_CutOffDurationModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_CutOffDurationModel>(model, "Att_CutOffDuration", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.DateEnd != null)
                model.DateEnd = model.DateEnd.AddDays(1).AddMilliseconds(-10);
            if (model.OvertimeEnd != null)
                model.OvertimeEnd = model.OvertimeEnd.Value.AddDays(1).AddMilliseconds(-10);
            if (model.LeavedayEnd != null)
                model.LeavedayEnd = model.LeavedayEnd.Value.AddDays(1).AddMilliseconds(-10);

            //message = string.Format(ConstantMessages.FieldNotAllowNull.Translate2(), ("ProfileID").Translate2());
            //model.ActionStatus = message;
         //   return model;
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_CutOffDurationEntity, Att_CutOffDurationModel>(model);
        }
	}
}