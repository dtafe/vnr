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
    public class Cat_HDTJobTypePriceServices : BaseService
    {
        public string ActionApproved(List<Guid> selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Cat_HDTJobTypePriceRepository(unitOfWork);
                var lstHDTJobTypePrices = repo.FindBy(m => m.ID != null && selectedIds.Contains(m.ID)).ToList();
                foreach (var HDTJobTypePrice in lstHDTJobTypePrices)
                {
                    HDTJobTypePrice.Status = HDTJobStatus.E_APPROVE.ToString();
                }
                repo.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
