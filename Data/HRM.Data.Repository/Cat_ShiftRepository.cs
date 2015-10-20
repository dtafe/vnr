using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ShiftRepository : CustomBaseRepository<Cat_Shift>
    {
        public Cat_ShiftRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        
    }
}
