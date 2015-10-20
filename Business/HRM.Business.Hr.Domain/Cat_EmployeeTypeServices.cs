using HRM.Data.BaseRepository;
using HRM.Data.Category.Model;
using HRM.Data.Hr.Data.Sql.Repositories;
using HRM.Data.Hr.Model;
using System.Linq;
using HRM.Data.Hr.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_EmployeeTypeServices
    {
        public IQueryable<Cat_EmployeeType> GetCatEmployeeTypes()
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                return repo.GetAllCatEmployeeTypes().Where(i => i.IsDelete == null);

            }
        }
        public Cat_EmployeeType GetByIdCatEmployeeTypes(int id)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                Cat_EmployeeType CatEmployeeType = new Cat_EmployeeType();
                CatEmployeeType = repo.GetById(id);
                if (CatEmployeeType.IsDelete == true) CatEmployeeType = null;
                return CatEmployeeType;
            }
        }
        public bool AddCatEmployeeTypes(Cat_EmployeeType CatEmployeeType)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                try
                {
                    repo.Add(CatEmployeeType);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateCatEmployeeType(Cat_EmployeeType CatEmployeeType)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                try
                {
                    repo.Edit(CatEmployeeType);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        public bool DeleteCatEmployeeType(int CatEmployeeTypeId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                Cat_EmployeeType CatEmployeeType = new Cat_EmployeeType();
                CatEmployeeType = repo.GetById(CatEmployeeTypeId);
                try
                {
                    repo.Remove(CatEmployeeType);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public bool DeleteEternity(int CatEmployeeTypeId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_EmployeeTypeRepository repo = new Cat_EmployeeTypeRepository(unitOfWork);
                Cat_EmployeeType CatEmployeeType = new Cat_EmployeeType();
                CatEmployeeType = repo.GetById(CatEmployeeTypeId);
                try
                {
                    repo.Delete(CatEmployeeType);
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
