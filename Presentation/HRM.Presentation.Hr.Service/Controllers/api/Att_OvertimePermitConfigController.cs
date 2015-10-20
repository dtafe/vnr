using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Presentation.Attendance.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
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
    public class Att_OvertimePermitConfigController : ApiController
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
        /// [Kiet.Chung] - Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_OvertimePermitModel GetById(Guid id)
        {
            var model = new Att_OvertimePermitModel();
            Att_OvertimePermitConfigServices service = new Att_OvertimePermitConfigServices();
            var entity = service.GetOTPermit(UserLogin);
            if (entity != null)
            {
                model = entity.CopyData<Att_OvertimePermitModel>();
            }
            return model;
        }
        /// <summary>
        /// [Hieu.Van] - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_OvertimePermitConfig")]
        public String Post([Bind]Att_OvertimePermitModel model)
        {
            string message = string.Empty;
            Att_OvertimePermitConfigServices service = new Att_OvertimePermitConfigServices();
            var entity = model.CopyData<OvertimePermitEntity>();
            service.SaveOvertimePermitConfig(entity,UserLogin );
            return message;
        }

    }
}