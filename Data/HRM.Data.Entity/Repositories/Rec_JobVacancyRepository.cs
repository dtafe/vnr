using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Rec_JobVacancyRepository : CustomBaseRepository<Rec_JobVacancy>
    {
        public Rec_JobVacancyRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
