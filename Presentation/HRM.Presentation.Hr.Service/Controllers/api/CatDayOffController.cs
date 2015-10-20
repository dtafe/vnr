using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Net.Http;
using System.Web.Mvc;
using System.Net;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

using HRM.Business.Category.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatDayOffController : ApiController
    {
        #region UserLogin
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        public int Weekday(DateTime dt, DayOfWeek startOfWeek)
        {
            return (dt.DayOfWeek - startOfWeek + 7) % 7;
        }
        public int GetIndexDayOfWeek(string dayOfWeek)
        {
            int i = 2;
            if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_MONDAY.ToString())
                i = 2;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_TUESDAY.ToString())
                i = 3;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_WEDNESDAY.ToString())
                i = 4;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_THURSDAY.ToString())
                i = 5;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_FRIDAY.ToString())
                i = 6;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_SATURDAY.ToString())
                i = 7;
            else if (dayOfWeek == EnumDropDown.DaysOFWeeks.E_SUNDAY.ToString())
                i = 8;
            return i;
        }
        private Cat_DayOffEntity createNewDayOff(string holidayType, DateTime dateOff)
        {
            return new Cat_DayOffEntity
            {
                Type = holidayType,
                DateOff = dateOff
            };
        }

        /// <summary>
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Ngày Nghỉ Lễ(Cat_DayOff) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatDayOffModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatDayOffModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_DayOffEntity>(id, ConstantSql.hrm_cat_sp_get_DayOffById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatDayOffModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Ngày Nghỉ Lễ(Cat_DayOff) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatDayOffModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_DayOffEntity, CatDayOffModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Ngày Nghỉ Lễ(Cat_DayyOff)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatDayOff")]
        public CatDayOffModel Post([Bind]CatDayOffModel model)
        {
            ActionService service = new ActionService(UserLogin);

            #region Validate
            var type = "Cat_DayOff";
            if (model.ID != Guid.Empty)
            {
                type = "Cat_DayOff_Edit";
            }
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatDayOffModel>(model, type, ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            if (model.ID != Guid.Empty)
            {
                return service.UpdateOrCreate<Cat_DayOffEntity, CatDayOffModel>(model);
            }
            List<Cat_DayOffEntity> listDayOffInsert = new List<Cat_DayOffEntity>();
            List<Cat_DayOffEntity> listDayOff = getDayOff(model);
            List<Cat_DayOffEntity> listDayOffDelete = new List<Cat_DayOffEntity>();

            string status = string.Empty;
            Cat_DayOffServices dayOffService = new Cat_DayOffServices();
            var listDayOffExist = dayOffService.GetAll();

            foreach (Cat_DayOffEntity item in listDayOff)
            {
                item.IsLeaveDay = model.IsLeaveDay;
                item.Comments = model.Comments;
                item.OrgStructureID = model.OrgStructureID;
                item.UserCreate = model.UserCreate;
                item.DateUpdate = model.DateUpdate;

                DateTime _dateOff = item.DateOff.Date;
                listDayOffExist = listDayOffExist.Where(dOff => dOff.DateOff.Date == _dateOff).ToList();

                if (listDayOffExist.Count > 0)
                {
                    if (model.Overwite)
                    {
                        listDayOffDelete.AddRange(listDayOffExist);
                        item.ID = Guid.NewGuid();
                        listDayOffInsert.Add(item);
                    }
                    else
                        continue;
                }
                else
                {
                    item.ID = Guid.NewGuid();
                    listDayOffInsert.Add(item);
                }
            }
            if (listDayOffDelete.Count > 0)
            {
                List<Guid> lstDel = listDayOffDelete.Select(s => s.ID).ToList();
                foreach (var item in lstDel)
                {
                    dayOffService.Remove<Cat_DayOffEntity>(item);
                }
            }
            CatDayOffModel rs = new CatDayOffModel();
            rs.ActionStatus = dayOffService.Add<Cat_DayOffEntity>(listDayOffInsert);
            return rs;




            //if (model.ID == Guid.Empty && model.DateEnd.HasValue)
            //{
            //    CatDayOffModel modelReturn = model.CopyData<CatDayOffModel>();
            //    DateTime dtFrom = model.DateOff;
            //    while (dtFrom <= model.DateEnd.Value)
            //    {
            //        CatDayOffModel NewModel = new CatDayOffModel();
            //        NewModel.DateOff = dtFrom;
            //        NewModel.Type = model.Type;
            //        NewModel.Status = model.Status;
            //        NewModel.Comments = model.Comments;
            //        NewModel.OrgStructureID = model.OrgStructureID;
            //        NewModel.IsLeaveDay = model.IsLeaveDay;
            //        NewModel.OrgStructureName = model.OrgStructureName;
            //        NewModel.UserUpdate = model.UserUpdate;
            //        NewModel.DateUpdate = model.DateUpdate;
            //        modelReturn = service.UpdateOrCreate<Cat_DayOffEntity, CatDayOffModel>(NewModel);
            //        if (modelReturn.ActionStatus != NotificationType.Success.ToString())
            //            return modelReturn;
            //        dtFrom = dtFrom.AddDays(1);
            //    }
            //    return modelReturn;
            //}
            //else
            //{
            //    return service.UpdateOrCreate<Cat_DayOffEntity, CatDayOffModel>(model);
            //}
        }

        public List<Cat_DayOffEntity> getDayOff(CatDayOffModel model)
        {
            List<Cat_DayOffEntity> listNewDayOff = new List<Cat_DayOffEntity>();
            #region Xử Lý khi chon LoopType

            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now;
            DateTime dateOff = DateTime.Now;
            DateTime dateMonthSelect = DateTime.Now;

            int fromYear, toYear = 0;
            int dayOfMonth = 0;
            string holidayType = model.Type;

            switch (model.LoopType)
            {
                case "E_WEEK":
                    dateStart = model.DateStart.Value;
                    if(model.DateEnd != null)
                    {
                        dateEnd = model.DateEnd.Value;
                    }
                    

                    for (DateTime idx = dateStart; idx <= dateEnd; idx = idx.AddDays(1))
                    {
                        int i = Weekday(idx, DayOfWeek.Monday) + 2;
                        if (i == GetIndexDayOfWeek(model.DaysOFWeeks))
                            listNewDayOff.Add(createNewDayOff(holidayType, idx));
                    }
                    break;
                case "E_MONTH":
                    dayOfMonth = int.Parse(model.DateOff.Day.ToString());
                    dateStart = model.DateStart.Value;
                    dateEnd = model.DateEnd != null ? model.DateEnd.Value : dateStart;
                    for (DateTime idx = dateStart; idx <= dateEnd; idx = idx.AddDays(1))
                    {
                        if (dayOfMonth == idx.Date.Day)
                        {
                            listNewDayOff.Add(createNewDayOff(holidayType, idx));
                        }
                    }
                    break;
                case "E_YEAR":
                    //fromYear = int.Parse(ddl_FromYear.SelectedItem.Value);
                    fromYear = int.Parse(model.DateStart.Value.Year.ToString());
                    toYear = model.DateEnd != null ? int.Parse(model.DateEnd.Value.Year.ToString()) : fromYear;

                    dateStart = new DateTime(fromYear, model.DateOff.Month, model.DateOff.Day);
                    dateEnd = new DateTime(toYear, model.DateOff.Month, model.DateOff.Day);

                    for (DateTime idx = dateStart; idx <= dateEnd; idx = idx.AddYears(1))
                    {
                        listNewDayOff.Add(createNewDayOff(holidayType, idx));
                    }
                    break;
                case "E_DAY":

                    dateStart = model.DateStart.Value;
                    if(model.DateEnd != null){
                        dateEnd = model.DateEnd.Value;
                    }
                    

                    for (DateTime idx = dateStart; idx <= dateEnd; idx = idx.AddDays(1))
                    {
                        listNewDayOff.Add(createNewDayOff(holidayType, idx));
                    }

                    break;

                default:
                    break;
            }
            #endregion
            return listNewDayOff;
        }
    }
}
