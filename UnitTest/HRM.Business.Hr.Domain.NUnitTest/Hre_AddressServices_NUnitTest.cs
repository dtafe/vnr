using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_AddressServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_Get()
        {
            Hre_AddressServices service = new Hre_AddressServices();
            var status = string.Empty;
            var repo = service.GetAllUseEntity<Hre_Address>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_GetById()
        {
            string status = string.Empty;
            Hre_AddressServices service = new Hre_AddressServices();
            var model = new Hre_Address { Id = 10 };
            var result = service.GetById<Hre_AddressEntity>(model.Id, ref status) as Hre_AddressEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.AddressName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_Add()
        {
            Hre_AddressServices service = new Hre_AddressServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Address
                {
                    AddressName = "AddressName " + i
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.AddressName
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_Edit()
        {
            Hre_AddressServices service = new Hre_AddressServices();
            var model = new Hre_Address
            {
                Id =10,
                AddressName = "Address Name ",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.AddressName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_Remove()
        {
            Hre_AddressServices service = new Hre_AddressServices();
            int rs = 0;
            var model = new Hre_Address { Id = 10 };
            string result = service.Remove<Hre_AddressEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_Address_Domain_Delete()
        {
            Hre_AddressServices service = new Hre_AddressServices();
            int rs = 0;
            var model = new Hre_Address { Id = 3 };
            string result = service.Delete<Hre_AddressEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
