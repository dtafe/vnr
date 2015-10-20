using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Can_MealRecordMissingRepository : CustomBaseRepository<Can_MealRecordMissing>
    {
        public Can_MealRecordMissingRepository(IUnitOfWork unitOfWork): base(unitOfWork){}
    }
}
