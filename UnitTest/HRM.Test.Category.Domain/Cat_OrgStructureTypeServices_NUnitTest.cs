using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Category.Model;
using HRM.Business.Category.Domain;

namespace HRM.Business.Category.Domain.NUnitTest
{
    [TestFixture]
    public class Cat_OrgStructureTypeServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Get()
        {
            Cat_OrgStructureTypeServices service = new Cat_OrgStructureTypeServices();
            IQueryable<Cat_OrgStructureType> repo = service.GetCatOrgStructureType();
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_GetById()
        {
            Cat_OrgStructureTypeServices service = new Cat_OrgStructureTypeServices();
            var model = new Cat_OrgStructureType { Id = 3 };
            var result = service.GetByIdCatOrgStructureType(model.Id) as Cat_OrgStructureType;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.OvertimeTypeName
                + " | " + result.Code
                + " | " + result.OrgStructureID
                + " | " + result.Comment
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Add()
        {
            Cat_OrgStructureTypeServices service = new Cat_OrgStructureTypeServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Cat_OrgStructureType
                {
                    OvertimeTypeName = "Add Tên Loại OT " + i,
                    Code = "Add Mã " + i,
                    OrgStructureID = 1,
                    Comment = "Add Comment " + i
                };
                result = service.AddCatOrgStructureType(model);
                NUnit.Framework.Assert.IsTrue(result);
                if (result == true)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.OvertimeTypeName
                        + " | " + model.Code
                        + " | " + model.OrgStructureID
                        + " | " + model.Comment
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
            Cat_OrgStructureTypeServices service = new Cat_OrgStructureTypeServices();
            var model = new Cat_OrgStructureType
            {
                Id = 3,
                OvertimeTypeName = "Update Tên Loại OT ",
                Code = "Update Mã ",
                OrgStructureID = 1,
                Comment = "Update Comment "
            };
            bool result = service.UpdateCatOrgStructureType(model);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.OvertimeTypeName
                        + " | " + model.Code
                        + " | " + model.OrgStructureID
                        + " | " + model.Comment
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Remove()
        {
            Cat_OrgStructureTypeServices service = new Cat_OrgStructureTypeServices();
            int rs = 0;
            var model = new Cat_OrgStructureType { Id = 3 };
            bool result = service.DeleteCatOrgStructureType(model.Id);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }
    }
}
