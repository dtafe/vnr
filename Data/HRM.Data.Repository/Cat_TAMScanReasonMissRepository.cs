using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_TAMScanReasonMissRepository : CustomBaseRepository<Cat_TAMScanReasonMiss>
    {
        public Cat_TAMScanReasonMissRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
