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
    public class BaseController : HrmMvcController
    {

        public string UserLogin
        {
            get
            {
                if (Request != null && Request.Headers != null)
                {
                    string[] headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    if (headerValues != null && headerValues.Count()>0)
                    {
                        return headerValues.FirstOrDefault();
                    }
                }
                return string.Empty;
            }
        }
        //public BaseController(string _userLogin)
        //{
        //    this.UserLogin = _userLogin;
        //}

        //public string GetHeader(string headerName)
        //{
        //    string[] headerValues = Request.Headers.GetValues(headerName);
        //    return headerValues.FirstOrDefault();
        //}

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu của một đối tượng theo điều kiện và return
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModelSearch"></typeparam>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public ActionResult GetListDataAndReturn<TModel, TEntity, TModelSearch>([DataSourceRequest] DataSourceRequest request, TModelSearch model, string storeName) where TEntity : class
        {
            var service = new BaseService();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                UserLogin = UserLogin,
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var status = string.Empty;
            var listEntity = service.GetData<TEntity>(lstModel, storeName, UserLogin, ref status);
            var listModel = new List<TModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    var newModle = (TModel)typeof(TModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new List<TModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu dùng store
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public List<TEntity> GetListData<TEntity>(string storeName) where TEntity : class
        {
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetDataNotParam<TEntity>(storeName, UserLogin, ref status);
            if (listEntity != null)
            {
                return listEntity;
            }
            return null;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu dùng store
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetData<TModel, TEntity>(string storeName) where TEntity : class
        {
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetDataNotParam<TEntity>(storeName, UserLogin, ref status);
            if (listEntity != null)
            {
                List<TModel> listModel = listEntity.Translate<TModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu dùng store
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="listObj"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetData<TModel, TEntity>(List<object> listObj, string storeName) where TEntity : class
        {
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<TEntity>(listObj, storeName, UserLogin, ref status);
            if (listEntity != null)
            {
                List<TModel> listModel = listEntity.Translate<TModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public List<TModel> GetData<TModel, TEntity>(string keyword, string storeName) where TEntity : class
        {
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<TEntity>(keyword, storeName, UserLogin, ref status);
            if (listEntity != null)
            {
                List<TModel> listModel = listEntity.Translate<TModel>();
                return listModel;
            }
            return null;
        }


        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu bảng cho control Multi, Dropdow, Combo...
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyword"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetDataForControl<TModel, TEntity>(string keyword, string storeName) where TEntity : class
        {
            if (keyword == null || keyword == " ")
                keyword = string.Empty;
            var service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<TEntity>(keyword, storeName, UserLogin, ref status);
            if (listEntity != null)
            {
                List<TModel> listModel = listEntity.Translate<TModel>();
                return Json(listModel, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy danh sách dữ liệu của một đối tượng theo điều kiện
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModelSearch"></typeparam>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public List<TModel> GetListData<TModel, TEntity, TModelSearch>([DataSourceRequest] DataSourceRequest request, TModelSearch model, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                UserLogin = UserLogin,
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var listEntity = service.GetData<TEntity>(lstModel, storeName, UserLogin, ref status);
            var listModel = new List<TModel>();

            //Tin.Nguyen.27/07/2015 enhance lại vì hok convert dc kiểu not allow null sang nullable
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    var newModle = (TModel)typeof(TModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }

                return listModel;
                // listModel = listEntity.Translate<TModel>();
            }

            return listModel;
        }


        public List<TEntity> GetListData<TEntity>(List<object> listParameter, string storeName, ref string status) where TEntity : class
        {
            var service = new BaseService();
            return service.GetData<TEntity>(listParameter, storeName, UserLogin, ref status);
        }

        public ActionResult ExportSelectedAndReturn<TEntity, TModel>(string selectedIds, string valueFields, string storeName) where TEntity : class
        {
            return ExportSelectedAndReturn<TEntity, TModel>(Guid.Empty, selectedIds, valueFields, storeName);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất các dòng dữ liệu được chọn của một đối tượng bất kỳ ra file Excel
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="selectedIds"></param>
        /// <param name="valueFields"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public ActionResult ExportSelectedAndReturn<TEntity, TModel>(Guid templateID, string selectedIds, string valueFields, string storeName) where TEntity : class
        {
            selectedIds = Common.DotNetToOracle(selectedIds);
            var service = new BaseService();
            var status = string.Empty;
            var listModel = new List<TModel>();
            var listEntity = service.GetData<TEntity>(selectedIds, storeName, UserLogin, ref status);
            if (listEntity != null && status == NotificationType.Success.ToString())
            {
                listModel = listEntity.Translate<TModel>();
                status = ExportService.Export(templateID, listModel, valueFields.Split(','), null);
            }
            return Json(status);
        }

        public ActionResult ExportSelectedAndReturnPortal<TEntity, TModel>(string selectedIds, string valueFields, string storeName) where TEntity : class
        {
            selectedIds = Common.DotNetToOracle(selectedIds);
            var service = new BaseService();
            var status = string.Empty;
            var listModel = new List<TModel>();
            var listEntity = service.GetData<TEntity>(selectedIds, storeName, UserLogin, ref status);
            if (listEntity != null && status == NotificationType.Success.ToString())
            {
                listModel = listEntity.Translate<TModel>();
                status = ExportService.ExportPortal(listModel, valueFields.Split(','));
            }
            return Json(status);
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuất tất cả dữ liệu của một đối tượng bất kỳ ra file Excel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportAllAndReturn<TEntity, TModel, TModelSearch>([DataSourceRequest] DataSourceRequest request, TModelSearch model, string storeName) where TEntity : class
        {
            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var listModel = GetListData<TModel, TEntity, TModelSearch>(request, model, storeName, ref status);
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status);
        }

        [HttpPost]
        public ActionResult ExportAllAndReturnPortal<TEntity, TModel, TModelSearch>([DataSourceRequest] DataSourceRequest request, TModelSearch model, string storeName) where TEntity : class
        {
            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var listModel = GetListData<TModel, TEntity, TModelSearch>(request, model, storeName, ref status);

            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.ExportPortal(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status);
        }

        /// <summary>
        /// Tạo dữ liệu lọc
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractFilterAttributes(DataSourceRequest request)
        {
            List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
            if (request.Filters == null || request.Filters.Count == 0)
                return list;
            if (request.Filters.FirstOrDefault().GetType() == typeof(Kendo.Mvc.FilterDescriptor))
            {
                foreach (Kendo.Mvc.FilterDescriptor filter in request.Filters)
                {
                    HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                    {
                        Member = filter.Member,
                        MemberType = filter.MemberType,
                        Value = filter.Value.ToString(),
                    };
                    switch (filter.Operator)
                    {
                        case Kendo.Mvc.FilterOperator.IsEqualTo:
                            attribute.Operator = FILTEROPERATOR.Equals;
                            break;
                        case Kendo.Mvc.FilterOperator.Contains:
                            attribute.Operator = FILTEROPERATOR.Contains;
                            break;
                        case Kendo.Mvc.FilterOperator.StartsWith:
                            attribute.Operator = FILTEROPERATOR.StartWith;
                            break;
                        case Kendo.Mvc.FilterOperator.EndsWith:
                            attribute.Operator = FILTEROPERATOR.EndWith;
                            break;
                    }
                    list.Add(attribute);
                }
            }
            else
            {
                foreach (Kendo.Mvc.CompositeFilterDescriptor item in request.Filters)
                {
                    foreach (Kendo.Mvc.FilterDescriptor filter in item.FilterDescriptors)
                    {
                        HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
                        {
                            Member = filter.Member,
                            MemberType = filter.MemberType,
                        };
                        switch (filter.Operator)
                        {
                            case Kendo.Mvc.FilterOperator.IsEqualTo:
                                attribute.Operator = FILTEROPERATOR.Equals;
                                break;
                            case Kendo.Mvc.FilterOperator.Contains:
                                attribute.Operator = FILTEROPERATOR.Contains;
                                break;
                            case Kendo.Mvc.FilterOperator.StartsWith:
                                attribute.Operator = FILTEROPERATOR.StartWith;
                                break;
                            case Kendo.Mvc.FilterOperator.EndsWith:
                                attribute.Operator = FILTEROPERATOR.EndWith;
                                break;
                        }
                        list.Add(attribute);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Tạo dữ liệu sắp xếp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<SortAttribute> ExtractSortAttributes(DataSourceRequest request)
        {
            List<SortAttribute> list = new List<SortAttribute>();
            if (request.Sorts == null)
                return list;
            foreach (var sort in request.Sorts)
            {
                SortAttribute attribute = new SortAttribute()
                {
                    Member = sort.Member,
                    SortDirection = sort.SortDirection
                };
                list.Add(attribute);
            }
            return list;
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

                //#region [Hien.Nguyen] xử lý tự động convert Guid orcl
                //Guid _tmp = new Guid();
                //if (attribute.Value2 != null && Guid.TryParse(attribute.Value2.ToString(), out _tmp) == true)
                //{
                //    attribute.Value2 = Common.DotNetToOracle(attribute.Value2.ToString());
                //} 
                //#endregion

                list.Add(attribute);
            }
            return list;
        }

        #region Export theo cách sinh mã
        public ActionResult Export<TEntity>(string selectedIds, string storeName, ConfigExport config) where TEntity : class
        {
            selectedIds = Common.DotNetToOracle(selectedIds);
            var service = new BaseService();
            var status = string.Empty;
            var listEntity = service.GetData<TEntity>(selectedIds, storeName, UserLogin, ref status);
            if (listEntity != null && listEntity.Any())
            {
                config.DataSource = listEntity;
                if (string.IsNullOrEmpty(config.VariableName))
                {
                    var str = listEntity[0].GetType().Name;
                    str = str.Substring(0, str.Length - 6) + "Model";
                    config.VariableName = str;
                }
                status = ExportService.ExportByGenCode(config);
            }

            return Json(status);
        }
        #endregion
          
        public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, string selectedIds, string constantPermission, string deleteType) where TModel : class
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, constantPermission);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(selectedIds))
            {
                selectedIds = deleteType + "," + selectedIds;
                var service = new RestServiceClient<TModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, urlService);
                var dataReturn = service.Delete(urlService, api, selectedIds);
                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm thực thi câu query
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string strQuery)
        {
            BaseService services = new BaseService();
            try
            {
                SqlHelper dataHelper = new SqlHelper(services.GetConnectionString().Replace(";multipleactiveresultsets=True;application name=EntityFramework", ""));
                QueryBuilder.DataHelper = dataHelper;
                dataHelper.CreateCommand(strQuery);
                return dataHelper.Fill();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
