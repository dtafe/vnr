using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.BaseRepository
{
    public class LockObjectRepository : VnResource.Helper.Setting.LockingManager
    {
        public override string SettingPath
        {
            get
            {
                base.SettingPath = Common.GetPath("Settings\\");
                return base.SettingPath;
            }
            set
            {
                base.SettingPath = value;
            }
        }
    }
}
