using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Ins_ReportD02Repository : CustomBaseRepository<Ins_ReportD02>
    {
        public Ins_ReportD02Repository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

    }
}
