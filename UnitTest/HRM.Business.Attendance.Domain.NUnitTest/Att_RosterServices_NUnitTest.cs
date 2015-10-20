using System;
using System.Linq;
using NUnit.Framework;
using HRM.Business.Attendance.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Hr.Domain;
using HRM.Data.Attendance.Model;
using HRM.Business.Attendance.Models;


namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestFixture]
    public class Att_RosterServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Get()
        {
            string status = string.Empty;
            Att_RosterServices service = new Att_RosterServices();
            var repo = service.GetAllUseEntity<Att_RosterEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_GetById()
        {
            Att_RosterServices service = new Att_RosterServices();
            var model = new Att_Roster { Id = 1 };
            string status = string.Empty;
            var result = service.GetById<Att_RosterEntity>(model.Id, ref status) as Att_RosterEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.Comment
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Add()
        {
            Att_RosterServices service = new Att_RosterServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_Roster
                {
                    ProfileID = 1,
                    Comment = "Comment " + i
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Comment
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
            Att_RosterServices service = new Att_RosterServices();
            var model = new Att_Roster
            {
                Id = 1,
                ProfileID = 1,
                Comment = "Update Name ",
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.Comment
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_CardHistory_Domain_Remove()
        {
            Att_RosterServices service = new Att_RosterServices();
            int rs = 0;
            var model = new Att_Roster { Id = 1 };
            string result = service.Remove<Att_RosterEntity>(model.Id);
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
        public void NUnit_CardHistory_Domain_Delete()
        {
            Att_RosterServices service = new Att_RosterServices();
            int rs = 0;
            var model = new Att_Roster { Id = 2 };
            string result = service.Delete<Att_RosterEntity>(model.Id);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
