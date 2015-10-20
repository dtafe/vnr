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
    public class Att_RosterGroupServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_RosterGroup_Domain_Get()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            string status = string.Empty;
            var repo = service.GetAllUseEntity<Att_RosterGroupEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_RosterGroup_Domain_GetById()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            var model = new Att_RosterGroup { Id = 1 };
            string status = string.Empty;
            var result = service.GetById<Att_RosterGroupEntity>(model.Id, ref status) as Att_RosterGroupEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.RosterGroupName
              
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_RosterGroup_Domain_Add()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_RosterGroup
                {
                    Comment = "Comment : "+ 1
                };
                result = service.Add(model);
                if (result != string.Empty)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_RosterGroup_Domain_Edit()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            var model = new Att_RosterGroup
            {
                Id = 1
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.MonShiftID
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_RosterGroup_Domain_Remove()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            int rs = 0;
            var model = new Att_RosterGroup { Id = 1 };
            string result = service.Remove<Att_RosterGroupEntity>(model.Id);
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
        public void NUnit_RosterGroup_Domain_Delete()
        {
            Att_RosterGroupServices service = new Att_RosterGroupServices();
            int rs = 0;
            var model = new Att_RosterGroup { Id = 3 };
            string result = service.Delete<Att_RosterGroupEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
