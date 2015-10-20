using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Category.Model;
using HRM.Business.Category.Domain;

namespace HRM.Business.Category.Domain.NUnitTest
{
    [TestFixture]
    public class Cat_OrgStructureServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Get()
        {
            Cat_OrgStructureServices service = new Cat_OrgStructureServices();
            IQueryable<Cat_OrgStructure> repo = service.GetCatOrgStructure();
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_GetById()
        {
            Cat_OrgStructureServices service = new Cat_OrgStructureServices();
            var model = new Cat_OrgStructure { Id = 3 };
            var result = service.GetByIdCatOrgStructure(model.Id) as Cat_OrgStructure;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.OrgStructureName
                + " | " + result.Code
                + " | " + result.IsRoot
                + " | " + result.Description
                + " | " + result.TypeID
                + " | " + result.ParentID
                + " | " + result.OrderNumber
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Add()
        {
            Cat_OrgStructureServices service = new Cat_OrgStructureServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Cat_OrgStructure
                {
                    OrgStructureName = "Phong Ban " + i,
                    Code = "Add Mã " + i,
                    IsRoot = true,
                    Description = "Mô Tả " + i,
                    TypeID = 1,
                    ParentID = 1,
                    OrderNumber = i
                };
                result = service.AddCatOrgStructure(model);
                NUnit.Framework.Assert.IsTrue(result);
                if (result == true)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.OrgStructureName
                        + " | " + model.Code
                        + " | " + model.IsRoot
                        + " | " + model.Description
                        + " | " + model.TypeID
                        + " | " + model.ParentID
                        + " | " + model.OrderNumber
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Edit()
        {
            Cat_OrgStructureServices service = new Cat_OrgStructureServices();
            var model = new Cat_OrgStructure
            {
                Id = 5,
                OrgStructureName = "Update Phòng Ban ",
                Code = "Update Mã ",
                IsRoot = true,
                Description = "Update Mô Tả ",
                TypeID = 1,
                ParentID = 1,
                OrderNumber = 321

            };
            bool result = service.UpdateCatOrgStructure(model);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.OrgStructureName
                        + " | " + model.Code
                        + " | " + model.IsRoot
                        + " | " + model.Description
                        + " | " + model.TypeID
                        + " | " + model.ParentID
                        + " | " + model.OrderNumber
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Remove()
        {
            Cat_OrgStructureServices service = new Cat_OrgStructureServices();
            int rs = 0;
            var model = new Cat_OrgStructure { Id = 3 };
            bool result = service.DeleteCatOrgStructure(model.Id);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }
    }
}
