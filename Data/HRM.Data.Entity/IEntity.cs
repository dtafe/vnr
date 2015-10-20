using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.Entity
{
    public interface IEntity
    {
        Guid ID
        {
            get;
            set;
        }

        Guid? UserLockID
        {
            get;
            set;
        }
        DateTime? DateLock
        {
            get;
            set;
        }

        bool? IsDelete
        {
            get;
            set;
        }
    }
}
