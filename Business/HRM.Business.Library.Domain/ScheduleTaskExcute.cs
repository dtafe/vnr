using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using VnResource.Tasks;
using VnResource.Helper.Utility;
using System.IO;
using System.Collections;
using HRM.Business.Attendance.Domain;
using HRM.Business.Main.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Attendance.Models;

namespace HRM.Business.Library.Domain
{
    public static class ScheduleTaskExcute
    {
        #region Xừ Lý Thông báo email đến các nhân viên quên quẹt thẻ

        public static void SendMailForgetTAMScanLog()
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService _base = new BaseService();
                string body2;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    string _typeTemplate = EnumDropDown.EmailType.E_SENDMAILFORGETTAMSCANLOG.ToString();
                    var template = unitOfWork.CreateQueryable<Sys_TemplateSendMail>(s => s.Type == _typeTemplate).FirstOrDefault();
                    if (template == null)
                        return;

                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return;
                    }
                    //Common.HostPath = System.Configuration.ConfigurationSettings.AppSettings["URLHost"].ToString();
                    var date1 = DateTime.Now;
                    if (date1.Day > 26)
                    {
                        date1 = date1.AddMonths(1);
                    }
                    DateTime dateStart = new DateTime(date1.Year, date1.Month, 26);
                    dateStart = dateStart.AddMonths(-1);
                    DateTime dateEnd = new DateTime(date1.Year, date1.Month, 25);
                    if (dateEnd >= DateTime.Now.Date)
                    {
                        dateEnd = DateTime.Now.Date.AddDays(-1);
                    }
                    dateEnd = dateEnd.AddDays(1).AddMilliseconds(-1);
                    var dayOffs = unitOfWork.CreateQueryable<Cat_DayOff>(s => dateStart <= s.DateOff && s.DateOff <= dateEnd).Select(s => s.DateOff).ToList();
                    var inOuts = unitOfWork.CreateQueryable<Att_Workday>(s => s.ShiftID != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                        .Select(s => new { s.InTime1, s.ProfileID, s.ShiftID, s.OutTime1, s.WorkDate }).OrderByDescending(s => s.InTime1).ToList();
                    string status = ProfileStatusSyn.E_HIRE.ToString();
                    var profiles = unitOfWork.CreateQueryable<Hre_Profile>(s => s.Email != null && s.StatusSyn == status
                        && (s.DateQuit == null || s.DateQuit > DateTime.Now)).ToList();

                    string key = AppConfig.E_SERVER_MAIL.ToString();
                    //Sys_AppConfig AppConfigServerSendMail = unitOfWork.CreateQueryable<Sys_AppConfig>(s => s.Info == key).FirstOrDefault();
                    //string host = AppConfigServerSendMail.Value1;
                    //string emailFrom = AppConfigServerSendMail.Value2;
                    //string pass = AppConfigServerSendMail.Value3;
                    //bool isSSL = false;
                    //string subject = System.Configuration.ConfigurationManager.AppSettings["SendMailInOut"];
                    //string emailTitle = System.Configuration.ConfigurationManager.AppSettings["SendMailInOut"];
                    //if (AppConfigServerSendMail.Value6 != null)
                    //    bool.TryParse(AppConfigServerSendMail.Value6, out isSSL);
                    //int port = AppConfigServerSendMail.Value30.AsInt();
                    var shifts = unitOfWork.CreateQueryable<Cat_Shift>().ToList();
                    string approve = RosterStatus.E_APPROVED.ToString();
                    var rosters = unitOfWork.CreateQueryable<Att_Roster>(s => s.MonShiftID != null && s.DateStart <= dateEnd && dateStart <= s.DateEnd && s.Status == approve).ToList<Att_Roster>();
                    List<Hre_Profile> profileHasIns = profiles;
                    List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                    List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();

                    var listHoliday = unitOfWork.CreateQueryable<Cat_DayOff>(s => dateStart <= s.DateOff && s.DateOff <= dateEnd).ToList<Cat_DayOff>();
                    string E_FIRST = PregnancyLeaveEarlyType.E_FIRST.ToString();
                    string E_LAST = PregnancyLeaveEarlyType.E_LAST.ToString();
                    var profileIDEarlys = unitOfWork.CreateQueryable<Att_Pregnancy>(s => (s.TypePregnancyEarly == E_FIRST || s.TypePregnancyEarly == E_LAST)
                        && s.DateStart <= dateEnd && dateStart <= s.DateEnd).Select(s => s.ProfileID).ToList();
                    var approveKey = LeaveDayStatus.E_APPROVED.ToString();

                    var E_FULLSHIFT = LeaveDayDurationType.E_FULLSHIFT.ToString();
                    var leavdays = unitOfWork.CreateQueryable<Att_LeaveDay>(s => s.DateStart <= dateEnd && dateStart <= s.DateEnd && s.Status == approveKey)
                        .Select(s => new { s.ProfileID, s.DateStart, s.DateEnd, s.DurationType }).ToList();
                    var leavdaysFullShifts = leavdays.Where(s => s.DurationType == E_FULLSHIFT).ToList();
                    //string contenFile = File.ReadAllText(UIController.GetPath("~/TemplateTemp/InOutTemplate.html"));
                    string codeEmp = "85S";
                    if (codeEmp == null)
                    {
                        profiles = profiles.Where(s => s.CodeEmp == codeEmp).ToList();
                    }
                    var profileIDs = profileHasIns.Select(s => s.ID).ToList();
                    var grades = unitOfWork.CreateQueryable<Att_Grade>(s => s.MonthStart <= dateEnd).OrderByDescending(d => d.MonthStart)
                        .Select(d => new { d.ProfileID, d.MonthStart, d.GradeAttendanceID }).ToList();
                    List<Guid> gardeIDs = grades.Select(d => d.GradeAttendanceID.Value).ToList();
                    List<Cat_GradeAttendance> garCfgs = unitOfWork.CreateQueryable<Cat_GradeAttendance>(s => s.Code != "GENERAL DIRECTOR" && s.Code != "DIRECTOR").ToList();
                    garCfgs = garCfgs.Where(s => gardeIDs.Contains(s.ID)).ToList();
                    var workHistorys = unitOfWork.CreateQueryable<Hre_WorkHistory>(d => d.OrganizationStructureID != null && profileIDs.Contains(d.ProfileID)).ToList<Hre_WorkHistory>();
                    foreach (var profile in profileHasIns)
                    {
                        string body = string.Empty;
                        var isSend = false;
                        int index = 1;
                        var profileLeavesFullShifts = leavdaysFullShifts.Where(s => s.ProfileID == profile.ID).ToList();
                        List<DateTime> dateLeaveFullShits = new List<DateTime>();
                        foreach (var leavesFullShift in profileLeavesFullShifts)
                        {
                            for (DateTime date = leavesFullShift.DateStart; date <= leavesFullShift.DateEnd; date = date.AddDays(1))
                            {
                                dateLeaveFullShits.Add(date);
                            }
                        }
                        for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                        {
                            if (date < profile.DateHire)
                            {
                                continue;
                            }
                            if (date.DayOfWeek == DayOfWeek.Sunday || dayOffs.Contains(date.Date) || dateLeaveFullShits.Contains(date.Date))
                            {
                                continue;
                            }
                            Att_AttendanceServices.GetRosterGroup(profileIDs, date, date, out lstRosterTypeGroup, out lstRosterGroup);
                            bool isSendIn = true;
                            bool isSendOut = true;
                            var inout = inOuts.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate == date);
                            var listRosterByProfile = rosters.Where(d => d.ProfileID == profile.ID).ToList();
                            var listWorkHistoryByProfile = workHistorys.Where(d => d.ProfileID == profile.ID).ToList();
                            var grade = grades.Where(d => d.ProfileID == profile.ID).OrderByDescending(d => d.MonthStart).FirstOrDefault();
                            Cat_GradeAttendance gradeCfg = garCfgs.FirstOrDefault(d => grade != null && d.ID == grade.GradeAttendanceID);
                            if (gradeCfg != null && gradeCfg.AttendanceMethod != AttendanceMethod.E_FULL.ToString())
                            {
                                var listRosterEntity = listRosterByProfile.Select(d => new Att_RosterEntity
                                {
                                    ID = d.ID,
                                    ProfileID = d.ProfileID,
                                    RosterGroupName = d.RosterGroupName,
                                    Type = d.Type,
                                    Status = d.Status,
                                    DateEnd = d.DateEnd,
                                    DateStart = d.DateStart,
                                    MonShiftID = d.MonShiftID,
                                    TueShiftID = d.TueShiftID,
                                    WedShiftID = d.WedShiftID,
                                    ThuShiftID = d.ThuShiftID,
                                    FriShiftID = d.FriShiftID,
                                    SatShiftID = d.SatShiftID,
                                    SunShiftID = d.SunShiftID,
                                    MonShift2ID = d.MonShiftID,
                                    TueShift2ID = d.TueShift2ID,
                                    WedShift2ID = d.WedShift2ID,
                                    ThuShift2ID = d.ThuShift2ID,
                                    FriShift2ID = d.FriShift2ID,
                                    SatShift2ID = d.SatShift2ID,
                                    SunShift2ID = d.SunShift2ID
                                }).ToList();

                                var listRosterGroupEntity = lstRosterGroup.Select(d => new Att_RosterGroupEntity
                                {
                                    ID = d.ID,
                                    DateEnd = d.DateEnd,
                                    DateStart = d.DateStart,
                                    MonShiftID = d.MonShiftID,
                                    TueShiftID = d.TueShiftID,
                                    WedShiftID = d.WedShiftID,
                                    ThuShiftID = d.ThuShiftID,
                                    FriShiftID = d.FriShiftID,
                                    SatShiftID = d.SatShiftID,
                                    SunShiftID = d.SunShiftID,
                                    RosterGroupName = d.RosterGroupName
                                }).ToList();

                                Dictionary<DateTime, Cat_Shift> listDailyShifts = Att_AttendanceLib.GetDailyShifts(profile != null ? profile.ID : Guid.Empty, date, date, listRosterEntity, listRosterGroupEntity, shifts);

                                bool isWorkDate = Att_WorkDayHelper.IsWorkDay(date, gradeCfg, listDailyShifts, listHoliday);
                                if (isWorkDate)
                                {
                                    var catshift = shifts.FirstOrDefault(s => inout != null && s.ID == inout.ShiftID);
                                    if (catshift != null && inout != null)
                                    {
                                        double minInLate = 0;
                                        double minOutEarly = 0;
                                        var leaveDayProfiles = leavdays.Where(s => s.ProfileID == profile.ID).ToList();
                                        var leaveDay = leaveDayProfiles.FirstOrDefault(s => s.DateStart.Date <= date && date <= s.DateEnd.Date);
                                        var dateBreakOut = new DateTime(date.Year, date.Month, date.Day, catshift.udCoBreakOut.Hour, catshift.udCoBreakOut.Minute, 0);
                                        var dateBreakIn = new DateTime(date.Year, date.Month, date.Day, catshift.udCoBreakIn.Hour, catshift.udCoBreakIn.Minute, 1);
                                        if (inout.InTime1 != null)//kiem tra vao tre
                                        {
                                            var dateInShift = new DateTime(date.Year, date.Month, date.Day, catshift.udCoIn.Hour, catshift.udCoIn.Minute, 0);// gio vao ca
                                            var dateInReal = new DateTime(date.Year, date.Month, date.Day, inout.InTime1.Value.Hour, inout.InTime1.Value.Minute, 0);// gio vao thuc te
                                            minInLate = dateInReal.Subtract(dateInShift).TotalMinutes;
                                            if (minInLate >= 30)// neu di tre >30 phut
                                            {
                                                if (leaveDay != null && leaveDay.DateStart != null)// neu dang ky nghi
                                                {
                                                    if (leaveDay.DateStart <= dateInReal && dateInReal <= leaveDay.DateEnd)// neu gio bat dau nghi 
                                                    {
                                                        dateInShift = leaveDay.DateEnd;
                                                    }
                                                    else if (dateInReal > leaveDay.DateStart && dateInReal > leaveDay.DateEnd)// neu gio vao > gio dang ky nghi
                                                    {
                                                        dateInShift = leaveDay.DateEnd;
                                                    }
                                                    if (dateBreakIn <= dateInReal)// neu gio ra trong vung nghi giua ca
                                                    {
                                                        dateInShift = dateBreakOut;
                                                    }
                                                    minInLate = dateInReal.Subtract(dateInShift).TotalMinutes;
                                                }
                                            }

                                        }
                                        if (inout.OutTime1 != null)//kiem tra ve som
                                        {
                                            var dateOutShift = new DateTime(date.Year, date.Month, date.Day, catshift.udCoOut.Hour, catshift.udCoOut.Minute, 0);// gio ra ca
                                            var dateOutReal = new DateTime(date.Year, date.Month, date.Day, inout.OutTime1.Value.Hour, inout.OutTime1.Value.Minute, 0);// gio vao thuc te
                                            minOutEarly = dateOutShift.Subtract(dateOutReal).TotalMinutes;
                                            if (minOutEarly >= 30)// neu ve som > 30 phut
                                            {
                                                if (dateBreakIn <= dateOutReal && dateOutReal <= dateBreakOut)// neu gio ra trong vung nghi giua ca
                                                {
                                                    dateOutReal = dateBreakOut;
                                                }
                                                if (leaveDay != null && leaveDay.DateStart != null)// neu dang ky nghi
                                                {
                                                    if (leaveDay.DateStart <= dateOutReal && dateOutReal <= leaveDay.DateEnd)// neu gio bat dau nghi 
                                                    {
                                                        dateOutShift = leaveDay.DateStart;
                                                    }
                                                    else if (dateOutReal < leaveDay.DateStart && dateOutReal < leaveDay.DateEnd)// neu gio vao > gio dang ky nghi
                                                    {
                                                        dateOutShift = leaveDay.DateStart;
                                                    }
                                                    minOutEarly = dateOutShift.Subtract(dateOutReal).TotalMinutes;
                                                }
                                            }

                                        }
                                        string minstr = string.Empty;
                                        if (minInLate < 30 && inout.InTime1 != null)
                                        {
                                            isSendIn = false;
                                        }
                                        if (minOutEarly < 30 && inout.OutTime1 != null)
                                        {
                                            isSendOut = false;
                                        }
                                        if (inout.OutTime1 == null || inout.InTime1 == null)
                                        {
                                            isSendOut = true;
                                        }
                                        if (isSendIn || isSendOut)
                                        {
                                            isSend = true;
                                        }
                                        if (inout.InTime1 != null)
                                        {
                                            if (profileIDEarlys.Contains(profile.ID))
                                            {
                                                if (minInLate < 90)
                                                {
                                                    isSendIn = false;
                                                }
                                                else
                                                {
                                                    minInLate -= 60;
                                                }
                                            }
                                            if (minInLate <= 0) minInLate = 0;
                                            if (minOutEarly <= 0) minOutEarly = 0;
                                            double sum = 0;
                                            if (minInLate >= 30)
                                                sum += minInLate;
                                            if (minOutEarly >= 30)
                                                sum += minOutEarly;
                                            minstr = sum.ToString();
                                        }
                                        if (isSendOut || isSendIn)
                                        {
                                            if (body == string.Empty)
                                            {
                                                body = "<tr><td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            }
                                            body += "<tr>";
                                            body += "<td>";
                                            body += index;
                                            body += "</td>";
                                            body += "<td style=\"text-align: left\"  class=\"dvtCellInfo\">";
                                            body += profile.ProfileName;
                                            body += "</td>";
                                            body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            body += profile.CodeEmp;
                                            body += "</td>";
                                            body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            body += date.ToShortDateString();
                                            body += "</td>";
                                            body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            body += (inout.InTime1 != null ? inout.InTime1.Value.ToString("HH:mm") : string.Empty);
                                            body += "</td>";
                                            body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            body += (inout.OutTime1 != null ? inout.OutTime1.Value.ToString("HH:mm") : string.Empty);
                                            body += "</td>";
                                            body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                            body += minstr;
                                            body += "</td>";
                                            index++;
                                        }
                                    }
                                    else
                                    {
                                        isSend = true;
                                        body += "<tr>";
                                        body += "<td>";
                                        body += index;
                                        body += "</td>";
                                        body += "<td style=\"text-align: left\"  class=\"dvtCellInfo\">";
                                        body += profile.ProfileName;
                                        body += "</td>";
                                        body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                        body += profile.CodeEmp;
                                        body += "</td>";
                                        body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                        body += date.ToShortDateString();
                                        body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                        body += "</td>";
                                        body += "<td style=\"text-align: center\"  class=\"dvtCellInfo\">";
                                        body += "</td>";
                                        index++;
                                    }
                                    body += "</tr>";
                                }
                            }
                        }
                        string emailTo = profile.Email;
                        if (isSend && !String.IsNullOrEmpty(emailTo))
                        {
                            try
                            {
                                string[] strsParaKey = new[] 
                            { 
                                "[ProfileName]"
                                ,"[Table]"
                                ,"[Date]"
                            };
                                string[] strsParaValues = new[] 
                            { 
                                profile.ProfileName,
                                body,
                               ""
                            };
                                string body1 = ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                                body2 = body1;

                                _base.SendMail(template.Subject, emailTo, body1, null);

                                //var body1 = att_OvertimeDAO.ReplaceContentFileContent(contenFile, strsParaKey, strsParaValues);

                                //string result = Common.SendMail(host, port, subject, emailTitle, body1, emailFrom, emailTo, true, isSSL, pass, true);
                                //if (!result.Contains("OK"))
                                //    Response.Write("<br/>" + "Error send mail: " + emailTo + " " + result);
                            }
                            catch
                            {
                            }
                            //Response.Write("<br/>Sended: " + emailTo + "  " + DateTime.Now.ToLongTimeString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region ReplaceContentFile

        public static string ReplaceContentFile(string contentFile, string[] strsParaKey, string[] strsParaValues)
        {
            if (!string.IsNullOrEmpty(contentFile))
            {
                for (int i = 0; i < strsParaKey.Length; i++)
                {
                    contentFile = contentFile.Replace(strsParaKey[i], strsParaValues[i]);
                }
            }

            return contentFile;
        }

        #endregion
        #region Schedule Task

        public class Task_AutoBackup : IJobItem
        {
            public string JobType { get; set; }

            public string Description { get; set; }

            public bool IsActivate { get; set; }

            public string JobArgs { get; set; }

            public void Execute()
            {

                if (!string.IsNullOrWhiteSpace(JobArgs))
                {
                    BaseService service = new BaseService();
                    string status = string.Empty;
                    var taskArgs = JobArgs.Split('|');
                    JobType = taskArgs.Length > 0 ? taskArgs[0] : string.Empty;
                    var procedure = taskArgs.Length > 1 ? taskArgs[1] : string.Empty;
                    var email = taskArgs.Length > 2 ? taskArgs[2] : string.Empty;

                    try
                    {
                        if (procedure == "SendMailForgetTAMScanLog")
                        {
                            SendMailForgetTAMScanLog();
                        }
                        else if (procedure == "SendMailContract")
                        {
                            LibraryService.SendMailContract();
                        }
                        else if (procedure == "SendMailEvalution")
                        {
                            LibraryService.SendMailEvalution();
                        }
                        else
                        {
                            //Gọi lệnh thực thi câu stored procedure
                            service.ActionData(procedure,7200, ref status);
                            service.SendMail(JobType, email, status, "");
                            //SendMail(email, JobType, status);
                            //List<object> obj = new List<object>();
                            //service.UpdateData<Eva_EvalutionData>(obj, ConstantSql.hrm_vnr_RemoveEvalutionData, ref status);
                        }

                    }
                    catch (Exception ex)
                    {

                        //Gọi hàm gửi mail và throw exception để lớp dưới ghi log
                        //Sử dụng TaskName làm tiêu đề mail gửi đi + exception
                       // SendMail(email, JobType, ex.GetExceptionMessage());
                        service.SendMail(JobType, email, ex.GetExceptionMessage(), "");
                        throw;
                    }
                }
            }

            private void SendMail(string email, string title, string message)
            {
                try
                {
                    SmtpMailHelper mailer = new SmtpMailHelper();
                    mailer.ToAddress = new string[] { email };
                    mailer.Attachments = null;
                    mailer.BCC = null;
                    mailer.CC = null;
                    mailer.Subject = title;
                    mailer.Body = message;
                    mailer.SmtpHost = "smtp.gmail.com";
                    mailer.Port = 578;
                    mailer.FromAddress = "hrm8@vnresource.net";
                    mailer.Password = "hrm8@1234";
                    mailer.Send();
                }
                catch (Exception)
                {

                }
            }
        }
        static JobScheduler scheduler = null;

        public static void RunTaskScheduler(Guid id)
        {
            if (scheduler != null)
            {
                scheduler.Stop();
                scheduler = null;
            }

            scheduler = new JobScheduler();
            string status = string.Empty;
            JobItem task = new JobItem();
            var service = new BaseService();
            var lstObj = new List<object>();
            List<Sys_AutoBackup> listAutoBackup = new List<Sys_AutoBackup>();
            var lstAutoBackup = service.GetById<Sys_AutoBackup>(id, ref status);
            listAutoBackup.Add(lstAutoBackup);
            foreach (var autoBackup in listAutoBackup)
            {
                task = new JobItem
                {
                    Interval = autoBackup.TimeWaiting == null ? 0 : autoBackup.TimeWaiting.Value,
                    DateExpired = autoBackup.DateExpired,
                    DateStart = autoBackup.DateStart.Value,
                    Description = autoBackup.Description,
                    IsActivate = autoBackup.IsActivate == null ? false : autoBackup.IsActivate.Value,
                    LastStart = autoBackup.LastStart,
                    JobArgs = autoBackup.AutoBackupName
                    + "|" + autoBackup.ProcedureName
                    + "|" + autoBackup.Email,
                    JobType = autoBackup.AutoBackupName,
                    JobItemType = typeof(Task_AutoBackup),
                    Type = (SchedulerType)Enum.Parse(typeof(SchedulerType), autoBackup.Type.ToString()),

                };
            }
            Task_AutoBackup itask = new Task_AutoBackup();
            itask.JobArgs = task.JobArgs;


            itask.Execute();
            // scheduler.Start();
        }

        public static void StartTaskScheduler(Guid id)
        {
            if (scheduler != null)
            {
                scheduler.Stop();
                scheduler = null;
            }

            scheduler = new JobScheduler();
            scheduler.TimerInterval = 60000;//30 phút
            string status = string.Empty;
            JobItem task = new JobItem();
            var service = new BaseService();
            var lstObj = new List<object>();
            List<Sys_AutoBackup> listAutoBackup = new List<Sys_AutoBackup>();
            var lstAutoBackup = service.GetById<Sys_AutoBackup>(id, ref status);
            listAutoBackup.Add(lstAutoBackup);
            foreach (var autoBackup in listAutoBackup)
            {
                scheduler.TaskItems.Add(new JobItem
                {
                    Interval = autoBackup.TimeWaiting == null ? 0 : autoBackup.TimeWaiting.Value,
                    DateExpired = autoBackup.DateExpired,
                    DateStart = autoBackup.DateStart.Value,
                    Description = autoBackup.Description,
                    IsActivate = autoBackup.IsActivate == null ? false : autoBackup.IsActivate.Value,
                    LastStart = autoBackup.LastStart,
                    JobArgs = autoBackup.AutoBackupName
                    + "|" + autoBackup.ProcedureName
                    + "|" + autoBackup.Email,
                    JobType = autoBackup.AutoBackupName,
                    JobItemType = typeof(Task_AutoBackup),
                    Type = (SchedulerType)Enum.Parse(typeof(SchedulerType), autoBackup.Type.ToString()),

                });
            }
            scheduler.Start();
        }

        public static void StartTaskSchedulerGlobal()
        {
            JobScheduler scheduler = new JobScheduler();
            scheduler.TimerInterval = 60000;//30 phút
            string status = string.Empty;

            var service = new BaseService();
            var lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(1);
            lstObj.Add(int.MaxValue - 1);
            var listAutoBackup = service.GetData<Sys_AutoBackupEntity>(lstObj, ConstantSql.hrm_sys_sp_get_AutoBackup,string.Empty, ref status).ToList();

            //var listAutoBackup = new List<Sys_AutoBackupEntity>();

            foreach (var autoBackup in listAutoBackup)
            {
                scheduler.TaskItems.Add(new JobItem
                {
                    Interval = autoBackup.TimeWaiting == null ? 0 : autoBackup.TimeWaiting.Value,
                    DateExpired = autoBackup.DateExpired,
                    DateStart = autoBackup.DateStart.Value,
                    Description = autoBackup.Description,
                    IsActivate = autoBackup.IsActivate == null ? false : autoBackup.IsActivate.Value,
                    LastStart = autoBackup.LastStart,
                    JobArgs = autoBackup.AutoBackupName
                    + "|" + autoBackup.ProcedureName
                    + "|" + autoBackup.Email,
                    JobType = autoBackup.AutoBackupName,
                    JobItemType = typeof(Task_AutoBackup),
                    Type = (SchedulerType)Enum.Parse(typeof(SchedulerType), autoBackup.Type.ToString()),
                });
            }

            scheduler.Start();
        }

        #endregion
    }
}
