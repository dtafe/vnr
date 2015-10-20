using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{

    public class Lau_TamScanLogRepository : CustomBaseRepository<LMS_TamScanLogLMS>
    {
        public Lau_TamScanLogRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
