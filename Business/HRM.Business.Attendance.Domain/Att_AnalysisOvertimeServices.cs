using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using System;
using HRM.Business.Category.Models;
using System.Collections.Generic;
using System.Reflection;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Infrastructure.Utilities.Helpers;
using System.Collections;
using HRM.Business.BaseModel;
using HRM.Business.Main.Domain;
using HRM.Business.HrmSystem.Models;

using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Business.Hr.Models;
using VnResource.Helper.Linq;
using System.Linq.Expressions;
using System.Globalization;


namespace HRM.Business.Attendance.Domain
{
    public class Att_AnalysisOvertimeServices : BaseService
    {
        #region Hàm Phân Tích Khi Tạo Mới Tăng Ca

        public List<Att_OvertimeEntity> LoadData(Att_OvertimeEntity overtime, string ProfileIds, bool ByShiftProfile,string UserLogin)
        {

            List<Att_OvertimeEntity> listOvertimeInsert = new List<Att_OvertimeEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoAtt_Pregnancy = new CustomBaseRepository<Att_Pregnancy>(unitOfWork);

                List<Att_Pregnancy> _LstPregnancy = new List<Att_Pregnancy>();
                Att_OvertimeServices overtimeDAO = new Att_OvertimeServices();
                string status = string.Empty;

                string proStr = Common.DotNetToOracle(ProfileIds);
                var lstProfileDetails = GetData<Hre_ProfileEntity>(proStr, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status);
                List<Guid> listProfileId = lstProfileDetails.Select(s => s.ID).ToList();

                string key = "HRM_ATT_OT";
                List<object> lstSysOT = new List<object>();
                lstSysOT.Add(key);
                lstSysOT.Add(null);
                lstSysOT.Add(null);
                var config = GetData<Sys_AllSettingEntity>(lstSysOT, ConstantSql.hrm_sys_sp_get_AllSetting,UserLogin, ref status);
                if (config == null)
                    return listOvertimeInsert;

                var OTThanTwoHour = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_ISALLOWADDHOURWHENOTTHANTWOHOUR.ToString()).FirstOrDefault();
                var OTBreakTime = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_DONOTSPLITOTBREAKTIME.ToString()).FirstOrDefault();
                var inmaternityregime = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME.ToString()).FirstOrDefault();


                List<Cat_DayOff> lstDayOff = repoCat_DayOff.FindBy(s => s.IsDelete == null).ToList();
                if (OTThanTwoHour.Value1 == bool.TrueString)
                    lstDayOff = lstDayOff.Where(dayoff => dayoff.Type == HolidayType.E_HOLIDAY_HLD.ToString()).ToList();

                bool isAllowCutOTBreakHour = false;
                if (OTBreakTime.Value1 == bool.TrueString)
                    isAllowCutOTBreakHour = true;

                Att_OvertimeEntity baseOT = null;

                //Trung.Le 20120621 #0014337 Nếu như CÓ THÊM đăng ký Leave loại nghỉ lễ (Mã: HLD) thì ngày đó tương đương với ngày nghỉ lễ
                string LeavedayTypeCode_HLD = LeavedayTypeCode.HLD.ToString();
                string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
                DateTime DateFromOvertime = overtime.WorkDate.Date;
                DateTime DateEndOvertime = overtime.WorkDate.Add(TimeSpan.FromHours(overtime.RegisterHours)).Date;
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                List<Att_LeaveDay> lstLeaveDayHoliday = repoAtt_LeaveDay
                        .FindBy(att => att.IsDelete == null && DateEndOvertime >= att.DateStart && DateFromOvertime <= att.DateEnd
                            && att.Status == E_APPROVED && att.Cat_LeaveDayType.Code == LeavedayTypeCode_HLD
                            && listProfileId.Contains(att.ProfileID))
                        .ToList();


                if (overtime.ID == Guid.Empty)
                {
                    //baseOT = GetBaseDataOvertime(baseOT, overtime, profile);
                    string _pregnancyType = PregnancyType.E_LEAVE_EARLY.ToString();
                    _LstPregnancy = repoAtt_Pregnancy
                        .FindBy(prg => prg.Type == _pregnancyType && prg.DateEnd >= overtime.WorkDate.Date && prg.DateStart <= overtime.WorkDate)
                        .ToList();

                    Hre_Profile _hreProfile = new Hre_Profile();
                    foreach (var profile in lstProfileDetails)
                    {
                        overtime.ProfileID = profile.ID;
                        overtime.ProfileName = profile.ProfileName;
                        overtime.CodeEmp = profile.CodeEmp;

                        listOvertimeInsert.AddRange(AnalysisOvertime(overtime,
                        GetListDayOffPerProfile(lstLeaveDayHoliday, profile, lstDayOff, E_HOLIDAY_HLD) //lstDayOff
                        , _LstPregnancy, ByShiftProfile, isAllowCutOTBreakHour, UserLogin));
                    }


                }
                #region overtime.ID == Guid.Empty && strListId.Length > 1
                //if (overtime.ID == Guid.Empty && strListId.Length > 1)
                //{
                //    List<Hre_Profile> listAllProfile = EntityService.GetEntityList<Hre_Profile>(GuidContext, LoginUserID.Value, pf => listProfileId.Contains(pf.ID));
                //    foreach (Guid _pfID in listProfileId)
                //    {
                //        if (_pfID != pfid)
                //        {
                //            baseOT = GetBaseDataOvertime(baseOT, overtime, profile);

                //            List<Hre_Profile> _listPfTemp = listAllProfile.Where(pf => pf.ID == _pfID).ToList();
                //            if (_listPfTemp.Count != 1)
                //            {
                //                continue;
                //            }
                //            profile = _listPfTemp[0];
                //            baseOT.Hre_Profile = profile;
                //            listOvertimeInsert.AddRange(overtimeDAO.AnalysisOvertime(baseOT,
                //                GetListDayOffPerProfile(lstLeaveDayHoliday, profile, lstDayOff, E_HOLIDAY_HLD) //lstDayOff
                //                , _LstPregnancy, GuidContext, LoginUserID.Value, rdbByShiftProfile, isAllowCutOTBreakHour));
                //        }
                //    }
                //}
                #endregion
                #region tan.do danh dau nguoi huong che do thai san
                //_listbaseData = new List<BaseDataOvertime>();
                if (inmaternityregime.Value1 != bool.TrueString)
                {
                    DateTime time = overtime.WorkDate;
                    string type = PregnancyStatus.E_LEAVE_EARLY.ToString();
                    List<Guid> guids = listOvertimeInsert.Select(s => s.ProfileID).ToList();
                    var pregnancies = repoAtt_Pregnancy
                        .FindBy(s => s.IsDelete == null && s.DateStart <= time && time <= s.DateEnd && s.Type == type
                        && guids.Contains(s.ProfileID))
                        .ToList();
                    foreach (var baseDataOvertime in listOvertimeInsert)
                    {
                        foreach (var attPregnancy in pregnancies)
                        {
                            if (baseDataOvertime.ProfileID == attPregnancy.ProfileID)
                            {
                                //_listbaseData.Add(baseDataOvertime);
                            }
                        }
                    }
                }
                #endregion
                return listOvertimeInsert;
            }
        }
        /// <summary>
        /// Edit comment Trung.le 20120529
        /// Them thuoc tinh Khong cat Overtime qua ngày E_STANDARD_WORKDAY - Value37 Trung.le 20120529
        /// </summary>
        /// <param name="overtime"></param>
        /// <param name="lstDayOff"></param>
        /// <param name="_LstPregnancy"></param>
        /// <param name="GuidContext"></param>
        /// <param name="_userId"></param>
        /// <param name="isByShift">Lấy theo ca làm việc của từng người</param>
        /// <returns></returns>
        public List<Att_OvertimeEntity> AnalysisOvertime(Att_OvertimeEntity overtime, List<Cat_DayOff> lstDayOff, 
            List<Att_Pregnancy> _LstPregnancy, bool isByShift, bool isAllowCutBreakHour,string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);

                Cat_Shift ShiftOfOT = repoCat_Shift.FindBy(s => s.ID == overtime.ShiftID).FirstOrDefault();


                string status = string.Empty;

                Att_OvertimeEntity baseOT = null;
                if (overtime != null)
                {
                    //overtime.SerialCode = overtime.Workdate.ToString("ddMMyyyy");
                }

                List<Att_OvertimeEntity> listOvertimeInsert = new List<Att_OvertimeEntity>();
                Hre_Profile profile = new Hre_Profile();
                profile.ID = overtime.ProfileID;
                DateTime _workDate = overtime.WorkDate;
                DateTime dateWorkDate = _workDate;

                string key = "HRM_ATT_OT";
                List<object> lstSysOT = new List<object>();
                lstSysOT.Add(key);
                lstSysOT.Add(null);
                lstSysOT.Add(null);
                var config = GetData<Sys_AllSettingEntity>(lstSysOT, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);
                if (config == null)
                    return listOvertimeInsert;
                var NoCutOvertimePassDay = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NOCUTOVERTIMEPASSDAY.ToString()).FirstOrDefault();
                var ByPeriodOfTime = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_BYPERIODOFTIME.ToString()).FirstOrDefault();
                var nightShiftFrom = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM.ToString()).FirstOrDefault();
                var nightShiftTo = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTTO.ToString()).FirstOrDefault();
                bool isNocutOvertimePassDay = Convert.ToBoolean(NoCutOvertimePassDay.Value1);//Không cắt Overtime qua ngày


                List<Hre_Profile> lstProfile = new List<Hre_Profile>() { profile };
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).ToList();
                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                Att_RosterServices.GetRosterGroup(lstProfileIDs, _workDate.Date, _workDate.Date, out lstRosterTypeGroup, out lstRosterGroup);

                string registryCode = "OT_" + overtime.CodeEmp + "_" + overtime.WorkDate.ToString("ddMMyyyyHHmmss");
                double basicHours = overtime.RegisterHours;
                double durationHours = overtime.RegisterHours;
                //overtime.BasicHours = basicHours;
                //overtime.RegisterCode = registryCode;
                bool isWorkDay = true;
                DateTime dateWorkDateEnd = dateWorkDate.AddHours(durationHours);
                Att_Grade grade = Att_GradeServices.GetGrade(profile, _workDate.Date);
                Cat_GradeAttendance gradeCfg = grade == null ? null : grade.Cat_GradeAttendance;
                if (gradeCfg == null)
                {
                    return listOvertimeInsert;
                }
                Hashtable htable = null;
                htable = Att_RosterServices.GetRosterTable(false, profile, _workDate.Date, _workDate.Date, lstRosterGroup, lstRosterTypeGroup);
                isWorkDay = Att_AttendanceServices.IsWorkDay(gradeCfg, htable, lstDayOff, _workDate.Date);
                if (isByShift)//Lấy theo ca làm việc của từng người
                {
                    #region Lấy theo ca làm việc của từng người

                    Cat_Shift ship = Att_AttendanceServices.GetShift(gradeCfg, htable, _workDate.Date);
                    if (ship != null)
                    {
                        if (isWorkDay)
                        {
                            if (overtime.DurationType == EnumDropDown.OvertimeDurationType.E_OT_LATE.ToString() && ship != null)
                            {
                                DateTime timeOut = ship.InTime.AddHours(ship.CoOut);
                                dateWorkDate = _workDate.Date.AddHours(timeOut.Hour).AddMinutes(timeOut.Minute);
                                dateWorkDateEnd = dateWorkDate.AddHours(durationHours);
                            }
                            else if (overtime.DurationType == EnumDropDown.OvertimeDurationType.E_OT_EARLY.ToString() && ship != null)
                            {
                                DateTime timeIn = ship.InTime;
                                dateWorkDate = _workDate.Date.AddHours(timeIn.Hour).AddMinutes(timeIn.Minute);
                                dateWorkDateEnd = dateWorkDate;
                                dateWorkDate = dateWorkDate.AddHours(-durationHours);
                            }
                        }
                        else
                        {
                            DateTime timeIn = ship.InTime;
                            dateWorkDate = _workDate.Date.AddHours(timeIn.Hour).AddMinutes(timeIn.Minute);
                        }
                    }
                    else
                    {
                        dateWorkDate = dateWorkDate.Date;
                        dateWorkDateEnd = dateWorkDate.AddHours(durationHours);
                    }
                    ShiftOfOT = ship;


                    htable = Att_RosterServices.GetRosterTable(false, profile, dateWorkDate, dateWorkDate.AddHours(durationHours), lstRosterGroup, lstRosterTypeGroup);
                    isWorkDay = Att_AttendanceServices.IsWorkDay(gradeCfg, htable, lstDayOff, dateWorkDate.AddHours(durationHours));

                    #endregion
                }

                //Kiem tra xem co trong thoi gian nghi thai san khong?
                if (isWorkDay && overtime.DurationType == EnumDropDown.OvertimeDurationType.E_OT_LATE.ToString()
                    && _LstPregnancy != null && _LstPregnancy.Exists(pc => pc.ProfileID == profile.ID && pc.DateEnd >= dateWorkDate && pc.DateStart <= dateWorkDateEnd))
                {
                    dateWorkDate = dateWorkDate.AddHours(-1);
                    dateWorkDateEnd = dateWorkDate.AddHours(durationHours);
                }
                overtime.WorkDate = dateWorkDate;



                string strHoursNightFrom = string.Empty;
                string strHoursNightTo = string.Empty;


                if (!Att_AttendanceServices.IsNightShiftByConfig(ByPeriodOfTime) && ShiftOfOT != null
                    && ShiftOfOT.NightTimeStart != null
                    && ShiftOfOT.NightTimeEnd != null)
                {
                    strHoursNightFrom = ShiftOfOT.NightTimeStart.Value.ToString("HH:mm:ss");
                    strHoursNightTo = ShiftOfOT.NightTimeEnd.Value.ToString("HH:mm:ss");
                }
                else
                {
                    strHoursNightFrom = string.IsNullOrEmpty(nightShiftFrom.Value1) == true ? "21:00:00" : nightShiftFrom.Value1 + ":00";
                    strHoursNightTo = string.IsNullOrEmpty(nightShiftTo.Value1) == true ? "05:00:00" : nightShiftTo.Value1 + ":00";
                }

                DateTime dateNightFrom = Common.ConvertStringToDateTime(dateWorkDate.Date.ToString("MM/dd/yyyy") + " " + strHoursNightFrom, CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
                DateTime dateNightTo = Common.ConvertStringToDateTime(dateWorkDate.Date.AddDays(1).ToString("MM/dd/yyyy") + " " + strHoursNightTo, CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);


                OvertimeInfo overtimeInfo = new OvertimeInfo(true);
                overtimeInfo.DateFrom = dateWorkDate;
                overtimeInfo.TotalHours = overtime.RegisterHours;

                overtimeInfo.DayShiftPoints = new DateTime[] { dateNightTo };

                if (isNocutOvertimePassDay)
                {
                    overtimeInfo.NightShiftPoints = new DateTime[] { dateNightFrom };
                }
                else
                {
                    //truong hop cat khi qua ngay hom sau
                    overtimeInfo.NightShiftPoints = new DateTime[] { dateNightFrom, dateNightFrom.Date };
                }

                if (ShiftOfOT != null && isAllowCutBreakHour)
                {
                    Cat_Shift shift = ShiftOfOT;
                    DateTime coBreakOut = shift.InTime.AddHours(shift.CoBreakIn);

                    if (shift.CoBreakOut - shift.CoBreakIn > 0)
                    {
                        overtimeInfo.BreaktPoints.Add(coBreakOut, shift.CoBreakOut - shift.CoBreakIn);
                    }
                }
                Hre_Profile temp = new Hre_Profile();
                temp.ID = overtime.ProfileID;

                overtimeInfo.Hre_Profile = temp;
                overtimeInfo.ListDayOff = lstDayOff;

                listOvertimeInsert = AnalysisOvertime(overtime, overtimeInfo);



                return listOvertimeInsert;
            }
        }

        /// <summary>
        /// Tách thời gian làm việc thành các khoản theo cấu hình.
        /// </summary>
        /// <param name="guidContext"></param>
        /// <param name="userID"></param>
        /// <param name="parameter">Điền đủ thông tin cho tất cả thuộc tính trong OvertimeInfo</param>
        /// <returns></returns>
        public List<Att_OvertimeEntity> AnalysisOvertime(Att_OvertimeEntity BaseDataOvertimeTemplate, OvertimeInfo parameter)
        {
            List<TimeSpan> listTimePoints = new List<TimeSpan>();

            if (parameter.DayShiftPoints != null)
            {
                listTimePoints.AddRange(parameter.DayShiftPoints.Select(d => d.TimeOfDay).ToList());
            }
            if (parameter.NightShiftPoints != null)
            {
                listTimePoints.AddRange(parameter.NightShiftPoints.Select(d => d.TimeOfDay).ToList());
            }

            if (parameter.BreaktPoints != null)
            {
                foreach (var item in parameter.BreaktPoints)
                {
                    TimeSpan breakStart = item.Key.TimeOfDay;
                    TimeSpan breakEnd = item.Key.AddHours(item.Value).TimeOfDay;

                    listTimePoints.Add(breakStart);
                    listTimePoints.Add(breakEnd);
                }
            }

            listTimePoints = listTimePoints.Distinct().ToList();
            List<Att_OvertimeEntity> listOvertimeInsert = new List<Att_OvertimeEntity>();
            List<Guid> lstProfileIDs = listOvertimeInsert.Select(m => m.ProfileID).ToList();
            List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
            List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
            Att_RosterServices.GetRosterGroup(lstProfileIDs, parameter.DateFrom, parameter.DateTo, out lstRosterTypeGroup, out lstRosterGroup);

            DateTime currentPoint = parameter.DateFrom;

            while (currentPoint < parameter.DateTo)
            {
                TimeSpan greaterTime = listTimePoints.Min();
                Att_OvertimeEntity baseDataOvertime = new Att_OvertimeEntity();
                //string[] strExclude = new string[] { BaseDataOvertime.FieldNames.ID, BaseDataOvertime.FieldNames.Can_Food, BaseDataOvertime.FieldNames.Can_Food2, BaseDataOvertime.FieldNames.Can_Menu, BaseDataOvertime.FieldNames.Can_Menu1 };
                //BaseDataOvertimeTemplate.CopyTo(baseDataOvertime, strExclude);
                baseDataOvertime.ID = Guid.NewGuid();
                baseDataOvertime.WorkDate = currentPoint;

                if (listTimePoints.Any(d => d > currentPoint.TimeOfDay))
                {
                    greaterTime = listTimePoints.Where(d => d > currentPoint.TimeOfDay).OrderBy(d => d).FirstOrDefault();
                    currentPoint = currentPoint.Date.Add(greaterTime);
                }
                else
                {
                    currentPoint = currentPoint.Date.AddDays(1).Add(greaterTime);
                }

                currentPoint = currentPoint > parameter.DateTo ? parameter.DateTo : currentPoint;
                baseDataOvertime.RegisterHours = currentPoint.Subtract(baseDataOvertime.WorkDate).TotalHours;

                baseDataOvertime.WorkDateEnd = currentPoint;//thới gian kết thúc ca
                baseDataOvertime.IsNightShift = IsNightShift(baseDataOvertime, parameter);
                baseDataOvertime.ProfileID = parameter.Hre_Profile.ID;
                if (!IsBreakTime(baseDataOvertime, parameter))
                {
                    if (parameter.Hre_Profile != null && parameter.ListDayOff != null)
                    {
                        baseDataOvertime.OvertimeTypeID = getOTType(baseDataOvertime.WorkDate, baseDataOvertime.IsNightShift,
                            parameter.Hre_Profile, parameter.ListDayOff, lstRosterGroup, lstRosterTypeGroup).ID;
                    }

                    listOvertimeInsert.Add(baseDataOvertime);
                }
            }

            return listOvertimeInsert;
        }

        List<Cat_DayOff> GetListDayOffPerProfile(List<Att_LeaveDay> lstLeaveDayHoliday, Hre_ProfileEntity profile, List<Cat_DayOff> lstDayOff, string E_HOLIDAY_HLD)
        {
            //string strCat_DayOff = CachObjects.ListCat_DayOff;
            //if (Cache[strCat_DayOff] == null)
            //    SecurityService.CacheBaseCat_Holiday();

            List<Att_LeaveDay> lstLeaveDayHolidayTemp = lstLeaveDayHoliday.Where(att => att.ProfileID == profile.ID).ToList();
            List<Cat_DayOff> lstDayOffPerProfile = new List<Cat_DayOff>();

            // lstDayOffPerProfile = SecurityService.CacheBaseCat_Holiday();

            #region Clone danh sach ngay nghi

            foreach (Cat_DayOff dayoff in lstDayOff)
            {
                lstDayOffPerProfile.Add(new Cat_DayOff()
                {
                    ID = dayoff.ID,
                    DateOff = dayoff.DateOff,
                    Type = dayoff.Type//Ngày nghỉ ngày thành ngày nghỉ Holiday
                });
            }

            #endregion

            if (lstLeaveDayHolidayTemp.Count() > 0)
            {
                foreach (Att_LeaveDay att in lstLeaveDayHolidayTemp)
                {
                    for (DateTime idx = att.DateStart.Date; idx <= att.DateEnd.Date; idx = idx.AddDays(1))
                    {
                        List<Cat_DayOff> lstDayofftemp = lstDayOffPerProfile.Where(da => att.DateStart.Date >= da.DateOff.Date
                            && att.DateEnd.Date <= da.DateOff.Date).ToList();
                        if (lstDayofftemp.Count() == 0)//Danh sách ngày nghỉ mỗi người chưa có ngày này --Trung.Le
                            lstDayOffPerProfile.Add(new Cat_DayOff()
                            {
                                DateOff = idx,
                                Type = E_HOLIDAY_HLD//Ngày nghỉ ngày thành ngày nghỉ Holiday
                            });
                        else//Danh sách ngày nghỉ mỗi người đã có ngày này --Trung.Le
                            lstDayofftemp[0].Type = E_HOLIDAY_HLD;//Chỉnh ngày nghỉ ngày thành ngày nghỉ Holiday
                    }
                }
            }

            return lstDayOffPerProfile;
        }


        private bool IsNightShift(Att_OvertimeEntity baseDataOvertime, OvertimeInfo parameter)
        {
            bool result = false;

            if (parameter.DayShiftPoints != null && parameter.NightShiftPoints != null &&
                parameter.DayShiftPoints.Count() > 0 && parameter.NightShiftPoints.Count() > 0)
            {
                //Tính ca ngày nhỏ nhất và ca ngày lớn nhất theo cấu hình Day/Night
                TimeSpan minDayShift = parameter.DayShiftPoints.Min(d => d.TimeOfDay);
                TimeSpan maxDayShift = DateTime.Now.Date.TimeOfDay;

                if (parameter.NightShiftPoints.Any(d => d.TimeOfDay > minDayShift))
                {
                    maxDayShift = parameter.NightShiftPoints.Where(d =>
                        d.TimeOfDay > minDayShift).Min(d => d.TimeOfDay);
                }
                else
                {
                    maxDayShift = parameter.NightShiftPoints.Min(d => d.TimeOfDay);
                    maxDayShift = maxDayShift.Add(new TimeSpan(1, 0, 0, 0));//cộng thêm 1 ngày
                }

                DateTime dayFrom = DateTime.Now.Date.Add(minDayShift);
                DateTime dayTo = DateTime.Now.Date.Add(maxDayShift);

                DateTime workDate = DateTime.Now.Date.Add(baseDataOvertime.WorkDate.TimeOfDay);
                DateTime endWorkDate = workDate.AddHours(baseDataOvertime.RegisterHours);
                result = !(workDate < dayTo && endWorkDate > dayFrom);
            }
            else if (parameter.DayShiftPoints != null && parameter.DayShiftPoints.Count() > 0)
            {
                //Tính ca ngày nhỏ nhất và ca ngày lớn nhất theo cấu hình Day/Night
                TimeSpan minDayShift = parameter.DayShiftPoints.Min(d => d.TimeOfDay);
                TimeSpan maxDayShift = parameter.DayShiftPoints.Max(d => d.TimeOfDay);

                DateTime dayFrom = DateTime.Now.Date.Add(minDayShift);
                DateTime dayTo = DateTime.Now.Date.Add(maxDayShift);

                DateTime workDate = DateTime.Now.Date.Add(baseDataOvertime.WorkDate.TimeOfDay);
                DateTime endWorkDate = workDate.AddHours(baseDataOvertime.RegisterHours);
                result = !(workDate < dayTo && endWorkDate > dayFrom);
            }
            else if (parameter.NightShiftPoints != null && parameter.NightShiftPoints.Count() > 0)
            {
                TimeSpan minNightShift = DateTime.Now.Date.TimeOfDay;
                TimeSpan maxNightShift = DateTime.Now.Date.TimeOfDay;

                //Tính ca ngày nhỏ nhất và ca ngày lớn nhất theo cấu hình Day/Night
                if (parameter.NightShiftPoints.Any(d => d.TimeOfDay > DateTime.Now.Date.TimeOfDay
                    && d.TimeOfDay < DateTime.Now.Date.AddHours(12).TimeOfDay))
                {
                    maxNightShift = parameter.NightShiftPoints.Where(d => d.TimeOfDay > DateTime.Now.Date.TimeOfDay
                        && d.TimeOfDay < DateTime.Now.Date.AddHours(12).TimeOfDay).Max(d => d.TimeOfDay);

                    if (parameter.NightShiftPoints.Any(d => d.TimeOfDay > maxNightShift))
                    {
                        minNightShift = parameter.NightShiftPoints.Where(d => d.TimeOfDay > maxNightShift).Min(d => d.TimeOfDay);
                        maxNightShift = maxNightShift.Add(new TimeSpan(1, 0, 0, 0));//cộng thêm 1 ngày
                    }
                    else
                    {
                        minNightShift = parameter.NightShiftPoints.Min(d => d.TimeOfDay);
                    }
                }
                else
                {
                    minNightShift = parameter.NightShiftPoints.Min(d => d.TimeOfDay);
                    maxNightShift = parameter.NightShiftPoints.Max(d => d.TimeOfDay);
                }

                DateTime nightFrom = DateTime.Now.Date.Add(minNightShift);
                DateTime nightTo = DateTime.Now.Date.Add(maxNightShift);

                DateTime workDate = DateTime.Now.Date.Add(baseDataOvertime.WorkDate.TimeOfDay);
                DateTime endWorkDate = workDate.AddHours(baseDataOvertime.RegisterHours);
                result = workDate < nightTo && endWorkDate > nightFrom;
            }

            return result;
        }

        private bool IsBreakTime(Att_OvertimeEntity baseDataOvertime, OvertimeInfo parameter)
        {
            return parameter.BreaktPoints.Where(d => d.Value > 0).Select(d => d.Key.TimeOfDay).Distinct().Any(d =>
                baseDataOvertime.WorkDate != null && d == baseDataOvertime.WorkDate.TimeOfDay);
        }

        public Cat_OvertimeType getOTType(DateTime dateWorkDate, bool isNightShift, Hre_Profile profile, List<Cat_DayOff> list_dayOff, List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);

                Cat_OvertimeType otType = null;
                try
                {
                    dateWorkDate = dateWorkDate.Date;

                    //LamLe : Them chuc nang neu tang ca vao ngay nghi thi chon loai tang ca phu thuoc vao cau hinh Tang ca trong ngay nghi.
                    List<Att_LeaveDay> lstLeave = repoAtt_LeaveDay
                        .FindBy(lv => lv.IsDelete == null && lv.DateStart <= dateWorkDate && lv.DateEnd >= dateWorkDate && lv.ProfileID == profile.ID)
                        .ToList();
                    if (lstLeave.Count > 0)
                    {
                        Att_LeaveDay leave = lstLeave[0];
                        if (leave != null && leave.Cat_LeaveDayType != null && leave.Cat_LeaveDayType.Cat_OvertimeType != null)
                        {
                            otType = leave.Cat_LeaveDayType.Cat_OvertimeType;
                            return otType;
                        }
                    }

                    Att_Grade grade = Att_GradeServices.GetGrade(profile, dateWorkDate);
                    if (grade == null)
                    {
                        string status = OverTimeType.E_WORKDAY.ToString();
                        otType = repoCat_OvertimeType.FindBy(dayoff => dayoff.IsDelete == null && dayoff.Code == status).FirstOrDefault();
                        return otType;
                    }
                    list_dayOff = list_dayOff.Where(df => df.DateOff.Date == dateWorkDate.Date).ToList();
                    Cat_GradeCfg gradecfg = grade.Cat_GradeCfg;

                    //Check overtime holiday
                    //List<Cat_DayOff> list_dayOff = EntityService.Instance.GetEntityList<Cat_DayOff>(GuidContext, _userId, dayoff => dayoff.DateOff == dateWorkDate);

                    if (list_dayOff.Count > 0)
                    {
                        //Cat_GradeCfg.FieldNames.Cat_OvertimeType1
                        bool isDayOffHollyDay = false;
                        foreach (var item in list_dayOff)
                        {
                            if (item.DateOff.Date == dateWorkDate.Date && item.Type == HolidayType.E_HOLIDAY_HLD.ToString())
                                isDayOffHollyDay = true;
                        }
                        //Ca dem ngay le
                        if (isDayOffHollyDay && isNightShift)
                            otType = gradecfg.Cat_OvertimeType5;
                        //Ca dem ngay nghi cuoi tuan
                        else if (!isDayOffHollyDay && isNightShift)
                            otType = gradecfg.Cat_OvertimeType4;
                        ////Ca ngay ngay le
                        else if (isDayOffHollyDay && !isNightShift)
                            otType = gradecfg.Cat_OvertimeType2;
                        ////Ca ngay ngay nghi cuoi tuan
                        else
                            otType = gradecfg.Cat_OvertimeType1;
                    }
                    else
                    {
                        Hashtable hsRoster = Att_RosterServices.GetRosterTable(false, profile, dateWorkDate, dateWorkDate, lstRosterGroup, lstRosterTypeGroup);

                        bool isWorkday = Att_AttendanceServices.IsWorkDay(grade.Cat_GradeAttendance, hsRoster, list_dayOff, dateWorkDate);
                        if (isWorkday)
                        {
                            if (isNightShift)
                            {
                                otType = gradecfg.Cat_OvertimeType3;
                            }
                            else
                            {
                                otType = gradecfg.Cat_OvertimeType;
                            }
                            //if (gradecfg.Cat_OvertimeType == null)
                            //{

                            //    string workday = OverTimeType.E_WORKDAY.ToString();
                            //    otType = EntityService.Instance.GetEntity<Cat_OvertimeType>(GuidContext, _userId, dayoff => dayoff.Code == workday);
                            //}
                            //else
                            //    otType = gradecfg.Cat_OvertimeType;


                        }
                        else
                        {
                            if (isNightShift)
                            {
                                otType = gradecfg.Cat_OvertimeType4;
                            }
                            else
                            {
                                otType = gradecfg.Cat_OvertimeType1;
                            }
                            //if (gradecfg.Cat_OvertimeType1 == null)
                            //{
                            //    string workday = OverTimeType.E_WORKDAY.ToString();
                            //    otType = EntityService.Instance.GetEntity<Cat_OvertimeType>(GuidContext, _userId, dayoff => dayoff.Code == workday);
                            //}
                            //else
                            //    otType = gradecfg.Cat_OvertimeType1;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return otType;
            }
        }



        #endregion
    }
}
