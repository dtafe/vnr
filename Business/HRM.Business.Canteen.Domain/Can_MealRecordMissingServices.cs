using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Business.HrmSystem.Models;
using System.Data;
using System.Data.OleDb;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace HRM.Business.Canteen.Domain
{
    public class Can_MealRecordMissingServices : BaseService
    {
        public void ComputeMealRecordMissing(List<Guid> lstProfileIds, DateTime dateFrom, DateTime dateToSearch, List<Guid> TamScanReasonMissID, string Status, List<Guid?> lstAllowanceTypeIDs)
        {
            List<Can_MealRecordMissingEntity> lstMealRecord = new List<Can_MealRecordMissingEntity>();
            using (var context = new VnrHrmDataContext())
            {
                DateTime dateTo = DateTime.Now;
                if (dateToSearch != null && dateToSearch != SqlDateTime.MaxValue.Value)
                {
                     dateTo = dateToSearch.AddDays(1).AddMinutes(-1);
                }
                if(lstProfileIds == null)
                {
                    return;
                }
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                string statusAllowance = StatusMealAllowanceToMoney.E_APPROVED.ToString();
                var repoMealAllowance = new Can_MealAllowanceToMoneyRepository(unitOfWork);
                var mealAllowanceProfies = repoMealAllowance.FindBy(s => s.Status == statusAllowance && dateFrom <= s.DateTo && s.DateFrom <= dateTo)
                    .Select(s => new { s.ProfileID, s.MealAllowanceTypeID, s.DateFrom, s.DateTo }).ToList();

                var repoMissReason = new Cat_TAMScanReasonMissRepository(unitOfWork);
                var resons = repoMissReason.FindBy(s => s.IsForCMS == true).Select(s => new { s.ID, s.IsFullPay }).ToList();

                var repoWorkday = new Att_WorkDayRepository(unitOfWork);
                var workDays = repoWorkday.FindBy(s => (s.FirstInTime != null || s.LastOutTime != null) && dateFrom <= s.WorkDate && s.WorkDate <= dateTo && lstProfileIds.Contains(s.ProfileID))
               .Select(s => new { s.ProfileID, s.WorkDate, s.MissInOutReason, s.Status, s.FirstInTime, s.LastOutTime }).ToList();

                var repoMealRecord = new Can_MealRecordRepository(unitOfWork);
                var mealRecordProfiles = repoMealRecord.FindBy(s => s.WorkDay != null && s.ProfileID != null
                && dateFrom <= s.WorkDay && s.WorkDay <= dateTo && lstProfileIds.Contains(s.ProfileID.Value))
                .Select(s => new { s.ProfileID, s.TimeLog, s.WorkDay }).ToList();

                var statusLeaveDay = AttendanceDataStatus.E_APPROVED.ToString();
                var repoLeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leaveDayProfiles = repoLeaveDay.FindBy(s => s.Status == statusLeaveDay && dateFrom <= s.DateEnd && s.DateStart <= dateTo)
               .Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();
                var tamSanResons = repoMissReason.GetAll().ToList();
                var repoMealAllowanceTypeSettings = new Can_MealAllowanceTypeSettingRepository(unitOfWork);
                var mealSesttings = repoMealAllowanceTypeSettings.GetAll().Select(s => new { s.ID, s.Amount, s.MealAllowanceTypeSettingName }).ToList();
                if (TamScanReasonMissID != null && TamScanReasonMissID[0] != Guid.Empty)
                {
                    workDays = workDays.Where(s => s.MissInOutReason != null && TamScanReasonMissID.Contains(s.MissInOutReason.Value)).ToList();
                }
                if (Status != null)
                {
                    workDays = workDays.Where(s => s.Status != null && Status == s.Status).ToList();
                }
                var repoRecordMissing = new Can_MealRecordMissingRepository(unitOfWork);
                List<Can_MealRecordMissing> mealRecordMissings = repoRecordMissing.FindBy(s => dateFrom <= s.WorkDate && s.WorkDate <= dateTo && s.ProfileID != null
                    && lstProfileIds.Contains(s.ProfileID.Value)).ToList();
                var workDayProfies = repoWorkday.FindBy(s => (s.InTime1 != null || s.OutTime1 != null) && dateFrom <= s.WorkDate
                   && s.WorkDate <= dateTo && lstProfileIds.Contains(s.ProfileID)).Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID }).ToList();
                var profileIDs = workDayProfies.Select(s => s.ProfileID).Distinct().ToList();
                var lstmeal = new List<Can_MealRecordMissing>();
                foreach (var profileID in profileIDs)
                {
                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        var recordProfileIDs = mealRecordProfiles.Where(s => s.WorkDay.Value.Date == date.Date && s.ProfileID == profileID).Select(s => s.ProfileID).ToList();
                        var leavedayProfileIDs = leaveDayProfiles.Where(s => s.DateStart != null && s.DateEnd != null && s.DateStart.Date <= date.Date
                            && date.Date <= s.DateEnd.Date && s.ProfileID == profileID).Select(s => s.ProfileID).ToList();
                        var workDayProfiles = workDays.Where(s => s.WorkDate.Date == date.Date && s.ProfileID == profileID && !recordProfileIDs.Contains(s.ProfileID) && !leavedayProfileIDs.Contains(s.ProfileID)).ToList();
                        foreach (var workDay in workDayProfiles)
                        {
                            var meal = mealRecordMissings.FirstOrDefault(s => s.ProfileID == workDay.ProfileID && s.WorkDate.Value.Date == workDay.WorkDate.Date);
                            if (meal == null)
                            {
                                meal = new Can_MealRecordMissing();
                                meal.ID = Guid.Empty;
                                lstmeal.Add(meal);
                            }
                            meal.ProfileID = workDay.ProfileID;
                            meal.WorkDate = workDay.WorkDate;
                            if (meal.TamScanReasonMissID == Guid.Empty)
                            {
                                if (workDay.MissInOutReason != Guid.Empty)
                                {
                                    meal.TamScanReasonMissID = workDay.MissInOutReason;
                                    var tamScan = tamSanResons.FirstOrDefault(s => s.ID == workDay.MissInOutReason);
                                    if (tamScan != null)
                                    {
                                        meal.MealAllowanceTypeSettingID = tamScan.MealAllowanceTypeSettingID;
                                        meal.TamScanReasonMissID = tamScan.ID;
                                        var messting = mealSesttings.FirstOrDefault(s => s.ID == tamScan.MealAllowanceTypeSettingID);
                                        if (messting != null)
                                        {
                                            meal.Amount = messting.Amount;
                                        }
                                    }
                                }
                                else
                                {
                                    var allownce = mealAllowanceProfies.FirstOrDefault(s => s.ProfileID == workDay.ProfileID &&
                                        s.DateFrom <= date && date <= s.DateTo);
                                    if (allownce != null)
                                    {
                                        meal.MealAllowanceTypeSettingID = allownce.MealAllowanceTypeID;
                                        var messting = mealSesttings.FirstOrDefault(s => s.ID == allownce.MealAllowanceTypeID);
                                        if (messting != null)
                                        {
                                            meal.Amount = messting.Amount;
                                        }
                                    }
                                }

                            }
                            if (meal.MealAllowanceTypeSettingID != Guid.Empty)
                            {
                                meal.Status = MealRecord_Status.E_APPROVED.ToString();
                            }
                            else
                            {
                                meal.Status = MealRecord_Status.E_SUBMIT.ToString();
                            }
                            meal.Type = WorkdaySrcType.E_MANUAL.ToString();
                            if (workDay.FirstInTime == null && workDay.LastOutTime == null && workDay.MissInOutReason != Guid.Empty)
                            {
                                var tamSanReson = resons.FirstOrDefault(s => s.ID == workDay.MissInOutReason);
                                if (tamSanReson != null)
                                {
                                    meal.IsFullPay = tamSanReson.IsFullPay;
                                }
                            }
                        }
                    }
                }

                 repoRecordMissing.Add(lstmeal);
                repoMissReason.SaveChanges();
            }
        }

        public void SubmitStatus(List<Guid> selectedIds, string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealRecordMissingRepository(unitOfWork);
                var lstMealRecordMissing = repo.FindBy(m => m.ID != null && selectedIds.Contains(m.ID)).ToList();
                foreach (var mealRecordMissing in lstMealRecordMissing)
                {
                    mealRecordMissing.Status = status;
                    repo.SaveChanges();
                }
               

            }
        }
    }
}
