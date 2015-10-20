using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_PayrollGroupController : ApiController
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
        /// [T Lấy dữ liệu Nhóm Lương(Cat_PayrollGroup) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_PayrollGroupModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_PayrollGroupModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_PayrollGroupEntity>(id, ConstantSql.hrm_cat_sp_get_PayrollGroupById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_PayrollGroupModel>();
            }
            model.ActionStatus = status;
            return model;
        }


    // DELETE api/<controller>/5
        public void Delete(Guid ID)
        {
            var service = new Cat_PayrollGroupServices();
            var result = service.Remove<Cat_PayrollGroupEntity>(ID);
        }


        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa nhóm lương(Cat_PayrollGroup)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_PayrollGroup")]
        public Cat_PayrollGroupModel Post([Bind]Cat_PayrollGroupModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_PayrollGroupModel>(model, "Cat_PayrollGroup", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_PayrollGroupEntity, Cat_PayrollGroupModel>(model);
        }

        
    }
}