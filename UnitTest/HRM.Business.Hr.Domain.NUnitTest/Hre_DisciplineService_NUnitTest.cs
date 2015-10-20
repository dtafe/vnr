using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestClass]
    public class Hre_DisciplineService_NUnitTest
    {
        [TestMethod]
        public void GetProfle()
        {
            string status = string.Empty;
            var service = new Hre_DisciplineServices();
            var repo = service.GetAllUseEntity<Hre_DisciplineEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());

        }

        [TestMethod]
        public void AddDiscipline()
        {
            for (int i = 0; i < 10000; i++)
            {
                var model = new Hre_Discipline
                {
                    DateOfEffective = DateTime.Now,
                    DateOfInsurance = DateTime.Now,
                    DecisionNo = "Công ty VnResource",
                    DateOfViolation = DateTime.Now,
                    DescriptionOfViolation = "Nghi nhieu",
                    ContentOfDiscipline = "Khong di lam",
                    DateOfExpiry = DateTime.Now,
                    DisciplinedDecidingOrgID = i,
                    DisciplineTypeID = i,
                    Notes = "Ghi chú",
                    ViolationRule = "ViolationRule",
                    Witnesses = "Witnesses",
                    ViolationRuleDescription = "ViolationRuleDescription",
                    ViolationRuleDescriptionEN = "ViolationRuleDescriptionEN",
                    ResultOfRecidivism = "TP. Hồ Chí Minh",
                    TimeOfViolation = "TimeOfViolation",
                    UserApprovedID = 10,
                    UserHeadID = 11,
                    ViolationExplain = "ViolationExplain",
                    UnionAgentID = i,
                    HeadAgentID = i,
                    ManagementAgentID = i,
                    PlaceViolation = "PlaceViolation",
                    DateEndOfViolation = DateTime.Now,
                    Attachment = "Attachment",
                    PercentDeduction = 59,
                    

                };
                var service = new Hre_DisciplineServices();
                var repo = service.Add(model);
                Console.Write("Record: " + repo);
            }
        }

        [TestMethod]
        public void EditDiscipline()
        {
            var model = new Hre_Discipline
            {
                Id = 1,
                DateOfEffective = DateTime.Now,
                DateOfInsurance = DateTime.Now,
                DecisionNo = "Công ty VnResource Test update",
                DateOfViolation = DateTime.Now,
                DescriptionOfViolation = "Nghi nhieu Test update",
                ContentOfDiscipline = "Khong di lam Test update",
                DateOfExpiry = DateTime.Now,
                DisciplinedDecidingOrgID = 2,
                DisciplineTypeID = 2,
                Notes = "Ghi chú Test update",

            };
            var service = new Hre_DisciplineServices();
            var repo = service.Edit(model);
            Console.Write("Result: " + repo);

        }

        [TestMethod]
        public void DeleteDiscipline()
        {
            const int id = 1;
            var service = new Hre_DisciplineServices();
            var repo = service.Delete<Hre_DisciplineEntity>(id);
            Console.Write("Result: " + repo);
        }

        [TestMethod]
        public void RemoveDiscipline()
        {
            const int id = 2;
            var service = new Hre_DisciplineServices();
            var repo = service.Remove<Hre_DisciplineEntity>(id);
            Console.Write("Result: " + repo);


        }

    }
}
