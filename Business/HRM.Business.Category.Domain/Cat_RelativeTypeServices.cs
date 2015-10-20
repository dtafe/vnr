using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_RelativeTypeServices :BaseService
    {
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_RelativeType> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        return repo.GetAll().Where(i => i.IsDelete == null);
        //    }
        //}

        ///// <summary>
        ///// Lấy ds tất cả Địa chỉ
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_RelativeType> GetRelativeType(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        return repo.GetRelativeTypeType(model);
        //    }
        //}

        //public List<Cat_RelativeTypeMultiEntity> GetRelativeTypeByIds(string selectedIds)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        return repo.GetRelativeTypeByIds(selectedIds);
        //    }
        //}


        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Cat_RelativeTypeMultiEntity GetById(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        var catRelativeType = repo.GetCatRelativeTypeByID(id);
        //        return catRelativeType;
        //    }
        //}

        //#region Add/Edit/Remove/Delete
        
        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="catRelativeType"></param>
        ///// <returns></returns>
        //public bool Add(Cat_RelativeType catRelativeType)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        ICat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(catRelativeType);
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
        ///// <param name="catRelativeType"></param>
        ///// <returns></returns>
        //public bool Edit(Cat_RelativeType catRelativeType)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        ICat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(catRelativeType);
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
        //        Cat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        var catRelativeType = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(catRelativeType);
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
        //        Cat_RelativeTypeRepository repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        var catRelativeType = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(catRelativeType);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        //#endregion


        ///// <summary>
        ///// Lấy toàn bộ data dùng store chỉ có ID, Name
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_RelativeTypeMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_RelativeTypeRepository(unitOfWork);
        //        return repo.GetMulti(text);
        //    }
        //}

    }
}
