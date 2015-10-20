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
    public class Hre_RelativesServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Relatives_Domain_Get()
        {
            string status = string.Empty;
            Hre_RelativesServices service = new Hre_RelativesServices();
            var repo = service.GetAllUseEntity<Hre_RelativesEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Relatives_Domain_GetById()
        {
            string status = string.Empty;
            Hre_RelativesServices service = new Hre_RelativesServices();
            var model = new Hre_Relatives { Id = 10 };
            var result = service.GetById<Hre_RelativesEntity>(model.Id, ref status) as Hre_RelativesEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID
                + " | " + result.RelativeName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
//[TestMethod]
        [Test]
        public void NUnit_Relatives_Domain_Add()
        {
            Hre_RelativesServices service = new Hre_RelativesServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_Relatives
                {
                    ProfileID = 1,
                    RelativeName = "RelativesName " + i,
                    DependantID = 1,
                    IsColleaggue = true
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.RelativeName
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Relatives_Domain_Edit()
        {
            Hre_RelativesServices service = new Hre_RelativesServices();
            var model = new Hre_Relatives
            {
                Id =10,
                ProfileID = 1,
                RelativeName = "Relatives Name ",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.RelativeName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Relatives_Domain_Remove()
        {
            Hre_RelativesServices service = new Hre_RelativesServices();
            int rs = 0;
            var model = new Hre_Relatives { Id = 10 };
            string result = service.Remove<Hre_RelativesEntity>(model.Id);
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
        public void NUnit_Relatives_Domain_Delete()
        {
            Hre_RelativesServices service = new Hre_RelativesServices();
            int rs = 0;
            var model = new Hre_Relatives { Id = 3 };
            string result = service.Delete<Hre_RelativesEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
