using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Data.Main.Data.Sql;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;
namespace HRM.Business.Category.Domain
{
    public class Cat_WorkPlaceServices : BaseService
    {
        public int? GetMaxWorkPlaceOrder()
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var maxOrder = unitOfWork.CreateQueryable<Cat_WorkPlace>(Guid.Empty).Max(m => m.OrderNumber);
                    maxOrder++;
                    return maxOrder;
                }
                catch
                {
                    return 1;
                }

            }
        }
    }
}
