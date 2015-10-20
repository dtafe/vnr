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
using HRM.Business.Main.Domain;
using HRM.Business.Category.Models;                      

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_ContractController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Contract(Hre_Contract) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ContractModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_ContractModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Hre_ContractEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ContractById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_ContractModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Contract(Hre_Contract) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ContractModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_ContractEntity, Hre_ContractModel>(id);
            return result;
        }

        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPut]
        [System.Web.Mvc.RouteAttribute("api/Hre_Contract")]
        public Hre_ContractModel Put([Bind]Hre_ContractModel model)
        {
            var ContractServices = new Hre_ContractServices();
            string status = string.Empty;
            string id = string.Empty;
            foreach (var item in model.selecteIds)
            {
                id += Common.DotNetToOracle(item.ToString()) + ",";
            }
            id = id.Substring(0, id.Length - 1);
            string type = model.Status;
            var service = new BaseService();
            var actionServices = new ActionService(UserLogin);
            if (model.ID == Guid.Empty)
            {
                model.DateExtend = model.DateEnd;
            }

            var objContract = new List<object>();
            objContract.AddRange(new object[2]);
            objContract[0] = 1;
            objContract[1] = int.MaxValue - 1;
            var lstContracts = actionServices.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractData, ref status).ToList();

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var idItem = id.Split(',');
                    for (int i = 0; i < idItem.Count(); i++)
                    {
                        var idInt = idItem[i];
                        if (idInt != null)
                        {
                            var entity = lstContracts.Where(s => s.ID != null && Common.DotNetToOracle(s.ID.ToString()) == idInt).FirstOrDefault();
                            var lstContractByProfileID = lstContracts.Where(s => s.ProfileID == entity.ProfileID && s.Status == EnumDropDown.Status.E_APPROVED.ToString()).ToList();
                            var listIdContract = string.Empty;
                            if (lstContractByProfileID.Count > 0)
                            {
                                listIdContract = string.Join(",", lstContractByProfileID.Select(d => d.ContractTypeID));
                            }

                            entity = ContractServices.SetNewCodeContract(entity, listIdContract, UserLogin);
                            entity.Status = type;
                            if (!string.IsNullOrEmpty(entity.ContractNo))
                            {
                                // Theo task 0049504 - chỉ edit lại cột TimesContract nếu hd loại có thời hạn
                                if (entity.ContractTypeID != null)
                                {
                                    var contracttypeBycontract = actionServices.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(entity.ContractTypeID.ToString()), ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status).FirstOrDefault();
                                    if (contracttypeBycontract != null && contracttypeBycontract.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_DURATION.ToString())
                                    {
                                        try
                                        {
                                            string times = entity.ContractNo.Substring(entity.ContractNo.Length - 1, 1);
                                            int Year = int.Parse(times);
                                            entity.TimesContract = Year;
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            status = ContractServices.Edit(entity);
                            model.SetPropertyValue(Constant.ActionStatus, status);
                        }
                    }
                }
                catch (Exception ex)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error.ToString() + "," + ex.Message);
                }
            }

            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Contract(Hre_Contract)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Contract")]
        public Hre_ContractModel Post([Bind]Hre_ContractModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ContractModel>(model, "Hre_Contract", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.ContractEvaType == "E_ANNUAL_EVALUATION")
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ContractModel>(model, "EvaContractinfo", "Hre_Contract", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }

            #endregion

            if(model.ID == Guid.Empty)
            {
                model.DateExtend = model.DateEnd;
            }

            if (model.CreateBasicSalary == true)
            {
                Sal_BasicSalaryServices salaryservices = new Sal_BasicSalaryServices();
                Sal_BasicSalaryEntity basicSalaryBycontract = new Sal_BasicSalaryEntity();

                basicSalaryBycontract.ProfileID = model.ProfileID;
                basicSalaryBycontract.GrossAmount = model.Salary != null ? model.Salary.ToString() : "0";
                basicSalaryBycontract.CurrencyID = model.CurenncyID != null ? model.CurenncyID.Value : Guid.Empty;
                basicSalaryBycontract.PersonalRate = model.PersonalRate;
                basicSalaryBycontract.DateOfEffect = model.DateStart;

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
            return service.UpdateOrCreate<Hre_ContractEntity, Hre_ContractModel>(model);
        }
    }
}
