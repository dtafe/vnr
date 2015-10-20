using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_CardHistoryServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Get()
        {
            string status = string.Empty;
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            var repo = service.GetAllUseEntity<Hre_CardHistoryEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_GetById()
        {
            string status = string.Empty;
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            var model = new Hre_CardHistory { Id = 2 };
            var result = service.GetById<Hre_CardHistoryEntity>(model.Id, ref status) as Hre_CardHistoryEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID
                + " | " + result.CardCode 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Add()
        {
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_CardHistory
                {
                    ProfileID = 1,
                    CardCode = "CardCode " + i
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.CardCode
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Edit()
        {
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            var model = new Hre_CardHistory
            {
                Id = 1,
                ProfileID = 1,
                CardCode = "Update Name ",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.CardCode
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Remove()
        {
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            int rs = 0;
            var model = new Hre_CardHistory { Id = 2 };
            string result = service.Remove<Hre_CardHistoryEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Delete()
        {
            Hre_CardHistoryServices service = new Hre_CardHistoryServices();
            int rs = 0;
            var model = new Hre_CardHistory { Id = 3 };
            string result = service.Delete<Hre_CardHistoryEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
