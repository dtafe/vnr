using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Lau_MarkerRepository : CustomBaseRepository<LMS_Marker>
    {
        public Lau_MarkerRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        
    } 
}
