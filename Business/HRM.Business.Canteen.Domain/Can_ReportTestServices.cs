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

namespace HRM.Business.Canteen.Domain
{
    public class Can_ReportTestServices : BaseService
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
            var profiles = new List<Hre_Profile>().Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                profiles = repo.FindBy(s => cardcode.Contains(s.CodeEmp)).Select(s => new { s.Id, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
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
                ReportAdjustmentMealAllowancePayment.OrgStructureId = orgbyprofile;
                var mealProfiles = lstMealRecord.Where(s => s.CardCode == profile.CodeEmp).ToList();
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
                    }
                }
                lstReportMealTimeSummary.Add(ReportAdjustmentMealAllowancePayment);
            }
            return lstReportMealTimeSummary;
        }
        /// <summary>
        /// [Tam.Le] - 20140725 - Lấy dữ liệu BC Chi Tiết Nhân Viên Có Chấm Công Nhưng Không Ăn
        /// </summary>
        /// <param name="CarteringIDs"></param>
        /// <param name="CanteenIDS"></param>
        /// <param name="LineIDS"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
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
            if (CanteenIDS[0] != null)
            {
                mealRecords = mealRecords.Where(s => s.CanteenID != null && CanteenIDS.Contains(s.CanteenID.Value)).ToList();
            }
            if (CarteringIDs[0] != null)
            {
                mealRecords = mealRecords.Where(s => s.CateringID != null && CarteringIDs.Contains(s.CateringID.Value)).ToList();
            }
            if (LineIDS[0] != null)
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
                    reportMealTimeDetailNoEatEntity.OrgCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    reportMealTimeDetailNoEatEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    reportMealTimeDetailNoEatEntity.OrgName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
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
            if (CanteenIDS[0] != null)
            {
                mealRecords = mealRecords.Where(s => s.CanteenID != null && CanteenIDS.Contains(s.CanteenID.Value)).ToList();
            }
            if (CarteringIDs[0] != null)
            {
                mealRecords = mealRecords.Where(s => s.CateringID != null && CarteringIDs.Contains(s.CateringID.Value)).ToList();
            }
            if (LineIDS[0] != null)
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
                    reportMealTimeDetailEntity.OrgCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportMealTimeDetailEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportMealTimeDetailEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    reportMealTimeDetailEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    reportMealTimeDetailEntity.OrgName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
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
    }
}
