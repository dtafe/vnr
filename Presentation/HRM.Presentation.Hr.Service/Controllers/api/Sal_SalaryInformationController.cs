using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_SalaryInformationController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu Thông Tin Tài Khoản(Sal_SalaryInformation) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_SalaryInformationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_SalaryInformationModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_SalaryInformationEntity>(id, ConstantSql.hrm_sal_sp_get_Sal_SalaryInformationById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Sal_SalaryInformationModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do]  - Xóa hoặc chuyển đổi trạng thái của Thông Tin Tài Khoản(Sal_SalaryInformation) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_SalaryInformationModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_SalaryInformationEntity, Sal_SalaryInformationModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do]  - Xử lý thêm mới hoặc chỉnh sửa một Thông Tin Tài Khoản(Sal_SalaryInformation)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_SalaryInformation")]
        public Sal_SalaryInformationModel Post([Bind]Sal_SalaryInformationModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_SalaryInformationModel>(model, "Sal_SalaryInformation", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_SalaryInformationEntity, Sal_SalaryInformationModel>(model);
        }
    }
}
