using System.Data.SqlClient;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Linq;
using VnResource.Helper.Data;
using VnResource.Helper.Utility;
using System.Reflection;
using System.Collections;
using HRM.Data.Entity;
using System.Data.Entity.Validation;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Hr.Models;
using System.Net.Mail;
using System.Net;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Finance.Models;
using HRM.Data.Entity.Repositories;

namespace HRM.Business.Finance.Domain
{
    /// <summary>Người Duyệt</summary>
    public class FIN_ApproverECLAIMService : BaseService
    {
        public FIN_ApproverECLAIMEntity AddApprovers(FIN_ApproverECLAIMEntity model)
        {
            var result = new FIN_ApproverECLAIMEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                //lay ds Approved theo profileId
                var profileId = Common.DotNetToOracle(string.Empty.ToString());
                if (model != null && model.ProfileID != null)
                {
                    profileId = Common.DotNetToOracle(model.ProfileID.ToString());
                }

                if (model != null)
                {
                    model.ApprovedIDs = model.ApprovedIDs.Where(m => m != Guid.Empty).ToList();
                    var lstEvaluators = model.ApprovedIDs == null ? new List<Guid>() : model.ApprovedIDs;
                    foreach (var evaluator in lstEvaluators)
                    {
                        result = model.CopyData<FIN_ApproverECLAIMEntity>();
                        result.ApprovedID = evaluator;
                        Add(result);
                    }
                    if (!lstEvaluators.Any())
                    {
                        //khi khong chon nguoi danh gia
                        result = model.CopyData<FIN_ApproverECLAIMEntity>();
                        result.ApprovedID = null;
                        Add(result);
                    }
                }
            }//end context
            return result;
        }

        public FIN_ApproverECLAIMEntity DeleteListApprover(string ID)
        {
            var result = new FIN_ApproverECLAIMEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new FIN_ApproverECLAIMRepository(unitOfWork);
                List<Guid> listID = new List<Guid>();
                var list = ID.Split(',');
                foreach (var x in list)
                {
                    try
                    {
                        var a = Guid.Parse(x).TryGetValue<Guid>();
                        if (a != Guid.Empty)
                            listID.Add(a);
                    }
                    catch { }
                }
                foreach (var mainID in listID)
                {
                    var Fin_ApproverClaim = repo.GetById(mainID);
                    var ilFin_Approver = repo.FindBy(x => x.ProfileID == Fin_ApproverClaim.ProfileID && x.ApprovedType == Fin_ApproverClaim.ApprovedType && x.IsDelete == null).ToList();
                    if (ilFin_Approver == null || ilFin_Approver.Count == 0)
                    {
                        continue;

                    }
                    if (CheckFinApproverHasUse(ilFin_Approver.Select(x => x.ProfileID ?? Guid.Empty).ToList()) == true)
                    {
                        continue;
                    }

                    else
                    {
                        foreach (var item in ilFin_Approver)
                        {
                            item.IsDelete = true;
                        }
                        repo.SaveChanges();
                        result = ilFin_Approver.FirstOrDefault().CopyData<FIN_ApproverECLAIMEntity>();
                    }
                }
            }
            return result;

        }


        // kiểm tra người đánh giá đã đánh giá 
        public bool CheckFinApproverHasUse(List<Guid> list)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var userApproveAmount = unitOfWork.CreateQueryable<FIN_HistoryApproveECLAIM>(Guid.Empty, m => m.UserApproveClaimID.HasValue && list.Contains(m.UserApproveClaimID.Value)).Count();
                if (userApproveAmount != 0)
                    return true;
            }
            return false;
        }


        #region Lay ds chưa duyệt (Chỉ được xoa những dữ liệu chưa duyệt)

        /// <summary>Kiểm tra TravelRequest được phép xoá không ?</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAllowRemoveFinTravelRequest(string id)
        {
            //Kiểm tra TravelRequest được phép xoá không ?
            //if Tất cả các dòng chưa duyệt => cho phép xoá
            //else => không cho phép xoá

            using (var context = new VnrHrmDataContext())
            {                
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));     
                var listID = Common.StringToList(id);
                //lay ds travelRequestID ở ds historyApproveEclaim
                var lstTravelRequestID = unitOfWork.CreateQueryable<FIN_HistoryApproveECLAIM>(Guid.Empty, m => m.TravelRequestID.HasValue && listID.Contains(m.TravelRequestID.Value))
                    .Select(m => m.TravelRequestID.Value).ToList();
                var listIDResult = listID.Except(lstTravelRequestID).ToList();
                if (listID.Count != listIDResult.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>Kiểm tra CashAdvance được phép xoá không ?</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAllowRemoveCashAdvance(string id)
        {
            //Kiểm tra CashAdvance được phép xoá không ?
            //if Tất cả các dòng chưa duyệt => cho phép xoá
            //else => không cho phép xoá

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var listID = Common.StringToList(id);
                //lay ds CashAdvanceID ở ds historyApproveEclaim
                var lstCashAdvanceID = unitOfWork.CreateQueryable<FIN_HistoryApproveECLAIM>(Guid.Empty, m => m.CashAdvanceID.HasValue && listID.Contains(m.CashAdvanceID.Value))
                    .Select(m => m.CashAdvanceID.Value).ToList();
                var listIDResult = listID.Except(lstCashAdvanceID).ToList();
                if (listID.Count != listIDResult.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>Kiểm tra Claim được phép xoá không ?</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAllowRemoveClaim(string id)
        {
            //Kiểm tra Claim được phép xoá không ?
            //if Tất cả các dòng chưa duyệt => cho phép xoá
            //else => không cho phép xoá

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var listID = Common.StringToList(id);
                //lay ds ClaimID ở ds historyApproveEclaim
                var lstClaimID = unitOfWork.CreateQueryable<FIN_HistoryApproveECLAIM>(Guid.Empty, m => m.ClaimID.HasValue && listID.Contains(m.ClaimID.Value))
                    .Select(m => m.ClaimID.Value).ToList();
                var listIDResult = listID.Except(lstClaimID).ToList();
                if (listID.Count != listIDResult.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion



    }
}
