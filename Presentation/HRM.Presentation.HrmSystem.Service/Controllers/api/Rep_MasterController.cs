using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Web;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Report.Domain;
using HRM.Business.Report.Models;


namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Rep_MasterController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    //var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    //userLogin = headerValues.FirstOrDefault();
                    userLogin = "son.vo";
                }
                return userLogin;
            }
        }
        #endregion
        public Rep_MasterModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rep_MasterModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Rep_MasterEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_rep_sp_get_MasterByID, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Rep_MasterModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rep_Master")]
        public Rep_MasterModel Post([Bind]Rep_MasterModel model)
        {
            string status=string.Empty;
            #region Validate

            //string message = string.Empty;
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rep_MasterModel>(model, "Rep_Master", ref message);
            //if (!checkValidate)
            //{
            //    model.ActionStatus = message;
            //    return model;
            //}

            #endregion
            ActionService service = new ActionService(UserLogin);

            Rep_MasterModel master = new Rep_MasterModel();
            if (model.ID == null || model.ID == Guid.Empty)
            {
                master = service.UpdateOrCreate<Rep_MasterEntity, Rep_MasterModel>(model);
                //Add column
                if (model.ListColumn != null && model.ListColumn.Count > 0)
                {
                    int number = 0;
                    foreach (var i in model.ListColumn)
                    {
                        number++;
                        List<string> columnName = i.Split(',').ToList();
                        Rep_ColumnModel item = new Rep_ColumnModel();
                        item.MasterID = master.ID;
                        item.ColumnName = columnName[1];
                        item.TableName = columnName.FirstOrDefault();
                        item.DisplayName = columnName.LastOrDefault();
                        item.ColumnOrdinal = number;
                        service.UpdateOrCreate<Rep_ColumnEntity, Rep_ColumnModel>(item);
                    }
                }

                //Add Group Default
                Rep_ConditionModel ConditionItem = new Rep_ConditionModel() { 
                    MasterID=master.ID,
                    OrderNumber=0,
                    WhereType=WhereType.None.ToString(),
                    ConditionGroupName="None",
                };
                service.UpdateOrCreate<Rep_ConditionEntity, Rep_ConditionModel>(ConditionItem);
            }
            else
            {
                Rep_MasterModel masteritem = GetById(model.ID);
                if (model.ScriptParams == null)
                {
                    model.ScriptParams = masteritem.ScriptParams;
                }
                if (model.ScriptExcute == null)
                {
                    model.ScriptExcute = masteritem.ScriptExcute;
                }

                master = service.UpdateOrCreate<Rep_MasterEntity, Rep_MasterModel>(model);

                var columnservice = new Rep_ColumnServices();
                List<object> listModel = new List<object>();
                listModel.Add(model.ID);
                listModel.Add(1);
                listModel.Add(Int32.MaxValue - 1);
                List<Rep_ColumnEntity> listColumn = columnservice.GetData<Rep_ColumnEntity>(listModel, ConstantSql.hrm_rep_sp_get_ColumnByMasterID, UserLogin, ref status);
                foreach (var i in listColumn)
                {
                    Delete(i.ID);
                }

                if (model.ListColumn != null && model.ListColumn.Count > 0)
                {
                    foreach (var i in model.ListColumn)
                    {
                        List<string> columnName = i.Split(',').ToList();
                        Rep_ColumnModel item = new Rep_ColumnModel();
                        item.MasterID = master.ID;
                        item.ColumnName = columnName[1];
                        item.TableName = columnName.FirstOrDefault();
                        item.DisplayName = columnName.LastOrDefault();
                        service.UpdateOrCreate<Rep_ColumnEntity, Rep_ColumnModel>(item);
                    }
                }
            }
            
            
            return master;
        }
        public void Delete(Guid id)
        {
            var service = new Rep_ColumnServices();
            var result = service.Remove<Rep_ColumnEntity>(id);
        }
	}
}