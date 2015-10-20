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
    public class Rec_RecCostDetailController : ApiController
    {
        /// <summary>
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
        public Rec_RecCostDetailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_RecCostDetailModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_RecCostDetailEntity>(id, ConstantSql.hrm_rec_sp_get_RecCostDetailById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Rec_RecCostDetailModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Rec_RecCostDetailServices();
            var result = service.Remove<Rec_RecCostDetailEntity>(ID);
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa nhãn hồ sơ (Rec_RecCostDetail)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_RecCostDetail")]
        public Rec_RecCostDetailModel Post([Bind]Rec_RecCostDetailModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_RecCostDetailModel>(model, "Rec_RecCostDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rec_RecCostDetailEntity, Rec_RecCostDetailModel>(model);
        }
    }
}