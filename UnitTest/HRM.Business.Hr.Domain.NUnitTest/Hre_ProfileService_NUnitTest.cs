using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HRM.Data.Hr.Model;
using NUnit.Framework;
using HRM.Business.Hr.Models;

namespace HRM.Business.Hr.Domain.NUnitTest
{
    [TestFixture]
    public class Hre_ProfileService_NUnitTest
    {
        [Test]
        public void GetProfle()
        {
            string status = string.Empty;
            Hre_ProfileServices service = new Hre_ProfileServices();
            var repo = service.GetAllUseEntity<Hre_ProfileEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());

        }
        [Test]
        public void AddProfleNoneReference()
        {
            for (int i = 21; i < 10000; i++)
            {
                var model = new Hre_Profile
                {
                    ProfileName = " Văn Thiên Hiếu 00" + i.ToString(),
                    LastName = "Hiếu 00" + i.ToString(),
                    FirstName = "Văn Thiên",
                    NameEnglish = "Kenptk 00" + i.ToString(),
                    ImagePath = "",
                    CodeEmp = "NV-IT-00" + i.ToString(),
                    CodeTax = "MST-NV-00" + i.ToString(),
                    CodeAttendance = "MTT-00" + i.ToString(),
                    DateHire = DateTime.Now,
                    DateEndProbation = DateTime.Now,
                    DateOfEffect = DateTime.Now,
                    WorkingPlace = "TP. Hồ Chí Minh",
                    OrgStructureID = 2,
                    JobTitleID = 4,
                    PositionID = 2,
                    EmpTypeID = 1,
                    Gender = "E_MALE",
                    MarriageStatus = "E_SINGLE",
                    DayOfBirth = 9,
                    MonthOfBirth = 7,
                    YearOfBirth = 1991,
                    PlaceOfBirth = "QT",
                    BloodType = "O",
                    Height = 170,
                    Weight = 59,
                    IDNo = "2014" + i,
                    IDDateOfIssue = DateTime.Now,
                    IDPlaceOfIssue = "TP. Hồ Chí Minh",
                    PassportNo = "20140100" + i,
                    Email = "hieu.van@vnresource.vn",
                    CellPhone = "01207666444",
                    Notes = "Công ty VnResource, test chương trình quản lý nhân sự version 8.0 đang trong quá trình phát triển và xây dựng  năm 2014",
                };
                var service = new Hre_ProfileServices();
                var repo = false;
                if (service.Add(model)=="0")
                {
                    repo = true;
                }
                NUnit.Framework.Assert.IsTrue(repo);
                Console.Write("Record: " + model.ProfileName);
            }
        }
        [Test]
        public void AddProfle()
        {
            for (int i = 0; i < 20; i++)
            {
                var model = new Hre_Profile
                {
                    ProfileName = "Nguyễn Ngọc Chúc "+i,
                    LastName = "Công ty VnResource",
                    FirstName = "Công ty VnResource",
                    NameEnglish = "Công ty VnResource",
                    ImagePath = "andrew.jpg",
                    CodeEmp = "NV00000"+i,
                    CodeTax = "T00000"+i,
                    CodeAttendance = "A00000"+i,
                    DateHire = DateTime.Now,
                    DateEndProbation = DateTime.Now,
                    OrgStructureID = 3,
                    PositionID = 1,
                    DateOfEffect = DateTime.Now,
                    CostCentreID = 1,
                    WorkingPlace = "TP. Hồ Chí Minh",
                    Gender = "male",
                    DayOfBirth = 10,
                    MonthOfBirth = 11,
                    YearOfBirth = 2014,
                    PlaceOfBirth = "TP. Hồ Chí Minh",
                    NationalityID = 1,
                    EthnicID = 1,
                    ReligionID = 1,
                    BloodType = "o",
                    Height = 170,
                    Weight = 59,
                    IDNo = "24110128"+i,
                    IDDateOfIssue = DateTime.Now,
                    IDPlaceOfIssue = "TP. Hồ Chí Minh",
                    PassportNo = "20001210"+i,
                    PassportDateOfExpiry = DateTime.Now,
                    PassportDateOfIssue = DateTime.Now,
                    PassportPlaceOfIssue = "TP. Hồ Chí Minh",
                    Email = "chuc.nguyen@vnresource.vn",
                    CellPhone = "0989199212",
                    HomePhone = "09121212",
                    BusinessPhone = "38006223",
                    PAddressID = 1,
                    TAddressID = 1,
                    JobTitleID = 1,
                    EmpTypeID = 1,
                    SupervisiorID = 1,
                    HighSupervisiorID = 1,
                    DateQuit = DateTime.Now,
                    Notes = "Công ty VnResource, test chương trình quản lý nhân sự version 8.0 đang trong quá trình phát triển và xây dựng  năm 2014",

                };
                var service = new Hre_ProfileServices();
                var repo = false;
                if (service.Add(model) == "0")
                {
                    repo = true;
                }
                NUnit.Framework.Assert.IsTrue(repo);
                Console.Write("Record: " + repo);
            }
        }

        [Test]
        public void EditProfle()
        {
            var model = new Hre_Profile
            {
                Id = 1,
                ProfileName = "Nguyễn Ngọc Chúc Test Edit",
                DateHire = DateTime.Now,
                OrgStructureID = 3,
                PositionID = 1,
                DateOfEffect = DateTime.Now,
                CostCentreID = 1,
                NationalityID = 1,
                EthnicID = 1,
                ReligionID = 1,
                JobTitleID = 1,
                EmpTypeID = 1,
                SupervisiorID = 1,
                HighSupervisiorID = 1,
                DateQuit = DateTime.Now

            };
            var service = new Hre_ProfileServices();
            var repo = service.Edit(model);
            Console.Write("Result: " + repo);

        }

        [Test]
        public void DeleteProfle()
        {
            const int id = 1;
            var service = new Hre_ProfileServices();
            var repo = service.Delete<Hre_ProfileEntity>(id);
            Console.Write("Result: " + repo);
        }

        [Test]
        public void RemoveProfle()
        {
            const int id = 2;
            var service = new Hre_ProfileServices();
            var repo = service.Remove<Hre_ProfileEntity>(id);
            Console.Write("Result: " + repo);


        }

    }
}
