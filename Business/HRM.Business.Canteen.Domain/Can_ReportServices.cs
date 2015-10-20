using HRM.Data.BaseRepository;

using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;

using HRM.Data.Main.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using System;
using System.Data;
using HRM.Data.Entity;
namespace HRM.Business.Canteen.Domain
{
    public class Can_ReportServices : BaseService
    {
        #region BC Thanh Toán Nhà Hàng - Report CMS _20140814
        public List<Can_ReportMealTimeSummaryEntity> ReportMealTimeSummary(List<Guid> CarteringIDs, List<Guid> CanteenIDS, List<Guid> LineIDS, DateTime DateFrom, DateTime dateEndSearch, List<Guid> OrgIds)
        {
            #region GetData
            using (var context = new VnrHrmDataContext())
            {
                DateTime DateTo = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                List<Can_ReportMealTimeSummaryEntity> lstReportMealTimeSummary = new List<Can_ReportMealTimeSummaryEntity>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordRepository(unitOfWork);
                var lstMealRecord = repo.FindBy(m => m.CateringID != null && m.WorkDay >= DateFrom && m.WorkDay <= DateTo && m.ProfileID != null).ToList();
                if (lstMealRecord.Count < 0)
                {
                    return lstReportMealTimeSummary;
                }
                var lstProfileIdsByMealRecord = lstMealRecord.Select(m => m.ProfileID).ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => lstProfileIdsByMealRecord.Contains(m.ID)).ToList();
                if (OrgIds != null && OrgIds[0] != Guid.Empty)
                {
                    profiles = profiles.Where(m => m.OrgStructureID != null && OrgIds.Contains(m.OrgStructureID.Value)).ToList();
                }
                if (CanteenIDS != null && CanteenIDS[0] != Guid.Empty)
                {
                    lstMealRecord = lstMealRecord.Where(m => m.CanteenID != null && CanteenIDS.Contains(m.CanteenID.Value)).ToList();
                }
                if (CarteringIDs != null && CarteringIDs[0] != Guid.Empty)
                {
                    lstMealRecord = lstMealRecord.Where(m => m.CateringID != null && CarteringIDs.Contains(m.CateringID.Value)).ToList();
                }
                if (lstMealRecord.Count < 1)
                {
                    return lstReportMealTimeSummary;
                }
                if (LineIDS != null && LineIDS[0] != Guid.Empty)
                {
                    lstMealRecord = lstMealRecord.Where(m => m.LineID != null && LineIDS.Contains(m.LineID.Value)).ToList();
                }
                if (lstMealRecord.Count < 0)
                {
                    return lstReportMealTimeSummary;
                }
                var repoCatering = new Can_CateringRepository(unitOfWork);
                var caterings = repoCatering.GetAll().ToList();
                var repoLine = new Can_LineRepository(unitOfWork);
                var lines = repoLine.GetAll().ToList();
                var repoCanteen = new Can_CanteenRepository(unitOfWork);
                var canteens = repoCanteen.GetAll().ToList();

            #endregion

                foreach (var canLine in lines)
                {
                    var cateIDs = lstMealRecord.Where(s => s.LineID == canLine.ID).Select(s => s.CateringID).Distinct().ToList();
                    foreach (var catering in caterings.Where(s => cateIDs.Contains(s.ID)).ToList())
                    {
                        var cantenIDs = lstMealRecord.Where(s => s.LineID == canLine.ID).Select(s => s.CanteenID).Distinct().ToList();
                        foreach (var canteen in canteens.Where(s => cantenIDs.Contains(s.ID)).ToList())
                        {
                            Can_ReportMealTimeSummaryEntity ReportMealTimeSummaryEntity = new Can_ReportMealTimeSummaryEntity();
                            ReportMealTimeSummaryEntity.CateringName = catering != null ? catering.CateringName : string.Empty;
                            ReportMealTimeSummaryEntity.CanteenName = canteen != null ? canteen.CanteenName : string.Empty;
                            ReportMealTimeSummaryEntity.DateFrom = DateFrom;
                            ReportMealTimeSummaryEntity.DateTo = DateTo;
                            ReportMealTimeSummaryEntity.LineName = canLine != null ? canLine.LineName : string.Empty;
                            ReportMealTimeSummaryEntity.Price = (double)(canLine.Amount != null ? canLine.Amount.Value : 0);
                            var count = lstMealRecord.Count(s => canLine != null && (canteen != null && (catering != null && (s.CateringID == catering.ID && s.CanteenID == canteen.ID && s.LineID == canLine.ID))));
                            var sumAll = lstMealRecord.Where(s => catering != null && catering.ID == s.CateringID).Sum(s => s.Amount);
                            if (count > 0)
                            {
                                ReportMealTimeSummaryEntity.Total = count;
                            }
                            if (canLine.Amount != null)
                            {
                                var amount = count * canLine.Amount;
                                if (amount > 0)
                                    ReportMealTimeSummaryEntity.TotalAmount = (double)amount;
                                if (amount > 0)
                                {
                                    var rate = (double)(amount / sumAll);
                                    ReportMealTimeSummaryEntity.Rate = Math.Round(rate, 6);
                                }
                            }
                            ReportMealTimeSummaryEntity.DatePrint = DateTime.Now;
                            if (ReportMealTimeSummaryEntity.Rate > 0)
                                lstReportMealTimeSummary.Add(ReportMealTimeSummaryEntity);
                        }
                    }
                }
                return lstReportMealTimeSummary;
            }
        }
        #endregion

        #region BC Thanh toán trợ cấp ăn - 20140812_1
        double GetCountAmount(int countForgotCard)
        {
            double c = 0;
            double a = Math.Round((double)(countForgotCard / 3), 0);
            if (a < 3)
            {
                c = a;
            }
            else
            {
                c = a * 2 + (countForgotCard - a * 3);
            }
            return c;
        }

        public List<Can_ReportAdjustmentMealAllowancePaymentEntity> ReportAdjustmentMealAllowancePayment(DateTime DateFrom, DateTime dateEndSearch, List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                #region GetData
                List<Can_ReportAdjustmentMealAllowancePaymentEntity> lstReportAdjustmentMealAllowancePayment = new List<Can_ReportAdjustmentMealAllowancePaymentEntity>();
                // Son.Vo - 20140801 - sửa logic theo yêu cầu mới của Honda
                DateTime DateTo = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoMealRecord = new Can_MealRecordRepository(unitOfWork);
                var lstMealRecord = repoMealRecord.FindBy(m => m.WorkDay != null && m.WorkDay >= DateFrom && m.WorkDay <= DateTo && m.ProfileID != null)
                    .Select(s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.NoWorkDay }).ToList();

                if (lstMealRecord.Count < 0)
                {
                    return lstReportAdjustmentMealAllowancePayment;
                }

                var status = StatusMealAllowanceToMoney.E_APPROVED.ToString();
                #region Dư ko dùng
                //var repoAllowanceToMoney = new Can_MealAllowanceToMoneyRepository(unitOfWork);
                //var mealAllowncaToMoneyProfileIDs = repoAllowanceToMoney.FindBy(s => s.Status == status && s.ProfileID != null && DateFrom <= s.DateTo && s.DateFrom <= DateTo).ToList();
                #endregion

                var lstProfileIdsByMeal = lstMealRecord.Select(s => s.ProfileID).Distinct().ToList();
                var repoprofiles = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoprofiles.FindBy(s => lstProfileIdsByMeal.Contains(s.ID)).Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                {
                    profiles = profiles.Where(m => lstProfileIDs.Contains(m.ID)).ToList();
                }

                var repomealAllowanceType = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var mealAllowanceTypeSantandIDs = repomealAllowanceType.FindBy(s => s.Standard == true).Select(m => m.ID).ToList();
                var mealAllowanceTypes = repomealAllowanceType.FindBy(s => s.IsDelete == null).ToList();
                var amountStardand = repomealAllowanceType.FindBy(s => s.Standard == true && s.Amount != null).Select(s => s.Amount.Value).FirstOrDefault();

                var repoLineHDTJob = new Can_LineRepository(unitOfWork);
                var lineHDTJobIDs = repoLineHDTJob.FindBy(s => s.HDTJ != null).Select(m => m.ID).ToList();

                var repoHDTJob = new Hre_HDTJobRepository(unitOfWork);
                var profileHDTJs = repoHDTJob.FindBy(s => DateFrom <= s.DateTo && s.DateFrom <= DateTo)
             .Select(s => new { s.ProfileID, s.Type, s.DateFrom, s.DateTo }).ToList(); ;
                var profileHDTJIDs = profileHDTJs.Select(s => s.ProfileID).Distinct().ToList();

                string approveKey = MealRecord_Status.E_APPROVED.ToString();
                var repo_MealRecordMissing = new Can_MealRecordMissingRepository(unitOfWork);
                var mealRecordMissings = repo_MealRecordMissing.FindBy(s => s.Status == approveKey && DateFrom <= s.WorkDate && s.WorkDate <= DateTo)
                    .Select(s => new { s.ProfileID, s.WorkDate, s.MealAllowanceTypeSettingID, s.TamScanReasonMissID, s.Amount }).ToList();

                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var dateStart1 = DateFrom.AddDays(-1);
                var workDays = repoWorkDay.FindBy(s => dateStart1 <= s.WorkDate && s.WorkDate <= DateTo).Select(s => new { s.ProfileID, s.FirstInTime, s.LastOutTime, s.ShiftID, s.WorkDate }).ToList();
                var workDayProfies = workDays.Where(s => s.FirstInTime != null || s.LastOutTime != null).ToList();

                var repotamScan = new Cat_TAMScanReasonMissRepository(unitOfWork);
                var tamSans = repotamScan.GetAll().ToList();
                var tamScanNotFullPayIDs = tamSans.Where(s => s.IsFullPay == null || s.IsFullPay == false).Select(s => s.ID).ToList();

                var reposhift = new Cat_ShiftRepository(unitOfWork);
                var shifts = reposhift.GetAll().ToList();

                var lines = new List<Can_Line>().ToList();
                var repolines = new Can_LineRepository(unitOfWork);
                lines = repolines.GetAll().ToList();

                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrg.GetAll().ToList();

                var orgTypes = new List<Cat_OrgStructureType>();
                var repoorgType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoorgType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                #endregion

                foreach (var profile in profiles)
                {

                    var mealProfiles = lstMealRecord.Where(s => s.ProfileID == profile.ID).ToList();
                    #region Dư ko dùng
                    //var mealAllownProfiles = mealAllowncaToMoneyProfileIDs.Where(s => s.ProfileID == profile.ID).ToList();
                    //var mealNotStardands = lstMealRecord.Where(s => s.ProfileID == profile.ID && s.Amount > amountStardand).ToList();
                    #endregion
                    Can_ReportAdjustmentMealAllowancePaymentEntity ReportAdjustmentMealAllowancePayment = new Can_ReportAdjustmentMealAllowancePaymentEntity();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                    ReportAdjustmentMealAllowancePayment.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    ReportAdjustmentMealAllowancePayment.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    ReportAdjustmentMealAllowancePayment.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    ReportAdjustmentMealAllowancePayment.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    ReportAdjustmentMealAllowancePayment.CodeEmp = profile.CodeEmp;
                    ReportAdjustmentMealAllowancePayment.ProfileName = profile.ProfileName;
                    ReportAdjustmentMealAllowancePayment.DateFrom = DateFrom;
                    ReportAdjustmentMealAllowancePayment.DateTo = DateTo;
                    ReportAdjustmentMealAllowancePayment.DatePrint = DateTime.Now;
                    var countCard = 0;
                    var sumCard = 0.0;
                    int countCardMore1 = 0;
                    var countNotWorkButHasEat = 0;
                    var sumNotWorkButHasEat = 0.0;
                    var countmealNotStardand = 0;
                    var amountmealNotStardand = 0.0;
                    var countHDTJ = 0;
                    var amountHDTJ = 0.0;
                    var countNotStandHDTJ = 0;
                    var amountNotStandHDTJ = 0.0;
                    int amount3OnMonth = 0;

                    var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.ID).Select(s => s.WorkDate.Date).ToList();
                    for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                    {
                        var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Date == date.Date);
                        var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                        var missing = mealRecordMissings.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Value.Date == date.Date);
                        if (missing != null)
                        {
                            if (missing.MealAllowanceTypeSettingID != null && missing.TamScanReasonMissID == null)
                            {
                                countCard++;
                                sumCard += (double)(missing.Amount != null ? missing.Amount.Value : 0);
                            }
                            else if (missing.TamScanReasonMissID != null)
                            {
                                var tamscan = tamSans.FirstOrDefault(s => s.ID == missing.TamScanReasonMissID);
                                if (tamscan != null)
                                {
                                    if (tamscan.IsFullPay == true)
                                    {
                                        countCard++;
                                        sumCard += (double)amountStardand;
                                    }
                                    else
                                    {
                                        amount3OnMonth++;
                                    }
                                }
                            }
                        }
                        if (mealProfiles.Count(s => s.TimeLog != null && s.TimeLog.Value.Date == date.Date) > 1)
                        {
                            countCardMore1++;
                        }
                        var record = lstMealRecord.FirstOrDefault(s => s.TimeLog != null && s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)
                            && s.ProfileID == profile.ID && date.Date == s.TimeLog.Value.Date && !workdayProfileDates.Contains(date.Date));
                        if (record != null)
                        {
                            countNotWorkButHasEat++;
                            sumNotWorkButHasEat += (double)(record.Amount != null ? record.Amount.Value : 0);
                        }
                        var meal = lstMealRecord.FirstOrDefault(s => s.TimeLog != null && s.Amount > amountStardand && s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)
                            && s.ProfileID == profile.ID && s.TimeLog.Value.Date == date.Date && workdayProfileDates.Contains(date.Date));
                        if (meal != null)
                        {
                            countmealNotStardand++;
                            amountmealNotStardand += (double)(meal.Amount != null ? meal.Amount.Value : 0);
                        }
                        var mealHDT = lstMealRecord.FirstOrDefault(s => s.ProfileID == profile.ID && s.LineID != null && lineHDTJobIDs.Contains(s.LineID.Value) && s.TimeLog != null && s.TimeLog.Value.Date == date.Date
                           && (!profileHDTJIDs.Contains(s.ProfileID.Value) || (profileHDTJIDs.Contains(s.ProfileID.Value) && !workdayProfileDates.Contains(date.Date))));
                        if (mealHDT != null)
                        {
                            countHDTJ++;
                            amountHDTJ += (double)(mealHDT.Amount != null ? mealHDT.Amount.Value : 0);
                        }
                        var mealNotStandandHDT = lstMealRecord.FirstOrDefault(s => s.ProfileID != null && s.ProfileID == profile.ID && s.LineID != null && lineHDTJobIDs.Contains(s.LineID.Value) && s.TimeLog != null && s.TimeLog.Value.Date == date.Date
                           && profileHDTJIDs.Contains(s.ProfileID.Value) && workdayProfileDates.Contains(date.Date));
                        if (mealNotStandandHDT != null && mealNotStandandHDT.LineID != null)
                        {
                            var line = lines.FirstOrDefault(s => s.ID == mealNotStandandHDT.LineID);
                            var hdtjob = profileHDTJs.FirstOrDefault(s => s.Type != null && s.ProfileID == profile.ID && s.DateFrom.Value.Date <= date.Date && date.Date <= s.DateTo.Value.Date);
                            if (line != null && line.HDTJ != null && hdtjob.Type != line.HDTJ)
                            {
                                var lineHDT = lines.FirstOrDefault(s => s.HDTJ == hdtjob.Type);
                                if (lineHDT != null && lineHDT.Amount != null && lineHDT.Amount < line.Amount)
                                {
                                    countNotStandHDTJ++;
                                    var lineAmount = line.Amount != null ? line.Amount.Value : 0;
                                    var lineAmountHDT = lineHDT.Amount != null ? lineHDT.Amount.Value : 0;
                                    amountNotStandHDTJ += (double)(lineAmount - lineAmountHDT);
                                }
                            }
                        }
                        double countMiss = mealRecordMissings.Count(s => s.ProfileID == profile.ID && s.TamScanReasonMissID != null && tamScanNotFullPayIDs.Contains(s.TamScanReasonMissID.Value));
                        countMiss = GetCountAmount((int)countMiss);
                        if (countCard > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.TotalMealAllowance = countCard + countMiss;
                        }
                        if (sumCard > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.SumAmount = (double)(sumCard + countMiss * (double)amountStardand);
                        }
                        if (countmealNotStardand > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.CountEatNotStandar = countmealNotStardand;
                            ReportAdjustmentMealAllowancePayment.AmountEatNotStandar = amountmealNotStardand - (double)(amountStardand * countmealNotStardand);
                        }
                        if (countCardMore1 > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.CountCardMore = countCardMore1;
                            ReportAdjustmentMealAllowancePayment.SumCardMore = (double)Math.Round(countCardMore1 * amountStardand, 2);
                        }
                        if (countNotWorkButHasEat > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.CountNotWorkHasEat = countNotWorkButHasEat;
                            ReportAdjustmentMealAllowancePayment.AmountNotWorkHasEat = sumNotWorkButHasEat;
                        }
                        amount3OnMonth = amount3OnMonth - (int)GetCountAmount(amount3OnMonth);
                        if (amount3OnMonth > 0)
                            ReportAdjustmentMealAllowancePayment.Amount3OnMonth = amount3OnMonth; if (countHDTJ > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.CountHDTJob = countHDTJ;
                            ReportAdjustmentMealAllowancePayment.AmounSubtractHDTJob = amountHDTJ;
                        }
                        if (countNotStandHDTJ > 0)
                        {
                            ReportAdjustmentMealAllowancePayment.CountCardWorngStandar = countNotStandHDTJ;
                            ReportAdjustmentMealAllowancePayment.AmountSubtractWorngStandar = amountNotStandHDTJ;
                        }
                    }
                    lstReportAdjustmentMealAllowancePayment.Add(ReportAdjustmentMealAllowancePayment);
                }
                return lstReportAdjustmentMealAllowancePayment;
            }
        }

        public bool SaveSumryMealRecord(Guid CutOffDurationID, List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    #region GetData
                    List<Can_ReportAdjustmentMealAllowancePaymentEntity> lstReportAdjustmentMealAllowancePayment = new List<Can_ReportAdjustmentMealAllowancePaymentEntity>();
            
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoMealRecord = new Can_MealRecordRepository(unitOfWork);
                    var cutoffdurationRepository = new Att_CutOffDurationRepository(unitOfWork);
                    var Cutoffduration = cutoffdurationRepository.FindBy(m => m.ID == CutOffDurationID && m.IsDelete != true).FirstOrDefault();
                    DateTime DateFrom = new DateTime();
                    DateTime DateTo = new DateTime();
                    if (Cutoffduration != null)
                    {
                        DateFrom = Cutoffduration.DateStart;
                        DateTo = Cutoffduration.DateEnd.AddDays(1).AddMilliseconds(-1);
                    }
                    var sumryMealRecordRepository = new Can_SumryMealRecordRepository(unitOfWork);
                    var lstMealRecord = repoMealRecord.FindBy(m => m.WorkDay != null && m.WorkDay >= DateFrom && m.WorkDay <= DateTo && m.ProfileID != null)
                        .Select(s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.NoWorkDay }).ToList();

                    if (lstMealRecord.Count < 0)
                    {
                        return true;
                    }

                    var status = StatusMealAllowanceToMoney.E_APPROVED.ToString();
                    #region Dư ko dùng
                    //var repoAllowanceToMoney = new Can_MealAllowanceToMoneyRepository(unitOfWork);
                    //var mealAllowncaToMoneyProfileIDs = repoAllowanceToMoney.FindBy(s => s.Status == status && s.ProfileID != null && DateFrom <= s.DateTo && s.DateFrom <= DateTo).ToList();
                    #endregion

                    var lstProfileIdsByMeal = lstMealRecord.Select(s => s.ProfileID).Distinct().ToList();
                    var repoprofiles = new Hre_ProfileRepository(unitOfWork);
                    var profiles = repoprofiles.FindBy(s => lstProfileIdsByMeal.Contains(s.ID)).Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeAttendance, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                    if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                    {
                        profiles = profiles.Where(m => lstProfileIDs.Contains(m.ID)).ToList();
                    }

                    var repomealAllowanceType = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                    var mealAllowanceTypeSantandIDs = repomealAllowanceType.FindBy(s => s.Standard == true).Select(m => m.ID).ToList();
                    var mealAllowanceTypes = repomealAllowanceType.FindBy(s => s.IsDelete == null).ToList();
                    var amountStardand = repomealAllowanceType.FindBy(s => s.Standard == true && s.Amount != null).Select(s => s.Amount.Value).FirstOrDefault();

                    var repoLineHDTJob = new Can_LineRepository(unitOfWork);
                    var lineHDTJobIDs = repoLineHDTJob.FindBy(s => s.HDTJ != null).Select(m => m.ID).ToList();

                    var repoHDTJob = new Hre_HDTJobRepository(unitOfWork);
                    var profileHDTJs = repoHDTJob.FindBy(s => DateFrom <= s.DateTo && s.DateFrom <= DateTo)
                        .Select(s => new { s.ProfileID, s.Type, s.DateFrom, s.DateTo }).ToList(); 
                    var profileHDTJIDs = profileHDTJs.Select(s => s.ProfileID).Distinct().ToList();

                    string approveKey = MealRecord_Status.E_APPROVED.ToString();
                    var repo_MealRecordMissing = new Can_MealRecordMissingRepository(unitOfWork);
                    var mealRecordMissings = repo_MealRecordMissing.FindBy(s => s.Status == approveKey && DateFrom <= s.WorkDate && s.WorkDate <= DateTo)
                        .Select(s => new { s.ProfileID, s.WorkDate, s.MealAllowanceTypeSettingID, s.TamScanReasonMissID, s.Amount }).ToList();

                    var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                    var dateStart1 = DateFrom.AddDays(-1);
                    var workDays = repoWorkDay.FindBy(s => dateStart1 <= s.WorkDate && s.WorkDate <= DateTo).Select(s => new { s.ProfileID, s.FirstInTime, s.LastOutTime, s.ShiftID, s.WorkDate }).ToList();
                    var workDayProfies = workDays.Where(s => s.FirstInTime != null || s.LastOutTime != null).ToList();

                    var repotamScan = new Cat_TAMScanReasonMissRepository(unitOfWork);
                    var tamSans = repotamScan.GetAll().ToList();
                    var tamScanNotFullPayIDs = tamSans.Where(s => s.IsFullPay == null || s.IsFullPay == false).Select(s => s.ID).ToList();

                    var reposhift = new Cat_ShiftRepository(unitOfWork);
                    var shifts = reposhift.GetAll().ToList();

                    var lines = new List<Can_Line>().ToList();
                    var repolines = new Can_LineRepository(unitOfWork);
                    lines = repolines.GetAll().ToList();

                    var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                    var orgs = repoOrg.GetAll().ToList();

                    var orgTypes = new List<Cat_OrgStructureType>();
                    var repoorgType = new Cat_OrgStructureTypeRepository(unitOfWork);
                    orgTypes = repoorgType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                    #endregion

                    foreach (var profile in profiles)
                    {
                      
                        var mealProfiles = lstMealRecord.Where(s => s.ProfileID == profile.ID).ToList();
                        #region Dư ko dùng
                        //var mealAllownProfiles = mealAllowncaToMoneyProfileIDs.Where(s => s.ProfileID == profile.ID).ToList();
                        //var mealNotStardands = lstMealRecord.Where(s => s.ProfileID == profile.ID && s.Amount > amountStardand).ToList();
                        #endregion
                        Can_ReportAdjustmentMealAllowancePaymentEntity ReportAdjustmentMealAllowancePayment = new Can_ReportAdjustmentMealAllowancePaymentEntity();
                        Guid? orgId = profile.OrgStructureID;
                        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                        ReportAdjustmentMealAllowancePayment.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        ReportAdjustmentMealAllowancePayment.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        ReportAdjustmentMealAllowancePayment.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        ReportAdjustmentMealAllowancePayment.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        ReportAdjustmentMealAllowancePayment.CodeEmp = profile.CodeEmp;
                        ReportAdjustmentMealAllowancePayment.ProfileName = profile.ProfileName;
                        ReportAdjustmentMealAllowancePayment.DateFrom = DateFrom;
                        ReportAdjustmentMealAllowancePayment.DateTo = DateTo;
                        ReportAdjustmentMealAllowancePayment.DatePrint = DateTime.Now;
                        var countCard = 0;
                        var sumCard = 0.0;
                        int countCardMore1 = 0;
                        var countNotWorkButHasEat = 0;
                        var sumNotWorkButHasEat = 0.0;
                        var countmealNotStardand = 0;
                        var amountmealNotStardand = 0.0;
                        var countHDTJ = 0;
                        var amountHDTJ = 0.0;
                        var countNotStandHDTJ = 0;
                        var amountNotStandHDTJ = 0.0;
                        int amount3OnMonth = 0;
                        var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.ID).Select(s => s.WorkDate.Date).ToList();
                        for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                        {
                            var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Date == date.Date);
                            var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                            var missing = mealRecordMissings.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Value.Date == date.Date);
                            if (missing != null)
                            {
                                if (missing.MealAllowanceTypeSettingID != null && missing.TamScanReasonMissID == null)
                                {
                                    countCard++;
                                    sumCard += (double)(missing.Amount != null ? missing.Amount.Value : 0);
                                }
                                else if (missing.TamScanReasonMissID != null)
                                {
                                    var tamscan = tamSans.FirstOrDefault(s => s.ID == missing.TamScanReasonMissID);
                                    if (tamscan != null)
                                    {
                                        if (tamscan.IsFullPay == true)
                                        {
                                            countCard++;
                                            sumCard += (double)amountStardand;
                                        }
                                        else
                                        {
                                            amount3OnMonth++;
                                        }
                                    }
                                }
                            }
                            if (mealProfiles.Count(s => s.TimeLog != null && s.TimeLog.Value.Date == date.Date) > 1)
                            {
                                countCardMore1++;
                            }
                            var record = lstMealRecord.FirstOrDefault(s => s.TimeLog != null && s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)
                                && s.ProfileID == profile.ID && date.Date == s.TimeLog.Value.Date && !workdayProfileDates.Contains(date.Date));
                            if (record != null)
                            {
                                countNotWorkButHasEat++;
                                sumNotWorkButHasEat += (double)(record.Amount != null ? record.Amount.Value : 0);
                            }
                            var meal = lstMealRecord.FirstOrDefault(s => s.TimeLog != null && s.Amount > amountStardand && s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)
                                && s.ProfileID == profile.ID && s.TimeLog.Value.Date == date.Date && workdayProfileDates.Contains(date.Date));
                            if (meal != null)
                            {
                                countmealNotStardand++;
                                amountmealNotStardand += (double)(meal.Amount != null ? meal.Amount.Value : 0);
                            }
                            var mealHDT = lstMealRecord.FirstOrDefault(s => s.ProfileID == profile.ID && s.LineID != null && lineHDTJobIDs.Contains(s.LineID.Value) && s.TimeLog != null && 
                                s.TimeLog.Value.Date == date.Date && (!profileHDTJIDs.Contains(s.ProfileID.Value) || (profileHDTJIDs.Contains(s.ProfileID.Value) && !workdayProfileDates.Contains(date.Date))));
                            if (mealHDT != null)
                            {
                                countHDTJ++;
                                amountHDTJ += (double)(mealHDT.Amount != null ? mealHDT.Amount.Value : 0);
                            }
                            var mealNotStandandHDT = lstMealRecord.FirstOrDefault(s => s.ProfileID != null && s.ProfileID == profile.ID && s.LineID != null && lineHDTJobIDs.Contains(s.LineID.Value) && 
                                s.TimeLog != null && s.TimeLog.Value.Date == date.Date && profileHDTJIDs.Contains(s.ProfileID.Value) && workdayProfileDates.Contains(date.Date));
                            if (mealNotStandandHDT != null && mealNotStandandHDT.LineID != null)
                            {
                                var line = lines.FirstOrDefault(s => s.ID == mealNotStandandHDT.LineID);
                                var hdtjob = profileHDTJs.FirstOrDefault(s => s.Type != null && s.ProfileID == profile.ID && s.DateFrom.Value.Date <= date.Date && date.Date <= s.DateTo.Value.Date);
                                if (line != null && line.HDTJ != null && hdtjob.Type != line.HDTJ)
                                {
                                    var lineHDT = lines.FirstOrDefault(s => s.HDTJ == hdtjob.Type);
                                    if (lineHDT != null && lineHDT.Amount != null && lineHDT.Amount < line.Amount)
                                    {
                                        countNotStandHDTJ++;
                                        var lineAmount = line.Amount != null ? line.Amount.Value : 0;
                                        var lineAmountHDT = lineHDT.Amount != null ? lineHDT.Amount.Value : 0;
                                        amountNotStandHDTJ += (double)(lineAmount - lineAmountHDT);
                                    }
                                }
                            }
                            double countMiss = mealRecordMissings.Count(s => s.ProfileID == profile.ID && s.TamScanReasonMissID != null && tamScanNotFullPayIDs.Contains(s.TamScanReasonMissID.Value));
                            countMiss = GetCountAmount((int)countMiss);
                            if (countCard > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.TotalMealAllowance = countCard + countMiss;
                            }
                            if (sumCard > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.SumAmount = (double)(sumCard + countMiss * (double)amountStardand);
                            }
                            if (countmealNotStardand > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.CountEatNotStandar = countmealNotStardand;
                                ReportAdjustmentMealAllowancePayment.AmountEatNotStandar = amountmealNotStardand - (double)(amountStardand * countmealNotStardand);
                            }
                            if (countCardMore1 > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.CountCardMore = countCardMore1;
                                ReportAdjustmentMealAllowancePayment.SumCardMore = (double)Math.Round(countCardMore1 * amountStardand, 2);
                            }
                            if (countNotWorkButHasEat > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.CountNotWorkHasEat = countNotWorkButHasEat;
                                ReportAdjustmentMealAllowancePayment.AmountNotWorkHasEat = sumNotWorkButHasEat;
                            }
                            amount3OnMonth = amount3OnMonth - (int)GetCountAmount(amount3OnMonth);
                            if (amount3OnMonth > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.Amount3OnMonth = amount3OnMonth;
                            }
                            if (countHDTJ > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.CountHDTJob = countHDTJ;
                                ReportAdjustmentMealAllowancePayment.AmounSubtractHDTJob = amountHDTJ;
                            }
                            if (countNotStandHDTJ > 0)
                            {
                                ReportAdjustmentMealAllowancePayment.CountCardWorngStandar = countNotStandHDTJ;
                                ReportAdjustmentMealAllowancePayment.AmountSubtractWorngStandar = amountNotStandHDTJ;
                            }
                        }
                        Can_SumryMealRecord sumryMealRecordNew = new Can_SumryMealRecord();
                        sumryMealRecordNew.ProfileID = profile.ID;
                        sumryMealRecordNew.DateFrom = Cutoffduration.DateStart;
                        sumryMealRecordNew.DateTo = Cutoffduration.DateEnd;
                        sumryMealRecordNew.TotalMealAllowance = ReportAdjustmentMealAllowancePayment.TotalMealAllowance;
                        sumryMealRecordNew.SumAmount = ReportAdjustmentMealAllowancePayment.SumAmount;
                        sumryMealRecordNew.CountEatNotStandar = ReportAdjustmentMealAllowancePayment.CountEatNotStandar;
                        sumryMealRecordNew.AmountEatNotStandar = ReportAdjustmentMealAllowancePayment.AmountEatNotStandar;
                        sumryMealRecordNew.CountCardMore = ReportAdjustmentMealAllowancePayment.CountCardMore;
                        sumryMealRecordNew.SumAmountCardMore = ReportAdjustmentMealAllowancePayment.SumCardMore;
                        sumryMealRecordNew.CountNotWorkHasEat = ReportAdjustmentMealAllowancePayment.CountNotWorkHasEat;
                        sumryMealRecordNew.AmountNotWorkHasEat = ReportAdjustmentMealAllowancePayment.AmountNotWorkHasEat;
                        sumryMealRecordNew.Amount3OnMonth = ReportAdjustmentMealAllowancePayment.Amount3OnMonth;
                        sumryMealRecordNew.CountHDTJob = ReportAdjustmentMealAllowancePayment.CountHDTJob;
                        sumryMealRecordNew.AmountHDTJob = ReportAdjustmentMealAllowancePayment.AmounSubtractHDTJob;
                        sumryMealRecordNew.AmountNotWorkButHasHDT = ReportAdjustmentMealAllowancePayment.AmountNotWorkButHasHDT;
                        sumryMealRecordNew.CountSubtractWorngStandarHDT = ReportAdjustmentMealAllowancePayment.CountSubtractWorngStandarHDT;
                        sumryMealRecordNew.AmountSubtractWorngStandarHDT = ReportAdjustmentMealAllowancePayment.AmountSubtractWorngStandarHDT;
                        sumryMealRecordRepository.Add(sumryMealRecordNew);
                        lstReportAdjustmentMealAllowancePayment.Add(ReportAdjustmentMealAllowancePayment);

                        if (profiles.IndexOf(profile) % 1000 == 0)
                        {
                            sumryMealRecordRepository.SaveChanges();
                        }
                    }
                    sumryMealRecordRepository.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region BC Quẹt Cửa Ăn Không Tiêu Chuẩn - Report CMS _20140814
        public DataTable ReportCardNotStand(List<Guid?> cateringIDs, List<Guid?> canteenIDs, List<Guid?> lineIDs, DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs)
        {
            DataTable datatable = CreateReportCardNotStandSchema(dateStart, dateEndSearch);
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_MealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repoCan_MealRecord.FindBy(s => s.ProfileID != null && s.WorkDay != null && dateStart <= s.WorkDay && s.WorkDay <= dateEnd)
               .Select(s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.WorkDay }).ToList();
                if (mealRecords.Count < 1)
                {
                    return datatable;
                }
                var Profileids = mealRecords.Select(s => s.ProfileID).ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(s => Profileids.Contains(s.ID)).ToList();

                var profileIdsByMealrecords = mealRecords.Where(m => m.ProfileID != null).Select(s => s.ProfileID.Value).ToList();
                var amountByMealrecords = mealRecords.Select(m => m.Amount).Distinct().ToList();

                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var workDayProfies = repoWorkDay.FindBy(s => s.ProfileID != null && s.WorkDate != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd &&
                    profileIdsByMealrecords.Contains(s.ProfileID)).ToList();

                var repoAllowanceType = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var mealAllowanceTypeStandard = repoAllowanceType.FindBy(s => s.Standard == true).FirstOrDefault();
                var amountStandar = mealAllowanceTypeStandard != null ? mealAllowanceTypeStandard.Amount : null;

                var repoline = new Can_LineRepository(unitOfWork);
                var lineHDTJobIDs = repoline.FindBy(s => s.HDTJ != null).Select(s => s.ID).ToList();
                var linIdNotAmoutStardands = repoline.FindBy(s => s.Amount != amountStandar).Select(s => s.ID).ToList();

                var repoOrgs = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrgs.FindBy(s => s.Code != null).ToList();

                if (orgIDs != null && orgIDs[0] != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }

                if (canteenIDs != null && canteenIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && canteenIDs.Contains(s.CanteenID.Value)).ToList();
                }
                if (cateringIDs != null && cateringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && cateringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (lineIDs != null && lineIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                }
                if (linIdNotAmoutStardands.Count > 0)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && linIdNotAmoutStardands.Contains(s.LineID.Value)).ToList();
                }
                if (lineHDTJobIDs.Count > 0)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)).ToList();
                }
                if (mealRecords.Count < 1)
                {
                    return datatable;
                }
                var repocanteen = new Can_CanteenRepository(unitOfWork);
                var canteens = repocanteen.GetAll().ToList();
                var repocatering = new Can_CateringRepository(unitOfWork);
                var caterings = repocatering.GetAll().ToList();
                var lines = repoline.GetAll().ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();

                var approve = LeaveDayStatus.E_APPROVED.ToString();
                var repoleavedays = new Att_LeavedayRepository(unitOfWork);
                var leavedays = repoleavedays.FindBy(s => s.Status == approve && dateStart <= s.DateEnd
                     && s.DateStart <= dateEnd && profileIdsByMealrecords.Contains(s.ProfileID)).Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();

                foreach (var profile in profiles)
                {
                    var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.ID).Select(s => s.WorkDate.Date).ToList();
                    var dateLeveDays = new List<DateTime>();
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leaveDay = leavedays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        if (leaveDay != null)
                        {
                            dateLeveDays.Add(date.Date);
                        }
                    }
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leaveDay = leavedays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        var meal = mealRecords.FirstOrDefault(s => s.ProfileID == profile.ID && s.TimeLog != null && s.WorkDay != null
                            && s.TimeLog.Value.Date == date.Date && workdayProfileDates.Contains(s.WorkDay.Value.Date) && leaveDay == null);
                        if (meal != null)
                        {
                            DataRow row = datatable.NewRow();
                            Guid? orgId = profile != null ? profile.OrgStructureID : null;
                            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                            row[Can_ReportCardNotStandEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Can_ReportCardNotStandEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            row[Can_ReportCardNotStandEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Can_ReportCardNotStandEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            row[Can_ReportCardNotStandEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Can_ReportCardNotStandEntity.FieldNames.ProfileName] = profile.ProfileName;
                            row[Can_ReportCardNotStandEntity.FieldNames.DateFrom] = dateStart;
                            row[Can_ReportCardNotStandEntity.FieldNames.DateTo] = dateEnd;
                            row[Can_ReportCardNotStandEntity.FieldNames.DatePrint] = DateTime.Now;
                            var cate = caterings.FirstOrDefault(s => s.ID == meal.CateringID);
                            var cateen = canteens.FirstOrDefault(s => s.ID == meal.CanteenID);
                            var line = lines.FirstOrDefault(s => s.ID == meal.LineID);
                            if (meal.WorkDay != null)
                            {
                                row["Data" + date.Day] = meal.WorkDay.Value.ToString(ConstantFormat.HRM_Format_Grid_ShortTime);
                            }
                            row[Can_ReportCardNotStandEntity.FieldNames.CateringName] = cate != null ? cate.CateringName : string.Empty; ;
                            row[Can_ReportCardNotStandEntity.FieldNames.CanteenName] = cateen != null ? cateen.CanteenName : string.Empty; ;
                            row[Can_ReportCardNotStandEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                            var countcard = mealRecords.Count(s => s.WorkDay != null && s.ProfileID == profile.ID && workdayProfileDates.Contains(s.WorkDay.Value.Date)
                                && !dateLeveDays.Contains(s.WorkDay.Value.Date));
                            if (countcard != null)
                            {
                                row[Can_ReportCardNotStandEntity.FieldNames.CountCard] = countcard;
                                var SumAmount = mealRecords.Where(s => s.WorkDay != null && s.ProfileID == profile.ID && workdayProfileDates.Contains(s.WorkDay.Value.Date) &&
                                    !dateLeveDays.Contains(s.WorkDay.Value.Date)).Sum(s => s.Amount) - countcard * amountStandar;
                                row[Can_ReportCardNotStandEntity.FieldNames.SumAmount] = SumAmount;
                            }
                            datatable.Rows.Add(row);
                        }
                    }
                }
                return datatable;
            }
        }

        DataTable CreateReportCardNotStandSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.SectionName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.CountCard, typeof(int));
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.SumAmount, typeof(double));
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.UserPrint);
            tb.Columns.Add(Can_ReportCardNotStandEntity.FieldNames.DatePrint, typeof(DateTime));
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Data" + date.Day))
                    tb.Columns.Add("Data" + date.Day);
            }
            return tb;
        }
        #endregion

        #region BC không chấm công có ăn - Report CMS _20140814
        public DataTable ReportMealTimeDetail(List<Guid?> CarteringIDs, List<Guid?> CanteenIDS, List<Guid?> LineIDS, DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                DataTable datatable = CreateReportMealTimeDetailSchema(dateStart, dateEnd);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoMealRecord = new Can_MealRecordRepository(unitOfWork);
                var repoLine = new Can_LineRepository(unitOfWork);
                var lineHDTJobIDs = repoLine.FindBy(s => s.HDTJ != null).Select(s => s.ID).ToList();
                var mealRecords = repoMealRecord.FindBy(s => s.LineID != null && !lineHDTJobIDs.Contains(s.LineID.Value)
                    && s.ProfileID != null && dateStart <= s.WorkDay && s.WorkDay <= dateEnd).ToList();
                if (mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var workDayProfies = repoWorkDay.FindBy(s => s.ProfileID != null && dateStart <= s.WorkDate
                  && s.WorkDate <= dateEnd && profileIds.Contains(s.ProfileID)).ToList();

                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(s => profileIds.Contains(s.ID)).ToList();

                var repoLeaveday = new Att_LeavedayRepository(unitOfWork);
                var leavedays = repoLeaveday.FindBy(s => dateStart <= s.DateEnd
                 && s.DateStart <= dateEnd && profileIds.Contains(s.ProfileID)).
                 Select(s => new { s.ProfileID, s.DateStart, s.DateEnd, s.LeaveDayTypeID }).ToList();

                var repoleaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leaveDayTypes = repoleaveDayType.GetAll().Select(s => new { s.ID, s.LeaveDayTypeName }).ToList();

                List<Cat_OrgStructure> orgs = new List<Cat_OrgStructure>();
                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                orgs = repoOrg.FindBy(s => s.Code != null).ToList();
                if (orgIDs != null && orgIDs.Count > 0)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                if (CanteenIDS != null && CanteenIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && CanteenIDS.Contains(s.CanteenID.Value)).ToList();
                }
                if (CarteringIDs != null && CarteringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && CarteringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (LineIDS != null && LineIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && LineIDS.Contains(s.LineID.Value)).ToList();
                }
                if (mealRecords.Count < 1)
                {
                    return datatable;
                }
                profileIds = mealRecords.Select(s => s.ProfileID.Value).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();

                var repoorgType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoorgType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();

                var repoCanteen = new Can_CanteenRepository(unitOfWork);
                var repoCatering = new Can_CateringRepository(unitOfWork);
                var canteens = repoCanteen.GetAll().ToList();
                var caterings = repoCatering.GetAll().ToList();
                var lines = repoLine.GetAll().ToList();
                #region Code Cũ
                //foreach (var profile in profiles)
                //{
                //    var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.Id).Select(s => s.WorkDate.Date).ToList();
                //    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                //    {
                //        var record = mealRecords.FirstOrDefault(s => s.ProfileID == profile.Id && date == s.TimeLog.Date && !workdayProfileDates.Contains(date.Date));
                //        if (record != null)
                //        {
                //            DataRow row = datatable.NewRow();
                //            int? orgId = profile.OrgStructureID;
                //            var org = orgs.FirstOrDefault(s => s.Id == profile.OrgStructureID);
                //            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                //            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                //            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                //            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.ProfileName] = profile.ProfileName;
                //            var line = repoLine.FindBy(m => record.LineID == m.Id).FirstOrDefault();
                //            var canteen = repoCanteen.FindBy(m => record.CanteenID == m.Id).FirstOrDefault();
                //            var catering = repoCatering.FindBy(m => record.CateringID == m.Id).FirstOrDefault();
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.CateringName] = catering != null ? catering.CateringName : null;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.CanteenName] = canteen != null ? canteen.CanteenName : null;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.LineName] = line != null ? line.LineName : null;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.DateFrom] = dateStart;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.DateTo] = dateEnd;
                //            row[Can_ReportMealTimeDetailEntity.FieldNames.DatePrint] = DateTime.Now;
                //            if (record.TimeLog != null)
                //            {
                //                row["Data" + date.Day] = record.TimeLog.ToString(ConstantFormat.HRM_Format_Grid_ShortTime);
                //            }
                //            datatable.Rows.Add(row);
                //        }
                //    }
                //} 
                #endregion
                foreach (var profile in profiles)
                {
                    var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.ID && (s.FirstInTime != null || s.LastOutTime != null)).Select(s => s.WorkDate.Date).ToList();
                    var mealRecordProfiles = mealRecords.Where(s => s.ProfileID == profile.ID).ToList();
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leaday = leavedays.FirstOrDefault(s => s.DateStart != null && s.DateEnd != null && s.ProfileID == profile.ID && s.DateStart.Date <= date.Date && date.Date <= s.DateEnd.Date);
                        var record = mealRecordProfiles.FirstOrDefault(s => date.Date == s.WorkDay.Value.Date && (!workdayProfileDates.Contains(s.WorkDay.Value.Date) || leaday != null));
                        if (record != null)
                        {
                            DataRow row = datatable.NewRow();
                            Guid? orgId = profile.OrgStructureID;
                            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                            row[Can_ReportMealTimeDetailEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.ProfileName] = profile.ProfileName;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.Date] = record.WorkDay;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.DateFrom] = dateStart;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.DateTo] = dateEnd;
                            var cate = caterings.FirstOrDefault(s => s.ID == record.CateringID);
                            var cateen = canteens.FirstOrDefault(s => s.ID == record.CanteenID);
                            var line = lines.FirstOrDefault(s => s.ID == record.LineID);
                            row[Can_ReportMealTimeDetailEntity.FieldNames.CateringName] = cate != null ? cate.CateringName : string.Empty; ;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.CanteenName] = cateen != null ? cateen.CanteenName : string.Empty; ;
                            row[Can_ReportMealTimeDetailEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                            if (record.TimeLog != null)
                            {
                                row["Data" + record.WorkDay.Value.Day] = record.TimeLog == null ? null : (object)record.TimeLog.Value.ToString(ConstantFormat.HRM_Format_Grid_ShortTime) ?? DBNull.Value;
                            }
                            row[Can_ReportMealTimeDetailEntity.FieldNames.DatePrint] = DateTime.Now;
                            if (leaday != null)
                            {
                                var leadayType = leaveDayTypes.FirstOrDefault(s => s.ID == leaday.LeaveDayTypeID);
                                row[Can_ReportMealTimeDetailEntity.FieldNames.LeaveDayTypeName] = leadayType != null ? leadayType.LeaveDayTypeName : string.Empty;
                            }
                            datatable.Rows.Add(row);
                        }
                    }
                }
                return datatable;
            }
        }

        DataTable CreateReportMealTimeDetailSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.SectionName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.LeaveDayTypeName);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.UserPrint);
            tb.Columns.Add(Can_ReportMealTimeDetailEntity.FieldNames.DatePrint, typeof(DateTime));
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Data" + date.Day))
                    tb.Columns.Add("Data" + date.Day);
            }
            return tb;
        }
        #endregion

        #region BC Chi Tiết Suất Ăn - 20140812_01
        public DataTable ReportDetailCard(List<Guid?> cateringIDs, List<Guid?> canteenIDs, List<Guid?> lineIDs, DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs)
        {
            DataTable datatable = CreateReportDetailCardSchema(dateStart, dateEndSearch);
            using (var context = new VnrHrmDataContext())
            {
                #region get data
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_MealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repoCan_MealRecord.FindBy(s => s.ProfileID != null && s.WorkDay != null &&
                dateStart <= s.WorkDay && s.WorkDay <= dateEnd).Select(s =>
                new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.WorkDay }).ToList();
                if (mealRecords != null && mealRecords.Count < 1)
                {
                    return datatable;
                }

                var profileIdsByMealRecord = mealRecords.Select(s => s.ProfileID).Distinct().ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => profileIdsByMealRecord.Contains(s.ID)).ToList();

                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var repoCan_Canteen = new Can_CanteenRepository(unitOfWork);
                var repoCan_Catering = new Can_CateringRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);

                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var lstallLine = repoCan_Line.GetAll().ToList();
                var lstallCatering = repoCan_Catering.GetAll().ToList();
                var lstallCanteen = repoCan_Canteen.GetAll().ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();

                if (orgIDs != null && orgIDs[0] != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                if (canteenIDs != null && canteenIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && canteenIDs.Contains(s.CanteenID.Value)).ToList();
                }
                if (cateringIDs != null && cateringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && cateringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (lineIDs != null && lineIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                }
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                #endregion
                foreach (var profile in profiles)
                {
                    var mealRecordByProfile = mealRecords.Where(m => m.ProfileID != null && m.ProfileID.Value == profile.ID).ToList();
                    if (mealRecordByProfile == null && mealRecordByProfile.Count() == 0)
                        continue;
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var lstMealByPro = mealRecords.Where(m => m.ProfileID == profile.ID && m.TimeLog != null && m.TimeLog.Value.Date == date.Date).ToList();
                        foreach (var MealByPro in lstMealByPro)
                        {
                            if (MealByPro != null)
                            {
                                DataRow row = datatable.NewRow();
                                var profilebymeal = repoHre_Profile.FindBy(m => m.ID == MealByPro.ProfileID).FirstOrDefault();
                                Guid? orgId = profilebymeal.OrgStructureID;
                                var org = orgs.FirstOrDefault(s => s.ID == profilebymeal.OrgStructureID);
                                var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                                var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                                var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                                row[Can_ReportDetailCardEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                row[Can_ReportDetailCardEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                row[Can_ReportDetailCardEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                row[Can_ReportDetailCardEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                row[Can_ReportDetailCardEntity.FieldNames.CodeEmp] = profilebymeal.CodeEmp;
                                row[Can_ReportDetailCardEntity.FieldNames.ProfileName] = profilebymeal.ProfileName;
                                if (MealByPro.WorkDay != null)
                                {
                                    row["Data" + date.Day] = MealByPro.WorkDay.Value.ToString(ConstantFormat.HRM_Format_Grid_ShortTime);
                                }
                                var line = lstallLine.Where(m => MealByPro.LineID == m.ID).FirstOrDefault();
                                row[Can_ReportDetailCardEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                                var catering = lstallCatering.Where(m => MealByPro.CateringID == m.ID).FirstOrDefault();
                                row[Can_ReportDetailCardEntity.FieldNames.CateringName] = catering != null ? catering.CateringName : string.Empty;
                                var canteen = lstallCanteen.Where(m => MealByPro.CanteenID == m.ID).FirstOrDefault();
                                row[Can_ReportDetailCardEntity.FieldNames.CanteenName] = canteen != null ? canteen.CanteenName : string.Empty;
                                row[Can_ReportDetailCardEntity.FieldNames.DateFrom] = dateStart;
                                row[Can_ReportDetailCardEntity.FieldNames.DateTo] = dateEnd;
                                row[Can_ReportDetailCardEntity.FieldNames.DatePrint] = DateTime.Now;
                                datatable.Rows.Add(row);
                            }
                        }
                    }
                }
                return datatable;
            }
        }

        DataTable CreateReportDetailCardSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.SectionName);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.UserPrint);
            tb.Columns.Add(Can_ReportDetailCardEntity.FieldNames.DatePrint, typeof(DateTime));
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Data" + date.Day))
                    tb.Columns.Add("Data" + date.Day);
            }
            return tb;
        }
        #endregion

        #region BC Suất Ăn Theo Địa Điểm - Report CMS _20140814
        DataTable CreateReportCardByLocationSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.SectionName);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.WorkLocationCode);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.WorkLocationHD);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.EatLoCationCode);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.EatLoCationHD);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.Quantity);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.Amount, typeof(Double));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.Hour);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.UserPrint);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DatePrint, typeof(DateTime));
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Data" + date.Day))
                    tb.Columns.Add("Data" + date.Day);
            }
            return tb;
        }

        public DataTable ReportCardByLocation(List<Guid?> CarteringIDs, List<Guid?> CanteenIDS, List<Guid?> LineIDS, DateTime dateStart, DateTime dateEndSearch, 
            List<Guid> lstProfileIds, List<Guid?> workPlaceIds, List<Guid?> eatLocationIds, Boolean isCludeEmpQuit)
        {
            DataTable datatable = CreateReportCardByLocationSchema(dateStart, dateEndSearch);
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_Line = new Can_LineRepository(unitOfWork);

                var repo = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repo.FindBy(s => s.ProfileID != null && s.WorkDay != null && s.TimeLog != null
                    && dateStart <= s.WorkDay && s.WorkDay <= dateEnd).ToList();
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => profileIds.Contains(m.ID)).ToList();
                var repoCan_Location = new Can_LocationRepository(unitOfWork);
                var lstLocation = repoCan_Location.FindBy(s => s.IsDelete == null).ToList();

                if (lstProfileIds != null && lstProfileIds.Count >= 1)
                {
                    profiles = profiles.Where(s => lstProfileIds.Contains(s.ID)).ToList();
                }

                if (workPlaceIds != null && workPlaceIds[0] != null)
                {
                    profiles = profiles.Where(s => workPlaceIds.Contains(s.WorkPlaceID)).ToList();
                }

                if(isCludeEmpQuit != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                var lstcateringids = mealRecords.Select(m => m.CateringID).ToList();
                var lstcanteenids = mealRecords.Select(m => m.CanteenID).ToList();
                var Cardcode = mealRecords.Select(s => s.ProfileID).Distinct().ToList();
                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrg.FindBy(m => m.Code != null).ToList();

                if (CarteringIDs != null && CarteringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CateringID != null && CarteringIDs.Contains(m.CateringID)).ToList();
                }

                if (CanteenIDS != null && CanteenIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CanteenID != null && CanteenIDS.Contains(m.CanteenID)).ToList();
                }

                if (LineIDS != null && LineIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.LineID != null && LineIDS.Contains(m.LineID)).ToList();
                }

                var repocanteen = new Can_CanteenRepository(unitOfWork);
                if (eatLocationIds != null && eatLocationIds[0] != null)
                {
                    var lstcanteenbylocation = repocanteen.FindBy(c => c.IsDelete == null && c.LocationID != null && eatLocationIds.Contains(c.LocationID.Value));
                    var lstcanteenbylocationID = lstcanteenbylocation.Select(s => s.ID).ToList();
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && lstcanteenbylocationID.Contains(s.CanteenID.Value)).ToList();
                }

                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }

                var repoWorkLocation = new Cat_WorkPlaceRepository(unitOfWork);
                var lstWorkPlace = repoWorkLocation.GetAll().ToList();
                var repocatering = new Can_CateringRepository(unitOfWork);
                var caterings = repocatering.GetAll().ToList();
                var canteens = repocanteen.GetAll().ToList();
                var repoline = new Can_LineRepository(unitOfWork);
                var lines = repoline.GetAll().ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                foreach (var profile in profiles)
                {
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var meal = mealRecords.FirstOrDefault(s => s.TimeLog != null && s.ProfileID == profile.ID && s.TimeLog.Value.Date == date.Date);
                        if (meal != null)
                        {
                            DataRow row = datatable.NewRow();
                            var cateen = canteens.FirstOrDefault(s => s.ID == meal.CanteenID);
                            var cate = caterings.FirstOrDefault(s => s.ID == meal.CateringID);
                            var line = lines.FirstOrDefault(s => s.ID == meal.LineID);
                            Guid? orgId = profile.OrgStructureID;
                            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                            row[Can_ReportCardByLocationEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.SectionName] = orgSection != null ? orgSection.Code : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Can_ReportCardByLocationEntity.FieldNames.ProfileName] = profile.ProfileName;
                            row[Can_ReportCardByLocationEntity.FieldNames.DateFrom] = dateStart;
                            row[Can_ReportCardByLocationEntity.FieldNames.DateTo] = dateEnd;
                            row[Can_ReportCardByLocationEntity.FieldNames.DatePrint] = DateTime.Now;
                            var location = cateen != null ? cateen.LocationID : null;
                            row[Can_ReportCardByLocationEntity.FieldNames.CateringName] = cate != null ? cate.CateringName : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.CanteenName] = cateen != null ? cateen.CanteenName : string.Empty;
                            row[Can_ReportCardByLocationEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                            if (lstWorkPlace != null && lstWorkPlace.Count > 0)
                            {
                                var workPlaceByPro = lstWorkPlace.Where(m => m.ID == profile.WorkPlaceID).FirstOrDefault();
                                row[Can_ReportCardByLocationEntity.FieldNames.WorkLocationCode] = workPlaceByPro != null ? workPlaceByPro.Code : string.Empty;
                                row[Can_ReportCardByLocationEntity.FieldNames.WorkLocationHD] = workPlaceByPro != null ? workPlaceByPro.WorkPlaceName : string.Empty;
                            }

                            if (location != null && location != Guid.Empty)
                            {
                                var locationname = lstLocation.Where(s => s.ID == location).FirstOrDefault();
                                row[Can_ReportCardByLocationEntity.FieldNames.EatLoCationCode] = location != null ? locationname.LocationCode : string.Empty;
                                row[Can_ReportCardByLocationEntity.FieldNames.EatLoCationHD] = location != null ? locationname.LocationName : string.Empty;
                            }
                            if (meal.WorkDay != null)
                            {
                                row["Data" + date.Day] = meal.WorkDay.Value.ToString(ConstantFormat.HRM_Format_Grid_ShortTime);
                                row[Can_ReportCardByLocationEntity.FieldNames.Quantity] = 1;
                            }
                            row[Can_ReportCardByLocationEntity.FieldNames.Amount] = (object)meal.Amount ?? DBNull.Value;
                            row[Can_ReportCardByLocationEntity.FieldNames.Date] = meal.WorkDay;
                            if (meal.TimeLog != null)
                            {
                                row[Can_ReportCardByLocationEntity.FieldNames.Hour] = meal.TimeLog.Value;
                            }
                            datatable.Rows.Add(row);
                        }
                    }
                }
            }
            return datatable;
        }
        #endregion

        #region BC có chấm công không ăn - Report CMS _20140814
        DataTable GetSchemaRptMealTimeDetailNoEat()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.SectionName);

            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeBranch);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeDepartment);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeTeam);
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeSection);
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportCardByLocationEntity.FieldNames.Date, typeof(DateTime));

            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.TimeIn, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.TimeOut, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.MealAllowanceName);

            tb.Columns.Add(Can_ReportMealTimeDetailNoEatEntity.FieldNames.DateExport, typeof(DateTime));
            return tb;
        }
        public DataTable ReportMealTimeDetailNoEat(List<Guid?> lstWorkPlace, String status, String codeEmp, Boolean isCludeEmpQuit,
            DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs)
        {
            List<Can_ReportMealTimeDetailNoEatEntity> lstreportMealTimeDetailNoEatEntity = new List<Can_ReportMealTimeDetailNoEatEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);

                var repomealRecordMissing = new Can_MealRecordMissingRepository(unitOfWork);
                var mealRecordMissings = repomealRecordMissing.FindBy(s => s.ProfileID != null && s.WorkDate != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd
                                         && s.MealAllowanceTypeSettingID == null)
                                         .Select(s => new { s.ProfileID, s.WorkDate, s.Status, s.MealAllowanceTypeSettingID, s.OrgStructureID }).ToList();
                DataTable table = GetSchemaRptMealTimeDetailNoEat();
                if (mealRecordMissings == null && mealRecordMissings.Count < 1)
                {
                    return table;
                }
                var profileIds = mealRecordMissings.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(s => profileIds.Contains(s.ID)).ToList();

                if (orgIDs != null && orgIDs[0] != null)
                {
                    profiles = profiles.Where(hr => hr.OrgStructureID != null && orgIDs.Contains(hr.OrgStructureID)).ToList();
                }
                if (lstWorkPlace[0] != Guid.Empty)
                {
                    profiles = profiles.Where(wp => wp.WorkPlaceID != null && lstWorkPlace.Contains(wp.WorkPlaceID.Value)).ToList();
                }
                if (isCludeEmpQuit != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecordMissings = mealRecordMissings.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                //trạng thái
                if (status != null)
                {
                    mealRecordMissings = mealRecordMissings.Where(meal => status.Contains(meal.Status)).ToList();
                }

                //workday
                var repAttWorkday = new Att_WorkDayRepository(unitOfWork);
                var lstWorkday = repAttWorkday.FindBy(wd => wd.WorkDate >= dateStart && wd.WorkDate <= dateEnd && profileIds.Contains(wd.ProfileID))
                                 .Select(wd => new {wd.ProfileID, wd.InTime1, wd.OutTime1, wd.WorkDate }).ToList();

                //danh mục leaveday type
                var repLeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leadayHLDID = repLeaveDayType.FindBy(ld => ld.Code == "HLD").Select(ld => ld.ID).FirstOrDefault();

                //att_leaveday
                var repAttLeaveDay = new Att_LeavedayRepository(unitOfWork);
                var E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                var lstLeaveday = repAttLeaveDay.FindBy(ld => ld.LeaveDayTypeID != leadayHLDID && (profileIds.Contains(ld.ProfileID))
                                && ld.Status == E_APPROVED && dateStart <= ld.DateEnd && ld.DateStart <= dateEnd)
                                .Select(ld => new { ld.ProfileID, ld.DateStart, ld.DateEnd }).ToList();

                //can_line
                var repHDTjob = new Can_LineRepository(unitOfWork);
                var lstHDTjobID = repHDTjob.FindBy(hdt => hdt.IsHDTJOB == true).Select(hdt => hdt.ID).ToList();
                
                //Can_mealRecord
                var repMealRecord = new Can_MealRecordRepository(unitOfWork);
                var lstMealRecord = repMealRecord.FindBy(meal => meal.LineID != null && !lstHDTjobID.Contains(meal.LineID.Value) && meal.ProfileID.HasValue && profileIds.Contains(meal.ProfileID.Value)
                                    && dateStart <= meal.WorkDay && meal.WorkDay <= dateEnd)
                                  .Select(meal => new { meal.ProfileID, meal.WorkDay }).ToList();

                //Allowance type setting
                var repAllowanceTypeSetting = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var lstAllowanceTypeSetting = repAllowanceTypeSetting.GetAll().Select(al => new {al.ID, al.MealAllowanceTypeSettingName }).ToList();


                var repoOrgType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var listOrgType = repoOrgType.FindBy(s => s.IsDelete == null).ToList();

                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var listOrgAll = repoOrg.GetAll().ToList();

                //var repoLine = new Can_LineRepository(unitOfWork);
                //var repoCatering = new Can_CateringRepository(unitOfWork);
                //var repoCanteen = new Can_CanteenRepository(unitOfWork);

                foreach (var mealRecordMissing in mealRecordMissings)
                {
                    // neu co quet the an thi ko tinh
                    var meadRecord = lstMealRecord.FirstOrDefault(s => s.ProfileID == mealRecordMissing.ProfileID && s.WorkDay.Value.Date == mealRecordMissing.WorkDate.Value.Date);
                    if (meadRecord != null)
                    {
                        continue;
                    }
                    // neu ngay do dk nghi thi ko tinh
                    var leaveday = lstLeaveday.FirstOrDefault(s => s.ProfileID == mealRecordMissing.ProfileID && s.DateStart <= mealRecordMissing.WorkDate && mealRecordMissing.WorkDate <= s.DateEnd);
                    if (leaveday != null)
                    {
                        continue;
                    }
                    // neu ngay do ko di lam thi ko tinh
                    var workday = lstWorkday.FirstOrDefault(s => s.ProfileID == mealRecordMissing.ProfileID && s.WorkDate.Date == mealRecordMissing.WorkDate.Value.Date);
                    if (workday == null)
                    {
                        continue;
                    }
                    DataRow row = table.NewRow();
                    if (workday != null && workday.InTime1 != null)
                    {
                        row["TimeIn"] = workday.InTime1;
                    }
                    if (workday != null && workday.OutTime1 != null)
                    {
                        row["TimeOut"] = workday.OutTime1;
                    }
                    var profile = profiles.FirstOrDefault(s => s.ID == mealRecordMissing.ProfileID);
                    if (profile != null)
                    {
                        var type = lstAllowanceTypeSetting.FirstOrDefault(s => s.ID == mealRecordMissing.MealAllowanceTypeSettingID);
                        var org = listOrgAll.FirstOrDefault(s => profile != null && s.ID == mealRecordMissing.OrgStructureID);
                        var orgBranch = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_BRANCH, listOrgAll, listOrgType);
                        var orgOrg = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_DEPARTMENT, listOrgAll, listOrgType);
                        var orgTeam = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_TEAM, listOrgAll, listOrgType);
                        var orgSection = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_SECTION, listOrgAll, listOrgType);

                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeDepartment] = orgOrg != null ? orgOrg.Code : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            
                        row[Can_ReportCardByLocationEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        row[Can_ReportCardByLocationEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Can_ReportCardByLocationEntity.FieldNames.Date] = mealRecordMissing.WorkDate;
                        row[Can_ReportCardByLocationEntity.FieldNames.DateFrom] = dateStart;
                        row[Can_ReportCardByLocationEntity.FieldNames.DateTo] = dateEnd;

                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.DateExport] = DateTime.Now;
                        row[Can_ReportMealTimeDetailNoEatEntity.FieldNames.MealAllowanceName] = type != null ? type.MealAllowanceTypeSettingName : string.Empty;
                        table.Rows.Add(row);
                    }

                }
                return table;
            }
        }
        #endregion

        #region BC Quẹt Thẻ Nhiều Lần - Report CMS _20140814
        DataTable CreateReportMultiSlideCardSchema(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.SectionName);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.SumCardMore, typeof(double));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.Amount, typeof(double));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.Hour);

            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.DateExport, typeof(DateTime));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.UserPrint);
            tb.Columns.Add(Can_ReportMultiSlIDeCardEntity.FieldNames.DatePrint, typeof(DateTime));
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                if (!tb.Columns.Contains("Data" + date.Day))
                    tb.Columns.Add("Data" + date.Day);
            }
            return tb;
        }

        public DataTable ReportMultiSlideCard(List<Guid?> CarteringIDs, List<Guid?> CanteenIDS, List<Guid?> LineIDS, List<Guid?> lstWorkPlaceID,
            DateTime dateStart, DateTime dateEndSearch, List<Guid> lstProfileIds, Boolean IsIncludeQuitEmp)
        {
            DataTable datatable = CreateReportMultiSlideCardSchema(dateStart, dateEndSearch);
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var lineHDTJobIDs = repoCan_Line.FindBy(s => s.HDTJ != null)
                 .Select(s => s.ID).ToList();

                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var wordDayProfiles = repoWorkDay.FindBy(s => s.ProfileID != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                    .Select(s => new { s.ProfileID, s.WorkDate, s.FirstInTime, s.LastOutTime }).ToList();

                var repo = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repo.FindBy(s => s.LineID != null && s.WorkDay != null && !lineHDTJobIDs.Contains(s.LineID.Value) && s.ProfileID != null
                    && dateStart <= s.WorkDay && s.WorkDay <= dateEnd).Select(s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.WorkDay }).ToList();
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => profileIds.Contains(m.ID)).ToList();
                if (lstProfileIds != null && lstProfileIds.Count >= 1)
                {
                    profiles = profiles.Where(s => lstProfileIds.Contains(s.ID)).ToList();
                }
                if (lstWorkPlaceID[0] != Guid.Empty)
                {
                    profiles = profiles.Where(hr => hr.WorkPlaceID != null && lstWorkPlaceID.Contains(hr.WorkPlaceID.Value)).ToList();
                }
                //Kiểm tra ko nghỉ việc
                if (IsIncludeQuitEmp != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                var Cardcode = mealRecords.Select(s => s.ProfileID).Distinct().ToList();
                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrg.FindBy(m => m.Code != null).ToList();
                var catering = new List<Can_Catering>().ToList();
                var repocatering = new Can_CateringRepository(unitOfWork);
                if (CarteringIDs != null && CarteringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CateringID != null && CarteringIDs.Contains(m.CateringID)).ToList();
                }
                var repocanteen = new Can_CanteenRepository(unitOfWork);
                if (CanteenIDS != null && CanteenIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CanteenID != null && CanteenIDS.Contains(m.CanteenID)).ToList();
                }
                if (LineIDS != null && LineIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.LineID != null && LineIDS.Contains(m.LineID)).ToList();
                }
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var orgTypes = new List<Cat_OrgStructureType>();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                var repoCan_Catering = new Can_CateringRepository(unitOfWork);
                var repoCan_Canteen = new Can_CanteenRepository(unitOfWork);
                var repoline = new Can_LineRepository(unitOfWork);
                var canteens = repoCan_Canteen.GetAll().ToList();
                var caterings = repoCan_Catering.GetAll().ToList();
                var lines = repoCan_Line.GetAll().ToList();

                var approve = LeaveDayStatus.E_APPROVED.ToString();
                var repoLeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leavedays = repoLeaveDay.FindBy(s => s.Status == approve && dateStart <= s.DateEnd
                     && s.DateStart <= dateEnd && profileIds.Contains(s.ProfileID)).Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();

                foreach (var profile in profiles)
                {
                    var mealRecordProfiles = mealRecords.Where(s => s.ProfileID == profile.ID).ToList();
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leaveDay = leavedays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        var workDayDates = wordDayProfiles.Where(s => s.ProfileID == profile.ID && (s.FirstInTime != null || s.LastOutTime != null)).Select(s => s.WorkDate.Date).ToList();
                        var mealDoubles = mealRecordProfiles.Where(s => s.TimeLog != null && s.TimeLog.Value.Date == date.Date).ToList();
                        if (mealDoubles.Count > 1)
                        {
                            foreach (var meal in mealDoubles)
                            {
                                DataRow row = datatable.NewRow();
                                Guid? orgId = profile != null ? profile.OrgStructureID : Guid.Empty;
                                var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                                var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                                var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                                var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.ProfileName] = profile.ProfileName;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.DateFrom] = dateStart;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.DateTo] = dateEnd;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.DatePrint] = DateTime.Now;
                                if (meal.TimeLog != null)
                                {
                                    row["Data" + date.Day] = meal.TimeLog.Value.ToString(ConstantFormat.HRM_Format_Grid_ShortTime);
                                }
                                var count1 = 0;
                                for (DateTime date1 = dateStart; date1 <= dateEnd; date1 = date1.AddDays(1))
                                {
                                    int countDay = mealRecordProfiles.Count(s => s.TimeLog != null && s.TimeLog.Value.Date == date1.Date && leaveDay == null);
                                    if (countDay > 1)
                                    {
                                        count1 += countDay - 1;
                                    }
                                }
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.SumCardMore] = count1;
                                var cate = caterings.FirstOrDefault(s => s.ID == meal.CateringID);
                                var cateen = canteens.FirstOrDefault(s => s.ID == meal.CanteenID);
                                var line = lines.FirstOrDefault(s => s.ID == meal.LineID);
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.CateringName] = cate != null ? cate.CateringName : string.Empty; ;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.CanteenName] = cateen != null ? cateen.CanteenName : string.Empty; ;
                                row[Can_ReportMultiSlIDeCardEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                                datatable.Rows.Add(row);
                            }
                        }
                    }
                }
                return datatable;
            }
        }
        #endregion

        #region Bc Tiền ăn người nhật - 20140812_1
        DataTable CreatePayMoneyEatSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var amounts = repoCan_Line.GetAll().OrderBy(s => s.Amount).Select(s => s.Amount).Distinct().ToList();
                DataTable tb = new DataTable();
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.ProfileName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.CateringName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.BranchName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.TeamName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.SectionName);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.CountEat, typeof(int));
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.SubtractSalary, typeof(double));
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.UserPrint);
                tb.Columns.Add(Can_ReportPayMoneyEatEntity.FieldNames.DatePrint, typeof(DateTime));
                foreach (var amount in amounts)
                {
                    if (!tb.Columns.Contains(amount.ToString()))
                    {
                        tb.Columns.Add(amount.ToString());
                    }
                }
                return tb;
            }
        }

        public DataTable GetReportPayMoneyEat(List<Guid?> cateringIDs, List<Guid?> canteenIDs, List<Guid?> lineIDs,
            DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs, List<Guid?> countryIDs, String codeEmp, Boolean IsIncludeQuitEmp)
        {
            DataTable datatable = CreatePayMoneyEatSchema();
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_MealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repoCan_MealRecord.FindBy(s => s.ProfileID != null && s.WorkDay != null
                  && dateStart <= s.WorkDay && s.WorkDay <= dateEnd).Select(
                  s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.NoWorkDay }).ToList();

                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(m => profileIds.Contains(m.ID)).ToList();

                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                if (orgIDs != null && orgIDs[0] != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                //if (empTypes != null && empTypes[0] != null)
                //{
                //    profiles = profiles.Where(s => s.EmpTypeID != null && empTypes.Contains(s.EmpTypeID.Value)).ToList();
                //}
                if (countryIDs[0] != Guid.Empty)
                {
                    profiles = profiles.Where(s => s.NationalityID != null && countryIDs.Contains(s.NationalityID.Value)).ToList();
                    //profileIds = profiles.Select(s => s.ID).ToList();
                    //mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (IsIncludeQuitEmp != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    //profileIds = profiles.Select(s => s.ID).ToList();
                    //mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                if (canteenIDs != null && canteenIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && canteenIDs.Contains(s.CanteenID.Value)).ToList();
                }
                if (cateringIDs != null && cateringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && cateringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (lineIDs != null && lineIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                }
                var codeAttendaceByProfile = profiles.Select(m => m.ID).ToList();
                if (codeAttendaceByProfile != null && codeAttendaceByProfile.Count > 0)
                {
                    mealRecords = mealRecords.Where(m => m.ProfileID != null && codeAttendaceByProfile.Contains(m.ProfileID.Value)).ToList();
                }
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                profileIds = mealRecords.Select(s => s.ProfileID.Value).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var repoCaterings = new Can_CateringRepository(unitOfWork);
                var caterings = repoCaterings.GetAll().ToList();
                var repoCanteen = new Can_CanteenRepository(unitOfWork);
                var canteens = repoCanteen.GetAll().ToList();

                var amounts = repoCan_Line.FindBy(s => s.Amount != null).Select(s => new { s.Amount, s.ID }).Distinct().ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                foreach (var profile in profiles)
                {
                    var careringIDs = mealRecords.Where(s => s.ProfileID == profile.ID).Select(s => s.CateringID).ToList();
                    var carerings = caterings.Where(s => careringIDs.Contains(s.ID)).ToList();
                    foreach (var catering in carerings)
                    {
                        DataRow row = datatable.NewRow();
                        Guid? orgId = profile.OrgStructureID;
                        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                        row[Can_ReportPayMoneyEatEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.DateFrom] = dateStart;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.DateTo] = dateEnd;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.DatePrint] = DateTime.Now;
                        row[Can_ReportPayMoneyEatEntity.FieldNames.CateringName] = catering.CateringName;

                        var lstAmount = amounts.Select(am => am.Amount).Distinct().ToList();
                        foreach (var amount in lstAmount)
                        {
                            var lsLineID = amounts.Where(am => am.Amount == amount).Select(am => am.ID).ToList();
                            var countCareing = mealRecords.Where(meal => meal.ProfileID == profile.ID && meal.LineID != null && lsLineID.Contains(meal.LineID.Value)).Count();
                            if (countCareing > 0)
                                row[amount.ToString()] = countCareing;
                        }

                        foreach (var amount in amounts)
                        {
                            var countCareing = mealRecords.Count(s => s.ProfileID == profile.ID && s.CateringID == catering.ID && s.LineID == amount.ID);
                            if (countCareing > 0)
                                row[amount.Amount.ToString()] = countCareing;
                        }
                        var countEat = mealRecords.Count(s => s.ProfileID == profile.ID && s.CateringID == catering.ID);
                        if (countEat > 0)
                            row[Can_ReportPayMoneyEatEntity.FieldNames.CountEat] = countEat;
                        var subtractSalary = mealRecords.Where(s => s.ProfileID == profile.ID && s.CateringID == catering.ID).Sum(s => s.Amount);
                        if (subtractSalary > 0)
                            row[Can_ReportPayMoneyEatEntity.FieldNames.SubtractSalary] = subtractSalary;
                        datatable.Rows.Add(row);
                    }
                }
                return datatable;
            }
        }
        #endregion

        #region Bc thu Lại tiền NV - 20140812_1
        DataTable CreateReportSumaryReturnMoneyEatSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoCan_MealAllowanceTypeSetting = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var amountStardand = repoCan_MealAllowanceTypeSetting.FindBy(s => s.Standard == true && s.Amount != null).Select(s => s.Amount).FirstOrDefault();
                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var amounts = repoCan_Line.FindBy(s => s.HDTJ == null && s.Amount != amountStardand).OrderBy(s => s.Amount).Select(s => s.Amount).Distinct().ToList();
                DataTable tb = new DataTable();
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.ProfileName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CateringName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CanteenName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.LineName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.BranchName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.TeamName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SectionName);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountCardMore, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumAmountCardMore, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountNotWorkButHasEat, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumNotWorkButHasEat, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumAmountMustSubtract, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.MoneyMustSubtractHDT, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumMoneyMustSubtract, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountWorkDay, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountSandard, typeof(double));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.UserPrint);
                tb.Columns.Add(Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DatePrint, typeof(DateTime));

                foreach (var amount in amounts)
                {
                    if (!tb.Columns.Contains(amount.ToString()))
                    {
                        tb.Columns.Add(amount.ToString());
                        tb.Columns.Add("Số Tiền " + amount, typeof(double));
                    }
                }
                return tb;
            }
        }

        public DataTable GetReportSumaryReturnMoneyEat(List<Guid?> cateringIDs, List<Guid?> canteenIDs, List<Guid?> lineIDs, List<Guid?> workPlaceIds,
            DateTime dateStart, DateTime dateEndSearch, List<Guid?> orgIDs, String codeEmp, Boolean IsIncludeQuitEmp)
        {
            DataTable datatable = CreateReportSumaryReturnMoneyEatSchema();
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateEnd = dateEndSearch.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_MealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecords = repoCan_MealRecord.FindBy(s => s.ProfileID != null && s.WorkDay != null &&
                dateStart <= s.WorkDay && s.WorkDay <= dateEnd).Select(
                s => new { s.ProfileID, s.TimeLog, s.CanteenID, s.CateringID, s.LineID, s.Amount, s.MealAllowanceTypeID, s.NoWorkDay, s.WorkDay }).ToList();

                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var workDayProfies = repoWorkDay.FindBy(s => (s.FirstInTime != null || s.LastOutTime != null) && dateStart <= s.WorkDate
                 && s.WorkDate <= dateEnd && profileIds.Contains(s.ProfileID)).Select(s => new { s.ProfileID, s.WorkDate }).ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(m => profileIds.Contains(m.ID)).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoHre_HDTJob = new Hre_HDTJobRepository(unitOfWork);
                var profileHDTJIDs = repoHre_HDTJob.FindBy(s => s.DateTo >= dateStart && s.DateFrom <= dateEnd).Select(s => s.ProfileID).ToList();
                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var lineHDTJIDs = repoCan_Line.FindBy(s => s.HDTJ != null).Select(s => s.ID).ToList();
                var repoCan_MealAllowanceTypeSetting = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var amountStardand = repoCan_MealAllowanceTypeSetting.FindBy(s => s.Standard == true && s.Amount != null).Select(s => s.Amount).FirstOrDefault();
                var repoLeaveDay = new Att_LeavedayRepository(unitOfWork);

                var leavedays = repoLeaveDay.FindBy(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && profileIds.Contains(s.ProfileID))
                    .Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();
                if (orgIDs != null && orgIDs[0] != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                if (workPlaceIds != null && workPlaceIds[0] != null)
                {
                    profiles = profiles.Where(s => workPlaceIds.Contains(s.WorkPlaceID)).ToList();
                }

                if (IsIncludeQuitEmp != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    //profileIds = profiles.Select(s => s.ID).ToList();
                    //mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                if (canteenIDs != null && canteenIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CanteenID != null && canteenIDs.Contains(s.CanteenID.Value)).ToList();
                }
                if (cateringIDs != null && cateringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.CateringID != null && cateringIDs.Contains(s.CateringID.Value)).ToList();
                }
                if (lineIDs != null && lineIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                }
                var codeAttendaceByProfile = profiles.Select(m => m.ID).ToList();
                if (codeAttendaceByProfile != null && codeAttendaceByProfile.Count > 0)
                {
                    mealRecords = mealRecords.Where(m => m.ProfileID != null && codeAttendaceByProfile.Contains(m.ProfileID.Value)).ToList();
                }
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value)).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();

                var repoLine = new Can_LineRepository(unitOfWork);
                var lines = repoLine.FindBy(s => s.Amount != null && s.HDTJ == null && s.Amount != amountStardand).Select(s => new { s.Amount.Value, s.ID }).Distinct().ToList();

                foreach (var profile in profiles)
                {
                    var workdayProfileDates = workDayProfies.Where(s => s.ProfileID == profile.ID).Select(s => s.WorkDate.Date).ToList();
                    var mealProfiles = mealRecords.Where(s => s.ProfileID == profile.ID).ToList();
                    DataRow row = datatable.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DateFrom] = dateStart;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DateTo] = dateEnd;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.DatePrint] = DateTime.Now;
                    bool isAdd = false;
                    var amounts = lines.Select(s => s.Value).Distinct().ToList();
                    foreach (var amount in amounts)
                    {
                        var countCareing = 0;
                        double sumCareing = 0;
                        var line1IDs = lines.Where(s => s.Value == amount).Select(s => s.ID).ToList();
                        for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                        {
                            var leaday = leavedays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                            countCareing += mealProfiles.Count(s => line1IDs.Contains(s.LineID.Value) && s.WorkDay.Value.Date == date.Date && (workdayProfileDates.Contains(s.WorkDay.Value.Date) || leaday != null));
                            var sumAmount = mealProfiles.Where(s => line1IDs.Contains(s.LineID.Value) && s.WorkDay.Value.Date == date.Date && (workdayProfileDates.Contains(s.WorkDay.Value.Date) || leaday != null)).Sum(s => s.Amount);
                            if (sumAmount > 0)
                            {
                                sumCareing += (double)(sumAmount.Value - amountStardand.Value);
                            }

                        }
                        if (countCareing > 0)
                        {
                            isAdd = true;
                            row[amount.ToString()] = countCareing;
                            row["Số Tiền " + amount] = sumCareing;
                        }
                    }

                    var countSard = mealProfiles.Where(hr => hr.WorkDay != null).Count(hr => hr.Amount == amountStardand && workdayProfileDates.Contains(hr.WorkDay.Value.Date));
                    var countWorkDay = workDayProfies.Where(wd => wd.WorkDate != null).Count(wd => workdayProfileDates.Contains(wd.WorkDate.Date));
                    if (countSard > 0)
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountSandard] = countSard;
                    if (countWorkDay > 0)
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountWorkDay] = countWorkDay;

                    var countCardMore1 = 0;
                    double sumAmountNotStardand = 0;// tong so tien quet the ko dat chuan
                    double? sumAmountHDTJOB = 0;// tong so tien cua nhan vien ko phai HDTJOb ma an o cua an HDTJOB
                    sumAmountHDTJOB = (double?)mealProfiles.Where(s => s.ProfileID == profile.ID && s.LineID != null && lineHDTJIDs.Contains(s.LineID.Value) && !profileHDTJIDs.Contains(s.ProfileID.Value)).Sum(s => s.Amount);
                    double sumAmountNotWorkHasEat = 0;// tong so tien ko di lam ma co an
                    var countNotWorkButHasEat = 0;
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leaday = leavedays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        if (mealProfiles.Count(s => s.WorkDay.Value.Date == date.Date) > 1)
                        {
                            countCardMore1++;
                        }
                        var amount = mealProfiles.FirstOrDefault(s => s.WorkDay.Value.Date == date.Date);
                        if (amount != null)
                        {
                            sumAmountNotStardand += (double)(amount.Amount.Value - amountStardand.Value);
                        }
                        var meal = mealProfiles.FirstOrDefault(s => date.Date == s.WorkDay.Value.Date && (!workdayProfileDates.Contains(s.WorkDay.Value.Date) || leaday != null));
                        if (meal != null)
                        {
                            countNotWorkButHasEat++;
                            sumAmountNotWorkHasEat += (double)meal.Amount.Value;
                        }
                    }

                    if (countCardMore1 > 0)
                    {
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountCardMore] = countCardMore1;
                        isAdd = true;
                    }
                    var sumCardMore = countCardMore1 * amountStardand.Value;
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumAmountCardMore] = sumCardMore;
                    if (countNotWorkButHasEat > 0)
                    {
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.CountNotWorkButHasEat] = countNotWorkButHasEat;
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumNotWorkButHasEat] = sumAmountNotWorkHasEat;
                        isAdd = true;
                    }
                    var sum = Math.Round((sumAmountNotWorkHasEat + sumAmountNotStardand + (double)sumCardMore), 0);
                    row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumAmountMustSubtract] = sum == 0 ? (object)DBNull.Value : sum;
                    sum = Math.Round(sumAmountHDTJOB.Value, 0);
                    if (sum > 0)
                    {
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.MoneyMustSubtractHDT] = sum;
                        isAdd = true;
                    }

                    sum = Math.Round((sumAmountNotWorkHasEat + sumAmountNotStardand + sumAmountHDTJOB.Value + (double)sumCardMore), 0);
                    if (sum > 0)
                    {
                        row[Can_ReportSumaryReturnMoneyEatEntity.FieldNames.SumMoneyMustSubtract] = sum;
                        isAdd = true;
                    }
                    if (isAdd)
                        datatable.Rows.Add(row);
                }
                return datatable;
            }
        }
        #endregion

        #region BC Nhân Viên Hưởng Trợ cấp - CMS
        DataTable CreateReportMealAllowanceOfEmployeeSchema()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ProfileName);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.MealAllowance, typeof(string));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ShiftActual, typeof(string));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ShiftApprove, typeof(string));

            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.SectionName);

            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeBranch);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeDepartment);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeTeam);
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeSection);

            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.Status, typeof(string));
            tb.Columns.Add(Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateExport, typeof(DateTime));

            return tb;
        }
        public DataTable ReportMealAllowanceOfEmployee(List<Guid?> lstMealAllowanceTypeSettingID, DateTime dateStart, DateTime dateEnd, 
            List<Guid> lstProfileIds, List<Guid?> workPlaceIds, Boolean isCludeEmpQuit)
        {
            DataTable datatable = CreateReportMealAllowanceOfEmployeeSchema();
            using (var context = new VnrHrmDataContext())
            {
                //DateTime dateEnd = dateEnd.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                string typeApprove = MealRecordMissingStatus.E_APPROVED.ToString(); 
                string typeWaitApprove = MealRecordMissingStatus.E_WAIT_APPROVED.ToString();

                var repoMealRecordMissing = new Can_MealRecordMissingRepository(unitOfWork);

                var lstMealRecordMiss = repoMealRecordMissing.FindBy(m => lstProfileIds.Contains(m.ProfileID.Value) &&
                                            (m.Status == typeApprove || m.Status == typeWaitApprove) &&
                                             m.WorkDate >= dateStart && m.WorkDate <= dateEnd).OrderBy(m => m.ProfileID).ThenBy(m => m.WorkDate).ToList();

                var profileIDs = lstMealRecordMiss.Select(hre => hre.ProfileID.Value).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => profileIDs.Contains(m.ID)).ToList();

                //tất cả phòng ban và loại trợ cấp
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var lstOrgAll = repoCat_OrgStructure.FindBy(og => og.IsDelete == null).ToList();

                var repoTypeMealAllSetting = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var lstTypeMealAllSetting = repoTypeMealAllSetting.GetAll().ToList();

                //ds Att_Workday và ca làm việc
                var repoAttWorkday = new Att_WorkDayRepository(unitOfWork);
                var workDays = repoAttWorkday.FindBy(wd => dateStart <= wd.WorkDate && wd.WorkDate <= dateEnd && profileIDs.Contains(wd.ProfileID))
                                .Select(wd => new { wd.ProfileID, wd.ShiftID, wd.WorkDate, wd.InTime1, wd.OutTime1, wd.ShiftActual, wd.ShiftApprove }).ToList();

                var repoShift = new Cat_ShiftRepository(unitOfWork);
                var lstShiftAll = repoShift.GetAll().ToList();

                if (lstProfileIds != null && lstProfileIds.Count >= 1)
                {
                    profiles = profiles.Where(s => lstProfileIds.Contains(s.ID)).ToList();
                }

                if (workPlaceIds != null && workPlaceIds[0] != null)
                {
                    profiles = profiles.Where(s => workPlaceIds.Contains(s.WorkPlaceID)).ToList();
                }

                if (isCludeEmpQuit != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                lstProfileIds = profiles.Select(s => s.ID).ToList();

                //ds ko quẹt thẻ theo loại trợ cấp
                if (lstMealAllowanceTypeSettingID[0] != Guid.Empty)
                {
                    lstMealRecordMiss = lstMealRecordMiss.Where(m => m.MealAllowanceTypeSettingID != null && lstMealAllowanceTypeSettingID.Contains(m.MealAllowanceTypeSettingID.Value)).ToList();
                }

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                foreach (var recordmiss in lstMealRecordMiss)
                {
                    if (recordmiss == null)
                        continue;
                    var profile = profiles.FirstOrDefault(s => s.ID == recordmiss.ProfileID);
                    if (profile == null) continue;
                    DataRow row = datatable.NewRow();

                    var orgId = lstOrgAll.Where(s => s.ID == recordmiss.OrgStructureID).Select(og => og.ID).FirstOrDefault();

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, lstOrgAll, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, lstOrgAll, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, lstOrgAll, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, lstOrgAll, orgTypes);

                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeDepartment] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateFrom] = dateStart;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.DateTo] = dateEnd;

                    var mealAllowance = lstTypeMealAllSetting.FirstOrDefault(m => m.ID == recordmiss.MealAllowanceTypeSettingID);
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.MealAllowance] = mealAllowance != null ? mealAllowance.MealAllowanceTypeSettingName : string.Empty;

                    var attworkday = workDays.FirstOrDefault(at => at.ProfileID == profile.ID && at.WorkDate == recordmiss.WorkDate);
                    if (attworkday != null)
                    {
                        var shiftActual = lstShiftAll.FirstOrDefault(sf => sf.ID == attworkday.ShiftActual);
                        row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ShiftActual] = shiftActual != null ? shiftActual.ShiftName : string.Empty;

                        var shiftApprove = lstShiftAll.FirstOrDefault(sf => sf.ID == attworkday.ShiftApprove);
                        row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.ShiftApprove] = shiftApprove != null ? shiftApprove.ShiftName : string.Empty;
                    }
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.Date] = recordmiss.WorkDate;
                    row[Can_ReportMealAllowanceOfEmployeeEntity.FieldNames.Status] = recordmiss.Status;

                    datatable.Rows.Add(row);
                }

            }
            return datatable;
        }
        #endregion

        #region BC Quẹt Thẻ HDT Nhiều Lần - CMS
        DataTable CreateReportHDTJobCardMoreSchema()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.ProfileName);

            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CateringName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CanteenName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.LineName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.DateCard, typeof(DateTime));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.Hour);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.SumCardMore, typeof(double));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.Amount, typeof(double));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.PriceStardand, typeof(double));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.Total, typeof(double));

            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.BranchName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.TeamName);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.SectionName);

            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CodeBranch);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CodeDepartment);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CodeTeam);
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.CodeSection);

            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Can_ReportHDTJobCardMoreEntity.FieldNames.DateExport, typeof(DateTime));
            return tb;
        }
        public DataTable ReportHDTJobCardMore(List<Guid?> CarteringIDs, List<Guid?> CanteenIDS, List<Guid?> LineIDS, DateTime dateStart, DateTime dateEnd, List<Guid> lstProfileIds,  Boolean isCludeEmpQuit)
        {
            DataTable datatable = CreateReportHDTJobCardMoreSchema();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_Line = new Can_LineRepository(unitOfWork);
                var repo = new Can_MealRecordRepository(unitOfWork);

                var mealRecords = repo.FindBy(s => s.ProfileID != null && s.WorkDay != null && s.TimeLog != null && dateStart <= s.WorkDay && s.WorkDay <= dateEnd).ToList();
                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }
                var profileIds = mealRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoProfile.FindBy(m => profileIds.Contains(m.ID)).ToList();

                var repoCan_Location = new Can_LocationRepository(unitOfWork);
                var lstLocation = repoCan_Location.FindBy(s => s.IsDelete == null).ToList();

                if (lstProfileIds != null && lstProfileIds.Count >= 1)
                {
                    profiles = profiles.Where(s => lstProfileIds.Contains(s.ID)).ToList();
                }

                if (isCludeEmpQuit != true)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                mealRecords = mealRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                var lstcateringids = mealRecords.Select(m => m.CateringID).ToList();
                var lstcanteenids = mealRecords.Select(m => m.CanteenID).ToList();
                var Cardcode = mealRecords.Select(s => s.ProfileID).Distinct().ToList();

                var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrg.FindBy(m => m.Code != null).ToList();

                if (CarteringIDs != null && CarteringIDs[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CateringID != null && CarteringIDs.Contains(m.CateringID)).ToList();
                }

                if (CanteenIDS != null && CanteenIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.CanteenID != null && CanteenIDS.Contains(m.CanteenID)).ToList();
                }

                if (LineIDS != null && LineIDS[0] != null)
                {
                    mealRecords = mealRecords.Where(m => m.LineID != null && LineIDS.Contains(m.LineID)).ToList();
                }

                if (mealRecords == null && mealRecords.Count < 1)
                {
                    return datatable;
                }

                var repoWorkLocation = new Cat_WorkPlaceRepository(unitOfWork);
                var lstWorkPlace = repoWorkLocation.GetAll().ToList();

                var repocatering = new Can_CateringRepository(unitOfWork);
                var caterings = repocatering.GetAll().ToList();

                var repocanteen = new Can_CanteenRepository(unitOfWork);
                var canteens = repocanteen.GetAll().ToList();

                var repoline = new Can_LineRepository(unitOfWork);
                var lines = repoline.GetAll().ToList();

                var orgTypes = new List<Cat_OrgStructureType>();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();

                var repoAttWorkday = new Att_WorkDayRepository(unitOfWork);
                var wordDayProfiles = repoAttWorkday.FindBy(wd => dateStart <= wd.WorkDate && wd.WorkDate <= dateEnd)
                                .Select(wd => new { wd.ProfileID, wd.ShiftID, wd.WorkDate, wd.InTime1, wd.OutTime1 }).ToList();

                var approve = LeaveDayStatus.E_APPROVED.ToString();
                var repoAttLeaveday = new Att_LeavedayRepository(unitOfWork);
                var leavedays = repoAttLeaveday.FindBy(ld => ld.Status == approve && dateStart <= ld.DateEnd
                                && ld.DateStart <= dateEnd && profileIds.Contains(ld.ProfileID)).Select(ld => new { ld.ProfileID, ld.DateStart, ld.DateEnd }).ToList();

                var repoHDTjob = new Hre_HDTJobRepository(unitOfWork);
                var hdtJobs = repoHDTjob.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) 
                                && dateStart <= s.DateTo && s.DateFrom <= dateEnd).Select(s => new { s.DateFrom, s.ProfileID, s.DateTo, s.Type }).ToList();

                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (!datatable.Columns.Contains("Data" + date.Day))
                    {
                        datatable.Columns.Add("Data" + date.Day, typeof(double));
                    }
                }

                foreach (var profile in profiles)
                {
                    List<DateTime> leaveDayDates = new List<DateTime>();
                    var leaveDayProfiles = leavedays.Where(s => s.ProfileID == profile.ID).ToList();
                    foreach (var leaveDayProfile in leaveDayProfiles)
                    {
                        for (DateTime date = leaveDayProfile.DateStart; date <= leaveDayProfile.DateEnd; date = date.AddDays(1))
                        {
                            leaveDayDates.Add(date.Date);
                        }
                    }
                    var workDayDates = wordDayProfiles.Where(s => s.ProfileID == profile.ID).Select(s => s.WorkDate.Date).ToList();
                    workDayDates = workDayDates.Where(s => !leaveDayDates.Contains(s.Date)).ToList();

                    var mealRecordProfiles = mealRecords.Where(s => s.ProfileID == profile.ID && workDayDates.Contains(s.WorkDay.Value.Date)).ToList();
                    if (mealRecordProfiles.Count > 0)
                    {
                        var dateend = mealRecordProfiles[mealRecordProfiles.Count - 1].WorkDay;
                        for (DateTime date = mealRecordProfiles[0].WorkDay.Value; date <= dateend; date = date.AddDays(1))
                        {
                            if (mealRecordProfiles.Count(s => s.WorkDay.Value.Date == date.Date) > 1)
                            {
                                var mealDoubles = mealRecordProfiles.Where(s => s.WorkDay.Value.Date == date.Date).ToList();
                                if (mealDoubles.Count > 1)
                                {
                                    var hdtType = hdtJobs.FirstOrDefault(s => s.DateFrom <= date && date <= s.DateTo && s.ProfileID == profile.ID);
                                    foreach (var meal in mealDoubles)
                                    {
                                        DataRow row = datatable.NewRow();

                                        var orgId = orgs.Where(og => og.ID == mealRecordProfiles[0].OrgStructureID).Select(og => og.ID).FirstOrDefault();
                                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CodeDepartment] = orgOrg != null ? orgOrg.Code : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.ProfileName] = profile.ProfileName;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.DateFrom] = dateStart;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.DateTo] = dateEnd;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.DateExport] = DateTime.Now;
                                        if (meal.TimeLog != null)
                                        {
                                            row[Can_ReportHDTJobCardMoreEntity.FieldNames.DateCard] = meal.WorkDay;
                                            row[Can_ReportHDTJobCardMoreEntity.FieldNames.Hour] = meal.TimeLog.Value.ToString("HH:mm:ss");
                                        }
                                        var count1 = 0;
                                        for (DateTime date1 = dateStart; date1 <= dateEnd; date1 = date1.AddDays(1))
                                        {
                                            int countDay = mealRecordProfiles.Count(s => s.WorkDay.Value.Date == date1.Date && !leaveDayDates.Contains(date1.Date));
                                            if (countDay > 1)
                                            {
                                                count1 += countDay - 1;
                                            }
                                        }
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.Total] = count1;
                                        row["Data" + date.Day] = mealDoubles.Sum(s => s.Amount);
                                        var cate = caterings.FirstOrDefault(s => s.ID == meal.CateringID);
                                        var cateen = canteens.FirstOrDefault(s => s.ID == meal.CanteenID);
                                        var line = lines.FirstOrDefault(s => s.ID == meal.LineID);
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CateringName] = cate != null ? cate.CateringName : string.Empty; ;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.CanteenName] = cateen != null ? cateen.CanteenName : string.Empty; ;
                                        row[Can_ReportHDTJobCardMoreEntity.FieldNames.LineName] = line != null ? line.LineName : string.Empty;
                                        if (hdtType != null)
                                        {
                                            var hdttype = lines.FirstOrDefault(s => s.HDTJ == hdtType.Type);
                                            if (hdttype != null)
                                            {
                                                row[Can_ReportHDTJobCardMoreEntity.FieldNames.PriceStardand] = (object)hdttype.Amount ?? DBNull.Value;
                                            }
                                        }
                                        datatable.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return datatable;
        }
        #endregion
    }
}
