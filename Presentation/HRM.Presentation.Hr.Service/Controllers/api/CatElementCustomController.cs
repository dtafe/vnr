using Antlr.Runtime.Misc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatElementCustomController : ApiController
    {
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
        /// <summary>
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Thiết Bị(Cat_Element) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatElementModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatElementModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ElementEntity>(id,ConstantSql.hrm_cat_sp_get_ElementById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatElementModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Element) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatElementModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ElementEntity, CatElementModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Cat_Element)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatElementCustom")]
        public CatElementModel Post([Bind]CatElementModel model)
        {
            ActionService service = new ActionService(UserLogin);
            
            string status = string.Empty;
            //#region Validate
            //string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatElementModel>(model, "Cat_Element", ref message);
            //if (!checkValidate)
            //{
            //    model.ActionStatus = message;
            //    return model;
            //}
            //#endregion
            if(model.IsSalaryTime){
                CatElementModel SalaryTimeModel = new CatElementModel();
                SalaryTimeModel.ElementCode = HRM.Infrastructure.Utilities.EnumDropDown.SalaryType.E_BASICSALARY_TIME.ToString();
                SalaryTimeModel.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                SalaryTimeModel.ElementName = ConstantDisplay.HRM_Category_GradePayroll_IsSalaryTime.ToString();
                SalaryTimeModel.GradePayrollID = model.GradePayrollID.Value;
                SalaryTimeModel.Formula = "([" + PayrollElement.SAL_BASIC_SALARY + "]" + "/" + "[" + PayrollElement.ATT_STD_DAY + "])" + "*" + "[" + PayrollElement.ATT_WORKING_DAY + "]";
                return service.UpdateOrCreate<Cat_ElementEntity, CatElementModel>(SalaryTimeModel);
            }
            if(model.IsSalaryOvertime){
                CatElementModel SalaryOvertimeModel = new CatElementModel();
                SalaryOvertimeModel.ElementCode = HRM.Infrastructure.Utilities.EnumDropDown.SalaryType.E_BASICSALARY_OVERTIME.ToString();
                SalaryOvertimeModel.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                SalaryOvertimeModel.ElementName = ConstantDisplay.HRM_Category_GradePayroll_IsSalaryOvertime.ToString();
                SalaryOvertimeModel.GradePayrollID = model.GradePayrollID.Value;
                SalaryOvertimeModel.Formula = "(([" + PayrollElement.SAL_BASIC_SALARY + "]" + "/" + "[" + PayrollElement.ATT_STD_DAY + "])" + "/" + "[" + PayrollElement.ATT_HOURS_PER_DAY + "])" + "*" + "[" + PayrollElement.ATT_OVERTIME_HOURS + "]";
                return service.UpdateOrCreate<Cat_ElementEntity, CatElementModel>(SalaryOvertimeModel);
            }

            if(model.IsSalaryLeaveDay){
                CatElementModel SalaryLeaveDayModel = new CatElementModel();
                SalaryLeaveDayModel.ElementCode = HRM.Infrastructure.Utilities.EnumDropDown.SalaryType.E_BASICSALARY_LEAVEDAY.ToString();
                SalaryLeaveDayModel.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                SalaryLeaveDayModel.ElementName = ConstantDisplay.HRM_Category_GradePayroll_IsSalaryLeaveDay.ToString();
                SalaryLeaveDayModel.GradePayrollID = model.GradePayrollID.Value;
                SalaryLeaveDayModel.Formula = "(([" + PayrollElement.SAL_BASIC_SALARY + "]" + "/" + "[" + PayrollElement.ATT_STD_DAY + "])" + "/" + "[" + PayrollElement.ATT_HOURS_PER_DAY + "])" + "*" + "[" + PayrollElement.ATT_TOTAL_PAID_LEAVEDAY_HOURS + "]"; 
             
                return service.UpdateOrCreate<Cat_ElementEntity, CatElementModel>(SalaryLeaveDayModel);
            }
           
            if(model.IsSalaryUsualAllowance){
                var UsualAllowanceServices = new Cat_UnusualAllowanceCfgServices();
                var elementServices = new Cat_ElementServices();
                var lstObj = new List<object>();
                lstObj.Add(null);
                lstObj.Add(null);
                lstObj.Add(1);
                lstObj.Add(int.MaxValue -1);

                var lstUsualAllowance = UsualAllowanceServices.GetData<Cat_UsualAllowanceEntity>(lstObj, ConstantSql.hrm_cat_sp_get_UsualAllowance, UserLogin, ref status).ToList();

                if (lstUsualAllowance != null)
                {
                    List<CatElementModel> lstElement = new List<CatElementModel>();
                    foreach (var item in lstUsualAllowance)
                    {
                        CatElementModel SalaryUsualAllowanceModel = new CatElementModel();
                        SalaryUsualAllowanceModel.ElementCode = item.Code;
                        SalaryUsualAllowanceModel.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                        SalaryUsualAllowanceModel.ElementName = item.UsualAllowanceName;
                        SalaryUsualAllowanceModel.GradePayrollID = model.GradePayrollID.Value;
                        SalaryUsualAllowanceModel.Formula = "[" + item.Code + "]";
                        lstElement.Add(SalaryUsualAllowanceModel);
                    }
                    if (lstElement != null)
                    {
                        model.ActionStatus =  elementServices.Add(lstElement);
                    }
                }
                return model;
               
            }
            
            return null;
        }
	}

}