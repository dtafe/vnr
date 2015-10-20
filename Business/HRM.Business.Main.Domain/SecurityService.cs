using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Security;
using VnResource.Helper.Data;

namespace HRM.Business.Main.Domain
{
    public class SecurityService
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// Kiểm tra người dùng có quyền thao tác trên một permission hay không?
        /// </summary>
        /// <param name="userID">User cần kiểm tra</param>
        /// <param name="privilegeType">Loại thao tác</param>
        /// <param name="permission">Tên quyền cần kiểm tra</param>
        /// <returns></returns>
        public bool CheckPermission(Guid userID, PrivilegeType privilegeType, string permission)
        {
           // return true;

            //if (Common.UserNameSystem == userID.ToString())
            //{
            //    return true;
            //}

            if (userID == Guid.Empty)
            {
                return true;
            }


            SecurityRepository security = new SecurityRepository();
            return security.CheckPermission(userID, privilegeType, permission);
        }
        /// <summary>
        /// [Hien.Nguyen] - Lấy tất cả các quyền của user
        /// Chỉ lấy quyền view, các quyền khác không lấy
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<string> GetAllPermission(Guid userID)
        {
            SecurityRepository security = new SecurityRepository();
            return security.GetPermission(userID, PrivilegeType.View);
        }
    }
}
