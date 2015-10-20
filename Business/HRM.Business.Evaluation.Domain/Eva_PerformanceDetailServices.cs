using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Business.Evaluation.Domain
{
    public class Eva_PerformanceDetailServices : BaseService
    {
        public bool AddPerformanceDetail(string kpiIDs, Guid performanceTemplateID)
        {
            var isSuccess = false;
            string status = string.Empty;
            if (kpiIDs != null &&  kpiIDs.Any() && performanceTemplateID != Guid.Empty)
            {
                //chức năng thêm và tiêu chí đánh giá
                var listKPI = kpiIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                using (var context = new VnrHrmDataContext())
                {
                      var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    if (listKPI.Any())
	                {
		                var performanceDetails = GetAllUseEntity<Eva_PerformanceDetail>(ref status).Where(p=>p.IsDelete == null && p.PerformanceTemplateID == performanceTemplateID);
                        #region Lay so thu tu lon nhat
                        var maxOrderNumberFollowTemplateId = performanceDetails.Max(m => m.OrderNumber);
                        if (maxOrderNumberFollowTemplateId == null)
                        {
                            maxOrderNumberFollowTemplateId = 0;
                        } 
                        #endregion
                        //lay danh sach rate cua KPI
                        var kpiRates = unitOfWork.CreateQueryable<Eva_KPI>(Guid.Empty, m => listKPI.Contains(m.ID)).Select(m => new { m.ID, m.Rate }).ToList();

                        foreach (var kpiID in listKPI)
                        {
                            if (!performanceDetails.Any(p => p.KPIID == kpiID))
                            {
                                //tao moi
                                var kpiRate = kpiRates.Where(m => m.ID == kpiID).Select(m=>m.Rate).FirstOrDefault();
                                maxOrderNumberFollowTemplateId++;
                                var performanceDetail = new Eva_PerformanceDetailEntity()
                                {
                                    KPIID = kpiID,
                                    PerformanceTemplateID = performanceTemplateID,
                                    OrderNumber = maxOrderNumberFollowTemplateId,
                                    Rate =kpiRate
                                };
                                isSuccess = true;
                                Add(performanceDetail);
                            }
                            else
                            {
                                //edit
                            }
                        } 
	                }
                }
            }

            return isSuccess;
        }

        #region edit
        public Eva_PerformanceDetailEntity EditPerformanceDetail(Guid performanceDetailID, double? rate, int? orderNumber)
        {
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var performanceDetailEntity = new Eva_PerformanceDetailEntity();
                var performanceDetail = unitOfWork.CreateQueryable<Eva_PerformanceDetail>(Guid.Empty, m => m.ID == performanceDetailID).FirstOrDefault();
                if (performanceDetail != null)
                {
                    performanceDetailEntity = performanceDetail.CopyData<Eva_PerformanceDetailEntity>();
                    performanceDetailEntity.Rate = rate;
                    performanceDetailEntity.OrderNumber = orderNumber;
                    Edit(performanceDetailEntity);
                }              
                return performanceDetailEntity;
            }
        }
        #endregion

        public List<Eva_PerformanceDetailEntity> GetPerformanceDetailsByTemplateID(Guid templateId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var lstEvaPerformanceDetails = unitOfWork.CreateQueryable<Eva_PerformanceDetail>(Guid.Empty, m => m.PerformanceTemplateID == templateId).ToList().Translate<Eva_PerformanceDetailEntity>();
                return lstEvaPerformanceDetails;
            }
        }

    }
}
