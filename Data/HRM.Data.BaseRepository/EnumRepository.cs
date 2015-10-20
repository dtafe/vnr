using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Utility;

namespace HRM.Data.BaseRepository
{
    public class EnumRepository
    {
        public static List<EnumDataInfo> GetEnumDataInfo(Type enumType)
        {
            var listEnumDataInfo = Utilities.GetEnumDataInfo(enumType);

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                var listEnumName = unitOfWork.CreateQueryable<Sys_EnumFilter>(Guid.Empty,
                    d => d.EnumType == enumType.Name).Select(d => d.EnumName).ToList();

                if (listEnumName != null && listEnumName.Count() > 0)
                {
                    listEnumDataInfo = listEnumDataInfo.Where(d => !listEnumName.Contains(d.Name)).ToList();
                }

                foreach (var item in listEnumDataInfo)
                {
                    var enumValue = Enum.Parse(enumType, item.Name);
                    item.Translate = item.Name.TranslateString();
                }
            }

            return listEnumDataInfo;
        }
    }
}
