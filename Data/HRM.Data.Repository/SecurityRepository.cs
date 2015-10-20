using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Data.Entity;
using VnResource.Helper.Security;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Data.BaseRepository;

namespace HRM.Data.Entity.Repositories
{
    public class SecurityRepository
    {
        #region Permission

        /// <summary>
        /// Kiểm tra người dùng có quyền thao tác trên một permission hay không?
        /// </summary>
        /// <param name="userID">User cần kiểm tra</param>
        /// <param name="privilegeType">Loại thao tác</param>
        /// <param name="permission">Tên quyền cần kiểm tra</param>
        /// <returns></returns>
        public bool CheckPermission(Guid userID, PrivilegeType privilegeType, string permission)
        {
            bool result = false;

            using (var context = new VnrHrmDataContext())
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                result = unitOfWork.CheckPermission(userID, privilegeType, permission);
            }

            return result;
        }

        public List<string> GetPermission(Guid userID, PrivilegeType privilegeType)
        {
            using (var context = new VnrHrmDataContext())
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                var listDataPermission = unitOfWork.GetUserPermission(userID);

                if (listDataPermission != null && listDataPermission.Count() > 0)
                {
                    return listDataPermission.Where(d => privilegeType.CheckPrivilege(d.PrivilegeNumber.GetInteger())).Select(d => d.ResourceName).Distinct().ToList();
                }
            }

            return new List<string>();
        }

        #endregion
    }
}
