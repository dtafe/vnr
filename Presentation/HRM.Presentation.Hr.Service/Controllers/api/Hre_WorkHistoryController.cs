using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_WorkHistoryController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu WorkHistory(Hre_WorkHistory) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Hre_WorkHistoryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_WorkHistoryModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Hre_WorkHistoryEntity>(id, ConstantSql.hrm_hr_sp_get_WorkHistoryById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Hre_WorkHistoryModel>();
            }
            if (model != null && model.DateEffective != null)
            {
                model.DateEffectiveOld = model.DateEffective;
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của WorkHistory(Hre_WorkHistory) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_WorkHistoryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_WorkHistoryEntity, Hre_WorkHistoryModel>(id);
            return result;
        }

        //public void Delete(Guid ID)
        //{
        //    var service = new Hre_WorkHistoryServices();
        //    var result = service.Remove<Hre_WorkHistoryEntity>(ID);
        //}

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một WorkHistory(Hre_WorkHistory)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_WorkHistory")]
        public Hre_WorkHistoryModel Post([Bind]Hre_WorkHistoryModel model)
        {
            #region Validate
            BaseService BaseService = new BaseService();
            string status = string.Empty;
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_WorkHistoryModel>(model, "Hre_WorkHistory", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion         
            var WorkHistoryServices = new Hre_WorkHistoryServices();
            var SalaryClassServices = new Cat_SalaryClassServices();
            var OrgStructureServices = new Cat_OrgStructureServices();
            var JobTitleServices = new Cat_JobTitleServices();
            var PositionServices = new Cat_PositionServices();
            var WorkHistoryOld = BaseService.GetData<Hre_WorkHistoryEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, UserLogin, ref status).OrderByDescending(s => s.DateEffective).FirstOrDefault();
            if (WorkHistoryOld != null)
            {
                model.SalaryClassNameOld = WorkHistoryOld.SalaryClassName;
                model.JobTitleOld = WorkHistoryOld.JobTitleName;
                model.PositionOld = WorkHistoryOld.PositionName;
                model.OrgStructureOldID = WorkHistoryOld.OrganizationStructureID;
                model.WorkLocationOld = WorkHistoryOld.WorkLocation;
            }

            if (model.SalaryClassID != null)
            {
                var abilityTitleBySalaryClass = BaseService.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(model.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId,UserLogin, ref status).FirstOrDefault();
                if (abilityTitleBySalaryClass != null)
                {
                    model.AbilityTileID = abilityTitleBySalaryClass.ID;
                }
            }
            // Son.Vo - 20140107 - Xử lý cập nhật ngược lại bảng profile
            if (model.Status == WorkHistoryStatus.E_APPROVED.ToString() && model.DateEffective != null && model.DateEffective <= DateTime.Now.Date)
            {
                Hre_ProfileServices profileServices = new Hre_ProfileServices();
                Hre_ProfileEntity profile = BaseService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById,UserLogin, ref status).FirstOrDefault();
                if (profile != null) 
                {
                    profile.OrgStructureID = model.OrganizationStructureID;
                    profile.JobTitleID = model.JobTitleID;
                    profile.PositionID = model.PositionID;
                    profile.DateOfEffect = model.DateEffective;
                    profile.LaborType = model.LaborType;
                    profile.CostCentreID = model.CostCentreID;
                    profile.FormType = model.FormType;
                    profile.EmpTypeID = model.EmployeeTypeID;
                    profile.WorkingPlace = model.WorkLocation;
                    profile.AbilityTileID = model.AbilityTileID;
                    profileServices.Edit(profile);
                }
            }

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_WorkHistoryEntity, Hre_WorkHistoryModel>(model);
        }

    }
}
