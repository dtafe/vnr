using System.Web.Http;
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
    public class Cat_SalaryRankController : ApiController
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
        /// [Tho.Bui] - Lấy dữ liệu mã hệ số Lương(Cat_SalaryRank) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SalaryRankModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_SalaryRankModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_SalaryRankEntity>(id, ConstantSql.hrm_cat_sp_get_SalaryRankById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_SalaryRankModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui]  - Xóa hoặc chuyển đổi trạng thái của hệ số lương(Cat_SalaryRank) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SalaryRankModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_SalaryRankEntity, Cat_SalaryRankModel>(id);
            return result;
        }

        /// <summary>
        /// [Tho.Bui]  - Xử lý thêm mới hoặc chỉnh sửa một hệ số Lương(Cat_SalaryRank)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_SalaryRank")]
        public Cat_SalaryRankModel Post([Bind]Cat_SalaryRankModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_SalaryRankModel>(model, "Cat_SalaryRank", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_SalaryRankEntity, Cat_SalaryRankModel>(model);
        }
	}
}