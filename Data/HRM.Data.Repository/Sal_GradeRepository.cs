using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Sal_GradeRepository : CustomBaseRepository<Sal_Grade>
    {
        public Sal_GradeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
