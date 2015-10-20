using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using System;
using HRM.Data.Entity;
using HRM.Business.Attendance.Domain;
using VnResource.Helper.Linq;
using HRM.Data.Attendance.Data.Sql.Repositories;


namespace HRM.Business.Hr.Domain
{
    public class Att_CompensationDetailServices : BaseService
    {
        #region Phép Bù Comprehension
        public void AnalyzeCompensation(int Year, List<Guid> lstProfileIDTotal)
        {
            //Lấy cấu hình
            //lấy ngày nghỉ (Bù)
            //Lấy tăng ca (Bù)
            //Lấy Ca Làm Việc (Bù)
            //Hệ Số Tăng Ca (Loại Tăng Ca)
            foreach (var lstProfileID in lstProfileIDTotal.Chunk(500))
            {
                using (var context = new VnrHrmDataContext())
                {
                    #region Khai báo
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                    var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                    var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                    var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                    var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                    var repoAtt_Overtime = new Att_OvertimeRepository(unitOfWork);
                    var repoAtt_CompensationConfig = new Att_CompensationConfigRepository(unitOfWork);
                    var repoAtt_CompensationDetail = new Att_CompensationDetailRepository(unitOfWork);


                    #endregion
                    #region Lấy Dữ Liệu
                    //   var profileByCodeEmp = repoHre_Profile.FindBy(p => p.IsDelete == null && p.CodeEmp == codeEmp).FirstOrDefault();

                    var lstProfile = repoHre_Profile.FindBy(m => lstProfileID.Contains(m.ID)).Select(m => new { m.ID, m.DateHire }).ToList();
                    var lstCompensationConfig = repoAtt_CompensationConfig
                        .FindBy(m => m.Year == Year && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value))
                        .Select(m => new CompensationConfigModel() { ID = m.ID, ProfileID = m.ProfileID, Year = m.Year, InitAvailable = m.InitAvailable, MonthBeginInYear = m.MonthBeginInYear, MonthResetInitAvailable = m.MonthResetInitAvailable, MonthStartProfile = m.MonthStartProfile })
                        .ToList();
                    int MonthBeginYear = 1;
                    if (lstCompensationConfig != null && lstCompensationConfig.Count>0)
                    {
                        MonthBeginYear = lstCompensationConfig.FirstOrDefault().MonthBeginInYear ?? 1;
                    }
                    DateTime BeginYear = new DateTime(Year, MonthBeginYear, 1);
                    DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                    string E_APPROVED_Leave = LeaveDayStatus.E_APPROVED.ToString();
                    var lstLeaveDayCompensationTypeID = repoCat_LeaveDayType
                        .FindBy(m => m.IsTimeOffInLieu == true)
                        .Select(m => m.ID)
                        .ToList();
                    var lstLeaveday = repoAtt_Leaveday
                       .FindBy(m => m.Status == E_APPROVED_Leave && m.DateStart <= EndYear && m.DateEnd >= BeginYear && lstLeaveDayCompensationTypeID.Contains(m.LeaveDayTypeID) && lstProfileID.Contains(m.ProfileID))
                       .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.LeaveHours, m.DurationType })
                       .ToList();

                    string E_TIMEOFF = MethodOption.E_TIMEOFF.ToString();
                    string E_APPROVED_Overtime = OverTimeStatus.E_APPROVED.ToString();
                    string E_CONFIRM_Overtime = OverTimeStatus.E_CONFIRM.ToString();
                    var lstOvertime = repoAtt_Overtime
                        .FindBy(m => m.WorkDateRoot >= BeginYear && m.WorkDateRoot <= EndYear && m.MethodPayment == E_TIMEOFF && (m.Status == E_APPROVED_Overtime || m.Status == E_CONFIRM_Overtime) && lstProfileID.Contains(m.ProfileID))
                        .Select(m => new { m.ID, m.ProfileID, m.WorkDateRoot, m.ApproveHours, m.ConfirmHours, m.Status }).ToList();

                    string E_APPROVED_Roster = RosterStatus.E_APPROVED.ToString();
                    var lstRoster = repoAtt_Roster
                       .FindBy(m => m.Status == E_APPROVED_Roster && m.DateStart <= EndYear && m.DateEnd >= BeginYear && lstProfileID.Contains(m.ProfileID))
                       .Select(m => new RosterModel
                       {
                           ID = m.ID,
                           ProfileID = m.ProfileID,
                           DateStart = m.DateStart,
                           DateEnd = m.DateEnd,
                           Type = m.Type,
                           MonShiftID = m.MonShiftID,
                           TueShiftID = m.TueShiftID,
                           WedShiftID = m.WedShiftID,
                           ThuShiftID = m.ThuShiftID,
                           FriShiftID = m.FriShiftID,
                           SatShiftID = m.SatShiftID,
                           SunShiftID = m.SunShiftID
                       })
                       .ToList();

                    var lstShift = repoCat_Shift.GetAll().Select(m => new { m.ID, m.WorkHours, m.StdWorkHours }).ToList();


                    List<Att_CompensationDetail> detailInDB = repoAtt_CompensationDetail
                        .FindBy(m => m.Year == Year && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value))
                        .ToList();
                    List<Att_CompensationDetail> detailNew = new List<Att_CompensationDetail>();

                    foreach (var ProfileID in lstProfileID)
                    {
                        double TotalLeaveHourBeforeMonth = 0;
                        double TotalOvertimeHourBeforeMonth = 0;
                        var Profile = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
                        int MonthHire = 1;
                        if (Profile.DateHire != null && Profile.DateHire >= BeginYear && Profile.DateHire <= EndYear)
                        {
                            if (Profile.DateHire.Value.Day >= 15)
                            {
                                MonthHire = Profile.DateHire.Value.AddMonths(1).Month;
                            }
                            else
                            {
                                MonthHire = Profile.DateHire.Value.Month;
                            }
                        }
                        for (DateTime Month = BeginYear; Month < EndYear; Month = Month.AddMonths(1))
                        {
                            if (MonthHire > Month.Month)
                                continue;

                            DateTime BeginMonth = Month;
                            DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);

                            double TotalLeaveHourInMonth = 0;
                            double TotalOvertimeHourInMonth = 0;
                            #region Tăng Ca
                            var lstOvertimeByProfileByMonth = lstOvertime.Where(m => m.ProfileID == ProfileID && m.WorkDateRoot >= BeginMonth && m.WorkDateRoot <= EndMonth).ToList();
                            TotalOvertimeHourInMonth += lstOvertimeByProfileByMonth.Where(m => m.Status == E_CONFIRM_Overtime).Sum(m => m.ConfirmHours);
                            TotalOvertimeHourInMonth += lstOvertimeByProfileByMonth.Where(m => m.Status == E_APPROVED_Overtime).Sum(m => m.ApproveHours ?? 0);
                            #endregion
                            #region Ngày Nghỉ
                            var lstLeavedayByProfileByMonth = lstLeaveday.Where(m => m.DateStart <= EndMonth && m.DateEnd >= BeginMonth && m.ProfileID == ProfileID).ToList();
                            var lstRosterByProfileByMonth = lstRoster.Where(m => m.DateStart <= EndMonth && m.DateEnd >= BeginMonth && m.ProfileID == ProfileID).ToList();
                            foreach (var Leaveday in lstLeavedayByProfileByMonth)
                            {
                                DateTime dateStart = Leaveday.DateStart < BeginMonth ? BeginMonth : Leaveday.DateStart;
                                DateTime dateEnd = Leaveday.DateEnd > EndMonth ? EndMonth : Leaveday.DateEnd;
                                for (DateTime dateCheck = dateStart; dateCheck <= dateEnd; dateCheck = dateCheck.AddDays(1))
                                {
                                    if (Leaveday.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString()) // Trong Ca
                                    {
                                        TotalLeaveHourInMonth += Leaveday.LeaveHours ?? 0;
                                    }
                                    else //Toàn Ca
                                    {
                                        //Check xem có Ca làm việc không
                                        Guid? ShiftID = null;
                                        var E_TIME_OFF = lstRosterByProfileByMonth.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.Type == RosterType.E_TIME_OFF.ToString()).FirstOrDefault();
                                        var E_CHANGE_SHIFT = lstRosterByProfileByMonth.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.Type == RosterType.E_CHANGE_SHIFT.ToString()).FirstOrDefault();
                                        var E_DEFAULT = lstRosterByProfileByMonth.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.Type == RosterType.E_DEFAULT.ToString()).FirstOrDefault();
                                        var E_ROSTERGROUP = lstRosterByProfileByMonth.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck && m.Type == RosterType.E_ROSTERGROUP.ToString()).FirstOrDefault();
                                        if (E_TIME_OFF != null)
                                        {
                                        }
                                        else if (E_CHANGE_SHIFT != null)
                                        {
                                            ShiftID = GetShiftRoster(dateCheck, E_CHANGE_SHIFT);
                                        }
                                        else if (E_DEFAULT != null)
                                        {
                                            ShiftID = GetShiftRoster(dateCheck, E_DEFAULT);
                                        }
                                        else if (E_ROSTERGROUP != null)
                                        {
                                            //Chưa hỗ trợ đăng ký ca theo nhóm
                                        }

                                        if (ShiftID != null)
                                        {
                                            var Shift = lstShift.Where(m => m.ID == ShiftID.Value).FirstOrDefault();
                                            if (Shift != null)
                                            {
                                                TotalLeaveHourInMonth += Shift.StdWorkHours ?? (Shift.WorkHours ?? 0);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Gán Dữ Liệu
                            CompensationConfigModel Config = lstCompensationConfig.Where(m => m.ProfileID == ProfileID).FirstOrDefault();
                            Att_CompensationDetail detail = detailInDB.Where(m => m.MonthYear == Month && m.ProfileID == ProfileID).FirstOrDefault();
                            if (detail == null)
                            {
                                detail = new Att_CompensationDetail();
                                detail.ID = Guid.NewGuid();
                                detailNew.Add(detail);
                            }

                            if (Config != null)
                            {
                                if (Config.MonthResetInitAvailable != null && Config.MonthResetInitAvailable.Value <= Month.Month)
                                {
                                }
                                else
                                {
                                    detail.InitAvailable = Config.InitAvailable;
                                }
                                detail.MonthBeginInYear = Config.MonthBeginInYear ?? 1;
                                detail.MonthResetInitAvailable = Config.MonthResetInitAvailable;

                            }
                            detail.ProfileID = ProfileID;
                            detail.Year = Year;
                            detail.MonthYear = Month;
                            detail.MonthStartProfile = MonthHire;
                            detail.LeaveInMonth = TotalLeaveHourInMonth;
                            detail.TotalLeaveBef = TotalLeaveHourBeforeMonth;
                            detail.OvertimeInMonth = TotalOvertimeHourInMonth;
                            detail.TotalOvertimeBef = TotalOvertimeHourBeforeMonth;
                            detail.Remain = ((detail.InitAvailable ?? 0) + TotalOvertimeHourBeforeMonth + TotalOvertimeHourInMonth) - (TotalLeaveHourBeforeMonth + TotalLeaveHourInMonth);

                            TotalOvertimeHourBeforeMonth += TotalOvertimeHourInMonth;
                            TotalLeaveHourBeforeMonth += TotalLeaveHourInMonth;
                            #endregion
                        }
                    }
                    repoAtt_CompensationDetail.Add(detailNew);
                    repoAtt_CompensationDetail.SaveChanges();
                    #endregion
                }
            }
        }

        private static Guid? GetShiftRoster(DateTime DateCheck, RosterModel roster)
        {
            Guid? result = null;

            switch (DateCheck.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = roster.MonShiftID;
                    break;
                case DayOfWeek.Tuesday:
                    result = roster.TueShiftID;
                    break;
                case DayOfWeek.Wednesday:
                    result = roster.WedShiftID;
                    break;
                case DayOfWeek.Thursday:
                    result = roster.ThuShiftID;
                    break;
                case DayOfWeek.Friday:
                    result = roster.FriShiftID;
                    break;
                case DayOfWeek.Saturday:
                    result = roster.SatShiftID;
                    break;
                case DayOfWeek.Sunday:
                    result = roster.SunShiftID;
                    break;
            }
            return result;
        }



        /// <summary>
        /// Cập nhật dữ liệu Compensation khi duyệt hoặc từ chối Tăng ca
        /// </summary>
        /// <param name="lstOvertime"></param>
        public static void UpdateCompensation(List<Att_Overtime> lstOvertime)
        {
            string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
            string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
        }
        /// <summary>
        /// Cập nhật dữ liệu Compensation khi duyệt và từ chối ngày nghỉ
        /// </summary>
        /// <param name="lstLeaveDay"></param>
        public static void UpdateCompensation(List<Att_LeaveDay> lstLeaveDay)
        {
            string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
            string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
        }




        #endregion
    }
}
