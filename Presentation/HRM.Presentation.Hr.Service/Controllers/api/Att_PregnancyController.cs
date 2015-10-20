using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
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

    public class Att_PregnancyController : ApiController
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
        /// [Chuc.Nguyen] - Lấy dữ liệu bảng Thai Sản(Att_Pregnancy) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_PregnancyModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_PregnancyModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_PregnancyEntity>(id, ConstantSql.hrm_att_sp_get_PregnancyById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_PregnancyModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của bảng Thai Sản(Att_Pregnancy) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_PregnancyModel DeleteOrRemove(Guid id)
        {
            //var service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Att_PregnancyEntity, Att_PregnancyModel>(id);
            //return result;
            Att_PregnancyServices service = new Att_PregnancyServices();
            var result = service.Remove<Att_PregnancyEntity>(id);
            //return result;
            return new Att_PregnancyModel();
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một bảng Thai Sản(Att_Pregnancy)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_Pregnancy")]
        public Att_PregnancyModel Post([Bind]Att_PregnancyModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_PregnancyModel>(model, "Att_Pregnancy", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion

            var service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_PregnancyEntity, Att_PregnancyModel>(model);
        }

    }
}