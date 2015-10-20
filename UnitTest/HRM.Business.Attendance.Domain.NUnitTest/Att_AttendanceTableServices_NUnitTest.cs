using System;
using System.Linq;
using NUnit.Framework;
using HRM.Business.Attendance.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Hr.Domain;
using HRM.Data.Attendance.Model;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using HRM.Business.Category.Models;

namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestClass]
    public class Att_AttendanceTableServices_NUnitTest
    {
        /// <summary>
        /// Tính Công
        /// </summary>
        [TestMethod]
        public void NUnit_AttendanceTable_Domain_ComputeAttendance()
        {
            int userID = 1;
            int taskID = 0;
            DateTime monthYear = DateTime.Parse("01-02-2014");
            int cutOffDurationID = 5;
            List<int> listDepartmentID = new List<int>(){3};
            List<int> listPayrollGroupID = new List<int>(){0};

            Att_AttendanceTableServices service = new Att_AttendanceTableServices();
            List<Att_AttendanceTable> ListAttendanceTable = service.ComputeAttendance(userID, taskID, monthYear, cutOffDurationID, listDepartmentID, listPayrollGroupID);
        }
    }
}
