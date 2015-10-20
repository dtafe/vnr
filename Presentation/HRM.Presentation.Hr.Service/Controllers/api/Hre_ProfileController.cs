using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Web;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_ProfileController : ApiController
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
        /// [Chuc.Nguyen] - Lấy dữ liệu Nhân Viên (Hre_Profile) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfileModel GetById(Guid id)
        {
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var entity = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
            //xu ly tam de lay ten loai nghi viec vi hien tai left join nhiu qa trong store khong chay dc
            if (entity.TypeOfStopID != null)
            {
                var catservice = new Cat_NameEntityServices();
                var entityNameEntity = catservice.GetData<Cat_NameEntityEntity>(Common.DotNetToOracle(entity.TypeOfStopID.ToString()), ConstantSql.hrm_cat_sp_get_NameEntityById, UserLogin, ref status).FirstOrDefault();
                if (entityNameEntity != null)
                {
                    entity.TypeOfStopName = entityNameEntity.NameEntityName;
                }

            }
            var model = entity.CopyData<Hre_ProfileModel>();
            if (model != null && model.DateOfEffect != null)
            {
                model.DateOfEffectOld = model.DateOfEffect;
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Nhân Viên (Hre_Profile) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfileModel DeleteOrRemove(string id)
        {
            //Xóa cache lưu lại của cây phòng ban
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeView");
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeViewSumProfile");

            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_ProfileEntity, Hre_ProfileModel>(id);
            return result;
        }

        /// <summary>
        ///[Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Nhân Viên (Hre_Profile)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public Hre_ProfileModel Post([Bind]Hre_ProfileModel model)
        {
            string status = string.Empty;
            if (model != null)
            {
                ActionService service = new ActionService(UserLogin);
                BaseService BaseService = new BaseService();

                if (!string.IsNullOrWhiteSpace(model.CodeAttendance))
                {
                    model.CodeAttendance = model.CodeAttendance.TrimAll();
                }

                #region Validate

                string message = string.Empty;

                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfileModel>(model, "Hre_Profile", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
                if (model.DateOfEffectOld != null)
                {
                    checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfileModel>(model, "Hre_ProfileCheckDateOfEffect", ref message);
                    if (!checkValidate)
                    {
                        model.ActionStatus = message;
                        return model;
                    }
                }
                #endregion

                #region Xử lý cập nhật AbilityTitle khi chọn rank

                if (model.SalaryClassID != null)
                {
                    var abilityTitleBySalaryClass = BaseService.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(model.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, UserLogin, ref status).FirstOrDefault();
                    if (abilityTitleBySalaryClass != null)
                    {
                        model.AbilityTileID = abilityTitleBySalaryClass.ID;
                    }
                }

                #endregion

                #region Xử Lý Lưu field DateOfBirth
                var dateOfBirth = string.Empty;
                if (model.DayOfBirth == 00 && model.MonthOfBirth == 00)
                {
                    model.ActionStatus = "ErrorDateOfBirth";
                    return model;
                }
                if (model.DayOfBirth == null && model.MonthOfBirth == null && model.YearOfBirth == null)
                {
                    dateOfBirth = string.Empty;

                }
                else
                {
                    if (model.DayOfBirth == null)
                    {
                        model.DayOfBirth = DateTime.Now.Day;
                    }
                    if (model.MonthOfBirth == null)
                    {
                        model.MonthOfBirth = DateTime.Now.Month;
                    }
                    if (model.YearOfBirth == null)
                    {
                        model.ActionStatus = "ErrorDateOfBirth";
                        return model;
                    }
                }


                if (model.DayOfBirth >= 0 || model.MonthOfBirth >= 0 || model.YearOfBirth >= 0)
                {
                    if (model.DayOfBirth == 30 && model.MonthOfBirth == 2)
                    {
                        model.ActionStatus = "ErrorDateOfBirth";
                        return model;
                    }
                    if (model.DayOfBirth == 31 && model.MonthOfBirth == 2)
                    {
                        model.ActionStatus = "ErrorDateOfBirth";
                        return model;
                    }
                    if (model.DayOfBirth > 31 || model.MonthOfBirth > 12)
                    {
                        model.ActionStatus = "ErrorDateOfBirth";
                        return model;
                    }
                    dateOfBirth = model.MonthOfBirth + "/" + model.DayOfBirth + "/" + model.YearOfBirth;

                }

                model.DateOfBirth = string.IsNullOrEmpty(dateOfBirth) ? (DateTime?)null : DateTime.Parse(dateOfBirth);
                #endregion
                Hre_ProfileModel HreProfile = new Hre_ProfileModel();
                if (model.ID != Guid.Empty)
                {
                    HreProfile = GetById(model.ID);
                }
                if (HreProfile != null && (model.ID == HreProfile.ID) && model.ActionStatus != "1")
                {
                    string[] listFieldName = new[] { "JobTitleID", "PositionID", "EmpTypeID", "DateOfEffect", "CostCentreID", "OrgStructureID", "WorkPlaceID", "SupervisorID" };
                    foreach (var item in listFieldName)
                    {
                        var value1 = model.GetPropertyValue(item);
                        var value2 = HreProfile.GetPropertyValue(item);
                        if ((value1 != null && value2 != null) && (value1.ToString() != value2.ToString()))
                        {
                            model.SetPropertyValue(Constant.ActionStatus, NotificationType.Change.ToString());
                            return model;
                        }
                    }
                    if (HreProfile.ProfileName != model.ProfileName)
                    {
                        if (model.ProfileName.Contains(' '))
                        {
                            model.FirstName = model.ProfileName.Substring(model.ProfileName.LastIndexOf(' ') + 1);
                            model.NameFamily = model.ProfileName.Substring(0, model.ProfileName.LastIndexOf(' '));
                        }
                        else
                        {
                            model.FirstName = model.ProfileName;
                        }
                    }
                }
                if (model.ActionStatus == "1" || model.ActionStatus == "Success" || string.IsNullOrEmpty(model.ActionStatus))
                {

                    var serviceAddress = new Hre_AddressServices();
                    var serviceProfile = new Hre_ProfileServices();
                    var profileEntity = new Hre_ProfileEntity();
                    if (model.ID != Guid.Empty)
                    {
                        profileEntity = serviceProfile.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(model.ID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
                    }

                    #region Xử lý lưu CardCode
                    Hre_CardHistoryServices cardservices = new Hre_CardHistoryServices();
                    Hre_WorkHistoryServices workHistoryservices = new Hre_WorkHistoryServices();

                    if (profileEntity != null && model.CodeAttendance != null && model.DateApplyAttendanceCode != null && (model.CodeAttendance != profileEntity.CodeAttendance
                    || model.DateApplyAttendanceCode != profileEntity.DateApplyAttendanceCode))
                    {
                        // Chỉ thay đổi mã chấm công
                        if (model.DateApplyAttendanceCode == profileEntity.DateApplyAttendanceCode && model.CodeAttendance != profileEntity.CodeAttendance)
                        {
                            Hre_CardHistoryEntity history = cardservices.GetData<Hre_CardHistoryEntity>(Common.DotNetToOracle(model.ID.ToString()), ConstantSql.hrm_hr_sp_get_CardHistoryByProfileId, UserLogin, ref status).OrderByDescending(s => s.DateEffect).FirstOrDefault();
                            if (history != null)
                            {
                                history.CardCode = model.CodeAttendance;
                                cardservices.Edit(history);
                            }
                            else
                            {
                                history = new Hre_CardHistoryEntity();
                                history.ProfileID = model.ID;
                                history.CardCode = model.CodeAttendance;
                                history.DateEffect = model.DateApplyAttendanceCode;
                                cardservices.Add(history);
                            }
                        }
                        // Chỉ thay đổi Ngày áp dụng mã chấm công

                        else if (model.CodeAttendance == profileEntity.CodeAttendance && model.DateApplyAttendanceCode != profileEntity.DateApplyAttendanceCode)
                        {
                            if (model.DateApplyAttendanceCode < profileEntity.DateApplyAttendanceCode)
                            {
                                model.StatusVerify = "Invalid";
                                return model;
                            }
                            Hre_CardHistoryEntity history = cardservices.GetData<Hre_CardHistoryEntity>(Common.DotNetToOracle(model.ID.ToString()), ConstantSql.hrm_hr_sp_get_CardHistoryByProfileId, UserLogin, ref status).OrderByDescending(s => s.DateEffect).FirstOrDefault();
                            if (history != null)
                            {
                                history.DateEffect = model.DateApplyAttendanceCode;
                                cardservices.Edit(history);
                            }
                            else
                            {
                                history = new Hre_CardHistoryEntity();
                                history.ProfileID = model.ID;
                                history.CardCode = model.CodeAttendance;
                                history.DateEffect = model.DateApplyAttendanceCode;
                                cardservices.Add(history);
                            }
                        }
                    }
                    // Tạo mới NV hoặc thay đổi cả 2( mã chấm công + ngày hiệu lực)
                    Hre_CardHistoryEntity cardhistory = null;
                    if (model != null && model.ID == Guid.Empty || (model != null && profileEntity != null && model.CodeAttendance != profileEntity.CodeAttendance
                                && model.DateApplyAttendanceCode != profileEntity.DateApplyAttendanceCode))
                    {
                        cardhistory = new Hre_CardHistoryEntity();
                        cardhistory.ProfileID = model.ID;
                        cardhistory.CardCode = model.CodeAttendance;
                        cardhistory.DateEffect = model.DateApplyAttendanceCode;
                    }
                    #endregion

                    #region Xử lý lưu quá trình công tác
                    Hre_WorkHistoryEntity workHistory = null;
                    if (model != null && (model.JobTitleID != profileEntity.JobTitleID || model.PositionID != profileEntity.PositionID || model.EmpTypeID != profileEntity.EmpTypeID ||
                        (model.DateOfEffect != null && profileEntity.DateOfEffect != null &&
                        model.DateOfEffect.Value.ToString() != profileEntity.DateOfEffect.Value.ToString())
                        || model.CostCentreID != profileEntity.CostCentreID || model.SupervisorID != profileEntity.SupervisorID
                        || model.OrgStructureID != profileEntity.OrgStructureID || model.WorkPlaceID != profileEntity.WorkPlaceID))
                    {

                        workHistory = new Hre_WorkHistoryEntity();
                        var orgService = new Cat_OrgStructureServices();
                        var jobtitleService = new Cat_JobTitleServices();
                        var postitionService = new Cat_PositionServices();
                        var workPlaceService = new Cat_WorkPlaceServices();

                        workHistory.CostCentreID = model.CostCentreID;
                        workHistory.OrganizationStructureID = model.OrgStructureID;
                        workHistory.PositionID = model.PositionID;
                        workHistory.JobTitleID = model.JobTitleID;
                        workHistory.SalaryClassID = model.SalaryClassID;
                        workHistory.CostSourceID = model.CostSourceID;
                        workHistory.AbilityTileID = model.AbilityTileID;
                        if (profileEntity != null)
                        {
                            var orgStructureOld = new Cat_OrgStructureEntity();
                            if (profileEntity.OrgStructureID != null && profileEntity.OrgStructureID != Guid.Empty)
                            {
                                orgStructureOld = orgService.GetData<Cat_OrgStructureEntity>(profileEntity.OrgStructureID, ConstantSql.hrm_cat_sp_get_OrgStructureById, UserLogin, ref status).FirstOrDefault();
                            }
                            var jobtitleOld = new Cat_JobTitleEntity();
                            if (profileEntity.JobTitleID != null && profileEntity.JobTitleID != Guid.Empty)
                            {
                                jobtitleOld = jobtitleService.GetData<Cat_JobTitleEntity>(profileEntity.JobTitleID, ConstantSql.hrm_cat_sp_get_HDTJobTypeById, UserLogin, ref status).FirstOrDefault();
                            }
                            var postitionOld = new Cat_PositionEntity();
                            if (profileEntity.PositionID != null && profileEntity.PositionID != Guid.Empty)
                            {
                                postitionOld = postitionService.GetData<Cat_PositionEntity>(profileEntity.PositionID, ConstantSql.hrm_cat_sp_get_PositionById, UserLogin, ref status).FirstOrDefault();
                            }
                            var workPlace = new Cat_WorkPlaceEntity();
                            if (model.WorkPlaceID != null && model.WorkPlaceID != Guid.Empty)
                            {
                                workPlace = workPlaceService.GetData<Cat_WorkPlaceEntity>(model.WorkPlaceID, ConstantSql.hrm_cat_sp_get_WorkPlaceById, UserLogin, ref status).FirstOrDefault();
                            }
                            var workPlaceOld = new Cat_WorkPlaceEntity();
                            if (profileEntity.WorkPlaceID != null && profileEntity.WorkPlaceID != Guid.Empty)
                            {
                                workPlaceOld = workPlaceService.GetData<Cat_WorkPlaceEntity>(profileEntity.WorkPlaceID, ConstantSql.hrm_cat_sp_get_WorkPlaceById, UserLogin, ref status).FirstOrDefault();
                            }
                            workHistory.OrgOld = orgStructureOld != null ? orgStructureOld.OrgStructureName : "";
                            workHistory.JobTitleOld = jobtitleOld != null ? jobtitleOld.JobTitleName : "";
                            workHistory.PositionOld = postitionOld != null ? postitionOld.PositionName : "";
                            workHistory.WorkLocation = workPlace != null ? workPlace.WorkPlaceName : "";
                            workHistory.WorkLocationOld = workPlaceOld != null ? workPlaceOld.WorkPlaceName : "";
                        }
                        if (workHistory.ID == Guid.Empty)
                        {
                            if (model.DateOfEffect != null)
                            {
                                workHistory.DateEffective = model.DateOfEffect.Value;
                            }
                            else
                            {
                                workHistory.DateEffective = DateTime.Now;
                            }
                        }

                        if (model.DateQuit != null && (profileEntity == null || profileEntity.DateQuit != model.DateQuit))
                        {
                            workHistory.DateEffective = model.DateQuit.Value;
                        }
                    }

                    #region Ngày hiệu lực phải <= ngày hiện tại thì mới cập nhật lại quá trình công tác
                    if (model.DateOfEffect > DateTime.Now && profileEntity != null && profileEntity.DateOfEffect != null)
                    {
                        model.DateOfEffect = profileEntity.DateOfEffect;
                        model.OrgStructureID = profileEntity.OrgStructureID;
                        model.ShopID = profileEntity.ShopID;
                        model.JobTitleID = profileEntity.JobTitleID;
                        model.PositionID = profileEntity.PositionID;
                        model.SupervisorID = profileEntity.SupervisorID;
                        model.HighSupervisorID = profileEntity.HighSupervisorID;
                        model.IsHeadDept = profileEntity.IsHeadDept;
                        model.EmpTypeID = profileEntity.EmpTypeID;
                        model.LaborType = profileEntity.LaborType;
                        model.SikillLevel = profileEntity.SikillLevel;
                        model.PayrollGroupID = profileEntity.PayrollGroupID;
                        model.SalaryClassID = profileEntity.SalaryClassID;
                        model.CostCentreID = profileEntity.CostCentreID;
                        model.LocationCode = profileEntity.LocationCode;
                        model.WorkPlaceID = profileEntity.WorkPlaceID;
                    }
                    #endregion

                    #endregion

                    var profileModel = service.UpdateOrCreate<Hre_ProfileEntity, Hre_ProfileModel>(model);

                    if (cardhistory != null && profileModel != null)
                    {
                        cardhistory.ProfileID = profileModel.ID;
                        cardservices.Add(cardhistory);
                    }

                    if (workHistory != null && profileModel != null)
                    {
                        workHistory.EmployeeTypeID = profileModel.EmpTypeID;
                        workHistory.ProfileID = profileModel.ID;
                        workHistoryservices.Add(workHistory);
                    }
                }
            }

            //Xóa cache lưu lại của cây phòng ban
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeView");
            HttpContext.Current.Cache.Remove("List_OrgStructureTreeViewSumProfile");

            return model;
        }
    }
}
