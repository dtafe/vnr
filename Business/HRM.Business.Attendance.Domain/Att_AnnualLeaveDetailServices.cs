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
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Attendance.Domain;

namespace HRM.Business.Hr.Domain
{
    public class Att_AnnualLeaveDetailServices : BaseService
    {
        public Double GetAnnualLeaveAvailableAllYear()
        {
            Random a = new Random();
            return a.Next(11, 100);
        }

        public bool CalculateAnnualLeaveDetails(int year)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_AnnualLeaveDetailRepository(unitOfWork);
                var rs = repo.CalculateAnnualLeaveDetails(year);
                return false;
            }
        }

        public bool AddList(List<Att_AnnualLeaveDetailEntity> models)
        {
            bool isSuccess = false;
            var leaveType = string.Empty;
            var status = string.Empty;
            var year = 2013;
            if (models != null && models.FirstOrDefault() != null && models.FirstOrDefault().Year != null) 
            {
                year = Convert.ToInt32(models.FirstOrDefault().Year);
                leaveType = models.FirstOrDefault().Type;
            }

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
                try
                {
                    var annualLeaveDetails = GetAllUseEntity<Att_AnnualLeaveDetail>(ref status).Where(p => p.Type == leaveType && p.Year == year).ToList().AsQueryable();
                    foreach (var attAnnualLeaveDetail in models)
                    {
                        var addSuccess = false;
                        var existAnnualDetail = annualLeaveDetails.Where(p => p.ProfileID == attAnnualLeaveDetail.ProfileID ).FirstOrDefault();
                        if (existAnnualDetail != null)
                        {
                            existAnnualDetail.Month1 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month2 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month3 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month4 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month5 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month6 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month7 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month8 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month9 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month10 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month11 = attAnnualLeaveDetail.Month1;
                            existAnnualDetail.Month12 = attAnnualLeaveDetail.Month1;

                            repo.Edit(existAnnualDetail);
                        }
                        else
                        {
                            Att_AnnualLeaveDetail temp = new Att_AnnualLeaveDetail();
                            temp = attAnnualLeaveDetail.CopyData<Att_AnnualLeaveDetail>();
                            repo.Add(temp);
                        }
                    }
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        


        #region hàm phân tích phép năm

        public Double GetAnnualLeaveAvailableAllYear(int Year, int Month, DateTime? dateHire,
           DateTime? dateEndProbation, DateTime? dateQuit, string fomularConfig, List<Att_LeaveDayEntity> lstLeaveDay_ByType_ByProfile, List<DateTime> lstDayOff, Guid? HDTJob4, Guid? HDTJob5, List<Hre_WorkHistoryEntity> lstWorkingHistory_ByProfile)
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
                for (int i = 0; i < 20; i++)
                {
                    DateSenior = DateStartProfile.Value.AddMonths(seniorMonth);
                    if (DateSenior < DateStartInYear)
                    {
                        level1++;
                        continue;
                    }
                    else if (DateSenior >= DateStartInYear && DateSenior < DateEndInYear)
                    {

                        for (int y = 0; y < 12; y++)
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
                        level2 = level1 + 1;
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
                Hre_WorkHistoryEntity workHistoryNewest_beforeBeginYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateStartInYear && m.JobTitleID == HDTJob4).OrderByDescending(m => m.DateEffective).FirstOrDefault();
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
                    Hre_WorkHistoryEntity workHistoryNewest_beforeEndYear = lstWorkingHistory_ByProfile.Where(m => m.DateEffective < DateEndInYear && m.JobTitleID == HDTJob4).OrderByDescending(m => m.DateEffective).FirstOrDefault();
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

            double result = (double)FormulaHelper.ParseFormula(formulaAnnualLeave, listElementFormular).Value;
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


        #region Hảm Phân Tích Phép Năm
        /// <summary>
        /// Button Tìm Kiếm Trong Phân Tích Phép Năm
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public List<Att_AnnualLeaveDetailEntity> SearchAnnualLeaveDetail(int Year, string orgStructure, string LstProfileStatus,string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);

                string E_ANNUAL_LEAVE = AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();

                Att_AnnualLeaveDetail AnnualLeaveDetailTop = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_ANNUAL_LEAVE && m.Year == Year)
                    .FirstOrDefault();
                if (AnnualLeaveDetailTop == null)
                {
                    return new List<Att_AnnualLeaveDetailEntity>();
                }
                DateTime beginyear = (AnnualLeaveDetailTop == null || AnnualLeaveDetailTop.MonthStart == null) ? new DateTime(Year, 1, 1) : AnnualLeaveDetailTop.MonthStart.Value;
                DateTime endYear = beginyear.AddYears(1).AddMinutes(-1);

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileQuery = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                if (LstProfileStatus != null)
                {
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_NEWEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateHire != null && m.DateHire >= beginyear && m.DateHire <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_RESIGNEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateQuit != null && m.DateQuit >= beginyear && m.DateQuit <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT4.ToString())
                    {
                        string Job4 = HDTJobType.E_Four.ToString();
                        List<Guid> lstprofileHDT4 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job4 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value)
                            .ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT4.Contains(m.ID)).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT5.ToString())
                    {
                        string Job5 = HDTJobType.E_Five.ToString();
                        List<Guid> lstprofileHDT5 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job5 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT5.Contains(m.ID)).ToList();
                    }
                }

                List<Guid> lstProfileID = lstProfileQuery.Select(m => m.ID).Distinct().ToList<Guid>();
                List<Att_AnnualLeaveDetailEntity> lstAnnDetail = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_ANNUAL_LEAVE && m.Year == Year
                    && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value)).ToList().Translate<Att_AnnualLeaveDetailEntity>();

                foreach (var ann in lstAnnDetail)
                {
                    var pro = lstProfileQuery.Where(s => s.ID == ann.ProfileID).FirstOrDefault();

                    ann.ProfileName = pro.ProfileName ?? string.Empty;
                    ann.CodeEmp = pro.CodeEmp ?? string.Empty;
                    ann.OrgStructureName = pro.OrgStructureName ?? string.Empty;
                    ann.DateHire = pro.DateHire ?? null;
                }
                return lstAnnDetail;
            }
        }

        /// <summary>
        /// Button Tính Phép Năm
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public bool ComputeAnnualLeaveDetail(int Year, string orgStructure, string LstProfileStatus, string UserLogin)
        {
            var result = false;
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);

                #region FilterInfo

                string E_ANNUAL_LEAVE = AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();

                Att_AnnualLeaveDetail AnnualLeaveDetailTop = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_ANNUAL_LEAVE && m.Year == Year).FirstOrDefault();
                DateTime beginyear = (AnnualLeaveDetailTop == null || AnnualLeaveDetailTop.MonthStart == null) ? new DateTime(Year, 1, 1) : AnnualLeaveDetailTop.MonthStart.Value;
                DateTime endYear = beginyear.AddYears(1).AddMinutes(-1);

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileQuery = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                if (LstProfileStatus != null)
                {
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_NEWEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateHire != null && m.DateHire >= beginyear && m.DateHire <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_RESIGNEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateQuit != null && m.DateQuit >= beginyear && m.DateQuit <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT4.ToString())
                    {
                        string Job4 = HDTJobType.E_Four.ToString();
                        List<Guid> lstprofileHDT4 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job4 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT4.Contains(m.ID)).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT5.ToString())
                    {
                        string Job5 = HDTJobType.E_Five.ToString();
                        List<Guid> lstprofileHDT5 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job5 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT5.Contains(m.ID)).ToList();
                    }
                }

                List<Guid> lstProfileID = lstProfileQuery.Select(m => m.ID).Distinct().ToList<Guid>();

                #endregion
                #region get Data
                string HRM_ATT_ANNUALLEAVE_ = AppConfig.HRM_ATT_ANNUALLEAVE_.ToString();
                List<object> lstO = new List<object>();
                lstO.Add(HRM_ATT_ANNUALLEAVE_);
                lstO.Add(null);
                lstO.Add(null);
                var config = GetData<Sys_AllSettingEntity>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);

                var formular1 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
                var formular2 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALLEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();

                if (config == null || string.IsNullOrEmpty(formular1.Value1) || string.IsNullOrEmpty(formular2.Value1))
                {
                    //Common.MessageBoxs(Messages.Msg, Messages.PleaseConfigAnnualLeaveAtTotalConfig, MessageBox.Icon.WARNING, string.Empty);
                    return result;
                }

                string formularConfig = formular1.Value1;
                string formularCompute = formular2.Value1;

                ParamGetConfigANL configAnl = new ParamGetConfigANL();
                (new Att_AttendanceServices()).GetConfigANL(formularConfig, out configAnl);

                int MonthBegin = 1;
                List<string> lstCodeLeaveTypeNonAnl = new List<string>();
                if (configAnl != null && configAnl.monthBeginYear != null)
                {
                    MonthBegin = configAnl.monthBeginYear;
                }
                if (configAnl != null && configAnl.lstCodeLeaveNonANL != null)
                {
                    lstCodeLeaveTypeNonAnl = configAnl.lstCodeLeaveNonANL;
                }
                DateTime BeginYear = new DateTime(Year, MonthBegin, 1);
                DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);

                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                List<DateTime> lstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff > BeginYear && m.DateOff <= EndYear)
                    .Select(m => m.DateOff)
                    .Distinct().ToList<DateTime>();
                List<Hre_ProfileEntity> lstprofile = lstProfileQuery
                    .Where(m => m.DateQuit == null || (m.DateQuit != null && m.DateQuit > BeginYear))
                    .ToList();
                List<Guid> lstLeavedayTypeNonAnl = repoCat_LeaveDayType
                    .FindBy(m => m.IsDelete == null && lstCodeLeaveTypeNonAnl.Contains(m.Code))
                    .Select(m => m.ID).ToList<Guid>();
                List<Att_LeaveDay> lstleavedayNonANl = repoAtt_LeaveDay
                    .FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && lstLeavedayTypeNonAnl.Contains(m.LeaveDayTypeID)
                        && lstProfileID.Contains(m.ProfileID))
                        .ToList();
                List<Att_LeaveDay> lstleavedayNonANlInYear = lstleavedayNonANl
                    .Where(m => m.DateStart <= EndYear && m.DateEnd >= BeginYear)
                    .ToList();
                List<Hre_HDTJob> lstHDTJob = repoHre_HDTJob
                    .FindBy(m => m.IsDelete == null && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value))
                    .ToList();
                List<Cat_JobTitle> lstJobtitle = repoCat_JobTitle.FindBy(s => s.IsDelete == null).ToList();
                #endregion
                #region Check Data
                string CodeEmpTmp = "";
                char[] ext = new char[] { ',' };
                List<string> LstCodeEmpTmp = CodeEmpTmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (LstCodeEmpTmp.Count >= 1)
                {
                    lstprofile = lstprofile.Where(m => LstCodeEmpTmp.Contains(m.CodeEmp)).ToList();
                }
                #endregion
                List<Att_AnnualLeaveDetail> lstAnnualDetail = new List<Att_AnnualLeaveDetail>();
                foreach (var item in lstprofile)
                {
                    List<Att_LeaveDay> lstLeavedayNonAnlByProfile = lstleavedayNonANl.Where(m => m.ProfileID == item.ID).ToList();
                    List<Att_LeaveDay> lstLeavedayNonAnlByProfileInYear = lstleavedayNonANlInYear.Where(m => m.ProfileID == item.ID).ToList();
                    List<Hre_HDTJob> lstHDTJobByProfile = lstHDTJob.Where(m => m.ProfileID == item.ID).ToList();
                    double AvailabelInYear = (new Att_AttendanceServices()).GetAnnualLeaveAvailableAllYear(Year, null, item.DateHire,
                   item.DateEndProbation, item.DateQuit, formularConfig,
                   formularCompute, lstLeavedayNonAnlByProfileInYear, lstJobtitle, lstDayOff, lstHDTJobByProfile, lstleavedayNonANl);

                    Att_AnnualLeaveDetail annualProfile = new Att_AnnualLeaveDetail();
                    annualProfile.ID = Guid.NewGuid();
                    annualProfile.ProfileID = item.ID;
                    annualProfile.MonthStart = BeginYear;
                    annualProfile.MonthEnd = EndYear;
                    annualProfile.Year = Year;
                    annualProfile.Type = E_ANNUAL_LEAVE;
                    annualProfile.Month1 = AvailabelInYear;
                    annualProfile.Month2 = AvailabelInYear;
                    annualProfile.Month3 = AvailabelInYear;
                    annualProfile.Month4 = AvailabelInYear;
                    annualProfile.Month5 = AvailabelInYear;
                    annualProfile.Month6 = AvailabelInYear;
                    annualProfile.Month7 = AvailabelInYear;
                    annualProfile.Month8 = AvailabelInYear;
                    annualProfile.Month9 = AvailabelInYear;
                    annualProfile.Month10 = AvailabelInYear;
                    annualProfile.Month11 = AvailabelInYear;
                    annualProfile.Month12 = AvailabelInYear;
                    lstAnnualDetail.Add(annualProfile);
                }

                //DataErrorCode DataErr = DataErrorCode.Unknown;

                if (lstAnnualDetail.Count > 0)
                {
                    #region lấy dữ liệu dưới DB xóa đi
                    List<Guid> lstProfileID_InDB = lstAnnualDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).ToList();


                    List<Att_AnnualLeaveDetail> lstAnnualLeaveDetail_InDB = repoAtt_AnnualLeaveDetail
                        .FindBy(m => m.IsDelete == null && m.Year == Year && m.Type == E_ANNUAL_LEAVE && m.ProfileID != null
                            && lstProfileID_InDB.Contains(m.ProfileID.Value))
                            .ToList();
                    foreach (var item in lstAnnualLeaveDetail_InDB)
                    {
                        item.IsDelete = true;
                    }
                    #endregion

                    repoAtt_AnnualLeaveDetail.Add(lstAnnualDetail);
                    try
                    {
                        repoAtt_AnnualLeaveDetail.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    //if (DataErr == DataErrorCode.Success)
                    //{
                    //    Common.MessageBoxs(Messages.Msg, Messages.SaveSuccess, MessageBox.Icon.INFO, string.Empty);
                    //}
                    //else
                    //{
                    //    Common.MessageBoxs(Messages.Msg, Messages.SaveUnSuccess, MessageBox.Icon.INFO, string.Empty);
                    //}
                }
                //else
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.NoDataToCompute, MessageBox.Icon.WARNING, string.Empty);
                //}
                result = true;
            }
            return result;
        }
        #endregion
    }
}
