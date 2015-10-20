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
    public class Hre_DisciplineServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Discipline_Domain_Get()
        {
            string status = string.Empty;
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            var repo = service.GetAllUseEntity<Hre_DisciplineEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Discipline_Domain_GetById()
        {
            string status = string.Empty;
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            var model = new Hre_Discipline { Id = 10 };
            var result = service.GetById<Hre_DisciplineEntity>(model.Id, ref status) as Hre_DisciplineEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID
                + " | " + result.ViolationRule 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
//[TestMethod]
        [Test]
        public void NUnit_Discipline_Domain_Add()
        {
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Discipline
                {
                    ProfileID = 1,
                    ViolationRule = "ViolationRule " + i,
                    DateOfEffective = DateTime.Now,
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.ViolationRule
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Discipline_Domain_Edit()
        {
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            var model = new Hre_Discipline
            {
                Id =10,
                ProfileID = 1,
                ViolationRule = "ViolationRule",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.ViolationRule
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Discipline_Domain_Remove()
        {
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            int rs = 0;
            var model = new Hre_Discipline { Id = 10 };
            string result = service.Remove<Hre_DisciplineEntity>(model.Id);
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
        public void NUnit_Discipline_Domain_Delete()
        {
            Hre_DisciplineServices service = new Hre_DisciplineServices();
            int rs = 0;
            var model = new Hre_Discipline { Id = 3 };
            string result = service.Delete<Hre_DisciplineEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
