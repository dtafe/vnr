using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
//using HRM.Business.HrmSystem.Domain;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Infrastructure.Utilities.Helper;
using System.Threading;
using HRM.Business.Insurance.Models;
using HRM.Business.Evaluation.Models;
using VnResource.Helper;
using VnResource.Helper.Data;
using System.Collections;
using HRM.Business.HrmSystem.Domain;

namespace HRM.Business.Payroll.Domain
{
    public class Sal_SalaryDepartmentServices : BaseService
    {
        #region Xử lý lương Bộ phận mới 2014

        public void ComputeDepartment(Guid departmentID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                var repoSal_SalaryDepartment = new CustomBaseRepository<Sal_SalaryDepartment>(unitOfWork);
                var repoSal_SalaryDepartmentItem = new CustomBaseRepository<Sal_SalaryDepartmentItem>(unitOfWork);
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDurationEntity>(unitOfWork);

                Sal_SalaryDepartment item = new Sal_SalaryDepartment();
                item = repoSal_SalaryDepartment.GetById(departmentID);

                //xóa các item nếu đã tính trước đó
                List<Sal_SalaryDepartmentItem> listDepartmentItem = new List<Sal_SalaryDepartmentItem>();
                listDepartmentItem = repoSal_SalaryDepartmentItem.GetAll().Where(m => m.SalaryDepartmentID == departmentID).ToList();
                repoSal_SalaryDepartmentItem.Delete(listDepartmentItem);
                unitOfWork.SaveChanges();

                //nếu chọn theo cutoff thì lấy datefrom dateto của cutoff
                if (item.CutOffDurationID != null)
                {
                    var cutoff= repoAtt_CutOffDuration.GetById((Guid)item.CutOffDurationID);
                    item.MonthYear = cutoff.MonthYear;
                    item.DateFrom = cutoff.DateStart;
                    item.DateTo = cutoff.DateEnd;
                }

                Hashtable hasTable = new Hashtable();
                List<Sal_SalaryDepartment> listSalaryDepartment = new List<Sal_SalaryDepartment>(); 
                listSalaryDepartment.Add(item);

                ComputeSalaryDepartment(item.MonthYear, (DateTime)item.DateFrom, (DateTime)item.DateTo, listSalaryDepartment, ref hasTable);

              
            }
        }

        public Hashtable ComputeSalaryDepartment(DateTime monthYear, DateTime DateStart, DateTime DateEnd, List<Sal_SalaryDepartment> lstSalDep, ref Hashtable htSalGroup)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();

                #region Ver8 chưa dùng
                //String enumFomula = PayrollComputeMethod.E_FORMULA.ToString();
                //var responCatGradeCfg = new CustomBaseRepository<Cat_GradeCfg>(unitOfWork);
                //var lstGradeCfg = responCatGradeCfg.GetAll().Where(gr => gr.IsDelete == null && gr.Formula == enumFomula).ToList();
                //if (lstGradeCfg.Count <= 0)
                //    return htSalGroup;
                #endregion

                if (lstSalDep == null)
                {
                    var responSalDept = new CustomBaseRepository<Sal_SalaryDepartment>(unitOfWork);
                    lstSalDep = responSalDept.FindBy(ls => ls.MonthYear == monthYear && ls.IsDelete == null).ToList();
                }
                List<Guid> lstGuidSalDep = lstSalDep.Select(sal => sal.ID).ToList();
                var responSalDeptItem = new CustomBaseRepository<Sal_SalaryDepartmentItem>(unitOfWork);
                List<Sal_SalaryDepartmentItem> lstSalDepItem = responSalDeptItem.FindBy(ls => lstGuidSalDep.Contains(ls.SalaryDepartmentID) && ls.IsDelete == null).ToList();

                //Tạo list chứa ds chi tiết lương BP đã tính trước đó => để xóa đi, update mới
                List<Sal_SalaryDepartmentItem> lstItemDelete = new List<Sal_SalaryDepartmentItem>();

                #region ver8 chưa dùng
                //lấy chu kỳ lương theo chế độ lương
                //var responAppconfig = new CustomBaseRepository<Sys_AppConfig>(unitOfWork);
                //List<Sys_AppConfig> lstappConfig = responAppconfig.GetAll().Where(ap => ap.IsDelete == null).ToList();
                //string type = AppConfig.E_RANGE_SALARY_MONTH.ToString();
                //Sys_AppConfig sys_AppConfig = lstappConfig.Where(sy => sy.Info == type).FirstOrDefault();
                //AttendanceService.GetRangeMaxMinGrade(lstGradeCfg, monthYear, out DateStart, out DateEnd);
                #endregion

                //AppConfig
                //var service = new Sys_AttOvertimePermitConfigServices();
                var value52 = Sys_Services.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE52); ; // service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE52);

                /*XỬ LÝ THEO 2 OPTION */
                //sử dụng hệ số từ lương cơ bản, bổ sung NV để tính lương với nhiều trạng thái (XC 1.1)
                if (value52 == AppConfig.E_RATE_BY_BASICSALARY.ToString())
                {
                    var responOrgAll = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                    List<Cat_OrgStructure> listOrgAll = responOrgAll.FindBy(org => org.IsDelete == null).ToList();
                    htSalGroup = ComputeSalGroup(unitOfWork, monthYear, lstSalDep, lstSalDepItem, ref lstItemDelete, DateStart, DateEnd,  listOrgAll);
                }
                //sử dụng hệ số từ màn hình nhập hệ số, nhân viên chi tiết trong table Sal_DepartmentItem(XC 2.1)
                else
                {
                    //Chỉ sử dụng: "lstSalDepItem" để lấy danh sách nhân viên và hệ số của nhân viên
                    htSalGroup = ComputeSalGroup(unitOfWork, monthYear, lstSalDep, lstSalDepItem, ref lstItemDelete, DateStart, DateEnd);
                }
                BaseService service = new BaseService();
                service.Delete<Sal_SalaryDepartmentItem>(lstItemDelete);
                //EntityService.SaveChanges(Guid.Empty);
                unitOfWork.SaveChanges();
                return htSalGroup;
            }
        }

        #region XuChi 1.1...Sử dụng hệ số lương cơ bản -->Kiểm tra các trạng thái nhân viên (new, edit, remove, fix)
        public Hashtable ComputeSalGroup(IUnitOfWork unitOfWork, DateTime monthYear, List<Sal_SalaryDepartment> lstSalDep, List<Sal_SalaryDepartmentItem> lstSalDepItem, ref List<Sal_SalaryDepartmentItem> lstItemDelete,
                                         DateTime DateStart, DateTime DateEnd, List<Cat_OrgStructure> listOrg)
        {
                #region Query Data

                Hashtable htSalGroup = new Hashtable();
                //Nhân viên
                var reposProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);

                //Nhân viên
                var reposSalaryDepartmentItem = new CustomBaseRepository<Sal_SalaryDepartmentItem>(unitOfWork);

                //Bảng công
                var reposAttendanceTb = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);

                //Bảng công chi tiết
                var reposAttendanceTbItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);

                //Ca làm việc
                var reposAttRoster = new CustomBaseRepository<Att_Roster>(unitOfWork);

                //Lương cơ bản
                var reposBasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);

                //Chế độ lương
                var reposSalGrade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
                #endregion

                foreach (Sal_SalaryDepartment salDep in lstSalDep)
                {
                    List<Sal_SalaryDepartmentItem> rsDepItem = new List<Sal_SalaryDepartmentItem>();
                    DateStart = salDep.DateFrom != null ? salDep.DateFrom.Value : DateStart;
                    DateEnd = salDep.DateTo != null ? salDep.DateTo.Value : DateEnd;

                    List<Guid> guidPro = new List<Guid>();
                    List<Guid> guidOrgAllChild = new List<Guid>();
                    List<Att_Roster> lstRosterLine = new List<Att_Roster>();

                    #region Lấy danh sách nhân viên trong phòng ban hoặc chuyền
                    if (salDep.TypeCompute == MethodComputeGroupSalary.E_COMPUTE_LINE.ToString())
                    {
                        lstRosterLine = reposAttRoster.FindBy(rs => rs.ProductionLineID == salDep.ProductionLineID &&
                                                              rs.IsDelete == null && rs.DateStart <= DateEnd && rs.DateEnd >= DateStart).ToList();
                        guidPro = lstRosterLine.Select(wh => wh.ProfileID).ToList();
                    }
                    else
                    {
                        if (salDep.OrgStructureID == null)
                            continue;
                        List<Cat_OrgStructure> lstChildsStructure = null;
                        Cat_OrgStructure DepartmentFilter = listOrg.FirstOrDefault(p => p.ID == salDep.OrgStructureID);
                        if (DepartmentFilter != null)
                            lstChildsStructure = GetChilds(listOrg, DepartmentFilter.ID);
                        else
                            lstChildsStructure = listOrg.Where(o => o.ID == salDep.OrgStructureID && o.IsDelete == null).ToList();
                        guidOrgAllChild = lstChildsStructure.Select(s => s.ID).Distinct().ToList();

                        //Lấy tất cả ID nhân viên trong các phòng ban trên.
                        guidPro = reposProfile.FindBy(hr => hr.IsDelete == null && guidOrgAllChild.Contains(hr.OrgStructureID.Value)).Select(hr => hr.ID).ToList();
                    }
                    #endregion

                    //Danh sách NV bao gồm các trạng thái (new, edit, fix, remove)
                    string stNEW = EnumDropDown.StatusLoadEnumSalDepartItem.E_NEW.ToString();
                    string stEDIT = EnumDropDown.StatusLoadEnumSalDepartItem.E_EDIT.ToString();
                    string stREMOVE = EnumDropDown.StatusLoadEnumSalDepartItem.E_REMOVE.ToString();
                    string stFIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_FIX.ToString();
                    string stEDIT_FIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_EDIT_FIX.ToString();
                    string[] strStatus = new string[] { stNEW, stREMOVE, stEDIT, stFIX, stEDIT_FIX };

                    var lstSalDepItemStatus = lstSalDepItem.Where(st => st.SalaryDepartmentID == salDep.ID && !String.IsNullOrEmpty(st.Status) && strStatus.Contains(st.Status)).ToList();
                    List<Guid> lstProAdd = new List<Guid>();
                    List<Guid> lstProFix = new List<Guid>();
                    List<Guid> lstProEdit = new List<Guid>();
                    List<Sal_SalaryDepartmentItem> lstItemRemove = new List<Sal_SalaryDepartmentItem>();

                    if (lstSalDepItemStatus.Count > 0)
                    {
                        #region LẤY RA DS NHÂN VIÊN VỚI CÁC TRẠNG THÁI
                        //Nếu tồn tại nhân viên là Fix, thì sẽ chỉ tính những profile có trạng thái fix, bỏ qua các trạng thái khác.
                        //Bổ sung thêm trạng thái Edit_Fix
                        lstProFix = lstSalDepItemStatus.Where(st => st.Status == stFIX || st.Status == stEDIT_FIX).Select(st => st.ProfileID).ToList();

                        //Lấy danh sách NV ko phải trạng thái fix trong departmentitem để xóa đi ( vì chỉ tính lương cho NV có Fix)
                        List<Sal_SalaryDepartmentItem> lstSalDepItemStatusNotFix = lstSalDepItem.Where(st => st.SalaryDepartmentID == salDep.ID && st.IsDelete == null).ToList();
                        lstSalDepItemStatusNotFix = lstSalDepItemStatusNotFix.Where(st => st.Status == null || (st.Status != stFIX && st.Status != stEDIT_FIX)).ToList();

                        if (lstProFix != null && lstProFix.Count > 0)
                        {
                            //Lấy những nhân viên không phải là Fix đưa vào trong lstItemDelete để xóa đi.
                            if (lstSalDepItemStatusNotFix.Count > 0)
                                lstItemDelete.AddRange(lstSalDepItemStatusNotFix);

                            //Chỉ add những nhân viên có Fix vào để tính.
                            guidPro.Clear();
                            foreach (Guid id in lstProFix)
                            {
                                guidPro.Add(id);
                            }
                        }
                        //Nếu ko có nhân viên nào là Fix, bắt đầu kiểm tra: New, edit, remove
                        else
                        {
                            //New
                            lstProAdd = lstSalDepItemStatus.Where(st => st.Status == stNEW).Select(st => st.ProfileID).ToList();
                            foreach (Guid id in lstProAdd)
                            {
                                if (!guidPro.Contains(id))
                                    guidPro.Add(id);
                            }

                            //Edit
                            lstProEdit = lstSalDepItemStatus.Where(st => st.Status == stEDIT).Select(st => st.ProfileID).ToList();
                            foreach (Guid id in lstProEdit)
                            {
                                if (!guidPro.Contains(id))
                                    guidPro.Add(id);
                            }

                            //Remove
                            lstItemRemove = lstSalDepItemStatus.Where(st => st.Status == stREMOVE).ToList();
                            List<Guid> lstProRemove = lstItemRemove.Select(st => st.ProfileID).ToList();
                            foreach (Guid id in lstProRemove)
                            {
                                if (guidPro.Contains(id))
                                    guidPro.Remove(id);
                            }
                        }
                        #endregion
                    }

                    //Lấy grade theo ds nhân viên
                    var lstGradeAll = reposSalGrade.FindBy(gr => gr.IsDelete == null && guidPro.Contains(gr.ProfileID)).ToList();

                    //Lấy Lương cơ bản theo ds nhân viên
                    var lstBasicSal = reposBasicSalary.FindBy(sal => sal.IsDelete == null && sal.DateOfEffect <= DateEnd && guidPro.Contains(sal.ProfileID)).ToList(); 

                    //Lấy roster theo ds nhân viên
                    var lstAllRoster = reposAttRoster.FindBy(rs => guidPro.Contains(rs.ProfileID) && rs.IsDelete == null && rs.DateStart <= DateEnd && rs.DateEnd >= DateStart)
                                                     .Select(rs => new { rs.ID, rs.ProfileID, rs.DateStart, rs.DateEnd, rs.SalaryType, rs.Cat_Shift }).ToList();
                    //Lấy bảng công theo ds nv
                    var lstAttAtendance = reposAttendanceTb.FindBy(att => att.IsDelete == null && att.MonthYear == monthYear && guidPro.Contains(att.ProfileID)).ToList();
                    var lstAttAtendanceID = lstAttAtendance.Select(att => att.ID).ToList();

                    //Lấy bảng công chi tiết
                    var lstAttAttendanceItem = reposAttendanceTbItem.FindBy(it => it.IsDelete == null && lstAttAtendanceID.Contains(it.AttendanceTableID)).ToList();

                    //Sau khi tiến hành các bước trên, sẽ có 1 danh sách nhân viên cuối cùng để xử lý.
                    var lstProfileOrg = reposProfile.FindBy(hr => guidPro.Contains(hr.ID)).ToList();

                    #region " Nếu tồn tại trạng thái Fix"
                    if (lstProFix != null && lstProFix.Count > 0)
                    {
                        foreach (Hre_Profile _pro in lstProfileOrg)
                        {
                            //Trạng thái: EDIT_FIX
                            //Những nhân viên đã được cố định và chỉnh sửa: Hệ số, giờ làm việc, OT thì sẽ lưu lại trong sal_departmentitem, và sẽ được lấy lên từ đây để tính, ko thông qua basicsalary nữa.
                            Sal_SalaryDepartmentItem itemProEditFix = new Sal_SalaryDepartmentItem();
                            if (lstProFix.Contains(_pro.ID))
                            {
                                itemProEditFix = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID &&
                                                                           it.Status == stEDIT_FIX).FirstOrDefault();
                                if (itemProEditFix != null)
                                {
                                    rsDepItem.Add(itemProEditFix);
                                    continue;
                                }
                            }

                            Sal_SalaryDepartmentItem itemProFix = new Sal_SalaryDepartmentItem(); ;
                            itemProFix = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID && it.Status == stFIX).FirstOrDefault();

                            #region Ver8 chưa dùng
                            //Sal_Grade grade = GradeDAO.GetGrade(lstGradeAll, _pro.ID, DateEnd);
                            //if (grade == null)
                            //{
                            //    continue;
                            //}
                            //Cat_GradeCfg gradeCfg = lstGradeCfg.Where(gra => gra.ID == grade.GradeID).FirstOrDefault();
                            //if (gradeCfg == null || gradeCfg.Formula != enumFomula)
                            //    continue;
                            //AttendanceService.GetSalaryDateRange(gradeCfg, sys_AppConfig, null, monthYear, out DateStart, out DateEnd);
                            //if (salDep.DateFrom != null && salDep.DateFrom.Value >= DateStart && salDep.DateFrom.Value <= DateEnd)
                            //{
                            //    DateStart = salDep.DateFrom.Value;
                            //}
                            //if (salDep.DateTo != null && salDep.DateTo.Value >= DateStart && salDep.DateTo.Value <= DateEnd)
                            //{
                            //    DateEnd = salDep.DateTo.Value;
                            //}
                            #endregion

                            //dữ liệu công
                            var _att = lstAttAtendance.Where(att => att.ProfileID == _pro.ID).Select(att => new { att.ID }).FirstOrDefault();
                            if (_att == null)
                                continue;
                            var _attLstItem1 = lstAttAttendanceItem.Where(at => at.AttendanceTableID == _att.ID).ToList();

                            List<Att_AttendanceTableItem> _attLstItem = new List<Att_AttendanceTableItem>();
                            #region Xử lý trường hợp 1 tháng có 2 cách tính lương ( lương bộ phận và lương thời gian). Vì vậy cần kiểm tra roster để lấy công cho phù hợp.
                            if (lstAllRoster.Count > 0)
                            {
                                var roster = lstAllRoster.Where(ro => ro.ProfileID == _pro.ID && ro.DateStart.Year == monthYear.Year
                                                                                              && ro.DateEnd.Month >= monthYear.Month && ro.DateStart.Month <= monthYear.Month
                                                                                              && ro.SalaryType != null && ro.SalaryType == SalaryRosterType.E_DEPTSAL.ToString()).FirstOrDefault();
                                if (roster != null && roster.Cat_Shift != null)
                                {
                                    _attLstItem = _attLstItem1.Where(it => it.ShiftID.HasValue && it.ShiftID.Value == roster.Cat_Shift.ID
                                                                                               && it.WorkDate >= roster.DateStart && it.WorkDate <= roster.DateEnd
                                                                                               && it.DutyCode == DutyCode.E_ON.ToString()).ToList();
                                }
                                else //Trừ bớt các ngày công là timesal
                                {
                                    var rostertimesal = lstAllRoster.Where(ro => ro.ProfileID == _pro.ID && ro.DateStart.Year == monthYear.Year
                                                                                                         && ro.DateEnd.Month >= monthYear.Month && ro.DateStart.Month <= monthYear.Month
                                                                                                         && ro.SalaryType != null && ro.SalaryType == SalaryRosterType.E_TIMESAL.ToString()).FirstOrDefault();
                                    if (rostertimesal != null && rostertimesal.Cat_Shift != null)
                                    {
                                        List<Att_AttendanceTableItem> lstattitemtimesal = _attLstItem1.Where(it => it.ShiftID.HasValue && it.ShiftID.Value == rostertimesal.Cat_Shift.ID
                                                                                                             && it.WorkDate >= rostertimesal.DateStart && it.WorkDate <= rostertimesal.DateEnd
                                                                                                             && it.DutyCode == DutyCode.E_ON.ToString()).ToList();
                                        foreach (Att_AttendanceTableItem it in lstattitemtimesal)
                                        {
                                            if (it != null)
                                                _attLstItem1.Remove(it);
                                        }
                                    }
                                }
                            }
                            if (_attLstItem.Count < 1)
                                _attLstItem = _attLstItem1;
                            #endregion

                            List<Sal_BasicSalary> lstBasicSalPro = lstBasicSal.Where(sal => sal.ProfileID == _pro.ID).OrderByDescending(sal => sal.DateOfEffect).ToList();

                            //Xóa những nhân viên được Fix có lương cơ bản đã đc tính trước đó, để tính lại.
                            if (lstBasicSalPro != null && lstBasicSalPro.Count > 0)
                            {
                                List<Sal_SalaryDepartmentItem> lstItemFix = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID
                                                                            && (it.Status == stFIX
                                                                            || it.Status == stEDIT_FIX)).ToList();
                                if (lstItemFix != null && lstItemFix.Count > 0)
                                    lstItemDelete.AddRange(lstItemFix);
                            }

                            //Bắt đầu thêm giờ công, hệ số, giờ OT lần lượt cho từng người (XC 1.2)
                            if (salDep.TypeCompute == MethodComputeGroupSalary.E_COMPUTE_LINE.ToString())
                            {
                                List<Att_Roster> lstRosterPro = lstRosterLine.Where(rs => rs.ProfileID == _pro.ID).ToList();
                                rsDepItem.AddRange(CalSalaryLineItemProfile(salDep, _attLstItem, _pro, lstBasicSalPro, lstRosterPro, DateStart, DateEnd, monthYear, itemProFix));
                            }
                            else
                            {
                                rsDepItem.AddRange(CalSalaryDepartmentItemProfile(salDep, _attLstItem, _pro, lstBasicSalPro, DateStart, DateEnd, monthYear, itemProFix));
                            }
                        }
                    }
                    #endregion
                    #region " Không tồn tại trạng thái Fix"
                    else
                    {
                        //test
                        //lstProfileOrg = lstProfileOrg.Where(hr => hr.CodeEmp == "VPM183").ToList();
                        //end test
                        foreach (Hre_Profile _pro in lstProfileOrg)
                        {
                            //Trạng thái: NEW
                            //Những nhân viên trong trạng thái này sẽ ko thuộc phòng ban hoặc chuyền ban đầu. Khi đó các hệ số, giờ làm việc, OT sẽ được thêm vào và lưu trong sal_departmentitem
                            Sal_SalaryDepartmentItem itemProAdd = new Sal_SalaryDepartmentItem();
                            if (lstProAdd.Contains(_pro.ID))
                            {
                                itemProAdd = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID &&
                                                                       it.Status == stNEW).FirstOrDefault();
                                rsDepItem.Add(itemProAdd);
                                continue;
                            }

                            //Trạng thái: EDIT
                            //Những nhân viên đã được chỉnh sửa: Hệ số, giờ làm việc, OT thì sẽ lưu lại trong sal_departmentitem, và sẽ được lấy lên từ đây để tính, ko thông qua basicsalary nữa.
                            if (lstProEdit.Contains(_pro.ID))
                            {
                                itemProAdd = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID &&
                                                                       it.Status == stEDIT).FirstOrDefault();
                                rsDepItem.Add(itemProAdd);
                                continue;
                            }

                            #region Ver8 chưa dùng
                            //Sal_Grade grade = GradeDAO.GetGrade(lstGradeAll, _pro.ID, DateEnd);
                            //if (grade == null)
                            //{
                            //    continue;
                            //}
                            //Cat_GradeCfg gradeCfg = lstGradeCfg.Where(gra => gra.ID == grade.GradeID).FirstOrDefault();
                            //if (gradeCfg == null || gradeCfg.Formula != enumFomula)
                            //    continue;
                            //List<Sal_SalaryDepartmentItem> lstItemPro = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID
                            //                                            && (String.IsNullOrEmpty(it.Status) || !strStatus.Contains(it.Status))).ToList();
                            //lstItemDelete.AddRange(lstItemPro);

                            //AttendanceService.GetSalaryDateRange(gradeCfg, sys_AppConfig, null, monthYear, out DateStart, out DateEnd);
                            //if (salDep.DateFrom != null && salDep.DateFrom.Value >= DateStart && salDep.DateFrom.Value <= DateEnd)
                            //{
                            //    DateStart = salDep.DateFrom.Value;
                            //}
                            //if (salDep.DateTo != null && salDep.DateTo.Value >= DateStart && salDep.DateTo.Value <= DateEnd)
                            //{
                            //    DateEnd = salDep.DateTo.Value;
                            //}
                            #endregion

                            //dữ liệu công
                            var _att = lstAttAtendance.Where(att => att.ProfileID == _pro.ID).Select(att => new { att.ID }).FirstOrDefault();
                            if (_att == null)
                                continue;
                            var _attLstItem1 = lstAttAttendanceItem.Where(at => at.AttendanceTableID == _att.ID).ToList();

                            List<Att_AttendanceTableItem> _attLstItem = new List<Att_AttendanceTableItem>();
                            #region Xử lý trường hợp 1 tháng có 2 cách tính lương ( lương bộ phận và lương thời gian). Vì vậy cần kiểm tra roster để lấy công cho phù hợp.
                            if (lstAllRoster.Count > 0)
                            {
                                var roster = lstAllRoster.Where(ro => ro.ProfileID == _pro.ID && ro.DateStart.Year == monthYear.Year
                                                                                              && ro.DateEnd.Month >= monthYear.Month && ro.DateStart.Month <= monthYear.Month
                                                                                              && ro.SalaryType != null && ro.SalaryType == SalaryRosterType.E_DEPTSAL.ToString()).FirstOrDefault();
                                if (roster != null && roster.Cat_Shift != null)
                                {
                                    _attLstItem = _attLstItem1.Where(it => it.ShiftID.HasValue && it.ShiftID.Value == roster.Cat_Shift.ID
                                                                                               && it.WorkDate >= roster.DateStart && it.WorkDate <= roster.DateEnd
                                                                                               && it.DutyCode == DutyCode.E_ON.ToString()).ToList();
                                }
                                else //Trừ bớt các ngày công là timesal
                                {
                                    var rostertimesal = lstAllRoster.Where(ro => ro.ProfileID == _pro.ID && ro.DateStart.Year == monthYear.Year
                                                                                                         && ro.DateEnd.Month >= monthYear.Month && ro.DateStart.Month <= monthYear.Month
                                                                                                         && ro.SalaryType != null && ro.SalaryType == SalaryRosterType.E_TIMESAL.ToString()).FirstOrDefault();
                                    if (rostertimesal != null && rostertimesal.Cat_Shift != null)
                                    {
                                        List<Att_AttendanceTableItem> lstattitemtimesal = _attLstItem1.Where(it => it.ShiftID.HasValue && it.ShiftID.Value == rostertimesal.Cat_Shift.ID
                                                                                                             && it.WorkDate >= rostertimesal.DateStart && it.WorkDate <= rostertimesal.DateEnd
                                                                                                             && it.DutyCode == DutyCode.E_ON.ToString()).ToList();
                                        foreach (Att_AttendanceTableItem it in lstattitemtimesal)
                                        {
                                            if (it != null)
                                                _attLstItem1.Remove(it);
                                        }
                                    }
                                }
                            }
                            if (_attLstItem.Count < 1)
                                _attLstItem = _attLstItem1;
                            #endregion

                            List<Sal_BasicSalary> lstBasicSalPro = lstBasicSal.Where(sal => sal.ProfileID == _pro.ID).OrderByDescending(sal => sal.DateOfEffect).ToList();

                            //Bắt đầu thêm giờ công, hệ số, giờ OT lần lượt cho từng người (XC 1.2)
                            if (salDep.TypeCompute == MethodComputeGroupSalary.E_COMPUTE_LINE.ToString())
                            {
                                List<Att_Roster> lstRosterPro = lstRosterLine.Where(rs => rs.ProfileID == _pro.ID).ToList();
                                rsDepItem.AddRange(CalSalaryLineItemProfile(salDep, _attLstItem, _pro, lstBasicSalPro, lstRosterPro, DateStart, DateEnd, monthYear, itemProAdd));
                            }
                            else
                            {
                                rsDepItem.AddRange(CalSalaryDepartmentItemProfile(salDep, _attLstItem, _pro, lstBasicSalPro, DateStart, DateEnd, monthYear, itemProAdd));
                            }
                        }
                    }
                    #endregion

                    //Trả về 0 các trạng thái REMOVE đã tính lần đầu
                    lstItemRemove.ForEach(it => it.AmoutSalary = 0);

                    //Tính toán tiền lương và các loại tiền OT (XC Finished)
                    ComputeAmountPro(rsDepItem, salDep);
                    foreach (var t in rsDepItem)
                    {
                        t.ID = Guid.NewGuid();
                        reposSalaryDepartmentItem.Add(t);
                    }
                    //View lên lưới những nhân viên đã chuyển qua trạng thái là E_REMOVE.
                    //Mục đích: Nếu muốn tính lại cho những NV này thì edit chuyển trạng thái lại để tính.
                    rsDepItem.AddRange(lstItemRemove);

                    htSalGroup.Add(salDep, rsDepItem);
                }
                return htSalGroup;
        }
        #endregion

        #region XuChi 1.2...Xử lý Tính giờ công, hệ số, giờ OT lần lượt cho từng người theo hệ số lương CB
        public List<Sal_SalaryDepartmentItem> CalSalaryDepartmentItemProfile(Sal_SalaryDepartment salDepartment, List<Att_AttendanceTableItem> lstAttItemPro, Hre_Profile pro,
                                                                             List<Sal_BasicSalary> lstBasicSalPro, DateTime dfrom, DateTime dto, DateTime monthYear,
                                                                             Sal_SalaryDepartmentItem itemProAdd)
        {

            List<Sal_SalaryDepartmentItem> lstItem = new List<Sal_SalaryDepartmentItem>();
            Double ratePer = 0;

            //Lấy hệ số trong lương cơ bản
            Sal_BasicSalary basSal = new Sal_BasicSalary();
            if (lstBasicSalPro.Count == 2)
                basSal = lstBasicSalPro[1];
            else if (lstBasicSalPro.Count == 1)
                basSal = lstBasicSalPro[0];

            if (basSal != null && ratePer != basSal.PersonalRate)
            {
                ratePer = basSal.PersonalRate != null ? basSal.PersonalRate.Value : 0;
                Sal_SalaryDepartmentItem depItem = null;

                string stFIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_FIX.ToString();
                string stEDIT_FIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_EDIT_FIX.ToString();
                if (itemProAdd != null && itemProAdd.ID.ToString() != Guid.Empty.ToString() && itemProAdd.Status != stFIX && itemProAdd.Status != stEDIT_FIX)
                {
                    depItem = itemProAdd;
                }
                else
                {
                    depItem = new Sal_SalaryDepartmentItem();
                }
                //depItem.Sal_SalaryDepartment = salDepartment;
                depItem.SalaryDepartmentID = salDepartment.ID;
                depItem.DateFrom = dfrom;
                depItem.DateTo = dto;
                depItem.ProfileID = pro.ID;
                depItem.Rate = ratePer;
                if (itemProAdd != null)
                    depItem.Status = itemProAdd.Status;

                for (DateTime idx = dfrom; idx <= dto; idx = idx.AddDays(1))
                {
                    //(3.Use general: Insert chi tiết --> Gần Finished)
                    depItem = SetItemSalDep(depItem, lstAttItemPro, idx);
                }

                //AppConfig
                var service = new Sys_AttOvertimePermitConfigServices();
                var value56 =  service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE56);
                var value57 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE57);
                var value58 =  service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE58);

                //Nếu có check áp dụng trừ số giờ cho nữ
                string gender = EnumDropDown.Gender.E_FEMALE.ToString();
                if (value56.ToLower() == true.ToString().ToLower() && pro.Gender == gender)
                {
                    if (!value57.IsNullOrEmpty() && !value58.IsNullOrEmpty())
                    {
                        double hoursub = Convert.ToDouble(value57);
                        double daywork = Convert.ToDouble(value58);
                        if (hoursub > 0 && daywork > 0)
                        {
                            var lstAttItem = lstAttItemPro.Where(p => p.IsHavingPregTreatment == true).ToList();
                            //Nếu nhân viên không có thai sản
                            if (lstAttItem.Count <= 0)
                            {
                                if (depItem.PaidWorkHours / 8 > daywork)
                                    depItem.PaidWorkHours = depItem.PaidWorkHours - hoursub;
                            }
                        }
                    }
                }

                lstItem.Add(depItem);
            }
            return lstItem;
        }

        public List<Sal_SalaryDepartmentItem> CalSalaryLineItemProfile(Sal_SalaryDepartment salDepartment, List<Att_AttendanceTableItem> lstAttItemPro, Hre_Profile pro,
                                                                             List<Sal_BasicSalary> lstBasicSalPro, List<Att_Roster> lstRosterPro, DateTime dfrom, DateTime dto, DateTime monthYear,
                                                                             Sal_SalaryDepartmentItem itemProAdd)
        {
            List<Sal_SalaryDepartmentItem> lstItem = new List<Sal_SalaryDepartmentItem>();
            Double ratePer = 0;

            //Lấy hệ số trong lương cơ bản
            Sal_BasicSalary basSal = new Sal_BasicSalary();
            if (lstBasicSalPro.Count == 2)
                basSal = lstBasicSalPro[1];
            else if (lstBasicSalPro.Count == 1)
                basSal = lstBasicSalPro[0];

            if (ratePer != basSal.PersonalRate)
            {
                ratePer = basSal.PersonalRate != null ? basSal.PersonalRate.Value : 0;
                Sal_SalaryDepartmentItem depItem = null;

                string stFIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_FIX.ToString();
                string stEDIT_FIX = EnumDropDown.StatusLoadEnumSalDepartItem.E_EDIT_FIX.ToString();
                if (itemProAdd != null && itemProAdd.ID.ToString() != Guid.Empty.ToString() && itemProAdd.Status != stFIX && itemProAdd.Status != stEDIT_FIX)
                {
                    depItem = itemProAdd;
                }
                else
                {
                    depItem = new Sal_SalaryDepartmentItem();
                }
                depItem.Sal_SalaryDepartment = salDepartment;
                depItem.DateFrom = dfrom;
                depItem.DateTo = dto;
                depItem.ProfileID = pro.ID;
                depItem.Rate = ratePer;
                if (itemProAdd != null)
                    depItem.Status = itemProAdd.Status;

                for (DateTime idx = dfrom; idx <= dto; idx = idx.AddDays(1))
                {
                    //(3.Use general: Insert chi tiết --> Gần Finished)
                    depItem = SetItemSalDep(depItem, lstAttItemPro, idx);
                }

                //AppConfig
                var service = new Sys_AttOvertimePermitConfigServices();
                var value56 =  service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE56);
                var value57 =  service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE57);
                var value58 =  service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE58);

                //Nếu có check áp dụng trừ số giờ cho nữ
                string gender = EnumDropDown.Gender.E_FEMALE.ToString();
                if (value56.ToLower() == true.ToString().ToLower() && pro.Gender == gender)
                {
                    if (!value57.IsNullOrEmpty() && !value58.IsNullOrEmpty())
                    {
                        double hoursub = Convert.ToDouble(value57);
                        double daywork = Convert.ToDouble(value58);
                        if (hoursub > 0 && daywork > 0)
                        {
                            var lstAttItem = lstAttItemPro.Where(p => p.IsHavingPregTreatment == true).ToList();
                            //Nếu nhân viên không có thai sản
                            if (lstAttItem.Count <= 0)
                            {
                                if (depItem.PaidWorkHours / 8 > daywork)
                                    depItem.PaidWorkHours = depItem.PaidWorkHours - hoursub;
                            }
                        }
                    }
                }
                lstItem.Add(depItem);
            }
            return lstItem;
        }
        #endregion

        #region XuChi 2.1...Sử dụng hệ số cá nhân tự nhập
        public Hashtable ComputeSalGroup(IUnitOfWork unitOfWork, DateTime monthYear, List<Sal_SalaryDepartment> lstSalDep, List<Sal_SalaryDepartmentItem> lstSalDepItem, ref List<Sal_SalaryDepartmentItem> lstItemDelete,
                                          DateTime DateStart, DateTime DateEnd)
        {
            #region Query data
            Hashtable htSalGroup = new Hashtable();
            //Nhân viên
            var reposProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);

            //Bảng công
            var reposAttendanceTb = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);

            //Bảng công chi tiết
            var reposAttendanceTbItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);

            //Chế độ lương
            var reposSalGrade = new CustomBaseRepository<Sal_Grade>(unitOfWork);
            #endregion

            foreach (Sal_SalaryDepartment salDep in lstSalDep)
            {
                List<Sal_SalaryDepartmentItem> rsDepItem = new List<Sal_SalaryDepartmentItem>();

                DateStart = salDep.DateFrom != null ? salDep.DateFrom.Value : DateStart;
                DateEnd = salDep.DateTo != null ? salDep.DateTo.Value : DateEnd;

                List<Guid> guidPro = new List<Guid>();

                //Lấy dữ liệu nhân viên
                guidPro = lstSalDepItem.Select(dt => dt.ProfileID).ToList();
                var lstProfileOrg = reposProfile.FindBy(hr => hr.IsDelete == null && guidPro.Contains(hr.ID)).ToList();

                //Lấy grade theo ds nhân viên
                var lstGradeAll = reposSalGrade.FindBy(gr => gr.IsDelete == null && guidPro.Contains(gr.ProfileID)).ToList();

                //Lấy bảng công theo ds nv
                var lstAttAtendance = reposAttendanceTb.FindBy(att => att.IsDelete == null && att.MonthYear == monthYear && guidPro.Contains(att.ProfileID)).ToList();
                var lstAttAtendanceID = lstAttAtendance.Select(att => att.ID).ToList();

                //Lấy bảng công chi tiết
                var lstAttAttendanceItem = reposAttendanceTbItem.FindBy(it => it.IsDelete == null && lstAttAtendanceID.Contains(it.AttendanceTableID)).ToList();

                foreach (Hre_Profile _pro in lstProfileOrg)
                {
                    #region Ver8 chưa dùng
                    //Sal_Grade grade = GradeDAO.GetGrade(lstGradeAll, _pro.ID, DateEnd);
                    //if (grade == null)
                    //    continue;

                    //Cat_GradeCfg gradeCfg = lstGradeCfg.Where(gra => gra.ID == grade.GradeID).FirstOrDefault();
                    //if (gradeCfg == null || gradeCfg.Formula != enumFomula)
                    //    continue;

                    //AttendanceService.GetSalaryDateRange(gradeCfg, sys_AppConfig, null, monthYear, out DateStart, out DateEnd);
                    //if (salDep.DateFrom != null && salDep.DateFrom.Value >= DateStart && salDep.DateFrom.Value <= DateEnd)
                    //{
                    //    DateStart = salDep.DateFrom.Value;
                    //}
                    //if (salDep.DateTo != null && salDep.DateTo.Value >= DateStart && salDep.DateTo.Value <= DateEnd)
                    //{
                    //    DateEnd = salDep.DateTo.Value;
                    //}
                    #endregion

                    //dữ liệu công
                    var _att = lstAttAtendance.Where(att => att.ProfileID == _pro.ID).Select(att => new { att.ID }).FirstOrDefault();
                    if (_att == null)
                        continue;
                    var _attLstItem = lstAttAttendanceItem.Where(at => at.AttendanceTableID == _att.ID).ToList();

                    //Nếu nhân viên được tính lại thì sẽ cho vào danh sách để xóa đi tính lại.
                    List<Sal_SalaryDepartmentItem> lstItemPro = lstSalDepItem.Where(it => it.ProfileID == _pro.ID && it.SalaryDepartmentID == salDep.ID).ToList();
                    lstItemDelete.AddRange(lstItemPro);

                    //Bắt đầu thêm giờ công, hệ số, giờ OT lần lượt cho từng người
                    rsDepItem.AddRange(CalSalaryItemProfile(salDep, _attLstItem, _pro, lstItemPro, DateStart, DateEnd, monthYear));
                }

                //Tính toán tiền lương và các loại tiền OT (XC Finished)
                ComputeAmountPro(rsDepItem, salDep);
                htSalGroup.Add(salDep, rsDepItem);
            }
            return htSalGroup;
        }
        #endregion

        #region XuChi 2.2...Xử lý Tính giờ công, hệ số, giờ OT lần lượt cho từng người theo hệ số nhập
        public List<Sal_SalaryDepartmentItem> CalSalaryItemProfile(Sal_SalaryDepartment salDepartment, List<Att_AttendanceTableItem> lstAttItemPro, Hre_Profile pro,
                                         List<Sal_SalaryDepartmentItem> lstItemPro, DateTime dfrom, DateTime dto, DateTime monthYear)
        {
            List<Sal_SalaryDepartmentItem> lstItem = new List<Sal_SalaryDepartmentItem>();
            List<Sal_SalaryDepartmentItem> lstDepItemPro = lstItemPro.Where(p => p.ProfileID == pro.ID).ToList();
            Double ratePer = 0;

            //Lấy hệ số từ bảng Sal_SalaryDepartmentItem
            foreach (Sal_SalaryDepartmentItem rtDepItem in lstDepItemPro)
            {
                ratePer = rtDepItem.Rate != null ? rtDepItem.Rate : 0;
                Sal_SalaryDepartmentItem depItem = new Sal_SalaryDepartmentItem();
                //depItem.Sal_SalaryDepartment = salDepartment;
                depItem.SalaryDepartmentID = salDepartment.ID;
                depItem.DateFrom = dfrom;
                depItem.DateTo = dto;
                depItem.ProfileID = pro.ID;
                depItem.Rate = ratePer;

                for (DateTime idx = dfrom; idx <= dto; idx = idx.AddDays(1))
                {
                    //(3.Use general: Insert chi tiết --> Gần Finished)
                    depItem = SetItemSalDep(depItem, lstAttItemPro, idx);
                }
                lstItem.Add(depItem);

            }
            return lstItem;
        }
        #endregion

        #region XuChi (3.Use general: Insert chi tiết --> Gần Finished)...Lấy giờ công và giờ OT cho chi tiết từng người
        public Sal_SalaryDepartmentItem SetItemSalDep(Sal_SalaryDepartmentItem depItem, List<Att_AttendanceTableItem> lstAttItemPro, DateTime idx)
        {
            Att_AttendanceTableItem attItem = lstAttItemPro.Where(att => att.WorkDate == idx).FirstOrDefault();
            if (attItem == null)
            {
                return depItem;
            }

            //AppConfig
            var service = new Sys_AttOvertimePermitConfigServices();
            var value52 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE52);
            var value59 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE59);
            var value8 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE8); 
           // OverTimeStatus.E_APPROVED?

            //Lấy giờ công và giờ ca đêm
            if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
            {
                //Nếu có check cấu hình chung là lấy giờ công trả lương
                if (value59.ToLower() == true.ToString().ToLower())
                {
                    double workPaidHour = attItem.WorkPaidHours;
                    //Nếu có thai sản thì trừ đi 1h
                    if (attItem.IsHavingPregTreatment == true && workPaidHour > 0)
                        workPaidHour = workPaidHour - 1;
                    depItem.PaidWorkHours += workPaidHour;
                }
                else
                {
                    double workpaidhournonpreg = 0; // attItem.WorkPaidHourNonPreg != null ? attItem.WorkPaidHourNonPreg.Value : 0;
                    workpaidhournonpreg = Math.Round(workpaidhournonpreg, 2);
                    depItem.PaidWorkHours += workpaidhournonpreg;
                }

                //(bổ sung lấy giờ ca đêm để tính tiền phụ trội ca đêm)
                double nightshifthour = attItem.NightShiftHours != null ? attItem.NightShiftHours : 0;
                nightshifthour = Math.Round(nightshifthour, 2);
                double nightshifthourDept = depItem.NightShiftHours != null ? depItem.NightShiftHours.Value : 0;
                depItem.NightShiftHours = nightshifthourDept + nightshifthour;
            }
            else
            {
                //Nếu có check cấu hình chung là lấy giờ công trả lương
                if (value59.ToLower() == true.ToString().ToLower())
                {
                    double workPaidHour = attItem.WorkPaidHours;
                    //Nếu có thai sản thì trừ đi 1h
                    if (attItem.IsHavingPregTreatment == true && workPaidHour > 0)
                        workPaidHour = workPaidHour - 1;
                    depItem.PaidWorkHours += workPaidHour;
                }
                else
                {
                    double workpaidhournonpreg = 0; // attItem.WorkPaidHourNonPreg != null ? attItem.WorkPaidHourNonPreg.Value : 0;
                    workpaidhournonpreg = Math.Round(workpaidhournonpreg, 1);
                    depItem.PaidWorkHours += workpaidhournonpreg;
                }
            }

            #region Gan OT
            List<int> lstOT = new List<int>();
            Double rate = 0; //Hệ số tăng ca, kiểm tra nếu = 0 sẽ loại bỏ khỏi việc tính lương BP, vì đây là trường hợp đặc biệt (tăng ca biệt phái)-->theo phongphu.
            Double hourOT = 0;
            lstOT.Add(1);
            lstOT.Add(2);
            lstOT.Add(3);
            lstOT.Add(4);

            #region Tính OT khi được duyệt
            if (value8 == OverTimeStatus.E_APPROVED.ToString())
            {
                foreach (var item in lstOT)
                {
                    switch (item)
                    {
                        #region case 1
                        case 1:
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime1Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime1Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime2Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime2Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime3Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime3Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime4Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime4Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime5Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime5Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime6Hours += attItem.OvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime6Hours = attItem.OvertimeRegisterHours;
                                break;
                            }
                            break;
                        #endregion
                        #region case 2
                        case 2:
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime1Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime1Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime2Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime2Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime3Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime3Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime4Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime4Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime5Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime5Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime6Hours += attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime6Hours = attItem.ExtraOvertimeRegisterHours;
                                break;
                            }
                            break;
                        #endregion
                        #region case3
                        case 3:
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime1Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime1Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime2Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime2Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime3Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime3Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime4Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime4Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime5Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime5Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime6Hours += attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime6Hours = attItem.ExtraOvertimeRegisterHours2;
                                break;
                            }
                            break;
                        #endregion
                        #region case 4
                        case 4:
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime1Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime1Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime2Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime2Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime3Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime3Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime4Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime4Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime5Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime5Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime6Hours += attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime6Hours = attItem.ExtraOvertimeRegisterHours3;
                                break;
                            }
                            break;
                        #endregion
                    }
                }
            }
            #endregion
            #region Tính OT với các trạng thái khác
            else
            {
                foreach (var item in lstOT)
                {
                    switch (item)
                    {
                        #region case 1
                        case 1:
                            rate = attItem.Cat_OvertimeType != null ? attItem.Cat_OvertimeType.Rate : 0;
                            if (rate == 0)
                                break;
                            hourOT = Math.Round(attItem.OvertimeHours != null ? attItem.OvertimeHours : 0, 2);
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime1Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime1Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime2Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime2Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime3Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime3Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime4Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime4Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime5Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime5Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.OvertimeTypeID)
                            {
                                depItem.Overtime6Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.OvertimeTypeID;
                                depItem.Overtime6Hours = hourOT;
                                break;
                            }
                            break;
                        #endregion
                        #region case 2
                        case 2:
                            rate = attItem.Cat_OvertimeType1 != null ? attItem.Cat_OvertimeType1.Rate : 0;
                            if (rate == 0)
                                break;
                            hourOT = Math.Round(attItem.ExtraOvertimeHours != null ? attItem.ExtraOvertimeHours : 0, 2);
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime1Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime1Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime2Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime2Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime3Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime3Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime4Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime4Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime5Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime5Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeTypeID)
                            {
                                depItem.Overtime6Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeTypeID;
                                depItem.Overtime6Hours = hourOT;
                                break;
                            }
                            break;
                        #endregion
                        #region case3
                        case 3:
                            rate = attItem.Cat_OvertimeType2 != null ? attItem.Cat_OvertimeType2.Rate : 0;
                            if (rate == 0)
                                break;
                            hourOT = Math.Round(attItem.ExtraOvertimeHours2 != null ? attItem.ExtraOvertimeHours2 : 0, 2);
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime1Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime1Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime2Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime2Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime3Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime3Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime4Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime4Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime5Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime5Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeType2ID)
                            {
                                depItem.Overtime6Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeType2ID;
                                depItem.Overtime6Hours = hourOT;
                                break;
                            }
                            break;
                        #endregion
                        #region case 4
                        case 4:
                            rate = attItem.Cat_OvertimeType3 != null ? attItem.Cat_OvertimeType3.Rate : 0;
                            if (rate == 0)
                                break;
                            hourOT = Math.Round(attItem.ExtraOvertimeHours3 != null ? attItem.ExtraOvertimeHours3 : 0, 2);
                            if (depItem.Overtime1TypeID != null && depItem.Overtime1TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime1Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime1TypeID == null)
                            {
                                depItem.Overtime1TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime1Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID != null && depItem.Overtime2TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime2Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime2TypeID == null)
                            {
                                depItem.Overtime2TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime2Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID != null && depItem.Overtime3TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime3Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime3TypeID == null)
                            {
                                depItem.Overtime3TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime3Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID != null && depItem.Overtime4TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime4Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime4TypeID == null)
                            {
                                depItem.Overtime4TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime4Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID != null && depItem.Overtime5TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime5Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime5TypeID == null)
                            {
                                depItem.Overtime5TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime5Hours = hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID != null && depItem.Overtime6TypeID == attItem.ExtraOvertimeType3ID)
                            {
                                depItem.Overtime6Hours += hourOT;
                                break;
                            }
                            else if (depItem.Overtime6TypeID == null)
                            {
                                depItem.Overtime6TypeID = attItem.ExtraOvertimeType3ID;
                                depItem.Overtime6Hours = hourOT;
                                break;
                            }
                            break;
                        #endregion
                    }
                }
            }
            #endregion

            #endregion
            return depItem;
        }
        #endregion

        #region XuChi (4.Use general: Finished)...Xử lý tính tiền lương, tính tiền các loại OT
        public List<Sal_SalaryDepartmentItem> ComputeAmountPro(List<Sal_SalaryDepartmentItem> lstDepItem, Sal_SalaryDepartment salDep)
        {
            //Hệ số trung bình và tổng giờ công tất cả NV (Sum of Tổng công của từng NV * hệ số cá nhân )
            Double avgSalOrg = 0;
            Double totalHoursRateOrg = 0;

            //AppConfig
            var service = new Sys_AttOvertimePermitConfigServices();
            var value52 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE54);
            var value54 = service.GetConfigValue<string>(AppConfig.HRM_SAL_CONFIG_VALUE54);

            //Tính tổng công sản phẩm [Tổng Công Sản Phẩm = Công Đi làm + OT1 + OT2 + OT3 + OT4 + OT5 + OT6 (với: OT chưa quy đổi)]
            #region " Lấy tổng công"
            if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
            {
                for (int i = 0; i < lstDepItem.Count(); i++)
                {
                    Sal_SalaryDepartmentItem item = lstDepItem[i];
                    Double totalOT = 0;
                    if (item.Overtime1TypeID != null)
                    {
                        Double hours = item.Overtime1Hours != null ? item.Overtime1Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType != null ? item.Cat_OvertimeType.Rate : 0;
                        item.udOT1 = hours * rate; //Lấy OT1 quy đổi để sử dụng tính số tiền OT (OT1AmountSalary)
                    }
                    if (item.Overtime2TypeID != null)
                    {
                        Double hours = item.Overtime2Hours != null ? item.Overtime2Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType1 != null ? item.Cat_OvertimeType1.Rate : 0;
                        item.udOT2 = hours * rate; //Lấy OT2 quy đổi để sử dụng tính số tiền OT (OT2AmountSalary)
                    }
                    if (item.Overtime3TypeID != null)
                    {
                        Double hours = item.Overtime3Hours != null ? item.Overtime3Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType2 != null ? item.Cat_OvertimeType2.Rate : 0;
                        item.udOT3 = hours * rate; //Lấy OT3 quy đổi để sử dụng tính số tiền OT (OT3AmountSalary)
                    }
                    if (item.Overtime4TypeID != null)
                    {
                        Double hours = item.Overtime4Hours != null ? item.Overtime4Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType3 != null ? item.Cat_OvertimeType3.Rate : 0;
                        item.udOT4 = hours * rate; //Lấy OT4 quy đổi để sử dụng tính số tiền OT (OT4AmountSalary)
                    }
                    if (item.Overtime5TypeID != null)
                    {
                        Double hours = item.Overtime5Hours != null ? item.Overtime5Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType4 != null ? item.Cat_OvertimeType4.Rate : 0;
                        item.udOT5 = hours * rate; //Lấy OT5 quy đổi để sử dụng tính số tiền OT (OT5AmountSalary)
                    }
                    if (item.Overtime6TypeID != null)
                    {
                        Double hours = item.Overtime6Hours != null ? item.Overtime6Hours.Value : 0;
                        totalOT += hours;

                        Double rate = item.Cat_OvertimeType5 != null ? item.Cat_OvertimeType5.Rate : 0;
                        item.udOT6 = hours * rate; //Lấy OT6 quy đổi để sử dụng tính số tiền OT (OT6AmountSalary)
                    }
                    item.udTotalHoursProduct = (lstDepItem[i].PaidWorkHours + totalOT);
                }
            }
            #endregion

            //Các trường hợp tính OT
            if (value54 == AppConfig.E_OT_CHANGE_COMPUTE.ToString())
            {
                #region "OT quy đổi"
                for (int i = 0; i < lstDepItem.Count(); i++)
                {
                    Sal_SalaryDepartmentItem item = lstDepItem[i];
                    Double totalOT = 0;

                    if (item.Overtime1TypeID != null)
                    {
                        Double hours = item.Overtime1Hours != null ? item.Overtime1Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType != null ? item.Cat_OvertimeType.Rate : 0;
                        totalOT += hours * rate;
                    }
                    if (item.Overtime2TypeID != null)
                    {
                        Double hours = item.Overtime2Hours != null ? item.Overtime2Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType1 != null ? item.Cat_OvertimeType1.Rate : 0;
                        totalOT += hours * rate;
                    }
                    if (item.Overtime3TypeID != null)
                    {
                        Double hours = item.Overtime3Hours != null ? item.Overtime3Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType2 != null ? item.Cat_OvertimeType2.Rate : 0;
                        totalOT += hours * rate;
                    }
                    if (item.Overtime4TypeID != null)
                    {
                        Double hours = item.Overtime4Hours != null ? item.Overtime4Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType3 != null ? item.Cat_OvertimeType3.Rate : 0;
                        totalOT += hours * rate;
                    }
                    if (item.Overtime5TypeID != null)
                    {
                        Double hours = item.Overtime5Hours != null ? item.Overtime5Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType4 != null ? item.Cat_OvertimeType4.Rate : 0;
                        totalOT += hours * rate;
                    }
                    if (item.Overtime6TypeID != null)
                    {
                        Double hours = item.Overtime6Hours != null ? item.Overtime6Hours.Value : 0;
                        Double rate = item.Cat_OvertimeType5 != null ? item.Cat_OvertimeType5.Rate : 0;
                        totalOT += hours * rate;
                    }

                    //Tổng giờ công và Total OT của từng NV. 
                    item.udTotalHoursOTPaid = (lstDepItem[i].PaidWorkHours + totalOT);

                    //Tổng giờ công của tất cả NV trong ds tính = Sum(Tổng công của từng NV * hệ số cá nhân )
                    totalHoursRateOrg += lstDepItem[i].Rate * item.udTotalHoursOTPaid;
                    if (i == lstDepItem.Count() - 1 && totalHoursRateOrg != 0)
                    {
                        if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
                            avgSalOrg = salDep.AmountAfterAdjust.Value / totalHoursRateOrg;
                        else
                            avgSalOrg = salDep.Amount / totalHoursRateOrg;
                    }
                }
                #endregion
            }
            else if (value54 == AppConfig.E_NOT_OT_COMPUTE.ToString())
            {
                #region "Không tính OT"
                for (int i = 0; i < lstDepItem.Count(); i++)
                {
                    Sal_SalaryDepartmentItem item = lstDepItem[i];

                    item.udTotalHoursOTPaid = lstDepItem[i].PaidWorkHours;
                    totalHoursRateOrg += lstDepItem[i].Rate * item.udTotalHoursOTPaid;
                    if (i == lstDepItem.Count() - 1 && totalHoursRateOrg != 0)
                    {
                        if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
                            avgSalOrg = salDep.AmountAfterAdjust.Value / totalHoursRateOrg;
                        else
                            avgSalOrg = salDep.Amount / totalHoursRateOrg;
                    }
                }
                #endregion
            }
            else
            {
                #region "OT chưa quy đổi"
                for (int i = 0; i < lstDepItem.Count(); i++)
                {
                    Sal_SalaryDepartmentItem item = lstDepItem[i];
                    Double totalOT = 0;

                    if (item.Overtime1TypeID != null)
                    {
                        Double hours = item.Overtime1Hours != null ? item.Overtime1Hours.Value : 0;
                        totalOT += hours;
                    }
                    if (item.Overtime2TypeID != null)
                    {
                        Double hours = item.Overtime2Hours != null ? item.Overtime2Hours.Value : 0;
                        totalOT += hours;
                    }
                    if (item.Overtime3TypeID != null)
                    {
                        Double hours = item.Overtime3Hours != null ? item.Overtime3Hours.Value : 0;
                        totalOT += hours;
                    }
                    if (item.Overtime4TypeID != null)
                    {
                        Double hours = item.Overtime4Hours != null ? item.Overtime4Hours.Value : 0;
                        totalOT += hours;
                    }
                    if (item.Overtime5TypeID != null)
                    {
                        Double hours = item.Overtime5Hours != null ? item.Overtime5Hours.Value : 0;
                        totalOT += hours;
                    }
                    if (item.Overtime6TypeID != null)
                    {
                        Double hours = item.Overtime6Hours != null ? item.Overtime6Hours.Value : 0;
                        totalOT += hours;
                    }

                    item.udTotalHoursOTPaid = (lstDepItem[i].PaidWorkHours + totalOT);
                    totalHoursRateOrg += lstDepItem[i].Rate * item.udTotalHoursOTPaid;
                    if (i == lstDepItem.Count() - 1 && totalHoursRateOrg != 0)
                    {
                        if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
                            avgSalOrg = salDep.AmountAfterAdjust.Value / totalHoursRateOrg;
                        else
                            avgSalOrg = salDep.Amount / totalHoursRateOrg;
                    }
                }
                #endregion
            }

            foreach (Sal_SalaryDepartmentItem item in lstDepItem)
            {
                //Tổng tiền sản phẩm. [udTotalHoursOTPaid = ngày công + tổng OT theo 3 TH (quy đổi, ko qđ, ko OT)]
                item.AmoutSalary = item.Rate * item.udTotalHoursOTPaid * avgSalOrg;

                //Nếu cách tính lương là dùng hệ số tự nhập từ màn hình nhập hệ số (Theo logic PHONGPHU)
                if (value52 == AppConfig.E_RATE_BY_ATUALARISING.ToString())
                {
                    //Tính StdAmountSalary (lương sản phẩm theo công). [udTotalHoursProduct = ngày công + tổng OT theo TH ko qđ]
                    if (item.udTotalHoursProduct != 0 && item.PaidWorkHours != 0)
                        item.StdAmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.PaidWorkHours;
                    else
                        item.StdAmountSalary = 0;

                    //Tính các tổng tiền OT và tiền phụ trội ca đêm (bổ sung thêm cho Phongphu)
                    if (item.udTotalHoursProduct == 0)
                    {
                        item.OT1AmountSalary = 0;
                        item.OT2AmountSalary = 0;
                        item.OT3AmountSalary = 0;
                        item.OT4AmountSalary = 0;
                        item.OT5AmountSalary = 0;
                        item.OT6AmountSalary = 0;
                        item.NightAmountSalary = 0;
                    }
                    else
                    {
                        item.OT1AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT1;
                        item.OT2AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT2;
                        item.OT3AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT3;
                        item.OT4AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT4;
                        item.OT5AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT5;
                        item.OT6AmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.udOT6;
                        item.NightAmountSalary = item.AmoutSalary / item.udTotalHoursProduct * item.NightShiftHours * 0.3; //0.3 là hệ số phụ trội ca đêm
                    }

                    //Tiền lương ngày
                    item.DayAmountSalary = item.StdAmountSalary + item.NightAmountSalary +
                                          item.OT1AmountSalary + item.OT2AmountSalary + item.OT3AmountSalary + item.OT4AmountSalary + item.OT5AmountSalary + item.OT6AmountSalary;
                }
            }
            salDep.TotalGroupHours = totalHoursRateOrg;
            return lstDepItem;
        }
        #endregion

        //Đệ quy: get ds nhân viên theo phòng ban (kể cả pb con)
        public static List<Cat_OrgStructure> GetChilds(List<Cat_OrgStructure> lstOrgStructAll, Guid parentOrgId)
        {
            List<Cat_OrgStructure> res = new List<Cat_OrgStructure>();
            Cat_OrgStructure parentOrg = lstOrgStructAll.Where(org => org.ID == parentOrgId).FirstOrDefault();
            List<Cat_OrgStructure> lstChild = lstOrgStructAll.Where(prop => prop.ParentID == parentOrgId).ToList();

            if (parentOrg != null)
            {
                res.Add(parentOrg);
            }

            //khi phòng ban không còn child thì dừng
            if (lstChild.Count > 0)
            {
                foreach (Cat_OrgStructure orgStrucChild in lstChild)
                {
                    res.AddRange(GetChilds(lstOrgStructAll, orgStrucChild.ID));
                }
            }
            return res;
        }
        #endregion

    }
}
