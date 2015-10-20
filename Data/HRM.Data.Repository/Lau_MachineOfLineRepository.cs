using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{

    public class Lau_MachineOfLineRepository : CustomBaseRepository<LMS_MachineOfLineLMS>
    {
        public Lau_MachineOfLineRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
