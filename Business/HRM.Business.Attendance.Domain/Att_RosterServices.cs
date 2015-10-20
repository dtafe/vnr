using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using System;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using VnResource.Helper.Data;
using HRM.Business.Hr.Models;
using System.Collections;

namespace HRM.Business.Attendance.Domain
{
    public class Att_RosterServices : BaseService
    {
        public string SaveList(List<Att_RosterEntity> lstModel)
        {
            using (var context = new VnrHrmDataContext())
            {
                List<Att_Roster> lstSave = new List<Att_Roster>();
                lstSave = lstModel.Translate<Att_Roster>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<Att_Roster>(unitOfWork);
                //int count = 0;
                foreach (var item in lstSave)
                {
                    //count += 1;
                    item.ID = Guid.NewGuid();
                    repo.Add(item);
                }
                try
                {
                    repo.SaveChanges();
                    return "Success";
                }
                catch (Exception)
                {
                    return "Error";
                }
            }
        }
        /// <summary>
        /// chuyển thành trạng thái Cho Duyet
        /// </summary>
        /// <returns></returns>
        public void SubmitRoster(List<Guid> ids)
        {
            var lstWorkday = new List<Att_RosterEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_RosterRepository(unitOfWork);
                lstWorkday = repo.FindBy(m => ids.Contains(m.ID)).ToList().Translate<Att_RosterEntity>();
                foreach (var workday in lstWorkday)
                {
                    workday.Status = AttendanceDataStatus.E_SUBMIT.ToString();
                    repo.SaveChanges();
                }
            }
        }

        public List<Att_ProfileEntity> CheckRoster(DateTime? DateStart, DateTime? DateEnd, List<Hre_ProfileEntity> lstProfileIDsModel, bool isNotRoster, bool isDuplicateRoster, bool? isConstantRoster)
        {
            // List<Att_ProfileEntity> reportData = new List<Att_ProfileEntity>();
            List<Guid> lstProfileIDs = lstProfileIDsModel.Select(s => s.ID).ToList();
            string status = string.Empty;
            List<Att_ProfileEntity> reportData = new List<Att_ProfileEntity>();
            var _baseService = new BaseService();
            if (DateStart > DateEnd)
            {
                return reportData;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoLeaveDay = new Att_LeavedayRepository(unitOfWork);

                List<Att_RosterEntity> listWRt = new List<Att_RosterEntity>();
                string E_CANCEL = RosterStatus.E_CANCEL.ToString();
                string E_REJECTED = RosterStatus.E_REJECTED.ToString();

                var repoRoster = new Att_RosterRepository(unitOfWork);
                var lstRoster = repoRoster.FindBy(s => s.IsDelete == null && s.DateStart <= DateEnd && s.DateEnd >= DateStart && s.Status != E_CANCEL && s.Status != E_REJECTED).ToList();
                //var lstRoster = GetData<Att_RosterEntity>(lstObj, ConstantSql.hrm_att_sp_get_Roster,ref status).Where(
                //    s => s.StatusRoster != E_CANCEL && s.StatusRoster != E_REJECTED
                //    ).ToList();

                var profileIds = lstRoster.Where(s => lstProfileIDs.Contains(s.ProfileID)).Select(s => s.ProfileID).Distinct().ToList();
                string key = ProfileStatusSyn.E_WAITING.ToString();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = lstProfileIDsModel.Where(s => s.IsDelete == null && s.StatusSyn != key && (s.DateQuit == null || s.DateQuit > DateStart))
                    .Select(s => new { s.ID, s.DateQuit, s.OrgStructureName, s.E_UNIT, s.E_DIVISION, s.E_DEPARTMENT, s.E_TEAM, s.E_SECTION, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, s.Email, s.CodeAttendance, s.Gender, s.BusinessPhone, s.DateEndProbation, s.DateHire }).ToList();
                if (lstProfileIDs.Count != 0 && lstProfileIDs[0] != Guid.Empty)
                {
                    profiles = profiles.Where(s => lstProfileIDs.Contains(s.ID)).ToList();
                }
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                string E_CANCEL_LEAVE = LeaveDayStatus.E_CANCEL.ToString();
                string E_REJECTED_LEAVE = LeaveDayStatus.E_REJECTED.ToString();

                var lstLeaveDay = repoLeaveDay.FindBy(s => s.IsDelete == null && s.DateStart <= DateEnd && s.DateEnd >= DateStart && s.Status != E_CANCEL_LEAVE && s.Status != E_REJECTED_LEAVE).ToList();

                //var lstLeaveDay = GetData<Att_LeaveDayEntity>(lstObj1, ConstantSql.hrm_att_sp_get_Leaveday, ref status).Where(
                //    s => s.StatusLeaveDay != E_CANCEL_LEAVE && s.StatusLeaveDay != E_REJECTED_LEAVE).ToList();
                //List<Guid> guids = EntityService.GetEntityList<Att_Roster>(false, EntityService.GuidMainContext, Guid.Empty, s => s.DateStart <= DateEnd && s.DateEnd >= DateStart).Select(s => s.ProfileID).Distinct().ToList();
                List<Guid> guids = lstRoster.Select(s => s.ProfileID).Distinct().ToList();

                //    var listProfile = GetProfilesWorking(OrgStructureID, DateStart);
                if (isNotRoster == true)
                {
                    //List<Hre_Profile> profiles = listProfile.Where(s => !guids.Contains(s.ID)).ToList();

                    if (isConstantRoster == true)
                    {
                        foreach (var profile in profiles)
                        {

                            for (DateTime dateCheck = DateStart.Value; dateCheck <= DateEnd.Value; dateCheck = dateCheck.AddDays(1))
                            {
                                List<Guid> ListProfileRosterIds = lstRoster.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck).Select(m => m.ProfileID).Distinct().ToList();
                                List<Guid> ListProfileLeaveIds = lstLeaveDay.Where(m => m.DateStart <= dateCheck && m.DateEnd >= dateCheck).Select(m => m.ProfileID).Distinct().ToList();

                                var profileNotIn = profiles.Where(m => !ListProfileRosterIds.Contains(m.ID) && !ListProfileLeaveIds.Contains(m.ID)).ToList();
                                //reportData.AddRange(profileNotIn.Translate<Att_ProfileEntity>());
                                //foreach (var profileNotRoster in profileNotIn)
                                //{
                                if (profileNotIn != null && profileNotIn.Count > 0)
                                {
                                    Att_ProfileEntity profileResult = new Att_ProfileEntity();

                                    //Guid? orgId = profile.OrgStructureID;
                                    //var orgModel = orgs.Where(s => s.ID == orgId).FirstOrDefault();
                                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                                    profileResult.E_UNIT = profile.E_UNIT;
                                    profileResult.E_DIVISION = profile.E_DIVISION;
                                    profileResult.E_DEPARTMENT = profile.E_DEPARTMENT;
                                    profileResult.E_TEAM = profile.E_TEAM;
                                    profileResult.E_SECTION = profile.E_SECTION;

                                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                                    profileResult.ID = profile.ID;
                                    profileResult.CodeEmp = profile.CodeEmp;
                                    profileResult.ProfileName = profile.ProfileName;
                                    profileResult.CodeAttendance = profile.CodeAttendance;
                                    profileResult.DateHire = profile.DateHire;
                                    profileResult.PositionName = positon != null ? positon.PositionName : string.Empty;
                                    profileResult.JobTitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                                    profileResult.Email = profile.Email;
                                    profileResult.DateEndProbation = profile.DateEndProbation;
                                    profileResult.DateQuit = profile.DateQuit;
                                    if (profile.Gender == "E_FEMALE")
                                        profileResult.Gender = "Nam";
                                    if (profile.Gender == "E_MALE")
                                        profileResult.Gender = "Nữ";
                                    reportData.Add(profileResult);
                                }

                                //}

                                //profileResult.AddRange(profileNotIn);
                            }
                        }
                        reportData = reportData.Distinct().ToList();
                        return reportData;
                    }
                    else
                    {
                        var rs = profiles.Where(s => !guids.Contains(s.ID)).ToList().Translate<Att_ProfileEntity>();
                        foreach (var profile in rs)
                        {
                            Att_ProfileEntity profileResult = new Att_ProfileEntity();

                            //Guid? orgId = profile.OrgStructureID;
                            //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                            profileResult.E_UNIT = profile.E_UNIT;
                            profileResult.E_DIVISION = profile.E_DIVISION;
                            profileResult.E_DEPARTMENT = profile.E_DEPARTMENT;
                            profileResult.E_TEAM = profile.E_TEAM;
                            profileResult.E_SECTION = profile.E_SECTION;
                            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                            profileResult.ID = profile.ID;
                            profileResult.CodeEmp = profile.CodeEmp;
                            profileResult.ProfileName = profile.ProfileName;
                            profileResult.CodeAttendance = profile.CodeAttendance;
                            profileResult.DateHire = profile.DateHire;
                            profileResult.PositionName = positon != null ? positon.PositionName : string.Empty;
                            profileResult.JobTitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                            profileResult.Email = profile.Email;
                            profileResult.DateEndProbation = profile.DateEndProbation;
                            profileResult.DateQuit = profile.DateQuit;
                            if (profile.Gender == "E_FEMALE")
                                profileResult.Gender = "Nam";
                            if (profile.Gender == "E_MALE")
                                profileResult.Gender = "Nữ";
                            reportData.Add(profileResult);
                        }
                        return reportData;
                    }

                    reportData = profiles.Distinct().ToList().Translate<Att_ProfileEntity>();


                }
                // danh sach trung ca
                //    else if (isDuplicateRoster == true)
                //    {

                //        var rosters = GetData<Att_RosterEntity>(lstObj, ConstantSql.hrm_att_sp_get_Roster, ref status).ToList();
                //        foreach (var pf in listProfile)
                //        {
                //            // lay ca cua nhan vien
                //            var listWRosterPr = rosters.Where(rt => rt.ProfileID == pf.ID).ToList();
                //            if (listWRosterPr.Count > 0)
                //                for (DateTime dx = DateStart.Value.Date; dx <= DateEnd; dx = dx.AddDays(1))
                //                {
                //                    var listWRoster = listWRosterPr.Where(s => s.ProfileID == pf.ID
                //                                                         && s.DateStart <= dx
                //                                                         && dx <= s.DateEnd
                //                                                         && s.Type == RosterType.E_DEFAULT.ToString()
                //                                                         && s.Status != RosterStatus.E_REJECTED.ToString()
                //                                                         && s.Status != RosterStatus.E_CANCEL.ToString()
                //                                                         && s.IsDelete == null).ToList();
                //                    if (listWRoster.Count >= 2)
                //                        listWRt.AddRange(listWRoster);
                //                }

                //        }

                //        var result = listWRt.OrderBy(s => s.CodeEmp).ToList();
                //        // return result;
                //    }

                //}


                return reportData;
            }
        }

        public List<Att_ProfileEntity> GetProfilesWorking(string guidOrg, DateTime? dateCheck, string userLogin)
        {

            string status = String.Empty;
            string key = ProfileStatusSyn.E_WAITING.ToString();
            List<object> lstObj = new List<object>();
            lstObj.Add(guidOrg);
            lstObj.Add(null);
            lstObj.Add(null);

            var profiles = GetData<Att_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Where(s => s.StatusSyn != key && (s.DateQuit == null || s.DateQuit >= dateCheck)).ToList();
            if (!string.IsNullOrEmpty(guidOrg))
            {
                List<Guid> guids = GetChildIDs(guidOrg, userLogin);
                if (guids.Count > 0)
                    profiles = profiles.Where(s => s.OrgStructureID != null && guids.Contains(s.OrgStructureID.Value)).ToList();
            }

            return profiles;
        }


        public List<Guid> GetChildIDs(string parentOrgId, string userLogin)
        {
            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(null);
            lstObj.Add(null);
            lstObj.Add(null);
            List<Cat_OrgStructure> lstAll = GetData<Cat_OrgStructure>(lstObj, ConstantSql.hrm_cat_sp_get_AllOrgStructure, userLogin, ref status);

            if (parentOrgId == string.Empty)
            {

                return lstAll.Select(s => s.ID).ToList();

            }
            List<Cat_OrgStructure> list = GetChilds(lstAll, Common.ConvertToGuid(parentOrgId));
            if (list.Count > 0)
                return list.Select(s => s.ID).ToList();
            return new List<Guid>();
        }

        public List<Cat_OrgStructure> GetChilds(List<Cat_OrgStructure> lstAll, Guid parentOrgId)
        {
            List<Cat_OrgStructure> res = new List<Cat_OrgStructure>();
            Cat_OrgStructure parentOrg = lstAll.Where(org => org.ID == parentOrgId).FirstOrDefault();

            res.Add(parentOrg);

            //Dieu kien dung cua de quy: khong con child

            List<Cat_OrgStructure> lstChild = lstAll.Where(prop => prop.ParentID == parentOrgId).ToList();

            if (lstChild.Count > 0)
            {
                foreach (Cat_OrgStructure orgStrucChild in lstChild)
                {
                    res.AddRange(GetChilds(lstAll, orgStrucChild.ID));
                }
            }

            return res;
        }
        public static void GetRosterGroup(List<Guid> lstProfileID, DateTime? DateFrom, DateTime? DateTo, out List<Att_Roster> lstRosterTypeGroup, out List<Att_RosterGroup> lstRosterGroup)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoAtt_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);

                lstRosterGroup = new List<Att_RosterGroup>();
                lstRosterTypeGroup = new List<Att_Roster>();

                string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();

                if (DateFrom == null || DateTo == null)
                {
                    lstRosterTypeGroup = repoAtt_Roster.GetAll().Where(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.GetAll().Where(m => m.DateStart != null && m.DateEnd != null).ToList<Att_RosterGroup>();
                }
                else
                {
                    lstRosterTypeGroup = repoAtt_Roster.GetAll().Where(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= DateTo).ToList<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.GetAll().Where(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= DateTo && m.DateEnd >= DateFrom).ToList<Att_RosterGroup>();
                }
            }
        }

        #region GetDailyShifts

        /// <summary>
        /// Kiểm tra ca làm việc hàng ngày của nhân viên.
        /// </summary>
        /// <param name="profile">Nhân viên cần kiểm tra</param>
        /// <param name="gradeCfg">Cấu hình của chế độ lương</param>
        /// <param name="listRoster">Lịch làm việc của nhân viên</param>
        /// <param name="listWorkHistory">Quá trình công tác</param>
        /// <param name="dateFrom">Khoảng thời gian cần kiểm tra</param>
        /// <param name="dateTo">Khoảng thời gian cần kiểm tra</param>
        /// <returns></returns>
        public static Dictionary<DateTime, Cat_Shift> GetDailyShifts(Guid profileID, Cat_GradeAttendance gradeCfg,
            List<Att_Roster> listRoster, List<Hre_WorkHistory> listWorkHistory, DateTime dateFrom, DateTime dateTo
            , List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {
            Dictionary<DateTime, Cat_Shift> listShift = new Dictionary<DateTime, Cat_Shift>();

            if (gradeCfg != null)
            {
                #region Ưu tiên ca theo roster cá nhân trước

                if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER.ToString())
                {
                    GetDailyShifts(profileID, listRoster, dateFrom, dateTo, ref listShift, lstRosterGroup, lstRosterTypeGroup);
                }

                #endregion

                #region Tiếp theo ưu tiên theo lịch sử công tác

                if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
                {
                    if (profileID != Guid.Empty && listWorkHistory != null)
                    {
                        GetDailyShifts(profileID, listWorkHistory, dateFrom, dateTo, ref listShift);
                    }
                }

                #endregion

                #region Ca cấu hình mặc định được ưu tiên sau cùng

                if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    GetDailyShifts(gradeCfg, dateFrom, dateTo, ref listShift);
                }

                #endregion
            }

            //Loại bỏ những kết quả có shift bị null => do bị rơi vào trường hợp time-off
            return listShift.Where(d => d.Value != null).ToDictionary(d => d.Key, d => d.Value);
        }

        /// <summary>
        /// Kiểm tra ca làm việc hàng ngày của nhân viên theo roster.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="listRoster"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="listShift"></param>
        private static void GetDailyShifts(Guid profileID, List<Att_Roster> listRoster,
            DateTime dateFrom, DateTime dateTo, ref Dictionary<DateTime, Cat_Shift> listShift, List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {
            #region Personal roster

            if (profileID != Guid.Empty && listRoster != null && listRoster.Count() > 0)
            {
                listRoster = listRoster.Where(d => d != null && d.ProfileID == profileID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                foreach (Att_Roster roster in listRoster)
                {
                    DateTime dateStart = dateFrom;
                    DateTime dateEnd = dateTo;

                    if (roster.DateStart != null && roster.DateStart > dateFrom)
                    {
                        dateStart = roster.DateStart;
                    }

                    if (roster.DateEnd != null && roster.DateEnd < dateTo)
                    {
                        dateEnd = roster.DateEnd;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        if (roster.Type == RosterType.E_TIME_OFF.ToString())
                        {
                            if (listShift.ContainsKey(date))
                            {
                                //Ngày nghỉ không dùng
                                listShift[date] = null;
                            }
                            else
                            {
                                listShift.Add(date, null);
                            }
                        }
                        else if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                        {
                            if (listShift.ContainsKey(date))
                            {
                                //Null khi time-off đã có trước
                                if (listShift[date] != null)
                                {
                                    //Ưu tiên ca làm việc được thay đổi
                                    listShift[date] = roster.Cat_Shift;
                                }
                            }
                            else
                            {
                                listShift.Add(date, roster.Cat_Shift);
                            }
                        }

                        else if (roster.Type == RosterType.E_DEFAULT.ToString())
                        {
                            if (!listShift.ContainsKey(date))
                            {
                                if (date.DayOfWeek == DayOfWeek.Monday && roster.Cat_Shift != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Tuesday && roster.Cat_Shift1 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift1);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday && roster.Cat_Shift2 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift2);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday && roster.Cat_Shift3 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift3);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday && roster.Cat_Shift4 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift4);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday && roster.Cat_Shift5 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift5);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday && roster.Cat_Shift6 != null)
                                {
                                    listShift.Add(date, roster.Cat_Shift6);
                                }
                            }
                        }
                    }
                }
            }

            if (profileID != Guid.Empty && lstRosterTypeGroup != null && lstRosterTypeGroup.Count() > 0)
            {
                #region triet.mai Loai E_ROSTERGROUP loại Đăng ký tăng ca theo nhóm
                //Logic khá phức tạp 
                //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRosterTypeGroup_ByProfile = lstRosterTypeGroup.Where(m => m.ProfileID == profileID).ToList();
                List<Att_Roster> lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.Where(m => m.Type == E_ROSTERGROUP).OrderByDescending(m => m.DateStart).ToList();
                bool isContinue = true; //Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;

                foreach (Att_Roster roster in lstRoster_Type_RosterGroup)
                {
                    if (!isContinue)
                    {
                        continue;
                    }

                    if (roster.DateStart <= dateFrom)
                    {
                        isContinue = false;
                    }

                    RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), roster.Type);
                    DateTime start = dateFrom < roster.DateStart ? roster.DateStart : dateFrom;
                    DateTime end = dateTo;

                    switch (type)
                    {
                        case RosterType.E_ROSTERGROUP:
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                if (!listShift.ContainsKey(date))
                                {
                                    Att_RosterGroup RosterGroup_Current = lstRosterGroup.Where(m => m.RosterGroupName == roster.RosterGroupName && m.DateStart <= date && m.DateEnd >= date).FirstOrDefault();
                                    if (RosterGroup_Current != null)
                                    {
                                        if (date.DayOfWeek == DayOfWeek.Monday && RosterGroup_Current.Cat_Shift != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Tuesday && RosterGroup_Current.Cat_Shift1 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift1);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Wednesday && RosterGroup_Current.Cat_Shift2 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift2);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Thursday && RosterGroup_Current.Cat_Shift3 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift3);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Friday && RosterGroup_Current.Cat_Shift4 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift4);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Saturday && RosterGroup_Current.Cat_Shift5 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift5);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Sunday && RosterGroup_Current.Cat_Shift6 != null)
                                        {
                                            listShift.Add(date, RosterGroup_Current.Cat_Shift6);
                                        }
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                #endregion
            }

            #endregion
        }

        /// <summary>
        /// Kiểm tra ca làm việc hàng ngày của nhân viên theo phòng ban và quá trình công tác.
        /// </summary>
        /// <param name="profile">Nhân viên cần kiểm tra</param>
        /// <param name="listWorkHistory">Quá trình công tác</param>
        /// <param name="dateFrom">Khoảng thời gian cần kiểm tra</param>
        /// <param name="dateTo">Khoảng thời gian cần kiểm tra</param>
        /// <returns></returns>
        private static void GetDailyShifts(Guid profileID, List<Hre_WorkHistory> listWorkHistory,
            DateTime dateFrom, DateTime dateTo, ref Dictionary<DateTime, Cat_Shift> listShift)
        {
            if (profileID != Guid.Empty)
            {
                #region WorkHistory roster

                for (DateTime date = dateFrom.Date; date <= dateTo; date = date.AddDays(1))
                {
                    if (!listShift.ContainsKey(date))
                    {
                        List<Hre_WorkHistory> listWorkdate = listWorkHistory.Where(d => d.ProfileID == profileID
                            && d.DateEffective <= date).OrderByDescending(d => d.DateEffective).ToList();

                        Cat_OrgStructure orgStructure = null;

                        if (listWorkdate != null && listWorkdate.Count > 0)
                        {
                            Hre_WorkHistory workHistor = listWorkdate[0];
                            orgStructure = workHistor.Cat_OrgStructure;
                        }

                        if (orgStructure != null)
                        {
                            if (date.DayOfWeek == DayOfWeek.Monday
                                && orgStructure.Cat_Shift != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Tuesday
                                && orgStructure.Cat_Shift1 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift1);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Wednesday
                                && orgStructure.Cat_Shift2 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift2);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Thursday
                                && orgStructure.Cat_Shift3 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift3);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Friday
                                && orgStructure.Cat_Shift4 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift4);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Saturday
                                && orgStructure.Cat_Shift5 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift5);
                            }
                            else if (date.DayOfWeek == DayOfWeek.Sunday
                                && orgStructure.Cat_Shift6 != null)
                            {
                                listShift.Add(date, orgStructure.Cat_Shift6);
                            }
                        }
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Kiểm tra ca làm việc hàng ngày của nhân viên theo cấu hình grade.
        /// </summary>
        /// <param name="gradeCfg"></param>
        /// <param name="dateFrom">Khoảng thời gian cần kiểm tra</param>
        /// <param name="dateTo">Khoảng thời gian cần kiểm tra</param>
        /// <returns></returns>
        private static void GetDailyShifts(Cat_GradeAttendance gradeCfg, DateTime dateFrom,
            DateTime dateTo, ref Dictionary<DateTime, Cat_Shift> listShift)
        {
            if (gradeCfg != null)
            {
                #region Grade default roster

                // Bản 8 ko xài hàm này
                //for (DateTime date = dateFrom.Date; date <= dateTo; date = date.AddDays(1))
                //{
                //    if (!listShift.ContainsKey(date))
                //    {
                //        if (date.DayOfWeek == DayOfWeek.Monday
                //            && gradeCfg.Cat_Shift != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Tuesday
                //            && gradeCfg.Cat_Shift1 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift1);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Wednesday
                //            && gradeCfg.Cat_Shift2 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift2);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Thursday
                //            && gradeCfg.Cat_Shift3 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift3);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Friday
                //            && gradeCfg.Cat_Shift4 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift4);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Saturday
                //            && gradeCfg.Cat_Shift5 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift5);
                //        }
                //        else if (date.DayOfWeek == DayOfWeek.Sunday
                //            && gradeCfg.Cat_Shift6 != null)
                //        {
                //            listShift.Add(date, gradeCfg.Cat_Shift6);
                //        }
                //    }
                //}

                #endregion
            }
        }

        #endregion

        #region Hỗ trợ Tạo mới Phân Tcih1 Tăng Ca
        public static Hashtable GetRosterTable(bool usingSecurity, Hre_Profile profile
                                                , DateTime from, DateTime to, List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {
            using (var contextRosterTable = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(contextRosterTable));
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);

                Att_Grade grade = repoAtt_Grade
                    .FindBy(grd => grd.IsDelete == null && grd.ProfileID == profile.ID).FirstOrDefault();
                Cat_GradeAttendance gradeCfg = grade != null ? grade.Cat_GradeAttendance : null;
                List<Hre_WorkHistory> whProfile = repoHre_WorkHistory.FindBy(wh => wh.ProfileID == profile.ID).ToList();
                List<Att_Roster> rosterCfg = List(usingSecurity, profile, from.Date, to.Date);
                return GetRosterTable(profile, rosterCfg, whProfile, gradeCfg, from.Date, to.Date, lstRosterGroup, lstRosterTypeGroup);
            }
        }

        /// <summary>
        /// Lay table roster Key: Date, Value : Shift
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="rosterCfgPro"></param>
        /// <param name="lstHistoryPro"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lstRosterGroup">Ds RosterGroup</param>
        /// <param name="lstRosterTypeGroup">Ds Roster loai E_ROSTERGROUP</param>
        /// <returns></returns>
        public static Hashtable GetRosterTable(Hre_Profile profile
                                , List<Att_Roster> rosterCfgPro
                                , List<Hre_WorkHistory> lstHistoryPro
                                , Cat_GradeAttendance gradeCfg
                                , DateTime from, DateTime to, List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {

            rosterCfgPro = rosterCfgPro.Where(s => s.ProfileID == profile.ID).ToList();
            Hashtable res = new Hashtable();
            from = from.Date;
            to = to.Date;
            //LamLe : 20120809 - #0014556 - Chi lay nhung roster o trang thai Approved
            String statusApproved = RosterStatus.E_APPROVED.ToString();
            List<Att_Roster> rosterApproved = rosterCfgPro.Where(ros => ros.Status == statusApproved || String.IsNullOrEmpty(ros.Status)).ToList();
            List<Att_Roster> lstRosterTypeGroup_ByProfile = lstRosterTypeGroup.Where(m => m.ProfileID == profile.ID).ToList();


            //LamLe - 20120803 - Lay ca lam viec trong phong ban.
            if (lstHistoryPro != null && gradeCfg != null && gradeCfg.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
            {
                res = GetRosterOrgTable(profile, lstHistoryPro, from, to);
            }
            if (rosterApproved != null) //LamLe - 20120803 - Lay ca lam viec dua vao roster
            {
                foreach (Att_Roster roster in rosterApproved)
                {
                    try
                    {
                        if (RosterType.E_DEFAULT.ToString() != roster.Type)
                            continue;

                        DateTime start = from;
                        if (roster.DateStart != null && roster.DateStart > start)
                            start = roster.DateStart;

                        DateTime end = to;
                        if (roster.DateEnd != null && roster.DateEnd < end)
                            end = roster.DateEnd;

                        for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                        {
                            ArrayList arr = new ArrayList();
                            if (idx.DayOfWeek == DayOfWeek.Monday)
                                arr.Add(roster.Cat_Shift);

                            else if (idx.DayOfWeek == DayOfWeek.Tuesday)
                                arr.Add(roster.Cat_Shift1);

                            else if (idx.DayOfWeek == DayOfWeek.Wednesday)
                                arr.Add(roster.Cat_Shift2);

                            else if (idx.DayOfWeek == DayOfWeek.Thursday)
                                arr.Add(roster.Cat_Shift3);

                            else if (idx.DayOfWeek == DayOfWeek.Friday)
                                arr.Add(roster.Cat_Shift4);

                            else if (idx.DayOfWeek == DayOfWeek.Saturday)
                                arr.Add(roster.Cat_Shift5);

                            else if (idx.DayOfWeek == DayOfWeek.Sunday)
                                arr.Add(roster.Cat_Shift6);
                            if (!res.ContainsKey(idx))
                                res.Add(idx, arr);

                        }
                    }
                    catch (System.Exception e)
                    {

                    }
                }

                #region triet.mai Loai E_ROSTERGROUP loại Đăng ký tăng ca theo nhóm
                //Logic khá phức tạp 
                //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.Where(m => m.Type == E_ROSTERGROUP).OrderByDescending(m => m.DateStart).ToList();
                bool isContinue = true; //Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;
                foreach (Att_Roster rosterGroup in lstRoster_Type_RosterGroup)
                {
                    if (isContinue == false)
                        continue;

                    if (rosterGroup.DateStart <= from)
                    {
                        isContinue = false;
                    }

                    DateTime start = from;
                    DateTime end = to;
                    RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), rosterGroup.Type);
                    switch (type)
                    {
                        case RosterType.E_ROSTERGROUP:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                bool isExist = res.ContainsKey(idx);
                                ArrayList arr = new ArrayList();
                                if (isExist)
                                    res.Remove(idx);

                                if (string.IsNullOrEmpty(rosterGroup.RosterGroupName))
                                    continue;
                                Att_RosterGroup RosterGroup_Current = lstRosterGroup.Where(m => m.RosterGroupName == rosterGroup.RosterGroupName && m.DateStart <= idx && m.DateEnd >= idx).FirstOrDefault();
                                if (RosterGroup_Current == null)
                                    continue;

                                if (idx.DayOfWeek == DayOfWeek.Monday && RosterGroup_Current.Cat_Shift != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift);

                                else if (idx.DayOfWeek == DayOfWeek.Tuesday && RosterGroup_Current.Cat_Shift1 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift1);

                                else if (idx.DayOfWeek == DayOfWeek.Wednesday && RosterGroup_Current.Cat_Shift2 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift2);

                                else if (idx.DayOfWeek == DayOfWeek.Thursday && RosterGroup_Current.Cat_Shift3 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift3);

                                else if (idx.DayOfWeek == DayOfWeek.Friday && RosterGroup_Current.Cat_Shift4 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift4);

                                else if (idx.DayOfWeek == DayOfWeek.Saturday && RosterGroup_Current.Cat_Shift5 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift5);

                                else if (idx.DayOfWeek == DayOfWeek.Sunday && RosterGroup_Current.Cat_Shift6 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift6);

                                if (!res.ContainsKey(idx))
                                    res.Add(idx, arr);
                            }
                            break;

                        default:
                            break;
                    }
                }
                #endregion



                //LamLe - 20121101 - Order theo loai de E_TIME_OFF uu tien sau cung
                rosterApproved = rosterApproved.OrderBy(rs => rs.Type).ToList();

                string E_TIME_OFF = RosterType.E_TIME_OFF.ToString();
                string E_CHANGE_SHIFT = RosterType.E_CHANGE_SHIFT.ToString();
                List<Att_Roster> lstRoster_Type_TimeOff_ChangeShift = rosterApproved.Where(m => m.Type == E_TIME_OFF || m.Type == E_CHANGE_SHIFT).ToList();
                foreach (Att_Roster roster in lstRoster_Type_TimeOff_ChangeShift)
                {
                    DateTime start = from;
                    if (roster.DateStart != null)
                        start = roster.DateStart;

                    DateTime end = to;
                    if (roster.DateEnd != null)
                        end = roster.DateEnd;

                    RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), roster.Type);
                    switch (type)
                    {
                        case RosterType.E_TIME_OFF:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                if (res.ContainsKey(idx))
                                    res.Remove(idx);
                            }
                            break;

                        case RosterType.E_CHANGE_SHIFT:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                bool isExist = res.ContainsKey(idx);
                                ArrayList arr = new ArrayList();

                                if (isExist)
                                    res.Remove(idx);
                                arr.Add(roster.Cat_Shift);

                                res.Add(idx, arr);
                            }
                            break;


                        //case RosterType.E_UNUSUAL:
                        //    for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                        //    {
                        //        bool isExist = res.ContainsKey(idx);
                        //        if (!isExist)
                        //            res.Add(idx, new ArrayList());

                        //        ((ArrayList)res[idx]).Add(roster.Cat_Shift);
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
            }
            return res;
        }
        public static List<Att_Roster> List(bool usingSecurity, Hre_Profile profile, DateTime from, DateTime to)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                //LamLe - 20120616 - task SONNGO chua assign - Sua lai dk giao ngay thang
                return repoAtt_Roster.FindBy(roster => roster.ProfileID == profile.ID
                                  && ((roster.DateStart <= to && roster.DateEnd >= from))
                                ).OrderBy(roster => roster.DateCreate).ToList();
            }
        }

        /// <summary>
        /// LamLe - 20120803 - Lay ca lam viec dua vao phong ban.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="lstHistory"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Hashtable GetRosterOrgTable(Hre_Profile profile
                                , List<Hre_WorkHistory> lstHistory
                                , DateTime from, DateTime to)
        {
            Hashtable res = new Hashtable();
            for (DateTime idx = from; idx <= to; idx = idx.AddDays(1))
            {
                List<Hre_WorkHistory> listWorkdate = lstHistory.Where(wh => wh.DateEffective <= idx).OrderByDescending(wh => wh.DateEffective).ToList();
                Cat_OrgStructure org = null;
                if (listWorkdate != null && listWorkdate.Count > 0)
                {
                    Hre_WorkHistory whDate = listWorkdate[0];
                    org = whDate.Cat_OrgStructure;
                }
                else if (profile.Cat_OrgStructure != null)
                {
                    org = profile.Cat_OrgStructure;
                }
                if (org == null)
                    continue;

                ArrayList arr = new ArrayList();
                if (idx.DayOfWeek == DayOfWeek.Monday)
                    arr.Add(org.Cat_Shift);

                else if (idx.DayOfWeek == DayOfWeek.Tuesday)
                    arr.Add(org.Cat_Shift1);

                else if (idx.DayOfWeek == DayOfWeek.Wednesday)
                    arr.Add(org.Cat_Shift2);

                else if (idx.DayOfWeek == DayOfWeek.Thursday)
                    arr.Add(org.Cat_Shift3);

                else if (idx.DayOfWeek == DayOfWeek.Friday)
                    arr.Add(org.Cat_Shift4);

                else if (idx.DayOfWeek == DayOfWeek.Saturday)
                    arr.Add(org.Cat_Shift5);

                else if (idx.DayOfWeek == DayOfWeek.Sunday)
                    arr.Add(org.Cat_Shift6);

                if (!res.ContainsKey(idx))
                    res.Add(idx, arr);
            }
            return res;
        }


        public static bool IsWorkDay(DateTime date, Hashtable rosterTable)
        {
            bool res = false;

            if (rosterTable != null && rosterTable.ContainsKey(date.Date))
            {
                if (((ArrayList)rosterTable[date.Date])[0] != null)
                    res = true;
            }

            return res;
        }

        public static Cat_Shift GetShift(DateTime date, Hashtable rosterTable)
        {
            try
            {
                if (rosterTable.ContainsKey(date.Date))
                    return ((ArrayList)rosterTable[date])[0] as Cat_Shift;
            }
            catch (System.Exception)
            { }

            return null;
        }
        #endregion

        #region validate
        /// <summary>
        /// hàm kiểm tra nhân viên nào có bị vấn đề ca liên tục hay ko. Giới hạn là trên 12h. Ps: hàm chưa xử lý được vấn đề RosterGroup
        /// </summary>
        /// <param name="GuidContext"></param>
        /// <param name="lstRoster"></param>
        /// <returns>Ds ProfileID nhân viên không hợp lệ</returns>
        public string ValidateShiftHourContinue(List<Att_RosterEntity> lstRoster)
        {
            if (lstRoster.Count == 0)
            {
                return string.Empty;
            }
            string Error = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                #region getData
                double NumMinHour = 12;
                List<Guid> lstProfileID = lstRoster.Select(m => m.ProfileID).Distinct().ToList();
                DateTime MinRoster = lstRoster.Min(m => m.DateStart);
                DateTime MaxRoster = lstRoster.Max(m => m.DateEnd);
                DateTime Bef1Day = MinRoster.AddDays(-1);
                DateTime Aft1Day = MaxRoster.AddDays(1);
                string E_CANCEL = RosterStatus.E_CANCEL.ToString();
                string E_REJECTED = RosterStatus.E_REJECTED.ToString();
                List<Guid> lstRosterIDs = lstRoster.Select(m => m.ID).ToList();
                var lstRosterBef1Day = unitOfWork.CreateQueryable<Att_Roster>(m => m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= Bef1Day && m.DateEnd >= Bef1Day && lstProfileID.Contains(m.ProfileID) && !lstRosterIDs.Contains(m.ID)).Select(m => new { m.ID, m.ProfileID, m.Type, m.DateStart, m.DateEnd, m.MonShiftID, m.TueShiftID, m.WedShiftID, m.ThuShiftID, m.FriShiftID, m.SatShiftID, m.SunShiftID }).ToList();
                var lstRosterAft1Day = unitOfWork.CreateQueryable<Att_Roster>(m => m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= Aft1Day && m.DateEnd >= Aft1Day && lstProfileID.Contains(m.ProfileID) && !lstRosterIDs.Contains(m.ID)).Select(m => new { m.ID, m.ProfileID, m.Type, m.DateStart, m.DateEnd, m.MonShiftID, m.TueShiftID, m.WedShiftID, m.ThuShiftID, m.FriShiftID, m.SatShiftID, m.SunShiftID }).ToList();
                var lstShift = unitOfWork.CreateQueryable<Cat_Shift>().Select(m => new { m.ID, m.InTime, m.CoOut }).ToList();
                var lstProfile = unitOfWork.CreateQueryable<Hre_Profile>(m => lstProfileID.Contains(m.ID)).Select(m => new { m.ID, m.ProfileName, m.CodeEmp }).ToList();
                #endregion
                foreach (var ProfileID in lstProfileID)
                {

                    Att_RosterEntity RosterbyProfile = lstRoster.Where(m => m.ProfileID == ProfileID).FirstOrDefault();
                    if (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString())
                    {
                        RosterbyProfile.TueShiftID = RosterbyProfile.WedShiftID = RosterbyProfile.ThuShiftID = RosterbyProfile.FriShiftID = RosterbyProfile.SatShiftID = RosterbyProfile.SunShiftID = RosterbyProfile.MonShiftID;
                    }
                    DateTime DateMin = RosterbyProfile.DateStart;
                    DateTime DateMax = RosterbyProfile.DateEnd;
                    DateTime DateBefore = DateMin.AddDays(-1);
                    DateTime DateAfter = DateMax.AddDays(1);
                    var RosterBef1DayByProfile = lstRosterBef1Day.Where(m => m.Type == RosterType.E_CHANGE_SHIFT.ToString() && m.ProfileID == ProfileID && m.DateStart <= DateBefore && m.DateEnd >= DateBefore && m.ID != RosterbyProfile.ID).FirstOrDefault();
                    if (RosterBef1DayByProfile == null)
                    {
                        RosterBef1DayByProfile = lstRosterBef1Day.Where(m => m.ProfileID == ProfileID && m.DateStart <= DateBefore && m.DateEnd >= DateBefore && m.ID != RosterbyProfile.ID).FirstOrDefault();
                    }
                    var RosterAft1DayByProfile = lstRosterAft1Day.Where(m => m.Type == RosterType.E_CHANGE_SHIFT.ToString() && m.ProfileID == ProfileID && m.DateStart <= DateAfter && m.DateEnd >= DateAfter && m.ID != RosterbyProfile.ID).FirstOrDefault();
                    if (RosterAft1DayByProfile == null)
                    {
                        RosterAft1DayByProfile = lstRosterAft1Day.Where(m => m.ProfileID == ProfileID && m.DateStart <= DateAfter && m.DateEnd >= DateAfter && m.ID != RosterbyProfile.ID).FirstOrDefault();
                    }
                    #region so sánh trước 1 ngày vs cái roster Đang xét
                    if (RosterBef1DayByProfile != null)
                    {
                        Guid ShiftRosterMin = Guid.Empty;
                        Guid ShiftRosterBef1 = Guid.Empty;
                        switch (DateMin.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.MonShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.SunShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Tuesday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.TueShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.MonShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Wednesday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.WedShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.TueShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Thursday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.ThuShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.WedShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Friday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.FriShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.ThuShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Saturday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.SatShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.FriShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Sunday:
                                ShiftRosterMin = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.SunShiftID ?? Guid.Empty;
                                ShiftRosterBef1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.SatShiftID ?? Guid.Empty;
                                break;
                        }
                        if (ShiftRosterMin != Guid.Empty && ShiftRosterBef1 != Guid.Empty)
                        {
                            var shiftMin = lstShift.Where(m => m.ID == ShiftRosterMin).FirstOrDefault();
                            var shiftBef = lstShift.Where(m => m.ID == ShiftRosterBef1).FirstOrDefault();
                            if (shiftMin != null && shiftBef != null)
                            {
                                DateTime beginShiftMin = DateMin.Date.AddHours(shiftMin.InTime.Hour).AddMinutes(shiftMin.InTime.Minute);
                                DateTime beginShiftBef = DateBefore.Date.AddHours(shiftBef.InTime.Hour).AddMinutes(shiftBef.InTime.Minute);
                                DateTime endShiftBef = beginShiftBef.AddHours(shiftBef.CoOut);
                                if (endShiftBef.AddHours(NumMinHour) > beginShiftMin)
                                {
                                    var profile = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
                                    if (profile != null)
                                    {
                                        Error += profile.ProfileName + " [" + profile.CodeEmp + "]; ";
                                    }

                                }
                            }
                        }
                    }
                    #endregion
                    #region so sánh sau 1 ngày vs cái roster đang xét
                    if (RosterAft1DayByProfile != null)
                    {
                        Guid ShiftRosterMax = Guid.Empty; //Của Roster Đang xét
                        Guid ShiftRosterAft1 = Guid.Empty; // Của Roster Trong DB
                        switch (DateMax.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.MonShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.ThuShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Tuesday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.TueShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.WedShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Wednesday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.WedShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.ThuShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Thursday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.ThuShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.FriShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Friday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.FriShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.SatShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Saturday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.SatShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.SunShiftID ?? Guid.Empty;
                                }
                                break;
                            case DayOfWeek.Sunday:
                                ShiftRosterMax = (RosterbyProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterbyProfile.MonShiftID ?? Guid.Empty : RosterbyProfile.SunShiftID ?? Guid.Empty;
                                if (RosterBef1DayByProfile == null)
                                {
                                    ShiftRosterAft1 = Guid.Empty;
                                }
                                else
                                {
                                    ShiftRosterAft1 = (RosterBef1DayByProfile.Type == RosterType.E_CHANGE_SHIFT.ToString()) ? RosterBef1DayByProfile.MonShiftID ?? Guid.Empty : RosterBef1DayByProfile.MonShiftID ?? Guid.Empty;
                                }
                                break;
                        }
                        if (ShiftRosterMax != Guid.Empty && ShiftRosterAft1 != Guid.Empty)
                        {
                            var shiftMax = lstShift.Where(m => m.ID == ShiftRosterMax).FirstOrDefault();
                            var shiftAfer = lstShift.Where(m => m.ID == ShiftRosterAft1).FirstOrDefault();
                            if (shiftMax != null && shiftAfer != null)
                            {
                                DateTime beginShiftMax = DateMax.Date.AddHours(shiftMax.InTime.Hour).AddMinutes(shiftMax.InTime.Minute);
                                DateTime endShiftMax = beginShiftMax.AddHours(shiftMax.CoOut);
                                DateTime beginShiftAft = DateAfter.Date.AddHours(shiftAfer.InTime.Hour).AddMinutes(shiftAfer.InTime.Minute);
                                if (endShiftMax.AddHours(NumMinHour) > beginShiftAft)
                                {
                                    var profile = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
                                    if (profile != null)
                                    {
                                        Error += profile.ProfileName + " [" + profile.CodeEmp + "]; ";
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    #region so sánh từng ngày liên tuc cho đến 7 ngày trong cái roster
                    for (int i = 0; i < 7; i++)
                    {
                        Guid ShiftCurrent = Guid.Empty;
                        Guid ShiftCurrentAfter = Guid.Empty;
                        DateTime DateCurrent = DateMin.AddDays(i);
                        switch (DateCurrent.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                ShiftCurrent = RosterbyProfile.MonShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.TueShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Tuesday:
                                ShiftCurrent = RosterbyProfile.TueShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.WedShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Wednesday:
                                ShiftCurrent = RosterbyProfile.WedShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.ThuShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Thursday:
                                ShiftCurrent = RosterbyProfile.ThuShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.FriShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Friday:
                                ShiftCurrent = RosterbyProfile.FriShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.SatShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Saturday:
                                ShiftCurrent = RosterbyProfile.SatShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.SunShiftID ?? Guid.Empty;
                                break;
                            case DayOfWeek.Sunday:
                                ShiftCurrent = RosterbyProfile.SunShiftID ?? Guid.Empty;
                                ShiftCurrentAfter = RosterbyProfile.MonShiftID ?? Guid.Empty;
                                break;
                        }
                        if (ShiftCurrent != Guid.Empty && ShiftCurrentAfter != Guid.Empty)
                        {
                            var shiftPast = lstShift.Where(m => m.ID == ShiftCurrent).FirstOrDefault();
                            var shiftFuture = lstShift.Where(m => m.ID == ShiftCurrentAfter).FirstOrDefault();
                            if (shiftPast != null && shiftFuture != null)
                            {
                                DateTime beginShiftPast = DateCurrent.Date.AddHours(shiftPast.InTime.Hour).AddMinutes(shiftPast.InTime.Minute);
                                DateTime endShiftPast = beginShiftPast.AddHours(shiftPast.CoOut);
                                DateTime beginShiftFuture = DateCurrent.Date.AddDays(1).AddHours(shiftFuture.InTime.Hour).AddMinutes(shiftFuture.InTime.Minute);
                                if (endShiftPast.AddHours(NumMinHour) > beginShiftFuture)
                                {
                                    var profile = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
                                    if (profile != null)
                                    {
                                        Error += profile.ProfileName + " [" + profile.CodeEmp + "]; ";
                                    }
                                    break;
                                }
                            }
                        }


                    }
                    #endregion

                }
            }

            if (Error != string.Empty)
            {
                Error = Error.Substring(0, Error.Length - 2);
                Error = Error + " Đã Bị Ràng Buộc Bởi Vấn Đề Các Ca Làm Việc Dưới 12 Tiếng";
            }

            return Error;
        }
        #endregion

        #region ImportRoster
        public void CheckData(Guid LoginUserID, List<ImportRosterModel> listRoster,
            out List<ImportRosterModel> ListRosterCorrect, out List<ImportRosterModel> ListRosterError)
        {
            int pageSize = 2500;
            int skipRows = 0;

            var listShiftInfo = new List<Cat_Shift>();
            var check12Hour = new Sys_AllSetting();
            List<string> listProfileCode= listRoster.Select(m=>m.CodeEmp).Distinct().ToList();
            
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                listShiftInfo = unitOfWork.CreateQueryable<Cat_Shift>().ToList();
                var ValidateLess12Hour = AppConfig.HRM_ATT_VALIDATE_ROSTER_NON_CONTINUE_12HOUR.ToString();
                check12Hour = unitOfWork.CreateQueryable<Sys_AllSetting>(m => m.Name == ValidateLess12Hour).FirstOrDefault();
            }
            var isAllowEditFutureRoster = (new SecurityService()).CheckPermission(LoginUserID, VnResource.Helper.Security.PrivilegeType.View, ConstantMessages.Att_EditFutureRoster);
            var isAllowEditPastRoster = (new SecurityService()).CheckPermission(LoginUserID, VnResource.Helper.Security.PrivilegeType.View, ConstantMessages.Att_EditPastRoster);
            ListRosterCorrect = new List<ImportRosterModel>();
            ListRosterError = new List<ImportRosterModel>();
            while (skipRows < listProfileCode.Count())
            {
                //Vinhtran - 20140718: Chia theo pagesize để không bị quá tải ram
                var listProfileSplit = listProfileCode.Skip(skipRows).Take(pageSize).ToList();
                var listRosterSplit = listRoster.Where(d => listProfileSplit.Contains(d.CodeEmp)).OrderBy(d => d.DateStart).ToList();
                CheckData(listRosterSplit, (check12Hour != null && check12Hour.Value1 == bool.TrueString), isAllowEditFutureRoster, isAllowEditPastRoster, listProfileSplit, listShiftInfo, ref ListRosterCorrect, ref ListRosterError);
                skipRows += pageSize;
            }
        }

        private void CheckData(List<ImportRosterModel> listRoster, bool check12Hours, bool isAllowEditFutureRoster,
            bool isAllowEditPastRoster, List<string> listProfileCode, List<Cat_Shift> listShiftInfo, ref List<ImportRosterModel> ListRosterCorrect, ref List<ImportRosterModel> ListRosterError)
        {
            #region Kiểm tra dữ liệu lỗi
            ListRosterError = new List<ImportRosterModel>();
            ListRosterCorrect = new List<ImportRosterModel>();
            
            var listProfileInfo = new List<Hre_Profile>().Select(m => new { m.ID, m.CodeEmp });
            var listOldRoster = new List<Att_Roster>().Select(d => new
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
            });
            var monthStart = DateTime.Today;
            var monthEnd = DateTime.Today;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                listProfileInfo = unitOfWork.CreateQueryable<Hre_Profile>(d => listProfileCode.Contains(d.CodeEmp.ToUpper())).Select(d => new { d.ID, d.CodeEmp }).ToList();
                var listProfileID = listProfileInfo.Select(d => d.ID).Distinct().ToList();
                var dateStart = listRoster.Select(d => d.DateStart).OrderBy(d => d).FirstOrDefault();
                var dateEnd = listRoster.Select(d => d.DateEnd).OrderBy(d => d).LastOrDefault();
                 monthStart = dateStart.Date.AddDays(1 - dateStart.Day);
                 monthEnd = monthStart.AddMonths(1).AddSeconds(-1);
                var checkDate = dateStart.AddDays(-1);
                string cancelStatus = RosterStatus.E_CANCEL.ToString();
                string rejectStatus = RosterStatus.E_REJECTED.ToString();
                listOldRoster = unitOfWork.CreateQueryable<Att_Roster>(
                d => d.Status != cancelStatus && d.Status != rejectStatus && listProfileID.Contains(d.ProfileID)
                    && d.DateStart <= dateEnd && d.DateEnd >= checkDate).Select(d => new
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
            }
            var currentDate = DateTime.Today;
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
                        ListRosterError.Add(roster);
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
                                ListRosterError.Add(roster);
                            }
                        }

                        if (!ListRosterError.Contains(roster))
                        {
                            //if (ImportType == ImportRosterType.OverrideMonth.ToString())
                            //{
                                //Không kiểm tra trùng theo kiểu miền giao DateStart và DateEnd
                                listOldRosterByProfile = listOldRosterByProfile.Where(d => !(d.DateStart >= roster.DateStart
                                    && d.DateEnd <= roster.DateEnd) && (d.DateStart < monthStart || d.DateEnd > monthEnd)).ToList();
                            //}
                            //else if (ImportType == ImportRosterType.OverrideHasValue.ToString())
                            //{
                            //    listOldRosterByProfile.Clear();
                            //}

                            if (listOldRosterByProfile.Count() > 0)
                            {
                                roster.Description = "Trùng lịch làm việc [" + roster.DateStart + " - " + roster.DateEnd + "]";
                                ListRosterError.Add(roster);
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
                                    ListRosterCorrect.Add(roster);
                                }
                                else
                                {
                                    roster.Description = "Ca làm việc liên tục dưới 12 tiếng";
                                    ListRosterError.Add(roster);
                                }
                            }
                        }
                    }
                }
                else
                {
                    roster.Description = "Không tìm thấy nhân viên [" + roster.CodeEmp + "]";
                    ListRosterError.Add(roster);
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

        public string SaveListRoster(List<ImportRosterModel> listRosterImport)
        {
            var ImportRoster = listRosterImport.CopyData<Att_RosterEntity>();
            BaseService _ba = new BaseService();
            var result = _ba.Add<Att_RosterEntity>(ImportRoster);


            // DataErrorCode errCode= DataErrorCode.Error;
            //using (var context = new VnrHrmDataContext())
            //{
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repo = new CustomBaseRepository<Att_Roster>(unitOfWork);
            //    repo.Add(ImportRoster);
            //    errCode  =  repo.SaveChanges();
            //    if(errCode== DataErrorCode.Success)
            //    {
            //        NotificationType.Success.ToString();
            //    }
            //    else
            //    {
            //        NotificationType.Error.ToString();
            //    }
            //}



            return result;
        }
        #endregion
    }
}
