using System;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    /// <summary> Quản Lý Phụ Cấp Ăn </summary>
    public class Can_MealAllowanceToMoneyRepository : CustomBaseRepository<Can_MealAllowanceToMoney>
    {
        public Can_MealAllowanceToMoneyRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
