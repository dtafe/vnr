using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VnResource.Helper.Ado;
using VnResource.Helper.Data;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_SqlCommanderServices : BaseService
    {
        public DataTable ExecuteCommand(Guid userID, string commandText,
            int pageIndex, int pageSize, out int totalRow)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable result = new DataTable("Sys_SQLCommanderModel");
                var connection = context.Database.Connection;

                using (DbCommander commander = new DbCommander(connection))
                {
                    if (pageIndex >= 0 && pageSize > 0)
                    {
                        var fromIndex = commandText.ToLower().IndexOf("from");
                        var countCommand = commandText.Substring(fromIndex);
                        countCommand = "Select Count(*) " + countCommand;
                        var rowCount = commander.ExecuteScalar(countCommand);
                        totalRow = rowCount.TryGetValue<int>();

                        int startRow = pageIndex * pageSize;
                        commander.Fill(result, startRow, pageSize, commandText);
                    }
                    else
                    {
                        commander.Fill(result, commandText);
                        totalRow = result.Rows.Count;
                    }

                    if ((result.Columns.Contains(Constant.GrossAmount) && result.Columns[Constant.GrossAmount].DataType == typeof(string))
                        || (result.Columns.Contains(Constant.Amount) && result.Columns[Constant.Amount].DataType == typeof(string)))
                    {
                        foreach (DataRow row in result.Rows)
                        {
                            try
                            {
                                if (result.Columns.Contains(Constant.Amount) 
                                    && result.Columns[Constant.Amount].DataType == typeof(string))
                                {
                                    if (!string.IsNullOrWhiteSpace(row[Constant.Amount].GetString()))
                                    {
                                        var grossAmount = row[Constant.Amount].GetString();
                                        row[Constant.Amount] = grossAmount.Decrypt();
                                    }
                                }

                                if (result.Columns.Contains(Constant.GrossAmount) 
                                    && result.Columns[Constant.GrossAmount].DataType == typeof(string))
                                {
                                    if (!string.IsNullOrWhiteSpace(row[Constant.GrossAmount].GetString()))
                                    {
                                        var grossAmount = row[Constant.GrossAmount].GetString();
                                        row[Constant.GrossAmount] = grossAmount.Decrypt();
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }

                return result;
            }
        }
        public string ExecuteStringCommand(string strCommand)
        {
            using (var context = new VnrHrmDataContext())
            {
                var connection = context.Database.Connection;
                using (DbCommander commander = new DbCommander(connection))
                {
                    try
                    {
                        commander.ExecuteNonQuery(strCommand);
                    }
                    catch (Exception e)
                    {
                        if (!e.ToString().Contains("There is already an object named") && !e.ToString().Contains("name is already used by an existing object"))
                            return e.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}
