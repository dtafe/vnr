using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Data.Attendance.Model;
using System.Linq;
using HRM.Data.Attendance.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_LeaveDayTypeServices
    {
        /// <summary>
        /// Lấy tất cả các bản ghi có thuộc tính IsDelete là null
        /// Gọi GetAllCatLeaveDayType() trong Cat_LeaveDayTypeRepository
        /// </summary>
        /// <returns></returns>
        public IQueryable<Cat_LeaveDayType> GetCatLeaveDayType()
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_LeaveDayTypeRepository repo = new Cat_LeaveDayTypeRepository(unitOfWork);
                return repo.GetAllCatLeaveDayType().Where(i => i.IsDelete == null);
            }

        }

        /// <summary>
        /// Tìm kiếm bản ghi có theo tham số id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_LeaveDayType GetByIdCatLeaveDayType(int id)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_LeaveDayTypeRepository repo = new Cat_LeaveDayTypeRepository(unitOfWork);
                Cat_LeaveDayType catLeaveDayType = new Cat_LeaveDayType();
                catLeaveDayType = repo.GetById(id);
                if (catLeaveDayType.IsDelete == true) catLeaveDayType = null;
                return catLeaveDayType;
            }
        }

        /// <summary>
        /// Thêm bản ghi mới
        /// </summary>
        /// <param name="catLeaveDayType"></param>
        /// <returns></returns>
        public bool AddCatLeaveDayType(Cat_LeaveDayType catLeaveDayType)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_LeaveDayTypeRepository repo = new Cat_LeaveDayTypeRepository(unitOfWork);
                try
                {
                    repo.Add(catLeaveDayType);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Update bản ghi
        /// </summary>
        /// <param name="catLeaveDayType"></param>
        /// <returns></returns>
        public bool UpdateCatLeaveDayType(Cat_LeaveDayType catLeaveDayType)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_LeaveDayTypeRepository repo = new Cat_LeaveDayTypeRepository(unitOfWork);
                try
                {
                    repo.Edit(catLeaveDayType);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Update thuộc tính IsDelete là true
        /// </summary>
        /// <param name="catLeaveDayTypeId"></param>
        /// <returns></returns>
        public bool DeleteCatLeaveDayType(int catLeaveDayTypeId)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_LeaveDayTypeRepository repo = new Cat_LeaveDayTypeRepository(unitOfWork);
                Cat_LeaveDayType catLeaveDayType = new Cat_LeaveDayType();
                catLeaveDayType = repo.GetById(catLeaveDayTypeId);
                try
                {
                    repo.Remove(catLeaveDayType);
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
