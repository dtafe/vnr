using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_HDTJobServices_NUnitTest
    {
       
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_HDTJob_Domain_Get()
        {
            string status = string.Empty;
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            var repo = service.GetAllUseEntity<Hre_HDTJobEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_HDTJob_Domain_GetById()
        {
            string status = string.Empty;
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            var model = new Hre_HDTJob { Id =5 };
            var result = service.GetById<Hre_HDTJobEntity>(model.Id, ref status) as Hre_HDTJobEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID 
                + " | " + result.DateFrom 
                + " | " + result.DateTo
                + " | " + result.HDTType
                + " | " + result.Description
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_HDTJob_Domain_Add()
        {
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_HDTJob
                {
                    ProfileID=1,
                    DateFrom = DateTime.Now,
                    DateTo = DateTime.Now,
                    HDTType = "HDTType " + i,
                    Description = "Description",
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.DateFrom
                        + " | " + model.DateTo
                        + " | " + model.HDTType
                        + " | " + model.Description
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_HDTJob_Domain_Edit()
        {
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            var model = new Hre_HDTJob
            {
                Id = 3,
                ProfileID = 1,
                HDTType = "Update HDTType",
                Description = "Update Description",
                DateFrom = DateTime.Parse("2014/04/03")

            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.DateFrom
                        + " | " + model.DateTo
                        + " | " + model.HDTType
                        + " | " + model.Description
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_HDTJob_Domain_Remove()
        {
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            int rs = 0;
            var model = new Hre_HDTJob { Id = 5};
            string result = service.Remove<Hre_HDTJobEntity>(model.Id);
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
        public void NUnit_HDTJob_Domain_Delete()
        {
            Hre_HDTJobServices service = new Hre_HDTJobServices();
            int rs = 0;
            var model = new Hre_HDTJob { Id = 5 };
            string result = service.Delete<Hre_HDTJobEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
