using HRM.Data.BaseRepository;
using HRM.Data.Canteen.Model;
using HRM.Data.Main.Data.Sql.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;
using HRM.Data.Canteen.Data;
using HRM.Data.Main.Sql;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using System;
using HRM.Data.Hr.Model;
using HRM.Data.Category.Model;
using HRM.Data.Attendance.Model;
using System.Data;

namespace HRM.Business.Canteen.Domain
{
    public class Can_ReportTestServices  : BaseService
    {
        /// <summary>
        /// [Son.Vo] - 20140724 - BC Thanh Toán Trợ Cấp Ăn Ca Cho Nhân Viên
        /// </summary>
        /// <param name="CarteringIDs"></param>
        /// <param name="CanteenIDS"></param>
        /// <param name="LineIDS"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public List<Can_ReportAdjustmentMealAllowancePaymentEntity> ReportAdjustmentMealAllowancePayment(DateTime DateFrom, DateTime DateTo, List<int> lstProfileIDs)
        {
            #region GetData
            var lstMealRecord = new List<Can_MealRecord>().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceType,  s.NoWorkDay }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordRepository(unitOfWork);
                lstMealRecord = repo.FindBy(m => m.TimeLog >= DateFrom || m.TimeLog <= DateTo && m.CardCode != null).Select(
               s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceType,  s.NoWorkDay }).ToList();
            }
            var cardcode = lstMealRecord.Select(s => s.CardCode).Distinct().ToList();
            var profiles = new List<Hre_Profile>().Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                profiles = repo.FindBy(s => cardcode.Contains(s.CodeAttendance)).Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            }

            var mealAllowanceTypeSantandIDs = new List<Can_MealAllowanceTypeSetting>().Select(m=> m.Id).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                mealAllowanceTypeSantandIDs = repo.FindBy(s => s.Standard == true).Select(m=> m.Id).ToList();
            }

            var lineHDTJobIDs = new List<Can_Line>().Select(m => m.Id).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_LineRepository(unitOfWork);
                lineHDTJobIDs = repo.FindBy(s => s.LineHDTJOB != null).Select(m => m.Id).ToList();
            }

            var lines = new List<Can_Line>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_LineRepository(unitOfWork);
                lines = repo.GetAll().ToList();
            }

            var lstOrgAll = new List<Cat_OrgStructure>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_OrgStructureRepository(unitOfWork);
                lstOrgAll = repo.GetAll().ToList();
            }
            #endregion
            List<Can_ReportAdjustmentMealAllowancePaymentEntity> lstReportAdjustmentMealAllowancePayment = new List<Can_ReportAdjustmentMealAllowancePaymentEntity>();
            foreach (var profile in profiles)
            {
                Can_ReportAdjustmentMealAllowancePaymentEntity ReportAdjustmentMealAllowancePayment = new Can_ReportAdjustmentMealAllowancePaymentEntity();
                var orgbyprofile = lstOrgAll.Where(m => m.Id == profile.OrgStructureID).Select(m => m.OrgStructureName).FirstOrDefault();
                ReportAdjustmentMealAllowancePayment.CodeEmp = profile.CodeEmp;
                ReportAdjustmentMealAllowancePayment.ProfileName = profile.ProfileName;
                ReportAdjustmentMealAllowancePayment.OrgStructureName = orgbyprofile;
                var mealProfiles = lstMealRecord.Where(s => s.CardCode == profile.CodeAttendance).ToList();
                if (mealProfiles.Count > 0)
                {
                    var line = lines.FirstOrDefault(s => s.Id == mealProfiles[0].LineID);
                    if (line != null)
                    {
                        ReportAdjustmentMealAllowancePayment.TotalAdditionalAmount = mealProfiles.Count * line.Amount;
                    }
                    ReportAdjustmentMealAllowancePayment.TotalMealTime = mealProfiles.Where(m => m.MealAllowanceType != null
                        && mealAllowanceTypeSantandIDs.Contains(m.MealAllowanceType)).Count();
                    ReportAdjustmentMealAllowancePayment.TotalDeductionAmount = mealProfiles.Where(m => m.MealAllowanceType != null
                      && mealAllowanceTypeSantandIDs.Contains(m.MealAllowanceType)).Sum(m => m.Amount.Value);
                    var countCardMore1 = 0;
                    double? AmountSubtractCardMore1 = 0;
                    for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                    {
                        if (mealProfiles.Count(s => s.TimeLog == date) > 1)
                        {
                            countCardMore1++;
                            var meal = mealProfiles.FirstOrDefault(s => s.TimeLog == date);
                            if (meal != null)
                            {
                                AmountSubtractCardMore1 += meal.Amount;
                            }

                        }
                    }
                    ReportAdjustmentMealAllowancePayment.TotalScanManyTime = countCardMore1;
                    ReportAdjustmentMealAllowancePayment.TotalDeductionScanManyTime = AmountSubtractCardMore1.Value;
                    ReportAdjustmentMealAllowancePayment.TotalNotWorkButEatTime = mealProfiles.Count(s => s.NoWorkDay == true);
                    ReportAdjustmentMealAllowancePayment.TotalNotWorkButEatAmount = mealProfiles.Sum(s => s.Amount.Value);
                    ReportAdjustmentMealAllowancePayment.TotalNotWorkButEatAmount = mealProfiles.Sum(s => s.Amount.Value);
                }
                lstReportAdjustmentMealAllowancePayment.Add(ReportAdjustmentMealAllowancePayment);
            }
            return lstReportAdjustmentMealAllowancePayment;
        }
        /// <summary>
        /// [Son.Vo] - 20140724 - Lấy dữ liệu BC Tổng Hợp Suất Ăn Của Nhân Viên
        /// </summary>
        /// <param name="CarteringIDs"></param>
        /// <param name="CanteenIDS"></param>
        /// <param name="LineIDS"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public List<Can_ReportMealTimeSummaryEntity> ReportMealTimeSummary(List<int?> CarteringIDs, List<int?> CanteenIDS, List<int?> LineIDS, DateTime DateFrom, DateTime DateTo)
        {
            #region GetData
            var lstMealRecord = new List<Can_MealRecord>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordRepository(unitOfWork);
                lstMealRecord = repo.FindBy(m => (m.TimeLog >= DateFrom || m.TimeLog <= DateTo) && m.CardCode != null).ToList();
            }

            var lstCanteens = new List<Can_Canteen>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_CanteenRepository(unitOfWork);
                if (CanteenIDS != null)
                {
                    lstCanteens = repo.FindBy(s => CanteenIDS.Contains(s.Id)).ToList();
                }
                else lstCanteens = repo.GetAll().ToList();
            }

            var lstCaterings = new List<Can_Catering>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_CateringRepository(unitOfWork);
                if (CarteringIDs != null)
                {
                    lstCaterings = repo.FindBy(s => CarteringIDs.Contains(s.Id)).ToList();
                }
                else lstCaterings = repo.GetAll().ToList();
            }

            var lstLines = new List<Can_Line>().ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_LineRepository(unitOfWork);
                if (LineIDS != null)
                {
                    lstLines = repo.FindBy(s => LineIDS.Contains(s.Id)).ToList();
                }
                else lstLines = repo.GetAll().ToList();
            }
            #endregion
            List<Can_ReportMealTimeSummaryEntity> lstReportMealTimeSummary = new List<Can_ReportMealTimeSummaryEntity>();
            foreach (var cate in lstCaterings)
            {
                Can_ReportMealTimeSummaryEntity ReportAdjustmentMealAllowancePayment = new Can_ReportMealTimeSummaryEntity();
                ReportAdjustmentMealAllowancePayment.Catering = cate.CateringName;
                foreach (var item in lstCanteens)
                {
                    ReportAdjustmentMealAllowancePayment.Canteen = item.CanteenName;
                    foreach (var line in lstLines)
                    {
                        ReportAdjustmentMealAllowancePayment.Line = line.LineName;
                        ReportAdjustmentMealAllowancePayment.Price = line.Amount.Value;
                        var sum = lstMealRecord.Where(s => s.CateringID == cate.Id && s.CanteenID == cate.Id && s.LineID == line.Id).Sum(s => s.Amount);
                        var rate = lstMealRecord.Where(s => s.CateringID == cate.Id && s.LineID == line.Id).Sum(s => s.Amount) / sum;
                        ReportAdjustmentMealAllowancePayment.TotalAmount = sum != null ? sum.Value : 0;
                        ReportAdjustmentMealAllowancePayment.Rate = rate != null ? rate.Value : 0;
                        lstReportMealTimeSummary.Add(ReportAdjustmentMealAllowancePayment);
                    }
                }
            }
            return lstReportMealTimeSummary;
        }
        public List<Can_ReportMealTimeDetailNoEatEntity> ReportMealTimeDetailNoEat(List<int?> CarteringIDs, List<int?> CanteenIDS, List<int?> LineIDS, DateTime dateStart, DateTime dateEnd, List<int?> orgIDs)
        {
            var mealRecords = new List<Can_MealRecord>().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordRepository(unitOfWork);
                if (LineIDS != null)
                {
                    mealRecords = repo.FindBy(s => s.CardCode != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd && s.NoWorkDay == true)
                    .Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID })
                    .ToList();
                }
                else mealRecords = repo.GetAll().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
            }
            var workDays = new List<Att_WorkDay>().Select(s => new { s.ProfileID, s.WorkDate }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_WorkDayRepository(unitOfWork);
                workDays = repo.FindBy(s => s.ProfileID != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                .Select(s => new { s.ProfileID, s.WorkDate }).ToList();
            }
            var lstCodeAttendance = mealRecords.Select(s => s.CardCode).Distinct().ToList();
            var lstProfile = new List<Hre_Profile>().Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                lstProfile = repo.FindBy(s => lstCodeAttendance.Contains(s.CodeAttendance))
                .Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance }).ToList();
            }
            List<Cat_OrgStructure> orgs = new List<Cat_OrgStructure>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_OrgStructureRepository(unitOfWork);
                orgs = repo.FindBy(s => s.Code != null).ToList();
            }
            lstProfile = lstProfile.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
            //if (!isIncludeQuitEmp)
            //{
            //    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
            //    profileIds = profiles.Select(s => s.ID).ToList();
            //    mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
            //}
            if (CanteenIDS != null)
            {
                mealRecords = mealRecords.Where(s => s.CanteenID != null && CanteenIDS.Contains(s.CanteenID.Value)).ToList();
            }
            if (CarteringIDs != null)
            {
                mealRecords = mealRecords.Where(s => s.CateringID != null && CarteringIDs.Contains(s.CateringID.Value)).ToList();
            }
            if (LineIDS != null)
            {
                mealRecords = mealRecords.Where(s => s.LineID != null && LineIDS.Contains(s.LineID.Value)).ToList();
            }
            lstCodeAttendance = mealRecords.Select(s => s.CardCode).Distinct().ToList();
            List<Hre_Profile> profiles = new List<Hre_Profile>();
            var lstProfileId = new List<int>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                profiles = repo.FindBy(s => lstCodeAttendance.Contains(s.CodeAttendance)).ToList();
                lstProfileId = profiles.Select(s => s.Id).ToList();
            }
            workDays = workDays.Where(s => lstProfileId.Contains(s.ProfileID)).ToList();
            var orgTypes = new List<Cat_OrgStructureType>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repo.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
            }
            Can_ReportMealTimeDetailNoEatEntity reportMealTimeDetailNoEatEntity = null;
            List<Can_ReportMealTimeDetailNoEatEntity> lstreportMealTimeDetailNoEatEntity = new List<Can_ReportMealTimeDetailNoEatEntity>();
            foreach (var workDay in workDays)
            {
                var profile = profiles.FirstOrDefault(s => s.Id == workDay.ProfileID);
                if (profile != null)
                {
                    int? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.Id == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    reportMealTimeDetailNoEatEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    reportMealTimeDetailNoEatEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    reportMealTimeDetailNoEatEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    reportMealTimeDetailNoEatEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    reportMealTimeDetailNoEatEntity.CodeEmp = profile.CodeEmp;
                    reportMealTimeDetailNoEatEntity.ProfileName = profile.ProfileName;
                    reportMealTimeDetailNoEatEntity.Date = workDay.WorkDate;
                }
                lstreportMealTimeDetailNoEatEntity.Add(reportMealTimeDetailNoEatEntity);
            }
            return lstreportMealTimeDetailNoEatEntity;
        }
        /// <summary>
        /// [Tam.Le] - 20140725 - Lấy dữ liệu BC Chi Tiết Nhân Viên Không Chấm Công Nhưng Có Ăn
        /// </summary>
        /// <param name="CarteringIDs"></param>
        /// <param name="CanteenIDS"></param>
        /// <param name="LineIDS"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public List<Can_ReportMealTimeDetailEntity> ReportMealTimeDetail(List<int?> CarteringIDs, List<int?> CanteenIDS, List<int?> LineIDS, DateTime dateStart, DateTime dateEnd, List<int?> orgIDs)
        {
            var mealRecords = new List<Can_MealRecord>().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordRepository(unitOfWork);
                if (LineIDS != null)
                {
                    mealRecords = repo.FindBy(s => s.CardCode != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd && s.NoWorkDay == true)
                    .Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID })
                    .ToList();
                }
                else mealRecords = repo.GetAll().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
            }
            var lstCodeAttendance = mealRecords.Select(s => s.CardCode).Distinct().ToList();
            var lstProfile = new List<Hre_Profile>().Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                lstProfile = repo.FindBy(s => lstCodeAttendance.Contains(s.CodeAttendance))
                .Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp }).ToList();
            }
            List<Cat_OrgStructure> orgs = new List<Cat_OrgStructure>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_OrgStructureRepository(unitOfWork);
                orgs = repo.FindBy(s => s.Code != null).ToList();
            }
            lstProfile = lstProfile.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
            //if (!isIncludeQuitEmp)
            //{
            //    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
            //    profileIds = profiles.Select(s => s.ID).ToList();
            //    mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
            //}
            if (CanteenIDS != null)
            {
                mealRecords = mealRecords.Where(s => s.CanteenID != null && CanteenIDS.Contains(s.CanteenID.Value)).ToList();
            }
            if (CarteringIDs != null)
            {
                mealRecords = mealRecords.Where(s => s.CateringID != null && CarteringIDs.Contains(s.CateringID.Value)).ToList();
            }
            if (LineIDS != null)
            {
                mealRecords = mealRecords.Where(s => s.LineID != null && LineIDS.Contains(s.LineID.Value)).ToList();
            }
            lstCodeAttendance = mealRecords.Select(s => s.CardCode).Distinct().ToList();
            List<Hre_Profile> profiles = new List<Hre_Profile>();
            var lstProfileId = new List<int>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                profiles = repo.FindBy(s => lstCodeAttendance.Contains(s.CodeAttendance)).ToList();
                lstProfileId = profiles.Select(s => s.Id).ToList();
            }
            var orgTypes = new List<Cat_OrgStructureType>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repo.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
            }
            Can_ReportMealTimeDetailEntity reportMealTimeDetailEntity = null;
            List<Can_ReportMealTimeDetailEntity> lstreportMealTimeDetailEntity = new List<Can_ReportMealTimeDetailEntity>();
            foreach (var mealRecord in mealRecords)
            {
                reportMealTimeDetailEntity = new Can_ReportMealTimeDetailEntity();
                var profile = profiles.FirstOrDefault(s => s.CodeAttendance == mealRecord.CardCode);
                if (profile != null)
                {
                    int? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.Id == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    reportMealTimeDetailEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    reportMealTimeDetailEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportMealTimeDetailEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportMealTimeDetailEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    reportMealTimeDetailEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    reportMealTimeDetailEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    reportMealTimeDetailEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    reportMealTimeDetailEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    reportMealTimeDetailEntity.CodeEmp = profile.CodeEmp;
                    reportMealTimeDetailEntity.ProfileName = profile.ProfileName;
                    reportMealTimeDetailEntity.Date = mealRecord.TimeLog;
                }
                lstreportMealTimeDetailEntity.Add(reportMealTimeDetailEntity);
            }
            return lstreportMealTimeDetailEntity;
        }
        DataTable CreateReportCardNotStandSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Cartering);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Canteen);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Line);
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Date" + date.Day))
                    tb.Columns.Add("Date" + date.Day, typeof(DateTime));
                //if (!tb.Columns.Contains("SumCardMore" + date.Day))
                //    tb.Columns.Add("SumCardMore" + date.Day, typeof(DateTime));
            }
            return tb;
        }
        /// <summary>
        /// [Tam.Le] - 20140726 - Lấy dữ liệu BC Trường Hợp Quẹt Thẻ Ở Hàng Ăn Không Tiêu Chuẩn
        /// </summary>
        /// <param name="CarteringIDs"></param>
        /// <param name="CanteenIDS"></param>
        /// <param name="LineIDS"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public List<Can_ReportCardNotStandEntity> ReportCardNotStand(List<int?> cateringIDs, List<int?> canteenIDs, List<int?> lineIDs, DateTime dateStart, DateTime dateEnd, List<int?> orgIDs)
        {
            DataTable datatable = CreateReportCardNotStandSchema(dateStart, dateEnd);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));               
                var repoCan_MealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repoCan_MealRecord.FindBy(s => s.CardCode != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                 .Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount,s.MealAllowanceType }).ToList();
                //var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var codeAttendances = mealRecords.Select(s => s.CardCode).Distinct().ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => codeAttendances.Contains(s.CodeAttendance))
                 .Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp,s.CodeAttendance }).ToList();
                var repoCan_MealAllowanceTypeSetting = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var mealAllowanceTypeSantandIDs = repoCan_MealAllowanceTypeSetting.FindBy(s => s.Standard == true)
                  .Select(s => s.Id).ToList();
                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var lineHDTJobIDs = repoCan_Line.FindBy(s => s.LineHDTJOB != null)
                  .Select(s => s.Id).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();                
                if (orgIDs != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                //if (!isIncludeQuitEmp)
                //{
                //    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                //    profileIds = profiles.Select(s => s.ID).ToList();
                //    mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                //}
                if (canteenIDs != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && canteenIDs.Contains(s.CanteenID.Value)).ToList();
                }
                if (cateringIDs != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && cateringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (lineIDs != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                }                
                //for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                //{
                //    if (!table.Columns.Contains("Date" + date.Day))
                //        table.Columns.Add("Date" + date.Day, typeof(DateTime));
                //}
                mealRecords = mealRecords.Where(s => s.MealAllowanceType != null && mealAllowanceTypeSantandIDs.Contains(s.MealAllowanceType)
                            && s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)).ToList();
                profiles = profiles.Where(s => codeAttendances.Contains(s.CodeAttendance)).ToList();

                var orgTypes = new List<Cat_OrgStructureType>();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                Can_ReportCardNotStandEntity reportCardNotStandEntity = null;
                List<Can_ReportCardNotStandEntity> lstreportCardNotStandEntity = new List<Can_ReportCardNotStandEntity>();
                foreach (var profile in profiles)
                {
                    reportCardNotStandEntity = new Can_ReportCardNotStandEntity();
                    int? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.Id == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    reportCardNotStandEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    reportCardNotStandEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportCardNotStandEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportCardNotStandEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    reportCardNotStandEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    reportCardNotStandEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    reportCardNotStandEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    reportCardNotStandEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    reportCardNotStandEntity.CodeEmp = profile.CodeEmp;
                    reportCardNotStandEntity.ProfileName = profile.ProfileName;
                    //for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    //{
                    //    var meal = mealRecords.FirstOrDefault(s => s.CardCode == profile.CodeAttendance && s.TimeLog == date);
                    //    if (meal != null)
                    //    {
                    //        //row["Date" + date.Day] = (object)meal.TimeLog ?? DBNull.Value;
                    //    }
                    //}
                    reportCardNotStandEntity.CountCard = mealRecords.Count(s => s.CardCode == profile.CodeAttendance);
                    reportCardNotStandEntity.SumAmount = mealRecords.Where(s => s.CardCode == profile.CodeAttendance).Sum(s => s.Amount);
                    lstreportCardNotStandEntity.Add(reportCardNotStandEntity);
                }
                return lstreportCardNotStandEntity;
            }
        }

        DataTable CreateReportMultiSlideCardSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Cartering);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Canteen);
            tb.Columns.Add(Can_ReportMultiSlideCardEntity.FieldNames.Line);
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Date" + date.Day))
                    tb.Columns.Add("Date" + date.Day, typeof(DateTime));
                //if (!tb.Columns.Contains("SumCardMore" + date.Day))
                //    tb.Columns.Add("SumCardMore" + date.Day, typeof(DateTime));
            }
            return tb;
        }

        
        public DataTable ReportMultiSlideCard(List<int?> CarteringIDs, List<int?> CanteenIDS, List<int?> LineIDS, DateTime dateStart, DateTime dateEnd, List<int> lstProfileIds)
        {
            List<Can_ReportMultiSlideCardEntity> lstReportMultiSlideCard = new List<Can_ReportMultiSlideCardEntity>();
            DataTable datatable = CreateReportMultiSlideCardSchema(dateStart, dateEnd);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var mealRecords = new List<Can_MealRecord>().Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
                var repo = new Can_MealRecordRepository(unitOfWork);
                mealRecords = repo.FindBy(s => s.CardCode != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd && s.NoWorkDay == true)
                        .Select(s => new { s.CardCode, s.TimeLog, s.CanteenID, s.CateringID, s.LineID }).ToList();
                var lstcateringids = mealRecords.Select(m =>m.CateringID).ToList();
                var lstcanteenids = mealRecords.Select(m =>m.CanteenID).ToList();
                var lstlineids = mealRecords.Select(m => m.LineID).ToList();

                var workDays = new List<Att_WorkDay>().Select(s => new { s.ProfileID, s.WorkDate }).ToList();
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                workDays = repoWorkDay.FindBy(s => s.ProfileID != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                    .Select(s => new { s.ProfileID, s.WorkDate }).ToList();
                
                var Cardcode = mealRecords.Select(s => s.CardCode).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => Cardcode.Contains(m.CodeAttendance) && lstProfileIds.Contains(m.Id)).Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance }).ToList();

                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrg.FindBy(m => m.Code != null).ToList();

                var catering = new List<Can_Catering>().ToList();
                var repocatering = new Can_CateringRepository(unitOfWork);
                if(CarteringIDs != null && CarteringIDs.Count >0)
                {
                    catering = repocatering.FindBy(m => CarteringIDs.Contains(m.Id)).ToList();
                }
                else
                    catering = repocatering.GetAll().ToList();


                var canteen = new List<Can_Canteen>().ToList();
                var repocanteen = new Can_CanteenRepository(unitOfWork);
                if (CanteenIDS != null && CanteenIDS.Count > 0)
                {
                    canteen = repocanteen.FindBy(m => CanteenIDS.Contains(m.Id)).ToList();
                }
                else
                    canteen = repocanteen.GetAll().ToList();

                var line = new List<Can_Line>().ToList();
                var repoline = new Can_LineRepository(unitOfWork);
                if (LineIDS != null && LineIDS.Count > 0)
                {
                    line = repoline.FindBy(m => LineIDS.Contains(m.Id)).ToList();
                }
                else
                    line = repoline.GetAll().ToList();

                Can_ReportMultiSlideCardEntity ReportMultiSlideCardEntity = new Can_ReportMultiSlideCardEntity();
                foreach (var profile in profiles)
                {
                    DataRow row = datatable.NewRow();
                    var orgbyprofile = orgs.Where(m => m.Id == profile.OrgStructureID).FirstOrDefault();
                    var cateringbyprofile = catering.Where(m => lstcateringids.Contains(m.Id)).FirstOrDefault();
                    var lineprofile = line.Where(m => lstlineids.Contains(m.Id)).FirstOrDefault();
                    var canteenbyprofile = canteen.Where(m =>  lstcanteenids.Contains(m.Id)).FirstOrDefault();
                    row[Can_ReportMultiSlideCardEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Can_ReportMultiSlideCardEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Can_ReportMultiSlideCardEntity.FieldNames.OrgStructureName] = orgbyprofile.OrgStructureName;
                    row[Can_ReportMultiSlideCardEntity.FieldNames.Cartering] = cateringbyprofile.CateringName;
                    row[Can_ReportMultiSlideCardEntity.FieldNames.Canteen] = canteenbyprofile.CanteenName;
                    row[Can_ReportMultiSlideCardEntity.FieldNames.Line] = lineprofile.LineName;
                    var mealRecordProfiles = mealRecords.Where(s => s.CardCode == profile.CodeAttendance).ToList();
                    var workDayProfiles = workDays.Where(s => s.ProfileID == profile.Id).ToList();
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var meal = mealRecords.FirstOrDefault(s => s.CardCode == profile.CodeAttendance && s.TimeLog == date);
                        if (meal != null)
                        {
                            row["Date" + date.Day] = (object)meal.TimeLog ?? DBNull.Value;
                            //row["SumCardMore" + date.Day] = mealRecordProfiles.Count(s => s.TimeLog == date) - workDayProfiles.Count(s => s.WorkDate == date);
                        }
                    }
                    datatable.Rows.Add(row);
                }
            }
            return datatable;
        }
       
    }
}
