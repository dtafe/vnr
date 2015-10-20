using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    //[TestClass]
    [TestFixture]
    public class Hre_RewardServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Reward_Domain_Get()
        {
            string status = string.Empty;
            Hre_RewardServices service = new Hre_RewardServices();
            var repo = service.GetAllUseEntity<Hre_RewardEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Reward_Domain_GetById()
        {
            string status = string.Empty;
            Hre_RewardServices service = new Hre_RewardServices();
            var model = new Hre_Reward { Id = 10 };
            var result = service.GetById<Hre_RewardEntity>(model.Id, ref status) as Hre_RewardEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID
                + " | " + result.Reason 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        //[TestMethod]
        [Test]
        public void NUnit_Reward_Domain_Add()
        {
            Hre_RewardServices service = new Hre_RewardServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Reward
                {
                    ProfileID = 1,
                    Reason = "Reason " + i,
                    RequesterID = 1,
                };
                result = service.Add(model);
                if (result != null)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Reason
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Reward_Domain_Edit()
        {
            Hre_RewardServices service = new Hre_RewardServices();
            var model = new Hre_Reward
            {
                Id =10,
                ProfileID = 1,
                Reason = "Reason",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.Reason
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Reward_Domain_Remove()
        {
            Hre_RewardServices service = new Hre_RewardServices();
            int rs = 0;
            var model = new Hre_Reward { Id = 10 };
            string result = service.Remove<Hre_RewardEntity>(model.Id);
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
        public void NUnit_Reward_Domain_Delete()
        {
            Hre_RewardServices service = new Hre_RewardServices();
            int rs = 0;
            var model = new Hre_Reward { Id = 3 };
            string result = service.Delete<Hre_RewardEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
