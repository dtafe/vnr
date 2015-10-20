using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Data.Main.Data.Sql;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Business.Category.Domain
{
    public class Cat_HDTJobGroupServices : BaseService
    {
        public ResultsObject UpdateStatus(string Ids,string Status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoHDTJobGroup = new CustomBaseRepository<Cat_HDTJobGroup>(unitOfWork);
                List<Cat_HDTJobGroup> listHDTJob = repoHDTJobGroup.FindBy(m => m.IsDelete != true).ToList();
                string[] ListIds = Ids.Split(',');
                listHDTJob = listHDTJob.Where(m => ListIds.Contains(m.ID.ToString())).ToList();
                listHDTJob.ForEach(m => m.Status = Status);
                unitOfWork.SaveChanges();
                return new ResultsObject();
            }
        }
    }
}
