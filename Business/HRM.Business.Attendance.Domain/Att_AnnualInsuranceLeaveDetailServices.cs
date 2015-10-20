using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Hr.Models;
using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Attendance.Domain;
using HRM.Business.HrmSystem.Models;

namespace HRM.Business.Hr.Domain
{
    public class Att_AnnualInsuranceLeaveDetailServices : BaseService
    {
        /// <summary>
        /// Button Tìm Kiếm Trong Phân Tích BHXH
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public List<Att_AnnualLeaveDetailEntity> SearchAnnualInsuranceLeaveDetail(int Year, string orgStructure, string LstProfileStatus,string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileQuery = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                string E_INSURANCE_LEAVE = AnnualLeaveDetailType.E_INSURANCE_LEAVE.ToString();

                List<Guid> lstProfileID = lstProfileQuery.Select(m => m.ID).ToList<Guid>();
                List<Att_AnnualLeaveDetailEntity> lstAnnDetail = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_INSURANCE_LEAVE && m.Year == Year
                    && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value)).ToList().Translate<Att_AnnualLeaveDetailEntity>();
                return lstAnnDetail;
            }
        }

        public bool HasConfigAnnualLeaveAtTotalConfig()
        {
            var isConfig = false;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_AppConfig = new CustomBaseRepository<Sys_AppConfig>(unitOfWork);

                string E_STANDARD_WORKDAY = AppConfig.E_STANDARD_WORKDAY.ToString();
                var appconfig = repoSys_AppConfig.FindBy(p => p.Info == E_STANDARD_WORKDAY).FirstOrDefault();
                if (appconfig == null || string.IsNullOrEmpty(appconfig.Value78) ||
                    string.IsNullOrEmpty(appconfig.Value79))
                {
                    //xuat thong bao : Vui lòng cấu hình tham số và công thức tính phép năm ở "Cấu hình chung" (PleaseConfigAnnualLeaveAtTotalConfig)
                    return false;
                }
            }
            return isConfig;
        }

        public bool ComputeInsuranceLeaveDetail(int Year, string orgStructure, string LstProfileStatus,string UserLogin)
        {
            /*
           * -get leaveDetail : type , year
           *   - get List Profile :
           *      + Theo phong ban
           *      + Theo trạng thai StatusEmpleaveDetail
           * 
           */

            var result = false;
        string status = string.Empty;


        using (var context = new VnrHrmDataContext())
        {
            var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
            var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
            var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
            var repoSys_AppConfig = new CustomBaseRepository<Sys_AppConfig>(unitOfWork);
            var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
            var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
            var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
            var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                
            #region get Data
            string HRM_ATT_ANNUALINSURANCELEAVE_ = AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_.ToString();
            List<object> lstO = new List<object>();
            lstO.Add(HRM_ATT_ANNUALINSURANCELEAVE_);
            lstO.Add(null);
            lstO.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);

            var formular1 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
            var formular2 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();

            if (config == null || string.IsNullOrEmpty(formular1.Value1) || string.IsNullOrEmpty(formular2.Value1))
            {
                //Common.MessageBoxs(Messages.Msg, Messages.PleaseConfigAnnualLeaveAtTotalConfig, MessageBox.Icon.WARNING, string.Empty);
                return result;
            }


            List<object> lstPro = new List<object>();
            lstPro.Add(orgStructure);
            lstPro.Add(null);
            lstPro.Add(null);
            var lstProfileQuery = GetData<Hre_ProfileEntity>(lstPro, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

            string formularConfig = formular1.Value1;
            string formularCompute = formular2.Value1;
            ParamGetConfigANL configAnl = new ParamGetConfigANL();
            (new Att_AttendanceServices()).GetConfigANL(formularConfig, out configAnl);

            int MonthBegin = 1;
            List<string> lstCodeLeaveTypeNonAnl = new List<string>();
            if (configAnl != null && configAnl.monthBeginYear != null)
            {
                MonthBegin = configAnl.monthBeginYear;
            }
            if (configAnl != null && configAnl.lstCodeLeaveNonANL != null)
            {
                lstCodeLeaveTypeNonAnl = configAnl.lstCodeLeaveNonANL;
            }
            DateTime BeginYear = new DateTime(Year, MonthBegin, 1);
            DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);

            string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
            List<DateTime> lstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff > BeginYear && m.DateOff <= EndYear)
                    .Select(m => m.DateOff).Distinct().ToList<DateTime>();
            List<Hre_ProfileEntity> lstprofile = lstProfileQuery
                .Where(m => m.DateQuit == null || (m.DateQuit != null && m.DateQuit > BeginYear)).ToList();
            List<Guid> lstProfileID = lstprofile.Select(m => m.ID).ToList();
            List<Guid> lstLeavedayTypeNonAnl = repoCat_LeaveDayType
                    .FindBy(m => m.IsDelete == null && lstCodeLeaveTypeNonAnl.Contains(m.Code)).Select(m => m.ID).ToList<Guid>();
            List<Att_LeaveDay> lstleavedayNonANl = repoAtt_LeaveDay
                    .FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && lstLeavedayTypeNonAnl.Contains(m.LeaveDayTypeID) && lstProfileID.Contains(m.ProfileID)).ToList<Att_LeaveDay>();
            List<Att_LeaveDay> lstleavedayNonANlInYear = lstleavedayNonANl.Where(m => m.DateStart <= EndYear && m.DateEnd >= BeginYear).ToList<Att_LeaveDay>();
            List<Hre_HDTJob> lstHDTJob = repoHre_HDTJob
                    .FindBy(m => m.IsDelete == null && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value)).ToList<Hre_HDTJob>();
            List<Cat_JobTitle> lstJobtitle = repoCat_JobTitle.FindBy(s => s.IsDelete == null).ToList();
            string E_INSURANCE_LEAVE = AnnualLeaveDetailType.E_INSURANCE_LEAVE.ToString();
            #endregion

            List<Att_AnnualLeaveDetail> lstInsDetail = new List<Att_AnnualLeaveDetail>();
            foreach (var item in lstprofile)
            {
                List<Att_LeaveDay> lstLeavedayNonAnlByProfile = lstleavedayNonANl.Where(m => m.ProfileID == item.ID).ToList();
                List<Att_LeaveDay> lstLeavedayNonAnlByProfileInYear = lstleavedayNonANlInYear.Where(m => m.ProfileID == item.ID).ToList();
                List<Hre_HDTJob> lstHDTJobByProfile = lstHDTJob.Where(m => m.ProfileID == item.ID).ToList();
                Att_AnnualLeaveDetail annualProfile = new Att_AnnualLeaveDetail();
                annualProfile.ID = Guid.NewGuid();
                annualProfile.ProfileID = item.ID;
                annualProfile.MonthStart = BeginYear;
                annualProfile.MonthEnd = EndYear;
                annualProfile.Year = Year;
                annualProfile.Type = E_INSURANCE_LEAVE;
                for (DateTime Month = BeginYear; Month < EndYear; Month = Month.AddMonths(1))
                {
                    double AvailabelInYear = (new Att_AttendanceServices()).GetAnnualLeaveAvailableAllYear( Year, Month.Month, item.DateHire,
                        item.DateEndProbation, item.DateQuit, formularConfig,
                        formularCompute, lstLeavedayNonAnlByProfileInYear, lstJobtitle, lstDayOff, lstHDTJobByProfile, lstLeavedayNonAnlByProfile);
                    if (Month.Month == 1)
                        annualProfile.Month1 = AvailabelInYear;
                    if (Month.Month == 2)
                        annualProfile.Month2 = AvailabelInYear;
                    if (Month.Month == 3)
                        annualProfile.Month3 = AvailabelInYear;
                    if (Month.Month == 4)
                        annualProfile.Month4 = AvailabelInYear;
                    if (Month.Month == 5)
                        annualProfile.Month5 = AvailabelInYear;
                    if (Month.Month == 6)
                        annualProfile.Month6 = AvailabelInYear;
                    if (Month.Month == 7)
                        annualProfile.Month7 = AvailabelInYear;
                    if (Month.Month == 8)
                        annualProfile.Month8 = AvailabelInYear;
                    if (Month.Month == 9)
                        annualProfile.Month9 = AvailabelInYear;
                    if (Month.Month == 10)
                        annualProfile.Month10 = AvailabelInYear;
                    if (Month.Month == 11)
                        annualProfile.Month11 = AvailabelInYear;
                    if (Month.Month == 12)
                        annualProfile.Month12 = AvailabelInYear;

                }
                lstInsDetail.Add(annualProfile);

            }

            DataErrorCode DataErr = DataErrorCode.Unknown;

            if (lstInsDetail.Count > 0)
            {
                #region lấy dữ liệu dưới DB xóa đi
                List<Guid> lstProfileID_InDB = lstInsDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).ToList();


                List<Att_AnnualLeaveDetail> lstAnnualLeaveDetail_InDB = repoAtt_AnnualLeaveDetail
                        .FindBy(m => m.IsDelete == null && m.Type == E_INSURANCE_LEAVE && m.Year == Year && m.ProfileID != null 
                            && lstProfileID_InDB.Contains(m.ProfileID.Value)).ToList();
                foreach (var item in lstAnnualLeaveDetail_InDB)
                {
                    item.IsDelete = true;
                }
                #endregion

                repoAtt_AnnualLeaveDetail.Add(lstInsDetail);
                try
                {
                    repoAtt_AnnualLeaveDetail.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                //EntityService.AddEntity<Att_AnnualLeaveDetail>(GuidContext, lstInsDetail.ToArray());
                //DataErr = EntityService.SubmitChanges(GuidContext, LoginUserID);

                //if (DataErr == DataErrorCode.Success)
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.SaveSuccess, MessageBox.Icon.INFO, string.Empty);
                //}
                //else
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.SaveUnSuccess, MessageBox.Icon.INFO, string.Empty);
                //}
            }
            //else
            //{
            //    Common.MessageBoxs(Messages.Msg, Messages.NoDataToCompute, MessageBox.Icon.WARNING, string.Empty);
            //}
            result = true;
        }
        return result;    
        }


    }

    public class HDTJobTypeRange
    {
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
