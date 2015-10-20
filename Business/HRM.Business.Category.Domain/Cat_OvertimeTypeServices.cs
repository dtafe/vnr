using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_OvertimeTypeServices : BaseService
    {

        #region HRM8.20/7/2014
        ///// <summary>
        ///// Lấy danh sách Cat_OvertimeType
        ///// </summary>
        ///// <returns></returns>      
        //public IQueryable<Cat_OvertimeTypeEntity> GetCat_OvertimeType(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var data = repo.GetOvertimeTypes(model);
        //        return data;
        //    }
        //}
        ///// <summary>
        ///// Lấy tất cả các bản ghi có thuộc tính IsDelete là null
        ///// Gọi GetAllCatOvertimeType() trong Cat_OvertimeTypeRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_OvertimeType> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var data = repo.GetAllCatOvertimeType().Where(i => i.IsDelete == null);
        //        return data;
        //    }

        //}

        //public IQueryable<Cat_OvertimeType> Get(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var data = repo.GetAllCatOvertimeType().Where(i => i.IsDelete == null);
        //        return data;
        //    }

        //}


        //public List<Cat_OvertimeTypeEntity> GetOvertimeType()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var data = repo.GetOvertimeType();
        //        return data;
        //    }

        //}

        ///// <summary> Tìm kiếm bản ghi có theo tham số id </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Cat_OvertimeType GetById(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        Cat_OvertimeType catOvertimeType = new Cat_OvertimeType();
        //        catOvertimeType = repo.GetById(id);
        //        if (catOvertimeType.IsDelete == true) catOvertimeType = null;
        //        return catOvertimeType;
        //    }
        //}

        //public IQueryable<Cat_OvertimeType> GetByCode(string code)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        ICat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var catOvertimeType = repo.FindBy(p=>p.Code == code && p.IsDelete == null);
        //        return catOvertimeType;
        //    }
        //}   

        //#region Add/Edit/Remove/Delete

        ///// <summary>
        ///// Thêm bản ghi mới
        ///// </summary>
        ///// <param name="catOvertimeType"></param>
        ///// <returns></returns>
        //public string Add(Cat_OvertimeType catOvertimeType)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(catOvertimeType);
        //            repo.SaveChanges();
        //            return "0";
        //        }
        //        catch(Exception ex)
        //        {
        //            return ex.Message;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Edit một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public string Edit(Cat_OvertimeType cat)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(cat);
        //            repo.SaveChanges();
        //            return "0";
        //        }
        //        catch (Exception ex)
        //        {
        //            return ex.Message;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Update thuộc tính IsDelete là true
        ///// </summary>
        ///// <param name="catOvertimeTypeId"></param>
        ///// <returns></returns>
        //public bool Delete(int catOvertimeTypeId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_OvertimeTypeRepository repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        Cat_OvertimeType catOvertimeType = new Cat_OvertimeType();
        //        catOvertimeType = repo.GetById(catOvertimeTypeId);
        //        try
        //        {
        //            repo.Remove(catOvertimeType);
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
        //public IQueryable<Cat_OvertimeTypeMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_OvertimeTypeRepository(unitOfWork);
        //        return repo.GetMulti(text);
        //    }
        //}
        #endregion


    }
}
