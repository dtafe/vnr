using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class FIN_ApproverECLAIMRepository : CustomBaseRepository<FIN_ApproverECLAIM>
    {
        public FIN_ApproverECLAIMRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
