using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.BaseRepository
{
    public class DataError
    {
        public DataError()
        {
        }
        private string _collumName = null;
        public string CollumName
        {
            get
            {
                return _collumName;
            }
            set
            {
                _collumName = value;
            }
        }
        private string _tableName = null;
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }
        private DataErrorCode _dataErrorCode = DataErrorCode.Unknown;
        public DataErrorCode DataErrorCode
        {
            get
            {
                return _dataErrorCode;
            }
            set
            {
                _dataErrorCode = value;
            }
        }
    }
}
