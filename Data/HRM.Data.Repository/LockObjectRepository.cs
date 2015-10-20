using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.Entity.Repositories
{
    public class LockObjectRepository
    {
        private Dictionary<string, LockObjectInfo> _lockObject;
        private Dictionary<string, LockObjectInfo> LockObjects
        {
            get
            {
                if (_lockObject == null)
                {
                    _lockObject = new Dictionary<string, LockObjectInfo>();
                }
                return _lockObject;
            }
        }

        private string LockObjectPath
        {
            get
            {
                return Common.GetPath("Setting\\LockObjects.xml");
            }
        }

        public LockObjectInfo GetLockObjectInfo(Type entityType)
        {
            if (entityType == null)
                return null;

            LockObjectInfo info = null;

            if (LockObjects.Count() <= 0)
            {
                ReadLockObjects();
            }

            if (LockObjects.ContainsKey(entityType.Name))
            {
                info = LockObjects[entityType.Name];
            }

            return info;
        }

        private void ReadLockObjects()
        {
            if (File.Exists(LockObjectPath))
            {
                DataSet dsLockObject = new DataSet();
                dsLockObject.ReadXml(LockObjectPath);

                if (dsLockObject.Tables[LockObjectInfo.FieldNames.FieldInfo] != null)
                {
                    foreach (DataRow item in dsLockObject.Tables[LockObjectInfo.FieldNames.FieldInfo].Rows)
                    {
                        if (item.Table.Columns.Contains(LockObjectInfo.FieldNames.TableName))
                        {
                            object tableName = item[LockObjectInfo.FieldNames.TableName];

                            if (tableName != null && !string.IsNullOrEmpty(tableName.ToString()))
                            {
                                LockObjectInfo info = new LockObjectInfo();
                                info.TableName = tableName.ToString();

                                if (item.Table.Columns.Contains(LockObjectInfo.FieldNames.Type))
                                {
                                    object checkType = item[LockObjectInfo.FieldNames.Type];
                                    if (checkType != null && !string.IsNullOrEmpty(checkType.ToString()))
                                    {
                                        info.Type = (CheckLockType)Enum.Parse(typeof(CheckLockType), checkType.ToString());
                                    }
                                }

                                if (item.Table.Columns.Contains(LockObjectInfo.FieldNames.FieldName))
                                {
                                    object fieldName = item[LockObjectInfo.FieldNames.FieldName];
                                    if (fieldName != null && !string.IsNullOrEmpty(fieldName.ToString()))
                                    {
                                        info.FieldName = fieldName.ToString();
                                    }
                                }

                                if (item.Table.Columns.Contains(LockObjectInfo.FieldNames.FieldStart))
                                {
                                    object startField = item[LockObjectInfo.FieldNames.FieldStart];
                                    if (startField != null && !string.IsNullOrEmpty(startField.ToString()))
                                    {
                                        info.FieldStart = startField.ToString();
                                    }
                                }
                                if (item.Table.Columns.Contains(LockObjectInfo.FieldNames.FieldEnd))
                                {
                                    object endField = item[LockObjectInfo.FieldNames.FieldEnd];

                                    if (endField != null && !string.IsNullOrEmpty(endField.ToString()))
                                    {
                                        info.FieldEnd = endField.ToString();
                                    }
                                }

                                if (!LockObjects.ContainsKey(tableName.ToString()))
                                {
                                    LockObjects.Add(tableName.ToString(), info);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
