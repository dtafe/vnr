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

namespace HRM.Business.Hr.Domain
{
    public class Att_AnnualLeaveServices : BaseService
    {
        #region MyRegion
        ///// <summary>
        ///// Lấy toàn bộ data
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AnnualLeave> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        return repo.Get().Where(i => i.IsDelete == null);
        //    }
        //}

        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Att_AnnualLeaveEntity Get(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        var data = repo.GetAnnualLeaveById(id);
        //        if (data.IsDelete == true) data = null;
        //        return data;
        //    }
        //}

        //public List<Att_AnnualLeaveEntity> GetAnnualLeaveByIds(string selectedIds)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        return repo.GetAnnualLeaveByIds(selectedIds);
        //    }
        //}

        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Add(Att_AnnualLeave model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(model);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Edit một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Edit(Att_AnnualLeave model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(model);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Remove 1 record là chuyển trạng thái IsDelete=true
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Remove(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(data);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Delete 1 record là xóa luôn record khỏi database
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Delete(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Delete(data);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        //#region Store Procedure

        //#region Danh sách

        ///// <summary>
        ///// Lấy danh sách dữ liệu AnnualLeaves [Tung.Ly - 2014/05/09]
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AnnualLeaveEntity> GetAnnualLeaves(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AnnualLeaveRepository repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        var rs = repo.GetAnnualLeaves(model);
        //        return rs;
        //    }
        //}

        //#endregion

        //#region Chi tiet

        ///// <summary>
        ///// Lấy dữ liệu AnnualLeave theo Id [Tung.Ly - 2014/05/09]
        ///// </summary>
        ///// <param name="annualLeaveId"></param>
        ///// <returns></returns>
        //public Att_AnnualLeaveEntity GetAnnualLeaveById(int annualLeaveId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AnnualLeaveRepository repo = new Att_AnnualLeaveRepository(unitOfWork);
        //        var data = repo.GetAnnualLeaveById(annualLeaveId);
        //        return data;
        //    }
        //}

        //#endregion

        //#endregion
        #endregion

        #region thanh toán phép năm đầu kỳ

        public void SetAnnualLeaveBeginYear(List<Guid> lstProfileIDs, double? limitMaximum, int Year, int? MonthResetANL, Guid? AllowanceTypeID, Guid? CuttOffDurationID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_AnnualLeave = new CustomBaseRepository<Att_AnnualLeave>(unitOfWork);
                var repoAtt_AnnualDetail = new CustomBaseRepository<Att_AnnualDetail>(unitOfWork);
                var repoSal_UnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
                //nếu có rồi thì chỉ cần Update
                //Nếu chưa có thì tạo mới
                Att_CutOffDuration CutOffDuration = new Att_CutOffDuration();
                if (CuttOffDurationID != null)
                {
                    CutOffDuration = repoAtt_CutOffDuration.FindBy(m => m.ID == CuttOffDurationID).FirstOrDefault();
                }


                int LastYear = Year - 1;
                DateTime? MaxTime = repoAtt_AnnualDetail.FindBy(m => m.IsDelete != null && m.Year == LastYear).OrderByDescending(m => m.MonthYear).Select(m => m.MonthYear).FirstOrDefault();
                List<Att_AnnualDetail> lstAnnualDetail = new List<Att_AnnualDetail>();
                if (MaxTime != null)
                {
                    lstAnnualDetail = repoAtt_AnnualDetail.FindBy(m => m.MonthYear == MaxTime
                        && m.ProfileID != null
                        && lstProfileIDs.Contains(m.ProfileID.Value)).ToList();
                }
                var lstAnnualLeaveInDB = repoAtt_AnnualLeave.FindBy(m => m.IsDelete == null && m.Year == Year && lstProfileIDs.Contains(m.ProfileID)).ToList();


                foreach (var ProfileID in lstProfileIDs)
                {
                    Att_AnnualLeave AnnualLeaveByProfile = lstAnnualLeaveInDB.Where(m => m.ProfileID == ProfileID).FirstOrDefault();
                    bool IsNew = false;
                    if (AnnualLeaveByProfile == null)
                    {
                        IsNew = true;
                        AnnualLeaveByProfile = new Att_AnnualLeave();
                        AnnualLeaveByProfile.ID = Guid.NewGuid();
                        AnnualLeaveByProfile.ProfileID = ProfileID;
                        AnnualLeaveByProfile.Year = Year;
                        AnnualLeaveByProfile.MonthStart = 1;

                    }
                    AnnualLeaveByProfile.MonthResetAnlOfBeforeYear = MonthResetANL;
                    double RemainLastYear = lstAnnualDetail.Where(m => m.ProfileID == ProfileID).Select(m => m.Remain ?? 0).FirstOrDefault();
                    if (limitMaximum != null)
                    {
                        AnnualLeaveByProfile.InitAnlValue = RemainLastYear > limitMaximum.Value ? limitMaximum.Value : RemainLastYear;
                        AnnualLeaveByProfile.SurplusAnllastYear = RemainLastYear > limitMaximum ? RemainLastYear - limitMaximum : 0;
                        if (CutOffDuration.ID != null && CutOffDuration.ID != Guid.Empty && AllowanceTypeID != null)
                        {
                            Sal_UnusualAllowance UnusualAllowance = new Sal_UnusualAllowance();
                            UnusualAllowance.ProfileID = ProfileID;
                            UnusualAllowance.MonthStart = CutOffDuration.DateStart;
                            UnusualAllowance.MonthEnd = CutOffDuration.DateEnd;
                            UnusualAllowance.UnusualEDTypeID = AllowanceTypeID;
                            UnusualAllowance.Amount = AnnualLeaveByProfile.SurplusAnllastYear;
                            repoSal_UnusualAllowance.Add(UnusualAllowance);
                        }
                    }
                    else
                    {
                        AnnualLeaveByProfile.InitAnlValue = RemainLastYear;
                        AnnualLeaveByProfile.SurplusAnllastYear = 0;
                    }
                    //Sửa lại theo phần tử mới mà Sơn build
                    if (IsNew)
                    {
                        repoAtt_AnnualLeave.Add(AnnualLeaveByProfile);
                    }
                    else
                    {
                        repoAtt_AnnualLeave.Edit(AnnualLeaveByProfile);
                    }
                }
                repoAtt_AnnualLeave.SaveChanges();
                repoSal_UnusualAllowance.SaveChanges();
            }
        }

        public void SetSickLeaveBeginYear(List<Guid> lstProfileIDs, double? limitMaximum, int Year)
        {
            //Đối với phép ốm thì tạm thời tính từ đầu năm đến cuối năm
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_AnnualLeave = new CustomBaseRepository<Att_AnnualLeave>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                int LastYear = Year - 1;
                DateTime beginLastYear = new DateTime(LastYear, 1, 1);
                DateTime endLastYear = beginLastYear.AddYears(1).AddMinutes(-1);
                List<Guid> SickLeaveType = repoCat_LeaveDayType.FindBy(m => m.IsDelete == null && m.Code == "SICK").Select(m => m.ID).ToList();
                string Approve = LeaveDayStatus.E_APPROVED.ToString();
                var lstLeaveDay = repoAtt_LeaveDay.FindBy(m =>
                    m.IsDelete == null
                    && m.Status == Approve
                    && m.DateStart < endLastYear
                    && m.DateEnd >= beginLastYear
                    && SickLeaveType.Contains(m.LeaveDayTypeID)
                    && lstProfileIDs.Contains(m.ProfileID)).Select(m => new { m.ProfileID, m.TotalDuration }).ToList();

                List<Att_AnnualLeave> lstAnnualLeaveInDB = repoAtt_AnnualLeave.FindBy(m => m.IsDelete == null && (m.Year == Year || m.Year == LastYear) && lstProfileIDs.Contains(m.ProfileID)).ToList();

                foreach (var ProfileID in lstProfileIDs)
                {
                    Att_AnnualLeave AnnualLeaveByProfile = lstAnnualLeaveInDB.Where(m => m.ProfileID == ProfileID && m.Year == Year).FirstOrDefault();
                    Att_AnnualLeave AnnualLeaveByProfile_Last = lstAnnualLeaveInDB.Where(m => m.ProfileID == ProfileID && m.Year == LastYear).FirstOrDefault();
                    bool IsNew = false;
                    if (AnnualLeaveByProfile == null)
                    {
                        IsNew = true;
                        AnnualLeaveByProfile = new Att_AnnualLeave();
                        AnnualLeaveByProfile.ID = Guid.NewGuid();
                        AnnualLeaveByProfile.ProfileID = ProfileID;
                        AnnualLeaveByProfile.Year = Year;
                        AnnualLeaveByProfile.MonthStart = 1;
                    }

                    double InitLastYear = 0;
                    if (AnnualLeaveByProfile_Last != null)
                        InitLastYear = AnnualLeaveByProfile_Last.InitSickValue;
                    double remainLastYear = InitLastYear - lstLeaveDay.Where(m => m.ProfileID == ProfileID).Sum(m => m.TotalDuration ?? 0);
                    if (limitMaximum != null)
                    {
                        AnnualLeaveByProfile.InitSickValue = remainLastYear > limitMaximum.Value ? limitMaximum.Value : remainLastYear;
                        AnnualLeaveByProfile.SurplusSicklastYear = remainLastYear > limitMaximum ? remainLastYear - limitMaximum : 0;
                    }
                    else
                    {
                        AnnualLeaveByProfile.InitSickValue = remainLastYear;
                        AnnualLeaveByProfile.SurplusSicklastYear = 0;
                    }
                    if (IsNew)
                    {
                        repoAtt_AnnualLeave.Add(AnnualLeaveByProfile);
                    }
                    else
                    {
                        repoAtt_AnnualLeave.Edit(AnnualLeaveByProfile);
                    }
                }
                repoAtt_AnnualLeave.SaveChanges();
            }
        }

        #endregion
    }
}
