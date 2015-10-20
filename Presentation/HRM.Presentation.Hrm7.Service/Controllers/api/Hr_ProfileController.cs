using HRM.Presentation.Hrm7.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Laundry.Models;
namespace HRM.Presentation.Hrm7.Service.Controllers.api
{
    public class Hr_ProfileController : ApiController
    {
        ////
        //// GET: /Hr_Profile/
        //public ActionResult Index()
        //{
        //    var dbhr7 = new HRM7Entities();
        //    var list = dbhr7.Hre_Profile.ToList();
        //    //VnrHrmDataContext dbMain = new VnrHrmDataContext();
        //    List<HRM.Presentation.Hr.Models.Hre_ProfileModel> listProfiles = new List<Hr.Models.Hre_ProfileModel>();
        //    //ViewBag.Title = "Home Page";
        //    HRM.Presentation.Hr.Models.Hre_ProfileModel profile = null;
        //    foreach (var item in list)
        //    {
        //        profile = new HRM.Presentation.Hr.Models.Hre_ProfileModel();
        //        profile.ProfileName = item.ProfileName;
        //        profile.CodeEmp = item.CodeEmp;
        //        profile.CodeAttendance = item.CodeAttendance;
        //        profile.IDNo = item.IDNo;
        //        listProfiles.Add(profile);
        //    }
        //    return View();
        //}

        public Hre_DataImportModel Get()
        {

            var dbhr7 = new HondaVersion3Entities();

           
            var listDataImportModel = new Hre_DataImportModel();

            #region Get Data Từ Bản 7

            #region Get Data Chấm Công Từ Bản 7
            // Get Data Att_WorkDay
            var listWorkdayEntity = dbhr7.Att_Workday.ToList();
            #endregion

            #region Get Data Danh Mục Từ Bản 7

            // Get Province
            var listProvinceEntity = dbhr7.Cat_Province.ToList();

            //Get Position
            var listPositionEntity = dbhr7.Cat_Position.ToList();

            //Get JobTitle
            var listJobTitleEntity = dbhr7.Cat_JobTitle.ToList();

            //get EmployeeType
            var listEmployeeTypeEntity = dbhr7.Cat_EmployeeType.ToList();

            //Get OrgStructure
            var listOrgStructureEntity = dbhr7.Cat_OrgStructure.ToList();

            //get WorkPlace
            var listWorkPlaceEntity = dbhr7.Cat_WorkPlace.ToList();

            //get CostCentre
            var listCatCostCentreEntity = dbhr7.Cat_CostCentre.ToList();

            //get Shift
            var listCatShiftEntity = dbhr7.Cat_Shift.ToList();

            //get TAMScanReasonMissing
            var listTAMScanReasonMissingEntity = dbhr7.Cat_TAMScanReasonMiss.ToList();

          
            #endregion

            #region Get Data Nhân Sự Từ Bản 7

            //get Profile
            var listProfileEntity = dbhr7.Hre_Profile.ToList();
            #endregion

            #region Get Data Nhà Ăn Từ Bản 7

            //get MealRecordMissing
            var listMealRecordMissingEntity = dbhr7.Can_MealRecordMissing.ToList();

            //get MealAllowanceTypeSetting
            var listMealAllowanceTypeSettingEntity = dbhr7.Can_MealAllowanceTypeSetting.ToList();

            //get MealAllowanceToMoney
            var listMealAllowanceToMoneyEntity = dbhr7.Can_MealAllowanceToMoney.ToList();

            //get TAMScanLogCMS
            var listTamScanLogCMSEntity = dbhr7.Can_TamScanLogCMS.ToList();
        
            #endregion

            #region Get Data Giặt Là Từ Bản 7
            
            //Lau_TAMScanLogLMS
            var listTAMScanLogLMSEntity = dbhr7.LMS_TamScanLogLMS.ToList();

            #endregion

            #endregion

            #region Kiểm Tra Dữ Liệu

            #region Danh Mục

            #region Cat_Province

            if (listProvinceEntity != null && listProvinceEntity.Count > 0)
            {
                //3. Add vào list Model để trả về
                var listProvinceModel = listProvinceEntity.Select(s => new CatProvinceModel()
                {
                    ProvinceName = s.ProvinceName,
                    ProvinceCode  = s.Code
                }).ToList();

                listDataImportModel.ListProvince = new List<CatProvinceModel>();
                listDataImportModel.ListProvince.AddRange(listProvinceModel);
            }

            #endregion

            #region Cat_JobTitle
            var listJobTitleModel = new List<HRM.Presentation.Category.Models.CatJobTitleModel>();
            if (listJobTitleEntity != null && listJobTitleEntity.Count > 0)
            {
                listJobTitleModel = listJobTitleEntity.Select(s => new HRM.Presentation.Category.Models.CatJobTitleModel()
               {
                   JobTitleCode = s.JobTitleCode,
                   JobTitleName = s.JobTitleName,
                   JobDescription = s.JobDescription,
                   JobTitleNameEn = s.JobTitleNameEn,
                   Notes = s.Notes,
                   SalaryMin = s.SalaryMin,
                   SalaryMax = s.SalaryMax,
                   DateCreate = s.DateCreate,
                   DateLock = s.DateLock,
                   DateUpdate = s.DateUpdate,
                   UserCreate = s.UserCreate,
                   UserUpdate = s.UserUpdate,


               }).ToList();
                listDataImportModel.ListJobTitle = new List<HRM.Presentation.Category.Models.CatJobTitleModel>();
                listDataImportModel.ListJobTitle.AddRange(listJobTitleModel);
            }
            else 
            {
                listDataImportModel.ListJobTitle = new List<HRM.Presentation.Category.Models.CatJobTitleModel>();
                listDataImportModel.ListJobTitle.AddRange(listJobTitleModel);
            }

            #endregion

            #region Cat_Position
            var listPositionModel = new List<HRM.Presentation.Category.Models.CatPositionModel>();
            if (listPositionEntity != null && listPositionEntity.Count > 0)
            {
                listPositionModel = listPositionEntity.Select(s => new HRM.Presentation.Category.Models.CatPositionModel()
               {
                   PositionName = s.PositionName,
                   PositionEngName = s.PositionEngName,
                   Description = s.Description,
                   IsDelete = s.IsDelete,
                   DateCreate = s.DateCreate,
                   DateLock = s.DateLock,
                   DateUpdate = s.DateUpdate,
                   PositionCode = s.Code,
                   UserCreate = s.UserCreate,
                   UserUpdate = s.UserUpdate,
                   IPCreate = s.IPCreate,
                   IPUpdate = s.IPUpdate,
                   ServerCreate = s.ServerCreate,
                   ServerUpdate = s.ServerUpdate,
                   JobTitleCode = s.Cat_JobTitle.FirstOrDefault() == null ? string.Empty : listJobTitleEntity.Where(m => m.PositionID == s.Cat_JobTitle.FirstOrDefault().PositionID).FirstOrDefault().JobTitleCode
               }).ToList();
                listDataImportModel.ListPosition = new List<HRM.Presentation.Category.Models.CatPositionModel>();
                listDataImportModel.ListPosition.AddRange(listPositionModel);
            }
            else 
            {
                listDataImportModel.ListPosition = new List<HRM.Presentation.Category.Models.CatPositionModel>();
                listDataImportModel.ListPosition.AddRange(listPositionModel);
            }

            #endregion

            #region Cat_EmployeeType

            if (listEmployeeTypeEntity != null && listEmployeeTypeEntity.Count > 0)
            {
                var listEmployeeTypeModel = listEmployeeTypeEntity.Select(s => new HRM.Presentation.Category.Models.CatEmployeeTypeModel()
                {
                    EmployeeTypeName = s.EmployeeTypeName,
                    CodeID = s.CodeID,
                    Notes = s.Notes,
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    IsDelete = s.IsDelete,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate
                }).ToList();
                listDataImportModel.ListEmployeeType = new List<HRM.Presentation.Category.Models.CatEmployeeTypeModel>();
                listDataImportModel.ListEmployeeType.AddRange(listEmployeeTypeModel);
            }
            #endregion

            #region Cat_WorkPlace
            var listWorkPlaceModel = new List<CatWorkPlaceModel>();
            if (listWorkPlaceEntity != null && listWorkPlaceEntity.Count > 0)
            {


                listWorkPlaceModel = listWorkPlaceEntity.Select(s => new CatWorkPlaceModel()
                {
                    WorkPlaceName = s.WorkPlaceName,
                    WorkPlaceCode = s.Code,
                    Description = s.Description,
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    DateLock = s.DateLock,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate,
                    IsDelete = s.IsDelete,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,


                }).ToList();
                listDataImportModel.ListWorkPlace = new List<CatWorkPlaceModel>();
                listDataImportModel.ListWorkPlace.AddRange(listWorkPlaceModel);
            }
            else 
            {
                listDataImportModel.ListWorkPlace = new List<CatWorkPlaceModel>();
                listDataImportModel.ListWorkPlace.AddRange(listWorkPlaceModel);
            }
            #endregion

            #region Cat_CostCentre
            var listCostCentreModel = new List<CatCostCentreModel>();
            if (listCatCostCentreEntity != null && listCatCostCentreEntity.Count > 0)
            {
                
                listCostCentreModel = listCatCostCentreEntity.Select(s => new CatCostCentreModel()
                {
                    CostCentreCode = s.Code,
                    CostCentreName = s.CostCentreName,
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    DateLock = s.DateLock,
                    GroupCost = s.GroupCost,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    IsDelete = s.IsDelete,
                    ServerCreate = s.ServerCreate,
                    ServerUpdate = s.ServerUpdate,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate,
                }).ToList();
                listDataImportModel.ListCostCentre = new List<CatCostCentreModel>();
                listDataImportModel.ListCostCentre.AddRange(listCostCentreModel);
            }
            else 
            {
                listDataImportModel.ListCostCentre = new List<CatCostCentreModel>();
                listDataImportModel.ListCostCentre.AddRange(listCostCentreModel);
            }

            #endregion

            #region Cat_Shift
            if (listCatShiftEntity != null && listCatShiftEntity.Count > 0)
            {
                var listCatShiftModel = new List<CatShiftModel>();
                listCatShiftModel = listCatShiftEntity.Select(s => new CatShiftModel()
                {
                    ShiftCode = s.Code,
                    CoBreakIn = s.CoBreakIn,
                    CoBreakOut = s.CoBreakOut,
                    CoOut = s.CoOut,
                    DateCreate = s.DateCreate,
                    DateLock = s.DateLock,
                    DateUpdate = s.DateUpdate,
                    HourStartOvertime = s.HourStartOvertime,
                    InTime = s.InTime,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    IsBreakAsWork = s.IsBreakAsWork,
                    IsDelete = s.IsDelete,
                    IsNightShift = s.IsNightShift,
                    IsNotApplyWorkHoursReal = s.IsNotApplyWorkHoursReal,
                    IsTemporary = s.IsTemporary,
                    MaxOut = s.MaxOut,
                    MinIn = s.MinIn,
                    NightTimeEnd = s.NightTimeEnd,
                    NightTimeStart = s.NightTimeStart,
                    ReduceNightShift = s.ReduceNightShift,
                    ServerCreate = s.ServerCreate,
                    ShiftBreakType = s.ShiftBreakType,
                    ShiftName = s.ShiftName,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate,
                    WorkHours = s.WorkHours
                }).ToList();
                listDataImportModel.ListCatShift = new List<CatShiftModel>();
                listDataImportModel.ListCatShift.AddRange(listCatShiftModel);
            }
            #endregion

            

            #endregion

            #region Nhân Sự

            #region Hre_Profile
            var listProfileModel = new List<HRM.Presentation.Hr.Models.Hre_ProfileModel>();
            if (listProfileEntity != null && listProfileEntity.Count > 0)
            {
                if (listProfileEntity.Select(m => m.JobTitleID) != null && listProfileEntity.Select(m => m.PositionID) != null && listProfileEntity.Select(m => m.WorkPlaceID) != null && listProfileEntity.Select(m => m.EmpTypeID) != null)
                {

                    var data = listProfileEntity.Take(500).ToList();

                    foreach (var item in data)
                    {
                        listProfileModel = data.Select(s => new HRM.Presentation.Hr.Models.Hre_ProfileModel()
                        {
                            CodeAttendance = s.CodeAttendance,
                            ProfileName = s.ProfileName,
                            DateHire = s.DateHire,
                            CodeEmp = s.CodeEmp,
                            DateApplyAttendanceCode = s.DateApplyAttendanceCode,
                            Email = s.Email,
                            IsDelete = s.IsDelete,
                            UserCreate = s.UserCreate,
                            UserUpdate = s.UserUpdate,
                            DateCreate = s.DateCreate,
                            DateUpdate = s.DateUpdate,
                            DayOfBirth = s.DayOfBirth,
                            JobtitleCode = s.JobTitleID != null ? listJobTitleEntity.Where(m => m.ID == s.JobTitleID).FirstOrDefault().JobTitleCode : string.Empty,
                            PositionCode = s.PositionID != null ? listPositionEntity.Where(m => m.ID == s.PositionID).FirstOrDefault().Code : string.Empty,
                            WorkPlaceCode = s.WorkPlaceID != null ? listWorkPlaceEntity.Where(m => m.ID == s.WorkPlaceID).FirstOrDefault().Code : string.Empty,
                            CostCentreCode = s.CostCentreID != null ? listCatCostCentreEntity.Where(m => m.ID == s.CostCentreID).FirstOrDefault().Code : string.Empty,
                            OrgStructureCode = s.OrgStructureID != null ? listOrgStructureEntity.Where(m => m.ID == s.OrgStructureID).FirstOrDefault().Code : string.Empty,
                            //    EmpTypeID = listEmployeeTypeEntity.Where(m => m.ID == s.EmpTypeID).FirstOrDefault().CodeID,
                            Weight = s.Weight,
                            CellPhone = s.Cellphone,
                            CodeTax = s.CodeTax,
                            DateEndProbation = s.DateEndProbation,
                            DateOfEffect = s.DateOfEffect,
                            DateQuit = s.DateQuit,
                            FirstName = s.FirstName,
                            MarriageStatus = s.MarriageStatus,
                            NameEnglish = s.NameEnglish,
                            MonthOfBirth = s.MonthOfBirth,
                            Gender = s.Gender,
                            Height = s.Height,
                            HomePhone = s.HomePhone,
                            IDDateOfIssue = s.IDDateOfIssue,
                            IDNo = s.IDNo,
                            IDPlaceOfIssue = s.IDPlaceOfIssue,
                            ImagePath = s.ImagePath,
                            Notes = s.Notes,
                            PassportDateOfExpiry = s.PassportDateOfExpiry,
                            PassportDateOfIssue = s.PassportDateOfIssue,
                            PassportNo = s.PassportNo,
                            PassportPlaceOfIssue = s.PassportPlaceOfIssue,
                            StatusSyn = s.StatusSyn,
                            YearOfBirth = s.YearOfBirth,
                            BusinessPhone = s.BusinessPhone,
                            RequestDate = s.RequestDate,
                            IPCreate = s.IPCreate,
                            IPUpdate = s.IPUpdate,


                        }).ToList();

                        listDataImportModel.ListProfile = new List<HRM.Presentation.Hr.Models.Hre_ProfileModel>();
                        listDataImportModel.ListProfile.AddRange(listProfileModel);
                    }
                }
            }
            else 
            {
                listDataImportModel.ListProfile = new List<HRM.Presentation.Hr.Models.Hre_ProfileModel>();
                listDataImportModel.ListProfile.AddRange(listProfileModel);
            }

            #endregion

            #endregion

            #region Chấm Công

            #region Att_WorkDay
            var listWorkdayModel = new List<Att_WorkDayModel>();
            if (listWorkdayEntity != null && listWorkdayEntity.Count > 0)
            {
                List<string> lstProfileCode = listDataImportModel.ListProfile.Select(m => m.CodeEmp).ToList();
                var data = listWorkdayEntity.Where(m => m.Hre_Profile != null && m.Hre_Profile.CodeEmp != string.Empty && lstProfileCode.Contains(m.Hre_Profile.CodeEmp)).Take(500).ToList();

                foreach (var item in data)
                {
                    listWorkdayModel = data.Select(s => new Att_WorkDayModel()
                    {
                        WorkDate = s.WorkDate,
                        WorkDuration = s.WorkDuration,
                        Type = s.Type,
                        Status = s.Status,
                        SrcType = s.SrcType,
                        LateEarlyReason = s.LateEarlyReason,
                        IsDelete = s.IsDelete,
                        InTime1 = s.InTime1,
                        InTime2 = s.InTime2,
                        InTime3 = s.InTime3,
                        InTime4 = s.InTime4,
                        FirstInTime = s.FirstInTime,
                        EarlyDuration1 = (int?)s.EarlyDuration1,
                        EarlyDuration2 = (int?)s.EarlyDuration2,
                        EarlyDuration3 = (int?)s.EarlyDuration3,
                        EarlyDuration4 = (int?)s.EarlyDuration4,
                        LastOutTime = s.LastOutTime,
                        OutTime1 = s.OutTime1,
                        OutTime2 = s.OutTime2,
                        OutTime3 = s.OutTime3,
                        OutTime4 = s.OutTime4,
                        DateCreate = s.DateCreate,
                        DateUpdate = s.DateUpdate,
                        IPCreate = s.IPCreate,
                        IPUpdate = s.IPUpdate,
                        ProfileCode = s.ProfileID != null ? listProfileEntity.Where(m => m.ID == s.ProfileID).FirstOrDefault().CodeEmp : string.Empty,
                        ShiftCode = s.ShiftID != null ? listCatShiftEntity.Where(m => m.ID == s.ShiftID).FirstOrDefault().Code : string.Empty,


                    }).ToList();
                    listDataImportModel.ListWorkDay = new List<Att_WorkDayModel>();
                    listDataImportModel.ListWorkDay.AddRange(listWorkdayModel);
                }

            }
            else 
            {
                listDataImportModel.ListWorkDay = new List<Att_WorkDayModel>();
                listDataImportModel.ListWorkDay.AddRange(listWorkdayModel);
            }

            #endregion

            #endregion

            #region Nhà Ăn

            #region MealAllowanceTypeSetting
            var listMealAllowanceTypeSettingModel = new List<Can_MealAllowanceTypeSettingModel>();
            if (listMealAllowanceTypeSettingEntity != null && listMealAllowanceTypeSettingEntity.Count > 0)
            {
                listMealAllowanceTypeSettingModel = listMealAllowanceTypeSettingEntity.Select(s => new Can_MealAllowanceTypeSettingModel()
                {
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    IsDelete = s.IsDelete,
                    MealAllowanceTypeCode = s.MealAllowanceTypeSettingCode,
                    Amount = (double?)s.Amount,
                    MealAllowanceTypeName = s.MealAllowanceTypeSettingName,
                    DateFrom = s.DateFrom,
                    DateTo = s.DateTo,
                    Note = s.Note,
                    ServerCreate = s.ServerCreate,
                    ServerUpdate = s.ServerUpdate,
                    Standard = s.Standard,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate,

                }).ToList();
                listDataImportModel.ListMealAllowanceTypeSetting = new List<Can_MealAllowanceTypeSettingModel>();
                listDataImportModel.ListMealAllowanceTypeSetting.AddRange(listMealAllowanceTypeSettingModel);
            }
            else 
            {
                listDataImportModel.ListMealAllowanceTypeSetting = new List<Can_MealAllowanceTypeSettingModel>();
                listDataImportModel.ListMealAllowanceTypeSetting.AddRange(listMealAllowanceTypeSettingModel);
            }
            
            #endregion

            #region TAMScanLog
            var listTAMScanLogReasonMissingModel = new List<Cat_TAMScanReasonMissModel>();

            if (listTAMScanReasonMissingEntity != null && listTAMScanReasonMissingEntity.Count > 0)
            {

                listTAMScanLogReasonMissingModel = listTAMScanReasonMissingEntity.Select(s => new Cat_TAMScanReasonMissModel()
               {
                   TAMScanReasonMissCode = s.Code,
                   TAMScanReasonMissName = s.TAMScanReasonMissName,
                   DateCreate = s.DateCreate,
                   DateUpdate = s.DateUpdate,
                   Description = s.Description,
                   IPCreate = s.IPCreate,
                   IPUpdate = s.IPUpdate,
                   IsDelete = s.IsDelete,
                   IsForCMS = s.IsForCMS,
                   IsFullPay = s.IsFullPay.HasValue,
                   IsRemind = s.IsRemind,
                   MealAllowanceTypeSettingCode = s.MealAllowanceTypeSettingID != null ? listMealAllowanceTypeSettingEntity.Where(m => m.ID == s.MealAllowanceTypeSettingID).FirstOrDefault().MealAllowanceTypeSettingCode : string.Empty,
                   ServerCreate = s.ServerCreate,
                   ServerUpdate = s.ServerUpdate,
                   UserCreate = s.UserCreate,
                   UserUpdate = s.UserUpdate,

               }).ToList();
                listDataImportModel.ListTAMScanReasonMissing = new List<Cat_TAMScanReasonMissModel>();
                listDataImportModel.ListTAMScanReasonMissing.AddRange(listTAMScanLogReasonMissingModel);
            }
            else 
            {
                listDataImportModel.ListTAMScanReasonMissing = new List<Cat_TAMScanReasonMissModel>();
                listDataImportModel.ListTAMScanReasonMissing.AddRange(listTAMScanLogReasonMissingModel);
            }

            #endregion

            #region MealAllowanceToMoney
            var listMealAllowanceToMoneyModel = new List<Can_MealAllowanceToMoneyModel>();
            if (listMealAllowanceToMoneyEntity != null && listMealAllowanceToMoneyEntity.Count > 0)
            {
                List<string> lstProfileCode = listDataImportModel.ListProfile.Select(m => m.CodeEmp).ToList();
                var data = listMealAllowanceToMoneyEntity.Where(m => m.Hre_Profile != null && m.Hre_Profile.CodeEmp != string.Empty && lstProfileCode.Contains(m.Hre_Profile.CodeEmp)).Take(500).ToList();

                foreach (var item in data)
                {
                    listMealAllowanceToMoneyModel = data.Select(s => new Can_MealAllowanceToMoneyModel()
                    {
                        ProfileCode = s.ProfileID != null ? listProfileEntity.Where(m => m.ID == s.ProfileID).FirstOrDefault().CodeEmp : string.Empty,
                        MealAllowanceTypeSettingCode = s.MealAllowanceTypeID != null ? listMealAllowanceTypeSettingEntity.Where(m => m.ID == s.MealAllowanceTypeID).FirstOrDefault().MealAllowanceTypeSettingCode : string.Empty,
                        DateCreate = s.DateCreate,
                        DateFrom = s.DateFrom,
                        DateTo = s.DateTo,
                        DateUpdate = s.DateUpdate,
                        IPCreate = s.IPCreate,
                        IPUpdate = s.IPUpdate,
                        IsDelete = s.IsDelete,
                        Note = s.Note,
                        ServerCreate = s.ServerCreate,
                        ServerUpdate = s.ServerUpdate,
                        UserCreate = s.UserCreate,
                        UserUpdate = s.UserUpdate,
                    }).ToList();
                    listDataImportModel.ListMealAllowanceToMoney = new List<Can_MealAllowanceToMoneyModel>();
                    listDataImportModel.ListMealAllowanceToMoney.AddRange(listMealAllowanceToMoneyModel);
                }

            }
            else 
            {
                listDataImportModel.ListMealAllowanceToMoney = new List<Can_MealAllowanceToMoneyModel>();
                listDataImportModel.ListMealAllowanceToMoney.AddRange(listMealAllowanceToMoneyModel);
            }
            
            
            #endregion

            #region TAMScanLogCMS
            var listTAMScanLogCMSModel = new List<Can_TamScanLogModel>();
            if (listTamScanLogCMSEntity != null && listTamScanLogCMSEntity.Count > 0)
            {
                listTAMScanLogCMSModel = listTamScanLogCMSEntity.Select(s => new Can_TamScanLogModel()
                {
                    CardCode = s.CardCode,
                    Comment = s.Comment,
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    IsDelete = s.IsDelete,
                    MachineCode = s.MachineCode,
                    ServerCreate = s.ServerCreate,
                    ServerUpdate = s.ServerUpdate,
                    SrcType = s.SrcType,
                    Status = s.Status,
                    TimeLog = s.TimeLog,
                    Type = s.Type,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate
                }).ToList();
                listDataImportModel.ListTAMScanLogCMS = new List<Can_TamScanLogModel>();
                listDataImportModel.ListTAMScanLogCMS.AddRange(listTAMScanLogCMSModel);
            }
            else 
            {
                listDataImportModel.ListTAMScanLogCMS = new List<Can_TamScanLogModel>();
                listDataImportModel.ListTAMScanLogCMS.AddRange(listTAMScanLogCMSModel);
            }
            #endregion


            #endregion

            #region Giặt Là

            #region TAMScanLogLMS
            var listTAMScanLogLMSModel = new List<Lau_TamScanLogModel>();
            if (listTAMScanLogLMSEntity != null && listTAMScanLogLMSEntity.Count > 0)
            {
                listTAMScanLogLMSModel = listTAMScanLogLMSEntity.Select(s => new Lau_TamScanLogModel()
                {
                    TamScanLogCode = s.Code,
                    Comment = s.Comment,
                    DateCreate = s.DateCreate,
                    DateUpdate = s.DateUpdate,
                    IPCreate = s.IPCreate,
                    IPUpdate = s.IPUpdate,
                    IsDelete = s.IsDelete,
                    TamScanLogMachineCode = s.MachineCode,
                    ServerCreate = s.ServerCreate,
                    ServerUpdate = s.ServerUpdate,
                    SrcType = s.SrcType,
                    Status = s.Status,
                    TimeLog = s.TimeLog,
                    Type = s.Type,
                    UserCreate = s.UserCreate,
                    UserUpdate = s.UserUpdate
                }).ToList();
                listDataImportModel.ListTAMScanLogLMS = new List<Lau_TamScanLogModel>();
                listDataImportModel.ListTAMScanLogLMS.AddRange(listTAMScanLogLMSModel);
            }
            else 
            {
                listDataImportModel.ListTAMScanLogLMS = new List<Lau_TamScanLogModel>();
                listDataImportModel.ListTAMScanLogLMS.AddRange(listTAMScanLogLMSModel);
            }
            #endregion
           
            #endregion


            #endregion

          

           return listDataImportModel;
            
        }
    }
}