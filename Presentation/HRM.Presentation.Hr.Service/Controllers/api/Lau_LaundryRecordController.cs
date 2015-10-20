
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Laundry.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Business.Laundry.Models;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Lau_LaundryRecordController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Tủ Đồ(Lau_Locker) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LaundryRecordModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Lau_LaundryRecordModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<LMS_LaundryRecordEntity>(id, ConstantSql.hrm_lau_sp_get_LaundryRecord_byId, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Lau_LaundryRecordModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tủ Đồ(Lau_Locker) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lau_LaundryRecordModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<LMS_LaundryRecordEntity, Lau_LaundryRecordModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Tủ Đồ(Lau_Locker)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/LaundryRecord")]
        public Lau_LaundryRecordModel Post([Bind]Lau_LaundryRecordModel model)
        {
            model.Type = LaundryRecordType.E_MANUAL.ToString();
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<LMS_LaundryRecordEntity, Lau_LaundryRecordModel>(model);
        }

        public Lau_LaundryRecordModel Put([Bind]Lau_LaundryRecordModel model)
        {
            model.ProfileIDs = Common.DotNetToOracle(model.ProfileIDs);
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.ProfileIDs);
            lstObj.Add(model.Status);
            var rs = service.UpdateData<Lau_LaundryRecordModel>(lstObj, ConstantSql.hrm_att_sp_get_LaundryRecord_UpdateStatus, ref status);
            if (rs != null)
            {
                return model;
            }
            return null;
        }
       
    }
}