using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_ResignReasonService : BaseService
    {
        #region HRM8 - 20140720
        //public IQueryable<Cat_ResignReasonEntity> GetCatResignReason()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ResignReasonRepository(unitOfWork);
        //        return repo.GetResignReason();
        //    }
        //}

        //public IQueryable<Cat_ResignReasonEntity> GetCatResignReason(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ResignReasonRepository(unitOfWork);
        //        return repo.GetResignReason(model);
        //    }
        //}


        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Cat_ResignReason GetById(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ResignReasonRepository repo = new Cat_ResignReasonRepository(unitOfWork);
        //        var catResignReason = repo.GetById(id);
        //        //if (catLateEarlyRule.IsDelete == true) catLateEarlyRule = null;
        //        return catResignReason;
        //    }
        //}


        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Add(Cat_ResignReason cat)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ResignReasonRepository repo = new Cat_ResignReasonRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(cat);
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
        ///// Edit một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Edit(Cat_ResignReason cat)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ResignReasonRepository repo = new Cat_ResignReasonRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(cat);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        //public bool Remove(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ResignReasonRepository repo = new Cat_ResignReasonRepository(unitOfWork);
        //        var cat = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(cat);
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
        ///// Delete 1 record là xóa luôn record khỏi database
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Delete(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ResignReasonRepository repo = new Cat_ResignReasonRepository(unitOfWork);
        //        var cat = repo.GetById(id);
        //        try
        //        {
        //            repo.Delete(cat);
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
