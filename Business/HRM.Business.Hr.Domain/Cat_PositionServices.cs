using HRM.Data.BaseRepository;
using HRM.Data.Category.Model;
using HRM.Data.Hr.Data.Sql.Repositories;
using HRM.Data.Hr.Model;
using System.Linq;
using HRM.Data.Hr.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_PositionServices
    {
        public IQueryable<Cat_Position> GetCatPositions()
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                return repo.GetAllCatPositions().Where(i => i.IsDelete == null);

            }
        }
        public Cat_Position GetByIdCatPositions(int id)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                Cat_Position CatPosition = new Cat_Position();
                CatPosition = repo.GetById(id);
                if (CatPosition.IsDelete == true) CatPosition = null;
                return CatPosition;
            }
        }
        public bool AddCatPositions(Cat_Position CatPosition)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                try
                {
                    repo.Add(CatPosition);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateCatPosition(Cat_Position CatPosition)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                try
                {
                    repo.Edit(CatPosition);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        public bool DeleteCatPosition(int CatPositionId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                Cat_Position CatPosition = new Cat_Position();
                CatPosition = repo.GetById(CatPositionId);
                try
                {
                    repo.Remove(CatPosition);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public bool DeleteEternity(int CatPositionId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
                Cat_Position CatPosition = new Cat_Position();
                CatPosition = repo.GetById(CatPositionId);
                try
                {
                    repo.Delete(CatPosition);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

    }
}
