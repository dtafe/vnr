using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Ado;

namespace HRM.Data.Repository
{
    public class SyncDataRepository
    {
        public void SyncCardHistory()
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                string syncType = SyncDataType.Sync_Card_History.ToString();

                var listSyncInfo = unitOfWork.CreateQueryable<Cat_Sync>(d => d.Code == syncType
                    && (!d.IsFromInner.HasValue || !d.IsFromInner.Value)).Select(d => new
                    {
                        d.ID,
                        d.Code,
                        d.SyncName,
                        d.ServerName,
                        d.UserName,
                        d.PassWord,
                        d.DatabaseName,
                        d.InnerTable,
                        d.OuterTable,
                        d.IsFromInner
                    }).ToList();

                var listSyncID = listSyncInfo.Select(d => d.ID).ToArray();

                var listSyncItemInfo = unitOfWork.CreateQueryable<Cat_SyncItem>(d =>
                    d.SyncID.HasValue && listSyncID.Contains(d.SyncID.Value)).Select(d => new
                    {
                        d.IsExcluded,
                        d.InnerField,
                        d.OuterField,
                        d.AllowNull,
                        d.AllowDuplicate,
                        d.DuplicateGroup,
                        d.FilterValues,
                        d.SyncID
                    }).ToList();

                foreach (var syncInfo in listSyncInfo)
                {
                    string connectionString = DbClientHelper.BuildSqlConnectionString(syncInfo.ServerName,
                        syncInfo.DatabaseName, syncInfo.UserName, syncInfo.PassWord);

                    var syncItems = listSyncItemInfo.Where(d => d.SyncID == syncInfo.ID);
                    SqlConnection connection = new SqlConnection(connectionString);

                    using (DbCommander commander = new DbCommander(connection))
                    {

                    }
                }
            }
        }

        public void SyncProfileQuit()
        {

        }
    }
}
