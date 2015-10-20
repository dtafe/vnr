using System.Linq;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_UserRepository : CustomBaseRepository<Sys_UserInfo>
    {
        public Sys_UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Sys_UserInfoEntity Login(string userName, string password)
        {
            var result = this.DbSet.FirstOrDefault(p => p.UserLogin.ToLower() == userName.ToLower() && p.Password == password 
                && p.IsDelete == null );
            if (result != null)
            {
                return result.Copy<Sys_UserInfoEntity>();
            }

            return null;
        }

        //public IQueryable<Sys_User> GetAllUsers()
        //{
        //    IQueryable<Sys_User> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        //public Sys_User GetUser(int userID)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_User>("exec GetUser @UserID", new SqlParameter
        //    {
        //        ParameterName = "UserID",
        //        Value = userID
        //    }).ToList<Sys_User>().FirstOrDefault<Sys_User>();
        //}

        //public IQueryable<Sys_UserInfoEntity> GetUsers(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_sys_sp_get_users);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_UserInfoEntity>(param.SqlQuery, param.Params).ToList<Sys_UserInfoEntity>().AsQueryable();
        //}

        /// <summary>
        /// Lấy danh sách users (sử dụng store hrm_hr_sp_get_User)
        /// </summary>
        /// <returns></returns>
        //public IQueryable<Sys_UserInfoEntity> GetUsers()
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_UserInfoEntity>(ConstantSql.hrm_hr_sp_get_User)
        //        .ToList<Sys_UserInfoEntity>().AsQueryable(); 
        //}


        /// <summary>
        /// Lấy đối tượng User theo Id (sử dụng store hrm_hr_sp_get_UserById)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public Sys_UserInfoEntity GetUserByUserId(int userId)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_UserInfoEntity>(ConstantSql.hrm_hr_sp_get_UserById, new SqlParameter
        //    {
        //        ParameterName = "userId",
        //        Value = userId
        //    }).ToList<Sys_UserInfoEntity>().FirstOrDefault<Sys_UserInfoEntity>();
        //}

        /// <summary>
        /// Lấy tất cả danh sách Loại Hợp đồng show lên Multi
        /// </summary>
        /// <returns></returns>
        //public IQueryable<Sys_UserMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    return this.UnitOfWork.Context.Database
        //        .SqlQuery<Sys_UserMultiEntity>(ConstantSql.hrm_sys_sp_get_user_multi
        //        , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //        .ToList<Sys_UserMultiEntity>()
        //        .AsQueryable();
        //}
    }
}
