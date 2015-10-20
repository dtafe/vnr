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

    public class Att_RosterGroupController : ApiController
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
        /// [Hieu.van - Lấy dữ liệu RosterGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_RosterGroupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_RosterGroupModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_RosterGroupEntity>(id, ConstantSql.hrm_att_sp_get_RosterGroupById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_RosterGroupModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Att_RosterGroup (Att_RosterGroup) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_RosterGroupModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_RosterGroupEntity, Att_RosterGroupModel>(id);
            return result;
            //Att_RosterGroupServices service = new Att_RosterGroupServices();
            //var result = service.Remove<Att_RosterGroupEntity>(id);
            //return new Att_RosterGroupModel();
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Att_RosterGroup)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_RosterGroup")]
        public Att_RosterGroupModel Post([Bind]Att_RosterGroupModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_RosterGroupModel>(model, "Att_RosterGroup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_RosterGroupEntity, Att_RosterGroupModel>(model);
        }


        /// <summary>
        /// [Hiau.Van] Update status 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Roster")]
        public Att_RosterGroupModel Put([Bind]Att_RosterGroupModel model)
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(model.ProfileIds);
            lstObj.Add(model.Status);
            var rs = service.UpdateData<Att_RosterGroupModel>(lstObj, ConstantSql.hrm_att_sp_Set_RosterGroup_Status, ref status);
            if (rs != null)
            {
                return model;
            }
            return null;
        }
    }
}