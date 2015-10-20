using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Data;
using VnResource.Helper.Linq;

namespace HRM.Business.Attendance.Domain
{
    public class Att_AnnualDetailServices : BaseService
    {
        #region Property
        private int? _MonthStartAnl;
        private int MonthStartAnl
        {
            get
            {
                if (_MonthStartAnl == null)
                {
                    int result = 1;
                    BaseService _baseService = new BaseService();
                    string status = string.Empty;
                    var key = AppConfig.HRM_ATT_ANNUALDETAIL_MONTHBEGININYEAR.ToString();
                    var lstConfig = _baseService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey,string.Empty, ref status);

                    if (lstConfig.Count > 0)
                    {
                        var temp = lstConfig.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(temp.Value1) && !string.IsNullOrEmpty(temp.Value1))
                        {
                            int.TryParse(temp.Value1, out result);
                        }

                    }
                    _MonthStartAnl = result;
                }
                if (_MonthStartAnl == 0)
                    _MonthStartAnl = 1;
                return _MonthStartAnl ?? 1;////Todo: Thêm vào cấu hình chung tháng chỗ này cho biết là chu kỳ tháng bắt đầu tính của Công ty là tháng mấy vd: Honda (Tháng 4 đến cuối tháng 3 năm sau).
            }
        }

        #endregion

        #region Tính toán Annual Cho Toàn Nhân Viên
        public List<Att_AnnualDetailEntity> AnalyzeAnnualDetail(int Year, string strOrgStructure, string strPayroll, string proName, string codeEmp,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_AnnualDetail = new CustomBaseRepository<Att_AnnualDetail>(unitOfWork);

                bool? isFullEmp = true;
                List<Att_AnnualDetailEntity> lstResult = new List<Att_AnnualDetailEntity>();

                var lstProfileQuery = unitOfWork.CreateQueryable<Hre_Profile>(m => m.IsDelete == null);
                if (strOrgStructure != null && strOrgStructure != string.Empty)
                {
                    isFullEmp = false;
                    List<string> lstOrgIDChar = strOrgStructure.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<Guid> lstOrgID = new List<Guid>();
                    foreach (var item in lstOrgIDChar)
                    {
                        Guid ID = Guid.Empty;
                        Guid.TryParse(item, out ID);
                        if (ID != Guid.Empty)
                        {
                            lstOrgID.Add(ID);
                        }
                    }
                    lstProfileQuery = lstProfileQuery.Where(m => m.OrgStructureID != null && lstOrgID.Contains(m.OrgStructureID.Value));
                }
                if (proName != null && proName != string.Empty)
                {
                    isFullEmp = false;
                    List<string> lstProName = proName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (lstProName.Count > 1)
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => lstProName.Contains(m.ProfileName));
                    }
                    else
                    {
                        string ProfileName = lstProName.FirstOrDefault();
                        lstProfileQuery = lstProfileQuery.Where(m => m.ProfileName.Contains(ProfileName));
                    }
                }
                if (codeEmp != null && codeEmp != string.Empty)
                {
                    isFullEmp = false;
                    List<string> lstCodeEmp = codeEmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.ToLower()).ToList();
                    if (lstCodeEmp.Count > 1)
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => lstCodeEmp.Contains(m.CodeEmp.ToLower()));
                    }
                    else
                    {
                        string CodeEmp = lstCodeEmp.FirstOrDefault();
                        lstProfileQuery = lstProfileQuery.Where(m => m.CodeEmp.ToLower().Contains(CodeEmp));
                    }
                }
                if (strPayroll != null)
                {
                    List<Guid> lstPgID = strPayroll.Split(',').Select(s => Guid.Parse(s)).ToList();
                    isFullEmp = false;
                    lstProfileQuery = lstProfileQuery.Where(p => p.PayrollGroupID.HasValue && lstPgID.Contains(p.PayrollGroupID.Value));
                }
                var lstProfile = lstProfileQuery.Select(m => new Hre_ProfileMultiField()
                {
                    ID = m.ID,
                    CodeEmp = m.CodeEmp,
                    OrgStructureID = m.OrgStructureID,
                    PayrollGroupID = m.PayrollGroupID,
                    ProfileName = m.ProfileName,
                    DateHire = m.DateHire,
                    OrgStructureName = m.Cat_OrgStructure.OrgStructureName,
                    PositionID = m.PositionID,
                    DateQuit = m.DateQuit,
                    DateEndProbation = m.DateEndProbation,
                    JobTitleID = m.JobTitleID
                }).ToList();
                var result = AnalyzeAnnualDetail(lstProfile, Year, isFullEmp, userLogin);

                Att_AnnualDetailEntity ann = new Att_AnnualDetailEntity();
                foreach (var item in result)
                {
                    if (item.ProfileID == null)
                        continue;
                    var temp = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                    ann = new Att_AnnualDetailEntity();
                    ann = item.CopyData<Att_AnnualDetailEntity>();
                    ann.DateHire = temp.DateHire;
                    ann.OrgStructureName = temp.OrgStructureName;
                    ann.ProfileName = temp.ProfileName;
                    ann.CodeEmp = temp.CodeEmp;
                    lstResult.Add(ann);
                }
                return lstResult;
            }
        }
        private List<Att_AnnualDetail> AnalyzeAnnualDetail(List<Hre_ProfileMultiField> lstProfile, int Year, bool? isFullEmp, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_AnnualDetail = new CustomBaseRepository<Att_AnnualDetail>(unitOfWork);

                List<Att_AnnualDetail> lstResult = new List<Att_AnnualDetail>();
                List<Guid> lstProfileID = lstProfile.Select(m => m.ID).ToList();
                int BeginMonth = MonthStartAnl;
                DateTime BeginYear = new DateTime(Year, BeginMonth, 1);
                DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                DateTime BeginYearToGetData = BeginYear.AddMonths(-1); //trừ một tháng để lấy data (bao những data thuộc những ngày trước tháng dành cho chế độ lương ko phải từ ngày 1)

                var lstGradeCgf = unitOfWork.CreateQueryable<Cat_GradeAttendance>().ToList();
                var lstLeaveDayTypeAnnual = unitOfWork.CreateQueryable<Cat_LeaveDayType>(s => s.IsAnnualLeave == true).ToList();
                List<Guid> lstAnnualTypeID = lstLeaveDayTypeAnnual.Select(s => s.ID).ToList();
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();



                var lstAnnualDetailInDB_Query = unitOfWork.CreateQueryable<Att_AnnualDetail>(m => m.ProfileID != null && m.Year == Year);
                var lstLeaveDayAnl_Query = unitOfWork.CreateQueryable<Att_LeaveDay>(m =>
                    m.Status == E_APPROVED
                    && m.DateStart <= EndYear
                    && m.DateEnd >= BeginYearToGetData
                    && m.LeaveDayTypeID != null
                    && lstAnnualTypeID.Contains(m.LeaveDayTypeID));
                var lstAttGrade_Query = unitOfWork.CreateQueryable<Att_Grade>();
                var lstAnualLeaveCfg_Query = unitOfWork.CreateQueryable<Att_AnnualLeave>(m => m.Year == Year);
                var lstlstRoster_Query = unitOfWork.CreateQueryable<Att_Roster>(m =>
                    m.Status == E_APPROVED
                    && m.DateStart <= EndYear
                    && m.DateEnd >= BeginYearToGetData);
                if (isFullEmp != null && isFullEmp.Value == false)
                {
                    lstAnnualDetailInDB_Query = lstAnnualDetailInDB_Query.Where(m => m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value));
                    lstLeaveDayAnl_Query = lstLeaveDayAnl_Query.Where(m => lstProfileID.Contains(m.ProfileID));
                    lstAttGrade_Query = lstAttGrade_Query.Where(m => m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value));
                    lstAnualLeaveCfg_Query = lstAnualLeaveCfg_Query.Where(m => lstProfileID.Contains(m.ProfileID));
                    lstlstRoster_Query = lstlstRoster_Query.Where(m => lstProfileID.Contains(m.ProfileID));
                }

                var lstAnnualDetailInDB = lstAnnualDetailInDB_Query.ToList();
                var lstLeaveDayAnl = lstLeaveDayAnl_Query.Select(m => new Att_LeaveDayInfo()
                {
                    ID = m.ID
                    ,
                    ProfileID = m.ProfileID
                    ,
                    DateStart = m.DateStart
                    ,
                    DateEnd = m.DateEnd
                    ,
                    LeaveDayTypeID = m.LeaveDayTypeID
                    ,
                    TotalDuration = m.TotalDuration
                    ,
                    Duration = m.Duration
                    ,
                    DurationType = m.DurationType
                    ,
                    LeaveDays = m.LeaveDays
                    ,
                    LeaveHours = m.LeaveHours
                }).ToList();
                var lstAttGrade = lstAttGrade_Query.ToList();
                var lstAnualLeaveCfg = lstAnualLeaveCfg_Query.ToList();
                var lstRoster = lstlstRoster_Query.Select(m => new Att_RosterInfo() { ID = m.ID, ProfileID = m.ProfileID, Type = m.Type, MonShiftID = m.MonShiftID, TueShiftID = m.TueShiftID, WedShiftID = m.WedShiftID, ThuShiftID = m.ThuShiftID, FriShiftID = m.FriShiftID, SatShiftID = m.SatShiftID, SunShiftID = m.SunShiftID }).ToList();


                var lstRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(m => m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData).ToList();
                var lstDayOff = unitOfWork.CreateQueryable<Cat_DayOff>(m => m.DateOff >= BeginYearToGetData && m.DateOff <= EndYear).ToList();

                // Phép Năm chốt Từ Ngày 1 -> 31 Hàng Tháng 
                bool IsFrom1To31 = false;
                string type2 = AppConfig.HRM_ATT_ANNUALLEAVE_ANNUALBEGINMONTHTOENDMONTH.ToString();
                Sys_AllSetting sys_ANNUAL_BEGINMONTHTO_ENDMONTH = unitOfWork.CreateQueryable<Sys_AllSetting>(sy => sy.Name == type2).FirstOrDefault();
                if (sys_ANNUAL_BEGINMONTHTO_ENDMONTH != null && sys_ANNUAL_BEGINMONTHTO_ENDMONTH.Value1 == bool.TrueString)
                {
                    IsFrom1To31 = true;
                }
                List<Guid> lstPositionIDs = lstProfile.Where(m => m.PositionID != null).Select(m => m.PositionID.Value).Distinct().ToList();
                List<Cat_Position> lstPosition = unitOfWork.CreateQueryable<Cat_Position>(m => lstPositionIDs.Contains(m.ID)).ToList<Cat_Position>();
                var lstAllSetting = unitOfWork.CreateQueryable<Sys_AllSetting>().ToList();

                var lstHDTJob=new List<Hre_HDTJob>();
                foreach (var templstProfileId in lstProfileID.Chunk(1000))
                {
                    lstHDTJob.AddRange(unitOfWork.CreateQueryable<Hre_HDTJob>(Guid.Empty, m => m.ProfileID != null && templstProfileId.Contains(m.ProfileID.Value)).ToList());
                }
                foreach (var profile in lstProfile)
                {
                    var lstGradeByProfile = lstAttGrade.Where(m => m.ProfileID == profile.ID).OrderByDescending(m => m.MonthStart).ToList();
                    var lstLeaveDayByprofile = lstLeaveDayAnl.Where(m => m.ProfileID == profile.ID).ToList();
                    var AnualLeaveCfg = lstAnualLeaveCfg.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                    var lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID).ToList();
                    var position = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();

                    List<Att_AnnualDetail> lstAnnualDetailByProfile_Update = new List<Att_AnnualDetail>();

                    List<Att_AnnualDetail> lstAnnualDetailByProfile = AnalyzeAnnualDetailPerProfile(profile,
                        lstGradeCgf, lstGradeByProfile, BeginYear, EndYear, lstLeaveDayByprofile, lstAnnualDetailInDB,
                        AnualLeaveCfg, lstRosterByProfile, lstRosterGroup, lstDayOff, IsFrom1To31,
                        position, lstAllSetting, lstHDTJob, out  lstAnnualDetailByProfile_Update, userLogin);
                    lstResult.AddRange(lstAnnualDetailByProfile);
                    lstResult.AddRange(lstAnnualDetailByProfile_Update);
                    repoAtt_AnnualDetail.Add(lstAnnualDetailByProfile);
                    repoAtt_AnnualDetail.Edit(lstAnnualDetailByProfile_Update);
                }
                repoAtt_AnnualDetail.SaveChanges();
                return lstResult;
            }
        }
        /// <summary>
        /// Hàm tính toán Phép năm dành cho từng nhân viên
        /// </summary>
        /// <param name="Profile">Nhân Viên</param>
        /// <param name="lstGradeCfg">Ds Chế Độ lương</param>
        /// <param name="lstGrade">Ds Grade của Nhân viên</param>
        /// <param name="BeginYear">Ngày bắt đầu của năm</param>
        /// <param name="EndYear">Ngày Kết Thúc Của Năm</param>
        /// <param name="lstLeaveAnl">Ds nghỉ phép năm</param>
        /// <param name="lstAnnualDetailInDB">Ds AnnualDetail trong DB</param>
        /// <param name="AnnualLeave">AnnualLeave</param>
        /// <param name="lstRosterInYear">Ds Roster</param>
        /// <param name="lstRosterGroup">Ds RosterGroup</param>
        /// <param name="lstWorkHistory">Ds WorkingHistory</param>
        /// <param name="lstDayOff">Ds Ngày Nghỉ Lễ </param>
        /// <returns></returns>
        private List<Att_AnnualDetail> AnalyzeAnnualDetailPerProfile(Hre_ProfileMultiField Profile, List<Cat_GradeAttendance>
            lstGradeCfg, List<Att_Grade> lstGrade, DateTime BeginYear, DateTime EndYear, List<Att_LeaveDayInfo> lstLeaveAnl,
            List<Att_AnnualDetail> lstAnnualDetailInDB, Att_AnnualLeave AnnualLeave, List<Att_RosterInfo> lstRosterInYearIn,
            List<Att_RosterGroup> lstRosterGroup, List<Cat_DayOff> lstDayOff,
            bool IsFrom1To31, Cat_Position Position, List<Sys_AllSetting> lstAllSetting, List<Hre_HDTJob> lstHDTJob, out List<Att_AnnualDetail> AnalyzeAnnualDetailPerProfile_Update, string userLogin)
        {
            AnalyzeAnnualDetailPerProfile_Update = new List<Att_AnnualDetail>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();

                var lstRosterID = lstRosterInYearIn.Select(s => s.ID).ToList();
                var lstRosterInYear = repoAtt_Roster.FindBy(s => lstRosterID.Contains(s.ID)).ToList();

                List<Att_AnnualDetail> lstResult = new List<Att_AnnualDetail>();
                double leaveBeginYearToMonth = 0;
                double leaveBeginYearToMonth_Init = 0;
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRoster_byProfile = lstRosterInYear.Where(m => m.ProfileID == Profile.ID && m.Type != E_ROSTERGROUP).ToList();
                List<Att_Roster> lstRosterTypeGroup = lstRosterInYear.Where(m => m.ProfileID == Profile.ID && m.Type == E_ROSTERGROUP).ToList();

                for (DateTime Month = BeginYear; Month <= EndYear; Month = Month.AddMonths(1))
                {
                    var gradeByProfileByTime = lstGrade.Where(m => m.ProfileID == Profile.ID && m.MonthStart <= Month).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                    if (gradeByProfileByTime == null)
                        continue;
                    var GradeCfg = lstGradeCfg.Where(m => m.ID == gradeByProfileByTime.GradeAttendanceID).FirstOrDefault();
                    if (GradeCfg == null)
                        continue;

                    DateTime BeginMonth = Month;
                    DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                    if (!IsFrom1To31)
                    {
                        Att_AttendanceServices.GetRangeMaxMinGrade(new List<Cat_GradeAttendance>() { GradeCfg }, Month, out BeginMonth, out EndMonth);
                    }

                    int AnnualLeaveMonthStart = AnnualLeave != null ? AnnualLeave.MonthStart : 1;
                    double AnnualLeaveInitAnlValue = AnnualLeave != null ? AnnualLeave.InitAnlValue : 0;

                    double Availale = Att_AttendanceLib.GetAnnualLeaveReceive(BeginYear.Year, Month, GradeCfg, Profile.DateHire,
                        Profile.DateEndProbation, Profile.DateQuit, AnnualLeaveMonthStart, AnnualLeaveInitAnlValue, Position, Profile,
                        lstLeaveAnl, lstAllSetting, lstHDTJob, lstDayOff.Select(m => m.DateOff).ToList(), lstLeaveAnl, userLogin);
                    double LeaveInMonth = 0;
                    List<Att_LeaveDayInfo> lstAnlInMonth = lstLeaveAnl.Where(m => m.ProfileID == Profile.ID && m.DateStart <= EndMonth && m.DateEnd >= BeginMonth).ToList();
                    foreach (var item in lstAnlInMonth)
                    {
                        var listRosterEntity = lstRoster_byProfile.Select(d => new Att_RosterEntity
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

                        if (item.DateStart >= BeginMonth && item.DateEnd <= EndMonth)
                        {
                            //Chi cần lấy TotalDuration hoặc lấy giờ rồi lấy ca
                            if (item.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                            {
                                LeaveInMonth += item.LeaveDays ?? 0;
                            }
                            else
                            {
                                //Lấy ra ca làm việc 
                                DateTime DateBeginLeave = item.DateStart.Date;
                                DateTime DateEndLeave = item.DateEnd.Date;

                                Dictionary<DateTime, Cat_Shift> dicShift = Att_AttendanceLib.GetDailyShifts(Profile != null ? Profile.ID : Guid.Empty, DateBeginLeave, DateEndLeave, listRosterEntity, listRosterGroupEntity, shifts);

                                if (dicShift != null && dicShift.ContainsKey(DateBeginLeave))
                                {
                                    Cat_Shift shift = dicShift[DateBeginLeave];
                                    if (shift != null)
                                    {
                                        double HourWorkday = shift.udStandardWorkHours > 0 ? shift.udStandardWorkHours : 8.0;

                                        LeaveInMonth += item.LeaveHours.Value / HourWorkday;
                                    }
                                }
                            }
                        }
                        else
                        {
                            DateTime DateBegin = BeginMonth > item.DateStart ? BeginMonth : item.DateStart;
                            DateTime DateEnd = EndMonth < item.DateEnd ? BeginMonth : item.DateEnd;
                            Dictionary<DateTime, Cat_Shift> dicShift = Att_AttendanceLib.GetDailyShifts(Profile != null ? Profile.ID : Guid.Empty, DateBegin, DateEnd, listRosterEntity, listRosterGroupEntity, shifts);
                            for (DateTime dateCheck = DateBegin; dateCheck <= DateEnd; dateCheck = dateCheck.AddDays(1))
                            {
                                if (Att_WorkDayHelper.IsWorkDay(dateCheck, GradeCfg, dicShift, lstDayOff) && !lstDayOff.Any(m => m.DateOff == dateCheck))
                                {
                                    LeaveInMonth++;
                                }
                            }
                        }
                    }
                    Att_AnnualDetail AnnualDetail = lstAnnualDetailInDB.Where(m => m.ProfileID == Profile.ID && m.MonthYear == Month).FirstOrDefault();
                    bool isNew = false;
                    if (AnnualDetail == null)
                    {
                        isNew = true;
                        AnnualDetail = new Att_AnnualDetail();
                    }
                    else
                    {
                        AnalyzeAnnualDetailPerProfile_Update.Add(AnnualDetail);
                    }


                    if (Profile.DateQuit != null)
                    {
                        DateTime MonthQuit = Profile.DateQuit.Value.Day >= 15 ? Profile.DateQuit.Value.AddMonths(1) : Profile.DateQuit.Value;
                        MonthQuit = new DateTime(MonthQuit.Year, MonthQuit.Month, 1);
                        if (Month >= MonthQuit)
                        {
                            AnnualDetail.IsDelete = true;
                        }
                    }
                    AnnualDetail.Available = Availale;
                    AnnualDetail.InitAvailable = AnnualLeave == null ? 0 : AnnualLeave.InitAnlValue;
                    AnnualDetail.ProfileID = Profile.ID;
                    //AnnualDetail.Hre_Profile = Profile.CopyData<Hre_Profile>();
                    AnnualDetail.Year = BeginYear.Year;
                    AnnualDetail.MonthYear = Month;
                    AnnualDetail.MonthBeginInYear = MonthStartAnl; //todo: Phần tử này để trong cấu hình chung
                    if (AnnualLeave != null && AnnualLeave.MonthResetAnlOfBeforeYear != null)
                    {
                        AnnualDetail.MonthResetInitAvailable = AnnualLeave.MonthResetAnlOfBeforeYear.Value;//todo: MonthReset Này để trong chế độ lương
                    }
                    else
                    {
                        AnnualDetail.MonthResetInitAvailable = 12;//todo: MonthReset Này để trong chế độ lương
                    }
                    AnnualDetail.MonthStartProfile = AnnualLeave == null ? 1 : AnnualLeave.MonthStart;
                    if (AnnualDetail.MonthResetInitAvailable != null && AnnualDetail.MonthResetInitAvailable != 12 && AnnualDetail.Year != null)
                    {
                        DateTime MonthReset = new DateTime(AnnualDetail.Year.Value, AnnualDetail.MonthResetInitAvailable.Value, 1);
                        AnnualDetail.IsHaveResetInitAvailable = true;
                        if (Month <= MonthReset) //Trong những tháng có
                        {
                            double delta = leaveBeginYearToMonth_Init + LeaveInMonth - AnnualDetail.InitAvailable.Value;
                            if (delta > 0)
                            {
                                AnnualDetail.TotalLeaveBefFromInitAvailable = leaveBeginYearToMonth_Init;
                                AnnualDetail.LeaveInMonthFromInitAvailable = LeaveInMonth - delta;
                                AnnualDetail.LeaveInMonth = delta;
                                AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                                leaveBeginYearToMonth += delta;
                                leaveBeginYearToMonth_Init += LeaveInMonth - delta;
                            }
                            else
                            {
                                AnnualDetail.TotalLeaveBefFromInitAvailable = leaveBeginYearToMonth_Init;
                                AnnualDetail.LeaveInMonthFromInitAvailable = LeaveInMonth;
                                AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                                AnnualDetail.LeaveInMonth = 0;
                                leaveBeginYearToMonth_Init += LeaveInMonth;
                            }
                        }
                        else //sau những tháng kho có
                        {
                            AnnualDetail.InitAvailable = 0;
                            AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                            AnnualDetail.LeaveInMonth = LeaveInMonth;
                            leaveBeginYearToMonth += LeaveInMonth;
                        }
                    }
                    else //Bình thường
                    {
                        AnnualDetail.IsHaveResetInitAvailable = false;
                        AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                        AnnualDetail.LeaveInMonth = LeaveInMonth;
                        leaveBeginYearToMonth += LeaveInMonth;
                    }
                    double remain = (AnnualDetail.Available ?? 0) + (AnnualDetail.InitAvailable ?? 0) - (AnnualDetail.TotalLeaveBefFromInitAvailable ?? 0) - (AnnualDetail.LeaveInMonthFromInitAvailable ?? 0) - (AnnualDetail.TotalLeaveBef ?? 0) - (AnnualDetail.LeaveInMonth ?? 0);
                    AnnualDetail.Remain = remain;
                    AnnualDetail.Type = AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
                    if (isNew)
                    {
                        lstResult.Add(AnnualDetail);
                    }
                }
                return lstResult;
            }
        }
        #endregion
    }
}
