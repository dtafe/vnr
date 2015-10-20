using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;
using HRM.Data.Main.Data;
using HRM.Data.Entity;
using System;
using System.Collections.Generic;
namespace HRM.Business.Canteen.Domain
{
    /// <summary> Phụ Cấp Ăn </summary>
    public class Can_MealAllowanceToMoneyServices : BaseService
    {
        public void SubmitStatus(List<Guid> selectedIds, string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Can_MealAllowanceToMoneyRepository(unitOfWork);
                var lstAllowanceToMoney = repo.FindBy(m => m.ID != null && selectedIds.Contains(m.ID)).ToList();
                foreach (var AllowanceToMoney in lstAllowanceToMoney)
                {
                    AllowanceToMoney.Status = status;
                }
                repo.SaveChanges();

            }
        }
    }
}
