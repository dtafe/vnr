using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Ins_InsuranceRecordRepository : CustomBaseRepository<Ins_InsuranceRecord>
    {
        public Ins_InsuranceRecordRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
