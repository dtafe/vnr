using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Data.Entity;
using VnResource.Helper.Security;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Data.Entity.Repositories
{
    public class SecurityRepository
    {
        #region Propeties

        static Dictionary<Guid, List<DataPermission>> userPermissions;

        /// <summary>
        /// Cache tất cả những thông tin phân quyền liên quan đến một user.
        /// </summary>
        private static Dictionary<Guid, List<DataPermission>> UserPermissions
        {
            get
            {
                if (userPermissions == null)
                {
                    userPermissions = new Dictionary<Guid, List<DataPermission>>();
                }

                return userPermissions;
            }
        }

        #endregion

        #region Permission

        /// <summary>
        /// Reset lại quyển của một user
        /// </summary>
        /// <param name="userID"></param>
        public void ResetPermission(Guid userID)
        {
            if (UserPermissions.ContainsKey(userID))
            {
                UserPermissions[userID] = null;
                UserPermissions.Remove(userID);
            }
        }

        /// <summary>
        /// Kiểm tra thông tin phân quyền của một người dùng.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<DataPermission> GetPermission(Guid userID)
        {
            List<DataPermission> result = null;

            if (UserPermissions != null && UserPermissions.ContainsKey(userID))
            {
                result = UserPermissions[userID];
            }
            else
            {
                //Chạy 1 lần đầu tiên cho mỗi user
                result = LoadPermission(userID);
            }

            return result;
        }

        /// <summary>
        /// Tải tất cả thông tin phân quyền của một người dùng.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<DataPermission> LoadPermission(Guid userID)
        {
            using (var context = new VnrHrmDataContext())
            {
                //Nguồn gốc của phân quyền: Kiểm tra user thuộc những nhóm người dùng, trung tâm, chi nhánh và nhóm dữ liệu nào?
                var listDataPermission = context.Set<Sys_DataPermission>().Where(d => (!d.IsDelete.HasValue || !d.IsDelete.Value)
                    && d.Sys_Group != null && d.Sys_Group.IsActivate && (!d.Sys_Group.IsDelete.HasValue || !d.Sys_Group.IsDelete.Value)
                        && d.UserID == userID).Select(d => new DataPermission
                        {
                            UserID = d.UserID,
                            GroupID = d.GroupID,
                            Branches = d.Branches,
                            DataGroups = d.DataGroups
                        }).ToList();

                //Lấy tất cả danh sách nhóm người dùng mà user này trực thuộc (được phân quyền)
                List<Guid?> listGroupID = listDataPermission.Select(d => d.GroupID).Distinct().ToList();

                var listGroupPermission2 = context.Set<Sys_GroupPermission2>().Where(d => (!d.IsDelete.HasValue || !d.IsDelete.Value)
                    && listGroupID.Contains(d.GroupID)).ToList();


                var listGroupPermission = listGroupPermission2.Select(d => new GroupPermission
                    {
                        GroupID = d.GroupID,
                        PrivilegeNumber = Convert.ToInt32(d.PrivilegeNumber??0),
                        ResourceName = d.Sys_Resource.ResourceName
                    }).ToList();

                foreach (var dataPermission in listDataPermission)
                {
                    if (dataPermission.Branches != null)
                    {
                        dataPermission.BranchNumbers = dataPermission.Branches.ToNumbers();

                        if (dataPermission.BranchNumbers != null && dataPermission.BranchNumbers.Count() > 0)
                        {
                            if (!dataPermission.BranchNumbers.Any(d => d > 0))
                            {
                                dataPermission.ListBranchID = new int[] { -1 }.ToList();
                            }
                            else
                            {
                                dataPermission.ListBranchID = context.Set<Cat_OrgStructure>().Where(d => (!d.IsDelete.HasValue || !d.IsDelete.Value)
                                    && dataPermission.BranchNumbers.Contains(d.OrderNumber)).Select(d => d.OrderNumber).ToList();
                            }
                        }
                    }

                    if (dataPermission.DataGroups != null)
                    {
                        dataPermission.DataGroupNumbers = dataPermission.DataGroups.ToNumbers();

                        if (dataPermission.DataGroupNumbers != null && dataPermission.DataGroupNumbers.Count() > 0)
                        {
                            //var listDataGroup = context.Set<Sys_DataGroup>().Where(d => (!d.IsDelete.HasValue || !d.IsDelete.Value)
                            //    && dataPermission.DataGroupNumbers.Contains(d.GroupNumber)).Select(d => d.ID).ToList();
                        }
                    }

                    dataPermission.ListGroupPermission = listGroupPermission.Where(d =>
                        d.GroupID == dataPermission.GroupID).ToList();
                }

                if (UserPermissions.ContainsKey(userID))
                {
                    UserPermissions[userID] = listDataPermission;
                }
                else
                {
                    //Lưu thông tin phân quyền của người dùng này
                    UserPermissions.Add(userID, listDataPermission);
                }

                return listDataPermission;
            }
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
            bool result = false;

            if (userID != Guid.Empty)
            {
                var listDataPermission = GetPermission(userID);

                if (listDataPermission != null && listDataPermission.Count() > 0)
                {
                    result = listDataPermission.Any(d => d.ListGroupPermission != null && d.ListGroupPermission.Any(p =>
                        p.ResourceName == permission && SecurityHelper.CheckPrivilege(privilegeType, p.PrivilegeNumber.GetInteger())));
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        public List<string> GetPermission(Guid userID, PrivilegeType privilegeType)
        {
            if (userID != Guid.Empty)
            {
                var listDataPermission = GetPermission(userID);

                if (listDataPermission != null && listDataPermission.Count() > 0)
                {
                    var listGroupPermission = listDataPermission.Where(d => d.ListGroupPermission != null && d.ListGroupPermission.Any(p =>
                            SecurityHelper.CheckPrivilege(privilegeType, p.PrivilegeNumber.GetInteger()))).SelectMany(d => d.ListGroupPermission);
                    return listGroupPermission.Where(m=>m.PrivilegeNumber!=0).Select(d=>d.ResourceName).Distinct().ToList();
                }
            }
            return new List<string>();

            
        }

        #endregion
    }

    public class GroupPermission
    {
        public Guid ID { get; set; }
        public Guid? GroupID { get; set; }
        public Guid? ResourceID { get; set; }
        public string ModuleName { get; set; }
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public int? PrivilegeNumber { get; set; }
        public bool IsVirtualPermission { get; set; }

        public string Name
        {
            get
            {
                return ResourceName.TranslateString();
            }
        }

        public bool? All
        {
            get
            {
                bool? all = null;

                int allNumber = (int)PrivilegeType.Create + (int)PrivilegeType.Delete + (int)PrivilegeType.Modify
                    + (int)PrivilegeType.View + (int)PrivilegeType.Export + (int)PrivilegeType.Import;

                if (PrivilegeNumber > 0 && PrivilegeNumber < allNumber)
                {
                    all = null;
                }
                else
                {
                    all = PrivilegeNumber >= allNumber;
                }

                return all;
            }
            set
            {
                if (value.GetBoolean())
                {
                    PrivilegeNumber = (int)PrivilegeType.Create + (int)PrivilegeType.Delete + (int)PrivilegeType.Modify
                        + (int)PrivilegeType.View + (int)PrivilegeType.Export + (int)PrivilegeType.Import;
                }
                else if (!value.GetBoolean())
                {
                    PrivilegeNumber = (int)PrivilegeType.None;
                }
            }
        }

        public bool Export
        {
            get
            {
                return PrivilegeType.Export.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.Export.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.Export;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.Export;
                    }
                }
            }
        }

        public bool Import
        {
            get
            {
                return PrivilegeType.Import.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.Import.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.Import;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.Import;
                    }
                }
            }
        }

        public bool View
        {
            get
            {
                return PrivilegeType.View.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.View.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.View;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.View;
                    }
                }
            }
        }

        public bool Delete
        {
            get
            {
                return PrivilegeType.Delete.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.Delete.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.Delete;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.Delete;
                    }
                }
            }
        }

        public bool Edit
        {
            get
            {
                return PrivilegeType.Modify.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.Modify.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.Modify;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.Modify;
                    }
                }
            }
        }


        public bool Create
        {
            get
            {
                return PrivilegeType.Create.CheckPrivilege(PrivilegeNumber.GetInteger());
            }
            set
            {
                if (PrivilegeType.Create.CheckPrivilege(PrivilegeNumber.GetInteger()))
                {
                    if (!value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() - (int)PrivilegeType.Create;
                    }
                }
                else
                {
                    if (value)
                    {
                        PrivilegeNumber = PrivilegeNumber.GetInteger() + (int)PrivilegeType.Create;
                    }
                }
            }
        }
    }

    public class DataPermission
    {
        public Guid ID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? GroupID { get; set; }
        public string GroupName { get; set; }

        public byte[] Branches { get; set; }
        public List<int> BranchNumbers { get; set; }
        public List<int> ListBranchID { get; set; }
        public string BranchNames { get; set; }

        public byte[] DataGroups { get; set; }
        public List<int> DataGroupNumbers { get; set; }
        public List<int> ListDataGroupID { get; set; }
        public string DataGroupNames { get; set; }

        /// <summary>
        /// Lưu thông tin group permission của user
        /// </summary>
        public List<GroupPermission> ListGroupPermission
        {
            get;
            set;
        }


    }
}
