using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Ins_ReportD02ItemRepository : CustomBaseRepository<Ins_ReportD02Item>
    {
        public Ins_ReportD02ItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
