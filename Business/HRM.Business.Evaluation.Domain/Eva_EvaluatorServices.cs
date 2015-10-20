using System;
using System.Linq.Dynamic;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;


namespace HRM.Business.Evaluation.Domain
{
    public class Eva_EvaluatorServices : BaseService
    {

        public Eva_EvaluatorEntity AddEvaluators(Eva_EvaluatorEntity model)
        {
            var result = new Eva_EvaluatorEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_EvaluatorRepository(unitOfWork);
                //lay ds evaluator theo profileId
                var profileId = Common.DotNetToOracle(string.Empty.ToString());
                if (model != null && model.ProfileID != null)
                {
                    profileId = Common.DotNetToOracle(model.ProfileID.ToString());
                }

                //var lstEvaluatorentities = GetData<Eva_EvaluatorEntity>(profileId, ConstantSql.hrm_eva_sp_get_EvaluatorByProfileId, ref status);
                if (model != null && model.ID == Guid.Empty)
                {
                    var lstEvaluators = model.EvaluatorIDs == null ? new List<Guid>() : model.EvaluatorIDs;
                    foreach (var evaluator in lstEvaluators)
                    {
                        result = model.CopyData<Eva_EvaluatorEntity>();
                        result.EvaluatorID = evaluator;
                        result.Rate = 1;
                        Add(result);
                    }
                }
                else
                {
                    var evaluator = unitOfWork.CreateQueryable<Eva_Evaluator>(Guid.Empty, m => m.ID == model.ID).FirstOrDefault();
                    if (evaluator != null)
                    {
                        result = evaluator.CopyData<Eva_EvaluatorEntity>();
                        result.Rate = model.Rate;
                        Edit(result);
                    }
                }
            }//end context
            return result;
        }

        public Eva_EvaluatorEntity AddEvaluator360s(Eva_EvaluatorEntity model)
        {
            var result = new Eva_EvaluatorEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_EvaluatorRepository(unitOfWork);
                //lay ds evaluator theo profileId
                var profileId = Common.DotNetToOracle(string.Empty.ToString());
                if (model != null && model.ProfileID != null)
                {
                    profileId = Common.DotNetToOracle(model.ProfileID.ToString());
                }

                //var lstEvaluatorentities = GetData<Eva_EvaluatorEntity>(profileId, ConstantSql.hrm_eva_sp_get_EvaluatorByProfileId, ref status);
                if (model != null && model.ID == Guid.Empty)
                {
                    var lstEvaluators = model.EvaluatorIDs == null ? new List<Guid>() : model.EvaluatorIDs;
                    foreach (var evaluator in lstEvaluators)
                    {
                        result = model.CopyData<Eva_EvaluatorEntity>();
                        result.EvaluatorID = evaluator;
                        result.Rate = 1;
                        result.OrderNo = 0;
                        Add(result);
                    }
                }
                else
                {
                    var evaluator = unitOfWork.CreateQueryable<Eva_Evaluator>(Guid.Empty, m => m.ID == model.ID).FirstOrDefault();
                    if (evaluator != null)
                    {
                        result = evaluator.CopyData<Eva_EvaluatorEntity>();
                        result.Rate = model.Rate;
                        Edit(result);
                    }
                }
            }//end context
            return result;
        }

        public Eva_EvaluatorEntity DeleteListEvaluator(string ID)
        {
            var result = new Eva_EvaluatorEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_EvaluatorRepository(unitOfWork);
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
                    var Eva_Evaluator = repo.GetById(mainID);
                    var ilEva_Evaluator = repo.FindBy(x => x.ProfileID == Eva_Evaluator.ProfileID && x.PerformanceTypeID == Eva_Evaluator.PerformanceTypeID && x.IsDelete == null).ToList();
                    if (ilEva_Evaluator == null || ilEva_Evaluator.Count == 0)
                    {
                        continue;

                    }
                    if (CheckEvaluatorHasUse(ilEva_Evaluator.Select(x => x.ProfileID ?? Guid.Empty).ToList(), Eva_Evaluator.PerformanceTypeID ?? Guid.Empty) == true)
                    {
                        continue;
                    }

                    else
                    {
                        foreach (var item in ilEva_Evaluator)
                        {
                            item.IsDelete = true;
                        }
                        repo.SaveChanges();
                        result = ilEva_Evaluator.FirstOrDefault().CopyData<Eva_EvaluatorEntity>();
                    }


                }



            }
            return result;

        }
        // kiểm tra người đánh giá đã đánh giá 
        public bool CheckEvaluatorHasUse(List<Guid> list, Guid PerformanceTypeID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_EvaluatorRepository(unitOfWork);
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);

                var ilPerformanceEva = repoPE.FindBy(x => list.Contains(x.Eva_Performance.ProfileID ?? Guid.Empty) && x.IsDelete == null && x.Eva_Performance.Eva_PerformanceTemplate.PerformanceTypeID == PerformanceTypeID).ToList();
                if (ilPerformanceEva != null && ilPerformanceEva.Count != 0)
                    return true;

            }

            return false;
        }

    }
}
