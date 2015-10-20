using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;
using System.Collections.Generic;
using System;
using HRM.Business.HrmSystem.Models;
using System.Data;
using System.Data.OleDb;
using HRM.Business.Main.Domain;
using VnResource.Helper.Data;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using VnResource.Helper.Linq;

namespace HRM.Business.Canteen.Domain
{
    public class Can_TamServices : BaseService
    {
        #region CreateComputingTask

        public Guid CreateComputingTask(Guid userID, DateTime dateFrom, DateTime dateTo)
        {
            #region Khởi tạo Sys_AsynTask cho mỗi lần xử lý

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                Sys_AsynTask task = new Sys_AsynTask();
                task.ID = Guid.NewGuid();
                task.PercentComplete = 0.01D;
                task.TimeStart = DateTime.Now;
                task.Status = AsynTaskStatus.Doing.ToString();
                task.Type = AsynTask.Download_TimeLog_CMS.ToString();
                task.Summary = "CMS TimeLog: " + dateFrom.ToString("dd/MM/yyyy") + " - " + dateTo.ToString("dd/MM/yyyy");
                unitOfWork.AddObject(typeof(Sys_AsynTask), task);
                unitOfWork.SaveChanges(userID);
                return task.ID;
            }

            #endregion
        }

        public void CompleteComputingTask(Guid asynTaskID, Guid userID,
            int totalComputed, int totalProfile, DataErrorCode dataErrorCode)
        {
            #region Lưu Sys_AsynTask khi xử lý xong

            if (asynTaskID != Guid.Empty)
            {
                using (var taskContext = new VnrHrmDataContext())
                {
                    IUnitOfWork taskunitOfWork = new UnitOfWork(taskContext);
                    var asynTask = taskunitOfWork.CreateQueryable<Sys_AsynTask>(s => s.ID == asynTaskID).FirstOrDefault();

                    if (asynTask != null)
                    {
                        asynTask.PercentComplete = 1D;
                        asynTask.TimeEnd = DateTime.Now;
                        asynTask.Status = AsynTaskStatus.Done.ToString();

                        var time = asynTask.TimeEnd.Value.Subtract(asynTask.TimeStart).TotalMinutes;
                        asynTask.Description += " - Result: " + totalComputed + "/" + totalProfile;
                        asynTask.Description += " - Time: " + time.ToString("N2");

                        if (dataErrorCode == DataErrorCode.Locked)
                        {
                            asynTask.PercentComplete = 1D;//Không cần nhân với 100
                            asynTask.Description = "Dữ Liệu Quẹt Thẻ Đã Bị Khóa";
                        }

                        dataErrorCode = taskunitOfWork.SaveChanges();
                    }
                }
            }

            #endregion
        }

        #endregion

        #region GetCardCodeFilter

        public bool IsConnected(AppConfig tamConfig, ref string serverTamLogConfig)
        {
            return IsConnected(tamConfig, false, ref serverTamLogConfig);
        }

        public bool IsConnected(AppConfig tamConfig, bool checkState, ref string serverTamLogConfig)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                string settingkey = tamConfig.ToString();

                var listSetting = unitOfWork.CreateQueryable<Sys_AllSetting>(Guid.Empty,
                    d => d.Name.Contains(settingkey)).ToArray();

                string serverName = listSetting.Where(m => m.Name == settingkey + AppConfig.SERVERNAME).Select(m => m.Value1).FirstOrDefault();
                string userName = listSetting.Where(m => m.Name == settingkey + AppConfig.USERID).Select(m => m.Value1).FirstOrDefault();
                string password = listSetting.Where(m => m.Name == settingkey + AppConfig.PASSWORD).Select(m => m.Value1).FirstOrDefault();
                string dbName = listSetting.Where(m => m.Name == settingkey + AppConfig.DBNAME).Select(m => m.Value1).FirstOrDefault();
                string isActivated = listSetting.Where(m => m.Name == settingkey + AppConfig.ISACTIVATED).Select(m => m.Value1).FirstOrDefault();

                string tableName = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.TABLENAME).Select(m => m.Value1).FirstOrDefault();
                string dataColName = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.DATACOLNAME).Select(m => m.Value1).FirstOrDefault();
                string cardColName = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.CARDCOLNAME).Select(m => m.Value1).FirstOrDefault();
                string typeColName = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.TYPE).Select(m => m.Value1).FirstOrDefault();

                string filterColumn = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.FILTERDATA).Select(m => m.Value1).FirstOrDefault();
                string filterData = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.FILTERDATASPLIT).Select(m => m.Value1).FirstOrDefault();
                string machineCode = listSetting.Where(m => m.Name == tamConfig.ToString() + AppConfig.MACHINECODE).Select(m => m.Value1).FirstOrDefault();
                string isByCodeEmp = listSetting.Where(m => m.Name == settingkey + AppConfig.ISCODEEMP).Select(m => m.Value1).FirstOrDefault();

                if (listSetting != null && listSetting.Count() > 0)
                {
                    string connectString = "provider=SQLOLEDB;DATA SOURCE=" + serverName + ";Initial Catalog="
                        + dbName + ";USER ID=" + userName + ";Password=" + password + ";";

                    try
                    {
                        if (!checkState || isActivated == bool.TrueString)
                        {
                            System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection();
                            cn.ConnectionString = connectString;
                            cn.Open();
                            cn.Close();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }

                    if (isActivated == bool.TrueString)
                    {
                        serverTamLogConfig = connectString + "|"
                                    + tableName + "|"
                                    + dataColName + "|"
                                    + cardColName + "|"
                                    + typeColName + "|"
                                    + filterColumn + "|"
                                    + filterData + "|"
                                    + machineCode + "|"
                                    + isByCodeEmp;
                    }

                    return true;
                }

                return false;
            }
        }

        public bool IsConnected(string serverName, string userId, string password, string dbName, ref string outputConnectString)
        {
            string connectString = "provider=SQLOLEDB;DATA SOURCE=" + serverName + ";Initial Catalog="
                + dbName + ";USER ID=" + userId + ";Password=" + password + ";";

            try
            {
                System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection();
                cn.ConnectionString = connectString;
                cn.Open();
                cn.Close();
                cn.Dispose();
            }
            catch (System.Exception)
            {
                return false;
            }

            outputConnectString = connectString;
            return true;
        }

        public List<Hre_CardHistoryEntity> GetCardCodeByProfile(List<Hre_CardHistoryEntity> listCardHistory,
            Hre_ProfileEntity profile, DateTime from, DateTime to)
        {
            List<Hre_CardHistoryEntity> lstCardCode = new List<Hre_CardHistoryEntity>();

            if (profile != null)
            {
                var listCardHistoryByProfile = listCardHistory.Where(his => his.ProfileID == profile.ID
                    && his.DateEffect <= to).OrderByDescending(his => his.DateEffect).ToList();

                //Chay tu ngay hieu luc lon nhat den ngay hieu luc nho nhat           
                foreach (var card in listCardHistoryByProfile)
                {
                    if (card.DateEffect <= from)
                    {
                        lstCardCode.Add(card);
                        break;
                    }
                    else
                    {
                        lstCardCode.Add(card);
                    }
                }
            }

            return lstCardCode;
        }

        private string GetColumeDataFilter(string columeFilter, string dataFilter)
        {
            string result = string.Empty;
            char[] ext = new char[] { ',', ';', '|' };

            List<string> lstDataFilter = dataFilter.Split(ext,
                StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var item in lstDataFilter)
            {
                if (item == string.Empty)
                    continue;

                result += columeFilter + " = '" + item + "' or ";
            }

            if (result != string.Empty && result.Length > 4)
            {
                result = result.Substring(0, result.Length - 4);
            }

            if (!string.IsNullOrWhiteSpace(result))
            {
                result = " and (" + result + ")";
            }

            return result;
        }

        private string GetCardCodeFilter(List<Guid> listProfileID, DateTime dateFrom,
            DateTime dateTo, string cardColName, bool filterByCodeEmp, bool isExcept)
        {
            string strReturn = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                if (listProfileID != null)
                {
                    var listCardHistory = unitOfWork.CreateQueryable<Hre_CardHistory>(Guid.Empty, d => d.DateEffect <= dateTo
                        && d.ProfileID.HasValue && listProfileID.Contains(d.ProfileID.Value)).Select(d => new
                        {
                            d.ProfileID,
                            d.CardCode,
                            d.DateEffect
                        }).ToList();

                    var listProfile = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty,
                        d => listProfileID.Contains(d.ID)).Select(d => new
                        {
                            d.ID,
                            d.CodeEmp
                        }).ToList();

                    foreach (var profile in listProfile)
                    {
                        if (!filterByCodeEmp)
                        {
                            //Vinhtran - 20141219 - 0036493: Không phân biệt hoa thường
                            var listCardHistoryByProfile = listCardHistory.Where(d => d.ProfileID
                                == profile.ID).OrderByDescending(d => d.DateEffect).ToList();

                            foreach (var profileCard in listCardHistoryByProfile)
                            {
                                if (string.IsNullOrEmpty(strReturn))
                                {
                                    if (isExcept)
                                        strReturn = "UPPER(" + cardColName + ") <> UPPER('" + profileCard.CardCode.GetString().Trim() + "')";
                                    else
                                        strReturn = "UPPER(" + cardColName + ") = UPPER('" + profileCard.CardCode.GetString().Trim() + "')";
                                }
                                else
                                {
                                    if (isExcept)
                                        strReturn += " and UPPER(" + cardColName + ") <> UPPER('" + profileCard.CardCode.GetString().Trim() + "')";
                                    else
                                        strReturn += " or UPPER(" + cardColName + ") = UPPER('" + profileCard.CardCode.GetString().Trim() + "')";
                                }

                                if (profileCard.DateEffect <= dateFrom)
                                {
                                    //Chỉ lấy thẻ hiệu lực theo thời gian chọn
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strReturn))
                            {
                                if (isExcept)
                                    strReturn = "UPPER(" + cardColName + ") <> UPPER('" + profile.CodeEmp.GetString().Trim() + "')";
                                else
                                    strReturn = "UPPER(" + cardColName + ") = UPPER('" + profile.CodeEmp.GetString().Trim() + "')";
                            }
                            else
                            {
                                if (isExcept)
                                    strReturn += " and UPPER(" + cardColName + ") <> UPPER('" + profile.CodeEmp.GetString().Trim() + "')";
                                else
                                    strReturn += " or UPPER(" + cardColName + ") = UPPER('" + profile.CodeEmp.GetString().Trim() + "')";
                            }
                        }
                    }
                }
            }

            return strReturn;
        }

        #endregion

        #region AutoSyncTAMLog

        public void SyncTAMLog(Guid userID, Guid taskID, bool isExcept, DateTime dateFrom, DateTime dateTo,
            List<Guid> listOrgStructureID, List<Guid> listPayrollGroupID, List<Guid> listWorkplaceID)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();

                var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(userID);

                if (listOrgStructureID != null && listOrgStructureID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.OrgStructureID.HasValue
                        && listOrgStructureID.Contains(d.OrgStructureID.Value));
                }

                if (listPayrollGroupID != null && listPayrollGroupID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.PayrollGroupID.HasValue
                        && listPayrollGroupID.Contains(d.PayrollGroupID.Value));
                }

                if (listWorkplaceID != null && listWorkplaceID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.WorkPlaceID.HasValue
                        && listWorkplaceID.Contains(d.WorkPlaceID.Value));
                }

                var listProfileID = profileQueryable.Where(d => (d.StatusSyn == null || d.StatusSyn != waitStatus)
                    && (d.DateQuit == null || d.DateQuit.Value > dateFrom)).Select(d => d.ID).ToList();

                SyncTAMLog(userID, taskID, isExcept, dateFrom, dateTo, listProfileID);
            }
        }

        public void SyncTAMLog(Guid userID, Guid taskID, bool isExcept,
            DateTime dateFrom, DateTime dateTo, List<Guid> listProfileID)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                Sys_AsynTask asynTask = null;
                int pageSize = 200;

                if (taskID != Guid.Empty)
                {
                    asynTask = unitOfWork.CreateQueryable<Sys_AsynTask>(Guid.Empty,
                        d => d.ID == taskID).FirstOrDefault();
                }

                if (listProfileID != null && listProfileID.Count() > 0)
                {
                    double totalComputed = 0;

                    foreach (var listSplitProfileID in listProfileID.Chunk(pageSize))
                    {
                        string serverTamLogConfig1 = string.Empty;
                        string serverTamLogConfig2 = string.Empty;
                        totalComputed += listSplitProfileID.Count();

                        if (IsConnected(AppConfig.HRM_SYS_CAN_TAMSCANLOG_1_, true, ref serverTamLogConfig1))
                        {
                            SyncTAMLog(userID, isExcept, dateFrom, dateTo, serverTamLogConfig1, listSplitProfileID.ToList());
                        }

                        if (IsConnected(AppConfig.HRM_SYS_CAN_TAMSCANLOG_2_, true, ref serverTamLogConfig2))
                        {
                            //Nếu có sử dụng 2 database chấm công thì đẩy dữ liệu vào cả 2
                            SyncTAMLog(userID, isExcept, dateFrom, dateTo, serverTamLogConfig2, listSplitProfileID.ToList());
                        }

                        if (asynTask != null)
                        {
                            asynTask.PercentComplete = totalComputed / (double)listProfileID.Count;
                            unitOfWork.SaveChanges(userID);
                        }
                    }
                }

                if (asynTask != null)
                {
                    asynTask.Status = AsynTaskStatus.Done.ToString();
                    asynTask.PercentComplete = 1D;//Không cần nhân với 100
                    asynTask.TimeEnd = unitOfWork.CurrentDate();
                    unitOfWork.SaveChanges(userID);
                }
            }
        }

        private void SyncTAMLog(Guid userID, bool isExcept, DateTime dateFrom,
            DateTime dateTo, string serverTamLogConfig, List<Guid> listProfileID)
        {
            OleDbConnection dbConnection = null;

            try
            {
                string[] strs = serverTamLogConfig.Split(new string[] 
                { 
                    "|" 
                }, StringSplitOptions.None);

                if (strs == null || strs.Count() < 0)
                {
                    return;
                }

                dbConnection = new OleDbConnection(strs[0]);
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                string tableName = strs[1];
                string dataColName = strs[2];
                string cardColName = strs[3];
                string columeFilter = string.Empty;
                string dataFilter = string.Empty;
                string serialNumber = string.Empty;
                string isByCodeEmp = string.Empty;

                try
                {
                    columeFilter = strs[5];
                    dataFilter = strs[6];
                    serialNumber = strs[7];
                    isByCodeEmp = strs[8];
                }
                catch
                {

                }

                String AdvFilter = String.Empty;
                bool filterByCodeEmp = false;

                if (isByCodeEmp == Boolean.TrueString)
                {
                    filterByCodeEmp = true;
                }

                String filterString = GetCardCodeFilter(listProfileID, dateFrom,
                    dateTo, cardColName, filterByCodeEmp, isExcept);

                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    AdvFilter += " And (" + filterString + ")";
                }

                if (columeFilter != string.Empty && dataFilter != string.Empty)
                {
                    AdvFilter = GetColumeDataFilter(columeFilter, dataFilter);
                }

                string sqlConString = string.Empty;

                string typeColName = strs[4];
                string fields = string.Empty;

                if (!string.IsNullOrWhiteSpace(typeColName))
                {
                    fields += "," + typeColName;
                }

                if (!string.IsNullOrWhiteSpace(serialNumber))
                {
                    if (string.IsNullOrWhiteSpace(typeColName))
                    {
                        fields += ",'' As Type";
                    }

                    fields += "," + serialNumber;
                }

                sqlConString = String.Format("Select {0},{1}{2} from {3} where {4} >= ? and {5} <= ? {6} Order by {7} asc, {8} asc",
                    new string[] { cardColName, dataColName, fields, tableName, dataColName, dataColName, AdvFilter, cardColName, dataColName });

                OleDbCommand command = null;
                command = new OleDbCommand(sqlConString, dbConnection);
                command.CommandType = CommandType.Text;

                OleDbParameter paramFrom = new OleDbParameter("From", OleDbType.Date);
                paramFrom.Value = dateFrom;
                command.Parameters.Add(paramFrom);

                OleDbParameter paramTo = new OleDbParameter("To", OleDbType.Date);
                paramTo.Value = dateTo;
                command.Parameters.Add(paramTo);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                var listTimeLog = SaveTAMLog(ds.Tables[0], dateFrom, dateTo, userID, listProfileID, filterByCodeEmp);
                var timeLog = listTimeLog.Select(d => d.TimeLog).FirstOrDefault();
                dateTo = timeLog.HasValue && timeLog < dateTo ? timeLog.Value : dateTo;

                dbConnection.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Can_TamScanLogCMS> SaveTAMLog(DataTable table, DateTime dateFrom,
            DateTime dateTo, Guid userID, List<Guid> listProfileID, bool filterByCodeEmp)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                if (table.Columns.Count == 2)
                {
                    table.Columns.Add();
                    table.Columns.Add();
                }
                else if (table.Columns.Count == 3)
                {
                    table.Columns.Add();
                }

                DataRow[] rows = new DataRow[table.Rows.Count];
                table.Rows.CopyTo(rows, 0);

                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                List<Can_TamScanLogCMS> listTAMScanLog = new List<Can_TamScanLogCMS>();

                var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(userID, pro => (pro.StatusSyn == null
                    || pro.StatusSyn != waitStatus) && (pro.DateQuit == null || pro.DateQuit.Value > dateFrom));

                var cardHistoryQueryable = unitOfWork.CreateQueryable<Hre_CardHistory>(userID);

                if (listProfileID != null && listProfileID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(pro => listProfileID.Contains(pro.ID));
                    cardHistoryQueryable = cardHistoryQueryable.Where(pro => pro.ProfileID.HasValue && listProfileID.Contains(pro.ProfileID.Value));
                }
                else
                {
                    cardHistoryQueryable = cardHistoryQueryable.Where(cr => cr.Hre_Profile != null
                        && (!cr.Hre_Profile.IsDelete.HasValue || cr.Hre_Profile.IsDelete == false));
                }

                var listAllProfile = profileQueryable.Select(d =>
                    new Hre_ProfileEntity
                    {
                        ID = d.ID,
                        CodeEmp = d.CodeEmp
                    }).ToList<Hre_ProfileEntity>();

                var listAllCardHistory = cardHistoryQueryable.Select(d =>
                    new Hre_CardHistoryEntity
                    {
                        ID = d.ID,
                        ProfileID = d.ProfileID,
                        CardCode = d.CardCode,
                        DateEffect = d.DateEffect
                    }).ToList<Hre_CardHistoryEntity>();

                for (int i = 0; i < rows.Length; i++)
                {
                    DataRow row = rows[i];
                    string cardCode = row[0].ToString();
                    cardCode = cardCode.ToUpper().Trim();

                    Can_TamScanLogCMS tam = new Can_TamScanLogCMS();

                    if (filterByCodeEmp)
                    {
                        tam.CodeEmp = cardCode;
                        tam.Comment = "Hệ thống cũ";
                    }
                    else
                    {
                        tam.CardCode = cardCode;
                    }

                    tam.TimeLog = Convert.ToDateTime(row[1].ToString().Trim());
                    tam.Type = row[2].ToString().Trim();
                    tam.MachineCode = row[3].ToString().Trim();
                    tam.Status = TAMScanStatus.E_LOADED.ToString();
                    List<Hre_ProfileEntity> listProfileByCode = null;

                    if (!string.IsNullOrWhiteSpace(tam.CardCode))
                    {
                        var listCardHistoryByCode = listAllCardHistory.Where(ch => ch.CardCode != null
                            && ch.CardCode.ToString().ToUpper().Trim() == cardCode).ToList();

                        if (listCardHistoryByCode.Count() <= 0)
                        {
                            listProfileByCode = listAllProfile.Where(d => d.CodeAttendance != null
                                && d.CodeAttendance.ToString().ToUpper().Trim() == cardCode).ToList();
                        }
                        else
                        {
                            var listProfileIDByCode = listCardHistoryByCode.Select(d => d.ProfileID).ToList();
                            listProfileByCode = listAllProfile.Where(d => listProfileIDByCode.Contains(d.ID)).ToList();
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(tam.CodeEmp))
                    {
                        listProfileByCode = listAllProfile.Where(d => d.CodeEmp != null
                            && d.CodeEmp.ToString().ToUpper().Trim() == cardCode).ToList();
                    }

                    if (listProfileByCode != null && listProfileByCode.Count > 0)
                    {
                        var dateStart = tam.TimeLog.HasValue ? tam.TimeLog.Value.Date : dateFrom;
                        var dateEnd = tam.TimeLog.HasValue ? tam.TimeLog.Value.Date.AddDays(1) : dateTo;
                        var rec = ConvertToRecord(tam, listProfileByCode, listAllCardHistory, dateStart, dateEnd);

                        if (rec != null)
                        {
                            listTAMScanLog.Add(rec);
                        }
                    }
                }

                DeleteTAMLog(unitOfWork, dateFrom, dateTo, listTAMScanLog, userID);
                unitOfWork.AddObject(listTAMScanLog.ToArray());
                unitOfWork.SaveChanges(userID);
                return listTAMScanLog;
            }
        }

        private Can_TamScanLogCMS ConvertToRecord(Can_TamScanLogCMS tam, List<Hre_ProfileEntity> listProfile,
            List<Hre_CardHistoryEntity> listCardHistory, DateTime from, DateTime to)
        {
            if (tam.CardCode == null && tam.CodeEmp == null)
                return null;

            string CardCode = tam.CardCode.GetString().Trim();
            string CodeEmp = tam.CodeEmp.GetString().Trim();

            if (!string.IsNullOrWhiteSpace(CodeEmp))
            {
                var profile = listProfile.Where(d => d.CodeEmp == CodeEmp).FirstOrDefault();

                if (profile != null)
                {
                    tam.ProfileID = profile.ID;
                    return tam;
                }
            }
            else
            {
                DateTime dateMin = DateTime.MinValue;
                Boolean check = false;

                foreach (var profile in listProfile)
                {
                    if (!string.IsNullOrWhiteSpace(CardCode))
                    {
                        var lstCardHistoryByProfile = GetCardCodeByProfile(listCardHistory, profile, from, to);
                        lstCardHistoryByProfile = lstCardHistoryByProfile.OrderBy(card => card.DateEffect).ToList();

                        foreach (var item in lstCardHistoryByProfile)
                        {
                            //ma cham cong History truoc do cua nhan vien khac. Quet the chi lay toi ngay hieu luc cua nhan vien do thoi
                            var cardHistoryByCard = listCardHistory.Where(card => card.DateEffect > item.DateEffect && card.CardCode != null
                                && card.CardCode.ToString().Trim() == item.CardCode).OrderBy(card => card.DateEffect).FirstOrDefault();

                            //Kiem tra khong duoc lay qua ngay hieu luc cua nhan vien khac cung su dung ma the do.
                            if (item.DateEffect.HasValue && item.DateEffect >= dateMin && item.DateEffect <= tam.TimeLog
                                && CardCode == item.CardCode.ToString().Trim() && (cardHistoryByCard == null
                                || tam.TimeLog < cardHistoryByCard.DateEffect))
                            {
                                dateMin = item.DateEffect.Value;
                                tam.ProfileID = profile.ID;
                                check = true;
                                break;
                            }
                        }
                    }

                    if (check)
                    {
                        break;
                    }
                }

                if (check)
                {
                    return tam;
                }
            }

            return null;
        }

        private void DeleteTAMLog(IUnitOfWork unitOfWork, DateTime dateFrom, DateTime dateTo,
            List<Can_TamScanLogCMS> listTAMScanLog, Guid userID)
        {
            //Honda dùng 2 db chấm công nên chỉ xóa dữ liệu trùng
            Boolean checkDeleteIdentical = true;

            List<Can_TamScanLogCMS> lstDelScan = new List<Can_TamScanLogCMS>();
            String status = TAMScanStatus.E_LOADED.ToString();

            List<Can_TamScanLogCMS> lstScanDB = unitOfWork.CreateQueryable<Can_TamScanLogCMS>(Guid.Empty,
                log => log.TimeLog >= dateFrom && log.TimeLog <= dateTo && log.Status == status).ToList();

            if (checkDeleteIdentical)
            {
                lstDelScan = lstScanDB.Where(del => listTAMScanLog.Any(tam => del.TimeLog == tam.TimeLog
                    && ((!string.IsNullOrWhiteSpace(tam.CardCode) && del.CardCode == tam.CardCode)
                    || (!string.IsNullOrWhiteSpace(tam.CodeEmp) && del.CodeEmp == tam.CodeEmp)))).ToList();
            }
            else
            {
                var lstCardCode = listTAMScanLog.Where(d => !string.IsNullOrWhiteSpace(d.CardCode)).Select(d => d.CardCode).Distinct().ToList();
                var lstCodeEmp = listTAMScanLog.Where(d => !string.IsNullOrWhiteSpace(d.CodeEmp)).Select(d => d.CodeEmp).Distinct().ToList();
                lstDelScan = lstScanDB.Where(del => lstCardCode.Contains(del.CardCode) || lstCodeEmp.Contains(del.CodeEmp)).ToList();
            }

            unitOfWork.RemoveObject(lstDelScan.ToArray());
        }

        #endregion
    }
}
