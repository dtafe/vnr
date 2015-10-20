using Devart.Data.Oracle;
using HRM.Infrastructure.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Utilities
{
    public enum FILTEROPERATOR
    {
        StartWith = 1,
        Contains,
        EndWith,
        Equals
    }
    public class SortAttribute
    {
        public string Member { get; set; }
        public System.ComponentModel.ListSortDirection SortDirection { get; set; }
    }

    public class FilterAttribute
    {
        public string Member { get; set; }
        public Type MemberType { get; set; }
        public FILTEROPERATOR Operator { get; set; }

        public string Value { get; set; }
        public object Value2 { get; set; }
    }

    public static class ListUtility
    {
        public static IQueryable<T> SortList<T>(this IQueryable<T> data,
          List<SortAttribute> sortExpressions)
        {
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
                return data;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                Func<T, object> expression = item => item.GetType()
                                .GetProperty(sortExpressions[i].Member)
                                .GetValue(item, null);

                if (sortExpressions[i].SortDirection == System.ComponentModel.ListSortDirection.Ascending)
                    data = data.OrderBy(expression).AsQueryable<T>();
                else
                    data = data.OrderByDescending(expression).AsQueryable<T>();
            }

            return data;
        }

        public static IQueryable<T> FilterList<T>(this IQueryable<T> data, List<FilterAttribute> filterExpressions)
        {
            if ((filterExpressions == null) || (filterExpressions.Count <= 0))
                return data;
            for (int i = 0; i < filterExpressions.Count; i++ )
            {
    
                Func<T, bool> expression = item =>
                    {
                        var filter = filterExpressions[i];
                        var itemValue = item.GetType()
                            .GetProperty(filter.Member)
                            .GetValue(item, null);
    
                        if (itemValue == null)
                            return false;
    
                        var value = filter.Value;
                        switch (filter.Operator)
                        {
                            case FILTEROPERATOR.Equals:
                                return itemValue.ToString().Equals(value);
                            case FILTEROPERATOR.Contains:
                                return itemValue.ToString().Contains(value);
                            case FILTEROPERATOR.StartWith:
                                return itemValue.ToString().StartsWith(value);
                            case FILTEROPERATOR.EndWith:
                                return itemValue.ToString().EndsWith(value);
                        }
    
                        return true;
                    };
                data = (IQueryable<T>)data.Where<T>(expression);
            }
    
            return data;
        }
        public static ParamaterModle ParseParamSQL(ListQueryModel model, string storeName)
        {
            string strQuery = storeName + " ";
            List<SqlParameter> lstParas = new List<SqlParameter>();
            SqlParameter para = null;
            var selectExport = model.AdvanceFilters.Where(w => w.Member != null &&  w.Member.ToLower() == "IsExport".ToLower()).FirstOrDefault();
            if (selectExport != null)
            {
                if (selectExport.Value2 != null)
                {
                    bool isExport = (bool)selectExport.Value2;
                    if (isExport)
                    {
                        model.PageSize = 100000;
                    }
                }
            }

            foreach (var item in model.AdvanceFilters)
            {
                if (item.Member != null && (item.Member.ToLower() == "IsExport".ToLower()
                    || item.Member == "ValueFields" || item.Member == "LoginUserID"))
                {
                    continue;
                }

                Type paramType = item.MemberType;

                strQuery += "@" + item.Member + ", ";
                para = new SqlParameter();
                para.ParameterName = item.Member;
                FilterAttribute filterAtt = model.Filters.Where(m => m.Member == item.Member).FirstOrDefault();
                object paramValue = item.Value2;
                if (paramValue == null && filterAtt != null && filterAtt.Value != null)
                {
                    paramValue = filterAtt.Value;
                }
                paramValue = para.Value = item.Value2;
                if (paramType == typeof(Int32?) || paramType == typeof(Int32))
                {
                    para.SqlDbType = System.Data.SqlDbType.Int;
                }
                //Tìm kiếm dạng Unicode, có dấu thay cho ký tự N trong Sql
                System.Text.RegularExpressions.Regex isGuid = new System.Text.RegularExpressions.Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", System.Text.RegularExpressions.RegexOptions.Compiled);
                bool _isMatch = isGuid.IsMatch(item.Value2 == null ? string.Empty : item.Value2.ToString());
                if ((item.MemberType == typeof(string) && !_isMatch && item.Member != "OrgStructureID") && paramValue != null)
                {
                    System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
                    para.Value = unicode.GetBytes(item.Value2.ToString());
                }
                
                else
                    if (item.MemberType == typeof(Int32?))
                    {
                        para.DbType = System.Data.DbType.Int32;
                    }
                    else if (item.MemberType == typeof(Double?))
                    {
                        para.DbType = System.Data.DbType.Double;
                    }
                    else if (item.MemberType == typeof(DateTime?) || item.MemberType == typeof(DateTime))
                    {
                        para.DbType = System.Data.DbType.DateTime;
                    }

                if (paramValue == null)
                    para.Value = DBNull.Value;
                lstParas.Add(para);
            }
            strQuery += "@PageIndex";
            para = new SqlParameter();
            para.ParameterName = "PageIndex";
            para.Value = model.PageIndex == 0 ? 1 : model.PageIndex;
            para.DbType = System.Data.DbType.Int32;
            lstParas.Add(para);
            if (model.PageSize > 0)
            {
                strQuery += ",@PageSize";
                para = new SqlParameter();
                para.ParameterName = "PageSize";
                para.Value = model.PageSize;
                para.DbType = System.Data.DbType.Int32;
                lstParas.Add(para);
            }
            else
            {
                //if (strQuery.LastIndexOf(',') > 0)
                //{
                //    strQuery = strQuery.Substring(0, strQuery.Length - 1);
                //}
            }


            ParamaterModle param = new ParamaterModle();
            param.Params = lstParas.ToArray();
            param.SqlQuery = strQuery;
            return param;
        }

        public static ParamaterModle ParseParamOracle(ListQueryModel model, string storeName)
        {
            string strQuery = "begin " + storeName + "(";
            List<OracleParameter> lstParas = new List<OracleParameter>();
            OracleParameter para = null;
            if (model.AdvanceFilters != null)
            {
                var selectExport = model.AdvanceFilters.Where(w => w.Member == "IsExport").FirstOrDefault();
                if (selectExport != null)
                {
                    if (selectExport.Value2 != null)
                    {
                        bool isExport = (bool)selectExport.Value2;
                        if (isExport)
                        {
                            model.PageSize = int.MaxValue;
                        }
                    }
                }

                foreach (var item in model.AdvanceFilters)
                {
                    if (item.Member != null && (item.Member.ToLower() == "IsExport".ToLower()
                        || item.Member == "ValueFields" || item.Member == "LoginUserID"))
                    {
                        continue;
                    }

                    Type paramType = item.MemberType;

                    strQuery += ":" + item.Member + ", ";
                    para = new OracleParameter();
                    para.ParameterName = item.Member;
                   

                   FilterAttribute filterAtt = model.Filters.Where(m => m.Member == item.Member).FirstOrDefault();
                   object paramValue = item.Value2;
                   if (paramValue == null && filterAtt != null && filterAtt.Value != null)
                    {
                        paramValue = filterAtt.Value;
                    }
                   para.Value = paramValue;
                    if (paramType == typeof(Int32?) || paramType == typeof(Int32))
                    {
                        para.DbType = System.Data.DbType.Int32;
                        para.OracleDbType = OracleDbType.Number;
                    }
                    //Tìm kiếm dạng Unicode, có dấu thay cho ký tự N trong Sql
                    if ((item.MemberType == typeof(string) && item.Member != "OrgStructureID") && paramValue != null)
                    {
                        //System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
                        para.DbType = System.Data.DbType.String;
                        para.OracleDbType = OracleDbType.NVarChar;
                        para.Value = paramValue;
                    }
                    //else
                    //{
                    //    if (item.MemberType == typeof(Int32?))
                    //    {
                    //        para.DbType = System.Data.DbType.Int32;
                    //    }
                    //    else if (item.MemberType == typeof(Double?))
                    //    {
                    //        para.DbType = System.Data.DbType.Double;
                    //    }
                    //}

                    if (paramValue == null || paramValue.ToString() == Guid.Empty.ToString())
                        para.Value = DBNull.Value;
                    lstParas.Add(para);
                }

               
                strQuery += ":PageIndex";
                para = new OracleParameter();
                para.ParameterName = "PageIndex";
                para.Value = model.PageIndex == 0 ? 1 : model.PageIndex;
                para.DbType = System.Data.DbType.Int32;
                para.OracleDbType = OracleDbType.Number;
                lstParas.Add(para);

                strQuery += ",:PageSize";
                para = new OracleParameter();
                para.ParameterName = "PageSize";
                para.Value = model.PageSize == 0 ? 50 : model.PageSize;
                para.DbType = System.Data.DbType.Int32;
                lstParas.Add(para);
                if (CheckStoreNamePermisson(storeName))
                {
                    strQuery += ",:UserName";
                    para = new OracleParameter();
                    para.ParameterName = "UserName";
                    para.Value = model.UserLogin;
                    para.DbType = System.Data.DbType.String;
                    para.OracleDbType = OracleDbType.NVarChar;
                    lstParas.Add(para);
                }

            }
            para = new OracleParameter();
            para.ParameterName = "R_Output";
            para.OracleDbType = OracleDbType.Cursor;
            para.Direction = System.Data.ParameterDirection.Output;
            lstParas.Add(para);
            ParamaterModle param = new ParamaterModle();
            param.Params = lstParas.ToArray();
            param.SqlQuery = strQuery + ",:R_Output); end;";
            return param;
        }

        public static ParamaterModle ParseParam(ListQueryModel model, string storeName)
        {
            if(Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                return ParseParamOracle(model, storeName);
            }
            else
            {
                return ParseParamSQL(model, storeName);
            }
        }

        public static bool CheckStoreNamePermisson(string storeName)
        {
            bool isCheck = false;
            if (!storeName.Contains("CAT_SP_GET_") && !storeName.Contains("SYS_SP_GET_") && !storeName.StartsWith("HRM_CAT_SP_GET_") && !storeName.Contains("SYS_SP_GET") && !storeName.Contains("CAT_GETDATA_") && (!storeName.StartsWith("hrm_") || !storeName.StartsWith("Hrm_")))
            {
                isCheck = true;
            }

            return isCheck;
        }
    }

}

