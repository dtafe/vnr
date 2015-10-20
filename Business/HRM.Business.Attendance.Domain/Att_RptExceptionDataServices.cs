using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Domain;
using System.Collections;
using System.Data;
using HRM.Data.Entity;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Main.Domain;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Business.Hr.Domain;
using HRM.Business.HrmSystem.Models;

namespace HRM.Business.Attendance.Domain
{
    public class Att_RptExceptionDataServices : BaseService
    {
        public List<Att_RptExceptionDataEntity> GetAtt_RptExceptionData(DateTime DateStart, DateTime DateEnd, String OrgStructureIDs, bool NoScan, bool DifferenceMoreRoster, double Difference, double LessHours, bool MissInOut, out string message,string userExport ,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                message = "";
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoAtt_InOut = new CustomBaseRepository<Att_InOut>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);


                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                List<object> lstOrgIDs = new List<object>();
                lstOrgIDs.AddRange(new object[3]);
                lstOrgIDs[0] = OrgStructureIDs;
                lstOrgIDs[1] = null;
                lstOrgIDs[2] = null;
                lstProfile = GetData<Hre_ProfileEntity>(lstOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref message);

                List<Att_RptExceptionDataEntity> Result = new List<Att_RptExceptionDataEntity>();
                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                lstProfile = lstProfile.Where(d => d.StatusSyn != waitStatus && (d.DateQuit == null || d.DateQuit.Value > DateEnd)).ToList();
                List<Guid> lstProfileID = lstProfile.Select(d => d.ID).ToList();
                List<Att_InOut> lstInOutAll = repoAtt_InOut.FindBy(wd => wd.WorkDate >= DateStart
                                                    && wd.WorkDate <= DateEnd).OrderBy(wd => wd.Hre_Profile.ProfileName).ToList();
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                List<Cat_OrgStructure> lstOrg = new List<Cat_OrgStructure>();
                if (!string.IsNullOrEmpty(OrgStructureIDs)) // Chỉ lấy phòng ban theo sự lựa chọn
                {
                    List<int> lstOrderNumber = OrgStructureIDs.Split(',').Distinct().Select(s => int.Parse(s)).ToList();
                    lstOrg = repoCat_OrgStructure.FindBy(m => m.IsDelete == null && lstOrderNumber.Contains(m.OrderNumber)).ToList();
                }
                else //Lấy hêt phòng ban
                {
                    lstOrg = repoCat_OrgStructure.FindBy(m => m.IsDelete == null).ToList();
                }
                if (NoScan)
                {
                    List<Cat_DayOff> lstHoliday = repoCat_DayOff.FindBy(d => d.IsDelete == null).ToList();
                    String RosterTypeAbsent = RosterType.E_TIME_OFF.ToString();
                    List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(prop => prop.DateStart <= DateEnd && prop.DateEnd >= DateStart
                                    && prop.Type != RosterTypeAbsent).ToList();
                    
                    List<Att_LeaveDay> lstLeaveDayAll = repoAtt_LeaveDay.FindBy(prop => prop.DateStart <= DateEnd && prop.DateEnd >= DateStart).ToList();
                    List<Att_Grade> lstAtt_Grade = repoAtt_Grade.FindBy(d => d.MonthStart <= DateEnd && d.MonthEnd >= DateStart && d.ProfileID.HasValue && lstProfileID.Contains(d.ProfileID.Value)).ToList();
                    //List<Sal_Grade> lstGradeAll = GradeDAO.getAllGrade(EntityService.GuidMainContext
                    //                                                , lstProfileID
                    //                                                , DateEnd
                    //                                                , LoginUserID);
                    List<Hre_WorkHistory> lstWHistory = repoHre_WorkHistory.FindBy(d => d.IsDelete == null).ToList();
                    List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                    List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                    Att_RosterServices.GetRosterGroup(lstProfileID, DateStart, DateEnd, out lstRosterTypeGroup, out  lstRosterGroup);
                    //RosterDAO.GetRosterGroup(GuidContext, lstProfileIDs, DateStart, DateEnd, out lstRosterTypeGroup, out lstRosterGroup);


                    foreach (Hre_ProfileEntity profile in lstProfile)
                    {
                        List<Att_LeaveDay> lstProfileLeaveDay = lstLeaveDayAll.Where(ld => ld.ProfileID == profile.ID).ToList();
                        List<Att_InOut> lstProfileInOut = lstInOutAll.Where(ld => ld.ProfileID == profile.ID).ToList();
                        //Sal_Grade grade = lstGradeAll.Where(grad => grad.ProfileID == profile.ID).SingleOrDefault();
                        Att_Grade grade = lstAtt_Grade.Where(d => d.ProfileID == profile.ID).FirstOrDefault();
                        List<Att_Roster> lstProfileRoster = lstRoster.Where(rt => rt.ProfileID == profile.ID).ToList();
                        List<Hre_WorkHistory> lstWHistoryPro = lstWHistory.Where(wh => wh.ProfileID == profile.ID).ToList();
                        Cat_GradeAttendance gradeCfg = grade == null ? null : grade.Cat_GradeAttendance;
                        //Hashtable htRoster = RosterDAO.GetRosterTable(profile, lstProfileRoster, lstWHistoryPro, gradeCfg, DateStart, DateEnd, lstRosterGroup, lstRosterTypeGroup);
                        Hre_Profile objProfile = profile.CopyData<Hre_Profile>();
                        Hashtable htRoster = Att_RosterServices.GetRosterTable(objProfile, lstProfileRoster, lstWHistoryPro, gradeCfg, DateStart, DateEnd, lstRosterGroup, lstRosterTypeGroup);
                        //string[] strDepartment = GradeCfgDAO.getLinkDepartment(ListCacheOrgStructure, profile.Cat_OrgStructure);
                        //profile.udLinkDepartmentName = strDepartment[0];
                        if (grade == null)
                            continue;
                        if (grade.Cat_GradeAttendance.AttendanceMethod != AttendanceMethod.E_TAM.ToString())
                            continue;

                        for (DateTime idx = DateStart; idx <= DateEnd
                                                     ; idx = idx.AddDays(1))
                        {
                            // kiểm tra nếu nghỉ việc trong tháng, các ngày sau không hiển thị lên
                            if (profile.DateQuit != null && idx > profile.DateQuit.Value) continue;

                            //In-Out
                            if (lstHoliday.Where(hol => hol.DateOff == idx).Count() > 0)
                                continue;

                            if (!Att_AttendanceServices.IsWorkDay(grade.Cat_GradeAttendance, htRoster, lstHoliday, idx)
                                || idx < profile.DateHire
                                || idx >= profile.DateQuit)
                                continue;

                            Att_RptExceptionDataEntity exceptionData = null;
                            List<Att_InOut> lstDateProfileInOut = lstProfileInOut.Where(inout => inout.WorkDate == idx).ToList();
                            //ko di lam, ko quet the
                            if (lstDateProfileInOut.Count <= 0)
                            {
                                //ko thong bao nghi
                                if (lstProfileLeaveDay.Where(ld => ld.ProfileID == profile.ID
                                                            && ld.DateStart.Date <= idx && ld.DateEnd.Date >= idx).Count() <= 0)
                                {
                                    exceptionData = SetExceptionData(null, objProfile, null, idx,userExport);
                                    var OrgC = lstOrg.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
                                    exceptionData.Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                                    var orgSection = LibraryService.GetNearestParent(profile.OrgStructureID, OrgUnit.E_SECTION, lstOrg, orgTypes);
                                    exceptionData.Section = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                                    exceptionData.Position = positon != null ? positon.Code : string.Empty;
                                    //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_InOut);
                                    //exceptionData.Exception = LanguageManager.GetString(Messages.MissingInOut);
                                    exceptionData.DataType = ("Att_InOut").TranslateString();
                                    exceptionData.Exception = ConstantMessages.MissingInOut.TranslateString();
                                    exceptionData.Description = "? - ?";
                                    Result.Add(exceptionData);
                                }
                            }

                        }
                    }
                }
                if (DifferenceMoreRoster)
                {
                    foreach (Att_InOut InOut in lstInOutAll)
                    {
                        if (InOut.InTime != null && InOut.OutTime != null)
                        {
                            if (Difference < Att_AttendanceServices.GetShiftRosterShiftInOutHour(InOut, InOut.Cat_Shift))
                            {
                                Att_RptExceptionDataEntity exceptionData = SetExceptionData(InOut, InOut.Hre_Profile
                                                                                    , InOut.Cat_Shift, InOut.WorkDate,userExport);
                                var OrgC = lstOrg.Where(m => m.ID == InOut.Hre_Profile.OrgStructureID).FirstOrDefault();
                                exceptionData.Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                                var orgSection = LibraryService.GetNearestParent(InOut.Hre_Profile.OrgStructureID, OrgUnit.E_SECTION, lstOrg, orgTypes);
                                exceptionData.Section = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                var positon = positions.FirstOrDefault(s => s.ID == InOut.Hre_Profile.PositionID);
                                exceptionData.Position = positon != null ? positon.Code : string.Empty;
                                //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_InOut);
                                //exceptionData.Exception = LanguageManager.GetString(Messages.DifferenceShiftRosterAndInOut) + " " + Difference + " " + LanguageManager.GetString(Messages.Hour);
                                exceptionData.DataType = ("Att_InOut").TranslateString();
                                exceptionData.Exception = ConstantMessages.DifferenceShiftRosterAndInOut.TranslateString() + " " + Difference + " " + ConstantMessages.Hour.TranslateString();
                                String desc = "InOut: ";
                                desc += InOut.InTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);
                                desc += " - ";
                                desc += InOut.OutTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);
                                desc += " / Shift: ";
                                desc += InOut.Cat_Shift.ShiftName;

                                exceptionData.Description = desc;
                                Result.Add(exceptionData);
                            }
                        }
                    }
                }
                if (LessHours > 0)
                {
                    Double minHrs = 0;
                    //Get Hours
                    try
                    {
                        minHrs = LessHours;
                    }
                    catch (System.Exception ex)
                    {

                    }


                    List<Att_InOut> lstInOutAllLessHours = lstInOutAll.Where(d => d.InTime.HasValue && d.OutTime.HasValue).ToList();

                    foreach (Att_InOut InOut in lstInOutAllLessHours)
                    {
                        Double workHours = Att_AttendanceServices.GetInOutWorkingHour(InOut, InOut.Cat_Shift);
                        if (minHrs > workHours)
                        {
                            Att_RptExceptionDataEntity exceptionData = SetExceptionData(InOut, InOut.Hre_Profile
                                                                                , InOut.Cat_Shift, InOut.WorkDate,userExport);
                            var OrgC = lstOrg.Where(m => m.ID == InOut.Hre_Profile.OrgStructureID).FirstOrDefault();
                            exceptionData.Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                            var orgSection = LibraryService.GetNearestParent(InOut.Hre_Profile.OrgStructureID, OrgUnit.E_SECTION, lstOrg, orgTypes);
                            exceptionData.Section = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            var positon = positions.FirstOrDefault(s => s.ID == InOut.Hre_Profile.PositionID);
                            exceptionData.Position = positon != null ? positon.Code : string.Empty;
                            //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_InOut);
                            //exceptionData.Exception = LanguageManager.GetString(Messages.WorkingHoursLess) + " " + Difference + " " + LanguageManager.GetString(Messages.Hour);
                            exceptionData.DataType = ("Att_InOut").TranslateString();
                            exceptionData.Exception = ConstantMessages.WorkingHoursLess.TranslateString() + " " + Difference + " " + ConstantMessages.Hour.TranslateString();
                            String desc = "";
                            desc = InOut.InTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);
                            desc += " - ";
                            desc += InOut.OutTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);

                            exceptionData.Description = desc;
                            Result.Add(exceptionData);
                        }
                    }

                }
                if (MissInOut)
                {
                    List<Att_InOut> lstInOutAllMissInOut = lstInOutAll.Where(wd => (wd.InTime.HasValue
                                                                                           && !wd.OutTime.HasValue)
                                                                                          ||
                                                                                          (!wd.InTime.HasValue
                                                                                           && wd.OutTime.HasValue)).ToList();
                    foreach (Att_InOut InOut in lstInOutAll)
                    {
                        if (InOut.InTime == null || InOut.OutTime == null)
                        {
                            Att_RptExceptionDataEntity exceptionData = SetExceptionData(InOut, InOut.Hre_Profile, InOut.Cat_Shift, InOut.WorkDate,userExport);
                            var OrgC = lstOrg.Where(m => m.ID == InOut.Hre_Profile.OrgStructureID).FirstOrDefault();
                            exceptionData.Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                            var orgSection = LibraryService.GetNearestParent(InOut.Hre_Profile.OrgStructureID, OrgUnit.E_SECTION, lstOrg, orgTypes);
                            exceptionData.Section = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            var positon = positions.FirstOrDefault(s => s.ID == InOut.Hre_Profile.PositionID);
                            exceptionData.Position = positon != null ? positon.Code : string.Empty;
                            //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_InOut);
                            //exceptionData.Exception = LanguageManager.GetString(Messages.MissingInOut);
                            exceptionData.DataType = ("Att_InOut").TranslateString();
                            exceptionData.Exception = ConstantMessages.MissingInOut.TranslateString();
                            String desc = "";
                            if (InOut.InTime != null)
                                desc = InOut.InTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);
                            else
                                desc = "?";
                            desc += " - ";
                            if (InOut.OutTime != null)
                                desc += InOut.OutTime.Value.ToString(ConstantFormat.HRM_Format_HourMinSecond);
                            else
                                desc += "?";
                            exceptionData.Description = desc;
                            Result.Add(exceptionData);
                        }
                    }
                }
                return Result;
            }
        }
        private Att_RptExceptionDataEntity SetExceptionData(Att_InOut InOut, Hre_Profile profile, Cat_Shift shift, DateTime idx,string userExport)
        {
            Att_RptExceptionDataEntity exceptionData = new Att_RptExceptionDataEntity();
            exceptionData.ID = Guid.NewGuid();
            exceptionData.InOutID = InOut == null ? Guid.Empty : InOut.ID;
            //exceptionData.EntityType = ClassNames.Att_InOut;
            exceptionData.EntityType = "Att_InOut";
            exceptionData.CodeEmp = profile.CodeEmp;
            exceptionData.ShiftID = shift == null ? Guid.Empty : shift.ID;
            exceptionData.ProfileID = profile.ID;
            exceptionData.ProfileName = profile.ProfileName;
            exceptionData.UserExport = userExport;
            exceptionData.DateExport = DateTime.Today;

            //exceptionData.Position = profile.Cat_Position == null ? string.Empty : profile.Cat_Position.PositionName;

            //string[] strDepartment = GradeCfgDAO.getLinkDepartment(ListCacheOrgStructure, profile.Cat_OrgStructure, true);
            //exceptionData.Department = strDepartment[1];

            exceptionData.Date = idx;
            return exceptionData;
        }
    }
}
