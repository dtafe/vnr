using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_ContractServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Get()
        {
            string status = string.Empty;
            Hre_ContractServices service = new Hre_ContractServices();
            var repo = service.GetAllUseEntity<Hre_ContractEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_GetById()
        {
            string status = string.Empty;
            Hre_ContractServices service = new Hre_ContractServices();
            var model = new Hre_Contract { Id = 3 };
            var result = service.GetById<Hre_ContractEntity>(model.Id, ref status) as Hre_ContractEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID 
                + " | " + result.Code 
                + " | " + result.ContractNo
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Add()
        {
            Hre_ContractServices service = new Hre_ContractServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Contract
                {
                    Code = "Code Test " + i,
                    ProfileID = 1,
                    ContractNo = "No " + i,
                    ContractTypeID= 1,
                    DateSigned = DateTime.Parse("2014/02/01"),
                    DateStart = DateTime.Parse("2014/04/03")

                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.ProfileID
                        + " | " + model.ContractNo
                        + " | " + model.ContractTypeID
                        + " | " + model.DateSigned
                        + " | " + model.DateStart
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
            Hre_ContractServices service = new Hre_ContractServices();
            var model = new Hre_Contract
            {
                Id = 5,
                Code = "Update Code Test",
                ProfileID = 1,
                ContractNo = "Update No ",
                ContractTypeID = 1,
                DateSigned = DateTime.Parse("2014/06/05"),
                DateStart = DateTime.Parse("2014/08/07")

            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.ProfileID
                        + " | " + model.ContractNo
                        + " | " + model.ContractTypeID
                        + " | " + model.DateSigned
                        + " | " + model.DateStart
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Contract_Domain_Remove()
        {
            Hre_ContractServices service = new Hre_ContractServices();
            int rs = 0;
            var model = new Hre_Contract { Id = 3 };
            string result = service.Remove<Hre_ContractEntity>(model.Id);
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
        public void NUnit_Contract_Domain_Delete()
        {
            Hre_ContractServices service = new Hre_ContractServices();
            int rs = 0;
            var model = new Hre_Contract { Id = 3 };
            string result = service.Delete<Hre_ContractEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
