using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Att_TimeSheetRepository : CustomBaseRepository<Att_TimeSheet>
    {
        public Att_TimeSheetRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
