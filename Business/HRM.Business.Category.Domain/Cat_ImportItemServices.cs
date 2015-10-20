using System.Collections.Generic;
using HRM.Data.BaseRepository;

using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Data.Main.Data.Sql;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using VnResource.Helper.Utility;
namespace HRM.Business.Category.Domain
{
    public class Cat_ImportItemServices : BaseService
    {
        /// <summary>
        /// Lấy danh sách các field thuộc 1 bảng (sử dụng Entity)
        /// </summary>
        /// <param name="objectNameRoot">Bảng gốc</param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public List<string> GetTableNames(string objectNameRoot, string objectName)
        {
            if (!string.IsNullOrEmpty(objectNameRoot) && objectNameRoot != objectName)
            {
                var test = LibraryService.GetEntityType(objectNameRoot);
                var testType = test.GetProperty(objectName);
                objectName = testType.PropertyType.Name;
            }

            var objectNameType = LibraryService.GetEntityType(objectName);
            //Chỉ lấy những properties ko phải ICollection`1 (những foreignkey trỏ đến bảng hiện tại {bảng [objectName]})
            var propertyTypeName = "ICollection`1";
            var properties = objectNameType.GetProperties()
                .Where(p => p.PropertyType.Name != propertyTypeName)
                .Select(d => d.Name)
                .OrderBy(d => d)
                .ToList();
            return properties;
        }
    }
}
