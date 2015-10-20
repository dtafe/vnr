using System.Collections.Generic;
using HRM.Business.Category.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain
{
    public class Cat_ShiftItemServices : BaseService
    {
        public List<Cat_ShiftItemEntity> GetCatShiftItem( string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_ShiftItemRepository repo = new Cat_ShiftItemRepository(unitOfWork);
                BaseService service = new BaseService();
                string status = string.Empty;
                return service.GetData<Cat_ShiftItemEntity>(ConstantSql.hrm_cat_sp_get_ShiftItem, string.Empty, userLogin, ref status);

            }
        }
    }
}
