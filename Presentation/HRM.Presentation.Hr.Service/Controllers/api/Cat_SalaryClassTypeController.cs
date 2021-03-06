﻿using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_SalaryClassTypeController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Loại mã Lương(Cat_SalaryClassType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SalaryClassTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_SalaryClassTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_SalaryClassTypeEntity>(id, ConstantSql.hrm_cat_sp_get_SalaryClassTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_SalaryClassTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Loại mã Lương(Cat_SalaryClassType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SalaryClassTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_SalaryClassTypeEntity, Cat_SalaryClassTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một  Loại mã Lương(Cat_SalaryClassType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_SalaryClassType")]
        public Cat_SalaryClassTypeModel Post([Bind]Cat_SalaryClassTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_SalaryClassTypeModel>(model, "Cat_SalaryClassType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_SalaryClassTypeEntity, Cat_SalaryClassTypeModel>(model);
        }
    }
}
