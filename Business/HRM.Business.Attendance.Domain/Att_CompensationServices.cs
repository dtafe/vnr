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
    public class Att_CompensationServices : BaseService
    {


        #region thanh toán phép Bù đầu kỳ

        public void SetCompensationLeaveBeginYear(List<Guid> lstProfileIDs, double? limitMaximum, int Year, int? MonthResetANL, Guid? AllowanceTypeID, Guid? CuttOffDurationID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_CompensationConfig = new CustomBaseRepository<Att_CompensationConfig>(unitOfWork);
                DateTime EndLastYear = new DateTime(Year - 1, 12, 1);

                List<Att_CompensationDetail> lstCompensationDetail = unitOfWork.CreateQueryable<Att_CompensationDetail>(m => m.MonthYear == EndLastYear
                                                                        && m.ProfileID != null
                                                                        && lstProfileIDs.Contains(m.ProfileID.Value)).ToList();
                var lstCompensationLeaveInDB = unitOfWork.CreateQueryable<Att_CompensationConfig>(m => m.Year == Year && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID.Value)).ToList();

                List<Att_CompensationConfig> lstNewData = new List<Att_CompensationConfig>();
                List<Att_CompensationConfig> lstEditData = new List<Att_CompensationConfig>();
                foreach (var ProfileID in lstProfileIDs)
                {
                    Att_CompensationConfig CompensationLeaveConfigByProfile = lstCompensationLeaveInDB.Where(m => m.ProfileID == ProfileID).FirstOrDefault();
                    bool IsNew = false;
                    if (CompensationLeaveConfigByProfile == null)
                    {
                        IsNew = true;
                        CompensationLeaveConfigByProfile = new Att_CompensationConfig();
                        CompensationLeaveConfigByProfile.ID = Guid.NewGuid();
                        CompensationLeaveConfigByProfile.ProfileID = ProfileID;
                        CompensationLeaveConfigByProfile.Year = Year;
                    }
                    CompensationLeaveConfigByProfile.MonthResetInitAvailable = MonthResetANL;
                    double RemainLastYear = lstCompensationDetail.Where(m => m.ProfileID == ProfileID).Select(m => m.Remain ?? 0).FirstOrDefault();
                    if (limitMaximum != null)
                    {
                        CompensationLeaveConfigByProfile.InitAvailable = RemainLastYear > limitMaximum.Value ? limitMaximum.Value : RemainLastYear;
                    }
                    else
                    {
                        CompensationLeaveConfigByProfile.InitAvailable = RemainLastYear;
                    }
                    if (IsNew)
                    {
                        lstNewData.Add(CompensationLeaveConfigByProfile);
                    }
                    else
                    {
                        lstEditData.Add(CompensationLeaveConfigByProfile);
                    }
                    //Sửa lại theo phần tử mới mà Sơn build
                }
                repoAtt_CompensationConfig.Add(lstNewData);
                repoAtt_CompensationConfig.Edit(lstEditData);
                unitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}
