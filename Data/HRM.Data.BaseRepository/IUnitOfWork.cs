using System;
using HRM.Infrastructure.Utilities;
using System.Data.Entity;
using System.Collections;
using HRM.Data.Entity;
using VnResource.Helper.Security;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HRM.Data.BaseRepository
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }
        DataErrorCode SaveChanges();
        DataErrorCode SaveChanges(Guid userID);
        DataErrorCode SaveChanges(Guid userID, bool isFromImport);
        int GetMaxLength(Type entityType, string fieldName);
        bool IsMetadataField(Type entityType, string fieldName);
        bool IsMetadataType(Type entityType);
        string GetFieldConstraint(Type entityType, string objectField);
        string GetFieldNavigation(Type entityType, string constraintField);
        string[] GetNotNullFields(Type entityType);
        Type GetEntityType(string entityName);
        DateTime CurrentDate();

        #region SetOrgStructureID

        bool SetCorrectOrgStructureID(IList listEntity);

        bool SetCorrectOrgStructureID(List<WorkHistoryInfo> listWorkHistory, IList listEntity);

        bool SetCorrectOrgStructureID(string checkField, List<WorkHistoryInfo> listWorkHistory, IList listEntity);

        #endregion

        #region GetConditionString

        void GetConditionString(Guid userID, PrivilegeType privilegeType,
            Type entityType, out string conditionString, out object[] parameters);

        #endregion

        #region CreateQueryable

        IQueryable CreateQueryable(Type entityType, string predicate, params object[] parameters);
        IQueryable CreateQueryable(Guid userID, Type entityType, string predicate, params object[] parameters);
        IQueryable<TEntity> CreateQueryable<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        IQueryable<TEntity> CreateQueryable<TEntity>(Guid userID, params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;

        #endregion

        #region IsDeleteByQuery

        int SetIsDelete(IQueryable queryable);

        int SetIsDelete<TEntity>(IQueryable<TEntity> queryable) where TEntity : class;

        #endregion

        #region DeleteByQuery

        int Delete(IQueryable queryable);

        int Delete<TEntity>(IQueryable<TEntity> queryable) where TEntity : class;

        #endregion

        #region AddObject

        void AddObject(params object[] entities);
        void AddObject(Type entityType, params object[] entities);
        void RemoveObject(params object[] entities);
        void RemoveObject(Type entityType, params object[] entities); 

        #endregion

        #region GenerateCode

        void GenerateCode(Guid userID, string fieldName, params object[] entities);

        void GenerateCode(Guid userID, string[] fieldNames, params object[] entities);

        void GenerateCode(Guid userID, string fieldName, bool isOnInitObject, params object[] entities);

        void GenerateCode(Guid userID, string[] fieldNames, bool isOnInitObject, params object[] entities);

        #endregion
    }
}
