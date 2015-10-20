using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_DependantServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Dependant_Domain_Get()
        {
            string status = string.Empty;
            Hre_DependantServices service = new Hre_DependantServices();
            var repo = service.GetAllUseEntity<Hre_DependantEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Dependant_Domain_GetById()
        {
            string status = string.Empty;
            Hre_DependantServices service = new Hre_DependantServices();
            var model = new Hre_Dependant { Id = 3 };
            var result = service.GetById<Hre_DependantEntity>(model.Id, ref status) as Hre_DependantEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID
                + " | " + result.DependantName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Dependant_Domain_Add()
        {
            Hre_DependantServices service = new Hre_DependantServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Dependant
                {
                    ProfileID = 1,
                    DependantName = "Dependant Name " + i
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.DependantName
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Dependant_Domain_Edit()
        {
            Hre_DependantServices service = new Hre_DependantServices();
            var model = new Hre_Dependant
            {
                Id = 1,
                ProfileID = 1,
                DependantName = "Update Name ",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.DependantName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Dependant_Domain_Remove()
        {
            Hre_DependantServices service = new Hre_DependantServices();
            int rs = 0;
            var model = new Hre_Dependant { Id = 3 };
            string result = service.Remove<Hre_DependantEntity>(model.Id);
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
        public void NUnit_Dependant_Domain_Delete()
        {
            Hre_DependantServices service = new Hre_DependantServices();
            int rs = 0;
            var model = new Hre_Dependant { Id = 3 };
            string result = service.Delete<Hre_DependantEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
