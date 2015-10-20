using System.Collections;
using System.Data;
using System.Globalization;
using Devart.Data.Oracle;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VnResource.Helper.Data;
using VnResource.Helper.Linq;
using EntityState = System.Data.Entity.EntityState;
using VnResource.Helper.Utility;

namespace HRM.Data.BaseRepository
{
    public class CustomBaseRepository<TEntity> : GenericRepository<TEntity> where TEntity : class
    {
        public CustomBaseRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region DeleteData

        /// <summary>
        /// [Chuc.Nguyen] - Xóa dữ liệu theo điều kiện dùng Store
        /// </summary>
        /// <param name="condition">Điều kiện xóa dữ liệu</param>
        /// <param name="storeName">Tên store không có tham số</param>
        /// <returns></returns>
        public string DeleteData(ListQueryModel condition, string storeName)
        {
            var status = string.Empty;
            try
            {
                ExecuteQuery(condition, storeName, ref status);
                return status;
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return status;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa dữ liệu theo điều kiện dùng Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string DeleteData(List<TEntity> entity)
        {
            string status;
            try
            {
                Delete(entity);
                SaveChanges();
                status = NotificationType.Success.ToString();
                return status;
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return status;
            }
        }
        #endregion

        #region GetData Group

        /// <summary>
        /// [Chuc.Nguyen] - Lấy một dòng dữ liệu dùng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy một dòng dữ liệu đầu tiên</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetData(object keyword, string storeName, string userLogin, ref string status)
        {
            return ExecuteQuery(keyword, storeName, userLogin, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu dùng Store
        /// </summary>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetData(string storeName, ref string status)
        {
            return ExecuteQuery(storeName, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu có điều kiện dùng Store
        /// </summary>
        /// <param name="condition">Điều kiện lấy dữ</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetData(ListQueryModel condition, string storeName, ref string status)
        {
            return ExecuteQuery(condition, storeName, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu có điều kiện dùng Store theo List Parameter
        /// </summary>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="useCursor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetData(List<object> listParameter, string storeName, bool useCursor, string userLogin, ref string status)
        {
            return ExecuteQuery(listParameter, storeName, useCursor, userLogin, ref status);
        }
        #endregion

        #region ExecuteQuery Group
        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu query dùng Store
        /// </summary>
        /// <param name="storeName">Tên store không có tham số</param>
        /// <param name="status">Trạng thái lấy thành công hay thất bại</param>
        /// <returns></returns>
        public IQueryable<TEntity> ExecuteQuery(string storeName, ref string status)
        {
            try
            {
                IQueryable<TEntity> listEntity;
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    if (storeName.Split('(').Length == 1)
                    {
                        storeName = "begin " + storeName + "(:R_Output);end;";
                    }
                    listEntity = this.UnitOfWork.Context.Database.SqlQuery<TEntity>(storeName,
                        new OracleParameter()
                        {
                            ParameterName = "R_Output",
                            OracleDbType = OracleDbType.Cursor,
                            Direction = System.Data.ParameterDirection.Output
                        }
                    ).AsQueryable();
                }
                else
                {
                    listEntity = UnitOfWork.Context.Database.SqlQuery<TEntity>(storeName).AsQueryable();
                }

                status = NotificationType.Success.ToString();
                return listEntity;
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu query theo điều kiện sử dụng Store
        /// </summary>
        /// <param name="condition">Điều kiện lấy dữ liệu</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> ExecuteQuery(ListQueryModel condition, string storeName, ref string status)
        {
            try
            {
                ParamaterModle param = ListUtility.ParseParam(condition, storeName);
                status = NotificationType.Success.ToString();
                return UnitOfWork.Context.Database.SqlQuery<TEntity>(param.SqlQuery, param.Params).AsQueryable();
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu query theo cặp Parameter sử dụng Store
        /// </summary>
        /// <param name="listParameter">Danh sách parameter</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="useCursor">Cho biết có sử dụng biến Out với Oracle</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> ExecuteQuery(List<object> listParameter, string storeName, bool useCursor, string userLogin, ref string status)
        {
            try
            {
                string strQuery = string.Empty;
                var parameterObjects = new List<object>();

                if (!string.IsNullOrEmpty(storeName))
                {
                    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                    {
                        if (storeName.Contains("begin"))
                        {
                            storeName = storeName.Substring(5, storeName.IndexOf('(') - 5);
                        }
                        strQuery = "begin " + storeName + "(";
                        for (int i = 0; i < listParameter.Count; i++)
                        {
                            var strParameter = listParameter[i];
                            strQuery += ":parameter" + i;
                            if (i != listParameter.Count - 1)
                            {
                                strQuery += ",";
                            }
                            var parameter = new OracleParameter
                            {
                                ParameterName = "parameter" + i,
                                Value = strParameter,
                                OracleDbType = OracleDbType.Object,
                                DbType = DbType.Object
                            };
                            if (strParameter != null)
                            {
                                if (!Common.IsAscii(strParameter.ToString()))
                                {
                                    parameter.OracleDbType = OracleDbType.NVarChar;
                                }
                            }
                            parameterObjects.Add(parameter);
                        }
                        if (ListUtility.CheckStoreNamePermisson(storeName))
                        {
                            strQuery += ",:UserName";
                            var para = new OracleParameter();
                            para.ParameterName = "UserName";
                            para.Value = userLogin;
                            para.DbType = System.Data.DbType.String;
                            para.OracleDbType = OracleDbType.NVarChar;
                            parameterObjects.Add(para);
                        }
                        if (useCursor)
                        {
                            strQuery += ",:R_Output";
                            var parameter = new OracleParameter
                            {
                                ParameterName = "R_Output",
                                OracleDbType = OracleDbType.Cursor,
                                Direction = System.Data.ParameterDirection.Output
                            };
                            parameterObjects.Add(parameter);
                        }
                        strQuery += "); end;";
                    }
                    else
                    {
                        if (!storeName.Contains("exec"))
                        {
                            strQuery = "exec " + storeName + " ";
                        }
                        else
                        {
                            strQuery = storeName + " ";
                        }

                        for (int i = 0; i < listParameter.Count; i++)
                        {
                            strQuery += "@parameter" + i;
                            if (i != listParameter.Count - 1)
                            {
                                strQuery += ",";
                            }
                            var parameter = new SqlParameter
                            {
                                ParameterName = "parameter" + i,
                                Value = listParameter[i] ?? DBNull.Value
                            };

                            parameterObjects.Add(parameter);
                            //  parameterObjects.Add(parameter);
                        }
                    }
                }
                status = NotificationType.Success.ToString();
                return UnitOfWork.Context.Database.SqlQuery<TEntity>(strQuery, parameterObjects.ToArray()).AsQueryable(); ;
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu query theo keyword sử dụng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, Id, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public IQueryable<TEntity> ExecuteQuery(object keyword, string storeName, string userLogin, ref string status)
        {
            try
            {
                #region Parameter

                var storeItem = storeName.Split('@');
                var strParameter = string.Empty;
                var strParameter1 = string.Empty;
                var strParameter2 = "UserName";
                if (storeItem.Length > 1)
                {
                    //storeName: "exec hrm_att_sp_get_Att_GradeById @Id"
                    strParameter = storeItem[1];
                }
                else
                {
                    //storeName: "begin ATT_SP_GET_GRADEBYID(:Id,:R_Output); end;"
                    var n = storeName.IndexOf('(');
                    var n1 = storeName.IndexOf(')');
                    var n2 = storeName.IndexOf(',');
                    if (n >= 0)
                    {
                        strParameter = storeName.Substring(n + 2, n2 - (n + 2));
                        strParameter1 = storeName.Substring(n2 + 2, n1 - (n2 + 2));
                    }
                }

                #endregion

                IQueryable<TEntity> listEntity;
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    OracleParameter paraKeyword = new OracleParameter();
                    OracleParameter paraUser = new OracleParameter();
                    if (keyword != null && keyword.GetType() == typeof(string))
                    {
                        paraKeyword.ParameterName = strParameter;
                        paraKeyword.Value = keyword;
                        paraKeyword.OracleDbType = OracleDbType.NVarChar;
                    }
                    else
                    {
                        paraKeyword.ParameterName = strParameter;
                        paraKeyword.Value = keyword;
                        paraKeyword.OracleDbType = OracleDbType.Object;
                        paraKeyword.DbType = DbType.Object;
                    }


                    if (ListUtility.CheckStoreNamePermisson(storeName))
                    {
                        storeName = storeName.Replace(":R_Output", ":UserName, :R_Output");
                        listEntity = UnitOfWork.Context.Database.SqlQuery<TEntity>(storeName,
                          paraKeyword,
                          new OracleParameter()
                          {
                              ParameterName = strParameter2,
                              Value = userLogin,
                              OracleDbType = OracleDbType.NVarChar,
                          },
                          new OracleParameter()
                          {
                              ParameterName = strParameter1,
                              OracleDbType = OracleDbType.Cursor,
                              Direction = System.Data.ParameterDirection.Output
                          }
                       ).AsQueryable();
                    }
                    else
                    {
                        listEntity = UnitOfWork.Context.Database.SqlQuery<TEntity>(storeName,
                        paraKeyword,
                        new OracleParameter()
                        {
                            ParameterName = strParameter1,
                            OracleDbType = OracleDbType.Cursor,
                            Direction = System.Data.ParameterDirection.Output
                        }
                     ).AsQueryable();
                    }
                  
                }
                else
                {
                    SqlParameter paraKeyword = new SqlParameter();
                    if (keyword != null && keyword.GetType() == typeof(string))
                    {
                        paraKeyword.ParameterName = strParameter;
                        paraKeyword.DbType = DbType.String;
                    }
                    else if (keyword != null && keyword.GetType() == typeof(Guid))
                    {
                        paraKeyword.ParameterName = strParameter;
                        paraKeyword.DbType = DbType.Guid;
                    }
                    else
                    {
                        paraKeyword.ParameterName = strParameter;
                        paraKeyword.DbType = DbType.Object;
                    }
                    paraKeyword.Value = keyword == null ? DBNull.Value : keyword;
                    listEntity = UnitOfWork.Context.Database.SqlQuery<TEntity>(storeName, paraKeyword).AsQueryable();
                }
                status = NotificationType.Success.ToString();
                return listEntity;
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
                return null;
            }
        }



        #endregion
    }

    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected DbSet<TEntity> DbSet { get; set; }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(typeof(UnitOfWork).Name);
            this.UnitOfWork = unitOfWork;
            this.DbSet = this.UnitOfWork.Context.Set<TEntity>();
        }

        public DateTime CurrentDateTime()
        {
            var currentDateTime = this.UnitOfWork.Context.Database.SqlQuery<DateTime>("exec hrm_sp_get_datetime").FirstOrDefault<DateTime>();
            return currentDateTime;
        }

        public DateTime CurrentDate()
        {
            var currentDate = CurrentDateTime();
            DateTime dtCurrent = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            return dtCurrent;
        }

    

        /// <summary>
        /// [Chuc.Nguyen] - Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            //.Where("IsDelete = null Or IsDelete = @0", false)
            return this.DbSet.AsQueryable();
        }

        public TEntity GetById(Guid ID)
        {
            TEntity result = this.DbSet.Find(ID);
            return result;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = UnitOfWork.CreateQueryable<TEntity>(predicate);
            return query;
        }

        public IQueryable<TResult> FindBy<TResult>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector)
        {
            return UnitOfWork.CreateQueryable<TEntity>(predicate).Select(selector);
        }

        public virtual void Add(TEntity entity)
        {
            UnitOfWork.AddObject(typeof(TEntity), entity);
        }

        public virtual void Add(List<TEntity> listEntity)
        {
            UnitOfWork.AddObject(typeof(TEntity), listEntity.ToArray());
        }

        public virtual void Attach(TEntity entity)
        {
            if (this.UnitOfWork.Context.Entry(entity).State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
        }

        public virtual void Remove(TEntity entity)
        {
            PropertyInfo info = typeof(TEntity).GetProperty("IsDelete");
            if (info != null)
            {
                info.SetValue(entity, true);
                DbSet.Attach(entity);
                this.UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
            }
            else
                Delete(entity);
        }

        public virtual void Save()
        {
            this.UnitOfWork.SaveChanges();
        }

        public virtual void Edit(TEntity entity)
        {
            DbSet.Attach(entity);
            this.UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Edit(List<TEntity> listEntity)
        {
            this.UnitOfWork.Context.Configuration.AutoDetectChangesEnabled = false;
            foreach (var entity in listEntity)
            {
                DbSet.Attach(entity);
                this.UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
            }

            this.UnitOfWork.Context.Configuration.AutoDetectChangesEnabled = true;
        }
        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            this.DbSet.RemoveRange(entities);
        }
        public virtual void SetNewCode(IUnitOfWork unitOfWork, Guid userID, HRM.Data.Entity.Sys_CodeObject codeObject, IList objects)
        {
            this.SetNewCode(unitOfWork, userID, codeObject, objects);
        }

        public virtual DataErrorCode SaveChanges()
        {
            return this.UnitOfWork.SaveChanges();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public DataErrorCode SubmitChanges()
        {
            try
            {
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                DataError dataError = new DataError();
                if (e.Message.Contains("Cannot insert the value NULL into column"))
                {
                    dataError.DataErrorCode = DataErrorCode.FieldNotAcceptNull;
                    dataError.CollumName = e.Message.Split('\'')[1];
                    dataError.TableName = e.Message.Split('\'')[3];
                    //FireDataErrorEvent(dataError);
                    return DataErrorCode.FieldNotAcceptNull;
                }
                else if (e.Message.Contains("FOREIGN KEY constraint"))
                {
                    dataError.DataErrorCode = DataErrorCode.FieldNotAcceptNull;
                    dataError.CollumName = e.Message.Split('\"').Where(str => str.StartsWith("dbo.")).Single().Substring(4);
                    //FireDataErrorEvent(dataError);
                    return DataErrorCode.FieldNotAcceptNull;
                }
                else if (e.Message.Contains("Cannot add an entity with a key that is already in use") || e.Message.Contains("Cannot insert duplicate key"))
                {
                    dataError.DataErrorCode = DataErrorCode.Dupplicate;
                    //FireDataErrorEvent(dataError);
                    return DataErrorCode.Dupplicate;
                }
                else
                {
                    if (e.Message.Contains("binary data"))
                        return DataErrorCode.BINARYDATA;
                    else
                        return DataErrorCode.Unknown;
                }
            }
            return DataErrorCode.Success;
        }
    }

    public class GenericRepository
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(typeof(UnitOfWork).Name);
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu lệnh sql 
        /// </summary>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public void ExecuteSqlCommand(string storeName, ref string status)
        {
            try
            {
                string strQuery = string.Empty;

                if (!string.IsNullOrEmpty(storeName))
                {
                    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                    {
                        if (storeName.Contains("begin"))
                        {
                            storeName = storeName.Substring(5, storeName.IndexOf('(') - 5);
                        }
                        strQuery = "begin " + storeName + "(";
                     
                        strQuery += "); end;";
                    }
                    else
                    {
                        if (!storeName.Contains("exec"))
                        {
                            strQuery = "exec " + storeName + " ";
                        }
                        else
                        {
                            strQuery = storeName + " ";
                        }
                    }
                }
                UnitOfWork.Context.Database.ExecuteSqlCommand(strQuery);
                status = NotificationType.Success.ToString();
            }
            catch (Exception ex)
            {
                status = NotificationType.Error + "," + ex.Message;
            }
        }
    }
}
