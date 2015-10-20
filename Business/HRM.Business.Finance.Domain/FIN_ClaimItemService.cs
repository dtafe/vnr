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

namespace HRM.Business.Finance.Domain
{
    public class FIN_ClaimItemService : BaseService
    {

        public void ActionApproved(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            string UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();

                if(status == "E_APPROVED")
                {
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType1 = FinApproverType.FIN_Claim_Profile.ToString();
                    var approvedType2 = FinApproverType.FIN_Claim_Manufacture.ToString();

                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();
                    if (lstId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Description2 = status;
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                        
                    }
                }

                if(status == "E_REJECTED")
                {
                    if (lstId.Count > 0)
                    {
                        lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                        foreach (var item in lstTravelRequestItem)
                        {
                            item.Description2 = status;
                        }
                        repoTravelRequestItem.SaveChanges();
                    }
                
                }
               

            }
        }

        public void ActionApprovedAll(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            string UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoClaimItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var repoClaim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var lstClaimId = repoClaim.GetAll().Where(s => s.IsDelete == null && lstId.Contains(s.ID)).Select(s => s.ID).ToList();

                var lstClaimItem = repoClaimItem.GetAll().Where(s => s.IsDelete == null).ToList();

          
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType1 = FinApproverType.FIN_Claim_Profile.ToString();
                    var approvedType2 = FinApproverType.FIN_Claim_Manufacture.ToString();
                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();

                if(status == "E_REQUEST")
                {
                    if (lstClaimId.Count > 0)
                    {
                        
                            lstClaimItem = lstClaimItem.Where(s => s.ClaimID != null && lstClaimId.Contains(s.ClaimID.Value)).ToList();
                            foreach (var item in lstClaimItem)
                            {
                                item.Description2 = status ;
                            }
                            repoClaimItem.SaveChanges();
                        

                    }
                }

                if(status == "E_APPROVED")
                {
                    if (lstClaimId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstClaimItem = lstClaimItem.Where(s => s.ClaimID != null && lstClaimId.Contains(s.ClaimID.Value)).ToList();
                            foreach (var item in lstClaimItem)
                            {
                                item.Description2 = status + "_" + approvedEntity.OrderNo.ToString();
                            }
                            repoClaimItem.SaveChanges();
                        }

                    }
                }

                    
                
            }
        }


        public void ActionSubmit(List<Guid> lstId, string status,Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            string UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();

                var claimID = lstTravelRequestItem.Where(s => s.ClaimID != null && lstId.Contains(s.ID)).Select(s => s.ClaimID).Distinct().FirstOrDefault();
                var userEntity = GetData<Sys_UserInfoEntity>(userId, ConstantSql.hrm_sys_sp_get_UserbyId, UserLogin, ref statusMes).FirstOrDefault();

                var claimEntity = new FIN_ClaimEntity();
                if (claimID != null)
                {
                    claimEntity = GetData<FIN_ClaimEntity>(claimID.Value, ConstantSql.hrm_hr_sp_get_ClaimById, UserLogin, ref statusMes).FirstOrDefault();
                }

                if (lstId.Count > 0)
                {
                    if (userEntity != null && claimEntity != null)
                    {
                        if (userEntity.ProfileID.Value == claimEntity.ProfileID.Value)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Description2 = status;
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                    }

                    
                }

            }
        }


    }
}
