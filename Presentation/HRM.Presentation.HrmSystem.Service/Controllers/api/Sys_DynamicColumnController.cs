using System;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_DynamicColumnController : ApiController
    {
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public IEnumerable<Sys_DynamicColumnModel> Get()
        {
            string status = string.Empty;
            var service = new Sys_DynamicColumnServices();
            var list = service.GetAllUseEntity<Sys_DynamicColumnEntity>(ref status);
            
            return list.Select(item => new Sys_DynamicColumnModel
            {
                ID = item.ID,
                ColumnName = item.ColumnName,
                Code = item.Code,
                Status = item.Status,
                DataType = item.DataType,
                Length = item.Length,
                TableName = item.TableName,
                Comment = item.Comment
            });

        }
        
        // GET api/<controller>/5
        public Sys_DynamicColumnModel Get(Guid id)
        {
            string status = string.Empty;
            var service = new Sys_DynamicColumnServices();
            var result = service.GetById<Sys_DynamicColumnEntity>(id, ref status);
            var Sys_DynamicColumn = new Sys_DynamicColumnModel
            {
                ID = result.ID,
                ColumnName = result.ColumnName,
                Code = result.Code,
                Status = result.Status,
                DataType = result.DataType,
                Length = result.Length,
                TableName = result.TableName,
                Comment = result.Comment
            };
            return Sys_DynamicColumn;
            
        }

        /// <summary>
        /// Xử lý edit hay add new tùy thuộc vào id đã có hay chưa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_DynamicColumn")]
        public string Post([Bind]Sys_DynamicColumnModel model)
        {
            var success = string.Empty;
            if (model.Length != null)
            {
                if (model.DataType == "Decimal") model.DataType = model.DataType + "(" + model.Length + ",0)";
                else
                {
                    model.DataType = model.DataType + "(" + model.Length + ")";
                }
            }
            else
            {
                model.DataType = model.DataType;
            }
            var listDynamicColumnModel = new List<Sys_DynamicColumnModel> { model };
            var entity = listDynamicColumnModel.ToList().Translate<Sys_DynamicColumnEntity>();
            var service = new Sys_DynamicColumnServices();
            if (model.ID != Guid.Empty)
            {
                entity[0].ID = model.ID;
                success = service.Edit<Sys_DynamicColumnEntity>(entity[0]) + ",0";
            }
            else
            {
                success = service.Add<Sys_DynamicColumnEntity>(entity[0]) + ",0";
                model.ID = entity[0].ID;
            }
            return success;
        }
        public Sys_DynamicColumnModel Put(Sys_DynamicColumnModel model)
        {
            var Sys_DynamicColumn = new Sys_DynamicColumnEntity
            {
                ID = model.ID,
                ColumnName = model.ColumnName,
                Code = model.Code,
                Status = model.Status,
                DataType = model.DataType,
                Length = model.Length,
                TableName = model.TableName,
                Comment = model.Comment
            };
            var service = new Sys_DynamicColumnServices();
            if (model.ID != Guid.Empty)
            {
                Sys_DynamicColumn.ID = model.ID;
                service.Edit<Sys_DynamicColumnEntity>(Sys_DynamicColumn);
            }
            else
            {
                service.Add(Sys_DynamicColumn);
                model.ID = Sys_DynamicColumn.ID;
            }
            return model;
        }

        // DELETE api/<controller>/5
        public Sys_DynamicColumnModel Delete(Guid id)
        {
            var service = new Sys_DynamicColumnServices();
            service.Remove<Sys_DynamicColumnEntity>(id);
            var result = new Sys_DynamicColumnModel();
            return result;
        }

    }
}