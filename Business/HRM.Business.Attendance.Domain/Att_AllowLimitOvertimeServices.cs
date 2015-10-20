using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Hr.Models;
using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;

namespace HRM.Business.Hr.Domain
{
    public class Att_AllowLimitOvertimeServices : BaseService
    {
        #region MyRegion

        ///// <summary>
        ///// Lấy danh sách tất cả giới hạn tăng ca cho phép
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AllowLimitOvertimeEntity> GetAllowLimitOvertimes(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AllowLimitOvertimeRepository repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        return repo.GetAllowLimitOvertimes(model);
        //    }
        //}
        ///// <summary>
        ///// Lấy toàn bộ data
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AllowLimitOvertime> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        return repo.Get().Where(i => i.IsDelete == null);
        //    }
        //}

        ///// <summary>
        ///// Lấy thông tin Att_AllowLimitOvertime bởi id
        ///// </summary>
        ///// <param name="profileId"></param>
        ///// <returns></returns>
        //public Att_AllowLimitOvertimeEntity GetAllowLimitOvertimeById(int allowLimitOvertime)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        var AllowLimitOvertime = repo.GetAllowLimitOvertimesbyid(allowLimitOvertime);
        //        return AllowLimitOvertime;
        //    }
        //}

        //public List<Att_AllowLimitOvertimeEntity> GetAllowLimitOvertimeByIds(string selectedIds)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        return repo.GetAllowLimitOvertimeByIds(selectedIds);
        //    }
        //}

        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Add(Att_AllowLimitOvertime model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(model);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Edit một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Edit(Att_AllowLimitOvertime model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(model);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Remove 1 record là chuyển trạng thái IsDelete=true
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Remove(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(data);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Delete 1 record là xóa luôn record khỏi database
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Delete(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AllowLimitOvertimeRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Delete(data);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}
        #endregion

        #region Mượn page để viết hàm phân tích phép năm

        public Double GetAnnualLeaveAvailableAllYear(int Year,  DateTime? dateHire,
           DateTime? dateEndProbation, DateTime? dateQuit,  string fomularConfig , List<Att_LeaveDayEntity> lstLeaveDay_ByType_ByProfile , List<DateTime> lstDayOff, Guid? HDTJob4, Guid? HDTJob5, List<Hre_WorkHistoryEntity> lstWorkingHistory_ByProfile)
        {
            #region Param
            int monthBeginYear = 0; //Tháng bắt đầu tính phép năm
            int dayBeginFullMonth = 0; //Ngày bắt đầu tính tròn ANL cho tháng
            int seniorMonth = 0; // Số tháng để có 1 level cho thâm niên
            int dayPerMonth = 0; // Số ngày cho 1 tháng
            double anlRoundUp = 0; //Số làm tròn Lên xuống
            string typeProfileBegin = string.Empty; //Loại lấy theo DateHire hay DateQuit
            int maxInMonthToGetAct = 0; //Ngày chuẩn để xét là DT4 và DT5 đc tính cho tháng àno
            double anlFullYear = 0; // Số ngày phép bình thường cho 1 năm (tính theo tháng)
            double anlSeniorMoreThanNormal = 0; // Số ngày phép Được cộng thêm do thâm niên so với bình thường (tính theo tháng)
            double anlHDT4MoreThanNormal = 0; // Số ngày phép được cộng thêm do HDT4 so với bình thường (tính theo tháng)
            double anlHDT5MoreThanNormal = 0; // Số ngày phép được cộng thêm do HDT5 so với bình thường (tính theo tháng)
            //set du lieu
            GetConfigANL(fomularConfig, out monthBeginYear, out dayBeginFullMonth, out seniorMonth,
                out dayPerMonth, out anlRoundUp, out typeProfileBegin, out maxInMonthToGetAct,
                out anlFullYear, out anlSeniorMoreThanNormal, out anlHDT4MoreThanNormal, out anlHDT5MoreThanNormal);
            #endregion

            #region Data
            //gan du lieu can thiet

            DateTime BeginYear = new DateTime(Year, monthBeginYear, 1);
            DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);
            DateTime? DateStartProfile = null;
            DateTime DateEndProfile = EndYear;

            if (typeProfileBegin == AnlProfileTypeBegin.E_DATE_ENDPROBATION.ToString())
            {
                DateStartProfile = dateEndProbation;
            }
            else
            {
                DateStartProfile = dateHire;
            }
            if (DateStartProfile == null)
                return 0;
            if (dateQuit != null && dateQuit < EndYear)
            {
                DateEndProfile = dateQuit.Value.Date.AddDays(1).AddMinutes(-1);
            }
            if (DateStartProfile.Value.Day > dayBeginFullMonth)
            {
                DateStartProfile = new DateTime(DateStartProfile.Value.AddMonths(1).Year, DateStartProfile.Value.AddMonths(1).Month, 1);
            }
            DateTime DateStartInYear = BeginYear > DateStartProfile.Value ? BeginYear : DateStartProfile.Value;
            DateTime DateEndInYear = EndYear < DateEndProfile ? EndYear : DateEndProfile;
            #endregion
            string formulaAnnualLeave = string.Empty; // lấy tư trong cong thức tính phép năm
            List<ElementFormula> listElementFormular = new List<ElementFormula>();
            if (formulaAnnualLeave.Contains(FormulaAnual.ANL_NORMAL.ToString()))
            {
                double value = 0;
                double monthWorkingNormalInYear = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (DateStartInYear.AddMonths(i) < DateEndInYear)
                    {
                        monthWorkingNormalInYear++;
                    }
                    else
                    {
                        break;
                    }
                }
                value = anlFullYear * monthWorkingNormalInYear;
                ElementFormula ElementFormula = new ElementFormula(FormulaAnual.ANL_NORMAL.ToString(), value, 0);
                listElementFormular.Add(ElementFormula);
            }
            if (formulaAnnualLeave.Contains(FormulaAnual.ANL_SENIOR.ToString()))
            {
                double value = 0;
                int level1 = 0;
                int level2 = 0;
                int MonthLevel1 = 0;
                int MonthLevel2 = 0;
                DateTime DateSenior = DateTime.MinValue;
                for (int i = 0; i < 20; i++ )
                {
                    DateSenior = DateStartProfile.Value.AddMonths(seniorMonth);
                    if (DateSenior < DateStartInYear)
                    {
                        level1++;
                        continue;
                    }
                    else if (DateSenior >= DateStartInYear && DateSenior < DateEndInYear)
                    {

                        for (int y = 0; y < 12;y++ )
                        {
                            if (DateStartInYear.AddMonths(y) < DateSenior)
                            {
                                MonthLevel1++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        level2= level1+1;
                        MonthLevel2 = 12 - MonthLevel1;
                    }
                    else
                    {
                        break;
                    }
                }
                value = (level1 * MonthLevel1 * anlSeniorMoreThanNormal) + (level2 * MonthLevel2 * anlSeniorMoreThanNormal);
                ElementFormula ElementFormula = new ElementFormula(FormulaAnual.ANL_SENIOR.ToString(), value, 1);
                listElementFormular.Add(ElementFormula);

            }
            if (formulaAnnualLeave.Contains(FormulaAnual.ANL_LEAVE_NON_HAVEANL.ToString()))
            {
                double value = 0;
                //Logic: Vướng cai đầu năm cuối năm nên phải cắt cái ngày đó ra cho chính xác
                double numLeave = 0;
                foreach (var item in lstLeaveDay_ByType_ByProfile)
	            {
                    if (item.DateStart < DateStartInYear && item.DateEnd > DateStartInYear)
                    {
                        DateTime FirstSunday = DateTime.MinValue;
                        for (DateTime DateCheck = DateStartInYear; DateCheck < item.DateEnd; DateCheck = DateCheck.AddDays(1))
                        {
                            if (DateCheck.DayOfWeek == DayOfWeek.Sunday)
                            {
                                FirstSunday = DateCheck;
                                break;
                            }
                        }
                        int sundayCount = 0;
                        if (FirstSunday != DateTime.MinValue)
                        {
                           sundayCount = (int)((item.DateEnd - FirstSunday).TotalDays / 7) + 1;
                        }
                        int dayOffCount = lstDayOff.Select(m => m.Date >= DateStartInYear && m.Date < item.DateEnd).Count();
                        numLeave += (item.DateEnd - DateStartInYear).TotalDays - sundayCount - dayOffCount;

                    }
                    else if (item.DateStart < DateEndInYear && item.DateEnd > DateEndInYear)
                    {

                        DateTime FirstSunday = DateTime.MinValue;
                        for (DateTime DateCheck = item.DateStart; DateCheck < DateEndInYear; DateCheck = DateCheck.AddDays(1))
                        {
                            if (DateCheck.DayOfWeek == DayOfWeek.Sunday)
                            {
                                FirstSunday = DateCheck;
                                break;
                            }
                        }
                        int sundayCount = 0;
                        if (FirstSunday != DateTime.MinValue)
                        {
                            sundayCount = (int)((DateEndInYear - FirstSunday).TotalDays / 7) + 1;
                        }
                        int dayOffCount = lstDayOff.Select(m => m.Date >= item.DateStart && m.Date < DateEndInYear).Count();
                        numLeave += (item.DateEnd - DateStartInYear).TotalDays - sundayCount - dayOffCount;
                    }
                    else
                    {
                        numLeave += item.LeaveDays ?? 0;
                    }
		 
	            }
                value = ((int)(numLeave / dayPerMonth)) + ((numLeave % dayPerMonth) >= anlRoundUp ? 1 : 0);
                ElementFormula ElementFormula = new ElementFormula(FormulaAnual.ANL_LEAVE_NON_HAVEANL.ToString(), value, 2);
                listElementFormular.Add(ElementFormula);

            }
            if (formulaAnnualLeave.Contains(FormulaAnual.ANL_WORK_HDT4.ToString()))
            {
                double value = 0;
                double monthCount = 0;
                double dayCount = 0;
                //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                Hre_WorkHistoryEntity workHistoryNewest_beforeBeginYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateStartInYear && m.JobTitleID==HDTJob4).OrderByDescending(m => m.DateEffective).FirstOrDefault();
                if (workHistoryNewest_beforeBeginYear == null || workHistoryNewest_beforeBeginYear.JobTitleID != HDTJob4)
                {
                   Hre_WorkHistoryEntity workHistoryAfterYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > DateStartInYear && m.DateEffective < DateEndInYear && m.JobTitleID == HDTJob4).OrderBy(m => m.DateEffective).FirstOrDefault();
                   if (workHistoryAfterYear != null)
                   {
                       DateTime dateStartJob = workHistoryAfterYear.DateEffective;
                       for (int i = 0; i < 20; i++)
                       {
                           Hre_WorkHistoryEntity workHistoryNextStep = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateStartJob && m.JobTitleID != HDTJob4).OrderBy(m => m.DateEffective).FirstOrDefault();
                           DateTime dateEndJob = DateTime.MaxValue;
                           if (workHistoryNextStep != null)
                           {
                               dateEndJob = workHistoryNextStep.DateEffective;
                           }
                           if (dateEndJob > DateEndInYear)
                           {
                               dateEndJob = DateEndInYear;
                           }
                           dayCount = (dateEndJob - DateStartInYear).TotalDays;

                           //Kiếm cặp tiếp theo
                           Hre_WorkHistoryEntity workHistoryNextStep2 = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateEndJob && m.JobTitleID == HDTJob4).OrderBy(m => m.DateEffective).FirstOrDefault();
                           if (workHistoryNextStep2 == null || workHistoryNextStep2.DateEffective > DateEndInYear)
                           {
                               break;
                           }
                           else
                           {
                               dateStartJob = workHistoryNextStep2.DateEffective;
                           }

                       }
                   }
                }
                else
                {
                    Hre_WorkHistoryEntity workHistoryNewest_beforeEndYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateEndInYear && m.JobTitleID==HDTJob4).OrderByDescending(m => m.DateEffective).FirstOrDefault();
                    if (workHistoryNewest_beforeBeginYear.ID == workHistoryNewest_beforeEndYear.ID)
                    {
                        //nếu như làm HDT Nguyên năm
                        dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                    }
                    else
                    {
                        //Lấy từng cặp trong khảong thời gian cho so voi ngày dateStartInyear và ngày dateEndInyear
                        DateTime dateStartJob = workHistoryNewest_beforeBeginYear.DateEffective;
                        for (int i = 0; i < 20; i++)
                        {
                            //Lấy ra cái mới nhất so với ngày hiện tại đang check
                            Hre_WorkHistoryEntity workHistoryNextStep = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateStartJob && m.JobTitleID != HDTJob4).OrderBy(m => m.DateEffective).FirstOrDefault();
                            DateTime dateEndJob = DateTime.MaxValue;
                            if (workHistoryNextStep != null)
                            {
                               dateEndJob = workHistoryNextStep.DateEffective;
                            }
                            if (dateEndJob > DateEndInYear)
                            {
                                dateEndJob = DateEndInYear;
                            }
                            dayCount = (dateEndJob - DateStartInYear).TotalDays;

                            //Kiếm cặp tiếp theo
                            Hre_WorkHistoryEntity workHistoryNextStep2 = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateEndJob && m.JobTitleID == HDTJob4).OrderBy(m => m.DateEffective).FirstOrDefault();
                            if (workHistoryNextStep2 == null || workHistoryNextStep2.DateEffective > DateEndInYear)
                            {
                                break;
                            }
                            else
                            {
                                dateStartJob = workHistoryNextStep2.DateEffective;
                            }

                        }

                    }
                }

                monthCount = ((int)(dayCount / dayPerMonth)) + ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                value = monthCount * anlHDT4MoreThanNormal;
                ElementFormula ElementFormula = new ElementFormula(FormulaAnual.ANL_WORK_HDT4.ToString(), value, 3);
                listElementFormular.Add(ElementFormula);

            }

            if (formulaAnnualLeave.Contains(FormulaAnual.ANL_WORK_HDT5.ToString()))
            {
                double value = 0;
                double monthCount = 0;
                double dayCount = 0;
                //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                Hre_WorkHistoryEntity workHistoryNewest_beforeBeginYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateStartInYear && m.JobTitleID == HDTJob5).OrderByDescending(m => m.DateEffective).FirstOrDefault();
                if (workHistoryNewest_beforeBeginYear == null || workHistoryNewest_beforeBeginYear.JobTitleID != HDTJob5)
                {
                    Hre_WorkHistoryEntity workHistoryAfterYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > DateStartInYear && m.DateEffective < DateEndInYear && m.JobTitleID == HDTJob5).OrderBy(m => m.DateEffective).FirstOrDefault();
                    if (workHistoryAfterYear != null)
                    {
                        DateTime dateStartJob = workHistoryAfterYear.DateEffective;
                        for (int i = 0; i < 20; i++)
                        {
                            Hre_WorkHistoryEntity workHistoryNextStep = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateStartJob && m.JobTitleID != HDTJob5).OrderBy(m => m.DateEffective).FirstOrDefault();
                            DateTime dateEndJob = DateTime.MaxValue;
                            if (workHistoryNextStep != null)
                            {
                                dateEndJob = workHistoryNextStep.DateEffective;
                            }
                            if (dateEndJob > DateEndInYear)
                            {
                                dateEndJob = DateEndInYear;
                            }
                            dayCount = (dateEndJob - DateStartInYear).TotalDays;

                            //Kiếm cặp tiếp theo
                            Hre_WorkHistoryEntity workHistoryNextStep2 = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateEndJob && m.JobTitleID == HDTJob5).OrderBy(m => m.DateEffective).FirstOrDefault();
                            if (workHistoryNextStep2 == null || workHistoryNextStep2.DateEffective > DateEndInYear)
                            {
                                break;
                            }
                            else
                            {
                                dateStartJob = workHistoryNextStep2.DateEffective;
                            }

                        }
                    }
                }
                else
                {
                    Hre_WorkHistoryEntity workHistoryNewest_beforeEndYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateEndInYear && m.JobTitleID == HDTJob5).OrderByDescending(m => m.DateEffective).FirstOrDefault();
                    if (workHistoryNewest_beforeBeginYear.ID == workHistoryNewest_beforeEndYear.ID)
                    {
                        //nếu như làm HDT Nguyên năm
                        dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                    }
                    else
                    {
                        //Lấy từng cặp trong khảong thời gian cho so voi ngày dateStartInyear và ngày dateEndInyear
                        DateTime dateStartJob = workHistoryNewest_beforeBeginYear.DateEffective;
                        for (int i = 0; i < 20; i++)
                        {
                            //Lấy ra cái mới nhất so với ngày hiện tại đang check
                            Hre_WorkHistoryEntity workHistoryNextStep = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateStartJob && m.JobTitleID != HDTJob5).OrderBy(m => m.DateEffective).FirstOrDefault();
                            DateTime dateEndJob = DateTime.MaxValue;
                            if (workHistoryNextStep != null)
                            {
                                dateEndJob = workHistoryNextStep.DateEffective;
                            }
                            if (dateEndJob > DateEndInYear)
                            {
                                dateEndJob = DateEndInYear;
                            }
                            dayCount = (dateEndJob - DateStartInYear).TotalDays;

                            //Kiếm cặp tiếp theo
                            Hre_WorkHistoryEntity workHistoryNextStep2 = lstWorkingHistory_ByProfile.Where(m => m.DateEffective > dateEndJob && m.JobTitleID == HDTJob5).OrderBy(m => m.DateEffective).FirstOrDefault();
                            if (workHistoryNextStep2 == null || workHistoryNextStep2.DateEffective > DateEndInYear)
                            {
                                break;
                            }
                            else
                            {
                                dateStartJob = workHistoryNextStep2.DateEffective;
                            }

                        }

                    }
                }

                monthCount = ((int)(dayCount / dayPerMonth)) + ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                value = monthCount * anlHDT5MoreThanNormal;
                ElementFormula ElementFormula = new ElementFormula(FormulaAnual.ANL_WORK_HDT5.ToString(), value, 4);
                listElementFormular.Add(ElementFormula);

            }

            double result = (double) FormulaHelper.ParseFormula(formulaAnnualLeave,listElementFormular).Value;
            return result;
        }


        private void GetConfigANL(string fomularConfig, out int monthBeginYear, out int dayBeginFullMonth, out int seniorMonth, out int dayPerMonth,
            out double anlRoundUp, out string typeProfileBegin, out int maxInMonthToGetAct,
            out double anlFullYear, out double anlSeniorMoreThanNormal, out double anlHDT4MoreThanNormal, out double anlHDT5MoreThanNormal)
        {
            #region set default value
            monthBeginYear = 1;
            dayBeginFullMonth = 1;
            seniorMonth = 12 * 5;
            dayPerMonth = 30;
            anlRoundUp = 0.5;
            typeProfileBegin = AnlProfileTypeBegin.E_DATE_HIRE.ToString();
            maxInMonthToGetAct = 15; //Trươc ngày 15 thì tính cho tháng đó
            anlFullYear = 12 / 12;
            anlSeniorMoreThanNormal = 1 / 12;
            anlHDT4MoreThanNormal = 2 / 12;
            anlHDT5MoreThanNormal = 4 / 12;
            #endregion

            #region set value by Config

            List<string> lstConfig = fomularConfig.Split(';').ToList();
            Dictionary<string, string> dicConfigANL = new Dictionary<string, string>();
            foreach (var item in lstConfig)
            {
                item.Replace("[", "");
                item.Replace("]", "");

                List<string> config = item.Split(':').ToList();
                if (config.Count > 1)
                {
                    dicConfigANL.Add(config[0], config[1]);
                }
            }

            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_MONTHBEGINYEAR.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_MONTHBEGINYEAR.ToString()], out value);
                monthBeginYear = value;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()], out value);
                dayBeginFullMonth = value;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_SENIOR_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_SENIOR_MONTH.ToString()], out value);
                seniorMonth = value;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_DAY_PER_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_DAY_PER_MONTH.ToString()], out value);
                dayPerMonth = value;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_ROUND_UP.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_ROUND_UP.ToString()], out value);
                anlRoundUp = value;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()))
            {
                typeProfileBegin = dicConfigANL[FormulaAnual.CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()];
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()], out value);
                maxInMonthToGetAct = value;
            }

            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()], out value);
                anlFullYear = value / 12;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()], out value);
                anlSeniorMoreThanNormal = value / 12;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()], out value);
                anlHDT4MoreThanNormal = value / 12;
            }
            if (dicConfigANL.ContainsKey(FormulaAnual.CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[FormulaAnual.CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()], out value);
                anlHDT5MoreThanNormal = value / 12;
            }


            #endregion
        }

        #endregion


    }
}
