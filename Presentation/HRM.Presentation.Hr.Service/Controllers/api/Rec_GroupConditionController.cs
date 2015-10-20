using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Recruitment.Models;
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
    public class Rec_GroupConditionController : ApiController
    {
        /// <summary>
        /// [T Lấy dữ liệu Nhãn Hồ Sơ(Rec_GroupCondition) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
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
        public Rec_GroupConditionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_GroupConditionModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_GroupConditionEntity>(id, ConstantSql.hrm_tra_sp_get_GroupConditionById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_GroupConditionModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public Rec_GroupConditionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Rec_GroupConditionEntity, Rec_GroupConditionModel>(id);
            return result;
        }

        //public Rec_GroupConditionModel DeleteOrRemove(string id)
        //{
        //    ActionService service = new ActionService(UserLogin);
        //    var result = service.DeleteOrRemove<Rec_GroupConditionEntity, Rec_GroupConditionModel>(id);
        //    return result;
        //}

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa nhãn hồ sơ (Rec_GroupCondition)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_GroupCondition")]
        public Rec_GroupConditionModel Post([Bind]Rec_GroupConditionModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_GroupConditionModel>(model, "Rec_GroupCondition", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_GroupConditionEntity, Rec_GroupConditionModel>(model);
        }
    }
}