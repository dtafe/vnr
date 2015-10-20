using System.Collections.Generic;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data.Sql.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
using HRM.Business.Attendance.Models;
//using HRM.Data.Main.Sql;
using HRM.Business.Main.Domain;

namespace HRM.Business.Attendance.Domain
{
    public class Att_AttendanceTableItemServices : BaseService
    {
        #region MyRegion
        ///// <summary>
        ///// Lấy danh sách tất cả tăng ca
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AttendanceTableItemEntity> GetAttendanceTableItem()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        return repo.GetAttendanceTableItem();
        //    }
        //}
        ///// <summary>
        ///// Lấy toàn bộ data
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AttendanceTableItem> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        return repo.GetAll().Where(i => i.IsDelete == null);
        //    }
        //}
        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Att_AttendanceTableItem Get(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        var data = repo.GetAttendanceTableItemById(id);
        //        return data;
        //    }
        //}

        ///// <summary>
        ///// Lấy dữ liệu theo ProfileId
        ///// </summary>
        ///// <param name="attendanceTableId"></param>
        ///// <returns></returns>
        //public List<Att_AttendanceTableItem> GetByAttendanceTableId(int attendanceTableId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        var data = repo.GetAttendanceTableItemByAttendanceTableId(attendanceTableId);
        //        return data;
        //    }
        //}

        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool Add(Att_AttendanceTableItem model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(model);
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
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool Edit(Att_AttendanceTableItem model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(model);
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
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Remove(data);
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
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        try
        //        {
        //            repo.Delete(data);
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
        ///// Lấy dữ liệu AttendanceTableItem theo Id
        ///// </summary>
        ///// <param name="attendanceTableItemId"></param>
        ///// <returns></returns>
        //public Att_AttendanceTableItem GetAttendanceTableItemById(int attendanceTableItemId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableItemRepository(unitOfWork);
        //        var data = repo.GetAttendanceTableItemById(attendanceTableItemId);
        //        if (data.IsDelete == true)
        //        {
        //            data = null;
        //        }
        //        return data;
        //    }
        //}
        #endregion
    }
}
