using System;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System.Linq;
using VnResource.Helper.Data;
using System.Data;
using HRM.Infrastructure.Utilities.Helper;

namespace HRM.Business.Evaluation.Domain
{
    public class Eva_LevelServices : BaseService
    {
        
        public Eva_LevelEntity GetLevel(float TotalMark)
        {
            var result = new Eva_LevelEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_LevelRepository(unitOfWork);
                var list = repo.FindBy(x => x.MinimumRating.HasValue && x.MaximumRating.HasValue && x.MinimumRating.Value <= TotalMark && x.MaximumRating.Value >= TotalMark && x.IsDelete == null).ToList();
                if (list != null && list.Count != 0)
                    result = list.FirstOrDefault().CopyData<Eva_LevelEntity>();

            }

            return result;
        }

    }
}
