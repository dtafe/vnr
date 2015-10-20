using System.Collections.Generic;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ShiftItemRepository : GenericRepository<Cat_ShiftItem>
    {
        public Cat_ShiftItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
