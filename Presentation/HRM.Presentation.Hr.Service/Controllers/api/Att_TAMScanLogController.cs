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
    public class Att_TAMScanLogController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Lịch Sử Thẻ(Att_TAMScanLog) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_TAMScanLogModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_TAMScanLogModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_TAMScanLogEntity>(id, ConstantSql.hrm_att_sp_get_TAMScanLogById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_TAMScanLogModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Lịch Sử Thẻ(Att_TAMScanLog) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_TAMScanLogModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_TAMScanLogEntity, Att_TAMScanLogModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Lịch Sử Thẻ(Att_TAMScanLog)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_TAMScanLog")]
        public Att_TAMScanLogModel Post([Bind]Att_TAMScanLogModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_TAMScanLogModel>(model, "Att_TAMScanLog", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_TAMScanLogEntity, Att_TAMScanLogModel>(model);
        }
	}
}