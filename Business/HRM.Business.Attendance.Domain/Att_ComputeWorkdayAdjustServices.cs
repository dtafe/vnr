using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Data;

namespace HRM.Business.Attendance.Domain
{
    public class Att_ComputeWorkdayAdjustServices : BaseService
    {
        public class TamScanLogModel
        {
            public Guid ID { get; set; }
            public bool Checked { get; set; }
            public bool IsExcess { get; set; }
            public DateTime? WorkDate { get; set; }
            public DateTime? TimeLog { get; set; }
            public Guid? ProfileID { get; set; }
            public string CardCode { get; set; }
            public string CodeEmp { get; set; }
            public string SrcType { get; set; }
            public string Type { get; set; }

            public string Code
            {
                get
                {
                    string code = string.Empty;

                    if (!string.IsNullOrWhiteSpace(CardCode))
                    {
                        code = CardCode;
                    }
                    else if (!string.IsNullOrWhiteSpace(CodeEmp))
                    {
                        code = CodeEmp;
                    }

                    return code;
                }
            }
        }

        public List<Att_WorkdayEntity> ComputeWorkdayAdjust(ListQueryModel lstModel, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;

                #region xử lý lấy lstProfileIds theo OrgStructureID
                //List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                //List<Guid> lstProfileIDs = new List<Guid>();
                //List<object> lstObj = new List<object>();
                //lstObj.Add(search.OrgStructureID);
                //lstObj.Add(null);
                //lstObj.Add(null);
                //if (search.ProfileName != null && search.OrgStructureID == null)
                //{
                //}
                //else
                //{
                //    lstProfile = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status);
                //    lstProfileIDs = lstProfile.Select(s => s.ID).ToList();
                //}
                #endregion

                var lstWorkday = GetData<Att_WorkdayEntity>(lstModel, ConstantSql.hrm_att_sp_get_WorkDay, UserLogin, ref status);
                SetStatusLeaveOnWorkday(lstWorkday, unitOfWork);
                return lstWorkday;
            }
        }

        private void SetStatusLeaveOnWorkday(List<Att_WorkdayEntity> lstWorkday, IUnitOfWork unitOfWork)
        {
            var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
            var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
            var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);

            if (lstWorkday == null || lstWorkday.Count == 0)
                return;
            List<Guid> lstProfileId = lstWorkday.Select(m => m.ProfileID).Distinct().ToList();
            DateTime DateMin = lstWorkday.Min(m => m.WorkDate);
            DateTime DateMax = lstWorkday.Max(m => m.WorkDate);
            DateMax = DateMax.Date.AddDays(1).AddMinutes(-1);
            string E_CANCEL = LeaveDayStatus.E_CANCEL.ToString();
            string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
            var lstLeaveDay = repoAtt_LeaveDay
                .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL
                    && m.Status != E_REJECTED && m.DateStart <= DateMax
                    && m.DateEnd >= DateMin && lstProfileId.Contains(m.ProfileID))
                .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.LeaveHours, m.LeaveDays, m.LeaveDayTypeID, m.Status })
                .ToList();
            var lstLeaveType = repoCat_LeaveDayType
                .FindBy(m => m.IsDelete == null)
                .Select(m => new { m.ID, m.Code, m.CodeStatistic })
                .ToList();
            var lstDayOffHoliday = repoCat_DayOff
                .FindBy(m => m.IsDelete == null && m.DateOff != null)
                .Select(m => new { m.DateOff, m.Type })
                .ToList();
            foreach (var item in lstWorkday)
            {
                if (item.ShiftID != null)
                {
                    item.ShiftInTime = item.ShiftInTime;
                    item.ShiftOutTime = item.ShiftInTime.Value.AddHours(item.CoOut.Value != null ? item.CoOut.Value : 0.0);
                }

                Guid profileId = item.ProfileID;
                DateTime workDate = item.WorkDate.Date;
                var Dayoff = lstDayOffHoliday.Where(m => m.DateOff == workDate).FirstOrDefault();
                if (Dayoff != null)
                {
                    string code = string.Empty;
                    if (Dayoff.Type == HolidayType.E_HOLIDAY_HLD.ToString())
                    {
                        code = "HLD";
                    }
                    else if (Dayoff.Type == HolidayType.E_WEEKEND_HLD.ToString())
                    {
                        code = "WH";
                    }
                    else
                    {
                        code = "CH";
                    }
                    var leavetype = lstLeaveType.Where(m => m.Code == code).FirstOrDefault();
                    if (leavetype == null)
                    {
                        continue;
                    }
                    string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                    item.udLeavedayCode1 = leavedayCode;
                    continue;
                }


                var lstLeaveDayByProfile = lstLeaveDay.Where(m => m.ProfileID == profileId && m.DateStart.Date <= workDate && m.DateEnd.Date >= workDate).ToList();
                int Num = 0;
                foreach (var leaveDay in lstLeaveDayByProfile)
                {
                    Num++;
                    if (Num == 3)
                        break;
                    if (leaveDay != null)
                    {
                        if (Num == 1)
                        {
                            var leavetype = lstLeaveType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
                            //string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                            string leavedayCode = leavetype.Code;
                            item.udLeavedayCode1 = leavedayCode;
                            item.udLeavedayStatus1 = leaveDay.Status;
                        }
                        else if (Num == 2)
                        {
                            var leavetype = lstLeaveType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
                            //string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                            string leavedayCode = leavetype.Code;
                            item.udLeavedayCode2 = leavedayCode;
                            item.udLeavedayStatus2 = leaveDay.Status;
                        }
                    }
                }
            }
            return;
        }

        public string ValidateSaveWorkday(Att_WorkdayEntity WorkdaySave, Att_WorkdayEntity WorkdayOld, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_CardHistory = new CustomBaseRepository<Hre_CardHistory>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_TAMScanLog = new CustomBaseRepository<Att_TAMScanLog>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_TAMScanReasonMiss = new CustomBaseRepository<Cat_TAMScanReasonMiss>(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);

                string message = "";
                List<Att_TAMScanLog> lstTamScanLog = new List<Att_TAMScanLog>();
                string Notes = string.Empty;
                #region cập nhật giờ intime va outtime
                //1. xử lí việc insert vào TamScanlog
                //2. xử lí việc chuyển in và out theo logic in phải nhỏ hơn out
                //3. xử lý việc Validate inout không thẻ lơn hơn workday 1 ngày
                DateTime? Intime1 = null;
                if (WorkdaySave.InTime1 != null)
                {
                    Intime1 = WorkdaySave.InTime1.Value.Date;
                }
                if (!string.IsNullOrEmpty(WorkdaySave.TempTimeIn) && Intime1.HasValue)
                {
                    DateTime TimeScan = DateTime.ParseExact(WorkdaySave.TempTimeIn, "HH:mm:ss", null);
                    Intime1 = Intime1.Value.AddHours(TimeScan.Hour).AddMinutes(TimeScan.Minute).AddSeconds(TimeScan.Second);
                }
                DateTime? OutTime1 = null;
                if (WorkdaySave.OutTime1 != null)
                {
                    OutTime1 = WorkdaySave.InTime1.Value.Date;
                }
                if (!string.IsNullOrEmpty(WorkdaySave.TempTimeOut) && OutTime1.HasValue)
                {
                    DateTime TimeScan = DateTime.ParseExact(WorkdaySave.TempTimeOut, "HH:mm:ss", null);
                    OutTime1 = OutTime1.Value.AddHours(TimeScan.Hour).AddMinutes(TimeScan.Minute).AddSeconds(TimeScan.Second);
                }

                Guid? MissInOutReason = WorkdaySave.MissInOutReason;

                //Check validate ko cho phép Intime hoặc out time
                if (((WorkdayOld.InTime1 == null && Intime1 != null) || (WorkdayOld.OutTime1 == null && OutTime1 != null)) && (MissInOutReason == null || MissInOutReason == Guid.Empty))
                {
                    message = ConstantMessages.plsInputTAMScanReasonMissBeforeChangeInOut.TranslateString();
                    return message;
                }
                //Chuyển dổi vị trí in và out
                if (Intime1 != null && OutTime1 != null && (Intime1 > OutTime1))
                {
                    DateTime DateChange = Intime1.Value;
                    Intime1 = OutTime1;
                    OutTime1 = DateChange;
                    // DataBeChangedBecauseInTimeIsAfterOutTime
                }
                bool isChangeTamScan = false;
                if (Intime1 != null)
                {
                    if (WorkdayOld.InTime1 != Intime1)
                    {
                        Notes += "Intime1:" + string.Format("{0:dd/MM/yyyy hh:mm:ss}", Intime1);
                    }

                    if (WorkdayOld.InTime1 == null) //Thiếu Intime. Thì cập nhật vào lstTamscanLog
                    {
                        Att_TAMScanLog Tams = new Att_TAMScanLog();
                        Tams.ID = Guid.NewGuid();
                        Tams.TimeLog = Intime1;
                        Tams.Status = TAMScanStatus.E_MANUAL.ToString();
                        lstTamScanLog.Add(Tams);
                        isChangeTamScan = true;
                    }
                    else
                    {
                        string E_MANUAL = TAMScanStatus.E_MANUAL.ToString();
                        Att_TAMScanLog tamScan = repoAtt_TAMScanLog.FindBy(m => m.TimeLog == WorkdayOld.InTime1.Value && m.ProfileID == WorkdayOld.ProfileID && m.Status == E_MANUAL).FirstOrDefault();
                        if (tamScan != null)
                        {
                            tamScan.TimeLog = Intime1;
                            isChangeTamScan = true;
                        }
                    }
                    WorkdaySave.InTime1 = Intime1; //Cập nhật vào workday
                }
                else if (WorkdayOld.InTime1 != null && Intime1 == null)
                {
                    Notes += "Intime1:" + "Null";
                    WorkdaySave.InTime1 = Intime1; //Cập nhật vào workday
                    string E_MANUAL = TAMScanStatus.E_MANUAL.ToString();
                    //Att_TAMScanLog tamScan = EntityService.CreateQueryable<Att_TAMScanLog>(false, GuidContext, Guid.Empty, m => m.TimeLog == WorkDay.InTime1.Value && m.ProfileID == WorkDay.ProfileID && m.Status == E_MANUAL).FirstOrDefault();
                    Att_TAMScanLog tamScan = repoAtt_TAMScanLog.FindBy(m => m.TimeLog == WorkdayOld.InTime1.Value && m.ProfileID == WorkdayOld.ProfileID && m.Status == E_MANUAL).FirstOrDefault();
                    if (tamScan != null)
                    {
                        tamScan.IsDelete = true;
                    }
                }
                if (OutTime1 != null)
                {
                    if (WorkdayOld.OutTime1 != OutTime1)
                    {
                        Notes += "OutTime1:" + string.Format("{0:dd/MM/yyyy hh:mm:ss}", OutTime1);
                    }
                    if (WorkdayOld.OutTime1 == null) //Thiếu OutTime. Thì cập nhật vào lstTamscanLog
                    {
                        Att_TAMScanLog Tams = new Att_TAMScanLog();
                        Tams.ID = Guid.NewGuid();
                        Tams.TimeLog = OutTime1;
                        Tams.Status = TAMScanStatus.E_MANUAL.ToString();
                        lstTamScanLog.Add(Tams);
                        isChangeTamScan = true;
                    }
                    WorkdaySave.OutTime1 = OutTime1;
                }
                else if (WorkdayOld.OutTime1 != null && OutTime1 == null)
                {
                    Notes += "OutTime1:" + "Null";
                    WorkdaySave.OutTime1 = OutTime1; //Cập nhật vào workday
                    //Att_TAMScanLog tamScan = EntityService.CreateQueryable<Att_TAMScanLog>(false, GuidContext, Guid.Empty, m => m.TimeLog == WorkDay.OutTime1.Value && m.ProfileID == WorkDay.ProfileID && m.Status == E_MANUAL).FirstOrDefault();
                    Att_TAMScanLog tamScan = repoAtt_TAMScanLog.FindBy(m => m.TimeLog == WorkdayOld.InTime1.Value && m.ProfileID == WorkdayOld.ProfileID && m.Status == TAMScanStatus.E_MANUAL.ToString()).FirstOrDefault();
                    if (tamScan != null)
                    {
                        tamScan.TimeLog = OutTime1;
                        isChangeTamScan = true;
                    }
                }
                #endregion
                #region xử lý tamsCanLog
                DateTime workdate = WorkdayOld.WorkDate;
                string cardCode = string.Empty;
                var Card = repoHre_CardHistory.FindBy(m => (!m.IsDelete.HasValue || m.IsDelete != true) && m.ProfileID == WorkdayOld.ProfileID && m.CardCode != null && m.DateEffect <= workdate).OrderByDescending(m => m.DateEffect).FirstOrDefault();
                //Hre_CardHistoryEntity Card = EntityService.CreateQueryable<Hre_CardHistory>(false, GuidContext, Guid.Empty, m => m.ProfileID == WorkDay.ProfileID && m.CardCode != null && m.DateEffect <= workdate).OrderByDescending(m => m.DateEffect).FirstOrDefault();
                if (Card != null)
                {
                    cardCode = Card.CardCode;
                }
                else
                {
                    string CodeAttendance = repoHre_Profile.FindBy(m => m.IsDelete == null && m.ID == WorkdayOld.ProfileID).Select(m => m.CodeAttendance).FirstOrDefault();
                    //string CodeAttendance = EntityService.CreateQueryable<Hre_Profile>(false, GuidContext, Guid.Empty, m => m.ID == WorkDay.ProfileID).Select(m => m.CodeAttendance).FirstOrDefault();
                    cardCode = CodeAttendance;
                }
                if (cardCode == string.Empty)
                {
                    //return new { valid = true };
                    message = ConstantMessages.Error.TranslateString();
                    return message;
                }


                if (lstTamScanLog.Count > 0)
                {
                    foreach (var Tam in lstTamScanLog)
                    {
                        Tam.CardCode = cardCode;
                        Tam.ProfileID = WorkdayOld.ProfileID;
                    }
                    //EntityService.AddEntity<Att_TAMScanLog>(GuidContext, lstTamScanLog.ToArray());
                    repoAtt_TAMScanLog.Add(lstTamScanLog);
                }
                #endregion
                #region Xử lý Xóa Trong tamsCanLog

                if (WorkdayOld.InTime1 != null && Intime1 == null)
                {
                    string E_MANUAL = TAMScanStatus.E_MANUAL.ToString();
                    List<Att_TAMScanLog> tamScan = repoAtt_TAMScanLog.FindBy(m => m.IsDelete == null && m.TimeLog == WorkdayOld.InTime1 && m.CardCode == cardCode).ToList();
                    //List<Att_TAMScanLog> tamScan = EntityService.CreateQueryable<Att_TAMScanLog>(false, GuidContext, Guid.Empty, m => m.TimeLog == InTimeBeforeChanges.Value && m.CardCode == cardCode
                    //    && m.Status == E_MANUAL).ToList<Att_TAMScanLog>();
                    if (tamScan != null && tamScan.Count > 0)
                    {
                        foreach (var item in tamScan)
                        {
                            item.IsDelete = true;
                        }
                    }
                }
                if (WorkdayOld.OutTime1 != null && OutTime1 == null)
                {
                    string E_MANUAL = TAMScanStatus.E_MANUAL.ToString();
                    List<Att_TAMScanLog> tamScan = repoAtt_TAMScanLog.FindBy(m => m.IsDelete == null && m.TimeLog == WorkdayOld.OutTime1 && m.CardCode == cardCode).ToList();
                    //List<Att_TAMScanLog> tamScan = EntityService.CreateQueryable<Att_TAMScanLog>(false, GuidContext, Guid.Empty, m => m.TimeLog == OutTimeBeforeChanges.Value && m.CardCode == cardCode
                    //   && m.Status == E_MANUAL).ToList<Att_TAMScanLog>();
                    if (tamScan.Count > 0)
                    {
                        foreach (var item in tamScan)
                        {
                            item.IsDelete = true;
                        }
                    }
                }

                #endregion

                #region Thay đổi Shift
                var ShiftAll = repoCat_Shift.FindBy(m => m.IsDelete == null).ToList();
                Guid ShiftIDActual = Guid.Empty;
                Guid ShiftIDApprove = Guid.Empty;
                string ShiftCodeActual = string.Empty;
                string ShiftCodeApprove = string.Empty;
                var ShiftActual = ShiftAll.Where(m => m.ID == WorkdaySave.ShiftActual).FirstOrDefault();
                var ShiftApprove = ShiftAll.Where(m => m.ID == WorkdaySave.ShiftApprove).FirstOrDefault();
                if (ShiftActual != null)
                {
                    ShiftIDActual = ShiftActual.ID;
                    ShiftCodeActual = ShiftActual.Code;
                }
                if (ShiftApprove != null)
                {
                    ShiftIDApprove = ShiftApprove.ID;
                    ShiftCodeApprove = ShiftApprove.Code;
                }
                if (WorkdayOld.ShiftActual != ShiftIDActual)
                {
                    Notes += "ShiftCodeActual:" + ShiftActual.Code;
                    Notes += "ShiftCodeApprove:" + ShiftActual.Code;
                    if (ShiftActual != null) // nếu như ShiftActual thay đổi thì thay đổi 
                    {
                        ShiftIDActual = ShiftActual.ID;
                        ShiftCodeActual = ShiftActual.Code;
                        ShiftIDApprove = ShiftActual.ID;
                        ShiftCodeApprove = ShiftActual.Code;
                    }
                }
                else if (WorkdayOld.ShiftApprove != ShiftIDApprove)
                {
                    Notes += "ShiftCodeApprove:" + ShiftCodeApprove;
                }

                WorkdaySave.ShiftActual = ShiftIDActual;
                WorkdaySave.ShiftApprove = ShiftIDApprove;
                #endregion
                #region thay dổi lateEarly
                string LateEarLyModify = string.Empty;
                double MinuteLateEarly = 0;
                if (WorkdaySave.LateEarlyDuration != null)
                {
                    MinuteLateEarly = WorkdaySave.LateEarlyDuration.Value;
                }
                string LateEarlyReason = string.Empty;
                if (!string.IsNullOrEmpty(WorkdaySave.LateEarlyReason))
                {
                    LateEarlyReason = WorkdaySave.LateEarlyReason;
                }


                bool IsChangeLateEarly = false;
                if (WorkdayOld.LateEarlyDuration != null && (WorkdayOld.LateEarlyDuration != MinuteLateEarly))
                {
                    IsChangeLateEarly = true;
                    Notes += "LateEarlyDuration:" + MinuteLateEarly;
                    WorkdaySave.LateEarlyDuration = MinuteLateEarly;
                }
                if ((WorkdayOld.LateEarlyReason == null && !string.IsNullOrEmpty(LateEarlyReason)) || WorkdayOld.LateEarlyReason != LateEarlyReason)
                {
                    Notes += "LateEarlyReason:" + LateEarlyReason;
                    WorkdaySave.LateEarlyReason = LateEarlyReason;
                }
                //Cat_TAMScanReasonMiss TAMScanReasonMiss = EntityService.CreateQueryable<Cat_TAMScanReasonMiss>(false, GuidContext, Guid.Empty, m => m.TAMScanReasonMissName == MissInOutReason).FirstOrDefault();
                Cat_TAMScanReasonMiss TAMScanReasonMiss = repoCat_TAMScanReasonMiss.FindBy(m => m.IsDelete == null && m.ID == MissInOutReason).FirstOrDefault();
                if (TAMScanReasonMiss != null)
                {
                    if (WorkdayOld.MissInOutReason == Guid.Empty || WorkdayOld.MissInOutReason != TAMScanReasonMiss.ID)
                    {
                        Notes += "MissInOutReason:" + TAMScanReasonMiss.TAMScanReasonMissName;
                    }
                    WorkdaySave.TAMScanReasonMissName = TAMScanReasonMiss.TAMScanReasonMissName;
                }
                else
                {
                    Notes += "MissInOutReason:" + "NULL";
                    WorkdaySave.MissInOutReason = null;
                }
                #endregion
                #region Đăng Ký Nghỉ LeaveDay

                string LeavedayCode = string.Empty;
                if (WorkdaySave.udLeavedayCode1 != null && WorkdayOld.udLeavedayCode1 != WorkdaySave.udLeavedayCode1)
                {
                    LeavedayCode = WorkdaySave.udLeavedayCode1;
                    if (LeavedayCode == "CO")
                    {
                        message = ConstantMessages.CantRegisterCO.TranslateString();
                        return message;
                        //Common.MessageBoxs(Messages.Msg, LanguageManager.GetString(Messages.CantRegisterCO), MessageBox.Icon.WARNING, string.Empty);
                        //return new { valid = true };
                    }
                }
                if (WorkdayOld != null && (!string.IsNullOrEmpty(WorkdayOld.udLeavedayCode1) || !string.IsNullOrEmpty(WorkdayOld.udLeavedayCode2)))
                {
                    if (!string.IsNullOrEmpty(WorkdayOld.udLeavedayStatus1) && WorkdayOld.udLeavedayStatus1 == LeaveDayStatus.E_APPROVED.ToString())
                    {
                        message = ConstantMessages.StatusApproveCannotEdit.TranslateString();
                        return message;
                        //Common.MessageBoxs(Messages.Msg, LanguageManager.GetString(Messages.DataCantBeModify), MessageBox.Icon.WARNING, string.Empty);
                        //return new { valid = true };
                    }
                }

                #endregion
                string TypeOld = WorkdaySave.Type;
                //if (isChangeTamScan)
                //{
                //    WorkdaySave.udIsManualFromTamScan = true;
                //}
                List<Att_WorkdayEntity> lstWorkdaySave = new List<Att_WorkdayEntity>() { WorkdaySave };
                List<Att_Workday> lstWorkdayNew = ComputeWorkday(lstWorkdaySave.Translate<Att_Workday>(), ShiftAll, isChangeTamScan, UserLogin);
                WorkdaySave.Type = TypeOld;
                if (lstWorkdayNew.Count > 0)
                {
                    Att_Workday workdayNew = lstWorkdayNew.FirstOrDefault();
                    if (workdayNew != null)
                    {
                        if (LeavedayCode != "")
                        {
                            string ErrLeave = SaveLeaveDay(workdayNew, LeavedayCode, UserLogin);
                            if (ErrLeave != string.Empty)
                            {
                                return ErrLeave;
                                //Common.MessageBoxs(Messages.Msg, ErrLeave, MessageBox.Icon.WARNING, string.Empty);
                                //return new { valid = true };
                            }
                        }
                        workdayNew.Type = TypeOld;
                        if (IsChangeLateEarly)
                            workdayNew.LateEarlyDuration = MinuteLateEarly;
                        //if (Notes != string.Empty)
                        //{
                        //    Notes = "(" + UserLogin + "-" + string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now) + "-[" + Notes + "])";
                        //}
                        string NoteValidated = workdayNew.Note + Notes;
                        if (NoteValidated.Length > 2000)
                        {
                            NoteValidated = NoteValidated.Substring(NoteValidated.Length - 2000, 2000);
                        }
                        if (Notes != string.Empty)
                        {
                            workdayNew.SrcType = WorkdaySrcType.E_MANUAL.ToString();
                        }
                        workdayNew.Note = NoteValidated;
                        if (workdayNew.Type == string.Empty)
                        {
                            workdayNew.Type = WorkdayType.E_NORMAL.ToString();
                        }
                        DateTime DateNew = workdayNew.WorkDate;
                        Guid ProfileID = workdayNew.ProfileID;
                    }
                }
                repoAtt_Workday.Edit(lstWorkdayNew);
                //EntityService.AddEntity<Att_Workday>(GuidContext, lstWorkdayNew.ToArray());
                //EntityService.SubmitChanges(GuidContext, LoginUserID);
                repoAtt_Workday.SaveChanges();
                //unitOfWork.SaveChanges();
                return message;
            }
        }

        public List<Att_Workday> ComputeWorkday(List<Att_Workday> listWorkday, List<Cat_Shift> listShift, bool isChangeTamScan, string UserLogin)
        {

            //var listShift = EntityService.CreateQueryable<Cat_Shift>(false,
            //    guidContext, Guid.Empty).ToList<Cat_Shift>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_Pregnancy = new CustomBaseRepository<Att_Pregnancy>(unitOfWork);
                var repoCat_LateEarlyRule = new CustomBaseRepository<Cat_LateEarlyRule>(unitOfWork);
                var repoSal_Grade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                var repoCat_GradeCfg = new CustomBaseRepository<Cat_GradeCfg>(unitOfWork);
                #region Tổng hợp lại workday
                if (listWorkday != null && listWorkday.Count() > 0)
                {
                    String appConfigInfo = AppConfig.E_SERVER_TAM.ToString();
                    DateTime? dateFrom = listWorkday.Min(d => d.WorkDate);
                    DateTime? dateTo = listWorkday.Max(d => d.WorkDate);
                    BaseService service = new BaseService();

                    string status = string.Empty;
                    List<object> lstO = new List<object>();
                    lstO.Add(null);
                    lstO.Add(null);
                    lstO.Add(null);

                    var config = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);
                    var configByName = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA.ToString()).FirstOrDefault();
                    //string strMaxHoursOneShift = configByName != null ? configByName.Value1 : "";
                    //string strMaxHoursNextInOut = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT.ToString()).FirstOrDefault().Value1;
                    //string strMinMinutesSameAtt = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT.ToString()).FirstOrDefault().Value1;
                    string strTypeLoadData = configByName != null ? configByName.Value1 : string.Empty;
                    //string strSymbolIn = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTFROM.ToString()).FirstOrDefault().Value1;
                    //string strSymbolOut = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_NIGHTSHIFTTO.ToString()).FirstOrDefault().Value1;
                    //string strDetectShift = config.Where(s => s.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT.ToString()).FirstOrDefault().Value1;
                    var listHoliday = repoCat_DayOff.FindBy(m => m.IsDelete == null && m.DateOff >= dateFrom && m.DateOff <= dateTo).Select(m => new { m.ID, m.DateOff, m.Type }).ToList();


                    foreach (var workday in listWorkday)
                    {
                        if (string.IsNullOrEmpty(strTypeLoadData) || strTypeLoadData == TypeLoadData.E_DEFAULT.ToString()
                            || strTypeLoadData == TypeLoadData.E_MAXMIN.ToString())
                        {
                            List<TamScanLogModel> listTamScanLogByDate = new List<TamScanLogModel>();
                            if (!workday.InTime1.HasValue && !workday.OutTime1.HasValue)
                            {
                                if (workday.Type != WorkdayType.E_HOLIDAY.ToString())
                                {
                                    workday.Type = WorkdayType.E_MISS_IN_OUT.ToString();
                                }
                            }
                            else if (!workday.InTime1.HasValue)
                            {
                                workday.Type = WorkdayType.E_MISS_IN.ToString();
                            }
                            else if (!workday.OutTime1.HasValue)
                            {
                                workday.Type = WorkdayType.E_MISS_OUT.ToString();
                            }
                            else
                            {
                                listTamScanLogByDate.Add(new TamScanLogModel { TimeLog = workday.InTime1 });
                                listTamScanLogByDate.Add(new TamScanLogModel { TimeLog = workday.OutTime1 });

                                var listTamScanLogByShift = GetTamScanLogByShift(workday.WorkDate, listShift, workday.ShiftApprove
                                    ?? workday.ShiftID, new Dictionary<DateTime, Guid?>(), listTamScanLogByDate.ToArray());

                                if (listTamScanLogByShift != null && listTamScanLogByShift.Count() >= 2)
                                {
                                    workday.Type = WorkdayType.E_NORMAL.ToString();
                                }
                                else
                                {
                                    workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Phân tích trễ sớm

                if (listWorkday.Count > 0)
                {
                    DateTime dateFrom = listWorkday.Min(m => m.WorkDate);
                    DateTime dateTo = listWorkday.Max(m => m.WorkDate);
                    dateFrom = dateFrom.Date;
                    dateTo = dateTo.Date.AddDays(1).AddMinutes(-1);
                    List<Guid> listProfileID = listWorkday.Select(m => m.ProfileID).ToList();
                    //List<Att_LeaveDay> lstLeaveday = EntityService.CreateQueryable<Att_LeaveDay>(false, guidContext, Guid.Empty, m => m.DateEnd >= dateFrom && m.DateStart < dateTo && listProfileID.Contains(m.ProfileID)).ToList<Att_LeaveDay>();
                    string E_LEAVE_EARLY = PregnancyType.E_LEAVE_EARLY.ToString();

                    List<Att_Pregnancy> lstPrenancy = repoAtt_Pregnancy.FindBy(m => m.IsDelete == null && m.Type == E_LEAVE_EARLY && m.DateEnd >= dateFrom && m.DateStart < dateTo && listProfileID.Contains(m.ProfileID)).ToList();
                    //List<Att_Pregnancy> lstPrenancy = EntityService.CreateQueryable<Att_Pregnancy>(false, guidContext, Guid.Empty, m => m.Type == E_LEAVE_EARLY && m.DateEnd >= dateFrom && m.DateStart < dateTo && listProfileID.Contains(m.ProfileID)).ToList<Att_Pregnancy>();
                    List<Cat_LateEarlyRule> lstLateEarlyRule = repoCat_LateEarlyRule.FindBy(m => m.IsDelete == null).ToList();
                    //List<Cat_LateEarlyRule> lstLateEarlyRule = EntityService.CreateQueryable<Cat_LateEarlyRule>(false, guidContext, Guid.Empty).ToList<Cat_LateEarlyRule>();
                    List<Sal_Grade> lstGrade = repoSal_Grade.FindBy(m => m.IsDelete == null && listProfileID.Contains(m.ProfileID)).ToList();
                    //List<Sal_Grade> lstGrade = EntityService.CreateQueryable<Sal_Grade>(false, guidContext, Guid.Empty, m => listProfileID.Contains(m.ProfileID)).ToList<Sal_Grade>();
                    List<Cat_GradeCfg> lstGradeConfig = repoCat_GradeCfg.FindBy(m => m.IsDelete == null).ToList();
                    //List<Cat_GradeCfg> lstGradeConfig = EntityService.CreateQueryable<Cat_GradeCfg>(false, guidContext, Guid.Empty).ToList<Cat_GradeCfg>();
                    List<DateTime> lstDayOff = repoCat_DayOff.FindBy(m => m.IsDelete == null && m.DateOff >= dateFrom && m.DateOff <= dateTo).Select(m => m.DateOff).ToList<DateTime>();
                    //List<DateTime> lstDayOff = EntityService.CreateQueryable<Cat_DayOff>(false, guidContext, Guid.Empty, m => m.DateOff >= dateFrom && m.DateOff <= dateTo).Select(m => m.DateOff).ToList<DateTime>();
                    //setManualInoutStatusWorkday(listWorkday);

                    AnalyzeLeaveLate(listWorkday, lstPrenancy, lstLateEarlyRule, listShift, lstGrade, lstGradeConfig, lstDayOff);
                }

                #endregion

                return listWorkday;
            }
        }

        private static List<TamScanLogModel> GetTamScanLogByShift(DateTime date, List<Cat_Shift> listShift,
            Guid? shiftID, Dictionary<DateTime, Guid?> listMonthShifts, params TamScanLogModel[] listTamScanLog)
        {
            List<TamScanLogModel> listTamScanLogByShift = null;

            if (listShift != null && shiftID.HasValue && shiftID != Guid.Empty)
            {
                var shiftInfo = listShift.Where(d => d.ID == shiftID).FirstOrDefault();

                if (shiftInfo != null)
                {
                    TimeSpan firstInTime = shiftInfo.InTime.TimeOfDay;
                    TimeSpan lastInTime = shiftInfo.InTime.TimeOfDay;
                    double firstMinIn = shiftInfo.MinIn;
                    double lastMaxOut = shiftInfo.MaxOut;
                    double lastCoOut = shiftInfo.CoOut;

                    if (shiftInfo.IsDoubleShift == true)
                    {
                        var firstShift = listShift.Where(d => d.ID == shiftInfo.FirstShiftID).FirstOrDefault();
                        var lastShift = listShift.Where(d => d.ID == shiftInfo.LastShiftID).FirstOrDefault();

                        firstInTime = firstShift != null ? firstShift.InTime.TimeOfDay : lastInTime;
                        lastInTime = lastShift != null ? lastShift.InTime.TimeOfDay : lastInTime;
                        firstMinIn = firstShift != null ? firstShift.MinIn : firstMinIn;
                        lastMaxOut = lastShift != null ? lastShift.MaxOut : lastMaxOut;
                        lastCoOut = lastShift != null ? lastShift.CoOut : lastCoOut;
                    }

                    DateTime maxDateExtent = date.AddDays(1).AddSeconds(-1);
                    DateTime maxShift = date.Add(lastInTime).AddHours(lastCoOut + lastMaxOut);
                    maxDateExtent = maxDateExtent > maxShift ? maxDateExtent : maxShift;

                    if (listMonthShifts != null && listMonthShifts.ContainsKey(date.AddDays(1)))
                    {
                        var nextShiftInfo = listShift.Where(d => d.ID == listMonthShifts[date.AddDays(1)]).FirstOrDefault();

                        if (nextShiftInfo != null)
                        {
                            TimeSpan nextShiftInTime = nextShiftInfo.InTime.TimeOfDay;
                            DateTime nextShiftInTimeMin = date.AddDays(1).Add(nextShiftInTime);
                            nextShiftInTimeMin = nextShiftInTimeMin.AddHours(-nextShiftInfo.MinIn);
                            maxDateExtent = maxDateExtent > nextShiftInTimeMin ? nextShiftInTimeMin : maxDateExtent;
                        }
                    }

                    var listTamScanLogByMaxDateExtent = listTamScanLog.Where(d =>
                        d.TimeLog >= date && d.TimeLog <= maxDateExtent).ToList();

                    listTamScanLogByShift = listTamScanLogByMaxDateExtent.Where(d => d.TimeLog >= date.Add(firstInTime).AddHours(-firstMinIn)
                        && d.TimeLog <= date.Add(lastInTime).AddHours(lastCoOut + lastMaxOut)).OrderBy(d => d.TimeLog).ToList();
                }
            }

            return listTamScanLogByShift;
        }

        #region Phân tích trễ sớm
        public void AnalyzeLeaveLate(List<Att_Workday> lstWorkday, List<Att_Pregnancy> lstPrenancy, List<Cat_LateEarlyRule> lstLateEarlyRule, List<Cat_Shift> lstShift, List<Sal_Grade> lstGrade, List<Cat_GradeCfg> lstGradeConfig, List<DateTime> lstDayOff)
        {

            string workdayStatus1 = WorkdayStatus.E_APPROVED.ToString();
            string workdayStatus2 = WorkdayStatus.E_WAIT_APPROVED.ToString();

            foreach (var Workday in lstWorkday)
            {
                if (Workday.InTime1 == null || Workday.OutTime1 == null)
                {
                    continue;
                }
                if (Workday.Status == workdayStatus1 || Workday.Status == workdayStatus2)
                {
                    continue;
                }
                // Workday.udIsManualFromTamScan == false
                if (Workday.SrcType == WorkdaySrcType.E_MANUAL.ToString())
                {
                    continue;
                }
                Cat_Shift Shift = null;
                Guid? ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                Shift = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                if (Shift == null)
                    continue;
                DateTime Date = Workday.WorkDate;
                if (lstDayOff.Any(m => m == Date.Date))
                    continue;

                DateTime? dateStartShift = Date.Date.AddHours(Shift.InTime.Hour).AddMinutes(Shift.InTime.Minute);
                DateTime? dateEndShift = dateStartShift.Value.AddHours(Shift.CoOut);
                string E_LEAVE_EARLY = PregnancyType.E_LEAVE_EARLY.ToString();
                Att_Pregnancy Prenancy = lstPrenancy.Where(m => m.ProfileID == Workday.ProfileID && m.DateStart <= Workday.WorkDate && m.DateEnd > Workday.WorkDate && m.Type == E_LEAVE_EARLY).FirstOrDefault();

                Sal_Grade GradeByProfile = lstGrade.Where(m => m.ProfileID == Workday.ProfileID && m.MonthStart < Workday.WorkDate).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                Cat_GradeCfg GradeCfg = null;
                List<Cat_LateEarlyRule> lstLateEarlyRule_By_Config = new List<Cat_LateEarlyRule>();
                if (GradeByProfile != null)
                {
                    GradeCfg = lstGradeConfig.Where(m => m.ID == GradeByProfile.GradeID).FirstOrDefault();
                }
                if (GradeCfg != null)
                {
                    lstLateEarlyRule_By_Config = lstLateEarlyRule.Where(m => m.GradeCfgID == GradeCfg.ID).ToList();
                }
                if (GradeCfg == null)
                    continue;
                #region tính có trễ sớm cho trường hơp đầu ca cuối ca
                //2. Kiểm tra đủ đk để không bị trễ sớm
                bool BeLate = true;
                bool BeEarly = true;
                double HourLate = 0;
                double HourEarly = 0;
                #region cặp thứ 1
                if (Workday.InTime1 != null && Workday.OutTime1 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime1.Value, Workday.OutTime1.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>()))
                    {
                        setNonLateEarly(Workday);
                        continue;
                    }
                }
                #endregion
                #region cặp thứ 2
                if (Workday.InTime2 != null && Workday.OutTime2 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime2.Value, Workday.OutTime2.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>()))
                    {
                        setNonLateEarly(Workday);
                        continue;
                    }
                }
                #endregion
                #region cặp thứ 3
                if (Workday.InTime3 != null && Workday.OutTime3 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime3.Value, Workday.OutTime3.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>()))
                    {
                        setNonLateEarly(Workday);
                        continue;
                    }
                }
                #endregion
                #region cặp thứ 4
                if (Workday.InTime4 != null && Workday.OutTime4 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime4.Value, Workday.OutTime4.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>()))
                    {
                        setNonLateEarly(Workday);
                        continue;
                    }
                }
                #endregion


                double Late = HourLate * 60; //Phut neen nhaan cho 60
                double EarLy = HourEarly * 60; //Phut neen nhaan cho 60
                if (Late > 480)
                {
                    Late = 480;
                    EarLy = 0;
                }
                if ((Late - (int)Late) > 0)
                {
                    Late = (int)Late + 1;
                }
                else
                {
                    Late = (int)Late;
                }

                if ((EarLy - (int)EarLy) > 0)
                {
                    EarLy = (int)EarLy + 1;
                }
                else
                {
                    EarLy = (int)EarLy;
                }

                Workday.LateDuration1 = Late;
                Workday.EarlyDuration1 = EarLy;
                bool isCheck = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString())
                    isCheck = true;
                bool isUseLateEarlyRule = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString() && GradeCfg.IsDeductInLateOutEarly == true && GradeCfg.IsLateEarlyRounding == true)
                    isUseLateEarlyRule = true;
                RoundLateEarly(Prenancy, lstLateEarlyRule_By_Config, ref HourLate, ref HourEarly, isCheck, isUseLateEarlyRule);

                double LateRound = HourLate * 60;
                double EarLyRound = HourEarly * 60;
                if (LateRound > 480)
                {
                    LateRound = 480;
                    EarLyRound = 0;
                }
                if ((LateRound - (int)LateRound) > 0)
                {
                    LateRound = (int)LateRound + 1;
                }
                else
                {
                    LateRound = (int)LateRound;
                }
                if ((EarLyRound - (int)EarLyRound) > 0)
                {
                    EarLyRound = (int)EarLyRound + 1;
                }
                else
                {
                    EarLyRound = (int)EarLyRound;
                }

                Workday.LateDuration4 = LateRound;
                Workday.EarlyDuration4 = EarLyRound;
                Workday.LateEarlyDuration = LateRound + EarLyRound;
                Workday.LateEarlyRoot = LateRound + EarLyRound;
                if (Workday.LateEarlyRoot != null && Workday.LateEarlyRoot > 0)
                {
                    Workday.Type = WorkdayType.E_LATE_EARLY.ToString();
                }
                #endregion
            }
        }
        private bool IsHaveLateEarly(DateTime ShiftIn, DateTime ShiftOut, DateTime InTime, DateTime OutTime, ref bool BeLate, ref bool BeEarly, ref double HourLate, ref double HourEarly, Dictionary<DateTime, DateTime> TimeLeave)
        {
            if (InTime <= ShiftIn)
            {
                BeLate = false;
                HourLate = 0;
            }
            else
            {
                double Late = (InTime - ShiftIn).TotalHours;
                foreach (var item in TimeLeave.Keys)
                {
                    DateTime Start = item;
                    DateTime End = TimeLeave[item];
                    if (InTime >= Start && ShiftIn >= End)//neu co giao nhau
                    {
                        //Tinh thoi gian giao nhau
                        if (ShiftIn > Start)
                        {
                            Start = ShiftIn;
                        }

                        if (InTime < End)
                        {
                            End = InTime;
                        }

                        Late -= (End - Start).TotalHours;
                    }
                }
                if (HourLate == 0 || Late < HourLate)
                {
                    HourLate = Late;
                }
            }
            if (OutTime >= ShiftOut)
            {
                BeEarly = false;
                HourEarly = 0;
            }
            else
            {
                double Early = (ShiftOut - OutTime).TotalHours;
                foreach (var item in TimeLeave.Keys)
                {
                    DateTime Start = item;
                    DateTime End = TimeLeave[item];
                    if (OutTime <= Start && ShiftOut >= Start)//neu co giao nhau
                    {
                        //Tinh thoi gian giao nhau
                        if (ShiftOut < End)
                        {
                            End = ShiftOut;
                        }

                        if (Start < OutTime)
                        {
                            Start = OutTime;
                        }

                        Early -= (End - Start).TotalHours;
                    }
                }
                if (HourEarly == 0 || Early < HourEarly)
                {
                    HourEarly = Early;
                }
            }
            if (BeLate == false && BeEarly == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void setNonLateEarly(Att_Workday workday)
        {
            workday.LateDuration1 = 0;
            workday.LateDuration2 = 0;
            workday.LateDuration3 = 0;
            workday.LateDuration4 = 0;
            workday.EarlyDuration1 = 0;
            workday.EarlyDuration2 = 0;
            workday.EarlyDuration3 = 0;
            workday.EarlyDuration4 = 0;
            workday.LateEarlyDuration = 0;
            workday.LateEarlyRoot = 0;
        }
        private void RoundLateEarly(Att_Pregnancy Pregnacy, List<Cat_LateEarlyRule> lstLateEarlyRule, ref double HourLate, ref double HourEarly, bool isCheck, bool isUseLateEarlyRule)
        {
            if (isCheck == false)
            {
                HourLate = 0;
                HourEarly = 0;
                return;
            }
            if (Pregnacy != null)
            {
                if (Pregnacy.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST.ToString())
                {
                    HourLate = HourLate - 1;
                    if (HourLate < 0)
                        HourLate = 0;
                }
                else if (Pregnacy.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST.ToString())
                {
                    HourEarly = HourEarly - 1;
                    if (HourEarly < 0)
                        HourEarly = 0;
                }
            }

            if (isUseLateEarlyRule)
            {
                if (HourLate > 0 && lstLateEarlyRule != null && lstLateEarlyRule.Count > 0)
                {
                    double LateMinute = HourLate * 60;
                    var LateEarlyRule = lstLateEarlyRule.Where(m => m.MinValue <= LateMinute && m.MaxValue > LateMinute).FirstOrDefault();
                    if (LateEarlyRule != null)
                    {
                        HourLate = LateEarlyRule.RoundValue / 60;
                    }

                }
                if (HourEarly > 0 && lstLateEarlyRule != null && lstLateEarlyRule.Count > 0)
                {
                    double EarlyMinute = HourEarly * 60;
                    var LateEarlyRule = lstLateEarlyRule.Where(m => m.MinValue <= EarlyMinute && m.MaxValue > EarlyMinute).FirstOrDefault();
                    if (LateEarlyRule != null)
                    {
                        HourEarly = LateEarlyRule.RoundValue / 60;
                    }
                }
            }

        }
        #endregion
        public string SaveLeaveDay(Att_Workday Workday, string LeaveTypeCode, string UserLogin)
        {
            if (Workday == null)
                return string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoAtt_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                Att_LeavedayServices leavedayServices = new Att_LeavedayServices();
                LeaveTypeCode = LeaveTypeCode.Replace("...", string.Empty);

                Guid? ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                double DurationHour = 8;
                var shift = repoCat_Shift.FindBy(m => m.ID == ShiftID).FirstOrDefault();
                //Cat_Shift shift = EntityService.CreateQueryable<Cat_Shift>(false, GuidContext, Guid.Empty, m => m.ID == ShiftID).FirstOrDefault();
                if (shift != null)
                    DurationHour = shift.WorkHours ?? 8;
                var leaveType = new List<Cat_LeaveDayType>().Select(m => new { m.ID, m.Code, m.CodeStatistic }).FirstOrDefault();
                if (!string.IsNullOrEmpty(LeaveTypeCode))
                {
                    leaveType = repoCat_LeaveDayType.FindBy(m => m.Code == LeaveTypeCode).Select(m => new { m.ID, m.Code, m.CodeStatistic }).FirstOrDefault();
                    //leaveType = EntityService.CreateQueryable<Cat_LeaveDayType>(false, GuidContext, Guid.Empty, m => m.CodeStatistic == LeaveTypeCode).Select(m => new { m.ID, m.Code, m.CodeStatistic }).FirstOrDefault();
                }

                DateTime workday = Workday.WorkDate;
                DateTime beginDate = workday.Date;
                DateTime endDate = beginDate.AddDays(1).AddMinutes(-1);

                DateTime beginShift = workday.AddHours(shift.InTime.Hour).AddMinutes(shift.InTime.Minute);
                DateTime endShift = beginShift.AddHours(shift.CoOut);
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
                List<Att_LeaveDay> lstLeaveDayInDbUpdate = repoAtt_LeaveDay.FindBy(m => m.DateStart <= endDate && m.DateEnd >= beginDate && m.ProfileID == Workday.ProfileID && m.Status != E_REJECTED).ToList<Att_LeaveDay>();
                List<Att_LeaveDay> lstLeaveDayInsert = new List<Att_LeaveDay>();

                if (lstLeaveDayInDbUpdate.Count > 0)
                {
                    if (!lstLeaveDayInDbUpdate.Any(m => m.Status == LeaveDayStatus.E_APPROVED.ToString()))
                    {
                        if (leaveType != null)
                        {
                            lstLeaveDayInDbUpdate.ForEach(m => m.LeaveDayTypeID = leaveType.ID);
                        }
                        else
                        {
                            lstLeaveDayInDbUpdate.ForEach(m => m.IsDelete = true);
                        }
                    }
                    else
                        lstLeaveDayInDbUpdate = new List<Att_LeaveDay>();
                }
                else
                {
                    if (leaveType != null)
                    {
                        Att_LeaveDay LeavedayInsert = new Att_LeaveDay();
                        LeavedayInsert.ID = Guid.Empty;
                        LeavedayInsert.ProfileID = Workday.ProfileID;
                        LeavedayInsert.TotalDuration = 1;
                        LeavedayInsert.LeaveDays = 1;
                        LeavedayInsert.Duration = shift.WorkHours ?? 8;
                        LeavedayInsert.LeaveHours = shift.WorkHours ?? 8;
                        LeavedayInsert.DurationType = LeaveDayDurationType.E_FULLSHIFT.ToString();
                        LeavedayInsert.DateStart = workday;
                        LeavedayInsert.DateEnd = workday;
                        LeavedayInsert.LeaveDayTypeID = leaveType.ID;
                        LeavedayInsert.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        lstLeaveDayInsert.Add(LeavedayInsert);
                    }
                }
                //Cap nhat leavedayID cho workday o day
                if (lstLeaveDayInsert.Count > 0)
                {
                    if (lstLeaveDayInsert.Count > 1)
                    {
                        Workday.LeaveDayID1 = lstLeaveDayInsert[0].ID;
                        Workday.LeaveDayID2 = lstLeaveDayInsert[1].ID;
                        //Workday.udLeavedayCode1 = LeaveTypeCode;
                        //Workday.udLeavedayCode2 = LeaveTypeCode;
                        //Workday.udLeavedayStatus1 = lstLeaveDayInsert.FirstOrDefault().Status.TranslateString();
                        //Workday.udLeavedayStatus2 = lstLeaveDayInsert.FirstOrDefault().Status.TranslateString();
                    }
                    else
                    {
                        Workday.LeaveDayID1 = lstLeaveDayInsert[0].ID;
                        //Workday.udLeavedayCode1 = LeaveTypeCode;
                        //Workday.udLeavedayStatus1 = lstLeaveDayInsert.FirstOrDefault().Status.TranslateString();
                    }
                }
                //else if (lstLeaveDayInDbUpdate.Count > 0)
                //{
                //    if (lstLeaveDayInDbUpdate.Count >= 2)
                //    {
                //        Workday.udLeavedayCode1 = LeaveTypeCode;
                //        Workday.udLeavedayCode2 = LeaveTypeCode;
                //        Workday.udLeavedayStatus1 = lstLeaveDayInDbUpdate.FirstOrDefault().Status.TranslateString();
                //        Workday.udLeavedayStatus2 = lstLeaveDayInDbUpdate.FirstOrDefault().Status.TranslateString();
                //    }
                //    else
                //    {
                //        Workday.udLeavedayCode1 = LeaveTypeCode;
                //        Workday.udLeavedayStatus1 = lstLeaveDayInDbUpdate.FirstOrDefault().Status.TranslateString();
                //        Workday.udLeavedayStatus2 = lstLeaveDayInDbUpdate.FirstOrDefault().Status.TranslateString();
                //    }
                //}

                #region Triet.Mai Validate Nghỉ Bù
                if (lstLeaveDayInsert.Count > 0)
                {
                    List<Guid> LeaveTypeDayOffID = repoCat_LeaveDayType.FindBy(m => m.IsTimeOffInLieu == true).Select(m => m.ID).ToList<Guid>();
                    //List<Guid> LeaveTypeDayOffID = EntityService.CreateQueryable<Cat_LeaveDayType>(false, GuidContext, Guid.Empty, m => m.IsTimeOffInLieu == true).Select(m => m.ID).ToList<Guid>();
                    if (lstLeaveDayInsert.FirstOrDefault().Status != OverTimeStatus.E_CANCEL.ToString() && LeaveTypeDayOffID.Any(m => m == lstLeaveDayInsert.FirstOrDefault().LeaveDayTypeID))
                    {
                        string Error = leavedayServices.ValidateLeaveDayTimeOff(lstLeaveDayInsert.Select(m => m.ProfileID).Distinct().ToList(), lstLeaveDayInsert);
                        if (Error != string.Empty)
                        {
                            return Error;
                        }
                    }
                }
                #endregion

                #region triet.mai validate vấn đề ngày nghỉ dành cho nhân viên thực tập

                string E_STANDARD_WORKDAY = AppConfig.E_STANDARD_WORKDAY.ToString();
                string status = string.Empty;
                List<object> lstO = new List<object>();
                lstO.Add(E_STANDARD_WORKDAY);
                lstO.Add(null);
                lstO.Add(null);

                Sys_AllSetting config = GetData<Sys_AllSetting>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status).FirstOrDefault();
                //Sys_AppConfig config = EntityService.GetEntity<Sys_AppConfig>(false, GuidContext, Guid.Empty, m => m.Info == E_STANDARD_WORKDAY);
                string validateEmpProbation = leavedayServices.ValidateLeaveTypeByNewEmployee(config, lstLeaveDayInsert);
                if (validateEmpProbation != string.Empty)
                {
                    return validateEmpProbation;
                }
                #endregion

                #region triet.mai cap nhat leaveDays va leaveHours
                lstLeaveDayInsert.AddRange(lstLeaveDayInDbUpdate);
                if (lstLeaveDayInsert.Count > 0)
                {
                    List<Cat_LeaveDayType> lstLeaveDayType = repoCat_LeaveDayType.FindBy(m => m.IsDelete == null).ToList<Cat_LeaveDayType>();
                    //List<Cat_LeaveDayType> lstLeaveDayType = EntityService.CreateQueryable<Cat_LeaveDayType>(false, GuidContext, Guid.Empty).ToList<Cat_LeaveDayType>();
                    List<Guid> lstProfileID1 = lstLeaveDayInsert.Select(m => m.ProfileID).Distinct().ToList<Guid>();
                    List<Hre_Profile> lstProfile = repoHre_Profile.FindBy(m => lstProfileID1.Contains(m.ID)).ToList<Hre_Profile>();
                    //List<Hre_Profile> lstProfile = EntityService.CreateQueryable<Hre_Profile>(false, GuidContext, Guid.Empty, m => lstProfileID1.Contains(m.ID)).ToList<Hre_Profile>();
                    DateTime dateMin1 = lstLeaveDayInsert.Min(m => m.DateStart).Date;
                    DateTime dateMax1 = lstLeaveDayInsert.Max(m => m.DateEnd);
                    List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                    List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                    var lstGrade = repoAtt_Grade.FindBy(m => lstProfileID1.Contains(m.ProfileID.Value))
                        .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID })
                        .OrderByDescending(m => m.MonthStart).ToList();
                    //var lstGrade = EntityService.CreateQueryable<Sal_Grade>(false, GuidContext, Guid.Empty, m => lstProfileID1.Contains(m.ProfileID))
                    //    .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeID })
                    //    .OrderByDescending(m => m.MonthStart)
                    //    .Execute();
                    List<Cat_GradeAttendance> lstGradeAttendance = repoCat_GradeAttendance.FindBy(m => m.IsDelete == null).ToList();
                    //List<Cat_GradeCfg> lstGradeCfg = EntityService.CreateQueryable<Cat_GradeCfg>(false, GuidContext, Guid.Empty).ToList<Cat_GradeCfg>();
                    List<Hre_WorkHistory> listWorkHistory = repoHre_WorkHistory.FindBy(m => m.DateEffective <= dateMax1 && lstProfileID1.Contains(m.ProfileID)).OrderByDescending(m => m.DateEffective).ToList<Hre_WorkHistory>();
                    //List<Hre_WorkHistory> listWorkHistory = EntityService.CreateQueryable<Hre_WorkHistory>(false, GuidContext, Guid.Empty, m => m.DateEffective <= dateMax1 && lstProfileID1.Contains(m.ProfileID)).OrderByDescending(m => m.DateEffective).ToList<Hre_WorkHistory>();

                    E_APPROVED = RosterStatus.E_APPROVED.ToString();
                    string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();

                    if (dateMin1 == null || dateMax1 == null)
                    {
                        lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID1.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList<Att_Roster>();
                        //lstRosterTypeGroup = EntityService.Instance.CreateQueryable<Att_Roster>(false, guidContext, Guid.Empty, m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList<Att_Roster>();
                        lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null).ToList<Att_RosterGroup>();
                        //lstRosterGroup = EntityService.Instance.CreateQueryable<Att_RosterGroup>(false, guidContext, Guid.Empty, m => m.DateStart != null && m.DateEnd != null).ToList<Att_RosterGroup>();
                    }
                    else
                    {
                        lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID1.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= dateMax1).ToList<Att_Roster>();
                        //lstRosterTypeGroup = EntityService.Instance.CreateQueryable<Att_Roster>(false, guidContext, Guid.Empty, m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= DateTo).ToList<Att_Roster>();
                        lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= dateMax1 && m.DateEnd >= dateMin1).ToList<Att_RosterGroup>();
                        //lstRosterGroup = EntityService.Instance.CreateQueryable<Att_RosterGroup>(false, guidContext, Guid.Empty, m => m.DateStart != null && m.DateEnd != null && m.DateStart <= DateTo && m.DateEnd >= DateFrom).ToList<Att_RosterGroup>();
                    }
                    //RosterDAO.GetRosterGroup(GuidContext, lstProfileID1, dateMin1, dateMax1, out lstRosterTypeGroup, out lstRosterGroup);
                    List<Cat_DayOff> lstHoliday = repoCat_DayOff.FindBy(m => m.IsDelete == null).ToList<Cat_DayOff>();
                    //List<Cat_DayOff> lstHoliday = EntityService.CreateQueryable<Cat_DayOff>(false, GuidContext, Guid.Empty).ToList<Cat_DayOff>();
                    string E_APPROVED1 = RosterStatus.E_APPROVED.ToString();
                    E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                    List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(m => m.Status == E_APPROVED1 && m.DateStart <= dateMax1 && m.DateEnd >= dateMin1 && lstProfileID1.Contains(m.ProfileID)).ToList<Att_Roster>();
                    //List<Att_Roster> lstRoster = EntityService.CreateQueryable<Att_Roster>(false, GuidContext, Guid.Empty, m => m.Status == E_APPROVED1 && m.DateStart <= dateMax1 && m.DateEnd >= dateMin1 && lstProfileID1.Contains(m.ProfileID)).ToList<Att_Roster>();
                    List<DateTime> lstHolidayType = lstHoliday.Select(m => m.DateOff).ToList<DateTime>();
                    //LeaveDayDAO ldDao = new LeaveDayDAO();
                    foreach (var item in lstLeaveDayInsert)
                    {
                        if (item.DurationType != null && item.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString())
                            continue;
                        Cat_LeaveDayType leaveDayType = lstLeaveDayType.Where(m => m.ID == item.LeaveDayTypeID).FirstOrDefault();
                        if (leaveDayType == null)
                            continue;
                        Hre_Profile profileInLeave = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                        if (profileInLeave == null)
                            continue;

                        DateTime dateFrom = item.DateStart;
                        DateTime dateTo = item.DateEnd;
                        dateTo = dateTo.AddDays(1).AddMinutes(-1);

                        Guid GradeAttendanceID = lstGrade.Where(m => m.ProfileID == profileInLeave.ID && m.MonthStart <= dateMax1 && m.GradeAttendanceID.HasValue).Select(m => m.GradeAttendanceID.Value).FirstOrDefault();
                        Cat_GradeAttendance gradeAttendance = lstGradeAttendance.Where(m => m.ID == GradeAttendanceID).FirstOrDefault();
                        List<Att_Roster> lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profileInLeave.ID).ToList();
                        List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(m => m.ProfileID == profileInLeave.ID && m.DateEffective < dateTo).ToList();
                        List<Att_Roster> lstRosterByProfileTypeGroup = lstRosterByProfile.Where(m => m.Type == E_ROSTERGROUP).ToList();
                        leavedayServices.AnalyseTotalLeaveDaysAndHours(item, leaveDayType, profileInLeave, gradeAttendance, lstRosterByProfile, lstRosterGroup, listWorkHistoryByProfile, lstHoliday, shifts);
                        if (item.ID == Guid.Empty)
                        {
                            item.ID = Guid.NewGuid();
                            repoAtt_LeaveDay.Add(item);
                        }
                        else
                            repoAtt_LeaveDay.Edit(item);
                    }
                }
                #endregion

                //repoAtt_LeaveDay.Add(lstLeaveDayInsert);
                repoAtt_LeaveDay.SaveChanges();
                //EntityService.AddEntity<Att_LeaveDay>(GuidContext, lstLeaveDayInsert.ToArray());
                return string.Empty;
            }
        }
    }
}
