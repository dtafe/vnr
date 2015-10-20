using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Utilities
{
    public class VNRException : System.Exception
    {
        public VNRException()
            : base()
        {

        }
        public VNRException(string message)
            : base(message)
        {

        }
        public VNRException(ExceptionType type, string message)
            : base(message)
        {
            ExceptionType = type;
        }
        public VNRException(ExceptionType type, string className, string message)
            : base(message)
        {
            ExceptionType = type;
            ClassName = className;
        }
        public VNRException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public VNRException(ExceptionType type, string message, Exception innerException)
            : base(message, innerException)
        {
            ExceptionType = type;
        }
        public VNRException(ExceptionType type, string message, string className, Exception innerException)
            : base(message, innerException)
        {
            ExceptionType = type;
            ClassName = className;
        }
        private ExceptionType _exceptionType = ExceptionType.NORMAL;
        public ExceptionType ExceptionType
        {
            get
            {
                return _exceptionType;
            }
            protected set
            {
                _exceptionType = value;
            }
        }
        private string strClassName = string.Empty;
        public string ClassName
        {
            get
            {
                return strClassName;
            }
            protected set
            {
                strClassName = value;
            }
        }
        public const string MoreThanOneElements = "More than one elements";
        public const string HaventLoadAppConfigsYet = "Haven't load app configs yet!";
        public const string InvalidUser = "Invalid user!";
        public const string InvalidQueryString = "Invalid query string!";
        public const string NullReferenceException = "Object can't be null!";
        public const string DoNotCompareTwoObjectsWithDifferentType = "Do not compare two objects with different type!";
        public const string MultiEditObjectsMustHaveMoreThanOneObject = "Multi edit objects must have more than one object!";
        public const string DataSourceMustBeAListData = "DataSource must be a list of data.";
    }
}
