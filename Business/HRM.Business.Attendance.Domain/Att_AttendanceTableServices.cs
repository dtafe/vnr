using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Business.Attendance.Models;
using System;
using HRM.Business.Category.Models;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using System.Data.SqlTypes;
//using HRM.Data.Hr.Model;
//using HRM.Data.Category.Model;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using HRM.Data.Attendance.Data.Sql.Repositories;
//using HRM.Data.HrmSystem.Model;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
//using HRM.Data.Hr.Data.Sql.Repositories;
//using HRM.Data.Hr.Sql;
//using HRM.Data.Category.Data.Sql.Repositories;

namespace HRM.Business.Attendance.Domain
{
    public class Att_AttendanceTableServices : BaseService
    {
        #region Methods

        ///// <summary>
        ///// Lấy danh sách tất cả tăng ca
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AttendanceTableEntity> GetAttendanceTable(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AttendanceTableRepository repo = new Att_AttendanceTableRepository(unitOfWork);
        //        return repo.GetAttendanceTable();
        //    }
        //}
        ///// <summary>
        ///// Lấy toàn bộ data
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_AttendanceTable> Get()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AttendanceTableRepository repo = new Att_AttendanceTableRepository(unitOfWork);
        //        return repo.GetAll().Where(i => i.IsDelete == null);
        //    }
        //}
        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Att_AttendanceTable Get(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AttendanceTableRepository repo = new Att_AttendanceTableRepository(unitOfWork);
        //        var data = repo.GetById(id);
        //        if (data.IsDelete == true) data = null;
        //        return data;
        //    }
        //}

        ///// <summary>
        ///// Lấy dữ liệu theo Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Att_AttendanceTable GetByProfileId(int ProfileId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        IAtt_AttendanceTableRepository repo = new Att_AttendanceTableRepository(unitOfWork);
        //        var data = repo.GetAttendanceTableByProfileId(ProfileId);
        //        return data;
        //    }
        //}


        //public List<Att_AttendanceTableEntity> GetAttendanceTableByIds(string selectedIds)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
        //        return repo.GetAttendanceTableByIds(selectedIds);
        //    }
        //}

        ///// <summary>
        ///// Thêm mới một record
        ///// </summary>
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Add(Att_AttendanceTable model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
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
        ///// <param name="cat"></param>
        ///// <returns></returns>
        //public bool Edit(Att_AttendanceTable model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
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
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
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
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
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
        ///// Lấy dữ liệu AttendanceTable theo Id
        ///// </summary>
        ///// <param name="overtimeId"></param>
        ///// <returns></returns>
        //public Att_AttendanceTable GetAttendanceTableById(int attendanceTableId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_AttendanceTableRepository(unitOfWork);
        //        var data = repo.GetAttendanceTableById(attendanceTableId);
        //        if (data.IsDelete == true)
        //        {
        //            data = null;
        //        }
        //        return data;
        //    }
        //} 

        #endregion

        public Att_AttendanceTable GetAttTableByCutOffAndProfileId(Guid profileId, Guid cutOffId, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_AttendanceTableRepository(unitOfWork);
                string status = string.Empty;
                List<object> lstObject = new List<object>();
                lstObject.Add(profileId);
                lstObject.Add(cutOffId);
                var data = repo.GetData(lstObject, ConstantSql.att_get_AttTable_CfId_PfId, UserLogin, ref status).FirstOrDefault();
                return data;
            }
        }
    }
}