using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Linq;

namespace HRM.Business.Attendance.Domain
{
    public static class Att_WorkDayHelper
    {
        #region ComputeWorkDays
        /// <summary>
        /// Tính toán thời gian làm việc thực tế của nhân viên.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="listWorkDay"></param>
        /// <param name="listAllShiftItem"></param>
        /// <param name="listRosterByProfile"></param>
        /// <param name="listPregnancy"></param>
        /// <param name="gradeByProfile"></param>
        /// <returns></returns>
        public static List<WorkDay> ComputeWorkDays(Hre_Profile profile, List<Att_Workday> listInOut, List<Cat_ShiftItem> listAllShiftItem,
            List<Att_Roster> listRosterByProfile, List<Att_Pregnancy> listPregnancy, Cat_GradeAttendance gradeCfgByProfile, string userLogin)
        {
            List<WorkDay> result = new List<WorkDay>();

            if (profile != null && listInOut != null && listInOut.Count() > 0)
            {
                listInOut = listInOut.OrderBy(inout => inout.WorkDate).ThenBy(inout => inout.InTime1).ToList();
                List<DateTime> listWorkDate = listInOut.Select(d => d.WorkDate.Date).Distinct().OrderBy(d => d).ToList();

                if (listWorkDate != null && listWorkDate.Count() > 0)
                {
                    Dictionary<DateTime, Cat_OrgStructure> listOrgStructure = Att_AttendanceLib.GetDailyLines(profile,
                        listRosterByProfile, listWorkDate.FirstOrDefault(), listWorkDate.LastOrDefault());

                    foreach (DateTime workDate in listWorkDate)
                    {
                        Cat_OrgStructure orgLine = null;

                        if (listOrgStructure.ContainsKey(workDate))
                        {
                            //luôn luôn tồn tại 1 line
                            orgLine = listOrgStructure[workDate];
                        }

                        List<Att_Workday> listInOutByWorkDate = listInOut.Where(io => io.WorkDate.Date == workDate).ToList();

                        if (listInOutByWorkDate != null && listInOutByWorkDate.Count() > 0)
                        {
                            Att_Pregnancy pregnancyByWorkDay = listPregnancy.Where(d => d.DateStart != null && d.DateEnd != null
                                && d.DateStart.Value <= workDate && d.DateEnd >= workDate).FirstOrDefault();

                            List<IGrouping<Cat_Shift, Att_Workday>> listInOutGroup = listInOutByWorkDate.Where(d =>
                                d.Cat_Shift != null).GroupBy(d => d.Cat_Shift).ToList();

                            foreach (IGrouping<Cat_Shift, Att_Workday> inOutGroup in listInOutGroup)
                            {
                                List<Cat_ShiftItem> listShiftItem = listAllShiftItem.Where(it =>
                                    it.ShiftID == inOutGroup.Key.ID).ToList();

                                WorkDay workDayItem = CreateWorkDay(profile, inOutGroup.ToList(), orgLine,
                                    inOutGroup.Key, listShiftItem, pregnancyByWorkDay, gradeCfgByProfile, userLogin);

                                if (workDayItem != null)
                                {
                                    result.Add(workDayItem);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        #endregion
        #region IsWorkDay

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// Chỉ áp dụng cho trường hợp RosterType = E_ISDEFAULT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="listDayOff"></param>
        /// <returns></returns>
        public static bool IsWorkDay(DateTime date, Cat_GradeAttendance gradeCfg, List<Cat_DayOff> listDayOff)
        {
            bool result = false;

            if (listDayOff == null || !listDayOff.Any(d => d.DateOff.Date == date.Date
                && d.Type == HolidayType.E_WEEKEND_HLD.ToString()))
            {
                if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    //if (date.DayOfWeek == DayOfWeek.Monday && gradeCfg.Cat_Shift != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Tuesday && gradeCfg.Cat_Shift1 != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Wednesday && gradeCfg.Cat_Shift2 != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Thursday && gradeCfg.Cat_Shift3 != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Friday && gradeCfg.Cat_Shift4 != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Saturday && gradeCfg.Cat_Shift5 != null)
                    //{
                    //    result = true;
                    //}
                    //else if (date.DayOfWeek == DayOfWeek.Sunday && gradeCfg.Cat_Shift6 != null)
                    //{
                    result = true;
                    //}
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="listDailyShift"></param>
        /// <param name="listDayOff"></param>
        /// <returns></returns>
        public static bool IsWorkDay(DateTime date, Cat_GradeAttendance gradeCfg,
            Dictionary<DateTime, Cat_Shift> listDailyShift, List<Cat_DayOff> listDayOff)
        {
            bool result = false;

            if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER.ToString()
                || gradeCfg.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
            {
                result = IsWorkDay(date, listDailyShift);
            }
            //else if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
            //{
            //    result = IsWorkDay(date, gradeCfg, listDayOff);
            //}
            else if (gradeCfg.RosterType == GradeRosterType.E_ISHOLIDAY.ToString())
            {
                if (listDayOff == null || !listDayOff.Any(d => d.DateOff.Date == date.Date
                    && d.Type == HolidayType.E_WEEKEND_HLD.ToString()))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// Chỉ áp dụng cho trường hợp RosterType = E_ISDEFAULT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="listDayOff"></param>
        /// <returns></returns>
        //public static bool IsWorkDay(DateTime date, Cat_GradeAttendance gradeCfg, List<Cat_DayOff> listDayOff)
        //{
        //    bool result = false;

        //    if (listDayOff == null || !listDayOff.Any(d => d.DateOff.Date == date.Date
        //        && d.Type == HolidayType.E_WEEKEND_HLD.ToString()))
        //    {
        //        if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
        //        {
        //            if (date.DayOfWeek == DayOfWeek.Monday && gradeCfg.Cat_Shift != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Tuesday && gradeCfg.Cat_Shift1 != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Wednesday && gradeCfg.Cat_Shift2 != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Thursday && gradeCfg.Cat_Shift3 != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Friday && gradeCfg.Cat_Shift4 != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Saturday && gradeCfg.Cat_Shift5 != null)
        //            {
        //                result = true;
        //            }
        //            else if (date.DayOfWeek == DayOfWeek.Sunday && gradeCfg.Cat_Shift6 != null)
        //            {
        //                result = true;
        //            }
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// </summary>
        /// <param name="date"></param>
        /// <param name="listDailyShift"></param>
        /// <returns></returns>
        public static bool IsWorkDay(DateTime date, Dictionary<DateTime, Cat_Shift> listDailyShift)
        {
            return listDailyShift != null && listDailyShift.Any(d =>
                d.Key.Date == date.Date && d.Value != null);
        }

        #endregion
        private static WorkDay CreateWorkDay(Hre_Profile profile, List<Att_Workday> listInOutByWorkDate, Cat_OrgStructure orgLine,
            Cat_Shift shift, List<Cat_ShiftItem> listShiftItem, Att_Pregnancy pregnancyByWorkDay, Cat_GradeAttendance graCfgPro, string userLogin)
        {
            WorkDay result = null;

            listInOutByWorkDate = listInOutByWorkDate.Where(d => d.InTime1 != null
                && d.OutTime1 != null).OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).ToList();

            if (listInOutByWorkDate != null && listInOutByWorkDate.Count() > 0)
            {
                #region Khởi tạo

                result = new WorkDay();
                result.Cat_Shift = shift;
                result.WorkDuration = 0D;

                result.WorkDate = listInOutByWorkDate.Select(d => d.WorkDate).FirstOrDefault();//ngày làm việc
                DateTime timeShiftStart = result.WorkDate.Date.Add(shift.InTime.TimeOfDay);//thời gian bắt đầu của ca làm việc
                DateTime timeShiftEnd = timeShiftStart.AddHours(shift.CoOut);////thời gian kết thúc của ca làm việc (date + time)
                double totalTimeShift = timeShiftEnd.Subtract(timeShiftStart).TotalHours;

                //LamLe - 20121017 - Lay gio lam viec trong grade hay trong Shift
                double workingStandardHour = graCfgPro.GetWorkHouPerDay(result.WorkDate);
                //if (shift != null && graCfgPro != null && graCfgPro.WorkHoursType == GradeHoursType.E_SHIFT_HOURS.ToString())
                if (shift != null && graCfgPro != null)
                {
                    workingStandardHour = shift.WorkHours != null ? shift.WorkHours.Value : 0D;
                }

                listShiftItem = listShiftItem.Where(sh => sh.ShiftItemType == ShiftItemType.E_SHIFTBREAK.ToString()).OrderBy(p => p.CoFrom).ToList();
                Guid? lineID = orgLine != null ? orgLine.ID : profile.OrgStructureID;//LamLe - 20121030 - Xu ly truong hop co Line org trong Roster
                Cat_ShiftItem shiftItemFlex = listShiftItem.Where(p => p.OrgStructureID == lineID).FirstOrDefault();

                double realCoBreakStart = 0D;
                double realCoBreakEnd = 0D;

                //Vinhtran: Kiểm tra có giờ nghỉ giữa ca làm việc hay không?
                if (shift.ShiftBreakType == ShiftBreakType.E_FLEXIBLE.ToString() && totalTimeShift > workingStandardHour
                    && shiftItemFlex != null && shiftItemFlex.CoFrom > 0 && shiftItemFlex.CoTo > shiftItemFlex.CoFrom)
                {
                    if (!shift.IsBreakAsWork.HasValue || !shift.IsBreakAsWork.Value)
                    {
                        shift.udCoBreakStart = shiftItemFlex.CoFrom;
                        shift.udCoBreakEnd = shiftItemFlex.CoTo;
                    }

                    realCoBreakStart = shiftItemFlex.CoFrom;
                    realCoBreakEnd = shiftItemFlex.CoTo;
                }
                else if (shift.CoBreakIn > 0 && shift.CoBreakOut > shift.CoBreakIn)
                {
                    if (!shift.IsBreakAsWork.HasValue || !shift.IsBreakAsWork.Value)
                    {
                        shift.udCoBreakStart = shift.CoBreakIn;
                        shift.udCoBreakEnd = shift.CoBreakOut;
                    }

                    realCoBreakStart = shift.CoBreakIn;
                    realCoBreakEnd = shift.CoBreakOut;
                }

                if (realCoBreakEnd > realCoBreakStart)
                {
                    totalTimeShift -= realCoBreakEnd - realCoBreakStart;
                }

                //Thời gian bắt đầu và kết thúc nghỉ giữa ca - dùng cho tính toán
                DateTime timeShiftBreakIn = timeShiftStart.AddHours(shift.udCoBreakStart);
                DateTime timeShiftBreakOut = timeShiftStart.AddHours(shift.udCoBreakEnd);

                //Khoảng thời gian của nửa ca đầu
                DateTime firstHalfShiftStart = timeShiftStart;
                DateTime firstHalfShiftEnd = timeShiftEnd;

                //Khoảng thời gian của nửa ca sau
                DateTime lastHalfShiftStart = timeShiftStart;
                DateTime lastHalfShiftEnd = timeShiftEnd;

                if (shift.udCoBreakEnd > shift.udCoBreakStart
                    && shift.udCoBreakStart > 0)
                {
                    firstHalfShiftStart = timeShiftStart;
                    firstHalfShiftEnd = timeShiftBreakIn;

                    lastHalfShiftStart = timeShiftBreakOut;
                    lastHalfShiftEnd = timeShiftEnd;
                }

                if (timeShiftBreakIn > timeShiftStart && listInOutByWorkDate.Count() > 1)
                {
                    //Lần quẹt thẻ vào đầu tiên và lần quẹt thẻ ra cuối cùng của nửa ca đâu 
                    if (listInOutByWorkDate.Any(d => d.InTime1 < timeShiftBreakIn))
                    {
                        result.FirstInTime = listInOutByWorkDate.Where(d => d.InTime1 < timeShiftBreakIn).OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).FirstOrDefault();
                        result.FirstOutTime = listInOutByWorkDate.Where(d => d.InTime1 < timeShiftBreakIn).OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).LastOrDefault();
                    }
                    else
                    {
                        result.FirstInTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).FirstOrDefault();
                        result.FirstOutTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).FirstOrDefault();
                    }

                    //Lần quẹt thẻ vào đầu tiên và lần quẹt thẻ ra cuối cùng của nửa ca sau 
                    if (listInOutByWorkDate.Any(d => d.OutTime1 > timeShiftBreakOut))
                    {
                        result.LastInTime = listInOutByWorkDate.Where(d => d.OutTime1 > timeShiftBreakOut).OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).FirstOrDefault();
                        result.LastOutTime = listInOutByWorkDate.Where(d => d.OutTime1 > timeShiftBreakOut).OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).LastOrDefault();
                    }
                    else
                    {
                        result.LastInTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).LastOrDefault();
                        result.LastOutTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).LastOrDefault();
                    }
                }
                else
                {
                    //Lần quẹt thẻ vào đầu tiên và lần quẹt thẻ ra cuối cùng của nửa ca đâu 
                    result.FirstInTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).FirstOrDefault();
                    result.FirstOutTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).LastOrDefault();

                    //Lần quẹt thẻ vào đầu tiên và lần quẹt thẻ ra cuối cùng của nửa ca sau 
                    result.LastInTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.InTime1).Select(d => d.InTime1.Value).FirstOrDefault();
                    result.LastOutTime = listInOutByWorkDate.OrderBy(d => d.WorkDate).ThenBy(d => d.OutTime1).Select(d => d.OutTime1.Value).LastOrDefault();
                }

                DateTime nightTimeStart = result.WorkDate.Date.AddHours(21);
                DateTime nightTimeEnd = result.WorkDate.Date.AddDays(1).AddHours(5);

                double nightDuration = 0D;
                double firstDuration = 0D;
                double lastDuration = 0D;

                if (shift.IsNightShift)
                {
                    if (shift.NightTimeStart == null || shift.NightTimeEnd == null)
                    {
                        string appConfigName = AppConfig.HRM_ATT_OT_.ToString();
                        double startHour = 21D;
                        double endHour = 5D;

                        List<object> lstParamSys = new List<object>();
                        lstParamSys.Add(appConfigName);
                        lstParamSys.Add(null);
                        lstParamSys.Add(null);
                        string status = string.Empty;
                        BaseService baseService = new BaseService();
                        var lstAppConfig = baseService.GetData<Sys_AllSetting>(lstParamSys, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);

                        Sys_AllSetting appConfig13 = lstAppConfig.Where(s => s.IsDelete == null && s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM.ToString()).FirstOrDefault();
                        Sys_AllSetting appConfig14 = lstAppConfig.Where(s => s.IsDelete == null && s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTTO.ToString()).FirstOrDefault();
                        //Sys_AppConfig appConfig = EntityService.Instance.GetEntityList<Sys_AppConfig>(false,
                        //    EntityService.Instance.GuidMainContext, Guid.Empty, d => d.Info == appConfigName).FirstOrDefault();

                        if (lstAppConfig != null && appConfig13 != null && appConfig14 != null)
                        {
                            double.TryParse(appConfig13.Value1, out startHour);
                            double.TryParse(appConfig14.Value1, out endHour);
                        }

                        nightTimeStart = shift.NightTimeStart == null ? result.WorkDate.Date.AddHours(startHour) : result.WorkDate.Date.Add(shift.NightTimeStart.Value.TimeOfDay);
                        nightTimeEnd = shift.NightTimeEnd == null ? result.WorkDate.Date.AddHours(endHour) : result.WorkDate.Date.Add(shift.NightTimeEnd.Value.TimeOfDay);
                    }
                    else
                    {
                        nightTimeStart = result.WorkDate.Date.Add(shift.NightTimeStart.Value.TimeOfDay);
                        nightTimeEnd = result.WorkDate.Date.Add(shift.NightTimeEnd.Value.TimeOfDay);
                    }

                    nightTimeEnd = nightTimeStart > nightTimeEnd ? nightTimeEnd.AddDays(1) : nightTimeEnd;
                }

                #endregion

                foreach (Att_Workday objInOut in listInOutByWorkDate)
                {
                    if (objInOut.InTime1.HasValue && objInOut.OutTime1.HasValue)
                    {
                        #region Tính work duration

                        DateTime inTime = objInOut.InTime1.Value;
                        DateTime outTime = objInOut.OutTime1.Value;

                        firstDuration += GetIntersectAmountMinutes(inTime, outTime, firstHalfShiftStart, firstHalfShiftEnd);

                        if (timeShiftBreakIn > timeShiftStart)
                        {
                            //Nếu có giờ nghỉ giữa ca
                            lastDuration += GetIntersectAmountMinutes(inTime, outTime, lastHalfShiftStart, lastHalfShiftEnd);
                        }

                        #endregion

                        #region Tính night shift

                        if (shift.IsNightShift)
                        {
                            if (pregnancyByWorkDay != null)
                            {
                                if ((pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST.ToString()
                                    || pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST_OUT_BEARK.ToString())
                                    && Common.IsOverlap(inTime, outTime, nightTimeStart, nightTimeEnd))
                                {
                                    nightTimeStart = nightTimeStart.AddHours(1);
                                }
                                else if ((pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST.ToString()
                                    || pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST_IN_BEARK.ToString())
                                    && Common.IsOverlap(inTime, outTime, nightTimeStart, nightTimeEnd))
                                {
                                    nightTimeEnd = nightTimeEnd.AddHours(-1);
                                }
                            }

                            //Truong hop nghi giua ca nam trong khoang bat dau ca dem
                            if (nightTimeStart >= timeShiftBreakIn && nightTimeStart <= timeShiftBreakOut)
                            {
                                nightDuration += GetIntersectAmountMinutes(inTime, outTime, timeShiftBreakOut, nightTimeEnd);
                            }
                            else if (nightTimeEnd >= timeShiftBreakIn && nightTimeEnd <= timeShiftBreakOut)
                            {
                                nightDuration += GetIntersectAmountMinutes(inTime, outTime, nightTimeStart, timeShiftBreakIn);
                            }
                            else if (nightTimeEnd > timeShiftBreakOut && nightTimeStart < timeShiftBreakIn)
                            {
                                nightDuration += GetIntersectAmountMinutes(inTime, outTime, nightTimeStart, timeShiftBreakIn);
                                nightDuration += GetIntersectAmountMinutes(inTime, outTime, timeShiftBreakOut, nightTimeEnd);
                            }
                            else
                            {
                                nightDuration += GetIntersectAmountMinutes(inTime, outTime, nightTimeStart, nightTimeEnd);
                            }
                        }

                        #endregion
                    }
                }

                if (shift.ReduceNightShift != null && shift.ReduceNightShift >= 0)
                {
                    Int32 reduceNightMinutes = Convert.ToInt32(shift.ReduceNightShift.Value * 60);
                    nightDuration = nightDuration > reduceNightMinutes ? reduceNightMinutes : nightDuration;
                }

                nightDuration = nightDuration > 0 ? nightDuration / 60 : 0D;
                firstDuration = firstDuration > 0 ? firstDuration / 60 : 0D;
                lastDuration = lastDuration > 0 ? lastDuration / 60 : 0D;

                //Vinhtran: Tổng thời gian làm việc - tính theo giờ
                result.WorkDuration = firstDuration + lastDuration;
                result.NightShiftDuration = nightDuration;
                result.FirstDuration = firstDuration;
                result.LastDuration = lastDuration;

                if (result.WorkDuration > workingStandardHour)
                {
                    result.WorkDuration = workingStandardHour;
                }
                if (result.NightShiftDuration > workingStandardHour)
                {
                    result.NightShiftDuration = workingStandardHour;
                }

                if (shift.IsDoubleShift.Get_Boolean())
                {
                    //Vinh.Tran: Xử lý trường hợp ca ghép
                    totalTimeShift = shift.WorkHours.Get_Double();
                }

                //Vinhtran: Tổng thời gian bị đi trễ hoặc về sớm - tính theo giờ
                result.LateEarlyDuration = totalTimeShift - result.WorkDuration;

                #region Tính trễ sớm

                //Có đi trễ hoặc về sớm
                if (result.LateEarlyDuration > 0)
                {
                    if (timeShiftBreakIn > timeShiftStart)
                    {
                        if (result.FirstInTime > firstHalfShiftStart && result.FirstInTime < firstHalfShiftEnd)
                        {
                            //thời gian đi trễ nửa ca đầu
                            result.FirstLateDuration = result.FirstInTime.Value.Subtract(firstHalfShiftStart).TotalHours;
                        }

                        if (result.LastInTime > lastHalfShiftStart)
                        {
                            //thời gian đi trễ nửa ca sau
                            result.LastLateDuration = result.LastInTime.Value.Subtract(lastHalfShiftStart).TotalHours;
                        }

                        result.FirstEarlyDuration = firstHalfShiftEnd.Subtract(firstHalfShiftStart).TotalHours - firstDuration - result.FirstLateDuration;
                        result.LastEarlyDuration = lastHalfShiftEnd.Subtract(lastHalfShiftStart).TotalHours - lastDuration - result.LastLateDuration;

                        result.LateDuration = result.FirstLateDuration + result.LastLateDuration;
                        result.EarlyDuration = result.FirstEarlyDuration + result.LastEarlyDuration;
                    }
                    else
                    {
                        if (result.FirstInTime > timeShiftStart)
                        {
                            //thời gian đi trễ nửa ca đầu
                            result.LateDuration = result.FirstInTime.Value.Subtract(timeShiftStart).TotalHours;
                            result.FirstLateDuration = result.LateDuration;
                            result.LastLateDuration = 0;
                        }

                        result.EarlyDuration = timeShiftEnd.Subtract(timeShiftStart).TotalHours - result.WorkDuration - result.LateDuration;
                        result.FirstEarlyDuration = result.EarlyDuration;
                        result.LastEarlyDuration = 0;
                    }

                    if (pregnancyByWorkDay != null)
                    {
                        //Thời gian làm việc lớn nhất có thể xảy ra
                        double maxWorkDuration = totalTimeShift - result.LateEarlyDuration + 1;
                        maxWorkDuration += shift.IsDoubleShift.Get_Boolean() ? 1 : 0;//Ca ghép thì thêm 1 giờ
                        maxWorkDuration = maxWorkDuration > totalTimeShift ? totalTimeShift : maxWorkDuration;

                        if (pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST.ToString())
                        {
                            //Chỉ được đi trễ 1 giờ đầu của nửa ca trước, không được về sớm trong nữa ca đầu.
                            result.WorkDuration += result.FirstLateDuration <= 1 ? result.FirstLateDuration : 1;
                            result.FirstLateDuration = result.FirstLateDuration <= 1 ? 0 : result.FirstLateDuration - 1;

                            if (shift.IsDoubleShift.Get_Boolean())
                            {
                                result.WorkDuration += result.LastLateDuration <= 1 ? result.LastLateDuration : 1;
                                result.LastLateDuration = result.LastLateDuration <= 1 ? 0 : result.LastLateDuration - 1;
                                result.LateDuration = result.LateDuration <= 2 ? 0 : result.LateDuration - 2;
                            }
                            else
                            {
                                result.LateDuration = result.LateDuration <= 1 ? 0 : result.LateDuration - 1;
                            }

                            result.WorkDuration = result.WorkDuration > maxWorkDuration ? maxWorkDuration : result.WorkDuration;
                        }
                        else if (pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST_OUT_BEARK.ToString())
                        {
                            //Được đi trễ hoặc về sớm 1 giờ bất kỳ của nửa ca trước, nếu là ca ghép thì được 1 tiếng trễ sớm ở mỗi ca (tổng là 2 tiếng)
                            double firstLatetEarly = timeShiftBreakIn > timeShiftStart ? result.FirstLateDuration + result.FirstEarlyDuration : result.LateDuration + result.EarlyDuration;
                            double lastLatetEarly = (shift.IsDoubleShift.Get_Boolean() && timeShiftBreakIn > timeShiftStart) ? result.LastLateDuration + result.LastEarlyDuration : 0;
                            double totalLatetEarly = firstLatetEarly + lastLatetEarly;

                            if (shift.IsDoubleShift.Get_Boolean())
                            {
                                result.WorkDuration += totalLatetEarly <= 2 ? totalLatetEarly : 2;
                                result.LateDuration = result.LateDuration <= 2 ? 0 : result.LateDuration - 2;
                            }
                            else
                            {
                                result.WorkDuration += totalLatetEarly <= 1 ? totalLatetEarly : 1;
                                result.LateDuration = result.LateDuration <= 1 ? 0 : result.LateDuration - 1;
                            }

                            if (timeShiftBreakIn > timeShiftStart)
                            {
                                result.FirstLateDuration = result.FirstLateDuration <= 1 ? 0 : result.FirstLateDuration - 1;
                                result.FirstEarlyDuration = firstLatetEarly - result.FirstLateDuration;

                                if (shift.IsDoubleShift.Get_Boolean())
                                {
                                    result.LastLateDuration = result.LastLateDuration <= 1 ? 0 : result.LastLateDuration - 1;
                                    result.LastEarlyDuration = lastLatetEarly - result.LastLateDuration;
                                }
                            }

                            result.EarlyDuration = totalLatetEarly - result.LateDuration;
                        }
                        else if (pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST_IN_BEARK.ToString())
                        {
                            //Được đi trễ hoặc về sớm 1 giờ bất kỳ của nửa ca sau, nếu là ca ghép thì được 1 tiếng trễ sớm ở mỗi ca (tổng là 2 tiếng)
                            double lastLatetEarly = timeShiftBreakIn > timeShiftStart ? result.LastLateDuration + result.LastEarlyDuration : result.LateDuration + result.EarlyDuration;
                            double firstLatetEarly = (shift.IsDoubleShift.Get_Boolean() && timeShiftBreakIn > timeShiftStart) ? result.FirstLateDuration + result.FirstEarlyDuration : 0;
                            double totalLatetEarly = firstLatetEarly + lastLatetEarly;

                            if (shift.IsDoubleShift.Get_Boolean())
                            {
                                result.WorkDuration += totalLatetEarly <= 2 ? totalLatetEarly : 2;
                                result.LateDuration = result.LateDuration <= 2 ? 0 : result.LateDuration - 2;
                            }
                            else
                            {
                                result.WorkDuration += totalLatetEarly <= 1 ? totalLatetEarly : 1;
                                result.LateDuration = result.LateDuration <= 1 ? 0 : result.LateDuration - 1;
                            }

                            if (timeShiftBreakIn > timeShiftStart)
                            {
                                result.LastLateDuration = result.LastLateDuration <= 1 ? 0 : result.LastLateDuration - 1;
                                result.LastEarlyDuration = lastLatetEarly - result.LastLateDuration;

                                if (shift.IsDoubleShift.Get_Boolean())
                                {
                                    result.FirstLateDuration = result.FirstLateDuration <= 1 ? 0 : result.FirstLateDuration - 1;
                                    result.FirstEarlyDuration = firstLatetEarly - result.FirstLateDuration;
                                }
                            }

                            result.EarlyDuration = totalLatetEarly - result.LateDuration;
                        }
                        else if (pregnancyByWorkDay.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST.ToString())
                        {
                            //Chỉ được về sớm 1 giờ cuối của nửa ca trước, không được đi trễ trong nữa ca sau.
                            if (timeShiftBreakIn > timeShiftStart)
                            {
                                result.WorkDuration += result.LastEarlyDuration <= 1 ? result.LastEarlyDuration : 1;
                                result.LastEarlyDuration = result.LastEarlyDuration <= 1 ? 0 : result.LastEarlyDuration - 1;

                                if (shift.IsDoubleShift.Get_Boolean())
                                {
                                    result.WorkDuration += result.FirstEarlyDuration <= 1 ? result.FirstEarlyDuration : 1;
                                    result.FirstEarlyDuration = result.FirstEarlyDuration <= 1 ? 0 : result.FirstEarlyDuration - 1;
                                }
                            }
                            else
                            {
                                if (shift.IsDoubleShift.Get_Boolean())
                                {
                                    result.WorkDuration += result.EarlyDuration <= 2 ? result.EarlyDuration : 2;
                                }
                                else
                                {
                                    result.WorkDuration += result.EarlyDuration <= 1 ? result.EarlyDuration : 1;
                                }
                            }

                            if (shift.IsDoubleShift.Get_Boolean())
                            {
                                result.EarlyDuration = result.EarlyDuration <= 2 ? 0 : result.EarlyDuration - 2;
                            }
                            else
                            {
                                result.EarlyDuration = result.EarlyDuration <= 1 ? 0 : result.EarlyDuration - 1;
                            }

                            result.WorkDuration = result.WorkDuration > maxWorkDuration ? maxWorkDuration : result.WorkDuration;
                        }
                    }
                }

                #endregion

                //Vinhtran: Tổng thời gian bị đi trễ hoặc về sớm - tính theo phút
                result.LateDuration = result.LateDuration > 0 ? result.LateDuration * 60 : 0D;
                result.EarlyDuration = result.EarlyDuration > 0 ? result.EarlyDuration * 60 : 0D;
                result.FirstLateDuration = result.FirstLateDuration > 0 ? result.FirstLateDuration * 60 : 0D;
                result.FirstEarlyDuration = result.FirstEarlyDuration > 0 ? result.FirstEarlyDuration * 60 : 0D;
                result.LastLateDuration = result.LastLateDuration > 0 ? result.LastLateDuration * 60 : 0D;
                result.LastEarlyDuration = result.LastEarlyDuration > 0 ? result.LastEarlyDuration * 60 : 0D;
                result.LateEarlyDuration = result.LateEarlyDuration > 0 ? result.LateEarlyDuration * 60 : 0D;

                //Vinhtran: Tổng thời gian (h) làm ca đêm lớn nhất có thể => làm tròn trễ sớm
                result.MaxNightDuration = nightTimeEnd.Subtract(nightTimeStart).TotalHours;

                //Thời gian bắt đầu và kết thúc nghỉ giữa ca - giá trị thực tế
                result.ShiftBreakInTime = timeShiftStart.AddHours(realCoBreakStart);
                result.ShiftBreakOutTime = timeShiftStart.AddHours(realCoBreakEnd);

                //Khoản thời gian làm việc của ca
                result.ShiftInTime = timeShiftStart;
                result.ShiftOutTime = timeShiftEnd;

                if (shift != null && shift.IsNotApplyWorkHoursReal != null && shift.IsNotApplyWorkHoursReal.Value == true)
                {
                    result.WorkDuration = workingStandardHour - Math.Abs(result.LateDuration / 60) - Math.Abs(result.EarlyDuration / 60);
                }
            }

            return result;
        }

        /// <summary>
        /// Lay mien giao giua hai khoang thoi gian
        /// </summary>
        /// <param name="In1"></param>
        /// <param name="Out1"></param>
        /// <param name="In2"></param>
        /// <param name="Out2"></param>
        /// <returns>don vi giao nhau la phut</returns>
        public static Int32 GetIntersectAmountMinutes(DateTime In1, DateTime Out1, DateTime In2, DateTime Out2)
        {
            Double result = 0;

            DateTime From = In2.CompareTo(In1) > 0 ? In2 : In1;
            DateTime To = Out2.CompareTo(Out1) < 0 ? Out2 : Out1;

            From = From.AddSeconds(-From.Second);
            To = To.AddSeconds(-To.Second);

            result = To.Subtract(From).TotalMinutes;
            result = result > 0 ? result : 0;

            return (Int32)Math.Round(result, 0);
        }


        
    }
}
