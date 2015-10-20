using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.Hr.Model;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_WorkHistoryServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_WorkHistory_Domain_Get()
        {
            string status = string.Empty;
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            var repo = service.GetAllUseEntity<Hre_WorkHistoryEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_WorkHistory_Domain_GetById()
        {
            string status = string.Empty;
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            var model = new Hre_WorkHistory { Id = 60 };
            var result = service.GetById<Hre_WorkHistoryEntity>(model.Id, ref status) as Hre_WorkHistoryEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.ProfileID 
                + " | " + result.PositionID
                + " | " + result.WorkLocation
                + " | " + result.JobTitleID
                + " | " + result.DateEffective
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_WorkHistory_Domain_Add()
        {
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Hre_WorkHistory
                {
                    ProfileID = 1,
                    PositionID = 1,
                    WorkLocation = "Location "+ i,
                    JobTitleID = 2,
                    DateEffective = DateTime.Parse("2014/02/01")
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.PositionID
                        + " | " + model.WorkLocation
                        + " | " + model.JobTitleID
                        + " | " + model.DateEffective
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_WorkHistory_Domain_Edit()
        {
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            var model = new Hre_WorkHistory
            {
                Id = 66,
                ProfileID = 1,
                PositionID = 1,
                WorkLocation = "Update Location ",
                JobTitleID = 2,
                DateEffective = DateTime.Parse("2014/04/03")

            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.PositionID
                        + " | " + model.WorkLocation
                        + " | " + model.JobTitleID
                        + " | " + model.DateEffective
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_WorkHistory_Domain_Remove()
        {
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            int rs = 0;
            var model = new Hre_WorkHistory { Id = 58 };
            string result = service.Remove<Hre_WorkHistoryEntity>(model.Id);
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
        public void NUnit_WorkHistory_Domain_Delete()
        {
            Hre_WorkHistoryServices service = new Hre_WorkHistoryServices();
            int rs = 0;
            var model = new Hre_WorkHistory { Id = 58 };
            string result = service.Delete<Hre_WorkHistoryEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
