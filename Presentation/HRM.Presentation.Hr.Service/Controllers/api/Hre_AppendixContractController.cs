using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Web.Mvc;
using System;
using HRM.Business.Payroll.Domain;
using HRM.Business.Payroll.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_AppendixContractController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu AppendixContract(Hre_AppendixContract) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AppendixContractModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_AppendixContractModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_AppendixContractEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_AppendixContractById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_AppendixContractModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của AppendixContract(Hre_AppendixContract) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AppendixContractModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_AppendixContractEntity, Hre_AppendixContractModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một AppendixContract(Hre_AppendixContract)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_AppendixContract")]
        public Hre_AppendixContractModel Post([Bind]Hre_AppendixContractModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_AppendixContractModel>(model, "Hre_AppendixContract", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            if(model.ProfileContractID != Guid.Empty)
            {
                model.ContractID = model.ProfileContractID;
            }
            if (model.AppendixCreateBasicSalary == true)
            {
                Sal_BasicSalaryServices salaryservices = new Sal_BasicSalaryServices();
                Sal_BasicSalaryEntity basicSalaryBycontract = new Sal_BasicSalaryEntity();

                basicSalaryBycontract.ProfileID = model.AppendixProfileID.Value;
                basicSalaryBycontract.GrossAmount = model.Salary != null ? model.Salary.ToString() : "0";
                basicSalaryBycontract.CurrencyID = model.CurenncyID != null ? model.CurenncyID.Value : Guid.Empty;
                basicSalaryBycontract.PersonalRate = model.PersonalRate;
                if (model.DateofEffect != null)
                {
                    basicSalaryBycontract.DateOfEffect = model.DateofEffect.Value;
                }

                basicSalaryBycontract.InsuranceAmount = model.InsuranceAmount != null ? model.InsuranceAmount.Value : 0;
                basicSalaryBycontract.CurrencyID1 = model.CurenncyID1;

                basicSalaryBycontract.ClassRateID = model.ClassRateID;
                basicSalaryBycontract.RankRateID = model.RankRateID;

                basicSalaryBycontract.AllowanceType1ID = model.AllowanceID1;
                basicSalaryBycontract.AllowanceAmount1 = model.Allowance1;
                basicSalaryBycontract.CurrencyID2 = model.CurenncyID2;

                basicSalaryBycontract.AllowanceType2ID = model.AllowanceID2;
                basicSalaryBycontract.AllowanceAmount2 = model.Allowance2;
                basicSalaryBycontract.CurrencyID3 = model.CurenncyID3;

                basicSalaryBycontract.AllowanceType3ID = model.AllowanceID3;
                basicSalaryBycontract.AllowanceAmount3 = model.Allowance3;
                basicSalaryBycontract.CurrencyID4 = model.CurenncyIDSalary;

                salaryservices.Add(basicSalaryBycontract);

            }
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_AppendixContractEntity, Hre_AppendixContractModel>(model, model.UserID);
        }
    }
}
