using HRM.Business.Hr.Models;
using System.Linq;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using System;
using HRM.Data.Entity;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Domain
{
    public class Hre_HDTJobServices : BaseService
    {
        public string ActionApproved(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_HDTJobRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHDTJobs = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var HDTJob in lstHDTJobs)
                {
                    HDTJob.Status = HDTJobStatus.E_APPROVE.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string ActionCancel(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_HDTJobRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHDTJobs = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var HDTJob in lstHDTJobs)
                {
                    HDTJob.Status = HDTJobStatus.E_CANCEL.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string ActionApprovedForHDTJobWaiting(string selectedIds, string userLG)
        {
            string message = string.Empty;
            Guid userID = Guid.Empty;
            Guid.TryParse(userLG, out userID);
            if (userID == Guid.Empty)
            {
                message = "Bạn Không Có Quyền Duyệt HDTJob";
                return message;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_HDTJobRepository(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHDTJobs = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                int suc = 0;
                int nop = 0;
                var checkPermission = repoSys_UserApprove.FindBy(s => s.IsDelete == null
                                                    && s.Type == ApproveType.E_HDTJOB_PAST.ToString()
                                                    && s.UserApproveID == userID).ToList();
                foreach (var HDTJob in lstHDTJobs)
                {
                    if (DateTime.Now.Date <= HDTJob.DateFrom.Value.Date)
                    {
                        suc += 1;
                        HDTJob.Status = HDTJobStatus.E_APPROVE.ToString();
                        repo.Edit(HDTJob);
                    }
                    else
                    {
                        if (checkPermission.Count > 0)
                        {
                            suc += 1;
                            HDTJob.Status = HDTJobStatus.E_APPROVE.ToString();
                            repo.Edit(HDTJob);
                        }
                        else
                        {
                            nop += 1;
                        }
                    }
                }

                repo.SaveChanges();
                if (nop > 0 && suc > 0)
                {
                    message = "Duyệt Thành Công [ " + suc + " ] Dòng HDTJob Và Bạn Không Có Quyền Duyệt [ " + nop + " ] Dòng HDTJob Trong Quá Khứ";
                    return message;
                }
                else if (suc > 0)
                {
                    message = "Bạn Đã Duyệt Thành Công [ " + suc + " ] Dòng HDTJob";
                    return message;
                }
                else if (nop > 0)
                {
                    message = "Bạn Không Có Quyền Duyệt [ " + nop + " ] Dòng HDTJob Trong Quá Khứ";
                    return message;
                }
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string ActionApprovedHDTJobOut(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHre_HDTJobs = repoHre_HDTJob.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                foreach (var HDTJob in lstHre_HDTJobs)
                {
                    HDTJob.StatusOut = HDTJobStatus.E_APPROVE.ToString();
                }
                unitOfWork.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        } 
        public int CheckDataHDTJobIn(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                int? countNotValid = null;
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHre_HDTJobs = repoHre_HDTJob.FindBy(m => m.ID != null && lstIds.Contains(m.ID) && m.StatusData == null).OrderBy(s => s.ProfileID).ThenBy(m => m.DateFrom).ToList();
                Guid? profileID = null;
                bool isInvalid = true;
                DateTime? dateFromOld = null;
                DateTime? dateToOld = null;
                foreach (var HDTJob in lstHre_HDTJobs)
                {
                    int countdouble = 0;
                    if (HDTJob.ProfileID != profileID)
                    {
                        countdouble = 0;
                        dateFromOld = HDTJob.DateFrom;
                        dateToOld = HDTJob.DateTo;
                        profileID = HDTJob.ProfileID;
                        isInvalid = true;
                    }
                    // dữ liệu ko được giao nhau
                    if (dateToOld <= HDTJob.DateTo && dateToOld >= HDTJob.DateFrom && dateFromOld != HDTJob.DateFrom && dateToOld != HDTJob.DateTo)
                    {
                        isInvalid = false;
                    }
                    if (HDTJob.DateTo < HDTJob.DateFrom)
                    {
                        isInvalid = false;
                    }
                    if (HDTJob.DateFrom != null && HDTJob.DateFrom.Value.Date < DateTime.Now.Date)
                    {
                        isInvalid = false;
                    }
                    if (HDTJob.StatusOut == null || HDTJob.StatusOut == HDTJobStatus.E_WAITING.ToString())
                    {
                        countdouble++;
                    }

                    if (countdouble >= 2)
                    {
                        isInvalid = false;
                    }

                    if (isInvalid == false)
                    {
                        //xử lý cột trạng thái dữ liệu
                        HDTJob.StatusData = CheckHDTJobType.E_WRONG_DATA.ToString();
                        countNotValid++;
                    }
                    else
                    {
                        HDTJob.StatusData = CheckHDTJobType.E_SUCCESS_DATA.ToString();
                    }
                }
                unitOfWork.SaveChanges();
                message = NotificationType.Success.ToString();
                return countNotValid != null ? (int)countNotValid : 0;
            }
        }
        public string CheckDataHDTJobOut(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHre_HDTJobs = repoHre_HDTJob.FindBy(m => m.ID != null && lstIds.Contains(m.ID) && m.StatusDataOut == null).OrderBy(s => s.ProfileID).ThenBy(m => m.DateFrom).ToList();
                Guid? profileID = null;
                bool isInvalid = true;
                DateTime? dateFromOld = null;
                DateTime? dateToOld = null;
                foreach (var HDTJob in lstHre_HDTJobs)
                {
                    int countdouble = 0;
                    if (HDTJob.ProfileID != profileID)
                    {
                        dateFromOld = HDTJob.DateFrom;
                        dateToOld = HDTJob.DateTo;
                        profileID = HDTJob.ProfileID;
                        isInvalid = true;
                        countdouble = 0;
                    }

                    // dữ liệu ko được giao nhau
                    if (dateToOld <= HDTJob.DateTo && dateToOld >= HDTJob.DateFrom && dateFromOld != HDTJob.DateFrom && dateToOld != HDTJob.DateTo)
                    {
                        isInvalid = false;
                    }

                    if (HDTJob.DateTo < HDTJob.DateFrom)
                    {
                        isInvalid = false;
                    }

                    if (HDTJob.DateTo != null && HDTJob.DateTo < DateTime.Now)
                    {
                        isInvalid = false;
                    }

                    if (HDTJob.StatusOut == null || HDTJob.StatusOut == HDTJobStatus.E_WAITING.ToString())
                    {
                        countdouble++;
                    }

                    if (countdouble >= 2)
                    {
                        isInvalid = false;
                    }

                    if (isInvalid == false)
                    {
                        //xử lý cột trạng thái dữ liệu
                        HDTJob.StatusDataOut = CheckHDTJobType.E_WRONG_DATA.ToString();
                    }
                    else
                    {
                        HDTJob.StatusDataOut = CheckHDTJobType.E_SUCCESS_DATA.ToString();
                    }
                }
                unitOfWork.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
