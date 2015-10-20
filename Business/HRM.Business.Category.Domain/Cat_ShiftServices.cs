using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_ShiftServices :BaseService
    {

        #region MyRegion
        ///// <summary>
        ///// Lấy danh sách Cat_Shift theo store
        ///// </summary>
        ///// <returns></returns>      
        //public IQueryable<Cat_ShiftEntity> GetCat_Shift(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
        //        var data = repo.GetCat_Shifts(model);
        //        return data;
        //    }
        //}
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Shift> GetAll()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
        //        return repo.GetAllCatShift().Where(i => i.IsDelete == null);

        //    }
        //}

        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public IQueryable<Cat_Shift> Get(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        ICat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
        //        return repo.GetAllCatShift().Where(i => i.IsDelete == null);

        //    }
        //}

        //public List<Cat_ShiftEntity> GetCatShift()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
        //        return repo.GetCatShift();

        //    }
        //}

        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Cat_Shift GetById(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
        //        var catshift= repo.GetById(id);
        //        if (catshift.IsDelete == true) catshift = null;
        //        return catshift;
        //    }
        //}

        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Add(Cat_Shift cat)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
        //public bool Edit(Cat_Shift cat)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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

        ///// <summary>
        ///// Remove 1 record là chuyển trạng thái IsDelete=true
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool Remove(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
        //        Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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

        ///// <summary>
        ///// Lấy toàn bộ data dùng store chỉ có ID, Name
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ShiftMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ShiftRepository(unitOfWork);
        //        return repo.GetMulti(text);
        //    }
        //} 
        #endregion
    }
}
