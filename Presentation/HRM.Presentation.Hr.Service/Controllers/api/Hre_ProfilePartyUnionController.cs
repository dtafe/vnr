using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_ProfilePartyUnionController : ApiController
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
        /// [Tho.Bui] - Lấy dữ liệu ProfilePartyUnion theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfilePartyUnionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_ProfilePartyUnionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_ProfilePartyUnionEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfilePartyUnionId, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_ProfilePartyUnionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui] - Lấy dữ liệu Nhân Viên (Hre_ProfilePartyUnion) theo ProfileId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfilePartyUnionModel Put(Hre_ProfilePartyUnionModel model)
        {
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            List<object> listObj = new List<object>() { model.ProfileID };
            var entity = service.GetData<Hre_ProfilePartyUnionEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfilePartyUnionprofileId, UserLogin, ref status).FirstOrDefault();
            var model1 = entity.CopyData<Hre_ProfilePartyUnionModel>();
            model.ActionStatus = status;
            return model1;
        }
        //public Hre_ProfilePartyUnionModel Put(Hre_ProfilePartyUnionModel model)
        //{
        //    string status = string.Empty;
        //    ActionService service = new ActionService(UserLogin);
        //    var entity = service.GetData<Hre_ProfilePartyUnionEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfilePartyUnionprofileId, ref status).FirstOrDefault();
        //    if (entity != null)
        //    {
        //        model = entity.CopyData<Hre_ProfilePartyUnionModel>();
        //    }
        //    model.ActionStatus = status;
        //    return model;
        //}
        /// <summary>
        /// [Tho.Bui] - Xử lý thêm mới hoặc chỉnh sửa một ProfilePartyUnion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_ProfilePartyUnion")]
        public Hre_ProfilePartyUnionModel Post([Bind]Hre_ProfilePartyUnionModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfilePartyUnionModel>(model, "Hre_ProfilePartyUnion", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_ProfilePartyUnionEntity, Hre_ProfilePartyUnionModel>(model);
        }
        /// <summary>
        /// [Tho.Bui] - Xóa hoặc chuyển đổi trạng thái của ProfilePartyUnion(ProfilePartyUnion) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfilePartyUnionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_ProfilePartyUnionEntity, Hre_ProfilePartyUnionModel>(id);
            return result;
        }
    }
}
