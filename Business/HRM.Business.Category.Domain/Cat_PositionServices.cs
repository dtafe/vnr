using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_PositionServices : BaseService
    {
        #region HRM8.20/7/2014
        ///// <summary>
        ///// Lấy danh sách tất cả chức vụ
        ///// </summary>
        ///// <returns></returns>      
        //public IQueryable<Cat_PositionEntity> Get(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        var data = repo.GetPositions(model);
        //        return data;
        //    }
        //}
        ///// <summary>
        ///// Lấy danh sách tất cả chức vụ cho multi select, dropdown,...
        ///// </summary>
        ///// <returns></returns>   
        //public IQueryable<Cat_PositionMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        var data = repo.GetMulti(text);
        //        return data;
        //    }
        //}
        //public IQueryable<Cat_Position> GetCatPositions()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        //  var data = repo.GetPositions();
        //        return repo.GetPositions();

        //    }
        //}
        //public Cat_Position GetByIdCatPositions(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        Cat_Position CatPosition = new Cat_Position();
        //        CatPosition = repo.GetById(id);
        //        //if (CatPosition.IsDelete == true) CatPosition = null;
        //        return CatPosition;
        //    }
        //}
        //public bool AddCatPositions(Cat_Position CatPosition)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(CatPosition);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public bool UpdateCatPosition(Cat_Position CatPosition)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(CatPosition);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}
        //public bool DeleteCatPosition(int CatPositionId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        Cat_Position CatPosition = new Cat_Position();
        //        CatPosition = repo.GetById(CatPositionId);
        //        try
        //        {
        //            repo.Remove(CatPosition);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        //public bool DeleteEternity(int CatPositionId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_PositionRepository repo = new Cat_PositionRepository(unitOfWork);
        //        Cat_Position CatPosition = new Cat_Position();
        //        CatPosition = repo.GetById(CatPositionId);
        //        try
        //        {
        //            repo.Delete(CatPosition);
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
