using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Can_MealAllowanceTypeSettingRepository : CustomBaseRepository<Can_MealAllowanceTypeSetting>
    {
        public Can_MealAllowanceTypeSettingRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
