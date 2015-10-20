using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System.Data;
using System;

namespace HRM.Business.Attendance.Domain
{
    public class Att_RosterGroupServices : BaseService
    {
        private List<Cat_Shift> _lstShift_RosterGroup;
        public List<Cat_Shift> lstShift_RosterGroup
        {
            get
            {
                if (_lstShift_RosterGroup != null)
                {
                    return _lstShift_RosterGroup;
                }
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP = AppConfig.HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP.ToString();
                    var AttConfig = unitOfWork.CreateQueryable<Sys_AllSetting>(m => m.Name == HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP).FirstOrDefault();
                    if (AttConfig != null)
                    {
                        List<string> CodeShifs = AttConfig.Value1.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        _lstShift_RosterGroup = unitOfWork.CreateQueryable<Cat_Shift>(m => CodeShifs.Contains(m.Code)).ToList();
                    }
                }
                return _lstShift_RosterGroup;
            }
        }

        private List<string> _lstName_RosterGroup;
        public List<string> lstName_RosterGroup
        {
            get
            {
                if (_lstName_RosterGroup != null)
                {
                    return _lstName_RosterGroup;
                }
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string HRM_ATT_CONFIG_NAME_ROSTERGROUP = AppConfig.HRM_ATT_CONFIG_NAME_ROSTERGROUP.ToString();
                    var AttConfig = unitOfWork.CreateQueryable<Sys_AllSetting>(m => m.Name == HRM_ATT_CONFIG_NAME_ROSTERGROUP).FirstOrDefault();
                    if (AttConfig != null)
                    {
                        List<string> CodeShifs = AttConfig.Value1.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        _lstName_RosterGroup = CodeShifs;
                    }
                }
                return _lstName_RosterGroup;
            }
        }


        public DataTable GetTableRosterGroupByMonth(DateTime MonthYear, ModifyType Type)
        {
            if (lstShift_RosterGroup == null || lstShift_RosterGroup.Count == 0)
                return new DataTable();

            DateTime BeginMonth = new DateTime(MonthYear.Year, MonthYear.Month, 1);
            DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
            List<string> lstShiftCode = lstShift_RosterGroup.Select(m => m.Code).Distinct().ToList();
            DataTable dt = GetSchemaRosterGroup(lstShiftCode);
            if (Type == ModifyType.E_CREATE)
            {
                for (DateTime datecheck = BeginMonth; datecheck < EndMonth; datecheck = datecheck.AddDays(1))
                {
                    DataRow dr = dt.NewRow();
                    dr[Att_ChangeRosterGroupEntity.FieldNames.DayOfMonth] = datecheck.ToString("dd/MM/yyyy");
                    string DayOfWeek = string.Empty;
                    switch (datecheck.DayOfWeek)
                    {
                        case System.DayOfWeek.Monday:
                            DayOfWeek = System.DayOfWeek.Monday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Tuesday:
                            DayOfWeek = System.DayOfWeek.Tuesday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Wednesday:
                            DayOfWeek = System.DayOfWeek.Wednesday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Thursday:
                            DayOfWeek = System.DayOfWeek.Thursday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Friday:
                            DayOfWeek = System.DayOfWeek.Friday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Saturday:
                            DayOfWeek = System.DayOfWeek.Saturday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Sunday:
                            DayOfWeek = System.DayOfWeek.Sunday.ToString().TranslateString();
                            break;
                    }
                    dr[Att_ChangeRosterGroupEntity.FieldNames.DayOfWeek] = DayOfWeek;
                    dt.Rows.Add(dr);
                }

            }
            else
            {
                var lstRosterGroup = new List<RosterGroupModel>();
                var lstShift = new List<Cat_Shift>();
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    lstRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= EndMonth && m.DateEnd >= BeginMonth)
                        .Select(m => new RosterGroupModel()
                        {
                            ID = m.ID,
                            RosterGroupName = m.RosterGroupName,
                            DateStart = m.DateStart.Value,
                            DateEnd = m.DateEnd.Value,
                            MonShiftID = m.MonShiftID,
                            TueShiftID = m.TueShiftID,
                            WedShiftID = m.WedShiftID,
                            ThuShiftID = m.ThuShiftID,
                            FriShiftID = m.FriShiftID,
                            SatShiftID = m.SatShiftID,
                            SunShiftID = m.SunShiftID
                        })
                        .ToList();
                    lstShift = unitOfWork.CreateQueryable<Cat_Shift>().ToList();
                }

                for (DateTime datecheck = BeginMonth; datecheck < EndMonth; datecheck = datecheck.AddDays(1))
                {
                    DataRow dr = dt.NewRow();
                    var lstRosterGroupByDate = lstRosterGroup.Where(m => m.DateStart <= datecheck && m.DateEnd >= datecheck).ToList();
                    dr[Att_ChangeRosterGroupEntity.FieldNames.DayOfMonth] = datecheck.ToString("dd/MM/yyyy");
                    string DayOfWeek = string.Empty;
                    switch (datecheck.DayOfWeek)
                    {
                        case System.DayOfWeek.Monday:
                            DayOfWeek = System.DayOfWeek.Monday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Tuesday:
                            DayOfWeek = System.DayOfWeek.Tuesday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Wednesday:
                            DayOfWeek = System.DayOfWeek.Wednesday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Thursday:
                            DayOfWeek = System.DayOfWeek.Thursday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Friday:
                            DayOfWeek = System.DayOfWeek.Friday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Saturday:
                            DayOfWeek = System.DayOfWeek.Saturday.ToString().TranslateString();
                            break;
                        case System.DayOfWeek.Sunday:
                            DayOfWeek = System.DayOfWeek.Sunday.ToString().TranslateString();
                            break;
                    }
                    dr[Att_ChangeRosterGroupEntity.FieldNames.DayOfWeek] = DayOfWeek;

                    foreach (var ShiftCode in lstShiftCode)
                    {
                        Guid ShiftID = lstShift.Where(m => m.Code == ShiftCode).Select(m => m.ID).FirstOrDefault();
                        if (ShiftID == null || ShiftID == Guid.Empty)
                            continue;

                        string NameOfGroup = string.Empty;
                        switch (datecheck.DayOfWeek)
                        {
                            case System.DayOfWeek.Monday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.MonShiftID != null && m.MonShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Tuesday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.TueShiftID != null && m.TueShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Wednesday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.WedShiftID != null && m.WedShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Thursday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.ThuShiftID != null && m.ThuShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Friday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.FriShiftID != null && m.FriShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Saturday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.SatShiftID != null && m.SatShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                            case System.DayOfWeek.Sunday:
                                NameOfGroup = lstRosterGroupByDate.Where(m => m.SunShiftID != null && m.SunShiftID.Value == ShiftID).Select(m => m.RosterGroupName).FirstOrDefault();
                                break;
                        }
                        dr[ShiftCode] = NameOfGroup;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;

        }
        public DataTable GetSchemaRosterGroup(List<string> lstCodeShift)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(Att_ChangeRosterGroupEntity.FieldNames.DayOfMonth, typeof(string));
            dt.Columns.Add(Att_ChangeRosterGroupEntity.FieldNames.DayOfWeek, typeof(string));
            foreach (var Code in lstCodeShift)
            {
                dt.Columns.Add(Code, typeof(string));
            }
            return dt;
        }
        public void SaveRosterGroup(DateTime MonthYear, DataTable dt)
        {
            DateTime BeginMonth = MonthYear;
            DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
            //For qua list Nhosm
            //For qua ngafy
            //Trong luc for qua ngay thi tach tuan
            //<Tên Nhóm Ca, <Ngày, Ca>>
            List<Cat_Shift> lstShift = lstShift_RosterGroup;
            List<RosterGroupTableModel> lstRosterTable = new List<RosterGroupTableModel>();
            foreach (DataRow dataRow in dt.Rows)
            {
                DateTime Date = Common.ConvertStringToDateTime(dataRow[ConstantMessages.DayOfMonth].ToString(), "dd/MM/yyyy");
                foreach (var shift in lstShift)
                {
                    if (dataRow[shift.Code] != null && dataRow[shift.Code].ToString() != string.Empty)
                    {
                        string RosterGroupName = dataRow[shift.Code].ToString();
                        RosterGroupTableModel GroupTable = new RosterGroupTableModel();
                        GroupTable.Date = Date;
                        GroupTable.RosterGroupName = RosterGroupName;
                        GroupTable.ShifID = shift.ID;
                        lstRosterTable.Add(GroupTable);
                    }
                }
            }

            List<string> lstNameOfGroup = lstName_RosterGroup;

            List<Att_RosterGroup> lstRosterGroup_Insert = new List<Att_RosterGroup>();

            foreach (var item in lstNameOfGroup)
            {
                List<RosterGroupTableModel> lstRosterTableByName = lstRosterTable.Where(m => m.RosterGroupName == item).OrderBy(m => m.Date).ToList();
                for (DateTime dateCheck = BeginMonth; dateCheck < EndMonth; dateCheck = dateCheck.AddDays(1))
                {
                    DateTime BeginWeek = dateCheck;
                    DateTime EndOfWeek = dateCheck;
                    Common.GetStartEndWeek(dateCheck, out BeginWeek, out EndOfWeek);
                    EndOfWeek = EndOfWeek.Date;
                    if (EndOfWeek > EndMonth)
                        EndOfWeek = EndMonth;
                    if (BeginWeek < BeginMonth)
                        BeginWeek = BeginMonth;

                    Att_RosterGroup RosterGroup = lstRosterGroup_Insert.Where(m => m.RosterGroupName == item && m.DateStart == BeginWeek).FirstOrDefault();
                    if (RosterGroup == null)
                    {
                        RosterGroup = new Att_RosterGroup();
                        RosterGroup.RosterGroupName = item;
                        RosterGroup.DateStart = BeginWeek;
                        RosterGroup.DateEnd = EndOfWeek;
                        lstRosterGroup_Insert.Add(RosterGroup);
                    }

                    RosterGroupTableModel rosterGroupInTable = lstRosterTableByName.Where(m => m.Date == dateCheck && m.ShifID != null && m.ShifID != Guid.Empty).FirstOrDefault();
                    if (rosterGroupInTable != null)
                    {
                        switch (dateCheck.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                RosterGroup.MonShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Tuesday:
                                RosterGroup.TueShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Wednesday:
                                RosterGroup.WedShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Thursday:
                                RosterGroup.ThuShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Friday:
                                RosterGroup.FriShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Saturday:
                                RosterGroup.SatShiftID = rosterGroupInTable.ShifID;
                                break;
                            case DayOfWeek.Sunday:
                                RosterGroup.SunShiftID = rosterGroupInTable.ShifID;
                                break;
                        }
                    }

                }
            }
            //Save, Delete cai cu trong Db
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                unitOfWork.SetIsDelete<Att_RosterGroup>(context.Att_RosterGroup.Where(m => m.DateStart <= EndMonth && m.DateEnd >= BeginMonth));
                unitOfWork.AddObject(lstRosterGroup_Insert.ToArray());
                unitOfWork.SaveChanges();
            }


        }


    }
}
