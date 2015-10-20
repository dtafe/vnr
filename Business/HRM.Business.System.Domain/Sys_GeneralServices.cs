using System;
using System.Collections.Generic;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using System.Linq;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using VnResource.Helper.Utility;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_GeneralServices : BaseService
    {
        public static List<EnumDataInfo> GetEnumData(Type enumType)
        {
            return EnumRepository.GetEnumDataInfo(enumType);
        }

    }
}
