using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Linq;
using VnResource.Helper.Data;
using VnResource.Helper.Utility;
using System.Reflection;
using System.Collections;
using HRM.Data.Entity;
using System.Data.Entity.Validation;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;

namespace HRM.Business.Main.Domain
{
    public class BaseService
    {
        public List<EntityType> GetListEntityType()
        {
            using (var context = new VnrHrmDataContext())
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.ListEntityType;
            }
        }

        public bool SendMail(string titleMail, string MailTo, string body, string attachFile)
        {
            return SendMail(titleMail, MailTo, body, attachFile, null);
        }

        /// <summary>
        /// Hieu.Van
        /// Hàm xử lý get thông tin để send mail
        /// </summary>
        /// <param name="titleMail"></param>
        /// <param name="MailTo"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendMail(string titleMail, string MailTo, string body, string attachFile,List<string> lstCcMail)
        {
            string status = string.Empty;
            var key = "HRM_SYS_CONFIG_MAIL_";
            List<object> lstParam = new List<object>();
            lstParam.Add(key);
            lstParam.Add(1);
            lstParam.Add(int.MaxValue - 1);
            var dataMail = GetData<Sys_AllSettingEntity>(lstParam, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            int Port = 587;
            string Host = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILSERVER.ToString()).FirstOrDefault().Value1;
            string MailFrom = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME.ToString()).FirstOrDefault().Value1;
            string MailUserName = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME.ToString()).FirstOrDefault().Value1;
            string MailPassword = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILPASSWORD.ToString()).FirstOrDefault().Value1;
            bool IsSSL = true;
            Boolean.TryParse(dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_SSL.ToString()).FirstOrDefault().Value1, out IsSSL);

            bool isSuccess = Common.ProcessSendMail(titleMail, Host, Port, MailFrom, MailUserName, MailPassword, MailTo, IsSSL, body, attachFile, lstCcMail);
            return isSuccess;
        }

        /// <summary>
        /// Hieu.Van
        /// Hàm xử lý get thông tin để send mail
        /// </summary>
        /// <param name="titleMail"></param>
        /// <param name="MailTo"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static void GetConfigSendMail(out string Host, out int Port, out string MailFrom, out string MailUserName, out string MailPassword, out bool IsSSL)
        {
            BaseService _base = new BaseService();
            string status = string.Empty;
            var key = "HRM_SYS_CONFIG_MAIL_";
            List<object> lstParam = new List<object>();
            lstParam.Add(key);
            lstParam.Add(1);
            lstParam.Add(int.MaxValue - 1);
            var dataMail = _base.GetData<Sys_AllSettingEntity>(lstParam, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            Port = 587;
            Host = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILSERVER.ToString()).FirstOrDefault().Value1;
            MailFrom = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME.ToString()).FirstOrDefault().Value1;
            MailUserName = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILUSERNAME.ToString()).FirstOrDefault().Value1;
            MailPassword = dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_MAILPASSWORD.ToString()).FirstOrDefault().Value1;
            IsSSL = true;
            Boolean.TryParse(dataMail.Where(s => s.Name == AppConfig.HRM_SYS_CONFIG_MAIL_SSL.ToString()).FirstOrDefault().Value1, out IsSSL);
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm tính value cho công tức
        /// </summary>
        /// <param name="ListElement">List các Element đã tính được trước đó</param>
        /// <param name="formula">Công thức</param>
        /// <returns>Giá trị tính được từ công thức</returns>
        public object GetObjectValue(List<Cat_ElementEntity> listElement, List<ElementFormula> ListElementFormula, string formula)
        {
            try
            {
                formula = formula.Replace("\n", "").Replace("\t", "").Trim();
                //Các phần tử tính lương tách ra từ 1 chuỗi công thức
                var ListFormula = ParseFormulaToList(formula).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();

                //Các phần tử tính lương chưa có kết quả
                var ListFormulaNotValue = ListFormula.Where(m => !ListElementFormula.Any(t => t.VariableName == m.Replace("[", "").Replace("]", ""))).ToList();

                //Nếu có tồn tại phần tử chưa có Value, thì đi tính value cho phần tử đó
                if (ListFormulaNotValue != null && ListFormulaNotValue.Count > 0)
                {
                    foreach (var i in ListFormulaNotValue)
                    {
                        if (!ListElementFormula.Any(m => m.VariableName == i.Replace("[", "").Replace("]", "")))
                        {
                            string ttt = "";
                            foreach (var j in listElement)
                            {
                                if (j.ElementCode == i.Replace("[", "").Replace("]", ""))
                                    ttt = j.Formula;
                            }
                            ElementFormula item = new ElementFormula(i.Replace("[", "").Replace("]", ""), GetObjectValue(listElement, ListElementFormula, listElement.Where(m => m.ElementCode == i.Replace("[", "").Replace("]", "")).FirstOrDefault().Formula), 0);
                            ListElementFormula.Add(item);
                        }
                    }
                }
                //Do mệnh đề if luôn trả về false nên xử lý riêng cho mệnh đề if ở đây
                //nguyên ngân là do dll CalcEngine, nhưng chưa tìm ra cách giải quyết
                if (formula.ToUpper().Contains("IF("))
                {
                    foreach (var i in ListElementFormula.Distinct().ToList())
                    {
                        if (formula.Contains("[" + i.VariableName + "]"))
                        {
                            formula = formula.Replace("[" + i.VariableName + "]", i.Value.ToString());
                        }
                    }
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }
                else
                {
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Tách công thức thành List các phần tử để kiểm tra
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public List<string> ParseFormulaToList(string formula)
        {
            formula = formula.Replace("[", " [").Replace("]", "] ");
            return formula.Split(' ').Where(m => m.StartsWith("[") && m.EndsWith("]")).Distinct().ToList();


            //formula = formula.Replace(" ", "").Replace("+", " ").Replace("-", " ").Replace("*", " ").Replace("/", " ").Replace("(", " ").Replace(")", "");
            //return formula.Split(' ').ToList();
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chỉnh sửa một đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Edit<TEntity>(TEntity entity) where TEntity : class
        {
            return Edit<TEntity>(entity, Guid.Empty);
        }
        public string Edit<TEntity>(TEntity entity, Guid userID) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var name = entity.GetType().Name.ToString();
                    //Type dbEntityType = LibraryService.GetEntityType(name.Substring(0, name.Length - 6));
                    Type dbEntityType = LibraryService.GetEntityType(name.Substring(0, name.LastIndexOf("Entity")));
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var objectID = entity.GetPropertyValue(Constant.ID).TryGetValue<Guid>();
                    object dbEntity = unitOfWork.CreateQueryable(userID, dbEntityType, Constant.ID + " = @0", objectID).FirstOrDefault();
                    if (objectID != Guid.Empty)
                    {
                        entity.SetPropertyValue(Constant.DateCreate, dbEntity.GetPropertyValue(Constant.DateCreate));
                        entity.SetPropertyValue(Constant.UserCreate, dbEntity.GetPropertyValue(Constant.UserCreate));
                        entity.SetPropertyValue(Constant.IPCreate, dbEntity.GetPropertyValue(Constant.IPCreate));
                        entity.SetPropertyValue(Constant.ServerCreate, dbEntity.GetPropertyValue(Constant.ServerCreate));
                    }
                    entity.CopyData(dbEntity, Constant.ID);
                    var status = unitOfWork.SaveChanges(userID);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }


        /// <summary>
        /// [Chuc.Nguyen] - Chỉnh sửa một danh sách đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public string Edit<TEntity>(List<TEntity> listEntity) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    repo.Edit(listEntity);
                    var status = repo.SaveChanges();

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    return NotificationType.Error + "," + exceptionMessage;
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        ///// <summary>
        ///// [Chuc.Nguyen] - Lưu một danh sách đối tượng
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="listEntity"></param>
        ///// <returns></returns>
        //public string Add<TEntity>(List<TEntity> listEntity) where TEntity : class
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new CustomBaseRepository<TEntity>(unitOfWork);
        //        try
        //        {
        //            repo.Add(listEntity);
        //            repo.SaveChanges();
        //            return NotificationType.Success.ToString();
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        //        {
        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            return NotificationType.Error + "," + ex.Message;
        //        }
        //    }
        //}

        /// <summary>
        /// [Chuc.Nguyen] - Tạo mới một đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Add<TEntity>(List<TEntity> listEntity) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var status = DataErrorCode.Success;

                    if (listEntity != null && listEntity.Count > 0)
                    {
                        var name = listEntity[0].GetType().Name.ToString();
                        Type dbEntityType = LibraryService.GetEntityType(name.Substring(0, name.Length - 6));
                        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                        var listDbEntity = new List<object>();
                        foreach (var item in listEntity)
                        {
                            object dbEntity = Activator.CreateInstance(dbEntityType);
                            item.CopyData(dbEntity);
                            dbEntity.SetPropertyValue(Constant.DateCreate, DateTime.Now);
                            listDbEntity.Add(dbEntity);
                        }

                        unitOfWork.AddObject(dbEntityType, listDbEntity.ToArray());
                        status = unitOfWork.SaveChanges(Guid.Empty);
                    }

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    return NotificationType.Error + "," + exceptionMessage;
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }
        public string Add<TEntity>(TEntity entity) where TEntity : class
        {
            return Add<TEntity>(entity, Guid.Empty);
        }
        /// <summary>
        /// [Chuc.Nguyen] - Tạo mới một đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Add<TEntity>(TEntity entity, Guid userID) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var name = entity.GetType().Name.ToString();
                    Type dbEntityType = LibraryService.GetEntityType(name.Substring(0, name.Length - 6));
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    object dbEntity = Activator.CreateInstance(dbEntityType);
                    entity.CopyData(dbEntity);

                    Guid newID = Guid.NewGuid();
                    dbEntity.SetPropertyValue(Constant.ID, newID);
                    dbEntity.SetPropertyValue(Constant.DateCreate, DateTime.Now);
                    unitOfWork.AddObject(dbEntityType, dbEntity);

                    if (dbEntityType == typeof(Hre_Profile))
                    {
                        unitOfWork.GenerateCode(userID, "CodeEmp", dbEntity);
                    }

                    var status = unitOfWork.SaveChanges(userID);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        entity.SetPropertyValue(Constant.ID, newID);
                        return NotificationType.Success.ToString();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    return NotificationType.Error + "," + exceptionMessage;
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        /// <summary>
        /// [Hieu.Van] - Tạo mới một đối tượng return model
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Add<TEntity>(TEntity entity, ref string status) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    entity.SetPropertyValue(Constant.DateCreate, DateTime.Now);
                    repo.Add(entity);
                    var saveStatus = repo.SaveChanges();

                    if (saveStatus == DataErrorCode.Locked)
                    {
                        status = NotificationType.Locked.ToString();
                    }
                    else
                    {
                        status = NotificationType.Success.ToString();
                    }

                    return entity;
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Chuyển đổi trạng thái của một đối tượng sang IsDelete
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Remove<TEntity>(Guid ID) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    List<object> listParamater = new List<object>();
                    //string tableName = typeof(TEntity).Name.Replace("Entity", "");
                    string tableName = typeof(TEntity).Name;
                    tableName = tableName.Substring(0, tableName.Length - 6);
                    Type dbEntityType = LibraryService.GetEntityType(tableName);
                    object dbEntity = unitOfWork.CreateQueryable(Guid.Empty, dbEntityType, Constant.ID + " = @0", ID).FirstOrDefault();
                    dbEntity.SetPropertyValue("IsDelete", true);
                    var status = unitOfWork.SaveChanges(Guid.Empty);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        /// <summary>
        /// [HieuVan] - Chuyển đổi trạng thái của một đối tượng Status
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public string UpdateStatus<TEntity>(Guid ID, string type) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    List<object> listParamater = new List<object>();
                    string tableName = typeof(TEntity).Name;
                    tableName = tableName.Substring(0, tableName.Length - 6);
                    Type dbEntityType = LibraryService.GetEntityType(tableName);
                    object dbEntity = unitOfWork.CreateQueryable(Guid.Empty, dbEntityType, Constant.ID + " = @0", ID).FirstOrDefault();
                    dbEntity.SetPropertyValue("Status", type);
                    var status = unitOfWork.SaveChanges(Guid.Empty);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa một đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete<TEntity>(Guid ID) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    //string tableName = typeof(TEntity).Name.Replace("Entity", "");
                    string tableName = typeof(TEntity).Name;
                    tableName = tableName.Substring(0, tableName.Length - 6);
                    Type dbEntityType = LibraryService.GetEntityType(tableName);
                    object dbEntity = unitOfWork.CreateQueryable(Guid.Empty, dbEntityType, Constant.ID + " = @0", ID).FirstOrDefault();
                    unitOfWork.RemoveObject(dbEntityType, dbEntity);
                    var status = unitOfWork.SaveChanges(Guid.Empty);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        public string Delete<TEntity>(List<TEntity> listEntity) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    unitOfWork.RemoveObject(typeof(TEntity), listEntity);
                    var status = unitOfWork.SaveChanges(Guid.Empty);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }
        public string Delete(Type entityType, params object[] entities)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    unitOfWork.RemoveObject(entityType, entities);
                    var status = unitOfWork.SaveChanges(Guid.Empty);

                    if (status == DataErrorCode.Locked)
                    {
                        return NotificationType.Locked.ToString();
                    }
                    else
                    {
                        return NotificationType.Success.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        public string DeleteColumMode(Guid id, string gridName)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    var repo = new CustomBaseRepository<Sys_ColumnMode>(unitOfWork);
                    var entity = repo.FindBy(d => d.UserInfoID == id && d.GridControlName == gridName).FirstOrDefault();
                    unitOfWork.RemoveObject(typeof(Sys_ColumnMode), entity);
                    unitOfWork.SaveChanges(Guid.Empty);
                    return NotificationType.Success.ToString();
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }
        /// <summary>
        /// [Chuc.Nguyen] - Lấy tất cả dữ liệu của một đối tượng bất kỳ dùng Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> GetAllUseEntity<TEntity>(ref string status) where TEntity : class
        {
            List<TEntity> entity = null;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    entity = repo.GetAll().ToList();
                    status = NotificationType.Success.ToString();
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                }
            }

            return entity;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu của một đối tượng bất kỳ theo Id dùng Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TEntity GetById<TEntity>(Guid id, ref string status) where TEntity : class
        {
            TEntity entity = null;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    entity = repo.GetById(id);
                    status = NotificationType.Success.ToString();
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                }
            }

            return entity;
        }

        #region GetData Group

        /// <summary>
        /// [Chuc.Nguyen] - Lấy một row dữ liệu của một đối tượng bất kỳ theo keyword dùng Store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="storeName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public object GetFirstData<TEntity>(object keyword, string storeName, string userLogin, ref string status) where TEntity : class
        {
            object fieldData = null;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    fieldData = repo.GetData(keyword, storeName, userLogin, ref status).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                }
            }

            return fieldData;
        }
        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu theo keyword sử dụng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, Id, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public List<TEntity> GetData<TEntity>(object keyword, string storeName, string userLogin, ref string status) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                List<TEntity> listEntity = new List<TEntity>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    listEntity = repo.GetData(keyword, storeName, userLogin, ref status).ToList();
                }
                catch (Exception ex)
                {
                    listEntity = null;
                    status = NotificationType.Error + "," + ex.Message;

                }
                return listEntity;

            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu có điều kiện sử dụng Store
        /// </summary>
        /// <param name="model"></param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public List<TEntity> GetData<TEntity>(ListQueryModel model, string storeName, string userLogin, ref string status) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                List<TEntity> listEntity;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    model.UserLogin = userLogin;
                    listEntity = repo.GetData(model, storeName, ref status).ToList();
                }
                catch (Exception ex)
                {
                    listEntity = null;
                    status = NotificationType.Error + "," + ex.Message;
                }

                return listEntity;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu sử dụng Store
        /// </summary>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns>
        public List<TEntity> GetDataNotParam<TEntity>(string storeName, string userLogin, ref string status) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                List<TEntity> listEntity;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    listEntity = repo.GetData(storeName, ref status).ToList();
                }
                catch (Exception ex)
                {
                    listEntity = null;
                    status = NotificationType.Error + "," + ex.Message;
                }

                return listEntity;
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu theo list parameter, có dữ liệu trả về nên dùng cursor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> GetData<TEntity>(List<object> listParameter, string storeName, string userLogin, ref string status) where TEntity : class
        {
            return ActionData<TEntity>(listParameter, storeName, true, userLogin, ref status);
        }

        #endregion

        #region UpdateData Group

        /// <summary>
        /// [Chuc.Nguyen] - Cập nhật dữ liệu theo list parameter, ko có dữ liệu trả về nên không dùng cursor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> UpdateData<TEntity>(List<object> listParameter, string storeName, ref string status) where TEntity : class
        {
            return ActionData<TEntity>(listParameter, storeName, false, string.Empty, ref status);
        }

        #endregion

        #region DeleteData Group

        /// <summary>
        /// [Chuc.Nguyen] - Xóa dữ liệu theo điều kiện sử dụng Store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public string DeleteData<TEntity>(ListQueryModel condition, string storeName) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    return repo.DeleteData(condition, storeName);
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        #endregion

        #region SaveData Group

        /// <summary>
        /// [Chuc.Nguyen] - Chạy câu query theo sử dụng Store theo Dictionary điều kiện key/value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <returns></returns>
        public string SaveData<TEntity>(Dictionary<object, object> value, string storeName) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    string status = string.Empty;
                    foreach (var item in value)
                    {
                        if (item.Value != null)
                        {
                            var checkExistName = GetData<TEntity>(item.Key, ConstantSql.hrm_cat_sp_get_GetAllSettings, string.Empty, ref status);
                            if (checkExistName != null && checkExistName.Count > 0)
                            {
                                List<object> listObj = new List<object>();
                                listObj.Add(item.Key);
                                listObj.Add(item.Value);
                                repo.ExecuteQuery(listObj, storeName, false, string.Empty, ref status).ToList<object>();
                            }
                            else
                            {
                                var entity = (TEntity)typeof(TEntity).CreateInstance();
                                entity.SetPropertyValue("Name", item.Key);
                                entity.SetPropertyValue("Value1", item.Value);
                                entity.SetPropertyValue("UserID", Guid.NewGuid());
                                Add<TEntity>(entity);
                                //repo.Add(entity);
                                //repo.SaveChanges();
                            }
                        }

                    }
                    return status;
                }
                catch (Exception ex)
                {
                    return NotificationType.Error + "," + ex.Message;
                }
            }
        }

        #endregion

        #region CustomNew

        /// <summary>
        /// [Chuc.Nguyen] - Chạy store với các tham số đều null (Chưa test)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storeName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> ExecStore<TEntity>(string storeName, string userLogin, ref string status) where TEntity : class
        {
            #region Parameter

            var listObjects = new List<object>();
            var i = 0;
            var useCursor = false;

            //storeName: "exec hrm_att_sp_get_Att_GradeById @Id"
            if (storeName.Contains("@"))
            {
                while ((i = storeName.IndexOf('@', i)) != -1)
                {
                    listObjects.Add(null);
                    i++;
                }
            }
            //storeName: "begin ATT_SP_GET_GRADEBYID(:Id,:R_Output); end;"
            else if (storeName.Contains(":"))
            {
                while ((i = storeName.IndexOf(':', i)) != -1)
                {
                    listObjects.Add(null);
                    i++;
                }

                if (storeName.Contains(":R_Output"))
                {
                    listObjects.RemoveAt(1);
                    useCursor = true;
                }
            }

            #endregion

            return ActionData<TEntity>(listObjects, storeName, useCursor, userLogin, ref status);
        }

        #endregion

        #region General
        /// <summary>
        /// [Chuc.Nguyen] - Hàm dùng chung cho thực hiện lệnh sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="useCursor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void ActionData(string storeName, ref string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new GenericRepository(unitOfWork);
                try
                {
                    repo.ExecuteSqlCommand(storeName, ref status);
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                }
            }
        }
        /// <summary>
        /// [Chuc.Nguyen] - Hàm dùng chung cho thực hiện lệnh sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="useCursor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void ActionData(string storeName, int commandTimeOut, ref string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                context.Database.CommandTimeout = commandTimeOut;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new GenericRepository(unitOfWork);
                try
                {
                    
                    repo.ExecuteSqlCommand(storeName, ref status);
                }
                catch (Exception ex)
                {
                    status = NotificationType.Error + "," + ex.Message;
                }
            }
        }


        /// <summary>
        /// /// [Chuc.Nguyen] - Hàm dùng chung cho thực hiện lệnh theo list para có sử dụng hay ko sử dụng cursor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="useCursor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> ActionData<TEntity>(List<object> listParameter, string storeName, bool useCursor, string userLogin, ref string status) where TEntity : class
        {
            using (var context = new VnrHrmDataContext())
            {
                List<TEntity> listEntity = new List<TEntity>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<TEntity>(unitOfWork);
                try
                {
                    listEntity = repo.ExecuteQuery(listParameter, storeName, useCursor, userLogin, ref status).ToList();
                }
                catch (Exception ex)
                {
                    listEntity = null;
                    status = NotificationType.Error + "," + ex.Message;
                }

                return listEntity;
            }
        }
        /// <summary>
        /// Danh sách các điều kiện do người dùng truyền vào
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractAdvanceFilterAttributes(object model)
        {
            List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
            if (model == null)
                return list;

            PropertyInfo[] propertyInfos = model.GetType().GetProperties();
            List<PropertyInfo> lstPropertyInfo = propertyInfos.ToList();

            foreach (PropertyInfo _profertyInfo in lstPropertyInfo)
            {
                HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                {
                    Member = _profertyInfo.Name,
                    MemberType = _profertyInfo.PropertyType,
                    Value2 = model.GetPropertyValue(_profertyInfo.Name)

                };
                if (_profertyInfo.PropertyType.Name == "List`1")
                {
                    attribute.MemberType = typeof(object);
                    var lstObj = (model.GetPropertyValue(_profertyInfo.Name) as IList);
                    object result = null;
                    if (lstObj != null)
                        result = string.Join(",", lstObj.OfType<object>().Select(x => x.ToString()).ToArray());
                    attribute.Value2 = result;
                }
                else if (_profertyInfo.PropertyType == typeof(DateTime))
                {
                    attribute.MemberType = typeof(DateTime);
                    if (attribute.Value2 != null && attribute.Value2.ToString() == DateTime.MinValue.ToString())
                    {
                        attribute.Value2 = null;
                    }
                }

                list.Add(attribute);
            }
            return list;
        }
        #endregion

        #region Get object by name
        public object GetById(Guid id, string strObjectName)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                Type entityType = strObjectName.GetEntityType(typeof(Att_Roster).Assembly);
                var data = unitOfWork.CreateQueryable(Guid.Empty, entityType, "ID = @0", id).FirstOrDefault();

                return data;
            }
        }
        #endregion

        /// <summary>
        /// [Chuc.Nguyen] - Hàm dùng chung cho thực hiện lệnh sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listParameter"></param>
        /// <param name="storeName"></param>
        /// <param name="useCursor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //public void ActionData(string storeName, ref string status)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new GenericRepository(unitOfWork);
        //        try
        //        {
        //            repo.ExecuteSqlCommand(storeName, ref status);
        //        }
        //        catch (Exception ex)
        //        {
        //            status = NotificationType.Error + "," + ex.Message;
        //        }
        //    }
        //}

        public string GetConnectionString()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                return unitOfWork.Context.Database.Connection.ConnectionString;
            }

        }

    }
}
