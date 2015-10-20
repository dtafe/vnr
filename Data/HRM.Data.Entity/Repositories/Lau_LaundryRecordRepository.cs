using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Lau_LaundryRecordRepository : CustomBaseRepository<LMS_LaundryRecord>
    {
        public Lau_LaundryRecordRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
