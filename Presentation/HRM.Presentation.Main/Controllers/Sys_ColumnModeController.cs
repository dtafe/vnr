using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.UI.Controls.Kendo;
using Newtonsoft.Json;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_ColumnModeController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        //1. Get database
        public GridBuilderModel GetGridBuilderInfo(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                var splitPara = parameter.Split(',');
                if (splitPara.Length > 1)
                {
                    Guid userId;
                    Guid.TryParse(splitPara[0], out userId);
                    if (userId != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(splitPara[1]))
                        {
                            var paraModel = new GridBuilderModel() { UserID = userId, GridControlName = splitPara[1], IsGet = true};
                            var service = new RestServiceClient<GridBuilderModel>(UserLogin);
                            var model = service.Post(_hrm_Hre_Service, "api/Sys_ColumnMode", paraModel);
                            if (model != null && !string.IsNullOrEmpty(model.StringXml))
                            {
                                model.StringXml = model.StringXml;
                                model.ID = model.ID;
                                model = GridBuilder(model, false);
                                return model;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public string SetDefaultGridBuilderInfo(GridBuilderInfo grid)
        {
            if (grid != null)
            {
                var str = "{\"GridColumnNames\": {\"GridColumnName\": [";
                var valueFields = grid.ValueFields;
                if (valueFields != null && valueFields.Any())
                {
                    foreach (var valueField  in valueFields)
                    {
                        str += "{\"ColumnName\":\"" + valueField + "\",";
                        var filterFields = grid.Filterable;
                        var sizeFields = grid.SizeFields;
                        var lockedFields = grid.LockedFields;
                        var groupFields = grid.GroupFields;
                        if (filterFields != null && filterFields.Any())
                        {
                            bool checkFilter = filterFields.ContainsKey(valueField);
                            str += "\"Filter\":\"" + checkFilter + "\",";
                        }
                        if (sizeFields != null && sizeFields.Any())
                        {
                            if (sizeFields.ContainsKey(valueField))
                            {
                                str += "\"Width\":" + sizeFields[valueField] + ",";
                            }
                        }
                        if (lockedFields != null && lockedFields.Any())
                        {
                            if (lockedFields.Contains(valueField))
                            {
                                str += "\"Locke\":" + valueField + ",";
                            }
                        }
                        if (groupFields != null && groupFields.Any())
                        {
                            if (groupFields.Contains(valueField))
                            {
                                str += "\"Group\":" + valueField + ",";
                            }
                        }
                        str = str.Substring(0, str.Length - 1) + "},";
                    }
                    str = str.Substring(0, str.Length - 1) + "]}}";
                    return str;
                    //SaveGridBuilderInfo(userId, gridFullName, str, grid.PageSize);
                }
            }
            return string.Empty;
        }

        public GridBuilderModel SaveGridBuilderInfo(Guid? userId, string gridColumnName, string columnMode, int pageSize)
        {

            if (userId != null && userId != Guid.Empty && !string.IsNullOrEmpty(gridColumnName))
            {
                GridBuilderModel model = new GridBuilderModel()
                {
                    UserID = (Guid)userId,
                    GridControlName = gridColumnName,
                    StringXml = columnMode,
                    PageSize = pageSize
                };
                var service = new RestServiceClient<GridBuilderModel>(UserLogin);
                return service.Post(_hrm_Hre_Service, "api/Sys_ColumnMode", model);
            }

            return null;
        }

        public ActionResult DeleteGridBuilderInfo(Guid? userId, string gridColumnName)
        {
            if (userId != null && userId != Guid.Empty && !string.IsNullOrEmpty(gridColumnName))
            {
                var para = userId + "," + gridColumnName;
                var service = new RestServiceClient<GridBuilderModel>(UserLogin);
                var result = service.Delete(_hrm_Hre_Service, "api/Sys_ColumnMode/", para);
                if (result != null)
                {
                    return Json(result);
                }
            }

            return Json(null);
        }

        public ActionResult Index(GridBuilderModel model)
        {
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.ObjectName))
                {
                    var objName = model.ObjectName;
                    string assembly = null;
                    if (objName.StartsWith("Hre"))
                    {
                        assembly = "HRM.Presentation.Hr.Models";
                    }
                    else if (objName.StartsWith("FIN"))
                    {
                        assembly = "HRM.Presentation.Hr.Models";
                    }
                    else if (objName.StartsWith("Att"))
                    {
                        assembly = "HRM.Presentation.Attendance.Models";
                    }
                    else if (objName.StartsWith("Cat"))
                    {
                        assembly = "HRM.Presentation.Category.Models";
                    }
                    else if (objName.StartsWith("Rec"))
                    {
                        assembly = "HRM.Presentation.Recruitment.Models";
                    }
                    else if (objName.StartsWith("Tra"))
                    {
                        assembly = "HRM.Presentation.Training.Models";
                    }
                    else if (objName.StartsWith("Sal"))
                    {
                        assembly = "HRM.Presentation.Payroll.Models";
                    }
                    else if (objName.StartsWith("Kai"))
                    {
                        assembly = "HRM.Presentation.Payroll.Models";
                    }
                    else if (objName.StartsWith("Ins"))
                    {
                        assembly = "HRM.Presentation.Insurance.Models";
                    }
                    else if (objName.StartsWith("Eva"))
                    {
                        assembly = "HRM.Presentation.Evaluation.Models";
                    }
                    Type objType = GetTypeByName(assembly, objName);
                    model = PopUpChangeColum(model, objType);
                }
            }

            return View(model);
        }

        public GridBuilderModel PopUpChangeColum(GridBuilderModel model, Type objType)
        {
            if (model != null)
            {
                model = GridBuilder(model, true);
                var listValueFields = model.ValueFields;
                var properties = objType.GetProperties();
                var columns = new Dictionary<string, string>();
                foreach (var property in properties)
                {
                    if (listValueFields == null || !listValueFields.Contains(property.Name))
                    {
                        columns.Add(property.Name, property.Name.TranslateString());
                    }
                }

                model.Columns = columns;
            }
            return model;
        }

        public GridBuilderModel GridBuilder(GridBuilderModel model, bool isDeser)
        {
            if (model != null && !string.IsNullOrEmpty(model.StringXml))
            {
                var columnMode = model.StringXml;
                XmlDocument doc = new XmlDocument();
                if (!isDeser)
                {
                    var splitStringXml = model.StringXml.Split('|');
                    columnMode = splitStringXml[0].ToString();
                    if (splitStringXml.Length > 1)
                    {
                        var strPageSize = splitStringXml[1].ToString();
                        var pageSize = 0;
                        int.TryParse(strPageSize, out pageSize);
                        model.PageSize = pageSize;
                    }
                    //Chuyển xml thành json để xử lý ajax
                    doc.LoadXml(columnMode);
                    model.StringXml = JsonConvert.SerializeXmlNode(doc);
                }
                else
                {
                    doc = JsonConvert.DeserializeXmlNode(columnMode);
                }
                //Chuyển xml thành data table để xử lý hiển thị colum trên popup
                DataSet ds = new DataSet();
                var reader = new XmlNodeReader(doc);
                ds.ReadXml(reader);
                if (ds.Tables.Count > 0)
                {
                    var table = ds.Tables[0];
                    var listValueFields = new List<string>(); 
                    var listLockeFields = new List<string>();
                    var listGroupFields = new List<string>();
                    var listSizeFields = new Dictionary<string, int>();
                    var listDisplayFields = new Dictionary<string, string>();
                    var listFilterFields = new Dictionary<string, bool>();
                    DataColumnCollection columns = table.Columns;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (columns.Contains("ColumnName"))
                        {
                            var colum = table.Rows[i]["ColumnName"].ToString();
                            if (columns.Contains("Width"))
                            {
                                var value = table.Rows[i]["Width"].ToString();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    var width = int.Parse(value);
                                    listSizeFields.Add(colum, width);
                                }
                            }
                            if (columns.Contains("Locke"))
                            {
                                var locke = bool.Parse(table.Rows[i]["Locke"].ToString());
                                if (locke)
                                {
                                    listLockeFields.Add(colum);
                                }
                            }
                            if (columns.Contains("Group"))
                            {
                                var group = bool.Parse(table.Rows[i]["Group"].ToString());
                                if (group)
                                {
                                    listGroupFields.Add(colum);
                                }
                            }
                            if (columns.Contains("Filter"))
                            {
                                var filter = bool.Parse(table.Rows[i]["Filter"].ToString());
                                listFilterFields.Add(colum, filter);
                            }
                            listValueFields.Add(colum);
                            listDisplayFields.Add(colum, colum.TranslateString());
                        }
                    }
                    model.ValueFields = listValueFields.ToArray();
                    model.SizeFields = listSizeFields;
                    model.DisplayFields = listDisplayFields;
                    model.LockedFields = listLockeFields.ToArray();
                    model.GroupFields = listGroupFields.ToArray();
                    model.DisplayFields = listDisplayFields;
                    model.FilterFields = listFilterFields;
                }
            }
            return model;
        }

        public Type GetTypeByName(string assemblyName, string objName)
        {
            var startupPath = Common.GetPath(string.Empty);
            Type objType = null;
            string assemblyPath = startupPath + "bin\\" + assemblyName;
            if (!assemblyPath.ToLower().EndsWith(".dll"))
            {
                assemblyPath = assemblyPath + ".dll";
            }

            if (System.IO.File.Exists(assemblyPath))
            {
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                objType = assembly.GetTypes().Where(d => d.Name == objName).FirstOrDefault();
            }

            return objType;
        }
	}
}