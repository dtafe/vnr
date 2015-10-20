using System;
using System.Linq;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Data.Attendance.Model;
using NUnit.Framework;

namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestFixture]
    public class Att_LeavedayServices_NUnitTest
    {
        public string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        //[Test]
        //public void NUnit_Leaveday_Domain_Get()
        //{
        //    Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
        //    var repo = service.GetAnnualLeaves();
        //    // IQueryable<Att_Leaveday> repo = service.Get();
        //    Console.Write("Total Record: " + repo.Count());
        //}

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Leaveday_Domain_GetById()
        {
            Att_LeavedayServices service = new Att_LeavedayServices();
            var model = new Att_Leaveday { Id = 3 };
            var result = service.GetById<Att_Leaveday>(model.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Leaveday_Domain_Add()
        {
            Att_LeavedayServices service = new Att_LeavedayServices();
            var result = "";
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_Leaveday
                {
                    ProfileID = 1,
                    Comment = "commnent " + i
                };
                result = service.Add(model);
                NUnit.Framework.Assert.IsNull(result);
                if (result != null)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.ProfileID
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Leaveday_Domain_Edit()
        {
            Att_LeavedayServices service = new Att_LeavedayServices();
            var model = new Att_Leaveday
            {
                Id = 1,
                ProfileID = 1,
                Comment = "Update Name ",
            };
            var result = service.Edit(model);
            NUnit.Framework.Assert.IsNull(result);
            if (result != null)
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
        public void NUnit_Leaveday_Domain_Remove()
        {
            Att_LeavedayServices service = new Att_LeavedayServices();
            int rs = 0;
            var model = new Att_Leaveday { Id = 3 };
            var result = service.Remove<Att_Leaveday>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_Leaveday_Domain_Delete()
        {
            Att_LeavedayServices service = new Att_LeavedayServices();
            int rs = 0;
            var model = new Att_Leaveday { Id = 3 };
            var result = service.Delete<Att_Leaveday>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }


    #region Test Unit Visual Studio
    
    //[TestClass]
    //public class Att_LeavedayServices_UnitTest
    //{
    //    [TestMethod]
    //    public void Unit_Leaveday_Domain_Get()
    //    {
    //        var service = new Att_AnnualLeaveServices();
    //        var repo = service.GetAnnualLeaves();
    //        Console.Write("Total Record: " + repo.Count());
    //    }

    //    [TestMethod]
    //    public void Unit_OverTime_Domain_GetAll()
    //    {
    //        //var service = new Att_OvertimeServices();
    //        //var repo = service.Get();
    //        //Console.Write("Total Record: " + repo.Count());
    //    }

    //}

    #endregion

}
