using HRM.Data.BaseRepository;
using HRM.Data.Category.Model;
using HRM.Data.Hr.Data.Sql.Repositories;
using HRM.Data.Hr.Model;
using System.Linq;
using HRM.Data.Hr.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_JobTitleServices
    {
        public IQueryable<Cat_JobTitle> GetCatJobTitles()
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                return repo.GetAllCatJobTitles().Where(i => i.IsDelete == null);

            }
        }
        public Cat_JobTitle GetByIdCatJobTitles(int id)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                Cat_JobTitle CatJobTitle = new Cat_JobTitle();
                CatJobTitle = repo.GetById(id);
                if (CatJobTitle.IsDelete == true) CatJobTitle = null;
                return CatJobTitle;
            }
        }
        public bool AddCatJobTitles(Cat_JobTitle CatJobTitle)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                try
                {
                    repo.Add(CatJobTitle);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateCatJobTitle(Cat_JobTitle CatJobTitle)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                try
                {
                    repo.Edit(CatJobTitle);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        public bool DeleteCatJobTitle(int CatJobTitleId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                Cat_JobTitle CatJobTitle = new Cat_JobTitle();
                CatJobTitle = repo.GetById(CatJobTitleId);
                try
                {
                    repo.Remove(CatJobTitle);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public bool DeleteEternity(int CatJobTitleId)
        {
            using (var context = new VnrHrmHrDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_JobTitleRepository repo = new Cat_JobTitleRepository(unitOfWork);
                Cat_JobTitle CatJobTitle = new Cat_JobTitle();
                CatJobTitle = repo.GetById(CatJobTitleId);
                try
                {
                    repo.Delete(CatJobTitle);
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
