using Aspose.Cells;
using HRM.Business.Attendance.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Security;
using VnResource.Helper.Data;
using VnResource.Importer;

namespace HRM.Business.Main.Domain
{
    public class ImportRosterService
    {
        #region Properties

        public string FileName { get; set; }

        public Guid LoginUserID { get; set; }

        public int SkipRowNumbers { get; set; }

        public ImportRosterType ImportType { get; set; }

        public static Dictionary<Guid, List<ImportRosterModel>> importObjects;
        public static Dictionary<Guid, List<ImportRosterModel>> invalidObjects;

        private static Dictionary<Guid, List<ImportRosterModel>> ImportObjects
        {
            get
            {
                if (importObjects == null)
                {
                    importObjects = new Dictionary<Guid, List<ImportRosterModel>>();
                }

                return importObjects;
            }
        }

        private static Dictionary<Guid, List<ImportRosterModel>> InvalidObjects
        {
            get
            {
                if (invalidObjects == null)
                {
                    invalidObjects = new Dictionary<Guid, List<ImportRosterModel>>();
                }

                return invalidObjects;
            }
        }

        #endregion

        #region ImportRoster

        public void ImportRoster()
        {
            object missing = Missing.Value;
            Workbook workbook = ExcelHelper.GetWorkbook(FileName);
            Worksheet worksheet = ExcelHelper.GetWorksheet(workbook);

            Aspose.Cells.Cells cells = worksheet.Cells;
            int startRowIndex = 3;//Mẫu roster từ dòng 3
            int startColumnIndex = 4;//Mẫu roster từ cột 4

            //Ngày đầu tháng là Cells[E2] là cells[1,4]
            var dateStart = cells["E2"].DateTimeValue;

            var monthStart = dateStart.Date;
            var monthEnd = monthStart.AddMonths(1);

            if (monthEnd.Day >= monthStart.Day)
            {
                //Tránh trường hợp kết tháng 1 và tháng 2
                monthEnd = monthEnd.AddSeconds(-1);
            }

            RemoveImportObject();
            RemoveInvalidObject();

            var listRosterCorrect = new List<ImportRosterModel>();
            var listRosterError = new List<ImportRosterModel>();
            var listRoster = new List<ImportRosterModel>();
            var listProfileCode = new List<string>();
            var listShiftCode = new List<string>();

            #region Đọc dữ liệu từ excel

            int maxColumnCount = worksheet.Cells.MaxColumn;
            int maxRowCount = worksheet.Cells.MaxRow;

            for (int i = startRowIndex; i <= maxRowCount; i += 1 + SkipRowNumbers)
            {
                var profileCode = ExcelHelper.GetExcelValue(cells[i, 1], typeof(string)).GetString();
                var profileName = ExcelHelper.GetExcelValue(cells[i, 2], typeof(string)).GetString();
                var departmentCode = ExcelHelper.GetExcelValue(cells[i, 3], typeof(string)).GetString();
                var listRosterByProfile = new List<ImportRosterModel>();

                if (!string.IsNullOrWhiteSpace(profileCode))
                {
                    profileCode = profileCode.Trim().ToUpper();

                    if (!listProfileCode.Contains(profileCode))
                    {
                        listProfileCode.Add(profileCode);
                    }

                    for (int j = startColumnIndex; j <= maxColumnCount; j++)
                    {
                        var currentDate = dateStart.AddDays(j - startColumnIndex);
                        var shiftCode = ExcelHelper.GetExcelValue(cells[i, j], typeof(string)).GetString();

                        if (!string.IsNullOrWhiteSpace(shiftCode))
                        {
                            //Dòng cuối cùng sẽ cùng tuần với ngày đang xét
                            var roster = listRosterByProfile.LastOrDefault();

                            if (roster == null || currentDate.DayOfWeek == DayOfWeek.Monday
                                || (roster != null && GetSunDate(roster) < currentDate))
                            {
                                roster = new ImportRosterModel();
                                listRosterByProfile.Add(roster);
                            }

                            if (!listShiftCode.Contains(shiftCode))
                            {
                                listShiftCode.Add(shiftCode);
                            }

                            //Dòng mới tạo thì CodeEmp chưa được set
                            if (string.IsNullOrWhiteSpace(roster.CodeEmp))
                            {
                                roster.CodeEmp = profileCode;
                                roster.ProfileName = profileName;
                                roster.CodeOrg = departmentCode;
                            }

                            if (roster != null)
                            {
                                if (currentDate.DayOfWeek == DayOfWeek.Monday)
                                {
                                    roster.MonShift = shiftCode;
                                    roster.MonDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    roster.TueShift = shiftCode;
                                    roster.TueDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Wednesday)
                                {
                                    roster.WedShift = shiftCode;
                                    roster.WedDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Thursday)
                                {
                                    roster.ThuShift = shiftCode;
                                    roster.ThuDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Friday)
                                {
                                    roster.FriShift = shiftCode;
                                    roster.FriDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    roster.SatShift = shiftCode;
                                    roster.SatDate = currentDate;
                                }
                                else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    roster.SunShift = shiftCode;
                                    roster.SunDate = currentDate;
                                }

                                roster.DateEnd = currentDate;

                                if (roster.DateStart <= SqlDateTime.MinValue.Value)
                                {
                                    roster.DateStart = currentDate;
                                }

                                if (roster.DateStart.Date < monthStart.Date)
                                {
                                    roster.DateStart = monthStart.Date;
                                }

                                if (roster.DateEnd.Date > monthEnd.Date)
                                {
                                    roster.DateEnd = monthEnd.Date;
                                }
                            }
                        }
                    }

                    if (listRosterByProfile.Any(d => listRoster.Any(p => p.CodeEmp == d.CodeEmp)))
                    {
                        var codeEmp = listRosterByProfile.Select(d => d.CodeEmp).FirstOrDefault();

                        listRosterError.Add(new ImportRosterModel
                        {
                            CodeEmp = codeEmp,
                            Description = "Trùng mã nhân viên trong file [" + codeEmp + "] dòng " + (i + 1)
                        });
                    }
                    else
                    {
                        listRoster.AddRange(listRosterByProfile.Where(d => !string.IsNullOrWhiteSpace(d.CodeEmp)).ToList());
                    }
                }
            }

            #endregion

            if (ImportObjects.ContainsKey(LoginUserID))
            {
                ImportObjects[LoginUserID] = listRosterCorrect;
            }
            else
            {
                ImportObjects.Add(LoginUserID, listRosterCorrect);
            }

            if (InvalidObjects.ContainsKey(LoginUserID))
            {
                InvalidObjects[LoginUserID] = listRosterError;
            }
            else
            {
                InvalidObjects.Add(LoginUserID, listRosterError);
            }

            //Kiểm tra dữ liệu lỗi và dữ liệu bị trùng trong db
            CheckData(listRoster, listProfileCode, listShiftCode);
        }

        private void CheckData(List<ImportRosterModel> listRoster,
            List<string> listProfileCode, List<string> listShiftCode)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                SecurityService security = new SecurityService();

                int pageSize = 2500;
                int skipRows = 0;

                var listShiftInfo = unitOfWork.CreateQueryable<Cat_Shift>(Guid.Empty,
                    d => listShiftCode.Contains(d.Code)).ToList<Cat_Shift>();

                var isAllowEditFutureRoster = security.CheckPermission(LoginUserID,
                    PrivilegeType.Modify, ConstantMessages.Att_EditFutureRoster.ToString());

                var isAllowEditPastRoster = security.CheckPermission(LoginUserID,
                    PrivilegeType.Modify, ConstantMessages.Att_EditPastRoster.ToString());

                var ValidateLess12Hour = AppConfig.HRM_ATT_VALIDATE_ROSTER_NON_CONTINUE_12HOUR.ToString();
                var check12Hours = unitOfWork.CreateQueryable<Sys_AllSetting>(d => d.Name == ValidateLess12Hour).Select(d => d.Value1).FirstOrDefault();

                while (skipRows < listProfileCode.Count())
                {
                    //Vinhtran - 20140718: Chia theo pagesize để không bị quá tải ram
                    var listProfileSplit = listProfileCode.Skip(skipRows).Take(pageSize).ToList();
                    var listRosterSplit = listRoster.Where(d => listProfileSplit.Contains(d.CodeEmp)).OrderBy(d => d.DateStart).ToList();
                    CheckData(listRosterSplit, check12Hours == bool.TrueString, isAllowEditFutureRoster, isAllowEditPastRoster, listProfileSplit, listShiftInfo);
                    skipRows += pageSize;
                }
            }
        }

        private void CheckData(List<ImportRosterModel> listRoster, bool check12Hours, bool isAllowEditFutureRoster,
            bool isAllowEditPastRoster, List<string> listProfileCode, List<Cat_Shift> listShiftInfo)
        {
            #region Kiểm tra dữ liệu lỗi

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                var listRosterCorrect = ImportObjects[LoginUserID];
                var listRosterError = InvalidObjects[LoginUserID];

                var listProfileInfo = unitOfWork.CreateQueryable<Hre_Profile>(LoginUserID,
                    d => listProfileCode.Contains(d.CodeEmp.ToUpper())).Select(d => new { d.ID, d.CodeEmp }).ToList();

                var listProfileID = listProfileInfo.Select(d => d.ID).Distinct().ToList();
                var dateStart = listRoster.Select(d => d.DateStart).OrderBy(d => d).FirstOrDefault();
                var dateEnd = listRoster.Select(d => d.DateEnd).OrderBy(d => d).LastOrDefault();
                var monthStart = dateStart.Date.AddDays(1 - dateStart.Day);
                var monthEnd = monthStart.AddMonths(1).AddSeconds(-1);
                var checkDate = dateStart.AddDays(-1);

                string cancelStatus = RosterStatus.E_CANCEL.ToString();
                string rejectStatus = RosterStatus.E_REJECTED.ToString();
                var currentDate = unitOfWork.CurrentDate();

                var listOldRoster = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, d => d.Status != cancelStatus
                    && d.Status != rejectStatus && listProfileID.Contains(d.ProfileID) && d.DateStart <= dateEnd
                    && d.DateEnd >= checkDate).Select(d => new
                    {
                        d.ProfileID,
                        d.DateStart,
                        d.DateEnd,
                        d.MonShiftID,
                        d.TueShiftID,
                        d.WedShiftID,
                        d.ThuShiftID,
                        d.FriShiftID,
                        d.SatShiftID,
                        d.SunShiftID,
                        d.Status,
                        d.Type
                    }).ToList();

                foreach (var roster in listRoster)
                {
                    var profileInfo = listProfileInfo.Where(d => d.CodeEmp != null
                        && d.CodeEmp.ToUpper() == roster.CodeEmp).FirstOrDefault();

                    if (profileInfo != null)
                    {
                        roster.ProfileID = profileInfo.ID;
                        string shiftNotFound = string.Empty;
                        var listShiftNotFound = new List<string>();

                        if (!string.IsNullOrWhiteSpace(roster.MonShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.MonShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.MonShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.MonShift))
                                {
                                    listShiftNotFound.Add(roster.MonShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.MonShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.TueShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.TueShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.TueShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.TueShift))
                                {
                                    listShiftNotFound.Add(roster.TueShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.TueShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.WedShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.WedShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.WedShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.WedShift))
                                {
                                    listShiftNotFound.Add(roster.WedShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.WedShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.ThuShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.ThuShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.ThuShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.ThuShift))
                                {
                                    listShiftNotFound.Add(roster.ThuShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.ThuShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.FriShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.FriShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.FriShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.FriShift))
                                {
                                    listShiftNotFound.Add(roster.FriShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.FriShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.SatShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.SatShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.SatShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.SatShift))
                                {
                                    listShiftNotFound.Add(roster.SatShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.SatShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(roster.SunShift))
                        {
                            var shiftInfo = listShiftInfo.Where(d =>
                                d.Code == roster.SunShift).FirstOrDefault();

                            if (shiftInfo != null)
                            {
                                roster.SunShiftID = shiftInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(roster.SunShift))
                                {
                                    listShiftNotFound.Add(roster.SunShift);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound)
                                        ? string.Empty : ",") + roster.SunShift;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(shiftNotFound))
                        {
                            roster.Description = "Không tìm thấy ca [" + shiftNotFound + "]";
                            listRosterError.Add(roster);
                        }
                        else
                        {
                            var listOldRosterByProfile = listOldRoster.Where(d => d.ProfileID == profileInfo.ID
                                && d.DateStart <= roster.DateEnd && d.DateEnd >= roster.DateStart).ToList();

                            var listApprovedRosterByProfile = listOldRosterByProfile.Where(d =>
                                d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                            if (listApprovedRosterByProfile.Count() > 0)
                            {
                                bool isPastRosterchanged = false, isFutureRosterchanged = false;

                                CheckChangedTime(currentDate, roster.MonShift, roster.MonDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.TueShift, roster.TueDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.WedShift, roster.WedDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.ThuShift, roster.ThuDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.FriShift, roster.FriDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.SatShift, roster.SatDate, ref isPastRosterchanged, ref isFutureRosterchanged);
                                CheckChangedTime(currentDate, roster.SunShift, roster.SunDate, ref isPastRosterchanged, ref isFutureRosterchanged);

                                if ((!isAllowEditPastRoster && isPastRosterchanged) || (!isAllowEditFutureRoster && isFutureRosterchanged))
                                {
                                    roster.Description = "Không được sửa lịch đã duyệt [" + roster.DateStart + " - " + roster.DateEnd + "]";
                                    listRosterError.Add(roster);
                                }
                            }

                            if (!listRosterError.Contains(roster))
                            {
                                if (ImportType == ImportRosterType.OverrideMonth)
                                {
                                    //Không kiểm tra trùng theo kiểu miền giao DateStart và DateEnd
                                    listOldRosterByProfile = listOldRosterByProfile.Where(d => !(d.DateStart >= roster.DateStart
                                        && d.DateEnd <= roster.DateEnd) && (d.DateStart < monthStart || d.DateEnd > monthEnd)).ToList();
                                }
                                else if (ImportType == ImportRosterType.OverrideHasValue)
                                {
                                    listOldRosterByProfile.Clear();
                                }

                                if (listOldRosterByProfile.Count() > 0)
                                {
                                    roster.Description = "Trùng lịch làm việc [" + roster.DateStart + " - " + roster.DateEnd + "]";
                                    listRosterError.Add(roster);
                                }
                                else
                                {
                                    var dateFrom = roster.DateStart.Date;
                                    var dateTo = roster.DateEnd.Date;
                                    bool shiftAvailable = true;

                                    if (check12Hours)
                                    {
                                        var rosterBefore = listOldRoster.Where(d => d.ProfileID == profileInfo.ID
                                            && d.DateEnd.Date == dateFrom.AddDays(-1)).FirstOrDefault();

                                        var rosterAfter = listOldRoster.Where(d => d.ProfileID == profileInfo.ID
                                            && d.DateStart.Date == dateTo.AddDays(1)).FirstOrDefault();

                                        if (rosterBefore == null)
                                        {
                                            rosterBefore = listRoster.Where(d => d != roster && d.CodeEmp == roster.CodeEmp
                                                && d.DateEnd.Date == dateFrom.AddDays(-1)).Select(d => new
                                                {
                                                    d.ProfileID,
                                                    d.DateStart,
                                                    d.DateEnd,
                                                    d.MonShiftID,
                                                    d.TueShiftID,
                                                    d.WedShiftID,
                                                    d.ThuShiftID,
                                                    d.FriShiftID,
                                                    d.SatShiftID,
                                                    d.SunShiftID,
                                                    d.Status,
                                                    d.Type
                                                }).FirstOrDefault();
                                        }

                                        for (DateTime date = dateFrom.Date; date <= dateTo; date = date.AddDays(1))
                                        {
                                            if (shiftAvailable)
                                            {
                                                Guid? shiftBefore = null;
                                                Guid? shiftAfter = null;

                                                if (date.DayOfWeek == DayOfWeek.Monday && roster.MonShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.SunShiftID;
                                                        shiftAfter = roster.MonShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.MonShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.TueShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.MonShiftID;
                                                    shiftAfter = roster.TueShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Tuesday && roster.TueShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.MonShiftID;
                                                        shiftAfter = roster.TueShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.TueShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.WedShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.TueShiftID;
                                                    shiftAfter = roster.WedShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Wednesday && roster.WedShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.TueShiftID;
                                                        shiftAfter = roster.WedShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.WedShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.ThuShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.WedShiftID;
                                                    shiftAfter = roster.ThuShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Thursday && roster.ThuShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.WedShiftID;
                                                        shiftAfter = roster.ThuShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.ThuShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.FriShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.ThuShiftID;
                                                    shiftAfter = roster.FriShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Friday && roster.FriShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.ThuShiftID;
                                                        shiftAfter = roster.FriShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.FriShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.SatShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.FriShiftID;
                                                    shiftAfter = roster.SatShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Saturday && roster.SatShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.FriShiftID;
                                                        shiftAfter = roster.SatShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.SatShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.SunShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    shiftBefore = roster.SatShiftID;
                                                    shiftAfter = roster.SunShiftID ?? null;
                                                }
                                                else if (date.DayOfWeek == DayOfWeek.Sunday && roster.SunShiftID.HasValue)
                                                {
                                                    if (date == dateFrom && rosterBefore != null)
                                                    {
                                                        shiftBefore = rosterBefore.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterBefore.MonShiftID : rosterBefore.SatShiftID;
                                                        shiftAfter = roster.SunShiftID;

                                                        shiftAvailable = CheckShift(date.AddDays(-1), shiftBefore, shiftAfter, listShiftInfo);
                                                    }
                                                    else if (date == dateTo && rosterAfter != null)
                                                    {
                                                        shiftBefore = roster.SunShiftID;
                                                        shiftAfter = rosterAfter.Type == RosterType.E_CHANGE_SHIFT.ToString() ? rosterAfter.MonShiftID : rosterAfter.MonShiftID;

                                                        shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                    }

                                                    if (date < dateTo)
                                                    {
                                                        shiftBefore = roster.SunShiftID;
                                                        shiftAfter = roster.MonShiftID ?? null;
                                                    }
                                                }

                                                if (shiftAvailable)
                                                {
                                                    shiftAvailable = CheckShift(date, shiftBefore, shiftAfter, listShiftInfo);
                                                }

                                                if (!shiftAvailable)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (shiftAvailable)
                                    {
                                        listRosterCorrect.Add(roster);
                                    }
                                    else
                                    {
                                        roster.Description = "Ca làm việc liên tục dưới 12 tiếng";
                                        listRosterError.Add(roster);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        roster.Description = "Không tìm thấy nhân viên [" + roster.CodeEmp + "]";
                        listRosterError.Add(roster);
                    }
                }
            }

            #endregion
        }

        private void CheckChangedTime(DateTime currentDate, string shiftCode, DateTime? shiftDate,
            ref bool isPastRosterchanged, ref bool isFutureRosterchanged)
        {
            if (!string.IsNullOrWhiteSpace(shiftCode))
            {
                if (shiftDate < currentDate.Date)
                {
                    isPastRosterchanged = true;
                }
                else
                {
                    isFutureRosterchanged = true;
                }
            }
        }

        private DateTime GetSunDate(ImportRosterModel roster)
        {
            DateTime result = DateTime.MinValue;

            if (roster != null)
            {
                if (roster.DateEnd.DayOfWeek == DayOfWeek.Sunday)
                {
                    result = roster.DateEnd;
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Saturday)
                {
                    result = roster.DateEnd.AddDays(1);
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Friday)
                {
                    result = roster.DateEnd.AddDays(2);
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Thursday)
                {
                    result = roster.DateEnd.AddDays(3);
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Wednesday)
                {
                    result = roster.DateEnd.AddDays(4);
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Tuesday)
                {
                    result = roster.DateEnd.AddDays(5);
                }
                else if (roster.DateEnd.DayOfWeek == DayOfWeek.Monday)
                {
                    result = roster.DateEnd.AddDays(6);
                }
            }

            return result;
        }

        private bool CheckShift(DateTime date, Guid? shiftBeforeID,
            Guid? shiftAfterID, List<Cat_Shift> listShiftInfo)
        {
            bool result = true;

            if (shiftBeforeID.HasValue && shiftAfterID.HasValue)
            {
                var shiftBefore = listShiftInfo.Where(d => d.ID == shiftBeforeID).FirstOrDefault();
                var shiftAfter = listShiftInfo.Where(d => d.ID == shiftAfterID).FirstOrDefault();

                if (shiftBefore != null && shiftAfter != null)
                {
                    var endShiftBefore = date.Date.Add(shiftBefore.InTime.TimeOfDay).AddHours(shiftBefore.CoOut);
                    var startShiftAfter = date.Date.AddDays(1).Add(shiftAfter.InTime.TimeOfDay);
                    double numMinHour = 12;//2 ca liên tiếp không được nhỏ hơn 12 tiếng

                    if (startShiftAfter.Subtract(endShiftBefore).TotalHours < numMinHour)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        #endregion

        public DataErrorCode SaveRoster()
        {
            var dataErrorCode = DataErrorCode.Success;
            var ListRosterCorrect = GetImportObject();

            if (ListRosterCorrect != null && ListRosterCorrect.Count() > 0)
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);
                    var listDuplicate = new List<Att_Roster>();

                    var listRoster = ListRosterCorrect.Select(d =>
                        new Att_Roster
                        {
                            ProfileID = d.ProfileID,
                            DateStart = d.DateStart,
                            DateEnd = d.DateEnd,
                            MonShiftID = d.MonShiftID,
                            TueShiftID = d.TueShiftID,
                            WedShiftID = d.WedShiftID,
                            ThuShiftID = d.ThuShiftID,
                            FriShiftID = d.FriShiftID,
                            SatShiftID = d.SatShiftID,
                            SunShiftID = d.SunShiftID,
                            Type = RosterType.E_DEFAULT.ToString(),
                            Status = RosterStatus.E_APPROVED.ToString()
                        }).ToArray();

                    if (ImportType == ImportRosterType.OverrideMonth)
                    {
                        var listProfileID = listRoster.Select(d => d.ProfileID).Distinct().ToList();
                        var dateStart = listRoster.Where(d => d.DateStart > SqlDateTime.MinValue.Value).Select(d => d.DateStart.Date).OrderBy(d => d).FirstOrDefault();
                        var dateEnd = dateStart.AddMonths(1).Date;//Sau này nếu nghiệp vụ thay đổi chỉ xóa những ngày khai báo thì lấy max của cột DateEnd trong danh sách

                        //Không xóa theo kiểu miền giao DateStart và DateEnd -> có thể sai trường hợp roster dài nhiều tháng
                        unitOfWork.Delete<Att_Roster>(unitOfWork.CreateQueryable<Att_Roster>(d => d.DateStart >= dateStart
                            && d.DateEnd <= dateEnd && listProfileID.Contains(d.ProfileID)));
                    }
                    else if (ImportType == ImportRosterType.OverrideHasValue)
                    {
                        var listProfileID = listRoster.Select(d => d.ProfileID).Distinct().ToList();
                        var dateStart = listRoster.Where(d => d.DateStart > SqlDateTime.MinValue.Value).Select(d => d.DateStart.Date).OrderBy(d => d).FirstOrDefault();
                        var dateEnd = listRoster.Where(d => d.DateEnd > SqlDateTime.MinValue.Value).Select(d => d.DateEnd.Date).OrderBy(d => d).LastOrDefault();

                        var listOldRoster = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, d => d.DateStart <= dateEnd
                            && d.DateEnd >= dateStart && listProfileID.Contains(d.ProfileID)).ToList<Att_Roster>();

                        foreach (var roster in listRoster)
                        {
                            DateTime rosterStart = roster.DateStart.Date;
                            DateTime rosterEnd = roster.DateEnd.Date;

                            for (DateTime date = rosterStart; date <= rosterEnd; date = date.AddDays(1))
                            {
                                var oldRoster = listOldRoster.Where(d => d.DateStart.Date <= date &&
                                    d.DateEnd.Date >= date && d.ProfileID == roster.ProfileID).FirstOrDefault();

                                if (oldRoster != null)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Monday && roster.MonShiftID.HasValue)
                                    {
                                        oldRoster.MonShiftID = roster.MonShiftID;
                                        roster.MonShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Tuesday && roster.TueShiftID.HasValue)
                                    {
                                        oldRoster.TueShiftID = roster.TueShiftID;
                                        roster.TueShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Wednesday && roster.WedShiftID.HasValue)
                                    {
                                        oldRoster.WedShiftID = roster.WedShiftID;
                                        roster.WedShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Thursday && roster.ThuShiftID.HasValue)
                                    {
                                        oldRoster.ThuShiftID = roster.ThuShiftID;
                                        roster.ThuShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Friday && roster.FriShiftID.HasValue)
                                    {
                                        oldRoster.FriShiftID = roster.FriShiftID;
                                        roster.FriShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Saturday && roster.SatShiftID.HasValue)
                                    {
                                        oldRoster.SatShiftID = roster.SatShiftID;
                                        roster.SatShiftID = null;
                                    }
                                    else if (date.DayOfWeek == DayOfWeek.Sunday && roster.SunShiftID.HasValue)
                                    {
                                        oldRoster.SunShiftID = roster.SunShiftID;
                                        roster.SunShiftID = null;
                                    }
                                }
                            }

                            if ((!roster.MonShiftID.HasValue && !roster.TueShiftID.HasValue && !roster.WedShiftID.HasValue
                                && !roster.ThuShiftID.HasValue && !roster.FriShiftID.HasValue && !roster.SatShiftID.HasValue
                                && !roster.SunShiftID.HasValue) || roster.DateStart > roster.DateEnd)
                            {
                                listDuplicate.Add(roster);
                            }
                        }
                    }

                    var listImported = listRoster.Where(d => !listDuplicate.Contains(d));
                    unitOfWork.SetCorrectOrgStructureID(listImported.ToList());

                    unitOfWork.AddObject(listImported.ToArray());
                    dataErrorCode = unitOfWork.SaveChanges(LoginUserID);

                    RemoveImportObject();
                    RemoveInvalidObject();
                }
            }

            return dataErrorCode;
        }

        #region GetImportObject

        public List<ImportRosterModel> GetImportObject()
        {
            if (ImportObjects.ContainsKey(LoginUserID))
            {
                return ImportObjects[LoginUserID];
            }

            return new List<ImportRosterModel>();
        }

        public List<ImportRosterModel> GetInvalidObject()
        {
            if (InvalidObjects.ContainsKey(LoginUserID))
            {
                return InvalidObjects[LoginUserID];
            }

            return new List<ImportRosterModel>();
        }

        private void RemoveImportObject()
        {
            if (ImportObjects.ContainsKey(LoginUserID))
            {
                ImportObjects[LoginUserID] = null;
                ImportObjects.Remove(LoginUserID);
            }
        }

        private void RemoveInvalidObject()
        {
            if (InvalidObjects.ContainsKey(LoginUserID))
            {
                InvalidObjects[LoginUserID] = null;
                InvalidObjects.Remove(LoginUserID);
            }
        }

        #endregion
    }
}
