using System.Threading;
using HRM.Infrastructure.Utilities;
using System;
using System.Linq;
using System.Data.Entity;
using VnResource.Helper.Data;
using VnResource.Helper.Linq;
using VnResource.Helper.Utility;
using System.Collections.Generic;
using HRM.Data.Entity;
using System.Web;
using System.Collections;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using VnResource.Helper.Setting;
using System.Data.Entity.Core.Objects;
using VnResource.Helper.Security;
using System.Reflection;
using System.Linq.Expressions;
using VnResource.Data.Entity;
using VnResource.Helper.Ado;
using System.Data.SqlClient;

namespace HRM.Data.BaseRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties

        private string[] excludedFields = new string[] 
        { 
            VnResource.Helper.Utility.DefaultConstants.ID,
            VnResource.Helper.Utility.DefaultConstants.IsDelete,
            VnResource.Helper.Utility.DefaultConstants.DateCreate,
            VnResource.Helper.Utility.DefaultConstants.DateUpdate,
            VnResource.Helper.Utility.DefaultConstants.UserCreate,
            VnResource.Helper.Utility.DefaultConstants.UserUpdate
        };

        /// <summary>
        /// Danh sách field không tracking log.
        /// </summary>
        public string[] ExcludedFields
        {
            get { return excludedFields; }
            set { excludedFields = value; }
        }

        public DbContext Context { get; internal set; }

        private static Dictionary<Guid, List<Guid>> userMasterDataGroup;

        public static Dictionary<Guid, List<Guid>> UserMasterDataGroup
        {
            get
            {
                if (userMasterDataGroup == null)
                {
                    userMasterDataGroup = new Dictionary<Guid, List<Guid>>();
                }

                return UnitOfWork.userMasterDataGroup;
            }
        }

        #endregion

        #region Constructors

        public UnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        #endregion

        #region CurrentDate

        public DateTime CurrentDate()
        {
            return DateTime.Now;
        }

        #endregion

        #region SaveChanges

        public DataErrorCode SaveChanges()
        {
            return SaveChanges(Guid.Empty);
        }

        public DataErrorCode SaveChanges(Guid userID)
        {
            return SaveChanges(userID, false);
        }

        public DataErrorCode SaveChanges(Guid userID, bool isFromImport)
        {
            DataErrorCode result = DataErrorCode.Unknown;

            var listItem = Context.ChangeTracker.Entries().Where(d => d.State == EntityState.Added
                || d.State == EntityState.Modified || d.State == EntityState.Deleted).ToList();

            var listChange = listItem.Select(d => d.Entity);

            if (CheckLock(listChange.ToArray()))
            {
                result = DataErrorCode.Locked;
            }
            else
            {
                try
                {
                    UserInfo userInfo = new UserInfo
                    {
                        ID = userID,
                        ServerDate = CurrentDate()
                    };

                    if (userID != Guid.Empty)
                    {
                        userInfo.LoginName = Context.Set<Sys_UserInfo>().Where(d =>
                            d.ID == userID).Select(d => d.UserLogin).FirstOrDefault();
                    }

                    foreach (var item in listItem)
                    {
                        if (item.Entity != null)
                        {
                            if (item.State == EntityState.Added)
                            {
                                var entityID = item.Entity.GetPropertyValue(DefaultConstants.ID);
                                if (entityID == null || entityID.GetString() == Guid.Empty.ToString())
                                {
                                    item.Entity.SetPropertyValue(DefaultConstants.ID, Guid.NewGuid());
                                }

                                if (item.Entity.GetPropertyValue(DefaultConstants.DateUpdate) == null)
                                {
                                    item.Entity.SetPropertyValue(DefaultConstants.DateUpdate, userInfo.ServerDate);
                                }

                                if (item.Entity.GetPropertyValue(DefaultConstants.DateCreate) == null)
                                {
                                    item.Entity.SetPropertyValue(DefaultConstants.DateCreate, userInfo.ServerDate);
                                }

                                if (item.Entity.GetPropertyValue(DefaultConstants.UserUpdate) == null)
                                {
                                    item.Entity.SetPropertyValue(DefaultConstants.UserUpdate, userInfo.LoginName);
                                }

                                if (item.Entity.GetPropertyValue(DefaultConstants.UserCreate) == null)
                                {
                                    item.Entity.SetPropertyValue(DefaultConstants.UserCreate, userInfo.LoginName);
                                }
                            }
                            else if (item.State == EntityState.Modified)
                            {
                                item.Entity.SetPropertyValue(DefaultConstants.DateUpdate, userInfo.ServerDate);
                                item.Entity.SetPropertyValue(DefaultConstants.UserUpdate, userInfo.LoginName);
                            }

                            if (isFromImport)
                            {
                                //Thực hiện tác vụ sau import
                                ExecuteTaskAfterImport(item);
                            }
                        }
                    }

                    GenerateCode(userID, string.Empty, false, listItem.Where(d => d.State
                        == EntityState.Added).Select(d => d.Entity).ToArray());

                    //Đồng bộ mã chấm công với máy
                    SyncCardCode(listItem.ToArray());

                    OnTracking(userInfo);
                    Context.SaveChanges();
                    result = DataErrorCode.Success;
                }
                catch (Exception e)
                {
                    DataError dataError = new DataError();
                    if (e.Message.Contains("Cannot insert the value NULL into column"))
                    {
                        dataError.DataErrorCode = DataErrorCode.FieldNotAcceptNull;
                        dataError.CollumName = e.Message.Split('\'')[1];
                        dataError.TableName = e.Message.Split('\'')[3];
                        result = DataErrorCode.FieldNotAcceptNull;
                    }
                    else if (e.Message.Contains("FOREIGN KEY constraint"))
                    {
                        dataError.DataErrorCode = DataErrorCode.FieldNotAcceptNull;
                        dataError.CollumName = e.Message.Split('\"').Where(str => str.StartsWith("dbo.")).Single().Substring(4);
                        result = DataErrorCode.FieldNotAcceptNull;
                    }
                    else if (e.Message.Contains("Cannot add an entity with a key that is already in use") || e.Message.Contains("Cannot insert duplicate key"))
                    {
                        dataError.DataErrorCode = DataErrorCode.Dupplicate;
                        result = DataErrorCode.Dupplicate;
                    }
                    else
                    {
                        if (e.Message.Contains("binary data"))
                        {
                            result = DataErrorCode.BINARYDATA;
                        }
                        else
                        {
                            result = DataErrorCode.Unknown;
                            throw;
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region CheckLock

        public bool CheckLock(Type entityType, DateTime? dateStart, DateTime? dateEnd)
        {
            string unlockStatus = LockObjectStatus.E_APPROVED.ToString();

            var listLockObjectItem = CreateQueryable<Sys_LockObjectItem>(d => d.Sys_LockObject.Status == unlockStatus
                && d.ObjectName == entityType.Name && (d.Sys_LockObject.IsDelete == null
                || !d.Sys_LockObject.IsDelete.Value)).Select(d => new LockObjectItem
                {
                    ObjectName = d.ObjectName,
                    DateStart = d.Sys_LockObject.DateStart,
                    DateEnd = d.Sys_LockObject.DateEnd,
                    OrgStructures = d.Sys_LockObject.OrgStructures,
                    PayrollGroups = d.Sys_LockObject.PayrollGroups,
                    WorkPlaceID = d.Sys_LockObject.WorkPlaceID
                }).ToList();

            LockObjectRepository lockObjectRepository = new LockObjectRepository();
            var lockInfo = lockObjectRepository.GetSettings(entityType.GetRealEntityType().Name);
            return CheckLock(null, dateStart, dateEnd, lockInfo, listLockObjectItem);
        }

        public bool CheckLock(params object[] entities)
        {
            bool result = false;

            if (entities != null && entities.Count() > 0)
            {
                string unlockStatus = LockObjectStatus.E_APPROVED.ToString();
                LockObjectRepository lockObjectRepository = new LockObjectRepository();

                var listEntityGroup = entities.Where(d => d != null).GroupBy(d => d.GetType()).ToList();
                var listTypeName = listEntityGroup.Select(d => d.Key.GetRealEntityType().Name).Distinct().ToList();

                var listLockObjectItem = CreateQueryable<Sys_LockObjectItem>().Where(d => d.Sys_LockObject.Status == unlockStatus
                    && listTypeName.Contains(d.ObjectName) && (d.Sys_LockObject.IsDelete == null
                    || !d.Sys_LockObject.IsDelete.Value)).Select(d => new LockObjectItem
                    {
                        ObjectName = d.ObjectName,
                        DateStart = d.Sys_LockObject.DateStart,
                        DateEnd = d.Sys_LockObject.DateEnd,
                        OrgStructures = d.Sys_LockObject.OrgStructures,
                        PayrollGroups = d.Sys_LockObject.PayrollGroups,
                        WorkPlaceID = d.Sys_LockObject.WorkPlaceID
                    }).ToList();

                if (listLockObjectItem != null && listLockObjectItem.Count() > 0)
                {
                    foreach (var entityGroup in listEntityGroup)
                    {
                        string realEntityName = entityGroup.Key.GetRealEntityType().Name;
                        var lockInfo = lockObjectRepository.GetSettings(realEntityName);

                        if (lockInfo != null)
                        {
                            var listLockObjectItemByType = listLockObjectItem.Where(d =>
                                d.ObjectName == realEntityName).ToList();

                            foreach (var entity in entityGroup)
                            {
                                if (entity != null)
                                {
                                    result = CheckLock(entity, lockInfo, listLockObjectItem);

                                    if (!result)
                                    {
                                        var entityEntry = Context.Entry(entity);

                                        if (entityEntry != null && entityEntry.State == EntityState.Modified)
                                        {
                                            //Kiểm tra lock trường hợp dữ liệu bị thay đổi - dữ liệu cũ có thể đã bị khóa
                                            result = CheckLock(entityEntry.OriginalValues.ToObject(), lockInfo, listLockObjectItem);
                                        }
                                    }
                                }

                                if (result)
                                {
                                    break;
                                }
                            }
                        }

                        if (result)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CheckLock(object entity, LockingInfo lockInfo,
            List<LockObjectItem> listLockObjectItem)
        {
            bool result = false;

            if (entity != null)
            {
                DateTime? dateStart = null;
                DateTime? dateEnd = null;

                if (!string.IsNullOrWhiteSpace(lockInfo.FieldName1) && entity.GetPropertyType(lockInfo.FieldName1).IsDateTime())
                {
                    dateStart = (DateTime?)entity.GetPropertyValue(lockInfo.FieldName1);
                }

                if (!string.IsNullOrWhiteSpace(lockInfo.FieldName2) && entity.GetPropertyType(lockInfo.FieldName2).IsDateTime())
                {
                    dateEnd = (DateTime?)entity.GetPropertyValue(lockInfo.FieldName2);
                }

                result = CheckLock(entity, dateStart, dateEnd, lockInfo, listLockObjectItem);
            }

            return result;
        }

        private bool CheckLock(object entity, DateTime? dateStart, DateTime? dateEnd,
            LockingInfo lockInfo, List<LockObjectItem> listLockObjectItem)
        {
            bool result = false;

            foreach (var lockObjectItem in listLockObjectItem)
            {
                if (!string.IsNullOrWhiteSpace(lockInfo.FieldName1) && !string.IsNullOrWhiteSpace(lockInfo.FieldName2))
                {
                    if (dateStart != null && dateEnd != null && Common.IsOverlap(dateStart.Value,
                        dateEnd.Value, lockObjectItem.DateStart, lockObjectItem.DateEnd))
                    {
                        result = true;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(lockInfo.FieldName1))
                {
                    if (dateStart != null && dateStart.Value >= lockObjectItem.DateStart
                        && dateStart.Value <= lockObjectItem.DateEnd)
                    {
                        result = true;
                    }
                }

                //Bị khóa theo điểu kiện thời gian
                if (result && entity != null)
                {
                    Type entityType = entity.GetType();
                    Guid orgStructureID = Guid.Empty;
                    Guid payrollGroupID = Guid.Empty;
                    Guid workPlaceID = Guid.Empty;

                    if (lockObjectItem.OrgStructures != null)
                    {
                        if (entityType.HasProperty("Hre_Profile"))
                        {
                            var profile = (Hre_Profile)entity.GetPropertyValue("Hre_Profile");
                            orgStructureID = profile != null ? profile.OrgStructureID.GetGuid() : Guid.Empty;
                        }
                        else if (entity.HasProperty("OrgStructureID"))
                        {
                            var orgStructureValue = entity.GetPropertyValue("OrgStructureID");
                            Guid.TryParse(orgStructureValue.GetString(), out orgStructureID);
                        }
                        else if (entity.HasProperty("Hre_Profile"))
                        {
                            var profile = (Hre_Profile)entity.GetPropertyValue("Hre_Profile");
                            orgStructureID = profile != null ? profile.OrgStructureID.GetGuid() : Guid.Empty;
                        }

                        //Danh sách phòng ban được áp dụng khóa đối tượng theo lockObject đang xét
                        List<int> listOrgOrderNumber = Common.GetListNumbersFromBinary(lockObjectItem.OrgStructures);

                        if (listOrgOrderNumber != null && listOrgOrderNumber.Count() > 0 && orgStructureID != Guid.Empty)
                        {
                            result = Context.Set<Cat_OrgStructure>().Where(d => listOrgOrderNumber.Contains(d.OrderNumber) && orgStructureID == d.ID).Any();
                        }
                    }

                    if (!result)
                    {
                        if (lockObjectItem.PayrollGroups != null)
                        {
                            if (entityType == typeof(Hre_Profile))
                            {
                                var payrollGroupValue = entity.GetPropertyValue("PayrollGroupID");
                                Guid.TryParse(payrollGroupValue.GetString(), out payrollGroupID);
                            }
                            else if (entity.HasProperty("Hre_Profile"))
                            {
                                var profile = (Hre_Profile)entity.GetPropertyValue("Hre_Profile");
                                payrollGroupID = profile != null ? profile.PayrollGroupID.GetGuid() : Guid.Empty;
                            }

                            List<int> listGroupOrderNumber = Common.GetListNumbersFromBinary(lockObjectItem.PayrollGroups);

                            if (listGroupOrderNumber != null && listGroupOrderNumber.Count() > 0 && payrollGroupID != Guid.Empty)
                            {
                                result = Context.Set<Cat_PayrollGroup>().Where(d => d.OrderNumber.HasValue
                                    && listGroupOrderNumber.Contains(d.OrderNumber.Value) && payrollGroupID == d.ID).Any();
                            }
                        }
                    }

                    if (!result)
                    {
                        if (lockObjectItem.WorkPlaceID.HasValue)
                        {
                            if (entityType == typeof(Hre_Profile))
                            {
                                var payrollGroupValue = entity.GetPropertyValue("WorkPlaceID");
                                Guid.TryParse(payrollGroupValue.GetString(), out workPlaceID);
                            }
                            else if (entity.HasProperty("Hre_Profile"))
                            {
                                var profile = (Hre_Profile)entity.GetPropertyValue("Hre_Profile");
                                workPlaceID = profile != null ? profile.WorkPlaceID.GetGuid() : Guid.Empty;
                            }

                            if (workPlaceID != Guid.Empty)
                            {
                                result = workPlaceID == lockObjectItem.WorkPlaceID;
                            }
                        }
                    }
                }

                if (result)
                {
                    break;
                }
            }

            return result;
        }

        #endregion

        #region GenerateCode

        public void GenerateCode(Guid userID, string fieldName, params object[] entities)
        {
            GenerateCode(userID, new string[] { fieldName }, entities);
        }

        public void GenerateCode(Guid userID, string[] fieldNames, params object[] entities)
        {
            GenerateCode(userID, fieldNames, true, entities);
        }

        public void GenerateCode(Guid userID, string fieldName, bool isOnInitObject, params object[] entities)
        {
            GenerateCode(userID, new string[] { fieldName }, isOnInitObject, entities);
        }

        public void GenerateCode(Guid userID, string[] fieldNames, bool isOnInitObject, params object[] entities)
        {
            ObjectCodeHelper codeGenerator = new ObjectCodeHelper(Context);

            if (fieldNames != null && fieldNames.Any(d => !string.IsNullOrWhiteSpace(d.TrimAll())))
            {
                codeGenerator.Generate(userID.ToString(), CurrentDate(), fieldNames, isOnInitObject, entities);
            }
            else
            {
                codeGenerator.Generate(userID.ToString(), CurrentDate(), isOnInitObject, entities);
            }
        }

        #endregion

        #region UserPermission

        /// <summary>
        /// Kiểm tra người dùng có quyền trên resource hay không?
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="privilegeType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool CheckPermission(Guid userID, PrivilegeType privilegeType, string permission)
        {
            var listUserPermission = GetUserPermission(userID, permission);//chỉ theo permission
            return CheckPermission(userID, privilegeType, permission, listUserPermission);
        }

        /// <summary>
        /// Kiểm tra người dùng có quyền trên resource hay không?
        /// Nên dùng hàm này khi cần kiểm tra nhiều resource cùng lúc.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="privilegeType"></param>
        /// <param name="permission"></param>
        /// <param name="listUserPermission"></param>
        /// <returns></returns>
        public bool CheckPermission(Guid userID, PrivilegeType privilegeType,
            string permission, UserPermission[] listUserPermission)
        {
            bool result = false;

            if (userID != Guid.Empty)
            {
                if (listUserPermission != null && listUserPermission.Count() > 0)
                {
                    result = listUserPermission.Any(d => d.ResourceName == permission
                        && privilegeType.CheckPrivilege(d.PrivilegeNumber.GetInteger()));
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Lấy tất cả quyền của một user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserPermission[] GetUserPermission(Guid userID)
        {
            return GetUserPermission(userID, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin quyền của một user theo permission.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        private UserPermission[] GetUserPermission(Guid userID, string permission)
        {
            UserPermission[] listUserPermission = null;

            if (userID != Guid.Empty)
            {
                //Nguồn gốc phân quyền: Kiểm tra user thuộc những nhóm người dùng, chi nhánh và nhóm dữ liệu nào?
                var groupPermissionQueryable = Context.Set<Sys_GroupPermission2>().Where(d => !d.IsDelete.HasValue || !d.IsDelete.Value);

                var dataQueryable = Context.Set<Sys_DataPermission>().Where(d => d.UserID == userID && d.Sys_Group != null && d.Sys_Group.IsActivate
                    && (!d.IsDelete.HasValue || !d.IsDelete.Value) && (!d.Sys_Group.IsDelete.HasValue || !d.Sys_Group.IsDelete.Value));

                if (!string.IsNullOrWhiteSpace(permission))
                {
                    groupPermissionQueryable = groupPermissionQueryable.Where(d =>
                        d.Sys_Resource != null && d.Sys_Resource.ResourceName == permission);

                    var listPermission = dataQueryable.Join(groupPermissionQueryable,
                        u => u.GroupID, g => g.GroupID, (u, g) => new
                         {
                             u.UserID,
                             u.GroupID,
                             u.DataGroups,
                             u.Branches,
                             g.PrivilegeNumber
                         }).ToArray();

                    listUserPermission = listPermission.Select(d => new UserPermission
                    {
                        UserID = d.UserID,
                        GroupID = d.GroupID,
                        DataGroups = d.DataGroups,
                        Branches = d.Branches,
                        PrivilegeNumber = Convert.ToInt32(d.PrivilegeNumber ?? 0),
                        ResourceName = permission
                    }).ToArray();
                }
                else
                {
                    var listPermission = dataQueryable.Join(groupPermissionQueryable,
                        u => u.GroupID, g => g.GroupID, (u, g) => new
                        {
                            u.UserID,
                            u.GroupID,
                            u.DataGroups,
                            u.Branches,
                            g.PrivilegeNumber,
                            g.Sys_Resource.ResourceName
                        }).ToArray();

                    listUserPermission = listPermission.Select(d => new UserPermission
                    {
                        UserID = d.UserID,
                        GroupID = d.GroupID,
                        DataGroups = d.DataGroups,
                        Branches = d.Branches,
                        PrivilegeNumber = Convert.ToInt32(d.PrivilegeNumber ?? 0),
                        ResourceName = d.ResourceName
                    }).ToArray();
                }
            }

            return listUserPermission;
        }

        #endregion

        #region IsNotSecurityType

        /// <summary>
        /// Những đối tượng không cần kiểm tra phân quyền dữ liệu.
        /// </summary>
        /// <param name="privilegeType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private bool IsNotSecurityType(PrivilegeType privilegeType, Type entityType)
        {
            bool result = false;

            if (privilegeType == PrivilegeType.View)
            {
                if (entityType == typeof(Sys_Resource))
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region GetParentEntityNames

        private List<string> GetParentEntityNames(Type entityType)
        {
            var listParentEntity = entityType.GetProperties().Where(d => d.PropertyType != null
                && IsMetadataType(d.PropertyType)).Select(d => d.PropertyType.Name).Distinct().ToList();

            if (!listParentEntity.Contains(entityType.Name))
            {
                listParentEntity.Add(entityType.Name);
            }

            return listParentEntity;
        }

        #endregion

        #region GetConditionString

        public void GetConditionString(Guid userID, PrivilegeType privilegeType,
            Type entityType, out string conditionString, out object[] parameters)
        {
            conditionString = string.Empty;
            parameters = null;

            if (userID != Guid.Empty && !IsNotSecurityType(privilegeType, entityType))
            {
                var listUserPermission = GetUserPermission(userID, entityType.Name);

                if (listUserPermission != null && CheckPermission(userID, PrivilegeType.View, entityType.Name, listUserPermission))
                {
                    var listDataGroupNumber = listUserPermission.Where(d => d.DataGroupNumbers != null).SelectMany(d => d.DataGroupNumbers).Distinct().ToList();
                    var listBranchNumber = listUserPermission.Where(d => d.BranchNumbers != null).SelectMany(d => d.BranchNumbers).Distinct().ToList();

                    string dataConditionString = string.Empty;
                    string branchConditionString = string.Empty;
                    string masterConditionString = string.Empty;
                    List<object> listParameter = new List<object>();

                    if (listBranchNumber != null && listBranchNumber.Count() > 0)
                    {
                        var listBranchID = Context.Set<Cat_OrgStructure>().Where(d => listBranchNumber.Contains(d.OrderNumber)
                            && (!d.IsDelete.HasValue || !d.IsDelete.Value)).Select(d => d.ID).Distinct().ToArray();

                        string branchTypeName = typeof(Cat_OrgStructure).Name;
                        string branchFieldName = "OrgStructureID";

                        if (entityType.Name == branchTypeName)
                        {
                            branchConditionString = "@" + listParameter.Count()
                                + ".Contains(outerIt." + DefaultConstants.ID + ")";

                            listParameter.Add(listBranchID);
                        }
                        else if (entityType.HasProperty(branchFieldName))
                        {
                            branchConditionString = "@" + listParameter.Count()
                                + ".Contains(outerIt." + branchFieldName + ")";

                            Type propertyType = entityType.GetPropertyType(branchFieldName);
                            if (propertyType != null && propertyType == typeof(Guid?))
                            {
                                listParameter.Add(listBranchID.Select(d => (Guid?)d).ToList());
                            }
                            else
                            {
                                listParameter.Add(listBranchID);
                            }
                        }
                    }

                    if (listDataGroupNumber != null && listDataGroupNumber.Count() > 0)
                    {
                        //Tất cả đối tượng quan hệ khoái ngoại với entity này + chính nó
                        var listParentEntityName = GetParentEntityNames(entityType);

                        var listDataGroupItem = GetListDataGroupItem(entityType,
                            listDataGroupNumber, listParentEntityName);

                        foreach (var dataGroupItem in listDataGroupItem)
                        {
                            if (!string.IsNullOrWhiteSpace(dataGroupItem.FilterValues))
                            {
                                string condition = string.Empty;

                                if (entityType.Name == dataGroupItem.ObjectName)
                                {
                                    condition = GetConditionString(entityType, entityType,
                                        dataGroupItem.ChildFieldLevel1, dataGroupItem, ref listParameter);
                                }
                                else
                                {
                                    condition = GetConditionString(entityType, entityType,
                                        dataGroupItem.ObjectName, dataGroupItem, ref listParameter);
                                }

                                if (!string.IsNullOrWhiteSpace(condition))
                                {
                                    if (string.IsNullOrWhiteSpace(dataConditionString))
                                    {
                                        dataConditionString += "(" + condition + ")";
                                    }
                                    else
                                    {
                                        dataConditionString += " And (" + condition + ")";
                                    }
                                }
                            }
                        }
                    }

                    if (UserMasterDataGroup != null && UserMasterDataGroup.ContainsKey(userID) && UserMasterDataGroup[userID] != null)
                    {
                        var listMasterDataGroupID = UserMasterDataGroup[userID];

                        var listObjectID = CreateQueryable<Cat_MasterDataGroupItem>(d => d.ObjectID.HasValue && d.ObjectName == entityType.Name
                            && d.MasterDataGroupID.HasValue && listMasterDataGroupID.Contains(d.MasterDataGroupID.Value)).Select(d => d.ObjectID.Value).ToList();

                        if (listObjectID != null && listObjectID.Count() > 0)
                        {
                            masterConditionString = "@" + listParameter.Count() + ".Contains(outerIt." + DefaultConstants.ID + ")";
                            listParameter.Add(listObjectID);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(masterConditionString))
                    {
                        if (!string.IsNullOrWhiteSpace(conditionString))
                        {
                            conditionString += " And ";
                        }

                        conditionString = "(" + masterConditionString + ")";
                    }

                    if (!string.IsNullOrWhiteSpace(branchConditionString))
                    {
                        if (!string.IsNullOrWhiteSpace(conditionString))
                        {
                            conditionString += " And ";
                        }

                        conditionString = "(" + branchConditionString + ")";
                    }

                    if (!string.IsNullOrWhiteSpace(dataConditionString))
                    {
                        if (!string.IsNullOrWhiteSpace(conditionString))
                        {
                            conditionString += " And ";
                        }

                        conditionString += dataConditionString;
                    }

                    if (listParameter.Count() > 0)
                    {
                        parameters = listParameter.ToArray();
                    }
                }
                else
                {
                    conditionString = "1 = @0";
                    parameters = new object[] { 0 };
                }
            }
        }

        private string GetConditionString(Type entityType, Type parentFieldType, string fieldName,
            DataGroupItem dataGroupItem, ref List<object> listParameters)
        {
            PropertyInfo propertyInfo = parentFieldType.GetProperty(fieldName);
            string conditionString = string.Empty;

            if (propertyInfo != null)
            {
                string subConditionString = string.Empty;
                if (IsMetadataType(propertyInfo.PropertyType))
                {
                    subConditionString = propertyInfo.Name + " != null And ";
                    PropertyInfo nextStepField = dataGroupItem.GetNextStepField(propertyInfo.PropertyType);

                    if (nextStepField != null && IsMetadataType(nextStepField.PropertyType))
                    {
                        string parentName = string.Empty;
                        if (propertyInfo.Name == dataGroupItem.ChildFieldLevel1 && entityType != null && entityType != parentFieldType)
                        {
                            parentName += parentFieldType.Name + ".";
                        }

                        subConditionString += parentName + propertyInfo.Name + ".";
                    }

                    subConditionString += GetConditionString(entityType, propertyInfo.PropertyType,
                        nextStepField.Name, dataGroupItem, ref listParameters);
                }
                else
                {
                    string fullPathChildName = dataGroupItem.GetFullPathField(entityType);
                    string conditionValue = string.Empty;

                    if (dataGroupItem.FilterValues.Contains("~"))
                    {
                        if (!dataGroupItem.FilterValues.StartsWith("~") && !dataGroupItem.FilterValues.EndsWith("~"))
                        {
                            string[] defaultValues = dataGroupItem.FilterValues.Split('~');
                            object fromValue = defaultValues[0].TryGetValue(propertyInfo.PropertyType);
                            object toValue = defaultValues[1].TryGetValue(propertyInfo.PropertyType);

                            conditionValue = fullPathChildName + " >= @" + listParameters.Count;
                            listParameters.Add(fromValue);//add from parameter

                            conditionValue += " And " + fullPathChildName + " <= @" + listParameters.Count;
                            listParameters.Add(toValue);//add to parameter
                        }
                    }
                    else
                    {
                        string[] defaultValues = dataGroupItem.FilterValues.Split('|', ';', ',');

                        foreach (string defaultValue in defaultValues)
                        {
                            object value = defaultValue.TryGetValue(propertyInfo.PropertyType);

                            if (value == null && propertyInfo.PropertyType.IsNullable())
                            {
                                fullPathChildName = fullPathChildName + ".HasValue";
                                value = false;
                            }

                            string condition = fullPathChildName + " = @" + listParameters.Count;
                            listParameters.Add(value);//add parameter

                            if (string.IsNullOrEmpty(conditionValue))
                            {
                                conditionValue = condition;
                            }
                            else
                            {
                                conditionValue += " Or " + condition;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(conditionValue))
                    {
                        subConditionString = "(" + conditionValue + ")";
                    }
                }

                conditionString += subConditionString;
            }

            return conditionString;
        }

        private List<DataGroupItem> GetListDataGroupItem(Type entityType,
            List<int> listDataGroupNumber, List<string> listParentEntity)
        {
            List<DataGroupItem> result = new List<DataGroupItem>();

            var listDataGroupItem = Context.Set<Cat_DataGroupDetail>().Where(d => listParentEntity.Contains(d.ObjectName)
                && d.Cat_DataGroup != null && listDataGroupNumber.Contains(d.Cat_DataGroup.OrderNumber)
                && (d.IsDelete.HasValue || !d.IsDelete.Value) && (d.Cat_DataGroup.IsDelete.HasValue
                || !d.Cat_DataGroup.IsDelete.Value)).Select(d => new DataGroupItem
                {
                    ID = d.ID,
                    ObjectName = d.ObjectName,
                    ChildFieldLevel1 = d.FieldName,
                    ChildFieldLevel2 = d.ChildFieldName,
                    ChildFieldLevel3 = d.ChildFieldName1,
                    ExcludedObjects = d.Exclusions,
                    FilterValues = d.Value
                }).ToArray();

            foreach (DataGroupItem dataGroupItem in listDataGroupItem)
            {
                if (!string.IsNullOrWhiteSpace(dataGroupItem.ExcludedObjects))
                {
                    //Loại trừ những đối tượng ngoại lệ, không áp dụng quyền này
                    string[] exclusions = dataGroupItem.ExcludedObjects.Split('|', ';', ',');

                    if (exclusions.Contains(entityType.Name))
                    {
                        continue;
                    }
                }

                var item = result.Where(d => d.ObjectName == dataGroupItem.ObjectName && d.ChildFieldLevel1 == dataGroupItem.ChildFieldLevel1
                    && d.ChildFieldLevel2 == dataGroupItem.ChildFieldLevel2 && d.ChildFieldLevel3 == dataGroupItem.ChildFieldLevel3
                    && d.RejectedValues.GetBoolean() == dataGroupItem.RejectedValues.GetBoolean()).FirstOrDefault();

                if (item != null)
                {
                    if (!string.IsNullOrWhiteSpace(dataGroupItem.FilterValues))
                    {
                        if (string.IsNullOrWhiteSpace(item.FilterValues))
                        {
                            item.FilterValues = dataGroupItem.FilterValues;
                        }
                        else
                        {
                            item.FilterValues += "|" + dataGroupItem.FilterValues;
                        }
                    }
                }
                else
                {
                    result.Add(dataGroupItem);
                }
            }

            return result;
        }

        #endregion

        #region CreateTrackingInfo

        internal IEnumerable<ObjectStateEntry> GetObjectStateEntries(ObjectContext objectContext)
        {
            var listObjectStateEntry = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).ToList();
            listObjectStateEntry.AddRange(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified));
            listObjectStateEntry.AddRange(objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted));
            return listObjectStateEntry;
        }

        private void OnTracking(UserInfo userInfo)
        {
            try
            {
                TrackingManager tracking = new TrackingManager();
                tracking.SettingPath = Common.GetPath("Tracking");

                var objectContext = (IObjectContextAdapter)Context;

                var listObjectStateEntry = GetObjectStateEntries(objectContext.ObjectContext);
                var listTrackingInfo = CreateTrackingInfo(userInfo.LoginName, userInfo.ServerDate, listObjectStateEntry.ToArray());
                tracking.WriteTrackings(userInfo.LoginName, userInfo.ServerDate, listTrackingInfo.ToArray());
            }
            catch (Exception)
            {

            }
        }

        public List<TrackingInfo> CreateTrackingInfo(string userUpdate,
            DateTime dateUpdate, params ObjectStateEntry[] objectStateEntries)
        {
            List<TrackingInfo> result = new List<TrackingInfo>();

            if (objectStateEntries != null && objectStateEntries.Count() > 0)
            {
                ItemTrackingManager itemTracking = new ItemTrackingManager();
                itemTracking.SettingPath = Common.GetPath("Settings");
                var listItemTrackingInfo = itemTracking.GetSettings();

                if (listItemTrackingInfo != null && listItemTrackingInfo.Count() > 0)
                {
                    foreach (var objectStateEntry in objectStateEntries)
                    {
                        string entityName = objectStateEntry.EntitySet != null ? objectStateEntry.EntitySet.Name : string.Empty;
                        ItemTrackingInfo itemTrackingInfo = listItemTrackingInfo.Where(d => d.Name == entityName).FirstOrDefault();

                        if (itemTrackingInfo != null)
                        {
                            if (objectStateEntry.State == EntityState.Added && itemTrackingInfo.Create)
                            {
                                result.Add(CreateTrackingInfo(userUpdate, dateUpdate, itemTrackingInfo, objectStateEntry));
                            }
                            else if (objectStateEntry.State == EntityState.Deleted && itemTrackingInfo.Delete)
                            {
                                result.Add(CreateTrackingInfo(userUpdate, dateUpdate, itemTrackingInfo, objectStateEntry));
                            }
                            else if (objectStateEntry.State == EntityState.Modified && itemTrackingInfo.Update)
                            {
                                var listModifiedProperty = objectStateEntry.GetModifiedProperties();

                                foreach (var modifiedProperty in listModifiedProperty)
                                {
                                    var itemExcludedFields = itemTrackingInfo.GetExcludedFields();

                                    if ((itemExcludedFields == null || !itemExcludedFields.Contains(modifiedProperty))
                                        && (ExcludedFields == null || !ExcludedFields.Contains(modifiedProperty)))
                                    {
                                        int originalOrdinal = objectStateEntry.OriginalValues.GetOrdinal(modifiedProperty);
                                        string typeName = objectStateEntry.OriginalValues.GetDataTypeName(originalOrdinal);
                                        Type fieldType = objectStateEntry.OriginalValues.GetFieldType(originalOrdinal);
                                        object originalValue = objectStateEntry.OriginalValues.GetValue(originalOrdinal);
                                        int currentOrdinal = objectStateEntry.CurrentValues.GetOrdinal(modifiedProperty);
                                        object currentValue = objectStateEntry.CurrentValues.GetValue(currentOrdinal);

                                        if (originalValue.HasChanged(currentValue, false))
                                        {
                                            var trackingInfo = CreateTrackingInfo(userUpdate, dateUpdate, itemTrackingInfo, objectStateEntry);
                                            trackingInfo.Reference = GetNavigationObject(entityName, modifiedProperty);//Tên bảng tham chiếu

                                            trackingInfo.CurrentValue = GetValue(currentValue, fieldType);
                                            trackingInfo.OriginalValue = GetValue(originalValue, fieldType);
                                            trackingInfo.FieldName = modifiedProperty;
                                            trackingInfo.FieldType = typeName;
                                            result.Add(trackingInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private TrackingInfo CreateTrackingInfo(string userUpdate, DateTime dateUpdate,
            ItemTrackingInfo itemTrackingInfo, ObjectStateEntry objectStateEntry)
        {
            TrackingInfo trackingInfo = new TrackingInfo();
            trackingInfo.TableName = objectStateEntry.EntitySet.Name;
            trackingInfo.State = objectStateEntry.State.ToString();
            trackingInfo.DateUpdate = dateUpdate;
            trackingInfo.UserUpdate = userUpdate;

            if (objectStateEntry.EntityKey != null && objectStateEntry.EntityKey.EntityKeyValues != null)
            {
                var listID = objectStateEntry.EntityKey.EntityKeyValues.Select(d => d.Value).ToList();
                trackingInfo.ObjectID = listID.ToSplitString();
            }

            if (itemTrackingInfo != null && objectStateEntry.Entity != null)
            {
                if (!string.IsNullOrWhiteSpace(itemTrackingInfo.CodeField))
                {
                    //itemTrackingInfo.CodeField có thể là nhiều cấp như: Invoice.Order.OrderNumber
                    var objectCode = objectStateEntry.Entity.GetPropertyValue(itemTrackingInfo.CodeField);
                    var fieldType = objectStateEntry.Entity.GetPropertyType(itemTrackingInfo.CodeField);
                    trackingInfo.ObjectCode = GetValue(objectCode, fieldType);
                }

                if (!string.IsNullOrWhiteSpace(itemTrackingInfo.NameField))
                {
                    //itemTrackingInfo.NameField có thể là nhiều cấp như: Invoice.Order.OrderNumber
                    var objectName = objectStateEntry.Entity.GetPropertyValue(itemTrackingInfo.NameField);
                    var fieldType = objectStateEntry.Entity.GetPropertyType(itemTrackingInfo.NameField);
                    trackingInfo.ObjectName = GetValue(objectName, fieldType);
                }

                if (!string.IsNullOrWhiteSpace(itemTrackingInfo.InfoField))
                {
                    //itemTrackingInfo.InfoField có thể là nhiều cấp như: Invoice.Order.OrderNumber
                    var moreInfo = objectStateEntry.Entity.GetPropertyValue(itemTrackingInfo.InfoField);
                    var fieldType = objectStateEntry.Entity.GetPropertyType(itemTrackingInfo.InfoField);
                    trackingInfo.MoreInfo = GetValue(moreInfo, fieldType);
                }
            }

            return trackingInfo;
        }

        private string GetValue(object value, Type fieldType)
        {
            string result = string.Empty;

            if (fieldType.IsDateTime())
            {
                if (!value.IsNullOrEmpty())
                {
                    bool invalidFormat = false;
                    var date = value.TryGetValue<DateTime>(out invalidFormat);

                    if (!invalidFormat)
                    {
                        string dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
                        result = date.ToString(dateTimeFormat);
                    }
                }
            }
            else
            {
                result = value.GetString();
            }

            return result;
        }

        #endregion

        #region SetOrgStructureID

        public Dictionary<string, string> GetOrgStructureTableMapping()
        {
            Dictionary<string, string> listTable = new Dictionary<string, string>();
            listTable.Add(typeof(Att_Roster).Name, "DateStart");
            listTable.Add(typeof(Att_Workday).Name, "WorkDate");
            listTable.Add(typeof(Att_LeaveDay).Name, "DateStart");
            listTable.Add(typeof(Att_Overtime).Name, "WorkDateRoot");
            listTable.Add(typeof(Att_AnnualLeaveDetail).Name, "MonthStart");
            listTable.Add(typeof(Att_AttendanceTable).Name, "MonthYear");
            listTable.Add(typeof(Att_TAMScanLog).Name, "TimeLog");
            listTable.Add(typeof(Can_BackPay).Name, "MonthYear");
            listTable.Add(typeof(Can_MealAllowanceToMoney).Name, "DateFrom");
            listTable.Add(typeof(Can_MealRecord).Name, "WorkDay");
            listTable.Add(typeof(Can_MealRecordMissing).Name, "WorkDate");
            listTable.Add(typeof(Can_TamScanLogCMS).Name, "TimeLog");
            listTable.Add(typeof(LMS_TamScanLogLMS).Name, "TimeLog");
            listTable.Add(typeof(LMS_LaundryRecord).Name, "WorkDay");
            return listTable;
        }

        public bool SetCorrectOrgStructureID(IList listEntity)
        {
            bool result = false;

            if (listEntity != null && listEntity.Count > 0)
            {
                Type entityType = listEntity.GetElementType();
                Dictionary<string, string> listTable = GetOrgStructureTableMapping();
                var tableInfo = listTable.Where(d => d.Key == entityType.Name).FirstOrDefault();

                if (IsMetadataField(entityType, "ProfileID"))
                {
                    if (!string.IsNullOrWhiteSpace(tableInfo.Value))
                    {
                        var listWorkHistory = new List<WorkHistoryInfo>();

                        var listProfileID = listEntity.AsQueryable().Select("ProfileID").GetList();

                        foreach (var item in VnResource.Helper.Linq.LinqExtensions.Chunk(listProfileID, 1000))
                        {
                            List<Guid> listSelectedID = item.OfType<Guid>().ToList();

                            if (VnResource.Helper.Data.DataHelper.GetRealPropertyType(entityType, "ProfileID") == typeof(Guid?))
                            {
                                listSelectedID = item.OfType<Guid?>().Select(d => d.Value).ToList();
                            }

                            listWorkHistory.AddRange(CreateQueryable<Hre_WorkHistory>(Guid.Empty,
                                d => d.OrganizationStructureID.HasValue && listSelectedID.Contains(d.ProfileID)).Select(d =>
                                    new WorkHistoryInfo
                                    {
                                        ProfileID = d.ProfileID,
                                        OrganizationStructureID = d.OrganizationStructureID,
                                        DateEffective = d.DateEffective
                                    }).ToList());
                        }

                        result = SetCorrectOrgStructureID(tableInfo.Value, listWorkHistory, listEntity);
                    }
                }
            }

            return result;
        }

        public bool SetCorrectOrgStructureID(List<WorkHistoryInfo> listWorkHistory, IList listEntity)
        {
            bool result = false;

            if (listEntity != null && listEntity.Count > 0)
            {
                Type entityType = listEntity.GetElementType();
                Dictionary<string, string> listTable = GetOrgStructureTableMapping();
                var tableInfo = listTable.Where(d => d.Key == entityType.Name).FirstOrDefault();
                result = SetCorrectOrgStructureID(tableInfo.Value, listWorkHistory, listEntity);
            }

            return result;
        }

        public bool SetCorrectOrgStructureID(string checkField, List<WorkHistoryInfo> listWorkHistory, IList listEntity)
        {
            bool result = false;

            if (listEntity != null && listEntity.Count > 0)
            {
                Type entityType = listEntity.GetElementType();

                if (IsMetadataField(entityType, "OrgStructureID")
                    && IsMetadataField(entityType, "ProfileID")
                    && IsMetadataField(entityType, checkField))
                {
                    foreach (var entity in listEntity)
                    {
                        var dateObject = entity.GetPropertyValue(checkField);
                        var profileID = VnResource.Helper.Data.DataHelper.TryGetValue<Guid>(entity.GetPropertyValue("ProfileID"));
                        var orgStructureID = VnResource.Helper.Data.DataHelper.TryGetValue<Guid>(entity.GetPropertyValue("OrgStructureID"));

                        if (dateObject != null)
                        {
                            DateTime date = VnResource.Helper.Data.DataHelper.TryGetValue<DateTime>(dateObject);
                            var workHistory = listWorkHistory.Where(d => d.ProfileID == profileID && d.DateEffective.Date <= date).OrderByDescending(d => d.DateEffective).FirstOrDefault();

                            if (workHistory == null)
                            {
                                if (checkField == "DateStart" || checkField == "DateFrom")
                                {
                                    checkField = checkField == "DateStart" ? "DateEnd" : "DateTo";
                                    date = VnResource.Helper.Data.DataHelper.TryGetValue<DateTime>(entity.GetPropertyValue(checkField));
                                }
                                else if (checkField == "MonthYear" || checkField == "MonthStart")
                                {
                                    date = date.Date.AddMonths(1).AddSeconds(-1);
                                }

                                workHistory = listWorkHistory.Where(d => d.ProfileID == profileID && d.DateEffective.Date <= date).OrderByDescending(d => d.DateEffective).FirstOrDefault();
                            }

                            if (workHistory != null && orgStructureID != workHistory.OrganizationStructureID.Value)
                            {
                                entity.SetPropertyValue("OrgStructureID", workHistory.OrganizationStructureID.Value);
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region CreateQueryable

        public List<TResult> GetData<TEntity, TResult>(PageDataInfo pageInfo, Expression<Func<TEntity,
            TResult>> selector, params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            List<TResult> result = null;

            string orderingString = string.Empty;
            int totalRowCount = pageInfo.TotalRowCount;
            string stringPredicate = string.Empty;
            object[] parameters = null;

            if (pageInfo != null)
            {
                if (pageInfo.OrderFields != null)
                {
                    orderingString = pageInfo.OrderFields.Select(d =>
                        d.FieldName).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(orderingString)
                        && typeof(TEntity).HasProperty(orderingString))
                    {
                        orderingString += " " + pageInfo.OrderFields.Select(d =>
                            d.Direction.ToString()).FirstOrDefault();
                    }
                }

                if (string.IsNullOrWhiteSpace(orderingString))
                {
                    orderingString = "DateUpdate DESC";
                }

                if (pageInfo.FilterFields != null && pageInfo.FilterFields.Count() > 0)
                {
                    pageInfo.FilterFields.ToList().BuildPredicate(typeof(TEntity),
                        out stringPredicate, out parameters);
                }
            }

            Guid userID = pageInfo != null ? pageInfo.UserID : Guid.Empty;
            var queryable = CreateQueryable(userID, predicates);

            if (!string.IsNullOrWhiteSpace(stringPredicate))
            {
                //Gán chuỗi điều kiện phân quyền dữ liệu vào câu lệnh truy vấn
                queryable = queryable.Where(stringPredicate, parameters);
            }

            queryable = queryable.Paging(orderingString, pageInfo.PageIndex,
                pageInfo.PageSize, ref totalRowCount);

            pageInfo.TotalRowCount = totalRowCount;
            result = queryable.Select(selector).ToList();

            return result;
        }

        public List<TEntity> GetData<TEntity>(PageDataInfo pageInfo,
            params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            List<TEntity> result = null;

            string orderingString = string.Empty;
            int totalRowCount = pageInfo.TotalRowCount;
            string stringPredicate = string.Empty;
            object[] parameters = null;

            if (pageInfo != null)
            {
                if (pageInfo.OrderFields != null)
                {
                    orderingString = pageInfo.OrderFields.Select(d =>
                        d.FieldName).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(orderingString)
                        && typeof(TEntity).HasProperty(orderingString))
                    {
                        orderingString += " " + pageInfo.OrderFields.Select(d =>
                            d.Direction.ToString()).FirstOrDefault();
                    }
                }

                if (string.IsNullOrWhiteSpace(orderingString))
                {
                    orderingString = "DateUpdate DESC";
                }

                if (pageInfo.FilterFields != null && pageInfo.FilterFields.Count() > 0)
                {
                    pageInfo.FilterFields.ToList().BuildPredicate(typeof(TEntity),
                        out stringPredicate, out parameters);
                }
            }

            Guid userID = pageInfo != null ? pageInfo.UserID : Guid.Empty;
            var queryable = CreateQueryable(userID, predicates);

            if (!string.IsNullOrWhiteSpace(stringPredicate))
            {
                //Gán chuỗi điều kiện phân quyền dữ liệu vào câu lệnh truy vấn
                queryable = queryable.Where(stringPredicate, parameters);
            }

            queryable = queryable.Paging(orderingString, pageInfo.PageIndex,
                pageInfo.PageSize, ref totalRowCount);

            pageInfo.TotalRowCount = totalRowCount;
            result = queryable.ToList();

            return result;
        }

        public IQueryable<TEntity> CreateQueryable<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            return CreateQueryable<TEntity>(Guid.Empty, predicates);
        }

        public IQueryable<TEntity> CreateQueryable<TEntity>(Guid userID,
            params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            IQueryable<TEntity> queryable = null;

            queryable = Context.Set<TEntity>().AsQueryable();

            if (predicates != null && predicates.Count() > 0)
            {
                foreach (var predicate in predicates)
                {
                    queryable = queryable.Where(predicate);
                }
            }

            object[] conditionParameters = null;
            string conditionString = string.Empty;

            GetConditionString(userID, PrivilegeType.View,
                typeof(TEntity), out conditionString, out conditionParameters);

            if (!string.IsNullOrWhiteSpace(conditionString))
            {
                queryable = queryable.Where(conditionString, conditionParameters);
            }

            queryable = queryable.Where("IsDelete = null Or IsDelete = false");

            return queryable;
        }

        public IQueryable CreateQueryable(Type entityType,
            string predicate, params object[] parameters)
        {
            return CreateQueryable(Guid.Empty, entityType, predicate, parameters);
        }

        public IQueryable CreateQueryable(Guid userID, Type entityType,
            string predicate, params object[] parameters)
        {
            IQueryable queryable = null;

            queryable = Context.Set(entityType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(predicate))
            {
                queryable = queryable.Where(predicate, parameters);
            }

            object[] conditionParameters = null;
            string conditionString = string.Empty;

            GetConditionString(userID, PrivilegeType.View,
                entityType, out conditionString, out conditionParameters);

            if (!string.IsNullOrWhiteSpace(conditionString))
            {
                queryable = queryable.Where(conditionString, conditionParameters);
            }

            queryable = queryable.Where("IsDelete = null Or IsDelete = false");

            return queryable;
        }

        #endregion

        #region AddAndRemoveObject

        public void AddObject(params object[] entities)
        {
            AddObject(null, entities);
        }

        public void AddObject(Type entityType, params object[] entities)
        {
            var autoDetectChangesEnabled = Context.Configuration.AutoDetectChangesEnabled;
            Context.Configuration.AutoDetectChangesEnabled = false;//Tắt validate các đối tượng

            foreach (var item in entities)
            {
                var entry = Context.Entry(item);

                if (entityType == null)
                {
                    entityType = item.GetType().GetRealEntityType();
                }

                if (entry == null || entry.State == System.Data.Entity.EntityState.Detached)
                {
                    Context.Set(entityType).Add(item);
                }
            }

            //Bật validate các đối tượng cho làm việc bình thường
            Context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public void RemoveObject(params object[] entities)
        {
            RemoveObject(null, entities);
        }

        public void RemoveObject(Type entityType, params object[] entities)
        {
            var autoDetectChangesEnabled = Context.Configuration.AutoDetectChangesEnabled;
            Context.Configuration.AutoDetectChangesEnabled = false;//Tắt validate các đối tượng

            foreach (var item in entities)
            {
                var entry = Context.Entry(item);

                if (entityType == null)
                {
                    entityType = item.GetType().GetRealEntityType();
                }

                if (entry != null && entry.State != System.Data.Entity.EntityState.Deleted
                    && entry.State != System.Data.Entity.EntityState.Detached)
                {
                    Context.Set(entityType).Remove(item);
                }
            }

            //Bật validate các đối tượng cho làm việc bình thường
            Context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        #endregion

        #region IsDeleteByQuery

        public int SetIsDelete(IQueryable queryable)
        {
            return VnResource.Data.Entity.EntityExtensions.SetIsDelete(queryable);
        }

        public int SetIsDelete<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
        {
            return VnResource.Data.Entity.EntityExtensions.SetIsDelete<TEntity>(queryable);
        }

        #endregion

        #region DeleteByQuery

        public int Delete(IQueryable queryable)
        {
            return VnResource.Data.Entity.EntityExtensions.Delete(queryable);
        }

        public int Delete<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
        {
            return VnResource.Data.Entity.EntityExtensions.Delete<TEntity>(queryable);
        }

        #endregion

        #region Metadata

        private List<EntityType> listEntityType;

        public List<EntityType> ListEntityType
        {
            get
            {
                if (listEntityType == null)
                {
                    var objectContext = (IObjectContextAdapter)Context;

                    listEntityType = objectContext.ObjectContext.MetadataWorkspace.GetItems(DataSpace.CSpace).Where(d =>
                        d.BuiltInTypeKind == BuiltInTypeKind.EntityType).Select(d => (EntityType)d).ToList();
                }

                return listEntityType;
            }
        }

        public List<DynamicEntity> GetChildFields(string objectName)
        {
            List<DynamicEntity> listProperty = new List<DynamicEntity>();

            if (!string.IsNullOrWhiteSpace(objectName))
            {
                var listEntityType = ListEntityType.Where(d => d.Name == objectName).ToList();
                listProperty = listEntityType.SelectMany(d => d.Properties.Select(p =>
                    new DynamicEntity { Name = p.Name })).ToList();

                listProperty.AddRange(listEntityType.SelectMany(d => d.NavigationProperties.Where(p =>
                    p.ToEndMember.RelationshipMultiplicity != RelationshipMultiplicity.Many).Select(p =>
                        new DynamicEntity { Name = p.Name })).OrderBy(d => d.Name).ToList());
            }

            return listProperty;
        }

        public List<String> GetKeyFieldNames(Type entityType)
        {
            return ListEntityType.Where(d => entityType != null && d.Name == entityType.Name).SelectMany(d => d.KeyMembers).Select(d => d.Name).ToList();
        }

        /// <summary>
        /// Kiểm tra độ dài ký tự tối đa của một field trong databse?
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int GetMaxLength(Type entityType, string fieldName)
        {
            Facet facetInfo = ListEntityType.Where(d => entityType != null && d.Name == entityType.Name).SelectMany(d => d.Properties.Where(p =>
                p.Name == fieldName && p.TypeUsage != null)).SelectMany(d => d.TypeUsage.Facets.Where(f => f.Name == "MaxLength")).FirstOrDefault();

            int maxlength = -1;
            if (facetInfo != null && facetInfo.Value != null)
            {
                int.TryParse(facetInfo.Value.ToString(), out maxlength);
            }
            return maxlength;
        }

        /// <summary>
        /// Kiểm tra một field có phải thuộc cấu trúc table trong database?
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool IsMetadataField(Type entityType, string fieldName)
        {
            return ListEntityType.Where(d => entityType != null && d.Name != null && d.Name == entityType.Name).Any(d =>
                d.Properties.Any(p => p.Name == fieldName) || d.NavigationProperties.Any(p => p.Name == fieldName));
        }

        /// <summary>
        /// Kiểm tra một type có phải là table trong database?
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool IsMetadataType(Type entityType)
        {
            return ListEntityType.Any(d => entityType != null &&
                d.Name != null && d.Name == entityType.Name);
        }

        /// <summary>
        /// Lấy cột ID khóa ngoại đã tạo ra objectField trong entityType.
        /// Ví dụ: entityType = InvoiceDetail, objectField = Invoice => InvoiceID
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="objectField"></param>
        /// <returns></returns>
        public string GetFieldConstraint(Type entityType, string objectField)
        {
            string result = string.Empty;

            NavigationProperty property = ListEntityType.Where(d => entityType != null && d.Name == entityType.Name).Select(d =>
                d.NavigationProperties.Where(p => p.Name == objectField).FirstOrDefault()).FirstOrDefault();

            if (property != null && property.RelationshipType != null)
            {
                AssociationType association = (AssociationType)property.RelationshipType;

                if (association != null)
                {
                    result = association.ReferentialConstraints.SelectMany(d =>
                        d.ToProperties).Select(d => d.Name).FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy field đối tượng được sinh ra từ 1 khóa ngoại trong entityType.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="constraintField"></param>
        /// <returns></returns>
        public string GetFieldNavigation(Type entityType, string constraintField)
        {
            string result = string.Empty;

            List<NavigationProperty> listProperty = ListEntityType.Where(d => entityType != null
                && d.Name == entityType.Name).SelectMany(d => d.NavigationProperties).ToList();

            foreach (NavigationProperty property in listProperty)
            {
                if (property != null && property.RelationshipType != null)
                {
                    AssociationType association = (AssociationType)property.RelationshipType;

                    if (association != null && association.ReferentialConstraints.Any(d =>
                        d.ToProperties.Any(p => p.Name == constraintField)))
                    {
                        result = property.Name;
                        break;
                    }
                }
            }

            return result;
        }

        public string GetNavigationObject(Type entityType, string constraintField)
        {
            string result = string.Empty;

            if (entityType != null)
            {
                result = GetNavigationObject(entityType.Name, constraintField);
            }

            return result;
        }

        public string GetNavigationObject(string entityName, string constraintField)
        {
            var property = GetNavigationProperty(entityName, constraintField);
            return property != null && property.ToEndMember != null ? property.ToEndMember.Name : string.Empty;
        }

        /// <summary>
        /// Lấy tên bảng khóa ngoại trong entityType theo constraintField.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="constraintField">Tên cột khóa ngoại</param>
        /// <returns></returns>
        private NavigationProperty GetNavigationProperty(string entityName, string constraintField)
        {
            NavigationProperty result = null;

            List<NavigationProperty> listProperty = ListEntityType.Where(d =>
                d.Name == entityName).SelectMany(d => d.NavigationProperties).ToList();

            foreach (NavigationProperty property in listProperty)
            {
                if (property != null && property.RelationshipType != null)
                {
                    AssociationType association = (AssociationType)property.RelationshipType;

                    if (association != null && association.ReferentialConstraints.Any(d =>
                        d.ToProperties.Any(p => p.Name == constraintField)))
                    {
                        result = property;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Liệt kê danh sách field không được bỏ trống.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public string[] GetNotNullFields(Type entityType)
        {
            return ListEntityType.Where(d => entityType != null && d.Name == entityType.Name).SelectMany(d =>
                d.Properties.Where(p => !p.Nullable)).Select(d => d.Name).Distinct().ToArray();
        }

        public Type GetEntityType(string entityName)
        {
            return entityName.GetEntityType(typeof(Sys_UserInfo).Assembly);
        }

        #endregion

        #region SyncData

        private void SyncCardCode(params DbEntityEntry[] listEntityEntry)
        {
            SyncCardCodeAds(listEntityEntry);
            SyncCardCodeTrio(listEntityEntry);
        }

        /// <summary>
        /// Hàm này chỉ hỗ trợ cho database ADS của Tinh Hoa
        /// </summary>
        /// <param name="listEntityEntry"></param>
        private void SyncCardCodeAds(params DbEntityEntry[] listEntityEntry)
        {
            try
            {
                var listProfileEntry = listEntityEntry.Where(d => d.Entity != null
                    && d.Entity.GetType().GetRealEntityType() == typeof(Hre_Profile)).ToArray();

                if (listProfileEntry != null && listProfileEntry.Count() > 0)
                {
                    string settingkey = AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString();
                    var listSetting = CreateQueryable<Sys_AllSetting>(Guid.Empty, d => d.Name.Contains(settingkey)).ToArray();
                    string serverName = listSetting.Where(m => m.Name == settingkey + AppConfig.SERVERNAME).Select(m => m.Value1).FirstOrDefault();
                    string userName = listSetting.Where(m => m.Name == settingkey + AppConfig.USERID).Select(m => m.Value1).FirstOrDefault();
                    string password = listSetting.Where(m => m.Name == settingkey + AppConfig.PASSWORD).Select(m => m.Value1).FirstOrDefault();
                    string dbName = listSetting.Where(m => m.Name == settingkey + AppConfig.DBNAME).Select(m => m.Value1).FirstOrDefault();
                    string isActivated = listSetting.Where(m => m.Name == settingkey + AppConfig.ISACTIVATED).Select(m => m.Value1).FirstOrDefault();
                    string allowSyncCard = listSetting.Where(m => m.Name == settingkey + AppConfig.SYNC_CARD).Select(m => m.Value1).FirstOrDefault();
                    string delayDays = listSetting.Where(m => m.Name == settingkey + AppConfig.DELAY_DAYS).Select(m => m.Value1).FirstOrDefault();

                    string connectionString = DbClientHelper.BuildSqlConnectionString(serverName, dbName, userName, password);
                    var deleteCardDelayDays = VnResource.Helper.Data.DataHelper.TryGetValue<int>(delayDays);
                    deleteCardDelayDays = deleteCardDelayDays < 0 ? 0 : deleteCardDelayDays;
                    DateTime currentDate = CurrentDate();

                    if (isActivated == bool.TrueString && allowSyncCard == bool.TrueString)
                    {
                        //Cấu trúc theo tài liệu của Tinh Hoa
                        string tableName = "IC_EmployeeWorkingInfo";
                        var dtCardCode = new System.Data.DataTable(tableName);
                        dtCardCode.Columns.Add("CardNumber");
                        dtCardCode.Columns.Add("UserID");
                        dtCardCode.Columns.Add("NameOnDevice");
                        dtCardCode.Columns.Add("Status");
                        dtCardCode.Columns.Add("Synched");
                        dtCardCode.Columns.Add("Privilege");
                        dtCardCode.Columns.Add("PassOnDevice");

                        foreach (var itemEntry in listProfileEntry)
                        {
                            try
                            {
                                var profileCode = itemEntry.Property("CodeEmp").CurrentValue.GetString().Trim();
                                var cardCode = itemEntry.Property("CodeAttendance").CurrentValue.GetString().Trim();
                                int cardStatus = -1;

                                if (itemEntry.State == System.Data.Entity.EntityState.Modified)
                                {
                                    if (itemEntry.Property("IsDelete").IsModified)
                                    {
                                        var isDelete = itemEntry.Property("IsDelete").CurrentValue;
                                        if (VnResource.Helper.Data.DataHelper.TryGetValue<bool>(isDelete))
                                        {
                                            if (deleteCardDelayDays <= 0)
                                            {
                                                cardStatus = 0;//delete 1 card trong Db Tinh Hoa
                                                dtCardCode.Rows.Add(cardCode, cardCode, profileCode, cardStatus, 0, 0, string.Empty);
                                            }
                                        }
                                    }
                                    else if (itemEntry.Property("DateQuit").IsModified)
                                    {
                                        var originalValue = itemEntry.Property("DateQuit").OriginalValue;
                                        var currentValue = itemEntry.Property("DateQuit").CurrentValue;

                                        if (currentValue.HasChanged(originalValue))
                                        {
                                            if (currentValue != null)
                                            {
                                                //Nếu DateQuit > ngày hiện tại thì sẽ chờ đến ngày và service sẽ chạy tự động
                                                if (VnResource.Helper.Data.DataHelper.TryGetValue<DateTime>(currentValue) <= currentDate)
                                                {
                                                    if (deleteCardDelayDays <= 0)
                                                    {
                                                        cardStatus = 0;//delete 1 card trong Db Tinh Hoa
                                                        dtCardCode.Rows.Add(cardCode, cardCode, profileCode, cardStatus, 0, 0, string.Empty);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                cardStatus = 1;//Nhận lại 1 card vào Db Tinh Hoa
                                                dtCardCode.Rows.Add(cardCode, cardCode, profileCode, cardStatus, 0, 0, string.Empty);
                                            }
                                        }
                                    }
                                    else if (itemEntry.Property("CodeAttendance").IsModified)
                                    {
                                        var originalValue = itemEntry.Property("CodeAttendance").OriginalValue.GetString().Trim();

                                        if (cardCode.HasChanged(originalValue))
                                        {
                                            if (deleteCardDelayDays <= 0)
                                            {
                                                cardStatus = 0;//delete 1 card trong Db Tinh Hoa
                                                dtCardCode.Rows.Add(originalValue, originalValue, profileCode, cardStatus, 0, 0, string.Empty);
                                            }

                                            cardStatus = 1;//tạo mới 1 card trong Db Tinh Hoa
                                            dtCardCode.Rows.Add(cardCode, cardCode, profileCode, cardStatus, 0, 0, string.Empty);
                                        }
                                    }
                                }
                                else if (itemEntry.State == System.Data.Entity.EntityState.Added)
                                {
                                    if (!string.IsNullOrWhiteSpace(cardCode))
                                    {
                                        cardStatus = 1;//tạo mới 1 card trong Db Tinh Hoa
                                        dtCardCode.Rows.Add(cardCode, cardCode, profileCode, cardStatus, 0, 0, string.Empty);
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        if (dtCardCode != null && dtCardCode.Rows.Count > 0)
                        {
                            SqlConnection connection = new SqlConnection(connectionString);

                            using (DbCommander commander = new DbCommander(connection))
                            {
                                commander.InsertTable(dtCardCode);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Hàm này chỉ hỗ trợ cho database Trio của HVN
        /// </summary>
        /// <param name="listEntityEntry"></param>
        private void SyncCardCodeTrio(params DbEntityEntry[] listEntityEntry)
        {
            try
            {
                var listProfileEntry = listEntityEntry.Where(d => d.Entity != null
                    && d.Entity.GetType().GetRealEntityType() == typeof(Hre_Profile)).ToArray();

                if (listProfileEntry != null && listProfileEntry.Count() > 0)
                {
                    string settingkey = AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString();
                    var listSetting = CreateQueryable<Sys_AllSetting>(Guid.Empty, d => d.Name.Contains(settingkey)).ToArray();
                    string serverName = listSetting.Where(m => m.Name == settingkey + AppConfig.SERVERNAME).Select(m => m.Value1).FirstOrDefault();
                    string userName = listSetting.Where(m => m.Name == settingkey + AppConfig.USERID).Select(m => m.Value1).FirstOrDefault();
                    string password = listSetting.Where(m => m.Name == settingkey + AppConfig.PASSWORD).Select(m => m.Value1).FirstOrDefault();
                    string dbName = listSetting.Where(m => m.Name == settingkey + AppConfig.DBNAME).Select(m => m.Value1).FirstOrDefault();
                    string isActivated = listSetting.Where(m => m.Name == settingkey + AppConfig.ISACTIVATED).Select(m => m.Value1).FirstOrDefault();
                    string allowSyncCard = listSetting.Where(m => m.Name == settingkey + AppConfig.SYNC_CARD).Select(m => m.Value1).FirstOrDefault();
                    string delayDays = listSetting.Where(m => m.Name == settingkey + AppConfig.DELAY_DAYS).Select(m => m.Value1).FirstOrDefault();

                    string connectionString = DbClientHelper.BuildSqlConnectionString(serverName, dbName, userName, password);
                    var deleteCardDelayDays = VnResource.Helper.Data.DataHelper.TryGetValue<int>(delayDays);
                    deleteCardDelayDays = deleteCardDelayDays < 0 ? 0 : deleteCardDelayDays;
                    DateTime currentDate = CurrentDate();

                    if (isActivated == bool.TrueString && allowSyncCard == bool.TrueString)
                    {
                        //Cấu trúc theo tài liệu của Trio
                        var dtCardCode = new System.Data.DataTable("tblEmployee");
                        dtCardCode.Columns.Add("CardNo");
                        dtCardCode.Columns.Add("EmployeeID");
                        dtCardCode.Columns.Add("FullName");
                        dtCardCode.Columns.Add("DivisionID");
                        dtCardCode.Columns.Add("DepartmentID");
                        dtCardCode.Columns.Add("SectionID");
                        dtCardCode.Columns.Add("GroupID");
                        dtCardCode.Columns.Add("PositionID");
                        dtCardCode.Columns.Add("Female", typeof(bool));
                        dtCardCode.Columns.Add("EmployeeStatusID", typeof(int));
                        dtCardCode.Columns.Add("Matched");
                        dtCardCode.Columns.Add("HireDate", typeof(DateTime));
                        dtCardCode.Columns.Add("TerminateDate", typeof(DateTime));

                        var dtCardCodeHistory = new System.Data.DataTable("tblCard");
                        dtCardCodeHistory.Columns.Add("CardID");
                        dtCardCodeHistory.Columns.Add("Employee");
                        dtCardCodeHistory.Columns.Add("FullName");

                        foreach (var itemEntry in listProfileEntry)
                        {
                            try
                            {
                                var gender = itemEntry.Property("Gender").CurrentValue;
                                var dateHire = itemEntry.Property("HateHire").CurrentValue;
                                var dateQuit = itemEntry.Property("DateQuit").CurrentValue;
                                var profileCode = itemEntry.Property("CodeEmp").CurrentValue.GetString().Trim();
                                var profileName = itemEntry.Property("ProfileName").CurrentValue.GetString().Trim();
                                var cardCode = itemEntry.Property("CodeAttendance").CurrentValue.GetString().Trim();
                                int isFemale = gender.GetString() == EnumDropDown.Sexual.E_FEMALE.ToString() ? 1 : 0;

                                if (itemEntry.State == System.Data.Entity.EntityState.Modified)
                                {
                                    if (itemEntry.Property("IsDelete").IsModified
                                        || itemEntry.Property("DateQuit").IsModified)
                                    {
                                        var isDelete = itemEntry.Property("IsDelete").CurrentValue;
                                        DateTime? dateTerminate = null;//VinhTran - 20150323 - 0042445
                                        string terminateCardCode = string.Empty;
                                        bool isDelay = false;

                                        if (VnResource.Helper.Data.DataHelper.TryGetValue<bool>(isDelete))
                                        {
                                            dateTerminate = DateTime.Now;
                                        }
                                        else if (dateQuit != null)
                                        {
                                            dateTerminate = VnResource.Helper.Data.DataHelper.TryGetValue<DateTime>(dateQuit);

                                            //Nếu DateQuit > ngày hiện tại thì sẽ chờ đến ngày và service sẽ chạy tự động
                                            if (dateTerminate > currentDate && deleteCardDelayDays > 0)
                                            {
                                                isDelay = true;
                                            }
                                        }
                                        else
                                        {
                                            terminateCardCode = cardCode.GetString();
                                        }

                                        if (!isDelay)
                                        {
                                            UpdateTerminateDate(connectionString, profileCode, dateTerminate);
                                            UpdateCardHistory(connectionString, profileCode, terminateCardCode);
                                        }
                                    }
                                    else if (itemEntry.Property("CodeAttendance").IsModified)
                                    {
                                        var originalValue = itemEntry.Property("CodeAttendance").OriginalValue.GetString().Trim();

                                        if (cardCode.HasChanged(originalValue))
                                        {
                                            //VinhTran - Khi update thì set Matched để bật cờ chờ đồng bộ với máy chấm công
                                            DateTime hireDate = dateHire != null ? VnResource.Helper.Data.DataHelper.TryGetValue<DateTime>(dateHire) : DateTime.MinValue;
                                            UpdateEmployee(connectionString, profileCode, cardCode, hireDate);
                                            UpdateCardHistory(connectionString, profileCode, cardCode);
                                        }
                                    }
                                }
                                else if (itemEntry.State == System.Data.Entity.EntityState.Added)
                                {
                                    if (!string.IsNullOrWhiteSpace(profileCode))
                                    {
                                        object matched = DBNull.Value;

                                        if (!string.IsNullOrWhiteSpace(cardCode.GetString()))
                                        {
                                            matched = 0;
                                        }

                                        dtCardCode.Rows.Add(cardCode, profileCode, profileName, 0, 0, 0, 0, 0, isFemale, 0, matched, dateHire ?? DBNull.Value, dateQuit ?? DBNull.Value);
                                        dtCardCodeHistory.Rows.Add(cardCode, profileCode, profileName);
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        if (dtCardCode != null && dtCardCode.Rows.Count > 0)
                        {
                            SqlConnection connection = new SqlConnection(connectionString);

                            using (DbCommander commander = new DbCommander(connection))
                            {
                                commander.InsertTable(dtCardCode);
                                commander.InsertTable(dtCardCodeHistory);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private int UpdateEmployee(string connectionString,
            string profileCode, string cardCode, DateTime dateHire)
        {
            string tableName = "tblEmployee";
            string profileField = "EmployeeID";
            string cardCodeField = "CardNo";

            string commandText = "Update " + tableName + " Set Matched = 0";

            if (!string.IsNullOrWhiteSpace(cardCodeField))
            {
                commandText += ", " + cardCodeField + " = '" + cardCode + "'";
            }

            commandText += " Where " + profileField + " = '" + profileCode + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            using (DbCommander commander = new DbCommander(connection))
            {
                return commander.ExecuteNonQuery(commandText);
            }
        }

        public static int UpdateTerminateDate(string connectionString, string profileCode, DateTime? dateQuit)
        {
            string tableName = "tblEmployee";
            string profileField = "EmployeeID";
            string cardCodeField = "CardNo";
            string dateQuitField = "TerminateDate";

            string commandText = "Update " + tableName + " Set EmployeeStatusID = " + (dateQuit.HasValue ? 20 : 0);

            if (!string.IsNullOrWhiteSpace(cardCodeField))
            {
                commandText += ", " + cardCodeField + " = ''";
            }

            if (!string.IsNullOrWhiteSpace(dateQuitField))
            {
                if (dateQuit.HasValue)
                {
                    commandText += ", " + dateQuitField + " = '" + dateQuit.Value.ToString("yyyy/MM/dd") + "'";
                }
                else
                {
                    commandText += ", " + dateQuitField + " = null";
                    commandText += ", Matched = null";
                }
            }

            commandText += " Where " + profileField + " = '" + profileCode + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            using (DbCommander commander = new DbCommander(connection))
            {
                return commander.ExecuteNonQuery(commandText);
            }
        }

        public static int UpdateCardHistory(string connectionString, string profileCode, string cardCode)
        {
            string tableName = "tblCard";
            string cardCodeField = "CardID";
            string profileField = "Employee";

            string commandText = "Update " + tableName + " Set " + cardCodeField + " = '" + cardCode + "'";
            commandText += " Where " + profileField + " = '" + profileCode + "'";

            SqlConnection connection = new SqlConnection(connectionString);

            using (DbCommander commander = new DbCommander(connection))
            {
                return commander.ExecuteNonQuery(commandText);
            }
        }

        public void SyncProfileQuit()
        {
            string settingkey = AppConfig.HRM_SYS_TAMSCANLOG_1_.ToString();
            var listSetting = CreateQueryable<Sys_AllSetting>(Guid.Empty, d => d.Name.Contains(settingkey)).ToArray();
            string serverName = listSetting.Where(m => m.Name == settingkey + AppConfig.SERVERNAME).Select(m => m.Value1).FirstOrDefault();
            string userName = listSetting.Where(m => m.Name == settingkey + AppConfig.USERID).Select(m => m.Value1).FirstOrDefault();
            string password = listSetting.Where(m => m.Name == settingkey + AppConfig.PASSWORD).Select(m => m.Value1).FirstOrDefault();
            string dbName = listSetting.Where(m => m.Name == settingkey + AppConfig.DBNAME).Select(m => m.Value1).FirstOrDefault();
            string isActivated = listSetting.Where(m => m.Name == settingkey + AppConfig.ISACTIVATED).Select(m => m.Value1).FirstOrDefault();
            string allowSyncCard = listSetting.Where(m => m.Name == settingkey + AppConfig.SYNC_CARD).Select(m => m.Value1).FirstOrDefault();
            string delayDays = listSetting.Where(m => m.Name == settingkey + AppConfig.DELAY_DAYS).Select(m => m.Value1).FirstOrDefault();
            string connectionString1 = DbClientHelper.BuildSqlConnectionString(serverName, dbName, userName, password);

            settingkey = AppConfig.HRM_SYS_TAMSCANLOG_2_.ToString();
            listSetting = CreateQueryable<Sys_AllSetting>(Guid.Empty, d => d.Name.Contains(settingkey)).ToArray();
            serverName = listSetting.Where(m => m.Name == settingkey + AppConfig.SERVERNAME).Select(m => m.Value1).FirstOrDefault();
            userName = listSetting.Where(m => m.Name == settingkey + AppConfig.USERID).Select(m => m.Value1).FirstOrDefault();
            password = listSetting.Where(m => m.Name == settingkey + AppConfig.PASSWORD).Select(m => m.Value1).FirstOrDefault();
            dbName = listSetting.Where(m => m.Name == settingkey + AppConfig.DBNAME).Select(m => m.Value1).FirstOrDefault();
            isActivated = listSetting.Where(m => m.Name == settingkey + AppConfig.ISACTIVATED).Select(m => m.Value1).FirstOrDefault();
            allowSyncCard = listSetting.Where(m => m.Name == settingkey + AppConfig.SYNC_CARD).Select(m => m.Value1).FirstOrDefault();
            delayDays = listSetting.Where(m => m.Name == settingkey + AppConfig.DELAY_DAYS).Select(m => m.Value1).FirstOrDefault();
            string connectionString2 = DbClientHelper.BuildSqlConnectionString(serverName, dbName, userName, password);

            var deleteCardDelayDays = VnResource.Helper.Data.DataHelper.TryGetValue<int>(delayDays);
            deleteCardDelayDays = deleteCardDelayDays < 0 ? 0 : deleteCardDelayDays;

            DateTime dateFrom = CurrentDate().Date;
            DateTime dateTo = dateFrom.AddDays(1).AddSeconds(-1);

            dateFrom = dateFrom.AddDays(-deleteCardDelayDays);
            dateTo = dateTo.AddDays(-deleteCardDelayDays);

            var listProfile = CreateQueryable<Hre_Profile>(d => d.DateQuit.HasValue
                && d.DateQuit >= dateFrom && d.DateQuit <= dateTo).Select(d => new
                {
                    d.ID,
                    d.CodeEmp,
                    d.CodeAttendance,
                    d.DateQuit,
                    d.DateHire
                }).ToArray();

            foreach (var profile in listProfile)
            {
                UpdateTerminateDate(connectionString2, profile.CodeEmp, profile.DateQuit.Value);
                UpdateCardHistory(connectionString2, profile.CodeEmp, string.Empty);
            }
        }

        #endregion

        #region AfterImport

        private void ExecuteTaskAfterImport(params DbEntityEntry[] listItem)
        {
            var taskAfteImport = CreateQueryable<Sys_AllSetting>(Guid.Empty, d =>
                d.Name == AppConfig.E_TASK_AFTER_IMPORT_PROFILE.ToString()).FirstOrDefault();

            if (taskAfteImport != null && taskAfteImport.Value1 == Boolean.TrueString)
            {
                foreach (var item in listItem)
                {
                    if (item.Entity != null)
                    {
                        if (item.Entity.GetType().GetRealEntityType() == typeof(Hre_Profile))
                        {
                            string codeAttendance = string.Empty;

                            if (item.State == EntityState.Added)
                            {
                                codeAttendance = item.Entity.GetPropertyValue("CodeAttendance").GetString();
                            }
                            else if (item.State == EntityState.Modified)
                            {
                                if (item.Property("CodeAttendance").IsModified)
                                {
                                    codeAttendance = item.Entity.GetPropertyValue("CodeAttendance").GetString();
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(codeAttendance))
                            {
                                var profileID = item.Entity.GetPropertyValue("ID");
                                var dateApply = item.Entity.GetPropertyValue("DateApplyAttendanceCode");
                                dateApply = dateApply != null ? dateApply : CurrentDate();

                                Hre_CardHistory cardHistory = new Hre_CardHistory();
                                cardHistory.SetPropertyValue("ProfileID", profileID);
                                cardHistory.SetPropertyValue("DateEffect", dateApply);
                                cardHistory.CardCode = codeAttendance;
                                cardHistory.ID = Guid.NewGuid();
                                AddObject(cardHistory);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
