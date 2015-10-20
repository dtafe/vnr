using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_ReligionServices :BaseService
    {

        #region MyRegion
        ///// <summary>
        ///// Lấy tất cả các bản ghi 
        ///// Gọi GetAllCatReligion() trong Cat_ReligionRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Religion> GetCatReligion(string name,string code)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        return repo.GetAllCatReligion(name,code);
        //    }
        //}

        ///// <summary>
        ///// Lấy tất cả các bản ghi 
        ///// Gọi GetAllCatReligion() trong Cat_ReligionRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Religion> GetCatReligion()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        return repo.GetAllCatReligion().Where(i => i.IsDelete == null);
        //    }
        //}

        ///// <summary>
        ///// Lấy bản ghi có id
        ///// </summary>
        ///// <returns></returns>
        //public Cat_Religion GetByIdCatReligion(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        Cat_Religion reli = new Cat_Religion();
        //        reli = repo.GetById(id);
        //        return reli;
        //    }
        //}

        ///// <summary>
        ///// Thêm bản ghi mới
        ///// </summary>
        ///// <param name="catReligion"></param>
        ///// <returns></returns>
        //public bool AddCatReligion(Cat_Religion catReligion)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(catReligion);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Update bản ghi
        ///// </summary>
        ///// <param name="catReligion"></param>
        ///// <returns></returns>
        //public bool UpdateCatReligion(Cat_Religion catReligion)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(catReligion);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Delete 
        ///// </summary>
        ///// <param name="catReligionId"></param>
        ///// <returns></returns>
        //public bool DeleteCatReligion(int catReligionId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        Cat_Religion catReligion = new Cat_Religion();
        //        catReligion = repo.GetById(catReligionId);
        //        try
        //        {
        //            repo.Remove(catReligion);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Delete 
        ///// </summary>
        ///// <param name="catReligionEternityId"></param>
        ///// <returns></returns>
        //public bool DeleteEternity(int catReligionId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ReligionRepository repo = new Cat_ReligionRepository(unitOfWork);
        //        Cat_Religion catReligion = new Cat_Religion();
        //        catReligion = repo.GetById(catReligionId);
        //        try
        //        {
        //            repo.Delete(catReligion);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //} 
        #endregion
    }
}
