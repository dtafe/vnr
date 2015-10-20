using System.Linq;
using HRM.Business.Laundry.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using System.Data;
using HRM.Business.Hr.Models;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;

namespace HRM.Business.Laundry.Domain
{
    public class Lau_ReportServices : BaseService
    {

        #region  - Báo cáo chi tiết nhân viên giặt
        public DataTable GetReportEmpDetail(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> marketIDs, List<Guid> lstProfileIds, DateTime dateStart, DateTime dateEnd, bool isIncludeQuitEmp, String codeEmp, string userLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            DataTable datatable = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_Record = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var laundryRecords = repoLau_Record.FindBy(s => s != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd).ToList();
                if (laundryRecords == null || laundryRecords.Count < 1)
                {
                    return null;
                }
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();

                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status)
                    .Where(s => lstProfileIds.Contains(s.ID))
                    .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance })
                    .ToList();
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                if (lockerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lockerIDs.Contains(m.LockerID)).ToList();
                }
                if (marketIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => marketIDs.Contains(m.MarkerID)).ToList();
                }
                if (lineIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }
                
                var lockers = repoLau_Locker.FindBy(s=>s.IsDelete == null).ToList();
                var markers = repoLau_Marker.FindBy(s=>s.IsDelete == null).ToList();
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();

                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (!datatable.Columns.Contains("Data" + date.Day))
                        datatable.Columns.Add("Data" + date.Day, typeof(DateTime));
                }
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                foreach (var record in laundryRecords)
                {
                    DataRow row = datatable.NewRow();
                    var profile = profiles.FirstOrDefault(s => s.ID == record.ProfileID);
                    var locker = lockers.FirstOrDefault(s => s.ID == record.LockerID);
                    var line = lines.FirstOrDefault(s => s.ID == record.LineID);
                    var marker = markers.FirstOrDefault(s => s.ID == record.MarkerID);

                    row[Lau_DataTableReportEntity.FieldNames.LockerName] = locker != null ? locker.LockerLMSName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.LineName] = line != null ? line.LineLMSName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.MarkerName] = marker != null ? marker.MarkerName : string.Empty;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(org.ID, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(org.ID, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(org.ID, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(org.ID, OrgUnit.E_GROUP, orgs, orgTypes);

                    row[Lau_DataTableReportEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;

                    row[Lau_DataTableReportEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Lau_DataTableReportEntity.FieldNames.CodeEmp] = profile.CodeEmp != null ? profile.CodeEmp : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.CodeAttendance] = profile.CodeAttendance != null ? profile.CodeAttendance : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.ProfileName] = profile.ProfileName != null ? profile.ProfileName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                    row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;
                    row[Lau_DataTableReportEntity.FieldNames.TimeLog] = record.TimeLog;
                    row["Data" + record.TimeLog.Value.Day] = record.TimeLog;
                    datatable.Rows.Add(row);
                }
                return datatable;
            }
        }
        #endregion

        #region - Báo cáo tổng hợp số lượng giặt là 
        /// <summary>
        /// [Hieu.Van]3.5.1	Báo cáo tổng hợp số lượng giặt là 
        /// </summary>
        /// <param name="lstLine"></param>
        /// <param name="lstLocker"></param>
        /// <param name="lstMarker"></param>
        /// <returns></returns>
        public DataTable GetReportSummaryLaundryRecord(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> marketIDs, List<Guid> lstProfileIds,
            DateTime dateStart, DateTime dateEnd, bool isIncludeQuitEmp, String codeEmp, string UserLogin)
        {
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            DataTable datatable = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_LaundryRecord = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);

                var laundryRecords = repoLau_LaundryRecord
                    .FindBy(s => s.ProfileID != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                    .Select(s => new { s.ID, s.ProfileID, s.TimeLog, s.LockerID, s.MarkerID, s.LineID, s.UserCreate, s.DateCreate, s.MachineCode, s.Amount })
                    .ToList();
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();

                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status)
                .Where(s => lstProfileIds.Contains(s.ID))
                .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.CodeEmp })
                .ToList();

                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();
                var lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var markets = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }

                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();

                if (lockerIDs.Count > 0)
                {
                    laundryRecords = laundryRecords.Where(s => s.LockerID != null && lockerIDs.Contains(s.LockerID.Value)).ToList();
                    lockers = lockers.Where(l => lockerIDs.Contains(l.ID)).ToList();
                }
                if (marketIDs.Count > 0)
                {
                    laundryRecords = laundryRecords.Where(s => s.MarkerID != null && marketIDs.Contains(s.MarkerID.Value)).ToList();
                    markets = markets.Where(m => marketIDs.Contains(m.ID)).ToList();
                }
                if (lineIDs.Count > 0)
                {
                    laundryRecords = laundryRecords.Where(s => s.LineID != null && lineIDs.Contains(s.LineID.Value)).ToList();
                    lines = lines.Where(l => lineIDs.Contains(l.ID)).ToList();
                }

                foreach (var marker in markets)
                {
                    var lokerIds = lines.Where(s => s.MarkerID == marker.ID).Select(s => s.LockerID).ToList();
                    var locker1s = lockers.Where(s => lokerIds.Contains(s.ID)).ToList();
                    foreach (var locker1 in locker1s)
                    {
                        var line1s = lines.Where(s => s.LockerID == locker1.ID && s.MarkerID == marker.ID).ToList();
                        foreach (var line1 in line1s)
                        {
                            var laundrys = laundryRecords.Where(s => s.LineID == line1.ID && s.LockerID == locker1.ID && s.MarkerID == marker.ID).ToList();
                            if (laundrys.Count > 0)
                            {
                                DataRow row = datatable.NewRow();
                                row[Lau_DataTableReportEntity.FieldNames.MarkerName] = marker.MarkerName;
                                row[Lau_DataTableReportEntity.FieldNames.LockerName] = locker1.LockerLMSName;
                                row[Lau_DataTableReportEntity.FieldNames.LineName] = line1.LineLMSName;
                                row[Lau_DataTableReportEntity.FieldNames.Price] = line1.Amount;
                                row[Lau_DataTableReportEntity.FieldNames.Volume] = laundrys.Count;
                                if (line1.Amount > 0)
                                {
                                    row[Lau_DataTableReportEntity.FieldNames.TotalAmount] = laundrys.Count * line1.Amount;
                                }
                                row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                                row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;
                                datatable.Rows.Add(row);
                            }
                        }
                    }
                }
                return datatable;
            }
        }
        #endregion

        #region - Báo cáo số lượng quần áo giặt  
        /// <summary>
        /// [Hieu.Van]3.5.1	Báo cáo số lượng quần áo giặt  
        /// </summary>
        /// <param name="lstLine"></param>
        /// <param name="lstLocker"></param>
        /// <param name="lstMarker"></param>
        /// <returns></returns>
        public DataTable GetReportClothing(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> markerIDs, List<Guid> lstProfileIds, DateTime dateStart,
            DateTime dateEnd, List<Guid> shiftIDs, bool isIncludeQuitEmp, string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            DataTable datatable = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_Record = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var laundryRecords = repoLau_Record.FindBy(s => s != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd).ToList();
                if (laundryRecords == null || laundryRecords.Count < 1)
                {
                    return null;
                }
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();

                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status)
                    .Where(s => lstProfileIds.Contains(s.ID))
                    .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance })
                    .ToList();
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    profileIds = profiles.Select(s => s.ID).ToList();
                    laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (lockerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lockerIDs.Contains(m.LockerID)).ToList();
                }
                if (markerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => markerIDs.Contains(m.MarkerID)).ToList();
                }
                if (lineIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }

                var lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var markers = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();

                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (!datatable.Columns.Contains("Data" + date.Day))
                        datatable.Columns.Add("Data" + date.Day, typeof(double));
                }
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                foreach (var profile in profiles)
                {
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var laundryRecordProfiles = laundryRecords.Where(s => s.ProfileID == profile.ID && s.TimeLog.Value.Date == date.Date).ToList();
                        if (laundryRecordProfiles.Count > 0)
                        {
                            DataRow row = datatable.NewRow();
                            row[Lau_DataTableReportEntity.FieldNames.TimeLog] = date.Date;
                            row[Lau_DataTableReportEntity.FieldNames.Total] = laundryRecordProfiles.Where(s => s.TimeLog.Value.Date == date.Date).Count();

                            var locker = lockers.FirstOrDefault(s => s.ID == laundryRecordProfiles[0].LockerID);
                            var line = lines.FirstOrDefault(s => s.ID == laundryRecordProfiles[0].LineID);
                            row[Lau_DataTableReportEntity.FieldNames.LockerName] = locker != null ? locker.LockerLMSName : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.LineName] = line != null ? line.LineLMSName : string.Empty;

                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                            row[Lau_DataTableReportEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;

                            row[Lau_DataTableReportEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                            row[Lau_DataTableReportEntity.FieldNames.CodeEmp] = profile.CodeEmp != null ? profile.CodeEmp : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.CodeAttendance] = profile.CodeAttendance != null ? profile.CodeAttendance : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.ProfileName] = profile.ProfileName != null ? profile.ProfileName : string.Empty;
                            row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                            row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;

                            row[Lau_DataTableReportEntity.FieldNames.TotalAmount] = laundryRecords.Count(s => s.ProfileID == profile.ID);
                            row["Data" + date.Day] = laundryRecordProfiles.Count;
                            datatable.Rows.Add(row);
                        }
                    }
                }
                return datatable;
            }
        } 
        #endregion

        #region - Báo cáo Tổng Hợp số lượng Quần Áo Giặt
        public DataTable GetReportSummaryClothing(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> markerIDs, List<Guid> lstProfileIds, DateTime dateStart,
            DateTime dateEnd, List<Guid> shiftIDs, bool isIncludeQuitEmp, string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            DataTable table = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_LaundryRecord = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);


                var laundryRecords = repoLau_LaundryRecord
                                .FindBy(s => s.ProfileID != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                                .Select(s => new { s.ProfileID, s.TimeLog, s.LockerID, s.MarkerID, s.LineID, s.Amount })
                                .ToList();
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status)
                   .Where(s => lstProfileIds.Contains(s.ID))
                   .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance })
                   .ToList();
                var workDays = repoAtt_Workday
                                .FindBy(s => dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                               .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate })
                               .ToList();

                var lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var markers = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    profileIds = profiles.Select(s => s.ID).ToList();
                    laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (lockerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lockerIDs.Contains(m.LockerID)).ToList();
                }
                if (lineIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }
                if (markerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }
                if (shiftIDs.Count != 0)
                {
                    var workDayProfileIDs = workDays.Select(s => s.ProfileID).ToList();
                    laundryRecords = laundryRecords.Where(s => workDayProfileIDs.Contains(s.ProfileID.Value)).ToList();
                    shifts = shifts.Where(s => shiftIDs.Contains(s.ID)).ToList();
                }
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                foreach (var locker in lockers)
                {
                    if (!table.Columns.Contains(locker.Code))
                        table.Columns.Add(locker.Code);
                }
                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    foreach (var shift in shifts)
                    {
                        var laundryProfiles = laundryRecords.Where(s => s.TimeLog.Value.Date == date.Date).ToList();
                        if (laundryProfiles.Count > 0)
                        {
                            DataRow row = table.NewRow();
                            row[Lau_DataTableReportEntity.FieldNames.TimeLog] = date;
                            foreach (var locker in lockers)
                            {
                                var count = laundryProfiles.Count(s => s.LockerID == locker.ID);
                                if (count > 0)
                                    row[locker.Code] = count;
                            }
                            row[Lau_DataTableReportEntity.FieldNames.ShiftName] = shift.ShiftName;
                            row[Lau_DataTableReportEntity.FieldNames.Total] = laundryProfiles.Count;
                            row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                            row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;
                            table.Rows.Add(row);
                        }
                    }

                }
                return table;
            }
        }
        #endregion

        #region - Báo cáo trừ tiền giặt
        public DataTable GetReportExceptClothing(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> markerIDs, List<Guid> lstProfileIds, DateTime dateStart,
            DateTime dateEnd, List<Guid> shiftIDs, bool isIncludeQuitEmp, string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            DataTable datatable = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_Record = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var laundryRecords = repoLau_Record
                        .FindBy(s => s.ProfileID != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                        .Select(s => new { s.ProfileID, s.TimeLog, s.LockerID, s.MarkerID, s.LineID, s.Amount })
                        .ToList();
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();

                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status)
                   .Where(s => lstProfileIds.Contains(s.ID))
                   .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance })
                   .ToList();


                var workDays = repoAtt_Workday
                        .FindBy(s => (s.InTime1 != null || s.OutTime1 != null) && profileIds.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                        .Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID })
                        .ToList();
                var lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var markers = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    profileIds = profiles.Select(s => s.ID).ToList();
                    laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (lockerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lockerIDs.Contains(m.LockerID)).ToList();
                }
                if (lineIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }
                if (markerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }

                if (shiftIDs.Count != 0)
                {
                    var workDayProfileIDs = workDays.Where(s => dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.ShiftID != null && shiftIDs.Contains(s.ShiftID.Value))
                        .Select(s => s.ProfileID).ToList();
                    laundryRecords = laundryRecords.Where(s => workDayProfileIDs.Contains(s.ProfileID.Value)).ToList();
                }
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                DataTable table = CreateDataTableReport(dateStart, dateEnd);
                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (!table.Columns.Contains("Data" + date.Day))
                        table.Columns.Add("Data" + date.Day, typeof(double));
                }
                profileIds = laundryRecords.Select(s => s.ProfileID.Value).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                foreach (var profile in profiles)
                {
                    DataRow row = table.NewRow();
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    Guid? orgId = profile.OrgStructureID;
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                    row[Lau_DataTableReportEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Lau_DataTableReportEntity.FieldNames.CodeEmp] = profile.CodeEmp != null ? profile.CodeEmp : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.CodeAttendance] = profile.CodeAttendance != null ? profile.CodeAttendance : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.ProfileName] = profile.ProfileName != null ? profile.ProfileName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                    row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;

                    var laury = laundryRecords.FirstOrDefault(s => s.ProfileID == profile.ID);
                    if (laury != null)
                    {
                        var locker = lockers.FirstOrDefault(s => s.ID == laury.LockerID);
                        var line = lines.FirstOrDefault(s => s.ID == laury.LineID);
                        row[Lau_DataTableReportEntity.FieldNames.LockerName] = locker != null ? locker.LockerLMSName : string.Empty;
                        row[Lau_DataTableReportEntity.FieldNames.LineName] = line != null ? line.LineLMSName : string.Empty;
                    }
                    row[Lau_DataTableReportEntity.FieldNames.SumMonthAmount] = laundryRecords.Count(s => s.ProfileID == profile.ID);

                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var laundryProfiles = laundryRecords.Where(s => s.ProfileID == profile.ID && s.TimeLog.Value.Date == date.Date).ToList();
                        if (laundryProfiles.Count > 0)
                        {
                            var count = laundryRecords.Count(s => s.ProfileID == profile.ID);
                            var standDard = workDays.Count(s => s.ProfileID == profile.ID);
                            row[Lau_DataTableReportEntity.FieldNames.AmountClothing] = standDard;
                            row[Lau_DataTableReportEntity.FieldNames.ExceedingStandards] = (count - standDard) > 0 ? (count - standDard) : 0;
                            row[Lau_DataTableReportEntity.FieldNames.SubtractSalary] = (count - standDard) > 0 ? (count - standDard) * 21000 : 0;

                            count = laundryProfiles.Count(s => s.TimeLog != null && s.TimeLog.Value.Date == date);
                            row["Data" + date.Day] = (double)(count);
                            row[Lau_DataTableReportEntity.FieldNames.Total] = (object)count ?? DBNull.Value;
                            row[Lau_DataTableReportEntity.FieldNames.TimeLog] = date;
                        }
                    }
                    table.Rows.Add(row);
                }
                return table;
            }
        }
        #endregion

        #region - Báo cáo tổng hợp trừ tiền giặt
        /// <summary>
        ///  [Tung.Ly 20140728]	Báo cáo tổng hợp trừ tiền giặt
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="lockerID"></param>
        /// <param name="lstProfileIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataTable GetReportSummaryExceptClothing(List<Guid?> lineIDs, List<Guid?> lockerIDs, List<Guid?> markerIDs, List<Guid> lstProfileIds, DateTime dateStart,
            DateTime dateEnd, List<Guid> shiftIDs, bool isIncludeQuitEmp, bool isViewOverProfile, string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            string status = string.Empty;
            string strIDs = string.Join(",", lstProfileIds.ToArray());
            DataTable datatable = CreateDataTableReport(dateStart, dateEnd);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_LaundryRecord = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                var repoWorkPlace = new CustomBaseRepository<Cat_WorkPlace>(unitOfWork);

                var laundryRecords = repoLau_LaundryRecord
                    .FindBy(s => s.ProfileID != null && dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                    .Select(s => new { s.ProfileID, s.TimeLog, s.LockerID, s.MarkerID, s.LineID, s.Amount, s.WorkDay })
                    .ToList();
                var profileIds = laundryRecords.Select(s => s.ProfileID.Value).Distinct().ToList();
                var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status)
                   .Where(s => lstProfileIds.Contains(s.ID))
                   .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.CodeAttendance, s.WorkPlaceID })
                   .ToList();
                var lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var markers = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();

                var workDays = repoAtt_Workday
                            .FindBy(s => (s.InTime1 != null || s.OutTime1 != null) && profileIds.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                            .Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID })
                            .ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                    profileIds = profiles.Select(s => s.ID).ToList();
                    laundryRecords = laundryRecords.Where(s => profileIds.Contains(s.ProfileID.Value)).ToList();
                }
                if (lockerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lockerIDs.Contains(m.LockerID)).ToList();
                }
                if (lineIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }
                if (markerIDs.Count != 0)
                {
                    laundryRecords = laundryRecords.Where(m => lineIDs.Contains(m.LineID)).ToList();
                }

                if (shiftIDs.Count != 0)
                {
                    var workDayProfileIDs = workDays.Where(s => dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.ShiftID != null && shiftIDs.Contains(s.ShiftID.Value))
                        .Select(s => s.ProfileID).ToList();
                    laundryRecords = laundryRecords.Where(s => workDayProfileIDs.Contains(s.ProfileID.Value)).ToList();
                }

                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var orgTypes = new List<Cat_OrgStructureType>();
                orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var lstWorkPlace = repoWorkPlace.FindBy(wp => wp.IsDelete == null).Select(wp => new {wp.ID, wp.PriceStandard }).ToList();

                DataTable table = CreateDataTableReport(dateStart, dateEnd);
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    if (!table.Columns.Contains("Data" + date.Day))
                    {
                        table.Columns.Add("Data" + date.Day, typeof(Double));
                    }
                }
                
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                foreach (var profile in profiles)
                {
                    bool isAdd = true;
                    if (isViewOverProfile)
                    {
                        isAdd = false;
                    }

                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    Guid? orgId = profile.OrgStructureID;

                    DataRow row = table.NewRow();
                    var laundryProfiles = laundryRecords.Where(s => s.ProfileID == profile.ID).OrderByDescending(lr => lr.WorkDay).ToList();
                    if (laundryProfiles.Count > 0)
                    {
                        var locker = lockers.FirstOrDefault(s => s.ID == laundryProfiles[0].LockerID);
                        var line = lines.FirstOrDefault(s => s.ID == laundryProfiles[0].LineID);
                        row[Lau_DataTableReportEntity.FieldNames.LockerName] = locker != null ? locker.LockerLMSName : string.Empty;
                        row[Lau_DataTableReportEntity.FieldNames.LineName] = line != null ? line.LineLMSName : string.Empty;
                        row[Lau_DataTableReportEntity.FieldNames.SumMonthAmount] = laundryRecords.Count(s => s.ProfileID == profile.ID);
                    }
                    var count = laundryProfiles.Count;
                    var standDard = workDays.Count(s => s.ProfileID == profile.ID);
                    row[Lau_DataTableReportEntity.FieldNames.StandardAmount] = standDard;
                    if ((count - standDard) > 0)
                    {
                        isAdd = true;
                        row[Lau_DataTableReportEntity.FieldNames.ExceedingStandards] = count - standDard;
                        var workPlace = lstWorkPlace.Where(wp => wp.ID == profile.WorkPlaceID).FirstOrDefault();
                        if (workPlace != null && workPlace.PriceStandard != null)
                        {
                            row[Lau_DataTableReportEntity.FieldNames.SubtractSalary] = (count - standDard) * workPlace.PriceStandard.Value;
                        }
                    }
                    else
                    {
                        row[Lau_DataTableReportEntity.FieldNames.ExceedingStandards] = 0;
                        row[Lau_DataTableReportEntity.FieldNames.SubtractSalary] = 0;
                    }
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                    row[Lau_DataTableReportEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Lau_DataTableReportEntity.FieldNames.CodeEmp] = profile.CodeEmp != null ? profile.CodeEmp : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.CodeAttendance] = profile.CodeAttendance != null ? profile.CodeAttendance : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.ProfileName] = profile.ProfileName != null ? profile.ProfileName : string.Empty;
                    row[Lau_DataTableReportEntity.FieldNames.DateFrom] = dateStart;
                    row[Lau_DataTableReportEntity.FieldNames.DateTo] = dateEnd;

                    for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                    {
                        row["Data" + date.Day] = laundryProfiles.Where(lr => lr.WorkDay.HasValue && lr.WorkDay.Value.Date == date.Date).Count();
                    }

                    if (isAdd)
                    {
                        table.Rows.Add(row);
                    }
                }
                return table;
            }
        } 
        #endregion

        #region tạo DataTable cho 
        DataTable CreateDataTableReport(DateTime dateStart, DateTime dateEnd)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.ProfileName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.CodeAttendance);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.LineName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.LockerName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.MarkerName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.LineID, typeof(System.Guid));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.LockerID, typeof(System.Guid));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.MarkerID, typeof(System.Guid));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.ShiftName);

            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.Price, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.TotalAmount, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.Volume, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.Total, typeof(System.Int32));

            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.BranchName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.TeamName);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.SectionName);

            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.BranchCode);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.DepartmentCode);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.TeamCode);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.SectionCode);
            
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.DateTo, typeof(DateTime));

            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.UserCreate);
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.DateCreate, typeof(DateTime));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.ID, typeof(System.Guid));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.TimeLog, typeof(DateTime));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.MachineCode);

            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.SumMonthAmount, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.AmountClothing, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.StandardAmount, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.ExceedingStandards, typeof(System.Int32));
            tb.Columns.Add(Lau_DataTableReportEntity.FieldNames.SubtractSalary, typeof(System.Double));
            return tb;
        }
        #endregion
    }
}
