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
    public class Cat_HDTJobTypeServices : BaseService
    {
        public string ActionApproved(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_HDTJobTypeRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstHDTJobTypes = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
               
                foreach (var HDTJobType in lstHDTJobTypes)
                {
                    HDTJobType.Status = HDTJobStatus.E_APPROVE.ToString();
                }

                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
