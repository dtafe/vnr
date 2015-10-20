using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Category.Model;
using HRM.Business.Category.Domain;

namespace HRM.Business.Category.Domain.NUnitTest
{
    [TestFixture]
    public class Cat_LeaveDayTypeServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Get()
        {
            Cat_LeaveDayTypeServices service = new Cat_LeaveDayTypeServices();
            IQueryable<Cat_LeaveDayType> repo = service.GetCatLeaveDayType();
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_GetById()
        {
            Cat_LeaveDayTypeServices service = new Cat_LeaveDayTypeServices();
            var model = new Cat_LeaveDayType { Id = 3 };
            var result = service.GetByIdCatLeaveDayType(model.Id) as Cat_LeaveDayType;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.Code
                + " | " + result.LeaveDayTypeName
                + " | " + result.PaidRate
                + " | " + result.SocialRate
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Add()
        {
            Cat_LeaveDayTypeServices service = new Cat_LeaveDayTypeServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Cat_LeaveDayType
                {
                    Code = "Add Code " + i,
                    LeaveDayTypeName = "Loai Ngay Nghi "+i,
                    PaidRate = 1,
                    SocialRate = 2
                };
                result = service.AddCatLeaveDayType(model);
                NUnit.Framework.Assert.IsTrue(result);
                if (result == true)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.LeaveDayTypeName
                        + " | " + model.PaidRate
                        + " | " + model.SocialRate
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
            Cat_LeaveDayTypeServices service = new Cat_LeaveDayTypeServices();
            var model = new Cat_LeaveDayType
            {
                Id = 5,
                Code = "Update Code ",
                LeaveDayTypeName = "Update Loai Ngay Nghi ",
                PaidRate = 3,
                SocialRate = 4

            };
            bool result = service.UpdateCatLeaveDayType(model);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.LeaveDayTypeName
                        + " | " + model.PaidRate
                        + " | " + model.SocialRate
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Remove()
        {
            Cat_LeaveDayTypeServices service = new Cat_LeaveDayTypeServices();
            int rs = 0;
            var model = new Cat_LeaveDayType { Id = 3 };
            bool result = service.DeleteCatLeaveDayType(model.Id);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

    }
}
