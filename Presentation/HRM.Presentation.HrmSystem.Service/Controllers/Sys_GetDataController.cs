using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Business.HrmSystem.Models;
using VnResource.Helper.Setting;
using ListQueryModel = HRM.Infrastructure.Utilities.ListQueryModel;
using System;
using System.Globalization;
using HRM.Presentation.Category.Models;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using Kbc.DataHelpers;
using Kbc.QueryBuilder;
using HRM.Business.Finance.Domain;
using HRM.Business.Report.Domain;
using HRM.Business.Report.Models;
using System.Data;
using System.Xml;
using System.IO;
using FilterType = VnResource.Helper.Setting.FilterType;
using System.Collections;
using HRM.Business.Hr.Models;
using VnResource.Helper.Utility;
using HRM.Business.Attendance.Models;
using System.Text;
using HRM.Business.Library.Domain;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    /// <summary> Hỗ trợ các hàm lấy dữ liệu cho các control như: dropdownlist, combobox, mutiselect....
    ///  </summary>
    public class Sys_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        string status = string.Empty;

        public JsonResult GetEnum(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var enumName = splitText[0];
            var search = string.Empty;

            if (splitText != null && splitText.Length > 1)
            {
                search = splitText[1];
            }

            var enumType = Utilities.GetEnumType(enumName, typeof(AnnualLeaveDetailType).Assembly);

            var data = Sys_GeneralServices.GetEnumData(enumType);
            if (data != null)
            {
                var result = data.Select(item => new Sys_EnumModel()
                {
                    Text = item.Translate,
                    Value = item.Name,
                });

                if (search != null && search != string.Empty && search != "undefined")
                {
                    result = result.Where(s => s.Text.Contains(search)).ToList();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_EnumModel()));
        }

        public ActionResult GetLogTrackingList([DataSourceRequest] DataSourceRequest request, TrackingInfoModel modelSearch)
        {
            var listFilterInfo = new List<FilterFieldInfo>();
            if (modelSearch.DateUpdate != null && modelSearch.DateUpdateTo != null)
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "DateUpdate",
                    FilterValue1 = modelSearch.DateUpdate,
                    FilterValue2 = modelSearch.DateUpdateTo,
                    IsManual = true,
                    Type = FilterType.Between
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.ModuleName))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "ModuleName",
                    FilterValue1 = modelSearch.ModuleName,
                    IsManual = true,
                    MultiSelect = true,
                    Type = FilterType.Object
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.ObjectID))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "ObjectID",
                    FilterValue1 = modelSearch.ObjectID,
                    IsManual = true,
                    Type = FilterType.Text
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.UserUpdate))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "UserUpdate",
                    FilterValue1 = modelSearch.UserUpdate,
                    IsManual = true,
                    Type = FilterType.Text
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.TableName))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "DataType",
                    FilterValue1 = modelSearch.TableName,
                    IsManual = true,
                    Type = FilterType.Object
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.ObjectCode))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "ObjectCode",
                    FilterValue1 = modelSearch.ObjectCode,
                    IsManual = true,
                    Type = FilterType.Text
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.State))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "State",
                    FilterValue1 = modelSearch.State,
                    IsManual = true,
                    Type = FilterType.Object
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (!string.IsNullOrEmpty(modelSearch.ObjectName))
            {
                var fieldInfo = new FilterFieldInfo
                {
                    Name = "ObjectName",
                    FilterValue1 = modelSearch.ObjectName,
                    IsManual = true,
                    Type = FilterType.Text
                };
                listFilterInfo.Add(fieldInfo);
            }
            if (listFilterInfo.All(d => d.Name != SettingConstants.TableName))
            {
                //Khi người dùng không chọn điều kiện thì tự động filter trong 1 tháng
                if (listFilterInfo.All(d => d.Name != SettingConstants.DateUpdate))
                {
                    listFilterInfo.Add(new FilterFieldInfo
                    {
                        Name = SettingConstants.DateUpdate,
                        FilterValue1 = DateTime.Now.AddMonths(-1),
                        FilterValue2 = DateTime.Now.AddDays(1),
                        Type = FilterType.Between
                    });
                }
            }

            TrackingManager trackingManager = new TrackingManager();
            trackingManager.SettingPath = Common.GetPath(Constant.Tracking);
            var listTrackingInfo = trackingManager.GetTrackings(listFilterInfo);
            return Json(listTrackingInfo.ToDataSourceResult(request));
        }

        public ActionResult GetItemTrackingList([DataSourceRequest] DataSourceRequest request, ItemTrackingModel modelSearch)
        {
            BaseService service = new BaseService();
            var listTableName = service.GetListEntityType();
            if (listTableName != null && listTableName.Any())
            {
                var listItemTracking = listTableName.Select(d => new ItemTrackingInfo
                {
                    Name = d.Name,
                    Alias = d.Name.TranslateString()
                }).ToList();
                var itemTracking = new ItemTrackingManager();
                itemTracking.SettingPath = Common.GetPath(Constant.Settings);
                var listOldItemTracking = itemTracking.GetSettings();

                if (listOldItemTracking != null)
                {
                    listItemTracking =
                        listItemTracking.Where(d => !listOldItemTracking.Any(p => d.Name == p.Name)).ToList();
                    listOldItemTracking.ToList().ForEach(d => d.Alias = d.Name.TranslateString());
                    listItemTracking.AddRange(listOldItemTracking);
                }

                //if (!listItemTracking.Any(d => d.Name == "UserAccess"))
                //{
                //    listItemTracking.Add(new ItemTrackingInfo
                //    {
                //        Name = "UserAccess",
                //        Alias = "UserAccess".TranslateString()
                //    });
                //}

                //if (!listItemTracking.Any(d => d.Name == "ComputeWorkday"))
                //{
                //    listItemTracking.Add(new ItemTrackingInfo
                //    {
                //        Name = "ComputeWorkday",
                //        Alias = "ComputeWorkday".TranslateString()
                //    });
                //}

                //if (!listItemTracking.Any(d => d.Name == "ComputeAttendance"))
                //{
                //    listItemTracking.Add(new ItemTrackingInfo
                //    {
                //        Name = "ComputeAttendance",
                //        Alias = "ComputeAttendance".TranslateString()
                //    });
                //}
                if (modelSearch != null && !string.IsNullOrEmpty(modelSearch.Name))
                {
                    listItemTracking = listItemTracking.Where(d => d.Name == modelSearch.Name).ToList();
                }
                var listData = listItemTracking.Translate<ItemTrackingModel>();
                return Json(listData.ToDataSourceResult(request));
            }
            return Json(null);
        }
        /// <summary>
        /// [Hieu.Van] - Lấy dữ liệu asyntask cho tính công
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetLanguage([DataSourceRequest] DataSourceRequest request, Sys_UserSettingModel model)
        {
            return Json("");
        }
        /// <summary>
        /// [Hieu.Van] - Lấy dữ liệu asyntask cho tính công
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAsynTaskList([DataSourceRequest] DataSourceRequest request, Sys_AsynTaskSearchModel model)
        {
            string status = string.Empty;
            var result = GetListData<Sys_AsynTaskModel, Sys_AsynTaskEntity, Sys_AsynTaskSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_AsynTask, ref status);

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            if (result != null && result.Count > 0 && result.FirstOrDefault().TotalRow != null)
            {
                dataSourceResult.Total = result.Count() <= 0 ? 0 : (int)result.FirstOrDefault().TotalRow;
            }
            return Json(dataSourceResult);
        }

        public ActionResult ActionApprovedLockObject(List<Guid> selectedIds, string status)
        {
            //List<Guid> lstHoldSalaryIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
            var services = new Sys_LockObjectServices();
            services.Approved(selectedIds, status);
            return Json("");
        }

        /// <summary>
        /// Next Status Of Record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNextStatusOfRecord_FIN_PurchaseRequest(Guid recordID)
        {
            var service = new FIN_PurchaseRequestService();
            var get = service.GetNextStatusOfRecord(recordID);
            bool IsLastApproved = false;
            if (get == "Approved")
            {
                IsLastApproved = true;
            }
            return Json(IsLastApproved);
        }
        /// <summary>
        /// Next Status Of Record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNextStatusOfRecord_FIN_Claim(Guid recordID)
        {
            var service = new FIN_ClaimService();
            var get = service.GetNextStatusOfRecord(recordID);
            bool IsLastApproved = false;
            if (get == "Approved")
            {
                IsLastApproved = true;
            }
            return Json(IsLastApproved);
        }

        /// <summary>
        /// Next Status Of Record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNextStatusOfRecord_FIN_TravelRequest(Guid recordID)
        {
            var service = new FIN_TravelRequestService();
            var get = service.GetNextStatusOfRecord(recordID);
            bool IsLastApproved = false;
            if (get == "Approved")
            {
                IsLastApproved = true;
            }
            return Json(IsLastApproved);
        }


        #region Multi

        /// <summary>
        /// Lấy tất cả GroupName
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMultiProcessApproved(string text)
        {
            return GetDataForControl<Sys_ProcessApprovedMultiModel, Sys_ProcessApprovedMultiEntity>(text, ConstantSql.hrm_sys_sp_get_ProcessApproved_multi);
        }

        /// <summary>
        /// Lấy tất cả GroupName
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMultiGroup(string text)
        {
            return GetDataForControl<Sys_GroupMultiModel, Sys_GroupMultiEntity>(text, ConstantSql.hrm_sys_sp_get_group_multi);
        }

        public JsonResult GetMultiResource()
        {
            var service = new Sys_ResourceServices();
            var get = service.GetAllUseEntity<Sys_ResourceEntity>(ref status);
            var result = get.Select(item => new Sys_ResourceModel()
            {
                ID = item.ID,
                ResourceName = item.ResourceName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUser(string text)
        {
            return GetDataForControl<Sys_UserMultiModel, Sys_UserMultiEntity>(text, ConstantSql.hrm_sys_sp_get_user_multi);
        }

        public JsonResult GetMultIDynamicColumn()
        {
            var service = new Sys_DynamicColumnServices();
            var get = service.GetAllUseEntity<Sys_DynamicColumnEntity>(ref status);
            var result = get.Select(item => new Sys_DynamicColumnModel()
            {
                ID = item.ID,
                ColumnName = item.ColumnName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultIDynamicColumnGetByTableName(string tableName)
        {
            var service = new Sys_DynamicColumnServices();
            var get = service.GetByTableName(tableName);
            var result = get.Select(item => new Sys_DynamicColumnModel()
            {
                ID = item.ID,
                ColumnName = item.ColumnName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region User

        ///// <summary>
        ///// [Hieu.Van] Lấy thông tin sau đăng nhập
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult GetMyInformation(int userID)
        //{
        //    string status = string.Empty;
        //    var service = new DashboardServices();
        //    var get = service.GetData<DashboardInformationEntity>(userID, ConstantSql.hrm_dashboard_sp_get_InformationByUserID, ref status).FirstOrDefault();
        //    var result = new DashboardInformationModel()
        //    {
        //        ID = get.Id,
        //        ProfileName = get.ProfileName,
        //        UserName = get.UserName,
        //        EmployeeTypeName = get.EmployeeTypeName,
        //        JobTitleName = get.JobTitleName,
        //        OrgStructureName = get.OrgStructureName,
        //        WorkingPlace = get.WorkingPlace,
        //        LastLogin = get.LastLogin
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //    #region Code Cũ
        //    //var service = new DashboardServices();
        //    //var get = service.GetMyInformation(userID);
        //    //var result = new DashboardInformationModel()
        //    //{
        //    //    ID = get.ID,
        //    //    ProfileName = get.ProfileName,
        //    //    UserName = get.UserName,
        //    //    EmployeeTypeName = get.EmployeeTypeName,
        //    //    JobTitleName = get.JobTitleName,
        //    //    OrgStructureName = get.OrgStructureName,
        //    //    WorkingPlace = get.WorkingPlace,
        //    //    LastLogin = get.LastLogin

        //    //};
        //    //return Json(result, JsonRequestBehavior.AllowGet); 
        //    #endregion
        //}

        /// <summary>
        /// [Tung.Ly- Modify] - 2014/07/04
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserList([DataSourceRequest] DataSourceRequest request, Sys_UserSearchModel Model)
        {
            return GetListDataAndReturn<Sys_UserModel, Sys_UserInfoEntity, Sys_UserSearchModel>(request, Model, ConstantSql.hrm_sys_sp_get_users);

            #region code cũ

            //var service = new Sys_UserServices();
            //var modelSearch = new Sys_UserSearchModel();
            //modelSearch.UserName = Model.UserName;
            //modelSearch.UserLogin = Model.UserLogin;
            //var model = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request),
            //    AdvanceFilters = ExtractAdvanceFilterAttributes(modelSearch)
            //};

            //var result = service.Get(model).ToList().Translate<Sys_UserModel>();

            //request.Page = 1;
            //var dataSourceResult = result.ToDataSourceResult(request);
            //dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            //return Json(dataSourceResult, JsonRequestBehavior.AllowGet); 
            #endregion

        }
        [HttpPost]
        public ActionResult GetCodeObjectList([DataSourceRequest] DataSourceRequest request, Sys_CodeObjectSearchModel model)
        {
            return GetListDataAndReturn<Sys_CodeObjectModel, Sys_CodeObjectEntity, Sys_CodeObjectSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_code_object);
        }

        [HttpPost]
        public ActionResult GetCodeConfigList([DataSourceRequest] DataSourceRequest request, Sys_CodeConfigSearchModel model)
        {
            return GetListDataAndReturn<Sys_CodeConfigModel, Sys_CodeConfigEntity, Sys_CodeConfigSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_code_config);
        }


        [HttpPost]
        public ActionResult CheckDuplicate(string tableName, string columnName, string keyword, Guid ID)
        {
            var isDuplicate = false;
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var objs = new List<object>();
            var id = BitConverter.ToString(ID.ToByteArray()).Replace("-", "");

            objs.Add(keyword);
            objs.Add(columnName);
            objs.Add(ID);
            //  objs.Add(id);
            objs.Add(tableName);


            var valueReturn = service.GetData<DuplicateEntity>(objs, ConstantSql.hrm_sys_sp_get_checkDuplicate, ref status);
            if (valueReturn != null && valueReturn.Any())
            {
                var ids = valueReturn.Select(p => p.ID).ToList();
                if (ids.Any() && !ids.Contains(ID))
                {
                    isDuplicate = true;
                }
            }
            return Json(isDuplicate);
        }

        [HttpPost]
        public ActionResult CheckDuplicate_2Condition(string tableName, string columnName1, string columnName2, string keyword1, string keyword2, int ID)
        {
            var isDuplicate = false;
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(keyword1);
            objs.Add(keyword2);
            objs.Add(columnName1);
            objs.Add(columnName2);
            objs.Add(ID);
            objs.Add(tableName);

            var valueReturn = service.GetData<DuplicateEntity>(objs, ConstantSql.hrm_sys_sp_get_checkDuplicate_2Condition, ref status);
            if (valueReturn != null && valueReturn.Any())
            {
                isDuplicate = true;
            }
            return Json(isDuplicate);
        }

        [HttpPost]
        public ActionResult CheckDuplicate_BookMark(string tableName, string columnName1, string columnName2, Guid keyword1, string keyword2, Guid ID)
        {
            var isDuplicate = false;
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(keyword1);
            objs.Add(keyword2);
            objs.Add(columnName1);
            objs.Add(columnName2);
            objs.Add(ID);
            objs.Add(tableName);

            var valueReturn = service.GetData<DuplicateEntity>(objs, ConstantSql.hrm_sys_sp_get_checkDuplicate_BookMark, ref status);
            if (valueReturn != null && valueReturn.Any())
            {
                isDuplicate = true;
            }
            return Json(isDuplicate);
        }

        [HttpPost]
        public ActionResult CheckDuplicate_4Condition(string tableName, string columnName1, string columnName2, string columnName3, string columnName4, Guid keyword1, Guid keyword2, DateTime? keyword3, DateTime? keyword4, Guid ID)
        {
            var isDuplicate = false;
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(keyword1);
            objs.Add(keyword2);
            objs.Add(keyword3);
            objs.Add(keyword4);
            objs.Add(columnName1);
            objs.Add(columnName2);
            objs.Add(columnName3);
            objs.Add(columnName4);
            objs.Add(ID);
            objs.Add(tableName);

            var valueReturn = service.GetData<DuplicateEntity>(objs, ConstantSql.hrm_sys_sp_get_checkDuplicate_4Condition, ref status);
            if (valueReturn != null && valueReturn.Any())
            {
                var ids = valueReturn.Select(p => p.ID).ToList();
                if (ids.Any() && !ids.Contains(ID))
                {
                    isDuplicate = true;
                }
            }
            return Json(isDuplicate);
        }


        //[HttpPost]
        //public ActionResult CheckDuplicateUser(string loginName, int ID)
        //{
        //    var isDuplicate = false;
        //    var service = new Sys_UserServices();

        //    List<object> aa = new List<object>();

        //    GetData<

        //    var isDuplicateUser = service.IsDuplicationUser(loginName, ID);
        //    if (isDuplicateUser)
        //    {
        //        isDuplicate = true;
        //    }
        //    return Json(isDuplicate);
        //}

        #region Change Password
        [HttpPost]
        public ActionResult ChangePasswordSys_User([DataSourceRequest] DataSourceRequest request, Sys_UserChangePasswordModel model)
        {
            var service = new Sys_UserServices();
            var result = false;
            if (model != null && model.ID != Guid.Empty && !string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                result = service.ChangePassword(model.ID, model.OldPassword, model.NewPassword);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Reset Password
        [HttpPost]
        public ActionResult ResetPasswordByListUsers([DataSourceRequest] DataSourceRequest request, List<Guid> userIds)
        {
            var service = new Sys_UserServices();
            var result = new List<UserPasswordResetModel>();
            if (userIds != null && userIds.Any())
            {
                result = service.ResetPassword(userIds).Translate<UserPasswordResetModel>().ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Group

        public JsonResult GetGroup()
        {
            var service = new Sys_GroupServices();
            var get = service.GetAllUseEntity<Sys_GroupEntity>(ref status);
            var result = get.Select(item => new Sys_GroupModel()
            {
                ID = item.ID,
                GroupName = item.GroupName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);

            #region code cũ
            //var service = new Sys_GroupServices();
            //var get = service.Get();
            //var result = get.Select(item => new Sys_GroupModel()
            //{
            //    ID = item.ID,
            //    GroupName = item.GroupName,
            //});
            //return Json(result, JsonRequestBehavior.AllowGet); 
            #endregion
        }

        /// <summary> Lấy dữ liệu load lên lưới bằng store
        /// [Thong.Huynh] - 2014/06/02 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGroupList([DataSourceRequest] DataSourceRequest request, Sys_GroupSearchModel Sys_GroupSearchModel)
        {
            return GetListDataAndReturn<Sys_GroupModel, Sys_GroupEntity, Sys_GroupSearchModel>(request, Sys_GroupSearchModel, ConstantSql.hrm_sys_sp_get_groups);
        }

        /// <summary> Lấy dữ liệu load lên lưới bằng store
        /// [Thong.Huynh] - 2014/06/02
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGroupUserList([DataSourceRequest] DataSourceRequest request, Sys_GroupUserModel Model)
        {
            //var service = new Sys_GroupEntityUserServices();
            //var result = service.GetData<Sys_GroupEntityUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUser, ref status).ToList().Translate<Sys_GroupUserModel>();
            //return Json(result.ToDataSourceResult(request));
            return null;
            #region code cũ
            //var service = new Sys_GroupEntityUserServices();

            //ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request)
            //};
            //var result = service.Get(lstModel).ToList().Translate<Sys_GroupUserModel>();

            //return Json(result.ToDataSourceResult(request)); 
            #endregion
        }

        #endregion

        #region DataPermission

        /// <summary>
        /// [Thong.Huynh] - 2014/06/02
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGroupPermissionList([DataSourceRequest] DataSourceRequest request, Sys_GroupPermissionModel model)
        {
            var serviceGroupPermission = new ActionService(UserLogin);
            var serviceResource = new Sys_ResourceServices();

            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            var listGroupPermission = serviceGroupPermission.GetData<Sys_GroupPermission2Entity>(ConstantSql.hrm_sys_sp_get_GroupPermission, UserLogin, ref status).Translate<Sys_GroupPermissionModel>();

            List<Guid> listResourceID = listGroupPermission.Select(d => d.ResourceID).Distinct().ToList();

            var resourceQueryable = serviceResource.GetAllUseEntity<Sys_ResourceEntity>(ref status);

            if (listResourceID.Count() > 0)
            {
                resourceQueryable = resourceQueryable.Where(d => !listResourceID.Contains(d.ID)).ToList();
            }

            listGroupPermission.AddRange(resourceQueryable.Select(d => new Sys_GroupPermissionModel
            {
                ResourceID = d.ID,
                ModuleName = d.ModuleName,
                ResourceType = d.ResourceType,
                ResourceName = d.ResourceName
            }).ToList());

            return Json(listGroupPermission.ToDataSourceResult(request));


            #region code cũ
            //var serviceGroupPermission = new Sys_GroupPermissionServices();
            //var serviceResource = new Sys_ResourceServices();

            //ListQueryModel lstModel = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request)
            //};
            //var listGroupPermission = serviceGroupPermission.GetGroupPermissions().Translate<Sys_GroupPermissionModel>();
            //List<int> listResourceID = listGroupPermission.Select(d => d.ResourceID).Distinct().ToList();

            //var resourceQueryable = serviceResource.Get().ToList();

            //if (listResourceID.Count() > 0)
            //{
            //    resourceQueryable = resourceQueryable.Where(d => !listResourceID.Contains(d.ID)).ToList();
            //}

            //listGroupPermission.AddRange(resourceQueryable.Select(d => new Sys_GroupPermissionModel
            //{
            //    ResourceID = d.ID,
            //    ModuleName = d.ModuleName,
            //    ResourceType = d.ResourceType,
            //    ResourceName = d.ResourceName
            //}).ToList());

            //return Json(listGroupPermission.ToDataSourceResult(request)); 
            #endregion
        }

        /// <summary> Lấy danh sách quyền bởi userID </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult GetDataPermissionByUserID([DataSourceRequest] DataSourceRequest request, Guid userID)
        {
            var service = new Sys_DataPermissionServices();
            var dataPermissionByuserID = service.GetByUserId(userID, UserLogin);
            var result = dataPermissionByuserID.Select(item => new Sys_DataPermissionModel
            {
                ID = item.ID,
                GroupName = item.GroupName,
                Code = item.Code,
                GroupID = item.GroupID,
                DataGroup = item.DataGroup,
                DataGroups = item.DataGroups,
                UserID = item.UserID,
                UserName = item.UserName,
                Branches = item.Branches.ToNumbers(),
                BranchesName = item.BranchesName,
                OrgStructure = item.OrgStructure,
            });
            return Json(result.ToDataSourceResult(request));
        }


        #endregion

        #region Resource

        public JsonResult GetResource()
        {
            var service = new Sys_ResourceServices();
            var get = service.GetAllUseEntity<Sys_ResourceEntity>(ref status);
            var result = get.Select(item => new Sys_ResourceModel()
            {
                ID = item.ID,
                ResourceName = item.ResourceName,
            });
            return Json(result, JsonRequestBehavior.AllowGet);

            #region code cũ
            //var service = new Sys_ResourceServices();
            //var get = service.Get();
            //var result = get.Select(item => new Sys_ResourceModel()
            //{
            //    ID = item.ID,
            //    ResourceName = item.ResourceName,
            //});
            //return Json(result, JsonRequestBehavior.AllowGet); 
            #endregion
        }

        /// <summary>
        /// [Chuc.Nguyen] - 2014/06/03
        /// Lấy dữ liệu load lên lưới bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetResourceList([DataSourceRequest] DataSourceRequest request)
        {
            //GetAllUseEntity<Lau_Locker>(ref status);
            var service = new Sys_ResourceServices();
            var result = service.GetAllUseEntity<Sys_ResourceEntity>(ref status).ToList().Translate<Sys_ResourceModel>();
            return Json(result.ToDataSourceResult(request));

            #region code cũ
            //var service = new Sys_ResourceServices();
            //var result = service.Get().ToList().Translate<Sys_ResourceModel>();
            //return Json(result.ToDataSourceResult(request)); 
            #endregion
        }

        [HttpPost]
        public ActionResult UpdateResourceList([DataSourceRequest] DataSourceRequest request)
        {
            /*
            *  Goal(Cập nhật tài nguyên)
            *  Steps :
            *      Step1    :  Lấy Ds tên các bảng trong DB
            *      Step2    :  Từ tên bảng phân tích được tên các module
            *      Step3    :  Tạo Resources List
            *      Step3.1  :    - Thêm DS các module vào resources (ResourceType là Module) [Từ 3 ký tự đầu tên bảng phân tích thành tên module]
            *      Step3.2  :    - Thêm DS các bảng trong db vào resources (ResourceType là Metadata)
            *      Step3.2  :    - Thêm DS OtherResource (dùng cho Enum [Report,nút,tên màn hình không giống tên bảng])
            *      Step4    :  Cập Nhật Resource list vào bảng Sys_Resource
            */


            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = 100000,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(null)
            };

            #region Lay ds bảng
            var sysGroupPermission = new ActionService(UserLogin);
            List<object> lstParam = new List<object>();
            lstParam.Add(null);
            lstParam.Add(1);
            lstParam.Add(int.MaxValue - 1);
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
            {
                lstParam.Add(null);
            }
            var tables = sysGroupPermission.GetData<Sys_TablesMultiEntity>(lstParam, ConstantSql.hrm_cat_sp_get_tables, ref status);
            #endregion

            List<Sys_ResourceEntity> lstResources = new List<Sys_ResourceEntity>();

            var service = new Sys_ResourceServices();
            var listResourceName = service.GetAllResourceName();

            #region Thêm Ds Modules vào Resource
            //lay 3 ký tự đầu của các bảng => Att,Sys,Hre,Eva,.....
            var modules = tables.Where(p => p.name.Length > 4).Select(p => p.name.Substring(0, 3)).Distinct().ToList();
            //Trường Hợp đặc biệt. Hrd : dùng cho phân quyền tab 
            modules.Add(HRM.Infrastructure.Utilities.ModuleKey.Hrd.ToString());

            //Thêm ds các module vào resource
            foreach (var moduleType in modules)
            {
                Sys_ResourceEntity resourceModule = new Sys_ResourceEntity
                {
                    ResourceName = Common.GetModuleName(moduleType, false),
                    ResourceType = HRM.Infrastructure.Utilities.ResourceType.Module.ToString(),
                    Category = HRM.Infrastructure.Utilities.ResourceType.Module.ToString()
                };
                lstResources.Add(resourceModule);
            }
            #endregion

            #region Thêm Ds table vào resoruce
            //Thêm ds các bảng vào resource
            foreach (var sysTablesMultiEntity in tables)
            {
                if (sysTablesMultiEntity != null && sysTablesMultiEntity.name.Length > 4)
                {
                    Sys_ResourceEntity resource = new Sys_ResourceEntity
                    {
                        ResourceName = sysTablesMultiEntity.name,
                        ResourceType = HRM.Infrastructure.Utilities.ResourceType.Metadata.ToString(),
                        Category = Common.GetModuleName(sysTablesMultiEntity.name, false)
                    };
                    lstResources.Add(resource);
                }
            }
            #endregion

            #region Others
            string[] listOtherResource = typeof(OtherResource).GetEnumNames();
            if (listOtherResource != null && listOtherResource.Count() > 0)
            {
                //Enum OtherResource:phân biệt ResourceName và ModuleName qua dấu '__'
                //Attendance__Abc,//trong đó Attendance là moduleName , Abc là ResoruceName
                listOtherResource = listOtherResource.Where(d => !string.IsNullOrWhiteSpace(d)
                    && !listResourceName.Contains(d.Split(new string[1] { "__" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault())).ToArray();

                lstResources.AddRange(listOtherResource.Select(d =>
                    new Sys_ResourceEntity
                    {
                        ResourceName = d.Split(new string[1] { "__" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault(),
                        ModuleName = d.Split(new string[1] { "__" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),
                        ResourceType = HRM.Infrastructure.Utilities.ResourceType.Other.ToString()
                    }).ToList());

                listResourceName.AddRange(lstResources.Select(d =>
                    d.ResourceName).ToList());
            }
            #endregion

            //update lai bang Sys_ResourceEntity
            var isSuccess = service.UpdateResource(lstResources);

            return Json(listResourceName.ToDataSourceResult(request));

        }

        #endregion

        #region General
        //private List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractAdvanceFilterAttributes(object model)
        //{
        //    List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
        //    if (model == null)
        //        return list;

        //    PropertyInfo[] propertyInfos = model.GetType().GetProperties();
        //    List<PropertyInfo> lstPropertyInfo = propertyInfos.ToList();

        //    foreach (PropertyInfo _profertyInfo in lstPropertyInfo)
        //    {
        //        HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
        //        {
        //            Member = _profertyInfo.Name,
        //            MemberType = _profertyInfo.PropertyType,
        //            Value2 = model.GetPropertyValue(_profertyInfo.Name)

        //        };
        //        if (_profertyInfo.PropertyType.Name == "List`1")
        //        {
        //            attribute.MemberType = typeof(object);
        //            var lstObj = (model.GetPropertyValue(_profertyInfo.Name) as IList);
        //            object result = null;
        //            if (lstObj != null)
        //                result = string.Join(",", lstObj.OfType<object>().Select(x => x.ToString()).ToArray());
        //            attribute.Value2 = result;
        //        }
        //        else if (_profertyInfo.PropertyType == typeof(DateTime))
        //        {
        //            attribute.MemberType = typeof(DateTime);
        //            if (attribute.Value2 != null && attribute.Value2.ToString() == DateTime.MinValue.ToString())
        //            {
        //                attribute.Value2 = null;
        //            }
        //        }

        //        list.Add(attribute);
        //    }
        //    return list;
        //}
        //private List<SortAttribute> ExtractSortAttributes(DataSourceRequest request)
        //{
        //    List<SortAttribute> list = new List<SortAttribute>();
        //    if (request.Sorts == null)
        //        return list;
        //    foreach (var sort in request.Sorts)
        //    {
        //        SortAttribute attribute = new SortAttribute()
        //        {
        //            Member = sort.Member,
        //            SortDirection = sort.SortDirection
        //        };
        //        list.Add(attribute);
        //    }
        //    return list;
        //}

        //private List<HRM.Infrastructure.Utilities.FilterAttribute> ExtractFilterAttributes(DataSourceRequest request)
        //{
        //    List<HRM.Infrastructure.Utilities.FilterAttribute> list = new List<HRM.Infrastructure.Utilities.FilterAttribute>();
        //    if (request.Filters == null)
        //        return list;
        //    foreach (Kendo.Mvc.FilterDescriptor filter in request.Filters)
        //    {
        //        HRM.Infrastructure.Utilities.FilterAttribute attribute = new HRM.Infrastructure.Utilities.FilterAttribute()
        //        {
        //            Member = filter.Member,
        //            MemberType = filter.MemberType,
        //        };
        //        switch (filter.Operator)
        //        {
        //            case Kendo.Mvc.FilterOperator.IsEqualTo:
        //                attribute.Operator = FILTEROPERATOR.Equals;
        //                break;
        //            case Kendo.Mvc.FilterOperator.Contains:
        //                attribute.Operator = FILTEROPERATOR.Contains;
        //                break;
        //            case Kendo.Mvc.FilterOperator.StartsWith:
        //                attribute.Operator = FILTEROPERATOR.StartWith;
        //                break;
        //            case Kendo.Mvc.FilterOperator.EndsWith:
        //                attribute.Operator = FILTEROPERATOR.EndWith;
        //                break;
        //        }
        //        list.Add(attribute);
        //    }
        //    return list;
        //}
        #endregion

        #region Sys_ReleaseNote

        ///// <summary>
        ///// Lấy tất cả danh sách ReleaseNote bởi store 
        ///// </summary>
        ///// <returns></returns>  
        //public ActionResult GetReleaseNoteList([DataSourceRequest] DataSourceRequest request, Sys_ReleaseNoteModel releaseNoteModel)
        //{
        //    //
        //    //var service = new Sys_ReleaseNoteServices();
        //    //var modelSearch = new Sys_ReleaseNoteSearchModel
        //    //{
        //    //    Code = releaseNoteModel.Code,
        //    //    ReleaseNoteName =  releaseNoteModel.ReleaseNoteName,
        //    //   // DateRelease = releaseNoteModel.DateRelease
        //    //};
        //    //ListQueryModel model = new ListQueryModel
        //    //{
        //    //    PageIndex = request.Page,
        //    //    Filters = ExtractFilterAttributes(request),
        //    //    Sorts = ExtractSortAttributes(request),
        //    //    AdvanceFilters = ExtractAdvanceFilterAttributes(modelSearch)            
        //    //};
        //    //var result = service.GetData<Sys_ReleaseNote>(model, ConstantSql.hrm_sys_sp_get_ReleaseNote, ref status).ToList().Translate<Sys_ReleaseNoteModel>();
        //    //return Json(result.ToDataSourceResult(request));



        //    #region code cũ
        //    //var service = new Sys_ReleaseNoteServices();
        //    //var modelSearch = new Sys_ReleaseNoteSearchModel
        //    //{
        //    //    Code = releaseNoteModel.Code,
        //    //    ReleaseNoteName =  releaseNoteModel.ReleaseNoteName,
        //    //   // DateRelease = releaseNoteModel.DateRelease
        //    //};
        //    //ListQueryModel model = new ListQueryModel
        //    //{
        //    //    PageIndex = request.Page,
        //    //    Filters = ExtractFilterAttributes(request),
        //    //    Sorts = ExtractSortAttributes(request),
        //    //    AdvanceFilters = ExtractAdvanceFilterAttributes(modelSearch)
        //    //};
        //    //var result = service.Get(model).ToList().Translate<Sys_ReleaseNoteModel>();
        //    //return Json(result.ToDataSourceResult(request)); 
        //    #endregion
        //}

        #endregion

        #region All Setting

        /// <summary>
        /// Lấy cấu hình màu cho giời hạn tăng ca
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetColorConfig([DataSourceRequest] DataSourceRequest request)
        {
            string status = string.Empty;
            string _level1 = string.Empty;
            string _level2 = string.Empty;
            string _level3 = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString());
            lstParam.Add(1);
            lstParam.Add(Int32.MaxValue - 1);
            var baseService = new ActionService(UserLogin);
            var lstConfig = baseService.GetData<Sys_AllSettingEntity>(lstParam, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            var conf1 = lstConfig.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString()).FirstOrDefault();
            if (conf1 != null)
            {
                _level1 = conf1.Value1;
            }
            var conf2 = lstConfig.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString()).FirstOrDefault();
            if (conf2 != null)
            {
                _level2 = conf2.Value1;
            }
            var conf3 = lstConfig.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString()).FirstOrDefault();
            if (conf3 != null)
            {
                _level3 = conf3.Value1;
            }

            return Json(new { Level1 = _level1, Level2 = _level2, Level3 = _level3 });
        }

        /// <summary>
        /// lấy cấu hình màu nhân viên đã nghĩ việc
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetColorConfig_Profile([DataSourceRequest] DataSourceRequest request)
        {
            string status = string.Empty;
            string _level1 = "red";
            string _level2 = "yellow";
            List<object> lstParam = new List<object>();
            lstParam.Add("HRM_HRE_CONFIG_PROFILEQUIT");
            lstParam.Add(1);
            lstParam.Add(Int32.MaxValue - 1);
            var baseService = new ActionService(UserLogin);
            var lstConfig = baseService.GetData<Sys_AllSettingEntity>(lstParam, ConstantSql.hrm_sys_sp_get_AllSetting, ref status);
            var conf1 = lstConfig.Where(s => s.Name == AppConfig.HRM_HRE_CONFIG_PROFILEQUITCOLOR.ToString()).FirstOrDefault();
            var conf2 = lstConfig.Where(s => s.Name == AppConfig.HRM_HRE_CONFIG_PROFILEQUITBACKGROUNDCOLOR.ToString()).FirstOrDefault();
            if (conf1 != null)
            {
                _level1 = conf1.Value1;
            }
            if (conf2 != null)
            {
                _level2 = conf2.Value1;
            }

            return Json(new { Font = _level1, Background = _level2 });
        }

        [HttpPost]
        public ActionResult ExportSys_AllSettingList([DataSourceRequest] DataSourceRequest request, Sys_AllSettingSearchModel model)
        {
            return ExportAllAndReturn<Sys_AllSettingModel, Sys_AllSettingEntity, Sys_AllSettingSearchModel>(request, model,
               ConstantSql.hrm_sys_sp_get_AllSetting);

            #region code Cũ
            var service = new Sys_AllSettingServices();
            var listModel = service.GetAllUseEntity<Sys_AllSettingEntity>(ref status);
            if (status == NotificationType.Success.ToString() && model != null && !string.IsNullOrEmpty(model.ValueFields))
            {
                status = ExportService.Export(listModel, model.ValueFields.Split(','));
            }
            return Json(status);
            #endregion
        }

        [HttpPost]
        public ActionResult GetSys_AllSettingList([DataSourceRequest] DataSourceRequest request, Sys_AllSettingSearchModel model)
        {
            return GetListDataAndReturn<Sys_AllSettingModel, Sys_AllSettingEntity, Sys_AllSettingSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_AllSetting);
        }

        [HttpPost]
        public ActionResult GetLockObjectList([DataSourceRequest] DataSourceRequest request, Sys_LockObjectModel model)
        {
            var actService = new ActionService(UserLogin);
            var status = string.Empty;
            DateTime? DateStart = null;
            DateTime? DateEnd = null;

            var objCutOff = new List<object>();
            objCutOff.Add(null);
            objCutOff.Add(1);
            objCutOff.Add(int.MaxValue - 1);
            var lstCutOff = actService.GetData<Att_CutOffDurationEntity>(objCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, ref status).ToList();

            List<int> orderNumber = new List<int>();
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                orderNumber = model.OrgStructureID.Split(',').Select(s => int.Parse(s)).ToList();
            }
            var strOrg = string.Empty;
            if (orderNumber.Count > 0)
            {
                strOrg = string.Join(",", orderNumber);
            }

            if (model.CutOffDurationID != null)
            {
                Att_CutOffDurationEntity CutoffDurationItem = lstCutOff.FirstOrDefault(m => m.ID == (Guid)model.CutOffDurationID);
                DateStart = CutoffDurationItem.DateStart;
                DateEnd = CutoffDurationItem.DateEnd;
            }

            var objLockObject = new List<object>();
            objLockObject.Add(DateStart);
            objLockObject.Add(DateEnd);
            objLockObject.Add(model.Type);

            objLockObject.Add(1);
            objLockObject.Add(int.MaxValue - 1);
            var lstEntity = actService.GetData<Sys_LockObjectEntity>(objLockObject, ConstantSql.hrm_sys_sp_get_LockObject, ref status).ToList();

            

            if(model.WorkPlaceID != null)
            {
                lstEntity = lstEntity.Where(s => s.WorkPlaceID != null && s.WorkPlaceID.Value == model.WorkPlaceID.Value).ToList();
            }

            //if (orderNumber.Count >0)
            //{
            //    lstEntity = lstEntity.Where(s => s.OrgStructures != null && s.OrgStructures == Common.ListNumbersToBinary(orderNumber)).ToList();
            //}
            var lstLockObject = new List<Sys_LockObjectModel>();

            var objOrg = new List<object>();
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(1);
            objOrg.Add(int.MaxValue - 1);
            var lstOrg = actService.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            foreach (var item in lstEntity)
            {
                var strOrderNumber = string.Empty;
                var lockObjectModel = item.CopyData<Sys_LockObjectModel>();
                var cutOfEntity = lstCutOff.Where(s => s.DateStart == lockObjectModel.DateStart && s.DateEnd == lockObjectModel.DateEnd).FirstOrDefault();
                if (cutOfEntity != null)
                {
                    lockObjectModel.CutOffDurationName = cutOfEntity.CutOffDurationName;
                }
                if (item.OrgStructures != null)
                {
                   
                    lockObjectModel.lstOrgStructureID = Common.GetListNumbersFromBinary(item.OrgStructures);
                
                    if (lockObjectModel.lstOrgStructureID.Count > 0)
                    {
                        strOrderNumber = string.Join(",", lockObjectModel.lstOrgStructureID);
                        var orgName = string.Empty;
                        var lstOrgByOrderNumber = lstOrg.Where(s => lockObjectModel.lstOrgStructureID.Contains(s.OrderNumber)).ToList();
                        foreach (var org in lstOrgByOrderNumber)
                        {
                            orgName += org.OrgStructureName + ",";
                        }
                        if (!string.IsNullOrEmpty(orgName))
                        {
                            lockObjectModel.OrgStructureName = orgName.Substring(0, orgName.Length - 1);

                        }

                    }
                }
                if (!string.IsNullOrEmpty(strOrderNumber) && !string.IsNullOrEmpty(strOrg))
                {
                    if (strOrderNumber == strOrg)
                    {
                        lstLockObject.Add(lockObjectModel);
                    }
                }
                else {
                    lstLockObject.Add(lockObjectModel);
                
                }
            }
            
                DataTable tb = new DataTable("Sys_LockObjectModel");

            if (lstLockObject.Count > 0)
            {
                tb.Columns.Add("CutOffDurationName");
                tb.Columns.Add("OrgStructureName");
                tb.Columns.Add("WorkPlaceName");
                tb.Columns.Add("StatusView");
                tb.Columns.Add("UserNameCreate");
                var lstItem = new List<Sys_LockObjectItemEntity>();
                var itemServices = new Sys_LockObjectServices();
                lstItem = itemServices.GetDataNotParam<Sys_LockObjectItemEntity>(ConstantSql.Hrm_SYS_SP_GET_LOCKOBJECTITEM, UserLogin, ref status);

                foreach (var item in lstLockObject)
                {
                    var lstItemByObjectID = lstItem.Where(s =>item.ID == s.LockObjectID).ToList();

                    DataRow dr = tb.NewRow();
                    dr["CutOffDurationName"] = item.CutOffDurationName ;
                    dr["OrgStructureName"] = item.OrgStructureName;
                    dr["WorkPlaceName"] = item.WorkPlaceName;
                    dr["StatusView"] = item.StatusView;
                    dr["UserNameCreate"] = item.UserNameCreate;
                    foreach (var objectItem in lstItemByObjectID)
                    {
                        if(!tb.Columns.Contains(objectItem.ObjectName))
                        {
                            tb.Columns.Add(objectItem.ObjectName);
                        }
                        if(tb.Columns.Contains(objectItem.ObjectName))
                        {
                            dr[objectItem.ObjectName] = objectItem.ObjectName;
                        }
                    }
                    tb.Rows.Add(dr);
                    

                }
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = tb,
                    FileName = "Sys_LockObjectModel",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = true
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, tb,null, model.ExportType);

                return Json(fullPath);
            }

            return Json(lstLockObject.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //   return GetListDataAndReturn<Sys_LockObjectModel, Sys_LockObjectEntity, Sys_LockObjectSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_LockObject);
        }


        [HttpPost]
        public ActionResult ExportAllLockObjectList([DataSourceRequest] DataSourceRequest request, Sys_LockObjectModel model)
        {
            var services = new BaseService();
            var actService = new ActionService(UserLogin);
            var ActionServices = new ActionService(UserLogin);
            var status = string.Empty;
            DateTime? DateStart = null;
            DateTime? DateEnd = null;

            var objCutOff = new List<object>();
            objCutOff.Add(null);
            objCutOff.Add(1);
            objCutOff.Add(int.MaxValue - 1);
            var lstCutOff = actService.GetData<Att_CutOffDurationEntity>(objCutOff, ConstantSql.hrm_att_sp_get_CutOffDurations, ref status).ToList();

            List<int> orderNumber = new List<int>();
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                orderNumber = model.OrgStructureID.Split(',').Select(s => int.Parse(s)).ToList();
            }
            var strOrg = string.Empty;
            if (orderNumber.Count > 0)
            {
                strOrg = string.Join(",", orderNumber);
            }

            if (model.CutOffDurationID != null)
            {
                Att_CutOffDurationEntity CutoffDurationItem = lstCutOff.FirstOrDefault(m => m.ID == (Guid)model.CutOffDurationID);
                DateStart = CutoffDurationItem.DateStart;
                DateEnd = CutoffDurationItem.DateEnd;
            }

            var objLockObject = new List<object>();
            objLockObject.Add(DateStart);
            objLockObject.Add(DateEnd);
            objLockObject.Add(model.Type);

            objLockObject.Add(1);
            objLockObject.Add(int.MaxValue - 1);
            var lstEntity = actService.GetData<Sys_LockObjectEntity>(objLockObject, ConstantSql.hrm_sys_sp_get_LockObject, ref status).ToList();
            if (model.WorkPlaceID != null)
            {
                lstEntity = lstEntity.Where(s => s.WorkPlaceID != null && s.WorkPlaceID.Value == model.WorkPlaceID.Value).ToList();
            }

            //if (orderNumber.Count >0)
            //{
            //    lstEntity = lstEntity.Where(s => s.OrgStructures != null && s.OrgStructures == Common.ListNumbersToBinary(orderNumber)).ToList();
            //}
            var lstLockObject = new List<Sys_LockObjectModel>();

            var objOrg = new List<object>();
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(null);
            objOrg.Add(1);
            objOrg.Add(int.MaxValue - 1);
            var lstOrg = actService.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            foreach (var item in lstEntity)
            {
                var strOrderNumber = string.Empty;
                var lockObjectModel = item.CopyData<Sys_LockObjectModel>();
                var cutOfEntity = lstCutOff.Where(s => s.DateStart == lockObjectModel.DateStart && s.DateEnd == lockObjectModel.DateEnd).FirstOrDefault();
                if (cutOfEntity != null)
                {
                    lockObjectModel.CutOffDurationName = cutOfEntity.CutOffDurationName;
                }
                if (item.OrgStructures != null)
                {

                    lockObjectModel.lstOrgStructureID = Common.GetListNumbersFromBinary(item.OrgStructures);

                    if (lockObjectModel.lstOrgStructureID.Count > 0)
                    {
                        strOrderNumber = string.Join(",", lockObjectModel.lstOrgStructureID);
                        var orgName = string.Empty;
                        var lstOrgByOrderNumber = lstOrg.Where(s => lockObjectModel.lstOrgStructureID.Contains(s.OrderNumber)).ToList();
                        foreach (var org in lstOrgByOrderNumber)
                        {
                            orgName += org.OrgStructureName + ",";
                        }
                        if (!string.IsNullOrEmpty(orgName))
                        {
                            lockObjectModel.OrgStructureName = orgName.Substring(0, orgName.Length - 1);

                        }

                    }
                }
                if (!string.IsNullOrEmpty(strOrderNumber) && !string.IsNullOrEmpty(strOrg))
                {
                    if (strOrderNumber == strOrg)
                    {
                        lstLockObject.Add(lockObjectModel);
                    }
                }
                else
                {
                    lstLockObject.Add(lockObjectModel);

                }
            }
            if(status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(lstLockObject, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(lstLockObject.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //   return GetListDataAndReturn<Sys_LockObjectModel, Sys_LockObjectEntity, Sys_LockObjectSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_LockObject);
        }


        [HttpPost]
        public ActionResult GetLockObjectItemByLockObjectIDList([DataSourceRequest] DataSourceRequest request, Guid? LockObjectID)
        {
            List<Sys_LockObjectItemModel> listModel = new List<Sys_LockObjectItemModel>();
            var service = new BaseService();
            var actService = new ActionService(UserLogin);
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request)
            };
            if (LockObjectID != null && LockObjectID != Guid.Empty)
            {
                var status = string.Empty;
                List<object> lstParam = new List<object>();
                lstParam.Add(LockObjectID.Value);
                lstParam.Add(1);
                lstParam.Add(Int32.MaxValue - 1);
                var listEntity = actService.GetData<Sys_LockObjectItemEntity>(lstParam, ConstantSql.hrm_sys_sp_get_LockObjectItemByLockObjectID, ref status);
                if (listEntity != null)
                {
                    request.Page = 1;
                    listModel = listEntity.Translate<Sys_LockObjectItemModel>();
                    var dataSourceResult = listModel.ToDataSourceResult(request);
                    if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                    {
                        dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                    }
                    return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(listModel.ToDataSourceResult(request));
        }
        /// <summary>
        /// [Hieu.Van]
        /// Lấy dateStart - dateEnd theo value2 - DateTime.Now
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult CheckSys_AllSetting(string key)
        {
            var sys_AllSetting = new Sys_AllSettingEntity();
            sys_AllSetting = LibraryService.GetSys_AllSettingByKey(key);
            string result = string.Empty;
            DateTime DateFrom = DateTime.MinValue;
            DateTime DateTo = DateTime.MinValue;
            if (sys_AllSetting != null)
            {
                DateFrom = sys_AllSetting.Value2 != null
                    ? DateTime.ParseExact(sys_AllSetting.Value2, ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString(), System.Globalization.CultureInfo.InvariantCulture)
                    : DateFrom;
                DateFrom = DateTime.ParseExact(DateFrom.ToString(ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString()), ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                DateTo = DateTime.ParseExact(DateTime.Now.ToString(ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString()), ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                return Json(new { DFrom = DateFrom, DTo = DateTo });
            }
            else
            {
                result = null;
                return Json(result);
            }
        }

        /// <summary>
        /// [Hieu.Van]
        /// Lấy dateStart - dateEnd theo value1 - value2
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult CheckSys_AllSetting_RepairSummary(string key)
        {
            var sys_AllSetting = new Sys_AllSettingEntity();
            sys_AllSetting = LibraryService.GetSys_AllSettingByKey(key);
            string result = string.Empty;
            DateTime DateFrom = DateTime.MinValue;
            DateTime DateTo = DateTime.MinValue;
            if (sys_AllSetting != null)
            {
                DateFrom = sys_AllSetting.Value1 != null
                    ? DateTime.ParseExact(sys_AllSetting.Value1, ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString(), System.Globalization.CultureInfo.InvariantCulture)
                    : DateFrom;
                DateFrom = DateTime.ParseExact(DateFrom.ToString(ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString()), ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                DateTo = sys_AllSetting.Value2 != null
                    ? DateTime.ParseExact(sys_AllSetting.Value2, ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString(), System.Globalization.CultureInfo.InvariantCulture)
                    : DateTo;
                DateTo = DateTime.ParseExact(DateTo.ToString(ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString()), ConstantFormat.HRM_Format_DayMonthYear_HoursMin_TT.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                return Json(new { DFrom = DateFrom, DTo = DateTo });
            }
            else
            {
                result = null;
                return Json(result);
            }
        }

        #endregion

        #region xử lý format ngày và get Percent

        /// <summary>
        /// [Hien.Nguyen] Lấy format ngày của server hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFormatDate(string value, bool IsTo = false)
        {

            try
            {
                DateTime result = new DateTime();
                if (value.IndexOf('-') != -1)
                {
                    if (value.Length > 10)
                    {
                        result = DateTime.ParseExact(value, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        result = DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    if (value.Length > 10)
                    {
                        result = DateTime.ParseExact(value.Replace("am", "").Replace("pm", ""), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        result = DateTime.ParseExact(value.Replace("am", "").Replace("pm", ""), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                if (IsTo == true)
                    result = result.AddDays(1).AddMilliseconds(-1);
                return Json(result.ToString());
            }
            catch
            {
                return Json(value);
            }


        }

        /// <summary>
        /// [Hien.Nguyen]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetPercentComplete(string id)
        {
            //  id = Common.DotNetToOracle(id);
            string status = string.Empty;
            var model = new Sys_AsynTaskModel();
            ActionService service = new ActionService(UserLogin);
            // var entity = service.GetData<Sys_AsynTaskEntity>(Guid.Parse(id), ConstantSql.hrm_sys_sp_get_AsynTask_ById, ref status).FirstOrDefault();
            var entity = service.GetByIdUseStore<Sys_AsynTaskEntity>(Guid.Parse(id), ConstantSql.hrm_sys_sp_get_AsynTask_ById, ref status);
            if (entity != null)
            {
                return entity.PercentComplete * 100;
            }
            return 100;
        }

        #endregion

        #region Other

        ///// <summary>
        ///// [Hieu.Van] Lấy thông tin Alerts
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult GetAlerts()
        //{
        //    var service = new DashboardServices();
        //    var get = service.GetAlerts();
        //    var result = new DashboardAlertsModel()
        //    {
        //        sumBirthDay = get.sumBirthDay,
        //        sumContract = get.sumContract,
        //        sumProbation = get.sumProbation
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        /// <summary>
        /// Lấy danh sách cột động cho các control khác grID
        /// </summary>
        /// <returns></returns>       
        //public ActionResult GetDynamicColumnList([DataSourceRequest] DataSourceRequest request, Sys_DynamicColumnEntity ctModel)
        //{
        //    var service = new Sys_DynamicColumnServices();
        //    ListQueryModel lstModel = new ListQueryModel
        //    {
        //        Filters = ExtractFilterAttributes(request),
        //        Sorts = ExtractSortAttributes(request)
        //    };
        //    var result = service.GetSys_DynamicColumn(lstModel).ToList().Translate<Sys_DynamicColumnModel>();
        //    return Json(result.ToDataSourceResult(request));           
        //}
        [HttpPost]
        public ActionResult GetDynamicColumnList([DataSourceRequest] DataSourceRequest request, Sys_DynamicColumnModel dynamicColumnModel)
        {
            Sys_DynamicColumnSearchModel modelSearch = new Sys_DynamicColumnSearchModel();
            modelSearch.ColumnName = dynamicColumnModel.ColumnName;
            return GetListDataAndReturn<Sys_DynamicColumnModel, Sys_DynamicColumnEntity, Sys_DynamicColumnSearchModel>(request, modelSearch, ConstantSql.hrm_sys_sp_get_DynamicColumn);


            #region code cũ
            //var service = new Sys_DynamicColumnServices();
            //Sys_DynamicColumnSearchModel modelSearch = new Sys_DynamicColumnSearchModel();
            //modelSearch.ColumnName = dynamicColumnModel.ColumnName;           
            //ListQueryModel model = new ListQueryModel
            //{
            //    PageIndex = request.Page,
            //    Filters = ExtractFilterAttributes(request),
            //    Sorts = ExtractSortAttributes(request),
            //    AdvanceFilters = ExtractAdvanceFilterAttributes(modelSearch)
            //};
            ////model.PageIndex = request.Page - 1;
            ////model.PageSize = request.PageSize;
            //var result = service.GetSys_DynamicColumn(model).ToList().Translate<Sys_DynamicColumnModel>();
            //return Json(result.ToDataSourceResult(request)); 
            #endregion
        }




        /// <summary>
        /// [Hieu.Van] - 2014/06/19
        /// Lấy Submenu bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubmenu(string siteName)
        {
            var service = new Sys_AllSettingServices();

            var Name = AppConfig.HRM_SYS_USERSETTING_SUBMENU.ToString();
            var keyword = siteName;
            Guid UserID = Guid.Empty;
            var result = service.GetSubmenu(Name, keyword, UserID);
            return Json(new { str = result });
        }

        /// <summary>
        /// [Tin.nguyen] - 2014/06/24
        /// Lấy Bookmark bằng store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBookmark([DataSourceRequest] DataSourceRequest request, string userID)
        {

            if (userID == Common.UserNameSystem)
            {
                return Json(new { });
            }
            //int userID = Gui.Parse(userID);
            ActionService service = new ActionService(UserLogin);
            // var service = new Sys_BookmarkServices();

            //var entity = service.GetData<Sys_BookmarkEntity>(userID, ConstantSql.hrm_can_sp_get_canteenbyid, ref status).FirstOrDefault();

            var result = service.GetData<Sys_BookmarkEntity>(Common.DotNetToOracle(userID), ConstantSql.hrm_sys_sp_get_BookmarkByUserID, ref status).ToList().Translate<Sys_BookmarkModel>();

            return Json(result, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult GetBookmarkHotKey([DataSourceRequest] DataSourceRequest request, Guid userID, string KeyCode)
        {

            //int userID = Gui.Parse(userID);
            // ActionService service = new ActionService(UserLogin);
            var service = new Sys_BookmarkServices();
            var actService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(userID);
            objs.Add(KeyCode);
            //var entity = service.GetData<Sys_BookmarkEntity>(userID, ConstantSql.hrm_can_sp_get_canteenbyid, ref status).FirstOrDefault();

            var result = actService.GetData<Sys_BookmarkEntity>(objs, ConstantSql.hrm_sys_sp_get_BookmarkByHotKey, ref status).ToList().Translate<Sys_BookmarkModel>();

            return Json(result, JsonRequestBehavior.AllowGet);


        }
        #endregion

        #region Sys_UserApprove

        /// <summary>
        /// Lấy multi cho UserApproved
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMultiUserApproved_E_ROSTER(string text)
        {
            var key = ApproveType.E_ROSTER.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUserApproved_E_OVERTIME(string text)
        {
            var key = ApproveType.E_OVERTIME.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            if (get != null)
            {
                var result = get.Select(item => new Sys_UserMultiModel()
                {
                    ID = item.ID,
                    UserInfoName = item.UserInfoName,
                    UserLogin = item.UserLogin,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserMultiModel()));
        }

        public JsonResult GetMultiUserApproved_Org_E_OVERTIME(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var orgID = Guid.Empty;
            var textSearch = string.Empty;
            var key = ApproveType.E_OVERTIME.ToString();

            if (splitText != null && splitText.Length > 1)
            {
                textSearch = splitText[0];
                orgID = new Guid(splitText[1]);
            }
            Sys_UserServices sysService = new Sys_UserServices();
            var data = sysService.ResourceUserApproved(orgID, key);
            if (data != null)
            {
                var result = data.Select(item => new Sys_UserMultiModel()
                {
                    ID = item.ID,
                    UserInfoName = item.UserInfoName,
                    UserLogin = item.UserLogin,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserMultiModel()));
        }


        public JsonResult GetMultiUserApproved_Org_E_LEAVE_DAY(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var orgID = Guid.Empty;
            var textSearch = string.Empty;
            var key = ApproveType.E_LEAVE_DAY.ToString();

            if (splitText != null && splitText.Length > 1)
            {
                textSearch = splitText[0];
                orgID = new Guid(splitText[1]);
            }
            Sys_UserServices sysService = new Sys_UserServices();
            var data = sysService.ResourceUserApproved(orgID, key);
            if (data != null)
            {
                var result = data.Select(item => new Sys_UserMultiModel()
                {
                    ID = item.ID,
                    UserInfoName = item.UserInfoName,
                    UserLogin = item.UserLogin,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserMultiModel()));
        }

        public JsonResult GetUserApproved_Multilevel_E_FIN_PurchaseRequest(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var recordID = Guid.Empty;
            var textSearch = string.Empty;

            if (splitText != null && splitText.Length > 1)
            {
                textSearch = splitText[0];
                recordID = new Guid(splitText[1]);
            }
            Sys_UserApproveServices sysService = new Sys_UserApproveServices();
            var data = sysService.GetUserApprovedForMulti_E_FIN_PurchaseRequest(recordID);

            if (data != null)
            {
                var result = data.Select(item => new Sys_UserApproveMultiModel()
                {
                    ID = item.ID,
                    UserApproveName = item.UserApproveName,
                    UserApproveID = item.UserApproveID,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserApproveMultiModel()));
        }

        public JsonResult GetUserApproved_Multilevel_E_FIN_TravelRequest(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var recordID = Guid.Empty;
            var textSearch = string.Empty;

            if (splitText != null || splitText.Length >= 1)
            {
                textSearch = splitText[0];
                recordID = new Guid(splitText[1]);
            }
            Sys_UserApproveServices sysService = new Sys_UserApproveServices();
            var data = sysService.GetUserApprovedForMulti_E_FIN_TravelRequest(recordID, UserLogin);

            if (data != null)
            {
                var result = data.Select(item => new Sys_UserApproveMultiModel()
                {
                    ID = item.ID,
                    UserApproveName = item.UserApproveName,
                    UserApproveID = item.UserApproveID,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserApproveMultiModel()));
        }

        public JsonResult GetUserApproved_Multilevel_E_FIN_ClaimRequest(string text)
        {
            var splitText = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var recordID = Guid.Empty;
            var textSearch = string.Empty;

            if (splitText != null && splitText.Length > 1)
            {
                textSearch = splitText[0];
                recordID = new Guid(splitText[1]);
            }
            Sys_UserApproveServices sysService = new Sys_UserApproveServices();
            var data = sysService.GetUserApprovedForMulti_E_FIN_ClaimRequest(recordID, UserLogin);

            if (data != null)
            {
                var result = data.Select(item => new Sys_UserApproveMultiModel()
                {
                    ID = item.ID,
                    UserApproveName = item.UserApproveName,
                    UserApproveID = item.UserApproveID,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_UserApproveMultiModel()));
        }

        [HttpPost]
        public JsonResult ProcessSendRequest_FIN_Claim(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_ClaimService sysService = new FIN_ClaimService();
            var status = sysService.ProcessSendRequest(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessSendRequest(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_PurchaseRequestService sysService = new FIN_PurchaseRequestService();
            var status = sysService.ProcessSendRequest(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessSendRequestTravel(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_TravelRequestService sysService = new FIN_TravelRequestService();
            var status = sysService.ProcessSendRequest(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessApproved(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_PurchaseRequestService sysService = new FIN_PurchaseRequestService();
            var status = sysService.ProcessApproved(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessApprovedClaim(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_ClaimService sysService = new FIN_ClaimService();
            var status = sysService.ProcessApprovedClaim(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessApprovedTravel(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_TravelRequestService sysService = new FIN_TravelRequestService();
            var status = sysService.ProcessApprovedTravelRequest(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessApprovedCashAdvance(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var userApprovedID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_CashAdvanceService sysService = new FIN_CashAdvanceService();
            var status = sysService.ProcessApprovedCashAdvance(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessReject(string host, string _loginID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var recordID = Guid.Empty;
            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_recordID, out recordID);

            FIN_PurchaseRequestService sysService = new FIN_PurchaseRequestService();
            var status = sysService.ProcessReject(host, LoginID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessRejectClaim(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var recordID = Guid.Empty;
            var userApprovedID = Guid.Empty;

            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_recordID, out recordID);
            Guid.TryParse(_userApprovedID, out userApprovedID);


            FIN_ClaimService sysService = new FIN_ClaimService();
            var status = sysService.ProcessRejectClaim(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        [HttpPost]
        public JsonResult ProcessRejectTravel(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var recordID = Guid.Empty;
            var userApprovedID = Guid.Empty;

            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_TravelRequestService sysService = new FIN_TravelRequestService();
            var status = sysService.ProcessRejectTravelRequest(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }
        [HttpPost]
        public JsonResult ProcessRejectCashAdvance(string host, string _loginID, string _userApprovedID, string _recordID, string type)
        {
            var LoginID = Guid.Empty;
            var recordID = Guid.Empty;
            var userApprovedID = Guid.Empty;

            Guid.TryParse(_loginID, out LoginID);
            Guid.TryParse(_userApprovedID, out userApprovedID);
            Guid.TryParse(_recordID, out recordID);

            FIN_CashAdvanceService sysService = new FIN_CashAdvanceService();
            var status = sysService.ProcessRejectCashAdvance(host, LoginID, userApprovedID, recordID, type);

            return Json(new { Status = status });
        }

        public JsonResult GetMultiUserApproved_E_REWARD(string text)
        {
            var key = ApproveType.E_REWARD.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUserApproved_E_DISCIPLINE(string text)
        {
            var key = ApproveType.E_DISCIPLINE.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUserApproved_E_TRA_REQUIREMENTTRAIN(string text)
        {
            var key = ApproveType.E_TRA_REQUIREMENTTRAIN.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUserApproved_E_LEAVE_DAY(string text)
        {
            var key = ApproveType.E_LEAVE_DAY.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiUserApproved_E_LOCKOBJECT(string text)
        {
            var key = ApproveType.E_LOCKOBJECT.ToString();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(text);
            lstParam.Add(key);
            var service = new ActionService(UserLogin);
            var get = service.GetData<Sys_UserInfoEntity>(lstParam, ConstantSql.hrm_sys_sp_get_userApproved_multi, ref status);
            var result = get.Select(item => new Sys_UserMultiModel()
            {
                ID = item.ID,
                UserInfoName = item.UserInfoName,
                UserLogin = item.UserLogin,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool GetPermissionApproved(string userID, string permissionCheck)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(userID, out id);
            if (id == Guid.Empty)
            {
                return false;
            }
            userID = Common.DotNetToOracle(userID);
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);

            //kiểm tra đối với những dự án không sử dụng phê duyệt thì mặc định status là approved
            var key = AppConfig.HRM_CONFIG_DEFAULTAPPROVED.ToString();
            var defaultapproved = baseService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            if (defaultapproved != null && defaultapproved.Count > 0)
            {
                var _default = defaultapproved.FirstOrDefault().Value1;
                if (_default == Boolean.TrueString)
                {
                    return true;
                }
            }

            List<object> lstSys = new List<object>();
            lstSys.Add(userID);
            lstSys.Add(permissionCheck);
            lstSys.Add(1);
            lstSys.Add(1000000);
            var config = baseService.GetData<Sys_UserApproveEntity>(lstSys, ConstantSql.hrm_sys_sp_get_UserApprove, ref status);
            if (config != null && config.Count > 0)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public ActionResult GetConditionApprovedList([DataSourceRequest] DataSourceRequest request, Sys_ConditionApprovedSearchModel model)
        {
            return GetListDataAndReturn<Sys_ConditionApprovedModel, Sys_ConditionApprovedEntity, Sys_ConditionApprovedSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_ConditionApproved);
        }

        [HttpPost]
        public ActionResult ExportAllConditionApprovedList([DataSourceRequest] DataSourceRequest request, Sys_ConditionApprovedSearchModel model)
        {
            return ExportAllAndReturn<Sys_ConditionApprovedEntity, Sys_ConditionApprovedModel, Sys_ConditionApprovedSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_ConditionApproved);
        }

        [HttpPost]
        public ActionResult ExportConditionApprovedSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sys_ConditionApprovedEntity, Sys_ConditionApprovedModel>(selectedIds, valueFields, ConstantSql.hrm_sys_sp_get_ConditionApprovedByIds);
        }

        [HttpPost]
        public ActionResult GetProcessApprovedList([DataSourceRequest] DataSourceRequest request, Sys_ProcessApprovedSearchModel model)
        {
            return GetListDataAndReturn<Sys_ProcessApprovedModel, Sys_ProcessApprovedEntity, Sys_ProcessApprovedSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_ProcessApproved);
        }

        [HttpPost]
        public ActionResult ExportAllProcessApprovedList([DataSourceRequest] DataSourceRequest request, Sys_ProcessApprovedSearchModel model)
        {
            return ExportAllAndReturn<Sys_ProcessApprovedEntity, Sys_ProcessApprovedModel, Sys_ProcessApprovedSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_ProcessApproved);
        }

        [HttpPost]
        public ActionResult ExportProcessApprovedSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sys_ProcessApprovedEntity, Sys_ProcessApprovedModel>(selectedIds, valueFields, ConstantSql.hrm_sys_sp_get_ProcessApprovedByIds);
        }


        [HttpPost]
        public ActionResult GetUserApproveList([DataSourceRequest] DataSourceRequest request, Sys_UserApproveSearchModel model)
        {
            if (model != null)
            {
                model.UserApproveID = Common.DotNetToOracle(model.UserApproveID);
            }

            return GetListDataAndReturn<Sys_UserApproveModel, Sys_UserApproveEntity, Sys_UserApproveSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_UserApprove);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ExportAllUserApproveList([DataSourceRequest] DataSourceRequest request, Sys_UserApproveSearchModel model)
        {
            return ExportAllAndReturn<Sys_UserApproveEntity, Sys_UserApproveModel, Sys_UserApproveSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_UserApprove);
        }

        public ActionResult ExportUserApproveSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Sys_UserApproveEntity, Sys_UserApproveModel>(selectedIds, valueFields, ConstantSql.hrm_sys_sp_get_UserApproveByIds);
        }

        #endregion

        #region TemplateSendMail
        [HttpPost]
        public ActionResult GetTeamPlateEmailList([DataSourceRequest] DataSourceRequest request, Sys_TemplateSendMailSearchModel model)
        {
            //if (model != null)
            //{
            //    model.UserApproveID = Common.DotNetToOracle(model.UserApproveID);
            //}
            return GetListDataAndReturn<Sys_TemplateSendMailModel, Sys_TemplateSendMailEntity, Sys_TemplateSendMailSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_TemplateSendMail);
        }
        #endregion

        #region Custom Report
        public ActionResult GetReportMaster([DataSourceRequest] DataSourceRequest request, Rep_MasterModelSearch model)
        {
            return GetListDataAndReturn<Rep_MasterModel, Rep_MasterEntity, Rep_MasterModelSearch>(request, model, ConstantSql.hrm_rep_sp_get_Master);
        }

        public JsonResult GetMultResource(string id)
        {
            var actService = new ActionService(UserLogin);
            if (id == string.Empty || id == null)
            {
                var listObj = new List<object> { 1, 1000 };
                BaseService service = new BaseService();                
                List<object> objs = new List<object>();
                objs.Add(1);
                objs.Add(Int32.MaxValue - 1);
                List<Cat_SysTablesMultiEntity> listTable = actService.GetData<Cat_SysTablesMultiEntity>(listObj, ConstantSql.hrm_cat_sp_get_tables, ref status);

                var result = from e in listTable
                             select new
                             {
                                 id = e.Name,
                                 Name = e.Name.TranslateString(),
                                 hasChildren = true,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var service = new Cat_ImportItemServices();
                List<object> objs = new List<object>();
                objs.Add(string.Empty);
                objs.Add(id);
                objs.Add(1);
                objs.Add(Int32.MaxValue - 1);
                var listTable = actService.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.FOREIGN.ToString());

                var result = from e in listTable
                             select new
                             {
                                 id = e.Name,
                                 Name = e.Name.TranslateString(),
                                 hasChildren = false,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetDisplayTable(string id)
        {
            var actService = new ActionService(UserLogin);
            if (id != string.Empty)
            {
                Guid MasterID = Guid.Empty;
                bool IsTable = Guid.TryParse(id, out MasterID);
                if (IsTable == true)
                {
                    ActionService services = new ActionService(UserLogin);
                    var MasterItem = services.GetByIdUseStore<Rep_MasterEntity>(MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);

                    var service = new Cat_ImportItemServices();
                    List<object> objs = new List<object>();
                    objs.Add(string.Empty);
                    objs.Add(MasterItem.ObjectMain);
                    objs.Add(1);
                    objs.Add(Int32.MaxValue - 1);
                    var listTable = actService.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.FOREIGN.ToString()).ToList();

                    listTable.Add(new CatChildFieldsMultiModel() { ID = MasterItem.ObjectMain, Name = MasterItem.ObjectMain });

                    var result = from e in listTable
                                 select new
                                 {
                                     id = e.Name,
                                     Name = e.Name.TranslateString(),
                                     hasChildren = true,
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var service = new Cat_ImportItemServices();
                    List<object> objs = new List<object>();
                    objs.Add(string.Empty);
                    objs.Add(id);
                    objs.Add(1);
                    objs.Add(Int32.MaxValue - 1);
                    var listField = actService.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.COLUMN.ToString() && !m.Name.EndsWith("ID")).ToList();

                    var result = from e in listField
                                 select new
                                 {
                                     id = e.ID,
                                     Name = e.Name.TranslateString(),
                                     hasChildren = false,
                                 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult GetDisplayColumn(Guid? id)
        {
            var actService = new ActionService(UserLogin);
            if (id != null)
            {
                Rep_ColumnServices ColumnServices = new Rep_ColumnServices();
                var listModel = new List<object>();
                listModel.Add(id);
                listModel.Add(1);
                listModel.Add(Int32.MaxValue - 1);
                List<Rep_ColumnEntity> ListColumn = actService.GetData<Rep_ColumnEntity>(listModel, ConstantSql.hrm_rep_sp_get_ColumnByMasterID, ref status).ToList();

                var result = from e in ListColumn
                             select new
                             {
                                 id = e.TableName + "," + e.ColumnName,
                                 Name = e.ColumnName.TranslateString(),
                                 hasChildren = false,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            return Json("");
        }

        public JsonResult GetMultResourceResult(string id)
        {
            if (id != null)
            {
                var service = new ActionService(UserLogin);
                List<object> objs = new List<object>();
                objs.Add(string.Empty);
                objs.Add(id);
                objs.Add(1);
                objs.Add(Int32.MaxValue - 1);
                var listField = service.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.COLUMN.ToString() && !m.Name.EndsWith("ID")).ToList();

                var result = from e in listField
                             select new
                             {
                                 id = e.ID,
                                 Name = e.Name,
                                 hasChildren = false,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<CatChildFieldsMultiModel> listField = new List<CatChildFieldsMultiModel>();
                var result = from e in listField
                             select new
                             {
                                 id = id,
                                 Name = e.Name,
                                 hasChildren = false,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetRep_ConditionItem([DataSourceRequest] DataSourceRequest request, Rep_ConditionSearchModel model)
        {
            //BaseService service = new BaseService();
            //List<object> objs = new List<object>();
            //objs.Add(model.MasterID);
            //objs.Add(1);
            //objs.Add(Int32.MaxValue - 1);
            //List<Rep_ConditionItemEntity> listConditionItem = service.GetData<Rep_ConditionItemEntity>(objs, ConstantSql.hrm_rep_sp_get_ConditionItem, ref status);

            //return Json(listConditionItem.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            return GetListDataAndReturn<Rep_ConditionItemModel, Rep_ConditionItemEntity, Rep_ConditionSearchModel>(request, model, ConstantSql.hrm_rep_sp_get_ConditionItem);
        }

        [HttpPost]
        public JsonResult GetMulti_Condition(Guid? MasterID)
        {
            var service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            objs.Add(MasterID);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            List<Rep_ConditionEntity> listCondition = service.GetData<Rep_ConditionEntity>(objs, ConstantSql.hrm_rep_sp_get_ConditionByMasterID, ref status).OrderByDescending(m => m.DateUpdate).ToList();
            return Json(listCondition, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMulti_Table_Enum(bool IsEnum = false)
        {
            if (IsEnum)
            {
                List<string> listName = typeof(EnumDropDown).Assembly.GetTypes().Where(d => d.IsEnum).Select(p => p.Name).ToList();
                var result = from e in listName
                             select new
                             {
                                 Name = e,
                             };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sysGroupPermission = new ActionService(UserLogin);
                List<object> objs = new List<object>();
                objs.Add(1);
                objs.Add(Int32.MaxValue - 1);
                List<Cat_SysTablesMultiEntity> listTable = sysGroupPermission.GetData<Cat_SysTablesMultiEntity>(objs, ConstantSql.hrm_cat_sp_get_tables, ref status);
                return Json(listTable, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetMulti_ConditionItem(Guid? MasterID)
        {
            if (MasterID != null && MasterID != Guid.Empty)
            {
                var service = new ActionService(UserLogin);
                List<object> objs = new List<object>();
                objs.Add(MasterID);
                objs.Add(1);
                objs.Add(Int32.MaxValue - 1);
                List<Rep_ConditionItemEntity> listCondition = service.GetData<Rep_ConditionItemEntity>(objs, ConstantSql.hrm_rep_sp_get_ConditionItemByMasterID, ref status).OrderByDescending(m => m.DateUpdate).ToList();
                return Json(listCondition, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult SaveCustomControl(Rep_ControlModel model)
        {
            ActionService services = new ActionService(UserLogin);
            var MasterItem = services.GetByIdUseStore<Rep_MasterModel>(model.MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);
            XmlDocument xml = new XmlDocument();

            //kiểm tra tồn tại của element này chưa
            if (MasterItem.ScriptParams != null && MasterItem.ScriptParams != string.Empty)//lần đầu tiên add control
            {
                xml.LoadXml(MasterItem.ScriptParams);

                XmlNodeList listNodes = xml.DocumentElement.SelectNodes("/Root/ControlModel");
                foreach (XmlNode i in listNodes)
                {
                    if (Guid.Parse(i[Rep_ControlModel.FieldNames.ConditionItemID].InnerText) == model.ConditionItemID)
                    {
                        return Json(false);
                    }
                }
            }
            xml = new XmlDocument();
            //Parent node
            XmlNode newElem0 = xml.CreateNode(XmlNodeType.Element, "ControlModel", "");

            XmlNode newElem1 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.Name, "");
            newElem1.InnerText = model.Name;
            newElem0.AppendChild(newElem1);

            XmlNode newElem2 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.ControlType, "");
            newElem2.InnerText = model.ControlType;
            newElem0.AppendChild(newElem2);

            XmlNode newElem3 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.TableName, "");
            newElem3.InnerText = model.TableName;
            newElem0.AppendChild(newElem3);

            XmlNode newElem4 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.TextField, "");
            newElem4.InnerText = model.TextField;
            newElem0.AppendChild(newElem4);

            XmlNode newElem5 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.ValueField, "");
            newElem5.InnerText = model.ValueField;
            newElem0.AppendChild(newElem5);

            XmlNode newElem6 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.IsEnum, "");
            newElem6.InnerText = model.IsEnum.ToString();
            newElem0.AppendChild(newElem6);

            XmlNode newElem7 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.ConditionItemID, "");
            newElem7.InnerText = model.ConditionItemID.ToString();
            newElem0.AppendChild(newElem7);

            XmlNode newElem8 = xml.CreateNode(XmlNodeType.Element, Rep_ControlModel.FieldNames.MasterID, "");
            newElem8.InnerText = model.MasterID.ToString();
            newElem0.AppendChild(newElem8);



            if (MasterItem.ScriptParams == null || MasterItem.ScriptParams == string.Empty)//lần đầu tiên add control
            {
                XmlNode newElem = xml.CreateNode(XmlNodeType.Element, "Root", "");
                newElem.AppendChild(newElem0);
                xml.AppendChild(newElem);
                MasterItem.ScriptParams = xml.InnerXml;
            }
            else//khi đã có controll
            {
                xml.LoadXml(MasterItem.ScriptParams);
                xml.FirstChild.AppendChild(newElem0);
                MasterItem.ScriptParams = xml.InnerXml;
            }
            services.UpdateOrCreate<Rep_MasterEntity, Rep_MasterModel>(MasterItem);
            return Json(true);
        }

        [HttpPost]
        public JsonResult GetControlForGrid(DataSourceRequest request, Guid masterID)
        {
            ActionService services = new ActionService(UserLogin);
            var MasterItem = services.GetByIdUseStore<Rep_MasterModel>(masterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);
            List<Rep_ControlModel> ListResult = new List<Rep_ControlModel>();

            XmlDocument xml = new XmlDocument();
            if (MasterItem.ScriptParams != null && MasterItem.ScriptParams != string.Empty)
            {
                xml.LoadXml(MasterItem.ScriptParams);

                XmlNodeList listNodes = xml.DocumentElement.SelectNodes("/Root/ControlModel");
                foreach (XmlNode i in listNodes)
                {
                    Rep_ControlModel item = new Rep_ControlModel();
                    item.ConditionItemID = Guid.Parse(i[Rep_ControlModel.FieldNames.ConditionItemID].InnerText);
                    item.ValueField = i[Rep_ControlModel.FieldNames.ValueField].InnerText;
                    item.TextField = i[Rep_ControlModel.FieldNames.TextField].InnerText;
                    item.TableName = i[Rep_ControlModel.FieldNames.TableName].InnerText;
                    item.IsEnum = bool.Parse(i[Rep_ControlModel.FieldNames.IsEnum].InnerText);
                    item.Name = i[Rep_ControlModel.FieldNames.Name].InnerText;
                    item.ControlType = i[Rep_ControlModel.FieldNames.ControlType].InnerText;
                    ListResult.Add(item);
                }
                return Json(ListResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
        public JsonResult GetMulti_Table(Guid? MasterID)
        {
            string status = string.Empty;
            Rep_MasterServices Masterservice = new Rep_MasterServices();
            Rep_ColumnServices ColumnServices = new Rep_ColumnServices();
            ActionService services = new ActionService(UserLogin);
            var ImportItemservice = new ActionService(UserLogin);

            var MasterItem = services.GetByIdUseStore<Rep_MasterEntity>((Guid)MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);

            List<object> objs = new List<object>();
            objs.Add(string.Empty);
            objs.Add(MasterItem.ObjectMain);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var listTable = ImportItemservice.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.FOREIGN.ToString()).ToList();

            listTable.Add(new CatChildFieldsMultiModel() { ID = MasterItem.ObjectMain, Name = MasterItem.ObjectMain });

            return new JsonResult() { Data = listTable };
        }

        [HttpPost]
        public JsonResult GetTableNameMultiple()
        {
            var sysGroupPermission = new ActionService(UserLogin);

            List<object> objs = new List<object>();
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            List<Cat_SysTablesMultiEntity> listTable = sysGroupPermission.GetData<Cat_SysTablesMultiEntity>(objs, ConstantSql.hrm_cat_sp_get_tables, ref status);

            return new JsonResult() { Data = listTable };
        }

        [HttpPost]
        public JsonResult GetMulti_Column(string ID)
        {
            var service = new ActionService(UserLogin);
            List<object> objs = new List<object>();
            objs.Add(string.Empty);
            objs.Add(ID);
            objs.Add(1);
            objs.Add(Int32.MaxValue - 1);
            var listField = service.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.COLUMN.ToString()).ToList();

            return Json(listField, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateQuery(Guid MasterID)
        {
            ActionService service = new ActionService(UserLogin);
            Cat_ImportItemServices ImportServices = new Cat_ImportItemServices();
            Rep_ColumnServices ColumnServices = new Rep_ColumnServices();
            var actService = new ActionService(UserLogin);
            Rep_ConditionServices ConditionServies = new Rep_ConditionServices();
            Rep_ConditionItemServices ConditionItemSerives = new Rep_ConditionItemServices();
            Rep_MasterServices MasterSerives = new Rep_MasterServices();

            string status = string.Empty;
            List<object> listModel = new List<object>();
            Rep_MasterEntity masterItem = service.GetByIdUseStore<Rep_MasterEntity>(MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);
            if (masterItem != null)
            {
                #region Get Data
                listModel = new List<object>();
                listModel.Add(masterItem.ID);
                listModel.Add(1);
                listModel.Add(Int32.MaxValue - 1);
                List<Rep_ColumnEntity> ListColumn = actService.GetData<Rep_ColumnEntity>(listModel, ConstantSql.hrm_rep_sp_get_ColumnByMasterID, ref status).ToList();

                listModel = new List<object>();
                listModel.Add(masterItem.ID);
                listModel.Add(1);
                listModel.Add(Int32.MaxValue - 1);
                List<Rep_ConditionEntity> ListCondition = actService.GetData<Rep_ConditionEntity>(listModel, ConstantSql.hrm_rep_sp_get_ConditionByMasterID, ref status).ToList();

                listModel = new List<object>();
                listModel.Add(masterItem.ID);
                listModel.Add(1);
                listModel.Add(Int32.MaxValue - 1);
                List<Rep_ConditionItemEntity> ListConditionItem = actService.GetData<Rep_ConditionItemEntity>(listModel, ConstantSql.hrm_rep_sp_get_ConditionItemByMasterID, ref status).ToList();
                #endregion

                QueryBuilderInfo queryBuilder = new QueryBuilderInfo();
                queryBuilder.IsAutoJoinTable = false;
                queryBuilder.MainFromTableName = masterItem.ObjectMain;
                queryBuilder.ListGroupConditionInfo = new List<GroupConditionInfo>();
                queryBuilder.ListQueryTableInfo = new List<QueryTableInfo>();
                queryBuilder.ListJoinTableInfo = new List<JoinTableInfo>();

                List<GroupConditionInfo> GroupCondition = new List<GroupConditionInfo>();
                for (int i = 0; i < ListCondition.Count; i++)
                {
                    GroupConditionInfo GroupConditionItem = new GroupConditionInfo();
                    GroupConditionItem.GroupConditionJoinType = (ConditionJoinType)Enum.Parse(typeof(ConditionJoinType), ListCondition[i].WhereType);
                    GroupConditionItem.GroupConditionOrdinal = (int)ListCondition[i].OrderNumber;
                    GroupConditionItem.ListQueryConditionInfo = new List<QueryConditionInfo>();

                    var itemCondtion = ListConditionItem.Where(m => m.JoinType == ConditionJoinType.None.ToString()).FirstOrDefault();
                    if (itemCondtion != null)
                    {
                        ListConditionItem.Remove(itemCondtion);
                        ListConditionItem.Insert(0, itemCondtion);
                    }

                    foreach (var j in ListConditionItem.Where(m => m.ConditionID == ListCondition[i].ID).ToList())
                    {
                        QueryConditionInfo ConditionItem = new QueryConditionInfo();
                        ConditionItem.ColumnName = j.ColumnName;
                        ConditionItem.ConditionOperator = (ConditionOperator)Enum.Parse(typeof(ConditionOperator), j.Operator); ;
                        ConditionItem.ConditionOrdinal = 0;
                        ConditionItem.ConditionValue1 = j.Value1;
                        ConditionItem.ConditionValue2 = j.Value2;
                        ConditionItem.TableName = j.TableName;
                        ConditionItem.ConditionJoinType = (ConditionJoinType)Enum.Parse(typeof(ConditionJoinType), j.JoinType);
                        GroupConditionItem.ListQueryConditionInfo.Add(ConditionItem);
                    }
                    queryBuilder.ListGroupConditionInfo.Add(GroupConditionItem);
                }

                #region Get Join Column
                //mở chế độ auto join lên
                queryBuilder.IsAutoJoinTable = true;

                List<string> ListTableData = new List<string>();

                if (ListColumn == null || ListColumn.Count <= 0)//Không chọn column thì select *
                {
                    //lấy table liên kết vs table chính
                    List<object> objs = new List<object>();
                    objs.Add(string.Empty);
                    objs.Add(masterItem.ObjectMain);
                    objs.Add(1);
                    objs.Add(Int32.MaxValue - 1);
                    var listfield = actService.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.FOREIGN.ToString()).ToList();

                    listfield.Add(new CatChildFieldsMultiModel() { ID = masterItem.ObjectMain, Name = masterItem.ObjectMain, Type = EnumDropDown.TypeTable.FOREIGN.ToString() });

                    ListTableData = listfield.Select(m => m.ID).Distinct().ToList();
                }
                else//có chọn column thì select theo column chọn
                {
                    ListTableData = ListColumn.Select(m => m.TableName).Distinct().ToList();
                }

                for (int i = 0; i < ListTableData.Count; i++)
                {
                    QueryTableInfo item = new QueryTableInfo();
                    item.IsActive = true;
                    item.TableName = ListTableData[i];
                    item.TableOrdinal = i + 1;
                    item.ListQueryColumnInfo = new List<QueryColumnInfo>();

                    var objs = new List<object>();
                    objs.Add(string.Empty);
                    objs.Add(ListTableData[i]);
                    objs.Add(1);
                    objs.Add(Int32.MaxValue - 1);
                    var listField = actService.GetData<CatChildFieldsMultiModel>(objs, ConstantSql.hrm_cat_sp_get_Import_childfield_multi, ref status).Where(m => m.Type == EnumDropDown.TypeTable.COLUMN.ToString()).ToList();

                    for (int j = 0; j < listField.Count; j++)
                    {
                        QueryColumnInfo columnItem = new QueryColumnInfo();
                        columnItem.AliasName = listField[j].Name;
                        columnItem.ColumnName = listField[j].Name;
                        columnItem.ColumnOrdinal = j + 1;
                        columnItem.DisplayName = listField[j].Name;
                        columnItem.IsActive = false;
                        columnItem.IsGroup = false;
                        columnItem.IsSum = false;
                        columnItem.TableName = ListTableData[i];
                        item.ListQueryColumnInfo.Add(columnItem);
                    }

                    queryBuilder.ListQueryTableInfo.Add(item);
                }

                #endregion

                #region create param
                string param = "SELECT";
                if (ListColumn == null || ListColumn.Count <= 0)
                {
                    param += " * ";
                }
                else
                {
                    foreach (var i in ListColumn)
                    {
                        if (i == ListColumn.LastOrDefault())
                        {
                            param += " [" + i.TableName + "].[" + i.ColumnName + "] ";
                        }
                        else
                        {
                            param += " [" + i.TableName + "].[" + i.ColumnName + "],";
                        }

                    }
                }
                #endregion

                SqlHelper dataHelper = new SqlHelper(MasterSerives.GetConnectionString().Replace(";multipleactiveresultsets=True;application name=EntityFramework", ""));
                QueryBuilder.DataHelper = dataHelper;
                QueryBuilder.AutoJoinTable(queryBuilder);

                string sqlQuery = QueryBuilder.BuildQuery(queryBuilder);
                masterItem.ScriptExcute = param + " " + sqlQuery;
                MasterSerives.Edit<Rep_MasterEntity>(masterItem);

                dataHelper.CreateCommand(param + " " + sqlQuery);
                dataHelper.AddParameter(QueryBuilder.BuildParameters(queryBuilder).ToArray());
                DataTable result = dataHelper.Fill();

                DataSourceRequest request = new DataSourceRequest();
                request.Page = 1;
                request.PageSize = 50;

                return new JsonResult() { Data = result.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult() { Data = new DataTable(), MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GeneralCustomReportData([Bind]Rep_MasterModel model)
        {
            ActionService service = new ActionService(UserLogin);
            Rep_ColumnServices ColumnServices = new Rep_ColumnServices();
            Rep_MasterServices MasterSerives = new Rep_MasterServices();
            Rep_ConditionServices ConditionServies = new Rep_ConditionServices();
            Rep_ConditionItemServices ConditionItemSerives = new Rep_ConditionItemServices();

            Rep_MasterEntity masterItem = service.GetByIdUseStore<Rep_MasterEntity>(model.ID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);

            List<object> listModel = new List<object>();
            listModel.Add(masterItem.ID);
            listModel.Add(1);
            listModel.Add(Int32.MaxValue - 1);
            List<Rep_ColumnEntity> ListColumn = service.GetData<Rep_ColumnEntity>(listModel, ConstantSql.hrm_rep_sp_get_ColumnByMasterID, ref status).OrderBy(m => m.ColumnOrdinal).ToList();

            listModel = new List<object>();
            listModel.Add(masterItem.ID);
            listModel.Add(1);
            listModel.Add(Int32.MaxValue - 1);
            List<Rep_ConditionEntity> ListCondition = service.GetData<Rep_ConditionEntity>(listModel, ConstantSql.hrm_rep_sp_get_ConditionByMasterID, ref status).ToList();

            listModel = new List<object>();
            listModel.Add(masterItem.ID);
            listModel.Add(1);
            listModel.Add(Int32.MaxValue - 1);
            List<Rep_ConditionItemEntity> ListConditionItem = service.GetData<Rep_ConditionItemEntity>(listModel, ConstantSql.hrm_rep_sp_get_ConditionItemByMasterID, ref status).ToList();

            QueryBuilderInfo queryBuilder = new QueryBuilderInfo();
            queryBuilder.ListQueryTableInfo = new List<QueryTableInfo>();
            queryBuilder.MainFromTableName = masterItem.ObjectMain;

            List<GroupConditionInfo> GroupCondition = new List<GroupConditionInfo>();


            if (masterItem.ScriptParams == null || masterItem.ScriptParams == string.Empty)
            {
                masterItem.ScriptParams = "<Root></Root>";
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(masterItem.ScriptParams);
            XmlNodeList listNodes = xml.DocumentElement.SelectNodes("/Root/ControlModel");

            //if (MasterItem.ScriptParams != null && MasterItem.ScriptParams != string.Empty)
            //{
            //    xml.LoadXml(MasterItem.ScriptParams);

            //    XmlNodeList listNodes = xml.DocumentElement.SelectNodes("/Root/ControlModel");
            //    foreach (XmlNode i in listNodes)

            for (int i = 0; i < ListCondition.Count; i++)
            {
                GroupConditionInfo GroupConditionItem = new GroupConditionInfo();
                GroupConditionItem.GroupConditionJoinType = (ConditionJoinType)Enum.Parse(typeof(ConditionJoinType), ListCondition[i].WhereType);
                GroupConditionItem.GroupConditionOrdinal = (int)ListCondition[i].OrderNumber;
                GroupConditionItem.ListQueryConditionInfo = new List<QueryConditionInfo>();

                var itemCondtion = ListConditionItem.Where(m => m.JoinType == ConditionJoinType.None.ToString()).FirstOrDefault();
                if (itemCondtion != null)
                {
                    ListConditionItem.Remove(itemCondtion);
                    ListConditionItem.Insert(0, itemCondtion);
                }

                foreach (var j in ListConditionItem.Where(m => m.ConditionID == ListCondition[i].ID).ToList())
                {
                    string value1 = j.Value1;
                    string value2 = j.Value2;
                    if (listNodes.Count > 0)
                    {
                        foreach (XmlNode t in listNodes)
                        {
                            if (t[Rep_ControlModel.FieldNames.ConditionItemID].InnerText == j.ID.ToString())
                            {
                                if (t[Rep_ControlModel.FieldNames.ControlType].InnerText == CustomReportControl.DateFromDateTo.ToString())
                                {
                                    List<string> controlValue = model.ListObject.Where(m => m.Contains(j.ID.ToString())).ToList();
                                    if (controlValue != null)
                                    {
                                        value1 = controlValue.FirstOrDefault().Replace(j.ID.ToString() + "/", "");
                                        value2 = controlValue.LastOrDefault().Replace(j.ID.ToString() + "/", "");
                                    }
                                }
                                else
                                {
                                    string controlValue = model.ListObject.Where(m => m.Contains(j.ID.ToString())).FirstOrDefault();
                                    if (controlValue != null)
                                    {
                                        value1 = controlValue.Split('/').LastOrDefault();
                                    }

                                }
                            }
                        }
                    }
                    QueryConditionInfo ConditionItem = new QueryConditionInfo();
                    ConditionItem.ColumnName = j.ColumnName;
                    ConditionItem.ConditionOperator = (ConditionOperator)Enum.Parse(typeof(ConditionOperator), j.Operator); ;
                    ConditionItem.ConditionOrdinal = 0;
                    ConditionItem.ConditionValue1 = value1;
                    ConditionItem.ConditionValue2 = value2;
                    ConditionItem.TableName = j.TableName;
                    ConditionItem.ConditionJoinType = (ConditionJoinType)Enum.Parse(typeof(ConditionJoinType), j.JoinType);
                    GroupConditionItem.ListQueryConditionInfo.Add(ConditionItem);
                }
                queryBuilder.ListGroupConditionInfo.Add(GroupConditionItem);
            }

            SqlHelper dataHelper = new SqlHelper(MasterSerives.GetConnectionString().Replace(";multipleactiveresultsets=True;application name=EntityFramework", ""));
            QueryBuilder.DataHelper = dataHelper;
            dataHelper.CreateCommand(masterItem.ScriptExcute);
            dataHelper.AddParameter(QueryBuilder.BuildParameters(queryBuilder).ToArray());
            DataTable result = dataHelper.Fill();

            #region Tạo và xuất template]
            object obj = new Rep_MasterModel();
            var isDataTable = false;

            if (model.IsCreateTemplateForDynamicGrid)
            {
                obj = result;
                isDataTable = true;
            }
            if (model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Rep_MasterModel",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportID != Guid.Empty)
            {

                var fullPath = ExportService.Export(model.ExportID, result, null, ExportFileType.Excel);

                return Json(fullPath);
            }
            #endregion



            DataSourceRequest request = new DataSourceRequest();
            request.Page = 1;
            request.PageSize = 50;

            return new JsonResult() { Data = result.ConfigTable().ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetDataRenderControl(Guid? MasterID, string TableName, string ValueField, string TextField, bool IsEnum)
        {
            string status = string.Empty;
            ActionService services = new ActionService(UserLogin);
            Rep_MasterServices MasterSerives = new Rep_MasterServices();
            var MasterItem = services.GetByIdUseStore<Rep_MasterEntity>((Guid)MasterID, ConstantSql.hrm_rep_sp_get_MasterByID, ref status);

            if (!IsEnum)//đối tượng
            {
                SqlHelper dataHelper = new SqlHelper(MasterSerives.GetConnectionString().Replace(";multipleactiveresultsets=True;application name=EntityFramework", ""));
                QueryBuilder.DataHelper = dataHelper;

                dataHelper.CreateCommand("select " + ValueField + "," + TextField + " from " + TableName);
                DataTable data = dataHelper.Fill();

                foreach (DataRow dr in data.Rows)
                {
                    if (dr[0].ToString() == "bweb.vn")
                    {

                    }
                }
                var results = from myRow in data.AsEnumerable()
                              select new
                              {

                                  Value = myRow[0],
                                  Text = myRow[1],
                              };
                //var result = from e in data.Rows
                //             select new 
                //             {
                //                 Value=e,
                //                 Text = e,
                //             };
                //return Json(result, JsonRequestBehavior.AllowGet);

                return new JsonResult() { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else//là enum
            {

            }

            return Json("");
        }
        #endregion

        #region Sys_AutoBackup
        [HttpPost]
        public ActionResult GetAutoBackupList([DataSourceRequest] DataSourceRequest request, Sys_AutoBackupSearchModel model)
        {
            return GetListDataAndReturn<Sys_AutoBackupModel, Sys_AutoBackupEntity, Sys_AutoBackupSearchModel>
                (request, model, ConstantSql.hrm_sys_sp_get_AutoBackup);
        }
        #endregion

        #region validate

        public ActionResult GetUserSettingValidate(Sys_UserSettingModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sys_UserSettingModel>(model, "Sys_UserSetting", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            return Json(message);
            #endregion
        }

        #endregion

        [HttpPost]
        public ActionResult GetLanguageNTheme(string id)
        {
            var service = new Sys_AttOvertimePermitConfigServices();
            var language = service.GetConfigValue<string>(AppConfig.HRM_SYS_USERSETTING_LANGUAGE.ToString() + "_" + id);
            var theme = service.GetConfigValue<string>(AppConfig.HRM_SYS_USERSETTING_THEME + "_" + id);
            var userSetting = new Sys_UserSettingModel()
            {
                LanguageName = language,
                ThemeName = theme//,
                //UserCreateID =  id
            };
            return Json(userSetting);
        }

        [HttpPost]
        public ActionResult ChangeTheme(string id)
        {
            var service = new Sys_AttOvertimePermitConfigServices();
            var theme = service.GetConfigValue<string>(AppConfig.HRM_SYS_USERSETTING_THEME + "_" + id);
            var userSetting = new Sys_UserSettingModel()
            {
                ThemeName = theme//,
                //UserCreateID =  id
            };
            return Json(userSetting);
        }

        #region Schedule Task
        [HttpPost]
        public ActionResult RunScheuleTasks(string id)
        {
            var status = string.Empty;
            var message = string.Empty;

            if (id.IndexOf(",") > 0)
            {
                var ScheduleIDs = id.Split(',');
                foreach (var item in ScheduleIDs)
                {
                    ScheduleTaskExcute.RunTaskScheduler(Common.ConvertToGuid(item));
                    message = NotificationType.Success.ToString();
                }
            }
            else
            {
                ScheduleTaskExcute.RunTaskScheduler(Common.ConvertToGuid(id));
                message = NotificationType.Success.ToString();
            }
            return Json(message);
        }

        public ActionResult StartScheuleTasks(string id)
        {
            var status = string.Empty;
            var message = string.Empty;
            if (id.IndexOf(",") > 0)
            {
                var ScheduleIDs = id.Split(',');
                foreach (var item in ScheduleIDs)
                {
                    ScheduleTaskExcute.StartTaskScheduler(Common.ConvertToGuid(item));
                    message = NotificationType.Success.ToString();
                }
            }
            else
            {
                ScheduleTaskExcute.StartTaskScheduler(Common.ConvertToGuid(id));
                message = NotificationType.Success.ToString();
            }
            return Json(message);
        }

        #endregion
        #region ConfigProcessApprove
        [HttpPost]
        public ActionResult GetConfigAPList([DataSourceRequest] DataSourceRequest request, Sys_ConfigProcessApproveSearchModel model)
        {
            //if (model != null)
            //{
            //    model.UserApproveID = Common.DotNetToOracle(model.UserApproveID);
            //}

            return GetListDataAndReturn<Sys_ConfigProcessApproveModel, Sys_ConfigProcessApproveEntity, Sys_ConfigProcessApproveSearchModel>(request, model, ConstantSql.hrm_sys_sp_get_ConfigProcessApprove);
        }
        #endregion

        public ActionResult GetVerSion(string Stringurl)
        {
            Stringurl = Common.GetPath(@"bin\HRM.Presentation.Main.dll");
            FileInfo file = new FileInfo(Stringurl);
            DateTime lastWriteTimeUtc = file.LastWriteTime;
            string _datetime = String.Format("{0:d/M/yyyy HH:mm:ss}", lastWriteTimeUtc); ;
            string _verSion = "";
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            List<object> para = new List<object>();
            para.Add(null);
            List<Sys_VersionEntity> lstVerSion = service.GetData<Sys_VersionEntity>(para, ConstantSql.hrm_sys_sp_get_Version, ref status);


            // Get ngu ngu mac dinh
            //HRM_SYS_USERSETTING_LANGUAGE

            Sys_AllSettingServices settingService = new Sys_AllSettingServices();
            List<Sys_AllSettingEntity> lstAllSetting = settingService.Get(AppConfig.HRM_SYS_SETTING_LANGUAGE_DEFAULT.ToString()).ToList();
            Sys_VersionEntity objVerSion = new Sys_VersionEntity();
            if (lstVerSion != null)
            {
                objVerSion = lstVerSion.OrderBy(s => s.Name).FirstOrDefault();
                if (objVerSion != null)
                {
                    _verSion = objVerSion.Value.ToString();
                }
            }
            List<object> obj = new List<object>();
            obj.Add(_datetime);
            obj.Add(_verSion);
            if(lstAllSetting != null && lstAllSetting.Count > 0)
            {
                obj.Add(lstAllSetting[0].Value1);
            }
            
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMultiForeignKey(string text)
        {
            var splitText = text.Split('|');
            var textSearch = (splitText != null && splitText.Count() > 1) ? splitText[0] : string.Empty;
            var table1 = (splitText != null && splitText.Count() > 1) ? splitText[1] : string.Empty;
            var table2 = (splitText != null && splitText.Count() > 2) ? splitText[2] : string.Empty;

            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.Add(textSearch);
            lstParam.Add(table1);
            lstParam.Add(table2);
            var service = new ActionService(UserLogin);
            var data = service.GetData<Sys_ForeignKeyMultiEntity>(lstParam, ConstantSql.hrm_sys_sp_get_ForeignKey_multi, ref status);

            if (data != null)
            {
                var result = data.Select(item => new Sys_ForeignKeyMultiModel()
                {
                    COLUMN_NAME = item.COLUMN_NAME,
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return (Json(new Sys_ForeignKeyMultiModel()));
        }

        /// <summary>
        /// Clear Cache
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }


            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }

            //foreach (System.Collections.DictionaryEntry entry in HttpContext.Cache){
            //    HttpContext.Cache.Remove(entry.Key.ToString());
            //}

            return Json("");
        }

        public ActionResult SQLCommander([DataSourceRequest] DataSourceRequest request, Sys_SQLCommanderModel model)
        {
            try
            {
                int totalRow = 0;
                var isDataTable = false;
                DataTable obj = null;

                Sys_SqlCommanderServices service = new Sys_SqlCommanderServices();
                DataTable dt = service.ExecuteCommand(Guid.Empty, model.Commander, request.Page, request.PageSize, out totalRow);


                var dataSourceResult = dt.ToDataSourceResult(request);
                dataSourceResult.Total = totalRow;

                if (model.IsCreateTemplateForDynamicGrid)
                {
                    obj = dt;
                    isDataTable = true;
                }
                if (model != null && model.IsCreateTemplate)
                {
                    var path = Common.GetPath("Templates");
                    ExportService exportService = new ExportService();

                    ConfigExport cfgExport = new ConfigExport()
                    {
                        Object = obj,
                        FileName = "Sys_SQLCommanderModel",
                        OutPutPath = path,
                        // HeaderInfo = listHeaderInfo,
                        DownloadPath = Hrm_Main_Web + "Templates",
                        IsDataTable = isDataTable
                    };
                    var str = exportService.CreateTemplate(cfgExport);
                    return Json(str);
                }

                if (model.ExportId != Guid.Empty)
                {
                    var fullPath = ExportService.Export(model.ExportId, dt, null, model.ExportType);
                    return Json(fullPath);
                }

                return new JsonResult()
                {
                    Data = dataSourceResult,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetDataByProfileID(string ProfileID)
        {

            string status = string.Empty;
            var profileID = Guid.Empty;

            if (!string.IsNullOrEmpty(ProfileID))
            {
                profileID = Common.ConvertToGuid(ProfileID);
            }

            ActionService service = new ActionService(UserLogin);
            var lstProfile = service.GetData<Hre_ProfileEntity>(profileID, ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();

            if (lstProfile != null)
            {
                return Json(lstProfile, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetDateByTime(int year)
        {
            var result = new Sys_EvaConfigEntity();
            string status = string.Empty;
            Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
            //if (time == 1)
            //{
            //    var _dateStart = sysServices.GetConfigValue<DateTime>(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME1);
            //    var _dateEnd = sysServices.GetConfigValue<DateTime>(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME1);
            //    if (_dateStart != null && _dateEnd != null)
            //    {
            //        result.DateStart = new DateTime(year, _dateStart.Month, _dateStart.Day).ToString("dd/MM/yyyy");
            //        result.DateEnd = new DateTime(year + 1, _dateEnd.Month, _dateEnd.Day).ToString("dd/MM/yyyy");
            //    }
            //}
            //else
            //{
            //    var _dateStart = sysServices.GetConfigValue<DateTime>(AppConfig.HRM_EVA_CONFIG_DATESTARTNODETIME2);
            //    var _dateEnd = sysServices.GetConfigValue<DateTime>(AppConfig.HRM_EVA_CONFIG_DATEENDNODETIME2);
            //    if (_dateStart != null && _dateEnd != null)
            //    {
            //        result.DateStart = new DateTime(year, _dateStart.Month, _dateStart.Day).ToString("dd/MM/yyyy");
            //        result.DateEnd = new DateTime(year + 1, _dateEnd.Month, _dateEnd.Day).ToString("dd/MM/yyyy");
            //    }
            //}
            var _DateConfig = sysServices.GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DAYENDYEARFINANCE);
            if (_DateConfig != null)
            {
                var _tempDate = _DateConfig.Value.AddDays(1);
                result.DateStart = new DateTime(year, _tempDate.Month, _tempDate.Day).ToString("dd/MM/yyyy");
                _DateConfig = new DateTime(DateTime.Now.Year, _DateConfig.Value.Month, _DateConfig.Value.Day); 
                if (DateTime.Now.Date > _DateConfig)
                {
                    result.DateEnd = new DateTime(year + 1, _DateConfig.Value.Month, _DateConfig.Value.Day).ToString("dd/MM/yyyy");
                }
                else
                {
                    result.DateEnd =  DateTime.Now.Date.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                result.Messages = "error";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddDataForLockObjectType(Guid LockObjectID, Guid LockObjectTypeID)
        {
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var LockObjItemServices = new Sys_LockObjectItemServices();
            var entity = service.GetByIdUseStore<Cat_NameEntityEntity>(LockObjectTypeID, ConstantSql.hrm_cat_sp_get_NameEntityById, ref status);
            var tableServices = new ActionService(UserLogin);
            var objTable = new List<object>();
            objTable.Add(null);
            objTable.Add(1);
            objTable.Add(int.MaxValue - 1);
            var lstTable = tableServices.GetData<Cat_SysTablesMultiEntity>(objTable, ConstantSql.hrm_cat_sp_get_tables, ref status).ToList();

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Description))
                {
                    string[] objName = entity.Description.Split(',');
                    if (objName.Length > 0)
                    {
                        foreach (var item in objName)
                        {
                            Sys_LockObjectItemEntity LockObjectItemEntity = new Sys_LockObjectItemEntity();
                            LockObjectItemEntity.LockObjectID = LockObjectID;
                            LockObjectItemEntity.ObjectName = item;
                            status = LockObjItemServices.Add(LockObjectItemEntity);
                        }
                    }
                }

            }
            //var service = new Hre_ProfileServices();
            //service.UpdateSalaryClassNameForProfile(SalaryClassName, ProfileIDs, DateEndProbation, DateHire, OrgStructureID, SalaryRankID, WorkPlaceID, ContractTypeID, BasicSalary);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Chuyển tiếng việt thành tiếng việt không dấu</summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Convert_VI_To_EN(string fullName)
        {
            string stFormD = fullName.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            var result = (sb.ToString().Normalize(NormalizationForm.FormD));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}