using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HRM.Data.Hr.Model;
using NUnit.Framework;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_RewardService_NUnitTest
    {
        [Test]
        public void GetReward()
        {
            string status = string.Empty;
            var service = new Hre_RewardServices();
            var repo = service.GetAllUseEntity<Hre_RewardEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());

        }

        [Test]
        public void AddReward()
        {
            for (int i = 0; i < 10000; i++)
            {
                var model = new Hre_Reward
                {
                    DateOfIssuance = DateTime.Now,
                    Reason = "T00000" + i,
                    RewardedTitleID = 1,
                    RewardedDecidingOrgID = 1,
                    RewardedTimeID = 1,
                    DateOfEffective = DateTime.Now,
                    Description = "T00000" + i,
                    UserApproveID = 1,
                    Requester = "T00000" + i,
                    RequesterID = 1,
                    RewardValue = 3,
                    NoOfReward = "T00000" + i,
                    RewardedTypeID = 1,
                    Attachment = "T00000" + i,
                   

                };
                var service = new Hre_RewardServices();
                var repo = service.Add(model);
                Console.Write("Record: " + repo);
            }
        }

        [Test]
        public void EditProfle()
        {
            var model = new Hre_Reward
            {
                DateOfIssuance = DateTime.Now,
                Reason = "T00000 test",
                RewardedTitleID = 2,
                RewardedDecidingOrgID = 2,
                RewardedTimeID = 2,
                DateOfEffective = DateTime.Now,
                Description = "T00000 test",
                UserApproveID = 2,
                Requester = "T00000 test",
                RequesterID = 2,
                RewardValue = 2,
                NoOfReward = "T00000 test",
                RewardedTypeID = 1,
                Attachment = "T00000 test",

            };
            var service = new Hre_RewardServices();
            var repo = service.Edit(model);
            Console.Write("Result: " + repo);

        }

        [Test]
        public void DeleteReward()
        {
            const int id = 1;
            var service = new Hre_RewardServices();
            var repo = service.Delete<Hre_RewardEntity>(id);
            Console.Write("Result: " + repo);
        }

        [Test]
        public void RemoveReward()
        {
            const int id = 2;
            var service = new Hre_RewardServices();
            var repo = service.Remove<Hre_RewardEntity>(id);
            Console.Write("Result: " + repo);


        }

    }
}
