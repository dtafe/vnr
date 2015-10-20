using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Lau_LineRepository : CustomBaseRepository<LMS_LineLMS>
    {
        public Lau_LineRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
