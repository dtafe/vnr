using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;
using System.Data;
using System.Collections.Generic;
using HRM.Business.Training.Domain;
using System.Linq;
using HRM.Business.Attendance.Domain;
using HRM.Presentation.Attendance.Models;
using System.Net.Http.Formatting;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Attendance.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_ChangeRosterGroupTableController : ApiController
    {
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
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_ChangeRosterGroupTableModel GetById(string id)
        {
            Att_ChangeRosterGroupTableModel result = new Att_ChangeRosterGroupTableModel();
            List<Guid> SelectedIds = new List<Guid>();
            var rosterGroupServices = new Att_RosterGroupServices();
            string status = string.Empty;
            var date = DateTime.Now;
            var services = new Sys_AllSettingServices();
            var key = AppConfig.HRM_ATT_CONFIG_NAME_ROSTERGROUP.ToString();
            var configEntity = services.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).ToList().FirstOrDefault();

        

            if (!string.IsNullOrEmpty(id))
            {
                if (id.IndexOf('-') > 1)
                {
                   
                    SelectedIds = id.Split(',').Select(s => Guid.Parse(s)).ToList();
                    if (SelectedIds != null && SelectedIds.Count > 0)
                    {
                        if (id != "Create")
                        {
                            var rosterGroupEntity = services.GetData<Att_RosterGroupEntity>(Common.DotNetToOracle(id), ConstantSql.hrm_att_sp_get_RosterGroupById, UserLogin, ref status).FirstOrDefault();
                            if (rosterGroupEntity != null && rosterGroupEntity.DateStart != null)
                            {
                                date = rosterGroupEntity.DateStart.Value;
                            }
                            result.Table = rosterGroupServices.GetTableRosterGroupByMonth(date, ModifyType.E_EDIT);
                            if (configEntity != null)
                            {
                                result.lstType = configEntity.Name.Split(',').ToList();
                            }
                            return result;
                        
                        }
                       
                    }
                }
                else {
                    if (configEntity != null)
                    {
                        result.lstType = configEntity.Value1.Split(',').ToList();
                    }
                    result.Table = rosterGroupServices.GetTableRosterGroupByMonth(date, ModifyType.E_CREATE);
                    return result;
                }
               
            }
           

            
            return null;
        }



        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_ChangeRosterGroupTable")]
        public Guid Post([Bind]FormDataCollection grouppermissionInfo)
        {
            Guid groupId = Guid.Empty;
            DataTable tb = new DataTable();
        

           var lstResource =  grouppermissionInfo.ToList();
           lstResource.Add( new KeyValuePair<string, string>("last_100", ""));
            #region Add Title Column
            foreach (var item in lstResource)
            {
                var str = item.Key.Split('_');
                if (int.Parse(str[1]) > 1)
                {
                    break;
                }
                tb.Columns.Add(str[0]);

            }
            #endregion

            #region Add Row
            var count = 1;
            var index = 0;
            string[] itemData = new string[10];

            foreach (var item in lstResource)
            {

                var flag = false;

                var str = item.Key.Split('_');
                if (int.Parse(str[1]) != count)
                {
                    flag = false;
                }
                if (int.Parse(str[1]) == count)
                {

                    if (tb.Columns.Contains(str[0]))
                    {
                        flag = true;
                        itemData[index] = item.Value;
                        index++;
                    }
                
                }
                if(flag == false)
                {
                    DataRow dr = tb.NewRow();
                    dr.ItemArray = itemData;
                    tb.Rows.Add(dr);
                    itemData = new string[10];
                    count++;
                    index = 0;

                    if (int.Parse(str[1]) == count)
                    {
                        if (tb.Columns.Contains(str[0]))
                        {
                            flag = true;
                            itemData[index] = item.Value;
                            index++;
                        }
                    }
                  
                }
              
               
            }
            #endregion

            var rosterGroupServices = new Att_RosterGroupServices();
           
            rosterGroupServices.SaveRosterGroup(DateTime.Now,tb);

            return groupId;
        }
	}
}