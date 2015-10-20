using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VnResource.Helper.Data;
using Kendo.Mvc.Extensions;
using System.Data;
using Kbc.DataHelpers;
using Kbc.QueryBuilder;

namespace HRM.Presentation.Service
{
    public class ActionService
    {
        //public string UserLogin
        //{
        //    get
        //    {
        //        if (Request != null && Request.Headers != null)
        //        {
        //            string[] headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
        //            return headerValues.FirstOrDefault();
        //        }
        //        return string.Empty;
        //    }
        //}

        private string UserLogin { get; set; }

        public ActionService(string _UserLogin)
        {
            this.UserLogin = _UserLogin;
        }

        /// <summary>
        /// Gán lại các dữ liệu kiểu ? và ko ? sau khi dùng hàm copydata khi lưu
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        public void ReSetDataSave<TEntity, TModel>(ref TEntity entity, ref TModel model)
        {
            if (entity != null && model != null)
            {
                var typePropertiesModel = model.GetType().GetProperties();
                foreach (var item in typePropertiesModel)
                {
                    if (item.PropertyType == typeof(DateTime?))
                    {
                        var date = model.GetPropertyValue(item.Name);
                        if (date != null && (DateTime)date != DateTime.MinValue)
                        {
                            entity.SetPropertyValue(item.Name, date);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gán lại các dữ liệu kiểu ? và ko ? sau khi dùng hàm copydata khi lấy dữ liệu
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        public void ReSetDataGet<TEntity, TModel>(ref TEntity entity, ref TModel model)
        {
            if (entity != null && model != null)
            {
                var typePropertiesEntity = entity.GetType().GetProperties();
                foreach (var item in typePropertiesEntity)
                {
                    var propertyType = model.GetPropertyType(item.Name);
                    if (propertyType == typeof(DateTime?))
                    {
                        var date = entity.GetPropertyValue(item.Name);
                        if (date != null && (DateTime)date != DateTime.MinValue)
                        {
                            model.SetPropertyValue(item.Name, date);
                        }
                    }
                }
            }
        }
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
            var service = new BaseService();
            return service.ActionData<TEntity>(listParameter, storeName, false, UserLogin, ref status);
        }
        public TModel UpdateOrCreate<TEntity, TModel>(TModel model) where TEntity : class, new()
        {
            return UpdateOrCreate<TEntity, TModel>(model, Guid.Empty);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý tạo mới hoặc chỉnh sửa cho một đối tượng
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel UpdateOrCreate<TEntity, TModel>(TModel model, Guid userID) where TEntity : class, new()
        {
            int check = 0;
            try
            {
                //ar entity = model.CopyData<TEntity>();

                var entity = new TEntity();
                foreach (var item in model.GetType().GetProperties())
                {
                    entity.SetPropertyValue(item.Name, model.GetPropertyValue(item.Name));
                }
                    
                //ReSetDataSave<TEntity, TModel>(ref entity, ref model);
                var service = new BaseService();
                if (model.GetPropertyValue(Constant.ID).TryGetValue<Guid>() != Guid.Empty)
                {
                    entity.SetPropertyValue(Constant.ID, model.GetPropertyValue(Constant.ID).TryGetValue<Guid>());

                    var status = service.Edit(entity, userID);
                    check = 1;
                    model.SetPropertyValue(Constant.ActionStatus, status);
                }
                else
                {
                    var status = service.Add(entity, userID);
                    model.SetPropertyValue(Constant.ID, entity.GetPropertyValue(Constant.ID));
                    model.SetPropertyValue(Constant.ActionStatus, status);
                    if (status == NotificationType.Locked.ToString())
                    {
                        model.SetPropertyValue(Constant.ID, Guid.Empty);
                    }
                }

            }
            catch (Exception ex)
            {
                if (check==1)
                {
                    model.SetPropertyValue(Constant.ActionStatus,  NotificationType.Error + "," + ex.Message);
                }
                model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error + "," + ex.Message);
            }
            
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của một đối tượng sang IsDelete
        /// </summary>
        public TModel DeleteOrRemove<TEntity,TModel>(string id) where TEntity : class
        {
            var model = (TModel)typeof(TModel).CreateInstance();
            var service = new BaseService();
            var status = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var idItem = id.Split(',');
                    for (int i = 1; i < idItem.Count(); i++)
                    {
                        var idInt = Guid.Parse(idItem[i]).TryGetValue<Guid>();
                        if (idInt != Guid.Empty)
                        {
                            if (idItem[0] == DeleteType.Delete.ToString())
                            {
                                status = service.Delete<TEntity>(idInt);
                            }
                            else if (idItem[0] == DeleteType.Remove.ToString())
                            {
                                status = service.Remove<TEntity>(idInt);
                            }
                            model.SetPropertyValue(Constant.ActionStatus, status);
                        }
                    }
                }
                catch(Exception ex)
                {
                    model.SetPropertyValue(Constant.ActionStatus, NotificationType.Error.ToString() + "," + ex.Message);
                }
            }

            return model;
        }
      
        /// <summary>
        /// [Chuc.Nguyen] - Lấy tất cả dữ liệu của một đối tượng bất kỳ dùng Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<TEntity> GetAllUserEntity<TEntity>(ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetAllUseEntity<TEntity>(ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu của một đối tượng theo Id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TEntity GetById<TEntity>(Guid id, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetById<TEntity>(id, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu của một đối tượng bất kỳ theo Id dùng Store
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ID"></param>
        /// <param name="storeName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TEntity GetByIdUseStore<TEntity>(Guid id, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetFirstData<TEntity>(Common.DotNetToOracle(id.ToString()), storeName, UserLogin, ref status).CopyData<TEntity>();
        }

        #region GetData

        public TModel GetData<TEntity, TModel>(object keyword, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            var dataEntity = service.GetData<TEntity>(keyword, storeName, UserLogin, ref status).FirstOrDefault();
            var dataModel = dataEntity.CopyData<TModel>();
            ReSetDataGet<TEntity, TModel>(ref dataEntity, ref dataModel);
            return dataModel;
        }
        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu theo keyword sử dụng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, Id, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns> 
        public List<TEntity> GetData<TEntity>(object keyword, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetData<TEntity>(keyword, storeName, UserLogin, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu theo keyword sử dụng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, Id, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns> 
        public List<TEntity> GetData<TEntity>(ListQueryModel listQueryModule, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetData<TEntity>(listQueryModule, storeName, UserLogin, ref status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu theo keyword sử dụng Store
        /// </summary>
        /// <param name="keyword">Từ khóa lấy dữ liệu: có thể là ListId, Id, có thể là Text...</param>
        /// <param name="storeName">Tên store lấy dữ liệu</param>
        /// <param name="status">Trạng thái lấy thành công hoặc lỗi</param>
        /// <returns></returns> 
        public object GetFirstData<TEntity>(object keyword, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetFirstData<TEntity>(keyword, storeName, UserLogin, ref status);
        }

        public TEntity GetDataByListParameter<TEntity>(List<object> listParameter, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            var data = service.GetData<TEntity>(listParameter, storeName, UserLogin, ref status);
            if (data!=null)
            {
                return data.FirstOrDefault();
            }
            return null;
        }
        public List<TEntity> GetData<TEntity>(List<object> listParameter, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            var data = service.GetData<TEntity>(listParameter, storeName, UserLogin, ref status);
            if (data != null)
            {
                return data.ToList();
            }
            return null;
        }
        #endregion
    }
}
